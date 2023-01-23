using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.DataModel
{
    [Table("Application", Schema = "rec")]
    public class Application : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        [ForeignKey("JobAdvertisement")]
        public string JobAdvertisementId { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }

        [ForeignKey("Organization")]
        public string OrganizationId { get; set; }

        [ForeignKey("ListOfValue")]
        public string TitleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long? Age { get; set; }
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
        //public Country PassportIssueCountry { get; set; }
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
        public string OtherAttachmentId { get; set; }
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
        public DateTime? VisaExpiry { get; set; }
        public string OtherVisaType { get; set; }

        [ForeignKey("Country")]
        public string OtherCountryVisa { get; set; }
        [ForeignKey("ListOfValue")]
        public string OtherCountryVisaType { get; set; }
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
        public DateTime? SignatureDate { get; set; }
        public string ApplicationNo { get; set; }
        public DateTime? AppliedDate { get; set; }
        [ForeignKey("Job")]
        public string JobId { get; set; }
        [ForeignKey("ApplicationState")]
        public string ApplicationState { get; set; }
        [ForeignKey("ApplicationStatus")]
        public string ApplicationStatus { get; set; }
        public double? Score { get; set; }
        public string BatchId { get; set; }
        public string WorkerBatchId { get; set; }
        //SourceTypeEnum
        public string SourceFrom { get; set; }
        public InterviewFeedbackEnum? InterviewSelectionFeedback { get; set; }
        public string SalaryOnAppointment { get; set; }
       
        public string AgencyId { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string InterviewByUserId { get; set; }
        [ForeignKey("ListOfValue")]
        public string AccommodationId { get; set; }
        public string OfferDesigination { get; set; }
        public string OfferGrade { get; set; }
        public string GaecNo { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string OfferSignedBy { get; set; }

        //[ForeignKey("Position")]
        //public string PositionId { get; set; }
        [ForeignKey("ListOfValue")]
        public string DivisionId { get; set; }
        public string Sourcing { get; set; }       
        public string ReportingToId { get; set; }
        public DateTime? DateOfArrival { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinRelationship { get; set; }
        public string NextOfKinEmail { get; set; }
        public string NextOfKinPhoneNo { get; set; }

        public string OtherNextOfKin { get; set; }
        public string OtherNextOfKinRelationship { get; set; }
        public string OtherNextOfKinEmail { get; set; }
        public string OtherNextOfKinPhoneNo { get; set; }

        public string WitnessName1 { get; set; }
        public string WitnessDesignation1 { get; set; }
        public DateTime? WitnessDate1 { get; set; }
        public string WitnessGAEC1 { get; set; }

        public string WitnessName2 { get; set; }
        public string WitnessDesignation2 { get; set; }
        public DateTime? WitnessDate2 { get; set; }
        public string WitnessGAEC2 { get; set; }
        [ForeignKey("ListOfValue")]
        public string PassportStatusId { get; set; }
        public string Remarks { get; set; }

        public string HiringManagerRemarks { get; set; }
        public string AppointmentRemarks { get; set; }

        public bool? SalaryRevision { get; set; }
        public double? SalaryRevisionAmount { get; set; }
        public string FinalOfferReference { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? JoiningNotLaterThan { get; set; }
        public bool? IsTrainee { get; set; }
        public string TravelOriginAndDestination { get; set; }

        public string VehicleTransport { get; set; }
        public long? AnnualLeave { get; set; }
        public long? ServiceCompletion { get; set; }
        public string JobNo { get; set; }
        public bool? IsLocalCandidate { get; set; }
        [ForeignKey("ListOfValue")]
        public string VisaCategory { get; set; }

        public string RequirementQualification { get; set; }
        public string ActualQualification { get; set; }
        public string RequirementTechnical { get; set; }
        public string ActualTechnical { get; set; }
        public string RequirementExperience { get; set; }
        public string ActualExperience { get; set; }
        public string RequirementSpecialization { get; set; }
        public string ActualSpecialization { get; set; }
        public string RequirementITSkills { get; set; }
        public string ActualITSkills { get; set; }
        public string NatureOfWork { get; set; }
        public string TrainingsUndergone { get; set; }
        public string CountriesWorked { get; set; }
        public string OrganizationWorked { get; set; }
        public string FieldOfExposure { get; set; }
        public string CertificateCourse { get; set; }
        public string PositionsWorked { get; set; }
        public string DrivingLicense { get; set; }
        public string ExtraCurricular { get; set; }
        public string AnyOtherLanguage { get; set; }
        [ForeignKey("ListOfValue")]
        public string SelectedThroughId { get; set; }
        public string InterviewVenue { get; set; }
        public string NewPostJustification { get; set; }
        public string DescribeHowHeSuits { get; set; }
        public string LeaveCycle { get; set; }
        public string OtherBenefits { get; set; }

        public bool? HRHeadApproval { get; set; }
        public string HRHeadComment { get; set; }

        public bool? HodApproval { get; set; }
        public string HodComment { get; set; }

        public bool? PlanningApproval { get; set; }
        public string PlanningComment { get; set; }

        public bool? EDApproval { get; set; }
        public string EDComment { get; set; }
        public string CandidateId { get; set; }
    }
}
