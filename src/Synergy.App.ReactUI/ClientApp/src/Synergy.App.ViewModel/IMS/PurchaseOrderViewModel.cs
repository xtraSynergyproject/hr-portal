using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PurchaseOrderViewModel:ServiceTemplateViewModel
    {
        public string RequisitionIds { get; set; }
        public string POItemsData { get; set; }
        public string VendorName { get; set; }
        public string VendorId { get; set; }
        public string POLegalEntityId { get; set; }
        public string CityName { get; set; }
        public string GstNo { get; set; }
        public string ShipTo { get; set; }        
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public string ContactPersonId { get; set; }
        public string ContactNo { get; set; }
        public string ShippingAddress { get; set; }
        public string Remark { get; set; }
        public string PhoneNo { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime PoDate { get; set; }
        public string AllowMoreThanApprovedQuantity { get; set; }
        public string PurchaseOrderNo { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? PurchaseOrderDate { get; set; }
        public string POValue { get; set; }
        public string POStatus { get; set; }
        public string PreparedBy { get; set; }
        public string POID { get; set; }
        public string ContactPersonName { get; set; }
        public string VendorAddress { get; set; }

        public string ItemHeadId { get; set; }
        public string ItemHeadName { get; set; }
        public string IssueFromIds { get; set; }
        public string AllItemsReceived { get; set; }
        public int SNo { get; set; }
    }
}

