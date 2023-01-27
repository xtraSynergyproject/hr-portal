using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class CountryViewModel : ViewModelBase
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string NameLocal { get; set; }
        [Display(Name="Dail Code")]
        [Required]
        public string CountryDialCode { get; set; }
      
        public Nullable<long> SequenceNo { get; set; }

    }
}
