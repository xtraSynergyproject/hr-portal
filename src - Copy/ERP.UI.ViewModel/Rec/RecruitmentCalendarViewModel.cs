using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class RecruitmentCalendarViewModel : ISchedulerEvent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public long? Duration { get; set; }
        public string Url { get; set; }
        public string JobName { get; set; }
        public long? JobId { get; set; }
        public string DepartmentName { get; set; }
        public long? DepartmentId { get; set; }

    }
}
