using LoLAutoAccepter.Models;
using LoLAutoAccepter.Utilities;
using System.Text.Json;

namespace LoL_AutoAccept
{
    /// <summary>
    /// �A�v���P�[�V�����ݒ��ҏW���邽�߂̃t�H�[��
    /// </summary>
    public partial class SettingsForm : Form
    {
        // �����K������: �t�B�[���h���� _config ��
        private readonly AppConfig _config;

        private List<(string Key, string Name)> _championList = new();

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
            checkBoxAutoBan.Checked = _config.AutoBanEnabled;
            LoadChampionList();
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
            _config.AutoBanEnabled = checkBoxAutoBan.Checked;
            // Name����ID���擾���ĕۑ�
            var selectedName = comboBoxAutoBanChampion.SelectedItem as string;
            var champ = _championList.FirstOrDefault(x => x.Name == selectedName);
            _config.AutoBanChampionId = int.TryParse(champ.Key, out var id) ? id : (int?)null;
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

        /// <summary>
        /// �`�����s�I�����X�g�����[�h���܂��B
        /// </summary>
        private void LoadChampionList()
        {
            try
            {
                var path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "LAA",
                    "champion_list.json"
                );
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    var list = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
                    if (list != null)
                    {
                        _championList = [.. list
                            .Where(x => x.ContainsKey("key") && x.ContainsKey("name"))
                            .Select(x => (x["key"], x["name"]))
                            .OrderBy(x => x.Item2)];

                        comboBoxAutoBanChampion.Items.Clear();
                        comboBoxAutoBanChampion.Items.Add(""); // ��I��
                        comboBoxAutoBanChampion.Items.AddRange(_championList.Select(x => x.Name).ToArray());

                        // ������ID���疼�O���t�������đI��
                        if (_config.AutoBanChampionId.HasValue)
                        {
                            var champ = _championList.FirstOrDefault(x => int.Parse(x.Key) == _config.AutoBanChampionId.Value);
                            if (!string.IsNullOrEmpty(champ.Name))
                                comboBoxAutoBanChampion.SelectedItem = champ.Name;
                            else
                                comboBoxAutoBanChampion.SelectedItem = "";
                        }
                        else
                        {
                            comboBoxAutoBanChampion.SelectedItem = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("�`�����s�I�����X�g�Ǎ����s: " + ex.Message);
            }
        }
    }
}