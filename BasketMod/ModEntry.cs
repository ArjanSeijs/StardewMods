using SpaceCore.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Delegates;
using StardewValley.Triggers;

namespace BasketMod;

public class ModEntry : Mod
{
    public static ModEntry Mod { get; private set; } = null!;

    public override void Entry(IModHelper helper)
    {
        Monitor.Log("BasketMod loaded (WIP)", LogLevel.Info);
        Mod = this;
        TriggerActionManager.RegisterTrigger($"{ModManifest.UniqueID}_OnBasketUse");
        TriggerActionManager.RegisterAction($"{ModManifest.UniqueID}_OnBasketUse", BasketUse);
        helper.ConsoleCommands.Add("basket", "basket <slotCapacity> <itemCapacity>,", BasketCommand);
    }

    private void BasketCommand(string arg1, string[] arg2)
    {
        var slotCapacity = int.Parse(arg2[0]);
        var itemCapacity = arg2.Length > 1 ? int.Parse(arg2[1]) : int.MaxValue;
        var inventory = new BasketInventory($"{ModManifest.UniqueID}_Basket", null, slotCapacity, itemCapacity);
        inventory.ShowMenu();
    }

    private bool BasketUse(string[] args, TriggerActionContext context, out string error)
    {
        Monitor.Log("Basket Used!", LogLevel.Alert);
        error = "Nothing to do";
        return true;
    }
}