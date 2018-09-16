using Newtonsoft.Json;

namespace VincereSharp
{
    public class CandidateSearchResultItem
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("current_job_title")]
        public string CurrentJobTitle { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
