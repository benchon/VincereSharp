using System;
using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class Job
    {
        [JsonProperty("auto_submit_candidate")]
        public bool AutoSubmitCandidate { get; set; }

        [JsonProperty("close_date")]
        public DateTime? CloseDate { get; set; }

        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("company_location_id")]
        public int? CompanyLocationId { get; set; }

        [JsonProperty("compensation")]
        public Compensation Compensation { get; set; }

        [JsonProperty("contact_id")]
        public int? ContactId { get; set; }

        [JsonProperty("contract_length")]
        public int? ContractLength { get; set; }

        [JsonProperty("creator_id")]
        public int? CreatorId { get; set; }

        [JsonProperty("difficulty_level")]
        public int? DifficultyLevel { get; set; }

        [JsonProperty("employment_type")]
        public string EmploymentType { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("floated_job")]
        public bool FloatedJob { get; set; }

        [JsonProperty("forecast_annual_fee")]
        public int? ForecastAnnualFee { get; set; }

        [JsonProperty("forecast_annual_fee_currency")]
        public string ForecastAnnualFeeCurrency { get; set; }

        [JsonProperty("head_count")]
        public int? HeadCount { get; set; }

        [JsonProperty("hiring_line_manager")]
        public string HiringLineManager { get; set; }

        [JsonProperty("hot_end_date")]
        public DateTime? HotEndDate { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("indeed_published_date")]
        public DateTime? IndeedPublishedDate { get; set; }

        [JsonProperty("industry_id")]
        public int? IndustryId { get; set; }

        [JsonProperty("internal_description")]
        public string InternalDescription { get; set; }

        [JsonProperty("internal_recruiter_contact_id")]
        public int? InternalRecruiterContactId { get; set; }

        [JsonProperty("job_title")]
        public string JobTitle { get; set; }

        [JsonProperty("job_type")]
        public string JobType { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("open_date")]
        public DateTime? OpenDate { get; set; }

        [JsonProperty("pay_rate")]
        public int? PayRate { get; set; }

        [JsonProperty("percentage_placement")]
        public int? PercentagePlacement { get; set; }

        [JsonProperty("projected_placement_date")]
        public DateTime? ProjectedPlacementDate { get; set; }

        [JsonProperty("private_job")]
        public bool PrivateJob { get; set; }

        [JsonProperty("public_description")]
        public string PublicDescription { get; set; }

        [JsonProperty("reason_for_difficulty")]
        public string ReasonForDifficulty { get; set; }

        [JsonProperty("registration_date")]
        public DateTime? RegistrationDate { get; set; }

        [JsonProperty("site_manager_contact_id")]
        public int? SiteManagerContactId { get; set; }

        [JsonProperty("skill_keywords")]
        public string SkillKeywords { get; set; }

        [JsonProperty("sourcing_difficulty")]
        public int? SourcingDifficulty { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("update_user_id")]
        public int? UpdateUserId { get; set; }

        [JsonProperty("updated_timestamp")]
        public DateTime? UpdatedTimestamp { get; set; }

        [JsonProperty("visible_to_all")]
        public bool VisibleToAll { get; set; }
    }

    public partial class Compensation
    {
        [JsonProperty("contract_length")]
        public int? ContractLength { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("formatted_pay_rate")]
        public string FormattedPayRate { get; set; }

        [JsonProperty("formatted_salary_from")]
        public string FormattedSalaryFrom { get; set; }

        [JsonProperty("formatted_salary_to")]
        public string FormattedSalaryTo { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("pay_rate")]
        public int? PayRate { get; set; }

        [JsonProperty("salary_from")]
        public int? SalaryFrom { get; set; }

        [JsonProperty("salary_to")]
        public int? SalaryTo { get; set; }

        [JsonProperty("salary_type")]
        public string SalaryType { get; set; }
    }
}
