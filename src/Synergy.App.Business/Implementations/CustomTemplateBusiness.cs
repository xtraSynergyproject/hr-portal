using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class CustomTemplateBusiness : BusinessBase<CustomTemplateViewModel, CustomTemplate>, ICustomTemplateBusiness
    {
        public CustomTemplateBusiness(IRepositoryBase<CustomTemplateViewModel, CustomTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<CustomTemplateViewModel>> Create(CustomTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model, autoCommit);
            return CommandResult<CustomTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CustomTemplateViewModel>> Edit(CustomTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model, autoCommit);
            return CommandResult<CustomTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
