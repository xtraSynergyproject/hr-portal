using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synergy.App.DataModel;

namespace CMS.UI.Web.Areas.Rec.Controllers
{
    [Area("Rec")]
    public class RecruitmentTransactionController : Controller
    {
        private readonly IRecruitmentTransactionBusiness _recruitmentBusiness;
        private readonly IUserContext _userContext;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly ICareerPortalBusiness _careerPortalBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        public RecruitmentTransactionController(IRecruitmentTransactionBusiness recruitmentBusiness, IUserContext userContext,
            IUserRoleBusiness userRoleBusiness, ICmsBusiness cmsBusiness, ILOVBusiness lOVBusiness, INoteBusiness noteBusiness,
            IServiceBusiness serviceBusiness, ICareerPortalBusiness careerPortalBusiness, ITableMetadataBusiness tableMetadataBusiness, ITemplateBusiness templateBusiness, ITaskBusiness taskBusiness)
        {
            _recruitmentBusiness = recruitmentBusiness;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _cmsBusiness = cmsBusiness;
            _lOVBusiness = lOVBusiness;
            _noteBusiness = noteBusiness;
            _serviceBusiness = serviceBusiness;
            _careerPortalBusiness = careerPortalBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _templateBusiness = templateBusiness;
            _taskBusiness = taskBusiness;
        }
        public async Task<IActionResult> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList)
        {
            var result = await _recruitmentBusiness.GetInboxTreeviewList(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
            var model = result.ToList();
            return Json(model);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Inbox()
        {
            return View();
        }
        public async Task<IActionResult> RecruitmentDashboard(string jobAdvId, string orgId, string permissions)
        {
            var model = new RecruitmentDashboardViewModel
            {
                JobAdvId = jobAdvId,
                OrganizationId = orgId
            };
            ViewBag.Permissions = permissions;
            ViewBag.Role = _userContext.UserRoleCodes;
            var count = await _recruitmentBusiness.GetPendingTaskListByUserId(_userContext.UserId);
            var PendingCount = count.Where(x => x.TaskStatusCode == "INPROGRESS" || x.TaskStatusCode == "OVERDUE").Count();
            model.GridTable = await _recruitmentBusiness.GetJobByOrgUnit(_userContext.UserId);
            ViewBag.PendingTaskCount = PendingCount;
            var userrole = _userContext.UserRoleIds.Split(",");
            if (userrole.Count() > 0)
            {
                model.UserRoleId = userrole[0];
            }
            //var DirectHiring = await _recTaskBusiness.GetList(x => x.TemplateCode == "DIRECT_HIRING" && x.TaskStatus != "Draft");
            //model.DirectHiring = DirectHiring.Count;

            return View(model);
        }
        public IActionResult JobAdvertisementIndex()
        {
            ViewBag.Permissions = "AddManpowerRecruitment";
            ViewBag.PortalId = _userContext.PortalId;
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetIdNameList(string type, string viewData = null)
        {
            var data = await _recruitmentBusiness.GetIdNameList(type);
            if (viewData != null)
            {
                ViewData[viewData] = data;
            }
            return Json(data);
        }
        public async Task<IActionResult> GetTaskByOrgUnit(/*[DataSourceRequest] DataSourceRequest request,*/ string userRoleId)
        {
            var Data = await _recruitmentBusiness.GetTaskByOrgUnit(_userContext.UserId, userRoleId);
            return Json(Data);
            //return Json(Data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetJobAdvertismentDashboard()
        {
            var result = await _recruitmentBusiness.GetJobIdNameDashboardList();
            result = result.OrderBy(x => x.JobName).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetOrganizationIdNameByRecruitmentList(string jobAddId)
        {
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
                return Json(list);
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
                return Json(list);
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetRecruitmentDashboardCount(string orgId)
        {
            var list = await _recruitmentBusiness.GetRecruitmentDashobardCount(orgId);
            return Json(list);
        }
        public async Task<JsonResult> GetUserPendingTaskByRole(/*[DataSourceRequest] DataSourceRequest request,*/ string orgId)
        {
            var list = await _recruitmentBusiness.GetPendingTaskDetailsForUser(_userContext.UserId, orgId, _userContext.UserRoleCodes);
            return Json(list);
        }
        [HttpGet]
        public async Task<ActionResult> GetRecruitmentDashboardByOrgJob(string orgId, string jobAdvId, string permission = "")
        {
            var data = await _recruitmentBusiness.GetManpowerRecruitmentSummaryByOrgJob(orgId, jobAdvId, permission);
            return Json(data);
        }
        public ActionResult ViewApplicationPendingTask()
        {
            //var model = new ApplicationViewModel();
            //model.OrganizationId = orgId;
            //model.JobId = jobId;
            return View("_ViewApplicationPendingTask");
        }
        public async Task<JsonResult> ReadViewApplicationPendingTaskData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var model = await _recruitmentBusiness.GetApplicationPendingTask(_userContext.UserId);
            if (_userContext.UserRoleCodes.Contains("HR"))
            {
                var model2 = await _recruitmentBusiness.GetApplicationWorkerPoolNotUnderApproval();
                if (model2 != null)
                {
                    model.ToList().AddRange(model2);
                }
            }
            //var data = model.ToDataSourceResult(request);
            return Json(model);
        }
        public async Task<IActionResult> JobAdvertisementStatistic(string jobAdvId, string state, string orgId, long count, string status, string permissions = "")
        {
            ViewBag.Permissions = permissions;
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
            return View(model);
        }

        //[HttpGet]
        //public async Task<ActionResult> GetRecruitmentDashboardByOrgJob(string orgId, string jobAdvId)
        //{
        //    var data = await _recruitmentBusiness.GetManpowerRecruitmentSummaryByOrgJob(orgId, jobAdvId);
        //    return Json(data);
        //}

        [HttpGet]
        public async Task<IActionResult> GetManpowerRequirement(string jobId, string orgId)
        {
            var data = await _recruitmentBusiness.GetManpowerRequirement(jobId, orgId);
            return Json(data);
        }

        public async Task<JsonResult> ReadTotalJobAdvertisementData(string jobAdvId, string orgId, string state, string tempcode, string nexttempcode, string visatempcode)
        {
            var model = await _recruitmentBusiness.GetTotalApplication(jobAdvId, orgId, state, tempcode, nexttempcode, visatempcode);

            return Json(model);
        }

        public async Task<IActionResult> ReadManpowerUniqueJobData()
        {
            var list = await _recruitmentBusiness.GetManpowerUniqueJobData();
            return Json(list);
        }

        public IActionResult JobAdvertisementInActive(string jobId, string permission)
        {
            var res = new JobAdvertisementViewModel();
            res.JobId = jobId;
            ViewBag.Permissions = permission;
            return View(res);
        }

        public IActionResult JobAdvertisementActive(string jobId, string orgid)
        {
            var res = new JobAdvertisementViewModel();
            res.JobId = jobId;
            return View(res);
        }

        public async Task<ActionResult> ReadJobadvtDataA(string jobid)
        {
            var temp = _userContext.UserRoleCodes.Split(",");
            var roleid = "";

            if (temp.Contains("HR"))
            {
                var hrrole = _userRoleBusiness.GetSingle(x => x.Code == "HR");
                roleid = hrrole.Result.Id;
            }
            else if (temp.Contains("CORPCOMP"))
            {
                var corprole = _userRoleBusiness.GetSingle(x => x.Code == "CORPCOMP");
                roleid = corprole.Result.Id;
            }

            var data = new List<JobAdvertisementViewModel>();
            if (jobid.IsNotNullAndNotEmpty() && roleid.IsNotNullAndNotEmpty())
            {
                var model = await _recruitmentBusiness.GetJobAdvertisement(jobid, roleid, StatusEnum.Active);
                data = model.ToList();
            }

            return Json(data);
        }

        public async Task<ActionResult> ReadJobadvtDataI(string jobid)
        {
            var roleid = "";

            var data = new List<JobAdvertisementViewModel>();
            if (jobid.IsNotNullAndNotEmpty())
            {
                var model = await _recruitmentBusiness.GetJobAdvertisement(jobid, roleid, StatusEnum.Inactive);
                data = model.ToList();
            }

            return Json(data);
        }

        public async Task<IActionResult> JobDescription(string jobId, string orgId, string taskStatus)
        {
            var jd = await _recruitmentBusiness.GetJobDescriptionData(jobId);

            if (jd != null)
            {
                jd.TaskStatus = taskStatus;
                jd.DataAction = DataActionEnum.Edit;
                return View(jd);
            }
            else
            {
                return View(new JobDescriptionViewModel
                {
                    JobId = jobId,
                    DataAction = DataActionEnum.Create,
                    TaskStatus = taskStatus
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetJobIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRJob", "");
            return Json(data);
        }

        public async Task<IActionResult> ReadJobDescCriteriaData(string jobdescid)
        {
            var data = new List<JobDescriptionCriteriaViewModel>();
            ViewBag.JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobdescid.IsNotNullAndNotEmpty())
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("Criteria", jobdescid);
                data = model.ToList();
            }
            return Json(data);
        }

        public async Task<IActionResult> ReadJobDescSkillsData(string jobdescid)
        {
            var data = new List<JobDescriptionCriteriaViewModel>();
            ViewBag.JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobdescid.IsNotNullAndNotEmpty())
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("Skills", jobdescid);
                data = model.ToList();
            }
            return Json(data);
        }

        public async Task<IActionResult> ReadJobDescOtherInfoData(string jobdescid)
        {
            var data = new List<JobDescriptionCriteriaViewModel>();
            ViewBag.JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobdescid.IsNotNullAndNotEmpty())
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("OtherInformation", jobdescid);
                data = model.ToList();
            }
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageJobDescription(JobDescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_JOB_DESCRIPTION";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    notemodel.Json = JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _noteBusiness.ManageNote(notemodel);

                    if (result.IsSuccess)
                    {
                        if (model.JobCriteria.IsNotNull())
                        {
                            var list = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.JobCriteria);
                            await CreateJobDescriptionCriteria(list, result.Item.UdfNoteTableId, "Criteria");
                        }

                        if (model.Skills.IsNotNull())
                        {
                            var list = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.Skills);
                            await CreateJobDescriptionCriteria(list, result.Item.UdfNoteTableId, "Skills");
                        }
                        if (model.OtherInformation.IsNotNull())
                        {
                            var list = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.OtherInformation);
                            await CreateJobDescriptionCriteria(list, result.Item.UdfNoteTableId, "OtherInformation");
                        }
                        return Json(new { success = true, data = result.Item.Id });
                    }
                    else
                    {
                        return Json(new { success = true, error = result.Messages.ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.JobDescriptionNoteId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    notemodel.Json = JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(notemodel);

                    if (result.IsSuccess)
                    {
                        if (model.JobCriteria.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.JobCriteria);
                            var existingjobcriteria = await _recruitmentBusiness.GetJobDescCriteriaList("Criteria", model.Id);
                            if (existingjobcriteria.IsNotNull())
                            {
                                foreach (var p in existingjobcriteria)
                                {
                                    var noteTempModel1 = new NoteTemplateViewModel();
                                    noteTempModel1.SetUdfValue = true;
                                    noteTempModel1.NoteId = p.NoteId;
                                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                                    var res = await _noteBusiness.DeleteNote(notemodel1);
                                }
                            }
                            if (jobcriteria.IsNotNull())
                            {
                                var list = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.JobCriteria);
                                await CreateJobDescriptionCriteria(list, result.Item.UdfNoteTableId, "Criteria");
                            }
                        }

                        if (model.Skills.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.Skills);
                            var existingjobcriteria = await _recruitmentBusiness.GetJobDescCriteriaList("Skills", model.Id);
                            if (existingjobcriteria.IsNotNull())
                            {
                                foreach (var p in existingjobcriteria)
                                {
                                    var noteTempModel1 = new NoteTemplateViewModel();
                                    noteTempModel1.SetUdfValue = true;
                                    noteTempModel1.NoteId = p.NoteId;
                                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                                    var res = await _noteBusiness.DeleteNote(notemodel1);
                                }
                            }
                            if (jobcriteria.IsNotNull())
                            {
                                var list = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.JobCriteria);
                                await CreateJobDescriptionCriteria(list, result.Item.UdfNoteTableId, "Skills");
                            }

                        }
                        if (model.OtherInformation.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.OtherInformation);
                            var existingjobcriteria = await _recruitmentBusiness.GetJobDescCriteriaList("OtherInformation", model.Id);
                            if (existingjobcriteria.IsNotNull())
                            {
                                foreach (var p in existingjobcriteria)
                                {
                                    var noteTempModel1 = new NoteTemplateViewModel();
                                    noteTempModel1.SetUdfValue = true;
                                    noteTempModel1.NoteId = p.NoteId;
                                    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                                    var res = await _noteBusiness.DeleteNote(notemodel1);
                                }
                            }
                            if (jobcriteria.IsNotNull())
                            {
                                var list = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.JobCriteria);
                                await CreateJobDescriptionCriteria(list, result.Item.UdfNoteTableId, "OtherInformation");
                            }
                        }
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, error = result.Messages });
                    }
                }
            }
            return Json(new { success = true });
        }

        public async Task CreateJobDescriptionCriteria(List<JobDescriptionCriteriaViewModel> list, string jobDescId, string type)
        {
            foreach (var item in list)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "JOB_DESCRIPTION_CRITERIA";
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("JobDescriptionId", jobDescId);
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

        public async Task<IActionResult> JobAdvertisement(string summaryId, string id, string jobId, string noteId, string serviceId, string statusCode, string orgId, long? requirement, string permission, bool isView = false, string layout = null)
        {
            var res = new JobAdvertisementViewModel();

            ViewBag.Permission = permission;
            ViewBag.JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            //var actionresult = await _applicationBusiness.GetListOfValueByType("LOV_JOBADVTACTION");
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
            if (layout.IsNotNull())
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
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
                model.Agencies = model.AgencyId.IsNotNullAndNotEmpty()? model.AgencyId.Trim('{', '}').Split(',').ToArray(): null;
                model.Status = StatusEnum.Active;
                model.IsView = isView;
                model.JobAdvNoteId = noteId;
                model.ServiceId = serviceId;
                model.ServiceStatusCode = statusCode;
                return View(model);
            }
            res.Status = StatusEnum.Active;
            res.IsView = isView;
            res.JobAdvNoteId = noteId;
            res.ServiceId = serviceId;
            res.ServiceStatusCode = statusCode;
            return View(res);
        }

        public async Task<IActionResult> ReadAgencyList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("REC_AGENCY", "");
            return Json(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetLocationIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRLocation", "");
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetNationallityIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRNationality", "");
            return Json(data);
        }

        public async Task<IActionResult> ReadJobAdvCriteriaData(string jobadvtid, string jobdescid)
        {
            var data = new List<JobCriteriaViewModel>();
            ViewBag.JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobadvtid.IsNotNullAndNotEmpty() && jobdescid.IsNullOrEmpty())
            {
                var model = await _recruitmentBusiness.GetJobCriteriaList("Criteria", jobadvtid);
                data = model.ToList();
            }
            else
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("Criteria", jobdescid);
                return Json(model);
            }

            return Json(data);
        }

        public async Task<IActionResult> ReadJobAdvSkillsData(string jobadvtid, string jobdescid)
        {
            var data = new List<JobCriteriaViewModel>();
            ViewBag.JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobadvtid.IsNotNullAndNotEmpty() && jobdescid.IsNullOrEmpty())
            {
                var model = await _recruitmentBusiness.GetJobCriteriaList("Skills", jobadvtid);
                data = model.ToList();
            }
            else
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("Skills", jobdescid);
                return Json(model);
            }

            return Json(data);
        }

        public async Task<IActionResult> ReadJobAdvOtherInfoData(string jobadvtid, string jobdescid)
        {
            var data = new List<JobCriteriaViewModel>();
            ViewBag.JobCriterias = await _lOVBusiness.GetList(x => x.LOVType == "CRITERIA_TYPE");
            if (jobadvtid.IsNotNullAndNotEmpty() && jobdescid.IsNullOrEmpty())
            {
                var model = await _recruitmentBusiness.GetJobCriteriaList("OtherInformation", jobadvtid);
                data = model.ToList();
            }
            else
            {
                var model = await _recruitmentBusiness.GetJobDescCriteriaList("OtherInformation", jobdescid);
                return Json(model);
            }

            return Json(data);
        }

        public IActionResult ViewListOfValue(string jobid)
        {
            return View(new LOVViewModel
            {
                ReferenceType = ReferenceTypeEnum.REC_Job,
                ReferenceId = jobid,
            });
        }

        public async Task<ActionResult> GetLOVList(string jobid)
        {
            var data = await _lOVBusiness.GetList(x => x.LOVType == "LIST_OF_VALUE_TYPE" && x.ReferenceId == jobid && x.Status != StatusEnum.Inactive);
            return Json(data);
        }

        public async Task<ActionResult> GetLOVTypeList(string code, string refId)
        {
            var data = await _lOVBusiness.GetList(x => x.LOVType == code && x.ReferenceId == refId && x.Status != StatusEnum.Inactive);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetListOfValueTypeList(string type, bool includePlaceHolder = false, string refTypeId = null)
        {
            var data = await _lOVBusiness.GetList(x => x.LOVType == type && x.ReferenceId == refTypeId && x.Status != StatusEnum.Inactive);
            if (includePlaceHolder)
            {
                data.Insert(0, new LOVViewModel() { Id = "0", Name = "Select" });
            }
            return Json(data.OrderBy(x => x.SequenceOrder));
        }


        public async Task<IActionResult> ManageListOfValue(string Id, string refTypeId)
        {
            var ListOfValue = await _lOVBusiness.GetSingleById(Id);

            if (ListOfValue != null)
            {
                ListOfValue.DataAction = DataActionEnum.Edit;
                return View(ListOfValue);
            }
            return View(new LOVViewModel() { ReferenceId = refTypeId, ReferenceType = ReferenceTypeEnum.REC_Job, DataAction = DataActionEnum.Create });
        }

        [HttpPost]
        public async Task<IActionResult> ManageListOfValue(LOVViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lovcode = String.Concat(model.Name.Where(c => !Char.IsWhiteSpace(c)));

                var exist = await _lOVBusiness.GetSingle(x => x.Name == model.Name && x.Id != model.Id);
                if (exist != null)
                {
                    return Json(new { success = false, error = "LOV Type Name already exist" });
                }
                else
                {
                    if (model.Json.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Enter LOV values" });
                    }
                    else
                    {
                        var lovs = JsonConvert.DeserializeObject<List<LOVViewModel>>(model.Json);
                        foreach (var item in lovs)
                        {
                            if (item.Name.IsNullOrEmptyOrWhiteSpace())
                            {
                                return Json(new { success = false, error = "Enter LOV name" });
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
                            return Json(new { success = true, lovtype = result.Item.Code, id = result.Item.Id });
                        }
                        else
                        {
                            return Json(new { success = false, result.Messages });
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

                            return Json(new { success = true, lovtype = result.Item.Code, id = result.Item.Id, action = "Edit" });

                        }
                        else
                        {
                            return Json(new { success = true, result.Messages });
                        }
                    }
                }
            }

            return View("ManageListOfValue", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageJobAdvertisement(JobAdvertisementViewModel model)
        {
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
                                await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Criteria");
                            }

                            if (model.Skills.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Skills");
                            }
                            if (model.OtherInformation.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "OtherInformation");
                            }

                            return Json(new { success = true });
                        }
                        else
                        {
                            return Json(new { success = false, error = result.HtmlError });
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
                                    await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Criteria");
                                }

                                if (model.Skills.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                    await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "Skills");
                                }
                                if (model.OtherInformation.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                    await CreateJobAdvertisementCriteria(jobcriteria, result.Item.UdfNoteTableId, "OtherInformation");
                                }

                                //await _taskBusiness.AssignTaskForJobAdvertisement(model.Id, model.JobName, model.CreatedDate, null);
                                return Json(new { success = true });
                            }
                            else
                            {
                                return Json(new { success = false, error = result.HtmlError });
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
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Criteria");
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
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Skills");
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
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "OtherInformation");
                                }
                            }
                            return Json(new { success = true });
                        }
                        else
                        {
                            return Json(new { success = false, error = update.HtmlError });
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
                        sermodel.StartDate = DateTime.Now;
                        sermodel.DueDate = DateTime.Now.AddDays(3);
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
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Criteria");
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
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "Skills");
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
                                    await CreateJobAdvertisementCriteria(jobcriteria, model.Id, "OtherInformation");
                                }
                            }

                            return Json(new { success = true });
                        }
                        else
                        {
                            return Json(new { success = false, error = result.HtmlError });
                        }
                    }
                }
            }

            return Json(new { success = false });

        }

        public async Task CreateJobAdvertisementCriteria(List<JobCriteriaViewModel> list, string jobAdvId, string type)
        {

            foreach (var item in list)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.ActiveUserId = _userContext.UserId;
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

        public IActionResult ManpowerRecruitmentSummaryIndex(string pageId, string permissions)
        {
            ViewBag.Permissions = permissions;
            ViewBag.PortalId = _userContext.PortalId;
            return View();
        }
        public async Task<ActionResult> ReadManpowerRequirementSummaryData()
        {
            var res = await _recruitmentBusiness.GetManpowerRecruitmentSummaryData();
            return Json(res);
        }

        public async Task<IActionResult> DeleteManpowerRequirement(string serviceId)
        {
            //var state = await _recruitmentBusiness.GetState(serviceId);
            var sermodel = new ServiceTemplateViewModel() { ServiceId = serviceId, ActiveUserId = _userContext.UserId };
            var model = await _serviceBusiness.GetServiceDetails(sermodel);
            model.SetUdfValue = true;
            await _serviceBusiness.DeleteService(model);
            await _serviceBusiness.Delete(serviceId);
            return Json(new { success = true });

            //if (state != null)
            //{
            //    if (state.ActionName == "Draft" || state.ActionName == null)
            //    {                    
            //await _serviceBusiness.DeleteService(model);
            //return Json(new { success = true });
            //    }
            //    else
            //    {
            //        return Json(new { success = false });
            //    }
            //}
            //else
            //{
            //    await _serviceBusiness.DeleteService(model);
            //    return Json(new { success = true });
            //}
        }

        public ActionResult ViewApplicationDetails(string jobId, string orgId)
        {
            var model = new ApplicationViewModel();
            model.OrganizationId = orgId;
            model.JobId = jobId;
            return View(model);
        }
        public async Task<JsonResult> ReadViewApplicationDetailsData(string JobId, string OrgId)
        {
            var model = await _recruitmentBusiness.GetViewApplicationDetails(JobId, OrgId);
            return Json(model);
        }
        [HttpGet]
        public async Task<ActionResult> GetOrgIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("HRDepartment", "");
            return Json(data);
        }

        public async Task<IActionResult> RecruitmentElement(string appId, string state, string taskStatus)
        {
            var currentTaskStatus = await _lOVBusiness.GetSingleById(taskStatus);
            var model = new RecCandidateElementInfoViewModel() { SalaryRevision = false };
            var temp = _userContext.UserRoleCodes.Split(",");

            if (temp.Contains("HR"))
            {
                model.IsHr = true;
            }
            if (currentTaskStatus.IsNotNull())
            {
                model.TaskStatus = currentTaskStatus.Code;
            }
            model.ReferenceId = appId;
            model.ApplcationStateName = state;
            if (appId.IsNotNull())
            {
                var appmodel = await _recruitmentBusiness.GetApplicationDetailsById(model.ReferenceId);

                model.ApplicantName = appmodel.FirstName + ' ' + appmodel.MiddleName + ' ' + appmodel.LastName;

                if (appmodel.JobId.IsNotNullAndNotEmpty())
                {
                    model.OfferDesigination = appmodel.JobId;
                }
                var manpowertype = await _recruitmentBusiness.GetJobIdNameListByJobAdvertisement(appmodel.JobId);
                model.OfferGrade = appmodel.OfferGrade;
                model.OfferDesigination = appmodel.OfferDesigination ?? appmodel.JobId;
                model.GaecNo = appmodel.GaecNo;
                model.JoiningDate = appmodel.JoiningDate;
                model.OfferSignedBy = appmodel.OfferSignedBy ?? "SATISH G. PILLAI";
                model.AnnualLeave = appmodel.AnnualLeave;
                model.AnnualLeave = appmodel.AnnualLeave;
                model.SalaryRevision = appmodel.SalaryRevision;
                model.SalaryRevisionAmount = appmodel.SalaryRevisionAmount;
                model.SalaryOnAppointment = appmodel.SalaryOnAppointment.IsNotNullAndNotEmpty() ? Int32.Parse(appmodel.SalaryOnAppointment) : 0;
                model.VisaCategory = appmodel.VisaCategoryId;
                model.AccommodationId = appmodel.AccommodationId;
                model.TravelOriginAndDestination = "Mumbai - Doha";

                if (model.ApplcationStateName == "FinalOfferSent")
                {
                    if (appmodel.FinalOfferReference.IsNullOrEmpty())
                    {
                        var str = await GenerateFinalOfferRef(appmodel.ApplicationNo);
                        appmodel.FinalOfferReference = str;

                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.NoteId = appmodel.ApplicationNoteId;
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.ActiveUserId = _userContext.UserId;

                        var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        noteModel.Json = JsonConvert.SerializeObject(appmodel);

                        var noteresult = await _noteBusiness.ManageNote(noteModel);

                        //await _recruitmentBusiness.UpdateFinalOfferReference(appmodel);

                    }
                    if ((manpowertype.ManpowerTypeCode == "Worker" || manpowertype.ManpowerTypeCode == "UnskilledWorker") && appmodel.OfferGrade == null)
                    {
                        var grade = await _recruitmentBusiness.GetGradeIdNameList("10");
                        model.OfferGrade = grade.FirstOrDefault().Id;
                    }

                    model.FinalOfferReference = appmodel.FinalOfferReference;
                    model.ContractStartDate = appmodel.ContractStartDate == null ? DateTime.Now : appmodel.ContractStartDate;
                    model.JoiningNotLaterThan = appmodel.JoiningNotLaterThan == null ? DateTime.Now.AddDays(30) : appmodel.JoiningNotLaterThan;
                    model.IsTrainee = appmodel.IsTrainee;

                    model.ServiceCompletion = appmodel.ServiceCompletion;
                    model.TravelOriginAndDestination = appmodel.TravelOriginAndDestination == null ? "Mumbai - Doha" : appmodel.TravelOriginAndDestination;
                    model.VehicleTransport = appmodel.VehicleTransport;
                    model.IsLocalCandidate = appmodel.IsLocalCandidate;
                    model.ServiceCompletion = appmodel.ServiceCompletion ?? 6;
                }
            }

            return View(model);
        }

        public async Task<string> GenerateFinalOfferRef(string appno)
        {
            var id = await _recruitmentBusiness.GenerateFinalOfferSeq();
            return string.Concat(appno, "-", "F", "-", id);
        }


        public async Task<JsonResult> ReadPayElementData(string appid)
        {
            var model = await _recruitmentBusiness.GetElementData(appid);
            var appmodel = await _recruitmentBusiness.GetApplicationDetailsById(appid);
            foreach (var a in model)
            {
                if (a.ElementName == "Basic")
                {
                    if (a.Value == null && appmodel.SalaryOnAppointment != null)
                    {
                        a.Value = Double.Parse(appmodel.SalaryOnAppointment);
                    }
                }
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageApplicationElement(RecCandidateElementInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appmodel = await _recruitmentBusiness.GetApplicationDetailsById(model.ReferenceId);
                appmodel.OfferDesigination = model.OfferDesigination;
                appmodel.OfferGrade = model.OfferGrade;
                appmodel.GaecNo = model.GaecNo;
                appmodel.JoiningDate = model.JoiningDate;
                appmodel.OfferSignedBy = model.OfferSignedBy;
                appmodel.AnnualLeave = model.AnnualLeave;
                appmodel.AccommodationId = model.AccommodationId;
                appmodel.VisaCategoryId = model.VisaCategory;
                appmodel.SalaryRevision = model.SalaryRevision;
                appmodel.SalaryRevisionAmount = model.SalaryRevisionAmount;
                appmodel.SalaryOnAppointment = model.SalaryOnAppointment.ToString();

                if (model.ApplcationStateName == "FinalOfferSent")
                {
                    appmodel.ContractStartDate = model.ContractStartDate;
                    appmodel.JoiningNotLaterThan = model.JoiningNotLaterThan;
                    appmodel.IsTrainee = model.IsTrainee;

                    appmodel.ServiceCompletion = model.ServiceCompletion;
                    appmodel.TravelOriginAndDestination = model.TravelOriginAndDestination;
                    appmodel.VehicleTransport = model.VehicleTransport;
                    appmodel.IsLocalCandidate = model.IsLocalCandidate;
                }

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = appmodel.ApplicationNoteId;
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;

                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                noteModel.Json = JsonConvert.SerializeObject(appmodel);

                var noteresult = await _noteBusiness.ManageNote(noteModel);
                //await _applicationBusiness.Edit(appmodel);

                if (model.JsonPayElement.IsNotNull())
                {
                    var JsonPayElement = JsonConvert.DeserializeObject<List<RecCandidatePayElementViewModel>>(model.JsonPayElement);

                    foreach (var a in JsonPayElement)
                    {
                        var jc = new RecCandidateElementInfoViewModel();
                        jc.ReferenceId = model.ReferenceId;
                        jc.Value = a.Value;
                        jc.ElementId = a.PayId;
                        jc.Comment = a.Comment;
                        if (a.ElementId.IsNotNullAndNotEmpty())
                        {
                            jc.Id = a.Id;

                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.NoteId = a.NoteId;
                            noteTempModel1.DataAction = DataActionEnum.Edit;
                            noteTempModel1.ActiveUserId = _userContext.UserId;

                            var noteModel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                            noteModel1.Json = JsonConvert.SerializeObject(jc);

                            var noteresult1 = await _noteBusiness.ManageNote(noteModel1);

                            //var res = await _recruitmentElementBusiness.Edit(jc);
                            if (noteresult1.IsSuccess)
                            {
                                ViewBag.Success = true;
                            }

                        }
                        else if (a.Value.IsNotNull() || a.Comment.IsNotNullAndNotEmpty())
                        {
                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.TemplateCode = "REC_PAY_ELEMENT_CANDIDATE";
                            noteTempModel1.DataAction = DataActionEnum.Create;
                            noteTempModel1.ActiveUserId = _userContext.UserId;

                            var noteModel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                            noteModel1.Json = JsonConvert.SerializeObject(jc);

                            var noteresult1 = await _noteBusiness.ManageNote(noteModel1);

                            //var res = await _recruitmentElementBusiness.Create(jc);  
                            if (noteresult1.IsSuccess)
                            {
                                ViewBag.Success = true;
                            }
                        }
                    }
                }
            }

            if (model.ApplcationStateName != "FinalOfferSent")
            {
                return RedirectToAction("IntentToOffer", new { appid = model.ReferenceId });
            }
            else
            {
                //return RedirectToAction("FinalOffer", new { applicationId = model.ReferenceId });
                 return Redirect("~/Cms/FastReport?rptName=REC_OfferLetter&lo="+LayoutModeEnum.Popup+ "&rptUrl=rec/query/GetOfferLetter/"+ model.ReferenceId);
            }

            return View("RecruitmentElement", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetGradeIdNameList()
        {
            var list = await _recruitmentBusiness.GetGradeIdNameList();
            return Json(list);
        }

        public async Task<JsonResult> ReadJobAdvertisementData(string jobAdvId, string orgId, string state, string tempcode, string nexttempcode, string visatempcode, string status)
        {
            var model = await _recruitmentBusiness.GetJobAdvertismentState(jobAdvId, orgId, state, tempcode, nexttempcode, visatempcode, status);
            return Json(model);
        }


        public async Task<JsonResult> GetDirectHiringData(string jobAdvId, string orgId)
        {
            var model = await _recruitmentBusiness.GetDirictHiringData(jobAdvId, orgId);
            return Json(model);
        }

        public async Task<JsonResult> GetJobAdvertismentApplication(string jobAdvId, string orgId, string state, string tempcode, string tempCodeOther)
        {
            var model = await _recruitmentBusiness.GetJobAdvertismentApplication(jobAdvId, orgId, state, tempcode, tempCodeOther);

            return Json(model);

        }
        //public async Task<JsonResult> GetDirectHiringData(string jobAdvId, string orgId)
        //{
        //    var model = await _recruitmentBusiness.GetDirictHiringData(jobAdvId, orgId);           
        //    return Json(model);
        //}
        public async Task<IActionResult> JoiningReport(string appid)
        {
            // var model = new ApplicationViewModel();

            var appmodel = await _careerPortalBusiness.GetAppDetailsById(appid);
            var sign = await _recruitmentBusiness.GetUserSign();
            if (sign != null)
            {
                appmodel.SignatureId = sign.Id;
            }

            var jd = await _recruitmentBusiness.GetJobManpowerType(appmodel.JobId);
            if (jd.Code != "Staff")
            {


                if (appmodel.NationalityId.IsNotNullAndNotEmpty())
                {
                    var gradename = await _recruitmentBusiness.GetNationalitybyId(appmodel.NationalityId);
                    if (gradename.IsNotNull())
                    {
                        appmodel.NationalityName = gradename.Name;
                    }
                }
                if (appmodel.TitleId.IsNotNullAndNotEmpty())
                {
                    var gradename = await _recruitmentBusiness.GetTitlebyId(appmodel.TitleId);
                    if (gradename.IsNotNull())
                    {
                        appmodel.TitleName = gradename.Name;
                    }
                }
                if (appmodel.JobId.IsNotNullAndNotEmpty())
                {
                    var jobname = await _recruitmentBusiness.GetJobNameById(appmodel.JobId);
                    appmodel.OfferDesigination = jobname.Name;
                }
                if (jd.Code == "Worker" || jd.Code == "UnskilledWorker" || jd.Code == "Welder")
                {
                    ViewBag.type = "Workers";
                }
                if (appid != null)
                {
                    return View("JoiningReport", appmodel);
                }
                else
                {
                    return View("JoiningReport", new RecApplicationViewModel());
                }
            }
            else
            {
                var staffDetails = await _recruitmentBusiness.GetStaffJoiningDetails(appid);

                if (staffDetails.JobId.IsNotNullAndNotEmpty())
                {
                    var jobname = await _recruitmentBusiness.GetJobNameById(appmodel.JobId);
                    staffDetails.OfferDesigination = jobname.Name;
                }
                staffDetails.JoiningTime = String.Format("{0:HH:mm}", staffDetails.CandJoiningDate);
                return View("StaffJoiningReport", staffDetails);
            }
        }

        public async Task<IActionResult> PersonalData(string appid)
        {
            if (appid != null)
            {
                var appmodel = await _careerPortalBusiness.GetAppDetailsById(appid);


                if (appmodel.NationalityId.IsNotNullAndNotEmpty())
                {
                    var gradename = await _recruitmentBusiness.GetNationalitybyId(appmodel.NationalityId);
                    appmodel.NationalityName = gradename.Name;
                }

                if (appmodel.TitleId.IsNotNullAndNotEmpty())
                {
                    var gradename = await _recruitmentBusiness.GetTitlebyId(appmodel.TitleId);
                    appmodel.TitleName = gradename.Name;
                }
                return View("PersonalData", appmodel);
            }
            else
            {
                return View("PersonalData", new RecApplicationViewModel());
            }

        }

        public async Task<IActionResult> Declaration(string appid)
        {
            // var model = new ApplicationViewModel();
            if (appid != null)
            {
                var appmodel = await _recruitmentBusiness.GetApplicationDeclarationData(appid);

                return View("Declaration", appmodel);

            }
            else
            {
                return View("Declaration", new RecApplicationViewModel());
            }

        }

        public IActionResult AnnexureForStaff()
        {
            return View();
        }



        public async Task<IActionResult> ConfidentialityAgreement(string appId)
        {
            if (appId != null)
            {
                var cadetails = await _recruitmentBusiness.GetConfidentialAgreementDetails(appId);
                // cadetails.FullName = String.Concat(cadetails.FirstName, ' ', cadetails.MiddleName, ' ', cadetails.LastName);            
                return View("ConfidentialityAgreement", cadetails);
            }
            else
            {
                return View("ConfidentialityAgreement", new RecApplicationViewModel());
            }
        }

        public async Task<IActionResult> CompetenceMatrix(string appId)
        {
            if (appId != null)
            {
                var cmdetails = await _recruitmentBusiness.GetCompetenceMatrixDetails(appId);
                cmdetails.FullName = String.Concat(cmdetails.FirstName, ' ', cmdetails.MiddleName, ' ', cmdetails.LastName);
                cmdetails.CandJoiningDate = cmdetails.JoiningNotLaterThan;
                return View("CompetenceMatrix", cmdetails);
            }
            else
            {
                return View("CompetenceMatrix", new RecApplicationViewModel());
            }
        }

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
            ViewBag.LoginUserId = _userContext.UserId;
            var userrole = _userContext.UserRoleCodes.Split(",");
            ViewBag.IsUserHR = userrole.Contains("HR");
            return View(model);
        }

        public async Task<IActionResult> ReadApplicationStateTrackDetails(string applicationId)
        {
            var list = await _recruitmentBusiness.GetAppStateTrackDetailsByCand(applicationId);
            return Json(list);
        }

        public IActionResult CandidateShortlist(string jobAdvId, string orgId, string batchId, string code, LayoutModeEnum layoutMode = LayoutModeEnum.Main)
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
        public async Task<JsonResult> ReadInterviewCandidates(string BatchId, string Mode)
        {
            var search = new ApplicationSearchViewModel()
            {
                BatchId = BatchId,
                Mode = Mode,
                ApplicationStateCode = "ShortListByHm",
                BatchStatusCode = "PendingWithHM",
                ApplicationStatusCode = "NotShortlisted",
            };

            var data = await _recruitmentBusiness.GetCandiadteShortListApplicationData(search);
            return Json(data);
        }

        public async Task<JsonResult> ReadFutureCandidates(string BatchId, string Mode)
        {
            var search = new ApplicationSearchViewModel()
            {
                BatchId = BatchId,
                Mode = Mode,
                ApplicationStateCode = "ShortListByHm",
                BatchStatusCode = "PendingWithHM",
                ApplicationStatusCode = "ShortlistForFuture",
            };

            var data = await _recruitmentBusiness.GetCandiadteShortListApplicationData(search);
            return Json(data);
        }

        public async Task<JsonResult> ReadJobOfferCandidatesData(string BatchId, string Mode)
        {
            var search = new ApplicationSearchViewModel()
            {
                BatchId = BatchId,
                Mode = Mode,
                ApplicationStateCode = "ShortListByHm",
                BatchStatusCode = "PendingWithHM",
                ApplicationStatusCode = "ShortlistedHM",
                TemplateCode = "SCHEDULE_INTERVIEW",
            };

            if (search.Mode == "InterviewRequested")
            {
                search.ApplicationStatusCode = "InterviewRequested";
            }
            var model = await _recruitmentBusiness.GetCandiadteShortListApplicationData(search);
            if (search.Mode == "ShortlistedByHM")
            {
                model = model.Where(x => x.TaskId == null).ToList();
            }
            else if (search.Mode == "InterviewRequested")
            {
                model = model.Where(x => x.TaskId != null).ToList();
            }

            return Json(model);
        }

        public async Task<ActionResult> ReadCloseBatchHmData(string jobAdvertismentId, string orgId, string hmId, BatchTypeEnum batchtype, string batchId)
        {
            IList<RecBatchViewModel> model1 = await _recruitmentBusiness.GetBatchHmData(jobAdvertismentId, orgId, hmId, batchtype, batchId);

            return Json(model1);
        }

        public async Task<IActionResult> CreateApplicationTrackforHm(string applicantIds)
        {
            var applicantlist = applicantIds.Split(",");

            foreach (var appId in applicantlist)
            {
                if (appId.IsNotNullAndNotEmpty())
                {
                    var status = await _recruitmentBusiness.CreateApplicationStatusTrack(appId);
                }
            }
            return Json(new { success = true });
        }
        #region Evaluation
        public async Task<JsonResult> GetHiringManagerList()
        {
            var hmmodelList = await _recruitmentBusiness.GetHiringManagersList();
            return Json(hmmodelList);
        }
        public async Task<JsonResult> GetJobIdNameDataList()
        {
            var joblist = await _recruitmentBusiness.GetJobIdNameDataList();
            return Json(joblist);
        }
        public async Task<IActionResult> CandidateEvaluationScaleReview(string applicationId, string taskStatus, string mode)
        {

            //applicationId = "4ae2e955-26e5-4dad-8c66-ea0540412ba1";
            if (taskStatus == "COMPLETED")
            {
                return RedirectToAction("CandidateEvaluationView", new { applicationId = applicationId });
            }

            var model = await _recruitmentBusiness.GetApplicationEvaluationDetails(applicationId);
            model.Mode = mode;
            var manpowertype = await _recruitmentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
            model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
            if (model.ManpowerTypeCode != "STAFF")
            {
                var grade = await _recruitmentBusiness.GetGradeIdNameList("10");
                if (grade != null && grade.Count()>0)
                {
                    model.OfferGrade = grade.FirstOrDefault().Id;
                }

            }

            var evallist = await _recruitmentBusiness.GetCandidateEvaluationDetails(applicationId);
            if (evallist != null && evallist.Count() > 0)
            {
                model.EvaluationData = evallist.OrderBy(x => x.SequenceOrder).ToList();
                model.EvaluationDataString = JsonConvert.SerializeObject(model.EvaluationData);
                model.IsCandidateEvaluation = true;
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                evallist = await _recruitmentBusiness.GetCandidateEvaluationTemplateData();
                model.EvaluationData = evallist.OrderBy(x => x.SequenceOrder).ToList();
                model.EvaluationDataString = JsonConvert.SerializeObject(model.EvaluationData);
                model.InterviewByUserId = _userContext.UserId;
                model.InterviewByUserName = _userContext.Name;
                model.DataAction = DataActionEnum.Edit;
                model.IsCandidateEvaluation = false;
            }
            return View("_CandidateEvaluationScaleReview", model);

        }

        public async Task<IActionResult> CandidateEvaluationView(string applicationId, string versionNo)
        {

            if (versionNo.IsNotNullAndNotEmpty())
            {
                var model = await _recruitmentBusiness.GetApplicationEvaluationDetails(applicationId);//new line
                //    var model = await _applicationBusiness.GetApplicationVersionEvaluationDetails(applicationId);
                //    if (model != null)
                //    {
                //        var evallist = await _candidateEvaluationVersionBusiness.GetList(x => x.ApplicationVersionId == model.Id);
                //        var version = evallist.OrderBy(x => x.SequenceOrder).ToList();
                //        model.EvaluationData = _mapper.Map<List<CandidateEvaluationVersionViewModel>, List<CandidateEvaluationViewModel>>(version);
                //        var manpowertype = await _jobAdvrtismentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                //        model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
                //    }
                model.DataAction = DataActionEnum.Read;
                return View("_CandidateEvaluationView", model);
            }
            else
            {
                var model = await _recruitmentBusiness.GetApplicationEvaluationDetails(applicationId);
                if (model != null)
                {
                    var evallist= await _recruitmentBusiness.GetCandidateEvaluationDetails(model.Id);
                    model.EvaluationData = evallist.OrderBy(x => x.SequenceOrder).ToList();
                    var manpowertype = await _recruitmentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                    model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
                }
                model.DataAction = DataActionEnum.Read;
                return View("_CandidateEvaluationView", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageCandidateEvaluationScaleReview(RecApplicationViewModel model)
        {

            var newModel = await _recruitmentBusiness.GetApplicationDetailsById(model.Id);
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
                if ((newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected) && model.ManpowerTypeCode == "STAFF")
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
                            if (newModel.ManpowerTypeCode == "STAFF")
                            {
                                status = "SHORTLISTED";
                            }
                            else
                            {
                                status = "SELECTED";
                            }
                        }
                        else if (newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Rejected)
                        {
                            status = "REJECTED";
                        }
                        if (status.IsNotNullAndNotEmpty())
                        {
                            //var status1 = await _applicationBusiness.GetApplicationStatusByCode(status);
                            var status1 = await _lOVBusiness.GetSingle(x => x.Code == status && x.LOVType == "APPLICATION_STATUS");
                            newModel.ApplicationStatus = status1.Id;
                        }
                    }

                    var applicationNoteTempModel = new NoteTemplateViewModel();
                    applicationNoteTempModel.DataAction = DataActionEnum.Edit;
                    applicationNoteTempModel.ActiveUserId = _userContext.UserId;
                    applicationNoteTempModel.TemplateCode = "REC_APPLICATION";
                    applicationNoteTempModel.NoteId = newModel.ApplicationNoteId;
                    var applicationNoteModel = await _noteBusiness.GetNoteDetails(applicationNoteTempModel);

                    applicationNoteModel.Json = JsonConvert.SerializeObject(newModel);

                    var result = await _noteBusiness.ManageNote(applicationNoteModel);

                    if (result.IsSuccess)
                    {
                        //var applicationversionId = "";
                        //var check = await _applicationVersionBusiness.GetList(x => x.ApplicationId == newModel.Id);
                        //if (model.Mode == "EDIT" && check.Count == 0)
                        //{
                        //    var version = _mapper.Map<ApplicationViewModel, ApplicationVersionViewModel>(newModel);
                        //    version.Id = null;
                        //    version.DataAction = DataActionEnum.Create;
                        //    version.ApplicationId = newModel.Id;
                        //    version.VersionNo = 1;

                        //    var res = await _applicationVersionBusiness.Create(version);
                        //    applicationversionId = res.Item.Id;
                        //}
                        if (model.ManpowerTypeCode == "STAFF")
                        {
                            if (newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected)
                            {
                            }
                            else
                            {

                            }

                            if (true)
                            {
                                var marks = model.MarksSelections.Split(",");
                                var tmarks = JsonConvert.DeserializeObject<List<RecCandidateEvaluationViewModel>>(model.EvaluationDataString);
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
                                            //tmarks[i].IsTemplate = false;
                                            tmarks[i].ApplicationId = model.Id;
                                            if (model.IsCandidateEvaluation)
                                            {
                                                //var evalResult = await _candidateEvaluationBusiness.Edit(tmarks[i]);
                                                var evalNoteTempModel = new NoteTemplateViewModel();
                                                evalNoteTempModel.DataAction = DataActionEnum.Edit;
                                                evalNoteTempModel.ActiveUserId = _userContext.UserId;
                                                evalNoteTempModel.TemplateCode = "REC_CANDIDATE_EVALUTION";
                                                evalNoteTempModel.NoteId = tmarks[i].CandidateEvaluationNoteId;
                                                var evalNoteModel = await _noteBusiness.GetNoteDetails(evalNoteTempModel);
                                                evalNoteModel.Json = JsonConvert.SerializeObject(tmarks[i]);
                                                var result1 = await _noteBusiness.ManageNote(evalNoteModel);
                                            }
                                            else
                                            {
                                                tmarks[i].Id = null;
                                                //var evalResult = await _candidateEvaluationBusiness.Create(tmarks[i]);

                                                var evalNoteTempModel = new NoteTemplateViewModel();
                                                evalNoteTempModel.DataAction = DataActionEnum.Create;
                                                evalNoteTempModel.ActiveUserId = _userContext.UserId;
                                                evalNoteTempModel.TemplateCode = "REC_CANDIDATE_EVALUTION";
                                                var evalNoteModel = await _noteBusiness.GetNoteDetails(evalNoteTempModel);
                                                evalNoteModel.Json = JsonConvert.SerializeObject(tmarks[i]);
                                                var result1 = await _noteBusiness.ManageNote(evalNoteModel);
                                            }
                                            //if (model.Mode == "EDIT" && check.Count == 0)
                                            //{
                                            //    var evalversion = _mapper.Map<CandidateEvaluationViewModel, CandidateEvaluationVersionViewModel>(tmarks[i]);
                                            //    evalversion.Id = null;
                                            //    evalversion.ApplicationVersionId = applicationversionId;
                                            //    evalversion.VersionNo = 1;
                                            //    await _candidateEvaluationVersionBusiness.Create(evalversion);
                                            //}
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
                                                //var evalResult = await _candidateEvaluationBusiness.Edit(tmarks[i]);
                                                var evalNoteTempModel = new NoteTemplateViewModel();
                                                evalNoteTempModel.DataAction = DataActionEnum.Edit;
                                                evalNoteTempModel.ActiveUserId = _userContext.UserId;
                                                evalNoteTempModel.TemplateCode = "REC_CANDIDATE_EVALUTION";
                                                evalNoteTempModel.NoteId = tmarks[i].CandidateEvaluationNoteId;
                                                var evalNoteModel = await _noteBusiness.GetNoteDetails(evalNoteTempModel);
                                                evalNoteModel.Json = JsonConvert.SerializeObject(tmarks[i]);
                                                var result2 = await _noteBusiness.ManageNote(evalNoteModel);
                                            }
                                            else
                                            {
                                                tmarks[i].Id = null;
                                                //var evalResult = await _candidateEvaluationBusiness.Create(tmarks[i]);
                                                
                                                var evalNoteTempModel = new NoteTemplateViewModel();
                                                evalNoteTempModel.DataAction = DataActionEnum.Create;
                                                evalNoteTempModel.ActiveUserId = _userContext.UserId;
                                                evalNoteTempModel.TemplateCode = "REC_CANDIDATE_EVALUTION";
                                                var evalNoteModel = await _noteBusiness.GetNoteDetails(evalNoteTempModel);
                                                evalNoteModel.Json = JsonConvert.SerializeObject(tmarks[i]);
                                                var result2 = await _noteBusiness.ManageNote(evalNoteModel);
                                            }
                                            //if (model.Mode == "EDIT" && check.Count == 0)
                                            //{
                                            //    var evalversion = _mapper.Map<CandidateEvaluationViewModel, CandidateEvaluationVersionViewModel>(tmarks[i]);
                                            //    evalversion.Id = null;
                                            //    evalversion.ApplicationVersionId = applicationversionId;
                                            //    evalversion.VersionNo = 1;
                                            //    await _candidateEvaluationVersionBusiness.Create(evalversion);
                                            //}
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
                                                //var evalResult = await _candidateEvaluationBusiness.Edit(tmarks[i]);
                                                var evalNoteTempModel = new NoteTemplateViewModel();
                                                evalNoteTempModel.DataAction = DataActionEnum.Edit;
                                                evalNoteTempModel.ActiveUserId = _userContext.UserId;
                                                evalNoteTempModel.TemplateCode = "REC_CANDIDATE_EVALUTION";
                                                evalNoteTempModel.NoteId = tmarks[i].CandidateEvaluationNoteId;
                                                var evalNoteModel = await _noteBusiness.GetNoteDetails(evalNoteTempModel);
                                                evalNoteModel.Json = JsonConvert.SerializeObject(tmarks[i]);
                                                var result3 = await _noteBusiness.ManageNote(evalNoteModel);
                                            }
                                            else
                                            {
                                                tmarks[i].Id = null;
                                                //var evalResult = await _candidateEvaluationBusiness.Create(tmarks[i]);

                                                var evalNoteTempModel = new NoteTemplateViewModel();
                                                evalNoteTempModel.DataAction = DataActionEnum.Create;
                                                evalNoteTempModel.ActiveUserId = _userContext.UserId;
                                                evalNoteTempModel.TemplateCode = "REC_CANDIDATE_EVALUTION";
                                                var evalNoteModel = await _noteBusiness.GetNoteDetails(evalNoteTempModel);
                                                evalNoteModel.Json = JsonConvert.SerializeObject(tmarks[i]);
                                                var result3 = await _noteBusiness.ManageNote(evalNoteModel);
                                            }
                                            //if (model.Mode == "EDIT" && check.Count == 0)
                                            //{
                                            //    var evalversion = _mapper.Map<CandidateEvaluationViewModel, CandidateEvaluationVersionViewModel>(tmarks[i]);
                                            //    evalversion.Id = null;
                                            //    evalversion.ApplicationVersionId = applicationversionId;
                                            //    evalversion.VersionNo = 1;
                                            //    await _candidateEvaluationVersionBusiness.Create(evalversion);
                                            //}
                                        }
                                        i++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || newModel.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected)
                            {
                            }
                            else
                            {
                            }
                        }
                        ViewBag.Success = true;
                    }
                    else
                    {
                        //ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("_CandidateEvaluationScaleReview", model);

        }

        #endregion
        public IActionResult ManageBulkScheduleInterview(string applicationIds)
        {
            var model = new RecApplicationViewModel();
            model.ApplicationIds = applicationIds;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageBulkScheduleInterview(RecApplicationViewModel model)
        {
            if (model.ApplicationIds.IsNotNullAndNotEmpty())
            {
                var appIds = model.ApplicationIds.Trim(',').Split(',');

                var serTemp = new ServiceTemplateViewModel();
                serTemp.TemplateCode = "SCHEDULE_INTERVIEW";
                serTemp.ActiveUserId = _userContext.UserId;
                serTemp.DataAction = DataActionEnum.Create;

                var sermodel = await _serviceBusiness.GetServiceDetails(serTemp);

                foreach (var appId in appIds)
                {
                    if (appId.IsNotNullAndNotEmpty())
                    {
                        var jsonmodel = new RecApplicationViewModel()
                        {
                            ApplicationId = appId,
                            InterviewDate = model.ScheduleInterveiwDate,
                        };

                        sermodel.Json = JsonConvert.SerializeObject(jsonmodel);
                        sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                        sermodel.DataAction = DataActionEnum.Create;
                        var result = await _serviceBusiness.ManageService(sermodel);
                    }
                }
                return Json(new { success = true });
            }
            return View("ManageBulkScheduleInterview", model);
        }

        public ActionResult ViewHmBatchCandidate(string batchid, string type)
        {
            ViewBag.BatchId = batchid;
            ViewBag.Type = type;
            return View();
        }

        public async Task<JsonResult> ReadWorkerPoolBatchData(string batchid, string type)
        {
            if (type == "WorkerPool")
            {
                var data = await _recruitmentBusiness.GetWorkerPoolBatchData(batchid);                
                return Json(data);
            }
            else
            {
                var data = await _recruitmentBusiness.GetBatchData(batchid);
                return Json(data);
            }
        }
        public IActionResult ViewTaskList(string taskid = null)
        {
            var model = new RecTaskViewModel { Id = taskid };
            return View(model);
        }
        public async Task<IActionResult> ReadTaskDataInProgress()
        {
            var result = await _recruitmentBusiness.GetRecTaskList("TASK_STATUS_INPROGRESS");
            var j = Json(result.OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<IActionResult> ReadTaskDataOverdue()
        {
            var result = await _recruitmentBusiness.GetRecTaskList("TASK_STATUS_OVERDUE");
            var j = Json(result.OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<IActionResult> ReadTaskDataCompleted()
        {
            var result = await _recruitmentBusiness.GetRecTaskList("TASK_STATUS_COMPLETE");
            var j = Json(result.OrderByDescending(x => x.StartDate));
            return j;
        }
        public IActionResult ActiveManpowerRecruitmentSummary(string pageId, string permissions)
        {
            ViewBag.Permissions = permissions;
            return View();
        }

        public async Task<ActionResult> ReadActiveManpowerRequirementSummaryData()
        {
            var res = await _careerPortalBusiness.GetActiveManpowerRecruitmentSummaryData();
            return Json(res);
        }
        public async Task<ActionResult> TaskListPartial(string code, string status = "TASK_STATUS_INPROGRESS", string batch = null, bool disableBulk = false, LayoutModeEnum layoutMode = LayoutModeEnum.Popup)
        {
            TaskIndexPageTemplateViewModel model = new TaskIndexPageTemplateViewModel();
            List<ServiceIndexPageColumnViewModel> columnList = new List<ServiceIndexPageColumnViewModel>();
            var template = await _templateBusiness.GetSingle(x => x.Code == code);
            if (template.IsNotNull()) 
            {
                model.TemplateId = template.Id;
               // var lov = await _lOVBusiness.GetSingle(x=>x.Code== status);
                var taskList = await _taskBusiness.GetList(x => x.TemplateId == template.Id /*&& x.TaskStatusId== lov.Id*/);
                var parentService = taskList.FirstOrDefault();
                if (parentService.IsNotNull()) 
                {
                    var service = await _serviceBusiness.GetSingleById(parentService.ParentServiceId);
                    var serviceTemplate = await _templateBusiness.GetSingle(x => x.Id == service.TemplateId);
                    var ServiceIndexPageTemplate = await _templateBusiness.GetSingle<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate>(x => x.TemplateId == service.TemplateId);
                     columnList = await _templateBusiness.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == ServiceIndexPageTemplate.Id);
                    //var tableMetadata = await _tableMetadataBusiness.GetSingleById(serviceTemplate.UdfTableMetadataId);
                    //columnList = await _tableMetadataBusiness.GetViewColumnByTableName(tableMetadata.Schema, tableMetadata.Name);

                }
               
            }
            model.SelectedRows = columnList;
            return View(model);

        }
        public async Task<IActionResult> PrepareCompetenceMatrix(string appId, string taskStatus, string tempCode, string rs = null)
        {
            var model = new CompetenceMatrixViewModel();

            if (rs.IsNullOrEmpty())
            {
                model.Source = "Hr";
            }
            if (taskStatus.IsNotNullAndNotEmpty())
            {
                model.TaskStatus = taskStatus;
            }
            if (tempCode.IsNotNullAndNotEmpty())
            {
                model.TempCode = tempCode;
            }
            return View("PrepareCompetenceMatrix", model);
        }
        public async Task<IActionResult> ManageCompetenceMatrix(CompetenceMatrixViewModel model)
        {
            if (model.TempCode == "STAFF_JOINING_FORMALITIES")
            {
                if (model.ServiceId.IsNullOrEmpty())
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "STAFF_JOINING_FORMALITIES";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }

                else
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }

            else if (model.TempCode == "WORKER_JOINING_FORMALITIES")
            {
                if (model.ServiceId.IsNullOrEmpty())
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "WORKER_JOINING_FORMALITIES";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
               
        public async Task<ActionResult> BulkAction(string code)
        {
            TaskIndexPageTemplateViewModel model = new TaskIndexPageTemplateViewModel();
            List<ServiceIndexPageColumnViewModel> columnList = new List<ServiceIndexPageColumnViewModel>();
            var template = await _templateBusiness.GetSingle(x => x.Code == code);
            if (template.IsNotNull())
            {
                model.TemplateId = template.Id;
                var taskList = await _taskBusiness.GetList(x => x.TemplateId == template.Id);
                var parentService = taskList.FirstOrDefault();
                if (parentService.IsNotNull())
                {
                    var service = await _serviceBusiness.GetSingleById(parentService.ParentServiceId);
                    var serviceTemplate = await _templateBusiness.GetSingle(x => x.Id == service.TemplateId);
                    var ServiceIndexPageTemplate = await _templateBusiness.GetSingle<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate>(x => x.TemplateId == service.TemplateId);
                    columnList = await _templateBusiness.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == ServiceIndexPageTemplate.Id);
                   

                }

            }
            model.SelectedRows = columnList;
            return View(model);

        }
        public IActionResult CreateBeneFiciary(string referId)
        {
            var model = new ApplicationBeneficiaryViewModel();
            model.DataAction = DataActionEnum.Create;
            model.ReferenceId = referId;
            return View("_ApplicationBeneficiary", model);
        }

        public async Task<IActionResult> EditBeneFiciary(string referId, string noteId)
        {
           
            var model = await _careerPortalBusiness.GetBeneficiaryDataByid(referId, noteId);
            if (model != null)
            {
                model.DataAction = DataActionEnum.Edit;
                model.ReferenceId = referId;
            }
            return View("_ApplicationBeneficiary", model);
        }

        public async Task<IActionResult> DeleteBeneFiciary(string noteId)
        {
            await _careerPortalBusiness.DeleteBeneficiary(noteId);
            return Json(true);
        }

    
        [HttpPost]
        public async Task<IActionResult> ManageBeneficiary(ApplicationBeneficiaryViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "REC_NOMINATION_BENEFITS";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.SequenceOrder = model.SequenceOrder;
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {

                    return Json(new { success = true });
                }
            }

            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.NoteId = model.NoteId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "REC_NOMINATION_BENEFITS";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                 notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.SequenceOrder = model.SequenceOrder;
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });
        }
        public async Task<JsonResult> ReadBeneficiaryData(string referId)
        {
               var model = await _careerPortalBusiness.ReadBeneficiaryData(referId);
               return Json(model);

        }

        public async Task<IActionResult> PrepareJoiningSource(string appId, string tempCode,string taskStatus)
        {
            var model = new ApplicationViewModel();
            model.ApplicationId = appId;
            //if (appId.IsNotNull())
            //{
            //    model = await _recruitmentBusiness.GetApplicationDetailsById(appId);
            //    model.FullName = model.FirstName + ' ' + model.MiddleName + ' ' + model.LastName;

            //}

            if (taskStatus.IsNotNullAndNotEmpty())
            {
                model.TaskStatus = taskStatus;
            }
            if (tempCode.IsNotNullAndNotEmpty())
            {
                model.TempCode = tempCode;
            }

            return View("PrepareJoiningSource", model);
        }

        //[HttpPost]
        //public async Task<IActionResult> ManageJoiningSource(ApplicationViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var appmodel = await _applicationBusiness.GetSingleById(model.Id);
        //        appmodel.Sourcing = model.Sourcing;
        //        appmodel.FirstName = model.FirstName;
        //        appmodel.LastName = model.LastName;
        //        appmodel.MiddleName = model.MiddleName;
        //        appmodel.NationalityId = model.NationalityId;
        //        appmodel.GaecNo = model.GaecNo;
        //        appmodel.JobNo = model.JobNo;
        //        appmodel.ReportingToId = model.ReportingToId;
        //        appmodel.NextOfKin = model.NextOfKin;
        //        appmodel.NextOfKinRelationship = model.NextOfKinRelationship;
        //        appmodel.NextOfKinEmail = model.NextOfKinEmail;
        //        appmodel.NextOfKinPhoneNo = model.NextOfKinPhoneNo;

        //        appmodel.OtherNextOfKin = model.OtherNextOfKin;
        //        appmodel.OtherNextOfKinRelationship = model.OtherNextOfKinRelationship;
        //        appmodel.OtherNextOfKinEmail = model.OtherNextOfKinEmail;
        //        appmodel.OtherNextOfKinPhoneNo = model.OtherNextOfKinPhoneNo;

        //        appmodel.WitnessName1 = model.WitnessName1;
        //        appmodel.WitnessDesignation1 = model.WitnessDesignation1;
        //        appmodel.WitnessGAEC1 = model.WitnessGAEC1;
        //        appmodel.WitnessDate1 = model.WitnessDate1;

        //        appmodel.WitnessName2 = model.WitnessName2;
        //        appmodel.WitnessDesignation2 = model.WitnessDesignation2;
        //        appmodel.WitnessGAEC2 = model.WitnessGAEC2;
        //        appmodel.WitnessDate2 = model.WitnessDate2;
        //        appmodel.DateOfArrival = model.DateOfArrival;
        //        appmodel.JoiningDate = model.JoiningDate;

        //        await _applicationBusiness.Edit(appmodel);
        //    }

        //    return View("JoiningSource", model);
        //}
        public async Task<IActionResult> ManageJoiningReport(ApplicationViewModel model)
        {
            if (model.TempCode == "STAFF_JOINING_FORMALITIES")
            {
                if (model.ServiceId.IsNullOrEmpty())
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "STAFF_JOINING_FORMALITIES";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {

                        return Json(new { success = true, referenceId= result.Item.UdfNoteTableId });

                        var noteTempModel1 = new NoteTemplateViewModel();
                        noteTempModel1.DataAction = DataActionEnum.Edit;
                        noteTempModel1.ActiveUserId = _userContext.UserId;
                        noteTempModel1.TemplateCode = "REC_NOMINATION_BENEFITS";
                        var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        notemodel.ServiceStatusCode = "NOTE_STATUS_INPROGRESS";
                        notemodel.DataAction = DataActionEnum.Create;
                        var jsonmodel = new ApplicationBeneficiaryViewModel()
                        {
                            ReferenceId = result.Item.UdfNoteTableId
                        };
                        var result1 = await _serviceBusiness.ManageService(notemodel);
                        if (result1.IsSuccess)
                        {

                            return Json(new { success = true });
                        }
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }

                else
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true , referenceId = result.Item.UdfNoteTableId });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }

            else if (model.TempCode == "WORKER_JOINING_FORMALITIES")
            {
                if (model.ServiceId.IsNullOrEmpty())
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "WORKER_JOINING_FORMALITIES";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, referenceId = result.Item.UdfNoteTableId });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var noteTempModel = new ServiceTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.ServiceId = model.ServiceId;
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);

                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, referenceId = result.Item.UdfNoteTableId });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> ViewJobAdvCandidate(string applicationId)
        {
            var model = new JobAdvertisementViewModel();
            var appmodel = await _recruitmentBusiness.GetApplicationForJobAdv(applicationId);
            if (appmodel.IsNotNull())
            {

                //model = await _jobAdvertisementBusiness.GetSingle(x=>x.JobId==appmodel.JobId && x.Status == StatusEnum.Active);
                //var result = await _jobAdvertisementBusiness.GetList(x => x.JobId == appmodel.JobId);

                var result = await _recruitmentBusiness.GetJobAdvertisementListByJobId(appmodel.JobId);
                model = result.OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                //model.OrganizationId = appmodel.OrganizationId;
                model.HiringManagerId = appmodel.HiringManagerId;
                model.RecruiterId = appmodel.RecruiterId;

                var manpowertype = await _recruitmentBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                if (manpowertype != null)
                {
                    model.ManpowerType = manpowertype.ManpowerTypeName;
                }
            }

            return View("ViewJobAdvCandidate", model);
        }

        public async Task<IActionResult> IntentToOffer(string appid)
        {
            var model = new RecruitmentPayElementViewModel();
            model.Basic = 0;
            model.ProfessionalAllowance = 0;
            model.UtilityAllowance = 0;
            model.HRA = 0;
            model.FRA = 0;
            model.Transportation = 0;
            model.FurnishingAllowance = 0;
            var appmodel = await _careerPortalBusiness.GetApplicationById(appid);
            model.ApplicantName = appmodel.FirstName + ' ' + appmodel.MiddleName + ' ' + appmodel.LastName;
            if (appmodel.OfferGrade.IsNotNullAndNotEmpty())
            {
                var gradename = await _careerPortalBusiness.GetGrade(appmodel.OfferGrade);
                model.Grade = gradename.Name;
                // var temp = model.Grade.Split("-");
                // model.GradeNumber =Int32.Parse(temp[1]);
            }
            model.SalaryRevision = appmodel.SalaryRevision;
            model.SalaryRevisionAmount = appmodel.SalaryRevisionAmount;
            model.SalaryRevisionComment = appmodel.SalaryRevisionComment;
            model.IsEligibleAfterProbation = appmodel.IsEligibleAfterProbation;
            if (appmodel.OfferDesigination.IsNotNullAndNotEmpty())
            {
                var jobname = await _recruitmentBusiness.GetJobNameById(appmodel.OfferDesigination);
                model.Desigination = jobname.Name;
            }
            else
            {
                var jobname = await _recruitmentBusiness.GetJobNameById(appmodel.JobId);
                model.Desigination = jobname.Name;
            }
            // model.Desigination = appmodel.OfferDesigination;
            model.AnnualLeave = appmodel.AnnualLeave;
            if (appmodel.AccommodationId.IsNotNullAndNotEmpty())
            {
                //var accom = await _recruitmentElementBusiness.GetAccomadationValue(appmodel.AccommodationId);
                var accom = await _lOVBusiness.GetSingleById(appmodel.AccommodationId);
                model.AccommodationName = accom.Name;
            }
            if (appmodel.VisaCategory.IsNotNullAndNotEmpty())
            {
                //var visacategory = await _recruitmentElementBusiness.GetAccomadationValue(appmodel.VisaCategory);
                var visacategory = await _lOVBusiness.GetSingleById(appmodel.VisaCategoryId);
                model.VisaCategoryName = visacategory.Name;
            }
            var list = await _recruitmentBusiness.GetElementData(appid);
            foreach (var a in list)
            {
                if (a.ElementName == "Basic")
                {
                    if (a.Value != null)
                    {
                        model.Basic = a.Value;
                    }
                }
                else if (a.ElementName == "Bonus")
                {
                    model.Bonus = a.Value;
                }
                else if (a.ElementName == "Mobile Allowance")
                {
                    model.MobileAllowance = a.Value;
                }
                else if (a.ElementName == "Transportation")
                {
                    if (a.Value != null)
                    {
                        model.Transportation = a.Value;
                    }
                    if (a.Comment != null)
                    {
                        model.TransportationText = a.Comment;
                    }
                }

                else if (a.ElementName == "Furnishing Allowance")
                {
                    if (a.Value != null)
                    {
                        model.FurnishingAllowance = a.Value;
                    }

                }

                else if (a.ElementName == "Food")
                {
                    model.Food = a.Value;
                }
                else if (a.ElementName == "Professional Allowance")
                {
                    if (a.Value != null)
                    {
                        model.ProfessionalAllowance = a.Value;
                    }
                }
                else if (a.ElementName == "Utility Allowance")
                {
                    if (a.Value != null)
                    {
                        model.UtilityAllowance = a.Value;
                    }

                }
                else if (a.ElementName == "HRA")
                {
                    if (a.Value != null)
                    {
                        model.HRA = a.Value;
                    }

                }
                else if (a.ElementName == "FRA")
                {
                    if (a.Value != null)
                    {
                        model.FRA = a.Value;
                    }

                }
                else if (a.ElementName == "Laundry")
                {
                    model.Laundry = a.Value;
                }
            }
            if (model.Grade != null && Int32.Parse(model.Grade) <= 16 && model.AccommodationName != null && model.AccommodationName == "Own Accommodation")
            {
                model.Total = model.Basic + model.ProfessionalAllowance + model.FRA + model.Transportation;
            }
            else if (model.Grade != null && Int32.Parse(model.Grade) <= 16 && model.AccommodationName != null && model.AccommodationName == "Company Provided Accommodation")
            {
                model.Total = model.Basic + model.ProfessionalAllowance;
            }
            else
            {
                model.Total = model.Basic + model.ProfessionalAllowance + model.HRA + model.UtilityAllowance;
            }

            //model.ApplicationId = appid;
            return View(model);
        }

        public IActionResult PrepareVacancyList(string serviceId,string taskServiceId,string type,bool isIframe=false)
        {
            if (type == "Service")
            {
                ViewBag.ServiceId = serviceId;
                ViewBag.IsService = true;
            }
            else
            {
                ViewBag.ServiceId = taskServiceId;
                ViewBag.IsService = false;
            }
            if (isIframe)
            {
                ViewBag.Layout = "/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            
            return View();
        }

        public async Task<ActionResult> ReadSelectedJobadvtData(string vacancyId)
        {
            var model = await _recruitmentBusiness.GetSelectedJobAdvertisement(vacancyId);            

            return Json(model);
        }

        public IActionResult JobAdvertisementList(string vacancyId)
        {
            ViewBag.VacancyListId = vacancyId;
            return View();
        }

        public async Task<ActionResult> GetJobAdvList()
        {
            var model = await _recruitmentBusiness.GetJobAdvList();

            return Json(model);
        }
        public IActionResult JobApplicationList()
        {           
            return View();
        }
        public IActionResult CRPFApplicationDetails()
        {
            return View();
        }
        public async Task<ActionResult> GetJobApplicationList()
        {
            var model = await _recruitmentBusiness.GetJobApplicationList();

            return Json(model);
        }

        public async Task<ActionResult> AddJobAdvertisements(string vacancyId, string jobAdvIds)
        {
            string[] jaId = jobAdvIds.Trim(',').Split(",").ToArray();

            foreach(var id in jaId)
            {
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("VacancyListId", vacancyId);
                ((IDictionary<String, Object>)exo).Add("JobAdvertisementId", id);

                var Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                await _cmsBusiness.CreateForm(Json, "", "CRPF_SELECTED_JOB_ADVERTISEMENT");
            }

            return Json(new { success = true });
        }

    }
}

