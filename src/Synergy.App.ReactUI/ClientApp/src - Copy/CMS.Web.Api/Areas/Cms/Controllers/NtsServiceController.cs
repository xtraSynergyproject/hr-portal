using CMS.Business;
using CMS.Common;
using CMS.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.Cms.Controllers
{
    [Route("cms/query")]
    [ApiController]
    public class NtsServiceController : ApiController
    {
        private readonly IServiceProvider _serviceProvider; 
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserContext _userContext;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NtsServiceController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider,IServiceBusiness serviceBusiness, IUserContext userContext) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
        }
        [HttpGet]
        [Route("LoadCustomServiceIndexPageGrid")]
        public async Task<IActionResult> LoadCustomServiceIndexPageGrid(string userId,string templateId, bool showAllOwnersService, string moduleCodes, string templateCodes, string categoryCodes)
        {

            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var dt = await _serviceBusiness.GetCustomServiceIndexPageGridData(null, templateId, showAllOwnersService, moduleCodes, templateCodes, categoryCodes);
            return Ok(dt);
        }
    }
}
