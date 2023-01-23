
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
  
    public partial class PAY_PayrollTransaction : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double EarningAmount { get; set; }
        public double DeductionAmount { get; set; }
        public double Amount { get; set; }
        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }
        public DateTime PostedDate { get; set; }
        public PayrollPostedSourceEnum PostedSource { get; set; }

        public PayrollProcessStatusEnum ProcessStatus { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public long PostedUserId { get; set; }

        public ElementTypeEnum ElementType { get; set; }
        public ElementCategoryEnum ElementCategory { get; set; }
        public ElementClassificationEnum ElementClassification { get; set; }


        public NodeEnum? ReferenceNode { get; set; }
        public long? ReferenceId { get; set; }

        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }


        public double? Rate { get; set; }
        public PayrollUomEnum? Uom { get; set; }

        //public TimeSpan? OverTime { get; set; }
        //public TimeSpan? DeductionTime { get; set; }

        public double? Quantity { get; set; }
        public double? EarningQuantity { get; set; }
        public double? DeductionQuantity { get; set; }
        public double? OpeningQuantity { get; set; }
        public double? ClosingQuantity { get; set; }
        public bool? IsTransactionClosed { get; set; }

    }
    public class R_PayrollTransaction_PersonRoot : RelationshipBase
    {

    }
    //public class R_PayrollTransaction_PostedBy_User : RelationshipBase
    //{

    //}
    public class R_PayrollTransaction_ElementRoot : RelationshipBase
    {

    }
    //public class R_PayrollTransaction_PayrollRun : RelationshipBase
    //{

    //}
}
