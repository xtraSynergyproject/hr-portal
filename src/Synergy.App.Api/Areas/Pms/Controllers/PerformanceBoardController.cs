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
    [Route("pms/PerformanceBoard")]
    [ApiController]
    public class PerformanceBoardController : ApiController
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly IPerformanceManagementBusiness _performanceManagementBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IServiceProvider _serviceProvider;
        public PerformanceBoardController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            IPerformanceManagementBusiness performanceManagementBusiness,
            ITaskBusiness taskBusiness, IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness,
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


        #region Performance   Document

        [HttpGet]
        [Route("ReadProjectData")]
        public async Task<ActionResult> ReadProjectData(string mode,string userId)
        {
            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (mode == "Shared")
            {
                var list = await _performanceManagementBusiness.GetPerformanceSharedData();
                foreach (var a in list)
                {
                    var res = await _serviceBusiness.GetList(x => x.ParentServiceId == a.Id && x.TemplateCode == "PMS_PERFORMANCE_DOCUMENT_STAGE"
                    && x.IsDeleted == false);
                    foreach (var r in res)
                    {
                        a.StageList.Add(new StageViewModel
                        {
                            StageId = r.Id,
                            DocumentStageId = r.UdfNoteTableId,
                            StageName = r.ServiceSubject,
                            StartDate = r.StartDate,
                            EndDate = r.DueDate,
                            Goals = 1,
                            Competency = 2,
                            Status = r.ServiceStatusName
                        });
                    }

                    
                }
                return Ok(list);
            }
            else
            {
                var list = await _performanceManagementBusiness.GetPerformanceData(_userContext.UserId);
               
                return Ok(list);
            }

        }
        #endregion

        [HttpGet]
        [Route("ReadGoals")]
        public async Task<ActionResult> ReadGoals( string performanceId, string stageId, string userId)
        {
            var list = await _performanceManagementBusiness.GetGoalWeightageByPerformanceId(performanceId, stageId, userId);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadCompentancy")]
        public async Task<ActionResult> ReadCompentancy( string performanceId, string stageId, string userId)
        {
            var list = await _performanceManagementBusiness.GetCompentencyWeightageByPerformanceId(performanceId, stageId, userId);
            return Ok(list);
        }

        [HttpGet]
        [Route("UpdateCompentancy")]
        public async Task<ActionResult> UpdateCompentancy(string compentencyList)
        {
            
            string[] main = compentencyList.Split(",");
            for (int i = 0; i < main.Length; i++)
            {
                string[] getgoals = main[i].Split(":");
                if (getgoals.Length > 1)
                {
                    await _performanceManagementBusiness.updateCompentancyWeightaged(getgoals[0], getgoals[1]);
                }

            }

            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("ReadGoalsUsrwise")]
        public async Task<ActionResult> ReadGoalsUsrwise(string PerformanceId, string Type, string userID)
        {
            {
                if (Type == "PMS_COMPENTENCY")
                {
                    var list = await _performanceManagementBusiness.GetCompentencyWeightageByPerformanceId(PerformanceId, null, null);
                    return Ok(list);
                    }
                else
                {
                    var list = await _performanceManagementBusiness.GetGoalWeightageByPerformanceId(PerformanceId, null, null);
                    return Ok(list);
                    }
            }
        }

        [HttpGet]
        [Route("DeleteGoalCompetency")]
        public async Task<IActionResult> DeleteGoalCompetency(string serviceId, string code)
        {
            var service = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == serviceId);
            if (code == "PMS_COMPENTENCY")
            {
                await _performanceManagementBusiness.DeleteCompetency(serviceId);
            }
            else if (code == "PMS_GOAL")
            {
                await _performanceManagementBusiness.DeleteGoal(serviceId);
            }
            else
            {
                await _performanceManagementBusiness.DeleteDevelopment(serviceId);
            }
            await _performanceManagementBusiness.DeletService(service.Id);
            return Ok(new { success = true });
        }


    }
}
