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
using Newtonsoft.Json.Linq;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class TaskTemplateController : ApplicationController
    {
        ITemplateBusiness _templateBusiness;
        IRecTaskTemplateBusiness _taskTemplateBusiness;
        public TaskTemplateController(ITemplateBusiness templateBusiness, IRecTaskTemplateBusiness taskTemplateBusiness)
        {
            _templateBusiness = templateBusiness;
            _taskTemplateBusiness = taskTemplateBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult>  Manage(string Id)
        {
            RecTaskTemplateViewModel model = new RecTaskTemplateViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                model = await _taskTemplateBusiness.GetSingleById(Id);
                model.DataAction = DataActionEnum.Edit;
            }
            else 
            {
                model.DataAction = DataActionEnum.Create;
            }
          
            return View("ManageTaskTemplate", model);
        }
        [HttpPost]
        public async Task<ActionResult> Manage(RecTaskTemplateViewModel model)
        {

            if (ModelState.IsValid)
            {
                //  var result = JObject.Parse(model.Json);
                //   var comp = result["components"].ToString();
                // JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {

                    var Taskresult = await _taskTemplateBusiness.ManageCreate(model);
                    if (Taskresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                    }

                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {


                    var Taskresult = await _taskTemplateBusiness.ManageEdit(model);
                    if (Taskresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Taskresult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Taskresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index", "Template");
        }

        [HttpGet]
        public async Task<IActionResult> GetPageJsonForm(string templateId)
        {
            var pagedata = await _templateBusiness.GetSingleById(templateId);
            return Json(pagedata.Json);
        }

        [HttpPost]
        public async Task<ActionResult> ManageTask(RecTaskTemplateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = JObject.Parse(model.Json);
                var comp = result["components"].ToString();
                JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {

                    //var res=await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Task });
                    //if (res.IsSuccess)
                    //{
                    var Taskresult = await _taskTemplateBusiness.Create(model);
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

                    //var res= await _templateBusiness.EditTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Task });
                    // if (res.IsSuccess)
                    // {
                    var Taskresult = await _taskTemplateBusiness.Edit(model);
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
                    //var efield = FindNonExist(rows, new List<string>());
                    //var param = new Dictionary<string, object>
                    //{
                    //    { "Id", model.Id }
                    //};
                    //var list = await _columnBusiness.GetList(x => x.TableMetadataId == model.TableMetadataId);
                    //var notexist = list.Where(x => !efield.Contains(x.Id));
                    //foreach (var item in notexist)
                    //{
                    //    await _columnBusiness.Delete(item.Id);
                    //}
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index", "Template");
        }
        public async Task<ActionResult> ReadTaskTemplateData()
        {
            var model = await _taskTemplateBusiness.GetList();
           // var dsResult = model.ToDataSourceResult(request);
            return Json(model);
        }

        public async Task<ActionResult> ReadEmailData()
        {
            var model = await _taskTemplateBusiness.GetEmailSetting();
            
            return Json(model);
        }

        public async Task<ActionResult> ReadData()
        {
            var list = await _taskTemplateBusiness.GetTaskTemplateList();            
            return Json(list);

        }
    }
}
