using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class Page : DataModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PageDetails { get; set; }
        public string Title { get; set; }
        public string MenuName { get; set; }
        public string ParentId { get; set; }
        public bool IsRootPage { get; set; }
        public TemplateTypeEnum PageType { get; set; }
       
        public bool HideInMenu { get; set; }
        public bool ShowOutsideMenuGroup { get; set; }
        public bool DontShowMenuInThisPage { get; set; }
        public bool AuthorizationNotRequired { get; set; }
        public bool ShowMenuWhenAuthorized { get; set; }
        public bool ShowMenuWhenNotAuthorized { get; set; }
        public string Layout { get; set; }

        [ForeignKey("Portal")]
        public string PortalId { get; set; }
        public Portal Portal { get; set; }

        [ForeignKey("MenuGroup")]
        public string MenuGroupId { get; set; }
        public MenuGroup MenuGroup { get; set; }
        //public bool ShowUnderMenuGroup { get; set; }
        public string Style { get; set; }
        public string Css { get; set; }
        public int Level { get; set; }
        public string IconCss { get; set; }
        public string IconColor { get; set; }
        public StatusEnum PageStatus { get; set; }
        public bool ShowTitleInPage { get; set; }



        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }
        public string JsonForm { get; set; }
        public bool EnableIndexPage { get; set; }
        public bool AllowIfPortalAccess { get; set; }
        public string IconFileId { get; set; }
        public bool EnableDynamicGridBinding { get; set; }

        public bool ExpandHelpPanel { get; set; }
        public bool IsPageRedirect { get; set; }
        public string GroupCode { get; set; }
        public bool DontShowInMainMenu { get; set; }

    }

    public class PagePublished : Page
    {
        public string PageId { get; set; }
        public string PublishedBy { get; set; }
        public string PublishedDate { get; set; }
        public int VersionNo { get; set; }
        public bool IsLatest { get; set; }
    }
}
