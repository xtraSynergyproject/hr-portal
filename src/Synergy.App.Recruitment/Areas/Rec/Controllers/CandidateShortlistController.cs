using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using Synergy.App.Business;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Common;
using System;
using Newtonsoft.Json;
using Synergy.App.DataModel;
using AutoMapper;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.Rec.Controllers
{
    [Area("Recruitment")]
    public class CandidateShortlistController : ApplicationController
    {
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly IApplicationVersionBusiness _applicationVersionBusiness;
        private readonly IJobAdvertisementBusiness _jobAdvrtismentBusiness;
        private readonly IBatchBusiness _batchBusiness;
        private readonly IApplicationStateCommentBusiness _applicationStateCommentBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ICandidateEvaluationBusiness _candidateEvaluationBusiness;
        private readonly ICandidateEvaluationVersionBusiness _candidateEvaluationVersionBusiness;
        private readonly IListOfValueBusiness _listOfValueBusiness;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        public CandidateShortlistController(IApplicationBusiness applicationBusiness, IJobAdvertisementBusiness jobAdvrtismentBusiness, IBatchBusiness batchBusiness,
            IApplicationStateCommentBusiness applicationStateCommentBusiness, IUserBusiness userBusiness,
            ICandidateEvaluationBusiness candidateEvaluationBusiness,
             IUserContext userContext, IListOfValueBusiness listOfValueBusiness, IMapper mapper
            , IApplicationVersionBusiness applicationVersionBusiness, ICandidateEvaluationVersionBusiness candidateEvaluationVersionBusiness)
        {
            _applicationBusiness = applicationBusiness;
            _jobAdvrtismentBusiness = jobAdvrtismentBusiness;
            _batchBusiness = batchBusiness;
            _applicationStateCommentBusiness = applicationStateCommentBusiness;
            _userBusiness = userBusiness;
            _candidateEvaluationBusiness = candidateEvaluationBusiness;
            _userContext = userContext;
            _listOfValueBusiness = listOfValueBusiness;
            _mapper = mapper;
            _applicationVersionBusiness = applicationVersionBusiness;
            _candidateEvaluationVersionBusiness = candidateEvaluationVersionBusiness;
        }
        public IActionResult Index(string jobAdvId, string orgId, string batchId, string code, LayoutModeEnum layoutMode = LayoutModeEnum.Main)
        {
            var model = new ApplicationSearchViewModel();
            model.OrganizationId = orgId;
            model.JobTitleForHiring = jobAdvId;
            model.BatchHiringManagerId = _userContext.UserId;
            model.BatchId = batchId;
            //return View("CandidateShortListByHM",model);
            ViewBag.Mode = code;
            ViewBag.Layout = "~/Areas/CMS/Views/Shared/_LayoutCms1.cshtml";
            if (layoutMode == LayoutModeEnum.None)
            {
                ViewBag.Layout = null;
            }
            return View(model);
        }

        public async Task<IActionResult> NotShortlistForInterview(string batchId)
        {
            var model = new ApplicationSearchViewModel();
            var batch = await _batchBusiness.GetSingleById(batchId);
            if (batch.IsNotNull())
            {
                model.OrganizationId = batch.OrganizationId;
                model.JobTitleForHiring = batch.JobId;
            }

            model.BatchId = batchId;
            model.BatchHiringManagerId = _userContext.UserId;
            return View("NotShortlistForInterview", model);
            // return View(model);
        }

        public async Task<IActionResult> AddComments(string applicationId)
        {
            var model = await _applicationBusiness.GetSingleById(applicationId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageApplicationComment(ApplicationViewModel model)
        {
            var data = await _applicationBusiness.GetSingleById(model.ApplicationId);
            data.ShortlistByHMComment = model.ShortlistByHMComment;
            var result = await _applicationBusiness.Edit(data);

            return Json(new { success = result.IsSuccess, error = result.Messages });
        }

        public async Task<IActionResult> ShortlistForInterview(string batchId)
        {
            var model = new ApplicationSearchViewModel();
            var batch = await _batchBusiness.GetSingleById(batchId);
            if (batch.IsNotNull())
            {
                model.OrganizationId = batch.OrganizationId;
                model.JobTitleForHiring = batch.JobId;
            }
            model.BatchId = batchId;
            model.BatchHiringManagerId = _userContext.UserId;
            return View("ShortlistForInterview", model);
            // return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetIdNameHmOrgList(string userId)
        {
            var data = await _applicationBusiness.GetIdNameHMOrganization(userId);
            return Json(data);
        }
        public async Task<IActionResult> CandidateShortlistByHR(string jobAdvId, string orgId, string batchId, LayoutModeEnum layoutMode = LayoutModeEnum.Popup, string status = null)
        {
            ApplicationSearchViewModel model = new ApplicationSearchViewModel();
            model.IssueDate = DateTime.Now;
            model.ExpiryDate = DateTime.Now;
            model.JobTitleForHiring = jobAdvId;
            ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            if (layoutMode == LayoutModeEnum.None)
            {
                ViewBag.Layout = null;
            }
            if (orgId.IsNotNullAndNotEmpty())
            {
                model.OrganizationId = orgId;
            }
            if (batchId.IsNotNullAndNotEmpty())
            {
                model.BatchId = batchId;
                var batch = await _batchBusiness.GetSingleById(batchId);
                if (batch != null)
                {
                    model.JobTitleForHiring = batch.JobId;
                    model.OrganizationId = batch.OrganizationId;
                }
            }
            if (status.IsNotNullAndNotEmpty())
            {
                // var list = await _listOfValueBusiness.GetList<IdNameViewModel, ApplicationStatus>(x => x.Code == "REJECTED");
                //var res = await _listOfValueBusiness.GetSingle<IdNameViewModel, ApplicationStatus>(x => x.Code == status);
                //model.ApplicationStatusId = res.Id;
                ViewBag.Pool = "RejectedPool";
            }
            return View(model);
        }
        public IActionResult WorkerPool(string jobAdvId, string orgId, bool isDashboard = false)
        {
            ApplicationSearchViewModel model = new ApplicationSearchViewModel();
            model.IssueDate = DateTime.Now;
            model.ExpiryDate = DateTime.Now;
            model.JobTitleForHiring = jobAdvId;
            model.IsDashboard = isDashboard;
            return View(model);
        }
        public async Task<IActionResult> CandidateEvaluationScaleReview(string applicationId, string taskStatus, string mode)
        {
            if (taskStatus == "COMPLETED")
            {
                return RedirectToAction("CandidateEvaluationView", new { applicationId = applicationId });
            }

            var model = await _applicationBusiness.GetApplicationEvaluationDetails(applicationId);
            model.Mode = mode;
            var manpowertype = await _jobAdvrtismentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
            model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;

            var evallist = await _candidateEvaluationBusiness.GetList(x => x.ApplicationId == applicationId);
            if (evallist != null && evallist.Count() > 0)
            {
                //model = await _applicationBusiness.GetApplicationEvaluationDetails(applicationId);
                //var evallist = await _candidateEvaluationBusiness.GetList(x => x.ApplicationId == model.Id);
                model.EvaluationData = evallist.OrderBy(x => x.SequenceOrder).ToList();
                model.EvaluationDataString = JsonConvert.SerializeObject(model.EvaluationData);
                //var manpowertype = await _jobAdvrtismentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                //model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
                model.IsCandidateEvaluation = true;
                model.DataAction = DataActionEnum.Edit;
                //if (model.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || model.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected || model.InterviewSelectionFeedback == InterviewFeedbackEnum.Rejected)
                //{
                //    model.DataAction = DataActionEnum.Read;
                //    return View("_CandidateEvaluationView", model);
                //}
            }
            else
            {
                //if (model.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || model.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected || model.InterviewSelectionFeedback == InterviewFeedbackEnum.Rejected)
                //{
                //    return RedirectToAction("CandidateEvaluationView", new { applicationId = applicationId });
                //}
                //model = await _applicationBusiness.GetSingleById(applicationId);
                //model.FullName = model.FirstName + " " + model.MiddleName + " " + model.LastName;
                evallist = await _candidateEvaluationBusiness.GetList(x => x.IsTemplate == true);
                model.EvaluationData = evallist.OrderBy(x => x.SequenceOrder).ToList();
                model.EvaluationDataString = JsonConvert.SerializeObject(model.EvaluationData);
                model.InterviewByUserId = _userContext.UserId;
                model.InterviewByUserName = _userContext.Name;
                //var manpowertype = await _jobAdvrtismentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                //model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
                model.DataAction = DataActionEnum.Edit;
                model.IsCandidateEvaluation = false;
            }

            return View("_CandidateEvaluationScaleReview", model);
        }
        public async Task<IActionResult> CandidateEvaluationView(string applicationId, string versionNo)
        {
            //applicationId = "c93b445e-4a79-416d-9ebd-2e9956f6eb87";
            //applicationId = "ebb1ba7f-0acb-4070-b069-dbc913c0a30e";
            if (versionNo.IsNotNullAndNotEmpty())
            {
                var model = await _applicationBusiness.GetApplicationVersionEvaluationDetails(applicationId);
                if (model != null)
                {
                    var evallist = await _candidateEvaluationVersionBusiness.GetList(x => x.ApplicationVersionId == model.Id);
                    var version = evallist.OrderBy(x => x.SequenceOrder).ToList();
                    model.EvaluationData = _mapper.Map<List<CandidateEvaluationVersionViewModel>, List<CandidateEvaluationViewModel>>(version);
                    var manpowertype = await _jobAdvrtismentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                    model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
                }
                model.DataAction = DataActionEnum.Read;
                return View("_CandidateEvaluationView", model);
            }
            else
            {
                var model = await _applicationBusiness.GetApplicationEvaluationDetails(applicationId);
                if (model != null)
                {
                    var evallist = await _candidateEvaluationBusiness.GetList(x => x.ApplicationId == model.Id);
                    model.EvaluationData = evallist.OrderBy(x => x.SequenceOrder).ToList();
                    var manpowertype = await _jobAdvrtismentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                    model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
                }
                model.DataAction = DataActionEnum.Read;
                return View("_CandidateEvaluationView", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateEvaluationScaleReview(ApplicationViewModel model)
        {

            var newModel = await _applicationBusiness.GetSingleById(model.Id);
            if (newModel != null)
            {
                newModel.JobId = model.JobId;
                newModel.DivisionId = model.DivisionId;
                newModel.OfferGrade = model.OfferGrade;
                newModel.Score = model.Score;
                newModel.InterviewSelectionFeedback = model.InterviewSelectionFeedback;
                newModel.SalaryOnAppointment = model.SalaryOnAppointment;
                newModel.HiringManagerRemarks = model.HiringManagerRemarks;
                newModel.EvaluationAttachmentId = model.EvaluationAttachmentId;
                newModel.AccommodationId = model.AccommodationId;
                newModel.InterviewDate = DateTime.Now;
                newModel.InterviewByUserId = model.InterviewByUserId;
                newModel.InterviewByUserName = model.InterviewByUserName;
                newModel.ManpowerTypeCode = model.ManpowerTypeCode;
                if ((newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected) && model.ManpowerTypeCode == "Staff")
                {
                    newModel.SelectedThroughId = model.SelectedThroughId;
                    newModel.InterviewVenue = model.InterviewVenue;
                    newModel.NewPostJustification = model.NewPostJustification;
                    newModel.DescribeHowHeSuits = model.DescribeHowHeSuits;
                    newModel.LeaveCycle = model.LeaveCycle;
                    newModel.OtherBenefits = model.OtherBenefits;
                    newModel.AppointmentRemarks = model.AppointmentRemarks;
                }

                newModel.DataAction = model.DataAction;

                if (newModel.DataAction == DataActionEnum.Edit)
                {
                    if (newModel.InterviewSelectionFeedback.IsNotNull())
                    {
                        var status = string.Empty;
                        if (newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected)
                        {
                            if (newModel.ManpowerTypeCode == "Staff")
                            {
                                status = "SHORTLISTED";
                            }
                            else
                            {
                                status = "SELECTED";
                                //status = "NotAddedToBatch";                               
                            }
                        }
                        else if (newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Rejected)
                        {
                            status = "REJECTED";
                        }
                        if (status.IsNotNullAndNotEmpty())
                        {
                            var status1 = await _applicationBusiness.GetApplicationStatusByCode(status);
                            newModel.ApplicationStatus = status1.Id;
                        }
                    }


                    var result = await _applicationBusiness.Edit(newModel);
                    if (result.IsSuccess)
                    {
                        var applicationversionId = "";
                        var check = await _applicationVersionBusiness.GetList(x => x.ApplicationId == newModel.Id);
                        if (model.Mode == "EDIT" && check.Count == 0)
                        {
                            var version = _mapper.Map<ApplicationViewModel, ApplicationVersionViewModel>(newModel);
                            version.Id = null;
                            version.DataAction = DataActionEnum.Create;
                            version.ApplicationId = newModel.Id;
                            version.VersionNo = 1;

                            var res = await _applicationVersionBusiness.Create(version);
                            applicationversionId = res.Item.Id;
                        }
                        if (model.ManpowerTypeCode == "Staff")
                        {
                            if (newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected)
                            {
                                //await _applicationBusiness.UpdateApplicationState(newModel.Id, "IntentToOffer");
                            }
                            else
                            {
                                //await _applicationBusiness.UpdateApplicationState(newModel.Id, "InterviewsCompleted");
                            }
                            //var status = await _applicationBusiness.CreateApplicationStatusTrack(newModel.Id);
                            if (true)
                            {
                                var marks = model.MarksSelections.Split(",");
                                var tmarks = JsonConvert.DeserializeObject<List<CandidateEvaluationViewModel>>(model.EvaluationDataString);
                                var i = 0;
                                foreach (var mark in marks)
                                {
                                    if (mark.IsNotNullAndNotEmpty())
                                    {
                                        var m = mark.Split("=");
                                        if (m[1] == "0")
                                        {
                                            tmarks[i].IsEvaluationScale1 = true;
                                            tmarks[i].IsEvaluationScale2 = false;
                                            tmarks[i].IsEvaluationScale3 = false;
                                            tmarks[i].Marks = "20";
                                            //tmarks[i].Id = null;
                                            tmarks[i].IsTemplate = false;
                                            tmarks[i].ApplicationId = model.Id;
                                            if (model.IsCandidateEvaluation)
                                            {
                                                var evalResult = await _candidateEvaluationBusiness.Edit(tmarks[i]);

                                            }
                                            else
                                            {
                                                tmarks[i].Id = null;
                                                var evalResult = await _candidateEvaluationBusiness.Create(tmarks[i]);
                                            }
                                            if (model.Mode == "EDIT" && check.Count == 0)
                                            {
                                                var evalversion = _mapper.Map<CandidateEvaluationViewModel, CandidateEvaluationVersionViewModel>(tmarks[i]);
                                                evalversion.Id = null;
                                                evalversion.ApplicationVersionId = applicationversionId;
                                                evalversion.VersionNo = 1;
                                                await _candidateEvaluationVersionBusiness.Create(evalversion);
                                            }
                                        }
                                        if (m[1] == "1")
                                        {
                                            tmarks[i].IsEvaluationScale1 = false;
                                            tmarks[i].IsEvaluationScale2 = true;
                                            tmarks[i].IsEvaluationScale3 = false;
                                            tmarks[i].Marks = "10";
                                            //tmarks[i].Id = null;
                                            tmarks[i].IsTemplate = false;
                                            tmarks[i].ApplicationId = model.Id;
                                            if (model.IsCandidateEvaluation)
                                            {
                                                var evalResult = await _candidateEvaluationBusiness.Edit(tmarks[i]);
                                            }
                                            else
                                            {
                                                tmarks[i].Id = null;
                                                var evalResult = await _candidateEvaluationBusiness.Create(tmarks[i]);
                                            }
                                            if (model.Mode == "EDIT" && check.Count == 0)
                                            {
                                                var evalversion = _mapper.Map<CandidateEvaluationViewModel, CandidateEvaluationVersionViewModel>(tmarks[i]);
                                                evalversion.Id = null;
                                                evalversion.ApplicationVersionId = applicationversionId;
                                                evalversion.VersionNo = 1;
                                                await _candidateEvaluationVersionBusiness.Create(evalversion);
                                            }
                                        }
                                        if (m[1] == "2")
                                        {
                                            tmarks[i].IsEvaluationScale1 = false;
                                            tmarks[i].IsEvaluationScale2 = false;
                                            tmarks[i].IsEvaluationScale3 = true;
                                            tmarks[i].Marks = "0";
                                            //tmarks[i].Id = null;
                                            tmarks[i].IsTemplate = false;
                                            tmarks[i].ApplicationId = model.Id;
                                            if (model.IsCandidateEvaluation)
                                            {
                                                var evalResult = await _candidateEvaluationBusiness.Edit(tmarks[i]);
                                            }
                                            else
                                            {
                                                tmarks[i].Id = null;
                                                var evalResult = await _candidateEvaluationBusiness.Create(tmarks[i]);
                                            }
                                            if (model.Mode == "EDIT" && check.Count == 0)
                                            {
                                                var evalversion = _mapper.Map<CandidateEvaluationViewModel, CandidateEvaluationVersionViewModel>(tmarks[i]);
                                                evalversion.Id = null;
                                                evalversion.ApplicationVersionId = applicationversionId;
                                                evalversion.VersionNo = 1;
                                                await _candidateEvaluationVersionBusiness.Create(evalversion);
                                            }
                                        }
                                        i++;
                                    }

                                }
                            }

                        }
                        else
                        {
                            //await _applicationBusiness.UpdateApplicationState(newModel.Id, "InterviewsCompleted");
                            if (newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected)
                            {
                                //await _applicationBusiness.UpdateApplicationState(newModel.Id, "WorkerPool");
                            }
                            else
                            {
                                //await _applicationBusiness.UpdateApplicationState(newModel.Id, "InterviewsCompleted");
                            }
                            //var status = await _applicationBusiness.CreateApplicationStatusTrack(newModel.Id);
                        }

                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }


            return View("_CandidateEvaluationScaleReview", model);
        }
        public async Task<IActionResult> CreateCandidateEvaluation()
        {
            //this method only for Back End Creation
            var model1 = new CandidateEvaluationViewModel();
            //1
            model1.EvaluationName = "ACADEMIC QUALIFICATION";
            model1.EvaluationScale1 = "Fully meets requirement";
            model1.IsEvaluationScale1 = false;
            model1.EvaluationScale2 = "Just meets the requirement";
            model1.IsEvaluationScale2 = false;
            model1.EvaluationScale3 = "Falls short of requirement";
            model1.IsEvaluationScale3 = false;
            model1.IsTemplate = true;
            model1.SequenceOrder = 1;
            model1.Status = StatusEnum.Active;
            var result1 = await _candidateEvaluationBusiness.Create(model1);
            if (result1.IsSuccess)
            {

            }

            var model2 = new CandidateEvaluationViewModel();
            //2
            model2.EvaluationName = "RELEVANT JOB EXPERIENCE AND KNOWLEDGE";
            model2.EvaluationScale1 = "Highly useful & knowledgeable";
            model2.IsEvaluationScale1 = false;
            model2.EvaluationScale2 = "Useful & knowledgeable";
            model2.IsEvaluationScale2 = false;
            model2.EvaluationScale3 = "Insignificant";
            model2.IsEvaluationScale3 = false;
            model2.IsTemplate = true;
            model2.SequenceOrder = 2;
            model2.Status = StatusEnum.Active;
            var result2 = await _candidateEvaluationBusiness.Create(model2);
            if (result2.IsSuccess)
            {

            }
            var model3 = new CandidateEvaluationViewModel();
            //3
            model3.EvaluationName = "KNOWLEDGE ON QUALITY ASPECTS";
            model3.EvaluationScale1 = "High";
            model3.IsEvaluationScale1 = false;
            model3.EvaluationScale2 = "Meets the requirement";
            model3.IsEvaluationScale2 = false;
            model3.EvaluationScale3 = "Poor";
            model3.IsEvaluationScale3 = false;
            model3.IsTemplate = true;
            model3.SequenceOrder = 3;
            model3.Status = StatusEnum.Active;
            var result3 = await _candidateEvaluationBusiness.Create(model3);
            if (result3.IsSuccess)
            {

            }
            var model4 = new CandidateEvaluationViewModel();
            //4
            model4.EvaluationName = "COMMUNICATION";
            model4.EvaluationScale1 = "Clear and confident";
            model4.IsEvaluationScale1 = false;
            model4.EvaluationScale2 = "Generally Good";
            model4.IsEvaluationScale2 = false;
            model4.EvaluationScale3 = "Lacks clarity/unconvincing";
            model4.IsEvaluationScale3 = false;
            model4.IsTemplate = true;
            model4.SequenceOrder = 4;
            model4.Status = StatusEnum.Active;
            var result4 = await _candidateEvaluationBusiness.Create(model4);
            if (result4.IsSuccess)
            {

            }
            var model5 = new CandidateEvaluationViewModel();
            //5
            model5.EvaluationName = "SAFETY AWARENESS";
            model5.EvaluationScale1 = "Meets requirement";
            model5.IsEvaluationScale1 = false;
            model5.EvaluationScale2 = "Average";
            model5.IsEvaluationScale2 = false;
            model5.EvaluationScale3 = "Poor";
            model5.IsEvaluationScale3 = false;
            model5.IsTemplate = true;
            model5.SequenceOrder = 5;
            model5.Status = StatusEnum.Active;
            var result5 = await _candidateEvaluationBusiness.Create(model5);
            if (result5.IsSuccess)
            {

            }
            var model6 = new CandidateEvaluationViewModel();
            //6
            model6.EvaluationName = "PRESENTABILITY";
            model6.EvaluationScale1 = "Impressive";
            model6.IsEvaluationScale1 = false;
            model6.EvaluationScale2 = "Acceptable";
            model6.IsEvaluationScale2 = false;
            model6.EvaluationScale3 = "Leaves poor inspiration";
            model6.IsEvaluationScale3 = false;
            model6.IsTemplate = true;
            model6.SequenceOrder = 6;
            model6.Status = StatusEnum.Active;
            var result6 = await _candidateEvaluationBusiness.Create(model6);
            if (result6.IsSuccess)
            {

            }
            var model7 = new CandidateEvaluationViewModel();
            //7
            model7.EvaluationName = "LOGICAL THINKING AND ANALYTICAL POWER";
            model7.EvaluationScale1 = "Impressive";
            model7.IsEvaluationScale1 = false;
            model7.EvaluationScale2 = "Acceptable";
            model7.IsEvaluationScale2 = false;
            model7.EvaluationScale3 = "Not suitable";
            model7.IsEvaluationScale3 = false;
            model7.IsTemplate = true;
            model7.SequenceOrder = 7;
            model7.Status = StatusEnum.Active;
            var result7 = await _candidateEvaluationBusiness.Create(model7);
            if (result7.IsSuccess)
            {

            }
            return View("_CandidateEvaluationScaleReview", model1);
        }
        public async Task<JsonResult> ReadInterviewCandidates([DataSourceRequest] DataSourceRequest request, ApplicationSearchViewModel search)
        {
            //var model = await _applicationBusiness.GetList(x => x.ApplicationState == "ShortlistedByHR");
            //search.ApplicationStateCode = "ShortListByHr";
            //search.BatchStatusCode = "PendingwithHM";
            //search.ApplicationStatusCode = "SHORTLISTED";
            search.ApplicationStateCode = "ShortListByHm";
            search.BatchStatusCode = "PendingwithHM";
            search.ApplicationStatusCode = "NotShortlisted";
            var model = await _applicationBusiness.GetCandiadteShortListApplicationData(search);
            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var data = model.ToDataSourceResult(request);
            return Json(data);
        }
        public async Task<JsonResult> ReadFutureCandidates([DataSourceRequest] DataSourceRequest request, ApplicationSearchViewModel search)
        {
            //var model = await _applicationBusiness.GetList(x => x.ApplicationState == "ShortlistedByHR");
            //search.ApplicationStateCode = "ShortListByHr";
            //search.BatchStatusCode = "PendingwithHM";
            //search.ApplicationStatusCode = "SHORTLISTED";
            search.ApplicationStateCode = "ShortListByHm";
            search.BatchStatusCode = "PendingwithHM";
            search.ApplicationStatusCode = "ShortlistForFuture";
            var model = await _applicationBusiness.GetCandiadteShortListApplicationData(search);
            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var data = model.ToDataSourceResult(request);
            return Json(data);
        }
        public async Task<JsonResult> ReadShortlistCandidatesData([DataSourceRequest] DataSourceRequest request, ApplicationSearchViewModel search)
        {
            var model = await _applicationBusiness.GetCandiadteShortListDataByHR(search);
            if (search.Pool.IsNotNullAndNotEmpty())
            {
                model = model.Where(x => x.ApplicationStatusCode == "REJECTED" || x.ApplicationStatusCode == "RejectedHM").ToList();
            }
            else
            {
                model = model.Where(x => x.ApplicationStatusCode != "REJECTED" && x.ApplicationStatusCode != "RejectedHM").ToList();
            }
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }
        public async Task<JsonResult> ReadWorkerPoolData([DataSourceRequest] DataSourceRequest request, ApplicationSearchViewModel search)
        {
            var model = await _applicationBusiness.GetWorkerPoolData(search);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }

        public ActionResult ViewBatchCandidate(string batchid, string type)
        {
            ViewBag.BatchId = batchid;
            ViewBag.Type = type;
            return View("_ViewBatchCandidate");

        }
        public async Task<JsonResult> ReadWorkerPoolBatchData([DataSourceRequest] DataSourceRequest request, string batchid, string type)
        {
            if (type == "WorkerPool")
            {
                var model = await _applicationBusiness.GetWorkerPoolBatchData(batchid);
                var data = model.ToDataSourceResult(request);
                return Json(data);
            }
            else
            {
                var model = await _applicationBusiness.GetBatchData(batchid);
                var data = model.ToDataSourceResult(request);
                return Json(data);
            }

        }
        public ActionResult ViewHmBatchCandidate(string batchid, string type)
        {
            ViewBag.BatchId = batchid;
            ViewBag.Type = type;
            return View("_ViewHmBatchCandidate");

        }
        public ActionResult ViewApplicationDetails(string jobId, string orgId)
        {
            var model = new ApplicationViewModel();
            model.OrganizationId = orgId;
            model.JobId = jobId;
            return View("_ViewApplicationDetails", model);

        }
        public async Task<JsonResult> ReadViewApplicationDetailsData([DataSourceRequest] DataSourceRequest request, string JobId, string OrgId)
        {
            var model = await _applicationBusiness.GetViewApplicationDetails(JobId, OrgId);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }
        public ActionResult ViewApplicationPendingTask()
        {
            //var model = new ApplicationViewModel();
            //model.OrganizationId = orgId;
            //model.JobId = jobId;
            return View("_ViewApplicationPendingTask");

        }
        public async Task<JsonResult> ReadViewApplicationPendingTaskData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _applicationBusiness.GetApplicationPendingTask(_userContext.UserId);
            if (_userContext.UserRoleCodes.Contains("HR"))
            {
                var model2 = await _applicationBusiness.GetApplicationWorkerPoolNotUnderApproval();
                if (model2 != null)
                {
                    model.AddRange(model2);
                }
            }
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }
        public async Task<JsonResult> ReadInterviewCandidatesData([DataSourceRequest] DataSourceRequest request, ApplicationSearchViewModel search)
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _applicationBusiness.GetCandiadteShortListDataByHR(search);
            //if (search.TotalExperience.IsNotNull())
            //{
            //    model.Where(x => x.TotalWorkExperience == search.TotalExperience.ToString());
            //}
            //if (search.JobTitle.IsNotNull())
            //{
            //    model.Where(x => x.JobTitle == search.JobTitle);
            //}
            //if (search.YearOfJobExperience.IsNotNull())
            //{
            //    model.Where(x => x.YearsOfJobExperience == search.YearOfJobExperience);
            //}
            //if (search.Industry.IsNotNull())
            //{
            //    model.Where(x => x.Industry == search.Industry);
            //}
            //if (search.ExpectedSalary.IsNotNull())
            //{
            //    model.Where(x => x.NetSalary == search.ExpectedSalary);
            //}
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }

        public async Task<JsonResult> ReadJobOfferCandidatesData([DataSourceRequest] DataSourceRequest request, ApplicationSearchViewModel search)
        {

            //var model = await _applicationBusiness.GetList(x => x.ApplicationState == "ShortlistedByHM");
            //search.ApplicationStateCode = "ShortListByHm";
            //search.BatchStatusCode = "PendingwithHM";
            //search.ApplicationStatusCode = "SHORTLISTED";
            search.ApplicationStateCode = "ShortListByHm";
            search.BatchStatusCode = "PendingwithHM";
            search.ApplicationStatusCode = "ShortlistedHM";
            search.TemplateCode = "SCHEDULE_INTERVIEW";
            if (search.Mode == "InterviewRequested")
            {
                search.ApplicationStatusCode = "InterviewRequested";
            }
            var model = await _applicationBusiness.GetCandiadteShortListApplicationData(search);
            if (search.Mode == "ShortlistedByHM")
            {
                model = model.Where(x => x.TaskId == null).ToList();
            }
            else if (search.Mode == "InterviewRequested")
            {
                model = model.Where(x => x.TaskId != null).ToList();
            }
            //search.ApplicationStateCode = "InterviewsCompleted";
            //search.BatchStatusCode = "PendingwithHM";
            //search.ApplicationStatusCode = "WAITLISTED";
            //search.TemplateCode = "SCHEDULE_INTERVIEW";
            //var model2 = await _applicationBusiness.GetCandiadteShortListApplicationData(search);
            //if (model2!=null)
            //{
            //    model.AddRange(model2);
            //}
            var data = model.ToDataSourceResult(request);
            return Json(data);
        }
        public async Task<JsonResult> ReadShortlistedHMInterviewData([DataSourceRequest] DataSourceRequest request, ApplicationSearchViewModel search)
        {
            search.ApplicationStateCode = "ShortListByHm";
            search.BatchStatusCode = "PendingwithHM";
            search.ApplicationStatusCode = "Interview";
            search.TemplateCode = "SCHEDULE_INTERVIEW";
            var model = await _applicationBusiness.GetCandiadteShortListApplicationData(search);
            var data = model.ToDataSourceResult(request);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationState(string applicants, string type)
        {
            var applicationstate = type;
            await _applicationBusiness.UpdateApplicationState(applicants, type);
            var str = applicants.Trim(',').Split(',');
            foreach (var item in str)
            {
                await _applicationBusiness.CreateApplicationStatusTrack(item, "SL_HM");
            }
            return Json(new { success = true });

        }
        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(string applicants, string type)
        {
            var applicationstate = type;
            await _applicationBusiness.UpdateApplicationtStatus(applicants, type);
            var str = applicants.Trim(',').Split(',');
            foreach (var item in str)
            {
                await _applicationBusiness.CreateApplicationStatusTrack(item, "SL_HM");
            }
            return Json(new { success = true });

        }

        public async Task<IActionResult> CreateApplicationTrackforHm(string applicantIds)
        {
            var applicantlist = applicantIds.Split(",");

            foreach (var appId in applicantlist)
            {
                if (appId.IsNotNullAndNotEmpty())
                {
                    var status = await _applicationBusiness.CreateApplicationStatusTrack(appId);
                }
            }
            return Json(new { success = true });
        }
        //[HttpPost]
        public async Task<ActionResult> UpdateApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state, string BatchId, string JobAddId, string JobId, string OrgId)
        {

            var result = await _applicationBusiness.UpdateApplicationStatus(type, status, applicationId, CandidateProfileId, state, BatchId, JobAddId, JobId, OrgId);
            if (result == "true")
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public async Task<ActionResult> UpdateApplicationBatch(string applicationId, string BatchId)
        {

            var result = await _applicationBusiness.UpdateApplicationBatch(applicationId, BatchId);
            if (result == "True")
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        //[HttpPost]
        public async Task<ActionResult> GetListOfValue(string type)
        {
            var result = await _applicationBusiness.GetListOfValueByType(type);
            return Json(result);
        }
        public async Task<ActionResult> GetJobAdvertisment(string OrganizationId)
        {
            //var result = await _jobAdvrtismentBusiness.GetJobIdNameByOrgIdList(OrganizationId);           
            var result = await _jobAdvrtismentBusiness.GetJobIdNameList();
            return Json(result);
        }
        public async Task<ActionResult> GetJobIdNameListForSelection(string OrganizationId)
        {
            //var result = await _jobAdvrtismentBusiness.GetJobIdNameByOrgIdList(OrganizationId);           
            var result = await _jobAdvrtismentBusiness.GetJobIdNameListForSelection();
            return Json(result);
        }

        public async Task<ActionResult> GetJobAdvertismentForWorker(string OrganizationId)
        {
            var result = await _jobAdvrtismentBusiness.GetJobIdNameList();
            if (result.IsNotNull())
            {
                result = result.Where(x => x.ManpowerTypeCode == "Worker" || x.ManpowerTypeCode == "UnskilledWorker" || x.ManpowerTypeCode == "Welder").ToList();
            }
            return Json(result);
        }
        public async Task<ActionResult> GetJobAdvertismentDashboard()
        {
            var result = await _jobAdvrtismentBusiness.GetJobIdNameDashboardList();
            result = result.OrderBy(x => x.JobName).ToList();
            return Json(result);
        }
        public async Task<ActionResult> GetJobAdvertismentList()
        {
            var result = await _jobAdvrtismentBusiness.GetJobIdNameDashboardList();
            result = result.OrderBy(x => x.JobName).ToList();
            var list = result.Select(x => new IdNameViewModel
            {
                Id = x.Id,
                Name = x.JobName
            }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetJobAdvertismentByOrg(string orgId)
        {
            var result = await _jobAdvrtismentBusiness.GetJobIdNameListByOrg(orgId);
            return Json(result);
        }
        public async Task<ActionResult> GetBatchList(string JobAddId)
        {
            var result = await _batchBusiness.GetList(x => x.JobId == JobAddId);
            return Json(result);
        }
        public async Task<ActionResult> GetBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId)
        {
            var listofval = await _listOfValueBusiness.GetList(x => x.Code == "Draft" && x.ListOfValueType == "BatchStatus");
            var result = await _batchBusiness.GetList(x => x.JobId == JobAddId && x.BatchStatus == listofval.FirstOrDefault().Id && x.BatchType == batchType/* && x.OrganizationId == orgId*/);
            return Json(result);
        }
        public async Task<ActionResult> GetWorkerBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId)
        {
            var listofval = await _listOfValueBusiness.GetList(x => x.Code == "Draft" && x.ListOfValueType == "BatchStatus");
            var result = await _batchBusiness.GetList(x => x.JobId == JobAddId && x.BatchStatus == listofval.FirstOrDefault().Id && x.BatchType == batchType && x.OrganizationId == orgId);
            // var result = await _batchBusiness.GetList(x => x.BatchStatus == listofval.FirstOrDefault().Id && x.BatchType == batchType);
            return Json(result);
        }
        public async Task<ActionResult> GetActiveBatchList(string JobAddId)
        {
            var result = await _batchBusiness.GetActiveBatchList(JobAddId);
            return Json(result);
        }
        public async Task<ActionResult> GetActiveBatchListByJobAdvOrg(string jobAdvId, string orgId)
        {
            var result = await _batchBusiness.GetActiveBatchListByJobAdvOrg(jobAdvId, orgId);
            return Json(result);
        }
        public async Task<ActionResult> GetActiveBatchHm(string jobAdvId, string orgId, string HmId)
        {
            var result = await _batchBusiness.GetActiveBatchHm(jobAdvId, orgId, HmId);
            return Json(result);
        }
        public IActionResult AddComment(string appId, string appStateId, string type)
        {
            ApplicationStateCommentViewModel model = new ApplicationStateCommentViewModel();
            ViewBag.type = type;
            model.ApplicationId = appId;
            model.ApplicationStateId = appStateId;
            return View(model);
        }
        public IActionResult ReturnCandidatePool(string appId, string appStateCode, string tempCode, string type)
        {
            ApplicationStateCommentViewModel model = new ApplicationStateCommentViewModel();
            ViewBag.type = type;
            model.ApplicationId = appId;
            //model.ApplicationStateId = appStateId;
            model.ApplicationStateCode = appStateCode;
            model.TemplateCode = tempCode;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageReturnCandidatePool(string Comment, string appId, string appStateId)
        {
            ApplicationStateCommentViewModel modelcomment = new ApplicationStateCommentViewModel();
            modelcomment.Comment = Comment;
            modelcomment.ApplicationId = appId;
            modelcomment.ApplicationStateId = appStateId;
            var result = await _applicationStateCommentBusiness.Create(modelcomment);
            if (result.IsSuccess)
            {
                //return Json(new { success = true, result = result.Item });
            }

            return Json(new { success = false });
        }
        public async Task<JsonResult> ReadApplicationStateComment([DataSourceRequest] DataSourceRequest request, string appId, string appStateId)
        {
            var model = await _applicationStateCommentBusiness.GetList(x => x.ApplicationId == appId && x.ApplicationStateId == appStateId);
            foreach (var data1 in model)
            {
                var user = await _userBusiness.GetSingleById(data1.CreatedBy);
                data1.CommentedBy = user.Name;
            }
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }
        [HttpPost]
        public async Task<ActionResult> ManageApplicationStateComment(string Comment, string appId, string appStateId)
        {
            ApplicationStateCommentViewModel model = new ApplicationStateCommentViewModel();
            model.Comment = Comment;
            model.ApplicationId = appId;
            model.ApplicationStateId = appStateId;
            var result = await _applicationStateCommentBusiness.Create(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true, result = result.Item });
            }
            return Json(new { success = false });
        }
        public async Task<ActionResult> GetStageIdNameList()
        {
            var listofval = await _listOfValueBusiness.GetList<IdNameViewModel, ApplicationState>(x => x.Code == "ShortListByHR" || x.Code == "UnReviewed" || x.Code == "Rereviewed");
            //var result = await _batchBusiness.GetList(x => x.JobId == JobAddId && x.BatchStatus == listofval.FirstOrDefault().Id && x.BatchType == batchType);
            return Json(listofval.OrderBy(x => x.SequenceOrder));
        }
        public async Task<ActionResult> GetApplicationStatusIdNameList(string state)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            if (state == "ShortListByHr" || state == ApplicationConstant.PlaceHolder_AllOption.ToString())
            {
                list = await _listOfValueBusiness.GetList<IdNameViewModel, ApplicationStatus>(x => x.Code == "WAITLISTED");
            }
            else if (state == "ShortListByHm")
            {
                list = await _listOfValueBusiness.GetList<IdNameViewModel, ApplicationStatus>(x => x.Code == "ShortlistedHM" || x.Code == "NotShortlisted" || x.Code == "RejectedHM" || x.Code == "Interview");
            }
            else if (state == "WorkerPool")
            {
                list = await _listOfValueBusiness.GetList<IdNameViewModel, ApplicationStatus>(x => x.Code == "NotAddedToBatch" || x.Code == "AddedToBatch" || x.Code == "UnderApproval");
            }
            //var result = await _batchBusiness.GetList(x => x.JobId == JobAddId && x.BatchStatus == listofval.FirstOrDefault().Id && x.BatchType == batchType);
            return Json(list.OrderBy(x => x.SequenceOrder));
        }


        public async Task<JsonResult> GetTreeViewForHM(string id, string parentId)
        {

            var result = await _applicationBusiness.GetHMTreeList(id);
            var model = result.ToList();
            return Json(model);
        }
        public async Task<IActionResult> InterviewSchedule(string BatchId)
        {
            var model = new ApplicationSearchViewModel();
            var Batch = await _batchBusiness.GetSingleById(BatchId);
            if (Batch != null)
            {
                model.OrganizationId = Batch.OrganizationId;
                model.JobTitleForHiring = Batch.JobId;
            }
            model.BatchId = BatchId;
            model.BatchHiringManagerId = _userContext.UserId;
            //return View("CandidateShortListByHM",model);
            return View(model);
        }
    }
}
