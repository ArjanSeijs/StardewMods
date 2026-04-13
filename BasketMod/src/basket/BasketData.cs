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
        if (item is not GenericTool basket || !item.modData.TryGetValue(QualifiedName.Field.BasketType, out var type))
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
        if (!data.TryGetValue(QualifiedName.Field.BasketId, out var inventoryId) || inventoryId.Equals("-1"))
        {
            inventoryId = Guid.NewGuid().ToString();
            data[QualifiedName.Field.BasketId] = inventoryId;
        }

        InventoryId = inventoryId;
        SlotCapacity = data.TryGetValue(QualifiedName.Field.SlotCapacity).ToInt() ?? 9;
        ItemCapacity = data.TryGetValue(QualifiedName.Field.ItemCapacity).ToInt() ?? int.MaxValue;
        StackCapacity = data.TryGetValue(QualifiedName.Field.StackCapacity).ToInt() ?? 999;
        ContextTags = data.TryGetValue(QualifiedName.Field.ContextTags, "");
        Type = data.TryGetValue(QualifiedName.Field.BasketType)!;
        Source = item;
    }

    public Item Source { get; set; }

    public string InventoryId { get; }

    public string Type { get; }

    public int ItemCapacity { get; set; }

    public int SlotCapacity { get; set; }

    public int StackCapacity { get; set; }

    public string ContextTags { get; set; }
}