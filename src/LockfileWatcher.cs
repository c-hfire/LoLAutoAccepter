using LoLAutoAccepter.Models;
using LoLAutoAccepter.Services;
using LoLAutoAccepter.Utilities;
using System.Text;
using System.IO;
using System.Threading.Tasks;

public class LockfileWatcher : IDisposable
{
    private readonly AppConfig config;
    private CancellationTokenSource? sessionCts;
    private Task? sessionTask;
    private FileSystemWatcher? fsWatcher;
    private string? lastLockfileContent;
    private readonly object sessionLock = new();

    /// <summary>
    /// lockfile のパスを取得します。
    /// </summary>
    private string LockfilePath => Path.Combine(config.LeagueOfLegendsDirectory, "lockfile");

    /// <summary>
    /// LockfileWatcher の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="config">アプリ設定</param>
    public LockfileWatcher(AppConfig config)
    {
        this.config = config;
    }

    /// <summary>
    /// lockfile監視を開始します。
    /// </summary>
    public void Start()
    {
        var dir = Path.GetDirectoryName(LockfilePath);
        if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
        {
            Logger.Write($"設定されたディレクトリが存在しません: {config.LeagueOfLegendsDirectory}。lockfile 監視を開始できません。設定を確認してください。");
            return;
        }

        try
        {
            fsWatcher = new FileSystemWatcher(dir)
            {
                Filter = "lockfile",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size,
                EnableRaisingEvents = true
            };
            fsWatcher.Changed += OnLockfileChanged;
            fsWatcher.Created += OnLockfileChanged;
            fsWatcher.Error += OnWatcherError;
            TryStartSession();
        }
        catch (Exception ex)
        {
            Logger.Write($"FileSystemWatcher の初期化に失敗しました: {ex.Message}");
        }
    }

    /// <summary>
    /// lockfile監視を停止します。
    /// </summary>
    public void Stop()
    {
        Dispose();
    }

    /// <summary>
    /// lockfile変更時の処理
    /// </summary>
    /// <param name="sender">イベント送信元</param>
    /// <param name="e">イベント引数</param>
    private void OnLockfileChanged(object? sender, FileSystemEventArgs e)
    {
        // 短時間に何度も来るので軽いデバウンス（簡易）
        Task.Delay(50).ContinueWith(_ => TryStartSession(), TaskScheduler.Default);
    }

    /// <summary>
    /// FileSystemWatcher エラー時の処理
    /// </summary>
    /// <param name="sender">イベント送信元</param>
    /// <param name="e">イベント引数</param>
    private void OnWatcherError(object? sender, ErrorEventArgs e)
    {
        Logger.Write($"FileSystemWatcher エラー: {e.GetException()?.Message}");
    }

    /// <summary>
    /// セッション開始を試みます。
    /// </summary>
    private void TryStartSession()
    {
        if (!File.Exists(LockfilePath))
        {
            Logger.Write("lockfileが存在しません。セッション開始中止。");
            return;
        }

        string? content = ReadLockfileContent(LockfilePath);
        if (content == null || content == lastLockfileContent)
            return;

        // lockfile の内容が有効かどうかチェック
        if (!LockfileParser.TryParse(content, out _, out _))
        {
            Logger.Write("lockfile の内容が不正です。セッション開始中止。");
            return;
        }

        lastLockfileContent = content;

        lock (sessionLock)
        {
            sessionCts?.Cancel();
            sessionCts = new CancellationTokenSource();

            Logger.Write("新しいlockfileを検出。セッション開始中…");

            // 例外を確実にログするために ContinueWith を付ける
            sessionTask = AutoAccepter.RunSessionAsync(sessionCts.Token, config, content);
            sessionTask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Logger.Write($"セッションで例外発生: {t.Exception?.GetBaseException().Message}");
                }
            }, TaskScheduler.Default);
        }
    }

    /// <summary>
    /// lockfileの内容を読み取ります。
    /// </summary>
    /// <param name="path">lockfileのパス</param>
    /// <returns>内容文字列またはnull</returns>
    private static string? ReadLockfileContent(string path)
    {
        // 競合回避のため短いリトライを行う
        const int maxAttempts = 3;
        for (int i = 0; i < maxAttempts; i++)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fs, Encoding.UTF8);
                var text = reader.ReadToEnd();
                return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
            }
            catch
            {
                // 少し待って再試行
                Thread.Sleep(30);
            }
        }
        return null;
    }

    /// <summary>
    /// リソースを解放します。
    /// </summary>
    public void Dispose()
    {
        fsWatcher?.Dispose();
        fsWatcher = null;

        lock (sessionLock)
        {
            sessionCts?.Cancel();
            sessionCts = null;

            if (sessionTask != null && !sessionTask.IsCompleted)
            {
                // 非同期で完了を監視し、完了時の例外をログする
                sessionTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        Logger.Write($"セッション終了時に例外: {t.Exception?.GetBaseException().Message}");
                }, TaskScheduler.Default);
            }
            sessionTask = null;
        }
    }
}