using StardewModdingAPI;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Menus;
using StardewValley.Objects;

namespace BasketMod;

/// <summary>
/// 
/// </summary>
public class BasketChest : Chest
{
    private readonly int _rows;
    private readonly int _cols;
    private readonly int _itemCapacity;

    public IInventory Inventory => GetItemsForPlayer();

    public int ItemCount
    {
        get { return Inventory.Sum(item => item.stack.Value); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="globalInventoryId"></param>
    /// <param name="slotCapacity"></param> Amount of Item Slots. If >= 9 it will be Ceil(SlotCapacity/3)*3 instead to a max of 36
    /// <param name="itemCapacity"></param> Amount of Total Items. 
    /// <param name="playerChest"></param>
    public BasketChest(string globalInventoryId, int slotCapacity = 5, int itemCapacity = int.MaxValue,
        bool playerChest = true) : base(playerChest)
    {
        _itemCapacity = itemCapacity;
        slotCapacity = Math.Clamp(slotCapacity, 2, 12 * 3);
        if (slotCapacity < 9)
        {
            _rows = 1;
            _cols = slotCapacity;
        }
        else
        {
            _rows = 3;
            _cols = (slotCapacity - 1) / 3 + 1;
        }

        GlobalInventoryId = globalInventoryId;
    }

    public override int GetActualCapacity()
    {
        return _rows * _cols;
    }

    public ItemGrabMenu GetItemGrabMenu()
    {
        // Menu Magic
        var menu = new ItemGrabMenu(
            Inventory,
            false,
            true,
            HighlightItemsCapacity,
            GrabItemFromInventoryCapacity,
            null,
            GrabItemFromChestCapacity,
            canBeExitedWithKey: true,
            showOrganizeButton: true,
            source: 1,
            sourceItem: this,
            context: this);
        var width = 64 * _cols + 8;
        var uiViewportWidth = Game1.uiViewport.Width / 2 - (width + 32) / 2;
        var yPositionOnScreen = menu.ItemsToGrabMenu.yPositionOnScreen - 16;
        menu.ItemsToGrabMenu.width = width;
        menu.ItemsToGrabMenu.SetPosition(uiViewportWidth, yPositionOnScreen);
        menu.RepositionSideButtons();
        return menu;
    }

    private void GrabItemFromInventoryCapacity(Item item, Farmer who)
    {
        if (item.Stack <= _itemCapacity - ItemCount)
        {
            grabItemFromInventory(item, who);
            return;
        }

        var itemToInsert = item.getOne();
        var itemToReturn = item.getOne();
        itemToInsert.Stack = Math.Clamp(item.Stack, 0, Math.Max(_itemCapacity - ItemCount, 0));
        itemToReturn.Stack = item.Stack - itemToInsert.Stack;
        grabItemFromInventory(itemToInsert, who);

        if (itemToReturn.Stack > 0)
        {
            who.addItemToInventory(itemToReturn); // TODO Better Way?
        }
    }

    private void GrabItemFromChestCapacity(Item item, Farmer who)
    {
        ModEntry.Mod.Monitor.Log($"{item.QualifiedItemId}:{item.Stack}", LogLevel.Debug);
        grabItemFromChest(item, who);
    }

    private bool HighlightItemsCapacity(Item i)
    {
        // if item is basket return false;
        if (GetItemsForPlayer().Contains(i)) return true;
        return ItemCount < _itemCapacity;
    }

    public override void ShowMenu()
    {
        Game1.activeClickableMenu = GetItemGrabMenu();
    }
}