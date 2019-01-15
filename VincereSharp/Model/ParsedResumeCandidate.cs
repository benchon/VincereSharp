using Newtonsoft.Json;
using System.Collections.Generic;

namespace VincereSharp.Model
{
    public partial class ParsedResumeCandidate : Candidate
    {
        [JsonProperty("education_details")]
        public List<EducationDetail> EducationDetails { get; set; }

        [JsonProperty("work_experiences")]
        public List<WorkExperience> WorkExperiences { get; set; }
    }
}