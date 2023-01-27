using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class SettingsController : Controller
    {
        private readonly ISettingsBusiness _seetingsBusiness;
        private readonly IDocumentTypeBusiness _documentTypeBusiness;
        private readonly ICompositionBusiness _compositionBusiness;
        IEditorTypeBusiness _editorTypeBusiness;
        public SettingsController(ISettingsBusiness seetingsBusiness,
           IDocumentTypeBusiness documentTypeBusiness,
           ICompositionBusiness compositionBusiness
            , IEditorTypeBusiness editorTypeBusiness)
        {
            _seetingsBusiness = seetingsBusiness;
            _documentTypeBusiness = documentTypeBusiness;
            _compositionBusiness = compositionBusiness;
            _editorTypeBusiness = editorTypeBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DocumentTypeDesign()
        {
            var model = new DocumentTypeViewModel();
            //model.Groups = new List<DocumentGroup> {
            //    { new DocumentGroup{ } },
            //    { new DocumentGroup{ } },
            //    { new DocumentGroup{ } }
            //};
            model.EditorTypeList= _editorTypeBusiness.GetList().Result.ToList();
            return View(model);
        }

        public ActionResult ReadCustomValidateData([DataSourceRequest] DataSourceRequest request)
        {
            var data = new List<IdNameViewModel>();
            data.Add(new IdNameViewModel { Id = "0", Name = "No Validation" });
            data.Add(new IdNameViewModel { Id = "1", Name = "Validate as an email address" });
            data.Add(new IdNameViewModel { Id = "2", Name = "Validate as a number" });
            data.Add(new IdNameViewModel { Id = "3", Name = "Validate as a URL" });
            data.Add(new IdNameViewModel { Id = "4", Name = "...or enter a custom validation" });
            //var dsResult = data.ToDataSourceResult(request);
            //return Json(dsResult);
            return Json(data);
        }

        //public async Task<IActionResult> GetDocumentGroupList([DataSourceRequest] DataSourceRequest request)
        //{

        //    var data =
        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}

        public async Task<JsonResult> GetDocumentTypeTreeList(string id)
        {
            var result = await _seetingsBusiness.GetDocumentTypeTreeList(id);
            return Json(result.ToList());
        }

    }
}
