using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class Address
    {
        [JsonProperty("address")]
        public string AddressAddress { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("latitude")]
        public int? Latitude { get; set; }

        [JsonProperty("location_name")]
        public string LocationName { get; set; }

        [JsonProperty("longitude")]
        public int? Longitude { get; set; }

        [JsonProperty("nearest_train_station")]
        public string NearestTrainStation { get; set; }

        [JsonProperty("post_code")]
        public string PostCode { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    } 
}
