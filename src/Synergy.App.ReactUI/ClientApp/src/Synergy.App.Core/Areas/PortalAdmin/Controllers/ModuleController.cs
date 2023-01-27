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

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class ModuleController : ApplicationController
    {

        private IModuleBusiness _moduleBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private static IUserContext _userContext;
        public ModuleController(IModuleBusiness moduleBusiness, IPortalBusiness portalBusiness, IUserContext userContext)
        {
            _moduleBusiness = moduleBusiness;
            _portalBusiness = portalBusiness;
            _userContext = userContext;
        }




        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetModuleList()
        {
            var data = await _moduleBusiness.GetList(x=>x.LegalEntityId==_userContext.LegalEntityId && x.PortalId==_userContext.PortalId);
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
            var model = await _moduleBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId &&  x.PortalId == _userContext.PortalId);
              return Json(model);
        }
        public IActionResult Create()
        {
            return View("Manage", new ModuleViewModel
            {
                DataAction = DataActionEnum.Create,
                PortalId = _userContext.PortalId,
                LegalEntityId = _userContext.LegalEntityId,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var module = await _moduleBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new ModuleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _moduleBusiness.Create(model);
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
                    var result = await _moduleBusiness.Edit(model);
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
            await _moduleBusiness.Delete(id);
            return View("Index", new ModuleViewModel());
        }

    }
}
