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
    public class BreResultBusiness : BusinessBase<BreResultViewModel, BreResult>, IBreResultBusiness
    {
        public BreResultBusiness(IRepositoryBase<BreResultViewModel, BreResult> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<BreResultViewModel>> Create(BreResultViewModel model)
        {
            if (model.BusinessRuleNodeId.IsNullOrEmpty())
            {
                return CommandResult<BreResultViewModel>.Instance(model, x => x.BusinessRuleNodeId, "Busienss Rule Note is required.");
            }
            var data = _autoMapper.Map<BreResultViewModel>(model);
            var result = await base.Create(data);
            return CommandResult<BreResultViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<BreResultViewModel>> Edit(BreResultViewModel model)
        {
            if (model.BusinessRuleNodeId.IsNullOrEmpty())
            {
                return CommandResult<BreResultViewModel>.Instance(model, x => x.BusinessRuleNodeId, "Busienss Rule Note is required.");
            }
            var data = _autoMapper.Map<BreResultViewModel>(model);
            var result = await base.Edit(data);
            return CommandResult<BreResultViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

    }
}
