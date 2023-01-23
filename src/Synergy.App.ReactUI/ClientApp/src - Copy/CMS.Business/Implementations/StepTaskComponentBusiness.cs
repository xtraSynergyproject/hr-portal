using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class StepTaskComponentBusiness : BusinessBase<StepTaskComponentViewModel, StepTaskComponent>, IStepTaskComponentBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<HierarchyMasterViewModel> _queryRepoH;

        public StepTaskComponentBusiness(IRepositoryQueryBase<IdNameViewModel> queryRepo,
            IRepositoryQueryBase<HierarchyMasterViewModel> queryRepoH,

            IRepositoryBase<StepTaskComponentViewModel, StepTaskComponent> repo, IMapper autoMapper
            ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;

            _queryRepoH = queryRepoH;
        }

        public async override Task<CommandResult<StepTaskComponentViewModel>> Create(StepTaskComponentViewModel model)
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

            var result = await base.Create(model);
            return CommandResult<StepTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<StepTaskComponentViewModel>> Edit(StepTaskComponentViewModel model)
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
            var result = await base.Edit(model);
            return CommandResult<StepTaskComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }




        public async Task<IList<IdNameViewModel>> GetComponentList()
        {
            string query = @$"SELECT ""Id"",""ComponentType"" as Name  
                            FROM public.""Component""
                            where ""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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

    }
}
