using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel.Sal
{
    public class SAL_Goal : NodeBase
    {
        public string Name { get; set; }
        public SalGoalIntervalEnum Interval { get; set; }
        public SalGoalTypeEnum GoalType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

       // public long? UsersList { get; set; }

       // public long? TeamList { get; set; }

        public SalGoalMetricEnum Metric { get; set; }

        public long Target { get; set; }
    }
    public class R_Goal_User : RelationshipBase
    {

    }
    public class R_Goal_Team : RelationshipBase
    {

    }
}
