using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Kendo.Mvc.UI;
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

namespace CMS.Business
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
            , IUserContext userContext) : base(repo, autoMapper)
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
        }

        public async Task ManageTable(TableMetadataViewModel tableMetadata)
        {
            var existingTableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableMetadata.Id);
            if (existingTableMetadata != null)
            {
                existingTableMetadata.OldName = tableMetadata.OldName;
                existingTableMetadata.OldSchema = tableMetadata.OldSchema;
                var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                (
	                select 1
	                from information_schema.{(existingTableMetadata.TableType == TableTypeEnum.Table ? "tables" : "views")} 
	                where table_schema = '{existingTableMetadata.Schema}'
	                and table_name = '{existingTableMetadata.Name}'
                ) ", null);
                if (tableExists)
                {

                    var recordExists = await _queryRepo.ExecuteScalar<bool?>(@$"select true from  
                    {existingTableMetadata.Schema}.""{existingTableMetadata.Name}"" limit 1 ", null);
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
                var tableVarWithSchema = "<<table-schema>>";
                var query = new StringBuilder();
                var oldTableWithSchema = $"{tableMetadata.OldSchema}.\"{tableMetadata.OldName}\"";
                query.Append($"DROP View {oldTableWithSchema};");
                query.Append($"CREATE OR REPLACE VIEW {tableVarWithSchema} As {tableMetadata.Query} ;");
                query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

                var tableWithSchema = $"{tableMetadata.Schema}.\"{tableMetadata.Name}\"";
                var table = $"{tableMetadata.Name}";
                var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);
                await _queryRepo.ExecuteCommand(tableQuery, null);
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
                var tableVarWithSchema = "<<table-schema>>";
                var query = new StringBuilder();
                if (dropTable)
                {
                    query.Append($"DROP View {tableVarWithSchema};");
                }
                query.Append($"CREATE OR REPLACE VIEW {tableVarWithSchema} As {tableMetadata.Query} ;");
                query.Append($"ALTER TABLE {tableVarWithSchema} OWNER to {ApplicationConstant.Database.Owner.Postgres};");

                var tableWithSchema = $"{tableMetadata.Schema}.\"{tableMetadata.Name}\"";
                var table = $"{tableMetadata.Name}";
                var tableQuery = query.ToString().Replace("<<table-schema>>", tableWithSchema).Replace("<<table>>", table);
                await _queryRepo.ExecuteCommand(tableQuery, null);
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
            await _queryRepo.ExecuteCommand(tableQuery, null);

            //var tableLogWithSchema = $"{ApplicationConstant.Database.Schema.Log}.\"{tableMetadata.Name}_Log\"";
            //var tableLog = $"{tableMetadata.Name}_Log";
            //var tableLogQuery = query.ToString().Replace("<<table-schema>>", tableLogWithSchema).Replace("<<table>>", tableLog);
            //await _queryRepo.ExecuteCommand(tableLogQuery, null);
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
                    var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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
            await _queryRepo.ExecuteCommand(tableQuery, null);
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
            await _queryRepo.ExecuteCommand(tableQuery, null);

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
            await _queryRepo.ExecuteCommand(tableQuery, null);

            //var tableLogWithSchema = $"{ApplicationConstant.Database.Schema.Log}.\"{tableMetadata.Name}_Log\"";
            //var tableLog = $"{tableMetadata.Name}_Log";
            //var tableLogQuery = query.ToString().Replace("<<table-schema>>", tableLogWithSchema).Replace("<<table>>", tableLog);
            //await _queryRepo.ExecuteCommand(tableLogQuery, null);
        }


        private async Task EditNoteTable(TableMetadataViewModel tableMetadata)
        {
            var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}'", null);

            var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

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
                await _queryRepo.ExecuteCommand(queryText, null);

                queryLog.Append(string.Join(",", alterColumnScriptList));
                queryLog.Append(";");
                await _queryRepo.ExecuteCommand(queryLog.ToString(), null);

            }

        }
        private async Task EditTaskTable(TableMetadataViewModel tableMetadata)
        {
            var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}'", null);

            var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

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
                await _queryRepo.ExecuteCommand(queryText, null);
            }

        }
        private async Task EditServiceTable(TableMetadataViewModel tableMetadata)
        {
            var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}'", null);

            var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

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
                await _queryRepo.ExecuteCommand(queryText, null);
            }

        }
        private async Task EditFormTable(TableMetadataViewModel tableMetadata)
        {
            var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}'", null);

            var constraints = await _queryRepo.ExecuteQueryDataTable(@$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';", null);

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
                await _queryRepo.ExecuteCommand(queryText, null);
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
                        var selectQuery = await GetSelectQuery(tableMetadata);
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
                default:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == udfTableMetadataId);
                    baseQuery = await GetSelectQuery(tableMetaData);
                    selectQuery = @$"{baseQuery} and ""{tableMetaData.Name}"".""Id""='{recordId}' limit 1";
                    dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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
                    break;
                case TemplateTypeEnum.Service:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.UdfTableMetadataId);
                    dt = await _serviceBusiness.GetServiceDataTableById(recordId, tableMetaData, isLog, logId);
                    break;
                case TemplateTypeEnum.Note:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.TableMetadataId);
                    dt = await _noteBusiness.GetNoteDataTableById(recordId, tableMetaData, isLog, logId);
                    break;
                default:
                    tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == page.Template.TableMetadataId);
                    baseQuery = await GetSelectQuery(tableMetaData);
                    selectQuery = @$"{baseQuery} and ""{tableMetaData.Name}"".""Id""='{recordId}' limit 1";
                    dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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
        public async Task<DataTable> GetData(string schemaName, string tableName, string columns = null, string filter = null, string orderbyColumns = null, OrderByEnum orderby = OrderByEnum.Ascending, string where = null)
        {
            var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Schema == schemaName && x.Name == tableName);
            var selectQuery = "";
            switch (tableMetaData.TemplateType)
            {
                case TemplateTypeEnum.Note:
                    selectQuery = await _noteBusiness.GetSelectQuery(tableMetaData, where, columns, filter);
                    break;
                case TemplateTypeEnum.Task:
                    selectQuery = await _taskBusiness.GetSelectQuery(tableMetaData, where, columns, filter);
                    break;
                case TemplateTypeEnum.Service:
                    selectQuery = await _serviceBusiness.GetSelectQuery(tableMetaData, where, columns, filter);
                    break;
                default:
                    selectQuery = await GetSelectQuery(tableMetaData, where, columns, filter);
                    break;
                    //default:
                    //    selectQuery = await GetCustomSelectQuery(tableMetaData, columns, filter);
                    //    break;
            }
            return await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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

        private async Task<string> GetSelectQuery(TableMetadataViewModel tableMetaData, string where = null, string filtercolumns = null, string filter = null)
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
                var query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
                ,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
                ,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false
                where c.""TableMetadataId""='{tableMetaData.Id}'
                and c.""IsForeignKey""=true and c.""IsDeleted""=false";
                var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
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
                where=where.Replace(",", "','");
                selectQuery = $"{selectQuery} {where}";
            }
            return selectQuery;
        }

        private async Task<string> GetDataByColumn(ColumnMetadataViewModel column, object columnValue, TableMetadataViewModel tableMetaData, string excludeId)
        {

            var selectQuery = @$"select * from {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" where  ""IsDeleted""=false and ""{column.Name}""='{columnValue}'";
            if (excludeId.IsNotNullAndNotEmpty())
            {
                selectQuery = @$"{selectQuery} and ""Id""<>'{excludeId}'";
            }
            selectQuery = @$"{selectQuery} limit 1";
            var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
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
                    model = await _repo.GetSingle<FormIndexPageTemplateViewModel, FormIndexPageTemplate>(x => x.TemplateId == page.TemplateId);
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
                    item.ColumnName = $"{item.ForeignKeyTableAliasName}_{item.ColumnMetadata.Name}";
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
                var result = await CreateForm(model.Json, model.PageId);
                if (result.Item1)
                {
                    model.RecordId = result.Item2;
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
                var result = await EditForm(model.RecordId, model.Json, model.PageId);
                if (result.Item1)
                {
                    model.RecordId = result.Item2;
                    return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model));
                }
                else
                {
                    return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model, result.Item1, result.Item2));
                }

            }
            return await Task.FromResult(CommandResult<FormTemplateViewModel>.Instance(model, false, "An error occured while processing your request"));
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
                        var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
                        selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
                        selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
                        selectQuery.Append(@$"where ""Id""='{model.RecordId}'");
                        var queryText = selectQuery.ToString();
                        await _queryRepo.ExecuteCommand(queryText, null);

                    }

                }
                return await Task.FromResult(id);
            }
            return await Task.FromResult(string.Empty);
        }
        public async Task<Tuple<bool, string>> CreateForm(string data, string pageId,string templateCode=null)
        {
            Template template = null;
            if (templateCode.IsNotNullAndNotEmpty())
            {
                template = await _repo.GetSingle(x => x.Code == templateCode);
            }
            else
            {
                var page = await _pageBusiness.GetPageForExecution(pageId);
                if (page != null )
                {
                    template = page.Template;
                }
            }
           // var page = await _pageBusiness.GetPageForExecution(pageId);
            if (template != null )
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
                            columnKeys.Add(@$"""{ col.Name}""");
                            columnValues.Add(BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType));
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
                        await _queryRepo.ExecuteCommand(queryText, null);

                    }

                }
                return await Task.FromResult(new Tuple<bool, string>(true, id));
            }
            return await Task.FromResult(new Tuple<bool, string>(false, "Page does not exist."));
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
            if(template!=null)
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
                        foreach (var col in tableColumns)
                        {
                            columnKeys.Add(@$"""{ col.Name}"" = {BusinessHelper.ConvertToDbValue(rowData.GetValueOrDefault(col.Name), col.IsSystemColumn, col.DataType)}");
                        }
                        selectQuery.Append($"{string.Join(",", columnKeys)}");
                        selectQuery.Append(Environment.NewLine);
                        selectQuery.Append(@$"where  ""Id"" = '{recordId}'");
                        var queryText = selectQuery.ToString();
                        await _queryRepo.ExecuteCommand(queryText, null);
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
            var query1 = "";
            if (taskdetails.IsNotNull() && taskdetails.TemplateCode == "EMAIL_TASK")
            {

                query1 = $@"with recursive parentemail as(
with recursive email as(
select nt.""TaskSubject"" as TaskSubject,TO_CHAR(nt.""StartDate"", 'MM/DD/YYYY HH12:MI:SS AM') as SentDate,nt.""TaskDescription"" as TaskDescription,nt.""AssignedToUserId"",
u.""Email"" as AssignedToUserName,ou.""Email"" as OwnerUserName,nt.""TemplateCode"",ru.""Email"" as RequestedByUserName,pet.""CC"" as CC,pet.""BCC"" as BCC,nt.""Id""
,case when pt.""TemplateCode""='EMAIL_TASK' then nt.""ParentTaskId"" else null end as ""ParentTaskId"",tl.""Code"" as ""StatusCode""
 ,pet.""To"" as To,pet.""From"" as From,nt.""Id"" as key,'' as title,true as lazy
from public.""NtsTask"" as nt
join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false
left join public.""User"" as u on u.""Id""=nt.""AssignedToUserId"" and u.""IsDeleted""=false
left join public.""User"" as ou on ou.""Id""=nt.""OwnerUserId"" and ou.""IsDeleted""=false
left join public.""User"" as ru on ru.""Id""=nt.""RequestedByUserId"" and ru.""IsDeleted""=false
left join public.""NtsTask"" as pt on pt.""Id""=nt.""ParentTaskId""	and pt.""IsDeleted""=false
 left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false
where 
	(nt.""Id""='{refId}' and 
	nt.""TemplateCode""='EMAIL_TASK') and nt.""IsDeleted""=false
union all
select nt.""TaskSubject"" as TaskSubject,TO_CHAR(nt.""StartDate"", 'MM/DD/YYYY HH12:MI:SS AM') as SentDate,nt.""TaskDescription"" as TaskDescription,nt.""AssignedToUserId"",
u.""Email"" as AssignedToUserName,ou.""Email"" as OwnerUserName,nt.""TemplateCode"",ru.""Email"" as RequestedByUserName,pet.""CC"" as CC,pet.""BCC"" as BCC,nt.""Id"",nt.""ParentTaskId"",tl.""Code"" as ""StatusCode""
 ,pet.""To"" as To,pet.""From"" as From,nt.""Id"" as key,'' as title,true as lazy
from public.""NtsTask"" as nt 
join email as em on em.""Id""=nt.""ParentTaskId""
join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false
left join public.""User"" as u on u.""Id""=nt.""AssignedToUserId"" and u.""IsDeleted""=false
left join public.""User"" as ou on ou.""Id""=nt.""OwnerUserId"" and ou.""IsDeleted""=false
left join public.""User"" as ru on ru.""Id""=nt.""RequestedByUserId"" and ru.""IsDeleted""=false
 left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false
where nt.""IsDeleted""=false
)select * from email
union 
	select nt.""TaskSubject"" as TaskSubject,TO_CHAR(nt.""StartDate"", 'MM/DD/YYYY HH12:MI:SS AM') as SentDate,nt.""TaskDescription"" as TaskDescription,nt.""AssignedToUserId"",
u.""Email"" as AssignedToUserName,ou.""Email"" as OwnerUserName,nt.""TemplateCode"",ru.""Email"" as RequestedByUserName,pet.""CC"" as CC,pet.""BCC"" as BCC,nt.""Id"",nt.""ParentTaskId"",tl.""Code"" as ""StatusCode""
 ,pet.""To"" as To,pet.""From"" as From,nt.""Id"" as key,'' as title,true as lazy
from public.""NtsTask"" as nt 
join parentemail as em on em.""ParentTaskId""=nt.""Id"" 
join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false
left join public.""User"" as u on u.""Id""=nt.""AssignedToUserId"" and u.""IsDeleted""=false
left join public.""User"" as ou on ou.""Id""=nt.""OwnerUserId"" and ou.""IsDeleted""=false
left join public.""User"" as ru on ru.""Id""=nt.""RequestedByUserId"" and ru.""IsDeleted""=false
 left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false
	where nt.""TemplateCode""='EMAIL_TASK' and nt.""IsDeleted""=false
)select * from parentemail";
            }
            else
            {
                query1 = $@"with recursive email as(
            select nt.""TaskSubject"" as TaskSubject,TO_CHAR(nt.""StartDate"", 'MM/DD/YYYY HH12:MI:SS AM') as SentDate,nt.""TaskDescription"" as TaskDescription,nt.""AssignedToUserId"",
            u.""Email"" as AssignedToUserName,ou.""Email"" as OwnerUserName,ru.""Email"" as RequestedByUserName,pet.""CC"" as CC,pet.""BCC"" as BCC,nt.""Id""
            ,case when pt.""TemplateCode""='EMAIL_TASK' then nt.""ParentTaskId"" else null end as ""ParentTaskId"",tl.""Code"" as ""StatusCode""
            ,pet.""To"" as To,pet.""From"" as From,nt.""Id"" as key,'' as title,true as lazy
            from public.""NtsTask"" as nt
            join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
            join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false
            left join public.""User"" as u on u.""Id""=nt.""AssignedToUserId"" and u.""IsDeleted""=false
            left join public.""User"" as ou on ou.""Id""=nt.""OwnerUserId"" and ou.""IsDeleted""=false
            left join public.""User"" as ru on ru.""Id""=nt.""RequestedByUserId"" and ru.""IsDeleted""=false
            left join public.""NtsTask"" as pt on pt.""Id""=nt.""ParentTaskId"" and pt.""IsDeleted""=false
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId""  and tl.""IsDeleted""=false 
            where 
            	(nt.""ReferenceId""='{refId}' and 
            	nt.""TemplateCode""='EMAIL_TASK') and nt.""IsDeleted""=false
            union all
            select nt.""TaskSubject"" as TaskSubject,TO_CHAR(nt.""StartDate"", 'MM/DD/YYYY HH12:MI:SS AM') as SentDate,nt.""TaskDescription"" as TaskDescription,nt.""AssignedToUserId"",
            u.""Email"" as AssignedToUserName,ou.""Email"" as OwnerUserName,ru.""Email"" as RequestedByUserName,pet.""CC"" as CC,pet.""BCC"" as BCC,nt.""Id"",nt.""ParentTaskId"",tl.""Code"" as ""StatusCode""
 ,pet.""To"" as To,pet.""From"" as From  ,nt.""Id"" as key,'' as title,true as lazy          
from public.""NtsTask"" as nt 
            join email as em on em.""Id""=nt.""ParentTaskId""
            join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
            join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false
            left join public.""User"" as u on u.""Id""=nt.""AssignedToUserId"" and u.""IsDeleted""=false
            left join public.""User"" as ou on ou.""Id""=nt.""OwnerUserId"" and ou.""IsDeleted""=false
            left join public.""User"" as ru on ru.""Id""=nt.""RequestedByUserId"" and ru.""IsDeleted""=false
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false
            where nt.""IsDeleted""=false
            )select * from email
                union
select nt.""TaskSubject"" as TaskSubject,TO_CHAR(nt.""StartDate"", 'MM/DD/YYYY HH12:MI:SS AM') as SentDate,nt.""TaskDescription"" as TaskDescription,nt.""AssignedToUserId"",
            u.""Email"" as AssignedToUserName,ou.""Email"" as OwnerUserName,ru.""Email"" as RequestedByUserName,pet.""CC"" as CC,pet.""BCC"" as BCC,nt.""Id"",nt.""ParentTaskId"",tl.""Code"" as ""StatusCode""
 ,pet.""To"" as To,pet.""From"" as From  ,nt.""Id"" as key,'' as title,true as lazy          
from public.""NtsTask"" as nt 
            join email as em on em.""ParentTaskId""=nt.""Id"" 
            join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
            join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false
            left join public.""User"" as u on u.""Id""=nt.""AssignedToUserId"" and u.""IsDeleted""=false
            left join public.""User"" as ou on ou.""Id""=nt.""OwnerUserId"" and ou.""IsDeleted""=false
            left join public.""User"" as ru on ru.""Id""=nt.""RequestedByUserId"" and ru.""IsDeleted""=false
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false
            where nt.""IsDeleted""=false



            ";
            }

            var queryData1 = await _queryRepoEmailTask.ExecuteQueryList(query1, null);
            return queryData1;
        }

        public async Task<IList<IdNameViewModel>> GetActivePersonList()
        {
            var query = $@"SELECT p.""Id"" as Id, CONCAT  (p.""FirstName"", ' ', p.""LastName"") as Name FROM cms.""N_CoreHR_HRPerson"" as p where  p.""PersonLegalEntityId"" ='{_repo.UserContext.LegalEntityId}' and p.""IsDeleted""=false ORDER BY ""Id"" ASC ";
            return await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
        }
        public async Task<IList<IdNameViewModel>> GetPayrollElementList()
        {
            var query = $@"SELECT p.""Id"" as Id, p.""ElementName"" as Name FROM cms.""N_PayrollHR_PayrollElement"" as p where   p.""IsDeleted""=false ORDER BY ""LastUpdatedDate"" ASC ";
            return await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
        }

        public async Task<TestViewModel> Test()
        {
            var query = @$"select true as ""Bool"",'wwe' as ""Url"",null as ""DateTime"",'12:24:02' as ""TimeSpan"",3 as ""DataActionEnum"",24 as ""Long"",3.25 as ""Double"",'new' as ""Url""";
            return await _queryRepo.ExecuteQuerySingle<TestViewModel>(query, null);
        }

        public async Task<IList<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userRoleCodes, string inboxCode)
        {
            var expObj = new List<TreeViewViewModel>();

            var list = new List<TreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');


                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX"
                };
                //if (count != null)
                //{
                //    item.Name = item.DisplayName = $"Inbox ({count + countjd + counthr})";
                //}
                list.Add(item);

            }
            else if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');

                query = @$"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || COALESCE(t.""Count"",0)||')'  as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='{inboxCode}' and usp.""IsDeleted""=false
