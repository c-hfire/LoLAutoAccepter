using System;
using System.IO;
using System.Text;

namespace LoLAutoAccepter.Utilities
{
    /// <summary>
    /// アプリケーションのログ出力を行う静的クラス
    /// </summary>
    public static class Logger
    {
        private static readonly string LogDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");
        private static readonly string LogFilePath =
            Path.Combine(LogDirectory, "log.txt");
        private static readonly object writeLock = new();

        private const long MaxLogFileBytes = 5 * 1024 * 1024; // 5MB

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
                lock (writeLock)
                {
                    RotateIfNeeded();
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // ログ出力失敗時は最小限に抑える（必要なら Debug 出力）
                try { System.Diagnostics.Debug.WriteLine(message); } catch { }
            }
        }

        /// <summary>
        /// ログディレクトリが存在しない場合は作成します。
        /// </summary>
        private static void EnsureLogDirectory()
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        /// <summary>
        /// ログファイルのサイズが閾値を超えた場合にバックアップファイルを作成します。
        /// </summary>
        private static void RotateIfNeeded()
        {
            try
            {
                if (File.Exists(LogFilePath))
                {
                    var fi = new FileInfo(LogFilePath);
                    if (fi.Length >= MaxLogFileBytes)
                    {
                        var bak = LogFilePath + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
                        File.Move(LogFilePath, bak);
                    }
                }
            }
            catch
            {
                // ローテーション失敗は無視（ロギング自体を阻害したくない）
            }
        }
    }
}