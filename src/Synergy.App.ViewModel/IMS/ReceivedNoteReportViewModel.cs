using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ReceivedNoteReportViewModel
    {
        public string VendorName { get; set; }
        public string ChallanNo { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime ChallanDate { get; set; }
        public string InvoiceNo { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime InvoiceDate { get; set; }
        public string ReceivedBy { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime ReceivedOn { get; set; }
        public List<GoodsReceiptItemViewModel> GoodsReceiptItems { get; set; }
    }
}

