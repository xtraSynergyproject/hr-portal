using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class MemberViewModel : Member
    {
       
        public string Email { get; set; }
        public DataActionEnum DataAction { get; set; }
        public string Status { get; set; }

    }
}
