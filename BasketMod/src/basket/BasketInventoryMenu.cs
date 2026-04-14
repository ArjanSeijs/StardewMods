using StardewValley;
using StardewValley.Menus;

namespace BasketMod.basket;

/// <summary>
/// ***Beware here be magic***
/// Some hacky logic to make a custom ItemGrabMenu.
/// </summary>
public class BasketInventoryMenu : ItemGrabMenu
{
    private readonly BasketInventory _basket;

    public static ItemGrabMenu Create(BasketInventory inventory)
    {
        var menu = new ItemGrabMenu(inventory,
            false,
            true,
            inventory.HighlightItems,
            inventory.GrabItemFromInventory,
            null,
            inventory.GrabItemFromChest,
            canBeExitedWithKey: true,
            showOrganizeButton: true, //Organize button overwrite the ItemsToGrabMenu
            source: 0,
            sourceItem: inventory.Source,
            context: inventory.Source);
        menu.RepositionSideButtons();
        //SetGrabMenu(menu, inventory);
        return menu;
    }

    /// <summary>
    /// Create a Menu for the inventory use <see cref="Create"/> to make a menu
    /// </summary>
    /// <param name="inventory"></param>
    private BasketInventoryMenu(BasketInventory inventory) : base(inventory,
        false,
        true,
        inventory.HighlightItems,
        inventory.GrabItemFromInventory,
        null,
        inventory.GrabItemFromChest,
        canBeExitedWithKey: true,
        showOrganizeButton: false, //Organize button overwrite the ItemsToGrabMenu
        source: 1,
        sourceItem: inventory.Source,
        context: inventory.Source)
    {
        _basket = inventory;
        //SetGrabMenu(this,_basket);
    }

    private static void SetGrabMenu(ItemGrabMenu grabMenu, BasketInventory basket)
    {
        var cols = basket.SlotCapacity < 9 ? basket.SlotCapacity : (basket.SlotCapacity - 1) / 3 + 1;
        var rows = basket.SlotCapacity < 9 ? 1 : 3;
        var grabMenuWidth = 64 * cols + 8;
        var xPosition = Game1.uiViewport.Width / 2 - (grabMenuWidth + 32) / 2;
        var yPosition = grabMenu.ItemsToGrabMenu.yPositionOnScreen - (basket.SlotCapacity < 9 ? 0 : 16);
        grabMenu.ItemsToGrabMenu = new InventoryMenu(xPosition, yPosition,
            grabMenu.ItemsToGrabMenu.playerInventory, grabMenu.ItemsToGrabMenu.actualInventory,
            grabMenu.ItemsToGrabMenu.highlightMethod, cols * rows, rows);
    }
}