using Microsoft.Win32;

/// <summary>
/// Windows スタートアップ登録の管理
/// </summary>
public static class StartupManager
{
    private const string RunKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
    /// スタートアップの有効/無効を設定する
    /// </summary>
    public static void SetStartupEnabled(bool enabled, string appName, string exePath)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true);
        if (key == null)
        {
            MessageBox.Show("スタートアップレジストリキーにアクセスできません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (enabled)
            key.SetValue(appName, exePath);
        else
            key.DeleteValue(appName, false);
    }

    /// <summary>
    /// スタートアップ登録を判定する
    /// </summary>
    public static bool IsStartupEnabled(string appName)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: false);
        return key?.GetValue(appName) != null;
    }
}