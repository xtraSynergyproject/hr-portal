using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    
    public partial class TAA_LeaveBalanceSheet : NodeBase
    {
        public int Year { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
    }
    public partial class R_LeaveBalanceSheet_User : RelationshipBase
    {
    }

    public partial class R_LeaveBalanceSheet_LeaveType : RelationshipBase
    {
    }

}
