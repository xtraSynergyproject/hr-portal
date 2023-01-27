using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class CurrencyViewModel : ViewModelBase
    {

        public long? CurrencyId { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Currency Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Currency Code")]
        public string Code { get; set; }
        [Required]
        [Display(Name = "Country")]
        public long? CountryId { get; set; }
        [Display(Name = "Country")]
        public string CountryName { get; set; }
        public string CodeAr { get; set; }
        public string NameAr { get; set; }
        public string NameBeforePoint { get; set; }
        public string NameBeforePointAr { get; set; }
        public string NameAfterPoint { get; set; }
        public string NameAfterPointAr { get; set; }
    }
}
