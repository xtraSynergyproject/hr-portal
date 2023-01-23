using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using CMS.Web;
using Hangfire;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Scheduler
{
    public class AccountController : ApplicationController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IPushNotificationBusiness _notifyBusiness;
        public IRecEmailBusiness _emailBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public AccountController(IUserBusiness userBusiness,
            AuthSignInManager<ApplicationIdentityUser> customUserManager, IUserContext userContext
            , IPortalBusiness portalBusiness, IPushNotificationBusiness notifyBusiness
            , IRecEmailBusiness emailBusiness)
        {
            _userBusiness = userBusiness;
            _customUserManager = customUserManager;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
            _notifyBusiness = notifyBusiness;
            _emailBusiness = emailBusiness;
        }
        public async Task<IActionResult> Login(string portalId, string returnUrl = null, string layout = null, string rm = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == portalId);
            var view = "";
            if (portal != null)
            {
                view = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/Login.cshtml";
                var model = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    LoginViewName = view,
                    PortalId = portalId,
                    Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_LoginLayout.cshtml",
                    RenderMode = rm

                };
                if (layout == "empty")
                {
                    model.Layout = null;
                }
                return View(view, model);
            }
            else
            {
                view = $"Login";
                return View(view, new LoginViewModel { PortalId = portalId, RenderMode = rm, ReturnUrl = returnUrl, LoginViewName = view });
            }

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await LoginUser(model);

            if (result.Item1)
            {
                return LocalRedirect(result.Item2);
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return View(model.LoginViewName, model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAjax(LoginViewModel model)
        {
            var result = await LoginUser(model);
            if (result.Item1)
            {
                return Json(new { success = result.Item1, url = result.Item2 });
            }
            return Json(new { success = result.Item1, error = result.Item2 });
        }
        private async Task<Tuple<bool, string>> LoginUser(LoginViewModel model)
        {
            var returnUrl = model.ReturnUrl ?? Url.Content("~/");

            var user = await _userBusiness.ValidateLogin(model.Email.Trim(), model.Password.Trim());

            if (user != null)
            {
                if (user.Status == StatusEnum.Inactive)
                {
                    return new Tuple<bool, string>(false, "User is inactive. Please activate the user or contact administrator");
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
                    if (model.RenderMode != null && model.RenderMode.ToLower() == "pv")
                    {
                        var portal = await _portalBusiness.GetSingleById(model.PortalId);
                        if (portal != null)
                        {
                            returnUrl = @$"/Portal/{portal.Name}?partialViewUrl={HttpUtility.UrlEncode(returnUrl)}";
                            return new Tuple<bool, string>(true, returnUrl);
                        }
                    }
                    return new Tuple<bool, string>(true, returnUrl);
                }
                else
                {

                    return new Tuple<bool, string>(false, "Invalid User Name/Password");
                }


            }
            else
            {
                return new Tuple<bool, string>(false, "Invalid User Name/Password");
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {

            return View(new ChangePassowrdViewModel() { UserId = _userContext.UserId });
        }
        public async Task<IActionResult> ChangePasswordPage(string portalId, string returnUrl)
        {
            var portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == portalId);
            var view = "";
            if (portal != null)
            {
                view = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/ChangePassword.cshtml";
                var model = new ChangePassowrdViewModel
                {
                    ReturnUrl = returnUrl,
                    UserId = _userContext.UserId
                };

                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_EmptyLayout.cshtml";
                return View(view, model);
            }
            else
            {
                view = $"~/Views/Shared/Themes/Recruitment/ChangePassword.cshtml";
                return View(view, new ChangePassowrdViewModel() { UserId = _userContext.UserId, ReturnUrl = returnUrl });
            }

        }
        [HttpPost]
        public async Task<IActionResult> ManageChangePassword(ChangePassowrdViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userBusiness.ChangePassword(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, result = result.Item });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                    return Json(new { success = false, errors = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Json(new { success = false, errors = "Invalid Action" });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassowrdViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userBusiness.ChangePassword(model);
                if (result.IsSuccess)
                {
                    ViewBag.Success = true;
                    //return PopupRedirect("Password changed successfully");
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            await _customUserManager.SignOutAsync();
            return LocalRedirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeLanguage(string hdnLang = null)
        {
            await ChangeUserLanguage(hdnLang);
            return Json(new { success = true });
        }
        public async Task<IActionResult> UnAuthorize(string portalId, string themeName, string returnUrl = null)
        {

            return View(new LoginViewModel { PortalId = portalId, ReturnUrl = returnUrl });
        }

        public ActionResult ForgotPassword(string portalId)
        {
            var model = new ForgotViewModel();
            model.Mode = "forgot";
            model.PortalId = portalId;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    var user = await _userBusiness.ConfirmEmailOTP(model.Email);
                    if (user != null)
                    {
                        try
                        {
                            EmailViewModel emailModel = new EmailViewModel();
                            emailModel.To = user.Email;
                            emailModel.Subject = "Synergy User Temporary Forgot Password OTP";
                            emailModel.Body = "Your Temporary Forgot Password OTP is : " + user.ForgotPasswordOTP;
                            var resultemail = await _emailBusiness.SendMail(emailModel);

                            //var result = await _notifyBusiness.Create(new NotificationViewModel
                            //{
                            //    Body = "Your Temporary Forgot Password OTP is : " + user.ForgotPasswordOTP,
                            //    Subject = "Synergy User Temporary Forgot Password OTP",
                            //    SmsText = "Your Temporary Forgot Password OTP is : " + user.ForgotPasswordOTP,
                            //    //From = _userContext.UserId,
                            //    From = user.Email,
                            //    FromUserId=user.Id,
                            //    ToUserId = user.Id,
                            //    NotifyByEmail = true,
                            //    NotifyBySms = true,
                            //    SendAlways = true,
                            //    SendAsync = false,
                            //    //To = _userContext.Email,
                            //    To = user.Email,
                            //});
                            if (resultemail.IsSuccess)
                            {
                                //comment after testing
                                //BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.SendEmailUsingHangfire(resultemail.Item.EmailUniqueId));

                                model.Mode = "success";
                                ViewBag.Status = "The temporary forgot password OTP has been sent to you";
                                return View(model);
                            }
                            else
                            {
                                ModelState.AddModelError("", resultemail.Messages.ElementAt(0).Value);
                            }
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError("", "Problem while sending email, Please check details.");

                        }
                    }
                    ModelState.AddModelError("", "The given email address is invalid. Please enter valid email or contact Administrator");
                }
            }
            return View(model);
        }

        public ActionResult ForgotPasswordOTP()
        {
            var model = new ForgotViewModel();
            model.Mode = "forgot";
            //model.Email = email;
            //model.ForgotPasswordOTP = otp;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPasswordOTP(ForgotViewModel model)
        {
            bool otpflag = false;
            if (ModelState.IsValid)
            {
                //await (Task.Delay(5));
                if (model != null)
                {

                    //var business = BusinessHelper.GetInstance<IUserBusiness>();
                    //var user = _userBusinessGlobal.ConfirmEmail(model.Email);
                    var user = await _userBusiness.GetSingleGlobal(x => x.Email == model.Email);

                    if (user != null)
                    {
                        try
                        {
                            if (user.ForgotPasswordOTP == model.ForgotPasswordOTP)
                            {
                                otpflag = true;
                            }
                            if (otpflag)
                            {
                                model.Mode = "successotp";
                                ViewBag.Status = "Create New Password";

                                return View("ForgotPassword", model);
                            }
                            else
                            {
                                //ModelState.AddModelError("", result.Messages.FirstOrDefault(x => x.Key == "ToUserId").Value);
                                ModelState.AddModelError("", "Please enter correct OTP");

                            }
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError("", "Problem while sending OTP, Please check details.");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The given user email is wrong please contact the Admin");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Some thing went wrong, please contact the Admin");
                }
            }
            return View("ForgotPassword", model);
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPasswordChange(ForgotViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    //var business = BusinessHelper.GetInstance<IUserBusiness>();
                    //var user = _userBusinessGlobal.ConfirmEmail(model.Email);
                    if (model.Password != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Please enter correct password and re-enter password.");
                    }
                    else
                    {
                        var user = await _userBusiness.GetSingleGlobal(x => x.Email == model.Email);
                        if (user != null)
                        {
                            try
                            {
                                var result = await _userBusiness.ConfirmPasswordChange(user.Email, model.Password);

                                if (result != null && result.ForgotPasswordOTP == null)
                                {
                                    model.Mode = "successchange";
                                    ViewBag.Status = "Password Changed";
                                    if (model.PortalId.IsNotNullAndNotEmpty())
                                    {
                                        var portal = await _portalBusiness.GetSingleById(model.PortalId);
                                        if (portal != null)
                                        {
                                            model.PortalName = portal.Name;
                                            model.LoginUrl = "/portal/" + portal.Name;
                                        }
                                        else
                                        {
                                            model.LoginUrl = "/account/login";
                                        }

                                    }
                                    else
                                    {
                                        model.LoginUrl = "/account/login";
                                    }
                                    return View("ForgotPassword", model);

                                }
                                else
                                {
                                    //ModelState.AddModelError("", result.Messages.FirstOrDefault(x => x.Key == "ToUserId").Value);
                                    ModelState.AddModelError("", "Please enter correct details");
                                }
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("", "Problem while password change, Please check details.");
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The given details is wrong please contact the Admin");
                }
            }
            return View("ForgotPassword", model);
        }

        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public ActionResult SwitchProfile()
        {

            //var model = new SwitchProfileViewModel
            //{
            //    LoggedInAsType = LoggedInUserIsAdmin
            //    || LoggedInAsType == LoggedInAsTypeEnum.LoginAsDifferentUser ?
            //    LoggedInAsTypeEnum.LoginAsDifferentUser : LoggedInAsTypeEnum.SwitchProfile
            //};
            var model = new SwitchProfileViewModel
            {
                LoggedInAsType = _userContext.IsSystemAdmin
                || _userContext.LoggedInAsType == LoggedInAsTypeEnum.LoginAsDifferentUser ?
                LoggedInAsTypeEnum.LoginAsDifferentUser : LoggedInAsTypeEnum.SwitchProfile
            };
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> SwitchProfile(SwitchProfileViewModel model)
        {
            var user = await _userBusiness.GetSingleById(model.SwitchToUserId);
            if (user != null)
            {

                user = await _userBusiness.ValidateLogin(user.Email, user.Password);

                if (user != null)
                {
                    var id = new ApplicationIdentityUser
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
                        LoggedInAsType = model.LoggedInAsType.Value,
                        LoggedInAsByUserId = _userContext.LoggedInAsByUserId,
                        LoggedInAsByUserName = _userContext.LoggedInAsByUserName,
                        LegalEntityId = user.LegalEntityId,
                        LegalEntityCode = user.LegalEntityCode,
                        PersonId = user.PersonId,
                        PositionId = user.PositionId,
                        DepartmentId = user.DepartmentId,
                        PortalId = _userContext.PortalId,
                        PortalName = _userContext.PortalName,
                        PortalTheme = _userContext.PortalTheme,

                    };
                    id.MapClaims();
                    var managerResult = await _customUserManager.PasswordSignInAsync(id, user.Password, true, lockoutOnFailure: false);
                }

                return Json(new { success = true });
            }
            else
            {
                ModelState.AddModelError("InvalidUser", "Invalid User");
                return Json(new { success = false/*, errors = ModelState.SerializeErrors()*/ });
            }
        }
        private async Task ChangeUserLanguage(string lang = null)
        {
            var identity = new ApplicationIdentityUser
            {
                Id = _userContext.UserId,
                UserName = _userContext.Name,
                IsSystemAdmin = _userContext.IsSystemAdmin,
                Email = _userContext.Email,
                UserUniqueId = _userContext.Email,
                CompanyId = _userContext.CompanyId,
                CompanyCode = _userContext.CompanyCode,
                CompanyName = _userContext.CompanyName,
                JobTitle = _userContext.JobTitle,
                PhotoId = _userContext.PhotoId,
                UserRoleCodes = _userContext.UserRoleCodes,
                UserRoleIds = _userContext.UserRoleIds,
                PortalId = _userContext.PortalId,
                UserPortals = _userContext.UserPortals,
                PortalTheme = _userContext.PortalTheme,
                LoggedInAsType = _userContext.LoggedInAsType,
                LoggedInAsByUserId = _userContext.LoggedInAsByUserId,
                LoggedInAsByUserName = _userContext.LoggedInAsByUserName,
                CultureName = lang,
                LegalEntityId = _userContext.LegalEntityId,
                LegalEntityCode = _userContext.LegalEntityCode,
                PersonId = _userContext.PersonId,
                PositionId = _userContext.PositionId,
                DepartmentId = _userContext.OrganizationId,
                PortalName = _userContext.PortalName

            };
            identity.MapClaims();
            await _customUserManager.SignInAsync(identity, true, authenticationMethod: null);
        }
        private async Task<IList<UserViewModel>> GetSwitchToUserList(DataSourceRequest request = null)
        {

            var hasLoggedinAsPermission = _userContext.IsSystemAdmin
                 || _userContext.LoggedInAsType == LoggedInAsTypeEnum.LoginAsDifferentUser;

            var searchParam = "";
            if (request != null && request.Filters.Count > 0)
            {
                searchParam = Convert.ToString(((Kendo.Mvc.FilterDescriptor)request.Filters.FirstOrDefault()).ConvertedValue);
                // searchParam = request.Filters.FirstOrDefault().
            }
            return await _userBusiness.GetSwitchToUserList(_userContext.UserId, _userContext.LoggedInAsByUserId, hasLoggedinAsPermission, searchParam);

        }
        public async Task<ActionResult> GetSwitchToUserVirtualData([DataSourceRequest] DataSourceRequest request)
        {
            var data = await GetSwitchToUserList(request);
            if (request.Filters != null)
            {
                request.Filters.Clear();
            }
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetSwitchUserValueMapper(string value)
        {
            var dataItemIndex = -1;
            var data = await GetSwitchToUserList();
            if (value != null)
            {
                var item = data.FirstOrDefault(x => x.Id == value);
                dataItemIndex = data.IndexOf(item);
            }
            return Json(dataItemIndex);
        }
    }
}
