using DiscordRPC;

/// <summary>
/// Discord Rich Presence �̊Ǘ����s���ÓI�N���X
/// </summary>
public static class DiscordRpcManager
{
    /// <summary>
    /// Discord RPC �N���C�A���g�̃C���X�^���X
    /// </summary>
    private static DiscordRpcClient client = null!;

    /// <summary>
    /// Discord RPC �����������܂��B
    /// </summary>
    public static void Initialize()
    {
        client = new DiscordRpcClient("1381577693862039562");
        client.Initialize();
    }

    /// <summary>
    /// Discord �̃v���[���X����ݒ肵�܂��B
    /// </summary>
    public static void SetPresence()
    {
        client.SetPresence(new RichPresence()
        {
            Buttons =
            [
                new DiscordRPC.Button
                {
                    Label = "�A�v�������",
                    Url = "https://github.com/c-hfire/LoLAutoAccepter/releases/latest"
                }
            ]
        });
    }

    /// <summary>
    /// Discord RPC ���V���b�g�_�E�����܂��B
    /// </summary>
    public static void Shutdown()
    {
        client.Dispose();
    }
}