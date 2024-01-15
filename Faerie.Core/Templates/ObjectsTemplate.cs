using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Faerie.Core.Templates
{
    class FileTemplate
    {
        [JsonPropertyName("hash")]
        public string? Hash { get; set; }
        [JsonPropertyName("size")]
        public int? Size { get; set; }
    }
    internal class ObjectsTemplate
    {
        [JsonPropertyName("objects")]
        public Dictionary<string, FileTemplate>? Objects { get; set; }
    }
}
