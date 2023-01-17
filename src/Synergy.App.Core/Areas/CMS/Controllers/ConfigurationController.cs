using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
////using Kendo.Mvc.UI;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Synergy.App.Business;
////using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class ConfigurationController : ApplicationController
    {
        IEditorTypeBusiness _editorTypeBusiness;

        public ConfigurationController(IEditorTypeBusiness editorTypeBusiness)
        {
            _editorTypeBusiness=editorTypeBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EditorType()
        {
            var model = _editorTypeBusiness.GetList().Result.ToList();

            return View(model);
        }
        public ActionResult ReadEditorTypeData([DataSourceRequest] DataSourceRequest request)
        {
            var model = _editorTypeBusiness.GetList();
            var data = model.Result.ToList();
            //var data = new List<EditorTypeViewModel>();
            //data.Add(new EditorTypeViewModel { Name="Test01", EditorCategory=EditorCategoryEnum.Common, ControlType=ControlTypeEnum.TextBox});
            //data.Add(new EditorTypeViewModel { Name = "Test02", EditorCategory = EditorCategoryEnum.Common, ControlType = ControlTypeEnum.TextBox });
            //data.Add(new EditorTypeViewModel { Name = "Test03", EditorCategory = EditorCategoryEnum.List, ControlType = ControlTypeEnum.TextBox });
            //data.Add(new EditorTypeViewModel { Name = "Test04", EditorCategory = EditorCategoryEnum.List, ControlType = ControlTypeEnum.TextBox });

            var dsResult = data;
            //var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public IActionResult CreateEditorType()
        {
            var model = new EditorTypeViewModel();
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }

        public async Task<IActionResult> EditEditorType(string id)
        {
            //var model = new EditorTypeViewModel();
            var model = await _editorTypeBusiness.GetSingleById(id);
            if(!model.EditorCategory.ToString().IsNullOrEmpty())
            {
                model.EditorCategoryData = model.EditorCategory.ToString();
            }
            if(!model.ControlType.ToString().IsNullOrEmpty())
            {
                model.ControlTypeData = model.ControlType.ToString();
            }
            model.DataAction = DataActionEnum.Edit;
            return View("CreateEditorType", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageEditorType(EditorTypeViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _editorTypeBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Editor type created successfully",true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _editorTypeBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Editor edited successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
                return View("CreateEditorType", model);
        }

    }
}
