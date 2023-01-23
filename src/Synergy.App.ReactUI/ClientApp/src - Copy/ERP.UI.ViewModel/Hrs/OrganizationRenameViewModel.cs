using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class OrganizationRenameViewModel : BaseRequestViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Organization Name")]
        public string FullName { get; set; }

        [Display(Name = "Reporting Name")]
        public string Name { get; set; }

        [Display(Name = "Organization Name (Arabic)")]
        public string NameAr { get; set; }

        [Required]
        [Display(Name = "Proposed Organization Name")]

        public string ProposedFullName { get; set; }
        [Required]
        [Display(Name = "Proposed Organization Name (Arabic)")]
        public string ProposedNameAr { get; set; }


        public string Remarks { get; set; }

        public int HierarchyNameId { get; set; }

        public DateTime AsOnDate { get; set; }

    }
}
