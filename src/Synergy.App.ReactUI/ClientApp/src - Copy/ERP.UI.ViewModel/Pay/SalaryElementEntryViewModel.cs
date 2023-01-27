using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SalaryElementEntryViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double? EarningAmount { get; set; }
        public double? DeductionAmount { get; set; }
        public double? Amount { get; set; }


        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public int? YearMonth { get; set; }

        public long PayrollRunId { get; set; }
        public long? PayrollId { get; set; }

        public ElementTypeEnum ElementType { get; set; }
        public ElementCategoryEnum ElementCategory { get; set; }
        public ElementClassificationEnum ElementClassification { get; set; }
        public string ElementName { get; set; }


        public long SalaryEntryId { get; set; }
        public long ElementId { get; set; }
        public long PersonId { get; set; }
        [Display(Name ="Employee Name /Iqama No")]
        public string PersonName { get; set; }

        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public DocumentStatusEnum PublishStatus { get; set; }


    }
}
