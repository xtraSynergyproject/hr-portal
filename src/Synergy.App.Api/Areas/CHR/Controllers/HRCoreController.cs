using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.CHR.Controllers
{
    [Route("chr/hrcore")]
    [ApiController]
    public class HRCoreController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHRCoreBusiness _hrCoreBusiness;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public HRCoreController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, IHRCoreBusiness hrCoreBusiness, IUserContext userContext) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _hrCoreBusiness = hrCoreBusiness;
        }

        [HttpGet]
        [Route("ResignationTermination")]
        public async Task<IActionResult> ResignationTermination( string userId,string portalName)
        {
            await Authenticate(userId,portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var regter = await _hrCoreBusiness.GetResignationTerminationList(userId);
            //var exist = regter.Where(x => x.ServiceStatus != "Canceled" && x.ServiceStatus != "Rejected").ToList();

            return Ok(regter);
        }

        [HttpGet]
        [Route("Termination")]
        public async Task<IActionResult> Termination( string userId,string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            //var model = new ResignationTerminationViewModel();

            var regter = await _hrCoreBusiness.GetResignationTerminationList(userId);
            //var exist = regter.Where(x => x.ServiceStatus != "Canceled" && x.ServiceStatus != "Rejected").ToList();

            return Ok(regter);
        }

        [HttpGet]
        [Route("GetAllOrganisation")]
        public async Task<IActionResult> GetAllOrganisation()
        {
            var model = await _hrCoreBusiness.GetAllOrganisation();
            return Ok(model);
        }

    }
}
