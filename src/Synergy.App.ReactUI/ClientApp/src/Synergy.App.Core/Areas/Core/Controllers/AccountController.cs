using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using CMS.Web;
using DNTCaptcha.Core;
using Hangfire;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMS.UI.Web
{
    public class AccountController : ApplicationController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IPushNotificationBusiness _notifyBusiness;
        private readonly INotificationBusiness _notificationBusiness;
        public IRecEmailBusiness _emailBusiness;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly IMapper _autoMapper;
        private readonly IApplicationAccessBusiness _applicationAccessBusiness;
        private readonly ICompanySettingBusiness _csb;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IUserRoleUserBusiness _userRoleUserBusiness;
        private readonly IUserPortalBusiness _userportalBusiness;
        private readonly IEGovernanceBusiness _eGovernanceBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public AccountController(IUserBusiness userBusiness, IUserPortalBusiness userportalBusiness, IEGovernanceBusiness eGovernanceBusiness,
            AuthSignInManager<ApplicationIdentityUser> customUserManager, IUserContext userContext
            , IPortalBusiness portalBusiness, IPushNotificationBusiness notifyBusiness
            , IRecEmailBusiness emailBusiness, IDNTCaptchaValidatorService validatorService, IMapper autoMapper
            , IApplicationAccessBusiness applicationAccessBusiness
            , ICompanySettingBusiness csb, IUserRoleBusiness userRoleBusiness, IUserRoleUserBusiness userRoleUserBusiness
            , INotificationBusiness notificationBusiness)
        {
            _userBusiness = userBusiness;
            _customUserManager = customUserManager;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
            _notifyBusiness = notifyBusiness;
            _emailBusiness = emailBusiness;
            _validatorService = validatorService;
            _autoMapper = autoMapper;
            _applicationAccessBusiness = applicationAccessBusiness;
            _csb = csb;
            _notificationBusiness = notificationBusiness;
            _userRoleBusiness = userRoleBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _userportalBusiness = userportalBusiness;
            _eGovernanceBusiness = eGovernanceBusiness;

        }
        public async Task<IActionResult> RegisterUser(string portalId, string returnUrl = null, string layout = null, string rm = null, bool eGovLayoutNull = false)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == portalId);
            var view = "";
            if (portal != null && portal.Name == "BLSCustomer")
            {
                return Redirect("/bls/blsapplication/blsregistration?portalId=" + portal.Id);
            }
            else
            {
                view = $"~/Areas/Core/Views/Account/Login.cshtml";
                return View(view, new LoginViewModel
                {
                    PortalId = portalId,
                    RenderMode = rm,
                    ReturnUrl = returnUrl,
                    LoginViewName = view,
                    AuthenticateViewName = $"~/Areas/Core/Views/Account/Authenticate.cshtml"
                });
            }

        }
        public async Task<IActionResult> Login(string portalId, string portalName = null, string returnUrl = null, string layout = null, string rm = null, bool eGovLayoutNull = false)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await _customUserManager.SignOutAsync();
            var portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == portalId || x.Name == portalName);
            var view = "";
            if (portal != null)
            {
                ViewBag.Portal = portal;
                view = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/Login.cshtml";
                var model = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    LoginViewName = view,
                    AuthenticateViewName = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/Authenticate.cshtml",
                    PortalId = portalId,
                    Layout = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/_LoginLayout.cshtml",
                    RenderMode = rm

                };
                if (layout == "empty")
                {
                    model.Layout = null;
                }
                ViewBag.FavIconId = portal.FavIconId;
                if (portal.Name == "EGovCustomer" || portal.Name == "EGovEmployee" || portal.Name == "SEBIInvestor")
                {
                    view = $"~/Areas/Core/Views/Account/EGovLogin.cshtml";
                    model.Email = "dummy@admin.com";
                    if (!eGovLayoutNull)
                    {
                        ViewBag.Layout = $"~/Areas/Core/Views/Shared/_LoginReference.cshtml";
                    }
                }
                if (portal.Name == "JammuSmartCityCustomer")
                {
                    model.Email = "dummy@admin.com";
                }
                return View(view, model);
            }
            else
            {
                view = $"~/Areas/Core/Views/Account/Login.cshtml";
                return View(view, new LoginViewModel
                {
                    PortalId = portalId,
                    RenderMode = rm,
                    ReturnUrl = returnUrl,
                    LoginViewName = view,
                    AuthenticateViewName = $"~/Areas/Core/Views/Account/Authenticate.cshtml"
                });
            }

        }
        public async Task<IActionResult> AccessDenied(string returnUrl)
        {
            var allowedPortals = await _userBusiness.GetAllowedPortalList(_userContext.UserId);
            if (allowedPortals != null && allowedPortals.Any())
            {
                if (allowedPortals.Count == 1)
                {
                    return Redirect($"/portal/{allowedPortals[0].Name}");
                }
                return View("SelectPortal", allowedPortals);
            }
            return View();
        }
        public async Task<IActionResult> Authenticate(string email, string returnUrl, string portalId)
        {
            var user = await _userBusiness.GetSingleGlobal(x => x.Email == email);
            if (user != null)
            {
                var vm = _autoMapper.Map<LoginViewModel>(user);
                var allowedPortals = await _userBusiness.GetAllowedPortalList(user.Id);

                vm.PortalId = portalId;

                if (portalId.IsNullOrEmpty())
                {
                    if (allowedPortals != null && allowedPortals.Count == 1)
                    {
                        vm.PortalId = allowedPortals.FirstOrDefault()?.Id;
                    }
                    var cms = allowedPortals.FirstOrDefault(x => x.Name == "CMS");
                    if (cms != null)
                    {
                        vm.PortalId = cms.Id;
                    }
                }
                //var portal = await _portalBusiness.GetSingleById(vm.PortalId);
                //if (portal != null)
                //{
                //    vm.EnableTwoFactorAuth = portal.EnableTwoFactorAuth;
                //    vm.TwoFactorAuthType = portal.TwoFactorAuthType;
                //    if (vm.TwoFactorAuthType == TwoFactorAuthTypeEnum.OTPAndPassword || !vm.EnableTwoFactorAuth)
                //    {
                //        vm.Password = "";
                //    }

                //}
                //if (user.OverrideTwoFactorAuthentication)
                //{
                //    vm.EnableTwoFactorAuth = user.EnableTwoFactorAuth;
                //    vm.TwoFactorAuthType = user.TwoFactorAuthType;
                //    if (vm.TwoFactorAuthType == TwoFactorAuthTypeEnum.OTPAndPassword || !vm.EnableTwoFactorAuth)
                //    {
                //        vm.Password = "";
                //    }
                //}

                if (vm.TwoFactorAuthType == TwoFactorAuthTypeEnum.OTPAndPassword || !vm.EnableTwoFactorAuth)
                {
                    vm.Password = "";
                }
                vm.ReturnUrl = returnUrl;
                return View(vm);
            }

            return View(new LoginViewModel());

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                model.Password = Helper.DecryptJavascriptAesCypher(model.Password);
            }
            catch (Exception)
            {

            }
            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    this.ModelState.AddModelError("captcha", "Please enter valid security code.");
                    return View(model.LoginViewName, model);
                }
            }
            else
            {
                return View(model.LoginViewName, model);
            }
            var result = await LoginUser(model);

            if (result.Item1)
            {
                return LocalRedirect(result.Item2);
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return View(model.LoginViewName, model);
        }
        [HttpPost]
        public async Task<IActionResult> EGovLogin(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return Json(new { success = false, error = "Please enter valid user id and password" });
            }
            var result = await LoginEGovUser(model);

            if (result.Item1)
            {
                return Json(new { success = true, url = result.Item2 });
            }
            return Json(new { success = false, error = result.Item2, ex = result.Item3 });
        }
        public async Task<IActionResult> ValidatePassword(string email, string url, string portal)
        {
            var user = await _userBusiness.GetSingleGlobal(x => x.Email == email);
            ; return View(new LoginViewModel() { Email = email, ReturnUrl = url, EnableTwoFactorAuth = user.EnableTwoFactorAuth, TwoFactorAuthType = user.TwoFactorAuthType, PortalId = portal });
        }
        public async Task<IActionResult> AdminValidatePassword(string email, string url)
        {
            var user = await _userBusiness.GetSingleGlobal(x => x.Email == email);
            return View(new LoginViewModel() { Email = email, ReturnUrl = url, EnableTwoFactorAuth = user.EnableTwoFactorAuth, TwoFactorAuthType = user.TwoFactorAuthType });
        }
        [HttpPost]
        public async Task<IActionResult> ReSendOTP(string email)
        {
            var result = await GenerateOtp(email);
            return Json(new { success = result });
        }
        private async Task<bool> GenerateOtp(string email)
        {
            var user = await _userBusiness.TwoFactorAuthOTP(email);
            var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "TWO_FACTOR_OTP");
            if (notificationTemplateModel.IsNotNull())
            {
                var notificationViewModel = new NotificationViewModel
                {
                    To = user.Email,
                    ToUserId = user.Id,
                    SendAlways = true,
                    NotifyBySms = true,
                    NotifyByEmail = true,
                    Subject = notificationTemplateModel.Subject,
                    Body = notificationTemplateModel.Body.Replace("{{EMAIL_OTP}}", user.TwoFactorAuthOTP),
                    SmsText = notificationTemplateModel.SmsText?.Replace("{{SMS_OTP}}", user.TwoFactorAuthOTP),
                };
                await _notificationBusiness.Create(notificationViewModel);
                return true;
            }
            return false;
        }

        [HttpPost]
        public async Task<IActionResult> JammuCustomerLogin(LoginViewModel model)
        {
            var user = await _userBusiness.GetSingle(x => x.Mobile == model.MobileNo);
            if (model.Mode == "VALIDATE")
            {
                if (model.TwoFactorAuthOTP.IsNullOrEmpty())
                {
                    return Json(new { success = false, error = "Please enter OTP" });

                }
                var result = await MobileNoLoginUser(user, model);

                if (result.Item1)
                {
                    return Json(new { success = true, ru = model.ReturnUrl });
                }
                return Json(new { success = false, error = result.Item2 });
            }
            if (user == null)
            {
                if (model.MobileNo.IsNullOrEmpty() || model.MobileNo.Length != 10)
                {
                    return Json(new { success = false, error = "Please enter a valid 10 digit mobile number" });
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
                    var userrole = await _userRoleBusiness.GetSingle(x => x.Code == "JSC_CITIZEN");
                    await _userRoleUserBusiness.Create<UserRoleUserViewModel, UserRoleUser>(new UserRoleUserViewModel
                    {
                        UserRoleId = userrole.Id,
                        UserId = user.Id,
                    });
                    // Assign portal access and default citizen role
                }
                else
                {
                    return Json(new { success = false, error = userResult.Messages.ToHtmlError() });
                }
            }
            var otpStatus = await GenerateOtp(user.Email);
            return Json(new { success = otpStatus, email = user.Email });
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
                            if (returnUrl.IsNullOrEmptyOrValue("/"))
                            {
                                returnUrl = @$"/Portal/{portal.Name}?partialViewUrl={HttpUtility.UrlEncode(returnUrl)}";
                            }

                            return new Tuple<bool, string>(true, returnUrl);
                        }
                    }
                    if (identity.PortalName.IsNotNull())
                    {
                        if (returnUrl.IsNullOrEmptyOrValue("/"))
                        {
                            returnUrl = @$"/Portal/{identity.PortalName}";
                        }

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

        [HttpPost]
        public async Task<IActionResult> EmailLogin(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model.LoginViewName, model);
            }
            var user = await _userBusiness.TwoFactorAuthOTP(model.Email);
            if (user != null)
            {
                if (user.Status == StatusEnum.Inactive)
                {
                    this.ModelState.AddModelError("inactive", "User is inactive. Please activate the user or contact administrator");
                }
                else if (user.EnableTwoFactorAuth)
                {
                    try
                    {
                        var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "TWO_FACTOR_OTP");
                        if (notificationTemplateModel.IsNotNull())
                        {
                            EmailViewModel emailModel = new EmailViewModel();
                            emailModel.To = user.Email;
                            emailModel.Subject = notificationTemplateModel.Subject;
                            emailModel.Body = notificationTemplateModel.Body.Replace("{{EMAIL_OTP}}", user.TwoFactorAuthOTP);
                            emailModel.Body = emailModel.Body.Replace("{{USER_NAME}}", user.Name);
                            var resultemail = await _emailBusiness.SendMail(emailModel);


                            if (resultemail.IsSuccess)
                            {
                                ViewBag.Status = "Your email authendication OTP has been sent to you";
                                ViewBag.Success = true;
                                return RedirectToAction("Authenticate", new { email = user.Email, returnUrl = model.ReturnUrl, portalId = model.PortalId });
                                var enc = Helper.EncryptJavascriptAesCypher($"email={user.Email}&portslId={model.PortalId}&returnUrl={model.ReturnUrl}");
                                return LocalRedirect($"/Authenticate?enc={enc}");
                            }
                            else
                            {
                                ModelState.AddModelError("", resultemail.Messages.ElementAt(0).Value);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Problem while sending email, Please check details.");

                    }
                }
                else
                {
                    //ViewBag.Success = true;
                    return RedirectToAction("Authenticate", new { email = user.Email, returnUrl = model.ReturnUrl, portalId = model.PortalId });
                    var enc = Helper.EncryptJavascriptAesCypher($"email={user.Email}&portslId={model.PortalId}&returnUrl={model.ReturnUrl}");
                    return LocalRedirect($"/Authenticate?enc={enc}");
                    //return View(model.AuthenticateViewName, model);
                }
            }
            else
            {
                this.ModelState.AddModelError("Invalid", "Invalid Email");
            }

            return View(model.LoginViewName, model);
        }
        [HttpPost]
        public async Task<IActionResult> Authenticate(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.PortalId.IsNullOrEmpty())
                {
                    //this.ModelState.AddModelError("Portal", "Please select the Portal.");
                    return Json(new { success = false, error = "Please select the Portal." });
                    //return View("Authenticate", model);
                }
                if (model.EnableTwoFactorAuth != true && !_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    //this.ModelState.AddModelError("captcha", "Please enter valid security code.");
                    return Json(new { success = false, error = "Please enter valid security code." });
                    // return View("Authenticate", model);
                }
                if (model.EnableTwoFactorAuth)
                {
                    if (model.TwoFactorAuthOTP.IsNullOrEmpty())
                    {
                        //this.ModelState.AddModelError("OTP", "Please enter the OTP.");
                        return Json(new { success = false, error = "Please enter the OTP." });
                        //return Json(new { success = true, error = "Please enter the OTP" });

                    }
                    //if (ModelState.ErrorCount > 0)
                    //{
                    //    return View("Authenticate", model);
                    //}

                }
                try
                {
                    if (model.EnableTwoFactorAuth)
                    {
                        if (model.TwoFactorAuthType.HasValue && model.TwoFactorAuthType == TwoFactorAuthTypeEnum.OTPAndPassword)
                        {
                            if (model.Password.IsNotNullAndNotEmpty())
                            {
                                model.Password = Helper.DecryptJavascriptAesCypher(model.Password);
                            }
                            else
                            {
                                // this.ModelState.AddModelError("pass", "Please enter the password.");
                                return Json(new { success = false, error = "Please enter the password." });
                                return View("Authenticate", model);
                            }
                        }
                        else
                        {
                            var pass = await _userBusiness.GetSingleGlobal(x => x.Email == model.Email.Trim());
                            //if (pass.Password.IsNotNullAndNotEmpty())
                            //{
                            //    model.Password = Helper.Decrypt(pass.Password);
                            //}
                            if (!model.EnableTwoFactorAuth || pass.TwoFactorAuthType == TwoFactorAuthTypeEnum.OTPAndPassword)
                            {
                                if (model.Password.IsNotNullAndNotEmpty())
                                {
                                    var enteredPass = Helper.DecryptJavascriptAesCypher(model.Password);
                                    var existingPass = Helper.Decrypt(pass.Password);
                                    if (enteredPass.IsNotNullAndNotEmpty())
                                    {
                                        if (existingPass != enteredPass)
                                        {
                                            return Json(new { success = false, error = "Please enter correct password." });
                                        }
                                        model.Password = Helper.Decrypt(pass.Password);
                                    }
                                    else
                                    {
                                        return Json(new { success = false, error = "Please enter the password." });
                                    }

                                }
                                else
                                {
                                    return Json(new { success = false, error = "Please enter the password." });

                                }
                            }
                            else
                            {
                                if (pass.Password.IsNotNullAndNotEmpty())
                                {
                                    model.Password = Helper.Decrypt(pass.Password);
                                }
                            }
                        }

                    }
                    else
                    {
                        if (model.Password.IsNotNullAndNotEmpty())
                        {
                            model.Password = Helper.DecryptJavascriptAesCypher(model.Password);
                        }
                        else
                        {
                            return Json(new { success = false, error = "Please enter the password." });
                            this.ModelState.AddModelError("pass", "Please enter the password.");
                            return View("Authenticate", model);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                return View("Authenticate", model);
            }
            var result = await EmailLoginUser(model);

            if (result.Item1)
            {
                return Json(new { success = true, ru = model.ReturnUrl });
                // return LocalRedirect(result.Item2);
                ViewBag.ReturnUlr = model.ReturnUrl;
                return LocalRedirect(model.ReturnUrl);
                //// ViewBag.Success = true;
                //return View("Validatepassword", model);
            }
            return Json(new { success = false, error = result.Item2 });
        }
        [HttpPost]
        public async Task<IActionResult> AdminEmailLoginSubmit(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.EnableTwoFactorAuth != true && !_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    this.ModelState.AddModelError("captcha", "Please enter valid security code.");
                    return View("AdminValidatepassword", model);
                }
                if (model.EnableTwoFactorAuth)
                {
                    if (model.TwoFactorAuthOTP.IsNullOrEmpty())
                    {
                        this.ModelState.AddModelError("OTP", "Please enter the OTP.");

                    }
                    if (ModelState.ErrorCount > 0)
                    {
                        return View("AdminValidatepassword", model);
                    }

                }
                try
                {
                    if (model.EnableTwoFactorAuth)
                    {
                        if (model.TwoFactorAuthType.HasValue && model.TwoFactorAuthType == TwoFactorAuthTypeEnum.OTPAndPassword)
                        {
                            if (model.Password.IsNotNullAndNotEmpty())
                            {
                                model.Password = Helper.DecryptJavascriptAesCypher(model.Password);
                            }
                            else
                            {
                                this.ModelState.AddModelError("pass", "Please enter the password.");
                                return View("AdminValidatepassword", model);
                            }
                        }
                        else
                        {
                            var pass = await _userBusiness.GetSingleGlobal(x => x.Email == model.Email.Trim());
                            if (pass.Password.IsNotNullAndNotEmpty())
                            {
                                model.Password = Helper.Decrypt(pass.Password);
                            }

                        }

                    }
                    else
                    {
                        if (model.Password.IsNotNullAndNotEmpty())
                        {
                            model.Password = Helper.DecryptJavascriptAesCypher(model.Password);
                        }
                        else
                        {
                            this.ModelState.AddModelError("pass", "Please enter the password.");
                            return View("AdminValidatepassword", model);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                return View("AdminValidatepassword", model);
            }
            var result = await EmailLoginUser(model);

            if (result.Item1)
            {
                // return LocalRedirect(result.Item2);
                ViewBag.ReturnUlr = model.ReturnUrl;
                ViewBag.Success = true;
                return View("AdminValidatepassword", model);
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return View("AdminValidatepassword", model);
        }
        private async Task<Tuple<bool, string>> EmailLoginUser(LoginViewModel model)
        {
            var returnUrl = model.ReturnUrl ?? Url.Content("~/");

            var user = await _userBusiness.ValidateLogin(model.Email.Trim(), model.Password.Trim());
            if (user != null)
            {
                if (user.EnableTwoFactorAuth)
                {
                    var twofactorAuth = await _userBusiness.GetSingle(x => x.Email == model.Email.Trim() && x.TwoFactorAuthOTP == model.TwoFactorAuthOTP);
                    if (twofactorAuth == null)
                    {
                        return new Tuple<bool, string>(false, "Invalid OTP");
                    }
                }

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

                    return new Tuple<bool, string>(false, "Invalid User Name/Password");
                }


            }
            else
            {
                return new Tuple<bool, string>(false, "Invalid Password");
            }
        }
        [HttpPost]
        public async Task<IActionResult> LoginAjax(LoginViewModel model)
        {
            try
            {
                //model.Password = Helper.DecryptJavascriptAesCypher(model.Password);
                //model.Password = Helper.Encrypt(model.Password);
                model.Password = model.Password;
            }
            catch (Exception)
            {

            }
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

                    await _applicationAccessBusiness.CreateAccessLog(Request, AccessLogTypeEnum.Login);
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
        private async Task<Tuple<bool, string, string>> LoginEGovUser(LoginViewModel model)
        {

            var returnUrl = model.ReturnUrl ?? Url.Content("~/");
            var baseAuthUrlItem = await _csb.GetSingle(x => x.Code == "SMARTCITY_AUTH_BASE_URL");
            var baseAuthUrl = "";
            if (baseAuthUrlItem != null)
            {
                baseAuthUrl = baseAuthUrlItem.Value;
            }
            var signInUrl = $"{baseAuthUrl.Trim('/')}/signin";
            var loginData = new EGovUserViewModel { userId = model.UserId, password = model.Password };
            var loginDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(loginData, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            var response = "";
            try
            {
                response = await Helper.HttpRequest(signInUrl, HttpVerb.Post, loginDataJson, true);
            }
            catch (Exception e)
            {

                return new Tuple<bool, string, string>(false, "Error in Authentication. Please contact Administrator", e.ToString());
            }

            var token = JToken.Parse(response);
            var login = token.SelectToken("login");

            if (Convert.ToString(login) != "SUCCESS")
            {
                //var createUserUrl = $"{baseAuthUrl.Trim('/')}/createuser"; //"https://api.belsmartcity.in/api/sc/createuser";
                //var data = new EGovUserViewModel
                //{
                //    userId = model.UserId,
                //    name = "admin",
                //    aadharNo = "12345",
                //    houseNo = "12345",
                //    street = "Downtown hills",
                //    wardName = "Srinagar",
                //    talukZone = "East",
                //    areaVillage = "Srinagar",
                //    postOffice = "Srinagar",
                //    districtCity = "Srinagar",
                //    stateName = "Jammu and Kashmir",
                //    pincode = "147203",
                //    nationality = "indian",
                //    gender = "M",
                //    dateOfBirth = "1989-12-12",
                //    emailId = "admin@synergy.com",
                //    contactNumber = "2599323204",
                //    departmentId = "GIS",
                //    role = new string[] { "ADMIN" },
                //    staffId = "123456",
                //    bloodGroup = "B+ve",
                //    password = "!Welcome123"
                //};

                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                //var createUserResponse = await Helper.HttpRequest(createUserUrl, HttpVerb.Post, json);
                //var createUserResponseToken = JToken.Parse(createUserResponse);
                //var signup = createUserResponseToken.SelectToken("createuser");
                //if (Convert.ToString(signup) != "SUCCESS")
                //{

                return new Tuple<bool, string, string>(false, "Invalid User Id/Password", "");
                //}

            }
            var user = await _userBusiness.ValidateUserByUserId(model.UserId);
            if (user == null)
            {
                var userName = token.SelectToken("name");
                var emailId = token.SelectToken("emailId");
                var userWard = token.SelectToken("wardName");
                var userPortal = await _portalBusiness.GetSingle(x => x.Name == "EGovCustomer");
                var u = await _userBusiness.CreateGlobal(new UserViewModel
                {
                    UserId = model.UserId,
                    UserName = model.UserId,
                    Name = Convert.ToString(userName),
                    Email = Convert.ToString(emailId), //$"{model.UserId}@egov.com",
                    Password = Helper.Encrypt(model.Password), //model.Password,
                    PasswordChanged = true,
                    Status = StatusEnum.Active,
                    IsDeleted = false,
                    CompanyId = _userContext.CompanyId,
                    LegalEntityId = _userContext.LegalEntityId,
                    CreatedBy = _userContext.UserId,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = _userContext.UserId,
                    LastUpdatedDate = DateTime.Now,
                    PortalId = userPortal.Id
                });
                if (u.IsSuccess)
                {
                    user = await _userBusiness.ValidateUserByUserId(model.UserId);
                    if (userPortal != null)
                    {
                        var res = await _userportalBusiness.Create(new UserPortalViewModel
                        {
                            UserId = user.Id,
                            PortalId = userPortal.Id,
                        });
                        user.UserPortals = userPortal.Name;

                    }
                    EGOVUserDetailsViewModel userEntryModel = new();
                    userEntryModel.UserId = user.Id;
                    userEntryModel.WardName = Convert.ToString(userWard);
                    var userEntryRes = await _eGovernanceBusiness.CreateUserEntry(userEntryModel);
                }


            }
            if (user != null)
            {
                if (user.Status == StatusEnum.Inactive)
                {
                    return new Tuple<bool, string, string>(false, "User is inactive. Please activate the user or contact administrator", "");
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
                    PortalId = user.PortalId,
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

                    await _applicationAccessBusiness.CreateAccessLog(Request, AccessLogTypeEnum.Login);
                    if (model.RenderMode != null && model.RenderMode.ToLower() == "pv")
                    {
                        var portal = await _portalBusiness.GetSingleById(model.PortalId);
                        if (portal != null)
                        {
                            returnUrl = @$"/Portal/{portal.Name}?partialViewUrl={HttpUtility.UrlEncode(returnUrl)}";
                            return new Tuple<bool, string, string>(true, returnUrl, "");
                        }
                    }
                    return new Tuple<bool, string, string>(true, returnUrl, "");
                }
                else
                {

                    return new Tuple<bool, string, string>(false, "Invalid User Id/Password", "");
                }


            }
            else
            {
                return new Tuple<bool, string, string>(false, "Invalid User Id/Password", "");
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
        public async Task<IActionResult> ChangeUserPassword(string userId)
        {
            return View("ChangeUserPassword", new ChangePassowrdViewModel() { UserId = userId });
        }
        public async Task<IActionResult> ChangePasswordPage(string portalId, string returnUrl)
        {
            var portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == portalId);
            var view = "";
            if (portal != null)
            {
                view = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/ChangePassword.cshtml";
                var model = new ChangePassowrdViewModel
                {
                    ReturnUrl = returnUrl,
                    UserId = _userContext.UserId
                };

                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/_EmptyLayout.cshtml";
                return View(view, model);
            }
            else
            {
                view = $"~/Areas/Core/Views/Shared/Themes/Recruitment/ChangePassword.cshtml";
                return View(view, new ChangePassowrdViewModel() { UserId = _userContext.UserId, ReturnUrl = returnUrl });
            }

        }
        [HttpPost]
        public async Task<IActionResult> ManageChangePassword(ChangePassowrdViewModel model)
        {
            try
            {
                model.CurrentPassword = Helper.DecryptJavascriptAesCypher(model.CurrentPassword);
                model.NewPassword = Helper.DecryptJavascriptAesCypher(model.NewPassword);
                model.ConfirmPassword = Helper.DecryptJavascriptAesCypher(model.ConfirmPassword);
            }
            catch (Exception)
            {

            }
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
            try
            {
                model.CurrentPassword = Helper.DecryptJavascriptAesCypher(model.CurrentPassword);
                model.NewPassword = Helper.DecryptJavascriptAesCypher(model.NewPassword);
                model.ConfirmPassword = Helper.DecryptJavascriptAesCypher(model.ConfirmPassword);
            }
            catch (Exception)
            {

                throw;
            }
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
        public async Task<IActionResult> ChangeUserPassword(ChangePassowrdViewModel model)
        {
            try
            {
                var user = await _userBusiness.GetSingleById(model.UserId);
                var decryptPass = Helper.Decrypt(user.Password);
                model.CurrentPassword = decryptPass;
                model.NewPassword = Helper.DecryptJavascriptAesCypher(model.NewPassword);
                model.ConfirmPassword = Helper.DecryptJavascriptAesCypher(model.ConfirmPassword);
            }
            catch (Exception)
            {

                throw;
            }
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
        [IgnoreAntiforgeryToken]
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

        public async Task<IActionResult> ForgotPassword(string portalId, string email)
        {

            ViewBag.Portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == portalId);
            var view = $"~/Areas/Core/Views/Account/ForgotPassword.cshtml";
            return View(view, new ForgotViewModel { Mode = "forgot", Email = email });
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
                            emailModel.Subject = "Synergy password reset OTP";
                            emailModel.Body = "Your password reset OTP is : " + user.ForgotPasswordOTP;
                            var resultemail = await _emailBusiness.SendMail(emailModel);


                            if (resultemail.IsSuccess)
                            {
                                ViewBag.Portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == model.PortalId);
                                model.Mode = "success";
                                ViewBag.Status = "The password reset OTP has been sent to your email";
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
                    ModelState.AddModelError("", "The given email is invalid. Please enter a valid email or contact Administrator");
                }
            }
            return View(model);
        }

        public ActionResult ForgotPasswordOTP()
        {
            var model = new ForgotViewModel();
            model.Mode = "forgot";
            return View("ForgotPassword");
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPasswordOTP(ForgotViewModel model)
        {
            //var view = $"~/Areas/Core/Views/Account/ForgotPassword.cshtml";
            ViewBag.Portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == model.PortalId);
            bool otpflag = false;

            if (ModelState.IsValid)
            {
                if (model != null)
                {
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
                    ModelState.AddModelError("", "Something went wrong, please contact the Admin");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPasswordChange(ForgotViewModel model)
        {
            var portal = await _userBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Id == model.PortalId);
            ViewBag.Portal = portal;
            model.Mode = "successotp";
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    if (model.Password != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Confirm password is not same as new password.");
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
                                    model.LoginUrl = "/account/login";
                                    if (portal != null && portal.Name != "CMS")
                                    {
                                        model.PortalName = portal.Name;
                                        model.LoginUrl = portal.Name == "EGovCustomer" ? $"/portal/{portal.Name.ToString()}/Login" : $"/portal/{portal.Name.ToString()}";
                                    }
                                    return View("ForgotPassword", model);

                                }
                                else
                                {
                                    ModelState.AddModelError("", "Please enter correct details");
                                }
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("", "Problem while password change. Please contact the Admin");
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The given details is wrong. Please contact the Admin");
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
        public ActionResult SwitchPortal()
        {
            return View(new SwitchProfileViewModel { SwitchPortalId = _userContext.PortalId });
        }
        [HttpPost]
        public async Task<JsonResult> SwitchPortal(SwitchProfileViewModel model)
        {
            var portal = await _portalBusiness.GetSingleById(model.SwitchPortalId);
            if (portal != null)
            {
                var id = new ApplicationIdentityUser
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
                    IsGuestUser = _userContext.IsGuestUser,
                    UserPortals = _userContext.UserPortals,
                    LoggedInAsType = _userContext.LoggedInAsType,
                    LoggedInAsByUserId = _userContext.LoggedInAsByUserId,
                    LoggedInAsByUserName = _userContext.LoggedInAsByUserName,
                    PortalTheme = portal.Theme.ToString(),
                    PortalId = portal.Id,
                    LegalEntityId = _userContext.LegalEntityId,
                    LegalEntityCode = _userContext.LegalEntityCode,
                    PersonId = _userContext.PersonId,
                    PositionId = _userContext.PositionId,
                    DepartmentId = _userContext.OrganizationId,
                    PortalName = portal.Name,
                };
                id.MapClaims();
                await _customUserManager.SignInAsync(id, true);
                return Json(new { success = true, portal = portal.Name });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<JsonResult> SwitchProfile(SwitchProfileViewModel model)
        {
            var user = await _userBusiness.GetSingleById(model.SwitchToUserId);
            if (user != null)
            {

                user = await _userBusiness.ValidateUser(user.Email);

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
                        PortalId = _userContext.PortalId,
                        IsGuestUser = _userContext.IsGuestUser,
                        UserPortals = user.UserPortals,
                        LoggedInAsType = model.LoggedInAsType.Value,
                        LoggedInAsByUserId = _userContext.LoggedInAsByUserId,
                        LoggedInAsByUserName = _userContext.LoggedInAsByUserName,
                        LegalEntityId = user.LegalEntityId,
                        LegalEntityCode = user.LegalEntityCode,
                        PersonId = user.PersonId,
                        PositionId = user.PositionId,
                        DepartmentId = user.DepartmentId,
                        PortalName = _userContext.PortalName,

                    };
                    id.MapClaims();
                    var managerResult = await _customUserManager.PasswordSignInAsync(id, user.Password, true, lockoutOnFailure: false);
                }
                TempData["IsSwitchProfile"] = true;
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
                IsGuestUser = _userContext.IsGuestUser,
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

            dynamic searchParam = null;
            if (request != null && request.Filters.IsNotNull() && request.Filters.Count > 0)
            {
                searchParam = request.Filters.FirstOrDefault();
                searchParam = searchParam.ConvertedValue;
            }
            return await _userBusiness.GetSwitchToUserList(_userContext.UserId, _userContext.LoggedInAsByUserId, hasLoggedinAsPermission, searchParam);

        }
        //public async Task<ActionResult> GetSwitchToUserVirtualData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var data = await GetSwitchToUserList(request);
        //    if (request.Filters != null)
        //    {
        //        request.Filters.Clear();
        //    }
        //    return Json(data.ToDataSourceResult(request));
        //}
        //public async Task<ActionResult> GetSwitchUserValueMapper(string value)
        //{
        //    var dataItemIndex = -1;
        //    var data = await GetSwitchToUserList();
        //    if (value != null)
        //    {
        //        var item = data.FirstOrDefault(x => x.Id == value);
        //        dataItemIndex = data.IndexOf(item);
        //    }
        //    return Json(dataItemIndex);
        //}


        public async Task<ActionResult> GetSwitchUserValueMapper(string value, string filters)
        {
            long dataItemIndex = -1;

            if (value != null)
            {
                var list = await _userBusiness.GetSwitchUserList(null, null, false, filters, 0, 0);
                dataItemIndex = list.ItemIndex;
            }
            return Json(dataItemIndex);
        }
        public async Task<ActionResult> GetSwitchToUserVirtualData(int page, int pageSize, string filters, string hierarchyId, string parentId = null)
        {
            var hasLoggedinAsPermission = _userContext.IsSystemAdmin
               || _userContext.LoggedInAsType == LoggedInAsTypeEnum.LoginAsDifferentUser;

            var list = await _userBusiness.GetSwitchUserList(_userContext.UserId, _userContext.LoggedInAsByUserId, hasLoggedinAsPermission, filters, pageSize, page);

            return Json(new { Data = list.Data, Total = list.Total });

        }

        [HttpPost]
        public async Task<ActionResult> ChangeContext(string email, string legalEntityId, string portalId)
        {
            var legalEntityCode = _userContext.LegalEntityCode;
            if (legalEntityId.IsNullOrEmpty())
            {
                legalEntityId = _userContext.LegalEntityId;

            }
            else
            {
                var le = await _userBusiness.GetSingleById<LegalEntityViewModel, LegalEntity>(legalEntityId);
                if (le != null)
                {
                    legalEntityCode = le.Code;
                }

            }
            var portalTheme = _userContext.PortalTheme;
            var portalName = _userContext.PortalName;
            if (portalId.IsNullOrEmpty())
            {
                portalId = _userContext.PortalId;

            }
            else
            {
                var portal = await _userBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                if (portal != null)
                {
                    portalTheme = Convert.ToString(portal.Theme);
                    portalName = portal.Name;
                }
            }
            var user = await _userBusiness.ValidateUser(email, legalEntityId);
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
                    PortalId = portalId,
                    UserPortals = user.UserPortals,
                    IsGuestUser = user.IsGuestUser,
                    PortalTheme = portalTheme,
                    LegalEntityId = legalEntityId,
                    LegalEntityCode = legalEntityCode,
                    LoggedInAsType = _userContext.LoggedInAsType,
                    LoggedInAsByUserId = _userContext.LoggedInAsByUserId,
                    LoggedInAsByUserName = _userContext.LoggedInAsByUserName,
                    CultureName = _userContext.CultureName,
                    PersonId = _userContext.PersonId,
                    PositionId = _userContext.PositionId,
                    DepartmentId = _userContext.OrganizationId,
                    PortalName = portalName
                };
                identity.MapClaims();
                await _customUserManager.SignInAsync(identity, true);



            }
            return Json(new { success = true });
        }
    }
}
