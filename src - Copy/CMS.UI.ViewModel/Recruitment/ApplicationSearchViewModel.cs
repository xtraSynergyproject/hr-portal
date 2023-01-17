using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;

namespace CMS.UI.ViewModel
{
    public class ApplicationSearchViewModel 
    {
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string BatchId { get; set; }
        public string RequiredCount { get; set; }
        public string JobTitleForHiring { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Age { get; set; }
        public string Religion { get; set; }
        public double? TotalExperience { get; set; }
        public string JobTitle { get; set; }
        public double? YearOfJobExperience { get; set; }
        public string OtherExperience { get; set; }
        public double? YearOfOtherCountryExperience { get; set; }
        public string Industry { get; set; }
        public double? YearOfIndustryExperience { get; set; }
        public string Category { get; set; }
        public double? CategoryExperience { get; set; }
        public string Proficiency { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string ProficiencyLevel { get; set; }
        public double ExpectedSalary { get; set; }
        public string NetSalary { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        //public string Description { get; set; }
        public string Duration { get; set; }
        public string Marks { get; set; }
        public string PassingYear { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PassportNumber { get; set; }
        public bool IsEnglishProficiency { get; set; }
        public bool IsArabicProficiency { get; set; }
        public bool IsComputerLiteratureProficiency { get; set; }
        public string EnglishProficiency { get; set; }
        public string ArabicProficiency { get; set; }
        public string ComputerLiteratureProficiency { get; set; }
        public string DL { get; set; }
        public bool JobApplicationSearch { get; set; }
        public bool CandidateProfileSearch { get; set; }
        public bool AllCandidateApplication { get; set; }
        public bool ShortlistedCandidateApplication { get; set; }
        public bool RejectedCandidateSearch { get; set; }
        public bool WaitlistedCandidateSearch { get; set; }
        public string ApplicationStateCode { get; set; }
        public string BatchStatusCode { get; set; }
        public string JobAdvertisementId { get; set; }
        public double? TotalGulfExperience { get; set; }
        public string ManpowerType { get; set; }
        public string Comment { get; set; }
        public string ApplicationStatusCode { get; set; }
        public string TemplateCode { get; set; }
        public string JobId { get; set; }
        public string BatchHiringManagerId { get; set; }
        public bool IsDashboard { get; set; }
        public string StageId { get; set; }
        public string ApplicationStatusId { get; set; }
        public string Mode { get; set; }
    }
}
