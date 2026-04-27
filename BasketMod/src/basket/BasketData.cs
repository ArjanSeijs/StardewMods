using StardewValley;
using StardewValley.Tools;

namespace BasketMod.basket;

public static class BasketDataExtension
{
    public static bool IsBasket(this Item item)
    {
        if (item is not GenericTool basket) return false;
        var data = basket.GetToolData().ModData;
        return data is not null && data.TryGetValue(QualifiedName.Field.BasketType, out _);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="basketData"></param>
    /// <returns></returns>
    public static bool AsBasket(this Item item, out BasketData? basketData)
    {
        if (item is not GenericTool basket)
        {
            basketData = null;
            return false;
        }

        var data = basket.GetToolData().ModData;
        if (data is null || !data.TryGetValue(QualifiedName.Field.BasketType, out _))
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
            inventoryId = QualifiedName.InventoryName(Guid.NewGuid().ToString());
            data[QualifiedName.Field.BasketId] = inventoryId;
        }

        GlobalInventoryId = inventoryId;
        
        var slotCapacity = toolData.GetValueOrDefault(QualifiedName.Field.SlotCapacity).ToInt() ?? 9;
        var itemCapacity = toolData.GetValueOrDefault(QualifiedName.Field.ItemCapacity).ToInt() ?? int.MaxValue;
        var stackCapacity = toolData.GetValueOrDefault(QualifiedName.Field.StackCapacity).ToInt() ?? 999;
        SlotCapacity = Math.Clamp(slotCapacity, 2, 12 * 3);
        ItemCapacity = Math.Clamp(itemCapacity, 1, int.MaxValue);
        StackCapacity = Math.Clamp(stackCapacity, 1, int.MaxValue);
        
        ContextTagsQuery = toolData.GetValueOrDefault(QualifiedName.Field.ContextTagsQuery, "");
        Type = toolData.GetValueOrDefault(QualifiedName.Field.BasketType)!;
        Source = item;
        Inception = toolData.TryGetValue(QualifiedName.Field.Inception, out var value) &&
                    (value?.ToLower().Equals("true") ?? false);
    }

    public BasketData(string globalInventoryId, int slotCapacity = 9, int itemCapacity = int.MaxValue,
        int stackCapacity = 999, string contextTagsQuery = "", Item? source = null, bool inception = false)
    {
        Source = source;
        GlobalInventoryId = globalInventoryId;
        Type = "Basket";
        SlotCapacity = slotCapacity;
        ItemCapacity = itemCapacity;
        StackCapacity = stackCapacity;
        ContextTagsQuery = contextTagsQuery;
        Inception = inception;
    }

    public Item? Source { get; }

    public string GlobalInventoryId { get; }

    public string Type { get; }

    public int SlotCapacity { get; }

    public int ItemCapacity { get; }

    public int StackCapacity { get; }

    public string ContextTagsQuery { get; }

    public bool Inception { get; }
}