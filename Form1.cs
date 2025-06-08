using System.Net.Http;
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
        // ���������p�̃L�����Z���g�[�N��
        private CancellationTokenSource autoAcceptCts = new();
        // �ݒ���
        private AppConfig config = new();
        // Lockfile�Ď��p
        private LockfileWatcher? lockfileWatcher;

        private const string GitHubReleasesApiUrl = "https://api.github.com/repos/c-hfire/LoLAutoAccepter/releases/latest";
        private const string CurrentVersion = "1.0.2";

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
            bool actualStartup = StartupManager.IsStartupEnabled(AppName);
            config.StartWithWindows = actualStartup;
            config.Save();

            isAutoAcceptEnabled = config.AutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;
            UpdateDelayMenuCheckmarks(config.AcceptDelaySeconds);
            startupToolStripMenuItem.Checked = StartupManager.IsStartupEnabled(AppName);

            Logger.Write("LoL Auto Accepter �N������");

            if (isAutoAcceptEnabled)
            {
                StartWatcher();
            }
        }

        /// <summary>
        /// �t�H�[���I�����̏���
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();
            StopWatcher();
        }

        /// <summary>
        /// ��������ON/OFF�؂�ւ�
        /// </summary>
        private void ToggleAutoAcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isAutoAcceptEnabled = !isAutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;

            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();

            Logger.Write($"���������� {(isAutoAcceptEnabled ? "ON" : "OFF")} �ɐ؂�ւ��܂����B");
            StopWatcher();

            if (isAutoAcceptEnabled)
            {
                StartWatcher();
            }
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
        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// �����f�B���C�b����ݒ�
        /// </summary>
        private void SetDelay(int seconds)
        {
            config.AcceptDelaySeconds = seconds;
            config.Save();
            UpdateDelayMenuCheckmarks(seconds);
        }

        /// <summary>
        /// �f�B���C�I�����j���[�̃`�F�b�N��Ԃ��X�V
        /// </summary>
        private void UpdateDelayMenuCheckmarks(int selected)
        {
            delay0ToolStripMenuItem.Checked = (selected == 0);
            delay2ToolStripMenuItem.Checked = (selected == 2);
            delay5ToolStripMenuItem.Checked = (selected == 5);
            delay10ToolStripMenuItem.Checked = (selected == 10);
        }

        // �e�f�B���C���j���[�N���b�N���̏���
        private void Delay0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(0);
            Logger.Write("�f�B���C��0�b�ɐݒ�");
        }

        private void Delay2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(2);
            Logger.Write("�f�B���C��2�b�ɐݒ�");
        }

        private void Delay5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(5);
            Logger.Write("�f�B���C��5�b�ɐݒ�");
        }

        private void Delay10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDelay(10);
            Logger.Write("�f�B���C��10�b�ɐݒ�");
        }

        /// <summary>
        /// �X�^�[�g�A�b�v�L��/�����؂�ւ�
        /// </summary>
        private void StartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool willEnable = !startupToolStripMenuItem.Checked;

            StartupManager.SetStartupEnabled(willEnable, AppName, Application.ExecutablePath);
            startupToolStripMenuItem.Checked = willEnable;

            config.StartWithWindows = willEnable;
            config.Save();

            Logger.Write($"�X�^�[�g�A�b�v�N���� {(willEnable ? "�L����" : "������")} ���܂����B");
        }

        /// <summary>
        /// �ݒ�t�H���_���G�N�X�v���[���[�ŊJ��
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
                Logger.Write($"�ݒ�t�H���_�̃I�[�v���Ɏ��s: {ex.Message}");
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
                Logger.Write("�A�b�v�f�[�g�`�F�b�N���s: " + ex.Message);
            }
        }

        // �o�[�W������r
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

        // �A�b�v�f�[�g�ʒm�i�^�X�N�g���C�o���[�� or ���b�Z�[�W�{�b�N�X���j
        private void ShowUpdateNotification(string latestTag, string? url)
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
