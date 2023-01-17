using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class TeamViewModel : Team
    {    
        
        public List<string> UserIds { get; set; }
        public string TeamOwnerId { get; set; }
        public bool IsTeamOwner { get; set; }
        public string UserName { get; set; }
        public string TeamOwnerName { get; set; }
        public int UsersCount { get; set; }
        public string TeamCreatedDate { get; set; }
        public string PhotoId { get; set; } 
        public List<UserViewModel> Users { get; set; }
        public string JobTitle { get; set; }
    }
}
