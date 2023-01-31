using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ERP.UI.ViewModel
{
    public class InspectionTemplateViewModel : ViewModelBase
    {       
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }        
        public string collection { get; set; }
        public List<InspectionList> Controls { get; set; }
    }
    public class InsTemp
    {
        public string Id { get; set; }
        public string Val { get; set; }
        public string Cls { get; set; }
        public string Area { get; set; }
    }
    public class InspectionList
    {
        public string ControlName { get; set; }
        public string ControlGroup { get; set; }
        public string ControlId { get; set; }
    }
}
