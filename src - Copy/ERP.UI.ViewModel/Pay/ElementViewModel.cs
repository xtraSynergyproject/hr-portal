using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ElementViewModel : DatedViewModelBase
    {
        public long ElementId { get; set; }
        [Required]
        [Display(Name = "Element Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Element Name")]
        public string Name { get; set; }

        [Display(Name = "Element Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Costing Account")]
        public string CostingAccount { get; set; }
        [Display(Name = "Costing Sub Account")]
        public string CostingSubAccount { get; set; }
        [Display(Name = "Balancing Account")]
        public string BalancingAccount { get; set; }
        [Display(Name = "Balancing Sub Account")]
        public string BalancingSubAccount { get; set; }

        public long? SequenceNo { get; set; }

        public string RollupValueMethodName { get; set; }
        public string ValueMethodName { get; set; }

        [Display(Name = "Value Type")]
        [Required]
        public ElementValueTypeEnum? ValueType { get; set; }
        [Display(Name = "Percentage Value")]
        public double? PercentageValue { get; set; }
        [Display(Name = "Percentage Element")]
        public long? PercentageElementId { get; set; }

        [Required]
        [Display(Name = "Element Type")]
        public ElementTypeEnum ElementType { get; set; }
        [Required]
        [Display(Name = "Element Entry Type")]
        public ElementEntryTypeEnum ElementEntryType { get; set; }
        [Required]
        [Display(Name = "Element Category")]
        public ElementCategoryEnum ElementCategory { get; set; }

        [Required]
        [Display(Name = "Element Classification")]
        public ElementClassificationEnum ElementClassification { get; set; }

        public string ElementVal { get; set; }
        public string Elements { get; set; }
        public long[] ElementData { get; set; }

        public long? ReverseElementId { get; set; }
        public string  ReverseElementCode { get; set; }
    }
}
