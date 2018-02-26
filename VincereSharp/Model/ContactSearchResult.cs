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
        public Item[] Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("current_location")]
        public CurrentLocation CurrentLocation { get; set; }
    }

    public partial class CurrentLocation
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("location_name")]
        public string LocationName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("post_code")]
        public string PostCode { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("nearest_train_station")]
        public string NearestTrainStation { get; set; }
    }

    public partial class ContactSearchResult
    {
        public static ContactSearchResult FromJson(string json) => JsonConvert.DeserializeObject<ContactSearchResult>(json, Converter.Settings);
    }

}
