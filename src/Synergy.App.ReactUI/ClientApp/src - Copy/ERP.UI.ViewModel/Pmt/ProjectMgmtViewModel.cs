using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class ProjectMgmtViewModel : ServiceViewModel
    {
        [Display(Name = "Project Type")]
        public ProjectTypeEnum? ProjectType { get; set; }
        [Display(Name = "Upload Logo")]
        public long? UploadLogoId { get; set; }
        [Display(Name = "Country")]
        public long? CountryId { get; set; }
        [Display(Name = "Country")]
        public string CountryName { get; set; }
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }
        [Display(Name = "City")]
        public long? CityId { get; set; }
        [Display(Name = "City")]
        public string CityName { get; set; }
    }
}
