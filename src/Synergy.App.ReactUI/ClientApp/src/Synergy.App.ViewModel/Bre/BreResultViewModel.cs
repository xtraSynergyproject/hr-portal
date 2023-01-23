using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class BreResultViewModel : BreResult
    {
        public Dictionary<string, string> DetailList { get; set; }
        public TemplateTypeEnum TemplateType { get; set; }
        public string TemplateTypeText { get; set; }
        public string NodeParentId { get; set; }
        public string BusinessRuleId { get; set; }
        public string TemplateId { get; set; }
        public string ProcessName { get; set; }
        public TrueOrFalseEnum MethodReturn { get; set; }
        public BusinessLogicExecutionTypeEnum? NodeType { get; set; }
    }
}
