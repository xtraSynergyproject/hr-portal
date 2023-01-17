using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
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
        public string ItemHeadName { get; set; }
        public string RequisitionValue { get; set; }
        public string POID { get; set; }
        public double RequisitionNo { get; set; }
        public string RequisitionType { get; set; }
        public string StorageType { get; set; }
        public string RequisitionStatus { get; set; }
        public string OrderValue { get; set; }
        public string ItemName { get; set; }
        public double RequisitionQuantity { get; set; }
        public double ApprovedQuantity { get; set; }
        public int SNo { get; set; }

    }
}
