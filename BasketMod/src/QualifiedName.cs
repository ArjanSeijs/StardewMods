using StardewValley;

namespace BasketMod;

public static class QualifiedName
{
    public const string InventoryIDPrefix = "EternalSoap.BasketMod";

    // Keep in sync with content.json

    public static class ItemIDs
    {
        public const string Basket = "(T)EternalSoap.BasketMod.CP.Basket";
    }

    public static class Field
    {
        public const string BasketType = "EternalSoap.BasketMod.CP.BasketType";
        public const string BasketId = "EternalSoap.BasketMod.CP.BasketId";
        public const string SlotCapacity = "EternalSoap.BasketMod.CP.SlotCapacity";
        public const string ItemCapacity = "EternalSoap.BasketMod.CP.ItemCapacity";
        public const string StackCapacity = "EternalSoap.BasketMod.CP.StackCapacity";
        public const string ContextTags = "EternalSoap.BasketMod.CP.ContextTags";
        public const string Inception = "EternalSoap.BasketMod.CP.Inception";   
    }

    public static class Highlight
    {
        public const string Forage = "Forage";
        public const string Gem = "Gem";
        public const string Bait = "Bait";
        public const string Tackle = "EternalSoap.BasketMod.CP.HighlightTackle";
    }

    public static string InventoryName(string basketId)
    {
        return $"{InventoryIDPrefix}.{basketId}";
    }
}