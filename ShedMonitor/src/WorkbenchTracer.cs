using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using ShedMonitor.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;

namespace ShedMonitor;

public class WorkbenchTracer
{
    private static Workbench? _wbTracer;


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

    public static void OnMenuChanged(MenuChangedEventArgs e)
    {
        _wbTracer = null;
    }
}