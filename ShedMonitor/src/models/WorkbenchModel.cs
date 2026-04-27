using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Objects;

namespace ShedMonitor.Models;

public record WorkbenchModel(Workbench Workbench, string HoverText = "")
{
    /// <summary>
    /// 
    /// </summary>
    public ParsedItemData WorkbenchSprite = ItemRegistry.GetDataOrErrorItem(Workbench.QualifiedItemId);

    /// <summary>
    /// 
    /// </summary>
    public ItemSpriteModel ItemSprite =>
        ItemSpriteModel.Create(Util.GetSignItemNearLocation(Workbench.Location, Workbench.TileLocation));

    public bool Open()
    {
        var location = Workbench.Location;
        var tileLocation = Workbench.TileLocation;
        var building = location.getBuildingAt(tileLocation) ??
                       location.getBuildingAt(tileLocation + new Vector2(0, -1)) ??
                       location.getBuildingAt(tileLocation + new Vector2(-1, 0)) ??
                       location.getBuildingAt(tileLocation + new Vector2(1, 0));
        if (building is null) return false;
        if (Game1.activeClickableMenu is not null)
        {
            Game1.nextClickableMenu.Add(Game1.activeClickableMenu);
        }

        var menu = StorageModel.CreateViewFromLocation(building.GetIndoors());
        
        Game1.activeClickableMenu = menu;
        return true;
    }
}