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
    /// <param name="client">HttpClient</param>
    /// <param name="baseUrl">API�̃x�[�XURL</param>
    /// <param name="config">�A�v���ݒ�</param>
    /// <param name="ct">�L�����Z���g�[�N��</param>
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
                            Logger.Write($"�����o��: {GetChampionNameById(championId)}");
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"AutoBanner�G���[: {ex.Message}");
        }
    }

    /// <summary>
    /// ���[�J���v���C���[�̃Z��ID���擾���܂��B
    /// </summary>
    /// <param name="sessionRoot">�Z�b�V�����̃��[�g�v�f</param>
    /// <returns>���[�J���v���C���[�̃Z��ID</returns>
    private static int GetLocalCellId(JsonElement sessionRoot)
    {
        return sessionRoot.TryGetProperty("localPlayerCellId", out var cellIdProp)
            ? cellIdProp.GetInt32()
            : -1;
    }

    /// <summary>
    /// �`�����s�I��ID����`�����s�I�������擾���܂��B
    /// </summary>
    /// <param name="id">�`�����s�I��ID</param>
    /// <returns>�`�����s�I����</returns>
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