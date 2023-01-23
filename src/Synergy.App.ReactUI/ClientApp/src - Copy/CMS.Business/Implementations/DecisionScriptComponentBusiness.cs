using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class DecisionScriptComponentBusiness : BusinessBase<DecisionScriptComponentViewModel, DecisionScriptComponent>, IDecisionScriptComponentBusiness
    {

        private readonly IServiceProvider _serviceProvider;
        public DecisionScriptComponentBusiness(IRepositoryBase<DecisionScriptComponentViewModel, DecisionScriptComponent> repo
            , IMapper autoMapper
            , IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _serviceProvider = serviceProvider;
        }

        public async override Task<CommandResult<DecisionScriptComponentViewModel>> Create(DecisionScriptComponentViewModel model)
        {

            var result = await base.Create(model);
            if (result.IsSuccess)
            {
                var brnBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
                await brnBusiness.ManageOperationValue(null, result.Item.Id, null);
            }
            return result;
        }

        public async override Task<CommandResult<DecisionScriptComponentViewModel>> Edit(DecisionScriptComponentViewModel model)
        {

            var result = await base.Edit(model);
            if (result.IsSuccess)
            {
                var brnBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
                await brnBusiness.ManageOperationValue(null, result.Item.Id, null);
            }
            return result;
        }

        public async Task RemoveDecisionScript(string componentId)
        {
            // Remove Decision  Component
            var DecisionScript = await GetSingle(x => x.ComponentId == componentId);
            if (DecisionScript != null)
            {
                var businessRuleModel = await GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.DecisionScriptComponentId == DecisionScript.Id);
                foreach (var ruleModel in businessRuleModel)
                {
                    await Delete<BusinessRuleModelViewModel, BusinessRuleModel>(ruleModel.Id);
                }
                await Delete(DecisionScript.Id);
            }

        }
    }
}
