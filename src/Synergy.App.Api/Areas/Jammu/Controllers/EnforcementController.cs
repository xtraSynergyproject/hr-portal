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
    [Route("jammu/enforcement")]
    [ApiController]
    public class EnforcementController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly ISmartCityBusiness _smartCityBusiness;
        public EnforcementController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            ISmartCityBusiness smartCityBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _smartCityBusiness = smartCityBusiness;
        }
        [HttpGet]
        [Route("GetViolationData")]
        public async Task<IActionResult> GetViolationData()
        {
            var res = await _smartCityBusiness.GetViolationData();
            return Ok(res);
        }

        [HttpPost]
        [Route("InsertEnforcementUnAuthorization")]
        public async Task<IActionResult> InsertEnforcementUnAuthorization(JSCEnforcementUnAuthorizationViewModel model)
        {
            await Authenticate(model.CreatedBy,model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var result = await _smartCityBusiness.InsertEnforcementUnAuthorization(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAuthorizationList")]
        public async Task<IActionResult> GetAuthorizationList(string userId)
        {
            var wardString = await _smartCityBusiness.GetWardListByUser(userId);
            var wardList = new List<string>();

            wardString = wardString.TrimStart('[');
            wardString = wardString.TrimEnd(']');
            wardString = wardString.Replace(" ", "");
            var list = wardString.Split(",");
            wardList.AddRange(list);
            var res = await _smartCityBusiness.GetAuthorizationList();
            if (wardList.Count > 0)
            {
                res = res.Where(x => wardList.Contains(x.WardNo)).ToList();
                //  data = data.Where(x => !wardList.Any(y => y == x.Id)).ToList();
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("GetWardListByUserId")]
        public async Task<IActionResult> GetWardListByUserId(string userId)
        {
            var wardString = await _smartCityBusiness.GetWardListByUser(userId);
            var wardList = new List<string>();

            wardString = wardString.TrimStart('[');
            wardString = wardString.TrimEnd(']');
            wardString = wardString.Replace(" ", "");
            var list = wardString.Split(",");
            wardList.AddRange(list);
            var res = await _smartCityBusiness.GetWardList();
            if (wardList.Count > 0)
            {
                res = res.Where(x => wardList.Contains(x.Id)).ToList();
                //  data = data.Where(x => !wardList.Any(y => y == x.Id)).ToList();
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("InsertEnforcementAuthorization")]
        public async Task<IActionResult> InsertEnforcementAuthorization(JSCEnforcementUnAuthorizationViewModel model)
        {
            await Authenticate(model.CreatedBy, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var result = await _smartCityBusiness.InsertEnforcementAuthorization(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEnforcementAuthorization")]
        public async Task<IActionResult> GetEnforcementAuthorization(DateTime? date, string ward, string userId)
        {
            //var data = await _smartCityBusiness.GetEnforcementAuthorization(userId);
            var data = await _smartCityBusiness.GetJSCAuthorizedViolationsDetail(date,ward,userId);
            return Ok(data);

        }

        [HttpGet]
        [Route("GetEnforcementUnAuthorization")]
        public async Task<IActionResult> GetEnforcementUnAuthorization(DateTime? date, string ward, string userId)
        {
            //var data = await _smartCityBusiness.GetEnforcementUnAuthorization(userId);
            var data = await _smartCityBusiness.GetJSCUnauthorizedViolationsDetail(date,ward, userId);
            return Ok(data);

        }

        [HttpPost]
        [Route("InsertEnforcementAuthorizedWeeklyReport")]
        public async Task<IActionResult> InsertEnforcementAuthorizedWeeklyReport(JSCEnforcementUnAuthorizationViewModel model)
        {
            await Authenticate(model.CreatedBy, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var result = await _smartCityBusiness.InsertEnforcementAuthorizedWeeklyReport(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetEnforcementAuthorizationWeeklyReport")]
        public async Task<IActionResult> GetEnforcementAuthorizationWeeklyReport()
        {
            //var data = await _smartCityBusiness.GetEnforcementUnAuthorization(userId);
            var data = await _smartCityBusiness.GetEnforcementAuthorizationWeeklyReport();
            return Ok(data);

        }
    }
}
