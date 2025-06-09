/// <summary>
/// アプリケーションのログ出力を行う静的クラス
/// </summary>
public static class Logger
{
    private static readonly string LogDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");
    private static readonly string LogFilePath =
        Path.Combine(LogDirectory, "log.txt");

    /// <summary>
    /// 指定したメッセージをログに記録します。
    /// </summary>
    /// <param name="message">出力するメッセージ</param>
    public static void Write(string message)
    {
        try
        {
            EnsureLogDirectory();
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
            // ログ出力失敗時は無視
        }
    }

    private static void EnsureLogDirectory()
    {
        if (!Directory.Exists(LogDirectory))
            Directory.CreateDirectory(LogDirectory);
    }
}
