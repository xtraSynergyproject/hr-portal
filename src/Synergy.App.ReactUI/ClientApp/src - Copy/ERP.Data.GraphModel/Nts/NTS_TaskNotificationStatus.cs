using ERP.Utility;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_TaskNotificationStatus : NodeBase
    {
        public bool IsNotificationSent { get; set; }
    }
    public class R_TaskNotificationStatus_Task : RelationshipBase
    {

    }
    public class R_TaskNotificationStatus_NotificationTemplate : RelationshipBase
    {

    }

}
