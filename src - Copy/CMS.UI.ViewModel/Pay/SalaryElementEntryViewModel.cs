using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
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

        public string PayrollRunId { get; set; }
        public string PayrollId { get; set; }

        public ElementTypeEnum ElementType { get; set; }
        public ElementCategoryEnum ElementCategory { get; set; }
        public ElementClassificationEnum ElementClassification { get; set; }
        public string ElementName { get; set; }
        public string ElementDisplayName { get; set; }

        public string SalaryEntryId { get; set; }
        public string ElementId { get; set; }
        public string PersonId { get; set; }
        [Display(Name = "Employee Name /Iqama No")]
        public string PersonName { get; set; }

        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public DocumentStatusEnum PublishStatus { get; set; }


    }
}
