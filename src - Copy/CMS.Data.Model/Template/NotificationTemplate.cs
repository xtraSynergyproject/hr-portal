using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NotificationTemplate : DataModelBase
    {

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public  Template  Template { get; set; }

        [ForeignKey("ParentNotificationTemplate")]
        public string ParentNotificationTemplateId { get; set; }
        public NotificationTemplate ParentNotificationTemplate { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SmsText { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySms { get; set; }
        public bool SendAlways { get; set; }
        public bool IsTemplate { get; set; }
        public bool AutoApplyOnAllTemplates { get; set; }
        public bool CopyFromTemplate { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public NotificationActionTypeEnum ActionType { get; set; }
        public string[] ActionStatusCodes { get; set; }

        public NtsActiveUserTypeEnum NotificationTo { get; set; }
        public string NotificationActionId { get; set; }
    }
    [Table("NotificationTemplateLog", Schema = "log")]
    public class NotificationTemplateLog : NotificationTemplate
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; } 
        public bool IsVersionLatest { get; set; }
    }

}
