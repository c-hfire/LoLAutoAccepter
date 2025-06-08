using System.Net.Http;
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
        // 自動承諾用のキャンセルトークン
        private CancellationTokenSource autoAcceptCts = new();
        // 設定情報
        private AppConfig config = new();
        // Lockfile監視用
        private LockfileWatcher? lockfileWatcher;

        private const string GitHubReleasesApiUrl = "https://api.github.com/repos/c-hfire/LoLAutoAccepter/releases/latest";
        private const string CurrentVersion = "1.0.2";

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
            bool actualStartup = StartupManager.IsStartupEnabled(AppName);
            config.StartWithWindows = actualStartup;
            config.Save();

            isAutoAcceptEnabled = config.AutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;
            UpdateDelayMenuCheckmarks(config.AcceptDelaySeconds);
            startupToolStripMenuItem.Checked = StartupManager.IsStartupEnabled(AppName);

            Logger.Write("LoL Auto Accepter 起動完了");

            if (isAutoAcceptEnabled)
            {
                StartWatcher();
            }
        }

        /// <summary>
        /// フォーム終了時の処理
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();
            StopWatcher();
        }

        /// <summary>
        /// 自動承諾ON/OFF切り替え
        /// </summary>
        private void ToggleAutoAcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isAutoAcceptEnabled = !isAutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;

            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();

            Logger.Write($"自動承諾を {(isAutoAcceptEnabled ? "ON" : "OFF")} に切り替えました。");
            StopWatcher();

            if (isAutoAcceptEnabled)
            {
                StartWatcher();
            }
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
        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// 承諾ディレイ秒数を設定
        /// </summary>
        private void SetDelay(int seconds)
        {
            config.AcceptDelaySeconds = seconds;
            config.Save();
            UpdateDelayMenuCheckmarks(seconds);
        }

        /// <summary>
        /// ディレイ選択メニューのチェック状態を更新
        /// </summary>
        private void UpdateDelayMenuCheckmarks(int selected)
        {
            delay0ToolStripMenuItem.Checked = (selected == 0);
            delay2ToolStripMenuItem.Checked = (selected == 2);
            delay5ToolStripMenuItem.Checked = (selected == 5);
            delay10ToolStripMenuItem.Checked = (selected == 10);
        }

        // 各ディレイメニュークリック時の処理
        private void Delay0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(0);
            Logger.Write("ディレイを0秒に設定");
        }

        private void Delay2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(2);
            Logger.Write("ディレイを2秒に設定");
        }

        private void Delay5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(5);
            Logger.Write("ディレイを5秒に設定");
        }

        private void Delay10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(10);
            Logger.Write("ディレイを10秒に設定");
        }

        /// <summary>
        /// スタートアップ有効/無効切り替え
        /// </summary>
        private void StartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool willEnable = !startupToolStripMenuItem.Checked;

            StartupManager.SetStartupEnabled(willEnable, AppName, Application.ExecutablePath);
            startupToolStripMenuItem.Checked = willEnable;

            config.StartWithWindows = willEnable;
            config.Save();

            Logger.Write($"スタートアップ起動を {(willEnable ? "有効化" : "無効化")} しました。");
        }

        /// <summary>
        /// 設定フォルダをエクスプローラーで開く
        /// </summary>
        private void OpenConfigFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string folderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "LAA");

                if (Directory.Exists(folderPath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", folderPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"設定フォルダのオープンに失敗: {ex.Message}");
            }
        }

        private async Task CheckForUpdateAsync()
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
        private bool IsNewerVersion(string latest, string current)
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
        private void ShowUpdateNotification(string latestTag, string? url)
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
