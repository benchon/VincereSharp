using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class WorkExperience
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("current_employer")]
        public bool CurrentEmployer { get; set; }

        [JsonProperty("experience_in_company")]
        public string ExperienceInCompany { get; set; }

        [JsonProperty("functional_expertise_id")]
        public long FunctionalExpertiseId { get; set; }

        [JsonProperty("industry_id")]
        public long IndustryId { get; set; }

        [JsonProperty("job_title")]
        public string JobTitle { get; set; }

        [JsonProperty("sub_function_id")]
        public long SubFunctionId { get; set; }

        [JsonProperty("work_from")]
        public System.DateTimeOffset WorkFrom { get; set; }

        [JsonProperty("work_to")]
        public System.DateTimeOffset WorkTo { get; set; }
    }
}
