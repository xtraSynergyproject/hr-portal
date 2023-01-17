using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class CalendarHolidayViewModel : ViewModelBase
    {   
        [Required]
        [Display(Name ="Holiday Name")]
        public string HolidayName { get; set; }
        [Required]
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? FromDate { get; set; }
        [Required]
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ToDate { get; set; }
        [Display(Name = "Calendar Name")]
        public long? CalendarId { get; set; }
        [Display(Name = "Calendar Name")]
        public string CalendarName { get; set; }
        public int Year { get; set; }

        public long? PersonId { get; set; }
        public long? UserId { get; set; }
    }
}
