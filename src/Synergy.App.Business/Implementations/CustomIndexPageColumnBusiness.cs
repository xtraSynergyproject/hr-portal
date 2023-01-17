using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class CustomIndexPageColumnBusiness : BusinessBase<CustomIndexPageColumnViewModel, CustomIndexPageColumn>, ICustomIndexPageColumnBusiness
    {
        public CustomIndexPageColumnBusiness(IRepositoryBase<CustomIndexPageColumnViewModel, CustomIndexPageColumn> repo
            , IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<CustomIndexPageColumnViewModel>> Create(CustomIndexPageColumnViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model, autoCommit);

            return CommandResult<CustomIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CustomIndexPageColumnViewModel>> Edit(CustomIndexPageColumnViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model, autoCommit);
            //return CommandResult<IndexPageTemplateViewModel>.Instance(model);
            return CommandResult<CustomIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
