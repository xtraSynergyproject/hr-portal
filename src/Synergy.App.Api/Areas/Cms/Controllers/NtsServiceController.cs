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
    [Route("cms/service")]
    [ApiController]
    public class NtsServiceController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceBusiness _serviceBusiness;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NtsServiceController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, IServiceBusiness serviceBusiness, IUserContext userContext) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceBusiness = serviceBusiness;
        }
        [HttpGet]
        [Route("LoadCustomServiceIndexPageGrid")]
        public async Task<IActionResult> LoadCustomServiceIndexPageGrid(string userId, string templateId, bool showAllOwnersService, string moduleCodes, string templateCodes, string categoryCodes)
        {

            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var dt = await _serviceBusiness.GetCustomServiceIndexPageGridData(null, templateId, showAllOwnersService, moduleCodes, templateCodes, categoryCodes);
            return Ok(dt);
        }

        [HttpGet]
        [Route("ReadDataLog")]
        public async Task<IActionResult> ReadDataLog(string ServiceId, string TemplateCode, string TemplateType)
        {
            //var model = await _serviceBusiness.GetServiceLog(ServiceId, TemplateCode);
            var data = await _serviceBusiness.GetDynamicService(TemplateCode, ServiceId, TemplateType);

            return Ok(data);

        }
            
        //New functions
        [HttpGet]
        [Route("GetServiceSummary")]
        public async Task<IActionResult> GetServiceSummary(string userId,string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _serviceBusiness.GetServiceSummary(_userContext.PortalId, _userContext.UserId);
            return Ok(result);
        }


        [HttpGet]
        [Route("ReadServiceListCount")]
        public async Task<IActionResult> ReadServiceListCount(string categoryCodes, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _serviceBusiness.GetServiceCountByServiceTemplateCodes(categoryCodes, _userContext.PortalId);
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadServiceData")]
        public async Task<IActionResult> ReadServiceData(string categoryCodes, string serviceStatus, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var list = await _serviceBusiness.GetServiceListByServiceCategoryCodes(categoryCodes, serviceStatus, _userContext.PortalId);
            //var j = Json(list.ToDataSourceResult(request));
            return Ok(list);
            
        }



        [HttpGet]
        [Route("ReadServiceHomeData")]
        public async Task<IActionResult> ReadServiceHomeData(string userId, string text, string templateCategoryCode, string filterUserId, string moduleId, string mode, string serviceNo, string serviceStatus, string subject, DateTime? startDate, DateTime? dueDate, DateTime? completionDate, string templateMasterCode,string portalName)
        {
            await Authenticate(userId,portalName);

            var _business = _serviceProvider.GetService<IServiceBusiness>();
            var _context = _serviceProvider.GetService<IUserContext>();
            if (userId.IsNullOrEmpty())
            {
                userId = _context.UserId;
            }
            var result = await _business.GetSearchResult(new ServiceSearchViewModel
            {
                ModuleId = moduleId,
                Mode = mode,
                ServiceNo = serviceNo,
                ServiceStatus = serviceStatus,
                Subject = subject,
                StartDate = startDate,
                DueDate = dueDate,
                CompletionDate = completionDate,
                TemplateMasterCode = templateMasterCode,
                TemplateCategoryCode = templateCategoryCode,
                FilterUserId = filterUserId,
                ////TemplateCategoryType= templateCategoryType,
                UserId = userId
            });
            if (text == "Today")
            {
                var res = result.Where(x => x.DueDate <= DateTime.Now && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT");
                return Ok(res);
            }
            else if (text == "Week")
            {
                var res = result.Where(x => (x.DueDate <= DateTime.Now.AddDays(7) && DateTime.Now <= x.DueDate && x.ServiceStatusCode != "SERVICE_STATUS_COMPLETE" && x.ServiceStatusCode != "SERVICE_STATUS_CANCEL" && x.ServiceStatusCode != "SERVICE_STATUS_DRAFT")).ToList();
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

        [HttpGet]
        [Route("DeleteService")]
        public async Task<ActionResult> DeleteService(string serviceId)
        {
            await _serviceBusiness.Delete(serviceId);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("DeleteServiceBook")]
        public async Task<ActionResult> DeleteServiceBook(string serviceId)
        {
            await _serviceBusiness.DeleteServiceBook(serviceId);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("GetServiceUsersIdNameList")]
        public async Task<ActionResult> GetServiceUsersIdNameList(string serviceId)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            list.Add(new IdNameViewModel { Id = "All", Name = "All" });
            var userList = await _serviceBusiness.GetServiceUserList(serviceId);
            list.AddRange(userList);
            return Ok(list);
        }

    }
}
