namespace Furnace.Actions.Minecraft.Data.Version;

using System;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class MinecraftVersionManifest
{
    [JsonProperty("latest")]
    public Latest Latest { get; set; }

    [JsonProperty("versions")]
    public Version[] Versions { get; set; }
}

public class Latest
{
    [JsonProperty("release")]
    public string Release { get; set; }

    [JsonProperty("snapshot")]
    public string Snapshot { get; set; }
}

public partial class Version
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("type")]
    public GameType Type { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("time")]
    public DateTimeOffset Time { get; set; }

    [JsonProperty("releaseTime")]
    public DateTimeOffset ReleaseTime { get; set; }

    [JsonProperty("sha1")]
    public string Sha1 { get; set; }

    [JsonProperty("complianceLevel")]
    public long ComplianceLevel { get; set; }
}

public partial class MinecraftVersionManifest
{
    public static MinecraftVersionManifest? FromJson(string jsonString) =>
        JsonConvert.DeserializeObject<MinecraftVersionManifest>(jsonString, Converter.Settings);

    public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            GameTypeJsonConverter.Singleton,
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}