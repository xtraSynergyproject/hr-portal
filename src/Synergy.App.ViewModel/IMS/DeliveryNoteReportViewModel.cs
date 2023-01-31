using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class DeliveryNoteReportViewModel
    {
        public string ServiceNo { get; set; }
        public string CreatedBy { get; set; }
        public string NameScopeOfWork { get; set; }
        public string GSTIN { get; set; }
        public DateTime DeliveryOn { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
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
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string RequisitionNo { get; set; }
        public string LegalEntityName { get; set; }
        public string LegalEntityAddress { get; set; }
        public string LegalEntityEmail { get; set; }
        public List<DeliveryItemViewModel> DeliveryItems { get; set; }
    }
}