left join(
	               select ur.""Id"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='CMS_SEBI_INT'
	               join public.""Template"" as tmp on tmp.""Code"" = usp.""TemplateCode""
				   join public.""NtsTask"" as task on task.""TemplateId"" =tmp.""Id"" 
					 join public.""LOV"" as lv on lv.""Id"" =task.""TaskStatusId"" 
	                
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
	                where  
					task.""AssignedToUserId"" = '{_repo.UserContext.UserId}'  and 
					lv.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false  and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false 
	                group by ur.""Id""  
                ) t on ur.""Id""=t.""Id""
				
where ur.""Id"" in ({roleText})";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                var obj = expObj.Where(x => x.Type == "USERROLE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "USERROLE")
            {

                query = $@"Select usp.""InboxStageName"" ||' (' || COALESCE(t.""Count"",0)||')'  as Name
                ,usp.""InboxStageName"" as id, '{id}' as ParentId, 'STAGE' as Type,
                true as hasChildren,
'{userRoleId}' as UserRoleId
--ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='CMS_SEBI_INT'
and usp.""IsDeleted""=false and ur.""Id""='{userRoleId}'
left join(
	               select ur.""Id"",usp.""InboxStageName"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='CMS_SEBI_INT'
	               join public.""Template"" as tmp on tmp.""Code"" = usp.""TemplateCode""
				   join public.""NtsTask"" as task on task.""TemplateId"" =tmp.""Id"" 
					 join public.""LOV"" as lv on lv.""Id"" =task.""TaskStatusId"" 
	                
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
	                where  
					task.""AssignedToUserId"" = '{_repo.UserContext.UserId}'  and   ur.""Id""='{userRoleId}' and

                    lv.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false  and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false 
	                group by ur.""Id"",usp.""InboxStageName""  
                ) t on usp.""InboxStageName""=t.""InboxStageName""
			
where ur.""Id""='{userRoleId}' group by usp.""InboxStageName"",t.""Count"" ";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

            }
            else if (type == "STAGE")
            {

                query = $@"Select usp.""TemplateShortName""  ||' (' || COALESCE(t.""Count"",0)||')'  as Name,
                coalesce(usp.""TemplateCode"", usp.""TemplateName"") as id, '{id}' as ParentId,
                 'TEMPLATE' as Type,'{userRoleId}' as UserRoleId,
                case when coalesce(usp.""TemplateCode"", usp.""TemplateName"")= 'NotSelected' then false else true end as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='{inboxCode}' 
left join(
	               select tmp.""Code"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='CMS_SEBI_INT'
	               join public.""Template"" as tmp on tmp.""Code"" = usp.""TemplateCode""
				   join public.""NtsTask"" as task on task.""TemplateId"" =tmp.""Id"" 
					 join public.""LOV"" as lv on lv.""Id"" =task.""TaskStatusId"" 
	                
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
	                where  
	ur.""Id"" = '{userRoleId}'
				and	task.""AssignedToUserId"" = '{_repo.UserContext.UserId}'  
	and 
					lv.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE') 
	and
                     ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false  and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and usp.""InboxStageName"" = '{id}'   
	                group by tmp.""Code""  
	) t on coalesce(usp.""TemplateCode"", usp.""TemplateName"")=t.""Code""
	
	

where 
				 ur.""Id"" = '{userRoleId}'  and usp.""InboxStageName"" = '{id}'  order by usp.""CreatedDate"" ";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



            }
            else if (type == "TEMPLATE")
            {

                //query = $@"select 'STATUS' as Type,s.""StatusLabel"" as Name,
                //'{parentId}' as StageId,s.""StatusCode"" as StatusCode,'{id}' as ParentId,
                //false as hasChildren, '{userRoleId}' as UserRoleId
                //FROM public.""UserRoleStageChild"" as s where s.""InboxStageId"" = '{parentId}'";
                query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)||')'  as Name,
                '{parentId}' as StageId,s.""StatusCode"" as StatusCode,'{id}' as ParentId,
                false as hasChildren, '{userRoleId}' as UserRoleId
                FROM public.""UserRoleStageChild"" as s 
