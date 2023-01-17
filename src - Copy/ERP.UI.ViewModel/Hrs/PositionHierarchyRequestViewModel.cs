using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PositionHierarchyRequestViewModel : BaseRequestViewModel
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

        [Required]
        [Display(Name = "Parent Position Name")]
        public int? ParentPositionId { get; set; }

        [Display(Name = "Proposed Parent Position Name")]
        public string ParentPositionName { get; set; }


        public string Remarks { get; set; }

        public int Level { get; set; }
        public TransactionMode Mode { get; set; }
        public bool EnableFromDate { get; set; }
        public bool IsSuccess { get; set; }
        public GridSelectOption HistoryGridSelectOption { get; set; }

    }
}
