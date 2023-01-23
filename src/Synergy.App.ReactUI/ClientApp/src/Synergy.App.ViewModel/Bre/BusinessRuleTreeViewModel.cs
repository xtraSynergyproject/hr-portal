using Synergy.App.Common;
using System;
using Synergy.App.DataModel;
namespace Synergy.App.ViewModel /*BRE.Data.Model*/
{
    public class BusinessRuleTreeViewModel : DataModelBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public bool Expanded { get; set; }
        public bool HasSubFolders { get; set; }
        public BusinessRuleTreeNodeTypeEnum BusinessRuleTreeNodeType { get; set; }


        public string IconCss
        {
            get
            {
                switch (BusinessRuleTreeNodeType)
                {
                    case BusinessRuleTreeNodeTypeEnum.Root:
                        return "fas fa-briefcase";
                    case BusinessRuleTreeNodeTypeEnum.BusinessArea:
                        return "fas fa-chart-area";
                    case BusinessRuleTreeNodeTypeEnum.BusinessSection:
                        return "fas fa-album-collection";
                    case BusinessRuleTreeNodeTypeEnum.BusinessRuleGroup:
                        return "fas fa-ball-pile";
                    case BusinessRuleTreeNodeTypeEnum.BusinessRule:
                        return "fas fa-pencil-ruler";
                    default:
                        return "fas fa-question";
                }
            }
        }
    }
}
