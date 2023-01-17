using CMS.Common;
using CMS.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class UserEntityPermissionViewModel : UserEntityPermission
    {
        public string[] LegalEntityId1 { get; set; }
        public string[] OrganisationId { get; set; }
        public string[] TemplateId { get; set; }
        public string[] WorkspaceId { get; set; }
        public string PermissionIds { get; set; }
    }
}
