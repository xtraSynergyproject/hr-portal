using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model

{
    public class Portal : DataModelBase
    {
        public string Name { get; set; }
        public ThemeEnum Theme { get; set; }
        public string DomainName { get; set; }
        public string ParentId { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public PortalStatusEnum PortalStatus { get; set; }
        public string LogoId { get; set; }
        public string BannerId { get; set; }
        public int BannerHeight { get; set; }
        public string FavIconId { get; set; }


        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string TopBannerBackColor { get; set; }
        public string TopBannerForeColor { get; set; }
        public string LeftMenuBackColor { get; set; }
        public string LeftMenuForeColor { get; set; }
        public bool EnableBreadcrumb { get; set; }
        public bool EnableMultiLanguage { get; set; }
        public string PortalFooterTeext { get; set; }
        public bool EnableLegalEntity { get; set; }
        public bool EnableAccordianMenu { get; set; }
        public string UserGuideId { get; set; }
        public string[] AllowedLanguageIds { get; set; }
        public string Layout { get; set; }

    }
    [Table("PortalLog", Schema = "log")]
    public class PortalLog : Portal
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }
}
