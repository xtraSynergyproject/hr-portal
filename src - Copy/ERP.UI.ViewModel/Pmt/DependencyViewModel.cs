using ERP.Data.Model;
using ERP.Utility;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel.Pmt
{
    public class DependencyViewModel : IGanttDependency
    {        
        public long DependencyID { get; set; }
        public long PredecessorID { get; set; }
        public long SuccessorID { get; set; }
        public DependencyType Type { get; set; }
    }
}
