using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class DeviceViewModel : ViewModelBase
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name="IP Address")]
        [Display(Name = "IPAddress", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public string IPAddress { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Port No")]
        [Display(Name = "PortNo", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public int PortNo { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Machine No")]
        [Display(Name = "MachineNo", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public int MachineNo { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Serial No")]
        [Display(Name = "SerialNo", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public string SerialNo { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Communication Type")]
        [Display(Name = "CommunicationType", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public CommunicationTypeEnum CommunicationType { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Punching Type")]
        [Display(Name = "PunchingType", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public PunchingTypeEnum PunchingType { get; set; }
        //[Display(Name = "Enable Error Notification")]
        [Display(Name = "EnableErrorNotification", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public bool EnableErrorNotification { get; set; }
        //[Display(Name = "Notificiation Recipient")]
        [Display(Name = "NotificiationRecipient", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public string NotificiationRecipient { get; set; }


        //  public TimeSpan? ScheduleTime { get; set; }
        [Display(Name = "Last Successful Download Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime? LastSuccessfulDownloadTime { get; set; }
        public DateTime ExecutionStartDate { get; set; }
        public DateTime? ExecutionEndDate { get; set; }
        [Display(Name = "Last Execution Status")]
        public ScheduleExecutionStatusEnum? ExecutionStatus { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Product")]
        [Display(Name = "ProductId", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public long ProductId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Time Zone")]
        [Display(Name = "TimeZoneId", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public long TimeZoneId { get; set; }

        public long ScheduleId { get; set; }
        public string ExecutionError { get; set; }
        public string ProductName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? ProposedLogCleanupStartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? ProposedLogCleanupEndTime { get; set; }
        public bool? EnableLogCleanup { get; set; }

        public DateTime? LastSuccessfulCleanupTime { get; set; }
    }
}
