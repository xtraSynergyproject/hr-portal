using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class VacationCostCenterViewModel
    {
        public string VacationName { get; set; }
        public int EmployeesCount { get; set; }
        public double DaysCount { get; set; }
        public long? CostCenterId { get; set; }
        public string CostCenterName { get; set; }

    }
}
