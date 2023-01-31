using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class UnitViewModel : ViewModelBase
    {
        [Display(Name = "S.No.")]
        public string SNO { get; set; }
        [Required]
        [Display(Name = "Unit Number")]
        public string Name { get; set; }
        [Display(Name = "Floor")]
        public string FloorNumber { get; set; }
        [Display(Name = "Unit Area")]
        public string Area { get; set; }
        [Display(Name = "Bedrooms")]
        public int? NoOfBedrooms { get; set; }
        [Display(Name = "Total Area")]
        public string TotalArea { get; set; }
        [Display(Name = "Balcony Area")]
        public string BalconyArea { get; set; }
        //public string Layout { get; set; }
        //[Display(Name = "Payment Plan")]
        //public string PaymentPlan { get; set; }
        // public long? Price { get; set; }
        [Display(Name = "Parking")]
        public int? NoOfParking { get; set; }
        //[Display(Name = "DLD Fee")]
        public long? DLDFee { get; set; }
        // [Display(Name = "Okoud Fee")]
        // public long? OkoudFee { get; set; }
        public int? VAT { get; set; }
        // public long? Commissions { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Project Name")]
        public long ProjectId { get; set; }

        [Display(Name = "Unit Type")]
        public string UnitType { get; set; }
        // [Display(Name = "Unit Type")]
        // public long UnitTypeId { get; set; }
        [Display(Name = "Status")]
        public SalUnitStatusEnum UnitStatus { get; set; }
        [Display(Name = "Unit Rate")]
        public string UnitRate { get; set; }
        [Display(Name = "Unit Price")]
        public string UnitPrice { get; set; }
        public long? UnitStatusUpdatedBy { get; set; }
        public DateTime? UnitStatusUpdatedDate { get; set; }
        public string UnitIdData { get; set; }
        public long? AttachmentId { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public long? ProposalId { get; set; }
        public string LeadName { get; set; }
        public string PropertyConsultantName { get; set; }
        public string BrokerName { get; set; }
        [Display(Name = "Measurement Unit")]
        public SalMeasurementUnitEnum MeasurementUnit { get; set; }
    }
}

