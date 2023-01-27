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
using Newtonsoft.Json.Linq;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class ServiceTemplateController : ApplicationController
    {
        ITemplateBusiness _templateBusiness;
        IServiceTemplateBusiness _serviceTemplateBusiness;
        public ServiceTemplateController(ITemplateBusiness templateBusiness, IServiceTemplateBusiness serviceTemplateBusiness)
        {
            _templateBusiness = templateBusiness;
            _serviceTemplateBusiness = serviceTemplateBusiness;
        }
        [HttpPost]
        public async Task<ActionResult> ManageService(ServiceTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = JObject.Parse(model.Json);
                var comp = result["components"].ToString();
                JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {

                    //var res = await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Task });
                    //if (res.IsSuccess)
                    //{
                        var Taskresult = await _serviceTemplateBusiness.Create(model);
                        if (Taskresult.IsSuccess)
                        {
                            return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                        }
                    //}
                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {

                    //var res = await _templateBusiness.EditTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Task });
                    //if (res.IsSuccess)
                    //{
                        var Taskresult = await _serviceTemplateBusiness.Edit(model);
                        if (Taskresult.IsSuccess)
                        {
                            return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                        }
                   // }
                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                   
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });          
        }

        public IActionResult Manage()
        {
            return View();
        }
        public async Task<ActionResult> ReadAdhocServiceList()
        {
            var list = await _serviceTemplateBusiness.GetAdhocServiceTemplateList();
            return Json(list);

        }


    }
}
