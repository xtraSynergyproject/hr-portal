using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class MenuGroupViewModel : MenuGroup
    {      
        public string SubModule { get; set; }
        public string PortalName { get; set; }
        public string RequestSource { get; set; }
        public DataActionEnum DataAction { get; set; }
        public string ToPortalId { get; set; }
        public string NewName { get; set; }
        public string PageJson { get; set; }
    }
}
