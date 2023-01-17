using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class DocumentTypeController : ApplicationController
    {      
        private readonly IDocumentTypeBusiness _documentTypeBusiness;

        public DocumentTypeController(IDocumentTypeBusiness documentTypeBusiness)
        {           
            _documentTypeBusiness = documentTypeBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDocumentType(DocumentTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _documentTypeBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        //return PopupRedirect("Document Type created successfully");
                        return RedirectToAction("Index", "DocumentType");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _documentTypeBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Document Type edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View(model);
        }

    }
}
