namespace Furnace.Actions.Minecraft;

public struct MinecraftRunActionData
{
    public Data.Game.MinecraftGameManifest GameManifest;
    public Data.MinecraftAssetsManifest AssetsManifest;
    public string TargetVersion;
    public string LocalRootFolder;
}