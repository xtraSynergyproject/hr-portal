using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.EJ2.Diagrams;
using Syncfusion.EJ2.Navigations;

namespace CMS.UI.Web.Controllers
{
    [Area("Bre")]
    public class BusinessRuleDiagramController : ApplicationController
    {
        private readonly IBusinessRuleNodeBusiness _breRuleNodeBusiness;
        private readonly IBusinessRuleConnectorBusiness _breRuleConnectorBusiness;
        private readonly IBusinessAreaBusiness _breAreaBusiness;
        private readonly IBusinessSectionBusiness _breSectionBusiness;
        private readonly IBusinessRuleGroupBusiness _breRuleGroupBusiness;
        private readonly IBusinessRuleBusiness _breRuleBusiness;
        private readonly IBreMetadataBusiness _breMetadataBusiness;
        private readonly IProcessDesignBusiness _processDesignBusiness;
        private readonly IComponentBusiness _componentBusiness;
        private readonly IMapper _autoMapper;
        public BusinessRuleDiagramController(IBusinessRuleNodeBusiness breRuleNodeBusiness, IBusinessAreaBusiness breAreaBusiness, IBusinessRuleGroupBusiness breRuleGroupBusiness, IBusinessSectionBusiness breSectionBusiness
            , IBusinessRuleConnectorBusiness breRuleConnectorBusiness, IMapper autoMapper, IBusinessRuleBusiness breRuleBusiness, IBreMetadataBusiness breMetadataBusiness,
            IProcessDesignBusiness processDesignBusiness
            , IComponentBusiness componentBusiness)
        {
            _breRuleNodeBusiness = breRuleNodeBusiness;
            _breAreaBusiness = breAreaBusiness;
            _breRuleGroupBusiness = breRuleGroupBusiness;
            _breSectionBusiness = breSectionBusiness;
            _breRuleConnectorBusiness = breRuleConnectorBusiness;
            _breRuleBusiness = breRuleBusiness;
            _autoMapper = autoMapper;
            _breMetadataBusiness = breMetadataBusiness;
            _processDesignBusiness = processDesignBusiness;
            _componentBusiness = componentBusiness;
        }



