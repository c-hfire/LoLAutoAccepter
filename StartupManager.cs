using Microsoft.Win32;

/// <summary>
/// アプリケーションのスタートアップ登録・解除を管理するクラス
/// </summary>
public static class StartupManager
{
    // スタートアップレジストリキーのパス
    private const string RunKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
    /// スタートアップ有効/無効を設定します。
    /// </summary>
    /// <param name="enabled">有効にする場合は true</param>
    /// <param name="appName">アプリ名（レジストリ登録名）</param>
    /// <param name="exePath">実行ファイルのパス</param>
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

    /// <summary>
    /// スタートアップに登録されているかを判定します。
    /// </summary>
    /// <param name="appName">アプリ名（レジストリ登録名）</param>
    /// <returns>登録されていれば true</returns>
    public static bool IsStartupEnabled(string appName)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: false);
        return key?.GetValue(appName) != null;
    }
}