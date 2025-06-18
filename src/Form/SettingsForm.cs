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
        private readonly AppConfig _config;
        private List<ChampionItem> _championList = [];

        public SettingsForm(AppConfig config)
        {
            InitializeComponent();
            _config = config;
            LoadSettingsToUI();
        }

        /// <summary>
        /// UI�ɐݒ�l�����[�h���܂��B
        /// </summary>
        private void LoadSettingsToUI()
        {
            checkBoxAutoAccept.Checked = _config.AutoAcceptEnabled;
            numericUpDownDelay.Value = _config.AcceptDelaySeconds;
            checkBoxStartup.Checked = _config.StartWithWindows;
            textBoxLoLDir.Text = _config.LeagueOfLegendsDirectory;
            checkBoxAutoClose.Checked = _config.AutoCloseOnAccept;
            checkBoxDiscordRpc.Checked = _config.DiscordRpcEnabled;
            checkBoxAutoBan.Checked = _config.AutoBanEnabled;
            LoadChampionListToComboBoxes();
        }

        /// <summary>
        /// UI�̒l��ݒ�ɕۑ����܂��B
        /// </summary>
        private void SaveUIToSettings()
        {
            _config.AutoAcceptEnabled = checkBoxAutoAccept.Checked;
            _config.AcceptDelaySeconds = (int)numericUpDownDelay.Value;
            _config.StartWithWindows = checkBoxStartup.Checked;
            _config.LeagueOfLegendsDirectory = textBoxLoLDir.Text;
            _config.AutoCloseOnAccept = checkBoxAutoClose.Checked;
            _config.DiscordRpcEnabled = checkBoxDiscordRpc.Checked;
            _config.AutoBanEnabled = checkBoxAutoBan.Checked;

            _config.AutoBanChampionIdTop = GetSelectedChampionId(comboBoxAutoBanTop);
            _config.AutoBanChampionIdJungle = GetSelectedChampionId(comboBoxAutoBanJungle);
            _config.AutoBanChampionIdMid = GetSelectedChampionId(comboBoxAutoBanMid);
            _config.AutoBanChampionIdAdc = GetSelectedChampionId(comboBoxAutoBanAdc);
            _config.AutoBanChampionIdSupport = GetSelectedChampionId(comboBoxAutoBanSupport);

            _config.Save();
        }

        /// <summary>
        /// �w�肵��ComboBox����I�����ꂽChampionId���擾���܂��B
        /// </summary>
        private static int? GetSelectedChampionId(ComboBox comboBox)
            => comboBox.SelectedItem is ChampionItem item && item.Id != 0 ? item.Id : null;

        private static string ConfigFolderPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            SaveUIToSettings();

            StartupManager.SetStartupEnabled(
                _config.StartWithWindows,
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
            using var folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = textBoxLoLDir.Text
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxLoLDir.Text = folderBrowserDialog.SelectedPath;
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
                    MessageBox.Show("�ݒ�t�H���_�����݂��܂���B", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�ݒ�t�H���_�̃I�[�v���Ɏ��s: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// �`�����s�I�����X�g�����[�h���A�e���[����ComboBox�ɃZ�b�g���܂��B
        /// </summary>
        private void LoadChampionListToComboBoxes()
        {
            try
            {
                var path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "LAA",
                    "champion_list.json"
                );
                if (!File.Exists(path)) return;

                var json = File.ReadAllText(path);
                var list = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
                if (list == null) return;

                _championList = [.. list
                    .Where(x => x.ContainsKey("key") && x.ContainsKey("name"))
                    .Select(x => new ChampionItem
                    {
                        Id = int.Parse(x["key"]),
                        Name = x["name"]
                    })
                    .OrderBy(x => x.Name)];

                var empty = new ChampionItem { Id = 0, Name = "" };
                var dataSource = new List<ChampionItem> { empty };
                dataSource.AddRange(_championList);

                SetChampionComboBox(comboBoxAutoBanTop, dataSource, _config.AutoBanChampionIdTop);
                SetChampionComboBox(comboBoxAutoBanJungle, dataSource, _config.AutoBanChampionIdJungle);
                SetChampionComboBox(comboBoxAutoBanMid, dataSource, _config.AutoBanChampionIdMid);
                SetChampionComboBox(comboBoxAutoBanAdc, dataSource, _config.AutoBanChampionIdAdc);
                SetChampionComboBox(comboBoxAutoBanSupport, dataSource, _config.AutoBanChampionIdSupport);
            }
            catch (Exception ex)
            {
                Logger.Write("�`�����s�I�����X�g�Ǎ����s: " + ex.Message);
            }
        }

        /// <summary>
        /// �w�肵��ComboBox�Ƀ`�����s�I�����X�g���Z�b�g���A�I��l�𔽉f���܂��B
        /// </summary>
        private static void SetChampionComboBox(ComboBox comboBox, List<ChampionItem> dataSource, int? selectedId)
        {
            comboBox.DataSource = new List<ChampionItem>(dataSource);
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "Id";
            comboBox.SelectedValue = selectedId ?? 0;
        }
    }

    public class ChampionItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() => Name;
    }

}