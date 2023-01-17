using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMS.Business;
using Microsoft.AspNetCore.Authorization;
using CMS.UI.ViewModel;
using CMS.Common;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Kendo.Mvc.UI;
using System.Data;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;
using CMS.Data.Model;
using AutoMapper;
using System.Web;
using CMS.UI.Utility;

namespace CMS.UI.Web.Controllers
{
    public class CmsTaskController : ApplicationController
    {

        //private readonly ICmsTaskBusiness _cmsTaskBusiness;

        //public CmsTaskController(ICmsTaskBusiness cmsTaskBusiness)
        //{
        //    _cmsTaskBusiness = cmsTaskBusiness;
        //}


        //public async Task<IActionResult> LoadIndexPageGrid([DataSourceRequest] DataSourceRequest request, NtsTaskOwnerTypeEnum ownerType, string indexPageTemplateId)
        //{
        //    var dt = await _cmsTaskBusiness.GetIndexPageData(indexPageTemplateId, ownerType, request);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        //public async Task<TaskIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)
        //{
        //    var model = await _cmsTaskBusiness.GetIndexPageViewModel(page);
        //    model.Page = page;
        //    model.PageId = page.Id;
        //    return model;
        //}
        //public async Task<TaskTemplateViewModel> GetViewModel(PageViewModel page)
        //{
        //    var model = await _cmsTaskBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == page.TemplateId);
        //    model.Page = page;
        //    model.PageId = page.Id;
        //    model.DataAction = page.DataAction;
        //    model.RecordId = page.RecordId;
        //    model.PortalName = page.Portal.Name;
        //    //model.TemplateId = page.TemplateId;
        //    return model;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Manage(TaskTemplateViewModel model)
        //{
        //    var result = await _cmsTaskBusiness.Manage(model);
        //    return Redirect($"~/Portal/{model.PortalName}");
        //}

    }
}
