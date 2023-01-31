using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class PayrollElementRunResultViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public double EarningAmount { get; set; }
        public double DeductionAmount { get; set; }
        public double Amount { get; set; }


        public DateTime? PayrollStartDate { get; set; }
        public DateTime? PayrollEndDate { get; set; }
        public int? YearMonth { get; set; }

        public string PayrollRunId { get; set; }
        public string PayrollId { get; set; }

        public ElementTypeEnum? ElementType { get; set; }
        public ElementCategoryEnum? ElementCategory { get; set; }
        public ElementClassificationEnum? ElementClassification { get; set; }

        public string PayrollRunResultId { get; set; }
        public string ElementId { get; set; }
        public string PayrollElementId { get; set; }
        public string ElementCode { get; set; }

        public string PersonId { get; set; }
        [Display(Name = "Execution Status")]
        public ExecutionStatusEnum? ExecutionStatus { get; set; }
        public string Error { get; set; }

        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }

        public double? Quantity { get; set; }
        public double? EarningQuantity { get; set; }
        public double? DeductionQuantity { get; set; }
        public double? OpeningQuantity { get; set; }
        public double? ClosingQuantity { get; set; }


    }
}
