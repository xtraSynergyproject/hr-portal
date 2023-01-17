using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{

    public class GEN_Notification : NodeBase
    {
        public ReferenceTypeEnum ReferenceTypeCode { get; set; }
        public ReadStatusEnum ReadStatus { get; set; }

        public NotificationStatusEnum NotificationStatus { get; set; }
        public string From { get; set; }

        public string To { get; set; }

        public string SenderName { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SmsText { get; set; }
        public DateTime? NotificationDateTime { get; set; }


        public string Url { get; set; }
        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }

        public ModuleEnum? ModuleName { get; set; }
        public NotificationTypeEnum? NotificationType { get; set; }

        public bool NotifyByEmail { get; set; }
        public bool NotifyBySms { get; set; }
        public bool SendAlways { get; set; }
        public string ReferenceTypeNo { get; set; }
    }
    public class R_Notification_ToUser : RelationshipBase
    {

    }
    public class R_Notification_FromUser : RelationshipBase
    {

    }
    public class R_Notification_MasterTemplate : RelationshipBase
    {

    }
    public class R_Notification_ToCandidate : RelationshipBase
    {

    }
    public class R_Notification_FromCandidate : RelationshipBase
    {

    }
    public class R_Notification_Reference : RelationshipBase
    {
        public ReferenceTypeEnum ReferenceTypeCode { get; set; }
    }
    public class R_Notification_NotificationGroup : RelationshipBase
    {
    }
}
