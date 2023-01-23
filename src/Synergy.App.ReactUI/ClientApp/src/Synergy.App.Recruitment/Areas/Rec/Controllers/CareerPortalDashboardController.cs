using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class CareerPortalDashboardController : ApplicationController
    {
        public IJobAdvertisementBusiness _jobAdvertisementBusiness;
        public IListOfValueBusiness _lovBusiness;
        public IPageBusiness _pageBusiness;
        public IUserBusiness _userBusiness;
        public ICandidateProfileBusiness _candidateProfileBusiness;
        public IRecEmailBusiness _emailBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IWebHelper _webApi;
        private readonly IUserContext _userContext;

        public CareerPortalDashboardController(IJobAdvertisementBusiness jobAdvertisementBusiness
            , IListOfValueBusiness lovBusiness,
            IPageBusiness pageBusiness
            , IUserBusiness userBusiness
            , ICandidateProfileBusiness candidateProfileBusiness
            , AuthSignInManager<ApplicationIdentityUser> customUserManager,
            IRecEmailBusiness emailBusiness,
            IWebHelper webApi, IUserContext userContext)
        {
            _jobAdvertisementBusiness = jobAdvertisementBusiness;
            _lovBusiness = lovBusiness;
            _pageBusiness = pageBusiness;
            _userBusiness = userBusiness;
            _candidateProfileBusiness = candidateProfileBusiness;
            _customUserManager = customUserManager;
            _emailBusiness = emailBusiness;
            _webApi = webApi;
            _userContext = userContext;
        }
        public async Task<IActionResult> Index(string pageId)
        {
            var model = new CareerPortalDashboardViewModel();
            ViewBag.Page =await _webApi.GetApiAsync<PageViewModel>("Recruitment/Query/GetPageForExecution?PageId="+ pageId); //await _pageBusiness.GetPageForExecution(pageId);
            //var jcl = await _lovBusiness.GetList(x => x.ListOfValueType == "JOB_CATEGORY" && x.Status != Common.StatusEnum.Inactive,x=>x.Parent);
            //model.JobCategoryList = await _webApi.GetListAsync<ListOfValueViewModel>("Recruitment/Query/GetJobCategoryListOfValue"); //jcl.OrderBy(x => x.SequenceOrder).ToList();
            model.JobCategoryList = await _webApi.GetApiListAsync<ListOfValueViewModel>("Recruitment/Query/GetJobCategoryListOfValueWithCount?agencyId="+_userContext.UserId); //jcl.OrderBy(x => x.SequenceOrder).ToList();
            //foreach (var category in model.JobCategoryList)
            //{
            //    var joblist = await _webApi.GetListAsync<JobAdvertisementViewModel>("Recruitment/Query/GetJobAdvertisementList?categoryId="+ category.Id); //_jobAdvertisementBusiness.GetJobAdvertisementList(null, category.Id, null, null);
            //    category.JobCount = joblist.Count().ToString();
            //}
            var manpowerlist = await _webApi.GetApiListAsync<ListOfValueViewModel>("Recruitment/Query/GetManpowerTypeListOfValue");//_lovBusiness.GetList(x => x.ListOfValueType == "LOV_MANPOWERTYPE" && x.Status != Common.StatusEnum.Inactive);

            manpowerlist.Add(new ListOfValueViewModel
            {
                Id="0",
                Name="All",
                Code="All"
            });
            model.ManpowerTypeList = manpowerlist.OrderBy(x => x.SequenceOrder).ToList();
            return View(model);
        }
        public async Task<IActionResult> Login(string pageId, string returnUrl)
        {
            var page = await _pageBusiness.GetPageForExecution(pageId);
            if (returnUrl.IsNullOrEmpty())
            {
                returnUrl = $@"/Portal/{page.Portal.Name}";
            }

            return RedirectToAction("Login", "Account", new { @area = "", portalId = page.Portal.Id, returnUrl = returnUrl, layout = "empty" });
        }
        public async Task<IActionResult> Register(string pageId, string layOut, string returnUrl)
        {
            var page = await _pageBusiness.GetPageForExecution(pageId);
            //returnUrl = $@"/Portal/{page.Portal.Name}/MyProfile";
            returnUrl = $@"/Recruitment/CareerPortalDashboard/ActivationInfo";
            var model = new UserViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = Guid.NewGuid().ToString(),
                ReturnUrl = returnUrl,
                Layout = "/Views/Shared/_PopupLayout.cshtml",
                PortalName= page.Portal.Name,
            };
            if (layOut == "empty")
            {
                model.Layout = null;
            }
            ViewBag.Page = page;
           
            return View("Register", model);
        }
        public async Task<IActionResult> ActivateUser(string code,string portalName,string email)
        {
            var activateUser = new ActivateUserViewModel();
            var user=await _userBusiness.GetSingle(x => x.ActivationCode == code && x.Email==email);
            if (user.IsNotNull())
            {
                user.ConfirmPassword = user.Password;              
                user.Status = StatusEnum.Active;                
                await _userBusiness.Edit(user);
                activateUser.IsActivated = true;
                activateUser.Url= $@"/Portal/{portalName}/MyProfile";
                EmailViewModel emailModel = new EmailViewModel();
                emailModel.CompanyId = user.CompanyId;
                emailModel.To = user.Email;
                emailModel.Subject = "Welcome Email";
                emailModel.Body = "Dear " + user.Name + "<br/> Greetings! <br/><br/> You are cordially invited " +
                    "to join GALFAR Recuritment(Synergy) system and start experiencing smarter collabration.<br/> Your User Id is " +
                    "" + user.Email + " and Password is: " + user.Password + "<br/> After logging in to GALFAR Recuritment(Synergy), please change your password.";
                var resultemail = await _emailBusiness.SendMail(emailModel);
                var user1 = await _userBusiness.ValidateLogin(user.Email, user.Password);

                if (user1 != null)
                {
                    var id = new ApplicationIdentityUser
                    {
                        Id = user1.Id,
                        UserName = user1.Name,
                        IsSystemAdmin = user1.IsSystemAdmin,
                        Email = user1.Email,
                        UserUniqueId = user1.Email,
                        CompanyId = user1.CompanyId,
                        CompanyCode = user1.CompanyCode,
                        CompanyName = user1.CompanyName,
                        JobTitle = user1.JobTitle,
                        PhotoId = user1.PhotoId,
                        UserRoleCodes = string.Join(",", user1.UserRoles.Select(x => x.Code)),
                        UserRoleIds = string.Join(",", user1.UserRoles.Select(x => x.Id)),
                        UserPortals = user1.UserPortals,
                        LegalEntityId=user1.LegalEntityId,
                        LegalEntityCode=user1.LegalEntityCode
                        //CandidateId=user.CandidateId
                    };
                    id.MapClaims();
                    var managerResult = await _customUserManager.PasswordSignInAsync(id, user.Password, true, lockoutOnFailure: false);
                }
            }
            activateUser.Layout = "/Views/Shared/_PopupLayout.cshtml";
            return View("ActivateUser", activateUser);
        }
        public async Task<IActionResult> ActivationInfo()
        {            
            ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            var usermodel = new UserViewModel
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Id = model.Id,
                DataAction = model.DataAction,
                PortalName = "CareerPortal",
                UserType = UserTypeEnum.CANDIDATE,
                ActivationCode = Guid.NewGuid().ToString(),
                Status=StatusEnum.Inactive,
                PasswordChanged=true,

            };
            if (usermodel.Id.IsNotNull())
            {
                var result = await _userBusiness.Create(usermodel);
                if (result.IsSuccess)
                {
                    // send Email
                    var link = $@"{_webApi.GetHost()}Recruitment/CareerPortalDashboard/ActivateUser?code={usermodel.ActivationCode}&portalName={usermodel.PortalName}&email={usermodel.Email}";
                    var emailModel = new EmailViewModel();
                    emailModel.CompanyId = result.Item.CompanyId;
                    emailModel.To = usermodel.Email;
                    emailModel.Subject = "Activate User";
                    emailModel.Body = "Dear "+ usermodel.Name+ "<br/> Greetings! <br/><br/> Kindly <a href='"+link+"' target='_blank'>click Here</a> to activate user <br/><br/> ";
                    var resultemail=await _emailBusiness.SendMail(emailModel);
                    var user = await _userBusiness.ValidateLogin(model.Email, model.Password);

                    //if (user != null)
                    //{
                    //    var id = new ApplicationIdentityUser
                    //    {
                    //        Id = user.Id,
                    //        UserName = user.Name,
                    //        IsSystemAdmin = user.IsSystemAdmin,
                    //        Email = user.Email,
                    //        UserUniqueId = user.Email,
                    //        CompanyId = user.CompanyId,
                    //        CompanyCode = user.CompanyCode,
                    //        CompanyName = user.CompanyName,
                    //        JobTitle = user.JobTitle,
                    //        PhotoId = user.PhotoId,
                    //        UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                    //        UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                    //        UserPortals=user.UserPortals
                    //        //CandidateId=user.CandidateId
                    //    };
                    //    id.MapClaims();
                    //    var managerResult = await _customUserManager.PasswordSignInAsync(id, model.Password, true, lockoutOnFailure: false);
                    //}
                    //ViewBag.Success = true;
                    var candmodel = new CandidateProfileViewModel
                    {
                        FirstName = model.Name,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserId = result.Item.Id,
                        SourceFrom = SourceTypeEnum.CareerPortal.ToString(),
                        Id = Guid.NewGuid().ToString()
                    };
                    var _candmodelresult = await _candidateProfileBusiness.CreateCandidate(candmodel);
                    if (_candmodelresult.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return View("Register", new UserViewModel());
        }
        //[HttpPost]
        //public async Task<IActionResult> Register1(UserViewModel model)
        //{
        //    var usermodel = new UserViewModel
        //    {
        //        Name = model.Name,
        //        Email = model.Email,
        //        Password = model.Password,
        //        ConfirmPassword = model.ConfirmPassword,
        //        Id = model.Id,
        //        DataAction = model.DataAction,
        //        PortalName = "CareerPortal",
        //        UserType = UserTypeEnum.CANDIDATE,
        //        ActivationCode = Guid.NewGuid().ToString(),
        //        Status = StatusEnum.Inactive,
        //    };
        //    if (usermodel.Id.IsNotNull())
        //    {
        //        var result = await _userBusiness.Create(usermodel);
        //        if (result.IsSuccess)
        //        {
        //            // send Email
        //            EmailViewModel emailModel = new EmailViewModel();
        //            emailModel.To = usermodel.Email;
        //            emailModel.Subject = "Welcome Email";
        //            emailModel.Body = "Dear " + usermodel.Name + "<br/> Greetings! <br/><br/> You are cordially invited " +
        //                "to join GALFAR Recuritment(Synergy) system and start experiencing smarter collabration.<br/> Your User Id is " +
        //                "" + usermodel.Email + " and Password is: " + usermodel.Password + "<br/> After logging in to GALFAR Recuritment(Synergy), please change your password.";
        //            var resultemail = await _emailBusiness.SendMail(emailModel);
        //            var user = await _userBusiness.ValidateLogin(model.Email, model.Password);

        //            if (user != null)
        //            {
        //                var id = new ApplicationIdentityUser
        //                {
        //                    Id = user.Id,
        //                    UserName = user.Name,
        //                    IsSystemAdmin = user.IsSystemAdmin,
        //                    Email = user.Email,
        //                    UserUniqueId = user.Email,
        //                    CompanyId = user.CompanyId,
        //                    CompanyCode = user.CompanyCode,
        //                    CompanyName = user.CompanyName,
        //                    JobTitle = user.JobTitle,
        //                    PhotoId = user.PhotoId,
        //                    UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
        //                    UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
        //                    UserPortals = user.UserPortals
        //                    //CandidateId=user.CandidateId
        //                };
        //                id.MapClaims();
        //                var managerResult = await _customUserManager.PasswordSignInAsync(id, model.Password, true, lockoutOnFailure: false);
        //            }
        //            //ViewBag.Success = true;
        //            var candmodel = new CandidateProfileViewModel
        //            {
        //                FirstName = model.Name,
        //                LastName = model.LastName,
        //                Email = model.Email,
        //                UserId = result.Item.Id,
        //                SourceFrom = SourceTypeEnum.CareerPortal.ToString(),
        //                Id = Guid.NewGuid().ToString()
        //            };
        //            var _candmodelresult = await _candidateProfileBusiness.CreateCandidate(candmodel);
        //            if (_candmodelresult.IsSuccess)
        //            {
        //                return Json(new { success = true });
        //            }
        //            else
        //            {
        //                ModelState.AddModelErrors(result.Messages);
        //                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelErrors(result.Messages);
        //            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        //        }
        //    }
        //    return View("Register", new UserViewModel());
        //}

        public async Task<IActionResult> ReadJobData(string keyWord, string categoryId, string locationId,string manpowerType)
        {
            //  var model = await _jobAdvertisementBusiness.GetList(x => x.JobAdvertisementStatus == "APPROVED" && x.ExpiryDate >= DateTime.Today);
            if (manpowerType == "0")
            {
                manpowerType = null;
            }
            var model = await _webApi.GetApiListAsync<JobAdvertisementViewModel>("Recruitment/Query/GetJobAdvertisementList?keyWord=" + keyWord + "&categoryId=" + categoryId+ "&locationId="+ locationId+ "&manpowerTypeId=" + manpowerType); //await _jobAdvertisementBusiness.GetJobAdvertisementList(keyWord, categoryId, locationId, manpowerType);
            //if (keyWord.IsNullOrEmpty() && categoryId.IsNullOrEmpty())
            //{
            //    model = model.Take(50).ToList();
            //}
            //if (keyWord.IsNotNullAndNotEmpty())
            //{
            //    model= model.Where(x => x.JobName.Contains(keyWord)).ToList();

            //}
            //if (categoryId.IsNotNullAndNotEmpty())
            //{
            //    model = model.Where(x => x.JobCategory== categoryId).ToList();

            //    var dsResult = data.ToDataSourceResult(request);
            //    return Json(dsResult);
            //}

            return Json(model);
        }
    }
}
