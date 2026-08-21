using LoLAutoAccepter.Models;
using LoLAutoAccepter.Services;
using LoLAutoAccepter.Utilities;
using System.Text.Json;

/// <summary>
/// マッチングの自動承諾を行うクラス
/// </summary>
public static class AutoAccepter
{
    /// <summary>
    /// セッションを開始し自動承諾を実行する
    /// </summary>
    public static async Task RunSessionAsync(CancellationToken ct, AppConfig config, string lockfileContent)
    {
        try
        {
            if (!TryParseLockfile(lockfileContent, out var baseUrl, out var auth)) return;

            using var client = LcuApiClient.Create(auth);

            Logger.Write($"接続成功: {baseUrl}");

            if (!await WaitForApiReadyAsync(client, baseUrl, ct)) return;

            await MonitorAndAcceptAsync(client, baseUrl, config, ct);
        }
        catch (Exception ex)
        {
            Logger.Write($"例外発生: {ex.Message}");
        }
    }

    /// <summary>
    /// lockfile を解析してベース URL と認証情報を取得する
    /// </summary>
    private static bool TryParseLockfile(string lockfileContent, out string baseUrl, out string auth)
    {
        if (!LockfileParser.TryParse(lockfileContent, out baseUrl, out auth))
        {
            Logger.Write("lockfile の形式が不正です。");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 内部APIが利用可能になるまで待機する
    /// </summary>
    private static async Task<bool> WaitForApiReadyAsync(HttpClient client, string baseUrl, CancellationToken ct)
    {
        for (int i = 0; i < 60; i++)
        {
            if (ct.IsCancellationRequested) return false;
            if (await IsApiReadyAsync(client, baseUrl, ct))
            {
                Logger.Write("内部APIが起動しました。セッションを開始します。");
                await LogSummonerNameAsync(client, baseUrl, ct);
                return true;
            }
            await Task.Delay(500, ct);
        }

        Logger.Write("内部APIの起動待ちがタイムアウトしました。セッションを終了します。");
        return false;
    }

    /// <summary>
    /// 内部APIの稼働を確認する
    /// </summary>
    private static async Task<bool> IsApiReadyAsync(HttpClient client, string baseUrl, CancellationToken ct)
    {
        try
        {
            var res = await client.GetAsync($"{baseUrl}/lol-platform-config/v1/namespaces", ct);
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        return false;
    }

    /// <summary>
    /// マッチングを監視し、必要に応じて承諾や自動処理を実行する
    /// </summary>
    private static async Task MonitorAndAcceptAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        bool accepted = false;
        string lockfilePath = Path.Combine(config.LeagueOfLegendsDirectory, "lockfile");

        while (!ct.IsCancellationRequested)
        {
            if (!File.Exists(lockfilePath))
            {
                Logger.Write("lockfileが削除されたため、セッションを終了します。");
                break;
            }

            try
            {
                accepted = await TryAcceptMatchAsync(client, baseUrl, config, ct) || accepted;

                if (accepted && config.AutoCloseOnAccept)
                {
                    Logger.Write("設定によりアプリを自動終了します。");
                    Application.Exit();
                }

                if (config.AutoBanEnabled)
                {
                    await AutoBanner.RunAsync(client, baseUrl, config, ct);
                }

                if (config.AutoPickEnabled)
                {
                    await AutoPicker.RunAsync(client, baseUrl, config, ct);
                }

                await Task.Delay(1000, ct);
            }
            catch (HttpRequestException ex)
            {
                Logger.Write($"接続エラー: {ex.Message}");
                break;
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Logger.Write($"監視ループ中の例外: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// マッチング検出時に承諾を試みる（成功なら true）
    /// </summary>
    private static async Task<bool> TryAcceptMatchAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        var state = await GetReadyCheckStateAsync(client, baseUrl, ct);
        if (state == null) return false;

        if (state == "Accepted")
        {
            return true;
        }
        if (state == "InProgress" && config.AutoAcceptEnabled)
        {
            await Task.Delay(config.AcceptDelaySeconds * 1000, ct);

            var checkState = await GetReadyCheckStateAsync(client, baseUrl, ct);
            if (checkState == "InProgress")
            {
                await client.PostAsync($"{baseUrl}/lol-matchmaking/v1/ready-check/accept", null, ct);
                Logger.Write("マッチ承諾を送信しました。");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ready-check の state を取得する
    /// </summary>
    private static async Task<string?> GetReadyCheckStateAsync(HttpClient client, string baseUrl, CancellationToken ct)
    {
        try
        {
            var response = await client.GetAsync($"{baseUrl}/lol-matchmaking/v1/ready-check", ct);
            if (!response.IsSuccessStatusCode) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseText);
            var root = doc.RootElement;
            if (root.TryGetProperty("state", out var stateProp))
            {
                return stateProp.GetString();
            }
        }
        catch
        {
        }
        return null;
    }

    /// <summary>
    /// サモナー名をログに出力する（最大3回試行）
    /// </summary>
    private static async Task LogSummonerNameAsync(HttpClient client, string baseUrl, CancellationToken ct)
    {
        const int maxAttempts = 3;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var summonerRes = await client.GetAsync($"{baseUrl}/lol-summoner/v1/current-summoner", ct);
                if (!summonerRes.IsSuccessStatusCode)
                {
                    if (attempt == maxAttempts)
                    {
                        Logger.Write($"アカウント名取得失敗: {summonerRes.StatusCode}");
                    }
                    await Task.Delay(500, ct);
                    continue;
                }

                var json = await summonerRes.Content.ReadAsStringAsync(ct);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("gameName", out var gameNameProp) &&
                    root.TryGetProperty("tagLine", out var tagLineProp))
                {
                    var gameName = gameNameProp.GetString();
                    var tagLine = tagLineProp.GetString();
                    if (!string.IsNullOrEmpty(gameName) && !string.IsNullOrEmpty(tagLine))
                    {
                        Logger.Write($"ログイン中のアカウント名: {gameName}#{tagLine}");
                        return;
                    }
                }

                if (attempt == maxAttempts)
                {
                    Logger.Write("アカウント名が取得できませんでした。");
                }
                else
                {
                    await Task.Delay(500, ct);
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                if (attempt == maxAttempts)
                    Logger.Write($"アカウント名取得時に例外: {ex.Message}");
                await Task.Delay(500, ct);
            }
        }
    }
}