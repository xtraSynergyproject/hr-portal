using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class PageMemberViewModel : PageMember
    {        
        public DataActionEnum DataAction { get; set; }
        public string MemberIds { get; set; }

    }
}
