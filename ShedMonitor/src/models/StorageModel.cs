using PropertyChanged.SourceGenerator;
using StardewUI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;

namespace ShedMonitor.Models;

public partial record StorageModel(string HeaderText, ChestModel[] Chests, WorkbenchModel[] WorkBenches)
{
    // True, Sort Horizontal, False Sort Vertical
    [Notify] private Direction _sort = _lastSort;
    private static Direction _lastSort = Direction.East;

    //TODO Improve
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


    [DependsOn(nameof(Sort))]
    public WorkbenchModel[] WorkbenchesSorted
    {
        get
        {
            return Sort switch
            {
                Direction.North => WorkBenches
                    .OrderBy(chest => chest.Workbench.TileLocation.X)
                    .ThenByDescending(chest => chest.Workbench.TileLocation.Y)
                    .ToArray(),
                Direction.East => WorkBenches
                    .OrderBy(chest => chest.Workbench.TileLocation.Y)
                    .ThenBy(chest => chest.Workbench.TileLocation.X)
                    .ToArray(),
                Direction.South => WorkBenches
                    .OrderBy(chest => chest.Workbench.TileLocation.X)
                    .ThenBy(chest => chest.Workbench.TileLocation.Y)
                    .ToArray(),
                Direction.West => WorkBenches
                    .OrderBy(chest => chest.Workbench.TileLocation.Y)
                    .ThenByDescending(chest => chest.Workbench.TileLocation.X)
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

    public static StorageModel CreateFromLocation(GameLocation location)
    {
        var chests =
            from chest in location.OfType<Chest>()
            select new ChestModel(chest);

        var workBenches =
            from workBench in location.OfType<Workbench>()
            select new WorkbenchModel(workBench);

        return new StorageModel("Chests", chests.ToArray(), workBenches.ToArray());
    }

    public static IClickableMenu CreateViewFromLocation(GameLocation location)
    {
        return ModEntry.Mod.ViewEngine.CreateMenuFromAsset(
            $"Mods/{ModEntry.Mod.ModManifest.Name}/Views/StorageView",
            CreateFromLocation(location));
    }
}