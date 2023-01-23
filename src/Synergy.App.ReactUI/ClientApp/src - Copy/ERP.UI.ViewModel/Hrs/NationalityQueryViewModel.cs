using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class NationalityQueryViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Nationality Name")]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Nationality Name (In Arabic)")]
        public string NameAr { get; set; }

        [Required]
        [Display(Name = "Nationality Group")]
        public int NationalityGroupId { get; set; }

        [Display(Name = "Nationality Group")]
        public string NationalityGroupName { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int? SequenceNo { get; set; }

    }
}
