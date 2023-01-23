using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class HrDirectViewModel : DatedViewModelBase
    {
        public long? AssignmentId { get; set; }
        // public long AssignmentMasterId { get; set; }        

        [Required]
        [Display(Name = "Employee Name")]
        public long? PersonId { get; set; }

        public long? ContractId { get; set; }
        public long? SalaryInfoId { get; set; }
        public long? EmployeeContractNoteId { get; set; }
        

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
        [Display(Name = "Actual Termination Date")]
        public DateTime? ActualTerminationDate { get; set; }

        [Required]
        [Display(Name = "Location")]
        public long? LocationId { get; set; }

        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Required]
        [Display(Name = "Assignmrnt Grade")]
        public long? GradeId { get; set; }
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "Job Grade")]
        public string JobGrade { get; set; }
        //[Required]
        //[Display(Name = "Biometric Id")]
        //public string BiometricId { get; set; }
        public long? PhotoId { get; set; }
        public long? UserId { get; set; }
        public long? PhotoVersion { get; set; }
        public string PhotoName { get; set; }
        public string Page { get; set; }

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


        [Display(Name = "Section Name")]
        public long? SectionId { get; set; }


        [Display(Name = "Section Name")]
        public string SectionName { get; set; }

        public string LegalEntityCode { get; set; }
        public BoolStatus IsPositionActive { get; set; }
        public BoolStatus IsPersonActive { get; set; }
        public BoolStatus IsContractActive { get; set; }
        public BoolStatus IsAssignmentActive { get; set; }
        public BoolStatus IsPayrollActive { get; set; }
        public BoolStatus IsUserActive { get; set; }

        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string WorkPhoneExtension { get; set; }
        public long JDNoteId { get; set; }
        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }
    }
}
