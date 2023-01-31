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
    [Route("cms/user")]
    [ApiController]
    public class UserController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private IUserRoleBusiness _userRoleBusiness;

        private IUserRolePortalBusiness _userRolePortalBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public UserController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IUserRoleBusiness userRoleBusiness,
            IUserRolePortalBusiness userRolePortalBusiness,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userRoleBusiness = userRoleBusiness;
            _userRolePortalBusiness = userRolePortalBusiness;
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

        [HttpGet]
        [Route("ReadUserPermissionData")]
        public async Task<ActionResult> ReadUserPermissionData( string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            var data = await _userBusiness.ViewUserPermissions(userId);

            var dsResult = (from x in data where x.PortalName == portalName select x).ToList();//data.Where(x=>x.PortalName==portalName);
            return Ok(dsResult);
        }

        [HttpGet]
        [Route("GetPermissionlist")]
        public async Task<ActionResult> GetPermissionlist(string userId, string pageName, string perName, string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _userPermissionBusiness = _serviceProvider.GetService<IUserPermissionBusiness>();
            var _pageBusiness = _serviceProvider.GetService<IPageBusiness>();
            var _permissionBusiness = _serviceProvider.GetService<IPermissionBusiness>();

            var list = new List<IdNameViewModel>();
            var result = await _userPermissionBusiness.GetList(x => x.UserId == userId);
            foreach (var item in result)
            {
                var page = await _pageBusiness.GetSingleById(item.PageId);
                foreach (var per in item.Permissions)
                {
                    if (page.IsNotNull())
                    {
                        var permission = await _permissionBusiness.GetSingle(x => x.Code == per);
                        list.Add(new IdNameViewModel { Name = page.Name + "-" + per, Id = "chk_" + permission.Id + "_" + page.Id });
                    }
                }
            }
            if (pageName.IsNotNullAndNotEmpty() && perName.IsNotNullAndNotEmpty())
            {
                list.Add(new IdNameViewModel { Name = pageName + "-" + perName });
            }
            // var model = result.ToList();
            return Ok(list);
        }


        [HttpGet]
        [Route("GetUserRoleIdNameList")]
        public async Task<ActionResult> GetUserRoleIdNameList(string userId,string portalName)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();

            var data = await _userRoleBusiness.GetList();
            if (_context.PortalId.IsNotNull())
            {
                data = await _userRolePortalBusiness.GetUserRoleByPortal(_context.PortalId);
            }
            return Ok(data);
        }

    }
}
