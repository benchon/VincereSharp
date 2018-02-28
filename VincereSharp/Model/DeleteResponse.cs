using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class DeleteResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("httpStatus")]
        public string HttpStatus { get; set; }

        [JsonProperty("errorId")]
        public string ErrorId { get; set; }
    }
}
