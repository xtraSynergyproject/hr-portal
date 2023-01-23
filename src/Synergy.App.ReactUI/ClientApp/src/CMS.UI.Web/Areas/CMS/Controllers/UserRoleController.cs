using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserRoleController : ApplicationController
    {
        private IUserRoleBusiness _userRoleBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        private IUserRolePortalBusiness _userRolePortalBusiness;

        public UserRoleController(IUserRoleBusiness userRoleBusiness, IUserRoleUserBusiness userRoleUserBusiness,
            IUserRolePortalBusiness userRolePortalBusiness)
        {
            _userRoleBusiness = userRoleBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _userRolePortalBusiness = userRolePortalBusiness;
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
    }
}
