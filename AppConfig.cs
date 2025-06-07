using System.Text.Json;

public class AppConfig
{
    public bool AutoAcceptEnabled { get; set; } = true;
    public int AcceptDelaySeconds { get; set; } = 2;
    public bool StartWithWindows { get; set; } = false;
    public string LeagueOfLegendsDirectory { get; set; } = @"C:\Riot Games\League of Legends";

    private static readonly string ConfigDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAA");

    private static readonly string ConfigPath = Path.Combine(ConfigDir, "config.json");

    public static AppConfig Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                string json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
        }
        catch (Exception ex)
        {
            Logger.Write("設定ファイルの読み込みエラー: " + ex.Message);
        }

        return new AppConfig();
    }

    public void Save()
    {
        try
        {
            if (!Directory.Exists(ConfigDir))
                Directory.CreateDirectory(ConfigDir);

            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }
        catch (Exception ex)
        {
            Logger.Write("設定ファイルの保存エラー: " + ex.Message);
        }
    }
}
