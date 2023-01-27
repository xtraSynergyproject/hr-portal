using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class SmsViewModel : ViewModelBase
    {

        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public string MobileNo { get; set; }
        public string SmsText { get; set; }
        public NotificationStatusEnum SmsStatus { get; set; }
        public int RetryCount { get; set; }
        public bool IsAsync { get; set; }
        public string SenderName { get; set; }
        public string SmsGateWay { get; set; }
        public string SmsUserId { get; set; }
        public string SmsPassword { get; set; }
        public string Error { get; set; }
        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }

    }
}
