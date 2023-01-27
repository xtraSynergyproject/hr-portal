using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;


namespace CMS.UI.ViewModel
{
    public class AssignmentViewModel:ViewModelBase
    {
        public string AssignmentId { get; set; }
        public string ActiveTab { get; set; }
        // public long AssignmentMasterId { get; set; }        
        DataActionEnum? DataAction { get; set; }
        public string[] UserRoleCodes { get; set; }
        public string PersonId { get; set; }
        public string EmployeeId { get; set; }
        public string NoteId { get; set; }
        public string NoteContractId { get; set; }
        public string NoteAssignmentId { get; set; }
        public string NotePositionHierarchyId { get; set; }
        public string NoteSalaryInfoId { get; set; }
        public string NoteLocationId { get; set; }
        public string NoteGradeId { get; set; }
        public string NotePositionId { get; set; }
        public string NoteDepartmentId { get; set; }
        public string NoteJobId { get; set; }
        public string ContractId { get; set; }
        public string PositionHierarchyId { get; set; }
        public string HierarchyId { get; set; }
        public string SalaryInfoId { get; set; }
        public string EmployeeContractNoteId { get; set; }

        public double? ClosingBalance { get; set; }

        public string PersonFullName { get; set; }

 
        public string IqamahNo { get; set; }
        public string PersonStatus { get; set; }


        public string AssignmentTypeCode { get; set; }


        public string AssignmentTypeName { get; set; }


        public string AssignmentStatusId { get; set; }


        public string AssignmentStatusName { get; set; }

        public bool IsPrimaryAssignment { get; set; }
  
        public string DateOfJoin { get; set; }
        public DateTime DTDateOfJoin { get; set; }

        public DateTime? AssignmentMasterDateOfJoin { get; set; }

        public ProbationPeriodEnum? ProbationPeriod { get; set; }

        public DateTime? ProbationEndDate { get; set; }

        public DateTime? NoticeStartDate { get; set; }
        public int? NoticePeriod { get; set; }

        public DateTime? ActualTerminationDate { get; set; }


        public string LocationId { get; set; }


        public string LocationName { get; set; }


        public string GradeId { get; set; }

        public string GradeName { get; set; }

        public string JobGrade { get; set; }
        //[Required]
        //[Display(Name = "Biometric Id")]
        //public string BiometricId { get; set; }
        public string PhotoId { get; set; }
        public string UserId { get; set; }
        public string PhotoVersion { get; set; }
        public string PhotoName { get; set; }
        public string Page { get; set; }


        public string DepartmentId { get; set; }


        public string DepartmentName { get; set; }


        public string JobId { get; set; }


        public string JobName { get; set; }


        public string PositionId { get; set; }


        public string PositionName { get; set; }


        public string PositionTitle { get; set; }


        public long? SupervisorId { get; set; }

        public string SupervisorName { get; set; }
        public string Remarks { get; set; }

        public bool PayAnnualTicketForSelf { get; set; }

        public bool PayAnnualTicketForDependent { get; set; }

        public string TicketDestinationId { get; set; }
        public string TicketDestination { get; set; }

        public string ClassOfTravelId { get; set; }
        public bool DisableControl { get; set; }


        public string SectionId { get; set; }



        public string SectionName { get; set; }

        public string LegalEntityCode { get; set; }
      

        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string WorkPhoneExtension { get; set; }
        public string JDNoteId { get; set; }
        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }
        public string SponsorName { get; set; }
        public bool? EnableRemoteSignIn { get; set; }

        public string UserStatus { get; set; }

        //Changes Status

        public string ChangedData { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangeStatus { get; set; }
        public string BasicSalary { get; set; }

        public string AnnualLeaveEntitlement { get; set; }
        public string PerformanceDocumentId { get; set; }
        public string PerformanceDocumentName { get; set; }
        public DateTime? PDStartDate { get; set; }
        public DateTime? PDEndDate { get; set; }
        public string PDStatus { get; set; }
        public string PDYear { get; set; }
        public string PDFinalRatingRounded { get; set; }
        public string PDBonus { get; set; }
        public string PDIncrement { get; set; }
        public ServiceViewModel PDStage { get; set; }
        public IList<ServiceViewModel> Goals { get; set; }
        public IList<ServiceViewModel> Competency { get; set; }
        public string ManagerJobName { get; set; }
        public string ManagerPersonFullName { get; set; }
        public string ManagerUserId { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
       
        public string BadgeAwardDate { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentScore { get; set; }
        public string AssessmentStartTime { get; set; }
        public IList<AssignmentViewModel> Criterias { get; set; }
        public string CriteriasList { get; set; }
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeImage { get; set; }
        public string CertificationName { get; set; }
        public string CertificateReferenceNo { get; set; }
        public string ExpiryLicenseDate { get; set; }
        public string ResultScore { get; set; }
        public string CompetencyName { get; set; }
        public string CurrentDateText { get; set; }
        public string SponsorLogoId { get; set; }
        public string SponsorLogo { get; set; }
        public string FinalScore { get; set; }
        public string FinalComments { get; set; }
        public string AssignmentGradeId { get; set; }
        public string AssignmentTypeId { get; set; }
        public string OrgLevel1Id { get; set; }
        public string OrgLevel2Id { get; set; }
        public string OrgLevel3Id { get; set; }
        public string OrgLevel4Id { get; set; }
        public string BrandId { get; set; }
        public string MarketId { get; set; }
        public string ProvinceId { get; set; }
        public string CareerLevelId { get; set; }


    }
}
