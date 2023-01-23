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

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class ResourceLanguageController : ApplicationController

    {

        private IResourceLanguageBusiness _resourceLanguageBusiness;
        private ITeamUserBusiness _teamuserBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IUserContext _userContext;
        private readonly ILOVBusiness _lovBusiness;

        public ResourceLanguageController(IResourceLanguageBusiness resourceLanguageBusiness, ITeamUserBusiness teamuserBusiness ,IPortalBusiness portalBusiness,
            IUserContext userContext, ILOVBusiness lovBusiness)
        {
            _resourceLanguageBusiness = resourceLanguageBusiness;
             _teamuserBusiness = teamuserBusiness;
            _portalBusiness = portalBusiness;
            _userContext = userContext;
            _lovBusiness = lovBusiness;
        }




        public IActionResult Index()
        {
            return View();
        }
       
      
        public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _resourceLanguageBusiness.GetList();
            var data = model;

            var dsResult = data;
            //var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<IActionResult> Create(string templateId,TemplateTypeEnum templateType,LayoutModeEnum? lo,string cbm,string defaultValue,string code)
        {
            var data=new ResourceLanguageViewModel
            {
                TemplateId = templateId,
                TemplateType = templateType,
                English = defaultValue,
                Code = code
            };
           
            if (lo==LayoutModeEnum.Popup) 
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            var record = await _resourceLanguageBusiness.GetExistingResourceLanguage(data);
            if (record != null)
            {
                record.DataAction = DataActionEnum.Edit;
                record.CallBackMethod = cbm;
                //record.Languages = await _lovBusiness.GetList(x => x.LOVType == "PREFERED_LANGUAGE");
                return View("Manage", record);
            }
            else 
            {
                return View("Manage", new ResourceLanguageViewModel
                {
                    DataAction = DataActionEnum.Create,
                    CallBackMethod = cbm,
                    TemplateId = templateId,
                    TemplateType = templateType,
                    English = defaultValue,
                    Code = code,
                  // Languages = await _lovBusiness.GetList(x => x.LOVType == "PREFERED_LANGUAGE")
            });
            }          
        }   

        [HttpPost]
        public async Task<IActionResult> Manage(ResourceLanguageViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _resourceLanguageBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, result = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _resourceLanguageBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, result = result.Item });

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }

            return View("Manage", model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _resourceLanguageBusiness.Delete(id);
            return View("Index", new ResourceLanguageViewModel());
        }
    }
}