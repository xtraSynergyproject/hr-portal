using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Linq;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class CustomIndexPageColumnBusiness : BusinessBase<CustomIndexPageColumnViewModel, CustomIndexPageColumn>, ICustomIndexPageColumnBusiness
    {
        public CustomIndexPageColumnBusiness(IRepositoryBase<CustomIndexPageColumnViewModel, CustomIndexPageColumn> repo
            , IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<CommandResult<CustomIndexPageColumnViewModel>> Create(CustomIndexPageColumnViewModel model)
        {
            var result = await base.Create(model);

            return CommandResult<CustomIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CustomIndexPageColumnViewModel>> Edit(CustomIndexPageColumnViewModel model)
        {
            var result = await base.Edit(model);
            //return CommandResult<IndexPageTemplateViewModel>.Instance(model);
            return CommandResult<CustomIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
