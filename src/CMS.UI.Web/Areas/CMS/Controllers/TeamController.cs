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
    public class TeamController : ApplicationController

    {
        private IUserBusiness _userBusiness;
        private ITeamBusiness _teamBusiness;
        private ITeamUserBusiness _teamuserBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IUserContext _userContext;

        public TeamController(ITeamBusiness teamBusiness,ITeamUserBusiness teamuserBusiness ,IPortalBusiness portalBusiness,
            IUserContext userContext,IUserBusiness userBusiness)
        {
            _teamBusiness = teamBusiness;
             _teamuserBusiness = teamuserBusiness;
            _portalBusiness = portalBusiness;
            _userContext = userContext;
            _userBusiness = userBusiness;
        }




        public IActionResult Index()
        {
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
            var model = await _teamBusiness.GetList();
            return Json(model);
        }
        public IActionResult Create()
        {
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
            //if (selectedValues.IsNotNullAndNotEmpty())
            //    {
            //        var selectedValueslist = selectedValues.Split(",");
            //        if (selectedValueslist.Length > 0)
            //        {
            //            foreach (var id in selectedValueslist)
            //            {
            //                var user = await _userBusiness.GetSingleById(id);
            //                list.Add(user);
            //            }
            //        }
            //    }
            //    list = await _teamuserBusiness.GetList(x => x.TeamId == selectedValues);
            //    if (list.Count() > 0)
            //    {
            //        foreach (var item in list)
            //        {
            //            var user = await _userBusiness.GetSingleById(item.UserId);
            //            item.UserName = user.Name;
            //        }
            //    }
            //}
            //return Json(list);
        }
        [HttpGet]
        public async Task<ActionResult> GetIdNameListByGroupCode(string groupCode)
        {
            var data = await _teamBusiness.GetTeamByGroupCode(groupCode);
            return Json(data);
        }
        public async Task<ActionResult> GetTeamOwner(string teamId)
        {
            var ownerUser= await _teamBusiness.GetTeamOwner(teamId);
            return Json(ownerUser.Id);
        }
        public async Task<ActionResult> GetTeamsBasedOnAllowedPortals()
        {
            var data = await _teamBusiness.GetTeamsBasedOnAllowedPortals(_userContext.PortalId);
            return Json(data);
        }
    }
}