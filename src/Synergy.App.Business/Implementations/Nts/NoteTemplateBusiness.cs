using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class NoteTemplateBusiness : BusinessBase<NoteTemplateViewModel, NoteTemplate>, INoteTemplateBusiness
    {
        INotificationTemplateBusiness _noteNotificationTemplateBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        INotificationTemplateBusiness _notificationTemplateBusiness;
        IColumnMetadataBusiness _columnMetadataBusiness;
        IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo1;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepoIdname;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        public NoteTemplateBusiness(IRepositoryBase<NoteTemplateViewModel, NoteTemplate> repo, IRepositoryQueryBase<IdNameViewModel> queryRepoIdname, IMapper autoMapper,
            INotificationTemplateBusiness noteNotificationTemplateBusiness, ITableMetadataBusiness tableMetadataBusiness
            , IRepositoryBase<TableMetadataViewModel, TableMetadata> repo1
                   , INotificationTemplateBusiness notificationTemplateBusiness, IColumnMetadataBusiness columnMetadataBusiness
            , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _noteNotificationTemplateBusiness = noteNotificationTemplateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _repo1 = repo1;
            _queryRepoIdname = queryRepoIdname;
            _ntsQueryBusiness = ntsQueryBusiness;
        }

        public async override Task<CommandResult<NoteTemplateViewModel>> Create(NoteTemplateViewModel model, bool autoCommit = true)
        {
            model.EnableIndexPage = true;
            model.EnableSaveAsDraft = true;
            model.EnableCancelButton = true;
            model.EnableBackButton = true;
            var errorList = new Dictionary<string, string>();

            if (model.Json.IsNotNullAndNotEmpty())
            {
                var columns = JObject.Parse(model.Json);
                JArray rows = (JArray)columns.SelectToken("components");
                var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (temp.IsNotNull())
                {
                    var table = await _tableMetadataBusiness.GetSingleById(temp.TableMetadataId);
                    if (table != null)
                    {
                        table.ColumnMetadatas = new List<ColumnMetadataViewModel>();
                        table.ChildTable = new List<TableMetadataViewModel>();

                        await _tableMetadataBusiness.ChildComp(rows, table, 1);
                    
                    var newtablemeta = new TableMetadataViewModel();
                    var basecolumnlist = _tableMetadataBusiness.AddBaseColumns(newtablemeta, _repo1, DataActionEnum.Create).ToList();
                    var validatelist = table.ColumnMetadatas.Where(x => basecolumnlist.Any(y => y.Name.ToLower() == x.Name.ToLower())).ToList();
                    if (validatelist.Count > 0)
                    {
                        var colnameslist = validatelist.Select(x => x.Name);
                        var colnames = "[" + string.Join(",", colnameslist) + "]";
                        errorList.Add("BaseColumns", "The columns " + colnames + " are reserved columns, You cannot add these columns");
                        return CommandResult<NoteTemplateViewModel>.Instance(model, false, errorList);
                    }
                    }
                }
               
            }

            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                var tableResult = await _tableMetadataBusiness.ManageTemplateTable(template, false, model.ParentTemplateId);
                if (tableResult.IsSuccess)
                {
                    model.TemplateViewModel.TableMetadataId = tableResult.Item.Id;
                    var notifcationList = await _notificationTemplateBusiness.
                    GetList(x => x.IsTemplate == true && x.NtsType == NtsTypeEnum.Note);
                    var template1 = await _repo.GetSingleById<TemplateViewModel, Template>(result.Item.TemplateId);
                    foreach (var item in notifcationList)
                    {
                        var notification = new NotificationTemplateViewModel();
                        notification.Id = null;
                        notification.Code = $"{template1?.Code}_{item.Code}";
                        notification.Name = $"{template1?.Code}_{item.Name}";
                        notification.ParentNotificationTemplateId = item.Id;
                        notification.TemplateId = result.Item.TemplateId;
                        notification.IsTemplate = false;
                        notification.NtsType = NtsTypeEnum.Note;
                        notification.DataAction = DataActionEnum.Create;
                        notification.ActionType = item.ActionType;
                        notification.ActionStatusCodes = item.ActionStatusCodes;
                        var serviceresult = await _notificationTemplateBusiness.Create(notification);
                        //var service = _autoMapper.Map<NotificationTemplateViewModel, NotificationTemplateViewModel>(item);
                        //service.Id = null;
                        //service.ParentNotificationTemplateId = item.Id;
                        //service.TemplateId = result.Item.Id;
                        //service.IsTemplate = false;
                        //service.DataAction = DataActionEnum.Create;
                        //var serviceresult = await _notificationTemplateBusiness.Create(service);
                        if (!serviceresult.IsSuccess)
                        {
                            serviceresult.Messages.Add("NoteTemplateName", "Note Template : Created.");
                            return CommandResult<NoteTemplateViewModel>.Instance(model, serviceresult.IsSuccess, serviceresult.Messages);
                        }
                    }
                }
                else
                {
                    return CommandResult<NoteTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                }
            }
            return CommandResult<NoteTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteTemplateViewModel>> Edit(NoteTemplateViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<NoteTemplateViewModel>(model);
            var errorList = new Dictionary<string, string>();
            var newtablemeta = new TableMetadataViewModel();
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var columns = JObject.Parse(model.Json);
                JArray rows = (JArray)columns.SelectToken("components");
                var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (temp.IsNotNull())
                {
                    var table = await _tableMetadataBusiness.GetSingleById(temp.TableMetadataId);
                    if (table != null)
                    {
                        table.ColumnMetadatas = new List<ColumnMetadataViewModel>();
                        table.ChildTable = new List<TableMetadataViewModel>();

                        await _tableMetadataBusiness.ChildComp(rows, table, 1);
                   
                    var basecolumnlist = _tableMetadataBusiness.AddBaseColumns(newtablemeta, _repo1, DataActionEnum.Edit).ToList();
                    var curColumnList = table.ColumnMetadatas.ToList();
                    var validatelist = curColumnList.Where(x => basecolumnlist.Any(y => y.Name.ToLower() == x.Name.ToLower())).ToList();
                    if (validatelist.Count > 0)
                     {
                        var colnameslist = validatelist.Select(x => x.Name);
                        var colnames = "[" + string.Join(",", colnameslist) + "]";
                        errorList.Add("BaseColumns", "The columns " + colnames + " are reserved columns, You cannot add these columns");
                        return CommandResult<NoteTemplateViewModel>.Instance(model, false, errorList);
                     }
                    }
                }
              

            }

            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                //var tableResult = await _tableMetadataBusiness.ManageTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Note });
                //if (!tableResult.IsSuccess) 
                //{
                //    return CommandResult<NoteTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                //}
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                    template.Json = model.Json;
                

                    var tableResult = await _tableMetadataBusiness.ManageTemplateTable(template, model.IgnorePermission, model.ParentTemplateId);

                    if (!tableResult.IsSuccess)
                    {
                        return CommandResult<NoteTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                    }
                    else
                    {
                        model.Json = template.Json;
                        await base.Edit(model,autoCommit);
                        await _repo.Edit<TemplateViewModel, Template>(template);
                    }
                }
            }
            return CommandResult<NoteTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<IdNameViewModel>> GetAdhocNoteTemplateList()
        {
            var querydata = await _ntsQueryBusiness.GetAdhocNoteTemplateListData();
            return querydata;
        }

        public async Task<CommandResult<NoteTemplateViewModel>> CopyNoteTemplate(NoteTemplateViewModel oldModel, string newTempId, CopyTemplateViewModel copyModel = null)
        {
            TemplateViewModel oldTempData = new();
            if (copyModel.IsNotNull())
            {
                oldTempData = copyModel.Template;   
            }
            else
            {
                oldTempData = await _repo.GetSingleById<TemplateViewModel, Template>(oldModel.TemplateId);
            }
            
            
            var newModel = await _repo.GetSingle(x => x.TemplateId == newTempId);
            var model = newModel;
            newModel = oldModel;

            newModel.Id = model.Id;
            newModel.TemplateId = model.TemplateId;
            newModel.NoteIndexPageTemplateId = model.NoteIndexPageTemplateId;
            newModel.CreatedDate = model.CreatedDate;
            newModel.CreatedBy = model.CreatedBy;
            newModel.LastUpdatedDate = model.LastUpdatedDate;
            newModel.LastUpdatedBy = model.LastUpdatedBy;
            newModel.CompanyId = model.CompanyId;
            newModel.LegalEntityId = model.LegalEntityId;

            var json = Helper.ReplaceJsonProperty(oldTempData.Json, "columnMetadataId");

            newModel.Json = json;


            var noteresult = await Edit(newModel);
            return noteresult;

        }

    }
}
