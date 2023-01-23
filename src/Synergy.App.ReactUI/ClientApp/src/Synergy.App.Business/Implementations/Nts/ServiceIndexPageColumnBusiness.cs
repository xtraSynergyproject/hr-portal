using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ServiceIndexPageColumnBusiness : BusinessBase<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>, IServiceIndexPageColumnBusiness
    {
        public ServiceIndexPageColumnBusiness(IRepositoryBase<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<ServiceIndexPageColumnViewModel>> Create(ServiceIndexPageColumnViewModel model, bool autoCommit = true)
        {
         
            var result = await base.Create(model,autoCommit);
            return CommandResult<ServiceIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ServiceIndexPageColumnViewModel>> Edit(ServiceIndexPageColumnViewModel model, bool autoCommit = true)
        {
           
            var result = await base.Edit(model,autoCommit);
            return CommandResult<ServiceIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
