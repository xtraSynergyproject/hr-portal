using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Kendo.Mvc.UI;
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


namespace CMS.Business
{
    public class NoteBusiness : BusinessBase<NoteViewModel, NtsNote>, INoteBusiness
    {
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileBusiness _fileBusiness;
        private readonly IRepositoryQueryBase<NtsLogViewModel> _queryNtsLog;
        private readonly IUserContext _userContex;
        private readonly ILOVBusiness _lOVBusiness;
        public NoteBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper, IRepositoryQueryBase<NoteViewModel> queryRepo,
            IServiceProvider serviceProvider, IFileBusiness fileBusiness,
        IRepositoryQueryBase<NtsLogViewModel> queryNtsLog, ILOVBusiness lOVBusiness,
            IUserContext userContex) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceProvider = serviceProvider;
            _userContex = userContex;
            _fileBusiness = fileBusiness;
            _queryNtsLog = queryNtsLog;
            _lOVBusiness = lOVBusiness;
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
                var isLatestRevision = rowData1.ContainsKey("isLatestRevision") ? Convert.ToString(rowData1["isLatestRevision"]) : "";
                if (model.NoteStatusCode == "NOTE_STATUS_INPROGRESS" && isLatestRevision != "True" && model.TemplateCode != "GENERAL_DOCUMENT")
                {
                    rowData1["isLatestRevision"] = "True";
                }
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
                var fileId = rowData1.ContainsKey("attachment") ? Convert.ToString(rowData1["attachment"]) : rowData1.ContainsKey("fileAttachment") ? Convert.ToString(rowData1["fileAttachment"]) : null;
                if (fileId.IsNotNullAndNotEmpty())
                {
                    var _ocrMappingBusiness = _serviceProvider.GetService<IOCRMappingBusiness>();
                    var actualList = await _ocrMappingBusiness.GetList(x => x.TemplateId == model.TemplateId);
                    if (actualList.Count > 0)
                    {
                       rowData1= await _ocrMappingBusiness.GetExtractedData(fileId,actualList,rowData1);
                    }                    
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                Hangfire.BackgroundJob.Enqueue<HangfireScheduler>(x => x.UpdateOldIsLatestRevision(model.NoteNo, model.NoteId, model.TemplateId, _userContex.ToIdentityUser()));
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
                var tquery = @$"select tm.*,t.""Id"" as TemplateId from public.""TableMetadata"" as tm
                            join public.""Template"" as t on t.""TableMetadataId""=tm.""Id""
                            where t.""Id"" in ('{id}') limit 1";
                var tableMetaDatas = await _queryRepo.ExecuteQueryList<TableMetadataViewModel>(tquery, null);
              
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
                var table = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.TableMetadataId);
                if (table != null)
                {
                    var query = @$"delete from {table.Schema}.""{ table.Name}"" where ""Id""='{model.UdfNoteTableId}';
                    delete from public.""NtsNote"" where ""Id""='{model.NoteId}';";
                    await _queryRepo.ExecuteCommand(query, null);
                }
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
            var query = @$"select cm.*,tm.""Name"" as ""TableName"",tm.""Schema"" as ""TableSchemaName"",
            t.""Id"" as ""TemplateId"" from public.""Template"" t
            join public.""TemplateCategory"" tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
            join public.""TableMetadata"" tm on t.""TableMetadataId""=tm.""Id"" and tm.""IsDeleted""=false 
            join public.""ColumnMetadata"" cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsDeleted""=false  and cm.""IsUdfColumn""=true
            where t.""IsDeleted""=false  ";

