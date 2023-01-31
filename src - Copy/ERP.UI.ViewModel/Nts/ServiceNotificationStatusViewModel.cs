using ERP.Utility;

namespace ERP.UI.ViewModel
{
  
    public partial class ServiceNotificationStatusViewModel:ViewModelBase
    {
        public long ServiceId { get; set; }
        public long NotificationTemplateId { get; set; }
        public bool IsNotificationSent { get; set; }
    }
    
} 
