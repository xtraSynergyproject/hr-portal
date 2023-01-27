using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;


namespace Synergy.App.ViewModel
{
    public class ListOfValueViewModel : ListOfValue
    {
       public string JobCount { get; set; }
       public string ParentName { get; set; }
       public string Json { get; set; }
        public bool hasChildren { get; set; }
    }
}
