using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class DecisionScriptComponent : DataModelBase
    {
        public string Script { get; set; }

        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }
        public BusinessRuleLogicTypeEnum? BusinessRuleLogicType { get; set; }
        public string OperationValue { get; set; }

    }
    [Table("DecisionScriptComponentLog", Schema = "log")]
    public class DecisionScriptComponentLog : DecisionScriptComponent
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
