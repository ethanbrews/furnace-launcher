using Newtonsoft.Json.Linq;

namespace Furnace.Actions.Minecraft.Data.Game
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MinecraftGameManifest
    {
        [JsonProperty("assets")] 
        public string Assets { get; set; }
        
        [JsonProperty("complianceLevel")] 
        public int ComplianceLevel { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mainClass")]
        public string MainClass { get; set; }
        
        [JsonProperty("minimumLauncherVersion")]
        public int MinimumLauncherVersion { get; set; }
        
        [JsonProperty("releaseTime")]
        public DateTime Time { get; set; }
        
        [JsonProperty("type")]
        public GameType GameType { get; set; }
        
        [JsonProperty("libraries")]
        public Library[] Libraries { get; set; }
        
        [JsonProperty("logging")]
        public Logging Logging { get; set; }
        
        [JsonProperty("downloads")]
        public Downloads Downloads { get; set; }
        
        [JsonProperty("javaVersion")]
        public JavaVersion JavaVersion { get; set; }
        
        [JsonProperty("assetIndex")]
        public ExtendedIdentifiedFileInfo AssetIndex { get; set; }
        
        [JsonProperty("arguments")]
        public Arguments Arguments { get; set; }
    }

    public class Arguments
    {
        [JsonProperty("game")]
        [JsonConverter(typeof(StringOrGameArgumentJsonConverter))]
        public GameArgument[] GameArguments { get; set; }
        
        [JsonProperty("jvm")]
        [JsonConverter(typeof(StringOrGameArgumentJsonConverter))]
        public GameArgument[] JvmArguments { get; set; }
    }

    public class GameArgument
    {
        [JsonProperty("value")]
        [JsonConverter(typeof(StringOrArrayJsonConverter))]
        public string[] Value { get; set; }

        [JsonProperty("rules")] public AcceptanceRule[]? Rules { get; set; }
    }

    public class JavaVersion
    {
        [JsonProperty("component")]
        public string Component { get; set; }
        
        [JsonProperty("majorVersion")]
        public int MajorVersion { get; set; }
    }

    public class Downloads
    {
        [JsonProperty("client")]
        public FileInfo Client { get; set; }
        
        [JsonProperty("client_mappings")]
        public FileInfo ClientMappings { get; set; }
        
        [JsonProperty("server")]
        public FileInfo Server { get; set; }
        
        [JsonProperty("server_mappings")]
        public FileInfo ServerMappings { get; set; }
    }
    
    public class Logging
    {
        [JsonProperty("client")]
        public LoggingClient Client { get; set; }
    }

    public class LoggingClient
    {
        [JsonProperty("argument")]
        public string Argument { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("file")]
        public IdentifiedFileInfo File { get; set; }
    }

    public class Library
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("downloads")]
        public LibraryDownload Downloads { get; set; }
    }

    public class LibraryDownload
    {
        [JsonProperty("artifact")]
        public FileInfoWithPath Artifact { get; set; }
        
        [JsonProperty("rules", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public AcceptanceRule[]? Rules { get; set; }
    }

    public class AcceptanceRule
    {
        [JsonProperty("features", Required = Required.Default)] 
        public Dictionary<string, bool>? Features { get; set; }

        [JsonProperty("action", Required = Required.Default)]
        public AllowAction? Action { get; set; }
        
        [JsonProperty("os", Required = Required.Default)]
        public OperatingSystemCondition? OperatingSystemCondition { get; set; }
    }

    public class OperatingSystemCondition
    {
        [JsonProperty("name", Required = Required.Default)]
        public Furnace.Actions.Minecraft.Data.OperatingSystem OperatingSystem { get; set; }

        [JsonProperty("arch", Required = Required.Default)]
        public Architecture Architecture { get; set; }
    }

    public class FileInfo
    {
        [JsonProperty("sha1")]
        public string Sha1 { get; set; }
        
        [JsonProperty("size")]
        public int Size { get; set; }
        
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public class FileInfoWithPath : FileInfo
    {
        [JsonProperty("path")]
        public string Path { get; set; }
    }

    public class IdentifiedFileInfo : FileInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class ExtendedIdentifiedFileInfo : IdentifiedFileInfo
    {
        [JsonProperty("totalSize")]
        public int TotalSize { get; set; }
    }

    public partial class MinecraftGameManifest
    {
        public static MinecraftGameManifest? FromJson(string jsonString) =>
            JsonConvert.DeserializeObject<MinecraftGameManifest>(jsonString, Converter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented, Converter.Settings);
    }
    
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                OperatingSystemJsonConverter.Singleton,
                AllowActionJsonConverter.Singleton,
                ArchitectureJsonConverter.Singleton,
                GameTypeJsonConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class StringOrGameArgumentJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(GameArgument);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.Type == JTokenType.String
                ? new GameArgument { Value = new[] { token.ToString() } }
                : serializer.Deserialize<GameArgument>(reader);
        }
        
        public override bool CanWrite => true;
 
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    internal class StringOrArrayJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(List<string>);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.Type == JTokenType.Array
                ? token.ToObject<List<string>>()
                : new List<string> { token.ToObject<string>()! };
        }
        
        public override bool CanWrite => true;
 
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not List<string> list || list.Count == 0)
                writer.WriteNull();
            else if (list.Count == 1)
                writer.WriteValue(list[0]);
            else
            {
                writer.WriteStartArray();
                list.ForEach(writer.WriteValue);
                writer.WriteEndArray();
            }
        }
    }
}
