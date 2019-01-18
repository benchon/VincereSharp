using Newtonsoft.Json;
using System;

namespace VincereSharp.Model
{
    public partial class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("httpStatus")]
        public int HttpStatus { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorId")]
        public Guid ErrorId { get; set; }

        [JsonProperty("errors")]
        public string[] Errors { get; set; }
    }
}