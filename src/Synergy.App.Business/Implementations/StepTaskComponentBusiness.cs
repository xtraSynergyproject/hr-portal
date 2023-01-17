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
    public class StepTaskComponentBusiness : BusinessBase<StepTaskComponentViewModel, StepTaskComponent>, IStepTaskComponentBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<HierarchyMasterViewModel> _queryRepoH;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public StepTaskComponentBusiness(IRepositoryQueryBase<IdNameViewModel> queryRepo,
            IRepositoryQueryBase<HierarchyMasterViewModel> queryRepoH,

            IRepositoryBase<StepTaskComponentViewModel, StepTaskComponent> repo, IMapper autoMapper
            , ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;

            _queryRepoH = queryRepoH;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<StepTaskComponentViewModel>> Create(StepTaskComponentViewModel model, bool autoCommit = true)
        {
            if (model.AssignedToTeamId.IsNullOrEmpty())
            {
                model.AssignedToTeamId = null;
            }
            if (model.AssignedToUserId.IsNullOrEmpty())
            {
                model.AssignedToUserId = null;
            }
            if (model.AssignedToHierarchyMasterId.IsNullOrEmpty())
            {
                model.AssignedToHierarchyMasterId = null;
            }
            if (model.SLASeconds.HasValue)
            {
                model.SLA = TimeSpan.FromSeconds(model.SLASeconds.Value);
            }
            var assignToType = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "TASK_ASSIGN_TO_TYPE" && x.Code == model.AssignedToTypeCode);
            model.AssignedToTypeId = assignToType?.Id;

            var result = await base.Create(model,autoCommit);
            return CommandResult<StepTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<StepTaskComponentViewModel>> Edit(StepTaskComponentViewModel model, bool autoCommit = true)
        {
            if (model.AssignedToTeamId.IsNullOrEmpty())
            {
                model.AssignedToTeamId = null;
            }
            if (model.AssignedToUserId.IsNullOrEmpty())
            {
                model.AssignedToUserId = null;
            }
            if (model.AssignedToHierarchyMasterId.IsNullOrEmpty())
            {
                model.AssignedToHierarchyMasterId = null;
            }
            if (model.SLASeconds.HasValue)
            {
                model.SLA = TimeSpan.FromSeconds(model.SLASeconds.Value);
            }
            else
            {
                model.SLA = null;
            }
            var assignToType = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "TASK_ASSIGN_TO_TYPE" && x.Code == model.AssignedToTypeCode);
            model.AssignedToTypeId = assignToType?.Id;
            var result = await base.Edit(model,autoCommit);
            return CommandResult<StepTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }




        public async Task<IList<IdNameViewModel>> GetComponentList()
        {
            var list = await _cmsQueryBusiness.GetComponentListData();
            return list;
        }

        public async Task RemoveStepTask(string componentId)
        {
            // Remove Decision  Component
            var DecisionScript = await GetSingle(x => x.ComponentId == componentId);
            if (DecisionScript != null)
            {
                await Delete(DecisionScript.Id);
            }
        }
        public async Task<IList<IdNameViewModel>> GetStepTaskParentList(string templateId)
        {
            var list = await _cmsQueryBusiness.GetStepTaskTemplateList(templateId);
            return list;
        }
    }
}
