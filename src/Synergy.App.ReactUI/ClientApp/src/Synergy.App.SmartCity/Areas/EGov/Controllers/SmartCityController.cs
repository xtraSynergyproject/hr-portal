using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
using Synergy.App.Common;
using Synergy.App.Business;
using Synergy.App.DataModel;
using System.Data;
using Synergy.App.WebUtility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using Nancy.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using static Nest.JoinField;
using DocumentFormat.OpenXml.Bibliography;
using System.Collections;
using Nancy;
using ProtoBuf.Meta;
using Elasticsearch.Net.Specification.CatApi;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Transactions;
using static MongoDB.Driver.WriteConcern;
using DocumentFormat.OpenXml.Office.CustomDocumentInformationPanel;

namespace Synergy.App.Areas.EGov.Controllers
{
    [Area("EGov")]
    public class SmartCityController : ApplicationController
    {
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ISmartCityBusiness _smartCityBusiness;
        ICompanySettingBusiness _companySettingBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IEGovernanceBusiness _eGovernanceBusiness;
        private readonly IStepTaskEscalationDataBusiness _stepTaskEscalationDataBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly INtsServiceCommentBusiness _ntsServiceCommentBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public SmartCityController(ITemplateBusiness templateBusiness
            , IServiceBusiness serviceBusiness
            , IStepTaskEscalationDataBusiness stepTaskEscalationDataBusiness
            , IEGovernanceBusiness eGovernanceBusiness
            , IUserContext userContext, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, ICmsBusiness cmsBusiness
            , ISmartCityBusiness smartCityBusiness, IPortalBusiness portalBusiness, ICompanySettingBusiness companySettingBusiness,
            ILOVBusiness lOVBusiness, IUserBusiness userBusiness, INtsServiceCommentBusiness _ntsServiceCommentBusiness,
            AuthSignInManager<ApplicationIdentityUser> customUserManager)
        {
            _templateBusiness = templateBusiness;
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
            _smartCityBusiness = smartCityBusiness;
            _portalBusiness = portalBusiness;
            _companySettingBusiness = companySettingBusiness;
            _stepTaskEscalationDataBusiness = stepTaskEscalationDataBusiness;
            _eGovernanceBusiness = eGovernanceBusiness;
            _lOVBusiness = lOVBusiness;
            _userBusiness = userBusiness;
            _customUserManager = customUserManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> JSCMapView()
        {

            return View();
        }
        public IActionResult GrievanceWorkflowIndex()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartmentList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("JSC_Department", "", "");
            return Json(data);

        }
        public async Task<IActionResult> GetGrievanceWorkflowList()
        {
            var data = await _smartCityBusiness.GetGrievanceWorkflowList();
            return Json(data);

        }

