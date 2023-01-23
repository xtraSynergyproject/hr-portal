using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class WebinarUserGroupViewModel: ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebinarUserId { get; set; }
        public long? RemoveUserId { get; set; }
        public string Users { get; set; }
        public int UsersCount { get; set; }
        public ICollection<string> UsersList { get; set; }

    }

}
