using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;

namespace Synergy.App.Common
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // private readonly AuthenticationStateProvider _sessionProvider;
        //  IAuthorizationService _authservice;
        // public UserContext(IHttpContextAccessor httpContextAccessor)
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            // _sessionProvider = sessionProvider;

            // _authservice = null;
        }

        public string UserId
        {
            get
            {

                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Sid))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.Id;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.Id;
                }
            }
        }

        private bool CheckInHttpContextItems()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Items != null
                && _httpContextAccessor.HttpContext.Items.ContainsKey("UserId"))
            {
                return true;
            }
            return false;
        }

        public string UserUniqueId
        {
            get
            {

                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.PrimarySid))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.PrimarySid).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserUniqueId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.UserUniqueId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserUniqueId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.UserUniqueId;
                }
            }
        }
        public HttpContext HttpContext
        {
            get
            {
                return _httpContextAccessor.HttpContext;
            }
        }
        public string Email
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Email))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["Email"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.Email;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["Email"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.Email;
                }

            }
        }
        public string Name
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Name))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["Name"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.UserName;
                    }

                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["Name"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.UserName;
                }
            }
        }
        public string JobTitle
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Actor))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Actor).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["JobTitle"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.JobTitle;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["JobTitle"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.JobTitle;
                }
            }
        }
        public string PhotoId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["PhotoId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.PhotoId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["PhotoId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.PhotoId;
                }
            }
        }

        public string CompanyId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Country))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Country).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["CompanyId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.CompanyId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["CompanyId"]);
                }
                else if (ApplicationIdentityUserStatic.CompanyId.IsNotNullAndNotEmpty())
                {
                    return ApplicationIdentityUserStatic.CompanyId;
                }
                return "5e44cd63-68f0-41f2-b708-0eb3bf9f4a72";
            }
        }

        public bool IsSystemAdmin
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.System))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.System).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val.ToSafeBool();
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["IsSystemAdmin"]).ToSafeBool();
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.IsSystemAdmin;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToBoolean(_httpContextAccessor.HttpContext.Items["IsSystemAdmin"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.IsSystemAdmin;
                }
            }
        }

        public bool IsGuestUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Anonymous))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Anonymous).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val.ToSafeBool();
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["IsGuestUser"]).ToSafeBool();
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.IsGuestUser;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToBoolean(_httpContextAccessor.HttpContext.Items["IsGuestUser"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.IsGuestUser;
                }
            }
        }

        public string CompanyCode
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.StateOrProvince))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.StateOrProvince).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["CompanyCode"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.CompanyCode;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["CompanyCode"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.CompanyCode;
                }
            }
        }
        public string CompanyName
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.PostalCode))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.PostalCode).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["CompanyName"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.CompanyName;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["CompanyName"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.CompanyName;
                }
            }
        }
        public string UserRoleIds
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Authentication))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Authentication).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserRoleIds"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.UserRoleIds;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserRoleIds"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.UserRoleIds;
                }
            }
        }
        public string UserRoleCodes
        {
            get
            {

                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.AuthenticationInstant))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.AuthenticationInstant).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserRoleCodes"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.UserRoleCodes;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserRoleCodes"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.UserRoleCodes;
                }
            }
        }
        public string PortalTheme
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.GivenName))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["PortalTheme"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.PortalTheme;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["PortalTheme"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.PortalTheme;
                }
            }
        }
        public string PortalId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.PrimaryGroupSid))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.PrimaryGroupSid).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["PortalId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.PortalId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["PortalId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.PortalId;
                }

            }
        }
        public string UserPortals
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.AuthorizationDecision))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.AuthorizationDecision).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserPortals"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.UserPortals;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["UserPortals"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.UserPortals;
                }

            }
        }

        public DateTime CurrentDate
        {
            get
            {

                return DateTime.Today;

            }
        }

        public DateTime CurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }
        public LoggedInAsTypeEnum LoggedInAsType
        {

            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Locality))
                {
                    return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Locality).Value.ToEnum<LoggedInAsTypeEnum>();

                }
                return LoggedInAsTypeEnum.LoginCredential;

            }


        }
        public string LoggedInAsByUserId
        {

            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.HomePhone))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.HomePhone).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["LoggedInAsByUserId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.LoggedInAsByUserId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["LoggedInAsByUserId"]);
                }
                else if (ApplicationIdentityUserStatic.LoggedInAsByUserId.IsNotNullAndNotEmpty())
                {
                    return ApplicationIdentityUserStatic.LoggedInAsByUserId;
                }
                return UserId;

            }


        }
        public string LoggedInAsByUserName
        {

            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Surname))
                {
                    return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Surname).Value;
                }
                return Name;

            }


        }

        public string LegalEntityId
        {

            get
            {

                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Uri))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Uri).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["LegalEntityId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.LegalEntityId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["LegalEntityId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.LegalEntityId;
                }

            }


        }
        public string LegalEntityCode
        {

            get
            {

                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Dsa))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Dsa).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["LegalEntityCode"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.LegalEntityCode;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["LegalEntityCode"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.LegalEntityCode;
                }

            }


        }
        public string CultureName
        {

            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.WindowsDeviceClaim))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.WindowsDeviceClaim).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["CultureName"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.CultureName;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["CultureName"]);
                }
                else if (ApplicationIdentityUserStatic.CultureName.IsNotNullAndNotEmpty())
                {
                    return ApplicationIdentityUserStatic.CultureName;
                }
                return "en-US";
            }
        }
        public string PositionId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.DenyOnlySid))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.DenyOnlySid).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["PositionId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.PositionId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["PositionId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.PositionId;
                }
            }
        }
        public string PortalName
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Spn))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Spn).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["PortalName"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.PortalName;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["PortalName"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.PortalName;
                }
            }
        }
        public string OrganizationId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.DenyOnlyPrimaryGroupSid))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.DenyOnlyPrimaryGroupSid).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["OrganizationId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.DepartmentId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["OrganizationId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.DepartmentId;
                }
            }
        }
        public string PersonId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.DenyOnlyPrimarySid))
                {
                    var val = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.DenyOnlyPrimarySid).Value;
                    if (val.IsNotNullAndNotEmpty())
                    {
                        return val;
                    }
                    else if (CheckInHttpContextItems())
                    {
                        return Convert.ToString(_httpContextAccessor.HttpContext.Items["PersonId"]);
                    }
                    else
                    {
                        return ApplicationIdentityUserStatic.PersonId;
                    }
                }
                else if (CheckInHttpContextItems())
                {
                    return Convert.ToString(_httpContextAccessor.HttpContext.Items["PersonId"]);
                }
                else
                {
                    return ApplicationIdentityUserStatic.PersonId;
                }
            }
        }
        public TimeZoneInfo TimeZone
        {

            get
            {

                return TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
        }
        public DateTime GetLocalTime(DateTime serverTime)
        {
            return TimeZoneInfo.ConvertTime(serverTime, TimeZone);
        }

        public DateTime GetServerTime(DateTime localTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZone);
        }
        public DateTime GetServerNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
        public DateTime GetLocalNow
        {
            get
            {
                return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZone);
            }
        }

        public string DateTimeFormat
        {

            get
            {

                return "dd/MMM/yyyy";
            }
        }
        public string DateFormat
        {

            get
            {

                return "dd/MMM/yyyy";
            }
        }
        public string TimeFormat
        {

            get
            {

                return "hh mm";
            }
        }
    }
}
