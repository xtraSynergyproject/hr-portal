using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pms.Controllers
{
    [Area("Pms")]
    public class PerformanceController : ApplicationController
    {

        private readonly IPerformanceManagementBusiness _pmtBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserContext _userContext;
        private readonly IPushNotificationBusiness _notificationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        public PerformanceController(IEmailBusiness emailBusiness,IPushNotificationBusiness notificationBusiness, IPerformanceManagementBusiness pmtBusiness, IUserBusiness _userBusiness, 
            IUserContext userContext, IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
            ITaskBusiness taskBusiness
            , INoteBusiness noteBusiness, IHRCoreBusiness hRCoreBusiness)
        {
            _pmtBusiness = pmtBusiness;
            _userContext = userContext;
            _notificationBusiness = notificationBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _emailBusiness = emailBusiness;
            _hRCoreBusiness = hRCoreBusiness;
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
        public ActionResult WbsTimeLine(string performanceId,string objectiveId, string templateName, InboxTypeEnum inboxType)
        {
            var model = new ProgramDashboardViewModel { Id = performanceId, ObjectiveId=objectiveId, TemplateName = templateName ,InboxType=inboxType};
            return View(model);

        }
        //public async Task<ActionResult> ProjectDashboard(string projectId)
        //{
        //    var projectlist = await _pmtBusiness.GetProjectSharedList(_userContext.UserId);
        //    if (projectId.IsNullOrEmpty() && projectlist != null && projectlist.Count > 0)
        //    {
        //        projectId = projectlist.FirstOrDefault().Id;
        //    }
        //    var model = await _pmtBusiness.GetPerformanceDashboardDetails(projectId);
        //    if (model != null)
        //    {
        //        model.ProjectList = projectlist.ToList();
        //    }
        //    return View(model);
        //}
        public async Task<ActionResult> GetPerformanceTaskChartByStatus(string projectId,string userId,string stageId)
        {
            var viewModel = await _pmtBusiness.GetTaskStatus(userId, projectId, stageId);
            //return Json(viewModel);

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList()
            };
            return Json(newlist);
            
        }
        public async Task<ActionResult> GetPerformanceServiceChartByStatus(string projectId,string servicetype,string stageId,string userId)
        {
            var viewModel = await _pmtBusiness.GetPerformanceServiceStatus(userId, projectId, servicetype, stageId);
            
            //var viewModel = await _pmtBusiness.GetPerformanceServicesStatus(_userContext.UserId,projectId, servicetype);
            //return Json(viewModel);

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList()
            };
            return Json(newlist);
        }
        public async Task<ActionResult> GetPerformanceTaskChartByType(string performanceId, string stageId, string userId)
        {
            var viewModel = await _pmtBusiness.GetTaskType(userId, performanceId, stageId);           
            //return Json(viewModel);

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList()
            };
            return Json(newlist);
        }
        public async Task<ActionResult> GetProjectStageChart(string projectId)
        {
            var viewModel = await _pmtBusiness.ReadPerformanceStageChartData(_userContext.UserId,projectId);
            //var viewModel = new List<ProjectDashboardChartViewModel>();
            //viewModel.Add(new ProjectDashboardChartViewModel { Value = 5, Type = "Test Stage 01", Code = "TestStage01" });
            //viewModel.Add(new ProjectDashboardChartViewModel { Value = 10, Type = "Test Stage 02", Code = "TestStage02" });
            return Json(viewModel);
        }
        public async Task<ActionResult> GetPerformanceUserIdNameList(string performanceId)
        {
            var viewModel = await _pmtBusiness.ReadPerformanceDashboardData(_userContext.UserId, performanceId, _userContext.UserRoleCodes);
            var list = viewModel.Select(x => new IdNameViewModel()
            {
                Id = x.UserId,
                Name = x.UserName

            }).GroupBy(x=>x.Id).Select(p=>p.FirstOrDefault()).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetPerformanceStageIdNameList(string performanceId, List<string> ownerIds = null, List<string> types = null)
        {
            var viewModel = await _pmtBusiness.GetPerformanceStageIdNameList(_userContext.UserId, performanceId,ownerIds,types);
            return Json(viewModel);
        }
        public async Task<ActionResult> GetPerformanceObjectiveIdNameList(string performanceId)
        {
            var viewModel = await _pmtBusiness.GetPerformanceObjectiveList(_userContext.UserId, performanceId);
            return Json(viewModel);
        }
        public async Task<ActionResult> GetProjectIdNameList()
        {
            var projectlist = await _pmtBusiness.GetPerformanceSharedList(_userContext.UserId);
            return Json(projectlist);
        }
        public async Task<ActionResult> GetPerformanceList(string ownerId = null)
        {
            var userId = _userContext.UserId;
            if (ownerId.IsNotNullAndNotEmpty())
            {
                userId = ownerId;
            }      
            var list = await _pmtBusiness.GetPerformanceDocumentList(userId);
            var j = Json(list);
            return j;
        }
        public async Task<ActionResult> GetPerformanceTaskList(string ownerId = null)
        {
            var userId = _userContext.UserId;
            if (ownerId.IsNotNullAndNotEmpty())
            {
                userId = ownerId;
            }
            var list = await _pmtBusiness.GetPerformanceDocumentTaskList();
            var j = Json(list);
            return j;
        }
        public async Task<ActionResult> GetPerformanceDetailsList(string MasterId = null,string deptId=null, string userId=null,string masterStageId=null)
        {          
            var list = await _pmtBusiness.GetPerformanceDocumentDetailsData(MasterId,deptId,userId,masterStageId);

            var j = Json(list);
            return j;
        }
        public async Task<ActionResult> GetPerformanceDetails(string MasterId = null, string deptId = null, string userId = null,string stageId=null)
        {
            var dmaster = await _pmtBusiness.GetPDMDetails(MasterId);
            if (dmaster.IsNotNull())
            {
                var list = await _pmtBusiness.GetPerformanceDocumentDetailsData(dmaster.UdfNoteId,null, null, stageId);
                if (deptId.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.DepartmentId == deptId).ToList();
                }
                if (userId.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.UserId == userId).ToList();
                }

                var j = Json(list);
                return j;
            }
            return null;
            
        }
        public ActionResult PerformanceDocumentList()
        {
            // var isProjectManager = IsProjectManager();
            PerformanceDocumentViewModel model = new PerformanceDocumentViewModel()
            {
                // Id = ProjectId,
                // IsAssignee = isProjectManager ? true : false
            };
            model.UserId = _userContext.UserId;
            return View(model);
        }
        public async Task<ActionResult> PerformanceDocumentDetailsData(string masterNodeId,string masterStageId)
        {
            
            PerformanceDocumentViewModel model = new PerformanceDocumentViewModel()
            {
                Id = masterNodeId,
               MasterStageId= masterStageId
            };
            model.UserId = _userContext.UserId;
            var masterDetails = await _pmtBusiness.GetPerformanceDocumentMasterByNoteId(masterNodeId);
            if (masterDetails.IsNotNull())
            {
                model.MasterName = masterDetails.Name;
            }
            var stage = await _pmtBusiness.GetPerformanceDocumentStageData(null, null, masterStageId);
            if (stage.IsNotNull()) 
            {
                model.MasterStageName = stage.Select(x=>x.Name).FirstOrDefault();
            }
            return View(model);
        }
        public ActionResult PerformanceDocumentTaskList()
        {
            
            PerformanceDocumentViewModel model = new PerformanceDocumentViewModel()
            {
              
            };
            model.UserId = _userContext.UserId;
            return View(model);
        }
        public ActionResult GetProjectTimeLog(long projectId)
        {
            //var viewModel = _pmtBusiness.GetTimeLog(projectId);
            var viewModel = new ProjectDashboardChartViewModel();
            return Json(viewModel);
        }
        public async Task<ActionResult> PerformanceWorkBreakDownStructure(string PerformanceId)
        {
            ViewBag.ProjectId = PerformanceId;
            var model = new ProgramDashboardViewModel { Id = PerformanceId };
            return View(model);
        }
        public ActionResult GetProjectAttachments([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            //var list = _pmtBusiness.ReadTaskFileData(projectId);
            var list = new List<FileViewModel>();
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetProjectNotificationList([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            //var result = _notificationBusiness.GetProjectNotificationList(LoggedInUserId, projectId);
            //var result = new List<NotificationViewModel>();
            var userId = _userContext.UserId;
            var result = await _notificationBusiness.GetServiceNotificationList(projectId, userId, 0);
            var j = Json(result);
            //var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public ActionResult GetProjectTeamData([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            //var list = _pmtBusiness.ReadProjectTeamData(projectId);
            var list = new List<TeamTaskDashboardViewModel>();
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetProjectOverDueTask([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            var list = await _pmtBusiness.ReadTaskOverdueData(projectId);
            //var list = new List<TaskViewModel>();
            return Json(list);
           // return Json(list.ToDataSourceResult(request));
        }
        public IActionResult PerformanceNotAssigned()
        {
            return View();
        }

        public async Task<IActionResult> ReadWBSTimelineGanttChartData([DataSourceRequest] DataSourceRequest request, string performanceId, List<string> projectIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
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
            var list = await _pmtBusiness.ReadWBSTimelineGanttChartData(_userContext.UserId, performanceId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> ReadPerformanceTaskGridViewData(string performanceId, string projectIds = null, string senderIds = null, string recieverids = null, string statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, string type =null,string stageId=null,string userId=null, InboxTypeEnum? inboxType=null)
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
            var list = await _pmtBusiness.ReadPerformanceDashboardTaskData(userId, performanceId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate, type,stageId);
            //var list = await _pmtBusiness.ReadPerformanceDashboardData(_userContext.UserId, performanceId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate,type);
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
            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<IActionResult> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList)
        {            
            if (_userContext.UserRoleCodes.Contains("PROJECT_MANAGER"))
            {
                var result = await _pmtBusiness.GetInboxMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
                var model = result.ToList();
                return Json(model);
            }
            else if (_userContext.UserRoleCodes.Contains("PROJECT_USER"))
            {
                var result = await _pmtBusiness.GetInboxMenuItemByUser(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
                var model = result.ToList();
                return Json(model);
            }            
            return Json(new List<TreeViewViewModel>());
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
        public async Task<ActionResult> WorklistDashboardCount(string moduleCode)
        {
            var userId = _userContext.UserId;
            var count =await _serviceBusiness.GetWorklistDashboardCount(userId, moduleCode);
           // var j = Json(count);
            return Json(count);
        }
        public async Task<ActionResult> WorklistDashboardNotesCount(string moduleCode)
        {
             var userId = _userContext.UserId;
             var count =await _noteBusiness.NotesDashboardCount(userId,null, moduleCode);
            return Json(count);
        }

        
        public async Task<IActionResult> TestRecieveEmail(ServiceTemplateViewModel model)
        {
            await _emailBusiness.ReceiveMail();
            return null;
        }
        public async Task<JsonResult> ReadProjectMembers(string projectId)
        {
            var model =await _pmtBusiness.ReadPerformanceTaskUserData(projectId);
            return Json(model);
        }
        public async Task<ActionResult> SubOrdinateDashboardCount(string userId)
        {          
            var count = await _serviceBusiness.GetWorklistDashboardCount(userId, null);
            // var j = Json(count);
            return Json(count);
        }

        public async Task<IActionResult> DepartmentGoals(string departmentId,string lo = null)
        {
            var model = new GoalViewModel()
            {
                Department = departmentId
            };
            if(model.Department.IsNullOrEmptyOrWhiteSpace())
            {
                model.Department = _userContext.OrganizationId;
            }
            bool enableCreate=false;

            var deptdetails = await _hRCoreBusiness.GetAllOrganisation();
            deptdetails = deptdetails.Where(x => x.DepartmentOwnerUserId == _userContext.UserId).ToList();

            var userrole = _userContext.UserRoleCodes;
            string[] roles = userrole.Split(",").ToArray();
            foreach(var role in roles)
            {
                if(role == "PERFORMANCE_MANAGER" || role == "ADMIN")
                {
                    enableCreate = true;
                } 
            }
            if (deptdetails.Count>0)
            {
                enableCreate = true;
            }
            if (lo == "popup")
            {
                ViewBag.Layout = lo;
                ViewBag.PortalId = _userContext.PortalId;
            }

            ViewBag.EnableCreateButton = enableCreate;            
            return View(model);
        }

        public async Task<IActionResult> GetDepartmentListByOrganization()
        {
            var model = await _pmtBusiness.GetDepartmentListByOrganization(_userContext.OrganizationId);
            return Json(model);
        }
        
        public async Task<IActionResult> GetAllYearFromPerformanceMaster(string departmentId)
        {
            var model = await _pmtBusiness.GetAllYearFromPerformanceMaster(departmentId);
            return Json(model);
        }
        
        public async Task<IActionResult> GetPerformanceMasterByDepartment(string departmentId, string year)
        {
            var model = await _pmtBusiness.GetPerformanceMasterByDepatment(departmentId, year);
            return Json(model);
        }
        
        public async Task<IActionResult> GetDepartmentGoalByDepartment(string departmentId, string documentMasterId)
        {
            var model = await _pmtBusiness.GetDepartmentGoalByDepartment(departmentId, documentMasterId);
            return Json(model);
        }
    }
}
