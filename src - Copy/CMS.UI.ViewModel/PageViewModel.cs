using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class PageViewModel : Page
    {
        public string state { get; set; }
        //public List<PageContentViewModel> Groups { get; set; }
        //public List<PageContentViewModel> Columns { get; set; }
        //public List<PageContentViewModel> Cells { get; set; }
        //public List<PageContentViewModel> Components { get; set; }
        public bool IsLatest { get; set; }
        // public string PageId { get; set; }
        public string ModuleId { get; set; }
       // public string ParentId { get; set; }
        public string SubModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string PortalName { get; set; }
        public RunningModeEnum RunningMode { get; set; }
        public RequestSourceEnum RequestSource { get; set; }
        public string RequestUrl { get; set; }
        public string RecordId { get; set; }
        public string CustomUrl { get; set; }
        public string UserPermissionId { get; set; }
        public string UserPortalId { get; set; }
        public string MenuGroupName { get; set; }
        public string MenuGroupShortName { get; set; }
        public string MenuGroupIconFileId { get; set; }
        public string MenuGroupDetails { get; set; }
        public string MenuGroupIconCss { get; set; }
        
        public long? MenuGroupSequenceOrder { get; set; }
        public List<BreadcrumbViewModel> Breadcrumbs { get; set; }
        public List<MenuViewModel> Menus { get; set; }


        //public UserPermissionViewModel UserPagePermission { get; set; }

        public string[] Permissions { get; set; }
        public string PermissionsText { get; set; }
        public bool IsAuthorized
        {
            get
            {
                return IsSystemAdmin ||
                    (AllowIfPortalAccess && UserPortalId.IsNotNullAndNotEmpty()) ||
                            (UserPermissionId.IsNotNullAndNotEmpty() && Permissions.Length > 0) ||
                              (AuthorizationNotRequired);

            }
        }

        public bool IsPopup { get; set; }
        public bool IsSystemAdmin { get; set; }
        public string ReturnUrl { get; set; }
        public LayoutModeEnum? LayoutMode { get; set; }
        public string PopupCallbackMethod { get; set; }
        public string Type { get; set; }
        public string TemplateName { get; set; }
        public TemplateTypeEnum TemplateType { get; set; }
        public string UdfTemplateId { get; set; }
        public string UdfTemplateName { get; set; } 
        public string UdfTableMetadataId { get; set; }
        public string UdfTableMetadataName { get; set; }
        public string TemplateTableMetadataId { get; set; }
        public string TemplateTableMetadataName { get; set; }
        public string TemplateCodes { get; set; }
        public NtsViewTypeEnum? ViewType { get; set; }
        public NtsViewTypeEnum? ViewMode { get; set; }

        public string PageTypeName { get {
                return PageType.ToString();
            
            } }
        public string PagePortalId { get; set; }
        public string key { get; set; }
        public string title { get; set; }
        public bool lazy { get; set; }
    }

}
