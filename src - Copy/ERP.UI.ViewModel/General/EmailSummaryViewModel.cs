using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class EmailSummaryViewModel : ViewModelBase
    {
        public DashboardSummaryViewModel Sps { get; set; }
        public DashboardSummaryViewModel Pms { get; set; }
        public DashboardSummaryViewModel Worklist { get; set; }

        public DateTime? SummaryDate { get; set; }
        public long? UserId { get; set; }
        public NotificationStatusEnum? EmailStatus { get; set; }

    }
}
