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
    public class NoteTemplateController : ApplicationController
    {
        private readonly INoteTemplateBusiness _notetemplateBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        public NoteTemplateController(INoteTemplateBusiness noteTemplateBusiness, ITemplateBusiness templateBusiness)
        {
            _notetemplateBusiness = noteTemplateBusiness;
            _templateBusiness = templateBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPageJsonForm(string pageId)
        {
            var pagedata = await _notetemplateBusiness.GetSingleById(pageId);
            return Json(pagedata);
        }

        public async Task<IActionResult> ManageNote(string templateId)
        {
            var model = new NoteTemplateViewModel();
            model.TemplateId = templateId;
            var temp = await _notetemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            if (temp == null)
            {
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = temp;
                model.DataAction = DataActionEnum.Edit;
            }
            return View("_ManageNote", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageNote(NoteTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = JObject.Parse(model.Json);
                var comp = result["components"].ToString();
                JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {
                    //var res = await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Note });
                    //if (res.IsSuccess)
                    //{
                        var Noteresult=await _notetemplateBusiness.Create(model);
                        if (Noteresult.IsSuccess)
                        {
                            return Json(new { success = true, templateId = Noteresult.Item.TemplateId });
                        }
                   // }
                    else
                    {
                        ModelState.AddModelErrors(Noteresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    //var res = await _templateBusiness.EditTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Note });
                    //if (res.IsSuccess)
                    //{
                        var Noteresult = await _notetemplateBusiness.Edit(model);
                        if (Noteresult.IsSuccess)
                        {
                            return Json(new { success = true, templateId = Noteresult.Item.TemplateId });
                        }
                   // }
                    else
                    {
                        ModelState.AddModelErrors(Noteresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index", "Template");
        }
        public async Task<ActionResult> ReadAdhocNoteList()
        {
            var list = await _notetemplateBusiness.GetAdhocNoteTemplateList();
            return Json(list);

        }
    }
}
