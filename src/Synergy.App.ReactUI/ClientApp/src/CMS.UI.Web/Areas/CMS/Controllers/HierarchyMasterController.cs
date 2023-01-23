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
    public class HierarchyMasterController : ApplicationController
    {
        private IUserBusiness _userBusiness;
        private IHierarchyMasterBusiness _hierarchyMasterBusiness;
        private ITeamBusiness _teamBusiness;
        private ITeamUserBusiness _teamUserBusiness;
        private IHRCoreBusiness _hRCoreBusiness;
        public HierarchyMasterController(IUserBusiness userBusiness, ITeamBusiness teamBusiness, ITeamUserBusiness teamUserBusiness,
            IHierarchyMasterBusiness hierarchyMasterBusiness, IHRCoreBusiness hRCoreBusiness)
        {
            _userBusiness = userBusiness;
            _teamBusiness = teamBusiness;
            _teamUserBusiness = teamUserBusiness;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _hRCoreBusiness = hRCoreBusiness;

        }
        public IActionResult Index()
        {
            return View();
        }


        //public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = _hierarchyMasterBusiness.GetList();
        //    var data = model.Result.ToList();
        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
        public async Task<ActionResult> ReadData()
        {
            var model =await _hierarchyMasterBusiness.GetList();    
            return Json(model);
        }

        public IActionResult CreateHierarchy()
        {
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
                list = await _userBusiness.GetList();
            }
            else 
            {
                if (teamId.IsNotNullAndNotEmpty())
                {
                    var team = await _teamBusiness.GetSingleById(teamId);
                    var teamUsers = await _teamUserBusiness.GetList(x => x.TeamId == teamId);
                    foreach (var id in teamUsers)
                    {
                        var user = await _userBusiness.GetSingleById(id.UserId);
                        list.Add(user);
                    }                    
                }
            }
            return Json(list);
        }

        [HttpGet]
        public async Task<ActionResult> GetRootNodeList(HierarchyTypeEnum type)
        {
            
            if (type == HierarchyTypeEnum.User)
            {
               var  data = await _userBusiness.GetUserIdNameList();
                return Json(data);
            }
            else if (type == HierarchyTypeEnum.Organization)
            {
                var data = await _hRCoreBusiness.GetAllOrganisation();
                return Json(data);
            }
            else if (type == HierarchyTypeEnum.Position)
            {
                var data = await _hRCoreBusiness.GetAllPosition();
                return Json(data);
            }
            else
            {
                var data = new List<IdNameViewModel>();
                return Json(data);
            }
          
        }
    }
}
