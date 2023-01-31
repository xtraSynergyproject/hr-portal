using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class EmployeeAnnualLeaveBalanceViewModel
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string IqhamahNo { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public double? AnnualLeaveEntitlement { get; set; }
        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }
    }
}
