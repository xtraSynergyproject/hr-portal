using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ApplicationLoginsViewModel
    {
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public long OrganizationId { get; set; }
    }
}
