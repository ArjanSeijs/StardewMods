using System.Collections;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Menus;

namespace BasketMod;

public class BasketInventory : IInventory
{
    public Item Source { get; }
    public int SlotCapacity { get; }
    public int ItemCapacity { get; }

    public IInventory Inventory => Game1.player.team.GetOrCreateGlobalInventory(GlobalInventoryID);

    public string GlobalInventoryID { get; }

    public int ItemCount => Inventory.Sum(item => item.stack.Value);

    public int SpaceLeft => Math.Max(0, ItemCapacity - ItemCount);

    public BasketInventory(string globalInventoryId, Item source, int slotCapacity = 9, int itemCapacity = int.MaxValue)
    {
        GlobalInventoryID = globalInventoryId;
        Source = source;
        SlotCapacity = Math.Clamp(slotCapacity, 2, 12 * 3);
        ItemCapacity = itemCapacity;
    }

    /// <summary>
    /// <see cref="StardewValley.Objects.Chest.grabItemFromChest"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="who"></param>
    public void GrabItemFromChest(Item item, Farmer who)
    {
        if (!who.couldInventoryAcceptThisItem(item))
            return;
        Inventory.Remove(item);
        Inventory.RemoveEmptySlots();
        ShowMenu();
    }

    /// <summary>
    /// <see cref="StardewValley.Objects.Chest.grabItemFromInventory"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="who"></param>
    public virtual void GrabItemFromInventory(Item item, Farmer who)
    {
        ModEntry.Mod.Monitor.Log($"Grabbing {item.QualifiedItemId} : {item.Stack}", LogLevel.Debug);
        if (item.Stack == 0)
            item.Stack = 1;
        var obj = this.AddItem(item);
        if (obj == null)
            who.removeItemFromInventory(item);
        else
            obj = who.addItemToInventory(obj);
        Inventory.RemoveEmptySlots();
        var id = Game1.activeClickableMenu.currentlySnappedComponent != null
            ? Game1.activeClickableMenu.currentlySnappedComponent.myID
            : -1;
        ShowMenu();
        (Game1.activeClickableMenu as ItemGrabMenu)!.heldItem = obj;
        if (id == -1)
            return;
        Game1.activeClickableMenu.currentlySnappedComponent = Game1.activeClickableMenu.getComponentWithID(id);
        Game1.activeClickableMenu.snapCursorToCurrentSnappedComponent();
        ModEntry.Mod.Monitor.Log($"Spaceleft {SpaceLeft} of {ItemCapacity} [{ItemCount}]", LogLevel.Debug);
    }

    public virtual Item? AddItem(Item item)
    {
        if (item.Stack <= SpaceLeft)
        {
            return AddItemHelper(item);
        }

        var (itemToAdd, diff) = item.GetMaxAmount(SpaceLeft);
        var leftOverItem = AddItemHelper(itemToAdd);
        var amountLeftOver = leftOverItem?.Stack ?? 0 + diff;
        return item.GetAmount(amountLeftOver);
    }

    /// <summary>
    /// Original addItem method of chest
    /// <see cref="StardewValley.Objects.Chest.addItem"/>
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected virtual Item? AddItemHelper(Item item)
    {
        item.resetState();
        Inventory.RemoveEmptySlots();
        foreach (var invItem in Inventory)
        {
            if (invItem == null || !invItem.canStackWith(item)) continue;
            var amount = item.Stack - invItem.addToStack(item);
            if (item.ConsumeStack(amount) == null)
                return null;
        }

        if (Inventory.Count >= SlotCapacity)
            return item;
        Inventory.Add(item);
        return null;
    }


    public bool HighlightItems(Item i)
    {
        // if is basket return false;
        if (Inventory.Contains(i)) return true;
        if (Inventory.Count >= SlotCapacity) return false;
        return ItemCount < ItemCapacity;
    }

