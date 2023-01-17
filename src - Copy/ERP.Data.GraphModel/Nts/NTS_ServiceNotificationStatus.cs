using ERP.Utility;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_ServiceNotificationStatus : NodeBase
    {       
       // public long ServiceId { get; set; }
       // public long NotificationTemplateId { get; set; }
        public bool IsNotificationSent { get; set; }
    }
    public class R_ServiceNotificationStatus_Service : RelationshipBase
    {

    }
    public class R_ServiceNotificationStatus_NotificationTemplate : RelationshipBase
    {

    }
}
