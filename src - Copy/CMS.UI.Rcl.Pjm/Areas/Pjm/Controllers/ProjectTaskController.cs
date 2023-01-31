using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectTaskController : ApplicationController
    {
        private readonly IProjectManagementBusiness _projectManagementBusiness;
        private readonly IUserContext _userContext;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INtsTaskTimeEntryBusiness _ntsTaskTimeEntryBusiness;
        private readonly IUserBusiness _userBusiness;


        public ProjectTaskController(IProjectManagementBusiness projectManagementBusiness, IUserContext userContext,
            ITaskBusiness taskBusiness,
                                        IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness,
                                        INtsTaskTimeEntryBusiness ntsTaskTimeEntryBusiness
                                        ,IUserBusiness userBusiness
            )
        {
            _projectManagementBusiness = projectManagementBusiness;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _ntsTaskTimeEntryBusiness = ntsTaskTimeEntryBusiness;
            _userBusiness = userBusiness;
        }

        public bool IsProjectManager()
        {
            var isProjectManager = false;
            
            var userRole = _userContext.UserRoleCodes.IsNotNull() ? _userContext.UserRoleCodes.Contains("PROJECT_MANAGER"):false;
            if (userRole)
            {
                isProjectManager = true;
            }
            return isProjectManager;
        }
        public async Task<ActionResult> GetProjectsList()
        {
            var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager();
            var projectList = await _projectManagementBusiness.GetProjectsList(userId, isProjectManager);
            var j = Json(projectList);
            return j;
        }
        public async Task<ActionResult> GetBannerProjectsList(string Id)
        {            
            var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager();
            var projectList = new List<IdNameViewModel>();
            if (Id.IsNullOrEmpty())
            {
                var list = await _projectManagementBusiness.GetProjectsLevel1(userId, isProjectManager);
                return Json(list);
            }
            else
            {
                var str = Id.Split("#");
                if(str[0]=="LEVEL2")
                {
                    var list = await _projectManagementBusiness.GetProjectsLevel2(userId, isProjectManager,str[1]);
                    return Json(list);
                }
                else if (str[0] == "LEVEL3")
                {
                    var list = await _projectManagementBusiness.GetProjectsLevel3(userId, isProjectManager, str[1],str[2]);
                    return Json(list);
                }

            }

            var j = Json(projectList);
            return j;
        }
        public async Task<ActionResult> ReadTaskWorkTimeDetails(string serviceId,string TimeLogDate, string TimeLogUserId)
        {
            //DateTime today = DateTime.Now;
            DateTime today = ApplicationConstant.DateAndTime.MinDate;
            if (TimeLogDate.IsNotNullAndNotEmpty() && TimeLogDate!="null") 
            {
                today = Convert.ToDateTime(TimeLogDate);
            }
            if (_userContext.UserRoleCodes.Contains("PROJECT_USER"))
            {
                TimeLogUserId = _userContext.UserId;
            }
            var list = await _ntsTaskTimeEntryBusiness.GetTimeEntriesData(serviceId, today,TimeLogUserId);
            

            return Json(list) ;
        }
        public async Task<ActionResult> GetProjectsTasksList(string projectId)
        {
            var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager();

            var list = await _projectManagementBusiness.ReadProjectTask(userId, projectId, isProjectManager, null, null, null, null, null, null, null);
            var j = Json(list);
            return j;
        }
        public async Task<ActionResult> Index(string pageName, string ProjectId)
        {           
            var userId = _userContext.UserId;
            var isProjectManager = IsProjectManager();

            if (ProjectId.IsNullOrEmpty())
            {
                var projectList = await _projectManagementBusiness.GetProjectsList(userId, isProjectManager);
                if (projectList != null && projectList.Count()>0)
                {
                    ProjectId = projectList.FirstOrDefault().Id;
                }
            }            
            ServiceViewModel model = new ServiceViewModel()
            {
                Id = ProjectId,
                PageName = pageName
            };
            //var model = await _projectManagementBusiness.GetProjectDetails(ProjectId);
            //model.PageName = pageName;
            
            return View(model);
        }
        public async Task<ActionResult> ProjectDetailsBanner(string ProjectId,string pageName)
        {
            var model = await _projectManagementBusiness.GetProjectDetails(ProjectId);
            if (model==null)
            {
                model = new ServiceViewModel();
            }
            model.Id = ProjectId;
            model.PageName = pageName;
           
            return View("_ProjectDetailsBanner",model);
        }
        public async Task<ActionResult> ProjectDetailsFilter(bool hideProject=false,bool stage=false)
        {
            ViewBag.HideProject = hideProject;
            ViewBag.Stage = stage;
            return View("_ProjectDetailsFilter");
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

        public async Task<IActionResult> ReadProjectUserWorkloadGridViewData([DataSourceRequest] DataSourceRequest request, List<string> projectIds =null, List<string> senderIds = null, List<string> recieverids = null, List<string> statusIds = null, FilterColumnEnum? column=null, DateTypeEnum? dateRange =null, DateTime? fromDate = null,DateTime? toDate = null)
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
            var list = await _projectManagementBusiness.ReadProjectUserWorkloadGridViewData(_userContext.UserId, projectIds, senderIds, recieverids, statusIds,column, fromDate, toDate);
          //  var j = Json(list);
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<ActionResult> ReadProjectTeamData([DataSourceRequest] DataSourceRequest request, string projectId)
        {            
            var isProjectManager = IsProjectManager();
            var userId = _userContext.UserId;
            var list = await _projectManagementBusiness.ReadProjectTeamWorkloadData(projectId, userId, isProjectManager);
             
            var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> ReadProjectTeamDataByUser([DataSourceRequest] DataSourceRequest request, string userId, string projectId)
        {
            var tasklist = await _projectManagementBusiness.ReadProjectTeamDataByUser(projectId, userId);            
            var j = Json(tasklist.ToDataSourceResult(request));
            return j;
        }        

        public async Task<ActionResult> ReadSubTaskData(string id, string taskId)
        {            
            if(id!=null)
            {
                taskId = id;
            }

            var list = await _projectManagementBusiness.ReadProjectSubTaskViewData(taskId);
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

            var isProjectManager = IsProjectManager();
            var userId = _userContext.UserId;
            var list = await _projectManagementBusiness.ReadProjectTask(userId, projectId, isProjectManager, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate);
            var j = Json(list);
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

            var list = await _projectManagementBusiness.ReadProjectTeamDateData(projectId);            

            var j = Json(list.ToDataSourceResult(request));
            return j;
        }
        public async Task<ActionResult> ReadProjectTeamDataByDate([DataSourceRequest] DataSourceRequest request, string projectId, DateTime startDate)
        {
            var isProjectManager = IsProjectManager();         
            
            var tasklist = await _projectManagementBusiness.ReadProjectTeamDataByDate(projectId, startDate, isProjectManager);

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
        var res = await _projectManagementBusiness.ReadProjectTotalTaskData(ProjectId);
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
                var list = await _projectManagementBusiness.ReadManagerProjectTaskAssignedData(projectId);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectTaskAssignedData(projectId);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }
           
        }

        public async Task<IActionResult> ReadProjectTaskOwnerData([DataSourceRequest] DataSourceRequest request, string projectId)
        {
            var isProjectManager = IsProjectManager();
            if (isProjectManager)
            {
                var list = await _projectManagementBusiness.ReadManagerProjectTaskOwnerData(projectId);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectTaskOwnerData(projectId);

                var j = Json(list.ToDataSourceResult(request));
                return j;
            }

        }
        public async Task<ActionResult> ReadProjectTaskData([DataSourceRequest] DataSourceRequest request, string projectId, string stage, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            IList<TeamWorkloadViewModel> model = new List<TeamWorkloadViewModel>();
            var isProjectManager = IsProjectManager();
            if (isProjectManager|| stage=="Show")
            {
                var list = await _projectManagementBusiness.ReadManagerProjectStageViewData(projectId);
                foreach (var data in list) 
                {
                    if (data.parentId.IsNullOrEmpty())
                    {
                        data.Sequence = 1;
                        model.Add(data);
                        model=GetSequence(data.id, list.ToList(), model.ToList(), data.Sequence);
                        break;
                    }
                   

                }
                return Json(model.ToDataSourceResult(request));
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
                return Json(model.ToDataSourceResult(request));
            }

          
        }
        public List<TeamWorkloadViewModel> GetSequence(string parentId, List<TeamWorkloadViewModel> oldList, List<TeamWorkloadViewModel> newList,long Sequence)
        {
            foreach (var data in oldList.Where(x=>x.parentId== parentId))
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

        public async Task<ActionResult> ReadProjectTaskView([DataSourceRequest] DataSourceRequest request, string Id,string stage,List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
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
           // var res = new List<TeamWorkloadViewModel>();
          //  bool flag=false;
            if (isProjectManager)
            {
                var list = await _projectManagementBusiness.ReadManagerProjectTaskViewData(Id,filteruser, filterstatus, ownerIds, column, fromDate, toDate);
                //var result=new List<TeamWorkloadViewModel>();
                //if (filteruser.Count>0)
                //{
                //    flag = true;
                //    foreach(var item in filteruser){
                //        res.AddRange(list.Where(x => x.UserId == item).ToList());
                //    }
                  
                       
                    
                //}
                //if (filterstatus.Length > 0)
                //{
                //    flag = true;
                //    foreach (var item in filterstatus)
                //    {
                //        res.AddRange(list.Where(x => x.TaskStatusId == item).ToList());
                //    }
                //}
                //if (filterSdate.HasValue)
                //{
                //    flag = true;
                //    res.AddRange(list.Where(x => x.StartDate.Date == filterSdate.Value.Date).ToList());
                //}
                //if (filterEdate.HasValue)
                //{
                //    flag = true;
                //    res.AddRange(list.Where(x => x.StartDate.Date == filterEdate.Value.Date).ToList());
                //}
                //if (flag)
                //{
                //    return Json(res.ToDataSourceResult(request));
                //}

                return Json(list.ToDataSourceResult(request));
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectTaskViewData(Id, filteruser, filterstatus, ownerIds, column, fromDate, toDate);
                //if (filteruser.Count > 0)
                //{
                //    flag = true;
                //    foreach (var item in filteruser)
                //    {
                //        res.AddRange(list.Where(x => x.UserId == item).ToList());
                //    }

                //}
                //if (filterstatus.Count > 0)
                //{
                //    flag = true;
                //    foreach (var item in filterstatus)
                //    {
                //        res.AddRange(list.Where(x => x.TaskStatusId == item).ToList());
                //    }
                //}
                //if (flag)
                //{
                //    return Json(res.ToDataSourceResult(request));
                //}
                return Json(list.ToDataSourceResult(request));
            }
        }

        public async Task<ActionResult> ReadSubTaskViewData(string taskId, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null)
        {
          
            var list = await _projectManagementBusiness.ReadProjectSubTaskViewData(taskId, filteruser, filterstatus, ownerIds);
            var j = Json(list);
            return j;
        }

        public async Task<ActionResult> ReadProjectTaskCalendarData([DataSourceRequest] DataSourceRequest request, string projectId, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
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
            if (isProjectManager)
            {
                var list = await _projectManagementBusiness.ReadManagerProjectCalendarViewData(projectId, filteruser,filterstatus,ownerIds,column,fromDate,toDate);
                return Json(list.ToDataSourceResult(request));
            }
            else
            {
                var list = await _projectManagementBusiness.ReadProjectCalendarViewData(projectId, filteruser, filterstatus, ownerIds, column, fromDate, toDate);
                return Json(list.ToDataSourceResult(request));
            }


        }
        [HttpGet]
        public async Task<ActionResult> GetServiceSequenceOrder(string parentId)
        {
            var list = await _serviceBusiness.GetList(x => x.ParentServiceId == parentId);
            var count = list.Max(x=>x.SequenceOrder);
            count = count ?? 0;
            return Json(new { count=count+1});
        }

        [HttpGet]
        public async Task<ActionResult> GetTaskSequenceOrder(string parentId)
        {
            var list = await _taskBusiness.GetList(x => x.ParentServiceId == parentId);
            var count = list.Max(x => x.SequenceOrder);
            count = count ?? 0;
            return Json(new { count = count+1});
        }
        [HttpGet]
        public async Task<ActionResult> GetSubTaskSequenceOrder(string parentId)
        {
            var list = await _taskBusiness.GetList(x => x.ParentTaskId == parentId);
            var count = list.Max(x => x.SequenceOrder);
            count = count ?? 0;
            return Json(new { count = count + 1 });
        }
        public async Task<ActionResult> TimeEntries(string projectId)
        {            
            var projectlist =await  _projectManagementBusiness.GetProjectSharedList(_userContext.UserId);

            if (projectId.IsNullOrEmpty() && projectlist != null && projectlist.Count > 0)
            {
                projectId = projectlist.FirstOrDefault().Id;
            }
            var model = await _projectManagementBusiness.GetProjectDetails(projectId);
            if (model != null)
            {
               // model.ProjectList = projectlist;
                var ViewModel =await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel { ServiceId = projectId, ActiveUserId= _userContext.UserId });
              //  var service =await _projectManagementBusiness.GetProjectManagementService(projectId);
                //if (service != null)
                //{
                //    model.ServiceStepServiceId = service.Id;
                //}
                model.Id = ViewModel.ServiceId;
                model.ServiceSubject = ViewModel.ServiceSubject;
                model.TemplateId = ViewModel.TemplateId;
                //if (service != null)
                //{
                //    model.ServiceStepServiceId = service.Id;
                //}
                ViewBag.UserId = _userContext.UserId;
                ViewBag.UserName = _userContext.Name;
              
                return View(model);
            }
            else
            {
                var newmodel = new ProjectDashboardViewModel();
                return View(newmodel);
            }
        }
        public async Task<IActionResult> AddTaskTimeEntry(string id,string taskId, string assignTo, string dataAction, string serviceId)
        {
            var model = new TaskTimeEntryViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                var data = await _ntsTaskTimeEntryBusiness.GetSingleById(id);
                model = data;
                model.DataAction = DataActionEnum.Edit;
            }
            else 
            {
                
                model.DataAction = DataActionEnum.Create;
                model.NtsTaskId = taskId;
                model.NtsServiceId = serviceId;
                model.UserId = assignTo;
                model.StartDate = System.DateTime.Now;
                model.EndDate = model.StartDate.AddDays(1);
                model.Duration = (model.EndDate - model.StartDate);
            }
            
            return View("_NtsTaskTimeEntry", model);
        }
        public async Task<IActionResult> DeleteTimeEntry(string Id)
        {
            await _ntsTaskTimeEntryBusiness.Delete(Id);
            return Json(true);
        }
        public async Task<ActionResult> GetSubordinatesUserIdNameList()
        {
            var list = await _projectManagementBusiness.GetSubordinatesUserIdNameList();
            return Json(list);
        }
        public async Task<string> GetNewGuid()
        {
            var newid = Guid.NewGuid().ToString();
            return newid;
        }
    }
}