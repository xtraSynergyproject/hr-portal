using System;
using Kendo.Mvc.UI;

namespace ERP.UI.ViewModel
{
    public class DependencyViewModel : IGanttDependency
    {
        public int DependencyID { get; set; }

        public int PredecessorID { get; set; }
        public int SuccessorID { get; set; }
        public DependencyType Type { get; set; }
    }
}
   