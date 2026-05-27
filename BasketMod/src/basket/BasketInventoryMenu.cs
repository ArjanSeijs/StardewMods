using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;

namespace BasketMod.basket;

/// <summary>
/// ***Beware here be magic***
/// Some hacky logic to make a custom ItemGrabMenu.
/// </summary>
public static class BasketInventoryMenu 
{

    private class BasketInventoryProxy : Chest
    {
        private readonly BasketInventory _inventory;

        internal BasketInventoryProxy(BasketInventory inventory)
        {
            _inventory = inventory;
            SpecialChestType = SpecialChestTypes.JunimoChest;
        }

        public override int GetActualCapacity()
        {
            return _inventory.SlotCapacity;
        }
        
        
    }

    public static ItemGrabMenu Create(BasketInventory inventory)
    {
        var menu = new ItemGrabMenu(inventory,
            false,
            true,
            inventory.HighlightItems,
            inventory.GrabItemFromInventory,
            null,
            inventory.GrabItemFromChest,
            canBeExitedWithKey: true,
            showOrganizeButton: !ModEntry.Mod.Config.UseCustomInventoryLogic, //Organise & Stack Does not yet support custom logic
            source: 1,
            sourceItem: new BasketInventoryProxy(inventory),
            context: inventory.Source);
        return menu;
    }
}