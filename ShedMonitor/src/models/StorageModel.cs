using PropertyChanged.SourceGenerator;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;

namespace ShedMonitor.Models;

public partial record StorageModel(string HeaderText, string LocationId, ChestModel[] Chests, BuildingModel[] Buildings)
{
    public enum SortDirection
    {
        North,
        East,
        South,
        West,
        Color
    }

    [Notify] private SortDirection _sort = LastSort.ContainsKey(LocationId)
        ? LastSort[LocationId]
        : SortDirection.Color;

    private static readonly Dictionary<string, SortDirection> LastSort = new();

    //TODO Improve
    [DependsOn(nameof(Sort))]
    public ChestModel[] ChestsSorted
    {
        get
        {
            return Sort switch
            {
                SortDirection.North => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.X)
                    .ThenByDescending(chest => chest.Chest.TileLocation.Y)
                    .ToArray(),
                SortDirection.East => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.Y)
                    .ThenBy(chest => chest.Chest.TileLocation.X)
                    .ToArray(),
                SortDirection.South => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.X)
                    .ThenBy(chest => chest.Chest.TileLocation.Y)
                    .ToArray(),
                SortDirection.West => Chests
                    .OrderBy(chest => chest.Chest.TileLocation.Y)
                    .ThenByDescending(chest => chest.Chest.TileLocation.X)
                    .ToArray(),
                SortDirection.Color => Chests
                    .OrderBy(chest => chest.Chest.QualifiedItemId)
                    .ThenBy(chest => chest.Chest.playerChoiceColor.Value.R)
                    .ThenBy(chest => chest.Chest.playerChoiceColor.Value.G)
                    .ThenBy(chest => chest.Chest.playerChoiceColor.Value.B)
                    .ToArray(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public void ToggleSort()
    {
        Sort = Sort switch
        {
            SortDirection.North => SortDirection.East,
            SortDirection.East => SortDirection.South,
            SortDirection.South => SortDirection.West,
            SortDirection.West => SortDirection.Color,
            SortDirection.Color => SortDirection.North,
            _ => throw new ArgumentOutOfRangeException()
        };

        LastSort[LocationId] = Sort;
        Game1.playSound("shwip");
    }

    public static StorageModel CreateFromLocation(GameLocation location)
    {
        var chests =
            from chest in location.OfType<Chest>()
            where chest.playerChest.Value
            select new ChestModel(chest);

        var buildings =
            location.IsBuildableLocation()
                ? from building in location.buildings
                where building.HasIndoors() && building.daysOfConstructionLeft.Value <= 0
                where (from chest in building.GetIndoors().OfType<Chest>() where chest.playerChest.Value select chest)
                    .Any()
                select new BuildingModel(building)
                : Array.Empty<BuildingModel>();

        return new StorageModel("Chests", location.NameOrUniqueName, chests.ToArray(), buildings.ToArray());
    }

    public static IClickableMenu CreateViewFromLocation(GameLocation location)
    {
        return ModEntry.Mod.ViewEngine.CreateMenuFromAsset(
            $"Mods/{ModEntry.Mod.ModManifest.Name}/Views/StorageView",
            CreateFromLocation(location));
    }
}