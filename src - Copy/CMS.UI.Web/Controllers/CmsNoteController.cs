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
    public class CmsNoteController : ApplicationController
    {


        //public CmsNoteController(ICmsNoteBusiness cmsNoteBusiness)
        //{
        //    _cmsNoteBusiness = cmsNoteBusiness;
        //}


        //public async Task<IActionResult> LoadIndexPageGrid([DataSourceRequest] DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string indexPageTemplateId)
        //{
        //    var dt = await _cmsNoteBusiness.GetIndexPageData(indexPageTemplateId, ownerType, request);
        //    return Json(dt.ToDataSourceResult(request));
        //}
        //public async Task<NoteIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)
        //{
        //    var model = await _cmsNoteBusiness.GetIndexPageViewModel(page);
        //    model.Page = page;
        //    model.PageId = page.Id;
        //    return model;
        //}
        //public async Task<NoteTemplateViewModel> GetViewModel(PageViewModel page)
        //{
        //    var model = await _cmsNoteBusiness.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == page.TemplateId);
        //    model.Page = page;
        //    model.PageId = page.Id;
        //    model.DataAction = page.DataAction;
        //    model.RecordId = page.RecordId;
        //    model.PortalName = page.Portal.Name;
        //    //model.TemplateId = page.TemplateId;
        //    return model;
        //}



        //[HttpPost]
        //public async Task<IActionResult> Manage(NoteTemplateViewModel model)
        //{

        //    var result = await _cmsNoteBusiness.Manage(model);
        //    return Redirect($"~/Portal/{model.PortalName}");
        //    //var template = await _templateBusiness.GetSingleById(model.TemplateId);
        //    //var tabledata = await _tableDataBusiness.GetSingleById(template.TableMetadataId);
        //    //tabledata.ColumnMetadataView = new List<ColumnMetadataViewModel>();
        //    //tabledata.ColumnMetadataView = await _columnDataBusiness.GetList(x => x.TableMetadataId == template.TableMetadataId);

        //    //var jsonResult = JObject.Parse(model.Json);
        //    //foreach (var item in tabledata.ColumnMetadataView)
        //    //{
        //    //    var valObj = jsonResult.SelectToken(item.Name);
        //    //    if (valObj != null)
        //    //    {
        //    //        item.Value = valObj;

        //    //    }
        //    //}

        //}

    }
}
