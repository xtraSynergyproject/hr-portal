using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class UserEntityPermissionViewModel : UserEntityPermission
    {
        public string[] LegalEntityId1 { get; set; }
        public string[] OrganisationId { get; set; }
        public string[] TemplateId { get; set; }
        public string[] WorkspaceId { get; set; }
        public string PermissionIds { get; set; }
        public string UserId { get; set; }
        public string UserRolepermission { get; set; }
        public string Portal { get; set; }
        public string PageId { get; set; }
        public string Page { get; set; }
        public string ActionId { get; set; }
        public string Action { get; set; }
        public string[] Permissions { get; set; }

    }
}
