namespace LoL_AutoAccept
{
    public partial class Form1 : Form
    {
        private readonly string AppName = "LoL_Auto_Accepter";
        private bool isAutoAcceptEnabled = true;
        private CancellationTokenSource autoAcceptCts = new();
        private AppConfig config = new();


        public Form1()
        {
            InitializeComponent();
        }

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
                autoAcceptCts = new CancellationTokenSource();
                _ = AutoAccepter.StartAsync(autoAcceptCts.Token, config);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();
        }

        private void ToggleAutoAcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isAutoAcceptEnabled = !isAutoAcceptEnabled;
            toggleAutoAcceptToolStripMenuItem.Checked = isAutoAcceptEnabled;

            config.AutoAcceptEnabled = isAutoAcceptEnabled;
            config.Save();

            Logger.Write($"���������� {(isAutoAcceptEnabled ? "ON" : "OFF")} �ɐ؂�ւ��܂����B");

            autoAcceptCts.Cancel();
            autoAcceptCts = new CancellationTokenSource();

            if (isAutoAcceptEnabled)
            {
                _ = AutoAccepter.StartAsync(autoAcceptCts.Token, config);
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Visible = false;
        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void SetDelay(int seconds)
        {
            config.AcceptDelaySeconds = seconds;
            config.Save();
            UpdateDelayMenuCheckmarks(seconds);
        }

        private void UpdateDelayMenuCheckmarks(int selected)
        {
            delay0ToolStripMenuItem.Checked = (selected == 0);
            delay2ToolStripMenuItem.Checked = (selected == 2);
            delay5ToolStripMenuItem.Checked = (selected == 5);
            delay10ToolStripMenuItem.Checked = (selected == 10);
        }


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

        private void StartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool willEnable = !startupToolStripMenuItem.Checked;

            StartupManager.SetStartupEnabled(willEnable, AppName, Application.ExecutablePath);
            startupToolStripMenuItem.Checked = willEnable;

            config.StartWithWindows = willEnable;
            config.Save();

            Logger.Write($"�X�^�[�g�A�b�v�N���� {(willEnable ? "�L����" : "������")} ���܂����B");
        }

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
            catch
            {
            }
        }


    }
}
