using BasketMod.basket;
using StardewValley;
using StardewValley.Menus;

namespace BasketMod;

/// <summary>
/// Beware here be magic
/// </summary>
public class BasketInventoryMenu : ItemGrabMenu
{
    private readonly BasketInventory _basket;

    public static BasketInventoryMenu Create(BasketInventory inventory)
    {
        var menu = new BasketInventoryMenu(inventory);
        return menu;
    }

    public BasketInventoryMenu(BasketInventory inventory) : base(inventory,
        false,
        true,
        inventory.HighlightItems,
        inventory.GrabItemFromInventory,
        null,
        inventory.GrabItemFromChest,
        canBeExitedWithKey: true,
        showOrganizeButton: inventory.StackCapacity >= 999,
        source: 1,
        sourceItem: inventory.Source,
        context: inventory.Source)
    {
        _basket = inventory;
        SetGrabMenu();
    }

    private void SetGrabMenu()
    {
        var cols = _basket.SlotCapacity < 9 ? _basket.SlotCapacity : (_basket.SlotCapacity - 1) / 3 + 1;
        var rows = _basket.SlotCapacity < 9 ? 1 : 3;
        var grabMenuWidth = 64 * cols + 8;
        var xPosition = Game1.uiViewport.Width / 2 - (grabMenuWidth + 32) / 2;
        var yPosition = ItemsToGrabMenu.yPositionOnScreen - (_basket.SlotCapacity < 9 ? 0 : 16);
        ItemsToGrabMenu = new InventoryMenu(xPosition, yPosition,
            ItemsToGrabMenu.playerInventory, ItemsToGrabMenu.actualInventory,
            ItemsToGrabMenu.highlightMethod, cols * rows, rows);
    }
}