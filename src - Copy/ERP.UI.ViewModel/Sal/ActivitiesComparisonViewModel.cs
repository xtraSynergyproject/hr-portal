using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ActivitiesComparisonViewModel : ViewModelBase
    {
        public long? TargetCall { get; set; }
        public long? TargetMeeting { get; set; }
        public long? TargetLead { get; set; }
        public long? TargetBroker { get; set; }
        public long? ActualCall { get; set; }
        public long? ActualtMeeting { get; set; }
        public long? ActualLead { get; set; }
        public long? ActualBroker { get; set; }
        public string UserName { get; set; }
        public string ActualDate { get; set; }
    }
}

