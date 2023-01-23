using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Cms.Controllers
{
    [Route("cms/task")]
    [ApiController]
    public class NtsTaskController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ITaskBusiness _taskBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NtsTaskController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider, ITaskBusiness taskBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _taskBusiness = taskBusiness;
        }

        [HttpGet]
        [Route("GetTaskSummary")]
        public async Task<IActionResult> GetTaskSummary(string userId,string portalName)
        {
            await Authenticate(userId,portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _taskBusiness.GetTaskSummary(_userContext.PortalId, _userContext.UserId);
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadTaskListCount")]
        public async Task<IActionResult> ReadTaskListCount(string categoryCodes, string userId, string portalName, bool showAllTaskForAdmin = true)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _taskBusiness.GetTaskCountByServiceTemplateCodes(categoryCodes, _userContext.PortalId, showAllTaskForAdmin);

            //var result = await _taskBusiness.GetTaskCountByServiceTemplateCodes(categoryCodes, _userContext.PortalId);
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadTaskList")]
        public async Task<IActionResult> ReadTaskList(string templateCodes = null, string portalName=null, string moduleCodes = null, string catCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();


            var dt = await _taskBusiness.GetTaskList(_userContext.PortalId, moduleCodes, templateCodes, catCodes, statusCodes, parentServiceId, userId, parentNoteId);
            return Ok(dt);
        }

        [HttpGet]
        [Route("ReadTaskData")]
        public async Task<IActionResult> ReadTaskData(string categoryCodes, string taskStatus, string userId, string portalName, bool showAllTaskForAdmin = true)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var list = await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, taskStatus, _userContext.PortalId, showAllTaskForAdmin);
            // var list = await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, taskStatus, _userContext.PortalId);
            //var list = new List<TaskViewModel>();// await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, taskStatus, _userContext.PortalId);
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadTaskHomeData")]
        public async Task<IActionResult> ReadTaskHomeData(string userId,string portalName, string moduleId, string mode, string taskNo, string taskStatus, string taskAssigneeIds, string subject, DateTime? startDate, DateTime? dueDate, DateTime? completionDate, string templateMasterCode, string text)
        {
            try
            {
                await Authenticate(userId,portalName);
                var _business = _serviceProvider.GetService<ITaskBusiness>();
                var _context = _serviceProvider.GetService<IUserContext>();
                if (userId.IsNullOrEmpty())
                {
                    userId = _context.UserId;
                }
                var taskAssignees = new List<string>();
                if (taskAssigneeIds.IsNotNullAndNotEmpty())
                {
                    taskAssignees = taskAssigneeIds.Split(",").ToList();

                }
                var result = await _business.GetSearchResult(new TaskSearchViewModel
                {
                    ModuleId = moduleId,
                    Mode = mode,
                    TaskNo = taskNo,
                    TaskStatus = taskStatus,
                    TaskAssigneeIds = taskAssignees,
                    Subject = subject,
                    StartDate = startDate,
                    DueDate = dueDate,
                    CompletionDate = completionDate,
                    TemplateMasterCode = templateMasterCode,
                    UserId = userId
                });
                result = result.Where(x => x.PortalId == _context.PortalId).ToList();

                if (text == "Today")
                {
                    var res = result.Where(x => x.DueDate <= DateTime.Now && x.TaskStatusCode != "TASK_STATUS_COMPLETE" && x.TaskStatusCode != "TASK_STATUS_CANCEL" && x.TaskStatusCode != "TASK_STATUS_DRAFT");
                    return Ok(res);
                }
                else if (text == "Week")
                {
                    var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.TaskStatusCode != "TASK_STATUS_COMPLETE" && x.TaskStatusCode != "TASK_STATUS_CANCEL" && x.TaskStatusCode != "TASK_STATUS_DRAFT")).ToList();
                    return Ok(res);
                }
                var data = result.OrderByDescending(x => x.LastUpdatedDate);
                if (data.Count() > 1000)
                {
                    return Ok(data.Take(1000));
                }
                else
                {
                    return Ok(data);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpGet]
        [Route("ReadTaskDataPending")]
        public async Task<IActionResult> ReadTaskDataPending(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _business = _serviceProvider.GetService<IRecTaskBusiness>();

            var result = await _business.GetActiveListByUserId(_context.UserId);
            var j = Ok(result.Where(x => x.TemplateCode == "CONFIRM_INDUCTION_DATE_TO_CANDIDATE" && (x.TaskStatusCode == "INPROGRESS" || x.TaskStatusCode == "OVERDUE")).OrderByDescending(x => x.StartDate));
            return j;
        }

        [HttpGet]
        [Route("ReadJDTaskDataPending")]
        public async Task<IActionResult> ReadJDTaskDataPending(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _business = _serviceProvider.GetService<IRecTaskBusiness>();
            var result = await _business.GetTaskByTemplateCode("JOBDESCRIPTION_HM");
            var j = Ok(result.OrderByDescending(x => x.StartDate));
            return j;
        }

    }
}
