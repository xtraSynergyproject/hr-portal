using AutoMapper;
//using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Recruitment.Areas.Rec.Controllers
{
    [Area("Rec")]
    public class ShortListByHrController : Controller
    {
        private readonly IApplicationBusiness _applicationBusiness;
       // private readonly IApplicationVersionBusiness _applicationVersionBusiness;
        private readonly IJobAdvertisementBusiness _jobAdvrtismentBusiness;
        private readonly IRecruitmentTransactionBusiness _recruitmentTransactionBusiness;
        private readonly IApplicationStateCommentBusiness _applicationStateCommentBusiness;
        private readonly ICareerPortalBusiness _careerPortalBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ICandidateEvaluationBusiness _candidateEvaluationBusiness;
        private readonly IPushNotificationBusiness _notificationBusiness;
        //private readonly ICandidateEvaluationVersionBusiness _candidateEvaluationVersionBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        private readonly IRecruitmentElementBusiness _recruitmentElementBusiness;
        public ShortListByHrController(IApplicationBusiness applicationBusiness, IJobAdvertisementBusiness jobAdvrtismentBusiness, IRecruitmentTransactionBusiness recruitmentTransactionBusiness,
           IApplicationStateCommentBusiness applicationStateCommentBusiness, IUserBusiness userBusiness,
           ICandidateEvaluationBusiness candidateEvaluationBusiness, ICareerPortalBusiness careerPortalBusiness,
           IPushNotificationBusiness notificationBusiness,
            IUserContext userContext, ILOVBusiness lovBusiness, IMapper mapper
          // , IApplicationVersionBusiness applicationVersionBusiness, 
           //ICandidateEvaluationVersionBusiness candidateEvaluationVersionBusiness
           , IRecruitmentElementBusiness recruitmentElementBusiness, INoteBusiness noteBusiness, IHRCoreBusiness hrCoreBusiness)
        {
            _applicationBusiness = applicationBusiness;
            _jobAdvrtismentBusiness = jobAdvrtismentBusiness;
            _recruitmentTransactionBusiness = recruitmentTransactionBusiness;
            _applicationStateCommentBusiness = applicationStateCommentBusiness;
            _userBusiness = userBusiness;
            _candidateEvaluationBusiness = candidateEvaluationBusiness;
            _userContext = userContext;
            _lovBusiness = lovBusiness;
            _mapper = mapper;
            _careerPortalBusiness = careerPortalBusiness;
            _notificationBusiness = notificationBusiness;
            // _applicationVersionBusiness = applicationVersionBusiness;
            //_candidateEvaluationVersionBusiness = candidateEvaluationVersionBusiness;
            _recruitmentElementBusiness = recruitmentElementBusiness;
            _noteBusiness = noteBusiness;
            _hrCoreBusiness = hrCoreBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CandidateShortlistByHR(string jobAdvId, string orgId, string batchId, LayoutModeEnum layoutMode = LayoutModeEnum.Popup, string status = null)
        {
            ApplicationSearchViewModel model = new ApplicationSearchViewModel();
            model.IssueDate = DateTime.Now;
            model.ExpiryDate = DateTime.Now;
            model.JobTitleForHiring = jobAdvId;
            ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
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
                var batch = await _recruitmentTransactionBusiness.GetBatchDataById(batchId);
                if (batch != null)
                {
                    model.JobTitleForHiring = batch.JobId;
                    model.OrganizationId = batch.OrganizationId;
                }
            }
            if (status.IsNotNullAndNotEmpty())
            {
                
                ViewBag.Pool = "RejectedPool";
            }
            return View(model);
        }
        public async Task<ActionResult> ReadBatchData(string jobIdAdvertismentId, BatchTypeEnum batchtype, string orgId, string batchId)
        {
           
            var model = await _recruitmentTransactionBusiness.GetBatchData(jobIdAdvertismentId, batchtype, orgId);
            if (batchId.IsNotNullAndNotEmpty() && batchId != "null")
            {
                model = model.Where(x => x.Id == batchId).ToList();
            }
            else
            {
                model = model.Where(x => x.BatchStatusCode == "Draft").ToList();
            }            
           
            return Json(model);
        }
        public async Task<JsonResult> ReadShortlistCandidatesData(string search1, ApplicationSearchViewModel search2)
        {
            ApplicationSearchViewModel search = new ApplicationSearchViewModel();
            
            if (search1 != "null")
            {
                search = JsonConvert.DeserializeObject<ApplicationSearchViewModel>(search1);
            }
            else
            {
                search = search2;
            }
            
            var model = await _careerPortalBusiness.GetCandiadteShortListDataByHR(search);
            if (search.Pool.IsNotNullAndNotEmpty())
            {
                model = model.Where(x => x.ApplicationStatusCode == "REJECTED" || x.ApplicationStatusCode == "RejectedHM").ToList();
            }
            else
            {
                model = model.Where(x => x.ApplicationStatusCode != "REJECTED" && x.ApplicationStatusCode != "RejectedHM").ToList();
            }
            var data = model;
            return Json(data);

        }

        public IActionResult AddComment(string appId, string appStateId, string type)
        {
            ApplicationStateCommentViewModel model = new ApplicationStateCommentViewModel();
            ViewBag.type = type;
            model.ApplicationId = appId;
            model.ApplicationStateId = appStateId;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageApplicationStateComment(string Comment, string appId, string appStateId)
        {
            ApplicationStateCommentViewModel model = new ApplicationStateCommentViewModel();
            model.Comment = Comment;
            model.ApplicationId = appId;
            model.ApplicationStateId = appStateId;

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "REC_APPLICATION_STATE_COMMENT";
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            notemodel.DataAction = DataActionEnum.Create;
            notemodel.CreatedBy = _userContext.UserId;
            
            var result = await _noteBusiness.ManageNote(notemodel);
            
            if (result.IsSuccess)
            {
                return Json(new { success = true, result = result.Item });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<ActionResult> ReadApplicationStateComment(string appId, string appStateId)
        {
            var result = await _recruitmentTransactionBusiness.GetApplicationStateCommentData(appId,appStateId);
            return Json(result);
        }
        public async Task<ActionResult> GetJobIdNameListForSelection(string OrganizationId)
        {
            //var result = await _jobAdvrtismentBusiness.GetJobIdNameByOrgIdList(OrganizationId);           
            var result = await _recruitmentTransactionBusiness.GetJobIdNameListForSelection();
            return Json(result);
        }
        public async Task<ActionResult> GetStageIdNameList()
        {
            var listofval = await _lovBusiness.GetList(x => x.Code == "ShortListByHR" || x.Code == "UnReviewed" || x.Code == "Rereviewed");
           
            return Json(listofval.OrderBy(x => x.SequenceOrder));
        }
        public async Task<ActionResult> GetBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId)
        {
            var listofval = await _lovBusiness.GetList(x => x.Code == "Draft" && x.LOVType == "BatchStatus");
            var result = await _recruitmentTransactionBusiness.GetBatchIdNameList(JobAddId, batchType, orgId);
            return Json(result);
        }
        public async Task<ActionResult> GetApplicationStatusIdNameList(string state)
        {
            List<LOVViewModel> list = new List<LOVViewModel>();
            List<IdNameViewModel> list1 = new List<IdNameViewModel>();
            if (state == "ShortListByHr" || state == ApplicationConstant.PlaceHolder_AllOption.ToString())
            {
                list = await _lovBusiness.GetList(x => x.Code == "WAITLISTED");
            }
            else if (state == "ShortListByHm")
            {
                list = await _lovBusiness.GetList(x => x.Code == "ShortlistedHM" || x.Code == "NotShortlisted" || x.Code == "RejectedHM" || x.Code == "Interview");
            }
            else if (state == "WorkerPool")
            {
                list = await _lovBusiness.GetList(x => x.Code == "NotAddedToBatch" || x.Code == "AddedToBatch" || x.Code == "UnderApproval");
            }
            list1 = list.Select(x => new IdNameViewModel() { Id = x.Id, Name = x.Name }).ToList();
            //var result = await _batchBusiness.GetList(x => x.JobId == JobAddId && x.BatchStatus == listofval.FirstOrDefault().Id && x.BatchType == batchType);
            return Json(list.OrderBy(x => x.SequenceOrder));
        }
        [HttpGet]
        public async Task<IActionResult> CheckManpowerRequirement(string jobId)
        {
            var rec = await _recruitmentTransactionBusiness.GetBatchDataByJobId(jobId);

            if (rec.Count > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> Create(string jobAdvertisementId, BatchTypeEnum batchType, string orgId, string taskId)
        {
            var model = new RecBatchViewModel();

            model.DataAction = DataActionEnum.Create;
            model.JobId = jobAdvertisementId.IsNotNullAndNotEmpty() ? jobAdvertisementId : null;
            model.BatchType = batchType;
            model.OrganizationId = orgId;
           
            var status = await _lovBusiness.GetSingle(x => x.LOVType == "BatchStatus" && x.Code == "Draft");
            model.BatchStatus = status.Id;
            if (jobAdvertisementId.IsNotNullAndNotEmpty() && orgId.IsNotNullAndNotEmpty())
            {
                var manpower = await _recruitmentTransactionBusiness.GetManpowerRecruitmentList(jobAdvertisementId , orgId);
                if (manpower.Count == 0)
                {
                    ViewBag.Message = "Please Create Manpower Summary";
                }
                else
                {
                    ViewBag.Message = "";
                }
            }
            //if (taskId.IsNotNullAndNotEmpty())
            //{
            //    var task = await _taskBusiness.GetSingleById(taskId);
            //    var status1 = await _lovBusiness.GetSingle(x => x.LOVType == "BatchStatus" && x.Code == "Batch_PendingwithHM");
            //    model.BatchStatus = status1.Id;
            //    if (task.TextValue5.IsNotNullAndNotEmpty())
            //    {
            //        var Batch = await _batchBusiness.GetSingleById(task.TextValue5);
            //        if (Batch != null)
            //        {

            //            Batch.DataAction = DataActionEnum.Edit;
            //            return View("Manage", Batch);
            //        }
            //    }
            //}


            return View("ManageBatch", model);
        }

        public async Task<IActionResult> Edit(string Id, string status = null)
        {
            var Batch = await _careerPortalBusiness.GetBatchDetailsById(Id);
            Batch.Name = Batch.BatchName;
            if (status == "PendingwithHM")
            {
                Batch.IsPending = true;
            }
            if (Batch != null)
            {

                Batch.DataAction = DataActionEnum.Edit;
                return View("ManageBatch", Batch);
            }
            return View("ManageBatch", new BatchViewModel());
        }

        public async Task<IActionResult> GenerateBatchNo(string JobId, string OrgId)
        {
            var Name = "";
            if (JobId.IsNotNullAndNotEmpty() && OrgId.IsNotNullAndNotEmpty())
            {
                var jobName = await _hrCoreBusiness.GetJobNameById(JobId);
                var OrgName = await _hrCoreBusiness.GetOrgNameById(OrgId);
                Name = await _recruitmentTransactionBusiness.GenerateNextBatchNameUsingOrg(OrgName.Name + "_" + jobName.Name + "_");
            }
            return Json(new { success = true, result = Name });
        }
        [HttpPost]
        public async Task<IActionResult> Manage(RecBatchViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    model.BatchName = model.Name;
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_BATCH";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);                   
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.BatchNoteId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }

            return View("ManageBatch", model);
        }

        [HttpGet]
        public async Task<ActionResult> GetHMListByOrgId(string orgId)
        {
            var data = await _recruitmentTransactionBusiness.GetHMListByOrgId(orgId);
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetHODListByOrgId(string orgId)
        {
            var data = await _recruitmentTransactionBusiness.GetHODListByOrgId(orgId);
            return Json(data);
        }

        public async Task<ActionResult> UpdateApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state, string BatchId, string JobAddId, string JobId, string OrgId)
        {

            var result = await _careerPortalBusiness.UpdateApplicationStatus(type, status, applicationId, CandidateProfileId, state, BatchId, JobAddId, JobId, OrgId);
            if (result == "true")
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(string applicants, string type)
        {
           // var applicationstate = type;
            await _recruitmentTransactionBusiness.UpdateApplicationtStatus(applicants, type);
            var str = applicants.Trim(',').Split(',');
            //foreach (var item in str)
           // {
               // await _applicationBusiness.CreateApplicationStatusTrack(item, "SL_HM");
           // }
            return Json(new { success = true });

        }
        public async Task<IActionResult> GetApplicantCount(string Id)
        {
            var Batch = await _recruitmentTransactionBusiness.GetBatchApplicantCount(Id);

            if (Batch != null)
            {

                return Json(new { success = true, count = Batch.NoOfApplication });
            }
            return Json(new { success = true, count = 0 });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBatchStatus(string batchId, string code)
        {
            await _recruitmentTransactionBusiness.UpdateStatus(batchId, code);
            // Send Notification to selected Hm
            var batch = await _careerPortalBusiness.GetBatchDetailsById(batchId);
            var user = await _userBusiness.GetSingleById(batch.HiringManager);
            var loggeduser = await _userBusiness.GetSingleById(_userContext.UserId);
            var notificationModel = new NotificationViewModel();
            notificationModel.Subject = batch.BatchName;
            notificationModel.Body = string.Concat("<div><h4>Hello ", user.Name, "</h4>You have received batch " + batch.Name + " to shortlist for interview</div> ");
            notificationModel.From = loggeduser.Email;
            notificationModel.ToUserId = user.Id;
            notificationModel.To = user.Email;
            var result = await _notificationBusiness.Create(notificationModel);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBatchClose(string batchId, string code)
        {
            await _recruitmentTransactionBusiness.UpdateStatus(batchId, code);
            var batchmodel = await _recruitmentTransactionBusiness.GetBatchDataById(batchId);
            if (code == "Close")
            {
               
            }
            return Json(new { success = true });
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
        public ActionResult ViewBatchCandidate(string batchid, string type)
        {
            ViewBag.BatchId = batchid;
            ViewBag.Type = type;
            return View("_ViewBatchCandidate");
        }
    }
}
