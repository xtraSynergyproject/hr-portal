using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Hangfire;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace CMS.Business
{
    public class ServiceBusiness : BusinessBase<ServiceViewModel, NtsService>, IServiceBusiness
    {
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ServiceSearchViewModel> _querySearchRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryChart;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryTaskRepo;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryNoteRepo;
        private readonly INtsServiceSharedBusiness _ntsServiceSharedBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IProcessDesignResultBusiness _processDesignResultBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly IUserContext _userContext;
        private readonly IRepositoryQueryBase<NtsLogViewModel> _queryNtsLog;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryCal;
        private readonly ILOVBusiness _lOVBusiness;
        public ServiceBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo
            , IMapper autoMapper
            , IRepositoryQueryBase<ServiceViewModel> queryRepo
            , INtsServiceSharedBusiness ntsServiceSharedBusiness
              , INoteBusiness noteBusiness
              , ITaskBusiness taskBusiness
            , IProcessDesignResultBusiness processDesignResultBusiness
            , IServiceProvider serviceProvider, IComponentResultBusiness componentResultBusiness,
            IRepositoryQueryBase<TaskViewModel> queryTaskRepo,
            IRepositoryQueryBase<NoteViewModel> queryNoteRepo,
            IUserContext userContext, ILOVBusiness lOVBusiness,
            IRepositoryQueryBase<ServiceSearchViewModel> querySearchRepo, IRepositoryQueryBase<ProjectGanttTaskViewModel> queryChart,
            IRepositoryQueryBase<NtsLogViewModel> queryNtsLog,
            IRepositoryQueryBase<DashboardCalendarViewModel> queryCal
            ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsServiceSharedBusiness = ntsServiceSharedBusiness;
            _noteBusiness = noteBusiness;
            _taskBusiness = taskBusiness;
            _processDesignResultBusiness = processDesignResultBusiness;
            _serviceProvider = serviceProvider;
            _componentResultBusiness = componentResultBusiness;
            _queryTaskRepo = queryTaskRepo;
            _querySearchRepo = querySearchRepo;
            _queryChart = queryChart;
            _queryNoteRepo = queryNoteRepo;
            _userContext = userContext;
            _queryNtsLog = queryNtsLog;
            _queryCal = queryCal;
            _lOVBusiness = lOVBusiness;

        }
        public async Task<IList<NTSMessageViewModel>> GetServiceAttachedReplies(string userId, string serviceId)
        {
            var query = $@"select ""C"".*
            ,""C"".""Comment"" as ""Body"" 
            ,""CBU"".""Id"" as ""FromId"" 
            ,""CBU"".""Name"" as ""From"" 
            ,""CBU"".""Email"" as ""FromEmail"" 
            ,""CTU"".""Id"" as ""ToId"" 
            ,""CTU"".""Name"" as ""To"" 
            ,""CTU"".""Email"" as ""ToEmail"" 
            ,""C"".""CommentedDate"" as ""SentDate"" 
            ,'Comment' as ""Type""
            from public.""NtsServiceComment"" as ""C""  
            left join public.""User"" as ""CBU"" on ""C"".""CommentedByUserId""=""CBU"".""Id"" and ""CBU"".""IsDeleted""=false 
            left join public.""User"" as ""CTU"" on ""C"".""CommentToUserId""=""CTU"".""Id"" and ""CTU"".""IsDeleted""=false 
            where ""C"".""NtsServiceId""='{serviceId}' and ""C"".""IsDeleted""=false ";


            var comments = await _queryRepo.ExecuteQueryList<NTSMessageViewModel>(query, null);
            return comments.OrderByDescending(x => x.SentDate).ToList();
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> ManagePresubmit(ServiceTemplateViewModel model)
        {
            dynamic dobject = new { };
            if (model.Json.IsNotNullAndNotEmptyAndNotValue("{}"))
            {
                dobject = model.Json.JsonToDynamicObject();
            }
            try
            {
                var result = await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PreSubmit, model.ServiceStatusCode, model, dobject);
                return result;
            }
            catch (Exception ex)
            {
                return CommandResult<ServiceTemplateViewModel>.Instance(model, false, $"Error in pre submit business rule execution: {ex.Message}");
            }

        }

        public async Task<CommandResult<ServiceTemplateViewModel>> ManageService(ServiceTemplateViewModel model)
        {
            CommandResult<ServiceTemplateViewModel> result = null;
            model.TemplateViewModel = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
            model.ServiceTemplateVM = await _repo.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == model.TemplateId);
            var existingItem = await _repo.GetSingleById(model.ServiceId);
            if (model.ServiceTemplateVM.IsNotNull() && model.ServiceTemplateVM.SubjectUdfMappingColumn.IsNotNullAndNotEmpty()) 
            {
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Json);
                if (rowData.IsNotNull() && rowData.ContainsKey(model.ServiceTemplateVM.SubjectUdfMappingColumn))  
                {
                    model.ServiceSubject =Convert.ToString(rowData[model.ServiceTemplateVM.SubjectUdfMappingColumn]);
                }
               
            }
            if (model.ServiceTemplateVM.IsNotNull() && model.ServiceTemplateVM.DescriptionUdfMappingColumn.IsNotNullAndNotEmpty())
            {
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Json);
                if (rowData.IsNotNull() && rowData.ContainsKey(model.ServiceTemplateVM.DescriptionUdfMappingColumn))
                {
                    model.ServiceDescription = Convert.ToString(rowData[model.ServiceTemplateVM.DescriptionUdfMappingColumn]);
                }

            }
            //var tableMetaBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
            //DataRow existingItemTableData = null;
            //NoteViewModel existingNote = null;
            //if (existingItem != null)
            //{
            //    existingItemTableData = await tableMetaBusiness.GetTableDataByHeaderId(existingItem.TemplateId, model.UdfNoteId);
            //    existingNote = await _repo.GetSingleById<NoteViewModel, NtsNote>(model.UdfNoteId);
            //}
            var ntsevent = new LOVViewModel();

            switch (model.ServiceStatusCode)
            {
                case "SERVICE_STATUS_DRAFT":
                    if (model.ServiceId.IsNullOrEmpty())
                    {
                        model.ServiceId = Guid.NewGuid().ToString();
                    }
                    if (model.DueDate != null && model.StartDate != null)
                    {
                        model.ServiceSLA = model.DueDate.Value.Subtract(model.StartDate.Value);
                    }
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_DRAFT");
                    model.ServiceEventId = ntsevent.Id;
                    result = await SaveAsDraft(model);
                    if (result.IsSuccess)
                    {
                        await UpdateServiceWorkflowStatus(result.Item.ServiceId, "Workflow Drafted", null);
                    }

                    break;
                case "SERVICE_STATUS_INPROGRESS":
                    if (model.ServiceId.IsNullOrEmpty())
                    {
                        model.ServiceId = Guid.NewGuid().ToString();
                    }
                    if (model.DueDate != null && model.StartDate != null)
                    {
                        model.ServiceSLA = model.DueDate.Value.Subtract(model.StartDate.Value);
                    }
                    if (model.VersionNo > 1)
                    {
                        ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CREATED_AS_NEWVERSION");
                    }
                    else
                    {
                        ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CREATED");
                    }

                    model.ServiceEventId = ntsevent.Id;
                    result = await Submit(model);
                    if (result.IsSuccess)
                    {
                        await UpdateServiceWorkflowStatus(result.Item.ServiceId, "Workflow Started", null);
                    }
                    break;
                case "SERVICE_STATUS_CANCEL":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CANCELLED");
                    model.ServiceEventId = ntsevent.Id;
                    result = await Cancel(model);
                    //if (result.IsSuccess)
                    //{
                    //    await UpdateServiceWorkflowStatus(result.Item.ServiceId, "Workflow Canceled", null);
                    //}
                    break;

                case "SERVICE_STATUS_CLOSE":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CLOSED");
                    model.ServiceEventId = ntsevent.Id;
                    result = await Close(model);
                    if (result.IsSuccess)
                    {
                        await UpdateServiceWorkflowStatus(result.Item.ServiceId, "Workflow Closed", null);
                    }
                    break;
                case "SERVICE_STATUS_OVERDUE":
                    result = await Overdue(model);
                    //if (result.IsSuccess)
                    //{
                    //    await UpdateServiceWorkflowStatus(result.Item.ServiceId, "Workflow OverDue", null);
                    //}
                    break;
                case "SERVICE_STATUS_COMPLETE":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_COMPLETED");
                    model.ServiceEventId = ntsevent.Id;
                    result = await Complete(model);
                    if (result.IsSuccess)
                    {
                        await UpdateServiceWorkflowStatus(result.Item.ServiceId, "Workflow Completed", null);
                    }
                    break;
                default:
                    return await Task.FromResult(CommandResult<ServiceTemplateViewModel>.Instance(model, false, "Inavlid Service Action"));
            }

            if (result != null && result.IsSuccess /*&& model.ServiceStatusCode != "SERVICE_STATUS_DRAFT"*/)
            {
                var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.UdfTableMetadataId);
                var selectQuery = await GetSelectQuery(tableMetaData, @$" and ""NtsService"".""Id""='{model.ServiceId}' limit 1");
                dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                try
                {
                    await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model.ServiceStatusCode, model, dobject);
                }
                catch (Exception ex)
                {

                    return CommandResult<ServiceTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.ToString()}");
                }


                if (model.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
                {
                    BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignResult(result.Item.TemplateId, result.Item.ServiceId, _repo.UserContext.ToIdentityUser()));
                    //await _processDesignResultBusiness.ExecuteProcessDesignResult(result.Item.TemplateId, result.Item.ServiceId);
                }
                //try
                //{
                //    result = await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PreSubmit, model.ServiceStatusCode, model, dobject);
                //}
                //catch (Exception ex)
                //{

                //    await RollBackData(existingItem, existingNote, existingItemTableData, model);
                //    return CommandResult<ServiceTemplateViewModel>.Instance(model, false, $"Error in pre submit business rule execution: {ex.ToString()}");
                //}
                //if (!result.IsSuccess)
                //{

                //    await RollBackData(existingItem, existingNote, existingItemTableData, model);
                //    return result;
                //}
                //else
                //{
                //    try
                //    {
                //        await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model.ServiceStatusCode, model, dobject);
                //    }
                //    catch (Exception ex)
                //    {

                //        return CommandResult<ServiceTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.ToString()}");
                //    }


                //    if (model.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
                //    {
                //        BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteProcessDesignResult(result.Item.TemplateId, result.Item.ServiceId));
                //    }
                //}
                if (result.IsSuccess)
                {
                    if (existingItem == null || model.ServiceStatusId != existingItem.ServiceStatusId)
                    {
                        await ManageNotification(result.Item);
                    }
                    try
                    {
                        await UpdateNotificationStatus(model.ServiceId, model.ServiceStatusCode);
                    }
                    catch (Exception ex)
                    {

                    }
                }



            }
            if (result.IsSuccess && (model.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || model.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS"))
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null && template.ViewType == NtsViewTypeEnum.Book)
                {
                    if (model.ParentServiceId.IsNotNullAndNotEmpty())
                    {
                        await UpdateBookSequenceOrder(model.ParentServiceId, model.SequenceOrder, model.ServiceId, "Service");
                    }
                    if (model.ParentNoteId.IsNotNullAndNotEmpty())
                    {
                        await UpdateBookSequenceOrder(model.ParentNoteId, model.SequenceOrder, model.ServiceId, "Note");
                    }

                }


            }
            return result;
        }
        private async Task UpdateBookSequenceOrder(string parentServiceId, long? sequenceOrder, string serviceId, string type)
        {
            if (type == "Service")
            {
                var query = $@"Update public.""NtsService"" set ""SequenceOrder""=(""SequenceOrder""+1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false and ""Id""<> '{serviceId}'";
                await _queryCal.ExecuteCommand(query, null);
            }
            if (type == "Note")
            {
                var query1 = $@"Update public.""NtsNote"" set ""SequenceOrder""=(""SequenceOrder""+1) where ""ParentNoteId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
                await _queryCal.ExecuteCommand(query1, null);
            }

            //var query2 = $@"Update public.""NtsTask"" set ""SequenceOrder""=(""SequenceOrder""+1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
            //await _queryCal.ExecuteCommand(query2, null);
        }
        public async Task UpdateCategorySequenceOrderOnDelete(string parentServiceId, long? sequenceOrder, string serviceId, string TemplateCode)
        {
            var query = $@"Update public.""NtsService"" set ""SequenceOrder""=(""SequenceOrder""-1) where  ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=true and ""Id""<>'{serviceId}' and ""TemplateCode""='{TemplateCode}'";
            await _queryCal.ExecuteCommand(query, null);

        }
        private async Task UpdateBookSequenceOrderOnDelete(string parentServiceId, long? sequenceOrder, string serviceId)
        {
            var query = $@"Update public.""NtsService"" set ""SequenceOrder""=(""SequenceOrder""-1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=true and ""Id""<>'{serviceId}'";
            await _queryCal.ExecuteCommand(query, null);
            //var query1 = $@"Update public.""NtsNote"" set ""SequenceOrder""=(""SequenceOrder""-1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
            //await _queryCal.ExecuteCommand(query1, null);
            //var query2 = $@"Update public.""NtsTask"" set ""SequenceOrder""=(""SequenceOrder""-1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
            //await _queryCal.ExecuteCommand(query2, null);
        }
        private async Task RollBackData(ServiceViewModel existingItem, NoteViewModel existingNote, DataRow existingItemTableData, ServiceTemplateViewModel model)
        {
            if (existingItem == null)
            {
                var table = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.UdfTableMetadataId);
                if (table != null)
                {
                    var query = @$"delete from {table.Schema}.""{ table.Name}"" where ""Id""='{model.UdfNoteTableId}';
                    delete from public.""NtsService"" where ""Id""='{model.ServiceId}';
                    delete from public.""NtsNote"" where ""Id""='{model.UdfNoteId}';";
                    await _queryRepo.ExecuteCommand(query, null);
                }
            }
            else
            {
                if (existingItemTableData != null)
                {
                    var table = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.UdfTableMetadataId);
                    await _noteBusiness.RollbackUdfTable(table, existingItemTableData.ToDictionary(), model.UdfNoteTableId);
                }
                if (existingNote != null)
                {
                    await _repo.Edit<NoteViewModel, NtsNote>(existingNote);
                }
                await _repo.Edit<ServiceViewModel, NtsService>(existingItem);
            }
        }
        public async Task ManageNotification(ServiceTemplateViewModel viewModel)
        {
            var notificationTemplates = await _repo.GetList<NotificationTemplate, NotificationTemplate>(x => x.TemplateId == viewModel.TemplateId || (x.NtsType == NtsTypeEnum.Service && x.AutoApplyOnAllTemplates), x => x.ParentNotificationTemplate);
            notificationTemplates = notificationTemplates.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            var reopenstatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_SERVICE_STATUS" && x.Code == "SERVICE_STATUS_REOPEN");
            if (reopenstatus != null && viewModel.IsReopened && viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT")
            {
                viewModel.ServiceStatusId = reopenstatus.Id;
            }
            foreach (var item in notificationTemplates)
            {
                var template = item;
                if (item.ParentNotificationTemplate != null)
                {
                    template = item.ParentNotificationTemplate;
                }
                if (template != null && template.NotificationActionId == viewModel.ServiceStatusId)
                {
                    await CreateNotification(viewModel, template);
                }

            }



        }
        private async Task CreateNotification(ServiceTemplateViewModel viewModel, NotificationTemplate item)
        {
            switch (item.NotificationTo)
            {
                case NtsActiveUserTypeEnum.Requester:
                    await SendNotification(viewModel, item, viewModel.RequestedByUserId);
                    break;
                case NtsActiveUserTypeEnum.Owner:
                    await SendNotification(viewModel, item, viewModel.OwnerUserId);
                    break;
                case NtsActiveUserTypeEnum.Assignee:
                    break;
                case NtsActiveUserTypeEnum.SharedWith:
                    break;
                case NtsActiveUserTypeEnum.SharedBy:
                    break;
                case NtsActiveUserTypeEnum.None:
                    break;
                case NtsActiveUserTypeEnum.OwnerOrRequester:
                    await SendNotification(viewModel, item, viewModel.OwnerUserId);
                    if (viewModel.OwnerUserId != viewModel.RequestedByUserId)
                    {
                        await SendNotification(viewModel, item, viewModel.RequestedByUserId);
                    }

                    break;
                default:
                    break;
            }




        }
        public async Task SendNotification(ServiceTemplateViewModel viewModel, NotificationTemplate item, string toUserId)
        {
            if (toUserId != null)
            {
                var notification = new NotificationViewModel
                {
                    DataAction = DataActionEnum.Create,
                    FromUserId = viewModel.OwnerUserId,
                    ToUserId = toUserId,
                    ReferenceType = ReferenceTypeEnum.NTS_Service,
                    ReferenceTypeNo = viewModel.ServiceNo,
                    ReferenceTypeId = viewModel.ServiceId,
                    NotifyByEmail = item.NotifyByEmail,
                    NotifyBySms = item.NotifyBySms,
                    DynamicObject = viewModel,
                    Subject = item.Subject,
                    Body = item.Body,
                    PortalId = _repo.UserContext.PortalId,
                    Url = SetNotificationUrl(viewModel, item, toUserId),
                    TemplateCode = viewModel.TemplateCode,
                    ActionStatus = NotificationActionStatusEnum.NoActionRequired,
                };
                if (item.ActionType == NotificationActionTypeEnum.Action)
                {
                    notification.ActionStatus = NotificationActionStatusEnum.Pending;
                }
                var notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
                await notificationBusiness.Create(notification);
            }
        }
        public async Task UpdateNotificationStatus(string serviceId, string status)
        {
            var notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
            var list = await notificationBusiness.GetList(x => x.ReferenceType == ReferenceTypeEnum.NTS_Service && x.ReferenceTypeId == serviceId);
            foreach (var item in list)
            {
                var template = await _repo.GetSingle<NotificationTemplate, NotificationTemplate>(x => x.Id == item.NotificationTemplateId);
                if (template != null && template.ActionType == NotificationActionTypeEnum.Action && template.ActionStatusCodes.Any(x => x.Equals(status)))
                {
                    item.ActionStatus = NotificationActionStatusEnum.Completed;
                    await notificationBusiness.Edit(item);
                }
            }
        }
        private string SetNotificationUrl(ServiceTemplateViewModel viewModel, NotificationTemplate item, string toUserId)
        {
            var customurl = HttpUtility.UrlEncode($@"serviceId={viewModel.ServiceId}");
            var url = $@"pageName=ServiceHome&portalId={_repo.UserContext.PortalId}&pageType=Custom&customUrl={customurl}";
            url = $@"Portal/{viewModel.PortalName}?pageUrl={HttpUtility.UrlEncode(url)}";
            return url;

        }

        private async Task<CommandResult<ServiceTemplateViewModel>> Submit(ServiceTemplateViewModel viewModel)
        {

            var validate = await ValidateService(viewModel);
            if (validate.Count > 0)
            {
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, validate);
            }
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            //_ntsValidator.ExecuteTaskPreExecutionScript(viewModel, errorList);
            return await SaveDraftOrSubmit(viewModel, "SERVICE_STATUS_INPROGRESS");
        }

        private async Task<CommandResult<ServiceTemplateViewModel>> SaveAsDraft(ServiceTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);

            var errorList = new List<KeyValuePair<string, string>>();
            //_ntsValidator.ExecuteTaskPreExecutionScript(viewModel, errorList);
            return await SaveDraftOrSubmit(viewModel, "SERVICE_STATUS_DRAFT");
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> Cancel(ServiceTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetServiceViewModelStatus(viewModel, "SERVICE_STATUS_CANCEL");
            var dm = await _repo.GetSingleById(viewModel.ServiceId);
            dm.ServiceStatusId = viewModel.ServiceStatusId;
            dm.ServiceEventId = viewModel.ServiceEventId;
            dm.CanceledDate = DateTime.Now;
            dm.CancelReason = viewModel.CancelReason;
            var result = await _repo.Edit(dm);
            var stepTaskList = await _componentResultBusiness.GetStepTaskList(viewModel.ServiceId);
            stepTaskList = stepTaskList.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").ToList();
            if (stepTaskList != null && stepTaskList.Count() > 0)
            {
                var taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
                foreach (var task in stepTaskList)
                {
                    var taskDetails = await taskBusiness.GetTaskDetails(new TaskTemplateViewModel()
                    {
                        TaskId = task.Id,
                        DataAction = DataActionEnum.Edit,
                        ActiveUserId = viewModel.ActiveUserId,
                        TaskTemplateType = TaskTypeEnum.StepTask
                    });

                    taskDetails.TaskStatusCode = "TASK_STATUS_CANCEL";
                    taskDetails.CancelReason = "Service is Canceled (" + viewModel.CancelReason + ")";
                    await taskBusiness.ManageTask(taskDetails);
                }

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
            //var errorList = new List<KeyValuePair<string, string>>();
            ////_ntsValidator.ExecuteTaskPreExecutionScript(viewModel, errorList);
            //return await SaveDraftOrSubmit(viewModel, "SERVICE_STATUS_CANCEL");
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> Close(ServiceTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetServiceViewModelStatus(viewModel, "SERVICE_STATUS_CLOSE");
            var dm = await _repo.GetSingleById(viewModel.ServiceId);
            dm.ServiceStatusId = viewModel.ServiceStatusId;
            dm.ServiceEventId = viewModel.ServiceEventId;
            dm.ClosedDate = DateTime.Now;
            dm.CloseComment = viewModel.CloseComment;
            dm.UserRating = viewModel.UserRating;
            var result = await _repo.Edit(dm);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> Overdue(ServiceTemplateViewModel viewModel)
        {
            await SetServiceViewModelStatus(viewModel, "SERVICE_STATUS_OVERDUE");
            var dm = await _repo.GetSingleById(viewModel.ServiceId);
            dm.ServiceStatusId = viewModel.ServiceStatusId;
            var result = await _repo.Edit(dm);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> Complete(ServiceTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetServiceViewModelStatus(viewModel, "SERVICE_STATUS_COMPLETE");
            var dm = await _repo.GetSingleById(viewModel.ServiceId);
            dm.ServiceStatusId = viewModel.ServiceStatusId;
            dm.ServiceEventId = viewModel.ServiceEventId;
            dm.CompletedDate = DateTime.Now;
            dm.CompleteReason = viewModel.CompleteReason;
            dm.ActualSLA = dm.CompletedDate.Value - (dm.StartDate ?? DateTime.Now);
            var result = await _repo.Edit(dm);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        private async Task<CommandResult<ServiceTemplateViewModel>> SaveDraftOrSubmit(ServiceTemplateViewModel viewModel, string status)
        {
            await SetServiceViewModelStatus(viewModel, status);
            CommandResult<ServiceTemplateViewModel> result = null;
            if (viewModel.DataAction == DataActionEnum.Create)
            {
                if (status == "SERVICE_STATUS_INPROGRESS")
                {
                    if (viewModel.SubmittedDate == null)
                    {
                        viewModel.SubmittedDate = DateTime.Now;
                    }

                }
                result = await CreateService(viewModel);
                if (result.IsSuccess)
                {
                    viewModel.VersionNo = result.Item.VersionNo;
                }
                else
                {

                    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
                }



            }
            else
            {
                if (status == "SERVICE_STATUS_INPROGRESS")
                {
                    if (viewModel.SubmittedDate == null)
                    {
                        viewModel.SubmittedDate = DateTime.Now;
                    }
                }
                result = await EditService(viewModel);
                //if (viewModel.TemplateAction == NtsActionEnum.Submit)
                //{
                //    if (viewModel.TeamId != null)
                //    {
                //        Share("", "" + viewModel.TeamId, viewModel.Id, viewModel.TemplateAction.ToString());
                //    }
                //}
                //result = EditTask(viewModel, false);
            }


            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<ServiceTemplateViewModel>> ManageBusinessRule(BusinessLogicExecutionTypeEnum executionType, string status, ServiceTemplateViewModel viewModel, dynamic viewModelDynamicObject)
        {
            var lov = await _repo.GetSingle<LOVViewModel, LOV>(x => x.Code == status);
            var action = lov?.Id;
            var businessRule = await _repo.GetSingle<BusinessRuleViewModel, BusinessRule>(x => x.TemplateId == viewModel.TemplateId
            && x.ActionId == action && x.BusinessLogicExecutionType == executionType);
            if (businessRule != null)
            {
                try
                {
                    var businessRuleBusiness = _serviceProvider.GetService<IBusinessRuleBusiness>();
                    var result = await businessRuleBusiness.ExecuteBusinessRule<ServiceTemplateViewModel>(businessRule, TemplateTypeEnum.Service, viewModel, viewModelDynamicObject);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                lov = await _repo.GetSingle<LOVViewModel, LOV>(x => x.Code == "SERVICE_STATUS_ALL");
                action = lov?.Id;
                businessRule = await _repo.GetSingle<BusinessRuleViewModel, BusinessRule>(x => x.TemplateId == viewModel.TemplateId
                && x.ActionId == action && x.BusinessLogicExecutionType == executionType);
                if (businessRule != null)
                {
                    try
                    {
                        var businessRuleBusiness = _serviceProvider.GetService<IBusinessRuleBusiness>();
                        var result = await businessRuleBusiness.ExecuteBusinessRule<ServiceTemplateViewModel>(businessRule, TemplateTypeEnum.Service, viewModel, viewModelDynamicObject);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);

        }

        private async Task<CommandResult<ServiceTemplateViewModel>> CreateService(ServiceTemplateViewModel viewModel)
        {
            var service = _autoMapper.Map<ServiceTemplateViewModel, ServiceViewModel>(viewModel);
            service.Id = viewModel.ServiceId;
            service.ServiceTemplateId = viewModel.Id;
            var result = await _repo.Create(service);
            return await ManageServiceUdfTable(viewModel, result);
        }
        private async Task<CommandResult<ServiceTemplateViewModel>> EditService(ServiceTemplateViewModel viewModel, bool editUdfTable = true)
        {
            var service = _autoMapper.Map<ServiceTemplateViewModel, ServiceViewModel>(viewModel);
            service.Id = viewModel.ServiceId;
            service.ServiceTemplateId = viewModel.Id;
            var result = await _repo.Edit(service);
            if (editUdfTable)
            {
                return await ManageServiceUdfTable(viewModel);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        private async Task<Dictionary<string, string>> ValidateService(ServiceTemplateViewModel viewModel)
        {
            var errorList = new Dictionary<string, string>();

            if (viewModel.ServiceTemplateVM.IsSubjectMandatory && viewModel.ServiceSubject.IsNullOrEmpty())
            {
                errorList.Add("ServiceSubject", string.Concat(viewModel.SubjectText.ToDefaultSubjectText(), " field is required"));
            }
            if (viewModel.ServiceTemplateVM.IsSubjectUnique)
            {
                //var servicelist = await _repo.execute(x=>x.TemplateId== viewModel.TemplateId);
                bool isexist = await IsServiceSubjectUnique(viewModel.TemplateId, viewModel.ServiceSubject, viewModel.ServiceId);//servicelist.Any(x => x.ServiceSubject.Equals(viewModel.ServiceSubject, StringComparison.InvariantCultureIgnoreCase) && x.Id!=viewModel.ServiceId);
                if (isexist)
                {
                    errorList.Add("ServiceSubject", string.Concat("The given ", viewModel.SubjectText.ToDefaultSubjectText(), " already exist. Please enter another ", viewModel.SubjectText.ToDefaultSubjectText()));
                }
            }
            if (viewModel.StartDate == null)
            {
                errorList.Add("StartDate", "Start Date field is required");
            }
            if (!viewModel.AllowPastStartDate && viewModel.StartDate != null && viewModel.StartDate.Value.Date < DateTime.Today)
            {
                errorList.Add("StartDate", "Start Date should be greater than or equal to today's date");
            }
            if ((viewModel.StartDate != null && viewModel.DueDate != null) && (viewModel.StartDate.Value > viewModel.DueDate.Value))
            {
                errorList.Add("DueDate", "Start Date should be less than or equal to due date");
            }
            if (viewModel.ServicePriorityId.IsNullOrEmpty())
            {
                errorList.Add("TaskPriorityId", "Priority is required");
            }


            //var validate = await ValidateForm(data, pageId, rowData, tableMetaData, tableColumns);
            //if (validate.IsNotNullAndNotEmpty())
            //{
            //    return await Task.FromResult(new Tuple<bool, string>(false, validate));
            //}

            //if (viewModel.TemplateAction == NtsActionEnum.Complete && viewModel.IsAttachmentRequired.ToSafeBool() && viewModel.CSVFileIds.IsNullOrEmptyOrValue(","))
            //{
            //    errorList.Add(new KeyValuePair<string, string>("Attachment", "Atleast one attachment is required"));
            //}

            return errorList;
        }

        private async Task<string> ValidateForm(string data, string pageId, Dictionary<string, object> rowData, TableMetadataViewModel tableMetaData, List<ColumnMetadataViewModel> tableColumns, string exculdeId = null)
        {
            return string.Empty;
        }

        private async Task<CommandResult<ServiceTemplateViewModel>> ManageServiceUdfTable(ServiceTemplateViewModel viewModel, ServiceViewModel model = null)
        {
            if (viewModel != null)
            {
                if (viewModel.UdfTemplateId.IsNotNullAndNotEmpty())
                {
                    NoteTemplateViewModel noteUdf = null;
                    if (viewModel.UdfNoteId.IsNullOrEmpty())
                    {
                        noteUdf = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                        {
                            TemplateId = viewModel.UdfTemplateId,
                            DataAction = DataActionEnum.Create,
                            OwnerUserId = viewModel.OwnerUserId,
                            RequestedByUserId = viewModel.RequestedByUserId,
                            ActiveUserId = viewModel.ActiveUserId,

                        });
                        noteUdf.DataAction = DataActionEnum.Create;
                        noteUdf.NoteSubject = viewModel.ServiceSubject;
                        noteUdf.NoteDescription = viewModel.ServiceDescription;
                        noteUdf.StartDate = viewModel.StartDate;
                        noteUdf.NoteId = Guid.NewGuid().ToString();
                        noteUdf.TemplateId = viewModel.UdfTemplateId;
                        noteUdf.Json = viewModel.Json;
                        if (viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT")
                        {
                            noteUdf.NoteStatusCode = "NOTE_STATUS_DRAFT";
                        }
                        else
                        {
                            noteUdf.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                        }
                        noteUdf.VersionNo = viewModel.VersionNo;
                        var result = await _noteBusiness.ManageNote(noteUdf);
                        if (result.IsSuccess)
                        {
                            if (model != null)
                            {
                                viewModel.UdfNoteId = model.UdfNoteId = result.Item.NoteId;
                                viewModel.UdfNoteTableId = model.UdfNoteTableId = result.Item.UdfNoteTableId;
                                model.DataAction = DataActionEnum.Edit;
                                await _repo.Edit(model);
                            }

                            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
                        }
                        else
                        {
                            var ret = CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, result.Message);
                            ret.Messages = result.Messages;
                            return ret;
                        }
                    }
                    else
                    {
                        noteUdf = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                        {
                            NoteId = viewModel.UdfNoteId,
                            RecordId = viewModel.UdfNoteTableId,
                            DataAction = DataActionEnum.Edit,
                            TemplateId = viewModel.UdfTemplateId,
                            ActiveUserId = viewModel.ActiveUserId,
                            UdfNoteTableId = viewModel.UdfNoteTableId

                        });
                        noteUdf.DataAction = DataActionEnum.Edit;
                        noteUdf.Json = viewModel.Json;
                        noteUdf.Subject = viewModel.ServiceSubject;
                        noteUdf.Description = viewModel.ServiceDescription;
                        noteUdf.StartDate = viewModel.StartDate;
                        noteUdf.NoteId = viewModel.UdfNoteId;
                        noteUdf.RecordId = viewModel.UdfNoteTableId;
                        noteUdf.TemplateId = viewModel.UdfTemplateId;
                        noteUdf.UdfNoteTableId = viewModel.UdfNoteTableId;
                        if (viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT")
                        {
                            noteUdf.NoteStatusCode = "NOTE_STATUS_DRAFT";
                        }
                        else
                        {
                            noteUdf.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                        }
                        noteUdf.AllowPastStartDate = true;
                        noteUdf.VersionNo = viewModel.VersionNo;
                        var result = await _noteBusiness.ManageNote(noteUdf);
                        if (result.IsSuccess)
                        {
                            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
                        }
                        else
                        {
                            var ret = CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, result.Message);
                            ret.Messages = result.Messages;
                            return ret;
                        }
                    }
                }
                else
                {
                    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "Udf Template not found");
                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "Template not found");
        }
        private async Task SetServiceViewModelStatus(ServiceTemplateViewModel viewModel, string statusCode = null)
        {
            statusCode = statusCode ?? viewModel.ServiceStatusCode;
            var serviceStatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_SERVICE_STATUS" && x.Code == statusCode);
            if (serviceStatus != null)
            {
                viewModel.ServiceStatusCode = serviceStatus.Code;
                viewModel.ServiceStatusId = serviceStatus.Id;
            }
        }
        public async Task<ServiceTemplateViewModel> GetServiceDetails(ServiceTemplateViewModel viewModel)
        {
            var model = new ServiceTemplateViewModel();
            if (viewModel.ServiceId.IsNullOrEmpty())
            {
                model = await GetServiceTemplateForNewService(viewModel);
                model.ActiveUserId = viewModel.ActiveUserId;
                model.Prms = viewModel.Prms;
                model.Udfs = viewModel.Udfs;
                model.ReadoOnlyUdfs = viewModel.ReadoOnlyUdfs;
                model.ServiceId = Guid.NewGuid().ToString();
                model.ServiceNo = await GenerateNextServiceNo(model);
                model.OwnerUserId = viewModel.OwnerUserId;
                model.ParentServiceId = viewModel.ParentServiceId;
                model.RequestedByUserId = viewModel.RequestedByUserId;

                var ownerUserId = model.Prms.GetValue("ownerUserId");
                var parentServiceId = model.Prms.GetValue("parentServiceId");
                var hideStepTaskDetails = model.Prms.GetValue("hideStepTaskDetails");
                var requestedByUserId = model.Prms.GetValue("requestedByUserId");
                var subject = model.Prms.GetValue("subject");
                var description = model.Prms.GetValue("description");
                var referenceId = model.Prms.GetValue("referenceId");
                var sequenceOrder = model.Prms.GetValue("sequenceOrder");
                var servicePlusId = model.Prms.GetValue("servicePlusId");
                var parentTaskId = model.Prms.GetValue("parentTaskId");
                var parentNoteId = model.Prms.GetValue("parentNoteId");
                var notePlusId = model.Prms.GetValue("notePlusId");
                if (notePlusId.IsNotNullAndNotEmpty())
                {
                    model.NotePlusId = notePlusId;
                }
                if (referenceId.IsNotNullAndNotEmpty())
                {
                    model.ReferenceId = referenceId;
                }
                if (subject.IsNotNullAndNotEmpty())
                {
                    model.ServiceSubject = subject;
                }
                if (description.IsNotNullAndNotEmpty())
                {
                    model.ServiceDescription = description;
                }
                if (ownerUserId.IsNotNullAndNotEmpty())
                {
                    model.OwnerUserId = ownerUserId;
                }
                if (parentServiceId.IsNotNullAndNotEmpty())
                {
                    model.ParentServiceId = parentServiceId;
                }
                if (parentTaskId.IsNotNullAndNotEmpty())
                {
                    model.ParentTaskId = parentTaskId;
                }
                if (parentNoteId.IsNotNullAndNotEmpty())
                {
                    model.ParentNoteId = parentNoteId;
                }
                if (hideStepTaskDetails.IsNotNullAndNotEmpty())
                {
                    model.HideStepTaskDetails = hideStepTaskDetails.ToSafeBool();
                }
                if (requestedByUserId.IsNotNullAndNotEmpty())
                {
                    model.RequestedByUserId = requestedByUserId;
                }
                if (servicePlusId.IsNotNullAndNotEmpty())
                {
                    model.ServicePlusId = servicePlusId;
                }
                if (sequenceOrder.IsNotNullAndNotEmpty())
                {
                    model.SequenceOrder = Convert.ToInt64(sequenceOrder);
                }
                if (model.SequenceOrder == null)
                {
                    model.SequenceOrder = 1;
                }

                model.VersionNo = 1;
                if (model.OwnerUserId.IsNullOrEmpty())
                {
                    model.OwnerUserId = _repo.UserContext.UserId;
                    model.OwnerUserName = _repo.UserContext.Name;
                    model.OwnerUserEmail = _repo.UserContext.Email;
                    model.OwnerUserPhotoId = _repo.UserContext.PhotoId;
                }
                else
                {
                    var owner = await _repo.GetSingleById<UserViewModel, User>(model.OwnerUserId);
                    if (owner != null)
                    {
                        model.OwnerUserName = owner.Name;
                        model.OwnerUserEmail = owner.Email;
                        model.OwnerUserPhotoId = owner.PhotoId;
                    }
                }

                if (model.RequestedByUserId.IsNullOrEmpty())
                {
                    model.RequestedByUserId = _repo.UserContext.UserId;
                    model.RequestedByUserName = _repo.UserContext.Name;
                    model.RequestedByUserEmail = _repo.UserContext.Email;
                    model.RequestedByUserPhotoId = _repo.UserContext.PhotoId;
                }
                else
                {
                    var requester = await _repo.GetSingleById<UserViewModel, User>(model.RequestedByUserId);
                    if (requester != null)
                    {
                        model.RequestedByUserName = requester.Name;
                        model.RequestedByUserEmail = requester.Email;
                        model.RequestedByUserPhotoId = requester.PhotoId;
                    }
                }
                model.StartDate = DateTime.Now;
                if (model.SLA == null)
                {
                    model.DueDate = model.StartDate.Value.AddDays(1);
                    model.ServiceSLA = TimeSpan.FromDays(1);
                }
                else
                {
                    model.DueDate = model.StartDate.Value.Add(model.SLA.Value);
                    model.ServiceSLA = model.SLA.Value;
                }
                model.ServiceSLAMinutes = model.ServiceSLA.Value.TotalSeconds;
                if (model.ActiveUserId == model.RequestedByUserId && model.ActiveUserId == model.OwnerUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Owner;
                }
                else
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Requester;
                }
                await GetUdfDetails(model);
            }
            else
            {
                model = await GetServiceTemplateById(viewModel);
                MapTemplate(model, viewModel);
                model.Udfs = viewModel.Udfs;
                if (model.ServiceSLA == null)
                {
                    if (model.StartDate.HasValue && model.DueDate.HasValue)
                    {
                        model.ServiceSLA = model.DueDate - model.StartDate;
                    }
                }
                if (model.ServiceSLA.HasValue)
                {
                    model.ServiceSLAMinutes = model.ServiceSLA.Value.TotalSeconds;
                }
                model.RecordId = viewModel.RecordId;
                model.ActiveUserId = viewModel.ActiveUserId;
                model.Prms = viewModel.Prms;
                model.ReadoOnlyUdfs = viewModel.ReadoOnlyUdfs;
                var hideStepTaskDetails = model.Prms.GetValue("hideStepTaskDetails");
                if (hideStepTaskDetails.IsNotNullAndNotEmpty())
                {
                    model.HideStepTaskDetails = hideStepTaskDetails.ToSafeBool();
                }

                //if (model.IncludeReadonlyData)
                //{
                //    await SetReadonlyData(model);
                //}
                model.IncludeSharedList = viewModel.IncludeSharedList;
                if (model.IncludeSharedList)
                {
                    model.SharedList = await SetSharedList(model);
                }
                if (model.ActiveUserId == model.RequestedByUserId && model.ActiveUserId == model.OwnerUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Owner;
                }
                else if (model.ActiveUserId == model.RequestedByUserId && model.ActiveUserId != model.OwnerUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Requester;
                }
                //else if (viewModel.ActiveUserId == model.AssignedToUserId)
                //{
                //    model.ServiceActiveUserType = NtsActiveUserTypeEnum.Assignee;
                //}
                else
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.None;
                }
                await GetUdfDetails(model);
                model.StepTasksList = await _componentResultBusiness.GetStepTaskList(model.ServiceId);
            }

            return await Task.FromResult(model);
        }
        private void MapTemplate(ServiceTemplateViewModel model, ServiceTemplateViewModel template)
        {
            model.HideHeader = template.HideHeader;
            model.ServiceNoText = template.ServiceNoText;
            model.AllowPastStartDate = template.AllowPastStartDate;
            model.EnableAttachment = template.EnableAttachment;
            model.EnableBackButton = template.EnableBackButton;
            model.EnableCancelButton = template.EnableCancelButton;
            model.EnableComment = template.EnableComment;
            model.EnablePrintButton = template.EnablePrintButton;
            model.HideBanner = template.HideBanner;
            model.HideDescription = template.HideDescription;
            model.HideExpiryDate = template.HideDescription;
            model.HidePriority = template.HidePriority;
            model.HideStartDate = template.HideStartDate;
            model.HideSubject = template.HideSubject;

        }
        private async Task GetUdfDetails(ServiceTemplateViewModel model)
        {
            if (model.ColumnList == null || !model.ColumnList.Any())
            {
                return;
            }
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var result = JObject.Parse(model.Json);
                var rows = (JArray)result.SelectToken("components");
                await ChildComp(rows, model.ColumnList, model);
                if (model.FormType == FormTypeEnum.Wizard)
                {
                    CustomizeJsonForWizard(rows, model.ColumnList, model);
                }
                else
                {
                    CustomizeJsonForForm(rows, model.ColumnList, model);
                }
                result.Remove("components");
                result.Add("components", JArray.FromObject(rows));
                model.Json = result.ToString();
            }
        }

        private void CustomizeJsonForWizard(JArray panels, List<ColumnMetadataViewModel> columnList, ServiceTemplateViewModel model)
        {
            var hidden = true;
            var activePanels = new JArray();
            foreach (JObject panel in panels)
            {
                hidden = true;
                var comps = (JArray)panel.SelectToken("components");
                hidden = ManagePanelVisibility(comps, hidden);
                if (hidden)
                {
                    var hiddenProp = new JProperty("hidden", true);
                    panel.Add(hiddenProp);
                }
                else
                {
                    var buttonSettings = (JObject)panel.SelectToken("buttonSettings");
                    if (buttonSettings != null)
                    {
                        var removed = panel.Remove("buttonSettings");
                    }
                    panel.Add("buttonSettings", JToken.Parse("{'previous': false,'cancel': false,'next': false,'submit': false}"));

                }
            }
            var apCount = panels.Where(x => x.SelectToken("hidden") == null || Convert.ToBoolean(x.SelectToken("hidden").ToString()) == false).Count();
            var i = 1;
            var isFirst = false;
            var isLast = false;
            foreach (JObject panel in panels)
            {
                var isHiddenPanel = (JToken)panel.SelectToken("hidden");
                isFirst = false;
                isLast = false;
                if (isHiddenPanel == null || Convert.ToBoolean(isHiddenPanel.ToString()) == false)
                {
                    if (i == 1)
                    {
                        isFirst = true;
                    }
                    if (i == apCount)
                    {
                        isLast = true;
                    }
                    ManagePanelActions(panel, isFirst, isLast, model);
                    i++;
                }

            }
        }

        private void CustomizeJsonForForm(JArray panels, List<ColumnMetadataViewModel> columnList, ServiceTemplateViewModel model)
        {
            var hidden = true;
            var activePanels = new JArray();
            foreach (JObject panel in panels)
            {
                var typeObj = panel.SelectToken("type");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    if (type != "panel")
                    {
                        continue;
                    }
                }
                hidden = true;
                var comps = (JArray)panel.SelectToken("components");
                if (comps.IsNotNull())
                {
                    hidden = ManagePanelVisibility(comps, hidden);
                }
                //else 
                //{
                //    hidden = false;
                //}
                if (hidden)
                {
                    var hiddenProp = new JProperty("hidden", true);
                    panel.Add(hiddenProp);
                }

            }
        }
        private bool ManagePanelVisibility(JArray comps, bool isHidden)
        {
            if (isHidden)
            {
                foreach (JObject jcomp in comps)
                {
                    var typeObj = jcomp.SelectToken("type");
                    var keyObj = jcomp.SelectToken("key");
                    if (typeObj.IsNotNull())
                    {
                        var type = typeObj.ToString();
                        var key = keyObj.ToString();
                        if (type == "textfield" || type == "textarea" || type == "number" || type == "password"
                            || type == "selectboxes" || type == "checkbox" || type == "select" || type == "radio"
                            || type == "email" || type == "url" || type == "phoneNumber" || type == "tags"
                            || type == "datetime" || type == "day" || type == "time" || type == "currency"
                            || type == "signature" || type == "file" || type == "hidden" || type == "datagrid"
                            || (type == "htmlelement" && key == "chartgrid"))
                        {

                            var isHiddenP = jcomp.SelectToken("hidden");
                            if (isHiddenP != null)
                            {
                                isHidden = Convert.ToBoolean(isHiddenP.ToString());
                                if (!isHidden)
                                {
                                    isHidden = false;
                                    break;

                                }
                            }
                            else
                            {
                                isHidden = false;
                                break;
                            }
                        }
                        else if (type == "columns")
                        {
                            JArray cols = (JArray)jcomp.SelectToken("columns");
                            foreach (var col in cols)
                            {
                                JArray rows = (JArray)col.SelectToken("components");
                                if (rows != null)
                                    isHidden = ManagePanelVisibility(rows, isHidden);
                            }
                        }
                        else if (type == "table")
                        {
                            JArray cols = (JArray)jcomp.SelectToken("rows");
                            foreach (var col in cols)
                            {
                                JArray rows = (JArray)col.SelectToken("components");
                                if (rows != null)
                                    isHidden = ManagePanelVisibility(rows, isHidden);
                            }
                        }
                        else
                        {
                            JArray rows = (JArray)jcomp.SelectToken("components");
                            if (rows != null)
                                isHidden = ManagePanelVisibility(rows, isHidden);
                        }
                    }
                }
            }
            return isHidden;
        }
        private void ManagePanelActions(JObject panel, bool isFirst, bool isLast, ServiceTemplateViewModel model)
        {
            var key = "";
            var keyObj = panel.SelectToken("key");
            if (keyObj != null)
            {
                key = keyObj.ToString();
            }
            var next = @$"{{
                                        ""input"": true,
                                        ""label"": ""Next"",
                                        ""tableView"": false,
                                        ""key"": ""{key}GotoNextpage"",
                                        ""rightIcon"": ""fa fa-chevron-right"",
                                        ""block"": false,
                                        ""action"": ""event"",
                                        ""type"": ""button"",
                                        ""event"": ""OnFormIoNextPage"",                                        
                                        ""hideOnChildrenHidden"": false,
                                        ""theme"": ""primary"",
                                        ""customClass"": ""wiz-btn"",
                                    }},";
            var prev = @$"{{
                                        ""input"": true,
                                        ""label"": ""Previous"",
                                        ""tableView"": false,
                                        ""key"": ""{key}gotoPreviousPage"",
                                        ""leftIcon"": ""fa fa-chevron-left"",
                                        ""block"": false,
                                        ""action"": ""event"",
                                        ""type"": ""button"",
                                        ""event"": ""OnFormIoPreviousPage"",
                                        ""hideOnChildrenHidden"": false,
                                        ""theme"": ""primary"",
                                        ""customClass"": ""wiz-btn"",
                                    }},";
            var draft = @$"{{
                                        ""input"": true,
                                        ""label"": ""{model.SaveAsDraftText.ToDefaultSaveAsDraftButtonText()}"",
                                        ""tableView"": false,
                                        ""key"": ""{key}SaveAsDraft"",
                                        ""block"": false,
                                        ""action"": ""event"",
                                        ""type"": ""button"",
                                        ""event"": ""OnFormIoWizardSaveAsDraft"",
                                        ""hideOnChildrenHidden"": false,
                                        ""theme"": ""info"",
                                        ""customClass"": ""wiz-btn"",
                                    }},";
            var submit = @$"{{
                                        ""input"": true,
                                        ""label"": ""{model.SubmitButtonText.ToDefaultSubmitButtonText()}"",
                                        ""tableView"": false,
                                        ""key"": ""{key}Submit"",
                                        ""block"": false,
                                        ""action"": ""event"",
                                        ""type"": ""button"",
                                        ""event"": ""OnFormIoWizardSubmit"",
                                        ""hideOnChildrenHidden"": false,
                                        ""theme"": ""primary"",
                                        ""customClass"": ""wiz-btn"",
                                    }},";
            var version = @$"{{
                                        ""input"": true,
                                        ""label"": ""{model.SubmitButtonText.ToDefaultSubmitButtonText()}"",
                                        ""tableView"": false,
                                        ""key"": ""{key}Versioning"",
                                        ""block"": false,
                                        ""action"": ""event"",
                                        ""type"": ""button"",
                                        ""event"": ""OnFormIoWizardSubmit"",
                                        ""hideOnChildrenHidden"": false,
                                        ""theme"": ""primary"",
                                        ""customClass"": ""wiz-btn"",
                                    }},";
            var comps = (JArray)panel.SelectToken("components");
            if (comps != null)
            {

                comps.Add(JToken.Parse(@$"{{""label"": ""Columns"",
                                ""input"": false,
                                ""tableView"": false,
                                ""key"": ""{key}Columns"",
                                ""columns"": [
                                {{
                                ""components"": [
                                        {(isFirst == false ? prev : "")}   
                                        {(isLast == false ? next : "")}     
                                        {(model.IsDraftButtonVisible ? draft : "")}   
                                        {(isLast == true && model.IsSubmitButtonVisible ? submit : "")}  
                                        {(isLast == true && model.IsReopenButtonVisible ? version : "")}  
                                       ],
                                ""width"": 12,
                                ""offset"": 0,
                                ""push"": 0,
                                ""pull"": 0,
                                ""size"": ""md"",
                                      
                            }}
                                      
                                ],
                                ""type"": ""columns"",
                                ""hideLabel"": true,
                                ""customClass"": ""wewe"",}}
            "));

            }
        }

        private async Task ChildComp(JArray comps, List<ColumnMetadataViewModel> Columns, ServiceTemplateViewModel model)
        {
            List<UdfPermissionViewModel> udfPermissons = null;

            udfPermissons = await _repo.GetList<UdfPermissionViewModel, UdfPermission>(x => x.TemplateId == model.TemplateId);

            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                var keyObj = jcomp.SelectToken("key");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    var key = keyObj.ToString();
                    if (type == "textfield" || type == "textarea" || type == "number" || type == "password"
                        || type == "selectboxes" || type == "checkbox" || type == "select" || type == "radio"
                        || type == "email" || type == "url" || type == "phoneNumber" || type == "tags"
                        || type == "datetime" || type == "day" || type == "time" || type == "currency"
                        || type == "signature" || type == "file" || type == "hidden" || type == "datagrid"
                        || (type == "htmlelement" && key == "chartgrid"))
                    {


                        var reserve = jcomp.SelectToken("reservedKey");
                        if (reserve == null)
                        {
                            var tempmodel = JsonConvert.DeserializeObject<FormFieldViewModel>(jcomp.ToString());


                            var columnId = jcomp.SelectToken("columnMetadataId");


                            if (columnId != null)
                            {
                                var columnMeta = Columns.FirstOrDefault(x => x.Name == tempmodel.key);
                                if (columnMeta != null)
                                {
                                    columnMeta.ActiveUserType = model.ActiveUserType;
                                    columnMeta.NtsStatusCode = model.ServiceStatusCode;
                                    //Set default value
                                    if (model.Udfs != null && model.Udfs.GetValue(columnMeta.Name) != null)
                                    {
                                        var newProperty = new JProperty("defaultValue", model.Udfs.GetValue(columnMeta.Name));
                                        jcomp.Add(newProperty);
                                    }

                                    var udfValue = new JProperty("udfValue", columnMeta.Value);
                                    jcomp.Add(udfValue);

                                    var isReadonly = false;
                                    if (model.ReadoOnlyUdfs != null && model.ReadoOnlyUdfs.ContainsKey(columnMeta.Name))
                                    {
                                        isReadonly = model.ReadoOnlyUdfs.GetValueOrDefault(columnMeta.Name);
                                    }
                                    if (udfPermissons == null)
                                    {
                                        var hiddenProperty = jcomp.SelectToken("hidden");
                                        if (hiddenProperty == null)
                                        {
                                            var newProperty = new JProperty("hidden", true);
                                            jcomp.Add(newProperty);
                                        }
                                    }
                                    else
                                    {
                                        var udfColumnPermission = udfPermissons.FirstOrDefault(x => x.ColumnMetadataId == tempmodel.columnMetadataId && x.TemplateId == model.TemplateId);
                                        if (udfColumnPermission != null)
                                        {
                                            udfColumnPermission.ActiveUserType = model.ActiveUserType;
                                            udfColumnPermission.NtsStatusCode = model.ServiceStatusCode;
                                            if (!udfColumnPermission.IsVisible)
                                            {
                                                var hiddenProperty = jcomp.SelectToken("hidden");
                                                if (hiddenProperty == null)
                                                {
                                                    var newProperty = new JProperty("hidden", true);
                                                    jcomp.Add(newProperty);
                                                    if (type == "datagrid")
                                                    {
                                                        JArray dataRows = (JArray)jcomp.SelectToken("components");
                                                        foreach (JObject jcomp1 in dataRows)
                                                        {
                                                            var newProperty1 = new JProperty("hidden", true);
                                                            jcomp1.Add(newProperty1);
                                                        }
                                                    }

                                                }
                                            }
                                            if (!udfColumnPermission.IsEditable || isReadonly || model.AllReadOnly)
                                            {
                                                var disableProperty = jcomp.SelectToken("disabled");
                                                if (disableProperty == null)
                                                {
                                                    var newProperty = new JProperty("disabled", true);
                                                    jcomp.Add(newProperty);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var hiddenProperty = jcomp.SelectToken("hidden");
                                            if (hiddenProperty == null)
                                            {
                                                var newProperty = new JProperty("hidden", true);
                                                jcomp.Add(newProperty);
                                                if (type == "datagrid")
                                                {
                                                    JArray dataRows = (JArray)jcomp.SelectToken("components");
                                                    foreach (JObject jcomp1 in dataRows)
                                                    {
                                                        var newProperty1 = new JProperty("hidden", true);
                                                        jcomp1.Add(newProperty1);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                            }

                        }
                    }
                    else if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await ChildComp(rows, Columns, model);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await ChildComp(rows, Columns, model);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            await ChildComp(rows, Columns, model);
                    }
                }
            }
        }
        private async Task<ServiceTemplateViewModel> GetServiceTemplateById(ServiceTemplateViewModel viewModel)
        {
            var data = new ServiceTemplateViewModel();
            var query = @$"select ""TM"".* from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""UdfTableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsService"" as ""S"" on ""T"".""Id""=""S"".""TemplateId"" and ""S"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false  and ""S"".""Id""='{viewModel.ServiceId}'";
            var tableMetadata = await _queryRepo.ExecuteQuerySingle<TableMetadataViewModel>(query, null);
            if (tableMetadata != null)
            {

                if (viewModel.LogId.IsNotNullAndNotEmpty())
                {
                    var log = await _queryRepo.ExecuteQuerySingleDynamicObject(@$"select ""IsLatest"" from log.""NtsServiceLog"" where  ""Id""='{viewModel.LogId}'", null);
                    if (log != null)
                    {
                        viewModel.IsLogRecord = !(bool)log.IsLatest;
                    }
                }
                var selectQuery = "";
                if (viewModel.IsLogRecord && viewModel.LogId.IsNotNullAndNotEmpty())
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsService"".""Id""='{viewModel.LogId}' limit 1", null, null, viewModel.IsLogRecord, viewModel.LogId);
                }
                else
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsService"".""Id""='{viewModel.ServiceId}' limit 1", null, null, viewModel.IsLogRecord, viewModel.LogId);

                }

                data = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(selectQuery, null);

                if (data.IsNotNull())
                {
                    data.IsLogRecord = viewModel.IsLogRecord;
                    data.LogId = viewModel.LogId;
                    data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetadata.Id
                    && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
                    if (data.ColumnList != null && viewModel.SetUdfValue)
                    {
                        var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            foreach (var item in data.ColumnList.Where(x => x.IsUdfColumn))
                            {
                                try
                                {
                                    item.Value = dr[item.Name];
                                    if (Convert.ToString(item.Value) == "")
                                    {
                                        item.Value = null;
                                    }
                                }
                                catch (Exception)
                                {


                                }

                            }
                        }
                    }
                }
            }
            return data;
        }

        public async Task<DataTable> GetServiceDataTableById(string serviceId, TableMetadataViewModel tableMetadata, bool isLog = false, string logId = null)
        {
            if (tableMetadata != null)
            {
                var selectQuery = "";
                if (isLog && logId.IsNotNullAndNotEmpty())
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsService"".""Id""='{logId}' limit 1", null, null, isLog, logId);
                }
                else
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsService"".""Id""='{serviceId}' limit 1");
                }

                var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                return dt;

            }

            return null;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableColumns(TableMetadataViewModel tableMetaData)
        {
            var columns = new List<ColumnMetadataViewModel>();
            var tables = new List<string>();
            var condition = new List<string>();
            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
                where 
                (
                    (t.""Schema""='{tableMetaData.Schema}' and t.""Name""='{tableMetaData.Name}' and (c.""IsUdfColumn""=true )) or
                    (t.""Schema""='public' and t.""Name""='ServiceTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsService')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false ";
            var pkColumns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            condition.Add(@$"""{tableMetaData.Name}"".""IsDeleted""=false");
            foreach (var item in pkColumns)
            {
                if (item.TableName == "NtsService" && item.IsPrimaryKey)
                {
                    item.Alias = "ServiceId";
                }
                else
                {
                    item.Alias = item.Name;
                }

                columns.Add(item);
            }
            var fks = pkColumns.Where(x => x.TableName == tableMetaData.Name && x.IsUdfColumn && x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty() && x.HideForeignKeyTableColumns == false).ToList();
            if (fks != null && fks.Count() > 0)
            {

                query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
                ,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
                ,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false 
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false 
                where c.""TableMetadataId""='{tableMetaData.Id}'
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  and c.""IsUdfColumn""=true 
                and (fc.""IsSystemColumn""=false or fc.""IsPrimaryKey""=true)";
                var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        var tableName = item.TableAliasName;
                        if (item.IsUdfColumn)
                        {
                            tableName = $@"{item.TableAliasName}_{item.ForeignKeyColumnName}";
                        }
                        item.TableName = tableName;
                        item.Alias = $"{item.ForeignKeyColumnName}_{item.Name}";
                        columns.Add(item);
                    }

                }
            }


            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "Id", Alias = "CreatedByUser_Id" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "Name", Alias = "CreatedByUser_Name" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "Email", Alias = "CreatedByUser_Email" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "JobTitle", Alias = "CreatedByUser_JobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "PhotoId", Alias = "CreatedByUser_PhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "Id", Alias = "UpdatedByUser_Id" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "Name", Alias = "UpdatedByUser_Name" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "Email", Alias = "UpdatedByUser_Email" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "JobTitle", Alias = "UpdatedByUser_JobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "PhotoId", Alias = "UpdatedByUser_PhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "Id", Alias = "OwnerUserId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "Name", Alias = "OwnerUserName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "Email", Alias = "OwnerUserEmail" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "JobTitle", Alias = "OwnerUserJobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "PhotoId", Alias = "OwnerUserPhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "Id", Alias = "RequestedByUserId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "Name", Alias = "RequestedByUserName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "Email", Alias = "RequestedByUserEmail" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "JobTitle", Alias = "RequestedByUserJobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "PhotoId", Alias = "RequestedByUserPhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "TableMetadataId", Alias = "TableMetadataId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Name", Alias = "TemplateName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "DisplayName", Alias = "TemplateDisplayName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "TableSelectionType", Alias = "TableSelectionType" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Code", Alias = "TemplateCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "ViewType", Alias = "ViewType" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "Json", Alias = "Json" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "TableMetadataId", Alias = "UdfTableMetadataId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceOwnerType", Name = "Code", Alias = "ServiceOwnerTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceOwnerType", Name = "Name", Alias = "ServiceOwnerTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServicePriority", Name = "Code", Alias = "ServicePriorityCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServicePriority", Name = "Name", Alias = "ServicePriorityName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceStatus", Name = "Code", Alias = "ServiceStatusCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceStatus", Name = "Name", Alias = "ServiceStatusName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceAction", Name = "Code", Alias = "ServiceActionCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceAction", Name = "Name", Alias = "ServiceActionName" });
            return columns;
        }
        public async Task<string> GetServiceSelectQuery(string where = null)
        {
            var tables = new List<string>();

            var columns = new List<string>();
            var condition = new List<string>();
            var pkColumns = await GetServiceViewableColumns();
            tables.Add(@$"public.""NtsService"" as ""NtsService""");
            condition.Add(@$"""NtsService"".""IsDeleted""=false");
            foreach (var item in pkColumns)
            {
                columns.Add(@$"{Environment.NewLine}""{item.TableName}"".""{item.Name}"" as ""{item.Alias}""");
            }
            tables.Add(@$"left join public.""Template"" as ""Template"" on ""NtsService"".""TemplateId""=""Template"".""Id"" and ""Template"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""Template"" as ""UdfTemplate"" on ""NtsService"".""UdfTemplateId""=""UdfTemplate"".""Id"" and ""UdfTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""OwnerUser"" on ""NtsService"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""RequestedByUser"" on ""NtsService"".""RequestedByUserId""=""RequestedByUser"".""Id"" and ""RequestedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""CreatedByUser"" on ""NtsService"".""CreatedBy""=""CreatedByUser"".""Id"" and ""CreatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""UpdatedByUser"" on ""NtsService"".""LastUpdatedBy""=""UpdatedByUser"".""Id"" and ""UpdatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""ServiceTemplate"" as ""ServiceTemplate"" on ""ServiceTemplate"".""TemplateId""=""Template"".""Id"" and ""ServiceTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""TemplateCategory"" as ""TemplateCategory"" on ""TemplateCategory"".""Id""=""Template"".""TemplateCategoryId"" and ""TemplateCategory"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""ServiceOwnerType"" on ""NtsService"".""ServiceOwnerTypeId""=""ServiceOwnerType"".""Id"" and ""ServiceOwnerType"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""ServicePriority"" on ""NtsService"".""ServicePriorityId""=""ServicePriority"".""Id"" and ""ServicePriority"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""ServiceStatus"" on ""NtsService"".""ServiceStatusId""=""ServiceStatus"".""Id"" and ""ServiceStatus"".""IsDeleted""=false");
            tables.Add(@$"left join public.""LOV"" as ""ServiceAction"" on ""NtsService"".""ServiceActionId""=""ServiceAction"".""Id"" and ""ServiceAction"".""IsDeleted""=false ");


            var selectQuery = @$"select {string.Join(",", columns)}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            if (where.IsNotNullAndNotEmpty())
            {
                selectQuery = $"{selectQuery} {where}";
            }
            return selectQuery;
        }

        public async Task<string> GetSelectQuery(TableMetadataViewModel tableMetaData, string where = null, string filtercolumns = null, string filter = null, bool isLog = false, string logId = null)
        {
            var tables = new List<string>();
            if (isLog && logId.IsNotNullAndNotEmpty())
            {
                tables.Add(@$"log.""{tableMetaData.Name}Log"" as ""{tableMetaData.Name}""");
            }
            else
            {
                tables.Add(@$"{tableMetaData.Schema}.""{tableMetaData.Name}"" as ""{tableMetaData.Name}""");
            }
            var columns = new List<string>();
            var condition = new List<string>();
            var pkColumns = await GetViewableColumns(tableMetaData);
            condition.Add(@$"""{tableMetaData.Name}"".""IsDeleted""=false");

            if (filtercolumns.IsNotNullAndNotEmpty() && filter.IsNotNullAndNotEmpty())
            {
                var filters = filtercolumns.Split(',');
                var filterValues = filter.Split(',');
                var i = 0;
                foreach (var item in filters)
                {
                    var value = filterValues[i];
                    condition.Add(@$"""{tableMetaData.Name}"".""{item}""='{value}'");
                    i++;
                }
            }
            foreach (var item in pkColumns)
            {
                if (item.Name == "LastUpdatedDateDisplay")
                {
                    columns.Add(@$"{Environment.NewLine} to_char(""{item.TableName}"".""LastUpdatedDate"",'dd-MM-yyyy HH12:mm AM') as ""{item.Alias}""");
                }
                else
                {
                    columns.Add(@$"{Environment.NewLine}""{item.TableName}"".""{item.Name}"" as ""{item.Alias}""");
                }
            }

            var fks = pkColumns.Where(x => x.TableName == tableMetaData.Name && x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty() && x.HideForeignKeyTableColumns == false).ToList();
            if (fks != null && fks.Count() > 0)
            {
                foreach (var item in fks)
                {
                    if (item.IsUdfColumn)
                    {
                        item.ForeignKeyTableAliasName = item.ForeignKeyTableName + "_" + item.Name;
                    }
                    tables.Add(@$"left join {item.ForeignKeyTableSchemaName}.""{item.ForeignKeyTableName}"" as ""{item.ForeignKeyTableAliasName}"" on ""{tableMetaData.Name}"".""{item.Name}""=""{item.ForeignKeyTableAliasName}"".""{item.ForeignKeyColumnName}"" and ""{item.ForeignKeyTableAliasName}"".""IsDeleted""=false");
                }
            }
            if (isLog && logId.IsNotNullAndNotEmpty())
            {
                tables.Add(@$"left join log.""NtsNoteLog"" as ""UdfNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""UdfNote"".""RecordId"" and ""{tableMetaData.Name}"".""VersionNo""=""UdfNote"".""VersionNo"" and ""{tableMetaData.Name}"".""IsVersionLatest""=true and ""UdfNote"".""IsVersionLatest""=true  and ""UdfNote"".""IsDeleted""=false and ""UdfNote"".""CompanyId"" = '{_repo.UserContext.CompanyId}'");
                tables.Add(@$"left join log.""NtsServiceLog"" as ""NtsService"" on ""{tableMetaData.Name}"".""NtsNoteId""=""NtsService"".""UdfNoteId""  and ""{tableMetaData.Name}"".""VersionNo""=""NtsService"".""VersionNo"" and ""{tableMetaData.Name}"".""IsVersionLatest""=true and ""NtsService"".""IsVersionLatest""=true  and ""NtsService"".""IsDeleted""=false and ""NtsService"".""CompanyId"" = '{_repo.UserContext.CompanyId}'");
            }
            else
            {
                tables.Add(@$"left join public.""NtsNote"" as ""UdfNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""UdfNote"".""Id"" and ""UdfNote"".""IsDeleted""=false ");
                tables.Add(@$"left join public.""NtsService"" as ""NtsService"" on ""UdfNote"".""Id""=""NtsService"".""UdfNoteId"" and ""NtsService"".""IsDeleted""=false ");
            }


            tables.Add(@$"left join public.""Template"" as ""Template"" on ""NtsService"".""TemplateId""=""Template"".""Id"" and ""Template"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""Template"" as ""UdfTemplate"" on ""NtsService"".""UdfTemplateId""=""UdfTemplate"".""Id"" and ""UdfTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""OwnerUser"" on ""NtsService"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""RequestedByUser"" on ""NtsService"".""RequestedByUserId""=""RequestedByUser"".""Id"" and ""RequestedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""CreatedByUser"" on ""NtsService"".""CreatedBy""=""CreatedByUser"".""Id"" and ""CreatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""UpdatedByUser"" on ""NtsService"".""LastUpdatedBy""=""UpdatedByUser"".""Id"" and ""UpdatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""ServiceTemplate"" as ""ServiceTemplate"" on ""ServiceTemplate"".""TemplateId""=""Template"".""Id"" and ""ServiceTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""TemplateCategory"" as ""TemplateCategory"" on ""TemplateCategory"".""Id""=""Template"".""TemplateCategoryId"" and ""TemplateCategory"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""ServiceOwnerType"" on ""NtsService"".""ServiceOwnerTypeId""=""ServiceOwnerType"".""Id"" and ""ServiceOwnerType"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""ServicePriority"" on ""NtsService"".""ServicePriorityId""=""ServicePriority"".""Id"" and ""ServicePriority"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""ServiceStatus"" on ""NtsService"".""ServiceStatusId""=""ServiceStatus"".""Id"" and ""ServiceStatus"".""IsDeleted""=false");
            tables.Add(@$"left join public.""LOV"" as ""ServiceAction"" on ""NtsService"".""ServiceActionId""=""ServiceAction"".""Id"" and ""ServiceAction"".""IsDeleted""=false ");


            var selectQuery = @$"select {string.Join(",", columns)}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            if (where.IsNotNullAndNotEmpty())
            {
                selectQuery = $"{selectQuery} {where}";
            }
            return selectQuery;
        }


        private async Task<string> GenerateNextServiceNo(ServiceTemplateViewModel model)
        {
            if (model.NumberGenerationType != NumberGenerationTypeEnum.SystemGenerated)
            {
                return "";
            }
            string prefix = "S";
            var today = DateTime.Today;
            var query = $@"update public.""NtsServiceSequence"" SET ""NextId"" = ""NextId""+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""SequenceDate"" = '{today.ToDatabaseDateFormat()}'
            RETURNING ""NextId""-1";
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            if (nextId == null)
            {
                nextId = 1;
                await _repo.Create<NtsServiceSequence, NtsServiceSequence>(
                    new NtsServiceSequence
                    {
                        SequenceDate = today,
                        NextId = 2,
                        CreatedBy = _repo.UserContext.UserId,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = _repo.UserContext.UserId,
                        LastUpdatedDate = DateTime.Now,
                        Status = StatusEnum.Active,
                    });
            }

            return $"{prefix}-{today.ToSequenceNumberFormat()}-{nextId}";
        }

        private async Task<ServiceTemplateViewModel> GetServiceTemplateForNewService(ServiceTemplateViewModel vm)
        {
            var query = @$"select ""TT"".*,
            ""NT"".""Json"" as ""Json"",
            ""T"".""UdfTemplateId"" as ""UdfTemplateId"",
            ""T"".""ViewType"" as ""ViewType"",
            ""T"".""Id"" as ""TemplateId"",
            ""T"".""TableMetadataId"" as ""TableMetadataId"",
            ""T"".""UdfTableMetadataId"" as ""UdfTableMetadataId"",
            ""T"".""Name"" as ""TemplateName"",
            ""T"".""DisplayName"" as ""TemplateDisplayName"",
            ""T"".""Code"" as ""TemplateCode"",
            ""TC"".""Id"" as ""TemplateCategoryId"",
            ""TC"".""Code"" as ""TemplateCategoryCode"",
            ""TC"".""Name"" as ""TemplateCategoryName"",
            ""TC"".""TemplateType"" as ""TemplateType"" ,
            ""TS"".""Id"" as ""ServiceStatusId"" ,
            ""TS"".""Code"" as ""ServiceStatusCode"" ,
            ""TS"".""Name"" as ""ServiceStatusName"" ,
            ""TA"".""Id"" as ""ServiceActionId"" ,
            ""TA"".""Code"" as ""ServiceActionCode"" ,
            ""TA"".""Name"" as ""ServiceActionName"" ,
            ""TP"".""Id"" as ""ServicePriorityId"" ,
            ""TP"".""Code"" as ""ServicePriorityCode"" ,
            ""TP"".""Name"" as ""ServicePriorityName"" 
            from public.""Template"" as ""T""
            join public.""Template"" as ""NT"" on ""T"".""UdfTemplateId""=""NT"".""Id"" and ""NT"".""IsDeleted""=false 
            join public.""TemplateCategory"" as ""TC"" on ""T"".""TemplateCategoryId""=""TC"".""Id"" and ""TC"".""IsDeleted""=false 
            join public.""ServiceTemplate"" as ""TT"" on ""TT"".""TemplateId""=""T"".""Id"" and ""TT"".""IsDeleted""=false 
            join public.""LOV"" as ""TS"" on ""TS"".""LOVType""='LOV_SERVICE_STATUS' and ""TS"".""Code""='SERVICE_STATUS_DRAFT'
            and ""TS"".""IsDeleted""=false 
            join public.""LOV"" as ""TA"" on ""TA"".""LOVType""='LOV_SERVICE_ACTION' and ""TA"".""Code""='SERVICE_ACTION_DRAFT' 
            and ""TA"".""IsDeleted""=false 
            join public.""LOV"" as ""TP"" on ""TP"".""LOVType""='SERVICE_PRIORITY' and ""TP"".""IsDeleted""=false 
            and (""TP"".""Id""=""TT"".""PriorityId"" or (""TT"".""PriorityId"" is null and ""TP"".""Code""='SERVICE_PRIORITY_MEDIUM'))
            where ""T"".""IsDeleted""=false  and (""T"".""Id""='{vm.TemplateId}' or ""T"".""Code""='{vm.TemplateCode}')";
            var data = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.UdfTableMetadataId
                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
            }
            return data;
        }

        private async Task SetReadonlyData(ServiceTemplateViewModel model)
        {
            var attachmentslist = await _repo.GetList<FileViewModel, File>(x => x.ReferenceTypeId == model.ServiceId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Service);
            var sharedlist = await _ntsServiceSharedBusiness.GetSearchResult(model.ServiceId);
            //var notificationlist = await _notificationBusiness.GetTaskNotificationList(model.TaskId, _userContex.UserId, 0);
            model.SharedCount = sharedlist.Count();
            model.AttachmentCount = attachmentslist.Count();
            model.NotificationCount = 0;// notificationlist.Count();
            model.CommentCount = 8;
        }
        private async Task<List<UserViewModel>> SetSharedList(ServiceTemplateViewModel model)
        {
            string query = @$"select n.""Id"" as Id,u.""Name"" as UserName,u.""PhotoId"" as PhotoId,u.""Email"" as Email
                              from public.""NtsServiceShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false 
                              
                              where n.""NtsServiceId""='{model.ServiceId}' and n.""IsDeleted""=false 
                                union select n.""Id"" as Id,t.""Name"" as UserName, t.""LogoId"" as PhotoId,t.""Name"" as Email
                              from public.""NtsServiceShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsServiceId""='{model.ServiceId}' and n.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
            //new List<UserViewModel> { { new UserViewModel { UserName = "Shafi", Email = "shaficmd@gmail.com" } } ,
            //{ new UserViewModel { UserName = "Shafi2", Email = "shaficmd@gmail.com2" } } };
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> DeleteService(ServiceTemplateViewModel model)
        {
            var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.UdfTableMetadataId);
            if (tableMetaData != null)
            {
                var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ");
                selectQuery.Append(Environment.NewLine);
                selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
                selectQuery.Append(@$"where ""NtsNoteId""='{model.UdfNoteId}';{Environment.NewLine}");
                selectQuery.Append(@$"update public.""NtsNote"" set ");
                selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
                selectQuery.Append(@$"where ""Id""='{model.UdfNoteId}'");
                var queryText = selectQuery.ToString();
                await _queryRepo.ExecuteCommand(queryText, null);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(model);
        }
        //public async Task GetServiceIndexPageCount(ServiceIndexPageTemplateViewModel model, PageViewModel page)
        //{
        //    var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",ts.""SharedByUserId""
        //    ,ts.""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode"",count(t.""Id"") as ""ServiceCount""      
        //    from public.""NtsService"" as t
        //    join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId""
        //    left join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and l.""IsDeleted""=false
        //    where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false and l.""IsDeleted""=false
        //    group by t.""OwnerUserId"",t.""RequestedByUserId"",ts.""SharedByUserId""
        //    ,ts.""SharedWithUserId"",l.""Code"" ";

        //    var result = await _queryRepo.ExecuteQueryList<ServiceTemplateViewModel>(query, null);
        //    var activeUserId = _repo.UserContext.UserId;
        //    var ownerOrRequester = result.Where(x => x.OwnerUserId == activeUserId || x.RequestedByUserId == activeUserId);
        //    if (ownerOrRequester != null && ownerOrRequester.Count() > 0)
        //    {
        //        model.CreatedOrRequestedByMeDraftCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
        //        model.CreatedOrRequestedByMeInProgreessOverDueCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
        //        model.CreatedOrRequestedByMeCompleteCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
        //        model.CreatedOrRequestedByMeRejectCancelCloseCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
        //    }

        //    var sharedWith = result.Where(x => x.SharedWithUserId == activeUserId);
        //    if (sharedWith != null && sharedWith.Count() > 0)
        //    {
        //        model.SharedWithMeDraftCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
        //        model.SharedWithMeInProgressOverDueCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
        //        model.SharedWithMeCompleteCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
        //        model.SharedWithMeRejectCancelCloseCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
        //    }
        //    var sharedBy = result.Where(x => x.SharedByUserId == activeUserId);
        //    if (sharedBy != null && sharedBy.Count() > 0)
        //    {
        //        model.SharedByMeDraftCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
        //        model.SharedByMeInProgreessOverDueCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
        //        model.SharedByMeCompleteCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
        //        model.SharedByMeRejectCancelCloseCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
        //    }
        //}
        public async Task GetServiceIndexPageCount(ServiceIndexPageTemplateViewModel model, PageViewModel page)
        {
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode"",count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false  
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedWithUserId"",l.""Code""";

            var udfjoin = "";
            var template = await _repo.GetSingleById<TemplateViewModel, Template>(page.TemplateId);
            if (template.IsNotNull())
            {
                var udfTableMetadataId = template.UdfTableMetadataId;
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(udfTableMetadataId);
                if (tableMetadata.IsNotNull())
                {
                    udfjoin = $@"join {tableMetadata.Schema}.""{tableMetadata.Name}"" as udf on udf.""NtsNoteId""=n.""Id"" and udf.""IsDeleted""=false";
                }
            }

            query = query.Replace("#UDFJOIN#", udfjoin);

            var result = await _queryRepo.ExecuteQueryList<ServiceTemplateViewModel>(query, null);
            var activeUserId = _repo.UserContext.UserId;
            var ownerOrRequester = result.Where(x => x.OwnerUserId == activeUserId || x.RequestedByUserId == activeUserId);
            if (ownerOrRequester != null && ownerOrRequester.Count() > 0)
            {
                model.CreatedOrRequestedByMeDraftCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
                model.CreatedOrRequestedByMeInProgreessOverDueCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
                model.CreatedOrRequestedByMeCompleteCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
                model.CreatedOrRequestedByMeRejectCancelCloseCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
            }

            var sharedWith = result.Where(x => x.SharedWithUserId == activeUserId);
            if (sharedWith != null && sharedWith.Count() > 0)
            {
                model.SharedWithMeDraftCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
                model.SharedWithMeInProgressOverDueCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
                model.SharedWithMeCompleteCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
                model.SharedWithMeRejectCancelCloseCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
            }
            var sharedBy = result.Where(x => x.SharedByUserId == activeUserId);
            if (sharedBy != null && sharedBy.Count() > 0)
            {
                model.SharedByMeDraftCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
                model.SharedByMeInProgreessOverDueCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
                model.SharedByMeCompleteCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
                model.SharedByMeRejectCancelCloseCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
            }
        }
        public async Task<ServiceIndexPageTemplateViewModel> GetServiceIndexPageViewModel(PageViewModel page)
        {
            var Service = await _repo.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == page.TemplateId);
            if (Service != null)
            {
                var model = await _repo.GetSingleById<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate>(Service.ServiceIndexPageTemplateId);
                //var cloumns = await _repo.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == model.Id && x.IsDeleted == false, x => x.ColumnMetadata);
                //var cloumns = new List<ServiceIndexPageColumnViewModel>();
                //foreach (var item in indexPageColumns)
                //{
                //    item.ColumnName = item.ColumnMetadata.Name;
                //    cloumns.Add(item);
                //}
                model.SelectedTableRows = await _repo.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == model.Id && x.IsDeleted == false, x => x.ColumnMetadata); ;
                await GetServiceIndexPageCount(model, page);
                //await BindServiceIndexPageCounts(model, page);
                return model;
            }
            return null;
        }


        public async Task<DataTable> GetServiceIndexPageGridData(DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string serviceStatusCode)
        {
            //throw new NotImplementedException();
            var indexPageTemplate = await _repo.GetSingleById<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate>(indexPageTemplateId);
            if (indexPageTemplate != null)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(indexPageTemplate.TemplateId);
                if (template != null)
                {
                    var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(template.UdfTableMetadataId);
                    if (tableMetadata != null)
                    {

                        var filter = "";
                        switch (ownerType)
                        {
                            case NtsActiveUserTypeEnum.Requester:
                                filter = @$" and (""NtsService"".""OwnerUserId""<>'{_repo.UserContext.UserId}' and ""NtsService"".""RequestedByUserId""='{_repo.UserContext.UserId}')";
                                break;
                            case NtsActiveUserTypeEnum.Owner:
                                filter = @$" and (""NtsService"".""OwnerUserId""='{_repo.UserContext.UserId}')";
                                break;
                            case NtsActiveUserTypeEnum.OwnerOrRequester:
                                filter = @$" and (""NtsService"".""OwnerUserId""='{_repo.UserContext.UserId}' or ""NtsService"".""RequestedByUserId""='{_repo.UserContext.UserId}')";
                                break;
                            case NtsActiveUserTypeEnum.SharedWith:
                                filter = @$" and (""NtsService"".""Id"" in ( Select ts.""NtsServiceId"" from public.""NtsServiceShared"" as ts where ts.""IsDeleted""=false  and ts.""SharedWithUserId""='{_repo.UserContext.UserId}') ) ";
                                break;
                            case NtsActiveUserTypeEnum.SharedBy:
                                filter = @$" and (""NtsService"".""Id"" in ( Select ts.""NtsServiceId"" from public.""NtsServiceShared"" as ts where ts.""IsDeleted""=false  and ts.""SharedByUserId""='{_repo.UserContext.UserId}') ) ";
                                break;
                            default:
                                break;
                        }
                        if (serviceStatusCode.IsNotNullAndNotEmpty())
                        {
                            var stausItems = serviceStatusCode.Split(',');
                            var statusText = "";
                            foreach (var item in stausItems)
                            {
                                statusText = $"{statusText},'{item}'";
                            }

                            filter = @$" {filter} and (""ServiceStatus"".""Code"" in ({statusText.Trim(',')}))";
                        }
                        var selectQuery = await GetSelectQuery(tableMetadata, filter);
                        if (indexPageTemplate.OrderByColumnId.IsNotNullAndNotEmpty())
                        {
                            var orderColumn = await _repo.GetSingleById<ColumnMetadataViewModel, ColumnMetadata>(indexPageTemplate.OrderByColumnId);
                            if (orderColumn != null)
                            {
                                var oderBy = "asc";
                                if (indexPageTemplate.OrderBy == OrderByEnum.Descending)
                                {
                                    oderBy = "desc";
                                }
                                selectQuery = @$"{selectQuery} order by ""{orderColumn.Name}"" {oderBy}";
                            }
                            else
                            {
                                selectQuery = @$"{selectQuery} order by ""LastUpdatedDate"" desc";
                            }

                        }
                        else
                        {
                            selectQuery = @$"{selectQuery} order by ""LastUpdatedDate"" desc";
                        }
                        var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                        return dt;

                    }
                }
            }
            return new DataTable();

        }
        public async Task<DataTable> GetCustomServiceIndexPageGridData(DataSourceRequest request, string templateId, bool showAllOwnersService, string moduleCodes, string templateCodes, string categoryCodes)
        {
            var indexPageTemplate = await _repo.GetSingle<CustomIndexPageTemplateViewModel, CustomIndexPageTemplate>(x => x.TemplateId == templateId);
            if (indexPageTemplate != null)
            {
                var where = "";
                if (!showAllOwnersService)
                {
                    where = @$" {where} and (""NtsService"".""OwnerUserId""='{_userContext.UserId}' or ""NtsService"".""RequestedByUserId""='{_userContext.UserId}')";
                }
                if (templateCodes.IsNotNullAndNotEmpty())
                {
                    var templates = templateCodes.Replace(",", "','");
                    where = @$" {where} and (""Template"".""Code"" in ('{templates}')) ";
                }
                if (categoryCodes.IsNotNullAndNotEmpty())
                {
                    var categories = categoryCodes.Replace(",", "','");
                    where = @$" {where} and (""TemplateCategory"".""Code"" in ('{categories}')) ";
                }
                var selectQuery = await GetServiceSelectQuery();
                selectQuery = $"{selectQuery} {where}";
                if (indexPageTemplate.OrderByColumnId.IsNotNullAndNotEmpty())
                {
                    var orderColumn = await _repo.GetSingleById<ColumnMetadataViewModel, ColumnMetadata>(indexPageTemplate.OrderByColumnId);
                    if (orderColumn != null)
                    {
                        var oderBy = "asc";
                        if (indexPageTemplate.OrderBy == OrderByEnum.Descending)
                        {
                            oderBy = "desc";
                        }
                        selectQuery = @$"{selectQuery} order by ""{orderColumn.Name}"" {oderBy}";
                    }
                    else
                    {
                        selectQuery = @$"{selectQuery} order by ""NtsService"".""LastUpdatedDate"" desc";
                    }

                }
                else
                {
                    selectQuery = @$"{selectQuery} order by ""NtsService"".""LastUpdatedDate"" desc";
                }
                var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                return dt;
            }
            return new DataTable();

        }
        public async Task<IList<ServiceViewModel>> GetNtsServiceIndexPageGridData(DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string serviceStatusCode, string categoryCode, string templateCode, string moduleCode)
        {

            var query = @$" Select t.*,tmpc.""Code"" as ""TemplateCategoryCode"",ou.""Name"" as ""OwnerUserName"",ru.""Name"" as ""RequestedByUserName""
                        ,cu.""Name"" as ""CreatedByUserName"",lu.""Name"" as ""LastUpdatedByUserName""
                        , tlov.""Name"" as ""ServiceStatusName"",tlov.""Code"" as ""ServiceStatusCode"",m.""Name"" as Module
from public.""NtsService"" as t
                        left join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false
                         left join public.""Module"" as m ON m.""Id""=tmp.""ModuleId"" and m.""IsDeleted""=false 
left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
                        left join public.""LOV"" as tlov on t.""ServiceStatusId""=tlov.""Id"" and tlov.""IsDeleted""=false 
                       
                        left join public.""User"" as ou ON ou.""Id""=t.""OwnerUserId"" and ou.""IsDeleted""=false 
                        left join public.""User"" as ru ON ru.""Id""=t.""RequestedByUserId"" and ru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=t.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=t.""LastUpdatedBy"" and lu.""IsDeleted""=false 
                        #WHERE# 
                        ORDER BY t.""LastUpdatedDate"" DESC ";
            var where = @$" WHERE 1=1 ";
            switch (ownerType)
            {
                case NtsActiveUserTypeEnum.Assignee:
                    where += @$" and t.""AssignedToUserId""='{_repo.UserContext.UserId}'";
                    break;
                case NtsActiveUserTypeEnum.OwnerOrRequester:
                    where += @$" and (t.""OwnerUserId""='{_repo.UserContext.UserId}' or t.""RequestedByUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.Requester:
                    where += @$" and (t.""OwnerUserId""<>'{_repo.UserContext.UserId}' and t.""RequestedByUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.Owner:
                    where += @$" and (t.""OwnerUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.SharedWith:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsServiceId"" from public.""NtsServiceShared"" as ts where ts.""IsDeleted""=false and ts.""SharedWithUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                case NtsActiveUserTypeEnum.SharedBy:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsServiceId"" from public.""NtsServiceShared"" as ts where ts.""IsDeleted""=false  and ts.""SharedByUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                default:
                    break;
            }
            if (serviceStatusCode.IsNotNullAndNotEmpty())
            {
                var servicestatus = $@" and tlov.""Code""='{serviceStatusCode}' ";
                where = where + servicestatus;
            }
            if (moduleCode.IsNotNullAndNotEmpty())
            {
                var module = $@" and m.""Code""='{moduleCode}' ";
                where = where + module;
            }
            if (templateCode.IsNotNullAndNotEmpty())
            {
                var tempItems = templateCode.Split(',');
                var tempText = "";
                foreach (var item in tempItems)
                {
                    tempText = $"{tempText},'{item}'";
                }
                //var tempcode = $@" and tmp.""Code""='{templateCode}' ";
                var tempcode = $@" and tmp.""Code"" IN ({tempText.Trim(',')}) ";
                where = where + tempcode;
            }
            if (categoryCode.IsNotNullAndNotEmpty())
            {
                var categoryItems = categoryCode.Split(',');
                var categoryText = "";
                foreach (var item in categoryItems)
                {
                    categoryText = $"{categoryText},'{item}'";
                }
                //var categorycode = $@" and tmpc.""Code""='{categoryCode}' ";
                var categorycode = $@" and tmpc.""Code"" IN ({categoryText.Trim(',')}) ";
                where = where + categorycode;
            }
            query = query.Replace("#WHERE#", where);
            var querydata = await _queryRepo.ExecuteQueryList(query, null);
            return querydata;
        }

        public async Task GetNtsServiceIndexPageCount(NtsServiceIndexPageViewModel model)
        {
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode"",count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" 
            join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",l.""Code"" 
            
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  ts.""SharedByUserId"",l.""Code"" 
            
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  ts.""SharedWithUserId"",l.""Code""";

            var where = "";
            if (model.TemplateCode.IsNotNullAndNotEmpty())
            {
                var tempItems = model.TemplateCode.Split(',');
                var tempText = "";
                foreach (var item in tempItems)
                {
                    tempText = $"{tempText},'{item}'";
                }
                //var tempcode = $@" and tmp.""Code""='{templateCode}' ";
                var tempcode = $@" and tmp.""Code"" IN ({tempText.Trim(',')}) ";
                where = where + tempcode;
            }
            if (model.CategoryCode.IsNotNullAndNotEmpty())
            {
                var categoryItems = model.CategoryCode.Split(',');
                var categoryText = "";
                foreach (var item in categoryItems)
                {
                    categoryText = $"{categoryText},'{item}'";
                }
                //var categorycode = $@" and tmpc.""Code""='{categoryCode}' ";
                var categorycode = $@" and tmpc.""Code"" IN ({categoryText.Trim(',')}) ";
                where = where + categorycode;
            }
            query = query.Replace("#WHERE#", where);

            var result = await _queryRepo.ExecuteQueryList<ServiceTemplateViewModel>(query, null);
            var activeUserId = _repo.UserContext.UserId;
            var ownerOrRequester = result.Where(x => x.OwnerUserId == activeUserId || x.RequestedByUserId == activeUserId);
            if (ownerOrRequester != null && ownerOrRequester.Count() > 0)
            {
                model.CreatedOrRequestedByMeDraftCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
                model.CreatedOrRequestedByMeInProgreessOverDueCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
                model.CreatedOrRequestedByMeCompleteCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
                model.CreatedOrRequestedByMeRejectCancelCloseCount = ownerOrRequester.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
            }

            var sharedWith = result.Where(x => x.SharedWithUserId == activeUserId);
            if (sharedWith != null && sharedWith.Count() > 0)
            {
                model.SharedWithMeDraftCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
                model.SharedWithMeInProgressOverDueCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
                model.SharedWithMeCompleteCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
                model.SharedWithMeRejectCancelCloseCount = sharedWith.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
            }
            var sharedBy = result.Where(x => x.SharedByUserId == activeUserId);
            if (sharedBy != null && sharedBy.Count() > 0)
            {
                model.SharedByMeDraftCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Sum(x => x.ServiceCount);
                model.SharedByMeInProgreessOverDueCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Sum(x => x.ServiceCount);
                model.SharedByMeCompleteCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Sum(x => x.ServiceCount);
                model.SharedByMeRejectCancelCloseCount = sharedBy.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").Sum(x => x.ServiceCount);
            }
        }


        public async Task<WorklistDashboardSummaryViewModel> GetWorklistDashboardCount(string userId, string moduleCodes = null, string templateCategoryCode = null, string taskTemplateIds = null, string serviceTemplateIds = null)
        {

            var cypher = $@"Select ns.""Code"" as ServiceStatusCode,u.""Id"" as OwnerUserId
                            from public.""NtsService"" as s                         
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false  
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false 
                            join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
                           left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #SERVICETEMPLATESEARCH#";
            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            var Templatesearch = "";

            if (serviceTemplateIds.IsNotNullAndNotEmpty())
            {
                serviceTemplateIds = serviceTemplateIds.Replace(",", "','");
                Templatesearch = $@" and tr.""Id"" in ('{serviceTemplateIds}')";
            }
            var taskTemplatesearch = "";
            if (taskTemplateIds.IsNotNullAndNotEmpty())
            {
                taskTemplateIds = taskTemplateIds.Replace(",", "','");
                taskTemplatesearch = $@" and tr.""Id"" in ('{taskTemplateIds}')";
            }
            cypher = cypher.Replace("#WHERE#", search);
            cypher = cypher.Replace("#SERVICETEMPLATESEARCH#", Templatesearch);
            var goalservice = await _queryRepo.ExecuteQueryList(cypher, null);
            var service = goalservice.Where(x => x.OwnerUserId == userId);
            var reqByMe = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE" || x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Count();
            var reqByMeOverdue = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Count();
            var reqByMePending = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").Count();
            var reqByMeCompleted = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Count();
            var reqByMeDraft = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").Count();
            var reqByMeCancel = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_CANCEL").Count();


            var cypher1 = $@"Select ns.""Code"" as ServiceStatusCode
from public.""NtsService"" as s
join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and s.""IsDeleted""=false and ss.""IsDeleted""=false 
join public.""LOV"" as sst on sst.""Id""=ss.""ServiceSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_USER' 
join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false  
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false   where u.""Id""='{userId}' and s.""PortalId""='{_userContext.PortalId}' #WHERE# #SERVICETEMPLATESEARCH#

union
Select ns.""Code"" as ServiceStatusCode
from public.""User"" as u
join public.""TeamUser"" as tu on tu.""UserId""=u.""Id"" and u.""IsDeleted""=false and tu.""IsDeleted""=false  
join public.""NtsServiceShared"" as ss on ss.""SharedWithTeamId""=tu.""TeamId"" and ss.""IsDeleted""=false 
join public.""LOV"" as sst on sst.""Id""=ss.""ServiceSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_TEAM' 
join public.""NtsService"" as s on s.""Id""=ss.""NtsServiceId"" and s.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""IsDeleted""=false
join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where u.""Id""='{userId}' and s.""PortalId""='{_userContext.PortalId}' #WHERE# #SERVICETEMPLATESEARCH# ";
            //var search1 = "";

            //if (moduleCodes.IsNotNullAndNotEmpty())
            //{
            //    //search1 = $@" and m.""Code""='{moduleCode}'";
            //    if (codes.IsNotNullAndNotEmpty())
            //    {
            //        search1 = $@" and m.""Code"" in (" + codes + ")";
            //    }
            //}

            cypher1 = cypher1.Replace("#WHERE#", search);
            cypher1 = cypher1.Replace("#SERVICETEMPLATESEARCH#", Templatesearch);
            var serviceShare = await _queryRepo.ExecuteQueryList(cypher1, null);
            //var serviceShare = goalservice.Where(x => share.Contains(x.ServiceId)).DistinctBy(x => x.ServiceId);
            var shareWithMe = serviceShare.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE" || x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Count();
            var shareWithMeOverdue = serviceShare.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Count();
            var shareWithPending = serviceShare.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").Count();
            var shareWithCompleted = serviceShare.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE").Count();
            var shareWithCancel = serviceShare.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_CANCEL").Count();

            var cypher2 = $@"Select ns.""Code"" as TaskStatusCode,u.""Id"" as OwnerUserId,ua.""Id"" as AssignedToUserId
                            from public.""NtsTask"" as s
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false  
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false
							left join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #TASKTEMPLATESEARCH#
                           ";
            cypher2 = cypher2.Replace("#WHERE#", search);
            cypher2 = cypher2.Replace("#TASKTEMPLATESEARCH#", taskTemplatesearch);
            var tasklist = await _queryTaskRepo.ExecuteQueryList(cypher2, null);
            var taskreq = tasklist.Where(x => x.OwnerUserId == userId);
            var t_reqByMe = taskreq.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE" || x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_COMPLETE" || x.TaskStatusCode == "TASK_STATUS_DRAFT").Count(); ;
            var t_reqByMeOverdue = taskreq.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count();
            var t_reqByMePending = taskreq.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count();
            var t_reqByMeCompleted = taskreq.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Count();
            var t_reqByMeDraft = taskreq.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT" && x.OwnerUserId == userId).Count();

            var task = tasklist.Where(x => x.AssignedToUserId == userId);
            var assignToMe = task.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE" || x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_COMPLETE").Count();
            var assToMeOverdue = task.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count();
            var assToMePending = task.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count();
            var assToMeCompleted = task.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Count();
            var assToMeRejected = task.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT").Count();

            var cypher3 = $@"Select ns.""Code"" as TaskStatusCode,s.""Id""
from public.""NtsTask"" as s
join public.""NtsTaskShared"" as ss on ss.""NtsTaskId""=s.""Id""  and s.""IsDeleted""=false  and ss.""IsDeleted""=false 
join public.""LOV"" as sst on sst.""Id""=ss.""TaskSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_USER' 
join public.""User"" as u on u.""Id""=ss.""SharedWithUserId""  and u.""IsDeleted""=false and u.""Id""='{userId}' 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #TASKTEMPLATESEARCH# 

union
Select ns.""Code"" as TaskStatusCode,s.""Id""
from public.""User"" as u
join public.""TeamUser"" as tu on tu.""UserId""=u.""Id""  and tu.""IsDeleted""=false  and u.""IsDeleted""=false and u.""Id""='{userId}' 
join public.""NtsTaskShared"" as ss on ss.""SharedWithTeamId""=tu.""TeamId""   and ss.""IsDeleted""=false 
join public.""LOV"" as sst on sst.""Id""=ss.""TaskSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_TEAM' 
join public.""NtsTask"" as s on s.""Id""=ss.""NtsTaskId""  and s.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #TASKTEMPLATESEARCH# ";
            cypher3 = cypher3.Replace("#WHERE#", search);
            cypher3 = cypher3.Replace("#TASKTEMPLATESEARCH#", taskTemplatesearch);
            var taskShared = await _queryTaskRepo.ExecuteQueryList(cypher3, null);
            var tskshareWithMe = taskShared.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE" || x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_COMPLETE").Count();
            var tskshareWithMeOverdue = taskShared.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count();
            var tskshareWithPending = taskShared.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count();
            var tskshareWithCompleted = taskShared.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Count();
            var tskshareWithRejected = taskShared.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT").Count();

            var count = new WorklistDashboardSummaryViewModel()
            {
                S_RequestedByMe = Convert.ToInt64(reqByMe),
                S_RequestOverdue = Convert.ToInt64(reqByMeOverdue),
                S_RequestCompleted = Convert.ToInt64(reqByMeCompleted),
                S_RequestDraft = Convert.ToInt64(reqByMeDraft),
                S_RequestPending = Convert.ToInt64(reqByMePending),
                S_RequestCancel = Convert.ToInt64(reqByMeCancel),
                S_SharedWithMe = Convert.ToInt64(shareWithMe),
                S_ShareWithPending = Convert.ToInt64(shareWithPending),
                S_ShareWithCompleted = Convert.ToInt64(shareWithCompleted),
                S_ShareWithOverdue = Convert.ToInt64(shareWithMeOverdue),
                S_ShareWithCancel = Convert.ToInt64(shareWithCancel),
                T_RequestedByMe = Convert.ToInt64(t_reqByMe),
                T_RequestOverdue = Convert.ToInt64(t_reqByMeOverdue),
                T_RequestCompleted = Convert.ToInt64(t_reqByMeCompleted),
                T_RequestDraft = Convert.ToInt64(t_reqByMeDraft),
                T_RequestPending = Convert.ToInt64(t_reqByMePending),
                T_AssignedToMe = Convert.ToInt64(assignToMe),
                T_AssignPending = Convert.ToInt64(assToMePending),
                T_AssignCompleted = Convert.ToInt64(assToMeCompleted),
                T_AssignOverdue = Convert.ToInt64(assToMeOverdue),
                T_AssignReject = Convert.ToInt64(assToMeRejected),
                T_SharedWithMe = Convert.ToInt64(tskshareWithMe),
                T_ShareWithPending = Convert.ToInt64(tskshareWithPending),
                T_ShareWithCompleted = Convert.ToInt64(tskshareWithCompleted),
                T_ShareWithOverdue = Convert.ToInt64(tskshareWithMeOverdue),
                T_ShareWithReject = Convert.ToInt64(tskshareWithRejected)

            };
            return count;
        }
        public async Task<IList<ServiceViewModel>> GetSearchResult(ServiceSearchViewModel searchModel)
        {
            //var hour = GeneralExtension.ServerToLocalTimeDiff(LegalEntityCode);
            var userId = searchModel.UserId;
            var cypher = $@"select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
,'Owner' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
from
public.""NtsService"" as n
    join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false and n.""IsDeleted""=false 
  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL#
                    union
                   select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
,'Requester' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
from
public.""NtsService"" as n
join public.""User"" as nru on n.""RequestedByUserId""=nru.""Id""	 and nru.""Id""='{userId}' and n.""IsDeleted""=false 
  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
 left  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL#
                    union
                                     select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
,'Shared' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
from
public.""NtsService"" as n
join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=n.""Id"" and ss.""IsDeleted""=false and n.""IsDeleted""=false 
join public.""LOV"" as sw on  sw.""LOVType""='SHARED_TYPE' and sw.""Code""='SHARED_TYPE_USER' and ss.""ServiceSharedWithTypeId""=sw.""Id"" 
join public.""User"" as nsu on ss.""SharedWithUserId"" = nsu.""Id"" and nsu.""IsDeleted""=false  and nsu.""Id""='{userId}'  
  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
 left  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
left join public.""User"" as nru on n.""RequestedByUserId""=nru.""Id""
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL#
                    union
                   select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
,'Shared' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
from
public.""NtsService"" as n
  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false and n.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
   join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=n.""Id""  
join public.""LOV"" as sw on  sw.""LOVType""='SHARED_TYPE' and sw.""Code""='SHARED_TYPE_TEAM' and ss.""ServiceSharedWithTypeId""=sw.""Id""
join public.""Team"" as ts on ss.""SharedWithTeamId"" = ts.""Id"" 
join public.""TeamUser"" as tu on ts.""Id"" = tu.""Id"" 
join public.""User"" as nsu on tu.""UserId"" = nsu.""Id"" 
 left  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false
left join public.""User"" as nru on n.""RequestedByUserId""=nru.""Id""	
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL# ";
            // var prms = new Dictionary<string, object>();
            //  prms.Add("Status", StatusEnum.Active.ToString());
            //prms.Add("CompanyId", CompanyId);
            //prms.Add("UserId", userId);
            //  prms.Add("hour", hour);

            var portalwhere = "";
            if (searchModel != null && searchModel.PortalNames.IsNotNullAndNotEmpty())
            {
                portalwhere = $@" where n.""PortalId"" in ('{searchModel.PortalNames}') ";
            }
            else
            {
                portalwhere = $@" where n.""PortalId"" in ('{_repo.UserContext.PortalId}') ";
            }
            cypher = cypher.Replace("#PORTAL#", portalwhere);

            var list = await _queryRepo.ExecuteQueryList(cypher, null);

            if (searchModel != null)
            {
                if (searchModel.ModuleCode.IsNotNull())
                {
                    var modules = searchModel.ModuleCode.Split(",");
                    list = list.Where(x => x.ModuleCode != null && modules.Any(y => y == x.ModuleCode)).ToList();
                }
                if (searchModel.ModuleId.IsNotNull())
                {
                    //list = list.Where(x => x.ModuleId == searchModel.ModuleId).ToList();
                    var modules = searchModel.ModuleId.Split(",");
                    list = list.Where(x => x.ModuleId != null && modules.Any(y => y == x.ModuleId)).ToList();
                }
                if (searchModel.TemplateMasterCode.IsNotNull())
                {
                    var modules = searchModel.TemplateMasterCode.Split(",");
                    list = list.Where(x => x.TemplateMasterCode != null && modules.Any(y => y == x.TemplateMasterCode)).ToList();
                }
                if (searchModel.TemplateCategoryCode.IsNotNull())
                {
                    var modules = searchModel.TemplateCategoryCode.Split(",");
                    list = list.Where(x => x.TemplateCategoryCode != null && modules.Any(y => y == x.TemplateCategoryCode)).ToList();
                }
                //if (searchModel.TemplateCategoryCode.IsNotNullAndNotEmpty())
                //{
                //    list = list.Where(x => x.TemplateCategoryCode == searchModel.TemplateCategoryCode).ToList();
                //}
                //if (searchModel.TemplateMasterCode.IsNotNullAndNotEmpty())
                //{
                //    list = list.Where(x => x.TemplateMasterCode == searchModel.TemplateMasterCode).ToList();
                //}

                if (searchModel.Mode.IsNotNullAndNotEmpty())
                {
                    var modes = searchModel.Mode.Split(',');
                    if (modes.Any(y => y == "REQ_BY"))
                        list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Owner).ToList();
                    if (modes.Any(y => y == "SHARE_TO"))
                        list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Shared).ToList();
                    //if ("REQ_BY".Equals(searchModel.Mode))
                    //    list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Owner).ToList();
                    //if ("SHARE_TO".Equals(searchModel.Mode))
                    //    list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Shared).ToList();
                }
                if (searchModel.FilterUserId.IsNotNull())
                {
                    list = list.Where(x => x.OwnerUserId == searchModel.FilterUserId).ToList();
                }
                if (searchModel.ServiceNo.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.ServiceNo.Contains(searchModel.ServiceNo, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }

                if (searchModel.ServiceStatus.IsNotNull())
                {
                    var serviceList = searchModel.ServiceStatus.Split(',');
                    list = list.Where(x => serviceList.Any(y => y == x.ServiceStatusCode)).ToList();
                    // list = list.Where(x => x.ServiceStatusCode == searchModel.ServiceStatus).ToList();
                }

                if (searchModel.Subject.IsNotNull())
                {
                    list = list.Where(x => x.ServiceSubject.IsNotNullAndNotEmpty() && x.ServiceSubject.Contains(searchModel.Subject, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
                if (searchModel.StartDate.IsNotNull())
                {
                    if (searchModel.StartToDate.IsNotNull())
                    {
                        list = list.Where(x => (x.StartDate >= searchModel.StartDate && x.StartDate <= searchModel.StartToDate)).ToList();
                    }
                    else
                    {
                        list = list.Where(x => x.StartDate.Value.Date == searchModel.StartDate.Value.Date).ToList();
                    }
                }
                if (searchModel.DueDate.IsNotNull())
                {
                    if (searchModel.DueDate.IsNotNull())
                    {
                        list = list.Where(x => (x.DueDate.Value.Date >= searchModel.DueDate.Value.Date && x.DueDate.Value.Date <= searchModel.DueDate.Value.Date)).ToList();
                    }
                    else
                    {
                        list = list.Where(x => x.DueDate.Value.Date == searchModel.DueDate.Value.Date).ToList();
                    }
                }
                if (searchModel.CompletionDate.IsNotNull())
                {
                    if (searchModel.CompletionDate.IsNotNull())
                    {
                        list = list.Where(x => (x.CompletionDate >= searchModel.CompletionDate && x.CompletionDate <= searchModel.CompletionDate)).ToList();
                    }
                    else
                    {
                        list = list.Where(x => x.CompletionDate.Value.Date == searchModel.CompletionDate.Value.Date).ToList();
                    }
                }
                if (searchModel.ClosedDate.IsNotNull())
                {
                    list = list.Where(x => (x.ClosedDate.Value.Date == searchModel.ClosedDate.Value.Date)).ToList();
                }

                if (searchModel.TemplateCategoryCode.IsNotNull())
                {
                    list = list.Where(x => x.TemplateCategoryCode == searchModel.TemplateCategoryCode).ToList();
                }
                if (searchModel.TemplateCategoryType.IsNotNull())
                {
                    list = list.Where(x => x.TemplateCategoryType == searchModel.TemplateCategoryType).ToList();
                }

            }

            list = list.Distinct(new ServiceViewModelComparer()).OrderByDescending(x => x.CreatedDate).ToList();/*.DistinctBy(x => x.Id)*/;

            return list;
        }

        public async Task<bool> IsServiceSubjectUnique(string templateId, string subject, string serviceId)
        {
            var query = $@"select * from public.""NtsService"" 
where ""TemplateId""='{templateId}' and  LOWER(""ServiceSubject"")=LOWER('{subject}') and ""Id""<> '{serviceId}' and ""IsDeleted""=false limit 1";
            var data = await _queryRepo.ExecuteQuerySingle(query, null);
            if (data != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseServiceSLA(ServiceSearchViewModel searchModel)
        {
            var userId = searchModel.UserId;
            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where n.""ServiceStatusId""='68fabc87-83fd-4975-afd6-e89fa74d51c4' and n.""IsDeleted""=false  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                            group by n.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryChart.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();

            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }
        public async Task<List<IdNameViewModel>> GetSharedList(string ServiceId)
        {
            string query = @$"select n.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsServiceShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" AND u.""IsDeleted""= false 
                              
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false 
                                union select u.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsServiceShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" AND t.""IsDeleted""= false 
 join public.""TeamUser"" as tu ON tu.""TeamId"" = n.""SharedWithTeamId"" AND tu.""IsDeleted""= false 
join public.""User"" as u ON u.""Id"" = tu.""UserId"" AND u.""IsDeleted""= false 
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list.Distinct().ToList();

        }
        public async Task<bool> IsExitEntryFeeAvailed(string userId)
        {
            //var param = Helper.GenerateCypherParameter("UserId", userId, "CompanyId", CompanyId, "Status", StatusEnum.Active);

            //var result = ExecuteCypherList<ServiceViewModel>(@"match (tr:NTS_TemplateMaster{IsDeleted:0,Status: { Status}})  
            //            where tr.Code in['ANNUAL_LEAVE','ANNUAL_LEAVE_ADV']
            //  match (tr)<-[:R_TemplateRoot]-(t:NTS_Template{IsDeleted:0,Status: { Status}})  
            //  match(t)<-[:R_Service_Template]-(s:NTS_Service{ IsDeleted: 0,Status: { Status},CompanyId: { CompanyId}})-[:R_Service_Owner_User]->(u: ADM_User{ Id:{UserId},IsDeleted: 0,Status: { Status},CompanyId: { CompanyId} })
            //    where s.TemplateAction IN ['Complete','Submit','Overdue']
            //     match (s)<-[:R_ServiceFieldValue_Service]-(nfv1:NTS_ServiceFieldValue{IsDeleted:0})-[:R_ServiceFieldValue_TemplateField]->(tf1{FieldName:'IsEligibleExitReEntryFee',IsDeleted:0})
            //     where nfv1.Code='YES'
            // RETURN s", param).Any();
            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>($@"Select s.* from 
public.""Template"" as t 
join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and t.""Code""='AnnualLeave' and s.""IsDeleted""=false 
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false 
join cms.""N_Leave_AnnualLeave"" as al on al.""NtsNoteId""=s.""UdfNoteId"" and al.""IsDeleted""=false
where lov.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and s.""OwnerUserId""='{userId}'
and t.""IsDeleted""=false
union
Select s.* from 
public.""Template"" as t 
join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and t.""Code""='ANNUAL_LEAVE_ADV' and s.""IsDeleted""=false 
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false 
--join cms.""N_Leave_AnnualLeaveADV"" as al on al.""NtsNoteId""=s.""UdfNoteId"" 
where lov.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and s.""OwnerUserId""='{userId}'
and t.""IsDeleted""=false 

", null);
            return result.Any();

        }
        public async Task<NoteViewModel> GetFBDashboardCount(string userId, string moduleId = null)
        {
            var photoB64 = "";
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var userdata = await _userBusiness.ViewModelList(userId);
            var userViewModel = userdata.FirstOrDefault();
            if (userViewModel.PersonId != null)
            {
                var photo = await _fileBusiness.GetFileByte(userViewModel.PhotoId);
                if (photo != null)
                {
                    var base64String = Convert.ToBase64String(photo, 0, photo.Length);
                    photoB64 = photo != null ? "data:image/png;base64," + base64String : "";
                }

            }
            var cypher = $@"select ns.""Code"" as ServiceStatusCode,u.""Id"" as OwnerUserId
from public.""NtsService"" as s
join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false 
where u.""Id""='{userId}' and s.""IsDeleted""=false  #WHERE#  
";

            var search = "";
            if (moduleId.IsNotNullAndNotEmpty())
            {
                search = $@" and t.ModuleId='{moduleId}'";

            }
            cypher = cypher.Replace("#WHERE#", search);



            var goalservice = await _queryRepo.ExecuteQueryList<ServiceViewModel>(cypher, null);

            var service = goalservice.Where(x => x.OwnerUserId == userId);
            var reqByMe = service.Count();
            var reqByMeOverdue = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").Count();
            var reqByMePending = service.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").Count();
            var cypher2 = $@"select ns.""Code"" as TaskStatusCode,u.""Id"" as OwnerUserId,ua.""Id"" as AssignedToUserId
from public.""NtsTask"" as s
join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false 
left join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId"" and ns.""IsDeleted""=false 
left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false 
left join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
  #WHERE# and s.""IsDeleted""=false  ";
            //var cypher2 = @"MATCH (s:NTS_Task{IsDeleted:0})-[:R_Task_Template]-(t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]-(tr:NTS_TemplateMaster{IsDeleted:0})
            //                #WHERE#
            //                optional match(s)-[:R_Task_Owner_User]-(u:ADM_User{IsDeleted:0})
            //                optional match(s)-[:R_Task_AssignedTo_User]-(ua:ADM_User{IsDeleted:0})
            //                optional match(s)-[:R_Task_Status_ListOfValue]-(ns:GEN_ListOfValue{IsDeleted:0})
            //                return ns.Code as TaskStatusCode,u.Id as OwnerUserId,ua.Id as AssignedTo";
            //  var prms2 = new Dictionary<string, object>();
            var search2 = "";
            if (moduleId.IsNotNullAndNotEmpty())
            {
                search2 = $@" where t.ModuleId='{moduleId}'";
            }
            cypher2 = cypher2.Replace("#WHERE#", search2);
            //prms2.Add("Status", StatusEnum.Active.ToString());
            //prms2.Add("CompanyId", CompanyId);
            //prms2.Add("UserId", userId);
            var tasklist = await _queryTaskRepo.ExecuteQueryList<TaskViewModel>(cypher2, null);

            var task = tasklist.Where(x => x.AssignedToUserId == userId);
            var assToMeOverdue = task.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count();
            var assToMePending = task.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count();
            var cyphernote = $@"select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId--,s.ReferenceType as ReferenceType
from public.""NtsNote"" as s
join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false
left join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId"" and ns.""IsDeleted""=false 
left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false 
where u.""Id""='{userId}' and s.""IsDeleted""=false";
            //var cyphernote = @"MATCH (u:ADM_User{Id:{UserId}})-[:R_Note_Owner_User]-(s:NTS_Note)-[:R_Note_Template]-(t:NTS_Template)-[:R_TemplateRoot]-(tr:NTS_TemplateMaster)
            //                optional match(s)-[:R_Note_Status_ListOfValue]-(ns:GEN_ListOfValue)
            //                optional match(s)-[:R_Note_Reference]-(r)
            //                return ns.Code as NoteStatusCode,u.Id as OwnerUserId,s.ReferenceType as ReferenceType";
            //var noteprms = new Dictionary<string, object>();
            //noteprms.Add("Status", StatusEnum.Active.ToString().ToString());
            //noteprms.Add("CompanyId", CompanyId);
            //noteprms.Add("UserId", userId);

            var note = await _queryNoteRepo.ExecuteQueryList<NoteViewModel>(cyphernote, null);

            var _createdByMeActive = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" /*&& (x.ReferenceType == NoteReferenceTypeEnum.Self)*/).Count();

            var count = new NoteViewModel()
            {
                S_RequestPending = Convert.ToInt64(reqByMePending),
                S_RequestOverdue = Convert.ToInt64(reqByMeOverdue),
                T_AssignPending = Convert.ToInt64(assToMePending),
                T_AssignOverdue = Convert.ToInt64(assToMeOverdue),
                N_Active = Convert.ToInt64(_createdByMeActive),
                base64Img = photoB64
            };

            return count;
        }
        public async Task<IList<ServiceViewModel>> GetServiceByUser(string userId)
        {
            //var prms = new Dictionary<string, object>
            //{
            //    { "userId",userId },
            //};

            //var taskQry = string.Concat(@"match (u:ADM_User{Id:{userId},IsDeleted:0})<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
            //match(s)-[R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue) where (lv.Code='OVER_DUE' or lv.Code='IN_PROGRESS'
            //or lv.Code='NOT_STARTED')
            //return s,lv.Name as ServiceStatusName,"
            //, Helper.UserNameWithEmail("u", " as OwnerDisplayName"));
            var taskQry = string.Concat($@"select  s.*,lv.""Name"" as ServiceStatusName,u.""Name"" as OwnerDisplayName
from public.""User"" as u 
join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
and (lv.""Code""='SERVICE_STATUS_OVERDUE' or lv.""Code""='SERVICE_STATUS_INPROGRESS'
            or lv.""Code""='SERVICE_STATUS_NOTSTARTED') where u.""Id""='{userId}' and u.""IsDeleted""=false"
    );

            var service = await _queryRepo.ExecuteQueryList<ServiceViewModel>(taskQry, null);
            return service.ToList();
        }
        public async Task ReAssignTerminatedEmployeeServices(string userId, List<string> serviceList)
        {
            foreach (var id in serviceList)
            {
                var newViewModel = await GetServiceDetails(new ServiceTemplateViewModel
                {
                    ServiceId = id,
                    ActiveUserId = _repo.UserContext.UserId,
                    DataAction = DataActionEnum.Edit/*, IsVersioning = true*/
                });
                newViewModel.OwnerUserId = userId;
                newViewModel.AllowPastStartDate = true;
                // newViewModel.TemplateAction = NtsActionEnum.EditAsNewVersion;
                await ManageService(newViewModel);
            }

        }

        public async Task<List<NtsLogViewModel>> GetVersionDetails(string serviceId)
        {

            var query = $@"select l.""ServiceSubject"" as Subject, l.* from log.""NtsServiceLog""  as l
            join public.""NtsService"" as n on l.""RecordId""=n.""Id"" and n.""IsDeleted""=false and l.""VersionNo""<>n.""VersionNo""
            where l.""IsDeleted""=false and l.""RecordId"" = '{serviceId}' and l.""IsVersionLatest""=true order by l.""VersionNo"" desc";
            var result = await _queryNtsLog.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<DashboardCalendarViewModel>> GetWorkPerformanceCount(string userId, string moduleCodes = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            //            var query = $@"select s.""Id"", ss.""Code"" as ServiceStatusCode,s.""StartDate""::TIMESTAMP::DATE,s.""DueDate""::TIMESTAMP::DATE,s.""ReminderDate""::TIMESTAMP::DATE,
            //s.""ClosedDate""::TIMESTAMP::DATE,nf.""CreatedDate""::TIMESTAMP::DATE
            //from public.""NtsService"" as s
            //join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false
            //join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
            //join public.""Module"" as m on tc.""ModuleId""=m.""Id"" and m.""IsDeleted""=false 
            //join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id""
            //left join public.""Notification"" as nf on s.""Id""=nf.""ReferenceTypeId""
            //where s.""IsDeleted""=false #WHERE# ";

            var query = $@"select n.""DueDate""::TIMESTAMP::DATE as Start, n.""DueDate""::TIMESTAMP::DATE as End,count(n.""Id""), 'OverDue' as StatusName
from public.""NtsService"" as n
  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false
  join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false 
  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false
  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='SERVICE_STATUS_OVERDUE'
  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false   
  #WHERE#
  where n.""IsDeleted""=false  and n.""PortalId""='{_userContext.PortalId}' group by n.""DueDate""::TIMESTAMP::DATE
union
select n.""StartDate""::TIMESTAMP::DATE as Start,n.""StartDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'Inprogress' as StatusName
from public.""NtsService"" as n
  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='SERVICE_STATUS_INPROGRESS'
  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false   
  #WHERE# 
  where n.""IsDeleted""=false and n.""PortalId""='{_userContext.PortalId}' group by n.""StartDate""::TIMESTAMP::DATE
union
select n.""ClosedDate""::TIMESTAMP::DATE as Start,n.""ClosedDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'Closed' as StatusName
 from public.""NtsService"" as n
  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false  
join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false and ns.""Code""='SERVICE_STATUS_CLOSE'
  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false   
  #WHERE# 
  where n.""IsDeleted""=false  and n.""PortalId""='{_userContext.PortalId}' group by n.""ClosedDate""::TIMESTAMP::DATE
union
select n.""StartDate""::TIMESTAMP::DATE as Start,n.""StartDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'NotStarted' as StatusName
 from public.""NtsService"" as n
  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false  
join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false and ns.""Code""='SERVICE_STATUS_NOTSTARTED'
  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false    
  #WHERE# 
  where n.""IsDeleted""=false and n.""PortalId""='{_userContext.PortalId}' group by n.""StartDate""::TIMESTAMP::DATE
union
select n.""ReminderDate""::TIMESTAMP::DATE as Start, n.""ReminderDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'Reminder' as StatusName
 from public.""NtsService"" as n
  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false
join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false  
join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false  
#WHERE# 
where n.""IsDeleted""=false and n.""PortalId""='{_userContext.PortalId}' and n.""ReminderDate"" is not null group by n.""ReminderDate""::TIMESTAMP::DATE 
union
select nf.""CreatedDate""::TIMESTAMP::DATE as Start,nf.""CreatedDate""::TIMESTAMP::DATE as End, count(nf.""Id""), 'Notification' as StatusName
from public.""NtsService"" as n
join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false
join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
#WHERE#  
join public.""Notification"" as nf on n.""Id""=nf.""ReferenceTypeId""
where nf.""IsDeleted"" = false and nf.""ToUserId""='{userId}' and nf.""PortalId""='{_userContext.PortalId}' group by nf.""CreatedDate""::TIMESTAMP::DATE  ";

            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            query = query.Replace("#WHERE#", search);

            var result = await _queryCal.ExecuteQueryList(query, null);

            foreach (var item in result)
            {
                if (item.StatusName == "OverDue")
                {
                    item.Start = item.Start.AddDays(1);
                    item.End = item.End.AddDays(1);
                }
            }

            var dates = result.Select(x => new DashboardCalendarViewModel() { Start = x.Start }).GroupBy(x => x.Start).ToList();

            List<DashboardCalendarViewModel> newresult = new List<DashboardCalendarViewModel>();
            foreach (var d in dates)
            {
                var newmodel = new DashboardCalendarViewModel();
                foreach (var i in result)
                {
                    newmodel.Start = d.Key;
                    newmodel.End = d.Key;
                    newmodel.IsAllDay = true;

                    if (d.Key == i.Start)
                    {
                        if (i.StatusName == "OverDue")
                        {
                            newmodel.BacklogCount = i.Count;
                        }
                        if (i.StatusName == "Inprogress")
                        {
                            newmodel.OpenCount = i.Count;
                        }
                        if (i.StatusName == "Closed")
                        {
                            newmodel.ClosedCount = i.Count;
                        }
                        if (i.StatusName == "Notification")
                        {
                            newmodel.NotificationCount = i.Count;
                        }
                        if (i.StatusName == "Reminder")
                        {
                            newmodel.ReminderCount = i.Count;
                        }
                        if (i.StatusName == "NotStarted")
                        {
                            newmodel.NotStartedCount = i.Count;
                        }

                    }
                }
                newresult.Add(newmodel);
            }

            return newresult;

        }


        public async Task<List<IdNameViewModel>> GetCMSExternalRequest()
        {
            var Templatesin = "";
            Int32 vl = 0;
            var _templateCategoryBusiness = _serviceProvider.GetService<ITemplateCategoryBusiness>();
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "CMS_SEBI" && x.TemplateType == TemplateTypeEnum.Service);
            if (tempCategory.IsNotNull())
            {
                var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);
                foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
                {
                    vl++;
                    if (vl == 1)
                    {
                        Templatesin += $@"'{item.Id}'";
                    }
                    else
                    {
                        Templatesin += $@",'{item.Id}'";
                    }
                }
            }





            var query = $@"select sr.""Id"" as Id ,sr.""ServiceSubject"" as Name 
from Public.""NtsService"" as sr inner join ""NtsNote"" as n on n.""Id""=sr.""UdfNoteId""  and n.""IsDeleted""=false
where sr.""TemplateId"" in ({Templatesin})  and sr.""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;


        }

        public async Task<List<ServiceViewModel>> GetServiceList(string portalId, string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string statusCodes = null, string parentServiceId = null)
        {

            var list = new List<ServiceViewModel>();
            string query = @$"Select s.""Id"",s.""ServiceNo"",s.""ServiceSubject"",s.""TemplateCode"",o.""Name"" as OwnerDisplayName,ts.""Name"" as ServiceStatusName,ts.""Code"" as ServiceStatusCode,
                            s.""StartDate"",s.""DueDate"",s.""RequestedByUserId"", tem.""DisplayName"" as TemplateDisplayName,
                            s.""CreatedDate"",s.""CompletedDate"" as CompletedDate, s.""RejectedDate"" as RejectedDate, s.""CanceledDate"" as CanceledDate, s.""ClosedDate"" as ClosedDate
                            From public.""NtsService"" as s
                            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  #TemCodeWhere#
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  #ModCodeWhere#
                            Join public.""User"" as o on s.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on s.""ServiceStatusId""=ts.""Id"" and ts.""IsDeleted""=false #StatusWhere#
                            where s.""PortalId""='{portalId}' and s.""IsDeleted""=false #OWNERWHERE# #ParentWhere# order by s.""CreatedDate"" desc";

            string ownerWhere = "";
            if (!showAllOwnersService || (requestby.IsNotNullAndNotEmpty() && requestby == "RequestedByMe"))
            {
                ownerWhere = $@" and (s.""RequestedByUserId""='{_repo.UserContext.UserId}' or s.""OwnerUserId""='{_repo.UserContext.UserId}') ";
            }
            query = query.Replace("#OWNERWHERE#", ownerWhere);

            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and tem.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            string statusCodeWhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                var codes = statusCodes.Trim(',').Replace(",", "','");
                statusCodeWhere = $@" and ts.""Code"" in ('{codes}')";
            }
            query = query.Replace("#StatusWhere#", statusCodeWhere);

            string parentWhere = "";
            if (parentServiceId.IsNotNullAndNotEmpty())
            {
                parentWhere = $@" and s.""ParentServiceId""='{parentServiceId}' ";
            }
            query = query.Replace("#ParentWhere#", parentWhere);

            var result = await _queryRepo.ExecuteQueryList(query, null);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            }
            return result;
        }

        public async Task<List<ServiceViewModel>> GetInternalServiceListFromExternalRequestId(string serviceId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "CMS_SEBI_INT", null, null, "externalServiceRequest,externalServiceRequest", "NtsNoteId,NtsNoteId");
            var query = $@"
select is.*,lov.""Code"" as ServiceStatusCode from public.""NtsService"" as s 
join (" + udfs + $@") as udf on udf.""externalServiceRequest""=s.""Id""
join public.""NtsService"" as is on is.""UdfNoteId""=udf.""NtsNoteId""
join public.""LOV"" as lov on lov.""Id""=is.""ServiceStatusId""
where s.""IsDeleted""=false and s.""Id""='{serviceId}' and is.""IsDeleted""=false
";
            var data = await _queryRepo.ExecuteQueryList(query, null);
            return data;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetExternalServiceChartByStatus()
        {
            var query = $@"
            select count(s) as ""Value"",case when tl.""Code"" is null then lov.""Name"" else task.""TaskSubject"" end as ""Type""
            --,s.""ServiceStatusId"" as ""Id"" 
          from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id""
            left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
            group by ""Type""--,s.""ServiceStatusId""
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetExternalUserServiceChartByStatus()
        {
            var query = $@"
            select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"" 
          from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            --left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""IsDeleted""=false
           -- left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false    and s.""OwnerUserId""='{_userContext.UserId}' 
            group by s.""ServiceStatusId"",""Type""
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetInternalServiceChartByStatus()
        {
            var query = $@"
            select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"" from public.""NtsService"" as s 
            
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_INT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false 
            group by ""Type"",s.""ServiceStatusId""
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }


        public async Task<List<ProjectDashboardChartViewModel>> GetExternalUserInternalServiceChartByStatus()
        {
            var query = $@"
                   select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"" 
          from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            --left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""IsDeleted""=false
           -- left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false    and s.""OwnerUserId""='{_userContext.UserId}'  and lov.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_CLOSE')
            group by s.""ServiceStatusId"",""Type""         
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }


        public async Task<List<ProjectDashboardChartViewModel>> GetInternalDashboardChartByStatus(ServiceSearchViewModel search)
        {
            var query = $@"
            select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"" from public.""NtsService"" as s 
            
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false and lov.""Code""<>'SERVICE_STATUS_DRAFT' #CATEGORY# #TEMPLATE# 
            group by ""Type"",s.""ServiceStatusId""
            ";

            var category = "";
            var template = "";
            if (search.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = search.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (search.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = search.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tem.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetInternalDashboardTaskChart(ServiceSearchViewModel search)
        {
            var query = $@"
            select count(t) as ""Value"",lovt.""Name"" as ""Type"",t.""TaskStatusId"" as ""Id"" from public.""NtsService"" as s 
            
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           Join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false  and t.""TaskType""=2
             join public.""LOV"" as lovt on lovt.""Id""=t.""TaskStatusId""
            where s.""IsDeleted""=false and lovt.""Code""<>'TASK_STATUS_DRAFT' #CATEGORY# #TEMPLATE# 
            group by ""Type"",t.""TaskStatusId""
            ";

            var category = "";
            var template = "";
            if (search.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = search.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (search.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = search.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tem.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetExternalServiceSLA(ServiceSearchViewModel searchModel)
        {

            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI'
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and n.""IsDeleted""=false  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                            group by n.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryChart.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();

            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetRequestSLA(ServiceSearchViewModel searchModel)
        {

            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false --and tc.""Code""='CMS_SEBI'
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and n.""IsDeleted""=false  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                           #CATEGORY# #TEMPLATE#  group by n.""DueDate""::TIMESTAMP::DATE ";

            var category = "";
            var template = "";
            if (searchModel.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = searchModel.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (searchModel.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = searchModel.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tr.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);

            var queryData = await _queryChart.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();

            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetExternalUserExternalServiceSLA(ServiceSearchViewModel searchModel)
        {

            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI'
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and n.""IsDeleted""=false and n.""OwnerUserId""='{_userContext.UserId}'  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                            group by n.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryChart.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();

            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<List<ServiceViewModel>> GetSEBIServiceList()
        {
            var query = $@"
            select s.*,case when tl.""Code"" is null then lov.""Name"" else task.""TaskSubject"" end as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id""
            left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
            union
            select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_INT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false  order by ""CreatedDate"" desc
            ";
            var data = await _queryRepo.ExecuteQueryList(query, null);
            return data;
        }

        public async Task<List<ServiceViewModel>> GetSEBIExternalServiceList(string tasktemplatecode)
        {
            var query = $@"
            select s.*,tl.""Name"" as ""ServiceStatusName"",tl.""Code"" as ""ServiceStatusCode"",nou.""Name"" as ""OwnerDisplayName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""TemplateCode""='{tasktemplatecode}'
            join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and s.""OwnerUserId""='{_userContext.UserId}'
           
            ";
            var data = await _queryRepo.ExecuteQueryList(query, null);
            return data;
        }

        public async Task<List<ServiceViewModel>> GetExternalSEBIServiceList()
        {
            var query = $@"
            select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" 
                   from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            --left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""IsDeleted""=false
            --left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and s.""OwnerUserId""='{_userContext.UserId}'  --and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
            union
            select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_INT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false and s.""OwnerUserId""='{_userContext.UserId}'
            order by ""CreatedDate"" desc";
            var data = await _queryRepo.ExecuteQueryList(query, null);
            return data;
        }
        public async Task<List<ServiceViewModel>> GetInternalDashboardServiceList(ServiceSearchViewModel searchModel)
        {
            var query = $@"
            select s.*,case when tl.""Code"" is null then lov.""Name"" else task.""TaskSubject"" end as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id""
            left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')  #TEMPLATE# 
            union
            select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false --and tc.""Code""='CMS_SEBI_INT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false and lov.""Code""<>'SERVICE_STATUS_DRAFT'  #CATEGORY# #TEMPLATE# 
            ";

            var category = "";
            var template = "";
            if (searchModel.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = searchModel.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (searchModel.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = searchModel.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tem.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);
            var data = await _queryRepo.ExecuteQueryList(query, null);
            return data;
        }


        public async Task<List<NtsLogViewModel>> GetServiceLog(string ServiceId, string TemplateCode)
        {

            var udfs = await GetUdfQuery(null, null, TemplateCode, null, "CreatedDate,Date");
            var Query = $@"Select L.""CreatedDate"" as LogStartDate from Log.""NtsServiceLog"" as L inner join public.""NtsService"" as s on L.""RecordId""=s.""Id"" where ""RecordId""='7de93086-958d-44fa-8674-5e161fe8ebbb'";
            var result = await _queryNtsLog.ExecuteQueryList(Query, null);
            return result;
        }

        public async Task<DataTable> GetDynamicService(string TemplateCode, string ServiceId, string TemplateType)
        {
            var udfs = await GetUdfQuery(TemplateType, null, TemplateCode, null, "CreatedDate,Date");
            var where = $@" where L.""RecordId""='{ServiceId}' ";
            udfs = udfs + where;
            var data = await _queryRepo.ExecuteQueryDataTable(udfs, null);


            return data;
        }


        public async Task<List<string>> GetDynamicServiceColumnLst(string TemplateCode, string TemplateType)
        {
            var udfs = await GetUdfColumnList(TemplateType, null, TemplateCode, null, "CreatedDate,Date");

            //var data = await _queryRepo.ExecuteQueryDataTable(udfs, null);
            return udfs;
        }


        public async Task<string> GetUdfQuery(string TemplateType, string categoryCode, string templateCodes, string excludeTemplateCodes, params string[] columns)
        {

            string Metadeta = "", MianTable = "", Eventid = "", LefCol = "";

            switch (TemplateType)
            {
                case "Service":
                    Metadeta = "UdfTableMetadataId";
                    MianTable = "NtsServiceLog";
                    Eventid = "ServiceEventId";
                    LefCol = "UdfNoteId";
                    break;
                case "Note":
                    Metadeta = "TableMetadataId";
                    MianTable = "NtsNoteLog";
                    Eventid = "NoteEventId";
                    LefCol = "RecordId";
                    break;
                case "Task":
                    Metadeta = "UdfTableMetadataId";
                    MianTable = "NtsTaskLog";
                    Eventid = "TaskEventId";
                    LefCol = "UdfNoteId";
                    break;
            }

            var query = @$"select cm.*,tm.""Name"" as ""TableName"",tm.""Schema"" as ""TableSchemaName"",
            t.""Id"" as ""TemplateId"" from public.""Template"" t
            join public.""TemplateCategory"" tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
            join public.""TableMetadata"" tm on t.""{Metadeta}""=tm.""Id"" and tm.""IsDeleted""=false 
            join public.""ColumnMetadata"" cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsDeleted""=false  and cm.""IsUdfColumn""=true
            where t.""IsDeleted""=false  ";


            if (templateCodes.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and t.""Code""='{templateCodes}'";
            }
            var columnList = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            var q = "";
            var queryList = new List<string>();

            if (columnList.Count > 0)
            {
                q = @$"Select Distinct L.""CreatedDate"" as ""LogStartDate"",L.""LogStartDateTime"",u.""Name"" as ""OwnerName"",lov.""Name"" as ""EventName"", ";
                var tables = columnList.GroupBy(x => x.TableName).Select(x => x.FirstOrDefault()).ToList();
                foreach (var table in columnList)
                {
                    if (table.Name != null)
                    {
                        q = @$"{q} udf.""{table.Name}"" as ""{table.Name}"",";
                    }
                    else
                    {
                        q = @$"{q} null as ""{null}"",";
                    }
                }
                var qery = @$"from Log.""{MianTable}"" as L left join log.""{columnList[0].TableName + "Log"}"" as udf on L.""{LefCol}"" = udf.""NtsNoteId""   and L.""VersionNo"" =udf.""VersionNo""
         left join public.""User"" as u on L.""CreatedBy""=u.""Id""
         left join public.""LOV"" as lov on lov.""Id""=L.""{Eventid}""";
                queryList.Add(@$"{q.Trim(',')} {qery}");
            }
            else
            {
                q = @$"Select Distinct L.""CreatedDate"" as ""LogStartDate"",L.""LogStartDateTime"",u.""Name"" as ""OwnerName"",lov.""Name"" as ""EventName""";
                var qery = @$"from Log.""{MianTable}"" as L 
         left join public.""User"" as u on L.""CreatedBy""=u.""Id""
         left join public.""LOV"" as lov on lov.""Id""=L.""{Eventid}""";
                queryList.Add(@$"{q.Trim(',')} {qery}");

            }

            if (queryList.Any())
            {
                return string.Join($"{Environment.NewLine} union {Environment.NewLine}", queryList);
            }
            return "";
        }


        public async Task<List<string>> GetUdfColumnList(string TemplateType, string categoryCode, string templateCodes, string excludeTemplateCodes, params string[] columns)
        {

            string Metadeta = "", MianTable = "";

            switch (TemplateType)
            {
                case "Service":
                    Metadeta = "UdfTableMetadataId";

                    break;
                case "Note":
                    Metadeta = "TableMetadataId";
                    break;
                case "Task":
                    Metadeta = "UdfTableMetadataId";
                    break;
            }
            var query = @$"select cm.*,tm.""Name"" as ""TableName"",tm.""Schema"" as ""TableSchemaName"",
            t.""Id"" as ""TemplateId"" from public.""Template"" t
            join public.""TemplateCategory"" tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
            join public.""TableMetadata"" tm on t.""{Metadeta}""=tm.""Id"" and tm.""IsDeleted""=false 
            join public.""ColumnMetadata"" cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsDeleted""=false  and cm.""IsUdfColumn""=true
            where t.""IsDeleted""=false  ";


            if (templateCodes.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and t.""Code""='{templateCodes}'";
            }
            var columnList = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);

            var tables = columnList.GroupBy(x => x.TableName).Select(x => x.FirstOrDefault()).ToList();
            var q = "";
            q = @$"select ";
            var queryList = new List<string>();

            foreach (var table in columnList)
            {
                if (table.Name != null)
                {
                    //q = @$"{q} ""{table.Name}"" as ""{table.LabelName}"",";

                    queryList.Add(table.Name);
                }

            }

            return queryList;
        }


        public async Task<List<IdNameViewModel>> GetServiceUserList(string serviceId)
        {
            string query = @$"select u.""Id"",u.""Name""
from public.""NtsService"" as s
join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false
where s.""Id""='{serviceId}' and s.""IsDeleted""=false
union
select u.""Id"",u.""Name""
from public.""NtsService"" as s
join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
where s.""Id""='{serviceId}' and s.""IsDeleted""=false
union

select u.""Id"",u.""Name""
from public.""NtsService"" as s
join public.""NtsTask"" as t on t.""ParentServiceId""=s.""Id"" and t.""IsDeleted""=false
join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false
where s.""Id""='{serviceId}' and s.""IsDeleted""=false

union

select u.""Id"",u.""Name""
from public.""NtsService"" as s
join public.""NtsServiceShared"" as t on t.""NtsServiceId""=s.""Id"" and t.""IsDeleted""=false
join public.""User"" as u on u.""Id""=t.""SharedWithUserId"" and u.""IsDeleted""=false
where s.""Id""='{serviceId}' and s.""IsDeleted""=false
union

select u.""Id"",u.""Name""
from public.""NtsService"" as s
join public.""NtsServiceShared"" as t on t.""NtsServiceId""=s.""Id"" and t.""IsDeleted""=false
join public.""Team"" as tm on tm.""Id""=t.""SharedWithTeamId"" and tm.""IsDeleted""=false
join public.""TeamUser"" as tu on tu.""TeamId""=tm.""Id"" and tu.""IsDeleted""=false
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
where s.""Id""='{serviceId}' and s.""IsDeleted""=false

 ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list.Distinct().ToList();

        }

        public async Task<IList<TreeViewViewModel>> GetDocumentTreeviewList(string id, string type, string parentId, string serviceId, string expandingList)
        {
            var expObj = new List<TreeViewViewModel>();

            var list = new List<TreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                query = @$"Select s.""Id"" as ""Id"",s.""ServiceSubject"" as Name,'Service' as Type,true as hasChildren from public.""NtsService"" as s                        
                        where s.""Id""='{serviceId}'
                        union
                        Select s.""Id"" as ""Id"",s.""TaskSubject"" as Name,'Task' as Type,true as hasChildren from public.""NtsTask"" as s                        
                        where s.""ParentServiceId""='{serviceId}'
                        union
                        Select s.""Id"" as ""Id"",s.""NoteSubject"" as Name,'Note' as Type,true as hasChildren from public.""NtsNote""  as s                        
                        where s.""ParentServiceId""='{serviceId}'
                        ";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                foreach (var l in list)
                {
                    l.children = true;
                    l.text = "Header";
                    l.parent = "#";
                    l.a_attr = new { data_id = l.id, data_name = l.Name, data_type = l.Type };
                }

            }
            else if (type == "Service")
            {
                var item = new TreeViewViewModel
                {
                    id = "SHEADER",
                    Name = "Header",
                    ParentId = id,
                    hasChildren = true,
                    Type = "Header",
                    children = true,
                    text = "Header",
                    parent = id,
                    a_attr = new { data_id = "SHEADER", data_name = "Header", data_type = "Header" },
                };

                list.Add(item);

                var item1 = new TreeViewViewModel
                {
                    id = "SDETAIL",
                    Name = "Detail",
                    ParentId = id,
                    hasChildren = true,
                    Type = "SDETAIL",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "SDETAIL", data_name = "Detail", data_type = "SDETAIL" },
                };

                list.Add(item1);
                var item3 = new TreeViewViewModel
                {
                    id = "SNOTIFICATION",
                    Name = "Notification",
                    ParentId = id,
                    hasChildren = true,
                    Type = "SNOTIFICATION",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "SNOTIFICATION", data_name = "Notification", data_type = "SNOTIFICATION" },
                };

                list.Add(item3);
                var item4 = new TreeViewViewModel
                {
                    id = "SCOMMENT",
                    Name = "Comment",
                    ParentId = id,
                    hasChildren = true,
                    Type = "SCOMMENT",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "SCOMMENT", data_name = "Comment", data_type = "SCOMMENT" },
                };

                list.Add(item4);

            }
            else if (type == "SHEADER")
            {
                query = @$"Select s.*,lv.""Name"" as ServiceStatusName from public.""NtsService"" as s 
                        join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId""
                        where s.""Id""='{parentId}'
                        ";

                var service = await _queryRepo.ExecuteQuerySingle(query, null);
                if (service != null)
                {
                    var item = new TreeViewViewModel
                    {
                        id = service.ServiceSubject,
                        Name = service.ServiceSubject,
                        Type = "BODY",
                        ParentId = "SHEADER",
                        children = true,
                        text = "Detail",
                        parent = id,
                        a_attr = new { data_id = service.ServiceSubject, data_name = service.ServiceSubject, data_type = "BODY" },
                    };
                    list.Add(item);

                    var item1 = new TreeViewViewModel
                    {
                        id = service.StartDate.ToString(),
                        Name = service.StartDate.ToString(),
                        Type = "BODY",
                        ParentId = "SHEADER",
                        children = true,
                        text = service.StartDate.ToString(),
                        parent = "SHEADER",
                        a_attr = new { data_id = service.StartDate.ToString(), data_name = service.StartDate.ToString(), data_type = "BODY" },
                    };
                    list.Add(item1);
                    var item2 = new TreeViewViewModel
                    {
                        id = service.ServiceStatusName,
                        Name = service.ServiceStatusName,
                        Type = "BODY",
                        ParentId = "SHEADER",
                        children = true,
                        text = service.ServiceStatusName,
                        parent = "SHEADER",
                        a_attr = new { data_id = service.ServiceStatusName.ToString(), data_name = service.ServiceStatusName.ToString(), data_type = "BODY" },
                    };
                    list.Add(item2);
                }

            }
            else if (type == "SDETAIL")
            {
                var service = await GetServiceDetails(new ServiceTemplateViewModel { ServiceId = parentId });
                foreach (var column in service.ColumnList)
                {
                    var item = new TreeViewViewModel
                    {
                        id = column.Name,
                        Name = column.Name + ":" + column.Value,
                        Type = "BODY",
                        ParentId = "SDETAIL",
                        children = true,
                        text = column.Name + ":" + column.Value,
                        parent = "SDETAIL",
                        a_attr = new { data_id = column.Name, data_name = column.Name + ":" + column.Value, data_type = "BODY" },
                    };
                    list.Add(item);
                }
            }
            else if (type == "SCOMMENT")
            {
                var _notificationBusiness = _serviceProvider.GetService<IPushNotificationBusiness>();
                var notification = await _notificationBusiness.GetServiceNotificationList(parentId, _userContext.UserId, 0);
                foreach (var column in notification)
                {
                    var item = new TreeViewViewModel
                    {
                        id = column.Subject,
                        Name = column.Subject,
                        Type = "BODY",
                        ParentId = "SCOMMENT",
                        children = true,
                        text = column.Subject,
                        parent = "SCOMMENT",
                        a_attr = new { data_id = column.Subject, data_name = column.Subject, data_type = "BODY" },
                    };
                    list.Add(item);
                }
            }
            else if (type == "SNOTIFICATION")
            {
                var _ntsServiceCommentBusiness = _serviceProvider.GetService<INtsServiceCommentBusiness>();
                var notification = await _ntsServiceCommentBusiness.GetSearchResult(parentId);
                foreach (var column in notification)
                {
                    var item = new TreeViewViewModel
                    {
                        id = column.Id,
                        Name = column.Comment,
                        Type = "BODY",
                        ParentId = "SNOTIFICATION",
                        children = true,
                        text = column.Comment,
                        parent = "SNOTIFICATION",
                        a_attr = new { data_id = column.Id, data_name = column.Comment, data_type = "BODY" },
                    };
                    list.Add(item);
                }
            }
            else if (type == "Task")
            {
                var item = new TreeViewViewModel
                {
                    id = "THEADER",
                    Name = "Header",
                    ParentId = id,
                    hasChildren = true,
                    Type = "THEADER",
                    children = true,
                    text = "Header",
                    parent = id,
                    a_attr = new { data_id = "THEADER", data_name = "Header", data_type = "THEADER" }
                };

                list.Add(item);

                var item1 = new TreeViewViewModel
                {
                    id = "TDETAIL",
                    Name = "Detail",
                    ParentId = id,
                    hasChildren = true,
                    Type = "TDETAIL",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "TDETAIL", data_name = "Detail", data_type = "TDETAIL" }
                };

                list.Add(item1);

                var item3 = new TreeViewViewModel
                {
                    id = "TCOMMENT",
                    Name = "Comment",
                    ParentId = id,
                    hasChildren = true,
                    Type = "TCOMMENT",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "TCOMMENT", data_name = "Comment", data_type = "TCOMMENT" }
                };

                list.Add(item3);

            }
            else if (type == "Note")
            {
                var item = new TreeViewViewModel
                {
                    id = "NHEADER",
                    Name = "Header",
                    ParentId = id,
                    hasChildren = true,
                    Type = "NHEADER",
                    children = true,
                    text = "Header",
                    parent = id,
                    a_attr = new { data_id = "NHEADER", data_name = "Header", data_type = "NHEADER" }
                };

                list.Add(item);

                var item1 = new TreeViewViewModel
                {
                    id = "NDETAIL",
                    Name = "Detail",
                    ParentId = id,
                    hasChildren = true,
                    Type = "NDETAIL",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "NDETAIL", data_name = "Detail", data_type = "NDETAIL" }
                };

                list.Add(item1);

                var item3 = new TreeViewViewModel
                {
                    id = "NCOMMENT",
                    Name = "Comment",
                    ParentId = id,
                    hasChildren = true,
                    Type = "NCOMMENT",
                    children = true,
                    text = "Comment",
                    parent = id,
                    a_attr = new { data_id = "NCOMMENT", data_name = "Comment", data_type = "NCOMMENT" }
                };

                list.Add(item3);

            }


            return list;
        }


        public async Task<IList<TreeViewViewModel>> GetDocumentIndexTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userRoleCodes, string docServiceId)
        {
            var list = new List<TreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var item = new TreeViewViewModel
                {
                    id = "SERVICE",
                    Name = "Service",
                    DisplayName = "Service",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "SERVICE",
                    children = true,
                    text = "Service",
                    parent = "#",
                    a_attr = new { data_id = "SERVICE", data_name = "Service", data_type = "SERVICE" }
                };
                list.Add(item);
            }
            else if (id == "SERVICE")
            {
                query = @$"Select distinct s.""Id"" as id,s.""ServiceSubject"" as Name, 'SERVICE' as ParentId, 'SERVICE' as Type,
                        true as hasChildren
                        FROM public.""NtsService"" as s
                        Where s.""IsDeleted""=false and s.""Id""='{docServiceId}'
                        union
                        Select distinct s.""Id"" as id,s.""ServiceSubject"" as Name, '{docServiceId}' as ParentId, 'SERVICE' as Type,
                        true as hasChildren
                        FROM public.""NtsService"" as s
                        Where s.""IsDeleted""=false and s.""ParentServiceId""='{docServiceId}'
                        union
                        select distinct t.""Id"" as id,t.""TaskSubject"" as Name, '{docServiceId}' as ParentId, 'TASK' as Type,
                        true as hasChildren
                        FROM public.""NtsTask"" as t
                        where t.""IsDeleted""=false and t.""ParentServiceId""='{docServiceId}'
                        union
                        select distinct n.""Id"" as id,n.""NoteSubject"" as Name, '{docServiceId}' as ParentId, 'NOTE' as Type,
                        false as hasChildren
                        FROM public.""NtsNote"" as n
                        where n.""IsDeleted""=false and n.""ParentServiceId""='{docServiceId}'
                        ";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                foreach (var s in list)
                {
                    s.children = true;
                    s.text = "Service";
                    s.parent = s.ParentId;
                    s.a_attr = new { data_id = s.id, data_name = s.Name, data_type = s.Type };
                }
            }
            else if (type == "SERVICE")
            {
                //query = $@"Select distinct s.""Id"" as id,s.""ServiceSubject"" as Name, '{docServiceId}' as ParentId, 'SUBSERVICE' as Type,
                //true as hasChildren
                //        FROM public.""NtsService"" as s
                //        Where s.""IsDeleted""=false and s.""ParentServiceId""='{docServiceId}'";
                //list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                var item = new TreeViewViewModel
                {
                    id = "SHEADER",
                    Name = "Header",
                    ParentId = id,
                    hasChildren = false,
                    Type = "SHEADER",
                    children = true,
                    text = "Header",
                    parent = id,
                    a_attr = new { data_id = "SHEADER", data_name = "Header", data_type = "SHEADER" }
                };
                list.Add(item);

                var item1 = new TreeViewViewModel
                {
                    id = "SDETAIL",
                    Name = "Detail",
                    ParentId = id,
                    hasChildren = false,
                    Type = "SDETAIL",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "SDETAIL", data_name = "Detail", data_type = "SDETAIL" }
                };
                list.Add(item1);

                var item3 = new TreeViewViewModel
                {
                    id = "SNOTIFICATION",
                    Name = "Notification",
                    ParentId = id,
                    hasChildren = false,
                    Type = "SNOTIFICATION",
                    children = true,
                    text = "Notification",
                    parent = id,
                    a_attr = new { data_id = "SNOTIFICATION", data_name = "Notification", data_type = "SNOTIFICATION" }
                };
                list.Add(item3);

                var item4 = new TreeViewViewModel
                {
                    id = "SCOMMENT",
                    Name = "Comment",
                    ParentId = id,
                    hasChildren = false,
                    Type = "SCOMMENT",
                    children = true,
                    text = "Comment",
                    parent = id,
                    a_attr = new { data_id = "SCOMMENT", data_name = "Comment", data_type = "SCOMMENT" }
                };
                list.Add(item4);
            }
            //else if (type == "SHEADER")
            //{
            //    query = @$"Select s.*,lv.""Name"" as ServiceStatusName from public.""NtsService"" as s 
            //            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId""
            //            where s.""Id""='{parentId}'
            //            ";

            //    var service = await _queryRepo.ExecuteQuerySingle(query, null);
            //    if (service != null)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = service.ServiceSubject,
            //            Name = service.ServiceSubject,
            //            Type = "BODY",
            //            ParentId = "SHEADER",
            //        };
            //        list.Add(item);

            //        var item1 = new TreeViewViewModel
            //        {
            //            id = service.StartDate.ToString(),
            //            Name = service.StartDate.ToString(),
            //            Type = "BODY",
            //            ParentId = "SHEADER",
            //        };
            //        list.Add(item1);
            //        var item2 = new TreeViewViewModel
            //        {
            //            id = service.ServiceStatusName,
            //            Name = service.ServiceStatusName,
            //            Type = "BODY",
            //            ParentId = "SHEADER",
            //        };
            //        list.Add(item2);
            //    }

            //}
            //else if (type == "SDETAIL")
            //{
            //    var service = await GetServiceDetails(new ServiceTemplateViewModel { ServiceId = parentId });
            //    foreach (var column in service.ColumnList)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = column.Name,
            //            Name = column.Name + ":" + column.Value,
            //            Type = "BODY",
            //            ParentId = "SDETAIL",
            //        };
            //        list.Add(item);
            //    }
            //}
            //else if (type == "SCOMMENT")
            //{
            //    var _notificationBusiness = _serviceProvider.GetService<IPushNotificationBusiness>();
            //    var notification = await _notificationBusiness.GetServiceNotificationList(parentId, _userContext.UserId, 0);
            //    foreach (var column in notification)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = column.Subject,
            //            Name = column.Subject,
            //            Type = "BODY",
            //            ParentId = "SCOMMENT",
            //        };
            //        list.Add(item);
            //    }
            //}
            //else if (type == "SNOTIFICATION")
            //{
            //    var _ntsServiceCommentBusiness = _serviceProvider.GetService<INtsServiceCommentBusiness>();
            //    var notification = await _ntsServiceCommentBusiness.GetSearchResult(parentId);
            //    foreach (var column in notification)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = column.Id,
            //            Name = column.Comment,
            //            Type = "BODY",
            //            ParentId = "SNOTIFICATION",
            //        };
            //        list.Add(item);
            //    }
            //}
            else if (type == "TASK")
            {
                //query = $@"select distinct t.""Id"" as id,t.""TaskSubject"" as Name, '{docServiceId}' as ParentId, 'STEPTASK' as Type,
                //            false as hasChildren
                //            from public.""NtsTask"" as t
                //            join public.""TaskTemplate"" as tt on tt.""Id""=t.""TaskTemplateId"" and tt.""IsDeleted""=false
                //        where t.""IsDeleted""=false and t.""ParentServiceId""='{docServiceId}' and tt.""TaskTemplateType""='2' ";
                //list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                var item = new TreeViewViewModel
                {
                    id = "THEADER",
                    Name = "Header",
                    ParentId = id,
                    hasChildren = false,
                    Type = "THEADER",
                    children = true,
                    text = "Header",
                    parent = id,
                    a_attr = new { data_id = "THEADER", data_name = "Header", data_type = "THEADER" }
                };

                list.Add(item);

                var item1 = new TreeViewViewModel
                {
                    id = "TDETAIL",
                    Name = "Detail",
                    ParentId = id,
                    hasChildren = false,
                    Type = "TDETAIL",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "TDETAIL", data_name = "Detail", data_type = "TDETAIL" }
                };

                list.Add(item1);
                var item2 = new TreeViewViewModel
                {
                    id = "TNOTIFICATION",
                    Name = "Notification",
                    ParentId = id,
                    hasChildren = false,
                    Type = "TNOTIFICATION",
                    children = true,
                    text = "Notification",
                    parent = id,
                    a_attr = new { data_id = "TNOTIFICATION", data_name = "Notification", data_type = "TNOTIFICATION" }
                };
                list.Add(item2);

                var item3 = new TreeViewViewModel
                {
                    id = "TCOMMENT",
                    Name = "Comment",
                    ParentId = id,
                    hasChildren = false,
                    Type = "TCOMMENT",
                    children = true,
                    text = "Comment",
                    parent = id,
                    a_attr = new { data_id = "TCOMMENT", data_name = "Comment", data_type = "TCOMMENT" }
                };

                list.Add(item3);
            }
            //else if (type == "THEADER")
            //{
            //    query = @$"Select t.*,lv.""Name"" as TaskStatusName from public.""NtsTask"" as t 
            //            join public.""LOV"" as lv on lv.""Id""=t.""TaskStatusId""
            //            where t.""ParentId""='{parentId}'
            //            ";

            //    var task = await _queryRepo.ExecuteQuerySingle<TaskViewModel>(query, null);
            //    if (task != null)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = task.TaskSubject,
            //            Name = task.TaskSubject,
            //            Type = "BODY",
            //            ParentId = "THEADER",
            //        };
            //        list.Add(item);

            //        var item1 = new TreeViewViewModel
            //        {
            //            id = task.StartDate.ToString(),
            //            Name = task.StartDate.ToString(),
            //            Type = "BODY",
            //            ParentId = "THEADER",
            //        };
            //        list.Add(item1);
            //        var item2 = new TreeViewViewModel
            //        {
            //            id = task.TaskStatusName,
            //            Name = task.TaskStatusName,
            //            Type = "BODY",
            //            ParentId = "THEADER",
            //        };
            //        list.Add(item2);
            //    }

            //}
            //else if (type == "TDETAIL")
            //{
            //    var task = await _taskBusiness.GetTaskDetails(new TaskTemplateViewModel { ParentServiceId = parentId });
            //    foreach (var column in task.ColumnList)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = column.Name,
            //            Name = column.Name + ":" + column.Value,
            //            Type = "BODY",
            //            ParentId = "TDETAIL",
            //        };
            //        list.Add(item);
            //    }
            //}
            //else if (type == "TCOMMENT")
            //{
            //    var _notificationBusiness = _serviceProvider.GetService<IPushNotificationBusiness>();
            //    var notification = await _notificationBusiness.GetTaskNotificationList(parentId, _userContext.UserId, 0);
            //    foreach (var column in notification)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = column.Subject,
            //            Name = column.Subject,
            //            Type = "BODY",
            //            ParentId = "TCOMMENT",
            //        };
            //        list.Add(item);
            //    }
            //}
            //else if (type == "TNOTIFICATION")
            //{
            //    var _ntsServiceCommentBusiness = _serviceProvider.GetService<INtsServiceCommentBusiness>();
            //    var notification = await _ntsServiceCommentBusiness.GetSearchResult(parentId);
            //    foreach (var column in notification)
            //    {
            //        var item = new TreeViewViewModel
            //        {
            //            id = column.Id,
            //            Name = column.Comment,
            //            Type = "BODY",
            //            ParentId = "SNOTIFICATION",
            //        };
            //        list.Add(item);
            //    }
            //}
            else if (type == "NOTE")
            {
                //query = $@"select distinct n.""Id"" as id,n.""NoteSubject"" as Name, '{docServiceId}' as ParentId, 'NOTE' as Type,
                //            false as hasChildren
                //            from public.""NtsNote"" as n
                //        where n.""IsDeleted""=false and n.""ParentServiceId""='{docServiceId}' ";
                //list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                var item = new TreeViewViewModel
                {
                    id = "NHEADER",
                    Name = "Header",
                    ParentId = id,
                    hasChildren = false,
                    Type = "NHEADER",
                    children = true,
                    text = "Comment",
                    parent = id,
                    a_attr = new { data_id = "NHEADER", data_name = "Header", data_type = "NHEADER" }
                };

                list.Add(item);

                var item1 = new TreeViewViewModel
                {
                    id = "NDETAIL",
                    Name = "Detail",
                    ParentId = id,
                    hasChildren = false,
                    Type = "NDETAIL",
                    children = true,
                    text = "Detail",
                    parent = id,
                    a_attr = new { data_id = "NDETAIL", data_name = "Detail", data_type = "NDETAIL" }
                };

                list.Add(item1);

                var item3 = new TreeViewViewModel
                {
                    id = "NCOMMENT",
                    Name = "Comment",
                    ParentId = id,
                    hasChildren = false,
                    Type = "NCOMMENT",
                    children = true,
                    text = "Comment",
                    parent = id,
                    a_attr = new { data_id = "NCOMMENT", data_name = "Comment", data_type = "NCOMMENT" }
                };

                list.Add(item3);

            }
            return list;
        }
        public async Task<ServiceTemplateViewModel> GetBookDetails(string serviceId)
        {
            var query = @$"select ""TM"".*,""S"".""Id"" as ""ServiceId"",""T"".""DisplayName"" as ""TemplateDisplayName""
            from ""public"".""ServiceTemplate"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""Id""=""TM"".""TemplateId"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsService"" as ""S"" on ""T"".""Id""=""S"".""TemplateId"" and ""S"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false  and ""S"".""Id""='{serviceId}'";
            var model = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            model.BookItems = await GetBookList(serviceId, null, true);
            return model;
        }
        public async Task<List<NtsViewModel>> GetBookList(string serviceId, string templateId, bool includeitemDetails = false)
        {
            string query = @$"select t.""DisplayName"" as ""TemplateName"",s.""ServiceSubject"" as ""Subject"",s.""ServiceDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,null as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,0 as ""Level"",'Service' as ""NtsType"",s.""Id"" as ""Id"",1 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""ServiceNo"" as ""NtsNo"",true as ""AutoLoad"",0 as ""SequenceOrder"",'1' as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsService"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""ServiceTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
	        left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""ServicePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsServiceShared"" as shr on shr.""NtsServiceId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'            
            where s.""Id""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            union all
           ( select t.""DisplayName"" as ""TemplateName"",s.""TaskSubject"" as ""Subject"",s.""TaskDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,'-1' as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Task' as ""NtsType"",s.""Id"" as ""Id"",2 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""TaskNo"" as ""NtsNo"",false as ""AutoLoad"",stc.""SequenceOrder"" as ""SequenceOrder"",'1' as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            FROM public.""NtsTask"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""TaskTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""StepTaskComponent"" as stc on s.""StepTaskComponentId""=stc.""Id"" and  stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lv on lv.""Id""=s.""TaskStatusId"" and lv.""IsDeleted"" = false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""TaskPriorityId"" and lp.""IsDeleted"" = false  and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and u.""IsDeleted"" = false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsTaskShared"" as shr on shr.""NtsTaskId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'            
            WHERE  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""ParentServiceId""='{serviceId}' 
                        and s.""TaskType""='2' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or s.""AssignedToUserId""='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            order by stc.""SequenceOrder"" )
             union all
            select t.""DisplayName"" as ""TemplateName"",s.""NoteSubject"" as ""Subject"",s.""NoteDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentNoteId"",s.""ParentTaskId"",s.""ParentServiceId"") as parentId,t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Note' as NtsType,s.""Id"" as Id,14 as ItemType,u.""Name"" as AssigneeOrOwner
            ,s.""ExpiryDate"" as DueDate,s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""NoteNo"" as NtsNo,false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as StartDate,s.""ReminderDate"" as ReminderDate,s.""CompletedDate"" as CompletedDate
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsNote"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and (t.""ViewType"" is null or t.""ViewType""<>2) and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""NoteTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""NoteStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""NotePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsNoteShared"" as shr on shr.""NtsNoteId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'             
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""TaskSubject"" as ""Subject"",s.""TaskDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentNoteId"",s.""ParentTaskId"",s.""ParentServiceId"") as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Task' as ""NtsType"",s.""Id"" as ""Id"",4 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""TaskNo"" as ""NtsNo"",false as ""AutoLoad"",s.""SequenceOrder"" as ""SequenceOrder"",null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsTask"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""TaskTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""TaskStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""TaskPriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsTaskShared"" as shr on shr.""NtsTaskId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                        
            where s.""ServicePlusId""='{serviceId}' and s.""TaskType""=4 and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or s.""AssignedToUserId""='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')            
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""ServiceSubject"" as ""Subject"",s.""ServiceDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentNoteId"",s.""ParentTaskId"",s.""ParentServiceId"") as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Service' as ""NtsType"",s.""Id"" as ""Id"",13 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""ServiceNo"" as ""NtsNo"",false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsService"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and (t.""ViewType"" is null or t.""ViewType""<>2) and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""ServiceTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""ServicePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsServiceShared"" as shr on shr.""NtsServiceId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                        
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""NoteSubject"" as ""Subject"",s.""NoteDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,'-2' as parentId,t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Note' as NtsType,s.""Id"" as Id,16 as ItemType,u.""Name"" as AssigneeOrOwner
            ,s.""ExpiryDate"" as DueDate,s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""NoteNo"" as NtsNo,false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as StartDate,s.""ReminderDate"" as ReminderDate,s.""CompletedDate"" as CompletedDate
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsNote"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""ViewType""=2 and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""NoteTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""NoteStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""NotePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsNoteShared"" as shr on shr.""NtsNoteId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                         
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')           
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""ServiceSubject"" as ""Subject"",s.""ServiceDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,'-2' as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Service' as ""NtsType"",s.""Id"" as ""Id"",16 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""ServiceNo"" as ""NtsNo"",false as ""AutoLoad"",nt.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsService"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""ViewType""=2 and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""ServiceTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""ServicePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsServiceShared"" as shr on shr.""NtsServiceId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                        
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            ";
            var list = await _queryRepo.ExecuteQueryList<NtsViewModel>(query, null);
            var root = list.FirstOrDefault(x => x.Level == 0);
            if (list.Any(x => x.ItemType == ItemTypeEnum.StepTask))
            {
                list.Add(new NtsViewModel
                {
                    TemplateName = "Step Tasks",
                    Subject = "Step Tasks",
                    Level = 1,
                    parentId = serviceId,
                    SequenceOrder = -1,
                    Id = "-1"
                });
            }
            list.Add(new NtsViewModel
            {
                TemplateName = "Child Books",
                Subject = "Child Books",
                Level = 1,
                parentId = serviceId,
                SequenceOrder = -2,
                Id = "-2",
                ItemType = ItemTypeEnum.BookSection
            });
            ProcessBookList(root, list, includeitemDetails);
            return list.OrderBy(x => x.Level).ThenBy(x => x.SequenceOrder).ThenBy(x => x.TemplateSequence).ThenBy(x => x.Sequence).ToList();
        }

        private void ProcessBookList(NtsViewModel root, List<NtsViewModel> list, bool includeitemDetails)
        {
            if (root != null)
            {
                var childs = list.Where(x => x.parentId == root.Id).OrderBy(x => x.SequenceOrder);
                root.MaxChildSequence = 0;
                if (childs.Any())
                {
                    root.HasChild = true;
                    root.MaxChildSequence = childs.Count();
                    var i = 1;
                    foreach (var item in childs)
                    {
                        item.Level = root.Level + 1;
                        item.ItemNo = $"{root.ItemNo}.{i++}";
                        item.ParentNtsType = root.NtsType;
                        ProcessBookList(item, list, includeitemDetails);
                    }
                }
            }
        }

        public async Task<NtsBookItemViewModel> GetBookById(string id)
        {
            string query = @$"select s.""ServiceSubject"" as Subject,'Service' as NtsType
,s.""Id"" as Id,coalesce(s.""ParentServiceId"",s.""ParentTaskId"",s.""ParentNoteId"") as parentId,s.""SequenceOrder""
            from public.""NtsService"" as s
            where s.""Id""='{id}' and s.""IsDeleted""=false
            union
            select s.""TaskSubject"" as Subject,'Task' as NtsType,s.""Id"" as Id
,coalesce(s.""ParentServiceId"",s.""ParentTaskId"",s.""ParentNoteId"") as parentId,s.""SequenceOrder""
            from public.""NtsTask"" as s
            where s.""Id""='{id}' and s.""IsDeleted""=false
            union
            select s.""NoteSubject"" as Subject,'Note' as NtsType,s.""Id"" as Id
,coalesce(s.""ParentServiceId"",s.""ParentTaskId"",s.""ParentNoteId"") as parentId,s.""SequenceOrder""
            from public.""NtsNote"" as s
            where s.""Id""='{id}' and s.""IsDeleted""=false

";



            var list = await _queryRepo.ExecuteQuerySingle<NtsBookItemViewModel>(query, null);
            return list;
        }

        public async Task<List<NtsViewModel>> GetServiceBookDetails(string serviceId)
        {
            List<NtsViewModel> ntslist = new List<NtsViewModel>();
            var data = await GetBookDetails(serviceId);
            ntslist = data.BookItems;
            if (ntslist != null && ntslist.Count() > 0)
            {
                foreach (var item in ntslist)
                {
                    item.Description = item.Description.HtmlDecode();
                }
            }
            return ntslist;
        }

        public async Task<List<ServiceViewModel>> ReadServiceBookData(string moduleCodes, string templateCodes, string categoryCodes)
        {

            var list = new List<ServiceViewModel>();
            string query = $@"Select s.""Id"",s.""ServiceNo"",s.""ServiceSubject"",s.""TemplateCode"",o.""Name"" as OwnerDisplayName,ts.""Name"" as ServiceStatusName,ts.""Code"" as ServiceStatusCode,
                         s.""ParentServiceId"",s.""StartDate"",s.""DueDate"",s.""RequestedByUserId"", tem.""DisplayName"" as TemplateDisplayName,
                            s.""CreatedDate"",s.""CompletedDate"" as CompletedDate, s.""RejectedDate"" as RejectedDate, s.""CanceledDate"" as CanceledDate, s.""ClosedDate"" as ClosedDate
,s.""Id"" as key,s.""ServiceNo"" as title,true as lazy                     
From public.""NtsService"" as s
                            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false #TemCodeWhere# 
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false #ModCodeWhere# 
                            Join public.""User"" as o on s.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on s.""ServiceStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                            where s.""IsDeleted""=false and s.""PortalId""='{_userContext.PortalId}'";
            //            string query = @$"With recursive Service as(
            //Select s.""Id"",s.""ServiceNo"",s.""ServiceSubject"",s.""TemplateCode"",o.""Name"" as OwnerDisplayName,ts.""Name"" as ServiceStatusName,ts.""Code"" as ServiceStatusCode,
            //                         s.""ParentServiceId"",s.""StartDate"",s.""DueDate"",s.""RequestedByUserId"", tem.""DisplayName"" as TemplateDisplayName,
            //                            s.""CreatedDate"",s.""CompletedDate"" as CompletedDate, s.""RejectedDate"" as RejectedDate, s.""CanceledDate"" as CanceledDate, s.""ClosedDate"" as ClosedDate
            //                            From public.""NtsService"" as s
            //                            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false #TemCodeWhere# 
            //                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
            //                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false #ModCodeWhere# 
            //                            Join public.""User"" as o on s.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
            //                            Join public.""LOV"" as ts on s.""ServiceStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
            //                            where s.""IsDeleted""=false and s.""ParentServiceId"" is null and s.""PortalId""='{_userContext.PortalId}'
            //union
            //	Select s.""Id"",s.""ServiceNo"",s.""ServiceSubject"",s.""TemplateCode"",o.""Name"" as OwnerDisplayName,ts.""Name"" as ServiceStatusName,ts.""Code"" as ServiceStatusCode,
            //                       s.""ParentServiceId"",s.""StartDate"",s.""DueDate"",s.""RequestedByUserId"", tem.""DisplayName"" as TemplateDisplayName,
            //                            s.""CreatedDate"",s.""CompletedDate"" as CompletedDate, s.""RejectedDate"" as RejectedDate, s.""CanceledDate"" as CanceledDate, s.""ClosedDate"" as ClosedDate
            //                            From public.""NtsService"" as s
            //	                        join Service as rs on rs.""Id""=s.""ParentServiceId""
            //                            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            //                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
            //                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  
            //                            Join public.""User"" as o on s.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
            //                            Join public.""LOV"" as ts on s.""ServiceStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
            //                            where s.""IsDeleted""=false and s.""PortalId""='{_userContext.PortalId}'


            //)select * from Service ";



            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and tem.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            var result = await _queryRepo.ExecuteQueryList(query, null);
            result = result.OrderByDescending(x => x.CreatedDate).ToList();
            //foreach (var i in result)
            //{
            //    i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //    i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //}
            return result;
        }

        public async Task<List<IdNameViewModel>> GetAllBookChild(List<IdNameViewModel> list, string serviceId)
        {
            var child = await GetList(x => x.ParentServiceId == serviceId);
            foreach (var item in child)
            {
                list.Add(new IdNameViewModel { Id = item.Id });
                await GetAllBookChild(list, item.Id);
            }
            return list;
        }

        public async Task DeleteServiceBook(string serviceId)
        {
            var list = new List<IdNameViewModel>();
            list.Add(new IdNameViewModel { Id = serviceId });
            var childlist = await GetAllBookChild(list, serviceId);
            foreach (var child in childlist)
            {
                var servicePlus = await GetList(x => x.ServicePlusId == child.Id);
                var taskPlus = await _taskBusiness.GetList(x => x.ServicePlusId == child.Id);
                var notePlus = await _noteBusiness.GetList(x => x.ServicePlusId == child.Id);
                foreach (var item in servicePlus)
                {
                    await Delete(item.Id);
                }
                foreach (var item in taskPlus)
                {
                    await _taskBusiness.Delete(item.Id);
                }
                foreach (var item in notePlus)
                {
                    await _noteBusiness.Delete(item.Id);
                }
                await Delete(child.Id);
                //var query = $@"update  cms.""N_SNC_TECProcess_ProcessCategory"" set ""IsDeleted""=true";
                //await _queryRepo.ExecuteCommand(query, null);


            }
            //var serviceDetails = await GetList(x => x.ServicePlusId == serviceId);
            //var taskDetails = await _taskBusiness.GetList(x => x.ServicePlusId == serviceId);
            //var noteeDetails = await _noteBusiness.GetList(x => x.ServicePlusId == serviceId);
            //foreach(var item in serviceDetails)
            //{
            //    await Delete(item.Id);
            //}
            //foreach (var item in taskDetails)
            //{
            //    await _taskBusiness.Delete(item.Id);
            //}
            //foreach (var item in noteeDetails)
            //{
            //    await _noteBusiness.Delete(item.Id);
            //}
            //await Delete(serviceId);
        }



        public async Task<CommandResult<ServiceViewModel>> ManageMoveToParentOld(ServiceViewModel model)
        {
            var moveToparent = await GetBookById(model.MoveToParent);
            var childlist = await GetBookList(model.Id, null);
            var postionId = childlist.FirstOrDefault(x => x.Id == model.MovePostionId);
            long? seqOrder = null;
            if (postionId.IsNotNull() && model.MovePostionSeq == MovePostionEnum.Before)
            {
                seqOrder = postionId.SequenceOrder;
                var serviceChildlist = childlist.Where(x => x.SequenceOrder >= postionId.SequenceOrder && x.parentId == model.MoveToParent && x.NtsType == NtsTypeEnum.Service).ToList();
                var taskChildlist = childlist.Where(x => x.SequenceOrder >= postionId.SequenceOrder && x.parentId == model.MoveToParent && x.NtsType == NtsTypeEnum.Task).ToList();
                var noteChildlist = childlist.Where(x => x.SequenceOrder >= postionId.SequenceOrder && x.parentId == model.MoveToParent && x.NtsType == NtsTypeEnum.Note).ToList();
                foreach (var item in serviceChildlist)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder + 1;
                    await Edit(service);
                }
                foreach (var item in taskChildlist)
                {
                    var task = await _taskBusiness.GetSingleById(item.Id);
                    task.SequenceOrder = task.SequenceOrder + 1;
                    await _taskBusiness.Edit(task);
                }
                foreach (var item in noteChildlist)
                {
                    var note = await _noteBusiness.GetSingleById(item.Id);
                    note.SequenceOrder = note.SequenceOrder + 1;
                    await _noteBusiness.Edit(note);
                }
            }
            else if (postionId.IsNotNull() && model.MovePostionSeq == MovePostionEnum.After)
            {
                seqOrder = postionId.SequenceOrder + 1;
                var serviceChildlist = childlist.Where(x => x.SequenceOrder > postionId.SequenceOrder && x.parentId == model.MoveToParent && x.NtsType == NtsTypeEnum.Service).ToList();
                var taskChildlist = childlist.Where(x => x.SequenceOrder > postionId.SequenceOrder && x.parentId == model.MoveToParent && x.NtsType == NtsTypeEnum.Task).ToList();
                var noteChildlist = childlist.Where(x => x.SequenceOrder > postionId.SequenceOrder && x.parentId == model.MoveToParent && x.NtsType == NtsTypeEnum.Note).ToList();
                foreach (var item in serviceChildlist)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder + 1;
                    await Edit(service);
                }
                foreach (var item in taskChildlist)
                {
                    var task = await _taskBusiness.GetSingleById(item.Id);
                    task.SequenceOrder = task.SequenceOrder + 1;
                    await _taskBusiness.Edit(task);
                }
                foreach (var item in noteChildlist)
                {
                    var note = await _noteBusiness.GetSingleById(item.Id);
                    note.SequenceOrder = note.SequenceOrder + 1;
                    await _noteBusiness.Edit(note);
                }
            }
            var parentService = "";
            var parentNote = "";
            var parentTask = "";
            if (moveToparent.IsNotNull() && moveToparent.NtsType == NtsTypeEnum.Service)
            {
                parentService = model.MoveToParent;
                parentNote = null;
                parentTask = null;
            }
            else if (moveToparent.IsNotNull() && moveToparent.NtsType == NtsTypeEnum.Task)
            {
                parentService = null;
                parentNote = null;
                parentTask = model.MoveToParent;
            }
            else if (moveToparent.IsNotNull() && moveToparent.NtsType == NtsTypeEnum.Note)
            {
                parentService = null;
                parentNote = model.MoveToParent;
                parentTask = null;
            }
            if (model.NtsType == NtsTypeEnum.Service)
            {
                var service = await GetSingleById(model.NtsId);
                service.ParentServiceId = parentService;
                service.ParentNoteId = parentNote;
                service.ParentTaskId = parentTask;
                service.SequenceOrder = seqOrder;

                var serviceedit = await Edit(service);

            }
            else if (model.NtsType == NtsTypeEnum.Task && moveToparent.IsNotNull() && moveToparent.NtsType == NtsTypeEnum.Task)
            {
                var task = await _taskBusiness.GetSingleById(model.NtsId);
                task.ParentServiceId = parentService;
                task.ParentNoteId = parentNote;
                task.ParentTaskId = parentTask;
                task.SequenceOrder = seqOrder;
                var edit = await _taskBusiness.Edit(task);
            }
            else if (model.NtsType == NtsTypeEnum.Note && moveToparent.IsNotNull() && moveToparent.NtsType == NtsTypeEnum.Note)
            {
                var note = await _noteBusiness.GetSingleById(model.NtsId);
                note.ParentServiceId = parentService;
                note.ParentNoteId = parentNote;
                note.ParentTaskId = parentTask;
                note.SequenceOrder = seqOrder;
                var edit = await _noteBusiness.Edit(note);
            }


            return CommandResult<ServiceViewModel>.Instance(model);
        }

        public async Task<CommandResult<ServiceViewModel>> ManageMoveToParent(ServiceViewModel model)
        {
            var moveToparent = await GetBookById(model.MoveToParent);
            var childlist = await GetBookList(model.Id, null);
            var siblings = childlist.Where(x => x.parentId == moveToparent.parentId).ToList();
            var parentDetails = childlist.FirstOrDefault(x => x.Id == moveToparent.parentId);
            var ntsDetails = childlist.FirstOrDefault(x => x.Id == model.NtsId);
            var ntsSiblings = childlist.Where(x => x.parentId == ntsDetails.parentId && x.SequenceOrder > ntsDetails.SequenceOrder).ToList();
            long? seqOrder = null;
            if (model.MoveType == BookMoveTypeEnum.SameAsLevel && model.MovePostionSeq == MovePostionEnum.Before)
            {
                seqOrder = moveToparent.SequenceOrder;
                var serviceChildlist = siblings.Where(x => x.SequenceOrder >= moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Service).ToList();
                var taskChildlist = siblings.Where(x => x.SequenceOrder >= moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Task).ToList();
                var noteChildlist = siblings.Where(x => x.SequenceOrder >= moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Note).ToList();
                foreach (var item in serviceChildlist)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder + 1;
                    await Edit(service);
                }
                foreach (var item in taskChildlist)
                {
                    var task = await _taskBusiness.GetSingleById(item.Id);
                    task.SequenceOrder = task.SequenceOrder + 1;
                    await _taskBusiness.Edit(task);
                }
                foreach (var item in noteChildlist)
                {
                    var note = await _noteBusiness.GetSingleById(item.Id);
                    note.SequenceOrder = note.SequenceOrder + 1;
                    await _noteBusiness.Edit(note);
                }
            }
            else if (model.MoveType == BookMoveTypeEnum.SameAsLevel && model.MovePostionSeq == MovePostionEnum.After)
            {
                seqOrder = moveToparent.SequenceOrder + 1;
                var serviceChildlist = siblings.Where(x => x.SequenceOrder > moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Service).ToList();
                var taskChildlist = siblings.Where(x => x.SequenceOrder > moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Task).ToList();
                var noteChildlist = siblings.Where(x => x.SequenceOrder > moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Note).ToList();
                foreach (var item in serviceChildlist)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder + 1;
                    await Edit(service);
                }
                foreach (var item in taskChildlist)
                {
                    var task = await _taskBusiness.GetSingleById(item.Id);
                    task.SequenceOrder = task.SequenceOrder + 1;
                    await _taskBusiness.Edit(task);
                }
                foreach (var item in noteChildlist)
                {
                    var note = await _noteBusiness.GetSingleById(item.Id);
                    note.SequenceOrder = note.SequenceOrder + 1;
                    await _noteBusiness.Edit(note);
                }
            }

            else if (model.MoveType == BookMoveTypeEnum.Child)
            {
                var currentChildList = childlist.Where(x => x.parentId == model.MoveToParent).ToList();
                seqOrder = currentChildList.Count + 1;
                parentDetails = childlist.FirstOrDefault(x => x.Id == moveToparent.Id);
            }
            var parentService = "";
            var parentNote = "";
            var parentTask = "";
            if (parentDetails.IsNotNull() && parentDetails.NtsType == NtsTypeEnum.Service)
            {
                parentService = parentDetails.Id;
                parentNote = null;
                parentTask = null;
            }
            else if (parentDetails.IsNotNull() && parentDetails.NtsType == NtsTypeEnum.Task)
            {
                parentService = null;
                parentNote = null;
                parentTask = parentDetails.Id;
            }
            else if (parentDetails.IsNotNull() && parentDetails.NtsType == NtsTypeEnum.Note)
            {
                parentService = null;
                parentNote = parentDetails.Id;
                parentTask = null;
            }
            if (model.NtsType == NtsTypeEnum.Service)
            {
                var service = await GetSingleById(model.NtsId);
                service.ParentServiceId = parentService;
                service.ParentNoteId = parentNote;
                service.ParentTaskId = parentTask;
                service.SequenceOrder = seqOrder;

                var serviceedit = await Edit(service);

            }
            else if (model.NtsType == NtsTypeEnum.Task)
            {
                var task = await _taskBusiness.GetSingleById(model.NtsId);
                task.ParentServiceId = parentService;
                task.ParentNoteId = parentNote;
                task.ParentTaskId = parentTask;
                task.SequenceOrder = seqOrder;
                var edit = await _taskBusiness.Edit(task);
            }
            else if (model.NtsType == NtsTypeEnum.Note)
            {
                var note = await _noteBusiness.GetSingleById(model.NtsId);
                note.ParentServiceId = parentService;
                note.ParentNoteId = parentNote;
                note.ParentTaskId = parentTask;
                note.SequenceOrder = seqOrder;
                var edit = await _noteBusiness.Edit(note);
            }
            foreach (var item in ntsSiblings)
            {
                if (item.NtsType == NtsTypeEnum.Service)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Task)
                {
                    var service = await _taskBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _taskBusiness.Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Note)
                {
                    var service = await _noteBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _noteBusiness.Edit(service);
                }
            }

            return CommandResult<ServiceViewModel>.Instance(model);
        }
        public async Task<List<ColumnMetadataViewModel>> GetServiceColumnById(string ServiceId)
        {
            List<ColumnMetadataViewModel> collist = new List<ColumnMetadataViewModel>();
            var data = new ServiceTemplateViewModel();
            var query = @$"select ""TM"".* from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""UdfTableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsService"" as ""S"" on ""T"".""Id""=""S"".""TemplateId"" and ""S"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false  and ""S"".""Id""='{ServiceId}'";
            var tableMetadata = await _queryRepo.ExecuteQuerySingle<TableMetadataViewModel>(query, null);
            if (tableMetadata != null)
            {


                var selectQuery = "";

                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsService"".""Id""='{ServiceId}' limit 1", null, null, false, null);

                }

                data = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(selectQuery, null);

                if (data.IsNotNull())
                {
                    data.IsLogRecord = false;
                    data.LogId = null;
                    data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetadata.Id
                    && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
                    if (data.ColumnList != null)
                    {
                        var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            foreach (var item in data.ColumnList.Where(x => x.IsUdfColumn))
                            {
                                try
                                {
                                    item.Value = dr[item.Name];
                                    if (Convert.ToString(item.Value) == "")
                                    {
                                        item.Value = null;
                                    }
                                }
                                catch (Exception)
                                {


                                }

                            }
                        }
                    }
                    collist = data.ColumnList;
                }
            }
            return collist;
        }

        public async Task<List<NtsBookItemViewModel>> GetBookItemChildList(string serviceId, string noteId, string taskId)
        {
            string query = @$"select s.""ServiceSubject"" as Subject,'Service' as NtsType,s.""Id"" as Id,s.""SequenceOrder""
            from public.""NtsService"" as s
            where  s.""IsDeleted""=false #SERVICE# #TASK# #NOTE#
            union
            select s.""TaskSubject"" as Subject,'Task' as NtsType,s.""Id"" as Id,s.""SequenceOrder""
            from public.""NtsTask"" as s
            where   s.""IsDeleted""=false #SERVICE# #TASK# #NOTE#
            union
            select s.""NoteSubject"" as Subject,'Note' as NtsType,s.""Id"" as Id,s.""SequenceOrder""
            from public.""NtsNote"" as s
            where s.""IsDeleted""=false #SERVICE# #TASK# #NOTE#

";
            var serviceFilter = "";
            var noteFilter = "";
            var taskFilter = "";
            if (serviceId.IsNotNullAndNotEmpty())
            {
                serviceFilter = $@" and ""ParentServiceId""='{serviceId}' ";
            }
            else if (noteId.IsNotNullAndNotEmpty())
            {
                noteFilter = $@" and ""ParentNoteId""='{noteId}' ";
            }
            else if (taskId.IsNotNullAndNotEmpty())
            {
                taskFilter = $@" and ""ParentTaskId""='{taskId}' ";
            }
            query = query.Replace("#SERVICE#", serviceFilter);
            query = query.Replace("#TASK#", taskFilter);
            query = query.Replace("#NOTE#", noteFilter);


            var list = await _queryRepo.ExecuteQueryList<NtsBookItemViewModel>(query, null);
            return list;
        }

        public async Task<List<NtsBookItemViewModel>> GetAllBookItemChild(List<NtsBookItemViewModel> list, string serviceId)
        {
            var data = await GetBookById(serviceId);
            list.Add(data);
            if (data.IsNotNull() && data.NtsType == NtsTypeEnum.Service)
            {
                var childList = await GetBookItemChildList(data.Id, null, null);
                foreach (var item in childList)
                {
                    // list.Add(item);
                    await GetAllBookItemChild(list, item.Id);
                }
            }
            else if (data.IsNotNull() && data.NtsType == NtsTypeEnum.Task)
            {
                var childList = await GetBookItemChildList(null, null, data.Id);
                foreach (var item in childList)
                {
                    //list.Add(item);
                    await GetAllBookItemChild(list, item.Id);
                }
            }
            else if (data.IsNotNull() && data.NtsType == NtsTypeEnum.Note)
            {
                var childList = await GetBookItemChildList(null, data.Id, null);
                foreach (var item in childList)
                {
                    //list.Add(item);
                    await GetAllBookItemChild(list, item.Id);
                }
            }
            return list;
        }

        public async Task DeleteServiceBookItem(string serviceId, string parentId, NtsTypeEnum parentType)
        {
            var list = new List<NtsBookItemViewModel>();
            var siblinglist = new List<NtsBookItemViewModel>();
            // list.Add(new NtsBookItemViewModel { Id = serviceId });
            var childlist = await GetAllBookItemChild(list, serviceId);
            if (parentType == NtsTypeEnum.Service)
            {
                siblinglist = await GetBookItemChildList(parentId, null, null);
            }
            else if (parentType == NtsTypeEnum.Task)
            {
                siblinglist = await GetBookItemChildList(null, null, parentId);
            }
            else if (parentType == NtsTypeEnum.Note)
            {
                siblinglist = await GetBookItemChildList(null, parentId, null);
            }
            var currentNts = childlist.FirstOrDefault(x => x.Id == serviceId);
            siblinglist = siblinglist.Where(x => x.SequenceOrder > currentNts.SequenceOrder).ToList();
            foreach (var item in siblinglist)
            {
                if (item.NtsType == NtsTypeEnum.Service)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Task)
                {
                    var service = await _taskBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _taskBusiness.Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Note)
                {
                    var service = await _noteBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _noteBusiness.Edit(service);
                }
            }
            foreach (var item in childlist)
            {
                if (item.NtsType == NtsTypeEnum.Service)
                {
                    await Delete(item.Id);
                }
                else if (item.NtsType == NtsTypeEnum.Task)
                {
                    await _taskBusiness.Delete(item.Id);
                }
                else if (item.NtsType == NtsTypeEnum.Note)
                {
                    await _noteBusiness.Delete(item.Id);
                }
            }
            await UpdateBookSequenceOrderOnDelete(parentId, currentNts.SequenceOrder, serviceId);
        }

        public async Task<List<ColumnMetadataViewModel>> GetServiceViewableColumns()
        {
            var columns = new List<ColumnMetadataViewModel>();
            var tables = new List<string>();
            var condition = new List<string>();
            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
                where 
                (
                    (t.""Schema""='public' and t.""Name""='ServiceTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsService')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false ";
            var pkColumns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            foreach (var item in pkColumns)
            {
                if (item.TableName == "NtsService" && item.IsPrimaryKey)
                {
                    item.Alias = "ServiceId";
                }
                else
                {
                    item.Alias = item.Name;
                }

                columns.Add(item);
            }

            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "Id", Alias = "CreatedByUser_Id" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "Name", Alias = "CreatedByUser_Name" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "Email", Alias = "CreatedByUser_Email" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "JobTitle", Alias = "CreatedByUser_JobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "CreatedByUser", Name = "PhotoId", Alias = "CreatedByUser_PhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "Id", Alias = "UpdatedByUser_Id" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "Name", Alias = "UpdatedByUser_Name" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "Email", Alias = "UpdatedByUser_Email" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "JobTitle", Alias = "UpdatedByUser_JobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UpdatedByUser", Name = "PhotoId", Alias = "UpdatedByUser_PhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "Id", Alias = "OwnerUserId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "Name", Alias = "OwnerUserName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "Email", Alias = "OwnerUserEmail" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "JobTitle", Alias = "OwnerUserJobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "OwnerUser", Name = "PhotoId", Alias = "OwnerUserPhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "Id", Alias = "RequestedByUserId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "Name", Alias = "RequestedByUserName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "Email", Alias = "RequestedByUserEmail" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "JobTitle", Alias = "RequestedByUserJobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "RequestedByUser", Name = "PhotoId", Alias = "RequestedByUserPhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "TableMetadataId", Alias = "TableMetadataId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Name", Alias = "TemplateName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "DisplayName", Alias = "TemplateDisplayName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "TableSelectionType", Alias = "TableSelectionType" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Code", Alias = "TemplateCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "ViewType", Alias = "ViewType" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "Json", Alias = "Json" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "TableMetadataId", Alias = "UdfTableMetadataId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceOwnerType", Name = "Code", Alias = "ServiceOwnerTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceOwnerType", Name = "Name", Alias = "ServiceOwnerTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServicePriority", Name = "Code", Alias = "ServicePriorityCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServicePriority", Name = "Name", Alias = "ServicePriorityName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceStatus", Name = "Code", Alias = "ServiceStatusCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceStatus", Name = "Name", Alias = "ServiceStatusName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceAction", Name = "Code", Alias = "ServiceActionCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "ServiceAction", Name = "Name", Alias = "ServiceActionName" });
            return columns;
        }

        public async Task<ServiceTemplateViewModel> GetFormIoData(string templateId, string serviceId, string userId)
        {

            var query = @$"select ""NT"".*,""TM"".""Name"" as ""TableName"" ,""lov"".""Code"" as ""ServiceStatusCode""
            ,""TM"".""Id"" as ""TableMetadataId"",""T"".""Json"" as ""Json""
            from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""TableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""ServiceTemplate"" as ""NT"" on ""T"".""Id""=""NT"".""TemplateId"" and ""NT"".""IsDeleted""=false 
            join ""public"".""NtsService"" as ""N"" on ""T"".""Id""=""N"".""TemplateId"" and ""N"".""IsDeleted""=false 
            join ""public"".""LOV"" as ""lov"" on ""N"".""ServiceStatusId""=""lov"".""Id"" and ""lov"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false   and ""N"".""Id""='{serviceId}'";

            var data = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.TableMetadataId
               && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
                data.AllReadOnly = true;
                await GetUdfDetails(data);
                var selectQuery = @$"select * from cms.""{data.TableName}"" where ""NtsServiceId""='{serviceId}' limit 1";
                var dr = await _queryRepo.ExecuteQueryDataRow(selectQuery, null);
                data.DataJson = dr.ToJson();
            }

            return data;
        }

        public async Task UpdateServiceWorkflowStatus(string serviceId, string WorkflowStatus, string taskId)
        {
            var service = await _repo.GetSingleById(serviceId);
            if (taskId.IsNotNullAndNotEmpty())
            {
                var task = await _repo.GetSingleById<TaskViewModel, NtsTask>(taskId);
                var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.TemplateId == task.TemplateId);
                service.WorkflowStatus = stepTaskComponent.WorkflowStatusName + " " + WorkflowStatus;
            }
            else
            {
                service.WorkflowStatus = WorkflowStatus;
            }

            await _repo.Edit(service);
        }
        public async Task<List<BookViewModel>> GetGroupBookByCategoryId(string categoryId)
        {
            var list = new List<BookViewModel>();

            var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and ps.""UdfNoteTableId"" ='{categoryId}'
             left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";


            list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            return list;
        }
        public async Task<List<BookViewModel>> GetAllBook(string templateCodes, string templateIds, string bookIds, string search, string categoryIds, string permission)
        {
            var list = new List<BookViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                if (_userContext.IsSystemAdmin || (permission != null && permission.Contains("CanViewAllBooks")))
                {
                    templateCodes = templateCodes.Replace(",", "','");
                    var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
                select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",mp.""BookId"" as ""BookId"" from cms.""N_SNC_TECProcess_ProcessItem"" as c
                 join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=c.""Id""  and mp.""IsDeleted"" = 'false'
                   group by mp.""BookId""  
              
            ) as Item on ""BookId""=bk.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
            order by ""LastUpdatedDate"" desc ";
                    var where = "";
                    var categorywhere = "";
                    var groupwhere = "";
                    if (search.IsNotNullAndNotEmpty())
                    {
                        where = $@"and (lower(bk.""ProcessGroupName"") ILIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") ILIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""ProcessGroupDescription"") ILIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"")";
                    }
                    if (bookIds.IsNotNullAndNotEmpty())
                    {
                        bookIds = bookIds.Replace(",", "','");
                        categorywhere = $@" and bk.""Id"" in ('{bookIds}')";
                    }
                    if (categoryIds.IsNotNullAndNotEmpty())
                    {
                        categoryIds = categoryIds.Replace(",", "','");
                        groupwhere = $@" and ps.""UdfNoteTableId"" in ('{categoryIds}')";
                    }
                    query = query.Replace("#SEARCHWHERE#", where);
                    query = query.Replace("#CATEGORYWHERE#", categorywhere);
                    query = query.Replace("#GROUPWHERE#", groupwhere);
                    list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
                }
                else
                {
                    templateCodes = templateCodes.Replace(",", "','");
                    var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
union

select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
           join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
union
select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
union
select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
order by ""LastUpdatedDate"" desc 

";
                    var where = "";
                    var categorywhere = "";
                    var groupwhere = "";
                    if (search.IsNotNullAndNotEmpty())
                    {
                        where = $@"and (lower(bk.""ProcessGroupName"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""ProcessGroupDescription"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"")";
                    }
                    if (bookIds.IsNotNullAndNotEmpty())
                    {
                        bookIds = bookIds.Replace(",", "','");
                        categorywhere = $@" and bk.""Id"" in ('{bookIds}')";
                    }
                    if (categoryIds.IsNotNullAndNotEmpty())
                    {
                        categoryIds = categoryIds.Replace(",", "','");
                        groupwhere = $@" and ps.""UdfNoteTableId"" in ('{categoryIds}')";
                    }
                    query = query.Replace("#SEARCHWHERE#", where);
                    query = query.Replace("#CATEGORYWHERE#", categorywhere);
                    query = query.Replace("#GROUPWHERE#", groupwhere);
                    list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
                }
            }
            else if (templateIds.IsNotNullAndNotEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Id"" in ('{templateIds}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
             left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'           
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Id"" in ('{templateIds}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by ""LastUpdatedDate"" desc";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else if (bookIds.IsNotNullAndNotEmpty())
            {
                bookIds = bookIds.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" ='false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'  and ps.""CompanyId""='{_userContext.CompanyId}'
             join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
               left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            where s.""Id"" in ('{bookIds}') and c.""IsDeleted"" = 'false'
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where s.""Id"" in ('{bookIds}') and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by ""LastUpdatedDate"" desc";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else if (search.IsNotNullAndNotEmpty())
            {

                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'  and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bk on s.""UdfNoteId""=bk.""NtsNoteId"" and bk.""IsDeleted"" = 'false' 
            where (lower(bk.""ProcessGroupName"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"")  and c.""IsDeleted"" = 'false'
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where (lower(bk.""ProcessGroupName"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"") and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by ""LastUpdatedDate"" desc";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }




            return list;
        }

        public async Task<List<IdNameViewModel>> GetProcessBookType(string templateCodes)
        {
            var list = new List<IdNameViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""CategoryName"" as Name,bk.""Id"" as Id
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

                list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            }


            return list;
        }
        public async Task<List<IdNameViewModel>> GetProcessBook(string templateCodes)
        {
            var list = new List<IdNameViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as Name,bk.""Id"" as Id
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

                list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            }


            return list;
        }
        public async Task<List<IdNameViewModel>> GetBookAllPages(string bookid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bk.""ItemName"" as Name,bk.""Id"" as Id, s.""Id"" as Code, mp.""SequenceOrder"" as Count
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            --join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by mp.""SequenceOrder"" ";

            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);


            return list;
        }
        public async Task<List<IdNameViewModel>> GetBookAllDirectPages(string bookid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bk.""ItemName"" as Name,bk.""Id"" as Id, s.""Id"" as Code
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk  
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId""  and bg.""Id""='{bookid}'  and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""IsDeleted"" = 'false'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";

            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);


            return list;
        }

        public async Task<List<IdNameViewModel>> GetChildPageList(string bookid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bk.""ItemName"" as Name,bk.""Id"" as Id, s.""Id"" as Code, mp.""SequenceOrder"" as Count
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
             join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by s.""SequenceOrder"" ";
            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }

        public async Task<List<IdNameViewModel>> GetBookByPageIdPages(string pageid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bg.""ProcessGroupName"" as Name,bg.""Id"" as Id, s.""Id"" as Code, mp.""SequenceOrder"" as Count
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""PageId""='{pageid}' and mp.""IsDeleted"" = 'false'
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by mp.""SequenceOrder"" ";

            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);


            return list;
        }
        public async Task<BookViewModel> GetBookDetail(string bookid)
        {

            var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName, c.""Id""
            as CategoryId,bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'  and ps.""CompanyId""='{_userContext.CompanyId}'
			             join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'

             left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
                select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",mp.""BookId"" as ""BookId"" from cms.""N_SNC_TECProcess_ProcessItem"" as c
                 join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=c.""Id""  and mp.""IsDeleted"" = 'false'
                   group by mp.""BookId""  
              
            ) as Item on ""BookId""=bk.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""Id""='{bookid}' and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

            var data = await _queryRepo.ExecuteQuerySingle<BookViewModel>(query, null);

            return data;

            //var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            //bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo
            //,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate""
            //,coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            //FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            //join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" ='false' and s.""CompanyId""='{_userContext.CompanyId}'
            //join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'  and ps.""CompanyId""='{_userContext.CompanyId}'
            // join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            //left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
            //   left join (select count(c.*) as Count,s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            //join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            //where s.""Id""  = '{bookid}' and c.""IsDeleted"" = 'false'
            //) as Item on ""ServiceId""=s.""Id""
            //left join (
            //    select avg(""Rating"") as ""Rating"",""NtsId"" as ""NtsId"" from public.""NtsRating""
            //    where ""IsDeleted"" = false group by ""NtsId""
            //) as r on ""NtsId""=s.""Id""
            //left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}'
            //where s.""Id""  = '{bookid}' and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

            //var details = await _queryRepo.ExecuteQuerySingle<BookViewModel>(query, null);
            //return details;
        }
        public async Task<BookViewModel> GetBookPageDetail(string bookid, string currentPageId)
        {
            var data = new BookViewModel();
            var query = @$"Select bk.""File"" as ""File"",bk.""ContentType"" as ""ContentType"",bk.""HtmlContent"" as HtmlContent
            ,bk.""HtmlCssField"" as HtmlCssField, bk.""ItemName""  as PageName, bk.""ItemDescription"" as PageDescription 
            ,mp.""SequenceOrder"" as SequenceOrder,bk.""Id"" as ""Id"", s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
             join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'
            
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            -- join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

            var list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);

            if (currentPageId.IsNullOrEmpty() || currentPageId == "undefined")
            {
                data.CurrentId = list.Where(x => x.SequenceOrder == 1).Select(x => x.Id).FirstOrDefault();
                data.NextId = list.Where(x => x.SequenceOrder == 2).Select(x => x.Id).FirstOrDefault();
            }
            else
            {
                data.CurrentId = currentPageId;
                var seq = list.Where(x => x.Id == currentPageId).Select(x => x.SequenceOrder).FirstOrDefault();
                var next = seq + 1;
                var prev = seq - 1;
                data.PreviousId = list.Where(x => x.SequenceOrder == prev).Select(x => x.Id).FirstOrDefault();
                data.NextId = list.Where(x => x.SequenceOrder == next).Select(x => x.Id).FirstOrDefault();
            }
            if (data.CurrentId.IsNotNullAndNotEmpty())
            {
                var Page = list.Where(x => x.Id == data.CurrentId).FirstOrDefault();
                if (Page != null)
                {
                    data.HtmlContent = Page.HtmlContent;
                    data.HtmlCssField = Page.HtmlCssField;
                    data.PageName = Page.PageName;
                    data.PageDescription = Page.PageDescription;
                    data.ServiceId = Page.ServiceId;
                    data.SequenceOrder = Page.SequenceOrder;
                    data.File = Page.File;
                    data.ContentType = Page.ContentType;
                    data.Id = Page.Id;
                }
            }
            else
            {
                var item = new BookViewModel
                {
                    PageName = "",
                    PageDescription = "",
                    SequenceOrder = 0,
                    HtmlContent = "",
                    HtmlCssField = ""
                };
                if (list != null && list.Count != 0)
                {
                    item = list.FirstOrDefault();

                    data.PageName = item.PageName;
                    data.PageDescription = item.PageDescription;
                    data.ServiceId = item.ServiceId;
                    data.SequenceOrder = item.SequenceOrder;
                    data.HtmlContent = item.HtmlContent;
                    data.HtmlCssField = item.HtmlCssField;
                    data.File = item.File;
                    data.ContentType = item.ContentType;
                    data.Id = item.Id;
                }
            }

            return data;
        }
        public async Task<bool> ValidateProjectName(ServiceTemplateViewModel viewModel)
        {
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            //var serId = Convert.ToString(rowData.GetValueOrDefault("Id"));
            //var projName = Convert.ToString(rowData.GetValueOrDefault("ServiceSubject"));
            //var tempCode = Convert.ToString(rowData.GetValueOrDefault("TemplateCode"));
            var list = await _repo.GetList(x => x.TemplateCode == viewModel.TemplateCode && x.Id != viewModel.ServiceId && x.ServiceSubject.ToLower() == viewModel.ServiceSubject.ToLower());

            if (list.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<List<TreeViewViewModel>> GetBookTreeList(string id, string templateCodes, string permission)
        {
            var _ntsBusiness = _serviceProvider.GetService<INtsBusiness>();
            //await _ntsBusiness.SendNotificationForRentServices();
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                var data = await GetAllCategoryByPermission(templateCodes, permission);
                foreach (var dr in data)
                {

                    var childs = await GetGroupBookByCategoryId(dr.Id);
                    list.Add(new TreeViewViewModel()
                    {
                        id = dr.Id,
                        DisplayName = dr.CategoryName,
                        ParentId = "#",
                        hasChildren = true,
                        Type = "Category",
                        Name = dr.CategoryName,
                        StageId = dr.ServiceId,
                        parent = "#",
                        Count = childs.Count(),

                    });
                }

                //list = data.Select(x => new TreeViewViewModel
                //{
                //    id = x.Id,
                //    DisplayName = x.CategoryName,
                //    ParentId = "#",
                //    hasChildren = true,
                //    Type = "Category",
                //    Name = x.CategoryName,
                //    StageId = x.ServiceId,
                //    parent = "#",
                //    a_attr = new
                //    {
                //        data_id = x.Id,
                //        data_name = x.CategoryName,
                //        data_type = "Category"
                //    }
                //}).ToList();

            }

            else
            {
                var data = await GetGroupBookByCategoryPermission(id, permission);


                if (data.IsNotNull() && data.Count() > 0)
                {

                    //list = (from dr in data
                    //        select new TreeViewViewModel()
                    //        {
                    //            id = dr.Id,
                    //            DisplayName = dr.BookName,
                    //            ParentId = dr.ParentServiceId,
                    //            hasChildren = false,
                    //            Type = "Group",
                    //            StageId = dr.ServiceId,
                    //            Name = dr.BookName,
                    //            parent = id,
                    //            SequenceOrder = dr.SequenceOrder,
                    //        }).OrderBy(x => x.SequenceOrder).ToList();
                    foreach (var dr in data)
                    {
                        var pagesdata = await GetBookAllPagesByBookId(dr.Id);
                        list.Add(new TreeViewViewModel()
                        {
                            id = dr.Id,
                            DisplayName = dr.BookName,
                            ParentId = dr.ParentServiceId,
                            hasChildren = false,
                            Type = "Group",
                            StageId = dr.ServiceId,
                            Name = dr.BookName,
                            parent = id,
                            Count = pagesdata.Count(),
                            SequenceOrder = dr.SequenceOrder,
                        });
                    }

                }
                else
                {
                    // get all pages by group id
                    var pagesdata = await GetBookAllPagesByBookId(id);
                    list = (from dr in pagesdata
                            select new TreeViewViewModel()
                            {
                                id = dr.Id,
                                DisplayName = dr.BookName,
                                ParentId = dr.ParentServiceId,
                                hasChildren = false,
                                Type = "Page",
                                StageId = dr.ServiceId,
                                Name = dr.BookName,
                                parent = id,
                                SequenceOrder = dr.SequenceOrder,
                            }).OrderBy(x => x.SequenceOrder).ToList();
                }
            }

            return list.OrderBy(x => x.SequenceOrder).ToList();
        }
        public async Task<List<BookViewModel>> GetAllCategoryByPermission(string templateCodes, string permission)
        {
            var list = new List<BookViewModel>();
            if (_userContext.IsSystemAdmin || (permission != null && permission.Contains("CanViewAllBooks")))
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            and tt.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else
            {
                var query = @$"select bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select distinct bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as cs on cs.""ParentServiceId""=s.""Id"" and cs.""IsDeleted"" = 'false' and cs.""CompanyId""='{_userContext.CompanyId}'            
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=cs.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
             select distinct bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
             join  public.""NtsService"" as cs on cs.""ParentServiceId""=s.""Id"" and cs.""IsDeleted"" = 'false' and cs.""CompanyId""='{_userContext.CompanyId}'   
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=cs.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
                

";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }


            return list;
        }
        public async Task<List<BookViewModel>> GetGroupBookByCategoryPermission(string categoryId, string permission)
        {
            var list = new List<BookViewModel>();

            if (_userContext.IsSystemAdmin || (permission != null && permission.Contains("CanViewAllBooks")))
            {
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
              FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'
          
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else
            {
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
             FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
              FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select distinct bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'          
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
             select distinct bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
             FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'   
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
                

            ";


                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            return list;
        }
        public async Task<List<BookViewModel>> GetBookAllPagesByBookId(string bookid)
        {
            var list = new List<BookViewModel>();
            var query = @$"select bk.""ItemName"" as BookName,bk.""ItemDescription"" as BookDescription,bk.""Id"" as Id, s.""Id"" as ServiceId, mp.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId, bk.""HtmlContent"" as HtmlContent,bk.""HtmlCssField"" as HtmlCssField
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'            
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            --join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by mp.""SequenceOrder"" ";

            list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);


            return list;
        }
        public async Task<List<BookViewModel>> GetAllBookPages()
        {
            var list = new List<BookViewModel>();
            var query = @$"select bk.""ItemName"" as PageName,bg.""ProcessGroupName"" as BookName,bk.""Id"" as Id, s.""Id"" as ServiceId, s.""SequenceOrder"" as SequenceOrder,ps.""ParentServiceId"" as ParentServiceId,c.""CategoryName"" as CategoryName
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""IsDeleted"" = 'false'        
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on mp.""BookId""=bg.""Id"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as gs on gs.""Id""=bg.""UdfNoteTableId"" and gs.""IsDeleted"" = 'false'
            join  public.""NtsService"" as pgs on pgs.""Id""=gs.""ParentServiceId"" and pgs.""IsDeleted"" = 'false'
            join  cms.""N_SNC_TECProcess_ProcessCategory"" as c on c.""NtsNoteId""=pgs.""UdfNoteId"" and c.""IsDeleted"" = 'false'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by s.""SequenceOrder"" ";

            list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);


            return list;
        }
        public async Task<List<BookViewModel>> GetAllProcessBook(string templateCodes)
        {
            var list = new List<BookViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,c.""CategoryName"" as CategoryName
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join  cms.""N_SNC_TECProcess_ProcessCategory"" as c on c.""NtsNoteId""=ps.""UdfNoteId"" and c.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }


            return list;
        }

        public async Task<List<BookRealtionViewModel>> GetAllBookRelationBySourceId(string sourceId)
        {
            var list = new List<BookRealtionViewModel>();
            var query = @$"select *
            FROM cms.""N_SNC_TECProcess_BookRelation"" as bk
           
where bk.""SourceTableId""='{sourceId}' and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'  ";

            list = await _queryRepo.ExecuteQueryList<BookRealtionViewModel>(query, null);


            return list;
        }

        public async Task CreateBookPageMapping(string pageId, string bookId, long? sequenceOrder)
        {
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
            dynamic exo = new System.Dynamic.ExpandoObject();
            ((IDictionary<String, Object>)exo).Add("PageId", pageId);
            ((IDictionary<String, Object>)exo).Add("BookId", bookId);
            ((IDictionary<String, Object>)exo).Add("SequenceOrder", sequenceOrder);
            var Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            var create = await _cmsBusiness.CreateForm(Json, "", "BookPageMapping");

        }
        public async Task DeleteBokPageMapping(string bookId, string pageId)
        {
            var query = $@"Update cms.""F_TECProcess_BookPageMapping"" set ""IsDeleted""=true where ""BookId""='{bookId}' and ""PageId""='{pageId}'";
            await _queryTaskRepo.ExecuteCommand(query, null);
        }


    }
}
