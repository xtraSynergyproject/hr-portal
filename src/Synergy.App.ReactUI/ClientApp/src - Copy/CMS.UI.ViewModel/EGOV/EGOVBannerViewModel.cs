using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class EGOVBannerViewModel
    {
        public string BannerImageId { get; set; }
        public string BannerContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? SequenceNo { get; set; }
        public string UrlLink { get; set; }

    }
}
