using Synergy.App.ViewModel;
using System.Collections.Generic;

namespace Synergy.App.Api.Areas.IMS.Models
{
    public class POInvoiceSubmitModel
    {
        public string userId { get; set; }
        public string invoiceno { get; set; }
        public string pidate { get; set; }
        public List<string> receiptIds { get; set; }

        public string poId { get; set; }
        //public List<GoodsReceiptViewModel> challanList { get; set; }
    }
}
