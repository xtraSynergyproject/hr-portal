using ERP.Utility;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_NotificationTemplate : NodeBase
    {        
        //public long? NotificationMasterTemplateId { get; set; }
        public bool DoNotCopyFromMasterTemplate { get; set; }
        
        public bool IsMasterTemplate { get; set; }
        //public long? TemplateId { get; set; }
        public string Name { get; set; }
        public NtsActionEnum TemplateAction { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        //public ICollection<NtsUserTypeEnum> TargetUserType { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SmsText { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySms { get; set; }
        public bool SendAlways { get; set; }
        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }
    }
    public class R_NotificationTemplate_TargetUserType_ListOfValue : RelationshipBase
    {

    }
    public class R_NotificationTemplate_NotificationMasterTemplate : RelationshipBase
    {

    }
    public class R_NotificationTemplate_Template : RelationshipBase
    {

    }

}
