using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.REC.Controllers
{
    [Route("rec/ShortlistByHr")]
    [ApiController]
    public class ShortlistByHrController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly IJobAdvertisementBusiness _jobAdvrtismentBusiness;
        private readonly IRecruitmentTransactionBusiness _recruitmentTransactionBusiness;
        private readonly IApplicationStateCommentBusiness _applicationStateCommentBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ICandidateEvaluationBusiness _candidateEvaluationBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IRecruitmentElementBusiness _recruitmentElementBusiness;
        private readonly ICareerPortalBusiness _careerPortalBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public ShortlistByHrController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
            IApplicationBusiness applicationBusiness, IJobAdvertisementBusiness jobAdvrtismentBusiness, ICareerPortalBusiness careerPortalBusiness,IRecruitmentTransactionBusiness recruitmentTransactionBusiness,
           IApplicationStateCommentBusiness applicationStateCommentBusiness, IUserBusiness userBusiness,
           ICandidateEvaluationBusiness candidateEvaluationBusiness,
            ILOVBusiness lovBusiness
           , IRecruitmentElementBusiness recruitmentElementBusiness, INoteBusiness noteBusiness, IHRCoreBusiness hrCoreBusiness,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _applicationBusiness = applicationBusiness;
            _jobAdvrtismentBusiness = jobAdvrtismentBusiness;
            _recruitmentTransactionBusiness = recruitmentTransactionBusiness;
            _applicationStateCommentBusiness = applicationStateCommentBusiness;
            _userBusiness = userBusiness;
            _candidateEvaluationBusiness = candidateEvaluationBusiness;
            _lovBusiness = lovBusiness;
            _recruitmentElementBusiness = recruitmentElementBusiness;
            _noteBusiness = noteBusiness;
            _hrCoreBusiness = hrCoreBusiness;
            _careerPortalBusiness = careerPortalBusiness;   
        }

        #region ShortList by Hr


        [HttpGet]
        [Route("GetJobIdNameListForSelection")]
        public async Task<ActionResult> GetJobIdNameListForSelection(string OrganizationId)
        {
            //var result = await _jobAdvrtismentBusiness.GetJobIdNameByOrgIdList(OrganizationId);           
            var result = await _recruitmentTransactionBusiness.GetJobIdNameListForSelection();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetBatchIdNameList")]
        public async Task<ActionResult> GetBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId)
        {
            var listofval = await _lovBusiness.GetList(x => x.Code == "Draft" && x.LOVType == "BatchStatus");
            var result = await _recruitmentTransactionBusiness.GetBatchIdNameList(JobAddId, batchType, orgId);
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadBatchData")]
        public async Task<IActionResult> ReadBatchData(string jobIdAdvertismentId, BatchTypeEnum batchtype, string orgId, string batchId)
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

            return Ok(model);
        }

        [HttpPost]
        [Route("ReadShortlistCandidatesData")]
        public async Task<IActionResult> ReadShortlistCandidatesData(ApplicationSearchViewModel search)
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
            var data = model;
            return Ok(data);

        }


        [HttpGet]
        [Route("GetStageIdNameList")]
        public async Task<ActionResult> GetStageIdNameList()
        {
            var listofval = await _lovBusiness.GetList(x => x.Code == "ShortListByHR" || x.Code == "UnReviewed" || x.Code == "Rereviewed");

            return Ok(listofval.OrderBy(x => x.SequenceOrder));
        }

        [HttpGet]
        [Route("GetHMListByOrgId")]
        public async Task<ActionResult> GetHMListByOrgId(string orgId)
        {
            var data = await _recruitmentTransactionBusiness.GetHMListByOrgId(orgId);
            return Ok(data);
        }
        [HttpGet]
        [Route("GetHODListByOrgId")]
        public async Task<ActionResult> GetHODListByOrgId(string orgId)
        {
            var data = await _recruitmentTransactionBusiness.GetHODListByOrgId(orgId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GenerateBatchNo")]
        public async Task<IActionResult> GenerateBatchNo(string JobId, string OrgId)
        {
            var Name = "";
            if (JobId.IsNotNullAndNotEmpty() && OrgId.IsNotNullAndNotEmpty())
            {
                var jobName = await _hrCoreBusiness.GetJobNameById(JobId);
                var OrgName = await _hrCoreBusiness.GetOrgNameById(OrgId);
                Name = await _recruitmentTransactionBusiness.GenerateNextBatchNameUsingOrg(OrgName.Name + "_" + jobName.Name + "_");
            }
            return Ok(new { success = true, result = Name });
        }

        [HttpGet]
        [Route("Create")]
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
                var manpower = await _recruitmentTransactionBusiness.GetManpowerRecruitmentList(jobAdvertisementId, orgId);
                if (manpower.Count == 0)
                {
                    return Ok(new { success = false, message = "Please Create Manpower Summary" });
                }
                else
                {
                    return Ok(new { success = false, message = "" });
                   
                }
            }

            return Ok( model);
        }

        [HttpGet]
        [Route("Edit")]
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
                return Ok(Batch);
            }
            return Ok(new BatchViewModel());
        }

        [HttpPost]
        [Route("Manage")]
        public async Task<IActionResult> Manage(RecBatchViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
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
                        return Ok(new { success = true });
                    }
                    return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            

            return Ok( model);
        }



        [HttpGet]
        [Route("GetApplicationStatusIdNameList")]
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
            return Ok(list.OrderBy(x => x.SequenceOrder));
        }


        #endregion

    }
}
