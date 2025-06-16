namespace LoL_AutoAccept
{
    /// <summary>
    /// アプリケーション設定を編集するためのフォーム
    /// </summary>
    public partial class SettingsForm : Form
    {
        // 命名規則統一: フィールド名を _config に
        private readonly AppConfig _config;

        /// <summary>
        /// 設定フォームを初期化します。
        /// </summary>
        /// <param name="config">アプリケーション設定</param>
        public SettingsForm(AppConfig config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
        }

        /// <summary>
        /// UIに設定値をロードします。
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
        /// UIの値を設定に保存します。
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
        /// 設定フォルダのパスを取得します。
        /// </summary>
        private static string ConfigFolderPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

        /// <summary>
        /// OKボタンクリック時の処理
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
        /// キャンセルボタンクリック時の処理
        /// </summary>
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// フォルダ選択ダイアログを表示します。
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
        /// 設定フォルダを開きます。
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
                    MessageBox.Show("設定フォルダが存在しません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"設定フォルダのオープンに失敗: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}