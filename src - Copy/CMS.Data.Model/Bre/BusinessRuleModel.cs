using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class BusinessRuleModel : DataModelBase
    {

        public LogicalEnum? Condition { get; set; }
        public BusinessRuleTypeEnum BusinessRuleType { get; set; }
        public BusinessRuleSourceEnum BusinessRuleSource { get; set; }

        public BreMetadataTypeEnum? FieldSourceType { get; set; }
        public string FieldId { get; set; }
        public string Field { get; set; }
        public string Label { get; set; }
        [UIHint("OperatorType")]
        public EqualityOperationEnum? OperatorType { get; set; }
        public BreMetadataTypeEnum? ValueSourceType { get; set; }
        public string ValueId { get; set; }
        public string Value { get; set; }
        public string ValueField { get; set; }
        public string ParentId { get; set; }
        public string BreMasterTableMetadataId { get; set; }
        public string DecisionScriptComponentId { get; set; }
        public string BusinessRuleNodeId { get; set; }
        public string OperationValue { get; set; }
        public string OperationBackendValue { get; set; }
    }
}
