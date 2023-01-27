using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Api.Controllers;
using System.Data;
using System.IO;
using Synergy.App.Api.Areas.PJM.Models;

namespace Synergy.App.Api.Areas.PJM.Controllers
{
    [Route("pjm/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IProjectManagementBusiness _projectManagementBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly IProjectManagementBusiness _pmtBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INtsTaskTimeEntryBusiness _ntsTaskTimeEntryBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IProjectEmailSetupBusiness _projectEmailBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IProjectEmailSetupBusiness _projectEmailSetupBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IPerformanceManagementBusiness _performanceManagementBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;


        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IPerformanceManagementBusiness performanceManagementBusiness, IProjectManagementBusiness projectManagementBusiness, IProjectManagementBusiness pmtBusiness, ITaskBusiness taskBusiness, INoteBusiness noteBusiness,
          IEmailBusiness emailBusiness, INtsTaskTimeEntryBusiness ntsTaskTimeEntryBusiness, IProjectEmailSetupBusiness projectEmailBusiness, IHRCoreBusiness hrCoreBusiness, IProjectEmailSetupBusiness projectEmailSetupBusiness,
          ITemplateBusiness templateBusiness,IServiceBusiness serviceBusiness, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _projectManagementBusiness = projectManagementBusiness;
            _pmtBusiness = pmtBusiness;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _serviceBusiness= serviceBusiness;
            _ntsTaskTimeEntryBusiness = ntsTaskTimeEntryBusiness;
            _emailBusiness = emailBusiness;
            _projectEmailBusiness = projectEmailBusiness;
            _hrCoreBusiness = hrCoreBusiness;
            _projectEmailSetupBusiness = projectEmailSetupBusiness;
            _templateBusiness = templateBusiness;
            _performanceManagementBusiness = performanceManagementBusiness;
        }

        #region Project Board
        [HttpGet]
        [Route("ReadProjectData")]
        public async Task<IActionResult> ReadProjectData(string userId,string mode)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            if (mode == "Shared")
            {
                var list = await _projectManagementBusiness.GetProjectSharedData();
                return Ok(list);
            }
            else
            {
                var list = await _projectManagementBusiness.GetProjectData();
                return Ok(list);
            }

        }

        #endregion Project Board

        #region Dashboard
        [HttpGet]
        [Route("GetProjectTaskChartByStatus")]
        public async Task<IActionResult> GetProjectTaskChartByStatus(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var viewModel = await _pmtBusiness.GetTaskStatus(projectId);
            return Ok(viewModel);
        }
        [HttpGet]
        [Route("GetProjectTaskChartByType")]
        public async Task<IActionResult> GetProjectTaskChartByType(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var viewModel = await _pmtBusiness.GetTaskType(projectId);
            return Ok(viewModel);
        }
        [HttpGet]
        [Route("GetProjectStageChart")]
        public async Task<IActionResult> GetProjectStageChart(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var viewModel = await _pmtBusiness.ReadProjectStageChartData(_userContext.UserId, projectId);
            return Ok(viewModel);
        }

        [HttpGet]
        [Route("GetProjectUserIdNameList")]
        public async Task<IActionResult> GetProjectUserIdNameList(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var viewModel = await _pmtBusiness.GetProjectUserIdNameList(projectId);
            return Ok(viewModel);
        }

        [HttpGet]
        [Route("GetProjectStageIdNameList")]
        public async Task<IActionResult> GetProjectStageIdNameList(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var viewModel = await _pmtBusiness.GetProjectStageIdNameList(_userContext.UserId, projectId);
            return Ok(viewModel);
        }