join public.""UserRoleStageParent"" as usp on usp.""Id""=s.""InboxStageId"" and usp.""IsDeleted"" = false and usp.""UserRoleId""='{userRoleId}'
left join(
	                select case when lv.""Code""='TASK_STATUS_OVERDUE' then 'TASK_STATUS_INPROGRESS' else lv.""Code"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
	                FROM 
                   
	                public.""Template"" as tmp 
				   join public.""NtsTask"" as task on task.""TemplateId"" =tmp.""Id"" 
					 join public.""LOV"" as lv on lv.""Id"" =task.""TaskStatusId"" 
	                
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
	                where  
					task.""AssignedToUserId"" = '{_repo.UserContext.UserId}'  and 
					--lv.""Code"" in (s.""StatusCode"") and
                     task.""IsDeleted""=false  and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false  and tmp.""Code""='{id}' 
	                group by TaskStatusCode  
                ) t on t.""taskstatuscode""=ANY(s.""StatusCode"") 
                where s.""IsDeleted""=false and usp.""TemplateCode"" = '{id}' order by s.""CreatedDate"" ";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            }

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


            var list = new List<TreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX",
                };

                list.Add(item);

            }
            else if (id == "INBOX")
            {
                query = $@"select t.""DisplayName"" || ' (' || Count(st.*) || ')' as Name, t.""Id"" as Id,true as hasChildren, 'SERVICETEMPLATE' as Type, 'INBOX' as ParentId
                            from public.""NtsService"" as s 
                            left join public.""Template"" as t on t.""Code"" = s.""TemplateCode""
                            left join public.""NtsService"" as ch on ch.""ServicePlusId"" = s.""Id""
                            left join public.""NtsTask"" as st on st.""ParentServiceId"" = ch.""Id"" 
                            where s.""TemplateCode"" in ('{templateCode}')
                            group by t.""DisplayName"", t.""Id""";
                var res = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                list.AddRange(res);
            }
            else if (type == "SERVICETEMPLATE")
            {
                query = $@"select case when s.""ServiceSubject"" <> null then s.""ServiceSubject"" else t.""DisplayName"" end  || ' (' || Count(st.*) || ')' as Name, s.""Id"" as Id,true as hasChildren, 'SERVICEPLUS' as Type, '{id}' as ParentId
                            from public.""NtsService"" as s 
                            left join public.""Template"" as t on t.""Code"" = s.""TemplateCode""
                            left join public.""NtsService"" as ch on ch.""ServicePlusId"" = s.""Id""
                            left join public.""NtsTask"" as st on st.""ParentServiceId"" = ch.""Id"" 
                            where s.""TemplateCode"" in ('{templateCode}')
                            group by s.""ServiceSubject"", s.""Id"", t.""DisplayName""";
                var res = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                list.AddRange(res);
            }

            else if (type == "SERVICEPLUS")
            {
                query = $@"select case when ch.""ServiceSubject"" <> null then ch.""ServiceSubject"" else t.""DisplayName"" end  || ' (' || Count(st.*) || ')' as Name, ch.""Id"" as Id,true as hasChildren, 'SERVICE' as Type,  '{id}' as ParentId
                            from 
                            public.""NtsService"" as ch
                            left join public.""Template"" as t on t.""Code"" = ch.""TemplateCode""
                            left join public.""NtsTask"" as st on st.""ParentServiceId"" = ch.""Id"" 
                             where ch.""ServicePlusId"" = '{id}'
                            group by ch.""ServiceSubject"", ch.""Id"", t.""DisplayName"" ";
                var res = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                list.AddRange(res);
            }

            else if (type == "SERVICE")
            {
                query = $@"select st.""TaskSubject""  as Name, st.""Id"" as Id ,false as hasChildren, 'STEPTASK' as Type,  '{id}' as ParentId
                from
                public.""NtsTask"" as st where st.""ParentServiceId"" = '{id}'
                group by st.""TaskSubject"", st.""Id"" ";
                var res = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                list.AddRange(res);
            }


            foreach (var item in list)
            {

                item.children = true;
                item.text = item.Name;
                item.parent = item.ParentId == null ? "#" : item.ParentId;
                item.a_attr = new { data_id = item.id, data_name = item.Name, data_type = item.Type };
            }

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

        public async Task<List<TaskTemplateViewModel>> ReadInboxData(string id, string type, string templateCode,string userId=null)
        {
            var query = "";
            var res = new List<TaskTemplateViewModel>();
            //if (type == "INBOX" || type == "SERVICETEMPLATE")
            if (type == "INBOX" || type == "PROCESS" || type == "USERROLE")
            {
                query = $@"select st.*,ou.""Name"" as OwnerUserName,au.""Name"" as AssignedToUserName,lv.""Name"" as TaskStatusName
                            from public.""NtsService"" as s 
                            left join public.""Template"" as t on t.""Code"" = s.""TemplateCode"" and t.""IsDeleted""=false
                            left join public.""NtsService"" as ch on ch.""ServicePlusId"" = s.""Id""  and ch.""IsDeleted""=false
                            left join public.""NtsTask"" as st on st.""ParentServiceId"" = ch.""Id"" or st.""ParentServiceId"" = s.""Id""   and st.""IsDeleted""=false
                            join public.""User"" as ou ou.""Id""=st.""OwnerUserId"" and ou.""IsDeleted""=false
join public.""User"" as au au.""Id""=st.""AssignedToUserId"" and au.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=st.""TaskStatusId"" and lv.""IsDeleted""=false 
where s.""TemplateCode"" in ('{templateCode}') and s.""IsDeleted""=false  #ASSIGNEEWHERE#
                            group by st.""Id"",ou.""Name"",au.""Name"",lv.""Name""";
                var search = "";
                if (userId.IsNotNullAndNotEmpty())
                {
                    search = $@"and st.""AssignedToUserId""='{userId}'";
                }
                query = query.Replace("#ASSIGNEEWHERE#", search);
                res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);
            }
            else if (type == "PROCESSSTAGE")
            {
                query = $@"select st.*,ou.""Name"" as OwnerUserName,au.""Name"" as AssignedToUserName,lv.""Name"" as TaskStatusName
                            from 
                            public.""NtsService"" as ch
                            left join public.""NtsTask"" as st on st.""ParentServiceId"" = ch.""Id""   or st.""ParentServiceId"" = '{id}'
join public.""User"" as ou ou.""Id""=st.""OwnerUserId"" and ou.""IsDeleted""=false
join public.""User"" as au au.""Id""=st.""AssignedToUserId"" and au.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=st.""TaskStatusId"" and lv.""IsDeleted""=false  
where ch.""ServicePlusId"" = '{id}'  #ASSIGNEEWHERE#
                             group by st.""Id"",ou.""Name"",au.""Name"",lv.""Name"" ";
                var search = "";
                if (userId.IsNotNullAndNotEmpty())
                {
                    search = $@"and st.""AssignedToUserId""='{userId}'";
                }
                query = query.Replace("#ASSIGNEEWHERE#", search);
                res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);

            }
            else if (type == "STAGE")
            {
                query = $@"select st.*,ou.""Name"" as OwnerUserName,au.""Name"" as AssignedToUserName,lv.""Name"" as TaskStatusName
                from
                public.""NtsTask"" as st 
join public.""User"" as ou on ou.""Id""=st.""OwnerUserId"" and ou.""IsDeleted""=false
join public.""User"" as au on au.""Id""=st.""AssignedToUserId"" and au.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=st.""TaskStatusId"" and lv.""IsDeleted""=false 
where st.""ParentServiceId"" = '{id}' #ASSIGNEEWHERE#
                 group by st.""Id"",ou.""Name"",au.""Name"",lv.""Name"" ";
                var search = "";
                if (userId.IsNotNullAndNotEmpty())
                {
                    search = $@"and st.""AssignedToUserId""='{userId}'";
                }
                query = query.Replace("#ASSIGNEEWHERE#", search);
                res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);

            }
            else if (type == "STEPTASK")
            {
                query = $@"select st.*,ou.""Name"" as OwnerUserName,au.""Name"" as AssignedToUserName,lv.""Name"" as TaskStatusName
                from
                public.""NtsTask"" as st 
join public.""User"" as ou on ou.""Id""=st.""OwnerUserId"" and ou.""IsDeleted""=false
join public.""User"" as au on au.""Id""=st.""AssignedToUserId"" and au.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=st.""TaskStatusId"" and lv.""IsDeleted""=false 
where st.""Id"" = '{id}'  #ASSIGNEEWHERE#
                 group by st.""Id"",ou.""Name"",au.""Name"",lv.""Name"" ";
                var search = "";
                if (userId.IsNotNullAndNotEmpty())
                {
                    search = $@"and st.""AssignedToUserId""='{userId}'";
                }
                query = query.Replace("#ASSIGNEEWHERE#", search);
                res = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);

            }            
                return res.Where(x => x.Id.IsNotNullAndNotEmpty()).ToList();           
        }
        public async Task<IList<TASTreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TASTreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TASTreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    projectId = obj.ProjectId;
                    stageId = obj.StageId;
                }
            }

            var list = new List<TASTreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');
                query = $@"  
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",s.""ParentServiceId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						     and usp.""UserRoleId"" in ({roleText}) where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""Id"" as ""ParentServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId"" 

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level from NtsService  where ""ParentServiceId"" is not null


                 union all

                 select t.""Id"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""                       
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                             where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                    )
                 SELECT count(""Id"") from Nts where Level='Child' ";
                var count = await _queryRepo.ExecuteScalar<long?>(query, null);

                var item = new TASTreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX"
                };
                if (count != null)
                {
                    item.Name = item.DisplayName = $"Inbox ({count })";
                }
                list.Add(item);

            }
            else if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');
                query = $@"Select distinct ur.""Id"" as UserRoleId,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,usp.""InboxStageName"" as id,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                left join(

                 WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",s.""ParentServiceId"",usp.""UserRoleId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
						     and usp.""UserRoleId"" in ({roleText}) 
                       where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					   
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""Id"" as ""ParentServiceId"",ns.""UserRoleId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId"" 

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""UserRoleId""  from NtsService where ""ParentServiceId"" is not null


                 union all

                 select t.""Id"",'Child',nt.""UserRoleId"" as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id"" 
 join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                     where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'        
                    )
                 SELECT count(""Id"") as ""Count"",""UserRoleId"" from Nts where Level='Child' group by ""UserRoleId""

						
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "PROCESSSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }
            }
            else if (type == "USERROLE")
            {
                query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{id}' as ParentId,                
                'PROCESS' as Type,'{userRoleId}' as UserRoleId,
                true as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                left join(

                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",s.""ParentServiceId"",usp.""TemplateCode""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
						     and usp.""UserRoleId"" = '{userRoleId}' where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""Id"" as ""ParentServiceId"",ns.""TemplateCode"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""TemplateCode""  from NtsService where ""ParentServiceId"" is not null


                 union all

                 select t.""Id"",'Child' as Level,nt.""TemplateCode"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
 join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                      where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'       
                    )

	          
                    SELECT count(""Id"") as ""Count"",""TemplateCode"" from Nts where Level='Child' group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "PROCESS").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PROCESS")
            {
                query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,
                true as hasChildren,s.""Id"" as ProjectId,
                '{id}' as ParentId,'PROCESSSTAGE' as Type
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",s.""ParentServiceId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    
						     where tt.""Code""='{id}' and (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", ns.""ServiceId"",ns.""Id"" as ""ParentServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService where ""ParentServiceId"" is not null


                 union all

                 select t.""Id"",'Child' as Level,nt.""ServiceId"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id"" 
 join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                           where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'  
                    )

	           
                    SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{id}' and (s.""OwnerUserId"" = '{userId}' or s.""RequestedByUserId""='{userId}') and t.""Count""!=0 and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "PROCESSSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "PROCESSSTAGE")
            {
                query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                    s.""Id"" as id,
                    false as hasChildren
                    FROM public.""NtsService"" as s
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId""
                    left join(
                    WITH RECURSIVE Nts AS(

                     WITH RECURSIVE NtsService AS(
                     SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s


            where s.""ParentServiceId""='{id}' and s.""IsDeleted""=false

                            union all
                            SELECT s.""Id"", ns.""ServiceId"" FROM public.""NtsService"" as s
                            join NtsService ns on s.""ParentServiceId""=ns.""Id""

                         join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false

                     )
                     SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService


                     union all

                     select t.""Id"",'Child' as Level,nt.""ServiceId"" 
                         FROM  public.""NtsTask"" as t
                            join Nts as nt on t.""ParentServiceId"" =nt.""Id""  and t.""IsDeleted""=false

                        )

                        SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""

                    ) t on s.""Id""=t.""ServiceId""
                    where s.""ParentServiceId""='{id}' and lv.""Code"" <> 'SERVICE_STATUS_DRAFT'
                    order by s.""SequenceOrder"" asc";

                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);
                var obj = expObj.Where(x => x.Type == "STAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }
            }
            else
            {


            }
            return list;
        }
        public async Task<IList<TASTreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TASTreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TASTreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    projectId = obj.ProjectId;
                    stageId = obj.StageId;
                }
            }

            var list = new List<TASTreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');
                query = $@"  
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						     and usp.""UserRoleId"" in ({roleText}) and s.""IsDeleted""=false
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""                       
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'    
                    )
                 SELECT count(""Id"") from Nts where Level='Child' ";
                var count = await _queryRepo.ExecuteScalar<long?>(query, null);

                var item = new TASTreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX"
                };
                if (count != null)
                {
                    item.Name = item.DisplayName = $"Inbox ({count })";
                }
                list.Add(item);

            }
            else if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');
                query = $@"Select distinct ur.""Id"" as UserRoleId,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,usp.""InboxStageName"" as id,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' and ur.""IsDeleted""=false
                left join(

                 WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""UserRoleId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						     and usp.""UserRoleId"" in ({roleText}) 
					  where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""UserRoleId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""UserRoleId""  from NtsService


                 union all

                 select t.""Id"",'Child',nt.""UserRoleId"" as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'          
                    )
                 SELECT count(""Id"") as ""Count"",""UserRoleId"" from Nts where Level='Child' group by ""UserRoleId""

						
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' 
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "PROCESSSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            //       else if (type == "USERROLE")
            //       {

            //           query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
            //           , usp.""InboxStageName"" as id, '{id}' as ParentId, 'PROJECTSTAGE' as Type,
            //           true as hasChildren, '{userRoleId}' as UserRoleId
            //           from public.""UserRole"" as ur
            //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'

            //            left join(
            //               WITH RECURSIVE Nts AS(

            //            WITH RECURSIVE NtsService AS(
            //            SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""InboxStageName""   FROM public.""NtsService"" as s
            //                    join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
            //	     where usp.""UserRoleId"" = '{userRoleId}' 

            //                   union all
            //                   SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""InboxStageName"" FROM public.""NtsService"" as s
            //                   join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

            //                join public.""User"" as u on s.""OwnerUserId""=u.""Id""

            //            )
            //            SELECT ""Id"",'Parent' as Level,""InboxStageName""  from NtsService


            //            union all

            //            select t.""Id"",'Child' as Level,nt.""InboxStageName"" 
            //                FROM  public.""NtsTask"" as t
            //                   join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
            //                  join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //                where t.""AssignedToUserId"" = '{userId}'           
            //               )

            //               SELECT count(""Id"") as ""Count"",""InboxStageName"" from Nts where Level='Child' group by ""InboxStageName""
            //) nt on usp.""InboxStageName""=nt.""InboxStageName""
            //           where ur.""Id"" = '{userRoleId}'
            //           Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
            //           order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

            //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



            //           var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
            //           if (obj.IsNotNull())
            //           {
            //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
            //               if (data.IsNotNull())
            //               {
            //                   data.expanded = true;
            //               }
            //           }

            //       }
            else if (type == "USERROLE")
            {
                query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{id}' as ParentId,                
                'PROCESS' as Type,'{userRoleId}' as UserRoleId,
                true as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
                left join(

                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""TemplateCode""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='TECProcess' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						     and usp.""UserRoleId"" = '{userRoleId}' 
					 where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""TemplateCode"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""TemplateCode""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""TemplateCode"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}'   and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'        
                    )

	          
                    SELECT count(""Id"") as ""Count"",""TemplateCode"" from Nts where Level='Child' group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' 
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "PROCESS").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PROCESS")
            {
                query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,
                true as hasChildren,s.""Id"" as ProjectId,
                '{id}' as ParentId,'PROCESSSTAGE' as Type
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'  
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						    
						     where tt.""Code""='{id}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
					 
                        union all
                        SELECT s.""Id"", ns.""ServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""ServiceId"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'         
                    )

	           
                    SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{id}' and  t.""Count""!=0 and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "PROCESSSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "PROCESSSTAGE")
            {
                query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                       s.""Id"" as id,
                       true as hasChildren
                       FROM public.""NtsService"" as s
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
                       left join(
                    WITH RECURSIVE NtsService AS ( 
                           SELECT distinct t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
                               FROM public.""NtsService"" as s                      
            	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false
            	where s.""ParentServiceId""='{id}' and t.""AssignedToUserId""='{userId}' and s.""IsDeleted""=false
            	union all
                               SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
                               FROM public.""NtsService"" as s
                               join NtsService ns on s.""ParentServiceId""=ns.""Id""
                               join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false
                            where t.""AssignedToUserId""='{userId}'  and s.""IsDeleted""=false     
                            --where s.""OwnerUserId""='{userId}'                        
                           )
                           SELECT count(ts) as ""Count"",""ServiceId"" from NtsService group by ""ServiceId""

                       ) t on s.""Id""=t.""ServiceId""
                       where s.""ParentServiceId""='{id}' and lv.""Code"" <>'SERVICE_STATUS_DRAFT' and s.""IsDeleted""=false
                       order by s.""SequenceOrder"" asc";

                list = await _queryRepo.ExecuteQueryList<TASTreeViewViewModel>(query, null);

            }
            else
            {
            }
            return list;
        }
    
        public async Task<string> GetLatestMigrationScript()
        {
            var query = $@"select ""MigrationId"" from public.""__EFMigrationsHistory"" 
                        order by left(""MigrationId"", strpos(""MigrationId"", '_') - 1) desc limit 1 ";
            var queryData = await _queryRepo.ExecuteScalar<string>(query,null);
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
                var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
                return error;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return ex.ToString();
            }

        }

    }
}
