using StardewValley;
using StardewValley.ItemTypeDefinitions;

namespace ShedMonitor.Models;

public record ItemSpriteModel(ParsedItemData Item)
{
    public ParsedItemData Item = Item;

    public static ItemSpriteModel Create(Item? item)
    {
        var itemData = ItemRegistry.GetDataOrErrorItem(item?.QualifiedItemId ?? "");
        return new ItemSpriteModel(itemData);
    }
}