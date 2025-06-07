using Microsoft.Win32;

public static class StartupManager
{
    private const string RunKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    public static void SetStartupEnabled(bool enabled, string appName, string exePath)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true);
        if (key == null)
        {
            MessageBox.Show("スタートアップレジストリキーにアクセスできません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (enabled)
        {
            key.SetValue(appName, exePath);
        }
        else
        {
            key.DeleteValue(appName, false);
        }
    }

    public static bool IsStartupEnabled(string appName)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: false);
        return key?.GetValue(appName) != null;
    }
}