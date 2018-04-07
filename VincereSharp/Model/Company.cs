namespace VincereSharp
{
    using Newtonsoft.Json;

    public partial class Company
    {
        [JsonProperty("billing_group_name")]
        public string BillingGroupName { get; set; }

        [JsonProperty("business_number")]
        public string BusinessNumber { get; set; }

        [JsonProperty("careersite_url")]
        public string CareersiteUrl { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("company_number")]
        public string CompanyNumber { get; set; }

        [JsonProperty("company_owners")]
        public int[] CompanyOwners { get; set; }

        [JsonProperty("contact_in_company")]
        public int? ContactInCompany { get; set; }

        [JsonProperty("creator_id")]
        public int? CreatorId { get; set; }

        [JsonProperty("employees_number")]
        public int? EmployeesNumber { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("facebook_url")]
        public string FacebookUrl { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("general_PO_number")]
        public string GeneralPoNumber { get; set; }

        [JsonProperty("head_quarter")]
        public string HeadQuarter { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("linkedin_url")]
        public string LinkedinUrl { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("registration_date")]
        public System.DateTimeOffset RegistrationDate { get; set; }

        [JsonProperty("rss_urls")]
        public string RssUrls { get; set; }

        [JsonProperty("switch_board")]
        public string SwitchBoard { get; set; }

        [JsonProperty("tax_exempt")]
        public bool? TaxExempt { get; set; }

        [JsonProperty("trading_name")]
        public string TradingName { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(CompanyName) &&
                   RegistrationDate != null;
        }
    }

    public partial class Company
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
