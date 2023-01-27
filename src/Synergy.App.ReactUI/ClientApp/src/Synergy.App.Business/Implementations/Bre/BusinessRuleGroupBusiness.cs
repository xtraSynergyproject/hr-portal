using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class BusinessRuleGroupBusiness : BusinessBase<BusinessRuleGroupViewModel, BusinessRuleGroup>, IBusinessRuleGroupBusiness
    {
        public BusinessRuleGroupBusiness(IRepositoryBase<BusinessRuleGroupViewModel, BusinessRuleGroup> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<BusinessRuleGroupViewModel>> Create(BusinessRuleGroupViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<BusinessRuleGroupViewModel>(model);
            var result = await base.Create<BusinessRuleGroupViewModel, BusinessRuleGroup>(data, autoCommit);
            return CommandResult<BusinessRuleGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
