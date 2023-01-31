using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class BusinessRuleBusiness : BusinessBase<BusinessRuleViewModel, BusinessRule>, IBusinessRuleBusiness
    {
        private readonly IRepositoryQueryBase<BusinessRuleViewModel> _queryBusinessRule;
        private readonly IBusinessRuleNodeBusiness _businessRuleNodeBusiness;
        private readonly IDynamicScriptBusiness _dynamicScriptBusiness;
        private readonly IServiceProvider _serviceProvider;
        public BusinessRuleBusiness(IRepositoryBase<BusinessRuleViewModel, BusinessRule> repo
            , IMapper autoMapper, IRepositoryQueryBase<BusinessRuleViewModel> queryBusinessRule,
            IBusinessRuleNodeBusiness businessRuleNodeBusiness
            , IDynamicScriptBusiness dynamicScriptBusiness,
             IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _queryBusinessRule = queryBusinessRule;
            _businessRuleNodeBusiness = businessRuleNodeBusiness;
            _dynamicScriptBusiness = dynamicScriptBusiness;
            _serviceProvider = serviceProvider;
        }
        public async override Task<CommandResult<BusinessRuleViewModel>> Create(BusinessRuleViewModel model)
        {
            var validateName = await IsExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<BusinessRuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);
            if (result.IsSuccess)
            {
                var brnModel = new BusinessRuleNodeViewModel()
                {
                    Name = "Start Business Rule",
                    BusinessRuleId = result.Item.Id,
                    Type = FlowShapes.Terminator,
                    IsStarter = true
                };
                await _businessRuleNodeBusiness.Create(brnModel);
            }

            return CommandResult<BusinessRuleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async override Task<CommandResult<BusinessRuleViewModel>> Edit(BusinessRuleViewModel model)
        {
            var validateName = await IsExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<BusinessRuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<BusinessRuleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<BusinessRuleViewModel>> IsExists(BusinessRuleViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            var exist = await _repo.GetSingle(x => x.TemplateId == model.TemplateId && x.ActionId == model.ActionId && x.Id != model.Id && x.IsDeleted == false && x.BusinessLogicExecutionType == model.BusinessLogicExecutionType);
            if (exist != null)
            {
                errorList.Add("Action", "Record already exist for selected Action.");
            }

            if (errorList.Count > 0)
            {
                return CommandResult<BusinessRuleViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<BusinessRuleViewModel>.Instance();
        }
        public async Task<List<BusinessRuleTreeViewModel>> GetBusinessRuleTreeList(string parentId)
        {
            var list = new List<BusinessRuleTreeViewModel>();

            if (parentId.IsNullOrEmpty())
            {

                var childList = await GetSingle<BusinessAreaViewModel, BusinessArea>(x => x.ParentId == _repo.UserContext.CompanyId);

                //var grandChildList = await GetList<BusinessSectionViewModel, BusinessSection>(x => x.CompanyId == _repo.UserContext.CompanyId);
                list.Add(new BusinessRuleTreeViewModel
                {
                    Expanded = true,
                    Id = _repo.UserContext.CompanyId,
                    Name = _repo.UserContext.CompanyName,
                    ParentId = null,
                    CompanyId = _repo.UserContext.CompanyId,
                    BusinessRuleTreeNodeType = BusinessRuleTreeNodeTypeEnum.Root,
                    HasSubFolders = childList != null
                });
                //list.AddRange(childList.Select(x => new BusinessRuleTreeViewModel
                //{
                //    Expanded = false,
                //    Id = x.Id,
                //    CompanyId = x.CompanyId,
                //    Name = x.Name,
                //    ParentId = x.ParentId,
                //    BusinessRuleTreeNodeType = BusinessRuleTreeNodeTypeEnum.BusinessArea,
                //    SequenceOrder = x.SequenceOrder
                //}));
                //list.ForEach(x => x.HasSubFolders = grandChildList.Any(y => y.ParentId == x.Id));

                return list;
            }
            else
            {
                if (parentId == _repo.UserContext.CompanyId)
                {
                    var childList = await GetList<BusinessAreaViewModel, BusinessArea>(x => x.ParentId == parentId);
                    var grandChildList = await GetList<BusinessSectionViewModel, BusinessSection>(x => x.CompanyId == _repo.UserContext.CompanyId);
                    list.AddRange(childList.Select(x => new BusinessRuleTreeViewModel
                    {
                        Expanded = true,
                        Id = x.Id,
                        CompanyId = x.CompanyId,
                        Name = x.Name,
                        ParentId = x.ParentId,
                        BusinessRuleTreeNodeType = BusinessRuleTreeNodeTypeEnum.BusinessArea,
                        SequenceOrder = x.SequenceOrder
                    }));
                    list.ForEach(x => x.HasSubFolders = grandChildList.Any(y => y.ParentId == x.Id));
                    return list;
                }
                else
                {
                    var ba = await _repo.GetSingleById<BusinessAreaViewModel, BusinessArea>(parentId);
                    if (ba != null)
                    {
                        var childList = await GetList<BusinessSectionViewModel, BusinessSection>(x => x.ParentId == parentId);
                        var grandChildList = await GetList<BusinessRuleGroupViewModel, BusinessRuleGroup>(x => x.CompanyId == _repo.UserContext.CompanyId);
                        list.AddRange(childList.Select(x => new BusinessRuleTreeViewModel
                        {
                            Expanded = true,
                            Id = x.Id,
                            CompanyId = x.CompanyId,
                            Name = x.Name,
                            ParentId = x.ParentId,
                            BusinessRuleTreeNodeType = BusinessRuleTreeNodeTypeEnum.BusinessSection,
                            SequenceOrder = x.SequenceOrder
                        }));
                        list.ForEach(x => x.HasSubFolders = grandChildList.Any(y => y.ParentId == x.Id));
                        return list;
                    }
                    var bs = await _repo.GetSingleById<BusinessSectionViewModel, BusinessSection>(parentId);
                    if (bs != null)
                    {
                        var childList = await GetList<BusinessRuleGroupViewModel, BusinessRuleGroup>(x => x.ParentId == parentId);
                        var grandChildList = await GetList<BusinessRuleViewModel, BusinessRule>(x => x.CompanyId == _repo.UserContext.CompanyId);
                        list.AddRange(childList.Select(x => new BusinessRuleTreeViewModel
                        {
                            Expanded = false,
                            Id = x.Id,
                            CompanyId = x.CompanyId,
                            Name = x.Name,
                            ParentId = x.ParentId,
                            BusinessRuleTreeNodeType = BusinessRuleTreeNodeTypeEnum.BusinessRuleGroup,
                            SequenceOrder = x.SequenceOrder
                        }));
                        list.ForEach(x => x.HasSubFolders = grandChildList.Any(y => y.ParentId == x.Id));
                        return list;
                    }
                    var bg = await _repo.GetSingleById<BusinessRuleGroupViewModel, BusinessRuleGroup>(parentId);
                    if (bg != null)
                    {
                        var childList = await GetList<BusinessRuleViewModel, BusinessRule>(x => x.ParentId == parentId);
                        list.AddRange(childList.Select(x => new BusinessRuleTreeViewModel
                        {
                            Expanded = false,
                            Id = x.Id,
                            CompanyId = x.CompanyId,
                            Name = x.Name,
                            ParentId = x.ParentId,
                            BusinessRuleTreeNodeType = BusinessRuleTreeNodeTypeEnum.BusinessRule,
                            SequenceOrder = x.SequenceOrder,
                            HasSubFolders = false
                        }));
                    }

                }
            }
            return list;
        }



        public async Task<bool> CreateBreMasterMetadata(BreMasterMetadataViewModel model)
        {
            var result = await this.Create<BreMasterMetadataViewModel, BreMasterTableMetadata>(model);
            return true;
        }
        public async Task<BusinessRuleViewModel> GetDiagramDataByRuleId(string BussinessRuleId)
        {
            var data = await GetList(x => x.Id == BussinessRuleId);
            if (data.FirstOrDefault() != null)
            {
                return data.FirstOrDefault();
            }
            return null;
        }

        public async Task<CommandResult<T>> ExecuteBusinessRule<T>(BusinessRuleViewModel businessRule, TemplateTypeEnum templateType, T viewModel, dynamic udf) where T : DataModelBase
        {
            try
            {


                if (businessRule == null)
                {
                    return CommandResult<T>.Instance(viewModel, false, "No Business rule");
                }
                var nodes = await _repo.GetList<BusinessRuleNodeViewModel, BusinessRuleNode>(x => x.BusinessRuleId == businessRule.Id);
                var connectors = await _repo.GetList<BusinessRuleConnectorViewModel, BusinessRuleConnector>(x => x.BusinessRuleId == businessRule.Id);
                var node = nodes.FirstOrDefault(x => x.IsStarter);
                var result = CommandResult<T>.Instance(viewModel);

                if (node != null)
                {
                    dynamic inputData = new { Data = udf, Context = _repo.UserContext };
                    result = await RunBusinessRule<T>(templateType, node, nodes, connectors, viewModel, inputData, inputData);
                    return result;
                }
                return CommandResult<T>.Instance(viewModel, false, "No Business rule node found");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<CommandResult<T>> RunBusinessRule<T>(TemplateTypeEnum templateType, BusinessRuleNodeViewModel node, List<BusinessRuleNodeViewModel> nodes
            , List<BusinessRuleConnectorViewModel> connectors, T viewModel, dynamic inputData, dynamic masterData)
        {
            try
            {


                var result = CommandResult<T>.Instance(viewModel);
                switch (node.Type)
                {
                    case FlowShapes.Terminator:
                        if (node.IsStarter)
                        {
                            var nodeList = await GetNextNodeList(node, nodes, connectors);
                            foreach (var item in nodeList)
                            {
                                result = await RunBusinessRule<T>(templateType, item, nodes, connectors, viewModel, inputData, masterData);
                                if (!result.IsSuccess)
                                {
                                    return result;
                                }
                            }
                        }
                        break;
                    case FlowShapes.Process:
                        result = await ExecuteProcess<T>(templateType, node, viewModel, inputData, masterData);
                        if (!result.IsSuccess)
                        {
                            return result;
                        }
                        var nodeList2 = await GetNextNodeList(node, nodes, connectors);
                        foreach (var item in nodeList2)
                        {
                            result = await RunBusinessRule<T>(templateType, item, nodes, connectors, viewModel, inputData, masterData);
                            if (!result.IsSuccess)
                            {
                                return result;
                            }
                        }
                        break;
                    case FlowShapes.Decision:
                        var decisionResult = await ExecuteDecision<T>(templateType, node, viewModel, inputData, masterData);
                        var nextConnector = connectors.FirstOrDefault(x => x.SourceId == node.Id && x.IsForTrue == decisionResult);
                        if (nextConnector != null)
                        {
                            node = nodes.FirstOrDefault(x => x.Id == nextConnector.TargetId);
                            var nodeList3 = await GetNextNodeList(node, nodes, connectors);
                            foreach (var item in nodeList3)
                            {
                                result = await RunBusinessRule<T>(templateType, item, nodes, connectors, viewModel, inputData, masterData);
                                if (!result.IsSuccess)
                                {
                                    return result;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task<bool> ExecuteDecision<T>(TemplateTypeEnum templateType, BusinessRuleNodeViewModel node, T viewModel, dynamic inputData, dynamic masterData)
        {
            try
            {
                if (node != null)
                {
                    if (node.BusinessRuleLogicType == BusinessRuleLogicTypeEnum.Custom)
                    {
                        CommandResult<T> executionResult = await _dynamicScriptBusiness.ExecuteScript<T>(node.Script, viewModel, templateType, inputData, _repo.UserContext, _serviceProvider);
                        if (executionResult != null)
                        {
                            return executionResult.IsSuccess;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (node.OperationValue.IsNotNullAndNotEmpty())
                        {
                            var text = Helper.ExecuteBreLogic<string>(node.OperationValue, inputData, masterData);
                            text = @$"#if({text})
true#else
false#end";
                            var result = Helper.ExecuteBreLogic<bool>(text, inputData, null);
                            return result;
                        }

                    }

                }
                return await Task.FromResult(false);
            }
            catch (Exception)
            {
                throw;
            }

        }


        private async Task<CommandResult<T>> ExecuteProcess<T>(TemplateTypeEnum templateType, BusinessRuleNodeViewModel node, T viewModel, dynamic inputData, dynamic masterData)
        {
            try
            {
                var processResult = await _repo.GetSingle<BreResultViewModel, BreResult>(x => x.BusinessRuleNodeId == node.Id);
                var result = CommandResult<T>.Instance(viewModel);
                if (processResult != null)
                {
                    if (processResult.BreExecuteMethodType == BreExecuteMethodTypeEnum.CustomMethod)
                    {
                        if (processResult.CustomMethodScript.IsNotNullAndNotEmpty())
                        {
                            CommandResult<T> executionResult = await _dynamicScriptBusiness.ExecuteScript<T>(processResult.CustomMethodScript, viewModel, templateType, inputData, _repo.UserContext, _serviceProvider);
                            if (processResult.ReturnIfMethodReturns.IsTrue())
                            {
                                if (processResult.MethodReturnValue.IsTrue())
                                {
                                    if (executionResult.IsSuccess)
                                    {
                                        result = executionResult;
                                        result.IsSuccess = false;
                                    }
                                }
                                else
                                {
                                    if (!executionResult.IsSuccess)
                                    {
                                        result = executionResult;
                                        result.IsSuccess = false;
                                    }
                                }
                            }

                        }
                        else
                        {
                            return CommandResult<T>.Instance(viewModel, false, "Process custom script not found in Business rule");
                        }

                    }
                    else if (processResult.BreExecuteMethodType == BreExecuteMethodTypeEnum.PredefinedMethod)
                    {

                        Type t = Assembly.GetExecutingAssembly().GetType(processResult.MethodNamespace);
                        var methodInfo = t.GetMethod(processResult.MethodName);
                        if (methodInfo != null)
                        {
                            var o = Activator.CreateInstance(t);
                            object[] parameters = new object[4];
                            parameters[0] = viewModel;
                            parameters[1] = inputData.Data;
                            parameters[2] = _repo.UserContext;
                            parameters[3] = _serviceProvider;
                            var taskExecutionResult = (Task<CommandResult<T>>)methodInfo.Invoke(o, parameters);
                            var executionResult = await taskExecutionResult;
                            if (processResult.ReturnIfMethodReturns.IsTrue())
                            {
                                if (processResult.MethodReturnValue.IsTrue())
                                {
                                    if (executionResult.IsSuccess)
                                    {
                                        result = executionResult;
                                        //result.IsSuccess = false;
                                    }
                                    else
                                    {
                                        result = executionResult;
                                        result.IsSuccess = false;
                                    }
                                }
                                else
                                {
                                    if (!executionResult.IsSuccess)
                                    {
                                        result = executionResult;
                                        result.IsSuccess = false;
                                    }
                                }
                            }
                        }
                    }
                    if (processResult.ReturnWithMessage.IsTrue())
                    {
                        result.IsSuccess = false;
                        if (result.Messages == null)
                        {
                            result.Messages = new Dictionary<string, string>();
                        }
                        if (processResult.Message != null)
                        {
                            foreach (var msg in processResult.Message)
                            {
                                result.Messages.Add(msg, msg);
                            }
                        }
                    }
                }
                else
                {
                    return CommandResult<T>.Instance(viewModel, false, "Process not found in Business rule");
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private async Task<List<BusinessRuleNodeViewModel>> GetNextNodeList(BusinessRuleNodeViewModel previousNode, List<BusinessRuleNodeViewModel> nodes, List<BusinessRuleConnectorViewModel> connectors)
        {
            var nextNodeIds = connectors.Where(x => x.SourceId == previousNode.Id).Select(x => x.TargetId);
            return await Task.FromResult(nodes.Where(x => nextNodeIds.Any(y => y == x.Id)).ToList());
        }

        public async Task<List<BusinessRuleViewModel>> GetBusinessRuleActionList(string templateId, int actionType)
        {
            var query = @$"select br.""Id"",br.""Name"", lov.""Name"" as ActionName from public.""BusinessRule"" as br
                        join public.""LOV"" as lov on lov.""Id"" = br.""ActionId""
                        where br.""TemplateId""='{templateId}' and br.""BusinessLogicExecutionType""='{actionType}' and br.""IsDeleted""=false";

            var queryData = await _queryBusinessRule.ExecuteQueryList<BusinessRuleViewModel>(query, null);
            return queryData;

        }

    }
}
