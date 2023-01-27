using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;


namespace CMS.UI.ViewModel
{
    public class ListOfValueViewModel : ListOfValue
    {
       public string JobCount { get; set; }
       public string ParentName { get; set; }
       public string Json { get; set; }
        public bool hasChildren { get; set; }
    }
}
