using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ExecutionScriptBusiness : BusinessBase<ExecutionScriptComponentViewModel, ExecutionScriptComponent>, IExecutionScriptBusiness
    {
        
        public ExecutionScriptBusiness(IRepositoryBase<ExecutionScriptComponentViewModel, ExecutionScriptComponent> repo, IMapper autoMapper
            ) : base(repo, autoMapper)
        {
           
        }

        public async override Task<CommandResult<ExecutionScriptComponentViewModel>> Create(ExecutionScriptComponentViewModel model, bool autoCommit = true)
        {

            var result = await base.Create(model,autoCommit);
            return CommandResult<ExecutionScriptComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ExecutionScriptComponentViewModel>> Edit(ExecutionScriptComponentViewModel model, bool autoCommit = true)
        {

            var result = await base.Edit(model,autoCommit);
            return CommandResult<ExecutionScriptComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
