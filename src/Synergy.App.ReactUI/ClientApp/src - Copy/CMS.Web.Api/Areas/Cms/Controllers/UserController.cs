using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.Cms.Controllers
{
    [Route("cms/user")]
    [ApiController]
    public class UserController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public UserController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
       
        [HttpGet]
        [Route("ReadUserData")]
        public async Task<ActionResult> ReadUserData()
        {
            var _business = _serviceProvider.GetService<IUserBusiness>();
            var model = await _business.GetUserList();
            var data = model.ToList();
            return Ok(data);
        }
    }
}
