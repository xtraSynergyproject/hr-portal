using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class ServiceController : ApplicationController
    {
        private readonly ISettingsBusiness _seetingsBusiness;
        private readonly INtsBusiness _ntsBusiness;
        private readonly IUserContext _userContext;
        private readonly IServiceBusiness _serviceBusiness;
        public ServiceController(ISettingsBusiness seetingsBusiness, INtsBusiness ntsBusiness,
            IUserContext userContext, IServiceBusiness serviceBusiness)
        {
            _seetingsBusiness = seetingsBusiness;
            _ntsBusiness = ntsBusiness;
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ReadAttachedReplies(string serviceId)
        {
            var result = await _serviceBusiness.GetServiceAttachedReplies(_userContext.UserId, serviceId);
            return Json(result);
        }
        public async Task<ActionResult> GetServiceDetails(string id)
        {
            var data = await _serviceBusiness.GetSingleById(id);
            return Json(data);
        }
        public async Task<IActionResult> GetServiceSummary()
        {
            var result = await _serviceBusiness.GetServiceSummary(_userContext.PortalId, _userContext.UserId);
            return Json(result);
        }
    }
}
