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
    public class RuntimeWorkflowDataBusiness : BusinessBase<RuntimeWorkflowDataViewModel, RuntimeWorkflowData>, IRuntimeWorkflowDataBusiness
    {
        IComponentBusiness _componentBusiness;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public RuntimeWorkflowDataBusiness(IRepositoryBase<RuntimeWorkflowDataViewModel, RuntimeWorkflowData> repo, IMapper autoMapper,
            IComponentBusiness componentBusiness, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _componentBusiness = componentBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<RuntimeWorkflowDataViewModel>> Create(RuntimeWorkflowDataViewModel model, bool autoCommit = true)
        {            
            var result = await base.Create(model,autoCommit);
            return CommandResult<RuntimeWorkflowDataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<RuntimeWorkflowDataViewModel>> Edit(RuntimeWorkflowDataViewModel model, bool autoCommit = true)
        {         
            var result = await base.Edit(model,autoCommit);
            return CommandResult<RuntimeWorkflowDataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<RuntimeWorkflowDataViewModel>> GetRuntimeWorkflowDataList(string runtimeWorkflowDataId)
        {
            var result = await _cmsQueryBusiness.GetRuntimeWorkflowDataList(runtimeWorkflowDataId);
            return result;
        }


    }
}
