using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class BankViewModel : ViewModelBase
    {
        [Required]
        [Display(Name="Bank Name")]
        public string Name { get; set; }
    }
}
