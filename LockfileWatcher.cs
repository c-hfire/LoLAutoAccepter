using System.Text;

/// <summary>
/// lockfile の監視とセッション管理を行うクラス
/// </summary>
public class LockfileWatcher
{
    // 設定情報
    private readonly AppConfig config;
    // セッション用キャンセルトークン
    private CancellationTokenSource? sessionCts;
    // セッションタスク
    private Task? sessionTask;
    // lockfile監視用
    private FileSystemWatcher? fsWatcher;
    // 前回読み込んだlockfile内容
    private string? lastLockfileContent;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public LockfileWatcher(AppConfig config)
    {
        this.config = config;
    }

    /// <summary>
    /// lockfile監視を開始
    /// </summary>
    public void Start()
    {
        string lockfilePath = Path.Combine(config.LeagueOfLegendsDirectory, "lockfile");

        fsWatcher = new FileSystemWatcher(Path.GetDirectoryName(lockfilePath)!);
        fsWatcher.Filter = "lockfile";
        fsWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size;
        fsWatcher.Changed += OnLockfileChanged;
        fsWatcher.Created += OnLockfileChanged;
        fsWatcher.EnableRaisingEvents = true;
        TryStartSession();
    }

    /// <summary>
    /// lockfile監視を停止し、セッションも終了
    /// </summary>
    public void Stop()
    {
        fsWatcher?.Dispose();
        fsWatcher = null;

        sessionCts?.Cancel();
        sessionCts = null;
        sessionTask = null;
    }

    /// <summary>
    /// lockfileの変更イベントハンドラ
    /// </summary>
    private void OnLockfileChanged(object sender, FileSystemEventArgs e)
    {
        TryStartSession();
    }

    /// <summary>
    /// lockfileの内容が変わった場合に新しいセッションを開始
    /// </summary>
    private void TryStartSession()
    {
        string lockfilePath = Path.Combine(config.LeagueOfLegendsDirectory, "lockfile");

        if (!File.Exists(lockfilePath))
        {
            Logger.Write("lockfileが存在しません。セッション開始中止。");
            return;
        }

        string? content = ReadLockfileContent(lockfilePath);
        if (content == null || content == lastLockfileContent)
        {
            return;
        }

        lastLockfileContent = content;

        sessionCts?.Cancel();
        sessionCts = new CancellationTokenSource();

        Logger.Write("新しいlockfileを検出。セッション開始中…");
        sessionTask = AutoAccepter.RunSessionAsync(sessionCts.Token, config, content);
    }

    /// <summary>
    /// lockfileの内容を安全に読み込む
    /// </summary>
    private static string? ReadLockfileContent(string path)
    {
        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        catch
        {
            return null;
        }
    }
}
