using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class LeaveViewModel : ViewModelBase
    {

        public string UserId { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveTypeCode { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public string LeaveStatus { get; set; }
        //public AddDeductEnum? AddDeduct { get; set; }
        public string AddDeduct { get; set; }
        public double? Adjustment { get; set; }
        public string ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceSubject { get; set; }
        public string ServiceDescription { get; set; }

    }
}
