using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{   
    public class BusinessRuleGroup : DataModelBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}
