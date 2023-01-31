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
    public class TeamController : ApplicationController

    {

        private ITeamBusiness _teamBusiness;
        private ITeamUserBusiness _teamuserBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IUserContext _userContext;
        private IPageBusiness _pageBusiness;
        private readonly IUserBusiness _userBusiness;

        public TeamController(ITeamBusiness teamBusiness,ITeamUserBusiness teamuserBusiness ,IPortalBusiness portalBusiness,
            IUserContext userContext, IPageBusiness pageBusiness, IUserBusiness userBusiness)
        {
            _teamBusiness = teamBusiness;
             _teamuserBusiness = teamuserBusiness;
            _portalBusiness = portalBusiness;
            _userContext = userContext;
            _pageBusiness = pageBusiness;
            _userBusiness = userBusiness;
        }
        public async Task<IActionResult> Index(string pageId)
        {
            if (pageId.IsNotNullAndNotEmpty())
            {
                var page = await _pageBusiness.GetSingleById(pageId);
                ViewBag.PageName = page.Title;
            }
            else 
            {
                ViewBag.PageName = "";
            }
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetTeamList()
        {
            var data = await _teamBusiness.GetList();
            var res = from d in data
                      where d.Status != StatusEnum.Inactive
                      select d;
            return Json(res);
        }
        //public async Task<JsonResult> GetModuleList()
        //{
        //    var data = await _moduleBusiness.GetList();
        //    return Json(data);
        //}
        public async Task<ActionResult> ReadData()
        {
            //var model = await _teamBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            var model = await _teamBusiness.GetTeamWithPortalIds();
            return Json(model);
        }
        public async Task<ActionResult> Create()
        {
            var portal = await _teamBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View("Manage", new TeamViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            });
        }
       /* public async Task<IActionResult> Edit(string Id)
        {
            var module = await _teamBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new TeamViewModel());
        }*/
        public async Task<IActionResult> Edit(string Id)
        {
            var portal = await _teamBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var team = await _teamBusiness.GetSingleById(Id);

            if (team != null)
            {
                
                var List = await _teamuserBusiness.GetList(x => x.TeamId == Id);
                if (List != null && List.Count() > 0)
                {
                    team.UserIds = List.Select(x => x.UserId).ToList();
                    team.TeamOwnerId=List.Where(x=>x.IsTeamOwner==true).Select(x => x.UserId).FirstOrDefault();
                }
                team.DataAction = DataActionEnum.Edit;
                return View("Manage", team);
            }
            return View("Manage", new TeamViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                  model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _teamBusiness.Create(model);
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
                    var result = await _teamBusiness.Edit(model);
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

            return View("Manage", model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _teamBusiness.Delete(id);
            return View("Index", new TeamViewModel());
        }
        //public async Task<IActionResult> Delete(string Id)
        //{
        //    await _teamBusiness.Delete(Id);
        //    return Json(true);
        //}
        //public ActionResult Delete(string id, string ru = null)
        //{
        //    ViewBag.Title = "Delete Task Work Time";
        //    var model = _moduleBusiness.GetSingle(x => x.Id == id);
        //    model.Operation = DataOperation.Delete;
        //    model.ReturnUrl = ru;
        //    var result = _moduleBusiness.Delete(model);
        //    if (!result.IsSuccess)
        //    {
        //        result.Message.Each(x => ModelState.AddModelError(x.Key, x.Value));
        //        return Json(new { success = false, errors = ModelState.SerializeErrors() });
        //    }
        //    else
        //    {
        //        return Json(new { success = true, operation = model.Operation.ToString(), Id = model.Id, ru = model.ReturnUrl });
        //    }
        //}
        public async Task<ActionResult> GetTeamByOwner()
        {
            var data =await _teamBusiness.GetTeamByOwner(_userContext.UserId);
            return Json(data);
        }
        public async Task<ActionResult> GetTeamMemberList(string teamId)
        {
            var data = await _teamBusiness.GetTeamMemberList(teamId);
            return Json(data);
        }
        public async Task<ActionResult> GetTeamByUser()
        {
            var data =await _teamBusiness.GetTeamByUser(_userContext.UserId);
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllowedPortals()
        {
            var data = await _portalBusiness.GetAllowedPortals();
            return Json(data);
        }

        public async Task<IActionResult> GetTeamUsersList(string selectedValues)
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
            return Json(list);            
        }
    }
}