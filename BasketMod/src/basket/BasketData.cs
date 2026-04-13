using System.Diagnostics.CodeAnalysis;
using StardewValley;
using StardewValley.Tools;

namespace BasketMod.basket;

public static class BasketDataExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="basketData"></param>
    /// <returns></returns>
    public static bool AsBasket(this Item item, [NotNullWhen(true)] out BasketData? basketData)
    {
        if (item is not GenericTool basket)
        {
            basketData = null;
            return false;
        }
        var data = basket.GetToolData().ModData;
        if (data is null || !data.TryGetValue(QualifiedName.Field.BasketType, out var type))
        {
            basketData = null;
            return false;
        }

        basketData = new BasketData(basket);
        return true;
    }
}

public struct BasketData
{
    public BasketData(GenericTool item)
    {
        var data = item.modData;
        var toolData = item.GetToolData().ModData;
        if (!data.TryGetValue(QualifiedName.Field.BasketId, out var inventoryId) || inventoryId.Equals("-1"))
        {
            inventoryId = Guid.NewGuid().ToString();
            data[QualifiedName.Field.BasketId] = inventoryId;
        }

        InventoryId = inventoryId;
        var slotCapacity = toolData.GetValueOrDefault(QualifiedName.Field.SlotCapacity).ToInt() ?? 9;
        var itemCapacity = toolData.GetValueOrDefault(QualifiedName.Field.ItemCapacity).ToInt() ?? int.MaxValue;
        var stackCapacity = toolData.GetValueOrDefault(QualifiedName.Field.StackCapacity).ToInt() ?? 999;
        SlotCapacity = Math.Clamp(slotCapacity, 2, 12 * 3);
        ItemCapacity = Math.Clamp(itemCapacity, 1, int.MaxValue);
        StackCapacity = Math.Clamp(stackCapacity, 1, int.MaxValue);
        ContextTagsQuery = toolData.GetValueOrDefault(QualifiedName.Field.ContextTagsQuery, "");
        Type = toolData.GetValueOrDefault(QualifiedName.Field.BasketType)!;
        Source = item;
    }

    public BasketData(string inventoryId, int itemCapacity = int.MaxValue, int slotCapacity = 9,
        int stackCapacity = 999, string contextTagsQuery = "", Item? source = null)
    {
        Source = source;
        InventoryId = inventoryId;
        Type = "Basket";
        ItemCapacity = itemCapacity;
        SlotCapacity = slotCapacity;
        StackCapacity = stackCapacity;
        ContextTagsQuery = contextTagsQuery;
    }

    public Item? Source { get; }

    public string InventoryId { get; }

    public string Type { get; }

    public int ItemCapacity { get; }

    public int SlotCapacity { get; }

    public int StackCapacity { get; }

    public string ContextTagsQuery { get; }
}