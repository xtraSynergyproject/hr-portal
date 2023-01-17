using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class MenuGroupDetailsController : ApplicationController
    {
        private readonly IConfiguration _configuration;
        private readonly IMenuGroupDetailsBusiness _menugroupDetailsBusiness;
        private readonly ISubModuleBusiness _submoduleBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IMenuGroupBusiness _MenuGroupBusiness;


        public MenuGroupDetailsController(IConfiguration configuration
         , IMenuGroupDetailsBusiness menuGroupDetailsBusiness, ISubModuleBusiness subModuleBusiness,
         IPortalBusiness portalBusiness, IMenuGroupBusiness MenuGroupBusiness)
        {
            _configuration = configuration;
            _menugroupDetailsBusiness = menuGroupDetailsBusiness;
            _submoduleBusiness = subModuleBusiness;
            _portalBusiness = portalBusiness;
            _MenuGroupBusiness = MenuGroupBusiness;
        }
        public async Task<IActionResult> Index(string Id)
        {
            var model = new MenuGroupDetailsViewModel { MenuGroupId = Id };
            if (Id.IsNotNull())
            {
                var modelM = await _MenuGroupBusiness.GetSingleById(Id);
                if (modelM.IsNotNull())
                {
                    model.IconFileId = modelM.MenuGroupIconFileId;
                    model.ExpandHelpPanel = modelM.ExpandHelpPanel;
                }
            }
            return View(model);
        }


        public async Task<JsonResult> ReadMenuGroupDetailsData([DataSourceRequest] DataSourceRequest request,string MenugroupId)
        {
            var list = await _menugroupDetailsBusiness.GetList(x=>x.MenuGroupId==MenugroupId);
            var dsResult = list;
            //var dsResult = list.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<JsonResult> ReadMenuGroupDetails(string MenugroupId)
        {
            var list = await _menugroupDetailsBusiness.GetList(x => x.MenuGroupId == MenugroupId);
           
            return Json(list);
        }

        public IActionResult CreateMenuGroupDetails(string MenugroupId)
        {
            return View("ManageMenuGroupDetails", new MenuGroupDetailsViewModel
            {
                DataAction = DataActionEnum.Create,
                MenuGroupId = MenugroupId,

            }); ;
        }

        public async Task<IActionResult> EditMenuGroupDetails(string Id)
        {
            var member = await _menugroupDetailsBusiness.GetSingleById(Id);

            if (member != null)
            {
                member.DataAction = DataActionEnum.Edit;
                return View("ManageMenuGroupDetails", member);
            }
            return View("ManageMenuGroupDetails", new MenuGroupDetailsViewModel());
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _menugroupDetailsBusiness.Delete(id);
            //return View("Index", new MenuGroupViewModel());
            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> ManageMenuGroupDetails(MenuGroupDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _menugroupDetailsBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                        //return PopupRedirect("Menu Group created successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _menugroupDetailsBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                        //return PopupRedirect("Menu Group edited successfully", true);                        
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return Json(new { success = true });
        }


        [HttpPost]

        public async Task<IActionResult> Saveimg(string Id, string IconId,bool ExpanndeHelp)
        {
            var model = await _MenuGroupBusiness.GetSingleById(Id);
            if (model.IsNotNull())
            {
                model.MenuGroupIconFileId = IconId;
                model.ExpandHelpPanel = ExpanndeHelp;
                var Result = await _MenuGroupBusiness.Edit(model);
                if (Result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }

            }
            else {
                return Json(new { success = false });
            }

            

        }
    }
}
