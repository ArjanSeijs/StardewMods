using System.Diagnostics.CodeAnalysis;
using BasketMod.basket;
using Netcode;
using StardewValley;
using StardewValley.Tools;
using Object = StardewValley.Object;

namespace BasketMod;

public static class Extensions
{
    /// <summary>
    /// Copy of item with stack of size. Will not create larger than max stack size.
    /// <see cref="Item.getOne"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static Item GetAmount(this Item item, int amount)
    {
        var copy = item.getOne();
        copy.Stack = Math.Min(amount, item.maximumStackSize());
        return copy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Item Copy(this Item item)
    {
        return item.GetAmount(item.Stack);
    }

    /// <summary>
    /// Get copy of this, and  item with left over amount
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static (Item, Item) GetSplit(this Item item, int amount)
    {
        var copy = item.GetAmount(Math.Min(amount, item.Stack));
        var leftOver = item.GetAmount(item.Stack - copy.Stack);
        return (copy, leftOver);
    }

    /// <summary>
    /// Add two item stacks together and return copy.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Item Add(this Item left, Item? right)
    {
        if (right == null)
            return left.Copy();
        return left.QualifiedItemId.Equals(right.QualifiedItemId)
            ? left.GetAmount(left.Stack + right.Stack)
            : throw new ArgumentException($"{left.QualifiedItemId} is not equal to {right.QualifiedItemId}");
    }

    /// <summary>
    /// <see cref="Item.addToStack"/>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="otherStack"></param>
    /// <param name="maxStackSize"></param>
    /// <returns></returns>
    public static int AddToStack(this Item item, Item otherStack, int maxStackSize)
    {
        int num = Math.Min(item.maximumStackSize(), maxStackSize);
        if (num == 1)
            return otherStack.Stack;
        item.stack.Value += otherStack.Stack;
        if (item is Object object1 && otherStack is Object object2 && object1.IsSpawnedObject &&
            !object2.IsSpawnedObject)
            object1.IsSpawnedObject = false;
        if (item.stack.Value <= num)
            return 0;
        int stack = item.stack.Value - num;
        item.stack.Value = num;
        return stack;
    }

    /// <summary>
    /// <see cref="int.TryParse(string,out int)"/>
    /// </summary>
    /// <param name="str"></param>
    /// <returns>null if it could not parse</returns>
    public static int? ToInt(this string? str)
    {
        return int.TryParse(str, out var v) ? v : null;
    }
}