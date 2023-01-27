using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class StyleViewModel : Style
    {
        public DataActionEnum DataAction { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string PageMenuName { get; set; }
        public bool PageHideInMenu { get; set; }
        public bool PageIsRootPage { get; set; }
    }
}
