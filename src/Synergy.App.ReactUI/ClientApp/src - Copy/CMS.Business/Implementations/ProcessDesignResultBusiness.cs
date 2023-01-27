using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ProcessDesignResultBusiness : BusinessBase<ProcessDesignResultViewModel, ProcessDesignResult>, IProcessDesignResultBusiness
    {
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly IServiceProvider _serviceProvider;

        public ProcessDesignResultBusiness(IRepositoryBase<ProcessDesignResultViewModel,
            ProcessDesignResult> repo,
            IMapper autoMapper,
          IComponentResultBusiness componentResultBusiness,
          IServiceProvider serviceProvider

            ) : base(repo, autoMapper)
        {
            _componentResultBusiness = componentResultBusiness;
            _serviceProvider = serviceProvider;
        }

        public override async Task<CommandResult<ProcessDesignResultViewModel>> Create(ProcessDesignResultViewModel model)
        {
            model.StartDate = DateTime.Now;
            model.ProcessDesignStatusId = await GetProcessDesignStatusId("PROCESS_DESIGN_STATUS_INPROGRESS");
            var result = await base.Create(model);
            if (result.IsSuccess)
            {
                var components = await _repo.GetList<ComponentViewModel, Component>(x => x.ProcessDesignId == model.ProcessDesignId);
                foreach (var component in components)
                {
                    var componentResult = new ComponentResultViewModel
                    {
                        ComponentType = component.ComponentType,
                        ProcessDesignResultId = result.Item.Id,
                        ProcessDesignId = model.ProcessDesignId,
                        ComponentId = component.Id,
                        NtsServiceId = model.NtsServiceId,
                        ComponentStatusId = await GetComponentStatusId("COMPONENT_STATUS_DRAFT"),
                    };
                    await _repo.Create<ComponentResultViewModel, ComponentResult>(componentResult);
                }
                var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
                var service = await _serviceBusiness.GetSingleById(model.NtsServiceId);
                if (service.NextStepTaskComponentId.IsNotNullAndNotEmpty())
                {
                    await _componentResultBusiness.ExecuteDynamicStepTaskComponent(service.NextStepTaskComponentId, model.NtsServiceId);
                }
                else
                {
                    var startComponentResult = await _repo.GetSingle<ComponentResultViewModel, ComponentResult>(x => x.ProcessDesignResultId == result.Item.Id && x.ComponentType == ProcessDesignComponentTypeEnum.Start);
                    if (startComponentResult != null)
                    {
                         BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(startComponentResult.Id,_repo.UserContext.ToIdentityUser()));
                        //await _componentResultBusiness.ExecuteComponent(startComponentResult.Id);
                    }
                }
            }
            return CommandResult<ProcessDesignResultViewModel>.Instance(model);
        }


        public override async Task<CommandResult<ProcessDesignResultViewModel>> Edit(ProcessDesignResultViewModel model)
        {
            model.StartDate = DateTime.Now;
            model.ProcessDesignStatusId = await GetProcessDesignStatusId("PROCESS_DESIGN_STATUS_INPROGRESS");
            var result = await base.Edit(model);
            if (result.IsSuccess)
            {
                var components = await _repo.GetList<ComponentViewModel, Component>(x => x.ProcessDesignId == model.ProcessDesignId);
                foreach (var component in components)
                {
                    var componentResult = await _repo.GetSingle<ComponentResultViewModel, ComponentResult>(x => x.ComponentId== component.Id && x.ComponentType==component.ComponentType && x.NtsServiceId==model.NtsServiceId && x.ProcessDesignResultId== result.Item.Id);
                    // componentResult = new ComponentResultViewModel()
                    // {
                    //componentResult.ComponentType = component.ComponentType;
                    //componentResult.ProcessDesignResultId = result.Item.Id;
                    //componentResult.ProcessDesignId = model.ProcessDesignId;
                   // componentResult.ComponentId = component.Id;
                    //componentResult.NtsServiceId = model.NtsServiceId;
                    componentResult.ComponentStatusId = await GetComponentStatusId("COMPONENT_STATUS_DRAFT");
                    //};
                    await _repo.Edit<ComponentResultViewModel, ComponentResult>(componentResult);
                }
                var startComponentResult = await _repo.GetSingle<ComponentResultViewModel, ComponentResult>(x => x.ProcessDesignResultId == result.Item.Id && x.ComponentType == ProcessDesignComponentTypeEnum.Start);
                if (startComponentResult != null)
                {
                    await _componentResultBusiness.ExecuteComponent(startComponentResult.Id);                   
                    //BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(startComponentResult.Id,_repo.UserContext.ToIdentityUser()));
                }
            }
            return CommandResult<ProcessDesignResultViewModel>.Instance(model);
        }
        public async Task ExecuteProcessDesignResult(string templateId, string serviceId)
        {
            var existing = await _repo.GetSingle(x => x.NtsServiceId == serviceId);
            if (existing == null)
            {
                var processDesign = await _repo.GetSingle<ProcessDesignViewModel, ProcessDesign>(x => x.TemplateId == templateId);
                if (processDesign != null)
                {
                    await Create(new ProcessDesignResultViewModel
                    {
                        NtsServiceId = serviceId,
                        ProcessDesignId = processDesign.Id,
                    });
                }

            }
            else 
            {
               // var processDesign = await _repo.GetSingle<ProcessDesignViewModel, ProcessDesign>(x => x.TemplateId == templateId);
               // var processDesignResult = await _repo.GetSingle<ProcessDesignResultViewModel, ProcessDesignResult>(x => x.ProcessDesignId == processDesign.Id && x.NtsServiceId==serviceId);
                //if (processDesign != null)
               // {
                    await Edit(existing);
               // }
            }
        }
        private async Task<string> GetComponentStatusId(string statusCode)
        {
            var lov = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "COMPONENT_STATUS" && x.Code == statusCode);
            if (lov != null)
            {
                return lov.Id;
            }
            return null;
        }
        private async Task<string> GetProcessDesignStatusId(string statusCode)
        {
            var lov = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "PROCESS_DESIGN_STATUS" && x.Code == statusCode);
            if (lov != null)
            {
                return lov.Id;
            }
            return null;
        }
    }
}
