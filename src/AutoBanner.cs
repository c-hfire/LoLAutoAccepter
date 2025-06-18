using LoLAutoAccepter.Models;
using LoLAutoAccepter.Utilities;
using System.Text.Json;

/// <summary>
/// �h���t�g�s�b�N���̎����o���������s���N���X
/// </summary>
public static class AutoBanner
{
    /// <summary>
    /// �w�肳�ꂽ�ݒ�ɏ]�������o�������s���܂��B
    /// </summary>
    public static async Task RunAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        if (!config.AutoBanEnabled) return;

        try
        {
            var sessionJson = await GetSessionJsonAsync(client, baseUrl, ct);
            if (sessionJson is null) return;

            using var doc = JsonDocument.Parse(sessionJson);

            if (IsCustomGame(doc.RootElement)) return;

            var lane = GetLocalPlayerLane(doc.RootElement);
            if (lane is null) return;

            int? banId = lane switch
            {
                "TOP" => config.AutoBanChampionIdTop,
                "JUNGLE" => config.AutoBanChampionIdJungle,
                "MIDDLE" or "MID" => config.AutoBanChampionIdMid,
                "BOTTOM" or "ADC" => config.AutoBanChampionIdAdc,
                "UTILITY" or "SUPPORT" => config.AutoBanChampionIdSupport,
                _ => null
            };

            if (!banId.HasValue) return;

            var bannedChampionIds = GetBannedChampionIds(doc.RootElement);
            if (bannedChampionIds.Contains(banId.Value)) return;

            if (!doc.RootElement.TryGetProperty("actions", out var actionsArray)) return;

            var banAction = FindBanAction(actionsArray, GetLocalCellId(doc.RootElement));
            if (banAction is null) return;

            await ExecuteBanAsync(client, baseUrl, banAction.Value, banId.Value, ct);
        }
        catch (Exception ex)
        {
            Logger.Write($"AutoBanner�G���[: {ex}");
        }
    }

    /// <summary>
    /// �Z�b�V��������JSON���擾���܂��B
    /// </summary>
    private static async Task<string?> GetSessionJsonAsync(HttpClient client, string baseUrl, CancellationToken ct)
    {
        try
        {
            var res = await client.GetAsync($"{baseUrl}/lol-champ-select/v1/session", ct);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadAsStringAsync(ct);
        }
        catch (Exception ex)
        {
            Logger.Write($"�Z�b�V�����擾�G���[: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// �J�X�^���Q�[�����ǂ����𔻒肵�܂��B
    /// </summary>
    private static bool IsCustomGame(JsonElement root)
    {
        return root.TryGetProperty("isCustomGame", out var isCustomGameProp) && isCustomGameProp.GetBoolean();
    }

    /// <summary>
    /// �w�肵���Z��ID�̃o���A�N�V�������������܂��B
    /// </summary>
    private static JsonElement? FindBanAction(JsonElement actionsArray, int localCellId)
    {
        foreach (var actionGroup in actionsArray.EnumerateArray())
        {
            foreach (var action in actionGroup.EnumerateArray())
            {
                if (action.GetProperty("type").GetString() is "ban" &&
                    action.GetProperty("actorCellId").GetInt32() == localCellId &&
                    action.GetProperty("isInProgress").GetBoolean())
                {
                    return action;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// �o���A�N�V���������s���܂��B
    /// </summary>
    private static async Task ExecuteBanAsync(HttpClient client, string baseUrl, JsonElement action, int championId, CancellationToken ct)
    {
        try
        {
            int id = action.GetProperty("id").GetInt32();
            var banBody = JsonSerializer.Serialize(new { championId, completed = true });
            var content = new StringContent(banBody, System.Text.Encoding.UTF8, "application/json");
            var banRes = await client.PatchAsync($"{baseUrl}/lol-champ-select/v1/session/actions/{id}", content, ct);
            if (banRes.IsSuccessStatusCode)
            {
                Logger.Write($"�����o��: {GetChampionNameById(championId)}");
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"�o�����s�G���[: {ex.Message}");
        }
    }

    /// <summary>
    /// ���[�J���v���C���[�̃Z��ID���擾���܂��B
    /// </summary>
    private static int GetLocalCellId(JsonElement sessionRoot)
    {
        return sessionRoot.TryGetProperty("localPlayerCellId", out var cellIdProp)
            ? cellIdProp.GetInt32()
            : -1;
    }

    /// <summary>
    /// �`�����s�I��ID����`�����s�I�������擾���܂��B
    /// </summary>
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
            if (!File.Exists(path)) return null;

            var json = File.ReadAllText(path);
            var list = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
            if (list is null) return null;

            var champ = list.FirstOrDefault(x =>
                x.TryGetValue("key", out var keyStr) &&
                int.TryParse(keyStr, out var champId) &&
                champId == id.Value);

            if (champ != null && champ.TryGetValue("name", out var name))
                return name;
        }
        catch (Exception ex)
        {
            Logger.Write($"�`�����s�I�����擾�G���[: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// ���łɃo������Ă���`�����s�I��ID�ꗗ���擾
    /// </summary>
    private static HashSet<int> GetBannedChampionIds(JsonElement root)
    {
        var banned = new HashSet<int>();
        if (root.TryGetProperty("bans", out var bansProp) &&
            bansProp.TryGetProperty("myTeamBans", out var myTeamBans) &&
            bansProp.TryGetProperty("theirTeamBans", out var theirTeamBans))
        {
            foreach (var ban in myTeamBans.EnumerateArray())
                if (ban.ValueKind == JsonValueKind.Number && ban.GetInt32() > 0)
                    banned.Add(ban.GetInt32());
            foreach (var ban in theirTeamBans.EnumerateArray())
                if (ban.ValueKind == JsonValueKind.Number && ban.GetInt32() > 0)
                    banned.Add(ban.GetInt32());
        }
        return banned;
    }

    /// <summary>
    /// ���[�J���v���C���[�̃��[�����擾���܂��B
    /// </summary>
    private static string? GetLocalPlayerLane(JsonElement sessionRoot)
    {
        if (!sessionRoot.TryGetProperty("myTeam", out var myTeam) ||
            !sessionRoot.TryGetProperty("localPlayerCellId", out var cellIdProp))
            return null;

        int localCellId = cellIdProp.GetInt32();
        foreach (var player in myTeam.EnumerateArray())
        {
            if (player.TryGetProperty("cellId", out var cellId) && cellId.GetInt32() == localCellId)
            {
                if (player.TryGetProperty("assignedPosition", out var laneProp))
                    return laneProp.GetString();
            }
        }
        return null;
    }
}