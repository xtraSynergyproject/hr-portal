using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ResponsibilityCenterViewModel : ViewModelBase
    {      
        [Required]
        [StringLength(50)]
        [Display(Name = "Responsibility Center Code")]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Responsibility Center Name")]
        public string Name { get; set; }       

        [StringLength(2000)]
        public string Description { get; set; }

    }
}
