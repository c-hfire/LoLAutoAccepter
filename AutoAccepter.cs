using System.Net.Http.Headers;
using System.Text;

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
            // lockfile から接続情報を取得
            string[] parts = lockfileContent.Split(':');
            if (parts.Length < 5)
            {
                Logger.Write("lockfile の形式が不正です。");
                return;
            }

            string port = parts[2];
            string token = parts[3];
            string protocol = parts[4].Trim(); // 改行や空白を除去

            string baseUrl = $"{protocol}://127.0.0.1:{port}";
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

            Logger.Write($"接続成功: {baseUrl}");

            // 内部APIの起動待ち
            bool isApiReady = false;
            for (int i = 0; i < 60; i++)
            {
                if (ct.IsCancellationRequested) return;

                try
                {
                    var res = await client.GetAsync($"{baseUrl}/lol-platform-config/v1/namespaces", ct);
                    if (res.IsSuccessStatusCode)
                    {
                        isApiReady = true;
                        Logger.Write("内部APIが起動しました。セッションを開始します。");
                        break;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Logger.Write($"API起動待ち中の接続エラー: {ex.Message}");
                }
                catch (TaskCanceledException)
                {
                    // キャンセル時は即終了
                    return;
                }
                await Task.Delay(500, ct);
            }

            if (!isApiReady)
            {
                Logger.Write("内部APIが30秒以内に起動しませんでした。再試行します。");
                return;
            }

            bool accepted = false;

            // マッチング状態を監視し、承諾処理を行う
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/lol-matchmaking/v1/ready-check", ct);
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
                    // キャンセル時は即終了
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Write($"監視ループ中の例外: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"例外発生: {ex.Message}");
        }
    }
}