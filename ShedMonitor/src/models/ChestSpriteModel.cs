using Microsoft.Xna.Framework;
using StardewUI.Graphics;
using StardewValley;
using StardewValley.Objects;

namespace ShedMonitor.Models;

public record ChestSpriteModel(Chest Chest)
{
    public Sprite Body = GetSprite(Chest, Type.Body);
    public Sprite Trim = GetSprite(Chest, Type.Trim);
    public Sprite Overlay = GetSprite(Chest, Type.Overlay);

    public string Tint => Chest.Tint.ToHex();
    public string OverlayTint => GetColorString();
    public bool DrawOverlay => !Chest.playerChoiceColor.Value.Equals(Color.Black);

    private string GetColorString()
    {
        var color = Chest.playerChoiceColor.Value;
        return color.Equals(Color.Black)
            ? Chest.Tint.ToHex()
            : color.ToHex();
    }

    private enum Type
    {
        Body = 1,
        Trim = 2,
        Overlay = 3
    }

    private static int GetSpriteIndex(Chest chest, Type type)
    {
        var body = 0;
        var trim = 8;
        var overlay = 0;
        switch (chest.QualifiedItemId)
        {
            case "(BC)130": // Chest
                body = 0;
                trim = 46;
                overlay = 38;
                break;
            case "(BC)BigChest":
                body = 0;
                trim = 16 /*0x10*/;
                overlay = 8;
                break;
            case "(BC)BigStoneChest":
                trim = 8;
                overlay = 0;
                break;
            case "(BC)256": // Junimo
            case "(BC)275": //hopper
                trim = 0;
                overlay = 0;
                break;
        }

        return type switch
        {
            Type.Body => body,
            Type.Trim => trim,
            Type.Overlay => overlay,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static Sprite GetSprite(Chest chest, Type type)
    {
        var dataOrErrorItem = ItemRegistry.GetDataOrErrorItem(chest.QualifiedItemId);
        var texture = dataOrErrorItem.GetTexture();
        var spriteIndex = GetSpriteIndex(chest, type);
        var sourceRect = dataOrErrorItem.GetSourceRect(spriteIndex);
        var sprite = new Sprite(texture, sourceRect);
        return sprite;
    }
}