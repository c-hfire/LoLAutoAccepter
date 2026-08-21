using LoLAutoAccepter.Utilities;
using System.Text.Json;

namespace LoLAutoAccepter.Models
{
    public class AppConfig
    {
        /// <summary>自動承諾を有効にするか</summary>
        public bool AutoAcceptEnabled { get; set; } = true;
        /// <summary>承諾までの遅延（秒）</summary>
        public int AcceptDelaySeconds { get; set; } = 0;
        /// <summary>Windows 起動時に自動起動するか</summary>
        public bool StartWithWindows { get; set; } = true;
        /// <summary>承諾後にアプリを自動終了するか</summary>
        public bool AutoCloseOnAccept { get; set; } = false;
        /// <summary>Discord Rich Presence の有効/無効</summary>
        public bool DiscordRpcEnabled { get; set; } = true;
        /// <summary>League of Legends のインストール先ディレクトリ</summary>
        public string LeagueOfLegendsDirectory { get; set; } = @"C:\Riot Games\League of Legends";
        /// <summary>自動バンを有効にするか</summary>
        public bool AutoBanEnabled { get; set; } = false;
        /// <summary>自動バン対象チャンピオン ID（Top）</summary>
        public int? AutoBanChampionIdTop { get; set; }
        /// <summary>自動バン対象チャンピオン ID（Jungle）</summary>
        public int? AutoBanChampionIdJungle { get; set; }
        /// <summary>自動バン対象チャンピオン ID（Mid）</summary>
        public int? AutoBanChampionIdMid { get; set; }
        /// <summary>自動バン対象チャンピオン ID（ADC）</summary>
        public int? AutoBanChampionIdAdc { get; set; }
        /// <summary>自動バン対象チャンピオン ID（Support）</summary>
        public int? AutoBanChampionIdSupport { get; set; }
        /// <summary>自動ピックを有効にするか</summary>
        public bool AutoPickEnabled { get; set; } = false;
        /// <summary>自動ピック対象チャンピオン ID（Top）</summary>
        public int? AutoPickChampionIdTop { get; set; }
        public int? SubPickChampionIdTop { get; set; }
        /// <summary>自動ピック対象チャンピオン ID（Jungle）</summary>
        public int? AutoPickChampionIdJungle { get; set; }
        public int? SubPickChampionIdJungle { get; set; }
        /// <summary>自動ピック対象チャンピオン ID（Mid）</summary>
        public int? AutoPickChampionIdMid { get; set; }
        public int? SubPickChampionIdMid { get; set; }
        /// <summary>自動ピック対象チャンピオン ID（ADC）</summary>
        public int? AutoPickChampionIdAdc { get; set; }
        public int? SubPickChampionIdAdc { get; set; }
        /// <summary>自動ピック対象チャンピオン ID（Support）</summary>
        public int? AutoPickChampionIdSupport { get; set; }
        public int? SubPickChampionIdSupport { get; set; }

        /// <summary>設定ファイル保存先のディレクトリパス</summary>
        private static string ConfigDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

        /// <summary>設定ファイルのパス</summary>
        private static string ConfigPath => Path.Combine(ConfigDir, "config.json");

        /// <summary>設定ファイルを読み込みます。失敗時は既定値を返します。</summary>
        public static AppConfig Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return new AppConfig();

                string json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();

                config.AcceptDelaySeconds = Math.Clamp(config.AcceptDelaySeconds, 0, 10);

                return config;
            }
            catch (Exception ex)
            {
                Logger.Write($"設定ファイルの読み込みエラー: {ex.Message}");
                return new AppConfig();
            }
        }

        /// <summary>設定をファイルに保存します</summary>
        public void Save()
        {
            try
            {
                EnsureConfigDirectory();
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                Logger.Write($"設定ファイルの保存エラー: {ex.Message}");
            }
        }

        /// <summary>設定保存ディレクトリを作成します（存在しない場合）</summary>
        private static void EnsureConfigDirectory()
        {
            if (!Directory.Exists(ConfigDir))
                Directory.CreateDirectory(ConfigDir);
        }
    }
}