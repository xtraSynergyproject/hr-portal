
using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class UserHierarchyPermissionViewModel : ViewModelBase
    {
        public long UserHierarchyPermissionId { get; set; }
        public long UserId { get; set; }
        public long HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public HierarchyPermissionEnum HierarchyPermission { get; set; }
        public string UserName { get; set; }
        [Display(Name ="Custom Permission")]
        public long? CustomPermissionId { get; set; }

    }
}


