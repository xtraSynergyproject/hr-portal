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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    [Authorize(Policy = nameof(AuthorizeCMS))]
    public class FormTemplateController : ApplicationController
    {
        IFormTemplateBusiness _formTemplateBusiness;
        ITemplateBusiness _templateBusiness;
        public FormTemplateController(IFormTemplateBusiness formTemplateBusiness, ITemplateBusiness templateBusiness)
        {
            _formTemplateBusiness = formTemplateBusiness;
            _templateBusiness = templateBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManageForm(string templateId)
        {
            var model = new FormTemplateViewModel();
            model.TemplateId = templateId;
            var temp = await _formTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = temp;
                model.DataAction = DataActionEnum.Edit;
            }
            return View("_ManageForm", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageForm(FormTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = JObject.Parse(model.Json);
                var comp = result["components"].ToString();
                JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {                   
                    //var res = await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json,TemplateType=TemplateTypeEnum.Form });
                    //if(res.IsSuccess)
                    //{
                        var formresult = await _formTemplateBusiness.Create(model);
                        if (formresult.IsSuccess)
                        {
                            return Json(new { success = true, templateId=formresult.Item.TemplateId });
                        }
                   // }
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        //return View("_MangeForm",model);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {                   
                    //var res = await _templateBusiness.EditTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Form });
                    //if (res.IsSuccess)
                    //{
                       var formresult = await _formTemplateBusiness.Edit(model);
                        if (formresult.IsSuccess)
                        {
                            return Json(new { success = true, templateId = formresult.Item.TemplateId });
                        }
                    //}
                    else
                    {
                        ModelState.AddModelErrors(formresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        //return View("_MangeForm", model);
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index", "Template");
        }




    }
}
