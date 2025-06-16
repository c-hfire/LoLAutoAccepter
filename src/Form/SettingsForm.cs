namespace LoL_AutoAccept
{
    /// <summary>
    /// �A�v���P�[�V�����ݒ��ҏW���邽�߂̃t�H�[��
    /// </summary>
    public partial class SettingsForm : Form
    {
        // �����K������: �t�B�[���h���� _config ��
        private readonly AppConfig _config;

        /// <summary>
        /// �ݒ�t�H�[�������������܂��B
        /// </summary>
        /// <param name="config">�A�v���P�[�V�����ݒ�</param>
        public SettingsForm(AppConfig config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
        }

        /// <summary>
        /// UI�ɐݒ�l�����[�h���܂��B
        /// </summary>
        private void LoadSettings()
        {
            checkBoxAutoAccept.Checked = _config.AutoAcceptEnabled;
            numericUpDownDelay.Value = _config.AcceptDelaySeconds;
            checkBoxStartup.Checked = _config.StartWithWindows;
            textBoxLoLDir.Text = _config.LeagueOfLegendsDirectory;
            checkBoxAutoClose.Checked = _config.AutoCloseOnAccept;
            checkBoxDiscordRpc.Checked = _config.DiscordRpcEnabled;
        }

        /// <summary>
        /// UI�̒l��ݒ�ɕۑ����܂��B
        /// </summary>
        private void SaveSettings()
        {
            _config.AutoAcceptEnabled = checkBoxAutoAccept.Checked;
            _config.AcceptDelaySeconds = (int)numericUpDownDelay.Value;
            _config.StartWithWindows = checkBoxStartup.Checked;
            _config.LeagueOfLegendsDirectory = textBoxLoLDir.Text;
            _config.AutoCloseOnAccept = checkBoxAutoClose.Checked;
            _config.DiscordRpcEnabled = checkBoxDiscordRpc.Checked;
            _config.Save();
        }

        /// <summary>
        /// �ݒ�t�H���_�̃p�X���擾���܂��B
        /// </summary>
        private static string ConfigFolderPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

        /// <summary>
        /// OK�{�^���N���b�N���̏���
        /// </summary>
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            SaveSettings();

            StartupManager.SetStartupEnabled(
                _config.StartWithWindows,
                "LoL_Auto_Accepter",
                Application.ExecutablePath
            );

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// �L�����Z���{�^���N���b�N���̏���
        /// </summary>
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// �t�H���_�I���_�C�A���O��\�����܂��B
        /// </summary>
        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using var folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = textBoxLoLDir.Text
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxLoLDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// �ݒ�t�H���_���J���܂��B
        /// </summary>
        private void ButtonOpenConfigFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(ConfigFolderPath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", ConfigFolderPath);
                }
                else
                {
                    MessageBox.Show("�ݒ�t�H���_�����݂��܂���B", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�ݒ�t�H���_�̃I�[�v���Ɏ��s: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}