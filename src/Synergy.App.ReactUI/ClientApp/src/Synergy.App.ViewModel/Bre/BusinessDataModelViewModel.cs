using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.App.ViewModel
{
    public class BusinessDataModelViewModel:BusinessData
    {
        public LogicalEnum Condition { get; set; }
        public string Field { get; set; }
        public string Label { get; set; }
        public EqualityOperationEnum Operator { get; set; }
        public object Value { get; set; }

        public int RuleId { get; set; }

        public int? ParentRuleId { get; set; }
    }
}
