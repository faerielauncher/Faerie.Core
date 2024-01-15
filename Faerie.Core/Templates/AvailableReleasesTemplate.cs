using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Faerie.Core.Templates
{
    internal class AvailableReleasesTemplate
    {
        [JsonPropertyName("available_lts_releases")]
        public required List<int> Available_lts_releases { get; set; }
        [JsonPropertyName("available_releases")]
        public required List<int> Available_releases { get; set; }
        [JsonPropertyName("most_recent_feature_release")]
        public required int Most_recent_feature_release { get; set; }
        [JsonPropertyName("most_recent_feature_version")]
        public required int Most_recent_feature_version { get; set; }
        [JsonPropertyName("most_recent_lts")]
        public required int Most_recent_lts { get; set; }
        [JsonPropertyName("tip_version")]
        public required int Tip_version { get; set; }
    }
}
