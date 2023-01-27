using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using SpreadsheetLight;
using Synergy.App.Common.Utilities;

namespace Synergy.App.Business
{
    public class TableMetadataBusiness : BusinessBase<TableMetadataViewModel, TableMetadata>, ITableMetadataBusiness
    {
        IColumnMetadataBusiness _columnMetadataBusiness;
        ICmsBusiness _cmsBusiness;
        IRepositoryQueryBase<TableMetadataViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public TableMetadataBusiness(IRepositoryBase<TableMetadataViewModel, TableMetadata> repo, IMapper autoMapper
            , IColumnMetadataBusiness columnMetadataBusiness
            , ICmsBusiness cmsBusiness
            , IRepositoryQueryBase<TableMetadataViewModel> queryRepo
            , ICmsQueryBusiness cmsQueryBusiness
            , IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _columnMetadataBusiness = columnMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
            _serviceProvider = serviceProvider;
        }

        public async override Task<CommandResult<TableMetadataViewModel>> Create(TableMetadataViewModel model, bool autoCommit = true)
        {
            var errorList = new Dictionary<string, string>();
            var dispname = await base.GetSingle(x => x.DisplayName == model.DisplayName);
            if (dispname != null)
            {
                errorList.Add("DisplayName", "Display name already exist.");
            }
            var tablename = await base.GetSingle(x => x.Name == model.Name);
            if (tablename != null)
            {
                errorList.Add("Name", "Name already exist.");
            }
            var tablealias = await base.GetSingle(x => x.Alias == model.Alias);
            if (tablealias != null)
            {
                errorList.Add("Alias", "Alias/Short name already exist.");
            }

            if (model.ColumnMetadatas.IsNotNull())
            {
                foreach (var col in model.ColumnMetadatas)
                {
                    if (col.ForeignKeyConstraintName.IsNotNullAndNotEmpty())
                    {
                        var columnconstraint = await _columnMetadataBusiness.GetSingle(c => c.ForeignKeyConstraintName == col.ForeignKeyConstraintName);
                        if (columnconstraint != null)
                        {
                            errorList.Add("ForeignKeyConstraintName", "Column - " + col.ForeignKeyConstraintName + ", Foreign key constraint name already exist.");
                        }
                    }

                }
            }

            if (errorList.Count > 0)
            {
                return CommandResult<TableMetadataViewModel>.Instance(model, false, errorList);
            }
            else
            {
                var result = await base.Create(model,autoCommit);
                if (model.TableType != TableTypeEnum.View)
                {
                    model.ColumnMetadatas = AddBaseColumns(model, _repo, DataActionEnum.Create);
                }
                if (result.IsSuccess)
                {
                    model.Id = result.Item.Id;
                    foreach (var col in model.ColumnMetadatas)
                    {
                        col.TableMetadataId = result.Item.Id;
                        if (col.DataAction == DataActionEnum.Create)
                        {
                            // col.IsNullable = true;
                            var colresult = await _columnMetadataBusiness.Create(col);
                            if (colresult.IsSuccess)
                            {
                                col.Id = colresult.Item.Id;
                            }
                        }
                        else if (col.DataAction == DataActionEnum.Edit)
                        {
                            var colresult = await _columnMetadataBusiness.Edit(col);
                            if (colresult.IsSuccess)
                            {
                                //col.Id = colresult.Item.Id;
                            }
                        }
                    }
                    await _cmsBusiness.ManageTable(model);

                }
                return CommandResult<TableMetadataViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }


        }
        public List<ColumnMetadataViewModel> AddBaseColumns(TableMetadataViewModel table, IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo, DataActionEnum dataAction)
        {
            var list = new List<ColumnMetadataViewModel>();
            if (table.ColumnMetadatas.IsNotNull())
            {
                list = table.ColumnMetadatas.OrderBy(x => x.SequenceOrder).ToList();
            }

            list.Insert(0, AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Id",
                LabelName = "Id",
                Alias = "Id",
                DataType = Common.DataColumnTypeEnum.Text,
                IsPrimaryKey = true,
                IsSystemColumn = true,
                IsHiddenColumn = true,
                IsNullable = false,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "CreatedDate",
                LabelName = "Created Date",
                Alias = "CreatedDate",
                DataType = Common.DataColumnTypeEnum.DateTime,
                IsSystemColumn = true,
                IsLogColumn = true,
                IsNullable = false,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "CreatedBy",
                LabelName = "Created By User Id",
                Alias = "CreatedBy",
                IsNullable = false,
                DataType = Common.DataColumnTypeEnum.Text,
                IsSystemColumn = true,
                IsLogColumn = true,

                IsForeignKey = true,
                IsVirtualForeignKey = true,
                ForeignKeyColumnName = "Id",
                ForeignKeyTableName = "User",
                ForeignKeyTableSchemaName = ApplicationConstant.Database.Schema._Public,
                ForeignKeyDisplayColumnName = "Name",
                ForeignKeyDisplayColumnAlias = "CreatedByUserName",
                ForeignKeyDisplayColumnLabelName = "Created By User",
                ForeignKeyConstraintName = $"FK_{table.Name}_User_CreatedBy_Id",
                ForeignKeyDisplayColumnDataType = DataColumnTypeEnum.Text,
                // HideForeignKeyTableColumns = true,
                ForeignKeyTableAliasName = "CreatedByUser"


            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LastUpdatedDate",
                LabelName = "Last Updated Date",
                Alias = "LastUpdatedDate",
                DataType = Common.DataColumnTypeEnum.DateTime,
                IsSystemColumn = true,
                IsLogColumn = true,
                IsNullable = false,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LastUpdatedBy",
                LabelName = "Updated By User Id",
                Alias = "LastUpdatedBy",
                DataType = Common.DataColumnTypeEnum.Text,
                IsSystemColumn = true,
                IsLogColumn = true,
                IsNullable = false,

                IsForeignKey = true,
                IsVirtualForeignKey = true,
                ForeignKeyColumnName = "Id",
                ForeignKeyTableName = "User",
                ForeignKeyTableSchemaName = ApplicationConstant.Database.Schema._Public,
                ForeignKeyDisplayColumnName = "Name",
                ForeignKeyDisplayColumnAlias = "LastUpdatedByUserName",
                ForeignKeyDisplayColumnLabelName = "Updated By User",
                ForeignKeyConstraintName = $"FK_{table.Name}_User_LastUpdatedBy_Id",
                ForeignKeyDisplayColumnDataType = DataColumnTypeEnum.Text,
                //  HideForeignKeyTableColumns = true,
                ForeignKeyTableAliasName = "UpdatedByUser"

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "IsDeleted",
                LabelName = "Is Deleted",
                Alias = "IsDeleted",
                DataType = Common.DataColumnTypeEnum.Bool,
                IsSystemColumn = true,
                IsHiddenColumn = true,
                IsNullable = false,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "CompanyId",
                LabelName = "Company Id",
                Alias = "CompanyId",
                DataType = Common.DataColumnTypeEnum.Text,
                IsSystemColumn = true,
                IsHiddenColumn = true,
                IsNullable = false,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "LegalEntityId",
                LabelName = "LegalEntity Id",
                Alias = "LegalEntityId",
                DataType = Common.DataColumnTypeEnum.Text,
                IsSystemColumn = true,
                IsHiddenColumn = true,
                IsNullable = true,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SequenceOrder",
                LabelName = "Sequence Order",
                Alias = "SequenceOrder",
                DataType = Common.DataColumnTypeEnum.Long,
                IsSystemColumn = true,
                IsHiddenColumn = true,
                IsNullable = true,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Status",
                LabelName = "Status",
                Alias = "Status",
                DataType = Common.DataColumnTypeEnum.Integer,
                IsSystemColumn = true,
                IsHiddenColumn = true,

            }, table, dataAction));
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "VersionNo",
                LabelName = "VersionNo",
                Alias = "VersionNo",
                DataType = Common.DataColumnTypeEnum.Long,
                IsSystemColumn = true,
                IsHiddenColumn = true,

            }, table, dataAction));
            switch (table.TemplateType)
            {
                case TemplateTypeEnum.Note:
                    AddNoteMasterColumns(list, table, _repo, dataAction);
                    break;
                case TemplateTypeEnum.Task:
                    AddTaskMasterColumns(list, table, _repo, dataAction);
                    break;
                case TemplateTypeEnum.Service:
                    AddServiceMasterColumns(list, table, _repo, dataAction);
                    break;
                case TemplateTypeEnum.Custom:
                case TemplateTypeEnum.ProcessDesign:
                case TemplateTypeEnum.NoteIndexPage:
                case TemplateTypeEnum.TaskIndexPage:
                case TemplateTypeEnum.ServiceIndexPage:
                case TemplateTypeEnum.FormIndexPage:
                case TemplateTypeEnum.Page:
                case TemplateTypeEnum.Form:
                default:
                    break;
            }
            return list;
        }
        private void AddNoteMasterColumns(List<ColumnMetadataViewModel> list, TableMetadataViewModel table, IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo, DataActionEnum dataAction)
        {
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Name = "NtsNoteId",
                LabelName = "NtsNoteId",
                Alias = "NtsNoteId",
                DataType = DataColumnTypeEnum.Text,
                IsSystemColumn = true,
                IsHiddenColumn = false,
                IsReferenceColumn = false,
                IsForeignKey = true,
                ForeignKeyColumnName = "Id",
                ForeignKeyTableName = "NtsNote",
                ForeignKeyTableAliasName = "NtsNote",
                IsNullable = false,
                HideForeignKeyTableColumns = false,
                ForeignKeyTableSchemaName = ApplicationConstant.Database.Schema._Public,
                ForeignKeyDisplayColumnName = "Subject",
                ForeignKeyDisplayColumnAlias = "NoteSubject",
                ForeignKeyDisplayColumnLabelName = "Note Subject",
                ForeignKeyConstraintName = $"FK_{table.Name}_NtsNote_NtsNoteId_Id",
                ForeignKeyDisplayColumnDataType = DataColumnTypeEnum.Text

            }, table, dataAction));




        }

        private void AddTaskMasterColumns(List<ColumnMetadataViewModel> list, TableMetadataViewModel table, IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo, DataActionEnum dataAction)
        {
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Name = "NtsTaskId",
                LabelName = "NtsTaskId",
                Alias = "NtsTaskId",
                DataType = DataColumnTypeEnum.Text,
                IsSystemColumn = true,
                IsHiddenColumn = false,
                IsReferenceColumn = false,
                IsForeignKey = true,
                ForeignKeyColumnName = "Id",
                ForeignKeyTableName = "NtsTask",
                ForeignKeyTableAliasName = "NtsTask",
                IsNullable = false,
                HideForeignKeyTableColumns = false,
                ForeignKeyTableSchemaName = ApplicationConstant.Database.Schema._Public,
                ForeignKeyDisplayColumnName = "Subject",
                ForeignKeyDisplayColumnAlias = "TaskSubject",
                ForeignKeyDisplayColumnLabelName = "Task Subject",
                ForeignKeyConstraintName = $"FK_{table.Name}_NtsTask_NtsTaskId_Id",
                ForeignKeyDisplayColumnDataType = DataColumnTypeEnum.Text

            }, table, dataAction));



        }
        private void AddServiceMasterColumns(List<ColumnMetadataViewModel> list, TableMetadataViewModel table, IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo, DataActionEnum dataAction)
        {
            list.Add(AddColumn(new ColumnMetadataViewModel
            {
                Name = "NtsServiceId",
                LabelName = "NtsServiceId",
                Alias = "NtsServiceId",
                DataType = DataColumnTypeEnum.Text,
                IsSystemColumn = true,
                IsHiddenColumn = false,
                IsReferenceColumn = false,
                IsForeignKey = true,
                ForeignKeyColumnName = "Id",
                ForeignKeyTableName = "NtsService",
                ForeignKeyTableAliasName = "NtsService",
                IsNullable = false,
                HideForeignKeyTableColumns = false,
                ForeignKeyTableSchemaName = ApplicationConstant.Database.Schema._Public,
                ForeignKeyDisplayColumnName = "Subject",
                ForeignKeyDisplayColumnAlias = "ServiceSubject",
                ForeignKeyDisplayColumnLabelName = "Service Subject",
                ForeignKeyConstraintName = $"FK_{table.Name}_NtsService_NtsServiceId_Id",
                ForeignKeyDisplayColumnDataType = DataColumnTypeEnum.Text

            }, table, dataAction));



        }



        private ColumnMetadataViewModel AddColumn(ColumnMetadataViewModel col, TableMetadataViewModel table, DataActionEnum dataAction)
        {
            col.Id = Guid.NewGuid().ToString();
            col.CreatedBy = _repo.UserContext.UserId;
            col.CreatedDate = DateTime.Now;
            col.LastUpdatedBy = _repo.UserContext.UserId;
            col.LastUpdatedDate = DateTime.Now;
            col.IsDeleted = false;
            col.CompanyId = _repo.UserContext.CompanyId;
            col.TableMetadataId = table.Id;
            col.DataAction = dataAction;
            return col;
        }



        public async override Task<CommandResult<TableMetadataViewModel>> Edit(TableMetadataViewModel data, bool autoCommit = true)
        {
            var errorList = new Dictionary<string, string>();
            var dispname = await base.GetSingle(x => x.DisplayName == data.DisplayName && x.Id != data.Id);
            if (dispname != null)
            {
                errorList.Add("DisplayName", "Display name already exist.");
            }
            var tablename = await base.GetSingle(x => x.Name == data.Name && x.Id != data.Id);
            if (tablename != null)
            {
                errorList.Add("Name", "Name already exist.");
            }
            var tablealias = await base.GetSingle(x => x.Alias == data.Alias && x.Id != data.Id);
            if (tablealias != null)
            {
                errorList.Add("Alias", "Alias/Short name already exist.");
            }
            if (data.ColumnMetadatas.IsNotNull())
            {
                foreach (var col in data.ColumnMetadatas)
                {
                    if (col.ForeignKeyConstraintName.IsNotNullAndNotEmpty())
                    {
                        //var columnconstraint = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(c => c.ForeignKeyConstraintName == col.ForeignKeyConstraintName && c.TableMetadataId!=model.Id);
                        var columnconstraint = await _columnMetadataBusiness.GetSingle(c => c.ForeignKeyConstraintName == col.ForeignKeyConstraintName && c.TableMetadataId != data.Id);
                        if (columnconstraint != null)
                        {
                            errorList.Add("ForeignKeyConstraintName", "Column - " + col.ForeignKeyConstraintName + ", Foreign key constraint name already exist.");
                        }
                    }
                }
            }

            if (errorList.Count > 0)
            {
                return CommandResult<TableMetadataViewModel>.Instance(data, false, errorList);
            }
            else
            {
                var result = await base.Edit(data,autoCommit);
                if (result.IsSuccess)
                {
                    if (data.TableType != TableTypeEnum.View)
                    {
                        data.ColumnMetadatas = AddBaseColumns(data, _repo, DataActionEnum.Edit).ToList();
                    }
                    var existList = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == data.Id);
                    var curlist = data.ColumnMetadatas.Select(x => x.Name).ToList();
                    var notexist = existList.Where(x => !curlist.Contains(x.Name)).ToList();
                    foreach (var delitem in notexist)
                    {
                        await _columnMetadataBusiness.Delete(delitem.Id);
                    }
                    foreach (var col2 in data.ColumnMetadatas)
                    {
                        col2.IgnorePermission = data.IgnorePermission;
                        if (col2.IsSystemColumn)
                        {
                            var changed = existList.FirstOrDefault(x => x.Name == col2.Name && x.DataType == col2.DataType && x.IsNullable == col2.IsNullable
                                && x.LabelName == col2.LabelName && x.Alias == col2.Alias && x.IsUniqueColumn == col2.IsUniqueColumn
                                && x.IsForeignKey == col2.IsForeignKey);
                            if (changed == null)
                            {
                                col2.TableMetadataId = result.Item.Id;
                                var isInDb = existList.Any(x => x.Name == col2.Name);
                                if (isInDb == false)
                                {
                                    var colresult = await _columnMetadataBusiness.Create(col2);
                                    if (colresult.IsSuccess)
                                    {
                                        col2.Id = colresult.Item.Id;
                                    }
                                }
                                else if (col2.DataAction == DataActionEnum.Edit)
                                {
                                    var colresult = await _columnMetadataBusiness.Edit(col2);
                                    if (colresult.IsSuccess)
                                    {

                                    }
                                }
                            }
                        }
                        else
                        {
                            var exist = existList.FirstOrDefault(x => x.Name == col2.Name);
                            if (exist == null)
                            {
                                col2.TableMetadataId = result.Item.Id;
                                var colresult = await _columnMetadataBusiness.Create(col2);
                                if (colresult.IsSuccess)
                                {
                                    col2.Id = colresult.Item.Id;
                                    await ManageUdfPermission(data, col2);
                                }
                            }
                            else
                            {
                                var isChanged = exist.Name != col2.Name || exist.DataType != col2.DataType || exist.IsNullable != col2.IsNullable
                                || exist.LabelName != col2.LabelName || exist.Alias != col2.Alias || exist.IsUniqueColumn != col2.IsUniqueColumn
                                || exist.IsForeignKey != col2.IsForeignKey
                                || exist.UdfUIType != col2.UdfUIType
                                || !Helper.CompareStringArray(exist.ViewableBy, col2.ViewableBy)
                                || !Helper.CompareStringArray(exist.ViewableContext, col2.ViewableContext)
                                || !Helper.CompareStringArray(exist.EditableBy, col2.EditableBy)
                                || !Helper.CompareStringArray(exist.EditableContext, col2.EditableContext);
                                if (isChanged)
                                {
                                    col2.TableMetadataId = result.Item.Id;
                                    var isInDb = existList.Any(x => x.Name == col2.Name);
                                    if (isInDb == false)
                                    {
                                        var colresult = await _columnMetadataBusiness.Create(col2);
                                        if (colresult.IsSuccess)
                                        {
                                            col2.Id = colresult.Item.Id;
                                            await ManageUdfPermission(data, col2);
                                        }
                                    }
                                    else if (col2.DataAction == DataActionEnum.Edit)
                                    {
                                        if (exist.Id == col2.Id)
                                        {
                                            var colresult = await _columnMetadataBusiness.Edit(col2);
                                            col2.Id = colresult.Item.Id;
                                            await ManageUdfPermission(data, col2);
                                        }
                                        else
                                        {
                                            errorList.Add("ColumnName", "Column - " + col2.Name + ", columnid mismatch");
                                            return CommandResult<TableMetadataViewModel>.Instance(data, false, errorList);

                                        }
                                    }
                                }
                            }
                        }


                    }
                    await _cmsBusiness.ManageTable(data);

                }
                return CommandResult<TableMetadataViewModel>.Instance(data);
            }


        }

        private async Task<CommandResult<TableMetadataViewModel>> CreateTemplateTable(TemplateViewModel model)
        {
            var table = new TableMetadataViewModel();
            table.IsChildTable = model.IsChildTable;
            var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.Id);
            if (temp != null)
            {
                table.ColumnMetadatas = new List<ColumnMetadataViewModel>();
                table.ChildTable = new List<TableMetadataViewModel>();

                if (model.Json.IsNotNullAndNotEmpty())
                {
                    var result = JObject.Parse(model.Json);
                    temp.Json = result.ToString();
                    JArray rows = (JArray)result.SelectToken("components");
                    await ChildComp(rows, table, 1);
                }
                table.Name = temp.Name;
                table.DisplayName = temp.DisplayName ?? temp.Name;
                table.Alias = temp.Code; table.Schema = ApplicationConstant.Database.Schema.Cms;
                table.TableType = TableTypeEnum.Table;
                table.TemplateType = model.TemplateType;
                table.TemplateId = model.Id;
                var res = await Create(table);
                if (res.IsSuccess)
                {

                    temp.TableMetadataId = res.Item.Id;
                    await _repo.Edit<TemplateViewModel, Template>(temp);                   
                }
                return res;
            }
            return CommandResult<TableMetadataViewModel>.Instance(table, false, new Dictionary<string, string>() { { "Name", "Name already exist." } });
        }
        private async Task<CommandResult<TableMetadataViewModel>> EditTemplateTable(TemplateViewModel model, bool ignorePermission, string parentTemplateId)
        {
            try
            {

                if (model.Json.IsNullOrEmpty())
                {
                    model.Json = "{}";
                }
                var result = JObject.Parse(model.Json);
                JArray rows = (JArray)result.SelectToken("components");
                var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.Id);
                var table = await GetSingleById(temp.TableMetadataId);
                table.IsChildTable = model.IsChildTable;
                if (table != null)
                {
                    table.ColumnMetadatas = new List<ColumnMetadataViewModel>();
                    table.ChildTable = new List<TableMetadataViewModel>();
                    await ChildComp(rows, table, 1);
                    table.IgnorePermission = ignorePermission;
                    table.TemplateId = parentTemplateId;

                    var res = await Edit(table);
                    if (res.IsSuccess)
                    {
                        temp.Json = model.Json = result.ToString();

                        if (ignorePermission)
                        {
                            var obj = JObject.Parse(result.ToString());
                            var rows1 = (JArray)obj.SelectToken("components");
                            await NoteUdfPermission(rows1);
                            temp.Json = obj.ToString();
                        }
                        await _repo.Edit<TemplateViewModel, Template>(temp);
                        var formbusi = _serviceProvider.GetService<ITemplateBusiness>();
                        foreach (var child in table.ChildTable)
                        {
                            var exist = await formbusi.GetSingle(x => x.Code == child.Name);
                            if (exist == null)
                            {
                                var ChildformTemp = new TemplateViewModel()
                                {
                                    PortalId = model.PortalId,
                                    Name = child.Name,
                                    DisplayName = child.Name,
                                    Code = child.Name,
                                    Json = child.Json,
                                    IsChildTable = true,
                                    TemplateType = TemplateTypeEnum.Form,
                                    TemplateCategoryId = model.TemplateCategoryId
                                };
                                await formbusi.Create(ChildformTemp);
                            }
                            else
                            {
                                exist.IsChildTable = true;
                                exist.Json = child.Json;
                                await ManageTemplateTable(exist, false, model.Id);
                            }
                        }
                    }
                    return res;
                }

                return null;
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                throw;
            }
        }
        private async Task NoteUdfPermission(JArray comps)
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
                                var columnMeta = await _columnMetadataBusiness.GetSingle(x => x.Name == tempmodel.key);
                                if (columnMeta != null)
                                {
                                    var ed = jcomp.SelectToken("editableContext");
                                    if (ed != null)
                                    {
                                        jcomp.SelectToken("editableContext").Replace(JToken.FromObject(columnMeta.EditableContext));
                                    }
                                    else if (columnMeta.EditableContext != null)
                                    {
                                        var newProperty = new JProperty("editableContext", columnMeta.EditableContext);
                                        jcomp.Add(newProperty);
                                    }
                                    var vc = jcomp.SelectToken("viewableContext");
                                    if (vc != null)
                                    {
                                        jcomp.SelectToken("viewableContext").Replace(JToken.FromObject(columnMeta.ViewableContext));
                                    }
                                    else if (columnMeta.ViewableContext != null)
                                    {
                                        var newProperty1 = new JProperty("viewableContext", columnMeta.ViewableContext);
                                        jcomp.Add(newProperty1);
                                    }
                                    var vb = jcomp.SelectToken("viewableBy");
                                    if (vb != null)
                                    {
                                        jcomp.SelectToken("viewableBy").Replace(JToken.FromObject(columnMeta.ViewableBy));
                                    }
                                    else if (columnMeta.ViewableBy != null)
                                    {
                                        var newProperty2 = new JProperty("viewableBy", columnMeta.ViewableBy);
                                        jcomp.Add(newProperty2);
                                    }
                                    var eb = jcomp.SelectToken("editableBy");
                                    if (eb != null)
                                    {
                                        jcomp.SelectToken("editableBy").Replace(JToken.FromObject(columnMeta.EditableBy));
                                    }
                                    else if (columnMeta.EditableBy != null)
                                    {
                                        var newProperty3 = new JProperty("editableBy", columnMeta.EditableBy);
                                        jcomp.Add(newProperty3);
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
                                await NoteUdfPermission(rows);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await NoteUdfPermission(rows);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            await NoteUdfPermission(rows);
                    }
                }
            }
        }
        public async Task<CommandResult<TableMetadataViewModel>> ManageTemplateTable(TemplateViewModel model, bool ignorePermission, string parentTemplateId)
        {
            var table = await GetSingleById(model.TableMetadataId);
            if (table == null)
            {
                table = await GetSingle(x => x.Name == model.Name);
                if (table == null)
                {
                    return await CreateTemplateTable(model);
                }
                else
                {
                    return await EditTemplateTable(model, ignorePermission, parentTemplateId);
                }
            }
            else
            {
                return await EditTemplateTable(model, ignorePermission, parentTemplateId);
            }

            // return res;
        }

        public async Task ChildComp(JArray comps, TableMetadataViewModel table, int seqNo)
        {
            if (comps == null)
            {
                return;
            }

            foreach (JObject jcomp in comps)
            {

                var typeObj = jcomp.SelectToken("type");
                var keyObj = jcomp.SelectToken("key");
                if (typeObj.IsNotNull() && keyObj.ToString()!="Id")
                {
                    var type = typeObj.ToString();
                    var key = keyObj.ToString();
                    if (type == "textfield" || type == "textarea" || type == "number" || type == "password"
                        || type == "selectboxes" || type == "checkbox" || type == "select" || type == "radio"
                        || type == "email" || type == "url" || type == "phoneNumber" || type == "tags"
                        || type == "datetime" || type == "day" || type == "time" || type == "currency" || type== "button"
                        || type == "signature" || type == "file" || type == "hidden" || type == "datagrid" || type == "editgrid"
                        || (type == "htmlelement" && key == "chartgrid") || (type == "htmlelement" && key == "chartJs"))
                        //|| (type == "content" && key == "content"))
                    {

                        DataColumnTypeEnum ftype = DataColumnTypeEnum.Text;
                        var reserve = jcomp.SelectToken("reservedKey");
                        if (reserve == null)
                        {
                            var tempmodel = JsonConvert.DeserializeObject<FormFieldViewModel>(jcomp.ToString());
                            var uiTye = tempmodel.type.ToEnum<UdfUITypeEnum>();
                            switch (uiTye)
                            {
                                case UdfUITypeEnum.textfield:
                                case UdfUITypeEnum.datagrid:
                                case UdfUITypeEnum.editgrid:
                                case UdfUITypeEnum.textarea:
                                case UdfUITypeEnum.password:
                                case UdfUITypeEnum.selectboxes:
                                case UdfUITypeEnum.content:
                                    ftype = DataColumnTypeEnum.Text;
                                    break;
                                case UdfUITypeEnum.number:
                                    ftype = DataColumnTypeEnum.Integer;
                                    break;

                                case UdfUITypeEnum.checkbox:
                                    ftype = DataColumnTypeEnum.Bool;
                                    break;

                                case UdfUITypeEnum.radio:
                                    ftype = DataColumnTypeEnum.Bool;
                                    break;
                                case UdfUITypeEnum.select:
                                    ftype = DataColumnTypeEnum.Text;
                                    if (tempmodel.multiple)
                                    {
                                        ftype = DataColumnTypeEnum.TextArray;
                                    }
                                    break;
                                case UdfUITypeEnum.datetime:
                                    ftype = DataColumnTypeEnum.DateTime;
                                    break;
                                case UdfUITypeEnum.time:
                                    ftype = DataColumnTypeEnum.Time;
                                    break;
                                case UdfUITypeEnum.file:
                                    ftype = DataColumnTypeEnum.Text;
                                    break;
                                //case UdfUITypeEnum.datagrid:
                                //    ftype = DataColumnTypeEnum.Table;
                                //    break;
                                default:
                                    ftype = DataColumnTypeEnum.Text;
                                    break;
                            }
                            //switch (tempmodel.type)
                            //{
                            //    case "textfield":
                            //        ftype = DataColumnTypeEnum.Text;
                            //        break;
                            //    case "textarea":
                            //        ftype = DataColumnTypeEnum.Text;
                            //        break;
                            //    case "number":
                            //        ftype = DataColumnTypeEnum.Integer;
                            //        break;
                            //    case "checkbox":
                            //        ftype = DataColumnTypeEnum.Bool;
                            //        break;
                            //    case "radio":
                            //        ftype = DataColumnTypeEnum.Bool;
                            //        break;
                            //    case "select":
                            //        ftype = DataColumnTypeEnum.Text;
                            //        var multi = jcomp.SelectToken("multiple");
                            //        if (multi.IsNotNull() && (bool)multi)
                            //        {
                            //            ftype = DataColumnTypeEnum.TextArray;
                            //        }
                            //        break;
                            //    case "datetime":
                            //        ftype = DataColumnTypeEnum.DateTime;
                            //        break;
                            //    case "time":
                            //        ftype = DataColumnTypeEnum.Time;
                            //        break;
                            //    case "file":
                            //        ftype = DataColumnTypeEnum.Text;
                            //        break;
                            //    case "password":
                            //        ftype = DataColumnTypeEnum.Text;
                            //        break;
                            //    default:
                            //        ftype = DataColumnTypeEnum.Text;
                            //        break;
                            //}

                            var columnId = jcomp.SelectToken("columnMetadataId");
                            var fieldmodel = new ColumnMetadataViewModel()
                            {
                                Name = tempmodel.key,
                                LabelName = tempmodel.label,
                                Alias = tempmodel.key,
                                DataType = ftype,
                                IsNullable = true,
                                IsUniqueColumn = tempmodel.unique,
                                IsUdfColumn = true,
                                EditableBy = tempmodel.EditableBy,
                                ViewableBy = tempmodel.ViewableBy,
                                EditableContext = tempmodel.EditableContext,
                                ViewableContext = tempmodel.ViewableContext,
                                IsForeignKey = false,
                                UdfUIType = uiTye,
                                SequenceOrder = seqNo,
                                IsMultiValueColumn = tempmodel.multiple
                            };
                          
                                seqNo++;
                           
                            if (tempmodel.multiple == false && tempmodel.allTable.IsNotNullAndNotEmpty() && tempmodel.allTable != "public.enum")
                            {
                                var split = tempmodel.allTable.Split('.');
                                var tableschema = split[0];
                                var tablename = split[1];
                                fieldmodel.IsForeignKey = true;
                                //fieldmodel.ForeignKeyColumnName = tempmodel.mapId;
                                fieldmodel.ForeignKeyColumnName = "Id";
                                fieldmodel.ForeignKeyTableName = tablename;
                                fieldmodel.ForeignKeyTableAliasName = tablename;
                                fieldmodel.IsNullable = true;
                                //fieldmodel.HideForeignKeyTableColumns = false;
                                fieldmodel.ForeignKeyTableSchemaName = split[0];
                                fieldmodel.ForeignKeyDisplayColumnName = tempmodel.mapValue;
                                fieldmodel.ForeignKeyDisplayColumnAlias = tablename + "_" + tempmodel.mapValue;
                                fieldmodel.ForeignKeyDisplayColumnLabelName = tablename + " " + tempmodel.mapValue;
                                // fieldmodel.ForeignKeyConstraintName = $"FK_{table.Name}_{tablename}_{tempmodel.key}_{tempmodel.mapId}";
                                fieldmodel.ForeignKeyConstraintName = $"FK_{table.Name}_{tablename}_{tempmodel.key}_Id";
                                fieldmodel.ForeignKeyDisplayColumnDataType = DataColumnTypeEnum.Text;

                            }
                            if (tempmodel.multiple == false && tempmodel.loadTable.IsNotNullAndNotEmpty() && tempmodel.loadTable == "LOV")
                            {
                                fieldmodel.IsForeignKey = true;
                                fieldmodel.ForeignKeyColumnName = "Id";
                                fieldmodel.ForeignKeyTableName = "LOV";
                                fieldmodel.ForeignKeyTableAliasName = "LOV";
                                fieldmodel.IsNullable = true;
                                //fieldmodel.HideForeignKeyTableColumns = false;
                                fieldmodel.ForeignKeyTableSchemaName = "public";
                                fieldmodel.ForeignKeyDisplayColumnName = "Name";
                                fieldmodel.ForeignKeyDisplayColumnAlias = "LOV_" + "Name";
                                fieldmodel.ForeignKeyDisplayColumnLabelName = "LOV " + "Name";
                                // fieldmodel.ForeignKeyConstraintName = $"FK_{table.Name}_{tablename}_{tempmodel.key}_{tempmodel.mapId}";
                                fieldmodel.ForeignKeyConstraintName = $"FK_{table.Name}_LOV_{tempmodel.key}_Id";
                                fieldmodel.ForeignKeyDisplayColumnDataType = DataColumnTypeEnum.Text;
                            }

                            if (columnId == null)
                            {
                                var column = await _columnMetadataBusiness.GetSingle(x => x.TableMetadataId == table.Id && x.Name == tempmodel.key);
                                if (column == null)
                                {
                                    fieldmodel.Id = Guid.NewGuid().ToString();
                                    fieldmodel.DataAction = DataActionEnum.Create;

                                }
                                else
                                {
                                    fieldmodel.Id = column.Id;
                                    fieldmodel.DataAction = DataActionEnum.Edit;
                                }
                                var newProperty = new JProperty("columnMetadataId", fieldmodel.Id);
                                jcomp.Add(newProperty);
                            }
                            else
                            {
                                var column = await _columnMetadataBusiness.GetSingle(x => x.Id == tempmodel.columnMetadataId && x.Name == tempmodel.key);
                                if (column == null)
                                {
                                    fieldmodel.Id = Guid.NewGuid().ToString();
                                    fieldmodel.DataAction = DataActionEnum.Create;
                                    jcomp.SelectToken("columnMetadataId").Replace(JToken.FromObject(fieldmodel.Id));

                                }
                                else
                                {
                                    fieldmodel.Id = tempmodel.columnMetadataId;
                                    fieldmodel.DataAction = DataActionEnum.Edit;
                                }
                            }
                           
                                table.ColumnMetadatas.Add(fieldmodel);
                            

                        }
                    }
                    else if (type == "columns")
                    {
                        JArray cols = (JArray)jcomp.SelectToken("columns");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await ChildComp(rows, table, seqNo);
                        }
                    }
                    else if (type == "table")
                    {
                        JArray cols1 = (JArray)jcomp.SelectToken("rows");
                       
                        foreach (var col in cols1)
                        {
                            foreach (var child in col.Children())
                            { 
                                JArray rows = (JArray)child.SelectToken("components");
                                if (rows != null)
                                    await ChildComp(rows, table, seqNo);
                            }
                        }
                    }                   
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            await ChildComp(rows, table, seqNo);
                    }
                    //Child Grid
                    if ((type=="datagrid" || type == "editgrid") && table.IsChildTable)
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            await ChildComp(rows, table, seqNo);
                    }
                    else if (type=="datagrid" || type == "editgrid")
                    {
                            bool parent=false;
                            JArray rows = (JArray)jcomp.SelectToken("components");
                            foreach (JObject jc in rows)
                            {
                                var parentId = jc.SelectToken("key");
                                if(parentId.IsNotNull() && parentId.ToString()=="ParentId")
                                {
                                    parent = true;
                                }                               
                            }
                        if (!parent)
                        {
                            JObject jb = new JObject();
                            var newProperty = new JProperty("key", "ParentId");
                            jb.Add(newProperty);
                            var newProperty1 = new JProperty("type", "textfield");
                            jb.Add(newProperty1);
                            var newProperty11 = new JProperty("hidden", true);
                            jb.Add(newProperty11);
                            var newProperty12 = new JProperty("clearOnHide", false);
                            jb.Add(newProperty12);
                            rows.Add(jb);
                            //ID
                            JObject jb1 = new JObject();
                            var newProperty2 = new JProperty("key", "Id");
                            jb1.Add(newProperty2);
                            var newProperty3 = new JProperty("type", "textfield");
                            jb1.Add(newProperty3);
                            var newProperty4 = new JProperty("hidden", true);
                            jb1.Add(newProperty4);
                            var newProperty5 = new JProperty("clearOnHide", false);
                            jb1.Add(newProperty5);
                            rows.Add(jb1);
                        }

                        var childmodel = new TableMetadataViewModel()
                        {
                            Name = key,
                            IsChildTable = true,
                            Json = jcomp.ToString()
                        };
                        table.ChildTable.Add(childmodel);
                        //JArray rows = (JArray)jcomp.SelectToken("components");
                        //if (rows != null)
                        //    await ChildComp(rows, table, seqNo);
                    }
                }
            }
        }

        public async Task UpdateStaticTables(string tableName = null)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("Synergy.App.DataModel"));
            if (assembly == null)
            {
                return;
            }
            var schema = "public";
            var allClasses = assembly.GetTypes().Where(a => !a.Name.EndsWith("Log") && a.IsClass && a.Namespace != null && a.Namespace.Contains(@"Synergy.App.DataModel")).ToList();
            if (tableName.IsNotNullAndNotEmpty())
            {
                allClasses = allClasses.Where(x => x.Name == tableName).ToList();
            }

            var tables = await _repo.GetList();
            // var columns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>();

            foreach (var table in allClasses)
            {
                schema = "public";
                var customAttributes = table.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), false).FirstOrDefault();
                if (customAttributes != null)
                {
                    var t = customAttributes as System.ComponentModel.DataAnnotations.Schema.TableAttribute;
                    if (t.Schema.IsNotNullAndNotEmpty())
                    {
                        if (t.Schema != schema)
                        {
                            continue;
                        }
                        schema = t.Schema;
                    }

                }
                var tableExist = tables.FirstOrDefault(x => x.Name == table.Name);
                if (tableExist != null)
                {
                    tableExist.Schema = schema;
                    tableExist.Name = table.Name;
                    tableExist.Alias = table.Name;
                    tableExist.DisplayName = table.Name;
                    tableExist.DataAction = DataActionEnum.Edit;
                    tableExist.LastUpdatedBy = _repo.UserContext.UserId;
                    tableExist.LastUpdatedDate = DateTime.Now;
                    tableExist.CreateTable = false;
                    var props = table.GetProperties().Where(p => p.DeclaringType != typeof(DataModelBase)).ToList();

                    props = props.Where(x =>
                    x.Name != "Id" &&
                            x.Name != "CreatedDate" &&
                            x.Name != "CreatedBy" &&
                            x.Name != "LastUpdatedDate" &&
                            x.Name != "LastUpdatedBy" &&
                            x.Name != "IsDeleted" &&
                            x.Name != "CompanyId" &&
                            x.Name != "SequenceOrder" &&
                            x.Name != "DataAction" &&
                            x.Name != "Status" &&
                            x.Name != "VersionNo"
                            && (x.PropertyType.BaseType == null || x.PropertyType.BaseType.Name != "DataModelBase")

                   ).ToList();
                    tableExist.ColumnMetadatas = new List<ColumnMetadataViewModel>();
                    foreach (var item in props)
                    {

                        tableExist.ColumnMetadatas.Add(new ColumnMetadataViewModel
                        {
                            Name = item.Name,
                            Alias = item.Name,
                            IsNullable = true,
                            DataType = item.PropertyType.ToDatabaseColumn(),
                            TableMetadataId = tableExist.Id,
                            DataAction = DataActionEnum.Create,
                            LabelName = item.Name.ToSentenceCase(),
                            Status = StatusEnum.Active,

                        });
                    }
                    await this.Edit(tableExist);

                }
                else
                {
                    var id = Guid.NewGuid().ToString();
                    tableExist = new TableMetadataViewModel { Id = id };
                    tableExist.Schema = schema;
                    tableExist.Name = table.Name;
                    tableExist.Alias = table.Name;
                    tableExist.DisplayName = table.Name;
                    tableExist.DataAction = DataActionEnum.Create;
                    tableExist.LastUpdatedBy = _repo.UserContext.UserId;
                    tableExist.LastUpdatedDate = DateTime.Now;
                    tableExist.CreatedBy = _repo.UserContext.UserId;
                    tableExist.CreatedDate = DateTime.Now;
                    tableExist.CreateTable = false;
                    tableExist.Status = StatusEnum.Active;
                    tableExist.CompanyId = _repo.UserContext.CompanyId;
                    var props = table.GetProperties();
                    tableExist.ColumnMetadatas = new List<ColumnMetadataViewModel>();
                    foreach (var item in props.Where(x =>
                    x.Name != "Id" &&
                            x.Name != "CreatedDate" &&
                            x.Name != "CreatedBy" &&
                            x.Name != "LastUpdatedDate" &&
                            x.Name != "LastUpdatedBy" &&
                            x.Name != "IsDeleted" &&
                            x.Name != "CompanyId" &&
                            x.Name != "SequenceOrder" &&
                            x.Name != "Status" &&
                            x.Name != "VersionNo"
                             && (x.PropertyType.BaseType == null || x.PropertyType.BaseType.Name != "DataModelBase")

                    ))
                    {

                        tableExist.ColumnMetadatas.Add(new ColumnMetadataViewModel
                        {
                            Name = item.Name,
                            Alias = item.Name,
                            IsNullable = true,
                            DataType = item.PropertyType.ToDatabaseColumn(),
                            TableMetadataId = id,
                            DataAction = DataActionEnum.Create,
                            LabelName = item.Name.ToSentenceCase(),
                            Status = StatusEnum.Active,

                        });

                    }

                    await this.Create(tableExist);
                }
            }
        }

        public async Task ManageUdfPermission(TableMetadataViewModel tableMetaData, ColumnMetadataViewModel col2)
        {
            var template = await _repo.GetSingleById<TemplateViewModel, Template>(tableMetaData.TemplateId);
            if (template != null)
            {
                if (template.TemplateType == TemplateTypeEnum.Task)
                {
                    var taskTemplate = await _repo.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == template.Id);
                    if (template.TemplateType == TemplateTypeEnum.Task && taskTemplate.TaskTemplateType != TaskTypeEnum.StepTask)
                    {
                        var coludf = await _repo.GetSingle<UdfPermissionViewModel, UdfPermission>(x => x.ColumnMetadataId == col2.Id && x.TemplateId == template.Id);
                        if (coludf != null)
                        {
                            coludf.EditableBy = col2.EditableBy;
                            coludf.ViewableBy = col2.ViewableBy;
                            coludf.EditableContext = col2.EditableContext;
                            coludf.ViewableContext = col2.ViewableContext;
                            var udfresult = await _repo.Edit<UdfPermissionViewModel, UdfPermission>(coludf);
                        }
                        else
                        {
                            UdfPermissionViewModel model = new UdfPermissionViewModel();

                            model.TemplateId = template.Id;
                            model.EditableBy = col2.EditableBy;
                            model.ViewableBy = col2.ViewableBy;
                            model.EditableContext = col2.EditableContext;
                            model.ViewableContext = col2.ViewableContext;
                            model.ColumnMetadataId = col2.Id;
                            var udfresult = await _repo.Create<UdfPermissionViewModel, UdfPermission>(model);

                        }
                    }
                }
                else if (template.TemplateType == TemplateTypeEnum.Service)
                {
                    var coludf = await _repo.GetSingle<UdfPermissionViewModel, UdfPermission>(x => x.ColumnMetadataId == col2.Id && x.TemplateId == template.Id);
                    if (coludf != null)
                    {
                        coludf.EditableBy = col2.EditableBy;
                        coludf.ViewableBy = col2.ViewableBy;
                        coludf.EditableContext = col2.EditableContext;
                        coludf.ViewableContext = col2.ViewableContext;
                        var udfresult = await _repo.Edit<UdfPermissionViewModel, UdfPermission>(coludf);
                    }
                    else
                    {
                        UdfPermissionViewModel model = new UdfPermissionViewModel();

                        model.TemplateId = template.Id;
                        model.EditableBy = col2.EditableBy;
                        model.ViewableBy = col2.ViewableBy;
                        model.EditableContext = col2.EditableContext;
                        model.ViewableContext = col2.ViewableContext;
                        model.ColumnMetadataId = col2.Id;
                        var udfresult = await _repo.Create<UdfPermissionViewModel, UdfPermission>(model);

                    }
                }
            }



        }

        public async Task<List<ColumnMetadataViewModel>> GetTableData(string tableMetadataId, string recordId)
        {
            var list = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetadataId);
            var table = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableMetadataId);
            if (table != null)
            {
                var name = table.Name;
                var schema = table.Schema;
                var dt = await _cmsQueryBusiness.GetTableData(tableMetadataId, recordId, name, schema);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    foreach (var item in list)
                    {
                        item.Value = row[item.Name];
                    }
                }

            }
            return list;
        }
        public async Task<DataRow> GetTableDataByColumn(string templateCode, string templateId, string udfName, string udfValue)
        {
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == templateCode || x.Id == templateId);
            if (template != null)
            {
                var tableId = template.UdfTableMetadataId.IsNullOrEmpty() ? template.TableMetadataId : template.UdfTableMetadataId;
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableId);
                if (tableMetadata != null)
                {
                    var name = tableMetadata.Name;
                    var schema = tableMetadata.Schema;
                    var dt = await _cmsQueryBusiness.GetTableDataByColumnData(schema, name, udfName, udfValue);
                    if (dt.Rows.Count > 0)
                    {

                        return dt.Rows[0];

                    }
                }
            }
            return null;
        }

        public async Task<DataRow> GetTableDataByHeaderId(string templateId, string headerId)
        {
            var template = await _repo.GetSingleById<TemplateViewModel, Template>(templateId);
            if (template != null)
            {
                var fieldName = "";
                switch (template.TemplateType)
                {
                    case TemplateTypeEnum.Form:
                        fieldName = "FormId";
                        break;
                    case TemplateTypeEnum.Note:
                    case TemplateTypeEnum.Task:
                    case TemplateTypeEnum.Service:
                        fieldName = "NtsNoteId";
                        break;
                    default:
                        break;
                }
                var tableId = template.UdfTableMetadataId.IsNullOrEmpty() ? template.TableMetadataId : template.UdfTableMetadataId;
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableId);
                if (tableMetadata != null)
                {
                    var schema = tableMetadata.Schema;
                    var name = tableMetadata.Name;
                   
                    var dt = await _cmsQueryBusiness.GetTableDataByHeaderIdData(schema, name, fieldName, headerId);
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0];
                    }
                }
            }
            return null;
        }

        public async Task<DataRow> DeleteTableDataByHeaderId(string templateCode, string templateId, string headerId)
        {
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == templateCode || x.Id == templateId);
            if (template != null)
            {
                var fieldName = "";
                switch (template.TemplateType)
                {
                    case TemplateTypeEnum.Form:
                        fieldName = "FormId";
                        break;
                    case TemplateTypeEnum.Note:
                    case TemplateTypeEnum.Task:
                    case TemplateTypeEnum.Service:
                        fieldName = "NtsNoteId";
                        break;
                    default:
                        break;
                }
                var tableId = template.UdfTableMetadataId.IsNullOrEmpty() ? template.TableMetadataId : template.UdfTableMetadataId;
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableId);
                if (tableMetadata != null)
                {
                    var schema = tableMetadata.Schema;
                    var name = tableMetadata.Name;

                    var dt = await _cmsQueryBusiness.DeleteTableDataByHeaderIdData(schema,name,fieldName,headerId);
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0];
                    }
                }
            }
            return null;
        }

        public async Task EditTableDataByHeaderId(string templateCode, string templateId, string headerId, Dictionary<string, object> columnsToUpdate)
        {
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == templateCode || x.Id == templateId);
            if (template != null)
            {
                var fieldName = "";
                switch (template.TemplateType)
                {
                    case TemplateTypeEnum.Form:
                        fieldName = "FormId";
                        break;
                    case TemplateTypeEnum.Note:
                    case TemplateTypeEnum.Task:
                    case TemplateTypeEnum.Service:
                        fieldName = "NtsNoteId";
                        break;

                    default:
                        break;
                }
                var tableId = template.UdfTableMetadataId.IsNullOrEmpty() ? template.TableMetadataId : template.UdfTableMetadataId;
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableId);
                if (tableMetadata != null)
                {
                    var columnKeys = new List<string>();
                    foreach (var col in columnsToUpdate)
                    {
                        columnKeys.Add(@$"""{ col.Key}"" = {BusinessHelper.ConvertToDbValue(col.Value, false, DataColumnTypeEnum.Text)}");
                    }
                    columnKeys.Add(@$"""LastUpdatedBy"" = '{_repo.UserContext.UserId}'");
                    columnKeys.Add(@$"""LastUpdatedDate"" = '{DateTime.Now.ToDatabaseDateFormat()}'");
                    var schema = tableMetadata.Schema;
                    var name = tableMetadata.Name;
                    var selectQuery = @$"update {schema}.""{name}"" set {string.Join(",", columnKeys)} where ""{fieldName}""='{headerId}' ";
                    await _cmsQueryBusiness.EditTableDataByHeaderIdData(schema, name, columnKeys, fieldName, headerId);
                }
            }
            // return ;
        }
        //public async Task<List<ColumnMetadataViewModel>> GetDataList(string ntsId, string templateId)
        //{

        //    var query = $@"select c.*,tm.""Name"" as ""TableName"",tm.""Schema"" as ""TableSchemaName""
        //    ,t.
        //    from public.""Template"" t join public.""TableMetadata"" tm
        //    on coalesce(t.""TableMetadataId"",t.""UdfTableMetadataId)=tm.""Id"" and tm.""IsDeleted""=false
        //    join public.""ColumnMetadata"" c on  c.""TableMetadataId""=tm.""Id""  and c.""IsDeleted""=false
        //    where t.""IsDeleted""=false";
        //    var columns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
        //    if (columns !=null &&  columns.Any())
        //    {
        //        var item = columns.FirstOrDefault();
        //       var selectQuery = @$"select * from {item.TableSchemaName}.{item.TableName} where ""{tableMetaData.Name}"".""Id""='{recordId}' limit 1";
        //        dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
        //    }
        //    WHERE TABLE_NAME = '{tableName}' and TABLE_SCHEMA = '{schema}'";
        //    if (template != null)
        //    {
        //        var fieldName = "";
        //        switch (template.TemplateType)
        //        {
        //            case TemplateTypeEnum.Form:
        //                fieldName = "FormId";
        //                break;
        //            case TemplateTypeEnum.Note:
        //            case TemplateTypeEnum.Task:
        //            case TemplateTypeEnum.Service:
        //                fieldName = "NtsNoteId";
        //                break;
        //            default:
        //                break;
        //        }
        //        var tableId = template.UdfTableMetadataId.IsNullOrEmpty() ? template.TableMetadataId : template.UdfTableMetadataId;
        //        var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableId);
        //        if (tableMetadata != null)
        //        {
        //            var columnKeys = new List<string>();
        //            foreach (var col in columnsToUpdate)
        //            {
        //                columnKeys.Add(@$"""{ col.Key}"" = {BusinessHelper.ConvertToDbValue(col.Value, false, DataColumnTypeEnum.Text)}");
        //            }
        //            columnKeys.Add(@$"""LastUpdatedBy"" = '{_repo.UserContext.UserId}'");
        //            columnKeys.Add(@$"""LastUpdatedDate"" = '{DateTime.Now.ToDatabaseDateFormat()}'");
        //            var selectQuery = @$"update {tableMetadata.Schema}.""{tableMetadata.Name}"" set {string.Join(",", columnKeys)} where ""{fieldName}""='{headerId}' ";
        //            await _queryRepo.ExecuteCommand(selectQuery, null);

        //        }
        //    }
        //    var query = $@"select * from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' and TABLE_SCHEMA='{schema}'";
        //    var columns = await _queryRepo.ExecuteQueryDataTable(query, null);
        //    var tableColumnList = new List<ColumnMetadataViewModel>();
        //    foreach (DataRow row in columns.Rows)
        //    {
        //        var data = new ColumnMetadataViewModel();
        //        data.Name = row["column_name"].ToString();
        //        data.Alias = row["column_name"].ToString();
        //        data.LabelName = row["column_name"].ToString();
        //        if (row["data_type"].ToString() == "boolean")
        //        {
        //            data.DataType = DataColumnTypeEnum.Bool;
        //        }
        //        else if (row["data_type"].ToString() == "timestamp without time zone")
        //        {
        //            data.DataType = DataColumnTypeEnum.DateTime;
        //        }
        //        else if (row["data_type"].ToString() == "bigint")
        //        {
        //            data.DataType = DataColumnTypeEnum.Long;
        //        }
        //        else if (row["data_type"].ToString() == "integer")
        //        {
        //            data.DataType = DataColumnTypeEnum.Integer;
        //        }
        //        else if (row["data_type"].ToString() == "double precision")
        //        {
        //            data.DataType = DataColumnTypeEnum.Double;
        //        }
        //        else if (row["data_type"].ToString() == "text[]")
        //        {
        //            data.DataType = DataColumnTypeEnum.TextArray;
        //        }
        //        else
        //        {
        //            data.DataType = (DataColumnTypeEnum)Enum.Parse(typeof(DataColumnTypeEnum), row["data_type"].ToString(), true);
        //        }

        //        data.IsNullable = Convert.ToString(row["is_nullable"]) == "YES" ? true : false;
        //        tableColumnList.Add(data);
        //    }
        //    return tableColumnList;
        //}

        public async Task<List<ColumnMetadataViewModel>> GetViewColumnByTableName(string schema, string tableName)
        {

            var columns = await _cmsQueryBusiness.GetViewColumnByTableNameData(schema, tableName);
            var tableColumnList = new List<ColumnMetadataViewModel>();
            foreach (DataRow row in columns.Rows)
            {
                var data = new ColumnMetadataViewModel();
                data.Name = row["column_name"].ToString();
                data.Alias = row["column_name"].ToString();
                data.LabelName = row["column_name"].ToString();
                if (row["data_type"].ToString() == "boolean")
                {
                    data.DataType = DataColumnTypeEnum.Bool;
                }
                else if (row["data_type"].ToString() == "timestamp without time zone")
                {
                    data.DataType = DataColumnTypeEnum.DateTime;
                }
                else if (row["data_type"].ToString() == "bigint")
                {
                    data.DataType = DataColumnTypeEnum.Long;
                }
                else if (row["data_type"].ToString() == "integer")
                {
                    data.DataType = DataColumnTypeEnum.Integer;
                }
                else if (row["data_type"].ToString() == "double precision")
                {
                    data.DataType = DataColumnTypeEnum.Double;
                }
                else if (row["data_type"].ToString() == "text[]")
                {
                    data.DataType = DataColumnTypeEnum.TextArray;
                }
                else
                {
                    data.DataType = (DataColumnTypeEnum)Enum.Parse(typeof(DataColumnTypeEnum), row["data_type"].ToString(), true);
                }

                data.IsNullable = Convert.ToString(row["is_nullable"]) == "YES" ? true : false;
                tableColumnList.Add(data);
            }
            return tableColumnList;
        }
        public async Task<ColumnMetadataViewModel> GetColumnByTableName(string schema, string tableName, string columnName)
        {
            tableName = tableName.Replace("\"", "");// Regex.Replace(tableName, @"[^0-9a-zA-Z]+", "");
            columnName = columnName.Replace("\"", ""); // Regex.Replace(columnName, @"[^0-9a-zA-Z]+", "");
            var model = await GetSingle(x => x.Schema == schema.Trim() && x.Name == tableName.Trim());
            if (model != null)
            {
                var column = await _columnMetadataBusiness.GetSingle(x => x.TableMetadataId == model.Id && x.Name == columnName);
                return column;
            }
            return new ColumnMetadataViewModel();
        }
        public async Task<MemoryStream> GetExcelForNoteTemplateUdf(string templateId)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>();
            reportList.Add("Sample Template");
            var template = await GetSingleById<TemplateViewModel, Template>(templateId);
            var tableMetadata = await GetSingleById(template.TableMetadataId);
            var columns = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == tableMetadata.Id);

            using (var sl = new SLDocument())
            {
                sl.AddWorksheet("SampleTemplate");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);
                if (columns.IsNotNull())
                { 
                   
                    sl.MergeWorksheetCells(string.Concat("A", 1), string.Concat("A", 1));
                    sl.SetCellValue(string.Concat("A", 1), "Note Subject");
                    sl.SetCellStyle(string.Concat("A", 1), string.Concat("A", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("B", 1), string.Concat("B", 1));
                    sl.SetCellValue(string.Concat("B", 1), "Note Description");
                    sl.SetCellStyle(string.Concat("B", 1), string.Concat("B", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("C", 1), string.Concat("C", 1));
                    sl.SetCellValue(string.Concat("C", 1), "Start Date (yyyy-MM-dd)");
                    sl.SetCellStyle(string.Concat("C", 1), string.Concat("C", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("D", 1), string.Concat("D", 1));
                    sl.SetCellValue(string.Concat("D", 1), "Due Date (yyyy-MM-dd)");
                    sl.SetCellStyle(string.Concat("D", 1), string.Concat("D", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("E", 1), string.Concat("E", 1));
                    sl.SetCellValue(string.Concat("E", 1), "Priority");
                    sl.SetCellStyle(string.Concat("E", 1), string.Concat("E", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("F", 1), string.Concat("F", 1));
                    sl.SetCellValue(string.Concat("F", 1), "Owned By");
                    sl.SetCellStyle(string.Concat("F", 1), string.Concat("F", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("G", 1), string.Concat("G", 1));
                    sl.SetCellValue(string.Concat("G", 1), "Requested By");
                    sl.SetCellStyle(string.Concat("G", 1), string.Concat("G", 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    char first = 'H';
                    foreach (var col in columns.Where(x => x.IsUdfColumn == true))
                    {
                        first = (char)((int)first);
                        sl.SetColumnWidth(first, 20);
                        sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                        col.Name = col.DataType.IsNotNull() && col.DataType == DataColumnTypeEnum.DateTime ? string.Concat(col.Name, "(yyyy-MM-dd)") : col.Name;
                        sl.SetCellValue(string.Concat(first, 1), col.Name);
                        sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                        first = (char)((int)first + 1);
                    }
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "SequenceOrder");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    first = (char)((int)first + 1);
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "Status");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }
            ms.Position = 0;
            return ms;
        }

        public async Task<MemoryStream> GetExcelForNoteTemplateData(string templateId)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>();
            reportList.Add("Template Data");
            var template = await GetSingleById<TemplateViewModel, Template>(templateId);
            var tableMetadata = await GetSingleById(template.TableMetadataId);
            var columns = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == tableMetadata.Id);
            var tabledata = await _cmsBusiness.GetDataListByTemplate(template.Code, template.Id);
            using (var sl = new SLDocument())
            {
                sl.AddWorksheet("TemplateData");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);

                if (columns.IsNotNull())
                {
                    sl.MergeWorksheetCells(string.Concat("A", 1), string.Concat("A", 1));
                    sl.SetCellValue(string.Concat("A", 1), "Note Subject");
                    sl.SetCellStyle(string.Concat("A", 1), string.Concat("A", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("B", 1), string.Concat("B", 1));
                    sl.SetCellValue(string.Concat("B", 1), "Note Description");
                    sl.SetCellStyle(string.Concat("B", 1), string.Concat("B", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("C", 1), string.Concat("C", 1));
                    sl.SetCellValue(string.Concat("C", 1), "Start Date (yyyy-MM-dd)");
                    sl.SetCellStyle(string.Concat("C", 1), string.Concat("C", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("D", 1), string.Concat("D", 1));
                    sl.SetCellValue(string.Concat("D", 1), "Expiry Date (yyyy-MM-dd)");
                    sl.SetCellStyle(string.Concat("D", 1), string.Concat("D", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("E", 1), string.Concat("E", 1));
                    sl.SetCellValue(string.Concat("E", 1), "Priority");
                    sl.SetCellStyle(string.Concat("E", 1), string.Concat("E", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("F", 1), string.Concat("F", 1));
                    sl.SetCellValue(string.Concat("F", 1), "Owned By");
                    sl.SetCellStyle(string.Concat("F", 1), string.Concat("F", 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("G", 1), string.Concat("G", 1));
                    sl.SetCellValue(string.Concat("G", 1), "Requested By");
                    sl.SetCellStyle(string.Concat("G", 1), string.Concat("G", 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    int row0 = 2;
                    foreach (DataRow data in tabledata.Rows)
                    {
                        var note = await _cmsBusiness.GetSingleById<NoteViewModel, NtsNote>(data["NtsNoteId"].ToString());
                        if (note.IsNotNull()) 
                        {
                            sl.MergeWorksheetCells(string.Concat("A", row0), string.Concat("A", row0));
                            sl.SetCellValue(string.Concat("A", row0), note.NoteSubject);
                            sl.SetCellStyle(string.Concat("A", row0), string.Concat("A", row0), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("B", row0), string.Concat("B", row0));
                            sl.SetCellValue(string.Concat("B", row0), note.NoteDescription);
                            sl.SetCellStyle(string.Concat("B", row0), string.Concat("B", row0), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("C", row0), string.Concat("C", row0));
                            sl.SetCellValue(string.Concat("C", row0), note.StartDate.ToYYYY_MM_DD_DateFormat());
                            sl.SetCellStyle(string.Concat("C", row0), string.Concat("C", row0), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("D", row0), string.Concat("D", row0));
                            sl.SetCellValue(string.Concat("D", row0), note.ExpiryDate.ToYYYY_MM_DD_DateFormat());
                            sl.SetCellStyle(string.Concat("D", row0), string.Concat("D", row0), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                            var priority = await _cmsBusiness.GetSingleById<LOVViewModel, LOV>(note.NotePriorityId);
                            sl.MergeWorksheetCells(string.Concat("E", row0), string.Concat("E", row0));
                            sl.SetCellValue(string.Concat("E", row0), priority.Name);
                            sl.SetCellStyle(string.Concat("E", row0), string.Concat("E", row0), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                            var owner = await _cmsBusiness.GetSingleById<UserViewModel, User>(note.OwnerUserId);
                            sl.MergeWorksheetCells(string.Concat("F", row0), string.Concat("F", row0));
                            sl.SetCellValue(string.Concat("F", row0), owner.Name);
                            sl.SetCellStyle(string.Concat("F", row0), string.Concat("F", row0), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                            var requestedBy = await _cmsBusiness.GetSingleById<UserViewModel, User>(note.RequestedByUserId);
                            sl.MergeWorksheetCells(string.Concat("G", row0), string.Concat("G", row0));
                            sl.SetCellValue(string.Concat("G", row0), requestedBy!=null?requestedBy.Name:"");
                            sl.SetCellStyle(string.Concat("G", row0), string.Concat("G", row0), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                            row0++;
                        }
                       
                    }
                    char first = 'H';
                    foreach (var col in columns.Where(x => x.IsUdfColumn == true))
                    {
                        first = (char)((int)first);
                        sl.SetColumnWidth(first, 20);
                        sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                        if (col.DataType.IsNotNull() && col.DataType == DataColumnTypeEnum.DateTime)
                        {
                            sl.SetCellValue(string.Concat(first, 1), string.Concat(col.Name, "(yyyy-MM-dd)"));
                        }
                        else
                        {
                            sl.SetCellValue(string.Concat(first, 1), col.Name);
                        }
                        sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                        int row = 2;
                        foreach (DataRow data in tabledata.Rows)
                        {
                            var colName = col.Name;
                            var val = col.DataType.IsNotNull() && col.DataType == DataColumnTypeEnum.DateTime && data[colName].ToString() !=""? Convert.ToDateTime(data[colName]).ToDD_YYYY_MM_DD() : data[colName].ToString();
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), val);
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                            row++;
                        }
                        first = (char)((int)first + 1);
                    }
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "SequenceOrder");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    int row1 = 2;
                    foreach (DataRow data in tabledata.Rows)
                    {

                        sl.MergeWorksheetCells(string.Concat(first, row1), string.Concat(first, row1));
                        sl.SetCellValue(string.Concat(first, row1), data["SequenceOrder"].ToString());
                        sl.SetCellStyle(string.Concat(first, row1), string.Concat(first, row1), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                        row1++;
                    }



                    first = (char)((int)first + 1);
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "Status");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    int row2 = 2;
                    foreach (DataRow data in tabledata.Rows)
                    {

                        sl.MergeWorksheetCells(string.Concat(first, row2), string.Concat(first, row2));
                        sl.SetCellValue(string.Concat(first, row2), data["SequenceOrder"].ToString());
                        sl.SetCellStyle(string.Concat(first, row2), string.Concat(first, row2), ExcelHelper.GetTextCaseStudyStyle(sl));
                        row2++;
                    }

                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }
            ms.Position = 0;
            return ms;
        }
        public async Task<MemoryStream> GetExcelForTemplateUdf(string templateId)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>();
           

            reportList.Add("Sample Template");
            var template = await GetSingleById<TemplateViewModel,Template>(templateId);
            var tableMetadata = await GetSingleById(template.TableMetadataId);
            var columns = await _columnMetadataBusiness.GetList(x=>x.TableMetadataId== tableMetadata.Id);

            using (var sl = new SLDocument())
            {
                
                  

                    sl.AddWorksheet("SampleTemplate");

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                
                    if (columns.IsNotNull())
                    {
                        //Int32 maxNo = columns.Count() > 0 ? columns.Count() : 0;
                        char first = 'A';

                    //for (int i = 0; i < maxNo; i++)
                    //{
                    foreach (var col in columns.Where(x=>x.IsUdfColumn==true))
                    {
                        first = (char)((int)first);
                            sl.SetColumnWidth(first, 20);
                            sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                        col.Name = col.DataType.IsNotNull() && col.DataType == DataColumnTypeEnum.DateTime ? string.Concat(col.Name, "(yyyy-MM-dd)") : col.Name;
                        sl.SetCellValue(string.Concat(first, 1), col.Name);
                            sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));

                            first = (char)((int)first + 1);
                        }
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "SequenceOrder");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    first = (char)((int)first + 1);
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "Status");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                }
                
                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }
            ms.Position = 0;
            return ms;
        }

        public async Task<MemoryStream> GetExcelForTemplateData(string templateId)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>();


            reportList.Add("Template Data");
            var template = await GetSingleById<TemplateViewModel, Template>(templateId);
            var tableMetadata = await GetSingleById(template.TableMetadataId);
            var columns = await _columnMetadataBusiness.GetList(x => x.TableMetadataId == tableMetadata.Id);
            var tabledata = await _cmsBusiness.GetDataListByTemplate(template.Code,template.Id);
            using (var sl = new SLDocument())
            {



                sl.AddWorksheet("TemplateData");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);

                if (columns.IsNotNull())
                {
                   // Int32 maxNo = columns.Count() > 0 ? columns.Count() : 0;
                    char first = 'A';
                    foreach (var col in columns.Where(x => x.IsUdfColumn == true))
                    {
                        first = (char)((int)first);
                        sl.SetColumnWidth(first, 20);
                        sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                        if (col.DataType.IsNotNull() && col.DataType == DataColumnTypeEnum.DateTime)
                        {
                            sl.SetCellValue(string.Concat(first, 1), string.Concat(col.Name, "(yyyy-MM-dd)"));
                        }
                        else 
                        {
                            sl.SetCellValue(string.Concat(first, 1), col.Name);
                        }
                       
                        
                        sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                        int row = 2;
                        foreach (DataRow data in tabledata.Rows)
                        {
                            var colName = col.Name;
                            var val=col.DataType.IsNotNull() && col.DataType == DataColumnTypeEnum.DateTime ? Convert.ToDateTime(data[colName]).ToDD_YYYY_MM_DD() : data[colName].ToString();
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), val);
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                            row++;
                        }
                        first = (char)((int)first + 1);
                    }
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "SequenceOrder");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    int row1 = 2;
                    foreach (DataRow data in tabledata.Rows)
                    {
                       
                        sl.MergeWorksheetCells(string.Concat(first, row1), string.Concat(first, row1));
                        sl.SetCellValue(string.Concat(first, row1), data["SequenceOrder"].ToString());
                        sl.SetCellStyle(string.Concat(first, row1), string.Concat(first, row1), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                        row1++;
                    }



                    first = (char)((int)first + 1);
                    first = (char)((int)first);
                    sl.SetColumnWidth(first, 20);
                    sl.MergeWorksheetCells(string.Concat(first, 1), string.Concat(first, 1));
                    sl.SetCellValue(string.Concat(first, 1), "Status");
                    sl.SetCellStyle(string.Concat(first, 1), string.Concat(first, 1), ExcelHelper.GetShiftEntryDateStyle(sl));
                    int row2 = 2;
                    foreach (DataRow data in tabledata.Rows)
                    {

                        sl.MergeWorksheetCells(string.Concat(first, row2), string.Concat(first, row2));
                        sl.SetCellValue(string.Concat(first, row2), data["SequenceOrder"].ToString());
                        sl.SetCellStyle(string.Concat(first, row2), string.Concat(first, row2), ExcelHelper.GetTextCaseStudyStyle(sl));
                        row2++;
                    }

                }
               
                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }
            ms.Position = 0;
            return ms;
        }

    }
}
