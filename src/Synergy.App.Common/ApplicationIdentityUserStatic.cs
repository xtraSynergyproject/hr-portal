using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Common
{
    public class ApplicationIdentityUserStatic
    {
        public static string UserName { get; set; }
        public static string Email { get; set; }
        public static string Id { get; set; }
        public static string CompanyId { get; set; }
        public static string CompanyCode { get; set; }
        public static string UserUniqueId { get; set; }
        public static string JobTitle { get; set; }
        public static string PhotoId { get; set; }
        public static string UserRoleIds { get; set; }
        public static string UserPortals { get; set; }
        public static string UserRoleCodes { get; set; }
        public static string CompanyName { get; set; }
        public static bool IsSystemAdmin { get; set; }
        public static bool IsGuestUser { get; set; }
        public static string PortalId { get; set; }
        public static string PortalName { get; set; }
        public static string PortalTheme { get; set; }
        public static LoggedInAsTypeEnum? LoggedInAsType { get; set; }
        public static string LoggedInAsByUserId { get; set; }
        public static string LoggedInAsByUserName { get; set; }
        public static string CultureName { get; set; }
        public static string LegalEntityId { get; set; }
        public static string LegalEntityCode { get; set; }
        public static string PersonId { get; set; }
        public static string PositionId { get; set; }
        public static string DepartmentId { get; set; }

    }
}
