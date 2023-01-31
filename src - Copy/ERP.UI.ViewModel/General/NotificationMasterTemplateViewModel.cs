using ERP.Data.Model;
using ERP.Utility;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class NotificationMasterTemplateViewModel : ViewModelBase
    {
        public long NotificationMasterTemplateId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public string Body { get; set; }
        [Display(Name = "Notify By Email")]
        public bool NotifyByEmail { get; set; }

        [Display(Name = "Notify By Email")]
        public string NotifyByEmailString {get { return NotifyByEmail ? "Yes" : "No"; }}

        [Display(Name = "Notify By Sms")]
        public bool NotifyBySms { get; set; }

        [Display(Name = "Notify By Sms")]
        public string NotifyBySmsString { get { return NotifyBySms ? "Yes" : "No"; } }

        public bool ShowOriginalSender { get; set; }
        [Display(Name = "Show Original Sender")]
        public string ShowOriginalSenderString { get { return ShowOriginalSender ? "Yes" : "No"; } }
        
        public bool DisableDefaultEmailTemplate { get; set; }
        [Display(Name = "Disable Default Email Template")]
        public string DisableDefaultEmailTemplateString { get { return DisableDefaultEmailTemplate ? "Yes" : "No"; } }
        [Display(Name = "Reference Type")]
        public string ReferenceTypeCode { get; set; }
        public string ReferenceTypeName { get; set; }
        public NtsActionEnum TemplateAction { get; set; }
        public ICollection<NtsUserTypeEnum> TargetUserType { get; set; }
        public NtsUserTypeEnum TargetUserTypes { get; set; }
        public string[] TargetUser { get; set; }
        public string SmsText { get; set; }
        public string NotificationUrl { get; set; }

        public NotificationTemplateTypeEnum? NotificationTemplateType { get; set; }
    }
}
