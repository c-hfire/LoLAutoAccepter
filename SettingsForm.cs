using System.Windows.Forms;
using System.IO;

namespace LoL_AutoAccept
{
    /// <summary>
    /// �A�v���P�[�V�����ݒ��ҏW���邽�߂̃t�H�[��
    /// </summary>
    public partial class SettingsForm : Form
    {
        private readonly AppConfig config;

        public SettingsForm(AppConfig config)
        {
            InitializeComponent();
            this.config = config;

            // �����l��UI�ɔ��f
            checkBoxAutoAccept.Checked = config.AutoAcceptEnabled;
            numericUpDownDelay.Value = config.AcceptDelaySeconds;
            checkBoxStartup.Checked = config.StartWithWindows;
            textBoxLoLDir.Text = config.LeagueOfLegendsDirectory;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            // UI�̒l��ݒ�ɔ��f
            config.AutoAcceptEnabled = checkBoxAutoAccept.Checked;
            config.AcceptDelaySeconds = (int)numericUpDownDelay.Value;
            config.StartWithWindows = checkBoxStartup.Checked;
            config.LeagueOfLegendsDirectory = textBoxLoLDir.Text;
            config.Save();

            // �X�^�[�g�A�b�v�ݒ�𔽉f
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
                    MessageBox.Show("�ݒ�t�H���_�����݂��܂���B", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�ݒ�t�H���_�̃I�[�v���Ɏ��s: " + ex.Message, "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}