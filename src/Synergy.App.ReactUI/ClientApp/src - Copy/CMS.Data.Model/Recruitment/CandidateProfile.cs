using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Model
{
    [Table("CandidateProfile", Schema = "rec")]
    public class CandidateProfile : DataModelBase
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("ListOfValue")]
        public string TitleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long? Age { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        [ForeignKey("Nationality")]
        public string NationalityId { get; set; }
        //public Nationality Nationality { get; set; }
        public string BloodGroup { get; set; }
        [ForeignKey("ListOfValue")]
        public string Gender { get; set; }
        [ForeignKey("ListOfValue")]
        public string MaritalStatus { get; set; }
        public int? NoOfChildren { get; set; }
        public string PassportNumber { get; set; }
        [ForeignKey("Country")]
        public string PassportIssueCountryId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? PassportExpiryDate { get; set; }
        public string QatarId { get; set; }
        public bool IsCopyofQID { get; set; }
        public string QIDAttachmentId { get; set; }
        public bool IsCopyofIDPassport { get; set; }
        public string PassportAttachmentId { get; set; }
        public bool IsCopyofAcademicCertificates { get; set; }
        public string AcademicCertificateId { get; set; }
        public bool IsCopyofOtherCertificates { get; set; }
        public string OtherCertificateId { get; set; }
        public bool IsMostRecentColorPhoto { get; set; }
        public string PhotoId { get; set; }
        public bool IsMostRecentCV { get; set; }
        public string ResumeId { get; set; }
        public bool IsLatestOfferLetterSalarySlip { get; set; }
        public string CoverLetterId { get; set; }
        public string CurrentAddressHouse { get; set; }
        public string CurrentAddressStreet { get; set; }
        public string CurrentAddressCity { get; set; }
        public string CurrentAddressState { get; set; }
        public string CurrentAddress { get; set; }
        [ForeignKey("Country")]
        public string CurrentAddressCountryId { get; set; }
        //public Country CurrentAddressCountry { get; set; }
        public string PermanentAddressHouse { get; set; }
        public string PermanentAddressStreet { get; set; }
        public string PermanentAddressCity { get; set; }
        public string PermanentAddressState { get; set; }
        public string PermanentAddress { get; set; }
        [ForeignKey("Country")]
        public string PermanentAddressCountryId { get; set; }
        //public Country PermanentAddressCountry { get; set; }
        public string Email { get; set; }
        public string ContactPhoneHome { get; set; }
        public string ContactPhoneLocal { get; set; }
        public string OptionForAnotherPosition { get; set; }
        public string AdditionalInformation { get; set; }
        public int? TimeRequiredToJoin { get; set; }
        public string ManagerJobTitleAndNoOfSubordinate { get; set; }

        public string HeardAboutUsFrom { get; set; }

        //public string OverseasSalary { get; set; }
        //public string IndianSalary { get; set; }
        public string NetSalary { get; set; }
        [ForeignKey("Currency")]
        public string NetSalaryCurrency { get; set; }
        public string ExpectedSalary { get; set; }
        public string ExpectedCurrency { get; set; }
        public string OtherAllowances { get; set; }

        [ForeignKey("Country")]
        public string VisaCountry { get; set; }
        [ForeignKey("ListOfValue")]
        public string VisaType { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? VisaExpiry { get; set; }
        public string OtherVisaType { get; set; }

        [ForeignKey("Country")]
        public string OtherCountryVisa { get; set; }
        [ForeignKey("ListOfValue")]
        public string OtherCountryVisaType { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? OtherCountryVisaExpiry { get; set; }

        public string QatarNocAvailable { get; set; }
        public string QatarNocNotAvailableReason { get; set; }

        public double? TotalWorkExperience { get; set; }
        //public string TotalQatarExperience { get; set; }
        //public string TotalGCCExperience { get; set; }
        //public string TotalOtherExperience { get; set; }
        public string Designation { get; set; }
        public string OtherDesignation { get; set; }
        public string NoticePeriod { get; set; }
        public string Signature { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? SignatureDate { get; set; }
        //SourceTypeEnum
        public string SourceFrom { get; set; }
       // [ForeignKey("AgencyUser")]
        public string AgencyId { get; set; }
        // public User AgencyUser { get; set; }
        [ForeignKey("ListOfValue")]
        public string PassportStatusId { get; set; }
        public string Remarks { get; set; }
        public int? Level { get; set; }
        public string[] BookMarks { get; set; }
    }
}
