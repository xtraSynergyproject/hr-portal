


using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SectionViewModel : ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

        public long? ParentSectionId { get; set; }
    }
}
