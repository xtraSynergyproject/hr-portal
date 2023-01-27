using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.App.ViewModel
{
    public class BusinessDataViewModel: BusinessData
    {
        public bool Expanded { get; set; }
        public bool HasSubFolders { get; set; }
        public BusinessDataTreeNodeTypeEnum BusinessRuleTreeNodeType { get; set; }
    }
}
