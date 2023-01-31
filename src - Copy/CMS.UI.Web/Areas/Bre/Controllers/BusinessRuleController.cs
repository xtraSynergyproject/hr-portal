using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.EJ2.Diagrams;
using Syncfusion.EJ2.Navigations;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CMS.UI.Utility;

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
        public BusinessRuleController(IBusinessRuleBusiness breRuleBusiness, IBusinessAreaBusiness breAreaBusiness, IBusinessRuleGroupBusiness breRuleGroupBusiness,
            IBusinessSectionBusiness breSectionBusiness, IBusinessRuleModelBusiness ruleBusiness,
            IComponentBusiness componentBusiness, IDecisionScriptComponentBusiness decisionScriptBusiness,
            IBusinessRuleNodeBusiness breRuleNodeBusiness)
        {
            _breRuleBusiness = breRuleBusiness;
            _breAreaBusiness = breAreaBusiness;
            _breRuleGroupBusiness = breRuleGroupBusiness;
            _breSectionBusiness = breSectionBusiness;
            _ruleBusiness = ruleBusiness;
            _componentBusiness = componentBusiness;
            _decisionScriptBusiness = decisionScriptBusiness;
            _breRuleNodeBusiness = breRuleNodeBusiness;
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
                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
                ViewBag.LayoutMode = LayoutModeEnum.Popup.ToString();
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
                    if (model.FieldDataType==DataColumnTypeEnum.Text || model.FieldDataType == DataColumnTypeEnum.DateTime)
                    {
                        leftB = "'$!Input" + "." + model.ParentFieldId + "." + model.Field+ "'";
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
                        leftB = "'$!Master" + "." + model.ParentFieldId + "." + model.Field+ "'";
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
