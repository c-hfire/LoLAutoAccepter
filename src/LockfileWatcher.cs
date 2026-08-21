using LoLAutoAccepter.Models;
using LoLAutoAccepter.Services;
using LoLAutoAccepter.Utilities;
using System;
using System.Text;
using System.IO;
using System.Threading;
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
    /// 最後に lockfile が見つからないログを出した時刻（抑制用）
    /// </summary>
    private DateTime lastNoLockfileLog = DateTime.MinValue;

    /// <summary>
    /// lockfile のパスを取得する
    /// </summary>
    private string LockfilePath => Path.Combine(config.LeagueOfLegendsDirectory, "lockfile");

    /// <summary>
    /// インスタンスを初期化する
    /// </summary>
    public LockfileWatcher(AppConfig config)
    {
        this.config = config;
    }

    /// <summary>
    /// lockfile の監視を開始する
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
    /// 監視を停止してリソースを解放する
    /// </summary>
    public void Stop()
    {
        Dispose();
    }

    /// <summary>
    /// lockfile 変更時にデバウンスしてセッション開始を試みる
    /// </summary>
    private void OnLockfileChanged(object? sender, FileSystemEventArgs e)
    {
        Task.Delay(300).ContinueWith(_ => TryStartSession(), TaskScheduler.Default);
    }

    /// <summary>
    /// FileSystemWatcher のエラーをログに出す
    /// </summary>
    private void OnWatcherError(object? sender, ErrorEventArgs e)
    {
        Logger.Write($"FileSystemWatcher エラー: {e.GetException()?.Message}");
    }

    /// <summary>
    /// セッション開始を試みる
    /// </summary>
    private void TryStartSession()
    {
        if (!File.Exists(LockfilePath))
        {
            if ((DateTime.Now - lastNoLockfileLog) > TimeSpan.FromSeconds(10))
            {
                Logger.Write("lockfileが存在しません。セッション開始中止。");
                lastNoLockfileLog = DateTime.Now;
            }
            return;
        }

        string? content = ReadLockfileContent(LockfilePath);
        if (content == null || content == lastLockfileContent)
            return;

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
    /// lockfile の内容を安全に読み取る（短いリトライあり）
    /// </summary>
    private static string? ReadLockfileContent(string path)
    {
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
                Thread.Sleep(30);
            }
        }
        return null;
    }

    /// <summary>
    /// リソースを解放する
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