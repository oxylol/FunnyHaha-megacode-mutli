using HarmonyLib;
using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Economy;

namespace SR2MP.Patches.Economy;

[HarmonyPatch(typeof(PlayerState))]
public static class CurrencyPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerState.AddCurrency))]
    public static void AddCurrency(
        ICurrency currencyDefinition,
        int adjust,
        bool showUiNotification = true)
    {
        if (handlingPacket) return;

        var currency = currencyDefinition.PersistenceId;

        var packet = new CurrencyPacket(adjust, (byte)currency, showUiNotification);

        Main.SendToAllOrServer(packet);
    }
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerState.SpendCurrency))]
    public static void SpendCurrency(
        ICurrency currency,
        int adjust)
    {
        if (handlingPacket) return;

        var currencyId = currency.PersistenceId;

        var packet = new CurrencyPacket(-adjust, (byte)currencyId, true);
        Main.SendToAllOrServer(packet);
    }
}