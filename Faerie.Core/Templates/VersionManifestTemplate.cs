using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Faerie.Core.Templates
{
    class LatestTemplate
    {
        [JsonPropertyName("release")]
        public string? Release { get; set; }
        [JsonPropertyName("snapshot")]
        public string? Snapshot { get; set; }
    }
    class VersionsTemplate
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("time")]
        public string? Time { get; set; }
        [JsonPropertyName("releaseTime")]
        public string? ReleaseTime { get; set; }
    }

    internal class VersionManifestTemplate
    {
        [JsonPropertyName("latest")]
        public LatestTemplate? Latest { get; set; }
        [JsonPropertyName("versions")]
        public List<VersionsTemplate>? Versions { get; set; }
    }
}
