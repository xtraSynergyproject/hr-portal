using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Help
{
    public class BusinessSupportViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int InProgressCount { get; set; }

        public int OverDueCount { get; set; }

        public int CompletedCount { get; set; }

        public long TotalTicketCount { get; set; }

        public long TotalNotificationCount { get; set; }

        public int NotificationReadCount { get; set; }

        public int NotificationUnReadCount { get; set; }
    }
}
