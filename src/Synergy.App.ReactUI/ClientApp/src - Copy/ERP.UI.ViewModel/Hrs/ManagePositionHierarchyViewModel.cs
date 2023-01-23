using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ManagePositionHierarchyViewModel : BaseViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Hierarchy Name")]
        public int? HierarchyNameId { get; set; }

        [Display(Name = "Hierarchy Name")]
        public string HierarchyName { get; set; }

        [Required]
        [Display(Name = "Position")]
        public int? PositionId { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Display(Name = "Parent Position")]
        public int? ParentPositionId { get; set; }

        [Display(Name = "Parent Position")]
        public string ParentPositionName { get; set; }

        [Display(Name = "Supervisor")]
        public int? SupervisorId { get; set; }

        [Display(Name = "Supervisor")]
        public string SupervisorName { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }


        [Display(Name = "Employee No")]
        public string EmployeeNo { get; set; }

        [Display(Name = "Group")]
        public string Group { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Division")]
        public string Division { get; set; }

        [Display(Name = "Section")]
        public string Section { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Display(Name = "Proposed Supervisor")]
        public int? ProposedSupervisorId { get; set; }

        [Display(Name = "Proposed Supervisor")]
        public string ProposedSupervisorName { get; set; }

        [Required]
        [Display(Name = "Proposed Parent Position")]
        public int? ProposedParentPositionId { get; set; }

        [Display(Name = "Proposed Parent Position")]
        public string ProposedParentPositionName { get; set; }


        public string Remarks { get; set; }

        public int Level { get; set; }
        public TransactionMode Mode { get; set; }
        public bool EnableFromDate { get; set; }
        public bool IsSuccess { get; set; }
        public GridSelectOption HistoryGridSelectOption { get; set; }
        [Display(Name = "Reason For Removal")]
        public bool PositionRemoveReasonCode { get; set; }

    }
}
