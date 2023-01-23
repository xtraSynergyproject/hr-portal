using ERP.Utility;
using System;

namespace ERP.UI.ViewModel
{
    public class RosterScheduleLeaveViewModel : ViewModelBase    {
       
        public long UserId { get; set; }
        public DateTime? Sunday { get; set; }
        public DateTime? Monday { get; set; }
        public DateTime? Tuesday { get; set; }
        public DateTime? Wednesday { get; set; }
        public DateTime? Thursday { get; set; }
        public DateTime? Friday { get; set; }
        public DateTime? Saturday { get; set; }

    }
}
