using System.Net.Http.Json;
using System.Text.Json;

namespace LoL_AutoAccept
{
    /// <summary>
    /// LoL���������A�v���̃��C���t�H�[���B
    /// �ݒ�Ǘ��E��������ON/OFF�E�A�b�v�f�[�g�m�F�Ȃǂ�S���B
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// �X�^�[�g�A�b�v�o�^�p�A�v����
        /// </summary>
        private readonly string AppName = "LoL_Auto_Accepter";

        /// <summary>
        /// ���������@�\�̗L��/����
        /// </summary>
        private bool isAutoAcceptEnabled = true;

        /// <summary>
        /// �A�v���ݒ�
        /// </summary>
        private AppConfig config = new();

        /// <summary>
        /// Lockfile�Ď��C���X�^���X
        /// </summary>
        private LockfileWatcher? lockfileWatcher;

        /// <summary>
        /// �ݒ�t�H�[���C���X�^���X
        /// </summary>
        private SettingsForm? settingsForm;

        /// <summary>
        /// GitHub Releases API URL
        /// </summary>
        private const string GitHubReleasesApiUrl = "https://api.github.com/repos/c-hfire/LoLAutoAccepter/releases/latest";

        /// <summary>
        /// ���݂̃o�[�W����
        /// </summary>
        private const string CurrentVersion = "1.0.4";

        /// <summary>
        /// �t�H�[���̃R���X�g���N�^
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            // DiscordRPC�̏������͌�ōs��
        }

        /// <summary>
        /// �t�H�[�����[�h���̏���������
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // �ݒ�Ǎ�
            config = AppConfig.Load();
            config.StartWithWindows = StartupManager.IsStartupEnabled(AppName);
            config.Save();

            // ����������Ԃ𔽉f
            isAutoAcceptEnabled = config.AutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;

            Logger.Write("LoL Auto Accepter �N������");

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
        /// �t�H�[���I�����̏���
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfigAndWatcher();
            if (config.DiscordRpcEnabled)
                DiscordRpcManager.Shutdown();
        }

        /// <summary>
        /// ��������ON/OFF�ؑփ��j���[�N���b�N��
        /// </summary>
        private void ToggleAutoAcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isAutoAcceptEnabled = !isAutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            Logger.Write($"���������� {(isAutoAcceptEnabled ? "ON" : "OFF")} �ɐ؂�ւ��܂����B");
            SaveConfigAndWatcher();
        }

        /// <summary>
        /// Lockfile�Ď��J�n
        /// </summary>
        private void StartWatcher()
        {
            lockfileWatcher = new LockfileWatcher(config);
            lockfileWatcher.Start();
        }

        /// <summary>
        /// Lockfile�Ď���~
        /// </summary>
        private void StopWatcher()
        {
            lockfileWatcher?.Stop();
            lockfileWatcher = null;
        }

        /// <summary>
        /// �ݒ�ۑ���Lockfile�Ď��ċN��
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
        /// �I�����j���[�N���b�N��
        /// </summary>
        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// �ŏ����E�^�X�N�o�[��\���ŋN�����A�A�b�v�f�[�g�m�F
        /// </summary>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Visible = false;

            // �A�b�v�f�[�g�`�F�b�N
            await CheckForUpdateAsync();
        }

        /// <summary>
        /// �ʒm�A�C�R���̃_�u���N���b�N�Őݒ�_�C�A���O�\��
        /// </summary>
        private void NotifyIcon1_DoubleClick(object sender, MouseEventArgs e)
        {
            ShowSettingsDialog();
        }

        /// <summary>
        /// �ݒ胁�j���[�N���b�N��
        /// </summary>
        private void SettingsStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingsDialog();
        }

        /// <summary>
        /// �ݒ�_�C�A���O�\���EOK���͐ݒ蔽�f
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
                Logger.Write("�ݒ��ۑ����܂����B");
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
        /// GitHub����ŐV�o�[�W�������擾���A�A�b�v�f�[�g������Βʒm
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
                Logger.Write("�A�b�v�f�[�g�`�F�b�N���s: " + ex.Message);
            }
        }

        /// <summary>
        /// �o�[�W������������r���A�ŐV���ǂ�������
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
        /// �V�o�[�W����������ꍇ�ɒʒm�_�C�A���O��\��
        /// </summary>
        private static void ShowUpdateNotification(string latestTag, string? url)
        {
            string msg = $"�V�����o�[�W���� {latestTag} �����p�\�ł��B�_�E�����[�h���܂����H";
            if (MessageBox.Show(msg, "�A�b�v�f�[�g�ʒm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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
        /// �ʒm�A�C�R���̏�Ԃ��X�V���܂��B
        /// </summary>
        private void UpdateNotifyIcon()
        {
            // ���\�[�X����A�C�R�����擾
            var icon = isAutoAcceptEnabled
                ? Properties.Resource1.icon_color
                : Properties.Resource1.icon_gray;

            notifyIcon1.Icon = icon;
        }

        /// <summary>
        /// �ŐV�̃`�����s�I���ꗗ���擾���A���[�J����JSON�ŕۑ����܂��B
        /// </summary>
        private static async Task FetchAndSaveChampionDataAsync()
        {
            try
            {
                // Data Dragon�̍ŐV�o�[�W�����擾
                using var client = new HttpClient();
                var versions = await client.GetFromJsonAsync<string[]>("https://ddragon.leagueoflegends.com/api/versions.json");
                var latestVersion = versions?[0] ?? "15.12.1"; // �o�[�W�������擾�ł��Ȃ��ꍇ�̓f�t�H���g

                // �`�����s�I���f�[�^�擾
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

                // JSON�t�@�C���Ƃ��ĕۑ�
                var dateDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");
                var savePath = Path.Combine(dateDir, "champion_list.json");
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                var json = JsonSerializer.Serialize(champList, options);
                await File.WriteAllTextAsync(savePath, json);
                Logger.Write("Data Dragon�ۑ�����");
            }
            catch (Exception ex)
            {
                Logger.Write("Data Dragon�擾���s: " + ex.Message);
            }
        }
    }
}
