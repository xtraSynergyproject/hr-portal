using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class RecApplicationViewModel :NoteTemplateViewModel
    {        
        public string CandidateProfileId { get; set; }       
        public string JobAdvertisementId { get; set; } 
        public string OrganizationId { get; set; }      
        public string TitleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long? Age { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }       
        public string NationalityId { get; set; }
        //public Nationality Nationality { get; set; }
        public string BloodGroup { get; set; }       
        public string Gender { get; set; }      
        public string GenderId { get; set; }      
        public string MaritalStatus { get; set; }
        public int? NoOfChildren { get; set; }
        public string PassportNumber { get; set; }      
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
        public string CurrentAddressCountryId { get; set; }
        //public Country CurrentAddressCountry { get; set; }
        public string PermanentAddressHouse { get; set; }
        public string PermanentAddressStreet { get; set; }
        public string PermanentAddressCity { get; set; }
        public string PermanentAddressState { get; set; }
        public string PermanentAddress { get; set; }      
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
        public string NetSalaryCurrency { get; set; }
        public string ExpectedSalary { get; set; }
        public string ExpectedCurrency { get; set; }
        public string OtherAllowances { get; set; }       
        public string VisaCountry { get; set; }      
        public string VisaType { get; set; }
        public DateTime? VisaExpiry { get; set; }
        public string OtherVisaType { get; set; }       
        public string OtherCountryVisa { get; set; }      
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
        public string JobId { get; set; }       
        public string ApplicationState { get; set; }      
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
      
        public string AccommodationId { get; set; }
        public string OfferDesigination { get; set; }
        public string OfferGrade { get; set; }
        public string GaecNo { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string OfferSignedBy { get; set; }      
       
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
       
        public string PassportStatusId { get; set; }
        public string Remarks { get; set; }
        public string HiringManagerRemarks { get; set; }
        public string AppointmentRemarks { get; set; }
        public bool SalaryRevision { get; set; }
        public double? SalaryRevisionAmount { get; set; }
        public string FinalOfferReference { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? JoiningNotLaterThan { get; set; }
        public bool IsTrainee { get; set; }
        public string TravelOriginAndDestination { get; set; }
        public string VehicleTransport { get; set; }
        public long? AnnualLeave { get; set; }
        public long? ServiceCompletion { get; set; }
        public string JobNo { get; set; }
        public bool IsLocalCandidate { get; set; }      
        public string VisaCategoryId { get; set; }
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
        public string SignatureId { get; set; }
        public string Nationality { get; set; }
        public string PassportIssueCountry { get; set; }      
        public string CurrentAddressCountry { get; set; }        
        public string PermanentAddressCountry { get; set; }      
        public string ApplicationId { get; set; }
        public string OrganizationName { get; set; }
        public long StateCount { get; set; }
        public string[] UserRoleCodes { get; set; }
        public string CurrentAddressCountryName { get; set; }
        public string PermanentAddressCountryName { get; set; }
        public string CurrentEmployer { get; set; }
        public string CurrentEmploymentLocation { get; set; }
        public string CurrentEmploymentPosition { get; set; }
        public DateTime? CurrentEmploymentStartDate { get; set; }
        public DateTime? CurrentEmploymentEndDate { get; set; }
        public string CurrentEmploymentResponsibilities { get; set; }
        public string ExperienceByCountryName { get; set; }
        public double? YearsOfCountryExperience { get; set; }
        public string ExperienceByJobName { get; set; }
        public double? YearsOfJobExperience { get; set; }
        public double? WorkExperience { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Category { get; set; }
        public string Languages { get; set; }
        public string ProficiencyLevel { get; set; }
        public double? ExperienceBySectorOrCategory { get; set; }
        public double? YearsOfExperienceBySectorOrCategory { get; set; }
        public string CurrentSalary { get; set; }     
        public double? SalaryAfterIncomeTax { get; set; }
        public double? OtherAllowance { get; set; }
        public string ComputerProficiencyProgram { get; set; }
        public string ComputerProficiencyLevel { get; set; }
        public string EductaionQualification { get; set; }
        public string EductaionSpecialization { get; set; }
        public string EductaionType { get; set; }
        public string EductaionInstiute { get; set; }
        public string EducationCountry { get; set; }
        public string EductaionDuartion { get; set; }
        public string EductaionPassingYears { get; set; }
        public string EductaionMarks { get; set; }
        public string CertificationQualification { get; set; }
        public string CertificationSpecialization { get; set; }
        public string CertificationType { get; set; }
        public string CertificationInstiute { get; set; }
        public string CertificationCountry { get; set; }
        public string CertificationDuartion { get; set; }
        public string CertificationPassingYears { get; set; }
        public string CertificationMarks { get; set; }
        public string TrainingQualification { get; set; }
        public string TrainingSpecialization { get; set; }
        public string TrainingType { get; set; }
        public string TrainingInstiute { get; set; }
        public string TrainingCountry { get; set; }
        public string TrainingDuartion { get; set; }
        public string TrainingPassingYears { get; set; }
        public string TrainingMarks { get; set; }
        public string DLCountry { get; set; }
        public string DLType { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public DateTime? DLValidUpto { get; set; }
        public string ReferenceName { get; set; }
        public string ReferencePosition { get; set; }
        public string ReferenceCompany { get; set; }
        public string ReferencePhone { get; set; }
        public string ReferenceEmail { get; set; }
        public string CandidateType { get; set; }
        public string ApplicationStateCode { get; set; }
        public string ApplicationStatusCode { get; set; }
        public string ApplicationStateName { get; set; }
        public string ApplicationStatusName { get; set; }
        public string JobTitle { get; set; }
        public string JobCategoryName { get; set; }
        public string LocationName { get; set; }
        public string NationalityName { get; set; }
        public string GenderName { get; set; }
        public string MaritalStatusName { get; set; }
        public string PassportIssueCountryName { get; set; }
        public string VisaCountryName { get; set; }
        public string VisaTypeName { get; set; }
        public string OtherCountryVisaName { get; set; }
        public string OtherCountryVisaTypeName { get; set; }
        public string BatchName { get; set; }
        public string BatchStatus { get; set; }
        public string BatchStatusName { get; set; }
        public string BatchStatusCode { get; set; }
        public double? TotalIndianExperience { get; set; }
        public string FullName { get; set; }
        public string GaecName { get; set; }
        public string PassportName { get; set; }
        public string designationName { get; set; }
        public double Accomodation { get; set; }
        public double Basic { get; set; }
        public double food { get; set; }
        public double SafetyAllowance { get; set; }

        public string SalaryCurrencyName { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? ContractDate { get; set; }
        public IList<RecCandidateEvaluationViewModel> EvaluationData { get; set; }
        public string MarksSelections { get; set; }
        public string EvaluationDataString { get; set; }
        public double? BasicPay { get; set; }
        public string BasicInWords { get; set; }
        public double? ProfessionalAllowance { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? CandJoiningDate { get; set; }
        public string PositionName { get; set; }
        public string DivisionName { get; set; }

        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? CurrentDate { get; set; }
        public string ManpowerTypeName { get; set; }
        public string ManpowerTypeCode { get; set; }
        
        public string ReportToName { get; set; }


        public string AccommodationName { get; set; }
        public string InterviewByUserName { get; set; }
        public double? TotalOtherExperience { get; set; }
        public double? TotalGCCExperience { get; set; }
        public string TaskId { get; set; }
        public string ServiceId { get; set; }
        public string TaskNo { get; set; }
        public string TaskSubject { get; set; }
        public string TaskTemplateCode { get; set; }
        public string TaskStatusCode { get; set; }
        public string NextTaskId { get; set; }
        public string NextTaskTemplateCode { get; set; }
        public string NextTaskStatusCode { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public int ContractYear { get; set; }
        public string GradeName { get; set; }
        public int GradeCode { get; set; }
        public string VisaTaskId { get; set; }
        public string VisaTaskTemplateCode { get; set; }
        public string VisaTaskStatusCode { get; set; }
        public string ApplicationUserId { get; set; }
        public string Source { get; set; }
        public string VisaCategoryCode { get; set; }
        public string SelectedThroughName { get; set; }
        public string JsonWorkerPool1 { get; set; }
        public double? AbroadExperiance { get; set; }
        public double? IndiaExperiance { get; set; }
        public long? level { get; set; }
        public bool? HRApprovl { get; set; }
        public bool? HodApprovl { get; set; }
        public bool? PlanningApprovl { get; set; }
        public string JobName { get; set; }
        public bool? EDApprovl { get; set; }
        //Step
        public string Step1 { get; set; }
        public string Step2 { get; set; }
        public string Step3 { get; set; }
        public string Step4 { get; set; }
        public string Step5 { get; set; }
        public string Step6 { get; set; }
        public string Step7 { get; set; }
        public string Step8 { get; set; }
        public string Step9 { get; set; }
        public string Step10 { get; set; }
        public string Step11 { get; set; }
        public string Step12 { get; set; }
        public string Step13 { get; set; }
        public string Step14 { get; set; }
        public string Step15 { get; set; }

        public string StepNo1 { get; set; }
        public string StepNo2 { get; set; }
        public string StepNo3 { get; set; }
        public string StepNo4 { get; set; }
        public string StepNo5 { get; set; }
        public string StepNo6 { get; set; }
        public string StepNo7 { get; set; }
        public string StepNo8 { get; set; }
        public string StepNo9 { get; set; }
        public string StepNo10 { get; set; }
        public string StepNo11 { get; set; }
        public string StepNo12 { get; set; }
        public string StepNo13 { get; set; }
        public string StepNo14 { get; set; }
        public string StepNo15 { get; set; }
        public string WorkerBatch { get; set; }
        public bool IsCandidateEvaluation { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? OfferCreatedDate { get; set; }
        public string TitleName { get; set; }
        public string HiringManagerId { get; set; }
        public string RecruiterId { get; set; }

        public string JoiningTime { get; set; }
        public string TaskStatus { get; set; }
        public string ApplicationIds { get; set; }
        public DateTime? ScheduleInterveiwDate { get; set; }
        public string ScheduleInterveiwComments { get; set; }

        public string AgencyName { get; set; }
        public string ApplicationNoteId { get; set; }
       
        public string HiringManagerName { get; set; }

        public string Mode { get; set; }
        public string EvaluationAttachmentId { get; set; }
        public string ApplicationStateId { get; set; }
        public string ApplicationStatusId { get; set; }
        public string TempCode { get; set; }
    }
}
