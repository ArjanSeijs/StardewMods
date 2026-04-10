using StardewValley;

namespace BasketMod;

public static class ItemExtensions
{
    /// <summary>
    /// Copy of item with stack of size.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static Item GetAmount(this Item item, int amount)
    {
        var copy = item.getOne();
        copy.Stack = amount;
        return copy;
    }

    /// <summary>
    /// Copy of item with stack size of max if item.Stack > max
    /// </summary>
    /// <param name="item"></param>
    /// <param name="max"></param>
    /// <returns>The item and how much is left over.</returns>
    public static (Item, int) GetMaxAmount(this Item item, int max)
    {
        var copy = item.GetAmount(Math.Min(max, item.Stack));
        return (copy, item.Stack - copy.Stack);
    }
}