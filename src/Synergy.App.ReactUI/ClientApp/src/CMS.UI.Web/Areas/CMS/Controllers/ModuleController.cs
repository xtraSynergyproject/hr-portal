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
    public class ModuleController : ApplicationController

    {

        private IModuleBusiness _moduleBusiness;
        private readonly IPortalBusiness _portalBusiness;


        public ModuleController(IModuleBusiness moduleBusiness, IPortalBusiness portalBusiness)
        {
            _moduleBusiness = moduleBusiness;
            _portalBusiness = portalBusiness;
        }




        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetModuleList(string portalId)
        {
            var data = await _moduleBusiness.GetList(x=>x.Status != StatusEnum.Inactive);
            if (portalId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.PortalId == portalId).ToList();
            }
            //var res = from d in data
            //          where d.Status != StatusEnum.Inactive
            //          select d;
            return Json(data);
        }
        //public async Task<JsonResult> GetModuleList()
        //{
        //    var data = await _moduleBusiness.GetList();
        //    return Json(data);
        //}
        public ActionResult ReadData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var model = _moduleBusiness.GetList();
            var data = model.Result;

            //var dsResult = data.ToDataSourceResult(request);
            //return Json(dsResult);
            return Json(data);
        }
        public IActionResult Create()
        {
            return View("Manage", new ModuleViewModel
            {
                DataAction = DataActionEnum.Create,
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
        public async Task<ActionResult> GetPortalList()
        {
            var list = await _portalBusiness.GetList();
            return Json(list);
        }
        [HttpGet]
        public JsonResult GetColumnName()
        {
            var data = new List<IdNameViewModel>();
            data.Add(new IdNameViewModel { Name = "Name" });
            data.Add(new IdNameViewModel { Name = "ShortName" });
            data.Add(new IdNameViewModel { Name = "Prefix" });
            return Json(data);
        }
    }
}