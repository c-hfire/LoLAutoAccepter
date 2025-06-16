using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

/// <summary>
/// マッチングの自動承諾処理を行うクラス
/// </summary>
public static class AutoAccepter
{
    /// <summary>
    /// セッションを開始し、マッチングを自動で承諾します。
    /// </summary>
    /// <param name="ct">キャンセルトークン</param>
    /// <param name="config">アプリ設定</param>
    /// <param name="lockfileContent">lockfile の内容</param>
    public static async Task RunSessionAsync(CancellationToken ct, AppConfig config, string lockfileContent)
    {
        try
        {
            if (!TryParseLockfile(lockfileContent, out var baseUrl, out var auth))
            {
                Logger.Write("lockfile の形式が不正です。");
                return;
            }

            using var client = CreateHttpClient(auth);

            Logger.Write($"接続成功: {baseUrl}");
            await LogSummonerNameAsync(client, baseUrl, ct);

            if (!await WaitForApiReadyAsync(client, baseUrl, ct))
            {
                Logger.Write("内部APIが30秒以内に起動しませんでした。");
                return;
            }

            await MonitorAndAcceptAsync(client, baseUrl, config, ct);
        }
        catch (Exception ex)
        {
            Logger.Write($"例外発生: {ex.Message}");
        }
    }

    /// <summary>
    /// lockfile の内容を解析し、APIのベースURLと認証情報を取得します。
    /// </summary>
    /// <param name="lockfileContent">lockfile の内容</param>
    /// <param name="baseUrl">APIのベースURL</param>
    /// <param name="auth">認証情報</param>
    /// <returns>解析に成功した場合は true</returns>
    private static bool TryParseLockfile(string lockfileContent, out string baseUrl, out string auth)
    {
        baseUrl = string.Empty;
        auth = string.Empty;
        var parts = lockfileContent.Split(':');
        if (parts.Length < 5) return false;

        string port = parts[2];
        string token = parts[3];
        string protocol = parts[4].Trim();

        baseUrl = $"{protocol}://127.0.0.1:{port}";
        auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));
        return true;
    }

    /// <summary>
    /// 認証情報付きの HttpClient を作成します。
    /// </summary>
    /// <param name="auth">認証情報</param>
    /// <returns>HttpClient インスタンス</returns>
    private static HttpClient CreateHttpClient(string auth)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        var client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        return client;
    }

    /// <summary>
    /// 内部APIが起動するまで待機します。
    /// </summary>
    /// <param name="client">HttpClient</param>
    /// <param name="baseUrl">APIのベースURL</param>
    /// <param name="ct">キャンセルトークン</param>
    /// <returns>APIが起動した場合は true</returns>
    private static async Task<bool> WaitForApiReadyAsync(HttpClient client, string baseUrl, CancellationToken ct)
    {
        for (int i = 0; i < 60; i++)
        {
            if (ct.IsCancellationRequested) return false;
            try
            {
                var res = await client.GetAsync($"{baseUrl}/lol-platform-config/v1/namespaces", ct);
                if (res.IsSuccessStatusCode)
                {
                    Logger.Write("内部APIが起動しました。セッションを開始します。");
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Write($"API起動待ち中の接続エラー: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return false;
            }
            await Task.Delay(500, ct);
        }
        return false;
    }

    /// <summary>
    /// マッチング状態を監視し、必要に応じて自動承諾を行います。
    /// </summary>
    /// <param name="client">HttpClient</param>
    /// <param name="baseUrl">APIのベースURL</param>
    /// <param name="config">アプリ設定</param>
    /// <param name="ct">キャンセルトークン</param>
    private static async Task MonitorAndAcceptAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        bool accepted = false;
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/lol-matchmaking/v1/ready-check", ct);
                if (!response.IsSuccessStatusCode)
                {
                    await Task.Delay(1000, ct);
                    continue;
                }

                string responseText = await response.Content.ReadAsStringAsync();

                if (responseText.Contains("InProgress"))
                {
                    if (!accepted)
                    {
                        Logger.Write($"マッチング検出。{config.AcceptDelaySeconds}秒後に承諾します。");
                        await Task.Delay(config.AcceptDelaySeconds * 1000, ct);
                        await client.PostAsync($"{baseUrl}/lol-matchmaking/v1/ready-check/accept", null, ct);
                        Logger.Write("マッチ承諾を送信しました。");
                        accepted = true;

                        if (config.AutoCloseOnAccept)
                        {
                            Logger.Write("設定によりアプリを自動終了します。");
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    accepted = false;
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
    /// サモナー名をログに出力します。
    /// </summary>
    /// <param name="client">HttpClient</param>
    /// <param name="baseUrl">APIのベースURL</param>
    /// <param name="ct">キャンセルトークン</param>
    private static async Task LogSummonerNameAsync(HttpClient client, string baseUrl, CancellationToken ct)
    {
        try
        {
            var summonerRes = await client.GetAsync($"{baseUrl}/lol-summoner/v1/current-summoner", ct);
            if (!summonerRes.IsSuccessStatusCode)
            {
                Logger.Write($"アカウント名取得失敗: {summonerRes.StatusCode}");
                return;
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
                }
                else
                {
                    Logger.Write("アカウント名が取得できませんでした。");
                }
            }
            else
            {
                Logger.Write("アカウント名が取得できませんでした。");
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"アカウント名取得失敗: {ex.Message}");
        }
    }
}