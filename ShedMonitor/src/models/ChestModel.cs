using StardewUI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace ShedMonitor.Models;

public record ChestModel(Chest Chest)
{
    /// <summary>
    /// 
    /// </summary>
    public string DisplayName => Chest.DisplayName;

    /// <summary>
    /// 
    /// </summary>
    public ChestSpriteModel ChestSprite = new(Chest);

    /// <summary>
    /// 
    /// </summary>
    public ItemSpriteModel ItemSprite =>
        ItemSpriteModel.Create(Util.GetSignItemNearChest(Chest) ?? Chest.Items.FirstOrDefault());

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Open()
    {
        Chest.ShowMenu();
        Game1.playSound(Chest.fridge.Value ? "doorCreak" : "openChest");
        Chest.performOpenChest();
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public bool SetDisplayName(string displayName)
    {
        Chest.displayName = displayName;
        return true;
    }

    #endregion
}