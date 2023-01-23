

namespace ERP.UI.ViewModel
{
  
    public partial class NoteNotificationStatusViewModel:ViewModelBase
    {
        public long NoteId { get; set; }
        public long NotificationTemplateId { get; set; }
        public bool IsNotificationSent { get; set; }
    }
    
}
 