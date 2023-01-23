using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ProjectDetailsViewModel
    {
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Unit Type")]
        public string UnitType { get; set; }
        [Display(Name = "Unit Number")]
        public string UnitName { get; set; }
        [Display(Name = "Unit Price")]
        public string UnitPrice { get; set; }
        [Display(Name = "Down Payment")]
        public string DownPayment { get; set; }
        [Display(Name = "Bedrooms")]
        public string NoOfBedRooms { get; set; }
        [Display(Name = "Floor Number")]
        public string FloorNumber { get; set; }
        public string Parking { get; set; }
        public string Status { get; set; }
        public string CountryId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyId { get; set; }
        [Display(Name = "Unit Rate")]
        public string UnitRate { get; set; }
        [Display(Name = "Unit Area")]
        public string UnitArea { get; set; }
        [Display(Name = "Balcony Area")]
        public string BalconyArea { get; set; }
        [Display(Name = "Total Area")]
        public string TotalArea { get; set; }
        [Display(Name = "DLD Fee")]
        public string DldFee { get; set; }
        [Display(Name = "Total Price")]
        public string TotalPrice { get; set; }
      
        public SalMeasurementUnitEnum MeasurementUnit { get; set; }

    }
}