        [HttpGet]
        [Route("ReadProjectTaskGridViewData")]
        public async Task<IActionResult> ReadProjectTaskGridViewData(string userId, string projectId, string projectIds = null, string senderIds = null, string recieverids = null, string statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Today)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(1);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextWeek)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(8);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextMonth)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(31);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Between)
            {
                toDate = toDate.Value.AddDays(1);
            }
            var list = await _pmtBusiness.ReadProjectTaskGridViewData(_userContext.UserId, projectId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            var j = Ok(list);
            return j;
        }



        #endregion Dashboard

        #region Project Book

        [HttpGet]
        [Route("LoadBook")]
        public async Task<IActionResult> LoadBook(string userId, string statusFilter, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var data = await _taskBusiness.LoadWorkBooks(_userContext.UserId, statusFilter);
            var jh = (from r in data where r.ParentId == null select r);
            List<LoadBookModel> getParent = (from d in data
                                             where d.ParentId == null
                                             select new LoadBookModel()
                                             {
                                                 Id = d.Id,
                                                 NoteNo = d.NoteNo,
                                                 NoteSubject = d.NoteSubject,
                                                 NoteDescription = d.NoteDescription,
                                                 NoteStatusCode = d.NoteStatusCode,
                                                 ChildNotesCount=(from e in data where e.ParentId==d.Id select e). ToList().Count()
                                             }).ToList();

            getParent.ToList().ForEach(n =>
            {
                if (n.ChildNotesCount > 0)
                {
                    n.ChildNotes = (from d in data
                                where d.ParentId == n.Id
                                select new LoadBookModel()
                                {
                                    Id = d.Id,
                                    NoteNo = d.NoteNo,
                                    NoteSubject = d.NoteSubject,
                                    NoteDescription = d.NoteDescription,
                                    NoteStatusCode = d.NoteStatusCode,
                                    ChildNotesCount = (from e in data where e.ParentId == d.Id select e).ToList().Count()
                                }).ToList();
                    MapParentChildBook(n.ChildNotes, data);

                }
            });
            return Ok(getParent);
        }

        private List<LoadBookModel> MapParentChildBook(List<LoadBookModel> chartModels, List<NoteViewModel> childList)
        {
            chartModels.ToList().ForEach(n =>
            {
            if (n.ChildNotesCount > 0)
            {
                n.ChildNotes = (from d in childList
                                where d.ParentId == n.Id
                                select new LoadBookModel()
                                {
                                    Id = d.Id,
                                    NoteNo = d.NoteNo,
                                    NoteSubject = d.NoteSubject,
                                    NoteDescription = d.NoteDescription,
                                    NoteStatusCode = d.NoteStatusCode,
                                    ChildNotesCount = (from e in childList where e.ParentId == d.Id select e).ToList().Count()
                                }).ToList();
                    MapParentChildBook(n.ChildNotes, childList);

                }
            });


            return chartModels;
        }

        [HttpGet]
        [Route("WorkBookCount")]
        public async Task<IActionResult> WorkBookCount(string userId, string bookId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            WorklistDashboardSummaryViewModel model = new WorklistDashboardSummaryViewModel();
            var NoteCount = await _noteBusiness.NotesCountForDashboard(_userContext.UserId, bookId);
            if (NoteCount != null)
            {
                model.N_CreatedByMeActive = NoteCount.createdByMeActive;
                model.N_CreatedByMeDraft = NoteCount.createdByMeDraft;
                model.N_CreatedByMeExpired = NoteCount.createdByMeExpired;

            }
            var TaskCount = await _taskBusiness.TaskCountForDashboard(_userContext.UserId, bookId);
            if (TaskCount != null)
            {
                model.T_AssignPending = TaskCount.T_AssignPending;
                model.T_AssignCompleted = TaskCount.T_AssignCompleted;
                model.T_AssignOverdue = TaskCount.T_AssignOverdue;
                model.T_AssignReject = TaskCount.T_AssignReject;
            }
            var notification = await _taskBusiness.NotificationDashboardIndex(_userContext.UserId, bookId);
            if (notification != null)
            {
                model.ReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.Read).Count();
                model.UnReadCount = notification.Where(x => x.ReadStatus == ReadStatusEnum.NotRead).Count();
                model.ArchivedCount = notification.Where(x => x.IsArchived == true).Count();
            }

            return Ok(model);
        }

        #endregion Project Book

        #region Project Task View


        private bool IsProjectManager(IUserContext _userContext)
        {
            var isProjectManager = false;

            var userRole = _userContext.UserRoleCodes.IsNotNull() ? _userContext.UserRoleCodes.Contains("PROJECT_MANAGER") : false;
            if (userRole)
            {
                isProjectManager = true;
            }
            return isProjectManager;
        }


        private List<TeamWorkloadViewModel> GetSequence(string parentId, List<TeamWorkloadViewModel> oldList, List<TeamWorkloadViewModel> newList, long Sequence)
        {
            foreach (var data in oldList.Where(x => x.parentId == parentId))
            {
                //if (data.parentId == parentId)
                //{
                data.Sequence = Sequence + 1;
                newList.Add(data);
                GetSequence(data.id, oldList, newList, data.Sequence);
                //}
            }
            return newList;
        }

        [HttpGet]
        [Route("GetProjectsList")]
        public async Task<IActionResult> GetProjectsList(string userId,string portalName)
        {
            await Authenticate(userId,portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager(_userContext);

            var projectList = await _projectManagementBusiness.GetProjectsList(_userContext.UserId, isProjectManager);
            var j = Ok(projectList);
            return j;
        }

        [HttpGet]
        [Route("ReadProjectTaskAssignedData")]
        public async Task<IActionResult> ReadProjectTaskAssignedData(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var isProjectManager = IsProjectManager(_userContext);
            if (isProjectManager)
            {
                var list = await _projectManagementBusiness.ReadManagerProjectTaskAssignedData(projectId);

                var j = Ok(list);
                return j;
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectTaskAssignedData(projectId);

                var j = Ok(list);
                return j;
            }

        }

        [HttpGet]
        [Route("ReadProjectTaskOwnerData")]
        public async Task<IActionResult> ReadProjectTaskOwnerData(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var isProjectManager = IsProjectManager(_userContext);
            if (isProjectManager)
            {
                var list = await _projectManagementBusiness.ReadManagerProjectTaskOwnerData(projectId);

                var j = Ok(list);
                return j;
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectTaskOwnerData(projectId);

                var j = Ok(list);
                return j;
            }

        }

        [HttpGet]
        [Route("ReadProjectTaskData")]
        public async Task<IActionResult> ReadProjectTaskData(string userId, string projectId, string stage, string filteruser = null, string filterstatus = null, string ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            IList<TeamWorkloadViewModel> model = new List<TeamWorkloadViewModel>();
            var isProjectManager = IsProjectManager(_userContext);
            if (isProjectManager || stage == "Show")
            {
                var list = await _projectManagementBusiness.ReadManagerProjectStageViewData(projectId);
                foreach (var data in list)
                {
                    if (data.parentId.IsNullOrEmpty())
                    {
                        data.Sequence = 1;
                        model.Add(data);
                        model = GetSequence(data.id, list.ToList(), model.ToList(), data.Sequence);
                        break;
                    }


                }
                return Ok(model);
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectStageViewData(projectId);
                foreach (var data in list)
                {
                    if (data.parentId.IsNullOrEmpty())
                    {
                        data.Sequence = 1;
                        model.Add(data);
                        model = GetSequence(data.id, list.ToList(), model.ToList(), data.Sequence);
                        break;
                    }


                }
                return Ok(model);
            }
        }

        [HttpGet]
        [Route("ReadSubTaskViewData")]
        public async Task<IActionResult> ReadSubTaskViewData(string userId, string taskId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var list = await _projectManagementBusiness.ReadProjectSubTaskViewData(taskId);
            var j = Ok(list);
            return j;
        }

        //Same function is created above which can be used for project task by date

        //public async Task<IActionResult> ReadProjectTaskGridViewData(string projectId, string projectIds = null, string senderIds = null, string recieverids = null, string statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        //{
        //    if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Today)
        //    {
        //        fromDate = System.DateTime.Now;
        //        toDate = fromDate.Value.AddDays(1);
        //    }
        //    else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextWeek)
        //    {
        //        fromDate = System.DateTime.Now;
        //        toDate = fromDate.Value.AddDays(8);
        //    }
        //    else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextMonth)
        //    {
        //        fromDate = System.DateTime.Now;
        //        toDate = fromDate.Value.AddDays(31);
        //    }
        //    else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Between)
        //    {
        //        toDate = toDate.Value.AddDays(1);
        //    }

        //    var isProjectManager = IsProjectManager();
        //    var userId = _userContext.UserId;
        //    var list = await _projectManagementBusiness.ReadProjectTask(userId, projectId, isProjectManager, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
        //    var j = Ok(list);
        //    return j;
        //}

        #endregion Project Task View

        #region Project User Workload

        [HttpGet]
        [Route("ReadProjectUserWorkloadGridViewData")]
        public async Task<IActionResult> ReadProjectUserWorkloadGridViewData(string userId, string projectIds = null, string senderIds = null, string recieverids = null, string statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            {
                if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Today)
                {
                    fromDate = System.DateTime.Now;
                    toDate = fromDate.Value.AddDays(1);
                }
                else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextWeek)
                {
                    fromDate = System.DateTime.Now;
                    toDate = fromDate.Value.AddDays(8);
                }
                else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextMonth)
                {
                    fromDate = System.DateTime.Now;
                    toDate = fromDate.Value.AddDays(31);
                }
                else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Between)
                {
                    toDate = toDate.Value.AddDays(1);
                }
                var list = await _projectManagementBusiness.ReadProjectUserWorkloadGridViewData(_userContext.UserId, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
                var j = Ok(list);
                //var j = Json(list.ToDataSourceResult(request));
                return j;
            }
        }
            #endregion User Workload

            #region Time Entries

            [HttpGet]
        [Route("ReadTaskWorkTimeDetails")]
        public async Task<IActionResult> ReadTaskWorkTimeDetails(string userId, string serviceId, string TimeLogDate)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            DateTime today = DateTime.Now;
            if (TimeLogDate.IsNotNullAndNotEmpty())
            {
                today = Convert.ToDateTime(TimeLogDate);
            }
            var list = await _ntsTaskTimeEntryBusiness.GetTimeEntriesData(serviceId, today);


            return Ok(list);
        }
        #endregion

        #region Project Planning View

        [HttpGet]
        [Route("ReadProjectsTaskForPlanning")]
        public async Task<IActionResult> ReadProjectsTaskForPlanning(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _pmtBusiness.ReadMindMapData(projectId);
            //result = result.Where(x => x.Type == "TASK" || x.Type == "SUBTASK").ToList();
            var model = result.Select(x => new ProjectTaskViewModel { Name = x.Title, Id = x.Id, ParentId = x.ParentId, UserName = x.OwnerName, ServiceId = x.ServiceId, StartDate = x.Start, DueDate = x.End, Type = x.Type }).ToList();
            var j = Ok(model);
            return j;
        }

        [HttpGet]
        [Route("ReadProjectPlanningTaskData")]
        public async Task<IActionResult> ReadProjectPlanningTaskData(string userId, string projectId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _pmtBusiness.ReadMindMapData(projectId);
            result = result.Where(x => x.Type == "TASK" || x.Type == "SUBTASK").ToList();

            var events = new List<ProjectTimelineViewModel>();
            foreach (var i in result)
            {
                events.Add(new ProjectTimelineViewModel()
                {
                    id = i.Id,
                    title = i.Title,
                    start = i.Start.ToString("yyyy-MM-dd"), //.ToString("yyyy-MM-dd"),//"2020-04-18"
                    end = i.End.ToString("yyyy-MM-dd"), // .ToString("yyyy-MM-dd"), //"2020-04-18"
                    allDay = false
                });
            }
            return Ok(events.ToArray());
        }
        #endregion

        #region Inbox

        [HttpGet]
        [Route("GetInboxTreeviewList")]
        public async Task<IActionResult> GetInboxTreeviewList(string userId, string id, string type, string parentId, string userRoleId, string stageName, string stageId, string batchId, string expandingList)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PROJECT_MANAGER"))
            {
                var result1 = await _pmtBusiness.GetInboxMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
                // var result1 = await _pmtBusiness.GetInboxMenuItem(id, type, parentId, userRoleId, _userContext.UserId, "bf9cabaa-4928-41c1-8b8d-0e949e163075", stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
                var model1 = result1.ToList();
                return Ok(model1);
            }
            else if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PROJECT_USER"))
            {
                var result = await _pmtBusiness.GetInboxMenuItemByUser(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
                var model = result.ToList();
                return Ok(model);
            }
            return Ok(new List<TASTreeViewViewModel>());
        }

        #endregion Indox



        #region Email Task List

        [HttpGet]
        [Route("GetEmailInboxTreeviewList")]
        public async Task<IActionResult> GetEmailInboxTreeviewList(string userId, string id, string config, string projectid)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var vm = new TreeViewViewModel();
            if (config == "project")
            {

                //  if(projectid.IsNotNullAndNotEmpty())
                // {
                var model = await _emailBusiness.GetInboxMenuItemProject(id, projectid);
                return Ok(model);
                // }
                //  return Ok(vm);

            }
            else if (config == "company")
            {
                var model = await _emailBusiness.GetInboxMenuItemCompany(id);
                return Ok(model);
            }
            else
            {
                var model = await _emailBusiness.GetInboxMenuItem(id);
                return Ok(model);
            }


        }

        [HttpGet]
        [Route("ReadEmailTasks")]
        public async Task<IActionResult> ReadEmailTasks(string userId, int PageSize, int Page, string id, string search)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            if (search.IsNotNullAndNotEmpty())
            {
                var result = await _emailBusiness.SearchEmailInbox(search);
                return Ok(result);
            }
            else
            {
                var Skip = PageSize * (Page - 1);
                var Take = PageSize;
                if (id.IsNullOrEmptyOrWhiteSpace())
                {
                    id = "INBOX";
                }
                var result = await _emailBusiness.ReceiveEmailInbox(id, Skip, Take);

                var result1 = new EmailTaskModel()
                {
                    Data = result,
                    Total = result.Count == 0 ? 0 : result.FirstOrDefault().Total,
                };
                return Ok(result1);
            }
        }


        [HttpGet]
        [Route("SaveEmailToNtsType")]
        public async Task<IActionResult> SaveEmailToNtsType(string NtsType, string Id, string templateCode, string prms,string portalName,string userId)
        {
            await Authenticate(userId,portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var result = await _emailBusiness.ReceiveEmailById(Id);
            var TypeId = "";
            var TemplateCode = "";
            prms = prms.Replace("&amp;", "&");

            if (NtsType == "Note")
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.AttachmentIds = result.AttachmentIds;

                noteTempModel.Prms = Helper.QueryStringToDictionary(prms);

                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = templateCode;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                notemodel.NoteDescription = result.Body;
                notemodel.NoteSubject = result.Subject;

                var result1 = await _noteBusiness.ManageNote(notemodel);
                if (result1.IsSuccess)
                {

                    TypeId = result1.Item.NoteId;
                }
            }
            else if (NtsType == "Service")
            {
                ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                serviceModel.ActiveUserId = _userContext.UserId;
                serviceModel.TemplateCode = templateCode;
                serviceModel.Prms = Helper.QueryStringToDictionary(prms);

                var service = await _serviceBusiness.GetServiceDetails(serviceModel);
                service.LastUpdatedBy = _userContext.UserId;
                service.DataAction = DataActionEnum.Create;
                service.ServiceStatusCode = "SERVICE_STATUS_DRAFT";

                //service.Json = JsonConvert.SerializeObject(assResultModel);

                var res = await _serviceBusiness.ManageService(service);
                if (res.IsSuccess)
                {
                    TypeId = res.Item.ServiceId;
                }
            }
            else if (NtsType == "Task")
            {
                var taskTempModel = new TaskTemplateViewModel();

                taskTempModel.TemplateCode = templateCode;
                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                // for line manager line.ManagerUserId
                //stepmodel.AssignedToUserId = line.ManagerUserId;
                stepmodel.OwnerUserId = _userContext.UserId;
                stepmodel.StartDate = DateTime.Now.Date;
                stepmodel.DueDate = DateTime.Now.AddDays(10);
                stepmodel.DataAction = DataActionEnum.Create;

                stepmodel.TaskStatusCode = "TASK_STATUS_DRAFT";
                //stepmodel.Json = "{}";
                //   dynamic exo = new System.Dynamic.ExpandoObject();

                //((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);

                //                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var resss = await _taskBusiness.ManageTask(stepmodel);
                if (resss.IsSuccess)
                {
                    TypeId = resss.Item.Id;
                }
            }
            return Ok(new { success = true, Id = TypeId, TemplateCode = templateCode, Attachmentids = result.AttachmentIds });
        }

        [HttpGet]
        [Route("ReadCompanyEmail")]
        public async Task<IActionResult> ReadCompanyEmail(string userId, string id)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                id = "INBOX";
            }
            var result = await _emailBusiness.ReceiveEmailCompanyInbox(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadProjectEmail")]
        public async Task<IActionResult> ReadProjectEmail(string userId, string id, string projectid)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = new List<MessageEmailViewModel>();
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                id = "INBOX";
            }
            if (projectid.IsNotNullAndNotEmpty())
            {
                result = await _emailBusiness.ReceiveEmailProjectInbox(id, projectid);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadProjectEmailSetups")]
        public async Task<IActionResult> ReadProjectEmailSetups(string userId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var projects = await _projectManagementBusiness.GetProjectsList(_userContext.UserId, true);
            var result = new List<IdNameViewModel>();
            foreach (var ids in projects)
            {
                var emailsetup = await _projectEmailBusiness.GetSingle(x => x.ServiceId == ids.Id);
                if (emailsetup.IsNotNull())
                {
                    result.Add(new IdNameViewModel()
                    {
                        Id = emailsetup.ServiceId,
                        Name = ids.Name
                    });
                }

            }
            return Ok(result);
        }


        #endregion

        #region Project User Hierarchy

        [HttpGet]
        [Route("GetChildList")]
        public async Task<IActionResult> GetChildList(string userId, string parentId, int levelUpto, string hierarchyId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var childList = await _hrCoreBusiness.GetUserHierarchy(parentId, levelUpto, hierarchyId);
            var json = Ok(childList);
            return json;

        }

        #endregion Project User hierarchy


        #region Projrect Calendar View

        [HttpGet]
        [Route("ReadProjectTaskCalendarData")]
        public async Task<IActionResult> ReadProjectTaskCalendarData(string userId, string projectId, string filterusers = null, string filterstatus = null, string ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //List<string> projectId = projectIds.Split(',').ToList<string>();
            List<string> filteruser = filterusers.Split(',').ToList<string>();
            List<string> filterstatuss = filterstatus.Split(',').ToList<string>();
            List<string> ownerId = ownerIds.Split(',').ToList<string>();
            if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Today)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(1);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextWeek)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(8);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.NextMonth)
            {
                fromDate = System.DateTime.Now;
                toDate = fromDate.Value.AddDays(31);
            }
            else if (dateRange.IsNotNull() && dateRange == DateTypeEnum.Between)
            {
                toDate = toDate.Value.AddDays(1);
            }
            var isProjectManager = IsProjectManager(_userContext);
            if (isProjectManager)
            {
                var list = await _projectManagementBusiness.ReadManagerProjectCalendarViewData(projectId, filteruser, filterstatuss, ownerId, column, fromDate, toDate);
                return Ok(list);
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectCalendarViewData(projectId, filteruser, filterstatuss, ownerId, column, fromDate, toDate);
                return Ok(list);
            }


        }

        #endregion calendar view

        #region Project Email Setup

        [HttpGet]
        [Route("ReadEmailData")]
        public async Task<IActionResult> ReadEmailData(string userId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var list = await _projectEmailSetupBusiness.GetEmailSetupList();
            return Ok(list);
        }

        #endregion


        #region Task Assigned to me

        [HttpGet]
        [Route("GetTaskTemplateList")]
        public async Task<IActionResult> GetTaskTemplateList(string userId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var templateList = await _templateBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task);
            var t = Ok(templateList);
            return t;
        }

        [HttpGet]
        [Route("GetTemplateDetailsById")]
        public async Task<IActionResult> GetTemplateDetailsById(string userId, string Id)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var template = await _templateBusiness.GetSingleById(Id);

            return Ok(template);
        }

        [HttpGet]
        [Route("GetTaskChartByStatus")]
        public async Task<IActionResult> GetTaskChartByStatus(string userId, string templateId)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskStatusByTemplate(templateId, userId);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetTaskChartByOwner")]
        public async Task<IActionResult> GetTaskChartByOwner(string userId, string templateId)
        {

            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskByUsers(templateId, userId);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetSLAChart")]
        public async Task<IActionResult> GetSLAChart(string userId, string templateId, DateTime fromDate, DateTime toDate)
        {

            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetSLADetails(templateId, userId, fromDate, toDate);
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadTaskGridViewData")]
        public async Task<IActionResult> ReadTaskGridViewData(string userId, string templateId, string statusIds = null, string senderIds = null)
        {

            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var list = await _pmtBusiness.ReadTaskGridViewData(templateId, _userContext.UserId, statusIds, senderIds);
            var j = Ok(list);
            return j;
        }

        [HttpGet]
        [Route("GetTaskOwnerUserIdNameList")]
        public async Task<IActionResult> GetTaskOwnerUserIdNameList(string userId, string templateId)
        {

            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskOwnerUsersList(templateId, userId);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetTaskUserIdNameList")]
        public async Task<IActionResult> GetTaskUserIdNameList(string userId, string templateId)
        {

            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //var userId = _userContext.UserId;
            var model = await _pmtBusiness.GetTaskUsersList(templateId);
            return Ok(model);
        }
        #endregion

        //#region Task By Status

       

        //[HttpGet]
        //[Route("ReadProjectTaskAssignedData")]
        //public async Task<IActionResult> ReadProjectTaskAssignedData(string userId, string projectId, string type)
        //{
        //    await Authenticate(userId);
        //    var _userContext = _serviceProvider.GetService<IUserContext>();
        //    var isProjectManager = IsProjectManager(_userContext);
        //    var templatecode = "";
        //    if (type == "Competency")
        //    {
        //        templatecode = "PMS_COMPENTENCY";
        //    }
        //    else if (type == "Development")
        //    {
        //        templatecode = "PMS_DEVELOPMENT";
        //    }
        //    else
        //    {
        //        templatecode = "PMS_GOAL";
        //    }

        //    var list = await _performanceManagementBusiness.ReadManagerProjectTaskAssignedData(projectId, templatecode);
        //    var j = Ok(list);
        //    return j;


        //}

        //[HttpGet]
        //[Route("ReadProjectTaskOwnerData")]
        //public async Task<IActionResult> ReadProjectTaskOwnerData(string userId, string projectId, string type)
        //{
        //    await Authenticate(userId);
        //    var _userContext = _serviceProvider.GetService<IUserContext>();
        //    var isProjectManager = IsProjectManager(_userContext);
        //    var templatecode = "";
        //    if (type == "Competency")
        //    {
        //        templatecode = "PMS_COMPENTENCY";
        //    }
        //    else if (type == "Development")
        //    {
        //        templatecode = "PMS_DEVELOPMENT";
        //    }
        //    else
        //    {
        //        templatecode = "PMS_GOAL";
        //    }

        //    var list = await _performanceManagementBusiness.ReadManagerProjectTaskOwnerData(projectId, templatecode);
        //    var j = Ok(list);
        //    return j;


        //}
        ////ReadProjectTaskData
        ////ReadSubTaskViewData

        //#endregion
    }

}



