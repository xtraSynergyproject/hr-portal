using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class MemberGroupViewModel : MemberGroup
    {
       public DataActionEnum DataAction { get; set; }
       public bool Status { get; set; }
    }
}
