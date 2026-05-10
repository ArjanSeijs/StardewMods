using BasketMod.api;
using BasketMod.basket;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace BasketMod;

public static class SpaceCoreEquipment
{
    public const int MaxSlots = 5;

    public static void RegisterSlots(ModEntry mod, ISpaceCoreApi spacecore)
    {
        for (var i = 0; i < MaxSlots; i++)
        {
            var bgTexture = mod.Helper.ModContent.Load<Texture2D>("assets/slot.png");
            var index = i;
            spacecore.RegisterEquipmentSlot(
                mod.ModManifest,
                QualifiedName.SpaceCoreSlot(i),
                SlotValidator,
                () => $"Bag_{index}",
                bgTexture);
        }
    }

    private static bool SlotValidator(Item? arg)
    {
        return arg is null || arg.IsBasket() || arg.IsSpecial();
    }

    public static void OnButtonPressed(ModEntry modEntry, ISpaceCoreApi spacecore)
    {
        for (var i = 0; i < MaxSlots; i++)
        {
            var keybind = ModEntry.Mod.Config.Buttons[i]?.IsDown() ?? false;
            var item = spacecore.GetItemInEquipmentSlot(Game1.player, QualifiedName.SpaceCoreSlot(i));
            if (!keybind || item is null) continue;
            var basket = item.AsBasket();
            if (basket is not null)
            {
                new BasketInventory(basket.Value).ShowMenu();
                break;   
            }

            if (item.IsSpecial())
            {
                item.PerformSpecial();
                break;
            }
        }
    }
}