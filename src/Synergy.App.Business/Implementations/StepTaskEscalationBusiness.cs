using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class StepTaskEscalationBusiness : BusinessBase<StepTaskEscalationViewModel, StepTaskEscalation>, IStepTaskEscalationBusiness
    {
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public StepTaskEscalationBusiness(IRepositoryBase<StepTaskEscalationViewModel, StepTaskEscalation> repo, IMapper autoMapper
         , ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<StepTaskEscalationViewModel>> Create(StepTaskEscalationViewModel model, bool autoCommit = true)
        {           
            var result = await base.Create(model,autoCommit);
            return CommandResult<StepTaskEscalationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<StepTaskEscalationViewModel>> Edit(StepTaskEscalationViewModel model, bool autoCommit = true)
        {
          
            var result = await base.Edit(model,autoCommit);
            return CommandResult<StepTaskEscalationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<StepTaskEscalationViewModel>> GetStepTaskEscalation(string stepTaskCompenentId)
        {
            var list = await _cmsQueryBusiness.GetStepTaskEscalation(stepTaskCompenentId);
            return list;
        }

        public async Task<List<StepTaskEscalationViewModel>> GetTaskListWithEscalation()
        {
            var list = await _cmsQueryBusiness.GetTaskListWithEscalation();
            return list;
        }



    }
}
