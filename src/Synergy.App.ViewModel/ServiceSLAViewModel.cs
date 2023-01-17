using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ServiceSLAViewModel
    {
        public string DepartmentId { get; set; }
        public string TemplateId { get; set; }
        public string TemplateCode { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
