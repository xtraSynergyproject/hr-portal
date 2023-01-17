using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
////using Kendo.Mvc.UI;

namespace Synergy.App.ViewModel
{
    public class RosterTimeLineViewModel //: ISchedulerEvent
    {
        public string Id { get; set; }
        public string PersonId { get; set; }
        public string UserName { get; set; }
        //public long? LeaveTypeId { get; set; }
        //public string LeaveTypeCode { get; set; }
        //public string LeaveStatus { get; set; }
        //public string LeaveStatusCode { get; set; }
        //private string _Title { get; set; }
        public string Type { get; set; }
        public int TypeVal { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public TimeSpan? Duty1StartTime { get; set; }
        public TimeSpan? Duty1EndTime { get; set; }
        public bool Duty1FallsNextDay { get; set; }
        public bool Duty2Enabled { get; set; }
        public TimeSpan? Duty2StartTime { get; set; }
        public TimeSpan? Duty2EndTime { get; set; }
        public bool Duty2FallsNextDay { get; set; }
        public bool Duty3Enabled { get; set; }
        public TimeSpan? Duty3StartTime { get; set; }
        public TimeSpan? Duty3EndTime { get; set; }
        public bool Duty3FallsNextDay { get; set; }

        public DateTime? RosterDate { get; set; }
    }
}
