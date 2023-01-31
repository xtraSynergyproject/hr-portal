using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class PageMemberViewModel : PageMember
    {        
        public DataActionEnum DataAction { get; set; }
        public string MemberIds { get; set; }

    }
}
