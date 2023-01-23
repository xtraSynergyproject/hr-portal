using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class DMSUserReportViewModel : ViewModelBase
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string UserName { get; set; }

        [DisplayFormat(DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? LastLoggedDate { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public string UserRole { get; set; }
        public string MobileNo { get; set; }
        public string[] Workspaces { get; set; }
        public string WorkSpace
        {
            get
            {
                var str = "";
                if (Workspaces != null)
                    str = string.Join(",", Workspaces);
                return str;
            }
        }

        public string[] UserRoles { get; set; }
        public string UserRole
        {
            get
            {
                var str = "";
                if (UserRoles != null)
                    str = string.Join(",", UserRoles);
                return str;
            }
        }

        public string[] LegalEntities { get; set; }
        public string LegalEntity
        {
            get
            {
                var str = "";
                if (LegalEntities != null)
                    str = string.Join(",", LegalEntities);
                return str;
            }
        }

        public string[] WorkspacePermissionGroups { get; set; }
        public string WorkspacePermissionGroup
        {
            get
            {
                var str = "";
                if (WorkspacePermissionGroups != null)
                    str = string.Join(",", WorkspacePermissionGroups);
                return str;
            }
        }
    }
}
