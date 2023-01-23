﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Synergy.App.Business;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Synergy.App.DataModel;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
//using Syncfusion.EJ2.Base;
using System.Text;
using Npgsql;
using FastMember;
using Microsoft.VisualBasic.FileIO;
using System.IO;
//using Syncfusion.EJ2.Maps;
using System.Data;
using Nest;
using ExcelDataReader;
using Synergy.App.WebUtility;
using System.Globalization;
using Hangfire;
using System.ServiceModel.Syndication;
using System.Xml;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Unsubscribing;
using Microsoft.AspNetCore.Http;
using FastReport.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;
using DocumentFormat.OpenXml.Office2013.Excel;
using AutoMapper.Configuration.Annotations;
using ProtoBuf.Meta;
//using ExcelDataReader;

namespace CMS.UI.Web.Areas.CMS.Controllers


{
    //[Authorize]
    [Area("Cms")]

    public class BusinessAnalyticsController : ApplicationController
    {

        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IWebHelper _webApi;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IIipBusiness _iipBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public BusinessAnalyticsController(IUserContext userContext, INoteBusiness noteBusiness, ITaskBusiness taskBusiness, IWebHelper webApi,
            ITableMetadataBusiness tableMetadataBusiness, ICmsBusiness cmsBusiness, INotificationBusiness notificationBusiness, ILOVBusiness lovBusiness, IIipBusiness iipBusiness,
            ITemplateBusiness templateBusiness, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {

            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _taskBusiness = taskBusiness;
            _webApi = webApi;
            _tableMetadataBusiness = tableMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _notificationBusiness = notificationBusiness;
            _lovBusiness = lovBusiness;
            _iipBusiness = iipBusiness;
            _templateBusiness = templateBusiness;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Streaming(string rtspLink, string id, string serverId = "")
        {
            if (rtspLink.IsNotNullAndNotEmpty())
            {
                rtspLink = rtspLink.Replace("^", "&");
                var splits = rtspLink.Split("@10");
                var _link = splits[1];
                var _host = splits[0];
                var host = _host.Replace("rtsp://", "");
                var creadential = host.Split(':');
                var username = creadential[0];
                var password = creadential[1];
                var model = new StreamingViewModel { RTSP_Id = id, RTSP_Url = ("rtsp://10" + _link), RTSP_User = username, RTSP_Pwd = password, ServerId = serverId };
                return View(model);
            }
            else
            {
                return View(new StreamingViewModel());
            }

        }
        public async Task<IActionResult> Schema()
        {
            return View();
        }
        public async Task<IActionResult> SynergySchema(string Id)
        {
            var model = new SynergySchemaViewModel();
            if (Id.IsNotNull())
            {
                model = await _noteBusiness.GetSynerySchemaById(Id);
                model.Query = model.Query.Replace("^", "'");
                model.DataAction = DataActionEnum.Edit;
            }

            return View(model);
        }

        public async Task<IActionResult> SynergySchemaIndex()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SynergySchemaValidate(SynergySchemaViewModel model)
        {
            if (model.IsNotNull() && model.Query.IsNotNullAndNotEmpty() && !model.ElsasticDB)
            {
                var result = await _noteBusiness.GetQueryValidate(model.Query);
                if (!result.IsSuccess)
                {
                    return Json(new { success = false, error = result.HtmlError });
                }
                return Json(new { success = true });
            }
            else if (model.IsNotNull() && model.Query.IsNotNullAndNotEmpty() && model.Query.Contains("*") && model.ElsasticDB)
            {
                var queryArr = model.Query.Split("from");
                if (queryArr.Count() > 1)
                {
                    var queryArr1 = queryArr.Last().Trim().Split(" ");
                    if (queryArr1.Count() > 0)
                    {
                        var indexName = queryArr1.First();
                        var url = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration) + indexName;
                        using (var httpClient = new HttpClient())
                        {
                            var address = new Uri(url);
                            var response = await httpClient.GetAsync(address);
                            if (response.IsSuccessStatusCode == true)
                            {
                                return Json(new { success = true });
                            }
                            else
                            {
                                return Json(new { success = false, error = "Table name not exist !" });
                            }
                        }
                    }

                }
                return Json(new { success = false, error = "Query is not valid." });
            }
            else if (model.IsNotNull() && model.Query.IsNotNullAndNotEmpty() && model.ElsasticDB)
            {
                var queryArr = model.Query.Split("from");
                if (queryArr.Count() > 1)
                {
                    var queryArr1 = queryArr.Last().Trim().Split(" ");
                    if (queryArr1.Count() > 0)
                    {
                        var indexName = queryArr1.First();
                        var url = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration) + indexName;
                        using (var httpClient = new HttpClient())
                        {
                            var address = new Uri(url);
                            var response = await httpClient.GetAsync(address);
                            if (response.IsSuccessStatusCode == true)
                            {
                                return Json(new { success = true });
                            }
                            else
                            {
                                return Json(new { success = false, error = "Table name not exist !" });
                            }
                        }
                    }

                }
                return Json(new { success = false, error = "Query is not valid." });

            }
            return Json(new { success = false, error = "Query is required." });
        }
        [HttpPost]
        public async Task<IActionResult> SynergySchemaGenerate(SynergySchemaViewModel model)
        {
            if (model.IsNotNull())
            {

                if (model.Query.IsNotNullAndNotEmpty() && model.SchemaName.IsNotNullAndNotEmpty() && !model.ElsasticDB)
                {
                    DataTable dt = await _noteBusiness.GetQueryResult(model.Query);
                    model.Query = model.Query.Replace("'", "^");
                    var noteTempModel = new NoteTemplateViewModel();
                    if (model.Id.IsNotNull())
                    {
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.NoteId = model.Id;
                    }
                    else
                    {
                        noteTempModel.DataAction = DataActionEnum.Create;
                    }
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "SynergySchema";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    notemodel.Json = JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        if (dt.Columns.Count > 0)
                        {
                            if (model.Id.IsNotNull())
                            {
                                var childNotes = await _noteBusiness.GetList(x => x.ParentNoteId == model.Id);
                                foreach (var item in childNotes)
                                {
                                    await _noteBusiness.Delete(item.Id);
                                }
                            }
                            foreach (System.Data.DataColumn columns in dt.Columns)
                            {
                                var cModel = new NoteTemplateViewModel();
                                cModel.DataAction = DataActionEnum.Create;
                                cModel.ActiveUserId = _userContext.UserId;
                                cModel.TemplateCode = "SCHEMA_DIMENSIONS";
                                var nModel = await _noteBusiness.GetNoteDetails(cModel);
                                nModel.NoteSubject = columns.ColumnName;
                                nModel.NoteDescription = columns.DataType.Name;
                                nModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                nModel.ParentNoteId = result.Item.NoteId;
                                var result1 = await _noteBusiness.ManageNote(nModel);
                            }
                        }
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, error = "Schema Generation Failed !" });
                    }

                }
                else if (model.Query.IsNotNullAndNotEmpty() && model.SchemaName.IsNotNullAndNotEmpty() && model.Query.Contains("*") && model.ElsasticDB)
                {
                    var queryArr = model.Query.Split("from");
                    if (queryArr.Count() > 1)
                    {
                        var queryArr1 = queryArr.Last().Trim().Split(" ");
                        if (queryArr1.Count() > 0)
                        {
                            var indexName = queryArr1.First();
                            var url = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration) + indexName;
                            using (var httpClient = new HttpClient())
                            {
                                var address = new Uri(url);
                                var response = await httpClient.GetAsync(address);
                                if (response.IsSuccessStatusCode == true)
                                {
                                    var noteTempModel = new NoteTemplateViewModel();
                                    if (model.Id.IsNotNull())
                                    {
                                        noteTempModel.DataAction = DataActionEnum.Edit;
                                        noteTempModel.NoteId = model.Id;
                                    }
                                    else
                                    {
                                        noteTempModel.DataAction = DataActionEnum.Create;
                                    }
                                    noteTempModel.ActiveUserId = _userContext.UserId;
                                    noteTempModel.TemplateCode = "SynergySchema";
                                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                    notemodel.Json = JsonConvert.SerializeObject(model);
                                    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                    var result = await _noteBusiness.ManageNote(notemodel);
                                    if (result.IsSuccess)
                                    {
                                        var json = await response.Content.ReadAsStringAsync();
                                        var _data = JToken.Parse(json);
                                        if (_data.IsNotNull())
                                        {
                                            var _index = _data.SelectToken(indexName);
                                            if (_index.IsNotNull())
                                            {
                                                var _mappings = _index.SelectToken("mappings");
                                                if (_mappings.IsNotNull())
                                                {
                                                    var _properties = (JObject)_mappings.SelectToken("properties");
                                                    if (_properties.Count > 0)
                                                    {
                                                        if (model.Id.IsNotNull())
                                                        {
                                                            var childNotes = await _noteBusiness.GetList(x => x.ParentNoteId == model.Id);
                                                            foreach (var item in childNotes)
                                                            {
                                                                await _noteBusiness.Delete(item.Id);
                                                            }
                                                        }

                                                    }
                                                    foreach (var item in _properties.Properties())
                                                    {
                                                        if (model.Query.Contains("*") || model.Query.Contains(item.Name))
                                                        {
                                                            var _type = item.First.SelectToken("type");
                                                            var _dataType = _type.Value<string>();
                                                            var cModel = new NoteTemplateViewModel();
                                                            cModel.DataAction = DataActionEnum.Create;
                                                            cModel.ActiveUserId = _userContext.UserId;
                                                            cModel.TemplateCode = "SCHEMA_DIMENSIONS";
                                                            var nModel = await _noteBusiness.GetNoteDetails(cModel);
                                                            nModel.NoteSubject = item.Name;
                                                            nModel.NoteDescription = _dataType == "text" ? "string" : _dataType == "date" ? "DateTime" : _dataType;
                                                            nModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                                            nModel.ParentNoteId = result.Item.NoteId;
                                                            var result1 = await _noteBusiness.ManageNote(nModel);
                                                        }

                                                    }
                                                }
                                            }


                                        }
                                        return Json(new { success = true });
                                    }
                                    else
                                    {
                                        return Json(new { success = false, error = "Schema Generation Failed !" });
                                    }

                                }
                                else
                                {
                                    return Json(new { success = false, error = "Table name not exist !" });
                                }
                            }
                        }

                    }
                }
                else if (model.Query.IsNotNullAndNotEmpty() && model.SchemaName.IsNotNullAndNotEmpty() && model.ElsasticDB)
                {
                    var queryArr = model.Query.Split("from");
                    if (queryArr.Count() > 1)
                    {
                        var queryArr1 = queryArr.Last().Trim().Split(" ");
                        if (queryArr1.Count() > 0)
                        {
                            var indexName = queryArr1.First();
                            var url = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration) + indexName;
                            using (var httpClient = new HttpClient())
                            {
                                var address = new Uri(url);
                                var response = await httpClient.GetAsync(address);
                                if (response.IsSuccessStatusCode == true)
                                {
                                    var noteTempModel = new NoteTemplateViewModel();
                                    if (model.Id.IsNotNull())
                                    {
                                        noteTempModel.DataAction = DataActionEnum.Edit;
                                        noteTempModel.NoteId = model.Id;
                                    }
                                    else
                                    {
                                        noteTempModel.DataAction = DataActionEnum.Create;
                                    }
                                    noteTempModel.ActiveUserId = _userContext.UserId;
                                    noteTempModel.TemplateCode = "SynergySchema";
                                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                    notemodel.Json = JsonConvert.SerializeObject(model);
                                    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                    var result = await _noteBusiness.ManageNote(notemodel);
                                    if (result.IsSuccess)
                                    {
                                        var json = await response.Content.ReadAsStringAsync();
                                        var _data = JToken.Parse(json);
                                        if (_data.IsNotNull())
                                        {
                                            var _index = _data.SelectToken(indexName);
                                            if (_index.IsNotNull())
                                            {
                                                var _mappings = _index.SelectToken("mappings");
                                                if (_mappings.IsNotNull())
                                                {
                                                    var _properties = (JObject)_mappings.SelectToken("properties");
                                                    if (_properties.Count > 0)
                                                    {
                                                        if (model.Id.IsNotNull())
                                                        {
                                                            var childNotes = await _noteBusiness.GetList(x => x.ParentNoteId == model.Id);
                                                            foreach (var item in childNotes)
                                                            {
                                                                await _noteBusiness.Delete(item.Id);
                                                            }
                                                        }

                                                    }
                                                    foreach (var item in _properties.Properties())
                                                    {
                                                        if (model.Query.Contains("*") || model.Query.Contains(item.Name))
                                                        {
                                                            var _type = item.First.SelectToken("type");
                                                            var _dataType = _type.Value<string>();
                                                            var cModel = new NoteTemplateViewModel();
                                                            cModel.DataAction = DataActionEnum.Create;
                                                            cModel.ActiveUserId = _userContext.UserId;
                                                            cModel.TemplateCode = "SCHEMA_DIMENSIONS";
                                                            var nModel = await _noteBusiness.GetNoteDetails(cModel);
                                                            nModel.NoteSubject = item.Name;
                                                            nModel.NoteDescription = _dataType == "text" ? "string" : _dataType == "date" ? "DateTime" : _dataType;
                                                            nModel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                                            nModel.ParentNoteId = result.Item.NoteId;
                                                            var result1 = await _noteBusiness.ManageNote(nModel);
                                                        }

                                                    }
                                                }
                                            }


                                        }
                                        return Json(new { success = true });
                                    }
                                    else
                                    {
                                        return Json(new { success = false, error = "Schema Generation Failed !" });
                                    }

                                }
                                else
                                {
                                    return Json(new { success = false, error = "Table name not exist !" });
                                }
                            }
                        }

                    }
                }
            }
            return Json(new { success = false, error = "Name and Query is required." });
        }
        public async Task<IActionResult> SynergySchemaMetadata(string id)
        {
            var model = await _noteBusiness.GetSynerySchemaById(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSynergySchemaMetadata(SynergySchemaViewModel model)
        {
            var noteTempModel = new NoteTemplateViewModel();
            if (model.IsNotNull())
            {
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.NoteId = model.Id;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "SynergySchema";
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var metadata = notemodel.ColumnList.Where(x => x.Name == "Metadata").FirstOrDefault();
                if (metadata.IsNotNull())
                {
                    metadata.Value = model.Metadata;
                }
                var rowData2 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var data2 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
                var result = await _noteBusiness.EditNoteUdfTable(notemodel, data2, notemodel.UdfNoteTableId);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }
        public async Task<IActionResult> SynergyQueryList(SynergySchemaViewModel model)
        {
            TempData["Query"] = model.Query;
            DataTable dt = await _noteBusiness.GetQueryResult(model.Query);
            return View(dt);

        }
        public async Task<ActionResult> ReadDataResult([DataSourceRequest] DataSourceRequest request)
        {
            var query = TempData["Query"].ToString();
            DataTable data = await _noteBusiness.GetQueryResult(query);
            var dsResult = data;
            return Json(dsResult);
        }

        public async Task<ActionResult> ReadDataIndex(/*[DataSourceRequest] DataSourceRequest request*/)
        {

            var data = await _noteBusiness.GetSyneryList();

            var dsResult = data/*.ToDataSourceResult(request)*/;
            return Json(dsResult);
        }


        public async Task<JsonResult> GetBusinessAnalyticsTreeList(string id, string parentId)
        {
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                list.Add(new TreeViewViewModel
                {
                    id = TemplateTypeEnum.Dashboard.ToString(),
                    Name = TemplateTypeEnum.Dashboard.ToString(),
                    DisplayName = TemplateTypeEnum.Dashboard.ToString(),
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root",
                    children = true,
                    text = TemplateTypeEnum.Dashboard.ToString(),
                    parent = "#",
                    a_attr = new { data_id = TemplateTypeEnum.Dashboard.ToString(), data_name = TemplateTypeEnum.Dashboard.ToString(), data_type = "Root" },
                });
            }
            if (id == TemplateTypeEnum.Dashboard.ToString())
            {
                TemplateTypeEnum type = id.ToEnum<TemplateTypeEnum>();
                var dashboards = await _noteBusiness.GetAllDashboardMaster();
                list.AddRange(dashboards.Select(x => new TreeViewViewModel
                {
                    id = x.Id,
                    Name = x.NoteSubject,
                    DisplayName = x.NoteSubject,
                    ParentId = id,
                    hasChildren = true,
                    expanded = false,
                    Type = "DashboardMaster",
                    children = true,
                    text = x.NoteSubject,
                    parent = id,
                    a_attr = new { data_id = x.Id, data_name = x.NoteSubject, data_type = "DashboardMaster" },
                }));
            }
            if (id.IsNotNullAndNotEmpty() && id != TemplateTypeEnum.Dashboard.ToString())
            {
                var dashboardItems = await _noteBusiness.GetList(x => x.ParentNoteId == id);
                list.AddRange(dashboardItems.Select(x => new TreeViewViewModel
                {
                    id = x.Id,
                    Name = x.NoteSubject,
                    DisplayName = x.NoteSubject,
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = x.TemplateCode,
                    children = true,
                    text = x.NoteSubject,
                    parent = id,
                    a_attr = new { data_id = x.Id, data_name = x.NoteSubject, data_type = x.TemplateCode },
                }));
            }



            return Json(list.ToList());
        }
        public async Task<IActionResult> Dashboard(string id, DataActionEnum dataAction)
        {
            var model = new DashboardMasterViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DashboardMaster";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageDashboardNote(DashboardMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DashboardMaster";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                else
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<IActionResult> MapLayerItem(string id, string parentId, DataActionEnum dataAction, string layout)
        {
            var model = new MapLayerItemViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "MAP_LAYER_ITEM";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.ParentNoteId = parentId;
                //model.CreatedBy = newmodel.CreatedBy;
                //model.CreatedDate = System.DateTime.Now;
                //model.LastUpdatedBy = newmodel.LastUpdatedBy;
                //model.LastUpdatedDate = System.DateTime.Now;
                //model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetMapLayerItemDetails(id);
                if (udf.IsNotNull())
                {
                    model.MapUrl = udf.MapUrl;
                    model.MapLayer = udf.MapLayer;
                    model.MapTransparency = udf.MapTransparency;
                    model.MapFormat = udf.MapFormat;
                    model.MapOpacity = udf.MapOpacity;
                    model.IsBaseMap = udf.IsBaseMap;
                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                //model.CreatedBy = newmodel.CreatedBy;
                //model.LastUpdatedBy = newmodel.LastUpdatedBy;
                //model.LastUpdatedDate = System.DateTime.Now;
                //model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageMapLayerItem(MapLayerItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "MAP_LAYER_ITEM" && x.ParentNoteId == model.ParentNoteId);
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "MAP_LAYER_ITEM";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "MAP_LAYER_ITEM" && x.ParentNoteId == model.ParentNoteId);
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        private async Task<string> BuildChartJson(JArray comps, string chartCum)
        {
            JToken nodes;
            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, chartCum);
                        }
                    }
                    else if (type == "tabs")
                    {

                        JArray cols = (JArray)jcomp.SelectToken("components");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, chartCum);
                        }

                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, chartCum);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                        {
                            chartCum = await BuildChartJson(rows, chartCum);
                        }
                        else
                        {
                            nodes = jcomp["chartItemName"];
                            if (nodes == null)
                            {
                                continue;
                            }
                            var chartName = nodes.Value<string>();
                            var chartNamear = chartName.Split('-');
                            var dashboardItem = await _noteBusiness.GetDashboardItemDetailsByName(chartNamear[0]);
                            if (!dashboardItem.IsNotNull())
                            {
                                continue;
                            }
                            var layers = await _noteBusiness.GetMapLayerItemList(dashboardItem.NoteId);
                            string layersString = null;
                            if (layers.Count > 0)
                            {
                                layersString = JsonConvert.SerializeObject(layers);
                            }
                            var filters = JsonConvert.DeserializeObject<List<DashboardItemFilterViewModel>>(dashboardItem.filterField);
                            //var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
                            //if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
                            //{
                            //    timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

                            //}
                            var chartInput = dashboardItem.chartMetadata;
                            var clickFunction = dashboardItem.onChartClickFunction;
                            var clickFunctionName = "On" + chartNamear[0].Replace(" ", string.Empty) + "Click";
                            if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
                            {
                                var metadataArray = chartInput.Split("],");
                                var filstr = "filters: [";
                                foreach (var filter in filters)
                                {
                                    if (filter.FilterText.Contains("^^") && filter.DefaultValue == "All")
                                    {
                                        continue;
                                    }
                                    else if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = "";
                                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                        {
                                            value = filter.DefaultValue;
                                        }
                                        else
                                        {
                                            value = '"' + filter.DefaultValue + '"';
                                        }
                                        var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                        filstr += fil;
                                    }

                                }
                                filstr += "]";
                                chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
                                chartInput = ReplaceChartSessionValue(chartInput);
                            }
                            else if (chartInput.Contains("^^"))
                            {
                                chartInput = ReplaceChartSessionValue(chartInput);
                                foreach (var filter in filters)
                                {
                                    if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = "";
                                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                        {
                                            value = filter.DefaultValue;
                                        }
                                        else
                                        {
                                            value = '"' + filter.DefaultValue + '"';
                                        }
                                        chartInput = chartInput.Replace(filter.FilterText, value);
                                    }

                                }

                            }
                            var chartBPCode = dashboardItem.boilerplateCode;
                            chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
                            chartBPCode = chartBPCode.Replace("@@xaxis@@", dashboardItem.Xaxis);
                            chartBPCode = chartBPCode.Replace("@@yaxis@@", dashboardItem.Yaxis);
                            chartBPCode = chartBPCode.Replace("@@Count@@", dashboardItem.Count);
                            chartBPCode = chartBPCode.Replace("@@DataSource@@", layersString);
                            chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
                            chartBPCode = chartBPCode.Replace("@@chartHtml@@", dashboardItem.NoteDescription);
                            chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
                            chartBPCode = chartBPCode.Replace("@@mapUrl@@", dashboardItem.mapUrl);
                            chartBPCode = chartBPCode.Replace("@@mapLayer@@", dashboardItem.mapLayer);
                            chartBPCode = chartBPCode.Replace("@@mode@@", dashboardItem.ThemeMode);
                            chartBPCode = chartBPCode.Replace("@@palette@@", dashboardItem.Palette);
                            chartBPCode = chartBPCode.Replace("@@monochrome@@", dashboardItem.MonocromeColor.IsNotNullAndNotEmpty() ? "true" : "false");
                            chartBPCode = chartBPCode.Replace("@@color@@", dashboardItem.MonocromeColor);
                            chartBPCode = chartBPCode.Replace("OnSeriesClick", clickFunctionName);
                            chartBPCode = chartBPCode.Replace("@@Latitude@@", "51.5");
                            chartBPCode = chartBPCode.Replace("@@Longitude@@", "-0.09");
                            chartBPCode = chartBPCode.Replace("@@LocationName@@", "London");
                            if (clickFunction.IsNotNullAndNotEmpty())
                            {
                                clickFunction = clickFunction.Replace("OnSeriesClick", clickFunctionName);
                                chartBPCode += clickFunction;
                            }
                            dashboardItem.chartMetadata = chartBPCode;
                            chartCum = chartCum + " \n " + chartBPCode;


                        }
                    }
                }
            }
            return chartCum;
        }
        private async Task<string> BuildChart(string json)
        {
            var chartCum = "";
            if (json.IsNotNullAndNotEmpty())
            {
                var data = JToken.Parse(json);
                JArray rows = (JArray)data.SelectToken("components");
                chartCum = await BuildChartJson(rows, chartCum);
            }
            return chartCum;

        }
        public async Task<string> GetChartDataByIdWithParams(string id, string param)
        {
            var chartCum = "";
            var dashboardItem = await _noteBusiness.GetDashboardItemDetailsById(id);
            if (!dashboardItem.IsNotNull())
            {
                return chartCum;
            }
            var layers = await _noteBusiness.GetMapLayerItemList(dashboardItem.NoteId);
            string layersString = null;
            if (layers.Count > 0)
            {
                layersString = JsonConvert.SerializeObject(layers);
            }
            var filters = JsonConvert.DeserializeObject<List<DashboardItemFilterViewModel>>(dashboardItem.filterField);
            //var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
            //if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
            //{
            //    timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

            //}
            var chartInput = dashboardItem.chartMetadata;
            var clickFunction = dashboardItem.onChartClickFunction;
            var clickFunctionName = "On" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "Click";
            if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
            {
                var metadataArray = chartInput.Split("],");
                var filstr = "filters: [";
                if (param.IsNotNullAndNotEmpty())
                {
                    var parameters = param.Split('&');
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var para = parameters[i].Split('=');
                        foreach (var filter in filters)
                        {
                            if (filter.FilterText.Contains(para[0]))
                            {
                                if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && filter.DefaultValue == "All" && para[1].IsNullOrEmpty())
                                {
                                    continue;
                                }
                                else if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                                {
                                    var value = "";
                                    if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                    {
                                        value = para[1];
                                    }
                                    else
                                    {
                                        value = '"' + para[1] + '"';
                                    }
                                    var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                    filstr += fil;
                                }
                                else if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && filter.DefaultValue.IsNotNullAndNotEmpty())
                                {
                                    var value = "";
                                    if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                    {
                                        value = filter.DefaultValue;
                                    }
                                    else
                                    {
                                        value = '"' + filter.DefaultValue + '"';
                                    }
                                    var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                    filstr += fil;
                                }
                            }

                        }


                    }
                    filstr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
                }
                else
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterText.Contains("^^") && filter.DefaultValue == "All")
                        {
                            continue;
                        }
                        else if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                        {
                            var value = "";
                            if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                            {
                                value = filter.DefaultValue;
                            }
                            else
                            {
                                value = '"' + filter.DefaultValue + '"';
                            }
                            var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                            filstr += fil;
                        }

                    }
                    filstr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
                }
                chartInput = ReplaceChartSessionValue(chartInput);
            }
            else if (chartInput.Contains("^^"))
            {
                chartInput = ReplaceChartSessionValue(chartInput);
                if (param.IsNotNullAndNotEmpty())
                {
                    var parameters = param.Split('&');
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var para = parameters[i].Split('=');
                        foreach (var filter in filters)
                        {
                            if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                            {
                                var value = "";
                                if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                {
                                    value = para[1];
                                }
                                else
                                {
                                    value = '"' + para[1] + '"';
                                }
                                chartInput = chartInput.Replace(filter.FilterText, value);
                            }

                        }

                    }
                }
                foreach (var filter in filters)
                {
                    if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                    {
                        var value = "";
                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                        {
                            value = filter.DefaultValue;
                        }
                        else
                        {
                            value = '"' + filter.DefaultValue + '"';
                        }
                        chartInput = chartInput.Replace(filter.FilterText, value);
                    }

                }


            }
            var chartBPCode = dashboardItem.boilerplateCode;
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
            chartBPCode = chartBPCode.Replace("@@xaxis@@", dashboardItem.Xaxis);
            chartBPCode = chartBPCode.Replace("@@yaxis@@", dashboardItem.Yaxis);
            chartBPCode = chartBPCode.Replace("@@Count@@", dashboardItem.Count);
            chartBPCode = chartBPCode.Replace("@@DataSource@@", layersString);
            chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
            chartBPCode = chartBPCode.Replace("@@chartHtml@@", dashboardItem.NoteDescription);
            chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
            chartBPCode = chartBPCode.Replace("@@mapUrl@@", dashboardItem.mapUrl);
            chartBPCode = chartBPCode.Replace("@@mapLayer@@", dashboardItem.mapLayer);
            chartBPCode = chartBPCode.Replace("@@mode@@", dashboardItem.ThemeMode);
            chartBPCode = chartBPCode.Replace("@@palette@@", dashboardItem.Palette);
            chartBPCode = chartBPCode.Replace("@@monochrome@@", dashboardItem.MonocromeColor.IsNotNullAndNotEmpty() ? "true" : "false");
            chartBPCode = chartBPCode.Replace("@@color@@", dashboardItem.MonocromeColor);
            chartBPCode = chartBPCode.Replace("OnSeriesClick", clickFunctionName);
            chartBPCode = chartBPCode.Replace("@@Latitude@@", "51.5");
            chartBPCode = chartBPCode.Replace("@@Longitude@@", "-0.09");
            chartBPCode = chartBPCode.Replace("@@LocationName@@", "London");
            if (clickFunction.IsNotNullAndNotEmpty())
            {
                clickFunction = clickFunction.Replace("OnSeriesClick", clickFunctionName);
                chartBPCode += clickFunction;
            }
            dashboardItem.chartMetadata = chartBPCode;
            chartCum = chartCum + " \n " + chartBPCode;
            return chartCum;
        }
        public async Task<string> GetChartDataByItemNameWithParams(string itemName, string param)
        {
            var chartCum = "";
            var dashboardItem = await _noteBusiness.GetDashboardItemDetailsByName(itemName);
            if (!dashboardItem.IsNotNull())
            {
                return chartCum;
            }
            var layers = await _noteBusiness.GetMapLayerItemList(dashboardItem.NoteId);
            string layersString = null;
            if (layers.Count > 0)
            {
                layersString = JsonConvert.SerializeObject(layers);
            }
            var filters = JsonConvert.DeserializeObject<List<DashboardItemFilterViewModel>>(dashboardItem.filterField);
            //var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
            //if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
            //{
            //    timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

            //}
            var chartInput = dashboardItem.chartMetadata;
            var clickFunction = dashboardItem.onChartClickFunction;
            var clickFunctionName = "On" + itemName.Replace(" ", string.Empty) + "Click";
            if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
            {
                var metadataArray = chartInput.Split("],");
                var filstr = "filters: [";
                if (param.IsNotNullAndNotEmpty())
                {
                    var parameters = param.Split('&');
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var para = parameters[i].Split('=');
                        foreach (var filter in filters)
                        {
                            if (filter.FilterText.Contains(para[0]))
                            {
                                if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && filter.DefaultValue == "All" && para[1].IsNullOrEmpty())
                                {
                                    continue;
                                }
                                else if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                                {
                                    var value = "";
                                    if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                    {
                                        value = para[1];
                                    }
                                    else
                                    {
                                        value = '"' + para[1] + '"';
                                    }
                                    var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                    filstr += fil;
                                }
                                else if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && filter.DefaultValue.IsNotNullAndNotEmpty())
                                {
                                    var value = "";
                                    if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                    {
                                        value = filter.DefaultValue;
                                    }
                                    else
                                    {
                                        value = '"' + filter.DefaultValue + '"';
                                    }
                                    var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                    filstr += fil;
                                }
                            }

                        }

                    }
                    filstr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
                }
                else
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterText.Contains("^^") && filter.DefaultValue == "All")
                        {
                            continue;
                        }
                        else if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                        {
                            var value = "";
                            if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                            {
                                value = filter.DefaultValue;
                            }
                            else
                            {
                                value = '"' + filter.DefaultValue + '"';
                            }
                            var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                            filstr += fil;
                        }

                    }
                    filstr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
                }
                chartInput = ReplaceChartSessionValue(chartInput);
            }
            else if (chartInput.Contains("^^"))
            {
                chartInput = ReplaceChartSessionValue(chartInput);
                if (param.IsNotNullAndNotEmpty())
                {
                    var parameters = param.Split('&');
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var para = parameters[i].Split('=');
                        foreach (var filter in filters)
                        {
                            if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                            {
                                var value = "";
                                if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                {
                                    value = para[1];
                                }
                                else
                                {
                                    value = '"' + para[1] + '"';
                                }
                                chartInput = chartInput.Replace(filter.FilterText, value);
                            }

                        }


                    }
                }
                foreach (var filter in filters)
                {
                    if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                    {
                        var value = "";
                        if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                        {
                            value = filter.DefaultValue;
                        }
                        else
                        {
                            value = '"' + filter.DefaultValue + '"';
                        }
                        chartInput = chartInput.Replace(filter.FilterText, value);
                    }

                }

            }
            var chartBPCode = dashboardItem.boilerplateCode;
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
            chartBPCode = chartBPCode.Replace("@@xaxis@@", dashboardItem.Xaxis);
            chartBPCode = chartBPCode.Replace("@@yaxis@@", dashboardItem.Yaxis);
            chartBPCode = chartBPCode.Replace("@@Count@@", dashboardItem.Count);
            chartBPCode = chartBPCode.Replace("@@DataSource@@", layersString);
            chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
            chartBPCode = chartBPCode.Replace("@@chartHtml@@", dashboardItem.NoteDescription);
            chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
            chartBPCode = chartBPCode.Replace("@@mapUrl@@", dashboardItem.mapUrl);
            chartBPCode = chartBPCode.Replace("@@mapLayer@@", dashboardItem.mapLayer);
            chartBPCode = chartBPCode.Replace("@@mode@@", dashboardItem.ThemeMode);
            chartBPCode = chartBPCode.Replace("@@palette@@", dashboardItem.Palette);
            chartBPCode = chartBPCode.Replace("@@monochrome@@", dashboardItem.MonocromeColor.IsNotNullAndNotEmpty() ? "true" : "false");
            chartBPCode = chartBPCode.Replace("@@color@@", dashboardItem.MonocromeColor);
            chartBPCode = chartBPCode.Replace("OnSeriesClick", clickFunctionName);
            chartBPCode = chartBPCode.Replace("@@Latitude@@", "51.5");
            chartBPCode = chartBPCode.Replace("@@Longitude@@", "-0.09");
            chartBPCode = chartBPCode.Replace("@@LocationName@@", "London");
            if (clickFunction.IsNotNullAndNotEmpty())
            {
                clickFunction = clickFunction.Replace("OnSeriesClick", clickFunctionName);
                chartBPCode += clickFunction;
            }
            dashboardItem.chartMetadata = chartBPCode;
            chartCum = chartCum + " \n " + chartBPCode;
            return chartCum;
        }
        public async Task<string> GetIframeDataByItemNameWithParams(string itemName, string param)
        {
            var chartCum = "";
            var dashboardItem = await _noteBusiness.GetDashboardItemDetailsByName(itemName);
            if (!dashboardItem.IsNotNull())
            {
                return chartCum;
            }
            var layers = await _noteBusiness.GetMapLayerItemList(dashboardItem.NoteId);
            string layersString = null;
            if (layers.Count > 0)
            {
                layersString = JsonConvert.SerializeObject(layers);
            }
            //var filters = JsonConvert.DeserializeObject<List<DashboardItemFilterViewModel>>(dashboardItem.filterField);
            //var chartInput = dashboardItem.chartMetadata;
            var chartInput = param;
            var clickFunction = dashboardItem.onChartClickFunction;
            var clickFunctionName = "On" + itemName.Replace(" ", string.Empty) + "Click";
            if (chartInput.Contains("^^"))
            {
                chartInput = ReplaceChartSessionValue(chartInput);
                //if (param.IsNotNullAndNotEmpty())
                //{

                //var parameters = param.Split('&');
                //for (int i = 0; i < parameters.Length; i++)
                //{
                //    var para = parameters[i].Split('=');
                //    foreach (var filter in filters)
                //    {
                //        if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                //        {
                //            //var lov = await _lovBusiness.GetSingle(x => x.Code == para[1]);
                //            var value = '"' + para[1] + '"';
                //            chartInput = chartInput.Replace(filter.FilterText, value);
                //        }

                //    }

                //}
                //}
                //foreach (var filter in filters)
                //{
                //    if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                //    {
                //        var value = '"' + filter.DefaultValue + '"';
                //        chartInput = chartInput.Replace(filter.FilterText, value);
                //    }

                //}

            }
            var chartBPCode = dashboardItem.boilerplateCode;
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
            chartBPCode = chartBPCode.Replace("@@xaxis@@", dashboardItem.Xaxis);
            chartBPCode = chartBPCode.Replace("@@yaxis@@", dashboardItem.Yaxis);
            chartBPCode = chartBPCode.Replace("@@Count@@", dashboardItem.Count);
            chartBPCode = chartBPCode.Replace("@@DataSource@@", layersString);
            chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
            chartBPCode = chartBPCode.Replace("@@chartHtml@@", dashboardItem.NoteDescription);
            chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
            chartBPCode = chartBPCode.Replace("@@mapUrl@@", dashboardItem.mapUrl);
            chartBPCode = chartBPCode.Replace("@@mapLayer@@", dashboardItem.mapLayer);
            chartBPCode = chartBPCode.Replace("@@mode@@", dashboardItem.ThemeMode);
            chartBPCode = chartBPCode.Replace("@@palette@@", dashboardItem.Palette);
            chartBPCode = chartBPCode.Replace("@@monochrome@@", dashboardItem.MonocromeColor.IsNotNullAndNotEmpty() ? "true" : "false");
            chartBPCode = chartBPCode.Replace("@@color@@", dashboardItem.MonocromeColor);
            chartBPCode = chartBPCode.Replace("OnSeriesClick", clickFunctionName);
            chartBPCode = chartBPCode.Replace("@@Latitude@@", "51.5");
            chartBPCode = chartBPCode.Replace("@@Longitude@@", "-0.09");
            chartBPCode = chartBPCode.Replace("@@LocationName@@", "London");
            if (clickFunction.IsNotNullAndNotEmpty())
            {
                clickFunction = clickFunction.Replace("OnSeriesClick", clickFunctionName);
                chartBPCode += clickFunction;
            }
            dashboardItem.chartMetadata = chartBPCode;
            chartCum = chartCum + " \n " + chartBPCode;
            return chartCum;
        }
        private string ReplaceChartSessionValue(string chartMetadata)
        {
            if (chartMetadata.Contains("^^UserId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserId^^", '"' + _userContext.UserId + '"');
            }
            if (chartMetadata.Contains("^^UserUniqueId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserUniqueId^^", '"' + _userContext.UserUniqueId + '"');
            }
            if (chartMetadata.Contains("^^Name^^"))
            {
                chartMetadata = chartMetadata.Replace("^^Name^^", '"' + _userContext.Name + '"');
            }
            if (chartMetadata.Contains("^^CompanyId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CompanyId^^", '"' + _userContext.CompanyId + '"');
            }
            if (chartMetadata.Contains("^^CompanyCode^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CompanyCode^^", '"' + _userContext.CompanyCode + '"');
            }
            if (chartMetadata.Contains("^^CompanyName^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CompanyName^^", '"' + _userContext.CompanyName + '"');
            }
            if (chartMetadata.Contains("^^Email^^"))
            {
                chartMetadata = chartMetadata.Replace("^^Email^^", '"' + _userContext.Email + '"');
            }
            if (chartMetadata.Contains("^^JobTitle^^"))
            {
                chartMetadata = chartMetadata.Replace("^^JobTitle^^", '"' + _userContext.JobTitle + '"');
            }
            if (chartMetadata.Contains("^^PhotoId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^PhotoId^^", '"' + _userContext.PhotoId + '"');
            }
            if (chartMetadata.Contains("^^IsSystemAdmin^^"))
            {
                chartMetadata = chartMetadata.Replace("^^IsSystemAdmin^^", '"' + _userContext.IsSystemAdmin.ToString() + '"');
            }
            if (chartMetadata.Contains("^^IsGuestUser^^"))
            {
                chartMetadata = chartMetadata.Replace("^^IsGuestUser^^", '"' + _userContext.IsGuestUser.ToString() + '"');
            }
            if (chartMetadata.Contains("^^UserRoleIds^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserRoleIds^^", '"' + _userContext.UserRoleIds + '"');
            }
            if (chartMetadata.Contains("^^UserRoleCodes^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserRoleCodes^^", '"' + _userContext.UserRoleCodes + '"');
            }
            if (chartMetadata.Contains("^^PortalTheme^^"))
            {
                chartMetadata = chartMetadata.Replace("^^PortalTheme^^", '"' + _userContext.PortalTheme + '"');
            }
            if (chartMetadata.Contains("^^UserPortals^^"))
            {
                chartMetadata = chartMetadata.Replace("^^UserPortals^^", '"' + _userContext.UserPortals + '"');
            }
            if (chartMetadata.Contains("^^PortalId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^PortalId^^", '"' + _userContext.PortalId + '"');
            }
            if (chartMetadata.Contains("^^CurrentDate^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CurrentDate^^", '"' + _userContext.GetLocalNow.Day.ToString() + '"');
            }
            if (chartMetadata.Contains("^^CurrentDateTime^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CurrentDateTime^^", '"' + _userContext.GetLocalNow.ToString() + '"');
            }
            if (chartMetadata.Contains("^^LoggedInAsType^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LoggedInAsType^^", '"' + _userContext.LoggedInAsType.ToString() + '"');
            }
            if (chartMetadata.Contains("^^LoggedInAsByUserId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LoggedInAsByUserId^^", '"' + _userContext.LoggedInAsByUserId + '"');
            }
            if (chartMetadata.Contains("^^LoggedInAsByUserName^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LoggedInAsByUserName^^", '"' + _userContext.LoggedInAsByUserName + '"');
            }
            if (chartMetadata.Contains("^^LegalEntityId^^"))
            {
                chartMetadata = chartMetadata.Replace("^^LegalEntityId^^", '"' + _userContext.LegalEntityId + '"');
            }
            if (chartMetadata.Contains("^^CultureName^^"))
            {
                chartMetadata = chartMetadata.Replace("^^CultureName^^", '"' + _userContext.CultureName + '"');
            }
            return chartMetadata;
        }
        public async Task<IActionResult> PreviewDashboard(string noteId)
        {
            try
            {
                var res = "";
                var pagedata = await _noteBusiness.GetDashboardMasterDetails(noteId);
                if (pagedata != null)
                    if (pagedata.layoutMetadata.IsNotNullAndNotEmpty())
                    {
                        res = _webApi.AddHost(pagedata.layoutMetadata);
                        //return Json(res);
                    }
                pagedata.Json = res;
                pagedata.ChartItems = await BuildChart(res);
                return View(pagedata);
                //var dashboard = new DashboardMasterViewModel { NoteId = noteId };
                //var dashboardItems = await _noteBusiness.GetList(x => x.ParentNoteId == noteId);
                //var chartItems = new List<DashboardItemMasterViewModel>();
                //var chartCum = "";
                //foreach (var item in dashboardItems)
                //{
                //    var dashboardItem = await _noteBusiness.GetDashboardItemDetailsWithChartTemplate(item.Id);
                //    var chartInput = dashboardItem.chartMetadata;
                //    var chartBPCode = dashboardItem.boilerplateCode;
                //    chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
                //    chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
                //    chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");

                //    dashboardItem.chartMetadata = chartBPCode;
                //    chartCum = chartCum + " \n " + chartBPCode;
                //    chartItems.Add(dashboardItem);
                //}
                //chartItems.First().chartMetadata = chartCum;
                //dashboard.chartItems = chartItems;
                //return View(dashboard);

            }
            catch (Exception e)
            {
                return View(new DashboardMasterViewModel());
            }

        }
        public async Task<IActionResult> PreviewPageTemplate(string templateId)
        {
            try
            {
                var res = "";
                var pagedata = await _templateBusiness.GetSingleById(templateId);
                if (pagedata != null)
                {
                    if (pagedata.Json.IsNotNullAndNotEmpty())
                    {
                        res = _webApi.AddHost(pagedata.Json);

                    }

                }
                var model = new DashboardMasterViewModel();
                model.Json = res;
                model.ChartItems = await BuildChart(res);
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
                return View("PreviewDashboard", model);


            }
            catch (Exception e)
            {
                return View(new DashboardMasterViewModel());
            }

        }
        public async Task<IActionResult> DrillDownReport(string chartName, string param)
        {
            try
            {
                var dashboardItem = await _noteBusiness.GetDashboardItemDetailsByName(chartName);
                if (!dashboardItem.IsNotNull())
                {
                    return View(new DashboardItemMasterViewModel());
                }
                var layers = await _noteBusiness.GetMapLayerItemList(dashboardItem.NoteId);
                string layersString = null;
                if (layers.Count > 0)
                {
                    layersString = JsonConvert.SerializeObject(layers);
                }
                var chartInput = dashboardItem.chartMetadata;
                var chartBPCode = dashboardItem.boilerplateCode;
                var clickFunction = dashboardItem.onChartClickFunction;
                var filters = JsonConvert.DeserializeObject<List<DashboardItemFilterViewModel>>(dashboardItem.filterField);
                //var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
                //if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
                //{
                //    timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

                //}
                if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
                {
                    var metadataArray = chartInput.Split("],");
                    var filstr = "filters: [";

                    if (param.IsNotNullAndNotEmpty())
                    {
                        var parameters = param.Split('&');
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var para = parameters[i].Split('=');
                            var typeTest = "^^" + para[0] + "^^";
                            foreach (var filter in filters)
                            {
                                if (filter.FilterText.Contains("^^") && filter.DefaultValue == "All" && para[1].IsNullOrEmpty())
                                {
                                    continue;
                                }
                                else if (filter.FilterText != typeTest)
                                {
                                    continue;
                                }
                                else if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                                {
                                    var value = "";
                                    if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                    {
                                        value = para[1];
                                    }
                                    else
                                    {
                                        value = '"' + para[1] + '"';
                                    }
                                    var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                    filstr += fil;
                                }
                                else if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                                {
                                    var value = "";
                                    if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                    {
                                        value = filter.DefaultValue;
                                    }
                                    else
                                    {
                                        value = '"' + filter.DefaultValue + '"';
                                    }
                                    var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                    filstr += fil;
                                }
                            }

                        }
                        filstr += "]";

                        chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
                    }
                    else
                    {
                        foreach (var filter in filters)
                        {
                            if (filter.FilterText.Contains("^^") && filter.DefaultValue == "All")
                            {
                                continue;
                            }
                            else if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                            {
                                var value = "";
                                if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                {
                                    value = filter.DefaultValue;
                                }
                                else
                                {
                                    value = '"' + filter.DefaultValue + '"';
                                }
                                var fil = "{\"member\": \"" + filter.FilterField + "\",\"operator\": \"" + filter.FilterOperator + "\",\"values\": [" + value + "]},";
                                filstr += fil;
                            }

                        }

                        filstr += "]";

                        chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + metadataArray[3];
                    }
                    chartInput = ReplaceChartSessionValue(chartInput);
                }
                else if (chartInput.Contains("^^"))
                {
                    chartInput = ReplaceChartSessionValue(chartInput);
                    if (param.IsNotNullAndNotEmpty())
                    {
                        var parameters = param.Split('&');
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var para = parameters[i].Split('=');
                            foreach (var filter in filters)
                            {
                                if (filter.FilterText.Contains("^^") && filter.FilterText.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                                {
                                    var value = "";
                                    if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                                    {
                                        value = para[1];
                                    }
                                    else
                                    {
                                        value = '"' + para[1] + '"';
                                    }
                                    chartInput = chartInput.Replace(filter.FilterText, value);
                                }

                            }


                        }
                    }
                    foreach (var filter in filters)
                    {
                        if (filter.FilterText.Contains("^^") && filter.DefaultValue.IsNotNullAndNotEmpty())
                        {
                            var value = "";
                            if (filter.FilterOperator == "inDateRange" || filter.FilterOperator == "notInDateRange" || filter.FilterOperator == "afterDate" || filter.FilterOperator == "beforeDate")
                            {
                                value = filter.DefaultValue;
                            }
                            else
                            {
                                value = '"' + filter.DefaultValue + '"';
                            }
                            chartInput = chartInput.Replace(filter.FilterText, value);
                        }

                    }

                }
                var clickFunctionName = "On" + chartName.Replace(" ", string.Empty) + "Click";
                chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
                chartBPCode = chartBPCode.Replace("@@xaxis@@", dashboardItem.Xaxis);
                chartBPCode = chartBPCode.Replace("@@yaxis@@", dashboardItem.Yaxis);
                chartBPCode = chartBPCode.Replace("@@Count@@", dashboardItem.Count);
                chartBPCode = chartBPCode.Replace("@@DataSource@@", layersString);
                chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
                chartBPCode = chartBPCode.Replace("@@chartHtml@@", dashboardItem.NoteDescription);
                chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
                chartBPCode = chartBPCode.Replace("@@mapUrl@@", dashboardItem.mapUrl);
                chartBPCode = chartBPCode.Replace("@@mapLayer@@", dashboardItem.mapLayer);
                chartBPCode = chartBPCode.Replace("@@mode@@", dashboardItem.ThemeMode);
                chartBPCode = chartBPCode.Replace("@@palette@@", dashboardItem.Palette);
                chartBPCode = chartBPCode.Replace("@@monochrome@@", dashboardItem.MonocromeColor.IsNotNullAndNotEmpty() ? "true" : "false");
                chartBPCode = chartBPCode.Replace("@@color@@", dashboardItem.MonocromeColor);
                chartBPCode = chartBPCode.Replace("OnSeriesClick", clickFunctionName);
                chartBPCode = chartBPCode.Replace("@@Latitude@@", "51.505");
                chartBPCode = chartBPCode.Replace("@@Longitude@@", "-0.09");
                chartBPCode = chartBPCode.Replace("@@LocationName@@", "London");
                if (clickFunction.IsNotNullAndNotEmpty())
                {
                    clickFunction = clickFunction.Replace("OnSeriesClick", clickFunctionName);
                    chartBPCode += clickFunction;
                }
                dashboardItem.chartMetadata = chartBPCode;
                dashboardItem.ChartKey = chartName.Replace(" ", string.Empty);
                return View(dashboardItem);

            }
            catch (Exception e)
            {
                return View(new DashboardItemMasterViewModel());
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetDashboardJson(string noteId)
        {
            var res = "";
            var pagedata = await _noteBusiness.GetDashboardMasterDetails(noteId);
            if (pagedata != null)
                if (pagedata.layoutMetadata.IsNotNullAndNotEmpty())
                {
                    res = _webApi.AddHost(pagedata.layoutMetadata);
                    //return Json(res);
                }
            return Json(res);
        }
        public async Task<IActionResult> DashboardItem(string id, string parentId, DataActionEnum dataAction, string layout)
        {
            var model = new DashboardItemMasterViewModel();
            //await GetMeasureDimensionsData(model);
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DashboardItem";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.ParentNoteId = parentId;
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.Layout = layout;
                model.filters = "[]";
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetDashboardItemMasterDetails(id);
                if (udf.IsNotNull())
                {
                    model.chartTypeId = udf.chartTypeId;
                    model.chartMetadata = udf.chartMetadata;
                    model.mapLayer = udf.mapLayer;
                    model.mapUrl = udf.mapUrl;
                    model.Help = udf.Help;
                    model.ThemeMode = udf.ThemeMode;
                    model.Palette = udf.Palette;
                    model.MonocromeColor = udf.MonocromeColor;
                    model.onChartClickFunction = udf.onChartClickFunction;
                    model.DynamicMetadata = udf.DynamicMetadata;
                    model.Xaxis = udf.Xaxis;
                    model.Yaxis = udf.Yaxis;
                    model.Count = udf.Count;
                    model.isLibrary = udf.isLibrary;
                    model.filters = udf.filters == null ? "[]" : udf.filters;
                    model.timeField = udf.timeField;
                    model.granularity = udf.granularity;
                    model.rangeType = udf.rangeType;
                    if (udf.measuresField.IsNotNullAndNotEmpty())
                    {
                        model.measuresField = udf.measuresField;
                    }
                    if (udf.dimensionsField.IsNotNullAndNotEmpty())
                    {
                        //model.dimensionsArray = udf.dimensionsField.Split(',');
                        model.dimensionsField = udf.dimensionsField.Replace(",", "','");
                    }
                    if (udf.segmentsField.IsNotNullAndNotEmpty())
                    {
                        //model.segmentsArray = udf.segmentsField.Split(',');
                        model.segmentsField = udf.segmentsField.Replace(",", "','");
                    }
                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteDescription = newmodel.NoteDescription;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.Layout = layout;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageDashboardItemNote(DashboardItemMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filterList = new List<DashboardItemFilterViewModel>();
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "DashboardItem");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    if (model.chartTypeId.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Please Select Chart Type !" });
                    }
                    var chart = await _noteBusiness.GetSingleById(model.chartTypeId);
                    if (!chart.NoteSubject.ToLower().Contains("iframe"))
                    {
                        if (model.filters.IsNotNullAndNotEmpty() && model.filters != "[]")
                        {
                            var qr = JsonConvert.DeserializeObject<dynamic>(model.filters);
                            var con = qr.condition;
                            var op = con.Value;
                            var rules = qr.rules;
                            var cubejsquery = "{";
                            cubejsquery = await ReadFilterJson(rules, op, cubejsquery, filterList);
                            cubejsquery += "}";
                            JObject json = JObject.Parse(cubejsquery);
                            string jsonStr = JsonConvert.SerializeObject(json);
                            string jsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: [" + jsonFormatted + "]," + metadataArray[3] + "]";
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: []," + metadataArray[3] + "]";
                        }
                        if (filterList.Count > 0)
                        {
                            model.filterField = JsonConvert.SerializeObject(filterList);
                        }
                        else
                        {
                            model.filterField = "[]";
                        }
                        if (model.timeField.IsNotNullAndNotEmpty())
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeField);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            timeQuery = timeQuery.Replace("#DURATION#", model.rangeType);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionsField = timejsonFormatted;
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: [" + timejsonFormatted + "]";
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: []";
                        }
                    }
                    else
                    {
                        model.filterField = "[]";
                    }

                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DashboardItem";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "DashboardItem");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var chart = await _noteBusiness.GetSingleById(model.chartTypeId);
                    if (!chart.NoteSubject.ToLower().Contains("iframe"))
                    {
                        if (model.filters.IsNotNullAndNotEmpty() && model.filters != "[]")
                        {
                            var qr = JsonConvert.DeserializeObject<dynamic>(model.filters);
                            var con = qr.condition;
                            var op = con.Value;
                            var rules = qr.rules;
                            var cubejsquery = "{";
                            cubejsquery = await ReadFilterJson(rules, op, cubejsquery, filterList);
                            cubejsquery += "}";
                            JObject json = JObject.Parse(cubejsquery);
                            string jsonStr = JsonConvert.SerializeObject(json);
                            string jsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: [" + jsonFormatted + "]," + metadataArray[3];
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: []," + metadataArray[3] + "]";
                        }
                        if (filterList.Count > 0)
                        {
                            model.filterField = JsonConvert.SerializeObject(filterList);
                        }
                        else
                        {
                            model.filterField = "[]";
                        }
                        if (model.timeField.IsNotNullAndNotEmpty())
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeField);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            timeQuery = timeQuery.Replace("#DURATION#", model.rangeType);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionsField = timejsonFormatted;
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: [" + timejsonFormatted + "]";
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: []";
                        }
                    }
                    else
                    {
                        model.filterField = "[]";
                    }

                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

        public async Task<IActionResult> GridStackDashboardItem(string id, string parentId, DataActionEnum dataAction, string layout)
        {
            var model = new DashboardItemMasterViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DashboardItem";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.ParentNoteId = parentId;
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.Layout = layout;
                model.filters = "[]";
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetDashboardItemMasterDetails(id);
                if (udf.IsNotNull())
                {
                    model.chartTypeId = udf.chartTypeId;
                    model.chartMetadata = udf.chartMetadata;
                    model.mapLayer = udf.mapLayer;
                    model.mapUrl = udf.mapUrl;
                    model.Help = udf.Help;
                    model.ThemeMode = udf.ThemeMode;
                    model.Palette = udf.Palette;
                    model.MonocromeColor = udf.MonocromeColor;
                    model.onChartClickFunction = udf.onChartClickFunction;
                    model.DynamicMetadata = udf.DynamicMetadata;
                    model.Xaxis = udf.Xaxis;
                    model.Yaxis = udf.Yaxis;
                    model.Count = udf.Count;
                    model.isLibrary = udf.isLibrary;
                    model.filters = udf.filters == null ? "[]" : udf.filters;
                    model.timeField = udf.timeField;
                    model.granularity = udf.granularity;
                    model.rangeType = udf.rangeType;
                    if (udf.measuresField.IsNotNullAndNotEmpty())
                    {
                        model.measuresField = udf.measuresField;
                    }
                    if (udf.dimensionsField.IsNotNullAndNotEmpty())
                    {
                        model.dimensionsField = udf.dimensionsField.Replace(",", "','");
                    }
                    if (udf.segmentsField.IsNotNullAndNotEmpty())
                    {
                        model.segmentsField = udf.segmentsField.Replace(",", "','");
                    }
                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteDescription = newmodel.NoteDescription;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.Layout = layout;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageGridStackDashboardItemNote(DashboardItemMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filterList = new List<DashboardItemFilterViewModel>();
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "DashboardItem");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    if (model.chartTypeId.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Please Select Chart Type !" });
                    }
                    var chart = await _noteBusiness.GetSingleById(model.chartTypeId);
                    if (!chart.NoteSubject.ToLower().Contains("iframe"))
                    {
                        if (model.filters.IsNotNullAndNotEmpty())
                        {
                            var qr = JsonConvert.DeserializeObject<dynamic>(model.filters);
                            var con = qr.condition;
                            var op = con.Value;
                            var rules = qr.rules;
                            var cubejsquery = "{";
                            cubejsquery = await ReadFilterJson(rules, op, cubejsquery, filterList);
                            cubejsquery += "}";
                            JObject json = JObject.Parse(cubejsquery);
                            string jsonStr = JsonConvert.SerializeObject(json);
                            string jsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: [" + jsonFormatted + "]," + metadataArray[3] + "]";
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: []," + metadataArray[3] + "]";
                        }
                        if (filterList.Count > 0)
                        {
                            model.filterField = JsonConvert.SerializeObject(filterList);
                        }
                        else
                        {
                            model.filterField = "[]";
                        }
                        if (model.timeField.IsNotNullAndNotEmpty())
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeField);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            timeQuery = timeQuery.Replace("#DURATION#", model.rangeType);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionsField = timejsonFormatted;
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: [" + timejsonFormatted + "]";
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: []";
                        }
                    }


                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DashboardItem";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, name = model.NoteSubject, id = newmodel.NoteId });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "DashboardItem");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var chart = await _noteBusiness.GetSingleById(model.chartTypeId);
                    if (!chart.NoteSubject.ToLower().Contains("iframe"))
                    {
                        if (model.filters.IsNotNullAndNotEmpty())
                        {
                            var qr = JsonConvert.DeserializeObject<dynamic>(model.filters);
                            var con = qr.condition;
                            var op = con.Value;
                            var rules = qr.rules;
                            var cubejsquery = "{";
                            cubejsquery = await ReadFilterJson(rules, op, cubejsquery, filterList);
                            cubejsquery += "}";
                            JObject json = JObject.Parse(cubejsquery);
                            string jsonStr = JsonConvert.SerializeObject(json);
                            string jsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: [" + jsonFormatted + "]," + metadataArray[3] + "]";
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "],filters: []," + metadataArray[3] + "]";
                        }
                        if (filterList.Count > 0)
                        {
                            model.filterField = JsonConvert.SerializeObject(filterList);
                        }
                        else
                        {
                            model.filterField = "[]";
                        }
                        if (model.timeField.IsNotNullAndNotEmpty())
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeField);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            timeQuery = timeQuery.Replace("#DURATION#", model.rangeType);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionsField = timejsonFormatted;
                            if (model.chartMetadata.Contains("measures"))
                            {
                                var metadataArray = model.chartMetadata.Split("],");
                                model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: [" + timejsonFormatted + "]";
                            }
                        }
                        else
                        {
                            var metadataArray = model.chartMetadata.Split("],");
                            model.chartMetadata = metadataArray[0] + "]," + metadataArray[1] + "]," + metadataArray[2] + "],timeDimensions: []";
                        }
                    }

                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, name = model.NoteSubject, id = newmodel.NoteId });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

        [HttpPost]
        public async Task<IActionResult> GridStacklayoutMetaData(string data, string id)
        {
            data = data.Replace("'", "^");
            await _noteBusiness.UpdateModel(data, id);
            return Json(new { success = true });


        }
        [HttpGet]
        public async Task<JsonResult> GetChartTemplateList()
        {
            var data = await _noteBusiness.GetAllChartTemplate();
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetChartTemplate(string id)
        {
            var data = await _noteBusiness.GetAllChartTemplate();
            var res = data.Where(x => x.Id == id).FirstOrDefault();
            return Json(res);
        }
        [HttpGet]
        public async Task<JsonResult> GetDashboardIdNameList()
        {
            var data = await _noteBusiness.GetAllGridStackDashboard();
            var list = data.Select(x => new IdNameViewModel { Id = x.Id, Name = x.NoteSubject }).ToList();
            return Json(list);
        }
        public async Task<JsonResult> GetFilters([DataSourceRequest] DataSourceRequest request, string id)
        {
            var udf = await _noteBusiness.GetDashboardItemMasterDetails(id);
            var data = new List<DashboardItemFilterViewModel>();
            if (udf.IsNotNull())
            {
                data = JsonConvert.DeserializeObject<List<DashboardItemFilterViewModel>>(udf.filterField);
            }
            var dsResult = data;
            // var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<JsonResult> GetTimeDimensions([DataSourceRequest] DataSourceRequest request, string id)
        {
            var udf = await _noteBusiness.GetDashboardItemMasterDetails(id);
            var data = new List<DashboardItemTimeDimensionViewModel>();
            if (udf.IsNotNull())
            {
                data = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(udf.timeDimensionsField);
            }
            var dsResult = data;
            //var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [HttpGet]
        public async Task GetCubeJsData(DashboardItemMasterViewModel viewModel)
        {
            var url = $@"{_webApi.GetCubeJSBaseUrl()}cubejs-api/v1/meta";
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            using (var httpClient = new HttpClient(handler))
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                var json = await response.Content.ReadAsStringAsync();
                var data1 = JToken.Parse(json);
                JArray rows = (JArray)data1.SelectToken("cubes");
                var str = JsonConvert.SerializeObject(rows);
                var model = JsonConvert.DeserializeObject<List<CubeJsViewModel>>(str);
                var measurementModel = new List<MeasuresViewModel>();
                var dimensionsModel = new List<DimensionsViewModel>();
                var segmentModel = new List<SegmentsViewModel>();
                foreach (var item in model)
                {
                    if (item.measures.Count > 0)
                    {
                        measurementModel.AddRange(item.measures);
                    }
                    if (item.dimensions.Count > 0)
                    {
                        dimensionsModel.AddRange(item.dimensions);
                    }
                    if (item.segments.Count > 0)
                    {
                        segmentModel.AddRange(item.segments);
                    }
                }
                viewModel.measures = measurementModel;
                viewModel.dimensions = dimensionsModel;
                viewModel.segments = segmentModel;
            }

        }
        [HttpGet]
        private async Task GetMeasureDimensionsData(DashboardItemMasterViewModel viewModel)
        {
            var measurementModel = await _noteBusiness.GetMeasures();
            var dimensionsModel = await _noteBusiness.GetDimensions();
            viewModel.measures = measurementModel;
            viewModel.dimensions = dimensionsModel;
        }
        public async Task<IActionResult> SocialWebsiteScrapping(string watchlistId, string keyword, string track, string ruturnPageName, string layout, bool showIncident = false, bool hideback = false)
        {
            //await RssFeedFileGenerateUsingHangfire();
            //await ReadCamera1Data();
            //await ReadCamera2Data();
            //await ImprotDataFromExcelForCamera1();
            //await ImprotDataFromExcelForCamera2();
            //await ImprotDataFromExcelForSampleData();
            //var list1 = await _noteBusiness.GetTwitterData();
            //var settings = new ConnectionSettings(new Uri("http://178.238.236.213:9200/"));
            //var client = new ElasticClient(settings);
            //var bulkIndexResponse = client.Bulk(b => b
            //                        .Index("twitter2")
            //                        .IndexMany(list1)
            //                    );


            //var list2 = await _noteBusiness.GetFacebookData("");
            //var bulkIndexResponse2 = client.Bulk(b => b
            //                        .Index("facebook1")
            //                        .IndexMany(list2)
            //                    );
            //var list3 = await _noteBusiness.GetInstagramData();
            //var bulkIndexResponse3 = client.Bulk(b => b
            //                        .Index("insta")
            //                        .IndexMany(list3)
            //                    );
            //var list4 = await _noteBusiness.GetYoutubeData();
            //var bulkIndexResponse4 = client.Bulk(b => b
            //                        .Index("youtube1")
            //                        .IndexMany(list4)
            //                    );
            //var list5 = await _noteBusiness.GetWhatsAppData();
            //var bulkIndexResponse5 = client.Bulk(b => b
            //                        .Index("whatsapp1")
            //                        .IndexMany(list5)
            //                    );
            if (watchlistId.IsNotNullAndNotEmpty())
            {
                var watchlist = await _noteBusiness.GetWatchlistDetails(watchlistId);
                ViewBag.IsAdvance = watchlist.isAdvance;
                ViewBag.PlainSearch = watchlist.plainSearch;
                ViewBag.DateFilterType = watchlist.dateFilterType;
                ViewBag.StartDate = watchlist.startDate;
                ViewBag.EndDate = watchlist.endDate;
            }
            if (track.IsNotNullAndNotEmpty())
            {
                var model = await _noteBusiness.GetSingle(x => x.TemplateCode == "TRACK_MASTER" && x.NoteSubject == track);
                if (model.IsNotNull())
                {
                    var keywords = await _noteBusiness.GetKeywordListByTrackId(model.Id);
                    if (keywords.Count > 0)
                    {
                        var searchStr = string.Join(" ", keywords);
                        ViewBag.KeyWord = searchStr;
                        ViewBag.Track = track;
                    }
                }
            }
            else
            {
                ViewBag.KeyWord = keyword;
            }
            ViewBag.RuturnPageName = ruturnPageName;
            ViewBag.Layout = layout;
            ViewBag.ShowIncident = showIncident;
            ViewBag.HideBack = hideback;
            var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
            ViewBag.ApiList = apilist.Where(x => x.Url.ToLower().Contains(x.NoteSubject.ToLower())).ToList();
            //ViewBag.ApiList = apilist.Where(x => x.Url.ToLower().Contains("get_xtra_get_fir")).ToList();
            return View();
        }
        public async Task<IActionResult> ReadTwitterData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            //var list = new List<Twitter1ViewModel>();
            //var url = "https://api.twitter.com/2/tweets/search/recent?query=" + searchStr;
            //using (var httpClient = new HttpClient())
            //{
            //    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAALGQPQEAAAAAO%2F6Bh8iW9lNBaTmsAtY%2BsPYUgjc%3DZVbyg27R3pokVj1OXr9wV7uorkzJRdE3qtF7mKHc8K3whRR8jl");
            //    var address = new Uri(url);
            //    var response = await httpClient.GetAsync(address);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonStr = await response.Content.ReadAsStringAsync();
            //        var json = JToken.Parse(jsonStr);
            //        var items = json.SelectToken("data");
            //        if (items.IsNotNull())
            //        {
            //            foreach (var item in items)
            //            {

            //                var dataStr = JsonConvert.SerializeObject(item);
            //                var model = JsonConvert.DeserializeObject<Twitter1ViewModel>(dataStr);
            //                if (model.IsNotNull())
            //                {
            //                    list.Add(model);
            //                }

            //            }
            //        }
            //    }


            //}
            if (searchStr.IsNullOrEmpty())
            {
                searchStr = "bhopal";
            }
            var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
            var list = new List<Twitter1ViewModel>();
            var url = socialApiUrl + "tweet_and_sentiment?keyword=" + searchStr.Trim();
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var data = json.SelectToken("data");
                    if (data.IsNotNull())
                    {
                        var items = data.SelectToken("data");
                        if (items.IsNotNull())
                        {
                            foreach (var item in items)
                            {


                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<Twitter1ViewModel>(dataStr);
                                if (model.IsNotNull())
                                {
                                    var _referenced_tweets = item.SelectToken("referenced_tweets");
                                    if (_referenced_tweets.IsNotNull() && _referenced_tweets.Count() > 0)
                                    {
                                        _referenced_tweets = _referenced_tweets.First();
                                        var referenced_tweets = JsonConvert.SerializeObject(_referenced_tweets);
                                        var referenced_tweets_model = JsonConvert.DeserializeObject<referenced_tweets>(referenced_tweets);
                                        if (referenced_tweets_model.IsNotNull())
                                        {
                                            model.type = referenced_tweets_model.type;
                                        }
                                    }
                                    var _public_metrics = item.SelectToken("public_metrics");
                                    var public_metrics = JsonConvert.SerializeObject(_public_metrics);
                                    var public_metricss_model = JsonConvert.DeserializeObject<public_metrics>(public_metrics);
                                    if (public_metricss_model.IsNotNull())
                                    {
                                        model.retweet_count = public_metricss_model.retweet_count;
                                        model.reply_count = public_metricss_model.reply_count;
                                        model.like_count = public_metricss_model.like_count;
                                        model.quote_count = public_metricss_model.quote_count;
                                    }
                                    var _polarity = item.SelectToken("polarity");
                                    var polarity = JsonConvert.SerializeObject(_polarity);
                                    var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                    if (polarity_model.IsNotNull())
                                    {
                                        model.pos = polarity_model.pos;
                                        model.neg = polarity_model.neg;
                                        model.neu = polarity_model.neu;
                                        model.compound = polarity_model.compound;
                                    }
                                    list.Add(model);
                                }

                            }

                        }
                    }

                }



            }
            return Json(list);


        }
        public async Task<IActionResult> ReadFacebookData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            if (searchStr.IsNullOrEmpty())
            {
                searchStr = "bhopal";
            }
            var list = new List<FacebookPostViewModel>();
            //var username = "8827426847";
            //var password = "Bibeksingh@23";
            var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
            var credential = await _noteBusiness.GetFacebookCredential();
            var url = socialApiUrl + "facebook_keyword_login?keyword=" + searchStr.Trim() + "&no_of_pages=1&username=" + credential.Name + "&password=" + credential.Code;
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(500);
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var data = json.SelectToken("data");
                    if (data.IsNotNull())
                    {
                        foreach (var item in data)
                        {
                            var dataStr = JsonConvert.SerializeObject(item);
                            var model = JsonConvert.DeserializeObject<FacebookPostViewModel>(dataStr);
                            list.Add(model);
                        }

                    }
                }


            }
            return Json(list);
        }
        public async Task<IActionResult> ReadInstagramData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            if (string.IsNullOrEmpty(searchStr))
            {
                searchStr = "bhopal";
            }
            var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
            var list = new List<InstagramPostViewModel>();
            var url = socialApiUrl + "gsearch_keyword?keyword=instagram " + searchStr.Trim() + "&no_of_pages=1";
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var data = json.SelectToken("data");
                    if (data.IsNotNull())
                    {
                        foreach (var item in data)
                        {

                            var dataStr = JsonConvert.SerializeObject(item);
                            var link = JsonConvert.DeserializeObject<string>(dataStr);
                            if (link.IsNotNull())
                            {
                                var model = new InstagramPostViewModel();
                                model.url = link;
                                model.created_date = DateTime.Now;
                                list.Add(model);
                            }

                        }
                    }
                }


            }
            return Json(list);
            //var model = new List<InstagramViewModel>();
            //var url = "http://178.238.236.213:9200/insta/_search";

            //if (searchStr.IsNullOrEmpty())
            //{
            //    var content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery1;
            //    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            //    using (var httpClient = new HttpClient())
            //    {
            //        var address = new Uri(url);
            //        //var response = await httpClient.GetAsync(address);
            //        var response = await httpClient.PostAsync(address, stringContent);
            //        var json = await response.Content.ReadAsStringAsync();


            //        var data = JToken.Parse(json);
            //        var data1 = data.SelectToken("hits");
            //        var data2 = data1.SelectToken("hits");
            //        foreach (var item in data2)
            //        {
            //            var source = item.SelectToken("_source");
            //            var str = JsonConvert.SerializeObject(source);
            //            var result = JsonConvert.DeserializeObject<InstagramViewModel>(str);

            //            model.Add(result);
            //        }

            //    }
            //}
            //else
            //{
            //    var content = "";
            //    if (plainSearch)
            //    {
            //        content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery2;
            //    }
            //    else if (!isAdvance)
            //    {
            //        content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery3;
            //    }
            //    else
            //    {
            //        content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery4;
            //    }
            //    content = content.Replace("#SEARCHWHERE#", searchStr);
            //    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            //    using (var httpClient = new HttpClient())
            //    {
            //        var address = new Uri(url);
            //        //var response = await httpClient.GetAsync(address);
            //        var response = await httpClient.PostAsync(address, stringContent);
            //        var json = await response.Content.ReadAsStringAsync();


            //        var data = JToken.Parse(json);
            //        var data1 = data.SelectToken("hits");
            //        var total = data1.SelectToken("total");
            //        var value = total.SelectToken("value");
            //        var data2 = data1.SelectToken("hits");
            //        foreach (var item in data2)
            //        {
            //            //var source = item.SelectToken("highlight");
            //            //var str = JsonConvert.SerializeObject(source);
            //            //var result = JsonConvert.DeserializeObject<InstagramArrayViewModel>(str);


            //            var source = item.SelectToken("_source");
            //            var highlights = item.SelectToken("highlight");
            //            var str = JsonConvert.SerializeObject(source);
            //            var strhighlight = JsonConvert.SerializeObject(highlights);
            //            var result = JsonConvert.DeserializeObject<InstagramViewModel>(str);
            //            var resultHighlight = JsonConvert.DeserializeObject<InstagramArrayViewModel>(strhighlight);


            //            if (result.IsNotNull())
            //            {
            //                var abc = new InstagramViewModel
            //                {
            //                    caption = (resultHighlight.IsNotNull() && resultHighlight.caption != null) ? string.Join("", resultHighlight.caption) : result.caption,
            //                    hashtags = (resultHighlight.IsNotNull() && resultHighlight.hashtags != null) ? string.Join("", resultHighlight.hashtags) : result.hashtags,
            //                    post_links = result.post_links,
            //                    image_links = result.image_links,
            //                    count = value.ToString(),
            //                    age = result.age,
            //                };
            //                model.Add(abc);
            //            }

            //        }

            //    }
            //}
            ////return Json(model.ToDataSourceResult(request));
            //return Json(model);

        }
        public async Task<IActionResult> ReadYoutubeData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            if (string.IsNullOrEmpty(searchStr))
            {
                searchStr = "bhopal";
            }
            var list = new List<Youtube1ViewModel>();
            var url = "https://youtube.googleapis.com/youtube/v3/search?part=snippet,id&maxResults=10&q=" + searchStr + "&type=video%2Clist&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var items = json.SelectToken("items");
                    if (items.IsNotNull())
                    {
                        foreach (var item in items)
                        {
                            var source = item.SelectToken("id");
                            var id = string.Empty;
                            if (source.IsNotNull())
                            {
                                var _id = source.SelectToken("videoId");
                                id = _id.Value<string>();
                            }
                            var _snippet = item.SelectToken("snippet");
                            var dataStr = JsonConvert.SerializeObject(_snippet);
                            var model = JsonConvert.DeserializeObject<Youtube1ViewModel>(dataStr);
                            if (model.IsNotNull())
                            {
                                model.Id = id;
                                list.Add(model);
                            }

                        }
                    }
                }



            }

            return Json(list);
            //var model = new List<YoutubeViewModel>();
            //var url = "http://178.238.236.213:9200/youtube1/_search";

            //if (searchStr.IsNullOrEmpty())
            //{
            //    var content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery1;
            //    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            //    using (var httpClient = new HttpClient())
            //    {
            //        var address = new Uri(url);
            //        //var response = await httpClient.GetAsync(address);
            //        var response = await httpClient.PostAsync(address, stringContent);
            //        var json = await response.Content.ReadAsStringAsync();


            //        var data = JToken.Parse(json);
            //        var data1 = data.SelectToken("hits");
            //        if (data1.IsNotNull())
            //        {
            //            var data2 = data1.SelectToken("hits");
            //            foreach (var item in data2)
            //            {
            //                var source = item.SelectToken("_source");
            //                var str = JsonConvert.SerializeObject(source);
            //                var result = JsonConvert.DeserializeObject<YoutubeViewModel>(str);

            //                model.Add(result);
            //            }
            //        }


            //    }
            //}
            //else
            //{
            //    var content = "";
            //    if (plainSearch)
            //    {
            //        content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery2;
            //    }
            //    else if (!isAdvance)
            //    {
            //        content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery3;
            //    }
            //    else
            //    {

            //        content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery4;
            //    }
            //    content = content.Replace("#SEARCHWHERE#", searchStr);
            //    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            //    using (var httpClient = new HttpClient())
            //    {

            //        var address = new Uri(url);
            //        //var response = await httpClient.GetAsync(address);
            //        var response = await httpClient.PostAsync(address, stringContent);
            //        var json = await response.Content.ReadAsStringAsync();


            //        var data = JToken.Parse(json);
            //        var data1 = data.SelectToken("hits");
            //        if (data1.IsNotNull())
            //        {
            //            var total = data1.SelectToken("total");
            //            var value = total.SelectToken("value");
            //            var data2 = data1.SelectToken("hits");
            //            foreach (var item in data2)
            //            {
            //                //var source = item.SelectToken("highlight");
            //                //var str = JsonConvert.SerializeObject(source);
            //                //var result = JsonConvert.DeserializeObject<YoutubeArrayViewModel>(str);


            //                var source = item.SelectToken("_source");
            //                var highlights = item.SelectToken("highlight");
            //                var str = JsonConvert.SerializeObject(source);
            //                var strhighlight = JsonConvert.SerializeObject(highlights);
            //                var result = JsonConvert.DeserializeObject<YoutubeViewModel>(str);
            //                var resultHighlight = JsonConvert.DeserializeObject<YoutubeArrayViewModel>(strhighlight);

            //                if (result.IsNotNull())
            //                {
            //                    var abc = new YoutubeViewModel
            //                    {
            //                        title = (resultHighlight.IsNotNull() && resultHighlight.title != null) ? string.Join("", resultHighlight.title) : string.Join("", result.title),
            //                        description = (resultHighlight.IsNotNull() && resultHighlight.description != null) ? string.Join("", resultHighlight.description) : string.Join("", result.description),
            //                        youtube_video_url = result.youtube_video_url,
            //                        count = value.ToString(),
            //                        publishtime = result.publishtime,
            //                    };
            //                    model.Add(abc);
            //                }

            //            }
            //        }


            //    }
            //}

            ////return Json(model.ToDataSourceResult(request));
            //return Json(model);

        }
        public async Task<IActionResult> ReadWhatsappData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<WhatsAppViewModel>();
            var url = "http://178.238.236.213:9200/whatsapp1/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadWhatsappDataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    if (data1.IsNotNull())
                    {
                        var data2 = data1.SelectToken("hits");
                        foreach (var item in data2)
                        {
                            var source = item.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<WhatsAppViewModel>(str);

                            model.Add(result);
                        }
                    }


                }
            }
            else
            {
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadWhatsappDataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadWhatsappDataQuery3;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadWhatsappDataQuery4;
                }
                content = content.Replace("#SEARCHWHERE#", searchStr);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    if (data1.IsNotNull())
                    {
                        var total = data1.SelectToken("total");
                        var value = total.SelectToken("value");
                        var data2 = data1.SelectToken("hits");
                        foreach (var item in data2)
                        {
                            //var source = item.SelectToken("highlight");
                            //var str = JsonConvert.SerializeObject(source);
                            //var result = JsonConvert.DeserializeObject<WhatsAppArrayViewModel>(str);


                            var source = item.SelectToken("_source");
                            var highlights = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var strhighlight = JsonConvert.SerializeObject(highlights);
                            var result = JsonConvert.DeserializeObject<WhatsAppViewModel>(str);
                            var resultHighlight = JsonConvert.DeserializeObject<WhatsAppArrayViewModel>(strhighlight);


                            if (result.IsNotNull())
                            {
                                var abc = new WhatsAppViewModel
                                {
                                    user = (resultHighlight.IsNotNull() && resultHighlight.user != null) ? string.Join("", resultHighlight.user) : string.Join("", result.user),
                                    messages = (resultHighlight.IsNotNull() && resultHighlight.messages != null) ? string.Join("", resultHighlight.messages) : string.Join("", result.messages),
                                    count = value.ToString()
                                };
                                model.Add(abc);
                            }

                        }
                    }


                }
            }

            //return Json(model.ToDataSourceResult(request));
            return Json(model);

        }
        public async Task<IActionResult> ReadTwitterAdvanceData([DataSourceRequest] DataSourceRequest request, string searchStr, string oparator)
        {
            var model = new List<TwitterViewModel>();
            var url = "http://178.238.236.213:9200/twitter2/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadTwitterAdvanceDataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("_source");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<TwitterViewModel>(str);

                        model.Add(result);
                    }

                }
            }
            else
            {
                var strArray = searchStr.Trim().Split(" ");
                var searchMultiple = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    var query = "";
                    if (i == strArray.Length - 1)
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadTwitterAdvanceDataQuery2;
                    }
                    else
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadTwitterAdvanceDataQuery3;
                    }
                    query = query.Replace("#SEARCHWHERE#", strArray[i]);
                    query = query.Replace("#OPERATORWHERE#", oparator);
                    searchMultiple = searchMultiple + query;
                }

                var content = ApplicationConstant.BusinessAnalytics.ReadTwitterAdvanceDataQuery4;
                content = content.Replace("#SEARCHWHERE#", searchMultiple);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<TwitterArrayViewModel>(str);
                        if (result.IsNotNull())
                        {
                            var abc = new TwitterViewModel { text = result.text != null ? string.Join("", result.text) : "", count = value.ToString() };
                            model.Add(abc);
                        }

                    }

                }
            }
            return Json(model);
            //return Json(model.ToDataSourceResult(request));

        }
        public async Task<IActionResult> ReadFacebookAdvanceData([DataSourceRequest] DataSourceRequest request, string searchStr, string oparator)
        {
            var model = new List<FacebookViewModel>();
            var url = "http://178.238.236.213:9200/facebook1/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadFacebookAdvanceDataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("_source");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<FacebookViewModel>(str);

                        model.Add(result);
                    }

                }
            }
            else
            {
                var strArray = searchStr.Trim().Split(" ");
                var searchMultiple = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    var query = "";
                    if (i == strArray.Length - 1)
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadFacebookAdvanceDataQuery2;
                    }
                    else
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadFacebookAdvanceDataQuery3;
                    }
                    query = query.Replace("#SEARCHWHERE#", strArray[i]);
                    query = query.Replace("#OPERATORWHERE#", oparator);
                    searchMultiple = searchMultiple + query;
                }

                var content = ApplicationConstant.BusinessAnalytics.ReadFacebookAdvanceDataQuery4;
                content = content.Replace("#SEARCHWHERE#", searchMultiple);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<FacebookArrayViewModel>(str);
                        if (result.IsNotNull())
                        {
                            var abc = new FacebookViewModel { post_message = result.post_message != null ? string.Join("", result.post_message) : "", count = value.ToString() };
                            model.Add(abc);
                        }

                    }

                }
            }
            return Json(model);
            //return Json(model.ToDataSourceResult(request));
        }
        public async Task<IActionResult> ReadInstagramAdvanceData([DataSourceRequest] DataSourceRequest request, string searchStr, string oparator)
        {
            var model = new List<InstagramViewModel>();
            var url = "http://178.238.236.213:9200/insta/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadInstagramAdvanceDataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("_source");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<InstagramViewModel>(str);

                        model.Add(result);
                    }

                }
            }
            else
            {
                var strArray = searchStr.Trim().Split(" ");
                var searchMultiple = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    var query = "";
                    if (i == strArray.Length - 1)
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadInstagramAdvanceDataQuery2;
                    }
                    else
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadInstagramAdvanceDataQuery3;
                    }
                    query = query.Replace("#SEARCHWHERE#", strArray[i]);
                    query = query.Replace("#OPERATORWHERE#", oparator);
                    searchMultiple = searchMultiple + query;
                }

                var content = ApplicationConstant.BusinessAnalytics.ReadInstagramAdvanceDataQuery4;
                content = content.Replace("#SEARCHWHERE#", searchMultiple);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<InstagramArrayViewModel>(str);
                        if (result.IsNotNull())
                        {
                            var abc = new InstagramViewModel { image_links = result.caption != null ? string.Join("", result.caption) : "", count = value.ToString() };
                            model.Add(abc);
                        }

                    }

                }
            }
            return Json(model);
            // return Json(model.ToDataSourceResult(request));

        }
        public async Task<IActionResult> ReadYoutubeAdvanceData([DataSourceRequest] DataSourceRequest request, string searchStr, string oparator)
        {
            var model = new List<YoutubeViewModel>();
            var url = "http://178.238.236.213:9200/youtube1/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadYoutubeAdvanceDataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("_source");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<YoutubeViewModel>(str);
                        model.Add(result);
                    }

                }
            }
            else
            {
                var strArray = searchStr.Trim().Split(" ");
                var searchMultiple = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    var query = "";
                    if (i == strArray.Length - 1)
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadYoutubeAdvanceDataQuery2;
                    }
                    else
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadYoutubeAdvanceDataQuery3;
                    }
                    query = query.Replace("#SEARCHWHERE#", strArray[i]);
                    query = query.Replace("#OPERATORWHERE#", oparator);
                    searchMultiple = searchMultiple + query;
                }

                var content = ApplicationConstant.BusinessAnalytics.ReadYoutubeAdvanceDataQuery4;
                content = content.Replace("#SEARCHWHERE#", searchMultiple);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<YoutubeArrayViewModel>(str);
                        if (result.IsNotNull())
                        {
                            var abc = new YoutubeViewModel { description = result.description != null ? string.Join("", result.description) : "", count = value.ToString() };
                            model.Add(abc);
                        }

                    }

                }
            }

            return Json(model);
            //return Json(model.ToDataSourceResult(request));

        }
        public async Task<IActionResult> ReadWhatsappAdvanceData([DataSourceRequest] DataSourceRequest request, string searchStr, string oparator)
        {
            var model = new List<WhatsAppViewModel>();
            var url = "http://178.238.236.213:9200/whatsapp1/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadWhatsappAdvanceDataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("_source");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<WhatsAppViewModel>(str);
                        model.Add(result);
                    }

                }
            }
            else
            {
                var strArray = searchStr.Trim().Split(" ");
                var searchMultiple = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    var query = "";
                    if (i == strArray.Length - 1)
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadWhatsappAdvanceDataQuery2;
                    }
                    else
                    {
                        query = ApplicationConstant.BusinessAnalytics.ReadWhatsappAdvanceDataQuery3;
                    }
                    query = query.Replace("#SEARCHWHERE#", strArray[i]);
                    query = query.Replace("#OPERATORWHERE#", oparator);
                    searchMultiple = searchMultiple + query;
                }

                var content = ApplicationConstant.BusinessAnalytics.ReadWhatsappAdvanceDataQuery4;
                content = content.Replace("#SEARCHWHERE#", searchMultiple);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var result = JsonConvert.DeserializeObject<WhatsAppArrayViewModel>(str);
                        if (result.IsNotNull())
                        {
                            var abc = new WhatsAppViewModel { messages = result.messages != null ? string.Join("", result.messages) : "", count = value.ToString() };
                            model.Add(abc);
                        }

                    }

                }
            }

            return Json(model);
            //return Json(model.ToDataSourceResult(request));

        }
        public async Task<IActionResult> DbConnection()
        {
            return View();
        }
        public async Task<JsonResult> ReadDbConnectionData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var data = await _noteBusiness.GetDbConnectionData();
            var dsResult = data/*.ToDataSourceResult(request)*/;
            return Json(dsResult);
        }
        public async Task<IActionResult> ManageDbConnection(string id, DataActionEnum dataAction)
        {
            var model = new DbConnectionViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DB_CONNECTION";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetDbConnectionDetails(id);
                if (udf.IsNotNull())
                {
                    model.hostName = udf.hostName;
                    model.port = udf.port;
                    model.maintenanceDatabase = udf.maintenanceDatabase;
                    model.username = udf.username;
                    model.role = udf.role;
                    model.service = udf.service;
                    model.password = udf.password;
                    model.databaseType = udf.databaseType;

                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageDbConnection(DbConnectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "DB_CONNECTION");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DB_CONNECTION";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, id = result.Item.Id });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "DB_CONNECTION");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<IActionResult> DeleteDbConnection(string id)
        {
            await _noteBusiness.Delete(id);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ManageSocialMediaHighlight(string keyword, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {

            if (keyword.IsNotNullAndNotEmpty())
            {
                keyword = keyword.ToLower();
                var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == keyword.Trim() && x.TemplateCode == "SOCIAL_MEDIA_HIGHLIGHT");
                if (validateNote.Any())
                {
                    return Json(new { success = false, error = "Keyword is already Exist" });
                }
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "SOCIAL_MEDIA_HIGHLIGHT";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = keyword.Trim();
                newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(new WatchlistViewModel { isAdvance = isAdvance, plainSearch = plainSearch, dateFilterType = dateFilterType, startDate = startDate, endDate = endDate });
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false, error = "Keyword is required" });
        }
        public async Task<IActionResult> WatchListDashboard()
        {
            return View();
        }
        public async Task<IActionResult> SaveQueryDashboard()
        {
            var data = await _noteBusiness.GetList(x => x.TemplateCode == "SOCIAL_MEDIA_HIGHLIGHT");
            ViewBag.KeywordList = data.Select(x => new IdNameViewModel { Id = x.Id, Name = x.NoteSubject }).ToList();
            return View();
        }

        public async Task<IActionResult> CCTVDashboard()
        {
            return View();
        }
        public async Task<IActionResult> CCTV(string id, string alertId, string alertNumber, bool isUpdate, string color)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            IIPNotificationViewModel iipModel = new IIPNotificationViewModel();
            //var query = ApplicationConstant.BusinessAnalytics.ReadDail100QueryWithSearch;
            //if (query.IsNotNullAndNotEmpty())
            //{
            //    if (id.IsNotNullAndNotEmpty())
            //    {
            //        var souceId = id.Split(",")[0];
            //        query = query.Replace("#SEARCHWHERE#", souceId);

            //    }
            //    var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
            //    var handler = new HttpClientHandler();
            //    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //    handler.ServerCertificateCustomValidationCallback =
            //        (httpRequestMessage, cert, cetChain, policyErrors) =>
            //        {
            //            return true;
            //        };
            //    using (var httpClient = new HttpClient(handler))
            //    {
            //        var url = eldbUrl + "dail100/_search?pretty=true&size=10000";
            //        var address = new Uri(url);
            //        var response = await httpClient.PostAsync(address, stringContent);
            //        var jsontrack = await response.Content.ReadAsStringAsync();
            //        var trackdata = JToken.Parse(jsontrack);
            //        var hits = trackdata.SelectToken("hits");
            //        if (hits.IsNotNull())
            //        {
            //            var _hits = hits.SelectToken("hits");

            //            foreach (var hit in _hits)
            //            {
            //                var source = hit.SelectToken("_source");
            //                var model = JsonConvert.DeserializeObject<Dial100ViewModel>(source.ToString());
            //                iipModel = new IIPNotificationViewModel
            //                {
            //                    Incident = model.event_type,
            //                    IncidentSubType = model.event_subType,
            //                    IncidentNumber = model.event_number,
            //                    IncidentTime = model.event_time,
            //                    Latitude = Convert.ToDouble(model.latitude),
            //                    Longitude = Convert.ToDouble(model.longitude),
            //                    PoliceStation = model.police_Station,
            //                    DistrictCode = model.district_Code,
            //                    Remark = model.event_remark
            //                };
            //                break;
            //            }
            //        }
            //    }
            //}
            //var model1 = await _noteBusiness.GetIIPCamera();
            //ViewBag.MarkersCamera1 = model1.ToArray();
            //var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
            //ViewBag.ApiList = apilist.Where(x => x.Url.ToLower().Contains(x.NoteSubject.ToLower())).ToList();
            var vdp = await _noteBusiness.GetVDPDistrictByCode(iipModel.DistrictCode);
            ViewBag.DistrictId = vdp.IsNotNull() ? vdp.Id : null;
            ViewBag.Dial100Ids = id;
            ViewBag.AlertId = alertId;
            ViewBag.AlertNumber = alertNumber;
            ViewBag.Color = color;
            if (alertId.IsNotNullAndNotEmpty() && isUpdate)
            {
                var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[""#IDS#""]}}]}},""script"": { ""source"": ""ctx._source['isRead'] =true;DateFormat df = new SimpleDateFormat(\""yyyy-MM-dd'T'HH:mm:ss.SSS\"");\ndf.setTimeZone(TimeZone.getTimeZone(\""IST\""));\nDate date = new Date();ctx._source['open_datetime'] =df.format(date);""} }";
                content2 = content2.Replace("#IDS#", alertId);
                var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                var url2 = eldbUrl + "iip_alert_data/_update_by_query";
                var address2 = new Uri(url2);
                using (var httpClient = new HttpClient())
                {
                    var response2 = await httpClient.PostAsync(address2, stringContent2);
                }
            }
            return View(iipModel);
        }
        public async Task<IActionResult> ReadCCTVFootageData()
        {
            var model = new List<YoutubeViewModel>();
            model.Add(new YoutubeViewModel { ytid = 1, youtube_video_url = "http://localhost:44389/images/VideoSample.mp4" });
            model.Add(new YoutubeViewModel { ytid = 2, youtube_video_url = "http://localhost:44389/images/VideoSample.mp4" });
            model.Add(new YoutubeViewModel { ytid = 3, youtube_video_url = "http://localhost:44389/images/VideoSample.mp4" });
            model.Add(new YoutubeViewModel { ytid = 4, youtube_video_url = "http://localhost:44389/images/VideoSample.mp4" });
            model.Add(new YoutubeViewModel { ytid = 5, youtube_video_url = "http://localhost:44389/images/VideoSample.mp4" });
            model.Add(new YoutubeViewModel { ytid = 6, youtube_video_url = "http://localhost:44389/images/VideoSample.mp4" });
            return Json(model);

        }
        public async Task<IActionResult> ReadIIPCameraData(int radius, double latitude, double longitude)
        {
            var newlist = new List<IIPCameraViewModel>();
            var list = await _noteBusiness.GetIIPCamera();
            if (radius == 0)
            {
                foreach (var item in list)
                {
                    try
                    {
                        var p2 = new GeoCoordinate(Convert.ToDouble(item.Latitude.Trim()), Convert.ToDouble(item.Longitude.Trim()));
                        newlist.Add(item);

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
                return Json(newlist);
            }
            var p1 = new GeoCoordinate(latitude, longitude);
            foreach (var item in list)
            {
                try
                {
                    var p2 = new GeoCoordinate(Convert.ToDouble(item.Latitude.Trim()), Convert.ToDouble(item.Longitude.Trim()));
                    var valid = GetDistanceTo(p1, p2, radius);
                    if (valid)
                    {
                        newlist.Add(item);
                    }
                }
                catch (Exception)
                {

                    continue;
                }

            }
            return Json(newlist);


        }
        public async Task<IActionResult> ReadIIPCameraDataByPoliceStation(string ps, string ds)
        {
            var list = await _noteBusiness.GetIIPCamera();
            var newlist = list.Where(x => x.PoliceStation.IsNotNull() && x.PoliceStation.ToLower().Contains(ps.ToLower())).ToList();
            return Json(newlist);


        }
        public async Task<JsonResult> ReadSocialMediaWatchlistData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var data = await _noteBusiness.GetList(x => x.TemplateCode == "SOCIAL_MEDIA_HIGHLIGHT");
            //var dsResult = data.ToDataSourceResult(request);
            var dsResult = data;
            return Json(dsResult);
        }
        public async Task<IActionResult> TestDbConnection(string id)
        {
            try
            {
                var db = await _noteBusiness.GetDbConnectionDetails(id);
                if (db.databaseType == DatabaseTypeEnum.Postgresql.ToString())
                {
                    var cs = "Host=" + db.hostName + ";Username=" + db.username + ";Password=" + db.password + ";Database=" + db.maintenanceDatabase;

                    using var con = new NpgsqlConnection(cs);
                    con.Open();

                    var sql = "SELECT version()";
                    //var sql = "select table_name from information_schema.tables where table_type = 'BASE TABLE' limit 1";

                    using var cmd = new NpgsqlCommand(sql, con);
                    //var list = cmd.ExecuteScalar();
                    var version = cmd.ExecuteScalar().ToString();
                    if (version.IsNotNullAndNotEmpty())
                    {
                        return Json(new { success = true });

                    }
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {

                return Json(new { success = false });
            }


        }
        public async Task<IActionResult> GetAllTablesNameFromDB(string id)
        {
            try
            {
                var tables = await _noteBusiness.GetList(x => x.TemplateCode == "DB_TABLES" && x.ParentNoteId == id);
                var db = await _noteBusiness.GetDbConnectionDetails(id);
                if (db.databaseType == DatabaseTypeEnum.Postgresql.ToString())
                {
                    var cs = "Host=" + db.hostName + ";Username=" + db.username + ";Password=" + db.password + ";Database=" + db.maintenanceDatabase;

                    using var con = new NpgsqlConnection(cs);
                    await con.OpenAsync();
                    var sql = "select table_name as Name from information_schema.tables where table_type = 'BASE TABLE' limit 5";
                    using var cmd = new NpgsqlCommand(sql, con);
                    var reader = await cmd.ExecuteReaderAsync();
                    var data = await ConvertToObject<IdNameViewModel>(reader);
                    // var data = _autoMapper.Map<IDataReader, List<VM>>(reader);
                    await reader.CloseAsync();
                    if (tables.Count > 0)
                    {
                        //data=data.Where(x => tables.Any(z => z.NoteSubject == x.Name)).Select(x => { x.Code = "true"; return x; }).ToList();
                        foreach (var item in tables)
                        {
                            foreach (var dt in data)
                            {
                                if (item.NoteSubject == dt.Name)
                                {
                                    dt.Code = "true";

                                }
                                dt.title = dt.Name;
                                dt.lazy = true;
                                dt.key = dt.Id;
                            }
                        }
                    }
                    return Json(data);
                }
                return Json(new { success = false });

            }
            catch (Exception ex)
            {

                return Json(new { success = false });
            }


        }

        public async Task<object> GetAllTablesNameFromDBTree(string id)
        {
            try
            {
                var tables = await _noteBusiness.GetList(x => x.TemplateCode == "DB_TABLES" && x.ParentNoteId == id);
                var db = await _noteBusiness.GetDbConnectionDetails(id);
                if (db.databaseType == DatabaseTypeEnum.Postgresql.ToString())
                {
                    var cs = "Host=" + db.hostName + ";Username=" + db.username + ";Password=" + db.password + ";Database=" + db.maintenanceDatabase;

                    using var con = new NpgsqlConnection(cs);
                    await con.OpenAsync();
                    var sql = "select table_name as Name from information_schema.tables where table_type = 'BASE TABLE' limit 5";
                    using var cmd = new NpgsqlCommand(sql, con);
                    var reader = await cmd.ExecuteReaderAsync();
                    var data = await ConvertToObject<IdNameViewModel>(reader);
                    // var data = _autoMapper.Map<IDataReader, List<VM>>(reader);
                    await reader.CloseAsync();
                    if (tables.Count > 0)
                    {
                        //data=data.Where(x => tables.Any(z => z.NoteSubject == x.Name)).Select(x => { x.Code = "true"; return x; }).ToList();
                        foreach (var item in tables)
                        {
                            foreach (var dt in data)
                            {
                                if (item.NoteSubject == dt.Name)
                                {
                                    dt.Code = "true";
                                    dt.selected = true;
                                }
                                dt.title = dt.Name;
                                dt.lazy = true;
                                dt.key = dt.Id;
                            }
                        }
                    }
                    var json = JsonConvert.SerializeObject(data);
                    return json;
                    //  return Json(data);
                }
                return Json(new { success = false });

            }
            catch (Exception ex)
            {

                return Json(new { success = false });
            }


        }
        [HttpPost]
        public async Task<IActionResult> ManageDbTables(string subject, string parentId)
        {

            if (subject.IsNotNullAndNotEmpty() && parentId.IsNotNullAndNotEmpty())
            {
                var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == subject.Trim() && x.TemplateCode == "DB_TABLES" && x.ParentNoteId == parentId);
                if (validateNote.Any())
                {
                    return Json(new { success = false, error = "table is already Exist" });
                }

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = DataActionEnum.Create;
                templateModel.TemplateCode = "DB_TABLES";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = subject.Trim();
                newmodel.ParentNoteId = parentId;
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false, error = "table is required" });
        }
        private async Task<List<T>> ConvertToObject<T>(NpgsqlDataReader rd) where T : class, new()
        {
            System.Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var list = new List<T>();
            while (await rd.ReadAsync())
            {
                var t = new T();

                for (int i = 0; i < rd.FieldCount; i++)
                {
                    if (!rd.IsDBNull(i))
                    {
                        string fieldName = rd.GetName(i);
                        var member = members.FirstOrDefault(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase));
                        if (member != null)
                        {
                            try
                            {
                                var mt = member.Type.FullName;

                                if (mt.Contains("System.Int32"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = int.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.Double"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = double.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.DateTime"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = Convert.ToDateTime(val);
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.EndsWith("Enum"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = int.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.Int16"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = short.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.Boolean"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = bool.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.StartsWith("System.Nullable`1[[Synergy.App.Common"))
                                {
                                    var splits = member.Type.FullName.Split(',');
                                    if (splits.Length > 0)
                                    {
                                        var typText = $"{splits[0].Replace("System.Nullable`1[[", "")}, Synergy.App.Common";
                                        var val = rd.GetValue(i);
                                        if (val != null)
                                        {
                                            var typ = System.Type.GetType(typText);
                                            var enumVal = Enum.Parse(typ, Convert.ToString(val));
                                            accessor[t, member.Name] = enumVal;
                                        }

                                    }

                                }
                                else if (mt.StartsWith("System.Nullable`1[[") && mt.Contains("System.Int64"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = long.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.StartsWith("System.Nullable`1[[") && mt.Contains("System.Int32"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = int.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else
                                {
                                    var temp = rd.GetValue(i);
                                    if (temp != null)
                                    {
                                        accessor[t, member.Name] = rd.GetValue(i);
                                    }

                                }


                            }
                            catch (Exception ex)
                            {

                                throw;
                            }

                        }
                    }
                }
                list.Add(t);
            }



            return list;
        }
        public async Task<JsonResult> ReadDbTablesData(string parentId)
        {
            var data = await _noteBusiness.GetList(x => x.TemplateCode == "DB_TABLES" && x.ParentNoteId == parentId);
            return Json(data);
        }
        public async Task ImprotDataFromExcelForCamera1()
        {
            var list = new List<CameraDetailsViewModel>();
            var fileName = "C:\\Dewas Camera detail MP-1.xlsx";
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        var i = 0;
                        while (reader.Read()) //Each row of the file
                        {
                            if (i == 0)
                            {
                                i++;
                                continue;

                            }
                            if (reader.GetValue(0) == null)
                            {
                                break;
                            }
                            list.Add(new CameraDetailsViewModel
                            {
                                SrNo = reader.GetValue(0).ToString(),
                                CameraName = reader.GetValue(1).ToString(),
                                LocationName = reader.GetValue(2).ToString(),
                                PoliceStation = reader.GetValue(3).ToString(),
                                Longitude = reader.GetValue(4).ToString(),
                                Latitude = reader.GetValue(5).ToString(),
                                IpAddress = reader.GetValue(6).ToString(),
                                RtspLink = reader.GetValue(7).ToString(),
                                TypeOfCamera = reader.GetValue(8).ToString(),
                                Make = reader.GetValue(9).ToString(),
                            });
                            i++;
                        }
                    }
                }
                var settings = new ConnectionSettings(new Uri("http://178.238.236.213:9200/"));
                var client = new ElasticClient(settings);
                var bulkIndexResponse = client.Bulk(b => b
                                        .Index("test_camera1")//dial100_test
                                        .IndexMany(list)
                                    );

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task ImprotDataFromExcelForSampleData()
        {
            var list = new List<Dial100OldViewmodel>();
            var fileName = "C:\\sampleData.xlsx";
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        var i = 0;
                        while (reader.Read()) //Each row of the file
                        {
                            if (i == 0)
                            {
                                i++;
                                continue;

                            }
                            if (reader.GetValue(0) == null)
                            {
                                break;
                            }
                            list.Add(new Dial100OldViewmodel
                            {
                                //SrNo = reader.GetValue(0).ToString(),
                                act_check = reader.GetValue(0).ToString(),
                                ag_id = reader.GetValue(1).ToString(),
                                agency_event_rev_num = reader.GetValue(2).ToString(),
                                carid = reader.GetValue(3).ToString(),
                                cdts = reader.GetValue(4).ToString(),
                                cdts2 = reader.GetValue(5).ToString(),
                                cpers = reader.GetValue(6).ToString(),
                                crew_id = reader.GetValue(7).ToString(),
                                csec = reader.GetValue(8).ToString(),
                                cterm = reader.GetValue(9).ToString(),
                                dgroup = reader.GetValue(10).ToString(),
                                disp_alarm_lev = reader.GetValue(11).ToString(),
                                disp_num = reader.GetValue(12).ToString(),
                                eid = reader.GetValue(13).ToString(),
                                lastxor = reader.GetValue(14).ToString(),
                                lastyor = reader.GetValue(15).ToString(),
                                latitude = reader.GetValue(16).ToString(),
                                location = reader.GetValue(17).ToString(),
                                longitude = reader.GetValue(18).ToString(),
                                mdthostname = reader.GetValue(19).ToString(),
                                mileage = reader.GetValue(20).ToString(),
                                num_1 = reader.GetValue(21).ToString(),
                                oag_id = reader.GetValue(22).ToString(),
                                odgroup = reader.GetValue(23).ToString(),
                                page_id = reader.GetValue(24).ToString(),
                                recovery_cdts = reader.GetValue(25).ToString(),
                                station = reader.GetValue(26).ToString(),
                                sub_tycod = reader.GetValue(27).ToString(),
                                track_personnel = reader.GetValue(28).ToString(),
                                tycod = reader.GetValue(29).ToString(),
                                ucust1 = reader.GetValue(30).ToString(),
                                ucust2 = reader.GetValue(31).ToString(),
                                ucust3 = reader.GetValue(32).ToString(),
                                ucust4 = reader.GetValue(33).ToString(),
                                uhiscm = reader.GetValue(34).ToString(),
                                un_hi_child_change_id = reader.GetValue(35).ToString(),
                                unique_id = reader.GetValue(36).ToString(),
                                unit_status = reader.GetValue(37).ToString(),
                                unityp = reader.GetValue(38).ToString(),
                            });
                            i++;
                        }
                    }
                }
                var settings = new ConnectionSettings(new Uri("http://178.238.236.213:9200/"));
                var client = new ElasticClient(settings);
                var bulkIndexResponse = client.Bulk(b => b
                                        .Index("dial_test")
                                        .IndexMany(list)
                                    );

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task ImprotDataFromExcelForCamera2()
        {
            var list = new List<CameraDetails2ViewModel>();
            var fileName = "C:\\Chhindwara Camera detail MP-2.xlsx";
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        var i = 0;
                        while (reader.Read()) //Each row of the file
                        {
                            if (i == 0)
                            {
                                i++;
                                continue;

                            }
                            if (reader.GetValue(0) == null)
                            {
                                break;
                            }
                            list.Add(new CameraDetails2ViewModel
                            {
                                SrNo = reader.GetValue(0).ToString(),
                                City = reader.GetValue(1).ToString(),
                                Location = reader.GetValue(2).ToString(),
                                PoliceStation = reader.GetValue(3).ToString(),
                                Latitude = reader.GetValue(4).ToString(),
                                Longitude = reader.GetValue(5).ToString(),
                                SwitchHostName = reader.GetValue(6).ToString(),
                                IpAddress = reader.GetValue(7).ToString(),
                                RtspLink = reader.GetValue(8).ToString(),
                            });
                            i++;
                        }
                    }
                }
                var settings = new ConnectionSettings(new Uri("http://178.238.236.213:9200/"));
                var client = new ElasticClient(settings);
                var bulkIndexResponse = client.Bulk(b => b
                                        .Index("test_camera2")
                                        .IndexMany(list)
                                    );

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IActionResult> ReadDial100Data(/*[DataSourceRequest] DataSourceRequest request,*/ string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<Dial100OldViewmodel>();
            var url = "http://178.238.236.213:9200/dial_test/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var data1 = data.SelectToken("hits");
                        if (data1.IsNotNull())
                        {
                            var data2 = data1.SelectToken("hits");
                            foreach (var item in data2)
                            {
                                var source = item.SelectToken("_source");
                                var str = JsonConvert.SerializeObject(source);
                                var result = JsonConvert.DeserializeObject<Dial100OldViewmodel>(str);
                                model.Add(result);
                            }
                        }
                    }



                }
            }
            else
            {
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery3;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery4;
                }
                content = content.Replace("#SEARCHWHERE#", searchStr);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var data1 = data.SelectToken("hits");
                        if (data1.IsNotNull())
                        {
                            var total = data1.SelectToken("total");
                            var value = total.SelectToken("value");
                            var data2 = data1.SelectToken("hits");
                            foreach (var item in data2)
                            {
                                var source = item.SelectToken("_source");
                                var highlight = item.SelectToken("highlight");
                                var str = JsonConvert.SerializeObject(source);
                                var strhighlight = JsonConvert.SerializeObject(highlight);
                                var result = JsonConvert.DeserializeObject<Dial100OldViewmodel>(str);
                                var resultHighlight = JsonConvert.DeserializeObject<Dial100OldArrayViewmodel>(strhighlight);

                                if (result.IsNotNull())
                                {

                                    var sres = new Dial100OldViewmodel
                                    {
                                        unityp = resultHighlight.unityp != null ? string.Join("", resultHighlight.unityp) : string.Join("", result.unityp),
                                        unit_status = resultHighlight.unit_status != null ? string.Join("", resultHighlight.unit_status) : string.Join("", result.unit_status),
                                        unique_id = resultHighlight.unique_id != null ? string.Join("", resultHighlight.unique_id) : string.Join("", result.unique_id),
                                        track_personnel = resultHighlight.track_personnel != null ? string.Join("", resultHighlight.track_personnel) : string.Join("", result.track_personnel),
                                        tycod = resultHighlight.tycod != null ? string.Join("", resultHighlight.tycod) : string.Join("", result.tycod),
                                        sub_tycod = resultHighlight.sub_tycod != null ? string.Join("", resultHighlight.sub_tycod) : string.Join("", result.sub_tycod),
                                        station = resultHighlight.station != null ? string.Join("", resultHighlight.station) : string.Join("", result.station),
                                        latitude = resultHighlight.latitude != null ? string.Join("", resultHighlight.latitude) : string.Join("", result.latitude),
                                        location = resultHighlight.location != null ? string.Join("", resultHighlight.location) : string.Join("", result.location),
                                        longitude = resultHighlight.longitude != null ? string.Join("", resultHighlight.longitude) : string.Join("", result.longitude),
                                        eid = resultHighlight.eid != null ? string.Join("", resultHighlight.eid) : string.Join("", result.eid),
                                        dgroup = resultHighlight.dgroup != null ? string.Join("", resultHighlight.dgroup) : string.Join("", result.dgroup),
                                        ag_id = resultHighlight.ag_id != null ? string.Join("", resultHighlight.ag_id) : string.Join("", result.ag_id),
                                    };

                                    //sres = new Dial100Viewmodel {
                                    //    unityp = result.unityp != null ? string.Join("", result.unityp) : "",
                                    //    unit_status = result.unit_status != null ? string.Join("", result.unit_status) : "",
                                    //    unique_id = result.unique_id != null ? string.Join("", result.unique_id) : "",
                                    //    track_personnel = result.track_personnel != null ? string.Join("", result.track_personnel) : "",
                                    //    tycod = result.tycod != null ? string.Join("", result.tycod) : "",
                                    //    sub_tycod = result.sub_tycod != null ? string.Join("", result.sub_tycod) : "",
                                    //    station = result.station != null ? string.Join("", result.station) : "",
                                    //    latitude = result.latitude != null ? string.Join("", result.latitude) : "",
                                    //    location = result.location != null ? string.Join("", result.location) : "",
                                    //    longitude = result.longitude != null ? string.Join("", result.longitude) : "",
                                    //    eid = result.eid != null ? string.Join("", result.eid) : "",
                                    //    dgroup = result.dgroup != null ? string.Join("", result.dgroup) : "",
                                    //    ag_id = result.ag_id != null ? string.Join("", result.ag_id) : "",
                                    //  };
                                    model.Add(sres);
                                }

                            }
                        }
                    }




                }
            }
            return Json(model);
        }
        public async Task<IActionResult> ReadDial100DataByIds(string ids)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var list = new List<Dial100ViewModel>();
            if (ids.IsNotNullAndNotEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadDial100DataByIdsQuery;
                var idsArray = ids.Split(",");
                var filIds = "[\"" + String.Join("\",\"", idsArray) + "\"]";
                content = content.Replace("#IDS#", filIds);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var url = eldbUrl + "dail100/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var data1 = data.SelectToken("hits");
                        if (data1.IsNotNull())
                        {
                            var data2 = data1.SelectToken("hits");
                            foreach (var item in data2)
                            {
                                var source = item.SelectToken("_source");
                                var str = JsonConvert.SerializeObject(source);
                                var result = JsonConvert.DeserializeObject<Dial100ViewModel>(str);
                                list.Add(result);
                            }
                        }
                    }
                }
            }
            return Json(list);
        }
        public async Task<IActionResult> ReadCCTVCamera1(/*[DataSourceRequest] DataSourceRequest request,*/ string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<CameraDetailsViewModel>();
            var url = "http://178.238.236.213:9200/test_camera1/_search";


            var dates = GetStartEndDateByFilterType(dateFilterType, startDate, endDate);


            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera1DataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var data1 = data.SelectToken("hits");
                        if (data1.IsNotNull())
                        {
                            var data2 = data1.SelectToken("hits");
                            foreach (var item in data2)
                            {
                                var source = item.SelectToken("_source");
                                var str = JsonConvert.SerializeObject(source);
                                var result = JsonConvert.DeserializeObject<CameraDetailsViewModel>(str);
                                model.Add(result);
                            }
                        }

                    }


                }
            }
            else
            {
                var content = "";
                if (plainSearch)
                {
                    if (dateFilterType != SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera1DataQuery2;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera1DataQuery3;
                    }
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera1DataQuery4;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera1DataQuery5;
                }
                content = content.Replace("#SEARCHWHERE#", searchStr);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var data1 = data.SelectToken("hits");
                        if (data1.IsNotNull())
                        {
                            var total = data1.SelectToken("total");
                            var value = total.SelectToken("value");
                            var data2 = data1.SelectToken("hits");
                            foreach (var item in data2)
                            {
                                var source = item.SelectToken("_source");
                                var highlight = item.SelectToken("highlight");
                                var str = JsonConvert.SerializeObject(source);
                                var strhighlight = JsonConvert.SerializeObject(highlight);
                                var result = JsonConvert.DeserializeObject<CameraDetailsViewModel>(str);
                                var resultHighlight = JsonConvert.DeserializeObject<CameraDetailsArrayViewModel>(strhighlight);

                                if (result.IsNotNull())
                                {

                                    var sres = new CameraDetailsViewModel
                                    {
                                        SrNo = resultHighlight.SrNo != null ? string.Join("", resultHighlight.SrNo) : string.Join("", result.SrNo),
                                        CameraName = resultHighlight.CameraName != null ? string.Join("", resultHighlight.CameraName) : string.Join("", result.CameraName),
                                        LocationName = resultHighlight.LocationName != null ? string.Join("", resultHighlight.LocationName) : string.Join("", result.LocationName),
                                        PoliceStation = resultHighlight.PoliceStation != null ? string.Join("", resultHighlight.PoliceStation) : string.Join("", result.PoliceStation),
                                        Longitude = resultHighlight.Longitude != null ? string.Join("", resultHighlight.Longitude) : string.Join("", result.Longitude),
                                        Latitude = resultHighlight.Latitude != null ? string.Join("", resultHighlight.Latitude) : string.Join("", result.Latitude),
                                        IpAddress = resultHighlight.IpAddress != null ? string.Join("", resultHighlight.IpAddress) : string.Join("", result.IpAddress),
                                        RtspLink = resultHighlight.RtspLink != null ? string.Join("", resultHighlight.RtspLink) : string.Join("", result.RtspLink),
                                        TypeOfCamera = resultHighlight.TypeOfCamera != null ? string.Join("", resultHighlight.TypeOfCamera) : string.Join("", result.TypeOfCamera),
                                        Make = resultHighlight.Make != null ? string.Join("", resultHighlight.Make) : string.Join("", result.Make),
                                    };

                                    model.Add(sres);
                                }

                            }
                        }
                    }




                }
            }
            return Json(model/*.ToDataSourceResult(request)*/);
        }

        private DateFilter GetStartEndDateByFilterType(SocialMediaDatefilters dateFilterType, string startDate, string endDate)
        {
            if (dateFilterType == SocialMediaDatefilters.AllTime)
            {
                return new DateFilter();
            }
            else if (dateFilterType == SocialMediaDatefilters.Custom)
            {
                return new DateFilter
                {
                    StartDate = startDate.ToSafeDateTime(),
                    EndDate = endDate.ToSafeDateTime(),
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last30Days)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-30),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last7Days)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.LastMonth)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddMonths(-1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.LastWeek)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.LastYear)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddYears(-1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.ThisMonth)
            {
                return new DateFilter
                {
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.ThisWeek)
            {
                int i = DateTime.Today.DayOfWeek - DayOfWeek.Monday;
                if (i == -1) i = 6;
                TimeSpan ts = new TimeSpan(i, 0, 0, 0);
                var dt = DateTime.Today.Subtract(ts);
                return new DateFilter
                {
                    StartDate = dt,
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.ThisYear)
            {
                return new DateFilter
                {
                    StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Today)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1).AddTicks(-1),
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Yesterday)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today.AddTicks(-1),
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last15Mins)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Today.AddMinutes(-15),
                    EndDate = DateTime.Today,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last5Mins)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Now.AddMinutes(-5),
                    EndDate = DateTime.Now,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last30Mins)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Now.AddMinutes(-30),
                    EndDate = DateTime.Now,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last1Hour)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Now.AddMinutes(-60),
                    EndDate = DateTime.Now,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last4Hour)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Now.AddHours(-4),
                    EndDate = DateTime.Now,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last8Hour)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Now.AddHours(-8),
                    EndDate = DateTime.Now,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last12Hour)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Now.AddHours(-12),
                    EndDate = DateTime.Now,
                };
            }
            else if (dateFilterType == SocialMediaDatefilters.Last24Hour)
            {
                return new DateFilter
                {
                    StartDate = DateTime.Now.AddHours(-24),
                    EndDate = DateTime.Now,
                };
            }
            else return new DateFilter();

        }

        public async Task<IActionResult> ReadCCTVCamera2(/*[DataSourceRequest] DataSourceRequest request, */string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<CameraDetailsViewModel>();
            var url = "http://178.238.236.213:9200/test_camera2/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera2DataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var data1 = data.SelectToken("hits");
                        if (data1.IsNotNull())
                        {
                            var data2 = data1.SelectToken("hits");
                            foreach (var item in data2)
                            {
                                var source = item.SelectToken("_source");
                                var str = JsonConvert.SerializeObject(source);
                                var result = JsonConvert.DeserializeObject<CameraDetailsViewModel>(str);
                                model.Add(result);
                            }
                        }
                    }



                }
            }
            else
            {
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera2DataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera2DataQuery3;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCCTVCamera2DataQuery4;
                }
                content = content.Replace("#SEARCHWHERE#", searchStr);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var data1 = data.SelectToken("hits");
                        if (data1.IsNotNull())
                        {
                            var total = data1.SelectToken("total");
                            var value = total.SelectToken("value");
                            var data2 = data1.SelectToken("hits");
                            foreach (var item in data2)
                            {
                                var source = item.SelectToken("_source");
                                var highlight = item.SelectToken("highlight");
                                var str = JsonConvert.SerializeObject(source);
                                var strhighlight = JsonConvert.SerializeObject(highlight);
                                var result = JsonConvert.DeserializeObject<CameraDetailsViewModel>(str);
                                var resultHighlight = JsonConvert.DeserializeObject<CameraDetailsArrayViewModel>(strhighlight);

                                if (result.IsNotNull())
                                {

                                    var sres = new CameraDetailsViewModel
                                    {
                                        SrNo = resultHighlight.SrNo != null ? string.Join("", resultHighlight.SrNo) : string.Join("", result.SrNo),
                                        CameraName = resultHighlight.CameraName != null ? string.Join("", resultHighlight.CameraName) : string.Join("", result.CameraName),
                                        LocationName = resultHighlight.LocationName != null ? string.Join("", resultHighlight.LocationName) : string.Join("", result.LocationName),
                                        PoliceStation = resultHighlight.PoliceStation != null ? string.Join("", resultHighlight.PoliceStation) : string.Join("", result.PoliceStation),
                                        Longitude = resultHighlight.Longitude != null ? string.Join("", resultHighlight.Longitude) : string.Join("", result.Longitude),
                                        Latitude = resultHighlight.Latitude != null ? string.Join("", resultHighlight.Latitude) : string.Join("", result.Latitude),
                                        IpAddress = resultHighlight.IpAddress != null ? string.Join("", resultHighlight.IpAddress) : string.Join("", result.IpAddress),
                                        RtspLink = resultHighlight.RtspLink != null ? string.Join("", resultHighlight.RtspLink) : string.Join("", result.RtspLink),
                                        TypeOfCamera = resultHighlight.TypeOfCamera != null ? string.Join("", resultHighlight.TypeOfCamera) : string.Join("", result.TypeOfCamera),
                                        Make = resultHighlight.Make != null ? string.Join("", resultHighlight.Make) : string.Join("", result.Make),
                                    };

                                    model.Add(sres);
                                }

                            }
                        }
                    }




                }
            }
            return Json(model/*.ToDataSourceResult(request)*/);
        }



        public async Task<List<CameraDetailsViewModel>> ReadCamera1Data()
        {
            var model = new List<CameraDetailsViewModel>();
            var url = "http://178.238.236.213:9200/test_camera1/_search";

            var content = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var json = await response.Content.ReadAsStringAsync();


                var data = JToken.Parse(json);
                var data1 = data.SelectToken("hits");
                var data2 = data1.SelectToken("hits");
                foreach (var item in data2)
                {
                    var source = item.SelectToken("_source");
                    var str = JsonConvert.SerializeObject(source);
                    var result = JsonConvert.DeserializeObject<CameraDetailsViewModel>(str);
                    model.Add(result);
                }

            }
            return model;

        }
        public async Task<List<CameraDetailsViewModel>> ReadCamera2Data()
        {
            var model = new List<CameraDetailsViewModel>();
            var url = "http://178.238.236.213:9200/test_camera2/_search";

            var content = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var json = await response.Content.ReadAsStringAsync();


                var data = JToken.Parse(json);
                var data1 = data.SelectToken("hits");
                var data2 = data1.SelectToken("hits");
                foreach (var item in data2)
                {
                    var source = item.SelectToken("_source");
                    var str = JsonConvert.SerializeObject(source);
                    var result = JsonConvert.DeserializeObject<CameraDetailsViewModel>(str);
                    result.LocationName = result.LocationName == null ? result.PoliceStation : result.LocationName;
                    result.CameraName = result.CameraName == null ? "NA" : result.CameraName;
                    model.Add(result);
                }

            }
            return model;

        }
        //public async Task<IActionResult> CCTVMonitor()
        //{
        //    var data = await ReadCamera1Data();
        //    var groupData = data.GroupBy(x => new { x.Latitude, x.Longitude });
        //    var markers1 = new List<MapsMarker>();
        //    foreach (var gdata in groupData)
        //    {
        //        var ldata = gdata.ToList();
        //        var sdata = "";
        //        foreach (var l in ldata)
        //        {
        //            sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
        //        }

        //        //var jsonObj = JsonConvert.SerializeObject(gdata);
        //        var marker = new MapsMarker
        //        {
        //            Visible = true,
        //            DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
        //            Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
        //            TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //        };
        //        markers1.Add(marker);
        //    }
        //    ViewBag.MarkersCamera1 = markers1.ToList();
        //    var data1 = await ReadCamera2Data();

        //    var groupData2 = data1.GroupBy(x => new { x.Latitude, x.Longitude });
        //    var markers2 = new List<MapsMarker>();
        //    foreach (var gdata in groupData2)
        //    {
        //        var ldata = gdata.ToList();
        //        var sdata = "";
        //        foreach (var l in ldata)
        //        {
        //            sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
        //        }

        //        //var jsonObj = JsonConvert.SerializeObject(gdata);
        //        var marker = new MapsMarker
        //        {
        //            Visible = true,
        //            DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
        //            Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
        //            TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //        };
        //        markers2.Add(marker);
        //    }
        //    ViewBag.MarkersCamera2 = markers2.ToList();
        //    return View();
        //}

        //public async Task<IActionResult> CCTVMonitorPartial(string stationName, string loc)
        //{
        //    var data = await ReadCamera1Data();
        //    var groupData = data.GroupBy(x => new { x.Latitude, x.Longitude });
        //    var markers1 = new List<MapsMarker>();
        //    foreach (var gdata in groupData)
        //    {
        //        var ldata = gdata.ToList();
        //        var sdata = "";
        //        foreach (var l in ldata)
        //        {
        //            sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
        //        }

        //        //var jsonObj = JsonConvert.SerializeObject(gdata);
        //        var marker = new MapsMarker
        //        {
        //            Visible = true,
        //            DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
        //            Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
        //            TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //        };
        //        markers1.Add(marker);
        //    }
        //    ViewBag.MarkersCamera1 = markers1.ToList();
        //    var groupData2 = data.GroupBy(x => new { x.Latitude, x.Longitude });
        //    var markers2 = new List<MapsMarker>();
        //    foreach (var gdata in groupData2)
        //    {
        //        var ldata = gdata.ToList();
        //        var sdata = "";
        //        foreach (var l in ldata)
        //        {
        //            sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
        //        }

        //        //var jsonObj = JsonConvert.SerializeObject(gdata);
        //        var marker = new MapsMarker
        //        {
        //            Visible = true,
        //            DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
        //            Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
        //            TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //        };
        //        markers2.Add(marker);
        //    }
        //    ViewBag.MarkersCamera2 = markers2.ToList();
        //    return PartialView("CCTVMonitorPartial");
        //}


        public async Task<IActionResult> CCTVMonitorView(string data)
        {
            var splitData = data.Split("|");
            foreach (var i in splitData)
            {

            }
            return View();
        }

        public async Task<IActionResult> GetCameraListByCity(string cityName)
        {
            var data = new List<CameraDetailsViewModel>();
            var lList = new List<IdNameViewModel>();

            if (cityName == "1")
            {
                data = await ReadCamera1Data();
            }
            else if (cityName == "2")
            {
                data = await ReadCamera2Data();
            }
            else
            {
                return Json(data);
            }

            var groupData = data.GroupBy(x => new { x.Latitude, x.Longitude });
            foreach (var gdata in groupData)
            {
                var ldata = gdata.ToList();
                var l = new IdNameViewModel
                {
                    Id = ldata[0].LocationName ?? ldata[0].PoliceStation,
                    Name = ldata[0].LocationName ?? ldata[0].PoliceStation,
                };
                lList.Add(l);
            }
            return Json(lList);
        }

        public async Task<IActionResult> SynergyFundingDashboard()
        {

            return View();
        }

        //public async Task<IActionResult> SynergyFundingMapPartial()
        //{
        //    var markers = new List<MapsMarker>();

        //    var marker = new MapsMarker
        //    {
        //        Visible = true,
        //        DataSource = new[] { new { name = "New York", latitude = 40.7128, longitude = 74.0060 } },
        //        TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //    };
        //    markers.Add(marker);
        //    var marker2 = new MapsMarker
        //    {
        //        Visible = true,
        //        DataSource = new[] { new { name = "Jersey City", latitude = 40.7178, longitude = 74.0431 } },
        //        TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //    };
        //    markers.Add(marker2);

        //    var marker3 = new MapsMarker
        //    {
        //        Visible = true,
        //        DataSource = new[] { new { name = "London", latitude = 51.5074, longitude = 0.1278 } },
        //        TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //    };
        //    markers.Add(marker3);
        //    var marker4 = new MapsMarker
        //    {
        //        Visible = true,
        //        DataSource = new[] { new { name = "Toronto", latitude = 43.6532, longitude = 79.3832 } },
        //        TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //    };
        //    markers.Add(marker4);
        //    ViewBag.Markers = markers.ToList();
        //    return View();
        //}
        public async Task<IActionResult> WidgetDashboardIndex()
        {
            return View();
        }
        public async Task<IActionResult> WidgetDashboard()
        {
            var parent = await _noteBusiness.GetSingle(x => x.TemplateCode == "DashboardMaster" && x.NoteSubject == "Widget Dashboard");
            var list = await _noteBusiness.GetDashboardItemMasterList(parent.IsNotNull() ? parent.Id : null);
            var widgetList = await _noteBusiness.GetWidgetItemList(parent.IsNotNull() ? parent.Id : null);

            foreach (var item in widgetList)
            {
                ViewData[item.NoteSubject] = await GetSocialMediaChartData(item.keyword, item.socialMediaType);
                var widgetItem = await _noteBusiness.GetWidgetItemDetailsByName(item.NoteSubject);
                var chartInput = widgetItem.chartMetadata;
                var chartBPCode = widgetItem.boilerplateCode;
                chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
                chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + widgetItem.NoteSubject.Replace(" ", string.Empty) + "'");
                chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
                item.chartMetadata = chartBPCode;
                item.ChartKey = item.NoteSubject.Replace(" ", "");
            }
            foreach (var item in list)
            {
                var dashboardItem = await _noteBusiness.GetDashboardItemDetailsByName(item.NoteSubject);
                var chartInput = dashboardItem.chartMetadata;
                var chartBPCode = dashboardItem.boilerplateCode;
                chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
                chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
                chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
                item.chartMetadata = chartBPCode;
                item.ChartKey = item.NoteSubject.Replace(" ", "");
            }
            ViewBag.ParentId = parent.IsNotNull() ? parent.Id : null;
            var model = new Tuple<List<DashboardItemMasterViewModel>, List<WidgetItemViewModel>>(list, widgetList);
            return View(model);
        }
        public async Task<IActionResult> WidgetItem(string id, string parentId, DataActionEnum dataAction)
        {
            var model = new WidgetItemViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "WIDGET_ITEM";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.ParentNoteId = parentId;
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.from = System.DateTime.Now;
                model.to = System.DateTime.Now;

            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetWidgetItemDetails(id);
                if (udf.IsNotNull())
                {
                    model.keyword = udf.keyword;
                    model.height = udf.height;
                    model.width = udf.width;
                    model.from = udf.from;
                    model.to = udf.to;
                    model.chartMetadata = udf.chartMetadata;
                    model.chartTypeId = udf.chartTypeId;
                    if (udf.socialMediaType.IsNotNullAndNotEmpty())
                    {
                        model.socialMediaTypeArray = udf.socialMediaType.Split(',');
                    }
                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManagWidgetItemNote(WidgetItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    //var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject);
                    //if (validateNote.Any())
                    //{
                    //    return Json(new { success = false, error = "Name is already Exist" });
                    //}
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "WIDGET_ITEM";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                else
                {
                    //var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject);
                    //if (validateNote.Any())
                    //{
                    //    return Json(new { success = false, error = "Name is already Exist" });
                    //}
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<List<SocialMediaChartViewModel>> GetSocialMediaChartData(string searchStr, string mediaType)
        {
            var model = new List<SocialMediaChartViewModel>();

            if (mediaType.IsNotNullAndNotEmpty())
            {
                var type = mediaType.Split(',');
                for (int i = 0; i < type.Length; i++)
                {
                    if (type[i] == "Facebook")
                    {
                        var content = ApplicationConstant.BusinessAnalytics.GetSocialMediaChartDataQuery1;

                        content = content.Replace("#SEARCHWHERE#", searchStr);
                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {

                            var url = "http://178.238.236.213:9200/facebook1/_search";
                            var address = new Uri(url);
                            var response = await httpClient.PostAsync(address, stringContent);
                            var json = await response.Content.ReadAsStringAsync();


                            var data = JToken.Parse(json);
                            var data1 = data.SelectToken("hits");
                            var total = data1.SelectToken("total");
                            var value = total.SelectToken("value");
                            var abc = new SocialMediaChartViewModel
                            {
                                MediaType = "Facebook",
                                Count = value.ToString()
                            };
                            model.Add(abc);
                        }
                    }
                    if (type[i] == "Twitter")
                    {
                        var content1 = ApplicationConstant.BusinessAnalytics.GetSocialMediaChartDataQuery2;

                        content1 = content1.Replace("#SEARCHWHERE#", searchStr);
                        var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                        using (var httpClient1 = new HttpClient())
                        {

                            var url = "http://178.238.236.213:9200/twitter2/_search";
                            var address = new Uri(url);
                            var response1 = await httpClient1.PostAsync(address, stringContent1);
                            var json1 = await response1.Content.ReadAsStringAsync();


                            var twitterdata = JToken.Parse(json1);
                            var twitterdata1 = twitterdata.SelectToken("hits");
                            var twittertotal = twitterdata1.SelectToken("total");
                            var value1 = twittertotal.SelectToken("value");

                            var twitter = new SocialMediaChartViewModel
                            {
                                MediaType = "Twitter",
                                Count = value1.ToString()
                            };
                            model.Add(twitter);
                        }
                    }
                    if (type[i] == "Youtube")
                    {
                        var content2 = ApplicationConstant.BusinessAnalytics.GetSocialMediaChartDataQuery3;

                        content2 = content2.Replace("#SEARCHWHERE#", searchStr);
                        var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                        using (var httpClient2 = new HttpClient())
                        {
                            var url = "http://178.238.236.213:9200/youtube1/_search";
                            var address = new Uri(url);
                            var response = await httpClient2.PostAsync(address, stringContent2);
                            var json = await response.Content.ReadAsStringAsync();


                            var data = JToken.Parse(json);
                            var data1 = data.SelectToken("hits");
                            var total = data1.SelectToken("total");
                            var value = total.SelectToken("value");
                            var youtube = new SocialMediaChartViewModel
                            {
                                MediaType = "Youtube",
                                Count = value.ToString()
                            };
                            model.Add(youtube);
                        }
                    }
                    if (type[i] == "WhatsApp")
                    {
                        var content3 = ApplicationConstant.BusinessAnalytics.GetSocialMediaChartDataQuery4;

                        content3 = content3.Replace("#SEARCHWHERE#", searchStr);
                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
                        using (var httpClient2 = new HttpClient())
                        {
                            var url = "http://178.238.236.213:9200/whatsapp1/_search";
                            var address = new Uri(url);
                            var response = await httpClient2.PostAsync(address, stringContent3);
                            var json = await response.Content.ReadAsStringAsync();


                            var data = JToken.Parse(json);
                            var data1 = data.SelectToken("hits");
                            var total = data1.SelectToken("total");
                            var value = total.SelectToken("value");
                            var youtube = new SocialMediaChartViewModel
                            {
                                MediaType = "WhatsApp",
                                Count = value.ToString()
                            };
                            model.Add(youtube);
                        }
                    }
                }
            }
            return model;

        }
        public async Task<IActionResult> SocialMediaReports()
        {
            var parent = await _noteBusiness.GetSingle(x => x.TemplateCode == "DashboardMaster" && x.NoteSubject == "Report Grids");
            ViewBag.ParentId = parent.IsNotNull() ? parent.Id : null;
            return View();
        }
        public async Task<IActionResult> ReportItem(string id, string parentId, DataActionEnum dataAction, string layout)
        {
            var model = new DashboardItemMasterViewModel();
            await GetMeasureDimensionsData(model);
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DashboardItem";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.ParentNoteId = parentId;
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.Layout = layout;

            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetDashboardItemMasterDetails(id);
                if (udf.IsNotNull())
                {
                    model.chartTypeId = udf.chartTypeId;
                    model.chartMetadata = udf.chartMetadata;
                    model.height = udf.height;
                    model.width = udf.width;
                    if (udf.measuresField.IsNotNullAndNotEmpty())
                    {
                        model.measuresArray = udf.measuresField.Split(',');
                    }
                    if (udf.dimensionsField.IsNotNullAndNotEmpty())
                    {
                        model.dimensionsArray = udf.dimensionsField.Split(',');
                    }
                    if (udf.segmentsField.IsNotNullAndNotEmpty())
                    {
                        model.segmentsArray = udf.segmentsField.Split(',');
                    }
                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.Layout = layout;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageReportItemNote(DashboardItemMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "DashboardItem");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DashboardItem";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "DashboardItem");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<JsonResult> ReadReportItemListData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var parent = await _noteBusiness.GetSingle(x => x.TemplateCode == "DashboardMaster" && x.NoteSubject == "Report Grids");
            var data = await _noteBusiness.GetList(x => x.TemplateCode == "DashboardItem" && x.ParentNoteId == parent.Id);
            //var dsResult = data.ToDataSourceResult(request);
            var dsResult = data;
            return Json(dsResult);
        }
        public async Task<IActionResult> GenerateSocialMediaReports(string id, string subject)
        {
            var dashboardItem = await _noteBusiness.GetDashboardItemDetailsById(id);
            var chartInput = dashboardItem.chartMetadata;
            var chartBPCode = dashboardItem.boilerplateCode;
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput.Replace("\"\"", "\""));
            chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "'");
            chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
            dashboardItem.chartMetadata = chartBPCode;
            dashboardItem.ChartKey = dashboardItem.NoteSubject.Replace(" ", "");
            return View(dashboardItem);
        }
        public async Task<IActionResult> RssFeed()
        {
            return View();
        }
        public async Task<JsonResult> ReadRssFeedData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var data = await _noteBusiness.GetRssFeedData();
            var dsResult = data/*.ToDataSourceResult(request)*/;
            return Json(dsResult);
        }
        public async Task<IActionResult> ManageRssFeed(string id, DataActionEnum dataAction)
        {
            var model = new RssFeedViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "RSS_FEED_MASTER";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetRssFeedDetails(id);
                if (udf.IsNotNull())
                {
                    model.feedName = udf.feedName;
                    model.feedUrl = udf.feedUrl;


                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteDescription = newmodel.NoteDescription;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageRssFeed(RssFeedViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "RSS_FEED_MASTER");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "RSS_FEED_MASTER";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, id = result.Item.Id });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "RSS_FEED_MASTER");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<IActionResult> DeleteRssFeed(string id)
        {
            await _noteBusiness.Delete(id);
            return Json(new { success = true });
        }
        public async Task<IActionResult> ReadTimesOfIndiaNewsFeed(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<NewsFeedsViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "rssfeeds/_search?pretty=true";

            var dateRange = GetStartEndDateByFilterType(dateFilterType, startDate, endDate);

            if (searchStr.IsNullOrEmpty())
            {
                var content = "";

                if (dateFilterType == SocialMediaDatefilters.AllTime)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery1;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery2;
                    content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                    content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                }
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    if (data1.IsNotNull())
                    {
                        var data2 = data1.SelectToken("hits");
                        foreach (var item in data2)
                        {
                            var source = item.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<NewsFeedsViewModel>(str);
                            model.Add(result);
                        }
                    }


                }
            }
            else
            {
                var content = "";
                if (plainSearch)
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery3;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery4;
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                    }
                }
                else if (!isAdvance)
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery5;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery6;
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    }
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery7;
                }
                content = content.Replace("#SEARCHWHERE#", searchStr);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    //var total = data1.SelectToken("total");
                    //var value = total.SelectToken("value");
                    if (data1.IsNotNull())
                    {
                        var data2 = data1.SelectToken("hits");
                        foreach (var item in data2)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var strhighlight = JsonConvert.SerializeObject(highlight);
                            var result = JsonConvert.DeserializeObject<NewsFeedsViewModel>(str);
                            var resultHighlight = JsonConvert.DeserializeObject<NewsFeedsArrayViewModel>(strhighlight);

                            if (result.IsNotNull())
                            {

                                var sres = new NewsFeedsViewModel
                                {
                                    _index = result._index,
                                    name = result.name,
                                    author = result.author,
                                    message = (resultHighlight.IsNotNull() && resultHighlight.message != null) ? string.Join("", resultHighlight.message) : result.message,
                                    title = (resultHighlight.IsNotNull() && resultHighlight.title != null) ? string.Join("", resultHighlight.title) : result.title,
                                    link = (resultHighlight.IsNotNull() && resultHighlight.link != null) ? string.Join("", resultHighlight.link) : result.link,
                                    published = result.published,
                                };

                                model.Add(sres);
                            }

                        }
                    }



                }
            }
            return Json(model/*.ToDataSourceResult(request)*/);
        }

        public async Task<IActionResult> GetAllCameraLocations()
        {
            var data = await ReadCamera1Data();
            data.AddRange(await ReadCamera2Data());
            var res = data.Select(x => new IdNameViewModel { Id = x.LocationName, Name = x.LocationName }).ToList();
            return Json(res.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList());
        }
        public async Task<IActionResult> GetAllLocations()
        {
            var data = await _noteBusiness.GetList(x => x.TemplateCode == "SM_LOCATION");
            var res = data.Select(x => new IdNameViewModel { Id = x.NoteSubject, Name = x.NoteSubject }).ToList();
            return Json(res.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList());
        }

        //public async Task<IActionResult> LocationDashboard()
        //{
        //    //await UpdateLocationCountUsingHangfire();
        //    var dync = new List<dynamic>();
        //    //Code for track Pai Chart
        //    var tracks = await _noteBusiness.GetList(x => x.TemplateCode == "TRACK_MASTER");
        //    var trackModel = new List<TrackChartViewModel>();

        //    foreach (var track in tracks)
        //    {
        //        var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
        //        if (keywords.Count > 0)
        //        {
        //            var searchStr = string.Join(" ", keywords);
        //            var content = ApplicationConstant.BusinessAnalytics.LocationDashboardDataQuery1;
        //            content = content.Replace("#SEARCHWHERE#", searchStr);
        //            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
        //            using (var httpClient = new HttpClient())
        //            {
        //                var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,whatsapp1,insta,twitter2,dial_test,test_camera1,test_camera2/_search?pretty=true";
        //                var address = new Uri(url1);
        //                var response = await httpClient.PostAsync(address, stringContent);
        //                var jsontrack = await response.Content.ReadAsStringAsync();


        //                var trackdata = JToken.Parse(jsontrack);
        //                var trackdata1 = trackdata.SelectToken("hits");
        //                var total = trackdata1.SelectToken("total");
        //                var value = total.SelectToken("value");
        //                trackModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Count = value.ToString() });
        //                if (trackdata1.IsNotNull())
        //                {
        //                    var hits = trackdata1.SelectToken("hits");
        //                    foreach (var hitsItem in hits)
        //                    {
        //                        var source = hitsItem.SelectToken("_source");
        //                        var souraceJson = JsonConvert.SerializeObject(source);
        //                        var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
        //                        //var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
        //                        dync.Add(result);

        //                    }
        //                }
        //            }
        //        }
        //    }

        //    var url = "..\\CMS.UI.Web\\wwwroot\\js\\CMS\\mapJson\\indiamap.json";
        //    string usmap = System.IO.File.ReadAllText(url);
        //    ViewBag.usmap = JsonConvert.DeserializeObject(usmap);
        //    var markers1 = new List<MapsMarker>();
        //    var data = await _noteBusiness.GetDistictByState("fe94e3c0-d311-4a89-a96a-67e249881d09");
        //    foreach (var d in data)
        //    {
        //        int count = Convert.ToInt32(d.Count);
        //        var stations = await _noteBusiness.GetPoliceStationByDistict(d.Id);
        //        foreach (var station in stations)
        //        {
        //            count += Convert.ToInt32(station.Count);
        //            var locations = await _noteBusiness.GetLocationByPoliceStation(station.Id);
        //            foreach (var location in locations)
        //            {
        //                count += Convert.ToInt32(location.Count);
        //            }
        //        }

        //        //var sub = d.Name;
        //        //var i = 0;
        //        //try
        //        //{
        //        //    foreach (var obj in dync)
        //        //    {
        //        //        var souraceJson = JsonConvert.SerializeObject(obj);                       
        //        //        if (souraceJson.ToLower().Contains(sub.ToLower()))
        //        //        {
        //        //            i++;
        //        //        }

        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{


        //        //}

        //        var marker = new MapsMarker
        //        {
        //            Visible = true,
        //            DataSource = new[] { new { name = d.Name, latitude = d.Latitude, longitude = d.Longitude } },
        //            Template = "<div class='marker-event' onclick=onMarkerClick('" + d.Id + "','" + d.Name + "')><div class='pin bounce'><span class='pin-label'>" + d.Name + " " + count + "</span></div><div class='pulse'></div></div>",
        //            TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //        };
        //        markers1.Add(marker);
        //    }
        //    if (data.IsNotNull())
        //    {
        //        ViewBag.StateName = data[0].State;
        //    }
        //    ViewBag.District = markers1;

        //    //Code for keyword Pai Chart
        //    foreach (var track in tracks)
        //    {
        //        var keywordModel = new List<TrackChartViewModel>();
        //        var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
        //        var query = "";
        //        foreach (var keyword in keywords)
        //        {

        //            var content = ApplicationConstant.BusinessAnalytics.LocationDashboardDataQuery2;
        //            content = content.Replace("#SEARCHWHERE#", keyword);
        //            query = query + content;
        //        }
        //        if (query.IsNotNullAndNotEmpty())
        //        {
        //            var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
        //            using (var httpClient = new HttpClient())
        //            {
        //                var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,insta,whatsapp1,twitter2,dial_test,test_camera1,test_camera2/_msearch?pretty=true";
        //                var address = new Uri(url1);
        //                var response = await httpClient.PostAsync(address, stringContent);
        //                var jsontrack = await response.Content.ReadAsStringAsync();


        //                var trackdata = JToken.Parse(jsontrack);
        //                var responsedata = trackdata.SelectToken("responses");
        //                var i = 0;
        //                foreach (var responseitem in responsedata)
        //                {
        //                    var hits = responseitem.SelectToken("hits");
        //                    var total = hits.SelectToken("total");
        //                    var value = total.SelectToken("value");
        //                    keywordModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Keyword = keywords[i], Count = value.ToString() });
        //                    i++;
        //                }
        //            }
        //            ViewData[track.NoteSubject] = keywordModel;
        //        }

        //    }
        //    ViewBag.dataSource = trackModel;

        //    return View();
        //}

        public async Task<IActionResult> PoliceStationMap(string locationId, string districtName)
        {
            //await UpdateLocationCountUsingHangfire();
            var url = "..\\CMS.UI.Web\\wwwroot\\js\\CMS\\mapJson\\indiamap.json";
            string usmap = System.IO.File.ReadAllText(url);
            ViewBag.usmap = JsonConvert.DeserializeObject(usmap);
            var markers1 = new List<MarkerData>();
            var data = await _noteBusiness.GetMarkersByDistrict(locationId);
            foreach (var d in data)
            {
                int count = Convert.ToInt32(d.Count);
                var locations = await _noteBusiness.GetLocationByPoliceStation(d.Id);
                foreach (var location in locations)
                {
                    count += Convert.ToInt32(location.Count);
                }

                //var marker = new MapsMarker
                //{
                //    Visible = true,
                //    DataSource = new[] { new { name = d.Name, latitude = d.Latitude, longitude = d.Longitude } },
                //    //Template = "<div class='marker-event' onclick=onMarkerClick('" + d.Id + "')><div class='pin bounce'><span class='pin-label'>" + d.Name + "</span></div><div class='pulse'></div></div>",
                //    Template = "<div onclick=onMarkerClick('" + d.Id + "')><span class='fa fa-map-marker' title='"+ d.Name+"'></span></div>",
                //    TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
                //};
                markers1.Add(new MarkerData { name = d.Name, latitude = Convert.ToDouble(d.Latitude), longitude = Convert.ToDouble(d.Longitude), count = count });
            }

            //Code for track Pai Chart
            var tracks = await _noteBusiness.GetList(x => x.TemplateCode == "TRACK_MASTER");
            var trackModel = new List<TrackChartViewModel>();

            foreach (var track in tracks)
            {
                var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
                if (keywords.Count > 0)
                {
                    var dync = new List<dynamic>();
                    var searchStr = string.Join(" ", keywords);
                    var content = ApplicationConstant.BusinessAnalytics.PoliceStationMapDataQuery1;
                    content = content.Replace("#SEARCHWHERE#", searchStr);
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,whatsapp1,insta,twitter2,dial_test,test_camera1,test_camera2/_search?pretty=true";
                        var address = new Uri(url1);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var jsontrack = await response.Content.ReadAsStringAsync();


                        var trackdata = JToken.Parse(jsontrack);
                        var trackdata1 = trackdata.SelectToken("hits");
                        var total = trackdata1.SelectToken("total");
                        var value = total.SelectToken("value");
                        if (trackdata1.IsNotNull())
                        {
                            var hits = trackdata1.SelectToken("hits");
                            foreach (var hitsItem in hits)
                            {
                                var source = hitsItem.SelectToken("_source");
                                var souraceJson = JsonConvert.SerializeObject(source);
                                var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
                                //var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
                                dync.Add(result);

                            }
                        }
                    }
                    var policeData = await _noteBusiness.GetMarkersByDistrict(locationId);
                    var i = 0;
                    foreach (var d in policeData)
                    {
                        var sub = d.Name;
                        try
                        {
                            foreach (var obj in dync)
                            {
                                var souraceJson = JsonConvert.SerializeObject(obj);
                                if (souraceJson.ToLower().Contains(sub.ToLower()))
                                {
                                    i++;
                                }

                            }
                            var locations = await _noteBusiness.GetLocationByPoliceStation(d.Id);
                            foreach (var location in locations)
                            {
                                foreach (var obj1 in dync)
                                {
                                    var souraceJson = JsonConvert.SerializeObject(obj1);
                                    if (souraceJson.ToLower().Contains(location.Name.ToLower()))
                                    {
                                        i++;
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                    trackModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Count = i.ToString() });
                }
            }
            if (data.Count > 0)
            {
                ViewBag.StateName = data[0].State;
                ViewBag.DistrictName = data[0].District;
            }
            else
            {
                ViewBag.StateName = "Madhya Pradesh";
                ViewBag.DistrictName = districtName;
            }
            ViewBag.District = markers1;
            ViewBag.dataSource = trackModel;
            return View();
        }
        public async Task<bool> UpdateLocationCountUsingHangfire()
        {
            try
            {

                //var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var dync = new List<dynamic>();
                //Code for track Pai Chart
                var tracks = await _noteBusiness.GetList(x => x.TemplateCode == "TRACK_MASTER");
                var trackModel = new List<TrackChartViewModel>();

                foreach (var track in tracks)
                {
                    var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
                    if (keywords.Count > 0)
                    {
                        var searchStr = string.Join(" ", keywords);
                        var content = ApplicationConstant.BusinessAnalytics.UpdateLocationCountUsingHangfireDataQuery1;
                        content = content.Replace("#SEARCHWHERE#", searchStr);
                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,whatsapp1,insta,twitter2,dial_test,test_camera1,test_camera2/_search?pretty=true";
                            var address = new Uri(url1);
                            var response = await httpClient.PostAsync(address, stringContent);
                            var jsontrack = await response.Content.ReadAsStringAsync();


                            var trackdata = JToken.Parse(jsontrack);
                            var trackdata1 = trackdata.SelectToken("hits");
                            var total = trackdata1.SelectToken("total");
                            var value = total.SelectToken("value");
                            trackModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Count = value.ToString() });
                            if (trackdata1.IsNotNull())
                            {
                                var hits = trackdata1.SelectToken("hits");
                                foreach (var hitsItem in hits)
                                {
                                    var source = hitsItem.SelectToken("_source");
                                    var souraceJson = JsonConvert.SerializeObject(source);
                                    var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
                                    //var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
                                    dync.Add(result);

                                }
                            }
                        }
                    }
                }
                var locationData = await _noteBusiness.GetList(x => x.TemplateCode == "SM_LOCATION");
                foreach (var d in locationData)
                {
                    var sub = d.NoteSubject;
                    var i = 0;
                    try
                    {
                        foreach (var obj in dync)
                        {
                            var souraceJson = JsonConvert.SerializeObject(obj);
                            if (souraceJson.ToLower().Contains(sub.ToLower()))
                            {
                                i++;
                            }

                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    await _noteBusiness.UpdateLocation(d.Id, i.ToString());
                }
                var policeData = await _noteBusiness.GetList(x => x.TemplateCode == "SM_POLICE_STATION");
                foreach (var d in policeData)
                {
                    var sub = d.NoteSubject;
                    var i = 0;
                    try
                    {
                        foreach (var obj in dync)
                        {
                            var souraceJson = JsonConvert.SerializeObject(obj);
                            if (souraceJson.ToLower().Contains(sub.ToLower()))
                            {
                                i++;
                            }

                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    await _noteBusiness.UpdatePoliceStation(d.Id, i.ToString());
                }
                var districtData = await _noteBusiness.GetList(x => x.TemplateCode == "SM_DISTRICT");
                foreach (var d in districtData)
                {
                    var sub = d.NoteSubject;
                    var i = 0;
                    try
                    {
                        foreach (var obj in dync)
                        {
                            var souraceJson = JsonConvert.SerializeObject(obj);
                            if (souraceJson.ToLower().Contains(sub.ToLower()))
                            {
                                i++;
                            }

                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    await _noteBusiness.UpdateDistrict(d.Id, i.ToString());
                }
                return true;


            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<IActionResult> Monitoring()
        {
            return View();
        }
        public async Task<IActionResult> ApiConnection()
        {
            return View();
        }
        public async Task<JsonResult> ReadApiConnectionData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var data = await _noteBusiness.GetApiConnectionData();
            var dsResult = data/*.ToDataSourceResult(request)*/;
            return Json(dsResult);
        }
        public async Task<IActionResult> ManageApiConnection(string id, DataActionEnum dataAction)
        {
            var model = new ApiConnectionViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "API_CONNECTION";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetApiConnectionDetails(id);
                if (udf.IsNotNull())
                {
                    model.restApiUrl = udf.restApiUrl;
                    model.parameters = udf.parameters;
                    model.userName = udf.userName;
                    model.password = udf.password;
                    model.apiKey = udf.apiKey;
                    model.pollingTime = udf.pollingTime;

                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageApiConnection(ApiConnectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "API_CONNECTION");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "API_CONNECTION";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, id = result.Item.Id });
                    }
                }
                else
                {
                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "API_CONNECTION");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

        public async Task<IActionResult> DeleteApiConnection(string id)
        {
            await _noteBusiness.Delete(id);
            return Json(new { success = true });
        }

        public async Task<JsonResult> ReadTrackWithKeywords(string id)
        {
            var data = new List<TagCategoryViewModel>();

            var trakList = await _noteBusiness.GetTrackList();
            foreach (var category in trakList)
            {
                TagCategoryViewModel model = new TagCategoryViewModel();
                model.Id = category.Id;
                model.Name = category.Name;
                var keywordList = await _noteBusiness.GetKeywordList(model.Id);
                model.Tags = new List<TagCategoryViewModel>();
                foreach (var tag in keywordList)
                {
                    TagCategoryViewModel tagmodel = new TagCategoryViewModel();
                    tagmodel.Id = tag.Id;
                    tagmodel.Name = tag.Name;
                    tagmodel.ParentNoteId = category.Id;
                    tagmodel.HasChildren = false;
                    data.Add(tagmodel);
                }
                model.HasChildren = keywordList.Count() > 0 ? true : false;
                if (keywordList.Count() > 0)
                {
                    data.Add(model);
                }
            }


            var result = data.Where(x => id.IsNotNullAndNotEmpty() ? x.ParentNoteId == id : x.ParentNoteId == null)
           .Select(item => new
           {
               id = item.Id,
               Name = item.Name,
               ParentId = item.ParentNoteId,
               hasChildren = item.HasChildren
           });
            return Json(result);
        }

        //public async Task<bool> RssFeedFileGenerateUsingHangfire()
        //{
        //    try
        //    {

        //        var EsdbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
        //        var path = ApplicationConstant.AppSettings.LogstashConfigPath(_configuration);
        //        //var _business = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
        //        var feeds = await _noteBusiness.GetRssFeedDataForScheduling();
        //        var syncdata = await _noteBusiness.GetScheduleSyncData();
        //        if (!syncdata.trackingDate.IsNotNull() || syncdata.trackingDate < feeds.First().LastUpdatedDate)
        //        {

        //            //string fileName = @"D:\Filess\RSS_FEED.conf";                                        
        //            string fileName = path + syncdata.scheduleTemplate + ".conf";
        //            try
        //            {
        //                // Check if file already exists. If yes, delete it.     
        //                if (!System.IO.File.Exists(fileName))
        //                {
        //                    //File.Delete(fileName);
        //                    // Create a new file 
        //                    using (System.IO.File.Create(fileName)) { }
        //                    var inputContent = "";
        //                    var filterContent = "";
        //                    foreach (var feed in feeds)
        //                    {
        //                        inputContent += "rss { url => '" + feed.feedUrl + "'      interval => 120     tags => '" + feed.feedName + "'   }";
        //                        filterContent += "if '" + feed.feedName + "' in [tags] {         mutate {           replace => { 'name' => '" + feed.feedName + "' }         }     }";
        //                    }
        //                    inputContent += "  //EOI";
        //                    filterContent += "  //EOF";
        //                    var fileContent = "input { //EOI } filter {  //EOF } output {     elasticsearch {         hosts => ['" + EsdbUrl + "'] 		index => 'rssfeeds'     } }";
        //                    fileContent = fileContent.Replace("//EOI", inputContent);
        //                    fileContent = fileContent.Replace("//EOF", filterContent);
        //                    StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(fileName));

        //                    writer.Write(fileContent);

        //                    writer.Close();
        //                    var strcontent = fileContent.Replace("'", "\"");
        //                    await _noteBusiness.UpdateScheduleSyncData(syncdata.Id, System.DateTime.Now, strcontent);
        //                }
        //                else
        //                {
        //                    var inputContent = "";
        //                    var filterContent = "";
        //                    foreach (var feed in feeds.Where(x => x.LastUpdatedDate > syncdata.trackingDate))
        //                    {
        //                        inputContent += "rss { url => '" + feed.feedUrl + "'      interval => 120     tags => '" + feed.feedName + "'   }";
        //                        filterContent += "if '" + feed.feedName + "' in [tags] {         mutate {           replace => { 'name' => '" + feed.feedName + "' }         }     }";
        //                    }
        //                    string fileContent = "";
        //                    using (StreamReader reader = new StreamReader(System.IO.File.OpenRead(fileName)))
        //                    {
        //                        fileContent = reader.ReadToEnd();
        //                        reader.Close();
        //                    }
        //                    if (inputContent.IsNotNullAndNotEmpty())
        //                    {
        //                        inputContent += "  //EOI";
        //                        fileContent = fileContent.Replace("//EOI", inputContent);
        //                    }
        //                    if (filterContent.IsNotNullAndNotEmpty())
        //                    {
        //                        filterContent += "  //EOF";
        //                        fileContent = fileContent.Replace("//EOF", filterContent);
        //                    }
        //                    StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(fileName));

        //                    writer.Write(fileContent);

        //                    writer.Close();
        //                    var strcontent = fileContent.Replace("'", "\"");
        //                    await _noteBusiness.UpdateScheduleSyncData(syncdata.Id, System.DateTime.Now, strcontent);
        //                }
        //            }
        //            catch (Exception Ex)
        //            {

        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //}
        public async Task<ActionResult> Logstash()
        {
            //var EsdbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var path = ApplicationConstant.AppSettings.LogstashConfigPath(_configuration);
            //var syncdata = await _noteBusiness.GetScheduleSyncData();
            //string fileName = @"D:\Filess\RSS_FEED.conf";                                        
            string fileName = path;
            // Check if file already exists. If yes, delete it.     
            if (!System.IO.File.Exists(fileName))
            {
                //File.Delete(fileName);
                // Create a new file 
                //using (System.IO.File.Create(fileName)) { }
                //var inputContent = "";
                //var filterContent = "";
                //foreach (var feed in feeds)
                //{
                //    inputContent += "rss { url => '" + feed.feedUrl + "'      interval => 120     tags => '" + feed.feedName + "'   }";
                //    filterContent += "if '" + feed.feedName + "' in [tags] {         mutate {           replace => { 'name' => '" + feed.feedName + "' }         }     }";
                //}
                //inputContent += "  //EOI";
                //filterContent += "  //EOF";
                //var fileContent = "input { //EOI } filter {  //EOF } output {     elasticsearch {         hosts => ['" + EsdbUrl + "'] 		index => 'rssfeeds'     } }";
                //fileContent = fileContent.Replace("//EOI", inputContent);
                //fileContent = fileContent.Replace("//EOF", filterContent);
                //StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(fileName));

                //writer.Write(fileContent);

                //writer.Close();
                //var strcontent = fileContent.Replace("'", "\"");
                //await _noteBusiness.UpdateScheduleSyncData(syncdata.Id, System.DateTime.Now, strcontent);
            }
            else
            {
                using (StreamReader reader = new StreamReader(System.IO.File.OpenRead(fileName)))
                {
                    ViewBag.FileConetent = reader.ReadToEnd();
                    reader.Close();
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ManageLogstash(SchedulerSyncViewModel model)
        {
            var content = model.logstashContent.HtmlDecode();
            //var EsdbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var path = ApplicationConstant.AppSettings.LogstashConfigPath(_configuration);
            //var syncdata = await _noteBusiness.GetScheduleSyncData();
            //string fileName = @"D:\Filess\RSS_FEED.conf";                                        
            //string fileName = path + syncdata.First().scheduleTemplate + ".conf";
            string fileName = path;
            // Check if file already exists. If yes, delete it.     
            if (!System.IO.File.Exists(fileName))
            {
                using (System.IO.File.Create(fileName)) { }
                StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(fileName));
                writer.Write(content);
                writer.Close();
            }
            else
            {
                StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(fileName));
                writer.Write(content);
                writer.Close();
            }
            return Json(new { success = true });
        }
        public async Task<IActionResult> Home()
        {
            //var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            //List<Dial100ViewModel> list = new List<Dial100ViewModel>();
            //var query = ApplicationConstant.BusinessAnalytics.ReadDial100EventQuery;
            //if (query.IsNotNullAndNotEmpty())
            //{
            //    var startDate = DateTime.Now.AddDays(-90);
            //    var endDate = DateTime.Now;
            //    query = query.Replace("#FILTERCOLUMN#", "event_time");
            //    query = query.Replace("#STARTDATE#", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            //    query = query.Replace("#ENDDATE#", endDate.ToString("yyyy-MM-dd HH:mm:ss"));
            //    var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
            //    var handler = new HttpClientHandler();
            //    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //    handler.ServerCertificateCustomValidationCallback =
            //        (httpRequestMessage, cert, cetChain, policyErrors) =>
            //        {
            //            return true;
            //        };
            //    using (var httpClient = new HttpClient(handler))
            //    {
            //        var url = eldbUrl + "dail100/_search?pretty=true";
            //        var address = new Uri(url);
            //        var response = await httpClient.PostAsync(address, stringContent);
            //        var jsontrack = await response.Content.ReadAsStringAsync();
            //        var trackdata = JToken.Parse(jsontrack);
            //        if (trackdata.IsNotNull())
            //        {
            //            var hits = trackdata.SelectToken("hits");
            //            if (hits.IsNotNull())
            //            {
            //                var total = hits.SelectToken("total");
            //                var value = total.SelectToken("value");
            //                var _hits = hits.SelectToken("hits");
            //                foreach (var hit in _hits)
            //                {
            //                    var source = hit.SelectToken("_source");                               
            //                    var str = JsonConvert.SerializeObject(source);                                
            //                    var result = JsonConvert.DeserializeObject<Dial100ViewModel>(str);                                
            //                    if (result.IsNotNull())
            //                    {
            //                        list.Add(result);
            //                    }

            //                }
            //            }
            //        }

            //    }
            //}
            //if (list.Count > 0)
            //{
            //    var _total= list.Count;
            //    var _open = list.Where(x => x.closing_Time.IsNullOrEmpty() || x.closing_Time == "N/A" || x.closing_Time == "<Not Available>").ToList().Count;
            //    var _close = list.Where(x => x.closing_Time.IsNotNullAndNotEmpty() && x.closing_Time != "N/A" && x.closing_Time != "<Not Available>").ToList().Count;                
            //    var openPer = (100 * _open) / _total;
            //    var closePer = (100 * _close) / _total;
            //    ViewBag.Total = _total;
            //    ViewBag.Open = _open;
            //    ViewBag.Close = _close;
            //    ViewBag.OpenPerc = openPer;
            //    ViewBag.ClosePerc = closePer;
            //}
            return View();
        }
        public async Task<IActionResult> IipAlertList(string colorCode)
        {
            ViewBag.Color = colorCode;
            return View();
        }
        public async Task<IActionResult> NotificationAlertIndex()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> ReadNotificationAlertList()
        {
            var data = await _noteBusiness.GetList(x => x.TemplateCode == "NOTIFICATION_ALERT");
            return Json(data);
        }
        public async Task<IActionResult> NotificationAlert(string id, DataActionEnum dataAction)
        {
            var model = new AlertViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "NOTIFICATION_ALERT";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.query = "[]";
                model.groupFilters = "[]";
                model.evaluateTime = "*/5 * * * *";
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                var udf = await _noteBusiness.GetNotificationALertDetails(id);
                if (udf.IsNotNull())
                {
                    model.queryTableId = udf.queryTableId;
                    model.query = udf.query;
                    model.groupFilters = udf.groupFilters;
                    model.conditionFunction = udf.conditionFunction;
                    model.conditionType = udf.conditionType;
                    model.conditionValue = udf.conditionValue;
                    model.evaluateTime = udf.evaluateTime;
                    model.summary = udf.summary;
                    model.colorCode = udf.colorCode;
                    model.cubeJsFilter = udf.cubeJsFilter;
                    model.columnReferenceId = udf.columnReferenceId;
                    model.timeDimensionId = udf.timeDimensionId;
                    model.timeDimensionDuration = udf.timeDimensionDuration;
                    model.timeDimensionFilter = udf.timeDimensionFilter;
                    model.granularity = udf.granularity;
                    model.chartTypeId = udf.chartTypeId;
                    model.userRole = udf.userRole;
                    model.isReporting = udf.isReporting;
                    model.frequency = udf.frequency;
                    model.limit = udf.limit;
                    if (udf.queryColumns.IsNotNullAndNotEmpty())
                    {
                        //model.queryColumns = udf.queryColumns.Replace(",", "','");
                        model.queryColumns = udf.queryColumns;
                    }
                    if (udf.groupbyColumns.IsNotNullAndNotEmpty())
                    {
                        model.groupbyColumns = udf.groupbyColumns.Replace(",", "','");

                    }
                }
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteDescription = newmodel.NoteDescription;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            model.PortalId = _userContext.PortalId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageNotificationAlert(AlertViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "NOTIFICATION_ALERT");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var qr = JsonConvert.DeserializeObject<dynamic>(model.query);
                    var con = qr.condition;
                    var op = con.Value;
                    var rules = qr.rules;
                    var cubejsquery = "{";
                    cubejsquery = await ReadJson(rules, op, cubejsquery);
                    cubejsquery += "}";
                    JObject json = JObject.Parse(cubejsquery);
                    string jsonStr = JsonConvert.SerializeObject(json);
                    string jsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                    model.cubeJsFilter = jsonFormatted;
                    if (model.timeDimensionId.IsNotNullAndNotEmpty())
                    {
                        if (model.timeDimensionDuration == "AllTime")
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeDimensionId);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionFilter = timejsonFormatted;
                        }
                        else
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeDimensionId);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            timeQuery = timeQuery.Replace("#DURATION#", model.timeDimensionDuration);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionFilter = timejsonFormatted;
                        }

                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "NOTIFICATION_ALERT";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        model.Id = result.Item.NoteId;
                        await _noteBusiness.TriggerHangfire(model);
                        return Json(new { success = true, id = result.Item.Id });

                    }
                }
                else
                {

                    var validateNote = await _noteBusiness.GetList(x => x.Id != model.NoteId && x.NoteSubject == model.NoteSubject && x.TemplateCode == "NOTIFICATION_ALERT");
                    if (validateNote.Any())
                    {
                        return Json(new { success = false, error = "Name is already Exist" });
                    }
                    var qr = JsonConvert.DeserializeObject<dynamic>(model.query);
                    var con = qr.condition;
                    var op = con.Value;
                    var rules = qr.rules;
                    var cubejsquery = "{";
                    cubejsquery = await ReadJson(rules, op, cubejsquery);
                    cubejsquery += "}";
                    JObject json = JObject.Parse(cubejsquery);
                    string jsonStr = JsonConvert.SerializeObject(json);
                    string jsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                    model.cubeJsFilter = jsonFormatted;
                    if (model.timeDimensionId.IsNotNullAndNotEmpty())
                    {
                        if (model.timeDimensionDuration == "AllTime")
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeDimensionId);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionFilter = timejsonFormatted;
                        }
                        else
                        {
                            var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";
                            timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeDimensionId);
                            timeQuery = timeQuery.Replace("#GRANULARITY#", model.granularity);
                            timeQuery = timeQuery.Replace("#DURATION#", model.timeDimensionDuration);
                            JObject timejson = JObject.Parse(timeQuery);
                            string timejsonStr = JsonConvert.SerializeObject(timejson);
                            string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                            model.timeDimensionFilter = timejsonFormatted;
                        }

                    }
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        await _noteBusiness.TriggerHangfire(model);
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<IActionResult> DeleteAlertRuleNote(string id)
        {
            var model = await _noteBusiness.GetSingleById(id);
            await _noteBusiness.Delete(id);
            Hangfire.RecurringJob.RemoveIfExists("AlertJobs-" + model.NoteSubject);
            return Json(new { success = true });
        }
        private async Task<string> ReadJson(dynamic rules, string condtion, string cubejsquery)
        {
            cubejsquery += condtion.ToLower() + ": [";
            foreach (var rule in rules)
            {
                var con = rule.condition;
                if (con != null)
                {
                    var op = con.Value;
                    cubejsquery += "{";
                    cubejsquery = await ReadJson(rule.rules, op, cubejsquery);
                    cubejsquery += "},";

                }
                else
                {
                    var f = rule["field"].Value;
                    var o = rule["operator"].Value;
                    var v = rule["value"].Value;
                    if (o == "set" || o == "notSet")
                    {
                        cubejsquery += "{ member: '" + f + "',operator:'" + o + "',},";

                    }
                    else
                    {
                        if (v != null)
                        {
                            string st = Convert.ToString(v);
                            st = st.Replace(",", "','");
                            cubejsquery += "{ member: '" + f + "',operator:'" + o + "',values: ['" + st + "'],},";

                        }

                    }
                }
            }
            cubejsquery += "],";
            return cubejsquery;
        }
        private async Task<string> ReadFilterJson(dynamic rules, string condtion, string cubejsquery, List<DashboardItemFilterViewModel> list)
        {
            cubejsquery += condtion.ToLower() + ": [";
            foreach (var rule in rules)
            {
                var con = rule.condition;
                if (con != null)
                {
                    var op = con.Value;
                    cubejsquery += "{";
                    cubejsquery = await ReadFilterJson(rule.rules, op, cubejsquery, list);
                    cubejsquery += "},";

                }
                else
                {
                    var f = rule["field"].Value;
                    var o = rule["operator"].Value;
                    var v = rule["value"];
                    var v1 = v[0];
                    var para = v1.Value;
                    var v2 = v[1];
                    var def = v2.Value;
                    var model = new DashboardItemFilterViewModel
                    {
                        FilterField = f,
                        FilterOperator = o,
                        FilterText = para,
                        DefaultValue = def
                    };
                    list.Add(model);
                    cubejsquery += "{ member: '" + f + "',operator:'" + o + "',values: ['" + para + "'],},";
                }
            }
            cubejsquery += "],";
            return cubejsquery;
        }
        public async Task<bool> CreateNotificationForSocialMediaUsingHangfire()
        {
            try
            {

                //var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var dync = new List<dynamic>();
                //Code for track Pai Chart
                var tracks = await _noteBusiness.GetList(x => x.TemplateCode == "TRACK_MASTER");
                //var trackModel = new List<TrackChartViewModel>();

                foreach (var track in tracks)
                {
                    var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
                    if (keywords.Count > 0)
                    {
                        var searchStr = string.Join(" ", keywords);
                        var content = ApplicationConstant.BusinessAnalytics.CreateNotificationForSocialMediaUsingHangfireQuery1;
                        content = content.Replace("#SEARCHWHERE#", searchStr);
                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,whatsapp1,insta,twitter2,dial_test/_search?pretty=true";
                            var address = new Uri(url1);
                            var response = await httpClient.PostAsync(address, stringContent);
                            var jsontrack = await response.Content.ReadAsStringAsync();


                            var trackdata = JToken.Parse(jsontrack);
                            var trackdata1 = trackdata.SelectToken("hits");
                            //var total = trackdata1.SelectToken("total");
                            //var value = total.SelectToken("value");
                            //trackModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Count = value.ToString() });
                            if (trackdata1.IsNotNull())
                            {
                                var hits = trackdata1.SelectToken("hits");
                                foreach (var hitsItem in hits)
                                {
                                    var _index = hitsItem.SelectToken("_index");
                                    var source = hitsItem.SelectToken("_source");
                                    var souraceJson = JsonConvert.SerializeObject(source);
                                    var highlight = hitsItem.SelectToken("highlight");
                                    var strhighlight = JsonConvert.SerializeObject(highlight);

                                    if (_index.ToString() == "facebook1")
                                    {
                                        var resultHighlight = JsonConvert.DeserializeObject<FacebookArrayViewModel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<FacebookViewModel>(souraceJson);
                                        var notification = new NotificationViewModel
                                        {
                                            Subject = "Facebook",
                                            Body = ((resultHighlight.IsNotNull() && resultHighlight.pagename.IsNotNull()) ? string.Join("", resultHighlight.pagename) : result.pagename) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.post_message.IsNotNull()) ? string.Join("", resultHighlight.post_message) : result.post_message),
                                            ToUserId = track.CreatedBy,
                                            FromUserId = track.CreatedBy,
                                            ReferenceType = ReferenceTypeEnum.SWS_Media,
                                            PortalId = _userContext.PortalId
                                        };
                                        await _notificationBusiness.Create(notification);
                                    }
                                    else if (_index.ToString() == "youtube1")
                                    {
                                        var resultHighlight = JsonConvert.DeserializeObject<YoutubeArrayViewModel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<YoutubeViewModel>(souraceJson);
                                        var notification = new NotificationViewModel
                                        {
                                            Subject = "Youtube",
                                            Body = ((resultHighlight.IsNotNull() && resultHighlight.title.IsNotNull()) ? string.Join("", resultHighlight.title) : result.title) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.description.IsNotNull()) ? string.Join("", resultHighlight.description) : result.description),
                                            ToUserId = track.CreatedBy,
                                            FromUserId = track.CreatedBy,
                                            ReferenceType = ReferenceTypeEnum.SWS_Media,
                                            PortalId = _userContext.PortalId
                                        };
                                        await _notificationBusiness.Create(notification);
                                    }
                                    else if (_index.ToString() == "whatsapp1")
                                    {
                                        var resultHighlight = JsonConvert.DeserializeObject<WhatsAppArrayViewModel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<WhatsAppViewModel>(souraceJson);
                                        var notification = new NotificationViewModel
                                        {
                                            Subject = "WhatsApp",
                                            Body = ((resultHighlight.IsNotNull() && resultHighlight.user.IsNotNull()) ? string.Join("", resultHighlight.user) : result.user) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.messages.IsNotNull()) ? string.Join("", resultHighlight.messages) : result.messages),
                                            ToUserId = track.CreatedBy,
                                            FromUserId = track.CreatedBy,
                                            ReferenceType = ReferenceTypeEnum.SWS_Media,
                                            PortalId = _userContext.PortalId
                                        };
                                        await _notificationBusiness.Create(notification);
                                    }
                                    else if (_index.ToString() == "twitter2")
                                    {
                                        var resultHighlight = JsonConvert.DeserializeObject<TwitterArrayViewModel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<TwitterViewModel>(souraceJson);
                                        var c = resultHighlight.text.Count();
                                        var notification = new NotificationViewModel
                                        {
                                            Subject = "Twitter",
                                            Body = ((resultHighlight.IsNotNull() && resultHighlight.text.IsNotNull()) ? string.Join("", resultHighlight.text) : result.text) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.hashtags.IsNotNull()) ? string.Join("", resultHighlight.hashtags) : result.hashtags) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.location.IsNotNull()) ? string.Join("", resultHighlight.location) : result.location),
                                            ToUserId = track.CreatedBy,
                                            FromUserId = track.CreatedBy,
                                            ReferenceType = ReferenceTypeEnum.SWS_Media,
                                            PortalId = _userContext.PortalId
                                        };
                                        await _notificationBusiness.Create(notification);
                                    }
                                    else if (_index.ToString() == "insta")
                                    {
                                        var resultHighlight = JsonConvert.DeserializeObject<InstagramArrayViewModel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<InstagramViewModel>(souraceJson);
                                        var notification = new NotificationViewModel
                                        {
                                            Subject = "Instagram",
                                            Body = ((resultHighlight.IsNotNull() && resultHighlight.hashtags.IsNotNull()) ? string.Join("", resultHighlight.hashtags) : result.hashtags) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.caption.IsNotNull()) ? string.Join("", resultHighlight.caption) : result.caption),
                                            ToUserId = track.CreatedBy,
                                            FromUserId = track.CreatedBy,
                                            ReferenceType = ReferenceTypeEnum.SWS_Media,
                                            PortalId = _userContext.PortalId
                                        };
                                        await _notificationBusiness.Create(notification);
                                    }
                                    else if (_index.ToString() == "rssfeeds")
                                    {
                                        var resultHighlight = JsonConvert.DeserializeObject<NewsFeedsArrayViewModel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<NewsFeedsViewModel>(souraceJson);
                                        var notification = new NotificationViewModel
                                        {
                                            Subject = "RSS Feed",
                                            Body = ((resultHighlight.IsNotNull() && resultHighlight.name.IsNotNull()) ? string.Join("", resultHighlight.name) : result.name) + "-" + ((resultHighlight.IsNotNull() && resultHighlight._index.IsNotNull()) ? string.Join("", resultHighlight._index) : result._index) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.author.IsNotNull()) ? string.Join("", resultHighlight.author) : result.author) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.message.IsNotNull()) ? string.Join("", resultHighlight.message) : result.message) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.title.IsNotNull()) ? string.Join("", resultHighlight.title) : result.title) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.published.IsNotNull()) ? string.Join("", resultHighlight.published) : result.published),
                                            ToUserId = track.CreatedBy,
                                            FromUserId = track.CreatedBy,
                                            ReferenceType = ReferenceTypeEnum.SWS_Media,
                                            PortalId = _userContext.PortalId
                                        };
                                        await _notificationBusiness.Create(notification);
                                    }
                                    else if (_index.ToString() == "dial_test")
                                    {
                                        var resultHighlight = JsonConvert.DeserializeObject<Dial100OldArrayViewmodel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<Dial100OldViewmodel>(souraceJson);
                                        var notification = new NotificationViewModel
                                        {
                                            Subject = "Dail 100",
                                            Body = ((resultHighlight.IsNotNull() && resultHighlight.unityp.IsNotNull()) ? string.Join("", resultHighlight.unityp) : result.unityp) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.unit_status.IsNotNull()) ? string.Join("", resultHighlight.unit_status) : result.unit_status) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.unique_id.IsNotNull()) ? string.Join("", resultHighlight.unique_id) : result.unique_id) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.track_personnel.IsNotNull()) ? string.Join("", resultHighlight.track_personnel) : result.track_personnel) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.tycod.IsNotNull()) ? string.Join("", resultHighlight.tycod) : result.tycod) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.sub_tycod.IsNotNull()) ? string.Join("", resultHighlight.sub_tycod) : result.sub_tycod) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.station.IsNotNull()) ? string.Join("", resultHighlight.station) : result.station) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.latitude.IsNotNull()) ? string.Join("", resultHighlight.latitude) : result.latitude) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.longitude.IsNotNull()) ? string.Join("", resultHighlight.longitude) : result.longitude) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.eid.IsNotNull()) ? string.Join("", resultHighlight.eid) : result.eid) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.dgroup.IsNotNull()) ? string.Join("", resultHighlight.dgroup) : result.dgroup) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.ag_id.IsNotNull()) ? string.Join("", resultHighlight.ag_id) : result.ag_id),
                                            ToUserId = track.CreatedBy,
                                            FromUserId = track.CreatedBy,
                                            ReferenceType = ReferenceTypeEnum.SWS_Media,
                                            PortalId = _userContext.PortalId
                                        };
                                        await _notificationBusiness.Create(notification);
                                    }
                                    //else if (_index.ToString() == "test_camera1" || _index.ToString() == "test_camera2")
                                    //{
                                    //    var resultHighlight = JsonConvert.DeserializeObject<CameraDetailsArrayViewModel>(strhighlight);
                                    //    var result = JsonConvert.DeserializeObject<CameraDetailsViewModel>(souraceJson);
                                    //    var notification = new NotificationViewModel
                                    //    {
                                    //        Subject = (_index.ToString() == "test_camera1")?"Dewas":"Chindwara",
                                    //        Body = ((resultHighlight.IsNotNull() && resultHighlight.SrNo.IsNotNull()) ? string.Join("", resultHighlight.SrNo) : result.SrNo) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.CameraName.IsNotNull()) ? string.Join("", resultHighlight.CameraName) : result.CameraName) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.LocationName.IsNotNull()) ? string.Join("", resultHighlight.LocationName) : result.LocationName) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.PoliceStation.IsNotNull()) ? string.Join("", resultHighlight.PoliceStation) : result.PoliceStation) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.Longitude.IsNotNull()) ? string.Join("", resultHighlight.Longitude) : result.Longitude) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.Latitude.IsNotNull()) ? string.Join("", resultHighlight.Latitude) : result.Latitude) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.IpAddress.IsNotNull()) ? string.Join("", resultHighlight.IpAddress) : result.IpAddress) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.RtspLink.IsNotNull()) ? string.Join("", resultHighlight.RtspLink) : result.RtspLink) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.TypeOfCamera.IsNotNull()) ? string.Join("", resultHighlight.TypeOfCamera) : result.TypeOfCamera) + "-" + ((resultHighlight.IsNotNull() && resultHighlight.Make.IsNotNull()) ? string.Join("", resultHighlight.Make) : result.Make),
                                    //        ToUserId = track.CreatedBy,
                                    //        FromUserId = track.CreatedBy,
                                    //        ReferenceType = ReferenceTypeEnum.SWS_Media,
                                    //        PortalId=_userContext.PortalId
                                    //    };
                                    //    await _notificationBusiness.Create(notification);
                                    //}                                    
                                    //var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);                                    


                                }
                                var content1 = ApplicationConstant.BusinessAnalytics.CreateNotificationForSocialMediaUsingHangfireQuery2;
                                content1 = content1.Replace("#SEARCHWHERE#", searchStr);
                                var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                                var url2 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,whatsapp1,insta,twitter2,dial_test/_update_by_query";
                                var address1 = new Uri(url2);
                                var response1 = await httpClient.PostAsync(address1, stringContent1);
                            }

                        }
                    }
                }
                return true;


            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<JsonResult> ReadNotificationData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var notifications = await _notificationBusiness.GetList(x => x.ReferenceType == ReferenceTypeEnum.SWS_Media);
            //var dsResult = notifications.OrderByDescending(x => x.CreatedDate).Take(20).ToDataSourceResult(request);
            var dsResult = notifications.OrderByDescending(x => x.CreatedDate).Take(20);
            return Json(dsResult);
        }
        public async Task<JsonResult> ShowAlert()
        {

            var dync = new List<dynamic>();
            var tracks = await _noteBusiness.GetList(x => x.TemplateCode == "TRACK_MASTER");
            var trackModel = new List<TrackChartViewModel>();
            //Code for keyword Pai Chart
            foreach (var track in tracks)
            {
                //var keywordModel = new List<TrackChartViewModel>();
                var keywords = await _noteBusiness.GetCanAlertKeywordListByTrackId(track.Id);
                var query = "";
                foreach (var keyword in keywords)
                {

                    var content = ApplicationConstant.BusinessAnalytics.ShowAlertQuery1;
                    content = content.Replace("#SEARCHWHERE#", keyword);
                    query = query + content;
                }
                if (query.IsNotNullAndNotEmpty())
                {
                    var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,insta,whatsapp1,twitter2,dial_test,test_camera1,test_camera2/_msearch?pretty=true";
                        var address = new Uri(url1);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var jsontrack = await response.Content.ReadAsStringAsync();


                        var trackdata = JToken.Parse(jsontrack);
                        var responsedata = trackdata.SelectToken("responses");
                        var i = 0;
                        foreach (var responseitem in responsedata)
                        {
                            var hits = responseitem.SelectToken("hits");
                            //var total = hits.SelectToken("total");
                            //var value = total.SelectToken("value");
                            int j = 0;
                            if (hits.IsNotNull())
                            {
                                var _hits = hits.SelectToken("hits");

                                foreach (var hit in _hits)
                                {
                                    var source = hit.SelectToken("_source");
                                    var is_alerted = source.SelectToken("is_alerted");
                                    if (is_alerted != null && is_alerted.ToObject<bool>() == false)
                                    {
                                        j++;
                                    }
                                }
                            }

                            trackModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Keyword = keywords[i], Count = j.ToString() });
                            i++;
                        }
                    }
                }

            }

            return Json(trackModel);
        }
        public async Task UpdateIsAlertedInElasticDb()
        {
            var content = @"{""query"": {""match_all"": { } },""script"": { ""source"": ""ctx._source['is_alerted'] =true""} }";
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,insta,whatsapp1,twitter2,dial_test,test_camera1,test_camera2/_update_by_query";
                var address = new Uri(url1);
                var response = await httpClient.PostAsync(address, stringContent);

            }
        }
        public async Task<IActionResult> ManageElasticDb()
        {
            var model = new TwitterViewModel { CreateDate = System.DateTime.Now };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageElasticDb(TwitterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var date = System.DateTime.Now;
                var content = ApplicationConstant.BusinessAnalytics.ManageElasticDbQuery1;
                content = content.Replace("#HASHTAGS#", model.hashtags);
                content = content.Replace("#DATE#", model.CreateDate.ToString());
                content = content.Replace("#TEXT#", model.text);
                content = content.Replace("#LOCATION#", model.location);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var url1 = "http://178.238.236.213:9200/twitter2/_doc";
                    var address = new Uri(url1);
                    var response = await httpClient.PostAsync(address, stringContent);

                }
                return Json(new { success = true });

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

        //public async Task<IActionResult> DewasCCTVCamera1()
        //{
        //    var data = await ReadCamera1Data();
        //    var groupData = data.GroupBy(x => new { x.Latitude, x.Longitude });
        //    var markers1 = new List<MapsMarker>();
        //    foreach (var gdata in groupData)
        //    {
        //        var ldata = gdata.ToList();
        //        var sdata = "";
        //        foreach (var l in ldata)
        //        {
        //            sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
        //        }
        //        var marker = new MapsMarker
        //        {
        //            Visible = true,
        //            DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
        //            Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
        //            TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
        //        };
        //        markers1.Add(marker);
        //    }
        //    ViewBag.MarkersCamera1 = markers1.Take(1).ToList();
        //    return View();
        //}
        public async Task<IActionResult> DewasCCTVCamera()
        {
            var data = await ReadCamera1Data();
            ViewBag.MarkersCamera1 = data.ToArray();
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetMeasuresData()
        {
            var data = await _noteBusiness.GetMeasures();
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetDimensionsData(string measure)
        {
            var data = await _noteBusiness.GetDimensionsByMeasue(measure);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetTimeDimensionsData(string measure)
        {
            var data = await _noteBusiness.GetDimensionsByMeasue(measure);
            data = data.Where(x => x.dataType == "DateTime").ToList();
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetDimensionsColumnData(string measure)
        {
            var data = await _noteBusiness.GetDimensionsColumnByMeasue(measure);
            foreach (var item in data)
            {
                string[] op = { "equals", "notEquals", "contains", "notContains", "set", "notSet", "inDateRange", "notInDateRange", "afterDate", "beforeDate" };
                item.operators = op;
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetDimensionsColumnDataForGroupFilter(string measure)
        {
            var data = await _noteBusiness.GetDimensionsColumnByMeasue(measure);
            foreach (var item in data)
            {
                string[] op = { "exist", "contains", "greater than", "less than" };
                item.operators = op;
            }
            return Json(data);
        }
        public async Task<IActionResult> GridStack()
        {
            return View();
        }
        public async Task<IActionResult> LoadIframePage(string src)
        {
            ViewBag.SRC = src;
            return View();
        }
        public async Task<IActionResult> GridStackItem(string id)
        {
            ViewBag.Id = id;
            ViewBag.backEnable = true;
            var model = new DashboardMasterViewModel { ParentNoteId = id, gridStack = true };
            model.layoutMetadata = "{}";
            return View(model);
        }
        public async Task<IActionResult> ManageGridStackItemTitle(string name, string id)
        {
            ViewBag.Name = name;
            ViewBag.Id = id;
            return View();
        }
        public async Task<IActionResult> onEditGridStackItem(string id, bool isEditable = false, bool canback = false)
        {
            ViewBag.Id = id;
            ViewBag.IsEditable = isEditable;
            ViewBag.backEnable = canback;
            var data = await _noteBusiness.GetAllGridStackDashboard();
            var item = data.FirstOrDefault(i => i.Id == id);
            item.ParentNoteId = id;
            item.gridStack = true;
            if (item.layoutMetadata != null && isEditable == false)
            {
                List<JObject> metadata = JsonConvert.DeserializeObject<List<JObject>>(item.layoutMetadata);
                for (int i = 0; i < metadata.Count; i++)
                {
                    metadata[i]["noMove"] = "true";
                    metadata[i]["noResize"] = "true";
                    metadata[i]["locked"] = "true";
                }
                item.layoutMetadata = Newtonsoft.Json.JsonConvert.SerializeObject(metadata);
                //item.layoutMetadata = item.layoutMetadata.Replace("dummy", "hidden");
            }

            item.layoutMetadata = item.layoutMetadata.IsNullOrEmpty() ? "{}" : item.layoutMetadata.Replace("^", "'");
            return View("GridStackItem", item);
        }

        public async Task<IActionResult> ManageGridStack(string id, DataActionEnum dataAction)
        {
            var model = new DashboardMasterViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DashboardMaster";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageGridStack(DashboardMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DashboardMaster";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    model.gridStack = true;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        //ViewBag.Success = true;
                        return Json(new { success = true, result = result });
                    }
                }
                else
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.NoteDescription = model.NoteDescription;
                    model.gridStack = true;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        //ViewBag.Success = true;
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }
        public async Task<IActionResult> GridStackPage(string id, string parentId, DataActionEnum dataAction)
        {
            var model = new DashboardMasterViewModel();
            model.ActiveUserId = _userContext.UserId;
            if (dataAction == DataActionEnum.Create)
            {

                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DashboardMaster";
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.CreatedBy = newmodel.CreatedBy;
                model.CreatedDate = System.DateTime.Now;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;
                model.ParentNoteId = parentId;
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.NoteId = id;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                model.NoteSubject = newmodel.NoteSubject;
                model.NoteId = id;
                model.ParentNoteId = parentId;
                model.CreatedBy = newmodel.CreatedBy;
                model.LastUpdatedBy = newmodel.LastUpdatedBy;
                model.LastUpdatedDate = System.DateTime.Now;
                model.CompanyId = newmodel.CompanyId;
                model.DataAction = dataAction;

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageGridStackPage(DashboardMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.TemplateCode = "DashboardMaster";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    model.gridStack = true;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, result = result.Item });
                    }
                }
                else
                {
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = model.DataAction;
                    templateModel.NoteId = model.NoteId;
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    newmodel.NoteSubject = model.NoteSubject;
                    newmodel.ParentNoteId = model.ParentNoteId;
                    model.gridStack = true;
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, result = result.Item });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }
        public async Task<JsonResult> GetGridStackPageDetails(string id)
        {
            var pagedata = await _noteBusiness.GetDashboardMasterDetails(id);
            if (pagedata != null && pagedata.layoutMetadata.IsNotNullAndNotEmpty())
            {
                var result = _webApi.AddHost(pagedata.layoutMetadata);
                var chartItems = await BuildChart(result);
                return Json(new { res = result, charts = chartItems });
            }
            return null;
        }
        [HttpGet]
        public async Task<JsonResult> ReadStackGridDashboardList()
        {
            var data = await _noteBusiness.GetAllGridStackDashboard();
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetLibraryDashboardItemList()
        {
            var data = await _noteBusiness.GetLibraryDashboardItemDetailsWithDashboard();
            return Json(data);
        }
        public async Task<IActionResult> SignPdf()
        {
            return View();
        }
        public async Task<bool> PushCCTNSApiDataToElasticDB()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
            foreach (var api in apilist)
            {

                query = query.Replace("#FILTERCOLUMN#", api.FilterColumn);
                var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var url = eldbUrl + api.NoteSubject + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, queryContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _responsedata = _dataToken.SelectToken("aggregations");
                        var _maxdateToken = _responsedata.SelectToken("max_date");
                        var _dateToken = _maxdateToken.Last();
                        var _date = _dateToken.Last();
                        var fromDate = _date.Value<DateTime>();
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now) ? DateTime.Now : api.ToDate) : ((toDate > DateTime.Now) ? DateTime.Now : toDate);
                        //var content2 = @"{""fromDate"" : ""#FROM_DATE#"" ,""toDate"" :""#To_DATE#"",""noOfRecords"":""10"" }";
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        content1 = content1.Replace("#FROM_DATE#", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        content1 = content1.Replace("#To_DATE#", batchToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                        var url1 = api.Url;
                        var address1 = new Uri(url1);
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", api.ApiAuthorization);
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = address1,
                            Content = stringContent1,
                        };
                        var response1 = await httpClient.SendAsync(request);
                        if (response1.IsSuccessStatusCode)
                        {
                            var json = await response1.Content.ReadAsStringAsync();
                            var dataToken = JToken.Parse(json);
                            var responsedata = api.Url.ToLower().Contains(api.NoteSubject) ? dataToken.SelectToken(api.ResponseToken) : dataToken.First().SelectToken(api.ResponseToken);
                            var objects = JArray.Parse(responsedata.ToString());
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (JObject root in objects)
                            {
                                dynamic obj = new System.Dynamic.ExpandoObject();
                                var id = string.Empty;
                                foreach (KeyValuePair<String, JToken> app in root)
                                {
                                    var key = app.Key;
                                    var value = app.Value;

                                    if (key == api.FilterColumn)
                                    {
                                        var a = value.Value<string>();
                                        DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                        ExpandoAddProperty(obj, key, dt);
                                    }
                                    else
                                    {
                                        var a = value.Value<string>();
                                        ExpandoAddProperty(obj, key, a);
                                    }
                                    if (key == api.IdColumn)
                                    {
                                        id = value.Value<string>();
                                    }


                                }
                                descriptor.Index<object>(i => i
                                    .Index(api.NoteSubject)
                                    .Id((Id)id)
                                    .Document(obj));
                            }
                            var bulkResponse = client.Bulk(descriptor);

                        }
                    }
                    else
                    {
                        var fromDate = api.FromDate;
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now) ? DateTime.Now : api.ToDate) : ((toDate > DateTime.Now) ? DateTime.Now : toDate);
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        content1 = content1.Replace("#FROM_DATE#", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        content1 = content1.Replace("#To_DATE#", batchToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                        var url1 = api.Url;
                        var address1 = new Uri(url1);
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", api.ApiAuthorization);
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = address1,
                            Content = stringContent1,
                        };
                        var response1 = await httpClient.SendAsync(request);
                        if (response1.IsSuccessStatusCode)
                        {
                            var json = await response1.Content.ReadAsStringAsync();
                            var dataToken = JToken.Parse(json);
                            var responsedata = api.Url.ToLower().Contains(api.NoteSubject) ? dataToken.SelectToken(api.ResponseToken) : dataToken.First().SelectToken(api.ResponseToken);
                            var objects = JArray.Parse(responsedata.ToString());
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (JObject root in objects)
                            {
                                dynamic obj = new System.Dynamic.ExpandoObject();
                                var id = string.Empty;
                                foreach (KeyValuePair<String, JToken> app in root)
                                {
                                    var key = app.Key;
                                    var value = app.Value;
                                    if (key == api.FilterColumn)
                                    {
                                        var a = value.Value<string>();
                                        DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                        ExpandoAddProperty(obj, key, dt);
                                    }
                                    else
                                    {
                                        var a = value.Value<string>();
                                        ExpandoAddProperty(obj, key, a);
                                    }
                                    if (key == api.IdColumn)
                                    {
                                        id = value.Value<string>();
                                    }

                                }
                                descriptor.Index<object>(i => i
                                    .Index(api.NoteSubject)
                                    .Id((Id)id)
                                    .Document(obj));
                            }
                            var bulkResponse = client.Bulk(descriptor);

                        }

                    }


                }

            }

            return true;
        }
        public async Task<bool> PushDial100ApiDataToElasticDB()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var content = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var url = "http://178.238.236.213:9200/dail3000/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var _jsondata = await response.Content.ReadAsStringAsync();
                    var _dataToken = JToken.Parse(_jsondata);
                    var _responsedata = _dataToken.SelectToken("aggregations");
                    var _maxdateToken = _responsedata.SelectToken("max_date");
                    var _dateToken = _maxdateToken.Last();
                    var _date = _dateToken.Last();
                    var fromDate = _date.Value<DateTime>();
                    //var fromDate = System.DateTime.Now.AddDays(-91);
                    var toDate = fromDate.AddDays(1);
                    var content1 = @"{""fromDate"" : ""#FROM_DATE#"" ,""toDate"" :""#To_DATE#"",""noOfRecords"":""10"" }";
                    content1 = content1.Replace("#FROM_DATE#", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    content1 = content1.Replace("#To_DATE#", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                    List<dynamic> list = new List<dynamic>();
                    var url1 = "http://14.99.150.248/SCRBAPI/api/SCRB/Events";
                    var address1 = new Uri(url1);
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", "U0NSQjoxMDA=");
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = address1,
                        Content = stringContent1,
                    };
                    var response1 = await httpClient.SendAsync(request);
                    if (response1.IsSuccessStatusCode)
                    {
                        var json = await response1.Content.ReadAsStringAsync();
                        var dataToken = JToken.Parse(json);
                        var responsedata = dataToken.First().SelectToken("eventInfo");
                        var objects = JArray.Parse(responsedata.ToString());
                        foreach (JObject root in objects)
                        {
                            dynamic obj = new System.Dynamic.ExpandoObject();
                            foreach (KeyValuePair<String, JToken> app in root)
                            {
                                var key = app.Key;
                                var value = app.Value;

                                if (key == "event_time")
                                {
                                    var a = value.Value<string>();
                                    DateTime dt = DateTime.ParseExact(a, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    ExpandoAddProperty(obj, key, dt);
                                }
                                else
                                {
                                    var a = value.Value<string>();
                                    ExpandoAddProperty(obj, key, a);
                                }



                            }
                            list.Add(obj);
                        }
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var doc in list)
                        {
                            descriptor.Index<object>(i => i
                                .Index("dail2000")
                                .Id((Id)doc.event_number)
                                .Document(doc));
                        }

                        var bulkResponse = client.Bulk(descriptor);

                    }
                }
                else
                {
                    //var fromDate = System.DateTime.Now.AddDays(-91);
                    //var toDate = fromDate.AddDays(30);
                    var fromDate = "2018-05-02 00:00:00";
                    var toDate = "2018-05-02 23:59:59";
                    var content1 = @"{""fromDate"" : ""#FROM_DATE#"" ,""toDate"" :""#To_DATE#"",""noOfRecords"":""10"" }";
                    content1 = content1.Replace("#FROM_DATE#", fromDate);
                    content1 = content1.Replace("#To_DATE#", toDate);
                    var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                    List<dynamic> list = new List<dynamic>();
                    var url1 = "http://14.99.150.248/SCRBAPI/api/SCRB/Events";
                    var address1 = new Uri(url1);
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", "U0NSQjoxMDA=");
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = address1,
                        Content = stringContent1,
                    };
                    var response1 = await httpClient.SendAsync(request);
                    if (response1.IsSuccessStatusCode)
                    {
                        var json = await response1.Content.ReadAsStringAsync();
                        var dataToken = JToken.Parse(json);
                        var responsedata = dataToken.First().SelectToken("eventInfo");
                        var objects = JArray.Parse(responsedata.ToString());
                        foreach (JObject root in objects)
                        {
                            dynamic obj = new System.Dynamic.ExpandoObject();
                            foreach (KeyValuePair<String, JToken> app in root)
                            {
                                var key = app.Key;
                                var value = app.Value;
                                if (key == "event_time")
                                {
                                    var a = value.Value<string>();
                                    DateTime dt = DateTime.ParseExact(a, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    ExpandoAddProperty(obj, key, dt);
                                }
                                else
                                {
                                    var a = value.Value<string>();
                                    ExpandoAddProperty(obj, key, a);
                                }

                            }
                            list.Add(obj);
                        }
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var doc in list)
                        {
                            descriptor.Index<object>(i => i
                                .Index("dail2000")
                                .Id((Id)doc.event_number)
                                .Document(doc));
                        }

                        var bulkResponse = client.Bulk(descriptor);

                    }

                }


            }

            return true;
        }
        public async Task<bool> PushTestDataToElasticDB()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var content = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {

                var fromDate = "2018-05-02 00:00:00";
                var toDate = "2018-05-02 23:59:59";
                var content1 = @"{""fromDate"" : ""#FROM_DATE#"" ,""toDate"" :""#To_DATE#"",""noOfRecords"":""10"" }";
                content1 = content1.Replace("#FROM_DATE#", fromDate);
                content1 = content1.Replace("#To_DATE#", toDate);
                var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                List<TestDial100ViewModel> list = new List<TestDial100ViewModel>();
                var url1 = "http://14.99.150.248/SCRBAPI/api/SCRB/Events";
                var address1 = new Uri(url1);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", "U0NSQjoxMDA=");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = address1,
                    Content = stringContent1,
                };
                var response1 = await httpClient.SendAsync(request);
                if (response1.IsSuccessStatusCode)
                {
                    var json = await response1.Content.ReadAsStringAsync();
                    var dataToken = JToken.Parse(json);
                    var responsedata = dataToken.First().SelectToken("eventInfo");
                    var objects = JArray.Parse(responsedata.ToString());
                    foreach (JObject root in objects)
                    {

                        var model = JsonConvert.DeserializeObject<TestDial100ViewModel>(root.ToString());

                        list.Add(model);
                    }
                    BulkDescriptor descriptor = new BulkDescriptor();
                    foreach (var doc in list)
                    {
                        descriptor.Index<object>(i => i
                            .Index("dail100")
                            .Id((Id)doc.event_number)
                            .Document(doc));
                    }

                    var bulkResponse = client.Bulk(descriptor);

                }




            }

            return true;
        }

        private void ExpandoAddProperty(System.Dynamic.ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        public async Task<JsonResult> ReadIIPAlertData()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<AlertViewModel> list = new List<AlertViewModel>();
            var query = ApplicationConstant.BusinessAnalytics.ReadIIPAlertDataQueryWithSearch;
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "iip_alert_data/_search?pretty=true&size=10000";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            //var _id = hit.SelectToken("_id");
                            //var id = _id.Value<string>();
                            var source = hit.SelectToken("_source");
                            var model = JsonConvert.DeserializeObject<AlertViewModel>(source.ToString());
                            model.Id = model.alertid;
                            list.Add(model);
                        }
                    }
                }
            }
            var newlist = list.DistinctBy(x => x.sourceIds).OrderByDescending(x => x.alert_date)/*.Take(20)*/.ToList();
            return Json(newlist);
        }
        public async Task<JsonResult> ReadIIPAlertGridData(string colorCode)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<AlertViewModel> list = new List<AlertViewModel>();
            var query = ApplicationConstant.BusinessAnalytics.ReadIipAlertList;
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "iip_alert_data/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            //var _id = hit.SelectToken("_id");
                            //var id = _id.Value<string>();
                            var source = hit.SelectToken("_source");
                            var model = JsonConvert.DeserializeObject<AlertViewModel>(source.ToString());
                            model.Id = model.alertid;
                            list.Add(model);
                        }
                    }
                }
            }
            if (colorCode.IsNotNullAndNotEmpty())
            {
                colorCode = "#" + colorCode;
                var newlist = list.Where(x => x.isVisible == true && x.colorCode == colorCode).DistinctBy(x => x.sourceIds).OrderByDescending(x => x.alert_date);
                return Json(newlist);
            }
            else
            {
                var newlist = list.DistinctBy(x => x.sourceIds).OrderByDescending(x => x.alert_date);
                return Json(newlist);
            }

        }
        public async Task<IActionResult> IipAlertActionReport(string parentId)
        {
            ViewBag.ParentId = parentId;
            return View();
        }
        public async Task<JsonResult> ReadIIPAlertActionReport(string parentId)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<AlertRemarkViewModel> list = new List<AlertRemarkViewModel>();
            var query = ApplicationConstant.BusinessAnalytics.ReadIipAlertActionList;
            query = query.Replace("#SEARCHWHERE#", parentId);
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "alert_action_remarks/_search?pretty=true&size=1000";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var model = JsonConvert.DeserializeObject<AlertRemarkViewModel>(source.ToString());
                            list.Add(model);
                        }
                    }
                }
            }
            return Json(list.OrderBy(x => x.remark_datetime));

        }
        public async Task<JsonResult> ReadIIPNotificationData()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<IIPNotificationViewModel> list = new List<IIPNotificationViewModel>();
            var query = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "dail100/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var model = JsonConvert.DeserializeObject<Dial100ViewModel>(source.ToString());
                            list.Add(new IIPNotificationViewModel
                            {
                                Incident = model.event_type,
                                IncidentSubType = model.event_subType,
                                IncidentNumber = model.event_number,
                                IncidentTime = model.event_time,
                                Latitude = Convert.ToDouble(model.latitude),
                                Longitude = Convert.ToDouble(model.longitude),
                                PoliceStation = model.police_Station,
                                Remark = model.event_remark
                            });
                        }
                    }
                }
            }
            return Json(list);
        }
        public async Task<IActionResult> DynamicGridviewIIP(string indexName, string gridName, string latitude, string longitude, string policeStation, string district)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<dynamic> list = new List<dynamic>();
            var query = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<CctnsCommonViewModel>(str);
                            list.Add(result);
                            //dynamic obj = new System.Dynamic.ExpandoObject();
                            //foreach (JProperty root in source)
                            //{

                            //    var key = root.Name;
                            //    var a = root.First().ToString();
                            //    ExpandoAddProperty(obj, key, a);
                            //}
                            //list.Add(obj);
                            break;
                        }
                    }
                }
            }
            ViewBag.IndexName = indexName;
            ViewBag.GridName = gridName;
            ViewBag.Latitude = latitude;
            ViewBag.Longitude = longitude;
            ViewBag.PoliceStation = policeStation;
            ViewBag.District = district;
            return View("Dail100List", list.ToDataTable());
        }
        public async Task<JsonResult> ReadCctnsDataList(string indexName, string policeStation, string district)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            //List<dynamic> list = new List<dynamic>();
            List<CctnsCommonViewModel> list = new List<CctnsCommonViewModel>();
            var query = ApplicationConstant.BusinessAnalytics.ReadCctnsCommonQuerywithDsitrictSearch;
            if (query.IsNotNullAndNotEmpty())
            {
                var disList = await _noteBusiness.GetAllDistrict();
                var dist = disList.Where(x => x.title == district).FirstOrDefault();
                query = query.Replace("#POLICESTATION#", policeStation.Replace("^", " "));
                query = query.Replace("#DISTRICT#", dist.IsNotNull() ? dist.Name : "");
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    if (trackdata.IsNotNull())
                    {
                        var hits = trackdata.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var total = hits.SelectToken("total");
                            var value = total.SelectToken("value");
                            var _hits = hits.SelectToken("hits");
                            foreach (var hit in _hits)
                            {
                                var source = hit.SelectToken("_source");
                                //var highlight = hit.SelectToken("highlight");
                                var str = JsonConvert.SerializeObject(source);
                                //var strhighlight = JsonConvert.SerializeObject(highlight);
                                //var result = JsonConvert.DeserializeObject<CctnsViewModel>(str);
                                //var resultHighlight = JsonConvert.DeserializeObject<CctnsArrayViewModel>(strhighlight);

                                //if (result.IsNotNull())
                                //{

                                //    var sres = new CctnsViewModel
                                //    {
                                //        ZONE_NAME = resultHighlight.ZONE_NAME != null ? string.Join("", resultHighlight.ZONE_NAME) : string.Join("", result.ZONE_NAME),
                                //        RANGE_NAME = resultHighlight.RANGE_NAME != null ? string.Join("", resultHighlight.RANGE_NAME) : string.Join("", result.RANGE_NAME),
                                //        DISTRICT = resultHighlight.DISTRICT != null ? string.Join("", resultHighlight.DISTRICT) : string.Join("", result.DISTRICT),
                                //        POLICE_STATION = resultHighlight.POLICE_STATION != null ? string.Join("", resultHighlight.POLICE_STATION) : string.Join("", result.POLICE_STATION),
                                //        REG_DT = result.REG_DT,
                                //        FIR_NUM = resultHighlight.FIR_NUM != null ? string.Join("", resultHighlight.FIR_NUM) : string.Join("", result.FIR_NUM),
                                //        ACT_SECTION = resultHighlight.ACT_SECTION != null ? string.Join("", resultHighlight.ACT_SECTION) : string.Join("", result.ACT_SECTION),
                                //        FIR_OCCURANCE_PLACE = resultHighlight.FIR_OCCURANCE_PLACE != null ? string.Join("", resultHighlight.FIR_OCCURANCE_PLACE) : string.Join("", result.FIR_OCCURANCE_PLACE),
                                //        FIR_COMPLINANT_DETAIL = resultHighlight.FIR_COMPLINANT_DETAIL != null ? string.Join("", resultHighlight.FIR_COMPLINANT_DETAIL) : string.Join("", result.FIR_COMPLINANT_DETAIL),
                                //        FIR_STATUS_TYPE = resultHighlight.FIR_STATUS_TYPE != null ? string.Join("", resultHighlight.FIR_STATUS_TYPE) : string.Join("", result.FIR_STATUS_TYPE),

                                //    };
                                //    list.Add(sres);

                                //}
                                var result = JsonConvert.DeserializeObject<CctnsCommonViewModel>(str);
                                //var resultHighlight = JsonConvert.DeserializeObject<CctnsCommonArrayViewModel>(strhighlight);

                                if (result.IsNotNull())
                                {
                                    //var _jsonstring = resultHighlight.JsonString != null ? string.Join("", resultHighlight.JsonString) : result.JsonString;
                                    //result.JsonString = _jsonstring;
                                    list.Add(result);

                                }

                            }
                        }
                    }

                }
            }
            return Json(list);
        }
        public async Task<IActionResult> DynamicGridviewIIPWithSearch(string indexName, string gridName, string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<dynamic> list = new List<dynamic>();
            var query = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            dynamic obj = new System.Dynamic.ExpandoObject();
                            foreach (JProperty root in source)
                            {

                                var key = root.Name;
                                var a = root.First().ToString();
                                ExpandoAddProperty(obj, key, a);
                            }
                            list.Add(obj);
                            break;
                        }
                    }
                }
            }
            ViewBag.IndexName = indexName;
            ViewBag.GridName = gridName;
            ViewBag.SearchStr = searchStr;
            ViewBag.IsAdvance = isAdvance;
            ViewBag.PlainSearch = plainSearch;
            ViewBag.DateFilterType = dateFilterType;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View("Dail100ListWithSearch", list.ToDataTable());
        }
        public async Task<JsonResult> ReadDial100DataListWithSearch(string indexName, string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var dateRange = GetStartEndDateByFilterType(dateFilterType, startDate, endDate);
            List<dynamic> list = new List<dynamic>();
            if (searchStr.IsNullOrEmpty())
            {
                var content = "";
                if (dateFilterType == SocialMediaDatefilters.AllTime)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery1;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery5;
                    content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                    content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                }
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    if (trackdata.IsNotNull())
                    {
                        var hits = trackdata.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var _hits = hits.SelectToken("hits");

                            foreach (var hit in _hits)
                            {
                                var source = hit.SelectToken("_source");
                                dynamic obj = new System.Dynamic.ExpandoObject();
                                foreach (JProperty root in source)
                                {

                                    var key = root.Name;
                                    var a = root.First().ToString();
                                    ExpandoAddProperty(obj, key, a);
                                }
                                list.Add(obj);
                            }
                        }
                    }

                }
            }
            else
            {
                var content = "";
                if (plainSearch && !isAdvance)
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery2;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery6;
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                    }
                }
                else if (!isAdvance)
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery3;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery7;
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    }
                }
                else
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery4;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery8;
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    }

                }
                content = content.Replace("#SEARCHWHERE#", searchStr.Replace("^", " "));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    if (trackdata.IsNotNull())
                    {
                        var hits = trackdata.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var total = hits.SelectToken("total");
                            var value = total.SelectToken("value");
                            var _hits = hits.SelectToken("hits");
                            foreach (var hit in _hits)
                            {
                                var source = hit.SelectToken("_source");
                                var highlight = hit.SelectToken("highlight");
                                var str = JsonConvert.SerializeObject(source);
                                var strhighlight = JsonConvert.SerializeObject(highlight);
                                var result = JsonConvert.DeserializeObject<Dial100ViewModel>(str);
                                var resultHighlight = JsonConvert.DeserializeObject<Dial100ArrayViewModel>(strhighlight);

                                if (result.IsNotNull())
                                {

                                    var sres = new Dial100ViewModel
                                    {
                                        caller_name = resultHighlight.caller_name != null ? string.Join("", resultHighlight.caller_name) : string.Join("", result.caller_name),
                                        caller_number = resultHighlight.caller_number != null ? string.Join("", resultHighlight.caller_number) : string.Join("", result.caller_number),
                                        event_number = resultHighlight.event_number != null ? string.Join("", resultHighlight.event_number) : string.Join("", result.event_number),
                                        event_time = result.event_time,
                                        event_remark = resultHighlight.event_remark != null ? string.Join("", resultHighlight.event_remark) : string.Join("", result.event_remark),
                                        event_type = resultHighlight.event_type != null ? string.Join("", resultHighlight.event_type) : string.Join("", result.event_type),
                                        event_subType = resultHighlight.event_subType != null ? string.Join("", resultHighlight.event_subType) : string.Join("", result.event_subType),
                                        latitude = resultHighlight.latitude != null ? string.Join("", resultHighlight.latitude) : string.Join("", result.latitude),
                                        longitude = resultHighlight.longitude != null ? string.Join("", resultHighlight.longitude) : string.Join("", result.longitude),
                                        district_Code = resultHighlight.district_Code != null ? string.Join("", resultHighlight.district_Code) : string.Join("", result.district_Code),
                                        police_Station = resultHighlight.police_Station != null ? string.Join("", resultHighlight.police_Station) : string.Join("", result.police_Station),
                                        Frv_Code = resultHighlight.Frv_Code != null ? string.Join("", resultHighlight.Frv_Code) : string.Join("", result.Frv_Code),
                                        dispatch_Time = resultHighlight.dispatch_Time != null ? string.Join("", resultHighlight.dispatch_Time) : string.Join("", result.dispatch_Time),
                                        acknowledge_Time = resultHighlight.acknowledge_Time != null ? string.Join("", resultHighlight.acknowledge_Time) : string.Join("", result.acknowledge_Time),
                                        enroute_Time = resultHighlight.enroute_Time != null ? string.Join("", resultHighlight.enroute_Time) : string.Join("", result.enroute_Time),
                                        arrival_Time = resultHighlight.arrival_Time != null ? string.Join("", resultHighlight.arrival_Time) : string.Join("", result.arrival_Time),
                                        closing_Time = resultHighlight.closing_Time != null ? string.Join("", resultHighlight.closing_Time) : string.Join("", result.closing_Time),
                                        disposition_Code = resultHighlight.disposition_Code != null ? string.Join("", resultHighlight.disposition_Code) : string.Join("", result.disposition_Code),
                                    };
                                    list.Add(sres);

                                }
                                //var source = hit.SelectToken("_source");
                                //dynamic obj = new System.Dynamic.ExpandoObject();
                                //foreach (JProperty root in source)
                                //{

                                //    var key = root.Name;
                                //    var a = root.First().ToString();
                                //    ExpandoAddProperty(obj, key, a);
                                //}

                                //var highlights = hit.SelectToken("highlight");
                                //dynamic obj1 = new System.Dynamic.ExpandoObject();
                                //foreach (JProperty root in highlights)
                                //{

                                //    var key = root.Name;
                                //    var a = root.First().ToString();
                                //    ExpandoAddProperty(obj1, key, a);
                                //}


                            }
                        }
                    }

                }
            }
            return Json(list);
        }
        public async Task<IActionResult> CctnsGridViewWithSearch(string indexName, string filterColumn, string gridName, string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<dynamic> list = new List<dynamic>();
            //List<CctnsCommonViewModel> list = new List<CctnsCommonViewModel>();
            var query = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<CctnsCommonViewModel>(str);
                            list.Add(result);
                            //dynamic obj = new System.Dynamic.ExpandoObject();
                            //foreach (JProperty root in source)
                            //{

                            //    var key = root.Name;
                            //    var a = root.First().ToString();
                            //    ExpandoAddProperty(obj, key, a);
                            //}
                            //list.Add(obj);
                            break;
                        }
                    }
                }
            }
            ViewBag.Heading = indexName.ToUpper();
            ViewBag.IndexName = indexName;
            ViewBag.FilterColumn = filterColumn;
            ViewBag.GridName = gridName;
            ViewBag.SearchStr = searchStr.HtmlEncode();
            ViewBag.IsAdvance = isAdvance;
            ViewBag.PlainSearch = plainSearch;
            ViewBag.DateFilterType = dateFilterType;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View("CctnsGridViewWithSearch", list.ToDataTable());
        }
        public async Task<JsonResult> ReadCctnsDataListWithSearch(string indexName, string filterColumn, string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var dateRange = GetStartEndDateByFilterType(dateFilterType, startDate, endDate);
            //List<dynamic> list = new List<dynamic>();
            List<CctnsCommonViewModel> list = new List<CctnsCommonViewModel>();
            if (searchStr.IsNullOrEmpty())
            {
                var content = "";

                if (dateFilterType == SocialMediaDatefilters.AllTime)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery1;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCctnsDataQuery1;
                    content = content.Replace("#FILTERCOLUMN#", filterColumn);
                    content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                    content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                }
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    if (trackdata.IsNotNull())
                    {
                        var hits = trackdata.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var _hits = hits.SelectToken("hits");

                            foreach (var hit in _hits)
                            {
                                var source = hit.SelectToken("_source");
                                var str = JsonConvert.SerializeObject(source);
                                var result = JsonConvert.DeserializeObject<CctnsCommonViewModel>(str);
                                if (result.IsNotNull())
                                {
                                    list.Add(result);
                                }
                                //dynamic obj = new System.Dynamic.ExpandoObject();
                                //foreach (JProperty root in source)
                                //{

                                //    var key = root.Name;
                                //    var a = root.First().ToString();
                                //    ExpandoAddProperty(obj, key, a);
                                //}
                                //list.Add(obj);
                            }
                        }
                    }

                }
            }
            else
            {
                var content = "";
                if (plainSearch)
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCctnsDataQuery2;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCctnsDataQuery6;
                        content = content.Replace("#FILTERCOLUMN#", filterColumn);
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                    }
                }
                else if (!isAdvance)
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCctnsDataQuery3;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCctnsDataQuery7;
                        content = content.Replace("#FILTERCOLUMN#", filterColumn);
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    }
                }
                else
                {
                    if (dateFilterType == SocialMediaDatefilters.AllTime)
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCctnsDataQuery4;
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadCctnsDataQuery8;
                        content = content.Replace("#FILTERCOLUMN#", filterColumn);
                        content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                    }
                }
                content = content.Replace("#SEARCHWHERE#", searchStr.Replace("^", " "));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + indexName + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    if (trackdata.IsNotNull())
                    {
                        var hits = trackdata.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var total = hits.SelectToken("total");
                            var value = total.SelectToken("value");
                            var _hits = hits.SelectToken("hits");
                            foreach (var hit in _hits)
                            {
                                var source = hit.SelectToken("_source");
                                var highlight = hit.SelectToken("highlight");
                                var str = JsonConvert.SerializeObject(source);
                                var strhighlight = JsonConvert.SerializeObject(highlight);
                                var result = JsonConvert.DeserializeObject<CctnsCommonViewModel>(str);
                                var resultHighlight = JsonConvert.DeserializeObject<CctnsCommonArrayViewModel>(strhighlight);

                                if (result.IsNotNull())
                                {
                                    var _jsonstring = resultHighlight.JsonString != null ? string.Join("", resultHighlight.JsonString) : result.JsonString;
                                    result.JsonString = _jsonstring;
                                    list.Add(result);

                                }
                                //dynamic obj = new System.Dynamic.ExpandoObject();
                                //foreach (JProperty root in source)
                                //{

                                //    var key = root.Name;
                                //    var a = root.First().ToString();
                                //    ExpandoAddProperty(obj, key, a);
                                //}
                                //list.Add(obj);

                            }
                        }
                    }

                }
            }
            return Json(list);
        }
        //public async Task<bool> ExecuteCubeJs(string id)
        //{
        //    var cubeJs = ApplicationConstant.AppSettings.CubeJsUrl(_configuration);
        //    var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
        //    var settings = new ConnectionSettings(new Uri(eldbUrl));
        //    var client = new ElasticClient(settings);
        //    BulkDescriptor descriptor = new BulkDescriptor();
        //    var rule = await _noteBusiness.GetNotificationALertDetails(id);
        //    if (rule.columnReferenceId.IsNotNullAndNotEmpty())
        //    {
        //        var content = @"{""measures"": [""#MEASURE#""],""dimensions"": #DIMENSION#,""timeDimensions"": [#TIMEDIMENSIONS#],""limit"":#LIMIT#,""order"": [[""Dial100Data.event_time"",""asc""]],""filters"": [" + rule.cubeJsFilter + "]}";
        //        //rule.queryColumns = rule.queryColumns.IsNotNullAndNotEmpty() ? rule.queryColumns.Replace(",", "\", \"") : rule.queryColumns;
        //        content = content.Replace("#MEASURE#", rule.queryTableId);
        //        content = content.Replace("#DIMENSION#", rule.queryColumns);
        //        content = content.Replace("#TIMEDIMENSIONS#", rule.timeDimensionFilter);
        //        content = content.Replace("#LIMIT#", rule.limit.IsNullOrEmpty() ? "10" : rule.limit);
        //        //content = content.Replace("equal", "equals");
        //        var tempObj = JsonConvert.DeserializeObject(content);
        //        var str = JsonConvert.SerializeObject(tempObj);
        //        var handler = new HttpClientHandler();
        //        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        //        handler.ServerCertificateCustomValidationCallback =
        //            (httpRequestMessage, cert, cetChain, policyErrors) =>
        //            {
        //                return true;
        //            };

        //        using (var httpClient = new HttpClient(handler))
        //        {
        //            var url1 = $@"{cubeJs}cubejs-api/v1/load?query={str}";
        //            var address = new Uri(url1);
        //            var response = await httpClient.GetAsync(address);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var _jsondata = await response.Content.ReadAsStringAsync();
        //                var _dataToken = JToken.Parse(_jsondata);
        //                var _data = _dataToken.SelectToken("data");
        //                if (_data.IsNotNull())
        //                {
        //                    var count = _data.Count();
        //                    if (count > 0)
        //                    {
        //                        var measurename = rule.queryTableId.Split('.')[0] + ".";
        //                        var datastr = _data.ToString().Replace(measurename, "");
        //                        var datalist = JsonConvert.DeserializeObject<List<Dial100ViewModel>>(datastr);
        //                        if (rule.conditionFunction == "count")
        //                        {
        //                            if (rule.conditionType == "isAbove")
        //                            {
        //                                if (rule.frequency.IsNotNullAndNotEmpty())
        //                                {
        //                                    while (datalist.Count() > 0)
        //                                    {
        //                                        dynamic obj = new System.Dynamic.ExpandoObject();
        //                                        var list = datalist.Take(rule.frequency.ToSafeInt()).ToList();
        //                                        datalist = datalist.Except(list).ToList();
        //                                        if (list.Count() > rule.conditionValue.ToSafeInt())
        //                                        {
        //                                            var evnetTime = list.Last().event_time;
        //                                            if (evnetTime.IsNotNull())
        //                                            {
        //                                                var event_date = evnetTime.ToSafeDateTime();
        //                                                ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                            }
        //                                            foreach (var property in rule.GetType().GetProperties())
        //                                            {
        //                                                var _key = property.Name;
        //                                                var _value = property.GetValue(rule);
        //                                                if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                                {
        //                                                    ExpandoAddProperty(obj, _key, _value);
        //                                                }

        //                                            }
        //                                            var ids = new List<string>();
        //                                            foreach (var item in list)
        //                                            {
        //                                                ids.Add(item.event_number);
        //                                            }
        //                                            //var ids = new List<string>();
        //                                            //foreach (var item in list)
        //                                            //{
        //                                            //    foreach (JProperty pr in item)
        //                                            //    {
        //                                            //        if (pr.Name == rule.columnReferenceId)
        //                                            //        {
        //                                            //            var ev = pr.First().Value<string>();
        //                                            //            if (ev.IsNotNull())
        //                                            //            {
        //                                            //                ids.Add(ev);
        //                                            //            }

        //                                            //        }

        //                                            //    }

        //                                            //}
        //                                            if (ids.Count > 0)
        //                                            {
        //                                                var filIds = String.Join(",", ids);
        //                                                ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                            }
        //                                            var year = DateTime.Now.Year;
        //                                            var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                            content3 = content3.Replace("#YEAR#", year.ToString());
        //                                            var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                            var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                            var address3 = new Uri(url3);
        //                                            var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                            if (response3.IsSuccessStatusCode)
        //                                            {
        //                                                var json3 = await response3.Content.ReadAsStringAsync();
        //                                                var data3 = JToken.Parse(json3);
        //                                                if (data3.IsNotNull())
        //                                                {
        //                                                    var hits = data3.SelectToken("hits");
        //                                                    if (hits.IsNotNull())
        //                                                    {
        //                                                        var total = hits.SelectToken("total");
        //                                                        if (total.IsNotNull())
        //                                                        {
        //                                                            var tt = total.First();
        //                                                            var countstr = tt.Last().ToString();
        //                                                            var count3 = countstr.ToSafeInt();
        //                                                            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                            else if (response3.ReasonPhrase == "Not Found")
        //                                            {
        //                                                var count3 = 0;
        //                                                var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                            }
        //                                            else
        //                                            {
        //                                                return false;
        //                                            }
        //                                            var alertId = Guid.NewGuid().ToString();
        //                                            ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                            ExpandoAddProperty(obj, "alertid", alertId);
        //                                            ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                            ExpandoAddProperty(obj, "isRead", false);
        //                                            ExpandoAddProperty(obj, "isVisible", true);
        //                                            descriptor.Index<object>(i => i
        //                                                .Index("iip_alert_data")
        //                                                .Id((Id)alertId)
        //                                                .Document(obj));

        //                                            var bulkResponse = client.Bulk(descriptor);
        //                                            if (bulkResponse.ApiCall.Success)
        //                                            {

        //                                                var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                                var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                                content2 = content2.Replace("#IDS#", filIds);
        //                                                var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                                var url2 = eldbUrl + "dail100/_update_by_query";
        //                                                var address2 = new Uri(url2);
        //                                                var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                            }
        //                                        }
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    dynamic obj = new System.Dynamic.ExpandoObject();
        //                                    var list = datalist;
        //                                    if (list.Count() > rule.conditionValue.ToSafeInt())
        //                                    {
        //                                        var evnetTime = list.Last().event_time;
        //                                        if (evnetTime.IsNotNull())
        //                                        {
        //                                            var event_date = evnetTime.ToSafeDateTime();
        //                                            ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                        }
        //                                        foreach (var property in rule.GetType().GetProperties())
        //                                        {
        //                                            var _key = property.Name;
        //                                            var _value = property.GetValue(rule);
        //                                            if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                            {
        //                                                ExpandoAddProperty(obj, _key, _value);
        //                                            }

        //                                        }
        //                                        var ids = new List<string>();
        //                                        foreach (var item in list)
        //                                        {
        //                                            ids.Add(item.event_number);
        //                                        }
        //                                        //var ids = new List<string>();
        //                                        //foreach (var item in list)
        //                                        //{
        //                                        //    foreach (JProperty pr in item)
        //                                        //    {
        //                                        //        if (pr.Name == rule.columnReferenceId)
        //                                        //        {
        //                                        //            var ev = pr.First().Value<string>();
        //                                        //            if (ev.IsNotNull())
        //                                        //            {
        //                                        //                ids.Add(ev);
        //                                        //            }

        //                                        //        }

        //                                        //    }

        //                                        //}
        //                                        if (ids.Count > 0)
        //                                        {
        //                                            var filIds = String.Join(",", ids);
        //                                            ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                        }
        //                                        var year = DateTime.Now.Year;
        //                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                        content3 = content3.Replace("#YEAR#", year.ToString());
        //                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                        var address3 = new Uri(url3);
        //                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                        if (response3.IsSuccessStatusCode)
        //                                        {
        //                                            var json3 = await response3.Content.ReadAsStringAsync();
        //                                            var data3 = JToken.Parse(json3);
        //                                            if (data3.IsNotNull())
        //                                            {
        //                                                var hits = data3.SelectToken("hits");
        //                                                if (hits.IsNotNull())
        //                                                {
        //                                                    var total = hits.SelectToken("total");
        //                                                    if (total.IsNotNull())
        //                                                    {
        //                                                        var tt = total.First();
        //                                                        var countstr = tt.Last().ToString();
        //                                                        var count3 = countstr.ToSafeInt();
        //                                                        var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                        ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        else if (response3.ReasonPhrase == "Not Found")
        //                                        {
        //                                            var count3 = 0;
        //                                            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                        }
        //                                        else
        //                                        {
        //                                            return false;
        //                                        }
        //                                        var alertId = Guid.NewGuid().ToString();
        //                                        ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                        ExpandoAddProperty(obj, "alertid", alertId);
        //                                        ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                        ExpandoAddProperty(obj, "isRead", false);
        //                                        ExpandoAddProperty(obj, "isVisible", true);
        //                                        descriptor.Index<object>(i => i
        //                                            .Index("iip_alert_data")
        //                                            .Id((Id)alertId)
        //                                            .Document(obj));

        //                                        var bulkResponse = client.Bulk(descriptor);
        //                                        if (bulkResponse.ApiCall.Success)
        //                                        {

        //                                            var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                            var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                            content2 = content2.Replace("#IDS#", filIds);
        //                                            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                            var url2 = eldbUrl + "dail100/_update_by_query";
        //                                            var address2 = new Uri(url2);
        //                                            var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                        }
        //                                    }
        //                                }


        //                            }
        //                            else if (rule.conditionType == "isBelow")
        //                            {
        //                                if (rule.frequency.IsNotNullAndNotEmpty())
        //                                {
        //                                    while (datalist.Count() > 0)
        //                                    {
        //                                        dynamic obj = new System.Dynamic.ExpandoObject();
        //                                        var list = datalist.Take(rule.frequency.ToSafeInt()).ToList();
        //                                        datalist = datalist.Except(list).ToList();
        //                                        if (list.Count() < rule.conditionValue.ToSafeInt())
        //                                        {
        //                                            var evnetTime = list.Last().event_time;
        //                                            if (evnetTime.IsNotNull())
        //                                            {
        //                                                var event_date = evnetTime.ToSafeDateTime();
        //                                                ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                            }
        //                                            foreach (var property in rule.GetType().GetProperties())
        //                                            {
        //                                                var _key = property.Name;
        //                                                var _value = property.GetValue(rule);
        //                                                if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                                {
        //                                                    ExpandoAddProperty(obj, _key, _value);
        //                                                }

        //                                            }
        //                                            var ids = new List<string>();
        //                                            foreach (var item in list)
        //                                            {
        //                                                ids.Add(item.event_number);
        //                                            }
        //                                            //var ids = new List<string>();
        //                                            //foreach (var item in list)
        //                                            //{
        //                                            //    foreach (JProperty pr in item)
        //                                            //    {
        //                                            //        if (pr.Name == rule.columnReferenceId)
        //                                            //        {
        //                                            //            var ev = pr.First().Value<string>();
        //                                            //            if (ev.IsNotNull())
        //                                            //            {
        //                                            //                ids.Add(ev);
        //                                            //            }

        //                                            //        }

        //                                            //    }

        //                                            //}
        //                                            if (ids.Count > 0)
        //                                            {
        //                                                var filIds = String.Join(",", ids);
        //                                                ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                            }
        //                                            var year = DateTime.Now.Year;
        //                                            var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                            content3 = content3.Replace("#YEAR#", year.ToString());
        //                                            var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                            var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                            var address3 = new Uri(url3);
        //                                            var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                            if (response3.IsSuccessStatusCode)
        //                                            {
        //                                                var json3 = await response3.Content.ReadAsStringAsync();
        //                                                var data3 = JToken.Parse(json3);
        //                                                if (data3.IsNotNull())
        //                                                {
        //                                                    var hits = data3.SelectToken("hits");
        //                                                    if (hits.IsNotNull())
        //                                                    {
        //                                                        var total = hits.SelectToken("total");
        //                                                        if (total.IsNotNull())
        //                                                        {
        //                                                            var tt = total.First();
        //                                                            var countstr = tt.Last().ToString();
        //                                                            var count3 = countstr.ToSafeInt();
        //                                                            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                            else if (response3.ReasonPhrase == "Not Found")
        //                                            {
        //                                                var count3 = 0;
        //                                                var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                            }
        //                                            else
        //                                            {
        //                                                return false;
        //                                            }
        //                                            var alertId = Guid.NewGuid().ToString();
        //                                            ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                            ExpandoAddProperty(obj, "alertid", alertId);
        //                                            ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                            ExpandoAddProperty(obj, "isRead", false);
        //                                            ExpandoAddProperty(obj, "isVisible", true);
        //                                            descriptor.Index<object>(i => i
        //                                                .Index("iip_alert_data")
        //                                                .Id((Id)alertId)
        //                                                .Document(obj));

        //                                            var bulkResponse = client.Bulk(descriptor);
        //                                            if (bulkResponse.ApiCall.Success)
        //                                            {

        //                                                var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                                var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                                content2 = content2.Replace("#IDS#", filIds);
        //                                                var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                                var url2 = eldbUrl + "dail100/_update_by_query";
        //                                                var address2 = new Uri(url2);
        //                                                var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                            }
        //                                        }
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    dynamic obj = new System.Dynamic.ExpandoObject();
        //                                    var list = datalist;
        //                                    if (list.Count() < rule.conditionValue.ToSafeInt())
        //                                    {
        //                                        var evnetTime = list.Last().event_time;
        //                                        if (evnetTime.IsNotNull())
        //                                        {
        //                                            var event_date = evnetTime.ToSafeDateTime();
        //                                            ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                        }
        //                                        foreach (var property in rule.GetType().GetProperties())
        //                                        {
        //                                            var _key = property.Name;
        //                                            var _value = property.GetValue(rule);
        //                                            if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                            {
        //                                                ExpandoAddProperty(obj, _key, _value);
        //                                            }

        //                                        }
        //                                        var ids = new List<string>();
        //                                        foreach (var item in list)
        //                                        {
        //                                            ids.Add(item.event_number);
        //                                        }
        //                                        //var ids = new List<string>();
        //                                        //foreach (var item in list)
        //                                        //{
        //                                        //    foreach (JProperty pr in item)
        //                                        //    {
        //                                        //        if (pr.Name == rule.columnReferenceId)
        //                                        //        {
        //                                        //            var ev = pr.First().Value<string>();
        //                                        //            if (ev.IsNotNull())
        //                                        //            {
        //                                        //                ids.Add(ev);
        //                                        //            }

        //                                        //        }

        //                                        //    }

        //                                        //}
        //                                        if (ids.Count > 0)
        //                                        {
        //                                            var filIds = String.Join(",", ids);
        //                                            ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                        }
        //                                        var year = DateTime.Now.Year;
        //                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                        content3 = content3.Replace("#YEAR#", year.ToString());
        //                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                        var address3 = new Uri(url3);
        //                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                        if (response3.IsSuccessStatusCode)
        //                                        {
        //                                            var json3 = await response3.Content.ReadAsStringAsync();
        //                                            var data3 = JToken.Parse(json3);
        //                                            if (data3.IsNotNull())
        //                                            {
        //                                                var hits = data3.SelectToken("hits");
        //                                                if (hits.IsNotNull())
        //                                                {
        //                                                    var total = hits.SelectToken("total");
        //                                                    if (total.IsNotNull())
        //                                                    {
        //                                                        var tt = total.First();
        //                                                        var countstr = tt.Last().ToString();
        //                                                        var count3 = countstr.ToSafeInt();
        //                                                        var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                        ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        else if (response3.ReasonPhrase == "Not Found")
        //                                        {
        //                                            var count3 = 0;
        //                                            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                        }
        //                                        else
        //                                        {
        //                                            return false;
        //                                        }
        //                                        var alertId = Guid.NewGuid().ToString();
        //                                        ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                        ExpandoAddProperty(obj, "alertid", alertId);
        //                                        ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                        ExpandoAddProperty(obj, "isRead", false);
        //                                        ExpandoAddProperty(obj, "isVisible", true);
        //                                        descriptor.Index<object>(i => i
        //                                            .Index("iip_alert_data")
        //                                            .Id((Id)alertId)
        //                                            .Document(obj));

        //                                        var bulkResponse = client.Bulk(descriptor);
        //                                        if (bulkResponse.ApiCall.Success)
        //                                        {

        //                                            var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                            var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                            content2 = content2.Replace("#IDS#", filIds);
        //                                            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                            var url2 = eldbUrl + "dail100/_update_by_query";
        //                                            var address2 = new Uri(url2);
        //                                            var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else//grouping
        //                        {
        //                            if (rule.groupbyColumns.IsNotNullAndNotEmpty())
        //                            {
        //                                var grpColArr = rule.groupbyColumns.Replace(measurename, "").Split(',');
        //                                var gfList = new List<BuilderFilterViewModel>();
        //                                var gf = JsonConvert.DeserializeObject<dynamic>(rule.groupFilters);
        //                                foreach (var item in gf.rules)
        //                                {
        //                                    var f = item["field"].Value;
        //                                    var o = item["operator"].Value;
        //                                    var v = item["value"].Value;
        //                                    var gf1 = new BuilderFilterViewModel { Field = f.Replace(measurename, ""), Operator = o, Value = v };
        //                                    gfList.Add(gf1);
        //                                }

        //                                //var grpDatalist = datalist.GroupBy(x => new { x.district_Code,x.event_type}).ToList();
        //                                if (grpColArr.Length == 1)
        //                                {                                           
        //                                    var grpDatalist = datalist.GroupBy(x => x.GetType().GetProperty(grpColArr[0]).GetValue(x, null)).ToList();
        //                                    foreach (var dataItem in grpDatalist)
        //                                    {
        //                                        var key = dataItem.Key;
        //                                        var newlist = dataItem.ToList();
        //                                        if (gfList.Count > 0)
        //                                        {
        //                                            var distCount = newlist.DistinctBy(x => x.GetType().GetProperty(gfList[0].Field).GetValue(x, null)).Count();
        //                                            var distList = newlist.DistinctBy(x => x.GetType().GetProperty(gfList[0].Field).GetValue(x, null)).Select(x => x.GetType().GetProperty(gfList[0].Field).GetValue(x, null)).ToList();
        //                                            if (gfList[0].Operator == "exist")
        //                                            {
        //                                                var isvalid = distList.Where(x => x.ToString() == gfList[0].Value).Any();
        //                                                if(isvalid && distCount > rule.conditionValue.ToSafeInt() /*&& rule.conditionType == "isAbove"*/)
        //                                                {

        //                                                    dynamic obj = new System.Dynamic.ExpandoObject();
        //                                                    var list = newlist;                                                           

        //                                                    var evnetTime = list.Last().event_time;
        //                                                    if (evnetTime.IsNotNull())
        //                                                    {
        //                                                        var event_date = evnetTime.ToSafeDateTime();
        //                                                        ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                                    }
        //                                                    foreach (var property in rule.GetType().GetProperties())
        //                                                    {
        //                                                        var _key = property.Name;
        //                                                        var _value = property.GetValue(rule);
        //                                                        if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                                        {
        //                                                            ExpandoAddProperty(obj, _key, _value);
        //                                                        }

        //                                                    }
        //                                                    var ids = new List<string>();
        //                                                    foreach (var item in list)
        //                                                    {
        //                                                        ids.Add(item.event_number);
        //                                                    }
        //                                                    if (ids.Count > 0)
        //                                                    {
        //                                                        var filIds = String.Join(",", ids);
        //                                                        ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                                    }
        //                                                    var year = DateTime.Now.Year;
        //                                                    var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                                    content3 = content3.Replace("#YEAR#", year.ToString());
        //                                                    var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                                    var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                                    var address3 = new Uri(url3);
        //                                                    var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                                    if (response3.IsSuccessStatusCode)
        //                                                    {
        //                                                        var json3 = await response3.Content.ReadAsStringAsync();
        //                                                        var data3 = JToken.Parse(json3);
        //                                                        if (data3.IsNotNull())
        //                                                        {
        //                                                            var hits = data3.SelectToken("hits");
        //                                                            if (hits.IsNotNull())
        //                                                            {
        //                                                                var total = hits.SelectToken("total");
        //                                                                if (total.IsNotNull())
        //                                                                {
        //                                                                    var tt = total.First();
        //                                                                    var countstr = tt.Last().ToString();
        //                                                                    var count3 = countstr.ToSafeInt();
        //                                                                    var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                                    ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                                }
        //                                                            }
        //                                                        }
        //                                                    }
        //                                                    else if (response3.ReasonPhrase == "Not Found")
        //                                                    {
        //                                                        var count3 = 0;
        //                                                        var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                        ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        return false;
        //                                                    }
        //                                                    var alertId = Guid.NewGuid().ToString();
        //                                                    ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                                    ExpandoAddProperty(obj, "alertid", alertId);
        //                                                    ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                                    ExpandoAddProperty(obj, "isRead", false);
        //                                                    ExpandoAddProperty(obj, "isVisible", true);
        //                                                    descriptor.Index<object>(i => i
        //                                                        .Index("iip_alert_data")
        //                                                        .Id((Id)alertId)
        //                                                        .Document(obj));

        //                                                    var bulkResponse = client.Bulk(descriptor);
        //                                                    if (bulkResponse.ApiCall.Success)
        //                                                        {

        //                                                            var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                                            var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                                            content2 = content2.Replace("#IDS#", filIds);
        //                                                            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                                            var url2 = eldbUrl + "dail100/_update_by_query";
        //                                                            var address2 = new Uri(url2);
        //                                                            var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                                        }

        //                                                }

        //                                            }
        //                                            else if (gfList[0].Operator == "contains")
        //                                            {

        //                                            }
        //                                            else if (gfList[0].Operator == "greterthan")
        //                                            {

        //                                            }
        //                                            else if (gfList[0].Operator == "lessthan")
        //                                            {

        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            if (rule.conditionType == "isAbove")
        //                                            {
        //                                                if (rule.frequency.IsNotNullAndNotEmpty())
        //                                                {
        //                                                    while (newlist.Count() > 0)
        //                                                    {
        //                                                        dynamic obj = new System.Dynamic.ExpandoObject();
        //                                                        var list = newlist.Take(rule.frequency.ToSafeInt()).ToList();
        //                                                        newlist = newlist.Except(list).ToList();
        //                                                        if (list.Count() > rule.conditionValue.ToSafeInt())
        //                                                        {
        //                                                            var evnetTime = list.Last().event_time;
        //                                                            if (evnetTime.IsNotNull())
        //                                                            {
        //                                                                var event_date = evnetTime.ToSafeDateTime();
        //                                                                ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                                            }
        //                                                            foreach (var property in rule.GetType().GetProperties())
        //                                                            {
        //                                                                var _key = property.Name;
        //                                                                var _value = property.GetValue(rule);
        //                                                                if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                                                {
        //                                                                    ExpandoAddProperty(obj, _key, _value);
        //                                                                }

        //                                                            }
        //                                                            var ids = new List<string>();
        //                                                            foreach (var item in list)
        //                                                            {
        //                                                                ids.Add(item.event_number);
        //                                                            }
        //                                                            if (ids.Count > 0)
        //                                                            {
        //                                                                var filIds = String.Join(",", ids);
        //                                                                ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                                            }
        //                                                            var year = DateTime.Now.Year;
        //                                                            var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                                            content3 = content3.Replace("#YEAR#", year.ToString());
        //                                                            var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                                            var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                                            var address3 = new Uri(url3);
        //                                                            var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                                            if (response3.IsSuccessStatusCode)
        //                                                            {
        //                                                                var json3 = await response3.Content.ReadAsStringAsync();
        //                                                                var data3 = JToken.Parse(json3);
        //                                                                if (data3.IsNotNull())
        //                                                                {
        //                                                                    var hits = data3.SelectToken("hits");
        //                                                                    if (hits.IsNotNull())
        //                                                                    {
        //                                                                        var total = hits.SelectToken("total");
        //                                                                        if (total.IsNotNull())
        //                                                                        {
        //                                                                            var tt = total.First();
        //                                                                            var countstr = tt.Last().ToString();
        //                                                                            var count3 = countstr.ToSafeInt();
        //                                                                            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                                            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                                        }
        //                                                                    }
        //                                                                }
        //                                                            }
        //                                                            else if (response3.ReasonPhrase == "Not Found")
        //                                                            {
        //                                                                var count3 = 0;
        //                                                                var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                                ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                return false;
        //                                                            }
        //                                                            var alertId = Guid.NewGuid().ToString();
        //                                                            ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                                            ExpandoAddProperty(obj, "alertid", alertId);
        //                                                            ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                                            ExpandoAddProperty(obj, "isRead", false);
        //                                                            ExpandoAddProperty(obj, "isVisible", true);
        //                                                            descriptor.Index<object>(i => i
        //                                                                .Index("iip_alert_data")
        //                                                                .Id((Id)alertId)
        //                                                                .Document(obj));

        //                                                            var bulkResponse = client.Bulk(descriptor);
        //                                                            if (bulkResponse.ApiCall.Success)
        //                                                            {

        //                                                                var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                                                var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                                                content2 = content2.Replace("#IDS#", filIds);
        //                                                                var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                                                var url2 = eldbUrl + "dail100/_update_by_query";
        //                                                                var address2 = new Uri(url2);
        //                                                                var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                                            }
        //                                                        }
        //                                                    }

        //                                                }
        //                                            }
        //                                        }
        //                                        //else if (rule.conditionType == "isBelow")
        //                                        //{
        //                                        //    if (cc < rule.conditionValue.ToSafeInt() )
        //                                        //    {
        //                                        //        var evnetTime = dataItem.ToList()[rule.frequency.ToSafeInt() - 1].event_time;
        //                                        //        if (evnetTime.IsNotNull())
        //                                        //        {
        //                                        //            var event_date = evnetTime.ToSafeDateTime();
        //                                        //            ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                        //        }
        //                                        //        foreach (var property in rule.GetType().GetProperties())
        //                                        //        {
        //                                        //            var _key = property.Name;
        //                                        //            var _value = property.GetValue(rule);
        //                                        //            if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                        //            {
        //                                        //                ExpandoAddProperty(obj, _key, _value);
        //                                        //            }

        //                                        //        }
        //                                        //        var ids = new List<string>();
        //                                        //        foreach (var item in dataItem)
        //                                        //        {
        //                                        //            ids.Add(item.event_number);
        //                                        //        }
        //                                        //        //foreach (var item in _data)
        //                                        //        //{
        //                                        //        //    foreach (JProperty pr in item)
        //                                        //        //    {
        //                                        //        //        if (pr.Name == rule.columnReferenceId)
        //                                        //        //        {
        //                                        //        //            var ev = pr.First().Value<string>();
        //                                        //        //            if (ev.IsNotNull())
        //                                        //        //            {
        //                                        //        //                ids.Add(ev);
        //                                        //        //            }

        //                                        //        //        }

        //                                        //        //    }

        //                                        //        //}
        //                                        //        if (ids.Count > 0)
        //                                        //        {
        //                                        //            var filIds = String.Join(",", ids);
        //                                        //            ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                        //        }
        //                                        //        var year = DateTime.Now.Year;
        //                                        //        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                        //        content3 = content3.Replace("#YEAR#", year.ToString());
        //                                        //        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                        //        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                        //        var address3 = new Uri(url3);
        //                                        //        var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                        //        if (response3.IsSuccessStatusCode)
        //                                        //        {
        //                                        //            var json3 = await response3.Content.ReadAsStringAsync();
        //                                        //            var data3 = JToken.Parse(json3);
        //                                        //            if (data3.IsNotNull())
        //                                        //            {
        //                                        //                var hits = data3.SelectToken("hits");
        //                                        //                if (hits.IsNotNull())
        //                                        //                {
        //                                        //                    var total = hits.SelectToken("total");
        //                                        //                    if (total.IsNotNull())
        //                                        //                    {
        //                                        //                        var tt = total.First();
        //                                        //                        var countstr = tt.Last().ToString();
        //                                        //                        var count3 = countstr.ToSafeInt();
        //                                        //                        var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                        //                        ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                        //                    }
        //                                        //                }
        //                                        //            }
        //                                        //        }
        //                                        //        else if (response3.ReasonPhrase == "Not Found")
        //                                        //        {
        //                                        //            var count3 = 0;
        //                                        //            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                        //            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                        //        }
        //                                        //        else
        //                                        //        {
        //                                        //            return false;
        //                                        //        }
        //                                        //        var alertId = Guid.NewGuid().ToString();
        //                                        //        ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                        //        ExpandoAddProperty(obj, "alertid", alertId);
        //                                        //        ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                        //        ExpandoAddProperty(obj, "isRead", false);
        //                                        //        ExpandoAddProperty(obj, "isVisible", true);                                                    
        //                                        //        descriptor.Index<object>(i => i
        //                                        //            .Index("iip_alert_data")
        //                                        //            .Id((Id)alertId)
        //                                        //            .Document(obj));

        //                                        //        var bulkResponse = client.Bulk(descriptor);
        //                                        //        if (bulkResponse.ApiCall.Success)
        //                                        //        {

        //                                        //            var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[#IDS#]}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";                                                        
        //                                        //            var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                        //            content2 = content2.Replace("#IDS#", filIds);
        //                                        //            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                        //            var url2 = eldbUrl + "dail100/_update_by_query";
        //                                        //            var address2 = new Uri(url2);
        //                                        //            var response2 = await httpClient.PostAsync(address2, stringContent2);
        //                                        //        }
        //                                        //    }
        //                                        //}


        //                                    }
        //                                }
        //                                else
        //                                {

        //                                }

        //                            }

        //                        }
        //                    }

        //                }



        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        //public async Task<bool> ExecuteCubeJs(string id)
        //{
        //    var cubeJs = ApplicationConstant.AppSettings.CubeJsUrl(_configuration);
        //    var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
        //    var settings = new ConnectionSettings(new Uri(eldbUrl));
        //    var client = new ElasticClient(settings);
        //    BulkDescriptor descriptor = new BulkDescriptor();
        //    var filterList = new List<BuilderFilterViewModel>();
        //    bool isValid = true;
        //    var rule = await _noteBusiness.GetNotificationALertDetails(id);
        //    if (rule.IsNotNull() && rule.columnReferenceId.IsNotNullAndNotEmpty())
        //    {
        //        var content = @"{""measures"": [""#MEASURE#""],""dimensions"": #DIMENSION#,""timeDimensions"": [#TIMEDIMENSIONS#],""limit"":#LIMIT#,""order"": [[""Dial100Data.event_time"",""asc""]],""filters"": [" + rule.cubeJsFilter + "]}";                
        //        content = content.Replace("#MEASURE#", rule.queryTableId);
        //        content = content.Replace("#DIMENSION#", rule.queryColumns);
        //        content = content.Replace("#TIMEDIMENSIONS#", rule.timeDimensionFilter);
        //        content = content.Replace("#LIMIT#", rule.limit.IsNullOrEmpty() ? "10" : rule.limit);                
        //        var tempObj = JsonConvert.DeserializeObject(content);
        //        var str = JsonConvert.SerializeObject(tempObj);
        //        var handler = new HttpClientHandler();
        //        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        //        handler.ServerCertificateCustomValidationCallback =
        //            (httpRequestMessage, cert, cetChain, policyErrors) =>
        //            {
        //                return true;
        //            };

        //        using (var httpClient = new HttpClient(handler))
        //        {
        //            var url1 = $@"{cubeJs}cubejs-api/v1/load?query={str}";
        //            var address = new Uri(url1);
        //            var response = await httpClient.GetAsync(address);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var _jsondata = await response.Content.ReadAsStringAsync();
        //                var _dataToken = JToken.Parse(_jsondata);
        //                var _data = _dataToken.SelectToken("data");
        //                if (_data.Count() > 0 )
        //                {                           
        //                    var tableName = rule.queryTableId.Split('.')[0] + ".";
        //                    var jsonStr = _data.ToString().Replace(tableName, "");
        //                    var datalist = JsonConvert.DeserializeObject<List<Dial100ViewModel>>(jsonStr);
        //                    if(datalist.Count > rule.conditionValue.ToSafeInt())
        //                    {
        //                        if (rule.groupFilters != "[]")
        //                        {                                   
        //                            var filters = JsonConvert.DeserializeObject<dynamic>(rule.groupFilters);
        //                            foreach (var item in filters.rules)
        //                            {
        //                                var f = item["field"].Value;
        //                                var o = item["operator"].Value;
        //                                var v = item["value"].Value;
        //                                var filter = new BuilderFilterViewModel { Field = f.Replace(tableName, ""), Operator = o, Value = v };
        //                                filterList.Add(filter);
        //                            }
        //                        }
        //                        if (rule.groupbyColumns.IsNotNullAndNotEmpty())
        //                        {
        //                            var grpColArr = rule.groupbyColumns.Replace(tableName, "").Split(','); 
        //                            if (grpColArr.Length == 1)
        //                            {
        //                                var grpDatalist = datalist.GroupBy(x => x.GetType().GetProperty(grpColArr[0]).GetValue(x, null)).ToList();
        //                                foreach (var dataItem in grpDatalist)
        //                                {                                            
        //                                    var dataItemList = dataItem.ToList();
        //                                    foreach (var filter in filterList)
        //                                    {

        //                                        if (filter.Operator == "exist")
        //                                        {
        //                                            var distList = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Select(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).ToList();
        //                                            var isExist = distList.Where(x => x.ToString() == filter.Value).Any();
        //                                            if (isExist)
        //                                            {

        //                                                isValid = true;
        //                                            }
        //                                            else
        //                                            {
        //                                                isValid = false;
        //                                                break;
        //                                            }
        //                                        }
        //                                        else if (filter.Operator == "contains")
        //                                        {

        //                                        }
        //                                        else if (filter.Operator == "greater than")
        //                                        {
        //                                            var distCount = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
        //                                            if(distCount > filter.Value.ToSafeInt())
        //                                            {
        //                                                isValid = true;
        //                                            }
        //                                            else
        //                                            {
        //                                                isValid = false;
        //                                                break;
        //                                            }
        //                                        }
        //                                        else if (filter.Operator == "less than")
        //                                        {
        //                                            var distCount = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
        //                                            if (distCount < filter.Value.ToSafeInt())
        //                                            {
        //                                                isValid = true;
        //                                            }
        //                                            else
        //                                            {
        //                                                isValid = false;
        //                                                break;
        //                                            }

        //                                        }
        //                                    }
        //                                    if (isValid && dataItemList.Count() > rule.conditionValue.ToSafeInt())
        //                                    {                                                
        //                                        dynamic obj = new System.Dynamic.ExpandoObject();
        //                                        var list = dataItemList;
        //                                        var evnetTime = list.Last().event_time;
        //                                        if (evnetTime.IsNotNull())
        //                                        {
        //                                            var event_date = evnetTime.ToSafeDateTime();
        //                                            ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                        }
        //                                        foreach (var property in rule.GetType().GetProperties())
        //                                        {
        //                                            var _key = property.Name;
        //                                            var _value = property.GetValue(rule);
        //                                            if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                            {
        //                                                ExpandoAddProperty(obj, _key, _value);
        //                                            }

        //                                        }
        //                                        var ids = new List<string>();
        //                                        foreach (var item in list)
        //                                        {
        //                                            ids.Add(item.event_number);
        //                                        }
        //                                        if (ids.Count > 0)
        //                                        {
        //                                            var filIds = String.Join(",", ids);
        //                                            ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                        }
        //                                        var year = DateTime.Now.Year;
        //                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                        content3 = content3.Replace("#YEAR#", year.ToString());
        //                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                        var address3 = new Uri(url3);
        //                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                        if (response3.IsSuccessStatusCode)
        //                                        {
        //                                            var json3 = await response3.Content.ReadAsStringAsync();
        //                                            var data3 = JToken.Parse(json3);
        //                                            if (data3.IsNotNull())
        //                                            {
        //                                                var hits = data3.SelectToken("hits");
        //                                                if (hits.IsNotNull())
        //                                                {
        //                                                    var total = hits.SelectToken("total");
        //                                                    if (total.IsNotNull())
        //                                                    {
        //                                                        var tt = total.First();
        //                                                        var countstr = tt.Last().ToString();
        //                                                        var count3 = countstr.ToSafeInt();
        //                                                        var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                        ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        else if (response3.ReasonPhrase == "Not Found")
        //                                        {
        //                                            var count3 = 0;
        //                                            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                        }
        //                                        else
        //                                        {
        //                                            return false;
        //                                        }
        //                                        var alertId = Guid.NewGuid().ToString();
        //                                        ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                        ExpandoAddProperty(obj, "alertid", alertId);
        //                                        ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                        ExpandoAddProperty(obj, "isRead", false);
        //                                        ExpandoAddProperty(obj, "isVisible", true);
        //                                        descriptor.Index<object>(i => i
        //                                            .Index("iip_alert_data")
        //                                            .Id((Id)alertId)
        //                                            .Document(obj));

        //                                        var bulkResponse = client.Bulk(descriptor);
        //                                        if (bulkResponse.ApiCall.Success)
        //                                        {

        //                                            var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                            var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                            content2 = content2.Replace("#IDS#", filIds);
        //                                            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                            var url2 = eldbUrl + "dail100/_update_by_query";
        //                                            var address2 = new Uri(url2);
        //                                            var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {

        //                            }

        //                        }
        //                        else
        //                        { 
        //                            foreach (var filter in filterList)
        //                            {

        //                                if (filter.Operator == "exist")
        //                                {
        //                                    var distList = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Select(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).ToList();
        //                                    var isExist = distList.Where(x => x.ToString() == filter.Value).Any();
        //                                    if (isExist)
        //                                    {

        //                                        isValid = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        isValid = false;
        //                                        break;
        //                                    }
        //                                }
        //                                else if (filter.Operator == "contains")
        //                                {

        //                                }
        //                                else if (filter.Operator == "greater than")
        //                                {
        //                                    var distCount = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
        //                                    if (distCount > filter.Value.ToSafeInt())
        //                                    {
        //                                        isValid = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        isValid = false;
        //                                        break;
        //                                    }
        //                                }
        //                                else if (filter.Operator == "less than")
        //                                {
        //                                    var distCount = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
        //                                    if (distCount < filter.Value.ToSafeInt())
        //                                    {
        //                                        isValid = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        isValid = false;
        //                                        break;
        //                                    }

        //                                }
        //                            }
        //                            if (isValid)
        //                            {

        //                                while (datalist.Count() > 0)
        //                                {
        //                                    dynamic obj = new System.Dynamic.ExpandoObject();
        //                                    var list = datalist.Take(rule.conditionValue.ToSafeInt() + 1).ToList();
        //                                    datalist = datalist.Except(list).ToList();
        //                                    if (list.Count() > rule.conditionValue.ToSafeInt())
        //                                    {
        //                                        var evnetTime = list.Last().event_time;
        //                                        if (evnetTime.IsNotNull())
        //                                        {
        //                                            var event_date = evnetTime.ToSafeDateTime();
        //                                            ExpandoAddProperty(obj, "event_datetime", event_date);

        //                                        }
        //                                        foreach (var property in rule.GetType().GetProperties())
        //                                        {
        //                                            var _key = property.Name;
        //                                            var _value = property.GetValue(rule);
        //                                            if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
        //                                            {
        //                                                ExpandoAddProperty(obj, _key, _value);
        //                                            }

        //                                        }
        //                                        var ids = new List<string>();
        //                                        foreach (var item in list)
        //                                        {
        //                                            ids.Add(item.event_number);
        //                                        }
        //                                        //var ids = new List<string>();
        //                                        //foreach (var item in list)
        //                                        //{
        //                                        //    foreach (JProperty pr in item)
        //                                        //    {
        //                                        //        if (pr.Name == rule.columnReferenceId)
        //                                        //        {
        //                                        //            var ev = pr.First().Value<string>();
        //                                        //            if (ev.IsNotNull())
        //                                        //            {
        //                                        //                ids.Add(ev);
        //                                        //            }

        //                                        //        }

        //                                        //    }

        //                                        //}
        //                                        if (ids.Count > 0)
        //                                        {
        //                                            var filIds = String.Join(",", ids);
        //                                            ExpandoAddProperty(obj, "sourceIds", filIds);
        //                                        }
        //                                        var year = DateTime.Now.Year;
        //                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
        //                                        content3 = content3.Replace("#YEAR#", year.ToString());
        //                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
        //                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
        //                                        var address3 = new Uri(url3);
        //                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
        //                                        if (response3.IsSuccessStatusCode)
        //                                        {
        //                                            var json3 = await response3.Content.ReadAsStringAsync();
        //                                            var data3 = JToken.Parse(json3);
        //                                            if (data3.IsNotNull())
        //                                            {
        //                                                var hits = data3.SelectToken("hits");
        //                                                if (hits.IsNotNull())
        //                                                {
        //                                                    var total = hits.SelectToken("total");
        //                                                    if (total.IsNotNull())
        //                                                    {
        //                                                        var tt = total.First();
        //                                                        var countstr = tt.Last().ToString();
        //                                                        var count3 = countstr.ToSafeInt();
        //                                                        var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                                        ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        else if (response3.ReasonPhrase == "Not Found")
        //                                        {
        //                                            var count3 = 0;
        //                                            var alertNumber = year + "-" + (count3 + 1).ToString();
        //                                            ExpandoAddProperty(obj, "alert_number", alertNumber);
        //                                        }
        //                                        else
        //                                        {
        //                                            return false;
        //                                        }
        //                                        var alertId = Guid.NewGuid().ToString();
        //                                        ExpandoAddProperty(obj, "alert_date", DateTime.Now);
        //                                        ExpandoAddProperty(obj, "alertid", alertId);
        //                                        ExpandoAddProperty(obj, "isFalseEvent", false);
        //                                        ExpandoAddProperty(obj, "isRead", false);
        //                                        ExpandoAddProperty(obj, "isVisible", true);
        //                                        descriptor.Index<object>(i => i
        //                                            .Index("iip_alert_data")
        //                                            .Id((Id)alertId)
        //                                            .Document(obj));

        //                                        var bulkResponse = client.Bulk(descriptor);
        //                                        if (bulkResponse.ApiCall.Success)
        //                                        {

        //                                            var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
        //                                            var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
        //                                            content2 = content2.Replace("#IDS#", filIds);
        //                                            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //                                            var url2 = eldbUrl + "dail100/_update_by_query";
        //                                            var address2 = new Uri(url2);
        //                                            var response2 = await httpClient.PostAsync(address2, stringContent2);

        //                                        }
        //                                    }
        //                                }

        //                            }
        //                        }                                
        //                    }                            
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        public async Task<bool> ExecuteCubeJs(string id)
        {
            var cubeJs = ApplicationConstant.AppSettings.CubeJsUrl(_configuration);
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            BulkDescriptor descriptor = new BulkDescriptor();
            var filterList = new List<BuilderFilterViewModel>();
            bool isValid = true;
            var rule = await _noteBusiness.GetNotificationALertDetails(id);
            if (rule.IsNotNull() && rule.columnReferenceId.IsNotNullAndNotEmpty())
            {
                var content = @"{""measures"": [""#MEASURE#""],""dimensions"": #DIMENSION#,""timeDimensions"": [#TIMEDIMENSIONS#],""limit"":#LIMIT#,""order"": [[""Dial100Data.event_time"",""asc""]],""filters"": [" + rule.cubeJsFilter + "]}";
                content = content.Replace("#MEASURE#", rule.queryTableId);
                content = content.Replace("#DIMENSION#", rule.queryColumns);
                content = content.Replace("#TIMEDIMENSIONS#", rule.timeDimensionFilter);
                content = content.Replace("#LIMIT#", rule.limit.IsNullOrEmpty() ? "10" : rule.limit);
                var tempObj = JsonConvert.DeserializeObject(content);
                var str = JsonConvert.SerializeObject(tempObj);
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                using (var httpClient = new HttpClient(handler))
                {
                    var url1 = $@"{cubeJs}cubejs-api/v1/load?query={str}";
                    var address = new Uri(url1);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _data = _dataToken.SelectToken("data");
                        if (_data.Count() > 0)
                        {
                            var tableName = rule.queryTableId.Split('.')[0] + ".";
                            var jsonStr = _data.ToString().Replace(tableName, "");
                            var datalist = JsonConvert.DeserializeObject<List<Dial100ViewModel>>(jsonStr);
                            if (datalist.Count > rule.conditionValue.ToSafeInt())
                            {
                                if (rule.groupFilters != "[]")
                                {
                                    var filters = JsonConvert.DeserializeObject<dynamic>(rule.groupFilters);
                                    foreach (var item in filters.rules)
                                    {
                                        var f = item["field"].Value;
                                        var o = item["operator"].Value;
                                        var v = item["value"].Value;
                                        var filter = new BuilderFilterViewModel { Field = f.Replace(tableName, ""), Operator = o, Value = v };
                                        filterList.Add(filter);
                                    }
                                }
                                if (rule.groupbyColumns.IsNotNullAndNotEmpty())
                                {
                                    var grpColArr = rule.groupbyColumns.Replace(tableName, "").Split(',');
                                    if (grpColArr.Length == 1)
                                    {
                                        var grpDatalist = datalist.GroupBy(x => x.GetType().GetProperty(grpColArr[0]).GetValue(x, null)).ToList();
                                        var maxCount = 0;
                                        var year = DateTime.Now.Year;
                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
                                        content3 = content3.Replace("#YEAR#", year.ToString());
                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
                                        var address3 = new Uri(url3);
                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
                                        if (response3.IsSuccessStatusCode)
                                        {
                                            var json3 = await response3.Content.ReadAsStringAsync();
                                            var data3 = JToken.Parse(json3);
                                            if (data3.IsNotNull())
                                            {
                                                var hits = data3.SelectToken("hits");
                                                if (hits.IsNotNull())
                                                {
                                                    var total = hits.SelectToken("total");
                                                    if (total.IsNotNull())
                                                    {
                                                        var tt = total.First();
                                                        var countstr = tt.Last().ToString();
                                                        maxCount = countstr.ToSafeInt();
                                                    }
                                                }
                                            }
                                        }
                                        else if (response3.ReasonPhrase == "Not Found")
                                        {
                                            maxCount = 0;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                        foreach (var dataItem in grpDatalist)
                                        {
                                            var dataItemList = dataItem.ToList();
                                            foreach (var filter in filterList)
                                            {

                                                if (filter.Operator == "exist")
                                                {
                                                    var distList = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Select(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).ToList();
                                                    var isExist = distList.Where(x => x.ToString() == filter.Value).Any();
                                                    if (isExist)
                                                    {

                                                        isValid = true;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                        break;
                                                    }
                                                }
                                                else if (filter.Operator == "contains")
                                                {

                                                }
                                                else if (filter.Operator == "greater than")
                                                {
                                                    var distCount = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                                    if (distCount > filter.Value.ToSafeInt())
                                                    {
                                                        isValid = true;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                        break;
                                                    }
                                                }
                                                else if (filter.Operator == "less than")
                                                {
                                                    var distCount = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                                    if (distCount < filter.Value.ToSafeInt())
                                                    {
                                                        isValid = true;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                        break;
                                                    }

                                                }
                                            }
                                            if (isValid && dataItemList.Count() > rule.conditionValue.ToSafeInt())
                                            {
                                                dynamic obj = new System.Dynamic.ExpandoObject();
                                                var list = dataItemList;
                                                var evnetTime = list.Last().event_time;
                                                if (evnetTime.IsNotNull())
                                                {
                                                    var event_date = evnetTime.ToSafeDateTime();
                                                    ExpandoAddProperty(obj, "event_datetime", event_date);

                                                }
                                                foreach (var property in rule.GetType().GetProperties())
                                                {
                                                    var _key = property.Name;
                                                    var _value = property.GetValue(rule);
                                                    if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
                                                    {
                                                        ExpandoAddProperty(obj, _key, _value);
                                                    }

                                                }
                                                var ids = new List<string>();
                                                foreach (var item in list)
                                                {
                                                    ids.Add(item.event_number);
                                                }
                                                if (ids.Count > 0)
                                                {
                                                    var filIds = String.Join(",", ids);
                                                    ExpandoAddProperty(obj, "sourceIds", filIds);
                                                }
                                                var alertId = Guid.NewGuid().ToString();
                                                ExpandoAddProperty(obj, "alert_date_utc", DateTime.UtcNow);
                                                ExpandoAddProperty(obj, "alert_date", Convert.ToDateTime(DateTime.UtcNow.AddHours(5.5).ToString()));
                                                ExpandoAddProperty(obj, "alertid", alertId);
                                                ExpandoAddProperty(obj, "isFalseEvent", false);
                                                ExpandoAddProperty(obj, "isRead", false);
                                                ExpandoAddProperty(obj, "isVisible", true);
                                                var exCount = maxCount;
                                                maxCount = await CheckAlertNoExist(maxCount);
                                                if (exCount == maxCount)
                                                {
                                                    return false;
                                                }
                                                ExpandoAddProperty(obj, "year", year);
                                                ExpandoAddProperty(obj, "y_count", maxCount);
                                                ExpandoAddProperty(obj, "alert_number", (year + "-" + (maxCount)).ToString());
                                                descriptor.Index<object>(i => i
                                                    .Index("iip_alert_data")
                                                    .Id((Id)alertId)
                                                    .Document(obj));

                                                var bulkResponse = client.Bulk(descriptor);
                                                if (bulkResponse.ApiCall.Success)
                                                {

                                                    var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
                                                    var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
                                                    content2 = content2.Replace("#IDS#", filIds);
                                                    var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                                                    var url2 = eldbUrl + "dail100/_update_by_query";
                                                    var address2 = new Uri(url2);
                                                    var response2 = await httpClient.PostAsync(address2, stringContent2);

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {

                                    }

                                }
                                else
                                {
                                    foreach (var filter in filterList)
                                    {

                                        if (filter.Operator == "exist")
                                        {
                                            var distList = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Select(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).ToList();
                                            var isExist = distList.Where(x => x.ToString() == filter.Value).Any();
                                            if (isExist)
                                            {

                                                isValid = true;
                                            }
                                            else
                                            {
                                                isValid = false;
                                                break;
                                            }
                                        }
                                        else if (filter.Operator == "contains")
                                        {

                                        }
                                        else if (filter.Operator == "greater than")
                                        {
                                            var distCount = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                            if (distCount > filter.Value.ToSafeInt())
                                            {
                                                isValid = true;
                                            }
                                            else
                                            {
                                                isValid = false;
                                                break;
                                            }
                                        }
                                        else if (filter.Operator == "less than")
                                        {
                                            var distCount = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                            if (distCount < filter.Value.ToSafeInt())
                                            {
                                                isValid = true;
                                            }
                                            else
                                            {
                                                isValid = false;
                                                break;
                                            }

                                        }
                                    }
                                    if (isValid)
                                    {
                                        var maxCount = 0;
                                        var year = DateTime.Now.Year;
                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
                                        content3 = content3.Replace("#YEAR#", year.ToString());
                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
                                        var address3 = new Uri(url3);
                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
                                        if (response3.IsSuccessStatusCode)
                                        {
                                            var json3 = await response3.Content.ReadAsStringAsync();
                                            var data3 = JToken.Parse(json3);
                                            if (data3.IsNotNull())
                                            {
                                                var hits = data3.SelectToken("hits");
                                                if (hits.IsNotNull())
                                                {
                                                    var total = hits.SelectToken("total");
                                                    if (total.IsNotNull())
                                                    {
                                                        var tt = total.First();
                                                        var countstr = tt.Last().ToString();
                                                        maxCount = countstr.ToSafeInt();
                                                    }
                                                }
                                            }
                                        }
                                        else if (response3.ReasonPhrase == "Not Found")
                                        {
                                            maxCount = 0;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                        while (datalist.Count() > 0)
                                        {
                                            dynamic obj = new System.Dynamic.ExpandoObject();
                                            var list = datalist.Take(rule.conditionValue.ToSafeInt() + 1).ToList();
                                            datalist = datalist.Except(list).ToList();
                                            if (list.Count() > rule.conditionValue.ToSafeInt())
                                            {
                                                var evnetTime = list.Last().event_time;
                                                if (evnetTime.IsNotNull())
                                                {
                                                    var event_date = evnetTime.ToSafeDateTime();
                                                    ExpandoAddProperty(obj, "event_datetime", event_date);

                                                }
                                                foreach (var property in rule.GetType().GetProperties())
                                                {
                                                    var _key = property.Name;
                                                    var _value = property.GetValue(rule);
                                                    if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
                                                    {
                                                        ExpandoAddProperty(obj, _key, _value);
                                                    }

                                                }
                                                var ids = new List<string>();
                                                foreach (var item in list)
                                                {
                                                    ids.Add(item.event_number);
                                                }
                                                if (ids.Count > 0)
                                                {
                                                    var filIds = String.Join(",", ids);
                                                    ExpandoAddProperty(obj, "sourceIds", filIds);
                                                }
                                                var alertId = Guid.NewGuid().ToString();
                                                ExpandoAddProperty(obj, "alert_date_utc", DateTime.UtcNow);
                                                ExpandoAddProperty(obj, "alert_date", Convert.ToDateTime(DateTime.UtcNow.AddHours(5.5).ToString()));
                                                ExpandoAddProperty(obj, "alertid", alertId);
                                                ExpandoAddProperty(obj, "isFalseEvent", false);
                                                ExpandoAddProperty(obj, "isRead", false);
                                                ExpandoAddProperty(obj, "isVisible", true);
                                                var exCount = maxCount;
                                                maxCount = await CheckAlertNoExist(maxCount);
                                                if (exCount == maxCount)
                                                {
                                                    return false;
                                                }
                                                ExpandoAddProperty(obj, "year", year);
                                                ExpandoAddProperty(obj, "y_count", maxCount);
                                                ExpandoAddProperty(obj, "alert_number", (year + "-" + (maxCount)).ToString());
                                                descriptor.Index<object>(i => i
                                                    .Index("iip_alert_data")
                                                    .Id((Id)alertId)
                                                    .Document(obj));

                                                var bulkResponse = client.Bulk(descriptor);
                                                if (bulkResponse.ApiCall.Success)
                                                {

                                                    var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
                                                    var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
                                                    content2 = content2.Replace("#IDS#", filIds);
                                                    var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                                                    var url2 = eldbUrl + "dail100/_update_by_query";
                                                    var address2 = new Uri(url2);
                                                    var response2 = await httpClient.PostAsync(address2, stringContent2);

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }


                    }
                }
                return true;
            }
            return false;
        }
        private async Task<int> CheckAlertNoExist(int no)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var year = DateTime.Now.Year;
            var content = @"{""size"": 1000,""query"":{""bool"":{""must"":[{""simple_query_string"" : {""query"": ""#ALERTNO#"",""fields"": [""alert_number""],""default_operator"": ""and""}}]}}}";
            content = content.Replace("#ALERTNO#", (year + "-" + (no + 1)).ToString());
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var url = eldbUrl + "iip_alert_data/_search?pretty=true";
            var address = new Uri(url);
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var response = await httpClient.PostAsync(address, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var hits = data.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var total = hits.SelectToken("total");
                            if (total.IsNotNull())
                            {
                                var tt = total.First();
                                var countstr = tt.Last().ToString();
                                var cc = countstr.ToSafeInt();
                                if (cc > 0)
                                {
                                    return await CheckAlertNoExist(no + 1);
                                }
                                else
                                {
                                    return no + 1;
                                }
                            }
                        }
                    }
                }
                else if (response.ReasonPhrase == "Not Found")
                {
                    return no + 1;
                }
                else
                {
                    return no;
                }
            }
            return no;
        }
        public async Task ExecuteRssFeeds()
        {
            var cubeJs = ApplicationConstant.AppSettings.CubeJsUrl(_configuration);
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            BulkDescriptor descriptor = new BulkDescriptor();
            var feeds = await _noteBusiness.GetRssFeedData();
            foreach (var _feed in feeds)
            {
                var reader = XmlReader.Create(_feed.feedUrl);
                var feed = SyndicationFeed.Load(reader);
                foreach (var i in feed.Items)
                {
                    var _post = new NewsFeedsViewModel
                    {
                        title = i.Title.Text,
                        message = i.Summary.Text,
                        link = i.Links.Count > 0 ? i.Links[0].Uri.OriginalString : null,
                        published = i.PublishDate.DateTime,
                        author = i.Authors.Count > 0 ? i.Authors[0].Name : null,
                        name = _feed.NoteDescription,
                        //_index = _feed.feedName
                    };
                    var _id = i.Id.IsNotNullAndNotEmpty() ? i.Id : i.Links[0].Uri.OriginalString;
                    descriptor.Index<object>(z => z
                                  .Index("rssfeeds")
                                  .Id((Id)_id)
                                  .Document(_post));

                }
                var bulkResponse = client.Bulk(descriptor);
            }


        }
        public async Task<bool> GetGeoDistance()
        {

            var p1 = new GeoCoordinate(23.2644, 77.4347);
            var p2 = new GeoCoordinate(23.2532, 77.4356);
            return GetDistanceTo(p1, p2, 500);

        }
        private bool GetDistanceTo(GeoCoordinate center, GeoCoordinate other, int radius)
        {
            if (double.IsNaN(center.Latitude) || double.IsNaN(center.Longitude) || double.IsNaN(other.Latitude) ||
                double.IsNaN(other.Longitude))
            {

            }

            var d1 = center.Latitude * (Math.PI / 180.0);
            var num1 = center.Longitude * (Math.PI / 180.0);
            var d2 = other.Latitude * (Math.PI / 180.0);
            var num2 = other.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            var distanceInMeters = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            return distanceInMeters < radius;
        }
        //public async Task<JsonResult> ReadCCTVCameraData()
        //{
        //    var list = await _noteBusiness.GetIIPCamera();
        //    return Json(list);
        //}
        public async Task<JsonResult> ReadCCTVCameraData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<CctvCameraViewModel> list = new List<CctvCameraViewModel>();
            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "cctv_camera/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    if (trackdata.IsNotNull())
                    {
                        var hits = trackdata.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var _hits = hits.SelectToken("hits");

                            foreach (var hit in _hits)
                            {
                                var source = hit.SelectToken("_source");
                                //dynamic obj = new System.Dynamic.ExpandoObject();
                                //foreach (JProperty root in source)
                                //{

                                //    var key = root.Name;
                                //    var a = root.First().ToString();
                                //    ExpandoAddProperty(obj, key, a);
                                //}
                                var str = JsonConvert.SerializeObject(source);
                                var result = JsonConvert.DeserializeObject<CctvCameraViewModel>(str);
                                list.Add(result);
                            }
                        }
                    }

                }
            }
            else
            {
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCctvCameraDataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCctvCameraDataQuery3;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadCctvCameraDataQuery4;
                }
                content = content.Replace("#SEARCHWHERE#", searchStr);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "cctv_camera/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    if (trackdata.IsNotNull())
                    {
                        var hits = trackdata.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var total = hits.SelectToken("total");
                            var value = total.SelectToken("value");
                            var _hits = hits.SelectToken("hits");
                            foreach (var hit in _hits)
                            {
                                var source = hit.SelectToken("_source");
                                var highlight = hit.SelectToken("highlight");
                                var str = JsonConvert.SerializeObject(source);
                                var strhighlight = JsonConvert.SerializeObject(highlight);
                                var result = JsonConvert.DeserializeObject<CctvCameraViewModel>(str);
                                var resultHighlight = JsonConvert.DeserializeObject<CctvCameraArrayViewModel>(strhighlight);

                                if (result.IsNotNull())
                                {

                                    var sres = new CctvCameraViewModel
                                    {
                                        CameraName = resultHighlight.CameraName != null ? string.Join("", resultHighlight.CameraName) : string.Join("", result.CameraName),
                                        City = resultHighlight.City != null ? string.Join("", resultHighlight.City) : string.Join("", result.City),
                                        Location = resultHighlight.Location != null ? string.Join("", resultHighlight.Location) : string.Join("", result.Location),
                                        PoliceStation = resultHighlight.PoliceStation != null ? string.Join("", resultHighlight.PoliceStation) : string.Join("", result.PoliceStation),
                                        Longitude = resultHighlight.Longitude != null ? string.Join("", resultHighlight.Longitude) : string.Join("", result.Longitude),
                                        Latitude = resultHighlight.Latitude != null ? string.Join("", resultHighlight.Latitude) : string.Join("", result.Latitude),
                                        IpAddress = resultHighlight.IpAddress != null ? string.Join("", resultHighlight.IpAddress) : string.Join("", result.IpAddress),
                                        RtspLink = resultHighlight.RtspLink != null ? string.Join("", resultHighlight.RtspLink) : string.Join("", result.RtspLink),
                                        SwitchHostName = resultHighlight.SwitchHostName != null ? string.Join("", resultHighlight.SwitchHostName) : string.Join("", result.SwitchHostName),
                                        Make = resultHighlight.Make != null ? string.Join("", resultHighlight.Make) : string.Join("", result.Make),
                                        TypeOfCamera = resultHighlight.TypeOfCamera != null ? string.Join("", resultHighlight.TypeOfCamera) : string.Join("", result.TypeOfCamera),
                                        Id = result.Id,

                                    };
                                    list.Add(sres);

                                }


                            }
                        }
                    }

                }
            }
            return Json(list);
        }
        public async Task<IActionResult> SyncDataElasticDb()
        {
            return View();
        }
        public async Task<bool> PushCCTVCameraData()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            query = query.Replace("#FILTERCOLUMN#", "lastUpdatedDate");
            var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "cctv_camera/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, queryContent);
                if (response.IsSuccessStatusCode)
                {
                    var _jsondata = await response.Content.ReadAsStringAsync();
                    var _dataToken = JToken.Parse(_jsondata);
                    var _responsedata = _dataToken.SelectToken("aggregations");
                    var _maxdateToken = _responsedata.SelectToken("max_date");
                    var _dateToken = _maxdateToken.Last();
                    var _date = _dateToken.Last();
                    var lastUpdatedDate = _date.Value<DateTime>();
                    var list = await _noteBusiness.GetCctvCamera(lastUpdatedDate);
                    var settings = new ConnectionSettings(new Uri(eldbUrl));
                    var client = new ElasticClient(settings);
                    //var bulkIndexResponse = client.Bulk(b => b
                    //                        .Index("cctv_camera")                                            
                    //                        .IndexMany(list)
                    //                    );
                    BulkDescriptor descriptor = new BulkDescriptor();
                    foreach (var doc in list)
                    {
                        descriptor.Index<object>(i => i
                            .Index("cctv_camera")
                            .Id((Id)doc.Id)
                            .Document(doc));
                    }
                    var bulkResponse = client.Bulk(descriptor);
                    if (bulkResponse.ApiCall.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
                else
                {
                    var list = await _noteBusiness.GetCctvCamera();
                    var settings = new ConnectionSettings(new Uri(eldbUrl));
                    var client = new ElasticClient(settings);
                    //var bulkIndexResponse = client.Bulk(b => b
                    //                        .Index("cctv_camera")
                    //                        .IndexMany(list)
                    //                    );
                    BulkDescriptor descriptor = new BulkDescriptor();
                    foreach (var doc in list)
                    {
                        descriptor.Index<object>(i => i
                            .Index("cctv_camera")
                            .Id((Id)doc.Id)
                            .Document(doc));
                    }
                    var bulkResponse = client.Bulk(descriptor);
                    if (bulkResponse.ApiCall.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }


            }

        }
        public async Task<IActionResult> ReadYoutubeDataBySerach(string policeStation, string latitude, string longitude)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var list = new List<Youtube1ViewModel>();
            var url = "https://youtube.googleapis.com/youtube/v3/search?part=snippet,id&location=" + latitude + "," + longitude + "&locationRadius=50km&maxResults=10&q=" + policeStation + "&type=video%2Clist&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var items = json.SelectToken("items");
                    BulkDescriptor descriptor = new BulkDescriptor();
                    foreach (var item in items)
                    {
                        var source = item.SelectToken("id");
                        var id = string.Empty;
                        if (source.IsNotNull())
                        {
                            var _id = source.SelectToken("videoId");
                            id = _id.Value<string>();
                        }
                        var _snippet = item.SelectToken("snippet");
                        var dataStr = JsonConvert.SerializeObject(_snippet);
                        var model = JsonConvert.DeserializeObject<Youtube1ViewModel>(dataStr);
                        if (model.IsNotNull())
                        {
                            model.Id = id;
                            model.keyword = policeStation;
                            list.Add(model);
                            var indexName = "youtube_post";
                            descriptor.Index<object>(i => i
                                    .Index(indexName)
                                    .Id((Id)model.Id)
                                    .Document(model));
                        }

                    }
                    var bulkResponse = client.Bulk(descriptor);
                }

            }

            return Json(list);
        }
        public async Task<IActionResult> YoutubeComment(string videoId)
        {
            ViewBag.VideoId = videoId;
            return View();
        }
        public async Task<IActionResult> ReadYoutubeCommentDataByVideoId(string videoId)
        {
            var list = new List<YoutubeCommentViewModel>();
            var url = "https://youtube.googleapis.com/youtube/v3/commentThreads?part=snippet&maxResults=10&videoId=" + videoId + "&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var items = json.SelectToken("items");
                    foreach (var item in items)
                    {
                        var source = item.SelectToken("id");
                        var id = string.Empty;
                        if (source.IsNotNull())
                        {
                            id = source.Value<string>();
                        }
                        var _snippet = item.SelectToken("snippet");
                        if (_snippet.IsNotNull())
                        {
                            var _topLevelComment = _snippet.SelectToken("topLevelComment");
                            if (_topLevelComment.IsNotNull())
                            {
                                var _mainsnippet = _topLevelComment.SelectToken("snippet");
                                if (_mainsnippet.IsNotNull())
                                {
                                    var dataStr = JsonConvert.SerializeObject(_mainsnippet);
                                    var model = JsonConvert.DeserializeObject<YoutubeCommentViewModel>(dataStr);
                                    if (model.IsNotNull())
                                    {
                                        model.Id = id;
                                        list.Add(model);
                                    }
                                }
                            }
                        }

                    }
                }


            }

            return Json(list);
        }
        public async Task<IActionResult> ReadTwitterDataBySerach(string policeStation)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var list = new List<Twitter1ViewModel>();
            //var url = "https://api.twitter.com/2/tweets/search/recent?query="+ policeStation;
            //var url = "https://api.twitter.com/2/tweets/search/recent?query=" + policeStation + "&max_results=10&expansions=attachments.poll_ids%2Cattachments.media_keys%2Cauthor_id%2Centities.mentions.username%2Cgeo.place_id%2Cin_reply_to_user_id%2Creferenced_tweets.id%2Creferenced_tweets.id.author_id&media.fields=duration_ms%2Cheight%2Cmedia_key%2Cpreview_image_url%2Ctype%2Curl%2Cwidth%2Cpublic_metrics%2Calt_text&place.fields=contained_within%2Ccountry%2Ccountry_code%2Cfull_name%2Cgeo%2Cid%2Cname%2Cplace_type&poll.fields=duration_minutes%2Cend_datetime%2Cid%2Coptions%2Cvoting_status&tweet.fields=attachments%2Cauthor_id%2Ccontext_annotations%2Cconversation_id%2Ccreated_at%2Centities%2Cgeo%2Cid%2Cin_reply_to_user_id%2Clang%2Cpublic_metrics%2Cpossibly_sensitive%2Creferenced_tweets%2Creply_settings%2Csource%2Ctext%2Cwithheld&user.fields=created_at%2Cdescription%2Centities%2Cid%2Clocation%2Cname%2Cpinned_tweet_id%2Cprofile_image_url%2Cprotected%2Cpublic_metrics%2Curl%2Cusername%2Cverified%2Cwithheld";
            var url = socialApiUrl + "tweet_and_sentiment?keyword=" + policeStation;
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAALGQPQEAAAAAO%2F6Bh8iW9lNBaTmsAtY%2BsPYUgjc%3DZVbyg27R3pokVj1OXr9wV7uorkzJRdE3qtF7mKHc8K3whRR8jl");
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var data = json.SelectToken("data");
                    if (data.IsNotNull())
                    {
                        var items = data.SelectToken("data");
                        if (items.IsNotNull())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {


                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<Twitter1ViewModel>(dataStr);
                                if (model.IsNotNull())
                                {
                                    var _referenced_tweets = item.SelectToken("referenced_tweets");
                                    if (_referenced_tweets.IsNotNull() && _referenced_tweets.Count() > 0)
                                    {
                                        _referenced_tweets = _referenced_tweets.First();
                                        var referenced_tweets = JsonConvert.SerializeObject(_referenced_tweets);
                                        var referenced_tweets_model = JsonConvert.DeserializeObject<referenced_tweets>(referenced_tweets);
                                        if (referenced_tweets_model.IsNotNull())
                                        {
                                            model.type = referenced_tweets_model.type;
                                        }
                                    }
                                    var _public_metrics = item.SelectToken("public_metrics");
                                    var public_metrics = JsonConvert.SerializeObject(_public_metrics);
                                    var public_metricss_model = JsonConvert.DeserializeObject<public_metrics>(public_metrics);
                                    if (public_metricss_model.IsNotNull())
                                    {
                                        model.retweet_count = public_metricss_model.retweet_count;
                                        model.reply_count = public_metricss_model.reply_count;
                                        model.like_count = public_metricss_model.like_count;
                                        model.quote_count = public_metricss_model.quote_count;
                                    }
                                    var _polarity = item.SelectToken("polarity");
                                    var polarity = JsonConvert.SerializeObject(_polarity);
                                    var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                    if (polarity_model.IsNotNull())
                                    {
                                        model.pos = polarity_model.pos;
                                        model.neg = polarity_model.neg;
                                        model.neu = polarity_model.neu;
                                        model.compound = polarity_model.compound;
                                    }
                                    model.keyword = policeStation;
                                    list.Add(model);
                                    var indexName = "twitter_post";
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)model.id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }
                    }

                }



            }

            return Json(list);
        }
        public async Task<IActionResult> ReadFacebookDataBySerach(string policeStation)
        {
            var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var list = new List<FacebookPostViewModel>();
            var credential = await _noteBusiness.GetFacebookCredential();
            //var username = "8827426847";
            //var password = "Bibeksingh@23";            
            var url = socialApiUrl + "facebook_keyword_login?keyword=" + policeStation.Trim() + "&no_of_pages=1&username=" + credential.Name + "&password=" + credential.Code;
            //var url = socialApiUrl + "facebook_keyword?keyword=" + policeStation.Replace(" ", "") + "&pages=1";
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var data = json.SelectToken("data");
                    if (data.IsNotNull())
                    {
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var item in data)
                        {
                            var dataStr = JsonConvert.SerializeObject(item);
                            var model = JsonConvert.DeserializeObject<FacebookPostViewModel>(dataStr);
                            if (model.IsNotNull())
                            {
                                model.keyword = policeStation;
                                list.Add(model);
                                var id = model.post_url;
                                var indexName = "facebook_post";
                                descriptor.Index<object>(i => i
                                        .Index(indexName)
                                        .Id((Id)id)
                                        .Document(model));
                            }

                        }
                        var bulkResponse = client.Bulk(descriptor);
                    }

                }



            }

            return Json(list);
        }
        public async Task<IActionResult> ReadInstagramDataBySerach(string policeStation)
        {
            var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var list = new List<InstagramPostViewModel>();
            var url = socialApiUrl + "gsearch_keyword?keyword=instagram " + policeStation.Replace(" ", "") + "&no_of_pages=1";
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var data = json.SelectToken("data");
                    if (data.IsNotNull())
                    {
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var item in data)
                        {

                            var dataStr = JsonConvert.SerializeObject(item);
                            var link = JsonConvert.DeserializeObject<string>(dataStr);
                            if (link.IsNotNull())
                            {
                                var model = new InstagramPostViewModel();
                                var id = link;
                                model.url = link;
                                model.created_date = DateTime.Now;
                                model.keyword = policeStation;
                                var indexName = "instagram_post";
                                descriptor.Index<object>(i => i
                                        .Index(indexName)
                                        .Id((Id)id)
                                        .Document(model));
                                list.Add(model);
                            }

                        }
                        var bulkResponse = client.Bulk(descriptor);

                    }
                }



            }

            return Json(list);
        }
        public async Task<IActionResult> ReadNewsFeedDataBySearch(string policeStation)
        {
            var list = new List<NewsFeedsViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "rssfeeds/_search?pretty=true";
            if (policeStation.IsNotNullAndNotEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadTimesOfIndiaNewsFeedDataQuery3;
                content = content.Replace("#SEARCHWHERE#", policeStation);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    //var response = await httpClient.GetAsync(address);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();


                    var data = JToken.Parse(json);
                    var data1 = data.SelectToken("hits");
                    //var total = data1.SelectToken("total");
                    //var value = total.SelectToken("value");
                    if (data1.IsNotNull())
                    {
                        var data2 = data1.SelectToken("hits");
                        foreach (var item in data2)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<NewsFeedsViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }



                }
            }
            return Json(list);
        }
        public async Task<IActionResult> ROIPaudio(string fileName)
        {
            try
            {
                var newpath = @"/tmp/roip-server/calls/" + fileName;
                //string newpath = System.IO.Path.Combine(path, fileName);
                FileInfo myFile = new FileInfo(newpath);
                bool exists = myFile.Exists;
                if (exists)
                {
                    //FileStream fs = myFile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);                    
                    //byte[] AsBytes = new byte[fs.Length];
                    byte[] AsBytes = System.IO.File.ReadAllBytes(newpath);
                    var AsBase64String = Convert.ToBase64String(AsBytes);
                    ViewBag.Base64 = AsBase64String;
                }
                else
                {
                    ViewBag.Error = "File not found!";
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                Console.WriteLine(e.ToString());
            }


            return View();
        }
        public async Task<JsonResult> GetRoipChanel()
        {

            var chanelList = await _iipBusiness.GetAllRoipChannel();
            return Json(chanelList.OrderBy(x => x.Code));

        }
        public async Task<JsonResult> ReadRoipData(DateTime date, string disCode, double from, double to)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<RoipViewModel> list = new List<RoipViewModel>();
            var chanelList = await _iipBusiness.GetAllRoipChannel();
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "roip_data/_search?pretty=true";
                var address = new Uri(url);
                var content = ApplicationConstant.BusinessAnalytics.ReadRoipDataQuery1;
                content = content.Replace("#STARTDATE#", date.ToString("yyyy-MM-dd HH:mm:ss"));
                content = content.Replace("#ENDDATE#", date.ToString("yyyy-MM-dd HH:mm:ss"));
                content = content.Replace("#STARTTIME#", date.AddMinutes(from).ToString("yyyy-MM-dd HH:mm:ss"));
                content = content.Replace("#ENDTIME#", date.AddMinutes(to).ToString("yyyy-MM-dd HH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<RoipViewModel>(str);
                            if (result.IsNotNull())
                            {
                                var chanelData = chanelList.Where(x => x.Code == result.channel_no).FirstOrDefault();
                                if (chanelData.IsNotNull())
                                {
                                    result.channel_name = chanelData.Name;
                                }

                            }
                            list.Add(result);
                        }
                    }
                }
            }
            if (list.Count > 0 && disCode.IsNotNullAndNotEmpty())
            {
                var newlist = list.Where(x => x.channel_no == disCode).ToList();
                return Json(newlist.OrderBy(x => x.start_datetime));
            }
            return Json(list.OrderBy(x => x.start_datetime));
        }
        public async Task<JsonResult> GetAllIIPDataSourceCount(string watchlistId, string keyword)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var dial100Count = "0";
            var rssFeedsCount = "0";
            var twitterCount = "0";
            var youtubeCount = "0";
            var facebookCount = "0";
            var cctnsCount = "0";
            if (watchlistId.IsNotNullAndNotEmpty())
            {
                var watchlist = await _noteBusiness.GetWatchlistDetails(watchlistId);
                var dateRange = GetStartEndDateByFilterType(watchlist.dateFilterType, watchlist.startDate, watchlist.endDate);
                if (true)//Dial100
                {
                    var content = "";
                    if (watchlist.plainSearch && !watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadDial100CountDataQuery1;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadDial100CountDataQuery2;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                        }
                    }
                    else if (!watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadDial100CountDataQuery3;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadDial100CountDataQuery4;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }
                    else
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadDial100CountDataQuery5;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadDial100CountDataQuery6;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }

                    }
                    content = content.Replace("#SEARCHWHERE#", keyword.Replace("^", " "));
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var handler = new HttpClientHandler();
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };
                    using (var httpClient = new HttpClient(handler))
                    {
                        var url = eldbUrl + "dail100/_count?pretty=true";
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var jsontrack = await response.Content.ReadAsStringAsync();
                        var trackdata = JToken.Parse(jsontrack);
                        if (trackdata.IsNotNull())
                        {
                            var count = trackdata.SelectToken("count");
                            if (count.IsNotNull())
                            {
                                dial100Count = count.ToString();
                            }
                        }

                    }
                }
                if (true)//RssFeed
                {
                    var content = "";
                    if (watchlist.plainSearch)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadRssFeedCountDataQuery1;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadRssFeedCountDataQuery2;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                        }
                    }
                    else if (!watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadRssFeedCountDataQuery3;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadRssFeedCountDataQuery4;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadRssFeedCountDataQuery5;
                    }
                    content = content.Replace("#SEARCHWHERE#", keyword);
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var url = eldbUrl + "rssfeeds/_count?pretty=true";
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JToken.Parse(json);
                        var count = data.SelectToken("count");
                        rssFeedsCount = count.ToString();
                    }
                }
                if (true)//twitter
                {
                    var content = "";
                    if (watchlist.plainSearch)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadTwitterCountDataQuery1;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadTwitterCountDataQuery2;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                        }
                    }
                    else if (!watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadTwitterCountDataQuery3;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadTwitterCountDataQuery4;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadTwitterCountDataQuery5;
                    }
                    content = content.Replace("#SEARCHWHERE#", keyword);
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var url = eldbUrl + "twitter_post/_count?pretty=true";
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JToken.Parse(json);
                        var count = data.SelectToken("count");
                        twitterCount = count.ToString();
                    }
                }
                if (true)//youtube
                {

                    var content = "";
                    if (watchlist.plainSearch)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadYoutubeCountDataQuery1;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadYoutubeCountDataQuery2;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                        }
                    }
                    else if (!watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadYoutubeCountDataQuery3;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadYoutubeCountDataQuery4;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadYoutubeCountDataQuery5;
                    }
                    content = content.Replace("#SEARCHWHERE#", keyword);
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var url = eldbUrl + "youtube_post/_count?pretty=true";
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JToken.Parse(json);
                        var count = data.SelectToken("count");
                        youtubeCount = count.ToString();
                    }
                }
                if (true)//Facebook
                {

                    var content = "";
                    if (watchlist.plainSearch)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadFacebookCountDataQuery1;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadFacebookCountDataQuery2;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                        }
                    }
                    else if (!watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadFacebookCountDataQuery3;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadFacebookCountDataQuery4;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }
                    else
                    {
                        content = ApplicationConstant.BusinessAnalytics.ReadFacebookCountDataQuery5;
                    }
                    content = content.Replace("#SEARCHWHERE#", keyword);
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var url = eldbUrl + "facebook_post/_count?pretty=true";
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JToken.Parse(json);
                        var count = data.SelectToken("count");
                        facebookCount = count.ToString();
                    }
                }
                if (true)
                {
                    var content = "";
                    if (watchlist.plainSearch && !watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadCctnsCountDataQuery1;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadCctnsCountDataQuery4;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"));

                        }
                    }
                    else if (!watchlist.isAdvance)
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadCctnsCountDataQuery2;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadCctnsCountDataQuery5;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }
                    else
                    {
                        if (watchlist.dateFilterType == SocialMediaDatefilters.AllTime)
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadCctnsCountDataQuery3;
                        }
                        else
                        {
                            content = ApplicationConstant.BusinessAnalytics.ReadCctnsCountDataQuery6;
                            content = content.Replace("#STARTDATE#", dateRange.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                            content = content.Replace("#ENDDATE#", dateRange.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }

                    }
                    content = content.Replace("#SEARCHWHERE#", keyword.Replace("^", " "));
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var handler = new HttpClientHandler();
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };
                    using (var httpClient = new HttpClient(handler))
                    {
                        var url = eldbUrl + "cctns_common/_count?pretty=true";
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var jsontrack = await response.Content.ReadAsStringAsync();
                        var trackdata = JToken.Parse(jsontrack);
                        if (trackdata.IsNotNull())
                        {
                            var count = trackdata.SelectToken("count");
                            if (count.IsNotNull())
                            {
                                cctnsCount = count.ToString();
                            }
                        }

                    }
                }

            }
            return Json(new { success = true, dial100 = dial100Count, rssFeed = rssFeedsCount, youtube = youtubeCount, twitter = twitterCount, facebook = facebookCount, cctns = cctnsCount });
        }
        public async Task<JsonResult> UpdateAlertVisibility(string id)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {

                var content = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[""#IDS#""]}}]}},""script"": { ""source"": ""ctx._source['isVisible'] =false""} }";
                content = content.Replace("#IDS#", id);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var url = eldbUrl + "iip_alert_data/_update_by_query";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                return Json(new
                {
                    success = false
                });
            }
        }
        public async Task<IActionResult> IipReports()
        {
            return View();
        }
        public async Task<JsonResult> GenerateIipReports(string chartTypeId, string chartName, string tableId, string columnIds, string filters, string timeDimenstionId, string timeDimensionDuration, string granularity)
        {
            columnIds = columnIds.Replace(",", "\",\"");
            var finalResult = "measures: [\"" + tableId + "\"],dimensions: [\"" + columnIds + "\"],";
            var chart = await _noteBusiness.GetChartTemplateById(chartTypeId);
            var cubejsquery = "";
            if (filters.IsNotNullAndNotEmpty())
            {
                var qr = JsonConvert.DeserializeObject<dynamic>(filters);
                var con = qr.condition;
                var op = con.Value;
                var rules = qr.rules;
                cubejsquery = "{";
                cubejsquery = await ReadJson(rules, op, cubejsquery);
                cubejsquery += "}";
                finalResult += "filters: [" + cubejsquery + "],";
            }
            else
            {
                finalResult += "filters: [],";
            }
            var timeQuery = "";
            if (timeDimenstionId.IsNotNullAndNotEmpty())
            {
                if (timeDimensionDuration == "AllTime")
                {
                    timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#""}";

                }
                else
                {
                    timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";

                }
                timeQuery = timeQuery.Replace("#COLUMNNAME#", timeDimenstionId);
                timeQuery = timeQuery.Replace("#GRANULARITY#", granularity);
                timeQuery = timeQuery.Replace("#DURATION#", timeDimensionDuration);
                finalResult += "timeDimensions: [" + timeQuery + "]";
            }
            else
            {
                finalResult += "timeDimensions: []";
            }
            var chartBPCode = chart.Code.Replace("@@input@@", finalResult.Replace("\"\"", "\""));
            chartBPCode = chartBPCode.Replace("@@chartid@@", "'" + chartName + "'");
            chartBPCode = chartBPCode.Replace("@@ctx@@", "'2d'");
            return Json(new
            {
                success = true,
                charts = chartBPCode
            });
        }
        [HttpPost]
        public async Task<IActionResult> SaveIipReports(string chartTypeId, string reportName, string tableId, string columnIds, string filters, string timeDimenstionId, string timeDimensionDuration, string granularity)
        {
            dynamic model = new System.Dynamic.ExpandoObject();
            if (reportName.IsNullOrEmpty())
            {
                return Json(new { success = false, error = "Please Enter Report Name !" });
            }
            if (chartTypeId.IsNullOrEmpty())
            {
                return Json(new { success = false, error = "Please Select Chart Type !" });
            }
            if (tableId.IsNullOrEmpty())
            {
                return Json(new { success = false, error = "Please Select Table !" });
            }
            if (columnIds.IsNullOrEmpty())
            {
                return Json(new { success = false, error = "Please Select Columns !" });
            }
            model.chartTypeId = chartTypeId;
            model.tableId = tableId;
            model.columnIds = columnIds;
            model.filters = filters;
            model.timeDimenstionId = timeDimenstionId;
            model.timeDimensionDuration = timeDimensionDuration;
            model.granularity = granularity;
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = _userContext.UserId;
            templateModel.DataAction = DataActionEnum.Create;
            templateModel.TemplateCode = "IIP_ADVANCE_REPORT";
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.NoteSubject = reportName;
            newmodel.NoteDescription = reportName;
            newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(newmodel);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Something went wrong!" });

        }
        public async Task<bool> PushCCTNSApiDistrictDataToElasticDB()
        {
            var districtCode = "";
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
            foreach (var api in apilist.Where(x => x.NoteSubject == "dail100"))
            {

                query = query.Replace("#FILTERCOLUMN#", api.FilterColumn);
                var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + api.NoteSubject + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, queryContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var log = await _noteBusiness.GetSchedulerLog(api.NoteSubject, districtCode);
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _responsedata = _dataToken.SelectToken("aggregations");
                        var _maxdateToken = _responsedata.SelectToken("max_date");
                        var _dateToken = _maxdateToken.Last();
                        var _date = _dateToken.Last();
                        var fromDate = _date.Value<DateTime>();
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now) ? DateTime.Now : api.ToDate) : ((toDate > DateTime.Now) ? DateTime.Now : toDate);
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        var url1 = api.Url.Trim('1');
                        var address1 = new Uri(url1);
                        if (url1.ToLower().Contains(api.NoteSubject))
                        {
                            content1 = content1.Replace("#FROM_DATE#", !log.success ? log.fromDate : fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#To_DATE#", !log.success ? log.toDate : batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#DISTRICT_CODE#", districtCode);
                            var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                            Console.WriteLine(content1);
                            var response1 = await httpClient.PostAsync(address1, stringContent1);
                            if (response1.IsSuccessStatusCode)
                            {
                                var json = await response1.Content.ReadAsStringAsync();
                                Console.WriteLine(json);
                                var dataToken = JToken.Parse(json);
                                var responsedata = dataToken.SelectToken(api.ResponseToken);
                                Console.WriteLine(responsedata);
                                if (responsedata.IsNotNull())
                                {
                                    var objects = JArray.Parse(responsedata.ToString());
                                    BulkDescriptor descriptor = new BulkDescriptor();
                                    foreach (JObject root in objects)
                                    {
                                        dynamic obj = new System.Dynamic.ExpandoObject();
                                        var id = string.Empty;
                                        foreach (KeyValuePair<String, JToken> app in root)
                                        {
                                            var key = app.Key;
                                            var value = app.Value;

                                            if (key == api.FilterColumn)
                                            {
                                                var a = value.Value<string>();
                                                DateTime dt = DateTime.Parse(a, null);
                                                //DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                                ExpandoAddProperty(obj, key, dt);
                                            }
                                            else
                                            {
                                                var a = value.Value<string>();
                                                ExpandoAddProperty(obj, key, a);
                                            }
                                            if (key == api.IdColumn)
                                            {
                                                id = value.Value<string>();
                                            }


                                        }
                                        descriptor.Index<object>(i => i
                                            .Index(api.NoteSubject)
                                            .Id((Id)id)
                                            .Document(obj));
                                    }
                                    var bulkResponse = client.Bulk(descriptor);
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = bulkResponse.ToString(),
                                        error = bulkResponse.Errors.ToString(),
                                        success = true
                                    });
                                    return true;
                                }
                                else
                                {
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = "[]",
                                        error = "",
                                        success = true
                                    });
                                    return false;
                                }

                            }
                            else
                            {
                                await this.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    districtCode = districtCode,
                                    response = await response1.Content.ReadAsStringAsync(),
                                    error = response1.ReasonPhrase.ToString(),
                                    success = false
                                });
                                return true;
                            }
                        }
                        else
                        {
                            content1 = content1.Replace("#FROM_DATE#", "2021-05-01 00:00:00" /*!log.success ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss")*/);
                            content1 = content1.Replace("#To_DATE#", "2021-05-02 00:00:00"/*!log.success ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss")*/);
                            var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", api.ApiAuthorization);
                            var request = new HttpRequestMessage
                            {
                                Method = HttpMethod.Get,
                                RequestUri = address1,
                                Content = stringContent1,
                            };
                            var response1 = await httpClient.SendAsync(request);
                            if (response1.IsSuccessStatusCode)
                            {
                                var json = await response1.Content.ReadAsStringAsync();
                                var dataToken = JToken.Parse(json);
                                var responsedata = json != "[]" ? dataToken.First().SelectToken(api.ResponseToken) : null;
                                if (responsedata.IsNotNull())
                                {
                                    var objects = JArray.Parse(responsedata.ToString());
                                    BulkDescriptor descriptor = new BulkDescriptor();
                                    foreach (JObject root in objects)
                                    {
                                        dynamic obj = new System.Dynamic.ExpandoObject();
                                        var id = string.Empty;
                                        foreach (KeyValuePair<String, JToken> app in root)
                                        {
                                            var key = app.Key;
                                            var value = app.Value;

                                            if (key == api.FilterColumn)
                                            {
                                                var a = value.Value<string>();
                                                DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                                ExpandoAddProperty(obj, key, dt);
                                            }
                                            else
                                            {
                                                var a = value.Value<string>();
                                                ExpandoAddProperty(obj, key, a);
                                            }
                                            if (key == api.IdColumn)
                                            {
                                                id = value.Value<string>();
                                            }


                                        }
                                        descriptor.Index<object>(i => i
                                            .Index(api.NoteSubject)
                                            .Id((Id)id)
                                            .Document(obj));
                                    }
                                    var bulkResponse = client.Bulk(descriptor);
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = !log.success ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        toDate = !log.success ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        districtCode = districtCode,
                                        response = bulkResponse.ToString(),
                                        error = bulkResponse.Errors.ToString(),
                                        success = true
                                    });
                                    return true;
                                }
                                else
                                {
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = !log.success ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        toDate = !log.success ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        districtCode = districtCode,
                                        response = "[]",
                                        error = "",
                                        success = true
                                    });
                                    return false;
                                }

                            }
                            else
                            {
                                await this.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = !log.success ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    toDate = !log.success ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    districtCode = districtCode,
                                    response = await response1.Content.ReadAsStringAsync(),
                                    error = response1.ReasonPhrase.ToString(),
                                    success = false
                                });
                                return true;
                            }
                        }

                    }
                    else
                    {
                        var fromDate = api.FromDate;
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now) ? DateTime.Now : api.ToDate) : ((toDate > DateTime.Now) ? DateTime.Now : toDate);
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        var url1 = api.Url;
                        var address1 = new Uri(url1);
                        if (url1.ToLower().Contains(api.NoteSubject))
                        {
                            content1 = content1.Replace("#FROM_DATE#", fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#To_DATE#", batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#DISTRICT_CODE#", districtCode);
                            var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                            Console.WriteLine(content1);
                            var response1 = await httpClient.PostAsync(address1, stringContent1);
                            if (response1.IsSuccessStatusCode)
                            {
                                var json = await response1.Content.ReadAsStringAsync();
                                Console.WriteLine(json);
                                var dataToken = JToken.Parse(json);
                                var responsedata = dataToken.SelectToken(api.ResponseToken);
                                Console.WriteLine(responsedata);
                                if (responsedata.IsNotNull() && !json.Contains("Not Data Found!"))
                                {
                                    var objects = JArray.Parse(responsedata.ToString());
                                    BulkDescriptor descriptor = new BulkDescriptor();
                                    foreach (JObject root in objects)
                                    {
                                        dynamic obj = new System.Dynamic.ExpandoObject();
                                        var id = string.Empty;
                                        foreach (KeyValuePair<String, JToken> app in root)
                                        {
                                            var key = app.Key;
                                            var value = app.Value;

                                            if (key == api.FilterColumn)
                                            {
                                                var a = value.Value<string>();
                                                //var b = "3/16/2017 12:00:00 AM";
                                                //string d = Convert.ToDateTime(a).ToString("dd/MM/yyyy"); //returns 25/09/2011
                                                DateTime dt = DateTime.Parse(a, null);
                                                //string v = dtb.ToString("dd/MM/yyyy");
                                                //DateTime dt = DateTime.ParseExact(d, api.DateFormat, CultureInfo.InvariantCulture);
                                                ExpandoAddProperty(obj, key, dt);
                                            }
                                            else
                                            {
                                                var a = value.Value<string>();
                                                ExpandoAddProperty(obj, key, a);
                                            }
                                            if (key == api.IdColumn)
                                            {
                                                id = value.Value<string>();
                                            }


                                        }
                                        descriptor.Index<object>(i => i
                                            .Index(api.NoteSubject)
                                            .Id((Id)id)
                                            .Document(obj));
                                    }
                                    var bulkResponse = client.Bulk(descriptor);
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = bulkResponse.ToString(),
                                        error = bulkResponse.Errors.ToString(),
                                        success = true
                                    });
                                    return true;
                                }
                                else
                                {
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = "[]",
                                        error = "",
                                        success = true
                                    });
                                    return false;
                                }

                            }
                            else
                            {
                                await this.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    districtCode = districtCode,
                                    response = await response1.Content.ReadAsStringAsync(),
                                    error = response1.ReasonPhrase.ToString(),
                                    success = false
                                });
                                return true;
                            }
                        }
                        else
                        {
                            content1 = content1.Replace("#FROM_DATE#", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            content1 = content1.Replace("#To_DATE#", batchToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", api.ApiAuthorization);
                            var request = new HttpRequestMessage
                            {
                                Method = HttpMethod.Get,
                                RequestUri = address1,
                                Content = stringContent1,
                            };
                            var response1 = await httpClient.SendAsync(request);
                            if (response1.IsSuccessStatusCode)
                            {
                                var json = await response1.Content.ReadAsStringAsync();
                                var dataToken = JToken.Parse(json);
                                var responsedata = dataToken.First().SelectToken(api.ResponseToken);
                                if (responsedata.IsNotNull())
                                {
                                    var objects = JArray.Parse(responsedata.ToString());
                                    BulkDescriptor descriptor = new BulkDescriptor();
                                    foreach (JObject root in objects)
                                    {
                                        dynamic obj = new System.Dynamic.ExpandoObject();
                                        var id = string.Empty;
                                        foreach (KeyValuePair<String, JToken> app in root)
                                        {
                                            var key = app.Key;
                                            var value = app.Value;
                                            if (key == api.FilterColumn)
                                            {
                                                var a = value.Value<string>();
                                                DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                                ExpandoAddProperty(obj, key, dt);
                                            }
                                            else
                                            {
                                                var a = value.Value<string>();
                                                ExpandoAddProperty(obj, key, a);
                                            }
                                            if (key == api.IdColumn)
                                            {
                                                id = value.Value<string>();
                                            }

                                        }
                                        descriptor.Index<object>(i => i
                                            .Index(api.NoteSubject)
                                            .Id((Id)id)
                                            .Document(obj));
                                    }
                                    var bulkResponse = client.Bulk(descriptor);
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = bulkResponse.ToString(),
                                        error = bulkResponse.Errors.ToString(),
                                        success = true
                                    });
                                    return true;
                                }
                                else
                                {
                                    await this.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = "[]",
                                        error = "",
                                        success = true
                                    });
                                    return false;
                                }

                            }
                            else
                            {
                                await this.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    districtCode = districtCode,
                                    response = await response1.Content.ReadAsStringAsync(),
                                    error = response1.ReasonPhrase.ToString(),
                                    success = false
                                });
                                return true;
                            }
                        }
                    }


                }

            }

            return true;
        }
        private async Task ManageSchedulerLog(SchedulerLogViewModel model)
        {
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = _userContext.UserId;
            templateModel.DataAction = DataActionEnum.Create;
            templateModel.TemplateCode = "SCHEDULER_LOG";
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.NoteSubject = model.NoteSubject;
            newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(newmodel);

        }
        //public async Task<JsonResult> ReadRoipDatafromMysql()
        //{
        //    var _today = DateTime.Now;
        //    var table = _today.ToString("yy-MM-dd");
        //    var list = new List<RoipViewModel>();
        //    var dbConnections = await _noteBusiness.GetDbConnectionData();
        //    if (dbConnections.Count > 0)
        //    {
        //        var con = dbConnections.Where(x => x.NoteSubject == "ROIP").FirstOrDefault();
        //        if (con.IsNotNull())
        //        {
        //            list = await _noteBusiness.GetRoipData(con.hostName, con.username, con.maintenanceDatabase, con.port, con.password, table);                    
        //        }
        //    }            
        //    return Json(list);
        //}
        public async Task<JsonResult> GetDates()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var content = ApplicationConstant.BusinessAnalytics.RoipDistinctTables;
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "roip_data/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var aggregations = trackdata.SelectToken("aggregations");
                    if (aggregations.IsNotNull())
                    {
                        var distinct_date_value = aggregations.SelectToken("distinct_date_value");
                        if (distinct_date_value.IsNotNull())
                        {
                            var buckets = distinct_date_value.SelectToken("buckets");
                            foreach (var bucket in buckets)
                            {
                                var key_as_string = bucket.SelectToken("key_as_string");
                                var date = Convert.ToDateTime(key_as_string);
                                list.Add(new IdNameViewModel { CreatedDate = date, Code = date.ToString("yyyy-MM-dd") });
                            }
                        }


                    }
                }
            }
            return Json(list.OrderByDescending(x => x.CreatedDate));
        }

        public IActionResult SocialMediaData()
        {
            return View();
        }

        public async Task<JsonResult> GetTwitterData(string locationName, int size = 0)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<TwitterTrendingViewModel> list = new List<TwitterTrendingViewModel>();

            var content = "";
            if (locationName.IsNotNullAndNotEmpty())
            {
                content = ApplicationConstant.BusinessAnalytics.GetTwitterTrendingResultsByLocation;
                content = content.Replace("#SEARCHWHERE#", locationName);
            }
            else
            {
                content = ApplicationConstant.BusinessAnalytics.ReadTwitterTrendingDataQuery;
            }

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "trending_twitter/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<TwitterTrendingViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }
            if (size != 0)
            {
                var tlist = list.Take(size);
                return Json(tlist);
            }
            return Json(list);
        }

        public async Task<JsonResult> GetYoutubeData(string locationName, int size = 0)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<YoutubeTrendingViewModel> list = new List<YoutubeTrendingViewModel>();

            var content = "";
            if (locationName.IsNotNullAndNotEmpty())
            {
                content = ApplicationConstant.BusinessAnalytics.GetYoutubeTrendingResultsByLocation;
                content = content.Replace("#SEARCHWHERE#", locationName);
            }
            else
            {
                content = ApplicationConstant.BusinessAnalytics.ReadYoutubeTrendingDataQuery;
            }


            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "trending_youtube/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<YoutubeTrendingViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }
            foreach (var x in list)
            {
                x.videoId = "https://www.youtube.com/watch?v=" + x.videoId;
            }
            if (size != 0)
            {
                var ylist = list.Take(size);

                return Json(ylist);
            }
            return Json(list);
        }

        public async Task<JsonResult> GetTrendingLocationList()
        {
            var list = await _noteBusiness.GetAllTrendingLocation();
            var locList = list.Select(x => x.NoteSubject).Distinct();
            return Json(locList);
        }

        public IActionResult TrendingTwitterPage(string locationName)
        {
            ViewBag.LocationName = locationName;
            return View();
        }
        public IActionResult TrendingYoutubePage(string locationName)
        {
            ViewBag.LocationName = locationName;
            return View();
        }

        public IActionResult Posts()
        {
            return View();
        }

        public async Task<JsonResult> GetAllTwitterPosts()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<TwitterViewModel> list = new List<TwitterViewModel>();


            var content = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;


            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "twitter_post/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<TwitterViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }

            return Json(list);
        }

        public async Task<bool> TestFacebookUser()
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var credential = await _noteBusiness.GetFacebookCredential();
                var url = "https://xtranet.aitalkx.com/social/fb_user_data?fb_user_id=sunil.grover.71404";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(500);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var json = JToken.Parse(jsonStr);
                    var data1 = json.SelectToken("data");
                    if (data1.IsNotNull())
                    {
                        foreach (var data in data1)
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            var dataStr = JsonConvert.SerializeObject(data);
                            var user = JsonConvert.DeserializeObject<FacebookUserViewModel>(dataStr);
                            var _details = data.SelectToken("introduction");
                            if (_details.IsNotNull())
                            {
                                var _joined = _details.First();
                                user.join_date = _joined.ToString();
                            }
                            var id = user.profile_url;
                            var indexName = "facebook_user";
                            descriptor.Index<object>(i => i
                                    .Index(indexName)
                                    .Id((Id)id)
                                    .Document(user));
                            var bulkResponse = client.Bulk(descriptor);
                            if (bulkResponse.IsValid)
                            {
                                var frnd_list = data.SelectToken("friend_list");
                                if (frnd_list.IsNotNull())
                                {
                                    BulkDescriptor descriptor1 = new BulkDescriptor();
                                    foreach (var item in frnd_list)
                                    {

                                        var dataStr1 = JsonConvert.SerializeObject(item);
                                        var friend = JsonConvert.DeserializeObject<FacebookFriendViewModel>(dataStr1);
                                        if (friend.IsNotNull())
                                        {
                                            friend.parent_id = user.profile_url;
                                            var id1 = user.profile_url + friend.friend_profile_url;
                                            var indexName1 = "facebook_friend";
                                            descriptor1.Index<object>(i => i
                                                    .Index(indexName1)
                                                    .Id((Id)id1)
                                                    .Document(friend));
                                        }

                                    }
                                    var bulkResponse1 = client.Bulk(descriptor1);
                                }
                                var post_list = data.SelectToken("post_data");
                                if (post_list.IsNotNull())
                                {
                                    BulkDescriptor descriptor2 = new BulkDescriptor();
                                    foreach (var item in post_list)
                                    {

                                        var dataStr2 = JsonConvert.SerializeObject(item);
                                        var post = JsonConvert.DeserializeObject<FacebookUserPostViewModel>(dataStr2);
                                        if (post.IsNotNull())
                                        {
                                            var _polarity = item.SelectToken("post_msg_polarity");
                                            if (_polarity.IsNotNull())
                                            {
                                                var polarity = JsonConvert.SerializeObject(_polarity);
                                                var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                                if (polarity_model.IsNotNull())
                                                {
                                                    post.pos = polarity_model.pos;
                                                    post.neg = polarity_model.neg;
                                                    post.neu = polarity_model.neu;
                                                    post.compound = polarity_model.compound;
                                                }
                                            }
                                            post.parent_id = user.profile_url;
                                            var id2 = user.profile_url + post.post_title;
                                            var indexName2 = "facebook_user_post";
                                            descriptor2.Index<object>(i => i
                                                    .Index(indexName2)
                                                    .Id((Id)id2)
                                                    .Document(post));
                                        }

                                    }
                                    var bulkResponse2 = client.Bulk(descriptor2);
                                }

                            }
                        }



                        return true;

                    }

                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IActionResult FacebookUsersData()
        {
            return View();
        }

        public IActionResult ForecastData()
        {
            return View();
        }

        public async Task<JsonResult> GetForecastData()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadDial100ForecastDataQuery;
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var url = eldbUrl + "dial100_forecast/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsonStr = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsonStr);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<Dial100ForcastViewModel>(str);
                            dynamic json = JsonConvert.DeserializeObject(result.json);
                            return Json(json);
                        }
                    }
                }

            }
            return Json(null);

        }

        public async Task<JsonResult> GetAllFacebookUsers()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<FacebookUserViewModel> list = new List<FacebookUserViewModel>();


            var content = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;


            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "facebook_user/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<FacebookUserViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }

            return Json(list);
        }

        public async Task<IActionResult> FBUserDetails(string profileId)
        {
            ViewBag.ProfileId = profileId;
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<FacebookUserViewModel> list = new List<FacebookUserViewModel>();


            var content = ApplicationConstant.BusinessAnalytics.GetFBUserById;
            content = content.Replace("#SEARCHWHERE#", profileId);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "facebook_user/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<FacebookUserViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }

            return View(list[0]);
        }

        public async Task<JsonResult> GetFBFriendsList(string profileId)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<FacebookFriendViewModel> list = new List<FacebookFriendViewModel>();


            var content = ApplicationConstant.BusinessAnalytics.GetFriendsListByParentId;
            content = content.Replace("#SEARCHWHERE#", profileId);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "facebook_friend/_search?pretty=true&size=10000";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<FacebookFriendViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }

            return Json(list);
        }

        public async Task<JsonResult> GetFBUserPosts(string profileId)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<FacebookUserPostViewModel> list = new List<FacebookUserPostViewModel>();


            var content = ApplicationConstant.BusinessAnalytics.GetFriendsListByParentId;
            content = content.Replace("#SEARCHWHERE#", profileId);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "facebook_user_post/_search?pretty=true&size=10000";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<FacebookUserPostViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }

            return Json(list);
        }
        public async Task<JsonResult> GetCommonFBFriendsList(string id)
        {
            var list1 = new List<FacebookCommonFriendViewModel>();
            var list2 = new List<FacebookCommonFriendViewModel>();
            var newlist = new List<FacebookCommonFriendViewModel>();
            var cubeJs = ApplicationConstant.AppSettings.CubeJsUrl(_configuration);
            var content = @"{""filters"": [],""measures"": [""FacebookFriend.count""],""timeDimensions"": [],""order"": {""FacebookFriend.count"": ""desc""},""dimensions"": [""FacebookFriend.friend_profile_url"",""FacebookFriend.friend_name""],""limit"": 10000}";
            var content2 = @"{""filters"": [{""member"": ""FacebookFriend.parent_id"",""operator"": ""equals"",""values"": [""#PARENTID#""]}],""measures"": [""FacebookFriend.count""],""timeDimensions"": [],""order"": {""FacebookFriend.count"": ""desc"" },""dimensions"": [ ""FacebookFriend.friend_profile_url""],""limit"": 10000}";
            content2 = content2.Replace("#PARENTID#", id);
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            using (var httpClient = new HttpClient(handler))
            {
                var url = $@"{cubeJs}cubejs-api/v1/load?query={content}";
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var _jsondata = await response.Content.ReadAsStringAsync();
                    var _dataToken = JToken.Parse(_jsondata);
                    var _data = _dataToken.SelectToken("data");
                    var data = JsonConvert.SerializeObject(_data);
                    data = data.Replace(".", "_");
                    var list = JsonConvert.DeserializeObject<List<FacebookCommonFriendViewModel>>(data);
                    list1 = list.Where(x => x.FacebookFriend_count > 1).ToList();
                }
                var url2 = $@"{cubeJs}cubejs-api/v1/load?query={content2}";
                var address2 = new Uri(url2);
                var response2 = await httpClient.GetAsync(address2);
                if (response2.IsSuccessStatusCode)
                {
                    var _jsondata = await response2.Content.ReadAsStringAsync();
                    var _dataToken = JToken.Parse(_jsondata);
                    var _data = _dataToken.SelectToken("data");
                    var data = JsonConvert.SerializeObject(_data);
                    data = data.Replace(".", "_");
                    list2 = JsonConvert.DeserializeObject<List<FacebookCommonFriendViewModel>>(data);
                }
            }
            foreach (var item1 in list1)
            {
                foreach (var item2 in list2)
                {
                    if (item1.FacebookFriend_friend_profile_url == item2.FacebookFriend_friend_profile_url)
                    {
                        newlist.Add(item1);
                        break;
                    }
                }
            }
            return Json(newlist);
        }
        public IActionResult Dial100FilterResults(string chartName, string param)
        {
            ViewBag.ChartName = chartName;
            var searchStr = "";
            var parameters = param.Split('&');
            foreach (var x in parameters)
            {
                var para = x.Split('=');
                if (searchStr.IsNullOrEmpty())
                {
                    searchStr += para[1];
                }
                else
                {
                    searchStr = searchStr + " " + para[1];
                }
            }

            ViewBag.param = searchStr;
            return View("Dial100FilterData");
        }


        public async Task<JsonResult> getDial100FilterResults(string searchStr)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<Dial100ViewModel> list = new List<Dial100ViewModel>();
            var content = ApplicationConstant.BusinessAnalytics.GetFilteredDial100Data;
            content = content.Replace("#SEARCHWHERE#", searchStr);


            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "dial100/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<Dial100ViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }

            return Json(list);
        }
        public async Task<IActionResult> CCTNSReportDetails()
        {
            var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
            ViewBag.ApiList = apilist.Where(x => x.Url.ToLower().Contains(x.NoteSubject.ToLower())).ToList();
            return View();
        }
        public async Task<JsonResult> GetCCTNSApiData()
        {
            var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
            var list = apilist.Where(x => x.Url.ToLower().Contains(x.NoteSubject.ToLower())).Select(x => new IdNameViewModel { Id = x.Id, Name = x.NoteSubject }).ToList();
            return Json(list);
        }
        public async Task<IActionResult> CCTNSApiDetails(string id, DateTime fromDate, DateTime todate, string queryStr)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<dynamic> list = new List<dynamic>();
            var query = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
            var api = await _noteBusiness.GetCCTNSApiMethodsDetails(id);
            if (query.IsNotNullAndNotEmpty())
            {
                var stringContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + api.NoteSubject.ToLower() + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var jsontrack = await response.Content.ReadAsStringAsync();
                    var trackdata = JToken.Parse(jsontrack);
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            dynamic obj = new System.Dynamic.ExpandoObject();
                            foreach (JProperty root in source)
                            {

                                var key = root.Name;
                                var a = root.First().ToString();
                                ExpandoAddProperty(obj, key, a);
                            }
                            list.Add(obj);
                            break;
                        }
                    }
                }
            }
            ViewBag.Heading = api.NoteSubject;
            ViewBag.IndexName = api.NoteSubject.ToLower();
            ViewBag.FilterColumn = api.FilterColumn;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = todate;
            ViewBag.queryStr = queryStr;
            return View("CCTNSGridView", list.ToDataTable());
        }
        public async Task<JsonResult> ReadCCTNSApiDetails(string indexName, string filterColumn, DateTime fromDate, DateTime todate, string queryStr)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<dynamic> list = new List<dynamic>();
            var content = "";
            if (queryStr.IsNullOrEmpty())
            {
                content = ApplicationConstant.BusinessAnalytics.ReadCctnsReportQuery;
            }
            else
            {
                content = ApplicationConstant.BusinessAnalytics.ReadCCTNSDataByFilters;
                content = content.Replace("#SEARCHWHERE#", queryStr);
            }
            content = content.Replace("#FILTERCOLUMN#", filterColumn);
            content = content.Replace("#STARTDATE#", fromDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            content = content.Replace("#ENDDATE#", todate.ToString("yyyy-MM-ddTHH:mm:ss"));
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {

                var url = eldbUrl + indexName + "/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            dynamic obj = new System.Dynamic.ExpandoObject();
                            foreach (JProperty root in source)
                            {

                                var key = root.Name;
                                var a = root.First().ToString();
                                ExpandoAddProperty(obj, key, a);
                            }
                            list.Add(obj);
                        }
                    }
                }

            }

            return Json(list);
        }

        public IActionResult Dial100EventsList()
        {
            return View();
        }
        public IActionResult CCTNSDetails(string cctnsData)
        {
            JObject json = JObject.Parse(cctnsData);
            string jsonStr = JsonConvert.SerializeObject(json);
            ViewBag.JsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
            return View();
        }

        public async Task<JsonResult> FetchDial100DataByFilters(string queryString, DateTime fromDate, DateTime todate)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<Dial100ViewModel> list = new List<Dial100ViewModel>();
            var content = "";
            if (queryString.IsNullOrEmpty())
            {
                content = ApplicationConstant.BusinessAnalytics.ReadDial100DataQuery5;
            }
            else
            {
                content = ApplicationConstant.BusinessAnalytics.ReadDial100DataByFilters;
                content = content.Replace("#SEARCHWHERE#", queryString);
            }
            content = content.Replace("#STARTDATE#", fromDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            content = content.Replace("#ENDDATE#", todate.ToString("yyyy-MM-ddTHH:mm:ss"));
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {

                var url = eldbUrl + "dail100/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<Dial100ViewModel>(str);
                            list.Add(result);
                        }
                    }
                }

            }

            return Json(list);
        }
        public async Task CCTNSApiDataMgrationByIndex()
        {
            try
            {
                var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
                var api = apilist.Where(x => x.Url.ToLower().Contains("get_xtra_police_custody_escape")).FirstOrDefault();
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var url = eldbUrl + api.NoteSubject.ToLower() + "/_search?pretty=true";
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var query = ApplicationConstant.BusinessAnalytics.CctnsMaxDateQuery;
                    query = query.Replace("#INDEXNAME#", api.NoteSubject.ToLower());
                    var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                    var main_url = eldbUrl + "cctns_common/_search?pretty=true";
                    var main_address = new Uri(main_url);
                    var main_response = await httpClient.PostAsync(main_address, queryContent);
                    if (main_response.IsSuccessStatusCode)
                    {
                        if (main_response.Content != null)
                        {
                            var _jsondata = await main_response.Content.ReadAsStringAsync();
                            if (_jsondata != null)
                            {
                                var _dataToken = JToken.Parse(_jsondata);
                                if (_dataToken != null)
                                {
                                    var _responsedata = _dataToken.SelectToken("aggregations");
                                    if (_responsedata != null)
                                    {
                                        var _maxdateToken = _responsedata.SelectToken("max_date");
                                        if (_maxdateToken.Count() > 0)
                                        {
                                            var _dateToken = _maxdateToken.Last();
                                            if (_dataToken.Count() > 0)
                                            {
                                                var _date = _dateToken.Last();
                                                if (_date != null && _date.HasValues)
                                                {
                                                    var _startDate = _date.Value<DateTime>();
                                                    var content = ApplicationConstant.BusinessAnalytics.CctnsDateRangeQuery;
                                                    content = content.Replace("#FILTERCOLUMN#", api.FilterColumn);
                                                    content = content.Replace("#STARTDATE#", _startDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                                                    content = content.Replace("#ENDDATE#", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                                                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                                                    var address = new Uri(url);
                                                    var response = await httpClient.PostAsync(address, stringContent);
                                                    var jsontrack = await response.Content.ReadAsStringAsync();
                                                    var trackdata = JToken.Parse(jsontrack);
                                                    if (trackdata.IsNotNull())
                                                    {
                                                        var hits = trackdata.SelectToken("hits");
                                                        if (hits.IsNotNull())
                                                        {
                                                            BulkDescriptor descriptor = new BulkDescriptor();
                                                            var _hits = hits.SelectToken("hits");
                                                            foreach (var hit in _hits)
                                                            {
                                                                var _id = hit.SelectToken("_id");
                                                                var _index = hit.SelectToken("_index");
                                                                var source = hit.SelectToken("_source");
                                                                if (source.IsNotNull())
                                                                {
                                                                    var _reportDate = source.SelectToken(api.FilterColumn);
                                                                    var str = JsonConvert.SerializeObject(source);
                                                                    var result = JsonConvert.DeserializeObject<CctnsKeywordViewModel>(str);
                                                                    if (result.IsNotNull())
                                                                    {
                                                                        var model = new CctnsCommonViewModel
                                                                        {
                                                                            ZoneName = result._ZoneName,
                                                                            RangeName = result._RangeName,
                                                                            District = result._District,
                                                                            PoliceStation = result._PoliceStation,
                                                                            JsonString = str,
                                                                            IndexName = _index.ToString(),
                                                                            ReportDate = ((DateTime)_reportDate)
                                                                        };
                                                                        var id = _id.ToString();
                                                                        var indexName = "cctns_common";
                                                                        descriptor.Index<object>(i => i
                                                                                .Index(indexName)
                                                                                .Id((Id)id)
                                                                                .Document(model));

                                                                    }

                                                                }


                                                            }
                                                            var bulkResponse = client.Bulk(descriptor);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                        }

                    }
                    else
                    {
                        var content = ApplicationConstant.BusinessAnalytics.CctnsBulkQuery;
                        content = content.Replace("#FILTERCOLUMN#", api.FilterColumn);
                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var jsontrack = await response.Content.ReadAsStringAsync();
                        var trackdata = JToken.Parse(jsontrack);
                        if (trackdata.IsNotNull())
                        {
                            var hits = trackdata.SelectToken("hits");
                            if (hits.IsNotNull())
                            {
                                BulkDescriptor descriptor = new BulkDescriptor();
                                var _hits = hits.SelectToken("hits");
                                foreach (var hit in _hits)
                                {
                                    var _id = hit.SelectToken("_id");
                                    var _index = hit.SelectToken("_index");
                                    var source = hit.SelectToken("_source");
                                    if (source.IsNotNull())
                                    {
                                        var _reportDate = source.SelectToken(api.FilterColumn);
                                        var str = JsonConvert.SerializeObject(source);
                                        var result = JsonConvert.DeserializeObject<CctnsKeywordViewModel>(str);
                                        if (result.IsNotNull())
                                        {
                                            var model = new CctnsCommonViewModel
                                            {
                                                ZoneName = result._ZoneName,
                                                RangeName = result._RangeName,
                                                District = result._District,
                                                PoliceStation = result._PoliceStation,
                                                JsonString = str,
                                                IndexName = _index.ToString(),
                                                ReportDate = ((DateTime)_reportDate)
                                            };
                                            var id = _id.ToString();
                                            var indexName = "cctns_common";
                                            descriptor.Index<object>(i => i
                                                    .Index(indexName)
                                                    .Id((Id)id)
                                                    .Document(model));

                                        }

                                    }


                                }
                                var bulkResponse = client.Bulk(descriptor);
                            }
                        }
                    }



                }
            }
            catch (Exception e)
            {
                throw;
            }



        }
        public IActionResult ROIPReport()
        {
            return View();
        }
        public async Task<JsonResult> GetVdpDistrictData()
        {
            var districtList = await _noteBusiness.GetAllVDPDistrict();
            return Json(districtList);
        }
        public async Task<JsonResult> ReadVdpData(string id)
        {
            var model = await _noteBusiness.GetVDPDistrictById(id);
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            List<VDPViewModel> list = new List<VDPViewModel>();
            var content = ApplicationConstant.BusinessAnalytics.ReadVdpDataQuery;
            content = content.Replace("#DISTRICTNAME#", model.Name);
            content = content.Replace("#STARTDATE#", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd'T'HH:mm:ss"));
            content = content.Replace("#ENDDATE#", DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "vdpi/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, stringContent);
                var jsontrack = await response.Content.ReadAsStringAsync();
                var trackdata = JToken.Parse(jsontrack);
                if (trackdata.IsNotNull())
                {
                    var hits = trackdata.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");

                        foreach (var hit in _hits)
                        {
                            var source = hit.SelectToken("_source");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<VDPViewModel>(str);
                            list.Add(result);
                        }
                    }
                }
            }
            return Json(list);
        }
        public async Task<JsonResult> VdpEvents(string id)
        {
            var _session = HttpContext.Session.GetString("mqttId");
            if (_session.IsNotNullAndNotEmpty())
            {
                string anpr_server_ip = _session.Split(',')[1];
                int mqtt_port = 1884;

                // Create a new MQTT client.
                var factory = new MqttFactory();
                var _mqttClient = factory.CreateMqttClient();

                var options = new MqttClientOptionsBuilder()
                            .WithClientId(_session)
                            .WithTcpServer(anpr_server_ip, mqtt_port)
                            .WithCleanSession()
                            .Build();
                await _mqttClient.ConnectAsync(options);
                HttpContext.Session.Remove("mqttId");
            }

            var model = await _noteBusiness.GetVDPDistrictById(id);
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                string anpr_topic = "events/ANPR/#";
                string anpr_server_ip = model.Id;
                int mqtt_port = 1884;

                // Create a new MQTT client.
                var factory = new MqttFactory();
                var _mqttClient = factory.CreateMqttClient();
                var mqttId = Guid.NewGuid().ToString() + "," + model.Id;
                HttpContext.Session.SetString("mqttId", mqttId);
                var options = new MqttClientOptionsBuilder()
                            .WithClientId(mqttId)
                            .WithTcpServer(anpr_server_ip, mqtt_port)
                            .WithCleanSession()
                            .Build();


                // connect handler
                _mqttClient.UseConnectedHandler(async e =>
                {
                    Console.WriteLine("Mqtt broker connected " + mqttId);

                    // Subscribe to a topic
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(anpr_topic).Build());

                });

                // disconnect handler - tries to reconnect
                _mqttClient.UseDisconnectedHandler(async e =>
                {
                    Console.WriteLine("Successfully Broker Discopnnected " + mqttId);

                    //await Task.Delay(TimeSpan.FromSeconds(5));

                    //try
                    //{
                    //    await _mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None); // Since 3.0.5 with CancellationToken
                    //}
                    //catch
                    //{

                    //}
                });
                var data = string.Empty;
                // handling message reveived on the topic
                _mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    data = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    if (data.IsNotNullAndNotEmpty())
                    {
                        var json = JToken.Parse(data);
                        var _xtradetails = json.SelectToken("ExtraDetails");
                        var dataStr = JsonConvert.SerializeObject(_xtradetails);
                        var obj = JsonConvert.DeserializeObject<VDPViewModel>(data);
                        var extraObj = JsonConvert.DeserializeObject<ExtraDetails>(dataStr);
                        using (var httpClient = new HttpClient())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            obj.Longitude = extraObj.Longitude;
                            obj.Latitude = extraObj.Latitude;
                            obj.Location = extraObj.Location;
                            obj.SerialNumber = extraObj.SerialNumber;
                            obj.DistrictId = extraObj.DistrictId;
                            obj.RTOCircleNumber = extraObj.RTOCircleNumber;
                            obj.DistrictName = model.Name;
                            obj.IpAddress = model.Id;
                            var dateTimeOffSet = DateTimeOffset.FromUnixTimeMilliseconds(obj.DetTime).LocalDateTime;
                            DateTime dateTime = dateTimeOffSet;
                            //System.DateTime dtDateTime = new DateTime(1900, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            //dtDateTime = dtDateTime.AddSeconds(obj.DetTime);
                            obj.Event_Date = dateTime;
                            descriptor.Index<object>(i => i
                                    .Index("vdpi")
                                    .Id((Id)obj.DetTime.ToString())
                                    .Document(obj));
                            var bulkResponse = client.Bulk(descriptor);
                            _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                            {
                                NoteSubject = "VDP",
                                fromDate = DateTime.Now.ToString(),
                                toDate = DateTime.Now.ToString(),
                                districtCode = model.Name,
                                response = data,
                                error = null,
                                success = true,
                                ActiveUserId = _userContext.UserId
                            });

                        }
                    }

                });
                await _mqttClient.ConnectAsync(options);
                return Json(new { success = true });

            }
            catch (Exception e)
            {
                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                {
                    NoteSubject = "VDP",
                    fromDate = DateTime.Now.ToString(),
                    toDate = DateTime.Now.ToString(),
                    districtCode = model.Name,
                    response = null,
                    error = e.Message.ToString(),
                    success = false,
                    ActiveUserId = _userContext.UserId
                });
                return Json(new { success = false });

            }
            //}
            //else
            //{
            //    Console.WriteLine("Broker already connected hence returning false");
            //    return Json(new { success = false });
            //}
        }
        public async Task<JsonResult> VdpDisconnect()
        {
            try
            {
                var mqttId = HttpContext.Session.GetString("mqttId");
                if (mqttId.IsNotNullAndNotEmpty())
                {
                    string anpr_server_ip = mqttId.Split(',')[1];
                    int mqtt_port = 1884;

                    // Create a new MQTT client.
                    var factory = new MqttFactory();
                    var _mqttClient = factory.CreateMqttClient();

                    var options = new MqttClientOptionsBuilder()
                                .WithClientId(mqttId)
                                .WithTcpServer(anpr_server_ip, mqtt_port)
                                .WithCleanSession()
                                .Build();
                    await _mqttClient.ConnectAsync(options);
                    HttpContext.Session.Remove("mqttId");
                    return Json(new { success = true });
                }
                return Json(new { success = false });
                //if (_mqttClient.IsConnected)
                //{                   
                //    string[] filters = { "events/ANPR/#" };

                //    var disconnectOption = new MqttClientDisconnectOptions
                //    {
                //        ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                //        ReasonString = "NormalDiconnection"
                //    };
                //    var unsubscribeOption = new MqttClientUnsubscribeOptions
                //    {
                //        TopicFilters = new List<string>(filters)
                //    };
                //    object p = await _mqttClient.UnsubscribeAsync(unsubscribeOption);
                //    await _mqttClient.DisconnectAsync(disconnectOption);
                //}               

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in Mqtt Disconnection " + e.ToString());
                return Json(new { success = false });
            }
        }
        public IActionResult VDPReport()
        {
            return View();
        }
        public async Task<bool> YoutubeTrendingDataMigration()
        {
            //var url = "https://youtube.googleapis.com/youtube/v3/search?part=snippet,id&location=" + api.latitude + "," + api.longitude + "&locationRadius=50km&maxResults=10&q=" + api.NoteSubject + "&type=video%2Clist&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
            var url = "https://youtube.googleapis.com/youtube/v3/search?part=snippet,id&location=23.259933,77.412613&locationRadius=50km&maxResults=10&q=Bhopal&type=video%2Clist&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
            using (var httpClient = new HttpClient())
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                var jsonStr = await response.Content.ReadAsStringAsync();
                var json = JToken.Parse(jsonStr);
                var items = json.SelectToken("items");
                if (items.IsNotNull())
                {
                    BulkDescriptor descriptor = new BulkDescriptor();
                    foreach (var item in items)
                    {
                        var source = item.SelectToken("id");
                        var id = string.Empty;
                        if (source.IsNotNull())
                        {
                            var videoid = source.SelectToken("videoId");
                            if (videoid.IsNotNull())
                            {
                                id = videoid.Value<string>();
                            }

                        }
                        var _snippet = item.SelectToken("snippet");


                        var dataStr = JsonConvert.SerializeObject(_snippet);
                        var model = JsonConvert.DeserializeObject<YoutubeTrendingViewModel>(dataStr);
                        var _statistics = item.SelectToken("statistics");
                        var dataStr1 = JsonConvert.SerializeObject(_statistics);
                        var model1 = JsonConvert.DeserializeObject<YoutubeTrendingViewModel>(dataStr1);
                        if (model.IsNotNull())
                        {
                            model.videoId = id;
                            model.location = "Bhopal";
                            if (model1.IsNotNull())
                            {
                                model.likeCount = model1.likeCount;
                                model.viewCount = model1.viewCount;
                                model.favoriteCount = model1.favoriteCount;
                                model.commentCount = model1.commentCount;

                            }
                            try
                            {
                                var _thumbnail = _snippet.SelectToken("thumbnails");
                                var _default = _thumbnail.SelectToken("default");
                                var _url = _default.SelectToken("url");
                                if (_url.IsNotNull())
                                {
                                    model.thumbnailUrl = _url.Value<string>();
                                }
                            }
                            catch (Exception)
                            {


                            }
                            var indexName = "trending_youtube";
                            descriptor.Index<object>(i => i
                                .Index(indexName)
                                .Id((Id)id)
                                .Document(model));
                        }

                    }
                    var bulkResponse = client.Bulk(descriptor);
                }

            }
            return true;
        }
        public async Task<IActionResult> AlertRemarks(string parentId, string type)
        {
            var model = new AlertRemarkViewModel { parentId = parentId, remark_type = type };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageAlertRemarks(AlertRemarkViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.remark.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Remark Is Required !" });
                    }
                    var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                    var settings = new ConnectionSettings(new Uri(eldbUrl));
                    var client = new ElasticClient(settings);
                    BulkDescriptor descriptor = new BulkDescriptor();
                    model.id = Guid.NewGuid().ToString();
                    if (model.isFalseEvent && model.remark_type.IsNullOrEmpty())
                    {
                        model.remark_type = "Fake";
                    }
                    else if (!model.isFalseEvent && model.remark_type.IsNullOrEmpty())
                    {
                        model.remark_type = "Action";
                    }
                    model.remark_datetime = DateTime.Now;
                    var indexName = "alert_action_remarks";
                    descriptor.Index<object>(i => i
                            .Index(indexName)
                            .Id((Id)model.id)
                            .Document(model));
                    var bulkResponse = client.Bulk(descriptor);
                    if (bulkResponse.IsValid)
                    {
                        if (model.remark_type == "Close")
                        {
                            var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[""#IDS#""]}}]}},""script"": { ""source"": ""ctx._source['isVisible'] =false;ctx._source['isFalseEvent'] =#ISFALSEEVENT#;DateFormat df = new SimpleDateFormat(\""yyyy-MM-dd'T'HH:mm:ss.SSS\"");\ndf.setTimeZone(TimeZone.getTimeZone(\""IST\""));\nDate date = new Date();ctx._source['close_datetime'] =df.format(date);""} }";
                            content2 = content2.Replace("#IDS#", model.parentId);
                            content2 = content2.Replace("#ISFALSEEVENT#", model.isFalseEvent.ToString().ToLower());
                            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                            var url2 = eldbUrl + "iip_alert_data/_update_by_query";
                            var address2 = new Uri(url2);
                            using (var httpClient = new HttpClient())
                            {
                                var response2 = await httpClient.PostAsync(address2, stringContent2);
                            }
                        }
                        //else
                        //{
                        //    var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[""#IDS#""]}}]}},""script"": { ""source"": ""ctx._source['isFalseEvent'] =#ISFALSEEVENT#""} }";
                        //    content2 = content2.Replace("#IDS#", model.parentId);
                        //    content2 = content2.Replace("#ISFALSEEVENT#", model.isFalseEvent.ToString());
                        //    var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                        //    var url2 = eldbUrl + "iip_alert_data/_update_by_query";
                        //    var address2 = new Uri(url2);
                        //    using (var httpClient = new HttpClient())
                        //    {
                        //        var response2 = await httpClient.PostAsync(address2, stringContent2);
                        //    }
                        //}
                        return Json(new { success = true });
                    }
                }
                return Json(new { success = false, error = ModelState.SerializeErrors() });
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = "Something went wrong !" });
            }


        }
        public async Task<bool> GetRoipTablesData()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var _date = await GetRoipLastDate();
            if (_date.IsNotNullAndNotEmpty())
            {
                var query = "select * from " + _date + ";";
                using var connection = new MySqlConnector.MySqlConnection("Server=10.10.10.110;User ID=root ;Password=pulsecom;Database=deepvlg");
                await connection.OpenAsync();
                using var command = new MySqlConnector.MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                BulkDescriptor descriptor = new BulkDescriptor();
                while (reader.Read())
                {
                    dynamic obj = new System.Dynamic.ExpandoObject();
                    var date_value = reader.GetDateTime("date_value");
                    ExpandoAddProperty(obj, "date_value", date_value);
                    Console.WriteLine(date_value);
                    var start_time = reader.GetTimeSpan("start_time");
                    ExpandoAddProperty(obj, "start_time", start_time.ToString());
                    var start_datetime = date_value.Add(start_time);
                    ExpandoAddProperty(obj, "start_datetime", start_datetime);
                    var end_time = reader.GetTimeSpan("end_time");
                    ExpandoAddProperty(obj, "end_time", end_time.ToString());
                    var end_datetime = date_value.Add(end_time);
                    ExpandoAddProperty(obj, "end_datetime", end_datetime);
                    var dur_time = reader.GetTimeSpan("dur_time");
                    ExpandoAddProperty(obj, "dur_time", dur_time.ToString());
                    var dur_datetime = date_value.Add(dur_time);
                    ExpandoAddProperty(obj, "dur_datetime", dur_datetime);
                    var file_name = reader.GetString("file_name");
                    ExpandoAddProperty(obj, "file_name", file_name);
                    var channel_no = reader.GetInt64("channel_no");
                    ExpandoAddProperty(obj, "channel_no", channel_no);
                    var id = file_name + dur_time.ToString();
                    descriptor.Index<object>(i => i
                           .Index("roip_data")
                           .Id((Id)id)
                           .Document(obj));
                }
                var bulkResponse = client.Bulk(descriptor);
            }

            return true;

        }
        private async Task<string> GetRoipLastDate()
        {
            List<string> tables = new List<string>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            query = query.Replace("#FILTERCOLUMN#", "date_value");
            var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "roip_data/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, queryContent);
                if (response.IsSuccessStatusCode)
                {
                    var _jsondata = await response.Content.ReadAsStringAsync();
                    var _dataToken = JToken.Parse(_jsondata);
                    var _responsedata = _dataToken.SelectToken("aggregations");
                    var _maxdateToken = _responsedata.SelectToken("max_date");
                    var _dateToken = _maxdateToken.Last();
                    var _date = _dateToken.Last();
                    var date = _date.Value<DateTime>();
                    var _newDate = date.AddDays(1);
                    var url1 = eldbUrl + "roip_tables/_search?size=10000";
                    var address1 = new Uri(url1);
                    var response1 = await httpClient.GetAsync(address1);
                    if (response1.IsSuccessStatusCode)
                    {
                        var _jsondata1 = await response1.Content.ReadAsStringAsync();
                        var _dataToken1 = JToken.Parse(_jsondata1);
                        if (_dataToken1.IsNotNull())
                        {
                            var hits = _dataToken1.SelectToken("hits");
                            if (hits.IsNotNull())
                            {
                                var _hits = hits.SelectToken("hits");
                                foreach (var hit in _hits)
                                {
                                    var source = hit.SelectToken("_source");
                                    if (source.IsNotNull())
                                    {
                                        var table_name = source.SelectToken("table_name");
                                        if (table_name.IsNotNull())
                                        {
                                            var _table = table_name.Value<string>();
                                            tables.Add(_table);
                                        }

                                    }



                                }
                            }
                        }
                    }
                    if (date == DateTime.Now.Date || (date.AddDays(1) == DateTime.Now.Date && date.AddDays(1).AddHours(2) < DateTime.Now))
                    {
                        var newDate1 = date.ToString("yy-MM-dd");
                        return newDate1.Replace("-", "_");
                    }
                    var dateStr = await GetRoipDate(tables, _newDate);
                    return dateStr;

                }
                else if (response.ReasonPhrase == "Not Found")
                {
                    return "22_02_25";
                }
            }
            return "";
        }

        private async Task<string> GetRoipDate(List<string> tables, DateTime date)
        {

            if (date == DateTime.Now.Date || date > DateTime.Now.Date)
            {
                var newDate1 = DateTime.Now.ToString("yy-MM-dd");
                return newDate1.Replace("-", "_");
            }
            var newDate = date.ToString("yy-MM-dd");
            var dateStr = newDate.Replace("-", "_");
            if (tables.Where(x => x == dateStr).Any())
            {
                return dateStr;
            }
            else
            {
                return await GetRoipDate(tables, date.AddDays(1));
            }


        }

        public async Task<IActionResult> SocialMedia(string content)
        {
            ViewBag.Content = content;
            return View();
        }
        public async Task<JsonResult> GetDial100District()
        {
            var list = await _noteBusiness.GetAllDistrict();
            return Json(list);
        }
        public async Task<JsonResult> GetDial100EventType()
        {
            var list = await _noteBusiness.GetAllDial100Event();
            return Json(list);
        }
        public async Task<JsonResult> GetDial100EventSubType(string eventCode)
        {
            var list = await _noteBusiness.GetAllDial100SubEvent(eventCode);
            return Json(list);
        }
        public async Task<JsonResult> GetAllTracks()
        {
            var list = await _noteBusiness.GetAllTracks();
            return Json(list);
        }
        public async Task<JsonResult> GetAllKeywords(string trackName)
        {
            var list = await _noteBusiness.GetAllKeywords(trackName);
            return Json(list);
        }
        public async Task<IActionResult> ReadNewsFeedDataByFilters(string district, string keyword, DateTime from, DateTime to)
        {
            var list = new List<NewsFeedsViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "rssfeeds/_search?pretty=true";
            if (district.IsNotNullAndNotEmpty() || keyword.IsNotNullAndNotEmpty())
            {
                
                var content = ApplicationConstant.BusinessAnalytics.ReadNewsFeedDataQueryWithFilters;
                if (district == null)
                {                    
                    content = content.Replace("#SEARCHWHERE#", keyword.Trim());
                }
                else
                {
                    var searchStr = district + " " + keyword;
                    content = content.Replace("#SEARCHWHERE#", searchStr.Trim());
                }
                
                content = content.Replace("#FILTERCOLUMN#", "published");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<NewsFeedsViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }
            }
            else
            {                
                var content = ApplicationConstant.BusinessAnalytics.ReadNewsFeedDataQueryWithFilters2;
                content = content.Replace("#FILTERCOLUMN#", "published");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<NewsFeedsViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }

            }
            return Json(list);
        }
        public async Task<IActionResult> ReadYoutubeDataByFilters(string district, string keyword, DateTime from, DateTime to)
        {
            var list = new List<Youtube1ViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "youtube_post/_search?pretty=true";
            if (district.IsNotNullAndNotEmpty() || keyword.IsNotNullAndNotEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQueryWithFilters;                
                if (district.IsNotNullAndNotEmpty())
                {
                    var qc = @"{ ""match"": { ""keyword"":""#KEYWORD#""}},";
                    qc = qc.Replace("#KEYWORD#", district.Trim());
                    content = content.Replace("#QUERY_MATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MATCH#", "");
                    
                }
                if (keyword.IsNotNullAndNotEmpty())
                {
                    var qc = @"{""multi_match"":{""fields"":[ ""description"", ""title"",""channelTitle"" ],""query"":""#SEARCHWHERE#""}},";
                    qc = qc.Replace("#SEARCHWHERE#", keyword.Trim());
                    content = content.Replace("#QUERY_MULTIMATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MULTIMATCH#", "");
                }                
                content = content.Replace("#FILTERCOLUMN#", "publishTime");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<Youtube1ViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }
            }
            else
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQueryWithFilters2;  
                content = content.Replace("#FILTERCOLUMN#", "publishTime");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<Youtube1ViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }

            }
            return Json(list);
        }
        public async Task<IActionResult> ReadTwitterDataByFilters(string district, string keyword, DateTime from, DateTime to)
        {
            var list = new List<Twitter1ViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "twitter_post/_search?pretty=true";
            if (district.IsNotNullAndNotEmpty() || keyword.IsNotNullAndNotEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadTwitterDataQueryWithFilters;
                if (district.IsNotNullAndNotEmpty())
                {
                    var qc = @"{ ""match"": { ""keyword"":""#KEYWORD#""}},";
                    qc = qc.Replace("#KEYWORD#", district.Trim());
                    content = content.Replace("#QUERY_MATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MATCH#", "");

                }
                if (keyword.IsNotNullAndNotEmpty())
                {
                    var qc = @"{""multi_match"":{""fields"":[ ""text"" ],""query"":""#SEARCHWHERE#""}},";
                    qc = qc.Replace("#SEARCHWHERE#", keyword.Trim());
                    content = content.Replace("#QUERY_MULTIMATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MULTIMATCH#", "");
                }               
                content = content.Replace("#FILTERCOLUMN#", "created_at");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<Twitter1ViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }
            }
            else
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadTwitterDataQueryWithFilters2;               
                content = content.Replace("#FILTERCOLUMN#", "created_at");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<Twitter1ViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }
            }
            return Json(list);
        }
        public async Task<IActionResult> ReadFacebookDataByFilters(string district, string keyword, DateTime from, DateTime to)
        {
            var list = new List<FacebookPostViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "facebook_post/_search?pretty=true";
            if (district.IsNotNullAndNotEmpty() || keyword.IsNotNullAndNotEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadFacebookDataQueryWithFilters;                
                if (district.IsNotNullAndNotEmpty())
                {
                    var qc = @"{ ""match"": { ""keyword"":""#KEYWORD#""}},";
                    qc = qc.Replace("#KEYWORD#", district.Trim());
                    content = content.Replace("#QUERY_MATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MATCH#", "");

                }
                if (keyword.IsNotNullAndNotEmpty())
                {
                    var qc = @"{""multi_match"":{""fields"":[ ""post_msg"",""page_name""],""query"":""#SEARCHWHERE#""}},";
                    qc = qc.Replace("#SEARCHWHERE#", keyword.Trim());
                    content = content.Replace("#QUERY_MULTIMATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MULTIMATCH#", "");
                }
                content = content.Replace("#FILTERCOLUMN#", "post_date");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<FacebookPostViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }
            }
            else
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadFacebookDataQueryWithFilters2;               
                content = content.Replace("#FILTERCOLUMN#", "post_date");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<FacebookPostViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }

            }
            return Json(list);
        }
        public async Task<IActionResult> ReadInstagramDataByFilters(string district, string keyword, DateTime from, DateTime to)
        {
            var list = new List<InstagramPostViewModel>();
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var url = eldbUrl + "instagram_post/_search?pretty=true";
            if (district.IsNotNullAndNotEmpty() || keyword.IsNotNullAndNotEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadFacebookDataQueryWithFilters;
                if (district.IsNotNullAndNotEmpty())
                {
                    var qc = @"{ ""match"": { ""keyword"":""#KEYWORD#""}},";
                    qc = qc.Replace("#KEYWORD#", district.Trim());
                    content = content.Replace("#QUERY_MATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MATCH#", "");

                }
                if (keyword.IsNotNullAndNotEmpty())
                {
                    var qc = @"{""multi_match"":{""fields"":[ ""url""],""query"":""#SEARCHWHERE#""}},";
                    qc = qc.Replace("#SEARCHWHERE#", keyword.Trim());
                    content = content.Replace("#QUERY_MULTIMATCH#", qc);
                }
                else
                {
                    content = content.Replace("#QUERY_MULTIMATCH#", "");
                }
                content = content.Replace("#FILTERCOLUMN#", "post_date");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<InstagramPostViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }
            }
            else
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQueryWithFilters2;
                content = content.Replace("#FILTERCOLUMN#", "created_date");
                content = content.Replace("#STARTDATE#", from.ToString("yyyy-MM-ddTHH:mm:ss"));
                content = content.Replace("#ENDDATE#", to.ToString("yyyy-MM-ddTHH:mm:ss"));
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, stringContent);
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    var hits = data.SelectToken("hits");
                    if (hits.IsNotNull())
                    {
                        var _hits = hits.SelectToken("hits");
                        foreach (var item in _hits)
                        {
                            var source = item.SelectToken("_source");
                            var highlight = item.SelectToken("highlight");
                            var str = JsonConvert.SerializeObject(source);
                            var result = JsonConvert.DeserializeObject<InstagramPostViewModel>(str);
                            if (result.IsNotNull())
                            {
                                list.Add(result);
                            }

                        }
                    }
                }

            }
            return Json(list);
        }
        public async Task<string> GetWordCloudImageBase64()
        {
            var WordCloudApiUrl = ApplicationConstant.AppSettings.WordCloudApiUrl(_configuration);
            using (var httpClient = new HttpClient())
            {
                var url = WordCloudApiUrl;
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                var jsonStr = await response.Content.ReadAsStringAsync();
                var json = JToken.Parse(jsonStr.Replace("b'","\"").Replace("'","\""));
                var data = json[0].SelectToken("data");
                if (data.IsNotNull())
                {
                    var _data = data.Last();
                    if (_data.IsNotNull())
                    {
                        var image = _data.First().Value<string>();
                        return image;

                    }
                }
            }
            return "";
        }
        public async Task<bool> FacebookDataMigration(string keyword)
        {
            try
            {                
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var credential = await _noteBusiness.GetFacebookCredential();               
                var url = socialApiUrl + "facebook_keyword_login?keyword=Dewas&no_of_pages=1&username=8827426847&password=Kshama@25&post_keyword=recent_posts";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(500);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data = json.SelectToken("data");
                        if (data.IsNotNull())
                        {                            
                            var items = data;
                            if (items.IsNotNull())
                            {
                                BulkDescriptor descriptor = new BulkDescriptor();
                                foreach (var item in items)
                                {

                                    var dataStr = JsonConvert.SerializeObject(item);
                                    var model = JsonConvert.DeserializeObject<FacebookPostViewModel>(dataStr);
                                    var _polarity = item.SelectToken("post_msg_polarity");
                                    var polarity = JsonConvert.SerializeObject(_polarity);
                                    var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                    if (polarity_model.IsNotNull())
                                    {
                                        model.pos = polarity_model.pos;
                                        model.neg = polarity_model.neg;
                                        model.neu = polarity_model.neu;
                                        model.compound = polarity_model.compound;
                                    }
                                    if (model.IsNotNull())
                                    {
                                        model.keyword = "Dewas";
                                        var id = model.post_url;
                                        var indexName = "facebook_post";                                        
                                        descriptor.Index<object>(i => i
                                                .Index(indexName)
                                                .Id((Id)id)
                                                .Document(model));
                                    }

                                }
                                var bulkResponse = client.Bulk(descriptor);
                            }

                        }
                    }


                }
                return true;
            }
            catch (Exception e)
            {
                throw;                 
            }

        }
        public async Task InstagramDataMigration(string keyword)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                //var url = socialApiUrl + "gsearch_keyword?keyword=instagram " + keyword + "&no_of_pages=1";
                var url = "https://xtranet.aitalkx.com/insta/instagram_keyword_login?keyword=Danga&no_of_pages=1&post_type=recent_posts";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(5000);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data = json.SelectToken("data");
                        if (data.IsNotNull())
                        {

                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in data)
                            {

                                var dataStr = JsonConvert.SerializeObject(item);
                                var link = JsonConvert.DeserializeObject<string>(dataStr);
                                if (link.IsNotNull())
                                {
                                    var model = new InstagramPostViewModel();
                                    var id = link;
                                    model.url = link;
                                    model.created_date = DateTime.Now;
                                    model.keyword = keyword;
                                    var indexName = "instagram_post";
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);


                        }
                    }


                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        public async Task YoutubeDataMigration(string keyword)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);                             
                var url = "https://xtranet.aitalkx.com/social/youtube_with_sentiment?keyword=Danga&no_of_pages=1&sortby=date";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(500);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data = json.SelectToken("data");
                        if (data.IsNotNull())
                        {
                            var items = data.SelectToken("page_0");
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {
                                var id = string.Empty;
                                if (item.IsNotNull())
                                {
                                    var _id = item.SelectToken("videoid");
                                    id = _id.Value<string>();
                                }
                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<Youtube1ViewModel>(dataStr);
                                var _polarity = item.SelectToken("title_polarity");
                                var polarity = JsonConvert.SerializeObject(_polarity);
                                var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                if (polarity_model.IsNotNull())
                                {
                                    model.pos = polarity_model.pos;
                                    model.neg = polarity_model.neg;
                                    model.neu = polarity_model.neu;
                                    model.compound = polarity_model.compound;
                                }
                                if (model.IsNotNull())
                                {
                                    model.Id = id;
                                    model.keyword = "Danga";                                    
                                    var indexName = "youtube_post";
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)model.Id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }
                        
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }



        }
    }





    public class MarkerData
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string name { get; set; }
        public int count { get; set; }
    }
}
