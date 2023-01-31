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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    [Authorize(Policy = nameof(AuthorizeCMS))]
    public class ApplicationDocumentController : ApplicationController
    {
        private readonly IApplicationDocumentBusiness _applicationDocumentBusiness;
        public ApplicationDocumentController(IApplicationDocumentBusiness applicationDocumentBusiness)
        {
            _applicationDocumentBusiness = applicationDocumentBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplicationDocument()
        {
            var model = new ApplicationDocumentViewModel()
            {
                DataAction = DataActionEnum.Create
            };
            return View(model);
        }

        public async Task<JsonResult> ReadApplicationDocumentData()
        {
            var list = await _applicationDocumentBusiness.GetApplicationDocumentList();
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> ManageApplicationDocument(ApplicationDocumentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _applicationDocumentBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);                        
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _applicationDocumentBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> EditApplicationDocument(string Id)
        {
            var appdoc = await _applicationDocumentBusiness.GetSingleById(Id);

            if (appdoc != null)
            {
                appdoc.DataAction = DataActionEnum.Edit;
                return View("ApplicationDocument", appdoc);
            }
            return View("ApplicationDocument", new ApplicationDocumentViewModel());
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _applicationDocumentBusiness.Delete(id);            
            return Json(new { success = true });
        }
    }
}
