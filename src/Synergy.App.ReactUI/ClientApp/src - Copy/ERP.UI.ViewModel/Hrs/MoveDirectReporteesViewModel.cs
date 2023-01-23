using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class MoveDirectReporteesViewModel : BaseViewModel
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

        [Display(Name = "Direct Reportees")]
        public string DirectReportees { get; set; }

        [Display(Name = "Supervisor")]
        public int? SupervisorId { get; set; }

        [Display(Name = "Supervisor")]
        public string SupervisorName { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }


        [Display(Name = "Employee No")]
        public string EmployeeNo { get; set; }



        [Display(Name = "Proposed Supervisor")]
        public int? ProposedSupervisorId { get; set; }

        [Display(Name = "Proposed Supervisor")]
        public string ProposedSupervisorName { get; set; }

        [Required]
        [Display(Name = "Proposed Parent Position Name")]
        public int? ProposedParentPositionId { get; set; }

        [Display(Name = "Proposed Parent Position Name")]
        public string ProposedParentPositionName { get; set; }


        public string Remarks { get; set; }

        public int Level { get; set; }
        public TransactionMode Mode { get; set; }
        public bool EnableFromDate { get; set; }
        public bool IsSuccess { get; set; }
        public GridSelectOption HistoryGridSelectOption { get; set; }

    }
}
