using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class RequisitionViewModel:ServiceTemplateViewModel
    {
        public string RequisitionId { get; set; }
        public string CustomerName { get; set; }
       
        public string Customer { get; set; }
       
        public DateTime RequisitionDate { get; set; }
        public string RequisitionParticular { get; set; }
        public string ProposalCode { get; set; }
        public string ItemHead { get; set; }
        public string RequisitionValue { get; set; }

    }
}
