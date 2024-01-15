using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Faerie.Core.Templates
{
    class ArgumentsTemplate
    {
        [JsonPropertyName("game")]
        public required List<object> Game { get; set; }
        [JsonPropertyName("jvm")]
        public required List<object> Jvm { get; set; }
    }

    class AssetIndexTemplate
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        [JsonPropertyName("sha1")]
        public required string Sha1 { get; set; }
        [JsonPropertyName("size")]
        public required int Size { get; set; }
        [JsonPropertyName("totalSize")]
        public required int TotalSize { get; set; }

        [JsonPropertyName("url")]
        public required string Url { get; set; }
    }
    class FilePropertyTemplate
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }
        [JsonPropertyName("sha1")]
        public string? Sha1 { get; set; }
        [JsonPropertyName("size")]
        public int? Size { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
    class DownloadsTemplate
    {
        [JsonPropertyName("client")]
        public required FilePropertyTemplate Client { get; set; }
        [JsonPropertyName("client_mappings")]
        public required FilePropertyTemplate Client_mappings { get; set; }
        [JsonPropertyName("server")]
        public required FilePropertyTemplate Server { get; set; }
        [JsonPropertyName("server_mappings")]
        public required FilePropertyTemplate Server_mappings { get; set; }
    }
    class JavaVersionTemplate
    {
        [JsonPropertyName("component")]
        public required string component { get; set; }
        [JsonPropertyName("majorVersion")]
        public required int majorVersion { get; set; }

    }

    class ArtifactTemplate
    {
        [JsonPropertyName("artifact")]
        public FilePropertyTemplate? Artifact { get; set; }
    }

    class RuleTemplate
    {
        [JsonPropertyName("action")]
        public string? Action { get; set; }
        [JsonPropertyName("os")]
        public Dictionary<string, string>? Os { get; set; }
        [JsonPropertyName("features")]
        public Dictionary<string, bool>? Features { get; set; }
    }

    class RulesTemplate
    {
        [JsonPropertyName("rules")]
        public List<RuleTemplate>? Rules { get; set; }
        [JsonPropertyName("value")]
        public object? Value { get; set; }
    }
    class LibrariesTemplate
    {
        [JsonPropertyName("downloads")]
        public ArtifactTemplate? Downloads { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("rules")]
        public List<RulesTemplate>? Rules { get; set; }
    }

    class LoggingClientTemplate
    {
        [JsonPropertyName("argument")]
        public required string Argument { get; set; }
        [JsonPropertyName("file")]
        public required FilePropertyTemplate File { get; set; }
        [JsonPropertyName("type")]
        public required string Type { get; set; }
    }
    internal class VersionTemplate
    {
        [JsonPropertyName("arguments")]
        public required ArgumentsTemplate Arguments { get; set; }
        [JsonPropertyName("assetIndex")]
        public required AssetIndexTemplate AssetIndex { get; set; }
        [JsonPropertyName("assets")]
        public required string Assets { get; set; }
        [JsonPropertyName("complianceLevel")]
        public required int ComplianceLevel { get; set; }
        [JsonPropertyName("downloads")]
        public required DownloadsTemplate Downloads { get; set; }
        [JsonPropertyName("javaVersion")]
        public required JavaVersionTemplate JavaVersion { get; set; }
        [JsonPropertyName("libraries")]
        public required List<LibrariesTemplate> Libraries { get; set; }
        [JsonPropertyName("logging")]
        public required Dictionary<string, LoggingClientTemplate> Client { get; set; }
        [JsonPropertyName("mainClass")]
        public required string MainClass { get; set; }
        [JsonPropertyName("minimumLauncherVersion")]
        public required int MinimumLauncherVersion { get; set; }
        [JsonPropertyName("releaseTime")]
        public required string ReleaseTime { get; set; }
        [JsonPropertyName("time")]
        public required string Time { get; set; }
        [JsonPropertyName("type")]
        public required string Type { get; set; }
    }
}
