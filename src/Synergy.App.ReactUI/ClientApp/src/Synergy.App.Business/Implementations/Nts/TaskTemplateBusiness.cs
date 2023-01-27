using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class TaskTemplateBusiness : BusinessBase<TaskTemplateViewModel, TaskTemplate>, ITaskTemplateBusiness
    {
        ITableMetadataBusiness _tableMetadataBusiness;
        INoteTemplateBusiness _noteTemplateBusiness;
        INotificationTemplateBusiness _notificationTemplateBusiness;
        IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo1;
        public TaskTemplateBusiness(IRepositoryBase<TaskTemplateViewModel, TaskTemplate> repo, IMapper autoMapper
            , ITableMetadataBusiness tableMetadataBusiness
            , INoteTemplateBusiness noteTemplateBusiness
       , INotificationTemplateBusiness notificationTemplateBusiness, IRepositoryBase<TableMetadataViewModel, TableMetadata> repo1
            ) : base(repo, autoMapper)
        {
            _tableMetadataBusiness = tableMetadataBusiness;
            _noteTemplateBusiness = noteTemplateBusiness;
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _repo1 = repo1;
        }

        public async override Task<CommandResult<TaskTemplateViewModel>> Create(TaskTemplateViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<TaskTemplateViewModel>(model);
            data.EnableIndexPage = true;
            data.EnableSaveAsDraft = true;
            data.EnableCancelButton = true;
            data.EnableRejectButton = true;
            data.EnableBackButton = true;
            var errorList = new Dictionary<string, string>();
            var newtablemeta = new TableMetadataViewModel();
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var columns = JObject.Parse(model.Json);
                JArray rows = (JArray)columns.SelectToken("components");
                var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (temp.IsNotNull())
                {
                    var table = await _tableMetadataBusiness.GetSingleById(temp.UdfTableMetadataId);
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
                            return CommandResult<TaskTemplateViewModel>.Instance(model, false, errorList);
                        }

                    }

                }

            }


            var result = await base.Create(data, autoCommit);
            if (result.IsSuccess)
            {
                var notifcationList = await _notificationTemplateBusiness.
                    GetList(x => x.IsTemplate == true && x.NtsType == NtsTypeEnum.Task);
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(result.Item.TemplateId);
                foreach (var item in notifcationList)
                {
                    var notification = new NotificationTemplateViewModel();
                    notification.Id = null;
                    notification.Code = $"{template?.Code}_{item.Code}";
                    notification.Name = $"{template?.Code}_{item.Name}";
                    notification.ParentNotificationTemplateId = item.Id;
                    notification.TemplateId = result.Item.TemplateId;
                    notification.IsTemplate = false;
                    notification.NtsType = NtsTypeEnum.Task;
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
                        serviceresult.Messages.Add("TaskTemplateName", "Task Template : Created.");
                        return CommandResult<TaskTemplateViewModel>.Instance(model, serviceresult.IsSuccess, serviceresult.Messages);
                    }
                }

            }
            else
            {
                return CommandResult<TaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return result;
        }

        public async override Task<CommandResult<TaskTemplateViewModel>> Edit(TaskTemplateViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<TaskTemplateViewModel>(model);
            var errorList = new Dictionary<string, string>();
            var newtablemeta = new TableMetadataViewModel();
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var columns = JObject.Parse(model.Json);
                JArray rows = (JArray)columns.SelectToken("components");
                var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (temp.IsNotNull())
                {
                    var table = await _tableMetadataBusiness.GetSingleById(temp.UdfTableMetadataId);
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
                            return CommandResult<TaskTemplateViewModel>.Instance(model, false, errorList);
                        }
                    }
                }

            }



            var result = await base.Edit(data,autoCommit);
            if (result.IsSuccess)
            {
                //var tableResult = await _tableMetadataBusiness.ManageTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Note });
                //if (!tableResult.IsSuccess)
                //{
                //    return CommandResult<TaskTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                //}
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                    //template.Json = model.Json;
                    //await _repo.Edit<TemplateViewModel, Template>(template);
                    //var tableResult = await _tableMetadataBusiness.ManageTemplateTable(template);

                    //var notemodel = _autoMapper.Map<TaskTemplateViewModel, NoteTemplateViewModel>(model);
                    var notetemplate = await _repo.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == template.UdfTemplateId);
                    if (notetemplate != null)
                    {
                        notetemplate.Json = model.Json;
                        if (model.TaskTemplateType != TaskTypeEnum.StandardTask && model.TaskTemplateType != TaskTypeEnum.AdhocTask)
                        {
                            notetemplate.IgnorePermission = true;
                        }

                        var tableResult = await _noteTemplateBusiness.Edit(notetemplate);
                        if (!tableResult.IsSuccess)
                        {
                            return CommandResult<TaskTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                        }
                        else
                        {
                            template.Json = notetemplate.Json;
                            await _repo.Edit<TemplateViewModel, Template>(template);
                        }
                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> ManageCreate(TaskTemplateViewModel model, bool autoCommit = true)
        {

            var result = await base.Create(model, autoCommit);

            return CommandResult<TaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> ManageEdit(TaskTemplateViewModel model, bool autoCommit = true)
        {

            var result = await base.Edit(model, autoCommit);

            return CommandResult<TaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> CopyTaskTemplate(TaskTemplateViewModel oldModel, string newTempId, CopyTemplateViewModel copyModel = null)
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
            newModel.TaskIndexPageTemplateId = model.TaskIndexPageTemplateId;
            newModel.CreatedDate = model.CreatedDate;
            newModel.CreatedBy = model.CreatedBy;
            newModel.LastUpdatedDate = model.LastUpdatedDate;
            newModel.LastUpdatedBy = model.LastUpdatedBy;
            newModel.CompanyId = model.CompanyId;
            newModel.LegalEntityId = model.LegalEntityId;

            
            if (oldTempData.Json.IsNotNullAndNotEmpty())
            {
                string json = Helper.ReplaceJsonProperty(oldTempData.Json, "columnMetadataId");
                newModel.Json = json;
            }
            var taskresult = await Edit(newModel);
            return taskresult;
        }

    }
}
