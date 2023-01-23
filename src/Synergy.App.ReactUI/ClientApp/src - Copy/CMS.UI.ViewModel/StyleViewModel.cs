using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
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
