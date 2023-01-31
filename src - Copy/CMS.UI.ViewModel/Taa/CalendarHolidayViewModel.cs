using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
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
