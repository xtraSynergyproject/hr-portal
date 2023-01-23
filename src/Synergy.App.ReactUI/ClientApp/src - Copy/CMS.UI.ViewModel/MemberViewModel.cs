using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class MemberViewModel : Member
    {
       
        public string Email { get; set; }
        public DataActionEnum DataAction { get; set; }
        public string Status { get; set; }

    }
}
