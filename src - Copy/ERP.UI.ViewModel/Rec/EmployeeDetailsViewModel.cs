using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class EmployeeDetailsViewModel
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public ReasonForLeavingEnum? ReasonForLeaving { get; set; }
        public string Quarter { get; set; }
        public string DepQuarter { get; set; }
        public DateTime? ActualTerminationDate { get; set; }
        public string Department { get; set; }
        public long? DepartmentId { get; set; }
        public string Supervisor { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string PositionStatus { get; set; }
        public long? PositionStatusId { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
    }
}
