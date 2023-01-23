using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERP.UI.ViewModel
{
    public class DatedSearchViewModelBase : SearchViewModelBase
    {
       // [Required]
        [StringLength(20)]
        [Display(Name = "Effective As-of Date")]
        public DateTime? EffectiveAsOfDate { get; set; }
    }
}
