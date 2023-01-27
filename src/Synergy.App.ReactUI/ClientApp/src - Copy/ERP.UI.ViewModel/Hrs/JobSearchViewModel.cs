using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class JobSearchViewModel : DatedSearchViewModelBase
    {
        public long JobId { get; set; }

        [StringLength(200)]
        [Display(Name = "Grade Name")]
        public string Name { get; set; }

        [Display(Name = "Grade Name(Arabic)")]
        public string NameAr { get; set; }
    }
}
