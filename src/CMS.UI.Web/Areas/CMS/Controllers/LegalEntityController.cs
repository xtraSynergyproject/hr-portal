using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class LegalEntityController : ApplicationController

    {

        private ILegalEntityBusiness _legalEntityBusiness;
        private readonly IDocumentBusiness _documentBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;


        public LegalEntityController(ILegalEntityBusiness legalEntityBusiness, IDocumentBusiness documentBusiness, IHRCoreBusiness hrCoreBusiness)
        {
            _legalEntityBusiness = legalEntityBusiness;
            _documentBusiness = documentBusiness;
            _hrCoreBusiness = hrCoreBusiness;
        }




        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = _legalEntityBusiness.GetData();
            var data = model.Result;

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public ActionResult ReadLegalEntityData()
        {
            var model = _legalEntityBusiness.GetData();
            var data = model.Result;
            return Json(data);
        }




        public IActionResult Create()
        {
            return View("Manage", new LegalEntityViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var module = await _legalEntityBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new LegalEntityViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(LegalEntityViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _legalEntityBusiness.Create(model);
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
                    var result = await _legalEntityBusiness.Edit(model);
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
            await _legalEntityBusiness.Delete(id);
            return View("Index", new LegalEntityViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(IList<IFormFile> file)
        {
            try
            {
                foreach (var f in file)
                {
                    var ms = new MemoryStream();
                    f.OpenReadStream().CopyTo(ms);
                    var result = await _documentBusiness.Create(new DocumentViewModel
                    {
                        Content = ms.ToArray(),
                        ContentType = f.ContentType,
                        Length = f.Length,
                        Name = f.FileName
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.Name);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.Name });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        [HttpGet]
        public async Task<ActionResult> GetIdNameList()
        {
            var data = await _legalEntityBusiness.GetData();

            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetNameList()
        {
            var data = await _legalEntityBusiness.GetList();

            return Json(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetCountryList()
        {
            var data = await _hrCoreBusiness.GetCountryList();


            return Json(data);
        }




    }
}