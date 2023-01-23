using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class PortalViewModel : Portal
    {
        public DataActionEnum DataAction { get; set; }
        public string LandingPage { get; set; }
        public string ThemeData { get; set; }
        public string PortalStatusData { get; set; }
        public string IconBgColorText
        {
            get
            {
                return IconBgColor.Coalesce("indigo");
            }

        }
        public string IconCssText
        {
            get
            {
                return IconCss.Coalesce("fa fa-browser");
            }

        }
        public string UserId { get; set; }
        public string CallBackMethod { get; set; }
    }
}
