using Newtonsoft.Json;

namespace Furnace.Actions.Minecraft.Data;

public enum Architecture
{
    X86, X64, Arm
}

internal class ArchitectureJsonConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(Architecture) || t == typeof(Architecture?);

    public override object? ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        return value switch
        {
            "x86" => Architecture.X86,
            "x64" => Architecture.X64,
            "arm" => Architecture.Arm,
            _ => throw new Exception("Cannot unmarshal type Architecture")
        };
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (Architecture)untypedValue;
        switch (value)
        {
            case Architecture.X64:
                serializer.Serialize(writer, "x64");
                return;
            case Architecture.X86:
                serializer.Serialize(writer, "x86");
                return;
            case Architecture.Arm:
                serializer.Serialize(writer, "arm");
                return;
            default:
                throw new Exception("Cannot marshal type Architecture");
        }
        
    }

    public static readonly ArchitectureJsonConverter Singleton = new();
}