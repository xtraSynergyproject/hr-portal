using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SalaryElementInfoViewModel : DatedViewModelBase
    {


        public long SalaryInfoId { get; set; }
        [Required]
        [Display(Name = "Element")]
        public long ElementId { get; set; }
        [Display(Name = "Element")]
        public string ElementName { get; set; }
        public string ElementCode { get; set; }
        [Required]
        public double Amount { get; set; }

        public long? ParentId { get; set; }
        public long? PersonId { get; set; }



        [Display(Name = "Salary Element Type")]
        public ElementTypeEnum? ElementType { get; set; }


        [Display(Name = "Salary Element Category")]
        public ElementCategoryEnum? ElementCategory { get; set; }


        [Display(Name = "Salary Element Classification")]
        public ElementClassificationEnum? ElementClassification { get; set; }

        public virtual DateTime? ElementEffectiveStartDate { get; set; }
        public virtual DateTime? ElementEffectiveEndDate { get; set; }


    }
}
