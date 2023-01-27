using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Humanizer;
////using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using Hangfire;

namespace Synergy.App.Business
{
    public class TaskBusiness : BusinessBase<TaskViewModel, NtsTask>, ITaskBusiness
    {
        private readonly IRepositoryQueryBase<TaskViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryChart;
        private readonly INtsTaskSharedBusiness _ntsTaskSharedBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly INoteBusiness _noteBusiness;
        // private readonly IUserBusiness _userBusiness;
        // private readonly IPushNotificationBusiness _notificationBusiness;
        private readonly IRepositoryQueryBase<NtsLogViewModel> _queryNtsLog;
        private readonly IUserContext _userContext;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryCal;
        private readonly ITeamBusiness _teamBusiness;
        private readonly IStepTaskComponentBusiness _stepCompBusiness;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public TaskBusiness(IRepositoryBase<TaskViewModel, NtsTask> repo, IMapper autoMapper, IRepositoryQueryBase<TaskViewModel> queryRepo,
             IRepositoryQueryBase<ProjectGanttTaskViewModel> queryChart, INtsTaskSharedBusiness ntsTaskSharedBusiness,
            //IPushNotificationBusiness notificationBusiness,
            IUserContext userContext, IRepositoryQueryBase<DashboardCalendarViewModel> queryCal,
            IServiceProvider serviceProvider, ILOVBusiness lOVBusiness
            , INoteBusiness noteBusiness, IRepositoryQueryBase<NtsLogViewModel> queryNtsLog
            , IComponentResultBusiness componentResultBusiness
            , ITeamBusiness teamBusiness, IStepTaskComponentBusiness stepCompBusiness, INtsQueryBusiness ntsQueryBusiness
            // , IHangfireScheduler hangfireScheduler
            ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsTaskSharedBusiness = ntsTaskSharedBusiness;
            //  _notificationBusiness = notificationBusiness;
            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _componentResultBusiness = componentResultBusiness;
            _serviceProvider = serviceProvider;
            _queryChart = queryChart;
            _queryNtsLog = queryNtsLog;
            _queryCal = queryCal;
            _lOVBusiness = lOVBusiness;
            _teamBusiness = teamBusiness;
            _stepCompBusiness = stepCompBusiness;
            //_userBusiness = userBusiness;
            //_userPortalBusiness = userPortalBusiness;
            _ntsQueryBusiness = ntsQueryBusiness;
            //_hangfireScheduler = hangfireScheduler;
        }
        public async Task<CommandResult<TaskTemplateViewModel>> DeleteTask(TaskTemplateViewModel model)
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
            return CommandResult<TaskTemplateViewModel>.Instance(model);
        }
        private async Task<CommandResult<TaskTemplateViewModel>> ManagePresubmit(TaskTemplateViewModel model)
        {
            dynamic dobject = new { };
            if (model.Json.IsNotNullAndNotEmptyAndNotValue("{}"))
            {
                dobject = model.Json.JsonToDynamicObject();
            }
            try
            {
                var result = await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PreSubmit, model.TaskStatusCode, model, dobject);
                return result;
            }
            catch (Exception ex)
            {
                return CommandResult<TaskTemplateViewModel>.Instance(model, false, $"Error in pre submit business rule execution: {ex.Message}");
            }

        }
        public async Task<CommandResult<TaskTemplateViewModel>> ManageTask(TaskTemplateViewModel model)
        {
            CommandResult<TaskTemplateViewModel> result = null;
            model.TemplateViewModel = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
            model.TaskTemplateVM = await _repo.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == model.TemplateId);
            if (model.AssignedToTeamUserId.IsNotNullAndNotEmpty())
            {
                model.AssignedToUserId = model.AssignedToTeamUserId;
            }
            var existingItem = await _repo.GetSingleById(model.TaskId);
            if (model.AssignedToTypeCode == "DIRECT_EMAIL")
            {
                // check if user email exist
                IUserBusiness _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                IUserPortalBusiness _userPortalBusiness = _serviceProvider.GetService<IUserPortalBusiness>();
                var exisitingUser = await _userBusiness.GetSingle(x => x.Email == model.AssignedToEmail);
                if (exisitingUser != null)
                {
                    model.AssignedToUserId = exisitingUser.Id;
                    // check if user has access to survey portal
                    var allowedPortals = await _userPortalBusiness.GetList(x => x.UserId == exisitingUser.Id && x.PortalId == _userContext.PortalId);
                    if (!allowedPortals.Any(x => x.PortalId == _userContext.PortalId))
                    {
                        var res1 = await _userPortalBusiness.Create(new UserPortalViewModel
                        {
                            UserId = exisitingUser.Id,
                            PortalId = _userContext.PortalId,
                        });
                    }
                }
                else
                {
                    // Create new User
                    var UserModel = new UserViewModel();
                    UserModel.Name = model.AssignedToEmail;
                    UserModel.Email = model.AssignedToEmail;
                    UserModel.EnableRegularEmail = true;
                    UserModel.SendWelcomeEmail = true;
                    var result1 = await _userBusiness.Create(UserModel);
                    if (result1.IsSuccess)
                    {
                        var res1 = await _userPortalBusiness.Create(new UserPortalViewModel
                        {
                            UserId = result1.Item.Id,
                            PortalId = _userContext.PortalId,
                        });
                        model.AssignedToUserId = result1.Item.Id;
                    }
                }
                model.AssignedToTypeCode = "TASK_ASSIGN_TO_USER";
            }

            //var tableMetaBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
            //DataRow existingItemTableData = null;
            //NoteViewModel existingNote = null;
            //if (existingItem != null)
            //{
            //    existingItemTableData = await tableMetaBusiness.GetTableDataByHeaderId(existingItem.TemplateId, model.UdfNoteId);
            //    existingNote = await _repo.GetSingleById<NoteViewModel, NtsNote>(model.UdfNoteId);
            //}
            if (model.TaskTemplateVM.IsNotNull() && model.TaskTemplateVM.SubjectUdfMappingColumn.IsNotNullAndNotEmpty())
            {
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Json);
                if (rowData.IsNotNull() && rowData.ContainsKey(model.TaskTemplateVM.SubjectUdfMappingColumn))
                {
                    model.TaskSubject = Convert.ToString(rowData[model.TaskTemplateVM.SubjectUdfMappingColumn]);
                }

            }
            if (model.TaskTemplateVM.IsNotNull() && model.TaskTemplateVM.DescriptionUdfMappingColumn.IsNotNullAndNotEmpty())
            {
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Json);
                if (rowData.IsNotNull() && rowData.ContainsKey(model.TaskTemplateVM.DescriptionUdfMappingColumn))
                {
                    model.TaskDescription = Convert.ToString(rowData[model.TaskTemplateVM.DescriptionUdfMappingColumn]);
                }

            }
            var ntsevent = new LOVViewModel();

            switch (model.TaskStatusCode)
            {
                case "TASK_STATUS_DRAFT":
                    if (model.TaskId.IsNullOrEmpty())
                    {
                        model.TaskId = Guid.NewGuid().ToString();
                    }
                    if (model.DueDate != null && model.StartDate != null)
                    {
                        model.TaskSLA = model.DueDate.Value.Subtract(model.StartDate.Value);
                    }
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_DRAFT");
                    model.TaskEventId = ntsevent.Id;
                    result = await SaveAsDraft(model);
                    break;
                case "TASK_STATUS_INPROGRESS":
                case "TASK_STATUS_PLANNED":
                    if (model.TaskId.IsNullOrEmpty())
                    {
                        model.TaskId = Guid.NewGuid().ToString();
                    }
                    if (model.DueDate != null && model.StartDate != null)
                    {
                        model.TaskSLA = model.DueDate.Value.Subtract(model.StartDate.Value);
                    }
                    if (model.VersionNo > 1)
                    {
                        ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CREATED_AS_NEWVERSION");
                    }
                    else
                    {
                        ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CREATED");
                    }
                    model.TaskEventId = ntsevent.Id;
                    result = await Submit(model, model.TaskStatusCode);
                    if (result.Item.TaskTemplateType == TaskTypeEnum.StepTask)
                    {
                        await UpdateServiceWorkflowStatusForTask(result.Item.ParentServiceId, "TASK_STATUS_INPROGRESS", result.Item.TaskId);
                    }
                    break;
                case "TASK_STATUS_COMPLETE":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_COMPLETED");
                    model.TaskEventId = ntsevent.Id;
                    result = await CompleteTask(model);
                    break;
                case "TASK_STATUS_SAVECHAGES":
                    //ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_COMPLETED");
                    //model.TaskEventId = ntsevent.Id;
                    result = await SaveChanges(model);
                    break;
                case "TASK_STATUS_CANCEL":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CANCELLED");
                    model.TaskEventId = ntsevent.Id;
                    result = await CancelTask(model);
                    //if (result.Item.TaskTemplateType == TaskTypeEnum.StepTask)
                    //{
                    //    var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
                    //    await _serviceBusiness.UpdateServiceWorkflowStatus(result.Item.ParentServiceId, "Canceled by", result.Item.TaskId);
                    //}
                    break;

                case "TASK_STATUS_OVERDUE":
                    result = await OverdueTask(model);
                    if (result.Item.TaskTemplateType == TaskTypeEnum.StepTask)
                    {

                        await UpdateServiceWorkflowStatusForTask(result.Item.ParentServiceId, "TASK_STATUS_OVERDUE", result.Item.TaskId);
                    }
                    break;
                case "TASK_STATUS_REJECT":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_REJECT");
                    model.TaskEventId = ntsevent.Id;
                    result = await RejectTask(model);
                    if (result.Item.TaskTemplateType == TaskTypeEnum.StepTask)
                    {

                        await UpdateServiceWorkflowStatusForTask(result.Item.ParentServiceId, "TASK_STATUS_REJECT", result.Item.TaskId);
                    }
                    break;
                default:
                    return await Task.FromResult(CommandResult<TaskTemplateViewModel>.Instance(model, false, "Inavlid Task Action"));
            }
            if (result != null && result.IsSuccess /*&& model.TaskStatusCode != "TASK_STATUS_DRAFT"*/)
            {
                var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.UdfTableMetadataId);
                var selectQuery = await GetSelectQuery(tableMetaData, @$" and ""NtsTask"".""Id""='{model.TaskId}' limit 1");
                dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                try
                {
                    await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model.TaskStatusCode, model, dobject);
                }
                catch (Exception ex)
                {
                    return CommandResult<TaskTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.Message}");
                }
                //try
                //{
                //    result = await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PreSubmit, model.TaskStatusCode, model, dobject);

                //}
                //catch (Exception ex)
                //{
                //    await RollBackData(existingItem, existingNote, existingItemTableData, model);
                //    return CommandResult<TaskTemplateViewModel>.Instance(model, false, $"Error in pre submit business rule execution: {ex.Message}");

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
                //        await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model.TaskStatusCode, model, dobject);
                //    }
                //    catch (Exception ex)
                //    {
                //        return CommandResult<TaskTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.Message}");
                //    }

                //}
                if (result.IsSuccess)
                {
                    var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                    await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.ManageStepTaskComponent(result.Item, _repo.UserContext.ToIdentityUser()));
                    //await _componentResultBusiness.ManageStepTaskComponent(result.Item);

                    if (existingItem == null || model.TaskStatusId != existingItem.TaskStatusId)
                    {
                        await ManageNotification(result.Item);
                    }
                    try
                    {
                        await UpdateNotificationStatus(model.TaskId, model.TaskStatusCode);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return result;
        }
        private async Task<CommandResult<TaskTemplateViewModel>> CancelTask(TaskTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetTaskViewModelStatus(viewModel, "TASK_STATUS_CANCEL");
            var dm = await _repo.GetSingleById(viewModel.TaskId);
            dm.TaskStatusId = viewModel.TaskStatusId;
            dm.TaskEventId = viewModel.TaskEventId;
            dm.CanceledDate = DateTime.Now;
            dm.CancelReason = viewModel.CancelReason;
            dm.IsReturned = viewModel.IsReturned;
            if (viewModel.IsReturned)
            {
                dm.IsReopened = false;
            }
            if (dm.ActualStartDate.IsNotNull())
            {
                dm.ActualSLA = dm.CanceledDate.Value.Subtract(dm.ActualStartDate.Value);
            }
            var result = await _repo.Edit(dm);
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<TaskTemplateViewModel>> OverdueTask(TaskTemplateViewModel viewModel)
        {
            //var presubmit = await ManagePresubmit(viewModel);
            //if (!presubmit.IsSuccess)
            //{
            //    return presubmit;
            //}
            await SetTaskViewModelStatus(viewModel, "TASK_STATUS_OVERDUE");
            var dm = await _repo.GetSingleById(viewModel.TaskId);
            dm.TaskStatusId = viewModel.TaskStatusId;
            var result = await _repo.Edit(dm);
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        private async Task RollBackData(TaskViewModel existingItem, NoteViewModel existingNote, DataRow existingItemTableData, TaskTemplateViewModel model)
        {
            if (existingItem == null)
            {
                var table = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.UdfTableMetadataId);
                if (table != null)
                {
                    await _ntsQueryBusiness.RollBackTaskData(model, table);


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
                await _repo.Edit<TaskViewModel, NtsTask>(existingItem);
            }
        }

        public async Task ManageNotification(TaskTemplateViewModel viewModel)
        {
            var returnstatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_TASK_STATUS" && x.Code == "TASK_STATUS_RETURN");
            var reopenstatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_TASK_STATUS" && x.Code == "TASK_STATUS_REOPEN");
            if (returnstatus != null && viewModel.IsReturned && (viewModel.TaskStatusCode == "TASK_STATUS_INPROGRESS" || viewModel.TaskStatusCode == "TASK_STATUS_CANCEL"))
            {
                viewModel.TaskStatusId = returnstatus.Id;
            }
            else if (reopenstatus != null && viewModel.IsReopened && viewModel.TaskStatusCode == "TASK_STATUS_INPROGRESS")
            {
                viewModel.TaskStatusId = reopenstatus.Id;
            }
            var notificationTemplates = await _repo.GetList<NotificationTemplate, NotificationTemplate>(x => x.TemplateId == viewModel.TemplateId || (x.NtsType == NtsTypeEnum.Task && x.AutoApplyOnAllTemplates), x => x.ParentNotificationTemplate);
            notificationTemplates = notificationTemplates.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            foreach (var item in notificationTemplates)
            {
                var template = item;
                if (item.ParentNotificationTemplate != null)
                {
                    template = item.ParentNotificationTemplate;
                }
                if (template.NotificationActionId == viewModel.TaskStatusId)
                {
                    await CreateNotification(viewModel, template);
                }
            }
        }

        private async Task CreateNotification(TaskTemplateViewModel viewModel, NotificationTemplate item)
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
                    await SendNotification(viewModel, item, viewModel.AssignedToUserId);
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
        public async Task SendNotification(TaskTemplateViewModel viewModel, NotificationTemplate item, string toUserId)
        {
            if (toUserId != null)
            {
                var notification = new NotificationViewModel
                {
                    DataAction = DataActionEnum.Create,
                    FromUserId = viewModel.OwnerUserId,
                    ToUserId = toUserId,
                    ReferenceType = ReferenceTypeEnum.NTS_Task,
                    ReferenceTypeId = viewModel.TaskId,
                    ReferenceTypeNo = viewModel.TaskNo,
                    NotifyByEmail = item.NotifyByEmail,
                    NotifyBySms = item.NotifyBySms,
                    DynamicObject = viewModel,
                    Subject = item.Subject,
                    Body = item.Body,
                    PortalId = _repo.UserContext.PortalId,
                    Url = SetNotificationUrl(viewModel, item, toUserId),
                    TemplateCode = viewModel.TemplateCode,
                    NotificationTemplateId = item.Id,
                    ActionStatus = NotificationActionStatusEnum.NoActionRequired,
                };
                if (item.NotificationTo == NtsActiveUserTypeEnum.Assignee)
                {
                    var compService = _serviceProvider.GetService<IComponentBusiness>();
                    var stepTaskComponent = await _stepCompBusiness.GetSingleById(viewModel.StepTaskComponentId);
                    var parentComponent = await compService.GetComponentParent(stepTaskComponent?.ComponentId);
                    if (parentComponent == null || !parentComponent.Any())
                    {
                        notification.TriggeredByReferenceType = ReferenceTypeEnum.NTS_Service;
                        notification.TriggeredByReferenceTypeId = viewModel.ParentServiceId;
                    }
                    else
                    {
                        var parent = parentComponent.FirstOrDefault();
                        var compResult = await _componentResultBusiness.GetSingle(x => x.ComponentId == parent.Id);
                        if (compResult != null && compResult.NtsTaskId.IsNotNullAndNotEmpty())
                        {
                            notification.TriggeredByReferenceType = ReferenceTypeEnum.NTS_Task;
                            notification.TriggeredByReferenceTypeId = compResult.NtsTaskId;
                        }

                    }

                    //notification.TriggeredByReferenceType=
                }
                if (item.ActionType == NotificationActionTypeEnum.Action)
                {
                    notification.ActionStatus = NotificationActionStatusEnum.Pending;
                }
                var notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
                await notificationBusiness.Create(notification);
            }
        }
        public async Task UpdateNotificationStatus(string taskId, string status)
        {
            var notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
            var list = await notificationBusiness.GetList(x => x.ReferenceType == ReferenceTypeEnum.NTS_Task && x.ReferenceTypeId == taskId);
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
        private string SetNotificationUrl(TaskTemplateViewModel viewModel, NotificationTemplate item, string toUserId)
        {
            var customurl = HttpUtility.UrlEncode($@"taskId={viewModel.TaskId}");
            var url = $@"pageName=TaskHome&portalId={_repo.UserContext.PortalId}&pageType=Custom&customUrl={customurl}";
            url = $@"Portal/{viewModel.PortalName}?pageUrl={HttpUtility.UrlEncode(url)}";
            return url;
        }

        private async Task<CommandResult<TaskTemplateViewModel>> ManageBusinessRule(BusinessLogicExecutionTypeEnum executionType, string status, TaskTemplateViewModel viewModel, dynamic viewModelDynamicObject)
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
                    var result = await businessRuleBusiness.ExecuteBusinessRule<TaskTemplateViewModel>(businessRule, TemplateTypeEnum.Service, viewModel, viewModelDynamicObject);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                lov = await _repo.GetSingle<LOVViewModel, LOV>(x => x.Code == "TASK_STATUS_ALL");
                action = lov?.Id;
                businessRule = await _repo.GetSingle<BusinessRuleViewModel, BusinessRule>(x => x.TemplateId == viewModel.TemplateId
                && x.ActionId == action && x.BusinessLogicExecutionType == executionType);
                if (businessRule != null)
                {
                    try
                    {
                        var businessRuleBusiness = _serviceProvider.GetService<IBusinessRuleBusiness>();
                        var result = await businessRuleBusiness.ExecuteBusinessRule<TaskTemplateViewModel>(businessRule, TemplateTypeEnum.Service, viewModel, viewModelDynamicObject);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);

        }

        private async Task<CommandResult<TaskTemplateViewModel>> Submit(TaskTemplateViewModel viewModel, string status)
        {
            var validate = await ValidateTask(viewModel);
            if (validate.Count > 0)
            {
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, validate);
            }
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            //_ntsValidator.ExecuteTaskPreExecutionScript(viewModel, errorList);
            return await SaveTask(viewModel, status);
        }
        private async Task<CommandResult<TaskTemplateViewModel>> CompleteTask(TaskTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetTaskViewModelStatus(viewModel, "TASK_STATUS_COMPLETE");
            var dm = await _repo.GetSingleById(viewModel.TaskId);
            dm.TaskStatusId = viewModel.TaskStatusId;
            dm.TaskEventId = viewModel.TaskEventId;
            dm.CompletedDate = DateTime.Now;
            dm.CompleteReason = viewModel.CompleteReason;
            if (dm.ActualStartDate.IsNotNull())
            {
                dm.ActualSLA = dm.CompletedDate.Value.Subtract(dm.ActualStartDate.Value);
            }
            var assignToType = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "TASK_ASSIGN_TO_TYPE" && x.Code == viewModel.NextTaskAssignedToTypeCode);
            if (assignToType != null)
            {
                dm.NextTaskAssignedToTypeId = assignToType.Id;
            }
            dm.NextTaskAssignedToUserId = viewModel.NextTaskAssignedToUserId;
            dm.NextTaskAssignedToTeamId = viewModel.NextTaskAssignedToTeamId;
            if (viewModel.NextTaskAssignedToTeamId.IsNotNullAndNotEmpty())
            {
                dm.NextTaskAssignedToUserId = viewModel.NextTaskAssignedToTeamUserId;
            }
            dm.NextTaskAssignedToHierarchyMasterId = viewModel.NextTaskAssignedToHierarchyMasterId;
            dm.NextTaskAssignedToHierarchyMasterLevelId = viewModel.NextTaskAssignedToHierarchyMasterLevelId;
            dm.NextTaskAttachmentId = viewModel.NextTaskAttachmentId;
            var result = await _repo.Edit(dm);
            return await ManageTaskUdfTable(viewModel, null);
        }
        private async Task<CommandResult<TaskTemplateViewModel>> SaveChanges(TaskTemplateViewModel viewModel)
        {
            //var presubmit = await ManagePresubmit(viewModel);
            //if (!presubmit.IsSuccess)
            //{
            //    return presubmit;
            //}

            //var result = await _repo.Edit(dm);
            return await ManageTaskUdfTable(viewModel, null);
        }
        private async Task<CommandResult<TaskTemplateViewModel>> RejectTask(TaskTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetTaskViewModelStatus(viewModel, "TASK_STATUS_REJECT");
            var dm = await _repo.GetSingleById(viewModel.TaskId);
            dm.TaskStatusId = viewModel.TaskStatusId;
            dm.TaskEventId = viewModel.TaskEventId;
            dm.RejectedDate = DateTime.Now;
            dm.RejectionReason = viewModel.RejectionReason;
            if (dm.ActualStartDate.IsNotNull())
            {
                dm.ActualSLA = dm.RejectedDate.Value.Subtract(dm.ActualStartDate.Value);
            }
            var result = await _repo.Edit(dm);
            //await EditTaskUdfTable(viewModel, viewModel.Json, viewModel.TaskTableId);
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        private async Task<CommandResult<TaskTemplateViewModel>> SaveAsDraft(TaskTemplateViewModel viewModel)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            var presubmit = await ManagePresubmit(viewModel);
            return await SaveTask(viewModel, "TASK_STATUS_DRAFT");
        }

        private async Task<CommandResult<TaskTemplateViewModel>> SaveTask(TaskTemplateViewModel viewModel, string status)
        {
            //await SetUdfDetails(viewModel);
            await SetTaskViewModelStatus(viewModel, status);
            CommandResult<TaskTemplateViewModel> result = null;
            var assignToType = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "TASK_ASSIGN_TO_TYPE" && x.Code == viewModel.AssignedToTypeCode);
            viewModel.AssignedToTypeId = assignToType?.Id;

            if (viewModel.DataAction == DataActionEnum.Create)
            {
                if (status == "TASK_STATUS_INPROGRESS" || status == "TASK_STATUS_PLANNED")
                {
                    if (viewModel.SubmittedDate == null)
                    {
                        viewModel.SubmittedDate = DateTime.Now;
                    }
                    if (status == "TASK_STATUS_INPROGRESS")
                    {
                        viewModel.ActualStartDate = DateTime.Now;
                    }
                }
                result = await CreateTask(viewModel);
                if (result.IsSuccess)
                {
                    viewModel.VersionNo = result.Item.VersionNo;
                }
                else
                {

                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
                }
            }
            else
            {
                if (status == "TASK_STATUS_INPROGRESS" || status == "TASK_STATUS_PLANNED")
                {
                    if (viewModel.SubmittedDate == null)
                    {
                        viewModel.SubmittedDate = DateTime.Now;
                    }
                }
                result = await EditTask(viewModel);

            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }



        public async Task<CommandResult<TaskTemplateViewModel>> CreateTask(TaskTemplateViewModel viewModel)
        {
            var task = _autoMapper.Map<TaskTemplateViewModel, TaskViewModel>(viewModel);
            task.Id = viewModel.TaskId;
            task.TaskTemplateId = viewModel.Id;
            task.TaskType = viewModel.TaskTemplateType;
            var result = await _repo.Create(task);
            return await ManageTaskUdfTable(viewModel, result);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> EditTask(TaskTemplateViewModel viewModel, bool editUdfTable = true)
        {
            var task = _autoMapper.Map<TaskTemplateViewModel, TaskViewModel>(viewModel);
            task.Id = viewModel.TaskId;
            task.TaskTemplateId = viewModel.Id;
            task.TaskType = viewModel.TaskTemplateType;
            var result = await _repo.Edit(task);
            if (editUdfTable)
            {
                return await ManageTaskUdfTable(viewModel, null);
            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public override async Task<CommandResult<TaskViewModel>> Edit(TaskViewModel viewModel, bool autoCommit = true)
        {
            var result = await _repo.Edit(viewModel);

            return CommandResult<TaskViewModel>.Instance(viewModel);
        }

        private async Task<Dictionary<string, string>> ValidateTask(TaskTemplateViewModel viewModel)
        {
            var errorList = new Dictionary<string, string>();

            if (viewModel.TaskTemplateVM.IsSubjectMandatory && viewModel.TaskSubject.IsNullOrEmpty())
            {
                errorList.Add("TaskSubject", string.Concat(viewModel.SubjectText.ToDefaultSubjectText(), " is required"));
            }
            if (viewModel.TaskTemplateVM.IsSubjectUnique && viewModel.TaskSubject.IsNullOrEmpty())
            {
                //var tasklist = await _repo.GetList(x => x.TemplateId == viewModel.TemplateId);
                bool isexist = await IsTaskSubjectUnique(viewModel.TemplateId, viewModel.TaskSubject, viewModel.TaskId); //tasklist.Any(x => x.TaskSubject.Equals(viewModel.TaskSubject, StringComparison.InvariantCultureIgnoreCase) && x.Id != viewModel.TaskId);
                if (isexist)
                {
                    errorList.Add("TaskSubject", string.Concat("The given ", viewModel.SubjectText.ToDefaultSubjectText(), " already exist. Please enter another ", viewModel.SubjectText.ToDefaultSubjectText()));
                }

            }
            if (viewModel.StartDate == null)
            {
                errorList.Add("StartDate", "Start Date is required");
            }
            if (!viewModel.AllowPastStartDate && viewModel.StartDate != null && viewModel.StartDate.Value.Date < DateTime.Today)
            {
                errorList.Add("StartDate", "Start Date should be greater than or equal to today's date");
            }
            if ((viewModel.StartDate != null && viewModel.DueDate != null) && (viewModel.StartDate.Value > viewModel.DueDate.Value))
            {
                errorList.Add("DueDate", "Start Date should be less than or equal to due date");
            }
            if (viewModel.TaskPriorityId.IsNullOrEmpty())
            {
                errorList.Add("TaskPriorityId", "Priority is required");
            }
            if (viewModel.AssignedToTypeCode.IsNullOrEmpty())
            {
                errorList.Add("AssignedToTypeId", "Assign To Type is required");
            }
            else
            {

                if (viewModel.AssignedToTypeCode == "TASK_ASSIGN_TO_USER")
                {
                    if (viewModel.AssignedToUserId.IsNullOrEmpty())
                    {
                        errorList.Add("AssignedToUserId", string.Concat(viewModel.AssignedToUserText.ToDefaultAssignedToUserText(), " is required"));
                    }
                }
                else if (viewModel.AssignedToTypeCode == "TASK_ASSIGN_TO_TEAM")
                {
                    if (viewModel.AssignedToTeamUserId.IsNullOrEmpty())
                    {
                        errorList.Add("AssignedToTeamUserId", "Assign To Team User is required");
                    }
                }
                else if (viewModel.AssignedToTypeCode == "TASK_ASSIGN_TO_USER_HIERARCHY")
                {
                    if (viewModel.AssignedToHierarchyMasterId.IsNullOrEmpty())
                    {
                        errorList.Add("AssignedToHierarchyMasterId", "Assign To Hierarchy Name is required");
                    }
                    if (viewModel.AssignedToHierarchyMasterLevelId == null)
                    {
                        errorList.Add("AssignedToHierarchyMasterLevelId", "Assign To Hierarchy Level is required");
                    }
                }
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
            //var uniqueColumns = tableColumns.Where(x => x.IsUniqueColumn).ToList();
            //foreach (var item in uniqueColumns)
            //{
            //    var value = rowData.GetValueOrDefault(item.Name);
            //    var exist = await GetDataByColumn(item, value, tableMetaData, exculdeId);
            //    if (exist.IsNotNullAndNotEmpty())
            //    {
            //        return await Task.FromResult(@$"An item already exists with '{item.Alias}' as '{value}'. Please enter another value");
            //    }
            //}
            return string.Empty;
        }

        private async Task<CommandResult<TaskTemplateViewModel>> ManageTaskUdfTable(TaskTemplateViewModel viewModel, TaskViewModel model = null)
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
                        noteUdf.NoteSubject = viewModel.TaskSubject;
                        noteUdf.NoteDescription = viewModel.TaskDescription;
                        noteUdf.StartDate = viewModel.StartDate;
                        noteUdf.AllowPastStartDate = viewModel.AllowPastStartDate;
                        noteUdf.NoteId = Guid.NewGuid().ToString();

                        noteUdf.TemplateId = viewModel.UdfTemplateId;
                        noteUdf.Json = viewModel.Json;
                        if (viewModel.TaskStatusCode == "TASK_STATUS_DRAFT")
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
                                await _repo.Edit(model);
                            }

                            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
                        }
                        else
                        {
                            var ret = CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, result.Message);
                            ret.Messages = result.Messages;
                            return ret;
                        }
                    }
                    else
                    {
                        noteUdf = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                        {
                            NoteId = viewModel.UdfNoteId,
                            RecordId = viewModel.UdfNoteId,
                            DataAction = DataActionEnum.Edit,
                            TemplateId = viewModel.UdfTemplateId,
                            ActiveUserId = viewModel.ActiveUserId,
                            UdfNoteTableId = viewModel.UdfNoteTableId

                        });
                        noteUdf.DataAction = DataActionEnum.Edit;
                        noteUdf.Json = viewModel.Json;
                        if (viewModel.TaskStatusCode == "TASK_STATUS_DRAFT")
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
                            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
                        }
                        else
                        {
                            var ret = CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, result.Message);
                            ret.Messages = result.Messages;
                            return ret;
                        }
                    }
                }
                else
                {
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "Udf Template not found");
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "Template not found");
        }

        private async Task SetTaskViewModelStatus(TaskTemplateViewModel viewModel, string statusCode = null)
        {
            statusCode = statusCode ?? viewModel.TaskStatusCode;
            var taskStatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_TASK_STATUS" && x.Code == statusCode);
            if (taskStatus != null)
            {
                viewModel.TaskStatusCode = taskStatus.Code;
                viewModel.TaskStatusId = taskStatus.Id;
            }
        }
        public async Task<TaskTemplateViewModel> GetTaskDetails(TaskTemplateViewModel viewModel)
        {
            var model = new TaskTemplateViewModel();
            if (viewModel.TaskId.IsNullOrEmpty())
            {
                model = await GetTaskTemplateForNewTask(viewModel);

                model.Prms = viewModel.Prms;
                model.Udfs = viewModel.Udfs;
                model.ReadoOnlyUdfs = viewModel.ReadoOnlyUdfs;
                model.TaskId = Guid.NewGuid().ToString();
                model.TaskNo = await GenerateNextTaskNo(model);
                model.ActiveUserId = viewModel.ActiveUserId;
                model.AssignedToUserId = viewModel.AssignedToUserId;
                model.OwnerUserId = viewModel.OwnerUserId;
                model.ParentServiceId = viewModel.ParentServiceId;
                model.ParentTaskId = viewModel.ParentTaskId;
                model.RequestedByUserId = viewModel.RequestedByUserId;

                var assignedToUserId = model.Prms.GetValue("assignedToUserId");
                var ownerUserId = model.Prms.GetValue("ownerUserId");
                var parentServiceId = model.Prms.GetValue("parentServiceId");
                var parentTaskId = model.Prms.GetValue("parentTaskId");
                var parentNoteId = model.Prms.GetValue("parentNoteId");
                var hideStepTaskDetails = model.Prms.GetValue("hideStepTaskDetails");
                var requestedByUserId = model.Prms.GetValue("requestedByUserId");
                var subject = model.Prms.GetValue("subject");
                var description = model.Prms.GetValue("description");
                var sequenceOrder = model.Prms.GetValue("sequenceOrder");
                var servicePlusId = model.Prms.GetValue("servicePlusId");
                var notePlusId = model.Prms.GetValue("notePlusId");
                if (notePlusId.IsNotNullAndNotEmpty())
                {
                    model.NotePlusId = notePlusId;
                }
                if (subject.IsNotNullAndNotEmpty())
                {
                    model.TaskSubject = subject;
                }
                if (description.IsNotNullAndNotEmpty())
                {
                    model.TaskDescription = description;
                }
                if (assignedToUserId.IsNotNullAndNotEmpty())
                {
                    model.AssignedToUserId = assignedToUserId;
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
                model.AssignedToTypeCode = "TASK_ASSIGN_TO_USER";
                if (model.TaskAssignedToTypeId.IsNotNullAndNotEmpty())
                {
                    var assignedToLOV = await _repo.GetSingleById<LOVViewModel, LOV>(model.TaskAssignedToTypeId);
                    if (assignedToLOV != null)
                    {
                        model.AssignedToTypeCode = assignedToLOV.Code;
                    }
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
                    model.TaskSLA = TimeSpan.FromDays(1);
                }
                else
                {
                    model.DueDate = model.StartDate.Value.Add(model.SLA.Value);
                    model.TaskSLA = model.SLA.Value;
                }
                model.TaskSLASeconds = model.TaskSLA.Value.TotalSeconds;
                if (model.ActiveUserId == model.RequestedByUserId && model.ActiveUserId == model.OwnerUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Owner;
                }
                else
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Requester;
                }
                if (model.AssignedToUserId.IsNullOrEmpty())
                {
                    model.AssignedToUserId = model.RequestedByUserId;
                    model.AssignedToUserName = model.RequestedByUserName;
                    model.AssignedToUserEmail = model.RequestedByUserEmail;
                }
                await GetUdfDetails(model);
            }
            else
            {
                model = await GetTaskTemplateById(viewModel);
                if (model.TaskSLA == null)
                {
                    if (model.StartDate.HasValue && model.DueDate.HasValue)
                    {
                        model.TaskSLA = model.DueDate - model.StartDate;
                    }
                }
                if (model.TaskSLA.HasValue)
                {
                    model.TaskSLASeconds = model.TaskSLA.Value.TotalSeconds;
                }
                model.ActualEndDate = model.CompletedDate ?? model.RejectedDate ?? model.CanceledDate;

                model.RecordId = viewModel.RecordId;
                model.ActiveUserId = viewModel.ActiveUserId;
                model.IncludeSharedList = viewModel.IncludeSharedList;
                model.Prms = viewModel.Prms;
                model.Udfs = viewModel.Udfs;
                model.ReadoOnlyUdfs = viewModel.ReadoOnlyUdfs;
                var hideStepTaskDetails = model.Prms.GetValue("hideStepTaskDetails");
                if (hideStepTaskDetails.IsNotNullAndNotEmpty())
                {
                    model.HideStepTaskDetails = hideStepTaskDetails.ToSafeBool();
                }
                if (model.IncludeSharedList)
                {
                    model.SharedList = await SetSharedList(model);
                }
                if (model.ActiveUserId == model.AssignedToUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Assignee;
                }
                else if (model.ActiveUserId == model.RequestedByUserId && model.ActiveUserId == model.OwnerUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Owner;
                }
                else if (model.ActiveUserId == model.RequestedByUserId && model.ActiveUserId != model.OwnerUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Requester;
                }
                else
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.None;
                }

                await GetUdfDetails(model);
                // Demo Code for the loading of parent Service
                //model.ParentServiceId = "aecc7f69-3301-4559-b5f0-3884495067f7";
                if (model.TaskTemplateType == TaskTypeEnum.StepTask || (model.TaskTemplateType == TaskTypeEnum.AdhocTask && model.ParentServiceId.IsNotNullAndNotEmpty()))
                {
                    if (model.ParentServiceId.IsNotNullAndNotEmpty())
                    {
                        var templateBusiness = (ITemplateBusiness)_serviceProvider.GetService(typeof(ITemplateBusiness));
                        var servicetemplateBusiness = (IServiceTemplateBusiness)_serviceProvider.GetService(typeof(IServiceTemplateBusiness));
                        var pageBusiness = (IPageBusiness)_serviceProvider.GetService(typeof(IPageBusiness));
                        model.ParentService = await _repo.GetSingleById<ServiceViewModel, NtsService>(model.ParentServiceId);
                        var Template = await templateBusiness.GetSingleById(model.ParentService.TemplateId);
                        model.ParentService.TemplateCode = Template.Code;
                        if (Template != null)
                        {
                            var st = await servicetemplateBusiness.GetSingle(x => x.TemplateId == Template.Id);
                            if (st != null)
                            {
                                model.EnableDynamicStepTaskSelection = st.EnableDynamicStepTaskSelection;
                                if (model.BannerFileId.IsNullOrEmpty() && st.BannerFileId.IsNotNullAndNotEmpty())
                                {
                                    model.BannerFileId = st.BannerFileId;
                                }
                            }
                            model.ServiceViewType = Template.ViewType;
                            model.ServiceBaseTemplate = Template;
                            var page = await pageBusiness.GetSingle(x => x.TemplateId == Template.Id);
                            var status = await pageBusiness.GetSingle<LOVViewModel, LOV>(x => x.Id == model.ParentService.ServiceStatusId);
                            if (page != null)
                            {
                                model.ParentService.PageName = page.Name;
                                model.ParentService.PageId = page.Id;
                            }
                            else
                            {

                            }
                            model.ParentService.ServiceStatusName = status.Name;
                            model.ParentService.ServiceStatusCode = status.Code;
                            if (model.ParentService.OwnerUserId.IsNotNullAndNotEmpty())
                            {
                                var owner = await _repo.GetSingleById<UserViewModel, User>(model.ParentService.OwnerUserId);
                                if (owner != null)
                                {
                                    model.ParentService.OwnerUserUserName = owner.Name;
                                }
                            }
                        }
                        if (model.TaskTemplateType == TaskTypeEnum.StepTask)
                        {
                            var TasksList = await _componentResultBusiness.GetStepTaskList(model.ParentServiceId);
                            model.StepTasksList = TasksList;
                        }


                        /*.Where(x => x.Id != model.TaskId).ToList()*/
                        //var TasksList = await _componentResultBusiness.GetList(x=>x.NtsServiceId== model.ParentServiceId);                   
                        //model.StepTasksList = new List<TaskViewModel>();
                        //foreach (var compResult in TasksList)
                        //{
                        //    if (model.TaskId!= compResult.NtsTaskId) 
                        //    {
                        //        var task = await GetSingleById(compResult.NtsTaskId);
                        //        var Template1 = await templateBusiness.GetSingleById(task.TemplateId);
                        //        if (Template1 != null)
                        //        {
                        //            var page = await pageBusiness.GetSingle(x => x.TemplateId == Template1.Id);
                        //            if (page != null)
                        //            {
                        //                task.PageName = page.Name;
                        //                task.PageId = page.Id;
                        //            }
                        //            else
                        //            {

                        //            }
                        //        }
                        //        model.StepTasksList.Add(task);
                        //    }
                        //}
                    }
                }

                model.IsLockVisible = false;
                model.IsTaskTeamOwner = false;
                if (model.AssignedToTeamId != null && (model.TaskStatusCode == "TASK_STATUS_INPROGRESS" || model.TaskStatusCode == "TASK_STATUS_OVERDUE"))
                {
                    var teamId = model.AssignedToTeamId;
                    // model.AssigneeDisplayName = model.AssigneeDisplayName + "(" + model.TeamName + ")";
                    var assignedId = model.AssignedToUserId;
                    var teamResult = await _teamBusiness.GetTeamMemberList(teamId);
                    if (teamResult.Any())
                    {
                        // newViewModel.IsLockVisible = newViewModel.LockStatus == NtsLockStatusEnum.Locked ? false : true;
                        var teamMember = false;
                        var ownerT = teamResult.Where(x => x.Id == _repo.UserContext.UserId).FirstOrDefault();
                        teamMember = ownerT.IsNotNull() ? true : false;

                        var IsOwner = ownerT.IsNotNull() ? ownerT.IsTeamOwner : false;
                        if (IsOwner)
                        {
                            model.IsTaskTeamOwner = true;
                            model.TeamMembers = teamResult.Where(x => x.Id != _repo.UserContext.UserId).ToList();
                        }
                        var lockv = await _lOVBusiness.GetSingleById(model.LockStatusId);
                        var lockstatus = "";
                        if (lockv != null)
                        {
                            lockstatus = lockv.Code;
                        }

                        model.IsLockVisible = ((lockstatus == "Released" || lockstatus == "") && (assignedId == _repo.UserContext.UserId || IsOwner || teamMember)) ? true : false;
                        model.IsReleaseVisible = (lockstatus == "Locked" && (assignedId == _repo.UserContext.UserId || IsOwner)) ? true : false;
                    }
                }
                if (model.TaskTemplateType == TaskTypeEnum.StepTask)
                {
                    var step = await _stepCompBusiness.GetSingleById(model.StepTaskComponentId);
                    if (step != null)
                    {
                        model.EnableChangingNextTaskAssignee = step.EnableChangingNextTaskAssignee;
                        model.ChangingNextTaskAssigneeTitle = step.ChangingNextTaskAssigneeTitle;
                        model.EnableReturnTask = step.EnableReturnTask;
                        model.ReturnTaskTitle = step.ReturnTaskTitle;
                        model.ReturnTaskButtonText = step.ReturnTaskButtonText;
                        model.EnableNextTaskAttachment = step.EnableNextTaskAttachment;
                        model.DisableNextTaskTeamChange = step.DisableNextTaskTeamChange;
                        model.EnablePlanning = step.EnablePlanning;
                        if (model.EnableChangingNextTaskAssignee)
                        {
                            var stepC = await _stepCompBusiness.GetSingle(x => x.Id == model.StepTaskComponentId);
                            await GetNextTaskAssignee(stepC.ComponentId, model);
                        }
                    }

                }

            }

            return await Task.FromResult(model);
        }
        private async Task GetNextTaskAssignee(string componentId, TaskTemplateViewModel task)
        {
            var taskBusiness = (ITaskBusiness)_serviceProvider.GetService(typeof(ITaskBusiness));

            var ComponentParent = await _repo.GetSingle<ComponentParentViewModel, ComponentParent>(x => x.ParentId == componentId);
            if (ComponentParent != null)
            {
                var Component = await _repo.GetSingle<ComponentViewModel, Component>(x => x.Id == ComponentParent.ComponentId);
                if (Component.ComponentType == ProcessDesignComponentTypeEnum.StepTask)
                {
                    var step = await _stepCompBusiness.GetSingle(x => x.ComponentId == Component.Id);
                    var assignToType = await _lOVBusiness.GetSingleById(step.AssignedToTypeId);
                    if (assignToType != null)
                    {
                        task.NextTaskAssignedToTypeCode = assignToType.Code;
                    }
                    task.NextTaskAssignedToTypeId = step.AssignedToTypeId;
                    task.NextTaskAssignedToUserId = step.AssignedToUserId;
                    task.NextTaskAssignedToTeamId = step.AssignedToTeamId;
                    if (task.NextTaskAssignedToTeamId.IsNotNullAndNotEmpty())
                    {
                        task.NextTaskAssignedToTeamUserId = step.AssignedToUserId;
                    }
                    task.NextTaskAssignedToHierarchyMasterId = step.AssignedToHierarchyMasterId;
                    task.NextTaskAssignedToHierarchyMasterLevelId = step.AssignedToHierarchyMasterLevelId;
                }
                else
                {
                    await GetNextTaskAssignee(Component.Id, task);
                }
            }
        }
        private async Task<List<UserViewModel>> SetSharedList(TaskTemplateViewModel model)
        {
            var list = await _ntsQueryBusiness.SetSharedTaskListData(model);
            return list;

        }
        public async Task GetTaskIndexPageCount(TaskIndexPageTemplateViewModel model, PageViewModel page)
        {

            var result = await _ntsQueryBusiness.GetTaskIndexPageCountData(model, page);
            var activeUserId = _repo.UserContext.UserId;
            var ownerOrRequester = result.Where(x => x.OwnerUserId == activeUserId || x.RequestedByUserId == activeUserId);
            if (ownerOrRequester != null && ownerOrRequester.Count() > 0)
            {
                model.CreatedOrRequestedByMeDraftCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.CreatedOrRequestedByMeInProgreessOverDueCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.CreatedOrRequestedByMeCompleteCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.CreatedOrRequestedByMeRejectCancelCloseCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
            var assignee = result.Where(x => x.AssignedToUserId == activeUserId);
            if (assignee != null && assignee.Count() > 0)
            {
                model.AssignedToMeDraftCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.AssignedToMeInProgreessOverDueCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.AssignedToMeCompleteCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.AssignedToMeRejectCancelCloseCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
            var sharedWith = result.Where(x => x.SharedWithUserId == activeUserId);
            if (sharedWith != null && sharedWith.Count() > 0)
            {
                model.SharedWithMeDraftCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.SharedWithMeInProgressOverDueCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.SharedWithMeCompleteCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.SharedWithMeRejectCancelCloseCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
            var sharedBy = result.Where(x => x.SharedByUserId == activeUserId);
            if (sharedBy != null && sharedBy.Count() > 0)
            {
                model.SharedByMeDraftCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.SharedByMeInProgreessOverDueCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.SharedByMeCompleteCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.SharedByMeRejectCancelCloseCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
        }
        public async Task<TaskIndexPageTemplateViewModel> GetTaskIndexPageViewModel(PageViewModel page)
        {
            var task = await _repo.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == page.TemplateId);
            if (task != null)
            {
                var model = await _repo.GetSingleById<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate>(task.TaskIndexPageTemplateId);
                //var indexPageColumns = await _repo.GetList<TaskIndexPageColumnViewModel, TaskIndexPageColumn>(x => x.TaskIndexPageTemplateId == model.Id && x.IsDeleted == false, x => x.ColumnMetadata);
                //var cloumns = new List<TaskIndexPageColumnViewModel>();
                //foreach (var item in indexPageColumns)
                //{
                //    item.ColumnName = item.ColumnMetadata.Name;
                //    cloumns.Add(item);
                //}
                model.SelectedTableRows = await _repo.GetList<TaskIndexPageColumnViewModel, TaskIndexPageColumn>(x => x.TaskIndexPageTemplateId == model.Id && x.IsDeleted == false, x => x.ColumnMetadata); ;
                await GetTaskIndexPageCount(model, page);
                return model;
            }
            return null;

        }

        public async Task<DataTable> GetTaskIndexPageGridData(DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string taskStatusCode)
        {
            var indexPageTemplate = await _repo.GetSingleById<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate>(indexPageTemplateId);
            if (indexPageTemplate != null)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(indexPageTemplate.TemplateId);
                if (template != null)
                {
                    var udfTableMetadataId = template.UdfTableMetadataId;
                    if (udfTableMetadataId.IsNullOrEmpty())
                    {
                        var stepTask = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.TemplateId == template.Id);
                        udfTableMetadataId = stepTask?.UdfTableMetadataId;
                    }

                    var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(udfTableMetadataId);
                    if (tableMetadata != null)
                    {
                        var selectQuery = await GetSelectQuery(tableMetadata);
                        var filter = "";
                        switch (ownerType)
                        {
                            case NtsActiveUserTypeEnum.Assignee:
                                filter = @$" and ""NtsTask"".""AssignedToUserId""='{_repo.UserContext.UserId}'";
                                break;
                            case NtsActiveUserTypeEnum.OwnerOrRequester:
                                filter = @$" and (""NtsTask"".""OwnerUserId""='{_repo.UserContext.UserId}' or ""NtsTask"".""RequestedByUserId""='{_repo.UserContext.UserId}')";
                                break;
                            case NtsActiveUserTypeEnum.Requester:
                                filter = @$" and (""NtsTask"".""OwnerUserId""<>'{_repo.UserContext.UserId}' and ""NtsTask"".""RequestedByUserId""='{_repo.UserContext.UserId}')";
                                break;
                            case NtsActiveUserTypeEnum.Owner:
                                filter = @$" and (""NtsTask"".""OwnerUserId""='{_repo.UserContext.UserId}')";
                                break;
                            case NtsActiveUserTypeEnum.SharedWith:
                                filter = @$" and (""NtsTask"".""Id"" in ( Select ts.""NtsTaskId"" from public.""NtsTaskShared"" as ts where ts.""IsDeleted""=false  and ts.""SharedWithUserId""='{_repo.UserContext.UserId}') ) ";
                                break;
                            case NtsActiveUserTypeEnum.SharedBy:
                                filter = @$" and (""NtsTask"".""Id"" in ( Select ts.""NtsTaskId"" from public.""NtsTaskShared"" as ts where ts.""IsDeleted""=false  and ts.""SharedByUserId""='{_repo.UserContext.UserId}') ) ";
                                break;
                            default:
                                break;
                        }
                        if (taskStatusCode.IsNotNullAndNotEmpty())
                        {
                            var stausItems = taskStatusCode.Split(',');
                            var statusText = "";
                            foreach (var item in stausItems)
                            {
                                statusText = $"{statusText},'{item}'";
                            }

                            filter = @$" {filter} and (""TaskStatus"".""Code"" in ({statusText.Trim(',')}))";
                        }
                        selectQuery = @$"{selectQuery} {filter}";
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
                                selectQuery = @$"{selectQuery} order by ""NtsTask"".""LastUpdatedDate"" desc";
                            }

                        }
                        else
                        {
                            selectQuery = @$"{selectQuery} order by ""NtsTask"".""LastUpdatedDate"" desc";
                        }
                        var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                        return dt;

                    }
                }
            }
            return new DataTable();

        }


        private async Task GetUdfDetails(TaskTemplateViewModel model)
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
        private void CustomizeJsonForWizard(JArray panels, List<ColumnMetadataViewModel> columnList, TaskTemplateViewModel model)
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

        private void CustomizeJsonForForm(JArray panels, List<ColumnMetadataViewModel> columnList, TaskTemplateViewModel model)
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
                            || type == "datetime" || type == "day" || type == "time" || type == "currency" || type == "button"
                            || type == "signature" || type == "file" || type == "hidden" || type == "datagrid" || type == "editgrid"
                            || (type == "htmlelement" && key == "chartgrid") || (type == "htmlelement" && key == "chartJs"))
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
        private void ManagePanelActions(JObject panel, bool isFirst, bool isLast, TaskTemplateViewModel model)
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
        private async Task ChildComp(JArray comps, List<ColumnMetadataViewModel> Columns, TaskTemplateViewModel model)
        {
            List<UdfPermissionViewModel> stepTaskUdfPermissons = null;
            //if (model.TaskTemplateType == TaskTypeEnum.StepTask)
            //{
            stepTaskUdfPermissons = await _repo.GetList<UdfPermissionViewModel, UdfPermission>(x => x.TemplateId == model.TemplateId);
            //}


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
                        || type == "datetime" || type == "day" || type == "time" || type == "currency" || type == "button"
                        || type == "signature" || type == "file" || type == "hidden" || type == "datagrid" || type == "editgrid" || (type == "htmlelement" && key == "chartgrid") || (type == "htmlelement" && key == "chartJs"))
                    {


                        var reserve = jcomp.SelectToken("reservedKey");
                        if (reserve == null)
                        {
                            var tempmodel = JsonConvert.DeserializeObject<FormFieldViewModel>(jcomp.ToString());
                            var columnId = jcomp.SelectToken("columnMetadataId");
                            if (columnId != null)
                            {
                                if (model.TaskTemplateType == TaskTypeEnum.StepTask)
                                {
                                    var columnMeta = Columns.FirstOrDefault(x => x.Name == tempmodel.key);
                                    if (columnMeta != null)
                                    {
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
                                        if (stepTaskUdfPermissons == null)
                                        {
                                            var hiddenProperty = jcomp.SelectToken("hidden");
                                            if (hiddenProperty == null)
                                            {
                                                var newProperty = new JProperty("hidden", true);
                                                jcomp.Add(newProperty);
                                                if (type == "datagrid" || type == "editgrid")
                                                {
                                                    JArray dataRows = (JArray)jcomp.SelectToken("components");
                                                    foreach (JObject jcomp1 in dataRows)
                                                    {
                                                        var hdnProperty = jcomp1.SelectToken("hidden");
                                                        if (hdnProperty == null)
                                                        {
                                                            var newProperty1 = new JProperty("hidden", true);
                                                            jcomp1.Add(newProperty1);
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var udfColumnPermission = stepTaskUdfPermissons.FirstOrDefault(x => x.ColumnMetadataId == tempmodel.columnMetadataId && x.TemplateId == model.TemplateId);
                                            if (udfColumnPermission != null)
                                            {
                                                udfColumnPermission.ActiveUserType = model.ActiveUserType;
                                                udfColumnPermission.NtsStatusCode = model.TaskStatusCode;
                                                if (!udfColumnPermission.IsVisible)
                                                {
                                                    var hiddenProperty = jcomp.SelectToken("hidden");
                                                    if (hiddenProperty == null)
                                                    {
                                                        var newProperty = new JProperty("hidden", true);
                                                        jcomp.Add(newProperty);
                                                        if (type == "datagrid" || type == "editgrid")
                                                        {
                                                            JArray dataRows = (JArray)jcomp.SelectToken("components");
                                                            foreach (JObject jcomp1 in dataRows)
                                                            {
                                                                var hdnProperty = jcomp1.SelectToken("hidden");
                                                                if (hdnProperty == null)
                                                                {
                                                                    var newProperty1 = new JProperty("hidden", true);
                                                                    jcomp1.Add(newProperty1);
                                                                }

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
                                                    if (type == "datagrid" || type == "editgrid")
                                                    {
                                                        JArray dataRows = (JArray)jcomp.SelectToken("components");
                                                        foreach (JObject jcomp1 in dataRows)
                                                        {
                                                            var hdnProperty = jcomp1.SelectToken("hidden");
                                                            if (hdnProperty == null)
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
                                else
                                {
                                    var columnMeta = Columns.FirstOrDefault(x => x.Name == tempmodel.key);
                                    if (columnMeta != null)
                                    {
                                        columnMeta.ActiveUserType = model.ActiveUserType;
                                        columnMeta.NtsStatusCode = model.TaskStatusCode;
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
                                        if (stepTaskUdfPermissons == null)
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
                                            if (!columnMeta.IsVisible)
                                            {
                                                var hiddenProperty = jcomp.SelectToken("hidden");
                                                if (hiddenProperty == null)
                                                {
                                                    var newProperty = new JProperty("hidden", true);
                                                    jcomp.Add(newProperty);
                                                }
                                            }
                                            if (!columnMeta.IsEditable || isReadonly || model.AllReadOnly)
                                            {
                                                var disableProperty = jcomp.SelectToken("disabled");
                                                if (disableProperty == null)
                                                {
                                                    var newProperty = new JProperty("disabled", true);
                                                    jcomp.Add(newProperty);
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
        private async Task<TaskTemplateViewModel> GetTaskTemplateById(TaskTemplateViewModel viewModel)
        {
            var data = new TaskTemplateViewModel();

            var tableMetadata = await _ntsQueryBusiness.GetTaskTemplateByIdData(viewModel);

            if (tableMetadata != null)
            {
                if (viewModel.LogId.IsNotNullAndNotEmpty())
                {
                    var log = await _ntsQueryBusiness.GetTaskTemplateByIdData1(viewModel);
                    if (log != null)
                    {
                        viewModel.IsLogRecord = !(bool)log.IsLatest;
                    }
                }
                var selectQuery = "";
                if (viewModel.IsLogRecord && viewModel.LogId.IsNotNullAndNotEmpty())
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsTask"".""Id""='{viewModel.LogId}' limit 1", null, null, viewModel.IsLogRecord, viewModel.LogId);
                }
                else
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsTask"".""Id""='{viewModel.TaskId}' limit 1", null, null, viewModel.IsLogRecord, viewModel.LogId);

                }
                data = await _queryRepo.ExecuteQuerySingle<TaskTemplateViewModel>(selectQuery, null);
                if (viewModel.TaskTemplateType == TaskTypeEnum.StepTask && data.ParentServiceId.IsNotNullAndNotEmpty())
                {
                    var service = await _repo.GetSingleById<ServiceViewModel, NtsService>(data.ParentServiceId);
                    if (service != null)
                    {
                        data.UdfNoteId = service.UdfNoteId;
                        data.UdfTemplateId = service.UdfTemplateId;
                    }
                }
                if (viewModel.TaskTemplateType == TaskTypeEnum.StepTask)
                {
                    var step = await _stepCompBusiness.GetSingleById(viewModel.StepTaskComponentId);
                    if (step != null)
                    {
                        viewModel.EnableChangingNextTaskAssignee = step.EnableChangingNextTaskAssignee;
                        viewModel.ChangingNextTaskAssigneeTitle = step.ChangingNextTaskAssigneeTitle;
                        viewModel.EnableReturnTask = step.EnableReturnTask;
                        viewModel.ReturnTaskTitle = step.ReturnTaskTitle;
                        viewModel.ReturnTaskButtonText = step.ReturnTaskButtonText;
                    }
                }
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
            return data;
        }


        public async Task<List<NTSMessageViewModel>> GetTaskMessageList(string userId, string taskId)

        {
            var comments = await _ntsQueryBusiness.GetTaskMessageListData(userId, taskId);

            return comments.OrderByDescending(x => x.SentDate).ToList();
        }
        public async Task<DataTable> GetTaskDataTableById(string taskId, TableMetadataViewModel tableMetadata, bool isLog = false, string logId = null)
        {
            if (tableMetadata != null)
            {
                var selectQuery = "";
                if (isLog && logId.IsNotNullAndNotEmpty())
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsTask"".""Id""='{logId}' limit 1", null, null, isLog, logId);
                }
                else
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsTask"".""Id""='{taskId}' limit 1");
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

            var pkColumns = await _ntsQueryBusiness.GetViewableTaskColumnsData(tableMetaData);
            condition.Add(@$"""{tableMetaData.Name}"".""IsDeleted""=false");
            foreach (var item in pkColumns)
            {
                if (item.TableName == "NtsTask" && item.IsPrimaryKey)
                {
                    item.Alias = "TaskId";
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


                var result = await _ntsQueryBusiness.GetViewableTaskColumnsData1(tableMetaData);
                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        var tableName = item.TableAliasName;
                        if (item.IsUdfColumn)
                        {
                            tableName = @$"{item.TableAliasName}_{item.ForeignKeyColumnName}";
                        }
                        item.TableName = tableName;
                        item.Alias = $"{item.ForeignKeyColumnName}_{item.Name}";
                        columns.Add(item);
                    }

                }
            }

            //query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
            //    FROM public.""ColumnMetadata"" c
            //    join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false
            //    where  t.""Schema""='public' and t.""Name"" in ('User','Template','LOV')
            //    and c.""IsDeleted""=false";
            //var customColumns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            //var userId = customColumns.FirstOrDefault(x => x.TableName == "User" && x.Name == "Id");
            //if (userId != null)
            //{
            //    var createdByUserId = _autoMapper.Map<ColumnMetadataViewModel, ColumnMetadataViewModel>(userId);
            //    createdByUserId.TableName = "CreatedByUser";
            //    createdByUserId.Alias = "CreatedByUser_Id";
            //}
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

            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "Id", Alias = "AssignedToUserId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "Name", Alias = "AssignedToUserName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "Email", Alias = "AssignedToUserEmail" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "JobTitle", Alias = "AssignedToUserJobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "PhotoId", Alias = "AssignedToUserPhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "TableMetadataId", Alias = "TableMetadataId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Name", Alias = "TemplateName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "DisplayName", Alias = "TemplateDisplayName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Code", Alias = "TemplateCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "Json", Alias = "Json" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "TableMetadataId", Alias = "UdfTableMetadataId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToTeam", Name = "Name", Alias = "AssignedToTeamName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToType", Name = "Code", Alias = "AssignedToTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToType", Name = "Name", Alias = "AssignedToTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskOwnerType", Name = "Code", Alias = "TaskOwnerTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskOwnerType", Name = "Name", Alias = "TaskOwnerTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskPriority", Name = "Code", Alias = "TaskPriorityCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskPriority", Name = "Name", Alias = "TaskPriorityName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskStatus", Name = "Code", Alias = "TaskStatusCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskStatus", Name = "Name", Alias = "TaskStatusName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskAction", Name = "Code", Alias = "TaskActionCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskAction", Name = "Name", Alias = "TaskActionName" });
            return columns;
        }
        public async Task<List<ColumnMetadataViewModel>> GetTaskViewableColumns()
        {
            var columns = new List<ColumnMetadataViewModel>();
            var tables = new List<string>();
            var condition = new List<string>();

            var pkColumns = await _ntsQueryBusiness.GetTaskViewableColumnsData();
            foreach (var item in pkColumns)
            {
                if (item.TableName == "NtsTask" && item.IsPrimaryKey)
                {
                    item.Alias = "TaskId";
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

            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "Id", Alias = "AssignedToUserId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "Name", Alias = "AssignedToUserName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "Email", Alias = "AssignedToUserEmail" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "JobTitle", Alias = "AssignedToUserJobTitle" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToUser", Name = "PhotoId", Alias = "AssignedToUserPhotoId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "TableMetadataId", Alias = "TableMetadataId" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Name", Alias = "TemplateName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "DisplayName", Alias = "TemplateDisplayName" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Code", Alias = "TemplateCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "Json", Alias = "Json" });
            columns.Add(new ColumnMetadataViewModel { TableName = "UdfTemplate", Name = "TableMetadataId", Alias = "UdfTableMetadataId" });

            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToTeam", Name = "Name", Alias = "AssignedToTeamName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToType", Name = "Code", Alias = "AssignedToTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "AssignedToType", Name = "Name", Alias = "AssignedToTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskOwnerType", Name = "Code", Alias = "TaskOwnerTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskOwnerType", Name = "Name", Alias = "TaskOwnerTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskPriority", Name = "Code", Alias = "TaskPriorityCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskPriority", Name = "Name", Alias = "TaskPriorityName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskStatus", Name = "Code", Alias = "TaskStatusCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskStatus", Name = "Name", Alias = "TaskStatusName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "TaskAction", Name = "Code", Alias = "TaskActionCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "TaskAction", Name = "Name", Alias = "TaskActionName" });
            return columns;
        }
        public async Task<string> GetSelectQuery(TableMetadataViewModel tableMetaData, string where = null, string filtercolumns = null, string filter = null, bool isLog = false, string logId = null, bool ignoreJoins = false, string returnColumns = null, int? limit = null, int? skip = null)
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
                columns.Add(@$"{Environment.NewLine}""{item.TableName}"".""{item.Name}"" as ""{item.Alias}""");
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
                tables.Add(@$"left join log.""NtsNoteLog"" as ""UdfNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""UdfNote"".""RecordId"" and ""{tableMetaData.Name}"".""VersionNo""=""UdfNote"".""VersionNo"" and ""{tableMetaData.Name}"".""IsVersionLatest""=true and ""UdfNote"".""IsVersionLatest""=true and ""UdfNote"".""IsDeleted""=false and ""UdfNote"".""CompanyId"" = '{_repo.UserContext.CompanyId}'");
                tables.Add(@$"left join log.""NtsTaskLog"" as ""NtsTask"" on ""{tableMetaData.Name}"".""NtsNoteId""=""NtsTask"".""UdfNoteId""  and ""{tableMetaData.Name}"".""VersionNo""=""NtsTask"".""VersionNo"" and ""{tableMetaData.Name}"".""IsVersionLatest""=true and ""NtsTask"".""IsVersionLatest""=true  and ""NtsTask"".""IsDeleted""=false and ""NtsTask"".""CompanyId"" = '{_repo.UserContext.CompanyId}'");
            }
            else
            {
                tables.Add(@$" left join public.""NtsNote"" as ""UdfNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""UdfNote"".""Id"" and ""UdfNote"".""IsDeleted""=false ");
                tables.Add(@$"left join public.""NtsTask"" as ""NtsTask"" on ""UdfNote"".""Id""=""NtsTask"".""UdfNoteId"" and ""NtsTask"".""IsDeleted""=false ");
            }
            tables.Add(@$"left join public.""Template"" as ""Template"" on ""NtsTask"".""TemplateId""=""Template"".""Id"" and ""Template"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""Template"" as ""UdfTemplate"" on ""NtsTask"".""UdfTemplateId""=""UdfTemplate"".""Id"" and ""UdfTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""AssignedToUser"" on ""NtsTask"".""AssignedToUserId""=""AssignedToUser"".""Id"" and ""AssignedToUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""Team"" as ""AssignedToTeam"" on ""NtsTask"".""AssignedToTeamId""=""AssignedToTeam"".""Id"" and ""AssignedToTeam"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""OwnerUser"" on ""NtsTask"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""RequestedByUser"" on ""NtsTask"".""RequestedByUserId""=""RequestedByUser"".""Id"" and ""RequestedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""CreatedByUser"" on ""NtsTask"".""CreatedBy""=""CreatedByUser"".""Id"" and ""CreatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""UpdatedByUser"" on ""NtsTask"".""LastUpdatedBy""=""UpdatedByUser"".""Id"" and ""UpdatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""TaskTemplate"" as ""TaskTemplate"" on ""TaskTemplate"".""TemplateId""=""Template"".""Id"" and ""TaskTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""TemplateCategory"" as ""TemplateCategory"" on ""TemplateCategory"".""Id""=""Template"".""TemplateCategoryId"" and ""TemplateCategory"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""TaskOwnerType"" on ""NtsTask"".""TaskOwnerTypeId""=""TaskOwnerType"".""Id"" and ""TaskOwnerType"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""TaskPriority"" on ""NtsTask"".""TaskPriorityId""=""TaskPriority"".""Id"" and ""TaskPriority"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""TaskStatus"" on ""NtsTask"".""TaskStatusId""=""TaskStatus"".""Id"" and ""TaskStatus"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""TaskAction"" on ""NtsTask"".""TaskActionId""=""TaskAction"".""Id"" and ""TaskAction"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""AssignedToType"" on ""NtsTask"".""AssignedToTypeId""=""AssignedToType"".""Id"" and ""AssignedToType"".""IsDeleted""=false ");
            var selectQuery = @$"select {string.Join(",", columns)}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            if (where.IsNotNullAndNotEmpty())
            {
                selectQuery = $"{selectQuery} {where}";
            }
            if (ignoreJoins)
            {
                if (returnColumns.IsNullOrEmpty())
                {
                    returnColumns = "*";
                }
                selectQuery = @$"select {returnColumns} from { tableMetaData.Schema}.""{ tableMetaData.Name}"" where ""IsDeleted""=false ";
                if (where.IsNotNullAndNotEmpty())
                {
                    selectQuery = $"{selectQuery} {where}";
                }
            }
            return selectQuery;

        }


        private async Task<string> GenerateNextTaskNo(TaskTemplateViewModel model)
        {
            if (model.NumberGenerationType != NumberGenerationTypeEnum.SystemGenerated)
            {
                return "";
            }
            string prefix = "T";
            var today = DateTime.Today;

            var nextId = await _ntsQueryBusiness.GenerateNextTaskNoData();
            if (nextId == null)
            {
                nextId = 1;
                await _repo.Create<NtsTaskSequence, NtsTaskSequence>(
                    new NtsTaskSequence
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

        private async Task<TaskTemplateViewModel> GetTaskTemplateForNewTask(TaskTemplateViewModel vm)
        {

            var data = await _ntsQueryBusiness.GetTaskTemplateForNewTaskData(vm);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.UdfTableMetadataId
                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
            }
            return data;
        }

        private async Task SetReadonlyData(TaskTemplateViewModel model)
        {
            var attachmentslist = await _repo.GetList<FileViewModel, File>(x => x.ReferenceTypeId == model.TaskId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task);
            var sharedlist = await _ntsTaskSharedBusiness.GetSearchResult(model.TaskId);
            //var notificationlist = await _notificationBusiness.GetTaskNotificationList(model.TaskId, _userContex.UserId, 0);
            model.SharedCount = sharedlist.Count();
            model.AttachmentCount = attachmentslist.Count();
            model.NotificationCount = 0;// notificationlist.Count();
            model.CommentCount = 8;
        }

        public async Task<IList<TaskViewModel>> GetNtsTaskIndexPageGridData(DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string taskStatusCode, string categoryCode, string templateCode, string moduleCode)
        {

            var querydata = await _ntsQueryBusiness.GetNtsTaskIndexPageGridData(request, ownerType, taskStatusCode, categoryCode, templateCode, moduleCode);
            return querydata;
        }
        public async Task GetNtsTaskIndexPageCount(NtsTaskIndexPageViewModel model)
        {

            var result = await _ntsQueryBusiness.GetNtsTaskIndexPageCountData(model);
            var activeUserId = _repo.UserContext.UserId;
            var ownerOrRequester = result.Where(x => x.OwnerUserId == activeUserId || x.RequestedByUserId == activeUserId);
            if (ownerOrRequester != null && ownerOrRequester.Count() > 0)
            {
                model.CreatedOrRequestedByMeDraftCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.CreatedOrRequestedByMeInProgreessOverDueCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.CreatedOrRequestedByMeCompleteCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.CreatedOrRequestedByMeRejectCancelCloseCount = ownerOrRequester.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
            var assignee = result.Where(x => x.AssignedToUserId == activeUserId);
            if (assignee != null && assignee.Count() > 0)
            {
                model.AssignedToMeDraftCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.AssignedToMeInProgreessOverDueCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.AssignedToMeCompleteCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.AssignedToMeRejectCancelCloseCount = assignee.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
            var sharedWith = result.Where(x => x.SharedWithUserId == activeUserId);
            if (sharedWith != null && sharedWith.Count() > 0)
            {
                model.SharedWithMeDraftCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.SharedWithMeInProgressOverDueCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.SharedWithMeCompleteCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.SharedWithMeRejectCancelCloseCount = sharedWith.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
            var sharedBy = result.Where(x => x.SharedByUserId == activeUserId);
            if (sharedBy != null && sharedBy.Count() > 0)
            {
                model.SharedByMeDraftCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_DRAFT").Sum(x => x.TaskCount);
                model.SharedByMeInProgreessOverDueCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").Sum(x => x.TaskCount);
                model.SharedByMeCompleteCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Sum(x => x.TaskCount);
                model.SharedByMeRejectCancelCloseCount = sharedBy.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT" || x.TaskStatusCode == "TASK_STATUS_CANCEL" || x.TaskStatusCode == "TASK_STATUS_CLOSE").Sum(x => x.TaskCount);
            }
        }

        public async Task<IList<NtsTaskIndexPageViewModel>> GetTaskCountByServiceTemplateCodes(string categoryCodes = null, string portalId = null, bool showAllTaskForAdmin = false, string templateCodes = null, string portalNames = null, string statusCodes = null, string userId = null)
        {
            var result = await _ntsQueryBusiness.GetTaskCountByServiceTemplateCodesData(categoryCodes, portalId, showAllTaskForAdmin, templateCodes, portalNames, statusCodes, userId);

            return result;
        }

        public async Task<IList<TaskViewModel>> GetTaskListByServiceCategoryCodes(string categoryCodes = null, string status = null, string portalId = null, bool showAllTaskForAdmin = false, string templateCodes = null, string portalNames = null, string userId = null)
        {
            var result = await _ntsQueryBusiness.GetTaskListByServiceCategoryCodesData(categoryCodes, status, portalId, showAllTaskForAdmin, templateCodes, portalNames, userId);
            return result;
        }

        public async Task<IList<TaskViewModel>> GetServiceAdhocTaskGridData(DataSourceRequest request, string adhocTaskTemplateIds, string serviceId)
        {

            var querydata = await _ntsQueryBusiness.GetServiceAdhocTaskGridData(request, adhocTaskTemplateIds, serviceId);
            return querydata;
        }

        public async Task<CommandResult<TaskViewModel>> CreateEmail(TaskViewModel model)
        {

            var result = new TaskViewModel();
            if (model.DataAction == DataActionEnum.Create)
            {
                var parenttask = await _repo.GetSingleById(model.ParentTaskId);
                var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == model.TemplateCode);
                model.StartDate = parenttask.StartDate;
                model.DueDate = parenttask.DueDate;
                model.TaskSLA = parenttask.TaskSLA;
                model.TaskPriorityId = parenttask.TaskPriorityId;
                model.TemplateId = template.Id;
                model.TaskNo = await GenerateNextTaskNo(new TaskTemplateViewModel { NumberGenerationType = NumberGenerationTypeEnum.SystemGenerated });
                result = await _repo.Create(model);
                return CommandResult<TaskViewModel>.Instance(result);

            }
            return CommandResult<TaskViewModel>.Instance(result);

        }
        public async Task<IList<TaskViewModel>> GetSearchResult(TaskSearchViewModel searchModel)
        {
            //var hour = GeneralExtension.ServerToLocalTimeDiff(LegalEntityCode);
            var userId = searchModel.UserId;
            var list = await _ntsQueryBusiness.GetTaskSearchResultData(searchModel);

            if (searchModel != null)
            {
                if (searchModel.ModuleCode.IsNotNull())
                {
                    var modules = searchModel.ModuleCode.Split(",");
                    list = list.Where(x => x.ModuleCode != null && modules.Any(y => y == x.ModuleCode)).ToList();
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
                if (searchModel.ModuleId.IsNotNull())
                {
                    // list = list.Where(x => x.ModuleId == searchModel.ModuleId).ToList();
                    var modules = searchModel.ModuleId.Split(",");
                    list = list.Where(x => x.ModuleId != null && modules.Any(y => y == x.ModuleId)).ToList();
                }
                if (searchModel.Mode.IsNotNullAndNotEmpty())
                {
                    var modes = searchModel.Mode.Split(',');
                    if (modes.Any(y => y == "ASSIGN_TO"))
                        list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Assignee).ToList();
                    if (modes.Any(y => y == "ASSIGN_BY"))
                        list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Owner).ToList();
                    if (modes.Any(y => y == "SHARE_TO"))
                        list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Shared).ToList();
                    //if ("ASSIGN_TO".Equals(searchModel.Mode))
                    //    list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Assignee).ToList();
                    //if ("ASSIGN_BY".Equals(searchModel.Mode))
                    //    list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Owner).ToList();
                    //if ("SHARE_TO".Equals(searchModel.Mode))
                    //    list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Shared).ToList();
                }
                if (searchModel.TaskNo.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.TaskNo.Contains(searchModel.TaskNo, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }

                if (searchModel.TaskStatus.IsNotNull())
                {
                    var taskList = searchModel.TaskStatus.Split(',');
                    list = list.Where(x => taskList.Any(y => y == x.TaskStatusCode)).ToList();
                    //list = list.Where(x => x.TaskStatusCode == searchModel.TaskStatus).ToList();
                }
                if (searchModel.TaskAssigneeIds.IsNotNull() && searchModel.TaskAssigneeIds.Count() > 0)
                {
                    // var TaskAssignee = searchModel.TaskAssigneeIds;
                    list = list.Where(x => searchModel.TaskAssigneeIds.Contains(x.AssignedToUserId)).ToList();
                    //list = list.Where(x => x.TaskStatusCode == searchModel.TaskStatus).ToList();
                }
                if (searchModel.Subject.IsNotNull())
                {
                    list = list.Where(x => x.TaskSubject.IsNotNullAndNotEmpty() && x.TaskSubject.Contains(searchModel.Subject, StringComparison.InvariantCultureIgnoreCase)).ToList();
                }

                if (searchModel.StartDate.IsNotNull())
                {
                    list = list.Where(x => x.StartDate.Value.Date == searchModel.StartDate.Value.Date).ToList();
                }
                if (searchModel.DueDate.IsNotNull())
                {
                    list = list.Where(x => x.DueDate.Value.Date == searchModel.DueDate.Value.Date).ToList();
                }
                if (searchModel.CompletionDate.IsNotNull())
                {
                    list = list.Where(x => x.DueDate == searchModel.CompletionDate).ToList();
                }
                if (searchModel.ClosedDate.IsNotNull())
                {
                    list = list.Where(x => x.ClosedDate.Value.Date == searchModel.ClosedDate.Value.Date).ToList();
                }
                if (searchModel.ReminderDate.IsNotNull())
                {
                    list = list.Where(x => x.ReminderDate.Value.Date == searchModel.ReminderDate.Value.Date).ToList();
                }
                //if (searchModel.ServiceId.IsNotNull())
                //{
                //    list = list.Where(x => x.ServiceId == searchModel.ServiceId).ToList();
                //}
                //if (searchModel.TemplateMasterCode.IsNotNull())
                //{
                //    list = list.Where(x => x.TemplateMasterCode == searchModel.TemplateMasterCode).ToList();
                //}
            }
            list = list.Distinct(new TaskViewModelComparer()).OrderByDescending(x => x.CreatedDate).ToList();
            return list.ToList();
        }

        public async Task<bool> IsTaskSubjectUnique(string templateId, string subject, string taskId)
        {
            var data = await _ntsQueryBusiness.IsTaskSubjectUniqueData(templateId, subject, taskId);
            return true;




        }
        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseTaskSLA(TaskSearchViewModel searchModel)
        {
            var userId = searchModel.UserId;


            var queryData = await _ntsQueryBusiness.GetDatewiseTaskSLAData(searchModel, userId);

            var list = new List<ProjectDashboardChartViewModel>();

            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }
        public async Task<List<IdNameViewModel>> GetSharedList(string TaskId)
        {
            var list = await _ntsQueryBusiness.GetTaskSharedListData(TaskId);
            return list.Distinct().ToList();

        }
        public async Task<IList<TaskViewModel>> GetTaskByUser(string userId)
        {

            var task = await _ntsQueryBusiness.GetTaskByUserData(userId);
            return task;
        }
        public async Task ReAssignTerminatedEmployeeTasks(string userId, List<string> taskList)
        {
            var _lovBussiness = _serviceProvider.GetService<ILOVBusiness>();
            var status = await _lovBussiness.GetList(x => x.LOVType == "LOV_TASK_STATUS");
            var AssignedTo = await _lovBussiness.GetSingle(x => x.LOVType == "TASK_ASSIGN_TO_TYPE" && x.Code == "TASK_ASSIGN_TO_USER");
            foreach (var id in taskList)
            {
                var newViewModel = await GetTaskDetails(new TaskTemplateViewModel
                { TaskId = id, ActiveUserId = _repo.UserContext.UserId, DataAction = DataActionEnum.Edit/*, ServiceVersionId = 0*/ });
                newViewModel.AssignedToUserId = userId;
                newViewModel.AssignedToTypeId = AssignedTo.Id;
                newViewModel.AssignedToTeamId = null;
                newViewModel.AllowPastStartDate = true;
                if (newViewModel.TaskStatusId == status.Where(x => x.Code == "TASK_STATUS_INPROGRESS").Select(x => x.Id).FirstOrDefault() || newViewModel.TaskStatusId == status.Where(x => x.Code == "TASK_STATUS_OVERDUE").Select(x => x.Id).FirstOrDefault())
                {
                    //newViewModel.TemplateAction = NtsActionEnum.EditAsNewVersion;
                    newViewModel.StartDate = DateTime.Now.ApplicationNow();
                    newViewModel.DueDate = newViewModel.StartDate.HasValue ? newViewModel.StartDate.Value.Add(newViewModel.SLA ?? new TimeSpan(0, 00, 00, 00)) : default(DateTime?);
                }
                await ManageTask(newViewModel);
            }
        }

        public async Task<IList<TaskViewModel>> GetTaskList(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null, string serId = null, string serTempCodes = null)
        {
            var result = await _ntsQueryBusiness.GetTaskListData(portalId, moduleCodes, templateCodes, categoryCodes, statusCodes, parentServiceId, userId, parentNoteId, serId, serTempCodes);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            }
            return result;
        }
        public async Task<IList<TaskViewModel>> GetTaskListDataByWithNoteReferenceId(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null, string serId = null, string serTempCodes = null)
        {
            var result = await _ntsQueryBusiness.GetTaskListDataByWithNoteReferenceId(portalId, moduleCodes, templateCodes, categoryCodes, statusCodes, parentServiceId, userId, parentNoteId, serId, serTempCodes);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            }
            return result;
        }
        

        public async Task<IList<TaskViewModel>> GetWorkboardTaskList(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null)
        {
            var result = await _ntsQueryBusiness.GetWorkboardTaskListData(portalId, moduleCodes, templateCodes, categoryCodes, statusCodes, parentServiceId, userId, parentNoteId);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetWorkPerformanceTaskList(TaskSearchViewModel search)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            if (search.TrendType == "Daily")
            {
                search.StartDate = search.DueDate.Value.AddDays(-6);
            }
            else if (search.TrendType == "Weekly")
            {
                var date = search.DueDate.Value.Date;
                DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                int offset = fdow - date.DayOfWeek;
                DateTime fdowDate = date.AddDays(offset);
                search.DueDate = fdowDate.AddDays(6);
                search.StartDate = cal.AddWeeks(search.DueDate.Value.Date, -6);
            }
            else if (search.TrendType == "Monthly")
            {
                search.StartDate = search.DueDate.Value.AddMonths(-6);
            }
            else if (search.TrendType == "Yearly")
            {
                search.StartDate = new DateTime(search.DueDate.Value.Year, 1, 1, 0, 0, 00);
                search.DueDate = new DateTime(search.DueDate.Value.Year, 12, 31, 0, 0, 00);
            }
            var tasklist = await _ntsQueryBusiness.GetWorkPerformanceTaskListData(search);
            var list = new List<ProjectDashboardChartViewModel>();
            if (search.TrendType == "Daily")
            {
                var list1 = tasklist.GroupBy(x => x.StartDate.Value.Date).Select(group => group.ToList()).ToList();
                var list2 = list1.Select(group => new ProjectDashboardChartViewModel
                {
                    Type = group.Select(x => x.StartDate.Value.Date.ToDefaultDateFormat()).FirstOrDefault(),
                    ValueB = group.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count(),
                    ValueO = group.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count(),
                    ValueC = group.Where(x => x.TaskStatusCode == "TASK_STATUS_CLOSE").Count(),
                }).ToList();
                list = list2;
            }
            else if (search.TrendType == "Weekly")
            {
                var list1 = tasklist.GroupBy(x => cal.GetWeekOfYear(x.StartDate.Value.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday)).Select(group => group.ToList()).ToList();
                var list2 = list1.Select(group => new ProjectDashboardChartViewModel
                {
                    Type = group.Select(x => ("Week-" + cal.GetWeekOfYear(x.StartDate.Value.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday))).FirstOrDefault(),
                    ValueB = group.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count(),
                    ValueO = group.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count(),
                    ValueC = group.Where(x => x.TaskStatusCode == "TASK_STATUS_CLOSE").Count(),
                }).ToList();
                list = list2;
            }
            else if (search.TrendType == "Monthly")
            {
                var list1 = tasklist.GroupBy(x => x.StartDate.Value.Date.Month).Select(group => group.ToList()).ToList();
                var list2 = list1.Select(group => new ProjectDashboardChartViewModel
                {
                    Type = group.Select(x => ((MonthEnum)x.StartDate.Value.Date.Month).ToString() + "-" + x.StartDate.Value.Date.Year.ToString()).FirstOrDefault(),
                    ValueB = group.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count(),
                    ValueO = group.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count(),
                    ValueC = group.Where(x => x.TaskStatusCode == "TASK_STATUS_CLOSE").Count(),
                }).ToList();
                list = list2;
            }
            else if (search.TrendType == "Yearly")
            {
                var list1 = tasklist.GroupBy(x => x.StartDate.Value.Date.Year).Select(group => group.ToList()).ToList();
                var list2 = list1.Select(group => new ProjectDashboardChartViewModel
                {
                    Type = group.Select(x => x.StartDate.Value.Date.Year.ToString()).FirstOrDefault(),
                    ValueB = group.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count(),
                    ValueO = group.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count(),
                    ValueC = group.Where(x => x.TaskStatusCode == "TASK_STATUS_CLOSE").Count(),
                }).ToList();
                list = list2;
            }

            return list;
        }

        public async Task<List<NtsLogViewModel>> GetVersionDetails(string taskId)
        {
            var result = await _ntsQueryBusiness.GetVersionDetailsTaskData(taskId);
            return result;

        }

        public async Task<List<DashboardCalendarViewModel>> GetWorkPerformanceCount(string userId, string moduleCodes = null, DateTime? fromDate = null, DateTime? toDate = null)
        {

            var result = await _ntsQueryBusiness.GetWorkPerformanceCountData(userId, moduleCodes, fromDate, toDate);

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
        public async Task<List<IdNameViewModel>> GetTaskUserList(string taskId)
        {
            var list = await _ntsQueryBusiness.GetTaskUserListData(taskId);
            return list.Distinct().ToList();

        }


        public async Task<string> ChangeAssignee(string taskId, string userId)
        {
            var data = await _repo.GetSingleById(taskId);

            var ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_LOCK");
            data.TaskEventId = ntsevent.Id;

            var lockStatus = await _lOVBusiness.GetSingle(x => x.Code == "Locked" && x.LOVType == "LOV_LOCK");
            data.LockStatusId = lockStatus.Id;
            data.AssignedToUserId = userId;
            var result = await _repo.Edit(data);
            var task = _autoMapper.Map<TaskViewModel, TaskTemplateViewModel>(data);
            await ManageNotification(task);
            //data.LockStatus = NtsLockStatusEnum.Locked;
            //var result = _repository.Edit(data, false);
            //_repository.CreateOneToOneRelationship<R_Task_AssignedTo_User, ADM_User>(taskId,
            //new R_Task_AssignedTo_User(), userId, false);
            //_repository.Commit();
            //var model = GetTaskDetails(new TaskViewModel { Id = taskId });
            //model.EventName = "Locked";
            //this.CreateTaskLog(model);
            //ManageNotification(model, ReferenceTypeEnum.NTS_Task, taskId);
            return "Success";
        }
        public async Task<string> ChangeLockStatus(string taskId)
        {
            var data = await _repo.GetSingleById(taskId);

            var ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_LOCK");
            data.TaskEventId = ntsevent.Id;

            var lockStatus = await _lOVBusiness.GetSingle(x => x.Code == "Released" && x.LOVType == "LOV_LOCK");
            data.LockStatusId = lockStatus.Id;
            var user = await _teamBusiness.GetTeamOwner(data.AssignedToTeamId);
            if (user != null)
            {
                data.AssignedToUserId = user.Id;
            }
            var result = await _repo.Edit(data);
            //var data = _repository.GetSingleById<NTS_Task>(taskId);
            //data.LockStatus = NtsLockStatusEnum.Released;
            //var result = _repository.Edit(data);
            //var model = GetTaskDetails(new TaskViewModel { Id = taskId });
            //model.EventName = "Released";
            //this.CreateTaskLog(model);
            return "Success";
        }
        public async Task<string> UpdateActualStartDate(string taskId)
        {
            var data = await _repo.GetSingleById(taskId);

            data.ActualStartDate = DateTime.Now;

            if (data.DueDate < data.ActualStartDate)
            {
                var taskStatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_TASK_STATUS" && x.Code == "TASK_STATUS_OVERDUE");
                if (taskStatus != null)
                {
                    data.TaskStatusId = taskStatus.Id;
                }
            }
            else
            {

                var taskStatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_TASK_STATUS" && x.Code == "TASK_STATUS_INPROGRESS");
                if (taskStatus != null)
                {
                    data.TaskStatusId = taskStatus.Id;
                }
            }

            var result = await _repo.Edit(data);

            return "Success";
        }

        public async Task<TaskTemplateViewModel> GetFormIoData(string templateId, string taskId, string userId)
        {
            var data = await _ntsQueryBusiness.GetFormIoData(templateId, taskId, userId);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.TableMetadataId
               && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
                data.AllReadOnly = true;
                await GetUdfDetails(data);

                var dr = await _ntsQueryBusiness.GetFormIoTaskData(data.TableName, taskId);
                data.DataJson = dr.ToJson();
            }

            return data;
        }

        public async Task<WorklistDashboardSummaryViewModel> TaskCountForDashboard(string userId, string bookId)
        {

            var task = await _ntsQueryBusiness.TaskCountForDashboardData(userId, bookId);
            var assToMePlanned = task.Where(x => x.TaskStatusCode == "TASK_STATUS_PLANNED").Count();
            var assToMePlannedOverdue = task.Where(x => x.TaskStatusCode == "TASK_STATUS_PLANNED_OVERDUE").Count();
            var assToMeOverdue = task.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").Count();
            var assToMePending = task.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").Count();
            var assToMeCompleted = task.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").Count();
            var assToMeRejected = task.Where(x => x.TaskStatusCode == "TASK_STATUS_REJECT").Count();
            var assToMeCancel = task.Where(x => x.TaskStatusCode == "TASK_STATUS_CANCEL").Count();


            var count = new WorklistDashboardSummaryViewModel()
            {
                T_AssignPlanned = Convert.ToInt64(assToMePlanned),
                T_AssignPlannedOverdue = Convert.ToInt64(assToMePlannedOverdue),
                T_AssignPending = Convert.ToInt64(assToMePending),
                T_AssignCompleted = Convert.ToInt64(assToMeCompleted),
                T_AssignOverdue = Convert.ToInt64(assToMeOverdue),
                T_AssignReject = Convert.ToInt64(assToMeRejected),
                T_AssignCancel = Convert.ToInt64(assToMeCancel),



            };
            return count;
        }
        public async Task<List<TaskViewModel>> TaskDashboardIndex(string userId, string statusFilter = null)
        {

            var task = await _ntsQueryBusiness.TaskDashboardIndexData(userId, statusFilter);
            return task;
        }
        public async Task<List<NoteViewModel>> LoadWorkBooks(string userId, string statusFilter = null)
        {

            var task = await _ntsQueryBusiness.LoadWorkBooksData(userId, statusFilter);

            foreach (var item in task)
            {
                item.key = item.Id;
                item.lazy = true;
                item.title = "";
                var flag = task.Any(x => x.Id == item.ParentId);
                if (item.ParentId.IsNotNull() && !flag)
                {
                    item.ParentId = null;
                }
                var list = await GetNextNoteSequenceNo(item.Id);
                item.NextSequenceNo = list.Count + 1;
                //if (item.BookType == "Note")
                //{
                //    item.NextSequenceNo = await GetNextNoteSequenceNo(item.Id)+1;
                //}
                //else if (item.BookType == "Service")
                //{
                //    item.NextSequenceNo = await GetNextServiceSequenceNo(item.Id)+1;
                //}

            }
            return task.OrderBy(x => x.SequenceOrder).OrderBy(x => x.CreatedDate).ToList();
        }

        public async Task<List<NoteViewModel>> LoadProcessBooks(string userId, string statusFilter = null)
        {

            var task = await _ntsQueryBusiness.LoadProcessBooksData(userId, statusFilter);

            foreach (var item in task)
            {
                var flag = task.Any(x => x.Id == item.ParentId);
                if (item.ParentId.IsNotNull() && !flag)
                {
                    item.ParentId = null;
                }
                var list = await GetNextNoteSequenceNo(item.Id);
                item.NextSequenceNo = list.Count + 1;
                //if (item.BookType == "Note")
                //{
                //    item.NextSequenceNo = await GetNextNoteSequenceNo(item.Id)+1;
                //}
                //else if (item.BookType == "Service")
                //{
                //    item.NextSequenceNo = await GetNextServiceSequenceNo(item.Id)+1;
                //}

            }
            return task.OrderBy(x => x.SequenceOrder).OrderBy(x => x.CreatedDate).ToList();
        }

        public async Task<List<NoteViewModel>> LoadProcessStage(string userId, string statusFilter = null)
        {

            var task = await _ntsQueryBusiness.LoadProcessStageData(userId, statusFilter);
            return task.OrderBy(x => x.SequenceOrder).OrderBy(x => x.CreatedDate).ToList();
        }

        public async Task<List<NoteViewModel>> GetNextNoteSequenceNo(string notePlusId)
        {



            var task = await _ntsQueryBusiness.GetNextNoteSequenceNoData(notePlusId);
            return task;
        }

        public async Task<long> GetNextServiceSequenceNo(string servicePlusId)
        {
            var task = await _ntsQueryBusiness.GetNextServiceSequenceNoData(servicePlusId);
            return task;
        }


        public async Task<List<NotificationViewModel>> NotificationDashboardIndex(string userId, string bookId)
        {

            var task = await _ntsQueryBusiness.NotificationDashboardIndexData(userId, bookId);
            return task;
        }


        //public async Task UpdateStepTaskAssignee(string taskId,string ownerUserId)
        //{

        //    await _ntsQueryBusiness.UpdateStepTaskAssigneeData(taskId, ownerUserId);


        //}
        public async Task<bool> UpdateStepTaskAssignee(string taskId, string ownerUserId)
        {
            var query = $@"Update public.""NtsTask"" set ""AssignedToUserId""='{ownerUserId}' where ""Id""='{taskId}' ";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task UpdateServiceWorkflowStatusForTask(string serviceId, string taskStatus, string taskId)
        {
            var service = await _repo.GetSingleById<ServiceViewModel, NtsService>(serviceId);
            var WorkflowStatus = "";
            if (taskId.IsNotNullAndNotEmpty())
            {
                var task = await _repo.GetSingleById(taskId);
                var user = await _repo.GetSingleById<UserViewModel, User>(task.AssignedToUserId);
                var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.TemplateId == task.TemplateId);
                if (taskStatus == "TASK_STATUS_INPROGRESS")
                {
                    if (stepTaskComponent.InprogressWorkflowStatus.IsNotNullAndNotEmpty())
                    {
                        WorkflowStatus = stepTaskComponent.InprogressWorkflowStatus;
                    }
                    else
                    {
                        WorkflowStatus = "Pending with ";
                    }
                }
                else if (taskStatus == "TASK_STATUS_OVERDUE")
                {
                    if (stepTaskComponent.OverdueWorkflowStatus.IsNotNullAndNotEmpty())
                    {
                        WorkflowStatus = stepTaskComponent.OverdueWorkflowStatus;
                    }
                    else
                    {
                        WorkflowStatus = "OverDue with ";
                    }
                }
                else if (taskStatus == "TASK_STATUS_REJECT")
                {
                    if (stepTaskComponent.RejectedWorkflowStatus.IsNotNullAndNotEmpty())
                    {
                        WorkflowStatus = stepTaskComponent.RejectedWorkflowStatus;
                    }
                    else
                    {
                        WorkflowStatus = "Rejected by ";
                    }
                }
                service.WorkflowStatus = stepTaskComponent.WorkflowStatusName + " " + WorkflowStatus + user.Name;
            }

            await _repo.Edit<ServiceViewModel, NtsService>(service);
        }

        public async Task<NtsSummaryViewModel> GetTaskSummary(string portalId, string userId)
        {
            var result = await _ntsQueryBusiness.GetTaskSummary(portalId, userId);
            return result;
        }

        public async Task<IList<NtsTaskIndexPageViewModel>> GetTemplatesListWithTaskCount(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllTasksForAdmin = false)
        {
            var result = await _ntsQueryBusiness.GetTemplatesListWithTaskCount(templateCodes, catCodes, groupCodes, showAllTasksForAdmin);
            return result;
        }

        public async Task<IList<TaskViewModel>> GetTaskListWithHourSpentData(string portalId, string statusCodes = null, string userId = null)
        {
            var result = await _ntsQueryBusiness.GetTaskListWithHoursSpentData(portalId, statusCodes, userId);
            foreach (var i in result)
            {
                i.DisplayDueDate = DateHumanizeExtensions.Humanize(i.DueDate);
            }
            return result;
        }
    }
}

