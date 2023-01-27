using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class TemplateStageController : ApplicationController

    {

        private ITemplateStageBusiness _templateStageBusiness;
        private readonly IPortalBusiness _portalBusiness;


        public TemplateStageController(ITemplateStageBusiness templateStageBusiness, IPortalBusiness portalBusiness)
        {
            _templateStageBusiness = templateStageBusiness;
            _portalBusiness = portalBusiness;
        }




        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetTemplateStageList(string portalId)
        {
            var data = await _templateStageBusiness.GetList(x=>x.StageType== TemplateStageTypeEnum.Stage);
            if (portalId.IsNotNullAndNotEmpty())
            {
                return Json(data.Where(x => x.PortalId == portalId));
            }
            return Json(data);
        }
        public async Task<JsonResult> GetTemplateStepList(string portalId)
        {
            var data = await _templateStageBusiness.GetList(x => x.StageType == TemplateStageTypeEnum.Step);
            if (portalId.IsNotNullAndNotEmpty())
            {
                return Json(data.Where(x => x.PortalId == portalId));
            }
            return Json(data);
        }
        //public async Task<JsonResult> GetModuleList()
        //{
        //    var data = await _moduleBusiness.GetList();
        //    return Json(data);
        //}
       
      
        public async Task<ActionResult> ReadData()
        {
            var model = await _templateStageBusiness.GetList();
            return Json(model);
        }
        public IActionResult Create()
        {
            return View("Manage", new TemplateStageViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var module = await _templateStageBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new TemplateStageViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(TemplateStageViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _templateStageBusiness.Create(model);
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
                    var result = await _templateStageBusiness.Edit(model);
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
            await _templateStageBusiness.Delete(id);
            return Json(true);
        }

    }
}