using StardewValley;
using StardewValley.Menus;

namespace BasketMod;

/// <summary>
/// Menu Magic
/// </summary>
public class BasketInventoryMenu : ItemGrabMenu
{
    public static BasketInventoryMenu Create(BasketInventory inventory)
    {
        var menu = new  BasketInventoryMenu(inventory);
        menu.RepositionSideButtons();
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
        showOrganizeButton: true,
        source: 1,
        sourceItem: inventory.Source,
        context: inventory.Source)
    {
        var cols = inventory.SlotCapacity < 9 ? inventory.SlotCapacity : (inventory.SlotCapacity - 1) / 3 + 1;
        var rows = inventory.SlotCapacity < 9 ? 1 : 3;
        var grabMenuWidth = 64 * cols + 8;
        var xPosition = Game1.uiViewport.Width / 2 - (grabMenuWidth + 32) / 2;
        var yPosition = ItemsToGrabMenu.yPositionOnScreen - (inventory.SlotCapacity < 9 ? 0 : 16);
        ItemsToGrabMenu = new InventoryMenu(xPosition, yPosition,
            ItemsToGrabMenu.playerInventory, ItemsToGrabMenu.actualInventory,
            ItemsToGrabMenu.highlightMethod, cols * rows, rows);
    }
}