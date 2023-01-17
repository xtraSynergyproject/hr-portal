using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class ProcessDesignController : ApplicationController
    {
        IPageTemplateBusiness _pageTemplateBusiness;
        ITemplateBusiness _templateBusiness;
        IServiceTemplateBusiness _serviceTemplateBusiness;
        ITableMetadataBusiness _tableBusiness;
        IColumnMetadataBusiness _columnBusiness;
        IProcessDesignVariableBusiness _processDesignVariableBusiness;
        IProcessDesignBusiness _processDesignBusiness;
        IStepTaskComponentBusiness _stepTaskComponentBusiness;
        ITeamBusiness _teamBusiness;
        IAdhocTaskBusiness _adhocTaskBusiness;
        IDecisionScriptComponentBusiness _decisionScriptBusiness;
        IExecutionScriptBusiness _executionScriptBusiness;
        IHierarchyMasterBusiness _hierarchyMasterBusiness;
        IComponentBusiness _componentBusiness;
        IColumnMetadataBusiness _columnMetadataBusiness;
        ILOVBusiness _lovBusiness;
        IUdfPermissionBusiness _UdfPermissionBusiness;
        ITemplateCategoryBusiness _templateCategoryBusiness;
        ITaskTemplateBusiness _taskTemplateBusiness;
        ILOVBusiness _LOVBusiness;
        ITaskBusiness _taskBusiness;
        IStepTaskEscalationBusiness _stepTaskEscalationBusiness;
        IRuntimeWorkflowBusiness _runtimeWorkflowBusiness;
        IRuntimeWorkflowDataBusiness _runtimeWorkflowDataBusiness;
        IUserBusiness _userBusiness;
        IUserPortalBusiness _userPortalBusiness;
        ITeamUserBusiness _teamUserBusiness;
        public ProcessDesignController(IPageTemplateBusiness pageTemplateBusiness
            , ITemplateBusiness templateBusiness
            , ITableMetadataBusiness tableBusiness
            , IColumnMetadataBusiness columnBusiness,
            ITeamBusiness teamBusiness, IStepTaskEscalationBusiness stepTaskEscalationBusiness,
            IProcessDesignVariableBusiness processDesignVariableBusiness,
            IStepTaskComponentBusiness stepTaskComponentBusiness,
        IAdhocTaskBusiness adhocTaskBusiness,
        IDecisionScriptComponentBusiness decisionScriptBusiness, IHierarchyMasterBusiness hierarchyMasterBusiness,
        IComponentBusiness componentBusiness, IProcessDesignBusiness processDesignBusiness,
         IExecutionScriptBusiness executionScriptBusiness, IServiceTemplateBusiness serviceTemplateBusiness,
         IColumnMetadataBusiness columnMetadataBusiness,
         ILOVBusiness lovBusiness,
         IUdfPermissionBusiness UdfPermissionBusiness, ITemplateCategoryBusiness templateCategoryBusiness,
         ITaskTemplateBusiness taskTemplateBusiness, ILOVBusiness LOVBusiness
            , ITaskBusiness taskBusiness, IRuntimeWorkflowBusiness runtimeWorkflowBusiness, IRuntimeWorkflowDataBusiness runtimeWorkflowDataBusiness,
         IUserBusiness userBusiness, IUserPortalBusiness userPortalBusiness, ITeamUserBusiness teamUserBusiness
        )
        {
            _pageTemplateBusiness = pageTemplateBusiness;
            _templateBusiness = templateBusiness;
            _tableBusiness = tableBusiness;
            _columnBusiness = columnBusiness;
            _processDesignVariableBusiness = processDesignVariableBusiness;
            _stepTaskComponentBusiness = stepTaskComponentBusiness;
            _teamBusiness = teamBusiness;
            _adhocTaskBusiness = adhocTaskBusiness;
            _decisionScriptBusiness = decisionScriptBusiness;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _componentBusiness = componentBusiness;
            _processDesignBusiness = processDesignBusiness;
            _executionScriptBusiness = executionScriptBusiness;
            _serviceTemplateBusiness = serviceTemplateBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _lovBusiness = lovBusiness;
            _UdfPermissionBusiness = UdfPermissionBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _taskTemplateBusiness = taskTemplateBusiness;
            _LOVBusiness = LOVBusiness;
            _taskBusiness = taskBusiness;
            _stepTaskEscalationBusiness = stepTaskEscalationBusiness;
            _runtimeWorkflowBusiness = runtimeWorkflowBusiness;
            _runtimeWorkflowDataBusiness = runtimeWorkflowDataBusiness;
            _userBusiness = userBusiness;
            _userPortalBusiness = userPortalBusiness;
            _teamUserBusiness = teamUserBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateProcessDesign(string templateId)
        {
            var processDesign = await _processDesignBusiness.GetSingle(x => x.TemplateId == templateId);
            if (processDesign != null)
            {
                // Create Start Component
                ComponentViewModel comp = new ComponentViewModel();
                comp.ComponentType = ProcessDesignComponentTypeEnum.Start;
                comp.ProcessDesignId = processDesign.Id;
                comp.Name = "Start WorkFlow";
                var result1 = await _componentBusiness.Create(comp);
                if (result1.IsSuccess)
                {
                    return Json(new { success = true, templateId = processDesign.TemplateId, id = processDesign.Id, startId = result1.Item.Id });
                }
                else
                {
                    return Json(new { success = false, templateId = processDesign.TemplateId, id = processDesign.Id });
                }
            }
            else
            {
                ProcessDesignViewModel model = new ProcessDesignViewModel();
                model.ProcessDesignType = ProcessDesignTypeEnum.ProcessDesign;
                model.TemplateId = templateId;
                var result = await _processDesignBusiness.Create(model);
                if (result.IsSuccess)
                {
                    // Create Start Component
                    ComponentViewModel comp = new ComponentViewModel();
                    comp.ComponentType = ProcessDesignComponentTypeEnum.Start;
                    comp.ProcessDesignId = result.Item.Id;
                    comp.Name = "Start WorkFlow";
                    var result1 = await _componentBusiness.Create(comp);
                    if (result1.IsSuccess)
                    {
                        return Json(new { success = true, templateId = result.Item.TemplateId, id = result.Item.Id, startId = result1.Item.Id });
                    }
                    else
                    {
                        return Json(new { success = false, templateId = result.Item.TemplateId, id = result.Item.Id });
                    }
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

                }
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateComponent(ComponentViewModel model)
        {
            if (model.Id.IsNotNull())
            {
                var decisionviewModel = await _decisionScriptBusiness.GetSingleById(model.Id);
                if (decisionviewModel.IsNotNull())
                {
                    var Comp = await _componentBusiness.GetSingleById(decisionviewModel.ComponentId);
                    if (Comp != null)
                    {
                        Comp.Name = model.Name;
                        var res = await _componentBusiness.Edit(Comp);
                    }
                    decisionviewModel.Script = model.Script;
                    decisionviewModel.BusinessRuleLogicType = model.BusinessRuleLogicType;
                    var decisionresult = await _decisionScriptBusiness.Edit(decisionviewModel);
                    if (decisionresult.IsSuccess)
                    {
                        return Json(new { success = true, id = decisionresult.Item.Id });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            else
            {
                var process = await _processDesignBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
                if (process != null)
                {
                    model.ProcessDesignId = process.Id;
                    var result1 = await _componentBusiness.Create(model);
                    if (result1.IsSuccess)
                    {
                        if (model.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                        {
                            // Create left Component
                            ComponentViewModel left = new ComponentViewModel();
                            left.ProcessDesignId = process.Id;
                            left.Name = "True";
                            left.ParentId = result1.Item.Id;
                            left.ComponentType = ProcessDesignComponentTypeEnum.True;
                            var leftresult = await _componentBusiness.Create(left);
                            // Create Right Component
                            ComponentViewModel right = new ComponentViewModel();
                            right.ProcessDesignId = process.Id;
                            right.Name = "False";
                            right.ParentId = result1.Item.Id;
                            right.ComponentType = ProcessDesignComponentTypeEnum.False;
                            var rightresult = await _componentBusiness.Create(right);
                            // Create Decision Script
                            DecisionScriptComponentViewModel decisionviewModel = new DecisionScriptComponentViewModel
                            {
                                Script = model.Script,
                                BusinessRuleLogicType = model.BusinessRuleLogicType,
                                Id = model.DecisionScriptComponentId,
                                ComponentId = result1.Item.Id
                            };
                            var decisionresult = await _decisionScriptBusiness.Create(decisionviewModel);

                        }
                        return Json(new { success = true, id = result1.Item.Id });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });

        }
        public async Task<IActionResult> GetChart(string processdesignId, string templateId)
        {
            var list = new List<ComponentViewModel>();
            if (processdesignId.IsNotNullAndNotEmpty())
            {
                var components = await _componentBusiness.GetList(x => x.ProcessDesignId == processdesignId);
                if (components != null && components.Count() > 0)
                {
                    list = components;
                    foreach (var comp in list)
                    {
                        var parents = await _componentBusiness.GetComponentParent(comp.Id);
                        if (parents != null)
                        {
                            comp.ParentId = parents.Select(x => x.ParentId).FirstOrDefault();
                        }
                        else
                        {
                            comp.ParentId = null;
                        }
                        if (comp.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                        {
                            var prop = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == comp.Id);

                            if (prop != null)
                            {
                                var assignedToUserType = await _lovBusiness.GetSingleById(prop.AssignedToTypeId);
                                prop.AssignedToTypeCode = assignedToUserType?.Code;
                                if (prop.SLA.HasValue)
                                {
                                    prop.SLASeconds = prop.SLA.Value.TotalSeconds;
                                }
                                comp.Properties = JsonConvert.SerializeObject(prop);
                            }
                        }
                        if (comp.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                        {
                            var prop = await _decisionScriptBusiness.GetSingle(x => x.ComponentId == comp.Id);
                            if (prop.IsNotNull())
                            {
                                comp.Properties = JsonConvert.SerializeObject(prop);
                            }
                        }
                        if (comp.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                        {
                            var prop = await _executionScriptBusiness.GetSingle(x => x.ComponentId == comp.Id);
                            if (prop.IsNotNull())
                            {
                                comp.Properties = JsonConvert.SerializeObject(prop);
                            }
                        }

                    }
                }
                else
                {
                    list.Add(new ComponentViewModel { Id = processdesignId, ParentId = null, Name = "Start", ComponentType = ProcessDesignComponentTypeEnum.Start });
                }
            }
            else
            {
                ProcessDesignViewModel model = new ProcessDesignViewModel();
                model.ProcessDesignType = ProcessDesignTypeEnum.ProcessDesign;
                model.TemplateId = templateId;
                var result = await _processDesignBusiness.Create(model);
                if (result.IsSuccess)
                {
                    list.Add(new ComponentViewModel { Id = result.Item.Id, ParentId = null, Name = "Start", ComponentType = ProcessDesignComponentTypeEnum.Start });
                }
            }
            return await Task.FromResult(Json(list));
        }
        public async Task<IActionResult> ManageWorkFlow(string TemplateId, string id)
        {
            var model = new ProcessDesignVariableViewModel();

            if (id.IsNotNullAndNotEmpty())
            {
                model = await _processDesignVariableBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.TemplateId = TemplateId;
            }

            return View(model);
        }
        public async Task<IActionResult> ManageStartEvent(string TemplateId)
        {
            var model = new ProcessDesignVariableViewModel();
            model.TemplateId = TemplateId;
            return View(model);
        }
        public IActionResult ManageContext(string ProcessDesignId)
        {
            ViewBag.ProcessDesignId = ProcessDesignId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVariable(ProcessDesignVariableViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var Pageresult = await _processDesignVariableBusiness.Create(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    var Pageresult = await _processDesignVariableBusiness.Edit(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> ManageAdhocTask(AdhocTaskComponentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var Pageresult = await _adhocTaskBusiness.Create(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    var Pageresult = await _adhocTaskBusiness.Edit(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        //[HttpPost]
        //public async Task<IActionResult> ManageStepTask(StepTaskComponentViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (model.DataAction == DataActionEnum.Create)
        //        {
        //            var Pageresult = await _stepTaskComponentBusiness.Create(model);
        //            if (Pageresult.IsSuccess)
        //            {
        //                return Json(new { success = true, Id = Pageresult.Item.Id });
        //            }
        //            else
        //            {
        //                ModelState.AddModelErrors(Pageresult.Messages);
        //                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        //            }

        //        }
        //        if (model.DataAction == DataActionEnum.Edit)
        //        {
        //            var Pageresult = await _stepTaskComponentBusiness.Edit(model);
        //            if (Pageresult.IsSuccess)
        //            {
        //                return Json(new { success = true, Id = Pageresult.Item.Id });
        //            }
        //            else
        //            {
        //                ModelState.AddModelErrors(Pageresult.Messages);
        //                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        //            }

        //        }
        //    }
        //    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        //}

        public async Task<IActionResult> LoadVariable(string elementName, string templateId, string ProcessDesignId)
        {
            ViewBag.TemplateId = templateId;
            return View("_Variable", new VariableViewModel { Element = elementName });
        }
        public async Task<IActionResult> ManageComponent(string compType, string compId, string blockid, string blockprop)
        {
            var type = compType.ToEnum<ProcessDesignComponentTypeEnum>();
            switch (type)
            {
                case ProcessDesignComponentTypeEnum.Email:
                    return await ManageEmail(compId);
                //case ProcessDesignComponentTypeEnum.AdhocTask:
                //    return await ManageAdhocTask(compId);
                case ProcessDesignComponentTypeEnum.StepTask:
                // return await ManageStepTask(compId, blockid, blockprop);

                // return await ManageStepTask(compId);
                case ProcessDesignComponentTypeEnum.DecisionScript:
                    return await ManageDecisionScript(compId, blockid, blockprop);
                case ProcessDesignComponentTypeEnum.ExecutionScript:
                    return await ManageExecutionScript(compId, blockid, blockprop);


                default:
                    break;
            }
            return await ManageEmail(compId);
        }
        public async Task<IActionResult> ManageEmail(string id)
        {
            var model = new EmailComponentViewModel();
            model.DataAction = DataActionEnum.Create;
            return View("_Email", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditEmail([FromBody] EmailComponentViewModel model)
        {
            if (model == null)
            {
                model = new EmailComponentViewModel();
            }
            model.DataAction = DataActionEnum.Create;
            model.TempId = model.TempId;
            return View("_Email", model);
        }
        public async Task<IActionResult> ManageAdhocTask(string id)
        {
            var model = new AdhocTaskComponentViewModel();
            model.DataAction = DataActionEnum.Create;
            model.AssigneeType = TaskAssignedToTypeEnum.User;
            return View("_AdhocTask", model);
        }

        public async Task<IActionResult> ManageStepTask(string serviceTemplateId, string id, string parentId, bool? isDiagram, LayoutModeEnum? lo = LayoutModeEnum.Main)
        {
            var model = new StepTaskComponentViewModel();
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
                ViewBag.PageType = "Popup";
            }
            if (id.IsNotNullAndNotEmpty())
            {
                var component = await _componentBusiness.GetSingleById(id);
                model = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == component.Id);
                model.DataAction = DataActionEnum.Edit;
                var AssignToTypeCode = await _lovBusiness.GetSingle(x => x.Id == model.AssignedToTypeId);
                model.AssignedToTypeCode = AssignToTypeCode.Code;
                model.ServiceTemplateId = serviceTemplateId;
                model.ParentId = component.ParentId;
            }
            else
            {
                var processDesign = await _processDesignBusiness.GetSingle(x => x.TemplateId == serviceTemplateId);
                model.AssignedToTypeCode = "TASK_ASSIGN_TO_USER";
                model.DataAction = DataActionEnum.Create;
                var taskprioritytype = await _LOVBusiness.GetSingle(x => x.LOVType == "TASK_PRIORITY" && x.Code == "TASK_PRIORITY_MEDIUM");
                model.PriorityId = taskprioritytype.Id;
                model.ServiceTemplateId = serviceTemplateId;
                model.ComponentId = Guid.NewGuid().ToString();
                //model.Id = Guid.NewGuid().ToString();
                model.ParentId = parentId;
                model.ProcessDesignId = processDesign.IsNotNull() ? processDesign.Id : null;
                //var servicetemplate = await _templateBusiness.GetSingleById(templateId);
                //if (servicetemplate != null)
                //{
                //    model.UdfTemplateId = servicetemplate.UdfTemplateId;
                //    model.UdfTableMetadataId = servicetemplate.UdfTableMetadataId;
                //}

            }
            if (isDiagram == true)
            {
                model.IsDiagram = true;
            }
            else
            {
                model.IsDiagram = false;
            }
            var servicetemplate = await _templateBusiness.GetSingleById(serviceTemplateId);
            if (servicetemplate.IsNotNull())
            {
                model.ModuleId = servicetemplate.ModuleId;
            }
            model.SLASeconds = model.SLA != null ? model.SLA.Value.TotalSeconds : 0;
            return View("_StepTask", model);
            //if (blockprop.IsNotNullAndNotEmpty())
            //{
            //    model = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(blockprop);
            //}
            //else
            //{

            //}
        }
        [HttpPost]
        public async Task<IActionResult> ManageStepTask(string templateId, string id, string compId, string blockprop, string parentId, bool? isDiagram)
        {
            var processDesign = await _processDesignBusiness.GetSingle(x => x.TemplateId == templateId);
            var model = new StepTaskComponentViewModel();
            if (blockprop.IsNotNullAndNotEmpty())
            {
                model = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(blockprop);
            }
            else
            {
                model.AssignedToTypeCode = "TASK_ASSIGN_TO_USER";
                model.DataAction = DataActionEnum.Create;
                var taskprioritytype = await _LOVBusiness.GetSingle(x => x.LOVType == "TASK_PRIORITY" && x.Code == "TASK_PRIORITY_MEDIUM");
                model.PriorityId = taskprioritytype.Id;
            }
            model.TemplateId = templateId;
            if (compId.IsNotNullAndNotEmpty())
            {
                model.ComponentId = compId;
            }
            else
            {
                model.ComponentId = Guid.NewGuid().ToString();
            }
            model.ParentId = parentId;
            model.ProcessDesignId = processDesign.IsNotNull() ? processDesign.Id : null;
            if (isDiagram == true)
            {
                model.IsDiagram = true;
            }
            else
            {
                model.IsDiagram = false;
            }
            model.ParentId = parentId;
            return View("_StepTask", model);
        }
        public IActionResult AddExistingComponent(string parentId, string templateId)
        {
            var model = new StepTaskComponentViewModel();
            model.ParentId = parentId;
            model.TemplateId = templateId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddExistingComponent(StepTaskComponentViewModel model)
        {
            var parentlist = await _componentBusiness.GetComponentParent(model.ComponentId);
            var exist = parentlist.Where(x => x.ParentId == model.ParentId);
            if (exist.Any())
            {
                return Json(new { success = false, error = "Parent relation already exist for the selected component" });
            }
            else
            {
                string[] parents = new string[] { model.ParentId };
                await _componentBusiness.CreateComponentParents(model.ComponentId, parents);
                return Json(new { success = true });
            }

        }

        [HttpPost]
        public async Task<IActionResult> ManagePage(PageTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = JObject.Parse(model.Json);
                var comp = result["components"].ToString();
                JArray rows = (JArray)result.SelectToken("components");

                if (model.DataAction == DataActionEnum.Create)
                {
                    var Pageresult = await _pageTemplateBusiness.Create(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, templateId = Pageresult.Item.TemplateId });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                    //var res = await _templateBusiness.CreateTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Page });
                    //if (res.IsSuccess)
                    //{
                    //    await _pageTemplateBusiness.Create(model);
                    //}
                    //else
                    //{
                    //    return View("_ManagePage", model);
                    //}
                }
                //else if (model.DataAction == DataActionEnum.Edit)
                //{
                //    var res = await _templateBusiness.EditTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Page });
                //    if (!res.IsSuccess)
                //    {
                //        return View("_ManagePage", model);
                //    }
                //}
                var temp = await _templateBusiness.GetSingleById(model.TemplateId);
                temp.Json = model.Json;
                await _templateBusiness.Edit(temp);

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> GetVariablesList(string TemplateId)
        {
            var model = await _processDesignVariableBusiness.GetList(x => x.TemplateId == TemplateId);
            return Json(model);
        }

        public async Task<IActionResult> GetColumnsList(string TemplateId)
        {
            var model = await _processDesignVariableBusiness.GetListByTemplate(TemplateId);
            return Json(model);
        }
        public async Task<IActionResult> ManageDecisionScript(string id, string compId, string blockprop)
        {
            var model = new DecisionScriptComponentViewModel();
            var arr = 0;
            if (blockprop.IsNotNullAndNotEmpty())
            {
                //if (!int.TryParse(blockprop, out arr))
                //{
                model = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(blockprop);

                // model.Script = a.Script;
                model.DataAction = DataActionEnum.Edit;


                //var list = JsonConvert.DeserializeObject<List<DecisionScriptComponentViewModel>>(blockprop);
                //foreach (var a in list)
                //{
                //    model.Script = a.Script;

                //}
            }
            else { model.DataAction = DataActionEnum.Create; }
            model.ComponentId = compId;
            return View("_DecisionScript", model);
        }



        [HttpPost]
        public async Task<IActionResult> ManageDecisionScript(DecisionScriptComponentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var Pageresult = await _decisionScriptBusiness.Create(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    var Pageresult = await _decisionScriptBusiness.Edit(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> ManageExecutionScript(string id, string compId, string blockprop)
        {
            var model = new ExecutionScriptComponentViewModel();

            var arr = 0;
            if (blockprop.IsNotNullAndNotEmpty())
            {
                // if (!int.TryParse(blockprop, out arr))
                //{

                model = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(blockprop);
                model.DataAction = DataActionEnum.Edit;

                //foreach (var a in list)
                //{
                //model.Script = a.Script;

                //}
                // }
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            model.ComponentId = compId;
            return View("_ExecutionScript", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageExecutionScript(ExecutionScriptComponentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var Pageresult = await _executionScriptBusiness.Create(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    var Pageresult = await _executionScriptBusiness.Edit(model);
                    if (Pageresult.IsSuccess)
                    {
                        return Json(new { success = true, Id = Pageresult.Item.Id });
                    }
                    else
                    {
                        ModelState.AddModelErrors(Pageresult.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }

                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<ActionResult> ReadTeamData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _teamBusiness.GetList();
            var data = model.ToList();

            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        public async Task<ActionResult> ReadHierarchyMasterData()
        {
            var model = await _hierarchyMasterBusiness.GetList();
            var data = model.ToList();

            //var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        public async Task<ActionResult> ReadComponentData()
        {
            var model = await _stepTaskComponentBusiness.GetComponentList();
            var data = model.ToList();

            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }

        public async Task<ActionResult> ReadHierarchyMasterLevelDataById(string Id)
        {
            var model = new List<IdNameViewModel>();
            if (Id.IsNotNullAndNotEmpty())
            {
                var data = await _hierarchyMasterBusiness.GetHierarchyMasterLevelById(Id);

                return Json(data.ToList());
            }
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProcessFlow(ProcessDesignViewModel model)
        {
            CommandResult<ProcessDesignViewModel> result;
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var data = JsonConvert.DeserializeObject<dynamic>(model.Json);
                FlowYObject obj = new FlowYObject();
                obj.html = data.html;
                obj.blocks = data.blocks;
                obj.blockarr = data.blockarr;// data["blockarr"].ToString();                
                model.ProcessDesignHtml = JsonConvert.SerializeObject(obj);  // data["html"].ToString();
            }
            else
            {
                model.ProcessDesignHtml = "";
            }
            var processdesign = await _processDesignBusiness.GetSingle(x => x.TemplateId == model.TemplateId);
            if (processdesign != null)
            {
                processdesign.ProcessDesignHtml = model.ProcessDesignHtml;
                result = await _processDesignBusiness.Edit(processdesign);
                if (result.IsSuccess)
                {
                    if (model.Json.IsNotNullAndNotEmpty())
                    {
                        var data = JObject.Parse(model.Json);
                        var blocks = data["blocks"].ToArray();
                        //foreach (var arr  in blockarr)
                        //{
                        //    if(arr.parent==)
                        //}
                        foreach (var block in blocks)
                        {
                            ComponentViewModel comp = new ComponentViewModel();
                            comp.ProcessDesignId = result.Item.Id;
                            var blockdata = block["data"];
                            var blockproperties = "";
                            foreach (var prop in blockdata)
                            {
                                if (prop["name"].ToString() == "blockelemtype")
                                {
                                    comp.ComponentType = (ProcessDesignComponentTypeEnum)Enum.Parse(typeof(ProcessDesignComponentTypeEnum), prop["value"].ToString(), true);// (ComponentTypeEnum)prop["value"].ToString();
                                }
                                if (prop["name"].ToString() == "id")
                                {
                                    comp.Id = prop["value"].ToString();
                                }
                                if (prop["name"].ToString() == "blockProp")
                                {
                                    blockproperties = prop["value"].ToString();
                                }
                            }
                            var existingComp = await _componentBusiness.GetSingle(x => x.ProcessDesignId == result.Item.Id && x.Id == comp.Id);
                            if (existingComp != null)
                            {
                                foreach (var block1 in blocks)
                                {
                                    var block1data = block1["data"];
                                    foreach (var prop in block1data)
                                    {
                                        if (block["parent"].ToString() == block1["id"].ToString())
                                        {
                                            if (prop["name"].ToString() == "id")
                                            {
                                                comp.ParentId = prop["value"].ToString();
                                            }
                                        }
                                    }
                                }
                                existingComp.ParentId = comp.ParentId;
                                var result1 = await _componentBusiness.Edit(existingComp);
                                if (result1.IsSuccess)
                                {
                                    //foreach (var prop in blockdata)
                                    //     {
                                    //         if (prop["name"].ToString() == "blockProp")
                                    //         {
                                    if (blockproperties.IsNotNullAndNotEmpty())
                                    {
                                        var comproperties = blockproperties;// prop["value"].ToString();
                                        if (comproperties != null && comproperties != "0")
                                        {
                                            if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                            {
                                                var properties = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(comproperties);
                                                properties.ComponentId = result1.Item.Id;
                                                var CompProp = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                                if (CompProp.IsNotNull())
                                                {
                                                    properties.Id = CompProp.Id;
                                                    var res = await _stepTaskComponentBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    var res = await _stepTaskComponentBusiness.Create(properties);
                                                }
                                            }
                                            else if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                            {
                                                var properties = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(comproperties);
                                                properties.ComponentId = result1.Item.Id;
                                                var CompProp = await _executionScriptBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                                if (CompProp.IsNotNull())
                                                {
                                                    properties.Id = CompProp.Id;
                                                    var res = await _executionScriptBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    var res = await _executionScriptBusiness.Create(properties);
                                                }
                                            }
                                            else if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                            {
                                                var properties = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(comproperties);
                                                properties.ComponentId = result1.Item.Id;
                                                var CompProp = await _decisionScriptBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                                if (CompProp.IsNotNull())
                                                {
                                                    properties.Id = CompProp.Id;
                                                    var res = await _decisionScriptBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    var res = await _decisionScriptBusiness.Create(properties);
                                                }
                                            }
                                        }
                                    }
                                    // }
                                    // }   
                                }
                            }
                            else
                            {
                                //foreach (var prop in blockdata)
                                //{

                                //    if (prop["name"].ToString() == "id")
                                //    {
                                //        comp.Id = prop["value"].ToString();
                                //    }
                                //}
                                foreach (var block1 in blocks)
                                {
                                    var block1data = block1["data"];
                                    foreach (var prop in block1data)
                                    {
                                        if (block["parent"].ToString() == block1["id"].ToString())
                                        {
                                            if (prop["name"].ToString() == "id")
                                            {
                                                comp.ParentId = prop["value"].ToString();
                                            }
                                        }
                                    }
                                }// comp.ParentId = block["parent"].ToString(); 
                                var result1 = await _componentBusiness.Create(comp);
                                if (result1.IsSuccess)
                                {
                                    //foreach (var prop in blockdata)
                                    //      {
                                    //          if (prop["name"].ToString() == "blockProp")
                                    //          {
                                    if (blockproperties.IsNotNullAndNotEmpty())
                                    {
                                        var comproperties = blockproperties;// prop["value"].ToString();
                                        if (comproperties != null && comproperties != "0")
                                        {
                                            if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                            {
                                                var properties = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(comproperties);
                                                properties.ComponentId = result1.Item.Id;
                                                var CompProp = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                                if (CompProp.IsNotNull())
                                                {
                                                    properties.Id = CompProp.Id;
                                                    var res = await _stepTaskComponentBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    var res = await _stepTaskComponentBusiness.Create(properties);
                                                }
                                            }
                                            else if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                            {
                                                var properties = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(comproperties);
                                                properties.ComponentId = result1.Item.Id;
                                                var CompProp = await _executionScriptBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                                if (CompProp.IsNotNull())
                                                {
                                                    properties.Id = CompProp.Id;
                                                    var res = await _executionScriptBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    var res = await _executionScriptBusiness.Create(properties);
                                                }
                                            }
                                            else if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                            {
                                                var properties = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(comproperties);
                                                properties.ComponentId = result1.Item.Id;
                                                var CompProp = await _decisionScriptBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                                if (CompProp.IsNotNull())
                                                {
                                                    properties.Id = CompProp.Id;
                                                    var res = await _decisionScriptBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    var res = await _decisionScriptBusiness.Create(properties);
                                                }
                                            }
                                        }
                                    }
                                    //}
                                    //}
                                }
                            }
                        }
                    }
                    else
                    {
                        // delete existing component
                        var existingComp = await _componentBusiness.GetList(x => x.ProcessDesignId == result.Item.Id);
                        foreach (var comp in existingComp)
                        {
                            await _componentBusiness.Delete(comp.Id);
                        }
                    }
                }
            }
            else
            {
                result = await _processDesignBusiness.Create(model);
                if (result.IsSuccess)
                {
                    if (model.Json.IsNotNullAndNotEmpty())
                    {
                        var data = JObject.Parse(model.Json);
                        var blocks = data["blocks"].ToArray();
                        foreach (var block in blocks)
                        {
                            ComponentViewModel comp = new ComponentViewModel();
                            comp.ProcessDesignId = result.Item.Id;
                            var blockdata = block["data"];
                            string blockproperties = "";
                            foreach (var prop in blockdata)
                            {
                                if (prop["name"].ToString() == "blockelemtype")
                                {
                                    comp.ComponentType = (ProcessDesignComponentTypeEnum)Enum.Parse(typeof(ProcessDesignComponentTypeEnum), prop["value"].ToString(), true);// (ComponentTypeEnum)prop["value"].ToString();
                                }
                                if (prop["name"].ToString() == "id")
                                {
                                    comp.Id = prop["value"].ToString();
                                }
                                if (prop["name"].ToString() == "blockProp")
                                {
                                    blockproperties = prop["value"].ToString();
                                }
                            }
                            foreach (var block1 in blocks)
                            {
                                var block1data = block1["data"];
                                foreach (var prop in block1data)
                                {
                                    if (block["parent"].ToString() == block1["id"].ToString())
                                    {
                                        if (prop["name"].ToString() == "id")
                                        {
                                            comp.ParentId = prop["value"].ToString();
                                        }
                                    }
                                }
                            }
                            var result1 = await _componentBusiness.Create(comp);
                            if (result1.IsSuccess)
                            {
                                //foreach (var prop in blockdata)
                                //{
                                //if (prop["name"].ToString() == "blockProp")
                                // {
                                if (blockproperties.IsNotNullAndNotEmpty())
                                {
                                    //var comproperties = prop["value"].ToString();
                                    var comproperties = blockproperties;
                                    if (comproperties != null && comproperties != "0")
                                    {
                                        if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                        {
                                            var properties = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(comproperties);
                                            properties.ComponentId = result1.Item.Id;
                                            var CompProp = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                            if (CompProp.IsNotNull())
                                            {
                                                properties.Id = CompProp.Id;
                                                var res = await _stepTaskComponentBusiness.Edit(properties);
                                            }
                                            else
                                            {
                                                var res = await _stepTaskComponentBusiness.Create(properties);
                                            }
                                        }
                                        else if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                        {
                                            var properties = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(comproperties);
                                            properties.ComponentId = result1.Item.Id;
                                            var CompProp = await _executionScriptBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                            if (CompProp.IsNotNull())
                                            {
                                                properties.Id = CompProp.Id;
                                                var res = await _executionScriptBusiness.Edit(properties);
                                            }
                                            else
                                            {
                                                var res = await _executionScriptBusiness.Create(properties);
                                            }
                                        }
                                        else if (result1.Item.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                        {
                                            var properties = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(comproperties);
                                            properties.ComponentId = result1.Item.Id;
                                            var CompProp = await _decisionScriptBusiness.GetSingle(x => x.ComponentId == result1.Item.Id);
                                            if (CompProp.IsNotNull())
                                            {
                                                properties.Id = CompProp.Id;
                                                var res = await _decisionScriptBusiness.Edit(properties);
                                            }
                                            else
                                            {
                                                var res = await _decisionScriptBusiness.Create(properties);
                                            }
                                        }
                                    }
                                }
                                //}
                                //}
                            }
                            // comp.ComponentType = JObject.Parse(blockdata.ToString());
                        }
                    }
                }
            }
            // 
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> SaveProcessData(string data)
        {
            if (data.IsNotNullAndNotEmpty())
            {
                var process = JsonConvert.DeserializeObject<ProcessDesignViewModel>(data);
                var existingProcess = await _processDesignBusiness.GetSingleById(process.Id);
                if (existingProcess != null)
                {
                    // Edit Existing process
                    var result = await _processDesignBusiness.Edit(existingProcess);
                    if (result.IsSuccess)
                    {
                        //var ids = String.Join(",", process.RemovedItem).Replace(",","','");
                        //var childlist = await _componentBusiness.GetChildList(ids);

                        var existinglist = await _componentBusiness.GetList(x => x.ProcessDesignId == process.Id);

                        List<string> newlistids = new List<string>();
                        foreach (var component in process.Components)
                        {
                            newlistids.Add(component.Id);
                        }
                        var diff = existinglist.Where(item => !newlistids.Contains(item.Id));
                        if (diff.Any())
                        {
                            foreach (var item in diff)
                            {
                                await _componentBusiness.Delete(item.Id);
                                var type = await _componentBusiness.GetSingleById(item.Id);
                                if (type.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                {
                                    var steptaskcomplist = await _stepTaskComponentBusiness.GetList(x => x.ComponentId == item.Id);
                                    if (steptaskcomplist != null)
                                    {
                                        foreach (var stc in steptaskcomplist)
                                        {
                                            await _stepTaskComponentBusiness.Delete(stc.Id);
                                        }
                                    }
                                }
                                if (type.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                {
                                    var descscriptcomplist = await _decisionScriptBusiness.GetList(x => x.ComponentId == item.Id);
                                    if (descscriptcomplist != null)
                                    {
                                        foreach (var dsc in descscriptcomplist)
                                        {
                                            await _decisionScriptBusiness.Delete(dsc.Id);
                                        }
                                    }
                                }
                                if (type.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                {
                                    var exescriptcomplist = await _executionScriptBusiness.GetList(x => x.ComponentId == item.Id);
                                    if (exescriptcomplist != null)
                                    {
                                        foreach (var exe in exescriptcomplist)
                                        {
                                            await _executionScriptBusiness.Delete(exe.Id);
                                        }
                                    }
                                }
                            }
                        }

                        // Check for existing Components
                        foreach (var component in process.Components)
                        {
                            var existingComp = await _componentBusiness.GetSingleById(component.Id);
                            if (existingComp != null)
                            {
                                // edit existing Components
                                component.ComponentType = component.ComponentsType;
                                component.Name = Enum.GetName(typeof(ProcessDesignComponentTypeEnum), component.ComponentType);
                                var result1 = await _componentBusiness.Edit(component);
                                if (result1.IsSuccess)
                                {

                                    // Check for properties
                                    if (component.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                    {
                                        if (component.Properties.IsNotNullAndNotEmpty())
                                        {
                                            var properties = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(component.Properties);
                                            if (properties.IsNotNull())
                                            {
                                                var steptask = await _stepTaskComponentBusiness.GetSingleById(properties.Id);
                                                if (steptask != null)
                                                {
                                                    // Edit existing property
                                                    var templateId = await _taskTemplateBusiness.GetSingleById(properties.TaskTemplateId);
                                                    properties.TemplateId = templateId.TemplateId;
                                                    var servicetemplate = await _templateBusiness.GetSingleById(process.TemplateId);
                                                    if (servicetemplate != null)
                                                    {
                                                        properties.UdfTemplateId = servicetemplate.UdfTemplateId;
                                                        properties.UdfTableMetadataId = servicetemplate.UdfTableMetadataId;
                                                    }
                                                    var AssignToTypeCode = await _lovBusiness.GetSingle(x => x.Code == properties.AssignedToTypeCode);
                                                    properties.AssignedToTypeId = AssignToTypeCode.Id;
                                                    var result3 = await _stepTaskComponentBusiness.Edit(properties);
                                                    if (result3.IsSuccess)
                                                    {
                                                        if (properties.NoteUDFs.IsNotNullAndNotEmpty())
                                                        {
                                                            var udfs = JsonConvert.DeserializeObject<List<UdfPermissionViewModel>>(properties.NoteUDFs);
                                                            await UpdateNoteUDF(udfs, templateId.TemplateId);
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    // Create Property
                                                    var templateId = await _taskTemplateBusiness.GetSingleById(properties.TaskTemplateId);
                                                    properties.TemplateId = templateId.TemplateId;
                                                    var servicetemplate = await _templateBusiness.GetSingleById(process.TemplateId);
                                                    if (servicetemplate != null)
                                                    {
                                                        properties.UdfTemplateId = servicetemplate.UdfTemplateId;
                                                        properties.UdfTableMetadataId = servicetemplate.UdfTableMetadataId;
                                                    }
                                                    var AssignToTypeCode = await _lovBusiness.GetSingle(x => x.Code == properties.AssignedToTypeCode);
                                                    properties.AssignedToTypeId = AssignToTypeCode.Id;
                                                    var result3 = await _stepTaskComponentBusiness.Create(properties);
                                                    if (result3.IsSuccess)
                                                    {
                                                        if (properties.NoteUDFs.IsNotNullAndNotEmpty())
                                                        {
                                                            var udfs = JsonConvert.DeserializeObject<List<UdfPermissionViewModel>>(properties.NoteUDFs);
                                                            await UpdateNoteUDF(udfs, templateId.TemplateId);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else if (component.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                    {
                                        if (component.Properties.IsNotNullAndNotEmpty())
                                        {
                                            var properties = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(component.Properties);
                                            if (properties.IsNotNull())
                                            {
                                                var steptask = await _decisionScriptBusiness.GetSingleById(properties.Id);
                                                if (steptask != null)
                                                {
                                                    // Edit existing property
                                                    var result3 = await _decisionScriptBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    // Create Property
                                                    var result3 = await _decisionScriptBusiness.Create(properties);
                                                }
                                            }
                                        }

                                    }
                                    else if (component.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                    {
                                        if (component.Properties.IsNotNullAndNotEmpty())
                                        {
                                            var properties = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(component.Properties);
                                            if (properties.IsNotNull())
                                            {
                                                var steptask = await _executionScriptBusiness.GetSingleById(properties.Id);
                                                if (steptask != null)
                                                {
                                                    // Edit existing property
                                                    var result3 = await _executionScriptBusiness.Edit(properties);
                                                }
                                                else
                                                {
                                                    // Create Property
                                                    var result3 = await _executionScriptBusiness.Create(properties);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Create Newly Added Component
                                component.ComponentType = component.ComponentsType;
                                component.Name = Enum.GetName(typeof(ProcessDesignComponentTypeEnum), component.ComponentType);
                                var result1 = await _componentBusiness.Create(component);
                                if (result1.IsSuccess)
                                {
                                    // check for Properties
                                    if (component.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                    {
                                        if (component.Properties.IsNotNullAndNotEmpty())
                                        {
                                            var properties = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(component.Properties);
                                            if (properties.IsNotNull())
                                            {
                                                // Create Property
                                                var templateId = await _taskTemplateBusiness.GetSingleById(properties.TaskTemplateId);
                                                properties.TemplateId = templateId.TemplateId;
                                                var servicetemplate = await _templateBusiness.GetSingleById(process.TemplateId);
                                                if (servicetemplate != null)
                                                {
                                                    properties.UdfTemplateId = servicetemplate.UdfTemplateId;
                                                    properties.UdfTableMetadataId = servicetemplate.UdfTableMetadataId;
                                                }
                                                var AssignToTypeCode = await _lovBusiness.GetSingle(x => x.Code == properties.AssignedToTypeCode);
                                                properties.AssignedToTypeId = AssignToTypeCode.Id;
                                                var result3 = await _stepTaskComponentBusiness.Create(properties);
                                                if (result3.IsSuccess)
                                                {
                                                    if (properties.NoteUDFs.IsNotNullAndNotEmpty())
                                                    {
                                                        var udfs = JsonConvert.DeserializeObject<List<UdfPermissionViewModel>>(properties.NoteUDFs);
                                                        await UpdateNoteUDF(udfs, templateId.TemplateId);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else if (component.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                    {
                                        if (component.Properties.IsNotNullAndNotEmpty())
                                        {
                                            var properties = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(component.Properties);
                                            if (properties.IsNotNull())
                                            {
                                                var steptask = await _decisionScriptBusiness.GetSingleById(properties.Id);
                                                // Create Property
                                                var result3 = await _decisionScriptBusiness.Create(properties);
                                            }
                                        }

                                    }
                                    else if (component.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                    {
                                        if (component.Properties.IsNotNullAndNotEmpty())
                                        {
                                            var properties = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(component.Properties);
                                            if (properties.IsNotNull())
                                            {
                                                // Create Property
                                                var result3 = await _executionScriptBusiness.Create(properties);
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
                    // create New Process
                    var result = await _processDesignBusiness.Create(process);
                    if (result.IsSuccess)
                    {
                        // Create Components
                        foreach (var component in process.Components)
                        {
                            component.ComponentType = component.ComponentsType;
                            component.Name = Enum.GetName(typeof(ProcessDesignComponentTypeEnum), component.ComponentType);
                            var result1 = await _componentBusiness.Create(component);
                            if (result1.IsSuccess)
                            {
                                if (component.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                                {
                                    if (component.Properties.IsNotNullAndNotEmpty())
                                    {
                                        var properties = JsonConvert.DeserializeObject<StepTaskComponentViewModel>(component.Properties);
                                        if (properties.IsNotNull())
                                        {
                                            // Create Property
                                            var templateId = await _taskTemplateBusiness.GetSingleById(properties.TaskTemplateId);
                                            properties.TemplateId = templateId.TemplateId;
                                            var servicetemplate = await _templateBusiness.GetSingleById(process.TemplateId);
                                            if (servicetemplate != null)
                                            {
                                                properties.UdfTemplateId = servicetemplate.UdfTemplateId;
                                            }
                                            var AssignToTypeCode = await _lovBusiness.GetSingle(x => x.Code == properties.AssignedToTypeCode);
                                            properties.AssignedToTypeId = AssignToTypeCode.Id;
                                            var result3 = await _stepTaskComponentBusiness.Create(properties);
                                            if (result3.IsSuccess)
                                            {
                                                if (properties.NoteUDFs.IsNotNullAndNotEmpty())
                                                {
                                                    var udfs = JsonConvert.DeserializeObject<List<UdfPermissionViewModel>>(properties.NoteUDFs);
                                                    await UpdateNoteUDF(udfs, templateId.TemplateId);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (component.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                                {
                                    if (component.Properties.IsNotNullAndNotEmpty())
                                    {
                                        var properties = JsonConvert.DeserializeObject<DecisionScriptComponentViewModel>(component.Properties);
                                        if (properties.IsNotNull())
                                        {
                                            // Create Property
                                            var result3 = await _decisionScriptBusiness.Create(properties);
                                        }
                                    }

                                }
                                else if (component.ComponentType == ProcessDesignComponentTypeEnum.ExecutionScript)
                                {
                                    if (component.Properties.IsNotNullAndNotEmpty())
                                    {
                                        var properties = JsonConvert.DeserializeObject<ExecutionScriptComponentViewModel>(component.Properties);
                                        if (properties.IsNotNull())
                                        {
                                            // Create Property
                                            var result3 = await _executionScriptBusiness.Create(properties);
                                        }

                                    }

                                }

                            }
                        }
                    }
                }
            }

            return Json(new { success = true });
        }
        //public async Task<IActionResult> LoadServiceNoteUdf([DataSourceRequest] DataSourceRequest request,string templateId)
        //{
        //    List<UdfPermissionViewModel> list = new List<UdfPermissionViewModel>();
        //    var serviceTemplate = await _templateBusiness.GetSingleById(templateId);
        //    if (serviceTemplate != null)
        //    {                
        //        var data = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == serviceTemplate.UdfTableMetadataId && x.IsUdfColumn==true);
        //        list = data.Select(x => new UdfPermissionViewModel
        //        {
        //            Id=x.Id,
        //            Name = x.Name,
        //            EditableBy=x.EditableBy,
        //            EditableContext=x.EditableContext,     
        //            ViewableBy =x.ViewableBy,
        //            ViewableContext=x.ViewableContext,
        //            ColumnMetadataId=x.Id
        //        }).ToList();
        //    }
        //    return Json(list.ToDataSourceResult(request));
        //}
        public async Task<ActionResult> LoadServiceNoteUdf([DataSourceRequest] DataSourceRequest request, string templateId, string componentId)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == template.UdfTableMetadataId && x.IsUdfColumn == true);

            var model = new List<UdfPermissionViewModel>();
            //var stepTaskComp = await _stepTaskComponentBusiness.GetSingle(x => x.Id == componentId);
            var stepTaskComp = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == componentId);
            List<UdfPermissionViewModel> pageColumns = new List<UdfPermissionViewModel>();
            if (stepTaskComp != null)
            {
                pageColumns = await _UdfPermissionBusiness.GetList(x => x.TemplateId == stepTaskComp.TemplateId);
            }
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new UdfPermissionViewModel
                {
                    Name = column.Name,
                    ColumnMetadataId = column.Id,

                };

                if (componentId != null && pageColumns != null)
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnMetadataId == column.Id);
                    if (existingColumn != null)
                    {
                        //data.Name = existingColumn.Name;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.EditableBy = existingColumn.EditableBy;
                        data.ViewableBy = existingColumn.ViewableBy;
                        data.EditableContext = existingColumn.EditableContext;
                        data.ViewableContext = existingColumn.ViewableContext;
                        List<string> editableContext = new List<string>();
                        foreach (var rec in data.EditableContext)
                        {
                            //var lov = await _lovBusiness.GetSingleById(rec);
                            var lov = await _lovBusiness.GetSingle(x => x.Code == rec);
                            if (lov != null)
                            {
                                editableContext.Add(lov.Name);
                            }
                        }
                        List<string> viewableContext = new List<string>();
                        foreach (var rec in data.ViewableContext)
                        {
                            //var lov = await _lovBusiness.GetSingleById(rec);
                            var lov = await _lovBusiness.GetSingle(x => x.Code == rec);
                            if (lov != null)
                            {
                                viewableContext.Add(lov.Name);
                            }
                        }
                        List<string> viewableby = new List<string>();
                        foreach (var rec in data.ViewableBy)
                        {
                            //var enumDisplayStatus = (NtsActiveUserTypeEnum)Convert.ToInt32(rec);
                            //viewableby.Add(enumDisplayStatus.ToString());
                            viewableby.Add(rec);
                        }
                        List<string> editableby = new List<string>();
                        foreach (var rec in data.EditableBy)
                        {
                            //var enumDisplayStatus = (NtsActiveUserTypeEnum)Convert.ToInt32(rec);
                            //editableby.Add(enumDisplayStatus.ToString());
                            editableby.Add(rec);
                        }

                        data.EditableByDisplay = string.Join(",", editableby);
                        data.ViewableByDisplay = string.Join(",", viewableby);
                        data.EditableContextDisplay = string.Join(",", editableContext);
                        data.ViewableContextDisplay = string.Join(",", viewableContext);
                        data.DataAction = DataActionEnum.Edit;
                        model.Add(data);
                    }
                    else
                    {

                        data.SequenceOrder = column.SequenceOrder;
                        data.CreatedDate = DateTime.Now;
                        data.LastUpdatedDate = DateTime.Now;
                        data.Id = Guid.NewGuid().ToString();
                        data.DataAction = DataActionEnum.Create;
                        model.Add(data);
                    }
                }
                else
                {

                    data.SequenceOrder = column.SequenceOrder;
                    data.CreatedDate = DateTime.Now;
                    data.LastUpdatedDate = DateTime.Now;
                    data.Id = Guid.NewGuid().ToString();
                    data.DataAction = DataActionEnum.Create;
                    model.Add(data);
                }
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.Name).ToList();
            return Json(model);
            // return Json(model.ToDataSourceResult(request));
        }

        public async Task<ActionResult> LoadServiceNoteUdf1(string templateId, string componentId)
        {
            // fetch  table with the help of templateId
            var template = await _templateBusiness.GetSingleById(templateId);
            // fetch all columns of the table with the help of tableid
            var columnMetaData = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == template.UdfTableMetadataId && x.IsUdfColumn == true);

            var model = new List<UdfPermissionViewModel>();
            //var stepTaskComp = await _stepTaskComponentBusiness.GetSingle(x => x.Id == componentId);
            var stepTaskComp = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == componentId);
            List<UdfPermissionViewModel> pageColumns = new List<UdfPermissionViewModel>();
            if (stepTaskComp != null)
            {
                pageColumns = await _UdfPermissionBusiness.GetList(x => x.TemplateId == stepTaskComp.TemplateId);
            }
            foreach (var column in columnMetaData.Where(x => x.IsHiddenColumn == false))
            {
                var data = new UdfPermissionViewModel
                {
                    Name = column.Name,
                    ColumnMetadataId = column.Id,

                };

                if (componentId != null && pageColumns != null)
                {
                    var existingColumn = pageColumns.FirstOrDefault(x => x.ColumnMetadataId == column.Id);
                    if (existingColumn != null)
                    {
                        //data.Name = existingColumn.Name;
                        data.SequenceOrder = existingColumn.SequenceOrder;
                        data.LastUpdatedDate = existingColumn.LastUpdatedDate;
                        data.CreatedDate = existingColumn.CreatedDate;
                        data.Id = existingColumn.Id;
                        data.EditableBy = existingColumn.EditableBy;
                        data.ViewableBy = existingColumn.ViewableBy;
                        data.EditableContext = existingColumn.EditableContext;
                        data.ViewableContext = existingColumn.ViewableContext;
                        List<string> editableContext = new List<string>();
                        foreach (var rec in data.EditableContext)
                        {
                            //var lov = await _lovBusiness.GetSingleById(rec);
                            var lov = await _lovBusiness.GetSingle(x => x.Code == rec);
                            if (lov != null)
                            {
                                editableContext.Add(lov.Name);
                            }
                        }
                        List<string> viewableContext = new List<string>();
                        foreach (var rec in data.ViewableContext)
                        {
                            //var lov = await _lovBusiness.GetSingleById(rec);
                            var lov = await _lovBusiness.GetSingle(x => x.Code == rec);
                            if (lov != null)
                            {
                                viewableContext.Add(lov.Name);
                            }
                        }
                        List<string> viewableby = new List<string>();
                        foreach (var rec in data.ViewableBy)
                        {
                            //var enumDisplayStatus = (NtsActiveUserTypeEnum)Convert.ToInt32(rec);
                            //viewableby.Add(enumDisplayStatus.ToString());
                            viewableby.Add(rec);
                        }
                        List<string> editableby = new List<string>();
                        foreach (var rec in data.EditableBy)
                        {
                            //var enumDisplayStatus = (NtsActiveUserTypeEnum)Convert.ToInt32(rec);
                            //editableby.Add(enumDisplayStatus.ToString());
                            editableby.Add(rec);
                        }

                        data.EditableByDisplay = string.Join(",", editableby);
                        data.ViewableByDisplay = string.Join(",", viewableby);
                        data.EditableContextDisplay = string.Join(",", editableContext);
                        data.ViewableContextDisplay = string.Join(",", viewableContext);
                        data.DataAction = DataActionEnum.Edit;
                        model.Add(data);
                    }
                    else
                    {

                        data.SequenceOrder = column.SequenceOrder;
                        data.CreatedDate = DateTime.Now;
                        data.LastUpdatedDate = DateTime.Now;
                        data.Id = Guid.NewGuid().ToString();
                        data.DataAction = DataActionEnum.Create;
                        model.Add(data);
                    }
                }
                else
                {

                    data.SequenceOrder = column.SequenceOrder;
                    data.CreatedDate = DateTime.Now;
                    data.LastUpdatedDate = DateTime.Now;
                    data.Id = Guid.NewGuid().ToString();
                    data.DataAction = DataActionEnum.Create;
                    model.Add(data);
                }
            }
            model = model.OrderBy(x => x.SequenceOrder).OrderBy(x => x.Name).ToList();
            return Json(model);
        }
        public async Task UpdateNoteUDF(List<UdfPermissionViewModel> data, string templateId)
        {
            foreach (var udf in data)
            {
                var record = await _UdfPermissionBusiness.GetSingleById(udf.Id);
                if (record != null)
                {
                    record.TemplateId = templateId;
                    record.EditableBy = udf.EditableBy;
                    record.ViewableBy = udf.ViewableBy;
                    record.EditableContext = udf.EditableContext;
                    record.ViewableContext = udf.ViewableContext;
                    // record.Name = udf.Name;                  
                    await _UdfPermissionBusiness.Edit(record);
                }
                else
                {
                    udf.TemplateId = templateId;
                    // udf.ColumnMetadataId = udf.Id;
                    await _UdfPermissionBusiness.Create(udf);
                }

            }
        }
        public async Task<ActionResult> DeleteComponent(string CompId)
        {
            await _componentBusiness.Delete(CompId);
            return Json(new { success = true });
        }

        public async Task<ActionResult> StepTaskTemplate(string templateId, string moduleId)
        {
            var tempcategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "STEP_TASK_CATEGORY");
            var model = new TemplateViewModel
            {
                DataAction = DataActionEnum.Create,
                TemplateType = TemplateTypeEnum.Task
            };
            if (templateId.IsNotNullAndNotEmpty())
            {
                var template = await _templateBusiness.GetSingleById(templateId);
                if (template != null)
                {
                    //model.UdfTableMetadataId = template.UdfTableMetadataId;
                    var category = await _templateCategoryBusiness.GetSingle(x => x.Id == template.TemplateCategoryId);
                    if (category != null)
                    {
                        var taskcategory = await _templateCategoryBusiness.GetSingle(x => x.TemplateType == TemplateTypeEnum.Task && x.Code == category.Code);
                        if (taskcategory != null)
                        {
                            model.TemplateCategoryId = taskcategory.Id;
                            model.TemplateCategoryName = taskcategory.Name;
                        }
                        else
                        {
                            model.TemplateCategoryId = tempcategory.Id;
                            model.TemplateCategoryName = tempcategory.Name;
                        }
                    }
                }
            }
            else
            {
                model.TemplateCategoryId = tempcategory.Id;
                model.TemplateCategoryName = tempcategory.Name;
            }
            model.ModuleId = moduleId;
            return View("_StepTaskTemplate", model);
        }
        public async Task<ActionResult> EditStepTaskTemplate(string templateId, string moduleId, string taskTempId)
        {
            var model = await _templateBusiness.GetSingleById(templateId);
            model.DataAction = DataActionEnum.Edit;


            return View("_StepTaskTemplate", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageStepTaskTemplate(TemplateViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    model.TaskType = TaskTypeEnum.StepTask;
                    var result = await _templateBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        var tasktemplate = await _templateBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == result.Item.Id);

                        return Json(new { success = true, tasktemplateId = tasktemplate.Id });

                        //return PopupRedirect("Style created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _templateBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true, templateId = result.Item.Id });
                        //return PopupRedirect("Style edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            //return RedirectToAction("Index");
            // return View("Index", new SynergyTemplateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageStepTaskComponentWithProperty(StepTaskComponentViewModel model)
        {
            if (model.IsDiagram)
            {
                if (ModelState.IsValid)
                {
                    var AssignToTypeCode = await _lovBusiness.GetSingle(x => x.Code == model.AssignedToTypeCode);
                    model.AssignedToTypeId = AssignToTypeCode.Id;
                    var taskTemplate = await _taskTemplateBusiness.GetSingleById(model.TaskTemplateId);
                    model.TemplateId = taskTemplate.TemplateId;
                    var servicetemplate = await _templateBusiness.GetSingleById(model.ServiceTemplateId);
                    if (servicetemplate != null)
                    {
                        model.UdfTemplateId = servicetemplate.UdfTemplateId;
                        model.UdfTableMetadataId = servicetemplate.UdfTableMetadataId;
                    }
                    //var servicetemplate = await _templateBusiness.GetSingleById(model.TemplateId);
                    //if (servicetemplate != null)
                    //{
                    //    model.UdfTemplateId = servicetemplate.UdfTemplateId;
                    //    model.UdfTableMetadataId = servicetemplate.UdfTableMetadataId;
                    //}
                    if (model.Id.IsNotNullAndNotEmpty())
                    {
                        var Pageresult = await _stepTaskComponentBusiness.Edit(model);
                        if (Pageresult.IsSuccess)
                        {
                            var comp = await _componentBusiness.GetSingleById(model.ComponentId);
                            if (comp.ParentId != model.ParentId)
                            {

                                comp.ParentId = model.ParentId;
                                var res = await _componentBusiness.Edit(comp);
                            }
                            if (model.NoteUDFs.IsNotNullAndNotEmpty())
                            {
                                var udfs = JsonConvert.DeserializeObject<List<UdfPermissionViewModel>>(model.NoteUDFs);
                                await UpdateNoteUDF(udfs, taskTemplate.TemplateId);
                            }
                            return Json(new { success = true, Id = Pageresult.Item.Id });
                        }
                        else
                        {
                            ModelState.AddModelErrors(Pageresult.Messages);
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }
                    }
                    else
                    {
                        ComponentViewModel comp = new ComponentViewModel();
                        comp.ComponentType = ProcessDesignComponentTypeEnum.StepTask;
                        comp.Name = "StepTask";
                        comp.ProcessDesignId = model.ProcessDesignId;
                        comp.ParentId = model.ParentId;
                        var result = await _componentBusiness.Create(comp);
                        if (result.IsSuccess)
                        {
                            model.ComponentId = result.Item.Id;
                            var Pageresult = await _stepTaskComponentBusiness.Create(model);
                            if (Pageresult.IsSuccess)
                            {
                                if (model.NoteUDFs.IsNotNullAndNotEmpty())
                                {
                                    var udfs = JsonConvert.DeserializeObject<List<UdfPermissionViewModel>>(model.NoteUDFs);
                                    await UpdateNoteUDF(udfs, taskTemplate.TemplateId);
                                }
                                return Json(new { success = true, Id = Pageresult.Item.Id });
                            }
                            else
                            {
                                ModelState.AddModelErrors(Pageresult.Messages);
                                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                            }
                        }
                        return Json(new
                        {
                            success = false
                        });
                    }

                    //if (model.DataAction == DataActionEnum.Create)
                    //{
                    //    var Pageresult = await _stepTaskComponentBusiness.Create(model);
                    //    if (Pageresult.IsSuccess)
                    //    {
                    //        return Json(new { success = true, Id = Pageresult.Item.Id });
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelErrors(Pageresult.Messages);
                    //        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    //    }

                    //}
                    //if (model.DataAction == DataActionEnum.Edit)
                    //{
                    //    var Pageresult = await _stepTaskComponentBusiness.Edit(model);
                    //    if (Pageresult.IsSuccess)
                    //    {
                    //        return Json(new { success = true, Id = Pageresult.Item.Id });
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelErrors(Pageresult.Messages);
                    //        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    //    }

                    //}
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public async Task<ActionResult> GetStepTaskComponent(string templateId, string serviceId, string taskId)
        {
            var processDesign = await _processDesignBusiness.GetSingle(x => x.TemplateId == templateId);
            var comp = await _componentBusiness.GetList(x => x.ProcessDesignId == processDesign.Id);
            var ids = comp.Select(x => x.Id);
            var model = await _stepTaskComponentBusiness.GetList(x => ids.Contains(x.ComponentId));
            var data = model.ToList();
            if (serviceId.IsNotNullAndNotEmpty())
            {
                var list = await _taskBusiness.GetList(x => x.ParentServiceId == serviceId);
                var tasks = list.Select(x => x.StepTaskComponentId);
                data = data.Where(x => !tasks.Contains(x.Id)).ToList();
            }
            if (taskId.IsNotNullAndNotEmpty())
            {
                var task = await _taskBusiness.GetSingleById(taskId);
                var component = await _stepTaskComponentBusiness.GetSingleById(task.StepTaskComponentId);
                if (component != null && component.EnableServiceComplete)
                {
                    var complete = new StepTaskComponentViewModel { Id = "SERVICE_COMPLETE", Subject = "Complete Service" };
                    data.Add(complete);
                }
                else
                {
                    var c = model.Max(x => x.SequenceOrder);
                    var step = model.Where(x => x.SequenceOrder == c).FirstOrDefault();
                    if (step != null && step.Id == task.StepTaskComponentId)
                    {
                        var complete = new StepTaskComponentViewModel { Id = "SERVICE_COMPLETE", Subject = "Complete Service" };
                        data.Add(complete);
                    }
                }

            }
            // var dsResult = data.ToDataSourceResult(request);
            return Json(data);
        }
        public async Task<ActionResult> GetStepTaskParents(string templateId, string componentId)
        {
            var data = await _stepTaskComponentBusiness.GetStepTaskParentList(templateId);
            var parent = new IdNameViewModel();
            if (data.IsNotNull() && data.Count() > 0)
            {
                foreach (var item in data)
                {
                    if (item.Code == null)// Code contains ParentId
                    {
                        var comp1 = await _componentBusiness.GetSingleById(item.Id);
                        if (comp1 != null && comp1.ComponentType == ProcessDesignComponentTypeEnum.Start)
                        {
                            var template = await _templateBusiness.GetSingleById(templateId);
                            parent = new IdNameViewModel()
                            {
                                Name = template.DisplayName,
                                Id = item.Id,
                            };
                        }
                    }
                    else
                    {
                        var comp = await _componentBusiness.GetSingleById(item.Code);
                        if (comp != null && comp.ComponentType == ProcessDesignComponentTypeEnum.Start)
                        {
                            var template = await _templateBusiness.GetSingleById(templateId);
                            parent = new IdNameViewModel()
                            {
                                Name = template.DisplayName,
                                Id = item.Code,
                            };
                        }
                    }

                }
            }
            else
            {
                var processDesign = await _processDesignBusiness.GetSingle(x => x.TemplateId == templateId);
                var comp1 = await _componentBusiness.GetSingle(x => x.ComponentType == ProcessDesignComponentTypeEnum.Start && x.ProcessDesignId == processDesign.Id);
                if (comp1 != null)
                {
                    var template = await _templateBusiness.GetSingleById(templateId);
                    parent = new IdNameViewModel()
                    {
                        Name = template.DisplayName,
                        Id = comp1.Id,
                    };
                }
            }
            if (parent.IsNotNull())
            {
                data.Add(parent);
            }
            data = data.Where(x => x.Id != componentId).ToList();
            return Json(data);
        }
        public async Task<JsonResult> ReadStepTaskAssigneeLogicData(string stepTaskComponentId)
        {
            // var stepTaskComponent = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == ComponentId);
            var list = await _processDesignVariableBusiness.GetList<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(x => x.StepTaskComponentId == stepTaskComponentId);
            foreach (var data in list)
            {
                var user = await _processDesignVariableBusiness.GetSingleById<UserViewModel, User>(data.AssignedToUserId);
                if (user != null)
                {
                    data.Assignee = user.Name;
                }

                var lov = await _lovBusiness.GetSingleById(data.AssignedToTypeId);
                if (lov != null)
                {
                    data.AssignedToType = lov.Name;
                }

            }
            return Json(list);
        }
        public async Task<JsonResult> ReadStepTaskSkipLogicData(string stepTaskComponentId)
        {
            // var stepTaskComponent = await _stepTaskComponentBusiness.GetSingle(x => x.ComponentId == ComponentId);
            var list = await _processDesignVariableBusiness.GetList<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(x => x.StepTaskComponentId == stepTaskComponentId);

            return Json(list);
        }

        public async Task<JsonResult> ReadStepTaskEscalation(string stepTaskComponentId, string id)
        {
            var list = new List<StepTaskEscalationViewModel>();
            if (id.IsNotNullAndNotEmpty())
            {
                list = await _processDesignVariableBusiness.GetList<StepTaskEscalationViewModel, StepTaskEscalation>(x => x.StepTaskComponentId == stepTaskComponentId && x.Id != id);

            }
            else
            {
                list = await _stepTaskEscalationBusiness.GetStepTaskEscalation(stepTaskComponentId);

            }

            return Json(list);
        }


        public async Task<ActionResult> ManageCustomAssignee(string id, string stepTaskComponentId, string parentId, string templateId)
        {
            var model = new StepTaskAssigneeLogicViewModel();
            if (id.IsNotNullAndNotEmpty())
            {


                var node = await _processDesignVariableBusiness.GetSingleById<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(id);
                if (node != null)
                {
                    var AssignToTypeCode = await _lovBusiness.GetSingleById(node.AssignedToTypeId);
                    model = new StepTaskAssigneeLogicViewModel
                    {
                        Id = node.Id,
                        Name = node.Name,
                        ExecutionLogic = node.ExecutionLogic,
                        ExecutionLogicDisplay = node.ExecutionLogicDisplay,
                        SequenceOrder = node.SequenceOrder,
                        SuccessResult = node.SuccessResult,
                        AssignedToHierarchyMasterId = node.AssignedToHierarchyMasterId,
                        AssignedToHierarchyMasterLevelId = node.AssignedToHierarchyMasterLevelId,
                        AssignedToTeamId = node.AssignedToTeamId,
                        AssignedToTypeId = node.AssignedToTypeId,
                        AssignedToUserId = node.AssignedToUserId,
                        StepTaskComponentId = stepTaskComponentId,
                        AssignedToType = AssignToTypeCode.Code,
                        ParentId = parentId,
                        TemplateId = templateId,
                        DataAction = DataActionEnum.Edit,
                    };
                }
                return View(model);
            }
            else
            {

                model = new StepTaskAssigneeLogicViewModel
                {
                    Id = Guid.NewGuid().ToString(),

                    StepTaskComponentId = stepTaskComponentId,
                    ParentId = parentId,
                    TemplateId = templateId,
                    DataAction = DataActionEnum.Create
                };

                return View(model);


            }
        }

        [HttpPost]
        public async Task<ActionResult> ManageCustomAssignee(StepTaskAssigneeLogicViewModel model)
        {
            if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _processDesignVariableBusiness.Edit<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                // Create Decision
                var result = await _processDesignVariableBusiness.Create<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }

            return Json(new { success = false });
        }


        public async Task<ActionResult> ManageStepTaskEscalation(string id, string stepTaskComponentId, string parentId, string templateId)
        {
            var model = new StepTaskEscalationViewModel();
            if (id.IsNotNullAndNotEmpty())
            {


                var node = await _stepTaskEscalationBusiness.GetSingleById(id);
                if (node != null)
                {
                    var AssignToTypeCode = await _lovBusiness.GetSingleById(node.AssignedToTypeId);
                    model = new StepTaskEscalationViewModel
                    {
                        Id = node.Id,
                        Name = node.Name,
                        SequenceOrder = node.SequenceOrder,
                        AssignedToHierarchyMasterId = node.AssignedToHierarchyMasterId,
                        AssignedToHierarchyMasterLevelId = node.AssignedToHierarchyMasterLevelId,
                        AssignedToTeamId = node.AssignedToTeamId,
                        AssignedToTypeId = node.AssignedToTypeId,
                        AssignedToUserId = node.AssignedToUserId,
                        StepTaskComponentId = stepTaskComponentId,
                        AssignedToType = AssignToTypeCode.Code,
                        ParentId = parentId,
                        TemplateId = templateId,
                        NewPriorityId = node.NewPriorityId,
                        NotificationTemplateId = node.NotificationTemplateId,
                        TriggerDaysAfterOverDue = node.TriggerDaysAfterOverDue,
                        StepTaskEscalationType = node.StepTaskEscalationType,
                        DataAction = DataActionEnum.Edit,
                        ParentStepTaskEscalationId = node.ParentStepTaskEscalationId,
                        EscalatedToNotificationTemplateId = node.EscalatedToNotificationTemplateId
                    };
                }
                return View(model);
            }
            else
            {

                model = new StepTaskEscalationViewModel
                {
                    Id = Guid.NewGuid().ToString(),

                    StepTaskComponentId = stepTaskComponentId,
                    ParentId = parentId,
                    TemplateId = templateId,
                    DataAction = DataActionEnum.Create
                };

                return View(model);


            }
        }

        [HttpPost]
        public async Task<ActionResult> ManageStepTaskEscalation(StepTaskEscalationViewModel model)
        {
            if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _stepTaskEscalationBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                // Create Decision
                var result = await _stepTaskEscalationBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }

            return Json(new { success = false });
        }

        public async Task<ActionResult> ManageStepTaskSkipLogic(string id, string stepTaskComponentId, string parentId, string templateId)
        {
            var model = new StepTaskSkipLogicViewModel();
            if (id.IsNotNullAndNotEmpty())
            {


                var node = await _processDesignVariableBusiness.GetSingleById<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(id);
                if (node != null)
                {
                    model = node;
                    // model = new StepTaskSkipLogicViewModel
                    // {
                    //Id = node.Id,
                    //Name = node.Name,
                    //StepTaskComponentId = stepTaskComponentId,
                    model.ParentId = parentId;
                    model.TemplateId = templateId;
                    model.DataAction = DataActionEnum.Edit;
                    // };
                }
                return View(model);
            }
            else
            {

                model = new StepTaskSkipLogicViewModel
                {
                    Id = Guid.NewGuid().ToString(),

                    StepTaskComponentId = stepTaskComponentId,
                    ParentId = parentId,
                    TemplateId = templateId,
                    DataAction = DataActionEnum.Create
                };

                return View(model);


            }
        }

        [HttpPost]
        public async Task<ActionResult> ManageStepTaskSkipLogic(StepTaskSkipLogicViewModel model)
        {
            if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _processDesignVariableBusiness.Edit<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                // Create Decision
                var result = await _processDesignVariableBusiness.Create<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }


        }
        public async Task<IActionResult> OnDeleteStepTaskAssigneeLogic(string Id)
        {
            var node = await _processDesignVariableBusiness.GetSingleById<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(Id);
            if (node != null)
            {
                await _processDesignVariableBusiness.Delete<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(Id);

            }
            return Json(true);
        }

        public async Task<IActionResult> OnDeleteStepTaskEscalation(string Id)
        {
            var node = await _processDesignVariableBusiness.GetSingleById<StepTaskEscalationViewModel, StepTaskEscalation>(Id);
            if (node != null)
            {
                await _processDesignVariableBusiness.Delete<StepTaskEscalationViewModel, StepTaskEscalation>(Id);

            }
            return Json(true);
        }
        public async Task<IActionResult> OnDeleteStepTaskSkipLogic(string Id)
        {
            var node = await _processDesignVariableBusiness.GetSingleById<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(Id);
            if (node != null)
            {
                await _processDesignVariableBusiness.Delete<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(Id);

            }
            return Json(true);
        }

        public async Task<IActionResult> RunTimeWorkflow(string templateId = null, string serviceId = null, string taskId = null, DataActionEnum dataAction = DataActionEnum.Create)
        {
            var exist = await _runtimeWorkflowBusiness.GetSingle(x => x.RuntimeWorkflowSourceTemplateId == templateId
            && x.SourceServiceId == serviceId && x.SourceTaskId == taskId);
            //var exist = await _runtimeWorkflowBusiness.GetSingle(x => x.RuntimeWorkflowSourceTemplateId == templateId);
            if (exist == null)
            {
                exist = new RuntimeWorkflowViewModel()
                {
                    RuntimeWorkflowSourceTemplateId = templateId,
                    SourceServiceId = serviceId,
                    SourceTaskId = taskId,
                };
                var result = await _runtimeWorkflowBusiness.Create(exist);
                exist = result.Item;
            }
            exist.DataAction = DataActionEnum.Edit;
            return View(exist);
        }

        public async Task<IActionResult> GetTriggeringStepTaskComponentList(string templateId)
        {
            var data = await _processDesignBusiness.GetTriggeringStepTaskComponentList(templateId);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRuntimeWorkflow(RuntimeWorkflowViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var runtimeWorkflow = await _stepTaskComponentBusiness.GetSingle(x => x.Id == model.TriggeringStepTaskComponentId);
                    if (runtimeWorkflow.IsNotNull())
                    {
                        model.TriggeringTemplateId = runtimeWorkflow.TemplateId;
                        model.TriggeringComponentId = runtimeWorkflow.ComponentId;
                    }

                    var result = await _runtimeWorkflowBusiness.Create(model);
                    return Json(new { success = result.IsSuccess, error=result.Messages });
                    //if (result.IsSuccess)
                    //{
                    //    //return Json(new { success = true, Id = result.Item.Id });
                    //    if (model.WorkFlowData.IsNotNull())
                    //    {
                    //        try
                    //        {
                    //            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(model.WorkFlowData);
                    //            foreach (var data in rowData)
                    //            {
                    //                data["AssignedToHierarchyMasterLevelId"] = data["AssignedToHierarchyMasterLevelId"] != null ? data["AssignedToHierarchyMasterLevelId"] : 0;
                    //                data["TeamAssignmentType"] = data["TeamAssignmentType"] != null ? data["TeamAssignmentType"] : 0;
                    //            }
                    //            model.WorkFlowData = JsonConvert.SerializeObject(rowData);
                    //            var workFlowData = JsonConvert.DeserializeObject<List<RuntimeWorkflowDataViewModel>>(model.WorkFlowData);
                    //            foreach (var a in workFlowData)
                    //            {
                    //                a.RuntimeWorkflowId = result.Item.Id;
                    //                await _runtimeWorkflowDataBusiness.Create(a);
                    //            }
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            throw;
                    //        }
                    //    }
                    //    return Json(new { success = true, Id = result.Item.Id });
                    //}
                    //else
                    //{
                    //    ModelState.AddModelErrors(result.Messages);
                    //    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    //}
                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    var runtimeWorkflow = await _stepTaskComponentBusiness.GetSingle(x => x.Id == model.TriggeringStepTaskComponentId);

                    if (runtimeWorkflow.IsNotNull())
                    {
                        model.TriggeringTemplateId = runtimeWorkflow.TemplateId;
                        model.TriggeringComponentId = runtimeWorkflow.ComponentId;
                    }

                    var result = await _runtimeWorkflowBusiness.Edit(model);

                    //if (result.IsSuccess)
                    //{
                    //    var workFlowData = JsonConvert.DeserializeObject<List<RuntimeWorkflowDataViewModel>>(model.WorkFlowData);
                    //    var createList = workFlowData.Where(x => x.DataAction == DataActionEnum.Create).ToList();
                    //    var editList = workFlowData.Where(x => x.DataAction != DataActionEnum.Create).ToList();
                    //    foreach (var c in createList)
                    //    {
                    //        var lov = await _lovBusiness.GetSingle(x => x.Code == c.AssignedToTypeId);
                    //        c.RuntimeWorkflowId = model.Id;
                    //        c.Id = "";
                    //        c.AssignedToTypeId = lov.Id;
                    //        await _runtimeWorkflowDataBusiness.Create(c);
                    //    }
                    //    foreach (var e in editList)
                    //    {
                    //        if (e.AssignedToTypeId.Contains("TASK_ASSIGN"))
                    //        {
                    //            var lov = await _lovBusiness.GetSingle(x => x.Code == e.AssignedToTypeId);
                    //            e.AssignedToTypeId = lov.Id;
                    //        }
                    //        e.RuntimeWorkflowId = model.Id;
                    //        await _runtimeWorkflowDataBusiness.Edit(e);
                    //    }
                    //    //return Json(new { success = true, Id = result.Item.Id });
                    //}
                    //else
                    //{
                    //    ModelState.AddModelErrors(result.Messages);
                    //    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                    //}
                    return Json(new { success = result.IsSuccess, error = result.Messages });
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> ManageRuntimeWorkflowData(RuntimeWorkflowDataViewModel model)
        {
            if(model.DataAction == DataActionEnum.Create)
            {                
                var lov = await _lovBusiness.GetSingle(x => x.Code == model.AssignedToTypeId);
                model.AssignedToTypeId = lov.Id;

                var rfwData = new RuntimeWorkflowData();
                if(lov.Code == "TASK_ASSIGN_TO_USER")
                {
                    rfwData = await _runtimeWorkflowDataBusiness.GetSingle(x => x.RuntimeWorkflowId == model.RuntimeWorkflowId && x.AssignedToTypeId == lov.Id && x.AssignedToUserId == model.AssignedToUserId);
                }
                else if(lov.Code== "TASK_ASSIGN_TO_TEAM")
                {
                    rfwData = await _runtimeWorkflowDataBusiness.GetSingle(x => x.RuntimeWorkflowId == model.RuntimeWorkflowId && x.AssignedToTypeId == lov.Id && x.TeamAssignmentType == model.TeamAssignmentType && x.AssignedToTeamId == model.AssignedToTeamId && x.AssignedToUserId == model.AssignedToUserId);
                }
                else if(lov.Code == "TASK_ASSIGN_TO_USER_HIERARCHY")
                {
                    rfwData = await _runtimeWorkflowDataBusiness.GetSingle(x => x.RuntimeWorkflowId == model.RuntimeWorkflowId && x.AssignedToTypeId == lov.Id && x.AssignedToHierarchyMasterId == model.AssignedToHierarchyMasterId && x.AssignedToHierarchyMasterLevelId == model.AssignedToHierarchyMasterLevelId);
                }

                if (rfwData.IsNotNull())
                {
                    return Json(new { success = false, error = "Record already exist" });
                }
                else
                {
                    var res = await _runtimeWorkflowDataBusiness.Create(model);
                    return Json(new { success = res.IsSuccess, error = res.Messages });
                }               
            }
            else
            {
                var rfwData = new RuntimeWorkflowData();
                if (model.AssignedToTypeId.Contains("TASK_ASSIGN"))
                {
                    var lov = await _lovBusiness.GetSingle(x => x.Code == model.AssignedToTypeId);
                    model.AssignedToTypeId = lov.Id;
                    model.AssignedToTypeCode = lov.Code;
                }
                else
                {
                    var lov = await _lovBusiness.GetSingle(x => x.Id == model.AssignedToTypeId);                    
                    model.AssignedToTypeCode = lov.Code;
                }
                if (model.AssignedToTypeCode == "TASK_ASSIGN_TO_USER")
                {
                    rfwData = await _runtimeWorkflowDataBusiness.GetSingle(x => x.RuntimeWorkflowId == model.RuntimeWorkflowId && x.AssignedToTypeId == model.AssignedToTypeId && x.AssignedToUserId == model.AssignedToUserId && x.Id != model.Id);
                }
                else if (model.AssignedToTypeCode == "TASK_ASSIGN_TO_TEAM")
                {
                    rfwData = await _runtimeWorkflowDataBusiness.GetSingle(x => x.RuntimeWorkflowId == model.RuntimeWorkflowId && x.AssignedToTypeId == model.AssignedToTypeId && x.TeamAssignmentType == model.TeamAssignmentType && x.AssignedToTeamId == model.AssignedToTeamId && x.AssignedToUserId == model.AssignedToUserId && x.Id != model.Id);
                }
                else if (model.AssignedToTypeCode == "TASK_ASSIGN_TO_USER_HIERARCHY")
                {
                    rfwData = await _runtimeWorkflowDataBusiness.GetSingle(x => x.RuntimeWorkflowId == model.RuntimeWorkflowId && x.AssignedToTypeId == model.AssignedToTypeId && x.AssignedToHierarchyMasterId == model.AssignedToHierarchyMasterId && x.AssignedToHierarchyMasterLevelId == model.AssignedToHierarchyMasterLevelId && x.Id != model.Id);
                }

                if (rfwData.IsNotNull())
                {
                    return Json(new { success = false, error = "Record already exist" });
                }
                else
                {
                    var res = await _runtimeWorkflowDataBusiness.Edit(model);
                    return Json(new { success = res.IsSuccess, error = res.Messages });
                }
            }
        }

        public async Task<IActionResult> GetRuntimeWorkflowDataList(string runtimeWorkflowDataId)
        {
            var data = new List<RuntimeWorkflowDataViewModel>();
            if (runtimeWorkflowDataId.IsNotNullAndNotEmpty())
            {
                //data = await _runtimeWorkflowDataBusiness.GetList(x => x.RuntimeWorkflowId == runtimeWorkflowDataId);
                data = await _runtimeWorkflowDataBusiness.GetRuntimeWorkflowDataList(runtimeWorkflowDataId);
            }
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRuntimeWorkflowData(string Id)
        {
            await _runtimeWorkflowDataBusiness.Delete(Id);
            return Json(new { success = true });
        }

    }
}
