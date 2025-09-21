using LoLAutoAccepter.Models;
using LoLAutoAccepter.Utilities;
using System.Text.Json;

/// <summary>
/// �h���t�g�s�b�N���̎����s�b�N�������s���N���X
/// </summary>
public static class AutoPicker
{
    /// <summary>
    /// �w�肳�ꂽ�ݒ�ɏ]�������s�b�N�����s���܂��B
    /// </summary>
    public static async Task RunAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        if (!config.AutoPickEnabled) return;

        try
        {
            var sessionJson = await ChampSelectUtils.GetSessionJsonAsync(client, baseUrl, ct);
            if (sessionJson is null) return;

            using var doc = JsonDocument.Parse(sessionJson);

#if DEBUG
            int? mainPickId = config.AutoPickChampionIdJungle;
            int? subPickId = config.SubPickChampionIdJungle;
#else
            if (ChampSelectUtils.IsCustomGame(doc.RootElement)) return;

            var lane = ChampSelectUtils.GetLocalPlayerLane(doc.RootElement);
            if (lane is null) return;

            int? mainPickId = lane switch
            {
                "top" => config.AutoPickChampionIdTop,
                "jungle" => config.AutoPickChampionIdJungle,
                "middle" => config.AutoPickChampionIdMid,
                "bottom" => config.AutoPickChampionIdAdc,
                "utility" => config.AutoPickChampionIdSupport,
                _ => null
            };
            int? subPickId = lane switch
            {
                "top" => config.SubPickChampionIdTop,
                "jungle" => config.SubPickChampionIdJungle,
                "middle" => config.SubPickChampionIdMid,
                "bottom" => config.SubPickChampionIdAdc,
                "utility" => config.SubPickChampionIdSupport,
                _ => null
            };
#endif

            if (!mainPickId.HasValue && !subPickId.HasValue) return;

            var bannedIds = ChampSelectUtils.GetBannedChampionIds(doc.RootElement);
            var pickedIds = ChampSelectUtils.GetPickedChampionIds(doc.RootElement);

            int? pickId = null;
            if (mainPickId.HasValue && !bannedIds.Contains(mainPickId.Value) && !pickedIds.Contains(mainPickId.Value))
            {
                pickId = mainPickId;
            }
            else if (subPickId.HasValue && !bannedIds.Contains(subPickId.Value) && !pickedIds.Contains(subPickId.Value))
            {
                pickId = subPickId;
            }
            else
            {
                return;
            }

            if (!doc.RootElement.TryGetProperty("actions", out var actionsArray)) return;

            var pickAction = ChampSelectUtils.FindActionByType(actionsArray, ChampSelectUtils.GetLocalCellId(doc.RootElement), "pick");
            if (pickAction is null) return;

            await ChampSelectUtils.ExecuteActionAsync(client, baseUrl, pickAction.Value, pickId.Value, ct, "�����s�b�N");
        }
        catch (Exception ex)
        {
            Logger.Write($"AutoPicker�G���[: {ex}");
        }
    }
}