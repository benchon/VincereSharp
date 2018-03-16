using System;
using Newtonsoft.Json;

namespace VincereSharp
{
    public partial class Candidate
    {
        [JsonProperty("affiliations")]
        public string Affiliations { get; set; }

        [JsonProperty("availability")]
        public string Availability { get; set; }

        [JsonProperty("availability_start")]
        public DateTime? AvailabilityStart { get; set; }

        [JsonProperty("candidate_source_id")]
        public int? CandidateSourceId { get; set; }

        [JsonProperty("company_count")]
        public int? CompanyCount { get; set; }

        [JsonProperty("contract_interval")]
        public string ContractInterval { get; set; }

        [JsonProperty("contract_rate")]
        public int? ContractRate { get; set; }

        [JsonProperty("country_of_domicile")]
        public string CountryOfDomicile { get; set; }

        [JsonProperty("creator_id")]
        public int? CreatorId { get; set; }

        [JsonProperty("currency_type")]
        public string CurrencyType { get; set; }

        [JsonProperty("current_bonus")]
        public double CurrentBonus { get; set; }

        [JsonProperty("current_salary")]
        public double CurrentSalary { get; set; }

        [JsonProperty("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("desired_bonus")]
        public double DesiredBonus { get; set; }

        [JsonProperty("desired_contract_rate")]
        public int? DesiredContractRate { get; set; }

        [JsonProperty("desired_salary")]
        public double DesiredSalary { get; set; }

        [JsonProperty("driving_license_number")]
        public string DrivingLicenseNumber { get; set; }

        [JsonProperty("driving_license_type")]
        public string DrivingLicenseType { get; set; }

        [JsonProperty("education_summary")]
        public string EducationSummary { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("employment_type")]
        public string EmploymentType { get; set; }

        [JsonProperty("experience")]
        public string Experience { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("first_name_kana")]
        public string FirstNameKana { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("gender_title")]
        public string GenderTitle { get; set; }

        [JsonProperty("highest_job_application_stage")]
        public string HighestJobApplicationStage { get; set; }

        [JsonProperty("home_phone")]
        public string HomePhone { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("ielts_score")]
        public int? IeltsScore { get; set; }

        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("last_name_kana")]
        public string LastNameKana { get; set; }

        [JsonProperty("linked_contact_id")]
        public int? LinkedContactId { get; set; }

        [JsonProperty("linked_in")]
        public string LinkedIn { get; set; }

        [JsonProperty("marital_status")]
        public string MaritalStatus { get; set; }

        [JsonProperty("met_status")]
        public string MetStatus { get; set; }

        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        [JsonProperty("middle_name_kana")]
        public string MiddleNameKana { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("nearest_train_station")]
        public string NearestTrainStation { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("note_by")]
        public int? NoteBy { get; set; }

        [JsonProperty("note_on")]
        public DateTime? NoteOn { get; set; }

        [JsonProperty("notice_days")]
        public int? NoticeDays { get; set; }

        [JsonProperty("objective")]
        public string Objective { get; set; }

        [JsonProperty("other_benefits")]
        public string OtherBenefits { get; set; }

        [JsonProperty("payslip_email")]
        public string PayslipEmail { get; set; }

        [JsonProperty("passport_no")]
        public string PassportNo { get; set; }

        [JsonProperty("personal_statements")]
        public string PersonalStatements { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("photo_url")]
        public string PhotoUrl { get; set; }

        [JsonProperty("place_of_birth")]
        public string PlaceOfBirth { get; set; }

        [JsonProperty("preferred_language")]
        public string PreferredLanguage { get; set; }

        [JsonProperty("present_salary_rate")]
        public double PresentSalaryRate { get; set; }

        [JsonProperty("publications")]
        public string Publications { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("registration_date")]
        public DateTime? RegistrationDate { get; set; }

        [JsonProperty("relocate")]
        public bool Relocate { get; set; }

        [JsonProperty("salary_months_per_year")]
        public int? SalaryMonthsPerYear { get; set; }

        [JsonProperty("salary_type")]
        public string SalaryType { get; set; }

        [JsonProperty("skills")]
        public string Skills { get; set; }

        [JsonProperty("statements")]
        public string Statements { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("toeic_score")]
        public int? ToeicScore { get; set; }

        [JsonProperty("total_gross")]
        public int? TotalGross { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }

        [JsonProperty("visa_note")]
        public string VisaNote { get; set; }

        [JsonProperty("visa_number")]
        public string VisaNumber { get; set; }

        [JsonProperty("visa_renewal_date")]
        public DateTime? VisaRenewalDate { get; set; }

        [JsonProperty("visa_status")]
        public string VisaStatus { get; set; }

        [JsonProperty("visa_type")]
        public string VisaType { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("work_email")]
        public string WorkEmail { get; set; }

        [JsonProperty("work_phone")]
        public string WorkPhone { get; set; }

        [JsonProperty("xing")]
        public string Xing { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   CandidateSourceId != null &&
                   RegistrationDate != null;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new NullReferenceException("Email is required");

            if (CandidateSourceId == null)
                throw new NullReferenceException("Candidate Source Id is required");

            if (RegistrationDate == null)
                throw new NullReferenceException("Registration Date is required");

        }
    }

    public class CandidateDeleteReason
    {
        public CandidateDeleteReason(string reason)
        {
            Reason = reason;
        }

        [JsonProperty("reason")]
        public string Reason { get; private set; }
    }
}
