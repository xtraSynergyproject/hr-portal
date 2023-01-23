using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class MenuGroupController : ApplicationController
    {
        private readonly IConfiguration _configuration;
        private readonly IMenuGroupBusiness _menugroupBusiness;
        private readonly ISubModuleBusiness _submoduleBusiness;
        private readonly IPortalBusiness _portalBusiness;

        public MenuGroupController(IConfiguration configuration
            , IMenuGroupBusiness menuGroupBusiness, ISubModuleBusiness subModuleBusiness,
            IPortalBusiness portalBusiness)
        {
            _configuration = configuration;
            _menugroupBusiness = menuGroupBusiness;
            _submoduleBusiness = subModuleBusiness;
            _portalBusiness = portalBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateMenuGroup(string portalId)
        {
            return View("ManageMenuGroup", new MenuGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                PortalId = portalId
            });
        }
        public async Task<IActionResult> EditMenuGroup(string Id)
        {
            var member = await _menugroupBusiness.GetSingleById(Id);

            if (member != null)
            {
                member.DataAction = DataActionEnum.Edit;
                return View("ManageMenuGroup", member);
            }
            return View("ManageMenuGroup", new MenuGroupViewModel());
        }

        public async Task<JsonResult> ReadMenuGroupData([DataSourceRequest] DataSourceRequest request)
        {
            var list = await _menugroupBusiness.GetMenuGroupList();
            var dsResult = list.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<JsonResult> ReadData()
        {
            var list = await _menugroupBusiness.GetMenuGroupList();          
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> ManageMenuGroup(MenuGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _menugroupBusiness.Create(model);
                    System.Diagnostics.Debug.WriteLine(result);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return PopupRedirect("Menu Group created successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _menugroupBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return PopupRedirect("Menu Group edited successfully", true);                        
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("ManageMenuGroup", model);
        }
        [HttpGet]
        public async Task<JsonResult> GetMenuGroupList(string Id)
        {
            var data = await _menugroupBusiness.GetList(x=>x.Id!=Id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetMenuGroupListByPortal(string id, string portalId, string SubModuleId)
        {
            if (id.IsNotNullAndNotEmpty()&&SubModuleId.IsNotNullAndNotEmpty())
            {

                var data = await _menugroupBusiness.GetList(x => x.PortalId == portalId && x.SubModuleId == SubModuleId);

                var result = data.Where(x => x.ParentId == id).Select(item => new
                {
                    id = item.Id,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    hasChildren = data.Where(x => x.ParentId == item.Id).Count() > 0 ? true : false,
                    //type = item.Name == "WORKSPACE_GENERAL" ? "workspace" : "folder",
                });
                return Json(result.ToList());
            }

            else if (id.IsNotNullAndNotEmpty() && SubModuleId.IsNullOrEmpty())
            {
                var data = await _menugroupBusiness.GetList(x => x.PortalId == portalId && x.ParentId == id);

                var result = data.Where(x => x.ParentId == id).Select(item => new
                {
                    id = item.Id,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    hasChildren = data.Where(x => x.ParentId == item.Id).Count() > 0 ? true : false,
                    //type = item.Name == "WORKSPACE_GENERAL" ? "workspace" : "folder",
                });
                return Json(result.ToList());
            }
            else
            {

                var data = await _menugroupBusiness.GetList(x => x.PortalId == portalId && x.SubModuleId == SubModuleId);

                var result = data.Where(x => x.ParentId == null).Select(item => new
                {
                    id = item.Id,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    hasChildren = data.Where(x => x.ParentId == item.Id).Count() > 0 ? true : false,
                    //type = item.Name == "WORKSPACE_GENERAL" ? "workspace" : "folder",
                });
                return Json(result.ToList());
            }
            
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _menugroupBusiness.Delete(id);
            //return View("Index", new MenuGroupViewModel());
            return Json(new { success = true });
        }
    }
}
