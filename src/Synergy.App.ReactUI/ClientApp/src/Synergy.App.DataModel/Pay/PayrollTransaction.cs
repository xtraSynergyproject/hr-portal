using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class PayrollTransaction : DataModelBase
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
    [Table("PayrollTransactionLog", Schema = "log")]
    public class PayrollTransactionLog : PayrollTransaction
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; } 
        public bool IsVersionLatest { get; set; }
    }
}
