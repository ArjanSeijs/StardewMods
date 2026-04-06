using PropertyChanged.SourceGenerator;
using StardewUI;
using StardewValley;

namespace ShedMonitor.Models;

public partial record ChestListModel(string HeaderText, ChestModel[] Chests)
{
    // True, Sort Horizontal, False Sort Vertical
    [Notify] private Direction _sort = _lastSort;
    private static Direction _lastSort = Direction.East;


    [DependsOn(nameof(Sort))]
    public ChestModel[] ChestsSorted
    {
        get
        {
            return Sort switch
            {
                Direction.North => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.X)
                    .ThenByDescending(chest => chest.Chest.TileLocation.Y)
                    .ToArray(),
                Direction.East => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.Y)
                    .ThenBy(chest => chest.Chest.TileLocation.X)
                    .ToArray(),
                Direction.South => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.X)
                    .ThenBy(chest => chest.Chest.TileLocation.Y)
                    .ToArray(),
                Direction.West => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.Y)
                    .ThenByDescending(chest => chest.Chest.TileLocation.X)
                    .ToArray(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public void ToggleSort()
    {
        Sort = Sort switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new ArgumentOutOfRangeException()
        };

        _lastSort = Sort;
        Game1.playSound("shwip");
    }

    public static ChestListModel CreateFromLocation(GameLocation location)
    {
        var chests =
            from chest in Util.GetChestsFromLocation(location)
            select new ChestModel(chest);

        return new ChestListModel("Chests", chests.ToArray());
    }
}