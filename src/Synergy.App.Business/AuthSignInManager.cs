using Synergy.App.Common;
using Synergy.App.Business;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Synergy.App.ViewModel;
using Synergy.App.DataModel;

namespace Synergy.App.Business
{

    public class AuthSignInManager<T> : SignInManager<ApplicationIdentityUser> where T : ApplicationIdentityUser
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserBusiness _userBusiness;

        public AuthSignInManager(
                  UserManager<ApplicationIdentityUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationIdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationIdentityUser>> logger,
            IAuthenticationSchemeProvider schemeProvider,
                 IUserConfirmation<ApplicationIdentityUser> userConfirmation,
                 IUserBusiness userBusiness
            )
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, userConfirmation)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _userBusiness = userBusiness;

        }

        public override async Task SignInAsync(ApplicationIdentityUser user, bool isPersistent, string authenticationMethod = null)
        {
            await base.SignInAsync(user, isPersistent, authenticationMethod);
        }
        public override async Task<ApplicationIdentityUser> ValidateSecurityStampAsync(ClaimsPrincipal principal)
        {
            try
            {
                var email = ((ClaimsIdentity)principal.Identity).Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (email != null)
                {

                    var user = await _userBusiness.ValidateUser(email.Value);
                    if (user != null)
                    {
                        var identity = new ApplicationIdentityUser
                        {
                            Id = user.Id,
                            UserName = user.Name,
                            IsSystemAdmin = user.IsSystemAdmin,
                            Email = user.Email,
                            UserUniqueId = user.Email,
                            CompanyId = user.CompanyId,
                            CompanyCode = user.CompanyCode,
                            CompanyName = user.CompanyName,
                            JobTitle = user.JobTitle,
                            PhotoId = user.PhotoId,
                            UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                            UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                            PortalId = user.PortalId,
                            UserPortals = user.UserPortals,
                            IsGuestUser = user.IsGuestUser,
                            LegalEntityId = user.LegalEntityId,
                            LegalEntityCode = user.LegalEntityCode,
                            PersonId = user.PersonId,
                            PositionId = user.PositionId,
                            DepartmentId = user.DepartmentId,
                            PortalName = user.PortalName,
                        };
                        var portalId = ((ClaimsIdentity)principal.Identity).Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimaryGroupSid);

                        if (portalId != null && portalId.Value.IsNotNullAndNotEmpty())
                        {
                            var portal = await _userBusiness.GetSingle<PortalViewModel, Portal>(x => x.Id == portalId.Value);
                            if (portal != null)
                            {
                                identity.PortalId = portal.Id;
                                identity.PortalName = portal.Name;
                                identity.PortalTheme = portal.Theme.ToString();

                            }
                        }
                        identity.MapClaims();
                        return await Task.FromResult(identity);
                    }

                }

                return await Task.FromResult<ApplicationIdentityUser>(null);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public override async Task<SignInResult> PasswordSignInAsync(ApplicationIdentityUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            try
            {
                await SignInAsync(user, isPersistent, null);
                return SignInResult.Success;
            }
            catch (Exception ex)
            {

                return SignInResult.Failed;
            }

        }
        public override bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var isSignedIn = principal != null && principal.Identity != null && principal.Identity.IsAuthenticated;
            return isSignedIn;
        }
        public override async Task SignOutAsync()
        {
            await base.SignOutAsync();
        }
        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationIdentityUser user)
        {
            return await GenerateUserIdentityAsync(user);
            //return base.CreateUserPrincipalAsync(user);
        }

        public async Task<ClaimsPrincipal> GenerateUserIdentityAsync(ApplicationIdentityUser user)
        {

            var claimsIdentity = new List<ClaimsIdentity> { { new ClaimsIdentity(user.Claims, "ApplicationCookie") } };
            var cp = new ClaimsPrincipal(claimsIdentity);
            return await Task.FromResult(cp);
        }
    }


    public class ApplicationIdentityUser : IdentityUser
    {

        public virtual bool success { get; set; }
        public virtual string CompanyId { get; set; }
        public virtual string UserUniqueId { get; set; }
        public virtual string CompanyCode { get; set; }
        public virtual string JobTitle { get; set; }
        public virtual string PhotoId { get; set; }
        public virtual string UserRoleIds { get; set; }
        public virtual string UserPortals { get; set; }
        public virtual string UserRoleCodes { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual bool IsSystemAdmin { get; set; }
        public virtual bool IsGuestUser { get; set; }
        public virtual string PortalId { get; set; }
        public virtual string PortalName { get; set; }
        public virtual string PortalTheme { get; set; }
        public virtual List<Claim> Claims { get; set; }
        public virtual LoggedInAsTypeEnum? LoggedInAsType { get; set; }
        public virtual string LoggedInAsByUserId { get; set; }
        public virtual string LoggedInAsByUserName { get; set; }
        public virtual string CultureName { get; set; }
        public virtual string LegalEntityId { get; set; }
        public virtual string LegalEntityCode { get; set; }
        public virtual string PersonId { get; set; }
        public virtual string PositionId { get; set; }
        public virtual string DepartmentId { get; set; }

        public void MapClaims()
        {
            var claimCollection = new List<Claim>();
            if (!string.IsNullOrEmpty(Id))
            {
                claimCollection.Add(new Claim(ClaimTypes.Sid, Id));
            }
            if (!string.IsNullOrEmpty(UserUniqueId))
            {
                claimCollection.Add(new Claim(ClaimTypes.PrimarySid, UserUniqueId));
            }

            claimCollection.Add(new Claim(ClaimTypes.System, IsSystemAdmin.ToString()));

            if (!string.IsNullOrEmpty(UserName))
            {
                claimCollection.Add(new Claim(ClaimTypes.Name, UserName));
            }
            if (!string.IsNullOrEmpty(Email))
            {
                claimCollection.Add(new Claim(ClaimTypes.Email, Email));
            }
            if (!string.IsNullOrEmpty(CompanyId))
            {
                claimCollection.Add(new Claim(ClaimTypes.Country, CompanyId));
            }
            if (!string.IsNullOrEmpty(CompanyCode))
            {
                claimCollection.Add(new Claim(ClaimTypes.StateOrProvince, CompanyCode));
            }
            if (!string.IsNullOrEmpty(CompanyName))
            {
                claimCollection.Add(new Claim(ClaimTypes.PostalCode, CompanyName));
            }
            if (!string.IsNullOrEmpty(JobTitle))
            {
                claimCollection.Add(new Claim(ClaimTypes.Actor, JobTitle));
            }
            if (!string.IsNullOrEmpty(PhotoId))
            {
                claimCollection.Add(new Claim(ClaimTypes.NameIdentifier, PhotoId));
            }
            if (!string.IsNullOrEmpty(UserRoleIds))
            {
                claimCollection.Add(new Claim(ClaimTypes.Authentication, UserRoleIds));
            }
            if (!string.IsNullOrEmpty(UserRoleCodes))
            {
                claimCollection.Add(new Claim(ClaimTypes.AuthenticationInstant, UserRoleCodes));
            }

            claimCollection.Add(new Claim(ClaimTypes.Anonymous, IsGuestUser.ToString()));

            if (!string.IsNullOrEmpty(PortalId))
            {
                claimCollection.Add(new Claim(ClaimTypes.PrimaryGroupSid, PortalId));
            }
            if (!string.IsNullOrEmpty(PortalTheme))
            {
                claimCollection.Add(new Claim(ClaimTypes.GivenName, PortalTheme));
            }
            if (!string.IsNullOrEmpty(UserPortals))
            {
                claimCollection.Add(new Claim(ClaimTypes.AuthorizationDecision, UserPortals));
            }
            if (LoggedInAsType != null)
            {
                claimCollection.Add(new Claim(ClaimTypes.Locality, LoggedInAsType.ToString()));
            }
            if (!string.IsNullOrEmpty(LoggedInAsByUserId))
            {
                claimCollection.Add(new Claim(ClaimTypes.HomePhone, LoggedInAsByUserId));
            }
            if (!string.IsNullOrEmpty(LoggedInAsByUserName))
            {
                claimCollection.Add(new Claim(ClaimTypes.Surname, LoggedInAsByUserName));
            }
            if (!string.IsNullOrEmpty(CultureName))
            {
                claimCollection.Add(new Claim(ClaimTypes.WindowsDeviceClaim, CultureName));
            }
            else
            {
                claimCollection.Add(new Claim(ClaimTypes.WindowsDeviceClaim, "en-US"));
            }
            if (!string.IsNullOrEmpty(LegalEntityId))
            {
                claimCollection.Add(new Claim(ClaimTypes.Uri, LegalEntityId));
            }
            if (!string.IsNullOrEmpty(LegalEntityCode))
            {
                claimCollection.Add(new Claim(ClaimTypes.Dsa, LegalEntityCode));
            }
            if (!string.IsNullOrEmpty(PersonId))
            {
                claimCollection.Add(new Claim(ClaimTypes.DenyOnlyPrimarySid, PersonId));
            }
            if (!string.IsNullOrEmpty(PositionId))
            {
                claimCollection.Add(new Claim(ClaimTypes.DenyOnlySid, PositionId));
            }
            if (!string.IsNullOrEmpty(DepartmentId))
            {
                claimCollection.Add(new Claim(ClaimTypes.DenyOnlyPrimaryGroupSid, DepartmentId));
            }
            if (!string.IsNullOrEmpty(PortalName))
            {
                claimCollection.Add(new Claim(ClaimTypes.Spn, PortalName));
            }
            Claims = claimCollection;
        }
    }

}