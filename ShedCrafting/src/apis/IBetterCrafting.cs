using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;

namespace ShedCrafting.Apis;

public interface IBetterCrafting
{
    /// <summary>
    /// Get the currently open Better Crafting menu. This may be <c>null</c> if
    /// the menu is still opening.
    /// </summary>
    IBetterCraftingMenu? GetActiveMenu();

    /// <summary>
    /// Try to open the Better Crafting menu. This may fail if there is another
    /// menu open that cannot be replaced.
    ///
    /// If opening the menu from an object in the world, such as a workbench,
    /// its location and tile position can be provided for automatic detection
    /// of nearby chests.
    ///
    /// Better Crafting has its own handling of mutexes, so please do not worry
    /// about locking Chests before handing them off to the menu.
    ///
    /// When discovering additional containers, Better Crafting scans all tiles
    /// around each of its existing known containers. If a location and position
    /// for the menu source is provided, the tiles around that position will
    /// be scanned as well.
    ///
    /// Discovery depends on the user's settings, though at a minimum a 3x3 area
    /// will be scanned to mimic the scanning radius of the vanilla workbench.
    /// </summary>
    /// <param name="cooking">If true, open the cooking menu. If false, open the crafting menu.</param>
    /// <param name="silent_open">If true, do not make a sound upon opening the menu.</param>
    /// <param name="location">The map the associated object is in, or null if there is no object</param>
    /// <param name="position">The tile position the associated object is at, or null if there is no object</param>
    /// <param name="area">The tile area the associated object covers, or null if there is no object or if the object only covers a single tile</param>
    /// <param name="discover_containers">If true, attempt to discover additional material containers.</param>
    /// <param name="containers">An optional list of containers to draw extra crafting materials from.</param>
    /// <param name="listed_recipes">An optional list of recipes by name. If provided, only these recipes will be listed in the crafting menu.</param>
    /// <param name="discover_buildings">If true, attempt to discover additional containers inside of adjacent buildings.</param>
    /// <returns>Whether or not the menu was opened successfully</returns>
    bool OpenCraftingMenu(
        bool cooking,
        bool silent_open = false,
        GameLocation? location = null,
        Vector2? position = null,
        Rectangle? area = null,
        bool discover_containers = true,
        IList<Tuple<object, GameLocation?>>? containers = null,
        IList<string>? listed_recipes = null,
        bool discover_buildings = false
    );
}

/// <summary>
/// A simplified interface for the PopulateContainers event that allows
/// you to remove the IBetterCraftingMenu interface.
/// </summary>
public interface ISimplePopulateContainersEvent
{
    /// <summary>
    /// A list of all the containers this menu should draw items from.
    /// </summary>
    IList<Tuple<object, GameLocation?>> Containers { get; }

    /// <summary>
    /// Set this to true to prevent Better Crafting from running its
    /// own container discovery logic, if you so desire.
    /// </summary>
    bool DisableDiscovery { get; set; }
}

public interface IPopulateContainersEvent : ISimplePopulateContainersEvent
{
    /// <summary>
    /// The relevant Better Crafting menu.
    /// </summary>
    IBetterCraftingMenu Menu { get; }
}

public interface IBetterCraftingMenu
{
    /// <summary>
    /// The <see cref="IClickableMenu"/> instance for this menu. This is the
    /// same object, but included for convenience due to how API proxying works.
    /// </summary>
    IClickableMenu Menu { get; }

    /// <summary>
    /// Whether or not this menu is going to perform container discovery.
    /// </summary>
    bool DiscoverContainers { get; }

    /// <summary>
    /// Whether or not this menu is going to scan buildings as part of
    /// container discovery.
    /// </summary>
    bool DiscoverBuildings { get; }

    /// <summary>
    /// If this menu is associated with a specific crafting station, this
    /// is the crafting station.
    /// </summary>
    ICraftingStation? Station { get; }

    /// <summary>
    /// Whether or not this crafting menu is ready, meaning that it has
    /// finished initializing. Note that it can still be busy crafting, so
    /// you may need to check <see cref="Working"/> as well.
    /// </summary>
    bool IsReady { get; }

    /// <summary>
    /// Whether or not this crafting menu is for cooking. If this is
    /// false, then the menu is for crafting recipes.
    /// </summary>
    bool Cooking { get; }

    /// <summary>
    /// Whether or not this is a standalone menu. If this is false,
    /// this menu is likely contained in <see cref="GameMenu"/>.
    /// </summary>
    bool Standalone { get; }

    /// <summary>
    /// Whether or not the user is currently editing their categories.
    /// </summary>
    bool Editing { get; }

    /// <summary>
    /// Whether or not the menu is actively crafting something. This
    /// will only return true when a craft is happening, or when the
    /// menu is waiting for an asynchronous craft to return.
    /// </summary>
    bool Working { get; }

    /// <summary>
    /// The location this menu was opened from, if it has an associated
    /// location. This may be null if the menu was not opened by
    /// interacting with something in the world, like a Workbench.
    /// </summary>
    GameLocation? Location { get; }

    /// <summary>
    /// The position this menu was opened from, if it has an associated
    /// position. This may be null if the menu was not opened by
    /// interacting with something in the world, like a kitchen.
    /// </summary>
    Vector2? Position { get; }

    /// <summary>
    /// The multi-tile area this menu was opened from, if it has an
    /// associated area. This is not set when working with single
    /// tile objects like a Workbench.
    /// </summary>
    Rectangle? Area { get; }

    /// <summary>
    /// Calling this method will toggle edit mode, as though the user
    /// clicked the button themselves.
    /// </summary>
    void ToggleEditMode();

    /// <summary>
    /// Get a list of specific recipes that are to be displayed in the
    /// crafting menu. If this list is <c>null</c>, all recipes will be
    /// displayed to the user.
    /// </summary>
    IReadOnlyList<string>? GetListedRecipes();

    /// <summary>
    /// Set a new list of specific recipes that are to be displayed in the
    /// crafting menu. Note: If the user does not know these recipes, they
    /// will not be displayed even if they're in this list.
    ///
    /// Set the list to <c>null</c> to display all recipes.
    /// </summary>
    /// <param name="recipes">The list of recipes that should be displayed.</param>
    void UpdateListedRecipes(IEnumerable<string>? recipes);
}

public interface ICraftingStation
{
    /// <summary>
    /// The crafting station's unique Id.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The display name of this crafting station.
    /// </summary>
    string? DisplayName { get; }

    /// <summary>
    /// Whether this crafting station's recipes should be available outside
    /// of this crafting station or not.
    /// </summary>
    bool AreRecipesExclusive { get; }

    /// <summary>
    /// When this is true, this crafting station's recipes will always be
    /// available, even if the player hasn't learned the recipe yet.
    /// </summary>
    bool DisplayUnknownRecipes { get; }

    /// <summary>
    /// Whether or not this crafting station is for cooking.
    /// </summary>
    bool IsCooking { get; }

    /// <summary>
    /// A list of recipes included in this crafting station.
    /// </summary>
    string[] Recipes { get; }
}