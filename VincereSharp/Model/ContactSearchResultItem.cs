using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class ContactSearchResultItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company")]
        public Company Company { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("phone")]
        public string[] Phone { get; set; }

        [JsonProperty("job_title")]
        public string JobTitle { get; set; }

        [JsonProperty("email")]
        public string[] Email { get; set; }
    }
}