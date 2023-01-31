using System;
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
using Humanizer;
//using ExcelDataReader;


namespace CMS.UI.Web.Areas.CMS.Controllers


{
    //[Authorize]
    [Area("Cms")]

    public class IIPPHQController : ApplicationController
    {

        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly IIipBusiness _iipBusiness;
        private readonly IWebHelper _webApi;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public IIPPHQController(IUserContext userContext, INoteBusiness noteBusiness, IIipBusiness iipBusiness, IWebHelper webApi,
            ITableMetadataBusiness tableMetadataBusiness, ICmsBusiness cmsBusiness, INotificationBusiness notificationBusiness, ILOVBusiness lovBusiness,
            ITemplateBusiness templateBusiness, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {

            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _iipBusiness = iipBusiness;
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
        public async Task<IActionResult> ReportItem(string id)
        {
            ViewBag.Id = id;
            ViewBag.backEnable = true;
            var model = new DashboardMasterViewModel { ParentNoteId = id, gridStack = true };
            model.layoutMetadata = "{}";
            return View(model);
        }
        public async Task<IActionResult> EditReportItem(string id, bool isEditable = false, bool canback = false)
        {
            ViewBag.Id = id;
            ViewBag.IsEditable = isEditable;
            ViewBag.backEnable = canback;
            var item = await _iipBusiness.GetReportDashboard(id);
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
            }
            item.layoutMetadata = item.layoutMetadata.IsNullOrEmpty() ? "{}" : item.layoutMetadata.Replace("^", "'");
            return View("ReportItem", item);
        }
        public async Task<IActionResult> ReportView(string id)
        {
            ViewBag.Id = id;           
            var item = await _iipBusiness.GetReportDashboard(id);
            item.ParentNoteId = id;
            item.gridStack = true;
            if (item.layoutMetadata != null)
            {
                List<JObject> metadata = JsonConvert.DeserializeObject<List<JObject>>(item.layoutMetadata);
                for (int i = 0; i < metadata.Count; i++)
                {
                    metadata[i]["noMove"] = "true";
                    metadata[i]["noResize"] = "true";
                    metadata[i]["locked"] = "true";
                }
                item.layoutMetadata = Newtonsoft.Json.JsonConvert.SerializeObject(metadata);
            }
            item.layoutMetadata = item.layoutMetadata.IsNullOrEmpty() ? "{}" : item.layoutMetadata.Replace("^", "'");
            return View(item);
        }

        public async Task<IActionResult> ManageReport(string id, DataActionEnum dataAction)
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
        public async Task<IActionResult> ManageReport(DashboardMasterViewModel model)
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
                    model.gridStack = false;
                    model.isReportDashboard = true;
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
                    model.gridStack = false;
                    model.isReportDashboard = true;
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
        public async Task<IActionResult> ReportDashboardItem(string id, string parentId, DataActionEnum dataAction, string layout)
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
        public async Task<IActionResult> ManageReportDashboardItem(DashboardItemMasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filterList = new List<DashboardItemFilterViewModel>();
                if (model.DataAction == DataActionEnum.Create)
                {
                    if (model.NoteSubject.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Name is Required" });
                    }
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
                    if (model.NoteSubject.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Name is Required" });
                    }
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
        public async Task<IActionResult> ReportDashboardlayoutMetaData(string data, string id)
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
        public async Task<JsonResult> GetCommonChartTemplateList()
        {
            var data = await _iipBusiness.GetAllCommonChartTemplate();
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
        public async Task<IActionResult> LoadIframePage(string src)
        {
            ViewBag.SRC = src;
            return View();
        }
        
        public async Task<IActionResult> ManageGridStackItemTitle(string name, string id)
        {
            ViewBag.Name = name;
            ViewBag.Id = id;
            return View();
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
            var data = await _iipBusiness.GetAllReportDashboard();
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetLibraryDashboardItemList()
        {
            var data = await _noteBusiness.GetLibraryDashboardItemDetailsWithDashboard();
            return Json(data);
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
                        cubejsquery += "{ member: '" + f + "',operator:'" + o + "',values: ['" + v + "'],},";

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
        public async Task<JsonResult> ReadDashboardItemlistData(string parentId)
        {
            var list = await _noteBusiness.GetDashboardItemMasterList(parentId);            
            return Json(list);
        }
        public async Task<IActionResult> CctvDashboard()
        { 
            return View();
        }
        public async Task<IActionResult> ReadCctvData()
        {
            var list = await _noteBusiness.GetIIPCamera();
            return Json(list);
        }
        public async Task<IActionResult> CctvCardView(string ids)
        {
            //ViewBag.Ids = ids;
            var newIds = ids.Replace(";", "','");
            var list = await _noteBusiness.GetIIPCameraByIds(newIds);
            ViewBag.CameraList = list;
            return View();
        }
        public async Task<IActionResult> ReadCctvCardData(string ids)
        {
            var newIds = ids.Replace(";", "','");
            var list = await _noteBusiness.GetIIPCameraByIds(newIds);
            return Json(list);
        }
        public async Task<IActionResult> CubeJsDynamicReport()
        {
            var model = new DynamicReportViewModel();            
            return View(model);

        }
        [HttpGet]
        public async Task<JsonResult> GetOrderDimensionsColumnData(string measure)
        {
            var data = await _noteBusiness.GetDimensionsColumnByMeasue(measure);
            foreach (var item in data)
            {
                string[] op = { "asc", "desc", "none" };
                item.operators = op;
                item.label = item.label.Titleize();
            }
            return Json(data);
        }
        public async Task<IActionResult> RenderCubeJsDynamicReport(DynamicReportViewModel model)
        {
            model.MetaDeta = @$"""measures"": [],""dimensions"": [],""filters"": [],""timeDimensions"":[],""order"": []";
            if (model.measuresField.IsNotNullAndNotEmpty())
            {
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = @$"""measures"": [""{ model.measuresField }""],{ metadataArray[1] }],{ metadataArray[2]}],{ metadataArray[3]}],{ metadataArray[4]}";
            }            
            if (model.dimensionsField.IsNotNullAndNotEmpty())
            {
                var df = JsonConvert.DeserializeObject(model.dimensionsField);
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = @$"{metadataArray[0]}],""dimensions"": {df},{metadataArray[2]}],{ metadataArray[3]}],{ metadataArray[4]}";
            }
            if (model.filters.IsNotNullAndNotEmpty())
            {
                var qr = JsonConvert.DeserializeObject<dynamic>(model.filters);
                var con = qr.condition;
                var op = con.Value;
                var rules = qr.rules;
                var cubejsquery = "{";
                cubejsquery = await CreateFilterJson(rules, op, cubejsquery);
                cubejsquery += "}";
                JObject json = JObject.Parse(cubejsquery);
                string jsonStr = JsonConvert.SerializeObject(json);
                string jsonFormatted = JValue.Parse(jsonStr).ToString(Newtonsoft.Json.Formatting.Indented);                
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = @$"{metadataArray[0]}],{ metadataArray[1]}],""filters"": [{ jsonFormatted}],{ metadataArray[3]}],{ metadataArray[4]}";

            }
            else
            {
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = $@"{metadataArray[0]}],{ metadataArray[1]}],""filters"": [],{ metadataArray[3]}],{ metadataArray[4]}";
            }            
            if (model.timeField.IsNotNullAndNotEmpty())
            {
                if(model.rangeType== "AllTime")
                {
                    var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#""}";
                    timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeField);
                    timeQuery = timeQuery.Replace("#GRANULARITY#", "day");                   
                    JObject timejson = JObject.Parse(timeQuery);
                    string timejsonStr = JsonConvert.SerializeObject(timejson);
                    string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                    var metadataArray = model.MetaDeta.Split("],");
                    model.MetaDeta = $@"{metadataArray[0]}],{metadataArray[1]}],{metadataArray[2]}],""timeDimensions"": [{timejsonFormatted}],{metadataArray[4]}";
                }
                else
                {
                    var timeQuery = @"{""dimension"":""#COLUMNNAME#"",""granularity"": ""#GRANULARITY#"",""dateRange"": ""#DURATION#""}";
                    timeQuery = timeQuery.Replace("#COLUMNNAME#", model.timeField);
                    timeQuery = timeQuery.Replace("#GRANULARITY#", "day");
                    timeQuery = timeQuery.Replace("#DURATION#", model.rangeType);
                    JObject timejson = JObject.Parse(timeQuery);
                    string timejsonStr = JsonConvert.SerializeObject(timejson);
                    string timejsonFormatted = JValue.Parse(timejsonStr).ToString(Newtonsoft.Json.Formatting.Indented);
                    var metadataArray = model.MetaDeta.Split("],");
                    model.MetaDeta = $@"{metadataArray[0]}],{metadataArray[1]}],{metadataArray[2]}],""timeDimensions"": [{timejsonFormatted}],{metadataArray[4]}";

                }
                              
            }
            else
            {
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = $@"{metadataArray[0]}],{metadataArray[1]}],{metadataArray[2]}],""timeDimensions"": [],{ metadataArray[4]}";
            }            
            if (model.orders.IsNotNullAndNotEmpty())
            {
                var qr = JsonConvert.DeserializeObject<dynamic>(model.orders);
                var con = qr.condition;
                var op = con.Value;
                var rules = qr.rules;
                var cubejsOrders = "";
                cubejsOrders = await CreateOrderJson(rules, cubejsOrders);
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = $@"{metadataArray[0]}],{ metadataArray[1]}],{ metadataArray[2]}],{metadataArray[3]}],""order"":[{ cubejsOrders}]";
                

            }
            else
            {
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = $@"{metadataArray[0]}],{metadataArray[1]}],{metadataArray[2]}],{metadataArray[3]}],{metadataArray[4]}";                
            }
            if (model.limit.IsNotNullAndNotEmpty())
            {
                var metadataArray = model.MetaDeta.Split("],");
                model.MetaDeta = $@"{metadataArray[0]}],{metadataArray[1]}],{metadataArray[2]}],{metadataArray[3]}],{metadataArray[4]},""limit"":{model.limit}";
            }
            JObject jStr = JObject.Parse("{"+model.MetaDeta+"}");
            ViewBag.Query = JsonConvert.SerializeObject(jStr);
            return View(model);
        }

        private async Task<string> CreateFilterJson(dynamic rules, string condtion, string cubejsquery)
        {
            cubejsquery += condtion.ToLower() + ": [";
            foreach (var rule in rules)
            {
                var con = rule.condition;
                if (con != null)
                {
                    var op = con.Value;
                    cubejsquery += "{";
                    cubejsquery = await CreateFilterJson(rule.rules, op, cubejsquery);
                    cubejsquery += "},";

                }
                else
                {
                    var f = rule["field"].Value;
                    var o = rule["operator"].Value;
                    var v = rule["value"].Value;                   
                    var para = v;                    
                    var model = new DashboardItemFilterViewModel
                    {
                        FilterField = f,
                        FilterOperator = o,
                        FilterText = para,                        
                    };                    
                    cubejsquery += "{ 'member': '" + f + "','operator':'" + o + "','values': ['" + para + "'],},";
                }
            }
            cubejsquery += "],";
            return cubejsquery;
        }
        private async Task<string> CreateOrderJson(dynamic rules, string cubejsquery)
        {
            
            foreach (var rule in rules)
            {  
                var f = rule["field"].Value;
                var o = rule["operator"].Value;
                if (o != "none")
                {
                    cubejsquery += "['" + f + "','" + o + "'],";
                }

            }            
            return cubejsquery.Trim(',');
        }
        [HttpGet]
        public async Task<JsonResult> GetMeasuresData()
        {
            var data = await _noteBusiness.GetMeasuresDisplay();
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetDimensionsData(string measure)
        {
            var data = await _noteBusiness.GetDimensionsByMeasueDisplay(measure);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetTimeDimensionsData(string measure)
        {
            var data = await _noteBusiness.GetDimensionsByMeasueDisplay(measure);
            data = data.Where(x => x.dataType == "DateTime").ToList();
            data = data.Select(c => { c.title = c.title.Titleize(); return c; }).ToList();
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
                item.label = item.label.Titleize();
            }
            return Json(data);
        }        
    }
}
