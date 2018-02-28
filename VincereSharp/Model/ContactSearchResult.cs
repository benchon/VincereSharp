using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class ContactSearchResult
    {
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("items")]
        public ContactSearchResultItem[] Items { get; set; }
    }

    public partial class ContactSearchResultItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company")]
        public Company Company { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("phone")]
        public string[] Phone { get; set; }

        [JsonProperty("job_title")]
        public string JobTitle { get; set; }

        [JsonProperty("email")]
        public string[] Email { get; set; }
    }

    public partial class Company
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }


}
