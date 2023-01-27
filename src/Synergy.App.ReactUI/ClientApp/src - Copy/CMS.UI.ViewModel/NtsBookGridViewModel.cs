using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class NtsBookGridViewModel
    {
        public NtsTypeEnum NtsType { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public string Section { get; set; }
        public string AssigneeOrOwner { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
