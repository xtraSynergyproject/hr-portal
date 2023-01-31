using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class HrAnalyticsViewModel : ViewModelBase
    {

        public string Department { get; set; }
        public string EmployeeName { get; set; }
        public string Supervisor { get; set; }
        [Display(Name ="Tenure(Months)")]
        public long? Tenure { get; set; }
        public double? AttritionRate { get; set; }
        public long? DepartureCount { get; set; }
        public long? Departures { get; set; }
        public long? Employees { get; set; }

    }
}
