using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.UI.ViewModel
{
    public class PositionSplitViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hierarchy Name")]
        public int? HierarchyNameId { get; set; }

        [Display(Name = "Hierarchy Name")]
        public string HierarchyName { get; set; }

        [Display(Name = "Position")]
        public int? PositionId { get; set; }

        public int? PositionHierarchyId { get; set; }

        [Display(Name = "Direct Reportees")]
        public string DirectReportees { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Display(Name = "Parent Position Name")]
        public int? ParentPositionId { get; set; }

        [Display(Name = "Split Position ")]
        public string SplitPositionName { get; set; }

        [Display(Name = "Split Position ")]
        public int? SplitPositionId { get; set; }

        [Display(Name = "  Parent Position Name")]
        public string ParentPositionName { get; set; }

        [Required]
        [Display(Name = "Split1 Position Name")]
        public int? Split1PositionId { get; set; }

        [Display(Name = "Split1 Position Name")]
        public string Split1PositionName { get; set; }

        [Required]
        [Display(Name = "Split2 Position Name")]
        public int? Split2PositionId { get; set; }

        [Display(Name = "Split2 Position Name")]
        public string Split2PositionName { get; set; }


        public string Remarks { get; set; }
        public DateTime AsOnDate { get; set; }

        public int Level { get; set; }
        public TransactionMode Mode { get; set; }
        public bool EnableFromDate { get; set; }
        public bool IsSuccess { get; set; }
        public GridSelectOption HistoryGridSelectOption { get; set; }
        public bool PositionRemoveReasonCode { get; set; }
        [Display(Name = "Position Name")]
        public virtual string PositionNameWithTitle { get; set; }
    
        [Display(Name = "Employee Name")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Parent Employee Name")]
        public string ParentEmployeeFullName { get; set; }

        [Display(Name = "Parent Employee Name")]
        public int ParentEmployeeId { get; set; }

        [Display(Name = "Propsed Parent Employee Name")]
        public string ProposedParentEmployeeFullName { get; set; }

        [Display(Name = "Propsed Parent Employee Name")]
        public int ProposedParentEmployeeId { get; set; }

        [Display(Name = "Position Effective Start Date")]
        public override DateTime? EffectiveFromDate
        {
            get
            {
                return base.EffectiveFromDate;
            }

            set
            {
                base.EffectiveFromDate = value;
            }
        }
        [Display(Name = "Position Effective To Date")]
        public override DateTime? EffectiveToDate
        {
            get
            {
                return base.EffectiveToDate;
            }

            set
            {
                base.EffectiveToDate = value;
            }
        }

    }
}
