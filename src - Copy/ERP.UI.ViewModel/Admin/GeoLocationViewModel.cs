using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class GeoLocationViewModel :  ViewModelBase
    {
        [Required]
        [Display(Name = "Location Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }
        [Required]
        [Display(Name = "Longtitude")]
        public double Longtitude { get; set; }

       

    }
}
