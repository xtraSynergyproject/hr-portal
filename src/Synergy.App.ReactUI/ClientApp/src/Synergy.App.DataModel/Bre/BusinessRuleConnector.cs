using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{   
    public class BusinessRuleConnector : DataModelBase
    {
        public string Name { get; set; }
        public string BusinessRuleId { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }      
        public bool IsForTrue { get; set; }
        public bool IsFromDecision { get; set; }

    }
}
