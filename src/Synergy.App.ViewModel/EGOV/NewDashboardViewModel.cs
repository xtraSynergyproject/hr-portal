using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class NewDashboardViewModel
    {
    //    public IList<NewdashboardViewModel> DashboardList { get; set; }
      
    //}

    //public class NewdashboardViewModel
    //{
       
        public int? Count { get; set; }
        public int? ProjectCount { get; set; }
        public int? FinishedCount { get; set; }
        public int? LikeCount { get; set; }
        public int? CommentCount { get; set; }
        public string ProjectDate { get; set; }
        public string DateTime { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }
}
