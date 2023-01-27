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

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class LegalEntityController : ApplicationController
    {
        private ILegalEntityBusiness _legalEntityBusiness;
        private readonly IDocumentBusiness _documentBusiness;
        private static IUserContext _userContext;
        private readonly IFileBusiness _fileBusiness;
        public LegalEntityController(ILegalEntityBusiness legalEntityBusiness, IDocumentBusiness documentBusiness
            , IUserContext userContext
            , IFileBusiness fileBusiness)
        {
            _legalEntityBusiness = legalEntityBusiness;
            _documentBusiness = documentBusiness;
            _userContext = userContext;
            _fileBusiness = fileBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ReadData()
        {
            var model = await _legalEntityBusiness.GetData();
            var data = model.Where(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId).ToList();
            //var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        public IActionResult Create()
        {
            var model = new LegalEntityViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            };
            model.PortalId = _userContext.PortalId;
            model.LegalEntityId = _userContext.LegalEntityId;
            return View("Manage", model);
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
        public async Task<IActionResult> Save(IList<IFormFile> files)
        {
            try
            {
                //foreach (var file in files)
                //{
                //    var ms = new MemoryStream();
                //    file.OpenReadStream().CopyTo(ms);
                //    var result = await _documentBusiness.Create(new DocumentViewModel
                //    {
                //        Content = ms.ToArray(),
                //        ContentType = file.ContentType,
                //        Length = file.Length,
                //        Name = file.FileName
                //    }
                //    );
                //    if (result.IsSuccess)
                //    {
                //        Response.Headers.Add("fileId", result.Item.Id);
                //        Response.Headers.Add("fileName", result.Item.Name);
                //        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.Name });
                //    }
                //    else
                //    {
                //        Response.Headers.Add("status", "false");
                //        return Content("");
                //    }
                //}

                foreach (var file in files)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
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
            data = data.Where(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId).ToList();
            return Json(data);
        }

    }
}
