using System;

namespace CMS.UI.Web
{
    public class SuccessViewModel
    {
        public string Message { get; set; }
        public bool EnableReloadParent { get; set; }
        public bool EnableBackButton { get; set; }
        public string BackButtonUrl { get; set; }
        public string BackButtonText { get; set; }

        public bool EnableCreateNewButton { get; set; }
        public string CreateNewButtonUrl { get; set; }
        public string CreateNewButtonText { get; set; }
    }
}
