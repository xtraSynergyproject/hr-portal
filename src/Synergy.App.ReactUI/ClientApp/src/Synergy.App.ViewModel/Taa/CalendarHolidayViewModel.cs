using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CalendarHolidayViewModel : NoteTemplateViewModel
    {
        public string HolidayName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CalendarId { get; set; }
        public string UserId { get; set; }

    }
}
