using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class PortalViewModel : Portal
    {
        public DataActionEnum DataAction { get; set; }
        public string LandingPage { get; set; }
        public string ThemeData { get; set; }
        public string PortalStatusData { get; set; }
    }
}
