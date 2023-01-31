using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JMCAssetPaymentViewModel: ViewModelBase
    {
        public string AssetId { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetName { get; set; }
        public string Consumer { get; set; }
        public string AssetConsumerId { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public long Amount { get; set; }
        public string PaymentModeId { get; set; }
        public string PaymentStatusId { get; set; }
        public string PaymentStatusName { get; set; }
        public string PaymentReferenceNo { get; set; }
        public long PaymentCount { get; set; }
        public string PropertyId { get; set; }
    }
}
