using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TargetViewModel : ViewModelBase
    {
        public long? TargetCall { get; set; }
        public long? TargetMeeting { get; set; }
        public long? TargetLead { get; set; }
        public long? TargetBroker { get; set; }
        public long? TargetSale { get; set; }
        public long? UserId { get; set; }
    }
}

