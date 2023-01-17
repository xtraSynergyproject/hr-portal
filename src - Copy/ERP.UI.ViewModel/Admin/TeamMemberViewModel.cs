using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TeamMemberViewModel : ViewModelBase
    {
       // public long TeamMemberId { get; set; }
        public long TeamId { get; set; }
        public long MemberUserId { get; set; }
        public bool IsTeamOwner { get; set; }
        public string MemberName { get; set; }
        public string Code { get; set; }
        public long? PhotoId { get; set; }
    }
}


