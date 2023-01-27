using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class PageTemplateController : ApplicationController
    {
        IPageTemplateBusiness _pageTemplateBusiness;
        ITemplateBusiness _templateBusiness;
        ITableMetadataBusiness _tableBusiness;
        IColumnMetadataBusiness _columnBusiness;

        public PageTemplateController(IPageTemplateBusiness pageTemplateBusiness
            , ITemplateBusiness templateBusiness
            , ITableMetadataBusiness tableBusiness
            , IColumnMetadataBusiness columnBusiness)
        {
            _pageTemplateBusiness = pageTemplateBusiness;
            _templateBusiness = templateBusiness;
            _tableBusiness = tableBusiness;
            _columnBusiness = columnBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManagePage(string templateId)
        {
            var model = new PageTemplateViewModel();
            model.TemplateId = templateId;
            var temp = await _pageTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model.DataAction = DataActionEnum.Edit;
            }
            return View("_ManagePage", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePage(PageTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = JObject.Parse(model.Json);
                var comp = result["components"].ToString();
                JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {
                   var Pageresult= await _pageTemplateBusiness.Create(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Pageresult.Item.TemplateId });
                    }
                    else 
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                    //var res = await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Page });
                    //if (res.IsSuccess)
                    //{
                    //    await _pageTemplateBusiness.Create(model);
                    //}
                    //else
                    //{
                    //    return View("_ManagePage", model);
                    //}
                }
                //else if (model.DataAction == DataActionEnum.Edit)
                //{
                //    var res = await _templateBusiness.EditTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Page });
                //    if (!res.IsSuccess)
                //    {
                //        return View("_ManagePage", model);
                //    }
                //}
                var temp = await _templateBusiness.GetSingleById(model.TemplateId);
                temp.Json = model.Json;
                await _templateBusiness.Edit(temp);

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

      
    }
}
