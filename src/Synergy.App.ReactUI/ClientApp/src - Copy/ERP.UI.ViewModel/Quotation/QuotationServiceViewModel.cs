using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
  public  class QuotationServiceViewModel:ServiceViewModel
    {
        public QuotationTypeEnum? QuotationType { get; set; }
        public InventoryTypeEnum? InventoryType { get; set; }
        public long? PurchaseOrderId { get; set; }
        public long? QuotationId { get; set; }
        public long? CustomerContactId { get; set; }
        public long? SupplierContactId { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Designation { get; set; }
        public string UserCode { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public long? DeliveryNoteId { get; set; }
    }
}
