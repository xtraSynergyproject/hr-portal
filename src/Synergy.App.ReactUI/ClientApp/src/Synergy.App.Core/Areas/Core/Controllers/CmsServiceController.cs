using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Synergy.App.Business;
using Microsoft.AspNetCore.Authorization;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
////using Kendo.Mvc.UI;
using System.Data;
////using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;
using Synergy.App.DataModel;
using AutoMapper;
using System.Web;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Controllers
{
    public class CmsServiceController : ApplicationController
    {

        //private readonly ICmsServiceBusiness _cmsServiceBusiness;

        //public CmsServiceController(ICmsServiceBusiness cmsServiceBusiness)
        //{
        //    _cmsServiceBusiness = cmsServiceBusiness;
        //}


        //public async Task<IActionResult> LoadIndexPageGrid([DataSourceRequest] DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string indexPageTemplateId)
        //{
        //    var dt = await _cmsServiceBusiness.GetIndexPageData(indexPageTemplateId, ownerType, request);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        //public async Task<ServiceIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)
        //{
        //    var model = await _cmsServiceBusiness.GetIndexPageViewModel(page);
        //    model.Page = page;
        //    model.PageId = page.Id;
        //    return model;
        //}
        //public async Task<ServiceTemplateViewModel> GetViewModel(PageViewModel page)
        //{
        //    var model = await _cmsServiceBusiness.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == page.TemplateId);
        //    model.Page = page;
        //    model.PageId = page.Id;
        //    model.DataAction = page.DataAction;
        //    model.RecordId = page.RecordId;
        //    model.PortalName = page.Portal.Name;
        //    //model.TemplateId = page.TemplateId;
        //    return model;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Manage(ServiceTemplateViewModel model)
        //{
        //    var result = await _cmsServiceBusiness.Manage(model);
        //    return Redirect($"~/Portal/{model.PortalName}");
        //}

    }
}
