using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;

namespace BasketMod.basket;

public static class BasketSpecial
{
    public static bool IsLinked(this Item item)
    {
        if (item is not GenericTool basket) return false;
        var data = basket.GetToolData().ModData;
        return data is not null &&
               data.TryGetValue(QualifiedName.Field.BasketSpecial, out var special) &&
               special is not null &&
               special.Equals("Linked");
    }

    public static bool IsSpecial(this Item item)
    {
        return item.IsLinked();
    }

    public static bool PerformSpecial(this Tool tool, GameLocation location, int x, int y, Farmer who)
    {
        if (!tool.IsLinked()) return false;
        if (location.getObjectAt(x, y) is Chest)
        {
            tool.modData[QualifiedName.Field.XCoord] = x.ToString();
            tool.modData[QualifiedName.Field.YCoord] = y.ToString();
            tool.modData[QualifiedName.Field.LocationID] = location.NameOrUniqueName;
            Game1.playSound("pickUpItem");
        }
        else
        {
            var xPres = tool.modData.TryGetValue(QualifiedName.Field.XCoord, out var xCoord);
            var yPres = tool.modData.TryGetValue(QualifiedName.Field.YCoord, out var yCoord);
            var namePres = tool.modData.TryGetValue(QualifiedName.Field.LocationID, out var locationID);
            if (!xPres || !yPres || !namePres || Game1.getLocationFromName(locationID) is not { } l ||
                l.getObjectAt(int.Parse(xCoord), int.Parse(yCoord)) is not Chest chest)
            {
                Game1.playSound("clank");
                return false;
            }

            Game1.playSound("openChest");
            chest.ShowMenu();
        }

        return true;
    }
}