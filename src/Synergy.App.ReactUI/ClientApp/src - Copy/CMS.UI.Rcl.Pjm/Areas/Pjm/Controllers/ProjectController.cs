using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectController : ApplicationController
    {

        private readonly IProjectManagementBusiness _pmtBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserContext _userContext;
        private readonly IPushNotificationBusiness _notificationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;
        public ProjectController(IEmailBusiness emailBusiness,IPushNotificationBusiness notificationBusiness, IProjectManagementBusiness pmtBusiness, 
            IUserContext userContext, IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
            ITaskBusiness taskBusiness
            , INoteBusiness noteBusiness)
        {
            _pmtBusiness = pmtBusiness;
            _userContext = userContext;
            _notificationBusiness = notificationBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _emailBusiness = emailBusiness;
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Inbox()
        {
            var model = new ApplicationSearchViewModel();
            return View(model);
        }
        public ActionResult WbsTimeLine(string projectId)
        {
            var model = new ProgramDashboardViewModel { Id = projectId };
            return View(model);

        }
        public async Task<ActionResult> ProjectDashboard(string projectId)
        {
            var projectlist = await _pmtBusiness.GetProjectSharedList(_userContext.UserId);
            if (projectId.IsNullOrEmpty() && projectlist != null && projectlist.Count > 0)
            {
                projectId = projectlist.FirstOrDefault().Id;
            }
            var model = await _pmtBusiness.GetProjectDashboardDetails(projectId);
            if (model != null)
            {
                model.ProjectList = projectlist.ToList();
            }
            else
            {
                model = new ProjectDashboardViewModel();
            }
            return View(model);
        }
        public async Task<ActionResult> GetProjectTaskChartByStatus(string projectId)
        {
            var viewModel = await _pmtBusiness.GetTaskStatus(projectId);
            //var viewModel = new List<ProjectDashboardChartViewModel>();
            //viewModel.Add(new ProjectDashboardChartViewModel {Value=5,Type="InProgress",Code="InProgress" });
            return Json(viewModel);
        }
        public async Task<ActionResult> GetProjectTaskChartByType(string projectId)
        {
            var viewModel = await _pmtBusiness.GetTaskType(projectId);
            //var viewModel = new List<ProjectDashboardChartViewModel>();
            //viewModel.Add(new ProjectDashboardChartViewModel {Value=5,Type="Noor",Code="Noor" });
            //viewModel.Add(new ProjectDashboardChartViewModel {Value=10,Type="Jameel",Code="Jameel" });
            return Json(viewModel);
        }
        public async Task<ActionResult> GetProjectUserIdNameList(string projectId)
        {
            var viewModel = await _pmtBusiness.GetProjectUserIdNameList(projectId);
            return Json(viewModel);
        }
        public async Task<ActionResult> GetProjectStageChart(string projectId)
        {
            var viewModel = await _pmtBusiness.ReadProjectStageChartData(_userContext.UserId,projectId);
            //var viewModel = new List<ProjectDashboardChartViewModel>();
            //viewModel.Add(new ProjectDashboardChartViewModel { Value = 5, Type = "Test Stage 01", Code = "TestStage01" });
            //viewModel.Add(new ProjectDashboardChartViewModel { Value = 10, Type = "Test Stage 02", Code = "TestStage02" });
            return Json(viewModel);
        }
        public async Task<ActionResult> GetProjectStageIdNameList(string projectId)
        {
            var viewModel = await _pmtBusiness.GetProjectStageIdNameList(_userContext.UserId, projectId);
            return Json(viewModel);
        } 
        public async Task<ActionResult> GetProjectIdNameList()
        {
            var projectlist = await _pmtBusiness.GetProjectSharedList(_userContext.UserId);
            return Json(projectlist);
        }

        public ActionResult GetProjectTimeLog(long projectId)
        {
            //var viewModel = _pmtBusiness.GetTimeLog(projectId);
            var viewModel = new ProjectDashboardChartViewModel();
            return Json(viewModel);
        }
        public async Task<ActionResult> ProjectWorkBreakDownStructure(string projectId)
        {
            ViewBag.ProjectId = projectId;
            var model = new ProgramDashboardViewModel { Id = projectId };
            return View(model);
        }
        public ActionResult GetProjectAttachments([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            //var list = _pmtBusiness.ReadTaskFileData(projectId);
            var list = new List<FileViewModel>();
            return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetProjectNotificationList(string projectId)
        {
            //var result = _notificationBusiness.GetProjectNotificationList(LoggedInUserId, projectId);
            //var result = new List<NotificationViewModel>();
            var userId = _userContext.UserId;
            var result = await _notificationBusiness.GetServiceNotificationList(projectId, userId, 0);
            var j = Json(result);
            return j;
        }
        public ActionResult GetProjectTeamData([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            //var list = _pmtBusiness.ReadProjectTeamData(projectId);
            var list = new List<TeamTaskDashboardViewModel>();
            return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetProjectOverDueTask([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            var list = await _pmtBusiness.ReadTaskOverdueData(projectId);
            //var list = new List<TaskViewModel>();
            return Json(list.ToDataSourceResult(request));
        }
        public ActionResult ProjectNotAssigned()
        {
            return View();
        }

        public async Task<IActionResult> ReadWBSTimelineGanttChartData([DataSourceRequest] DataSourceRequest request, string projectId, List<string> projectIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
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
            var list = await _pmtBusiness.ReadWBSTimelineGanttChartData(_userContext.UserId, projectId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> ReadProjectTimelineData([DataSourceRequest] DataSourceRequest request)
        {           
            var list = await _pmtBusiness.ReadProjectTimelineData();
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> ReadProjectTaskGridViewData(string projectId, string projectIds = null, string senderIds = null, string recieverids = null, string statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
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
            var list = await _pmtBusiness.ReadProjectTaskGridViewData(_userContext.UserId,projectId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            var j = Json(list);
            return j;
        }
        public virtual JsonResult ReadWBSTimelineGanttDependencyData([DataSourceRequest] DataSourceRequest request)
        {
            var list = new List<ProjectGanttTaskViewModel>();
            list.Add(new ProjectGanttTaskViewModel { Title = "Completed Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            list.Add(new ProjectGanttTaskViewModel { Title = "InProgress Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            list.Add(new ProjectGanttTaskViewModel { Title = "Draft Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            list.Add(new ProjectGanttTaskViewModel { Title = "Draft Task", Start = DateTime.Now, End = DateTime.Now, UserName = "Saman", OwnerName = "System Admin" });
            //var list = _pmtBusiness.GetWBSTimelineGanttDependencyData(projectId);
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<IActionResult> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList)
        {            
            if (_userContext.UserRoleCodes.IsNotNull() &&_userContext.UserRoleCodes.Contains("PROJECT_MANAGER"))
            {
               var result1 = await _pmtBusiness.GetInboxMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
               // var result1 = await _pmtBusiness.GetInboxMenuItem(id, type, parentId, userRoleId, _userContext.UserId, "bf9cabaa-4928-41c1-8b8d-0e949e163075", stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
                var model1 = result1.ToList();
                return Json(model1);
            }
            else if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PROJECT_USER"))
            {
               var result = await _pmtBusiness.GetInboxMenuItemByUser(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
               var model = result.ToList();
               return Json(model);
            }            
            return Json(new List<TASTreeViewViewModel>());
        }

        public async Task<IActionResult> ProjectMindMap(string projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        public async Task<IActionResult> MindMapSettings()
        {
            return View();
        }
          public async Task<IActionResult> WBSDiagram(string projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        public async Task<IActionResult> GetWBSItem(string projectId)
        {
            //var result = await _pmtBusiness.GetWBSItemData(null, null, null, null, _userContext.UserId, _userContext.UserRoleIds, null, null, null, null, _userContext.UserRoleCodes);
            //return Json(result.ToList());

            var result = await _pmtBusiness.ReadMindMapData(projectId);
            return Json(result.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> SaveMindMap(ServiceTemplateViewModel model)
        {
            var res = await _pmtBusiness.CreateMindMap(model.Json);
            return null;
        }
        public async Task<ActionResult> WorklistDashboardCount(string moduleCodes,string userId,string taskTemplateIds, string serviceTemplateIds)
        {
            if (!userId.IsNotNullAndNotEmpty()) 
            {
                userId = _userContext.UserId;
            }             
            var count =await _serviceBusiness.GetWorklistDashboardCount(userId, moduleCodes,null, taskTemplateIds, serviceTemplateIds);
           // var j = Json(count);
            return Json(count);
        }
        public async Task<ActionResult> WorklistDashboardNotesCount(string moduleCodes, string userId, string noteTemplateIds)
        {
            if (!userId.IsNotNullAndNotEmpty())
            {
                userId = _userContext.UserId;
            }
            //var userId = _userContext.UserId;
             var count =await _noteBusiness.NotesDashboardCount(userId,null, moduleCodes, noteTemplateIds);
            return Json(count);
        }
        public ActionResult ProjectPlanningView(string ProjectId)
        {
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId
            };
            return View(model);
        }
        public async Task<ActionResult> ReadProjectPlanningTaskData(string projectId)
        {

            var result = await _pmtBusiness.ReadMindMapData(projectId);
            result = result.Where(x => x.Type == "TASK" || x.Type == "SUBTASK").ToList();

            var events = new List<ProjectTimelineViewModel>();
            foreach (var i in result)
            {
                events.Add(new ProjectTimelineViewModel()
                {
                    id = i.Id,
                    title = i.Title+" - "+i.UserName,
                    start = i.Start.ToString("yyyy-MM-dd"), //.ToString("yyyy-MM-dd"),//"2020-04-18"
                    end = i.End.ToString("yyyy-MM-dd"), // .ToString("yyyy-MM-dd"), //"2020-04-18"
                    allDay = false
                });
            }
            return Json(events.ToArray());
        }
        public async Task<ActionResult> ReadProjectsTaskForPlanning([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            var result = await _pmtBusiness.ReadMindMapData(projectId);
            //result = result.Where(x => x.Type == "TASK" || x.Type == "SUBTASK").ToList();
            var model = result.Select(x => new ProjectTaskViewModel { Name = x.Title, Id = x.Id, ParentId = x.ParentId,UserName=x.OwnerName,ServiceId=x.ServiceId,StartDate=x.Start,DueDate=x.End ,Type=x.Type, ProjectStatusCode=x.TaskStatusCode}).ToList();
            var j = Json(model.ToDataSourceResult(request));
            return j;
        }
        [HttpPost]
        public async Task<ActionResult> UpdateTaskScheduleDate(TaskViewModel model)
        {
            var task = await _taskBusiness.GetSingleById(model.Id);
            if (task != null)
            {
                task.StartDate = model.StartDate;
                task.DueDate = model.DueDate;
                if (model.DueDate.IsNotNull())
                {
                    task.StartDate = model.StartDate;
                    task.DueDate = model.DueDate;
                }
                else
                {
                    task.StartDate = model.StartDate;
                    task.DueDate = model.StartDate.Value.Add(model.TaskSLA);
                }
                await _taskBusiness.Edit(task);
            }
            return Json(new
            {
                success = true
            });
        }
        public async Task<IActionResult> TestRecieveEmail(ServiceTemplateViewModel model)
        {
            await _emailBusiness.ReceiveMail();
            return null;
        }
        public async Task<JsonResult> ReadProjectMembers(string projectId)
        {
            var model =await _pmtBusiness.ReadProjectTaskUserData(projectId);
            return Json(model);
        }
        public async Task<ActionResult> SubOrdinateDashboardCount(string userId, string moduleCodes)
        {          
            var count = await _serviceBusiness.GetWorklistDashboardCount(userId, moduleCodes);
            // var j = Json(count);
            return Json(count);
        }

        public virtual async Task<JsonResult> DestroyTask([DataSourceRequest] DataSourceRequest request, ProjectGanttTaskViewModel task)
        {
            if (ModelState.IsValid)
            {
               await  _taskBusiness.Delete(task.Id);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual async Task<JsonResult> CreateTask([DataSourceRequest] DataSourceRequest request, ProjectGanttTaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                var tasktemp = new TaskTemplateViewModel
                {
                    TemplateCode = "PROJECT_ADHOC_TASK",
                    DataAction = DataActionEnum.Create,
                    TaskSubject = task.Title,
                    StartDate = task.Start,
                    DueDate = task.End,
                    TaskStatusCode = "TASK_STATUS_DRAFT",
                    OwnerUserId = _userContext.UserId
                    //Priority = task.Priority,


                };
                await  _taskBusiness.ManageTask(tasktemp);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual async Task<JsonResult> UpdateTask([DataSourceRequest] DataSourceRequest request, ProjectGanttTaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                var tasktemp = await _taskBusiness.GetSingleById(task.Id);
                if (tasktemp != null)
                {
                    tasktemp.TaskSubject = task.Title;
                    tasktemp.StartDate = task.Start;
                    tasktemp.DueDate = task.End;
                    await _taskBusiness.Edit(tasktemp);
                }
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public async Task<ActionResult> ProjectHierarchy(string hierarchyCode, string permissions)
        {
            var date = DateTime.Now.Date;
            //var LoggedInUserPositionId = _userContext.OrganizationId;

            var rootNodes = await _userBusiness.GetUserHierarchyRootId(_userContext.UserId, hierarchyCode, _userContext.UserId);
            var hierarchyId = rootNodes.Item3;
            var HierarchyRootNodeId = rootNodes.Item1;
            var AllowedRootNodeId = rootNodes.Item2;
            //var lvl = await _hrCoreBusiness.GetUserNodeLevel(rootNodes.Item2, rootNodes.Item3);
            var viewModel = new UserChartIndexViewModel
            {
                HierarchyId = hierarchyId,
                HierarchyRootNodeId = "-1",
                AllowedRootNodeId = "-1",

                CanAddRootNode = HierarchyRootNodeId == AllowedRootNodeId && HierarchyRootNodeId == "",
                AllowedRootNodeLevel = 0,
                AsOnDate = date.ToYYYY_MM_DD_DateFormat(),
                // RequestSource = rs,  
                Permission = permissions

            };
            // ViewBag.PositionId = orgId;           

            ViewBag.IsAsOnDate = date.ToYYYY_MM_DD_DateFormat();
            // ViewBag.LoggedInEmpId = personId;
            //ViewBag.LoggedInPositionId = LoggedInUserPositionId;

            ViewBag.AsOnDateDisplay = date.ToYYYY_MM_DD_DateFormat();
            ViewBag.Permissions = permissions;
            return View(viewModel);
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchy(string parentId, int levelUpto, string nodeType)
        {
            var result = await _pmtBusiness.GetProjectHierarchy(parentId, levelUpto, nodeType);
            return result;
        }

    }
}
