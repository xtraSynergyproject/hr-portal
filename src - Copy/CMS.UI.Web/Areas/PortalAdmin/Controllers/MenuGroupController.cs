using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class MenuGroupController : ApplicationController
    {
        private readonly IConfiguration _configuration;
        private readonly IMenuGroupBusiness _menugroupBusiness;
        private readonly ISubModuleBusiness _submoduleBusiness;
        private readonly IPortalBusiness _portalBusiness;
       private static IUserContext _userContext;
        
        public MenuGroupController(IConfiguration configuration
          , IMenuGroupBusiness menuGroupBusiness, ISubModuleBusiness subModuleBusiness,
          IPortalBusiness portalBusiness, IUserContext userContext)
        {
            _configuration = configuration;
            _menugroupBusiness = menuGroupBusiness;
            _submoduleBusiness = subModuleBusiness;
            _portalBusiness = portalBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateMenuGroup(string source)
        {
            return View("ManageMenuGroup", new MenuGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                PortalId = _userContext.PortalId,
                LegalEntityId = _userContext.LegalEntityId,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                RequestSource= source
            }) ;
        }
        public async Task<IActionResult> EditMenuGroup(string Id,string source)
        {
            var member = await _menugroupBusiness.GetSingleById(Id);

            if (member != null)
            {
                member.DataAction = DataActionEnum.Edit;
                member.RequestSource = source;
                return View("ManageMenuGroup", member);
            }
            return View("ManageMenuGroup", new MenuGroupViewModel());
        }

        public async Task<JsonResult> ReadMenuGroupData()
        {
            var list = await _menugroupBusiness.GetMenuGroupListPortalAdmin(_userContext.PortalId,_userContext.LegalEntityId);
            //var dsResult = list.ToDataSourceResult(request);
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
            //  var data = await _menugroupBusiness.GetList(x => x.PortalId==_userContext.PortalId && x.LegalEntityId==_userContext.LegalEntityId && x.Id!=Id);

            var data = await _menugroupBusiness.GetMenuGroupListparentPortalAdmin(_userContext.PortalId, _userContext.LegalEntityId, Id);


            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetMenuGroupListByPortal(string portalId, string SubModuleId)
        {
            var data = await _menugroupBusiness.GetList(x => x.PortalId == portalId && x.SubModuleId == SubModuleId);
            return Json(data);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _menugroupBusiness.Delete(id);
            //return View("Index", new MenuGroupViewModel());
            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<JsonResult> GetPortalList()
        {
            var data = await _portalBusiness.GetList(x=>x.Id==_userContext.PortalId);
            return Json(data);
        }

    }
}
