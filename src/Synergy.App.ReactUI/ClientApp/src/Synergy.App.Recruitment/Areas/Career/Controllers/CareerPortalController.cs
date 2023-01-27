using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.Career.Controllers
{
    [Area("Career")]
    public class CareerPortalController : Controller
    {
        IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        IPageBusiness _pageBusiness;
        ICareerPortalBusiness _careerPortalBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IMasterBusiness _masterBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly ICandidateExperienceByOtherBusiness _candidateExperienceByOtherBusiness;
        private ICandidateExperienceByCountryBusiness _candidateExperienceByCountryBusiness;
        private IJobAdvertisementBusiness _jobAdvertisementBusiness;
        private IRecruitmentTransactionBusiness _recruitmentTransactionBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IWebHelper _webApi;
        public CareerPortalController(IUserBusiness userBusiness, IUserContext userContext, IPageBusiness pageBusiness, 
            ICareerPortalBusiness careerPortalBusiness, ICmsBusiness cmsBusiness, ILOVBusiness lOVBusiness, INoteBusiness noteBusiness,
            IMasterBusiness masterBusiness, IFileBusiness fileBusiness,
            ICandidateExperienceByOtherBusiness candidateExperienceByOtherBusiness,
             ICandidateExperienceByCountryBusiness candidateExperienceByCountryBusiness,
             IJobAdvertisementBusiness jobAdvertisementBusiness, IRecruitmentTransactionBusiness recruitmentTransactionBusiness
            , IEmailBusiness emailBusiness, AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IWebHelper webApi)
        {
            _userBusiness = userBusiness;
            _userContext = userContext;
            _pageBusiness = pageBusiness;
            _careerPortalBusiness = careerPortalBusiness;
            _cmsBusiness = cmsBusiness;
            _lOVBusiness = lOVBusiness;
            _noteBusiness = noteBusiness;
            _masterBusiness = masterBusiness;
            _fileBusiness = fileBusiness;
            _candidateExperienceByOtherBusiness = candidateExperienceByOtherBusiness;
            _candidateExperienceByCountryBusiness = candidateExperienceByCountryBusiness;
            _jobAdvertisementBusiness = jobAdvertisementBusiness;
            _recruitmentTransactionBusiness = recruitmentTransactionBusiness;
            _emailBusiness = emailBusiness;
            _customUserManager = customUserManager;
            _webApi = webApi;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View();
        }

        public async Task<IActionResult> CareerPortalDashboard(string pageId)
        {
            var model = new CareerPortalDashboardViewModel();
            ViewBag.Page = await _pageBusiness.GetPageForExecution(pageId);
            model.JobCategoryList = await _careerPortalBusiness.GetJobAdvertisementListWithCount(_userContext.UserId);
            var manpowerlist = await _careerPortalBusiness.GetManpowerTypeListOfValue();
            manpowerlist.Add(new LOVViewModel
            {
                Id = "0",
                Name = "All",
                Code = "All"
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
            returnUrl = $@"/Career/CareerPortal/ActivationInfo";
            var model = new UserViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = Guid.NewGuid().ToString(),
                ReturnUrl = returnUrl,
                Layout = "/Areas/Core/Views/Shared/_PopupLayout.cshtml",
                PortalName = page.Portal.Name,
            };
            if (layOut == "empty")
            {
                model.Layout = null;
            }
            ViewBag.Page = page;

            return View("Register", model);
        }
        public async Task<IActionResult> ActivateUser(string code, string portalName, string email)
        {
            var activateUser = new ActivateUserViewModel();
            var user = await _userBusiness.GetSingle(x => x.ActivationCode == code && x.Email == email);
            if (user.IsNotNull())
            {
                user.ConfirmPassword = user.Password;
                user.Status = StatusEnum.Active;
                await _userBusiness.Edit(user);
                activateUser.IsActivated = true;
                activateUser.Url = $@"/Portal/{portalName}/MyProfile";
                EmailViewModel emailModel = new EmailViewModel();
                emailModel.CompanyId = user.CompanyId;
                emailModel.To = user.Email;
                emailModel.Subject = "Welcome Email";
                emailModel.Body = "Dear " + user.Name + "<br/> Greetings! <br/><br/> You are cordially invited " +
                    "to join Recuritment(Synergy) system and start experiencing smarter collabration.<br/> Your User Id is " +
                    "" + user.Email + " and Password is: " + Helper.Decrypt(user.Password)  + "<br/> After logging in to Recuritment(Synergy), please change your password.";
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
                        LegalEntityId = user1.LegalEntityId,
                        LegalEntityCode = user1.LegalEntityCode
                        //CandidateId=user.CandidateId
                    };
                    id.MapClaims();
                    var managerResult = await _customUserManager.PasswordSignInAsync(id, user.Password, true, lockoutOnFailure: false);
                }
            }
            activateUser.Layout = "/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            return View("ActivateUser", activateUser);
        }
        public async Task<IActionResult> ActivationInfo()
        {
            ViewBag.Layout = "/Areas/Core/Views/Shared/_PopupLayout.cshtml";
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
                Status = StatusEnum.Inactive,
                PasswordChanged = true,

            };
            if (usermodel.Id.IsNotNull())
            {
                var resultc = await _userBusiness.Create(usermodel);
                if (resultc.IsSuccess)
                {
                    // send Email
                    var link = $@"{_webApi.GetHost()}Career/CareerPortal/ActivateUser?code={usermodel.ActivationCode}&portalName={usermodel.PortalName}&email={usermodel.Email}";
                    var emailModel = new EmailViewModel();
                    emailModel.CompanyId = resultc.Item.CompanyId;
                    emailModel.To = usermodel.Email;
                    emailModel.Subject = "Activate User";
                    emailModel.Body = "Dear " + usermodel.Name + "<br/> Greetings! <br/><br/> Kindly <a href='" + link + "' target='_blank'>click Here</a> to activate user <br/><br/> ";
                    var resultemail = await _emailBusiness.SendMail(emailModel);
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
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_CANDIDATE";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    var candmodel = new CandidateProfileViewModel
                    {
                        FirstName = model.Name,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserId = resultc.Item.Id,
                        SourceFrom = SourceTypeEnum.CareerPortal.ToString()
                    };

                    notemodel.Json = JsonConvert.SerializeObject(candmodel);

                    var result = await _noteBusiness.ManageNote(notemodel);
                    //var candmodel = new CandidateProfileViewModel
                    //{
                    //    FirstName = model.Name,
                    //    LastName = model.LastName,
                    //    Email = model.Email,
                    //    UserId = result.Item.Id,
                    //    SourceFrom = SourceTypeEnum.CareerPortal.ToString(),
                    //    Id = Guid.NewGuid().ToString()
                    //};
                    //var _candmodelresult = await _candidateProfileBusiness.CreateCandidate(candmodel);
                    if (result.IsSuccess)
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
                    ModelState.AddModelErrors(resultc.Messages);
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return View("Register", new UserViewModel());
        }
        public async Task<IActionResult> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerType, string agencyId)
        {
            var data = await _careerPortalBusiness.GetJobAdvertisementList(keyWord, categoryId, locationId, manpowerType, agencyId);
            return Json(data);
        }

        public async Task<IActionResult> GetJobList(string type)
        {
            var data = await _careerPortalBusiness.GetListOfValue(type);
            return Json(data);
        }

        public async Task<IActionResult> JobDetails(string jobAdvId, string pageId, bool isDirectLogin = false)
        {
            var model = await _careerPortalBusiness.GetNameById(jobAdvId);
            var manpowertype = await _careerPortalBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
            if (manpowertype != null)
            {
                model.ManpowerType = manpowertype.ManpowerTypeName;
            }
            if (Request.HttpContext.User.Identity.IsAuthenticated == true && !_userContext.IsGuestUser)
            {
                var candidate = await _careerPortalBusiness.IsCandidateProfileFilled();
                if (isDirectLogin)
                {
                    if (candidate.Item2 == false)
                    {
                        return RedirectToAction("Index", "CandidateProfile", new { area = "Career", jobAdvId = jobAdvId });
                    }
                    else if (candidate.Item2 == true)
                    {
                        return RedirectToAction("ApplyJob", "CandidateProfile", new { area = "Career", jobAdvId = jobAdvId, candidateProfileId = candidate.Item1.Id });
                    }
                }
                if (candidate != null && candidate.Item1 != null)
                {
                    var alreadyApplied = await _careerPortalBusiness.GetApplicationData(candidate.Item1.Id, jobAdvId);
                    model.AlreadyApplied = alreadyApplied != null;
                    model.CandidateId = candidate.Item1.Id;
                    model.IsCandidateDetailsFilled = candidate.Item2;
                    if (candidate.Item1.BookMarks != null && candidate.Item1.BookMarks.Contains(jobAdvId))
                    {
                        model.IsBookmarked = true;
                    }
                    else
                    {
                        model.IsBookmarked = false;
                    }
                }
            }
            ViewBag.Page = await _pageBusiness.GetPageForExecution(pageId);
            return View(model);
        }

        public async Task<IActionResult> ManageBookmark(string jobAdvId)
        {
            List<string> list = new List<string>();
            var model = await _careerPortalBusiness.GetCandidateDataByUserId(_userContext.UserId);
            bool exist = false;
            if (model.BookMarks.IsNotNull())
            {
                 exist = model.BookMarks.Contains(jobAdvId);
            }
            if (exist)
            {
                //list = model.BookMarks.ToList();
                list = model.BookMarks.Split(',').ToList();
                list.Remove(jobAdvId);
                //model.BookMarks = list.ToArray();
                model.BookMarks = String.Join(",", list);

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.CandidateNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                return Json(new { success = false });
                
            }
            else
            {
                if (model.BookMarks.IsNotNull()) {
                    //list = model.BookMarks.ToList();
                    list = model.BookMarks.Split(',').ToList();
                }
                list.Add(jobAdvId);
                //model.BookMarks = list.ToArray();
                model.BookMarks = String.Join(",", list);

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.CompanyId;
                noteTempModel.NoteId = model.CandidateNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                return Json(new { success = true });
                

            }
        }

        public IActionResult WorkerCandidate(string jobAdvId)
        {
            ViewBag.JobAdvId = jobAdvId;
            return View();
        }

        public async Task<IActionResult> ReadGridWorkerDetails()
        {
            var result = new List<WorkerCandidateViewModel>();
            var candidateProfile = await _careerPortalBusiness.GetWorkerList(_userContext.UserId);
            if (candidateProfile.IsNotNull() && candidateProfile.Count() > 0)
            {
                foreach (var can in candidateProfile)
                {
                    var res = new WorkerCandidateViewModel
                    {
                        CandidateName = can.FirstName,
                        DOB = can.BirthDate.ToDefaultDateFormat(),
                        Mobile = can.ContactPhoneHome,
                        PassportCountry = can.PassportIssueCountryId,
                        PassportExpiry = can.PassportExpiryDate.ToDefaultDateFormat(),
                        PassportNumber = can.PassportNumber,
                        PassportStatus = can.PassportStatusId,
                        Remarks = can.Remarks,
                        Salary_QAR = can.NetSalary,
                        Age = can.Age.ToString(),
                        TotalWorkExperience = can.TotalWorkExperience.ToString(),
                        Position = can.JobAdvertisement
                    };

                    if (res.PassportCountry.IsNotNullAndNotEmpty())
                    {
                        var nation = await _masterBusiness.GetIdNameList("Country");
                        if (nation.IsNotNull())
                        {
                            var re = nation.Where(x => x.Id == res.PassportCountry).FirstOrDefault();
                            if (re.IsNotNull())
                            {
                                res.PassportCountry = re.Name;// + "_" + re.Id;
                            }
                        }
                    }


                    if (res.Position.IsNotNullAndNotEmpty())
                    {
                        //var jd = await _jobAdvertisementBusiness.Get();
                        var jd = await _masterBusiness.GetJobNameById(res.Position.Split("_")[^1]);
                        if (jd.IsNotNull())
                        {
                            res.Position = jd.Name;// + "_" + res.Position.Split("_")[^1];
                        }
                    }

                    if (res.PassportStatus.IsNotNullAndNotEmpty())
                    {
                        var data = await _lOVBusiness.GetList(x => x.LOVType == "LOV_PASSPORTSTATUS");
                        if (data.IsNotNull())
                        {
                            var re = data.Where(x => x.Id == res.PassportStatus).FirstOrDefault();
                            if (re.IsNotNull())
                            {
                                res.PassportStatus = re.Name;// + "_" + re.Id;
                            }
                        }
                    }

                    var resume = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
                    if (resume.IsNotNull())
                    {
                        res.DocumentLink = resume.FileName + "_" + resume.Id;
                    }
                    /////////////////////////////////////////////////////////////////////////
                    //if (res.PassportCountry.IsNotNullAndNotEmpty())
                    //{
                    //    var nation = await _masterBusiness.GetIdNameList("Country");
                    //    if (nation.IsNotNull())
                    //    {
                    //        var re = nation.Where(x => x.Id == res.PassportCountry).FirstOrDefault();
                    //        if (re.IsNotNull())
                    //        {
                    //            res.PassportCountry = re.Id;// + "_" + re.Id;
                    //        }
                    //    }
                    //}


                    //if (res.Position.IsNotNullAndNotEmpty())
                    //{
                    //    //var jd = await _jobAdvertisementBusiness.Get();
                    //    //var jd = await _masterBusiness.GetJobNameById(res.Position.Split("_")[^1]);
                    //    //if (jd.IsNotNull())
                    //    //{
                    //    //    res.Position = jd.Id;// + "_" + res.Position.Split("_")[^1];
                    //    //}
                    //}

                    //if (res.PassportStatus.IsNotNullAndNotEmpty())
                    //{
                    //    var data = await _lOVBusiness.GetList(x => x.LOVType == "LOV_PASSPORTSTATUS");
                    //    if (data.IsNotNull())
                    //    {
                    //        var re = data.Where(x => x.Id == res.PassportStatus).FirstOrDefault();
                    //        if (re.IsNotNull())
                    //        {
                    //            res.PassportStatus = re.Id;// + "_" + re.Id;
                    //        }
                    //    }
                    //}

                    //var resume = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
                    //if (resume.IsNotNull())
                    //{
                    //    res.DocumentLink = /*resume.FileName + "_" +*/ resume.Id;
                    //}

                    var othetExp = await _lOVBusiness.GetSingle(x => x.Code == "Other" && x.LOVType == "LOV_Country");
                    if (othetExp.IsNotNull())
                    {
                        var candidateExpOther = await _candidateExperienceByOtherBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
                        if (candidateExpOther.IsNotNull())
                        {
                            res.WorkExperienceAbroad = candidateExpOther.NoOfYear.ToString();
                        }
                    }

                    var countryIndia = string.Empty;

                    var countryListData = await _masterBusiness.GetIdNameList("Country");
                    if (countryListData.IsNotNull())
                    {
                        var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
                        if (selectedCountry.IsNotNull())
                        {
                            countryIndia = selectedCountry.Id;
                        }

                    }
                    var candidateExpCountry = await _candidateExperienceByCountryBusiness.GetSingle(x => x.CandidateProfileId == can.Id && x.CountryId == countryIndia);
                    if (candidateExpCountry.IsNotNull())
                    {
                        res.WorkExperienceIndia = candidateExpCountry.NoOfYear.ToString();
                    }

                    if (res.WorkExperienceIndia.IsNotNull() && res.WorkExperienceAbroad.IsNotNull())
                    {
                        res.TotalWorkExperience = (int.Parse(res.WorkExperienceAbroad) + int.Parse(res.WorkExperienceIndia)).ToString();
                    }
                    else if (res.WorkExperienceIndia.IsNotNull())
                    {
                        res.TotalWorkExperience = int.Parse(res.WorkExperienceIndia).ToString();
                    }
                    else if (res.WorkExperienceIndia.IsNotNull())
                    {
                        res.TotalWorkExperience = int.Parse(res.WorkExperienceAbroad).ToString();
                    }
                    result.Add(res);

                }
            }
            return Json(result);
        }

        public async Task<ActionResult> GetJobAdvertismentForWorker(string jobAdvId)
        {
            var result = await _jobAdvertisementBusiness.GetJobIdNameList();
            if (jobAdvId.IsNotNull())
            {
                result = result.Where(x => x.JobAdvId == jobAdvId && x.ManpowerTypeName != "Staff").ToList();
            }
            return Json(result);
        }

        public IActionResult StaffCandidate(string jobAdvId)
        {
            ViewBag.JobAdvId = jobAdvId;
            return View();
        }

        public async Task<IActionResult> ReadGridStaffDetails(string candidateProfileId)
        {
            var result = new List<StaffCandidateViewModel>();
            var candidateProfile = await _careerPortalBusiness.GetStaffList(_userContext.UserId);
            if (candidateProfile.IsNotNull() && candidateProfile.Count() > 0)
            {
                foreach (var can in candidateProfile)
                {
                    var job = await _recruitmentTransactionBusiness.GetJobNameById(can.JobAdvertisement.Split("_")[^1]);
                    var appDetails = await _careerPortalBusiness.GetCandidateAppDetails(can.Id, job.Id);
                    //GetSingle(x => x.CandidateProfileId == can.Id && x.JobId == job.Id);
                    var res = new StaffCandidateViewModel
                    {
                        CandidateName = can.FirstName,
                        PassportNumber = can.PassportNumber,
                        Nationality = can.nationality,
                        ContactNumber = can.ContactPhoneHome,
                        EmailId = can.Email,
                        TotalExperience = can.TotalWorkExperience.ToString(),
                        PresentSalary = can.NetSalary,
                        NoticePeriod = can.NoticePeriod,
                        CurrentLocation = can.CurrentAddress,
                        ResumeLink = can.ResumeAttachmentName,
                        CVSendDate = can.CreatedDate.ToDatabaseDateFormat(),
                        JobAdvertisement = can.JobAdvertisement,
                        ExpectedSalary = can.ExpectedSalary,
                        PermanentAddressHouse = can.PermanentAddressHouse,
                        PermanentAddressStreet = can.PermanentAddressStreet,
                        PermanentAddressCity = can.PermanentAddressCity,
                        PermanentAddressState = can.PermanentAddressState,
                        PermanentAddressCountry = can.PermanentAddressCountryId,
                        MaritalStatus = can.MaritalStatus,
                        CandidateId = can.Id,
                    };
                    if (appDetails.IsNotNull())
                    {
                        res.CVSendDate = appDetails.AppliedDate.ToDefaultDateFormat();
                        res.ApplicationState = appDetails.ApplicationStateName;
                        res.ApplicationNo = appDetails.ApplicationNo;
                    }
                 if (res.Nationality.IsNotNull() && !res.Nationality.Contains("_"))
                    {
                        //var nationality = await _masterBusiness.GetIdNameList("Nationality");
                        //res.Nationality = nationality.Where(x => x.Id == res.Nationality).FirstOrDefault().Name;
                        var nationality = await _recruitmentTransactionBusiness.GetNationalitybyId(res.Nationality);
                        res.Nationality = nationality.Name.ToString();

                    }
                    else if (res.Nationality.Contains("_"))
                    {
                        res.Nationality = res.Nationality.Split("_")[0];
                    }
                    //PermanentAddressCountry
                    if (res.PermanentAddressCountry.IsNotNull() && !res.PermanentAddressCountry.Contains("_"))
                    {
                        //var permanentAddressCountry = await _masterBusiness.GetIdNameList("Country");
                        //res.PermanentAddressCountry = permanentAddressCountry.Where(x => x.Id == res.PermanentAddressCountry).FirstOrDefault().Name;
                        var permanentAddressCountry = await _recruitmentTransactionBusiness.GetCountrybyId(res.PermanentAddressCountry);
                        res.PermanentAddressCountry = permanentAddressCountry.ToString();
                    }
                    else if (res.PermanentAddressCountry.IsNotNull() && res.PermanentAddressCountry.Contains("_"))
                    {
                        res.PermanentAddressCountry = res.PermanentAddressCountry.Split("_")[0];
                    }
                    //MaritalStatus
                    if (res.MaritalStatus.IsNotNull() && !res.MaritalStatus.Contains("_"))
                    {
                        var maritalStatus = await _lOVBusiness.GetList(x => x.LOVType == "LOV_MARITALSTATUS");
                        res.MaritalStatus = maritalStatus.Where(x => x.Id == res.MaritalStatus).FirstOrDefault().Name;
                    }
                    else if (res.MaritalStatus.IsNotNull() && res.MaritalStatus.Contains("_"))
                    {
                        res.MaritalStatus = res.MaritalStatus.Split("_")[0];
                    }

                    var jd = await _recruitmentTransactionBusiness.GetJobNameById(res.JobAdvertisement.Split("_")[^1]);
                    if (jd.IsNotNull())
                    {
                        res.JobAdvertisement = jd.Name; //+ "_" + res.JobAdvertisement.Split("_")[^1];
                    }


                    var candidateQualification = _careerPortalBusiness.GetCandidateEducationalbyId(can.Id);
                    if (candidateQualification.IsNotNull())
                    {
                        //res.Qualification = candidateQualification.OtherQualification;
                    }

                    var candidateExperience = await _careerPortalBusiness.GetCandidateExperiencebyId(can.Id);
                    if (candidateExperience.IsNotNull())
                    {
                        res.PresentEmployer = candidateExperience.Employer;
                        res.Designation = candidateExperience.JobTitle;
                    }

                    var resume = await _fileBusiness.GetSingle(x => x.Id == can.ResumeId);
                    if (resume.IsNotNull())
                    {
                        res.ResumeLink = resume.FileName + "_" + resume.Id;
                    }

                    var pass = await _fileBusiness.GetSingle(x => x.Id == can.PassportAttachmentId);
                    if (pass.IsNotNull())
                    {
                        res.PassportLink = pass.FileName + "_" + pass.Id;
                    }

                    var candidateExp = await _careerPortalBusiness.GetCandidateExperiencebyId(can.Id);
                    if (candidateExp.IsNotNull())
                    {
                        var a = await _fileBusiness.GetSingle(x => x.Id == candidateExp.AttachmentId);
                        if (a.IsNotNull())
                        {
                            res.ExperienceLetterLink = a.FileName + "_" + a.Id;
                        }
                    }

                    var candidateEducation = await _careerPortalBusiness.GetCandidateEducationalbyId(can.Id);
                    if (candidateEducation.IsNotNull())
                    {

                        var a = await _fileBusiness.GetSingle(x => x.Id == candidateEducation.AttachmentId);
                        if (a.IsNotNull())
                        {
                            res.EducationLink = a.FileName + "_" + a.Id;
                        }
                    }
                    var other = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
                    if (other.IsNotNull())
                    {
                        res.DocumentLink = other.FileName + "_" + other.Id;
                    }
                    result.Add(res);

                }

            }

            return Json(result);
        }

            //        if (res.PassportCountry.IsNotNullAndNotEmpty())
            //        {
            //            var nation = await _masterBusiness.GetIdNameList("Country");
            //            if (nation.IsNotNull())
            //            {
            //                var re = nation.Where(x => x.Id == res.PassportCountry).FirstOrDefault();
            //                if (re.IsNotNull())
            //                {
            //                    res.PassportCountry = re.Name;// + "_" + re.Id;
            //                }
            //            }
            //        }


            //        if (res.Position.IsNotNullAndNotEmpty())
            //        {
            //            //var jd = await _jobAdvertisementBusiness.Get();
            //            var jd = await _masterBusiness.GetJobNameById(res.Position.Split("_")[^1]);
            //            if (jd.IsNotNull())
            //            {
            //                res.Position = jd.Name;// + "_" + res.Position.Split("_")[^1];
            //            }
            //        }

            //        if (res.PassportStatus.IsNotNullAndNotEmpty())
            //        {
            //            var data = await _lOVBusiness.GetList(x => x.LOVType == "LOV_PASSPORTSTATUS");
            //            if (data.IsNotNull())
            //            {
            //                var re = data.Where(x => x.Id == res.PassportStatus).FirstOrDefault();
            //                if (re.IsNotNull())
            //                {
            //                    res.PassportStatus = re.Name;// + "_" + re.Id;
            //                }
            //            }
            //        }

            //        var resume = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
            //        if (resume.IsNotNull())
            //        {
            //            res.DocumentLink = resume.FileName + "_" + resume.Id;
            //        }

            //        var othetExp = await _lOVBusiness.GetSingle(x => x.Code == "Other" && x.LOVType == "LOV_Country");
            //        if (othetExp.IsNotNull())
            //        {
            //            var candidateExpOther = await _candidateExperienceByOtherBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //            if (candidateExpOther.IsNotNull())
            //            {
            //                res.WorkExperienceAbroad = candidateExpOther.NoOfYear.ToString();
            //            }
            //        }

            //        var countryIndia = string.Empty;

            //        var countryListData = await _masterBusiness.GetIdNameList("Country");
            //        if (countryListData.IsNotNull())
            //        {
            //            var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
            //            if (selectedCountry.IsNotNull())
            //            {
            //                countryIndia = selectedCountry.Id;
            //            }

            //        }
            //        var candidateExpCountry = await _candidateExperienceByCountryBusiness.GetSingle(x => x.CandidateProfileId == can.Id && x.CountryId == countryIndia);
            //        if (candidateExpCountry.IsNotNull())
            //        {
            //            res.WorkExperienceIndia = candidateExpCountry.NoOfYear.ToString();
            //        }

            //        if (res.WorkExperienceIndia.IsNotNull() && res.WorkExperienceAbroad.IsNotNull())
            //        {
            //            res.TotalWorkExperience = (int.Parse(res.WorkExperienceAbroad) + int.Parse(res.WorkExperienceIndia)).ToString();
            //        }
            //        else if (res.WorkExperienceIndia.IsNotNull())
            //        {
            //            res.TotalWorkExperience = int.Parse(res.WorkExperienceIndia).ToString();
            //        }
            //        else if (res.WorkExperienceIndia.IsNotNull())
            //        {
            //            res.TotalWorkExperience = int.Parse(res.WorkExperienceAbroad).ToString();
            //        }
            //        result.Add(res);

            //    }
            //}
            //return Json(result);
        

        public async Task<ActionResult> GetJobAdvertismentForStaff(string jobAdvId)
        {
            var result = await _jobAdvertisementBusiness.GetJobIdNameList();
            if (jobAdvId.IsNotNull())
            {
                result = result.Where(x => x.JobAdvId == jobAdvId && x.ManpowerTypeName == "Staff").ToList();
            }
            return Json(result);
        }

        public async Task<ActionResult> GetCountry()
        {
            var nationality = await _masterBusiness.GetIdNameList("Country");
            return Json(nationality);
        }
        public async Task<ActionResult> GetMaritalStatus()
        {
            var data = await _lOVBusiness.GetList(x => x.LOVType == "LOV_MARITALSTATUS");
             return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageWorkerDetails(List<WorkerCandidateViewModel> list)
        {
            if (list.IsNotNull())
            {
                //var a = await _careerPortalBusiness.CreateWorkerCandidateAndApplication(list);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
