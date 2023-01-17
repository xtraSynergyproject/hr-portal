using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ComponentBusiness : BusinessBase<ComponentViewModel, Component>, IComponentBusiness
    {
        private readonly IRepositoryQueryBase<ComponentParentViewModel> _queryRepo;
        private readonly IDecisionScriptComponentBusiness _decisionScriptBusiness;
        private readonly IStepTaskComponentBusiness _stepTaskComponentBusiness;
        public ComponentBusiness(IRepositoryBase<ComponentViewModel, Component> repo, IMapper autoMapper,
            IRepositoryQueryBase<ComponentParentViewModel> queryRepo,
            IDecisionScriptComponentBusiness decisionScriptBusiness
            , IStepTaskComponentBusiness stepTaskComponentBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _decisionScriptBusiness = decisionScriptBusiness;
            _stepTaskComponentBusiness = stepTaskComponentBusiness;
        }

        public async override Task<CommandResult<ComponentViewModel>> Create(ComponentViewModel model)
        {

         
            var result = await base.Create(model);
            string [] parents= { model.ParentId };        
           await CreateComponentParents(result.Item.Id, parents);
            return CommandResult<ComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ComponentViewModel>> Edit(ComponentViewModel model)
        {          
            var result = await base.Edit(model);
            string[] parents = { model.ParentId };
            await EditComponentParents(result.Item.Id, parents);
            return CommandResult<ComponentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<ComponentParentViewModel>> GetComponentParent(string componentId)
        {
            var query = @$"select cp.* from public.""ComponentParent"" as cp where cp.""ComponentId"" ='{componentId}' and cp.""IsDeleted"" = false";
            var data = await _queryRepo.ExecuteQueryList<ComponentParentViewModel>(query, null);
            return data;
        }
        public async Task<List<ComponentParentViewModel>> GetComponentChild(string componentId)
        {
            var query = @$"select cp.* from public.""ComponentParent"" as cp where cp.""ParentId"" ='{componentId}' and cp.""IsDeleted"" = false";
            var data = await _queryRepo.ExecuteQueryList<ComponentParentViewModel>(query, null);
            return data;
        }

        public async Task RemoveParents(string componentId)
        {
            var query = @$"update  public.""ComponentParent"" set ""IsDeleted"" = true where ""ComponentId"" ='{componentId}'";
            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);
        }

        public async Task CreateComponentParents(string componentId, string[] parents)
        {
            var model = new ComponentParentViewModel();
            foreach (var p in parents)
            {
                if (p.IsNotNullAndNotEmpty()) 
                {
                    model.ComponentId = componentId;
                    model.ParentId = p;
                    var result = await base.Create<ComponentParentViewModel, ComponentParent>(model);
                }               
            }
        }
        public async Task EditComponentParents(string componentId, string[] parents)
        {
          
            foreach (var p in parents)
            {
                if (p.IsNotNullAndNotEmpty())
                {
                    var model = await _repo.GetSingle<ComponentParentViewModel, ComponentParent>(x => x.ComponentId == componentId);
                    model.ComponentId = componentId;
                    model.ParentId = p;
                    var result = await base.Edit<ComponentParentViewModel, ComponentParent>(model);
                }
            }
        }

        public async Task RemoveComponentsByProcessDesignId(string ProcessDesignId)
        {
            // Remove Decision  Component
            var ComponentsList = await GetList(x => x.ProcessDesignId == ProcessDesignId);
            if (ComponentsList != null && ComponentsList.Count() > 0)
            {
                foreach (var comp in ComponentsList)
                {
                    if (comp.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                    {
                        // Remove Step Task Component
                        await _stepTaskComponentBusiness.RemoveStepTask(comp.Id);
                    }
                    if (comp.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                    {
                        // Remove Decision  Component
                        await _decisionScriptBusiness.RemoveDecisionScript(comp.Id);
                    }
                    await RemoveParents(comp.Id);
                    await Delete<ComponentViewModel, Component>(comp.Id);
                }
            }
        }
        public async Task RemoveComponentsAndItsChild(string ComponentId)
        {
            // Remove Decision  Component
            var list = new List<ComponentViewModel>();
            var ComponentsList = await GetComponentsAndChilds(ComponentId, list);
            if (ComponentsList != null && ComponentsList.Count() > 0)
            {
                foreach (var comp in ComponentsList)
                {
                    if (comp.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                    {
                        // Remove Step Task Component
                        await _stepTaskComponentBusiness.RemoveStepTask(comp.Id);
                    }
                    if (comp.ComponentType == ProcessDesignComponentTypeEnum.DecisionScript)
                    {
                        // Remove Decision  Component
                        await _decisionScriptBusiness.RemoveDecisionScript(comp.Id);
                    }
                    await RemoveParents(comp.Id);
                    await Delete<ComponentViewModel, Component>(comp.Id);
                }
            }
        }

        public async Task<List<ComponentViewModel>> GetComponentsAndChilds(string ComponentId, List<ComponentViewModel> list)
        {
            var comp = await GetSingleById(ComponentId);
            if (comp!=null)
            {
                // Get Childs
                var childList = await GetComponentChild(comp.Id);
                foreach (var child in childList) 
                {                   
                    var templist = await GetComponentsAndChilds(child.ComponentId,list) ;
                  
                }
                list.Add(comp);
            }
            
            return list;
        }
    }
}
