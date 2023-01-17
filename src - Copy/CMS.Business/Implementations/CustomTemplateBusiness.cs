using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class CustomTemplateBusiness : BusinessBase<CustomTemplateViewModel, CustomTemplate>, ICustomTemplateBusiness
    {
        public CustomTemplateBusiness(IRepositoryBase<CustomTemplateViewModel, CustomTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<CustomTemplateViewModel>> Create(CustomTemplateViewModel model)
        {
            var result = await base.Create(model);
            return CommandResult<CustomTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CustomTemplateViewModel>> Edit(CustomTemplateViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<CustomTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
