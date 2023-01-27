using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class WorkerPool1ViewModel 
    {
        public bool? HRApprovl { get; set; }
        public string HRApprovlName { get; set; }
        public string HRHeadApprovalName { get; set; }
      
        public string HRHeadComment { get; set; }

        public bool? HodApprovl { get; set; }
        public string HodComment { get; set; }

        public bool? PlanningApprovl { get; set; }
        public string PlanningComment { get; set; }

        public bool? EDApprovl { get; set; }
        public string EDComment { get; set; }
        public string Id { get; set; }
        public string HODApprovalName { get; set; }
      
        public string PlanningApprovalName { get; set; }
        public string EDApprovalName { get; set; }




    }
}
