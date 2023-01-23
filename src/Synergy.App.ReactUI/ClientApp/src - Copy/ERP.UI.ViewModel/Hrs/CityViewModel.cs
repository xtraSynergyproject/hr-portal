using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class CityViewModel : ViewModelBase
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string NameLocal { get; set; }
        [Required]
        [Display(Name = "Country")]
        public long? CountryId { get; set; }
        [Display(Name = "Country")]
        public string CountryName { get; set; }
    }
}
