namespace LoL_AutoAccept
{
    /// <summary>
    /// アプリケーション設定を編集するためのフォーム
    /// </summary>
    public partial class SettingsForm : Form
    {
        private readonly AppConfig config;

        /// <summary>
        /// 設定フォームを初期化します。
        /// </summary>
        public SettingsForm(AppConfig config)
        {
            InitializeComponent();
            this.config = config;
            LoadConfigToUI();
        }

        private void LoadConfigToUI()
        {
            checkBoxAutoAccept.Checked = config.AutoAcceptEnabled;
            numericUpDownDelay.Value = config.AcceptDelaySeconds;
            checkBoxStartup.Checked = config.StartWithWindows;
            textBoxLoLDir.Text = config.LeagueOfLegendsDirectory;
            checkBoxAutoClose.Checked = config.AutoCloseOnAccept;
        }

        private void SaveUIToConfig()
        {
            config.AutoAcceptEnabled = checkBoxAutoAccept.Checked;
            config.AcceptDelaySeconds = (int)numericUpDownDelay.Value;
            config.StartWithWindows = checkBoxStartup.Checked;
            config.LeagueOfLegendsDirectory = textBoxLoLDir.Text;
            config.AutoCloseOnAccept = checkBoxAutoClose.Checked;
            config.Save();
        }

        private static string ConfigFolderPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

        /// <summary>
        /// OKボタンクリック時の処理
        /// </summary>
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            SaveUIToConfig();

            StartupManager.SetStartupEnabled(
                config.StartWithWindows,
                "LoL_Auto_Accepter",
                Application.ExecutablePath
            );

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// キャンセルボタンクリック時の処理
        /// </summary>
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog
            {
                SelectedPath = textBoxLoLDir.Text
            };
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxLoLDir.Text = fbd.SelectedPath;
            }
        }

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
                    MessageBox.Show("設定フォルダが存在しません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("設定フォルダのオープンに失敗: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}