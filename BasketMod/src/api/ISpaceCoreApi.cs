using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace BasketMod.api;

public interface ISpaceCoreApi
{
    public void RegisterEquipmentSlot(IManifest modManifest, string globalId, Func<Item, bool> slotValidator, Func<string> slotDisplayName, Texture2D bgTex, Rectangle? bgRect = null);
    public Item GetItemInEquipmentSlot(Farmer farmer, string globalId);
    public void SetItemInEquipmentSlot(Farmer farmer, string globalId, Item item);
    public bool CanItemGoInEquipmentSlot(string globalId, Item item);
}