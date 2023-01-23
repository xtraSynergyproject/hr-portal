using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class NotificationTemplateViewModel : ViewModelBase
    {
        // public long NotificationTemplateId { get; set; }
        //public long DefaultNotificationTemplateId { get; set; }
        public long? NotificationMasterTemplateId { get; set; }
        [Display(Name = "DoNotCopyFromMasterTemplate", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public bool DoNotCopyFromMasterTemplate { get; set; }
        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }
        public string Name { get; set; }
        public long TemplateId { get; set; }
        public string TemplateName { get; set; }
        //public NtsTypeEnum NtsType { get; set; }
        public NtsActionEnum TemplateAction { get; set; }
        public ICollection<NtsUserTypeEnum> TargetUserType { get; set; }
        public NtsUserTypeEnum TargetUserTypes { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        [Display(Name = "NotifyByEmail", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public bool NotifyByEmail { get; set; }
        [Display(Name = "NotifyBySms", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public bool NotifyBySms { get; set; }
        public bool IsMasterTemplate { get; set; }
        public string[] TargetUser { get; set; }
        public string SmsText { get; set; }

    }
}
