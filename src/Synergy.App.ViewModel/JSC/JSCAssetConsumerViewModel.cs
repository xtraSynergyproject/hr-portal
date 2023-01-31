using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCAssetConsumerViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AssetName { get; set; }
        public string AssetDescription { get; set; }
        public string SpecificLocation { get; set; }
        public string AssetPhotoId { get; set; }        
        public string WardId { get; set; }        
        public string Latitude { get; set; }        
        public string Longitude { get; set; }        
        public DateTime? AllotmentFromDate { get; set; }
        public DateTime? AllotmentToDate { get; set; }
        public string WardName { get; set; }
        public long AssetCount { get; set; }
        public string AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public long Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public string PaymentStatusName { get; set; }
        public string TaskId { get; set; }
        public string UdfNoteTableId { get; set; }
        public string AssignedToUserId { get; set; }

        public string ConsumerNo { get; set; }
        public string ConsumerId { get; set; }
        public string ConsumerName { get; set; }
        public DateTime? AllotmentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentReferenceNo { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? BillDate { get; set; }
    }
}
