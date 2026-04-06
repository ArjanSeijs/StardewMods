using ShedMonitor.apis;
using StardewModdingAPI;

namespace ShedMonitor;

public class Config
{
    public static Config Instance { get; private set; } = null!;
    public SButton OpenKey { get; private set; } = SButton.B;

    public static void Register(ModEntry modEntry)
    {
        Instance = modEntry.Helper.ReadConfig<Config>();
        var configMenu =
            modEntry.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
            return;

        // register mod
        configMenu.Register(
            mod: modEntry.ModManifest,
            reset: () => Instance = new Config(),
            save: () => modEntry.Helper.WriteConfig(Instance)
        );

        configMenu.AddKeybind(
            mod: modEntry.ModManifest,
            setValue: value => Instance.OpenKey = value,
            getValue: () => Instance.OpenKey,
            name: () => "Key");
    }
}