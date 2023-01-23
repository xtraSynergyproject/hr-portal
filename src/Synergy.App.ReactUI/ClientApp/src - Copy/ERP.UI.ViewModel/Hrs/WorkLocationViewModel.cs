using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class WorkLocationViewModel : BaseViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Work Location Name")]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(2000)]
        public virtual string Address { get; set; }

        public int? SequenceNo { get; set; }

    }
}
