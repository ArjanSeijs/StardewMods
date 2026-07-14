using System.Text;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace BasketMod.basket;

/// <summary>
/// Custom Basket Logic
/// </summary>
public static class BasketToolPatch
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: AccessTools.Method(typeof(Tool), nameof(Tool.DoFunction)),
            prefix: new HarmonyMethod(typeof(BasketToolPatch), nameof(DoFunction_Prefix))
        );
        harmony.Patch(
            original: AccessTools.Method(typeof(Item), nameof(Item.canBeTrashed)),
            prefix: new HarmonyMethod(typeof(BasketToolPatch), nameof(CanBeTrashed_Prefix))
        );
        harmony.Patch(
            original: AccessTools.Method(typeof(Tool), nameof(Tool.drawTooltip)),
            postfix: new HarmonyMethod(typeof(BasketToolPatch), nameof(DrawToolTip_PostFix))
        );
        harmony.Patch(
            original: AccessTools.Method(typeof(Tool), nameof(Tool.getExtraSpaceNeededForTooltipSpecialIcons)),
            postfix: new HarmonyMethod(typeof(BasketToolPatch), nameof(SpaceNeeded_PostFix))
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="location"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="power"></param>
    /// <param name="who"></param>
    /// <returns></returns>
    public static bool DoFunction_Prefix(Tool __instance, GameLocation location, int x, int y, int power, Farmer who)
    {
        try
        {
            if (Game1.player.UniqueMultiplayerID != who.UniqueMultiplayerID) return true;
            if (__instance.PerformSpecial(location, x, y, who)) return true;
            var basket = __instance.AsBasket();
            if (basket is null) return true;

            var inv = new BasketInventory(basket.Value);
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

    public static bool CanBeTrashed_Prefix(Tool __instance, ref bool __result)
    {
        try
        {
            var basket = __instance.AsBasket();
            if (basket is null) return true;
            __result = new BasketInventory(basket.Value).IsEmpty();
            return false;
        }
        catch (Exception e)
        {
            ModEntry.Mod.Monitor.Log(e.ToString(), LogLevel.Error);
            return true;
        }
    }

    private static int _errorCounter;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="spriteBatch"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="font"></param>
    /// <param name="alpha"></param>
    /// <param name="overrideText"></param>
    public static void DrawToolTip_PostFix(Tool __instance, SpriteBatch spriteBatch, ref int x, ref int y,
        SpriteFont font, float alpha, StringBuilder overrideText)
    {
        try
        {
            if (!ModEntry.Mod.Config.ItemBasketTooltip) return;
            var basket = __instance.AsBasket();
            if (basket is null) return;

            var xPos = x;
            var yPos = y + 16;
            Item? prevItem = null;

            foreach (var item in new BasketInventory(basket.Value).Take(7))
            {
                if (prevItem != null) xPos += item.DisplayName.Equals(prevItem.DisplayName) ? 16 : 32;

                item.drawInMenu(spriteBatch, new Vector2(xPos, yPos), 1f, 1f, 1f,
                    StackDrawType.HideButShowQuality, Color.White, false);


                prevItem = item;
            }
        }
        catch (Exception e)
        {
            if (_errorCounter < 20)
            {
                ModEntry.Mod.Monitor.Log("Failed DrawToolTip_PostFix for" + __instance?.QualifiedItemId ?? "<NULL>",
                    LogLevel.Error);
                ModEntry.Mod.Monitor.Log(e.ToString(), LogLevel.Error);
                _errorCounter++;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="__result"></param>
    /// <param name="font"></param>
    /// <param name="minWidth"></param>
    /// <param name="horizontalBuffer"></param>
    /// <param name="startingHeight"></param>
    /// <param name="descriptionText"></param>
    /// <param name="boldTitleText"></param>
    /// <param name="moneyAmountToDisplayAtBottom"></param>
    public static void SpaceNeeded_PostFix(Tool __instance, ref Point __result, SpriteFont font,
        int minWidth, int horizontalBuffer, int startingHeight, StringBuilder descriptionText,
        string boldTitleText, int moneyAmountToDisplayAtBottom)
    {
        try
        {
            if (!ModEntry.Mod.Config.ItemBasketTooltip) return;
            var basket = __instance.AsBasket();
            if (basket is null) return;
            if (new BasketInventory(basket.Value).Count > 0) __result += new Point(0, 64);
        }
        catch (Exception e)
        {
            if (_errorCounter < 20)
            {
                ModEntry.Mod.Monitor.Log("Failed SpaceNeeded_PostFix for" + __instance?.QualifiedItemId ?? "<NULL>",
                    LogLevel.Error);
                ModEntry.Mod.Monitor.Log(e.ToString(), LogLevel.Error);
                _errorCounter++;
            }
        }
    }
}