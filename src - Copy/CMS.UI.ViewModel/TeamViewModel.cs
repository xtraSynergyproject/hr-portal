using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TeamViewModel : Team
    {
     
        public DataActionEnum DataAction { get; set; }

        public List<string> UserIds { get; set; }
        public String TeamOwnerId { get; set; }
        public bool IsTeamOwner { get; set; }
        public string UserName { get; set; }
        
    }
}
