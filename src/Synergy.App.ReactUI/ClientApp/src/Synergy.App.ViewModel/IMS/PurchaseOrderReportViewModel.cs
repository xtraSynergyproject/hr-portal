using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PurchaseOrderReportViewModel
    {
        public string ServiceNo { get; set; }
        public string RequisitionIds { get; set; }
        public string POItemsData { get; set; }
        public string VendorName { get; set; }
        public string VendorId { get; set; }
        public string BillToUnit { get; set; }
        public string BillToUnitName { get; set; }
        public string BillToUnitAddress { get; set; }
        public string BillToUnitCountry { get; set; }
        public string BillToUnitState { get; set; }
        public string BillToUnitCity { get; set; }
        public string BillToUnitPinCode { get; set; }
        public string BillToUnitPANNo { get; set; }
        public string BillToUnitTANNo { get; set; }
        public string BillToUnitGSTNo { get; set; }
        public string BillToUnitPhoneNo { get; set; }
        public string BillToUnitMobile { get; set; }
        public string GstNo { get; set; }
        public string ShipTo { get; set; }        
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
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
        public string CreatedBy { get; set; }
        public string POID { get; set; }
        public string ContactPersonName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorGstNo { get; set; }
        public string VendorPanNo { get; set; }
        public string ItemHeadId { get; set; }
        public string AllItemsReceived { get; set; }
        public decimal TotalBaseValue { get; set; }
        public decimal TotalSGST { get; set; }
        public decimal TotalCGST { get; set; }
        public decimal TotalIGST { get; set; }
        public decimal NetPayable { get; set; }
        public string NetPayableInWords { get; set; }
        public List<POTermsAndConditionsViewModel> POTermsAndConditions { get; set; }
        public List<POItemsViewModel> POItems { get; set; }


    }
}

