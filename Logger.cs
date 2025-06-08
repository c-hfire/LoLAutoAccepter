using System.Diagnostics;

/// <summary>
/// アプリケーションのログ出力を行う静的クラス
/// </summary>
public static class Logger
{
    // ログ保存ディレクトリ
    private static readonly string LogDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

    // ログファイルパス
    private static readonly string LogFilePath =
        Path.Combine(LogDirectory, "log.txt");

    /// <summary>
    /// メッセージをログファイルに追記します。
    /// </summary>
    /// <param name="message">出力するメッセージ</param>
    public static void Write(string message)
    {
        try
        {
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
        }
    }
}
