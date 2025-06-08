using System.Text.Json;

namespace LoL_AutoAccept
{
    /// <summary>
    /// メインフォームクラス。自動承諾機能や設定の管理を行う。
    /// </summary>
    public partial class Form1 : Form
    {
        // アプリ名（スタートアップ登録用）
        private readonly string AppName = "LoL_Auto_Accepter";
        // 自動承諾機能の有効/無効フラグ
        private bool isAutoAcceptEnabled = true;
        // 設定情報
        private AppConfig config = new();
        // Lockfile監視用
        private LockfileWatcher? lockfileWatcher;

        private const string GitHubReleasesApiUrl = "https://api.github.com/repos/c-hfire/LoLAutoAccepter/releases/latest";
        private const string CurrentVersion = "1.0.3";

        /// <summary>
        /// フォーム初期化
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロード時の処理
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            config = AppConfig.Load();
            config.StartWithWindows = StartupManager.IsStartupEnabled(AppName);
            config.Save();

            isAutoAcceptEnabled = config.AutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;

            Logger.Write("LoL Auto Accepter 起動完了");

            if (isAutoAcceptEnabled)
                StartWatcher();
        }

        /// <summary>
        /// フォーム終了時の処理
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfigAndWatcher();
        }

        /// <summary>
        /// 自動承諾ON/OFF切り替え
        /// </summary>
        private void ToggleAutoAcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isAutoAcceptEnabled = !isAutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            Logger.Write($"自動承諾を {(isAutoAcceptEnabled ? "ON" : "OFF")} に切り替えました。");
            SaveConfigAndWatcher();
        }

        /// <summary>
        /// Lockfile監視開始
        /// </summary>
        private void StartWatcher()
        {
            lockfileWatcher = new LockfileWatcher(config);
            lockfileWatcher.Start();
        }

        /// <summary>
        /// Lockfile監視停止
        /// </summary>
        private void StopWatcher()
        {
            lockfileWatcher?.Stop();
            lockfileWatcher = null;
        }

        private void SaveConfigAndWatcher()
        {
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();
            StopWatcher();
            if (isAutoAcceptEnabled)
                StartWatcher();
        }

        /// <summary>
        /// 終了メニュークリック時
        /// </summary>
        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// フォームを最小化・タスクバー非表示で起動
        /// </summary>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Visible = false;

            // 起動時にアップデートチェック
            await CheckForUpdateAsync();
        }

        /// <summary>
        /// 通知アイコンのダブルクリックでウィンドウ表示
        /// </summary>
        private void NotifyIcon1_DoubleClick(object sender, MouseEventArgs e)
        {
            ShowSettingsDialog();
        }

        private void SettingsStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingsDialog();
        }

        private void ShowSettingsDialog()
        {
            using var dlg = new SettingsForm(config);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Logger.Write("設定を保存しました。");
                isAutoAcceptEnabled = config.AutoAcceptEnabled;
                SaveConfigAndWatcher();
            }
        }

        private static async Task CheckForUpdateAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("LoLAutoAccepter-Updater");

                var response = await client.GetAsync(GitHubReleasesApiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var latestTag = doc.RootElement.GetProperty("tag_name").GetString();

                if (!string.IsNullOrEmpty(latestTag) && IsNewerVersion(latestTag, CurrentVersion))
                {
                    var htmlUrl = doc.RootElement.GetProperty("html_url").GetString();
                    ShowUpdateNotification(latestTag, htmlUrl);
                }
            }
            catch (Exception ex)
            {
                Logger.Write("アップデートチェック失敗: " + ex.Message);
            }
        }

        // バージョン比較
        private static bool IsNewerVersion(string latest, string current)
        {
            string l = latest.TrimStart('v', 'V');
            string c = current.TrimStart('v', 'V');
            if (Version.TryParse(l, out var latestVer) && Version.TryParse(c, out var currentVer))
            {
                return latestVer > currentVer;
            }
            return false;
        }

        // アップデート通知（タスクトレイバルーン or メッセージボックス等）
        private static void ShowUpdateNotification(string latestTag, string? url)
        {
            string msg = $"新しいバージョン {latestTag} が利用可能です。ダウンロードしますか？";
            if (MessageBox.Show(msg, "アップデート通知", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(url))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
        }
    }
}
