using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.Api.Areas.EGov.Models;
using Synergy.App.Api.Controllers;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Jammu.Controllers
{ 
    [Route("jammu/grievance")]
    [ApiController]
    public class GrievanceController : ApiController
    {
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly ISmartCityBusiness _smartCityBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceBusiness _serviceBusiness;
        public GrievanceController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceBusiness serviceBusiness, ITaskBusiness taskBusiness, IServiceProvider serviceProvider,
            ISmartCityBusiness smartCityBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _smartCityBusiness = smartCityBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
        }

        [HttpGet]
        [Route("GetGrievanceTypeByDepartment")]
        public async Task<IActionResult> GetGrievanceTypeByDepartment(string department)
        {
            var res = await _smartCityBusiness.GetGrievanceTypeByDepartment(department);
            return Ok(res);
        }

        [HttpGet]
        [Route("ReadComplaintData")]
        public async Task<IActionResult> ReadComplaintData(string userId, string portalName, bool isUpperLevel, string status)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var isAdmin = _userContext.IsSystemAdmin;
            isAdmin = _userContext.IsSystemAdmin;
            var list = await _smartCityBusiness.GetJSCComplaintForResolver(isAdmin, isUpperLevel);
            foreach (var item in list)
            {
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                }
            }

            if (status.IsNotNullAndNotEmpty())
            {
                if (status == "GRV_PENDING")
                {
                    list = list.Where(x => x.GrvStatusCode == status || x.GrvStatusCode == null).ToList();
                }
                else
                {
                    list = list.Where(x => x.GrvStatusCode == status).ToList();
                }
            }

            return Ok(list);
        }

        [HttpGet]
        [Route("GetJSCMyComplaint")]
        public async Task<IActionResult> GetJSCMyComplaint(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var data = await _smartCityBusiness.GetJSCMyComplaint();
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadGrievanceCommentDataList")]
        public async Task<ActionResult> ReadGrievanceCommentDataList(string userId,string portalName,string serviceId, bool isLevelUser)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _smartCityBusiness.GetAllGrievanceComment(serviceId, isLevelUser);
            return Ok(model);
        }

        [HttpGet]
        [Route("JSCComplaintList")]
        public async Task<IActionResult> GetComplaintCountByStatus(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var isAdmin = _userContext.IsSystemAdmin;
            //var list = await _smartCityBusiness.GetJSCComplaintForResolver(isAdmin, true);
            //var disposedCount = list.Where(x => x.GrvStatus == "Disposed").Count();
            //var pendingCount = list.Where(x => x.GrvStatus == null).Count();
            //var inProgressCount = list.Where(x => x.GrvStatus == "In Progress").Count();
            //var notPertainedCount = list.Where(x => x.GrvStatus == "Not Pertaining").Count();
            //return Ok(new { disposed=disposedCount,pending=pendingCount,InProgress=inProgressCount,NotPertained=notPertainedCount});
            var list = await _smartCityBusiness.GetJSCComplaintForResolver(isAdmin, true);
            var DisposedCount = list.Where(x => x.GrvStatusCode == "GRV_DISPOSED").Count();
            var PendingCount = list.Where(x => x.GrvStatusCode == null || x.GrvStatusCode == "GRV_PENDING").Count();
            var InProgressCount = list.Where(x => x.GrvStatusCode == "GRV_IN_PROGRESS").Count();
            var NotPertainedCount = list.Where(x => x.GrvStatusCode == "GRV_NOT_PERTAINING").Count();
            var result = new { disposed = DisposedCount, pending = PendingCount, InProgress = InProgressCount, NotPertained = NotPertainedCount };
            return Ok(result);
        }

        [HttpGet]
        [Route("ManageComplaintResolverInput")]
        public async Task<IActionResult> ManageComplaintResolverInput(string id, string status, string documentId)
        {
            var details = await _smartCityBusiness.UpdateResolverInput(id, status, documentId);
            return Ok(details);
        }

        [HttpPost]
        [Route("ManageComplaint")]
        public async Task<IActionResult> ManageComplaint(JSCComplaintViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var serTempModel = new ServiceTemplateViewModel
            {
                DataAction = model.DataAction,
                ActiveUserId = _userContext.UserId,
                TemplateCode = "JSC_LODGECOMPLAINT",
                CreatedBy = _userContext.UserId,
                OwnerUserId = _userContext.UserId,
            };
            var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);
            sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
            sermodel.DataAction = DataActionEnum.Create;
            var result = await _serviceBusiness.ManageService(sermodel);
            return Ok(result);
        }

        [HttpGet]
        [Route("CheckIfDDNExist")]
        public async Task<ActionResult> CheckIfDDNExist(string ddn)
        {
            var model = await _smartCityBusiness.CheckIfDDNExist(ddn);
            return Ok(model);
        }

        [HttpGet]
        [Route("ComplaintMarkFlag")]
        public async Task<IActionResult> ComplaintMarkFlag(string id)
        {
            var details = await _smartCityBusiness.ComplaintMarkFlag(id);
            return Ok(details);
        }

        [HttpGet]
        [Route("ReopenComplaint")]
        public async Task<IActionResult> ReopenComplaint(string parentId, string documents)
        {
            var details = await _smartCityBusiness.ReopenComplaint(parentId, documents);
            return Ok(details);
        }

        [HttpGet]
        [Route("GetReopenComplaintDetails")]
        public async Task<IActionResult> GetReopenComplaintDetails(string parentId)
        {
            var details = await _smartCityBusiness.GetReopenComplaintDetails(parentId);
            return Ok(details);
        }

        [HttpGet]
        [Route("UpdateDepartmentByOperator")]
        public async Task<IActionResult> UpdateDepartmentByOperator(string id, string departmentId, string grievanceTypeId)
        {
            var details = await _smartCityBusiness.UpdateDepartmentByOperator(id, departmentId, grievanceTypeId);
            return Ok(details);
        }
        [HttpGet]
        [Route("MarkDisposedByOperator")]
        public async Task<IActionResult> MarkDisposedByOperator(string id)
        {
            var details = await _smartCityBusiness.MarkDisposedByOperator(id);
            return Ok(details);
        }
    }
}
