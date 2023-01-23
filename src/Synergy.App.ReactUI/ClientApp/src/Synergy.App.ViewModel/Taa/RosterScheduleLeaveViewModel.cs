using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class RosterScheduleLeaveViewModel 
    {

        public string UserId { get; set; }
        public DateTime? Sunday { get; set; }
        public DateTime? Monday { get; set; }
        public DateTime? Tuesday { get; set; }
        public DateTime? Wednesday { get; set; }
        public DateTime? Thursday { get; set; }
        public DateTime? Friday { get; set; }
        public DateTime? Saturday { get; set; }

    }
}
