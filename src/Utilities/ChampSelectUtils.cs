using System.Text.Json;

namespace LoLAutoAccepter.Utilities
{
    /// <summary>
    /// �`�����s�I���Z���N�g�֘A�̋��ʃ��[�e�B���e�B
    /// </summary>
    public static class ChampSelectUtils
    {
        /// <summary>
        /// �Z�b�V��������JSON���擾���܂��B
        /// </summary>
        public static async Task<string?> GetSessionJsonAsync(HttpClient client, string baseUrl, CancellationToken ct)
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
        public static bool IsCustomGame(JsonElement root)
        {
            return root.TryGetProperty("isCustomGame", out var isCustomGameProp) && isCustomGameProp.GetBoolean();
        }

        /// <summary>
        /// ���[�J���v���C���[�̃Z��ID���擾���܂��B
        /// </summary>
        public static int GetLocalCellId(JsonElement sessionRoot)
        {
            return sessionRoot.TryGetProperty("localPlayerCellId", out var cellIdProp)
                ? cellIdProp.GetInt32()
                : -1;
        }

        /// <summary>
        /// �`�����s�I��ID����`�����s�I�������擾���܂��B
        /// </summary>
        public static string? GetChampionNameById(int? id)
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
        public static HashSet<int> GetBannedChampionIds(JsonElement root)
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

            if (root.TryGetProperty("actions", out var actionsArray))
            {
                foreach (var actionGroup in actionsArray.EnumerateArray())
                {
                    foreach (var action in actionGroup.EnumerateArray())
                    {
                        if (action.TryGetProperty("type", out var typeProp) &&
                            typeProp.GetString() == "ban" &&
                            action.TryGetProperty("championId", out var champIdProp))
                        {
                            int champId = champIdProp.GetInt32();
                            if (champId > 0)
                                banned.Add(champId);
                        }
                    }
                }
            }

            return banned;
        }

        /// <summary>
        /// ���łɃs�b�N����Ă���`�����s�I��ID�ꗗ���擾
        /// </summary>
        public static HashSet<int> GetPickedChampionIds(JsonElement root)
        {
            var picked = new HashSet<int>();
            if (root.TryGetProperty("myTeam", out var myTeam))
            {
                foreach (var player in myTeam.EnumerateArray())
                {
                    if (player.TryGetProperty("championId", out var champIdProp))
                    {
                        int champId = champIdProp.GetInt32();
                        if (champId > 0)
                            picked.Add(champId);
                    }
                }
            }
            if (root.TryGetProperty("theirTeam", out var theirTeam))
            {
                foreach (var player in theirTeam.EnumerateArray())
                {
                    if (player.TryGetProperty("championId", out var champIdProp))
                    {
                        int champId = champIdProp.GetInt32();
                        if (champId > 0)
                            picked.Add(champId);
                    }
                }
            }
            return picked;
        }

        /// <summary>
        /// ���[�J���v���C���[�̃��[�����擾���܂��B
        /// </summary>
        public static string? GetLocalPlayerLane(JsonElement sessionRoot)
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

        /// <summary>
        /// �w�肵���Z��ID�ƃA�N�V�����^�C�v�̃A�N�V�������������܂��B
        /// </summary>
        public static JsonElement? FindActionByType(JsonElement actionsArray, int localCellId, string actionType)
        {
            foreach (var actionGroup in actionsArray.EnumerateArray())
            {
                foreach (var action in actionGroup.EnumerateArray())
                {
                    if (action.GetProperty("type").GetString() == actionType &&
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
        /// �ėp�I�ȃA�N�V�������s���\�b�h�i�o���E�s�b�N���ʁj
        /// </summary>
        public static async Task ExecuteActionAsync(
            HttpClient client,
            string baseUrl,
            JsonElement action,
            int championId,
            CancellationToken ct,
            string logPrefix)
        {
            try
            {
                int id = action.GetProperty("id").GetInt32();
                var body = JsonSerializer.Serialize(new { championId, completed = true });
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var res = await client.PatchAsync($"{baseUrl}/lol-champ-select/v1/session/actions/{id}", content, ct);
                if (res.IsSuccessStatusCode)
                {
                    Logger.Write($"{logPrefix}: {GetChampionNameById(championId)}");
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{logPrefix}���s�G���[: {ex.Message}");
            }
        }
    }
}