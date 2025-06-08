using System.Text.Json;

namespace LoL_AutoAccept
{
    /// <summary>
    /// ���C���t�H�[���N���X�B���������@�\��ݒ�̊Ǘ����s���B
    /// </summary>
    public partial class Form1 : Form
    {
        // �A�v�����i�X�^�[�g�A�b�v�o�^�p�j
        private readonly string AppName = "LoL_Auto_Accepter";
        // ���������@�\�̗L��/�����t���O
        private bool isAutoAcceptEnabled = true;
        // �ݒ���
        private AppConfig config = new();
        // Lockfile�Ď��p
        private LockfileWatcher? lockfileWatcher;

        private const string GitHubReleasesApiUrl = "https://api.github.com/repos/c-hfire/LoLAutoAccepter/releases/latest";
        private const string CurrentVersion = "1.0.3";

        /// <summary>
        /// �t�H�[��������
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �t�H�[�����[�h���̏���
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            config = AppConfig.Load();
            config.StartWithWindows = StartupManager.IsStartupEnabled(AppName);
            config.Save();

            isAutoAcceptEnabled = config.AutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;

            Logger.Write("LoL Auto Accepter �N������");

            if (isAutoAcceptEnabled)
                StartWatcher();
        }

        /// <summary>
        /// �t�H�[���I�����̏���
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfigAndWatcher();
        }

        /// <summary>
        /// ��������ON/OFF�؂�ւ�
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

        private void SaveConfigAndWatcher()
        {
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();
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
        /// �t�H�[�����ŏ����E�^�X�N�o�[��\���ŋN��
        /// </summary>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Visible = false;

            // �N�����ɃA�b�v�f�[�g�`�F�b�N
            await CheckForUpdateAsync();
        }

        /// <summary>
        /// �ʒm�A�C�R���̃_�u���N���b�N�ŃE�B���h�E�\��
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
                Logger.Write("�ݒ��ۑ����܂����B");
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
                Logger.Write("�A�b�v�f�[�g�`�F�b�N���s: " + ex.Message);
            }
        }

        // �o�[�W������r
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

        // �A�b�v�f�[�g�ʒm�i�^�X�N�g���C�o���[�� or ���b�Z�[�W�{�b�N�X���j
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
    }
}
