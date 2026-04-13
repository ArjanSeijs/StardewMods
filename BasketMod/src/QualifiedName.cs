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
        public const string ContextTagsQuery = "EternalSoap.BasketMod.CP.ContextTagsQuery";
        public const string Inception = "EternalSoap.BasketMod.CP.Inception";   
    }

    public static string InventoryName(string basketId)
    {
        return $"{InventoryIDPrefix}.{basketId}";
    }
}