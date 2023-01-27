using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ScheduleInvoiceViewModel:ServiceTemplateViewModel
    {
        public string RequisitionId { get; set; }
        public string RequisitionCode { get; set; }
        public string CustomerName { get; set; }
       
        public string CustomerId { get; set; }
       
        public DateTime RequisitionFromDate { get; set; }
        public DateTime RequisitionToDate { get; set; }
        public string AmountBase { get; set; }
        public string InvoiceScheduleDate { get; set; }
        public string ItemHeadId { get; set; }
        public string Requisition { get; set; }
        public string RequisitionIds { get; set; }
        public string POItemsData { get; set; }
        public string ISId { get; set; }
        public string ItemId { get; set; }
        public string RequisitionItemId { get; set; }


    }
}
