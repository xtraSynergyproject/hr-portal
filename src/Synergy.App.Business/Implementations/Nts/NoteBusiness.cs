using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
////using Kendo.Mvc.UI;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hangfire;

namespace Synergy.App.Business
{
    public class NoteBusiness : BusinessBase<NoteViewModel, NtsNote>, INoteBusiness
    {
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileBusiness _fileBusiness;
        private readonly IRepositoryQueryBase<NtsLogViewModel> _queryNtsLog;
        private readonly IUserContext _userContex;
        private readonly ILOVBusiness _lOVBusiness;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public NoteBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper, IRepositoryQueryBase<NoteViewModel> queryRepo,
            IServiceProvider serviceProvider, IFileBusiness fileBusiness,
        IRepositoryQueryBase<NtsLogViewModel> queryNtsLog, ILOVBusiness lOVBusiness,
            IUserContext userContex, INtsQueryBusiness ntsQueryBusiness
             //, IHangfireScheduler hangfireScheduler
            ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceProvider = serviceProvider;
            _userContex = userContex;
            _fileBusiness = fileBusiness;
            _queryNtsLog = queryNtsLog;
            _lOVBusiness = lOVBusiness;
            _ntsQueryBusiness = ntsQueryBusiness;
            //_hangfireScheduler = hangfireScheduler;
        }
        public async Task<CommandResult<NoteTemplateViewModel>> DeleteNote(NoteTemplateViewModel model)
        {
            var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.TableMetadataId);
            if (tableMetaData != null)
            {
                var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ");
                selectQuery.Append(Environment.NewLine);
                selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
                selectQuery.Append(@$"where ""NtsNoteId""='{model.NoteId}';{Environment.NewLine}");
                selectQuery.Append(@$"update public.""NtsNote"" set ");
                selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
                selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
                selectQuery.Append(@$"where ""Id""='{model.NoteId}'");
                var queryText = selectQuery.ToString();
                await _queryRepo.ExecuteCommand(queryText, null);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(model);
        }
        private async Task<CommandResult<NoteTemplateViewModel>> ManagePresubmit(NoteTemplateViewModel model)
        {
            dynamic dobject = new { };
            if (model.Json.IsNotNullAndNotEmptyAndNotValue("{}"))
            {
                dobject = model.Json.JsonToDynamicObject();
            }
            try
            {
                var result = await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PreSubmit, model.NoteStatusCode, model, dobject);
                return result;
            }
            catch (Exception ex)
            {
                return CommandResult<NoteTemplateViewModel>.Instance(model, false, $"Error in pre submit business rule execution: {ex.Message}");
            }

        }
        public async Task<CommandResult<NoteTemplateViewModel>> ManageNote(NoteTemplateViewModel model)
        {

            CommandResult<NoteTemplateViewModel> result = null;
            model.TemplateViewModel = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
            model.NoteTemplateVM = await _repo.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == model.TemplateId);
            var existingItem = await _repo.GetSingleById(model.NoteId);
            if (model.NoteTemplateVM.IsNotNull() && model.NoteTemplateVM.SubjectUdfMappingColumn.IsNotNullAndNotEmpty())
            {
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Json);
                if (rowData.IsNotNull() && rowData.ContainsKey(model.NoteTemplateVM.SubjectUdfMappingColumn))
                {
                    model.NoteSubject = Convert.ToString(rowData[model.NoteTemplateVM.SubjectUdfMappingColumn]);
                }

            }
            if (model.NoteTemplateVM.IsNotNull() && model.NoteTemplateVM.DescriptionUdfMappingColumn.IsNotNullAndNotEmpty())
            {
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Json);
                if (rowData.IsNotNull() && rowData.ContainsKey(model.NoteTemplateVM.DescriptionUdfMappingColumn))
                {
                    model.NoteDescription = Convert.ToString(rowData[model.NoteTemplateVM.DescriptionUdfMappingColumn]);
                }

            }
            //var tableMetaBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
            //DataRow existingItemTableData = null;
            //if (existingItem != null)
            //{
            //    existingItemTableData = await tableMetaBusiness.GetTableDataByHeaderId(existingItem.TemplateId, model.NoteId);
            //}
            var ntsevent = new LOVViewModel();
            switch (model.NoteStatusCode)
            {
                case "NOTE_STATUS_DRAFT":
                    if (model.NoteId.IsNullOrEmpty())
                    {
                        model.NoteId = Guid.NewGuid().ToString();
                    }
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_DRAFT");
                    model.NoteEventId = ntsevent.Id;
                    result = await SaveAsDraft(model);
                    break;
                case "NOTE_STATUS_INPROGRESS":
                    if (model.NoteId.IsNullOrEmpty())
                    {
                        model.NoteId = Guid.NewGuid().ToString();
                    }
                    if (model.VersionNo > 1)
                    {
                        ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CREATED_AS_NEWVERSION");
                    }
                    else
                    {
                        ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CREATED");
                    }
                    model.NoteEventId = ntsevent.Id;
                    result = await Submit(model);
                    break;
                case "NOTE_STATUS_COMPLETE":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_COMPLETED");
                    model.NoteEventId = ntsevent.Id;
                    result = await CompleteNote(model);
                    break;
                case "NOTE_STATUS_CLOSE":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_CLOSED");
                    model.NoteEventId = ntsevent.Id;
                    result = await CloseNote(model);
                    break;
                case "NOTE_STATUS_EXPIRE":
                    ntsevent = await _lOVBusiness.GetSingle(x => x.Code == "NTS_EVENT_EXPIRED");
                    model.NoteEventId = ntsevent.Id;
                    result = await ExpireNote(model);
                    break;

                default:
                    return await Task.FromResult(CommandResult<NoteTemplateViewModel>.Instance(model, false, "Inavlid Note Action"));
            }

            if (result != null && result.IsSuccess /*&& model.NoteStatusCode != "NOTE_STATUS_DRAFT"*/)
            {
                var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.TableMetadataId);
                var selectQuery = await GetSelectQuery(tableMetaData, @$" and ""NtsNote"".""Id""='{model.NoteId}' limit 1");
                dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                try
                {
                    await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model.NoteStatusCode, model, dobject);

                }
                catch (Exception ex)
                {

                    return CommandResult<NoteTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.Message}");
                }
                //try
                //{
                //    result = await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PreSubmit, model.NoteStatusCode, model, dobject);
                //}
                //catch (Exception ex)
                //{

                //    await RollBackData(existingItem, existingItemTableData, model);
                //    return CommandResult<NoteTemplateViewModel>.Instance(model, false, $"Error in pre submit business rule execution: {ex.Message}");

                //}
                //if (!result.IsSuccess)
                //{
                //    await RollBackData(existingItem, existingItemTableData, model);
                //    return result;
                //}
                //else
                //{
                //    try
                //    {
                //        await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model.NoteStatusCode, model, dobject);

                //    }
                //    catch (Exception ex)
                //    {

                //        return CommandResult<NoteTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.Message}");
                //    }
                //}
                if (result.IsSuccess)
                {
                    //DMS document count logic
                    await ManagePermissionWorkspaceIdAndCount(model);
                    if (existingItem == null || model.NoteStatusId != existingItem.NoteStatusId)
                    {
                        await ManageNotification(result.Item);
                    }
                    try
                    {
                        await UpdateNotificationStatus(model.NoteId, model.NoteStatusCode);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return result;
        }
        private async Task ManagePermissionWorkspaceIdAndCount(NoteTemplateViewModel model)
        {
            model.TemplateViewModel = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
            var templateCategory = await _repo.GetSingleById<TemplateCategoryViewModel, TemplateCategory>(model.TemplateViewModel.TemplateCategoryId);
            if (templateCategory.Code == "GENERAL_FOLDER" || templateCategory.Code == "WORKSPACE_GENERAL" || templateCategory.Code == "GENERAL_DOCUMENT")
            {
                if (model.DataAction == DataActionEnum.Create && model.VersionNo == 1)
                {
                    if (templateCategory.Code == "GENERAL_DOCUMENT")
                    {
                        var _dmsBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();
                        var _tagBusiness = _serviceProvider.GetService<INtsTagBusiness>();
                        var parentlist = await _dmsBusiness.GetAllParentByNoteId(model.NoteId);
                        var taglist = new List<NtsTagViewModel>();
                        foreach (var parent in parentlist)
                        {
                            var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = model.NoteId };
                            taglist.Add(tag);
                            //var res = await _tagBusiness.Create(tag);
                        }
                        if (taglist.Count > 0)
                        {
                            var res = await _tagBusiness.CreateMany(taglist);
                        }
                    }

                }
                if (model.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || model.NoteStatusCode == "NOTE_STATUS_DRAFT")
                {
                    if (templateCategory.Code == "GENERAL_FOLDER" || templateCategory.Code == "WORKSPACE_GENERAL" || templateCategory.Code == "GENERAL_DOCUMENT")
                    {
                        var _documentpermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
                        if (model.DataAction == DataActionEnum.Create)
                        {
                            var permissionData = new DocumentPermissionViewModel
                            {
                                DataAction = DataActionEnum.Create,
                                PermissionType = DmsPermissionTypeEnum.Allow,
                                Access = DmsAccessEnum.FullAccess,
                                AppliesTo = DmsAppliesToEnum.ThisFolderSubFoldersAndFiles,
                                IsInherited = false,
                                PermittedUserId = model.OwnerUserId,
                                NoteId = model.NoteId,
                                Isowner = true,
                            };
                            await _documentpermissionBusiness.Create(permissionData);
                            if (model.ParentNoteId.IsNotNull())
                            {
                                await _documentpermissionBusiness.ManageInheritedPermission(model.NoteId, model.ParentNoteId);
                            }
                        }
                    }
                }
                if (templateCategory.Code == "GENERAL_FOLDER")
                {
                    var noteViewModel = await _repo.GetSingleById(model.NoteId);
                    var workspaceId = await UpdateWorkspaceId(noteViewModel);
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.TemplateCode = model.TemplateCode;
                    noteTempModel.NoteId = model.NoteId;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await GetNoteDetails(noteTempModel);
                    var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                    if (workspaceId.IsNotNullAndNotEmpty())
                    {
                        var _tableMetadataBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
                        var udftabledata = await _tableMetadataBusiness.GetTableDataByColumn("WORKSPACE_GENERAL", "", "NtsNoteId", workspaceId);
                        var k = rowData1.FirstOrDefault(x => x.Key == "WorkspaceId" || x.Key == "workspaceId");
                        if (udftabledata.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                        {
                            rowData1[k.Key] = Convert.ToString(udftabledata["Id"]);
                        }
                    }
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                }
                if (templateCategory.Code == "GENERAL_DOCUMENT")
            {
                var noteViewModel = await _repo.GetSingleById(model.NoteId);
                var workspaceId = await UpdateWorkspaceId(noteViewModel);
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = model.TemplateCode;
                noteTempModel.NoteId = model.NoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                //var isLatestRevision = rowData1.ContainsKey("isLatestRevision") ? Convert.ToString(rowData1["isLatestRevision"]) : "";
                //if (model.NoteStatusCode == "NOTE_STATUS_INPROGRESS" && isLatestRevision != "True" && model.TemplateCode != "GENERAL_DOCUMENT")
                //{
                //    rowData1["isLatestRevision"] = "True";
                //}
                if (workspaceId.IsNotNullAndNotEmpty())
                {
                    var _tableMetadataBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
                    var udftabledata = await _tableMetadataBusiness.GetTableDataByColumn("WORKSPACE_GENERAL", "", "NtsNoteId", workspaceId);
                    var k = rowData1.FirstOrDefault(x => x.Key == "WorkspaceId" || x.Key == "workspaceId");
                    if (udftabledata.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                    {
                        rowData1[k.Key] = Convert.ToString(udftabledata["Id"]);
                    }
                }
                var fileId = rowData1.ContainsKey("fileAttachment") ? Convert.ToString(rowData1["fileAttachment"]) : null;
                if (fileId.IsNotNullAndNotEmpty())
                {
                    var note = await _repo.GetSingleById<NoteViewModel, NtsNote>(model.NoteId);
                    if (note.IsNotNull())
                    {
                        note.DmsAttachmentId = fileId;
                        await Edit(note);
                    }
                    var _ocrMappingBusiness = _serviceProvider.GetService<IOCRMappingBusiness>();
                    var actualList = await _ocrMappingBusiness.GetList(x => x.TemplateId == model.TemplateId);
                    if (actualList.Count > 0)
                    {
                        rowData1 = await _ocrMappingBusiness.GetExtractedData(fileId, actualList, rowData1);
                    }
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                    //var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                    //await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.UpdateOldIsLatestRevision(model.NoteNo, model.NoteId, model.TemplateId, _userContex.ToIdentityUser()));
                }
            }
        }
        public async Task<CommandResult<List<NoteTemplateViewModel>>> ManageBulkNote(List<NoteTemplateViewModel> model)
        {

            CommandResult<List<NoteTemplateViewModel>> result = null;
            //model.TemplateViewModel = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
            //model.NoteTemplateVM = await _repo.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == model.TemplateId);
            // var existingItem = await _repo.GetSingleById(model.NoteId);           

            result = await SaveBulkNote(model);

            if (result != null && result.IsSuccess /*&& model.NoteStatusCode != "NOTE_STATUS_DRAFT"*/)
            {
                foreach (var data in model)
                {
                    await ManagePermissionWorkspaceIdAndCount(data);
                }
                var ids = model.Select(x => x.TemplateId);
                var id = string.Join("','", ids);
                //var tquery = @$"select tm.*,t.""Id"" as TemplateId from public.""TableMetadata"" as tm
                //            join public.""Template"" as t on t.""TableMetadataId""=tm.""Id""
                //            where t.""Id"" in ('{id}') limit 1";
                //var tableMetaDatas = await _queryRepo.ExecuteQueryList<TableMetadataViewModel>(tquery, null);
                var tableMetaDatas = await _ntsQueryBusiness.ManageBulkNoteData(id);

                try
                {
                    foreach (var data in model)
                    {
                        var tableMetaData = tableMetaDatas.Where(x => x.TemplateId == data.TemplateId).FirstOrDefault();
                        var selectQuery = await GetSelectQuery(tableMetaData, @$" and ""NtsNote"".""Id""='{data.NoteId}' limit 1");
                        dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                        await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, data.NoteStatusCode, data, dobject);
                    }

                }
                catch (Exception ex)
                {

                    return CommandResult<List<NoteTemplateViewModel>>.Instance(model, false, $"Error in post submit business rule execution: {ex.Message}");
                }

                if (result.IsSuccess)
                {
                    foreach (var item in result.Item)
                    {
                        await ManageNotification(item);
                    }
                }
            }

            return result;
        }
        private async Task RollBackData(NoteViewModel existingItem, DataRow existingItemTableData, NoteTemplateViewModel model)
        {
            if (existingItem == null)
            {
                await _ntsQueryBusiness.RollBackData1(model);
            }
            else
            {
                if (existingItemTableData != null)
                {
                    var table = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.TableMetadataId);
                    await RollbackUdfTable(table, existingItemTableData.ToDictionary(), model.UdfNoteTableId);
                }
                await _repo.Edit<NoteViewModel, NtsNote>(existingItem);
            }
        }

        public async Task<string> GetUdfQuery(string categoryId, string categoryCode, string templateCodes, string excludeTemplateCodes, params string[] columns)
        {
            var columnList = await _ntsQueryBusiness.GetUdfQueryData(categoryId, categoryCode);

            var tables = columnList.GroupBy(x => x.TableName).Select(x => x.FirstOrDefault()).ToList();
            var queryList = new List<string>();
            var q = "";
            foreach (var table in tables)
            {
                q = @$"select '{table.TemplateId}' as ""TemplateId"",""NtsNoteId"" as ""NtsNoteId"",";
                foreach (var column in columns)
                {
                    var splt = column.Split(',');
                    var col = splt[0];
                    var alias = splt[1];
                    var dbColumn = columnList.FirstOrDefault(x => x.TableName == table.TableName && x.Name == col);
                    if (dbColumn != null)
                    {
                        q = @$"{q} ""{dbColumn.Name}"" as ""{alias}"",";
                    }
                    else
                    {
                        q = @$"{q} null as ""{alias}"",";
                    }
                }
                queryList.Add(@$"{q.Trim(',')} from {table.TableSchemaName}.""{table.TableName}""");
            }
            if (queryList.Any())
            {
                return string.Join($"{Environment.NewLine} union {Environment.NewLine}", queryList);
            }
            return "";
        }
        public async Task ManageNotification(NoteTemplateViewModel viewModel)
        {
            var notificationTemplates = await _repo.GetList<NotificationTemplate, NotificationTemplate>(x => x.TemplateId == viewModel.TemplateId || (x.NtsType == NtsTypeEnum.Note && x.AutoApplyOnAllTemplates), x => x.ParentNotificationTemplate);
            notificationTemplates = notificationTemplates.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            foreach (var item in notificationTemplates)
            {
                var template = item;
                if (item.ParentNotificationTemplate != null)
                {
                    template = item.ParentNotificationTemplate;
                }
                if (template.NotificationActionId == viewModel.NoteStatusId)
                {
                    await CreateNotification(viewModel, template);
                }
            }

        }
        private async Task CreateNotification(NoteTemplateViewModel viewModel, NotificationTemplate item)
        {
            switch (item.NotificationTo)
            {
                case NtsActiveUserTypeEnum.Requester:
                    await SendNotification(viewModel, item, viewModel.RequestedByUserId);
                    break;
                case NtsActiveUserTypeEnum.Owner:
                    await SendNotification(viewModel, item, viewModel.OwnerUserId);
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
        public async Task SendNotification(NoteTemplateViewModel viewModel, NotificationTemplate item, string toUserId)
        {
            if (toUserId != null)
            {
                var notification = new NotificationViewModel
                {
                    DataAction = DataActionEnum.Create,
                    FromUserId = viewModel.OwnerUserId,
                    ToUserId = toUserId,
                    ReferenceType = ReferenceTypeEnum.NTS_Note,
                    ReferenceTypeId = viewModel.NoteId,
                    ReferenceTypeNo = viewModel.NoteNo,
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
                if (item.ActionType == NotificationActionTypeEnum.Action)
                {
                    notification.ActionStatus = NotificationActionStatusEnum.Pending;
                }
                var notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
                await notificationBusiness.Create(notification);
            }
        }
        public async Task UpdateNotificationStatus(string noted, string status)
        {
            var notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
            var list = await notificationBusiness.GetList(x => x.ReferenceType == ReferenceTypeEnum.NTS_Note && x.ReferenceTypeId == noted);
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
        private string SetNotificationUrl(NoteTemplateViewModel viewModel, NotificationTemplate item, string toUserId)
        {
            var customurl = HttpUtility.UrlEncode($@"noteId={viewModel.NoteId}");
            var url = $@"pageName=NoteHome&portalId={_repo.UserContext.PortalId}&pageType=Custom&customUrl={customurl}";
            url = $@"Portal/{viewModel.PortalName}?pageUrl={HttpUtility.UrlEncode(url)}";
            return url;

        }

        private async Task<CommandResult<NoteTemplateViewModel>> ManageBusinessRule(BusinessLogicExecutionTypeEnum executionType, string status, NoteTemplateViewModel viewModel, dynamic viewModelDynamicObject)
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
                    var result = await businessRuleBusiness.ExecuteBusinessRule<NoteTemplateViewModel>(businessRule, TemplateTypeEnum.Note, viewModel, viewModelDynamicObject);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                lov = await _repo.GetSingle<LOVViewModel, LOV>(x => x.Code == "NOTE_STATUS_ALL");
                action = lov?.Id;
                businessRule = await _repo.GetSingle<BusinessRuleViewModel, BusinessRule>(x => x.TemplateId == viewModel.TemplateId
                && x.ActionId == action && x.BusinessLogicExecutionType == executionType);
                if (businessRule != null)
                {
                    try
                    {
                        var businessRuleBusiness = _serviceProvider.GetService<IBusinessRuleBusiness>();
                        var result = await businessRuleBusiness.ExecuteBusinessRule<NoteTemplateViewModel>(businessRule, TemplateTypeEnum.Note, viewModel, viewModelDynamicObject);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);

        }

        private async Task<CommandResult<NoteTemplateViewModel>> CloseNote(NoteTemplateViewModel viewModel)
        {

            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetNoteViewModelStatus(viewModel, "NOTE_STATUS_CLOSE");
            var dm = await _repo.GetSingleById(viewModel.NoteId);
            dm.NoteStatusId = viewModel.NoteStatusId;
            dm.NoteEventId = viewModel.NoteEventId;
            dm.ClosedDate = DateTime.Now;
            dm.CloseComment = viewModel.CloseComment;
            dm.UserRating = viewModel.UserRating;
            var result = await _repo.Edit(dm);
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<NoteTemplateViewModel>> ExpireNote(NoteTemplateViewModel viewModel)
        {
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            await SetNoteViewModelStatus(viewModel, "NOTE_STATUS_EXPIRE");
            var dm = await _repo.GetSingleById(viewModel.NoteId);
            dm.NoteStatusId = viewModel.NoteStatusId;
            dm.NoteEventId = viewModel.NoteEventId;
            dm.ExpiryDate = DateTime.Now;
            var result = await _repo.Edit(dm);
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<NoteTemplateViewModel>> CompleteNote(NoteTemplateViewModel viewModel)
        {

            var validate = await ValidateNote(viewModel);
            if (validate.Count > 0)
            {
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, validate);
            }
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            viewModel.CompletedDate = DateTime.Now;
            return await SaveNote(viewModel, "NOTE_STATUS_COMPLETE");
        }
        private async Task<CommandResult<NoteTemplateViewModel>> Submit(NoteTemplateViewModel viewModel)
        {
            var validate = await ValidateNote(viewModel);
            if (validate.Count > 0)
            {
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, validate);
            }
            var presubmit = await ManagePresubmit(viewModel);
            if (!presubmit.IsSuccess)
            {
                return presubmit;
            }
            return await SaveNote(viewModel, "NOTE_STATUS_INPROGRESS");
        }
        private async Task<CommandResult<NoteTemplateViewModel>> SaveAsDraft(NoteTemplateViewModel viewModel)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            await ManagePresubmit(viewModel);
            return await SaveNote(viewModel, "NOTE_STATUS_DRAFT");
        }

        private async Task<CommandResult<NoteTemplateViewModel>> SaveNote(NoteTemplateViewModel viewModel, string status)
        {
            //await SetUdfDetails(viewModel);
            await SetNoteViewModelStatus(viewModel, status);
            CommandResult<NoteTemplateViewModel> result = null;


            if (viewModel.DataAction == DataActionEnum.Create)
            {
                if (status == "NOTE_STATUS_INPROGRESS")
                {
                    viewModel.SubmittedDate = DateTime.Now;
                }
                result = await CreateNote(viewModel);
                viewModel.VersionNo = result.Item.VersionNo;

            }
            else
            {
                result = await EditNote(viewModel);

            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<List<NoteTemplateViewModel>>> SaveBulkNote(List<NoteTemplateViewModel> viewModel)
        {
            var validate = await ValidateBulkNote(viewModel);
            //CommandResult<List<NoteTemplateViewModel>> result = null;
            var noteStatus = await _repo.GetList<LOVViewModel, LOV>(x => x.LOVType == "LOV_NOTE_STATUS");
            var events = await _lOVBusiness.GetList(x => x.LOVType == "NTS_EVENT");

            if (validate.Count > 0)
            {
                return CommandResult<List<NoteTemplateViewModel>>.Instance(viewModel, false, validate);
            }

            foreach (var model in viewModel)
            {
                var presubmit = await ManagePresubmit(model);
                if (model.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
                {
                    if (!presubmit.IsSuccess)
                    {
                        //return presubmit;
                        return CommandResult<List<NoteTemplateViewModel>>.Instance(viewModel, false, presubmit.Message);
                    }
                }
            }
            var noteNoList = await GenerateBulkNoteNo(viewModel.Count());
            var arrNo = noteNoList.ToArray();
            var i = 0;
            foreach (var model in viewModel)
            {
                model.NoteId = Guid.NewGuid().ToString();
                model.NoteNo = arrNo[i];
                var status = noteStatus.Where(x => x.Code == model.NoteStatusCode).FirstOrDefault();
                if (status != null)
                {
                    model.NoteStatusId = status.Id;
                }
                if (model.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
                {
                    model.SubmittedDate = DateTime.Now;
                }
                i++;
                switch (model.NoteStatusCode)
                {
                    case "NOTE_STATUS_DRAFT":
                        var ntsevent = events.Where(x => x.Code == "NTS_EVENT_DRAFT").FirstOrDefault();
                        model.NoteEventId = ntsevent.Id;
                        break;
                    case "NOTE_STATUS_INPROGRESS":
                        var pevent = events.Where(x => x.Code == "NTS_EVENT_CREATED").FirstOrDefault();
                        model.NoteEventId = pevent.Id;
                        break;

                    case "NOTE_STATUS_EXPIRE":
                        var eevent = events.Where(x => x.Code == "NTS_EVENT_EXPIRED").FirstOrDefault();
                        model.NoteEventId = eevent.Id;
                        break;

                    default:
                        break;
                }

            }

            var note = _autoMapper.Map<List<NoteTemplateViewModel>, List<NoteViewModel>>(viewModel);
            foreach (var data in note)
            {
                data.NoteTemplateId = data.Id;
                data.Id = data.NoteId;
            }
            await _repo.CreateMany(note);
            var udfresult = await BulkInsertNoteUdfTable(viewModel);

            return CommandResult<List<NoteTemplateViewModel>>.Instance(viewModel);
        }


        public async Task<CommandResult<NoteTemplateViewModel>> CreateNote(NoteTemplateViewModel viewModel)
        {
            var note = _autoMapper.Map<NoteTemplateViewModel, NoteViewModel>(viewModel);
            note.Id = viewModel.NoteId;
            note.NoteTemplateId = viewModel.Id;
            var result = await _repo.Create(note);
            var udfresult = await InsertNoteUdfTable(viewModel, viewModel.Json);
            var j = viewModel.UdfNoteTableId;
            return udfresult;
        }
        public async Task<CommandResult<NoteTemplateViewModel>> EditNote(NoteTemplateViewModel viewModel, bool editUdfTable = true)
        {
            var note = _autoMapper.Map<NoteTemplateViewModel, NoteViewModel>(viewModel);
            note.Id = viewModel.NoteId;
            note.NoteTemplateId = viewModel.Id;
            var result = await _repo.Edit(note);
            if (editUdfTable && viewModel.Json != null)
            {
                await EditNoteUdfTable(viewModel, viewModel.Json, viewModel.UdfNoteTableId);
            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        private async Task<Dictionary<string, string>> ValidateNote(NoteTemplateViewModel viewModel)
        {
            var errorList = new Dictionary<string, string>();
            if (viewModel.NoteTemplateVM.NumberGenerationType == NumberGenerationTypeEnum.UserEntry
               && viewModel.NoteTemplateVM.IsNumberNotMandatory == false
               && viewModel.NoteNo.IsNullOrEmpty())
            {
                errorList.Add("NoteNo", string.Concat(viewModel.NoteNo.ToDefaultNoteNumberText(), " is required"));
            }
            if (viewModel.NoteTemplateVM.IsSubjectMandatory && viewModel.NoteSubject.IsNullOrEmpty())
            {
                errorList.Add("NoteSubject", string.Concat(viewModel.SubjectText.ToDefaultSubjectText(), " is required"));
            }
            if (viewModel.NoteTemplateVM.IsSubjectUnique && viewModel.NoteSubject.IsNullOrEmpty())
            {
                bool isexist = await IsNoteSubjectUnique(viewModel.TemplateId, viewModel.NoteSubject, viewModel.NoteId);
                // var notelist = await _repo.GetList(x => x.TemplateId == viewModel.TemplateId);
                // bool isexist = notelist.Any(x => x.NoteSubject.Equals(viewModel.NoteSubject, StringComparison.InvariantCultureIgnoreCase) && x.Id != viewModel.NoteId);
                if (isexist)
                {
                    errorList.Add("NoteSubject", string.Concat("The given ", viewModel.SubjectText.ToDefaultSubjectText(), " already exist. Please enter another ", viewModel.SubjectText.ToDefaultSubjectText()));
                }

            }
            if (viewModel.StartDate == null)
            {
                errorList.Add("StartDate", "Start Date is required");
            }
            if (viewModel.DataAction == DataActionEnum.Create && viewModel.AllowPastStartDate == false && viewModel.StartDate != null && viewModel.StartDate.Value.Date < DateTime.Today)
            {
                errorList.Add("StartDate", "Start Date should be greater than or equal to today's date");
            }
            if ((viewModel.StartDate != null && viewModel.ExpiryDate != null) && (viewModel.StartDate.Value > viewModel.ExpiryDate.Value))
            {
                errorList.Add("DueDate", "Start Date should be less than or equal to due date");
            }
            if (viewModel.NotePriorityId.IsNullOrEmpty())
            {
                errorList.Add("NotePriorityId", "Priority is required");
            }
            await ValidateUniqueUdf(viewModel, errorList);
            return errorList;
        }

        private async Task ValidateUniqueUdf(NoteTemplateViewModel viewModel, Dictionary<string, string> errorList)
        {
            var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == viewModel.TemplateViewModel.TableMetadataId);
            if (tableMetaData != null)
            {
                var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                if (tableColumns != null && tableColumns.Any(x => x.IsUniqueColumn))
                {
                    var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                    foreach (var col in tableColumns.Where(x => x.IsUniqueColumn))
                    {
                        var value = BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType);
                        var query = @$"select ""Id"" from {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" where ""IsDeleted""=false and ""NtsNoteId""<>'{viewModel.NoteId}' and ""{col.Name}""={value} limit 1";
                        var data = await _queryRepo.ExecuteQuerySingle<NoteViewModel>(query, null);
                        if (data != null)
                        {
                            errorList.Add(col.Name, string.Concat("The given value for '", col.LabelName, "' already exist. Please enter another value"));
                        }
                    }
                }
            }
        }
        private async Task<Dictionary<string, string>> ValidateBulkNote(List<NoteTemplateViewModel> viewModelList)
        {
            var errorList = new Dictionary<string, string>();
            var ids = viewModelList.Select(x => x.TemplateId);
            var NoteTemplateVMs = await _repo.GetList<NoteTemplateViewModel, NoteTemplate>(x => ids.Contains(x.TemplateId));
            foreach (var viewModel in viewModelList)
            {
                var NoteTemplateVM = NoteTemplateVMs.Where(x => x.TemplateId == viewModel.TemplateId).FirstOrDefault();
                if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
                {
                    if (NoteTemplateVM.NumberGenerationType == NumberGenerationTypeEnum.UserEntry
                       && NoteTemplateVM.IsNumberNotMandatory == false
                       && viewModel.NoteNo.IsNullOrEmpty())
                    {
                        errorList.Add("NoteNo", string.Concat(viewModel.NoteNo.ToDefaultNoteNumberText(), " is required"));
                    }
                    if (NoteTemplateVM.IsSubjectMandatory && viewModel.NoteSubject.IsNullOrEmpty())
                    {
                        errorList.Add("NoteSubject", string.Concat(viewModel.SubjectText.ToDefaultSubjectText(), " is required"));
                    }
                    if (NoteTemplateVM.IsSubjectUnique && viewModel.NoteSubject.IsNullOrEmpty())
                    {
                        bool isexist = await IsNoteSubjectUnique(viewModel.TemplateId, viewModel.NoteSubject, viewModel.NoteId);
                        // var notelist = await _repo.GetList(x => x.TemplateId == viewModel.TemplateId);
                        // bool isexist = notelist.Any(x => x.NoteSubject.Equals(viewModel.NoteSubject, StringComparison.InvariantCultureIgnoreCase) && x.Id != viewModel.NoteId);
                        if (isexist)
                        {
                            errorList.Add("NoteSubject", string.Concat("The given ", viewModel.SubjectText.ToDefaultSubjectText(), " already exist. Please enter another ", viewModel.SubjectText.ToDefaultSubjectText()));
                        }

                    }
                    if (viewModel.StartDate == null)
                    {
                        errorList.Add("StartDate", "Start Date is required");
                    }
                    if (viewModel.DataAction == DataActionEnum.Create && viewModel.AllowPastStartDate == false && viewModel.StartDate != null && viewModel.StartDate.Value.Date < DateTime.Today)
                    {
                        errorList.Add("StartDate", "Start Date should be greater than or equal to today's date");
                    }
                    if ((viewModel.StartDate != null && viewModel.ExpiryDate != null) && (viewModel.StartDate.Value > viewModel.ExpiryDate.Value))
                    {
                        errorList.Add("DueDate", "Start Date should be less than or equal to due date");
                    }
                    if (viewModel.NotePriorityId.IsNullOrEmpty())
                    {
                        errorList.Add("NotePriorityId", "Priority is required");
                    }
                }
            }

            var id = string.Join("','", ids);

            var tableMetaDatas = await _ntsQueryBusiness.ValidateBulkNoteData(id);
            var metaids = tableMetaDatas.Select(x => x.Id);
            var tableColumn = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => metaids.Contains(x.TableMetadataId));
            foreach (var item in viewModelList)
            {
                if (item.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
                {
                    var tableMetaData = tableMetaDatas.Where(x => x.TemplateId == item.TemplateId).FirstOrDefault();
                    if (tableMetaData != null)
                    {
                        var tableColumns = tableColumn.Where(x => x.TableMetadataId == tableMetaData.Id);
                        if (tableColumns != null && tableColumns.Any(x => x.IsUniqueColumn))
                        {
                            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.Json);
                            foreach (var col in tableColumns.Where(x => x.IsUniqueColumn))
                            {
                                var value = BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType);
                                var query = @$"select ""Id"" from {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" where ""NtsNoteId""<>'{item.NoteId}' and ""{col.Name}""={value} limit 1";
                                var data = await _queryRepo.ExecuteQuerySingle<NoteViewModel>(query, null);
                                if (data != null)
                                {
                                    errorList.Add(col.Name, string.Concat("The given value for '", col.LabelName, "' already exist. Please enter another value"));
                                }
                            }
                        }
                    }
                }
            }
            return errorList;
        }

        private async Task<CommandResult<NoteTemplateViewModel>> InsertNoteUdfTable(NoteTemplateViewModel viewModel, string data)
        {
            try
            {
                if (viewModel != null && viewModel.TemplateViewModel != null)
                {
                    var id = Guid.NewGuid().ToString();
                    var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                    var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == viewModel.TemplateViewModel.TableMetadataId);
                    if (tableMetaData != null)
                    {
                        var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                        if (tableColumns != null && tableColumns.Count > 0)
                        {

                            var selectQuery = new StringBuilder(@$"insert into {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}""");
                            var logQuery = new StringBuilder(@$"insert into {ApplicationConstant.Database.Schema.Log}.""{tableMetaData.Name}Log""");
                            var columnKeys = new List<string>();
                            columnKeys.Add(@"""Id""");
                            var columnValues = new List<object>();
                            columnValues.Add(@$"'{id}'");
                            if (!viewModel.SetUdfValue)
                            {
                                foreach (var col in tableColumns.Where(x => x.IsSystemColumn == false))
                                {
                                    columnKeys.Add(@$"""{ col.Name}""");
                                    columnValues.Add(BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType));

                                }
                            }
                            else
                            {
                                foreach (var col in viewModel.ColumnList.Where(x => x.IsSystemColumn == false))
                                {
                                    columnKeys.Add(@$"""{ col.Name}""");
                                    columnValues.Add(BusinessHelper.ConvertToDbValue(col.Value, col.IsSystemColumn, col.DataType));

                                }
                            }

                            columnKeys.Add(@$"""SequenceOrder""");
                            var sq = rowData.GetValueOrDefault("SequenceOrder");
                            if (sq == null)
                            {
                                sq = 1;
                            }
                            columnValues.Add(sq);
                            columnKeys.Add(@"""NtsNoteId""");
                            columnKeys.Add(@"""CreatedDate""");
                            columnKeys.Add(@"""CreatedBy""");
                            columnKeys.Add(@"""LastUpdatedDate""");
                            columnKeys.Add(@"""LastUpdatedBy""");
                            columnKeys.Add(@"""IsDeleted""");
                            columnKeys.Add(@"""CompanyId""");
                            columnKeys.Add(@"""LegalEntityId""");
                            //columnKeys.Add(@"""SequenceOrder""");
                            columnKeys.Add(@"""Status""");
                            columnKeys.Add(@"""VersionNo""");






                            columnValues.Add(@$"'{viewModel.NoteId}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{viewModel.CreatedBy}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{viewModel.LastUpdatedBy}'");
                            columnValues.Add(@$"false");
                            columnValues.Add(@$"'{viewModel.CompanyId}'");
                            columnValues.Add(@$"'{viewModel.LegalEntityId}'");
                            // columnValues.Add(@$"1");
                            columnValues.Add(@$"{(int)StatusEnum.Active}");
                            columnValues.Add(@$"1");
                            selectQuery.Append(Environment.NewLine);
                            selectQuery.Append($"({string.Join(", ", columnKeys)})");
                            selectQuery.Append(Environment.NewLine);
                            selectQuery.Append($"values ({string.Join(", ", columnValues)})");
                            var queryText = selectQuery.ToString();
                            await _queryRepo.ExecuteCommand(queryText, null);

                            columnKeys.Add(@"""RecordId""");
                            columnKeys.Add(@"""LogVersionNo""");
                            columnKeys.Add(@"""IsLatest""");
                            columnKeys.Add(@"""LogStartDate""");
                            columnKeys.Add(@"""LogEndDate""");
                            columnKeys.Add(@"""LogStartDateTime""");
                            columnKeys.Add(@"""LogEndDateTime""");
                            columnKeys.Add(@"""IsDatedLatest""");
                            columnKeys.Add(@"""IsVersionLatest""");

                            columnValues.Add(@$"'{id}'");
                            columnValues.Add(@$"1");
                            columnValues.Add(@$"true");
                            columnValues.Add(@$"'{DateTime.Today.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{ApplicationConstant.DateAndTime.MaxDate.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{ApplicationConstant.DateAndTime.MaxDate.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"true");
                            columnValues.Add(@$"true");

                            columnValues[0] = @$"'{Guid.NewGuid().ToString()}'";
                            logQuery.Append(Environment.NewLine);
                            logQuery.Append($"({string.Join(", ", columnKeys)})");
                            logQuery.Append(Environment.NewLine);
                            logQuery.Append($"values ({string.Join(", ", columnValues)})");
                            await InsertIntoLogTable(logQuery.ToString(), viewModel, tableMetaData);
                            //await _queryRepo.ExecuteCommand(queryText, null);


                            viewModel.UdfNoteTableId = id;

                        }
                        var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                        var _cmsQueryBusiness = _serviceProvider.GetService<ICmsQueryBusiness>();

                        var childTable = tableColumns.Where(x => x.UdfUIType == UdfUITypeEnum.editgrid || x.UdfUIType == UdfUITypeEnum.datagrid).ToList();
                        foreach (var child in childTable)
                        {
                            var gridData = rowData.GetValueOrDefault(child.Name);
                            if (gridData != null)
                            {
                                //JArray result = JArray.FromObject(gridData);

                                //var jsonG = gridData.ToString();
                               
                                JArray result = JArray.Parse(gridData.ToString());

                                foreach (JObject jcomp in result)
                                {
                                    if (jcomp.Count > 0)
                                    {
                                        jcomp.Remove("ParentId");                                       
                                        var newProperty = new JProperty("ParentId", id);
                                        jcomp.Add(newProperty);
                                        //jcomp.SelectToken("ParentId").Replace(JToken.FromObject(id));
                                        var record = await _cmsBusiness.CreateForm(jcomp.ToString(), null, child.Name);
                                    }
                                    //jcomp.SelectToken("Id").Replace(JToken.FromObject(record.Item2));

                                }
                                //var query = @$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ""{child.Name}""='{result}'
                                //                where ""Id""='{id}' ";

                                //await _cmsQueryBusiness.TableQueryExecute(query);
                            }

                        }

                    }
                    return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
                }
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, "Note Table does not exist.");
            }
            catch (Exception ex)
            {

                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, $"Note table insert error. {ex.Message}");
            }


        }

        private async Task InsertIntoLogTable(string queryText, NoteTemplateViewModel viewModel, TableMetadataViewModel tableMetaData)
        {
            try
            {
                var tableExists = await _ntsQueryBusiness.InsertIntoLogTableData(tableMetaData);
                if (tableExists)
                {
                    await _queryRepo.ExecuteCommand(queryText, null);
                }
                else
                {
                    await CreateLogTable(tableMetaData);
                    await _queryRepo.ExecuteCommand(queryText, null);
                }
            }
            catch (Exception e)
            {

            }

        }

        public async Task CreateLogTable(TableMetadataViewModel tableMetadata)
        {

            var logTableName = $"{tableMetadata.Name}Log";
            var logExists = await _repo.GetList<TableMetadataViewModel, TableMetadata>(x => x.Name == logTableName);
            if (logExists.IsNotNull() && logExists.Any())
            {
                foreach (var tbl in logExists)
                {
                    await _queryRepo.ExecuteCommand(@$"update public.""ColumnMetadata"" set ""IsDeleted""=true where ""TableMetadataId""='{tbl.Id}'", null);
                    await _repo.Delete<TableMetadataViewModel, TableMetadata>(tbl.Id);
                }

            }
            var logTableMetadata = _autoMapper.Map<TableMetadataViewModel, TableMetadataViewModel>(tableMetadata);
            logTableMetadata.Name = logTableMetadata.DisplayName = logTableMetadata.Alias = logTableName;
            logTableMetadata.Id = Guid.NewGuid().ToString();
            logTableMetadata.Schema = ApplicationConstant.Database.Schema.Log;
            logTableMetadata.CreatedDate = logTableMetadata.LastUpdatedDate = DateTime.Now;
            logTableMetadata.CreatedBy = logTableMetadata.LastUpdatedBy = _userContex.UserId;
            var tableResult = await _repo.Create<TableMetadataViewModel, TableMetadata>(logTableMetadata);

            var columns = await _repo.GetList<ColumnMetadata, ColumnMetadata>(x => x.TableMetadataId == tableMetadata.Id);
            var logColumns = new List<ColumnMetadataViewModel>();
            foreach (var item in columns)
            {
                var logColumn = _autoMapper.Map<ColumnMetadata, ColumnMetadataViewModel>(item);
                logColumn.Id = Guid.NewGuid().ToString();
                logColumn.CreatedDate = logColumn.LastUpdatedDate = DateTime.Now;
                logColumn.CreatedBy = logColumn.LastUpdatedBy = _userContex.UserId;
                logColumn.TableMetadataId = logTableMetadata.Id;
                logColumn.ForeignKeyConstraintName = null;
                logColumn.IsForeignKey = false;
                logColumns.Add(logColumn);
            }
            AddLogColumns(logColumns, logTableMetadata.Id);
            await _repo.CreateMany<ColumnMetadataViewModel, ColumnMetadata>(logColumns);
            columns = await _repo.GetList<ColumnMetadata, ColumnMetadata>(x => x.TableMetadataId == logTableMetadata.Id && x.IsVirtualColumn == false);

            var tableVarWithSchema = "<<table-schema>>";
            var tableVar = "<<table>>";

            var query = new StringBuilder();
            var tableExists = await _ntsQueryBusiness.createLogTableData(tableMetadata);
            if (tableExists)
            {
                query.Append($"DROP TABLE {tableVarWithSchema} cascade;");
            }

            query.Append($"CREATE TABLE {tableVarWithSchema}(");
            var pk = "";
            var columnStr = new List<string>();
            foreach (var column in columns.Where(x => !x.IsReferenceColumn))
            {
                var columnText = "";
                var type = ConvertToPostgreType(column);

                if (column.IsPrimaryKey)
                {
                    pk = @$", CONSTRAINT ""PK_{tableVar}"" PRIMARY KEY (""{column.Name}"")";
                }
                //if (column.IsForeignKey && column.IsVirtualForeignKey == false)
                //{
                //    fk += $@"CONSTRAINT ""{column.ForeignKeyConstraintName}"" FOREIGN KEY (""{column.Name}"") REFERENCES {column.ForeignKeyTableSchemaName}.""{column.ForeignKeyTableName}"" (""{column.ForeignKeyColumnName}"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL,";
                //}
                columnText = $"\"{column.Name}\" {type}";
                if (column.DataType == DataColumnTypeEnum.Text)
                {
                    columnText = string.Concat(columnText, $" COLLATE {ApplicationConstant.Database.Schema._Public}.{ApplicationConstant.Database.DefaultCollation}");
                }
                if (!column.IsNullable)
                {
                    columnText = string.Concat(columnText, " NOT NULL");
                }
                columnStr.Add(columnText);
            }
            query.Append(string.Join(string.Concat(",", Environment.NewLine), columnStr));
            query.Append(Environment.NewLine);
            query.Append(pk);
            query.Append(')');
            query.Append(Environment.NewLine);
            query.Append($"TABLESPACE {ApplicationConstant.Database.TableSpace};");
            query.Append(Environment.NewLine);
            query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

            var tableWithSchema = $"{ApplicationConstant.Database.Schema.Log}.\"{logTableMetadata.Name}\"";
            var table = $"{logTableMetadata.Name}";
            var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);
            await _queryRepo.ExecuteCommand(tableQuery, null);

        }
        private async Task<CommandResult<List<NoteTemplateViewModel>>> BulkInsertNoteUdfTable(List<NoteTemplateViewModel> viewModelList)
        {
            var queryList = new List<string>();
            var queryLogList = new List<string>();
            var ids = viewModelList.Select(x => x.TemplateId);
            var tempid = string.Join("','", ids);
            var tableMetaDatas = await _ntsQueryBusiness.BulkInsertNoteUdfTableData(tempid);

            var metaids = tableMetaDatas.Select(x => x.Id);
            var tableColumn = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => metaids.Contains(x.TableMetadataId));
            foreach (var viewModel in viewModelList)
            {
                try
                {

                    var id = Guid.NewGuid().ToString();
                    var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                    //var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == viewModel.TemplateViewModel.TableMetadataId);
                    var tableMetaData = tableMetaDatas.Where(x => x.TemplateId == viewModel.TemplateId).FirstOrDefault();
                    if (tableMetaData != null)
                    {
                        var tableColumns = tableColumn.Where(x => x.TableMetadataId == tableMetaData.Id).ToList();
                        if (tableColumns != null && tableColumns.Count > 0)
                        {

                            var selectQuery = new StringBuilder(@$"insert into {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}""");
                            var logQuery = new StringBuilder(@$"insert into {ApplicationConstant.Database.Schema.Log}.""{tableMetaData.Name}Log""");
                            var columnKeys = new List<string>();
                            columnKeys.Add(@"""Id""");
                            var columnValues = new List<object>();
                            columnValues.Add(@$"'{id}'");
                            if (!viewModel.SetUdfValue)
                            {
                                foreach (var col in tableColumns.Where(x => x.IsSystemColumn == false))
                                {
                                    columnKeys.Add(@$"""{ col.Name}""");
                                    columnValues.Add(BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType));

                                }
                            }
                            else
                            {
                                foreach (var col in viewModel.ColumnList.Where(x => x.IsSystemColumn == false))
                                {
                                    columnKeys.Add(@$"""{ col.Name}""");
                                    columnValues.Add(BusinessHelper.ConvertToDbValue(col.Value, col.IsSystemColumn, col.DataType));

                                }
                            }

                            columnKeys.Add(@$"""SequenceOrder""");
                            var sq = rowData.GetValueOrDefault("SequenceOrder");
                            if (sq == null)
                            {
                                sq = 1;
                            }
                            columnValues.Add(sq);
                            columnKeys.Add(@"""NtsNoteId""");
                            columnKeys.Add(@"""CreatedDate""");
                            columnKeys.Add(@"""CreatedBy""");
                            columnKeys.Add(@"""LastUpdatedDate""");
                            columnKeys.Add(@"""LastUpdatedBy""");
                            columnKeys.Add(@"""IsDeleted""");
                            columnKeys.Add(@"""CompanyId""");
                            columnKeys.Add(@"""LegalEntityId""");
                            //columnKeys.Add(@"""SequenceOrder""");
                            columnKeys.Add(@"""Status""");
                            columnKeys.Add(@"""VersionNo""");






                            columnValues.Add(@$"'{viewModel.NoteId}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{viewModel.CreatedBy}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{viewModel.LastUpdatedBy}'");
                            columnValues.Add(@$"false");
                            columnValues.Add(@$"'{viewModel.CompanyId}'");
                            columnValues.Add(@$"'{viewModel.LegalEntityId}'");
                            // columnValues.Add(@$"1");
                            columnValues.Add(@$"{(int)StatusEnum.Active}");
                            columnValues.Add(@$"1");
                            selectQuery.Append(Environment.NewLine);
                            selectQuery.Append($"({string.Join(", ", columnKeys)})");
                            selectQuery.Append(Environment.NewLine);
                            selectQuery.Append($"values ({string.Join(", ", columnValues)})");
                            var queryText = selectQuery.ToString();
                            queryList.Add(queryText);
                            //await _queryRepo.ExecuteCommand(queryText, null);

                            columnKeys.Add(@"""RecordId""");
                            columnKeys.Add(@"""LogVersionNo""");
                            columnKeys.Add(@"""IsLatest""");
                            columnKeys.Add(@"""LogStartDate""");
                            columnKeys.Add(@"""LogEndDate""");
                            columnKeys.Add(@"""LogStartDateTime""");
                            columnKeys.Add(@"""LogEndDateTime""");
                            columnKeys.Add(@"""IsDatedLatest""");
                            columnKeys.Add(@"""IsVersionLatest""");

                            columnValues.Add(@$"'{id}'");
                            columnValues.Add(@$"1");
                            columnValues.Add(@$"true");
                            columnValues.Add(@$"'{DateTime.Today.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{ApplicationConstant.DateAndTime.MaxDate.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{ApplicationConstant.DateAndTime.MaxDate.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"true");
                            columnValues.Add(@$"true");

                            columnValues[0] = @$"'{Guid.NewGuid().ToString()}'";
                            logQuery.Append(Environment.NewLine);
                            logQuery.Append($"({string.Join(", ", columnKeys)})");
                            logQuery.Append(Environment.NewLine);
                            logQuery.Append($"values ({string.Join(", ", columnValues)})");
                            // await InsertIntoLogTable(logQuery.ToString(), viewModel, tableMetaData);
                            //await _queryRepo.ExecuteCommand(queryText, null);
                            queryLogList.Add(logQuery.ToString());

                            viewModel.UdfNoteTableId = id;

                        }

                    }

                }
                catch (Exception ex)
                {

                    return CommandResult<List<NoteTemplateViewModel>>.Instance(viewModelList, false, $"Note table insert error. {ex.Message}");
                }

            }
            try
            {
                if (queryList.Count > 0)
                {
                    var query = String.Join(";", queryList);
                    await _queryRepo.ExecuteCommand(query, null);
                }
                try
                {
                    if (queryLogList.Count > 0)
                    {
                        var query = String.Join(";", queryLogList);
                        await _queryRepo.ExecuteCommand(query, null);
                    }
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {
                return CommandResult<List<NoteTemplateViewModel>>.Instance(viewModelList, false, $"Note table insert error. {ex.Message}");
            }

            return CommandResult<List<NoteTemplateViewModel>>.Instance(viewModelList);
        }

        private void AddLogColumns(List<ColumnMetadataViewModel> logColumns, string tableMetadataId)
        {

            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "RecordId",
                LabelName = "Record Id",
                Alias = "RecordId",
                DataType = Common.DataColumnTypeEnum.Text,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LogVersionNo",
                LabelName = "LogVersion No",
                Alias = "LogVersionNo",
                DataType = Common.DataColumnTypeEnum.Long,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "IsLatest",
                LabelName = "Is Latest",
                Alias = "IsLatest",
                DataType = Common.DataColumnTypeEnum.Bool,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LogStartDate",
                LabelName = "Log Start Date",
                Alias = "LogStartDate",
                DataType = Common.DataColumnTypeEnum.DateTime,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LogEndDate",
                LabelName = "Log End Date",
                Alias = "LogEndDate",
                DataType = Common.DataColumnTypeEnum.DateTime,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LogStartDateTime",
                LabelName = "Log Start DateTime",
                Alias = "LogStartDateTime",
                DataType = Common.DataColumnTypeEnum.DateTime,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LogEndDateTime",
                LabelName = "Log End DateTime",
                Alias = "LogEndDateTime",
                DataType = Common.DataColumnTypeEnum.DateTime,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "IsDatedLatest",
                LabelName = "Is Dated Latest",
                Alias = "IsDatedLatest",
                DataType = Common.DataColumnTypeEnum.Bool,
                IsSystemColumn = true,
            }, tableMetadataId));
            logColumns.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "IsVersionLatest",
                LabelName = "Is Version Latest",
                Alias = "IsVersionLatest",
                DataType = Common.DataColumnTypeEnum.Bool,
                IsSystemColumn = true,
            }, tableMetadataId));
        }
        private ColumnMetadataViewModel AddColumn(ColumnMetadataViewModel col, string tableMetadataId)
        {
            col.Id = Guid.NewGuid().ToString();
            col.CreatedBy = _repo.UserContext.UserId;
            col.CreatedDate = DateTime.Now;
            col.LastUpdatedBy = _repo.UserContext.UserId;
            col.LastUpdatedDate = DateTime.Now;
            col.IsDeleted = false;
            col.CompanyId = _repo.UserContext.CompanyId;
            col.TableMetadataId = tableMetadataId;
            col.DataAction = DataActionEnum.Create;
            return col;
        }
        private string ConvertToPostgreType(ColumnMetadata column)
        {
            if (column.IsSystemColumn)
            {
                return column.DataType switch
                {
                    DataColumnTypeEnum.Text => "text",
                    DataColumnTypeEnum.Bool => "boolean",
                    DataColumnTypeEnum.DateTime => "timestamp without time zone",
                    DataColumnTypeEnum.Integer => "integer",
                    DataColumnTypeEnum.Double => "double precision",
                    DataColumnTypeEnum.Long => "bigint",
                    DataColumnTypeEnum.TextArray => "text[]",
                    _ => "text",
                };
            }
            else
            {
                return "text";
            }
        }
        public async Task<CommandResult<NoteTemplateViewModel>> EditNoteUdfTable(NoteTemplateViewModel viewModel, string data, string udfNoteTableId)
        {
            try
            {
                if (viewModel != null)
                {
                    var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    if (rowData.ContainsKey("NtsNoteId"))
                    {
                        if (rowData["NtsNoteId"] == null)
                        {
                            rowData["NtsNoteId"] = viewModel.NoteId;
                        }

                    }
                    else
                    {
                        rowData.Add("NtsNoteId", viewModel.NoteId);
                    }
                    var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == viewModel.TableMetadataId);
                    if (tableMetaData != null)
                    {
                        var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                        if (tableColumns != null && tableColumns.Count > 0)
                        {

                            var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}""");
                            selectQuery.Append(Environment.NewLine);
                            selectQuery.Append("set");
                            selectQuery.Append(Environment.NewLine);
                            var columnKeys = new List<string>();
                            foreach (var col in tableColumns.Where(x => x.IsUdfColumn && x.UdfUIType != UdfUITypeEnum.editgrid && x.UdfUIType != UdfUITypeEnum.datagrid))
                            {
                                columnKeys.Add(@$"""{ col.Name}"" = {BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType)}");
                            }

                            var sq = rowData.GetValueOrDefault("SequenceOrder");
                            if (sq == null)
                            {
                                sq = viewModel.SequenceOrder;
                                if (sq == null)
                                {
                                    sq = 1;
                                }
                            }
                            columnKeys.Add(@$"""SequenceOrder"" = {sq}");
                            columnKeys.Add(@$"""VersionNo"" = {viewModel.VersionNo}");
                            columnKeys.Add(@$"""LastUpdatedBy"" = '{viewModel.LastUpdatedBy}'");
                            columnKeys.Add(@$"""LastUpdatedDate"" = '{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnKeys.Add(@$"""LegalEntityId"" = '{viewModel.LegalEntityId}'");

                            selectQuery.Append($"{string.Join(",", columnKeys)}");
                            selectQuery.Append(Environment.NewLine);
                            selectQuery.Append(@$"where  ""Id"" = '{udfNoteTableId}'");
                            var queryText = selectQuery.ToString();
                            await _queryRepo.ExecuteCommand(queryText, null);
                            await EditUdfTableLog(viewModel, tableMetaData, udfNoteTableId, data);

                            //Data Grid
                            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                            var _cmsQueryBusiness = _serviceProvider.GetService<ICmsQueryBusiness>();

                            var childTable = tableColumns.Where(x => x.UdfUIType == UdfUITypeEnum.editgrid || x.UdfUIType == UdfUITypeEnum.datagrid).ToList();
                            foreach (var child in childTable)
                            {
                                var gridData = rowData.GetValueOrDefault(child.Name);
                                if (gridData != null)
                                {
                                    var jsonG = gridData.ToString();
                                    //if (!jsonG.StartsWith("{"))
                                    //{
                                    //    jsonG = string.Concat("{", jsonG, "}");
                                    //}
                                    // var res = JObject.Parse(jsonG);
                                    JArray result = JArray.Parse(gridData.ToString());
                                    var list = JsonConvert.DeserializeObject<IList<IdNameViewModel>>(gridData.ToString());
                                    var ids = list.Select(x => x.Id).ToList();
                                    var tableName = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == child.Name);
                                    var delRec = await _cmsBusiness.GetData(ApplicationConstant.Database.Schema.Cms, tableName.Name, "ParentId", udfNoteTableId);
                                    var delList = (from DataRow dr in delRec.Rows
                                                   where !ids.Contains(dr["Id"].ToString())
                                                   select new IdNameViewModel()
                                                   {
                                                       Id = dr["Id"].ToString(),
                                                   }).ToList();
                                    foreach (var del in delList)
                                    {
                                        await _cmsQueryBusiness.DeleteFrom(new FormTemplateViewModel { RecordId = del.Id }, new TableMetadataViewModel { Name = tableName.Name });
                                    }
                                    foreach (JObject jcomp in result)
                                    {
                                        if (jcomp.Count > 0)
                                        {
                                            var dataId = jcomp.SelectToken("Id");
                                           
                                            if (dataId == null || dataId.ToString().IsNullOrEmpty())
                                            {
                                                jcomp.Remove("ParentId");
                                                var newProperty = new JProperty("ParentId", udfNoteTableId);
                                                jcomp.Add(newProperty);
                                                //jcomp.SelectToken("ParentId").Replace(JToken.FromObject(udfNoteTableId));
                                                var res = await _cmsBusiness.CreateForm(jcomp.ToString(), null, child.Name);
                                                //jcomp.SelectToken("Id").Replace(JToken.FromObject(res.Item2));
                                            }
                                            else
                                            {
                                                var editRec = await _cmsBusiness.GetDataById(TemplateTypeEnum.Form, new PageViewModel { PageType = TemplateTypeEnum.Form, Template = new Template { TableMetadataId = tableName.TableMetadataId } }, dataId.ToString());
                                                if (editRec != null)
                                                {
                                                    var dict = editRec.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => editRec[c]);
                                                    var json = JsonConvert.SerializeObject(dict);
                                                    var js = JObject.Parse(json);
                                                    var tc = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableName.TableMetadataId);
                                                    if (tc != null && tc.Count > 0)
                                                    {
                                                        foreach (var col in tc.Where(x => x.IsUdfColumn))
                                                        {
                                                            js.SelectToken(col.Name).Replace(JToken.FromObject(jcomp.SelectToken(col.Name)));
                                                        }
                                                    }
                                                    await _cmsBusiness.EditForm(dataId.ToString(), js.ToString(), null, child.Name);

                                                }
                                            }
                                        }

                                    }
                                    //var query = @$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ""{child.Name}""='{result}'
                                    //            where ""Id""='{udfNoteTableId}' ";

                                    //await _cmsQueryBusiness.TableQueryExecute(query);
                                }
                            }
                            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);

                        }

                    }
                }
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, "Note viewmodel not found");
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        private async Task EditUdfTableLog(NoteTemplateViewModel viewModel, TableMetadataViewModel tableMetaData, string noteId, string data)
        {

            var tableExists = await _ntsQueryBusiness.EditUdfTableLogData(tableMetaData);
            var log = new LogModel();
            log.RecordId = noteId;
            log.Id = Guid.NewGuid().ToString();
            log.LogVersionNo = 1;
            log.VersionNo = viewModel.VersionNo;
            log.IsLatest = true;
            log.IsVersionLatest = true;
            log.IsDatedLatest = true;
            log.LogStartDate = DateTime.Today;
            log.LogEndDate = ApplicationConstant.DateAndTime.MaxDate;
            log.LogStartDateTime = DateTime.Now;
            log.LogEndDateTime = ApplicationConstant.DateAndTime.MaxDate;
            if (tableExists)
            {
                var exist = await _queryRepo.ExecuteQueryDataRow(
                    @$"select * from log.""{tableMetaData.Name}Log"" where ""RecordId""='{noteId}' and  ""IsLatest"" = true", null);
                if (exist != null)
                {
                    if (log.LogStartDate == Convert.ToDateTime(exist["LogStartDate"]))
                    {
                        log.LogVersionNo = Convert.ToInt64(exist["LogVersionNo"]) + 1;
                        exist["IsDatedLatest"] = false;
                    }
                    else
                    {
                        exist["LogEndDate"] = Convert.ToDateTime(exist["LogStartDate"]).AddDays(-1);
                    }
                    if (log.VersionNo == Convert.ToInt64(exist["VersionNo"]))
                    {
                        exist["IsVersionLatest"] = false;
                    }
                    exist["IsLatest"] = false;
                    var updateQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Log}.""{tableMetaData.Name}Log""");
                    updateQuery.Append(Environment.NewLine);
                    updateQuery.Append("set");
                    updateQuery.Append(Environment.NewLine);
                    var columnKeys = new List<string>();
                    columnKeys.Add(@$"""IsDatedLatest"" = { exist["IsDatedLatest"]}");
                    columnKeys.Add(@$"""LogEndDate"" = '{Convert.ToDateTime(exist["LogEndDate"]).ToDatabaseDateFormat()}'");
                    columnKeys.Add(@$"""IsVersionLatest"" = { exist["IsVersionLatest"]}");
                    columnKeys.Add(@$"""IsLatest"" = { exist["IsLatest"]}");
                    columnKeys.Add(@$"""VersionNo"" = { exist["VersionNo"]}");
                    updateQuery.Append($"{string.Join(",", columnKeys)}");
                    updateQuery.Append(Environment.NewLine);
                    updateQuery.Append(@$"where  ""Id"" = '{exist["Id"]}'");
                    var updateQueryText = updateQuery.ToString();
                    await _queryRepo.ExecuteCommand(updateQueryText, null);
                }
                var queryText = await GetInsertLogQueryInEditMode(viewModel, tableMetaData, noteId, log, data);
                if (queryText.IsNotNullAndNotEmpty())
                {
                    await _queryRepo.ExecuteCommand(queryText, null);
                }

            }
            else
            {
                await CreateLogTable(tableMetaData);
                var queryText = await GetInsertLogQueryInEditMode(viewModel, tableMetaData, noteId, log, data);
                if (queryText.IsNotNullAndNotEmpty())
                {
                    await _queryRepo.ExecuteCommand(queryText, null);
                }
            }
        }

        private async Task<string> GetInsertLogQueryInEditMode(NoteTemplateViewModel viewModel, TableMetadataViewModel tableMetaData, string noteId, LogModel log, string data)
        {
            try
            {
                if (viewModel != null && viewModel.TemplateViewModel != null)
                {
                    var logTableName = $"{tableMetaData.Name}Log";
                    var id = Guid.NewGuid().ToString();
                    var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    if (rowData.ContainsKey("NtsNoteId"))
                    {
                        if (rowData["NtsNoteId"] == null)
                        {
                            rowData["NtsNoteId"] = viewModel.NoteId;
                        }
                    }
                    else
                    {
                        rowData.Add("NtsNoteId", viewModel.NoteId);
                    }
                    var logTableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Name == logTableName);
                    if (tableMetaData != null)
                    {
                        var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == logTableMetaData.Id);
                        if (tableColumns != null && tableColumns.Count > 0)
                        {

                            var logQuery = new StringBuilder(@$"insert into {ApplicationConstant.Database.Schema.Log}.""{logTableMetaData.Name}""");
                            var columnKeys = new List<string>();
                            columnKeys.Add(@"""Id""");
                            var columnValues = new List<object>();
                            columnValues.Add(@$"'{Guid.NewGuid().ToString()}'");
                            foreach (var col in tableColumns.Where(x => x.IsSystemColumn == false))
                            {
                                columnKeys.Add(@$"""{ col.Name}""");
                                columnValues.Add(BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType));

                            }
                            columnKeys.Add(@"""NtsNoteId""");
                            columnKeys.Add(@"""CreatedDate""");
                            columnKeys.Add(@"""CreatedBy""");
                            columnKeys.Add(@"""LastUpdatedDate""");
                            columnKeys.Add(@"""LastUpdatedBy""");
                            columnKeys.Add(@"""IsDeleted""");
                            columnKeys.Add(@"""CompanyId""");
                            columnKeys.Add(@"""LegalEntityId""");
                            columnKeys.Add(@"""SequenceOrder""");
                            columnKeys.Add(@"""Status""");
                            columnKeys.Add(@"""VersionNo""");
                            columnValues.Add(@$"'{viewModel.NoteId}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{viewModel.CreatedBy}'");
                            columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{viewModel.LastUpdatedBy}'");
                            columnValues.Add(@$"false");
                            columnValues.Add(@$"'{viewModel.CompanyId}'");
                            columnValues.Add(@$"'{viewModel.LegalEntityId}'");
                            columnValues.Add(@$"1");
                            columnValues.Add(@$"{(int)StatusEnum.Active}");
                            columnValues.Add(@$"{log.VersionNo}");

                            columnKeys.Add(@"""RecordId""");
                            columnKeys.Add(@"""LogVersionNo""");
                            columnKeys.Add(@"""IsLatest""");
                            columnKeys.Add(@"""LogStartDate""");
                            columnKeys.Add(@"""LogEndDate""");
                            columnKeys.Add(@"""LogStartDateTime""");
                            columnKeys.Add(@"""LogEndDateTime""");
                            columnKeys.Add(@"""IsDatedLatest""");
                            columnKeys.Add(@"""IsVersionLatest""");

                            columnValues.Add(@$"'{log.RecordId}'");
                            columnValues.Add(@$"{log.LogVersionNo}");
                            columnValues.Add(@$"{log.IsLatest}");
                            columnValues.Add(@$"'{log.LogStartDate.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{log.LogEndDate.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{log.LogStartDateTime.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"'{log.LogEndDateTime.ToDatabaseDateFormat()}'");
                            columnValues.Add(@$"{log.IsDatedLatest}");
                            columnValues.Add(@$"{log.IsVersionLatest}");

                            logQuery.Append(Environment.NewLine);
                            logQuery.Append($"({string.Join(", ", columnKeys)})");
                            logQuery.Append(Environment.NewLine);
                            logQuery.Append($"values ({string.Join(", ", columnValues)})");
                            return logQuery.ToString();

                        }

                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task RollbackUdfTable(TableMetadata tableMetaData, Dictionary<string, object> rowData, string recordId)
        {
            try
            {
                if (tableMetaData != null)
                {
                    var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                    if (tableColumns != null && tableColumns.Count > 0)
                    {

                        var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}""");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append("set");
                        selectQuery.Append(Environment.NewLine);
                        var columnKeys = new List<string>();
                        foreach (var col in tableColumns.Where(x => x.IsVirtualColumn == false))
                        {
                            columnKeys.Add(@$"""{ col.Name}"" = {BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType)}");
                        }
                        selectQuery.Append($"{string.Join(",", columnKeys)}");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append(@$"where  ""Id"" = '{recordId}'");
                        var queryText = selectQuery.ToString();
                        await _queryRepo.ExecuteCommand(queryText, null);
                    }

                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private async Task SetNoteViewModelStatus(NoteTemplateViewModel viewModel, string statusCode = null)
        {
            statusCode = statusCode ?? viewModel.NoteStatusCode;
            var taskStatus = await _repo.GetSingle<LOVViewModel, LOV>(x => x.LOVType == "LOV_NOTE_STATUS" && x.Code == statusCode);
            if (taskStatus != null)
            {
                viewModel.NoteStatusCode = taskStatus.Code;
                viewModel.NoteStatusId = taskStatus.Id;
            }
        }
        public async Task<NoteTemplateViewModel> GetNoteDetails(NoteTemplateViewModel viewModel)
        {
            var model = new NoteTemplateViewModel();
            if (viewModel.NoteId.IsNullOrEmpty())
            {
                model = await GetNoteTemplateForNewNote(viewModel);
                model.AttachmentIds = viewModel.AttachmentIds;
                model.ActiveUserId = viewModel.ActiveUserId;
                model.DataAction = viewModel.DataAction;
                model.Prms = viewModel.Prms;
                model.Udfs = viewModel.Udfs;
                model.ReadoOnlyUdfs = viewModel.ReadoOnlyUdfs;
                model.NoteId = Guid.NewGuid().ToString();
                model.NoteNo = await GenerateNextNoteNo(model);
                if (viewModel.OwnerUserId.IsNotNullAndNotEmpty())
                {
                    model.OwnerUserId = viewModel.OwnerUserId;
                }
                else
                {
                    model.OwnerUserId = viewModel.ActiveUserId;
                }

                model.ParentNoteId = viewModel.ParentNoteId;
                if (viewModel.RequestedByUserId.IsNotNullAndNotEmpty())
                {
                    model.RequestedByUserId = viewModel.RequestedByUserId;
                }
                else
                {
                    model.RequestedByUserId = viewModel.ActiveUserId;
                }



                var ownerUserId = model.Prms.GetValue("ownerUserId");
                var parentNoteId = model.Prms.GetValue("parentNoteId");
                var requestedByUserId = model.Prms.GetValue("requestedByUserId");
                var subject = model.Prms.GetValue("subject");
                var description = model.Prms.GetValue("description");
                var sequenceOrder = model.Prms.GetValue("sequenceOrder");
                var servicePlusId = model.Prms.GetValue("servicePlusId");

                var parentServiceId = model.Prms.GetValue("parentServiceId");
                var parentTaskId = model.Prms.GetValue("parentTaskId");
                var notePlusId = model.Prms.GetValue("notePlusId");
                if (notePlusId.IsNotNullAndNotEmpty())
                {
                    model.NotePlusId = notePlusId;
                }
                if (subject.IsNotNullAndNotEmpty())
                {
                    model.NoteSubject = subject;
                }
                if (description.IsNotNullAndNotEmpty())
                {
                    model.NoteDescription = description;
                }
                if (ownerUserId.IsNotNullAndNotEmpty())
                {
                    model.OwnerUserId = ownerUserId;
                }
                if (parentNoteId.IsNotNullAndNotEmpty())
                {
                    model.ParentNoteId = parentNoteId;
                }
                if (parentTaskId.IsNotNullAndNotEmpty())
                {
                    model.ParentTaskId = parentTaskId;
                }
                if (parentServiceId.IsNotNullAndNotEmpty())
                {
                    model.ParentServiceId = parentServiceId;
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
                if (model.ActiveUserId == model.RequestedByUserId && model.ActiveUserId == model.OwnerUserId)
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Owner;
                }
                else
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.Requester;
                }
                if (model.AttachmentIds.IsNotNullAndNotEmpty())
                {
                    await CreateAttachments(model);
                }
                GetUdfDetails(model);
            }
            else
            {
                model = await GetNoteTemplateById(viewModel);
                model.RecordId = viewModel.RecordId;
                model.ActiveUserId = viewModel.ActiveUserId;
                model.IncludeSharedList = viewModel.IncludeSharedList;
                model.DataAction = viewModel.DataAction;
                model.Udfs = viewModel.Udfs;
                model.ReadoOnlyUdfs = viewModel.ReadoOnlyUdfs;
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

                else
                {
                    model.ActiveUserType = NtsActiveUserTypeEnum.None;
                }

                var _repoPermission = _serviceProvider.GetService<IDocumentPermissionBusiness>();
                var permissionList = await _repoPermission.GetNotePermissionData(model.NoteId);
                if (permissionList != null && permissionList.Count() > 0)
                {
                    var permittedUser = permissionList.Where(x => x.PermittedUserId == model.ActiveUserId).ToList();
                    if (permittedUser != null && permittedUser.Count() > 0)
                    {
                        model.IsPermittedUser = true;
                    }
                }

                GetUdfDetails(model);
            }
            //will add ntsId to session varibale here
            return model;
        }

        private async Task CreateAttachments(NoteTemplateViewModel model)
        {
            var attachments = model.AttachmentIds.Replace(",", "','");
            await _ntsQueryBusiness.CreateAttachmentsData(model, attachments);


        }

        private async Task<NoteTemplateViewModel> GetNoteTemplateById(NoteTemplateViewModel viewModel)
        {
            var data = new NoteTemplateViewModel();

            var tableMetadata = await _ntsQueryBusiness.GetNoteTemplateByIdData(viewModel);
            if (tableMetadata != null)
            {
                if (viewModel.LogId.IsNotNullAndNotEmpty())
                {
                    var log = await _queryRepo.ExecuteQuerySingleDynamicObject(@$"select ""IsLatest"" from log.""NtsNoteLog"" where  ""Id""='{viewModel.LogId}'", null);
                    if (log != null)
                    {
                        viewModel.IsLogRecord = !(bool)log.IsLatest;
                    }
                }
                var selectQuery = "";
                if (viewModel.IsLogRecord && viewModel.LogId.IsNotNullAndNotEmpty())
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsNote"".""Id""='{viewModel.LogId}' limit 1", null, null, viewModel.IsLogRecord, viewModel.LogId);
                }
                else
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsNote"".""Id""='{viewModel.NoteId}' limit 1", null, null, viewModel.IsLogRecord, viewModel.LogId);

                }

                data = await _queryRepo.ExecuteQuerySingle<NoteTemplateViewModel>(selectQuery, null);
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
        private async Task<NoteTemplateViewModel> GetNoteTemplateForNewNote(NoteTemplateViewModel vm)
        {
            var data = await _ntsQueryBusiness.GetNoteTemplateForNewNoteData(vm);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.TableMetadataId
                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
            }

            return data;
        }
        public async Task<string> GenerateNextNoteNo(NoteTemplateViewModel model)
        {
            if (model.NumberGenerationType != NumberGenerationTypeEnum.SystemGenerated)
            {
                return "";
            }
            string prefix = "N";
            var today = DateTime.Today;

            var nextId = await _ntsQueryBusiness.GenerateNextNoteNoData();
            if (nextId == null)
            {
                nextId = 1;
                await _repo.Create<NtsNoteSequence, NtsNoteSequence>(
                    new NtsNoteSequence
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
        private async Task<List<string>> GenerateBulkNoteNo(long count)
        {
            var list = new List<string>();
            string prefix = "N";
            var today = DateTime.Today;

            var nextId = await _ntsQueryBusiness.GenerateNextNoteNoData(count);
            if (nextId == null)
            {
                nextId = 1;
                await _repo.Create<NtsNoteSequence, NtsNoteSequence>(
                    new NtsNoteSequence
                    {
                        SequenceDate = today,
                        NextId = nextId.Value + count,
                        CreatedBy = _repo.UserContext.UserId,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = _repo.UserContext.UserId,
                        LastUpdatedDate = DateTime.Now,
                        Status = StatusEnum.Active,
                    });
            }
            if (nextId != null)
            {
                var max = nextId.Value + count;
                for (var i = nextId.Value; i < max; i++)
                {
                    list.Add($"{prefix}-{today.ToSequenceNumberFormat()}-{i}");
                }
            }
            return list;
        }
        public async Task<List<string>> GenerateItemSerialNumbers(long count)
        {
            var list = new List<string>();
            string prefix = "N";
            var today = DateTime.Today;

            var nextId = await _ntsQueryBusiness.GenerateNextNoteNoData(count);
            if (nextId == null)
            {
                nextId = 1;
                await _repo.Create<NtsNoteSequence, NtsNoteSequence>(
                    new NtsNoteSequence
                    {
                        SequenceDate = today,
                        NextId = nextId.Value + count,
                        CreatedBy = _repo.UserContext.UserId,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = _repo.UserContext.UserId,
                        LastUpdatedDate = DateTime.Now,
                        Status = StatusEnum.Active,
                    });
            }
            if (nextId != null)
            {
                var max = nextId.Value + count;
                for (var i = nextId.Value; i < max; i++)
                {
                    list.Add($"{today.ToYyyyMmDdFormat()}{i}");
                }
            }
            return list;
        }
        private void GetUdfDetails(NoteTemplateViewModel model)
        {
            if (model.ColumnList == null || !model.ColumnList.Any())
            {
                return;
            }
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var result = JObject.Parse(model.Json);
                var rows = (JArray)result.SelectToken("components");
                ChildComp(rows, model.ColumnList, model);
                result.Remove("components");
                result.Add("components", JArray.FromObject(rows));
                model.Json = result.ToString();
            }
        }
        private void ChildComp(JArray comps, List<ColumnMetadataViewModel> Columns, NoteTemplateViewModel model)
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
                        || type == "signature" || type == "file" || type == "hidden" || type == "datagrid" || type == "editgrid" || (type == "htmlelement" && key == "chartgrid") || (type == "htmlelement" && key == "chartJs"))
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
                                    columnMeta.NtsStatusCode = model.NoteStatusCode;
                                    var isReadonly = false;
                                    if (model.ReadoOnlyUdfs != null && model.ReadoOnlyUdfs.ContainsKey(columnMeta.Name))
                                    {
                                        isReadonly = model.ReadoOnlyUdfs.GetValueOrDefault(columnMeta.Name);
                                    }
                                    var udfValue = new JProperty("udfValue", columnMeta.Value);
                                    jcomp.Add(udfValue);

                                    //Set default value
                                    if (model.Udfs != null && model.Udfs.GetValue(columnMeta.Name) != null)
                                    {
                                        var newProperty = new JProperty("defaultValue", model.Udfs.GetValue(columnMeta.Name));
                                        jcomp.Add(newProperty);
                                    }
                                    if (!columnMeta.IsVisible)
                                    {
                                        var hiddenproperty = jcomp.SelectToken("hidden");
                                        if (hiddenproperty == null)
                                        {
                                            var newProperty = new JProperty("hidden", true);
                                            jcomp.Add(newProperty);
                                        }

                                    }
                                    if (!columnMeta.IsEditable || isReadonly || model.AllReadOnly)
                                    {
                                        var newhiddenproperty = jcomp.SelectToken("disabled");
                                        if (newhiddenproperty == null)
                                        {
                                            var newProperty = new JProperty("disabled", true);
                                            jcomp.Add(newProperty);
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
                                ChildComp(rows, Columns, model);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                ChildComp(rows, Columns, model);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            ChildComp(rows, Columns, model);
                    }
                }
            }
        }
        private void JsonToDictionary(NoteTemplateViewModel model, Dictionary<string, object> dict)
        {
            var jObject = JObject.Parse(model.Json);
            var comps = (JArray)jObject.SelectToken("components");
            if (comps.IsNotNull())
            {
                foreach (JObject jcomp in comps)
                {
                    var typeObj = jcomp.SelectToken("type");
                    if (typeObj.IsNotNull())
                    {
                        var type = typeObj.ToString();
                        if (type == "textfield" || type == "textarea" || type == "number" || type == "password"
                            || type == "selectboxes" || type == "checkbox" || type == "select" || type == "radio"
                            || type == "email" || type == "url" || type == "phoneNumber" || type == "tags"
                            || type == "datetime" || type == "day" || type == "time" || type == "currency" || type == "button"
                            || type == "signature" || type == "file" || type == "hidden")
                        {


                            var reserve = jcomp.SelectToken("reservedKey");
                            if (reserve == null)
                            {
                                var tempmodel = JsonConvert.DeserializeObject<FormFieldViewModel>(jcomp.ToString());
                                var columnId = jcomp.SelectToken("columnMetadataId");
                                if (columnId != null)
                                {
                                    dict.Add(tempmodel.key, tempmodel.value);
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
                                    JsonToDictionary(model, dict);
                            }
                        }
                        else if (type == "table")
                        {
                            JArray cols = (JArray)jcomp.SelectToken("rows");
                            foreach (var col in cols)
                            {
                                JArray rows = (JArray)col.SelectToken("components");
                                if (rows != null)
                                    JsonToDictionary(model, dict);
                            }
                        }
                        else
                        {
                            JArray rows = (JArray)jcomp.SelectToken("components");
                            if (rows != null)
                                JsonToDictionary(model, dict);
                        }
                    }
                }
            }

        }

        public async Task<List<ColumnMetadataViewModel>> GetViewableColumns(TableMetadataViewModel tableMetaData, bool ignoreJoins = false)
        {
            var columns = new List<ColumnMetadataViewModel>();
            var tables = new List<string>();
            var condition = new List<string>();

            var pkColumns = await _ntsQueryBusiness.GetViewableColumnsData(tableMetaData);
            condition.Add(@$"""{tableMetaData.Name}"".""IsDeleted""=false");
            foreach (var item in pkColumns)
            {
                if (item.TableName == tableMetaData.Name && item.IsPrimaryKey)
                {
                    item.Alias = "UdfNoteTableId";
                }
                else if (item.TableName == "NtsNote" && item.IsPrimaryKey)
                {
                    item.Alias = "NoteId";
                }
                else
                {
                    item.Alias = item.Name;
                }

                columns.Add(item);
            }
            if (!ignoreJoins)
            {
                var fks = pkColumns.Where(x => x.TableName == tableMetaData.Name && x.IsUdfColumn && x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty() && x.HideForeignKeyTableColumns == false).ToList();
                if (fks != null && fks.Count() > 0)
                {


                    var result = await _ntsQueryBusiness.GetViewableColumnsData2(tableMetaData);
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
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Code", Alias = "TemplateCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Json", Alias = "Json" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NoteOwnerType", Name = "Code", Alias = "NoteOwnerTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NoteOwnerType", Name = "Name", Alias = "NoteOwnerTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NotePriority", Name = "Code", Alias = "NotePriorityCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NotePriority", Name = "Name", Alias = "NotePriorityName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NoteStatus", Name = "Code", Alias = "NoteStatusCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NoteStatus", Name = "Name", Alias = "NoteStatusName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NoteAction", Name = "Code", Alias = "NoteActionCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NoteAction", Name = "Name", Alias = "NoteActionName" });
            return columns;
        }
        public async Task<List<ColumnMetadataViewModel>> GetNoteViewableColumns()
        {
            var columns = new List<ColumnMetadataViewModel>();
            var tables = new List<string>();
            var condition = new List<string>();

            var pkColumns = await _ntsQueryBusiness.GetNoteViewableColumnsData();
            foreach (var item in pkColumns)
            {

                if (item.TableName == "NtsNote" && item.IsPrimaryKey)
                {
                    item.Alias = "NoteId";
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
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Code", Alias = "TemplateCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "Template", Name = "Json", Alias = "Json" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NoteOwnerType", Name = "Code", Alias = "NoteOwnerTypeCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NoteOwnerType", Name = "Name", Alias = "NoteOwnerTypeName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NotePriority", Name = "Code", Alias = "NotePriorityCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NotePriority", Name = "Name", Alias = "NotePriorityName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NoteStatus", Name = "Code", Alias = "NoteStatusCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NoteStatus", Name = "Name", Alias = "NoteStatusName" });

            columns.Add(new ColumnMetadataViewModel { TableName = "NoteAction", Name = "Code", Alias = "NoteActionCode" });
            columns.Add(new ColumnMetadataViewModel { TableName = "NoteAction", Name = "Name", Alias = "NoteActionName" });
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

            var pkColumns = await GetViewableColumns(tableMetaData, ignoreJoins);
            condition.Add(@$"""{tableMetaData.Name}"".""IsDeleted""=false");

            var noteTemlateViewModel = await _ntsQueryBusiness.GetSelectQueryData(tableMetaData);
            if (noteTemlateViewModel != null && noteTemlateViewModel.EnableLegalEntityFilter)
            {
                condition.Add(@$"""{tableMetaData.Name}"".""LegalEntityId""='{_userContex.LegalEntityId}'");
            }
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
            if (!ignoreJoins)
            {
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
            }

            if (isLog && logId.IsNotNullAndNotEmpty())
            {
                tables.Add(@$" left join log.""NtsNoteLog"" as ""NtsNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""NtsNote"".""RecordId"" and ""{tableMetaData.Name}"".""LogVersionNo""=""NtsNote"".""LogVersionNo""  and ""NtsNote"".""IsDeleted""=false");
            }
            else
            {
                tables.Add(@$" join public.""NtsNote"" as ""NtsNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""NtsNote"".""Id"" and ""NtsNote"".""IsDeleted""=false");
            }

            tables.Add(@$"join public.""Template"" as ""Template"" on ""NtsNote"".""TemplateId""=""Template"".""Id"" and ""Template"".""IsDeleted""=false ");
            tables.Add(@$"join public.""TemplateCategory"" as ""TemplateCategory"" on ""TemplateCategory"".""Id""=""Template"".""TemplateCategoryId"" and ""TemplateCategory"".""IsDeleted""=false ");
            tables.Add(@$"join public.""NoteTemplate"" as ""NoteTemplate"" on ""NoteTemplate"".""TemplateId""=""Template"".""Id"" and ""NoteTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""OwnerUser"" on ""NtsNote"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""RequestedByUser"" on ""NtsNote"".""RequestedByUserId""=""RequestedByUser"".""Id"" and ""RequestedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""CreatedByUser"" on ""NtsNote"".""CreatedBy""=""CreatedByUser"".""Id"" and ""CreatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""UpdatedByUser"" on ""NtsNote"".""LastUpdatedBy""=""UpdatedByUser"".""Id"" and ""UpdatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NoteOwnerType"" on ""NtsNote"".""NoteOwnerTypeId""=""NoteOwnerType"".""Id"" and ""NoteOwnerType"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NotePriority"" on ""NtsNote"".""NotePriorityId""=""NotePriority"".""Id"" and ""NotePriority"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NoteStatus"" on ""NtsNote"".""NoteStatusId""=""NoteStatus"".""Id"" and ""NoteStatus"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NoteAction"" on ""NtsNote"".""NoteActionId""=""NoteAction"".""Id"" and ""NoteAction"".""IsDeleted""=false ");
            var selectQuery = @$"select {string.Join(",", columns)}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            if (where.IsNotNullAndNotEmpty())
            {
                selectQuery = $"{selectQuery} {where}";
            }
            if (returnColumns.IsNotNullAndNotEmpty())

            {
                returnColumns = @$"""{returnColumns.Replace(",", "\",\"")}""";
            }
            //if (ignoreJoins)
            //{
            //    if (returnColumns.IsNullOrEmpty())
            //    {
            //        returnColumns = "*";
            //    }
            //    else
            //    {
            //        returnColumns = @$"""{returnColumns.Replace(",", "\",\"")}""";
            //    }
            //    selectQuery = @$"select {returnColumns} from { tableMetaData.Schema}.""{ tableMetaData.Name}"" where ""IsDeleted""=false ";
            //    if (where.IsNotNullAndNotEmpty())
            //    {
            //        selectQuery = $"{selectQuery} {where}";
            //    }
            //}
            return selectQuery;
        }
        public async Task<DataTable> GetNoteDataTableById(string noteId, TableMetadataViewModel tableMetadata, bool isLog = false, string logId = null)
        {
            if (tableMetadata != null)
            {
                var selectQuery = "";
                if (isLog && logId.IsNotNullAndNotEmpty())
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsNote"".""Id""='{logId}' limit 1", null, null, isLog, logId);
                }
                else
                {
                    selectQuery = await GetSelectQuery(tableMetadata, @$" and ""NtsNote"".""Id""='{noteId}' limit 1");
                }

                var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                return dt;

            }
            return null;
        }




        public async Task GetNoteIndexPageCount(NoteIndexPageTemplateViewModel model, PageViewModel page)
        {

            var query = await _ntsQueryBusiness.GetNoteIndexPageCountData(page);
            var udfjoin = "";
            var template = await _repo.GetSingleById<TemplateViewModel, Template>(page.TemplateId);
            if (template.IsNotNull())
            {
                var udfTableMetadataId = template.TableMetadataId;
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(udfTableMetadataId);
                if (tableMetadata.IsNotNull())
                {
                    udfjoin = $@"join {tableMetadata.Schema}.""{tableMetadata.Name}"" as udf on udf.""NtsNoteId""=t.""Id"" and udf.""IsDeleted""=false";
                }
            }
            query = query.Replace("#UDFJOIN#", udfjoin);
            var result = await _queryRepo.ExecuteQueryList<NoteTemplateViewModel>(query, null);

            var activeUserId = _repo.UserContext.UserId;
            var ownerOrRequester = result.Where(x => x.OwnerUserId == activeUserId || x.RequestedByUserId == activeUserId);
            if (ownerOrRequester != null && ownerOrRequester.Count() > 0)
            {
                model.CreatedOrRequestedByMeDraftCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Sum(x => x.NoteCount);
                model.CreatedOrRequestedByMeInProgreessOverDueCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "NOTE_STATUS_OVERDUE").Sum(x => x.NoteCount);
                model.CreatedOrRequestedByMeCompleteCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Sum(x => x.NoteCount);
                model.CreatedOrRequestedByMeExpiredCancelCloseCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "NOTE_STATUS_CANCEL" || x.NoteStatusCode == "NOTE_STATUS_CLOSE").Sum(x => x.NoteCount);
            }

            var sharedWith = result.Where(x => x.SharedWithUserId == activeUserId);
            if (sharedWith != null && sharedWith.Count() > 0)
            {
                model.SharedWithMeDraftCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Sum(x => x.NoteCount);
                model.SharedWithMeInProgressOverDueCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "NOTE_STATUS_OVERDUE").Sum(x => x.NoteCount);
                model.SharedWithMeCompleteCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Sum(x => x.NoteCount);
                model.SharedWithMeExpiredCancelCloseCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "NOTE_STATUS_CANCEL" || x.NoteStatusCode == "NOTE_STATUS_CLOSE").Sum(x => x.NoteCount);
            }
            var sharedBy = result.Where(x => x.SharedByUserId == activeUserId);
            if (sharedBy != null && sharedBy.Count() > 0)
            {
                model.SharedByMeDraftCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Sum(x => x.NoteCount);
                model.SharedByMeInProgreessOverDueCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "NOTE_STATUS_OVERDUE").Sum(x => x.NoteCount);
                model.SharedByMeCompleteCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Sum(x => x.NoteCount);
                model.SharedByMeExpiredCancelCloseCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "NOTE_STATUS_CANCEL" || x.NoteStatusCode == "NOTE_STATUS_CLOSE").Sum(x => x.NoteCount);
            }
        }
        public async Task<NoteIndexPageTemplateViewModel> GetNoteIndexPageViewModel(PageViewModel page)
        {
            var task = await _repo.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == page.TemplateId);
            if (task != null)
            {
                var model = await _repo.GetSingleById<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate>(task.NoteIndexPageTemplateId);
                //var indexPageColumns = await _repo.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == model.Id, x => x.ColumnMetadata);
                //var cloumns = new List<NoteIndexPageColumnViewModel>();
                //foreach (var item in indexPageColumns)
                //{
                //    item.ColumnName = item.ColumnMetadata.Name;
                //    cloumns.Add(item);
                //}
                var columns = await _repo.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == model.Id, x => x.ColumnMetadata);
                model.SelectedTableRows = columns.OrderBy(x => x.SequenceOrder).ThenBy(x => x.HeaderName).ToList();
                await GetNoteIndexPageCount(model, page);
                return model;
            }
            return null;

        }
        //public async Task<DataTable> GetNoteIndexPageGridDataOld(DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string noteStatusCode)
        //{

        //    var indexPageTemplate = await _repo.GetSingleById<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate>(indexPageTemplateId);
        //    if (indexPageTemplate != null)
        //    {
        //        var template = await _repo.GetSingleById<TemplateViewModel, Template>(indexPageTemplate.TemplateId);
        //        if (template != null)
        //        {
        //            var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(template.TableMetadataId);
        //            if (tableMetadata != null)
        //            {
        //                var selectQuery = await GetSelectQuery(tableMetadata);
        //                var filter = "";
        //                switch (ownerType)
        //                {
        //                    case NtsActiveUserTypeEnum.Requester:
        //                        filter = @$" and (""NtsNote"".""OwnerUserId""<>'{_repo.UserContext.UserId}' and ""NtsNote"".""RequestedByUserId""='{_repo.UserContext.UserId}')";
        //                        break;
        //                    case NtsActiveUserTypeEnum.Owner:
        //                        filter = @$" and (""NtsNote"".""OwnerUserId""='{_repo.UserContext.UserId}')";
        //                        break;
        //                    case NtsActiveUserTypeEnum.SharedWith:
        //                        break;
        //                    case NtsActiveUserTypeEnum.SharedBy:
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                if (noteStatusCode.IsNotNullAndNotEmpty())
        //                {
        //                    filter = @$" {filter} and (""NoteStatus"".""Code""='{noteStatusCode}')";
        //                }
        //                selectQuery = @$"{selectQuery} {filter}";
        //                if (indexPageTemplate.OrderByColumnId.IsNotNullAndNotEmpty())
        //                {
        //                    var orderColumn = await _repo.GetSingleById<ColumnMetadataViewModel, ColumnMetadata>(indexPageTemplate.OrderByColumnId);
        //                    if (orderColumn != null)
        //                    {
        //                        var oderBy = "asc";
        //                        if (indexPageTemplate.OrderBy == OrderByEnum.Descending)
        //                        {
        //                            oderBy = "desc";
        //                        }
        //                        selectQuery = @$"{selectQuery} order by ""{orderColumn.Name}"" {oderBy}";
        //                    }
        //                    else
        //                    {
        //                        selectQuery = @$"{selectQuery} order by ""LastUpdatedDate"" desc";
        //                    }

        //                }
        //                else
        //                {
        //                    selectQuery = @$"{selectQuery} order by ""LastUpdatedDate"" desc";
        //                }
        //                var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
        //                return dt;

        //            }
        //        }
        //    }
        //    return new DataTable();

        //}
        public async Task<DataTable> GetNoteIndexPageGridData(DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string noteStatusCode, string userId = null, bool ignoreJoins = true)
        {
            if (userId.IsNullOrEmpty())
            {
                userId = _repo.UserContext.UserId;
            }
            var indexPageTemplate = await _repo.GetSingleById<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate>(indexPageTemplateId);
            if (indexPageTemplate != null)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(indexPageTemplate.TemplateId);
                if (template != null)
                {
                    var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(template.TableMetadataId);
                    if (tableMetadata != null)
                    {

                        var selectQuery = await GetSelectQuery(tableMetadata, ignoreJoins: ignoreJoins); //await GetSelectQuery(tableMetadata, ignoreJoins:true);
                        var filter = "";
                        switch (ownerType)
                        {

                            case NtsActiveUserTypeEnum.Requester:
                                filter = @$" and (""NtsNote"".""OwnerUserId""<>'{userId}' and ""NtsNote"".""RequestedByUserId""='{userId}')";
                                break;
                            case NtsActiveUserTypeEnum.Owner:
                                filter = @$" and (""NtsNote"".""OwnerUserId""='{userId}')";
                                break;
                            case NtsActiveUserTypeEnum.SharedWith:

                                var sharedwithlist = await _ntsQueryBusiness.GetNoteIndexPageGridData(userId);
                                if (sharedwithlist.Count() > 0)
                                {
                                    var taskIds = string.Join(",", sharedwithlist.Select(x => x.NtsTaskId));
                                    taskIds = taskIds.Replace(",", "','");
                                    filter = @$" and (""NtsNote"".""Id"" IN ('{taskIds}') ) ";
                                }
                                else
                                {
                                    filter = @$" and (""NtsNote"".""Id"" IN ('') ) ";
                                }
                                break;
                            case NtsActiveUserTypeEnum.SharedBy:

                                var sharedbylist = await _ntsQueryBusiness.GetNoteIndexPageGridData1(userId);

                                if (sharedbylist.Count() > 0)
                                {
                                    var noteIds = string.Join(",", sharedbylist.Select(x => x.NtsNoteId));
                                    noteIds = noteIds.Replace(",", "','");
                                    filter = @$" and (""NtsNote"".""Id"" IN ('{noteIds}') ) ";
                                }
                                else
                                {
                                    filter = @$" and (""NtsNote"".""Id"" IN ('') ) ";
                                }
                                break;
                            default:
                                break;
                        }
                        if (noteStatusCode.IsNotNullAndNotEmpty())
                        {
                            var stausItems = noteStatusCode.Split(',');
                            var statusText = "";
                            foreach (var item in stausItems)
                            {
                                statusText = $"{statusText},'{item}'";
                            }

                            filter = @$" {filter} and (""NoteStatus"".""Code"" in ({statusText.Trim(',')}))";
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


        private async Task<List<UserViewModel>> SetSharedList(NoteTemplateViewModel model)
        {
            var list = await _ntsQueryBusiness.SetsharedListData(model);
            return list;
            //new List<UserViewModel> { { new UserViewModel { UserName = "Shafi", Email = "shaficmd@gmail.com" } } ,
            //{ new UserViewModel { UserName = "Shafi2", Email = "shaficmd@gmail.com2" } } };
        }
        public async Task<List<NTSMessageViewModel>> GetNoteMessageList(string userId, string noteId)
        {
            var comments = await _ntsQueryBusiness.GetNoteMessageListData(userId, noteId);
            return comments.OrderByDescending(x => x.SentDate).ToList();
        }

        public async Task GetNtsNoteIndexPageCount(NtsNoteIndexPageViewModel model)
        {

            var result = await _ntsQueryBusiness.GetNtsNoteIndexPageCountData(model);
            var activeUserId = _repo.UserContext.UserId;
            var ownerOrRequester = result.Where(x => x.OwnerUserId == activeUserId || x.RequestedByUserId == activeUserId);
            if (ownerOrRequester != null && ownerOrRequester.Count() > 0)
            {
                model.CreatedOrRequestedByMeDraftCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Sum(x => x.NoteCount);
                model.CreatedOrRequestedByMeInProgreessOverDueCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "NOTE_STATUS_OVERDUE").Sum(x => x.NoteCount);
                model.CreatedOrRequestedByMeCompleteCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Sum(x => x.NoteCount);
                model.CreatedOrRequestedByMeExpiredCancelCloseCount = ownerOrRequester.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "NOTE_STATUS_CANCEL" || x.NoteStatusCode == "NOTE_STATUS_CLOSE").Sum(x => x.NoteCount);
            }

            var sharedWith = result.Where(x => x.SharedWithUserId == activeUserId);
            if (sharedWith != null && sharedWith.Count() > 0)
            {
                model.SharedWithMeDraftCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Sum(x => x.NoteCount);
                model.SharedWithMeInProgressOverDueCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "NOTE_STATUS_OVERDUE").Sum(x => x.NoteCount);
                model.SharedWithMeCompleteCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Sum(x => x.NoteCount);
                model.SharedWithMeExpiredCancelCloseCount = sharedWith.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "NOTE_STATUS_CANCEL" || x.NoteStatusCode == "NOTE_STATUS_CLOSE").Sum(x => x.NoteCount);
            }
            var sharedBy = result.Where(x => x.SharedByUserId == activeUserId);
            if (sharedBy != null && sharedBy.Count() > 0)
            {
                model.SharedByMeDraftCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Sum(x => x.NoteCount);
                model.SharedByMeInProgreessOverDueCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "NOTE_STATUS_OVERDUE").Sum(x => x.NoteCount);
                model.SharedByMeCompleteCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Sum(x => x.NoteCount);
                model.SharedByMeExpiredCancelCloseCount = sharedBy.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "NOTE_STATUS_CANCEL" || x.NoteStatusCode == "NOTE_STATUS_CLOSE").Sum(x => x.NoteCount);
            }
        }

        public async Task<IList<NoteViewModel>> GetNtsNoteIndexPageGridData(DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string noteStatusCode, string categoryCode, string templateCode, string moduleCode, NtsViewTypeEnum? ntsViewType)
        {
            var querydata = await _ntsQueryBusiness.GetNtsNoteIndexPageGridData(ownerType, noteStatusCode, categoryCode, templateCode, moduleCode, ntsViewType);
            return querydata;
        }
        public async Task<DashboardNoteViewModel> NotesDashboardCount(string userId, string type = "ALL", string moduleName = null, string noteTemplateIds = null)
        {

            var note = await _ntsQueryBusiness.NotesDashboardCountData(userId, type = "ALL", moduleName, noteTemplateIds);

            //var _createdByMe = note.Where(x => (type != "ALL" ? (type == "OTHERS" ? (x.ReferenceType != NoteReferenceTypeEnum.Self) : x.ReferenceType == type.ToEnum<NoteReferenceTypeEnum>()) : true)).Count();
            var _createdByMeExpired = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE").Count();
            var _createdByMeActive = note.Where(x => /*x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" ||*/ x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Count();
            var _createdByMeDraft = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Count();
            /*.DistinctBy(x => x.Id).ToList()*/
            ;
            // var notesharebyme = ExecuteCypherList<NoteViewModel>(cypher1, prms1).DistinctBy(x => x.Id).ToList();
            var noteshared = await _ntsQueryBusiness.NotesDashboardCountData1(userId, type = "ALL", moduleName, noteTemplateIds);

            var _shareWithMe = noteshared.Count();
            var _sharedWithMeExpired = noteshared.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE").Count();
            var _sharedWithMeActive = noteshared.Where(x => /*x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" ||*/ x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Count();
            var _sharedWithMeDraft = noteshared.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Count();


            //  var notesharebyme = ExecuteCypherList<NoteViewModel>(cypher2, prms2).Where(x => x.ShareTo != null).ToList();
            //var _sharedByMe = notesharebyme.Where(x => (type != "ALL" ? (type == "OTHERS" ? (x.ReferenceType != NoteReferenceTypeEnum.Self) : x.ReferenceType == type.ToEnum<NoteReferenceTypeEnum>()) : true)).Count();
            var notesharebyme = await _ntsQueryBusiness.NotesDashboardCountData2(userId, type = "ALL", moduleName, noteTemplateIds);
            var _sharedByMeExpired = notesharebyme.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" /*&& (type != "ALL" ? (type == "OTHERS" ? (x.ReferenceType != NoteReferenceTypeEnum.Self) : x.ReferenceType == type.ToEnum<NoteReferenceTypeEnum>()) : true)*/).Count();
            var _sharedByMeActive = notesharebyme.Where(x => /*x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" ||*/ x.NoteStatusCode == "NOTE_STATUS_COMPLETE" /*&& (type != "ALL" ? (type == "OTHERS" ? (x.ReferenceType != NoteReferenceTypeEnum.Self) : x.ReferenceType == type.ToEnum<NoteReferenceTypeEnum>()) : true)*/).Count();
            var _sharedByMeDraft = notesharebyme.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT"/* && (type != "ALL" ? (type == "OTHERS" ? (x.ReferenceType != NoteReferenceTypeEnum.Self) : x.ReferenceType == type.ToEnum<NoteReferenceTypeEnum>()) : true)*/).Count();


            var count = new DashboardNoteViewModel()
            {
                //createdByMe = _createdByMe,
                createdByMeExpired = _createdByMeExpired,
                createdByMeActive = _createdByMeActive,
                createdByMeDraft = _createdByMeDraft,

                // sharedByMe = (long)_sharedByMe,
                sharedByMeExpired = _sharedByMeExpired,
                sharedByMeActive = _sharedByMeActive,
                sharedByMeDraft = _sharedByMeDraft,

                // shareWithMe = (long)_shareWithMe,
                sharedWithMeExpired = _sharedWithMeExpired,
                sharedWithMeActive = _sharedWithMeActive,
                sharedWithMeDraft = _sharedWithMeDraft,

            };
            return count;
        }

        public async Task<IList<NoteViewModel>> GetSearchResult(NoteSearchViewModel searchModel)
        {
            if (searchModel.UserId.IsNullOrEmpty())
            {
                searchModel.UserId = _repo.UserContext.UserId;
            }
            var querydata = await _ntsQueryBusiness.GetSearchResultData(searchModel);
            if (searchModel.ModuleCode.IsNotNullAndNotEmpty())
            {
                var modules = searchModel.ModuleCode.Split(",");
                querydata = querydata.Where(x => x.ModuleCode != null && modules.Any(y => y == x.ModuleCode)).ToList();
            }
            return querydata;
        }
        public async Task<CommandResult<NoteViewModel>> GetQueryValidate(string query)
        {
            var model = new NoteViewModel();
            try
            {
                var queryText = query;
                dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(queryText, null);
                if (dobject == null)
                {
                    return CommandResult<NoteViewModel>.Instance(model, false, $"Error : Invalid Query.");
                }
                return CommandResult<NoteViewModel>.Instance(model);
            }
            catch (Exception ex)
            {
                return CommandResult<NoteViewModel>.Instance(model, false, $"Error : {ex.Message}");
            }
        }


        public async Task<List<SynergySchemaViewModel>> GetSyneryList()
        {

            var result = await _ntsQueryBusiness.GetSyneryListData();
            return result;

        }
        public async Task<DateTime> GetLastUpdatedSynerySchema()
        {

            var result = await _ntsQueryBusiness.GetLastUpdatedSynerySchemaData();
            return result;

        }

        public async Task<SynergySchemaViewModel> GetSynerySchemaById(string Id)
        {

            var result = await _ntsQueryBusiness.GetSynerySchemaByIdData(Id);

            return result;

        }


        public async Task<DataTable> GetQueryResult(string Query)
        {
            var result = await _queryRepo.ExecuteQueryDataTable(Query, null);
            return result;
        }

        public async Task<bool> DeleteSchema(string NoteId)
        {
            var query = await _ntsQueryBusiness.DeleteSchemaData(NoteId);
            return query;
        }
        public async Task<IList<NoteViewModel>> GetAllDashboardMaster()
        {

            var querydata = await _ntsQueryBusiness.GetAllDashboardMasterData();
            return querydata;
        }
        public async Task<IList<DashboardMasterViewModel>> GetAllGridStackDashboard()
        {

            var querydata = await _ntsQueryBusiness.GetAllGridStackDashboardData();
            return querydata;
        }
        public async Task<IList<KanbanBoardViewModel>> GetKanbanBoard()
        {

            var querydata = await _ntsQueryBusiness.GetKanbanBoardData();
            return querydata;
        }
        public async Task<KanbanBoardViewModel> GetKanbanBoardDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetKanbanBoardDetailsData(noteId);
            return querydata;
        }
        public async Task<DashboardMasterViewModel> GetDashboardMasterDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetDashboardMasterDetailsData(noteId);
            return querydata;
        }


        public async Task UpdateModel(string data, string id)
        {
            await _ntsQueryBusiness.UpdateModelData(data, id);

        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemMasterDetails(string noteId)
        {

            var querydata = await _ntsQueryBusiness.GetDashboardItemMasterDetailsData(noteId);
            return querydata;
        }
        public async Task<MapLayerItemViewModel> GetMapLayerItemDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetMapLayerItemDetailsData(noteId);
            return querydata;
        }
        public async Task<List<MapLayerItemViewModel>> GetMapLayerItemList(string parentNoteId)
        {
            var querydata = await _ntsQueryBusiness.GetMapLayerItemListData(parentNoteId);
            return querydata;
        }
        public async Task<WidgetItemViewModel> GetWidgetItemDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetWidgetItemDetailsData(noteId);
            return querydata;
        }
        public async Task<List<WidgetItemViewModel>> GetWidgetItemList(string parentId)
        {
            var querydata = await _ntsQueryBusiness.GetWidgetItemListData(parentId);
            return querydata;
        }
        public async Task<WidgetItemViewModel> GetWidgetItemDetailsByName(string name)
        {
            var querydata = await _ntsQueryBusiness.GetWidgetItemDetailsByNameData(name);
            return querydata;
        }
        public async Task<List<DashboardItemMasterViewModel>> GetDashboardItemMasterList(string parentId)
        {
            var querydata = await _ntsQueryBusiness.GetDashboardItemMasterListData(parentId);
            return querydata;
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsWithChartTemplate(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetDashboardItemDetailsWithChartTemplateData(noteId);
            return querydata;
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsByName(string name)
        {

            var querydata = await _ntsQueryBusiness.GetDashboardItemDetailsByNameData(name);
            return querydata;
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsById(string id)
        {

            var querydata = await _ntsQueryBusiness.GetDashboardItemDetailsByIdData(id);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllChartTemplate()
        {

            var querydata = await _ntsQueryBusiness.GetAllChartTemplateData();
            return querydata;
        }
        public async Task<IdNameViewModel> GetChartTemplateById(string id)
        {

            var querydata = await _ntsQueryBusiness.GetChartTemplateByIdData(id);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetAllDashboardItemDetailsWithDashboard()
        {

            var querydata = await _ntsQueryBusiness.GetAllDashboardItemDetailsWithDashboardData();
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetLibraryDashboardItemDetailsWithDashboard()
        {

            var querydata = await _ntsQueryBusiness.GetLibraryDashboardItemDetailsWithDashboardData();
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetTagIdNameList(string portalId)
        {

            var querydata = await _ntsQueryBusiness.GetTagIdNameListData(portalId);
            return querydata;
        }
        public async Task<List<TagCategoryViewModel>> GetTagCategoryList(string TemplateId)
        {
            var querydata = await _ntsQueryBusiness.GetTagCategoryListData(TemplateId);
            return querydata;
        }
        public async Task<List<TagCategoryViewModel>> GetTagListByCategoryId(string CategoryId)
        {

            var querydata = await _ntsQueryBusiness.GetTagListByCategoryIdData(CategoryId);
            return querydata;
        }
        public async Task<TagCategoryViewModel> GetCategoryByTagId(string tagId)
        {

            var querydata = await _ntsQueryBusiness.GetCategoryByTagIdData(tagId);
            return querydata;
        }
        public async Task<IList<NoteViewModel>> GetNoteDataListByTemplateCode(string templateCode)
        {
            var querydata = await _ntsQueryBusiness.GetNoteDataListByTemplateCodeData(templateCode);
            return querydata;
        }
        public async Task<IList<DirectoryContent>> GetAllDocuments(string parentId)
        {
            var queryData = await _ntsQueryBusiness.GetAllDocumentsData(parentId);
            var list = queryData;
            return list;
        }
        public async Task<List<DirectoryContent>> GetAllChildDocuments(string parentId)
        {
            var queryData = await _ntsQueryBusiness.GetAllChildDocumentsData(parentId);
            var list = queryData;
            return list;
        }
        public async Task<List<DirectoryContent>> GetAllFolderDocuments(string parentId)
        {
            var queryData = await _ntsQueryBusiness.GetAllFolderDocumentsData(parentId);
            var list = queryData;
            return list;
        }
        public async Task<List<DirectoryContent>> GetAllDocumentFiles(string documentId)
        {
            var list = new List<DirectoryContent>();
            var tableName = await _ntsQueryBusiness.GetAllDocumentFilesData(documentId);
            var attchments = await BuildChart(tableName.Json);
            attchments = attchments.TrimEnd(',');
            var fields = attchments.Split(',');
            foreach (var field in fields)
            {
                var queryData = await _ntsQueryBusiness.GetAllDocumentFilesData1(field, tableName.Name, documentId);

                if (queryData.IsNotNull() && queryData.id.IsNotNullAndNotEmpty() && queryData.id != "[]")
                {
                    list.Add(queryData);
                }

            }
            return list;
        }
        private async Task<string> BuildChartJson(JArray comps, string chartCum)
        {
            JToken nodes;
            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, chartCum);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                chartCum = await BuildChartJson(rows, chartCum);
                        }
                    }
                    else if (type == "fieldset")
                    {

                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            chartCum = await BuildChartJson(rows, chartCum);

                    }
                    else if (type == "file")
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                        {
                            chartCum = await BuildChartJson(rows, chartCum);
                        }
                        else
                        {
                            nodes = jcomp["key"];
                            if (nodes == null)
                            {
                                continue;
                            }
                            var key = nodes.Value<string>();
                            chartCum += key + ",";

                        }
                    }
                }
            }
            return chartCum;
        }
        private async Task<string> BuildChart(string json)
        {
            var chartCum = "";
            if (json.IsNotNullAndNotEmpty())
            {
                var data = JToken.Parse(json);
                JArray rows = (JArray)data.SelectToken("components");
                chartCum = await BuildChartJson(rows, chartCum);
            }
            return chartCum;

        }
        public async Task<bool> IsNoteSubjectUnique(string templateId, string subject, string noteId)
        {
            var data = await _ntsQueryBusiness.IsNoteSubjectUniqueData(templateId, subject, noteId);
            return data;
        }
        public async Task<IList<SocialWebsiteViewModel>> GetAllSocialWebsite()
        {
            var querydata = await _ntsQueryBusiness.GetAllSocialWebsiteData();
            return querydata;
        }
        public async Task<IList<FacebookViewModel>> GetFacebookData(string searchStr)
        {
            var querydata = await _ntsQueryBusiness.GetFacebookData(searchStr);
            return querydata;
        }
        public async Task<IList<TwitterViewModel>> GetTwitterData()
        {
            var querydata = await _ntsQueryBusiness.GetTwitterData();
            return querydata;

        }
        public async Task<IList<YoutubeViewModel>> GetYoutubeData()
        {
            var querydata = await _ntsQueryBusiness.GetYoutubeData();
            return querydata;
        }
        public async Task<IList<WhatsAppViewModel>> GetWhatsAppData()
        {
            var querydata = await _ntsQueryBusiness.GetWhatsAppData();
            return querydata;
        }
        public async Task<IList<InstagramViewModel>> GetInstagramData()
        {
            var querydata = await _ntsQueryBusiness.GetInstagramData();
            return querydata;
        }
        public async Task<List<DbConnectionViewModel>> GetDbConnectionData()
        {
            var querydata = await _ntsQueryBusiness.GetDbConnectionData();
            return querydata;
        }
        public async Task<List<ApiConnectionViewModel>> GetApiConnectionData()
        {
            var querydata = await _ntsQueryBusiness.GetApiConnectionData();
            return querydata;
        }
        public async Task<DbConnectionViewModel> GetDbConnectionDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetDbConnectionDetailsData(noteId);
            return querydata;
        }
        public async Task<ApiConnectionViewModel> GetApiConnectionDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetApiConnectionDetailsData(noteId);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetSharedList(string NoteId)
        {
            var querydata = await _ntsQueryBusiness.GetSharedListData(NoteId);
            return querydata;

        }
        public async Task<List<RssFeedViewModel>> GetRssFeedData()
        {
            var querydata = await _ntsQueryBusiness.GetRssFeedData();
            return querydata;
        }
        public async Task<List<RssFeedViewModel>> GetRssFeedDataForScheduling()
        {
            var querydata = await _ntsQueryBusiness.GetRssFeedDataForScheduling();
            return querydata;
        }
        public async Task<List<RssFeedViewModel>> GetRssFeedDataForSchedulingByTemplateCode(string templateCode)
        {
            var querydata = await _ntsQueryBusiness.GetRssFeedDataForSchedulingByTemplateCode(templateCode);
            return querydata;
        }
        public async Task<List<SchedulerSyncViewModel>> GetScheduleSyncData()
        {
            var querydata = await _ntsQueryBusiness.GetScheduleSyncData();
            return querydata;
        }
        public async Task UpdateScheduleSyncData(string id, DateTime trackingDate, string content)
        {

            await _ntsQueryBusiness.UpdateScheduleSyncData(id, trackingDate, content);
        }
        public async Task<RssFeedViewModel> GetRssFeedDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetRssFeedDetailsData(noteId);
            return querydata;
        }
        public async Task<AlertViewModel> GetNotificationALertDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetNotificationALertDetailsData(noteId);
            return querydata;
        }
        public async Task<WatchlistViewModel> GetWatchlistDetails(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetWatchlistDetailsData(noteId);
            return querydata;
        }
        public async Task<List<AlertViewModel>> GetAlertRulelist()
        {
            var querydata = await _ntsQueryBusiness.GetAlertRulelistData();
            return querydata;
        }
        public async Task<List<string>> GetKeywordListByTrackId(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetKeywordListByTrackIdData(noteId);
            return querydata;
        }
        public async Task<List<string>> GetCanAlertKeywordListByTrackId(string noteId)
        {
            var querydata = await _ntsQueryBusiness.GetKeywordListByTrackIdData(noteId);
            return querydata;
        }

        public async Task<IList<MapMarkerViewModel>> GetDistictByState(string stateId)
        {
            var querydata = await _ntsQueryBusiness.GetDistictByStateData(stateId);
            return querydata;
        }
        public async Task<IList<MapMarkerViewModel>> GetPoliceStationByDistict(string districtId)
        {
            var querydata = await _ntsQueryBusiness.GetPoliceStationByDistictData(districtId);
            return querydata;
        }
        public async Task<IList<MapMarkerViewModel>> GetLocationByPoliceStation(string policeStationId)
        {
            var querydata = await _ntsQueryBusiness.GetLocationByPoliceStationData(policeStationId);
            return querydata;
        }
        public async Task UpdateLocation(string id, string count)
        {
            await _ntsQueryBusiness.UpdateLocationData(id, count);
        }
        public async Task UpdatePoliceStation(string id, string count)
        {
            await _ntsQueryBusiness.UpdatePoliceStationData(id, count);
        }
        public async Task UpdateDistrict(string id, string count)
        {
            await _ntsQueryBusiness.UpdateDistrictData(id, count);

        }

        public async Task<IList<MapMarkerViewModel>> GetMarkersByDistrict(string locationId)
        {
            var querydata = await _ntsQueryBusiness.GetMarkersByDistrictData(locationId);
            return querydata;
        }

        public async Task<List<TagCategoryViewModel>> GetTrackList()
        {
            var querydata = await _ntsQueryBusiness.GetTrackListData();
            return querydata;
        }

        public async Task<List<TagCategoryViewModel>> GetKeywordList(string trackId)
        {
            var querydata = await _ntsQueryBusiness.GetKeywordListData(trackId);
            return querydata;
        }

        public async Task<List<NoteViewModel>> GetDocumentByNoteAndRevision(string templateId, string noteno, string revision)
        {
            var templateDetails = await _repo.GetSingleById<TemplateViewModel, Template>(templateId);
            var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(templateDetails.TableMetadataId);
            var revData = await _repo.GetSingle<LOVViewModel, LOV>(x => x.Name == revision && x.LOVType == "REVISION");
            if (templateDetails.IsNotNull() && tableMetadata.IsNotNull() && revData.IsNotNull())
            {
                var querydata = await _ntsQueryBusiness.GetDocumentByNoteAndRevisionData(tableMetadata.Name, templateId, noteno, revData.Id);
                return querydata;
            }
            return null;
        }

        public async Task<List<ColumnMetadataViewModel>> DynamicUdfColumns(string templateId)
        {
            var querydata = await _ntsQueryBusiness.DynamicUdfColumnsData(templateId);
            return querydata;
        }

        public async Task<string> GetServiceWorkflowTemplateId(long id)
        {
            var querydata = await _ntsQueryBusiness.GetServiceWorkflowTemplateId(id);
            return querydata;
        }

        public async Task<string> GetServiceWorkflowTemplateId(string id)
        {
            var templateDetails = await _repo.GetSingleById<TemplateViewModel, Template>(id);
            if (templateDetails.IsNotNull())
            {
                return templateDetails.WorkFlowTemplateId;
            }
            return null;
        }

        public async Task<List<NoteViewModel>> GetWorkspaceDataForAdmin()
        {
            var list = await _repo.GetList(x => x.OwnerUserId == _userContex.UserId && x.TemplateCode == "WORKSPACE_GENERAL");
            //var model = new List<NoteTemplateViewModel>();
            //foreach (var i in list)
            //{
            //    var details = await GetNoteDetails(new NoteTemplateViewModel { NoteId = i.Id, TemplateCode = "WORKSPACE_GENERAL" });
            //    if (details.IsNotNull())
            //    {
            //        model.Add(details);
            //    }
            //}
            return list;
        }


        public async Task<NoteTemplateViewModel> CopyDocument(string copyFromId, string copyToNewParent, bool allowSubFolder, string ToWorkspaceId, string FromWorkspaceId, string ownerUserId)
        {
            bool Isvalid = false;
            // Get Existing Folder
            var note = await GetNoteDetails(new NoteTemplateViewModel { Id = copyFromId });
            note.Id = null;
            note.DataAction = DataActionEnum.Create;
            if (ownerUserId.IsNotNullAndNotEmpty())
            {
                note.OwnerUserId = ownerUserId;
            }
            if (note.NoteSubject == null)
            {
                note.NoteStatusCode = "NOTE_STATUS_DRAFT";
            }
            else
            {
                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            }

            note.ParentNoteId = copyToNewParent;
            //note.WorkspaceId = FromWorkspaceId;
            // Copy attachment UDF values and set them in new folder or Document
            if (note.ColumnList != null)
            {
                var attachment = note.ColumnList.FirstOrDefault(x => x.Name == "attachment");
                if (attachment != null)
                {
                    var newattachmentudf = await _fileBusiness.CopyFile(attachment.Value.ToString());
                    attachment.Value = newattachmentudf.Id.ToString();
                }
                if (ToWorkspaceId != FromWorkspaceId)
                {

                    var workspaceId = note.ColumnList.FirstOrDefault(x => x.Name == "Workspace");
                    if (workspaceId != null)
                    {
                        workspaceId.Value = ToWorkspaceId.ToString();
                    }
                }

            }
            var result = await ManageNote(note);
            return note;

        }

        public async Task<List<JsonViewModel>> GetUdfJsonModel(string json)
        {
            var list = new List<JsonViewModel>();
            if (json.IsNotNullAndNotEmpty())
            {
                var data = JToken.Parse(json);
                JArray rows = (JArray)data.SelectToken("components");
                var jlist = await BuildUdfList(rows, list);
                return jlist;
            }
            return list;
        }

        private async Task<List<JsonViewModel>> BuildUdfList(JArray comps, List<JsonViewModel> list)
        {
            JToken nodes;
            foreach (JObject jcomp in comps)
            {
                var typeObj = jcomp.SelectToken("type");
                if (typeObj.IsNotNull())
                {
                    var type = typeObj.ToString();
                    if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await BuildUdfList(rows, list);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await BuildUdfList(rows, list);
                        }
                    }
                    else if (type == "fieldset")
                    {

                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            await BuildUdfList(rows, list);

                    }
                    else
                    {
                        try
                        {
                            JArray rows = (JArray)jcomp.SelectToken("components");
                            // chartCum += jcomp.ToString() + "##";
                            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            var jlist = new JavaScriptSerializer().Deserialize<JsonViewModel>(jcomp.ToString());
                            list.Add(jlist);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return list;
        }

        public async Task Archive(string id)
        {
            var model = await _repo.GetSingleById(id);
            model.IsArchived = true;
            await _repo.Edit(model);
        }


        public async Task<string> CreateFileForPostAttachment(string fileName, string fileExt, long contentLength)
        {
            var id = Guid.NewGuid().ToString();
            var result = await _fileBusiness.Create(new FileViewModel
            {
                //Id = id,
                DataAction = DataActionEnum.Create,
                AttachmentType = AttachmentTypeEnum.File.ToString(),
                FileName = fileName,
                Path = @"\Posting\" + id + @"\" + id + fileExt,
                FileExtension = fileExt,
                ContentLength = contentLength,
            });
            if (result != null && result.IsSuccess)
            {
                return result.Item.Id;
            }
            return "";
        }
        public async Task<string> GetServiceDocumentId(string serviceId)
        {
            var udfs = await GetUdfQuery(null, "DMS_GALGARSERVICE", null, null, "documentId,documentId");
            var result = await _ntsQueryBusiness.GetServiceDocumentId(serviceId, udfs);
            return result;
        }
        public async Task<IList<NtsNoteCommentViewModel>> GetInlineCommentResult(string NoteId)
        {
            //var _noteBusiness = BusinessHelper.GetInstance<INoteBusiness>();

            var _note = await GetNoteDetails(new NoteTemplateViewModel { NoteId = NoteId });
            List<NtsNoteCommentViewModel> model = new List<NtsNoteCommentViewModel>();
            //List<CommentsViewModel> modelAnnotations = new List<CommentsViewModel>();
            //var _fileBusiness = BusinessHelper.GetInstance<IFileBusiness>();

            if (_note != null)
            {
                var udfs = await GetUdfQuery(null, "DMS_GALGARSERVICE", null, null, "documentId,documentId");
                var files = await _ntsQueryBusiness.GetInlineCommentResultData(NoteId, udfs);


                //if (_note.ColumnList != null)
                //{
                //    var columns = _note.ColumnList.Where(x => x.UdfUIType == UdfUITypeEnum.file);

                //    foreach (var col in columns)
                //    {
                //if (col.Value.IsNotNull())
                //{
                foreach (var col in files)
                {
                    var doc = await _fileBusiness.GetSingleById(col.Id);

                    if (doc != null)
                    {
                        GetFileInlineComment(doc, model);
                    }
                }
                //}
                //    }

                //}

            }
            try
            {
                var allNested = model.SelectMany(x => x.comments).ToList();
                allNested.AddRange(model.ToList());
                return allNested;
            }
            catch (Exception e)
            {
                return model;
            }

        }
        private void GetFileInlineComment(FileViewModel doc, List<NtsNoteCommentViewModel> model)
        {
            if (doc != null)
            {
                //if (doc.AnnotationsText != null && doc.AnnotationsText != string.Empty)
                //{
                //    var json = JArray.Parse(doc.AnnotationsText);
                //    int pageNo = 0;
                //    foreach (var token in json)
                //    {
                //        var modelAnnotations = JsonConvert.DeserializeObject<List<NtsNoteCommentViewModel>>(token.ToString());
                //        pageNo++;
                //        modelAnnotations.ForEach(xz => xz.pageNumber = pageNo);                        
                //        modelAnnotations.ForEach(xz => xz.FileId = doc.Id);
                //        modelAnnotations.ForEach(xz => xz.FileName = doc.FileName);
                //        model.AddRange(modelAnnotations);
                //    }
                //}
                if (doc.AnnotationsText != null && doc.AnnotationsText != string.Empty)
                {
                    try
                    {
                        var json = JObject.Parse(doc.AnnotationsText);
                        {
                            var modelAnnotations2 = ((Newtonsoft.Json.Linq.JObject)json);
                            int pageNo = 0;

                            foreach (var x in modelAnnotations2)
                            {
                                pageNo++;
                                string name = x.Key;
                                JToken value = x.Value;

                                var json1 = JObject.Parse(value.ToString());

                                var modelAnnotations3 = ((Newtonsoft.Json.Linq.JObject)json1);

                                foreach (var y in modelAnnotations3)
                                {
                                    string _name = y.Key;
                                    JToken _value = y.Value;
                                    var modelAnnotations = JsonConvert.DeserializeObject<List<NtsNoteCommentViewModel>>((_value).ToString());
                                    modelAnnotations.ForEach(xz => xz.pageNumber = pageNo);
                                    modelAnnotations.ForEach(xz => xz.FileId = doc.Id);
                                    modelAnnotations.ForEach(xz => xz.FileName = doc.FileName);
                                    model.AddRange(modelAnnotations);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }


                }
            }
        }

        public async Task UpdateOldIsLatestRevision(string noteNo, string noteId, string templateId)
        {
            var notelist = await this.GetList(x => x.NoteSubject == noteNo && x.Id != noteId && x.TemplateId == templateId);

            foreach (var note in notelist)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = note.TemplateCode;
                noteTempModel.NoteId = note.Id;
                noteTempModel.SetUdfValue = true;
                var notemodel = await this.GetNoteDetails(noteTempModel);
                // var rowData1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(notemodel.Json);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                // var isLatestRevision1 = Convert.ToString(rowData1["isLatetRevision"]);
                rowData1["isLatestRevision"] = "False";
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await this.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
        }
        public async Task<NoteViewModel> GetWorkspaceId(string noteId)
        {
            var value = await _ntsQueryBusiness.GetWorkspaceIdData(noteId);
            return value;

        }

        public async Task<string> UpdateWorkspaceId(NoteViewModel model)
        {
            if (model.ParentNoteId.IsNotNullAndNotEmpty())
            {
                var parentNote = await this.GetSingleById(model.ParentNoteId);
                if (parentNote.IsNotNull() && parentNote.TemplateCode == "WORKSPACE_GENERAL")
                {
                    return parentNote.Id;
                }
                else
                {
                    var parent = await UpdateWorkspaceId(parentNote);
                    return parent;
                }


            }

            else
            {
                return model.Id;
            }

            // return "";
        }
        public async Task<bool> DeleteWorkspace(string NoteId)
        {
            var note = await _repo.GetSingleById(NoteId);
            if (note != null)
            {

                await _ntsQueryBusiness.DeleteWorkspaceData(NoteId);

                await Delete(NoteId);

            }
            return false;
        }

        public async Task<string> GetFolderWorkspace(string id)
        {
            var value = await _ntsQueryBusiness.GetFolderWorkspaceData(id);
            return value;
        }

        public async Task<List<NtsLogViewModel>> GetVersionDetails(string noteId)
        {
            var result = await _ntsQueryBusiness.GetVersionDetailsData(noteId);
            return result;

        }
        public async Task<List<NtsViewModel>> GetNoteBookDetails(string noteId)
        {
            List<NtsViewModel> ntslist = new List<NtsViewModel>();
            var data = await GetBookDetails(noteId);
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
        public async Task<NoteTemplateViewModel> GetBookDetails(string noteId)
        {

            var model = await _ntsQueryBusiness.GetBookDetailsData(noteId);
            model.BookItems = await GetBookList(noteId, null, true);
            return model;
        }
        public async Task<List<NtsViewModel>> GetBookList(string noteId, string templateId, bool includeitemDetails = false)
        {

            var list = await _ntsQueryBusiness.GetBookListData(noteId);
            var root = list.FirstOrDefault(x => x.Level == 0);
            ProcessBookList(root, list, includeitemDetails);
            return list.OrderBy(x => x.Level).ThenBy(x => x.ItemNo).ToList();
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
            var list = await _ntsQueryBusiness.GetBookByIdData(id);
            return list;
        }

        public async Task<CommandResult<NoteViewModel>> ManageMoveToParent(NoteViewModel model)
        {
            var moveToparent = await GetBookById(model.MoveToParent);
            var childlist = await GetBookList(model.Id, null);
            var siblings = childlist.Where(x => x.parentId == moveToparent.parentId).ToList();
            var parentDetails = childlist.FirstOrDefault(x => x.Id == moveToparent.parentId);
            var ntsDetails = childlist.FirstOrDefault(x => x.Id == model.NtsId);
            var ntsSiblings = childlist.Where(x => x.parentId == ntsDetails.parentId && x.SequenceOrder > ntsDetails.SequenceOrder).ToList();
            long? seqOrder = null;
            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
            var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
            if (model.MoveType == BookMoveTypeEnum.SameAsLevel && model.MovePostionSeq == MovePostionEnum.Before)
            {
                seqOrder = moveToparent.SequenceOrder;
                var serviceChildlist = siblings.Where(x => x.SequenceOrder >= moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Service).ToList();
                var taskChildlist = siblings.Where(x => x.SequenceOrder >= moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Task).ToList();
                var noteChildlist = siblings.Where(x => x.SequenceOrder >= moveToparent.SequenceOrder && x.NtsType == NtsTypeEnum.Note).ToList();
                foreach (var item in serviceChildlist)
                {
                    var service = await _serviceBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder + 1;
                    await _serviceBusiness.Edit(service);
                }
                foreach (var item in taskChildlist)
                {
                    var task = await _taskBusiness.GetSingleById(item.Id);
                    task.SequenceOrder = task.SequenceOrder + 1;
                    await _taskBusiness.Edit(task);
                }
                foreach (var item in noteChildlist)
                {
                    var note = await GetSingleById(item.Id);
                    note.SequenceOrder = note.SequenceOrder + 1;
                    await Edit(note);
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
                    var service = await _serviceBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder + 1;
                    await _serviceBusiness.Edit(service);
                }
                foreach (var item in taskChildlist)
                {
                    var task = await _taskBusiness.GetSingleById(item.Id);
                    task.SequenceOrder = task.SequenceOrder + 1;
                    await _taskBusiness.Edit(task);
                }
                foreach (var item in noteChildlist)
                {
                    var note = await GetSingleById(item.Id);
                    note.SequenceOrder = note.SequenceOrder + 1;
                    await Edit(note);
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
                var service = await _serviceBusiness.GetSingleById(model.NtsId);
                service.ParentServiceId = parentService;
                service.ParentNoteId = parentNote;
                service.ParentTaskId = parentTask;
                service.SequenceOrder = seqOrder;

                var serviceedit = await _serviceBusiness.Edit(service);

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
                var note = await GetSingleById(model.NtsId);
                note.ParentServiceId = parentService;
                note.ParentNoteId = parentNote;
                note.ParentTaskId = parentTask;
                note.SequenceOrder = seqOrder;
                var edit = await Edit(note);
            }
            foreach (var item in ntsSiblings)
            {
                if (item.NtsType == NtsTypeEnum.Service)
                {
                    var service = await _serviceBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _serviceBusiness.Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Task)
                {
                    var service = await _taskBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _taskBusiness.Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Note)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await Edit(service);
                }
            }

            return CommandResult<NoteViewModel>.Instance(model);
        }

        public async Task<List<NtsBookItemViewModel>> GetBookItemChildList(string serviceId, string noteId, string taskId)
        {
            var list = await _ntsQueryBusiness.GetBookItemChildListData(serviceId, noteId, taskId);
            return list;
        }


        public async Task<List<NtsBookItemViewModel>> GetAllBookItemChild(List<NtsBookItemViewModel> list, string noteId)
        {
            var data = await GetBookById(noteId);
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

        public async Task DeleteServiceBookItem(string noteId, string parentId, NtsTypeEnum parentType)
        {
            var list = new List<NtsBookItemViewModel>();
            var siblinglist = new List<NtsBookItemViewModel>();
            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
            var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
            // list.Add(new NtsBookItemViewModel { Id = serviceId });
            var childlist = await GetAllBookItemChild(list, noteId);
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
            var currentNts = childlist.FirstOrDefault(x => x.Id == noteId);
            siblinglist = siblinglist.Where(x => x.SequenceOrder > currentNts.SequenceOrder).ToList();
            foreach (var item in siblinglist)
            {
                if (item.NtsType == NtsTypeEnum.Service)
                {
                    var service = await _serviceBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _serviceBusiness.Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Task)
                {
                    var service = await _taskBusiness.GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await _taskBusiness.Edit(service);
                }
                else if (item.NtsType == NtsTypeEnum.Note)
                {
                    var service = await GetSingleById(item.Id);
                    service.SequenceOrder = service.SequenceOrder - 1;
                    await Edit(service);
                }
            }
            foreach (var item in childlist)
            {
                if (item.NtsType == NtsTypeEnum.Service)
                {
                    await _serviceBusiness.Delete(item.Id);
                }
                else if (item.NtsType == NtsTypeEnum.Task)
                {
                    await _taskBusiness.Delete(item.Id);
                }
                else if (item.NtsType == NtsTypeEnum.Note)
                {
                    await Delete(item.Id);
                }
            }

        }

        public async Task<NoteTemplateViewModel> GetFormIoData(string templateId, string noteId, string userId)
        {

            var data = await _ntsQueryBusiness.GetFormIoData(noteId);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.TableMetadataId
                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
                data.AllReadOnly = true;
                GetUdfDetails(data);

                var dr = await _ntsQueryBusiness.GetFormIoData1(data.TableName, noteId);
                data.DataJson = dr.ToJson();
            }


            return data;
        }
        public async Task UpdateDocumentCountCustom(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.DataAction == DataActionEnum.Create && viewModel.VersionNo == 1)
            {
                var _tagBusiness = sp.GetService<INtsTagBusiness>();
                var _dmsBusiness = sp.GetService<IDMSDocumentBusiness>();
                var parentlist = await _dmsBusiness.GetAllParentByNoteId(viewModel.NoteId);
                foreach (var parent in parentlist)
                {
                    var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = viewModel.NoteId };
                    var res = await _tagBusiness.Create(tag);
                }
            }
        }

        public async Task<DashboardNoteViewModel> NotesCountForDashboard(string userId, string bookId)
        {
            //            //var cypher = $@"Select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
            //            //                from public.""NtsNote"" as s                         
            //            //                join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false and tr.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}' 
            //            //                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
            //            //                join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}'  and u.""IsDeleted""=false 
            //            //                join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
            //            //               left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
            //            //               where 1=1 and s.""PortalId""='{_userContex.PortalId}' and s.""IsDeleted""=false ";
            //            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType
            // from  public.""Template"" as t 
            // join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
            //and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
            //                             join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 

            //							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
            //left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
            //and n.""PortalId""='{_userContex.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE#

            //union 
            //Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType
            // from  public.""Template"" as t 
            // join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
            //and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
            //                             join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 

            //							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
            //left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
            //and n.""PortalId""='{_userContex.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE# ";
            //            var search = "";
            //            if (bookId.IsNotNullAndNotEmpty())
            //            {
            //                search = $@" and n.""Id""='{bookId}'";
            //            }
            //            cypher = cypher.Replace("#WHERE#", search);
            //            var note = await _queryRepo.ExecuteQueryList(cypher, null);
            var note = await _ntsQueryBusiness.NotesCountForDashboardData(userId, bookId);
            var _createdByMeExpired = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "SERVICE_STATUS_CANCEL").Count();
            var _createdByMeActive = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "SERVICE_STATUS_INPROGRESS" || x.NoteStatusCode == "SERVICE_STATUS_OVERDUE" || x.NoteStatusCode == "SERVICE_STATUS_COMPLETE").Count();
            var _createdByMeDraft = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT" || x.NoteStatusCode == "SERVICE_STATUS_DRAFT").Count();
            var _createdByMeOverdue = note.Where(x => x.NoteStatusCode == "SERVICE_STATUS_OVERDUE").Count();


            var count = new DashboardNoteViewModel()
            {
                //createdByMe = _createdByMe,
                createdByMeExpired = _createdByMeExpired,
                createdByMeActive = _createdByMeActive,
                createdByMeDraft = _createdByMeDraft,
                createdByMeOverdue = _createdByMeOverdue,
            };
            return count;
        }

        public async Task<DashboardNoteViewModel> ProcessBookCountForDashboard(string userId, string bookId)
        {

            var note = await _ntsQueryBusiness.ProcessBookCountForDashboardData(userId, bookId);
            var _createdByMeExpired = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "SERVICE_STATUS_CANCEL").Count();
            var _createdByMeActive = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "SERVICE_STATUS_INPROGRESS").Count();
            var _createdByMeDraft = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT" || x.NoteStatusCode == "SERVICE_STATUS_DRAFT").Count();
            var _createdByMeOverdue = note.Where(x => x.NoteStatusCode == "SERVICE_STATUS_OVERDUE").Count();
            var _createdByMeCompleted = note.Where(x => x.NoteStatusCode == "SERVICE_STATUS_COMPLETE").Count();


            var count = new DashboardNoteViewModel()
            {
                //createdByMe = _createdByMe,
                createdByMeExpired = _createdByMeExpired,
                createdByMeActive = _createdByMeActive,
                createdByMeDraft = _createdByMeDraft,
                createdByMeOverdue = _createdByMeOverdue,
                createdByMeCompleted = _createdByMeCompleted,
            };
            return count;
        }


        public async Task<DashboardNoteViewModel> ProcessStageCountForDashboard(string userId, string bookId)
        {
            //var cypher = $@"Select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
            //                from public.""NtsNote"" as s                         
            //                join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false and tr.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}' 
            //                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
            //                join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}'  and u.""IsDeleted""=false 
            //                join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
            //               left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
            //               where 1=1 and s.""PortalId""='{_userContex.PortalId}' and s.""IsDeleted""=false ";
            var note = await _ntsQueryBusiness.ProcessStageCountForDashboardData(userId, bookId);


            var _createdByMeExpired = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE" || x.NoteStatusCode == "SERVICE_STATUS_CANCEL").Count();
            var _createdByMeActive = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || x.NoteStatusCode == "SERVICE_STATUS_INPROGRESS").Count();
            var _createdByMeDraft = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT" || x.NoteStatusCode == "SERVICE_STATUS_DRAFT").Count();
            var _createdByMeOverdue = note.Where(x => x.NoteStatusCode == "SERVICE_STATUS_OVERDUE").Count();
            var _createdByMeCompleted = note.Where(x => x.NoteStatusCode == "SERVICE_STATUS_COMPLETE").Count();

            var count = new DashboardNoteViewModel()
            {
                //createdByMe = _createdByMe,
                createdByMeExpired = _createdByMeExpired,
                createdByMeActive = _createdByMeActive,
                createdByMeDraft = _createdByMeDraft,
                createdByMeOverdue = _createdByMeOverdue,
                createdByMeCompleted = _createdByMeCompleted,
            };
            return count;
        }

        public async Task<SynergyWebsiteViewModel> GetSynergyWebsite(string id)
        {

            var value = await _ntsQueryBusiness.GetSynergyWebsiteData(id);
            return value;
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsite()
        {
            var list = await _ntsQueryBusiness.GetAllSynergyWebsiteData();

            return list.ToList();
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsiteNote()
        {
            var list = await _ntsQueryBusiness.GetAllSynergyWebsiteNoteData();
            return list.ToList();
        }

        public async Task<List<IdNameViewModel>> GetAllBookChild(List<IdNameViewModel> list, string noteId)
        {
            var child = await GetList(x => x.ParentNoteId == noteId);
            foreach (var item in child)
            {
                list.Add(new IdNameViewModel { Id = item.Id });
                await GetAllBookChild(list, item.Id);
            }
            return list;
        }

        public async Task DeleteNoteBook(string noteId)
        {
            var list = new List<IdNameViewModel>();
            list.Add(new IdNameViewModel { Id = noteId });
            var childlist = await GetAllBookChild(list, noteId);
            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
            var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
            foreach (var child in childlist)
            {
                var servicePlus = await GetList(x => x.NotePlusId == child.Id);
                var taskPlus = await _taskBusiness.GetList(x => x.NotePlusId == child.Id);
                var notePlus = await _serviceBusiness.GetList(x => x.NotePlusId == child.Id);
                foreach (var item in notePlus)
                {
                    await Delete(item.Id);
                }
                foreach (var item in taskPlus)
                {
                    await _taskBusiness.Delete(item.Id);
                }
                foreach (var item in servicePlus)
                {
                    await _serviceBusiness.Delete(item.Id);
                }
                await Delete(child.Id);
            }

        }
        public async Task<List<MeasuresViewModel>> GetMeasures()
        {

            var querydata = await _ntsQueryBusiness.GetMeasuresData();
            return querydata;
        }
        public async Task<List<MeasuresViewModel>> GetMeasuresDisplay()
        {
            var querydata = await _ntsQueryBusiness.GetMeasuresDataDisplay();
            return querydata;
        }
        public async Task<List<DimensionsViewModel>> GetDimensions()
        {
            var querydata = await _ntsQueryBusiness.GetDimensionsData();
            return querydata;
        }
        public async Task<List<DimensionsViewModel>> GetDimensionsByMeasue(string measure)
        {

            var querydata = await _ntsQueryBusiness.GetDimensionsByMeasueData(measure);
            return querydata;
        }
        public async Task<List<DimensionsViewModel>> GetDimensionsByMeasueDisplay(string measure)
        {

            var querydata = await _ntsQueryBusiness.GetDimensionsByMeasueDataDisplay(measure);
            return querydata;
        }
        public async Task<List<DimensionsColumnViewModel>> GetDimensionsColumnByMeasue(string measure)
        {

            var querydata = await _ntsQueryBusiness.GetDimensionsColumnByMeasueData(measure);
            return querydata;
        }

        public async Task<bool> DeleteAor(NoteTemplateViewModel model)
        {
            var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""N_HR_BusinessHierarchyAOR"" set ");
            selectQuery.Append(Environment.NewLine);
            selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
            selectQuery.Append(@$"where ""NtsNoteId""='{model.NoteId}';{Environment.NewLine}");
            selectQuery.Append(@$"update public.""NtsNote"" set ");
            selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
            selectQuery.Append(@$"where ""Id""='{model.NoteId}'");
            var queryText = selectQuery.ToString();
            await _queryRepo.ExecuteCommand(queryText, null);
            return true;
        }
        public async Task<IList<SocailScrappingApiViewModel>> GetAllCCTNSApiMethods()
        {
            var querydata = await _ntsQueryBusiness.GetAllCCTNSApiMethodsData();
            return querydata;
        }
        public async Task<SocailScrappingApiViewModel> GetCCTNSApiMethodsDetails(string id)
        {
            var querydata = await _ntsQueryBusiness.GetAllCCTNSApiMethodsDetails(id);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllDistrict()
        {
            var querydata = await _ntsQueryBusiness.GetAllDistrictData();
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllVDPDistrict()
        {
            var querydata = await _ntsQueryBusiness.GetAllVDPDistrictData();
            return querydata;
        }
        public async Task<IdNameViewModel> GetVDPDistrictByCode(string code)
        {
            var querydata = await _ntsQueryBusiness.GetVDPDistrictDataByCode(code);
            return querydata;
        }
        public async Task<IdNameViewModel> GetVDPDistrictById(string id)
        {
            var querydata = await _ntsQueryBusiness.GetVDPDistrictDataById(id);
            return querydata;
        }
        public async Task<SchedulerLogViewModel> GetSchedulerLog(string subject, string districtCode)
        {
            var querydata = await _ntsQueryBusiness.GetSchedulerLogData(subject, districtCode);
            return querydata;
        }
        public async Task<IList<SocailScrappingApiParameterViewModel>> GetAllCCTNSApiMethodsParameter(string parameterIds)
        {
            var querydata = await _ntsQueryBusiness.GetAllCCTNSApiMethodsParameterData(parameterIds);
            return querydata;
        }
        public async Task<List<IIPCameraViewModel>> GetIIPCamera()
        {
            var querydata = await _ntsQueryBusiness.GetIIPCameraData();
            return querydata;
        }
        public async Task<List<IIPCameraViewModel>> GetIIPCameraByIds(string ids)
        {
            var querydata = await _ntsQueryBusiness.GetIIPCameraDatabyIds(ids);
            return querydata;
        }
        public async Task<List<CctvCameraViewModel>> GetCctvCamera(DateTime? lastUpdatedDate = null)
        {
            var querydata = await _ntsQueryBusiness.GetCctvCameraData(lastUpdatedDate);
            return querydata;
        }
        public async Task TriggerHangfire(AlertViewModel model)
        {
            Hangfire.RecurringJob.RemoveIfExists("AlertJobs-" + model.NoteSubject);
            Hangfire.RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>("AlertJobs-" + model.NoteSubject, x => x.GenerateAlert(model.Id), model.evaluateTime);
        }
        public async Task ManageSchedulerLog(SchedulerLogViewModel model)
        {
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = model.ActiveUserId;
            templateModel.DataAction = DataActionEnum.Create;
            templateModel.TemplateCode = "SCHEDULER_LOG";
            var newmodel = await GetNoteDetails(templateModel);
            newmodel.NoteSubject = model.NoteSubject;
            newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var result = await ManageNote(newmodel);

        }
        //public async Task<List<RoipViewModel>> GetRoipData(string host, string user, string database, string port, string password, string table)
        //{
        //    var list = await _ntsQueryBusiness.GetRoipData(host, user, database, port, password, table);
        //    return list;
        //}
        public async Task<IList<TrendingLocationViewModel>> GetAllTrendingLocation()
        {

            var querydata = await _ntsQueryBusiness.GetAllTrendingLocationData();
            return querydata;
        } 
        public async Task<IList<string>> GetAllKeywordForHarvesting()
        {

            var querydata = await _ntsQueryBusiness.GetAllKeywordForHarvesting();
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllFacebookUser()
        {

            var querydata = await _ntsQueryBusiness.GetAllFacebookUserData();
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllDial100Event()
        {

            var querydata = await _ntsQueryBusiness.GetAllDial100Event();
            return querydata;
        } 
        public async Task<IList<IdNameViewModel>> GetAllDial100SubEvent()
        {

            var querydata = await _ntsQueryBusiness.GetAllDial100SubEvent();
            return querydata;
        } 
        public async Task<IList<IdNameViewModel>> GetAllDial100SubEvent(string eventCode)
        {

            var querydata = await _ntsQueryBusiness.GetAllDial100SubEvent(eventCode);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllTracks()
        {

            var querydata = await _ntsQueryBusiness.GetAllTracks();
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllKeywords(string trackName)
        {

            var querydata = await _ntsQueryBusiness.GetAllKeywords(trackName);
            return querydata;
        }
        public async Task<IdNameViewModel> GetFacebookCredential()
        {

            var querydata = await _ntsQueryBusiness.GetFacebookCredentialData();
            return querydata;
        }
        public async Task<bool> CreateGeneralDocument(string noteNo, List<string> folders, string fileId, string fileName, string userId)
        {
            if (noteNo.IsNotNullAndNotEmpty())
            {
                var mainFolder = await GetSingle(x => x.NoteNo == noteNo);
                if (mainFolder.IsNotNull())
                {
                    var parentId = mainFolder.Id;
                    foreach (var folder in folders)
                    {
                        var childFolder = await GetSingle(x => x.ParentNoteId == parentId && x.NoteSubject == folder);
                        if (childFolder.IsNotNull())
                        {
                            parentId = childFolder.Id;
                        }
                        else
                        {
                            var templateModel = new NoteTemplateViewModel
                            {
                                TemplateCode = "GENERAL_FOLDER",
                                DataAction = DataActionEnum.Create,
                                OwnerUserId = userId,
                                ActiveUserId = userId,
                                RequestedByUserId = userId,
                                ParentNoteId = parentId,
                                StartDate = DateTime.Now.ApplicationNow().Date
                            };
                            var folderModel = await GetNoteDetails(templateModel);
                            folderModel.NoteSubject = folder;
                            folderModel.NoteDescription = folder;
                            folderModel.ParentNoteId = parentId;
                            folderModel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                            folderModel.Json = "{}";
                            var folderResult = await ManageNote(folderModel);
                            if (folderResult.IsSuccess)
                            {
                                parentId = folderResult.Item.NoteId;
                            }
                        }
                    }
                    var model = new NoteTemplateViewModel
                    {
                        TemplateCode = "GENERAL_DOCUMENT",
                        DataAction = DataActionEnum.Create,
                        OwnerUserId = userId,
                        ActiveUserId = userId,
                        RequestedByUserId = userId,
                        ParentNoteId = parentId,
                        StartDate = DateTime.Now.ApplicationNow().Date,

                    };
                    var newmodel = await GetNoteDetails(model);
                    newmodel.NoteSubject = fileName;
                    newmodel.Description = fileName;
                    newmodel.ParentNoteId = parentId;
                    newmodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    newmodel.Json = "{}";
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    if (fileId.IsNotNullAndNotEmpty())
                    {

                        ((IDictionary<String, Object>)exo).Add("fileAttachment", fileId);
                        newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    }
                    var result = await ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return true;

                    }
                }
            }

            return false;
        }
    }
}
