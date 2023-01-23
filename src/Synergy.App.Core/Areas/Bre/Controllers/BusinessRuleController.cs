﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Syncfusion.EJ2.Diagrams;
//using Syncfusion.EJ2.Navigations;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Controllers
{
    [Area("Bre")]
    public class BusinessRuleController : ApplicationController
    {
        private readonly IBusinessRuleBusiness _breRuleBusiness;
        private readonly IBusinessAreaBusiness _breAreaBusiness;
        private readonly IBusinessSectionBusiness _breSectionBusiness;
        private readonly IBusinessRuleGroupBusiness _breRuleGroupBusiness;
        private readonly IBusinessRuleModelBusiness _ruleBusiness;
        private readonly IComponentBusiness _componentBusiness;
        private readonly IDecisionScriptComponentBusiness _decisionScriptBusiness;
        private readonly IBusinessRuleNodeBusiness _breRuleNodeBusiness;
        private readonly IWebHelper _webApi;
        private readonly ILOVBusiness _lOVBusiness;
        public BusinessRuleController(IBusinessRuleBusiness breRuleBusiness, IBusinessAreaBusiness breAreaBusiness, IBusinessRuleGroupBusiness breRuleGroupBusiness,
            IBusinessSectionBusiness breSectionBusiness, IBusinessRuleModelBusiness ruleBusiness,
            IComponentBusiness componentBusiness, IDecisionScriptComponentBusiness decisionScriptBusiness,
            IBusinessRuleNodeBusiness breRuleNodeBusiness, ILOVBusiness lOVBusiness
            , IWebHelper webApi)
        {
            _breRuleBusiness = breRuleBusiness;
            _breAreaBusiness = breAreaBusiness;
            _breRuleGroupBusiness = breRuleGroupBusiness;
            _breSectionBusiness = breSectionBusiness;
            _ruleBusiness = ruleBusiness;
            _componentBusiness = componentBusiness;
            _decisionScriptBusiness = decisionScriptBusiness;
            _breRuleNodeBusiness = breRuleNodeBusiness;
            _webApi = webApi;
            _lOVBusiness = lOVBusiness;
        }
        /// <summary>
        /// calls this to fetch the business tree
        /// </summary>
        /// <returns>list of Business Rule</returns>
        //        public async Task<IActionResult> Index()
        //        {

        //            List<object> menuItems = new List<object>();
        //            menuItems.Add(new
        //            {
        //                text = "Add Business Area",
        //                id = "0",
        //                iconCss = "fa fas fa-plus"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Edit Business Area",
        //                id = "1",
        //                iconCss = "fa fas fa-pencil-alt"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Add Business Section",
        //                id = "2",
        //                iconCss = "fa fas fa-plus"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Edit Business Section",
        //                id = "3",
        //                iconCss = "fa fas fa-pencil-alt"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Add Business Rule Group",
        //                id = "4",
        //                iconCss = "fa fas fa-plus"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Edit Business Rule Group",
        //                id = "5",
        //                iconCss = "fa fas fa-pencil-alt"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Add Business Rule",
        //                id = "6",
        //                iconCss = "fa fas fa-plus"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Edit Business Rule",
        //                id = "7",
        //                iconCss = "fa fas fa-pencil-alt"
        //            });
        //            menuItems.Add(new
        //            {
        //                text = "Open Business Rule",
        //                id = "8",
        //                iconCss = "fa fas fa-box-open"
        //            });
        //            ViewBag.menuItems = menuItems;
        //            double[] intervals = { 1, 9, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75 };
        //            DiagramGridlines grIdLines = new DiagramGridlines()
        //            { LineColor = "#e0e0e0", LineIntervals = intervals };
        //            ViewBag.gridLines = grIdLines;
        //            DiagramMargin margin = new DiagramMargin() { Left = 15, Right = 15, Bottom = 15, Top = 15 };
        //            ViewBag.margin = margin;
        //            ViewBag.getNodeDefaults = "getNodeDefaults";
        //            ViewBag.getConnectorDefaults = "getConnectorDefaults";
        //            List<ContextMenuItem> contextMenuItemModels = new List<ContextMenuItem>()
        //{
        //        new ContextMenuItem()
        //        {
        //            Text ="Create Node",
        //            Id="Node",
        //            Items = new List<ContextMenuItem>()
        //{
        //                new ContextMenuItem(){  Text ="Decision", Id="Decision1", },
        //                new ContextMenuItem(){  Text ="Process", Id="Process1", },
        //                new ContextMenuItem(){  Text ="End", Id="Terminator1", }
        //            }
        //        },
        //         new ContextMenuItem()
        //        {
        //            Text ="Remove Node",
        //            Id="RemoveNode",

        //        },
        //        new ContextMenuItem()
        //        {
        //            Text ="If Yes, Create",
        //            Id="IfNode",

        //            Items = new List<ContextMenuItem>()
        //{
        //                new ContextMenuItem(){  Text ="Decision", Id="Decision2", },
        //                new ContextMenuItem(){  Text ="Process", Id="Process2", },
        //                new ContextMenuItem(){  Text ="End", Id="Terminator2", }
        //            }
        //        },
        //        new ContextMenuItem()
        //        {
        //            Text ="If No, Create",
        //            Id="ElseNode",

        //            Items = new List<ContextMenuItem>()
        //{
        //                new ContextMenuItem(){  Text ="Decision", Id="Decision3", },
        //                new ContextMenuItem(){  Text ="Process", Id="Process3", },
        //                new ContextMenuItem(){  Text ="End", Id="Terminator3", }
        //            }
        //        },
        //        new ContextMenuItem()
        //        {
        //            Text ="View Detail",
        //            Id="View",
        //        },

        //    };
        //            ViewBag.contextMenuItems = contextMenuItemModels;
        //            return View();
        //        }

        [Authorize]
        public async Task<IActionResult> Index(string breId, string templateId)
        {
            ViewBag.BusinessRuleId = breId;
            ViewBag.TemplateId = templateId;
            return View();
        }

        public async Task<JsonResult> GetBusinessRuleTreeList(string id)
        {
            var model = await _breRuleBusiness.GetBusinessRuleTreeList(id);
            var result = model.Select(x => new
            {
                id = x.Id,
                Name = x.Name,
                hasChildren = x.HasSubFolders,
                ItemType = x.BusinessRuleTreeNodeType.ToString(),
                IconCss = x.IconCss,
                expanded = x.Expanded

            }).ToList();
            return Json(result.ToList());
        }

        public async Task<IActionResult> CreateBusinessArea(string parentId, string id)
        {
            var model = new BusinessAreaViewModel();
            if (id != null && id != "")
            {
                model = await _breAreaBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            model.ParentId = parentId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBusinessArea(BusinessAreaViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _breAreaBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Business area created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _breAreaBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Business area edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> CreateBusinessSection(string parentId, string id)
        {
            var model = new BusinessSectionViewModel();
            if (id != null && id != "")
            {
                model = await _breSectionBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            model.ParentId = parentId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessSection(BusinessSectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _breSectionBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Business section created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _breSectionBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Business section edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> CreateBusinessRuleGroup(string parentId, string id)
        {
            var model = new BusinessRuleGroupViewModel();
            if (id != null && id != "")
            {
                model = await _breRuleGroupBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            model.ParentId = parentId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessRuleGroup(BusinessRuleGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _breRuleGroupBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Business rule group created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _breRuleGroupBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Business rule group edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> CreateBusinessRule(string templateId, BusinessLogicExecutionTypeEnum type, TemplateTypeEnum templateType, string businessRuleId, LayoutModeEnum? lo = null)
        {
            var bre = await _breRuleBusiness.GetSingle(x => x.Id == businessRuleId);
            var model = new BusinessRuleViewModel();
            if (bre != null)
            {
                model = bre;
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.TemplateId = templateId;
                model.BusinessLogicExecutionType = type;
            }
            if (templateType == TemplateTypeEnum.Service)
            {
                model.LOVType = "LOV_SERVICE_STATUS";
            }
            else if (templateType == TemplateTypeEnum.Task)
            {
                model.LOVType = "LOV_TASK_STATUS";
            }
            else if (templateType == TemplateTypeEnum.Note)
            {
                model.LOVType = "LOV_NOTE_STATUS";
            }
            else if (templateType == TemplateTypeEnum.Form)
            {
                model.LOVType = "LOV_FORM_STATUS";
            }
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "/Areas/Core/Views/Shared/_PopupLayout.cshtml";
                ViewBag.LayoutMode = LayoutModeEnum.Popup.ToString();
            }
            if(type == BusinessLogicExecutionTypeEnum.PreLoad)
            {
                var action = await _lOVBusiness.GetSingle(x => x.Code == "SERVICE_STATUS_ALL");
                model.ActionId = action.Id;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessRule(BusinessRuleViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _breRuleBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        // return PopupRedirect("Business rule created successfully");
                        return Json(new { success = true, BreId = result.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _breRuleBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        //return PopupRedirect("Business rule edited successfully");
                        return Json(new { success = true, BreId = result.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            // return View(model);
        }

        public async Task<ActionResult> RuleBuilder(string id, string nodeId, string ruleId, string templateId, string decisionParentId, bool? isWorkFlow, bool? isBusinessDiagram)
        {
            var model = new BusinessRuleViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                if (isWorkFlow == true)
                {
                    var comp = await _componentBusiness.GetSingleById(id);
                    if (comp != null)
                    {
                        var decisionComponent = await _decisionScriptBusiness.GetSingle(x => x.ComponentId == comp.Id);
                        model = new BusinessRuleViewModel
                        {
                            Id = decisionComponent.Id,
                            Name = comp.Name,
                            TemplateId = templateId,
                            DecisionScriptComponentId = decisionComponent.Id,
                            IsWorkFlow = isWorkFlow,
                            DecisionParentId = decisionParentId,
                            DataAction = DataActionEnum.Edit,
                        };

                        ViewBag.Script = decisionComponent.Script;
                        ViewBag.BusinessRuleLogicType = decisionComponent.BusinessRuleLogicType;
                    }
                    ViewBag.ruleId = ruleId;
                }
                else if (isBusinessDiagram == true)
                {
                    var node = await _breRuleNodeBusiness.GetSingleById(id);
                    if (node != null)
                    {

                        model = new BusinessRuleViewModel
                        {
                            Id = node.Id,
                            Name = node.Name,
                            TemplateId = templateId,
                            // DecisionScriptComponentId = decisionComponent.Id,
                            IsBusinessDiagram = isBusinessDiagram,
                            DecisionParentId = decisionParentId,
                            DataAction = DataActionEnum.Edit,
                        };
                    }

                    ViewBag.ruleId = ruleId;
                    var result = await _breRuleNodeBusiness.GetSingleById(id);//.Create(model);
                    if (result.IsNotNull())
                    {
                        ViewBag.Script = result.Script;
                        ViewBag.BusinessRuleLogicType = result.BusinessRuleLogicType;
                    }
                }


                return View(model);
            }
            else
            {
                if (isWorkFlow == true)
                {
                    model = new BusinessRuleViewModel
                    {
                        Id = nodeId,
                        TemplateId = templateId,
                        Name = "Decision",
                        DecisionScriptComponentId = Guid.NewGuid().ToString(),
                        IsWorkFlow = isWorkFlow,
                        DecisionParentId = decisionParentId,
                        DataAction = DataActionEnum.Create
                    };
                    ViewBag.ruleId = ruleId;
                    return View(model);
                }
                else if (isBusinessDiagram == true)
                {
                    model = new BusinessRuleViewModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Decision",
                        TemplateId = templateId,
                        //DecisionScriptComponentId = Guid.NewGuid().ToString(),
                        IsBusinessDiagram = isBusinessDiagram,
                        DecisionParentId = decisionParentId,
                        DataAction = DataActionEnum.Create
                    };
                    ViewBag.ruleId = ruleId;
                    return View(model);
                }
                else
                {
                    model = new BusinessRuleViewModel
                    {
                        Id = nodeId,
                        TemplateId = templateId,
                        //DecisionScriptComponentId = Guid.NewGuid().ToString(),
                        //IsBusinessDiagram = isBusinessDiagram,
                        //DecisionParentId = decisionParentId,
                        DataAction = DataActionEnum.Create
                    };
                    ViewBag.ruleId = ruleId;
                    return View(model);
                }

            }
        }

        public ActionResult BusinessRuleInputData(string ruleId, string templateId)
        {
            ViewBag.ruleId = ruleId;
            ViewBag.templateId = templateId;
            return View();
        }
        public ActionResult BusinessRuleMasterData(string ruleId, string templateId)
        {
            ViewBag.ruleId = ruleId;
            ViewBag.templateId = templateId;
            return View();
        }
        public async Task<ActionResult> AddCondition(string nodeId, string parentId, DataActionEnum dataAction, LogicalEnum? condition, string ruleId, string id, string templateId, string decisionScriptComponentId)
        {
            if (!id.IsNullOrEmpty())
            {
                var model = await _ruleBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
                model.RuleId = ruleId;
                model.TemplateId = templateId;
                return View(model);
            }
            else
            {
                var model = new BusinessRuleModelViewModel
                {
                    BusinessRuleNodeId = nodeId,
                    ParentId = parentId,
                    DataAction = dataAction,
                    Condition = condition,
                    RuleId = ruleId,
                    TemplateId = templateId,
                    BusinessRuleType = BusinessRuleTypeEnum.Operational,
                    BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule,
                    DecisionScriptComponentId = decisionScriptComponentId
                };
                ViewBag.ruleId = ruleId;
                return View(model);
            }
        }
        public async Task<ActionResult> AddConditionForStepTaskAssigneeLogic(string nodeId, string parentId, DataActionEnum dataAction, LogicalEnum? condition, string ruleId, string id, string templateId, string decisionScriptComponentId, string referenceId)
        {
            if (!id.IsNullOrEmpty())
            {
                var model = await _ruleBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
                model.RuleId = ruleId;
                model.TemplateId = templateId;
                model.FieldSourceType = BreMetadataTypeEnum.InputData;
                model.ValueSourceType = BreMetadataTypeEnum.Constant;
                if (model.DataJson.IsNullOrEmpty())
                {
                    model.DataJson = "{}";
                }
                return View(model);
            }
            else
            {
                var model = new BusinessRuleModelViewModel
                {
                    BusinessRuleNodeId = nodeId,
                    ParentId = parentId,
                    DataAction = dataAction,
                    Condition = condition,
                    RuleId = ruleId,
                    TemplateId = templateId,
                    BusinessRuleType = BusinessRuleTypeEnum.Operational,
                    BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule,
                    DecisionScriptComponentId = decisionScriptComponentId,
                    ReferenceId = referenceId,
                    DataJson = "{}"
                };
                ViewBag.ruleId = ruleId;
                await SetUIJson(model);
                return View(model);
            }
        }

        private async Task SetUIJson(BusinessRuleModelViewModel template)
        {
            try
            {
                var model = await _ruleBusiness.GetSingle<TemplateViewModel, Template>(x => x.Id == template.TemplateId);
                //return Json(new { success = true, json = model.Json });
                var result = JObject.Parse(model.Json);
                var components = (JArray)result.SelectToken("components");
                var controls = new List<JObject>();
                var data = JObject.Parse(template.DataJson);
                GetColumnComponents(components, data, controls);
                var rows = JArray.Parse(@$"[]");
                if (controls.Any())
                {
                    foreach (var item in controls)
                    {

                        var label = Convert.ToString(item.SelectToken("label"));
                        var key = Convert.ToString(item.SelectToken("key"));
                        var id = Convert.ToString(item.SelectToken("columnMetadataId"));

                        var props = item.Properties();
                        if (!props.Any(x => x.Name == "hideLabel"))
                        {
                            item.Add("hideLabel", true);
                        }
                        if (props.Any(x => x.Name == "disabled"))
                        {
                            item.Remove("disabled");
                        }
                        if (props.Any(x => x.Name == "validate"))
                        {
                            item.Remove("validate");
                        }
                        if (!props.Any(x => x.Name == "customClass"))
                        {
                            item.Add("customClass", "formio-display");
                        }
                        if (!props.Any(x => x.Name == "id"))
                        {
                            item.Add("id", id);
                        }
                        var value = data[key];
                        if (value != null)
                        {
                            if (props.Any(x => x.Name == "defaultValue"))
                            {
                                item.Remove("defaultValue");
                            }
                            item.Add("defaultValue", value);
                        }
                        var labelObj = JObject.Parse(@$"  {{
        ""components"": [
          {{
                    ""label"": ""HTML"",
            ""content"": ""{label}"",
            ""key"": ""html"",
            ""type"": ""htmlelement"",
            ""id"":""label_{id}""
          }}
        ]
      }}");
                        var controlObj = JObject.Parse(@$"{{""components"": [] }}");
                        var ctrlComp = (JArray)controlObj.SelectToken("components");
                        ctrlComp.Add(item);
                        var row = JArray.Parse(@$"[]");
                        row.Add(labelObj);
                        row.Add(controlObj);
                        rows.Add(row);
                    }
                    var ret = JArray.Parse(@$"[]");
                    var obj = JObject.Parse(@$"{{
  ""label"": ""Preview"",
  ""cellAlignment"": ""left"",
  ""striped"": true,
  ""bordered"": true,
  ""condensed"": true,
  ""key"": ""table"",
  ""type"": ""table"",
  ""numRows"": {controls.Count},
  ""numCols"": 2,
  ""input"": false,
  ""tableView"": false, 
  ""id"":""sdsdsdsdsd""
}}");

                    obj.Add("rows", rows);
                    ret.Add(obj);
                    result.Remove("components");
                    result.Add("components", JArray.FromObject(ret));
                    result.Remove("display");
                    result.Add("display", "form");
                    template.UIJson = _webApi.AddHost(result.ToString());

                }
                else
                {
                    template.UIJson = "{}";
                }


            }
            catch (Exception e)
            {

                throw;
            }
        }
        private void GetColumnComponents(JArray comps, JObject data, List<JObject> controls)
        {
            try
            {
                foreach (JObject jcomp in comps)
                {
                    var typeObj = jcomp.SelectToken("type");
                    var keyObj = jcomp.SelectToken("key");
                    if (typeObj.IsNotNull())
                    {
                        var type = typeObj.ToString();
                        if (type == "textfield" || type == "select" || type == "checkbox")
                        {
                            var show = false;
                            var showText = jcomp.SelectToken("showInBusinessLogic");
                            if (showText.IsNotNull())
                            {
                                show = (showText.ToString().ToLower() == "true");
                            }
                            var key = keyObj.ToString();
                            if (show)
                            {

                                try
                                {
                                    var reserve = jcomp.SelectToken("reservedKey");
                                    if (reserve == null)
                                    {
                                        var columnId = jcomp.SelectToken("columnMetadataId");
                                        if (columnId != null)
                                        {
                                            controls.Add(jcomp);
                                            //var newProperty = new JProperty("defaultValue", model.Udfs.GetValue(columnMeta.Name));
                                            //jcomp.Add(newProperty);
                                        }

                                    }
                                }
                                catch (Exception we)
                                {

                                    throw;
                                }

                            }
                        }
                        else if (type == "columns")
                        {
                            try
                            {
                                JArray cols = (JArray)jcomp.SelectToken("columns");
                                foreach (var col in cols)
                                {
                                    JArray rows = (JArray)col.SelectToken("components");
                                    if (rows != null)
                                        GetColumnComponents(rows, data, controls);
                                }
                            }
                            catch (Exception e)
                            {


                            }

                        }
                        else if (type == "panel")
                        {
                            try
                            {
                                var pComps = (JArray)jcomp.SelectToken("components");
                                if (pComps != null)
                                {
                                    GetColumnComponents(pComps, data, controls);
                                }
                            }
                            catch (Exception e)
                            {

                            }


                        }
                        else if (type == "table")
                        {
                            try
                            {
                                var rows = (JArray)jcomp.SelectToken("rows");
                                foreach (var row in rows)
                                {
                                    if (row != null)
                                    {
                                        foreach (JToken cell in row.Children())
                                        {
                                            var comp = (JArray)cell.SelectToken("components");
                                            GetColumnComponents(rows, data, controls);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                                throw;
                            }

                        }
                        else
                        {
                            JArray rows = (JArray)jcomp.SelectToken("components");
                            if (rows != null)
                                GetColumnComponents(rows, data, controls);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        private async Task ChildComp(JArray comps, List<ColumnMetadataViewModel> Columns, ServiceTemplateViewModel model)
        {
            List<UdfPermissionViewModel> udfPermissons = null;
            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                var keyObj = jcomp.SelectToken("key");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    var key = keyObj.ToString();
                    if (type == "textfield" || type == "textarea" || type == "number" || type == "password"
                        || type == "selectboxes" || type == "checkbox" || type == "select" || type == "radio"
                        || type == "email" || type == "url" || type == "phoneNumber" || type == "tags"
                        || type == "datetime" || type == "day" || type == "time" || type == "currency" || type == "button"
                        || type == "signature" || type == "file" || type == "hidden" || type == "datagrid" || type == "editgrid"
                        || (type == "htmlelement" && key == "chartgrid") || (type == "htmlelement" && key == "chartJs"))
                    {


                        var reserve = jcomp.SelectToken("reservedKey");
                        if (reserve == null)
                        {
                            var tempmodel = JsonConvert.DeserializeObject<FormFieldViewModel>(jcomp.ToString());


                            var columnId = jcomp.SelectToken("columnMetadataId");


                            if (columnId != null)
                            {
                                var columnMeta = Columns.FirstOrDefault(x => x.Name == tempmodel.key);
                                if (columnMeta != null)
                                {
                                    columnMeta.ActiveUserType = model.ActiveUserType;
                                    columnMeta.NtsStatusCode = model.ServiceStatusCode;
                                    //Set default value
                                    if (model.Udfs != null && model.Udfs.GetValue(columnMeta.Name) != null)
                                    {
                                        var newProperty = new JProperty("defaultValue", model.Udfs.GetValue(columnMeta.Name));
                                        jcomp.Add(newProperty);
                                    }

                                    var udfValue = new JProperty("udfValue", columnMeta.Value);
                                    jcomp.Add(udfValue);

                                    var isReadonly = false;
                                    if (model.ReadoOnlyUdfs != null && model.ReadoOnlyUdfs.ContainsKey(columnMeta.Name))
                                    {
                                        isReadonly = model.ReadoOnlyUdfs.GetValueOrDefault(columnMeta.Name);
                                    }
                                    //if (model.HiddenUdfs != null && model.HiddenUdfs.ContainsKey(columnMeta.Name))
                                    //{
                                    //    isReadonly = model.ReadoOnlyUdfs.GetValueOrDefault(columnMeta.Name);
                                    //}
                                    if ((udfPermissons == null) || (model.HiddenUdfs != null && model.HiddenUdfs.ContainsKey(columnMeta.Name)))
                                    {
                                        var hiddenProperty = jcomp.SelectToken("hidden");
                                        if (hiddenProperty == null)
                                        {
                                            var newProperty = new JProperty("hidden", true);
                                            jcomp.Add(newProperty);
                                        }
                                    }
                                    else
                                    {
                                        var udfColumnPermission = udfPermissons.FirstOrDefault(x => x.ColumnMetadataId == tempmodel.columnMetadataId && x.TemplateId == model.TemplateId);
                                        if (udfColumnPermission != null)
                                        {
                                            udfColumnPermission.ActiveUserType = model.ActiveUserType;
                                            udfColumnPermission.NtsStatusCode = model.ServiceStatusCode;
                                            if (!udfColumnPermission.IsVisible)
                                            {
                                                var hiddenProperty = jcomp.SelectToken("hidden");
                                                if (hiddenProperty == null)
                                                {
                                                    var newProperty = new JProperty("hidden", true);
                                                    jcomp.Add(newProperty);
                                                    if (type == "datagrid" || type == "editgrid")
                                                    {
                                                        JArray dataRows = (JArray)jcomp.SelectToken("components");
                                                        foreach (JObject jcomp1 in dataRows)
                                                        {
                                                            var newProperty1 = new JProperty("hidden", true);
                                                            jcomp1.Add(newProperty1);
                                                        }
                                                    }

                                                }
                                            }
                                            if (!udfColumnPermission.IsEditable || isReadonly || model.AllReadOnly)
                                            {
                                                var disableProperty = jcomp.SelectToken("disabled");
                                                if (disableProperty == null)
                                                {
                                                    var newProperty = new JProperty("disabled", true);
                                                    jcomp.Add(newProperty);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var hiddenProperty = jcomp.SelectToken("hidden");
                                            if (hiddenProperty == null)
                                            {
                                                var newProperty = new JProperty("hidden", true);
                                                jcomp.Add(newProperty);
                                                if (type == "datagrid" || type == "editgrid")
                                                {
                                                    JArray dataRows = (JArray)jcomp.SelectToken("components");
                                                    foreach (JObject jcomp1 in dataRows)
                                                    {
                                                        var newProperty1 = new JProperty("hidden", true);
                                                        jcomp1.Add(newProperty1);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                            }

                        }
                    }
                    else if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await ChildComp(rows, Columns, model);
                        }
                    }
                    else if (type == "table")
                    {
                        var rows = (JArray)jcomp.SelectToken("rows");
                        foreach (var row in rows)
                        {
                            if (row != null)
                            {
                                foreach (JToken cell in row.Children())
                                {
                                    var comp = (JArray)cell.SelectToken("components");
                                    await ChildComp(comp, Columns, model);
                                }
                            }
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            await ChildComp(rows, Columns, model);
                    }
                }
            }
        }
        public async Task<ActionResult> AddGroup(string nodeId, string parentId, DataActionEnum dataAction, LogicalEnum? condition, string ruleId, string id, string decisionScriptComponentId)
        {
            if (!id.IsNullOrEmpty())
            {
                var model = await _ruleBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
                model.RuleId = ruleId;
                return View(model);
            }
            else
            {
                var model = new BusinessRuleModelViewModel
                {
                    BusinessRuleNodeId = nodeId,
                    ParentId = parentId,
                    DataAction = dataAction,
                    Condition = condition,
                    RuleId = ruleId,
                    BusinessRuleType = BusinessRuleTypeEnum.Logical,
                    BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule,
                    DecisionScriptComponentId = decisionScriptComponentId
                };
                var result = await _ruleBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
                // ViewBag.ruleId = ruleId;
                //  return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessRuleBuilderGroup(DataActionEnum dataAction, string nodeId, string parentId, LogicalEnum condition, string ruleId, string id, string decisionScriptComponetId)
        {
            if (dataAction == DataActionEnum.Create)
            {
                var model = new BusinessRuleModelViewModel
                {
                    DataAction = dataAction,
                    BusinessRuleNodeId = nodeId,
                    ParentId = parentId,
                    Condition = condition,
                    RuleId = ruleId,
                    BusinessRuleType = BusinessRuleTypeEnum.Logical,
                    BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule,
                    DecisionScriptComponentId = decisionScriptComponetId
                };

                var result = await _ruleBusiness.Create(model);
                if (result.IsSuccess)
                {
                    //return PopupRedirect("Group created successfully");
                    return Json(new { success = true, decisionScriptComponetId = decisionScriptComponetId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            else if (dataAction == DataActionEnum.Edit)
            {
                var model = await _ruleBusiness.GetSingleById(id);
                if (model.Condition == LogicalEnum.And)
                {
                    model.Condition = LogicalEnum.Or;
                }
                else
                {
                    model.Condition = LogicalEnum.And;
                }
                //model.Condition = condition;
                model.DataAction = DataActionEnum.Edit;
                var result = await _ruleBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, decisionScriptComponetId = decisionScriptComponetId });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessRuleCondition(BusinessRuleModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var left = "";
                var leftB = "";
                var right = "";
                var rightB = "";
                if (model.FieldSourceType == BreMetadataTypeEnum.InputData)
                {
                    left = "Input" + "." + model.ParentFieldId + "." + model.Field;
                    if (model.FieldDataType == DataColumnTypeEnum.Text || model.FieldDataType == DataColumnTypeEnum.DateTime)
                    {
                        leftB = "'$!Input" + "." + model.ParentFieldId + "." + model.Field + "'";
                    }
                    else
                    {
                        leftB = "$!Input" + "." + model.ParentFieldId + "." + model.Field;
                    }

                }
                else if (model.FieldSourceType == BreMetadataTypeEnum.MasterData)
                {
                    left = "Master" + "." + model.ParentFieldId + "." + model.Field;
                    if (model.FieldDataType == DataColumnTypeEnum.Text || model.FieldDataType == DataColumnTypeEnum.DateTime)
                    {
                        leftB = "'$!Master" + "." + model.ParentFieldId + "." + model.Field + "'";
                    }
                    else
                    {
                        leftB = "$!Master" + "." + model.ParentFieldId + "." + model.Field;
                    }

                }
                else if (model.FieldSourceType == BreMetadataTypeEnum.Constant)
                {
                    left = model.Field;
                    leftB = model.Field;
                }
                if (model.ValueSourceType == BreMetadataTypeEnum.InputData)
                {
                    right = "Input" + "." + model.ParentValueId + "." + model.Value;
                    if (model.FieldDataType == DataColumnTypeEnum.Text || model.FieldDataType == DataColumnTypeEnum.DateTime)
                    {
                        rightB = "'$!Input" + "." + model.ParentValueId + "." + model.Value + "'";
                    }
                    else
                    {
                        rightB = "$!Input" + "." + model.ParentValueId + "." + model.Value;
                    }

                }
                else if (model.ValueSourceType == BreMetadataTypeEnum.MasterData)
                {
                    right = "Master" + "." + model.ParentValueId + "." + model.Value;
                    if (model.FieldDataType == DataColumnTypeEnum.Text || model.FieldDataType == DataColumnTypeEnum.DateTime)
                    {
                        rightB = "'$!Master" + "." + model.ParentValueId + "." + model.Value + "'";
                    }
                    else
                    {
                        rightB = "$!Master" + "." + model.ParentValueId + "." + model.Value;
                    }

                }
                else if (model.ValueSourceType == BreMetadataTypeEnum.Constant)
                {
                    right = model.Value;
                    rightB = model.Value;
                }


                var symbol = "";


                if (model.OperatorType == EqualityOperationEnum.Equal)
                {
                    symbol = "==";
                }
                else if (model.OperatorType == EqualityOperationEnum.GreaterThan)
                {
                    symbol = ">";
                }
                else if (model.OperatorType == EqualityOperationEnum.GreaterThanOrEqual)
                {
                    symbol = ">=";
                }
                else if (model.OperatorType == EqualityOperationEnum.LessThan)
                {
                    symbol = "<";
                }
                else if (model.OperatorType == EqualityOperationEnum.LessThanOrEqual)
                {
                    symbol = "<=";
                }
                else if (model.OperatorType == EqualityOperationEnum.NotEqual)
                {
                    symbol = "!=";
                }
                model.OperationValue = $"{left} {symbol} {right}";
                model.OperationBackendValue = $"{leftB} {symbol} {rightB}";
                if (model.DataAction == DataActionEnum.Create)
                {

                    var result = await _ruleBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _ruleBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("AddCondition", model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessRuleData(string templateId, string actionId)
        {
            var bre = await _breRuleBusiness.GetSingle(x => x.TemplateId == templateId && x.ActionId == actionId);
            var model = new BusinessRuleViewModel();
            if (bre != null)
            {
                model = bre;
                model.DataAction = DataActionEnum.Edit;
                return Json(new { success = true, BreId = bre.Id });
            }
            else
            {
                var temp = await _breRuleBusiness.GetSingleById<TemplateViewModel, Template>(templateId);
                var lov = await _breRuleBusiness.GetSingleById<LOVViewModel, LOV>(actionId);
                model.Name = temp.Name + "_" + lov.Name;
                model.Code = temp.Name + "_" + lov.Name;
                model.ActionId = actionId;
                model.TemplateId = temp.Id;
                var result = await _breRuleBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, BreId = result.Item.Id });
                }
            }

            return Json(new { success = false });
        }
        public async Task<IActionResult> DeleteRule(string id)
        {
            await _breRuleBusiness.Delete(id);
            return Json(new { success = true });
        }
    }
}