    /*
    public ItemGrabMenu GetItemGrabMenu()
    {
        var cols = SlotCapacity < 9 ? SlotCapacity : (SlotCapacity - 1) / 3 + 1;
        var rows = SlotCapacity < 9 ? 1 : 3;
        ModEntry.Mod.Monitor.Log($"{SlotCapacity} -> [{rows},{cols}] = {rows * cols}", LogLevel.Debug);
        // Menu Magic
        var menu = new ItemGrabMenu(
            this,
            false,
            true,
            HighlightItems,
            GrabItemFromInventory,
            null,
            GrabItemFromChest,
            canBeExitedWithKey: true,
            showOrganizeButton: true,
            source: 1,
            sourceItem: Source,
            context: Source);
        var width = 64 * cols + 8;
        var xPosition = Game1.uiViewport.Width / 2 - (width + 32) / 2;
        var yPosition = menu.ItemsToGrabMenu.yPositionOnScreen - (SlotCapacity < 9 ? 0 : 16);
        menu.ItemsToGrabMenu = new InventoryMenu(xPosition, yPosition,
            menu.ItemsToGrabMenu.playerInventory, menu.ItemsToGrabMenu.actualInventory,
            menu.ItemsToGrabMenu.highlightMethod, cols * rows, rows);

        menu.RepositionSideButtons();
        return menu;
    }
    */
    
    public void ShowMenu()
    {
        Game1.activeClickableMenu = BasketInventoryMenu.Create(this);
    }


    #region Delegate

    public IEnumerator<Item> GetEnumerator()
    {
        return Inventory.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Inventory).GetEnumerator();
    }

    public void Add(Item? item)
    {
        Inventory.Add(item);
    }

    public void Clear()
    {
        Inventory.Clear();
    }

    public bool Contains(Item? item)
    {
        return Inventory.Contains(item);
    }

    public void CopyTo(Item[] array, int arrayIndex)
    {
        Inventory.CopyTo(array, arrayIndex);
    }

    public bool Remove(Item? item)
    {
        return Inventory.Remove(item);
    }

    public int Count => Inventory.Count;

    public bool IsReadOnly => Inventory.IsReadOnly;

    public int IndexOf(Item? item)
    {
        return Inventory.IndexOf(item);
    }

    public void Insert(int index, Item? item)
    {
        Inventory.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        Inventory.RemoveAt(index);
    }

    public Item this[int index]
    {
        get => Inventory[index];
        set => Inventory[index] = value;
    }

    public bool HasAny()
    {
        return Inventory.HasAny();
    }

    public bool HasEmptySlots()
    {
        return Inventory.HasEmptySlots();
    }

    public int CountItemStacks()
    {
        return Inventory.CountItemStacks();
    }

    public void OverwriteWith(IList<Item> list)
    {
        Inventory.OverwriteWith(list);
    }

    public IList<Item> GetRange(int index, int count)
    {
        return Inventory.GetRange(index, count);
    }

    public void AddRange(ICollection<Item> collection)
    {
        Inventory.AddRange(collection);
    }

    public void RemoveRange(int index, int count)
    {
        Inventory.RemoveRange(index, count);
    }

    public void RemoveEmptySlots()
    {
        Inventory.RemoveEmptySlots();
    }

    public bool ContainsId(string itemId)
    {
        return Inventory.ContainsId(itemId);
    }

    public bool ContainsId(string itemId, int minimum)
    {
        return Inventory.ContainsId(itemId, minimum);
    }

    public int CountId(string itemId)
    {
        return Inventory.CountId(itemId);
    }

    public IEnumerable<Item> GetById(string itemId)
    {
        return Inventory.GetById(itemId);
    }

    public int Reduce(Item item, int count, bool reduceRemainderFromInventory = false)
    {
        return Inventory.Reduce(item, count, reduceRemainderFromInventory);
    }

    public int ReduceId(string itemId, int count)
    {
        return Inventory.ReduceId(itemId, count);
    }

    public bool RemoveButKeepEmptySlot(Item item)
    {
        return Inventory.RemoveButKeepEmptySlot(item);
    }

    public bool IsLocalPlayerInventory
    {
        get => Inventory.IsLocalPlayerInventory;
        set => Inventory.IsLocalPlayerInventory = value;
    }

    public long LastTickSlotChanged => Inventory.LastTickSlotChanged;

    #endregion
}