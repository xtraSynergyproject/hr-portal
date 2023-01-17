using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class UnitTypeViewModel : ViewModelBase
    {
        
        [Required]
        [Display(Name ="Unit Type Name")]
        public string Name { get; set; }        
      
        [Display(Name = "Description")]
        public string Description { get; set; }
       

      
    }
}