            if (categoryId.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and tc.""Id""='{categoryId}'";
            }
            if (categoryCode.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and tc.""Code""='{categoryCode}'";
            }
            var columnList = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
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
            if (editUdfTable)
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
            var tquery = @$"select tm.*,t.""Id"" as TemplateId from public.""TableMetadata"" as tm
                            join public.""Template"" as t on t.""TableMetadataId""=tm.""Id""
                            where t.""Id"" in ('{id}') limit 1";
            var tableMetaDatas = await _queryRepo.ExecuteQueryList<TableMetadataViewModel>(tquery, null);
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
                var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                (
	                select 1
	                from information_schema.tables
	                where table_schema = '{ApplicationConstant.Database.Schema.Log}'
	                and table_name = '{tableMetaData.Name}Log'
                ) ", null);
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
            var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                (
	                select 1
	                from information_schema.tables
	                where table_schema = 'log'
	                and table_name = '{tableMetadata.Name}Log'
                ) ", null);
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
            var tquery = @$"select tm.*,t.""Id"" as TemplateId from public.""TableMetadata"" as tm
                            join public.""Template"" as t on t.""TableMetadataId""=tm.""Id""
                            where t.""Id"" in ('{tempid}') limit 1";
            var tableMetaDatas = await _queryRepo.ExecuteQueryList<TableMetadataViewModel>(tquery, null);
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
                            foreach (var col in tableColumns.Where(x => x.IsUdfColumn))
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
            var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                (
	                select 1
	                from information_schema.tables
	                where table_schema = '{ApplicationConstant.Database.Schema.Log}'
	                and table_name = '{tableMetaData.Name}Log'
                ) ", null);
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
            var query = @$"update public.""File"" set ""ReferenceTypeCode""='Note',""ReferenceTypeId""='{model.NoteId}'
            where ""Id"" in ('{attachments}')";
            await _queryRepo.ExecuteCommand(query, null);
        }

        private async Task<NoteTemplateViewModel> GetNoteTemplateById(NoteTemplateViewModel viewModel)
        {
            var data = new NoteTemplateViewModel();
            var query = @$"select ""TM"".* from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""TableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsNote"" as ""N"" on ""T"".""Id""=""N"".""TemplateId"" and ""N"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false   and ""N"".""Id""='{viewModel.NoteId}'";
            var tableMetadata = await _queryRepo.ExecuteQuerySingle<TableMetadataViewModel>(query, null);
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
            var query = @$"select ""TT"".*,
            ""T"".""Json"" as ""Json"",
            ""T"".""Id"" as ""TemplateId"",
            ""T"".""TableMetadataId"" as ""TableMetadataId"",
            ""T"".""Name"" as ""TemplateName"",
            ""T"".""DisplayName"" as ""TemplateDisplayName"",
            ""T"".""Code"" as ""TemplateCode"",
            ""TC"".""Id"" as ""TemplateCategoryId"",
            ""TC"".""Code"" as ""TemplateCategoryCode"",
            ""TC"".""Name"" as ""TemplateCategoryName"",
            ""TC"".""TemplateType"" as ""TemplateType"" ,
            ""TS"".""Id"" as ""NoteStatusId"" ,
            ""TS"".""Code"" as ""NoteStatusCode"" ,
            ""TS"".""Name"" as ""NoteStatusName"" ,
            ""TA"".""Id"" as ""NoteActionId"" ,
            ""TA"".""Code"" as ""NoteActionCode"" ,
            ""TA"".""Name"" as ""NoteActionName"" ,
            ""TP"".""Id"" as ""NotePriorityId"" ,
            ""TP"".""Code"" as ""NotePriorityCode"" ,
            ""TP"".""Name"" as ""NotePriorityName"" 
            from public.""Template"" as ""T""
            join public.""TemplateCategory"" as ""TC"" on ""T"".""TemplateCategoryId""=""TC"".""Id"" and ""TC"".""IsDeleted""=false 
            join public.""NoteTemplate"" as ""TT"" on ""TT"".""TemplateId""=""T"".""Id"" and ""TT"".""IsDeleted""=false 
            join public.""LOV"" as ""TS"" on ""TS"".""LOVType""='LOV_NOTE_STATUS' and ""TS"".""Code""='NOTE_STATUS_DRAFT'
            and ""TS"".""IsDeleted""=false 
            join public.""LOV"" as ""TA"" on ""TA"".""LOVType""='LOV_NOTE_ACTION' and ""TA"".""Code""='NOTE_ACTION_DRAFT' 
            and ""TA"".""IsDeleted""=false 
            join public.""LOV"" as ""TP"" on ""TP"".""LOVType""='NOTE_PRIORITY' and ""TP"".""IsDeleted""=false 
            and (""TP"".""Id""=""TT"".""PriorityId"" or ""TP"".""Code""='NOTE_PRIORITY_MEDIUM')
            where ""T"".""IsDeleted""=false and (""T"".""Id""='{vm.TemplateId}' or ""T"".""Code""='{vm.TemplateCode}')";
            var data = await _queryRepo.ExecuteQuerySingle<NoteTemplateViewModel>(query, null);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.TableMetadataId
                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
            }

            return data;
        }
        private async Task<string> GenerateNextNoteNo(NoteTemplateViewModel model)
        {
            if (model.NumberGenerationType != NumberGenerationTypeEnum.SystemGenerated)
            {
                return "";
            }
            string prefix = "N";
            var today = DateTime.Today;
            var query = $@"update public.""NtsNoteSequence"" SET ""NextId"" = ""NextId""+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""SequenceDate"" = '{today.ToDatabaseDateFormat()}'
            RETURNING ""NextId""-1";
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
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
            var query = $@"update public.""NtsNoteSequence"" SET ""NextId"" = ""NextId""+{count},
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""SequenceDate"" = '{today.ToDatabaseDateFormat()}'
            RETURNING ""NextId""-{count}";
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
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
                        || type == "datetime" || type == "day" || type == "time" || type == "currency"
                        || type == "signature" || type == "file" || type == "hidden" || type == "datagrid" || (type == "htmlelement" && key == "chartgrid"))
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
                            || type == "datetime" || type == "day" || type == "time" || type == "currency"
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
                    (t.""Schema""='{tableMetaData.Schema}' and t.""Name""='{tableMetaData.Name}' and (c.""IsUdfColumn""=true or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NoteTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsNote')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false";
            var pkColumns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
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
                            tableName = @$"{item.TableAliasName}_{item.ForeignKeyColumnName}";
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
            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
                where 
                (
                    (t.""Schema""='public' and t.""Name""='NoteTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsNote')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false";
            var pkColumns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
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
            var noteTemlateViewModel = await _queryRepo.ExecuteQuerySingle<NoteTemplateViewModel>
                (@$"select nt.* from public.""NoteTemplate"" nt 
                join public.""Template"" t on nt.""TemplateId""=t.""Id"" and t.""IsDeleted""=false 
                where nt.""IsDeleted""=false and t.""TableMetadataId""='{tableMetaData.Id}' ", null);
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
                tables.Add(@$" left join log.""NtsNoteLog"" as ""NtsNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""NtsNote"".""RecordId"" and ""{tableMetaData.Name}"".""LogVersionNo""=""NtsNote"".""LogVersionNo""  and ""NtsNote"".""IsDeleted""=false");
            }
            else
            {
                tables.Add(@$" join public.""NtsNote"" as ""NtsNote"" on ""{tableMetaData.Name}"".""NtsNoteId""=""NtsNote"".""Id"" and ""NtsNote"".""IsDeleted""=false");
            }

            tables.Add(@$"left join public.""Template"" as ""Template"" on ""NtsNote"".""TemplateId""=""Template"".""Id"" and ""Template"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""OwnerUser"" on ""NtsNote"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""RequestedByUser"" on ""NtsNote"".""RequestedByUserId""=""RequestedByUser"".""Id"" and ""RequestedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""CreatedByUser"" on ""NtsNote"".""CreatedBy""=""CreatedByUser"".""Id"" and ""CreatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""User"" as ""UpdatedByUser"" on ""NtsNote"".""LastUpdatedBy""=""UpdatedByUser"".""Id"" and ""UpdatedByUser"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""NoteTemplate"" as ""NoteTemplate"" on ""NoteTemplate"".""TemplateId""=""Template"".""Id"" and ""NoteTemplate"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""TemplateCategory"" as ""TemplateCategory"" on ""TemplateCategory"".""Id""=""Template"".""TemplateCategoryId"" and ""TemplateCategory"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NoteOwnerType"" on ""NtsNote"".""NoteOwnerTypeId""=""NoteOwnerType"".""Id"" and ""NoteOwnerType"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NotePriority"" on ""NtsNote"".""NotePriorityId""=""NotePriority"".""Id"" and ""NotePriority"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NoteStatus"" on ""NtsNote"".""NoteStatusId""=""NoteStatus"".""Id"" and ""NoteStatus"".""IsDeleted""=false ");
            tables.Add(@$"left join public.""LOV"" as ""NoteAction"" on ""NtsNote"".""NoteActionId""=""NoteAction"".""Id"" and ""NoteAction"".""IsDeleted""=false ");
            var selectQuery = @$"select {string.Join(",", columns)}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            if (where.IsNotNullAndNotEmpty())
            {
                selectQuery = $"{selectQuery} {where}";
            }
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
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""NoteStatusCode"",count(distinct t.""Id"") as ""NoteCount""      
            from public.""NtsNote"" as t
            #UDFJOIN#
            join public.""LOV"" as l on l.""Id""=t.""NoteStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""NoteStatusCode""
            ,count(distinct t.""Id"") as ""NoteCount""      
            from public.""NtsNote"" as t
            #UDFJOIN#
            join public.""NtsNoteShared"" as ts on t.""Id""=ts.""NtsNoteId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""NoteStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""NoteStatusCode""
            ,count(distinct t.""Id"") as ""NoteCount""      
            from public.""NtsNote"" as t
            #UDFJOIN#
            join public.""NtsNoteShared"" as ts on t.""Id""=ts.""NtsNoteId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""NoteStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedWithUserId"",l.""Code""";
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
        public async Task<DataTable> GetNoteIndexPageGridData(DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string noteStatusCode, string userId = null)
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

                        var selectQuery = await GetSelectQuery(tableMetadata);
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
                                var sharedwithquery = $@" Select ts.* from public.""NtsNoteShared"" as ts where ts.""SharedWithUserId""='{userId}' and ts.""IsDeleted""=false  ";
                                var sharedwithlist = await _queryRepo.ExecuteQueryList<NtsTaskSharedViewModel>(sharedwithquery, null);
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
                                var sharedbyquery = $@" Select ts.* from public.""NtsNoteShared"" as ts where ts.""SharedByUserId""='{userId}' and ts.""IsDeleted""=false  ";
                                var sharedbylist = await _queryRepo.ExecuteQueryList<NtsNoteSharedViewModel>(sharedbyquery, null);
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
            string query = @$"select n.""Id"" as Id,u.""Name"" as UserName,u.""Email"" as Email,u.""PhotoId"" as PhotoId
                              from public.""NtsNoteShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false 
                              
                              where n.""NtsNoteId""='{model.NoteId}' and n.""IsDeleted""=false 
                                union select n.""Id"" as Id,t.""Name"" as UserName,t.""Name"" as Email, t.""LogoId"" as PhotoId
                              from public.""NtsNoteShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsNoteId""='{model.NoteId}' AND n.""IsDeleted""= false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
            //new List<UserViewModel> { { new UserViewModel { UserName = "Shafi", Email = "shaficmd@gmail.com" } } ,
            //{ new UserViewModel { UserName = "Shafi2", Email = "shaficmd@gmail.com2" } } };
        }
        public async Task<List<NTSMessageViewModel>> GetNoteMessageList(string userId, string noteId)
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
            from public.""NtsNoteComment"" as ""C""  
            left join public.""User"" as ""CBU"" on ""C"".""CommentedByUserId""=""CBU"".""Id"" and ""CBU"".""IsDeleted""=false
            left join public.""NtsNoteCommentUser"" as ""NCU"" on ""C"".""Id""=""NCU"".""NtsNoteCommentId"" and ""NCU"".""IsDeleted""=false
            left join public.""User"" as ""CTU"" on ""NCU"".""CommentToUserId""=""CTU"".""Id"" and ""CTU"".""IsDeleted""=false 
            where ""C"".""NtsNoteId""='{noteId}' and ""C"".""IsDeleted""=false  ";

            var comments = await _queryRepo.ExecuteQueryList<NTSMessageViewModel>(query, null);

            return comments.OrderByDescending(x => x.SentDate).ToList();
        }

        public async Task GetNtsNoteIndexPageCount(NtsNoteIndexPageViewModel model)
        {
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""NoteStatusCode"",count(distinct t.""Id"") as ""NoteCount""      
            from public.""NtsNote"" as t
            join public.""LOV"" as l on l.""Id""=t.""NoteStatusId"" and l.""IsDeleted""=false 
             join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
             #WHERE#
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""NoteStatusCode""
            ,count(distinct t.""Id"") as ""NoteCount""      
            from public.""NtsNote"" as t
            join public.""NtsNoteShared"" as ts on t.""Id""=ts.""NtsNoteId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""NoteStatusId"" and l.""IsDeleted""=false 
             join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  ts.""SharedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""NoteStatusCode""
            ,count(distinct t.""Id"") as ""NoteCount""      
            from public.""NtsNote"" as t
            join public.""NtsNoteShared"" as ts on t.""Id""=ts.""NtsNoteId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""NoteStatusId"" and l.""IsDeleted""=false 
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

        public async Task<IList<NoteViewModel>> GetNtsNoteIndexPageGridData(DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string noteStatusCode, string categoryCode, string templateCode, string moduleCode, NtsViewTypeEnum? ntsViewType)
        {

            var query = @$" Select t.*,tmpc.""Code"" as ""TemplateCategoryCode"",ou.""Name"" as ""OwnerUserName"",ru.""Name"" as ""RequestedByUserName""
                        ,cu.""Name"" as ""CreatedByUserName"",lu.""Name"" as ""LastUpdatedByUserName""
                        , tlov.""Name"" as ""NoteStatusName"", tlov.""Code"" as ""NoteStatusCode"" ,m.""Name"" as Module
from public.""NtsNote"" as t
                        left join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
                          left join public.""Module"" as m ON m.""Id""=tmp.""ModuleId"" and m.""IsDeleted""=false 
left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
                        left join public.""LOV"" as tlov on t.""NoteStatusId""=tlov.""Id"" and tlov.""IsDeleted""=false 
                        left join public.""User"" as ou ON ou.""Id""=t.""OwnerUserId"" and ou.""IsDeleted""=false 
                        left join public.""User"" as ru ON ru.""Id""=t.""RequestedByUserId"" and ru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=t.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=t.""LastUpdatedBy"" and lu.""IsDeleted""=false 
                        #WHERE# 
                        ORDER BY t.""LastUpdatedDate"" DESC ";
            var where = @$" WHERE 1=1 and t.""PortalId""='{_userContex.PortalId}'";
            switch (ownerType)
            {

                case NtsActiveUserTypeEnum.Requester:
                    where += @$" and (t.""OwnerUserId""<>'{_repo.UserContext.UserId}' and t.""RequestedByUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.Owner:
                    where += @$" and (t.""OwnerUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.SharedWith:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false and ts.""SharedWithUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                case NtsActiveUserTypeEnum.SharedBy:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false  and ts.""SharedByUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                default:
                    break;
            }
            if (noteStatusCode.IsNotNullAndNotEmpty())
            {
                var taskstatus = $@" and tlov.""Code""='{noteStatusCode}' ";
                where = where + taskstatus;
            }
            if (ntsViewType.HasValue)
            {
                var ViewType = $@" and t.""ViewType""='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), ntsViewType.ToString()))}' ";
                where = where + ViewType;
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
        public async Task<DashboardNoteViewModel> NotesDashboardCount(string userId, string type = "ALL", string moduleName = null, string noteTemplateIds = null)
        {
            var cypher = $@"Select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
                            from public.""NtsNote"" as s                         
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
                            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}'  and u.""IsDeleted""=false 
                            join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
                           left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
                           where 1=1 and s.""PortalId""='{_userContex.PortalId}' and s.""IsDeleted""=false  #WHERE# #NOTETEMPLATESEARCH#";

            var search = "";
            string codes = null;

            if (moduleName.IsNotNullAndNotEmpty())
            {
                // search = $@" where m.""Code""='{moduleName}' ";
                var modules = moduleName.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and  m.""Code"" in (" + codes + ")";
                }
            }
            var noteTemplatesearch = "";
            if (noteTemplateIds.IsNotNullAndNotEmpty())
            {
                noteTemplateIds = noteTemplateIds.Replace(",", "','");
                noteTemplatesearch = $@" and tr.""Id"" in ('{noteTemplatesearch}')";
            }
            cypher = cypher.Replace("#WHERE#", search);
            cypher = cypher.Replace("#NOTETEMPLATESEARCH#", noteTemplatesearch);
            var note = await _queryRepo.ExecuteQueryList(cypher, null);
            //var _createdByMe = note.Where(x => (type != "ALL" ? (type == "OTHERS" ? (x.ReferenceType != NoteReferenceTypeEnum.Self) : x.ReferenceType == type.ToEnum<NoteReferenceTypeEnum>()) : true)).Count();
            var _createdByMeExpired = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE").Count();
            var _createdByMeActive = note.Where(x => /*x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" ||*/ x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Count();
            var _createdByMeDraft = note.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Count();

            var cypher1 = $@"Select s.""Id"" as Id,ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
from public.""NtsNote"" as s
join public.""NtsNoteShared"" as ss on ss.""NtsNoteId""=s.""Id""  and ss.""IsDeleted""=false 
--join public.""LOV"" as sst on sst.""Id""=ss.""NoteharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_USER' and sst.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=ss.""SharedWithUserId""  and u.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false 
where u.""Id""='{userId}' and s.""PortalId""='{_userContex.PortalId}' and s.""IsDeleted""=false  #WHERE# #NOTETEMPLATESEARCH#

union
Select distinct s.""Id"" as Id,ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
from public.""User"" as u
join public.""TeamUser"" as tu on tu.""UserId""=u.""Id""  and tu.""IsDeleted""=false  
join public.""NtsNoteShared"" as ss on ss.""SharedWithTeamId""=tu.""TeamId""  and ss.""IsDeleted""=false  
--join public.""LOV"" as sst on sst.""Id""=ss.""NoteSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_TEAM' and sst.""IsDeleted""=false 
join public.""NtsNote"" as s on s.""Id""=ss.""NtsNoteId""  and s.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false 
where u.""Id""='{userId}' and s.""PortalId""='{_userContex.PortalId}' and u.""IsDeleted""=false  #WHERE# #NOTETEMPLATESEARCH# ";

            //var search1 = "";

            //if (moduleName.IsNotNullAndNotEmpty())
            //{
            //    //search1 = $@" and m.""Code""='{moduleName}' ";
            //    if (codes.IsNotNullAndNotEmpty())
            //    {
            //        search1 = $@" and m.""Code"" in (" + codes + ")";
            //    }
            //}

            cypher1 = cypher1.Replace("#WHERE#", search);
            cypher1 = cypher1.Replace("#NOTETEMPLATESEARCH#", noteTemplatesearch);
            var noteshared = await _queryRepo.ExecuteQueryList(cypher1, null)/*.DistinctBy(x => x.Id).ToList()*/;
            // var notesharebyme = ExecuteCypherList<NoteViewModel>(cypher1, prms1).DistinctBy(x => x.Id).ToList();

            var _shareWithMe = noteshared.Count();
            var _sharedWithMeExpired = noteshared.Where(x => x.NoteStatusCode == "NOTE_STATUS_EXPIRE").Count();
            var _sharedWithMeActive = noteshared.Where(x => /*x.NoteStatusCode == "NOTE_STATUS_INPROGRESS" ||*/ x.NoteStatusCode == "NOTE_STATUS_COMPLETE").Count();
            var _sharedWithMeDraft = noteshared.Where(x => x.NoteStatusCode == "NOTE_STATUS_DRAFT").Count();

            var cypher2 = $@"Select distinct ns.""Code"" as NoteStatusCode,s.""Id"" as Id
from public.""User"" as u
join public.""NtsNote"" as s on s.""OwnerUserId""=u.""Id"" and s.""IsDeleted""=false  
join public.""NtsNoteShared"" as ss on ss.""CreatedBy""=u.""Id"" and s.""Id""=ss.""NtsNoteId"" and ss.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
where u.""Id""='{userId}' and u.""IsDeleted""=false  and s.""PortalId""='{_userContex.PortalId}' #WHERE#";


            cypher2 = cypher2.Replace("#WHERE#", search);
            cypher2 = cypher2.Replace("#NOTETEMPLATESEARCH#", noteTemplatesearch);
            var notesharebyme = await _queryRepo.ExecuteQueryList(cypher2, null);
            //  var notesharebyme = ExecuteCypherList<NoteViewModel>(cypher2, prms2).Where(x => x.ShareTo != null).ToList();
            //var _sharedByMe = notesharebyme.Where(x => (type != "ALL" ? (type == "OTHERS" ? (x.ReferenceType != NoteReferenceTypeEnum.Self) : x.ReferenceType == type.ToEnum<NoteReferenceTypeEnum>()) : true)).Count();
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
            var query = @$" Select t.*,tmpc.""Code"" as ""TemplateCategoryCode"",ou.""Name"" as ""OwnerUserName"",ru.""Name"" as ""RequestedByUserName""
                        ,cu.""Name"" as ""CreatedByUserName"",lu.""Name"" as ""LastUpdatedByUserName""
                        , tlov.""Name"" as ""NoteStatusName"", tlov.""Code"" as ""NoteStatusCode"" ,m.""Name"" as Module,tmp.""Code"" as TemplateMasterCode,m.""Code"" as ModuleCode
from public.""NtsNote"" as t
                         join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false and t.""IsDeleted""=false 
                          left join public.""Module"" as m ON m.""Id""=tmp.""ModuleId"" and m.""IsDeleted""=false 
left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
 left join public.""Module"" as tcm ON tcm.""Id""=tmpc.""ModuleId"" and tcm.""IsDeleted""=false 
                        left join public.""LOV"" as tlov on t.""NoteStatusId""=tlov.""Id"" and tlov.""IsDeleted""=false 
                        left join public.""User"" as ou ON ou.""Id""=t.""OwnerUserId"" and ou.""IsDeleted""=false 
                        left join public.""User"" as ru ON ru.""Id""=t.""RequestedByUserId"" and ru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=t.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=t.""LastUpdatedBy"" and lu.""IsDeleted""=false 
                        #WHERE# 
                        ORDER BY t.""LastUpdatedDate"" DESC ";
            var where = @$" WHERE 1=1  ";
            //var ownerType = NtsActiveUserTypeEnum.Owner;
            //if (searchModel.Mode == "REQ_BY")
            //{
            //    ownerType = NtsActiveUserTypeEnum.Owner;
            //}
            //else if (searchModel.Mode == "ASSIGN_BY")
            //{
            //    ownerType = NtsActiveUserTypeEnum.SharedBy;
            //}
            //else if (searchModel.Mode == "SHARE_TO")
            //{
            //    ownerType = NtsActiveUserTypeEnum.SharedWith;
            //}
            //switch (ownerType)
            //{

            //    case NtsActiveUserTypeEnum.Requester:
            //        where += @$" and (t.""OwnerUserId""<>'{searchModel.UserId}' and t.""RequestedByUserId""='{searchModel.UserId}')";
            //        break;
            //    case NtsActiveUserTypeEnum.Owner:
            //        where += @$" and (t.""OwnerUserId""='{searchModel.UserId}')";
            //        break;
            //    case NtsActiveUserTypeEnum.SharedWith:
            //        where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false and ts.""SharedWithUserId""='{searchModel.UserId}') ) ";
            //        break;
            //    case NtsActiveUserTypeEnum.SharedBy:
            //        where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false and ts.""SharedByUserId""='{searchModel.UserId}') ) ";
            //        break;
            //    default:
            //        break;
            //}
            if (searchModel.Mode.IsNotNullAndNotEmpty())
            {
                var modes = searchModel.Mode.Split(',');
                if (modes.Any(y => y == "REQ_BY"))
                    where += @$" and (t.""OwnerUserId""='{searchModel.UserId}')";
                if (modes.Any(y => y == "SHARE_TO"))
                    where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false and ts.""SharedWithUserId""='{searchModel.UserId}' ) ) ";
                if (modes.Any(y => y == "ASSIGN_BY"))
                    where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false and ts.""SharedByUserId""='{searchModel.UserId}' ) ) ";
                //if ("REQ_BY".Equals(searchModel.Mode))
                //    list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Owner).ToList();
                //if ("SHARE_TO".Equals(searchModel.Mode))
                //    list = list.Where(x => x.TemplateUserType == NtsUserTypeEnum.Shared).ToList();
            }
            if (searchModel.NoteStatusIds.IsNotNull())
            {
                var statusids = string.Join("','", searchModel.NoteStatusIds);
                var notestatus = $@" and t.""NoteStatusId"" in ('{statusids}') ";
                where = where + notestatus;
            }
            if (searchModel.NoteStatus.IsNotNull())
            {
                searchModel.NoteStatus = searchModel.NoteStatus.Replace(",", "','");
                var notestatus = $@" and tlov.""Code"" in ('{searchModel.NoteStatus}') ";
                where = where + notestatus;
            }
            if (searchModel.ModuleId.IsNotNullAndNotEmpty())
            {
                searchModel.ModuleId = searchModel.ModuleId.Replace(",", "','");
                var module = $@" and m.""Id"" in ('{searchModel.ModuleId}') ";
                where = where + module;
            }
            if (searchModel.PortalNames.IsNotNullAndNotEmpty())
            {
                var portal = $@" and t.""PortalId"" in ('{searchModel.PortalNames}') ";
                where = where + portal;
            }
            else
            {
                var portal = $@" and t.""PortalId"" in ('{_repo.UserContext.PortalId}') ";
                where = where + portal;
            }

            //if (templateCode.IsNotNullAndNotEmpty())
            //{
            //    var tempItems = templateCode.Split(',');
            //    var tempText = "";
            //    foreach (var item in tempItems)
            //    {
            //        tempText = $"{tempText},'{item}'";
            //    }
            //    //var tempcode = $@" and tmp.""Code""='{templateCode}' ";
            //    var tempcode = $@" and tmp.""Code"" IN ({tempText.Trim(',')}) ";
            //    where = where + tempcode;
            //}
            //if (categoryCode.IsNotNullAndNotEmpty())
            //{
            //    var categoryItems = categoryCode.Split(',');
            //    var categoryText = "";
            //    foreach (var item in categoryItems)
            //    {
            //        categoryText = $"{categoryText},'{item}'";
            //    }
            //    //var categorycode = $@" and tmpc.""Code""='{categoryCode}' ";
            //    var categorycode = $@" and tmpc.""Code"" IN ({categoryText.Trim(',')}) ";
            //    where = where + categorycode;
            //}
            query = query.Replace("#WHERE#", where);
            var querydata = await _queryRepo.ExecuteQueryList(query, null);
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
            var query = $@"select S.""SchemaName"",S.""Query"",S.""Metadata"",case when (S.""ElsasticDB"" is null or S.""ElsasticDB""='False') then false else true end as ElsasticDB,N.""Id"" as Id, N.""Id"" as NoteId,u.""Name"" as CreatedBy from  ""NtsNote""  N 
                    inner join cms.""N_BusinessAnalytics_SynergySchema"" S on S.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false 
                    left join public.""User"" as u on u.""Id""=N.""CreatedBy"" and u.""IsDeleted""=false 
                    where S.""IsDeleted""= false";

            var result = await _queryRepo.ExecuteQueryList<SynergySchemaViewModel>(query, null);
            return result;

        }
        public async Task<DateTime> GetLastUpdatedSynerySchema()
        {
            var query = $@"select cn.""LastUpdatedDate"" as LastUpdatedDate 
                    from  ""NtsNote""  N 
                    inner join cms.""N_BusinessAnalytics_SynergySchema"" S on S.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false
                    inner join ""NtsNote""  cn on cn.""ParentNoteId""=N.""Id""
                    where S.""IsDeleted""= false order by cn.""LastUpdatedDate"" desc limit 1";

            var result = await _queryRepo.ExecuteScalar<DateTime>(query, null);
            return result;

        }

        public async Task<SynergySchemaViewModel> GetSynerySchemaById(string Id)
        {
            var query = $@"select S.""SchemaName"",S.""Query"",S.""Metadata"",case when (S.""ElsasticDB"" is null or S.""ElsasticDB""='False') then false else true end as ElsasticDB ,N.""Id"" as Id, N.""Id"" as NoteId from  ""NtsNote""  N inner join cms.""N_BusinessAnalytics_SynergySchema"" S on S.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false where S.""IsDeleted""= false and N.""Id""='{Id}'";


            var result = await _queryRepo.ExecuteQuerySingle<SynergySchemaViewModel>(query, null);
            return result;

        }


        public async Task<DataTable> GetQueryResult(string Query)
        {
            var result = await _queryRepo.ExecuteQueryDataTable(Query, null);
            return result;
        }

        public async Task<bool> DeleteSchema(string NoteId)
        {

            var query = $@"update cms.""N_BusinessAnalytics_SynergySchema"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;

        }
        public async Task<IList<NoteViewModel>> GetAllDashboardMaster()
        {
            var query = @$" select n.* 
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_DashboardMaster"" as dm on dm.""NtsNoteId""=n.""Id"" and (dm.""gridStack""='False' or dm.""gridStack"" is null) and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_DashboardMaster' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList(query, null);
            return querydata;
        }
        public async Task<IList<DashboardMasterViewModel>> GetAllGridStackDashboard()
        {
            var query = @$" select n.*,dm.""layoutMetadata"" as ""layoutMetadata"" 
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false  and n.""ParentNoteId"" is null
                        join cms.""N_CoreHR_DashboardMaster"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""gridStack""='True' and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_DashboardMaster' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<DashboardMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<KanbanBoardViewModel>> GetKanbanBoard()
        {
            var query = @$" select n.*,dm.""boards"" as ""boards"" ,dm.""kanbanTemplate"" as kanbanTemplate
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_BusinessAnalytics_KanbanBoard"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Name""='N_BusinessAnalytics_KanbanBoard' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<KanbanBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<KanbanBoardViewModel> GetKanbanBoardDetails(string noteId)
        {
            var query = @$" select dm.""boards"" as ""boards"",dm.""kanbanTemplate"" as kanbanTemplate
                        from cms.""N_BusinessAnalytics_KanbanBoard"" as dm                         
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<KanbanBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<DashboardMasterViewModel> GetDashboardMasterDetails(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""layoutMetadata"" as layoutMetadata                         
                        from cms.""N_CoreHR_DashboardMaster"" as dm 
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardMasterViewModel>(query, null);
            return querydata;
        }


        public async Task UpdateModel(string data, string id)
        {
            var updateQuery = $@"Update cms.""N_CoreHR_DashboardMaster"" set ""layoutMetadata"" = '{data}' where ""NtsNoteId"" = '{id}' and ""IsDeleted"" = false ";
            await _queryRepo.ExecuteCommand(updateQuery, null);
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemMasterDetails(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId, dm.""chartMetadata"" as chartMetadata,
                        dm.""measuresField"" as measuresField,dm.""dimensionsField"" as dimensionsField,dm.""segmentsField"" as segmentsField,
                        dm.""filterField"" as filterField,dm.""mapUrl"" as mapUrl,dm.""mapLayer"" as mapLayer,ct.""Help"" as Help,
                        dm.""ThemeMode"" as ThemeMode,dm.""Palette"" as Palette,dm.""MonocromeColor"" as MonocromeColor,
                        dm.""onChartClickFunction"" as onChartClickFunction,dm.""DynamicMetadata"" as DynamicMetadata,dm.""isLibrary"" as isLibrary,
                        dm.""Xaxis"" as Xaxis,dm.""Yaxis"" as Yaxis,dm.""Count"" as Count,dm.""timeDimensionsField"" as timeDimensionsField
                        from cms.""N_CoreHR_DashboardItem"" as dm 
                        left join cms.""N_CoreHR_ChartTemplate"" as ct on ct.""NtsNoteId""=dm.""chartTypeId"" and ct.""IsDeleted""=false
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardItemMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<MapLayerItemViewModel> GetMapLayerItemDetails(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""MapUrl"" as ""MapUrl"", dm.""MapLayer"" as ""MapLayer"",
                        dm.""MapTransparency"" as ""MapTransparency"", dm.""MapFormat"" as ""MapFormat"",
                        dm.""MapOpacity"" as ""MapOpacity"", dm.""IsBaseMap"" as ""IsBaseMap""
                        from cms.""N_BusinessAnalytics_MapLayerItem"" as dm                        
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<MapLayerItemViewModel>(query, null);
            return querydata;
        }
        public async Task<List<MapLayerItemViewModel>> GetMapLayerItemList(string parentNoteId)
        {
            var query = @$" select n.""NoteSubject"" as ""NoteSubject"",dm.""NtsNoteId"" as NoteId, dm.""MapUrl"" as ""MapUrl"", dm.""MapLayer"" as ""MapLayer"",
                        dm.""MapTransparency"" as ""MapTransparency"", dm.""MapFormat"" as ""MapFormat"",
                        dm.""MapOpacity"" as ""MapOpacity"", dm.""IsBaseMap"" as ""IsBaseMap""
                        from cms.""N_BusinessAnalytics_MapLayerItem"" as dm   
                        join public.""NtsNote"" as n on n.""Id""=dm.""NtsNoteId"" and n.""IsDeleted""=false and dm.""IsDeleted""=false
                        join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""IsDeleted""=false
                        where p.""Id""='{parentNoteId}'";
            var querydata = await _queryRepo.ExecuteQueryList<MapLayerItemViewModel>(query, null);
            return querydata;
        }
        public async Task<WidgetItemViewModel> GetWidgetItemDetails(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""keyword"" as keyword, dm.""socialMediaType"" as socialMediaType,
                        dm.""from"" as from,dm.""to"" as to,dm.""height"" as height,dm.""width"" as width,dm.""chartMetadata"" as chartMetadata,dm.""chartTypeId"" as chartTypeId
                        from cms.""N_SWS_WidgetItem"" as dm 
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WidgetItemViewModel>(query, null);
            return querydata;
        }
        public async Task<List<WidgetItemViewModel>> GetWidgetItemList(string parentId)
        {
            var query = @$" select m.*,dm.""NtsNoteId"" as NoteId, dm.""keyword"" as keyword, dm.""socialMediaType"" as socialMediaType,
                        dm.""from"" as from,dm.""to"" as to,dm.""height"" as height,dm.""width"" as width,dm.""chartMetadata"" as chartMetadata,dm.""chartTypeId"" as chartTypeId,n.""NoteSubject"" as chartTypeName
                        from cms.""N_SWS_WidgetItem"" as dm 
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as n on n.""Id""=dm.""chartTypeId"" and n.""IsDeleted""=false 
                        where m.""ParentNoteId""='{parentId}' and dm.""IsDeleted""=false
                        order by m.""CreatedDate"" ";
            var querydata = await _queryRepo.ExecuteQueryList<WidgetItemViewModel>(query, null);
            return querydata;
        }
        public async Task<WidgetItemViewModel> GetWidgetItemDetailsByName(string name)
        {
            var query = @$" select  m.""NoteSubject"" as NoteSubject,dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId, dm.""chartMetadata"" as chartMetadata,ct.""boilerplateCode""  as boilerplateCode                       
                        from cms.""N_SWS_WidgetItem"" as dm
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as n on n.""Id""=dm.""chartTypeId"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as ct on n.""Id""=ct.""NtsNoteId"" and ct.""IsDeleted""=false 
                        where m.""NoteSubject""='{name}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WidgetItemViewModel>(query, null);
            return querydata;
        }
        public async Task<List<DashboardItemMasterViewModel>> GetDashboardItemMasterList(string parentId)
        {
            var query = @$" select m.*,dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId, dm.""chartMetadata"" as chartMetadata,
                        dm.""measuresField"" as measuresField,dm.""dimensionsField"" as dimensionsField,dm.""segmentsField"" as segmentsField,
                        dm.""filterField"" as filterField,dm.""mapUrl"" as mapUrl,dm.""mapLayer"" as mapLayer,dm.""Help"" as Help,
                        dm.""ThemeMode"" as ThemeMode,dm.""Palette"" as Palette,dm.""MonocromeColor"" as MonocromeColor,
                        dm.""Xaxis"" as Xaxis,dm.""Yaxis"" as Yaxis,dm.""Count"" as Count
                        from cms.""N_CoreHR_DashboardItem"" as dm 
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        where m.""ParentNoteId""='{parentId}' and dm.""IsDeleted""=false 
                        order by m.""CreatedDate"" ";
            var querydata = await _queryRepo.ExecuteQueryList<DashboardItemMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsWithChartTemplate(string noteId)
        {
            var query = @$" select  m.""NoteSubject"" as NoteSubject,dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId,
                        dm.""chartMetadata"" as chartMetadata,ct.""boilerplateCode""  as boilerplateCode ,dm.""mapUrl"" as mapUrl,
                        dm.""mapLayer"" as mapLayer,dm.""Help"" as Help,dm.""ThemeMode"" as ThemeMode,dm.""Palette"" as Palette,dm.""MonocromeColor"" as MonocromeColor,
                        dm.""Xaxis"" as Xaxis,dm.""Yaxis"" as Yaxis,dm.""Count"" as Count
                        from cms.""N_CoreHR_DashboardItem"" as dm
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as n on n.""Id""=dm.""chartTypeId"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as ct on n.""Id""=ct.""NtsNoteId"" and ct.""IsDeleted""=false 
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardItemMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsByName(string name)
        {
            var query = @$" select  m.""NoteSubject"" as NoteSubject,m.""NoteDescription"" as NoteDescription,dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId,
                        dm.""chartMetadata"" as chartMetadata, dm.""filterField"" as filterField,ct.""boilerplateCode""  as boilerplateCode,
                        dm.""onChartClickFunction"" as onChartClickFunction,dm.""mapUrl"" as mapUrl,dm.""mapLayer"" as mapLayer,dm.""Help"" as Help,
                        dm.""ThemeMode"" as ThemeMode,dm.""Palette"" as Palette,dm.""MonocromeColor"" as MonocromeColor,dm.""DynamicMetadata"" as DynamicMetadata,
                        dm.""Xaxis"" as Xaxis,dm.""Yaxis"" as Yaxis,dm.""Count"" as Count,dm.""timeDimensionsField"" as timeDimensionsField
                        from cms.""N_CoreHR_DashboardItem"" as dm
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as n on n.""Id""=dm.""chartTypeId"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as ct on n.""Id""=ct.""NtsNoteId"" and ct.""IsDeleted""=false 
                        where m.""NoteSubject""='{name}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardItemMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsById(string id)
        {
            var query = @$" select  m.""NoteSubject"" as NoteSubject,m.""NoteDescription"" as NoteDescription,dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId,
                        dm.""chartMetadata"" as chartMetadata, dm.""filterField"" as filterField,ct.""boilerplateCode""  as boilerplateCode,
                        dm.""onChartClickFunction"" as onChartClickFunction,dm.""mapUrl"" as mapUrl,dm.""mapLayer"" as mapLayer,dm.""Help"" as Help,
                        dm.""ThemeMode"" as ThemeMode,dm.""Palette"" as Palette,dm.""MonocromeColor"" as MonocromeColor,dm.""DynamicMetadata"" as DynamicMetadata,
                        dm.""Xaxis"" as Xaxis,dm.""Yaxis"" as Yaxis,dm.""Count"" as Count,dm.""timeDimensionsField"" as timeDimensionsField
                        from cms.""N_CoreHR_DashboardItem"" as dm
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as n on n.""Id""=dm.""chartTypeId"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as ct on n.""Id""=ct.""NtsNoteId"" and ct.""IsDeleted""=false 
                        where m.""Id""='{id}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardItemMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllChartTemplate()
        {
            var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as Name,dm.""Help"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_ChartTemplate' and t.""IsDeleted""=false   order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetAllDashboardItemDetailsWithDashboard()
        {
            var query = @$" select Concat(n.""NoteSubject"" ,' - ',m.""NoteSubject"") as Name,Concat(m.""NoteSubject"" ,'-',c.""NoteSubject"") as Id                  
                        from cms.""N_CoreHR_DashboardItem"" as dm
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as n on n.""Id""=m.""ParentNoteId""  and n.""IsDeleted""=false                      
                        join public.""NtsNote"" as c on c.""Id""=dm.""chartTypeId""  and c.""IsDeleted""=false                    
                        where dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetLibraryDashboardItemDetailsWithDashboard()
        {
            var query = @$" select Concat(n.""NoteSubject"" ,' - ',m.""NoteSubject"") as Name,m.""Id"" as Id                  
                        from cms.""N_CoreHR_DashboardItem"" as dm
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as n on n.""Id""=m.""ParentNoteId""  and n.""IsDeleted""=false                      
                        join public.""NtsNote"" as c on c.""Id""=dm.""chartTypeId""  and c.""IsDeleted""=false                    
                        where dm.""IsDeleted""=false and dm.""isLibrary""='True' ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetTagIdNameList(string portalId)
        {
            var query = @$" select tg.""TagCategoryName""  as Name,n.""Id"" as Id                  
                        from public.""NtsNote"" as n
                        join cms.""N_General_TagCategory"" as tg on tg.""NtsNoteId""=n.""Id""    and tg.""IsDeleted""=false    
                        join public.""Template"" as m on n.""TemplateId""=m.""Id""  and m.""IsDeleted""=false                                         
                        where   n.""IsDeleted""=false  and m.""Code""='TAG_CATEGORY' #WHERE# ";
            var search = "";
            if (portalId.IsNotNullAndNotEmpty())
            {
                search = $@"and n.""PortalId""='{portalId}'";
            }
            query = query.Replace("#WHERE#", search);
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TagCategoryViewModel>> GetTagCategoryList(string TemplateId)
        {
            var query = @$"  select tg.""TagCategoryName"" as Name,n.""Id"" as Id,temp.""Id"" as TemplateId
from public.""Template"" as temp
join public.""NtsNote"" as n on n.""Id"" = Any(temp.""AllowedTagCategories"")  and n.""IsDeleted""=false 
join cms.""N_General_TagCategory"" as tg on tg.""NtsNoteId""=n.""Id""  and tg.""IsDeleted""=false 
where temp.""Id""='{TemplateId}' and temp.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TagCategoryViewModel>> GetTagListByCategoryId(string CategoryId)
        {
            var query = @$" select n.""NoteSubject""  as Name,n.""Id"" as Id               
                        from public.""NtsNote"" as n
                        join cms.""N_General_Tag"" as tg on tg.""NtsNoteId""=n.""Id""  and tg.""IsDeleted""=false  
                        where  n.""IsDeleted""=false  and n.""ParentNoteId""='{CategoryId}'";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<TagCategoryViewModel> GetCategoryByTagId(string tagId)
        {
            var query = @$" select n.""NoteSubject""  as Name,n.""Id"" as Id               
                        from public.""NtsNote"" as n
                       join public.""Template"" as m on n.""TemplateId""=m.""Id""   and m.""IsDeleted""=false      
                        join public.""NtsNote"" as tg on tg.""ParentNoteId""=n.""Id""  and tg.""IsDeleted""=false      
                        where  n.""IsDeleted""=false  and m.""Code""='TAG_CATEGORY' and tg.""Id""='{tagId}'";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<NoteViewModel>> GetNoteDataListByTemplateCode(string templateCode)
        {

            var query = @$" Select t.*,tmpc.""Code"" as ""TemplateCategoryCode"",ou.""Name"" as ""OwnerUserName"",ru.""Name"" as ""RequestedByUserName""
                        ,cu.""Name"" as ""CreatedByUserName"",lu.""Name"" as ""LastUpdatedByUserName""
                        , tlov.""Name"" as ""NoteStatusName"", tlov.""Code"" as ""NoteStatusCode"" ,m.""Name"" as Module
                        from public.""NtsNote"" as t
                        join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false  and tmp.""Code""='{templateCode}'
                        left join public.""Module"" as m ON m.""Id""=tmp.""ModuleId"" and m.""IsDeleted""=false 
                        left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
                        left join public.""LOV"" as tlov on t.""NoteStatusId""=tlov.""Id"" and tlov.""IsDeleted""=false 
                        left join public.""User"" as ou ON ou.""Id""=t.""OwnerUserId"" and ou.""IsDeleted""=false 
                        left join public.""User"" as ru ON ru.""Id""=t.""RequestedByUserId"" and ru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=t.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=t.""LastUpdatedBy"" and lu.""IsDeleted""=false 
                        
                        ORDER BY t.""LastUpdatedDate"" DESC ";
            var querydata = await _queryRepo.ExecuteQueryList(query, null);
            return querydata;
        }
        public async Task<IList<DirectoryContent>> GetAllDocuments(string parentId)
        {
            string query = $@"  
                             WITH RECURSIVE Department AS(
                                 select d.*
                                from public.""NtsNote"" as d
                                where d.""TemplateCode"" = 'WORKSPACE_GENERAL' and d.""IsDeleted""=false 
                              union all

                                 select h.*
                                from public.""NtsNote"" as h                                
                                join Department ns on h.""ParentNoteId"" = ns.""Id""
                                where h.""IsDeleted""=false 
                        
                             )
                            select ""Id"" as id,""NoteSubject"" as name,""ParentNoteId"" as parentId,""CreatedDate"" as dateCreated,true as hasChild from Department	
                            ";



            var queryData = await _queryRepo.ExecuteQueryList<DirectoryContent>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<DirectoryContent>> GetAllChildDocuments(string parentId)
        {
            string query = $@"  select ""Id"" as id,""NoteSubject"" as name,""ParentNoteId"" as parentId,""CreatedDate"" as dateCreated,true as hasChild,""TemplateCode"" as TemplateCode
                            
                                from public.""NtsNote""                               
                                where ""ParentNoteId"" = '{parentId}' and ""IsDeleted""=false  and ""IsArchived""=false
                            ";



            var queryData = await _queryRepo.ExecuteQueryList<DirectoryContent>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<DirectoryContent>> GetAllFolderDocuments(string parentId)
        {
            string query = $@"  select n.""Id"" as id,n.""NoteSubject"" as name,n.""ParentNoteId"" as parentId,n.""CreatedDate"" as dateCreated,true as hasChild,n.""TemplateCode"" as TemplateCode,'Document' as FolderType
                            
                                from public.""NtsNote""   as n
join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false  and tc.""Code""='GENERAL_DOCUMENT'
                                where ""ParentNoteId"" = '{parentId}'  and n.""IsArchived""=false and n.""IsDeleted""=false 
                            ";



            var queryData = await _queryRepo.ExecuteQueryList<DirectoryContent>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<DirectoryContent>> GetAllDocumentFiles(string documentId)
        {
            var list = new List<DirectoryContent>();
            string query = $@"select t.* from public.""NtsNote"" as n
                            join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false 
                            where n.""Id""= '{documentId}' and n.""IsDeleted""=false  and n.""IsArchived""=false
                            ";
            var tableName = await _queryRepo.ExecuteQuerySingle<TemplateViewModel>(query, null);
            var attchments = await BuildChart(tableName.Json);
            attchments = attchments.TrimEnd(',');
            var fields = attchments.Split(',');
            foreach (var field in fields)
            {
                string query1 = $@"select d.""{field}"" as id,f.""FileName"" as name,n.""Id"" as parentId,'File' as FolderType,'0' as Count,
                            d.""CreatedDate"" as dateCreated,d.""LastUpdatedDate"" as dateModified,true as hasChild,'FILE' as TemplateCode
                            ,n.""NoteNo"" as NoteNo,u.""Name"" as CreatedBy
                            from cms.""{tableName.Name}""  as d
                            join public.""NtsNote"" as n on n.""Id""=d.""NtsNoteId"" and n.""IsDeleted""=false 
                            left join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false
                            left join public.""File"" as f on f.""Id""=d.""{field}"" and f.""IsDeleted""=false 
                            where n.""Id""= '{documentId}' and d.""IsDeleted""=false 
                            ";
                var queryData = await _queryRepo.ExecuteQuerySingle<DirectoryContent>(query1, null);
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
            var query = $@"select * from public.""NtsNote"" 
where ""TemplateId""='{templateId}' and  LOWER(""NoteSubject"")=LOWER('{subject}') and ""Id""<> '{noteId}' and ""IsDeleted""=false  limit 1";
            var data = await _queryRepo.ExecuteQuerySingle(query, null);
            if (data != null)
            {
                return true;
            }
            return false;
        }
        public async Task<IList<SocialWebsiteViewModel>> GetAllSocialWebsite()
        {
            var query = @$" select n.*,dm.""socialMediaType""  as socialMediaType,dm.""postType"" as postType
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_SWS_SocialMediaMaster"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Name""='N_SWS_SocialMediaMaster' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<SocialWebsiteViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<FacebookViewModel>> GetFacebookData(string searchStr)
        {
            var query = @$" select * from public.""three_facebook_keyword_post""
                        #WHERE#
                        -- order by ""fbid"" desc limit 10 
                        ";
            var where = "";
            if (searchStr.IsNotNullAndNotEmpty())
            {
                where = @$" WHERE ""pagename"" like '%{searchStr}%' COLLATE ""tr-TR-x-icu"" ";
            }
            query = query.Replace("#WHERE#", where);
            var querydata = await _queryRepo.ExecuteQueryList<FacebookViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<TwitterViewModel>> GetTwitterData()
        {
            var query = @$" select * from public.""three_new_tweet_table""
                        -- order by ""created_at"" desc limit 10 
";
            var querydata = await _queryRepo.ExecuteQueryList<TwitterViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<YoutubeViewModel>> GetYoutubeData()
        {
            var query = @$" select * from public.""three_youtube""
                        -- order by ""ytid"" desc limit 10 
";
            var querydata = await _queryRepo.ExecuteQueryList<YoutubeViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<WhatsAppViewModel>> GetWhatsAppData()
        {
            var query = @$" select * from public.""sm_whatsapp""
                         -- limit 10 
                            ";
            var querydata = await _queryRepo.ExecuteQueryList<WhatsAppViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<InstagramViewModel>> GetInstagramData()
        {
            var query = @$" select * from public.""three_instagram_data""
                         --order by ""id"" desc limit 10 
                         ";
            var querydata = await _queryRepo.ExecuteQueryList<InstagramViewModel>(query, null);
            return querydata;
        }
        public async Task<List<DbConnectionViewModel>> GetDbConnectionData()
        {
            var query = @$" select n.*,db.""hostName"" as hostName,db.""port"" as port,db.""maintenanceDatabase"" as maintenanceDatabase,
                        db.""username"" as username,db.""role"" as role,db.""service"" as service,db.""password"" as password,db.""databaseType"" as databaseType
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_SWS_DbConnection"" as db on db.""NtsNoteId""=n.""Id"" and db.""IsDeleted""=false 
                        where t.""Name""='N_SWS_DbConnection' and t.""IsDeleted""=false  ";
            var querydata = await _queryRepo.ExecuteQueryList<DbConnectionViewModel>(query, null);
            return querydata;
        }
        public async Task<List<ApiConnectionViewModel>> GetApiConnectionData()
        {
            var query = @$" select n.*,db.""restApiUrl"" as restApiUrl,db.""parameters"" as parameters,db.""userName"" as userName,
                        db.""password"" as password,db.""apiKey"" as apiKey,db.""pollingTime"" as pollingTime
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_SWS_ApiConnection"" as db on db.""NtsNoteId""=n.""Id"" and db.""IsDeleted""=false 
                        where t.""Name""='N_SWS_ApiConnection' and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<ApiConnectionViewModel>(query, null);
            return querydata;
        }
        public async Task<DbConnectionViewModel> GetDbConnectionDetails(string noteId)
        {
            var query = @$" select db.""NtsNoteId"" as NoteId,db.""hostName"" as hostName,db.""port"" as port,db.""maintenanceDatabase"" as maintenanceDatabase,
                        db.""username"" as username,db.""role"" as role,db.""service"" as service,db.""password"" as password,db.""databaseType"" as databaseType
                        from cms.""N_SWS_DbConnection"" as db 
                        where db.""NtsNoteId""='{noteId}' and db.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DbConnectionViewModel>(query, null);
            return querydata;
        }
        public async Task<ApiConnectionViewModel> GetApiConnectionDetails(string noteId)
        {
            var query = @$" select db.""NtsNoteId"" as NoteId,db.""restApiUrl"" as restApiUrl,db.""parameters"" as parameters,db.""userName"" as userName,
                        db.""password"" as password,db.""apiKey"" as apiKey,db.""pollingTime"" as pollingTime
                        from cms.""N_SWS_ApiConnection"" as db 
                        where db.""NtsNoteId""='{noteId}' and db.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<ApiConnectionViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetSharedList(string NoteId)
        {
            string query = @$"select u.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsNoteShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false 
                              
                              where n.""NtsNoteId""='{NoteId}' and n.""IsDeleted""=false 
                                union
select u.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsNote"" as n
                               join public.""User"" as u ON u.""Id"" = n.""RequestedByUserId"" and u.""IsDeleted""=false 
                              
                              where n.""Id""='{NoteId}' and n.""IsDeleted""=false 
union
select u.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsNote"" as n
                               join public.""User"" as u ON u.""Id"" = n.""OwnerUserId"" and u.""IsDeleted""=false 
                              
                              where n.""Id""='{NoteId}' and n.""IsDeleted""=false 
                                union

select u.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsNoteShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false 
 join public.""TeamUser"" as tu ON tu.""TeamId"" = n.""SharedWithTeamId"" and tu.""IsDeleted""=false 
join public.""User"" as u ON u.""Id"" = tu.""UserId"" and u.""IsDeleted""=false 
                              where n.""NtsNoteId""='{NoteId}' and n.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list.Distinct().ToList();

        }
        public async Task<List<RssFeedViewModel>> GetRssFeedData()
        {
            var query = @$" select n.*,db.""feedName"" as feedName,db.""feedUrl"" as feedUrl
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_SWS_RssFeedMaster"" as db on db.""NtsNoteId""=n.""Id"" and db.""IsDeleted""=false 
                        where t.""Name""='N_SWS_RssFeedMaster' and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<RssFeedViewModel>(query, null);
            return querydata;
        }
        public async Task<List<RssFeedViewModel>> GetRssFeedDataForScheduling()
        {
            var query = @$" select n.""NoteSubject"",f.""feedName"",f.""feedUrl"",n.""LastUpdatedDate""
                            from  cms.""N_SWS_RssFeedMaster"" as f
                            join public.""NtsNote"" as n on n.""Id""=f.""NtsNoteId"" and n.""IsDeleted""=false 
                            where f.""IsDeleted""=false 
                            order by n.""LastUpdatedDate"" desc 
                            ";
            var querydata = await _queryRepo.ExecuteQueryList<RssFeedViewModel>(query, null);
            return querydata;
        }
        public async Task<List<RssFeedViewModel>> GetRssFeedDataForSchedulingByTemplateCode(string templateCode)
        {
            var query = @$" select n.""NoteSubject"",f.""feedName"",f.""feedUrl"",n.""LastUpdatedDate""
                            from  cms.""N_SWS_RssFeedMaster"" as f
                            join public.""NtsNote"" as n on n.""Id""=f.""NtsNoteId"" and n.""IsDeleted""=false 
                            where f.""IsDeleted""=false and n.""TemplateCode""='{templateCode}'
                            order by n.""LastUpdatedDate"" desc 
                            ";
            var querydata = await _queryRepo.ExecuteQueryList<RssFeedViewModel>(query, null);
            return querydata;
        }
        public async Task<List<SchedulerSyncViewModel>> GetScheduleSyncData()
        {
            var query = @$" select n.*,db.""logstashContent"" as logstashContent,db.""trackingDate"" as trackingDate,db.""scheduleTemplate"" as scheduleTemplate
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_SWS_SchedulerSyncMaster"" as db on db.""NtsNoteId""=n.""Id"" and db.""IsDeleted""=false 
                        where t.""Name""='N_SWS_SchedulerSyncMaster'  and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<SchedulerSyncViewModel>(query, null);
            return querydata;
        }
        public async Task UpdateScheduleSyncData(string id, DateTime trackingDate, string content)
        {
            var query = @$"UPDATE cms.""N_SWS_SchedulerSyncMaster"" 
                           set ""trackingDate""='{trackingDate}',""logstashContent""='{content}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_SchedulerSyncMaster"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<RssFeedViewModel> GetRssFeedDetails(string noteId)
        {
            var query = @$" select db.""NtsNoteId"" as NoteId,db.""feedName"" as feedName,db.""feedUrl"" as feedUrl
                        from cms.""N_SWS_RssFeedMaster"" as db 
                        where db.""NtsNoteId""='{noteId}' and db.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<RssFeedViewModel>(query, null);
            return querydata;
        }
        public async Task<List<string>> GetKeywordListByTrackId(string noteId)
        {
            var query = @$" select m.""NoteSubject"" from cms.""N_SWS_TrackMaster"" as t
                        join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false  and n.""Id""='{noteId}'
                        join cms.""N_SWS_Keyword"" as k on k.""track""=t.""Id"" and k.""IsDeleted""=false 
                        join public.""NtsNote"" as m on m.""Id""=k.""NtsNoteId"" and m.""IsDeleted""=false 
                         where and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteScalarList<string>(query, null);
            return querydata;
        }
        public async Task<List<string>> GetCanAlertKeywordListByTrackId(string noteId)
        {
            var query = @$" select m.""NoteSubject"" from cms.""N_SWS_TrackMaster"" as t
                        join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false  and n.""Id""='{noteId}'
                        join cms.""N_SWS_Keyword"" as k on k.""track""=t.""Id"" and k.""canAlert""='True' and k.""IsDeleted""=false 
                        join public.""NtsNote"" as m on m.""Id""=k.""NtsNoteId"" and m.""IsDeleted""=false 
                         where t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteScalarList<string>(query, null);
            return querydata;
        }

        public async Task<IList<MapMarkerViewModel>> GetDistictByState(string stateId)
        {
            var query = @$"SELECT d.""Id"" as Id, n.""NoteSubject"" as Name,d.""pinLatitude"" as Latitude,d.""resultCount"" as Count,
                        d.""pinLongitude"" as Longitude, 'Madhya Pradesh' as State FROM cms.""N_SWS_District"" as d 
                        left join public.""NtsNote"" as n on n.""Id"" = d.""NtsNoteId"" and n.""IsDeleted""=false 
                        where  d.""state"" = '{stateId}'  and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<MapMarkerViewModel>> GetPoliceStationByDistict(string districtId)
        {
            var query = @$"SELECT d.""Id"" as Id, n.""NoteSubject"" as Name,d.""pinLatitude"" as Latitude,d.""resultCount"" as Count,
                        d.""pinLongitude"" as Longitude, 'Madhya Pradesh' as State FROM cms.""N_SWS_PoliceStation"" as d 
                        left join public.""NtsNote"" as n on n.""Id"" = d.""NtsNoteId"" and n.""IsDeleted""=false 
                        where  d.""district"" = '{districtId}'  and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<MapMarkerViewModel>> GetLocationByPoliceStation(string policeStationId)
        {
            var query = @$"SELECT d.""Id"" as Id, n.""NoteSubject"" as Name,d.""pinLatitude"" as Latitude,d.""resultCount"" as Count,
                        d.""pinLongitude"" as Longitude, 'Madhya Pradesh' as State FROM cms.""N_SWS_Location"" as d 
                        left join public.""NtsNote"" as n on n.""Id"" = d.""NtsNoteId"" and n.""IsDeleted""=false 
                        where  d.""policeStation"" = '{policeStationId}'  and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return querydata;
        }
        public async Task UpdateLocation(string id, string count)
        {
            var query = @$"UPDATE cms.""N_SWS_Location"" 
                           set ""resultCount""='{count}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_Location"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}' ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdatePoliceStation(string id, string count)
        {
            var query = @$"UPDATE cms.""N_SWS_PoliceStation"" 
                           set ""resultCount""='{count}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_PoliceStation"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateDistrict(string id, string count)
        {
            var query = @$"UPDATE cms.""N_SWS_District"" 
                           set ""resultCount""='{count}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_District"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}'";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task<IList<MapMarkerViewModel>> GetMarkersByDistrict(string locationId)
        {
            var query = @$"SELECT d.""Id"" as Id, n.""NoteSubject"" as Name,d.""pinLatitude"" as Latitude,
                        d.""pinLongitude"" as Longitude, nddd.""NoteSubject"" as District, sn.""NoteSubject"" as State,d.""resultCount"" as Count FROM cms.""N_SWS_PoliceStation"" as d 
                        left join public.""NtsNote"" as n on n.""Id"" = d.""NtsNoteId"" and n.""IsDeleted""=false 
						left join cms.""N_SWS_District"" as ndd on ndd.""Id"" = d.""district"" and ndd.""IsDeleted""=false 
						left join public.""NtsNote"" as nddd on nddd.""Id"" = ndd.""NtsNoteId"" and nddd.""IsDeleted""=false 
                        left join cms.""N_SWS_State"" as st on st.""Id"" = d.""state"" and st.""IsDeleted""=false 
						left join public.""NtsNote"" as sn on sn.""Id"" = st.""NtsNoteId"" and sn.""IsDeleted""=false 
                        where  d.""district"" = '{locationId}'  and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return querydata;
        }

        public async Task<List<TagCategoryViewModel>> GetTrackList()
        {
            var query = @$"SELECT tm.""Id"" as Id, n.""NoteSubject"" as Name FROM cms.""N_SWS_TrackMaster"" as tm
                            left join public.""NtsNote"" as n on n.""Id"" = tm.""NtsNoteId"" and n.""IsDeleted""=false 
                            where tm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }

        public async Task<List<TagCategoryViewModel>> GetKeywordList(string trackId)
        {
            var query = @$"SELECT tm.""Id"" as Id, n.""NoteSubject"" as Name FROM cms.""N_SWS_Keyword"" as tm
                            left join public.""NtsNote"" as n on n.""Id"" = tm.""NtsNoteId"" and n.""IsDeleted""=false 
                            where tm.""track"" = '{trackId}' and tm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }

        public async Task<List<NoteViewModel>> GetDocumentByNoteAndRevision(string templateId, string noteno, string revision)
        {
            var templateDetails = await _repo.GetSingleById<TemplateViewModel, Template>(templateId);
            var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(templateDetails.TableMetadataId);
            var revData = await _repo.GetSingle<LOVViewModel, LOV>(x => x.Name == revision && x.LOVType == "REVISION");
            if (templateDetails.IsNotNull() && tableMetadata.IsNotNull() && revData.IsNotNull())
            {
                //  JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId""

                var query = @$"select distinct n.* from public.""NtsNote"" as n 
                                join public.""Template"" as t on t.""Id"" = n.""TemplateId"" and t.""IsDeleted""=false 
                                join cms.""{tableMetadata.Name}"" p ON n.""Id"" = p.""NtsNoteId"" 
                                where n.""NoteNo"" = '{noteno}' and t.""Id"" ='{templateId}' and p.""revision"" = '{revData.Id}' and n.""IsDeleted""=false  ";

                var querydata = await _queryRepo.ExecuteQueryList<NoteViewModel>(query, null);
                return querydata;
            }
            return null;
        }

        public async Task<List<ColumnMetadataViewModel>> DynamicUdfColumns(string templateId)
        {
            var query = @$"select
                            cm.* from public.""Template"" as t
                            join public.""TableMetadata"" as tm on tm.""Id"" = t.""TableMetadataId"" and tm.""IsDeleted""=false 
                            join public.""ColumnMetadata"" as cm  on cm.""TableMetadataId"" = tm.""Id"" and cm.""IsDeleted""=false 
                            where t.""Id"" = '{templateId}' and cm.""IsUdfColumn"" = true and t.""IsDeleted""=false ";

            var querydata = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return querydata;
        }

        public async Task<string> GetServiceWorkflowTemplateId(long id)
        {
            var query = @$"select
                            cm.* from public.""Template"" as t
                            join public.""TableMetadata"" as tm on tm.""Id"" = t.""TableMetadataId"" and tm.""IsDeleted""=false 
                            join public.""ColumnMetadata"" as cm  on cm.""TableMetadataId"" = tm.""Id"" and cm.""IsDeleted""=false 
                            where t.""Id"" = '{id}' and cm.""IsUdfColumn"" = true and t.""IsDeleted""=false ";

            var querydata = await _queryRepo.ExecuteScalar<string>(query, null);
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
            var query = $@" select udf.""documentId"" as ""Id"" from public.""NtsService"" as s
                                join (" + udfs + $@") as udf on udf.""NtsNoteId""=s.""UdfNoteId"" 
                                where s.""IsDeleted""=false  and s.""Id""='{serviceId}' ";
            var result = await _queryRepo.ExecuteScalar<string>(query, null);
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
                var Query = $@"select f.*,case when fl.""Id"" is not null then fl.""AnnotationsText"" else f.""AnnotationsText"" end as ""AnnotationsText""  from public.""NtsNote"" as n 

                                join public.""File"" as f on f.""ReferenceTypeId""=n.""Id"" and f.""IsDeleted""=false
                                left join log.""FileLog"" as fl on fl.""RecordId""=f.""Id"" and fl.""IsLatest""=true
                                where n.""Id""='{NoteId}' and n.""IsDeleted""=false 
                                union
                                select f.*,case when fl.""Id"" is not null then fl.""AnnotationsText"" else f.""AnnotationsText"" end as ""AnnotationsText"" from public.""NtsService"" as s
                                join (" + udfs + $@") as udf on udf.""NtsNoteId""=s.""UdfNoteId"" 
                                join public.""File"" as f on f.""ReferenceTypeId""=s.""Id"" and f.""IsDeleted""=false 
                                left join log.""FileLog"" as fl on fl.""RecordId""=f.""Id"" and fl.""IsLatest""=true
                                where udf.""documentId""='{NoteId}' and s.""IsDeleted""=false 
                                union
                                select f.* ,case when fl.""Id"" is not null then fl.""AnnotationsText"" else f.""AnnotationsText"" end as ""AnnotationsText""                                                     
                                FROM public.""NtsTask"" as t
                                join public.""Template"" as temp on temp.""Id""=t.""TemplateId"" and temp.""IsDeleted""=false 
                                join public.""LOV"" as lv on lv.""Id""=t.""TaskStatusId"" and lv.""IsDeleted""=false 
                                join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false
			                    join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false 
                                join (" + udfs + $@") as udf on udf.""NtsNoteId""=s.""UdfNoteId"" 
                                join public.""File"" as f on f.""ReferenceTypeId""=t.""Id""  and f.""IsDeleted""=false 	
                                left join log.""FileLog"" as fl on fl.""RecordId""=f.""Id"" and fl.""IsLatest""=true
                                WHERE t.""IsDeleted""=false  and udf.""documentId""='{NoteId}' and t.""TaskType""='2' 
";
                var files = await _queryRepo.ExecuteQueryList<FileViewModel>(Query, null);

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
            var notelist = await this.GetList(x => x.NoteNo == noteNo && x.Id != noteId && x.TemplateId == templateId);

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
            var cypher = $@"select n.""Id"" as Id from 
                         cms.""N_GENERAL_FOLDER_WORKSPACE"" as n 
                         where n.""NtsNoteId""='{noteId}' and n.""IsDeleted""=false 
                           ";

            var value = await _queryRepo.ExecuteQuerySingle<NoteViewModel>(cypher, null);
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
                var query = $@"update  cms.""N_GENERAL_FOLDER_WORKSPACE"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<string> GetFolderWorkspace(string id)
        {
            var cypher = $@"select wp.""WorkspaceId""  from cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" and n.""IsDeleted""=false 
                            where n.""Id"" = '{id}'and wp.""IsDeleted""=false  ";
            var value = await _queryRepo.ExecuteScalar<string>(cypher, null);
            return value;
        }

        public async Task<List<NtsLogViewModel>> GetVersionDetails(string noteId)
        {
            var query = $@"select l.""NoteSubject"" as Subject, l.* from log.""NtsNoteLog""  as l
            join public.""NtsNote"" as n on l.""RecordId""=n.""Id"" and n.""IsDeleted""=false  and l.""VersionNo""<>n.""VersionNo""
            where l.""IsDeleted""=false  and  l.""RecordId"" = '{noteId}' and l.""IsVersionLatest""=true order by l.""VersionNo"" desc";
            var result = await _queryNtsLog.ExecuteQueryList(query, null);
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
            var query = @$"select ""TM"".*,""S"".""Id"" as ""NoteId"",""T"".""DisplayName"" as ""TemplateDisplayName""
            from ""public"".""NoteTemplate"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""Id""=""TM"".""TemplateId"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsNote"" as ""S"" on ""T"".""Id""=""S"".""TemplateId"" and ""S"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false  and ""S"".""Id""='{noteId}'";
            var model = await _queryRepo.ExecuteQuerySingle<NoteTemplateViewModel>(query, null);
            model.BookItems = await GetBookList(noteId, null, true);
            return model;
        }
        public async Task<List<NtsViewModel>> GetBookList(string noteId, string templateId, bool includeitemDetails = false)
        {
            string query = @$"select t.""DisplayName"" as ""TemplateName"",s.""NoteSubject"" as ""Subject"",s.""NoteDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,null as ""parentId""
            ,0 as ""Level"",'Note' as ""NtsType"",s.""Id"" as ""Id"",1 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""ExpiryDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""NoteNo"" as ""NtsNo"",true as ""AutoLoad"",0 as ""SequenceOrder"",'1' as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsNote"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""NoteTemplate"" as nt on t.""Id""=nt.""TemplateId"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            left join public.""LOV"" as lp on lp.""Id""=s.""NotePriorityId"" and lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            where s.""Id""='{noteId}' and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            union
            select t.""DisplayName"" as ""TemplateName"",s.""NoteSubject"" as ""Subject"",s.""NoteDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentServiceId"",s.""ParentTaskId"",s.""ParentNoteId"") as parentId
            ,1 as ""Level"",'Note' as NtsType,s.""Id"" as Id,14 as ItemType,u.""Name"" as AssigneeOrOwner
            ,s.""ExpiryDate"" as DueDate,s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""NoteNo"" as NtsNo,false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as StartDate,s.""ReminderDate"" as ReminderDate,s.""CompletedDate"" as CompletedDate
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsNote"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""NoteTemplate"" as nt on t.""Id""=nt.""TemplateId"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            left join public.""LOV"" as lp on lp.""Id""=s.""NotePriorityId"" and lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            where s.""NotePlusId""='{noteId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            union
            select t.""DisplayName"" as ""TemplateName"",s.""TaskSubject"" as ""Subject"",s.""TaskDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,'0' as ""parentId""
            ,1 as ""Level"",'Task' as ""NtsType"",s.""Id"" as ""Id"",2 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""TaskNo"" as ""NtsNo"",false as ""AutoLoad"",0 as ""SequenceOrder"",'1' as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsTask"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""TaskTemplate"" as nt on t.""Id""=nt.""TemplateId"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""TaskStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            left join public.""LOV"" as lp on lp.""Id""=s.""TaskPriorityId"" and lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            where s.""NotePlusId""='{noteId}' and s.""TaskType""=4 and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            union
            select t.""DisplayName"" as ""TemplateName"",s.""ServiceSubject"" as ""Subject"",s.""ServiceDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentServiceId"",s.""ParentTaskId"",s.""ParentNoteId"") as ""parentId""
            ,1 as ""Level"",'Service' as ""NtsType"",s.""Id"" as ""Id"",13 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""ServiceNo"" as ""NtsNo"",false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsService"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""ServiceTemplate"" as nt on t.""Id""=nt.""TemplateId"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            left join public.""LOV"" as lp on lp.""Id""=s.""ServicePriorityId"" and lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            where s.""NotePlusId""='{noteId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var list = await _queryRepo.ExecuteQueryList<NtsViewModel>(query, null);
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
            var query = @$"select ""NT"".*,""TM"".""Name"" as ""TableName"" ,""lov"".""Code"" as ""NoteStatusCode""
            ,""TM"".""Id"" as ""TableMetadataId"",""T"".""Json"" as ""Json""
            from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""TableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""NoteTemplate"" as ""NT"" on ""T"".""Id""=""NT"".""TemplateId"" and ""NT"".""IsDeleted""=false 
            join ""public"".""NtsNote"" as ""N"" on ""T"".""Id""=""N"".""TemplateId"" and ""N"".""IsDeleted""=false 
            join ""public"".""LOV"" as ""lov"" on ""N"".""NoteStatusId""=""lov"".""Id"" and ""lov"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false   and ""N"".""Id""='{noteId}'";
            var data = await _queryRepo.ExecuteQuerySingle<NoteTemplateViewModel>(query, null);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.TableMetadataId
                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
                data.AllReadOnly = true;
                GetUdfDetails(data);
                var selectQuery = @$"select * from cms.""{data.TableName}"" where ""NtsNoteId""='{noteId}' limit 1";
                var dr = await _queryRepo.ExecuteQueryDataRow(selectQuery, null);
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
            //var cypher = $@"Select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
            //                from public.""NtsNote"" as s                         
            //                join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false and tr.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}' 
            //                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
            //                join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}'  and u.""IsDeleted""=false 
            //                join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
            //               left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
            //               where 1=1 and s.""PortalId""='{_userContex.PortalId}' and s.""IsDeleted""=false ";
            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType
 from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContex.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContex.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE# ";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var note = await _queryRepo.ExecuteQueryList(cypher, null);
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
            //var cypher = $@"Select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
            //                from public.""NtsNote"" as s                         
            //                join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false and tr.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}' 
            //                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
            //                join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}'  and u.""IsDeleted""=false 
            //                join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
            //               left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
            //               where 1=1 and s.""PortalId""='{_userContex.PortalId}' and s.""IsDeleted""=false ";
            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType
 from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContex.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContex.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE# ";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var note = await _queryRepo.ExecuteQueryList(cypher, null);
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
            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType
 from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""Id""=n.""NotePlusId"" and pn.""IsDeleted"" = false                          
join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContex.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
join public.""NtsService"" as pn on pn.""Id""=n.""ParentServiceId"" and pn.""Id""=n.""ServicePlusId"" and pn.""IsDeleted"" = false
join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContex.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE# ";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var note = await _queryRepo.ExecuteQueryList(cypher, null);
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
            var cypher = $@"select wp.*  from cms.""N_BusinessAnalytics_SynergyWebsite"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" and n.""IsDeleted""=false 
                            where n.""Id"" = '{id}'and wp.""IsDeleted""=false  ";
            var value = await _queryRepo.ExecuteQuerySingle<SynergyWebsiteViewModel>(cypher, null);
            return value;
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsite()
        {
            var cypher = $@"select wp.*,n.""Id"" as NoteId,n.""NoteSubject"" as Name  from cms.""N_BusinessAnalytics_SynergyWebsite"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" 
                            where n.""IsDeleted""=false  and wp.""IsDeleted""=false  ";
            var list = await _queryRepo.ExecuteQueryList<SynergyWebsiteViewModel>(cypher, null);
            return list.ToList();
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsiteNote()
        {
            var cypher = $@"select wp.*,n.""Id"" as Id,n.""Id"" as NoteId,n.""NoteSubject"" as Name  from cms.""N_BusinessAnalytics_SynergyWebsite"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" 
                            where n.""IsDeleted""=false  and wp.""IsDeleted""=false and wp.""IsTemplate""='False' ";
            var list = await _queryRepo.ExecuteQueryList<SynergyWebsiteViewModel>(cypher, null);
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
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.count') as ""name"",CONCAT(ss.""SchemaName"",'.count') as ""title""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MeasuresViewModel>(query, null);
            return querydata;
        }
        public async Task<List<DimensionsViewModel>> GetDimensions()
        {
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""name"",
                            CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""title""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id""
                            join public.""NtsNote"" as d on d.""ParentNoteId""=n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<DimensionsViewModel>(query, null);
            return querydata;
        }
        public async Task<List<DimensionsViewModel>> GetDimensionsByMeasue(string measure)
        {
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""name"",
                            CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""title""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id"" and CONCAT(ss.""SchemaName"",'.count')='{measure}'
                            join public.""NtsNote"" as d on d.""ParentNoteId""=n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<DimensionsViewModel>(query, null);
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
    }
}
