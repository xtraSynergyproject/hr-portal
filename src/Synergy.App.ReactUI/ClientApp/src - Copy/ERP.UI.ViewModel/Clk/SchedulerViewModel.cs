using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SchedulerViewModel : ViewModelBase
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public string Name { get; set; }
        //[Required]
        //[Display(Name = "Schedule Type")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "ScheduleType", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public ScheduleTypeEnum ScheduleType { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "SchedulerAction", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public ClockServerSchedulerActionEnum SchedulerAction { get; set; }

        [Display(Name = "ScheduleTime", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? ScheduleTime { get; set; }
        [Display(Name = "ScheduleOrder", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public long? ScheduleOrder { get; set; }
        //[Required]
        //[Display(Name = "Start Date")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "DelayInExecution", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public int? DelayInExecution { get; set; }
        [Display(Name = "ScheduleRecur", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public int ScheduleRecur { get; set; }
        [Display(Name = "EnableManualExecution", ResourceType = typeof(ERP.Translation.Clk.Clk))]
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

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //public string Devices { get; set; }

        //public long[] DeviceData { get; set; }
    }
}
