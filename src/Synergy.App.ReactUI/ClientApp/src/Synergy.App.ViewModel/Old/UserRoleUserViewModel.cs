using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using Synergy.App.Common;
namespace Synergy.App.ViewModel
{
    public class UserRoleUserViewModel : UserRoleUser
    {
        public string UserName { get; set; }
        //public string[] Columns { get; set; }
        
        public IList<UserViewModel> UserList { get; set; }
        public IList<UserRoleViewModel> UserRoleList { get; set; }
        public IList<UserRoleUserViewModel> UserRoleUserList { get; set; }
        public string PhotoId { get; set; }
        public bool IsChecked {get;set;}
    }    
}
