using AutoMapper;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
//using Syncfusion.EJ2.Diagrams;
using System.Linq;
using Newtonsoft.Json.Linq;
using Synergy.App.Common;

namespace Synergy.App.Business
{
    public class BusinessRuleNodeBusiness : BusinessBase<BusinessRuleNodeViewModel, BusinessRuleNode>, IBusinessRuleNodeBusiness
    {
        private readonly IServiceProvider _serviceProvider;
        public BusinessRuleNodeBusiness(IRepositoryBase<BusinessRuleNodeViewModel, BusinessRuleNode> repo
            , IMapper autoMapper
            , IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _serviceProvider = serviceProvider;
        }

        //public async Task<List<DiagramNode>> GetNode(string BussinessRuleId)
        //{
        //    var data = await GetList(x => x.BusinessRuleId == BussinessRuleId);
        //    var model = _autoMapper.Map<List<DiagramNode>>(data);
        //    return model;
        //}
        //public async Task CreateNode(BusinessRuleNodeViewModel model)
        //{           
        //    await _repo.Create(model);
        //}
        //public async Task EditNode(BusinessRuleNodeViewModel model)
        //{
        //    await _repo.Edit(model);
        //}
        public async override Task<CommandResult<BusinessRuleNodeViewModel>> Create(BusinessRuleNodeViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model, autoCommit);
            if (result.IsSuccess)
            {
                var brnBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
                await brnBusiness.ManageOperationValue(result.Item.Id, null, null);
            }
            return result;
        }
        public async override Task<CommandResult<BusinessRuleNodeViewModel>> Edit(BusinessRuleNodeViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model, autoCommit);
            if (result.IsSuccess)
            {
                var brnBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
                await brnBusiness.ManageOperationValue(result.Item.Id, null, null);
            }
            return result;
        }
        public async Task RemoveBusinessRuleAndItsNode(string ruleId)
        {
            // Remove Decision  Component
            var NodesList = await GetList(x => x.BusinessRuleId == ruleId);
            if (NodesList != null && NodesList.Count() > 0)
            {
                foreach (var node in NodesList)
                {
                    // Remove Connectors
                    await RemoveConnectors(node.Id);
                    if (node.Type == FlowShapes.Decision)
                    {
                        // Remove Decision Component
                        await RemoveDecision(node.Id);
                    }
                    else if (node.Type == FlowShapes.Process)
                    {
                        // Remove Process  Component
                        await RemoveProcess(node.Id);
                    }
                    else
                    {
                        // Remove Terminator Or Start
                        await Delete(node.Id);
                    }
                }
            }
            await Delete<BusinessRuleViewModel, BusinessRule>(ruleId);
        }
        public async Task RemoveNodeAndItsChild(string nodeId)
        {
            // Remove Decision  Component
            var list = new List<BusinessRuleNodeViewModel>();
            var NodesList = await GetNodeAndChilds(nodeId, list);
            if (NodesList != null && NodesList.Count() > 0)
            {
                foreach (var node in NodesList)
                {
                    // Remove Connectors
                    await RemoveConnectors(node.Id);
                    if (node.Type == FlowShapes.Decision)
                    {
                        // Remove Decision Component
                        await RemoveDecision(node.Id);
                    }
                    else if (node.Type == FlowShapes.Process)
                    {
                        // Remove Process  Component
                        await RemoveProcess(node.Id);
                    }
                    else
                    {
                        // Remove Terminator Or Start
                        await Delete(node.Id);
                    }
                }
            }
        }
        public async Task RemoveDecision(string nodeId)
        {
            // Remove Decision  Component
            var Decision = await GetSingleById(nodeId);
            if (Decision != null)
            {
                var businessRuleModel = await GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.BusinessRuleNodeId == nodeId);
                foreach (var ruleModel in businessRuleModel)
                {
                    await Delete<BusinessRuleModelViewModel, BusinessRuleModel>(ruleModel.Id);
                }
                await Delete(Decision.Id);
            }

        }
        public async Task RemoveProcess(string nodeId)
        {
            // Remove Process  Component
            var Process = await GetSingleById(nodeId);
            if (Process != null)
            {
                var businessRuleModel = await GetList<BreResultViewModel, BreResult>(x => x.BusinessRuleNodeId == nodeId);
                foreach (var ruleModel in businessRuleModel)
                {
                    await Delete<BreResultViewModel, BreResult>(ruleModel.Id);
                }
                await Delete(Process.Id);
            }

        }
        public async Task RemoveConnectors(string nodeId)
        {
            var belowConnector = await GetList<BusinessRuleConnectorViewModel, BusinessRuleConnector>(x => x.SourceId == nodeId);
            if (belowConnector != null)
            {
                foreach (var connector in belowConnector)
                {
                    await Delete<BusinessRuleConnectorViewModel, BusinessRuleConnector>(connector.Id);
                }
            }
            var aboveConnector = await GetList<BusinessRuleConnectorViewModel, BusinessRuleConnector>(x => x.TargetId == nodeId);
            if (aboveConnector != null)
            {
                foreach (var connector in aboveConnector)
                {
                    await Delete<BusinessRuleConnectorViewModel, BusinessRuleConnector>(connector.Id);
                }
            }

        }
        public async Task<List<BusinessRuleNodeViewModel>> GetNodeAndChilds(string nodeId, List<BusinessRuleNodeViewModel> list)
        {
            var comp = await GetSingleById(nodeId);
            if (comp != null)
            {
                // GetConnector
                var belowConnector = await GetList<BusinessRuleConnectorViewModel, BusinessRuleConnector>(x => x.SourceId == nodeId);
                if (belowConnector != null)
                {
                    foreach (var connector in belowConnector)
                    {
                        var node = await GetSingleById(connector.TargetId);
                        // Get Childs                
                        var childList = await GetNodeAndChilds(node.Id, list);
                    }
                }

                list.Add(comp);
            }

            return list;
        }

        public async Task<List<dynamic>> CopyBusinessRuleNodes(List<dynamic> BRIds, List<BusinessRuleNodeViewModel> oldNodesList)
        {
            List<dynamic> nodeIds = new();
            foreach (var node in oldNodesList)
            {
                var idx = BRIds.FindIndex(x => x.OldId == node.BusinessRuleId);
                string newId = BRIds[idx].NewId.ToString();
                var model = _autoMapper.Map<BusinessRuleNodeViewModel>(node);
                model.Id = null;
                model.BusinessRuleId = newId;
                var res = await Create(model);
                if (res.IsSuccess)
                {
                    var x = new { OldId = node.Id, NewId = res.Item.Id };
                    nodeIds.Add(x);
                }
            }
            return nodeIds;
        }


    }
}
