using ERP.Utility;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class OrganizationDocumentViewModel : DatedViewModelBase
    {
        [Display(Name = "Department Id")]
        public long OrganizationDocumentId { get; set; }
        [Required]
        [Display(Name = "Document Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        
        [Display(Name = "Document Upload")]
        public long? DocumentId { get; set; }
        public long OwnerId { get; set; }
        [Required]
        [Display(Name = "Document Upload")]
        public long? FileId { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

        public FileViewModel SelectedFile { get; set; }
        [Display(Name = "File")]
        public long DocumentFileId { get; set; }
        public string Users { get; set; }

        public IList<OrganizationDocumentPermissionViewModel> PermissionList {get; set;}


        [Display(Name = "All Users Can View")]
        public bool AllCanView { get; set; }

        public bool CanEdit { get; set; }


    }
}
