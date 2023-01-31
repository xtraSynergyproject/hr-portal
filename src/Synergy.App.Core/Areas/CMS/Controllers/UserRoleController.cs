using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Synergy.App.DataModel;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserRoleController : ApplicationController
    {
        private IUserRoleBusiness _userRoleBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        private IUserRolePortalBusiness _userRolePortalBusiness;
        private IUserRoleHierarchyPermissionBusiness _userRoleHierarchyPermissionBusiness;
        private IUserContext _userContext;
        private IPortalBusiness _portalBusiness;
        private IPageBusiness _pageBusiness;
        private IUserRolePreferenceBusiness _userRolePreferenceBusiness;
        public UserRoleController(IUserRoleBusiness userRoleBusiness, IUserRoleUserBusiness userRoleUserBusiness, IPortalBusiness portalBusiness,
          IUserContext userContext,  IPageBusiness pageBusiness,IUserRolePortalBusiness userRolePortalBusiness, IUserRolePreferenceBusiness userRolePreferenceBusiness,
          IUserRoleHierarchyPermissionBusiness userRoleHierarchyPermissionBusiness)
        {
            _userRoleBusiness = userRoleBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _userRolePortalBusiness = userRolePortalBusiness;
            _userRoleHierarchyPermissionBusiness = userRoleHierarchyPermissionBusiness;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
            _pageBusiness = pageBusiness;
            _userRolePreferenceBusiness = userRolePreferenceBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }


      
        public async Task<ActionResult> ReadData()
        {
            var model = await _userRoleBusiness.GetList();
            return Json(model);
        }

        public IActionResult CreateUserRole()
        {
            return View("ManageUserRole", new UserRoleViewModel
            {
                DataAction = DataActionEnum.Create,
                //Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            });
        }

        public async Task<IActionResult> EditUserRole(string Id)
        {
            var member = await _userRoleBusiness.GetSingleById(Id);

            if (member != null)
            {
                //var RoleList = await _userRoleUserBusiness.GetList(x => x.UserRoleId == Id);
                //if (RoleList != null && RoleList.Count() > 0)
                //{
                //    member.UserIds = RoleList.Select(x => x.UserId).ToList();

                //}
                var userPortals = await _userRolePortalBusiness.GetPortalByUserRole(Id);
                member.Portal = new List<string>();
                if (userPortals.IsNotNull())
                {
                    foreach (var up in userPortals)
                    {
                        member.Portal.Add(up.PortalId);
                    }
                }
                member.DataAction = DataActionEnum.Edit;
                return View("ManageUserRole", member);
            }
            return View("ManageUserRole", new UserRoleViewModel());
        }

        public async Task<IActionResult> DeleteUserRole(string Id)
        {


            await _userRoleBusiness.Delete(Id);
            return Json(true);
        }

        public async Task<IActionResult> DeleteUserFromUserRole(string Id)
        {
            await _userRoleUserBusiness.Delete(Id);
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRole(UserRoleViewModel model)
        {
            var userRoleId = ""; 
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userRoleBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //Add UserPortal data
                        if (model.Portal.IsNotNull())
                        {
                            foreach (var p in model.Portal)
                            {
                                var res = await _userRolePortalBusiness.Create(new UserRolePortalViewModel
                                {
                                    UserRoleId = result.Item.Id,
                                    PortalId = p,
                                });
                            }
                        }
                        userRoleId = result.Item.Id;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _userRoleBusiness.Edit(model);
                    if (result.IsSuccess)
                    {

                        var portals = await _userRolePortalBusiness.GetPortalByUserRole(model.Id);

                        if (portals.IsNotNull())
                        {
                            foreach (var p in portals)
                            {
                                if (model.Portal == null || model.Portal.Contains(p.PortalId) == false)
                                {
                                    await _userRolePortalBusiness.Delete(p.Id);
                                }
                            }
                        }
                        if (model.Portal.IsNotNull())
                        {

                            foreach (var p in model.Portal)
                            {
                                if (!portals.Where(x => x.PortalId == p).Any())
                                {
                                    var res = await _userRolePortalBusiness.Create(new UserRolePortalViewModel
                                    {
                                        UserRoleId = result.Item.Id,
                                        PortalId = p,
                                    });
                                }
                            }
                        }
                        ViewBag.Success = true;
                        userRoleId = result.Item.Id;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return Json(new { success=true, data = userRoleId });
        }

        [HttpGet]
        public async Task<ActionResult> GetUserRoleIdNameList(string portalId)
        {
            var data = await _userRoleBusiness.GetList();
            if (portalId.IsNotNull()) 
            {
                data = await _userRolePortalBusiness.GetUserRoleByPortal(portalId);
            }
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToUserRole(string userRoleId, string userId)
        {
            var exist = await _userRoleUserBusiness.GetSingle(x => x.UserId == userId && x.UserRoleId == userRoleId);
            if (exist.IsNotNull())
            {
                return Json(new { success = false, msg="User already added to this role" });
            }
            else
            {
                var model = new UserRoleUserViewModel()
                {
                    UserRoleId = userRoleId,
                    UserId = userId
                };
                var result = await _userRoleUserBusiness.Create(model);
                return Json(new { success = true });
            }            

        }

        public async Task<IActionResult> UserRoleHierarchyPermission(UserRoleHierarchyPermissionViewModel model)
        {


            return View("UserRoleHierarchyPermission", model);
        }

        public async Task<IActionResult> ManageUserRoleHierarchyPermission(string userRoleId, string pId)
        {
            if (pId.IsNotNullAndNotEmpty())
            {

                var member = await _userRoleHierarchyPermissionBusiness.GetSingleById(pId);



                member.DataAction = DataActionEnum.Edit;
                return View("ManageUserRoleHierarchyPermission", member);
            }
            else
            {
                var model = new UserRoleHierarchyPermissionViewModel();
                model.UserRoleId = userRoleId;
                model.DataAction = DataActionEnum.Create;
                return View("ManageUserRoleHierarchyPermission", model);
            }

        }
        [HttpPost]
        public async Task<ActionResult> ManageUserRoleHierarchyPermission(UserRoleHierarchyPermissionViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userRoleHierarchyPermissionBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _userRoleHierarchyPermissionBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageUserRoleHierarchyPermission", model);
        }
        public ActionResult ReadUserRoleHierarchyPermission(string userRoleId)
        {

            //var list = await _userHierarchyPermissionBusiness.GetUserPermissionHierarchy(userId);
            //var json = Json(list.ToDataSourceResult(request));
            //return json;
            var model = _userRoleHierarchyPermissionBusiness.GetUserPermissionHierarchy(userRoleId);
            var data = model.Result;
            return Json(data);
        }
        public async Task<IActionResult> DeleteUserRoleHierarchyPermission(string Id)
        {
            await _userRoleHierarchyPermissionBusiness.Delete(Id);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<ActionResult> ReadPortalByCompanyId()
        {
            var portals = await _userRoleBusiness.GetSingle<CompanyViewModel, Company>(x => x.Id == _userContext.CompanyId);
            var licensedPortalIds = portals.LicensedPortalIds.ToList();
            var portalIds = await _portalBusiness.GetList(x => licensedPortalIds.Any(y => y == x.Id));
          
            return Json(portalIds);
        }

        [HttpGet]
        public async Task<ActionResult> ReadPageByPortalId(string portalId)
        {
            
            var pages = await _pageBusiness.GetList(x => x.PortalId == portalId);
            return Json(pages);
        }

        public async Task<ActionResult> GetUserRolePreferences(string userRoleId)
        {
            var model = await _userRoleBusiness.GetUserRolePreferences(userRoleId);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserRolePreferences(string userRoleId, string portalId,string pageId,DataActionEnum mode)
        {
            if (mode == DataActionEnum.Create)
            {
                var exist = await _userRolePreferenceBusiness.GetSingle(x => x.UserRoleId == userRoleId && x.PreferencePortalId == portalId);
                if (exist.IsNotNull())
                {
                    return Json(new { success = false, msg = "User Role Preference for the Portal already Exist" });
                }
                else
                {
                    var model = new UserRolePreferenceViewModel()
                    {
                        UserRoleId = userRoleId,
                        PreferencePortalId = portalId,
                        DefaultLandingPageId=pageId,
                        DataAction=DataActionEnum.Create,
                    };
                    var result = await _userRolePreferenceBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false });
                }
            }
            
            
            else
            {
                var exist = await _userRolePreferenceBusiness.GetSingle(x => x.UserRoleId == userRoleId && x.PreferencePortalId == portalId);
                exist.DefaultLandingPageId = pageId;
                exist.DataAction = DataActionEnum.Edit;
                var result = await _userRolePreferenceBusiness.Edit(exist);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }

        }

        public async Task<IActionResult> DeleteUserRolePreference(string Id)
        {


            await _userRolePreferenceBusiness.Delete(Id);
            return Json(true);
        }
    }
}
