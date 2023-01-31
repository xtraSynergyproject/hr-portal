
using ERP.Utility;

namespace ERP.Data.GraphModel
{
    public partial class PAY_ElementRoot : RootNodeBase
    {

    }
  
    public partial class PAY_Element : NodeDatedBase
    {
        [NotMapped]
        public long ElementId { get; set; }
        public ElementValueTypeEnum ValueType { get; set; }
        public double? PercentageValue { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public long? SequenceNo { get; set; }


        public string CostingAccount { get; set; }
        public string CostingSubAccount { get; set; }

        public string BalancingAccount { get; set; }
        public string BalancingSubAccount { get; set; }

        public string RollupValueMethodName { get; set; }
        public string ValueMethodName { get; set; }

        public ElementTypeEnum ElementType { get; set; }
        public ElementEntryTypeEnum ElementEntryType { get; set; }

        public ElementCategoryEnum ElementCategory { get; set; }

        public ElementClassificationEnum ElementClassification { get; set; }
  

    }
    public class R_Element_Percentage_ElementRoot : RelationshipBase
    {

    }
    public class R_ElementRoot_Reverse_ElementRoot : RelationshipBase
    {

    }
    public class R_ElementRoot : RelationshipDatedBase
    {

    }
}
