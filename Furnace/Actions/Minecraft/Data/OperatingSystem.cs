using Newtonsoft.Json;

namespace Furnace.Actions.Minecraft.Data;

public enum OperatingSystem
{
    Mac, Windows, Linux
}

internal class OperatingSystemJsonConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(OperatingSystem) || t == typeof(OperatingSystem?);

    public override object? ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        return value switch
        {
            "linux" => OperatingSystem.Linux,
            "osx" => OperatingSystem.Mac,
            "windows" => OperatingSystem.Windows,
            _ => throw new Exception("Cannot unmarshal type OperatingSystem")
        };
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (OperatingSystem)untypedValue;
        switch (value)
        {
            case OperatingSystem.Linux:
                serializer.Serialize(writer, "linux");
                return;
            case OperatingSystem.Mac:
                serializer.Serialize(writer, "osx");
                return;
            case OperatingSystem.Windows:
                serializer.Serialize(writer, "windows");
                return;
            default:
                throw new Exception("Cannot marshal type Name");
        }
        
    }

    public static readonly OperatingSystemJsonConverter Singleton = new();
}