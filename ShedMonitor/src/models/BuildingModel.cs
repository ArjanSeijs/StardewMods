using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Objects;

namespace ShedMonitor.Models;

public record BuildingModel(Building Building)
{
    public Sprite BuildingSprite = new(Building.texture.Value, Building.getSourceRect());

    public ParsedItemData ItemSprite => GetItemSprite();

    private ParsedItemData GetItemSprite()
    {
        var location = Building.GetParentLocation();
        var x = Building.tileX.Value;
        var y = Building.tileY.Value;
        var width = Building.tilesWide.Value;
        var height = Building.tilesHigh.Value;
        for (var i = 0; i < width; i++)
        {
            var tile = new Vector2(x + i + 1, y + height);
            if (!location.objects.TryGetValue(tile, out var obj) || obj is not Sign sign) continue;
            var item = sign.displayItem.Value;
            if (item is null) continue;
            return ItemRegistry.GetDataOrErrorItem(item.QualifiedItemId);
        }

        return ItemRegistry.GetDataOrErrorItem("");
    }

    public bool Open()
    {
        if (Building.GetIndoors() is null) return false;
        if (!Building.OnUseHumanDoor(Game1.player)) return false;
        var menu = StorageModel.CreateViewFromLocation(Building.GetIndoors());
        Game1.activeClickableMenu = menu;
        return true;
    }
}