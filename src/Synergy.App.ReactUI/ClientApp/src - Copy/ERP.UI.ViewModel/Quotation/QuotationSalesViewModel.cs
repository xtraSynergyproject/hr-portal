using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class QuotationSalesViewModel : ViewModelBase
    {       
        public string QuotationNo { get; set; }
        public string SystemName { get; set; }
        public long? CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Currency { get; set; }
        public string NetAmount { get; set; }
        public DateTime QuotationDate { get; set; }
    }
  
}
