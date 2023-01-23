using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
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


namespace CMS.Business
{
    public class ComponentResultBusiness : BusinessBase<ComponentResultViewModel, ComponentResult>, IComponentResultBusiness
    {
        private readonly IRepositoryQueryBase<ComponentParentViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDynamicScriptBusiness _dynamicScriptBusiness;
        private readonly IRepositoryQueryBase<ComponentResultViewModel> _queryData;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;

        public ComponentResultBusiness(IRepositoryBase<ComponentResultViewModel, ComponentResult> repo, IMapper autoMapper,
            IRepositoryQueryBase<ComponentParentViewModel> queryRepo
            , IServiceProvider serviceProvider
            , IDynamicScriptBusiness dynamicScriptBusiness,
             IRepositoryQueryBase<ComponentResultViewModel> queryData,
             IUserHierarchyBusiness userHierarchyBusiness
             ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceProvider = serviceProvider;
            _dynamicScriptBusiness = dynamicScriptBusiness;
            _queryData = queryData;
            _userHierarchyBusiness = userHierarchyBusiness;
            // _serviceBusiness = serviceBusiness;
            //  _taskBusiness = taskBusiness;
            // _noteBusiness = noteBusiness;
            //  _tableMetadataBusiness = tableMetadataBusiness;
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
                             BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id,_repo.UserContext.ToIdentityUser()));
                            //await ExecuteComponent(item.Id);
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
                        BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));

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
                        BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(trueComponent.Id, _repo.UserContext.ToIdentityUser()));
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
                        BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(falseComponent.Id, _repo.UserContext.ToIdentityUser()));
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

        private async Task ExecuteStepTaskComponent(ComponentResultViewModel componentResult)
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
            var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.ComponentId == componentResult.ComponentId, x => x.TaskTemplate);
            // Any task with this step task Component(if yess add version)
            var Ntstask = await taskBusiness.GetSingle(x => x.StepTaskComponentId == stepTaskComponent.Id && x.ParentServiceId== componentResult.NtsServiceId);
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

            await SetStepTaskAssignee(task, service, componentResult, stepTaskComponent);
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
        public async Task ExecuteDynamicStepTaskComponent(string stepcomponentId,string serviceId)
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
        private async Task GetNextTaskAssignee(string componentId,TaskTemplateViewModel task,ServiceTemplateViewModel service)
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
                    var stepTaskComp = await GetSingle(x => x.ComponentId == Component.Id && x.NtsServiceId==service.ServiceId);
                    if (stepTaskComp != null)
                    {
                        var pretask = await taskBusiness.GetSingleById(stepTaskComp.NtsTaskId);
                        if (pretask.NextTaskAssignedToTypeId.IsNotNullAndNotEmpty())
                        {
                            await SetNextStepTaskAssignee(task, service, pretask);
                            task.TaskId = Guid.NewGuid().ToString();
                            if (pretask.NextTaskAttachmentId.IsNotNullAndNotEmpty())
                            {
                                var attach = await fileBusiness.GetSingleById(pretask.NextTaskAttachmentId);
                                if(attach!=null)
                                {
                                    attach.ReferenceTypeCode = ReferenceTypeEnum.NTS_Task;
                                    attach.ReferenceTypeId = task.TaskId;
                                    attach.Id = null;
                                    await fileBusiness.Create(attach);
                                }
                            }
                        }
                        if(pretask.CompleteReason.IsNotNullAndNotEmpty())
                        {
                            var cmt = new NtsTaskCommentViewModel
                            {
                                Comment = pretask.CompleteReason,
                                NtsTaskId = task.TaskId,
                                CommentedByUserId =pretask.AssignedToUserId,
                                CommentedTo=CommentToEnum.All,
                                DataAction=DataActionEnum.Create,

                            };
                            await commentBusiness.Create(cmt);
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
                        BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id, _repo.UserContext.ToIdentityUser()));

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
                        BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id,_repo.UserContext.ToIdentityUser()));

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
            if(task.NextStepTaskComponentId.IsNotNullAndNotEmpty())
            {
                if (task.NextStepTaskComponentId== "SERVICE_COMPLETE")
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
                else {
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
                await GetPreviousTask(componentResult.ComponentId, task);
            }
            else
            {
                await UpdateComponentResultStatus(componentResult, "COMPONENT_STATUS_CANCELED");
                await UpdateService(componentResult, "COMPONENT_STATUS_CANCELED");
            }

        }

        private async Task UpdateService(ComponentResultViewModel componentResult, string statusCode = "COMPONENT_STATUS_COMPLETED")
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

                    await serviceBusiness.ManageService(service);
                
            }
        }
        private async Task ManageStepTaskComponentComplete(TaskTemplateViewModel task)
        {
            var componentResult = await _repo.GetSingle(x => x.NtsTaskId == task.TaskId);
            await UpdateComponentResultStatus(componentResult);
            var childComponents = await GetChildComponentResultList(componentResult);
            if (childComponents.Count > 0)
            {
                foreach (var item in childComponents)
                {
                    var parents = await GetParentList(item);
                    if (!parents.Any(x => x.ComponentStatusCode != "COMPONENT_STATUS_COMPLETED"))
                    {
                        BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignComponent(item.Id,_repo.UserContext.ToIdentityUser()));
                        //await ExecuteComponent(item.Id);
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
            var query = $@"select cr.* from public.""ComponentResult"" as cr
            join public.""Component"" as c on cr.""ComponentId""=c.""Id"" and c.""IsDeleted""=false
            join public.""ComponentParent"" as cp on c.""Id""=cp.""ComponentId"" and cp.""IsDeleted""=false
            where  cr.""IsDeleted""=false  
            and cr.""NtsServiceId""='{componentResult.NtsServiceId}' and cp.""ParentId""='{componentResult.ComponentId}'";
            return await _queryRepo.ExecuteQueryList<ComponentResultViewModel>(query, null);

        }
        private async Task<List<ComponentResultViewModel>> GetParentList(ComponentResultViewModel componentResult)
        {
            var query = $@"select cr.*,lov.""Code"" as ""ComponentStatusCode"" from public.""ComponentResult"" as cr
            join public.""LOV"" as lov on cr.""ComponentStatusId""=lov.""Id"" and lov.""IsDeleted""=false
            join public.""Component"" as c on cr.""ComponentId""=c.""Id"" and c.""IsDeleted""=false
            join public.""ComponentParent"" as cp on c.""Id""=cp.""ParentId"" and cp.""IsDeleted""=false
            where  cr.""IsDeleted""=false  
            and cr.""NtsServiceId""='{componentResult.NtsServiceId}' and cp.""ComponentId""='{componentResult.ComponentId}'";
            return await _queryRepo.ExecuteQueryList<ComponentResultViewModel>(query, null);

        }


        private async Task SetStepTaskAssignee(TaskTemplateViewModel task, ServiceTemplateViewModel service, ComponentResultViewModel component, StepTaskComponentViewModel stepTaskComponent)
        {
            if (stepTaskComponent != null)
            {
                task.AssignedToTypeId = stepTaskComponent.AssignedToTypeId;
                task.AssignedToUserId = stepTaskComponent.AssignedToUserId;
                task.AssignedToTeamId = stepTaskComponent.AssignedToTeamId;
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
            string query = @$"select pl.*, vt.""Name"" as ComponentStatusName,u.""Name"" as Assignee,u.""Id"" as AssigneeId,u.""Email"" as Email,u.""PhotoId"" as AssigneePhotoId,temp.""Code"" as TemplateMasterCode 
            ,vt.""Code"" as ComponentStatusCode,t.""TaskNo"" as TaskNo,t.""Id"" as TaskId,s.""ServiceSubject"" as ServiceSubject
            FROM public.""ComponentResult"" as pl
            JOIN public.""LOV"" as vt ON vt.""Id"" = pl.""ComponentStatusId"" and vt.""IsDeleted"" = false
 left join public.""NtsService"" as s on s.""Id""=pl.""NtsServiceId"" and s.""IsDeleted"" = false
            left join public.""NtsTask"" as t on t.""Id""=pl.""NtsTaskId"" and t.""IsDeleted"" = false
left join public.""Template"" as temp on temp.""Id""=t.""TemplateId"" and temp.""IsDeleted"" = false
            left join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""TemplateId"" and tt.""IsDeleted"" = false
            left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted"" = false
            WHERE pl.""IsDeleted"" = false and pl.""NtsServiceId""='{serviceId}'";
            var queryData = await _queryRepo.ExecuteQueryList<ComponentResultViewModel>(query, null);
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
                                //data.Assignee = user.Name;
                                //data.AssigneeId = user.Id;
                                //data.Email = user.Email;
                                //data.AssigneePhotoId = user.PhotoId;
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
            string query = @$"select t.*,stc.""SequenceOrder"" as StepTaskSequenceOrder,lv.""Name"" as TaskStatusName,lv.""Code"" as TaskStatusCode,u.""Name"" as AssigneeUserName, p.""Name"" as PageName,p.""Id"" as PageId ,temp.""Code"" as TemplateMasterCode                                                          
            FROM public.""NtsTask"" as t
            join public.""Template"" as temp on temp.""Id""=t.""TemplateId"" and temp.""IsDeleted"" = false
            join public.""StepTaskComponent"" as stc on t.""StepTaskComponentId""=stc.""Id"" and stc.""IsDeleted""=false
            left join public.""LOV"" as lv on lv.""Id""=t.""TaskStatusId"" and lv.""IsDeleted"" = false
            left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted"" = false
            left join public.""Page"" as p on p.""TemplateId""=temp.""Id"" and p.""IsDeleted""=false
            WHERE t.""IsDeleted"" = false and t.""ParentServiceId""='{serviceId}' 
            and t.""TaskType""='2' and temp.""IsDeleted""=false order by stc.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);            
            return queryData;
        }
        public async Task<List<TemplateViewModel>> GetStepTaskTemplateList(string serviceTemplateId)
        {
            string query = @$"select t.* FROM  public.""Template"" as t 
            join public.""StepTaskComponent"" as stc on t.""Id""=stc.""TemplateId"" and stc.""IsDeleted"" = false
            join public.""Component"" as comp on comp.""Id""=stc.""ComponentId"" and comp.""IsDeleted"" = false
            join public.""ProcessDesign"" as p on p.""Id""=comp.""ProcessDesignId"" and p.""IsDeleted""=false
            WHERE t.""IsDeleted"" = false and p.""TemplateId""='{serviceTemplateId}' ";
            var queryData = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return queryData;
        }

        public async Task<List<AssignmentViewModel>> GetUserListOnNtsBasis(string id, NtsTypeEnum type)
        {
            if (type == NtsTypeEnum.Service)
            {
                var list = new List<AssignmentViewModel>();
                var query = $@"
		select  u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		 join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 	union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		 join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false 
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		  join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false 
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		  join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false
		    join public.""Team"" as t on ss.""SharedWithTeamId""=t.""Id"" and t.""IsDeleted""=false
			 join public.""TeamUser"" as tu on t.""Id""=tu.""TeamId"" and tu.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
";
                list = await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
                var componentlist = await GetComponentResultList(id);
                if (componentlist != null && componentlist.Count() > 0)
                {
                    foreach (var comp in componentlist)
                    {
                        if (comp.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                        {
                            if (!(list.Any(x => x.UserId == comp.AssigneeId)))
                            {
                                var query1 = $@"select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM  public.""User"" as u 
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where u.""IsDeleted""=false and u.""Id""='{comp.AssigneeId}'";
                                var list1 = await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query1, null);
                                list.AddRange(list1);
                            }
                        }
                    }

                }
                return list;
            }
            if (type == NtsTypeEnum.Task)
            {
                var query = $@"
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		 join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 	union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		 join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		  union 
select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		 join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		  join public.""NtsTaskShared"" as ss on ss.""NtsTaskId""=s.""Id"" and ss.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		  join public.""NtsTaskShared"" as ss on ss.""NtsTaskId""=s.""Id"" and ss.""IsDeleted""=false
		    join public.""Team"" as t on ss.""SharedWithTeamId""=t.""Id"" and t.""IsDeleted""=false
			 join public.""TeamUser"" as tu on t.""Id""=tu.""TeamId"" and tu.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=tu.""UserId""  and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
";
                return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
            }
            else
            {
                var query = $@"
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		 join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 	union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		 join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		  join public.""NtsNoteShared"" as ss on ss.""NtsNoteId""=s.""Id"" and ss.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
	 select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		  join public.""NtsNoteShared"" as ss on ss.""NtsNoteId""=s.""Id"" and ss.""IsDeleted""=false
		    join public.""Team"" as t on ss.""SharedWithTeamId""=t.""Id"" and t.""IsDeleted""=false
			 join public.""TeamUser"" as tu on t.""Id""=tu.""TeamId"" and tu.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
";
                return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
            }
        }
    }
}
