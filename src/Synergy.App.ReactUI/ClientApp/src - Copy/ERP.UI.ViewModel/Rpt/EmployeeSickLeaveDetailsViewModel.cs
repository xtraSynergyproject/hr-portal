using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class EmployeeSickLeaveDetailsViewModel
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string IqhamahNo { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }
        public double? DaysCount { get; set; }
        public long ServiceId { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public string LeaveDuration { get; set; }
    }
}
