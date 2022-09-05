using Furnace.Actions.Minecraft.Data;
using Furnace.Actions.Minecraft.Data.Game;
using Furnace.Actions.Minecraft.Data.Version;

namespace Furnace.Actions.Minecraft;

public struct MinecraftInstallActionData
{
    public string? TargetVersion;
    public string LocalRootFolder;
    public MinecraftVersionManifest? VersionManifest;
    public MinecraftAssetsManifest? AssetsManifest;
    public MinecraftGameManifest? GameManifest;

    public List<RemoteLibrary> Libraries;
    public List<RemoteFile> Assets;
    public List<RemoteFile> GameFiles;

    public List<RemoteFile> AllRemoteFiles => Libraries
        .Concat(Assets)
        .Concat(GameFiles)
        .ToList();
}