        public async Task<JsonResult> GetDiagramData(string ruleId)
        {
            var data = await _breRuleBusiness.GetDiagramDataByRuleId(ruleId);
            return Json(data);
        }
        public async Task<IActionResult> BusinessFlowDiagram(string ruleId, string templateId)
        {
            BusinessRuleNodeViewModel model = new BusinessRuleNodeViewModel();
            model.TemplateId = templateId;
            double[] intervals = { 1, 9, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75, 0.25, 9.75 };
            DiagramGridlines grIdLines = new DiagramGridlines()
            { LineColor = "#e0e0e0", LineIntervals = intervals };
            ViewBag.gridLines = grIdLines;
            DiagramMargin margin = new DiagramMargin() { Left = 15, Right = 15, Bottom = 15, Top = 15 };
            ViewBag.margin = margin;
            ViewBag.getNodeDefaults = "getNodeDefaults";
            ViewBag.getConnectorDefaults = "getConnectorDefaults";
            var diagram = await _breRuleBusiness.GetDiagramDataByRuleId(ruleId);
            if (diagram != null)
            {
                //model.Id = diagram.Id;
                if (diagram.DiagramData != null && diagram.DiagramData != "")
                {
                    ViewBag.DiagramData = diagram.DiagramData;
                }
            }
            model.BusinessRuleId = ruleId;
            ViewBag.Connectors = await _breRuleConnectorBusiness.GetConnector(ruleId);
            List<ContextMenuItem> contextMenuItemModels = new List<ContextMenuItem>()
{
        new ContextMenuItem()
        {
            Text ="Create Node",
            Id="Node",
            Items = new List<ContextMenuItem>()
{
                new ContextMenuItem(){  Text ="Decision", Id="Decision1", },
                new ContextMenuItem(){  Text ="Process", Id="Process1", },
                new ContextMenuItem(){  Text ="End", Id="Terminator1", }
            }
        },
         new ContextMenuItem()
        {
            Text ="Remove Node",
            Id="RemoveNode",

        },
        new ContextMenuItem()
        {
            Text ="If Yes, Create",
            Id="IfNode",

            Items = new List<ContextMenuItem>()
{
                new ContextMenuItem(){  Text ="Decision", Id="Decision2", },
                new ContextMenuItem(){  Text ="Process", Id="Process2", },
                new ContextMenuItem(){  Text ="End", Id="Terminator2", }
            }
        },
        new ContextMenuItem()
        {
            Text ="If No, Create",
            Id="ElseNode",

            Items = new List<ContextMenuItem>()
{
                new ContextMenuItem(){  Text ="Decision", Id="Decision3", },
                new ContextMenuItem(){  Text ="Process", Id="Process3", },
                new ContextMenuItem(){  Text ="End", Id="Terminator3", }
            }
        },
        new ContextMenuItem()
        {
            Text ="View Detail",
            Id="View",
        },

    };
            ViewBag.contextMenuItems = contextMenuItemModels;



            List<DiagramCommand> commands = new List<DiagramCommand>();
            commands.Add(new DiagramCommand() { Name = "delete", CanExecute = "canExecuteDelete", Execute = "executeDelete", Gesture = new DiagramKeyGesture() { Key = Keys.Delete, KeyModifiers = KeyModifiers.None } });
            commands.Add(new DiagramCommand() { Name = "copy", CanExecute = "canExecuteDelete", Execute = "executeDelete", Gesture = new DiagramKeyGesture() { Key = Keys.C, KeyModifiers = KeyModifiers.Control } });
            commands.Add(new DiagramCommand() { Name = "paste", CanExecute = "canExecuteDelete", Execute = "executeDelete", Gesture = new DiagramKeyGesture() { Key = Keys.V, KeyModifiers = KeyModifiers.Control } });
            commands.Add(new DiagramCommand() { Name = "cut", CanExecute = "canExecuteDelete", Execute = "executeDelete", Gesture = new DiagramKeyGesture() { Key = Keys.X, KeyModifiers = KeyModifiers.Control } });
            //commands.Add(new DiagramCommand() { Name = "navigationLeft", CanExecute = "canExecuteNavigation", Execute = "executeNavigationLeft", Gesture = new DiagramKeyGesture() { Key = Keys.Left } });
            //commands.Add(new DiagramCommand() { Name = "navigationRight", CanExecute = "canExecuteNavigation", Execute = "executeNavigationRight", Gesture = new DiagramKeyGesture() { Key = Keys.Right } });
            ViewBag.commands = commands;


            return View(model);
        }
        public ActionResult GetRandomId()
        {
            var data = ObjectId.GenerateNewId().ToString();
            return Json(data);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BusinessRuleViewModel model)
        {
            try
            {
                var data = JObject.Parse(model.DiagramData);
                var nodes = data["nodes"];
                var connectors = data["connectors"];
                var newNodes = nodes.ToObject<DiagramNode[]>();
                var newConnectors = connectors.ToObject<DiagramConnector[]>();
                if (model.Id != null)
                {
                    var data1 = await _breRuleBusiness.GetSingleById(model.Id);
                    if (data1.DiagramData != null)
                    {
                        var DiagramData = JObject.Parse(data1.DiagramData);
                        var nodes1 = DiagramData["nodes"];
                        var connectors1 = DiagramData["connectors"];
                        #region manage already existing newly added and removed nodes when diagram data is already available
                        var existingNodes = nodes1.ToObject<DiagramNode[]>();
                        var existingNodeIds = existingNodes.Select(x => x.Id);
                        var newNodeIds = newNodes.Select(x => x.Id);
                        var Total = existingNodeIds.Union(newNodeIds);
                        var NodesRemoved = Total.Except(newNodeIds).ToList();
                        var NodesAdded = Total.Except(existingNodeIds).ToList();
                        var NodesToUpdate = newNodeIds.Intersect(existingNodeIds).ToList();
                        // Removed the Nodes which are removed
                        foreach (var Id in NodesRemoved)
                        {
                            await _breRuleNodeBusiness.Delete(Id);
                        }
                        // Add the Nodes which are newly added
                        foreach (var Id in NodesAdded)
                        {
                            var node = newNodes.FirstOrDefault(x => x.Id == Id);
                            var nodeData = node.Shape.ToString();
                            var shape = JObject.Parse(nodeData);
                            var type = shape["shape"];
                            BusinessRuleNodeViewModel nodeModel = new BusinessRuleNodeViewModel();
                            nodeModel.Name = node.Annotations[0].Content;
                            nodeModel.BusinessRuleId = model.Id;
                            nodeModel.Id = Id;
                            nodeModel.Type = type.ToObject<FlowShapes>();
                            if (nodeModel.Name == "Start" && nodeModel.Type == FlowShapes.Terminator)
                            {
                                nodeModel.IsStarter = true;
                            }
                            await _breRuleNodeBusiness.Create(nodeModel);
                        }
                        // Update the existing Nodes
                        foreach (var Id in NodesToUpdate)
                        {
                            var node = newNodes.FirstOrDefault(x => x.Id == Id);
                            var nodeData = node.Shape.ToString();
                            var shape = JObject.Parse(nodeData);
                            var type = shape["shape"];
                            var existingnode = await _breRuleNodeBusiness.GetSingleById(Id);
                            if (existingnode != null)
                            {
                                existingnode.Name = node.Annotations[0].Content;
                                existingnode.BusinessRuleId = model.Id;
                                existingnode.Type = type.ToObject<FlowShapes>();
                                if (existingnode.Name == "Start" && existingnode.Type == FlowShapes.Terminator)
                                {
                                    existingnode.IsStarter = true;
                                }
                                await _breRuleNodeBusiness.Edit(existingnode);
                            }
                        }
                        #endregion

                        #region manage already existing connectors and newly added connectors
                        var existingConnectors = connectors1.ToObject<DiagramConnector[]>();
                        var existingConnectorIds = existingConnectors.Select(x => x.Id);
                        var newConnectorIds = newConnectors.Select(x => x.Id);
                        var Connectors = existingConnectorIds.Union(newConnectorIds);
                        var ConnectorsRemoved = Connectors.Except(newConnectorIds).ToList();
                        var ConnectorsAdded = Connectors.Except(existingConnectorIds).ToList();
                        var ConnectorsToUpdate = newConnectorIds.Intersect(existingConnectorIds).ToList();
                        // Removed the Connectors which are removed
                        foreach (var Id in ConnectorsRemoved)
                        {
                            await _breRuleConnectorBusiness.Delete(Id);
                        }
                        // Add the Connectors which are newly added
                        foreach (var Id in ConnectorsAdded)
                        {
                            var connector = newConnectors.FirstOrDefault(x => x.Id == Id);
                            BusinessRuleConnectorViewModel connectorModel = new BusinessRuleConnectorViewModel();
                            if (connector.Annotations.Count > 0)
                            {
                                connectorModel.Name = connector.Annotations[0].Content;
                                if (connectorModel.Name == "Yes")
                                {
                                    connectorModel.IsForTrue = true;
                                }
                                else
                                {
                                    connectorModel.IsForTrue = false;
                                }
                            }
                            else
                            {
                                connectorModel.Name = "";
                            }
                            connectorModel.Id = Id;
                            connectorModel.BusinessRuleId = model.Id;
                            connectorModel.SourceId = connector.SourceID;
                            connectorModel.TargetId = connector.TargetID;
                            await _breRuleConnectorBusiness.Create(connectorModel);
                        }
                        // Update the existing Connectors
                        foreach (var Id in ConnectorsToUpdate)
                        {
                            var connector = newConnectors.FirstOrDefault(x => x.Id == Id);
                            var existingconnector = await _breRuleConnectorBusiness.GetSingleById(Id);
                            if (existingconnector != null)
                            {
                                if (connector.Annotations.Count > 0)
                                {
                                    existingconnector.Name = connector.Annotations[0].Content;
                                    if (existingconnector.Name == "Yes")
                                    {
                                        existingconnector.IsForTrue = true;
                                    }
                                    else
                                    {
                                        existingconnector.IsForTrue = false;
                                    }
                                }
                                else
                                {
                                    existingconnector.Name = "";
                                }
                                existingconnector.BusinessRuleId = model.Id;
                                existingconnector.SourceId = connector.SourceID;
                                existingconnector.TargetId = connector.TargetID;
                                await _breRuleConnectorBusiness.Edit(existingconnector);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region save nodes and connectors separately
                        foreach (var node in newNodes)
                        {
                            var nodeData = node.Shape.ToString();
                            var shape = JObject.Parse(nodeData);
                            var type = shape["shape"];
                            var existingnode = await _breRuleNodeBusiness.GetSingleById(node.Id);
                            if (existingnode != null)
                            {
                                existingnode.Name = node.Annotations[0].Content;
                                existingnode.BusinessRuleId = model.Id;
                                //existingnode.Id = node.Id;
                                existingnode.Type = type.ToObject<FlowShapes>();
                                if (existingnode.Name == "Start" && existingnode.Type == FlowShapes.Terminator)
                                {
                                    existingnode.IsStarter = true;
                                }
                                await _breRuleNodeBusiness.Edit(existingnode);
                            }
                            else
                            {
                                BusinessRuleNodeViewModel nodeModel = new BusinessRuleNodeViewModel();
                                nodeModel.Name = node.Annotations[0].Content;
                                nodeModel.BusinessRuleId = model.Id;
                                nodeModel.Id = node.Id;
                                nodeModel.Type = type.ToObject<FlowShapes>();
                                if (nodeModel.Name == "Start" && nodeModel.Type == FlowShapes.Terminator)
                                {
                                    nodeModel.IsStarter = true;
                                }
                                await _breRuleNodeBusiness.Create(nodeModel);
                            }
                        }
                        foreach (var connector in newConnectors)
                        {
                            var existingconnector = await _breRuleConnectorBusiness.GetSingleById(connector.Id);
                            if (existingconnector != null)
                            {
                                if (connector.Annotations.Count > 0)
                                {
                                    existingconnector.Name = connector.Annotations[0].Content;
                                    if (existingconnector.Name == "Yes")
                                    {
                                        existingconnector.IsForTrue = true;
                                    }
                                    else
                                    {
                                        existingconnector.IsForTrue = false;
                                    }
                                }
                                else
                                {
                                    existingconnector.Name = "";
                                }
                                existingconnector.BusinessRuleId = model.Id;
                                existingconnector.SourceId = connector.SourceID;
                                existingconnector.TargetId = connector.TargetID;
                                await _breRuleConnectorBusiness.Edit(existingconnector);
                            }
                            else
                            {
                                BusinessRuleConnectorViewModel connectorModel = new BusinessRuleConnectorViewModel();
                                if (connector.Annotations.Count > 0)
                                {
                                    connectorModel.Name = connector.Annotations[0].Content;
                                    if (connectorModel.Name == "Yes")
                                    {
                                        connectorModel.IsForTrue = true;
                                    }
                                    else
                                    {
                                        connectorModel.IsForTrue = false;
                                    }
                                }
                                else
                                {
                                    connectorModel.Name = "";
                                }
                                connectorModel.Id = connector.Id;
                                connectorModel.BusinessRuleId = model.Id;
                                connectorModel.SourceId = connector.SourceID;
                                connectorModel.TargetId = connector.TargetID;
                                await _breRuleConnectorBusiness.Create(connectorModel);
                            }
                        }
                        #endregion
                    }
                    data1.DiagramData = model.DiagramData;
                    var result = await _breRuleBusiness.Edit(data1);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }

        public async Task<IActionResult> BusinessRuleTestPage(string ruleId, string templateId)
        {
            ViewBag.ruleId = ruleId;
            ViewBag.templateId = templateId;
            return View();
        }

        public async Task<JsonResult> GetInputJsonData(string data)
        {
            // var data = await _breMetadataBusiness.GetList(x=>x.BreMetadataType==BreMetadataTypeEnum.InputData && x.BusinessRuleId==ruleId);
            //var result=System.Text.Json.JsonSerializer.Serialize(data[0]);            
            return Json(data);
        }

        public async Task<ActionResult> CreateNode(BusinessRuleNodeViewModel model)
        {
            try
            {
                model.DataAction = DataActionEnum.Create;
                var result = await _breRuleNodeBusiness.Create(model);
                if (result.IsSuccess)
                {
                    var connectorModel = new BusinessRuleConnectorViewModel();
                    connectorModel.BusinessRuleId = model.BusinessRuleId;
                    connectorModel.SourceId = model.SourceId;
                    connectorModel.TargetId = result.Item.Id;
                    await _breRuleConnectorBusiness.Create(connectorModel);

                    return Json(new { success = true, nodeId = result.Item.Id });
                }
                else
                {
                    return Json(new { success = false });
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }

        public async Task<ActionResult> CreateBusinessDiagramNode(BusinessRuleNodeViewModel model)
        {
            // Create Decision
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _breRuleNodeBusiness.Create(model);
                if (result.IsSuccess)
                {
                    // Create Connector with parent
                    BusinessRuleConnectorViewModel connectorModel = new BusinessRuleConnectorViewModel();
                    connectorModel.Name = "";
                    connectorModel.Id = Guid.NewGuid().ToString();
                    connectorModel.BusinessRuleId = model.BusinessRuleId;
                    connectorModel.SourceId = model.ParentNodeId;
                    connectorModel.TargetId = result.Item.Id;
                    await _breRuleConnectorBusiness.Create(connectorModel);
                    if (model.Type == FlowShapes.Decision)
                    {
                        // Create Left Child
                        BusinessRuleNodeViewModel left = new BusinessRuleNodeViewModel();
                        left.BusinessRuleId = model.BusinessRuleId;
                        left.ParentNodeId = result.Item.Id;
                        left.Type = FlowShapes.Document;
                        left.Id = Guid.NewGuid().ToString();
                        left.Name = "False";
                        left.IsStarter = false;
                        left.Isleft = true;
                        left.Script = model.Script;
                        left.BusinessRuleLogicType = model.BusinessRuleLogicType;
                        await CreateDecisionChildNode(left);
                        // Create Right Child
                        BusinessRuleNodeViewModel right = new BusinessRuleNodeViewModel();
                        right.BusinessRuleId = model.BusinessRuleId;
                        right.ParentNodeId = result.Item.Id;
                        right.Type = FlowShapes.Document;
                        right.Id = Guid.NewGuid().ToString();
                        right.Name = "True";
                        right.IsStarter = false;
                        right.Script = model.Script;
                        right.BusinessRuleLogicType = model.BusinessRuleLogicType;
                        await CreateDecisionChildNode(right);
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            else  
            {
                var node = await _breRuleNodeBusiness.GetSingleById(model.Id);
                node.Name = model.Name;
                var result = await _breRuleNodeBusiness.Edit(node);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }


        public async Task<ActionResult> ManageBusinessDiagramNode(BusinessRuleNodeViewModel model)
        {
            if (model.DataAction == DataActionEnum.Edit)
            {
                var result = await _breRuleNodeBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                } else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                // Create Decision
                var result = await _breRuleNodeBusiness.Create(model);
                if (result.IsSuccess)
                {
                    // Create Connector with parent
                    BusinessRuleConnectorViewModel connectorModel = new BusinessRuleConnectorViewModel();
                    connectorModel.Name = "";
                    connectorModel.Id = Guid.NewGuid().ToString();
                    connectorModel.BusinessRuleId = model.BusinessRuleId;
                    connectorModel.SourceId = model.ParentNodeId;
                    connectorModel.TargetId = result.Item.Id;
                    await _breRuleConnectorBusiness.Create(connectorModel);
                    if (model.Type == FlowShapes.Decision)
                    {
                        // Create Left Child
                        BusinessRuleNodeViewModel left = new BusinessRuleNodeViewModel();
                        left.BusinessRuleId = model.BusinessRuleId;
                        left.ParentNodeId = result.Item.Id;
                        left.Type = FlowShapes.Document;
                        left.Id = Guid.NewGuid().ToString();
                        left.Name = "False";
                        left.IsStarter = false;
                        left.Isleft = true;
                        left.Script = model.Script;
                        left.BusinessRuleLogicType = model.BusinessRuleLogicType;
                        await CreateDecisionChildNode(left);
                        // Create Right Child
                        BusinessRuleNodeViewModel right = new BusinessRuleNodeViewModel();
                        right.BusinessRuleId = model.BusinessRuleId;
                        right.ParentNodeId = result.Item.Id;
                        right.Type = FlowShapes.Document;
                        right.Id = Guid.NewGuid().ToString();
                        right.Name = "True";
                        right.IsStarter = false;
                        right.Script = model.Script;
                        right.BusinessRuleLogicType = model.BusinessRuleLogicType;
                        await CreateDecisionChildNode(right);
                    }
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        private async Task CreateDecisionChildNode(BusinessRuleNodeViewModel model)
        {
            var result = await _breRuleNodeBusiness.Create(model);
            if (result.IsSuccess)
            {
                // Create Connector with parent
                BusinessRuleConnectorViewModel connectorModel = new BusinessRuleConnectorViewModel();
                connectorModel.Name = "";
                connectorModel.Id = Guid.NewGuid().ToString();
                connectorModel.BusinessRuleId = model.BusinessRuleId;
                connectorModel.SourceId = model.ParentNodeId;
                connectorModel.TargetId = result.Item.Id;
                if (model.Isleft == true)
                {
                    connectorModel.Name = "No";
                    connectorModel.IsForTrue = false;
                    connectorModel.IsFromDecision = true;
                }
                else
                {
                    connectorModel.Name = "Yes";
                    connectorModel.IsForTrue = true;
                    connectorModel.IsFromDecision = true;
                }
                await _breRuleConnectorBusiness.Create(connectorModel);
            }
        }

        public async Task<ActionResult> RemoveDiagramNode(string templateId,string nodeId,string operation) 
        {
            if (operation=="RemoveWorkflow") 
            {
                // Delete Process Design and All its Childs 
                await _processDesignBusiness.RemoveProcessDesign(templateId);               
            }
            if (operation == "RemoveComponent")
            {
                // Delete Step Task Component its ComponentScript and All its Childs 
                await _componentBusiness.RemoveComponentsAndItsChild(nodeId);
            }
            if (operation == "RemoveBusinessRuleNode")
            {
                // Delete a Node and All its Childs node
                await _breRuleNodeBusiness.RemoveNodeAndItsChild(nodeId);
            }
            if (operation == "RemoveBusinessRule")
            {
                // Delete Business Rule and All its Child
                await _breRuleNodeBusiness.RemoveBusinessRuleAndItsNode(nodeId);
            }
            return Json(new { success=true});
        }
    }
}
