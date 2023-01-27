using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
////using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class CmsBusiness : BusinessBase<TemplateViewModel, Template>, ICmsBusiness
    {
        private readonly IRepositoryQueryBase<TemplateViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<TaskIndexPageTemplateViewModel> _querytaskRepo;
        private readonly IRepositoryQueryBase<ServiceIndexPageTemplateViewModel> _queryServiceRepo;
        private readonly IRepositoryQueryBase<NoteIndexPageTemplateViewModel> _queryNoteRepo;
        private readonly IRepositoryQueryBase<NtsTaskSharedViewModel> _queryRepoTaskShared;
        private readonly IRepositoryQueryBase<NtsServiceSharedViewModel> _queryRepoServiceShared;
        private readonly IRepositoryQueryBase<NtsNoteSharedViewModel> _queryRepoNoteShared;
        private readonly IRepositoryQueryBase<EmailTaskViewModel> _queryRepoEmailTask;
        private readonly IPageBusiness _pageBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserContext _userContext;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        private readonly IServiceProvider _serviceProvider;
        public CmsBusiness(IRepositoryBase<TemplateViewModel, Template> repo
            , IMapper autoMapper
            , IRepositoryQueryBase<TemplateViewModel> queryRepo
            , IRepositoryQueryBase<NtsTaskSharedViewModel> queryRepoTaskShared
            , IRepositoryQueryBase<NtsServiceSharedViewModel> queryRepoServiceShared
            , IRepositoryQueryBase<NtsNoteSharedViewModel> queryRepoNoteShared
            , IRepositoryQueryBase<EmailTaskViewModel> queryRepoEmailTask
            , IPageBusiness pageBusiness
            , INoteBusiness noteBusiness
            , IRepositoryQueryBase<ServiceIndexPageTemplateViewModel> queryServiceRepo
            , IRepositoryQueryBase<TaskIndexPageTemplateViewModel> querytaskRepo
            , IRepositoryQueryBase<NoteIndexPageTemplateViewModel> queryNoteRepo
            , ITaskBusiness taskBusiness
            , IServiceBusiness serviceBusiness
            , IUserContext userContext
            , IServiceProvider serviceProvider
            , ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryNoteRepo = queryNoteRepo;
            _querytaskRepo = querytaskRepo;
            _queryServiceRepo = queryServiceRepo;
            _queryRepoNoteShared = queryRepoNoteShared;
            _queryRepoTaskShared = queryRepoTaskShared;
            _queryRepoServiceShared = queryRepoServiceShared;
            _queryRepoEmailTask = queryRepoEmailTask;
            _pageBusiness = pageBusiness;
            _noteBusiness = noteBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _cmsQueryBusiness = cmsQueryBusiness;
            _serviceProvider = serviceProvider;
        }

        public async Task ManageTable(TableMetadataViewModel tableMetadata)
        {
            var existingTableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableMetadata.Id);
            if (existingTableMetadata != null)
            {
                existingTableMetadata.OldName = tableMetadata.OldName;
                existingTableMetadata.OldSchema = tableMetadata.OldSchema;
                //var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                //(
                // select 1
                // from information_schema.{(existingTableMetadata.TableType == TableTypeEnum.Table ? "tables" : "views")} 
                // where table_schema = '{existingTableMetadata.Schema}'
                // and table_name = '{existingTableMetadata.Name}'
                //) ", null);

                var tableExists = await _cmsQueryBusiness.ManageTableExists(existingTableMetadata);
                if (tableExists)
                {

                    //var recordExists = await _queryRepo.ExecuteScalar<bool?>(@$"select true from  
                    //{existingTableMetadata.Schema}.""{existingTableMetadata.Name}"" limit 1 ", null);

                    var recordExists = await _cmsQueryBusiness.ManageTableRecordExists(existingTableMetadata);
                    if (recordExists.IsTrue())
                    {
                        await EditTable(existingTableMetadata);
                    }
                    else
                    {
                        await CreateTable(existingTableMetadata, true);
                    }

                }
                else
                {
                    await CreateTable(existingTableMetadata, false);
                }

            }

        }


        private async Task EditTable(TableMetadataViewModel tableMetadata)
        {
            if (tableMetadata.TableType == TableTypeEnum.Table)
            {
                switch (tableMetadata.TemplateType)
                {
                    case TemplateTypeEnum.Form:
                        await EditFormTable(tableMetadata);
                        break;
                    case TemplateTypeEnum.Note:
                        await EditNoteTable(tableMetadata);
                        break;
                    case TemplateTypeEnum.Task:
                        await EditTaskTable(tableMetadata);
                        break;
                    case TemplateTypeEnum.Service:
                        await EditServiceTable(tableMetadata);
                        break;
                    case TemplateTypeEnum.Custom:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //var tableVarWithSchema = "<<table-schema>>";
                //var query = new StringBuilder();
                //var oldTableWithSchema = $"{tableMetadata.OldSchema}.\"{tableMetadata.OldName}\"";
                //query.Append($"DROP View {oldTableWithSchema};");
                //query.Append($"CREATE OR REPLACE VIEW {tableVarWithSchema} As {tableMetadata.Query} ;");
                //query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

                //var tableWithSchema = $"{tableMetadata.Schema}.\"{tableMetadata.Name}\"";
                //var table = $"{tableMetadata.Name}";
                //var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);
                //await _queryRepo.ExecuteCommand(tableQuery, null);

                await _cmsQueryBusiness.EditTableSchema(tableMetadata);
            }
        }

        private async Task CreateTable(TableMetadataViewModel tableMetadata, bool dropTable)
        {

            if (tableMetadata.TableType == TableTypeEnum.Table)
            {
                switch (tableMetadata.TemplateType)
                {

                    case TemplateTypeEnum.Form:
                        await CreateFormTable(tableMetadata, dropTable);
                        break;
                    case TemplateTypeEnum.Note:
                        await CreateNoteTable(tableMetadata, dropTable);
                        break;
                    case TemplateTypeEnum.Task:
                        await CreateTaskTable(tableMetadata, dropTable);
                        break;
                    case TemplateTypeEnum.Service:
                        await CreateServiceTable(tableMetadata, dropTable);
                        break;
                    case TemplateTypeEnum.Custom:
                    case TemplateTypeEnum.FormIndexPage:
                    case TemplateTypeEnum.Page:
                    default:
                        break;
                }
            }
            else
            {
                //var tableVarWithSchema = "<<table-schema>>";
                //var query = new StringBuilder();
                //if (dropTable)
                //{
                //    query.Append($"DROP View {tableVarWithSchema};");
                //}
                //query.Append($"CREATE OR REPLACE VIEW {tableVarWithSchema} As {tableMetadata.Query} ;");
                //query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

                //var tableWithSchema = $"{tableMetadata.Schema}.\"{tableMetadata.Name}\"";
                //var table = $"{tableMetadata.Name}";
                //var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);
                //await _queryRepo.ExecuteCommand(tableQuery, null);

                await _cmsQueryBusiness.CreateTableSchema(tableMetadata, dropTable);
            }
        }

        private async Task CreateFormTable(TableMetadataViewModel tableMetadata, bool dropTable)
        {
            var tableVarWithSchema = "<<table-schema>>";
            var tableVar = "<<table>>";

            var columns = await _repo.GetList<ColumnMetadata, ColumnMetadata>(x => x.TableMetadataId == tableMetadata.Id && x.IsVirtualColumn == false);
            var query = new StringBuilder();
            if (dropTable)
            {
                query.Append($"DROP TABLE {tableVarWithSchema};");
            }
            query.Append($"CREATE TABLE {tableVarWithSchema}(");
            var pk = "";
            var fk = "";
            var columnStr = new List<string>();
            foreach (var column in columns)
            {
                var columnText = "";
                var type = ConvertToPostgreType(column);

                if (column.IsPrimaryKey)
                {
                    pk = @$", CONSTRAINT ""PK_{tableVar}"" PRIMARY KEY (""{column.Name}"")";
                }
                if (column.IsForeignKey && column.IsVirtualForeignKey == false && column.IsMultiValueColumn == false)
                {
                    fk += $@"CONSTRAINT ""{column.ForeignKeyConstraintName}"" FOREIGN KEY (""{column.Name}"") REFERENCES {column.ForeignKeyTableSchemaName}.""{column.ForeignKeyTableName}"" (""{column.ForeignKeyColumnName}"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL,";
                }
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
            if (fk.Trim(',').IsNotNullAndNotEmpty())
            {
                query.Append(",");
            }
            query.Append(fk.Trim(','));
            query.Append(Environment.NewLine);
            query.Append(')');
            query.Append(Environment.NewLine);
            query.Append($"TABLESPACE {ApplicationConstant.Database.TableSpace};");
            query.Append(Environment.NewLine);
            query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

            var tableWithSchema = $"{ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"";
            var table = $"{tableMetadata.Name}";
            var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);
            //await _queryRepo.ExecuteCommand(tableQuery, null);

            await _cmsQueryBusiness.TableQueryExecute(tableQuery);

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

        public async Task<DataTable> GetDataListByTemplate(string templateCode, string templateId, string where = null)
        {
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == templateCode || x.Id == templateId);
            if (template != null)
            {
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(template.UdfTableMetadataId ?? template.TableMetadataId);
                if (tableMetadata != null)
                {
                    var selectQuery = await GetSelectQuery(tableMetadata, where);
                    //var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                    var dt = await _cmsQueryBusiness.GetQueryDataTable(selectQuery);
                    return dt;
                }
            }

            return null;
        }
        private async Task CreateNoteTable(TableMetadataViewModel tableMetadata, bool dropTable)
        {
            var tableVarWithSchema = "<<table-schema>>";
            var tableVar = "<<table>>";
            var columns = await _repo.GetList<ColumnMetadata, ColumnMetadata>(x => x.TableMetadataId == tableMetadata.Id && x.IsVirtualColumn == false);
            var query = new StringBuilder();
            if (dropTable)
            {
                query.Append($"DROP TABLE {tableVarWithSchema} cascade;");
            }
            query.Append($"CREATE TABLE {tableVarWithSchema}(");
            var pk = "";
            var fk = "";
            var columnStr = new List<string>();
            foreach (var column in columns.Where(x => !x.IsReferenceColumn))
            {
                var columnText = "";
                var type = ConvertToPostgreType(column);

                if (column.IsPrimaryKey)
                {
                    pk = @$", CONSTRAINT ""PK_{tableVar}"" PRIMARY KEY (""{column.Name}""),";
                }
                if (column.IsForeignKey && column.IsVirtualForeignKey == false && column.IsMultiValueColumn == false)
                {
                    fk += $@"CONSTRAINT ""{column.ForeignKeyConstraintName}"" FOREIGN KEY (""{column.Name}"") REFERENCES {column.ForeignKeyTableSchemaName}.""{column.ForeignKeyTableName}"" (""{column.ForeignKeyColumnName}"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL,";
                }
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
            query.Append(fk.Trim(','));
            //query.Append($@"CONSTRAINT ""FK_{tableMetadata.Name}_NtsNote_NtsNoteId"" FOREIGN KEY (""NtsNoteId"") REFERENCES {ApplicationConstant.Database.Schema._Public}.""NtsNote"" (""Id"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL");
            //query.Append(Environment.NewLine);
            query.Append(')');
            query.Append(Environment.NewLine);
            query.Append($"TABLESPACE {ApplicationConstant.Database.TableSpace};");
            query.Append(Environment.NewLine);
            query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

            var tableWithSchema = $"{ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"";
            var table = $"{tableMetadata.Name}";
            var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);
            //await _queryRepo.ExecuteCommand(tableQuery, null);

            await _cmsQueryBusiness.TableQueryExecute(tableQuery);

            await _noteBusiness.CreateLogTable(tableMetadata);

            //var tableLogWithSchema = $"{ApplicationConstant.Database.Schema.Log}.\"{tableMetadata.Name}_Log\"";
            //var tableLog = $"{tableMetadata.Name}_Log";
            //var tableLogQuery = query.ToString().Replace("<<table-schema>>", tableLogWithSchema).Replace("<<table>>", tableLog);
            //await _queryRepo.ExecuteCommand(tableLogQuery, null);
        }

        private async Task CreateTaskTable(TableMetadataViewModel tableMetadata, bool dropTable)
        {
            var tableVarWithSchema = "<<table-schema>>";
            var tableVar = "<<table>>";
            var columns = await _repo.GetList<ColumnMetadata, ColumnMetadata>(x => x.TableMetadataId == tableMetadata.Id && x.IsVirtualColumn == false);
            var query = new StringBuilder();
            if (dropTable)
            {
                query.Append($"DROP TABLE {tableVarWithSchema};");
            }
            query.Append($"CREATE TABLE {tableVarWithSchema}(");
            var pk = "";
            var fk = "";
            var columnStr = new List<string>();
            foreach (var column in columns.Where(x => !x.IsReferenceColumn))
            {
                var columnText = "";
                var type = ConvertToPostgreType(column);

                if (column.IsPrimaryKey)
                {
                    pk = @$", CONSTRAINT ""PK_{tableVar}"" PRIMARY KEY (""{column.Name}""),";
                }
                if (column.IsForeignKey && column.IsVirtualForeignKey == false && column.IsMultiValueColumn == false)
                {
                    fk += $@"CONSTRAINT ""{column.ForeignKeyConstraintName}"" FOREIGN KEY (""{column.Name}"") REFERENCES {column.ForeignKeyTableSchemaName}.""{column.ForeignKeyTableName}"" (""{column.ForeignKeyColumnName}"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL,";
                }
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
            query.Append(fk.Trim(','));
            //query.Append($@"CONSTRAINT ""FK_{tableMetadata.Name}_NtsNote_NtsNoteId"" FOREIGN KEY (""NtsNoteId"") REFERENCES {ApplicationConstant.Database.Schema._Public}.""NtsNote"" (""Id"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL");
            //query.Append(Environment.NewLine);
            query.Append(')');
            query.Append(Environment.NewLine);
            query.Append($"TABLESPACE {ApplicationConstant.Database.TableSpace};");
            query.Append(Environment.NewLine);
            query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

            var tableWithSchema = $"{ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"";
            var table = $"{tableMetadata.Name}";
            var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);

            //await _queryRepo.ExecuteCommand(tableQuery, null);
            await _cmsQueryBusiness.TableQueryExecute(tableQuery);

            //var tableLogWithSchema = $"{ApplicationConstant.Database.Schema.Log}.\"{tableMetadata.Name}_Log\"";
            //var tableLog = $"{tableMetadata.Name}_Log";
            //var tableLogQuery = query.ToString().Replace("<<table-schema>>", tableLogWithSchema).Replace("<<table>>", tableLog);
            //await _queryRepo.ExecuteCommand(tableLogQuery, null);
        }
        private async Task CreateServiceTable(TableMetadataViewModel tableMetadata, bool dropTable)
        {
            var tableVarWithSchema = "<<table-schema>>";
            var tableVar = "<<table>>";
            var columns = await _repo.GetList<ColumnMetadata, ColumnMetadata>(x => x.TableMetadataId == tableMetadata.Id && x.IsVirtualColumn == false);
            var query = new StringBuilder();
            if (dropTable)
            {
                query.Append($"DROP TABLE {tableVarWithSchema};");
            }
            query.Append($"CREATE TABLE {tableVarWithSchema}(");
            var pk = "";
            var fk = "";
            var columnStr = new List<string>();
            foreach (var column in columns.Where(x => !x.IsReferenceColumn))
            {
                var columnText = "";
                var type = ConvertToPostgreType(column);

                if (column.IsPrimaryKey)
                {
                    pk = @$", CONSTRAINT ""PK_{tableVar}"" PRIMARY KEY (""{column.Name}""),";
                }
                if (column.IsForeignKey && column.IsVirtualForeignKey == false && column.IsMultiValueColumn == false)
                {
                    fk += $@"CONSTRAINT ""{column.ForeignKeyConstraintName}"" FOREIGN KEY (""{column.Name}"") REFERENCES {column.ForeignKeyTableSchemaName}.""{column.ForeignKeyTableName}"" (""{column.ForeignKeyColumnName}"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL,";
                }
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
            query.Append(fk.Trim(','));
            //query.Append($@"CONSTRAINT ""FK_{tableMetadata.Name}_NtsNote_NtsNoteId"" FOREIGN KEY (""NtsNoteId"") REFERENCES {ApplicationConstant.Database.Schema._Public}.""NtsNote"" (""Id"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL");
            //query.Append(Environment.NewLine);
            query.Append(')');
            query.Append(Environment.NewLine);
            query.Append($"TABLESPACE {ApplicationConstant.Database.TableSpace};");
            query.Append(Environment.NewLine);
            query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

            var tableWithSchema = $"{ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"";
            var table = $"{tableMetadata.Name}";
            var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);

            //await _queryRepo.ExecuteCommand(tableQuery, null);
            await _cmsQueryBusiness.TableQueryExecute(tableQuery);

            //var tableLogWithSchema = $"{ApplicationConstant.Database.Schema.Log}.\"{tableMetadata.Name}_Log\"";
            //var tableLog = $"{tableMetadata.Name}_Log";
            //var tableLogQuery = query.ToString().Replace("<<table-schema>>", tableLogWithSchema).Replace("<<table>>", tableLog);
            //await _queryRepo.ExecuteCommand(tableLogQuery, null);
        }


        private async Task EditNoteTable(TableMetadataViewModel tableMetadata)
        {
            //   var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            //   from information_schema.columns where table_schema = '{tableMetadata.Schema}'
            //and table_name = '{tableMetadata.Name}'", null);

            var existColumn = await _cmsQueryBusiness.GetEditNoteTableExistColumn(tableMetadata);

            //var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            //FROM pg_catalog.pg_constraint con
            //INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            //INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            //WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

            var constraints = await _cmsQueryBusiness.GetEditNoteTableConstraints(tableMetadata);

            var tableColumnList = new List<ColumnMetadataViewModel>();
            foreach (DataRow row in existColumn.Rows)
            {
                var data = new ColumnMetadataViewModel();
                data.Name = row["column_name"].ToString();
                data.DataTypestr = row["data_type"].ToString();
                data.IsNullable = Convert.ToString(row["is_nullable"]) == "YES" ? true : false;
                tableColumnList.Add(data);
            }
            var existingMetadaColumnList = await _repo.GetList<ColumnMetadata, ColumnMetadata>
                (x => x.TableMetadataId == tableMetadata.Id && x.IsReferenceColumn == false && x.IsVirtualColumn == false);
            var query = new StringBuilder($"ALTER TABLE {ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"");
            var queryLog = new StringBuilder($"ALTER TABLE {ApplicationConstant.Database.Schema.Log}.\"{tableMetadata.Name}Log\"");
            var alterColumnScriptList = new List<string>();
            foreach (var existingMetadaColumn in existingMetadaColumnList)
            {
                var postgreType = ConvertToPostgreType(existingMetadaColumn);
                var isInTable = tableColumnList.FirstOrDefault(x => x.Name == existingMetadaColumn.Name);
                if (isInTable != null)
                {
                    if (postgreType != isInTable.DataTypestr)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\" TYPE {postgreType}");
                        ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);

                    }
                    else if (existingMetadaColumn.IsNullable != isInTable.IsNullable)
                    {
                        if (existingMetadaColumn.IsNullable)
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  DROP NOT NULL");
                        }
                        else
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  SET NOT NULL");

                        }
                    }

                }
                else
                {
                    var nullSet = "";
                    if (!existingMetadaColumn.IsNullable)
                    {
                        nullSet = " NOT NULL";
                    }


                    if (existingMetadaColumn.DataType == DataColumnTypeEnum.Text)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet} COLLATE {ApplicationConstant.Database.Schema._Public}.{ApplicationConstant.Database.DefaultCollation}");
                    }
                    else
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet}");
                    }
                    ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);
                }
            }
            if (alterColumnScriptList.Count > 0)
            {
                query.Append(string.Join(",", alterColumnScriptList));
                query.Append(";");
                var queryText = query.ToString();

                //await _queryRepo.ExecuteCommand(queryText, null);
                await _cmsQueryBusiness.TableQueryExecute(queryText);

                queryLog.Append(string.Join(",", alterColumnScriptList));
                queryLog.Append(";");

                //await _queryRepo.ExecuteCommand(queryLog.ToString(), null);
                await _cmsQueryBusiness.TableQueryExecute(queryLog.ToString());

            }

        }
        private async Task EditTaskTable(TableMetadataViewModel tableMetadata)
        {
            //   var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            //   from information_schema.columns where table_schema = '{tableMetadata.Schema}'
            //and table_name = '{tableMetadata.Name}'", null);

            var existColumn = await _cmsQueryBusiness.GetEditTaskTableExistColumn(tableMetadata);

            //var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            //FROM pg_catalog.pg_constraint con
            //INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            //INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            //WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

            var constraints = await _cmsQueryBusiness.GetEditTaskTableConstraints(tableMetadata);

            var tableColumnList = new List<ColumnMetadataViewModel>();
            foreach (DataRow row in existColumn.Rows)
            {
                var data = new ColumnMetadataViewModel();
                data.Name = row["column_name"].ToString();
                data.DataTypestr = row["data_type"].ToString();
                data.IsNullable = Convert.ToString(row["is_nullable"]) == "YES" ? true : false;
                tableColumnList.Add(data);
            }
            var existingMetadaColumnList = await _repo.GetList<ColumnMetadata, ColumnMetadata>
                (x => x.TableMetadataId == tableMetadata.Id && x.IsReferenceColumn == false && x.IsVirtualColumn == false);
            var query = new StringBuilder($"ALTER TABLE {ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"");
            var alterColumnScriptList = new List<string>();
            foreach (var existingMetadaColumn in existingMetadaColumnList)
            {
                var postgreType = ConvertToPostgreType(existingMetadaColumn);
                var isInTable = tableColumnList.FirstOrDefault(x => x.Name == existingMetadaColumn.Name);
                if (isInTable != null)
                {
                    if (postgreType != isInTable.DataTypestr)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\" TYPE {postgreType}");
                        ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);

                    }
                    else if (existingMetadaColumn.IsNullable != isInTable.IsNullable)
                    {
                        if (existingMetadaColumn.IsNullable)
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  DROP NOT NULL");
                        }
                        else
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  SET NOT NULL");

                        }
                    }

                }
                else
                {
                    var nullSet = "";
                    if (!existingMetadaColumn.IsNullable)
                    {
                        nullSet = " NOT NULL";
                    }


                    if (existingMetadaColumn.DataType == DataColumnTypeEnum.Text)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet} COLLATE {ApplicationConstant.Database.Schema._Public}.{ApplicationConstant.Database.DefaultCollation}");
                    }
                    else
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet}");
                    }
                    ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);
                }
            }
            if (alterColumnScriptList.Count > 0)
            {
                query.Append(string.Join(",", alterColumnScriptList));
                query.Append(";");
                var queryText = query.ToString();

                //await _queryRepo.ExecuteCommand(queryText, null);
                await _cmsQueryBusiness.TableQueryExecute(queryText);
            }

        }
        private async Task EditServiceTable(TableMetadataViewModel tableMetadata)
        {
            //   var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            //   from information_schema.columns where table_schema = '{tableMetadata.Schema}'
            //and table_name = '{tableMetadata.Name}'", null);

            var existColumn = await _cmsQueryBusiness.GetEditServiceTableExistColumn(tableMetadata);

            //var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            //FROM pg_catalog.pg_constraint con
            //INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            //INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            //WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

            var constraints = await _cmsQueryBusiness.GetEditServiceTableConstraints(tableMetadata);

            var tableColumnList = new List<ColumnMetadataViewModel>();
            foreach (DataRow row in existColumn.Rows)
            {
                var data = new ColumnMetadataViewModel();
                data.Name = row["column_name"].ToString();
                data.DataTypestr = row["data_type"].ToString();
                data.IsNullable = Convert.ToString(row["is_nullable"]) == "YES" ? true : false;
                tableColumnList.Add(data);
            }
            var existingMetadaColumnList = await _repo.GetList<ColumnMetadata, ColumnMetadata>
                (x => x.TableMetadataId == tableMetadata.Id && x.IsReferenceColumn == false && x.IsVirtualColumn == false);
            var query = new StringBuilder($"ALTER TABLE {ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"");
            var alterColumnScriptList = new List<string>();
            foreach (var existingMetadaColumn in existingMetadaColumnList)
            {
                var postgreType = ConvertToPostgreType(existingMetadaColumn);
                var isInTable = tableColumnList.FirstOrDefault(x => x.Name == existingMetadaColumn.Name);
                if (isInTable != null)
                {
                    if (postgreType != isInTable.DataTypestr)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\" TYPE {postgreType}");
                        ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);

                    }
                    else if (existingMetadaColumn.IsNullable != isInTable.IsNullable)
                    {
                        if (existingMetadaColumn.IsNullable)
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  DROP NOT NULL");
                        }
                        else
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  SET NOT NULL");

                        }
                    }

                }
                else
                {
                    var nullSet = "";
                    if (!existingMetadaColumn.IsNullable)
                    {
                        nullSet = " NOT NULL";
                    }


                    if (existingMetadaColumn.DataType == DataColumnTypeEnum.Text)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet} COLLATE {ApplicationConstant.Database.Schema._Public}.{ApplicationConstant.Database.DefaultCollation}");
                    }
                    else
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet}");
                    }
                    ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);
                }
            }
            if (alterColumnScriptList.Count > 0)
            {
                query.Append(string.Join(",", alterColumnScriptList));
                query.Append(";");
                var queryText = query.ToString();

                //await _queryRepo.ExecuteCommand(queryText, null);
                await _cmsQueryBusiness.TableQueryExecute(queryText);
            }

        }
        private async Task EditFormTable(TableMetadataViewModel tableMetadata)
        {
            //   var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            //   from information_schema.columns where table_schema = '{tableMetadata.Schema}'
            //and table_name = '{tableMetadata.Name}'", null);

            var existColumn = await _cmsQueryBusiness.GetEditFormTableExistColumn(tableMetadata);

            //var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            //FROM pg_catalog.pg_constraint con
            //INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            //INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            //WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

            var constraints = await _cmsQueryBusiness.GetEditServiceTableConstraints(tableMetadata);

            var tableColumnList = new List<ColumnMetadataViewModel>();
            foreach (DataRow row in existColumn.Rows)
            {
                var data = new ColumnMetadataViewModel();
                data.Name = row["column_name"].ToString();
                data.DataTypestr = row["data_type"].ToString();
                data.IsNullable = Convert.ToString(row["is_nullable"]) == "YES" ? true : false;
                tableColumnList.Add(data);
            }
            var existingMetadaColumnList = await _repo.GetList<ColumnMetadata, ColumnMetadata>
                (x => x.TableMetadataId == tableMetadata.Id && x.IsVirtualColumn == false);
            var query = new StringBuilder($"ALTER TABLE {ApplicationConstant.Database.Schema.Cms}.\"{tableMetadata.Name}\"");
            var alterColumnScriptList = new List<string>();
            foreach (var existingMetadaColumn in existingMetadaColumnList)
            {
                var postgreType = ConvertToPostgreType(existingMetadaColumn);
                var isInTable = tableColumnList.FirstOrDefault(x => x.Name == existingMetadaColumn.Name);
                if (isInTable != null)
                {
                    if (postgreType != isInTable.DataTypestr)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\" TYPE {postgreType}");
                        ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);

                    }
                    else if (existingMetadaColumn.IsNullable != isInTable.IsNullable)
                    {
                        if (existingMetadaColumn.IsNullable)
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  DROP NOT NULL");
                        }
                        else
                        {
                            alterColumnScriptList.Add($"{Environment.NewLine}ALTER COLUMN \"{existingMetadaColumn.Name}\"  SET NOT NULL");

                        }
                    }

                }
                else
                {
                    var nullSet = "";
                    if (!existingMetadaColumn.IsNullable)
                    {
                        nullSet = " NOT NULL";
                    }


                    if (existingMetadaColumn.DataType == DataColumnTypeEnum.Text)
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet} COLLATE {ApplicationConstant.Database.Schema._Public}.{ApplicationConstant.Database.DefaultCollation}");
                    }
                    else
                    {
                        alterColumnScriptList.Add($"{Environment.NewLine}ADD COLUMN \"{existingMetadaColumn.Name}\" {postgreType} {nullSet}");
                    }
                    ManageForeignKey(alterColumnScriptList, existingMetadaColumn, constraints);
                }
            }
            if (alterColumnScriptList.Count > 0)
            {
                query.Append(string.Join(",", alterColumnScriptList));
                query.Append(";");
                var queryText = query.ToString();

                //await _queryRepo.ExecuteCommand(queryText, null);
                await _cmsQueryBusiness.TableQueryExecute(queryText);
            }

        }

        private void ManageForeignKey(List<string> alterColumnScriptList, ColumnMetadata column, DataTable constraints)
        {
            if (column.IsForeignKey && column.IsVirtualForeignKey == false && column.ForeignKeyConstraintName.IsNotNullAndNotEmpty())
            {
                var existingFk = constraints.AsEnumerable().FirstOrDefault
                    (r => r.Field<string>("conname") == column.ForeignKeyConstraintName);
                if (existingFk != null)
                {
                    alterColumnScriptList.Add($@"DROP CONSTRAINT ""{column.ForeignKeyConstraintName}""");
                }
                alterColumnScriptList.Add($@"ADD CONSTRAINT ""{column.ForeignKeyConstraintName}"" FOREIGN KEY (""{column.Name}"") REFERENCES {column.ForeignKeyTableSchemaName}.""{column.ForeignKeyTableName}"" (""{column.ForeignKeyColumnName}"") MATCH SIMPLE ON UPDATE NO ACTION  ON DELETE SET NULL");
            }
        }

        public async Task<DataTable> GetFormIndexPageGridData(string indexPageTemplateId, DataSourceRequest request)
        {
            var indexPageTemplate = await _repo.GetSingleById<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(indexPageTemplateId);
            if (indexPageTemplate != null)
            {
                var template = await _repo.GetSingleById<TemplateViewModel, Template>(indexPageTemplate.TemplateId);
                if (template != null)
                {
                    var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(template.TableMetadataId);
                    if (tableMetadata != null)
                    {
                        var selectQuery = await GetFormSelectQuery(tableMetadata);
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
                        //var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                        var dt = await _cmsQueryBusiness.GetQueryDataTable(selectQuery);
                        return dt;

                    }
                }
            }
            return new DataTable();
        }



        public async Task<DataRow> GetDataById(string recordId, string udfTableMetadataId, string templateId, TemplateTypeEnum pageType)
        {
            var baseQuery = "";
            var selectQuery = "";
            var dt = new DataTable();
            TableMetadataViewModel tableMetaData = null;
            switch (pageType)
            {
                case TemplateTypeEnum.Task:
                    if (udfTableMetadataId.IsNullOrEmpty())
                    {
                        var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.TemplateId == templateId);
                        udfTableMetadataId = stepTaskComponent?.UdfTableMetadataId;
                    }
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == udfTableMetadataId);
                    dt = await _taskBusiness.GetTaskDataTableById(recordId, tableMetaData);
                    break;
                case TemplateTypeEnum.Service:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == udfTableMetadataId);
                    dt = await _serviceBusiness.GetServiceDataTableById(recordId, tableMetaData);
                    break;
                case TemplateTypeEnum.Note:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == udfTableMetadataId);
                    dt = await _noteBusiness.GetNoteDataTableById(recordId, tableMetaData);
                    break;
                case TemplateTypeEnum.Form:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == udfTableMetadataId);
                    baseQuery = await GetFormSelectQuery(tableMetaData);
                    selectQuery = @$"{baseQuery} and ""{tableMetaData.Name}"".""Id""='{recordId}' limit 1";
                    dt = await _cmsQueryBusiness.GetQueryDataTable(selectQuery);
                    break;
                default:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == udfTableMetadataId);
                    baseQuery = await GetSelectQuery(tableMetaData);
                    selectQuery = @$"{baseQuery} and ""{tableMetaData.Name}"".""Id""='{recordId}' limit 1";

                    //dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                    dt = await _cmsQueryBusiness.GetQueryDataTable(selectQuery);

                    break;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                var dict = dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
                var j = JsonConvert.SerializeObject(dict);
                return dr;
            }
            return null;

        }

        public async Task<DataRow> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId, bool isLog = false, string logId = null)
        {
            var baseQuery = "";
            var selectQuery = "";
            var dt = new DataTable();
            TableMetadataViewModel tableMetaData = null;
            var tablemetaid = "";
            switch (page.PageType)
            {
                case TemplateTypeEnum.Task:
                    var udfTableMetadataId = page.Template.UdfTableMetadataId;
                    if (udfTableMetadataId.IsNullOrEmpty())
                    {
                        var stepTaskComponent = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.TemplateId == page.Template.Id);
                        udfTableMetadataId = stepTaskComponent?.UdfTableMetadataId;
                    }
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == udfTableMetadataId);
                    dt = await _taskBusiness.GetTaskDataTableById(recordId, tableMetaData, isLog, logId);
                    tablemetaid = udfTableMetadataId;
                    break;
                case TemplateTypeEnum.Service:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.UdfTableMetadataId);
                    dt = await _serviceBusiness.GetServiceDataTableById(recordId, tableMetaData, isLog, logId);
                    tablemetaid = page.Template.UdfTableMetadataId;
                    break;
                case TemplateTypeEnum.Note:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.TableMetadataId);
                    dt = await _noteBusiness.GetNoteDataTableById(recordId, tableMetaData, isLog, logId);
                    tablemetaid = page.Template.TableMetadataId;

                    break;
                case TemplateTypeEnum.Form:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.TableMetadataId);
                    baseQuery = await GetFormSelectQuery(tableMetaData);
                    selectQuery = @$"{baseQuery} and ""{tableMetaData.Name}"".""Id""='{recordId}' limit 1";
                    dt = await _cmsQueryBusiness.GetQueryDataTable(selectQuery);
                    tablemetaid = page.Template.TableMetadataId;

                    break;
                default:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.TableMetadataId);
                    baseQuery = await GetSelectQuery(tableMetaData);
                    selectQuery = @$"{baseQuery} and ""{tableMetaData.Name}"".""Id""='{recordId}' limit 1";

                    //dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
                    dt = await _cmsQueryBusiness.GetQueryDataTable(selectQuery);
                    tablemetaid = page.Template.TableMetadataId;

                    break;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                var dict = dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
                var j = JsonConvert.SerializeObject(dict);
                var tc = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tablemetaid);
                var childTable = tc.Where(x => x.UdfUIType == UdfUITypeEnum.editgrid || x.UdfUIType == UdfUITypeEnum.datagrid).ToList();
                foreach (var child in childTable)
                {
                    var tableName = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == child.Name);
                    //var editRec = await GetDataById(TemplateTypeEnum.Form, new PageViewModel { PageType = TemplateTypeEnum.Form, Template = new Template { TableMetadataId = tableName.TableMetadataId } }, dataId.ToString());
                    var recId = "";
                    if (page.PageType==TemplateTypeEnum.Form)
                    {
                        recId = dr["Id"].ToString();
                    }
                    else
                    {
                        recId = dr["UdfNoteTableId"].ToString();
                    }
                    var editRec = await GetData(ApplicationConstant.Database.Schema.Cms, tableName.Name, "ParentId", recId);

                    if (editRec != null)
                    {
                        //if (editRec.Rows.Count > 0)
                        //{
                        var json = JsonConvert.SerializeObject(editRec);
                        dr[child.Name] = json;
                        //}
                    }
                }
                return dr;
            }
            return null;

        }
        public async Task<DataTable> GetData(string schemaName, string tableName, string columns = null, string filter = null, string orderbyColumns = null, OrderByEnum orderby = OrderByEnum.Ascending, string where = null, bool ignoreJoins = false, string returnColumns = null, int? limit = null, int? skip = null, bool enableLocalization = false, string lang = null)
        {
            var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Schema == schemaName && x.Name == tableName);
            var selectQuery = "";
            switch (tableMetaData.TemplateType)
            {
                case TemplateTypeEnum.Note:
                    selectQuery = await _noteBusiness.GetSelectQuery(tableMetaData, where, columns, filter, false, null, ignoreJoins, returnColumns, limit, skip);
                    break;
                case TemplateTypeEnum.Task:
                    selectQuery = await _taskBusiness.GetSelectQuery(tableMetaData, where, columns, filter, false, null, ignoreJoins, returnColumns, limit, skip);
                    break;
                case TemplateTypeEnum.Service:
                    selectQuery = await _serviceBusiness.GetSelectQuery(tableMetaData, where, columns, filter, false, null, ignoreJoins, returnColumns, limit, skip);
                    break;
                case TemplateTypeEnum.Form:
                    selectQuery = await GetFormSelectQuery(tableMetaData, where, columns, filter, ignoreJoins, returnColumns, limit, skip, enableLocalization, lang);
                    break;
                default:
                    selectQuery = await GetSelectQuery(tableMetaData, where, columns, filter, ignoreJoins, returnColumns, limit, skip, enableLocalization);
                    break;
                    //default:
                    //    selectQuery = await GetCustomSelectQuery(tableMetaData, columns, filter);
                    //    break;
            }
            //return await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
            var dt = await _cmsQueryBusiness.GetQueryDataTable(selectQuery);
            return dt;
        }

        private async Task<string> GetCustomSelectQuery(TableMetadataViewModel tableMetaData, string filtercolumns, string filter)
        {
            var columns = new List<string>();
            var tables = new List<string>();
            var condition = new List<string>();
            var pkColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id
               && x.IsVirtualColumn == false);
            foreach (var item in pkColumns)
            {
                columns.Add(@$"{Environment.NewLine}""{tableMetaData.Name}"".""{item.Name}"" as ""{item.Name}""");
            }
            tables.Add(@$"{tableMetaData.Schema}.""{tableMetaData.Name}"" as ""{tableMetaData.Name}""");
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
            //var fks = pkColumns.Where(x => x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty() && x.HideForeignKeyTableColumns == false).ToList();
            //if (fks != null && fks.Count() > 0)
            //{
            //    foreach (var item in fks)
            //    {
            //        if (item.IsUdfColumn)
            //        {
            //            item.ForeignKeyTableAliasName = item.ForeignKeyTableName + "_" + item.Name;
            //        }
            //        tables.Add(@$"left join {item.ForeignKeyTableSchemaName}.""{item.ForeignKeyTableName}"" as ""{item.ForeignKeyTableAliasName}"" 
            //        on ""{tableMetaData.Name}"".""{item.Name}""=""{item.ForeignKeyTableAliasName}"".""{item.ForeignKeyColumnName}"" 
            //        and ""{item.ForeignKeyTableAliasName}"".""IsDeleted""=false");
            //    }
            //    var query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
            //    ,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
            //    ,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
            //    FROM public.""ColumnMetadata"" c
            //    join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
            //    join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false
            //    where c.""TableMetadataId""='{tableMetaData.Id}'
            //    and c.""IsForeignKey""=true and c.""IsDeleted""=false";
            //    var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            //    if (result != null && result.Count() > 0)
            //    {
            //        foreach (var item in result)
            //        {
            //            var tableName = @$"""{item.TableAliasName}""";
            //            if (item.IsUdfColumn)
            //            {
            //                tableName = @$"""{item.TableAliasName}_{item.ForeignKeyColumnName}""";
            //            }

            //            columns.Add(@$"{Environment.NewLine}{tableName}.""{item.Name}"" as ""{item.ForeignKeyColumnName}_{item.Name}""");
            //        }

            //    }
            //}

            var selectQuery = @$"select {string.Join(",", columns)}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            return selectQuery;
        }
        private async Task<string> GetFormSelectQuery(TableMetadataViewModel tableMetaData, string where = null, string filtercolumns = null, string filter = null, bool ignoreJoins = false, string returnColumns = null, int? limit = null, int? skip = null, bool enableLocalization = false, string lang = null)
        {
            var columns = new List<string>();
            var tables = new List<string>();
            var condition = new List<string>();
            var pkColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id && x.IsVirtualColumn == false);
            FormTemplateViewModel formTemplate = null;
            string displayColumnId = null;
            string displayColumn = null;
            if (enableLocalization || lang != "en-US")
            {
                //TODO Change to single qury
                var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.TableMetadataId == tableMetaData.Id);
                if (template != null)
                {
                    formTemplate = await _repo.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == template.Id);
                }

                displayColumnId = formTemplate?.DisplayColumnId;

            }
            string localizedColumn = null;
            foreach (var item in pkColumns)
            {
                if (displayColumnId.IsNotNullAndNotEmpty() && displayColumnId == item.Id)
                {
                    displayColumn = item.Name;
                    var appLanguage = _userContext.CultureName;
                    var col = @$"""{tableMetaData.Name}"".""{item.Name}"" as ""{item.Name}""";
                    if (lang.IsNotNullAndNotEmpty())
                    {
                        switch (lang)
                        {
                            case "ar-SA":
                                localizedColumn = col = @$"coalesce(lang.""Arabic"",""{tableMetaData.Name}"".""{item.Name}"") as ""{item.Name}""";
                                break;
                            case "hi-IN":
                                localizedColumn = col = @$"coalesce(lang.""Hindi"",""{tableMetaData.Name}"".""{item.Name}"") as ""{item.Name}""";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (appLanguage)
                        {
                            case "ar-SA":
                                localizedColumn = col = @$"coalesce(lang.""Arabic"",""{tableMetaData.Name}"".""{item.Name}"") as ""{item.Name}""";
                                break;
                            case "hi-IN":
                                localizedColumn = col = @$"coalesce(lang.""Hindi"",""{tableMetaData.Name}"".""{item.Name}"") as ""{item.Name}""";
                                break;
                            default:
                                break;
                        }
                    }

                    columns.Add(@$"{Environment.NewLine}{col}");
                }
                else
                {
                    columns.Add(@$"{Environment.NewLine}""{tableMetaData.Name}"".""{item.Name}"" as ""{item.Name}""");
                }

            }
            tables.Add(@$"{tableMetaData.Schema}.""{tableMetaData.Name}"" as ""{tableMetaData.Name}""");
            condition.Add(@$"""{tableMetaData.Name}"".""IsDeleted""=false");
            var formTemlateViewModel = await _cmsQueryBusiness.GetSelectQueryData(tableMetaData);
            if (formTemlateViewModel != null && formTemlateViewModel.EnableLegalEntityFilter)
            {
                condition.Add(@$"""{tableMetaData.Name}"".""LegalEntityId""='{_userContext.LegalEntityId}'");
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
            if (displayColumnId.IsNotNullAndNotEmpty())
            {
                tables.Add(@$"left join public.""FormResourceLanguage"" as lang 
                    on ""{tableMetaData.Name}"".""Id""=lang.""FormTableId"" 
                    and lang.""IsDeleted""=false");
            }
            var fks = pkColumns.Where(x => x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty() && x.HideForeignKeyTableColumns == false).ToList();
            if (fks != null && fks.Count() > 0)
            {
                foreach (var item in fks)
                {
                    if (item.IsUdfColumn)
                    {
                        item.ForeignKeyTableAliasName = item.ForeignKeyTableName + "_" + item.Name;
                    }
                    tables.Add(@$"left join {item.ForeignKeyTableSchemaName}.""{item.ForeignKeyTableName}"" as ""{item.ForeignKeyTableAliasName}"" 
                    on ""{tableMetaData.Name}"".""{item.Name}""=""{item.ForeignKeyTableAliasName}"".""{item.ForeignKeyColumnName}"" 
                    and ""{item.ForeignKeyTableAliasName}"".""IsDeleted""=false");
                }
                var result = await _cmsQueryBusiness.GetForeignKeyColumnByTableMetadata(tableMetaData);

                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        var tableName = @$"""{item.TableAliasName}""";
                        if (item.IsUdfColumn)
                        {
                            tableName = @$"""{item.TableAliasName}_{item.ForeignKeyColumnName}""";
                        }

                        columns.Add(@$"{Environment.NewLine}{tableName}.""{item.Name}"" as ""{item.ForeignKeyColumnName}_{item.Name}""");
                    }

                }
            }
            //var cols = "*";
            //if (columns.Any())
            //{
            //    cols = string.Join(",", columns);
            //}

            var selectQuery = @$"select { string.Join(",", columns)}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            if (where.IsNotNullAndNotEmpty())
            {
                where = where.Replace(",", "','");
                selectQuery = $"{selectQuery} {where}";
            }
            if (ignoreJoins)
            {
                var langQuery = "";
                if (returnColumns.IsNullOrEmpty())
                {
                    returnColumns = @$"""{tableMetaData.Name}"".*";
                }
                else
                {
                    returnColumns = @$"""{returnColumns.Replace(",", "\",\"")}""";

                }
                if (localizedColumn.IsNotNullAndNotEmpty())
                {
                    returnColumns = @$"{returnColumns},{localizedColumn}";
                    langQuery = @$"left join public.""FormResourceLanguage"" as lang  on ""{tableMetaData.Name}"".""Id""=lang.""FormTableId"" and lang.""IsDeleted""=false";
                }
                selectQuery = @$"select {returnColumns} from { tableMetaData.Schema}.""{ tableMetaData.Name}"" 
                {langQuery}
                where ""{tableMetaData.Name}"".""IsDeleted""=false ";
                if (where.IsNotNullAndNotEmpty())
                {
                    selectQuery = $"{selectQuery} {where}";
                }
            }
            return selectQuery;
        }

        private async Task<string> GetSelectQuery(TableMetadataViewModel tableMetaData, string where = null, string filtercolumns = null, string filter = null, bool ignoreJoins = false, string returnColumns = null, int? limit = null, int? skip = null, bool enableLocalization = false)
        {
            var columns = new List<string>();
            var tables = new List<string>();
            var condition = new List<string>();
            var pkColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id
               && x.IsVirtualColumn == false);
            foreach (var item in pkColumns)
            {
                columns.Add(@$"{Environment.NewLine}""{tableMetaData.Name}"".""{item.Name}"" as ""{item.Name}""");
            }
            tables.Add(@$"{tableMetaData.Schema}.""{tableMetaData.Name}"" as ""{tableMetaData.Name}""");
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
            var fks = pkColumns.Where(x => x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty() && x.HideForeignKeyTableColumns == false).ToList();
            if (fks != null && fks.Count() > 0)
            {
                foreach (var item in fks)
                {
                    if (item.IsUdfColumn)
                    {
                        item.ForeignKeyTableAliasName = item.ForeignKeyTableName + "_" + item.Name;
                    }
                    tables.Add(@$"left join {item.ForeignKeyTableSchemaName}.""{item.ForeignKeyTableName}"" as ""{item.ForeignKeyTableAliasName}"" 
                    on ""{tableMetaData.Name}"".""{item.Name}""=""{item.ForeignKeyTableAliasName}"".""{item.ForeignKeyColumnName}"" 
                    and ""{item.ForeignKeyTableAliasName}"".""IsDeleted""=false");
                }
                //var query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
                //,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
                //,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
                //FROM public.""ColumnMetadata"" c
                //join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
                //join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false
                //where c.""TableMetadataId""='{tableMetaData.Id}'
                //and c.""IsForeignKey""=true and c.""IsDeleted""=false";

                //var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
                var result = await _cmsQueryBusiness.GetForeignKeyColumnByTableMetadata(tableMetaData);

                if (result != null && result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        var tableName = @$"""{item.TableAliasName}""";
                        if (item.IsUdfColumn)
                        {
                            tableName = @$"""{item.TableAliasName}_{item.ForeignKeyColumnName}""";
                        }

                        columns.Add(@$"{Environment.NewLine}{tableName}.""{item.Name}"" as ""{item.ForeignKeyColumnName}_{item.Name}""");
                    }

                }
            }
            var cols = "*";
            if (columns.Any())
            {
                cols = string.Join(",", columns);
            }

            var selectQuery = @$"select {cols}  {Environment.NewLine}from {string.Join(Environment.NewLine, tables)}{Environment.NewLine}where {string.Join(Environment.NewLine + "and ", condition)}";
            if (where.IsNotNullAndNotEmpty())
            {
                where = where.Replace(",", "','");
                selectQuery = $"{selectQuery} {where}";
            }
            if (ignoreJoins)
            {

                if (returnColumns.IsNullOrEmpty())
                {
                    returnColumns = "*";
                }
                else
                {
                    returnColumns = @$"""{returnColumns.Replace(",", "\",\"")}""";
                }
                selectQuery = @$"select {returnColumns} from { tableMetaData.Schema}.""{ tableMetaData.Name}"" where ""IsDeleted""=false ";
                if (where.IsNotNullAndNotEmpty())
                {
                    selectQuery = $"{selectQuery} {where}";
                }
            }
            return selectQuery;
        }

        private async Task<string> GetDataByColumn(ColumnMetadataViewModel column, object columnValue, TableMetadataViewModel tableMetaData, string excludeId)
        {

            var dt = await _cmsQueryBusiness.GetDataByColumn(column, columnValue, tableMetaData, excludeId);
            if (dt.Rows.Count > 0)
            {

                var dr = dt.Rows[0];
                var dict = dr.Table.Columns
                .Cast<DataColumn>()
                .ToDictionary(c => c.ColumnName, c => dr[c]);
                var j = JsonConvert.SerializeObject(dict);
                return j;
            }
            return string.Empty;
        }

        public async Task<FormIndexPageTemplateViewModel> GetFormIndexPageViewModel(PageViewModel page)
        {
            FormIndexPageTemplateViewModel model = null;
            switch (page.PageType)
            {

                case TemplateTypeEnum.FormIndexPage:
                    model = await _repo.GetSingle<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(x => x.TemplateId == page.TemplateId,y=>y.Template);
                    break;
                case TemplateTypeEnum.Form:
                    var form = await _repo.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == page.TemplateId);
                    if (form != null)
                    {
                        model = await _repo.GetSingleById<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(form.IndexPageTemplateId);
                    }
                    break;
                //case TemplateTypeEnum.Note:
                //    var note = await _repo.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == page.TemplateId);
                //    if (note != null)
                //    {
                //        model = await _repo.GetSingleById<FormIndexPageTemplateViewModel, NoteIndexPageTemplate>(note.NoteIndexPageTemplateId);
                //    }
                //    break;
                //case TemplateTypeEnum.Task:
                //    var task = await _repo.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == page.TemplateId);
                //    if (task != null)
                //    {
                //        model = await _repo.GetSingleById<FormIndexPageTemplateViewModel, TaskIndexPageTemplate>(task.TaskIndexPageTemplateId);
                //    }
                //    break;
                //case TemplateTypeEnum.Service:
                //    var service = await _repo.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == page.TemplateId);
                //    if (service != null)
                //    {
                //        model = await _repo.GetSingleById<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(service.ServiceIndexPageTemplateId);
                //    }
                //    break;
                case TemplateTypeEnum.Custom:
                    break;
                default:
                    break;
            }

            var indexPageColumns = await _repo.GetList<FormIndexPageColumnViewModel, FormIndexPageColumn>
                (x => x.FormIndexPageTemplateId == model.Id && x.IsDeleted == false, x => x.ColumnMetadata);
            var cloumns = new List<FormIndexPageColumnViewModel>();
            foreach (var item in indexPageColumns.OrderBy(x => x.SequenceOrder))
            {
                if (item.IsForeignKeyTableColumn)
                {
                    // item.ColumnName = $"{item.ForeignKeyTableAliasName}_{item.ColumnMetadata.Name}";
                }
                else
                {
                    item.ColumnName = item.ColumnMetadata.Name;
                }

                cloumns.Add(item);
            }
            model.SelectedTableRows = cloumns;
            return model;
        }

        public async Task<CommandResult<FormTemplateViewModel>> ManageForm(FormTemplateViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var presubmit = await ManagePresubmit(model);
                if (!presubmit.IsSuccess)
                {
                    return presubmit;
                }
                var result = await CreateForm(model.Json, model.PageId, model.TemplateCode);
                if (result.Item1)
                {
                    model.RecordId = result.Item2;
                    var table = await _repo.GetSingle<TemplateViewModel, Template>(x=>x.Code==model.TemplateCode);
                    var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(table.TableMetadataId);
                    var selectQuery = await GetSelectQuery(tableMetaData, @$" and ""{tableMetaData.Name}"".""Id""='{model.RecordId}' limit 1");
                    dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                    try
                    {
                        await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model, dobject);

                    }
                    catch (Exception ex)
                    {

                        return CommandResult<FormTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.Message}");
                    }
                    
                    return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model));
                }
                else
                {
                    return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model, result.Item1, result.Item2));
                }

            }
            else if (model.DataAction == DataActionEnum.Delete)
            {
                await DeleteFrom(model);
                return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model));
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                var presubmit = await ManagePresubmit(model);
                if (!presubmit.IsSuccess)
                {
                    return presubmit;
                }
                var result = await EditForm(model.RecordId, model.Json, model.PageId,model.TemplateCode);
                if (result.Item1)
                {
                    model.RecordId = result.Item2;
                    var table = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == model.TemplateCode);
                    var tableMetaData = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(table.TableMetadataId); 
                    var selectQuery = await GetSelectQuery(tableMetaData, @$" and ""{tableMetaData.Name}"".""Id""='{model.RecordId}' limit 1");
                    dynamic dobject = await _queryRepo.ExecuteQuerySingleDynamicObject(selectQuery, null);
                    try
                    {
                        await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PostSubmit, model, dobject);

                    }
                    catch (Exception ex)
                    {

                        return CommandResult<FormTemplateViewModel>.Instance(model, false, $"Error in post submit business rule execution: {ex.Message}");
                    }
                    return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model));
                }
                else
                {
                    return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model, result.Item1, result.Item2));
                }

            }
            return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model, false, "An error occured while processing your request"));
        }
        private async Task<CommandResult<FormTemplateViewModel>> ManagePresubmit(FormTemplateViewModel model)
        {
            dynamic dobject = new { };
            if (model.Json.IsNotNullAndNotEmptyAndNotValue("{}"))
            {
                dobject = model.Json.JsonToDynamicObject();
            }
            try
            {
                var result = await ManageBusinessRule(BusinessLogicExecutionTypeEnum.PreSubmit, model, dobject);
                return result;
            }
            catch (Exception ex)
            {
                return CommandResult<FormTemplateViewModel>.Instance(model, false, $"Error in pre submit business rule execution: {ex.Message}");
            }

        }
        private async Task<CommandResult<FormTemplateViewModel>> ManageBusinessRule(BusinessLogicExecutionTypeEnum executionType, FormTemplateViewModel viewModel, dynamic viewModelDynamicObject)
        {
            var businessRule = await _repo.GetSingle<BusinessRuleViewModel, BusinessRule>(x => x.TemplateId == viewModel.TemplateId
             && x.BusinessLogicExecutionType == executionType);
            if (businessRule != null)
            {
                try
                {
                    var businessRuleBusiness = _serviceProvider.GetService<IBusinessRuleBusiness>();
                    var result = await businessRuleBusiness.ExecuteBusinessRule<FormTemplateViewModel>(businessRule, TemplateTypeEnum.Form, viewModel, viewModelDynamicObject);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                businessRule = await _repo.GetSingle<BusinessRuleViewModel, BusinessRule>(x => x.TemplateId == viewModel.TemplateId
                && x.BusinessLogicExecutionType == executionType);
                if (businessRule != null)
                {
                    try
                    {
                        var businessRuleBusiness = _serviceProvider.GetService<IBusinessRuleBusiness>();
                        var result = await businessRuleBusiness.ExecuteBusinessRule<FormTemplateViewModel>(businessRule, TemplateTypeEnum.Form, viewModel, viewModelDynamicObject);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);

        }

        private async Task<string> DeleteFrom(FormTemplateViewModel model)
        {
            var page = await _pageBusiness.GetPageForExecution(model.PageId);
            if (page != null && page.Template != null)
            {
                var id = Guid.NewGuid().ToString();
                var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.TableMetadataId);
                if (tableMetaData != null)
                {
                    var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                    if (tableColumns != null && tableColumns.Count > 0)
                    {
                        //var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ");
                        //selectQuery.Append(Environment.NewLine);
                        //selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
                        //selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
                        //selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
                        //selectQuery.Append(@$"where ""Id""='{model.RecordId}'");
                        //var queryText = selectQuery.ToString();
                        //await _queryRepo.ExecuteCommand(queryText, null);

                        await _cmsQueryBusiness.DeleteFrom(model, tableMetaData);

                    }

                }
                return await Task.FromResult(id);
            }
            return await Task.FromResult(string.Empty);
        }
        public async Task<Tuple<bool, string>> CreateForm(string data, string pageId, string templateCode = null)
        {
            Template template = null;
            if (templateCode.IsNotNullAndNotEmpty())
            {
                template = await _repo.GetSingle(x => x.Code == templateCode);
            }
            else
            {
                var page = await _pageBusiness.GetPageForExecution(pageId);
                if (page != null)
                {
                    template = page.Template;
                }
            }
            // var page = await _pageBusiness.GetPageForExecution(pageId);
            if (template != null)
            {
                var id = Guid.NewGuid().ToString();
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == template.TableMetadataId);
                if (tableMetaData != null)
                {
                    var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                    if (tableColumns != null && tableColumns.Count > 0)
                    {
                        var validate = await ValidateForm(data, pageId, rowData, tableMetaData, tableColumns);
                        if (validate.IsNotNullAndNotEmpty())
                        {
                            return await Task.FromResult(new Tuple<bool, string>(false, validate));
                        }
                        var selectQuery = new StringBuilder(@$"insert into {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}""");
                        var columnKeys = new List<string>();
                        columnKeys.Add(@"""Id""");
                        var columnValues = new List<object>();
                        columnValues.Add(@$"'{id}'");
                        foreach (var col in tableColumns.Where(x => x.IsSystemColumn == false))
                        {
                            if (col.Name != "Id")
                            {
                                columnKeys.Add(@$"""{ col.Name}""");
                                columnValues.Add(BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType));
                            }
                        }

                        columnKeys.Add(@$"""SequenceOrder""");
                        var sq = rowData.GetValueOrDefault("SequenceOrder");
                        if (sq == null)
                        {
                            sq = 1;
                        }
                        columnValues.Add(sq);
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

                        columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                        columnValues.Add(@$"'{_repo.UserContext.UserId}'");
                        columnValues.Add(@$"'{DateTime.Now.ToDatabaseDateFormat()}'");
                        columnValues.Add(@$"'{_repo.UserContext.UserId}'");
                        columnValues.Add(@$"false");
                        columnValues.Add(@$"'{_repo.UserContext.CompanyId}'");
                        columnValues.Add(@$"'{_repo.UserContext.LegalEntityId}'");
                        //columnValues.Add(@$"1");
                        columnValues.Add(@$"{(int)StatusEnum.Active}");
                        columnValues.Add(@$"1");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append($"({string.Join(", ", columnKeys)})");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append($"values ({string.Join(", ", columnValues)})");
                        var queryText = selectQuery.ToString();

                        //await _queryRepo.ExecuteCommand(queryText, null);
                        await _cmsQueryBusiness.TableQueryExecute(queryText);
                        await ManageLocalization(rowData, id);
                    }
                    var childTable = tableColumns.Where(x => x.UdfUIType == UdfUITypeEnum.editgrid || x.UdfUIType == UdfUITypeEnum.datagrid).ToList();
                    foreach (var child in childTable)
                    {
                        var gridData = rowData.GetValueOrDefault(child.Name);
                        if (gridData != null)
                        {
                            //data = gridData.ToString();
                            //data = data.Replace("[", "").Replace("]", "");
                            //JArray result = JArray.FromObject(gridData);
                            JArray result = JArray.Parse(gridData.ToString());
                            foreach (JObject jcomp in result)
                            {
                                if (jcomp.Count > 0)
                                {
                                    jcomp.Remove("ParentId");                                   
                                    var newProperty = new JProperty("ParentId", id);
                                    jcomp.Add(newProperty);
                                    //jcomp.SelectToken("ParentId").Replace(JToken.FromObject(id));
                                    var record = await CreateForm(jcomp.ToString(), null, child.Name);
                                    //jcomp.SelectToken("Id").Replace(JToken.FromObject(record.Item2));
                                }

                            }
                            //var query = @$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ""{child.Name}""='{result}'
                            //                    where ""Id""='{id}' ";

                            //await _cmsQueryBusiness.TableQueryExecute(query);
                        }

                    }
                }
                return await Task.FromResult(new Tuple<bool, string>(true, id));
            }
            return await Task.FromResult(new Tuple<bool, string>(false, "Page does not exist."));
        }

        private async Task ManageLocalization(Dictionary<string, object> rowData, string id)
        {
            var obj = Convert.ToString(rowData.GetValueOrDefault("Localization"));
            var localizedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
            if (localizedData != null && localizedData.Any())
            {
                string hindi = null;
                if (localizedData.ContainsKey("Hindi"))
                {
                    hindi = Convert.ToString(localizedData["Hindi"]);
                }
                string arabic = null;
                if (localizedData.ContainsKey("Arabic"))
                {
                    arabic = Convert.ToString(localizedData["Arabic"]);
                }
                var exist = await _repo.GetSingle<FormResourceLanguageViewModel, FormResourceLanguage>(x => x.FormTableId == id);
                if (exist == null)
                {
                    await _repo.Create<FormResourceLanguageViewModel, FormResourceLanguage>(new FormResourceLanguageViewModel
                    {
                        Hindi = hindi,
                        Arabic = arabic,
                        FormTableId = id
                    });
                }
                else
                {
                    if (hindi != null)
                    {
                        exist.Hindi = hindi;
                    }
                    if (arabic != null)
                    {
                        exist.Arabic = arabic;
                    }
                    await _repo.Edit<FormResourceLanguage, FormResourceLanguage>(exist);
                }
            }
        }

        private async Task<string> ValidateForm(string data, string pageId, Dictionary<string, object> rowData, TableMetadataViewModel tableMetaData, List<ColumnMetadataViewModel> tableColumns, string exculdeId = null)
        {
            var uniqueColumns = tableColumns.Where(x => x.IsUniqueColumn).ToList();
            foreach (var item in uniqueColumns)
            {
                var value = rowData.GetValueOrDefault(item.Name);
                var exist = await GetDataByColumn(item, value, tableMetaData, exculdeId);
                if (exist.IsNotNullAndNotEmpty())
                {
                    return await Task.FromResult(@$"An item already exists with '{item.Alias}' as '{value}'. Please enter another value");
                }
            }
            return string.Empty;
        }


        public async Task<Tuple<bool, string>> EditForm(string recordId, string data, string pageId, string templateCode = null)
        {
            Template template = null;
            if (templateCode.IsNotNullAndNotEmpty())
            {
                template = await _repo.GetSingle(x => x.Code == templateCode);
            }
            else
            {
                var page = await _pageBusiness.GetPageForExecution(pageId);
                if (page != null)
                {
                    template = page.Template;
                }
            }
            //var page = await _pageBusiness.GetPageForExecution(pageId);
            //if (page != null && page.Template != null)
            if (template != null)
            {
                var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == template.TableMetadataId);
                if (tableMetaData != null)
                {
                    var tableColumns = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == tableMetaData.Id);
                    if (tableColumns != null && tableColumns.Count > 0)
                    {
                        var validate = await ValidateForm(data, pageId, rowData, tableMetaData, tableColumns, recordId);
                        if (validate.IsNotNullAndNotEmpty())
                        {
                            return await Task.FromResult(new Tuple<bool, string>(false, validate));
                        }
                        var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}""");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append("set");
                        selectQuery.Append(Environment.NewLine);
                        var columnKeys = new List<string>();
                        foreach (var col in tableColumns.Where(x=>x.IsUdfColumn && x.UdfUIType != UdfUITypeEnum.editgrid && x.UdfUIType != UdfUITypeEnum.datagrid))
                        {
                            columnKeys.Add(@$"""{ col.Name}"" = {BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType)}");
                        }
                        columnKeys.Add(@$"""LastUpdatedBy"" = '{_userContext.UserId}'");
                        columnKeys.Add(@$"""LastUpdatedDate"" = '{DateTime.Now.ToDatabaseDateFormat()}'");
                        selectQuery.Append($"{string.Join(",", columnKeys)}");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append(@$"where  ""Id"" = '{recordId}'");
                        var queryText = selectQuery.ToString();

                        //await _queryRepo.ExecuteCommand(queryText, null);
                        await _cmsQueryBusiness.TableQueryExecute(queryText);
                        await ManageLocalization(rowData, recordId);

                        var childTable = tableColumns.Where(x => x.UdfUIType == UdfUITypeEnum.editgrid || x.UdfUIType == UdfUITypeEnum.datagrid).ToList();
                        foreach (var child in childTable)
                        {
                            var gridData = rowData.GetValueOrDefault(child.Name);
                            if (gridData != null)
                            {
                                //JArray result = JArray.FromObject(gridData);
                                JArray result = JArray.Parse(gridData.ToString());
                                var list = JsonConvert.DeserializeObject<IList<IdNameViewModel>>(gridData.ToString());
                                var ids = list.Select(x => x.Id).ToList();
                                var tableName = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Code == child.Name);
                                var delRec = await GetData("cms", tableName.Name, "ParentId", recordId);
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
                                        var dataId = jcomp.GetValue("Id");
                                        if (dataId.IsNotNull() && dataId.ToString().IsNotNullAndNotEmptyAndNotValue("null"))
                                        {
                                            await EditForm(dataId.ToString(), jcomp.ToString(), null, child.Name);
                                        }
                                        else
                                        {
                                            jcomp.Remove("ParentId");                                           
                                            var newProperty = new JProperty("ParentId", recordId);
                                            jcomp.Add(newProperty);
                                            //jcomp.SelectToken("ParentId").Replace(JToken.FromObject(recordId));
                                            await CreateForm(jcomp.ToString(), null, child.Name);

                                        }
                                    }

                                }
                                //var query = @$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ""{child.Name}""='{result}'
                                //                where ""Id""='{recordId}' ";

                                //await _cmsQueryBusiness.TableQueryExecute(query);
                            }
                        }

                        return await Task.FromResult(new Tuple<bool, string>(true, recordId));

                    }

                }
            }
            return await Task.FromResult(new Tuple<bool, string>(false, "Page does not exist."));
        }

        public async Task<IList<EmailTaskViewModel>> ReadEmailTaskData(string refId, ReferenceTypeEnum refType)
        {
            var taskdetails = new TaskViewModel();
            if (refType == ReferenceTypeEnum.NTS_Task)
            {
                taskdetails = await _taskBusiness.GetSingle(x => x.Id == refId);
            }

            //var queryData1 = await  _queryRepoEmailTask.ExecuteQueryList(query1, null);
            var queryData1 = await _cmsQueryBusiness.ReadEmailTaskData(refId, refType, taskdetails);
            return queryData1;
        }

        public async Task<IList<IdNameViewModel>> GetActivePersonList()
        {
            var list = await _cmsQueryBusiness.GetActivePersonList();
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetPayrollElementList()
        {
            var list = await _cmsQueryBusiness.GetPayrollElementList();
            return list;
        }

        public async Task<TestViewModel> Test()
        {
            var result = await _cmsQueryBusiness.Test();
            return result;
        }

        public async Task<IList<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userRoleCodes, string inboxCode)
        {
            var list = await _cmsQueryBusiness.GetInboxTreeviewList(id, type, parentId, userRoleId, userId, userRoleIds, stageName, stageId, batchId, expandingList, userRoleCodes, inboxCode);
            return list;
        }

        public async Task<FormTemplateViewModel> GetFormDetails(FormTemplateViewModel viewModel)
        {
            var template = await _repo.GetSingle<TemplateViewModel, Template>(x => x.Id == viewModel.TemplateId);
            if (template != null)
            {
                viewModel.TableMetadataId = template.TableMetadataId;
            }
            viewModel.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == viewModel.TableMetadataId

                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);

            GetFormUdfDetails(viewModel);
            //will add ntsId to session varibale here
            return viewModel;
        }


        public void GetFormUdfDetails(FormTemplateViewModel model)
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
        private void ChildComp(JArray comps, List<ColumnMetadataViewModel> Columns, FormTemplateViewModel model)
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
                                    ///columnMeta.ActiveUserType = model.ActiveUserType;
                                    //columnMeta.NtsStatusCode = model.NoteStatusCode;
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
                                    //if (!columnMeta.IsVisible)
                                    //{
                                    //    var hiddenproperty = jcomp.SelectToken("hidden");
                                    //    if (hiddenproperty == null)
                                    //    {
                                    //        var newProperty = new JProperty("hidden", true);
                                    //        jcomp.Add(newProperty);
                                    //    }

                                    //}
                                    //if (!columnMeta.IsEditable || isReadonly)
                                    //{
                                    //    var newhiddenproperty = jcomp.SelectToken("disabled");
                                    //    if (newhiddenproperty == null)
                                    //    {
                                    //        var newProperty = new JProperty("disabled", true);
                                    //        jcomp.Add(newProperty);
                                    //    }
                                    //}

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

        public async Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string templateCode)
        {
            var list = await _cmsQueryBusiness.GetInboxMenuItem(id, type, templateCode);
            return list;
        }

        //public async Task<List<TaskTemplateViewModel>> ReadInboxData(string id, string type, string templateCode)
        //{
        //    var query = "";
        //    var res = new List<TaskTemplateViewModel>();
        //    if (type == "INBOX" || type== "SERVICETEMPLATE")
        //    {
        //        query = $@"select st.*
        //                    from public.""NtsService"" as s 
        //                    left join public.""Template"" as t on t.""Code"" = s.""TemplateCode""
        //                    left join public.""NtsService"" as ch on ch.""ServicePlusId"" = s.""Id""
        //                    left join public.""NtsTask"" as st on st.""ParentServiceId"" = ch.""Id"" 
        //                    where s.""TemplateCode"" in ('{templateCode}')
        //                    group by st.""Id""";
        //        res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);
        //    }
        //    else if (type == "SERVICEPLUS")
        //    {
        //        query = $@"select st.*
        //                    from 
        //                    public.""NtsService"" as ch
        //                    left join public.""NtsTask"" as st on st.""ParentServiceId"" = ch.""Id"" 
        //                     where ch.""ServicePlusId"" = '{id}'
        //                     group by st.""Id"" ";
        //        res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);

        //    }
        //    else if (type == "SERVICE")
        //    {
        //        query = $@"select st.*
        //        from
        //        public.""NtsTask"" as st where st.""ParentServiceId"" = '{id}'
        //         group by st.""Id"" ";
        //        res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);

        //    }
        //    else if (type == "STEPTASK")
        //    {
        //        query = $@"select st.*
        //        from
        //        public.""NtsTask"" as st where st.""Id"" = '{id}'
        //         group by st.""Id"" ";
        //        res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);

        //    }
        //        return res.Where(x=>x.Id.IsNotNullAndNotEmpty()).ToList();
        //}

        public async Task<List<TaskTemplateViewModel>> ReadInboxData(string id, string type, string templateCode, string userId = null)
        {
            var list = await _cmsQueryBusiness.ReadInboxData(id, type, templateCode, userId);
            return list;
        }
        public async Task<IList<TASTreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var list = await _cmsQueryBusiness.GetInboxMenuItem(id, type, parentId, userRoleId, userId, userRoleIds, stageName, stageId, projectId, expandingList, userroleCode);
            return list;
        }
        public async Task<IList<TASTreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var list = await _cmsQueryBusiness.GetInboxMenuItemByUser(id, type, parentId, userRoleId, userId, userRoleIds, stageName, stageId, projectId, expandingList, userroleCode);
            return list;
        }

        public async Task<string> GetLatestMigrationScript()
        {
            var queryData = await _cmsQueryBusiness.GetLatestMigrationScript();
            return queryData;
        }

        public async Task<List<string>> GetAllMigrationsList()
        {
            var queryData = await _cmsQueryBusiness.GetAllMigrationsList();
            return queryData;
        }

        public async Task<List<TemplateViewModel>> GetTemplateListByTemplateType(int i)
        {
            var queryData = await _cmsQueryBusiness.GetTemplateListByTemplateType(i);
            return queryData;
        }

        public async Task<string> ExecuteMigrationScript(string script)
        {
            var error = "Query Execution Success";
            try
            {
                var exScript = script;
                //var exScript = script.Replace("\"","\"\"");
                var query = $@" " + exScript + " ";
                //var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
                var queryData = await _cmsQueryBusiness.ExecuteMigrationScript(script);
                return error;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return ex.ToString();
            }

        }

        public async Task<List<IdNameViewModel>> GetCSCOfficeType(string templateCode)
        {
            var queryData = await _cmsQueryBusiness.GetCSCOfficeType(templateCode);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetCSCSubfficeType(string officeId, string districtId)
        {
            var queryData = await _cmsQueryBusiness.GetCSCSubfficeType(officeId, districtId);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetRevenueVillage(string officeId, string subDistrictId)
        {
            var queryData = await _cmsQueryBusiness.GetRevenueVillage(officeId, subDistrictId);
            return queryData;
        }
        public async Task<List<DynamicGridViewModel>> GetDataGridValue(string parentId)
        {
            return await _cmsQueryBusiness.GetDataGridValue(parentId);
        }

        public async Task<List<TemplateViewModel>> GetStepTaskTemplateListData(string serviceTemplateId)
        {
            var queryData = await _cmsQueryBusiness.GetStepTaskTemplateListData(serviceTemplateId);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetStepTaskTemplateList(string serviceTemplateId)
        {
            var queryData = await _cmsQueryBusiness.GetStepTaskTemplateList(serviceTemplateId);
            return queryData;
        }

        // ---------------------------------------------- COPY METHODS ------------------------------------------
        public async Task<CommandResult<ServiceTemplateViewModel>> CopyServiceTemplate(ServiceTemplateViewModel oldModel, string newTempId, CopyTemplateViewModel copyModel = null)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _serviceTemplateBusiness = _serviceProvider.GetService<IServiceTemplateBusiness>();

            TemplateViewModel oldTempModel = new();
            if (copyModel.IsNotNull())
            {
                oldTempModel = copyModel.Template;
            }
            else
            {
                oldTempModel = await _templateBusiness.GetSingleById(oldModel.TemplateId);
            }

            var newModel = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);
            ServiceTemplateViewModel model = new();

            model = newModel;
            newModel = oldModel;
            newModel.Id = model.Id;
            newModel.TemplateId = model.TemplateId;
            newModel.UdfTableMetadataId = model.UdfTableMetadataId;
            newModel.UdfTemplateId = model.UdfTemplateId;
            newModel.ServiceIndexPageTemplateId = model.ServiceIndexPageTemplateId;

            var json2 = Helper.ReplaceJsonProperty(oldTempModel.Json, "columnMetadataId");

            newModel.Json = json2;

            var serviceResult = await _serviceTemplateBusiness.Edit(newModel);
            return serviceResult;

        }

        public async Task<CommandResult<ServiceIndexPageTemplateViewModel>> CopyServiceTempltaeIndexPageData(ServiceIndexPageTemplateViewModel model, string newTempId, CopyTemplateViewModel copyModel = null)
        {
            var _serviceIndexPageTemplateBusiness = _serviceProvider.GetService<IServiceIndexPageTemplateBusiness>();
            var _serviceIndexPageColumnBusiness = _serviceProvider.GetService<IServiceIndexPageColumnBusiness>();


            var newServiceIndexModel = await _serviceIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);

            var newTempModel = await _repo.GetSingleById<TemplateViewModel, Template>(newTempId);
            List<ServiceIndexPageColumnViewModel> rows = new();

            if (copyModel.IsNotNull())
            {
                rows = copyModel.ServiceIndexPageColumn;
            }
            else
            {
                rows = await _serviceIndexPageColumnBusiness.GetList(x => x.ServiceIndexPageTemplateId == model.Id);
            }
            var newColumnMetaData = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == newTempModel.UdfTableMetadataId);

            foreach (var i in rows)
            {
                i.DataAction = DataActionEnum.Create;
                i.Id = null;
                i.Select = true;
                i.ServiceIndexPageTemplateId = newServiceIndexModel.Id;
                if (i.IsForeignKeyTableColumn == false)
                {
                    var column = newColumnMetaData.SingleOrDefault(x => x.Alias == i.ColumnName);
                    i.ColumnMetadataId = column.Id;
                }
                else
                {
                    i.ColumnMetadataId = null;

                }
            }
            model.SelectedTableRows = rows;

            var existingModel = newServiceIndexModel;
            newServiceIndexModel = model;

            newServiceIndexModel.Id = existingModel.Id;
            newServiceIndexModel.TemplateId = existingModel.TemplateId;
            newServiceIndexModel.CreatedDate = existingModel.CreatedDate;
            newServiceIndexModel.CreatedBy = existingModel.CreatedBy;
            newServiceIndexModel.LastUpdatedDate = existingModel.LastUpdatedDate;
            newServiceIndexModel.LastUpdatedBy = existingModel.LastUpdatedBy;
            newServiceIndexModel.CompanyId = existingModel.CompanyId;
            newServiceIndexModel.LegalEntityId = existingModel.LegalEntityId;
            newServiceIndexModel.SelectedTableRows = model.SelectedTableRows;

            var result = await _serviceIndexPageTemplateBusiness.Edit(newServiceIndexModel);
            return result;



        }

        public async Task<CommandResult<PageTemplateViewModel>> CopyPageTemplate(PageTemplateViewModel oldModel, string newTempId)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _pageTemlateBusiness = _serviceProvider.GetService<IPageTemplateBusiness>();

            var oldTempData = await _templateBusiness.GetSingleById(oldModel.TemplateId);
            //var oldModel = await _pageTemlateBusiness.GetSingle(x => x.TemplateId == oldTempId);
            var newModel = await _pageTemlateBusiness.GetSingle(x => x.TemplateId == newTempId);

            newModel.Json = oldTempData.Json;
            newModel.SequenceOrder = oldModel.SequenceOrder;

            var pageResult = await _pageTemlateBusiness.Edit(newModel);
            return pageResult;
        }

        public async Task<CommandResult<FormTemplateViewModel>> CopyFormTemplate(FormTemplateViewModel oldModel, string newTempId)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _formTemplateBusiness = _serviceProvider.GetService<IFormTemplateBusiness>();

            var oldTempData = await _templateBusiness.GetSingleById(oldModel.TemplateId);
            //var oldModel = await _formTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
            var newModel = await _formTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);
            FormTemplateViewModel model = new();

            model = newModel;
            newModel = oldModel;

            newModel.Id = model.Id;
            newModel.TemplateId = model.TemplateId;
            newModel.IndexPageTemplateId = model.IndexPageTemplateId;
            newModel.CreatedDate = model.CreatedDate;
            newModel.CreatedBy = model.CreatedBy;
            newModel.LastUpdatedDate = model.LastUpdatedDate;
            newModel.LastUpdatedBy = model.LastUpdatedBy;
            newModel.CompanyId = model.CompanyId;
            newModel.LegalEntityId = model.LegalEntityId;

            var json = Helper.ReplaceJsonProperty(oldTempData.Json, "columnMetadataId");

            newModel.Json = json;

            var formresult = await _formTemplateBusiness.Edit(newModel);
            return formresult;
        }

        public async Task<CommandResult<FormIndexPageTemplateViewModel>> CopyFormTemplateIndexPageData(FormIndexPageTemplateViewModel model, string newTempId)
        {
            var _formIndexPageTemplateBusiness = _serviceProvider.GetService<IFormIndexPageTemplateBusiness>();
            var _formIndexPageColumnBusiness = _serviceProvider.GetService<IFormIndexPageColumnBusiness>();

            var newFormIndexModel = await _formIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);
            var rows = await _formIndexPageColumnBusiness.GetList(x => x.FormIndexPageTemplateId == model.Id);
            foreach (var i in rows)
            {
                i.DataAction = DataActionEnum.Create;
                i.Id = null;
                i.Select = true;
                i.FormIndexPageTemplateId = newFormIndexModel.Id;
            }
            model.SelectedTableRows = rows;

            var existingModel = newFormIndexModel;
            newFormIndexModel = model;

            newFormIndexModel.Id = existingModel.Id;
            newFormIndexModel.TemplateId = existingModel.TemplateId;
            newFormIndexModel.CreatedDate = existingModel.CreatedDate;
            newFormIndexModel.CreatedBy = existingModel.CreatedBy;
            newFormIndexModel.LastUpdatedDate = existingModel.LastUpdatedDate;
            newFormIndexModel.LastUpdatedBy = existingModel.LastUpdatedBy;
            newFormIndexModel.CompanyId = existingModel.CompanyId;
            newFormIndexModel.LegalEntityId = existingModel.LegalEntityId;
            newFormIndexModel.SelectedTableRows = model.SelectedTableRows;

            var result = await _formIndexPageTemplateBusiness.Edit(newFormIndexModel);
            return result;
        }

        public async Task<CommandResult<NoteTemplateViewModel>> CopyNoteTemplate(NoteTemplateViewModel oldModel, string newTempId)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _noteTemplateBusiness = _serviceProvider.GetService<INoteTemplateBusiness>();

            var oldTempData = await _templateBusiness.GetSingleById(oldModel.TemplateId);
            //var oldModel = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
            var newModel = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);
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


            var noteresult = await _noteTemplateBusiness.Edit(newModel);
            return noteresult;

        }

        public async Task<CommandResult<NoteIndexPageTemplateViewModel>> CopyNoteTempltaeIndexPageData(NoteIndexPageTemplateViewModel model, string newTempId)
        {
            var _noteIndexPageTemplateBusiness = _serviceProvider.GetService<INoteIndexPageTemplateBusiness>();
            var _noteindexPageColumnBusiness = _serviceProvider.GetService<INoteIndexPageColumnBusiness>();

            var newNoteIndexModel = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);

            var rows = await _noteindexPageColumnBusiness.GetList(x => x.NoteIndexPageTemplateId == model.Id);
            foreach (var i in rows)
            {
                i.DataAction = DataActionEnum.Create;
                i.Id = null;
                i.Select = true;
                i.NoteIndexPageTemplateId = newNoteIndexModel.Id;
            }
            model.SelectedTableRows = rows;

            var existingModel = newNoteIndexModel;
            newNoteIndexModel = model;

            newNoteIndexModel.Id = existingModel.Id;
            newNoteIndexModel.TemplateId = existingModel.TemplateId;
            newNoteIndexModel.CreatedDate = existingModel.CreatedDate;
            newNoteIndexModel.CreatedBy = existingModel.CreatedBy;
            newNoteIndexModel.LastUpdatedDate = existingModel.LastUpdatedDate;
            newNoteIndexModel.LastUpdatedBy = existingModel.LastUpdatedBy;
            newNoteIndexModel.CompanyId = existingModel.CompanyId;
            newNoteIndexModel.LegalEntityId = existingModel.LegalEntityId;

            var result = await _noteIndexPageTemplateBusiness.Edit(newNoteIndexModel);
            return result;

        }

        public async Task<CommandResult<TaskTemplateViewModel>> CopyTaskTemplate(TaskTemplateViewModel oldModel, string newTempId)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _taskTemplateBusiness = _serviceProvider.GetService<ITaskTemplateBusiness>();

            var oldTempData = await _templateBusiness.GetSingleById(oldModel.TemplateId);
            //var oldModel = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
            var newModel = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);
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

            var json = Helper.ReplaceJsonProperty(oldTempData.Json, "columnMetadataId");

            newModel.Json = json;
            var taskresult = await _taskTemplateBusiness.Edit(newModel);
            return taskresult;
        }

        public async Task<CommandResult<TaskIndexPageTemplateViewModel>> CopyTaskTempltaeIndexPageData(TaskIndexPageTemplateViewModel model, string newTempId)
        {
            var _taskIndexPageTemplateBusiness = _serviceProvider.GetService<ITaskIndexPageTemplateBusiness>();
            var _taskindexPageColumnBusiness = _serviceProvider.GetService<ITaskIndexPageColumnBusiness>();

            var newTaskIndexModel = await _taskIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == newTempId);

            var rows = await _taskindexPageColumnBusiness.GetList(x => x.TaskIndexPageTemplateId == model.Id);
            foreach (var i in rows)
            {
                i.DataAction = DataActionEnum.Create;
                i.Id = null;
                i.Select = true;
                i.TaskIndexPageTemplateId = newTaskIndexModel.Id;
            }
            model.SelectedTableRows = rows;

            var existingModel = newTaskIndexModel;
            newTaskIndexModel = model;

            newTaskIndexModel.Id = existingModel.Id;
            newTaskIndexModel.TemplateId = existingModel.TemplateId;
            newTaskIndexModel.CreatedDate = existingModel.CreatedDate;
            newTaskIndexModel.CreatedBy = existingModel.CreatedBy;
            newTaskIndexModel.LastUpdatedDate = existingModel.LastUpdatedDate;
            newTaskIndexModel.LastUpdatedBy = existingModel.LastUpdatedBy;
            newTaskIndexModel.CompanyId = existingModel.CompanyId;
            newTaskIndexModel.LegalEntityId = existingModel.LegalEntityId;


            var result = await _taskIndexPageTemplateBusiness.Edit(newTaskIndexModel);
            return result;

        }

        public async Task<bool> CopyLanguage(List<ResourceLanguageViewModel> items, string newTemplateId)
        {
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            foreach (var item in items)
            {
                item.Id = null;
                item.TemplateId = newTemplateId;

                if (item.Id.IsNullOrEmpty())
                {
                    await _resourceLanguageBusiness.Create(item);
                }
                else
                {
                    await _resourceLanguageBusiness.Edit(item);
                }
            }
            return true;
        }

        public async Task<List<dynamic>> CopyProcessDesign(List<ProcessDesignViewModel> list, string newTemplateId)
        {
            var _processDesignTemplateBusiness = _serviceProvider.GetService<IProcessDesignBusiness>();
            List<dynamic> pdList = new();
            var newPDList = await _processDesignTemplateBusiness.GetList(x => x.TemplateId == newTemplateId);
            if (newPDList.IsNotNull())
            {
                var i = 0;
                foreach (var item in newPDList)
                {
                    var x = new { OldPdId = list[i].Id, NewPdId = item.Id };
                    pdList.Add(x);
                    i++;
                }
            }
            else
            {
                var i = 0;
                foreach (var item in list)
                {
                    ProcessDesignViewModel pdItem = new();
                    pdItem.Id = null;
                    pdItem.TemplateId = newTemplateId;
                    pdItem.Name = item.Name;
                    pdItem.SequenceOrder = item.SequenceOrder;
                    pdItem.Status = item.Status;
                    pdItem.VersionNo = item.VersionNo;
                    pdItem.ProcessDesignHtml = item.ProcessDesignHtml;
                    pdItem.ActionId = item.ActionId;
                    pdItem.BusinessLogicExecutionType = item.BusinessLogicExecutionType;
                    pdItem.ProcessDesignType = item.ProcessDesignType;
                    pdItem.PortalId = item.PortalId;
                    var res = await _processDesignTemplateBusiness.Create(pdItem);
                    var x = new { OldPdId = list[i].Id, NewPdId = res.Item.Id };
                    pdList.Add(x);
                    i++;
                }
            }
            return pdList;
        }

        public async Task<List<dynamic>> CopyComponents(string oldPdId, string newPdId, List<ComponentViewModel> oldList)
        {
            var componentBusiness = _serviceProvider.GetService<IComponentBusiness>();

            List<dynamic> compList = new();

            var oldComponentsList = oldList.Where(x => x.ProcessDesignId == oldPdId).ToList();

            // Parent Component Created
            var compItem = oldComponentsList.SingleOrDefault(x => x.ParentId == null);
            var newComp = _autoMapper.Map<ComponentViewModel>(compItem);
            newComp.Id = null;
            newComp.ProcessDesignId = newPdId;
            newComp.ParentId = null;
            var compRes = await componentBusiness.Create(newComp);

            // Parent List Created
            var x = new { OldParentId = compItem.Id, NewParentId = compRes.Item.Id };
            compList.Add(x);
            var i = 0;

            while (i < compList.Count)
            {
                string oldcompid = compList[i].OldParentId.ToString();
                string newcompid = compList[i].NewParentId.ToString();

                var temp = oldComponentsList.Where(x => x.ParentId == oldcompid);
                foreach (var t in temp)
                {
                    var newComp1 = _autoMapper.Map<ComponentViewModel>(t);
                    newComp1.Id = null;
                    newComp1.ProcessDesignId = newPdId;
                    newComp1.ParentId = newcompid;
                    var compRes1 = await componentBusiness.Create(newComp1);
                    x = new { OldParentId = t.Id, NewParentId = compRes1.Item.Id };
                    compList.Add(x);
                }
                i++;
            }

            return compList;
        }

        public async Task<List<dynamic>> CreateStepTask(List<TemplateViewModel> oldStepTaskList, string jsonStr)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            List<dynamic> StepTaskIdList = new();
            dynamic json = JsonConvert.DeserializeObject(jsonStr);
            foreach (var item in json)
            {
                string x = item["Id"];
                var idx = oldStepTaskList.FindIndex(a => a.Id == x);
                TemplateViewModel newTemp = new();
                newTemp = oldStepTaskList[idx];
                newTemp.Id = null;
                newTemp.Name = item["New Name"];
                newTemp.DisplayName = item["New Display Name"];
                newTemp.Code = item["New Code"];

                newTemp.TaskType = TaskTypeEnum.StepTask;
                var result = await _templateBusiness.Create(newTemp);
                if (result.IsSuccess)
                {
                    var i = new { OldStepTaskId = item["Id"], NewStepTaskId = result.Item.Id };
                    StepTaskIdList.Add(i);
                    var res = await CopyTask(x, result.Item.Id);
                    //// fetch old task template data 
                    //var tasktemplate = await _templateBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(i => i.TemplateId == x);
                    //var copytasktempres = await CopyTaskTemplate(tasktemplate, result.Item.Id);
                    //if (copytasktempres.IsSuccess)
                    //{
                    //    // Index
                    //    var oldTaskIndexModel = await _templateBusiness.GetSingle<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate>(i => i.TemplateId == x);
                    //    var taskIndexResult = await CopyTaskTempltaeIndexPageData(oldTaskIndexModel, result.Item.Id); // done
                    //    if (taskIndexResult.IsSuccess)
                    //    {
                    //        // Language
                    //        var oldLanguageList = await _templateBusiness.GetList<ResourceLanguageViewModel, ResourceLanguage>(i => i.TemplateId == x);
                    //        var languageResult = await CopyLanguage(oldLanguageList, result.Item.Id); // done
                    //    }

                    //}

                }
                else
                {

                    //return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }


            }
            return StepTaskIdList;
        }

        public async Task<List<dynamic>> CopyStepTaskComponent(List<dynamic> compList, List<dynamic> stepTaskIdList, string newServiceId, string oldServiceId, List<StepTaskComponentViewModel> oldStepTaskCompList, CopyTemplateViewModel copyModel = null)
        {
            var _stepTaskComponentBusiness = _serviceProvider.GetService<IStepTaskComponentBusiness>();

            List<dynamic> StepCompIdList = new();

            foreach (var comp in compList)
            {
                string oldId = comp.OldParentId.ToString();

                var stepTaskComp = oldStepTaskCompList.SingleOrDefault(x => x.ComponentId == oldId);

                if (stepTaskComp.IsNotNull())
                {
                    var oldStepTaskCompId = stepTaskComp.Id;

                    string newId = comp.NewParentId.ToString();
                    var NewServiceModel = await _stepTaskComponentBusiness.GetSingleById<TemplateViewModel, Template>(newServiceId);

                    var NewColumnMetadata = await _stepTaskComponentBusiness.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == NewServiceModel.UdfTableMetadataId);

                    var idx = stepTaskIdList.FindIndex(x => x.OldStepTaskId == stepTaskComp.TemplateId);
                    string oldStepTaskTempId = stepTaskIdList[idx].OldStepTaskId.ToString();
                    string newStepTaskTempId = stepTaskIdList[idx].NewStepTaskId.ToString();

                    var taskTempModel = await _stepTaskComponentBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == newStepTaskTempId);
                    var assineeType = await _stepTaskComponentBusiness.GetSingleById<LOVViewModel, LOV>(stepTaskComp.AssignedToTypeId);


                    var model = _autoMapper.Map<StepTaskComponentViewModel>(stepTaskComp);
                    model.Id = null;
                    model.ComponentId = newId;
                    model.UdfTemplateId = NewServiceModel.UdfTemplateId;
                    model.UdfTableMetadataId = NewServiceModel.UdfTableMetadataId;
                    model.TemplateId = newStepTaskTempId;
                    model.TaskTemplateId = taskTempModel.Id;
                    model.AssignedToTypeCode = assineeType.Code;

                    var result = await _stepTaskComponentBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        var x = new { OldStepTaskCompId = oldStepTaskCompId, NewStepTaskCompId = result.Item.Id };
                        StepCompIdList.Add(x);

                        // UpdateUDFs
                        List<UdfPermissionViewModel> oldUdfList = new();
                        TemplateViewModel oldServiceModel = new();
                        List<ColumnMetadataViewModel> oldColumnMetadataList = new();
                        if (!copyModel.IsNotNull())
                        {
                            oldUdfList = await _stepTaskComponentBusiness.GetList<UdfPermissionViewModel, UdfPermission>(x => x.TemplateId == oldStepTaskTempId);
                            oldServiceModel = await _stepTaskComponentBusiness.GetSingleById<TemplateViewModel, Template>(oldServiceId);
                            oldColumnMetadataList = await _stepTaskComponentBusiness.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == oldServiceModel.UdfTableMetadataId);
                        }
                        else
                        {
                            int index = copyModel.StepTaskTemplate.FindIndex(x => x.Id == oldStepTaskTempId);
                            var StepTaskDetails = copyModel.StepTaskDetails[index];
                            oldUdfList = StepTaskDetails.UdfPermission;
                            oldColumnMetadataList = StepTaskDetails.ColumnMetadata;
                        }
                        await UpdateUdfPermission(oldUdfList, NewColumnMetadata, newStepTaskTempId, oldColumnMetadataList);

                    }
                }
            }

            return StepCompIdList;
        }

        public async Task UpdateUdfPermission(List<UdfPermissionViewModel> oldUdfList, List<ColumnMetadataViewModel> columnMetadataList, string newStepTaskTempId, List<ColumnMetadataViewModel> oldColumnMetadataList)
        {
            var _UdfPermissionBusiness = _serviceProvider.GetService<IUdfPermissionBusiness>();

            foreach (var udf in oldUdfList)
            {
                var idx = oldColumnMetadataList.FindIndex(x => x.Id == udf.ColumnMetadataId);
                if (idx == -1)
                {
                    continue;
                }
                var alias = oldColumnMetadataList[idx].Alias;
                var newIdx = columnMetadataList.FindIndex(x => x.Alias == alias);

                udf.Id = null;
                udf.ColumnMetadataId = columnMetadataList[newIdx].Id;
                udf.TemplateId = newStepTaskTempId;
                await _UdfPermissionBusiness.Create(udf);

            }
        }

        public async Task<List<dynamic>> CopyDecisionScriptComponent(List<dynamic> ComponentList, List<DecisionScriptComponentViewModel> oldList)
        {
            List<dynamic> decisionScriptCompIds = new();
            foreach (var item in oldList)
            {
                var idx = ComponentList.FindIndex(x => x.OldParentId == item.ComponentId);
                string newId = ComponentList[idx].NewParentId.ToString();
                var model = _autoMapper.Map<DecisionScriptComponentViewModel>(item);
                model.Id = null;
                model.ComponentId = newId;
                var res = await _cmsQueryBusiness.Create<DecisionScriptComponentViewModel, DecisionScriptComponent>(model);
                if (res.IsSuccess)
                {
                    //var rule = await _cmsQueryBusiness.GetSingle<BusinessRuleModelViewModel, BusinessRuleModel>(i => i.DecisionScriptComponentId == res.Item.Id);
                    //var x = new { OldId = item.Id, NewId = res.Item.Id, BRMRootId = rule.Id };
                    var x = new { OldId = item.Id, NewId = res.Item.Id };
                    decisionScriptCompIds.Add(x);
                }
            }
            return decisionScriptCompIds;
        }

        public async Task<bool> CopyBusinessRuleModelForDecisionScript(List<BusinessRuleModelViewModel> oldList, List<dynamic> decisionScriptCompIds)
        {
            var _businessRuleModelBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            if (oldList.IsNotNull())
            {
                List<BusinessRuleModelViewModel> parentList = new();
                parentList = oldList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical).ToList();
                List<dynamic> parentIdList = new();
                foreach (var item in parentList)
                {
                    BusinessRuleModelViewModel model = new();
                    var idx = decisionScriptCompIds.FindIndex(x => x.OldId == item.DecisionScriptComponentId);
                    string newDSCId = decisionScriptCompIds[idx].NewId.ToString();
                    CommandResult<BusinessRuleModelViewModel> res;
                    //if (item.ParentId != null)
                    //{
                    // Create
                    model = _autoMapper.Map<BusinessRuleModelViewModel>(item);
                    model.Id = null;
                    model.DecisionScriptComponentId = newDSCId;
                    res = await _businessRuleModelBusiness.Create(model);
                    //}
                    //else
                    //{
                    // Edit
                    //string BRMId = decisionScriptCompIds[idx].BRMRootId.ToString();
                    //model = await _businessRuleModelBusiness.GetSingleById(BRMId);
                    //var newId = model.Id;
                    //model = _autoMapper.Map<BusinessRuleModelViewModel>(item);
                    //model.Id = newId;
                    //model.DecisionScriptComponentId = newDSCId;
                    //res = await _businessRuleModelBusiness.Edit(model);

                    //}

                    if (res.IsSuccess)
                    {
                        var x = new { OldBRMId = item.Id, NewBRMId = res.Item.Id };
                        parentIdList.Add(x);
                        List<BusinessRuleModelViewModel> childList = new();
                        childList = oldList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Operational && x.ParentId == item.Id).ToList();
                        foreach (var child in childList)
                        {

                            BusinessRuleModelViewModel childModel = new();
                            childModel = _autoMapper.Map<BusinessRuleModelViewModel>(child);
                            childModel.Id = null;
                            childModel.ParentId = res.Item.Id;
                            childModel.DecisionScriptComponentId = newDSCId;

                            var childRes = await _businessRuleModelBusiness.Create(childModel);
                            if (!childRes.IsSuccess)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                // Updating parent id for sub parents under root
                var subParentList = oldList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical && x.ParentId != null).ToList();
                foreach (var item in subParentList)
                {
                    // fetch entry
                    var idx = parentIdList.FindIndex(x => x.OldBRMId == item.Id);
                    string newId = parentIdList[idx].NewBRMId.ToString();

                    var subParentModel = await _businessRuleModelBusiness.GetSingleById(newId);

                    // fetch parent id
                    var i = parentIdList.FindIndex(x => x.OldBRMId == item.ParentId);
                    subParentModel.ParentId = parentIdList[i].NewBRMId.ToString();

                    await _businessRuleModelBusiness.Edit(subParentModel);
                }
                return true;
            }
            return true;
        }

        public async Task<List<dynamic>> CopyStepTaskLogic(List<StepTaskSkipLogicViewModel> oldStepTaskSkipLogic, List<dynamic> StepTaskCompIdList)
        {
            List<dynamic> skipLogicIds = new();
            foreach (var comp in oldStepTaskSkipLogic)
            {
                var idx = StepTaskCompIdList.FindIndex(x => x.OldStepTaskCompId == comp.StepTaskComponentId);
                var oldStepTaskCompId = StepTaskCompIdList[idx].OldStepTaskCompId.ToString();
                var newStepTaskCompId = StepTaskCompIdList[idx].NewStepTaskCompId.ToString();

                StepTaskSkipLogicViewModel model = new();
                model.PortalId = comp.PortalId;
                model.Status = comp.Status;
                model.VersionNo = comp.VersionNo;
                model.SequenceOrder = comp.SequenceOrder;
                model.SuccessResult = comp.SuccessResult;
                model.ExecutionLogic = comp.ExecutionLogic;
                model.ExecutionLogicDisplay = comp.ExecutionLogicDisplay;
                model.Name = comp.Name;
                model.StepTaskComponentId = newStepTaskCompId;

                var result = await _cmsQueryBusiness.Create<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(model);
                if (result.IsSuccess)
                {
                    var obj = new { OldId = comp.Id, NewId = result.Item.Id };
                    skipLogicIds.Add(obj);
                }
            }

            return skipLogicIds;
        }

        public async Task<bool> CopyBusinessRuleModelForLogics(List<BusinessRuleModelViewModel> oldBRModelList, List<dynamic> LogicIds)
        {
            var _businessRuleModelBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            if (oldBRModelList.IsNotNull())
            {
                List<BusinessRuleModelViewModel> parentList = new();
                parentList = oldBRModelList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical).ToList();
                List<dynamic> parentIdList = new();
                foreach (var item in parentList)
                {
                    var idx = LogicIds.FindIndex(x => x.OldId == item.BusinessRuleNodeId);
                    string newSkipLogicId = LogicIds[idx].NewId.ToString();
                    BusinessRuleModelViewModel model = new();
                    model = _autoMapper.Map<BusinessRuleModelViewModel>(item);
                    model.Id = null;
                    model.BusinessRuleNodeId = newSkipLogicId;

                    var result = await _businessRuleModelBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        var x = new { OldBRMId = item.Id, NewBRMId = result.Item.Id };
                        parentIdList.Add(x);
                        List<BusinessRuleModelViewModel> childList = new();
                        childList = oldBRModelList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Operational && x.ParentId == item.Id).ToList();
                        foreach (var child in childList)
                        {

                            BusinessRuleModelViewModel childModel = new();
                            childModel = _autoMapper.Map<BusinessRuleModelViewModel>(child);
                            childModel.Id = null;
                            childModel.BusinessRuleNodeId = newSkipLogicId;
                            childModel.ReferenceId = newSkipLogicId;
                            childModel.ParentId = result.Item.Id;

                            var childRes = await _businessRuleModelBusiness.Create(childModel);
                            if (!childRes.IsSuccess)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                // Updating parent id for sub parents under root
                var subParentList = oldBRModelList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical && x.ParentId != null).ToList();
                foreach (var item in subParentList)
                {
                    var idx = parentIdList.FindIndex(x => x.OldBRMId == item.Id);
                    string newId = parentIdList[idx].NewBRMId.ToString();
                    var subParentModel = await _businessRuleModelBusiness.GetSingleById(newId);
                    var i = parentIdList.FindIndex(x => x.OldBRMId == item.ParentId);
                    subParentModel.ParentId = parentIdList[i].NewBRMId.ToString();
                    await _businessRuleModelBusiness.Edit(subParentModel);
                }
                return true;
            }
            return true;
        }

        public async Task<List<dynamic>> CopyStepTaskAssignee(List<StepTaskAssigneeLogicViewModel> oldStepTaskAssigneeLogic, List<dynamic> StepTaskCompIdList)
        {
            List<dynamic> assigneeLogicIds = new();
            foreach (var comp in oldStepTaskAssigneeLogic)
            {
                var idx = StepTaskCompIdList.FindIndex(x => x.OldStepTaskCompId == comp.StepTaskComponentId);
                var oldStepTaskCompId = StepTaskCompIdList[idx].OldStepTaskCompId.ToString();
                var newStepTaskCompId = StepTaskCompIdList[idx].NewStepTaskCompId.ToString();

                StepTaskAssigneeLogicViewModel model = new();
                model.PortalId = comp.PortalId;
                model.Status = comp.Status;
                model.VersionNo = comp.VersionNo;
                model.SequenceOrder = comp.SequenceOrder;
                model.SuccessResult = comp.SuccessResult;
                model.ExecutionLogic = comp.ExecutionLogic;
                model.ExecutionLogicDisplay = comp.ExecutionLogicDisplay;
                model.Name = comp.Name;
                model.StepTaskComponentId = newStepTaskCompId;
                model.AssignedToTypeId = comp.AssignedToTypeId;
                model.AssignedToUserId = comp.AssignedToUserId;
                model.AssignedToTeamId = comp.AssignedToTeamId;
                model.AssignedToHierarchyMasterId = comp.AssignedToHierarchyMasterId;
                model.AssignedToHierarchyMasterLevelId = comp.AssignedToHierarchyMasterLevelId;
                //model.TeamWorkAssignmentType = comp.TeamWorkAssignmentType;
                model.TeamAssignmentType = comp.TeamAssignmentType;

                var result = await _cmsQueryBusiness.Create<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(model);
                if (result.IsSuccess)
                {
                    var obj = new { OldId = comp.Id, NewId = result.Item.Id };
                    assigneeLogicIds.Add(obj);
                }
            }

            return assigneeLogicIds;
        }

        public async Task<List<BusinessRuleModelViewModel>> GetOldBusinessRuleModelDataForStepTaskAssigneeLogic(List<StepTaskAssigneeLogicViewModel> oldStepTaskAssigneeLogic)
        {
            List<BusinessRuleModelViewModel> list = new();
            foreach (var item in oldStepTaskAssigneeLogic)
            {
                string i = item.Id;
                var result = await _cmsQueryBusiness.GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.BusinessRuleNodeId == i);
                foreach (var res in result)
                {
                    list.Add(res);
                }

            }
            return list;
        }

        // Copying Business Rules for pre/post/preload scripts
        public async Task<List<dynamic>> CopyBusinessRules(List<BusinessRuleViewModel> oldList, string newServiceId)
        {
            List<dynamic> businessRuleIds = new();
            foreach (var item in oldList)
            {
                var model = _autoMapper.Map<BusinessRuleViewModel>(item);
                model.Id = null;
                model.TemplateId = newServiceId;
                var res = await _cmsQueryBusiness.Create<BusinessRuleViewModel, BusinessRule>(model);
                if (res.IsSuccess)
                {
                    var x = new { OldId = item.Id, NewId = res.Item.Id };
                    businessRuleIds.Add(x);
                }

            }
            return businessRuleIds;
        }

        // copying business rule nodes
        public async Task<List<dynamic>> CopyBusinessRuleNodes(List<dynamic> BRIds, List<BusinessRuleNodeViewModel> oldNodesList)
        {
            List<dynamic> nodeIds = new();
            foreach (var node in oldNodesList)
            {
                var idx = BRIds.FindIndex(x => x.OldId == node.BusinessRuleId);
                string newId = BRIds[idx].NewId.ToString();
                var model = _autoMapper.Map<BusinessRuleNodeViewModel>(node);
                model.Id = null;
                model.BusinessRuleId = newId;
                var res = await _cmsQueryBusiness.Create<BusinessRuleNodeViewModel, BusinessRuleNode>(model);
                if (res.IsSuccess)
                {
                    var x = new { OldId = node.Id, NewId = res.Item.Id };
                    nodeIds.Add(x);
                }
            }
            return nodeIds;
        }

        // Copying bre results
        public async Task<List<dynamic>> CopyBreResults(List<dynamic> nodeIds, List<BreResultViewModel> oldList)
        {
            List<dynamic> breResultIds = new();
            foreach (var item in oldList)
            {
                var idx = nodeIds.FindIndex(x => x.OldId == item.BusinessRuleNodeId);
                string newId = nodeIds[idx].NewId.ToString();
                var model = _autoMapper.Map<BreResultViewModel>(item);
                model.Id = null;
                model.BusinessRuleNodeId = newId;
                var res = await _cmsQueryBusiness.Create<BreResultViewModel, BreResult>(model);
                if (res.IsSuccess)
                {
                    var x = new { OldId = item.Id, NewId = res.Item.Id };
                    breResultIds.Add(x);
                }
            }
            return breResultIds;
        }

        // Copying business rule connector
        public async Task<bool> CopyBusinessRuleConnector(List<dynamic> BRIds, List<dynamic> nodeIds, List<BusinessRuleConnectorViewModel> oldList)
        {
            foreach (var item in oldList)
            {
                var BRIdx = BRIds.FindIndex(x => x.OldId == item.BusinessRuleId);
                string newBRId = BRIds[BRIdx].NewId.ToString();
                var srcIdx = nodeIds.FindIndex(x => x.OldId == item.SourceId);
                string newSrcId = nodeIds[srcIdx].NewId.ToString();
                var targetIdx = nodeIds.FindIndex(x => x.OldId == item.TargetId);
                string newTargetId = nodeIds[targetIdx].NewId.ToString();

                var model = _autoMapper.Map<BusinessRuleConnectorViewModel>(item);
                model.Id = null;
                model.BusinessRuleId = newBRId;
                model.SourceId = newSrcId;
                model.TargetId = newTargetId;

                var res = await _cmsQueryBusiness.Create<BusinessRuleConnectorViewModel, BusinessRuleConnector>(model);

                if (!res.IsSuccess)
                {
                    return false;
                }
            }
            return true;
        }

        // Copy step task scripts
        public async Task<bool> CopyStepTaskScripts(List<dynamic> stepTaskIds, List<TemplateViewModel> oldStepTaskList)
        {
            foreach (var item in stepTaskIds)
            {
                string oldStepTaskId = item.OldStepTaskId.ToString();
                string newStepTaskId = item.NewStepTaskId.ToString();
                var oldBusinessRuleList = await GetOldBusinessRuleList(oldStepTaskId);
                // copy business rule
                var brids = await CopyBusinessRules(oldBusinessRuleList, newStepTaskId);
                // fetch business rule nodes
                var oldBRNodeList = await GetOldBusinessRuleNodeList(oldBusinessRuleList);
                // copy business rule nodes
                var nodeids = await CopyBusinessRuleNodes(brids, oldBRNodeList);
                // fetch bre results
                var oldBreResultsList = await GetOldBreResultList(oldBRNodeList);
                // copy bre results
                var breIds = await CopyBreResults(nodeids, oldBreResultsList);
                // fetch business rule connectors
                var oldConnectorsList = await GetOldBRConnectorList(oldBusinessRuleList);
                // copy connectors
                var connectorResult = await CopyBusinessRuleConnector(brids, nodeids, oldConnectorsList);
                // fetch business decisions
                var oldBusinessDecisionList = await GetOldBRMListforBusinessDecision(oldBRNodeList);
                // copy business decisions
                var decisionResult = await CopyBusinessRuleModelForLogics(oldBusinessDecisionList, nodeids);

            }
            return true;
        }

        // ---------------------------------------------- GET METHODS ------------------------------------------
        public async Task<List<ComponentViewModel>> GetOldComponentsList(ProcessDesignViewModel PdList)
        {
            List<ComponentViewModel> list = new();
            list = await _cmsQueryBusiness.GetList<ComponentViewModel, Component>(x => x.ProcessDesignId == PdList.Id);
            return list;
        }

        public async Task<List<DecisionScriptComponentViewModel>> GetOldDecisionScriptComponentData(List<ComponentViewModel> ComponentList)
        {
            List<DecisionScriptComponentViewModel> list = new();
            foreach (var item in ComponentList)
            {
                string id = item.Id;
                var decisionComponent = await _cmsQueryBusiness.GetList<DecisionScriptComponentViewModel, DecisionScriptComponent>(x => x.ComponentId == id);
                if (decisionComponent.Count() != 0)
                {
                    foreach (var comp in decisionComponent)
                    {
                        list.Add(comp);
                    }
                }
            }
            return list;
        }

        public async Task<List<BusinessRuleModelViewModel>> GetOldBusinessRuleModelForDecisionScript(List<DecisionScriptComponentViewModel> decisionScriptComps)
        {
            List<BusinessRuleModelViewModel> list = new();
            foreach (var item in decisionScriptComps)
            {
                string id = item.Id;
                var itemList = await _cmsQueryBusiness.GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.DecisionScriptComponentId == id);
                foreach (var x in itemList)
                {
                    list.Add(x);
                }
            }
            return list;
        }


        public async Task<List<BusinessRuleModelViewModel>> GetOldBRMListforBusinessDecision(List<BusinessRuleNodeViewModel> nodes)
        {
            List<BusinessRuleModelViewModel> list = new();
            foreach (var item in nodes)
            {
                string i = item.Id;
                var result = await _cmsQueryBusiness.GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.BusinessRuleNodeId == i);
                foreach (var res in result)
                {
                    list.Add(res);
                }

            }
            return list;
        }

        public async Task<List<BusinessRuleViewModel>> GetOldBusinessRuleList(string oldServiceId)
        {
            var list = await _cmsQueryBusiness.GetList<BusinessRuleViewModel, BusinessRule>(x => x.TemplateId == oldServiceId);
            return list;
        }

        public async Task<List<BusinessRuleNodeViewModel>> GetOldBusinessRuleNodeList(List<BusinessRuleViewModel> oldBusinessRules)
        {
            List<BusinessRuleNodeViewModel> list = new();
            foreach (var item in oldBusinessRules)
            {
                var nodeList = await _cmsQueryBusiness.GetList<BusinessRuleNodeViewModel, BusinessRuleNode>(x => x.BusinessRuleId == item.Id);
                foreach (var node in nodeList)
                {
                    list.Add(node);
                }
            }
            return list;
        }

        public async Task<List<BreResultViewModel>> GetOldBreResultList(List<BusinessRuleNodeViewModel> nodes)
        {
            List<BreResultViewModel> list = new();
            foreach (var node in nodes)
            {
                var resultList = await _cmsQueryBusiness.GetList<BreResultViewModel, BreResult>(x => x.BusinessRuleNodeId == node.Id);
                foreach (var result in resultList)
                {
                    list.Add(result);
                }
            }
            return list;
        }

        public async Task<List<BusinessRuleConnectorViewModel>> GetOldBRConnectorList(List<BusinessRuleViewModel> businessRuleList)
        {
            List<BusinessRuleConnectorViewModel> list = new();
            foreach (var res in businessRuleList)
            {
                var connectorList = await _cmsQueryBusiness.GetList<BusinessRuleConnectorViewModel, BusinessRuleConnector>(x => x.BusinessRuleId == res.Id);
                foreach (var connector in connectorList)
                {
                    list.Add(connector);
                }
            }
            return list;
        }

        public async Task<List<StepTaskAssigneeLogicViewModel>> GetOldStepTaskAssigneeLogics(List<StepTaskComponentViewModel> StepTaskCompList)
        {
            List<StepTaskAssigneeLogicViewModel> list = new();
            foreach (var item in StepTaskCompList)
            {
                string i = item.Id;
                var skipList = await _cmsQueryBusiness.GetList<StepTaskAssigneeLogicViewModel, StepTaskAssigneeLogic>(x => x.StepTaskComponentId == i);
                foreach (var subItem in skipList)
                {
                    list.Add(subItem);
                }
            }
            return list;
        }

        public async Task<List<BusinessRuleModelViewModel>> GetOldBusinessRuleModelDataForStepTaskSkipLogic(List<StepTaskSkipLogicViewModel> oldStepTaskSkipLogic)
        {
            List<BusinessRuleModelViewModel> list = new();
            foreach (var item in oldStepTaskSkipLogic)
            {
                string i = item.Id;
                var result = await _cmsQueryBusiness.GetList<BusinessRuleModelViewModel, BusinessRuleModel>(x => x.BusinessRuleNodeId == i);
                foreach (var res in result)
                {
                    list.Add(res);
                }

            }
            return list;
        }

        public async Task<List<StepTaskSkipLogicViewModel>> GetOldStepTaskSkipLogics(List<StepTaskComponentViewModel> StepTaskCompList)
        {
            List<StepTaskSkipLogicViewModel> list = new();
            foreach (var item in StepTaskCompList)
            {
                string i = item.Id;
                var skipList = await _cmsQueryBusiness.GetList<StepTaskSkipLogicViewModel, StepTaskSkipLogic>(x => x.StepTaskComponentId == i);
                foreach (var subItem in skipList)
                {
                    list.Add(subItem);
                }
            }
            return list;
        }

        public async Task<List<StepTaskComponentViewModel>> GetOldStepTaskComponents(List<ComponentViewModel> compList)
        {
            List<StepTaskComponentViewModel> list = new();
            foreach (var comp in compList)
            {
                string oldId = comp.Id;
                var stepTaskComp = await _cmsQueryBusiness.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.ComponentId == oldId);
                if (stepTaskComp.IsNotNull())
                {
                    list.Add(stepTaskComp);
                }
            }
            return list;
        }





        // api will use this method -----------------------------------------------------------------------------
        public async Task<Dictionary<string, dynamic>> GetTemplateCompleteDataById(string id)
        {
            Dictionary<string, dynamic> list = new();
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();

            var templateData = await _templateBusiness.GetSingleById(id);
            list["Template"] = templateData;

            var templateCategoryData = await _templateBusiness.GetSingleById<TemplateCategoryViewModel, TemplateCategory>(templateData.TemplateCategoryId);
            list["TemplateCategory"] = templateCategoryData;

            var resourceLanguage = await _templateBusiness.GetList<ResourceLanguageViewModel, ResourceLanguage>(x => x.TemplateId == id);
            list["ResourceLanguage"] = resourceLanguage;

            if (templateData.TemplateType == TemplateTypeEnum.Note)
            {
                var noteTempData = await _templateBusiness.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == id);
                var noteIndexData = await _templateBusiness.GetSingle<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate>(x => x.TemplateId == id);
                list["NoteTemplate"] = noteTempData;
                list["NoteIndexPageTemplate"] = noteIndexData;
                list["NoteIndexPageColumn"] = await _templateBusiness.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == noteIndexData.Id);

            }
            else if (templateData.TemplateType == TemplateTypeEnum.Page)
            {
                var pageTempData = await _templateBusiness.GetSingle<PageTemplateViewModel, PageTemplate>(x => x.TemplateId == id);
                list["PageTemplate"] = pageTempData;
            }
            else if (templateData.TemplateType == TemplateTypeEnum.Form)
            {
                var formTempData = await _templateBusiness.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == id);
                var formIndexData = await _templateBusiness.GetSingle<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(x => x.TemplateId == id);
                list["FormTemplate"] = formTempData;
                list["FormIndexPageTemplate"] = formTempData;
                list["FormIndexPageColumn"] = await _templateBusiness.GetList<FormIndexPageColumnViewModel, FormIndexPageColumn>(x => x.FormIndexPageTemplateId == formIndexData.Id);

            }
            else if (templateData.TemplateType == TemplateTypeEnum.Service)
            {
                var serviceTempData = await _templateBusiness.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == id);
                var serviceIndexData = await _templateBusiness.GetSingle<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate>(x => x.TemplateId == id);
                var processDesignData = await _templateBusiness.GetSingle<ProcessDesignViewModel, ProcessDesign>(x => x.TemplateId == id);

                list["ServiceTemplate"] = serviceTempData;
                list["ServiceIndexPageTemplate"] = serviceIndexData;
                list["ServiceIndexPageColumn"] = await _templateBusiness.GetList<ServiceIndexPageColumnViewModel, ServiceIndexPageColumn>(x => x.ServiceIndexPageTemplateId == serviceIndexData.Id);
                list["ProcessDesign"] = processDesignData;

                var componentData = await _templateBusiness.GetList<ComponentViewModel, Component>(x => x.ProcessDesignId == processDesignData.Id);
                list["Component"] = componentData;

                var decisionScriptCompList = await GetOldDecisionScriptComponentData(componentData);
                list["DecisionScriptComponent"] = decisionScriptCompList;


                var BRMforDecisionComp = await GetOldBusinessRuleModelForDecisionScript(decisionScriptCompList);
                list["BusinessRuleModelForDecisionScript"] = BRMforDecisionComp;

                var StepTaskList = await GetStepTaskTemplateListData(id);
                list["StepTaskTemplate"] = StepTaskList;

                //-------------------------------------------------------------

                List<Dictionary<string, dynamic>> stepList = new();
                foreach (var step in StepTaskList)
                {
                    var res = await GetTemplateCompleteDataById(step.Id);
                    var ColumnMetadataList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == templateData.UdfTableMetadataId);
                    res["ColumnMetadata"] = ColumnMetadataList;
                    stepList.Add(res);
                }
                list["StepTaskDetails"] = stepList;

                //-------------------------------------------------------------

                var StepTaskComponentsList = await GetOldStepTaskComponents(componentData);
                list["StepTaskComponent"] = StepTaskComponentsList;

                var StepTaskSkipLogicList = await GetOldStepTaskSkipLogics(StepTaskComponentsList);
                list["StepTaskSkipLogic"] = StepTaskSkipLogicList;

                var BRMforSkipLogic = await GetOldBusinessRuleModelDataForStepTaskSkipLogic(StepTaskSkipLogicList);
                list["BusinessRuleModelForSkipLogic"] = BRMforSkipLogic;

                var StepTaskAssigneeLogicList = await GetOldStepTaskAssigneeLogics(StepTaskComponentsList);
                list["StepTaskAssigneeLogic"] = StepTaskAssigneeLogicList;

                var BRMforAssignee = await GetOldBusinessRuleModelDataForStepTaskAssigneeLogic(StepTaskAssigneeLogicList);
                list["BusinessRuleModelForAssignee"] = BRMforAssignee;

                var BRList = await GetOldBusinessRuleList(id);
                list["BusinessRule"] = BRList;

                var NodesList = await GetOldBusinessRuleNodeList(BRList);
                list["BusinessNode"] = NodesList;

                var BreResultsList = await GetOldBreResultList(NodesList);
                list["BreResult"] = BreResultsList;

                var ConnectorsList = await GetOldBRConnectorList(BRList);
                list["BusinessRuleConnector"] = ConnectorsList;

                var BRMforBusinessDecision = await GetOldBRMListforBusinessDecision(NodesList);
                list["BusinessRuleModelForBusinessDecision"] = BRMforBusinessDecision;


            }
            else if (templateData.TemplateType == TemplateTypeEnum.Task)
            {
                var taskTempData = await _templateBusiness.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == id);
                list["TaskTemplate"] = taskTempData;

                var taskIndexData = await _templateBusiness.GetSingle<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate>(x => x.TemplateId == id);
                list["TaskIndexPageTemplate"] = taskIndexData;

                list["TaskIndexPageColumn"] = await _templateBusiness.GetList<TaskIndexPageColumnViewModel, TaskIndexPageColumn>(x => x.TaskIndexPageTemplateId == taskIndexData.Id);

                var BRList = await GetOldBusinessRuleList(id);
                list["BusinessRule"] = BRList;

                var NodesList = await GetOldBusinessRuleNodeList(BRList);
                list["BusinessNode"] = NodesList;

                var BreResultsList = await GetOldBreResultList(NodesList);
                list["BreResult"] = BreResultsList;

                var ConnectorsList = await GetOldBRConnectorList(BRList);
                list["BusinessRuleConnector"] = ConnectorsList;

                var BRMforBusinessDecision = await GetOldBRMListforBusinessDecision(NodesList);
                list["BusinessRuleModelForBusinessDecision"] = BRMforBusinessDecision;

                var UdfList = await _repo.GetList<UdfPermissionViewModel, UdfPermission>(x => x.TemplateId == id);
                list["UdfPermission"] = UdfList;



            }
            else if (templateData.TemplateType == TemplateTypeEnum.Custom)
            {
                var customTempData = await _templateBusiness.GetSingle<CustomTemplateViewModel, CustomTemplate>(x => x.TemplateId == id);
                list["CustomTemplate"] = customTempData;
            }
            return list;

        }

        // copying templates fetched from dev -------------------------------------------------------------------

        public async Task<bool> CopyDevTemplates(CopyTemplateViewModel model)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();

            TemplateCategoryViewModel tempCategory = model.TemplateCategory;

            var tempateCat = await _templateBusiness.GetSingle<TemplateCategoryViewModel, TemplateCategory>(x => x.Id == tempCategory.Id);
            if (!tempateCat.IsNotNull())
            {
                var tempCatRes = await _templateBusiness.Create<TemplateCategoryViewModel, TemplateCategory>(tempCategory);
            }

            TemplateViewModel temp = new();

            string name = model.Template.Name;
            var flag = name.Split('_', 3);
            temp.Name = flag[2];

            temp.Code = model.Template.Code;
            temp.DisplayName = model.Template.DisplayName;
            temp.SequenceOrder = model.Template.SequenceOrder;
            temp.GroupCode = model.Template.GroupCode;
            temp.PortalId = model.Template.PortalId;
            temp.TemplateCategoryId = model.Template.TemplateCategoryId;
            temp.TemplateType = model.Template.TemplateType;

            var tempResult = await _templateBusiness.Create(temp);

            if (tempResult.IsSuccess)
            {
                TemplateTypeEnum type = temp.TemplateType;
                if (type == TemplateTypeEnum.Page)
                {
                    var res = await CopyPage(null, tempResult.Item.Id, true, model);
                }
                else if (type == TemplateTypeEnum.Form)
                {
                    var res = await CopyForm(null, tempResult.Item.Id, true, model);
                }
                else if (type == TemplateTypeEnum.Note)
                {
                    var res = await CopyNote(null, tempResult.Item.Id, true, model);
                }
                else if (type == TemplateTypeEnum.Task)
                {
                    var res = await CopyDevTask(tempResult.Item.Id, model);
                }
                else if (type == TemplateTypeEnum.Service)
                {
                    var res = await CopyDevService(tempResult.Item.Id, model);
                }
                else if (type == TemplateTypeEnum.Custom)
                {
                    var res = await CopyCustomTemplate(null, tempResult.Item.Id, model);
                }
            }


            return true;

        }

        public async Task<bool> CopyDevTask(string newTemplateId, CopyTemplateViewModel copyModel)
        {
            var _taskTemplateBusiness = _serviceProvider.GetService<ITaskTemplateBusiness>();
            var _taskIndexPageTemplateBusiness = _serviceProvider.GetService<ITaskIndexPageTemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            var _businessRule = _serviceProvider.GetService<IBusinessRuleBusiness>();
            var _businessRuleNode = _serviceProvider.GetService<IBusinessRuleNodeBusiness>();
            var _businessRuleConnector = _serviceProvider.GetService<IBusinessRuleConnectorBusiness>();
            var _businessRuleModel = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            var _breResultBusiness = _serviceProvider.GetService<IBreResultBusiness>();
            // Task Template
            var oldTaskModel = copyModel.TaskTemplate;
            var taskResult = await _taskTemplateBusiness.CopyTaskTemplate(oldTaskModel, newTemplateId, copyModel);
            if (taskResult.IsSuccess)
            {
                // Index
                var oldTaskIndexModel = copyModel.TaskIndexPageTemplate;
                var taskIndexResult = await _taskIndexPageTemplateBusiness.CopyTaskTemplateIndexPageData(oldTaskIndexModel, newTemplateId, copyModel);
                if (taskIndexResult.IsSuccess)
                {
                    // Language
                    var oldLanguageList = copyModel.ResourceLanguage;
                    var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId);
                    if (languageResult)
                    {
                        var oldBusinessRuleList = copyModel.BusinessRule;
                        // copy business rule
                        var brids = await _businessRule.CopyBusinessRules(oldBusinessRuleList, newTemplateId);
                        // fetch business rule nodes
                        var oldBRNodeList = copyModel.BusinessNode;
                        // copy business rule nodes
                        var nodeids = await _businessRuleNode.CopyBusinessRuleNodes(brids, oldBRNodeList);
                        // fetch bre results
                        var oldBreResultsList = copyModel.BreResult;
                        // copy bre results
                        var breIds = await _breResultBusiness.CopyBreResults(nodeids, oldBreResultsList);
                        // fetch business rule connectors
                        var oldConnectorsList = copyModel.BusinessRuleConnector;
                        // copy connectors
                        var connectorResult = await _businessRuleConnector.CopyBusinessRuleConnector(brids, nodeids, oldConnectorsList);
                        // fetch business decisions
                        var oldBusinessDecisionList = copyModel.BusinessRuleModelForDecisionScript;
                        // copy business decisions
                        var decisionResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBusinessDecisionList, nodeids);
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task<List<dynamic>> CopyDevStepTask(List<CopyTemplateViewModel> copyTempList)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();

            List<dynamic> StepTaskIdList = new();

            foreach (var model in copyTempList)
            {
                TemplateViewModel temp = new();

                string name = model.Template.Name;
                var flag = name.Split('_', 3);
                temp.Name = flag[2];

                temp.Code = model.Template.Code;
                temp.DisplayName = model.Template.DisplayName;
                temp.SequenceOrder = model.Template.SequenceOrder;
                temp.GroupCode = model.Template.GroupCode;
                temp.PortalId = model.Template.PortalId;
                temp.TemplateCategoryId = model.Template.TemplateCategoryId;
                temp.TemplateType = model.Template.TemplateType;


                temp.TaskType = TaskTypeEnum.StepTask;
                var result = await _templateBusiness.Create(temp);
                if (result.IsSuccess)
                {
                    var i = new { OldStepTaskId = model.Template.Id, NewStepTaskId = result.Item.Id };
                    StepTaskIdList.Add(i);
                    var res = await CopyDevTask(result.Item.Id, model);

                }
                else
                {

                }


            }
            return StepTaskIdList;
        }

        public async Task<bool> CopyDevService(string newTemplateId, CopyTemplateViewModel copyModel)
        {
            var _serviceTemplateBusiness = _serviceProvider.GetService<IServiceTemplateBusiness>();
            var _serviceIndexPageTemplateBusiness = _serviceProvider.GetService<IServiceIndexPageTemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            var _businessRule = _serviceProvider.GetService<IBusinessRuleBusiness>();
            var _businessRuleNode = _serviceProvider.GetService<IBusinessRuleNodeBusiness>();
            var _businessRuleConnector = _serviceProvider.GetService<IBusinessRuleConnectorBusiness>();
            var _businessRuleModel = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            var _breResultBusiness = _serviceProvider.GetService<IBreResultBusiness>();
            var _processDesignTemplateBusiness = _serviceProvider.GetService<IProcessDesignBusiness>();
            var _componentBusiness = _serviceProvider.GetService<IComponentBusiness>();
            var _decisionScriptComponentBusiness = _serviceProvider.GetService<IDecisionScriptComponentBusiness>();

            // Service 
            var oldServiceModel = copyModel.ServiceTemplate;
            var serviceResult = await CopyServiceTemplate(oldServiceModel, newTemplateId, copyModel);

            if (serviceResult.IsSuccess)
            {
                // Index
                var oldServiceIndexModel = copyModel.ServiceIndexPageTemplate;
                var serviceIndexResult = await CopyServiceTempltaeIndexPageData(oldServiceIndexModel, newTemplateId, copyModel);
                if (serviceIndexResult.IsSuccess)
                {
                    // Language
                    var oldLanguageList = copyModel.ResourceLanguage;
                    var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId);

                    if (languageResult)
                    {
                        var oldPD = copyModel.ProcessDesign;
                        var newPD = await _processDesignTemplateBusiness.GetSingle(x => x.TemplateId == newTemplateId);
                        if (oldPD.IsNotNull()) // --------- condition not required
                        {
                            // Fetch old components
                            var oldComponentsList = copyModel.Component;
                            // Components Creation
                            var ComponentIdList = await CopyComponents(oldPD.Id, newPD.Id, oldComponentsList);
                            // Fetch old Decision script component
                            var oldDecisionScriptCompList = copyModel.DecisionScriptComponent;
                            // Copy Decision Script Component
                            var decisionScriptCompIds = await CopyDecisionScriptComponent(ComponentIdList, oldDecisionScriptCompList);
                            // Fetch Business Rule Model for Decision Script Component
                            var DSCRulesList = copyModel.BusinessRuleModelForDecisionScript;
                            // Copy Business rule model for decision script component
                            var DSCRulesResult = await _businessRuleModel.CopyBusinessRuleModelForDecisionScript(DSCRulesList, decisionScriptCompIds);
                            //-------------------------------------------------------------------------------------
                            // Step Task Template Creation
                            //var oldStepTaskList = copyModel.StepTaskTemplate;

                            //var stepTaskIdList = await CreateStepTask(oldStepTaskList, model.StepTaskJson);
                            var stepTaskIdList = await CopyDevStepTask(copyModel.StepTaskDetails);
                            // Fetch old Step task components
                            var oldStepTaskComponentsList = copyModel.StepTaskComponent;
                            // Step Task Component Creation
                            var stepTaskCompIdList = await CopyStepTaskComponent(ComponentIdList, stepTaskIdList, newTemplateId, null, oldStepTaskComponentsList, copyModel);
                            //-------------------------------------------------------------------------------------
                            // get old step task skip logics
                            var OldStepTaskSkipLogicList = copyModel.StepTaskSkipLogic;
                            // Copy StepTaskSkipLogic
                            var SkipLogicIds = await CopyStepTaskLogic(OldStepTaskSkipLogicList, stepTaskCompIdList);
                            // get business rule model old of skip logics
                            var oldBRModelData = copyModel.BusinessRuleModelForSkipLogic;
                            // Copy Business rule model for skip logics
                            var BRModelResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBRModelData, SkipLogicIds);
                            //----------------------------------------------------------------------------------------
                            // Get old step task custom assignee logics
                            var OldStepTaskAssigneeLogicList = copyModel.StepTaskAssigneeLogic;
                            // Copy Step Task Assignee Logic
                            var AssigneeLogicIds = await CopyStepTaskAssignee(OldStepTaskAssigneeLogicList, stepTaskCompIdList);
                            // Get business rule model old of assignee logics
                            var oldBRModelAssigneeData = copyModel.BusinessRuleModelForAssignee;
                            // Copy business rule model for assignee logics
                            var BRModelAssigneeResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBRModelAssigneeData, AssigneeLogicIds);
                            //----------------------------------------------------------------------------------------
                            // Fetch old business rules
                            var oldBRList = copyModel.BusinessRule;
                            // Copy Business rules
                            var BRIds = await _businessRule.CopyBusinessRules(oldBRList, newTemplateId);
                            // Fetch old business rule nodes
                            var oldNodesList = copyModel.BusinessNode;
                            // Copy Business rule nodes
                            var NodeIds = await _businessRuleNode.CopyBusinessRuleNodes(BRIds, oldNodesList);
                            // Fetch old Bre Results
                            var oldBreResultsList = copyModel.BreResult;
                            // Copy Bre Results
                            var BreIds = await _breResultBusiness.CopyBreResults(NodeIds, oldBreResultsList);
                            // Fetch old Business rule connectors
                            var oldConnectorsList = copyModel.BusinessRuleConnector;
                            // copy Connectors
                            var ConnectorResult = await _businessRuleConnector.CopyBusinessRuleConnector(BRIds, NodeIds, oldConnectorsList);
                            // Fetch BRM data for business decision
                            var oldBRMListForBusinessDecision = copyModel.BusinessRuleModelForBusinessDecision;
                            // Copy brm for business decision
                            var BRModelBusinessDecisionResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBRMListForBusinessDecision, NodeIds);

                            return true;
                        }
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //--------------------------------------------------------------------------------------------------------

        public async Task<bool> CopyForm(string oldTempId, string newTemplateId, bool devImport = false, CopyTemplateViewModel copyModel = null)
        {
            var _formTemplateBusiness = _serviceProvider.GetService<IFormTemplateBusiness>();
            var _formIndexPageTemplateBusiness = _serviceProvider.GetService<IFormIndexPageTemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            // Form
            if (!devImport)
            {
                var oldFormModel = await _cmsQueryBusiness.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == oldTempId);
                var formResult = await _formTemplateBusiness.CopyFormTemplate(oldFormModel, newTemplateId); // done
                if (formResult.IsSuccess)
                {
                    // Index
                    var oldFormIndexModel = await _cmsQueryBusiness.GetSingle<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(x => x.TemplateId == oldTempId);
                    var formIndexResult = await _formIndexPageTemplateBusiness.CopyFormTemplateIndexPageData(oldFormIndexModel, newTemplateId); // done
                    if (formIndexResult.IsSuccess)
                    {
                        // Language
                        var oldLanguageList = await _cmsQueryBusiness.GetList<ResourceLanguageViewModel, ResourceLanguage>(x => x.TemplateId == oldTempId);
                        var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                var oldFormModel = copyModel.FormTemplate;
                var formResult = await _formTemplateBusiness.CopyFormTemplate(oldFormModel, newTemplateId, copyModel, devImport);
                if (formResult.IsSuccess)
                {
                    // Index
                    var oldFormIndexModel = copyModel.FormIndexPageTemplate;
                    var formIndexResult = await _formIndexPageTemplateBusiness.CopyFormTemplateIndexPageData(oldFormIndexModel, newTemplateId, devImport, copyModel);
                    if (formIndexResult.IsSuccess)
                    {
                        // Language
                        var oldLanguageList = copyModel.ResourceLanguage;
                        var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> CopyPage(string oldTempId, string newTemplateId, bool devImport = false, CopyTemplateViewModel copyModel = null)
        {
            var _pageTemplateBusiness = _serviceProvider.GetService<IPageTemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            // Page
            PageTemplateViewModel oldPageModel = new();
            CommandResult<PageTemplateViewModel> pageResult;
            if (!devImport)
            {
                oldPageModel = await _pageTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
                var oldTempModel = await _pageTemplateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Id == oldTempId);
                pageResult = await _pageTemplateBusiness.CopyPageTemplate(oldTempModel.Json, oldPageModel, newTemplateId);
            }
            else
            {
                oldPageModel = copyModel.PageTemplate;
                pageResult = await _pageTemplateBusiness.CopyPageTemplate(copyModel.Template.Json, oldPageModel, newTemplateId);
            }


            if (pageResult.IsSuccess)
            {
                List<ResourceLanguageViewModel> oldLanguageList = new();
                //Language
                if (!devImport)
                {
                    oldLanguageList = await _resourceLanguageBusiness.GetList(x => x.TemplateId == oldTempId);
                }
                else
                {
                    oldLanguageList = copyModel.ResourceLanguage;
                }


                var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId); // done
                if (!languageResult)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> CopyNote(string oldTempId, string newTemplateId, bool devImport = false, CopyTemplateViewModel copyModel = null)
        {
            var _noteTemplateBusiness = _serviceProvider.GetService<INoteTemplateBusiness>();
            var _noteIndexPageTemplateBusiness = _serviceProvider.GetService<INoteIndexPageTemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            if (!devImport)
            {
                // Note
                var oldNoteModel = await _noteTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
                var noteResult = await _noteTemplateBusiness.CopyNoteTemplate(oldNoteModel, newTemplateId); // done

                if (noteResult.IsSuccess)
                {
                    // Index
                    var oldNoteIndexModel = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
                    var noteIndexResult = await _noteIndexPageTemplateBusiness.CopyNoteTemplateIndexPageData(oldNoteIndexModel, newTemplateId); // done
                    if (noteIndexResult.IsSuccess)
                    {
                        // DMSOCRMapping
                        // Language
                        var oldLanguageList = await _resourceLanguageBusiness.GetList(x => x.TemplateId == oldTempId);
                        var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId); // done
                        if (!languageResult)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Note
                var oldNoteModel = copyModel.NoteTemplate;
                var noteResult = await _noteTemplateBusiness.CopyNoteTemplate(oldNoteModel, newTemplateId, copyModel);

                if (noteResult.IsSuccess)
                {
                    // Index
                    var oldNoteIndexModel = copyModel.NoteIndexPageTemplate;
                    var noteIndexResult = await _noteIndexPageTemplateBusiness.CopyNoteTemplateIndexPageData(oldNoteIndexModel, newTemplateId, copyModel);
                    if (noteIndexResult.IsSuccess)
                    {
                        // DMSOCRMapping
                        // Language
                        var oldLanguageList = copyModel.ResourceLanguage;
                        var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId);
                        if (!languageResult)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> CopyTask(string oldTempId, string newTemplateId)
        {
            var _taskTemplateBusiness = _serviceProvider.GetService<ITaskTemplateBusiness>();
            var _taskIndexPageTemplateBusiness = _serviceProvider.GetService<ITaskIndexPageTemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            var _businessRule = _serviceProvider.GetService<IBusinessRuleBusiness>();
            var _businessRuleNode = _serviceProvider.GetService<IBusinessRuleNodeBusiness>();
            var _businessRuleConnector = _serviceProvider.GetService<IBusinessRuleConnectorBusiness>();
            var _businessRuleModel = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            var _breResultBusiness = _serviceProvider.GetService<IBreResultBusiness>();
            // Task Template
            var oldTaskModel = await _taskTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
            var taskResult = await _taskTemplateBusiness.CopyTaskTemplate(oldTaskModel, newTemplateId); // done
            if (taskResult.IsSuccess)
            {
                // Index
                var oldTaskIndexModel = await _taskIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
                var taskIndexResult = await _taskIndexPageTemplateBusiness.CopyTaskTemplateIndexPageData(oldTaskIndexModel, newTemplateId); // done
                if (taskIndexResult.IsSuccess)
                {
                    // Language
                    var oldLanguageList = await _resourceLanguageBusiness.GetList(x => x.TemplateId == oldTempId);
                    var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId); // done
                    if (languageResult)
                    {
                        var oldBusinessRuleList = await GetOldBusinessRuleList(oldTempId);
                        // copy business rule
                        var brids = await _businessRule.CopyBusinessRules(oldBusinessRuleList, newTemplateId);
                        // fetch business rule nodes
                        var oldBRNodeList = await GetOldBusinessRuleNodeList(oldBusinessRuleList);
                        // copy business rule nodes
                        var nodeids = await _businessRuleNode.CopyBusinessRuleNodes(brids, oldBRNodeList);
                        // fetch bre results
                        var oldBreResultsList = await GetOldBreResultList(oldBRNodeList);
                        // copy bre results
                        var breIds = await _breResultBusiness.CopyBreResults(nodeids, oldBreResultsList);
                        // fetch business rule connectors
                        var oldConnectorsList = await GetOldBRConnectorList(oldBusinessRuleList);
                        // copy connectors
                        var connectorResult = await _businessRuleConnector.CopyBusinessRuleConnector(brids, nodeids, oldConnectorsList);
                        // fetch business decisions
                        var oldBusinessDecisionList = await GetOldBRMListforBusinessDecision(oldBRNodeList);
                        // copy business decisions
                        var decisionResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBusinessDecisionList, nodeids);
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }


        public async Task<bool> CopyService(string oldTempId, string newTemplateId, TemplateViewModel model)
        {
            var _serviceTemplateBusiness = _serviceProvider.GetService<IServiceTemplateBusiness>();
            var _serviceIndexPageTemplateBusiness = _serviceProvider.GetService<IServiceIndexPageTemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();
            var _businessRule = _serviceProvider.GetService<IBusinessRuleBusiness>();
            var _businessRuleNode = _serviceProvider.GetService<IBusinessRuleNodeBusiness>();
            var _businessRuleConnector = _serviceProvider.GetService<IBusinessRuleConnectorBusiness>();
            var _businessRuleModel = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            var _breResultBusiness = _serviceProvider.GetService<IBreResultBusiness>();
            var _processDesignTemplateBusiness = _serviceProvider.GetService<IProcessDesignBusiness>();
            var _componentBusiness = _serviceProvider.GetService<IComponentBusiness>();
            var _decisionScriptComponentBusiness = _serviceProvider.GetService<IDecisionScriptComponentBusiness>();

            // Service 
            var oldServiceModel = await _serviceTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
            var serviceResult = await CopyServiceTemplate(oldServiceModel, newTemplateId); // done

            if (serviceResult.IsSuccess)
            {
                // Index
                var oldServiceIndexModel = await _serviceIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
                var serviceIndexResult = await CopyServiceTempltaeIndexPageData(oldServiceIndexModel, newTemplateId); // done
                if (serviceIndexResult.IsSuccess)
                {
                    // Language
                    var oldLanguageList = await _resourceLanguageBusiness.GetList(x => x.TemplateId == oldTempId);
                    var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId); // done

                    if (languageResult)
                    {
                        var oldPD = await _processDesignTemplateBusiness.GetSingle(x => x.TemplateId == oldTempId);
                        var newPD = await _processDesignTemplateBusiness.GetSingle(x => x.TemplateId == newTemplateId);
                        if (oldPD.IsNotNull()) // --------- condition not required
                        {
                            // Process Design Creation
                            //var PDList = await _cmsBusiness.CopyProcessDesign(oldPDList, newTemplateId); --- not required
                            //List<dynamic> PDList2 = new();
                            //PDList2.Add(new { OldPdId = oldPD.Id, NewPdId = newPD.Id });

                            // Fetch old components
                            var oldComponentsList = await GetOldComponentsList(oldPD);
                            // Components Creation
                            var ComponentIdList = await CopyComponents(oldPD.Id, newPD.Id, oldComponentsList);
                            // Fetch old Decision script component
                            var oldDecisionScriptCompList = await GetOldDecisionScriptComponentData(oldComponentsList);
                            // Copy Decision Script Component
                            var decisionScriptCompIds = await CopyDecisionScriptComponent(ComponentIdList, oldDecisionScriptCompList);
                            // Fetch Business Rule Model for Decision Script Component
                            var DSCRulesList = await GetOldBusinessRuleModelForDecisionScript(oldDecisionScriptCompList);
                            // Copy Business rule model for decision script component
                            var DSCRulesResult = await _businessRuleModel.CopyBusinessRuleModelForDecisionScript(DSCRulesList, decisionScriptCompIds);
                            //-------------------------------------------------------------------------------------
                            // Step Task Template Creation
                            var oldStepTaskList = await GetStepTaskTemplateListData(oldTempId);
                            var stepTaskIdList = await CreateStepTask(oldStepTaskList, model.StepTaskJson);
                            // Fetch old Step task components
                            var oldStepTaskComponentsList = await GetOldStepTaskComponents(oldComponentsList);
                            // Step Task Component Creation
                            var stepTaskCompIdList = await CopyStepTaskComponent(ComponentIdList, stepTaskIdList, newTemplateId, oldTempId, oldStepTaskComponentsList);
                            //-------------------------------------------------------------------------------------
                            // get old step task skip logics
                            var OldStepTaskSkipLogicList = await GetOldStepTaskSkipLogics(oldStepTaskComponentsList);
                            // Copy StepTaskSkipLogic
                            var SkipLogicIds = await CopyStepTaskLogic(OldStepTaskSkipLogicList, stepTaskCompIdList);
                            // get business rule model old of skip logics
                            var oldBRModelData = await GetOldBusinessRuleModelDataForStepTaskSkipLogic(OldStepTaskSkipLogicList);
                            // Copy Business rule model for skip logics
                            var BRModelResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBRModelData, SkipLogicIds);
                            //----------------------------------------------------------------------------------------
                            // Get old step task custom assignee logics
                            var OldStepTaskAssigneeLogicList = await GetOldStepTaskAssigneeLogics(oldStepTaskComponentsList);
                            // Copy Step Task Assignee Logic
                            var AssigneeLogicIds = await CopyStepTaskAssignee(OldStepTaskAssigneeLogicList, stepTaskCompIdList);
                            // Get business rule model old of assignee logics
                            var oldBRModelAssigneeData = await GetOldBusinessRuleModelDataForStepTaskAssigneeLogic(OldStepTaskAssigneeLogicList);
                            // Copy business rule model for assignee logics
                            var BRModelAssigneeResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBRModelAssigneeData, AssigneeLogicIds);
                            //----------------------------------------------------------------------------------------
                            // Fetch old business rules
                            var oldBRList = await GetOldBusinessRuleList(oldTempId);
                            // Copy Business rules
                            var BRIds = await _businessRule.CopyBusinessRules(oldBRList, newTemplateId);
                            // Fetch old business rule nodes
                            var oldNodesList = await GetOldBusinessRuleNodeList(oldBRList);
                            // Copy Business rule nodes
                            var NodeIds = await _businessRuleNode.CopyBusinessRuleNodes(BRIds, oldNodesList);
                            // Fetch old Bre Results
                            var oldBreResultsList = await GetOldBreResultList(oldNodesList);
                            // Copy Bre Results
                            var BreIds = await _breResultBusiness.CopyBreResults(NodeIds, oldBreResultsList);
                            // Fetch old Business rule connectors
                            var oldConnectorsList = await GetOldBRConnectorList(oldBRList);
                            // copy Connectors
                            var ConnectorResult = await _businessRuleConnector.CopyBusinessRuleConnector(BRIds, NodeIds, oldConnectorsList);
                            // Fetch BRM data for business decision
                            var oldBRMListForBusinessDecision = await GetOldBRMListforBusinessDecision(oldNodesList);
                            // Copy brm for business decision
                            var BRModelBusinessDecisionResult = await _businessRuleModel.CopyBusinessRuleModelForLogics(oldBRMListForBusinessDecision, NodeIds);
                            //----------------------------------------------------------------------------------------
                            // Copying Pre/Post/PreLoad Scripts for StepTasks
                            //var stScriptsCopyRes = await _cmsBusiness.CopyStepTaskScripts(stepTaskIdList, oldStepTaskList);
                            return true;
                        }
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CopyCustomTemplate(string oldTempId, string newTemplateId, CopyTemplateViewModel copyModel = null)
        {
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _resourceLanguageBusiness = _serviceProvider.GetService<IResourceLanguageBusiness>();

            var customTemp = await _templateBusiness.GetSingle<CustomTemplateViewModel, CustomTemplate>(x => x.TemplateId == newTemplateId);
            CustomTemplateViewModel oldCustomTemp = new();
            if (copyModel.IsNotNull())
            {
                oldCustomTemp = copyModel.CustomTemplate;
            }
            else
            {
                oldCustomTemp = await _templateBusiness.GetSingle<CustomTemplateViewModel, CustomTemplate>(x => x.TemplateId == oldTempId);
            }

            CustomTemplateViewModel temp = customTemp;
            customTemp = _autoMapper.Map<CustomTemplateViewModel>(oldCustomTemp);
            customTemp.Id = temp.Id;
            customTemp.TemplateId = temp.TemplateId;
            customTemp.CreatedBy = temp.CreatedBy;
            customTemp.CreatedDate = temp.CreatedDate;
            customTemp.LegalEntityId = temp.LegalEntityId;
            customTemp.LastUpdatedDate = temp.LastUpdatedDate;
            customTemp.CompanyId = temp.CompanyId;

            var customRes = await _templateBusiness.Edit<CustomTemplateViewModel, CustomTemplate>(customTemp);
            if (customRes.IsSuccess)
            {
                List<ResourceLanguageViewModel> oldLanguageList = new();
                if (copyModel.IsNotNull())
                {
                    oldLanguageList = copyModel.ResourceLanguage;
                }
                else
                {
                    oldLanguageList = await _resourceLanguageBusiness.GetList(x => x.TemplateId == oldTempId);
                }
                var languageResult = await _resourceLanguageBusiness.CopyLanguage(oldLanguageList, newTemplateId);
                if (languageResult)
                {
                    return true;
                }
                return false;
            }
            return false;

        }

        public async Task<List<PropertyViewModel>> GetPropertyData(string userId)
        {
            var queryData = await _cmsQueryBusiness.GetPropertyData(userId);
            return queryData;
        }
    }
}
