using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class UserGroupController : ApplicationController
    {
        private IUserGroupBusiness _userGroupBusiness;
        private IUserGroupUserBusiness _userGroupUserBusiness;
        private IUserContext _userContext;
        private readonly IPortalBusiness _portalBusiness;


        public UserGroupController(IUserGroupBusiness userGroupBusiness, IUserContext userContext, IUserGroupUserBusiness userGroupUserBusiness,IPortalBusiness portalBusiness)
        {
            _userGroupBusiness = userGroupBusiness;
            _userGroupUserBusiness = userGroupUserBusiness;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReadData()
        {
            //var model = await _userGroupBusiness.GetList(x=>x.PortalId== _userContext.PortalId && x.LegalEntityId== _userContext.LegalEntityId);
            var model = await _userGroupBusiness.GetTeamWithPortalIds();
            return Json(model);
        }
        public async Task<IActionResult> CreateUserGroup()
        {
            var portal = await _userGroupBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View("ManageUserGroup", new UserGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                PortalId = _userContext.PortalId ,
                LegalEntityId = _userContext.LegalEntityId
                //Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            });
            
        }


        public async Task<IActionResult> EditUserGroup(string Id)
        {
            var portal = await _userGroupBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var member = await _userGroupBusiness.GetSingleById(Id);

            if (member != null)
            {
                var RoleList = await _userGroupUserBusiness.GetList(x => x.UserGroupId == Id);
                if (RoleList != null && RoleList.Count() > 0)
                {
                    member.UserIds = RoleList.Select(x => x.UserId).Distinct().ToList();

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
                    var result = await _userGroupBusiness.CreateFromPortal(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
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
                        return Json(new { success = false,error=ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _userGroupBusiness.EditFromPortal(model);
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
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        [HttpGet]
        public async Task<ActionResult> GetUserRoleIdNameList()
        {
            var data = await _userGroupBusiness.GetList(x=> x.PortalId == _userContext.PortalId && x.LegalEntityId == _userContext.LegalEntityId);
            return Json(data);
        }

        public async Task<ActionResult> GetUserGroupList([DataSourceRequest] DataSourceRequest request, string selectedValues)
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
            return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetUserGroupIdNameList()
        {
            //List<TemplateViewModel> list = new List<TemplateViewModel>();
            //var list = await _userGroupBusiness.GetList(x=>x.PortalId == _userContext.PortalId && x.LegalEntityId == _userContext.LegalEntityId);
            var list = await _userGroupBusiness.GetList(x=>x.LegalEntityId == _userContext.LegalEntityId);
            var list1 = list.Where(x => x.AllowedPortalIds.Contains(_userContext.PortalId)).ToList();
            return Json(list1);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllowedPortals()
        {
            var data = await _portalBusiness.GetAllowedPortals();
            return Json(data);
        }

    }
}
