using ERP.Data.Model;
using ERP.Utility;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class NotificationSearchViewModel : ViewModelBase
    {
        public IList<NotificationViewModel> Notifications { get; set; }
    }
}
