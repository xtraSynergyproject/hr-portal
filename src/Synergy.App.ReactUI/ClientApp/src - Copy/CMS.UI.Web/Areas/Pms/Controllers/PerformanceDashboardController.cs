using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pms.Controllers
{
    [Area("Pms")]
    public class PerformanceDashboardController : ApplicationController
    {
        private readonly IPerformanceManagementBusiness _performanceManagementBusiness;
        private readonly IUserContext _userContext;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;

        public PerformanceDashboardController(IPerformanceManagementBusiness performanceManagementBusiness, IUserContext userContext,
            ITaskBusiness taskBusiness,
                                        IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness)
        {
            _performanceManagementBusiness = performanceManagementBusiness;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
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
        public async Task<ActionResult> GetProjectsList()
        {            
            var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager();

            var projectList = await _performanceManagementBusiness.GetPerformanceList(userId, isProjectManager);
            var j = Json(projectList);
            return j;
        }
        public async Task<ActionResult> Index(string pageName, string ProjectId)
        {           
            var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager();

            var projectList = await _performanceManagementBusiness.GetPerformanceList(userId, isProjectManager);
            if(pageName == "PerformanceResult")
            {
                projectList = await _performanceManagementBusiness.GetPDMList();
            }
            //if (projectList != null && projectList.Count()>0)
            //{
            //    ProjectId = projectList.FirstOrDefault().Id;
            //}
            ServiceViewModel model = new ServiceViewModel()
            {
                Id = ProjectId,
                PageName = pageName
            };
            //var model = await _projectManagementBusiness.GetProjectDetails(ProjectId);
            //model.PageName = pageName;
            
            return View(model);
        }
        public async Task<ActionResult> PerformanceDetailsBanner(string ProjectId,string PerformanceStage, string pageName,string PerformanceUser,string PerformanceUserYear,string type,string departmentId=null)
        {
            var model = new ServiceViewModel();
               
            if(pageName == "PerformanceResult")
            {
                model = await _performanceManagementBusiness.GetPDMDetails(ProjectId);
            }
            else if (ProjectId.IsNotNullAndNotEmpty())
            {
                model = await _performanceManagementBusiness.GetPerformanceDetails(ProjectId);
              //  model.SubStages = await _performanceManagementBusiness.ReadPerformanceDocumentStagesData(ProjectId);
                var data = await _performanceManagementBusiness.GetPerformanceDocumentStageDataByServiceId(ProjectId, PerformanceUser);
                if (data.Count > 0)
                {
                    model.PerformanceStage = data.FirstOrDefault().StageId;
                }
            }
            if (PerformanceUser.IsNullOrEmpty() )
            {
                PerformanceUser = _userContext.UserId;

            }

            if (PerformanceUserYear.IsNullOrEmpty())
            {
                PerformanceUserYear= DateTime.Now.Year.ToString();                      
            }
            if (ProjectId.IsNullOrEmpty())
            {
                var projectList = await _performanceManagementBusiness.GetPerformanceList(PerformanceUser, false, PerformanceUserYear);
               if(projectList.IsNotNull() && projectList.Count > 0)
                {
                    ProjectId = projectList.FirstOrDefault().Id;
                    model = await _performanceManagementBusiness.GetPerformanceDetails(ProjectId);
                    var data = await _performanceManagementBusiness.GetPerformanceDocumentStageDataByServiceId(ProjectId, PerformanceUser);
                    if (data.Count > 0)
                    {
                        PerformanceStage = data.FirstOrDefault().StageId;
                    }
                  //  model.SubStages = await _performanceManagementBusiness.ReadPerformanceDocumentStagesData(ProjectId);
                }              
                //model.ServiceType = PerformanceStageEnum.All.ToString();
                //var userYearList = await _performanceManagementBusiness.GetYearByUserId(PerformanceUser);
                //var year = DateTime.Now.Year.ToString();
                //var list = userYearList.Where(x => x.Id == year).ToList();
                //if (list != null)
                //{
                //    PerformanceUserYear = year;
                  
                //}
            }

            model.Id = ProjectId;
            model.PageName = pageName;
            model.PerformanceUser = PerformanceUser;
            model.PerformanceUserYear = PerformanceUserYear;
            model.DepartmentId = departmentId;
            model.PerformanceStage = PerformanceStage;
            model.ServiceType = type;
            return View("_PerformanceDetailsBanner", model);
        }

        public async Task<ActionResult> GetPerformanceStages(string performanceId, string ownerUserId)
        {
            var data = await _performanceManagementBusiness.GetPerformanceDocumentStageDataByServiceId(performanceId,ownerUserId);
            return Json(data);
        }

        public async Task<ActionResult> PerformanceDashboard(string PerformanceId, string stageId = null,string performanceUserId=null)
        {
            var projectlist = await _performanceManagementBusiness.GetPerformanceSharedList(_userContext.UserId);
            if (PerformanceId.IsNullOrEmpty() && projectlist != null && projectlist.Count > 0)
            {
                PerformanceId = projectlist.FirstOrDefault().Id;
            }

            //var model = await _performanceManagementBusiness.GetPerformanceDashboardDetails(PerformanceId, stageId);
            var model = await _performanceManagementBusiness.GetPerformanceDashboardDetails(PerformanceId, null);
            //if(stageId.IsNotNullAndNotEmpty())
            //{
            //    model= model.wher
            //}
            if (model != null)
            {
                model.ProjectList = projectlist.ToList();
                model.UserId = _userContext.UserId;
                model.StageId = stageId;
            }
            if (stageId.IsNotNullAndNotEmpty())
            {
                ViewBag.StageId = stageId;
            }
            if (performanceUserId.IsNotNullAndNotEmpty())
            {
                ViewBag.PerformanceUserId = performanceUserId;
            }
            return View(model);
        }
       
        
        public ActionResult ProjectTaskByName(string ProjectId)
        {            
            var isProjectManager = IsProjectManager();
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId,
                IsAssignee = isProjectManager ? true : false
            };
            return View(model);
        }

        public ActionResult ProjectTaskCalendarView(string ProjectId)
        {
            var isProjectManager = IsProjectManager();
            TeamWorkloadViewModel model = new TeamWorkloadViewModel()
            {
                Id = ProjectId,
                UserName = _userContext.Name,
                EmailId = _userContext.Email,
                PhotoId = _userContext.PhotoId             
                
            };
            return View("ProjectTaskCalendarView", model);
        }

        public async Task<IActionResult> ReadProjectUserWorkloadGridViewData([DataSourceRequest] DataSourceRequest request, List<string> projectIds=null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column=null, DateTypeEnum? dateRange =null, DateTime? fromDate = null,DateTime? toDate = null)
        {
            if(dateRange.IsNotNull()&& dateRange == DateTypeEnum.Today)
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
            else if(dateRange.IsNotNull() && dateRange == DateTypeEnum.Between)
            {
                toDate = toDate.Value.AddDays(1);
            }
            var list = await _performanceManagementBusiness.ReadPerformanceUserWorkloadGridViewData(_userContext.UserId, projectIds, senderIds, recieverids, statusIds,column, fromDate, toDate);
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<ActionResult> ReadProjectTeamData([DataSourceRequest] DataSourceRequest request, string projectId)
        {            
            var isProjectManager = IsProjectManager();
            var userId = _userContext.UserId;
            var list = await _performanceManagementBusiness.ReadProjectTeamWorkloadData(projectId, userId, isProjectManager);
             
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> ReadProjectTeamDataByUser([DataSourceRequest] DataSourceRequest request, string userId, string projectId)
        {
            var tasklist = await _performanceManagementBusiness.ReadProjectTeamDataByUser(projectId, userId);            
            var j = Json(tasklist.ToDataSourceResult(request));
            return j;
        }        

        public async Task<ActionResult> ReadSubTaskData(string id, string taskId)
        {            
            if(id!=null)
            {
                taskId = id;
            }

            var list = await _performanceManagementBusiness.ReadProjectSubTaskViewData(taskId,null,null);
            foreach(var item in list)
            {
                var sub = await _taskBusiness.GetList(x => x.ParentTaskId == item.Id);
                if(sub !=null)
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
            var j = Json(list.ToDataSourceResult(request));
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

            var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<ActionResult> ReadProjectTeamDataByDate([DataSourceRequest] DataSourceRequest request, string projectId, DateTime startDate)
        {
            var isProjectManager = IsProjectManager();         
            
            var tasklist = await _performanceManagementBusiness.ReadProjectTeamDataByDate(projectId, startDate, isProjectManager);

            var j = Json(tasklist.ToDataSourceResult(request));
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

    
        public async Task<ActionResult> ProjectTaskView(string ProjectId)
        {
            var model = new TeamWorkloadViewModel();
            model.Id = ProjectId; model.TaskCount = "0";
            model.CompletedCount = "0";
            model.DayLeft = "0";
        var res = await _performanceManagementBusiness.ReadProjectTotalTaskData(ProjectId,null);
            if (res.IsNotNull())
            {
                model.TaskCount = (res.AllTaskCount - res.CompletedCount).ToString();
                model.CompletedCount = res.CompletedCount.ToString();
                if (res.DueDate < DateTime.Now)
                {
                    model.DayLeft = "0";
                }
                else
                {
                    model.DayLeft = (res.DueDate.Date - DateTime.Now.Date).Days.ToString();
                }

            }
            
            ViewBag.UserId = _userContext.UserId;
           
          
            return View(model);
        }
    
      
        public async Task<IActionResult> ReadProjectTaskAssignedData([DataSourceRequest] DataSourceRequest request,string projectId)
        {
            var isProjectManager = IsProjectManager();
            if (isProjectManager)
            {
                var list = await _performanceManagementBusiness.ReadManagerProjectTaskAssignedData(projectId,null);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }
            else
            {
                var list = await _performanceManagementBusiness.ReadProjectTaskAssignedData(projectId,null);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }
           
        }

        public async Task<IActionResult> ReadProjectTaskOwnerData([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            var isProjectManager = IsProjectManager();
            if (isProjectManager)
            {
                var list = await _performanceManagementBusiness.ReadManagerProjectTaskOwnerData(projectId,null);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }
            else
            {
                var list = await _performanceManagementBusiness.ReadProjectTaskOwnerData(projectId,null);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }

        }
        public async Task<ActionResult> ReadProjectTaskData([DataSourceRequest] DataSourceRequest request, string projectId, string stage, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {

            var isProjectManager = IsProjectManager();
            if (isProjectManager|| stage=="Show")
            {
                var list = await _performanceManagementBusiness.ReadManagerPerformanceGoalViewData(projectId,null,null);
                return Json(list.ToDataSourceResult(request));
            }
            else
            {
                var list = await _performanceManagementBusiness.ReadProjectStageViewData(projectId);
                return Json(list.ToDataSourceResult(request));
            }

          
        }

       

        public async Task<ActionResult> ReadSubTaskViewData(string taskId)
        {
          
            var list = await _performanceManagementBusiness.ReadProjectSubTaskViewData(taskId,null,null);
            var j = Json(list);
            return j;
        }
        public ActionResult PerformanceSummary()
        {
            var model = new PerformanceDashboardViewModel();
            return View(model);
        }
        public ActionResult PerformanceSummaryDetail(string subject, string status,string filter)
        {
            var model = new PerformanceDashboardViewModel { Subject=subject, Status = status,FilterVal =filter};
            return View(model);
        }
        public async Task<IActionResult> ReadPerformanceSummaryData(string filter)
        {
           
            var list = await _performanceManagementBusiness.GetPerformanceSummaryData(filter);
            var j = Json(list);
            return j;
        }
        public async Task<IActionResult> ReadPerformanceSummaryDetail(string filter,string status,string serviceId)
        {

            var list = await _performanceManagementBusiness.GetPerformanceSummaryDetail(filter, status, serviceId);
            var j = Json(list);
            return j;
        }

        //public async Task<ActionResult> ReadProjectTaskCalendarData([DataSourceRequest] DataSourceRequest request, string projectId, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
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
        //    if (isProjectManager)
        //    {
        //        var list = await _performanceManagementBusiness.ReadManagerProjectCalendarViewData(projectId, filteruser,filterstatus,ownerIds,column,fromDate,toDate);
        //        return Json(list.ToDataSourceResult(request));
        //    }
        //    else
        //    {
        //        var list = await _performanceManagementBusiness.ReadProjectCalendarViewData(projectId, filteruser, filterstatus, ownerIds, column, fromDate, toDate);
        //        return Json(list.ToDataSourceResult(request));
        //    }


        //}


        public async Task<ActionResult> PerformanceResult(string pageName, string ProjectId,string departmentId = null)
        {    
            var projectList = await _performanceManagementBusiness.GetPDMList();
            
            if (ProjectId.IsNullOrEmpty() && projectList != null && projectList.Count() > 0)
            {
                ProjectId = projectList.FirstOrDefault().Id;
            }
            ServiceViewModel model = new ServiceViewModel()
            {
                Id = ProjectId,
                PageName = pageName,
                DepartmentId = departmentId
            };            

            return View(model);
        }

        public async Task<ActionResult> GetPerformanceChart(string pdmId,string deptId = null)
        {
            //var pdmaster = await _performanceManagementBusiness.GetPDMDetails(pdmId);
            var viewModel = await _performanceManagementBusiness.GetPerformanceFinalReport(pdmId, deptId);
            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = viewModel.GroupBy(x => x.RatingCode).Select(group => new { Value = group.Count(), Type = group.Key }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value }).ToList();
            
            return Json(list);
        }
    }
}