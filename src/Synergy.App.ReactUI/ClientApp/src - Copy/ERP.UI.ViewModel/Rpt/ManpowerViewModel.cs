using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ManpowerViewModel 
    {
        public long? HeadCount { get; set; }
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime ToDate { get; set; }
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime FromDate { get; set; }
        [Display(Name = "Organisation Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Organisation Name")]
        public string OrganizationName { get; set; }
        [Display(Name = "Brand Name")]
        public long? BrandId { get; set; }
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }
        public long? HeadAsJoinedCount { get; set; }
        public long? HeadAsLeftCount { get; set; }
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        public long? LengthOfService { get; set; }

    }
}
