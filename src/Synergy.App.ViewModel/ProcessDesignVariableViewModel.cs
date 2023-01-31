using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class ProcessDesignVariableViewModel : ProcessDesignVariable
    {
      public string FullyQualifiedName { get; set; }
        public string ParentId { get; set; }
        public bool hasChildren { get; set; }
        public bool expanded { get; set; }
    }
}
