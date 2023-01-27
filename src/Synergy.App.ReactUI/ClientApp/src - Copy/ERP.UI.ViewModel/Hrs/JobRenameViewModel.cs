using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class JobRenameViewModel : BaseRequestViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        public string Name { get; set; }

        [Display(Name = "Job Name (Arabic)")]
        public string NameAr { get; set; }

        [Required]
        [Display(Name = "Proposed Job Name")]
        public string ProposedFullName { get; set; }

        [Required]
        [Display(Name = "Proposed Job Name (Arabic)")]
        public string ProposedNameAr { get; set; }


        public string Remarks { get; set; }

        public int HierarchyNameId { get; set; }

        public DateTime AsOnDate { get; set; }

    }
}

