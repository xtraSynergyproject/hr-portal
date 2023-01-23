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

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    
    [Area("Cms")]
    public class SubModuleController : ApplicationController
    {
        private readonly ISubModuleBusiness _submoduleBusiness;
        private readonly IModuleBusiness _moduleBusiness;

        public SubModuleController(ISubModuleBusiness subModuleBusiness, IModuleBusiness moduleBusiness)
        {
            _submoduleBusiness = subModuleBusiness;
            _moduleBusiness = moduleBusiness;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetSubModuleList(string portalId,string moduleId)
        {
            var data = await _submoduleBusiness.GetList(x=> x.Status != StatusEnum.Inactive);
            if (portalId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.PortalId == portalId).ToList();
            }
            if (moduleId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.ModuleId == moduleId).ToList();
            }
            //var res = from d in data
            //          where d.Status != StatusEnum.Inactive
            //          select d;
            return Json(data);
        }
        //public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        // {
        //     var model = _submoduleBusiness.GetSubModuleList();
        //     var data = model.Result;

        //     var dsResult = data.ToDataSourceResult(request);
        //     return Json(dsResult);
        // }


      
        public async Task<ActionResult> ReadData()
        {
            var model = await _submoduleBusiness.GetSubModuleList();
            return Json(model);
        }

        public IActionResult Create()
        {
            return View("Manage", new SubModuleViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var module = await _submoduleBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new SubModuleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(SubModuleViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _submoduleBusiness.Create(model);
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
                    var result = await _submoduleBusiness.Edit(model);
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
            await _submoduleBusiness.Delete(id);
            return View("Index", new SubModuleViewModel());
        }


    }
}
