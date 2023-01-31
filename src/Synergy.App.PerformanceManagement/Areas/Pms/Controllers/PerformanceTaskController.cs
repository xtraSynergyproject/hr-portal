using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pms.Controllers
{
    [Area("PMS")]
    public class PerformanceTaskController : ApplicationController
    {
        private readonly IPerformanceManagementBusiness _performanceManagementBusiness;
        private readonly IUserContext _userContext;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;

        public PerformanceTaskController(IPerformanceManagementBusiness performanceManagementBusiness, IUserContext userContext,
            ITaskBusiness taskBusiness,
                                        IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness,
                                        IUserHierarchyBusiness userHierarchyBusiness)
        {
            _performanceManagementBusiness = performanceManagementBusiness;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
        }

        public bool IsProjectManager()
        {
            var isProjectManager = false;
            if (_userContext.UserRoleCodes.IsNotNullAndNotEmpty())
            {
                var userRole = _userContext.UserRoleCodes.Contains("PROJECT_MANAGER");
                if (userRole)
                {
                    isProjectManager = true;
                }
            }
            return isProjectManager;
        }
        public async Task<ActionResult> GetPerformanceUserList()
        {
            var userList = new List<IdNameViewModel>();
            var subordinate = await _userHierarchyBusiness.GetHierarchyUsers("PERFORMANCE_HIERARCHY", _userContext.UserId, 1, 1);
            userList = subordinate.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            userList.Insert(0, new IdNameViewModel { Id = _userContext.UserId, Name = _userContext.Name });
            // userList.Add(new IdNameViewModel {Id=_userContext.UserId,Name=_userContext.Name });

            var j = Json(userList);
            return j;
        }
        public async Task<ActionResult> GetPerformanceUserYearList(string userId)
        {
            var userYearList = new List<IdNameViewModel>();
            userYearList = await _performanceManagementBusiness.GetYearByUserId(userId);
            // userYearList.Add(new IdNameViewModel { Id="py01",Name="Test 2021"});
            var j = Json(userYearList);
            return j;
        }
        public async Task<ActionResult> GetPerformanceList(string ownerId = null, string year = null)
        {
            var userId = _userContext.UserId;
            if (ownerId.IsNotNullAndNotEmpty())
            {
                userId = ownerId;
            }
            var isProjectManager = IsProjectManager();

            var projectList = await _performanceManagementBusiness.GetPerformanceList(userId, isProjectManager, year);
            var j = Json(projectList);
            return j;
        }
        public async Task<ActionResult> GetPDMList(string ownerId = null, string year = null)
        {
            var userId = _userContext.UserId;
            if (ownerId.IsNotNullAndNotEmpty())
            {
                userId = ownerId;
            }

            var list = await _performanceManagementBusiness.GetPDMList(year);
            return Json(list);
        }
        public async Task<ActionResult> Index(string pageName, string performanceId)
        {
            var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager();

            var projectList = await _performanceManagementBusiness.GetPerformanceList(userId, isProjectManager);
            if (projectList != null)
            {
                performanceId = projectList.FirstOrDefault().Id;
            }
            ServiceViewModel model = new ServiceViewModel()
            {
                Id = performanceId,
                PageName = pageName
            };
            //var model = await _projectManagementBusiness.GetProjectDetails(ProjectId);
            //model.PageName = pageName;

            return View(model);
        }

        public async Task<ActionResult> PerformanceDetailsFilter(bool hideProject = false, bool stage = false, bool hideReceiver = false, bool isLineManage = false, bool isService = false)
        {
            ViewBag.HideProject = hideProject;
            ViewBag.Stage = stage;
            ViewBag.HideReceiver = hideReceiver;
            ViewBag.IsLineManage = isLineManage;
            ViewBag.IsService = isService;
            return View("_PerformanceDetailsFilter");
        }

        public async Task<IActionResult> PerformanceByObjectives(string ProjectId, string userId, LayoutModeEnum layout = LayoutModeEnum.Main)
        {
            var isProjectManager = IsProjectManager();
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId,
                IsAssignee = isProjectManager ? true : false
            };
            model.UserId = _userContext.UserId;
            if (layout == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            else
            {
                ViewBag.layout = null;
            }
            model.PTemplateCode = "0";
            model.PInProgressCount = 0;
            model.PCompletedCount = 0;
            model.PRejectedCount = 0;
            model.GTemplateCode = "0";
            model.GInProgressCount = 0;
            model.GCompletedCount = 0;
            model.GRejectedCount = 0;
            model.CTemplateCode = "0";
            model.CInProgressCount = 0;
            model.CCompletedCount = 0;
            model.CRejectedCount = 0;
            model.DTemplateCode = "0";
            model.DInProgressCount = 0;
            model.DCompletedCount = 0;
            model.DRejectedCount = 0;
            List<PMSDashboardViewModel> list = new List<PMSDashboardViewModel>();
            var data = await _performanceManagementBusiness.ReadPerformanceObjectivesGridViewData(_userContext.UserId);
            if (userId.IsNotNullAndNotEmpty())
            {
                model.HierarchyUserId = userId;
                data = data.Where(x => x.OwnerUserId == userId).ToList();
            }
            if (data.IsNotNull())
            {
                foreach (var item in data.GroupBy(x => x.Type))
                {
                    list.Add(new PMSDashboardViewModel
                    {
                        ServiceName = item.Select(x => x.Type).FirstOrDefault(),
                        TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
                        InProgressCount = item.Count(x => x.NtsStatusCode == "SERVICE_STATUS_INPROGRESS" || x.NtsStatusCode == "SERVICE_STATUS_OVERDUE"),
                        CompletedCount = item.Count(x => x.NtsStatusCode == "SERVICE_STATUS_COMPLETE"),
                        RejectedCount = item.Count(x => x.NtsStatusCode == "SERVICE_STATUS_REJECT" || x.NtsStatusCode == "SERVICE_STATUS_CANCEL")
                    });
                }

                foreach (var item in list)
                {
                    if (item.TemplateCode== "PMS_PERFORMANCE_DOCUMENT"/* item.ServiceName == "Performance Document"*/)
                    {
                        model.PTemplateCode = item.TemplateCode;
                        model.PInProgressCount = item.InProgressCount;
                        model.PCompletedCount = item.CompletedCount;
                        model.PRejectedCount = item.RejectedCount;
                    }
                    if (item.TemplateCode == "PMS_GOAL"/*item.ServiceName == "Goal"*/)
                    {
                        model.GTemplateCode = item.TemplateCode;
                        model.GInProgressCount = item.InProgressCount;
                        model.GCompletedCount = item.CompletedCount;
                        model.GRejectedCount = item.RejectedCount;
                    }
                    if (item.TemplateCode == "PMS_COMPENTENCY"/*item.ServiceName == "Competency"*/)
                    {
                        model.CTemplateCode = item.TemplateCode;
                        model.CInProgressCount = item.InProgressCount;
                        model.CCompletedCount = item.CompletedCount;
                        model.CRejectedCount = item.RejectedCount;
                    }
                    if (item.TemplateCode == "PMS_DEVELOPMENT"/*item.ServiceName == "Development"*/)
                    {
                        model.DTemplateCode = item.TemplateCode;
                        model.DInProgressCount = item.InProgressCount;
                        model.DCompletedCount = item.CompletedCount;
                        model.DRejectedCount = item.RejectedCount;
                    }
                }
            }

            return View(model);
        }



        public async Task<IActionResult> ReadPerformanceObjectivesGridViewData([DataSourceRequest] DataSourceRequest request, string userId, List<string> projectIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, List<string> pmsTypes = null, List<string> stageIds = null, string statusCodes = null)
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
            var list = await _performanceManagementBusiness.ReadPerformanceObjectivesGridViewData(_userContext.UserId, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate, pmsTypes, stageIds, statusCodes);
            if (userId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.OwnerUserId == userId).ToList();
            }
            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<IActionResult> EmployeePerformanceDetails(string ProjectId, string userId, LayoutModeEnum layout = LayoutModeEnum.Main)
        {
            var isProjectManager = IsProjectManager();
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId,
                IsAssignee = isProjectManager ? true : false
            };
            model.UserId = _userContext.UserId;
            if (layout == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            else
            {
                ViewBag.layout = null;
            }
            model.GTemplateCode = "0";
            model.GInProgressCount = 0;
            model.GCompletedCount = 0;
            model.GRejectedCount = 0;
            model.CTemplateCode = "0";
            model.CInProgressCount = 0;
            model.CCompletedCount = 0;
            model.CRejectedCount = 0;
            model.DTemplateCode = "0";
            model.DInProgressCount = 0;
            model.DCompletedCount = 0;
            model.DRejectedCount = 0;
            List<PMSDashboardViewModel> list = new List<PMSDashboardViewModel>();
            var data = await _performanceManagementBusiness.ReadEmployeePerformanceObjectivesData(userId);
            if (userId.IsNotNullAndNotEmpty())
            {
                model.HierarchyUserId = userId;
            }
            if (data.Count > 0)
            {
                model.UserName = data.FirstOrDefault().OwnerName;
                foreach (var item in data.GroupBy(x => x.Type))
                {
                    list.Add(new PMSDashboardViewModel
                    {
                        ServiceName = item.Select(x => x.Type).FirstOrDefault(),
                        TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
                        InProgressCount = item.Count(x => x.NtsStatusCode == "SERVICE_STATUS_INPROGRESS" || x.NtsStatusCode == "SERVICE_STATUS_OVERDUE"),
                        CompletedCount = item.Count(x => x.NtsStatusCode == "SERVICE_STATUS_COMPLETE"),
                        RejectedCount = item.Count(x => x.NtsStatusCode == "SERVICE_STATUS_REJECT" || x.NtsStatusCode == "SERVICE_STATUS_CANCEL")
                    });
                }

                foreach (var item in list)
                {
                    if (item.ServiceName == "Goal")
                    {
                        model.GTemplateCode = item.TemplateCode;
                        model.GInProgressCount = item.InProgressCount;
                        model.GCompletedCount = item.CompletedCount;
                        model.GRejectedCount = item.RejectedCount;
                    }
                    if (item.ServiceName == "Competency")
                    {
                        model.CTemplateCode = item.TemplateCode;
                        model.CInProgressCount = item.InProgressCount;
                        model.CCompletedCount = item.CompletedCount;
                        model.CRejectedCount = item.RejectedCount;
                    }
                    if (item.ServiceName == "Development")
                    {
                        model.DTemplateCode = item.TemplateCode;
                        model.DInProgressCount = item.InProgressCount;
                        model.DCompletedCount = item.CompletedCount;
                        model.DRejectedCount = item.RejectedCount;
                    }
                }
            }

            return View(model);
        }



        public async Task<IActionResult> ReadEmployeePerformanceObjectivesData([DataSourceRequest] DataSourceRequest request, string performanceId, string year, string userId, List<string> projectIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, List<string> pmsTypes = null, List<string> stageIds = null, string statusCodes = null)
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
            var list = await _performanceManagementBusiness.ReadEmployeePerformanceObjectivesData(userId, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate, pmsTypes, stageIds, statusCodes);
            if (year.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.Year == year).ToList();

            }
            if (performanceId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.ParentId == performanceId).ToList();

            }
            var j = Json(list);
           // var j = Json(list.ToDataSourceResult(request));
            return j;
        }



        public IActionResult CreateObjectives(string userId)
        {
            if (userId.IsNotNullAndNotEmpty())
            {
                ViewBag.UserId = userId;
            }

            return View();
        }
        public IActionResult CreatePerformanceTask()
        {
            return View();
        }
        public async Task<ActionResult> GetSubordinatesIdNameList()
        {
            var list = await _performanceManagementBusiness.GetSubordinatesIdNameList();
            return Json(list);
        }
        public ActionResult PerformanceTaskByName(string ProjectId)
        {
            var isProjectManager = IsProjectManager();
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId,
                IsAssignee = isProjectManager ? true : false
            };
            model.UserId = _userContext.UserId;
            return View(model);
        }



        public async Task<IActionResult> ReadPerformanceUserWorkloadGridViewData(List<string> projectIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, List<string> pmsTypes = null, List<string> stageIds = null)
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
            var list = await _performanceManagementBusiness.ReadPerformanceTaskUserGridViewData(_userContext.UserId, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate, pmsTypes, stageIds);
            var j = Json(list);
            return j;
        }
        public async Task<ActionResult> ReadProjectTeamData([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            var isProjectManager = IsProjectManager();
            var userId = _userContext.UserId;
            var list = await _performanceManagementBusiness.ReadProjectTeamWorkloadData(projectId, userId, isProjectManager);

            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> ReadProjectTeamDataByUser([DataSourceRequest] DataSourceRequest request, string userId, string projectId)
        {
            var tasklist = await _performanceManagementBusiness.ReadProjectTeamDataByUser(projectId, userId);
            var j = Json(tasklist);
            //var j = Json(tasklist.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> ReadSubTaskData(string id, string taskId)
        {
            if (id != null)
            {
                taskId = id;
            }

            var list = await _performanceManagementBusiness.ReadProjectSubTaskViewData(taskId, null, null);
            foreach (var item in list)
            {
                var sub = await _taskBusiness.GetList(x => x.ParentTaskId == item.id);
                if (sub != null)
                {
                    item.hasChildren = true;
                }
            }
            var j = Json(list);

            return j;

        }

        public ActionResult ProjectTaskByDate(string ProjectId)
        {
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId
            };
            return View(model);
        }
        public async Task<IActionResult> ReadProjectTaskGridViewData([DataSourceRequest] DataSourceRequest request, string projectId, List<string> projectIds = null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
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

            var isProjectManager = IsProjectManager();
            var userId = _userContext.UserId;
            var list = await _performanceManagementBusiness.ReadProjectTask(userId, projectId, isProjectManager, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        //public ActionResult ReadProjectTeamDataByDate([DataSourceRequest] DataSourceRequest request, long userId)            
        //{
        //    var list = new List<TeamWorkloadViewModel>();
        //    if (userId == 1) {
        //        list.Add(new TeamWorkloadViewModel { UserName = "Sham", TaskName = "Cms Template", TaskStatus = "Completed", TaskId = "123" });
        //        list.Add(new TeamWorkloadViewModel { UserName = "Jam", TaskName = "Team Master", TaskStatus = "InProgress", TaskId = "423" });
        //        list.Add(new TeamWorkloadViewModel { UserName = "Ham", TaskName = "Recruitment", TaskStatus = "Draft", TaskId = "523" });
        //    }
        //    if (userId == 2) {
        //        list.Add(new TeamWorkloadViewModel { UserName = "Sid", TaskName = "UI Changes", TaskStatus = "Draft", TaskId = "623" });
        //        list.Add(new TeamWorkloadViewModel { UserName = "Manoj Kumar", TaskName = "Woking in portal side", TaskStatus = "Completed", TaskId = "723" });
        //    }
        //    var j = Json(list.ToDataSourceResult(request));
        //    return j;
        //}

        public async Task<ActionResult> ReadProjectTeamDateData([DataSourceRequest] DataSourceRequest request, string projectId)
        {

            var list = await _performanceManagementBusiness.ReadProjectTeamDateData(projectId);

            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<ActionResult> ReadProjectTeamDataByDate([DataSourceRequest] DataSourceRequest request, string projectId, DateTime startDate)
        {
            var isProjectManager = IsProjectManager();

            var tasklist = await _performanceManagementBusiness.ReadProjectTeamDataByDate(projectId, startDate, isProjectManager);

            var j = Json(tasklist);
            //var j = Json(tasklist.ToDataSourceResult(request));
            return j;
        }

        //public ActionResult ReadProjectTeamDateData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var list = new List<TeamWorkloadViewModel>();
        //    list.Add(new TeamWorkloadViewModel {UserId="1", StartDateStr = "1-mar-2021" });
        //    list.Add(new TeamWorkloadViewModel {UserId="2" , StartDateStr = "2-mar-2021" });
        //    list.Add(new TeamWorkloadViewModel {  StartDateStr = "3-mar-2021" });
        //    list.Add(new TeamWorkloadViewModel {  StartDateStr = "4-mar-2021" });
        //    list.Add(new TeamWorkloadViewModel { StartDateStr = "5-mar-2021" });
        //    list.Add(new TeamWorkloadViewModel {  StartDateStr = "6-mar-2021" });
        //    list.Add(new TeamWorkloadViewModel {  StartDateStr = "7-mar-2021" });
        //    list.Add(new TeamWorkloadViewModel {  StartDateStr = "8-mar-2021" });
        //    var j = Json(list.ToDataSourceResult(request));
        //    return j;
        //}


        public async Task<ActionResult> PerformanceTaskView(string ProjectId, string PerformanceStage, string type, string PerformanceUser, string PerformanceUserYear)
        {
            var model = new TeamWorkloadViewModel();
            model.Id = ProjectId; model.TaskCount = "0";
            model.PerformanceUser = PerformanceUser;
            model.PerformanceUserYear = PerformanceUserYear;
            model.PerformanceStage = PerformanceStage;
            if (PerformanceStage.IsNotNullAndNotEmpty())
            {
                var data = await _performanceManagementBusiness.GetPerformanceDocumentStageDataByServiceId(ProjectId, PerformanceUser, PerformanceStage);
                if (data.Count > 0)
                {
                    model.PerformanceStageName = data.First().StageName;
                    model.PerformanceStageOwner = data.First().OwnerDisplayName;
                    model.PerformanceStageServiceId = data.First().Id;
                    model.StartDate = (DateTime)data.First().StartDate;
                    model.DueDate = (DateTime)data.First().EndDate;

                }
            }
            model.CompletedCount = "0";
            model.DayLeft = "0";
            var templatecode = "";

            //if (type == "Competency")
            //{
            //    templatecode = "PMS_COMPENTENCY";
            //    var comptlist = await _performanceManagementBusiness.ReadPerformanceCompetencyStageViewData(ProjectId, PerformanceStage, PerformanceUser);
            //    if (comptlist.Count > 0)
            //    {
            //        model.CTemplateCode = "PMS_COMPENTENCY";
            //        model.CInProgressCount = comptlist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
            //        model.CCompletedCount = comptlist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
            //        model.CRejectedCount = comptlist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
            //        model.CTotalCount = model.CInProgressCount + model.CCompletedCount + model.CRejectedCount;
            //    }
            //    else
            //    {
            //        model.CTemplateCode = "PMS_COMPENTENCY";
            //        model.CInProgressCount = 0;
            //        model.CCompletedCount = 0;
            //        model.CRejectedCount = 0;
            //        model.CTotalCount = 0;
            //    }
            //}
            //else if (type == "Development")
            //{
            //    templatecode = "PMS_DEVELOPMENT";
            //    var devloplist = await _performanceManagementBusiness.ReadPerformanceDevelopmentViewData(ProjectId, PerformanceStage, PerformanceUser);
            //    if (devloplist.Count > 0)
            //    {
            //        model.DTemplateCode = templatecode;
            //        model.DInProgressCount = devloplist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
            //        model.DCompletedCount = devloplist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
            //        model.DRejectedCount = devloplist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
            //        model.DTotalCount = model.DInProgressCount + model.DCompletedCount + model.DRejectedCount;
            //    }
            //    else
            //    {
            //        model.DTemplateCode = templatecode;
            //        model.DInProgressCount = 0;
            //        model.DCompletedCount = 0;
            //        model.DRejectedCount = 0;
            //        model.DTotalCount = 0;
            //    }
            //}
            //else if (type == "All")
            //{
            //    templatecode = "All";
            //    var alllist = await _performanceManagementBusiness.ReadPerformanceAllData(ProjectId, PerformanceStage, PerformanceUser);
            //    if (alllist.Count > 0)
            //    {
            //        model.DTemplateCode = "PMS_DEVELOPMENT";
            //        model.DInProgressCount = alllist.Where(x => x.TemplateCode == "PMS_DEVELOPMENT").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
            //        model.DCompletedCount = alllist.Where(x => x.TemplateCode == "PMS_DEVELOPMENT").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
            //        model.DRejectedCount = alllist.Where(x => x.TemplateCode == "PMS_DEVELOPMENT").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
            //        model.DTotalCount = model.DInProgressCount + model.DCompletedCount + model.DRejectedCount;
            //        model.CTemplateCode = "PMS_COMPENTENCY";
            //        model.CInProgressCount = alllist.Where(x => x.TemplateCode == "PMS_COMPENTENCY").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
            //        model.CCompletedCount = alllist.Where(x => x.TemplateCode == "PMS_COMPENTENCY").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
            //        model.CRejectedCount = alllist.Where(x => x.TemplateCode == "PMS_COMPENTENCY").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
            //        model.CTotalCount = model.CInProgressCount + model.CCompletedCount + model.CRejectedCount;
            //        model.GTemplateCode = "PMS_GOAL";
            //        model.GInProgressCount = alllist.Where(x => x.TemplateCode == "PMS_GOAL").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
            //        model.GCompletedCount = alllist.Where(x => x.TemplateCode == "PMS_GOAL").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
            //        model.GRejectedCount = alllist.Where(x => x.TemplateCode == "PMS_GOAL").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
            //        model.GTotalCount = model.GInProgressCount + model.GCompletedCount + model.GRejectedCount;
            //    }
            //    else
            //    {
            //        model.DTemplateCode = "PMS_DEVELOPMENT";
            //        model.DInProgressCount = 0;
            //        model.DCompletedCount = 0;
            //        model.DRejectedCount = 0;
            //        model.DTotalCount = 0;
            //        model.GTemplateCode = "PMS_GOAL";
            //        model.GInProgressCount = 0;
            //        model.GCompletedCount = 0;
            //        model.GRejectedCount = 0;
            //        model.GTotalCount = 0;
            //        model.CTemplateCode = "PMS_COMPENTENCY";
            //        model.CInProgressCount = 0;
            //        model.CCompletedCount = 0;
            //        model.CRejectedCount = 0;
            //        model.CTotalCount = 0;
            //    }
            //}
            //else
            //{
            //    templatecode = "PMS_GOAL";
            //    var goallist = await _performanceManagementBusiness.ReadManagerPerformanceGoalViewData(ProjectId, PerformanceStage, PerformanceUser);
            //    if (goallist.Count > 0)
            //    {
            //        model.GTemplateCode = templatecode;
            //        model.GInProgressCount = goallist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
            //        model.GCompletedCount = goallist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
            //        model.GRejectedCount = goallist.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
            //        model.GTotalCount = model.GInProgressCount + model.GCompletedCount + model.GRejectedCount;
            //    }
            //    else
            //    {
            //        model.GTemplateCode = templatecode;
            //        model.GInProgressCount = 0;
            //        model.GCompletedCount = 0;
            //        model.GRejectedCount = 0;
            //        model.GTotalCount = 0;
            //    }
            //}

            var alllist = await _performanceManagementBusiness.ReadPerformanceAllData(ProjectId, PerformanceStage, PerformanceUser);
            if (alllist.Count > 0)
            {
                model.DTemplateCode = "PMS_DEVELOPMENT";
                model.DInProgressCount = alllist.Where(x => x.TemplateCode == "PMS_DEVELOPMENT").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
                model.DCompletedCount = alllist.Where(x => x.TemplateCode == "PMS_DEVELOPMENT").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
                model.DRejectedCount = alllist.Where(x => x.TemplateCode == "PMS_DEVELOPMENT").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
                model.DTotalCount = model.DInProgressCount + model.DCompletedCount + model.DRejectedCount;
                model.CTemplateCode = "PMS_COMPENTENCY";
                model.CInProgressCount = alllist.Where(x => x.TemplateCode == "PMS_COMPENTENCY").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
                model.CCompletedCount = alllist.Where(x => x.TemplateCode == "PMS_COMPENTENCY").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
                model.CRejectedCount = alllist.Where(x => x.TemplateCode == "PMS_COMPENTENCY").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
                model.CTotalCount = model.CInProgressCount + model.CCompletedCount + model.CRejectedCount;
                model.GTemplateCode = "PMS_GOAL";
                model.GInProgressCount = alllist.Where(x => x.TemplateCode == "PMS_GOAL").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE");
                model.GCompletedCount = alllist.Where(x => x.TemplateCode == "PMS_GOAL").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE");
                model.GRejectedCount = alllist.Where(x => x.TemplateCode == "PMS_GOAL").Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL");
                model.GTotalCount = model.GInProgressCount + model.GCompletedCount + model.GRejectedCount;
            }
            else
            {
                model.DTemplateCode = "PMS_DEVELOPMENT";
                model.DInProgressCount = 0;
                model.DCompletedCount = 0;
                model.DRejectedCount = 0;
                model.DTotalCount = 0;
                model.GTemplateCode = "PMS_GOAL";
                model.GInProgressCount = 0;
                model.GCompletedCount = 0;
                model.GRejectedCount = 0;
                model.GTotalCount = 0;
                model.CTemplateCode = "PMS_COMPENTENCY";
                model.CInProgressCount = 0;
                model.CCompletedCount = 0;
                model.CRejectedCount = 0;
                model.CTotalCount = 0;
            }

            List<PMSDashboardViewModel> list = new List<PMSDashboardViewModel>();
            var codes = type == "All" ? "PMS_GOAL,PMS_COMPENTENCY,PMS_DEVELOPMENT" : templatecode;
            //var result = await _performanceManagementBusiness.GetTaskListByType(codes, ProjectId, PerformanceUser, PerformanceStage);
            //var stagelist = await _performanceManagementBusiness.GetStageTaskList(codes, ProjectId, PerformanceUser, model.PerformanceStageServiceId);

            var result = await _performanceManagementBusiness.GetTaskListByType("PMS_GOAL,PMS_COMPENTENCY,PMS_DEVELOPMENT", ProjectId, PerformanceUser, PerformanceStage);
            var stagelist = await _performanceManagementBusiness.GetStageTaskList("PMS_GOAL,PMS_COMPENTENCY,PMS_DEVELOPMENT", ProjectId, PerformanceUser, model.PerformanceStageServiceId);

            if (stagelist.Count > 0)
            {
                model.TaskPendingCount = stagelist.Count(x => x.NtsStatusCode == "TASK_STATUS_INPROGRESS" || x.NtsStatusCode == "TASK_STATUS_OVERDUE");
                model.TaskCompletedCount = stagelist.Count(x => x.NtsStatusCode == "TASK_STATUS_COMPLETE");
                model.TaskCancelledCount = stagelist.Count(x => x.NtsStatusCode == "TASK_STATUS_REJECT" || x.NtsStatusCode == "TASK_STATUS_CANCEL");
                model.TaskTotalCount = model.TaskPendingCount + model.TaskCompletedCount + model.TaskCancelledCount;

            }
            if (result.IsNotNull())
            {
                model.TaskPendingCount += result.Count(x => x.NtsStatusCode == "TASK_STATUS_INPROGRESS" || x.NtsStatusCode == "TASK_STATUS_OVERDUE");
                model.TaskCompletedCount += result.Count(x => x.NtsStatusCode == "TASK_STATUS_COMPLETE");
                model.TaskCancelledCount += result.Count(x => x.NtsStatusCode == "TASK_STATUS_REJECT" || x.NtsStatusCode == "TASK_STATUS_CANCEL");
                model.TaskTotalCount = model.TaskPendingCount + model.TaskCompletedCount + model.TaskCancelledCount;

                //foreach (var item in result.GroupBy(x => x.TemplateCode))
                //{
                //    list.Add(new PMSDashboardViewModel
                //    {                        
                //        TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
                //        InProgressCount = item.Count(x => x.NtsStatusCode == "TASK_STATUS_INPROGRESS" || x.NtsStatusCode == "TASK_STATUS_OVERDUE"),
                //        CompletedCount = item.Count(x => x.NtsStatusCode == "TASK_STATUS_COMPLETE"),
                //        RejectedCount = item.Count(x => x.NtsStatusCode == "TASK_STATUS_REJECT" || x.NtsStatusCode == "TASK_STATUS_CANCEL")
                //    });
                //}

                //foreach (var item in list)
                //{                    
                //    if (item.TemplateCode == "PMS_GOAL" || item.TemplateCode == "PMS_GOAL_ADHOC_TASK")
                //    {                        
                //        model.GInProgressCount += item.InProgressCount;
                //        model.GCompletedCount += item.CompletedCount;
                //        model.GRejectedCount += item.RejectedCount;
                //    }
                //    if (item.TemplateCode == "PMS_COMPENTENCY" || item.TemplateCode == "PMS_COMPENTENCY_ADHOC_TASK")
                //    {                        
                //        model.CInProgressCount += item.InProgressCount;
                //        model.CCompletedCount += item.CompletedCount;
                //        model.CRejectedCount += item.RejectedCount;
                //    }
                //    if (item.TemplateCode == "PMS_DEVELOPMENT")
                //    {                        
                //        model.DInProgressCount = item.InProgressCount;
                //        model.DCompletedCount = item.CompletedCount;
                //        model.DRejectedCount = item.RejectedCount;
                //    }
                //}
            }

            //model.PInProgressCount = model.GInProgressCount + model.CInProgressCount + model.DInProgressCount;
            //model.PCompletedCount = model.GCompletedCount + model.CCompletedCount + model.DCompletedCount;
            //model.PRejectedCount = model.GRejectedCount + model.CRejectedCount + model.DRejectedCount;


            #region 
            //var serviceList = await _performanceManagementBusiness.GetServiceListByPDMId(ProjectId, codes, PerformanceUser);
            //List<PMSDashboardViewModel> serlist = new List<PMSDashboardViewModel>();
            //if (serviceList != null)
            //{
            //    foreach (var item in serviceList.GroupBy(x => x.TemplateCode))
            //    {
            //        serlist.Add(new PMSDashboardViewModel
            //        {
            //            TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
            //            InProgressCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE"),
            //            CompletedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE"),
            //            RejectedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
            //        });
            //    }
            //    foreach (var item in serlist)
            //    {
            //        if (item.TemplateCode == "PMS_GOAL")
            //        {
            //            model.GTemplateCode = item.TemplateCode;    
            //            model.GInProgressCount = item.InProgressCount;
            //            model.GCompletedCount = item.CompletedCount;
            //            model.GRejectedCount = item.RejectedCount;
            //            model.GTotalCount = model.GInProgressCount + model.GCompletedCount + model.GRejectedCount;                                              
            //        }
            //        if (item.TemplateCode == "PMS_COMPENTENCY")
            //        {
            //            model.CTemplateCode = item.TemplateCode;
            //                model.CInProgressCount = item.InProgressCount;
            //                model.CCompletedCount = item.CompletedCount;
            //                model.CRejectedCount = item.RejectedCount;
            //                model.CTotalCount = model.CInProgressCount + model.CCompletedCount + model.CRejectedCount;                                             
            //        }
            //        if (item.TemplateCode == "PMS_DEVELOPMENT")
            //        {
            //            model.DTemplateCode = item.TemplateCode;
            //                model.DInProgressCount = item.InProgressCount;
            //                model.DCompletedCount = item.CompletedCount;
            //                model.DRejectedCount = item.RejectedCount;
            //                model.DTotalCount = model.DInProgressCount + model.DCompletedCount + model.DRejectedCount;                                            
            //        }
            //    }
            //} 
            #endregion


            //var res = await _performanceManagementBusiness.ReadProjectTotalTaskData(ProjectId, templatecode);
            //if (res.IsNotNull())
            //{
            //    model.TaskCount = (res.AllTaskCount - res.CompletedCount).ToString();
            //    model.CompletedCount = res.CompletedCount.ToString();
            //    if (res.DueDate < DateTime.Now)
            //    {
            //        model.DayLeft = "0";
            //    }
            //    else
            //    {
            //        model.DayLeft = (res.DueDate.Date - DateTime.Now.Date).Days.ToString();
            //    }
            //    model.ProjectOwnerId = res.OwnerUserId;
            //    model.RequestedByUserId = res.RequestedByUserId;
            //}

            ViewBag.UserId = _userContext.UserId;
            model.TemplateCode = type;
            return View(model);
        }

        public IActionResult SelectCategoryMaster(string templateCode, string categoryCode)
        {
            var model = new TemplateViewModel();
            model.Code = templateCode;
            model.CategoryCode = categoryCode;
            return View(model);
        }
        public async Task<IActionResult> ReadPerformanceTaskCategory(string categoryCode)
        {
            //var result = new List<TemplateCategoryViewModel>();
            //if (categoryCode.IsNotNullAndNotEmpty())
            //{
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && x.Code == categoryCode);
            //}
            //else
            //{
            //    result = await _templateCategoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && x.TemplateCategoryType == TemplateCategoryTypeEnum.Standard);
            //}

            //result.Add(new TemplateCategoryViewModel {Name="Categroy1",Code="Categroy1" });
            //result.Add(new TemplateCategoryViewModel {Name="Categroy2",Code="Categroy2" });
            //result.Add(new TemplateCategoryViewModel {Name="Categroy3",Code="Categroy3" });

            var result = await _performanceManagementBusiness.GetPerformanceTaskCompetencyCategory();
            var j = Json(result);
            return j;
        }
        public async Task<IActionResult> ReadPerformanceTaskMaster(string templateCode, string categoryCode)
        {
            //var result = await _templateBusiness.GetTemplateList(templateCode, categoryCode);
            //var result = new List<TemplateViewModel>();
            //result.Add(new TemplateViewModel {Id="M00101",Code="Master0101",DisplayName="Master 0101",Description="Master Details 0101",CategoryCode= "Categroy1" });
            //result.Add(new TemplateViewModel {Id="M00102",Code="Master0102",DisplayName="Master 0102",Description="Master Details 0102", CategoryCode = "Categroy1" });
            //result.Add(new TemplateViewModel {Id="M00201",Code="Master0201",DisplayName="Master 0201",Description="Master Details 0101", CategoryCode = "Categroy2" });
            //result.Add(new TemplateViewModel {Id="M00202",Code="Master0202",DisplayName="Master 0202",Description="Master Details 0202", CategoryCode = "Categroy2" });
            //if (categoryCode.IsNotNullAndNotEmpty())
            //{
            //    result = result.Where(x => x.CategoryCode == categoryCode).ToList();
            //}
            var result = await _performanceManagementBusiness.GetPerformanceTaskCompetencyMaster(templateCode, categoryCode);
            result = result.OrderBy(x => x.CompetencyName).ToList();
            var j = Json(result);
            return j;
        }
        public async Task<IActionResult> ReadProjectTaskAssignedData([DataSourceRequest] DataSourceRequest request, string projectId, string type)
        {
            var isProjectManager = IsProjectManager();
            var templatecode = "";
            if (type == "Competency")
            {
                templatecode = "PMS_COMPENTENCY";
            }
            else if (type == "Development")
            {
                templatecode = "PMS_DEVELOPMENT";
            }
            else
            {
                templatecode = "PMS_GOAL";
            }

            var list = await _performanceManagementBusiness.ReadManagerProjectTaskAssignedData(projectId, templatecode);
            var j = Json(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;


        }

        public async Task<IActionResult> ReadProjectTaskOwnerData([DataSourceRequest] DataSourceRequest request, string projectId, string type)
        {
            var isProjectManager = IsProjectManager();
            var templatecode = "";
            if (type == "Competency")
            {
                templatecode = "PMS_COMPENTENCY";
            }
            else if (type == "Development")
            {
                templatecode = "PMS_DEVELOPMENT";
            }
            else
            {
                templatecode = "PMS_GOAL";
            }

            var list = await _performanceManagementBusiness.ReadManagerProjectTaskOwnerData(projectId, templatecode);
            var j = Json(list);
            // var j = Json(list.ToDataSourceResult(request));
            return j;


        }
        public async Task<ActionResult> ReadGoalServiceData([DataSourceRequest] DataSourceRequest request, string performanceId, string stageId, string performanceUser, string type, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var list = await _performanceManagementBusiness.ReadManagerPerformanceGoalViewData(performanceId, stageId, performanceUser);
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> ReadCompetencyServiceData([DataSourceRequest] DataSourceRequest request, string performanceId, string performanceUser, string stageId, string stage, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var list = await _performanceManagementBusiness.ReadPerformanceCompetencyStageViewData(performanceId, stageId, performanceUser);
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }

        public async Task<ActionResult> ReadDevelopmentServiceData([DataSourceRequest] DataSourceRequest request, string performanceId, string performanceUser, string stageId, string stage, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var list = await _performanceManagementBusiness.ReadPerformanceDevelopmentViewData(performanceId, stageId, performanceUser);
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }

        public async Task<ActionResult> ReadAllServiceData([DataSourceRequest] DataSourceRequest request, string performanceId, string performanceUser, string stageId, string stage, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var list = await _performanceManagementBusiness.ReadPerformanceAllData(performanceId, stageId, performanceUser);
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }

        public async Task<ActionResult> ReadPerformanceTaskView([DataSourceRequest] DataSourceRequest request, string Id, string performanceUser, string stage, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, string type = null)
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
            //var isProjectManager = IsProjectManager();

            //if (isProjectManager)
            //{
            //    var list = await _performanceManagementBusiness.ReadManagerProjectTaskViewData(Id,filteruser, filterstatus, ownerIds, column, fromDate, toDate);
            //    //var result=new List<TeamWorkloadViewModel>();
            //    //if (filteruser.Count>0)
            //    //{
            //    //    flag = true;
            //    //    foreach(var item in filteruser){
            //    //        res.AddRange(list.Where(x => x.UserId == item).ToList());
            //    //    }



            //    //}
            //    //if (filterstatus.Length > 0)
            //    //{
            //    //    flag = true;
            //    //    foreach (var item in filterstatus)
            //    //    {
            //    //        res.AddRange(list.Where(x => x.TaskStatusId == item).ToList());
            //    //    }
            //    //}
            //    //if (filterSdate.HasValue)
            //    //{
            //    //    flag = true;
            //    //    res.AddRange(list.Where(x => x.StartDate.Date == filterSdate.Value.Date).ToList());
            //    //}
            //    //if (filterEdate.HasValue)
            //    //{
            //    //    flag = true;
            //    //    res.AddRange(list.Where(x => x.StartDate.Date == filterEdate.Value.Date).ToList());
            //    //}
            //    //if (flag)
            //    //{
            //    //    return Json(res.ToDataSourceResult(request));
            //    //}

            //    return Json(list.ToDataSourceResult(request));
            //}
            //else
            //{
            var list = await _performanceManagementBusiness.ReadPerformanceTaskViewData(Id, performanceUser, filteruser, filterstatus, ownerIds, column, fromDate, toDate, type);

            return Json(list);
            // return Json(list.ToDataSourceResult(request));
            //}
        }

        public async Task<object> ReadSubTaskViewData(string taskId, string id, string status)
        {

            var list = await _performanceManagementBusiness.ReadProjectSubTaskViewData(taskId, id, status);
            var result = list.Select(x => new
            {
                id = x.TaskId,
                TaskName = x.TaskName,
                StartDate = x.StartDate,
                DueDate = x.DueDate,
                UserName = x.UserName,
                Priority = x.Priority,
                TaskStatus = x.TaskStatus,
                PhotoId = x.PhotoId,
                UserId = x.UserId,
                Code = x.Code,
                SubTaskCount = x.SubTaskCount,
                ProjectOwnerId = x.ProjectOwnerId,
                hasChildren = x.HasSubFolders,
                ParentId = x.parentId,
                ProjectId = x.ProjectId,
                TemplateCode = x.TemplateCode,
                TaskStatusCode = x.TaskStatusCode,
                expanded = true,
                key = x.TaskId,
                title = " ",
                lazy = true
            }).ToList();
           // var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
           // return json;
            var j = Json(result);
             return j;
        }

        public async Task<ActionResult> ReadPerformanceStageTask([DataSourceRequest] DataSourceRequest request, string performanceId, string stageId, string ownerUserId, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, string type = null)
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
            var data = await _performanceManagementBusiness.GetPerformanceDocumentStageDataByServiceId(performanceId, ownerUserId, stageId);
            IList<TeamWorkloadViewModel> list = new List<TeamWorkloadViewModel>();
            if (data.Count > 0)
            {
                var stageServiceId = data.FirstOrDefault().Id;
                list = await _performanceManagementBusiness.ReadPerformanceTaskViewData(stageServiceId, ownerUserId, filteruser, filterstatus, ownerIds, column, fromDate, toDate, type);
            }

            return Json(list);
            //return Json(list.ToDataSourceResult(request));
            //}
        }
        public ActionResult PerformanceCalendarView(string ProjectId, string stageId)
        {
            var isProjectManager = IsProjectManager();
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId,
                UserName = _userContext.Name,
                EmailId = _userContext.Email,
                PhotoId = _userContext.PhotoId,
                PerformanceStage = stageId,

            };
            return View("PerformanceCalendarView", model);
        }

        public async Task<ActionResult> ReadPerformanceCalendarData([DataSourceRequest] DataSourceRequest request, string projectId, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, string performanceStageId = null)
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
            //var isProjectManager = IsProjectManager();
            //if (isProjectManager)
            //{
            //    var list = await _performanceManagementBusiness.ReadManagerPerformanceCalendarViewData(projectId, filteruser, filterstatus, ownerIds, column, fromDate, toDate);
            //    return Json(list.ToDataSourceResult(request));
            //}
            //else
            //{
            var list = await _performanceManagementBusiness.ReadPerformanceCalendarViewData(projectId, filteruser, filterstatus, ownerIds, column, fromDate, toDate, performanceStageId);
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }

        //    public async Task<ActionResult> ReadPerformancePlanningTaskData(string projectId)
        //    {

        //        var result = await _performanceManagementBusiness.ReadMindMapData(projectId);
        //        result = result.Where(x => x.Type == "TASK" || x.Type == "SUBTASK").ToList();

        //        var events = new List<ProjectTimelineViewModel>();
        //        foreach (var i in result)
        //        {
        //            events.Add(new ProjectTimelineViewModel()
        //            {
        //                id = i.Id,
        //                title = i.Title + " - " + i.UserName,
        //                start = i.Start.ToString("yyyy-MM-dd"), //.ToString("yyyy-MM-dd"),//"2020-04-18"
        //                end = i.End.ToString("yyyy-MM-dd"), // .ToString("yyyy-MM-dd"), //"2020-04-18"
        //                allDay = false
        //            });
        //        }
        //        return Json(events.ToArray());
        //    }
        //  [HttpPost]
        //public async Task<ActionResult> UpdateTaskScheduleDate(TaskViewModel model)
        //{
        //    var task = await _taskBusiness.GetSingleById(model.Id);
        //    if (task != null)
        //    {
        //        task.StartDate = model.StartDate;
        //        task.DueDate = model.DueDate;
        //        if (model.DueDate.IsNotNull())
        //        {
        //            task.StartDate = model.StartDate;
        //            task.DueDate = model.DueDate;
        //        }
        //        else
        //        {
        //            task.StartDate = model.StartDate;
        //            task.DueDate = model.StartDate.Value.Add(model.TaskSLA);
        //        }
        //        await _taskBusiness.Edit(task);
        //    }
        //    return Json(new
        //    {
        //        success = true
        //    });
        //}

        public IActionResult ServiceListByFilters(string userId = null, string statusCodes = null, string templateCodes = null, string parentServiceId = null, string cbm = null)
        {
            ViewBag.CBM = cbm;
            return View(new ServiceTemplateViewModel
            {
                OwnerUserId = userId,
                TemplateCode = templateCodes,
                ServiceStatusCode = statusCodes,
                ParentServiceId = parentServiceId,
                PortalId = _userContext.PortalId,
            });
        }

        public async Task<IActionResult> ReadServiceList(string templateCodes = null, string moduleCodes = null, string catCodes = null, string requestBy = null, bool showAllOwnersService = false, string statusCodes = null, string parentServiceId = null)
        {
            var dt = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, catCodes, requestBy, showAllOwnersService, statusCodes, parentServiceId);
            return Json(dt);
        }
    }

   
}