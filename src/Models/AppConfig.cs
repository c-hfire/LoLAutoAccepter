using LoLAutoAccepter.Utilities;
using System.Text.Json;

namespace LoLAutoAccepter.Models
{
    public class AppConfig
    {
        /// <summary>自動承諾機能の有効/無効</summary>
        public bool AutoAcceptEnabled { get; set; } = true;
        /// <summary>承諾までの遅延秒数</summary>
        public int AcceptDelaySeconds { get; set; } = 0;
        /// <summary>Windows起動時に自動起動するか</summary>
        public bool StartWithWindows { get; set; } = false;
        /// <summary>承諾後アプリを自動終了するか</summary>
        public bool AutoCloseOnAccept { get; set; } = false;
        /// <summary>Discord Rich Presenceの有効/無効</summary>
        public bool DiscordRpcEnabled { get; set; } = true;
        /// <summary>League of Legendsのインストールディレクトリ</summary>
        public string LeagueOfLegendsDirectory { get; set; } = @"C:\Riot Games\League of Legends";
        /// <summary>自動バン機能の有効/無効</summary>
        public bool AutoBanEnabled { get; set; } = false;
        /// <summary>自動バン対象チャンピオンID</summary>
        public int? AutoBanChampionId { get; set; }

        /// <summary>
        /// 設定ファイル保存ディレクトリのパスを取得します。
        /// </summary>
        private static string ConfigDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

        /// <summary>
        /// 設定ファイルのパスを取得します。
        /// </summary>
        private static string ConfigPath => Path.Combine(ConfigDir, "config.json");

        /// <summary>
        /// 設定ファイルを読み込みます。失敗時はデフォルト値を返します。
        /// </summary>
        /// <returns>AppConfigインスタンス</returns>
        public static AppConfig Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return new AppConfig();

                string json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();

                // 設定値のバリデーション
                config.AcceptDelaySeconds = Math.Clamp(config.AcceptDelaySeconds, 0, 10);

                return config;
            }
            catch (Exception ex)
            {
                Logger.Write($"設定ファイルの読み込みエラー: {ex.Message}");
                return new AppConfig();
            }
        }

        /// <summary>
        /// 設定ファイルを保存します。
        /// </summary>
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

        /// <summary>
        /// 設定ファイル保存ディレクトリを確認し、存在しない場合は作成します。
        /// </summary>
        private static void EnsureConfigDirectory()
        {
            if (!Directory.Exists(ConfigDir))
                Directory.CreateDirectory(ConfigDir);
        }
    }
}