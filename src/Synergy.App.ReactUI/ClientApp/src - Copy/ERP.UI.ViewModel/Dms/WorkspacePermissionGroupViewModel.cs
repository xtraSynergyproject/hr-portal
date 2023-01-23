using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class WorkspacePermissionGroupViewModel:ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Users { get; set; }
        public long? RemoveUserId { get; set; }
        public int UsersCount { get; set; }
        public ICollection<string> UsersList { get; set; }
    }
}
