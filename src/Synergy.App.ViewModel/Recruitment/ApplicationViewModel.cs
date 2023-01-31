using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ApplicationViewModel : Application
    {
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
        public IList<CandidateEvaluationViewModel> EvaluationData { get; set; }
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
        public string CandidateId { get; set; }
        public string HiringManagerName { get; set; }
        public string NoteId { get; set; }
        public string Json { get; set; }


        //multi attachment

        public string QIDAttachmentId2 { get; set; }
        public string QIDAttachmentId3 { get; set; }
        public string QIDAttachmentId4 { get; set; }
        public string QIDAttachmentId5 { get; set; }
        public string PassportAttachmentId2 { get; set; }
        public string PassportAttachmentId3 { get; set; }
        public string PassportAttachmentId4 { get; set; }
        public string PassportAttachmentId5 { get; set; }
        public string AcademicCertificateId2 { get; set; }
        public string AcademicCertificateId3 { get; set; }
        public string AcademicCertificateId4 { get; set; }
        public string AcademicCertificateId5 { get; set; }
        public string OtherCertificateId2 { get; set; }
        public string OtherCertificateId3 { get; set; }
        public string OtherCertificateId4 { get; set; }
        public string OtherCertificateId5 { get; set; }
        public string ApplicationStateId { get; set; }
        public string ApplicationStatusId { get; set; }

        public string Mode { get; set; }
        public string EvaluationAttachmentId { get; set; }
        public string TempCode { get; set; }

        public string SalaryRevisionComment { get; set; }

        public string TransportationText;
        public bool IsEligibleAfterProbation { get; set; }
        public string VisaCategoryId { get; set; }

    }
}
