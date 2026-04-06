using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;

namespace ShedMonitor;

public static class Util
{
    public static Item? GetSignItemNearChest(Chest chest)
    {
        var chestTile = chest.TileLocation;
        var location = chest.Location;
        Vector2[] tilesToCheck =
        {
            new(chestTile.X, chestTile.Y - 1), // top
            new(chestTile.X - 1, chestTile.Y), // left
            new(chestTile.X + 1, chestTile.Y), // right
            new(chestTile.X, chestTile.Y + 1), // bottom
        };

        foreach (var tile in tilesToCheck)
        {
            if (location.objects.TryGetValue(tile, out var obj) && obj is Sign sign)
            {
                return sign.displayItem.Value;
            }
        }

        return null;
    }

    public static IEnumerable<Chest> GetChestsFromLocation(GameLocation location)
    {
        return location.Objects.Values.OfType<Chest>();
    }

    public static string ToHex(this Color color, int? alpha = null)
    {
        return alpha is null
            ? $"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}"
            : $"#{color.R:X2}{color.G:X2}{color.B:X2}{alpha:X2}";
    }
}