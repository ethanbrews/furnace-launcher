namespace Furnace;

public static class Constants
{
    public static Uri AppInstallerUri => new Uri("http://storage.ethanbrews.me/Furnance/Alpha/Furnace.appinstaller");
    public static Uri MinecraftVersionManifestUri => new Uri("https://piston-meta.mojang.com/mc/game/version_manifest_v2.json");
    public static string MinecraftResourcesUri => "http://resources.download.minecraft.net/{0}/{1}";
    /*
    public static Uri AppInstallerUri => new Uri("http://storage.ethanbrews.me/Furnance/Alpha/Furnace.appinstaller");
    public static Uri MinecraftLauncherUri => new Uri("https://launcher.mojang.com/download/Minecraft.exe");
    public static Uri MinecraftVersionManifestUri => new Uri("https://launchermeta.mojang.com/mc/game/version_manifest.json");
    public static Uri FabricInstallerApiEndpoint => new Uri("https://meta.fabricmc.net/v2/versions/installer");
    public static Uri FabricLoaderApiEndpoint => new Uri("https://meta.fabricmc.net/v1/versions/loader");
    public static Uri YggdrasilApiEndpoint => new Uri("https://authserver.mojang.com");

    public static Uri TwitchModEndpoint => new Uri("https://addons-ecs.forgesvc.net/api/v2/");
    public static Uri TwitchModDownloadEndpoint = new Uri(TwitchModEndpoint, "addon/{0}/file/{1}");
    */
}