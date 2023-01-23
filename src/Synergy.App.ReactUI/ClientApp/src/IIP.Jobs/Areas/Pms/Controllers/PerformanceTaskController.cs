using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities;

namespace Synergy.App.Api.Areas.Pms.Controllers
{
    [Route("pms/performanceTask")]
    [ApiController]
    public class PerformanceTaskController : ApiController
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness; 
        private readonly IPerformanceManagementBusiness _performanceManagementBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IServiceProvider _serviceProvider;
        public PerformanceTaskController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            IPerformanceManagementBusiness performanceManagementBusiness, 
            ITaskBusiness taskBusiness,IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness,
            IUserHierarchyBusiness userHierarchyBusiness) : base(serviceProvider)
        {
            _customUserManager = customUserManager;
            _serviceProvider = serviceProvider; 
            _performanceManagementBusiness = performanceManagementBusiness;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
        }


        #region Manage Performance

        [HttpGet]
        [Route("GetPerformanceUserList")]
        public async Task<ActionResult> GetPerformanceUserList(string userId)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var userList = new List<IdNameViewModel>();
            var subordinate = await _userHierarchyBusiness.GetHierarchyUsers("PERFORMANCE_HIERARCHY", _userContext.UserId, 1, 1);
            userList = subordinate.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            userList.Insert(0, new IdNameViewModel { Id = _userContext.UserId, Name = _userContext.Name });

            var j = Ok(userList);
            return j;
        }

        [HttpGet]
        [Route("GetPerformanceUserYearList")]
        public async Task<ActionResult> GetPerformanceUserYearList(string userId)
        {
            var userYearList = new List<IdNameViewModel>();
            userYearList = await _performanceManagementBusiness.GetYearByUserId(userId);
            // userYearList.Add(new IdNameViewModel { Id="py01",Name="Test 2021"});
            var j = Ok(userYearList);
            return j;
        }

        [HttpGet]
        [Route("ReadGoalServiceData")]
        public async Task<ActionResult> ReadGoalServiceData(string performanceId, string stageId, string performanceUser)//, string type, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var list = await _performanceManagementBusiness.ReadManagerPerformanceGoalViewData(performanceId, stageId, performanceUser);
            return Ok(list);
            //return Json(list.ToDataSourceResult(request));
        }

        [HttpGet]
        [Route("GetPerformanceList")]
        public async Task<IActionResult> GetPerformanceList(string userId,string ownerId)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var id = _userContext.UserId;
            if (ownerId.IsNotNullAndNotEmpty())
            {
                id = ownerId;
            }
            var list = await _performanceManagementBusiness.GetPerformanceDocumentList(id);
            var j = Ok(list);
            return j;
        }

        
        [HttpGet]
        [Route("ReadPerformanceTaskView")]
        public async Task<ActionResult> ReadPerformanceTaskView(string Id, string performanceUser,  FilterColumnEnum? column = null,string filterusers=null,string filterStatus=null,string ownerids=null,DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, string type = null)
        {
            List<string> filteruser = null;
            List<string> filterstatus = null;
            List<string> ownerIds = null;
            if (filterusers.IsNotNullAndNotEmpty())
            {
                filteruser = filterusers.Split(',').ToList<string>();
            }
            if (filterStatus.IsNotNullAndNotEmpty())
            {
                filterstatus = filterStatus.Split(',').ToList<string>();
            }
            if (ownerids.IsNotNullAndNotEmpty())
            {
                ownerIds = ownerids.Split(',').ToList<string>();
            }
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
            
            var list = await _performanceManagementBusiness.ReadPerformanceTaskViewData(Id, performanceUser, filteruser, filterstatus, ownerIds, column, fromDate, toDate, type);

            return Ok(list);
           
        }

        [HttpGet]
        [Route("PerformanceTaskView")]
        public async Task<ActionResult> PerformanceTaskView(string projectId, string performanceStage, string type, string performanceUser, string performanceUserYear)
        {
            var model = new TeamWorkloadViewModel();
            model.Id = projectId; model.TaskCount = "0";
            model.PerformanceUser = performanceUser;
            model.PerformanceUserYear = performanceUserYear;
            model.PerformanceStage = performanceStage;
            if (performanceStage.IsNotNullAndNotEmpty())
            {
                var data = await _performanceManagementBusiness.GetPerformanceDocumentStageDataByServiceId(projectId, performanceUser, performanceStage);
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

           
            var alllist = await _performanceManagementBusiness.ReadPerformanceAllData(projectId, performanceStage, performanceUser);
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

            var result = await _performanceManagementBusiness.GetTaskListByType("PMS_GOAL,PMS_COMPENTENCY,PMS_DEVELOPMENT", projectId, performanceUser, performanceStage);
            var stagelist = await _performanceManagementBusiness.GetStageTaskList("PMS_GOAL,PMS_COMPENTENCY,PMS_DEVELOPMENT", projectId, performanceUser, model.PerformanceStageServiceId);

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

            } 
            model.TemplateCode = type;
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadAllServiceData")]
        public async Task<ActionResult> ReadAllServiceData(string performanceId, string performanceUser, string stageId)
        {
            var list = await _performanceManagementBusiness.ReadPerformanceAllData(performanceId, stageId, performanceUser);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadCompetencyServiceData")]
        public async Task<ActionResult> ReadCompetencyServiceData( string performanceId, string performanceUser, string stageId)
        {
            var list = await _performanceManagementBusiness.ReadPerformanceCompetencyStageViewData(performanceId, stageId, performanceUser);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadDevelopmentServiceData")]
        public async Task<ActionResult> ReadDevelopmentServiceData(string performanceId, string performanceUser, string stageId, string stage,
            string filterusers = null, string filterStatus = null, string ownerids = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null,
            DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<string> filteruser = null;
            List<string> filterstatus = null;
            List<string> ownerIds = null;
            if (filterusers.IsNotNullAndNotEmpty())
            {
                filteruser = filterusers.Split(',').ToList<string>();
            }
            if (filterStatus.IsNotNullAndNotEmpty())
            {
                filterstatus = filterStatus.Split(',').ToList<string>();
            }
            if (ownerids.IsNotNullAndNotEmpty())
            {
                ownerIds = ownerids.Split(',').ToList<string>();
            }
            var list = await _performanceManagementBusiness.ReadPerformanceDevelopmentViewData(performanceId, stageId, performanceUser);
            return Ok(list);
        }
        #endregion

        #region Performance Calendar


        [HttpGet]
        [Route("ReadPerformanceCalendarData")]
        public async Task<ActionResult> ReadPerformanceCalendarData( string projectId, string filterusers = null, string filterStatus = null, string ownerids = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, string performanceStageId = null)
        {
            List<string> filteruser = null;
            List<string> filterstatus = null;
            List<string> ownerIds = null;
            if (filterusers.IsNotNullAndNotEmpty())
            {
                filteruser = filterusers.Split(',').ToList<string>();
            }
            if (filterStatus.IsNotNullAndNotEmpty())
            {
                filterstatus = filterStatus.Split(',').ToList<string>();
            }
            if (ownerids.IsNotNullAndNotEmpty())
            {
                ownerIds = ownerids.Split(',').ToList<string>();
            }

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
            var list = await _performanceManagementBusiness.ReadPerformanceCalendarViewData(projectId, filteruser, filterstatus, ownerIds, column, fromDate, toDate, performanceStageId);
            return Ok(list);
           
        }

        #endregion

        #region Subordinate Objective

        [HttpGet]
        [Route("ReadPerformanceObjectivesGridViewData")]
        public async Task<IActionResult> ReadPerformanceObjectivesGridViewData(string ownerUserId, string userId,string pmsTypeList=null,string stageIdList=null, string projectIdList = null, string senderIdList = null,string statusIdList=null, string receiverIdList = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null,  string statusCodes = null)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            List<string> projectIds = null;
            List<string> senderIds = null;
            List<string> recieverids = null;
            List<string> statusIds = null;
            List<string> pmsTypes = null;
            List<string> stageIds = null;
            if (projectIdList.IsNotNullAndNotEmpty())
            {
                 projectIds = projectIdList.Split(',').ToList<string>();
            }
            if (senderIdList.IsNotNullAndNotEmpty())
            {
                 senderIds = senderIdList.Split(',').ToList<string>();
            }
            if (receiverIdList.IsNotNullAndNotEmpty())
            {
                recieverids = receiverIdList.Split(',').ToList<string>();
            }
            if (statusIdList.IsNotNullAndNotEmpty())
            {
                 statusIds = statusIdList.Split(',').ToList<string>();
            }
            if (pmsTypeList.IsNotNullAndNotEmpty())
            {
                 pmsTypes = pmsTypeList.Split(',').ToList<string>();
            }
            if (stageIdList.IsNotNullAndNotEmpty())
            {
                 stageIds = stageIdList.Split(',').ToList<string>();
            }
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
            if (ownerUserId.IsNotNullAndNotEmpty())//param name userid is changed to owner userid because in mobile userid in madetory to send 
            {
                list = list.Where(x => x.OwnerUserId == ownerUserId).ToList();
            }
            var j = Ok(list);
            //var j = Json(list.ToDataSourceResult(request));
            return j;
        }


        [HttpGet]
        [Route("GetSubordinatesIdNameList")]
        public async Task<ActionResult> GetSubordinatesIdNameList(string userId)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var list = await _performanceManagementBusiness.GetSubordinatesIdNameList();
            return Ok(list);
        }

        #endregion


        #region

        [HttpGet]
        [Route("ReadPerformanceUserWorkloadGridViewData")]
        public async Task<IActionResult> ReadPerformanceUserWorkloadGridViewData(string userId,string projectIdList = null, string senderIdList = null, string statusIdList = null, string receiverIdList = null, string pmsTypeList = null, string stageIdList = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            List<string> projectIds = null;
            List<string> senderIds = null;
            List<string> recieverids = null;
            List<string> statusIds = null;
            List<string> pmsTypes = null;
            List<string> stageIds = null;
            if (projectIdList.IsNotNullAndNotEmpty())
            {
                projectIds = projectIdList.Split(',').ToList<string>();
            }
            if (senderIdList.IsNotNullAndNotEmpty())
            {
                senderIds = senderIdList.Split(',').ToList<string>();
            }
            if (receiverIdList.IsNotNullAndNotEmpty())
            {
                recieverids = receiverIdList.Split(',').ToList<string>();
            }
            if (statusIdList.IsNotNullAndNotEmpty())
            {
                statusIds = statusIdList.Split(',').ToList<string>();
            }
            if (pmsTypeList.IsNotNullAndNotEmpty())
            {
                pmsTypes = pmsTypeList.Split(',').ToList<string>();
            }
            if (stageIdList.IsNotNullAndNotEmpty())
            {
                stageIds = stageIdList.Split(',').ToList<string>();
            }
           
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
            var j = Ok(list);
            return j;
        }


        #endregion


        #region Performance Result 

        [HttpGet]
        [Route("GetPDMList")]
        public async Task<ActionResult> GetPDMList(string userId,string ownerId = null, string year = null)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var list = await _performanceManagementBusiness.GetPDMList(year);
            return Ok(list);
        }

        #endregion
    }
}
