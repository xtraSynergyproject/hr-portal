using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_SLAChangeRequest : NodeBase
    {
        public DateTime? StartDate { get; set; }
        public int? ProposedSLA { get; set; }
        public DateTime? ProposedDueDate { get; set; }
        public int? ApprovedSLA { get; set; }
        public DateTime? ApprovedDueDate { get; set; }
        public SLARequestStatusEnum RequestStatus { get; set; }
        public string RequestReason { get; set; }
        public string RejectReason { get; set; }
        public string ApprovalComment { get; set; }
    }
    public class R_SLAChangeRequest_Task : RelationshipBase
    {

    }
}
