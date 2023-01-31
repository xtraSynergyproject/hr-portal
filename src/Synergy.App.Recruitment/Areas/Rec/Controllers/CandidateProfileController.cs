using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.ViewModel;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Synergy.App.Common;
using Synergy.App.Business;
using Newtonsoft.Json;
using AutoMapper;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.Rec.Controllers
{
    [Area("Recruitment")]
    public class CandidateProfileController : ApplicationController
    {
        private readonly ICandidateProfileBusiness _candidateProfileBusiness;
        private readonly ICandidateEducationalBusiness _candidateEducationalBusiness;
        private readonly ICandidateExperienceBusiness _candidateExperienceBusiness;
        private readonly ICandidateExperienceByCountryBusiness _candidateExperienceByCountryBusiness;
        private readonly ICandidateExperienceByNatureBusiness _candidateExperienceByNatureBusiness;
        private readonly ICandidateExperienceByJobBusiness _candidateExperienceByJobBusiness;
        private readonly ICandidateExperienceBySectorBusiness _candidateExperienceBySectorBusiness;
        private readonly IListOfValueBusiness _listOfValueBusiness;
        private readonly ICandidateComputerProficiencyBusiness _candidateComputerProficiencyBusiness;
        private readonly ICandidateReferencesBusiness _candidateReferencesBusiness;
        private readonly ICandidateLanguageProficiencyBusiness _candidateLanguageProficiencyBusiness;
        private readonly ICandidateDrivingLicenseBusiness _candidateDrivingLicenseBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly ICandidateProjectBusiness _candidateProjectBusiness;
        private readonly IApplicationExperienceByNatureBusiness _applicationExperienceByNatureBusiness;
        private readonly IApplicationExperienceBusiness _applicationExperienceBusiness;
        private readonly IApplicationExperienceByCountryBusiness _applicationExperienceByCountryBusiness;
        private readonly IApplicationExperienceByJobBusiness _applicationExperienceByJobBusiness;
        private readonly IApplicationExperienceBySectorBusiness _applicationExperienceBySectorBusiness;
        private readonly IApplicationProjectBusiness _applicationProjectBusiness;
        private readonly IApplicationEducationalBusiness _applicationEducationalBusiness;
        private readonly IApplicationComputerProficiencyBusiness _applicationComputerProficiencyBusiness;
        private readonly IApplicationLanguageProficiencyBusiness _applicationLanguageProficiencyBusiness;
        private readonly IApplicationDrivingLicenseBusiness _applicationDrivingLicenseBusiness;
        private readonly IApplicationReferencesBusiness _applicationReferencesBusiness;
        private readonly IJobCriteriaBusiness _jobCriteriaBusiness;
        private IMapper _autoMapper;
        private IPageBusiness _pageBusiness;
        private readonly IAppointmentApprovalRequestBusiness _appointmentApprovalRequestBusiness;
        private readonly ICandidateExperienceByOtherBusiness _candidateExperienceByOtherBusiness;
        private readonly IApplicationExperienceByOtherBusiness _applicationExperienceByOtherBusiness;
        private readonly IMasterBusiness _masterBusiness;
        private readonly IApplicationJobCriteriaBusiness _applicationJobCriteriaBusiness;
        private readonly IJobAdvertisementBusiness _jobAdvertisementBusiness;
        private readonly IRecTaskBusiness _recTaskBusiness;

        public CandidateProfileController(ICandidateProfileBusiness candidateProfileBusiness, ICandidateEducationalBusiness candidateEducationalBusiness,
            ICandidateExperienceBusiness candidateExperienceBusiness, ICandidateExperienceByCountryBusiness candidateExperienceByCountryBusiness,
            ICandidateExperienceByNatureBusiness candidateExperienceByNatureBusiness, ICandidateExperienceByJobBusiness candidateExperienceByJobBusiness,
             ICandidateExperienceBySectorBusiness candidateExperienceBySectorBusiness, IListOfValueBusiness listOfValueBusiness,
            ICandidateComputerProficiencyBusiness candidateComputerProficiencyBusiness,
            ICandidateReferencesBusiness candidateReferencesBusiness,
            IApplicationBusiness applicationBusiness, IRecTaskBusiness recTaskBusiness,
            ICandidateLanguageProficiencyBusiness candidateLanguageProficiencyBusiness, ICandidateDrivingLicenseBusiness candidateDrivingLicenseBusiness,
            ICandidateProjectBusiness candidateProjectBusiness, IApplicationExperienceByNatureBusiness applicationExperienceByNatureBusiness,
            IApplicationExperienceBusiness applicationExperienceBusiness, IApplicationExperienceByCountryBusiness applicationExperienceByCountryBusiness,
            IApplicationExperienceByJobBusiness applicationExperienceByJobBusiness, IApplicationExperienceBySectorBusiness applicationExperienceBySectorBusiness,
            IApplicationProjectBusiness applicationProjectBusiness, IApplicationEducationalBusiness applicationEducationalBusiness,
            IApplicationComputerProficiencyBusiness applicationComputerProficiencyBusiness, IApplicationLanguageProficiencyBusiness applicationLanguageProficiencyBusiness,
            IApplicationDrivingLicenseBusiness applicationDrivingLicenseBusiness, IApplicationReferencesBusiness applicationReferencesBusiness,
            IJobCriteriaBusiness jobCriteriaBusiness,
            IMapper autoMapper, IAppointmentApprovalRequestBusiness appointmentApprovalRequestBusiness, IPageBusiness pageBusiness,
             ICandidateExperienceByOtherBusiness candidateExperienceByOtherBusiness, IApplicationExperienceByOtherBusiness applicationExperienceByOtherBusiness,


             IMasterBusiness masterBusiness, IApplicationJobCriteriaBusiness applicationJobCriteriaBusiness, IJobAdvertisementBusiness jobAdvertisementBusiness)

        {
            _candidateProfileBusiness = candidateProfileBusiness;
            _candidateEducationalBusiness = candidateEducationalBusiness;
            _candidateExperienceBusiness = candidateExperienceBusiness;
            _candidateExperienceByCountryBusiness = candidateExperienceByCountryBusiness;
            _candidateExperienceByJobBusiness = candidateExperienceByJobBusiness;
            _candidateExperienceByNatureBusiness = candidateExperienceByNatureBusiness;
            _candidateExperienceBySectorBusiness = candidateExperienceBySectorBusiness;
            _listOfValueBusiness = listOfValueBusiness;
            _candidateComputerProficiencyBusiness = candidateComputerProficiencyBusiness;
            _candidateReferencesBusiness = candidateReferencesBusiness;
            _candidateLanguageProficiencyBusiness = candidateLanguageProficiencyBusiness;
            _candidateDrivingLicenseBusiness = candidateDrivingLicenseBusiness;
            _applicationBusiness = applicationBusiness;
            _candidateProjectBusiness = candidateProjectBusiness;
            _applicationExperienceByNatureBusiness = applicationExperienceByNatureBusiness;
            _applicationExperienceBusiness = applicationExperienceBusiness;
            _applicationExperienceByCountryBusiness = applicationExperienceByCountryBusiness;
            _applicationExperienceByJobBusiness = applicationExperienceByJobBusiness;
            _applicationExperienceBySectorBusiness = applicationExperienceBySectorBusiness;
            _applicationProjectBusiness = applicationProjectBusiness;
            _applicationEducationalBusiness = applicationEducationalBusiness;
            _applicationComputerProficiencyBusiness = applicationComputerProficiencyBusiness;
            _applicationLanguageProficiencyBusiness = applicationLanguageProficiencyBusiness;
            _applicationDrivingLicenseBusiness = applicationDrivingLicenseBusiness;
            _applicationReferencesBusiness = applicationReferencesBusiness;
            _jobCriteriaBusiness = jobCriteriaBusiness;
            _autoMapper = autoMapper;
            _appointmentApprovalRequestBusiness = appointmentApprovalRequestBusiness;
            _pageBusiness = pageBusiness;
            _candidateExperienceByOtherBusiness = candidateExperienceByOtherBusiness;
            _applicationExperienceByOtherBusiness = applicationExperienceByOtherBusiness;
            _masterBusiness = masterBusiness;
            _applicationJobCriteriaBusiness = applicationJobCriteriaBusiness;
            _jobAdvertisementBusiness = jobAdvertisementBusiness;
            _recTaskBusiness = recTaskBusiness;
        }
        public async Task<IActionResult> AppointmentApprovalRequest(string applicationId)
        {
            if (applicationId != null)
            {
                var appdetails = await _applicationBusiness.GetSingleById(applicationId);
                var candName = String.Concat(appdetails.FirstName, " ", appdetails.MiddleName, " ", appdetails.LastName);
                var appreqmodel = new AppointmentApprovalRequestViewModel
                {
                    SelectedCandidate = candName,
                    DataAction = DataActionEnum.Create,
                    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    ApplicationId = applicationId
                };

                return View("AppointmentApprovalRequest", appreqmodel);
            }
            else
            {
                return View("AppointmentApprovalRequest", new AppointmentApprovalRequestViewModel());
            }
        }
        public async Task<IActionResult> CandidateApplicationList(string pageId)
        {
            var model = new CandidateApplicationViewModel();
            var candidate = await _candidateProfileBusiness.GetCandidateByUser();
            if (candidate != null)
            {
                model.ApplicationList = await _applicationBusiness.GetApplicationListByCandidate(candidate.Id);
                var bookmarklist = candidate.BookMarks;
                if(bookmarklist.IsNotNull() && bookmarklist.Length > 0)
                {
                    var jobIds = String.Join(",", bookmarklist);
                    jobIds = jobIds.Replace(",","','");
                    jobIds = String.Concat("'", jobIds, "'");                    
                    model.BookmarkList = await _jobAdvertisementBusiness.GetBookmarksJobList(jobIds);

                    var candidatedetails = await _candidateProfileBusiness.IsCandidateProfileFilled();
                    if (candidatedetails != null && candidatedetails.Item1 != null)
                    {
                        //var alreadyApplied = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == candidatedetails.Item1.Id && x.JobAdvertisementId == jobAdvId);
                        //model.AlreadyApplied = alreadyApplied != null;
                        model.CandidateId = candidatedetails.Item1.Id;
                        model.IsCandidateDetailsFilled = candidatedetails.Item2;
                    }

                }

            }
            model.ConfidentialList = await _jobAdvertisementBusiness.GetJobAdvertisementByAgency();
            ViewBag.Page = await _pageBusiness.GetPageForExecution(pageId);
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> AppointmentApprovalRequest(AppointmentApprovalRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _appointmentApprovalRequestBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("index",new {id= result.Item.Id });
                        //return PopupRedirect("Portal created successfully");
                        return Json(new { success = true, candidateProfileId = result.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _appointmentApprovalRequestBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("index", new { id = result.Item.Id });
                        //return PopupRedirect("Portal edited successfully");
                        return Json(new { success = true, candidateProfileId = result.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> Index(string id,string currentTabInfo, string jobAdvId=null,string layout=null,string taskid=null)
        {
            var ppIssueCountry = await _masterBusiness.GetNationalityIdByName();
            var ppIssueCountryId = "";
            if (ppIssueCountry.IsNotNull())
            {
                ppIssueCountryId = ppIssueCountry.Id;
            }
            if(layout== "Popup")
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = null;
            }
            if (id.IsNullOrEmpty())
            {
                var candidate = await _candidateProfileBusiness.GetCandidateByUser();
                if (candidate==null)
                {
                    candidate = await _candidateProfileBusiness.GetCandidateByEmail();
                }
                if (candidate != null&& layout != "Popup")
                {
                    if(candidate.PassportIssueCountryId == null)
                    { 
                        candidate.PassportIssueCountryId = ppIssueCountryId;
                    }                    
                    candidate.DataAction = DataActionEnum.Edit;
                    candidate.CurrentTabInfo = currentTabInfo;
                    ViewBag.JobAdvId = jobAdvId;
                    return View("Index", candidate);
                }
                if (taskid.IsNotNullAndNotEmpty())
                {
                    var task = await _recTaskBusiness.GetSingleById(taskid);
                    if (task.TextValue6.IsNotNullAndNotEmpty())
                    {
                        var model = await _candidateProfileBusiness.GetSingleById(task.TextValue6);
                        if (model.PassportIssueCountryId == null)
                        {
                            model.PassportIssueCountryId = ppIssueCountryId;
                        }
                        model.DataAction = DataActionEnum.Edit;
                        model.CurrentTabInfo = currentTabInfo;
                        ViewBag.JobAdvId = jobAdvId;
                        model.TaskId = taskid;
                        return View("Index", model);
                    }

                }
                
                return View("Index", new CandidateProfileViewModel {TaskId=taskid, DataAction = DataActionEnum.Create, PassportIssueCountryId = ppIssueCountryId });
            }

            else
            {
                var model = await _candidateProfileBusiness.GetSingleById(id);
                if(model.PassportIssueCountryId == null)
                {
                    model.PassportIssueCountryId = ppIssueCountryId;
                }  
                if(model.ExperienceLevelId.IsNotNullAndNotEmpty())
                {
                    var exp = await _listOfValueBusiness.GetSingleById(model.ExperienceLevelId);
                    if(exp.IsNotNull())
                    {
                        model.ExperienceLevelCode = exp.Code;
                    }
                }
                if (model.OtherQualificationId.IsNotNullAndNotEmpty())
                {
                    var qualifi = await _listOfValueBusiness.GetSingleById(model.OtherQualificationId);
                    if (qualifi.IsNotNull())
                    {
                        model.OtherQualificationCode = qualifi.Code;
                    }
                }
                model.DataAction = DataActionEnum.Edit;
                model.CurrentTabInfo = currentTabInfo;
                ViewBag.JobAdvId = jobAdvId;
                model.TaskId = taskid;
                return View("Index", model);
            }


        }
        public async Task<IActionResult> PrintableView(string candidateProfileId, string applicationId)
        {
            //var exist = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == candidateProfileId);
            if (applicationId != null)
            {
                var appmodel = await _applicationBusiness.GetApplicationDetails(applicationId);
                ViewBag.type = "application";
                appmodel.Criterias = await _applicationJobCriteriaBusiness.GetCriteriaData(applicationId, "Criteria");
                //appmodel.Criterias = await _applicationJobCriteriaBusiness.GetList(x => x.ApplicationId == applicationId && x.Type == "Criteria");
                appmodel.Skills = await _applicationJobCriteriaBusiness.GetCriteriaData(applicationId, "Skills");
                //appmodel.Skills = await _applicationJobCriteriaBusiness.GetList(x => x.ApplicationId == applicationId && x.Type == "Skills");
                appmodel.OtherInformations = await _applicationJobCriteriaBusiness.GetCriteriaData(applicationId, "OtherInformation");
                //appmodel.OtherInformations = await _applicationJobCriteriaBusiness.GetList(x => x.ApplicationId == applicationId && x.Type == "OtherInformation");
                               
                return View("_PrintableView", appmodel);
            }
            else
            {
                var candmodel = await _candidateProfileBusiness.GetCandProfileDetails(candidateProfileId);
                return View("_PrintableView", candmodel);
            }
        }
        public async Task<IActionResult> ApplicationComplete(string applicationNo)
        {
            return View(new ApplicationCompleteViewModel { ApplicationNo = applicationNo });
        }
        public async Task<IActionResult> ApplyJob(string jobAdvId, string candidateProfileId)
        {
            var exist = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == candidateProfileId && x.JobAdvertisementId == jobAdvId);
            if (exist != null)
            {
                
               
                var appmodel = await _applicationBusiness.GetApplicationDetails(candidateProfileId, jobAdvId);
                if (appmodel.IsNotNull())
                {
                    appmodel.JobAdvertisementId = jobAdvId;
                    appmodel.Criterias = _jobCriteriaBusiness.GetApplicationJobCriteriaByApplicationIdAndType(appmodel.Id, "Criteria").Result.ToList();
                    appmodel.CriteriasList = JsonConvert.SerializeObject(appmodel.Criterias);
                    appmodel.Skills = _jobCriteriaBusiness.GetApplicationJobCriteriaByApplicationIdAndType(appmodel.Id, "Skills").Result.ToList();
                    appmodel.SkillsList = JsonConvert.SerializeObject(appmodel.Skills);
                    appmodel.OtherInformations = _jobCriteriaBusiness.GetApplicationJobCriteriaByApplicationIdAndType(appmodel.Id, "OtherInformation").Result.ToList();
                    appmodel.OtherInformationsList = JsonConvert.SerializeObject(appmodel.OtherInformations);
                    var jobdetails = await _jobAdvertisementBusiness.GetNameById(jobAdvId);
                    if (jobdetails != null)
                    {
                        appmodel.JobName = jobdetails.JobName;
                        appmodel.JobCategoryName = jobdetails.JobCategoryName;
                        appmodel.JobLocationName = jobdetails.LocationName;

                    }
                }
               


                ViewBag.type = "application";
                return View(appmodel);
                
            }
            else
            {
                var candmodel = await _candidateProfileBusiness.GetCandProfileDetails(candidateProfileId);
                if (candmodel.IsNotNull())
                {
                    candmodel.JobAdvertisementId = jobAdvId;
                    candmodel.Criterias = _jobCriteriaBusiness.GetApplicationJobCriteriaByJobAndType(jobAdvId, "Criteria").Result.ToList();
                    candmodel.CriteriasList = JsonConvert.SerializeObject(candmodel.Criterias);
                    candmodel.Skills = _jobCriteriaBusiness.GetApplicationJobCriteriaByJobAndType(jobAdvId, "Skills").Result.ToList();
                    candmodel.SkillsList = JsonConvert.SerializeObject(candmodel.Skills);
                    candmodel.OtherInformations = _jobCriteriaBusiness.GetApplicationJobCriteriaByJobAndType(jobAdvId, "OtherInformation").Result.ToList();
                    candmodel.OtherInformationsList = JsonConvert.SerializeObject(candmodel.OtherInformations);
                    candmodel.SignatureDate = System.DateTime.Now;
                    var jobdetails = await _jobAdvertisementBusiness.GetNameById(jobAdvId);
                    if (jobdetails != null)
                    {
                        candmodel.JobName = jobdetails.JobName;
                        candmodel.JobCategoryName = jobdetails.JobCategoryName;
                        candmodel.JobLocationName = jobdetails.LocationName;

                    }
                   
                }
                ViewBag.type = "candidate";
                return View(candmodel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ManageApplication(CandidateProfileViewModel model)
        {
            if (model.IsNotNull())
            {
                var candmodel = await _candidateProfileBusiness.GetCandProfileDetails(model.Id);
                if (candmodel.IsNotNull())
                {
                    candmodel.IsCopyofQID = model.IsCopyofQID == true ? model.IsCopyofQID : candmodel.IsCopyofQID;
                    candmodel.QIDAttachmentId = model.QIDAttachmentId.IsNotNullAndNotEmpty() ? model.QIDAttachmentId : candmodel.QIDAttachmentId;
                    candmodel.QIDAttachmentId2 = model.QIDAttachmentId2.IsNotNullAndNotEmpty() ? model.QIDAttachmentId2 : candmodel.QIDAttachmentId2;
                    candmodel.QIDAttachmentId3 = model.QIDAttachmentId3.IsNotNullAndNotEmpty() ? model.QIDAttachmentId3 : candmodel.QIDAttachmentId3;
                    candmodel.QIDAttachmentId4 = model.QIDAttachmentId4.IsNotNullAndNotEmpty() ? model.QIDAttachmentId4 : candmodel.QIDAttachmentId4;
                    candmodel.QIDAttachmentId5 = model.QIDAttachmentId5.IsNotNullAndNotEmpty() ? model.QIDAttachmentId5 : candmodel.QIDAttachmentId5;
                    
                    candmodel.IsCopyofIDPassport = model.IsCopyofIDPassport == true ? model.IsCopyofIDPassport : candmodel.IsCopyofIDPassport;
                    candmodel.PassportAttachmentId = model.PassportAttachmentId.IsNotNullAndNotEmpty() ? model.PassportAttachmentId : candmodel.PassportAttachmentId;
                    candmodel.PassportAttachmentId2 = model.PassportAttachmentId2.IsNotNullAndNotEmpty() ? model.PassportAttachmentId2 : candmodel.PassportAttachmentId2;
                    candmodel.PassportAttachmentId3 = model.PassportAttachmentId3.IsNotNullAndNotEmpty() ? model.PassportAttachmentId3 : candmodel.PassportAttachmentId3;
                    candmodel.PassportAttachmentId4 = model.PassportAttachmentId4.IsNotNullAndNotEmpty() ? model.PassportAttachmentId4 : candmodel.PassportAttachmentId4;
                    candmodel.PassportAttachmentId5 = model.PassportAttachmentId5.IsNotNullAndNotEmpty() ? model.PassportAttachmentId5 : candmodel.PassportAttachmentId5;
                    
                    candmodel.IsCopyofAcademicCertificates = model.IsCopyofAcademicCertificates == true ? model.IsCopyofAcademicCertificates : candmodel.IsCopyofAcademicCertificates;
                    candmodel.AcademicCertificateId = model.AcademicCertificateId.IsNotNullAndNotEmpty() ? model.AcademicCertificateId : candmodel.AcademicCertificateId;
                    candmodel.AcademicCertificateId2 = model.AcademicCertificateId2.IsNotNullAndNotEmpty() ? model.AcademicCertificateId2 : candmodel.AcademicCertificateId2;
                    candmodel.AcademicCertificateId3 = model.AcademicCertificateId3.IsNotNullAndNotEmpty() ? model.AcademicCertificateId3 : candmodel.AcademicCertificateId3;
                    candmodel.AcademicCertificateId4 = model.AcademicCertificateId4.IsNotNullAndNotEmpty() ? model.AcademicCertificateId4 : candmodel.AcademicCertificateId4;
                    candmodel.AcademicCertificateId5 = model.AcademicCertificateId5.IsNotNullAndNotEmpty() ? model.AcademicCertificateId5 : candmodel.AcademicCertificateId5;

                    candmodel.IsCopyofOtherCertificates = model.IsCopyofOtherCertificates == true ? model.IsCopyofOtherCertificates : candmodel.IsCopyofOtherCertificates;
                    candmodel.OtherCertificateId = model.OtherCertificateId.IsNotNullAndNotEmpty() ? model.OtherCertificateId : candmodel.OtherCertificateId;
                    candmodel.OtherCertificateId2 = model.OtherCertificateId2.IsNotNullAndNotEmpty() ? model.OtherCertificateId2 : candmodel.OtherCertificateId2;
                    candmodel.OtherCertificateId3 = model.OtherCertificateId3.IsNotNullAndNotEmpty() ? model.OtherCertificateId3 : candmodel.OtherCertificateId3;
                    candmodel.OtherCertificateId4 = model.OtherCertificateId4.IsNotNullAndNotEmpty() ? model.OtherCertificateId4 : candmodel.OtherCertificateId4;
                    candmodel.OtherCertificateId5 = model.OtherCertificateId5.IsNotNullAndNotEmpty() ? model.OtherCertificateId5 : candmodel.OtherCertificateId5;

                    candmodel.ResumeId = model.ResumeId.IsNotNullAndNotEmpty() ? model.ResumeId : candmodel.ResumeId;
                    candmodel.IsMostRecentCV = model.IsMostRecentCV == true ? model.IsMostRecentCV : candmodel.IsMostRecentCV;
                    candmodel.CoverLetterId = model.CoverLetterId.IsNotNullAndNotEmpty() ? model.CoverLetterId : candmodel.CoverLetterId;
                    candmodel.IsLatestOfferLetterSalarySlip = model.IsLatestOfferLetterSalarySlip == true ? model.IsLatestOfferLetterSalarySlip : candmodel.IsLatestOfferLetterSalarySlip;
                    candmodel.PhotoId = model.PhotoId.IsNotNullAndNotEmpty() ? model.PhotoId : candmodel.PhotoId;
                    candmodel.IsMostRecentColorPhoto = model.IsMostRecentColorPhoto == true ? model.IsMostRecentColorPhoto : candmodel.IsMostRecentColorPhoto;
                    var result = await _candidateProfileBusiness.Edit(candmodel);
                }
                model.Criterias = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(model.CriteriasList);
                model.Skills = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(model.SkillsList);
                //model.OtherInformations = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(model.OtherInformationsList);
                var app = await _applicationBusiness.UpdateApplication(model);
                var appno = app.Item.ApplicationNo.IsNotNull() ? app.Item.ApplicationNo : app.Item.Id;
                return Json(new { success = true, applicationNo = appno });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidateApplicationsCount(string candidateProfileId)
        {
            var result = await _applicationBusiness.GetList(x => x.CandidateProfileId == candidateProfileId);
            if(result.Count >= 3)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ManageCandidateProfile(CandidateProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var passportexist = await _candidateProfileBusiness.GetSingle(x => x.PassportNumber == model.PassportNumber);
                    if (passportexist != null)
                    {
                        var errorList = new Dictionary<string, string>();
                        errorList.Add("Validate", "Passport Number Already Exist");
                        ModelState.AddModelErrors(errorList);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                    var result = await _candidateProfileBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("index",new {id= result.Item.Id });
                        //return PopupRedirect("Portal created successfully");
                        return Json(new { success = true, candidateProfileId = result.Item.Id,currentTabInfo=model.CurrentTabInfo });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var candexp = await _candidateExperienceBusiness.GetCandidateExperienceDuration(model.Id);
                    if (candexp.IsNotNull())
                    {
                        if (candexp.TotalDuration.IsNotNull())
                        {
                            model.TotalWorkExperience = candexp.TotalDuration.Value;
                        }
                    }
                    var passportexist = await _candidateProfileBusiness.GetSingle(x => x.Id != model.Id && x.PassportNumber == model.PassportNumber);
                    if(passportexist!=null)
                    {
                        var errorList = new Dictionary<string, string>();
                        errorList.Add("Validate", "Passport Number Already Exist");
                        ModelState.AddModelErrors(errorList);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                    var existingModel = await _candidateProfileBusiness.GetSingle(x => x.Id == model.Id);
                    if (existingModel!=null)
                    {
                        if (model.UserId.IsNullOrEmpty())
                        {
                            model.UserId = existingModel.UserId;
                        }
                        if (model.SourceFrom.IsNullOrEmpty())
                        {
                            model.SourceFrom = existingModel.SourceFrom;
                        }
                    }
                    var result = await _candidateProfileBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("index", new { id = result.Item.Id });
                        //return PopupRedirect("Portal edited successfully");
                        var applist = await _applicationBusiness.GetList(x=>x.CandidateProfileId==model.Id);
                        foreach(var app in applist)
                        {
                           
                            if (model.CurrentTabInfo == "CandidateInfo")
                            {
                                var appEdit = await _applicationBusiness.GetSingleById(app.Id);
                                appEdit.DataAction = DataActionEnum.Edit;

                                appEdit.FirstName = model.FirstName;
                                appEdit.MiddleName = model.MiddleName;
                                appEdit.LastName = model.LastName;
                                appEdit.TitleId = model.TitleId;
                                appEdit.Age = model.Age;
                                appEdit.BirthDate = model.BirthDate;
                                appEdit.BirthPlace = model.BirthPlace;
                                appEdit.BloodGroup = model.BloodGroup;
                                appEdit.Gender = model.Gender;
                                appEdit.MaritalStatus = model.MaritalStatus;
                                appEdit.PassportNumber = model.PassportNumber;
                                appEdit.PassportIssueCountryId = model.PassportIssueCountryId;
                                appEdit.PassportExpiryDate = model.PassportExpiryDate;
                                appEdit.QatarId = model.QatarId;
                                appEdit.VisaCountry = model.VisaCountry;
                                appEdit.VisaType = model.VisaType;
                                appEdit.VisaExpiry = model.VisaExpiry;
                                appEdit.OtherCountryVisa = model.OtherCountryVisa;
                                appEdit.OtherCountryVisaType = model.OtherCountryVisaType;
                                appEdit.OtherCountryVisaExpiry = model.OtherCountryVisaExpiry;
                                appEdit.ContactPhoneHome = model.ContactPhoneHome;
                                appEdit.ContactPhoneLocal = model.ContactPhoneLocal;


                                appEdit.IsCopyofQID = model.IsCopyofQID;
                                appEdit.QIDAttachmentId = model.QIDAttachmentId;
                                appEdit.QIDAttachmentId2 = model.QIDAttachmentId2;
                                appEdit.QIDAttachmentId3 = model.QIDAttachmentId3;
                                appEdit.QIDAttachmentId4 = model.QIDAttachmentId4;
                                appEdit.QIDAttachmentId5 = model.QIDAttachmentId5;

                                appEdit.IsCopyofIDPassport = model.IsCopyofIDPassport;
                                appEdit.PassportAttachmentId = model.PassportAttachmentId;
                                appEdit.PassportAttachmentId2 = model.PassportAttachmentId2;
                                appEdit.PassportAttachmentId3 = model.PassportAttachmentId3;
                                appEdit.PassportAttachmentId4 = model.PassportAttachmentId4;
                                appEdit.PassportAttachmentId5 = model.PassportAttachmentId5;

                                appEdit.IsCopyofAcademicCertificates = model.IsCopyofAcademicCertificates;
                                appEdit.AcademicCertificateId = model.AcademicCertificateId;
                                appEdit.AcademicCertificateId2 = model.AcademicCertificateId2;
                                appEdit.AcademicCertificateId3 = model.AcademicCertificateId3;
                                appEdit.AcademicCertificateId4 = model.AcademicCertificateId4;
                                appEdit.AcademicCertificateId5 = model.AcademicCertificateId5;

                                appEdit.IsCopyofOtherCertificates = model.IsCopyofOtherCertificates;
                                appEdit.OtherCertificateId = model.OtherCertificateId;
                                appEdit.OtherCertificateId2 = model.OtherCertificateId2;
                                appEdit.OtherCertificateId3 = model.OtherCertificateId3;
                                appEdit.OtherCertificateId4 = model.OtherCertificateId4;
                                appEdit.OtherCertificateId5 = model.OtherCertificateId5;

                                appEdit.ResumeId = model.ResumeId;
                                appEdit.IsMostRecentCV = model.IsMostRecentCV;
                                appEdit.CoverLetterId = model.CoverLetterId;
                                appEdit.IsLatestOfferLetterSalarySlip = model.IsLatestOfferLetterSalarySlip;
                                appEdit.PhotoId = model.PhotoId;
                                appEdit.IsMostRecentColorPhoto = model.IsMostRecentColorPhoto;
                                
                                appEdit.PermanentAddressCity = model.PermanentAddressCity;
                                appEdit.PermanentAddressHouse = model.PermanentAddressHouse;
                                appEdit.PermanentAddressStreet = model.PermanentAddressStreet;
                                appEdit.PermanentAddressState = model.PermanentAddressState;
                                appEdit.PermanentAddressCountryId = model.PermanentAddressCountryId;

                                appEdit.CurrentAddressHouse = model.CurrentAddressHouse;
                                appEdit.CurrentAddressCity = model.CurrentAddressCity;
                                appEdit.CurrentAddressState = model.CurrentAddressState;
                                appEdit.CurrentAddressStreet = model.CurrentAddressStreet;
                                appEdit.CurrentAddressCountryId = model.CurrentAddressCountryId;


                                await _applicationBusiness.Edit(appEdit);
                            }
                            if (model.CurrentTabInfo == "EmploymentInfo")
                            {
                                await _applicationBusiness.UpdateApplicationExperienceWhenProfileUpdate(model.Id, app.Id);

                                var appEdit = await _applicationBusiness.GetSingleById(app.Id);
                                appEdit.DataAction = DataActionEnum.Edit;

                                appEdit.TotalWorkExperience = model.TotalWorkExperience;
                                await _applicationBusiness.Edit(appEdit);

                            }
                            if (model.CurrentTabInfo == "QualificationInfo")
                            {
                                await _applicationBusiness.UpdateApplicationEducationWhenProfileUpdate(model.Id, app.Id);
                            }
                        }
                        return Json(new { success = true, candidateProfileId = result.Item.Id, currentTabInfo = model.CurrentTabInfo });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            //return View("index", model);
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public IActionResult CreateCandidateLanguageProf(string candidateProfileId)
        {
            var model = new CandidateLanguageProficiencyViewModel();
            model.DataAction = DataActionEnum.Create;
            model.CandidateProfileId = candidateProfileId;
            return View("_ManageCandidateLanguageProf", model);
        }
        public async Task<IActionResult> EditCandidateLanguageProf(string id)
        {
            var model = await _candidateLanguageProficiencyBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateLanguageProf", model);
        }
        public async Task<IActionResult> DeleteCandidateLanguageProf(string Id)
        {
            await _candidateLanguageProficiencyBusiness.Delete(Id);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ManageCandidateLanguageProf(CandidateLanguageProficiencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateLanguageProficiencyBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateLanguageProficiencyBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("_ManageCandidateLanguageProf", model);
        }

        public async Task<JsonResult> ReadCandidateLanguageProfData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var applangList = await _applicationLanguageProficiencyBusiness.GetListByApplication(candidateProfileId);
                var applangResult = applangList.ToDataSourceResult(request);
                return Json(applangResult);
            }
            else
            {
                //var modelList = new List<CandidateLanguageProficiencyViewModel>();
                var modelList = await _candidateLanguageProficiencyBusiness.GetListByCandidate(candidateProfileId);
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }

        public IActionResult CreateCandidateComputerProf(string candidateProfileId)
        {
            var model = new CandidateComputerProficiencyViewModel();
            model.DataAction = DataActionEnum.Create;
            model.CandidateProfileId = candidateProfileId;
            return View("_ManageCandidateComputerProf", model);
        }
        public async Task<IActionResult> EditCandidateComputerProf(string id)
        {
            var model = await _candidateComputerProficiencyBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateComputerProf", model);
        }
        public async Task<IActionResult> DeleteCandidateComputerProf(string Id)
        {
            await _candidateComputerProficiencyBusiness.Delete(Id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateComputerProf(CandidateComputerProficiencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateComputerProficiencyBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateComputerProficiencyBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("_ManageCandidateComputerProf", model);
        }
        public async Task<JsonResult> ReadCandidateComputerProfData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appcompList = await _applicationComputerProficiencyBusiness.GetListByApplication(candidateProfileId);
                var appcompResult = appcompList.ToDataSourceResult(request);
                return Json(appcompResult);
            }
            else
            {
                //var modelList = new List<CandidateComputerProficiencyViewModel>();
                var modelList = await _candidateComputerProficiencyBusiness.GetListByCandidate(candidateProfileId);
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }
        public IActionResult CreateCandidateEducational(string candidateProfileId, QualificationTypeEnum qualificationType)
        {
            var model = new CandidateEducationalViewModel();
            model.DataAction = DataActionEnum.Create;
            model.QualificationType = qualificationType;
            model.CandidateProfileId = candidateProfileId;
            return View("_ManageCandidateEducational", model);
        }
        public async Task<IActionResult> EditCandidateEducational(string id)
        {
            var model = await _candidateEducationalBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateEducational", model);
        }
        public async Task<IActionResult> DeleteCandidateEducational(string Id)
        {
            await _candidateEducationalBusiness.Delete(Id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateEducational(CandidateEducationalViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateEducationalBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateEducationalBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("_ManageCandidateEducational", model);
        }
        public async Task<JsonResult> ReadCandidateEducationalData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appEduList = await _applicationEducationalBusiness.GetListByApplication(QualificationTypeEnum.Educational, candidateProfileId);
                foreach (var l in appEduList)
                {
                    if (l.OtherQualification != null)
                    {
                        l.QualificationName = l.OtherQualification;
                    }
                    if (l.OtherSpecialization != null)
                    {
                        l.SpecializationName = l.OtherSpecialization;
                    }
                    if (l.OtherEducationType != null)
                    {
                        l.EducationTypeName = l.OtherEducationType;
                    }
                }
                var appEduResult = appEduList.ToDataSourceResult(request);
                return Json(appEduResult);
            }
            else
            {
                var modelList = await _candidateEducationalBusiness.GetListByCandidate(QualificationTypeEnum.Educational, candidateProfileId);
                foreach (var l in modelList)
                {
                    if(l.OtherQualification !=null)
                    {
                        l.QualificationName = l.OtherQualification;
                    }
                    if(l.OtherSpecialization !=null)
                    {
                        l.SpecializationName = l.OtherSpecialization;
                    }
                    if (l.OtherEducationType != null)
                    {
                        l.EducationTypeName = l.OtherEducationType;
                    }
                }
                var dsResult = modelList.ToDataSourceResult(request);
                
                return Json(dsResult);
            }
        }
        public async Task<JsonResult> ReadCandidateCertificationsData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appcertList = await _applicationEducationalBusiness.GetListByApplication(QualificationTypeEnum.Certifications, candidateProfileId);
                var dsResult = appcertList.ToDataSourceResult(request);
                return Json(dsResult);
            }
            else
            {
                var modelList = await _candidateEducationalBusiness.GetListByCandidate(QualificationTypeEnum.Certifications, candidateProfileId);
                foreach (var l in modelList)
                {
                    if (l.OtherQualification != null)
                    {
                        l.QualificationName = l.OtherQualification;
                    }
                    if (l.OtherSpecialization != null)
                    {
                        l.SpecializationName = l.OtherSpecialization;
                    }
                    if (l.OtherEducationType != null)
                    {
                        l.EducationTypeName = l.OtherEducationType;
                    }
                }
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }
        public async Task<JsonResult> ReadCandidateTrainingsData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var apptraininglist = await _applicationEducationalBusiness.GetListByApplication(QualificationTypeEnum.Trainings, candidateProfileId);
                var apptrainingResult = apptraininglist.ToDataSourceResult(request);
                return Json(apptrainingResult);
            }
            else
            {
                var modelList = await _candidateEducationalBusiness.GetListByCandidate(QualificationTypeEnum.Trainings, candidateProfileId);
                foreach (var l in modelList)
                {
                    if (l.OtherQualification != null)
                    {
                        l.QualificationName = l.OtherQualification;
                    }
                    if (l.OtherSpecialization != null)
                    {
                        l.SpecializationName = l.OtherSpecialization;
                    }
                    if (l.OtherEducationType != null)
                    {
                        l.EducationTypeName = l.OtherEducationType;
                    }
                }
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }
        public async Task<JsonResult> ReadCandidateExperienceData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var expmodel = await _applicationExperienceBusiness.GetListByApplication(candidateProfileId);
                var expresult = expmodel.ToDataSourceResult(request);
                return Json(expresult);
            }
            else
            {
                var modelList = await _candidateExperienceBusiness.GetListByCandidate(candidateProfileId);
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }
        public async Task<JsonResult> ReadCandidateExpByNatureData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appmodel = await _applicationExperienceByNatureBusiness.GetList(x => x.ApplicationId == candidateProfileId);
                var appmodelList = appmodel.OrderBy(x => x.SequenceOrder);
                var applresult = appmodelList.ToDataSourceResult(request);
                return Json(applresult);
            }
            else
            {
                var candmodel = await _candidateExperienceByNatureBusiness.GetList(x => x.CandidateProfileId == candidateProfileId);
                var candmodelList = candmodel.OrderBy(x => x.SequenceOrder);
                var dsResult = candmodelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }

        public async Task<JsonResult> ReadCandidateExpBySectorList([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appbysectorlist = await _applicationExperienceBySectorBusiness.GetListByApplication(candidateProfileId);
                var appbysectorResult = appbysectorlist.ToDataSourceResult(request);
                return Json(appbysectorResult);
            }
            else
            {
                var modelList = await _candidateExperienceBySectorBusiness.GetListByCandidate(candidateProfileId);
                //var modelList = new List<CandidateExperienceBySectorViewModel>();
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }
        public async Task<JsonResult> ReadCandidateExpByOtherList([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appbysectorlist = await _applicationExperienceByOtherBusiness.GetListByApplication(candidateProfileId);
                var appbysectorResult = appbysectorlist.ToDataSourceResult(request);
                return Json(appbysectorResult);
            }
            else
            {
                var modelList = await _candidateExperienceByOtherBusiness.GetNameByCandidate(candidateProfileId);
                //var modelList = new List<CandidateExperienceBySectorViewModel>();
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
            
        }
        public async Task<JsonResult> GetCountryIdNameList()
        {
            //var list = new List<IdNameViewModel>();
            //list.Add(new IdNameViewModel { Id = "60125324d63ca4dd574e8c99", Name = "UAE" });
            //list.Add(new IdNameViewModel { Id = "60125324d63ca4dd574e8c98", Name = "INDIA" });
            var list = await _applicationBusiness.GetCountryIdNameList();
            return Json(list);
        }

        public async Task<JsonResult> GetNationalityIdNameList()
        {
            //var list = new List<IdNameViewModel>();
            //list.Add(new IdNameViewModel { Id = "60125324d63ca4dd574e8c99", Name = "UAE" });
            //list.Add(new IdNameViewModel { Id = "60125324d63ca4dd574e8c98", Name = "INDIA" });
            var list = await _applicationBusiness.GetNationalityIdNameList();
            return Json(list);
        }

        public IActionResult CreateCandidateExperience(string candidateProfileId)
        {
            var model = new CandidateExperienceViewModel();
            model.CandidateProfileId = candidateProfileId;
            model.DataAction = DataActionEnum.Create;
            return View("_ManageCandidateExperience", model);
        }
        public async Task<IActionResult> EditCandidateExperience(string id)
        {
            var model = await _candidateExperienceBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateExperience", model);
        }
        public async Task<IActionResult> DeleteCandidateExperience(string Id)
        {
            await _candidateExperienceBusiness.Delete(Id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateExperience(CandidateExperienceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateExperienceBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateExperienceBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_ManageCandidateExperience", model);
        }

        //[HttpGet]
        //public async Task<JsonResult> GetLocationList()
        //{
        //    //var data = await _submoduleBusiness.GetList();
        //    //var res = from d in data
        //    //          where d.Status != StatusEnum.Inactive
        //    //          select d;
        //    return Json(res);
        //}
        //[HttpGet]
        //public async Task<JsonResult> GetPositionList()
        //{
        //    //var data = await _submoduleBusiness.GetList();
        //    //var res = from d in data
        //    //          where d.Status != StatusEnum.Inactive
        //    //          select d;
        //    return Json(res);
        //}
        public IActionResult CreateCandExpByCountry(string candidateProfileId)
        {
            var model = new CandidateExperienceByCountryViewModel();
            model.DataAction = DataActionEnum.Create;
            model.CandidateProfileId = candidateProfileId;
            return View("_ManageCandExpByCountry", model);
        }
        public async Task<IActionResult> EditCandExpByCountry(string id)
        {
            var model = await _candidateExperienceByCountryBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandExpByCountry", model);
        }
        public async Task<IActionResult> DeleteCandExpByCountry(string Id)
        {
            await _candidateExperienceByCountryBusiness.Delete(Id);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ManageCandidateExperienceByCountry(CandidateExperienceByCountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateExperienceByCountryBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateExperienceByCountryBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_ManageCandExpByCountry", model);
        }
        public async Task<JsonResult> ReadCandExpByCountryData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var expbycountryList = await _applicationExperienceByCountryBusiness.GetListByApplication(candidateProfileId);
                var dsResult = expbycountryList.ToDataSourceResult(request);
                return Json(dsResult);
            }
            else
            {
                //var modelList = await _candidateExperienceByCountryBusiness.GetList(x => x.CandidateProfileId == candidateProfileId);
                var modelList = await _candidateExperienceByCountryBusiness.GetListByCandidate(candidateProfileId);
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }

        }

        public IActionResult CreateCandExpByJob(string candidateProfileId)
        {
            var model = new CandidateExperienceByJobViewModel();
            model.DataAction = DataActionEnum.Create;
            model.CandidateProfileId = candidateProfileId;
            return View("_ManageCandExpByJob", model);
        }
        public async Task<IActionResult> EditCandExpByJob(string id)
        {
            var model = await _candidateExperienceByJobBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandExpByJob", model);
        }
        public async Task<IActionResult> DeleteCandExpByJob(string Id)
        {
            await _candidateExperienceByJobBusiness.Delete(Id);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ManageCandidateExperienceByJob(CandidateExperienceByJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateExperienceByJobBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateExperienceByJobBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("_ManageCandExpByJob", model);
        }
        public async Task<JsonResult> ReadCandExpByJobData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appjoblist = await _applicationExperienceByJobBusiness.GetListByApplication(candidateProfileId);
                var appjobresult = appjoblist.ToDataSourceResult(request);
                return Json(appjobresult);
            }
            else
            {
                var modelList = await _candidateExperienceByJobBusiness.GetListByCandidate(candidateProfileId);
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }
        public async Task<JsonResult> GetJobIdList()
        {
            // var data = await _batchBusiness.GetList();
            List<CandidateExperienceByJobViewModel> res = new List<CandidateExperienceByJobViewModel>();
            res.Add(new CandidateExperienceByJobViewModel { JobId = "3110" });
            res.Add(new CandidateExperienceByJobViewModel { JobId = "9444" });
            res.Add(new CandidateExperienceByJobViewModel { JobId = "7777" });

            return Json(res);
        }

        public IActionResult CreateCandidateExpByNature(string candidateProfileId)
        {
            var model = new CandidateExperienceByNatureViewModel();
            model.CandidateProfileId = candidateProfileId;
            model.DataAction = DataActionEnum.Create;
            return View("_ManageCandidateExpByNature", model);
        }
        public async Task<IActionResult> EditCandidateExpByNature(string id)
        {
            var model = await _candidateExperienceByNatureBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateExpByNature", model);
        }
        public async Task<IActionResult> DeleteCandidateExpByNature(string Id)
        {
            await _candidateExperienceByNatureBusiness.Delete(Id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateExpByNature(CandidateExperienceByNatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateExperienceByNatureBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateExperienceByNatureBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_ManageCandidateExpByNature", model);
        }

        //Candidate Experience By Sector//
        public IActionResult CreateCandidateExpBySector(string candidateProfileId)
        {
            var model = new CandidateExperienceBySectorViewModel();
            model.CandidateProfileId = candidateProfileId;
            model.DataAction = DataActionEnum.Create;
            return View("_ManageCandidateExpBySector", model);
        }
        public async Task<IActionResult> EditCandidateExpBySector(string id)
        {
            var model = await _candidateExperienceBySectorBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateExpBySector", model);
        }
        public async Task<IActionResult> DeleteCandidateExpBySector(string Id)
        {
            await _candidateExperienceBySectorBusiness.Delete(Id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateExpBySector(CandidateExperienceBySectorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateExperienceBySectorBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateExperienceBySectorBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_ManageCandidateExpBySector", model);
        }
        public IActionResult CreateCandidateExpByOther(string candidateProfileId)
        {
            var model = new CandidateExperienceByOtherViewModel();
            model.CandidateProfileId = candidateProfileId;
            model.DataAction = DataActionEnum.Create;
            return View("_ManageCandidateExpByOther", model);
        }
        public async Task<IActionResult> EditCandidateExpByOther(string id)
        {
            var model = await _candidateExperienceByOtherBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateExpByOther", model);
        }
        public async Task<IActionResult> DeleteCandidateExpByOther(string Id)
        {
            await _candidateExperienceByOtherBusiness.Delete(Id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateExpByOther(CandidateExperienceByOtherViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateExperienceByOtherBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateExperienceByOtherBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_ManageCandidateExpByOther", model);
        }
        public IActionResult CreateCandRefer(string candidateProfileId)
        {
            var model = new CandidateReferencesViewModel();
            model.DataAction = DataActionEnum.Create;
            model.CandidateProfileId = candidateProfileId;
            return View("_ManageCandRefer", model);
        }
        public async Task<IActionResult> EditCandRefer(string id)
        {
            var model = await _candidateReferencesBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandRefer", model);
        }
        public async Task<IActionResult> DeleteCandRefer(string Id)
        {
            await _candidateReferencesBusiness.Delete(Id);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ManageCandRefer(CandidateReferencesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateReferencesBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateReferencesBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("_ManageCandRefer", model);
        }
        public async Task<JsonResult> ReadCandRefer([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appmodel = await _applicationReferencesBusiness.GetList(x => x.ApplicationId == candidateProfileId);
                var appmodelList = appmodel.OrderBy(x => x.SequenceOrder);
                var dsResult = appmodelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
            else
            {
                var candmodel = await _candidateReferencesBusiness.GetList(x => x.CandidateProfileId == candidateProfileId);
                var candmodelList = candmodel.OrderBy(x => x.SequenceOrder);
                var dsResult = candmodelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }

        public async Task<JsonResult> ReadCandidateDrivingLicenseData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appDLList = await _applicationDrivingLicenseBusiness.GetLicenseListByApplication(candidateProfileId);
                var appDLResult = appDLList.ToDataSourceResult(request);
                return Json(appDLResult);
            }
            else
            {
                var modelList = await _candidateDrivingLicenseBusiness.GetLicenseListByCandidate(candidateProfileId);
                //var modelList = new List<CandidateDrivingLicenseViewModel>();
                var dsResult = modelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }

        public IActionResult CreateCandidateDrivingLicense(string candidateProfileId)
        {
            var model = new CandidateDrivingLicenseViewModel();
            model.CandidateProfileId = candidateProfileId;
            model.DataAction = DataActionEnum.Create;
            return View("_ManageCandidateDrivingLicense", model);
        }
        public async Task<IActionResult> EditCandidateDrivingLicense(string id)
        {
            var model = await _candidateDrivingLicenseBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateDrivingLicense", model);
        }
        public async Task<IActionResult> DeleteCandidateDrivingLicense(string Id)
        {
            await _candidateDrivingLicenseBusiness.Delete(Id);
            return Json(new { success = true });
        }

        public async Task<JsonResult> GetCountryList()
        {
            var result = await _candidateDrivingLicenseBusiness.GetCountryListData();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandDrivingLicense(CandidateDrivingLicenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateDrivingLicenseBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateDrivingLicenseBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_ManageCandidateDrivingLicense", model);
        }
        public IActionResult CreateCandidateProject(string candidateProfileId)
        {
            var model = new CandidateProjectViewModel();
            model.CandidateProfileId = candidateProfileId;
            model.DataAction = DataActionEnum.Create;
            return View("_ManageCandidateProject", model);
        }
        public async Task<IActionResult> EditCandidateProject(string id)
        {
            var model = await _candidateProjectBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View("_ManageCandidateProject", model);
        }
        public async Task<IActionResult> DeleteCandidateProject(string Id)
        {
            await _candidateProjectBusiness.Delete(Id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateProject(CandidateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _candidateProjectBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _candidateProjectBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_ManageCandidateProject", model);
        }
        public async Task<JsonResult> ReadCandidateProjectData([DataSourceRequest] DataSourceRequest request, string candidateProfileId, string type)
        {
            if (type == "application")
            {
                var appmodel = await _applicationProjectBusiness.GetList(x => x.ApplicationId == candidateProfileId);
                var appmodelList = appmodel.OrderBy(x => x.SequenceOrder);
                var dsResult = appmodelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
            else
            {
                var candmodel = await _candidateProjectBusiness.GetList(x => x.CandidateProfileId == candidateProfileId);
                var candmodelList = candmodel.OrderBy(x => x.SequenceOrder);
                var dsResult = candmodelList.ToDataSourceResult(request);
                return Json(dsResult);
            }
        }

        public async Task<IActionResult> GetDocuments(string candidateProfileId, string applicationId)
        {
            if (applicationId.IsNullOrEmpty())
            {
                var docList = await _candidateProfileBusiness.GetDocumentsByCandidate(candidateProfileId);
                var list = await _candidateExperienceBusiness.GetListByCandidate(candidateProfileId);
                docList.ExperienceDoc = list.Where(x => x.AttachmentId != null).ToList();
                return View("_ViewDocuments", docList);
            }
            else
            {
                var appdocList = await _applicationBusiness.GetDocumentsByApplication(applicationId);
                var list = await _applicationExperienceBusiness.GetListByApplication(applicationId);
                appdocList.AppExperienceDoc = list.Where(x => x.AttachmentId != null).ToList();
                return View("_ViewDocuments", appdocList);
            }

        }
        public IActionResult CandidateReport()
        {           
            return View();
        }
        public async Task<JsonResult> ReadCandidateReportData([DataSourceRequest] DataSourceRequest request)
        {           
            var modelList = await _applicationBusiness.GetCandidateReportData();
            var dsResult = modelList.ToDataSourceResult(request);
            return Json(dsResult);
            
        }
        [HttpGet]
        public async Task<IActionResult> GetCandidateExperienceDuration(string candidateProfileId)
        {
            var candexp = await _candidateExperienceBusiness.GetCandidateExperienceDuration(candidateProfileId);
            double candexpTotal=0;
            if (candexp.IsNotNull())
            {
                if (candexp.TotalDuration.IsNotNull())
                {
                    candexpTotal = candexp.TotalDuration.Value;
                }
            }
            return Json(new { success = true, data = candexpTotal });
        }
        [HttpGet]
        public async Task<IActionResult> GetLOVCode(string lovId)
        {            
            var data = "";
            if (lovId.IsNotNullAndNotEmpty())
            {
                var lov = await _listOfValueBusiness.GetSingleById(lovId);
                if (lov.IsNotNull())
                {
                    data = lov.Code;
                }
            }
            return Json(new { success = true, data = data });
        }

        public IActionResult AllApplicationsList()
        {
            return View();
        }
            
    }
}
