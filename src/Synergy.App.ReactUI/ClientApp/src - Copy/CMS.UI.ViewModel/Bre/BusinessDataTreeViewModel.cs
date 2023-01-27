using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.UI.ViewModel
{
    public class BusinessDataTreeViewModel : DataModelBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public bool Expanded { get; set; }
        public bool HasSubFolders { get; set; }
        public BusinessDataTreeNodeTypeEnum BusinessDataTreeNodeType { get; set; }
    }
}
