using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class StepTaskSkipLogic : DataModelBase
    {
        public string StepTaskComponentId { get; set; }
        public string Name { get; set; }
        public string ExecutionLogicDisplay { get; set; }
        public string ExecutionLogic { get; set; }
        public bool SuccessResult { get; set; }
    }
    [Table("StepTaskSkipLogicLog", Schema = "log")]
    public class StepTaskSkipLogicLog : StepTaskSkipLogic
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
