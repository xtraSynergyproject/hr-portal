using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Pms.Controllers
{
    [Route("pms/performance")]
    [ApiController]
    public class PerformanceController : ApiController
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly IPerformanceManagementBusiness _pmtBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IServiceProvider _serviceProvider;
        public PerformanceController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            IPerformanceManagementBusiness performanceManagementBusiness,
            ITaskBusiness taskBusiness, IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness,
            IUserHierarchyBusiness userHierarchyBusiness) : base(serviceProvider)
        {
            _customUserManager = customUserManager;
            _serviceProvider = serviceProvider;
            _pmtBusiness = performanceManagementBusiness;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
        }

        [HttpGet]
        [Route("GetPerformanceStageIdNameList")]
        public async Task<ActionResult> GetPerformanceStageIdNameList(string userId,string performanceId,string ownerids,string typeList )
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            List<string> ownerIds = null;
            List<string> types = null;
            if (ownerids.IsNotNullAndNotEmpty())
            {
                ownerIds = ownerids.Split(',').ToList<string>();
            }
            if (typeList.IsNotNullAndNotEmpty())
            {
                types = typeList.Split(',').ToList<string>();
            }

            var viewModel = await _pmtBusiness.GetPerformanceStageIdNameList(_userContext.UserId, performanceId, ownerIds, types);
            return Ok(viewModel);
        }

        [HttpGet]
        [Route("GetPerformanceUserIdNameList")]
        public async Task<ActionResult> GetPerformanceUserIdNameList(string performanceId,string userId)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var viewModel = await _pmtBusiness.ReadPerformanceDashboardData(_userContext.UserId, performanceId, _userContext.UserRoleCodes);
            var list = viewModel.Select(x => new IdNameViewModel()
            {
                Id = x.UserId,
                Name = x.UserName

            }).GroupBy(x => x.Id).Select(p => p.FirstOrDefault()).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadPerformanceTaskGridViewData")]
        public async Task<IActionResult> ReadPerformanceTaskGridViewData(string performanceId, string userId ,string projectIds = null, string senderIds = null, string recieverids = null, string statusIds = null, FilterColumnEnum? column = null, DateTypeEnum? dateRange = null, DateTime? fromDate = null, DateTime? toDate = null, string type = null, string stageId = null, InboxTypeEnum? inboxType = null)
        {
            await Authenticate(userId, "PerformanceManagement");
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
            var list = await _pmtBusiness.ReadPerformanceDashboardTaskData(userId, performanceId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate, type, stageId);

            //var list = await _pmtBusiness.ReadPerformanceDashboardTaskData(userId, performanceId, _userContext.UserRoleCodes, projectIds, senderIds, recieverids, statusIds, column, fromDate, toDate, type, stageId);
            var j = Ok(list);
            return j;
        }

        [HttpGet]
        [Route("GetPerformanceObjectiveIdNameList")]
        public async Task<ActionResult> GetPerformanceObjectiveIdNameList(string performanceId,string userId)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var viewModel = await _pmtBusiness.GetPerformanceObjectiveList(_userContext.UserId, performanceId);
            return Ok(viewModel);
        }

        [HttpGet]
        [Route("GetPerformanceServiceChartByStatus")]
        public async Task<ActionResult> GetPerformanceServiceChartByStatus(string projectId, string servicetype, string stageId, string userId)
        {
            var viewModel = await _pmtBusiness.GetPerformanceServiceStatus(userId, projectId, servicetype, stageId);

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList()
            };
            return Ok(newlist);
        }

        [HttpGet]
        [Route("GetPerformanceTaskChartByType")]
        public async Task<ActionResult> GetPerformanceTaskChartByType(string performanceId, string stageId, string userId)
        {
            var viewModel = await _pmtBusiness.GetTaskType(userId, performanceId, stageId);
            //return Json(viewModel);

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList()
            };
            return Ok(newlist);
        }

        #region Performance Result

        [HttpGet]
        [Route("GetPerformanceDetails")]
        public async Task<ActionResult> GetPerformanceDetails(string masterId = null, string deptId = null, string userId = null, string stageId = null)
        {
            var dmaster = await _pmtBusiness.GetPDMDetails(masterId);
            if (dmaster.IsNotNull())
            {
                var list = await _pmtBusiness.GetPerformanceDocumentDetailsData(dmaster.UdfNoteId, null, null, stageId);
                if (deptId.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.DepartmentId == deptId).ToList();
                }
                if (userId.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.UserId == userId).ToList();
                }

                var j = Ok(list);
                return j;
            }
            return null;

        }



        #endregion

        #region Department Goal

        [HttpGet]
        [Route("GetPerformanceMasterByDepartment")]
        public async Task<IActionResult> GetPerformanceMasterByDepartment(string departmentId, string year)
        {
            var model = await _pmtBusiness.GetPerformanceMasterByDepatment(departmentId, year);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetDepartmentGoalByDepartment")]
        public async Task<IActionResult> GetDepartmentGoalByDepartment(string departmentId, string documentMasterId)
        {
            var model = await _pmtBusiness.GetDepartmentGoalByDepartment(departmentId, documentMasterId);
            return Ok(model);
        }


        [HttpGet]
        [Route("GetAllYearFromPerformanceMaster")]
        public async Task<IActionResult> GetAllYearFromPerformanceMaster(string departmentId)
        {
            var model = await _pmtBusiness.GetAllYearFromPerformanceMaster(departmentId);
            return Ok(model);
        }
        #endregion

    }
}
