using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class WebinarViewModel: ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name="TopBanner")]
        public long? TopBannerId { get; set; }
        public FileViewModel BannerTop { get; set; }
        [Display(Name = "LeftBanner")]
        public long? LeftBannerId { get; set; }
        public FileViewModel BannerLeft { get; set; }
        [Display(Name = "BottomLeftBanner")]
        public long? BottomBannerId { get; set; }
        public FileViewModel BottomBanner { get; set; }
        [Display(Name = "BottomRightBanner")]
        public long? BottomRightBannerId { get; set; }
        public FileViewModel BottomRightBanner { get; set; }
        public string RegisteredSuccessMessage { get; set; }
        [Required]
        [Display(Name = "NotificationTemplate")]
        public long NotificationTemplateId { get; set; }
    }

}
