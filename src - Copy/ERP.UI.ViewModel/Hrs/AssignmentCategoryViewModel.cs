using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class AssignmentCategoryViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Assignment Category Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? SequenceNo { get; set; }

    }
}
