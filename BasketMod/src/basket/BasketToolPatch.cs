using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace BasketMod.basket;

public static class BasketToolPatch
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: AccessTools.Method(typeof(Tool), nameof(Tool.DoFunction)),
            prefix: new HarmonyMethod(typeof(BasketToolPatch), nameof(DoFunction_Prefix))
        );
    }

    public static bool DoFunction_Prefix(Tool __instance, GameLocation location, int x, int y, int power, Farmer who)
    {
        try
        {
            if (!__instance.AsBasket(out var basket)) return true;

            var inv = BasketInventory.Create(basket.Value);
            inv.ShowMenu();
            Game1.playSound("tinyWhip");
            return false;
        }
        catch (Exception e)
        {
            ModEntry.Mod.Monitor.Log(e.ToString(), LogLevel.Error);
            return true;
        }
    }
}