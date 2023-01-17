using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserGroupController : ApplicationController
    {
        private IUserGroupBusiness _userGroupBusiness;
        private IUserGroupUserBusiness _userGroupUserBusiness;



        public UserGroupController(IUserGroupBusiness userGroupBusiness, IUserGroupUserBusiness userGroupUserBusiness)
        {
            _userGroupBusiness = userGroupBusiness;
            _userGroupUserBusiness = userGroupUserBusiness;

        }
        public IActionResult Index()
        {
            return View();
        }
      
        public async Task<ActionResult> ReadData()
        {
            var model = await _userGroupBusiness.GetList();
            return Json(model);
        }
        public IActionResult CreateUserGroup()
        {
            return View("ManageUserGroup", new UserGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                //Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            });
        }


        public async Task<IActionResult> EditUserGroup(string Id)
        {
            var member = await _userGroupBusiness.GetSingleById(Id);

            if (member != null)
            {
                var RoleList = await _userGroupUserBusiness.GetList(x => x.UserGroupId == Id);
                if (RoleList != null && RoleList.Count() > 0)
                {
                    member.UserIds = RoleList.Select(x => x.UserId).ToList();

                }
                //var userPortals = await _userRolePortalBusiness.GetPortalByUserRole(Id);
                //member.Portal = new List<string>();
                //if (userPortals.IsNotNull())
                //{
                //    foreach (var up in userPortals)
                //    {
                //        member.Portal.Add(up.PortalId);
                //    }
                //}
                member.DataAction = DataActionEnum.Edit;
                return View("ManageUserGroup", member);
            }
            return View("ManageUserGroup", new UserGroupViewModel());
        }

        public async Task<IActionResult> DeleteUserGroup(string Id)
        {


            await _userGroupBusiness.Delete(Id);
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserGroup(UserGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userGroupBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //Add UserPortal data
                        //if (model.Portal.IsNotNull())
                        //{
                        //    foreach (var p in model.Portal)
                        //    {
                        //        var res = await _userGroupUserBusiness.Create(new UserRolePortalViewModel
                        //        {
                        //            UserRoleId = result.Item.Id,
                        //            PortalId = p,
                        //        });
                        //    }
                        //}
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _userGroupBusiness.Edit(model);
                    if (result.IsSuccess)
                    {

                        //var portals = await _userRolePortalBusiness.GetPortalByUserRole(model.Id);

                        //if (portals.IsNotNull())
                        //{
                        //    foreach (var p in portals)
                        //    {
                        //        if (model.Portal == null || model.Portal.Contains(p.PortalId) == false)
                        //        {
                        //            await _userRolePortalBusiness.Delete(p.Id);
                        //        }
                        //    }
                        //}
                        //if (model.Portal.IsNotNull())
                        //{
                        //
                        //    foreach (var p in model.Portal)
                        //    {
                        //        if (!portals.Where(x => x.PortalId == p).Any())
                        //        {
                        //            var res = await _userRolePortalBusiness.Create(new UserRolePortalViewModel
                        //            {
                        //                UserRoleId = result.Item.Id,
                        //                PortalId = p,
                        //            });
                        //        }
                        //    }
                        //}
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageUserGroup", model);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserRoleIdNameList()
        {
            var data = await _userGroupBusiness.GetList();
            return Json(data);
        }

        public async Task<ActionResult> GetUserGroupList(string selectedValues)
        {
            List<UserGroupViewModel> list = new List<UserGroupViewModel>();
            if (selectedValues.IsNotNullAndNotEmpty())
            {
                var selectedValueslist = selectedValues.Split(",");
                if (selectedValueslist.Length > 0)
                {
                    foreach (var id in selectedValueslist)
                    {
                        var temp = await _userGroupBusiness.GetSingleById(id);
                        list.Add(temp);
                    }
                }
            }
            return Json(list);
        }
        public async Task<ActionResult> GetUserGroupIdNameList()
        {
            //List<TemplateViewModel> list = new List<TemplateViewModel>();
            var list = await _userGroupBusiness.GetList();
            return Json(list);
        }

    }
}
