using Newtonsoft.Json;
using System;

namespace VincereSharp.Model
{
    public partial class EducationDetail
    {
        [JsonProperty("course")]
        public string Course { get; set; }

        [JsonProperty("degree_name")]
        public string DegreeName { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("education_level")]
        public string EducationLevel { get; set; }

        [JsonProperty("gpa")]
        public string Gpa { get; set; }

        [JsonProperty("grade")]
        public string Grade { get; set; }

        [JsonProperty("graduation_date")]
        public DateTimeOffset? GraduationDate { get; set; }

        [JsonProperty("honorific")]
        public string Honorific { get; set; }

        [JsonProperty("honors")]
        public string Honors { get; set; }

        [JsonProperty("institution_address")]
        public string InstitutionAddress { get; set; }

        [JsonProperty("institution_name")]
        public string InstitutionName { get; set; }

        [JsonProperty("major")]
        public string Major { get; set; }

        [JsonProperty("minor")]
        public string Minor { get; set; }

        [JsonProperty("qualification")]
        public string Qualification { get; set; }

        [JsonProperty("school_name")]
        public string SchoolName { get; set; }

        [JsonProperty("school_address")]
        public string SchoolAddress { get; set; }

        [JsonProperty("start_date")]
        public DateTimeOffset? StartDate { get; set; }

        [JsonProperty("thesis")]
        public string Thesis { get; set; }

        [JsonProperty("training")]
        public string Training { get; set; }
    }
}