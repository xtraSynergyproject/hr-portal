using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class DeliveryNoteViewModel:ServiceTemplateViewModel
    {
        public string NameScopeOfWork { get; set; }
        public string GSTIN { get; set; }
        public DateTime DeliveryOn { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public string VehicleNo { get; set; }
        public string PIN { get; set; }
        public string ShippingAddress { get; set; }
        public string RequisitionId { get; set; }
        public string RequisitionCode { get; set; }
        public string RequisitionValue { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateTimeFormat)]
        public DateTime RequisitionDate { get; set; }
        public string ItemHead { get; set; }
        public string Particular { get; set; }
        public string IssueRemark { get; set; }
        public string IssuedItemIds { get; set; }
        public string AccknowledgeFileId { get; set; }
        
    }
}
