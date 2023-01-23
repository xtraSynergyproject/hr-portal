using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class InboxController : ApplicationController
    {
        private readonly IUserContext _userContext;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        public InboxController(IUserContext userContext, ICmsBusiness cmsBusiness, IServiceBusiness serviceBusiness, ITemplateBusiness templateBusiness
            , ILOVBusiness lovBusiness
            , ITaskBusiness taskBusiness)
        {
            _userContext = userContext;
            _cmsBusiness = cmsBusiness;
            _serviceBusiness = serviceBusiness;
            _templateBusiness = templateBusiness;
            _lovBusiness = lovBusiness;
            _taskBusiness = taskBusiness;
        }

        public IActionResult Index(string inboxCode)
        {
            ViewBag.InboxCode = inboxCode;
            return View();
        }
        public async Task<ActionResult> InboxRightIndex(string templateCode,string statusName)
        {
            var template = await _templateBusiness.GetSingle(x => x.Code == templateCode);
            ViewBag.TemplateCode = templateCode;
            ViewBag.StatusName = statusName;
            if (template.TemplateType == TemplateTypeEnum.Service)
            {
                return View("ServiceIndex");
            }
            else 
            {
                return View("TaskIndex");
            }
            
        }
        public async Task<ActionResult> ReadServiceData([DataSourceRequest] DataSourceRequest request, string templateCode, string statusName)
        {
            //var template = await _templateBusiness.GetSingle(x => x.Code == templateCode);
            //statusName = statusName.TrimEnd(',');
            //var ids = statusName.Split(",");
            //var status = await _lovBusiness.GetList(x => ids.Contains(x.Code) && x.LOVType=="LOV_SERVICE_STATUS");
            //var list = status.Select(x => x.Id);
            //var model = await _serviceBusiness.GetList(x=>x.TemplateId== template.Id && list.Contains(x.ServiceStatusId));

            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            search.TemplateMasterCode = templateCode;
            search.ServiceStatus = statusName;
            var viewModel = await _serviceBusiness.GetSearchResult(search);

            var dsResult = viewModel.OrderByDescending(x => x.LastUpdatedDate).ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadData(string templateCode, string statusName)
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            search.TemplateMasterCode = templateCode;
            search.ServiceStatus = statusName;
            var viewModel = await _serviceBusiness.GetSearchResult(search);
            var dsResult = viewModel.OrderByDescending(x => x.LastUpdatedDate);
            return Json(dsResult);
        }

        public async Task<ActionResult> ReadTaskData([DataSourceRequest] DataSourceRequest request, string templateCode, string statusName)
        {
            var template = await _templateBusiness.GetSingle(x => x.Code == templateCode);
            //statusName = statusName.TrimEnd(',');
            //var ids = statusName.Split(",");
            //var status = await _lovBusiness.GetList(x => ids.Contains(x.Code) && x.LOVType == "LOV_TASK_STATUS");
            //var list = status.Select(x => x.Id);

            //var model = await _taskBusiness.GetList(x => x.TemplateId == template.Id && list.Contains(x.TaskStatusId));

            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = _userContext.UserId;
            search.TemplateMasterCode = templateCode;
            search.Mode = "ASSIGN_TO";
            search.TaskStatus = statusName;
            var viewModel = await _taskBusiness.GetSearchResult(search);

            var dsResult = viewModel.OrderByDescending(x => x.LastUpdatedDate).ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadTaskDataList(string templateCode, string statusName)
        {
            var template = await _templateBusiness.GetSingle(x => x.Code == templateCode);            
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = _userContext.UserId;
            search.TemplateMasterCode = templateCode;
            search.Mode = "ASSIGN_TO";
            search.TaskStatus = statusName;
            var viewModel = await _taskBusiness.GetSearchResult(search);
            var dsResult = viewModel.OrderByDescending(x => x.LastUpdatedDate);
            return Json(dsResult);
        }
        public async Task<IActionResult> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList, string inboxCode)
        {
            var result = await _cmsBusiness.GetInboxTreeviewList(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes, inboxCode);
            var model = result.ToList();
            return Json(model);
        }
        public IActionResult TECProcessInbox(string templateCode)
        {
            ViewBag.TemplateCode = templateCode;
            return View();
        }
        public async Task<IActionResult> GetTECProcessInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList)
        {
            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PROCESS_OWNER"))
            {
                var result1 = await _cmsBusiness.GetInboxMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);

                var model1 = result1.ToList();
                return Json(model1);
            }
            else if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PROCESS_USER"))
            {
                var result = await _cmsBusiness.GetInboxMenuItemByUser(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
                var model = result.ToList();
                return Json(model);
            }
            return Json(new List<TASTreeViewViewModel>());
        }

        public async Task<IActionResult> ReadInboxData(string id, string type, string templateCode)
        {
            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PROCESS_OWNER"))
            {
                var result1 = await _cmsBusiness.ReadInboxData(id, type, templateCode);
                var model1 = result1.ToList();
                return Json(model1);
            }
            else if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PROCESS_USER"))
            {
                var result1 = await _cmsBusiness.ReadInboxData(id, type, templateCode,_userContext.UserId);
                var model1 = result1.ToList();
                return Json(model1);
            }
            return Json(new List<TaskTemplateViewModel>());
        }
        public IActionResult InboxData(string id, string type, string templateCode)
        {
            ViewBag.Id = id;
            ViewBag.Type = type;
            ViewBag.TemplateCode = templateCode;
            return View();
        }

       
    }
}
