using Newtonsoft.Json;

namespace Furnace.Actions.Minecraft.Data;

public enum GameType { OldAlpha, OldBeta, Release, Snapshot };

internal class GameTypeJsonConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(GameType) || t == typeof(GameType?);

    public override object? ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        return value switch
        {
            "old_alpha" => GameType.OldAlpha,
            "old_beta" => GameType.OldBeta,
            "release" => GameType.Release,
            "snapshot" => GameType.Snapshot,
            _ => throw new Exception("Cannot unmarshal type GameType")
        };
    }

    public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (GameType)untypedValue;
        switch (value)
        {
            case GameType.OldAlpha:
                serializer.Serialize(writer, "old_alpha");
                return;
            case GameType.OldBeta:
                serializer.Serialize(writer, "old_beta");
                return;
            case GameType.Release:
                serializer.Serialize(writer, "release");
                return;
            case GameType.Snapshot:
                serializer.Serialize(writer, "snapshot");
                return;
            default:
                throw new Exception("Cannot marshal type GameType");
        }
    }

    public static readonly GameTypeJsonConverter Singleton = new();
}