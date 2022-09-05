using System.Net;
using System.IO;
using Furnace.Actions.Minecraft.Data;
using Furnace.Actions.Minecraft.Data.Game;
using Furnace.Actions.Minecraft.Data.Version;
using File = System.IO.File;

namespace Furnace.Actions.Minecraft;

public class MinecraftInstallAction : IAction, IProgress
{
    private HttpClient _webClient;
    private MinecraftInstallActionData _data;
    public event EventHandler? ActionCompletedEvent;
    public event EventHandler<int>? ProgressChangedEvent;

    public MinecraftInstallAction(string version)
    {
        _data = new();
        _data.TargetVersion = version;
        _webClient = new();
        _data.LocalRootFolder = Path.Join(Directory.GetCurrentDirectory(), "Minecraft");
    }

    public async Task RunAsync()
    {
        ProgressChangedEvent?.Invoke(this, 0);
        await LoadManifestsAsync();
        ProgressChangedEvent?.Invoke(this, 10);
        GenerateLibrariesList();
        ProgressChangedEvent?.Invoke(this, 20);
        GenerateAssetsList();
        ProgressChangedEvent?.Invoke(this, 30);
        GenerateGameFilesList();
        ProgressChangedEvent?.Invoke(this, 40);
        await DownloadAllFilesAsync();
        ProgressChangedEvent?.Invoke(this, 90);
        await WriteManifestsAsync();
        ProgressChangedEvent?.Invoke(this, 100);
        
        ActionCompletedEvent?.Invoke(this, EventArgs.Empty);
    }

    private static FileStream CreateFileSafe(string path)
    {
        CreateDirectoryForFile(path);
        return File.Create(path);
    }

    private static void CreateDirectoryForFile(string path)
    {
        var fileInfo = new System.IO.FileInfo(path);
        fileInfo.Directory?.Create();
    }

    private async Task LoadManifestsAsync()
    {
        var targetVersion = _data.TargetVersion!;
        _data.VersionManifest = MinecraftVersionManifest.FromJson(await _webClient.GetStringAsync(Constants.MinecraftVersionManifestUri));
        var targetVersionUri = _data.VersionManifest.Versions.First(x => x.Id == targetVersion).Url;
        _data.GameManifest = MinecraftGameManifest.FromJson(await _webClient.GetStringAsync(targetVersionUri));
        _data.AssetsManifest = MinecraftAssetsManifest.FromJson(await _webClient.GetStringAsync(_data.GameManifest.AssetIndex.Url));
    }

    private void GenerateLibrariesList()
    {
        _data.Libraries = new List<RemoteLibrary>();
        
        foreach (var lib in _data.GameManifest!.Libraries)
        {
            // Standard Java Library - TODO: Check OS!
            _data.Libraries.Add(new(Path.Combine("Libraries", lib.Downloads.Artifact.Path), lib.Downloads.Artifact.Url, false));
        }
    }

    private void GenerateAssetsList()
    {
        _data.Assets = _data.AssetsManifest!.Objects.Select(asset => new RemoteFile(
            Path.Combine("Assets", "Objects", asset.Value.Hash[..2], asset.Value.Hash),
            new Uri(string.Format(Constants.MinecraftResourcesUri, asset.Value.Hash[..2], asset.Value.Hash))
        )).ToList();
    }

    private void GenerateGameFilesList()
    {
        var gameManifest = _data.GameManifest!;
        _data.GameFiles = new List<RemoteFile>();
        
        // Client
        _data.GameFiles.Add(new(Path.Combine(gameManifest.Id, "client.jar"), gameManifest.Downloads.Client.Url));
        if (gameManifest.Downloads?.ClientMappings != null)
            _data.GameFiles.Add(new(Path.Combine(gameManifest.Id, "client.txt"), gameManifest.Downloads.ClientMappings.Url));
        
        // Server
        _data.GameFiles.Add(new(Path.Combine(gameManifest.Id, "server.jar"), gameManifest.Downloads!.Server.Url));
        if (gameManifest.Downloads?.ServerMappings != null)
            _data.GameFiles.Add(new(Path.Combine(gameManifest.Id, "server.txt"), gameManifest.Downloads.ServerMappings.Url));

        // Log4j Config
        _data.GameFiles.Add(new (Path.Combine(gameManifest.Id, "log4j.xml"), gameManifest.Logging.Client.File.Url));
    }

    private async Task DownloadAllFilesAsync(bool overwrite = false)
    {
        foreach (var file in _data.AllRemoteFiles)
        {
            var path = Path.Join(_data.LocalRootFolder, file.Path);
            var existing = File.Exists(path);

            if (!overwrite && existing) continue;
            var fileStream = CreateFileSafe(path);
            var bytes = await _webClient.GetByteArrayAsync(file.Uri);
            await fileStream.WriteAsync(bytes);
        }
    }

    private static async Task WriteLongString(string path, string longString)
    {
        await using var sw = new StreamWriter(path);
        await sw.WriteAsync(longString);
        await sw.FlushAsync();
    }

    private async Task WriteManifestsAsync()
    {
        var assetManifestPath = Path.Combine(_data.LocalRootFolder, "Assets", "Indexes", $"{_data.GameManifest!.AssetIndex.Id}.json");
        var gameManifestPath = Path.Combine(_data.LocalRootFolder, _data.GameManifest.Id, "manifest.json");

        CreateDirectoryForFile(assetManifestPath);
        CreateDirectoryForFile(gameManifestPath);

        File.WriteAllText(assetManifestPath, _data.AssetsManifest.ToJson());
        File.WriteAllText(gameManifestPath, _data.GameManifest.ToJson());
        
        Console.WriteLine(_data.GameManifest.ToJson());
    }
}