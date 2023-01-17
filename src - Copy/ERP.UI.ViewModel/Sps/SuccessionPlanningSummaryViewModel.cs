using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class SuccessionPlanningSummaryViewModel : ViewModelBase    {
        [Display(Name = "StageName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningSummary))]
        public string StageName { get; set; }
        [Display(Name = "NotStarted", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningSummary))]
        public long NotStarted { get; set; }
        [Display(Name = "Started", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningSummary))]
        public long Started { get; set; }
        [Display(Name = "Completed", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningSummary))]
        public long Completed { get; set; }
        [Display(Name = "Cancelled", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningSummary))]
        public long Cancelled { get; set; }
        [Display(Name = "Overdue", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningSummary))]
        public long Overdue { get; set; }
    }
}
