using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PayrollSchedulerViewModel : ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Schedule Type")]
        public ScheduleTypeEnum ScheduleType { get; set; }
        [Required]
        [Display(Name = "Schedule Action")]
        public PayrollSchedulerActionEnum SchedulerAction { get; set; }

        [Display(Name = "Schedule Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? ScheduleTime { get; set; }
        [Display(Name = "Schedule Order")]
        public long? ScheduleOrder { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Delay In Execution")]
        public int? DelayInExecution { get; set; }
        [Display(Name = "Schedule Recurrence")]
        public int ScheduleRecur { get; set; }
        [Display(Name = "Enable Manual Execution")]
        public bool? EnableManualExecution { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string Param5 { get; set; }
        public string Param6 { get; set; }
        public string Param7 { get; set; }
        public string Param8 { get; set; }
        public string Param9 { get; set; }
        public string Param10 { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime? LastScheduleStartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime? LastScheduleEndTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime? LastSuccessfulProcessTime { get; set; }

        public ScheduleExecutionStatusEnum ExecutionStatus { get; set; }
        public ScheduleExecutionStatusEnum LastScheduleStatus { get; set; }

        //[Required]
        //public string Devices { get; set; }

        //public long[] DeviceData { get; set; }
    }
}
