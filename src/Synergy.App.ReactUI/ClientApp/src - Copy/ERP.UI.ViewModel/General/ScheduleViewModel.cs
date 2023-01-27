using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ScheduleViewModel : ViewModelBase    {

        //public long ScheduleId { get; set; }
        public ScheduleEnum ScheduleFor { get; set; }
        public long? ReferenceId { get; set; }
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
        public int? Recurrence { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? Duration { get; set; }

        public RecurrencePatternEnum RecurrencePattern { get; set; }
        public int? RecurrenceInterval { get; set; }

        public DailyRecurrenceTypeEnum? DailyRecurrenceType { get; set; }

        //public ICollection<WeekDay> WeeklyRecurrenceDays { get; set; }

        public MonthlyRecurrenceTypeEnum? MonthlyRecurrenceType { get; set; }
        public int? MonthlyRecurrenceDate { get; set; }
        public int? MonthlyRecurrenceWeekNumber { get; set; }
        public WeekDay? MonthlyRecurrenceWeekDay { get; set; }

        public YearlyRecurrenceTypeEnum? YearlyRecurrenceType { get; set; }
        public int? YearlyRecurrenceMonth { get; set; }
        public int? YearlyRecurrenceMonthDate { get; set; }

        public int? YearlyRecurrenceWeekNumber { get; set; }
        public WeekDay? YearlyRecurrenceWeekDay { get; set; }
        public int? YearlyRecurrenceWeekDayMonth { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        public ScheduleEndTypeEnum ScheduleEndType { get; set; }
        public int? EndAfterRecurrence { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndByDate { get; set; }

        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        //public bool WeekDay8 { get; set; }

        public int? DailyRecurrence { get; set; }
        public int? WeeklyRecurrence { get; set; }
        public int? MonthlyDayRecurrence { get; set; }
        public int? MonthlyWeekRecurrence { get; set; }
        public int? YearlyRecurrence { get; set; }
        public string Description { get; set; }
        
    }
}

