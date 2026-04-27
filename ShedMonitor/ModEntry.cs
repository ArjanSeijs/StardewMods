using HarmonyLib;
using ShedMonitor.apis;
using ShedMonitor.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ShedMonitor;

// ReSharper disable once UnusedType.Global
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ModEntry : Mod
{
    public static ModEntry Mod { get; private set; } = null!;

    public IViewEngine ViewEngine = null!;

    /*********
     ** Public methods
     *********/
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        helper.Events.Input.ButtonPressed += OnButtonPressed;
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.Display.MenuChanged += OnMenuChanged;
        Mod = this;
        Harmony();
    }

    private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
    {
        WorkbenchTracer.OnMenuChanged(e);
    }

    private void Harmony()
    {
        var harmony = new Harmony(ModManifest.UniqueID);
        WorkbenchTracer.Patches.Apply(harmony);
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        ViewEngine = Helper.ModRegistry.GetApi<IViewEngine>("focustense.StardewUI")!;
        ViewEngine.RegisterViews($"Mods/{ModManifest.Name}/Views", "assets/views");
        ViewEngine.RegisterSprites($"Mods/{ModManifest.Name}/Sprites", "assets/sprites");
#if DEBUG
        ViewEngine.EnableHotReloadingWithSourceSync();
#endif

        Config.Register(this);
    }


    /*********
     ** Private methods
     *********/
    /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        // ignore if player hasn't loaded a save yet
        if (!Context.IsWorldReady || !Context.IsPlayerFree)
            return;

        // print button presses to the console window
        if (e.Button == Config.Instance.OpenKey)
            Game1.activeClickableMenu = StorageModel.CreateViewFromLocation(Game1.currentLocation);
    }
}