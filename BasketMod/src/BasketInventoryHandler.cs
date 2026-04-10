using StardewValley;
using StardewValley.Inventories;
using StardewValley.Menus;
using StardewValley.Objects;

namespace BasketMod;

public class BasketInventoryHandler
{
    public static void ShowMenu(string basketId, int width)
    {
        var itemChest = new BasketChest(ToGlobalInventoryId(basketId), width * 2, itemCapacity:10);
        itemChest.ShowMenu();
    }

    public static string ToGlobalInventoryId(string basketId)
    {
        return $"{ModEntry.Mod.ModManifest.UniqueID}.{basketId}";
    }
}