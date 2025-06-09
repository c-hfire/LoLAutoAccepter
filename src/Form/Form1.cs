using System.Text.Json;

namespace LoL_AutoAccept
{
    /// <summary>
    /// LoL���������A�v���̃��C���t�H�[���B
    /// �ݒ�Ǘ��E��������ON/OFF�E�A�b�v�f�[�g�m�F�Ȃǂ�S���B
    /// </summary>
    public partial class Form1 : Form
    {
        // �X�^�[�g�A�b�v�o�^�p�A�v����
        private readonly string AppName = "LoL_Auto_Accepter";
        // ���������@�\�̗L��/����
        private bool isAutoAcceptEnabled = true;
        // �A�v���ݒ�
        private AppConfig config = new();
        // Lockfile�Ď��C���X�^���X
        private LockfileWatcher? lockfileWatcher;

        private const string GitHubReleasesApiUrl = "https://api.github.com/repos/c-hfire/LoLAutoAccepter/releases/latest";
        private const string CurrentVersion = "1.0.3";

        /// <summary>
        /// �t�H�[���̃R���X�g���N�^
        /// </summary>
        public Form1()
        {
            InitializeComponent();
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

            UpdateNotifyIcon();
        }

        /// <summary>
        /// �t�H�[���I�����̏���
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfigAndWatcher();
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
            using var dlg = new SettingsForm(config);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Logger.Write("�ݒ��ۑ����܂����B");
                isAutoAcceptEnabled = config.AutoAcceptEnabled;
                SaveConfigAndWatcher();
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

        private void UpdateNotifyIcon()
        {
            // ���\�[�X����A�C�R�����擾
            var icon = isAutoAcceptEnabled
                ? Properties.Resource1.icon_color
                : Properties.Resource1.icon_gray;

            notifyIcon1.Icon = icon;
        }
    }
}
