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
    public static async Task RunAsync(HttpClient client, string baseUrl, AppConfig config, CancellationToken ct)
    {
        if (!config.AutoBanEnabled) return;

        try
        {
            var sessionJson = await ChampSelectUtils.GetSessionJsonAsync(client, baseUrl, ct);
            if (sessionJson is null) return;

            using var doc = JsonDocument.Parse(sessionJson);

            if (ChampSelectUtils.IsCustomGame(doc.RootElement)) return;

            var lane = ChampSelectUtils.GetLocalPlayerLane(doc.RootElement);
            if (lane is null) return;

            int? banId = lane switch
            {
                "top" => config.AutoBanChampionIdTop,
                "jungle" => config.AutoBanChampionIdJungle,
                "middle" => config.AutoBanChampionIdMid,
                "bottom" => config.AutoBanChampionIdAdc,
                "utility" => config.AutoBanChampionIdSupport,
                _ => null
            };

            if (!banId.HasValue) return;

            var bannedChampionIds = ChampSelectUtils.GetBannedChampionIds(doc.RootElement);
            if (bannedChampionIds.Contains(banId.Value)) return;

            if (!doc.RootElement.TryGetProperty("actions", out var actionsArray)) return;

            var banAction = ChampSelectUtils.FindActionByType(actionsArray, ChampSelectUtils.GetLocalCellId(doc.RootElement), "ban");
            if (banAction is null) return;

            await ChampSelectUtils.ExecuteActionAsync(client, baseUrl, banAction.Value, banId.Value, ct, "自動バン");
        }
        catch (Exception ex)
        {
            Logger.Write($"AutoBannerエラー: {ex}");
        }
    }
}