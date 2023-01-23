using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using Kendo.Mvc.Extensions;

namespace Synergy.App.Api.Areas.REC.Controllers
{
    [Route("rec/RecruitmentTransaction")]
    [ApiController]
    public class RecruitmentTransactionController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRecruitmentTransactionBusiness _recruitmentBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ICareerPortalBusiness _careerPortalBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IRecQueryBusiness _recQueryBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public RecruitmentTransactionController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
            IRecruitmentTransactionBusiness recruitmentBusiness,
         ICareerPortalBusiness careerPortalBusiness,IApplicationBusiness applicationBusiness,
        IUserRoleBusiness userRoleBusiness, ICmsBusiness cmsBusiness, ILOVBusiness lOVBusiness, INoteBusiness noteBusiness, IRecQueryBusiness recQueryBusiness,
            IServiceBusiness serviceBusiness, ITableMetadataBusiness tableMetadataBusiness, ITemplateBusiness templateBusiness, ITaskBusiness taskBusiness
          ,IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _recruitmentBusiness = recruitmentBusiness;
            _userRoleBusiness = userRoleBusiness;
            _cmsBusiness = cmsBusiness;
            _lOVBusiness = lOVBusiness;
            _noteBusiness = noteBusiness;
            _serviceBusiness = serviceBusiness;
            _careerPortalBusiness = careerPortalBusiness;
            _applicationBusiness = applicationBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _recQueryBusiness = recQueryBusiness; 
            _templateBusiness = templateBusiness;
            _taskBusiness = taskBusiness;
        }

        #region DASHBOARD

        [HttpGet]
        [Route("GetIdNameList")]
        public async Task<IActionResult> GetIdNameList(string type)
        {
            var data = await _recruitmentBusiness.GetIdNameList(type);

            return Ok(data);
        }

        [HttpGet]
        [Route("GetTaskByOrgUnit")]
        public async Task<IActionResult> GetTaskByOrgUnit(string userRoleId, string userId)
        {
            await Authenticate(userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var Data = await _recruitmentBusiness.GetTaskByOrgUnit(_userContext.UserId, userRoleId);
            return Ok(Data);
        }

        [HttpGet]
        [Route("GetJobAdvertismentDashboard")]
        public async Task<IActionResult> GetJobAdvertismentDashboard()
        {
            var result = await _recruitmentBusiness.GetJobIdNameDashboardList();
            result = result.OrderBy(x => x.JobName).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetOrganizationIdNameByRecruitmentList")]
        public async Task<IActionResult> GetOrganizationIdNameByRecruitmentList(string jobAddId, string userId)
        {
            await Authenticate(userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            IList<IdNameViewModel> list = new List<IdNameViewModel>();
            var data = await _recruitmentBusiness.GetOrgByJobAddId(jobAddId);
            var Role = _userContext.UserRoleCodes;
            if (Role.Contains("HM"))
            {
                var orglist = await _recruitmentBusiness.GetHmOrg(_userContext.UserId);
                foreach (var obj in data)
                {
                    //if (orglist.Any(x => x.Id == obj.Id))
                    //{
                    //    list.Add(obj);
                    //}
                    if (orglist.Id.Contains(obj.Id))
                    {
                        list.Add(obj);
                    }
                }
                return Ok(list);
            }
            else if (Role.Contains("ORG_UNIT"))
            {
                var orglist = await _recruitmentBusiness.GetHODOrg(_userContext.UserId);
                foreach (var obj in data)
                {
                    //if (orglist.Any(x => x.Id == obj.Id))
                    //{
                    //    list.Add(obj);
                    //}
                    if (orglist.Id.Contains(obj.Id))
                    {
                        list.Add(obj);
                    }
                }
                return Ok(list);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetUserPendingTaskByRole")]
        public async Task<IActionResult> GetUserPendingTaskByRole(string orgId, string userId)
        {
            await Authenticate(userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var list = await _recruitmentBusiness.GetPendingTaskDetailsForUser(_userContext.UserId, orgId, _userContext.UserRoleCodes);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetRecruitmentDashboardCount")]
        public async Task<IActionResult> GetRecruitmentDashboardCount(string orgId)
        {
            var list = await _recruitmentBusiness.GetRecruitmentDashobardCount(orgId);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetRecruitmentDashboardByOrgJob")]
        public async Task<IActionResult> GetRecruitmentDashboardByOrgJob(string orgId, string jobAdvId, string permission = "")
        {
            var data = await _recruitmentBusiness.GetManpowerRecruitmentSummaryByOrgJob(orgId, jobAdvId, permission);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadViewApplicationPendingTaskData")]
        public async Task<ActionResult> ReadViewApplicationPendingTaskData(string userId,string portalName)
        {
            await Authenticate(userId,portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var model = await _recruitmentBusiness.GetApplicationPendingTask(_userContext.UserId);
            if (_userContext.UserRoleCodes.Contains("HR"))
            {
                var model2 = await _recruitmentBusiness.GetApplicationWorkerPoolNotUnderApproval();
                if (model2 != null)
                {
                    model.ToList().AddRange(model2);
                }
            }
            return Ok(model);
        }

        [HttpGet]
        [Route("JobAdvertisementStatistic")]
        public async Task<IActionResult> JobAdvertisementStatistic(string jobAdvId, string state, string orgId, long count, string status, string permissions = "")
        {
            var model = new RecApplicationViewModel();
            model.JobId = jobAdvId;
            model.ApplicationState = state;
            model.ApplicationStatus = status;
            if (state == null)
            {
                model.ApplicationStateName = "Number Of Applications";
            }
            else if (state == "InterviewsCompleted" && status == "WAITLISTED")
            {
                model.ApplicationStateName = "Waitlist By HM";
            }
            else
            {
                //var state1 = await _recruitmentBusiness.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == state);
                //var state1 = await _recruitmentBusiness.GetApplicationStateByCode(state);
                var state1 = await _lOVBusiness.GetSingle(x => x.Code == state && x.LOVType == "APPLICATION_STATE");
                model.ApplicationStateName = state1.Name;
            }
            model.OrganizationId = orgId;
            model.StateCount = count;
            if (jobAdvId.IsNotNullAndNotEmpty())
            {
                var jobname = await _recruitmentBusiness.GetJobNameById(jobAdvId);
                model.JobTitle = jobname.Name;
            }
            else
            {
                model.JobTitle = "All";
            }
            return Ok(model);
        }


        [HttpGet]
        [Route("ReadTotalJobAdvertisementData")]
        public async Task<IActionResult> ReadTotalJobAdvertisementData(string jobAdvId, string orgId, string state, string tempcode, string nexttempcode, string visatempcode)
        {
            var model = await _recruitmentBusiness.GetTotalApplication(jobAdvId, orgId, state, tempcode, nexttempcode, visatempcode);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetManpowerRequirement")]
        public async Task<IActionResult> GetManpowerRequirement(string jobId, string orgId)
        {
            var data = await _recruitmentBusiness.GetManpowerRequirement(jobId, orgId);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadJobAdvertisementData")]
        public async Task<IActionResult> ReadJobAdvertisementData(string jobAdvId, string orgId, string state, string tempcode, string nexttempcode, string visatempcode, string status)
        {
            var model = await _recruitmentBusiness.GetJobAdvertismentState(jobAdvId, orgId, state, tempcode, nexttempcode, visatempcode, status);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetDirectHiringData")]
        public async Task<IActionResult> GetDirectHiringData(string jobAdvId, string orgId)
        {
            var model = await _recruitmentBusiness.GetDirictHiringData(jobAdvId, orgId);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetJobAdvertismentApplication")]
        public async Task<IActionResult> GetJobAdvertismentApplication(string jobAdvId, string orgId, string state, string tempcode, string tempCodeOther)
        {
            var model = await _recruitmentBusiness.GetJobAdvertismentApplication(jobAdvId, orgId, state, tempcode, tempCodeOther);

            return Ok(model);

        }

        [HttpGet]
        [Route("ReadViewApplicationDetailsData")]
        public async Task<IActionResult> ReadViewApplicationDetailsData(string JobId, string OrgId)
        {
            var model = await _recruitmentBusiness.GetViewApplicationDetails(JobId, OrgId);
            return Ok(model);
        }
        #endregion

        #region Job Advertisement
        [HttpGet]
        [Route("ReadManpowerUniqueJobData")]
        public async Task<IActionResult> ReadManpowerUniqueJobData()
        {
            var list = await _recruitmentBusiness.GetManpowerUniqueJobData();
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadJobAdvCriteriaData")]
        public async Task<IActionResult> ReadJobAdvCriteriaData(string jobadvtid, string jobdescid)
        {
            var data = new List<JobCriteriaViewModel>();
            var JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobadvtid.IsNotNullAndNotEmpty() && jobdescid.IsNullOrEmpty())
            {
                var model = await _recruitmentBusiness.GetJobCriteriaList("Criteria", jobadvtid);
                data = model.ToList();
            }
            else
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("Criteria", jobdescid);
                return Ok(model);
            }

            return Ok(data);
        }

        [HttpGet]
        [Route("ReadJobAdvSkillsData")]
        public async Task<IActionResult> ReadJobAdvSkillsData(string jobadvtid, string jobdescid)
        {
            var data = new List<JobCriteriaViewModel>();
            var JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobadvtid.IsNotNullAndNotEmpty() && jobdescid.IsNullOrEmpty())
            {
                var model = await _recruitmentBusiness.GetJobCriteriaList("Skills", jobadvtid);
                data = model.ToList();
            }
            else
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("Skills", jobdescid);
                return Ok(model);
            }

            return Ok(data);
        }

        [HttpGet]
        [Route("ReadJobAdvOtherInfoData")]
        public async Task<IActionResult> ReadJobAdvOtherInfoData(string jobadvtid, string jobdescid)
        {
            var data = new List<JobCriteriaViewModel>();
            var JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobadvtid.IsNotNullAndNotEmpty() && jobdescid.IsNullOrEmpty())
            {
                var model = await _recruitmentBusiness.GetJobCriteriaList("OtherInformation", jobadvtid);
                data = model.ToList();
            }
            else
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("OtherInformation", jobdescid);
                return Ok(model);
            }

            return Ok(data);
        }

        [HttpPost]
        [Route("ManageJobAdvertisement")]
        public async Task<IActionResult> ManageJobAdvertisement(JobAdvertisementViewModel model)
        {
            await Authenticate(model.userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var actionresult = await _lOVBusiness.GetList(x => x.LOVType == "LOV_JOBADVTACTION");
            var draftactionid = "";
            var submitactionid = "";
            var approveactionid = "";
            foreach (var a in actionresult)
            {
                if (a.Name == "Approve")
                {
                    approveactionid = a.Id;
                }
                if (a.Name == "Submit")
                {
                    submitactionid = a.Id;
                }
                if (a.Name == "Draft")
                {
                    draftactionid = a.Id;
                }
            }
            var temp = _userContext.UserRoleCodes.Split(",");
            var hrroliid = "";
            var corproliid = "";
            if (temp.Contains("HR"))
            {
                var hrrole = await _userRoleBusiness.GetSingle(x => x.Code == "HR");
                hrroliid = hrrole.Id;
            }
            model.RoleId = hrroliid;

            if (model.SaveType == "JOBADVTACTION_APPROVE")
            {
                if (temp.Contains("CORPCOMP"))
                {
                    var corprole = await _userRoleBusiness.GetSingle(x => x.Code == "CORPCOMP");
                    corproliid = corprole.Id;
                    model.ApprovedDate = DateTime.Now;
                }
                model.RoleId = corproliid;
            }

            if (model.Status == 0)
            {
                model.Status = StatusEnum.Active;
            }

            if (ModelState.IsValid)
            {
                if (model.AgencyIds.IsNotNull())
                {
                    //model.AgencyId = model.AgencyIds.ToArray();
                    model.AgencyId = string.Concat("{", string.Join(",", model.AgencyIds), "}");
                }
                if (model.DataAction == DataActionEnum.Create)
                {
                    if (model.SaveType == "JOBADVTACTION_DRAFT")
                    {
                        model.RoleId = hrroliid;
                        if (model.SaveType != "")
                        {
                            var m = await _lOVBusiness.GetSingle(x => x.LOVType == "LOV_JOBADVTACTION" && x.Code == model.SaveType);
                            model.ActionId = m.Id;
                            model.ActionName = m.Name;
                        }

                        var serTempModel = new ServiceTemplateViewModel();
                        serTempModel.DataAction = DataActionEnum.Create;
                        serTempModel.ActiveUserId = _userContext.UserId;
                        serTempModel.TemplateCode = "REC_JOB_ADVERTISEMENT";
                        var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);

                        sermodel.Json = JsonConvert.SerializeObject(model);
                        sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                        sermodel.DataAction = DataActionEnum.Create;
                        var result = await _serviceBusiness.ManageService(sermodel);

                        if (result.IsSuccess)
                        {
                            if (model.JobCriteria.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                                await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Criteria", _userContext);
                            }

                            if (model.Skills.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Skills", _userContext);
                            }
                            if (model.OtherInformation.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "OtherInformation", _userContext);
                            }

                            return Ok(new { success = true });
                        }
                        else
                        {
                            return Ok(new { success = false, error = result.Messages });
                        }
                    }

                    else if (model.SaveType == "JOBADVTACTION_APPROVE")
                    {
                        if (model.Id.IsNullOrEmpty())
                        {
                            model.RoleId = hrroliid;
                            if (model.SaveType != "")
                            {
                                var m = await _lOVBusiness.GetSingle(x => x.LOVType == "LOV_JOBADVTACTION" && x.Code == model.SaveType);
                                model.ActionId = m.Id;
                                model.ActionName = m.Name;
                            }

                            var serTempModel = new ServiceTemplateViewModel();
                            serTempModel.DataAction = DataActionEnum.Create;
                            serTempModel.ActiveUserId = _userContext.UserId;
                            serTempModel.TemplateCode = "REC_JOB_ADVERTISEMENT";

                            var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);

                            sermodel.Json = JsonConvert.SerializeObject(model);
                            sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                            sermodel.DataAction = DataActionEnum.Create;
                            var result = await _serviceBusiness.ManageService(sermodel);

                            if (result.IsSuccess)
                            {
                                if (model.JobCriteria.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                                    await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Criteria", _userContext);
                                }

                                if (model.Skills.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                    await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Skills", _userContext);
                                }
                                if (model.OtherInformation.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                    await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "OtherInformation", _userContext);
                                }

                                //await _taskBusiness.AssignTaskForJobAdvertisement(model.Id, model.JobName, model.CreatedDate, null);
                                return Ok(new { success = true });
                            }
                            else
                            {
                                return Ok(new { success = false, error = result.Messages });
                            }
                        }
                    }

                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    if (model.SaveType == "JOBADVTACTION_DRAFT" && model.ActionId != submitactionid)
                    {
                        if (model.SaveType != "")
                        {
                            var m = await _lOVBusiness.GetSingle(x => x.LOVType == "LOV_JOBADVTACTION" && x.Code == model.SaveType);
                            model.ActionId = m.Id;
                            model.ActionName = m.Name;
                        }

                        var noteTempModel1 = new NoteTemplateViewModel();
                        noteTempModel1.SetUdfValue = true;
                        noteTempModel1.NoteId = model.JobAdvNoteId;
                        var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                        var data1 = JsonConvert.SerializeObject(model);
                        var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, model.Id);

                        if (update.IsSuccess)
                        {
                            if (model.JobCriteria.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                                var existingjobcriteria = await _recruitmentBusiness.GetJobCriteriaList("Criteria", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {
                                        var noteTempModel = new NoteTemplateViewModel();
                                        noteTempModel.SetUdfValue = true;
                                        noteTempModel.NoteId = p.NtsNoteId;
                                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                        var res = await _noteBusiness.DeleteNote(notemodel);
                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Criteria", _userContext);
                                }

                            }

                            if (model.Skills.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                var existingjobcriteria = await _recruitmentBusiness.GetJobCriteriaList("Skills", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {
                                        var noteTempModel = new NoteTemplateViewModel();
                                        noteTempModel.SetUdfValue = true;
                                        noteTempModel.NoteId = p.NtsNoteId;
                                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                        var res = await _noteBusiness.DeleteNote(notemodel);
                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Skills", _userContext);
                                }

                            }
                            if (model.OtherInformation.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                var existingjobcriteria = await _recruitmentBusiness.GetJobCriteriaList("OtherInformation", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {
                                        var noteTempModel = new NoteTemplateViewModel();
                                        noteTempModel.SetUdfValue = true;
                                        noteTempModel.NoteId = p.NtsNoteId;
                                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                        var res = await _noteBusiness.DeleteNote(notemodel);
                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "OtherInformation", _userContext);
                                }
                            }
                            return Ok(new { success = true });
                        }
                        else
                        {
                            return Ok(new { success = false, error = update.Messages });
                        }
                    }

                    else if (model.SaveType == "JOBADVTACTION_APPROVE")
                    {
                        if (model.SaveType != "")
                        {
                            var m = await _lOVBusiness.GetSingle(x => x.LOVType == "LOV_JOBADVTACTION" && x.Code == model.SaveType);
                            model.ActionId = m.Id;
                            model.ActionName = m.Name;
                        }
                        var serTempModel = new ServiceTemplateViewModel();
                        serTempModel.DataAction = DataActionEnum.Edit;
                        serTempModel.ActiveUserId = _userContext.UserId;
                        serTempModel.ServiceId = model.ServiceId;
                        var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);

                        sermodel.Json = JsonConvert.SerializeObject(model);
                        sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                        var result = await _serviceBusiness.ManageService(sermodel);

                        if (result.IsSuccess)
                        {
                            if (model.JobCriteria.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                                var existingjobcriteria = await _recruitmentBusiness.GetJobCriteriaList("Criteria", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {
                                        var noteTempModel = new NoteTemplateViewModel();
                                        noteTempModel.SetUdfValue = true;
                                        noteTempModel.NoteId = p.NtsNoteId;
                                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                        var res = await _noteBusiness.DeleteNote(notemodel);
                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Criteria", _userContext);
                                }
                            }

                            if (model.Skills.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                var existingjobcriteria = await _recruitmentBusiness.GetJobCriteriaList("Skills", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {
                                        var noteTempModel = new NoteTemplateViewModel();
                                        noteTempModel.SetUdfValue = true;
                                        noteTempModel.NoteId = p.NtsNoteId;
                                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                        var res = await _noteBusiness.DeleteNote(notemodel);
                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Skills", _userContext);
                                }
                            }
                            if (model.OtherInformation.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                var existingjobcriteria = await _recruitmentBusiness.GetJobCriteriaList("OtherInformation", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {
                                        var noteTempModel = new NoteTemplateViewModel();
                                        noteTempModel.SetUdfValue = true;
                                        noteTempModel.NoteId = p.NtsNoteId;
                                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                        var res = await _noteBusiness.DeleteNote(notemodel);
                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "OtherInformation", _userContext);
                                }
                            }

                            return Ok(new { success = true });
                        }
                        else
                        {
                            return Ok(new { success = false, error = result.Messages });
                        }
                    }
                }
            }

            return Ok(new { success = false });

        }

        protected async Task CreateJobAdvertisementCriteria(List<JobCriteriaViewModel> list, string jobAdvId, string type,IUserContext userContext)
        {

            foreach (var item in list)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = userContext.UserId;
                noteTempModel.TemplateCode = "REC_JOB_ADV_CRITERIA";
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("JobAdvertisementId", jobAdvId);
                ((IDictionary<String, Object>)exo).Add("Criteria", item.Criteria);
                ((IDictionary<String, Object>)exo).Add("CriteriaTypeId", item.CriteriaTypeId);
                ((IDictionary<String, Object>)exo).Add("Weightage", item.Weightage);
                ((IDictionary<String, Object>)exo).Add("ListOfValueTypeId", item.ListOfValueTypeId);
                ((IDictionary<String, Object>)exo).Add("Type", type);

                notemodel.Json = JsonConvert.SerializeObject(exo);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                await _noteBusiness.ManageNote(notemodel);
            }

        }

        [HttpGet]
        [Route("ViewListOfValue")]
        public IActionResult ViewListOfValue(string jobid)
        {
            return Ok(new LOVViewModel
            {
                ReferenceType = ReferenceTypeEnum.REC_Job,
                ReferenceId = jobid,
            });
        }

        [HttpGet]
        [Route("GetLOVList")]
        public async Task<ActionResult> GetLOVList(string jobid)
        {
            var data = await _lOVBusiness.GetList(x => x.LOVType == "LIST_OF_VALUE_TYPE" && x.ReferenceId == jobid && x.Status != StatusEnum.Inactive);
            return Ok(data);
        }

        [HttpGet]
        [Route("CreateListOfValue")]
        public async Task<IActionResult> CreateListOfValue(string Id, string refTypeId)
        {
            var ListOfValue = await _lOVBusiness.GetSingleById(Id);

            if (ListOfValue != null)
            {
                ListOfValue.DataAction = DataActionEnum.Edit;
                return Ok(ListOfValue);
            }
            return Ok(new LOVViewModel() { ReferenceId = refTypeId, ReferenceType = ReferenceTypeEnum.REC_Job, DataAction = DataActionEnum.Create });
        }

        [HttpPost]
        [Route("ManageListOfValue")]
        public async Task<IActionResult> ManageListOfValue(LOVViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lovcode = String.Concat(model.Name.Where(c => !Char.IsWhiteSpace(c)));

                var exist = await _lOVBusiness.GetSingle(x => x.Name == model.Name && x.Id != model.Id);
                if (exist != null)
                {
                    return Ok(new { success = false, error = "LOV Type Name already exist" });
                }
                else
                {
                    if (model.Json.IsNullOrEmpty())
                    {
                        return Ok(new { success = false, error = "Enter LOV values" });
                    }
                    else
                    {
                        var lovs = JsonConvert.DeserializeObject<List<LOVViewModel>>(model.Json);
                        foreach (var item in lovs)
                        {
                            if (item.Name.IsNullOrEmptyOrWhiteSpace())
                            {
                                return Ok(new { success = false, error = "Enter LOV name" });
                            }
                        }
                    }

                    model.Code = String.Concat("JOB_" + lovcode);
                    model.LOVType = "LIST_OF_VALUE_TYPE";
                    model.Status = StatusEnum.Active;
                    if (model.DataAction == DataActionEnum.Create)
                    {
                        var result = await _lOVBusiness.Create(model);

                        if (result.IsSuccess)
                        {
                            if (model.Json.IsNotNullAndNotEmpty())
                            {
                                try
                                {
                                    var json = JsonConvert.DeserializeObject<List<LOVViewModel>>(model.Json);
                                    foreach (var item in json)
                                    {
                                        var childlovcode = String.Concat(item.Name.Where(c => !Char.IsWhiteSpace(c)));
                                        item.Code = String.Concat("JOB_" + childlovcode);
                                        if (item.ParentId == null)
                                        {
                                            item.LOVType = result.Item.Code;
                                            item.ParentId = result.Item.Id;
                                        }
                                        item.ReferenceId = result.Item.ReferenceId;
                                        item.ReferenceType = result.Item.ReferenceType;

                                        item.Status = StatusEnum.Active;
                                        var r = await _lOVBusiness.Create(item);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            return Ok(new { success = true, lovtype = result.Item.Code, id = result.Item.Id });
                        }
                        else
                        {
                            return Ok(new { success = false, result.Messages });
                        }
                    }
                    else if (model.DataAction == DataActionEnum.Edit)
                    {
                        var result = await _lOVBusiness.Edit(model);

                        if (result.IsSuccess)
                        {
                            if (model.Json.IsNotNullAndNotEmpty())
                            {
                                var json = JsonConvert.DeserializeObject<List<LOVViewModel>>(model.Json);
                                var existing = await _lOVBusiness.GetList(x => x.ParentId == result.Item.Id);
                                foreach (var item1 in existing)
                                {
                                    var exitem = json.Where(x => x.Id == item1.Id).ToList();
                                    if (exitem.Count == 0)
                                    {
                                        await _lOVBusiness.Delete(item1.Id);
                                    }
                                }
                                var newexisting = await _lOVBusiness.GetList(x => x.ParentId == result.Item.Id);
                                foreach (var item in json)
                                {
                                    var exitem = newexisting.Where(x => x.Id == item.Id).ToList();
                                    if (item.Id.IsNotNullAndNotEmpty() && exitem.Count != 0)
                                    {
                                        var childlovcode = String.Concat(item.Name.Where(c => !Char.IsWhiteSpace(c)));
                                        item.Code = String.Concat("JOB_" + childlovcode);
                                        if (item.ParentId == null)
                                        {
                                            item.LOVType = result.Item.Code;
                                            item.ParentId = result.Item.Id;
                                        }
                                        item.ReferenceId = result.Item.ReferenceId;
                                        item.ReferenceType = result.Item.ReferenceType;

                                        item.Status = StatusEnum.Active;
                                        var r = await _lOVBusiness.Edit(item);
                                    }
                                    else
                                    {
                                        var childlovcode = String.Concat(item.Name.Where(c => !Char.IsWhiteSpace(c)));
                                        item.Code = String.Concat("JOB_" + childlovcode);
                                        if (item.ParentId == null)
                                        {
                                            item.LOVType = result.Item.Code;
                                            item.ParentId = result.Item.Id;
                                        }
                                        item.ReferenceId = result.Item.ReferenceId;
                                        item.ReferenceType = result.Item.ReferenceType;

                                        item.Status = StatusEnum.Active;
                                        var r = await _lOVBusiness.Create(item);
                                    }
                                }
                            }

                            return Ok(new { success = true, lovtype = result.Item.Code, id = result.Item.Id, action = "Edit" });

                        }
                        else
                        {
                            return Ok(new { success = true, result.Messages });
                        }
                    }
                }
            }

            return Ok( model);
        }


        #endregion


        #region Manpower summery
        [HttpGet]
        [Route("ReadManpowerRequirementSummaryData")]
        public async Task<IActionResult> ReadManpowerRequirementSummaryData(string userId   )
        {
            await Authenticate(userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var res = await _recruitmentBusiness.GetManpowerRecruitmentSummaryData();
            return Ok(res);
        }

        #endregion

        #region RECRUITMENT IN PROGRESS
        [HttpGet]
        [Route("ReadActiveManpowerRequirementSummaryData")]
        public async Task<IActionResult> ReadActiveManpowerRequirementSummaryData(string userId)
        {
            await Authenticate(userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var res = await _careerPortalBusiness.GetActiveManpowerRecruitmentSummaryData();
            return Ok(res);
        }


        #endregion

        #region  Report 

        [HttpGet]
        [Route("GetCandidateReportData")]
        public async Task<IActionResult> GetCandidateReportData()
        {
            var res = await _applicationBusiness.GetCandidateReportData();
            return Ok(res);
        }

       

        [HttpGet]
        [Route("GetPendingforHM")]
        public async Task<IActionResult> GetPendingforHM()
        {
            var res = await _recQueryBusiness.PendingforHM();
            return Ok(res);
        }


        [HttpGet]
        [Route("GetRejectedforHM")]
        public async Task<IActionResult> GetRejectedforHM()
        {
            var res = await _recQueryBusiness.RejectedforHM();
            return Ok(res);
        }

        [HttpGet]
        [Route("GetShortlistforFuture")]
        public async Task<IActionResult> ShortlistforFuture()
        {
            var res = await _recQueryBusiness.ShortlistforFuture();
            return Ok(res);
        }

        [HttpGet]
        [Route("PendingForUser")]
        public async Task<IActionResult> PendingForUser()
        {
            var res = await _recQueryBusiness.ShortlistforFuture();
            return Ok(res);
        }

        [HttpGet]
        [Route("PendingForHR")]
        public async Task<IActionResult> PendingForHR()
        {
            var res = await _recQueryBusiness.PendingforHR();
            return Ok(res);
        }

        [HttpGet]
        [Route("PendingForED")]
        public async Task<IActionResult> PendingForED()
        {
            var res = await _recQueryBusiness.PendingforED();
            return Ok(res);
        }
        #endregion
        
        #region Master list

        [HttpGet]
        [Route("GetJobIdNameList")]
        public async Task<ActionResult> GetJobIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRJob", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetOrgIdNameList")]
        public async Task<ActionResult> GetOrgIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRDepartment", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadAgencyList")]
        public async Task<IActionResult> ReadAgencyList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("REC_AGENCY", "");
            return Ok(data);
        }

        [HttpGet]
        [Route("GetLocationIdNameList")]
        public async Task<ActionResult> GetLocationIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRLocation", "");
            return Ok(data);
        }
        [HttpGet]
        [Route("GetNationallityIdNameList")]
        public async Task<ActionResult> GetNationallityIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRNationality", "");
            return Ok(data);
        }

        #endregion

        #region Inbox

        [HttpGet]
        [Route("GetInboxTreeviewList")]
        public async Task<IActionResult> GetInboxTreeviewList(string id,string portalName, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var result = await _recruitmentBusiness.GetInboxTreeviewList(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
            var model = result.ToList();
            return Ok(model);
        }

        #endregion


        #region Task List

        [HttpGet]
        [Route("ReadTaskDataInProgress")]
        public async Task<IActionResult> ReadTaskDataInProgress()
        {
            var result = await _recruitmentBusiness.GetRecTaskList("TASK_STATUS_INPROGRESS");
            var j = Ok(result.OrderByDescending(x => x.StartDate));
            return j;
        }
        [HttpGet]
        [Route("ReadTaskDataOverdue")]
        public async Task<IActionResult> ReadTaskDataOverdue()
        {
            var result = await _recruitmentBusiness.GetRecTaskList("TASK_STATUS_OVERDUE");
            var j = Ok(result.OrderByDescending(x => x.StartDate));
            return j;
        }
        [HttpGet]
        [Route("ReadTaskDataCompleted")]
        public async Task<IActionResult> ReadTaskDataCompleted()
        {
            var result = await _recruitmentBusiness.GetRecTaskList("TASK_STATUS_COMPLETE");
            var j = Ok(result.OrderByDescending(x => x.StartDate));
            return j;
        }
        #endregion


        #region Prepare Vacancy

        [HttpGet]
        [Route("ReadSelectedJobadvtData")]
        public async Task<ActionResult> ReadSelectedJobadvtData(string vacancyId)
        {
            var model = await _recruitmentBusiness.GetSelectedJobAdvertisement(vacancyId);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetJobAdvList")]
        public async Task<ActionResult> GetJobAdvList()
        {
            var model = await _recruitmentBusiness.GetJobAdvList();

            return Ok(model);
        }

        [HttpGet]
        [Route("AddJobAdvertisements")]
        public async Task<ActionResult> AddJobAdvertisements(string vacancyId, string jobAdvIds)
        {
            string[] jaId = jobAdvIds.Trim(',').Split(",").ToArray();

            foreach (var id in jaId)
            {
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("VacancyListId", vacancyId);
                ((IDictionary<String, Object>)exo).Add("JobAdvertisementId", id);

                var Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                await _cmsBusiness.CreateForm(Json, "", "CRPF_SELECTED_JOB_ADVERTISEMENT");
            }

            return Ok(new { success = true });
        }
        #endregion

        [HttpGet]
        [Route("ReadApplicationStateTrackDetails")]
        public async Task<IActionResult> ReadApplicationStateTrackDetails(string applicationId)
        {
            var list = await _recruitmentBusiness.GetAppStateTrackDetailsByCand(applicationId);
            return Ok(list);
        }

        [HttpGet]
        [Route("ApplicationStateTrack")]
        public async Task<IActionResult> ApplicationStateTrack(string applicationId)
        {
            var model = new ApplicationStateTrackDetailViewModel();
            var appdetails = await _recruitmentBusiness.GetApplicationDetailsById(applicationId);
            model.ApplicationId = applicationId;
            model.CandidateName = String.Concat(appdetails.FirstName, ' ', appdetails.MiddleName, ' ', appdetails.LastName);
            model.AppliedDate = appdetails.AppliedDate;
            model.JobName = appdetails.JobName;
            model.OrgName = appdetails.OrganizationName;
            model.ApplicationStateName = appdetails.ApplicationStateName;
            model.ApplicationStatusName = appdetails.ApplicationStatusName;
            model.BatchName = appdetails.BatchName;
            return Ok(model);
        }

        [HttpGet]
        [Route("JobAdvertisement")]
        public async Task<IActionResult> JobAdvertisement(string userId,string id, string jobId, string noteId, string serviceId, string statusCode, string orgId, long? requirement, string permission, bool isView = false,string layout=null)
        {
            var res = new JobAdvertisementViewModel();
            await Authenticate(userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var actionresult = await _lOVBusiness.GetList(x => x.LOVType == "LOV_JOBADVTACTION");
            var manpowertype = await _recruitmentBusiness.GetJobIdNameListByJobAdvertisement(jobId);
            if (manpowertype != null)
            {
                res.ManpowerType = manpowertype.ManpowerTypeName;
            }

            var approvalid = "";
            var submitid = "";
            var draftid = "";
            foreach (var a in actionresult)
            {
                if (a.Name == "Approve")
                {
                    approvalid = a.Id;
                }
                if (a.Name == "Submit")
                {
                    submitid = a.Id;
                }
                if (a.Name == "Draft")
                {
                    draftid = a.Id;
                }
            }
            res.ApprovalId = approvalid;
            res.SubmitId = submitid;
            res.DraftId = draftid;
            res.JobId = jobId;

            var temp = _userContext.UserRoleCodes.Split(",");
            res.UserRoleCodes = temp;
            if (requirement.IsNotNull())
            {
                res.NoOfPosition = requirement;
            }
            res.DataAction = DataActionEnum.Create;
            var jd = await _recruitmentBusiness.GetJobDescriptionData(jobId);
            if (jd != null)
            {
                res.Description = jd.Description;
                res.Responsibilities = jd.Responsibilities;
                res.Experience = jd.Experience;
                res.QualificationId = jd.QualificationId;
                res.JobCategoryId = jd.JobCategoryId;
                res.ShowJobDesc = jd.Id;
            }
            if (id.IsNotNull())
            {
                var model = await _recruitmentBusiness.GetJobAdvertisementData(id);

                if (manpowertype != null)
                {
                    model.ManpowerType = manpowertype.ManpowerTypeName;
                }
                model.DataAction = DataActionEnum.Edit;
                model.ApprovalId = approvalid;
                model.SubmitId = submitid;
                model.DraftId = draftid;
                model.UserRoleCodes = temp;
                model.Layout = layout;
                model.ShowJobDesc = null;
                model.IsView = isView;
                model.Agencies = model.AgencyId.IsNotNullAndNotEmpty() ? model.AgencyId.Trim('{', '}').Split(',').ToArray() : null;
                model.Status = StatusEnum.Active;
                model.IsView = isView;
                model.JobAdvNoteId = noteId;
                model.ServiceId = serviceId;
                model.ServiceStatusCode = statusCode;
                return Ok(model);
            }
            res.Status = StatusEnum.Active;
            res.IsView = isView;
            res.JobAdvNoteId = noteId;
            res.ServiceId = serviceId;
            res.ServiceStatusCode = statusCode;
            return Ok(res);
        }

    }
}
