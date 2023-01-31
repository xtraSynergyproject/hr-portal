using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class MenuGroupViewModel : MenuGroup
    {      
        public string SubModule { get; set; }
        public string PortalName { get; set; }
        public string RequestSource { get; set; }
        public DataActionEnum DataAction { get; set; }
    }
}
