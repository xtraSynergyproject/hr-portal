using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////using Kendo.Mvc.UI;
namespace Synergy.App.ViewModel
{
    public class DMSCalenderViewModel//: ISchedulerEvent
    {

        public string Id { get; set; }

        public string Title { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public string NtsId { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public string UserId { get; set; }

        public string RecurrId { get; set; }

        public DateTime DueDate { get; set; }

        public string TitleLimited { get { return Title; } }
        public string NoteStatus { get; set; }

        public string TemplateName { get; set; }

        public string templatecode { get; set; }


        public string ParentId { get; set; }



    }
}
