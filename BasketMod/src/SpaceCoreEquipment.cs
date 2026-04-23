using BasketMod.api;
using BasketMod.basket;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace BasketMod;

public static class SpaceCoreEquipment
{
    public static void RegisterSlots(ModEntry mod, ISpaceCoreApi spacecore)
    {
        for (var i = 0; i < 3; i++)
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
        return arg is null || arg.IsBasket();
    }
}