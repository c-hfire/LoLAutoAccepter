using System.Windows.Forms;
using System.IO;

namespace LoL_AutoAccept
{
    /// <summary>
    /// アプリケーション設定を編集するためのフォーム
    /// </summary>
    public partial class SettingsForm : Form
    {
        private readonly AppConfig config;

        public SettingsForm(AppConfig config)
        {
            InitializeComponent();
            this.config = config;

            // 初期値をUIに反映
            checkBoxAutoAccept.Checked = config.AutoAcceptEnabled;
            numericUpDownDelay.Value = config.AcceptDelaySeconds;
            checkBoxStartup.Checked = config.StartWithWindows;
            textBoxLoLDir.Text = config.LeagueOfLegendsDirectory;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            // UIの値を設定に反映
            config.AutoAcceptEnabled = checkBoxAutoAccept.Checked;
            config.AcceptDelaySeconds = (int)numericUpDownDelay.Value;
            config.StartWithWindows = checkBoxStartup.Checked;
            config.LeagueOfLegendsDirectory = textBoxLoLDir.Text;
            config.Save();

            // スタートアップ設定を反映
            StartupManager.SetStartupEnabled(
                config.StartWithWindows,
                "LoL_Auto_Accepter",
                Application.ExecutablePath
            );

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = textBoxLoLDir.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxLoLDir.Text = fbd.SelectedPath;
            }
        }

        private void ButtonOpenConfigFolder_Click(object sender, EventArgs e)
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