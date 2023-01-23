using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
  public  class GoalViewModel
    {
        public string Id { get; set; }
        public string GoalName { get; set; }
        public string ServiceId { get; set; }
        public Int32 Weightage { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string GoalStatus { get; set; }
        public string SuccessCriteria { get; set; }
        public string ParentGoal { get; set; }
        public string Department { get; set; }        
        public string DepartmentOwnerUserId { get; set; }
        public string Year { get; set; }
        public string PerformanceMaster { get; set; }
        public string PerformanceStage { get; set; }
        public string PerformanceUser { get; set; }
    }
}
