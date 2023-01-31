using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class HierarchyNameViewModel : BaseViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Hierarchy Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hierarchy Type")]
        public string HierarchyType { get; set; }

        public HierarchyType HierarchyTypeName { get; set; }

        public string Description { get; set; }

        public int? SequenceNo { get; set; }

    }
}
