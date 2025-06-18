using LoLAutoAccepter.Models;
using LoLAutoAccepter.Utilities;
using System.Text.Json;

namespace LoL_AutoAccept
{
    /// <summary>
    /// アプリケーション設定を編集するためのフォーム
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
        /// UIに設定値をロードします。
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
        /// UIの値を設定に保存します。
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
        /// 指定したComboBoxから選択されたChampionIdを取得します。
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
                    MessageBox.Show("設定フォルダが存在しません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"設定フォルダのオープンに失敗: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// チャンピオンリストをロードし、各レーンのComboBoxにセットします。
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
                Logger.Write("チャンピオンリスト読込失敗: " + ex.Message);
            }
        }

        /// <summary>
        /// 指定したComboBoxにチャンピオンリストをセットし、選択値を反映します。
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