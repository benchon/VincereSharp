namespace VincereSharp
{
    using System;
    using Newtonsoft.Json;

    public partial class Contact
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("candidate_source_id")]
        public long? CandidateSourceId { get; set; }

        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        [JsonProperty("contact_owners")]
        public string[] ContactOwners { get; set; }

        [JsonProperty("creator_id")]
        public long? CreatorId { get; set; }

        [JsonProperty("customer_probability")]
        public long? CustomerProbability { get; set; }

        [JsonProperty("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("first_name_kana")]
        public string FirstNameKana { get; set; }

        [JsonProperty("gender_title")]
        public string GenderTitle { get; set; }

        [JsonProperty("home_phone")]
        public string HomePhone { get; set; }

        [JsonProperty("is_assistant")]
        public bool IsAssistant { get; set; }

        [JsonProperty("job_level")]
        public string JobLevel { get; set; }

        [JsonProperty("job_level_hierarchy")]
        public long? JobLevelHierarchy { get; set; }

        [JsonProperty("job_title")]
        public string JobTitle { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("last_name_kana")]
        public string LastNameKana { get; set; }

        [JsonProperty("linkedin")]
        public string Linkedin { get; set; }

        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        [JsonProperty("middle_name_kana")]
        public string MiddleNameKana { get; set; }

        [JsonProperty("mobile_phone")]
        public string MobilePhone { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("preferred_contact_method")]
        public string PreferredContactMethod { get; set; }

        [JsonProperty("personal_email")]
        public string PersonalEmail { get; set; }

        [JsonProperty("preferred_time_from")]
        public string PreferredTimeFrom { get; set; }

        [JsonProperty("preferred_time_to")]
        public string PreferredTimeTo { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Registration Date. Must be in format "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'.
        /// </summary>
        [JsonProperty("registration_date")]
        public DateTime? RegistrationDate { get; set; }

        [JsonProperty("report_to_contact_id")]
        public long? ReportToContactId { get; set; }

        [JsonProperty("skills")]
        public string Skills { get; set; }

        [JsonProperty("skype")]
        public string Skype { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("switchboard_phone")]
        public string SwitchboardPhone { get; set; }

        [JsonProperty("switchboard_phone_ext")]
        public string SwitchboardPhoneExt { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("xing")]
        public string Xing { get; set; }
    }

    public partial class Contact
    {
        public static Contact FromJson(string json) => JsonConvert.DeserializeObject<Contact>(json, VincereSharp.Converter.Settings);
    }

    public class ContactCreatedResponse
    {
        public int id { get; set; }
    }
}
