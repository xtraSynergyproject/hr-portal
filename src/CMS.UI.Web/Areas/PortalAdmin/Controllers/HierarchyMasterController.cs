using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class HierarchyMasterController : ApplicationController
    {
        private IUserBusiness _userBusiness;
        private IHierarchyMasterBusiness _hierarchyMasterBusiness;
        private ITeamBusiness _teamBusiness;
        private ITeamUserBusiness _teamUserBusiness;
        private static IUserContext _userContext;
        public HierarchyMasterController(IUserBusiness userBusiness, ITeamBusiness teamBusiness, ITeamUserBusiness teamUserBusiness,
            IHierarchyMasterBusiness hierarchyMasterBusiness, IUserContext userContext)
        {
            _userBusiness = userBusiness;
            _teamBusiness = teamBusiness;
            _teamUserBusiness = teamUserBusiness;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _userContext = userContext;

        }
        public IActionResult Index()
        {
        return View();
        }
       public async Task<ActionResult> ReadData()
            {
                var model = await _hierarchyMasterBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
                return Json(model);
            }
        

        public async Task<IActionResult> CreateHierarchy()
        {
            var portal = await _hierarchyMasterBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View("ManageHierarchy", new HierarchyMasterViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),               
            });
        }

        public async Task<IActionResult> DeleteHierarchy(string Id)
        {
            await _hierarchyMasterBusiness.Delete(Id);
            return Json(true);
        }

        public async Task<IActionResult> EditHierarchy(string Id)
        {
            var portal = await _hierarchyMasterBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var member = await _hierarchyMasterBusiness.GetSingleById(Id);

            if (member != null)
            {                
                //member.ConfirmPassword = member.Password;
                member.DataAction = DataActionEnum.Edit;
                return View("ManageHierarchy", member);
            }
            return View("ManageHierarchy", new HierarchyMasterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageUser(HierarchyMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _hierarchyMasterBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {                                    
                    var result = await _hierarchyMasterBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageHierarchy", model);
        }
        [HttpGet]
        public async Task<ActionResult> GetUserIdNameList()
        {
            var data =await _userBusiness.GetUserIdNameList();
            return Json(data);
        }
        public async Task<ActionResult> GetUsersList([DataSourceRequest] DataSourceRequest request,string selectedValues)
        {
            List<UserViewModel> list = new List<UserViewModel>();
            if (selectedValues.IsNotNullAndNotEmpty())
            {
                var selectedValueslist = selectedValues.Split(",");
                if (selectedValueslist.Length > 0)
                {
                    foreach (var id in selectedValueslist)
                    {
                        var user = await _userBusiness.GetSingleById(id);
                        list.Add(user);
                    }
                }
            }
            return Json(list.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetUsersListOnTypeBasis([DataSourceRequest] DataSourceRequest request, string assigneeType, string teamId)
        {
            List<UserViewModel> list = new List<UserViewModel>();
            if (assigneeType == TaskAssignedToTypeEnum.User.ToString())
            {
                list = await _userBusiness.GetList(x=>x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            }
            else 
            {
                if (teamId.IsNotNullAndNotEmpty())
                {
                    var team = await _teamBusiness.GetSingleById(teamId);
                    var teamUsers = await _teamUserBusiness.GetList(x => x.TeamId == teamId && x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
                    foreach (var id in teamUsers)
                    {
                        var user = await _userBusiness.GetSingleById(id.UserId);
                        list.Add(user);
                    }                    
                }
            }
            return Json(list);
        }
    }
}
