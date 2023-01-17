using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ServiceTemplateBusiness : BusinessBase<ServiceTemplateViewModel, ServiceTemplate>, IServiceTemplateBusiness
    {
        ITableMetadataBusiness _tableMetadataBusiness;
        IServiceIndexPageTemplateBusiness _serviceIndexPageBusiness;
        INoteTemplateBusiness _noteTemplateBusiness;
        INotificationTemplateBusiness _notificationTemplateBusiness;
        IColumnMetadataBusiness _columnMetadataBusiness;
        IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo1;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepoIdname;
        public ServiceTemplateBusiness(IRepositoryBase<ServiceTemplateViewModel, ServiceTemplate> repo, IRepositoryQueryBase<IdNameViewModel> queryRepoIdname, IMapper autoMapper,
            ITableMetadataBusiness tableMetadataBusiness
            , IServiceIndexPageTemplateBusiness serviceIndexPageBusiness
            , INoteTemplateBusiness noteTemplateBusiness, IRepositoryBase<TableMetadataViewModel, TableMetadata> repo1
            , INotificationTemplateBusiness notificationTemplateBusiness
            , IColumnMetadataBusiness columnMetadataBusiness
            , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _tableMetadataBusiness = tableMetadataBusiness;
            _serviceIndexPageBusiness = serviceIndexPageBusiness;
            _noteTemplateBusiness = noteTemplateBusiness;
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _repo1 = repo1;
            _queryRepoIdname = queryRepoIdname;
            _ntsQueryBusiness = ntsQueryBusiness;
        }

        public async override Task<CommandResult<ServiceTemplateViewModel>> Create(ServiceTemplateViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<ServiceTemplateViewModel>(model);
            data.EnableIndexPage = true;
            data.EnableSaveAsDraft = true;
            data.EnableCancelButton = true;
            data.EnableBackButton = true;

            var errorList = new Dictionary<string, string>();
            var newtablemeta = new TableMetadataViewModel();
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var columns = JObject.Parse(model.Json);
                JArray rows = (JArray)columns.SelectToken("components");
                var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
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
                    return CommandResult<ServiceTemplateViewModel>.Instance(model, false, errorList);
                }
                }
            }


            var result = await base.Create(data, autoCommit);
            if (result.IsSuccess)
            {
                var notifcationList = await _notificationTemplateBusiness.
                    GetList(x => x.IsTemplate == true && x.NtsType == NtsTypeEnum.Service);
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
                    notification.NtsType = NtsTypeEnum.Service;
                    notification.DataAction = DataActionEnum.Create;
                    notification.ActionType = item.ActionType;
                    notification.ActionStatusCodes = item.ActionStatusCodes;
                    var serviceresult = await _notificationTemplateBusiness.Create(notification);
                    if (!serviceresult.IsSuccess)
                    {
                        serviceresult.Messages.Add("ServiceTemplateName", "Service Template : Created.");
                        return CommandResult<ServiceTemplateViewModel>.Instance(model, serviceresult.IsSuccess, serviceresult.Messages);
                    }
                }


            }
            return CommandResult<ServiceTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ServiceTemplateViewModel>> Edit(ServiceTemplateViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<ServiceTemplateViewModel>(model);

            var errorList = new Dictionary<string, string>();
            var newtablemeta = new TableMetadataViewModel();
            if (model.Json.IsNotNullAndNotEmpty())
            {
                var columns = JObject.Parse(model.Json);
                JArray rows = (JArray)columns.SelectToken("components");
                var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
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
                    return CommandResult<ServiceTemplateViewModel>.Instance(model, false, errorList);
                }
                }

            }


            var result = await base.Edit(data,autoCommit);
            if (result.IsSuccess)
            {
                //var tableResult = await _tableMetadataBusiness.ManageTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Note });
                //if (!tableResult.IsSuccess)
                //{
                //    return CommandResult<ServiceTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                //}
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(model.TemplateId);
                if (template != null)
                {
                   // template.Json = model.Json;
                    
                    //var tableResult = await _tableMetadataBusiness.ManageTemplateTable(template);
                    var notetemplate = await _repo.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == template.UdfTemplateId);
                    if (notetemplate != null)
                    {
                        notetemplate.Json = model.Json;
                        notetemplate.IgnorePermission = true;
                        notetemplate.ParentTemplateId = model.TemplateId;
                        var tableResult = await _noteTemplateBusiness.Edit(notetemplate);
                        if (!tableResult.IsSuccess)
                        {
                            return CommandResult<ServiceTemplateViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                        }
                        else
                        {
                            template.Json = notetemplate.Json;
                            await _repo.Edit<TemplateViewModel, Template>(template);
                        }
                    }
                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
       
        public async Task<ServiceTemplateViewModel> GetServiceTemplateByTemplateId(string templateId)
        {
            return await _repo.GetSingle(x => x.IsDeleted == false && x.TemplateId == templateId);
        }
        public async Task<IList<IdNameViewModel>> GetAdhocServiceTemplateList()
        {
            var list = await _ntsQueryBusiness.GetAdhocServiceTemplateListData();
            return list;
        }
    }
}
