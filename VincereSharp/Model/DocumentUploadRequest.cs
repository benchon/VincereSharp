using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class DocumentUploadRequest
    {
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("document_type_id")]
        public long DocumentTypeId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("base_64_content")]
        public string Base64_Content { get; set; }

        [JsonProperty("original_cv")]
        public bool OriginalCv { get; set; }
    }

    public partial class DocumentUploadResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public partial class ResumeParseRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
