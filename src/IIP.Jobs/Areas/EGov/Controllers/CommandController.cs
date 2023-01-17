using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.EGov.Controllers
{
    [Route("egov/command")]
    [ApiController]
    public class CommandController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public CommandController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        [HttpPost]
        [Route("UpdateFacilityHealth")]
        public async Task<ActionResult> UpdateFacilityHealth(FacilityViewModel model)
        {
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();

            dynamic exo = new System.Dynamic.ExpandoObject();
            
            ((IDictionary<String, Object>)exo).Add("FacilityId", model.Id);
            ((IDictionary<String, Object>)exo).Add("HealthStatusId", model.HealthStatusId);            
            
            var Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            await _cmsBusiness.CreateForm(Json, "", "SWACHH_SANJY_FACILITY_HEALTH_FORM");

            return Ok(new { success = true});
        } 
    }
}
