using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CalendarViewModel : NoteTemplateViewModel
    {
        public int Year { get; set; }
        [Display(Name = "Calendar Name")]
        public string Name { get; set; }
        public string Code { get; set; }
        [Display(Name = "Sunday")]
        public bool IsSundayWeekEnd { get; set; }
        [Display(Name = "Monday")]
        public bool IsMondayWeekEnd { get; set; }
        [Display(Name = "Tuesday")]
        public bool IsTuesdayWeekEnd { get; set; }
        [Display(Name = "Wednesday")]
        public bool IsWednesdayWeekEnd { get; set; }
        [Display(Name = "Thursday")]
        public bool IsThursdayWeekEnd { get; set; }
        [Display(Name = "Friday")]
        public bool IsFridayWeekEnd { get; set; }
        [Display(Name = "Saturday")]
        public bool IsSaturdayWeekEnd { get; set; }

        public int? HolidayCount { get; set; }
        public int? WeekendCount { get; set; }

    }
}
