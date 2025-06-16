using System.Text.Json;

public static class AutoBanner
{
    /// <summary>
    /// バンフェーズで自動バンを行う
    /// </summary>
    public static async Task RunAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        if (!config.AutoBanEnabled) return;

        int? championId = GetChampionIdByName(config.AutoBanChampionName);
        if (championId == null) return;

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
                        var banBody = JsonSerializer.Serialize(new { championId = championId.Value, completed = true });
                        var content = new StringContent(banBody, System.Text.Encoding.UTF8, "application/json");
                        var banRes = await client.PatchAsync($"{baseUrl}/lol-champ-select/v1/session/actions/{id}", content, ct);
                        if (banRes.IsSuccessStatusCode)
                        {
                            Logger.Write($"自動バン: {config.AutoBanChampionName}");
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

    private static int GetLocalCellId(JsonElement sessionRoot)
    {
        return sessionRoot.TryGetProperty("localPlayerCellId", out var cellIdProp)
            ? cellIdProp.GetInt32()
            : -1;
    }

    private static int? GetChampionIdByName(string? name)
    {
        if (string.IsNullOrEmpty(name)) return null;
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
                var champ = list?.FirstOrDefault(x => x.TryGetValue("name", out var n) && n == name);
                if (champ != null && champ.TryGetValue("key", out var keyStr) && int.TryParse(keyStr, out var id))
                    return id;
            }
        }
        catch { }
        return null;
    }
}