using ERP.Utility;

namespace ERP.UI.ViewModel
{
  
    public partial class TaskNotificationStatusViewModel:ViewModelBase
    {
        public long TaskId { get; set; }
        public long NotificationTemplateId { get; set; }
        public bool IsNotificationSent { get; set; } 
    }
    
}
