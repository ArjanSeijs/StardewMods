using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Menus;
using StardewValley.Objects;

namespace ShedCrafting;

public static class ShedCrafting
{
    /// <summary>
    /// Set on workbenchInteraction, reset on menuChange.
    /// </summary>
    private static Workbench? _wbTracer;

    /// <summary>
    /// Get the chests from a location. Will Search in the following order:
    /// tileLocation, top, left, right
    /// </summary>
    /// <param name="tileLocation"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    public static IEnumerable<Chest> GetChests(Vector2 tileLocation, GameLocation location)
    {
        var building = location.getBuildingAt(tileLocation) ??
                       location.getBuildingAt(tileLocation + new Vector2(0, -1)) ??
                       location.getBuildingAt(tileLocation + new Vector2(-1, 0)) ??
                       location.getBuildingAt(tileLocation + new Vector2(1, 0));
        return building is not null ? building.GetIndoors().Objects.Values.OfType<Chest>() : Enumerable.Empty<Chest>();
    }

    /// <summary>
    /// <see cref="GetChests"/>
    /// </summary>
    /// <param name="tileLocation"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    public static IEnumerable<Inventory> GetInventories(Vector2 tileLocation, GameLocation location)
    {
        return from chest in GetChests(tileLocation, location) select chest.Items;
    }

    /// <summary>
    /// If this is a crafting page from a workbench
    /// </summary>
    /// <param name="e"></param>
    public static void OnMenuChanged(MenuChangedEventArgs e)
    {
        if (e.NewMenu is CraftingPage page && _wbTracer is not null)
        {
            var inventories = GetInventories(_wbTracer.TileLocation, _wbTracer.Location);
            page._materialContainers.AddRange(inventories);
        }

        _wbTracer = null;
    }


    // TODO: Think about shared lib
    public static class Patches
    {
        internal static void Apply(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Workbench), nameof(Workbench.checkForAction)),
                postfix: new HarmonyMethod(typeof(Patches), nameof(CheckForAction_PostFix))
            );
        }

        /// <summary>
        /// On Workbench Action, set Current to 
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <param name="who"></param>
        /// <param name="justCheckingForActivity"></param>
        // ReSharper disable once UnusedParameter.Global
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal static void CheckForAction_PostFix(Workbench __instance, ref bool __result, Farmer who,
            bool justCheckingForActivity)
        {
            try
            {
                if (justCheckingForActivity || !__result) return;
                ModEntry.Mod.Monitor.Log($"Instance: {__instance.Name} {__instance.TileLocation}");
                _wbTracer = __instance;
            }
            catch (Exception ex)
            {
                ModEntry.Mod.Monitor.Log($"Failed in {nameof(CheckForAction_PostFix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}