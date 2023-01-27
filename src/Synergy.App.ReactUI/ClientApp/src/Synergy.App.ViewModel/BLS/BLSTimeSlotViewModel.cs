using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class BLSTimeSlotViewModel : ViewModelBase
    {
        public WeekDay Day { get; set; }
        public string LocationId { get; set; }
        public string SlotCategoryId { get; set; }   
        public string LocationName { get; set; }        
        public int SlotDuration { get; set; }
        public int NoOfCounter { get; set; }
        public List<TimeSlot> TimeSlotList { get; set; }
        public string TimeSlots { get; set; }
        public string BLS_TIME_SLOT { get; set; }
    }
    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime  { get; set; }
        public string ParentId { get; set; }
        public string Slot { get; set; }
        public string Id { get; set; }
        public bool Checked { get; set; }
    }
    public class Holiday
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
