


using System;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class LeaveViewModel : ViewModelBase    {

        public long? UserId { get; set; }
        public long? LeaveTypeId { get; set; }
        public string LeaveTypeCode { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public NtsActionEnum? LeaveStatus { get; set; }
        public AddDeductEnum? AddDeduct { get; set; }
        public double? Adjustment { get; set; }

    }
}
