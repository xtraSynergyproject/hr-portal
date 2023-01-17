using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class EGovLandingPageViewModel
    {
        public List<EGOVBannerViewModel> SilderBanner { get; set; }
        public List<EGOVBannerViewModel> ProjectCompleted { get; set; }
        public List<EGOVBannerViewModel> ProjectOnGoing { get; set; }
        public List<EGOVBannerViewModel> ProjectUpComing { get; set; }
        public List<EGOVBannerViewModel> LatestNews { get; set; }
        public List<EGOVBannerViewModel> Notifications { get; set; }
        public List<EGOVBannerViewModel> CorporatePhotos { get; set; }
        public List<EGOVBannerViewModel> Tenders { get; set; }
        public List<EGOVBannerViewModel> OtherWebsites { get; set; }
        public List<TemplateViewModel> Templates { get; set; }
    }
}
