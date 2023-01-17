using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class FormIndexPageColumnBusiness : BusinessBase<FormIndexPageColumnViewModel, FormIndexPageColumn>, IFormIndexPageColumnBusiness
    {
        public FormIndexPageColumnBusiness(IRepositoryBase<FormIndexPageColumnViewModel, FormIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<FormIndexPageColumnViewModel>> Create(FormIndexPageColumnViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<FormIndexPageColumnViewModel>(model);
            var result = await base.Create(data, autoCommit);
            return CommandResult<FormIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<FormIndexPageColumnViewModel>> Edit(FormIndexPageColumnViewModel model, bool autoCommit = true)
        {
            //ar data = _autoMapper.Map<IndexPageColumnViewModel>(model);
            var result = await base.Edit(model,autoCommit);
            return CommandResult<FormIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
