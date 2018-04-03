using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class ReferenceResponse
    {
        [JsonProperty("value_search")]
        public string ValueSearch { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("source_type")]
        public string SourceType { get; set; }
    }
}
