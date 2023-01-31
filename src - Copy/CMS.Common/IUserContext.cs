using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMS.Common
{
    public interface IUserContext
    {

        HttpContext HttpContext { get; }
        string UserId { get; }
        // string CandidateId { get; }
        string UserUniqueId { get; }
        string Name { get; }
        string CompanyId { get; }
        string CompanyCode { get; }
        string CompanyName { get; }
        string Email { get; }
        string JobTitle { get; }
        string PhotoId { get; }
        //bool IsUser { get; }
        //bool IsAdmin { get; }
        bool IsSystemAdmin { get; }
        bool IsGuestUser { get; }
        string UserRoleIds { get; }

        string UserRoleCodes { get; }
        string PortalTheme { get; }
        string UserPortals { get; }
        string PortalId { get; }
        string PortalName { get; }

        // List<Claim> Claims { get; }
        LoggedInAsTypeEnum LoggedInAsType { get; }
        string LoggedInAsByUserId { get; }
        string LoggedInAsByUserName { get; }
        string LegalEntityId { get; }
        string LegalEntityCode { get; }
        string CultureName { get; }
        string OrganizationId { get; }
        string PositionId { get; }
        string PersonId { get; }
        TimeZoneInfo TimeZone { get; }
        string DateTimeFormat { get; }
        DateTime GetServerNow { get; }
        DateTime GetLocalNow { get; }
        DateTime GetLocalTime(DateTime serverTime);
        DateTime GetServerTime(DateTime localTime);


    }
}
