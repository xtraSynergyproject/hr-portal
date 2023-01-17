using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class EmployeeTimePermissionViewModel
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string IqhamahNo { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionName { get; set; }
        public string SignInType { get; set; }
        public DateTime? LogDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TimePermissionType { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Status { get; set; }
        public long ServiceId { get; set; }
        public string OtherInformation { get; set; }
        public long OrganizationId { get; set; }
    }
}
