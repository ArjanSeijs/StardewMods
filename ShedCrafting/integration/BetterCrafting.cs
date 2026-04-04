using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ShedCrafting.integration;

public static class BetterCrafting
{
    /// <summary>
    /// Set to true when we replace 
    /// </summary>
    private static bool _callFlag;

    public static void OnMenuChanged(MenuChangedEventArgs e)
    {
        // We Just Tried to Replace it with our Page
        if (_callFlag)
        {
            _callFlag = false;
            return;
        }

        // 
        var api = ModEntry.Mod.BetterCraftingAPI;
        var menu = api?.GetActiveMenu();
        if (api is null || menu?.Position is null || menu.Location is null) return;

        var chests = (from chest in ShedCrafting.GetChests(menu.Position.Value, menu.Location)
            select new Tuple<object, GameLocation?>(chest, chest.Location)).ToList();
        if (chests.Count == 0) return;

        // Do The Replacing
        _callFlag = true;
        var success = api.OpenCraftingMenu(
            menu.Cooking,
            true,
            menu.Location,
            menu.Position,
            menu.Area,
            menu.DiscoverContainers,
            chests,
            menu.GetListedRecipes()?.ToList(),
            menu.DiscoverBuildings
        );
        if (!success)
        {
            ModEntry.Mod.Monitor.Log("OnMenuChanged [BetterCrafting] Could not replace menu.", LogLevel.Debug);
        }
    }
}