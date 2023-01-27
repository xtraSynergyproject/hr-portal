using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    
    public partial class TAA_LeaveType : NodeBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool ValidateEntitlement { get; set; }
        public double DefaultEntitlement { get; set; }

    }
    
}
