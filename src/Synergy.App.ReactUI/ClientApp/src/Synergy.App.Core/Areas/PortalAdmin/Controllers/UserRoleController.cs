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
using Newtonsoft.Json.Linq;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class UserRoleController : ApplicationController
    {
        private IUserRoleBusiness _userRoleBusiness;
        private IPageBusiness _pageBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        private IUserRolePortalBusiness _userRolePortalBusiness;
        private IUserContext _userContext;
        private IUserBusiness _userBusiness;
        private IPortalBusiness _portalBusiness;
        public UserRoleController(IUserRoleBusiness userRoleBusiness, IUserRoleUserBusiness userRoleUserBusiness,
            IUserRolePortalBusiness userRolePortalBusiness, IUserContext userContext, IPageBusiness pageBusiness,
            IUserBusiness userBusiness, IPortalBusiness portalBusiness)
        {
            _userRoleBusiness = userRoleBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _userRolePortalBusiness = userRolePortalBusiness;
            _userContext = userContext;
            _pageBusiness = pageBusiness;
            _userBusiness = userBusiness;
            _portalBusiness = portalBusiness;
        }
        public async Task<IActionResult> Index(string pageId, string portalId)
        {

            var model = new UserRoleViewModel { PortalId = portalId };
            if (pageId.IsNotNullAndNotEmpty())
            {
                var page = await _pageBusiness.GetSingleById(pageId);
                ViewBag.PageName = page.Title;
            }
            else
            {
                ViewBag.PageName = "";
            }
            return View(model);
        }


        public async Task<IActionResult> ReadData()
        {
            //var model = await _userRoleBusiness.GetList(x=>x.LegalEntityId== _userContext.LegalEntityId && x.PortalId== _userContext.PortalId);
            var model = await _userRoleBusiness.GetUserRolesWithPortalIds();
            return Json(model);
        }

        public IActionResult CreateUserRole(string portalId)
        {
            var model = new UserRoleViewModel
            {
                DataAction = DataActionEnum.Create,
                LegalEntityId = _userContext.LegalEntityId,
                PortalId = portalId,
                Portal = new List<string> { portalId },
                Status = StatusEnum.Active
                //Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            };
            return View("ManageUserRole", model);
        }

        public async Task<IActionResult> EditUserRole(string Id, string portalId)
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
                        if (model.Portals.IsNotNull())
                        {
                            foreach (var p in model.Portals)
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
                        return Json(new { success = false, error = result.HtmlError });
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
                        //ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = result.HtmlError });
                    }
                }
            }

            return Json(new { success = true, data = userRoleId });
        }

        [HttpGet]
        public async Task<ActionResult> GetUserRoleIdNameList()
        {
            //var data = await _userRoleBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            var data = await _userRoleBusiness.GetUserRolesWithPortalIds();
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToUserRole(string userRoleId, string userId)
        {
            var exist = await _userRoleUserBusiness.GetSingle(x => x.UserId == userId && x.UserRoleId == userRoleId);
            if (exist.IsNotNull())
            {
                return Json(new { success = false, msg = "User already added to this role" });
            }
            else
            {
                var model = new UserRoleUserViewModel()
                {
                    UserRoleId = userRoleId,
                    UserId = userId.Trim(),
                    LegalEntityId = _userContext.LegalEntityId,
                    PortalId = _userContext.PortalId
                };
                var result = await _userRoleUserBusiness.Create(model);
                return Json(new { success = true });
            }

        }

        public async Task<IActionResult> UserRoleMapping(string portalNames = null, string legalEntityId = null)
        {
            //var userroles = await _userRoleBusiness.GetList(x => x.PortalId == _userContext.PortalId);
            //userroles = userroles.OrderBy(x => x.Name).ToList();
            var model = new UserRoleUserViewModel();
            model.PortalId = _userContext.PortalId;

            //var cols = new List<string>();
            //foreach (var item in userroles)
            //{
            //    if (item.Name.IsNotNullAndNotEmpty())                   

            //    cols.Add(item.Name);
            //}
            //model.Columns = cols.ToArray();


            model.LegalEntityId = legalEntityId;
            if (model.LegalEntityId.IsNullOrEmpty())
            {
                var le = await _pageBusiness.GetSingle<LegalEntityViewModel, Synergy.App.DataModel.LegalEntity>
                    (x => x.Code == "CASABLANCA");
                model.LegalEntityId = le?.Id;
            }

            var userslist = await _userBusiness.GetList(x => x.LegalEntityIds.Contains(model.LegalEntityId));
            model.UserList = userslist;
            var portallist = await _portalBusiness.GetPortalListByPortalNames(portalNames);

            if (portallist.Count > 0)
            {
                string[] portalIds = portallist.Select(x => x.Id).ToArray();
                var userrolelist = await _userRoleBusiness.GetList(x => portalIds.Contains(x.PortalId));
                model.UserRoleList = userrolelist.OrderBy(x => x.Name).ToList();
            }

            if (userslist.Count > 0)
            {
                string[] userIds = userslist.Select(x => x.Id).ToArray();
                var userRoleUser = await _userRoleUserBusiness.GetList(x => userIds.Contains(x.UserId));
                model.UserRoleUserList = userRoleUser;
            }

            return View(model);
        }

        public async Task<IActionResult> ReadUserRoleMappingData(string portalId)
        {
            var userroles = await _userRoleUserBusiness.GetList(x => x.PortalId == portalId);
            var users = await _userBusiness.GetUsersWithPortalIds();

            var userroledata = new List<UserRoleUserViewModel>();
            foreach (var user in users)
            {
                var userroleuser = new UserRoleUserViewModel() { UserId = user.Id, UserName = user.Name };
                int i = 1;
                if (userroles.IsNotNull())
                {
                    foreach (var ur in userroles.Where(x => x.UserId == user.Id))
                    {
                        var Col1 = string.Concat("Check", i);
                        ApplicationExtension.SetPropertyValue(userroleuser, Col1, true);
                        i++;
                    }
                }
                userroledata.Add(userroleuser);
            }
            return Json(userroledata);
        }

        public async Task<IActionResult> ManageUserRoleMapping(string userRoleData)
        {
            try
            {                        
                var data = JsonConvert.DeserializeObject<List<UserRoleUserViewModel>>(userRoleData);
                var checkedList = data.Where(x => x.IsChecked == true).ToList();
                var unCheckedList = data.Where(x => x.IsChecked == false).ToList();

                foreach(var item in checkedList)
                {
                    var exist = await _userRoleUserBusiness.GetSingle(x => x.UserId == item.UserId && x.UserRoleId == item.UserRoleId );
                    if (exist.IsNotNull())
                    {
                        exist.IsDeleted = false;
                        await _userRoleUserBusiness.Edit(exist);
                    }
                    else
                    {
                        await _userRoleUserBusiness.Create(item);
                    }
                }
                foreach(var item in unCheckedList)
                {
                    var exist = await _userRoleUserBusiness.GetSingle(x => x.UserId == item.UserId && x.UserRoleId == item.UserRoleId);
                    if (exist.IsNotNull())
                    {
                        exist.IsDeleted = true;
                        await _userRoleUserBusiness.Edit(exist);
                    }
                }

                return Json(new { success = true });
            }
            catch(Exception e)
            {                
                return Json(new { success = false, error = e.Message});
            }
            
            
        }
    }
}
