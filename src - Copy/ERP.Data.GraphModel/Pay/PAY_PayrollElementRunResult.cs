
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
  
    public partial class PAY_PayrollElementRunResult : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public int YearMonth { get; set; }

        public double EarningAmount { get; set; }
        public double DeductionAmount { get; set; }
        public double Amount { get; set; }

        public ElementTypeEnum? ElementType { get; set; }
        public ElementCategoryEnum? ElementCategory { get; set; }
        public ElementClassificationEnum? ElementClassification { get; set; }


        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }


        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }

        public double? Quantity { get; set; }
        public double? EarningQuantity { get; set; }
        public double? DeductionQuantity { get; set; }
        public double? OpeningQuantity { get; set; }
        public double? ClosingQuantity { get; set; }

    }
    public class R_PayrollElementRunResult_PayrollRunResult: RelationshipBase
    {

    }
    public class R_PayrollElementRunResult_ElementRoot : RelationshipBase
    {

    }
    
}
