using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SalesReportsViewModel : ViewModelBase
    {
        [Display(Name = "Property Consultant")]
        public string PropertyConsultant { get; set; }
        [Display(Name = "Total Unit Sale")]
        public long? TotalUnitSale { get; set; }
        [Display(Name = "Total DP Collected")]
        public long? TotalDPCollected { get; set; }
        [Display(Name = "Total Commission")]
        public string TotalCommission { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [DateCompare("StartDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
    }
}

