﻿using LoLAutoAccepter.Models;
using LoLAutoAccepter.Services;
using LoLAutoAccepter.Utilities;
using System.Text;

/// <summary>
/// lockfile の監視とセッション管理を行うクラス
/// </summary>
public class LockfileWatcher
{
    private readonly AppConfig config;
    private CancellationTokenSource? sessionCts;
    private Task? sessionTask;
    private FileSystemWatcher? fsWatcher;
    private string? lastLockfileContent;

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
        fsWatcher = new FileSystemWatcher(Path.GetDirectoryName(LockfilePath)!);
        fsWatcher.Filter = "lockfile";
        fsWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size;
        fsWatcher.Changed += OnLockfileChanged;
        fsWatcher.Created += OnLockfileChanged;
        fsWatcher.EnableRaisingEvents = true;
        TryStartSession();
    }

    /// <summary>
    /// lockfile監視を停止します。
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
    /// lockfile変更時の処理
    /// </summary>
    /// <param name="sender">イベント送信元</param>
    /// <param name="e">イベント引数</param>
    private void OnLockfileChanged(object? sender, FileSystemEventArgs e)
    {
        TryStartSession();
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

        sessionCts?.Cancel();
        sessionCts = new CancellationTokenSource();

        Logger.Write("新しいlockfileを検出。セッション開始中…");
        sessionTask = AutoAccepter.RunSessionAsync(sessionCts.Token, config, content);
    }

    /// <summary>
    /// lockfileの内容を読み取ります。
    /// </summary>
    /// <param name="path">lockfileのパス</param>
    /// <returns>内容文字列またはnull</returns>
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
            // 読み込み失敗時はnullを返す
            return null;
        }
    }
}