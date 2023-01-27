using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PurchaseInvoiceReportViewModel
    {
        public string InvoiceNo { get; set; }
        public string VendorName { get; set; }
        public string VendorGSTNo { get; set; }

        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public List<InvoiceItemViewModel> PurchaseInvoiceItems { get; set; }
        public int SNo { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }

    }
}

