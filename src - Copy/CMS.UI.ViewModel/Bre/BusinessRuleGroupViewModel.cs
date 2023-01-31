using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class BusinessRuleGroupViewModel : BusinessRuleGroup
    {
        public BusinessRuleTreeNodeTypeEnum BusinessRuleTreeNodeType { get; set; }
        public DataActionEnum DataAction { get; set; }
    }
}
