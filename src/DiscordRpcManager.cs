using DiscordRPC;

/// <summary>
/// Discord Rich Presence の管理を行う静的クラス
/// </summary>
public static class DiscordRpcManager
{
    /// <summary>
    /// Discord RPC クライアントのインスタンス
    /// </summary>
    private static DiscordRpcClient? client;

    /// <summary>
    /// Discord RPC を初期化します。
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
    /// Discord のプレゼンス情報を設定します。
    /// </summary>
    public static void SetPresence()
    {
        if (client == null) return;
        client.SetPresence(new RichPresence()
        {
            Buttons =
            [
                new DiscordRPC.Button
                {
                    Label = "アプリを入手",
                    Url = "https://github.com/c-hfire/LoLAutoAccepter/releases/latest"
                }
            ]
        });
    }

    /// <summary>
    /// Discord RPC をシャットダウンします。
    /// </summary>
    public static void Shutdown()
    {
        if (client == null) return;
        client.Dispose();
        client = null;
    }
}