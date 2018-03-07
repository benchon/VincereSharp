using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class SearchResult<T>
    {
        [JsonProperty("result")]
        public Result<T> Result { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public partial class Result<T>
    {
        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("items")]
        public T[] Items { get; set; }
    }
}
