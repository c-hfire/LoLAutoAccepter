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
        // 命名規則統一: フィールド名を _config に
        private readonly AppConfig _config;

        private List<(string Key, string Name)> _championList = new();

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
            checkBoxAutoBan.Checked = _config.AutoBanEnabled;
            LoadChampionList();
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
            _config.AutoBanEnabled = checkBoxAutoBan.Checked;
            // NameからIDを取得して保存
            var selectedName = comboBoxAutoBanChampion.SelectedItem as string;
            var champ = _championList.FirstOrDefault(x => x.Name == selectedName);
            _config.AutoBanChampionId = int.TryParse(champ.Key, out var id) ? id : (int?)null;
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

        /// <summary>
        /// チャンピオンリストをロードします。
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
                        comboBoxAutoBanChampion.Items.Add(""); // 空選択
                        comboBoxAutoBanChampion.Items.AddRange(_championList.Select(x => x.Name).ToArray());

                        // ここでIDから名前を逆引きして選択
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
                Logger.Write("チャンピオンリスト読込失敗: " + ex.Message);
            }
        }
    }
}