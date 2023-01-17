using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ScheduleInfo
    {
        public bool isPrivate;
        public string location;

        public string id { get; set; }
        public string calendarId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public bool isAllday { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string category { get; set; }
        public string dueDateClass { get; set; }
        public string color { get; set; }
        public string bgColor { get; set; }
        public string dragBgColor { get; set; }
        public string borderColor { get; set; }
        public string customStyle { get; set; }
        public bool isFocused { get; set; }
        public bool isPending { get; set; }
        public bool isVisible { get; set; }
        public bool isReadOnly { get; set; }
        public double goingDuration { get; set; }
        public double comingDuration { get; set; }
        public string recurrenceRule { get; set; }
        public string state { get; set; }
        public raw raw { get; set; }
    }

    public class raw
    {
        public string memo { get; set; }
        public bool hasToOrCc { get; set; }
        public bool hasRecurrenceRule { get; set; }
        public string location { get; set; }
        public string classs { get; set; }
        public creator creator { get; set; }
    }
    public class creator
    {
        public string name { get; set; }
        public string avatar { get; set; }
        public string company { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
