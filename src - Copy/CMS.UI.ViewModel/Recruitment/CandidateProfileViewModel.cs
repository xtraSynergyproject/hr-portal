using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;


namespace CMS.UI.ViewModel
{
    public class CandidateProfileViewModel : CandidateProfile
    {
        //public string UserId { get; set; }
        //public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        //public string LastName { get; set; }
        //public string Age { get; set; }
        //public DateTime? BirthDate { get; set; }
        //public string BirthPlace { get; set; }
        //public string NationalityId { get; set; }
        //public Nationality Nationality { get; set; }
        //public string BloodGroup { get; set; }
        //public string Gender { get; set; }
        //public string MaritalStatus { get; set; }
        //public int? NoOfChildren { get; set; }
        //public string PassportNumber { get; set; }
        //public string PassportIssueCountryId { get; set; }
        //public DateTime? PassportExpiryDate { get; set; }
        //public string QatarId { get; set; }
        //public bool IsCopyofIDPassport { get; set; }
        //public string PassportAttachmentId { get; set; }
        //public bool IsCopyofAcademicCertificates { get; set; }
        //public string AcademicCertificateId { get; set; }
        //public bool IsCopyofOtherCertificates { get; set; }
        //public string OtherCertificateId { get; set; }
        //public bool IsMostRecentColorPhoto { get; set; }
        //public string PhotoId { get; set; }
        //public bool IsMostRecentCV { get; set; }
        //public string ResumeId { get; set; }
        //public bool IsLatestOfferLetterSalarySlip { get; set; }
        //public string CoverLetterId { get; set; }
        //public string CurrentAddressHouse { get; set; }
        //public string CurrentAddressStreet { get; set; }
        //public string CurrentAddressCity { get; set; }
        //public string CurrentAddressState { get; set; }
        //public string CurrentAddress { get; set; }
        //public string CurrentAddressCountryId { get; set; }
        ////public Country CurrentAddressCountry { get; set; }
        //public string PermanentAddressHouse { get; set; }
        //public string PermanentAddressStreet { get; set; }
        //public string PermanentAddressCity { get; set; }
        //public string PermanentAddressState { get; set; }
        //public string PermanentAddress { get; set; }
        //public string PermanentAddressCountryId { get; set; }
        ////public Country PermanentAddressCountry { get; set; }
        //public string Email { get; set; }
        //public string ContactPhoneHome { get; set; }
        //public string ContactPhoneLocal { get; set; }
        //public string OptionForAnotherPosition { get; set; }
        //public string AdditionalInformation { get; set; }
        //public int? TimeRequiredToJoin { get; set; }
        //public string ManagerJobTitleAndNoOfSubordinate { get; set; }

        //public string HeardAboutUsFrom { get; set; }

        public string OverseasSalary { get; set; }
        public string IndianSalary { get; set; }
        //public string NetSalary { get; set; }
        //public string OtherAllowances { get; set; }
        //public string VisaCountry { get; set; }
        //public string VisaType { get; set; }
        //public DateTime? VisaExpiry { get; set; }
        //public string OtherVisaType { get; set; }
        //public string OtherCountryVisa { get; set; }
        //public string OtherCountryVisaType { get; set; }
        //public DateTime? OtherCountryVisaExpiry { get; set; }

        //public string QatarNocAvailable { get; set; }
        //public string QatarNocNotAvailableReason { get; set; }

        //public string TotalWorkExperience { get; set; }
        //public string TotalQatarExperience { get; set; }
        //public string TotalGCCExperience { get; set; }
        //public string TotalOtherExperience { get; set; }
        //public string Designation { get; set; }
        //public string NoticePeriod { get; set; }
        //public string Signature { get; set; }
        //public DateTime? SignatureDate { get; set; }
        public string CandidateEducationalData { get; set; }
        public string CandidateCertificationsData { get; set; }
        public string CandidateTrainingsData { get; set; }
        public string CandidateExperienceData { get; set; }
        public string NationalityName { get; set; }
        public string GenderName { get; set; }
        public string MaritalStatusName { get; set; }
        public string PassportIssueCountryName { get; set; }
        public string VisaCountryName { get; set; }
        public string VisaTypeName { get; set; }
        public string OtherCountryVisaName { get; set; }
        public string OtherCountryVisaTypeName{get;set;}
        public string CurrentAddressCountryName {get;set;}
        public string PermanentAddressCountryName { get; set; }
        public string PassportAttachmentName { get; set; }
        public string AcademicCertificateName { get; set; }
        public string OtherCertificateName { get; set; }
        public string ResumeAttachmentName { get; set; }
        public string CoverLetterAttachmentName { get; set; }
        public string SalaryCurrencyName { get; set; }

        public string JobAdvertisementId { get; set; }
        public IList<ApplicationJobCriteriaViewModel> Criterias { get; set; }
        public string CriteriasList { get; set; }
        public IList<ApplicationJobCriteriaViewModel> Skills { get; set; }
        public string SkillsList { get; set; }
        public IList<ApplicationJobCriteriaViewModel> OtherInformations { get; set; }
        public string OtherInformationsList { get; set; }
        public string JobAdvertisement { get; set; }
        public string CurrentTabInfo { get; set; }
        public string ApplicationNo { get; set; }
        public string BatchName { get; set; }
        public string TitleName { get; set; }
        public string WorkerBatch { get; set; }
        public string JobName { get; set; }
        public string JobCategoryName { get; set; }
        public string JobLocationName { get; set; }
        public string TaskId { get; set; }


        public IList<CandidateExperienceViewModel> ExperienceDoc { get; set; }
        public IList<ApplicationExperienceViewModel> AppExperienceDoc { get; set; }
    }
}
