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
    public class BreResultBusiness : BusinessBase<BreResultViewModel, BreResult>, IBreResultBusiness
    {
        public BreResultBusiness(IRepositoryBase<BreResultViewModel, BreResult> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<BreResultViewModel>> Create(BreResultViewModel model, bool autoCommit = true)
        {
            if (model.BusinessRuleNodeId.IsNullOrEmpty())
            {
                return CommandResult<BreResultViewModel>.Instance(model, x => x.BusinessRuleNodeId, "Busienss Rule Note is required.");
            }
            var data = _autoMapper.Map<BreResultViewModel>(model);
            var result = await base.Create(data, autoCommit);
            return CommandResult<BreResultViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<BreResultViewModel>> Edit(BreResultViewModel model, bool autoCommit = true)
        {
            if (model.BusinessRuleNodeId.IsNullOrEmpty())
            {
                return CommandResult<BreResultViewModel>.Instance(model, x => x.BusinessRuleNodeId, "Busienss Rule Note is required.");
            }
            var data = _autoMapper.Map<BreResultViewModel>(model);
            var result = await base.Edit(data, autoCommit);
            return CommandResult<BreResultViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<dynamic>> CopyBreResults(List<dynamic> nodeIds, List<BreResultViewModel> oldList)
        {
            List<dynamic> breResultIds = new();
            foreach (var item in oldList)
            {
                var idx = nodeIds.FindIndex(x => x.OldId == item.BusinessRuleNodeId);
                string newId = nodeIds[idx].NewId.ToString();
                var model = _autoMapper.Map<BreResultViewModel>(item);
                model.Id = null;
                model.BusinessRuleNodeId = newId;
                var res = await base.Create(model);
                if (res.IsSuccess)
                {
                    var x = new { OldId = item.Id, NewId = res.Item.Id };
                    breResultIds.Add(x);
                }
            }
            return breResultIds;
        }

    }
}
