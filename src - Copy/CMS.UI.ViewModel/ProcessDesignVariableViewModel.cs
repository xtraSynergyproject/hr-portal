using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class ProcessDesignVariableViewModel : ProcessDesignVariable
    {
      public string FullyQualifiedName { get; set; }
        public string ParentId { get; set; }
        public bool hasChildren { get; set; }
        public bool expanded { get; set; }
    }
}
