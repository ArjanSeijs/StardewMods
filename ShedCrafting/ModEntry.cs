using HarmonyLib;
using ShedCrafting.Apis;
using ShedCrafting.integration;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Objects;

namespace ShedCrafting;

public class ModEntry : Mod
{
    public static ModEntry Mod { get; private set; } = null!;

    public IBetterCrafting? BetterCraftingAPI;

    public override void Entry(IModHelper helper)
    {
        Mod = this;
        helper.Events.Display.MenuChanged += OnMenuChanged;
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        Harmony();
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        BetterCraftingAPI = Helper.ModRegistry.GetApi<IBetterCrafting>("leclair.bettercrafting");
    }

    private void Harmony()
    {
        var harmony = new Harmony(ModManifest.UniqueID);
        harmony.Patch(
            original: AccessTools.Method(typeof(Workbench), nameof(Workbench.checkForAction)),
            postfix: new HarmonyMethod(typeof(ShedCrafting.WorkbenchListener),
                nameof(ShedCrafting.WorkbenchListener.CheckForAction_PostFix))
        );
    }


    private static void OnMenuChanged(object? sender, MenuChangedEventArgs e)
    {
        // Vanilla Menu
        ShedCrafting.OnMenuChanged(e);
        // Other Menu(s)
        BetterCrafting.OnMenuChanged(e);
    }
}