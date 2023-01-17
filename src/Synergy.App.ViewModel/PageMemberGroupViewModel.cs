using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class PageMemberGroupViewModel : PageMemberGroup
    {        
        public DataActionEnum DataAction { get; set; }

        public string MemberGroupIds { get; set; }

    }
}
