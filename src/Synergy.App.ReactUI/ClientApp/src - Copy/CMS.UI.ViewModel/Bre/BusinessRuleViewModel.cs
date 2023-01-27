using CMS.Common;
using CMS.Data.Model;
using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.UI.ViewModel
{
    public class BusinessRuleViewModel : BusinessRule
    {
        public bool Expanded { get; set; }
        public bool RuleExist { get; set; }
        public bool HasSubFolders { get; set; }
        public BusinessRuleTreeNodeTypeEnum BusinessRuleTreeNodeType { get; set; }
        public string ActionName { get; set; }
        public string LOVType { get; set; }
        public string DecisionScriptComponentId { get; set; }
        public string DecisionParentId { get; set; }
        public bool? IsWorkFlow { get; set; }
        public bool? IsBusinessDiagram { get; set; }

    }
}
