
namespace ERP.Data.GraphModel
{
  
    public partial class NTS_NoteNotificationStatus : NodeBase
    {       
       // public long NoteId { get; set; }
       // public long NotificationTemplateId { get; set; }
        public bool IsNotificationSent { get; set; }
    }
    public class R_NoteNotificationStatus_Note : RelationshipBase
    {

    }
    public class R_NoteNotificationStatus_NotificationTemplate : RelationshipBase
    {

    }
}
