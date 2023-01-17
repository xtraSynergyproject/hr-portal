using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.App.ViewModel
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
