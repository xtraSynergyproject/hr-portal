using System;
using Kendo.Mvc.UI;


namespace Cms.UI.ViewModel
{
    public class DependencyViewModel : IGanttDependency

    {
        public int DependencyID { get; set; }

        public int PredecessorID { get; set; }
        public int SuccessorID { get; set; }
        public DependencyType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        // string IGanttDependency.DependencyType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public DependencyType Type { get; set; }
    }
}

