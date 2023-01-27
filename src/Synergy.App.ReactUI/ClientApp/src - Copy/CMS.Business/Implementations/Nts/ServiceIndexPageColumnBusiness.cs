using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ServiceIndexPageColumnBusiness : BusinessBase<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>, IServiceIndexPageColumnBusiness
    {
        public ServiceIndexPageColumnBusiness(IRepositoryBase<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<ServiceIndexPageColumnViewModel>> Create(ServiceIndexPageColumnViewModel model)
        {
         
            var result = await base.Create(model);
            return CommandResult<ServiceIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ServiceIndexPageColumnViewModel>> Edit(ServiceIndexPageColumnViewModel model)
        {
           
            var result = await base.Edit(model);
            return CommandResult<ServiceIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
