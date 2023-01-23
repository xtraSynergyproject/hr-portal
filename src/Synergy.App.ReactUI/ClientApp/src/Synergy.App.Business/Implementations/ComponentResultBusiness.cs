using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Hangfire;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ComponentResultBusiness : BusinessBase<ComponentResultViewModel, ComponentResult>, IComponentResultBusiness
    {
        private readonly IRepositoryQueryBase<ComponentParentViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDynamicScriptBusiness _dynamicScriptBusiness;
        private readonly IRepositoryQueryBase<ComponentResultViewModel> _queryData;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public ComponentResultBusiness(IRepositoryBase<ComponentResultViewModel, ComponentResult> repo, IMapper autoMapper,
            IRepositoryQueryBase<ComponentParentViewModel> queryRepo
            , IServiceProvider serviceProvider
            , IDynamicScriptBusiness dynamicScriptBusiness,
             IRepositoryQueryBase<ComponentResultViewModel> queryData,
             IUserHierarchyBusiness userHierarchyBusiness
            , ICmsQueryBusiness cmsQueryBusiness
             //, IHangfireScheduler hangfireScheduler
             ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceProvider = serviceProvider;
            _dynamicScriptBusiness = dynamicScriptBusiness;
            _queryData = queryData;
            _userHierarchyBusiness = userHierarchyBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
            //_hangfireScheduler = hangfireScheduler;
        }


        public async Task ExecuteComponent(string componentResultId)
        {
            var componentResult = await _repo.GetSingleById(componentResultId);
            try
            {
                if (componentResult != null)
                {
                    componentResult.StartDate = DateTime.Now;
                    switch (componentResult.ComponentType)
                    {
                        case ProcessDesignComponentTypeEnum.Start:
                            await ExecuteStartComponent(componentResult);
                            break;
                        case ProcessDesignComponentTypeEnum.Stop:
                            await ExecuteStopComponent(componentResult);
                            break;
                        case ProcessDesignComponentTypeEnum.StepTask:
                            await ExecuteStepTaskComponent(componentResult);
                            break;
                        case ProcessDesignComponentTypeEnum.DecisionScript:
                            await ExecuteDecisionScriptComponent(componentResult);
                            break;
                        case ProcessDesignComponentTypeEnum.ExecutionScript:
                            await ExecuteExecutionScriptComponent(componentResult);
                            break;
                        case ProcessDesignComponentTypeEnum.True:
                            await ExecuteTrueComponent(componentResult);
                            break;
                        case ProcessDesignComponentTypeEnum.False:
                            await ExecuteFalseComponent(componentResult);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                componentResult.Error = ex.ToString();
                await UpdateComponentResultStatus(componentResult, "COMPONENT_STATUS_ERROR");
            }


        }

        private async Task ExecuteStartComponent(ComponentResultViewModel componentResult)
        {

            await UpdateComponentResultStatus(componentResult);
            var childComponents = await GetChildComponentResultList(componentResult);
            if (childComponents.Count > 0)
            {
                foreach (var item in childComponents)
                {
                    try
                    {
                        var parents = await GetParentList(item);
                        if (!parents.Any(x => x.ComponentStatusCode != "COMPONENT_STATUS_COMPLETED"))
                        {
                            // var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                            //await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));
                            await ExecuteComponent(item.Id);
                        }


                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
            }
            else
            {
                await UpdateService(componentResult);
            }

        }
        private async Task ExecuteStopComponent(ComponentResultViewModel componentResult)
        {
            await UpdateComponentResultStatus(componentResult);
            await UpdateService(componentResult);
        }
        private async Task ExecuteExecutionScriptComponent(ComponentResultViewModel componentResult)
        {
            var executionScriptComponent = await _repo.GetSingle<ExecutionScriptComponentViewModel, ExecutionScriptComponent>(x => x.ComponentId == componentResult.ComponentId);
            if (executionScriptComponent != null && executionScriptComponent.Script.IsNotNullAndNotEmpty())
            {
                //var scriptParam = await GetScriptParam(componentResult);
                //await _dynamicScriptBusiness.StepTaskExecutionScript(executionScriptComponent.Script, scriptParam);
            }
            await UpdateComponentResultStatus(componentResult);

            var childComponents = await GetChildComponentResultList(componentResult);
            if (childComponents.Count > 0)
            {
                foreach (var item in childComponents)
                {
                    var parents = await GetParentList(item);
                    if (!parents.Any(x => x.ComponentStatusCode != "COMPONENT_STATUS_COMPLETED"))
                    {
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));

                    }
                }
            }
            else
            {
                await UpdateService(componentResult);
            }
        }
        private async Task UpdateComponentResultStatus(ComponentResultViewModel componentResult, string statusCode = "COMPONENT_STATUS_COMPLETED")
        {
            componentResult.ComponentStatusId = await GetComponentStatus(statusCode);
            if (statusCode != "COMPONENT_STATUS_INPROGRESS")
            {
                componentResult.EndDate = DateTime.Now;
            }

            await _repo.Edit(componentResult);
        }
        private async Task ExecuteDecisionScriptComponent(ComponentResultViewModel componentResult)
        {

            bool result = false;
            var decisionScriptComponent = await _repo.GetSingle<DecisionScriptComponentViewModel, DecisionScriptComponent>(x => x.ComponentId == componentResult.ComponentId);
            if (decisionScriptComponent != null)
            {
                var sb = _serviceProvider.GetService<IServiceBusiness>();
                var viewModel = await sb.GetServiceDetails(new ServiceTemplateViewModel { ServiceId = componentResult.NtsServiceId, ActiveUserId = componentResult.CreatedBy });
                dynamic udf = default(dynamic);
                if (viewModel != null)
                {
                    var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(viewModel.UdfTableMetadataId);
                    var selectQuery = await sb.GetSelectQuery(tableMetaData, @$" and ""NtsService"".""Id""='{viewModel.ServiceId}' limit 1");
                    udf = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                }
                dynamic inputData = new { Data = udf, Context = _repo.UserContext };
                if (decisionScriptComponent.BusinessRuleLogicType == BusinessRuleLogicTypeEnum.Custom)
                {
                    CommandResult<ServiceTemplateViewModel> executionResult = await _dynamicScriptBusiness.ExecuteScript<ServiceTemplateViewModel>(decisionScriptComponent.Script, viewModel, TemplateTypeEnum.Service, inputData, _repo.UserContext, _serviceProvider);
                    if (executionResult != null)
                    {
                        result = executionResult.IsSuccess;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    if (decisionScriptComponent.OperationValue.IsNotNullAndNotEmpty())
                    {
                        var text = Helper.ExecuteBreLogic<string>(decisionScriptComponent.OperationValue, inputData, inputData);
                        text = @$"#if({text})
true#else
false#end";
                        result = Helper.ExecuteBreLogic<bool>(text, inputData, inputData);
                    }

                }

            }
            await UpdateComponentResultStatus(componentResult);
            var childComponents = await GetChildComponentResultList(componentResult);
            if (result)
            {
                if (childComponents != null)
                {
                    var trueComponent = childComponents.FirstOrDefault(x => x.ComponentType == ProcessDesignComponentTypeEnum.True);
                    if (trueComponent != null)
                    {
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(trueComponent.Id, _repo.UserContext.ToIdentityUser()));
                    }
                }
            }
            else
            {
                if (childComponents != null)
                {
                    var falseComponent = childComponents.FirstOrDefault(x => x.ComponentType == ProcessDesignComponentTypeEnum.False);
                    if (falseComponent != null)
                    {
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(falseComponent.Id, _repo.UserContext.ToIdentityUser()));
                    }
                }
            }

        }

        //private async Task<ScriptParam> GetScriptParam(ComponentResultViewModel componentResult)
        //{
        //    var scriptParam = new ScriptParam
        //    {
        //        sp = _serviceProvider,
        //    };
        //    if (componentResult != null)
        //    {
        //        var taskBusiness = (ITaskBusiness)_serviceProvider.GetService(typeof(ITaskBusiness));
        //        var noteBusiness = (INoteBusiness)_serviceProvider.GetService(typeof(INoteBusiness));
        //        var tableMetadataBusiness = (ITableMetadataBusiness)_serviceProvider.GetService(typeof(ITableMetadataBusiness));
        //        var serviceBusiness = (IServiceBusiness)_serviceProvider.GetService(typeof(IServiceBusiness));
        //        if (componentResult.NtsTaskId != null)
        //        {
        //            TaskTemplateViewModel taskViewModel = new TaskTemplateViewModel() { TaskId = componentResult.NtsTaskId };
        //            scriptParam.taskViewModel = await taskBusiness.GetTaskDetails(taskViewModel);

        //        }
        //        if (componentResult.NtsServiceId != null)
        //        {
        //            var Service = await serviceBusiness.GetSingleById(componentResult.NtsServiceId);
        //            if (Service != null && Service.UdfNoteId.IsNotNullAndNotEmpty())
        //            {
        //                NoteTemplateViewModel noteTemplate = new NoteTemplateViewModel() { NoteId = Service.UdfNoteId };
        //                scriptParam.noteViewModel = await noteBusiness.GetNoteDetails(noteTemplate);
        //                var NoteTemplate = await _repo.GetSingleById<TemplateViewModel, Template>(Service.UdfNoteId);
        //                if (NoteTemplate != null)
        //                {
        //                    await tableMetadataBusiness.GetTableData(NoteTemplate.TableMetadataId, scriptParam.noteViewModel.UdfNoteTableId);
        //                }
        //            }
        //            ServiceTemplateViewModel serviceViewModel = new ServiceTemplateViewModel() { ServiceId = componentResult.NtsServiceId };
        //            scriptParam.serviceViewModel = await serviceBusiness.GetServiceDetails(serviceViewModel);
        //        }
        //    }
        //    return scriptParam;
        //}

        public async Task ExecuteStepTaskComponent(ComponentResultViewModel componentResult)
        {
            var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.ComponentId == componentResult.ComponentId, x => x.TaskTemplate);

            var skip = await SkipStepTask(stepTaskComponent, componentResult);
            if (skip)
            {
                await UpdateComponentResultStatus(componentResult, "COMPONENT_STATUS_COMPLETED");
                var childComponents = await GetChildComponentResultList(componentResult);
                if (childComponents.Count > 0)
                {
                    foreach (var item in childComponents)
                    {
                        var parents = await GetParentList(item);
                        if (!parents.Any(x => x.ComponentStatusCode != "COMPONENT_STATUS_COMPLETED"))
                        {
                            //await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));
                            var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                            await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));
                            //await ExecuteComponent(item.Id);
                        }
                    }
                }
                else
                {
                    await UpdateService(componentResult, "COMPONENT_STATUS_COMPLETED");
                }
                return;
            }
            if (stepTaskComponent.IsRuntimeComponent)
            {
                var runtimeWorkflow = await _repo.GetSingle<RuntimeWorkflowViewModel, RuntimeWorkflow>
                    (x => x.TriggeringStepTaskComponentId == stepTaskComponent.Id
                    && x.SourceServiceId == componentResult.NtsServiceId
                    && x.SourceTaskId == componentResult.NtsTaskId);
                if (runtimeWorkflow != null)
                {
                    var runtimeDataList = await _repo.GetList<RuntimeWorkflowDataViewModel, RuntimeWorkflowData>
                    (x => x.RuntimeWorkflowId == runtimeWorkflow.Id);
                    if (runtimeDataList != null && runtimeDataList.Any())
                    {
                        if (runtimeWorkflow.RuntimeWorkflowExecutionMode == WorkflowExecutionModeEnum.Sequential)
                        {

                            var runtimeData = runtimeDataList.FirstOrDefault(x => x.Id == componentResult.RuntimeWorkflowDataId);
                            if (runtimeData != null)
                            {
                                await CreateTask(componentResult, stepTaskComponent, runtimeData);
                            }
                        }
                        else
                        {
                            foreach (var runtimeData in runtimeDataList)
                            {
                                componentResult = await GetSingle(x => x.RuntimeWorkflowDataId == runtimeData.Id);
                                await CreateTask(componentResult, stepTaskComponent, runtimeData);
                            }
                        }
                        return;
                    }
                }
            }
            await CreateTask(componentResult, stepTaskComponent);

        }

        private async Task CreateTask(ComponentResultViewModel componentResult, StepTaskComponentViewModel stepTaskComponent, RuntimeWorkflowDataViewModel runtimeData = null)
        {
            var taskBusiness = (ITaskBusiness)_serviceProvider.GetService(typeof(ITaskBusiness));
            var serviceBusiness = (IServiceBusiness)_serviceProvider.GetService(typeof(IServiceBusiness));
            var serviceData = await _repo.GetSingleById<ServiceViewModel, NtsService>(componentResult.NtsServiceId);
            var service = await serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                ServiceId = componentResult.NtsServiceId,
                DataAction = DataActionEnum.Read,
                ActiveUserId = serviceData.OwnerUserId
            });

            // Any task with this step task Component(if yess add version)
            var Ntstask = await taskBusiness.GetSingle(x => x.StepTaskComponentId == stepTaskComponent.Id
            && x.ParentServiceId == componentResult.NtsServiceId && x.RuntimeWorkflowDataId == componentResult.RuntimeWorkflowDataId);
            TaskTemplateViewModel task = new TaskTemplateViewModel();
            if (Ntstask != null)
            {

                task = await taskBusiness.GetTaskDetails(new TaskTemplateViewModel
                {
                    TaskId = Ntstask.Id,
                    DataAction = DataActionEnum.Edit,
                    ActiveUserId = service.OwnerUserId,
                    TaskTemplateType = TaskTypeEnum.StepTask

                });

                task.VersionNo += 1;
                task.IsReopened = true;
            }
            else
            {
                task = await taskBusiness.GetTaskDetails(new TaskTemplateViewModel
                {
                    TemplateId = stepTaskComponent.TemplateId,
                    DataAction = DataActionEnum.Create,
                    ActiveUserId = service.OwnerUserId,
                    StepTaskComponentId = stepTaskComponent.Id
                });
                task.DataAction = DataActionEnum.Create;

                task.CompletedDate = null;
                task.CompleteReason = null;
                task.RejectedDate = null;
                task.RejectionReason = null;
                task.CanceledDate = null;
                task.CancelReason = null;

                await SetStepTaskAssignee(task, service, componentResult, stepTaskComponent, runtimeData);
                task.OwnerUserId = service.OwnerUserId;
                task.RecordId = task.TaskId;

                task.RequestedByUserId = service.RequestedByUserId;
                task.TaskSubject = stepTaskComponent.Subject;
                task.TemplateId = stepTaskComponent.TemplateId;
                task.Description = stepTaskComponent.Description;
                task.StepTaskComponentId = stepTaskComponent.Id;


                task.TaskTemplateType = TaskTypeEnum.StepTask;
                task.UdfNoteId = service.UdfNoteId;
                task.UdfNoteTableId = service.UdfNoteTableId;
                task.UdfTableMetadataId = service.UdfTableMetadataId;
                task.UdfTemplateId = service.UdfTemplateId;
                task.Json = service.Json;
                task.ParentServiceId = service.ServiceId;

                var compSb = await _cmsQueryBusiness.GetComponentParentData(componentResult.ComponentId);
                var componen = compSb.FirstOrDefault();
                if (componen != null)
                {
                    var _ccBusiness = _serviceProvider.GetService<IComponentBusiness>();
                    var compo = await _ccBusiness.GetSingleById(componen.ParentId);
                    if (compo != null && compo.ComponentType == ProcessDesignComponentTypeEnum.Start)
                    {
                        task.TriggeredByReferenceId = service.ServiceId;
                        task.TriggeredByReferenceType = ReferenceTypeEnum.NTS_Service;
                    }
                    else
                    {
                        var compres = await GetSingle(x => x.ComponentId == componen.ParentId && x.NtsServiceId == service.ServiceId);
                        if (compres != null)
                        {
                            task.TriggeredByReferenceType = ReferenceTypeEnum.NTS_Task;
                            task.TriggeredByReferenceId = compres.NtsTaskId;
                        }
                    }
                }

                task.PortalId = service.PortalId;
                task.CompanyId = service.CompanyId;
                task.LegalEntityId = service.LegalEntityId;
                task.CreatedBy = service.CreatedBy;
                task.LastUpdatedBy = service.LastUpdatedBy;
                //next component set up
                await GetNextTaskAssignee(componentResult.ComponentId, task, service);
            }
            if (stepTaskComponent.EnablePlanning)
            {
                task.TaskStatusCode = "TASK_STATUS_PLANNED";
            }
            else
            {
                task.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                task.ActualStartDate = DateTime.Now;
            }
            task.StartDate = DateTime.Now;
            task.SLA = stepTaskComponent.SLA;
            if (!task.SLA.HasValue)
            {
                task.SLA = TimeSpan.FromDays(1);
            }

            task.ActiveUserId = service.OwnerUserId;
            task.DueDate = DateTime.Now.Add(task.SLA.Value);
            var dt = new DataTable();
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Id == service.TemplateId);
            var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == template.UdfTableMetadataId);
            dt = await serviceBusiness.GetServiceDataTableById(service.ServiceId, tableMetaData);
            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var tc = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                var childTable = tc.Where(x => x.UdfUIType == UdfUITypeEnum.editgrid || x.UdfUIType == UdfUITypeEnum.datagrid).ToList();
                foreach (var child in childTable)
                {
                    var tableName = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == child.Name);
                    var editRec = await _cmsBusiness.GetData(ApplicationConstant.Database.Schema.Cms, tableName.Name, "ParentId", dr["UdfNoteTableId"].ToString());
                    if (editRec != null)
                    {
                        //if (editRec.Rows.Count > 0)
                        //{
                        var json = JsonConvert.SerializeObject(editRec);
                        dr[child.Name] = json;
                        //}
                    }
                }
                var dict = dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
                var j = JsonConvert.SerializeObject(dict);
                task.Json = j;
            }
            var result = await taskBusiness.ManageTask(task);
            if (result.IsSuccess)
            {
                componentResult.NtsTaskId = result.Item.TaskId;
                await UpdateComponentResultStatus(componentResult, "COMPONENT_STATUS_INPROGRESS");
            }
            else
            {
                componentResult.Error = result.Messages.ToHtmlError();
                await UpdateComponentResultStatus(componentResult);
            }
        }

        private async Task<bool> SkipStepTask(StepTaskComponentViewModel stepTaskComponent, ComponentResultViewModel componentResult)
        {
            try
            {
                var service = await _repo.GetSingleById<ServiceViewModel, NtsService>(componentResult.NtsServiceId);
                var serviceTemplate = await _repo.GetSingleById<ServiceTemplateViewModel, ServiceTemplate>(service.ServiceTemplateId, x => x.UdfTemplate);
                var skipLogic = await _repo.GetList<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(x => x.StepTaskComponentId == stepTaskComponent.Id);
                if (skipLogic != null && skipLogic.Any())
                {
                    var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(serviceTemplate.UdfTemplate.TableMetadataId);
                    var serviceBusiness = (IServiceBusiness)_serviceProvider.GetService(typeof(IServiceBusiness));
                    var selectQuery = await serviceBusiness.GetSelectQuery(tableMetaData, @$" and ""NtsService"".""Id""='{ componentResult.NtsServiceId}' limit 1");
                    dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                    foreach (var item in skipLogic.OrderBy(x => x.SequenceOrder).ToList())
                    {
                        var rules = await _repo.GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.ReferenceId == item.Id);
                        foreach (var rule in rules)
                        {
                            if (rule != null && rule.BusinessRuleType == BusinessRuleTypeEnum.Operational)
                            {
                                dynamic inputData = new { Data = dobject, Context = _repo.UserContext };
                                var text = Helper.ExecuteBreLogic<string>(rule.OperationBackendValue, inputData, null);
                                text = @$"#if({text})
            true#else
            false#end";
                                var result = Helper.ExecuteBreLogic<bool>(text, inputData, null);
                                return result;
                            }
                        }


                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public async Task ExecuteDynamicStepTaskComponent(string stepcomponentId, string serviceId)
        {
            var taskBusiness = (ITaskBusiness)_serviceProvider.GetService(typeof(ITaskBusiness));
            var serviceBusiness = (IServiceBusiness)_serviceProvider.GetService(typeof(IServiceBusiness));
            var serviceData = await _repo.GetSingleById<ServiceViewModel, NtsService>(serviceId);
            var service = await serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                ServiceId = serviceId,
                DataAction = DataActionEnum.Read,
                ActiveUserId = serviceData.OwnerUserId
            });
            var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.Id == stepcomponentId, x => x.TaskTemplate);
            // Any task with this step task Component(if yess add version)
            var Ntstask = await taskBusiness.GetSingle(x => x.StepTaskComponentId == stepcomponentId && x.ParentServiceId == serviceId);
            TaskTemplateViewModel task = new TaskTemplateViewModel();
            if (Ntstask != null)
            {

                task = await taskBusiness.GetTaskDetails(new TaskTemplateViewModel
                {
                    TaskId = Ntstask.Id,
                    DataAction = DataActionEnum.Edit,
                    ActiveUserId = service.OwnerUserId,
                    TaskTemplateType = TaskTypeEnum.StepTask

                });

                task.VersionNo += 1;
                task.IsReopened = true;
            }
            else
            {
                task = await taskBusiness.GetTaskDetails(new TaskTemplateViewModel
                {
                    TemplateId = stepTaskComponent.TemplateId,
                    DataAction = DataActionEnum.Create,
                    ActiveUserId = service.OwnerUserId,
                    StepTaskComponentId = stepTaskComponent.Id
                });
                task.DataAction = DataActionEnum.Create;

                task.CompletedDate = null;
                task.CompleteReason = null;
                task.RejectedDate = null;
                task.RejectionReason = null;
                task.CanceledDate = null;
                task.CancelReason = null;

                await SetStepTaskAssignee(task, service, new ComponentResultViewModel(), stepTaskComponent);
                task.OwnerUserId = service.OwnerUserId;
                task.RecordId = task.TaskId;

                task.RequestedByUserId = service.RequestedByUserId;
                task.TaskSubject = stepTaskComponent.Subject;
                task.TemplateId = stepTaskComponent.TemplateId;
                task.Description = stepTaskComponent.Description;
                task.StepTaskComponentId = stepTaskComponent.Id;


                task.TaskTemplateType = TaskTypeEnum.StepTask;
                task.UdfNoteId = service.UdfNoteId;
                task.UdfNoteTableId = service.UdfNoteTableId;
                task.UdfTableMetadataId = service.UdfTableMetadataId;
                task.UdfTemplateId = service.UdfTemplateId;
                task.Json = service.Json;
                task.ParentServiceId = service.ServiceId;

                task.PortalId = service.PortalId;
                task.CompanyId = service.CompanyId;
                task.LegalEntityId = service.LegalEntityId;
                task.CreatedBy = service.CreatedBy;
                task.LastUpdatedBy = service.LastUpdatedBy;
                //next component set up
                // await GetNextTaskAssignee(componentResult.ComponentId, task, service);
            }
            task.TaskStatusCode = "TASK_STATUS_INPROGRESS";
            task.StartDate = DateTime.Now;
            task.SLA = stepTaskComponent.SLA;
            if (!task.SLA.HasValue)
            {
                task.SLA = TimeSpan.FromDays(1);
            }

            task.ActiveUserId = service.OwnerUserId;
            task.DueDate = DateTime.Now.Add(task.SLA.Value);
            var dt = new DataTable();
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Id == service.TemplateId);
            var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == template.UdfTableMetadataId);
            dt = await serviceBusiness.GetServiceDataTableById(service.ServiceId, tableMetaData);
            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                var dict = dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
                var j = JsonConvert.SerializeObject(dict);
                task.Json = j;
            }
            var result = await taskBusiness.ManageTask(task);



        }
        private async Task GetNextTaskAssignee(string componentId, TaskTemplateViewModel task, ServiceTemplateViewModel service)
        {

            var taskBusiness = (ITaskBusiness)_serviceProvider.GetService(typeof(ITaskBusiness));
            var fileBusiness = (IFileBusiness)_serviceProvider.GetService(typeof(IFileBusiness));
            var commentBusiness = (INtsTaskCommentBusiness)_serviceProvider.GetService(typeof(INtsTaskCommentBusiness));
            var ComponentParent = await _repo.GetSingle<ComponentParentViewModel, ComponentParent>(x => x.ComponentId == componentId);
            if (ComponentParent != null)
            {
                var Component = await _repo.GetSingle<ComponentViewModel, Component>(x => x.Id == ComponentParent.ParentId);
                if (Component.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                {
                    var stepTaskComp = await GetSingle(x => x.ComponentId == Component.Id && x.NtsServiceId == service.ServiceId);
                    if (stepTaskComp != null)
                    {
                        var pretask = await taskBusiness.GetSingleById(stepTaskComp.NtsTaskId);
                        if (pretask.IsNotNull())
                        {
                            if (pretask.NextTaskAssignedToTypeId.IsNotNullAndNotEmpty())
                            {
                                await SetNextStepTaskAssignee(task, service, pretask);
                                task.TaskId = Guid.NewGuid().ToString();
                                if (pretask.NextTaskAttachmentId.IsNotNullAndNotEmpty())
                                {
                                    var attach = await fileBusiness.GetSingleById(pretask.NextTaskAttachmentId);
                                    if (attach != null)
                                    {
                                        attach.ReferenceTypeCode = ReferenceTypeEnum.NTS_Task;
                                        attach.ReferenceTypeId = task.TaskId;
                                        attach.Id = null;
                                        await fileBusiness.Create(attach);
                                    }
                                }
                            }
                            if (pretask.CompleteReason.IsNotNullAndNotEmpty())
                            {
                                var cmt = new NtsTaskCommentViewModel
                                {
                                    Comment = pretask.CompleteReason,
                                    NtsTaskId = task.TaskId,
                                    CommentedByUserId = pretask.AssignedToUserId,
                                    CommentedTo = CommentToEnum.All,
                                    DataAction = DataActionEnum.Create,

                                };
                                await commentBusiness.Create(cmt);
                            }
                        }

                    }
                }
                else
                {
                    await GetNextTaskAssignee(Component.Id, task, service);
                }
            }
        }
        private async Task ExecuteFalseComponent(ComponentResultViewModel componentResult)
        {
            await UpdateComponentResultStatus(componentResult);
            var childComponents = await GetChildComponentResultList(componentResult);
            if (childComponents.Count > 0)
            {
                foreach (var item in childComponents)
                {
                    var parents = await GetParentList(item);
                    if (!parents.Any(x => x.ComponentStatusCode != "COMPONENT_STATUS_COMPLETED"))
                    {
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));

                    }
                }
            }
            else
            {
                await UpdateService(componentResult);
            }
        }

        private async Task ExecuteTrueComponent(ComponentResultViewModel componentResult)
        {
            await UpdateComponentResultStatus(componentResult);
            var childComponents = await GetChildComponentResultList(componentResult);
            if (childComponents.Count > 0)
            {
                foreach (var item in childComponents)
                {
                    var parents = await GetParentList(item);
                    if (!parents.Any(x => x.ComponentStatusCode != "COMPONENT_STATUS_COMPLETED"))
                    {
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));

                    }
                }
            }
            else
            {
                await UpdateService(componentResult);
            }
        }

        public async Task ManageStepTaskComponent(TaskTemplateViewModel task)
        {
            if (task.NextStepTaskComponentId.IsNotNullAndNotEmpty())
            {
                if (task.NextStepTaskComponentId == "SERVICE_COMPLETE")
                {
                    var sb = _serviceProvider.GetService<IServiceBusiness>();
                    var service = await sb.GetServiceDetails(new ServiceTemplateViewModel
                    {
                        ServiceId = task.ParentServiceId,
                        ActiveUserId = task.OwnerUserId
                    });
                    service.DataAction = DataActionEnum.Edit;
                    service.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";

                    await sb.ManageService(service);
                }
                else
                {
                    await ExecuteDynamicStepTaskComponent(task.NextStepTaskComponentId, task.ParentServiceId);
                }
            }
            else if (task.TaskTemplateType == TaskTypeEnum.StepTask)
            {
                switch (task.TaskStatusCode)
                {
                    case "TASK_STATUS_COMPLETE":
                        await ManageStepTaskComponentComplete(task);
                        break;
                    case "TASK_STATUS_REJECT":
                        await ManageStepTaskComponentReject(task);
                        break;
                    case "TASK_STATUS_CANCEL":
                        await ManageStepTaskComponentCancel(task);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task ManageStepTaskComponentCancel(TaskTemplateViewModel task)
        {
            var componentResult = await _repo.GetSingle(x => x.NtsTaskId == task.TaskId);
            if (task.IsReturned)
            {
                await UpdateComponentResultStatus(componentResult, "COMPONENT_STATUS_CANCELED");
                await UpdateService(componentResult, "COMPONENT_STATUS_CANCELED", task);
                await GetPreviousTask(componentResult.ComponentId, task);
            }
            else
            {
                await UpdateComponentResultStatus(componentResult, "COMPONENT_STATUS_CANCELED");
                await UpdateService(componentResult, "COMPONENT_STATUS_CANCELED", task);
            }

        }

        private async Task UpdateService(ComponentResultViewModel componentResult, string statusCode = "COMPONENT_STATUS_COMPLETED", TaskTemplateViewModel task = null)
        {
            var resultList = await GetComponentResultList(componentResult.NtsServiceId);
            if (!resultList.Any(x => x.ComponentStatusCode == "COMPONENT_STATUS_INPROGRESS"))
            {
                var sb = _serviceProvider.GetService<IServiceBusiness>();
                var service = await sb.GetServiceDetails(new ServiceTemplateViewModel
                {
                    ServiceId = componentResult.NtsServiceId,
                    ActiveUserId = componentResult.LastUpdatedBy
                });
                service.DataAction = DataActionEnum.Edit;
                service.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";
                if (statusCode == "COMPONENT_STATUS_CANCELED" || statusCode == "COMPONENT_STATUS_REJECTED")
                {
                    service.ServiceStatusCode = "SERVICE_STATUS_CANCEL";
                    if (task != null && task.IsReturned)
                    {
                        service.ReturnReason = task.ReturnReason;
                        service.ReturnedDate = task.ReturnedDate ?? DateTime.Now;
                    }
                }
                await sb.ManageService(service);
            }
        }

        private async Task ManageStepTaskComponentReject(TaskTemplateViewModel task)
        {
            var componentResult = await _repo.GetSingle(x => x.NtsTaskId == task.TaskId);
            await UpdateComponentResultStatus(componentResult, "COMPONENT_STATUS_REJECTED");
            await UpdateService(componentResult, "COMPONENT_STATUS_REJECTED");

        }
        private async Task GetPreviousTask(string componentId, TaskTemplateViewModel task)
        {

            var taskBusiness = (ITaskBusiness)_serviceProvider.GetService(typeof(ITaskBusiness));
            var serviceBusiness = (IServiceBusiness)_serviceProvider.GetService(typeof(IServiceBusiness));

            var ComponentParent = await _repo.GetSingle<ComponentParentViewModel, ComponentParent>(x => x.ComponentId == componentId);
            if (ComponentParent != null)
            {
                var Component = await _repo.GetSingle<ComponentViewModel, Component>(x => x.Id == ComponentParent.ParentId);

                if (Component.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                {
                    var stepTaskComp = await GetSingle(x => x.ComponentId == Component.Id && x.NtsServiceId == task.ParentServiceId);
                    if (stepTaskComp != null)
                    {
                        var pretask = await taskBusiness.GetTaskDetails(new TaskTemplateViewModel
                        {
                            TaskId = stepTaskComp.NtsTaskId,
                            DataAction = DataActionEnum.Read,
                            TaskTemplateType = TaskTypeEnum.StepTask

                        });
                        pretask.DataAction = DataActionEnum.Edit;
                        pretask.VersionNo = pretask.VersionNo + 1;
                        pretask.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        pretask.AllowPastStartDate = true;

                        var service = await serviceBusiness.GetSingleById(task.ParentServiceId);
                        var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Id == service.TemplateId);
                        var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == template.UdfTableMetadataId);
                        var dt = new DataTable();
                        dt = await serviceBusiness.GetServiceDataTableById(service.Id, tableMetaData);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            var dict = dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
                            var j = JsonConvert.SerializeObject(dict);
                            pretask.Json = j;
                        }

                        await taskBusiness.ManageTask(pretask);
                    }
                    return;

                }
                else
                {
                    await GetPreviousTask(Component.Id, task);
                }
            }
            else
            {

                var service = await serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
                {
                    ServiceId = task.ParentServiceId,
                    DataAction = DataActionEnum.Read,

                });
                service.DataAction = DataActionEnum.Edit;
                service.VersionNo = service.VersionNo + 1;
                service.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                service.AllowPastStartDate = true;
                service.IsReopened = true;

                var serviceT = await serviceBusiness.GetSingleById(task.ParentServiceId);
                var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Id == serviceT.TemplateId);
                var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == template.UdfTableMetadataId);
                var dt = new DataTable();
                dt = await serviceBusiness.GetServiceDataTableById(serviceT.Id, tableMetaData);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var dr = dt.Rows[0];
                    var dict = dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
                    var j = JsonConvert.SerializeObject(dict);
                    service.Json = j;
                }
                if (task != null && task.IsReturned)
                {
                    service.ReturnReason = task.ReturnReason;
                    service.ReturnedDate = task.ReturnedDate ?? DateTime.Now;
                }
                await serviceBusiness.ManageService(service);

            }
        }
        private async Task ManageStepTaskComponentComplete(TaskTemplateViewModel task)
        {
            var componentResult = await _repo.GetSingle(x => x.NtsTaskId == task.TaskId);
            await UpdateComponentResultStatus(componentResult);
            if (componentResult.RuntimeWorkflowDataId.IsNotNullAndNotEmpty())
            {
                var runtimeWorkflowData = await _repo.GetSingleById<RuntimeWorkflowDataViewModel, RuntimeWorkflowData>
                    (componentResult.RuntimeWorkflowDataId);
                if (runtimeWorkflowData != null)
                {
                    var runtimeWorkflow = await _repo.GetSingleById<RuntimeWorkflowViewModel, RuntimeWorkflow>
                    (runtimeWorkflowData.RuntimeWorkflowId);
                    if (runtimeWorkflow != null
                        && runtimeWorkflow.RuntimeWorkflowExecutionMode == WorkflowExecutionModeEnum.Sequential)
                    {
                        var workflowDataList = await _repo.GetList<RuntimeWorkflowDataViewModel, RuntimeWorkflowData>
                         (x => x.RuntimeWorkflowId == runtimeWorkflow.Id);
                        var nextWorkflowData = workflowDataList.Where
                            (x => x.SequenceOrder > runtimeWorkflowData.SequenceOrder)
                            .OrderBy(x => x.SequenceOrder).FirstOrDefault();
                        if (nextWorkflowData != null)
                        {
                            var nextCompResult = await _repo.GetSingle(x => x.RuntimeWorkflowDataId == nextWorkflowData.Id);
                            if (nextCompResult != null)
                            {
                                var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                                await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(nextCompResult.Id, _repo.UserContext.ToIdentityUser()));
                            }
                        }
                    }
                }

            }
            var childComponents = await GetChildComponentResultList(componentResult);
            if (childComponents.Count > 0)
            {
                foreach (var item in childComponents)
                {
                    var parents = await GetParentList(item);
                    if (!parents.Any(x => x.ComponentStatusCode != "COMPONENT_STATUS_COMPLETED"))
                    {
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));
                    }

                }
            }
            else
            {
                await UpdateService(componentResult, "COMPONENT_STATUS_COMPLETED");
            }
        }


        private async Task<List<ComponentResultViewModel>> GetChildComponentResultList(ComponentResultViewModel componentResult)
        {
            return await _cmsQueryBusiness.GetChildComponentResultListData(componentResult);

        }
        private async Task<List<ComponentResultViewModel>> GetParentList(ComponentResultViewModel componentResult)
        {
            var ntsServiceId = componentResult.NtsServiceId;
            var componentId = componentResult.ComponentId;
            return await _cmsQueryBusiness.GetParentListData(ntsServiceId, componentId);

        }


        private async Task SetStepTaskAssignee(TaskTemplateViewModel task, ServiceTemplateViewModel service, ComponentResultViewModel component, StepTaskComponentViewModel stepTaskComponent, RuntimeWorkflowDataViewModel runtimeData = null)
        {
            if (stepTaskComponent != null)
            {
                var customAssignee = await SetCustomAssignee(task, service, component, stepTaskComponent);
                if (customAssignee)
                {
                    return;
                }
                task.AssignedToTypeId = stepTaskComponent.AssignedToTypeId;
                task.AssignedToUserId = stepTaskComponent.AssignedToUserId;
                task.AssignedToTeamId = stepTaskComponent.AssignedToTeamId;
                if (runtimeData != null)
                {
                    task.AssignedToTypeId = runtimeData.AssignedToTypeId;
                    task.AssignedToUserId = runtimeData.AssignedToUserId;
                    task.AssignedToTeamId = runtimeData.AssignedToTeamId;
                }
                var assignToType = await _repo.GetSingleById<LOVViewModel, LOV>(stepTaskComponent.AssignedToTypeId);

                if (assignToType != null && assignToType.Code == "TASK_ASSIGN_TO_USER_HIERARCHY")
                {
                    var userHierarchy = await _userHierarchyBusiness.GetList(x => x.UserId == service.OwnerUserId && x.HierarchyMasterId == stepTaskComponent.AssignedToHierarchyMasterId);
                    if (userHierarchy != null && userHierarchy.Count() > 0)
                    {
                        foreach (var userherirchy in userHierarchy)
                        {
                            if (userherirchy.LevelNo == stepTaskComponent.AssignedToHierarchyMasterLevelId)
                            {
                                if (userherirchy.OptionNo == 1)
                                {
                                    if (userherirchy.ParentUserId == null)
                                    {
                                        if (userherirchy.OptionNo == 2)
                                        {
                                            if (userherirchy.ParentUserId == null)
                                            {
                                                if (userherirchy.OptionNo == 3)
                                                {
                                                    if (userherirchy.ParentUserId == null)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        task.AssignedToUserId = userherirchy.ParentUserId;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                task.AssignedToUserId = userherirchy.ParentUserId;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        task.AssignedToUserId = userherirchy.ParentUserId;
                                    }
                                }
                            }

                        }

                    }
                }
            }
        }

        private async Task<bool> SetCustomAssignee(TaskTemplateViewModel taskTemplate, ServiceTemplateViewModel serviceTemplate, ComponentResultViewModel componentResult, StepTaskComponentViewModel stepTaskComponent)
        {
            try
            {
                var assigneeLogic = await _repo.GetList<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(x => x.StepTaskComponentId == stepTaskComponent.Id);
                if (assigneeLogic != null && assigneeLogic.Any())
                {
                    var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(serviceTemplate.UdfTableMetadataId);
                    var serviceBusiness = (IServiceBusiness)_serviceProvider.GetService(typeof(IServiceBusiness));
                    var selectQuery = await serviceBusiness.GetSelectQuery(tableMetaData, @$" and ""NtsService"".""Id""='{ componentResult.NtsServiceId}' limit 1");
                    dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                    foreach (var item in assigneeLogic.OrderBy(x => x.SequenceOrder).ToList())
                    {
                        var rules = await _repo.GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.ReferenceId == item.Id);
                        foreach (var rule in rules)
                        {
                            if (rule != null && rule.BusinessRuleType == BusinessRuleTypeEnum.Operational)
                            {
                                dynamic inputData = new { Data = dobject, Context = _repo.UserContext };
                                var text = Helper.ExecuteBreLogic<string>(rule.OperationBackendValue, inputData, null);
                                text = @$"#if({text})
            true#else
            false#end";
                                var result = Helper.ExecuteBreLogic<bool>(text, inputData, null);
                                if (result)
                                {
                                    taskTemplate.AssignedToTypeId = item.AssignedToTypeId;
                                    taskTemplate.AssignedToTeamId = item.AssignedToTeamId;
                                    taskTemplate.AssignedToUserId = item.AssignedToUserId;
                                    return true;
                                }
                            }
                        }


                    }
                }
            }
            catch (Exception)
            {


            }
            return false;
        }

        private async Task SetNextStepTaskAssignee(TaskTemplateViewModel task, ServiceTemplateViewModel service, TaskViewModel previoustask)
        {
            if (previoustask != null)
            {
                task.AssignedToTypeId = previoustask.NextTaskAssignedToTypeId;
                task.AssignedToUserId = previoustask.NextTaskAssignedToUserId;
                task.AssignedToTeamId = previoustask.NextTaskAssignedToTeamId;
                var assignToType = await _repo.GetSingleById<LOVViewModel, LOV>(previoustask.NextTaskAssignedToTypeId);

                if (assignToType != null && assignToType.Code == "TASK_ASSIGN_TO_USER_HIERARCHY")
                {
                    var userHierarchy = await _userHierarchyBusiness.GetList(x => x.UserId == service.OwnerUserId && x.HierarchyMasterId == previoustask.NextTaskAssignedToHierarchyMasterId);
                    if (userHierarchy != null && userHierarchy.Count() > 0)
                    {
                        foreach (var userherirchy in userHierarchy)
                        {
                            if (userherirchy.LevelNo == previoustask.NextTaskAssignedToHierarchyMasterLevelId)
                            {
                                if (userherirchy.OptionNo == 1)
                                {
                                    if (userherirchy.ParentUserId == null)
                                    {
                                        if (userherirchy.OptionNo == 2)
                                        {
                                            if (userherirchy.ParentUserId == null)
                                            {
                                                if (userherirchy.OptionNo == 3)
                                                {
                                                    if (userherirchy.ParentUserId == null)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        task.AssignedToUserId = userherirchy.ParentUserId;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                task.AssignedToUserId = userherirchy.ParentUserId;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        task.AssignedToUserId = userherirchy.ParentUserId;
                                    }
                                }
                            }

                        }

                    }
                }
            }
        }


        private async Task<string> GetComponentStatus(string statusCode)
        {
            var lov = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "COMPONENT_STATUS" && x.Code == statusCode);
            if (lov != null)
            {
                return lov.Id;
            }
            return null;
        }

        public async Task InitiateAllComponentsByProcessDesign(string processDesignId)
        {
            throw new NotImplementedException();
        }

        public async Task ProcessAllOpenComponents()
        {
            throw new NotImplementedException();
        }
        public async Task<List<ComponentResultViewModel>> GetComponentResultList(string serviceId)
        {
            var queryData = await _cmsQueryBusiness.GetComponentResultListData(serviceId);
            foreach (var data in queryData)
            {
                if (data.ComponentType == ProcessDesignComponentTypeEnum.StepTask && data.ComponentStatusName == "Draft")
                {
                    var userBusiness = (IUserBusiness)_serviceProvider.GetService(typeof(IUserBusiness));
                    var serviceData = await _repo.GetSingleById<ServiceViewModel, NtsService>(data.NtsServiceId);
                    var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.ComponentId == data.ComponentId, x => x.TaskTemplate);

                    if (stepTaskComponent != null)
                    {
                        var assignToType = await _repo.GetSingleById<LOVViewModel, LOV>(stepTaskComponent.AssignedToTypeId);
                        if (assignToType != null && assignToType.Code == "TASK_ASSIGN_TO_USER_HIERARCHY")
                        {
                            var userHierarchy = await _userHierarchyBusiness.GetList(x => x.UserId == serviceData.OwnerUserId && x.HierarchyMasterId == stepTaskComponent.AssignedToHierarchyMasterId);
                            if (userHierarchy != null && userHierarchy.Count() > 0)
                            {
                                foreach (var userherirchy in userHierarchy)
                                {
                                    if (userherirchy.LevelNo == stepTaskComponent.AssignedToHierarchyMasterLevelId)
                                    {
                                        if (userherirchy.OptionNo == 1)
                                        {
                                            if (userherirchy.ParentUserId == null)
                                            {
                                                if (userherirchy.OptionNo == 2)
                                                {
                                                    if (userherirchy.ParentUserId == null)
                                                    {
                                                        if (userherirchy.OptionNo == 3)
                                                        {
                                                            if (userherirchy.ParentUserId == null)
                                                            {
                                                            }
                                                            else
                                                            {
                                                                var user = await userBusiness.GetSingleById(userherirchy.ParentUserId);
                                                                data.Assignee = user.Name;
                                                                data.AssigneeId = user.Id;
                                                                data.Email = user.Email;
                                                                data.AssigneePhotoId = user.PhotoId;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //task.AssignedToUserId = userherirchy.ParentUserId;
                                                        var user = await userBusiness.GetSingleById(userherirchy.ParentUserId);
                                                        data.Assignee = user.Name;
                                                        data.AssigneeId = user.Id;
                                                        data.Email = user.Email;
                                                        data.AssigneePhotoId = user.PhotoId;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //  task.AssignedToUserId = userherirchy.ParentUserId;
                                                var user = await userBusiness.GetSingleById(userherirchy.ParentUserId);
                                                data.Assignee = user.Name;
                                                data.AssigneeId = user.Id;
                                                data.Email = user.Email;
                                                data.AssigneePhotoId = user.PhotoId;
                                            }
                                        }
                                    }

                                }

                            }
                        }
                        else if (assignToType != null && assignToType.Code == "TASK_ASSIGN_TO_TEAM")
                        {
                            var users = await _repo.GetList<TeamUserViewModel, TeamUser>(x => x.TeamId == stepTaskComponent.AssignedToTeamId && x.IsTeamOwner == true);
                            if (users != null)
                            {
                                var user = await userBusiness.GetSingleById(users.Select(x => x.Id).FirstOrDefault());
                            }
                        }
                        else
                        {
                            var user = await userBusiness.GetSingleById(stepTaskComponent.AssignedToUserId);
                            data.Assignee = user.Name;
                            data.AssigneeId = user.Id;
                            data.Email = user.Email;
                            data.AssigneePhotoId = user.PhotoId;
                        }
                    }
                }

            }
            return queryData;
        }
        public async Task<List<TaskViewModel>> GetStepTaskList(string serviceId)
        {
            var queryData = await _cmsQueryBusiness.GetStepTaskListData(serviceId);
            return queryData;
        }
        public async Task<List<TemplateViewModel>> GetStepTaskTemplateList(string serviceTemplateId)
        {
            var queryData = await _cmsQueryBusiness.GetStepTaskTemplateListData(serviceTemplateId);
            return queryData;
        }

        public async Task<List<NtsEmailViewModel>> GetNtsEmailTree(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string departmentId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //var list = await _cmsQueryBusiness.GetNtsEmailList(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes, null, null, null, departmentId, fromDate, toDate);

            var dlist = await GetNtsEmailList(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes, null, null, null, null, departmentId, fromDate, toDate);

            var list = dlist.DistinctBy(x => x.TargetId);
            var emailTree = new List<NtsEmailViewModel>();
            var inboxCount = list.Where(x => x.EmailType == EmailTypeEnum.Inbox).Count();
            emailTree.Add(new NtsEmailViewModel
            {
                Count = inboxCount,
                text = $"Incoming({inboxCount})",
                id = "INBOX",
                TemplateCode = "INBOX",
                EmailType = EmailTypeEnum.Inbox,
                parent = "#",
                icon = "fa fa-inbox-in text-dark",
                TargetId = "INBOX",
                a_attr = new { data_id = "INBOX", data_name = "Incoming", data_type = EmailTypeEnum.Inbox.ToString(), email_type = "Inbox", data_catid = "", data_tempid = "" },
            }
           );
            var outBoxCount = list.Where(x => x.EmailType == EmailTypeEnum.Outbox).Count();
            emailTree.Add(new NtsEmailViewModel
            {
                Count = outBoxCount,
                text = $"Outgoing({outBoxCount})",
                id = "OUTBOX",
                TemplateCode = "OUTBOX",
                EmailType = EmailTypeEnum.Outbox,
                parent = "#",
                icon = "fa fa-inbox-out text-dark",
                TargetId = "OUTBOX",
                a_attr = new { data_id = "OUTBOX", data_name = "Outgoing", data_type = EmailTypeEnum.Outbox.ToString(), email_type = "Outbox", data_catid = "", data_tempid = "" },
            }
            );
            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Inbox)
                .GroupBy(x => x.CategoryId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = x.CategoryId,
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.CategoryName}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId)})",
                    IconCss = x.IconCss,
                    parent = "INBOX",
                    EmailType = x.EmailType,
                    icon = "fa fa-file-alt text-dark",
                    ActualStatus = x.ActualStatus,
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.CategoryCode, data_name = x.CategoryName, data_type = EmailTypeEnum.Inbox.ToString(), email_type = "Category", data_catid = x.CategoryId, data_tempid = "", cat_name = x.CategoryName },
                }).Distinct().ToList());
            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Outbox)
                .GroupBy(x => x.CategoryId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = x.CategoryId,
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.CategoryName}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId)})",
                    IconCss = x.IconCss,
                    parent = "OUTBOX",
                    EmailType = x.EmailType,
                    icon = "fa fa-file-alt text-dark",
                    ActualStatus = x.ActualStatus,
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.CategoryCode, data_name = x.CategoryName, data_type = EmailTypeEnum.Outbox.ToString(), email_type = "Category", data_catid = x.CategoryId, data_tempid = "", cat_name = x.CategoryName },
                }).Distinct().ToList());

            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Inbox)
                .GroupBy(x => x.TemplateId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = x.TemplateId,
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.TemplateName}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId)})",
                    IconCss = x.IconCss,
                    parent = x.CategoryId,
                    EmailType = x.EmailType,
                    icon = "fa fa-file-alt text-dark",
                    ActualStatus = x.ActualStatus,
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.TemplateCode, data_name = x.TemplateName, data_type = EmailTypeEnum.Inbox.ToString(), email_type = "Template", data_status = x.ActualStatus, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },
                }).Distinct().ToList());

            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Inbox && x.InboxStatus == EmailInboxTypeEnum.Drafted)
              .GroupBy(x => x.TemplateId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
              {
                  id = $"D_{x.TemplateId}",
                  TemplateId = x.TemplateId,
                  TemplateCode = x.TemplateCode,
                  CategoryId = x.CategoryId,
                  CategoryCode = x.CategoryCode,
                  text = $"{x.InboxStatus.ToString()}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId && y.InboxStatus == x.InboxStatus)})",
                  IconCss = x.IconCss,
                  parent = x.TemplateId,
                  EmailType = x.EmailType,
                  ActualStatus = x.ActualStatus,
                  ServiceId = x.ServiceId,
                  icon = "fa fa-file-alt text-dark",
                  TargetType = x.TargetType,
                  TargetId = x.TargetId,
                  a_attr = new { data_id = x.InboxStatus.ToString(), data_name = x.InboxStatus.ToString(), data_type = EmailTypeEnum.Inbox.ToString(), email_type = "Status", data_status = x.ActualStatus, data_tempcode = x.TemplateCode, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },
              }).Distinct().ToList());
            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Inbox && x.InboxStatus == EmailInboxTypeEnum.Pending)
               .GroupBy(x => x.TemplateId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
               {
                   id = $"P_{x.TemplateId}",
                   TemplateId = x.TemplateId,
                   TemplateCode = x.TemplateCode,
                   CategoryId = x.CategoryId,
                   CategoryCode = x.CategoryCode,
                   text = $"{x.InboxStatus.ToString()}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId && y.InboxStatus == x.InboxStatus)})",
                   IconCss = x.IconCss,
                   parent = x.TemplateId,
                   EmailType = x.EmailType,
                   ActualStatus = x.ActualStatus,
                   ServiceId = x.ServiceId,
                   icon = "fa fa-file-alt text-dark",
                   TargetType = x.TargetType,
                   TargetId = x.TargetId,
                   a_attr = new { data_id = x.InboxStatus.ToString(), data_name = x.InboxStatus.ToString(), data_type = EmailTypeEnum.Inbox.ToString(), email_type = "Status", data_status = x.ActualStatus, data_tempcode = x.TemplateCode, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },
               }).Distinct().ToList());
            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Inbox && x.InboxStatus == EmailInboxTypeEnum.Completed)
                .GroupBy(x => x.TemplateId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = $"C_{x.TemplateId}",
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.InboxStatus.ToString()}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId && y.InboxStatus == x.InboxStatus)})",
                    IconCss = x.IconCss,
                    parent = x.TemplateId,
                    EmailType = EmailTypeEnum.Inbox,
                    ActualStatus = x.ActualStatus,
                    ServiceId = x.ServiceId,
                    icon = "fa fa-file-alt text-dark",
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.InboxStatus.ToString(), data_name = x.InboxStatus.ToString(), data_type = EmailTypeEnum.Inbox.ToString(), email_type = "Status", data_status = x.ActualStatus, data_tempcode = x.TemplateCode, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },
                }).Distinct().ToList());

            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Outbox)
                .GroupBy(x => x.TemplateId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = x.TemplateId,
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.TemplateName}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId)})",
                    IconCss = x.IconCss,
                    parent = x.CategoryId,
                    EmailType = x.EmailType,
                    icon = "fa fa-file-alt text-dark",
                    ActualStatus = x.ActualStatus,
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.TemplateCode, data_name = x.TemplateName, data_type = EmailTypeEnum.Outbox.ToString(), email_type = "Template", data_status = x.ActualStatus, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },
                }).Distinct().ToList());
            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Outbox && x.InboxStatus == EmailInboxTypeEnum.Drafted)
                .GroupBy(x => new { x.TemplateId }).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = $"D_{x.TemplateId}",
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.InboxStatus.ToString()}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId && y.InboxStatus == x.InboxStatus)})",
                    IconCss = x.IconCss,
                    parent = x.TemplateId,
                    EmailType = x.EmailType,
                    ActualStatus = x.ActualStatus,
                    ServiceId = x.ServiceId,
                    icon = "fa fa-file-alt text-dark",
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.InboxStatus.ToString(), data_name = x.InboxStatus.ToString(), data_type = EmailTypeEnum.Outbox.ToString(), email_type = "Status", data_status = x.ActualStatus, data_tempcode = x.TemplateCode, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },
                }).Distinct().ToList());
            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Outbox && x.InboxStatus == EmailInboxTypeEnum.Pending)
                .GroupBy(x => x.TemplateId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = $"P_{x.TemplateId}",
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.InboxStatus.ToString()}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId && y.InboxStatus == x.InboxStatus)})",
                    IconCss = x.IconCss,
                    parent = x.TemplateId,
                    EmailType = x.EmailType,
                    ActualStatus = x.ActualStatus,
                    ServiceId = x.ServiceId,
                    icon = "fa fa-file-alt text-dark",
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.InboxStatus.ToString(), data_name = x.InboxStatus.ToString(), data_type = EmailTypeEnum.Outbox.ToString(), email_type = "Status", data_status = x.ActualStatus, data_tempcode = x.TemplateCode, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },
                }).Distinct().ToList());

            emailTree.AddRange(list.Where(x => x.EmailType == EmailTypeEnum.Outbox && x.InboxStatus == EmailInboxTypeEnum.Completed)
                .GroupBy(x => x.TemplateId).Select(x => x.FirstOrDefault()).Select(x => new NtsEmailViewModel
                {
                    id = $"C_{x.TemplateId}",
                    TemplateId = x.TemplateId,
                    TemplateCode = x.TemplateCode,
                    CategoryId = x.CategoryId,
                    CategoryCode = x.CategoryCode,
                    text = $"{x.InboxStatus.ToString()}({list.Count(y => y.EmailType == x.EmailType && y.CategoryId == x.CategoryId && y.TemplateId == x.TemplateId && y.InboxStatus == x.InboxStatus)})",
                    IconCss = x.IconCss,
                    parent = x.TemplateId,
                    EmailType = x.EmailType,
                    ActualStatus = x.ActualStatus,
                    ServiceId = x.ServiceId,
                    icon = "fa fa-file-alt text-dark",
                    TargetType = x.TargetType,
                    TargetId = x.TargetId,
                    a_attr = new { data_id = x.InboxStatus.ToString(), data_name = x.InboxStatus.ToString(), data_type = EmailTypeEnum.Outbox.ToString(), email_type = "Status", data_status = x.ActualStatus, data_tempcode = x.TemplateCode, data_catid = x.CategoryId, data_tempid = x.TemplateId, cat_name = x.CategoryName, temp_name = x.TemplateName },

                }).Distinct().ToList());
            return emailTree;
        }

        public async Task<List<NtsEmailViewModel>> GetNtsEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string categoryId = null, string templateId = null, EmailTypeEnum? emailType = null, EmailInboxTypeEnum? inboxStatus = null, string departmentId = null, DateTime? fromDate = null, DateTime? toDate = null, NtsEmailTargetTypeEnum? targetType1 = null, NtsEmailTargetTypeEnum? targetType2 = null, string wfStatus = null, SLATypeEnum? slaType = null)
        {

            var list = await _cmsQueryBusiness.GetNtsEmailList(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes, categoryId, templateId, null, departmentId, fromDate, toDate);
            if (emailType.HasValue)
            {
                list = list.Where(x => x.EmailType == emailType).ToList();
            }
            if (inboxStatus.HasValue)
            {
                list = list.Where(x => x.InboxStatus == inboxStatus).ToList();
            }
            if (departmentId.IsNotNullAndNotEmpty())
            {
                list = list.Where(x => x.MessageUserDepartmentId == departmentId).ToList();
            }
            if (fromDate.HasValue)
            {
                list = list.Where(x => x.StartDate.Value.Date >= fromDate.Value).ToList();
            }
            if (toDate.HasValue)
            {
                list = list.Where(x => x.StartDate.Value.Date <= toDate.Value).ToList();
            }
            return list;
        }
        public async Task<List<NtsEmailViewModel>> GetNtsEmailDetails(string serviceId, string targetId, NtsEmailTargetTypeEnum? targetType)
        {

            var data = await _cmsQueryBusiness.GetNtsEmailList(null, null, null, null, null, null, null, null, null, serviceId);
            //data = data.Where(x => !(x.TargetType == NtsEmailTargetTypeEnum.Service && x.ServicePlusId != null)).ToList();
            var list = new List<NtsEmailViewModel>();
            var item = data.FirstOrDefault(x => x.TargetId == targetId);
            if (item != null)
            {
                item.CanReply = true;
                var so = 1;
                PrepareEmailList(data, list, item, so);
            }
            list.ForEach(x => x.SequenceOrderText = $"{list.Count() - x.SequenceOrder + 1} of {list.Count()}");
            return list;
        }

        private void PrepareEmailList(List<NtsEmailViewModel> data, List<NtsEmailViewModel> list, NtsEmailViewModel item, int so)
        {
            item.SequenceOrder = so;
            list.Add(item);
            var child = data.FirstOrDefault(x => x.TargetId == item.ParentId);
            if (child != null)
            {
                if (child.TargetType == NtsEmailTargetTypeEnum.Service && child.ServicePlusId != null)
                {
                    child = data.FirstOrDefault(x => x.TargetId == child.ParentId);
                    if (child != null)
                    {
                        PrepareEmailList(data, list, child, ++so);
                    }
                }
                else
                {
                    PrepareEmailList(data, list, child, ++so);
                }
            }
        }

        public async Task<List<AssignmentViewModel>> GetUserListOnNtsBasis(string id, NtsTypeEnum type)
        {
            if (type == NtsTypeEnum.Service)
            {
                var list = new List<AssignmentViewModel>();
                list = await _cmsQueryBusiness.GetUserListOnNtsBasisData(id);
                var componentlist = await GetComponentResultList(id);
                if (componentlist != null && componentlist.Count() > 0)
                {
                    foreach (var comp in componentlist)
                    {
                        if (comp.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                        {
                            if (!(list.Any(x => x.UserId == comp.AssigneeId)))
                            {
                                var list1 = await _cmsQueryBusiness.GetUserListOnNtsBasisData1(comp.AssigneeId);
                                list.AddRange(list1);
                            }
                        }
                    }

                }
                return list;
            }
            if (type == NtsTypeEnum.Task)
            {

                return await _cmsQueryBusiness.GetUserListOnNtsBasisData2(id);

            }
            else
            {

                return await _cmsQueryBusiness.GetUserListOnNtsBasisData3(id);
            }
        }

        public async Task EscalateTask()
        {
            try
            {
                var data = await _cmsQueryBusiness.GetOverDueTaskListForEscalation();
                foreach (var item in data)
                {
                    var existingEscalation = await _repo.GetSingle<StepTaskEscalationDataViewModel, StepTaskEscalationData>(x => x.StepTaskEscalationId == item.Id && x.NtsTaskId == item.TaskId);
                    if (existingEscalation == null)
                    {
                        var stepTaskEscalationData = new StepTaskEscalationDataViewModel
                        {
                            StepTaskEscalationId = item.Id,
                            StepTaskComponentId = item.StepTaskComponentId,
                            NtsTaskId = item.TaskId,
                            NtsServiceId = item.ParentServiceId,
                            EscalatedToUserId = item.AssignedToUserId,
                            EscalatedDate = DateTime.Now
                        };
                        var res = await base.Create<StepTaskEscalationDataViewModel, StepTaskEscalationData>(stepTaskEscalationData);

                        if (res.IsSuccess)
                        {
                            var _notificationTemplateBusiness = _serviceProvider.GetService<INotificationTemplateBusiness>();
                            var _notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
                            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();


                            var notificationEscalatedTemp = await _notificationTemplateBusiness.GetSingleById(item.EscalatedToNotificationTemplateId);
                            var taskModel = await _taskBusiness.GetSingleById(item.TaskId);

                            var notificationToEscalatedUser = new NotificationViewModel()
                            {
                                DataAction = DataActionEnum.Create,
                                //To = res.Item.EscalatedToUserId,
                                ToUserId = res.Item.EscalatedToUserId,
                                //FromUserId = model.CreatedBy,
                                Subject = notificationEscalatedTemp.Subject,
                                Body = notificationEscalatedTemp.Body,
                                SendAlways = true,
                                NotifyByEmail = true,
                                ReferenceType = ReferenceTypeEnum.NTS_Task,
                                ReferenceTypeId = item.TaskId,
                                //ReferenceTypeNo = viewModel.TaskNo,
                                DynamicObject = taskModel,
                                //PortalId = 

                            };
                            await _notificationBusiness.Create(notificationToEscalatedUser);


                            var notificationTemp = await _notificationTemplateBusiness.GetSingleById(item.NotificationTemplateId);

                            var notificationToTaskAssignee = new NotificationViewModel()
                            {
                                DataAction = DataActionEnum.Create,
                                ToUserId = taskModel.AssignedToUserId,
                                Subject = notificationTemp.Subject,
                                Body = notificationTemp.Body,
                                SendAlways = true,
                                NotifyByEmail = true,
                                ReferenceType = ReferenceTypeEnum.NTS_Task,
                                ReferenceTypeId = item.TaskId,
                                DynamicObject = taskModel,

                            };
                            await _notificationBusiness.Create(notificationToTaskAssignee);



                        }


                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
