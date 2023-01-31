using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class RuntimeWorkflowBusiness : BusinessBase<RuntimeWorkflowViewModel, RuntimeWorkflow>, IRuntimeWorkflowBusiness
    {
        IComponentBusiness _componentBusiness;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public RuntimeWorkflowBusiness(IRepositoryBase<RuntimeWorkflowViewModel, RuntimeWorkflow> repo, IMapper autoMapper,
            IComponentBusiness componentBusiness, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _componentBusiness = componentBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<RuntimeWorkflowViewModel>> Create(RuntimeWorkflowViewModel model, bool autoCommit = true)
        {            
            var result = await base.Create(model,autoCommit);
            return CommandResult<RuntimeWorkflowViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<RuntimeWorkflowViewModel>> Edit(RuntimeWorkflowViewModel model, bool autoCommit = true)
        {         
            var result = await base.Edit(model,autoCommit);
            return CommandResult<RuntimeWorkflowViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

       
    }
}
