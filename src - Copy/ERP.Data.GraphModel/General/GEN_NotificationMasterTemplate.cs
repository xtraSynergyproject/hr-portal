using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class GEN_NotificationMasterTemplate : NodeBase
    {       
        public string Code { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SmsText { get; set; }

        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }
        public NtsActionEnum TemplateAction { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySms { get; set; }
        public bool SendAlways { get; set; }
        public string NotificationUrl { get; set; }
        // public ICollection<NtsUserTypeEnum> TargetUserType { get; set; }
        public NotificationTemplateTypeEnum? NotificationTemplateType { get; set; }

    }
    public class R_NotificationMasterTemplate_TargetUserType_ListOfValue : RelationshipBase
    {

    }
}
