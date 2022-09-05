namespace Furnace.Actions.Minecraft.Data;

public class RemoteFile
{
    public string Path;
    public Uri Uri;

    public RemoteFile(string path, Uri uri)
    {
        Path = path;
        Uri = uri;
    }
}