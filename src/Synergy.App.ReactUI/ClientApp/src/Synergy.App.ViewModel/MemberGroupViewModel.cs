using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class MemberGroupViewModel : MemberGroup
    {
       public DataActionEnum DataAction { get; set; }
       public bool Status { get; set; }
    }
}
