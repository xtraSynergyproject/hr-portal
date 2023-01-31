using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class HITSVacationViewModel : ViewModelBase
    {
        public long EmpId { get; set; }
        public string Name { get; set; }
        public string Vacation { get; set; }
        public Nullable<DateTime> From { get; set; }
        public Nullable<DateTime> To { get; set; }
        public string EnterBy { get; set; }
        public Nullable<DateTime> EnterDate { get; set; }
        public string ChangeBy { get; set; }
        public Nullable<DateTime> ChangeDate { get; set; }

        [Display(Name = "Calendar Days")]
        public double? LeaveDuration { get; set; }

        [Display(Name = "Working Days")]
        public double? WorkingDuration { get; set; }
    }
}
