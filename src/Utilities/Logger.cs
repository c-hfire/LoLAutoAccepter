using System;
using System.IO;
using System.Text;

namespace LoLAutoAccepter.Utilities
{
    /// <summary>
    /// ログ出力を行う静的クラス
    /// </summary>
    public static class Logger
    {
        private static readonly string LogDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");
        private static readonly string LogFilePath =
            Path.Combine(LogDirectory, "log.txt");
        private static readonly object writeLock = new();
        private const long MaxLogFileBytes = 5 * 1024 * 1024;

        /// <summary>
        /// メッセージをログファイルに追記する
        /// </summary>
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
                try { System.Diagnostics.Debug.WriteLine(message); } catch { }
            }
        }

        /// <summary>
        /// ログディレクトリが存在しない場合は作成する
        /// </summary>
        private static void EnsureLogDirectory()
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        /// <summary>
        /// ログファイルサイズが閾値を超えたらバックアップを作成する
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
            }
        }
    }
}