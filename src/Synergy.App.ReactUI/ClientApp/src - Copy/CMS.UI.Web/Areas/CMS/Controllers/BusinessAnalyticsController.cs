using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CMS.Business;
using Microsoft.AspNetCore.Mvc;
using CMS.UI.ViewModel;
using CMS.Common;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Kendo.Mvc.UI;
using CMS.Data.Model;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using Syncfusion.EJ2.Base;
using System.Text;
using Npgsql;
using FastMember;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using Syncfusion.EJ2.Maps;
using System.Data;
using Nest;
using ExcelDataReader;
using CMS.UI.Utility;
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
        private readonly ITemplateBusiness _templateBusiness;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        
        public BusinessAnalyticsController(IUserContext userContext, INoteBusiness noteBusiness, ITaskBusiness taskBusiness, IWebHelper webApi,
            ITableMetadataBusiness tableMetadataBusiness, ICmsBusiness cmsBusiness, INotificationBusiness notificationBusiness, ILOVBusiness lovBusiness,
            ITemplateBusiness templateBusiness,Microsoft.Extensions.Configuration.IConfiguration configuration)
        {

            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _taskBusiness = taskBusiness;
            _webApi = webApi;
            _tableMetadataBusiness = tableMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _notificationBusiness = notificationBusiness;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _configuration = configuration;            
        }

        public async Task<IActionResult> Index()
        {
            return View();
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
            else if (model.IsNotNull() && model.Query.IsNotNullAndNotEmpty() && model.ElsasticDB)
            {
                var queryArr=model.Query.Split(" ");
                var indexName = queryArr.Last();
                var url = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration)+indexName;
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);                    
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode == true)
                    {
                        //var json = await response.Content.ReadAsStringAsync();
                        //var data = JToken.Parse(json);
                    }                    
                    else
                    {
                        return Json(new { success = false, error = "Table name not exist !" });
                    }
                }                
                return Json(new { success = true });
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
                else if(model.Query.IsNotNullAndNotEmpty() && model.SchemaName.IsNotNullAndNotEmpty() && model.ElsasticDB)
                {
                    var queryArr = model.Query.Split(" ");
                    var indexName = queryArr.Last();
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
                                                    nModel.NoteDescription = _dataType == "text" ? "string" : _dataType;
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
            var dsResult = data.ToDataSourceResult(request);
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
                    var validateNote = await _noteBusiness.GetList(x => x.NoteSubject == model.NoteSubject && x.TemplateCode == "MAP_LAYER_ITEM" && x.ParentNoteId==model.ParentNoteId);
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
                            var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
                            if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
                            {
                                timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

                            }
                            var chartInput = dashboardItem.chartMetadata;
                            var clickFunction = dashboardItem.onChartClickFunction;
                            var clickFunctionName = "On" + chartNamear[0].Replace(" ", string.Empty) + "Click";
                            if(chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
                            {
                                var metadataArray = chartInput.Split("],");
                                var filstr = "filters: [";
                                var timeDimsStr = "timeDimensions: [";
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
                                foreach (var timdim in timeDimensions)
                                {
                                    if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue == "All")
                                    {
                                        continue;
                                    }
                                    else if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = timdim.DefaultValue;
                                        var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                        timeDimsStr += fil;
                                    }                                   

                                }
                                filstr += "]";
                                timeDimsStr += "]";
                                chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + timeDimsStr;
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
                                foreach (var timdim in timeDimensions)
                                {
                                    if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                                    {
                                        var value = '"' + timdim.DefaultValue + '"';
                                        chartInput = chartInput.Replace(timdim.TimeParams, value);
                                    }

                                }

                            }
                            var chartBPCode = dashboardItem.boilerplateCode;
                            chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
                            chartBPCode = chartBPCode.Replace("@@monochrome@@", dashboardItem.MonocromeColor.IsNotNullAndNotEmpty() ? "true":"false");
                            chartBPCode = chartBPCode.Replace("@@color@@", dashboardItem.MonocromeColor);
                            chartBPCode = chartBPCode.Replace("OnSeriesClick", clickFunctionName);
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
            var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
            if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
            {
                timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

            }
            var chartInput = dashboardItem.chartMetadata;
            var clickFunction = dashboardItem.onChartClickFunction;
            var clickFunctionName = "On" + dashboardItem.NoteSubject.Replace(" ", string.Empty) + "Click";
            if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
            {
                var metadataArray = chartInput.Split("],");
                var filstr = "filters: [";
                var timeDimsStr = "timeDimensions: [";
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
                        foreach (var timdim in timeDimensions)
                        {
                            if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && timdim.DefaultValue == "All" && para[1].IsNullOrEmpty())
                            {
                                continue;
                            }
                            else if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                            {
                                var value = para[1];
                                var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                timeDimsStr += fil;
                            }
                            else if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && timdim.DefaultValue.IsNotNullAndNotEmpty())
                            {
                                var value = timdim.DefaultValue;
                                var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                timeDimsStr += fil;
                            }

                        }

                    }
                    filstr += "]";
                    timeDimsStr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + timeDimsStr;
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
                    foreach (var timdim in timeDimensions)
                    {
                        if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue == "All")
                        {
                            continue;
                        }
                        else if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                        {
                            var value = timdim.DefaultValue;
                            var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                            timeDimsStr += fil;
                        }

                    }
                    filstr += "]";
                    timeDimsStr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + timeDimsStr;
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
                        foreach (var timdim in timeDimensions)
                        {
                            if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                            {
                                var value = para[1];
                                chartInput = chartInput.Replace(timdim.TimeParams, value);
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
                foreach (var timdim in timeDimensions)
                {
                    if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                    {
                        var value = timdim.DefaultValue;
                        chartInput = chartInput.Replace(timdim.TimeParams, value);
                    }

                }

            }
            var chartBPCode = dashboardItem.boilerplateCode;
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
            var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
            if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
            {
                timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

            }
            var chartInput = dashboardItem.chartMetadata;
            var clickFunction = dashboardItem.onChartClickFunction;
            var clickFunctionName = "On" + itemName.Replace(" ", string.Empty) + "Click";
            if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
            {
                var metadataArray = chartInput.Split("],");
                var filstr = "filters: [";
                var timeDimsStr = "timeDimensions: [";
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
                        foreach (var timdim in timeDimensions)
                        {
                            if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && timdim.DefaultValue == "All" && para[1].IsNullOrEmpty())
                            {
                                continue;
                            }
                            else if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                            {
                                var value = para[1];
                                var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                timeDimsStr += fil;
                            }
                            else if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && timdim.DefaultValue.IsNotNullAndNotEmpty())
                            {
                                var value = timdim.DefaultValue;
                                var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                timeDimsStr += fil;
                            }

                        }

                    }
                    filstr += "]";
                    timeDimsStr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + timeDimsStr;
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
                    foreach (var timdim in timeDimensions)
                    {
                        if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue == "All")
                        {
                            continue;
                        }
                        else if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                        {
                            var value = timdim.DefaultValue;
                            var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                            timeDimsStr += fil;
                        }

                    }
                    filstr += "]";
                    timeDimsStr += "]";
                    chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + timeDimsStr;
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
                        foreach (var timdim in timeDimensions)
                        {
                            if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                            {
                                var value = para[1];
                                chartInput = chartInput.Replace(timdim.TimeParams, value);
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
                foreach (var timdim in timeDimensions)
                {
                    if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                    {
                        var value = timdim.DefaultValue;
                        chartInput = chartInput.Replace(timdim.TimeParams, value);
                    }

                }

            }
            var chartBPCode = dashboardItem.boilerplateCode;
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
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
                var timeDimensions = new List<DashboardItemTimeDimensionViewModel>();
                if (dashboardItem.timeDimensionsField.IsNotNullAndNotEmpty())
                {
                    timeDimensions = JsonConvert.DeserializeObject<List<DashboardItemTimeDimensionViewModel>>(dashboardItem.timeDimensionsField);

                }
                if (chartInput.Contains("^^") && dashboardItem.DynamicMetadata)
                {
                    var metadataArray = chartInput.Split("],");
                    var filstr = "filters: [";
                    var timeDimsStr = "timeDimensions: [";
                    if (param.IsNotNullAndNotEmpty())
                    {
                        var parameters = param.Split('&');
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var para = parameters[i].Split('=');
                            foreach (var filter in filters)
                            {
                                if (filter.FilterText.Contains("^^") && filter.DefaultValue == "All" && para[1].IsNullOrEmpty())
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
                            foreach (var timdim in timeDimensions)
                            {
                                if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue == "All" && para[1].IsNullOrEmpty())
                                {
                                    continue;
                                }
                                else if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                                {
                                    var value = para[1];
                                    var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                    timeDimsStr += fil;
                                }
                                else if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                                {
                                    var value = timdim.DefaultValue;
                                    var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                    timeDimsStr += fil;
                                }

                            }
                        }
                        filstr += "]";
                        timeDimsStr += "]";
                        chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr +","+ timeDimsStr;
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
                        foreach (var timdim in timeDimensions)
                        {
                            if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue == "All")
                            {
                                continue;
                            }
                            else if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                            {
                                var value = timdim.DefaultValue;
                                var fil = "{\"dimension\": \"" + timdim.TimeDimensionField + "\",\"granularity\": \"" + timdim.RangeBy + "\",\"dateRange\": [" + value + "]},";
                                timeDimsStr += fil;
                            }                            

                        }
                        filstr += "]";
                        timeDimsStr += "]";
                        chartInput = metadataArray[0] + "]," + metadataArray[1] + "]," + filstr + "," + timeDimsStr;
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
                            foreach (var timdim in timeDimensions)
                            {
                                if (timdim.TimeParams.Contains("^^") && timdim.TimeParams.Contains(para[0]) && para[1].IsNotNullAndNotEmpty())
                                {
                                    var value = para[1];
                                    chartInput = chartInput.Replace(timdim.TimeParams, value);
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
                    foreach (var timdim in timeDimensions)
                    {
                        if (timdim.TimeParams.Contains("^^") && timdim.DefaultValue.IsNotNullAndNotEmpty())
                        {
                            var value =  timdim.DefaultValue ;
                            chartInput = chartInput.Replace(timdim.TimeParams, value);
                        }

                    }
                }
                var clickFunctionName = "On" + chartName.Replace(" ", string.Empty) + "Click";
                chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
                    model.DynamicMetadata = udf.DynamicMetadata;
                    model.isLibrary = udf.isLibrary;
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
                    model.DynamicMetadata = udf.DynamicMetadata;
                    model.isLibrary = udf.isLibrary;
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
                        return Json(new { success = true, name=model.NoteSubject, id=newmodel.NoteId});
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
            var data = await _noteBusiness.GetAllDashboardMaster();
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
            var dsResult = data.ToDataSourceResult(request);
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
            var dsResult = data.ToDataSourceResult(request);
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
        public async Task<IActionResult> SocialWebsiteScrapping(string keyword, string track, string ruturnPageName, string layout, bool showIncident = false, bool hideback = false)
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
            return View();
        }
        public async Task<IActionResult> ReadTwitterData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<TwitterViewModel>();
            var url = "http://178.238.236.213:9200/twitter2/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadTwitterDataQuery1;
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
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadTwitterDataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadTwitterDataQuery3;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadTwitterDataQuery4;
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
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        var source = item.SelectToken("_source");
                        var highlights = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var strhighlight = JsonConvert.SerializeObject(highlights);
                        var result = JsonConvert.DeserializeObject<TwitterViewModel>(str);
                        var resultHighlight = JsonConvert.DeserializeObject<TwitterArrayViewModel>(strhighlight);

                        if (result.IsNotNull())
                        {
                            var abc = new TwitterViewModel
                            {
                                hashtags = (resultHighlight.IsNotNull() && resultHighlight.hashtags != null) ? string.Join("", resultHighlight.hashtags) : string.Join("", result.hashtags),
                                text = (resultHighlight.IsNotNull() && resultHighlight.text != null) ? string.Join("", resultHighlight.text) : string.Join("", result.text),
                                location = (resultHighlight.IsNotNull() && resultHighlight.location != null) ? string.Join("", resultHighlight.location) : string.Join("", result.location),
                                url = result.url,
                                count = value.ToString(),
                                created_at = result.created_at,
                            };
                            model.Add(abc);
                        }

                    }

                }
            }
            //return Json(model.ToDataSourceResult(request));
            return Json(model);

        }
        public async Task<IActionResult> ReadFacebookData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<FacebookViewModel>();
            var url = "http://178.238.236.213:9200/facebook1/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadFacebookDataQuery1;
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
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadFacebookDataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadFacebookDataQuery3;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadFacebookDataQuery4;
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
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        //var source = item.SelectToken("highlight");
                        //var str = JsonConvert.SerializeObject(source);
                        //var result = JsonConvert.DeserializeObject<FacebookArrayViewModel>(str);

                        var source = item.SelectToken("_source");
                        var highlights = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var strhighlight = JsonConvert.SerializeObject(highlights);
                        var result = JsonConvert.DeserializeObject<FacebookViewModel>(str);
                        var resultHighlight = JsonConvert.DeserializeObject<FacebookArrayViewModel>(strhighlight);

                        if (result.IsNotNull())
                        {
                            var abc = new FacebookViewModel
                            {
                                page_url = result.page_url,
                                post_url = result.post_url,
                                pagename = (resultHighlight.IsNotNull() && resultHighlight.pagename != null) ? string.Join("", resultHighlight.pagename) : string.Join("", result.pagename),
                                post_message = (resultHighlight.IsNotNull() && resultHighlight.post_message != null) ? string.Join("", resultHighlight.post_message) : string.Join("", result.post_message),
                                count = value.ToString(),
                                date_time = result.date_time,
                            };
                            model.Add(abc);
                        }

                    }

                }
            }
            //return Json(model.ToDataSourceResult(request));
            return Json(model);
        }
        public async Task<IActionResult> ReadInstagramData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<InstagramViewModel>();
            var url = "http://178.238.236.213:9200/insta/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content =ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery1;
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
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery3;
                }
                else
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadInstagramDataQuery4;
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
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        //var source = item.SelectToken("highlight");
                        //var str = JsonConvert.SerializeObject(source);
                        //var result = JsonConvert.DeserializeObject<InstagramArrayViewModel>(str);


                        var source = item.SelectToken("_source");
                        var highlights = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var strhighlight = JsonConvert.SerializeObject(highlights);
                        var result = JsonConvert.DeserializeObject<InstagramViewModel>(str);
                        var resultHighlight = JsonConvert.DeserializeObject<InstagramArrayViewModel>(strhighlight);


                        if (result.IsNotNull())
                        {
                            var abc = new InstagramViewModel
                            {
                                caption = (resultHighlight.IsNotNull() && resultHighlight.caption != null) ? string.Join("", resultHighlight.caption) : result.caption,
                                hashtags = (resultHighlight.IsNotNull() && resultHighlight.hashtags != null) ? string.Join("", resultHighlight.hashtags) : result.hashtags,
                                post_links = result.post_links,
                                image_links = result.image_links,
                                count = value.ToString(),
                                age = result.age,
                            };
                            model.Add(abc);
                        }

                    }

                }
            }
            //return Json(model.ToDataSourceResult(request));
            return Json(model);

        }
        public async Task<IActionResult> ReadYoutubeData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<YoutubeViewModel>();
            var url = "http://178.238.236.213:9200/youtube1/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery1;
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
                var content = "";
                if (plainSearch)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery2;
                }
                else if (!isAdvance)
                {
                    content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery3;
                }
                else
                {

                    content = ApplicationConstant.BusinessAnalytics.ReadYoutubeDataQuery4;
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
                    var total = data1.SelectToken("total");
                    var value = total.SelectToken("value");
                    var data2 = data1.SelectToken("hits");
                    foreach (var item in data2)
                    {
                        //var source = item.SelectToken("highlight");
                        //var str = JsonConvert.SerializeObject(source);
                        //var result = JsonConvert.DeserializeObject<YoutubeArrayViewModel>(str);


                        var source = item.SelectToken("_source");
                        var highlights = item.SelectToken("highlight");
                        var str = JsonConvert.SerializeObject(source);
                        var strhighlight = JsonConvert.SerializeObject(highlights);
                        var result = JsonConvert.DeserializeObject<YoutubeViewModel>(str);
                        var resultHighlight = JsonConvert.DeserializeObject<YoutubeArrayViewModel>(strhighlight);

                        if (result.IsNotNull())
                        {
                            var abc = new YoutubeViewModel
                            {
                                title = (resultHighlight.IsNotNull() && resultHighlight.title != null) ? string.Join("", resultHighlight.title) : string.Join("", result.title),
                                description = (resultHighlight.IsNotNull() && resultHighlight.description != null) ? string.Join("", resultHighlight.description) : string.Join("", result.description),
                                youtube_video_url = result.youtube_video_url,
                                count = value.ToString(),
                                publishtime = result.publishtime,
                            };
                            model.Add(abc);
                        }

                    }

                }
            }

            //return Json(model.ToDataSourceResult(request));
            return Json(model);

        }
        public async Task<IActionResult> ReadWhatsappData(string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<WhatsAppViewModel>();
            var url = "http://178.238.236.213:9200/whatsapp1/_search";

            if (searchStr.IsNullOrEmpty())
            {
                var content =ApplicationConstant.BusinessAnalytics.ReadWhatsappDataQuery1;
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
            return Json(model.ToDataSourceResult(request));

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
            return Json(model.ToDataSourceResult(request));
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
            return Json(model.ToDataSourceResult(request));

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

            return Json(model.ToDataSourceResult(request));

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

            return Json(model.ToDataSourceResult(request));

        }
        //public async Task<IActionResult> ReadFacebookData2([DataSourceRequest] DataSourceRequest request, string searchStr)
        //{
        //    var model = new List<ElasticSerachViewModel>();
        //    var url = "http://178.238.236.213:9200/esdb-doc-index/_search";

        //    if (searchStr.IsNullOrEmpty())
        //    {
        //        var content = @"{""query"": {""match_all"": { } } }";
        //        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
        //        using (var httpClient = new HttpClient())
        //        {
        //            var address = new Uri(url);
        //            //var response = await httpClient.GetAsync(address);
        //            var response = await httpClient.PostAsync(address, stringContent);
        //            var json = await response.Content.ReadAsStringAsync();


        //            var data = JToken.Parse(json);
        //            var data1 = data.SelectToken("hits");
        //            var data2 = data1.SelectToken("hits");
        //            foreach (var item in data2)
        //            {
        //                var source = item.SelectToken("_source");
        //                var str = JsonConvert.SerializeObject(source);
        //                var result = JsonConvert.DeserializeObject<ElasticSerachViewModel>(str);
        //                result.versionNo = "0";
        //                model.Add(result);
        //            }

        //        }
        //    }
        //    else
        //    {
        //        var content = @"{""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""metadata"", ""subject"", ""description"" ],""query"":""#SEARCHWHERE#"",""fuzziness"":""AUTO""}}}},""highlight"":{""fields"":{""metadata"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""subject"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2},""description"":{""pre_tags"" : [""<em>""], ""post_tags"" : [""</em>""] ,""fragment_size"" : 1000, ""number_of_fragments"" : 2}}}}";
        //        content = content.Replace("#SEARCHWHERE#", searchStr);
        //        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
        //        using (var httpClient = new HttpClient())
        //        {
        //            var address = new Uri(url);
        //            //var response = await httpClient.GetAsync(address);
        //            var response = await httpClient.PostAsync(address, stringContent);
        //            var json = await response.Content.ReadAsStringAsync();


        //            var data = JToken.Parse(json);
        //            var data1 = data.SelectToken("hits");
        //            var total = data1.SelectToken("total");
        //            var value = total.SelectToken("value");
        //            var data2 = data1.SelectToken("hits");
        //            foreach (var item in data2)
        //            {
        //                var source = item.SelectToken("highlight");
        //                var str = JsonConvert.SerializeObject(source);
        //                var result = JsonConvert.DeserializeObject<ElasticSerach1ViewModel>(str);
        //                if (result.IsNotNull())
        //                {
        //                    var abc = new ElasticSerachViewModel { subject = result.subject != null ? string.Join("", result.subject) : "", description = result.description != null ? string.Join("", result.description) : "", metadata = result.metadata != null ? string.Join("", result.metadata) : "", versionNo = value.ToString() };
        //                    model.Add(abc);
        //                }

        //            }

        //        }
        //    }
        //    //var abc=JsonConvert.SerializeObject(content);

        //    return Json(model.ToDataSourceResult(request));
        //    //var settings = new ConnectionSettings(new Uri("http://178.238.236.213:9200/")).DefaultIndex("esdb-doc-index");

        //    //var client = new ElasticClient(settings);
        //    //var asyncIndexResponse = await client.IndexAsync(,"esdb-doc-index");
        //    //var list = await _noteBusiness.GetFacebookData(searchStr);
        //    //return Json(list.ToDataSourceResult(request));
        //}
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
        public async Task<IActionResult> ManageSocialMediaHighlight(string keyword)
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

        public async Task<IActionResult> CCTVDashboard()
        {
            return View();
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
                                else if (mt.StartsWith("System.Nullable`1[[CMS.Common"))
                                {
                                    var splits = member.Type.FullName.Split(',');
                                    if (splits.Length > 0)
                                    {
                                        var typText = $"{splits[0].Replace("System.Nullable`1[[", "")}, CMS.Common";
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
            var list = new List<Dial100Viewmodel>();
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
                            list.Add(new Dial100Viewmodel
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
            var model = new List<Dial100Viewmodel>();
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
                                var result = JsonConvert.DeserializeObject<Dial100Viewmodel>(str);
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
                                var result = JsonConvert.DeserializeObject<Dial100Viewmodel>(str);
                                var resultHighlight = JsonConvert.DeserializeObject<Dial100ArrayViewmodel>(strhighlight);

                                if (result.IsNotNull())
                                {

                                    var sres = new Dial100Viewmodel
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

            var content =ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery1;
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

            var content = ApplicationConstant.BusinessAnalytics.ReadCamera1DataQuery2;
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
        public async Task<IActionResult> CCTVMonitor()
        {
            var data = await ReadCamera1Data();
            var groupData = data.GroupBy(x => new { x.Latitude, x.Longitude });
            var markers1 = new List<MapsMarker>();
            foreach (var gdata in groupData)
            {
                var ldata = gdata.ToList();
                var sdata = "";
                foreach (var l in ldata)
                {
                    sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
                }

                //var jsonObj = JsonConvert.SerializeObject(gdata);
                var marker = new MapsMarker
                {
                    Visible = true,
                    DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
                    Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
                    TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
                };
                markers1.Add(marker);
            }
            ViewBag.MarkersCamera1 = markers1.ToList();
            var data1 = await ReadCamera2Data();

            var groupData2 = data1.GroupBy(x => new { x.Latitude, x.Longitude });
            var markers2 = new List<MapsMarker>();
            foreach (var gdata in groupData2)
            {
                var ldata = gdata.ToList();
                var sdata = "";
                foreach (var l in ldata)
                {
                    sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
                }

                //var jsonObj = JsonConvert.SerializeObject(gdata);
                var marker = new MapsMarker
                {
                    Visible = true,
                    DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
                    Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
                    TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
                };
                markers2.Add(marker);
            }
            ViewBag.MarkersCamera2 = markers2.ToList();
            return View();
        }

        public async Task<IActionResult> CCTVMonitorPartial(string stationName, string loc)
        {
            var data = await ReadCamera1Data();
            var groupData = data.GroupBy(x => new { x.Latitude, x.Longitude });
            var markers1 = new List<MapsMarker>();
            foreach (var gdata in groupData)
            {
                var ldata = gdata.ToList();
                var sdata = "";
                foreach (var l in ldata)
                {
                    sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
                }

                //var jsonObj = JsonConvert.SerializeObject(gdata);
                var marker = new MapsMarker
                {
                    Visible = true,
                    DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
                    Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
                    TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
                };
                markers1.Add(marker);
            }
            ViewBag.MarkersCamera1 = markers1.ToList();
            var groupData2 = data.GroupBy(x => new { x.Latitude, x.Longitude });
            var markers2 = new List<MapsMarker>();
            foreach (var gdata in groupData2)
            {
                var ldata = gdata.ToList();
                var sdata = "";
                foreach (var l in ldata)
                {
                    sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
                }

                //var jsonObj = JsonConvert.SerializeObject(gdata);
                var marker = new MapsMarker
                {
                    Visible = true,
                    DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
                    Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
                    TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
                };
                markers2.Add(marker);
            }
            ViewBag.MarkersCamera2 = markers2.ToList();
            return PartialView("CCTVMonitorPartial");
        }


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

        public async Task<IActionResult> SynergyFundingMapPartial()
        {
            var markers = new List<MapsMarker>();

            var marker = new MapsMarker
            {
                Visible = true,
                DataSource = new[] { new { name = "New York", latitude = 40.7128, longitude = 74.0060 } },
                TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
            };
            markers.Add(marker);
            var marker2 = new MapsMarker
            {
                Visible = true,
                DataSource = new[] { new { name = "Jersey City", latitude = 40.7178, longitude = 74.0431 } },
                TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
            };
            markers.Add(marker2);

            var marker3 = new MapsMarker
            {
                Visible = true,
                DataSource = new[] { new { name = "London", latitude = 51.5074, longitude = 0.1278 } },
                TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
            };
            markers.Add(marker3);
            var marker4 = new MapsMarker
            {
                Visible = true,
                DataSource = new[] { new { name = "Toronto", latitude = 43.6532, longitude = 79.3832 } },
                TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
            };
            markers.Add(marker4);
            ViewBag.Markers = markers.ToList();
            return View();
        }
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
                chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
                chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
            chartBPCode = chartBPCode.Replace("@@input@@", chartInput);
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
        public async Task<IActionResult> ReadTimesOfIndiaNewsFeed(/*[DataSourceRequest] DataSourceRequest request,*/ string searchStr, bool isAdvance = false, bool plainSearch = false, SocialMediaDatefilters dateFilterType = SocialMediaDatefilters.AllTime, string startDate = null, string endDate = null)
        {
            var model = new List<NewsFeedsViewModel>();
            var url = "http://178.238.236.213:9200/rssfeeds/_search?pretty=true";

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
                                link = result.link,
                                published = result.published,
                            };

                            model.Add(sres);
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

        public async Task<IActionResult> LocationDashboard()
        {
            //await UpdateLocationCountUsingHangfire();
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
                    var content = ApplicationConstant.BusinessAnalytics.LocationDashboardDataQuery1;
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

            var url = "..\\CMS.UI.Web\\wwwroot\\js\\CMS\\mapJson\\indiamap.json";
            string usmap = System.IO.File.ReadAllText(url);
            ViewBag.usmap = JsonConvert.DeserializeObject(usmap);
            var markers1 = new List<MapsMarker>();
            var data = await _noteBusiness.GetDistictByState("fe94e3c0-d311-4a89-a96a-67e249881d09");
            foreach (var d in data)
            {
                int count = Convert.ToInt32(d.Count);
                var stations = await _noteBusiness.GetPoliceStationByDistict(d.Id);
                foreach (var station in stations)
                {
                    count += Convert.ToInt32(station.Count);
                    var locations = await _noteBusiness.GetLocationByPoliceStation(station.Id);
                    foreach (var location in locations)
                    {
                        count += Convert.ToInt32(location.Count);
                    }
                }

                //var sub = d.Name;
                //var i = 0;
                //try
                //{
                //    foreach (var obj in dync)
                //    {
                //        var souraceJson = JsonConvert.SerializeObject(obj);                       
                //        if (souraceJson.ToLower().Contains(sub.ToLower()))
                //        {
                //            i++;
                //        }

                //    }
                //}
                //catch (Exception ex)
                //{


                //}

                var marker = new MapsMarker
                {
                    Visible = true,
                    DataSource = new[] { new { name = d.Name, latitude = d.Latitude, longitude = d.Longitude } },
                    Template = "<div class='marker-event' onclick=onMarkerClick('" + d.Id + "','" + d.Name + "')><div class='pin bounce'><span class='pin-label'>" + d.Name + " " + count + "</span></div><div class='pulse'></div></div>",
                    TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
                };
                markers1.Add(marker);
            }
            if (data.IsNotNull())
            {
                ViewBag.StateName = data[0].State;
            }
            ViewBag.District = markers1;

            //Code for keyword Pai Chart
            foreach (var track in tracks)
            {
                var keywordModel = new List<TrackChartViewModel>();
                var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
                var query = "";
                foreach (var keyword in keywords)
                {

                    var content = ApplicationConstant.BusinessAnalytics.LocationDashboardDataQuery2;
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
                            var total = hits.SelectToken("total");
                            var value = total.SelectToken("value");
                            keywordModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Keyword = keywords[i], Count = value.ToString() });
                            i++;
                        }
                    }
                    ViewData[track.NoteSubject] = keywordModel;
                }

            }
            ViewBag.dataSource = trackModel;

            return View();
        }

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
            //var tasks = await _taskBusiness.GetList(x => x.TemplateCode == "INCIDENT");
            //var search = new TaskSearchViewModel { UserId = _userContext.UserId };
            //var tasks = await _taskBusiness.GetSearchResult(search);
            //ViewBag.InProgressCount = tasks.Where(x => x.TemplateMasterCode == "INCIDENT" & (x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE")).Count();
            //ViewBag.InCompletedCount = tasks.Where(x => x.TemplateMasterCode == "INCIDENT" & x.TaskStatusCode == "TASK_STATUS_COMPLETE").Count();

            //var locationId = "6823236a-3376-4829-9a2f-0e5001534ae6";
            //var districtName = "Dewas";

            //var url = "..\\CMS.UI.Web\\wwwroot\\js\\CMS\\mapJson\\indiamap.json";
            //string usmap = System.IO.File.ReadAllText(url);
            //ViewBag.usmap = JsonConvert.DeserializeObject(usmap);
            //var markers1 = new List<MarkerData>();
            //var data = await _noteBusiness.GetMarkersByDistrict(locationId);
            //foreach (var d in data)
            //{
            //    int count = Convert.ToInt32(d.Count);
            //    var locations = await _noteBusiness.GetLocationByPoliceStation(d.Id);
            //    foreach (var location in locations)
            //    {
            //        count += Convert.ToInt32(location.Count);
            //    }

            //    markers1.Add(new MarkerData { name = d.Name, latitude = Convert.ToDouble(d.Latitude), longitude = Convert.ToDouble(d.Longitude), count = count });
            //}

            ////Code for track Pai Chart
            //var tracks = await _noteBusiness.GetList(x => x.TemplateCode == "TRACK_MASTER");
            //var trackModel = new List<TrackChartViewModel>();

            //foreach (var track in tracks)
            //{
            //    var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
            //    if (keywords.Count > 0)
            //    {
            //        var dync = new List<dynamic>();
            //        var searchStr = string.Join(" ", keywords);
            //        var content = ApplicationConstant.BusinessAnalytics.HomeDataQuery1;
            //        content = content.Replace("#SEARCHWHERE#", searchStr);
            //        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            //        using (var httpClient = new HttpClient())
            //        {
            //            var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,whatsapp1,insta,twitter2,dial_test,test_camera1,test_camera2/_search?pretty=true";
            //            var address = new Uri(url1);
            //            var response = await httpClient.PostAsync(address, stringContent);
            //            var jsontrack = await response.Content.ReadAsStringAsync();


            //            var trackdata = JToken.Parse(jsontrack);
            //            var trackdata1 = trackdata.SelectToken("hits");
            //            var total = trackdata1.SelectToken("total");
            //            var value = total.SelectToken("value");
            //            if (trackdata1.IsNotNull())
            //            {
            //                var hits = trackdata1.SelectToken("hits");
            //                foreach (var hitsItem in hits)
            //                {
            //                    var source = hitsItem.SelectToken("_source");
            //                    var souraceJson = JsonConvert.SerializeObject(source);
            //                    var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
            //                    //var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
            //                    dync.Add(result);

            //                }
            //            }
            //        }
            //        var policeData = await _noteBusiness.GetMarkersByDistrict(locationId);
            //        var i = 0;
            //        foreach (var d in policeData)
            //        {
            //            var sub = d.Name;
            //            try
            //            {
            //                foreach (var obj in dync)
            //                {
            //                    var souraceJson = JsonConvert.SerializeObject(obj);
            //                    if (souraceJson.ToLower().Contains(sub.ToLower()))
            //                    {
            //                        i++;
            //                    }

            //                }
            //                var locations = await _noteBusiness.GetLocationByPoliceStation(d.Id);
            //                foreach (var location in locations)
            //                {
            //                    foreach (var obj1 in dync)
            //                    {
            //                        var souraceJson = JsonConvert.SerializeObject(obj1);
            //                        if (souraceJson.ToLower().Contains(location.Name.ToLower()))
            //                        {
            //                            i++;
            //                        }

            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {


            //            }

            //        }
            //        trackModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Count = i.ToString() });
            //    }
            //}
            //if (data.Count > 0)
            //{
            //    ViewBag.StateName = data[0].State;
            //    ViewBag.DistrictName = data[0].District;
            //}
            //else
            //{
            //    ViewBag.StateName = "Madhya Pradesh";
            //    ViewBag.DistrictName = districtName;
            //}
            //ViewBag.District = markers1;
            //ViewBag.dataSource = trackModel;
            ////await CreateNotificationForSocialMediaUsingHangfire();           
            return View();
        }
        public async Task<IActionResult> HomeAlert(string msg)
        {
            ViewBag.Msg = msg;
            return View();
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
                                        var resultHighlight = JsonConvert.DeserializeObject<Dial100ArrayViewmodel>(strhighlight);
                                        var result = JsonConvert.DeserializeObject<Dial100Viewmodel>(souraceJson);
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

        public async Task<IActionResult> DewasCCTVCamera1()
        {
            var data = await ReadCamera1Data();
            var groupData = data.GroupBy(x => new { x.Latitude, x.Longitude });
            var markers1 = new List<MapsMarker>();
            foreach (var gdata in groupData)
            {
                var ldata = gdata.ToList();
                var sdata = "";
                foreach (var l in ldata)
                {
                    sdata += l.LocationName.Replace(" ", "%#$%") + ":%#$%" + l.CameraName.Replace(" ", "%#$%") + "," + l.IpAddress.ToString().Replace(" ", "%#$%") + "|";
                }
                var marker = new MapsMarker
                {
                    Visible = true,
                    DataSource = new[] { new { name = ldata[0].LocationName + ":" + ldata[0].CameraName, latitude = ldata[0].Latitude, longitude = ldata[0].Longitude } },
                    Template = "<div class='marker-event' onclick=onMarkerClick('" + sdata + "')><div class='pin bounce'><span class='pin-label'>" + gdata.Count() + "</span></div><div class='pulse'></div></div>",
                    TooltipSettings = new MapsTooltipSettings { Visible = true, ValuePath = "name" },
                };
                markers1.Add(marker);
            }
            ViewBag.MarkersCamera1 = markers1.Take(1).ToList();
            return View();
        }
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
        public async Task<IActionResult> GridStack()
        {
            return View();
        }
        public async Task<IActionResult> GridStackItem(string id)
        {
            ViewBag.Id = id;
            ViewBag.backEnable = true;
            var model = new DashboardMasterViewModel { ParentNoteId = id,gridStack=true };
            model.layoutMetadata = "{}";
            return View(model);
        }
        public async Task<IActionResult> ManageGridStackItemTitle(string name, string id)
        {
            ViewBag.Name = name;
            ViewBag.Id = id;
            return View();
        }
        public async Task<IActionResult> onEditGridStackItem(string id, bool isEditable=false)
        {
            ViewBag.Id = id;
            ViewBag.IsEditable = isEditable;
            ViewBag.backEnable = true;
            var data = await _noteBusiness.GetAllGridStackDashboard();
            var item = data.FirstOrDefault(i => i.Id == id);
            item.ParentNoteId = id;
            item.gridStack = true;
            if (item.layoutMetadata != null && isEditable==false) {
                List<JObject> metadata = JsonConvert.DeserializeObject<List<JObject>>(item.layoutMetadata);
                for (int i = 0; i<metadata.Count; i++)
                {
                    metadata[i]["noMove"] = "true";
                    metadata[i]["noResize"] = "true";
                    metadata[i]["locked"] = "true";
                }
                item.layoutMetadata = Newtonsoft.Json.JsonConvert.SerializeObject(metadata);
                //item.layoutMetadata = item.layoutMetadata.Replace("dummy", "hidden");
            }

            item.layoutMetadata = item.layoutMetadata.IsNullOrEmpty() ?"{}":item.layoutMetadata.Replace("^","'");
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
                        return Json(new { success = true , result = result});
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
        public async Task<IActionResult> GridStackPage(string id,string parentId, DataActionEnum dataAction)
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
                return Json(new {res= result,charts= chartItems });
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
       
    }
    public class MarkerData
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string name { get; set; }
        public int count { get; set; }
    }
}
