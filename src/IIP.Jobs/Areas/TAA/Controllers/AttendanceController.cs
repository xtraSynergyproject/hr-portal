using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Api.Controllers;

namespace Synergy.App.Api.Areas.TAA.Controllers
{
    [Route("taa/attendance")]
    [ApiController]
    public class AttendanceController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IAttendanceBusiness _attendanceBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public AttendanceController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, IAttendanceBusiness attendanceBusiness, IHRCoreBusiness hRCoreBusiness, IUserContext userContext) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _attendanceBusiness = attendanceBusiness;
            _hRCoreBusiness = hRCoreBusiness;
        }

        
        [HttpGet]
        [Route("GetEmployeeAtendanceList")]
        public async Task<ActionResult> GetEmployeeAtendanceList(string userId,string portalName,DateTime searchStart,DateTime searchEnd, DateTime searchMonth,string searchType)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _attendanceBusiness.GetTimeinTimeOutDetails(userId, searchStart, searchEnd,searchMonth, searchType);

            return Ok(model);

        }
    }
}
