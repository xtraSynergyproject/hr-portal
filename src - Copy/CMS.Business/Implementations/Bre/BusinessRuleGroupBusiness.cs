using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class BusinessRuleGroupBusiness : BusinessBase<BusinessRuleGroupViewModel, BusinessRuleGroup>, IBusinessRuleGroupBusiness
    {
        public BusinessRuleGroupBusiness(IRepositoryBase<BusinessRuleGroupViewModel, BusinessRuleGroup> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<BusinessRuleGroupViewModel>> Create(BusinessRuleGroupViewModel model)
        {
            var data = _autoMapper.Map<BusinessRuleGroupViewModel>(model);
            var result = await base.Create<BusinessRuleGroupViewModel, BusinessRuleGroup>(data);
            return CommandResult<BusinessRuleGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
