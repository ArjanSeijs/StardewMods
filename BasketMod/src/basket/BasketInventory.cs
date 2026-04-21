using System.Collections;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Menus;

namespace BasketMod.basket;

public class BasketInventory : IInventory
{
    public BasketData BasketData { get; }

    /// <summary>
    /// The Basket Item
    /// </summary>
    public Item? Source => BasketData.Source;

    /// <summary>
    /// The amount of slots (item stacks)
    /// </summary>
    public int SlotCapacity => BasketData.SlotCapacity;

    /// <summary>
    /// Max amount of total items
    /// </summary>
    public int ItemCapacity => BasketData.ItemCapacity;

    /// <summary>
    /// Max Amount of items in one slot
    /// </summary>
    public int StackCapacity => BasketData.StackCapacity;

    /// <summary>
    /// Global Inventory for this basket
    /// </summary>
    public IInventory Inventory => Game1.player.team.GetOrCreateGlobalInventory(GlobalInventoryId);

    /// <summary>
    /// The Global Inventory ID Including prefix
    /// </summary>
    public string GlobalInventoryId => BasketData.GlobalInventoryId;

    /// <summary>
    /// The Sum of all items in the inventory
    /// </summary>

    public int ItemCount => Inventory.Sum(item => item?.Stack ?? 0);

    /// <summary>
    /// <see cref="ItemCapacity"/> - <see cref="ItemCount"/>
    /// </summary>
    public int ItemRemainingCapacity => Math.Max(0, ItemCapacity - ItemCount);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="basketData"></param>
    public BasketInventory(BasketData basketData)
    {
        BasketData = basketData;
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual Item? AddItem(Item item)
    {
        // In case something goes wrong
        ModEntry.Mod.RecoverInventory.RemoveEmptySlots();
        ModEntry.Mod.RecoverInventory.Insert(0, item);
        // Original Logic
        if (item.Stack <= ItemRemainingCapacity)
        {
            return AddItemHelper(item);
        }

        //Modified Logic
        var (itemToAdd, rest1) = item.GetSplit(ItemRemainingCapacity);
        var rest2 = AddItemHelper(itemToAdd);
        return rest1.Add(rest2);
    }

    /// <summary>
    /// Modified AddItem Method from 
    /// <see cref="StardewValley.Objects.Chest.addItem"/>
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected virtual Item? AddItemHelper(Item item)
    {
        if (item.Stack > ItemRemainingCapacity)
            ModEntry.Mod.Monitor.Log(
                $"Item {item.QualifiedItemId} has size {item.Stack} while the remaining capacity is {ItemRemainingCapacity}",
                LogLevel.Error);
        // Altered Logic from Chest AddItem
        item.resetState();
        Inventory.RemoveEmptySlots();
        foreach (var invItem in Inventory)
        {
            if (invItem == null || !invItem.canStackWith(item)) continue;
            var amount = item.Stack - invItem.AddToStack(item, StackCapacity);
            if (item.ConsumeStack(amount) == null)
                return null;
        }

        if (Inventory.Count >= SlotCapacity)
            return item;
        var itemResized = item.GetAmount(Extensions.Min(item.Stack, StackCapacity, ItemRemainingCapacity));
        Inventory.Add(itemResized);
        return item.ConsumeStack(itemResized.Stack);
    }


    /// <summary>
    /// If Item can be added to the inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HighlightItems(Item item)
    {
        if (Source == item) return false; // Cannot put self in self
        if (Inventory.Contains(item)) return true; // Can take all items out
        if (ItemCount >= ItemCapacity) return false; // Only if there is Space Left
        if (item.AsBasket(out _)) return BasketData.Inception;
        if (BasketData.ContextTagsQuery.Equals("")) return true; // No query so include all
        var disjunctions = BasketData.ContextTagsQuery.Split("||");
        var contextTags = item.GetContextTags();
        return disjunctions.Any(query => ItemContextTagManager.DoesTagQueryMatch(query, contextTags));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ItemGrabMenu InventoryMenu()
    {
        return BasketInventoryMenu.Create(this);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowMenu()
    {
        Game1.activeClickableMenu = InventoryMenu();
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