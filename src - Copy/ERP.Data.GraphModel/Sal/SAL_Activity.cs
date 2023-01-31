using System;
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class SAL_Activity : NodeBase
    {
        public SalActivityTypeEnum ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public SalActivityOutcomeEnum ActivityOutcome { get; set; }
        public SalActivityStatusEnum ActivityStatus { get; set; }
      
        public DateTime? MeetingDate { get; set; }
    }

    public class R_Activity_Lead : RelationshipBase
    {
        
    }

}
