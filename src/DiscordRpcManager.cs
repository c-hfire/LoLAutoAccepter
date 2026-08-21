using DiscordRPC;

/// <summary>
/// Discord Rich Presence の管理
/// </summary>
public static class DiscordRpcManager
{
    /// <summary>
    /// Discord RPC クライアント
    /// </summary>
    private static DiscordRpcClient? client;

    /// <summary>
    /// Discord RPC を初期化する
    /// </summary>
    public static void Initialize()
    {
        if (client == null)
        {
            client = new DiscordRpcClient("1381577693862039562");
            client.Initialize();
        }
    }

    /// <summary>
    /// プレゼンスを設定する
    /// </summary>
    public static void SetPresence()
    {
        if (client == null) return;
        client.SetPresence(new RichPresence()
        {
            Buttons = new[]
            {
                new DiscordRPC.Button
                {
                    Label = "最新リリースを確認",
                    Url = "https://github.com/c-hfire/LoLAutoAccepter/releases/latest"
                }
            }
        });
    }

    /// <summary>
    /// Discord RPC をシャットダウンする
    /// </summary>
    public static void Shutdown()
    {
        if (client == null) return;
        client.Dispose();
        client = null;
    }
}