using LoLAutoAccepter.Models;
using LoLAutoAccepter.Utilities;
using System.Text.Json;

/// <summary>
/// ドラフトピック時の自動バン処理を行うクラス
/// </summary>
public static class AutoBanner
{
    /// <summary>
    /// 指定された設定に従い自動バンを実行します。
    /// </summary>
    /// <param name="client">HttpClient</param>
    /// <param name="baseUrl">APIのベースURL</param>
    /// <param name="config">アプリ設定</param>
    /// <param name="ct">キャンセルトークン</param>
    public static async Task RunAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        if (!config.AutoBanEnabled) return;
        if (!config.AutoBanChampionId.HasValue) return;

        int championId = config.AutoBanChampionId.Value;

        try
        {
            var res = await client.GetAsync($"{baseUrl}/lol-champ-select/v1/session", ct);
            if (!res.IsSuccessStatusCode) return;

            var json = await res.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("actions", out var actionsArray)) return;

            foreach (var actionGroup in actionsArray.EnumerateArray())
            {
                foreach (var action in actionGroup.EnumerateArray())
                {
                    if (action.GetProperty("type").GetString() is "ban" &&
                        action.GetProperty("actorCellId").GetInt32() == GetLocalCellId(doc.RootElement) &&
                        action.GetProperty("isInProgress").GetBoolean())
                    {
                        int id = action.GetProperty("id").GetInt32();
                        var banBody = JsonSerializer.Serialize(new { championId = championId, completed = true });
                        var content = new StringContent(banBody, System.Text.Encoding.UTF8, "application/json");
                        var banRes = await client.PatchAsync($"{baseUrl}/lol-champ-select/v1/session/actions/{id}", content, ct);
                        if (banRes.IsSuccessStatusCode)
                        {
                            Logger.Write($"自動バン: {GetChampionNameById(championId)}");
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"AutoBannerエラー: {ex.Message}");
        }
    }

    /// <summary>
    /// ローカルプレイヤーのセルIDを取得します。
    /// </summary>
    /// <param name="sessionRoot">セッションのルート要素</param>
    /// <returns>ローカルプレイヤーのセルID</returns>
    private static int GetLocalCellId(JsonElement sessionRoot)
    {
        return sessionRoot.TryGetProperty("localPlayerCellId", out var cellIdProp)
            ? cellIdProp.GetInt32()
            : -1;
    }

    /// <summary>
    /// チャンピオンIDからチャンピオン名を取得します。
    /// </summary>
    /// <param name="id">チャンピオンID</param>
    /// <returns>チャンピオン名</returns>
    private static string? GetChampionNameById(int? id)
    {
        if (!id.HasValue) return null;
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
                var champ = list?.FirstOrDefault(x => x.TryGetValue("key", out var keyStr) && int.TryParse(keyStr, out var champId) && champId == id.Value);
                if (champ != null && champ.TryGetValue("name", out var name))
                    return name;
            }
        }
        catch { }
        return null;
    }
}