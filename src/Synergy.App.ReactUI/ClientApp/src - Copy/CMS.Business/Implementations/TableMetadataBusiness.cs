using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class TableMetadataBusiness : BusinessBase<TableMetadataViewModel, TableMetadata>, ITableMetadataBusiness
    {
        IColumnMetadataBusiness _columnMetadataBusiness;
        ICmsBusiness _cmsBusiness;
        IRepositoryQueryBase<TableMetadataViewModel> _queryRepo;
        public TableMetadataBusiness(IRepositoryBase<TableMetadataViewModel, TableMetadata> repo, IMapper autoMapper
            , IColumnMetadataBusiness columnMetadataBusiness
            , ICmsBusiness cmsBusiness
            , IRepositoryQueryBase<TableMetadataViewModel> queryRepo) : base(repo, autoMapper)
        {
            _columnMetadataBusiness = columnMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<TableMetadataViewModel>> Create(TableMetadataViewModel model)
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
                var result = await base.Create(model);
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



        public async override Task<CommandResult<TableMetadataViewModel>> Edit(TableMetadataViewModel data)
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
                var result = await base.Edit(data);
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
            var temp = await _repo.GetSingleById<TemplateViewModel, Template>(model.Id);
            if (temp != null)
            {
                table.ColumnMetadatas = new List<ColumnMetadataViewModel>();
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
                if (table != null)
                {
                    table.ColumnMetadatas = new List<ColumnMetadataViewModel>();
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
                        JArray cols = (JArray)jcomp.SelectToken("rows");
                        foreach (var col in cols)
                        {
                            JArray rows = (JArray)col.SelectToken("components");
                            if (rows != null)
                                await ChildComp(rows, table, seqNo);
                        }
                    }
                    else
                    {
                        JArray rows = (JArray)jcomp.SelectToken("components");
                        if (rows != null)
                            await ChildComp(rows, table, seqNo);
                    }
                }
            }
        }

        public async Task UpdateStaticTables(string tableName = null)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("CMS.Data.Model"));
            if (assembly == null)
            {
                return;
            }
            var schema = "public";
            var allClasses = assembly.GetTypes().Where(a => !a.Name.EndsWith("Log") && a.IsClass && a.Namespace != null && a.Namespace.Contains(@"CMS.Data.Model")).ToList();
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
                var query = $@"select * from {table.Schema}.""{table.Name}"" where ""Id""='{recordId}' limit 1";
                var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
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
                    var selectQuery = @$"select * from {tableMetadata.Schema}.""{tableMetadata.Name}"" where  ""IsDeleted""=false and ""{udfName}""='{udfValue}' limit 1";
                    var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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
                    var selectQuery = @$"select * from {tableMetadata.Schema}.""{tableMetadata.Name}"" where  ""IsDeleted""=false and ""{fieldName}""='{headerId}' limit 1";
                    var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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
                    var selectQuery = @$"update {tableMetadata.Schema}.""{tableMetadata.Name}"" set  ""IsDeleted""=true where ""{fieldName}""='{headerId}' ";
                    var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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
                    var selectQuery = @$"update {tableMetadata.Schema}.""{tableMetadata.Name}"" set {string.Join(",", columnKeys)} where ""{fieldName}""='{headerId}' ";
                    await _queryRepo.ExecuteCommand(selectQuery, null);

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

            var query = $@"select * from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' and TABLE_SCHEMA='{schema}'";
            var columns = await _queryRepo.ExecuteQueryDataTable(query, null);
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
    }
}
