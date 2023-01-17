using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class MenuViewModel : DataModelBase
    {
        public string ParentId { get; set; }
        public string MenuDetails { get; set; }
        public List<PageDetailsViewModel> MenuDetailSlides { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string IconCss { get; set; }
        public string IconFileId { get; set; }
        public string IconColor { get; set; }
        public string Url { get; set; }
        public string Layout { get; set; }
        public bool IsRoot { get; set; }
        public bool IsHidden { get; set; }
        public bool HasChild { get; set; }
        public TemplateTypeEnum PageType { get; set; }

        public bool HideInMenu { get; set; }
        public bool ShowOutsideMenuGroup { get; set; }
        public bool DontShowMenuInThisPage { get; set; }
        public bool AuthorizationNotRequired { get; set; }
        public bool ShowMenuWhenAuthorized { get; set; }
        public bool ShowMenuWhenNotAuthorized { get; set; }
        public string[] Permissions { get; set; }
        public string UserPermissionId { get; set; }
        public string MenuGroupName { get; set; }
        public string MenuGroupShortName { get; set; }
        public long? MenuGroupSequenceOrder { get; set; }
        public string Type { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public bool IsPageRedirect { get; set; }
    }
    public class MenuParam
    {
        public string Html { get; set; }
        public int Height { get; set; }
        public int ColumnCount { get; set; }
    }
}
