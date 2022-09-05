using Newtonsoft.Json;

namespace Furnace.Actions.Minecraft.Data;

public enum AllowAction
{
    Allow, Disallow
}

internal class AllowActionJsonConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(AllowAction) || t == typeof(AllowAction?);

    public override object? ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        return value switch
        {
            "allow" => AllowAction.Allow,
            "disallow" => AllowAction.Disallow,
            _ => throw new Exception("Cannot unmarshal type Action")
        };
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (AllowAction)untypedValue;
        switch (value)
        {
            case AllowAction.Allow:
                serializer.Serialize(writer, "allow");
                return;
            case AllowAction.Disallow:
                serializer.Serialize(writer, "disallow");
                return;
            default:
                throw new Exception("Cannot marshal type Action");
        }
        
    }

    public static readonly AllowActionJsonConverter Singleton = new();
}