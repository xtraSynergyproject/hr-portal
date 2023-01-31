using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class PageMemberGroupViewModel : PageMemberGroup
    {        
        public DataActionEnum DataAction { get; set; }

        public string MemberGroupIds { get; set; }

    }
}
