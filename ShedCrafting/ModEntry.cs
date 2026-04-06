using HarmonyLib;
using ShedCrafting.Apis;
using ShedCrafting.integration;
using StardewModdingAPI;
using StardewModdingAPI.Events;

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
        ShedCrafting.Patches.Apply(harmony);
    }


    private static void OnMenuChanged(object? sender, MenuChangedEventArgs e)
    {
        // Vanilla Menu
        ShedCrafting.OnMenuChanged(e);
        // Other Menu(s)
        BetterCrafting.OnMenuChanged(e);
    }
}