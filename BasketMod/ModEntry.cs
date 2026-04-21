using BasketMod.basket;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Menus;

namespace BasketMod;

public class ModEntry : Mod
{
    public static ModEntry Mod { get; private set; } = null!;

    /// <summary>
    /// For debugging purposes
    /// </summary>
    public Inventory RecoverInventory { get; private set; } = null!;

    public override void Entry(IModHelper helper)
    {
        Mod = this;
        helper.ConsoleCommands.Add("basket_open", "basket_open <id> <slotCapacity?> <itemCapacity?>,", BasketCommand);
        helper.ConsoleCommands.Add("basket_recover", "Menu that stores items in case something goes wrong",
            BasketRecoverCommand);
        helper.ConsoleCommands.Add("basket_list_inventories", "basket_list_inventories <all?> ",
            ListInventoriesCommand);
        RecoverInventory = new Inventory();
        helper.Events.GameLoop.Saving += OnSaving;

        void OnSaving(object? sender, SavingEventArgs e)
        {
            // Cleanup empty inventories
            var keys = from value in Game1.player.team.globalInventories.Keys
                where value.StartsWith(QualifiedName.InventoryIDPrefix) &&
                      Game1.player.team.globalInventories[value].IsEmpty()
                select value;
            foreach (var key in keys)
            {
                Game1.player.team.globalInventories.Remove(key);
            }
        }

        Harmony();
    }

    private void Harmony()
    {
        var harmony = new Harmony(ModManifest.UniqueID);
        BasketToolPatch.Apply(harmony);
    }

    private void BasketRecoverCommand(string command, string[] args)
    {
        Game1.activeClickableMenu = new ItemGrabMenu(RecoverInventory, RecoverInventory);
    }

    private static void BasketCommand(string command, string[] args)
    {
        var basketId = args.Length > 0 ? args[0] : "Example";
        var qualifiedBasketId =
            basketId.StartsWith("$") ? basketId[1..] : $"{QualifiedName.InventoryIDPrefix}.{basketId}";
        var slotCapacity = args.Length > 1 ? int.Parse(args[1]) : 36;
        var itemCapacity = args.Length > 2 ? int.Parse(args[2]) : int.MaxValue;
        var stackCapacity = args.Length > 3 ? int.Parse(args[3]) : int.MaxValue;
        var query = args.Length > 4 ? args[4] : "";
        var inventory =
            new BasketInventory(new BasketData(qualifiedBasketId, slotCapacity, itemCapacity, stackCapacity, query));
        inventory.ShowMenu();
    }

    private void ListInventoriesCommand(string command, string[] args)
    {
        var all = args.Length > 0 && (args[0].ToLower() == "all" || args[0].ToLower() == "true");
        foreach (var key in Game1.player.team.globalInventories.Keys)
        {
            if (key == null) continue;
            if (key.StartsWith(QualifiedName.InventoryIDPrefix) || all)
            {
                Monitor.Log($"Inventory: {key}", LogLevel.Info);
            }
        }
    }

    public static void Debug(string? message)
    {
#if DEBUG
        Mod.Monitor.Log(message ?? "<null>", LogLevel.Debug);
#endif
    }
}