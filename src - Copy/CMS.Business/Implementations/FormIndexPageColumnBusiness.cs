using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class FormIndexPageColumnBusiness : BusinessBase<FormIndexPageColumnViewModel, FormIndexPageColumn>, IFormIndexPageColumnBusiness
    {
        public FormIndexPageColumnBusiness(IRepositoryBase<FormIndexPageColumnViewModel, FormIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<FormIndexPageColumnViewModel>> Create(FormIndexPageColumnViewModel model)
        {
            var data = _autoMapper.Map<FormIndexPageColumnViewModel>(model);
            var result = await base.Create(data);
            return CommandResult<FormIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<FormIndexPageColumnViewModel>> Edit(FormIndexPageColumnViewModel model)
        {
            //ar data = _autoMapper.Map<IndexPageColumnViewModel>(model);
            var result = await base.Edit(model);
            return CommandResult<FormIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
