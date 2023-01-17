using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ProcessDesignResultBusiness : BusinessBase<ProcessDesignResultViewModel, ProcessDesignResult>, IProcessDesignResultBusiness
    {
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly IServiceProvider _serviceProvider;
        //private readonly IHangfireScheduler _hangfireScheduler;

        public ProcessDesignResultBusiness(IRepositoryBase<ProcessDesignResultViewModel,
            ProcessDesignResult> repo,
            IMapper autoMapper,
          IComponentResultBusiness componentResultBusiness,
          IServiceProvider serviceProvider
            //, IHangfireScheduler hangfireScheduler

            ) : base(repo, autoMapper)
        {
            _componentResultBusiness = componentResultBusiness;
            _serviceProvider = serviceProvider;
            //_hangfireScheduler = hangfireScheduler;
        }

        public override async Task<CommandResult<ProcessDesignResultViewModel>> Create(ProcessDesignResultViewModel model, bool autoCommit = true)
        {
            model.StartDate = DateTime.Now;
            model.ProcessDesignStatusId = await GetProcessDesignStatusId("PROCESS_DESIGN_STATUS_INPROGRESS");
            var result = await base.Create(model, autoCommit);
            if (result.IsSuccess)
            {
                var components = await _repo.GetList<ComponentViewModel, Component>(x => x.ProcessDesignId == model.ProcessDesignId);
                foreach (var component in components)
                {
                    if (component.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                    {
                        var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>
                                                (x => x.ComponentId == component.Id);
                        if (stepTaskComponent != null && stepTaskComponent.IsRuntimeComponent)
                        {
                            var runTimeWorkflow = await _repo.GetSingle<RuntimeWorkflowViewModel, RuntimeWorkflow>
                            (x => x.TriggeringStepTaskComponentId == stepTaskComponent.Id
                            && x.SourceServiceId == model.NtsServiceId);
                            if (runTimeWorkflow != null)
                            {
                                var runTimeDataList = await _repo.GetList<RuntimeWorkflowDataViewModel, RuntimeWorkflowData>
                                (x => x.RuntimeWorkflowId == runTimeWorkflow.Id);
                                if (runTimeDataList != null && runTimeDataList.Any())
                                {
                                    string parentId = null;
                                    foreach (var runTimeData in runTimeDataList.OrderBy(x => x.SequenceOrder))
                                    {
                                        var compResult = new ComponentResultViewModel
                                        {
                                            ComponentType = component.ComponentType,
                                            ProcessDesignResultId = result.Item.Id,
                                            ProcessDesignId = model.ProcessDesignId,
                                            ComponentId = component.Id,
                                            NtsServiceId = model.NtsServiceId,
                                            ComponentStatusId = await GetComponentStatusId("COMPONENT_STATUS_DRAFT"),
                                            RuntimeWorkflowDataId = runTimeData.Id,
                                            ParentComponentResultId = parentId
                                        };
                                        await _repo.Create<ComponentResultViewModel, ComponentResult>(compResult);
                                        if (runTimeWorkflow.RuntimeWorkflowExecutionMode==WorkflowExecutionModeEnum.Sequential)
                                        {
                                            parentId = compResult.Id;
                                        }
                                       
                                    }
                                    continue;
                                }
                            }
                        }
                    }

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
                        //var sm = _serviceProvider.GetService<ISmartCityBusiness>();
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(startComponentResult.Id, _repo.UserContext.ToIdentityUser()));
                        //await _componentResultBusiness.ExecuteComponent(startComponentResult.Id);
                    }
                }
            }
            return CommandResult<ProcessDesignResultViewModel>.Instance(model);
        }


        public override async Task<CommandResult<ProcessDesignResultViewModel>> Edit(ProcessDesignResultViewModel model, bool autoCommit = true)
        {
            model.StartDate = DateTime.Now;
            model.ProcessDesignStatusId = await GetProcessDesignStatusId("PROCESS_DESIGN_STATUS_INPROGRESS");
            var result = await base.Edit(model, autoCommit);
            if (result.IsSuccess)
            {
                var components = await _repo.GetList<ComponentViewModel, Component>(x => x.ProcessDesignId == model.ProcessDesignId);
                foreach (var component in components)
                {
                    var componentResult = await _repo.GetSingle<ComponentResultViewModel, ComponentResult>(x => x.ComponentId == component.Id && x.ComponentType == component.ComponentType && x.NtsServiceId == model.NtsServiceId && x.ProcessDesignResultId == result.Item.Id);
                    componentResult.ComponentStatusId = await GetComponentStatusId("COMPONENT_STATUS_DRAFT");
                    await _repo.Edit<ComponentResultViewModel, ComponentResult>(componentResult);
                }
                var startComponentResult = await _repo.GetSingle<ComponentResultViewModel, ComponentResult>(x => x.ProcessDesignResultId == result.Item.Id && x.ComponentType == ProcessDesignComponentTypeEnum.Start);
                if (startComponentResult != null)
                {
                    var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                    await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(startComponentResult.Id, _repo.UserContext.ToIdentityUser()));
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
                var hasReturnedtask = await GetReturnedTask(serviceId);
                if (!hasReturnedtask)
                {
                    await Edit(existing);
                }

            }
        }

        private async Task<bool> GetReturnedTask(string serviceId)
        {
            var stepTaskList = await _componentResultBusiness.GetStepTaskList(serviceId);
            var returnedTask = stepTaskList.FirstOrDefault(x => x.IsReturned == true && x.IsReopened == false);
            if (returnedTask != null)
            {
                var componentResult = await _componentResultBusiness.GetSingle(x => x.NtsTaskId == returnedTask.Id);
                if (componentResult != null)
                {
                    await _componentResultBusiness.ExecuteStepTaskComponent(componentResult);
                    return true;
                }
            }
            return false;
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
