using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ExecutionScriptBusiness : BusinessBase<ExecutionScriptComponentViewModel, ExecutionScriptComponent>, IExecutionScriptBusiness
    {
        
        public ExecutionScriptBusiness(IRepositoryBase<ExecutionScriptComponentViewModel, ExecutionScriptComponent> repo, IMapper autoMapper
            ) : base(repo, autoMapper)
        {
           
        }

        public async override Task<CommandResult<ExecutionScriptComponentViewModel>> Create(ExecutionScriptComponentViewModel model)
        {

            var result = await base.Create(model);
            return CommandResult<ExecutionScriptComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ExecutionScriptComponentViewModel>> Edit(ExecutionScriptComponentViewModel model)
        {

            var result = await base.Edit(model);
            return CommandResult<ExecutionScriptComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
