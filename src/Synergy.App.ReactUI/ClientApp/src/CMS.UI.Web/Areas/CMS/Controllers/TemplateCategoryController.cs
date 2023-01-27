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
    public class TemplateCategoryController : ApplicationController

    {

        private ITemplateCategoryBusiness _templatecategoryBusiness;
        private readonly IPortalBusiness _portalBusiness;


        public TemplateCategoryController(ITemplateCategoryBusiness templatecategoryBusiness, IPortalBusiness portalBusiness)
        {
            _templatecategoryBusiness = templatecategoryBusiness;
            _portalBusiness = portalBusiness;
        }




        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetNtsCategoryList()
        {
            var data = await _templatecategoryBusiness.GetList();
            return Json(data);
        }
        public IActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = _templatecategoryBusiness.GetList();
            var data = model.Result;

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public async Task<JsonResult> GetCategoryList(string id, string CatId, TemplateTypeEnum Type,string portalId=null)
        {

            if (id.IsNotNullAndNotEmpty())
            {
                var data = await _templatecategoryBusiness.GetList(x => x.Id != CatId && x.TemplateType == Type);
                if (portalId.IsNotNullAndNotEmpty())
                {
                    data = data.Where(x => x.PortalId == portalId).ToList();
                }
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
            else {
                var data = await _templatecategoryBusiness.GetList(x => x.Id != CatId && x.TemplateType == Type );
                if (portalId.IsNotNullAndNotEmpty())
                {
                    data = data.Where(x => x.PortalId == portalId).ToList();
                }
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

        public IActionResult Create(TemplateTypeEnum type, LayoutModeEnum lo, string cbm,string portalId)
        {
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            return View("Manage", new TemplateCategoryViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                TemplateType = type,
                LayoutMode=lo,
                PopupCallbackMethod=cbm,
                PortalId= portalId
                //GroupPortals=
            });           
        }
        public async Task<IActionResult> Edit(string id, LayoutModeEnum lo, string cbm)
        {
            var module = await _templatecategoryBusiness.GetSingleById(id);
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            }
            ViewBag.CallbackMethod = cbm;
            ViewBag.LayoutMode = Convert.ToString(lo);
            if (module != null)
            {
                module.DataAction = DataActionEnum.Edit;
                module.LayoutMode = lo;
                module.PopupCallbackMethod = cbm;
                return View("Manage", module);
            }
            return View("Manage", new TemplateCategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(TemplateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _templatecategoryBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        if (model.LayoutMode == LayoutModeEnum.Popup && model.PopupCallbackMethod.IsNotNullAndNotEmpty())
                        {
                            return Json(new { success = true, templateCategoryId = result.Item.Id });
                        }
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _templatecategoryBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        if (model.LayoutMode == LayoutModeEnum.Popup && model.PopupCallbackMethod.IsNotNullAndNotEmpty())
                        {
                            return Json(new { success = true, templateCategoryId = result.Item.Id });
                        }
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return RedirectToAction("index", "Template");
        }
        [HttpGet]
        public async Task<JsonResult> GetPortalList()
        {
            //var data = await _portalBusiness.GetPortalForUser(id);
            var data = await _portalBusiness.GetPortals(null);
            return Json(data);
        }
    }
}