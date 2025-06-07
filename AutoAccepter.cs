using System.Net.Http.Headers;
using System.Text;

public static class AutoAccepter
{
    public static async Task StartAsync(CancellationToken ct, AppConfig config)
    {
        string lockfilePath = Path.Combine(config.LeagueOfLegendsDirectory, "lockfile");
        string? lastLockfileContent = null;

        while (!ct.IsCancellationRequested)
        {
            try
            {
                if (!File.Exists(lockfilePath))
                {
                    await Task.Delay(5000, ct);
                    continue;
                }

                string[] parts;
                string? content = ReadLockfileContent(lockfilePath);
                if (content == null)
                {
                    Logger.Write("lockfile の読み取りに失敗しました。");
                    await Task.Delay(5000, ct);
                    continue;
                }

                if (lastLockfileContent == content)
                {
                    await Task.Delay(5000, ct);
                    continue;
                }

                lastLockfileContent = content;
                parts = content.Split(':');

                if (parts.Length < 5)
                {
                    Logger.Write("lockfile の形式が不正です。");
                    await Task.Delay(5000, ct);
                    continue;
                }

                string port = parts[2];
                string token = parts[3];
                string protocol = parts[4];

                string baseUrl = $"{protocol}://127.0.0.1:{port}";
                string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));

                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var client = new HttpClient(handler);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

                Logger.Write($"接続成功: {baseUrl}");
                bool accepted = false;

                while (!ct.IsCancellationRequested)
                {
                    string? currentLockfile = ReadLockfileContent(lockfilePath);
                    if (currentLockfile == null || currentLockfile != lastLockfileContent)
                    {
                        Logger.Write("lockfile が変化しました。再接続を試みます。");
                        break;
                    }

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync($"{baseUrl}/lol-matchmaking/v1/ready-check", ct);
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
                }
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Logger.Write("例外発生: " + ex.Message);
                await Task.Delay(5000, ct);
            }
        }
    }

    private static string? ReadLockfileContent(string path)
    {
        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs);
            return reader.ReadToEnd();
        }
        catch
        {
            return null;
        }
    }
}
