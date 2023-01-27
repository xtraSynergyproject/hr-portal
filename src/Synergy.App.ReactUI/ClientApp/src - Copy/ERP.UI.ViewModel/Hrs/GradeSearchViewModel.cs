using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class GradeSearchViewModel : DatedSearchViewModelBase
    {

        public long GradeId { get; set; }

        [StringLength(200)]
        [Display(Name = "Grade Name")]
        public string Name { get; set; }

        [Display(Name = "Grade Name(Arabic)")]
        public string NameLocal { get; set; }
       
    }
}
