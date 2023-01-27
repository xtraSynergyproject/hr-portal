using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Synergy.App.ViewModel
{
    public class BusinessRuleModelViewModel : BusinessRuleModel
    { 
        public string CollectionId { get; set; }
        public string RuleId { get; set; }
        public string TemplateId { get; set; }
        public string TableMetaId { get; set; }
        public DataColumnTypeEnum? FieldDataType { get; set; }
        public DataColumnTypeEnum? ValueDataType { get; set; }
        public string DecisionNodeId { get; set; }
        public string FieldSourceTypeStr { get; set; }
        public string ValueSourceTypeStr { get; set; }
        public int? ParentRuleId { get; set; }
        public bool IsParent { get; set; }
        public string ConditionStr {
            get { return Condition != null ? Condition.ToString() : ""; }
        }
       
        public string OperatorStr {
            get { return OperatorType != null ? OperatorType.ToString() : ""; }
        }
        public bool HasChildren { get; set; }
        public bool Expanded { get; set; }      
        public string ParentFieldId { get; set; }

        public string ParentValueId { get; set; }
        public string UIJson { get; set; }

    }
}
