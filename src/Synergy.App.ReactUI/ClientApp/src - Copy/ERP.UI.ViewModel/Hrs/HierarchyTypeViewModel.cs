using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class HierarchyTypeViewModel : BaseViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Hierarchy Type Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? SequenceNo { get; set; }

    }
}
