namespace Furnace.Actions.Minecraft.Data;

public class RemoteLibrary : RemoteFile
{

    public bool IsNative;
    
    public RemoteLibrary(string path, Uri uri, bool isNative) : base(path, uri)
    {
        IsNative = isNative;
    }
}