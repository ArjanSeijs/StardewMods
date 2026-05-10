using BasketMod.api;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace BasketMod;

public class Config
{
    public static void Initialize(ModEntry mod, IGenericModConfigMenuApi configMenu)
    {
        ModEntry.Mod.Config = mod.Helper.ReadConfig<Config>();
        
        configMenu.Register(mod.ModManifest,
            () => ModEntry.Mod.Config = new Config(),
            save: () => mod.Helper.WriteConfig(ModEntry.Mod.Config));

        for (var i = 0; i < SpaceCoreEquipment.MaxSlots; i++)
        {
            var index = i;
            configMenu.AddKeybindList(
                mod.ModManifest,
                name: () => "Bag_" + index,
                tooltip: () => "Open item in ",
                getValue: () => ModEntry.Mod.Config.Buttons[index] ?? new KeybindList(),
                setValue: list => ModEntry.Mod.Config.Buttons[index] = list);
        }
    }

    public KeybindList?[] Buttons = new KeybindList[SpaceCoreEquipment.MaxSlots];
}