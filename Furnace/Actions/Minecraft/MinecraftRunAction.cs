using Furnace.Actions.Minecraft.Data;
using Furnace.Actions.Minecraft.Data.Game;

namespace Furnace.Actions.Minecraft;

public class MinecraftRunAction : IAction
{
    public event EventHandler? ActionCompletedEvent;
    private MinecraftRunActionData _data;

    public MinecraftRunAction(string version)
    {
        _data = new MinecraftRunActionData
        {
            TargetVersion = version,
            LocalRootFolder = Path.Join(Directory.GetCurrentDirectory(), "Minecraft")
        };
    }
    
    public async Task RunAsync()
    {
        await LoadManifestsAsync();
    }

    private async Task LoadManifestsAsync()
    {
        var text = await File.ReadAllTextAsync(Path.Combine(_data.LocalRootFolder, _data.GameManifest.Id,
            "manifest.json"));
        _data.GameManifest = MinecraftGameManifest.FromJson(text);
        _data.AssetsManifest = MinecraftAssetsManifest.FromJson(await File.ReadAllTextAsync(Path.Combine(_data.LocalRootFolder, "Assets", "Indexes", $"{_data.GameManifest!.AssetIndex.Id}.json")));
    }
}