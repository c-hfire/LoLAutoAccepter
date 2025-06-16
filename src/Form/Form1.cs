using System.Net.Http.Json;
using System.Text.Json;

namespace LoL_AutoAccept
{
    /// <summary>
    /// LoL自動承諾アプリのメインフォーム。
    /// 設定管理・自動承諾ON/OFF・アップデート確認などを担当。
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// スタートアップ登録用アプリ名
        /// </summary>
        private readonly string AppName = "LoL_Auto_Accepter";

        /// <summary>
        /// 自動承諾機能の有効/無効
        /// </summary>
        private bool isAutoAcceptEnabled = true;

        /// <summary>
        /// アプリ設定
        /// </summary>
        private AppConfig config = new();

        /// <summary>
        /// Lockfile監視インスタンス
        /// </summary>
        private LockfileWatcher? lockfileWatcher;

        /// <summary>
        /// 設定フォームインスタンス
        /// </summary>
        private SettingsForm? settingsForm;

        /// <summary>
        /// GitHub Releases API URL
        /// </summary>
        private const string GitHubReleasesApiUrl = "https://api.github.com/repos/c-hfire/LoLAutoAccepter/releases/latest";

        /// <summary>
        /// 現在のバージョン
        /// </summary>
        private const string CurrentVersion = "1.0.4";

        /// <summary>
        /// フォームのコンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            // DiscordRPCの初期化は後で行う
        }

        /// <summary>
        /// フォームロード時の初期化処理
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // 設定読込
            config = AppConfig.Load();
            config.StartWithWindows = StartupManager.IsStartupEnabled(AppName);
            config.Save();

            // 自動承諾状態を反映
            isAutoAcceptEnabled = config.AutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;

            Logger.Write("LoL Auto Accepter 起動完了");

            if (isAutoAcceptEnabled)
                StartWatcher();

            _ = FetchAndSaveChampionDataAsync();
            UpdateNotifyIcon();

            if (config.DiscordRpcEnabled)
            {
                DiscordRpcManager.Initialize();
                DiscordRpcManager.SetPresence();
            }
            ShowSettingsDialog();
        }

        /// <summary>
        /// フォーム終了時の処理
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfigAndWatcher();
            if (config.DiscordRpcEnabled)
                DiscordRpcManager.Shutdown();
        }

        /// <summary>
        /// 自動承諾ON/OFF切替メニュークリック時
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

        /// <summary>
        /// 設定保存とLockfile監視再起動
        /// </summary>
        private void SaveConfigAndWatcher()
        {
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();
            _ = FetchAndSaveChampionDataAsync();
            UpdateNotifyIcon();
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
        /// 最小化・タスクバー非表示で起動し、アップデート確認
        /// </summary>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Visible = false;

            // アップデートチェック
            await CheckForUpdateAsync();
        }

        /// <summary>
        /// 通知アイコンのダブルクリックで設定ダイアログ表示
        /// </summary>
        private void NotifyIcon1_DoubleClick(object sender, MouseEventArgs e)
        {
            ShowSettingsDialog();
        }

        /// <summary>
        /// 設定メニュークリック時
        /// </summary>
        private void SettingsStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingsDialog();
        }

        /// <summary>
        /// 設定ダイアログ表示・OK時は設定反映
        /// </summary>
        private void ShowSettingsDialog()
        {
            if (settingsForm != null && !settingsForm.IsDisposed)
            {
                settingsForm.Activate();
                return;
            }

            settingsForm = new SettingsForm(config);
            settingsForm.FormClosed += (_, __) => settingsForm = null;
            if (settingsForm.ShowDialog(this) == DialogResult.OK)
            {
                Logger.Write("設定を保存しました。");
                isAutoAcceptEnabled = config.AutoAcceptEnabled;
                SaveConfigAndWatcher();

                if (config.DiscordRpcEnabled)
                {
                    DiscordRpcManager.Initialize();
                    DiscordRpcManager.SetPresence();
                }
                else
                {
                    DiscordRpcManager.Shutdown();
                }
            }
        }

        /// <summary>
        /// GitHubから最新バージョンを取得し、アップデートがあれば通知
        /// </summary>
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

        /// <summary>
        /// バージョン文字列を比較し、最新かどうか判定
        /// </summary>
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

        /// <summary>
        /// 新バージョンがある場合に通知ダイアログを表示
        /// </summary>
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

        /// <summary>
        /// 通知アイコンの状態を更新します。
        /// </summary>
        private void UpdateNotifyIcon()
        {
            // リソースからアイコンを取得
            var icon = isAutoAcceptEnabled
                ? Properties.Resource1.icon_color
                : Properties.Resource1.icon_gray;

            notifyIcon1.Icon = icon;
        }

        /// <summary>
        /// 最新のチャンピオン一覧を取得し、ローカルにJSONで保存します。
        /// </summary>
        private static async Task FetchAndSaveChampionDataAsync()
        {
            try
            {
                // Data Dragonの最新バージョン取得
                using var client = new HttpClient();
                var versions = await client.GetFromJsonAsync<string[]>("https://ddragon.leagueoflegends.com/api/versions.json");
                var latestVersion = versions?[0] ?? "15.12.1"; // バージョンが取得できない場合はデフォルト

                // チャンピオンデータ取得
                var champUrl = $"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/data/ja_JP/champion.json";
                var champJson = await client.GetStringAsync(champUrl);

                using var doc = JsonDocument.Parse(champJson);
                var data = doc.RootElement.GetProperty("data");

                var champList = new List<object>();
                foreach (var champ in data.EnumerateObject())
                {
                    var champObj = champ.Value;
                    champList.Add(new
                    {
                        key = champObj.GetProperty("key").GetString(),
                        name = champObj.GetProperty("name").GetString()
                    });
                }

                // JSONファイルとして保存
                var dateDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");
                var savePath = Path.Combine(dateDir, "champion_list.json");
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                var json = JsonSerializer.Serialize(champList, options);
                await File.WriteAllTextAsync(savePath, json);
                Logger.Write("Data Dragon保存成功");
            }
            catch (Exception ex)
            {
                Logger.Write("Data Dragon取得失敗: " + ex.Message);
            }
        }
    }
}
