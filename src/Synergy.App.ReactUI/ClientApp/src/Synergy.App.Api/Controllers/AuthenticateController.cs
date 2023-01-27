using Synergy.App.Business;
using Synergy.App.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.ViewModel;
using Synergy.App.DataModel;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Synergy.App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IPortalBusiness _portalBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IApplicationAccessBusiness _applicationAccessBusiness;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IUserRoleUserBusiness _userRoleUserBusiness;

        public AuthenticateController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IConfiguration configuration
            , IUserBusiness userBusiness
            , IUserContext userContext, INotificationBusiness notificationBusiness
            , IPortalBusiness portalBusiness, IApplicationAccessBusiness applicationAccessBusiness,
            IUserRoleBusiness userRoleBusiness, IUserRoleUserBusiness userRoleUserBusiness)
        {
            _customUserManager = customUserManager;
            _configuration = configuration;
            _userBusiness = userBusiness;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
            _applicationAccessBusiness = applicationAccessBusiness;
            _notificationBusiness = notificationBusiness;
            _userRoleBusiness = userRoleBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var user = await _userBusiness.ValidateLogin(model.Email.Trim(), model.Password.Trim());
            if (user != null)
            {

                if (user.Status == StatusEnum.Inactive)
                {
                    return Unauthorized();
                }
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
                    UserPortals = user.UserPortals,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                };
                identity.MapClaims();
                var result = await _customUserManager.PasswordSignInAsync(identity, model.Password, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {


                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(6),
                        claims: identity.Claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            return Unauthorized();
        }


        [HttpPost]
        [Route("AuthenticateLogin")]
        public async Task<IActionResult> AuthenticateLogin(string username, string password)
        {

            var user = await _userBusiness.ValidateLogin(username.Trim(), password.Trim());
            if (user != null)
            {

                if (user.Status == StatusEnum.Inactive)
                {
                    return Unauthorized();
                }
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
                    UserPortals = user.UserPortals,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                };
                identity.MapClaims();
                var result = await _customUserManager.PasswordSignInAsync(identity, password, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {


                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(6),
                        claims: identity.Claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(identity);
                }
                else
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }
        [HttpGet]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            password = Helper.Decrypt(password);
            var user = await _userBusiness.ValidateLogin(username.Trim(), password.Trim());
            if (user != null)
            {
                if (user.Status == StatusEnum.Inactive)
                {
                    return Ok(new
                    {
                        success = false,
                        error = "User is not active"
                    });
                }
                var identity = new ApplicationIdentityUser
                {
                    success = true,
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
                    UserPortals = user.UserPortals,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                };
                return Ok(identity);
            }
            return Ok(new
            {
                success = false,
                error = "Inavlid User Name/Password."
            });
        }

        [HttpPost]
        [Route("GetPortalListByEmail")]
        public async Task<ActionResult> GetPortalListByEmail(string email)
        {
            var userid = await _userBusiness.GetSingle(x => x.Email == email.Trim());
            var data = await _userBusiness.GetAllowedPortalList(userid.Id);
            return Ok(data);
        }

        [HttpPost]
        [Route("AuthenticateByPhone")]
        public async Task<IActionResult> AuthenticateByPhone(LoginViewModel model)
        {
            var user = await _userBusiness.GetSingle(x => x.Mobile == model.MobileNo);
            if (model.Mode == "VALIDATE")
            {
                //if (model.TwoFactorAuthOTP.IsNullOrEmpty())
                //{
                //    return Ok(new { success = false, error = "Please enter OTP" });

                //}
                //var result = await MobileNoLoginUser(user, model);

                //if (result.Item1)
                //{
                //    return Ok(new { success = true, ru = model.ReturnUrl });
                //}
                //return Ok(new { success = false, error = result.Item2 });
            }
            if (user == null)
            {
                if (model.MobileNo.IsNullOrEmpty() || model.MobileNo.Length != 10)
                {
                    return Ok(new { success = false, error = "Please enter a valid 10 digit mobile number" });
                }

                user = new UserViewModel
                {
                    Email = model.MobileNo,
                    Mobile = model.MobileNo,
                    UserName = model.MobileNo,
                    EnableTwoFactorAuth = true,
                    Name = model.MobileNo

                };
                var userResult = await _userBusiness.Create(user);
                if (userResult.IsSuccess)
                {
                    await _userBusiness.Create<UserPortalViewModel, UserPortal>(new UserPortalViewModel
                    {
                        PortalId = model.PortalId,
                        UserId = user.Id,
                    });
                    var portal = await _portalBusiness.GetSingleById(model.PortalId);
                    
                    if(portal.Name == "JammuSmartCityCustomer")
                    {
                        var userrole = await _userRoleBusiness.GetSingle(x => x.Code == "JSC_CITIZEN");
                        await _userRoleUserBusiness.Create<UserRoleUserViewModel, UserRoleUser>(new UserRoleUserViewModel
                        {
                            UserRoleId = userrole.Id,
                            UserId = user.Id,
                        });
                    }                   
                    
                    // Assign portal access and default citizen role
                }
                else
                {
                    return Ok(new { success = false, error = userResult.Messages.ToHtmlError() });
                }
            }

            var userRoles = await _userBusiness.GetList<UserRoleUser, UserRoleUser>(x => x.UserId == user.Id, x => x.UserRole);
            var roles = userRoles.Select(x => x.UserRole).ToList();
            user.UserRole = roles.Select(x => x.Code).ToList();

            var otpStatus = await GenerateOtp(user.Email);

            return Ok(new { success = otpStatus, email = user });
        }

        private async Task<Tuple<bool, string>> MobileNoLoginUser(UserViewModel user, LoginViewModel model)
        {
            user = await _userBusiness.ValidateUserById(user.Id);
            var returnUrl = model.ReturnUrl ?? Url.Content("~/");
            if (user != null)
            {
                if (user.TwoFactorAuthOTP != model.TwoFactorAuthOTP)
                {
                    return new Tuple<bool, string>(false, "Invalid OTP");
                }
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
                    PortalId = model.PortalId,
                    UserPortals = user.UserPortals,
                    IsGuestUser = user.IsGuestUser,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,

                };
                if (model.PortalId.IsNotNullAndNotEmpty())
                {
                    var portal = await _portalBusiness.GetSingleById(model.PortalId);
                    if (portal != null)
                    {
                        identity.PortalTheme = portal.Theme.ToString();
                        identity.PortalName = portal.Name;
                    }
                }
                identity.MapClaims();
                var result = await _customUserManager.PasswordSignInAsync(identity, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    await _applicationAccessBusiness.CreateLogin(Request, user);
                    if (model.RenderMode != null && model.RenderMode.ToLower() == "pv")
                    {
                        var portal = await _portalBusiness.GetSingleById(model.PortalId);
                        if (portal != null)
                        {
                            returnUrl = @$"/Portal/{portal.Name}?partialViewUrl={HttpUtility.UrlEncode(returnUrl)}";
                            return new Tuple<bool, string>(true, returnUrl);
                        }
                    }
                    if (identity.PortalName.IsNotNull())
                    {
                        returnUrl = @$"/Portal/{identity.PortalName}";
                        if (identity.PortalName.ToLower() == "cms")
                        {
                            returnUrl = @$"/cms/content/index";
                        }
                        model.ReturnUrl = returnUrl;
                    }
                    return new Tuple<bool, string>(true, returnUrl);
                }
                else
                {

                    return new Tuple<bool, string>(false, "Invalid Mobile/OTP");
                }
            }
            else
            {
                return new Tuple<bool, string>(false, "Mobile number not registered");
            }
        }

        private async Task<bool> GenerateOtp(string email)
        {
            var user = await _userBusiness.TwoFactorAuthOTP(email);
            var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "TWO_FACTOR_OTP");
            if (notificationTemplateModel.IsNotNull())
            {
                //EmailViewModel emailModel = new EmailViewModel();
                //emailModel.To = user.Email;
                //emailModel.Subject = notificationTemplateModel.Subject;
                //emailModel.Body = notificationTemplateModel.Body.Replace("{{EMAIL_OTP}}", user.TwoFactorAuthOTP);
                //var resultemail = await _emailBusiness.SendMail(emailModel);
                var notificationViewModel = new NotificationViewModel
                {
                    To = user.Email,
                    ToUserId = user.Id,
                    SendAlways = true,
                    NotifyBySms = true,
                    NotifyByEmail = true,
                    Subject = notificationTemplateModel.Subject,
                    Body = notificationTemplateModel.Body.Replace("{{EMAIL_OTP}}", user.TwoFactorAuthOTP)
                };
                await _notificationBusiness.Create(notificationViewModel);
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("GenerateToken")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateToken(string username, string password)
        {

            var user = await _userBusiness.ValidateLogin(username.Trim(), password.Trim());
            if (user != null)
            {

                if (user.Status == StatusEnum.Inactive)
                {
                    return Unauthorized();
                }
                //var identity = new ApplicationIdentityUser
                //{
                //    Id = user.Id,
                //    UserName = user.Name,
                //    IsSystemAdmin = user.IsSystemAdmin,
                //    Email = user.Email,
                //    UserUniqueId = user.Email,
                //    CompanyId = user.CompanyId,
                //    CompanyCode = user.CompanyCode,
                //    CompanyName = user.CompanyName,
                //    JobTitle = user.JobTitle,
                //    PhotoId = user.PhotoId,
                //    UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                //    UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                //    UserPortals = user.UserPortals,
                //    LegalEntityId = user.LegalEntityId,
                //    LegalEntityCode = user.LegalEntityCode,
                //    PersonId = user.PersonId,
                //    PositionId = user.PositionId,
                //    DepartmentId = user.DepartmentId,
                //};
                //identity.MapClaims();
                //var result = await _customUserManager.PasswordSignInAsync(identity, password, true, lockoutOnFailure: false);
                else
                {
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user.Id),
                            }
                        ),
                        Expires = DateTime.Now.AddHours(6),
                        SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),

                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    String finalToken = tokenHandler.WriteToken(token);

                    return Ok(finalToken);
                }
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