        public IActionResult JSCDashboard()
        {
            return View();

        }
        public async Task<IActionResult> GrievanceWorkflow(string id = null)
        {
            JSCGrievanceWorkflow model = new();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _smartCityBusiness.GetGrievanceWorkflowById(id);
                model.DataAction = DataActionEnum.Edit;
                model.WardIds = model.WardId.Trim('{').Trim('}').Split(",").ToArray();

                if (model.WorkflowLevelId.IsNotNullAndNotEmpty())
                {
                    var wkcode = await _lOVBusiness.GetSingle(x => x.Id == model.WorkflowLevelId);
                    model.WorkflowLevelId = wkcode.Code;
                }
                if (model.Level1AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Id == model.Level1AssignedToTypeId);
                    model.Level1AssignedToTypeId = type1.Code;
                }
                if (model.Level2AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Id == model.Level2AssignedToTypeId);
                    model.Level2AssignedToTypeId = type1.Code;
                }
                if (model.Level3AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Id == model.Level3AssignedToTypeId);
                    model.Level3AssignedToTypeId = type1.Code;
                }
                if (model.Level4AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Id == model.Level4AssignedToTypeId);
                    model.Level4AssignedToTypeId = type1.Code;
                }

                if (model.Level1AssignedToTeamId.IsNotNullAndNotEmpty())
                {
                    model.Level1AssignedToTeamUserId = model.Level1AssignedToUserId;
                }
                if (model.Level2AssignedToTeamId.IsNotNullAndNotEmpty())
                {
                    model.Level2AssignedToTeamUserId = model.Level2AssignedToUserId;
                }
                if (model.Level3AssignedToTeamId.IsNotNullAndNotEmpty())
                {
                    model.Level3AssignedToTeamUserId = model.Level3AssignedToUserId;
                }
                if (model.Level4AssignedToTeamId.IsNotNullAndNotEmpty())
                {
                    model.Level4AssignedToTeamUserId = model.Level4AssignedToUserId;
                }

            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }

            return View(model);
        }

        public async Task<IActionResult> ManageGrievanceWorkflow(JSCGrievanceWorkflow model)
        {
            if (ModelState.IsValid)
            {
                if (model.WorkflowLevelId.IsNotNullAndNotEmpty())
                {
                    var wkcode = await _lOVBusiness.GetSingle(x => x.Code == model.WorkflowLevelId);
                    model.WorkflowLevelId = wkcode.Id;
                }

                if (model.Level1AssignedToTypeId == "TASK_ASSIGN_TO_USER")
                {
                    model.Level1AssignedToTeamId = null;
                    model.Level1AssignedToTeamUserId = null;
                }
                else
                {
                    model.Level1AssignedToUserId = null;
                }
                if (model.Level2AssignedToTypeId == "TASK_ASSIGN_TO_USER")
                {
                    model.Level2AssignedToTeamId = null;
                    model.Level2AssignedToTeamUserId = null;
                }
                else
                {
                    model.Level2AssignedToUserId = null;
                }
                if (model.Level3AssignedToTypeId == "TASK_ASSIGN_TO_USER")
                {
                    model.Level3AssignedToTeamId = null;
                    model.Level3AssignedToTeamUserId = null;
                }
                else
                {
                    model.Level3AssignedToUserId = null;
                }
                if (model.Level4AssignedToTypeId == "TASK_ASSIGN_TO_USER")
                {
                    model.Level4AssignedToTeamId = null;
                    model.Level4AssignedToTeamUserId = null;
                }
                else
                {
                    model.Level4AssignedToUserId = null;
                }

                if (model.Level1AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Code == model.Level1AssignedToTypeId);
                    model.Level1AssignedToTypeId = type1.Id;
                }
                if (model.Level2AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Code == model.Level2AssignedToTypeId);
                    model.Level2AssignedToTypeId = type1.Id;
                }
                if (model.Level3AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Code == model.Level3AssignedToTypeId);
                    model.Level3AssignedToTypeId = type1.Id;
                }
                if (model.Level4AssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var type1 = await _lOVBusiness.GetSingle(x => x.Code == model.Level4AssignedToTypeId);
                    model.Level4AssignedToTypeId = type1.Id;
                }

                if (model.Level1AssignedToTeamUserId.IsNotNullAndNotEmpty())
                {
                    model.Level1AssignedToUserId = model.Level1AssignedToTeamUserId;
                }
                if (model.Level2AssignedToTeamUserId.IsNotNullAndNotEmpty())
                {
                    model.Level2AssignedToUserId = model.Level2AssignedToTeamUserId;
                }
                if (model.Level3AssignedToTeamUserId.IsNotNullAndNotEmpty())
                {
                    model.Level3AssignedToUserId = model.Level3AssignedToTeamUserId;
                }
                if (model.Level4AssignedToTeamUserId.IsNotNullAndNotEmpty())
                {
                    model.Level4AssignedToUserId = model.Level4AssignedToTeamUserId;
                }

                model.WardId = string.Concat("{", string.Join(",", model.WardIds), "}");

                var temp = await _templateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_GRIEVANCE_WORKFLOW");

                if (model.DataAction == DataActionEnum.Create)
                {
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Create;
                    formTempModel.TemplateCode = "JSC_GRIEVANCE_WORKFLOW";
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    formmodel.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Edit;
                    formTempModel.TemplateCode = "JSC_GRIEVANCE_WORKFLOW";
                    //formTempModel.Id = 
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.RecordId = model.Id;
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    formmodel.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public IActionResult JSCFormulaIndex()
        {

            return View();
        }

        public async Task<IActionResult> GetFormulaList(string type)
        {
            var data = await _smartCityBusiness.GetFormulaList(type);
            return Json(data);

        }
        public async Task<IActionResult> GetFormulaFactor()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("FACTOR_TYPE", "", "");
            return Json(data);

        }
        public async Task<IActionResult> JSCFormula(string type, string id = null)
        {
            var model = new JSCFormulaViewModel();
            model.Formula = "{}";
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _smartCityBusiness.GetFormulaById(id);
                model.DataAction = DataActionEnum.Edit;
                //if (model.Formula.IsNotNullAndNotEmptyAndNotValue("{}"))
                //{
                //    var token = JObject.Parse(model.Formula);
                //    PrintKeys(model, token);
                //}
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                //model.EffectiveToDate = new DateTime(12, 31, 4734);
                var lov = await _lOVBusiness.GetSingle(x => x.Code == type);
                model.FormulaType = lov.Id;
            }
            var list = await _smartCityBusiness.GetFormulaType();
            model.CustomList = new List<IdNameViewModel>();
            model.CustomList = list;
            return View(model);
        }
        static void PrintKeys(JSCFormulaViewModel model, JObject token)
        {
            if (model.Formula.IsNullOrEmptyOrValue("{}"))
            {
                model.FormulaText = "";
                return;
            }
            var op = Convert.ToString(token.GetValue("operator"));
            var addBracket = false;
            if (op.IsNotNullAndNotEmpty())
            {
                if (op == "*" || op == "x" || op == "X" || op == "/")
                {
                    addBracket = true;
                }
                if (addBracket)
                {
                    model.FormulaText = $"{model.FormulaText}(|";
                }

                var op1 = (JObject)token.GetValue("operand1");
                if (op1.ContainsKey("operator"))
                {
                    PrintKeys(model, op1);
                }
                else
                {
                    var val = (JObject)op1.GetValue("value");
                    if (val != null)
                    {
                        var type = Convert.ToString(val.GetValue("type"));
                        if (type == "unit")
                        {
                            var unit = Convert.ToString(val.GetValue("unit"));
                            model.FormulaText = $"{model.FormulaText}{unit}|";
                        }
                        else if (type == "item")
                        {
                            var item = (JObject)val.GetValue("item");
                            var itemValue = Convert.ToString(item.GetValue("value"));
                            var itemTest = Convert.ToString(item.GetValue("test"));
                            model.FormulaText = $"{model.FormulaText}{itemValue}~{itemTest}|";

                        }
                    }
                }
                model.FormulaText = $"{model.FormulaText}{op}|";
                var op2 = (JObject)token.GetValue("operand2");
                if (op2.ContainsKey("operator"))
                {
                    PrintKeys(model, op2);
                }
                else
                {
                    var val = (JObject)op2.GetValue("value");
                    if (val != null)
                    {
                        var type = Convert.ToString(val.GetValue("type"));
                        if (type == "unit")
                        {
                            var unit = Convert.ToString(val.GetValue("unit"));
                            model.FormulaText = $"{model.FormulaText}{unit}|";
                        }
                        else if (type == "item")
                        {
                            var item = (JObject)val.GetValue("item");
                            var itemValue = Convert.ToString(item.GetValue("value"));
                            var itemTest = Convert.ToString(item.GetValue("test"));
                            model.FormulaText = $"{model.FormulaText}{itemValue}~{itemTest}|";

                        }
                    }
                }
                if (addBracket)
                {
                    model.FormulaText = $"{model.FormulaText})|";
                }

            }
        }
        public void ChildComp(JObject comps, string formula)
        {

            var opt1 = comps.SelectToken("operator");
            if (opt1 != null)
            {
                var opr = opt1.ToString();
                var obj2 = (JObject)comps.SelectToken("operand2");
                var obj3 = (JObject)obj2.SelectToken("value");
                var obj4 = obj3.SelectToken("unit").ToString();

                JObject rows = (JObject)comps.SelectToken("operand1");
                formula = opr + obj4 + formula;
                ChildComp(rows, formula);
            }
            else
            {
                var obj2 = (JObject)comps.SelectToken("operand2");
                var obj3 = (JObject)obj2.SelectToken("value");
                var obj4 = obj3.SelectToken("unit").ToString();
                formula = obj4 + formula;
            }

        }

        public async Task<IActionResult> ManageFormula(JSCFormulaViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Formula.IsNotNullAndNotEmptyAndNotValue("{}"))
                {
                    var token = JObject.Parse(model.Formula);
                    PrintKeys(model, token);
                }
                if (model.DataAction == DataActionEnum.Create)
                {
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Create;
                    formTempModel.TemplateCode = "JSC_FORMULA_SETTINGS";
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        ViewBag.Success = "True";
                        //RedirectToAction("JSCFormula");
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Edit;
                    formTempModel.TemplateCode = "JSC_FORMULA_SETTINGS";
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.RecordId = model.Id;
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                         ViewBag.Success = "True";
                       //return RedirectToAction("JSCFormula");
                        return Json(new { success = true });
                        
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> GetJSCMapViewTreeList(string id)
        {
            var mapdata = await _smartCityBusiness.GetJSCMapViewTreeList();
            var warddata = mapdata.Where(x => x.ParentId == "WARD").OrderBy(x => x.Name).ToList();
            var localitydata = mapdata.Where(x => x.ParentId == "LOCALITY").OrderBy(x => x.Name).ToList();
            var landmarkdata = mapdata.Where(x => x.ParentId == "LANDMARK").OrderBy(x => x.Name).ToList();
            var result = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                //Ward
                result.Add(new TreeViewViewModel
                {
                    id = "WARD",
                    Name = "Ward",
                    DisplayName = "Ward",
                    ParentId = "",
                    hasChildren = true,
                    children = true,
                    text = "Ward",
                    parent = "#",
                    a_attr = new { data_id = "WARD", data_name = "Ward" },
                });
                //Locality
                result.Add(new TreeViewViewModel
                {
                    id = "LOCALITY",
                    Name = "Locality",
                    DisplayName = "Locality",
                    ParentId = "",
                    hasChildren = true,
                    children = true,
                    text = "Locality",
                    parent = "#",
                    a_attr = new { data_id = "LOCALITY", data_name = "LOCALITY" },
                });
                //Landmark
                result.Add(new TreeViewViewModel
                {
                    id = "LANDMARK",
                    Name = "Landmark",
                    DisplayName = "Landmark",
                    ParentId = "",
                    hasChildren = true,
                    children = true,
                    text = "Landmark",
                    parent = "#",
                    a_attr = new { data_id = "LANDMARK", data_name = "LANDMARK" },
                });
            }
            else
            {
                if (id == "WARD")
                {
                    result.AddRange(warddata.Select(x => new TreeViewViewModel
                    {
                        id = x.id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.Name,
                        children = false,
                        a_attr = new { data_id = x.id, data_name = x.Name, data_type = "WARD", data_latitude = x.Latitude, data_Longitude = x.Longitude },
                    }));
                }
                else if (id == "LOCALITY")
                {
                    result.AddRange(localitydata.Select(x => new TreeViewViewModel
                    {
                        id = x.id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.Name,
                        children = false,
                        a_attr = new { data_id = x.id, data_name = x.Name, data_type = "LOCALITY", data_latitude = x.Latitude, data_Longitude = x.Longitude },
                    }));
                }
                else if (id == "LANDMARK")
                {
                    result.AddRange(landmarkdata.Select(x => new TreeViewViewModel
                    {
                        id = x.id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.Name,
                        children = false,
                        a_attr = new { data_id = x.id, data_name = x.Name, data_type = "LANDMARK", data_latitude = x.Latitude, data_Longitude = x.Longitude },
                    }));
                }

            }
            //var model = result.OrderBy(x => x.Name).ToList();
            return Json(result);
        }

        public async Task<IActionResult> AssetMapView()
        {
            var data = await _smartCityBusiness.GetAssetMapViewTreeList();
            data = data.Where(x => x.ParentId != "WARD").ToList();
            ViewBag.AssetList = data;
            return View();
        }

        public async Task<IActionResult> GetAssetMapViewTreeList(string id)
        {
            var mapdata = await _smartCityBusiness.GetAssetMapViewTreeList();
            var warddata = mapdata.Where(x => x.ParentId == "WARD").OrderBy(x => x.Name).ToList();

            var result = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                //Ward
                result.Add(new TreeViewViewModel
                {
                    id = "WARD",
                    Name = "Ward",
                    DisplayName = "Ward",
                    ParentId = "",
                    hasChildren = true,
                    children = true,
                    text = "Ward",
                    parent = "#",
                    a_attr = new { data_id = "WARD", data_name = "Ward" },
                });

            }
            else
            {
                if (id == "WARD")
                {
                    result.AddRange(warddata.Select(x => new TreeViewViewModel
                    {
                        id = x.id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = x.ParentId,
                        hasChildren = true,
                        parent = x.ParentId,
                        text = x.Name,
                        children = true,
                        a_attr = new { data_id = x.id, data_name = x.Name, data_type = "WARD" },
                    }));
                }
                else
                {
                    var assetdata = mapdata.Where(x => x.ParentId == id).OrderBy(x => x.Name).ToList();
                    result.AddRange(assetdata.Select(x => new TreeViewViewModel
                    {
                        id = x.id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.Name,
                        children = false,
                        a_attr = new { data_id = x.id, data_name = x.Name, data_type = "ASSET", data_lat = x.Latitude, data_long = x.Longitude },
                    }));
                }

            }
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsset()
        {
            var data = await _smartCityBusiness.GetAssetMapViewTreeList();
            data = data.Where(x => x.ParentId != "WARD" && x.ParentId != null).ToList();
            return Json(data);
        }


        [HttpGet]
        public async Task<IActionResult> GetJSCAssetConsumerData(string consumerId)
        {
            var data = await _smartCityBusiness.ReadJSCAssetConsumerData(consumerId);
            return Json(data);
        }

        public IActionResult PropertyDashboard()
        {
            return View();
        }

        public async Task<IActionResult> GetAssetCountByWard(string wardId = null, string collectorId = null, string revType = null)
        {
            var data = await _smartCityBusiness.GetAssetCountByWard(wardId, collectorId, revType);

            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = data.Select(x => x.WardName).ToList(),
                ItemValueSeries = data.Select(x => x.AssetCount).ToList(),
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetAssetAllotmentCountByWard(string wardId = null, string collectorId = null, string revType = null)
        {
            var data = await _smartCityBusiness.GetAssetAllotmentCountByWard(wardId, collectorId, revType);
            var assetTypeList = await _eGovernanceBusiness.GetAssetTypeListForJammu();
            var wardList = data.Select(x => x.WardName).Distinct().ToList();

            List<StackBarChartViewModel> stackData = new();

            foreach (var item in assetTypeList)
            {
                StackBarChartViewModel model = new();
                model.name = item.Name;
                List<long> dataVal = new();
                foreach (var ward in wardList)
                {
                    var flag = data.Where(x => x.WardName == ward && x.AssetTypeId == item.Id).ToList();
                    if (flag.Count == 0)
                    {
                        dataVal.Add(0);
                    }
                    else
                    {
                        dataVal.Add(flag[0].AssetCount);
                    }
                }
                model.data = dataVal;
                stackData.Add(model);

            }

            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = wardList,
                stackBarValues = stackData,
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetAssetPaymentCountByWard(string wardId = null, string collectorId = null, string revType = null)
        {
            var data = await _smartCityBusiness.GetAssetPaymentCountByWard(wardId, collectorId);

            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = data.Select(x => x.WardName).ToList(),
                ItemValueSeries = data.Select(x => x.Amount).ToList(),
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetAssetPaymentCountByAssetType(string wardId = null, string collectorId = null, string revType = null)
        {
            var data = await _smartCityBusiness.GetAssetPaymentCountByAssetType(wardId, collectorId);

            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = data.Select(x => x.AssetTypeName).ToList(),
                ItemValueSeries = data.Select(x => x.Amount).ToList(),
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetAssetPaymentCountByCollector(string wardId = null, string collectorId = null, string revType = null)
        {
            var data = await _smartCityBusiness.GetAssetPaymentCountByCollector(wardId, collectorId);

            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = data.Select(x => x.CollectorName).ToList(),
                ItemValueSeries = data.Select(x => x.Amount).ToList(),
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetAssetPaymentCountByPaymentStatus(string wardId = null, string collectorId = null, string revType = null)
        {
            var data = await _smartCityBusiness.GetAssetPaymentCountByPaymentStatus(wardId, collectorId);

            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = data.Select(x => x.PaymentStatusName).ToList(),
                ItemValueSeries = data.Select(x => x.PaymentCount).ToList(),
            };
            return Json(chartData);
        }

        public IActionResult RevenueDashboard()
        {
            return View();
        }

        public async Task<IActionResult> GISView()
        {
            // await TestGenerateBillPayment();

            var data = await _smartCityBusiness.GetJSCColonyMapViewList();
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }

            ViewBag.geoJson = JsonConvert.SerializeObject(data);
            return View();
        }

        public async Task<IActionResult> GetColonyMapViewTreeList(string id)
        {
            var mapdata = await _smartCityBusiness.GetJSCColonyMapViewList();
            var warddata = mapdata.Where(x => x.ParentId == "COLONY").OrderBy(x => x.col_name).ToList();

            var result = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                //Ward
                result.Add(new TreeViewViewModel
                {
                    id = "COLONY",
                    Name = "Colony",
                    DisplayName = "Colony",
                    ParentId = "",
                    hasChildren = true,
                    children = true,
                    text = "Colony",
                    parent = "#",
                    a_attr = new { data_id = "COLONY", data_name = "Colony" },
                    //state = new { selected = true }
                });

            }
            else
            {
                if (id == "COLONY")
                {
                    result.AddRange(warddata.Select(x => new TreeViewViewModel
                    {
                        id = x.col_id,
                        Name = x.col_name,
                        DisplayName = x.col_name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.col_name,
                        children = false,
                        a_attr = new { data_id = x.col_id, data_name = x.col_name, data_type = "COLONY" },
                        //state = new { selected = true }

                    }));
                }
                else
                {
                    var assetdata = mapdata.Where(x => x.ParentId == id).OrderBy(x => x.col_name).ToList();
                    result.AddRange(assetdata.Select(x => new TreeViewViewModel
                    {
                        id = x.col_id,
                        Name = x.col_name,
                        DisplayName = x.col_name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.col_name,
                        children = false,
                        a_attr = new { data_id = x.col_id, data_name = x.col_name, data_type = "COL", },
                        //state = new { selected = true }
                    }));
                }

            }
            return Json(result);
        }

        public async Task<IActionResult> GetColoniesData(string ids)
        {
            var data = await _smartCityBusiness.GetJSCColonyMapViewList();
            var filtered = new List<JSCColonyViewModel>();
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }

            var idlist = ids.Split("|");
            foreach (var item in idlist)
            {
                filtered.Add(data.Where(x => x.col_id == item).FirstOrDefault());
            }
            return Json(filtered);
        }
        public async Task<IActionResult> GenerateBillPayment()
        {
            await TestGenerateBillPayment();
            return View();
        }

        public async Task<IActionResult> TestGenerateBillPayment()
        {
            await _smartCityBusiness.GenerateAssetBillPayment();
            return null;
        }

        public async Task<IActionResult> MyJSCPayments(string portalNames = null)
        {
            ViewBag.PortalNames = portalNames;

            var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_JAMMU_URL");
            ViewBag.ReturnUrl = cs.Value;

            return View();
        }

        public IActionResult SearchAssetDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsset(JSCParcelViewModel model)
        {
            //var data = await _smartCityBusiness.GetAssetByServiceNo(model.ServiceNo);
            var data = await _smartCityBusiness.GetParcelByPropertyId(model.prop_id);
            if (data.IsNotNull())
            {
                return Json(new { success = true, id = data.gid });
            }
            return Json(new { success = false, error = "Details not found. Please enter correct asset no." });

        }

        public IActionResult SearchConsumerDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchConsumer(JSCParcelViewModel model)
        {
            //var data = await _smartCityBusiness.GetConsumerByConsumerNo(model.ConsumerNo);
            var data = await _smartCityBusiness.GetParcelByMobileOrAadhar(model.tel_no);
            if (data.IsNotNull())
            {
                return Json(new { success = true, mobile = data.tel_no, aadhar = data.aadhar });
            }
            return Json(new { success = false, error = "Details not found. Please enter correct consumer no." });

        }

        public async Task<IActionResult> ConsumerData(string id, bool backEnable = false, string assetId = null)
        {
            ViewBag.BackEnable = backEnable;
            ViewBag.AssetId = assetId;
            //var data = await _smartCityBusiness.GetConsumerById(id);
            var data = await _smartCityBusiness.GetParcelByMobileOrAadhar(id);
            return View(data);
        }

        public async Task<IActionResult> AssetData(string id, bool backEnable = false, string consumerId = null)
        {
            ViewBag.BackEnable = backEnable;
            ViewBag.ConsumerId = consumerId;
            var data = await _smartCityBusiness.GetAssetById(id);
            return View(data);
        }

        public async Task<IActionResult> GetAssetConsumerData(string assetId)
        {
            var data = await _smartCityBusiness.GetAssetConsumerData(assetId);
            return Json(data);
        }

        public async Task<IActionResult> GetAssetPaymentData(string assetId)
        {
            var data = await _smartCityBusiness.GetAssetPaymentData(assetId);
            return Json(data);
        }

        public async Task<IActionResult> GetConsumerAssetData(string consumerId)
        {
            var data = await _smartCityBusiness.GetConsumerAssetData(consumerId);
            return Json(data);
        }

        public async Task<IActionResult> GetConsumerPaymentData(string consumerId)
        {
            var data = await _smartCityBusiness.GetConsumerPaymentData(consumerId);
            return Json(data);
        }

        public async Task<IActionResult> GetConsumerAssetData2(string mobileNo, string aadhar)
        {
            var data = await _smartCityBusiness.GetPropertyDetailsForConsumer(mobileNo, aadhar);
            return Json(data);
        }

        public async Task<IActionResult> GetConsumerPaymentData2(string mobileNo, string aadhar)
        {
            var data = await _smartCityBusiness.GetPaymentDetailsForConsumer(mobileNo, aadhar);
            return Json(data);
        }


        public IActionResult JSCParcelGISView()
        {
            return View();
        }

        public async Task<IActionResult> GetParcelMapViewTreeList(string id)
        {
            var mapdata = await _smartCityBusiness.GetJSCParcelMapViewList("", "");
            var warddata = mapdata.Where(x => x.ParentId == "PARCEL").OrderBy(x => x.own_name).ToList();

            var result = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                //Ward
                result.Add(new TreeViewViewModel
                {
                    id = "PARCEL",
                    Name = "Parcel",
                    DisplayName = "Parcel",
                    ParentId = "",
                    hasChildren = true,
                    children = true,
                    text = "Parcel",
                    parent = "#",
                    a_attr = new { data_id = "PARCEL", data_name = "Parcel" },
                    //state = new { selected = true }
                });

            }
            else
            {
                if (id == "PARCEL")
                {
                    result.AddRange(warddata.Select(x => new TreeViewViewModel
                    {
                        id = x.prop_id,
                        Name = x.own_name,
                        DisplayName = x.own_name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.own_name,
                        children = false,
                        a_attr = new { data_id = x.prop_id, data_name = x.own_name, data_type = "PARCEL" },
                        //state = new { selected = true }

                    }));
                }
                else
                {
                    var assetdata = mapdata.Where(x => x.ParentId == id).OrderBy(x => x.own_name).ToList();
                    result.AddRange(assetdata.Select(x => new TreeViewViewModel
                    {
                        id = x.prop_id,
                        Name = x.own_name,
                        DisplayName = x.own_name,
                        ParentId = x.ParentId,
                        hasChildren = false,
                        parent = x.ParentId,
                        text = x.own_name,
                        children = false,
                        a_attr = new { data_id = x.prop_id, data_name = x.own_name, data_type = "PROP", },
                        //state = new { selected = true }
                    }));
                }

            }
            return Json(result);
        }

        public async Task<IActionResult> GetParcelColumn()
        {
            var res = await _smartCityBusiness.GetParcelColumnList();
            return Json(res);
        }
        public async Task<IActionResult> GetParcelSearchResult(string colName, string colText)
        {
            var data = await _smartCityBusiness.GetJSCParcelMapViewList(colName, colText);
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }
            return Json(data);
        }

        public async Task<IActionResult> GetColonyList()
        {
            var data = await _smartCityBusiness.GetColonyList();
            return Json(data);
        }
        public async Task<IActionResult> GetParcelTypeList()
        {
            var data = await _smartCityBusiness.GetParcelTypeList();
            return Json(data);
        }
        public async Task<IActionResult> GetWardList()
        {
            var data = await _smartCityBusiness.GetWardList();
            return Json(data);
        }

        public async Task<IActionResult> GetWardListLoginType(string loginType)
        {
            var sublogindata = await _smartCityBusiness.GetSubLogin(loginType);          
            var wardList = new List<string>();

                foreach(var item in sublogindata)
                {
                    var ward = item.Ward;
                    ward = ward.TrimStart('[');
                    ward = ward.TrimEnd(']');
                    ward = ward.Replace(" ", "");
                    var list = ward.Split(",");
                    
                    wardList.AddRange(list);
                }
            var data = await _smartCityBusiness.GetWardList();
            if (wardList.Count > 0)
            {
                data = data.Where(x => !wardList.Contains(x.Id)).ToList();
              //  data = data.Where(x => !wardList.Any(y => y == x.Id)).ToList();
            }           
            return Json(data);
        }

        public async Task<IActionResult> GetUserListLoginType(string dataAction)
        {
            if (dataAction == "Create")
            {
                var data = await _smartCityBusiness.GetUserListForSubLogin();
                return Json(data);
            }
            else
            {
                var data = await _userBusiness.GetUserListForPortal();
                return Json(data);
            }         
           
        }
        public async Task<IActionResult> GetJSCZoneListByDepartment(string departmentId)
        {
            var data = await _smartCityBusiness.GetJSCZoneListByDepartment(departmentId);
            return Json(data);
        }
        public async Task<IActionResult> GetJSCZoneIdNameList()
        {
            var data = await _smartCityBusiness.GetJSCZoneList();
            return Json(data);
        }
        public async Task<IActionResult> GetJSCDepartmentList()
        {
            var data = await _smartCityBusiness.GetJSCDepartmentList();
            return Json(data);
        }
        public async Task<IActionResult> GetJSCRevenueTypeList()
        {
            var data = await _smartCityBusiness.GetJSCRevenueTypeList();
            return Json(data);
        }
        public async Task<IActionResult> GetJSCGrievanceEmployeeByDepartment(string departmentId)
        {
            var list = await _smartCityBusiness.GetGrievanceWorkflowList();
            var data = list.Where(x => x.DepartmentId == departmentId).FirstOrDefault();
            var emplist = new List<IdNameViewModel>();
            if (data != null)
            {
                if (data.Level1AssignedToUserId.IsNotNullAndNotEmpty())
                {
                    var user1 = await _userBusiness.GetSingleById(data.Level1AssignedToUserId);
                    if (user1 != null)
                    {
                        emplist.Add(new IdNameViewModel { Id = user1.Id, Name = user1.Name });
                    }
                }
                if (data.Level2AssignedToUserId.IsNotNullAndNotEmpty())
                {
                    var user2 = await _userBusiness.GetSingleById(data.Level2AssignedToUserId);
                    if (user2 != null)
                    {
                        emplist.Add(new IdNameViewModel { Id = user2.Id, Name = user2.Name });
                    }
                }
                if (data.Level3AssignedToUserId.IsNotNullAndNotEmpty())
                {
                    var user3 = await _userBusiness.GetSingleById(data.Level3AssignedToUserId);
                    if (user3 != null)
                    {
                        emplist.Add(new IdNameViewModel { Id = user3.Id, Name = user3.Name });
                    }
                }
                if (data.Level4AssignedToUserId.IsNotNullAndNotEmpty())
                {
                    var user4 = await _userBusiness.GetSingleById(data.Level4AssignedToUserId);
                    if (user4 != null)
                    {
                        emplist.Add(new IdNameViewModel { Id = user4.Id, Name = user4.Name });
                    }
                }
            }
            return Json(emplist);
        }
        public async Task<IActionResult> GetJSCTurnaroundDateRange()
        {
            var data = new List<IdNameViewModel>();
            data.Add(new IdNameViewModel { Code = "MONTH", Name = "Last Month" });
            data.Add(new IdNameViewModel { Code = "QUARTER", Name = "Last Quarter" });
            data.Add(new IdNameViewModel { Code = "HALF_YEAR", Name = "Last 6 Months" });
            return Json(data);
        }
        public async Task<IActionResult> GetJSCGrievanceTypeList()
        {
            var data = await _smartCityBusiness.GetJSCGrievanceTypeList();
            return Json(data);
        }
        public async Task<IActionResult> GetParcelIdNameList()
        {
            var data = await _smartCityBusiness.GetParcelIdNameList();
            return Json(data);

        }
        public async Task<IActionResult> GetTransferStationList()
        {
            var data = await _smartCityBusiness.GetTransferStationList();
            return Json(data);

        }
        public async Task<IActionResult> GetBinCollectorMobile(string userId)
        {
            var data = await _smartCityBusiness.GetBinCollectorMobile(userId);
            if (data.IsNotNull())
            {
                return Json(new { success = true, result = data });
            }
            return Json(new { success = false });
        }
        public async Task<IActionResult> GetJSCOwnerList()
        {
            var data = await _smartCityBusiness.GetJSCOwnerList();
            return Json(data);
        }

        public async Task<IActionResult> ApplyJSCCitizenService(string templateCode, string parcelType, string wardNo)
        {
            return View();
        }

        public async Task<IActionResult> GetParcelSearchByWardandType(string ward, string parcelType)
        {
            var data = await _smartCityBusiness.GetParcelSearchByWardandType(ward, parcelType);
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }
            return Json(data);
        }

        public async Task<IActionResult> JSCServiceTemplateTiles(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalId = null, string selectedCatCode = null)
        {
            var model = new TemplateViewModel();
            model.Code = templateCode;
            model.CategoryCode = categoryCode;
            model.UserId = userId;
            model.ModuleCodes = moduleCodes;
            model.Prms = prms;
            model.CallBackMethodName = cbm;
            model.TemplateIds = templateIds;
            model.CategoryIds = categoryIds;
            model.TemplateCategoryType = categoryType;
            model.SelectedCategoryCode = selectedCatCode;
            if (model.SelectedCategoryCode.IsNotNullAndNotEmpty())
            {
                var cat = await _portalBusiness.GetSingle<TemplateCategoryViewModel, TemplateCategory>
                    (x => x.Code == model.SelectedCategoryCode);
                model.SelectedCategoryName = cat?.Name;
            }

            if (portalId.IsNotNullAndNotEmpty())
            {
                var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                if (portal?.Name == "EGovCustomer")
                {
                    var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                    ViewBag.SmartCityUrl = cs.Value;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> GetGrievanceTypeByDepartment(string department)
        {
            var res = await _smartCityBusiness.GetGrievanceTypeByDepartment(department);
            return Json(res);
        }

        public async Task<IActionResult> JSCNeedsAndWantsHome()
        {
            var UserId = _userContext.UserId;
            var data = await _eGovernanceBusiness.GetWardData(UserId);
            var data1 = await _eGovernanceBusiness.GetNWTimeLineData();
            foreach (var item in data1)
            {
                if (DateTime.Now >= item.FromDate && DateTime.Now <= item.ToDate)
                {
                    ViewBag.Exist = true;
                    ViewBag.Message = item.IsNotNull() ? item.Message : null;
                    ViewBag.FromDate = item.FromDate;
                    ViewBag.ToDate = item.ToDate;

                }
                else { ViewBag.Exist = false; }
            }
            ViewBag.UserId = UserId;
            ViewBag.WardName = data.IsNotNull() ? data.WardName : null;
            ViewBag.WardId = data.IsNotNull() ? data.WardId : null;

            var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal?.Name == "EGovCustomer")
            {
                var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                ViewBag.SmartCityUrl = cs.Value;
            }

            return View(data);
        }

        public async Task<IActionResult> JSCCititzenProjectsHomeList(string type, string userId, DateTime fromDate, DateTime toDate)
        {
            var model = await _eGovernanceBusiness.GetJSCProposalProjectsList(type, userId);

            foreach (var i in model)
            {
                i.DisplayRequestedDate = i.RequestedDate.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            }
            return Json(model);
        }

        public IActionResult ManageJSCGarbageCollection()
        {
            return View();
        }


        public IActionResult ManageJSCGarbageCollectionByAuto()
        {
            return View();
        }

        public async Task<IActionResult> GetWardData(string wardNo)
        {
            var data = await _smartCityBusiness.GetJSCWardMapViewList(wardNo);
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }
            return Json(data);
        }


        public async Task<IActionResult> CitizenAssetList(string templateCode, string tempName)
        {
            ViewBag.TemplateCode = templateCode;
            ViewBag.TemplateName = tempName.Replace('_', ' ');
            return View();
        }

        public async Task<IActionResult> GetParcelListByUser(string userId)
        {
            var data = await _smartCityBusiness.GetJSCParcelListByUser(userId);
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }
            return Json(data);
        }

        public async Task<IActionResult> GetParcelDataByPclId(string id)
        {
            var data = await _smartCityBusiness.GetParcelDataByPclId(id);
            data.geometry = JObject.Parse(data.geometry);
            return Json(data);
        }

        public async Task<IActionResult> GetJSCPendingPaymentsList(string portalNames = null, string propertyId = null)
        {
            string ids = null; //_userContext.PortalId;
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }

            var data = await _smartCityBusiness.GetJSCPaymentsList(ids);
            if (propertyId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.SourceReferenceId == propertyId).ToList();
            }
            var j = Json(data.Where(x => x.PaymentStatusCode == "JSC_NOT_PAID").OrderBy(x => x.DueDate));

            return j;
        }

        public async Task<IActionResult> GetJSCCompletedPaymentsList(string portalNames = null, string propertyId = null)
        {
            string ids = null; //_userContext.PortalId;
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }

            var data = await _smartCityBusiness.GetJSCPaymentsList(ids);
            if (propertyId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.SourceReferenceId == propertyId).ToList();
            }
            var j = Json(data.Where(x => x.PaymentStatusCode == "JSC_SUCCESS").OrderByDescending(x => x.DueDate));
            return j;
        }

        //public async Task<IActionResult> UpdateJSCPaymentDetails(string paymentsIds = null,string portalNames=null)
        //{
        //    var ids = _userContext.PortalId;
        //    if (portalNames.IsNotNullAndNotEmpty())
        //    {
        //        string[] names = portalNames.Split(",").ToArray();
        //        var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
        //        string[] portalIds = portals.Select(x => x.Id).ToArray();
        //        ids = String.Join("','", portalIds);
        //    }

        //    var data = await _smartCityBusiness.GetJSCPaymentsList(ids);

        //    string[] payIds = paymentsIds.Split(",").ToArray();
        //    var payments = data.Where(x => payIds.Contains(x.Id)).ToList();
        //    var total = 0;
        //    foreach(var item in payments)
        //    {
        //        total = total + item.Amount.ToSafeInt();
        //    }

        //    var status = await _lOVBusiness.GetSingle(x => x.Code == "JSC_NOT_PAID");
        //    var mode = await _lOVBusiness.GetSingle(x => x.Code == "JSC_ONLINE");

        //    var paymentModel = new JSCPaymentViewModel()
        //    {
        //        PaymentIds = paymentsIds,
        //        TotalAmount = total.ToString(),
        //        PaymentStatus = status.Id,
        //        PaymentMode = mode.Id,
        //        PaymentDate = DateTime.Now,
        //    };

        //    var noteTempModel = new NoteTemplateViewModel();
        //    noteTempModel.DataAction = DataActionEnum.Create;
        //    noteTempModel.ActiveUserId = _userContext.UserId;
        //    noteTempModel.TemplateCode = "JSC_PAYMENT_DETAIL";
        //    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

        //    notemodel.Json = JsonConvert.SerializeObject(paymentModel);

        //    var result = await _noteBusiness.ManageNote(notemodel);

        //    return Json(new { success = true, data = result.Item.UdfNoteTableId });

        //}

        [HttpPost]
        public async Task<JsonResult> OnlinePaymentJSC(string ntsId = null, string noteTableId = null, long amount = 0, NtsTypeEnum ntsType = NtsTypeEnum.Task, string assigneeUserId = null, string returnUrl = null, string paymentIds = null)
        {
            var model = new OnlinePaymentViewModel()
            {
                NtsId = ntsId,
                UdfTableId = noteTableId,
                Amount = amount,
                NtsType = ntsType,
                UserId = assigneeUserId,
                ReturnUrl = returnUrl,
                PaymentIds = paymentIds
            };

            var result = await _smartCityBusiness.UpdateOnlinePaymentDetailsJSC(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true, requestURL = result.Item.RequestUrl/*, returnurl*/ });
            }
            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        }
        public async Task<IActionResult> PaymentResponseJSC(string msg)
        {
            if (msg.IsNotNullAndNotEmpty())
            {
                var items = msg.Split('|');
                var userId = "";
                var portalId = "";
                if (items.Length > 6)
                {
                    userId = items[18];
                    portalId = items[19];

                    var portal = await _portalBusiness.GetSingleById(portalId);
                    var user = await _userBusiness.ValidateUserById(userId);
                    if (user != null && portal != null)
                    {
                        var id = new ApplicationIdentityUser
                        {
                            Id = user.Id,
                            UserName = user.Name,
                            IsSystemAdmin = user.IsSystemAdmin,
                            Email = user.Email,
                            UserUniqueId = user.Email,
                            CompanyId = user.CompanyId,
                            CompanyCode = user.CompanyCode,
                            CompanyName = user.CompanyName,
                            JobTitle = user.JobTitle,
                            PhotoId = user.PhotoId,
                            UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                            UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                            PortalId = portalId,
                            PortalTheme = portal.Theme.ToString(),
                            UserPortals = user.UserPortals,
                            LegalEntityId = user.LegalEntityId,
                            LegalEntityCode = user.LegalEntityCode,
                            PersonId = user.PersonId,
                            PositionId = user.PositionId,
                            DepartmentId = user.DepartmentId,
                            PortalName = portal.Name,

                        };
                        id.MapClaims();
                        await _customUserManager.SignInAsync(id, true);
                    }
                    return RedirectToAction("PaymentResponseJSCSuccess", "SmartCity", new { @area = "EGov", @msg = msg });
                }
            }
            return RedirectToAction("PaymentResponseJSCSuccess", "SmartCity", new { @area = "EGov", @msg = msg });
        }
        public async Task<IActionResult> PaymentResponseJSCSuccess(string msg)
        {
            if (msg.IsNullOrEmpty())
            {
                return View(new OnlinePaymentViewModel { PaymentStatusCode = "JSC_ERROR", UserMessage = "Invalid Message from Payment Gateway" });
            }
            var responseViewModel = await ValidatePaymentResponse(msg);
            ///Update Online payment
            await _smartCityBusiness.UpdateOnlinePaymentJSC(responseViewModel);
            if (responseViewModel.PaymentStatusCode == "JSC_SUCCESS")
            {
                responseViewModel.UserMessage = $"Your payment has been completed successfully. Please note the reference number: {responseViewModel.PaymentReferenceNo} for further communication.";
                // update udf payment status id,payment reference and then complete the task
                if (responseViewModel.NtsType == NtsTypeEnum.Task)
                {
                    var payIds = responseViewModel.PaymentIds;
                    string[] pIds = payIds.Split(",").ToArray();

                    var data = await _smartCityBusiness.GetJSCPaymentsList(null, _userContext.UserId);
                    data = data.Where(x => pIds.Contains(x.Id)).ToList();

                    string[] taskIds = data.Select(x => x.TaskId).ToArray();

                    foreach (var id in taskIds)
                    {
                        //Update paymentdetailid in Payment table
                        var newmodel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                        {
                            NoteId = data.Where(x => x.TaskId == id).Select(x => x.NoteId).FirstOrDefault(),
                            DataAction = DataActionEnum.Edit,
                            ActiveUserId = _userContext.UserId,
                            SetUdfValue = true
                        });
                        var udfnotetableId = data.Where(x => x.TaskId == id).Select(x => x.Id).FirstOrDefault();
                        var status = await _lOVBusiness.GetSingle(x => x.Code == "JSC_SUCCESS");

                        var items = msg.Split('|');

                        var rowData1 = newmodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        rowData1["PaymentDetailId"] = items[1];
                        rowData1["PaymentStatus"] = status.Id;
                        rowData1["PaymentDate"] = DateTime.Now;
                        rowData1["ReferenceNo"] = responseViewModel.PaymentReferenceNo;

                        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                        var updatenote = await _noteBusiness.EditNoteUdfTable(newmodel, data1, udfnotetableId);

                        //Complete all payment task 
                        TaskTemplateViewModel model = new TaskTemplateViewModel();
                        model.TaskId = id;
                        var step = await _taskBusiness.GetSingleById(id);
                        if (step.ParentServiceId != null)
                        {
                            model.TaskTemplateType = TaskTypeEnum.StepTask;
                        }

                        model.DataAction = DataActionEnum.Edit;
                        model.ActiveUserId = _userContext.UserId;
                        model.SetUdfValue = true;
                        var taskModel = await _taskBusiness.GetTaskDetails(model);
                        if (taskModel != null)
                        {
                            if (taskModel.ColumnList != null && taskModel.ColumnList.Any())
                            {
                                var rowData = taskModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                                if (rowData.ContainsKey("PaymentStatusId"))
                                {
                                    rowData["PaymentStatusId"] = responseViewModel.PaymentStatusId;
                                }
                                if (rowData.ContainsKey("PaymentReferenceNo"))
                                {
                                    rowData["PaymentReferenceNo"] = responseViewModel.PaymentReferenceNo;
                                }
                                taskModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                            }
                            taskModel.TaskStatusCode = "TASK_STATUS_COMPLETE";
                            if (step.ParentServiceId != null)
                            {
                                taskModel.TaskTemplateType = TaskTypeEnum.StepTask;
                            }

                            taskModel.DataAction = DataActionEnum.Edit;
                            taskModel.ActiveUserId = _userContext.UserId;
                            var update = await _taskBusiness.ManageTask(taskModel);
                        }
                    }
                }
                else if (responseViewModel.NtsType == NtsTypeEnum.Note)
                {
                    var payId = responseViewModel.PaymentIds;

                    await _smartCityBusiness.UpdatePropertyTax(responseViewModel.PaymentStatusId, responseViewModel.PaymentReferenceNo, payId);

                    var data = await _smartCityBusiness.GetPropertyPaymentDetailById(payId);

                    var temp = await _templateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Code == "PROPERTY_INSTALLMENT");
                    
                    //Create Receipt table

                    var rtemp = await _templateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_PropertyTax_Payment_Receipt");

                    var rformTempModel = new FormTemplateViewModel();
                    rformTempModel.DataAction = DataActionEnum.Create;
                    rformTempModel.TemplateCode = "JSC_PropertyTax_Payment_Receipt";
                    var rformmodel = await _cmsBusiness.GetFormDetails(rformTempModel);

                    dynamic exo = new System.Dynamic.ExpandoObject();

                    var prefix = "RE";
                    var seq = await _smartCityBusiness.GetNextPropertyReceiptNumber();
                    var sequence = seq.PadLeft(6, '0');
                    var receiptno = $"{prefix}{sequence}";

                    ((IDictionary<String, Object>)exo).Add("ReceiptNumber", receiptno);
                    ((IDictionary<String, Object>)exo).Add("ReceiptAmount", data.Amount);
                    ((IDictionary<String, Object>)exo).Add("InstallmentId", payId);
                    ((IDictionary<String, Object>)exo).Add("ReceiptYearId", data.Year);
                    ((IDictionary<String, Object>)exo).Add("DdnNo", data.ddnNo);
                    ((IDictionary<String, Object>)exo).Add("Date", DateTime.Now);

                    rformmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

                    rformmodel.Page = new PageViewModel { Template = new Template { TableMetadataId = rtemp.TableMetadataId } };
                    var rres = await _cmsBusiness.ManageForm(rformmodel);
                    responseViewModel.ReturnUrl = responseViewModel.ReturnUrl + "?prms=ReceiptId="+ rformmodel.RecordId;
                }
            }
            else
            {
                responseViewModel.UserMessage = responseViewModel.ResponseError;
            }
            return View(responseViewModel);
        }

        private async Task<OnlinePaymentViewModel> ValidatePaymentResponse(string msg)
        {
            var values = msg.Split('|');
            var id = values[1];
            var model = await _smartCityBusiness.GetOnlinePaymentJSC(id);
            if (model == null)
            {
                return new OnlinePaymentViewModel
                {
                    PaymentStatusCode = "ERROR",
                    ResponseError = "Invalid Transaction"
                };
            }
            model.AuthStatus = values[14];
            model.ResponseErrorCode = values[23];
            model.ResponseError = values[24];
            model.PaymentReferenceNo = values[2];
            if (model.PaymentReferenceNo == "NA")
            {
                model.PaymentReferenceNo = "";
            }
            model.ResponseChecksumValue = values[25];
            model.ResponseMessage = msg.Replace($"|{model.ResponseChecksumValue}", "");

            model.ResponseUrl = Request.Path;

            var paymentStatus = await _eGovernanceBusiness.GetList<LOVViewModel, LOV>(x => x.LOVType == "JSC_PAYMENT_STATUS");
            switch (model.AuthStatus)
            {
                case "0300":
                    var success = paymentStatus.FirstOrDefault(x => x.Code == "JSC_SUCCESS");
                    if (success != null)
                    {
                        model.PaymentStatusId = success.Id;
                        model.PaymentStatusCode = success.Code;
                        model.PaymentStatusName = success.Name;
                    }
                    break;
                default:
                    var fail = paymentStatus.FirstOrDefault(x => x.Code == "JSC_ERROR");
                    if (fail != null)
                    {
                        model.PaymentStatusId = fail.Id;
                        model.PaymentStatusCode = fail.Code;
                        model.PaymentStatusName = fail.Name;
                    }
                    if (model.ResponseError.IsNullOrEmpty() || model.ResponseError == "NA")
                    {
                        model.ResponseError = "An error occured while processing your request.";
                    }
                    if (model.PaymentReferenceNo.IsNotNullAndNotEmpty())
                    {
                        model.ResponseError = $"{model.ResponseError} Please note the reference number: {model.PaymentReferenceNo} for further communication.";
                    }
                    break;
            }
            return model;
        }
        public async Task<IActionResult> GetJSCLocalityListByWard(string wardNo)
        {
            var data = await _smartCityBusiness.GetJSCLocalityList(wardNo);
            return Json(data);
        }

        public async Task<IActionResult> GetJSCSubLocalityList(string wardNo, string loc)
        {
            var data = await _smartCityBusiness.GetJSCSubLocalityList(wardNo, loc);
            return Json(data);
        }
        public async Task<IActionResult> GetJSCSubLocalityIdNameList()
        {
            var data = await _smartCityBusiness.GetJSCSubLocalityIdNameList();
            return Json(data);
        }

        public async Task<IActionResult> GetParcelListByWard(string ward)
        {
            var data = await _smartCityBusiness.GetParcelSearchByWard(ward);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageGarbageCollection(List<string> listOfProps)
        {
            var data = await _smartCityBusiness.ManageGarbageCollection(string.Join(",", listOfProps), _userContext.UserId, "", "", "");
            return Json(new { success = data });
        }

        public async Task<IActionResult> GetJSCBinFeeAmount(DateTime bookingFromDate, DateTime bookingToDate, string binTypeId, string binSizeId, long binNumber)
        {
            var data = await _smartCityBusiness.GetJSCBinFeeAmount(bookingFromDate, bookingToDate, binTypeId, binSizeId, binNumber);
            if (data.IsNotNull())
            {
                return Json(new { success = true, result = data });
            }
            return Json(new { success = false });
        }
        public IActionResult GrievanceReportTurnaroundTime()
        {
            ViewBag.ReportTitle = "Grievance Report - Turnaround Time";
            ViewBag.ReportTypeCode = "TURNAROUND_TIME";

            return View();
        }
        public async Task<IActionResult> GetGrievanceReportTurnaroundTimeData(string department, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetGrievanceReportTurnaroundTimeData(department, fromDate, toDate);
            foreach (var item in data)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }

            return Json(data);
        }
        public async Task<IActionResult> GetGrvStatusOnMap()
        {
            var list = new List<IdNameViewModel>();
            var grvstatuslist = await _lOVBusiness.GetList(x => x.LOVType == "JSC_GRV_STATUS" && x.IsDeleted == false);
            var pendingstatus = grvstatuslist.Where(x => x.Code == "GRV_PENDING").FirstOrDefault();
            list.Add(new IdNameViewModel {Id=pendingstatus.Id, Name=pendingstatus.Name, Code=pendingstatus.Code });
            return Json(list);
        }
        public IActionResult GrievanceReportHeatMap()
        {
            return View();
        }
        public async Task<IActionResult> GetGrievanceReportHeatMapData(DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetGrievanceReportGISBasedData(fromDate, toDate);
            foreach (var a in data)
            {
                //a.geometry = JObject.Parse(a.geometry);
                var geom = JsonConvert.DeserializeObject<dynamic>(a.geometry).coordinates[0][0][0];
                a.Latitude = geom[1];
                a.Longitude = geom[0];
            }
            return Json(data);
        }
        public IActionResult GrievanceReportHeatMapWard()
        {
            return View();
        }
        public async Task<IActionResult> GetGrievanceReportWardHeatMapData(DateTime fromDate, DateTime toDate, string departmentId, string statusId)
        {
            var data = await _smartCityBusiness.GetGrievanceReportWardHeatMapData(fromDate, toDate, departmentId);
            double minValue = data.Min(x=>x.Count);
            double maxValue = data.Max(x => x.Count);

            if (statusId== "GRV_PENDING")
            {
                minValue = data.Min(x=>x.PendingCount);
                maxValue = data.Max(x=>x.PendingCount);
            }
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
                //    a.Color = "#660000";
                double value = a.Count;
                if (statusId == "GRV_PENDING")
                {
                    value = a.PendingCount;
                }
                string darkColor = "rgba(38,0,38,1)"; //"rgba(102,0,0,1)";
                string lightColor = "rgba(191,127,191,1)"; //"rgba(255,178,178,1)";
                double valueP = ApplicationExtension.CalculatePercentage(minValue, maxValue, value);
                var color = ApplicationExtension.GenerateGradientColor(lightColor , darkColor, valueP);
                a.Color = color;
            }
            return Json(data);
        }
        public IActionResult GrievanceReportGISBased()
        {
            return View();
        }
        public async Task<IActionResult> SearchGrievanceReportGISBased(DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetGrievanceReportGISBasedData(fromDate, toDate);
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }
            return Json(data);
        }
        public async Task<IActionResult> GetJSCComplaintByDDN(string ddn)
        {
            var data = await _smartCityBusiness.GetJSCComplaintByDDN(ddn);
            foreach (var item in data)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }
            return Json(data);
        }
        public async Task<IActionResult> ViewJSCComplaintByDDN(string ddn)
        {
            ViewBag.DDN = ddn;
            return View();
        }
        public async Task<IActionResult> SearchParcelDataByWardandLocality(string wardId = null, string locality = null, string ddn = null, string auto = null, DateTime? date = null, bool? isCollected = null)
        {
            var data = await _smartCityBusiness.GetJSCParcelDataForGarbageCollection(wardId, locality, ddn, auto, date);
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }
            if (isCollected.IsNotNull())
            {
                data = data.Where(x => x.IsGarbageCollected == isCollected).ToList();
            }
            return Json(data);
        }
        public async Task<IActionResult> GetParcelDataByAutowise(string wardId = null, string locality = null, string subLocality = null, string auto = null, DateTime? date = null)
        {
            var data = await _smartCityBusiness.GetGISDataByAutoWise(auto, date);
            foreach (var a in data)
            {
                a.geometry = JObject.Parse(a.geometry);
            }
            return Json(data);
        }

        public async Task<IActionResult> JSCPropertyDetails(string propertyId)
        {
            //propertyId = "1";
            var data = await _smartCityBusiness.GetPropertyById(propertyId);
            data.geometry = JObject.Parse(data.geometry);
            ViewBag.PortalId = _userContext.PortalId;
            return View(data);
        }

        public async Task<IActionResult> GetPaymentDetailsByPropertyId(string gid)
        {
            var data = await _smartCityBusiness.GetPaymentDetailsByPropertyId(gid);
            return Json(data);
        }

        public async Task<IActionResult> GetGarbageCollectionDetailsByPropertyId(string gid)
        {
            var data = await _smartCityBusiness.GetGarbageCollectionDetailsByPropertyId(gid);
            return Json(data);
        }

        public async Task<IActionResult> GetRevenueTypeList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("JSC_REVENUE_TYPE", null, null);
            return Json(data);
        }

        public async Task<IActionResult> GetAssetTypeList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("JSC_ASSET_TYPE", null, null);
            return Json(data);
        }

        public async Task<IActionResult> GetTotalRevenue(int? year, string months = null, string wardIds = null, string assetTypeIds = null, string revenueTypeIds = null)
        {

            var data = await _smartCityBusiness.GetTotalRevenue(year, months, wardIds, assetTypeIds, revenueTypeIds);
            var expected = data.Select(x => x.Expected).FirstOrDefault();
            var actual = data.Select(x => x.Actual).FirstOrDefault();
            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueSeries = new List<long>() { expected, actual },
                ItemValueLabel = new List<string>() { "Expected", "Actual" }
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetRevenueByWard(int? year, string months = null, string wardIds = null, string assetTypeIds = null, string revenueTypeIds = null)
        {
            var data = await _smartCityBusiness.GetRevenueByWard(year, months, wardIds, assetTypeIds, revenueTypeIds);
            List<StackBarChartViewModel> stackData = new();

            data = data.OrderBy(x => x.GroupName).ToList();

            //foreach(var item in data)
            //{
            //    var expected = item.Expected;
            //    var actual = item.Actual;
            //    StackBarChartViewModel model1 = new()
            //    {
            //        name = item.GroupName,
            //        data = new List<long>() { expected, actual }
            //    };
            //    stackData.Add(model1);
            //}

            StackBarChartViewModel model1 = new();
            model1.name = "Expected";
            model1.data = data.Select(x => x.Expected).ToList();

            StackBarChartViewModel model2 = new();
            model2.name = "Actual";
            model2.data = data.Select(x => x.Actual).ToList();

            stackData.Add(model1);
            stackData.Add(model2);

            var cat = data.Select(x => x.GroupName).ToList();
            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = cat,
                stackBarValues = stackData,
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetTotalRevenueByAssetType()
        {
            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueSeries = new List<long>() { 50000, 65000 },
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetRevenueByAssetType(int? year, string months = null, string wardIds = null, string assetTypeIds = null, string revenueTypeIds = null)
        {
            var data = await _smartCityBusiness.GetRevenueByAssetType(year, months, wardIds, assetTypeIds, revenueTypeIds);
            List<StackBarChartViewModel> stackData = new();

            //foreach (var item in data)
            //{
            //    var expected = item.Expected;
            //    var actual = item.Actual;
            //    StackBarChartViewModel model1 = new()
            //    {
            //        name = item.GroupName,
            //        data = new List<long>() { expected, actual }
            //    };
            //    stackData.Add(model1);
            //}


            StackBarChartViewModel model1 = new();
            model1.name = "Expected";
            model1.data = data.Select(x => x.Expected).ToList();

            StackBarChartViewModel model2 = new();
            model2.name = "Actual";
            model2.data = data.Select(x => x.Actual).ToList();

            stackData.Add(model1);
            stackData.Add(model2);

            var cat = data.Select(x => x.GroupName).ToList();
            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = cat,
                stackBarValues = stackData,
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetTotalRevenueByRevenueType()
        {
            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueSeries = new List<long>() { 50000, 65000 },
            };
            return Json(chartData);
        }

        public async Task<IActionResult> GetRevenueByRevenueType(int? year, string months = null, string wardIds = null, string assetTypeIds = null, string revenueTypeIds = null)
        {
            var data = await _smartCityBusiness.GetRevenueByRevenueType(year, months, wardIds, assetTypeIds, revenueTypeIds);
            List<StackBarChartViewModel> stackData = new();

            //foreach (var item in data)
            //{
            //    var expected = item.Expected;
            //    var actual = item.Actual;
            //    StackBarChartViewModel model1 = new()
            //    {
            //        name = item.GroupName,
            //        data = new List<long>() { expected, actual }
            //    };
            //    stackData.Add(model1);
            //}


            StackBarChartViewModel model1 = new();
            model1.name = "Expected";
            model1.data = data.Select(x => x.Expected).ToList();

            StackBarChartViewModel model2 = new();
            model2.name = "Actual";
            model2.data = data.Select(x => x.Actual).ToList();

            stackData.Add(model1);
            stackData.Add(model2);

            var cat = data.Select(x => x.GroupName).ToList();
            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = cat,
                stackBarValues = stackData,
            };
            return Json(chartData);
        }

        public async Task<IActionResult> MyRequest(string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string requestby = null, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            var result = await _serviceBusiness.GetAllServicesList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, false);
            ViewBag.UserId = _userContext.UserId;

            List<customIndexPageTemplateViewModel> list = new List<customIndexPageTemplateViewModel>();
            var colorList = new List<string> { "bg-gradient-red-yellow", "bg-gradient-aqua", "bg-gradient-green", "bg-gradient-red", "bg-gradient-purple", "bg-gradient-blue" };
            var iconColorList = new List<string> { "bg-solid-yellow", "bg-solid-aqua", "bg-solid-green", "bg-solid-red", "bg-solid-purple", "bg-solid-blue" };
            int i = 0;
            //foreach (var item in result.GroupBy(x => x.TemplateCode))
            //{
            //    int idx = i % 6;
            //    list.Add(new customIndexPageTemplateViewModel
            //    {
            //        ServiceName = item.Select(x => x.TemplateDisplayName).FirstOrDefault(),
            //        InProgressCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE"),
            //        CompletedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE"),
            //        RejectedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL"),
            //        TemplateIconId = item.Select(x => x.IconFileId).FirstOrDefault(),
            //        TemplateColorCss = colorList[idx],
            //        TemplateIconColorCss = iconColorList[idx]
            //    }); ;
            //    i++;
            //}

            foreach (var item in result)
            {
                int idx = i % 6;
                list.Add(new customIndexPageTemplateViewModel
                {
                    ServiceName = item.TemplateDisplayName,
                    InProgressCount = item.InprogressCount,
                    CompletedCount = item.CompletedCount,
                    RejectedCount = item.RejectedCount,
                    TemplateIconId = item.IconFileId,
                    TemplateColorCss = colorList[idx],
                    TemplateIconColorCss = iconColorList[idx]
                }); ;
                i++;
            }

            var model = new CustomIndexPageTemplateViewModel()
            {
                ServiceList = list,
                TemplateCodes = templateCodes,
                ModuleCodes = moduleCodes,
                CategoryCodes = categoryCodes,
                IsDisableCreate = isDisableCreate,
                ShowAllOwnersService = showAllOwnersService,
                PortalId = _userContext.PortalId,
            };

            var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
            ViewBag.SmartCityUrl = cs?.Value;

            return View(model);
        }

        public async Task<IActionResult> GetMyAllRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string search = null, DateTime? From = null, DateTime? To = null, string statusIds = null, string templateIds = null)
        {
            var dt = await _smartCityBusiness.GetMyAllRequestList(showAllOwnersService, moduleCodes, templateCodes, categoryCodes, search, From, To, statusIds, templateIds);
            return Json(dt);
        }

        public async Task<IActionResult> GetParcelByPropertyId(string propId, string year)
        {
            var res = await _smartCityBusiness.GetParcelByPropertyId(propId);
            //var yearDetails = await _smartCityBusiness.GetFinancialYearDetailsById(year);
            if (res.IsNotNull())
            {
                var taxAmount = await _smartCityBusiness.GetCalculatedPropertyTaxAmount(null, year);
                //if (taxAmount.Error.IsNotNullAndNotEmpty())
                //{
                //    res.Error = taxAmount.Error;
                //}
                //else
                //{
                //    res.Amount = taxAmount.Rate.ToString();
                //}

            }
            return Json(res);
        }
        public async Task<IActionResult> GetCalculatedPropertyTaxAmount(string propId, string year)
        {
            var data = await _smartCityBusiness.GetCalculatedPropertyTaxAmount(null, year);

            return Json(data);
        }
        public async Task<IActionResult> CitizenAssetMap()
        {
            return View();
        }

        public IActionResult ViewGeneratedBillDetails()
        {
            return View();
        }

        public async Task<IActionResult> GetPropertyTaxPaymentDetails(string propNo = null, string taskNo = null)
        {
            var res = await _smartCityBusiness.GetPropertyTaxPaymentDetails(propNo, taskNo);
            return Json(res);
        }

        public IActionResult CreateNewFactor()
        {
            return View(new LOVViewModel
            {
                DataAction = DataActionEnum.Create,
                LOVType = "JSC_FORMULA_TYPE",

            });
        }

        public IActionResult GarbageCollectionStatus()
        {
            return View();
        }

        public async Task<IActionResult> GetNoOfHouseholdCovered(string wardId = null, string collectorId = null, string revType = null)
        {
            var data = await _smartCityBusiness.GetJSCParcelDataForGarbageCollectionByWard(wardId);

            if (collectorId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.CollectorId == collectorId).ToList();
            }

            var res = data.Where(x => x.IsGarbageCollected == true).Count();
            var other = data.Where(x => x.IsGarbageCollected == false).Count();

            var result = new List<ProjectDashboardChartViewModel>
            {
                new ProjectDashboardChartViewModel { Type = "Garbage Collected", Value = res },
                new ProjectDashboardChartViewModel { Type = "Garbage Not Collected", Value = other }
            };

            var chartData = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = result.Select(x => x.Type).ToList(),
                ItemValueSeries = result.Select(x => x.Value).ToList(),
                TotalNoPropertyGarbageCollected = res.ToString(),
                TotalNoPropertyGarbageNotCollected = other.ToString(),
                TotalNoProperty = data.Count.ToString(),

            };
            return Json(chartData);
        }

        public IActionResult GarbageCollectionReport()
        {
            return View();
        }
        public async Task<IActionResult> GetAllGarbageCollectionData(string autoNo = null, string wardNo = null, string collector = null, DateTime? collectionDate = null)
        {
            var data = await _smartCityBusiness.GetAllGarbageCollectionData(autoNo, wardNo, collector, collectionDate);
            return Json(data);
        }

        public IActionResult GarbageCollectionMapView()
        {
            return View();
        }
        public IActionResult JSCGrievanceMapReport()
        {
            return View();
        }

        public IActionResult ViewDoorToDoorGarbageCollection()
        {
            return View();
        }
        public async Task<IActionResult> GetUserDoorToDoorGarbageCollectionData(DateTime? collectionDate = null)
        {
            var userId = _userContext.UserId;
            var garbageType = "Res_Com";
            if (collectionDate == null)
            {
                collectionDate = DateTime.Now;
            }
            var data = await _smartCityBusiness.GetUserDoorToDoorGarbageCollectionData(userId, garbageType, collectionDate);
            return Json(data);
        }
        public IActionResult ViewBWGGarbageCollection()
        {
            return View();
        }
        public IActionResult JSCGrievanceWardandComplaintTypeReport()
        {
            return View();
        }

        public IActionResult JSCGrievanceWardAndGrievanceTypeReport()
        {
            return View();
        }

        public IActionResult JSCGrievanceMapViewReport()
        {
            return View();
        }
        public async Task<IActionResult> GrievanceReportDeptWard()
        {
            var warddata = await _smartCityBusiness.GetWardList();
            var deptdata = await _smartCityBusiness.GetJSCDepartmentList();
            var wId = warddata.Where(x => x.Id == "62").Select(x => x.Id).FirstOrDefault();
            var dId = deptdata.Where(x => x.Name == "Buildings").Select(x => x.Id).FirstOrDefault();
            ViewBag.WardId = wId;
            ViewBag.DepartmentId = dId;
            return View();
        }
        public async Task<IActionResult> GrievanceReportDeptWardWise()
        {
            var warddata = await _smartCityBusiness.GetWardList();
            var deptdata = await _smartCityBusiness.GetJSCDepartmentList();
            var wId = warddata.Where(x => x.Id == "62").Select(x => x.Id).FirstOrDefault();
            var dId = deptdata.Where(x => x.Name == "Buildings").Select(x => x.Id).FirstOrDefault();
            ViewBag.WardId = wId;
            ViewBag.DepartmentId = dId;
            return View();
        }
        public IActionResult AutoWiseGarbageCollectedStatus()
        {
            return View();
        }
        public async Task<IActionResult> GetGrievanceReportDeptWardWiseData(string departmentId, string wardId)
        {
            var res = await _smartCityBusiness.GetGrievanceReportDeptWardData();
            var warddata = await _smartCityBusiness.GetWardList();
            var deptdata = await _smartCityBusiness.GetJSCDepartmentList();
            if (departmentId.IsNotNullAndNotEmpty())
            {
                var d = deptdata.Where(x => x.Id == departmentId).FirstOrDefault();
                res = res.Where(x => x.DepartmentId == departmentId).OrderBy(x => x.Ward).ToList();
                if (wardId.IsNotNullAndNotEmpty())
                {
                    res = res.Where(x => x.Ward == wardId).ToList();
                    if (res.Count == 0)
                    {
                        var w = warddata.Where(x => x.Id == wardId).FirstOrDefault();
                        res.Add(new JSCComplaintViewModel { Ward = w.Id, DepartmentId = d.Id, Department = d.Name });
                    }
                }
                else
                {

                    foreach (var w in warddata)
                    {
                        if (res.Where(x => x.Ward == w.Id).Any())
                        {
                            // do nothing
                        }
                        else
                        {
                            res.Add(new JSCComplaintViewModel { Ward = w.Id, DepartmentId = d.Id, Department = d.Name });
                        }
                    }
                    res = res.Where(x => x.DepartmentId == departmentId).OrderBy(x => x.Ward).ToList();
                }
            }
            var stackbarseries = new List<StackBarChartViewModel>();
            var stackbarcate = new List<string>();
            var s1 = new StackBarChartViewModel() { name = "Pending", data = new List<long>() };
            var s2 = new StackBarChartViewModel() { name = "In Progress", data = new List<long>() };
            var s3 = new StackBarChartViewModel() { name = "Not Pertaining", data = new List<long>() };
            var s4 = new StackBarChartViewModel() { name = "Disposed", data = new List<long>() };
            var colors = new List<string>();
            foreach (var item in res)
            {
                //stackbarcate.Add(item.Ward + "-" + item.Department);
                //stackbarcate.Add(warddata.Where(x => x.Id == item.Ward).Select(x => x.Name).FirstOrDefault());
                stackbarcate.Add("W-" + warddata.Where(x => x.Id == item.Ward).Select(x => x.Id).FirstOrDefault());
                if (item.StatusList.IsNotNullAndNotEmpty())
                {
                    var statusList = item.StatusList.Split(",").ToList();
                    var pendingCnt = statusList.Where(x => x == "GRV_PENDING").Count();
                    colors.Add("#FF2E2E");
                    s1.data.Add(pendingCnt);
                    var inProgressCnt = statusList.Where(x => x == "GRV_IN_PROGRESS").Count();
                    //colors.Add("#088FFA");
                    colors.Add("#FFBF00");
                    s2.data.Add(inProgressCnt);
                    var notPertaining = statusList.Where(x => x == "GRV_NOT_PERTAINING").Count();
                    colors.Add("#9AA394");
                    s3.data.Add(notPertaining);
                    var disposedCnt = statusList.Where(x => x == "GRV_DISPOSED").Count();
                    colors.Add("#228B22");
                    s4.data.Add(disposedCnt);
                }
                else
                {
                    var pendingCnt = 0;
                    colors.Add("#FF2E2E");
                    s1.data.Add(pendingCnt);
                    var inProgressCnt = 0;
                    //colors.Add("#088FFA");
                    colors.Add("#FFBF00");
                    s2.data.Add(inProgressCnt);
                    var notPertaining = 0;
                    colors.Add("#9AA394");
                    s3.data.Add(notPertaining);
                    var disposedCnt = 0;
                    colors.Add("#228B22");
                    s4.data.Add(disposedCnt);
                }
            }

            stackbarseries.Add(s1);
            stackbarseries.Add(s2);
            stackbarseries.Add(s3);
            stackbarseries.Add(s4);

            var chartData = new ProjectDashboardChartViewModel
            {
                StackBarItemValueSeries = stackbarseries,
                StackBarCategories = stackbarcate,
                Colors = colors
            };
            return Json(chartData);
        }
        public async Task<IActionResult> GetGrievanceReportDeptWardData(string wardId, string departmentId)
        {
            var res = await _smartCityBusiness.GetGrievanceReportDeptWardData();
            var deptdata = await _smartCityBusiness.GetJSCDepartmentList();
            if (wardId.IsNotNullAndNotEmpty())
            {
                res = res.Where(x => x.Ward == wardId).OrderBy(x => x.Department).ToList();
                if (departmentId.IsNotNullAndNotEmpty())
                {
                    res = res.Where(x => x.DepartmentId == departmentId).ToList();
                    if (res.Count == 0)
                    {
                        var d = deptdata.Where(x => x.Id == departmentId).FirstOrDefault();
                        res.Add(new JSCComplaintViewModel { Ward = wardId, DepartmentId = d.Id, Department = d.Name });
                    }
                }
                else
                {

                    foreach (var d in deptdata)
                    {
                        if (res.Where(x => x.DepartmentId == d.Id).Any())
                        {
                            // do nothing
                        }
                        else
                        {
                            res.Add(new JSCComplaintViewModel { Ward = wardId, DepartmentId = d.Id, Department = d.Name });
                        }
                    }
                    res = res.Where(x => x.Ward == wardId).OrderBy(x => x.Department).ToList();
                }
            }
            var stackbarseries = new List<StackBarChartViewModel>();
            var stackbarcate = new List<string>();
            var s1 = new StackBarChartViewModel() { name = "Pending", data = new List<long>() };
            var s2 = new StackBarChartViewModel() { name = "In Progress", data = new List<long>() };
            var s3 = new StackBarChartViewModel() { name = "Not Pertaining", data = new List<long>() };
            var s4 = new StackBarChartViewModel() { name = "Disposed", data = new List<long>() };
            var colors = new List<string>();
            foreach (var item in res)
            {
                //stackbarcate.Add(item.Ward + "-" + item.Department);
                stackbarcate.Add(item.Department);
                if (item.StatusList.IsNotNullAndNotEmpty())
                {
                    var statusList = item.StatusList.Split(",").ToList();
                    var pendingCnt = statusList.Where(x => x == "GRV_PENDING").Count();
                    colors.Add("#FF2E2E");
                    //colors.Add("#ff6c6c");
                    s1.data.Add(pendingCnt);
                    var inProgressCnt = statusList.Where(x => x == "GRV_IN_PROGRESS").Count();
                    //colors.Add("#088FFA"); //blue
                    colors.Add("#FFBF00"); //amber
                    //colors.Add("#ffd24c");
                    s2.data.Add(inProgressCnt);
                    var notPertaining = statusList.Where(x => x == "GRV_NOT_PERTAINING").Count();
                    colors.Add("#9AA394");
                    //colors.Add("#b8beb4");
                    s3.data.Add(notPertaining);
                    var disposedCnt = statusList.Where(x => x == "GRV_DISPOSED").Count();
                    colors.Add("#228B22");
                    //colors.Add("#64ad64");
                    s4.data.Add(disposedCnt);
                }
                else
                {
                    var pendingCnt = 0;
                    colors.Add("#FF2E2E");
                    //colors.Add("#ff6c6c");
                    s1.data.Add(pendingCnt);
                    var inProgressCnt = 0;
                    //colors.Add("#088FFA"); //blue
                    colors.Add("#FFBF00"); //amber
                    //colors.Add("#ffd24c");
                    s2.data.Add(inProgressCnt);
                    var notPertaining = 0;
                    colors.Add("#9AA394");
                    //colors.Add("#b8beb4");
                    s3.data.Add(notPertaining);
                    var disposedCnt = 0;
                    colors.Add("#228B22");
                    //colors.Add("#64ad64");
                    s4.data.Add(disposedCnt);
                }
            }

            stackbarseries.Add(s1);
            stackbarseries.Add(s2);
            stackbarseries.Add(s3);
            stackbarseries.Add(s4);

            var chartData = new ProjectDashboardChartViewModel
            {
                StackBarItemValueSeries = stackbarseries,
                StackBarCategories = stackbarcate,
                Colors = colors
            };
            return Json(chartData);
        }
        public async Task<IActionResult> GrievanceReportDashboard()
        {
            var warddata = await _smartCityBusiness.GetWardList();
            var deptdata = await _smartCityBusiness.GetJSCDepartmentList();
            var wId = warddata.Where(x => x.Id == "62").Select(x => x.Id).FirstOrDefault();
            var dId = deptdata.Where(x => x.Name == "Buildings").Select(x => x.Id).FirstOrDefault();
            ViewBag.WardId = wId;
            ViewBag.DepartmentId = dId;
            return View();
        }
        public async Task<IActionResult> GrievanceReportComplaintList()
        {

            ViewBag.CategoryCodes = "JSC_GRIEVANCE_SERVICE";
            //ViewBag.ShowAllTaskForAdmin = showAllTaskForAdmin;
            //var isAdmin = _userContext.IsSystemAdmin;
            var fromDate = DateTime.Today.AddMonths(-6);
            var toDate = DateTime.Today;
            var list = await _smartCityBusiness.GetGrievanceReportComplaintListData(null, null, null, null, fromDate, toDate, null);
            ViewBag.DisposedCount = list.Where(x => x.GrvStatusCode == "GRV_DISPOSED").Count();
            ViewBag.PendingCount = list.Where(x => x.GrvStatusCode == null || x.GrvStatusCode == "GRV_PENDING").Count();
            ViewBag.InProgressCount = list.Where(x => x.GrvStatusCode == "GRV_IN_PROGRESS").Count();
            ViewBag.NotPertainedCount = list.Where(x => x.GrvStatusCode == "GRV_NOT_PERTAINING").Count();
            return View();
        }
        public async Task<IActionResult> ReadGrievanceReportComplaintListData(string wardId, string departmentId, string complaintTypeId, string statusCode, DateTime fromDate, DateTime toDate, string complaintNo)
        {
            //isAdmin = _userContext.IsSystemAdmin;
            var list = await _smartCityBusiness.GetGrievanceReportComplaintListData(wardId, departmentId, complaintTypeId, statusCode, fromDate, toDate, complaintNo);
            foreach (var item in list)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }

            //if (status.IsNotNullAndNotEmpty())
            //{
            //    if (status == "GRV_PENDING")
            //    {
            //        list = list.Where(x => x.GrvStatusCode == status || x.GrvStatusCode == null).ToList();
            //    }
            //    else
            //    {
            //        list = list.Where(x => x.GrvStatusCode == status).ToList();
            //    }
            //}

            return Json(list);
        }
        public IActionResult GrievanceReportAging()
        {
            ViewBag.ReportTitle = "Grievance Report - Aging";
            ViewBag.ReportTypeCode = "AGING_WISE";
            return View();
        }
        public IActionResult GrievanceReportDepartment(string typeCode)
        {
            ViewBag.ReportTitle = "";
            ViewBag.ReportTypeCode = typeCode;
            if (typeCode == "DEPARTMENT_WISE")
            {
                ViewBag.ReportTitle = "Grievance Report - Department Report";
            }
            else if (typeCode == "WARD_WISE")
            {
                ViewBag.ReportTitle = "Grievance Report - Ward Report";
            }
            else if (typeCode == "COMPLAINTTYPE_WISE")
            {
                ViewBag.ReportTitle = "Grievance Report - Complaint Type Wise";
            }
            else if (typeCode == "STATUS_WISE")
            {
                ViewBag.ReportTitle = "Grievance Report - Complaint Status Wise";
            }
            else if (typeCode == "EMPLOYEE_WISE")
            {
                ViewBag.ReportTitle = "Grievance Report - Employee Wise";
            }
            return View();
        }
        public async Task<IActionResult> GetGrievanceReportDeptTurnaroundTimeTrendData()
        {
            var fromDate = DateTime.Today.AddMonths(-1).FirstDateOfMonth();
            var toDate = DateTime.Today.AddMonths(-1).LastDateOfMonth();
            var chartData = new ProjectDashboardChartViewModel();
            chartData.StackBarItemValueSeries = new List<StackBarChartViewModel>();
            var maxtrenddata = new List<long>(); // { 45, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10 };
            var avgtrenddata = new List<long>(); // { 35, 41, 62, 42, 13, 18, 29, 37, 36, 51, 32, 35 };
            var mintrenddata = new List<long>(); // { 87, 57, 74, 99, 75, 38, 62, 47, 82, 56, 45, 47 };
            var catgtrend = new List<string>(); // { "01 Jan", "02 Jan", "03 Jan", "04 Jan", "05 Jan", "06 Jan", "07 Jan", "08 Jan", "09 Jan", "10 Jan", "11 Jan", "12 Jan" };
            var res = await _smartCityBusiness.GetGrievanceReportDeptTurnaroundTimeData("", "", "", fromDate, toDate);
            res = res.OrderBy(x => x.CreatedDate).ToList();
            var checkDate = fromDate;
            var itemdata = res.GroupBy(x => x.CreatedDate.Date).Select(gr => new JSCComplaintViewModel { CreatedDate = gr.Key, TrendDateText = gr.Key.ToDD_MM_YYYY(), MaxDays = Convert.ToInt64(gr.Max(x => x.NoOfDaysDisposed)), AverageDays = Convert.ToInt64(gr.Average(x => x.NoOfDaysDisposed)), MinDays = Convert.ToInt64(gr.Min(x => x.NoOfDaysDisposed)) }).ToList();
            while (checkDate.Date <= toDate.Date)
            {
                var item = itemdata.Where(x => x.CreatedDate.Date == checkDate.Date).FirstOrDefault();
                if (item != null)
                {
                    maxtrenddata.Add(item.MaxDays);
                    avgtrenddata.Add(item.AverageDays);
                    mintrenddata.Add(item.MinDays);
                    catgtrend.Add(item.TrendDateText);
                }
                else
                {
                    maxtrenddata.Add(0);
                    avgtrenddata.Add(0);
                    mintrenddata.Add(0);
                    catgtrend.Add(checkDate.ToDD_MM_YYYY());
                }
                checkDate = checkDate.AddDays(1);
            }

            //foreach (var item in itemdata)
            //{
            //    maxtrenddata.Add(item.MaxDays);
            //    avgtrenddata.Add(item.AverageDays);
            //    mintrenddata.Add(item.MinDays);
            //    catgtrend.Add(item.TrendDateText);
            //}

            var avgtrend = new StackBarChartViewModel { name = "Average", data = avgtrenddata };
            chartData.StackBarItemValueSeries.Add(avgtrend);
            //chartData.Colors.Add("#088FFA");
            var mintrend = new StackBarChartViewModel { name = "Minimum", data = mintrenddata };
            chartData.StackBarItemValueSeries.Add(mintrend);
            //chartData.Colors.Add("#228B22");
            var maxtrend = new StackBarChartViewModel { name = "Maximum", data = maxtrenddata };
            chartData.StackBarItemValueSeries.Add(maxtrend);
            //chartData.Colors.Add("#FF2E2E");

            chartData.ItemValueLabel = catgtrend;

            //chartData.Colors = new List<string> { "#FF2E2E" };
            chartData.XaxisTitle = "";
            return Json(chartData);
        }
        public async Task<IActionResult> GetGrievanceReportDeptTurnaroundTimeData(string typeCode, string departmentId, string wardId, DateTime fromDate, DateTime toDate,string employeeId)
        {
            var chartData = new ProjectDashboardChartViewModel();
            var todayDate = DateTime.Today;
            var res = await _smartCityBusiness.GetGrievanceReportDeptTurnaroundTimeData(typeCode, departmentId, wardId, fromDate, toDate);
            if (employeeId.IsNotNullAndNotEmpty())
            {
                res = res.Where(x => x.Level1UserId == employeeId).ToList();
            }
            var resdata = new List<JSCComplaintViewModel>();
            //if (departmentId.IsNullOrEmpty() && wardId.IsNullOrEmpty())
            //{
            //    resdata = res.GroupBy(x => x.Department).Select(gr => new JSCComplaintViewModel { Department = gr.Key, NoOfDaysDisposed = Convert.ToInt64(gr.Average(x => x.NoOfDaysDisposed)) }).ToList();
            //    chartData.ItemValueLabel = resdata.Select(x => x.Department).ToList(); // agingDays;
            //    chartData.XaxisTitle = "Department(s)";
            //}
            //else if (departmentId.IsNullOrEmpty() && wardId.IsNotNullAndNotEmpty())
            //{
            //    resdata = res.GroupBy(x => x.Ward).Select(gr => new JSCComplaintViewModel { Ward = gr.Key, NoOfDaysDisposed = Convert.ToInt64(gr.Average(x => x.NoOfDaysDisposed)) }).ToList();
            //    chartData.ItemValueLabel = resdata.Select(x => x.Ward).ToList(); // agingDays;
            //    chartData.XaxisTitle = "Ward(s)";
            //}

            resdata = res.GroupBy(x => x.Department).Select(gr => new JSCComplaintViewModel { Department = gr.Key, NoOfDaysDisposed = Convert.ToInt64(gr.Average(x => x.NoOfDaysDisposed)) }).ToList();
            chartData.ItemValueLabel = resdata.Select(x => x.Department).ToList(); // agingDays;
            chartData.XaxisTitle = "Department(s)";

            chartData.BarItemValueSeries = new List<BarChartViewModel>();
            chartData.BarItemValueSeries.Add(new BarChartViewModel { data = resdata.Select(x => x.NoOfDaysDisposed).ToList() });
            chartData.Colors = new List<string> { "#228B22" };

            return Json(chartData);
        }
        public async Task<IActionResult> GetGrievanceReportAgingData(string typeCode)
        {
            var chartData = new ProjectDashboardChartViewModel();
            var todayDate = DateTime.Today;
            var res = await _smartCityBusiness.GetGrievanceReportAgingData(typeCode, todayDate, todayDate);
            var agingDays = new List<string> { "0-5", "6-10", "11-15", "16-20", "21-25", "26-30", "31-Above" };
            var pendigcount = new List<long>();
            pendigcount.Add(res.Where(x => x.NoOfDaysPending <= 5).Count());
            pendigcount.Add(res.Where(x => x.NoOfDaysPending >= 6 && x.NoOfDaysPending <= 10).Count());
            pendigcount.Add(res.Where(x => x.NoOfDaysPending >= 11 && x.NoOfDaysPending <= 15).Count());
            pendigcount.Add(res.Where(x => x.NoOfDaysPending >= 16 && x.NoOfDaysPending <= 20).Count());
            pendigcount.Add(res.Where(x => x.NoOfDaysPending >= 21 && x.NoOfDaysPending <= 25).Count());
            pendigcount.Add(res.Where(x => x.NoOfDaysPending >= 26 && x.NoOfDaysPending <= 30).Count());
            pendigcount.Add(res.Where(x => x.NoOfDaysPending >= 31).Count());
            chartData.ItemValueLabel = agingDays;
            chartData.BarItemValueSeries = new List<BarChartViewModel>();
            chartData.BarItemValueSeries.Add(new BarChartViewModel { data = pendigcount });
            chartData.Colors = new List<string> { "#FF2E2E" };
            chartData.XaxisTitle = "Days";
            return Json(chartData);
        }
        public async Task<IActionResult> GrievanceReportDeptWardStackBar(string typeCode)
        {
            ViewBag.ReportTitle = "";
            ViewBag.ReportTypeCode = typeCode;
            if (typeCode == "DEPARTMENT_STATUS_WISE")
            {
                ViewBag.ReportTitle = "Department Wise";
            }
            else if (typeCode == "WARD_STATUS_WISE")
            {
                ViewBag.ReportTitle = "Ward Wise";
            }
            return View();
        }
        public async Task<IActionResult> GrievanceReportZoneWise(string typeCode)
        {
            ViewBag.ReportTitle = "";
            ViewBag.ReportTypeCode = typeCode;
            if (typeCode == "ZONE_WISE")
            {
                ViewBag.ReportTitle = "Zone Wise";
            }
            return View();
        }
        public async Task<IActionResult> GetGrievanceReportDepartmentWiseData(string typeCode, string wardId, string departmentId, string grievanceTypeId, string grvStatusId, string zoneId, DateTime fromDate, DateTime toDate)
        {
            var chartData = new ProjectDashboardChartViewModel();
            var res = await _smartCityBusiness.GetGrievanceReportDepartmentWiseData(typeCode, fromDate, toDate);
            var deptdata = await _smartCityBusiness.GetJSCDepartmentList();
            var warddata = await _smartCityBusiness.GetWardList();
            var zonedata = await _smartCityBusiness.GetJSCZoneList();
            if (res.IsNotNull())
            {
                if (typeCode == "DEPARTMENT_WISE")
                {
                    chartData.XaxisTitle = "Department(s)";
                    chartData.ItemValueLabel = new List<string>();
                    if (departmentId.IsNotNullAndNotEmpty())
                    {
                        res = res.Where(x => x.DepartmentId == departmentId).ToList();
                    }
                    else
                    {
                        foreach (var d in deptdata)
                        {
                            if (res.Where(x => x.DepartmentId == d.Id).Any())
                            {
                                // do nothing
                            }
                            else
                            {
                                res.Add(new JSCComplaintViewModel { DepartmentId = d.Id, Department = d.Name, ComplaintCount = 0 });
                            }
                        }
                        res = res.OrderBy(x => x.Department).ToList();
                    }
                    chartData.ItemValueLabel.AddRange(res.Select(x => x.Department));
                    chartData.BarItemValueSeries = new List<BarChartViewModel>();
                    chartData.BarItemValueSeries.Add(new BarChartViewModel { data = res.Select(x => x.ComplaintCount).ToList() });
                }
                else if (typeCode == "DEPARTMENT_STATUS_WISE")
                {
                    if (departmentId.IsNotNullAndNotEmpty())
                    {
                        res = res.Where(x => x.DepartmentId == departmentId).ToList();
                    }
                    var stackbarseries = new List<StackBarChartViewModel>();
                    var stackbarcate = new List<string>();
                    var s1 = new StackBarChartViewModel() { name = "Pending", data = new List<long>() };
                    var s2 = new StackBarChartViewModel() { name = "In Progress", data = new List<long>() };
                    var s3 = new StackBarChartViewModel() { name = "Not Pertaining", data = new List<long>() };
                    var s4 = new StackBarChartViewModel() { name = "Disposed", data = new List<long>() };
                    var colors = new List<string>();
                    foreach (var item in res)
                    {
                        //stackbarcate.Add(item.Ward + "-" + item.Department);
                        stackbarcate.Add(item.Department);
                        if (item.StatusList.IsNotNullAndNotEmpty())
                        {
                            var statusList = item.StatusList.Split(",").ToList();
                            var pendingCnt = statusList.Where(x => x == "GRV_PENDING").Count();
                            colors.Add("#FF2E2E");
                            //colors.Add("#ff6c6c");
                            s1.data.Add(pendingCnt);
                            var inProgressCnt = statusList.Where(x => x == "GRV_IN_PROGRESS").Count();
                            //colors.Add("#088FFA"); //blue
                            colors.Add("#FFBF00"); //amber
                                                   //colors.Add("#ffd24c");
                            s2.data.Add(inProgressCnt);
                            var notPertaining = statusList.Where(x => x == "GRV_NOT_PERTAINING").Count();
                            colors.Add("#9AA394");
                            //colors.Add("#b8beb4");
                            s3.data.Add(notPertaining);
                            var disposedCnt = statusList.Where(x => x == "GRV_DISPOSED").Count();
                            colors.Add("#228B22");
                            //colors.Add("#64ad64");
                            s4.data.Add(disposedCnt);
                        }
                        else
                        {
                            var pendingCnt = 0;
                            colors.Add("#FF2E2E");
                            //colors.Add("#ff6c6c");
                            s1.data.Add(pendingCnt);
                            var inProgressCnt = 0;
                            //colors.Add("#088FFA"); //blue
                            colors.Add("#FFBF00"); //amber
                                                   //colors.Add("#ffd24c");
                            s2.data.Add(inProgressCnt);
                            var notPertaining = 0;
                            colors.Add("#9AA394");
                            //colors.Add("#b8beb4");
                            s3.data.Add(notPertaining);
                            var disposedCnt = 0;
                            colors.Add("#228B22");
                            //colors.Add("#64ad64");
                            s4.data.Add(disposedCnt);
                        }
                    }

                    stackbarseries.Add(s1);
                    stackbarseries.Add(s2);
                    stackbarseries.Add(s3);
                    stackbarseries.Add(s4);

                    chartData = new ProjectDashboardChartViewModel
                    {
                        StackBarItemValueSeries = stackbarseries,
                        StackBarCategories = stackbarcate,
                        Colors = colors,
                        XaxisTitle = "Department(s)"
                    };
                }
                else if (typeCode == "WARD_WISE")
                {
                    chartData.XaxisTitle = "Ward(s)";
                    var wardList = await _smartCityBusiness.GetWardList();
                    if (wardId.IsNotNullAndNotEmpty())
                    {
                        res = res.Where(x => x.Ward == wardId).ToList();
                    }
                    foreach (var item in res)
                    {
                        item.Ward = wardList.Where(x => x.Id == item.Ward).Select(x => x.Name).FirstOrDefault();
                    }
                    chartData.ItemValueLabel = new List<string>();
                    chartData.ItemValueLabel.AddRange(res.Select(x => x.Ward));
                    //chartData.ItemValueSeries = new List<long>();
                    //chartData.ItemValueSeries.AddRange(res.Select(x => x.ComplaintCount));
                    chartData.BarItemValueSeries = new List<BarChartViewModel>();
                    chartData.BarItemValueSeries.Add(new BarChartViewModel { data = res.Select(x => x.ComplaintCount).ToList() });
                    chartData.Colors = new List<string> { "#008FFB", "#00E396", "#FEB019", "#FF4560", "#775DD0", "#3F51B5", "#03A9F4", "#F9CE1D", "#546E7A", "#D4526E", "#13D8AA", "#A5978B", "#4ECDC4", "#C7F464", "#81D4FA", "#F9A3A4" };
                }
                else if (typeCode == "WARD_STATUS_WISE")
                {
                    if (wardId.IsNotNullAndNotEmpty())
                    {
                        res = res.Where(x => x.Ward == wardId).ToList();
                    }
                    var stackbarseries = new List<StackBarChartViewModel>();
                    var stackbarcate = new List<string>();
                    var s1 = new StackBarChartViewModel() { name = "Pending", data = new List<long>() };
                    var s2 = new StackBarChartViewModel() { name = "In Progress", data = new List<long>() };
                    var s3 = new StackBarChartViewModel() { name = "Not Pertaining", data = new List<long>() };
                    var s4 = new StackBarChartViewModel() { name = "Disposed", data = new List<long>() };
                    var colors = new List<string>();
                    foreach (var item in res)
                    {
                        //stackbarcate.Add(item.Ward + "-" + item.Department);
                        //stackbarcate.Add(warddata.Where(x => x.Id == item.Ward).Select(x => x.Name).FirstOrDefault());
                        stackbarcate.Add("W-" + warddata.Where(x => x.Id == item.Ward).Select(x => x.Id).FirstOrDefault());
                        if (item.StatusList.IsNotNullAndNotEmpty())
                        {
                            var statusList = item.StatusList.Split(",").ToList();
                            var pendingCnt = statusList.Where(x => x == "GRV_PENDING").Count();
                            colors.Add("#FF2E2E");
                            s1.data.Add(pendingCnt);
                            var inProgressCnt = statusList.Where(x => x == "GRV_IN_PROGRESS").Count();
                            //colors.Add("#088FFA");
                            colors.Add("#FFBF00");
                            s2.data.Add(inProgressCnt);
                            var notPertaining = statusList.Where(x => x == "GRV_NOT_PERTAINING").Count();
                            colors.Add("#9AA394");
                            s3.data.Add(notPertaining);
                            var disposedCnt = statusList.Where(x => x == "GRV_DISPOSED").Count();
                            colors.Add("#228B22");
                            s4.data.Add(disposedCnt);
                        }
                        else
                        {
                            var pendingCnt = 0;
                            colors.Add("#FF2E2E");
                            s1.data.Add(pendingCnt);
                            var inProgressCnt = 0;
                            //colors.Add("#088FFA");
                            colors.Add("#FFBF00");
                            s2.data.Add(inProgressCnt);
                            var notPertaining = 0;
                            colors.Add("#9AA394");
                            s3.data.Add(notPertaining);
                            var disposedCnt = 0;
                            colors.Add("#228B22");
                            s4.data.Add(disposedCnt);
                        }
                    }

                    stackbarseries.Add(s1);
                    stackbarseries.Add(s2);
                    stackbarseries.Add(s3);
                    stackbarseries.Add(s4);

                    chartData = new ProjectDashboardChartViewModel
                    {
                        StackBarItemValueSeries = stackbarseries,
                        StackBarCategories = stackbarcate,
                        Colors = colors,
                        XaxisTitle = "Ward(s)"
                    };
                }
                else if (typeCode == "ZONE_WISE")
                {
                    if (departmentId.IsNotNullAndNotEmpty())
                    {
                        res = res.Where(x => x.DepartmentId == departmentId).ToList();
                        if (zoneId.IsNotNullAndNotEmpty())
                        {
                            res = res.Where(x => x.ZoneId == zoneId).ToList();
                        }
                    }

                    var stackbarseries = new List<StackBarChartViewModel>();
                    var stackbarcate = new List<string>();
                    var s1 = new StackBarChartViewModel() { name = "Pending", data = new List<long>() };
                    var s2 = new StackBarChartViewModel() { name = "In Progress", data = new List<long>() };
                    var s3 = new StackBarChartViewModel() { name = "Not Pertaining", data = new List<long>() };
                    var s4 = new StackBarChartViewModel() { name = "Disposed", data = new List<long>() };
                    var colors = new List<string>();
                    foreach (var item in res)
                    {
                        //stackbarcate.Add(item.Ward + "-" + item.Department);
                        //stackbarcate.Add(warddata.Where(x => x.Id == item.Ward).Select(x => x.Name).FirstOrDefault());
                        //stackbarcate.Add("Z-" + warddata.Where(x => x.Id == item.Ward).Select(x => x.Id).FirstOrDefault());
                        stackbarcate.Add(item.ZoneName);
                        if (item.StatusList.IsNotNullAndNotEmpty())
                        {
                            var statusList = item.StatusList.Split(",").ToList();
                            var pendingCnt = statusList.Where(x => x == "GRV_PENDING").Count();
                            colors.Add("#FF2E2E");
                            s1.data.Add(pendingCnt);
                            var inProgressCnt = statusList.Where(x => x == "GRV_IN_PROGRESS").Count();
                            //colors.Add("#088FFA");
                            colors.Add("#FFBF00");
                            s2.data.Add(inProgressCnt);
                            var notPertaining = statusList.Where(x => x == "GRV_NOT_PERTAINING").Count();
                            colors.Add("#9AA394");
                            s3.data.Add(notPertaining);
                            var disposedCnt = statusList.Where(x => x == "GRV_DISPOSED").Count();
                            colors.Add("#228B22");
                            s4.data.Add(disposedCnt);
                        }
                        else
                        {
                            var pendingCnt = 0;
                            colors.Add("#FF2E2E");
                            s1.data.Add(pendingCnt);
                            var inProgressCnt = 0;
                            //colors.Add("#088FFA");
                            colors.Add("#FFBF00");
                            s2.data.Add(inProgressCnt);
                            var notPertaining = 0;
                            colors.Add("#9AA394");
                            s3.data.Add(notPertaining);
                            var disposedCnt = 0;
                            colors.Add("#228B22");
                            s4.data.Add(disposedCnt);
                        }
                    }

                    stackbarseries.Add(s1);
                    stackbarseries.Add(s2);
                    stackbarseries.Add(s3);
                    stackbarseries.Add(s4);

                    chartData = new ProjectDashboardChartViewModel
                    {
                        StackBarItemValueSeries = stackbarseries,
                        StackBarCategories = stackbarcate,
                        Colors = colors,
                        XaxisTitle = "Zone(s)"
                    };
                }
                else if (typeCode == "COMPLAINTTYPE_WISE")
                {
                    chartData.XaxisTitle = "Complaint Type(s)";
                    var grevList = await _smartCityBusiness.GetJSCGrievanceTypeList();
                    if (grievanceTypeId.IsNotNullAndNotEmpty())
                    {
                        res = res.Where(x => x.GrievanceTypeId == grievanceTypeId).ToList();
                    }
                    else
                    {
                        //foreach (var g in grevList)
                        //{
                        //    if (res.Where(x => x.GrievanceTypeId == g.Id).Any())
                        //    {
                        //        // do nothing
                        //    }
                        //    else
                        //    {
                        //        res.Add(new JSCComplaintViewModel { GrievanceTypeId = g.Id, GrievanceType = g.Name, ComplaintCount = 0 });
                        //    }
                        //}
                        res = res.OrderBy(x => x.GrievanceType).ToList();
                    }
                    //chartData.ItemValueLabel = new List<string>() { "Complaint 1", "Complaint 2", "Complaint 3" };
                    //chartData.ItemValueSeries = new List<long>() { 44, 55, 13 };
                    chartData.ItemValueLabel = new List<string>();
                    chartData.ItemValueLabel.AddRange(res.Select(x => x.GrievanceType));
                    //chartData.ItemValueSeries = new List<long>();
                    //chartData.ItemValueSeries.AddRange(res.Select(x => x.ComplaintCount).ToList());
                    chartData.BarItemValueSeries = new List<BarChartViewModel>();
                    chartData.BarItemValueSeries.Add(new BarChartViewModel { data = res.Select(x => x.ComplaintCount).ToList() });
                }
                else if (typeCode == "STATUS_WISE")
                {
                    chartData.XaxisTitle = "Complaint Status";
                    var statusList = await _lOVBusiness.GetList(x => x.LOVType == "JSC_GRV_STATUS" && x.IsDeleted == false);
                    if (grvStatusId.IsNotNullAndNotEmpty())
                    {
                        res = res.Where(x => x.GrvStatusId == grvStatusId).ToList();
                    }
                    else
                    {
                        foreach (var s in statusList)
                        {
                            if (res.Where(x => x.GrvStatusId == s.Id).Any())
                            {
                                // do nothing
                            }
                            else
                            {
                                res.Add(new JSCComplaintViewModel { GrvStatusId = s.Id, GrvStatus = s.Name, ComplaintCount = 0 });
                            }

                        }
                        foreach (var r in res)
                        {
                            if (r.GrvStatus == "Pending")
                            {
                                r.SequenceNo = 1;
                            }
                            if (r.GrvStatus == "In Progress")
                            {
                                r.SequenceNo = 2;
                            }
                            if (r.GrvStatus == "Not Pertaining")
                            {
                                r.SequenceNo = 3;
                            }
                            if (r.GrvStatus == "Disposed")
                            {
                                r.SequenceNo = 4;
                            }
                        }
                        res = res.OrderBy(x => x.SequenceNo).ToList();
                    }

                    chartData.ItemValueLabel = new List<string>();
                    chartData.ItemValueLabel.AddRange(res.Select(x => x.GrvStatus));
                    chartData.ItemValueSeries = new List<long>();
                    chartData.Colors = new List<string>();
                    chartData.ItemValueSeries.AddRange(res.Select(x => x.ComplaintCount).ToList());
                    if (res.IsNotNull() && res.Count > 0)
                    {
                        foreach (var r in res)
                        {
                            if (r.GrvStatus == "Pending")
                            {
                                chartData.Colors.Add("#FF2E2E");
                                //chartData.Colors.Add("#ff6c6c");
                            }
                            if (r.GrvStatus == "In Progress")
                            {
                                //chartData.Colors.Add("#088FFA");
                                chartData.Colors.Add("#FFBF00");
                                //chartData.Colors.Add("#ffd24c");
                            }
                            if (r.GrvStatus == "Not Pertaining")
                            {
                                chartData.Colors.Add("#9AA394");
                                //chartData.Colors.Add("#b8beb4");
                            }
                            if (r.GrvStatus == "Disposed")
                            {
                                chartData.Colors.Add("#228B22");
                                //chartData.Colors.Add("#64ad64");
                            }
                        }
                    }
                }
                else if (typeCode == "EMPLOYEE_WISE")
                {
                    chartData.ItemValueLabel = new List<string>() { "Employee 1", "Employee 2", "Employee 3" };
                    chartData.ItemValueSeries = new List<long>() { 44, 55, 13 };
                }
            }
            return Json(chartData);
        }
        //public async Task<IActionResult> JSCGrievanceWardandComplaintTypeReport(string wardId = null, string complaintType = null, string complaintNo = null, string mobileNo = null, string name = null, string operatorId = null)
        //{
        //    wardId = "62";
        //    var data = await _smartCityBusiness.GetJSCGrievanceReport(wardId);

        //    var res = data.Where(x => x.IsGarbageCollected == true).Count();
        //    var other = data.Where(x => x.IsGarbageCollected == false).Count();

        //    var result = new List<ProjectDashboardChartViewModel>
        //    {
        //        new ProjectDashboardChartViewModel { Type = "Garbage Collected", Value = res },
        //        new ProjectDashboardChartViewModel { Type = "Garbage Not Collected", Value = other }
        //    };

        //    var chartData = new ProjectDashboardChartViewModel
        //    {
        //        ItemValueLabel = result.Select(x => x.Type).ToList(),
        //        ItemValueSeries = result.Select(x => x.Value).ToList(),
        //    };
        //    return Json(chartData);
        //}

        //public async Task<IActionResult> JSCGrievanceMapReport(string wardId = null, string complaintType = null, string complaintNo = null, string mobileNo = null, string name = null, string operatorId = null)
        //{
        //    wardId = "62";
        //    var data = await _smartCityBusiness.GetJSCGrievanceReport(wardId);

        //    var res = data.Where(x => x.IsGarbageCollected == true).Count();
        //    var other = data.Where(x => x.IsGarbageCollected == false).Count();

        //    var result = new List<ProjectDashboardChartViewModel>
        //    {
        //        new ProjectDashboardChartViewModel { Type = "Garbage Collected", Value = res },
        //        new ProjectDashboardChartViewModel { Type = "Garbage Not Collected", Value = other }
        //    };

        //    var chartData = new ProjectDashboardChartViewModel
        //    {
        //        ItemValueLabel = result.Select(x => x.Type).ToList(),
        //        ItemValueSeries = result.Select(x => x.Value).ToList(),
        //    };
        //    return Json(chartData);
        //}

        public IActionResult RegisterNewAsset()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterNewAsset(JSCAssetViewModel model)
        {
            var property = await _smartCityBusiness.GetParcelByDDNNO(model.DDNNO);
            if (property.IsNotNull())
            {
                var assets = await _smartCityBusiness.GetJSCRegisteredAssetsList(_userContext.UserId);
                var exist = assets.Where(x => x.mmi_id == model.DDNNO).FirstOrDefault();
                if (exist.IsNotNull())
                {
                    return Json(new { success = false, error = "Property already registered" });
                }
                else
                {
                    var res = await _smartCityBusiness.RegisterNewAsset(model);
                    return Json(new { success = res.IsSuccess, error = res.Messages, data = res.Item });
                }
            }
            else
            {
                return Json(new { success = false, error = "Property not found" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerityRegisterAssetOTP(string otp, string ddnNo, string serviceId)
        {
            var property = await _smartCityBusiness.GetParcelByDDNNO(ddnNo);
            if (property.IsNotNull())
            {
                var user = await _userBusiness.GetSingle(x => x.Mobile == property.tel_no);
                if (user.TwoFactorAuthOTP == otp)
                {
                    ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                    serviceModel.ActiveUserId = _userContext.UserId;
                    serviceModel.DataAction = DataActionEnum.Edit;
                    serviceModel.ServiceId = serviceId;
                    var service = await _serviceBusiness.GetServiceDetails(serviceModel);

                    service.DataAction = DataActionEnum.Edit;
                    service.ActiveUserId = _userContext.UserId;
                    service.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";

                    var res = await _serviceBusiness.ManageService(service);
                    return Json(new { success = res.IsSuccess, error = res.Messages });
                }
                else
                {
                    return Json(new { success = false, error = "Invalid OTP" });
                }
            }
            return Json(new { success = false, error = "Property Not Found" });
        }

        public async Task<IActionResult> GetJSCRegisteredAssetsList(string userId)
        {
            var data = await _smartCityBusiness.GetJSCRegisteredAssetsList(userId);
            return Json(data);
        }

        public IActionResult ManageUserIndex()
        {
            return View();
        }

        public async Task<IActionResult> GetUserList()
        {
            var data = await _userBusiness.GetUserListForPortal();
            return Json(data);
        }

        public async Task<IActionResult> ManageUser(string userId)
        {
            var data = await _userBusiness.GetSingleById(userId);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUser(UserViewModel model)
        {
            var userdata = await _cmsBusiness.GetDataListByTemplate("JSC_USER", "", $@"and ""F_JAMMU_SMART_CITY_JSCUser"".""UserId"" = '{model.Id}' ");
            DateTime? effDate = null;

            var temp = await _templateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_USER");
            if (temp.IsNotNull())
            {
                var formTemplate = await _templateBusiness.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                formTemplate.TemplateCode = "JSC_USER";
                formTemplate.DataAction = DataActionEnum.Create;

                formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("UserId", model.Id);
                ((IDictionary<String, Object>)exo).Add("EmailId", model.Email);
                ((IDictionary<String, Object>)exo).Add("Name", model.Name);
                ((IDictionary<String, Object>)exo).Add("MobileNo", model.Mobile);
                ((IDictionary<String, Object>)exo).Add("EffectiveDate", DateTime.Now);

                if (userdata.Rows.Count > 0)
                {
                    var jscuserid = "";
                    foreach (DataRow d in userdata.Rows)
                    {
                        jscuserid = Convert.ToString(d["Id"]);
                        effDate = Convert.ToDateTime(d["EffectiveDate"]);
                    }

                    if (effDate.Value.Date == DateTime.Now.Date)
                    {
                        //((IDictionary<String, Object>)exo).Add("Id", jscuserid);
                        formTemplate.RecordId = jscuserid;
                        formTemplate.DataAction = DataActionEnum.Edit;
                    }
                }

                formTemplate.Json = JsonConvert.SerializeObject(exo);
                var result = await _cmsBusiness.ManageForm(formTemplate);
            }

            var userdetail = await _userBusiness.GetSingleById(model.Id);
            userdetail.Name = model.Name;
            userdetail.Email = model.Email;
            userdetail.Mobile = model.Mobile;
            var res = await _userBusiness.Edit(userdetail);

            return Json(new { success = res.IsSuccess, error = res.Messages });

        }

        public IActionResult CreateJSCUser()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageJSCUser(UserViewModel model)
        {
            model.DataAction = DataActionEnum.Create;
            model.PortalId = _userContext.PortalId;
            model.LegalEntityIds = new[] { _userContext.LegalEntityId };

            var userres = await _userBusiness.Create(model);

            if (userres.IsSuccess)
            {
                var temp = await _templateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_USER");
                if (temp.IsNotNull())
                {
                    var formTemplate = await _templateBusiness.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                    formTemplate.TemplateCode = "JSC_USER";
                    formTemplate.DataAction = DataActionEnum.Create;

                    formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("UserId", userres.Item.Id);
                    ((IDictionary<String, Object>)exo).Add("EmailId", model.Email);
                    ((IDictionary<String, Object>)exo).Add("Name", model.Name);
                    ((IDictionary<String, Object>)exo).Add("MobileNo", model.Mobile);
                    ((IDictionary<String, Object>)exo).Add("EffectiveDate", DateTime.Now);

                    formTemplate.Json = JsonConvert.SerializeObject(exo);
                    var result = await _cmsBusiness.ManageForm(formTemplate);
                }
                return Json(new { success = true });
            }
            return Json(new { success = userres.IsSuccess, error = userres.Messages });
        }

        public IActionResult GrievanceAgingReport()
        {
            return View();
        }

        public async Task<IActionResult> GetGrievanceReport(GrievanceDatefilters datefilters, string startDate, string endDate)
        {
            var data = await _smartCityBusiness.GetGrievanceReportData(datefilters, startDate, endDate);
            foreach (var item in data)
            {
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                }
            }

            return Json(data);
        }

        public IActionResult JSCMyComplaint()
        {
            return View();
        }

        public async Task<IActionResult> GetJSCMyComplaint()
        {
            var data = await _smartCityBusiness.GetJSCMyComplaint();
            return Json(data);
        }

        public async Task<IActionResult> JSCComplaintList(bool showAllTaskForAdmin = false)
        {

            ViewBag.CategoryCodes = "JSC_GRIEVANCE_SERVICE";
            ViewBag.ShowAllTaskForAdmin = showAllTaskForAdmin;
            var isAdmin = _userContext.IsSystemAdmin;
            var list = await _smartCityBusiness.GetJSCComplaintForResolver(isAdmin, true);
            ViewBag.DisposedCount = list.Where(x => x.GrvStatusCode == "GRV_DISPOSED").Count();
            ViewBag.PendingCount = list.Where(x => x.GrvStatusCode == null || x.GrvStatusCode == "GRV_PENDING").Count();
            ViewBag.InProgressCount = list.Where(x => x.GrvStatusCode == "GRV_IN_PROGRESS").Count();
            ViewBag.NotPertainedCount = list.Where(x => x.GrvStatusCode == "GRV_NOT_PERTAINING").Count();
            return View();
        }

        public async Task<IActionResult> ReadComplaintData(bool isAdmin, bool isUpperLevel, string status)
        {
            isAdmin = _userContext.IsSystemAdmin;
            var list = await _smartCityBusiness.GetJSCComplaintForResolver(isAdmin, isUpperLevel);
            foreach (var item in list)
            {
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                }
            }

            if (status.IsNotNullAndNotEmpty())
            {
                if (status == "GRV_PENDING")
                {
                    list = list.Where(x => x.GrvStatusCode == status || x.GrvStatusCode == null).ToList();
                }
                else
                {
                    list = list.Where(x => x.GrvStatusCode == status).ToList();
                }
            }

            return Json(list);
        }

        //public async Task<IActionResult> ReadComplaintData(string categoryCodes = null, string tempCodes = null, bool showAllTaskForAdmin = false, string userId = null)
        //{
        //    if (userId == null)
        //    {
        //        userId = _userContext.UserId;
        //    }
        //    var list = await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, null, _userContext.PortalId, showAllTaskForAdmin, tempCodes, null, userId);
        //    //var j = Json(list.ToDataSourceResult(request));
        //    var j = Json(list);
        //    return j;
        //}

        public async Task<IActionResult> RegisterComplaint(bool showAllTaskForAdmin = false)
        {

            ViewBag.TemplateCode = "JSC_LODGECOMPLAINT";
            ViewBag.ShowAllTaskForAdmin = showAllTaskForAdmin;
            return View();
        }

        public async Task<IActionResult> ReadRegisterComplaint(string tempCodes = null, string userId = null)
        {

            var list = await _smartCityBusiness.GetComplaintslist(tempCodes, _userContext.UserId);
            var j = Json(list);
            return j;

        }

        public async Task<IActionResult> RegisterNewComplaint()
        {

            ViewBag.TemplateCode = "JSC_LODGECOMPLAINT";
            return View();
        }
        public async Task<IActionResult> ReadComplaintDataWithMobileNumber(string tempCodes = null, string userId = null)
        {
            if (userId.IsNotNullAndNotEmpty())
            {
                var list = await _smartCityBusiness.GetComplaintslist(tempCodes, userId);
                foreach (var item in list)
                {
                    var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                    if (flagDetails.IsNotNull())
                    {
                        item.FlagDetails = flagDetails.ToList();
                        item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                        item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                        item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    }
                }
                var j = Json(list);
                return j;
            }
            return Json(new List<JSCComplaintViewModel>());
        }

        [HttpGet]
        public async Task<ActionResult> CheckUserExistingWithMobileNo(string mobileNo)
        {
            if (mobileNo.IsNotNullAndNotEmpty())
            {
                var userDetails = await _userBusiness.GetSingle(x => x.Name == mobileNo);
                if (userDetails.IsNotNull())
                {
                    return Json(new { userId = userDetails.Id });
                }
            }
            return Json(new { userId = "" });
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserWithMobileNo(string mobileNo)
        {
            if (mobileNo.IsNotNullAndNotEmpty())
            {
                var userDetails = await _userBusiness.GetSingle(x => x.Name == mobileNo);
                if (userDetails.IsNotNull())
                {
                    return Json(new { success = true, userId = userDetails.Id });
                }
                var userModel = new UserViewModel();
                userModel.Name = mobileNo;
                userModel.Email = mobileNo;
                userModel.Mobile = mobileNo;
                userModel.Status = StatusEnum.Active;
                var res = await _userBusiness.Create(userModel);
                if (res.IsSuccess)
                {
                    return Json(new { success = true, userId = res.Item.Id });
                }
            }
            return Json(new { success = false });
        }
        public async Task<IActionResult> JSCCommunityHall()
        {
            var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            var bookbydata = await _lOVBusiness.GetList(x => x.LOVType == "JSC_HALLBOOKINGBY" && x.IsDeleted == false);
            var paymentMode = await _lOVBusiness.GetList(x => x.LOVType == "JSC_PAYMENT_MODE" && x.IsDeleted == false);
            if (portal?.Name == "JammuSmartCityCustomer")
            {
                ViewBag.BookingBy = bookbydata.Where(x => x.Code == "JSC_HALLBOOKINGBY_CITIZEN").Select(x => x.Id).FirstOrDefault();
                ViewBag.PaymentMode = paymentMode.Where(x => x.Code == "JSC_ONLINE").Select(x => x.Id).FirstOrDefault();
            }
            else
            {               
                ViewBag.PaymentMode = paymentMode.Where(x => x.Code == "JSC_OFFLINE").Select(x => x.Id).FirstOrDefault();
                ViewBag.BookingBy = bookbydata.Where(x => x.Code == "JSC_HALLBOOKINGBY_SINGLEWINDOW").Select(x => x.Id).FirstOrDefault();
            }
            var dateselecttype = await _lOVBusiness.GetList(x => x.LOVType == "JSC_DATE_SELECTION_TYPE" && x.IsDeleted == false);
            ViewBag.DateTypeRange = dateselecttype.Where(x => x.Code == "JSC_DATE_RANGE").Select(x => x.Id).FirstOrDefault();
            ViewBag.DateTypeMulti = dateselecttype.Where(x => x.Code == "JSC_MULTIPLE_DATES").Select(x => x.Id).FirstOrDefault();
            return View();
        }

        public async Task<IActionResult> JSCCommunityHallBooking(string hallId,string bookingById, string dtseltype,DateTime? fdate,DateTime? tdate,string multidate,string rate,string paymentMode)
        {
            JSCCommunityHallBookingViewModel model = new JSCCommunityHallBookingViewModel();
            //model.Id = hallId;
            model.BookingById = bookingById;
            model.DateSelectionType = dtseltype;
            model.BookingFromDate = fdate;
            model.BookingToDate = tdate;
            model.MultipleDates = multidate;
            //model.GrandTotal = Convert.ToDouble(rate);
            model.Amount = Convert.ToDouble(rate);
            model.Name = _userContext.Name;
            model.Email = _userContext.Email;
            model.PaymentMode = paymentMode;
            //return View(model);

            var halldetails = await _smartCityBusiness.GetJSCCommunityHallDetailsById(hallId);
            var hallrate = halldetails.StandardRate;
            var discper = halldetails.JMCDiscountPercentage;
            if (halldetails.SpecialRate>0)
            {
                hallrate = halldetails.SpecialRate;
            }
            var commhalllist = new List<CommunityHallBooking>();
            var dateselecttype = await _lOVBusiness.GetSingleById(model.DateSelectionType);
            var revenuelist = await _smartCityBusiness.GetJSCRevenueTypeList();
            if (revenuelist != null)
            {
                model.RevenueTypeId = revenuelist.Where(x => x.Code == "COMMUNITY_HALL_COLLECTION").Select(x => x.Id).FirstOrDefault();
            }
            if (dateselecttype.Code == "JSC_DATE_RANGE")
            {
                var nodays = (model.BookingToDate.Value.Date - model.BookingFromDate.Value.Date).Days + 1;
                var tamt = nodays * hallrate;
                commhalllist.Add(new CommunityHallBooking
                {
                    CommunityHallId = hallId,
                    CommunityBookingFromDate = model.BookingFromDate.Value,
                    CommunityBookingToDate = model.BookingToDate.Value,
                    NoOfDays = nodays,
                    Rate = hallrate,
                    TotalAmount = tamt
                });
                model.GrandTotal = tamt;
                model.Amount = tamt;
            }

           
            if (dateselecttype.Code == "JSC_MULTIPLE_DATES")
            {
                var grandtotal = 0.0;
                var tamt = 0.0;
                var multipledate = model.MultipleDates.Split(",");
                foreach (var dt in multipledate)
                {
                    var nodays = (Convert.ToDateTime(dt) - Convert.ToDateTime(dt)).Days + 1;
                    tamt = nodays * hallrate;
                    commhalllist.Add(new CommunityHallBooking
                    {
                        CommunityHallId = hallId,
                        CommunityBookingFromDate = Convert.ToDateTime(dt),
                        CommunityBookingToDate = Convert.ToDateTime(dt),
                        NoOfDays = nodays,
                        Rate = hallrate,
                        TotalAmount = tamt
                    });
                    grandtotal = grandtotal + tamt;                    
                }
                model.GrandTotal = grandtotal;
                model.Amount = grandtotal;
            }
            model.JMCDiscountPercentage = discper;

            model.CommunityHallBookingList = commhalllist;
            model.JSC_CommunityHallBooking = Newtonsoft.Json.JsonConvert.SerializeObject(model.CommunityHallBookingList);
            var serviceTempModel = new ServiceTemplateViewModel();
            serviceTempModel.DataAction = DataActionEnum.Create;
            serviceTempModel.ActiveUserId = _userContext.UserId;
            serviceTempModel.TemplateCode = "JSC_COMMUNITY_HALL_SERVICE";
            var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
            serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            serviceModel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
            serviceModel.DataAction = DataActionEnum.Create;
            //serviceModel.ServiceId = model.ServiceId;
            var result = await _serviceBusiness.ManageService(serviceModel);
            if (result.IsSuccess)
            {
                model.ServiceId = result.Item.ServiceId;
                model.ServiceNo = result.Item.ServiceNo;
                model.UdfNoteTableId = result.Item.UdfNoteTableId;
                return View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> SearchJSCCommunityHallList(string communityHallId, string wardId)
        {
            var data = await _smartCityBusiness.SearchJSCCommunityHallList(communityHallId,wardId);
            return Json(data);
        }
        public async Task<IActionResult> ReadCommunityHallList(string type, DateTime? start = null, DateTime? end = null, string[] dates = null)
        {
            var data = await _smartCityBusiness.GetCommunityHallList(type, start, end, dates);
            return Json(data);  
        }
        public async Task<IActionResult> GetJSCCommunityHallDetailsById(string communityHallId)
        {
            var data = await _smartCityBusiness.GetJSCCommunityHallDetailsById(communityHallId);
            return Json(data);
        }
        public async Task<IActionResult> GetJSCCommunityHallIdNameList(string wardId)
        {
            var data = await _smartCityBusiness.GetJSCCommunityHallIdNameList(wardId);
            return Json(data);
        }
        public async Task<IActionResult> GetJSCFunctionTypeIdNameList()
        {
            var data = await _smartCityBusiness.GetJSCFunctionTypeIdNameList();
            return Json(data);
        }
        public async Task<IActionResult> ViewJSCCommunityHallDetails(string communityHallId)
        {
            var model = await _smartCityBusiness.GetJSCCommunityHallDetailsById(communityHallId);
            return View(model);
        }
        public async Task<IActionResult> ViewJSCCommunityHallPhotos(string communityHallId)
        {
            var model = await _smartCityBusiness.GetJSCCommunityHallPhotos(communityHallId);
            return View(model);
        }
        public async Task<IActionResult> GetJSCCommunityHallServiceChildData(string parentId)
        {
            var data = await _smartCityBusiness.GetJSCCommunityHallServiceChildData(parentId);
            foreach (var item in data)
            {
                item.CommunityBookingFromDateText = item.CommunityBookingFromDate.ToDD_MM_YYYY();
                item.CommunityBookingToDateText = item.CommunityBookingToDate.ToDD_MM_YYYY();
            }
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageJSCCommunityHallBooking(JSCCommunityHallBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                    var serviceTempModel = new ServiceTemplateViewModel();
                    serviceTempModel.DataAction = DataActionEnum.Edit;
                    serviceTempModel.SetUdfValue = true;
                    serviceTempModel.ActiveUserId = _userContext.UserId;
                    //serviceTempModel.TemplateCode = "JSC_COMMUNITY_HALL_SERVICE";
                    serviceTempModel.ServiceId = model.ServiceId;
                    var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                    var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                    rowData["Name"] = model.Name;
                    rowData["Email"] = model.Email;
                    rowData["Mobile"] = model.Mobile;
                    rowData["PANNo"] = model.PANNo;
                    rowData["Aadhar"] = model.Aadhar;
                    rowData["IsJmcEmployeeId"] = model.IsJmcEmployeeId;
                    rowData["FunctionTypeId"] = model.FunctionTypeId;
                    rowData["Amount"] = model.Amount;
                    //rowData["JMCDiscountPercentage"] = model.JMCDiscountPercentage;
                    serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    serviceModel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    serviceModel.DataAction = DataActionEnum.Edit;
                    
                    var result = await _serviceBusiness.ManageService(serviceModel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, data=result.Item });
                    }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> JSCCommunityHallDirectPayment(string serviceId)
        {
            var paydetails = await _smartCityBusiness.GetPaymentDetailsByServiceId(serviceId);
            var paydata = paydetails.FirstOrDefault();
            if (paydata != null)
            {
                return Json(new { success = true, data = paydata });
            }
            return Json(new { success = false, error = "error contact admin" });
        }
        public async Task<IActionResult> JSCCommunityHallReportBooking()
        {
            return View();
        }
        public async Task<IActionResult> JSCCommunityHallReportAvailable()
        {
            return View();
        }
        public async Task<IActionResult> GetCommunityHallReportData(bool isAvailable, string hallId, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetCommunityHallList("DateRange",fromDate,toDate,null);
            var list = data.Where(x=>x.IsAvailable== isAvailable).ToList();
            if (hallId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.CommunityHallId == hallId).ToList();
            }
            return Json(list);
        }
        public IActionResult BBPSRegisterTiles(string serviceType)
        {
            var model = new BBPSRegisterViewModel { ServiceType = serviceType };
            return View(model);
        }

        public async Task<IActionResult> GetBBPSRegisterList(string serviceType)
        {
            var data = await _smartCityBusiness.GetBBPSRegisterList(serviceType);
            return Json(data);

        }
        public IActionResult GrievanceComments(string serviceId, bool IsAddCommentEnabled)
        {
            var model = new NtsServiceCommentViewModel();
            model.NtsServiceId = serviceId;
            model.IsAddCommentEnabled = IsAddCommentEnabled;
            return View(model);
        }
        public async Task<IActionResult> ManageGrievanceForm(string complaintId, string ownerUserId)
        {
            var details = new ServiceTemplateViewModel();
            var complaintDetails = new JSCComplaintViewModel();
            if (complaintId.IsNotNullAndNotEmpty())
            {
                details = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel { ServiceId = complaintId, OwnerUserId = ownerUserId });
                complaintDetails = await _smartCityBusiness.GetJSCMyComplaintById(details.ServiceId);
            }

            var parcelDetails = await _smartCityBusiness.GetParcelByDDNNO(complaintDetails.DDN);
            if (parcelDetails.IsNotNull())
            {
                var geom = JsonConvert.DeserializeObject<dynamic>(parcelDetails.geometry).coordinates[0][0][0];
                complaintDetails.DDNLat = geom[0];
                complaintDetails.DDNLong = geom[1];

            }

            if (_userContext.UserId == complaintDetails.Level1User)
            {
                complaintDetails.IsComplaintResolver = true;
                complaintDetails.IsLevelUser = true;
            }
            else
            {
                complaintDetails.IsComplaintResolver = false;
                complaintDetails.IsLevelUser = false;

            }

            if (_userContext.UserRoleCodes.Contains("COMPLAINT_OPERATOR"))
            {
                complaintDetails.IsComplaintOperator = true;
            }
            else
            {
                complaintDetails.IsComplaintOperator = false;
            }
            if (_userContext.UserId == complaintDetails.Level2User)
            {
                complaintDetails.IsLevelUser = true;
            }
            if (_userContext.UserId == complaintDetails.Level3User)
            {
                complaintDetails.IsLevelUser = true;
            }
            if (_userContext.UserId == complaintDetails.Level4User)
            {
                complaintDetails.IsLevelUser = true;
            }

            var reopenDetails = await _smartCityBusiness.GetReopenComplaintDetails(complaintDetails.ComplaintId);
            if (reopenDetails.IsNotNull())
            {
                complaintDetails.ReopenCount = reopenDetails.Count;
            }
            if (complaintDetails.GrvStatusId.IsNullOrEmpty())
            {
                var lovRes = await _lOVBusiness.GetSingle(x => x.Code == "GRV_PENDING");
                complaintDetails.GrvStatusId = lovRes.IsNotNull() ? lovRes.Id : null;
                complaintDetails.GrvStatus = lovRes.IsNotNull() ? lovRes.Id : null;
            }

            var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(complaintDetails.ComplaintId);
            if (flagDetails.IsNotNull())
            {
                var res = flagDetails.Where(x => x.CreatedBy == _userContext.UserId).FirstOrDefault();
                if (res.IsNotNull())
                {
                    complaintDetails.IsFlag = true;
                    complaintDetails.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    complaintDetails.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    complaintDetails.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                }
                else
                {
                    complaintDetails.IsFlag = false;
                }
            }
            else
            {
                complaintDetails.IsFlag = false;
            }
            reopenDetails.ToList().ForEach(i =>
            {
                i.ReopenDateTime = i.ReopenDateTime.ToSafeDateTime().ToDefaultDateTimeFormat();
            });
            flagDetails.ToList().ForEach(i =>
            {
                i.FlagDateTime = i.FlagDateTime.ToSafeDateTime().ToDefaultDateTimeFormat();
            });
            complaintDetails.FlagDetails = flagDetails.ToList();
            complaintDetails.ReopenDetails = reopenDetails.ToList();
            complaintDetails.IsEdit = complaintId.IsNotNullAndNotEmpty() ? true : false;
            return View(complaintDetails);
        }

        public async Task<IActionResult> JSCHome(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalId = null, string selectedCatCode = null)
        {
            var model = new TemplateViewModel();
            model.Code = templateCode;
            model.CategoryCode = categoryCode;
            model.UserId = userId;
            model.ModuleCodes = moduleCodes;
            model.Prms = prms;
            model.CallBackMethodName = cbm;
            model.TemplateIds = templateIds;
            model.CategoryIds = categoryIds;
            model.TemplateCategoryType = categoryType;
            model.SelectedCategoryCode = selectedCatCode;
            if (model.SelectedCategoryCode.IsNotNullAndNotEmpty())
            {
                var cat = await _portalBusiness.GetSingle<TemplateCategoryViewModel, TemplateCategory>
                    (x => x.Code == model.SelectedCategoryCode);
                model.SelectedCategoryName = cat?.Name;
            }

            if (portalId.IsNotNullAndNotEmpty())
            {
                var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                if (portal?.Name == "EGovCustomer")
                {
                    var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                    ViewBag.SmartCityUrl = cs.Value;
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageComplaint(JSCComplaintViewModel model)
        {
            var serTempModel = new ServiceTemplateViewModel
            {
                DataAction = model.DataAction,
                ActiveUserId = _userContext.UserId,
                TemplateCode = "JSC_LODGECOMPLAINT",
                CreatedBy = _userContext.UserId,
                OwnerUserId = _userContext.UserId,
            };
            var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);
            sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
            sermodel.DataAction = DataActionEnum.Create;
            var result = await _serviceBusiness.ManageService(sermodel);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ManageComplaintResolverInput(string id, string status, string documentId)
        {
            var details = await _smartCityBusiness.UpdateResolverInput(id, status, documentId);
            return Json(details);
        }

        [HttpPost]
        public async Task<IActionResult> ComplaintMarkFlag(string id)
        {
            var details = await _smartCityBusiness.ComplaintMarkFlag(id);
            return Json(details);
        }

        [HttpPost]
        public async Task<IActionResult> ReopenComplaint(string parentId, string documents)
        {
            var details = await _smartCityBusiness.ReopenComplaint(parentId, documents);
            return Json(details);
        }

        public async Task<IActionResult> GetReopenComplaintDetails(string parentId)
        {
            var details = await _smartCityBusiness.GetReopenComplaintDetails(parentId);
            return Json(details);
        }

        public async Task<ActionResult> ReadGrievanceCommentDataList(string serviceId, bool isLevelUser)
        {
            var model = await _smartCityBusiness.GetAllGrievanceComment(serviceId, isLevelUser);

            return Ok(model);
        }

        public async Task<ActionResult> CheckIfDDNExist(string ddn)
        {
            var model = await _smartCityBusiness.CheckIfDDNExist(ddn);
            return Ok(model);
        }

        public async Task<IActionResult> UpdateDepartmentByOperator(string id, string departmentId, string grievanceTypeId)
        {
            var details = await _smartCityBusiness.UpdateDepartmentByOperator(id, departmentId, grievanceTypeId);
            return Json(details);
        }
        public async Task<IActionResult> MarkDisposedByOperator(string id)
        {
            var details = await _smartCityBusiness.MarkDisposedByOperator(id);
            return Json(details);
        }

        public async Task<IActionResult> GetComplaintCountByStatus()
        {
            var list = await _smartCityBusiness.GetJSCComplaintForResolver(true, true);
            var DisposedCount = list.Where(x => x.GrvStatusCode == "GRV_DISPOSED").Count();
            var PendingCount = list.Where(x => x.GrvStatusCode == null || x.GrvStatusCode == "GRV_PENDING").Count();
            var InProgressCount = list.Where(x => x.GrvStatusCode == "GRV_IN_PROGRESS").Count();
            var NotPertainedCount = list.Where(x => x.GrvStatusCode == "GRV_NOT_PERTAINING").Count();
            var result = new { DisposedCount = DisposedCount, PendingCount = PendingCount, InProgressCount = InProgressCount, NotPertainedCount = NotPertainedCount };
            return Json(result);
        }

        public async Task<IActionResult> GetMSWAutoDetails(string id)
        {
            var data = await _smartCityBusiness.GetMSWAutoDetails(id);
            return Json(data);
        }
        public async Task<IActionResult> GetBWGAutoDetails(string id)
        {
            var data = await _smartCityBusiness.GetBWGAutoDetails(id);
            return Json(data);
        }
        public async Task<IActionResult> ShowMSWReport()
        {
            var transferStationList = await _smartCityBusiness.GetTransferStationList();
            var transferStationId = transferStationList.Where(x => x.Code == _userContext.UserId).Select(x => x.Id).Distinct().FirstOrDefault();
            ViewBag.TransferStationId = transferStationId;
            return View();
        }

        public async Task<IActionResult> ShowBWGReport()
        {
            var transferStationList = await _smartCityBusiness.GetTransferStationList();
            var transferStationId = transferStationList.Where(x => x.Code == _userContext.UserId).Select(x => x.Id).Distinct().FirstOrDefault();
            ViewBag.TransferStationId = transferStationId;
            return View();
        }
        public async Task<IActionResult> GetMSWReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId)
        {
            var autoList = await _smartCityBusiness.GetJSCAutoListByTransferStation(transferStationId);
            //string[] idList = autoList.Select(x => x.Id).ToArray();
            //var ids = String.Join("','", idList);
            var data = await _smartCityBusiness.GetMSWReportDetails(autoId, startDate, endDate, transferStationId);
            return Json(data);
        }

        public async Task<IActionResult> GetBWGReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId)
        {
            var autoList = await _smartCityBusiness.GetJSCAutoListByTransferStation(transferStationId);
            var data = await _smartCityBusiness.GetBWGReportDetails(autoId, startDate, endDate, transferStationId);
            return Json(data);
        }

        public async Task<IActionResult> TransferPointOutWardRegisterReport()
        {
            var transferStationList = await _smartCityBusiness.GetTransferStationList();
            var transferStationId = transferStationList.Where(x => x.Code == _userContext.UserId).Select(x => x.Id).Distinct().FirstOrDefault();
            ViewBag.TransferStationId = transferStationId;
            return View();
        }
        public async Task<IActionResult> TransferPointInWardRegisterReport()
        {
            var transferStationList = await _smartCityBusiness.GetTransferStationList();
            var transferStationId = transferStationList.Where(x => x.Code == _userContext.UserId).Select(x => x.Id).Distinct().FirstOrDefault();
            ViewBag.TransferStationId = transferStationId;
            return View();
        }

        public async Task<IActionResult> GetAutoWiseGarbageCollectedData(string autoId)
        {
            var chartData = new ProjectDashboardChartViewModel();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            var autoDetails = await _smartCityBusiness.GetMSWAutoDetails(autoId);
            var data = await _smartCityBusiness.GetMSWReportDetails(autoId, startDate, endDate, autoDetails.TransferStationId);
            if (data.IsNotNull())
            {
                //chartData.ItemValueLabel = new List<string>();
                //chartData.BarItemValueSeries = new List<BarChartViewModel>();

                //foreach (var i in data)
                //{
                //    chartData.ItemValueLabel.Add("Resedential Garbage Collected");
                //    chartData.BarItemValueSeries.Add(new BarChartViewModel { data = data.NoOfHouseHold.to });
                //};
            }
            return Json(data);
        }

        public async Task<IActionResult> GetJSCAutoList()
        {
            var data = await _smartCityBusiness.GetJSCAutoList();
            return Json(data);
        }

        public async Task<IActionResult> GetJSCCollectorList()
        {
            var data = await _smartCityBusiness.GetBinCollectorNameList();
            return Json(data);
        }

        public async Task<IActionResult> GetJSCAutoListByTransferStation(string transferStationId)
        {
            var data = await _smartCityBusiness.GetJSCAutoListByTransferStation(transferStationId);
            return Json(data);
        }

        public async Task<IActionResult> GetComplaintZoneStatusData(string zone, string status, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetComplaintZoneStatusData(zone, status, fromDate, toDate);
            foreach (var item in data)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }
            return Json(data);
        }
        public async Task<IActionResult> GetComplaintWardDepartmentStatusData(string warddept, string status, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetComplaintWardDepartmentStatusData(warddept, status, fromDate, toDate);
            foreach (var item in data)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }
            return Json(data);
        }
        public async Task<IActionResult> GetComplaintByWardAndDepartmentWithStatusDetails(string department, string status)
        {
            var data = await _smartCityBusiness.GetComplaintByWardAndDepartmentWithStatusDetails(department, status);
            foreach (var item in data)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }
            return Json(data);
        }

        public async Task<IActionResult> GetComplaintReportAgingData(string name, string reportType, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetGrievanceReportAgingData(reportType, fromDate, toDate);
            if (name.IsNotNullAndNotEmpty())
            {
                var days = name.Split("-")[0];
                if (days == "0")
                {
                    data = data.Where(x => x.NoOfDaysPending <= 5).ToList();
                }
                else if (days == "6")
                {
                    data = data.Where(x => x.NoOfDaysPending >= 6 && x.NoOfDaysPending <= 10).ToList();
                }
                else if (days == "11")
                {
                    data = data.Where(x => x.NoOfDaysPending >= 11 && x.NoOfDaysPending <= 15).ToList();
                }
                else if (days == "16")
                {
                    data = data.Where(x => x.NoOfDaysPending >= 16 && x.NoOfDaysPending <= 20).ToList();
                }
                else if (days == "21")
                {
                    data = data.Where(x => x.NoOfDaysPending >= 21 && x.NoOfDaysPending <= 25).ToList();
                }
                else if (days == "26")
                {
                    data = data.Where(x => x.NoOfDaysPending >= 26 && x.NoOfDaysPending <= 30).ToList();
                }
                else if (days == "31")
                {
                    data = data.Where(x => x.NoOfDaysPending >= 31).ToList();
                }
            }
            foreach (var item in data)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }
            return Json(data);
        }
        public async Task<IActionResult> GetComplaintReportData(string name, string reportType, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityBusiness.GetComplaintReportData(name, reportType, fromDate, toDate);
            foreach (var item in data)
            {
                var a = "";
                item.CreatedDateText = item.CreatedDate.ToDD_MM_YYYY();
                var flagDetails = await _smartCityBusiness.GetFlagComplaintDetails(item.ComplaintId);
                if (flagDetails.IsNotNull())
                {
                    item.FlagDetails = flagDetails.ToList();
                    item.IsFlagByLevel2 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_2") ? true : false;
                    item.IsFlagByLevel3 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_3") ? true : false;
                    item.IsFlagByLevel4 = _userContext.UserRoleCodes.Contains("COMPLAINT_RESOLVER_LEVEL_4") ? true : false;
                    item.LevelUserRole = _userContext.UserRoleCodes;
                    foreach (var f in flagDetails)
                    {
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_2"))
                        {
                            a = a + "<span style='color:dodgerblue;padding-left:2px;font-weight:bold' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 2 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_3"))
                        {
                            a = a + "<span style='color:orange;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 3 </span>";
                        }
                        if (f.LevelUserRole.Contains("COMPLAINT_RESOLVER_LEVEL_4"))
                        {
                            a = a + "<span style='color:red;padding-left:2px' class='fas fa-flag-pennant' title='" + f.OwnerUserName + "'> 4 </span>";
                        }
                    }
                    if (item.ReopenCount > 0)
                    {
                        a = a + " <span title='Reopened' style='color:blue;cursor:pointer' class='fas fa-rotate-right'></span>";
                    }
                    if (item.GrvStatusCode == null || item.GrvStatusCode == "GRV_PENDING")
                    {
                        a = a + "<input title='View Details' style='font-weight:bold' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    else
                    {
                        a = a + "<input title='View Details' type='button' class='btn btn-link' onclick='OpenTask(\"" + item.Id + "\",\"" + item.TemplateCode + "\" )' value=\"" + item.ServiceNo + "\" />";
                    }
                    item.ComplaintNoText = a;
                }
            }
            return Json(data);
        }

        public async Task<IActionResult> GetCollectorListByWard(string wardId)
        {
            var data = await _smartCityBusiness.GetCollectorListByWard(wardId);
            return Json(data);
        }

        public IActionResult JSCEmployeeLandingPage()
        {
            return View();
        }

        public IActionResult JSCSanitationDashboard()
        {
            return View();
        }

        public async Task<IActionResult> GetGarbageCollectionInNoInPie()
        {
            var chartData = new ProjectDashboardChartViewModel();
            var res = await _smartCityBusiness.GetGarbageCollectedAndNotCollectedList();
            chartData.XaxisTitle = "Garbage Collection Status";

            chartData.ItemValueLabel = new List<string>
            {
                "Collected",
                "Not Collected"
            };
            var data = new List<long>
            {
                res.Where(x => x.IsGarbageCollected == true).Count(),
                res.Where(x => x.IsGarbageCollected == false).Count()
            };
            chartData.ItemValueSeries = data;
            return Json(chartData);
        }
        public async Task<IActionResult> GetGarbageCollectionInNoInBar()
        {
            List<StackBarChartViewModel> stackData = new();
            var stackbarseries = new List<StackBarChartViewModel>();
            var stackbarcate = new List<string>();
            var s1 = new StackBarChartViewModel() { name = "Collected", data = new List<long>() };
            var s2 = new StackBarChartViewModel() { name = "Not Collected", data = new List<long>() };

            stackbarseries.Add(s1);
            stackbarseries.Add(s2);
            var warddata = await _smartCityBusiness.GetWardList();

            foreach (var item in warddata)
            {
                var res = await _smartCityBusiness.GetGarbageCollectedAndNotCollectedListByWard(item.Id);

                stackbarcate.Add(item.Name);

                var collectCnt = res.Where(x => x.IsGarbageCollected == true).Count();
                s1.data.Add(collectCnt);
                var notCollectedCnt = res.Where(x => x.IsGarbageCollected == false).Count();
                s2.data.Add(notCollectedCnt);
            }

            var chartData = new ProjectDashboardChartViewModel
            {
                StackBarItemValueSeries = stackbarseries,
                StackBarCategories = stackbarcate,
            };
            return Json(chartData);

        }
        public async Task<IActionResult> GetGarbageCollectionDateByPropertyType()
        {
            var chartData = new ProjectDashboardChartViewModel();
            var res = await _smartCityBusiness.GetGarbageCollectionDateByPropertyType();

            //chartData.BarItemValueSeries = new List<BarChartViewModel>();
            chartData.StackBarItemValueSeries = new List<StackBarChartViewModel>();
            var data1 = new List<long>
            {
                //60,
                //20
                res.Where(x => x.IsGarbageCollected == true && x.GarbageTypeName == "Residential").Count(),
                res.Where(x => x.IsGarbageCollected == true && x.GarbageTypeName == "Commercial").Count(),
            };

            var data2 = new List<long>
            {
                //120,
                //50
                res.Where(x => x.IsGarbageCollected == false && (x.GarbageTypeName == "Residential" ||  x.GarbageTypeName == null)).Count(),
                res.Where(x => x.IsGarbageCollected == false && x.GarbageTypeName == "Commercial").Count()
            };
            //chartData.BarItemValueSeries.Add(new BarChartViewModel { data = data1.ToList(), name = "Collected" });
            //chartData.BarItemValueSeries.Add(new BarChartViewModel { data = data2.ToList(), name = "Not Collected" });
            chartData.StackBarItemValueSeries.Add(new StackBarChartViewModel { data = data1.ToList(), name = "Collected" });
            chartData.StackBarItemValueSeries.Add(new StackBarChartViewModel { data = data2.ToList(), name = "Not Collected" });
            chartData.StackBarCategories = new List<string>
            {
                "Residential",
                "Commercial"
            };
            return Json(chartData);
        }
        public async Task<IActionResult> GetGarbageCollectionDateByPropertyTypeAndWard()
        {
            var chartData = new ProjectDashboardChartViewModel();

            //chartData.BarItemValueSeries = new List<BarChartViewModel>();
            chartData.StackBarItemValueSeries = new List<StackBarChartViewModel>();
            var data1 = new List<long>();
            chartData.BarItemValueSeries = new List<BarChartViewModel>();

            var warddata = await _smartCityBusiness.GetWardList();
            chartData.StackBarCategories = new List<string>();
            var data2 = new List<long>();
            foreach (var item in warddata)
            {
                var res = await _smartCityBusiness.GetGarbageCollectionDateByPropertyTypeAndWard(item.Id);

                chartData.StackBarCategories.Add(item.Name + " - Residential");
                data1.Add(res.Where(x => x.IsGarbageCollected == true && x.GarbageTypeName == "Residential").Count());
                data1.Add(res.Where(x => x.IsGarbageCollected == false && x.GarbageTypeName == "Residential").Count());



                chartData.StackBarCategories.Add(item.Name + " - Commercial");
                data2.Add(res.Where(x => x.IsGarbageCollected == true && x.GarbageTypeName == "Commercial").Count());
                data2.Add(res.Where(x => x.IsGarbageCollected == false && x.GarbageTypeName == "Commercial").Count());

            }

            //var data1 = new List<long>
            //{
            //    60,
            //    20,
            //    100,
            //    //res.Where(x => x.IsGarbageCollected == true && x.GarbageTypeName == "Residential").Count(),
            //    //res.Where(x => x.IsGarbageCollected == true && x.GarbageTypeName == "Commercial").Count(),
            //};

            //var data2 = new List<long>
            //{
            //    120,
            //    50,
            //    500,
            //    //res.Where(x => x.IsGarbageCollected == false && (x.GarbageTypeName == "Residential" && x.GarbageTypeName == null)).Count(),
            //    //res.Where(x => x.IsGarbageCollected == false && (x.GarbageTypeName == "Commercial" && x.GarbageTypeName == null)).Count()
            //};
            chartData.StackBarItemValueSeries.Add(new StackBarChartViewModel { data = data1.ToList(), name = "Collected" });
            chartData.StackBarItemValueSeries.Add(new StackBarChartViewModel { data = data2.ToList(), name = "Not Collected" });

            //chartData.StackBarCategories = new List<string>
            //{
            //    "W62 - Residential",
            //    "W62 - Commercial",
            //    "W64 - Residential",
            //    "W64 - Commercial",
            //    "W12 - Residential",
            //    "W12 - Commercial",
            //};
            return Json(chartData);
        }
        public async Task<IActionResult> GetGarbageCollectionInNoInPieinKgs()
        {
            var chartData = new ProjectDashboardChartViewModel();
            var res = await _smartCityBusiness.GetGarbageWetAndDryWasteInKgs();
            chartData.XaxisTitle = "Garbage Collection Status";

            chartData.ItemValueLabel = new List<string>
            {
                "Dry",
                "Wet"
            };
            var data = new List<long>
            {
               res.DryWasteInKgs,
               res.WetWasteInKgs
            };
            chartData.ItemValueSeries = data;
            return Json(chartData);
        }
        public async Task<IActionResult> GetGarbageCollectionInNoInBarinKgs()
        {
            List<StackBarChartViewModel> stackData = new();
            var stackbarseries = new List<StackBarChartViewModel>();
            var stackbarcate = new List<string>();
            var s1 = new StackBarChartViewModel() { name = "Dry", data = new List<long>() };
            var s2 = new StackBarChartViewModel() { name = "Wet", data = new List<long>() };

            stackbarseries.Add(s1);
            stackbarseries.Add(s2);
            var warddata = await _smartCityBusiness.GetWardList();

            foreach (var item in warddata)
            {
                var res = await _smartCityBusiness.GetGarbageWetAndDryWasteInKgsByWard(item.Id);
                stackbarcate.Add(item.Name);

                //var collectCnt = res.Where(x => x.IsGarbageCollected == true).Count();
                s1.data.Add(res.DryWasteInKgs);
                //var notCollectedCnt = res.Where(x => x.IsGarbageCollected == false).Count();
                s2.data.Add(res.WetWasteInKgs);
            }

            var chartData = new ProjectDashboardChartViewModel
            {
                StackBarItemValueSeries = stackbarseries,
                StackBarCategories = stackbarcate,
            };
            return Json(chartData);

        }
        public async Task<IActionResult> GetGarbageCollectionDateByPropertyTypeinKgs()
        {
            var chartData = new ProjectDashboardChartViewModel();
            var res = await _smartCityBusiness.GetGarbageWetAndDryWasteInKgsByPropertyType();
            chartData.XaxisTitle = "Garbage Collection Status";

            chartData.ItemValueLabel = new List<string>
            {
                "Commercial",
                "Residential"
            };
            var data = new List<long>
            {
                res.WasteCommercial,
                res.WasteResidential,
            };
            chartData.ItemValueSeries = data;
            return Json(chartData);
        }
        public async Task<IActionResult> GetGarbageCollectionDateByPropertyTypeAndWardinKgs()
        {
            List<StackBarChartViewModel> stackData = new();
            var stackbarseries = new List<StackBarChartViewModel>();
            var stackbarcate = new List<string>();
            var s1 = new StackBarChartViewModel() { name = "Commercial", data = new List<long>() };
            var s2 = new StackBarChartViewModel() { name = "Residential", data = new List<long>() };

            stackbarseries.Add(s1);
            stackbarseries.Add(s2);
            var warddata = await _smartCityBusiness.GetWardList();

            foreach (var item in warddata)
            {
                var res = await _smartCityBusiness.GetGarbageWetAndDryWasteInKgsByPropertyTypeByWard(item.Id);

                stackbarcate.Add(item.Name);

                //var collectCnt = res.Where(x => x.IsGarbageCollected == true).Count();
                s1.data.Add(res.WasteCommercial);
                //var notCollectedCnt = res.Where(x => x.IsGarbageCollected == false).Count();
                s2.data.Add(res.WasteResidential);
            }

            var chartData = new ProjectDashboardChartViewModel
            {
                StackBarItemValueSeries = stackbarseries,
                StackBarCategories = stackbarcate,
            };
            return Json(chartData);

        }
        public IActionResult JSCSearchProperty()
        {
            return View();
        }

        public async Task<IActionResult> GetParcelByDDNNo(string ddnNo)
        {
            var data = await _smartCityBusiness.GetPropertiesByDDNNO(ddnNo);
            var model = data.DistinctBy(x => x.mmi_id).FirstOrDefault();
           
            return Json(model);
        }
        public async Task<IActionResult> JSCPropertyTaxView(string assessmentId)
        {
            var model = await _smartCityBusiness.GetSelfAssessmentData(assessmentId);
            model.FloorDetail = await _smartCityBusiness.GetAssessmentFloorData(assessmentId);
            var taxAmount = await _smartCityBusiness.GetCalculatedPropertyTaxAmount(model, assessmentId);
          
            return View(taxAmount);
        }
        [HttpGet]
        public async Task<IActionResult> GetBuildingCategory(string buildingType)
        {

            var model = await _smartCityBusiness.GetBuildingCategory(buildingType);
            return Ok(model);

        }
        public IActionResult JSCSanitationDashBoardView()
        {
            return View();
        }
        public async Task<IActionResult> GetPropertyRegistrationStatusWise()
        {
            var chartData = new ProjectDashboardChartViewModel();
            var data = await _smartCityBusiness.GetPropertyRegistrationStatusWise();

            return Json(data);
        }



        public async Task<IActionResult> GetParcelForPropertyTaxCal()
        {
            var data = await _smartCityBusiness.GetParcelForPropertyTaxCal();
            var model = data.FirstOrDefault();
            return Json(model);
        }

        public IActionResult JSCPropertTaxCalculation()
        {
            return View();
        }

        public async Task<IActionResult> GetAssessmentDDNNo()
        {
            var data = await _smartCityBusiness.GetViewAssessmentByDDNNO();
            //var model = data.Where(x => x.PropertyType == x.LOV_ID);
            return Json(data);
        }

        public async Task<IActionResult> GetPropertyTaxPaymentReceiptByDDN(string DDNNO)  
        {
            var data = await _smartCityBusiness.GetPropertyTaxPaymentReceiptByDDN(DDNNO);
            return Json(data);
        }
        //public async Task<IActionResult> GetPropertyTaxPaymentReceiptByReceiptId(string ReceiptId)
        //{
        //    var data = await _smartCityBusiness.GetPropertyTaxPaymentReceiptByReceiptId(ReceiptId);
        //    return Json(data);
        //}

        public async Task<IActionResult> GetJSCVehicleDetails(string vehicleId, DateTime? startDate, DateTime? endDate)
        {
            var data = await _smartCityBusiness.GetJSCVehicleDetails(vehicleId, startDate, endDate);
            return Json(data);
        }

        public async Task<IActionResult> GetTransferStationDetails(string transferStationId)
        {
            var data = await _smartCityBusiness.GetTransferStationDetails(transferStationId);
            return Json(data);
        }

        public async Task<IActionResult> GetOutwardVehicleList()
        {
            var data = await _smartCityBusiness.GetOutwardVehicleList(DateTime.Now);
            return Json(data);
        }

        public async Task<IActionResult> GetVehicleIdForLoggedInUser()
        {
            var data = await _smartCityBusiness.GetVehicleIdForLoggedInUser(_userContext.UserId);
            return Json(data);
        }

        public async Task<IActionResult> GetJSCOutwardReport(DateTime? date)
        {
            var data = await _smartCityBusiness.GetJSCOutwardReport(date);
            return Json(data);
        }

        public async Task<IActionResult> GetJSCInwardReport(DateTime? date)
        {
            var data = await _smartCityBusiness.GetJSCInwardReport(date);
            return Json(data);
        }

        public async Task<IActionResult> AddPropertyByUser(string ddnNo)
        {
            var temp = await _templateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_USER_PROPERTY_MAP");

            var formTempModel = new FormTemplateViewModel();
            formTempModel.DataAction = DataActionEnum.Create;
            formTempModel.TemplateCode = "JSC_USER_PROPERTY_MAP";
            var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("userId", _userContext.UserId);
            ((IDictionary<String, Object>)exo).Add("DdnNo", ddnNo);
            formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            formmodel.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };
            var res = await _cmsBusiness.ManageForm(formmodel);
            if (res.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }
        public async Task<IActionResult> GetViewPropertyByUserMap()
        {
            var data = await _smartCityBusiness.GetViewPrpertyMapByDdnNoAndUser();
            return Json(data);
        }

        public async Task<IActionResult> GetViewPrpertyForSelfAssessment()
        {
            var data = await _smartCityBusiness.GetViewPrpertyForSelfAssessment();
            return Json(data);
        }

        public async Task<IActionResult> GetAddPropertyIsAlreadyExist(string ddnNo)
        {
            var data = await _smartCityBusiness.GetAddPropertyExist(ddnNo);
            if (data.Count() > 0)
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
            //return Json(data);
        }


        public async Task<IActionResult> GetBWGCollection()
        {
            var chartData = new ProjectDashboardChartViewModel();
            var res = await _smartCityBusiness.GetBWGCollection();
            chartData.XaxisTitle = "BWG Collection Status";

            chartData.ItemValueLabel = new List<string>
            {
                "Dry",
                "Wet"
            };
            var data = new List<long>
            {
                res.WetWasteInKgs,
                res.DryWasteInKgs,
            };
            chartData.ItemValueSeries = data;
            return Json(chartData);

        }

        public async Task<IActionResult> GetGarbageCollection10DaysCount(DateTime date)
        {
            var collectedCountList = new List<int>();
            var notCollectedCountList = new List<int>();
            var dryCountList = new List<int>();
            var wetCountList = new List<int>();
            var dryCountiList = new List<int>();
            var wetCountiList = new List<int>();
            var commercialCountList = new List<int>();
            var residentialCountList = new List<int>();

            var collectedCount = 10;
            var notCollectedCount = 10;
            var dryCount = 10;
            var wetCount = 10;
            var commercialCount = 10;
            var residentialCount = 10;
            var dryCounti = 10;
            var wetCounti = 10;


            var TodaysDate = await _smartCityBusiness.GetGarbageCollectionDateByDate(date.Date);
            collectedCount = TodaysDate.Where(x => x.Name == "Garbage Collected").Select(x => x.Count).FirstOrDefault().ToSafeInt();
            notCollectedCount = TodaysDate.Where(x => x.Name == "Garbage Not Collected").Select(x => x.Count).FirstOrDefault().ToSafeInt();
            dryCount = TodaysDate.Where(x => x.Name == "Outward Dry").Select(x => x.Count).FirstOrDefault().ToSafeInt();
            wetCount = TodaysDate.Where(x => x.Name == "Outward Wet").Select(x => x.Count).FirstOrDefault().ToSafeInt();
            dryCounti = TodaysDate.Where(x => x.Name == "Inward Dry").Select(x => x.Count).FirstOrDefault().ToSafeInt();
            wetCounti = TodaysDate.Where(x => x.Name == "Inward Wet").Select(x => x.Count).FirstOrDefault().ToSafeInt();
            commercialCount = TodaysDate.Where(x => x.Name == "Commercial").Select(x => x.Count).FirstOrDefault().ToSafeInt();
            residentialCount = TodaysDate.Where(x => x.Name == "Residential").Select(x => x.Count).FirstOrDefault().ToSafeInt();

            DateTime[] last10Days = Enumerable.Range(0, 10)
                                        .Select(i => date.Date.AddDays(-i))
                                        .ToArray();
            foreach (var d in last10Days)
            {
                var data = await _smartCityBusiness.GetGarbageCollectionDateByDate(d);
                if (data.IsNotNull())
                {
                    var cnt = data.Where(x => x.Name == "Garbage Collected").Select(x => x.Count).FirstOrDefault();
                    collectedCountList.Add(cnt.ToSafeInt());
                    cnt = data.Where(x => x.Name == "Garbage Not Collected").Select(x => x.Count).FirstOrDefault();
                    notCollectedCountList.Add(cnt.ToSafeInt());
                    cnt = data.Where(x => x.Name == "Residential").Select(x => x.Count).FirstOrDefault();
                    residentialCountList.Add(cnt.ToSafeInt());
                    cnt = data.Where(x => x.Name == "Commercial").Select(x => x.Count).FirstOrDefault();
                    commercialCountList.Add(cnt.ToSafeInt());
                    cnt = data.Where(x => x.Name == "Outward Dry").Select(x => x.Count).FirstOrDefault();
                    dryCountList.Add(cnt.ToSafeInt());
                    cnt = data.Where(x => x.Name == "Outward Wet").Select(x => x.Count).FirstOrDefault();
                    wetCountList.Add(cnt.ToSafeInt());
                    cnt = data.Where(x => x.Name == "Inward Dry").Select(x => x.Count).FirstOrDefault();
                    dryCountiList.Add(cnt.ToSafeInt());
                    cnt = data.Where(x => x.Name == "Inward Wet").Select(x => x.Count).FirstOrDefault();
                    wetCountiList.Add(cnt.ToSafeInt());
                }
            }
            return Json(new
            {
                collectedCountList = collectedCountList,
                notCollectedCountList = notCollectedCountList,
                dryCountList = dryCountList,
                wetCountList = wetCountList,
                dryCountiList = dryCountiList,
                wetCountiList = wetCountiList,
                residentialCountList = residentialCountList,
                commercialCountList = commercialCountList,
                collectedCount = collectedCount,
                notCollectedCount = notCollectedCount,
                dryCount = dryCount,
                wetCount = wetCount,
                dryCounti = dryCounti,
                wetCounti = wetCounti,
                commercialCount = commercialCount,
                residentialCount = residentialCount
            });
        }
        public async Task<IActionResult> GetGarbageCollectionMonthCount()
        {
            var wet = new List<int>();
            var dry = new List<int>();
            var dates = new List<string>();

            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);


            DateTime[] monthDates = Enumerable.Range(0, 31)
                                        .Select(i => startDate.AddDays(i))
                                        .ToArray();

            foreach (var date in monthDates)
            {
                dates.Add(date.ToString("dd-MM"));
                var data = await _smartCityBusiness.GetGarbageCollectionDateByDate(date);
                if (data.IsNotNull())
                {
                    var cnt = data.Where(x => x.Name == "Outward Wet").Select(x => x.Count).FirstOrDefault().ToSafeInt() +
                        data.Where(x => x.Name == "Inward Wet").Select(x => x.Count).FirstOrDefault().ToSafeInt();
                    wet.Add(cnt);
                    cnt = data.Where(x => x.Name == "Outward Dry").Select(x => x.Count).FirstOrDefault().ToSafeInt() +
                        data.Where(x => x.Name == "Inward Dry").Select(x => x.Count).FirstOrDefault().ToSafeInt();
                    dry.Add(cnt);

                }
            }
            return Json(new
            {
                dates = dates,
                wet = wet,
                dry = dry
            });
        }

        public async Task<IActionResult> GetGarbageCollectionAreaCount(DateTime date)
        {
            var wardNameList = new List<string>();
            var residential = new List<string>();
            var commercial = new List<string>();
            var bwg = new List<string>();

            var wardList = await _smartCityBusiness.GetWardList();
            wardNameList = wardList.Select(x => (x.Name)).ToList();
            foreach (var item in wardList.Select(x => x.Id).ToList())
            {
                var data = await _smartCityBusiness.GetGarbageCollectionDataByWardAndDate(date, item);
                if (data.IsNotNull())
                {
                    residential.Add(data.Where(x => x.Name == "Residential").Select(x => x.Count).FirstOrDefault());
                    commercial.Add(data.Where(x => x.Name == "Commercial").Select(x => x.Count).FirstOrDefault());
                    bwg.Add(data.Where(x => x.Name == "BWG").Select(x => x.Count).FirstOrDefault());
                }

            }
            return Json(new
            {
                wardList = wardNameList,
                residential = residential,
                commercial = commercial,
                bwg = bwg
            });
        }

       public IActionResult JSCUnauthorizedViolationsReport()
        {
              return View();
        }
        public async Task<IActionResult> GetUnauthorizedCaseList()
        {
            var data = await _smartCityBusiness.GetUnauthorizedCaseList();
            return Json(data);

        }
        public async Task<IActionResult> GetJSCUnauthorizedViolationsDetail(DateTime? date,string Ward, string userId)
        {
            var data = await _smartCityBusiness.GetJSCUnauthorizedViolationsDetail(date,Ward,userId);
            return Json(data);
        }

        public async Task<IActionResult> ViewEnforcementUnauthorizationCaseDetails(string id)
        {
            var model = new JSCEnforcementUnAuthorizationViewModel();
            var list = await _smartCityBusiness.GetJSCUnauthorizedViolationsDetail(null,null,null);
            if (list.Count > 0 && id.IsNotNullAndNotEmpty())
            {
                model = list.FirstOrDefault(x => x.Id == id);
                var violIds = model.TypesOfViolation.Split(",");
                var violationList = await _smartCityBusiness.GetEnforcementViolations();
                model.TypesOfViolationList = violationList.Where(x=>violIds.Any(y=>y==x.Id)).ToList();
            }
            return View(model);
        }

        public IActionResult JSCAuthorizedViolationsReport()
        {
            return View();
        }

        public async Task<IActionResult> GetAuthorizedCaseList()
        {
            var data = await _smartCityBusiness.GetAuthorizedCaseList();
            return Json(data);

        }
        public async Task<IActionResult> GetJSCAuthorizedViolationsDetail(DateTime? date, string Ward, string UserId)
        {
            var data = await _smartCityBusiness.GetJSCAuthorizedViolationsDetail(date, Ward,UserId);
            return Json(data);
        }

        public async Task<IActionResult> ViewEnforcementAuthorizationCaseDetails(string id)
        {
            var model = new JSCEnforcementUnAuthorizationViewModel();
            var list = await _smartCityBusiness.GetJSCAuthorizedViolationsDetail(null, null,null);
            if (list.Count > 0 && id.IsNotNullAndNotEmpty())
            {
                model = list.FirstOrDefault(x => x.Id == id);
                var violIds = model.TypesOfViolation.Split(",");
                var violationList = await _smartCityBusiness.GetEnforcementViolations();
                model.TypesOfViolationList = violationList.Where(x => violIds.Any(y => y == x.Id)).ToList();
            }
            return View(model);
        }

        public IActionResult JSCPropertyPayment(string assessmentId,string ddnno,double amount ,double PaidAmount)
        {
            var model = new JSCPropertySelfAssessmentViewModel();
            model.Id = assessmentId;
            model.DdnNo = ddnno;
            model.TotalAmount = amount;
            //model.Year = Year;
            model.TotalAmount = PaidAmount;
            return View(model);
        }
        
        public async Task<IActionResult> GetPropertyPaymentDetails()
        {
            var data = await _smartCityBusiness.GetPropertyPaymentDetails();
            return Json(data);
        }
        public IActionResult JSCOBPSAuthorizationReport()
        {
            return View();
        }
        public async Task<IActionResult> PropertyPaymentDetails(string ReceiptId)
        {
           // ReceiptId = "abcd1234567890";
            var model = new PropertyTaxPaymentReceiptViewModel();

            var data = await _smartCityBusiness.GetPropertyTaxPaymentReceiptByReceiptId(ReceiptId);
            if (data.IsNotNull())
            {
                model = data;
            }
            return View(model);
        }

        public async Task<IActionResult> GetJSCOBPSAuthorizedDetail(DateTime? date, string Ward)
        {
            var data = await _smartCityBusiness.GetJSCOBPSAuthorizedDetail(date, Ward);
            return Json(data);
        }
        public IActionResult JSCEnforcementSubloginMapping()
        {
            return View();
        }
        public async Task<IActionResult> ViewJSCOBPSAuthorizationReport(string id)
        {
            var model = new JSCEnforcementUnAuthorizationViewModel();
            var list = await _smartCityBusiness.GetJSCOBPSAuthorizedDetail(null, null);
            if (list.Count > 0 && id.IsNotNullAndNotEmpty())
            {
                model = list.FirstOrDefault(x => x.Id == id);
                return View(model);
            }
            return View(model);
        }


        public async Task<IActionResult> configerform(double Amount, string DdnNo , string Asssessmentid ,double PaidAmount)
        {
            var model = new JSCPropertyTaxInstallmentViewModel();
            model.Amount = Amount;
            model.ddnNo = DdnNo;
            model.AssessmentId = Asssessmentid;
            model.PaidAmount = PaidAmount;
            //model.Year = Year;
            var formTempModel = new FormTemplateViewModel();
            formTempModel.DataAction = DataActionEnum.Create;
            formTempModel.TemplateCode = "PROPERTY_TAX_PAYMENT_AMOUNT";
            var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
            formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var res = await _cmsBusiness.ManageForm(formmodel);

            

            var model1 = new JSCPropertyTaxInstallmentViewModel();
            model1.Amount = Amount;
            model1.ddnNo = DdnNo;
            model1.TaxAmountId = formmodel.RecordId;
            model1.PaymentDate = new DateTime();
            model1.Year = DateTime.Now.Year.ToString();
            model1.PaymentFrom = Convert.ToDateTime("01-04-2022 00:00:00");
            model1.PaymentTo = Convert.ToDateTime("30-03-2023 00:00:00");
            var formTempModel1 = new FormTemplateViewModel();
            formTempModel1.DataAction = DataActionEnum.Create;
            formTempModel1.TemplateCode = "PROPERTY_INSTALLMENT";
            var formmodel1 = await _cmsBusiness.GetFormDetails(formTempModel1);
            formmodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model1);
            var res1 = await _cmsBusiness.ManageForm(formmodel);


            return Json(new { success = true });

           

        }           

        public async Task<IActionResult> GetEnforcementSubloginMappinglist()
        {
            var data = await _smartCityBusiness.GetEnforcementSubloginMappinglist();
            return Json(data);

        }
    }

}
