using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class OrganizationDocumentPermissionViewModel : ViewModelBase
    {
        public long OrganizationDocumentPermissionId { get; set; }
        public long OrganizationDocumentId { get; set; }
        public long UserId { get; set; }
        [Display (Name ="User Name")]
        public string UserName { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
    }
}
