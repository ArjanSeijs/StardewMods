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

        configMenu.AddBoolOption(mod.ModManifest,
            name: () => "Use Custom Inventory Logic",
            tooltip: () =>
                "Disable this if you want the stack size to always be 999 or have issues with (modded) items losing data",
            getValue: () => ModEntry.Mod.Config.UseCustomInventoryLogic,
            setValue: val => ModEntry.Mod.Config.UseCustomInventoryLogic = val);

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

    public bool UseCustomInventoryLogic = true;
}