using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.UI.ViewModel
{
    public class BusinessDataViewModel: BusinessData
    {
        public bool Expanded { get; set; }
        public bool HasSubFolders { get; set; }
        public BusinessDataTreeNodeTypeEnum BusinessRuleTreeNodeType { get; set; }
    }
}
