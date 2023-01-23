using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmploymentInfoViewModel : ViewModelBase
    {
        public long? AssignmentId { get; set; }
        public long? AssignmentRootId { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public long? PersonId { get; set; }
       

        [Display(Name = "Employee Name")]
        public string PersonFullName { get; set; }

        [Display(Name = "Iqamah No")]
        public string IqamahNo { get; set; }

        [Required]
        [Display(Name = "Assignment Type")]
        public string AssignmentTypeCode { get; set; }

        [Display(Name = "Assignment Type")]
        public string AssignmentTypeName { get; set; }

        [Required]
        [Display(Name = "Assignment Status")]
        public long? AssignmentStatusId { get; set; }

        [Display(Name = "Assignment Status")]
        public string AssignmentStatusName { get; set; }
        [Display(Name = "Primary Assignment")]
        public bool IsPrimaryAssignment { get; set; }
        [Required]
        [Display(Name = "Date Of Join")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }
        [Display(Name = "Date Of Join")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AssignmentMasterDateOfJoin { get; set; }

        [Display(Name = "Probation Period")]
        public ProbationPeriodEnum ProbationPeriod { get; set; }
        [Display(Name = "Probation End Date")]
        public DateTime? ProbationEndDate { get; set; }
        [Display(Name = "Notice Start Date")]
        public DateTime? NoticeStartDate { get; set; }
        [Display(Name = "Notice Period (in Days)")]
        public int? NoticePeriod { get; set; }
        [Display(Name = "Actual Termination Date")]
        public DateTime? ActualTerminationDate { get; set; }

        [Required]
        [Display(Name = "Location")]
        public long? LocationId { get; set; }

        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Required]
        [Display(Name = "Assignment Grade")]
        public long? GradeId { get; set; }
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "Job Grade")]
        public string JobGrade { get; set; }
     
        public long? UserId { get; set; }      

        [Required]
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

        [Required]
        [Display(Name = "Job")]
        public long? JobId { get; set; }

        [Display(Name = "Job")]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "Position")]
        public long? PositionId { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Display(Name = "Job Title")]
        public string PositionTitle { get; set; }

        [Display(Name = "Supervisor Name")]
        public long? SupervisorId { get; set; }
        [Display(Name = "Supervisor Name")]
        public string SupervisorName { get; set; }
        public string Remarks { get; set; }

        public bool PayAnnualTicketForSelf { get; set; }

        public bool PayAnnualTicketForDependent { get; set; }
        [Display(Name = "Ticket Destination")]
        public long? TicketDestinationId { get; set; }
        public string TicketDestination { get; set; }
        [Display(Name = "Class Of Travel")]
        public long? ClassOfTravelId { get; set; }
        public bool DisableControl { get; set; }

        [Display(Name = "Contract Type")]
        public ContractTypeEnum? ContractType { get; set; }
        [Display(Name = "Sponsor")]
        public long? SponsorId { get; set; }

        public long? ContractId { get; set; }
        public long? ContractRootId { get; set; }

        [Display(Name = "Contract End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEndDate { get; set; }

        [Display(Name = "Contract Renewable")]
        public BoolStatus? ContractRenewable { get; set; }

        [Display(Name = "Hierarchy Name")]
        public long? ParentHierarchyId { get; set; }

        [Display(Name = "Hierarchy Name")]
        public string HierarchyName { get; set; }
      
        [Display(Name = "Parent Position")]
        public long? ParentPositionId { get; set; }

        public long? ParentPositionRelationshipId { get; set; }

        [Display(Name = "Legal Entity")]
        public long? ParentPositionLegalEntityId { get; set; }

        public virtual bool IsLatest { get; set; }

        public long? RootId { get; set; }
    }
}
