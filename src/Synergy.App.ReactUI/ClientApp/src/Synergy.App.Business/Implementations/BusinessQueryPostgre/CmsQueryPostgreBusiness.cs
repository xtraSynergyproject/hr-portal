using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Data;
using Hangfire;

namespace Synergy.App.Business
{
    public class CmsQueryPostgreBusiness : BusinessBase<TemplateViewModel, Template>, ICmsQueryBusiness
    {
        private readonly IUserContext _userContext;
        private readonly IRepositoryQueryBase<TemplateViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;

        //private readonly IHangfireScheduler _hangfireScheduler;

        public CmsQueryPostgreBusiness(IRepositoryBase<TemplateViewModel, Template> repo
                    , IMapper autoMapper, IUserContext userContext
                    , IRepositoryQueryBase<TemplateViewModel> queryRepo
            , IServiceProvider serviceProvider
            //, IHangfireScheduler hangfireScheduler
            ) : base(repo, autoMapper)
        {
            _userContext = userContext;
            _queryRepo = queryRepo;
            _serviceProvider = serviceProvider;
            //_hangfireScheduler = hangfireScheduler;
        }

        #region CmsBusiness
        public async Task<bool> ManageTableExists(TableMetadataViewModel existingTableMetadata)
        {
            var query = @$"select exists (
	                        select 1
	                        from information_schema.{(existingTableMetadata.TableType == TableTypeEnum.Table ? "tables" : "views")} 
	                        where table_schema = '{existingTableMetadata.Schema}'
	                        and table_name = '{existingTableMetadata.Name}'
                        ) ";
            var exist = await _queryRepo.ExecuteScalar<bool>(query, null);
            return exist;
        }
        public async Task<bool?> ManageTableRecordExists(TableMetadataViewModel existingTableMetadata)
        {
            var query = @$"select true from  
                        {existingTableMetadata.Schema}.""{existingTableMetadata.Name}"" limit 1 ";

            var recordExists = await _queryRepo.ExecuteScalar<bool?>(query, null);
            return recordExists;
        }
        public async Task EditTableSchema(TableMetadataViewModel tableMetadata)
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
        public async Task CreateTableSchema(TableMetadataViewModel tableMetadata, bool dropTable)
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
        public async Task TableQueryExecute(string tableQuery)
        {
            await _queryRepo.ExecuteCommand(tableQuery, null);
        }
        public async Task<DataTable> GetQueryDataTable(string selectQuery)
        {
            var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
            return dt;
        }
        public async Task<DataTable> GetEditNoteTableExistColumn(TableMetadataViewModel tableMetadata)
        {
            var query = @$"select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}'";

            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetEditNoteTableConstraints(TableMetadataViewModel tableMetadata)
        {
            var query = @$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}' ";

            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetEditTaskTableExistColumn(TableMetadataViewModel tableMetadata)
        {
            var query = @$"select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}'";

            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetEditTaskTableConstraints(TableMetadataViewModel tableMetadata)
        {
            var query = @$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';";

            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetEditServiceTableExistColumn(TableMetadataViewModel tableMetadata)
        {
            var query = @$"select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}'";

            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetEditServiceTableConstraints(TableMetadataViewModel tableMetadata)
        {
            var query = @$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';";

            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetEditFormTableExistColumn(TableMetadataViewModel tableMetadata)
        {
            var query = @$" select column_name,data_type,is_nullable	   
            from information_schema.columns where table_schema = '{tableMetadata.Schema}'
	        and table_name = '{tableMetadata.Name}' ";
            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetEditFormTableConstraints(TableMetadataViewModel tableMetadata)
        {
            var query = @$"SELECT con.conname as conname
            FROM pg_catalog.pg_constraint con
            INNER JOIN pg_catalog.pg_class rel ON rel.oid = con.conrelid
            INNER JOIN pg_catalog.pg_namespace nsp ON nsp.oid = connamespace
            WHERE nsp.nspname = '{tableMetadata.Schema}' AND rel.relname = '{tableMetadata.Name}';";

            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<List<ColumnMetadataViewModel>> GetForeignKeyColumnByTableMetadata(TableMetadataViewModel tableMetaData)
        {
            var query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
                ,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
                ,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false
                where c.""TableMetadataId""='{tableMetaData.Id}'
                and c.""IsForeignKey""=true and c.""IsDeleted""=false";

            var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return result;

        }
        public async Task<DataTable> GetDataByColumn(ColumnMetadataViewModel column, object columnValue, TableMetadataViewModel tableMetaData, string excludeId)
        {

            var selectQuery = @$"select * from {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" where  ""IsDeleted""=false and ""{column.Name}""='{columnValue}'";
            if (excludeId.IsNotNullAndNotEmpty())
            {
                selectQuery = @$"{selectQuery} and ""Id""<>'{excludeId}'";
            }
            selectQuery = @$"{selectQuery} limit 1";
            var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
            return dt;
        }
        public async Task DeleteFrom(FormTemplateViewModel model, TableMetadataViewModel tableMetaData)
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
        public async Task<List<EmailTaskViewModel>> ReadEmailTaskData(string refId, ReferenceTypeEnum refType, TaskViewModel taskdetails)
        {
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

            var queryData = await _queryRepo.ExecuteQueryList<EmailTaskViewModel>(query1, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetActivePersonList()
        {
            var query = $@"SELECT p.""Id"" as Id, CONCAT  (p.""FirstName"", ' ', p.""LastName"") as Name 
                        FROM cms.""N_CoreHR_HRPerson"" as p where  p.""PersonLegalEntityId"" ='{_repo.UserContext.LegalEntityId}' 
                        and p.""IsDeleted""=false ORDER BY ""Id"" ASC ";

            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetPayrollElementList()
        {
            var query = $@"SELECT p.""Id"" as Id, p.""ElementName"" as Name ,p.""ElementClassification"" as Code
                            FROM cms.""N_PayrollHR_PayrollElement"" as p 
                            where   p.""IsDeleted""=false ORDER BY ""LastUpdatedDate"" ASC ";

            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<TestViewModel> Test()
        {
            var query = @$"select true as ""Bool"",'wwe' as ""Url"",null as ""DateTime"",'12:24:02' as ""TimeSpan"",3 as ""DataActionEnum"",24 as ""Long"",3.25 as ""Double"",'new' as ""Url""";
            var result = await _queryRepo.ExecuteQuerySingle<TestViewModel>(query, null);
            return result;
        }
        public async Task<List<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userRoleCodes, string inboxCode)
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
                    ask.""AssignedToUserId"" = '{_repo.UserContext.UserId}'  and   ur.""Id""='{userRoleId}' and
                    
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
        public async Task<List<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string templateCode)
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
        public async Task<List<TaskTemplateViewModel>> ReadInboxData(string id, string type, string templateCode, string userId = null)
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
            var list = res.Where(x => x.Id.IsNotNullAndNotEmpty()).ToList();
            return list;
        }
        public async Task<List<TASTreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
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
        public async Task<List<TASTreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
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
            var queryData = await _queryRepo.ExecuteScalar<string>(query, null);
            return queryData;
        }

        public async Task<List<string>> GetAllMigrationsList()
        {
            var query = $@"select ""MigrationId"" from public.""__EFMigrationsHistory"" 
                        order by left(""MigrationId"", strpos(""MigrationId"", '_') - 1) ";
            var queryData = await _queryRepo.ExecuteScalarList<string>(query, null);
            return queryData;
        }



        public async Task<TemplateViewModel> ExecuteMigrationScript(string script)
        {
            var exScript = script;
            //var exScript = script.Replace("\"","\"\"");
            var query = $@" " + exScript + " ";
            var queryData = await _queryRepo.ExecuteQuerySingle<TemplateViewModel>(query, null);
            return queryData;
        }

        public async Task<List<TemplateViewModel>> GetTemplateListByTemplateType(int i)
        {
            var query = $@"select t.""Id"" as TemplateId, t.""TemplateCategoryId"", t.""Name"", t.""Code"", c.""Name"" as TemplateCategoryName
                            from public.""Template"" as t 
                            join public.""TemplateCategory"" as c on t.""TemplateCategoryId"" = c.""Id""
                            where t.""TemplateType"" = '{i}' and t.""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return queryData;
        }
        #endregion




        #region TemplateBusiness
        public async Task<List<TemplateViewModel>> GetTemplateDeleteList()
        {
            var query = @$"select t.*
                        from public.""Template"" as t
                        where t.""Id"" not in (select ""UdfTemplateId"" from public.""Template"" where ""UdfTemplateId"" is not null ) ";

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return list;
        }
        public async Task SetOcrTemplateFileId(string templateId, string fileId)
        {
            var query = @$"update public.""NoteTemplate"" 
                        set ""OcrTemplateFileId"" = '{fileId}'
                        where ""TemplateId"" = '{templateId}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<List<TemplateViewModel>> GetTemplateServiceListbyTeam(string tCode, string teamId)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""Code""='{tCode}'
                        join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""DefaultOwnerTeamId""='{teamId}'
                        where t.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return list;
        }

        public async Task<List<WorkflowViewModel>> GetTemplateDetail(string id)
        {

            string query = @$"select distinct t.""Id"" as Id,t.""DisplayName"" as Subject, t.""Code"" as TemplateCode, t.""Id"" as TemplateId,
                                    t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, null as StatusId, null as StatusName,
                                null as AssignedToUserId, null as AssignedToUserName,
                                '' as StartDate,'' as DueDate, null as ParentId, null as ComponentId, 'Service' as Type
                                from  public.""Template"" as t --on t.""Id"" = s.""TemplateId""
                                left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where t.""Id"" ='{id}' and t.""IsDeleted"" = false";
            var list = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);
            return list;
        }

        public async Task<List<WorkflowViewModel>> GetWorkflowDiagramDetailsByTemplate(string id)
        {
            var query = @$"select distinct stc.""Id"" as Id,t.""DisplayName"" as Subject,t.""Code"" as TemplateCode, stc.""TaskTemplateId"" as TemplateId,
                                t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, null as StatusId, null as StatusName,
                                null as AssignedToUserId, null as AssignedToUserName,
                                '' as StartDate,'' as DueDate, cp.""ParentId"" as ParentId, c.""Id"" as ComponentId, 'Task' as Type
                                --,case when ""c"".""ParentId"" <> ""cp"".""ParentId""
                                --then CONCAT(""c"".""ParentId"",',' ,""cp"".""ParentId"")
                                --else ""c"".""ParentId"" end as ""ParentIds""  
, stc.""SequenceOrder"" as SequenceOrder
from public.""Template"" as st
 left join public.""ProcessDesign"" as pd on  st.""Id"" = pd.""TemplateId"" and pd.""IsDeleted"" = false
  left join public.""Component"" as c on pd.""Id"" = c.""ProcessDesignId"" and c.""IsDeleted"" = false
left join public.""ComponentParent"" as cp on c.""Id""=cp.""ComponentId"" and cp.""IsDeleted"" = false                      
 left join public.""ComponentResult"" as cr  on c.""Id""=cr.""ComponentId""   and cr.""IsDeleted"" = false   
                                left join public.""NtsTask"" as task on task.""Id"" = cr.""NtsTaskId"" and task.""IsDeleted"" = false
                                left join public.""LOV"" as lov on lov.""Id"" = task.""TaskStatusId"" and lov.""IsDeleted"" = false
                                left join public.""User"" as u on u.""Id"" = task.""AssignedToUserId"" and u.""IsDeleted"" = false
                                join public.""StepTaskComponent"" as stc on stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted"" = false
                                 join public.""Template"" as t on t.""Id"" = stc.""TemplateId"" and t.""IsDeleted"" = false
                               left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where  c.""ComponentType"" = 4 and st.""Id"" ='{id}' and st.""IsDeleted""= false 
								order by stc.""SequenceOrder""";

            var step = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);
            return step;

        }

        public async Task<List<WorkflowViewModel>> GetServiceDetail(string id)
        {
            string query = @$"select distinct s.""Id"" as Id,s.""ServiceSubject"" as Subject, t.""Code"" as TemplateCode, s.""TemplateId"" as TemplateId,
                                    t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, s.""ServiceStatusId"" as StatusId, lov.""Name"" as StatusName,
                                u.""Id"" as AssignedToUserId, u.""Name"" as AssignedToUserName,
                                s.""StartDate"" as StartDate,s.""DueDate"" as DueDate, null as ParentId, null as ComponentId, 'Service' as Type
                                from public.""NtsService"" as s  
                                left join public.""LOV"" as lov on lov.""Id"" = s.""ServiceStatusId"" and lov.""IsDeleted"" = false
                                left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted"" = false
                                left join public.""Template"" as t on t.""Id"" = s.""TemplateId"" and t.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where s.""Id"" ='{id}' and s.""IsDeleted"" = false";
            var list = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);
            return list;
        }

        public async Task<List<WorkflowViewModel>> GetWorkflowDiagramDetails(string id)
        {
            var query = @$"select distinct case when cr.""NtsTaskId"" is not null then cr.""NtsTaskId"" else t.""Id"" end as Id,stc.""Subject"" as Subject,t.""Code"" as TemplateCode, stc.""TaskTemplateId"" as TemplateId,
                                t.""DisplayName"" as TemplateName, t.""TemplateStageId"" as StageId, t.""TemplateStepId"" as StepId, ts.""Name"" as StageName,
                                ts1.""Name"" as StepName, task.""TaskStatusId"" as StatusId, lov.""Name"" as StatusName,
                                u.""Id"" as AssignedToUserId, u.""Name"" as AssignedToUserName,
                                task.""StartDate"" as StartDate,task.""DueDate"" as DueDate, c.""ParentId"" as ParentId, c.""Id"" as ComponentId, 'Task' as Type
                               , case when ""c"".""ParentId"" <> ""cp"".""ParentId""
                                then CONCAT(""c"".""ParentId"",',' ,""cp"".""ParentId"")
                                else ""c"".""ParentId"" end as ""ParentIds""  , stc.""SequenceOrder"" as SequenceOrder
                                from public.""ComponentResult"" as cr  
                                left join public.""Component"" as c on c.""Id"" = cr.""ComponentId"" and c.""IsDeleted"" = false
                                left join public.""ComponentParent"" as cp on ""c"".""Id""=cp.""ComponentId""                                
                                left join public.""NtsTask"" as task on task.""Id"" = cr.""NtsTaskId"" and task.""IsDeleted"" = false
                                left join public.""LOV"" as lov on lov.""Id"" = task.""TaskStatusId"" and lov.""IsDeleted"" = false
                                left join public.""User"" as u on u.""Id"" = task.""AssignedToUserId"" and u.""IsDeleted"" = false
                                left join public.""StepTaskComponent"" as stc on stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted"" = false
                                left join public.""Template"" as t on t.""Id"" = stc.""TemplateId"" and t.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts on ts.""Id"" = t.""TemplateStageId"" and ts.""IsDeleted"" = false
                                left join public.""TemplateStage"" as ts1 on ts1.""Id"" = t.""TemplateStepId"" and ts1.""IsDeleted"" = false
                                where  c.""ComponentType"" = 4 and cr.""NtsServiceId"" ='{id}' and cr.""IsDeleted"" = false order by stc.""SequenceOrder""";

            var step = await _queryRepo.ExecuteQueryList<WorkflowViewModel>(query, null);
            return step;
        }

        public async Task<List<TemplateViewModel>> GetTemplateServiceList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null, ServiceTypeEnum? serviceType = null, string groupCodes = null)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"",t.""SequenceOrder"", tc.""Code"" as CategoryCode, st.""IconFileId"", ct.""IconFileId"" as ""CustomIcon"", t.""Code"",
                        ct.""ActionName"",ct.""ControllerName"",ct.""AreaName"",ct.""Parameter"",t.""ViewType"" as ViewType,st.""ServiceTemplateType""
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""TemplateCategoryType""={(int)categoryType}
                        and tc.""IsDeleted""=false 
                        left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false 
                        left join public.""CustomTemplate"" as ct on ct.""TemplateId""=t.""Id"" and ct.""IsDeleted""=false
                        left join public.""Module"" as m on m.""Id""=tc.""ModuleId"" and m.""IsDeleted""=false
                        join public.""Portal"" as p on t.""PortalId""=p.""Id"" and p.""IsDeleted""=false #PORTALWHERE#
                        where t.""IsDeleted""=false and  (t.""TemplateType"" =6 or t.""TemplateType"" =7) 
                        #servicetypewhere#
                        --and (""ServiceTemplateType""=1 or ""ServiceTemplateType"" is null)
                        --and t.""PortalId""='{_repo.UserContext.PortalId}' 
                        #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# #categoryIdSearch# #GROUPCODEWHERE# ";

            var serTypeWhere = "";
            if (serviceType.IsNotNull())
            {
                if (serviceType == ServiceTypeEnum.StandardService)
                {
                    serTypeWhere = $@"and (""ServiceTemplateType""=1 or ""ServiceTemplateType"" is null)";
                }
                else
                {
                    serTypeWhere = $@"and (""ServiceTemplateType""={(int)serviceType})";
                }
            }
            query = query.Replace("#servicetypewhere#", serTypeWhere);

            var portalwhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalwhere = $@" and p.""Name"" in ('{portalNames.Replace(",", "','")}') ";
            }
            else
            {
                portalwhere = $@" and p.""Id""='{_repo.UserContext.PortalId}'";
            }
            query = query.Replace("#PORTALWHERE#", portalwhere);

            var templatecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);
            var modulecodeSearch = "";
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);

            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);
            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }

            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var groupCodeWhere = groupCodes.IsNotNullAndNotEmpty() ? $@" and t.""GroupCode"" in ('{groupCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#GROUPCODEWHERE#", groupCodeWhere);


            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);

            return list;
        }
        public async Task<List<TemplateViewModel>> GetTemplateListByTaskTemplate(string taskTemplateId)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId"", tt.""TaskTemplateType"" as TaskType
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                        join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false 
                        #WHERE# ";
            var where = @$" WHERE t.""IsDeleted""=false ";
            if (taskTemplateId.IsNotNullAndNotEmpty())
            {
                var adhocItems = taskTemplateId.Split(',');
                var adhocText = "";
                foreach (var item in adhocItems)
                {
                    adhocText = $"{adhocText},'{item}'";
                }
                var adhocId = $@" and t.""Id"" IN ({adhocText.Trim(',')}) ";
                where = where + adhocId;
            }
            else
            {
                var adhocId = $@" and tt.""Id"" IN ('') ";
                where = where + adhocId;
            }
            query = query.Replace("#WHERE#", where);
            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return list;
        }

        public async Task<List<BusinessDiagramNodeViewModel>> GetTemplateBusinessDiagram(string templateId)
        {
            var query = @$"select ""T"".""Id"" as ""Id"",""T"".""Id"" as ""ReferenceId"",null as ""ParentIds"",""T"".""DisplayName"" as ""Title""
            ,""T"".""Description"" as ""Description"",""T"".""TemplateType"" as ""TemplateType"",'TEMPLATE' as ""Type"",3 as ""NodeShape""
            from public.""Template"" as ""T"" 
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false
            union
            select ""CM"".""Id"" as ""Id"",""CM"".""Id"" as ""ReferenceId"",'UDF_ROOT' as ""ParentIds"",""CM"".""LabelName"" as ""Title""
            ,""CM"".""LabelName"" as ""Description"",""T"".""TemplateType"" as ""TemplateType"",'UDF' as ""Type""
            ,1 as ""NodeShape""
            from public.""Template"" as ""T"" 
            join public.""TableMetadata"" as ""TM"" on coalesce(""T"".""TableMetadataId"",""T"".""UdfTableMetadataId"")=""TM"".""Id""
            join public.""ColumnMetadata"" as ""CM"" on ""TM"".""Id""=""CM"".""TableMetadataId"" and ""CM"".""IsUdfColumn""=true
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""TM"".""IsDeleted""=false and ""CM"".""IsDeleted""=false
            union
            select 'PRE_'||""ST"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId"",'PRE_ACTION_ROOT' as ""ParentIds"",coalesce(""ST"".""Description"",""ST"".""Name"") as ""Title""
            ,""ST"".""Description"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,'PRE_ACTION' as ""Type"",1 as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS' 
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""BR"".""BusinessLogicExecutionType""=0 and ""BR"".""TemplateId""=""T"".""Id""
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false and ""BR"".""IsDeleted""=false
            union
            select 'POST_'||""ST"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId"",'POST_ACTION_ROOT' as ""ParentIds"",coalesce(""ST"".""Description"",""ST"".""Name"") as ""Title""
            ,""ST"".""Description"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,'POST_ACTION' as ""Type"",1 as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""BR"".""BusinessLogicExecutionType""=1 and ""BR"".""TemplateId""=""T"".""Id""
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false and ""BR"".""IsDeleted""=false
             
            union
            select 'PRE_LOAD_'||""ST"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId"",'PRE_LOAD_ROOT' as ""ParentIds"",coalesce(""ST"".""Description"",""ST"".""Name"") as ""Title""
            ,""ST"".""Description"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,'PRE_LOAD' as ""Type"",1 as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""BR"".""BusinessLogicExecutionType""=3 and ""BR"".""TemplateId""=""T"".""Id""
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false and ""BR"".""IsDeleted""=false
            
            union
            select  ""BN"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId""
            ,case when ""BN"".""IsStarter""=true then  'PRE_'||""ST"".""Id"" else ""BC"".""SourceId"" end as ""ParentIds""
            ,""BN"".""Name"" as ""Title""
            ,""BN"".""Name"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,case when ""BN"".""Type""='0' and ""BN"".""IsStarter""=true then 'BR_START' 
            when ""BN"".""Type""='0' and ""BN"".""IsStarter""=false then 'BR_STOP' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=true  then 'BR_TRUE' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=false  then 'BR_FALSE' 
            when ""BN"".""Type""='2' then 'BR_DECISION' when ""BN"".""Type""='1' then 'BR_PROCESS' end as ""Type""
            ,case when ""BN"".""Type""='3' then 0 when ""BN"".""Type""='0' then 4
            else ""BN"".""Type"" end as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""T"".""Id""=""BR"".""TemplateId"" and ""BR"".""BusinessLogicExecutionType""=0
            join public.""BusinessRuleNode"" as ""BN"" on ""BR"".""Id""=""BN"".""BusinessRuleId""  
            left join public.""BusinessRuleConnector"" as ""BC"" on ""BN"".""Id""=""BC"".""TargetId"" 
            and ""BR"".""Id""=""BC"".""BusinessRuleId"" and ""BC"".""IsDeleted""=false
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false
            and ""BR"".""IsDeleted""=false and ""BN"".""IsDeleted""=false
            union
            select  ""BN"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId""
            ,case when ""BN"".""IsStarter""=true then  'POST_'||""ST"".""Id"" else ""BC"".""SourceId"" end as ""ParentIds""
            ,""BN"".""Name"" as ""Title""
            ,""BN"".""Name"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,case when ""BN"".""Type""='0' and ""BN"".""IsStarter""=true then 'BR_START' 
            when ""BN"".""Type""='0' and ""BN"".""IsStarter""=false then 'BR_STOP' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=true  then 'BR_TRUE' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=false  then 'BR_FALSE'
            when ""BN"".""Type""='2' then 'BR_DECISION' when ""BN"".""Type""='1' then 'BR_PROCESS' end as ""Type""
            ,case when ""BN"".""Type""='3' then 0 when ""BN"".""Type""='0' then 4
            else ""BN"".""Type"" end as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""T"".""Id""=""BR"".""TemplateId"" and ""BR"".""BusinessLogicExecutionType""=1
            join public.""BusinessRuleNode"" as ""BN"" on ""BR"".""Id""=""BN"".""BusinessRuleId""  
            left join public.""BusinessRuleConnector"" as ""BC"" on ""BN"".""Id""=""BC"".""TargetId"" 
            and ""BR"".""Id""=""BC"".""BusinessRuleId"" and ""BC"".""IsDeleted""=false
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false
            and ""BR"".""IsDeleted""=false and ""BN"".""IsDeleted""=false

union
            select  'PRE_LOAD_'||""BN"".""Id"" as ""Id"",""BR"".""Id"" as ""ReferenceId""
            ,case when ""BN"".""IsStarter""=true then 'PRE_LOAD_'||""ST"".""Id"" else 'PRE_LOAD_'||""BC"".""SourceId"" end as ""ParentIds""
            ,""BN"".""Name"" as ""Title""
            ,""BN"".""Name"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,case when ""BN"".""Type""='0' and ""BN"".""IsStarter""=true then 'BR_START' 
            when ""BN"".""Type""='0' and ""BN"".""IsStarter""=false then 'BR_STOP' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=true  then 'BR_TRUE' 
            when ""BN"".""Type""='3' and ""BC"".""IsFromDecision""=true and ""BC"".""IsForTrue""=false  then 'BR_FALSE'
            when ""BN"".""Type""='2' then 'BR_DECISION' when ""BN"".""Type""='1' then 'BR_PROCESS' end as ""Type""
            ,case when ""BN"".""Type""='3' then 0 when ""BN"".""Type""='0' then 4
            else ""BN"".""Type"" end as ""NodeShape""
            from public.""Template"" as ""T""  join public.""LOV"" as ""ST"" 
            on case when ""T"".""TemplateType""='4' then 'LOV_NOTE_STATUS' when ""T"".""TemplateType""='5' then 'LOV_TASK_STATUS' 
            when ""T"".""TemplateType""='6' then 'LOV_SERVICE_STATUS'
            when ""T"".""TemplateType""='3' then 'LOV_FORM_STATUS' end=""ST"".""LOVType"" 
            join public.""BusinessRule"" as ""BR"" on ""ST"".""Id""=""BR"".""ActionId"" 
            and ""T"".""Id""=""BR"".""TemplateId"" and ""BR"".""BusinessLogicExecutionType""=3
            join public.""BusinessRuleNode"" as ""BN"" on ""BR"".""Id""=""BN"".""BusinessRuleId""  
            left join public.""BusinessRuleConnector"" as ""BC"" on ""BN"".""Id""=""BC"".""TargetId"" 
            and ""BR"".""Id""=""BC"".""BusinessRuleId"" and ""BC"".""IsDeleted""=false
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""ST"".""IsDeleted""=false
            and ""BR"".""IsDeleted""=false and ""BN"".""IsDeleted""=false

            union
            select ""C"".""Id"" as ""Id"",""STC"".""TemplateId"" as ""ReferenceId""
             ,case when ""C"".""ComponentType""='1' then 'WF_ROOT' when ""C"".""ComponentType""='4' and ""C"".""ParentId"" <> ""cp"".""ParentId""
            then CONCAT(""C"".""ParentId"",',' ,""cp"".""ParentId"")
            else ""C"".""ParentId"" end as ""ParentIds""

            ,""C"".""Name"" as ""Title"",""C"".""Name"" as ""Description"",""T"".""TemplateType"" as ""TemplateType""
            ,case when ""C"".""ComponentType""='1' then 'WF_START' when ""C"".""ComponentType""='2' then 'WF_STOP'
            when ""C"".""ComponentType""='8' then 'WF_TRUE' when ""C"".""ComponentType""='9' then 'WF_FALSE' 
            when ""C"".""ComponentType""='4' then 'WF_STEP_TASK' when ""C"".""ComponentType""='6' then 'WF_DECISION' end as ""Type""
            ,case when ""C"".""ComponentType""='1' then 4 when ""C"".""ComponentType""='2' then 4 
            when ""C"".""ComponentType""='8' then 0 when ""C"".""ComponentType""='9' then 0 
            when ""C"".""ComponentType""='4' then 3 when ""C"".""ComponentType""='6' then 2 end as ""NodeShape""
            from public.""Template"" as ""T""  
            join public.""ProcessDesign"" as ""PD"" on ""T"".""Id""=""PD"".""TemplateId""
            join public.""Component"" as ""C"" on ""PD"".""Id""=""C"".""ProcessDesignId""
            left join public.""ComponentParent"" as cp on ""C"".""Id""=cp.""ComponentId""
            left join public.""StepTaskComponent"" as ""STC"" on  ""STC"".""ComponentId"" = ""C"".""Id"" and ""STC"".""IsDeleted""=false
            where ""T"".""Id""='{templateId}' and ""T"".""IsDeleted""=false and ""PD"".""IsDeleted""=false and ""C"".""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<BusinessDiagramNodeViewModel>(query, null);
            return list;
        }

        public async Task<List<IdNameViewModel>> GetComponentsList(string templateId)
        {
            var query = $@"Select c.""Id"", case when c.""ComponentType"" = '4' then stc.""Subject"" when c.""ComponentType"" = '6' then c.""Name"" end as ""Name""
            from public.""Template"" as t  
            join public.""ProcessDesign"" as pd on t.""Id""=pd.""TemplateId"" and pd.""IsDeleted""=false
            join public.""Component"" as c on pd.""Id""=c.""ProcessDesignId"" and c.""IsDeleted""=false
            left join public.""StepTaskComponent"" as stc on  stc.""ComponentId"" = c.""Id"" and stc.""IsDeleted""=false
            where t.""Id""='{templateId}' and(c.""ComponentType""=4 or c.""ComponentType""=6) and t.""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<TemplateViewModel>> GetAllowedTemplateList(string categoryCode, string userId, TemplateTypeEnum? templateType, TaskTypeEnum? taskType, string portalCode = null)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType""
            , tc.""Code"" as CategoryCode,coalesce(nt.""IconFileId"",tt.""IconFileId"",st.""IconFileId"") as ""IconFileId""
            ,coalesce(nt.""TemplateColor"",tt.""TemplateColor"",st.""TemplateColor"",'#87CEEB') as ""TemplateColor""
            ,tt.""TaskTemplateType"" as ""TaskType"",tc.""Name"" as ""TemplateCategoryName""
            from public.""Template"" as t
            join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId""  
            left join public.""NoteTemplate"" as nt on nt.""TemplateId""=t.""Id"" and nt.""IsDeleted"" = false
            left join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted"" = false
            left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted"" = false
            where t.""IsDeleted"" = false and tc.""IsDeleted"" = false ";

            if (templateType != null)
            {
                query = $@"{query} and t.""TemplateType""={(int)templateType} ";
            }
            if (categoryCode.IsNotNullAndNotEmpty())
            {
                categoryCode = categoryCode.Replace(",", "','");
                categoryCode = String.Concat("'", categoryCode, "'");
                query = $@"{query} and tc.""Code"" in (" + categoryCode + ") ";
                //query = $@"{query} and tc.""Code""='{categoryCode}' ";
            }
            if (taskType.IsNotNull())
            {
                query = $@"{query} and tt.""TaskTemplateType""={(int)taskType} ";
            }
            return await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
        }

        public async Task<List<TemplateViewModel>> GetNoteTemplateList(string tCode, string tcCode, string mCodes, string templateIds = null, string categoryIds = null, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId""
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""TemplateCategoryType""={(int)categoryType} and tc.""IsDeleted""=false 
                        join public.""NoteTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false 
left join public.""Module"" as m on m.""Id""=tc.""ModuleId"" and m.""IsDeleted""=false  
join public.""Portal"" as p on t.""PortalId""=p.""Id"" and p.""IsDeleted""=false #PORTALWHERE#
where t.""TemplateType"" =4 and t.""IsDeleted""=false #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# #categoryIdSearch#";


            var portalWhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalWhere = @$" and p.""Name"" in ('{portalNames.Replace(",", "','")}') ";
            }
            else
            {
                portalWhere = @$" and p.""Id""='{_repo.UserContext.PortalId}'";
            }
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var templatecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);
            var modulecodeSearch = "";
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);
            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);
            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }
            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }

        public async Task<List<TemplateViewModel>> GetTemplateList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId"", tt.""TaskTemplateType"" as TaskType
                        from public.""Template"" as t                        
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""TemplateCategoryType""={(int)categoryType}
                        left join public.""Module"" as m on m.""Id""=tc.""ModuleId"" and m.""IsDeleted""=false 
                        join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false and tt.""TaskTemplateType"" != 2
                        join public.""Portal"" as p on t.""PortalId""=p.""Id"" and p.""IsDeleted""=false #PORTALWHERE#
                        where t.""TemplateType"" = 5 and t.""IsDeleted""=false #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# #categoryIdSearch#";

            var portalWhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalWhere = @$" and p.""Name"" in ('{portalNames.Replace(",", "','")}') ";
            }
            else
            {
                portalWhere = @$" and p.""Id""='{_repo.UserContext.PortalId}'";
            }
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var templatecodeSearch = "";
            var modulecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);
            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);
            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }
            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }
        public async Task<List<TemplateViewModel>> GetAdhocTemplateList(string tCode, string tcCode, string moduleId)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId"", tt.""TaskTemplateType"" as TaskType
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                        join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""=false 
                        where tt.""TaskTemplateType"" = 4 and t.""PortalId""='{_repo.UserContext.PortalId}' and t.""TemplateType"" = 5 and t.""IsDeleted""=false #templatecodesearch# #categorycodesearch# #module# ";

            var templatecodeSearch = "";
            var moduleSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = @" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);

            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            if (!moduleId.IsNullOrEmpty())
            {
                moduleSearch = @" and t.""ModuleId"" = '" + moduleId + "'";
            }
            query = query.Replace("#module#", moduleSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }

        public async Task<TemplateViewModel> CheckTemplate(TemplateViewModel model)
        {
            var query = $@"select * from public.""Template"" where ""Code""='{model.Code}' and ""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQuerySingle<TemplateViewModel>(query, null);
            return result;
        }

        public async Task<TemplateViewModel> SelectTemplateByName(TemplateViewModel viewModel)
        {
            var query = @$"select ""Id"" from public.""Template"" 
                            where ""IsDeleted""=false and ""Id""<>'{viewModel.Id}' and LOWER(""Name"")=LOWER('{viewModel.Name}')";
            var givenName = await _queryRepo.ExecuteScalar<TemplateViewModel>(query, null);
            return givenName;
        }

        public async Task<TemplateViewModel> SelectTemplateByCode(TemplateViewModel viewModel)
        {
            var query = @$"select ""Id"" from public.""Template"" 
                            where ""IsDeleted""=false and ""Id""<>'{viewModel.Id}' and LOWER(""Code"")=LOWER('{viewModel.Code}')";
            var code = await _queryRepo.ExecuteScalar<TemplateViewModel>(query, null);
            return code;
        }





        #endregion


        #region PageBusiness
        public async Task<List<PageViewModel>> PortalDetails()
        {
            var newquery = @"select p.""Title"",pt.""Name"" as PortalName,mg.""Name"" as MenuGroupName,sm.""Name"" as SubModuleName,m.""Name"" as ModuleName,
t.""DisplayName"" as TemplateName,p.""PageType"",pt.""Id"" as PortalId,p.""Id"" as Id
from public.""Page"" as p
join public.""Portal"" as pt on pt.""Id""=p.""PortalId"" and pt.""IsDeleted""=false
join public.""MenuGroup"" as mg on mg.""Id""= p.""MenuGroupId"" and mg.""IsDeleted""=false
join public.""SubModule"" as sm on sm.""Id""= mg.""SubModuleId"" and sm.""IsDeleted""=false
join public.""Module"" as m on m.""Id""= sm.""ModuleId"" and m.""IsDeleted""=false
join public.""Template"" as t on t.""Id""= p.""TemplateId"" and t.""IsDeleted""=false
where p.""IsDeleted""=false";
            var res = await _queryRepo.ExecuteQueryList<PageViewModel>(newquery, null);
            return res;



        }

        public async Task<MenuGroupViewModel> GetMenuGroup(string language, PageViewModel page)
        {
            var menuGroup = await _queryRepo.ExecuteQuerySingle<MenuGroupViewModel>(
                    @$"select mg.* ,coalesce(""rlTitle"".""{language}"",mg.""Name"") as ""Name""
                    from public.""MenuGroup"" as mg
                    left join public.""ResourceLanguage"" as ""rlTitle"" on mg.""Id""=""rlTitle"".""TemplateId"" 
                    and ""rlTitle"".""TemplateType""=14 and ""rlTitle"".""Code""='MenuGroupName'  and ""rlTitle"".""IsDeleted""=false
                    where mg.""Id""='{page.MenuGroupId}' and mg.""IsDeleted""=false"
                   , null);

            return menuGroup;
        }

        public async Task<PageViewModel> GetUserPagePermission(string portalId, string pageId)
        {
            var result = await _queryRepo.ExecuteQuerySingle<PageViewModel>(
                @$"select pa.""Id"" as Id,pa.""AllowIfPortalAccess"" as AllowIfPortalAccess,
                pa.""AuthorizationNotRequired"" as AuthorizationNotRequired,upe.""Id"" as UserPermissionId,
                up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions 
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id"" and po.""IsDeleted""=false
                left join public.""UserPortal"" as up on po.""Id""=up.""PortalId"" and up.""IsDeleted""=false
                and up.""UserId""='{_repo.UserContext.UserId}' 
                left join public.""UserPermission"" as upe on pa.""Id""=upe.""PageId"" 
                and upe.""IsDeleted""=false and upe.""UserId""='{_repo.UserContext.UserId}'
                where  pa.""PortalId""='{portalId}' and pa.""Id""='{pageId}' and pa.""CompanyId""='{_repo.UserContext.CompanyId}' 
                and pa.""IsDeleted""=false 
                union
                select pa.""Id"" as Id,pa.""AllowIfPortalAccess"" as AllowIfPortalAccess,
                pa.""AuthorizationNotRequired"" as AuthorizationNotRequired,upe.""Id"" as UserPermissionId,
                up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions 
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""  and po.""IsDeleted""=false
                left join 
                ( select up.* from 
                public.""UserRolePortal"" as up 
                join public.""UserRole"" as ur on up.""UserRoleId""=ur.""Id""  
                and ur.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru on ur.""Id""=uru.""UserRoleId""
                and uru.""IsDeleted""=false and uru.""UserId""='{_repo.UserContext.UserId}' 
                where up.""PortalId""='{portalId}' and up.""IsDeleted""=false 
                ) as up on po.""Id""=up.""PortalId""
                left join 
                ( select upe.*
                from public.""UserRolePermission"" as upe 
                join public.""UserRole"" as ur2 on upe.""UserRoleId""=ur2.""Id""
                and ur2.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru2 on ur2.""Id""=uru2.""UserRoleId""
                and uru2.""IsDeleted""=false and uru2.""UserId""='{_repo.UserContext.UserId}' 
                where upe.""PageId""='{pageId}' and upe.""IsDeleted""=false 
                )upe on pa.""Id""=upe.""PageId""
                where  pa.""PortalId""='{portalId}' and pa.""Id""='{pageId}' and pa.""CompanyId""='{_repo.UserContext.CompanyId}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false"
                , null);

            return result;
        }

        #endregion
        public async Task<List<ApplicationDocumentViewModel>> GetApplicationDocumentListData()
        {
            var query = @$"select ad.*,f.""FileName"" as DocumentFileName
                from public.""ApplicationDocument"" as ad
                left join public.""File"" as f on ad.""DocumentId""=f.""Id"" and f.""IsDeleted""=false            
                where ad.""IsDeleted""=false";
            return await _queryRepo.ExecuteQueryList<ApplicationDocumentViewModel>(query, null);
        }
        public async Task<ApplicationDocumentViewModel> GetApplicationDocumentData(string documentCode)
        {
            var query = @$"select ad.*,f.""FileName"" as DocumentFileName
            from public.""ApplicationDocument"" as ad
            left join public.""File"" as f on ad.""DocumentId""=f.""Id"" and f.""IsDeleted""=false            
            where ad.""IsDeleted""=false";
            return null;

        }

        public async Task<List<ApplicationErrorViewModel>> GetApplicationErrorListData()
        {
            var query = @$"select ad.*,f.""FileName"" as DocumentFileName
            from public.""ApplicationDocument"" as ad
            left join public.""File"" as f on ad.""DocumentId""=f.""Id"" and f.""IsDeleted""=false            
            where ad.""IsDeleted""=false";
            return await _queryRepo.ExecuteQueryList<ApplicationErrorViewModel>(query, null);
        }
        public async Task<TableMetadataViewModel> GetViewableColumnMetadataListData(string schemaName, string tableName)
        {
            var query = @$"select ta.""Id"",t.""TemplateType"" from public.""TableMetadata"" ta 
            left join public.""Template"" t on ta.""Id""=coalesce(t.""UdfTableMetadataId"",t.""TableMetadataId"") and t.""IsDeleted""=false
            where ta.""Schema""='{schemaName}' and ta.""Name""='{tableName}' and ta.""IsDeleted""=false";
            var tableMetadata = await _queryRepo.ExecuteQuerySingle<TableMetadataViewModel>(query, null);
            return tableMetadata;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForFormData(string tableMetadataId)
        {
            var query = $@"SELECT  fc.*,true as IsForeignKeyTableColumn,c.""ForeignKeyTableName"" as TableName
                ,c.""ForeignKeyTableSchemaName"" as TableSchemaName,c.""ForeignKeyTableAliasName"" as  TableAliasName
                ,c.""Name"" as ""ForeignKeyBaseColumnName""
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId"" and fc.""IsDeleted""=false
                where c.""TableMetadataId""='{tableMetadataId}' and fc.""IsHiddenColumn"" = false  and c.""HideForeignKeyTableColumns""=false  
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  ";
            var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return result;

        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForNoteData(string tableMetadataId)
        {

            var query = $@"SELECT  fc.*,true as IsForeignKeyTableColumn,c.""ForeignKeyTableName"" as TableName
                ,c.""ForeignKeyTableSchemaName"" as TableSchemaName,c.""ForeignKeyTableAliasName"" as  TableAliasName
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId"" and fc.""IsDeleted""=false
                where c.""TableMetadataId""='{tableMetadataId}' and fc.""IsHiddenColumn"" = false and c.""HideForeignKeyTableColumns""=false 
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  ";
            var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return result;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForTaskData(string tableMetadataId)
        {
            var query = $@"SELECT  fc.*,true as IsForeignKeyTableColumn,c.""ForeignKeyTableName"" as TableName
                ,c.""ForeignKeyTableSchemaName"" as TableSchemaName,c.""ForeignKeyTableAliasName"" as  TableAliasName
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId"" and fc.""IsDeleted""=false
                where c.""TableMetadataId""='{tableMetadataId}' and fc.""IsHiddenColumn"" = false and c.""HideForeignKeyTableColumns""=false  
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  ";
            var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return result;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForTaskData2()
        {
            var query1 = $@"SELECT c.* ,c.""Name"" as ""Name"",c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false
            where t.""Name""='NtsTask' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false 
            union
            SELECT  c.*,'Template'||c.""Name"" as ""Name"",c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false
            where t.""Name""='Template' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false 
            and c.""Name"" in ('Name','Code')
            union
            SELECT  c.*,'CreatedByUser'||c.""Name"" as ""Name"",'CreatedByUser '||c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
            where t.""Name""='User' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false 
            and c.""Name"" in ({Helper.GetViewableUserColumns})
            union
            SELECT  c.*,'UpdatedByUser'||c.""Name"" as ""Name"",'UpdatedByUser '||c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false
            where t.""Name""='User' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false  
            and c.""Name"" in ({Helper.GetViewableUserColumns})
            ";
            var result1 = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query1, null);
            return result1;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForServiceData(string tableMetadataId)
        {
            var query = $@"SELECT  fc.*,true as IsForeignKeyTableColumn,c.""ForeignKeyTableName"" as TableName
                ,c.""ForeignKeyTableSchemaName"" as TableSchemaName,c.""ForeignKeyTableAliasName"" as  TableAliasName
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId"" and fc.""IsDeleted""=false
                where c.""TableMetadataId""='{tableMetadataId}' and fc.""IsHiddenColumn"" = false and c.""HideForeignKeyTableColumns""=false  
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  ";
            var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return result;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForServiceData1(string tableMetadataId)
        {
            var query1 = $@"SELECT c.* ,c.""Name"" as ""Name"",c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
            where t.""Name""='NtsService' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false 
            union
            SELECT  c.*,'Template'||c.""Name"" as ""Name"",c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
            where t.""Name""='Template' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false 
            and c.""Name"" in ('Name','Code')
            union
            SELECT  c.*,'CreatedByUser'||c.""Name"" as ""Name"",'CreatedByUser '||c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
            where t.""Name""='User' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false 
            and c.""Name"" in ({Helper.GetViewableUserColumns})
            union
            SELECT  c.*,'UpdatedByUser'||c.""Name"" as ""Name"",'UpdatedByUser '||c.""LabelName"" as ""LabelName""
            FROM public.""ColumnMetadata"" c
            join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
            where t.""Name""='User' and  c.""IsHiddenColumn"" = false and c.""IsDeleted""=false 
            and c.""Name"" in ({Helper.GetViewableUserColumns})
            ";
            var result1 = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query1, null);
            return result1;
        }
        public async Task<List<ComponentParentViewModel>> GetComponentParentData(string componentId)
        {
            var query = @$"select cp.* from public.""ComponentParent"" as cp where cp.""ComponentId"" ='{componentId}' and cp.""IsDeleted"" = false";
            var data = await _queryRepo.ExecuteQueryList<ComponentParentViewModel>(query, null);
            return data;
        }
        public async Task<List<ComponentParentViewModel>> GetComponentChildData(string componentId)
        {
            var query = @$"select cp.* from public.""ComponentParent"" as cp where cp.""ParentId"" ='{componentId}' and cp.""IsDeleted"" = false";
            var data = await _queryRepo.ExecuteQueryList<ComponentParentViewModel>(query, null);
            return data;
        }
        public async Task RemoveParentsData(string componentId)
        {
            var query = @$"update  public.""ComponentParent"" set ""IsDeleted"" = true where ""ComponentId"" ='{componentId}'";
            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);
        }
        public async Task<List<ComponentResultViewModel>> GetChildComponentResultListData(ComponentResultViewModel componentResult)
        {
            var query = $@"select cr.* from public.""ComponentResult"" as cr
            join public.""Component"" as c on cr.""ComponentId""=c.""Id"" and c.""IsDeleted""=false
            join public.""ComponentParent"" as cp on c.""Id""=cp.""ComponentId"" and cp.""IsDeleted""=false
            where  cr.""IsDeleted""=false  
            and cr.""NtsServiceId""='{componentResult.NtsServiceId}' and cp.""ParentId""='{componentResult.ComponentId}'";
            var list = await _queryRepo.ExecuteQueryList<ComponentResultViewModel>(query, null);

            return list.Where(x => x.ParentComponentResultId is null || x.ParentComponentResultId == componentResult.Id).ToList();

        }
        public async Task<List<ComponentResultViewModel>> GetParentListData(string ntsServiceId, string componentId)
        {
            var query = $@"select cr.*,lov.""Code"" as ""ComponentStatusCode"" from public.""ComponentResult"" as cr
            join public.""LOV"" as lov on cr.""ComponentStatusId""=lov.""Id"" and lov.""IsDeleted""=false
            join public.""Component"" as c on cr.""ComponentId""=c.""Id"" and c.""IsDeleted""=false
            join public.""ComponentParent"" as cp on c.""Id""=cp.""ParentId"" and cp.""IsDeleted""=false
            where  cr.""IsDeleted""=false  
            and cr.""NtsServiceId""='{ntsServiceId}' and cp.""ComponentId""='{componentId}'";
            return await _queryRepo.ExecuteQueryList<ComponentResultViewModel>(query, null);

        }
        public async Task<List<ComponentResultViewModel>> GetStepTaskSkipRuleList(string componentId)
        {
            var query = $@"select r.* from public.""StepTaskSkipLogic"" sl
            join public.""BusinessRuleModel"" r on sl.""Id""=r.""ReferenceId"" and r.""IsDeleted""=false
            where sl.""IsDeleted""=false and sl.""StepTaskComponentId""='{componentId}' order by sl.""SequenceOrder""";
            return await _queryRepo.ExecuteQueryList<ComponentResultViewModel>(query, null);
        }
        public async Task<List<ComponentResultViewModel>> GetComponentResultListData(string serviceId)
        {
            string query = @$"select pl.*, vt.""Name"" as ComponentStatusName,u.""Name"" as Assignee,u.""Id"" as AssigneeId,u.""Email"" as Email,u.""PhotoId"" as AssigneePhotoId,temp.""Code"" as TemplateMasterCode 
            ,vt.""Code"" as ComponentStatusCode,t.""TaskNo"" as TaskNo,t.""Id"" as TaskId,s.""ServiceSubject"" as ServiceSubject,coalesce(t.""TaskSubject"",stc.""Subject"") as ""TaskSubject""
           ,coalesce(t.""StartDate"",pl.""StartDate"") as ""StartDate"",coalesce(t.""DueDate"",pl.""EndDate"") as ""EndDate""
            FROM public.""ComponentResult"" as pl
            JOIN public.""LOV"" as vt ON vt.""Id"" = pl.""ComponentStatusId"" and vt.""IsDeleted"" = false
            left join public.""NtsService"" as s on s.""Id""=pl.""NtsServiceId"" and s.""IsDeleted"" = false
            left join public.""NtsTask"" as t on t.""Id""=pl.""NtsTaskId"" and t.""IsDeleted"" = false
            left join public.""StepTaskComponent"" as stc on pl.""ComponentId""=stc.""ComponentId"" and stc.""IsDeleted""=false
            left join public.""Template"" as temp on temp.""Id""=t.""TemplateId"" and temp.""IsDeleted"" = false
            left join public.""TaskTemplate"" as tt on tt.""TemplateId""=t.""TemplateId"" and tt.""IsDeleted"" = false
            left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted"" = false
            WHERE pl.""IsDeleted"" = false and pl.""NtsServiceId""='{serviceId}'
            order by stc.""SequenceOrder"",pl.""SequenceOrder"",t.""SequenceOrder""";
            var queryData = await _queryRepo.ExecuteQueryList<ComponentResultViewModel>(query, null);
            return queryData;
        }
        public async Task<List<NtsEmailViewModel>> GetNtsEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string categoryId = null, string templateId = null, string serviceId = null, string departmentId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            string query = @$"select * from (
            select s.""Id"" as ""TargetId"",'Service' as ""TargetType"",s.""Id"" as ""ServiceId"",s.""ServiceNo"" as ""ServiceNo""
            ,s.""Id"" as ""NtsId"",'Service' as ""NtsType"",s.""ServiceNo"" as ""NtsNo"",s.""ServicePlusId"" as ServicePlusId
            ,coalesce(s.""RequestedByUserId"", s.""OwnerUserId"") ""FromUserId""
            ,coalesce(s.""RequestedByUserId"", s.""OwnerUserId"") ""ToUserId""
            ,coalesce( s.""ServiceSubject"", stemp.""DisplayName"") ""Subject""
            ,coalesce( s.""ServiceDescription"", stemp.""Description"", stemp.""DisplayName"") ""Body""
            ,s.""CreatedDate"",s.""DueDate"",case when   s.""RequestedByUserId""='{userId}' or  s.""OwnerUserId""='{userId}' then 'Outbox' 
            else 'None' end ""EmailType""
            ,stempc.""Code"" as ""CategoryCode"",stempc.""Id"" as ""CategoryId"",stempc.""Name"" as ""CategoryName""
            ,stemp.""Code"" as ""TemplateCode"",stemp.""Id"" as ""TemplateId"",stemp.""DisplayName"" as ""TemplateName""
            ,st.""Code"" ""ActualStatus""
            ,fu.""Name"" as ""FromUserName"",fu.""PhotoId"" ""FromUserPhotoId"",fu.""JobTitle"" ""FromUserJobTitle"",x.""DepartmentName"" ""FromUserDepartmentName"",fu.""Email"" ""FromUserEmail""
            ,fu.""Name"" as ""ToUserName"",fu.""PhotoId"" ""ToUserPhotoId"",fu.""JobTitle"" ""ToUserJobTitle"", x.""DepartmentName"" ""ToUserDepartmentName"",fu.""Email"" ""ToUserEmail""
            ,null ""ParentId"",stemp.""Code"" ""TemplateCode"",s.""WorkflowStatus"" ""WorkflowStatus"",s.""StartDate"" ""StartDate""
            ,coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ReturnedDate"",'{DateTime.Today.ToDatabaseDateFormat()}') ""CompletedDate""
            from public.""NtsService"" as s
            join public.""LOV"" as st on st.""Id""=s.""ServiceStatusId"" and st.""IsDeleted""=false 
            join public.""Template"" as stemp on stemp.""Id""=s.""TemplateId"" and stemp.""IsDeleted"" = false
            join public.""TemplateCategory"" as stempc on stemp.""TemplateCategoryId""=stempc.""Id"" and stempc.""IsDeleted"" = false
            <<portal>>
            --left join  public.""Notification"" as n on  n.""TriggeredByReferenceTypeId""=s.""Id"" and n.""IsDeleted"" = false
            left join  public.""User"" as fu on  fu.""Id""=coalesce(s.""RequestedByUserId"", s.""OwnerUserId"") and fu.""IsDeleted"" = false
            left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=fu.""Id"" and p.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId"" = p.""Id"" and a.""IsDeleted""=false 
            Left join cms.""N_CoreHR_HRDepartment"" as x on x.""Id"" = a.""DepartmentId"" and x.""IsDeleted""=false                          
            where s.""IsDeleted"" = false and s.""ServicePlusId"" is null <<S_USER>> <<where>>
            union
            select s.""Id"" as ""TargetId"",'Service' as ""TargetType"",sp.""Id"" as ""ServiceId"",sp.""ServiceNo"" as ""ServiceNo""
            ,s.""Id"" as ""NtsId"",'Service' as ""NtsType"",s.""ServiceNo"" as ""NtsNo"",s.""ServicePlusId"" as ServicePlusId
            ,coalesce(s.""RequestedByUserId"", s.""OwnerUserId"") ""FromUserId""
            ,coalesce(s.""RequestedByUserId"", s.""OwnerUserId"") ""ToUserId""
            ,coalesce( s.""ServiceSubject"", stemp.""DisplayName"") ""Subject""
            ,coalesce( s.""ServiceDescription"", stemp.""Description"", stemp.""DisplayName"") ""Body""
            ,s.""CreatedDate"",s.""DueDate"",case when   s.""RequestedByUserId""='{userId}' or  s.""OwnerUserId""='{userId}' then 'Outbox' 
            else 'None' end ""EmailType""
            ,stempc.""Code"" as ""CategoryCode"",stempc.""Id"" as ""CategoryId"",stempc.""Name"" as ""CategoryName""
            ,stemp.""Code"" as ""TemplateCode"",stemp.""Id"" as ""TemplateId"",stemp.""DisplayName"" as ""TemplateName""
            ,st.""Code"" ""ActualStatus""
            ,fu.""Name"" as ""FromUserName"",fu.""PhotoId"" ""FromUserPhotoId"",fu.""JobTitle"" ""FromUserJobTitle"",x.""DepartmentName"" ""FromUserDepartmentName"",fu.""Email"" ""FromUserEmail""
            ,fu.""Name"" as ""ToUserName"",fu.""PhotoId"" ""ToUserPhotoId"",fu.""JobTitle"" ""ToUserJobTitle"", x.""DepartmentName"" ""ToUserDepartmentName"",fu.""Email"" ""ToUserEmail""
            ,coalesce( s.""TriggeredByReferenceId"",s.""ParentServiceId"") ""ParentId"",stemp.""Code"" ""TemplateCode"",s.""WorkflowStatus"" ""WorkflowStatus"",s.""StartDate"" ""StartDate""
            ,coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ReturnedDate"",'{DateTime.Today.ToDatabaseDateFormat()}') ""CompletedDate""
            from public.""NtsService"" as s
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted""=false 
            join public.""NtsService"" as sp on sp.""Id""=s.""ServicePlusId"" and sp.""IsDeleted""=false 
            join public.""LOV"" as st on st.""Id""=s.""ServiceStatusId"" and st.""IsDeleted""=false 
            join public.""Template"" as stemp on stemp.""Id""=s.""TemplateId"" and stemp.""IsDeleted"" = false
            join public.""TemplateCategory"" as stempc on stemp.""TemplateCategoryId""=stempc.""Id"" and stempc.""IsDeleted"" = false
            <<portal>>
            --left join  public.""Notification"" as n on  n.""TriggeredByReferenceTypeId""=s.""Id"" and n.""IsDeleted"" = false
            left join  public.""User"" as fu on  fu.""Id""=coalesce(s.""RequestedByUserId"", s.""OwnerUserId"") and fu.""IsDeleted"" = false
            left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=fu.""Id"" and p.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId"" = p.""Id"" and a.""IsDeleted""=false 
            Left join cms.""N_CoreHR_HRDepartment"" as x on x.""Id"" = a.""DepartmentId"" and x.""IsDeleted""=false                          
            where s.""IsDeleted"" = false and s.""ServicePlusId"" is not null <<S_USER>> <<where>>
            union
            select s.""Id"" as ""TargetId"",'StepTask' as ""TargetType"", coalesce(sp.""Id"",s.""ParentServiceId"") as ""ServiceId""
            ,coalesce(sp.""ServiceNo"",ps.""ServiceNo"") as ""ServiceNo""
            ,s.""Id"" as ""NtsId"",'Task' as ""NtsType"",s.""TaskNo"" as ""NtsNo"",null as ServicePlusId
            , coalesce(s.""OwnerUserId"") ""FromUserId""
            ,coalesce(s.""AssignedToUserId"") ""ToUserId""
            ,coalesce(s.""TaskSubject"", temp.""DisplayName"") ""Subject""
            ,coalesce( s.""TaskDescription"", temp.""Description"", temp.""DisplayName"") ""Body""
            ,s.""CreatedDate"",s.""DueDate"",case when s.""OwnerUserId""='{userId}' then 'Outbox' 
            when  s.""AssignedToUserId""='{userId}' then 'Inbox' else 'None' end ""EmailType""
            ,stempc.""Code"" as ""CategoryCode"",stempc.""Id"" as ""CategoryId"",stempc.""Name"" as ""CategoryName""
            ,stemp.""Code"" as ""TemplateCode"",stemp.""Id"" as ""TemplateId"",stemp.""DisplayName"" as ""TemplateName""
            ,st.""Code"" ""ActualStatus""
            ,fu.""Name"" as ""FromUserName"",fu.""PhotoId"" ""FromUserPhotoId"",fu.""JobTitle"" ""FromUserJobTitle"",fx.""DepartmentName"" ""FromUserDepartmentName"",fu.""Email"" ""FromUserEmail""
            ,tu.""Name"" as ""ToUserName"",tu.""PhotoId"" ""ToUserPhotoId"",tu.""JobTitle"" ""ToUserJobTitle"",x.""DepartmentName"" ""ToUserDepartmentName"",tu.""Email"" ""ToUserEmail""
            ,coalesce(s.""TriggeredByReferenceId"",s.""ParentServiceId"") ""ParentId"",temp.""Code"" ""TemplateCode"",ps.""WorkflowStatus"" ""WorkflowStatus"",s.""StartDate"" ""StartDate""
            ,coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ReturnedDate"",'{DateTime.Today.ToDatabaseDateFormat()}') ""CompletedDate""
            from public.""NtsTask"" as s
            join public.""LOV"" as st on st.""Id""=s.""TaskStatusId"" and st.""IsDeleted""=false 
            join public.""Template"" as temp on temp.""Id""=s.""TemplateId"" and temp.""IsDeleted"" = false
            join public.""NtsService"" as ps on s.""ParentServiceId""=ps.""Id"" and ps.""IsDeleted"" = false
            join public.""Template"" as stemp on stemp.""Id""=ps.""TemplateId"" and stemp.""IsDeleted"" = false
            join public.""TemplateCategory"" as stempc on stemp.""TemplateCategoryId""=stempc.""Id"" and stempc.""IsDeleted"" = false
            <<portal>>
            left join public.""NtsService"" as sp on ps.""ServicePlusId""=sp.""Id"" and sp.""IsDeleted"" = false
            left join  public.""Notification"" as n on  n.""TriggeredByReferenceTypeId""=s.""Id"" and n.""IsDeleted"" = false
            left join  public.""User"" as fu on  fu.""Id""= s.""OwnerUserId"" and fu.""IsDeleted"" = false
            left join  public.""User"" as tu on  tu.""Id""= s.""AssignedToUserId"" and tu.""IsDeleted"" = false
            left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=tu.""Id"" and p.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId"" = p.""Id"" and a.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRDepartment"" as x on x.""Id"" = a.""DepartmentId"" and x.""IsDeleted""=false   
            left join cms.""N_CoreHR_HRPerson"" as fp on fp.""UserId""=fu.""Id"" and fp.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as fa on fa.""EmployeeId"" = fp.""Id"" and fa.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRDepartment"" as fx on fx.""Id"" = fa.""DepartmentId"" and fx.""IsDeleted""=false  
            where s.""IsDeleted"" = false and temp.""Code"" not in('INCIDENT_REPLY','INCIDENT_ACCEPTANCE') <<T_USER>> <<where>>
            union
            select s.""Id"" as ""TargetId"",'SubTask' as ""TargetType"", s.""ParentServiceId"" as ""ServiceId"",ps.""ServiceNo"" as ""ServiceNo""
            ,s.""Id"" as ""NtsId"",'Task' as ""NtsType"",s.""TaskNo"" as ""NtsNo"",null as ServicePlusId
            , coalesce(s.""OwnerUserId"") ""FromUserId""
            ,coalesce(s.""AssignedToUserId"") ""ToUserId""
            ,coalesce( s.""TaskSubject"", temp.""DisplayName"") ""Subject""
            ,coalesce( s.""TaskDescription"", temp.""Description"", temp.""DisplayName"") ""Body""
            ,s.""CreatedDate"",s.""DueDate"",case when   s.""OwnerUserId""='{userId}' then 'Outbox' 
            when  s.""AssignedToUserId""='{userId}' then 'Inbox' else 'None' end ""EmailType""
            ,stempc.""Code"" as ""CategoryCode"",stempc.""Id"" as ""CategoryId"",stempc.""Name"" as ""CategoryName""
            ,stemp.""Code"" as ""TemplateCode"",stemp.""Id"" as ""TemplateId"",stemp.""DisplayName"" as ""TemplateName""
            ,st.""Code"" ""ActualStatus""
            ,fu.""Name"" as ""FromUserName"",fu.""PhotoId"" ""FromUserPhotoId"",fu.""JobTitle"" ""FromUserJobTitle"",fx.""DepartmentName"" ""FromUserDepartmentName"",fu.""Email"" ""FromUserEmail""
            ,tu.""Name"" as ""ToUserName"",tu.""PhotoId"" ""ToUserPhotoId"",tu.""JobTitle"" ""ToUserJobTitle"",x.""DepartmentName"" ""ToUserDepartmentName"",tu.""Email"" ""ToUserEmail""
            ,coalesce(s.""TriggeredByReferenceId"",s.""ReferenceId"")  ""ParentId"",temp.""Code"" ""TemplateCode"",ps.""WorkflowStatus"" ""WorkflowStatus"",s.""StartDate"" ""StartDate""
            ,coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ReturnedDate"",'{DateTime.Today.ToDatabaseDateFormat()}') ""CompletedDate""
            from public.""NtsTask"" as s
            join public.""LOV"" as st on st.""Id""=s.""TaskStatusId"" and st.""IsDeleted""=false 
            join public.""Template"" as temp on temp.""Id""=s.""TemplateId"" and temp.""IsDeleted"" = false
            join public.""NtsService"" as ps on s.""ParentServiceId""=ps.""Id"" and ps.""IsDeleted"" = false
            join public.""Template"" as stemp on stemp.""Id""=ps.""TemplateId"" and stemp.""IsDeleted"" = false
            join public.""TemplateCategory"" as stempc on stemp.""TemplateCategoryId""=stempc.""Id"" and stempc.""IsDeleted"" = false
            <<portal>>
            --left join  public.""Notification"" as n on  n.""TriggeredByReferenceTypeId""=s.""Id"" and n.""IsDeleted"" = false
            left join  public.""User"" as fu on  fu.""Id""= s.""OwnerUserId"" and fu.""IsDeleted"" = false
            left join  public.""User"" as tu on  tu.""Id""= s.""AssignedToUserId"" and tu.""IsDeleted"" = false
            left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=tu.""Id"" and p.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId"" = p.""Id"" and a.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRDepartment"" as x on x.""Id"" = a.""DepartmentId"" and x.""IsDeleted""=false   
            left join cms.""N_CoreHR_HRPerson"" as fp on fp.""UserId""=fu.""Id"" and fp.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as fa on fa.""EmployeeId"" = fp.""Id"" and fa.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRDepartment"" as fx on fx.""Id"" = fa.""DepartmentId"" and fx.""IsDeleted""=false  
            where s.""IsDeleted"" = false and temp.""Code""='INCIDENT_REPLY' <<T_USER>> <<where>>
            union
            select s.""Id"" as ""TargetId"",'AcceptanceTask' as ""TargetType"", s.""ParentServiceId"" as ""ServiceId"",ps.""ServiceNo"" as ""ServiceNo""
            ,s.""Id"" as ""NtsId"",'Task' as ""NtsType"",s.""TaskNo"" as ""NtsNo"",null as ServicePlusId
            , coalesce(s.""OwnerUserId"") ""FromUserId""
            ,coalesce(s.""AssignedToUserId"") ""ToUserId""
            ,coalesce( s.""TaskSubject"", temp.""DisplayName"") ""Subject""
            ,coalesce( s.""TaskDescription"", temp.""Description"", temp.""DisplayName"") ""Body""
            ,s.""CreatedDate"",s.""DueDate"",case when  s.""OwnerUserId""='{userId}' then 'Outbox' 
            when s.""AssignedToUserId""='{userId}' then 'Inbox' else 'None' end ""EmailType""
            ,stempc.""Code"" as ""CategoryCode"",stempc.""Id"" as ""CategoryId"",stempc.""Name"" as ""CategoryName""
            ,stemp.""Code"" as ""TemplateCode"",stemp.""Id"" as ""TemplateId"",stemp.""DisplayName"" as ""TemplateName""
            ,st.""Code"" ""ActualStatus""
            ,fu.""Name"" as ""FromUserName"",fu.""PhotoId"" ""FromUserPhotoId"",fu.""JobTitle"" ""FromUserJobTitle"",fx.""DepartmentName"" ""FromUserDepartmentName"",fu.""Email"" ""FromUserEmail""
            ,tu.""Name"" as ""ToUserName"",tu.""PhotoId"" ""ToUserPhotoId"",tu.""JobTitle"" ""ToUserJobTitle"",x.""DepartmentName"" ""ToUserDepartmentName"",tu.""Email"" ""ToUserEmail""
            ,coalesce(s.""TriggeredByReferenceId"",s.""ReferenceId"")  ""ParentId"",temp.""Code"" ""TemplateCode"",ps.""WorkflowStatus"" ""WorkflowStatus"",s.""StartDate"" ""StartDate""
            ,coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ReturnedDate"",'{DateTime.Today.ToDatabaseDateFormat()}') ""CompletedDate""
            from public.""NtsTask"" as s
            join public.""LOV"" as st on st.""Id""=s.""TaskStatusId"" and st.""IsDeleted""=false 
            join public.""Template"" as temp on temp.""Id""=s.""TemplateId"" and temp.""IsDeleted"" = false
            join public.""NtsService"" as ps on s.""ParentServiceId""=ps.""Id"" and ps.""IsDeleted"" = false
            join public.""Template"" as stemp on stemp.""Id""=ps.""TemplateId"" and stemp.""IsDeleted"" = false
            join public.""TemplateCategory"" as stempc on stemp.""TemplateCategoryId""=stempc.""Id"" and stempc.""IsDeleted"" = false
            <<portal>>
            --left join  public.""Notification"" as n on  n.""TriggeredByReferenceTypeId""=s.""Id"" and n.""IsDeleted"" = false
            left join  public.""User"" as fu on  fu.""Id""= s.""OwnerUserId"" and fu.""IsDeleted"" = false
            left join  public.""User"" as tu on  tu.""Id""= s.""AssignedToUserId"" and tu.""IsDeleted"" = false
            left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=tu.""Id"" and p.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId"" = p.""Id"" and a.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRDepartment"" as x on x.""Id"" = a.""DepartmentId"" and x.""IsDeleted""=false   
            left join cms.""N_CoreHR_HRPerson"" as fp on fp.""UserId""=fu.""Id"" and fp.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRAssignment"" as fa on fa.""EmployeeId"" = fp.""Id"" and fa.""IsDeleted""=false 
            left join cms.""N_CoreHR_HRDepartment"" as fx on fx.""Id"" = fa.""DepartmentId"" and fx.""IsDeleted""=false  
            where s.""IsDeleted"" = false and temp.""Code""='INCIDENT_ACCEPTANCE' <<T_USER>> <<where>>
            )
            result where  1=1 <<serviceIdWhere>> order by ""CreatedDate"" desc";
            var portal = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalNames = $"'{portalNames.Replace(",", "','")}'";
                portal = @$" join public.""Portal"" port on port.""Name"" in ({portalNames}) and port.""IsDeleted""=false and  port.""Id""=ANY(stempc.""AllowedPortalIds"")";
            }
            var where = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = $"'{templateCodes.Replace(",", "','")}'";
                where = @$"{where} and stemp.""Code"" in ({templateCodes})";
            }
            if (templateId.IsNotNullAndNotEmpty())
            {
                where = @$"{where} and stemp.""Id"" ='{templateId}'";
            }

            if (catCodes.IsNotNullAndNotEmpty())
            {
                catCodes = $"'{catCodes.Replace(",", "','")}'";
                where = @$"{where} and stempc.""Code"" in ({catCodes})";
            }
            if (categoryId.IsNotNullAndNotEmpty())
            {
                where = @$"{where} and stempc.""Id""='{categoryId}'";
            }
            var serviceIdWhere = "";
            if (serviceId.IsNotNullAndNotEmpty())
            {
                serviceIdWhere = @$"{serviceIdWhere} and ""ServiceId""='{serviceId}'";
            }
            var s_user = "";
            var t_user = "";
            if (userId.IsNotNullAndNotEmpty())
            {

                s_user = @$" and (s.""RequestedByUserId"" = '{userId}' or s.""OwnerUserId"" = '{userId}')";
                t_user = @$" and (s.""OwnerUserId"" = '{userId}' or s.""AssignedToUserId"" = '{userId}')";

            }
            query = query.Replace("<<portal>>", portal).Replace("<<where>>", where).Replace("<<serviceIdWhere>>", serviceIdWhere)
                .Replace("<<S_USER>>", s_user).Replace("<<T_USER>>", t_user);
            var queryData = await _queryRepo.ExecuteQueryList<NtsEmailViewModel>(query, null);
            return queryData;
        }
        //union
        //   select s.""Id"" as ""TargetId"",'ServiceComment' as ""TargetType"", s.""NtsServiceId"" as ""ServiceId"",ps.""ServiceNo"" as ""ServiceNo""
        //    ,s.""NtsServiceId"" as ""NtsId"",'Service' as ""NtsType"",ps.""ServiceNo"" as ""NtsNo""
        //    , coalesce(s.""CommentedByUserId"") ""FromUserId""
        //    ,coalesce(scu.""CommentToUserId"") ""ToUserId""
        //    ,coalesce(n.""Subject"", s.""Comment"") ""Subject""
        //    ,coalesce(n.""Body"", s.""Comment"") ""Description""
        //    ,s.""CreatedDate"",s.""CreatedDate"" ""DueDate"",case when n.""FromUserId""='{userId}' or s.""CommentedByUserId""='{userId}' then 'Outbox' 
        //    when n.""ToUserId""='{userId}' or scu.""CommentToUserId""='{userId}' then 'Inbox' end ""EmailType""
        //    , stempc.""Code"" as ""CategoryCode"", stempc.""Id"" as ""CategoryId"", stempc.""Name"" as ""CategoryName""
        //    , stemp.""Code"" as ""TemplateCode"", stemp.""Id"" as ""TemplateId"", stemp.""DisplayName"" as ""TemplateName""
        //    ,'COMPLETED' ""ActualStatus""
        //    , fu.""Name"" as ""FromUserName"", fu.""PhotoId"" ""FromUserPhotoId"", fu.""JobTitle"" ""FromUserJobTitle"", fu.""DepartmentName"" ""FromUserDepartmentName"", fu.""Email"" ""FromUserEmail""
        //    , tu.""Name"" as ""ToUserName"", tu.""PhotoId"" ""ToUserPhotoId"", tu.""JobTitle"" ""ToUserJobTitle"", x.""DepartmentName"" ""ToUserDepartmentName"", tu.""Email"" ""ToUserEmail""
        //    , coalesce(s.""ParentCommentId"", s.""NtsServiceId"") ""ParentId"",stemp.""Code"" ""TemplateCode""
        //    from public.""NtsServiceComment"" as s
        //    join public.""NtsService"" as ps on s.""NtsServiceId""=ps.""Id"" and ps.""IsDeleted"" = false
        //    join public.""Template"" as stemp on stemp.""Id""=ps.""TemplateId"" and stemp.""IsDeleted"" = false
        //    join public.""TemplateCategory"" as stempc on stemp.""TemplateCategoryId""=stempc.""Id"" and stempc.""IsDeleted"" = false
        //    <<portal>>
        //    left join public.""Notification"" as n on  n.""TriggeredByReferenceTypeId""=s.""Id"" and n.""IsDeleted"" = false
        //    left join  public.""NtsServiceCommentUser"" as scu on  scu.""NtsServiceCommentId""=s.""Id"" and scu.""IsDeleted"" = false
        //    left join  public.""User"" as fu on  fu.""Id""= s.""CommentedByUserId"" and fu.""IsDeleted"" = false
        //    left join  public.""User"" as tu on  tu.""Id""= scu.""CommentToUserId"" and tu.""IsDeleted"" = false
        //     left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=tu.""Id"" and p.""IsDeleted""=false 
        //    left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId"" = p.""Id"" and a.""IsDeleted""=false 
        //     Left join cms.""N_CoreHR_HRDepartment"" as x on x.""Id"" = a.""DepartmentId"" and x.""IsDeleted""=false   
        //    where s.""IsDeleted"" = false <<where>>
        //    union
        //    select s.""Id"" as ""TargetId"",'TaskComment' as ""TargetType"", ps.""Id"" as ""ServiceId"", ps.""ServiceNo"" as ""ServiceNo""
        //    , s.""NtsTaskId"" as ""NtsId"",'Task' as ""NtsType"", ps.""ServiceNo"" as ""NtsNo""
        //    , coalesce(s.""CommentedByUserId"") ""FromUserId""
        //    ,coalesce(tcu.""CommentToUserId"") ""ToUserId""
        //    ,coalesce(n.""Subject"", s.""Comment"") ""Subject""
        //    ,coalesce(n.""Body"", s.""Comment"") ""Description""
        //    ,s.""CreatedDate"",s.""CreatedDate"" ""DueDate"",case when n.""FromUserId""='{userId}' or s.""CommentedByUserId""='{userId}' then 'Outbox' 
        //    when n.""ToUserId""='{userId}' or tcu.""CommentToUserId""='{userId}' then 'Inbox' end ""EmailType""
        //    , stempc.""Code"" as ""CategoryCode"", stempc.""Id"" as ""CategoryId"", stempc.""Name"" as ""CategoryName""
        //    , stemp.""Code"" as ""TemplateCode"", stemp.""Id"" as ""TemplateId"", stemp.""DisplayName"" as ""TemplateName""
        //    ,'COMPLETED' ""ActualStatus""
        //    , fu.""Name"" as ""FromUserName"", fu.""PhotoId"" ""FromUserPhotoId"", fu.""JobTitle"" ""FromUserJobTitle"", fu.""DepartmentName"" ""FromUserDepartmentName"", fu.""Email"" ""FromUserEmail""
        //    , tu.""Name"" as ""ToUserName"", tu.""PhotoId"" ""ToUserPhotoId"", tu.""JobTitle"" ""ToUserJobTitle"", x.""DepartmentName"" ""ToUserDepartmentName"", tu.""Email"" ""ToUserEmail""
        //    , coalesce(s.""ParentCommentId"", s.""NtsTaskId"") ""ParentId"",ttemp.""Code"" ""TemplateCode""
        //    from public.""NtsTaskComment"" as s
        //    join public.""NtsTask"" as t on s.""NtsTaskId""=t.""Id"" and t.""IsDeleted"" = false
        //    join public.""Template"" as ttemp on ttemp.""Id""=t.""TemplateId"" and ttemp.""IsDeleted"" = false
        //    join public.""NtsService"" as ps on t.""ParentServiceId""=ps.""Id"" and ps.""IsDeleted"" = false
        //    join public.""Template"" as stemp on stemp.""Id""=ps.""TemplateId"" and stemp.""IsDeleted"" = false
        //    join public.""TemplateCategory"" as stempc on stemp.""TemplateCategoryId""=stempc.""Id"" and stempc.""IsDeleted"" = false
        //    <<portal>>
        //    left join  public.""Notification"" as n on  n.""TriggeredByReferenceTypeId""=s.""Id"" and n.""IsDeleted"" = false
        //    left join public.""NtsTaskCommentUser"" as tcu on  tcu.""NtsTaskCommentId""=s.""Id"" and tcu.""IsDeleted"" = false
        //    left join  public.""User"" as fu on  fu.""Id""= s.""CommentedByUserId"" and fu.""IsDeleted"" = false
        //    left join  public.""User"" as tu on  tu.""Id""= tcu.""CommentToUserId"" and tu.""IsDeleted"" = false
        //    left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=tu.""Id"" and p.""IsDeleted""=false 
        //    left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId"" = p.""Id"" and a.""IsDeleted""=false 
        //     Left join cms.""N_CoreHR_HRDepartment"" as x on x.""Id"" = a.""DepartmentId"" and x.""IsDeleted""=false   
        //    where s.""IsDeleted"" = false <<where>>)result where(""FromUserId""='{userId}' or ""ToUserId""='{userId}' ) <<serviceIdWhere>> order by ""CreatedDate"" desc";
        public async Task<List<TaskViewModel>> GetStepTaskListData(string serviceId)
        {
            string query = @$"select t.*,stc.""SequenceOrder"" as StepTaskSequenceOrder,lv.""Name"" as TaskStatusName,lv.""Code"" as TaskStatusCode
            ,u.""Name"" as AssigneeUserName, p.""Name"" as PageName,p.""Id"" as PageId ,temp.""Code"" as TemplateMasterCode,stc.""Id"" as ""TaskComponentId""                                                          
            FROM public.""NtsTask"" as t
            join public.""Template"" as temp on temp.""Id""=t.""TemplateId"" and temp.""IsDeleted"" = false
            join public.""StepTaskComponent"" as stc on t.""StepTaskComponentId""=stc.""Id"" and stc.""IsDeleted""=false
            left join public.""LOV"" as lv on lv.""Id""=t.""TaskStatusId"" and lv.""IsDeleted"" = false
            left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted"" = false
            left join public.""Page"" as p on p.""TemplateId""=temp.""Id"" and p.""IsDeleted""=false
            WHERE t.""IsDeleted"" = false and t.""ParentServiceId""='{serviceId}' 
            and t.""TaskType""='2' and temp.""IsDeleted""=false order by stc.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<TemplateViewModel>> GetStepTaskTemplateListData(string serviceTemplateId)
        {
            string query = @$"select t.* FROM  public.""Template"" as t 
            join public.""StepTaskComponent"" as stc on t.""Id""=stc.""TemplateId"" and stc.""IsDeleted"" = false
            join public.""Component"" as comp on comp.""Id""=stc.""ComponentId"" and comp.""IsDeleted"" = false
            join public.""ProcessDesign"" as p on p.""Id""=comp.""ProcessDesignId"" and p.""IsDeleted""=false
            WHERE t.""IsDeleted"" = false and p.""TemplateId""='{serviceTemplateId}' ";
            var queryData = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return queryData;
        }
        public async Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData(string id)
        {

            var query = $@"
		select  u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		 join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 	union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		 join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false 
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		  join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false 
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsService"" as s
		  join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false
		    join public.""Team"" as t on ss.""SharedWithTeamId""=t.""Id"" and t.""IsDeleted""=false
			 join public.""TeamUser"" as tu on t.""Id""=tu.""TeamId"" and tu.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
";
            var list = await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
            return list;
        }
        public async Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData1(string comp)
        {

            var query1 = $@"select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM  public.""User"" as u 
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where u.""IsDeleted""=false and u.""Id""='{comp}'";
            var list1 = await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query1, null);
            return list1;
        }
        public async Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData2(string id)
        {
            var query = $@"
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		 join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 	union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		 join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		  union 
select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		 join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		  join public.""NtsTaskShared"" as ss on ss.""NtsTaskId""=s.""Id"" and ss.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsTask"" as s
		  join public.""NtsTaskShared"" as ss on ss.""NtsTaskId""=s.""Id"" and ss.""IsDeleted""=false
		    join public.""Team"" as t on ss.""SharedWithTeamId""=t.""Id"" and t.""IsDeleted""=false
			 join public.""TeamUser"" as tu on t.""Id""=tu.""TeamId"" and tu.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=tu.""UserId""  and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
";
            return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
        }
        public async Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData3(string id)
        {
            var query = $@"
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		 join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 	union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		 join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		
		 union 
		select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		  join public.""NtsNoteShared"" as ss on ss.""NtsNoteId""=s.""Id"" and ss.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
		 union 
	 select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,assi.""Id"" as AssignmentId,
hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo
		 FROM public.""NtsNote"" as s
		  join public.""NtsNoteShared"" as ss on ss.""NtsNoteId""=s.""Id"" and ss.""IsDeleted""=false
		    join public.""Team"" as t on ss.""SharedWithTeamId""=t.""Id"" and t.""IsDeleted""=false
			 join public.""TeamUser"" as tu on t.""Id""=tu.""TeamId"" and tu.""IsDeleted""=false
		 join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
where s.""Id""='{id}' and s.""IsDeleted""=false
";
            return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
        }
        public async Task<MessageEmailViewModel> GetTaskMessageIdData(string MessageId)
        {
            var query = @$"select nt.""Id"" as TaskId,pet.""MessageId"" as MessageId
                from public.""NtsTask"" as nt
                join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
                join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false

                where 
	            pet.""MessageId""='{MessageId}' and 
	            nt.""TemplateCode""='EMAIL_TASK' and nt.""IsDeleted""=false";
            var data = await _queryRepo.ExecuteQuerySingle<MessageEmailViewModel>(query, null);
            return data;
        }
        public async Task<List<FileViewModel>> GetFileListData()
        {
            var query = $@"Select * from  public.""File""
                    where ""IsDeleted""=false and ""MongoPreviewFileId"" is null and ""FileExtension"" in ('.pptx','.doc','.docx') Order by ""LastUpdatedDate"" Desc limit 20";
            var list = await _queryRepo.ExecuteQueryList<FileViewModel>(query, null);
            return list;

        }
        public async Task<List<IdNameViewModel>> GetFileLogsDetailsData(string fileId)
        {
            var query = $@"Select ""Id"",""VersionNo"" as Name,""IsLatest"" as Code from log.""FileLog"" where ""RecordId""='{fileId}' and ""IsVersionLatest""=true";
            var data = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return data;
        }
        public async Task<List<FileViewModel>> GetFileLogsDetailsByIdData(string Id)
        {
            var query = $@"Select l.*,l.""RecordId"" as Id,l.""VersionNo"" as VersionNo,f.""FileName"" as FileName,
l.""FileExtension"" as FileExtension
from log.""FileLog"" as l
join  public.""File"" as f on f.""Id""=l.""RecordId"" and f.""IsDeleted""=false

where l.""Id""='{Id}'  ";
            var data = await _queryRepo.ExecuteQueryList<FileViewModel>(query, null);
            return data;
        }
        public async Task<FileViewModel> GetFileLogsDetailsByFileIdAndVersionData(string FileId, long versionNo)
        {
            var query = $@"Select l.*,l.""RecordId"" as Id,l.""VersionNo"" as VersionNo,f.""FileName"" as FileName,
l.""FileExtension"" as FileExtension
from log.""FileLog"" as l
join  public.""File"" as f on f.""Id""=l.""RecordId"" and f.""IsDeleted""=false

where l.""RecordId""='{FileId}' and l.""VersionNo""={versionNo} and ""IsVersionLatest""=true";
            var data = await _queryRepo.ExecuteQuerySingle<FileViewModel>(query, null);
            return data;
        }
        public async Task<IList<GrantAccessViewModel>> GetGrantAccessListData(string userId)
        {
            string query = @$"SELECT ga.*,gu.""Name"" as UserName
                            FROM public.""GrantAccess"" as ga                            
                            inner join public.""User"" as gu on gu.""Id""=ga.""UserId"" and gu.""IsDeleted""=false
                            inner join public.""User"" as u on u.""Id""=ga.""CreatedBy"" and u.""IsDeleted""=false
                            where u.""Id""='{userId}' and ga.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<GrantAccessViewModel>(query, null);
            return list;
        }
        public async Task<IList<TaskViewModel>> GetBHServiceData(bool showAllService)
        {
            var query = $@" select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate""
                            ,st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode
                            ,coalesce(s.""WorkflowStatus"", sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName
                            ,so.""Name"" as ServiceOwner
                            ,t.""Id"" as TaskActionId,t.""TaskNo"" as TaskNo,t.""TemplateCode"" as TemplateMasterCode
                            ,l.""Name"" as TaskStatusName
                            from public.""NtsService"" as s
                            join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""Code""='BH_CHANGE_REQUEST' 
                            left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false 
                            left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""IsDeleted""=false 
                            left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false 
                            join public.""User"" as so on s.""RequestedByUserId""=so.""Id"" and so.""IsDeleted""=false 
                            left join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false 
                            left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false 
                            left join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false 
                            where s.""IsDeleted""=false #OWNERUSER#
                            order by s.""LastUpdatedDate"" desc
                            ";
            var owneruser = "";
            if (!showAllService)
            {
                owneruser = $@" and (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId""='{_repo.UserContext.UserId}') ";
            }
            query = query.Replace("#OWNERUSER#", owneruser);
            var result = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return result;
        }
        public async Task<IList<TaskViewModel>> GetBHTaskData()
        {
            var query = $@" select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate""
                            ,st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode
                            ,coalesce(s.""WorkflowStatus"", sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName
                            ,so.""Name"" as ServiceOwner
                            ,t.""Id"" as TaskActionId,t.""TaskNo"" as TaskNo,t.""TemplateCode"" as TemplateMasterCode
                            ,l.""Name"" as TaskStatusName
                            ,u.""Name"" as AssigneeUserName
                            from public.""NtsService"" as s
                            join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""Code""='BH_CHANGE_REQUEST' 
                            left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false 
                            left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""IsDeleted""=false 
                            left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false 
                            join public.""User"" as so on s.""RequestedByUserId""=so.""Id"" and so.""IsDeleted""=false 
                            left join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false 
                            left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false 
                            left join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false 
                            where s.""IsDeleted""=false and t.""AssignedToUserId""='{_repo.UserContext.UserId}'
                            order by s.""LastUpdatedDate"" desc
                            ";

            var result = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return result;
        }
        public async Task<List<BusinessHierarchyPermissionViewModel>> GetBusinessHierarchyPermissionData(string groupCode)
        {
            var query = $@"select bhp.*,lov.""Name"" as PermissionName
                        from cms.""N_HR_BusinessHierarchyPermission"" as bhp
                        left join public.""LOV"" as lov on lov.""Id""=bhp.""PermissionId"" and lov.""IsDeleted""=false
                        where bhp.""IsDeleted""=false #WHERE#
                            ";
            var where = "";
            if (groupCode.IsNotNullAndNotEmpty())
            {
                where = $@" and lov.""GroupCode""='{groupCode}' ";
            }
            query = query.Replace("#WHERE#", where);
            var list = await _queryRepo.ExecuteQueryList<BusinessHierarchyPermissionViewModel>(query, null);
            return list;
        }
        public async Task<string> GetBusinessHierarchyPermissionData2(string users)
        {
            var query1 = $@" select string_agg(u.""Name""::text, ', ') as username
                                    from public.""User"" as u
                                    where u.""IsDeleted"" = false and u.""Id"" IN ({users})
                                    ";
            var data = await _queryRepo.ExecuteScalar<string>(query1, null);
            return data;
        }
        public async Task<bool> DeleteBusinessHierarchyPermissionData(string id, string noteId)
        {

            var query = $@" update cms.""N_HR_BusinessHierarchyPermission""
                            set ""IsDeleted""=true
                            where ""Id""='{id}' and ""NtsNoteId""='{noteId}' ";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<List<HybridHierarchyViewModel>> GetHierarchyPathData(string hierarchyItemId)
        {
            var query = $@"with recursive hrchy AS(
	         select ""Id"",""ParentId"" from public.""HybridHierarchy"" where ""IsDeleted""=false 
			and ""Id""='{hierarchyItemId}'
             union all
	         select h.""Id"",h.""ParentId"" from public.""HybridHierarchy""  as h
	         join hrchy on h.""Id""=hrchy.""ParentId"" where h.""IsDeleted""=false 
	         )select ""Id"",""ParentId"" from hrchy where ""ParentId"" is not null ";
            var list = await _queryRepo.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            return list;
        }

        public async Task<List<HybridHierarchyViewModel>> GetHierarchyParentDetailsData(string hierarchyItemId)
        {
            var query = $@"with recursive hrchy AS(
	         select *,1 ""Level"" from public.""HybridHierarchy"" where ""IsDeleted""=false 
			and ""Id""='{hierarchyItemId}'
             union all
	         select h.*,hrchy.""Level""+1 ""Level"" from public.""HybridHierarchy""  as h
	         join hrchy on h.""Id""=hrchy.""ParentId"" where h.""IsDeleted""=false 
	         )select hrchy.*,r.""Name"" from hrchy
			 left join
			 (
				 select ""Id"",null as EmployeeId,""DepartmentName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""CareerLevel"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_CareerLevel"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""JobTitle"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""PersonFullName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPerson"" where ""IsDeleted""=false
				union
		        select ""Id"",null as EmployeeId,""PositionName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPosition"" where ""IsDeleted""=false
				union
		        select '-1' ""Id"",null as EmployeeId,""Name"" as ""Name"",null ""NtsNoteId"" from public.""HierarchyMaster"" where ""IsDeleted""=false 
				and ""Code""='BUSINESS_HIERARCHY'
			 ) r on coalesce(hrchy.""ReferenceId"",'-1')=r.""Id"" order by hrchy.""Level"" desc ";
            var list = await _queryRepo.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            return list;

        }
        public async Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyChildListData(string parentId, int level, int levelupto, bool enableAOR, string bulkRequestId, bool includeParent, string userId, bool isAdmin)
        {


            var query = $@" with recursive hrchy AS(
	         select *,1 ""LevelUpto"",{level} ""Level"" from public.""HybridHierarchy""  
	         where ""IsDeleted""=false and ""{(includeParent ? "Id" : "ParentId")}""='{parentId}'
             union all
	         select h.*,hrchy.""LevelUpto""+1 ""LevelUpto"",hrchy.""Level""+1 ""Level"" from public.""HybridHierarchy""  as h
	         join hrchy on h.""ParentId""=hrchy.""Id"" where h.""IsDeleted""=false and ""LevelUpto""<{(includeParent ? "=" : "")}{levelupto}
	         )
            select hrchy.""Id"",hrchy.""ParentId"",reference.""NtsNoteId"" as ""NtsId"",reference.""EmployeeId"",
            hrchy.""ReferenceType"",hrchy.""ReferenceId"",hrchy.""Level"" ""LevelId"",'SERVICE_STATUS_COMPLETE' as ""StatusCode""
	         ,hrchy.""LevelUpto"",COALESCE(cc.""DirectChildCount"",0)+COALESCE(scc.""DirectChildCount"",0) as ""DirectChildCount""
            ,COALESCE(acc.""AllChildCount"",0) as ""AllChildCount"",reference.""Name"" 
            ,case when '{isAdmin}'='true' then true when hp.""HierarchyId"" is not null then true else false end ""HasResponibility""
            ,null ""WorkflowStatus"" ,null ""DueDate"",'1,2,3' as ""PermissionCodes"",hrchy.""BulkRequestId"",hrchy.""HierarchyPath""
            from hrchy 
	        left join(
	 	        select ""ParentId"",count(""Id"") ""DirectChildCount"" from public.""HybridHierarchy""  where ""IsDeleted""=false
		        group by ""ParentId""
	         ) cc on hrchy.""Id""=cc.""ParentId""
            left join(
	 	         select p.""Id"",count(p.""Id"") ""AllChildCount"" from public.""HybridHierarchy"" p
                 join public.""HybridHierarchy"" c on p.""Id""=ANY(c.""HierarchyPath"")
                 where p.""IsDeleted""=false and c.""IsDeleted""=false   group by p.""Id""
	         ) acc on hrchy.""Id""=acc.""Id""
            left join(
	 	        select distinct ""BusinessHierarchyId"" ""HierarchyId"" from cms.""N_HR_BusinessHierarchyAOR""  where ""IsDeleted""=false
		        and ""UserId""='{userId}'
	         ) hp on hrchy.""Id""=hp.""HierarchyId""
            left join(
			with service as(
			select n.""BusinessHierarchyParentId"" as ""ParentId"",count(s.""Id"") as ""DirectChildCount""            
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_DepartmentRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' --and n.""BusinessHierarchyParentId""='{parentId}' 
            group by n.""BusinessHierarchyParentId""
            union
            select n.""BusinessHierarchyParentId"" as ""ParentId"", count(s.""Id"") as ""DirectChildCount"" 
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_JobRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' --and n.""BusinessHierarchyParentId""='{parentId}'
            group by n.""BusinessHierarchyParentId""
            union
            select n.""BusinessHierarchyParentId"" as ""ParentId"", count(s.""Id"") as ""DirectChildCount""  
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_CareerLevelRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' --and n.""BusinessHierarchyParentId""='{parentId}'
            group by n.""BusinessHierarchyParentId""
            union
            select n.""BusinessHierarchyParentId"" as ""ParentId"", count(s.""Id"") as ""DirectChildCount""  
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_NewEmployeeRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' --and n.""BusinessHierarchyParentId""='{parentId}'
            group by n.""BusinessHierarchyParentId""
            union
            select n.""BusinessHierarchyParentId"" as ""ParentId"", count(s.""Id"") as ""DirectChildCount""  
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_NewPositionRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' --and n.""BusinessHierarchyParentId""='{parentId}'

                     group by n.""BusinessHierarchyParentId""
				 )select ""ParentId"",sum(""DirectChildCount"") as ""DirectChildCount"" from service group by ""ParentId""
	 	        
	         ) scc on hrchy.""Id""=scc.""ParentId""
	         left join(
                select ""Id"",null as ""EmployeeId"",""DepartmentName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as ""EmployeeId"",""CareerLevel"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_CareerLevel"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as ""EmployeeId"",""JobTitle"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false
		        union
		        select ""Id"",""Id"" as ""EmployeeId"",""PersonFullName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPerson"" where ""IsDeleted""=false
			    union
		        select ""Id"",null as ""Id"",""PositionName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPosition"" where ""IsDeleted""=false
			    union
		        select '-1' ""Id"",null as ""EmployeeId"",""Name"" as ""Name"",null ""NtsNoteId"" from public.""HierarchyMaster"" where ""IsDeleted""=false
                and ""Code""='BUSINESS_HIERARCHY'
	         ) reference on hrchy.""ReferenceId""=reference.""Id""
            union
            select s.""UdfNoteTableId"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
            ,'DEPARTMENT_SERVICE' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
            ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""DepartmentName"" as ""Name""
            ,false ""HasResponsibility"",s.""WorkflowStatus"",s.""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_DepartmentRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BusinessHierarchyParentId""='{parentId}'
            union
            select s.""UdfNoteTableId"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
            ,'JOB_SERVICE' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
            ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""JobTitle"" as ""Name""
            ,false ""HasResponsibility"",s.""WorkflowStatus"",s.""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_JobRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BusinessHierarchyParentId""='{parentId}'
            union
            select s.""UdfNoteTableId"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
            ,'CAREER_LEVEL_SERVICE' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
            ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""CareerLevel"" as ""Name""
            ,false ""HasResponsibility"",s.""WorkflowStatus"",s.""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_CareerLevelRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BusinessHierarchyParentId""='{parentId}'
            union
            select s.""UdfNoteTableId"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
            ,'PERSON_SERVICE' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
            ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""FirstName"" as ""Name""
            ,false ""HasResponsibility"",s.""WorkflowStatus"",s.""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_NewEmployeeRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BusinessHierarchyParentId""='{parentId}'
            union
            select s.""UdfNoteTableId"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
            ,'POSITION_SERVICE' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
            ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""PositionName"" as ""Name""
            ,false ""HasResponsibility"",s.""WorkflowStatus"",s.""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
            from public.""NtsService"" s
            join cms.""N_SNC_CHR_NewPositionRequest"" n on s.""UdfNoteTableId""=n.""Id"" and n.""IsDeleted""=false
            join public.""LOV"" l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
            where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BusinessHierarchyParentId""='{parentId}'";

            if (bulkRequestId.IsNotNullAndNotEmpty())
            {
                //list = list.Where(x => x.BulkRequestId == bulkRequestId).ToList();
                query = $@"{query} 
                union
                select n.""Id"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
                ,'BULK_REQUEST' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
                ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""DepartmentName"" as ""Name""
                ,false ""HasResponsibility"",null ""WorkflowStatus"",null ""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
                from public.""NtsNote"" s
                join cms.""N_CoreHR_NewDepartmentCreate"" n on s.""Id""=n.""NtsNoteId"" and n.""IsDeleted""=false
                join public.""LOV"" l on s.""NoteStatusId""=l.""Id"" and l.""IsDeleted""=false
                left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
                where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BulkRequestId""='{bulkRequestId}'
                union
                select n.""Id"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
                ,'BULK_REQUEST' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
                ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""JobTitle"" as ""Name""
                ,false ""HasResponsibility"",null ""WorkflowStatus"",null ""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
                from public.""NtsNote"" s
                join cms.""N_CoreHR_NewJobCreate"" n on s.""Id""=n.""NtsNoteId"" and n.""IsDeleted""=false
                join public.""LOV"" l on s.""NoteStatusId""=l.""Id"" and l.""IsDeleted""=false
                left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
                where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BulkRequestId""='{bulkRequestId}'
                union
                select n.""Id"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
                ,'BULK_REQUEST' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
                ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""CareerLevel"" as ""Name""
                ,false ""HasResponsibility"",null ""WorkflowStatus"",null ""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
                from public.""NtsNote"" s
                join cms.""N_CoreHR_NewCareerLevelCreate"" n on s.""Id""=n.""NtsNoteId"" and n.""IsDeleted""=false
                join public.""LOV"" l on s.""NoteStatusId""=l.""Id"" and l.""IsDeleted""=false
                left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
                where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BulkRequestId""='{bulkRequestId}'
                union
                select n.""Id"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
                ,'BULK_REQUEST' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
                ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""PositionName"" as ""Name""
                ,false ""HasResponsibility"",null ""WorkflowStatus"",null ""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
                from public.""NtsNote"" s
                join cms.""N_CoreHR_NewPositionCreate"" n on s.""Id""=n.""NtsNoteId"" and n.""IsDeleted""=false
                join public.""LOV"" l on s.""NoteStatusId""=l.""Id"" and l.""IsDeleted""=false
                left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
                where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BulkRequestId""='{bulkRequestId}'
                union
                select n.""Id"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
                ,'BULK_REQUEST' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
                ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""NewDepartmentName"" as ""Name""
                ,false ""HasResponsibility"",null ""WorkflowStatus"",null ""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
                from public.""NtsNote"" s
                join cms.""N_CoreHR_RenameDepartment"" n on s.""Id""=n.""NtsNoteId"" and n.""IsDeleted""=false
                join public.""LOV"" l on s.""NoteStatusId""=l.""Id"" and l.""IsDeleted""=false
                left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
                where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BulkRequestId""='{bulkRequestId}'
                union
                select n.""Id"" as ""Id"",n.""BusinessHierarchyParentId"" as ""ParentId"",s.""Id"" as ""NtsId"",null as EmployeeId
                ,'BULK_REQUEST' as ""ReferenceType"",null as ""ReferenceId"",coalesce(h.""LevelId"",0)+1 as ""LevelId"",l.""Code"" as ""StatuCode"" 
                ,1 as ""LevelUpto"",0 as ""DirectChildCount"",0 as ""AllChildCount"",n.""NewJobName"" as ""Name""
                ,false ""HasResponsibility"",null ""WorkflowStatus"",null ""DueDate"",null as ""PermissionCodes"",null ""BulkRequestId"",null ""HierarchyPath""
                from public.""NtsNote"" s
                join cms.""N_CoreHR_RenameJob"" n on s.""Id""=n.""NtsNoteId"" and n.""IsDeleted""=false
                join public.""LOV"" l on s.""NoteStatusId""=l.""Id"" and l.""IsDeleted""=false
                left join public.""HybridHierarchy"" h on n.""BusinessHierarchyParentId""=h.""Id"" and h.""IsDeleted""=false
                where s.""IsDeleted""=false and l.""Code""<>'SERVICE_STATUS_COMPLETE' and n.""BulkRequestId""='{bulkRequestId}'
                ";
            }
            var list = await _queryRepo.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            return list;
        }
        public async Task UpdateHierarchyPathData(HybridHierarchyViewModel hybridmodel, string values)
        {

            var query = $@" update public.""HybridHierarchy""
                            set ""HierarchyPath""='{values}',
                            ""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',
                            ""LastUpdatedBy""='{_repo.UserContext.UserId}'
                            where ""Id""='{hybridmodel.Id}'";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyListData(string referenceType = null, string searckKey = null, bool bindPath = false)
        {
            var query = $@"select h.*,r.""Name"" from public.""HybridHierarchy"" h 
			left join
			(
				select ""Id"",null as EmployeeId,""DepartmentName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""CareerLevel"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_CareerLevel"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""JobTitle"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""PersonFullName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPerson"" where ""IsDeleted""=false
			    union
		        select ""Id"",null as EmployeeId,""PositionName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPosition"" where ""IsDeleted""=false
			    union
		        select '-1' ""Id"",null as EmployeeId,""Name"" as ""Name"",null ""NtsNoteId"" from public.""HierarchyMaster"" where ""IsDeleted""=false 
			    and ""Code""='BUSINESS_HIERARCHY'
			) r on coalesce(h.""ReferenceId"",'-1')=r.""Id"" where h.""IsDeleted""=false ";
            if (referenceType.IsNotNullAndNotEmpty())
            {
                var reference = referenceType.ToEnum<BusinessHierarchyItemTypeEnum>();
                switch (reference)
                {
                    case BusinessHierarchyItemTypeEnum.ROOT:
                        query = $@"select h.*,r.""Name"" ""Name"" from public.""HybridHierarchy"" h 
                        join cms.""HierarchyMaster"" r on h.""Id""=r.""Id""
                        where h.""IsDeleted""=false and r.""IsDeleted""=false and h.""ReferenceType""='{referenceType}'";
                        break;
                    case BusinessHierarchyItemTypeEnum.LEVEL1:
                    case BusinessHierarchyItemTypeEnum.LEVEL2:
                    case BusinessHierarchyItemTypeEnum.LEVEL3:
                    case BusinessHierarchyItemTypeEnum.LEVEL4:
                    case BusinessHierarchyItemTypeEnum.BRAND:
                    case BusinessHierarchyItemTypeEnum.MARKET:
                    case BusinessHierarchyItemTypeEnum.PROVINCE:
                    case BusinessHierarchyItemTypeEnum.DEPARTMENT:
                        query = $@"select h.*,r.""DepartmentName"" ""Name"" from public.""HybridHierarchy"" h 
                        join cms.""N_CoreHR_HRDepartment"" r on h.""ReferenceId""=r.""Id""
                        where h.""IsDeleted""=false and r.""IsDeleted""=false and h.""ReferenceType""='{referenceType}'";
                        break;
                    case BusinessHierarchyItemTypeEnum.CAREER_LEVEL:
                        query = $@"select h.*,r.""CareerLevel"" ""Name"" from public.""HybridHierarchy"" h 
                        join cms.""N_CoreHR_CareerLevel"" r on h.""ReferenceId""=r.""Id""
                        where h.""IsDeleted""=false and r.""IsDeleted""=false and h.""ReferenceType""='{referenceType}'";
                        break;
                    case BusinessHierarchyItemTypeEnum.JOB:
                        query = $@"select h.*,r.""JobTitle"" ""Name"" from public.""HybridHierarchy"" h 
                        join cms.""N_CoreHR_HRJob"" r on h.""ReferenceId""=r.""Id""
                        where h.""IsDeleted""=false and r.""IsDeleted""=false and h.""ReferenceType""='{referenceType}'";
                        break;
                    case BusinessHierarchyItemTypeEnum.POSITION:
                        query = $@"select h.*,r.""PositionName"" ""Name"" from public.""HybridHierarchy"" h 
                        join cms.""N_CoreHR_HRPosition"" r on h.""ReferenceId""=r.""Id""
                        where h.""IsDeleted""=false and r.""IsDeleted""=false and h.""ReferenceType""='{referenceType}'";
                        break;
                    case BusinessHierarchyItemTypeEnum.EMPLOYEE:
                        query = $@"select h.*,r.""PersonFullName"" ""Name"" from public.""HybridHierarchy"" h 
                        join cms.""N_CoreHR_HRPerson"" r on h.""ReferenceId""=r.""Id""
                        where h.""IsDeleted""=false and r.""IsDeleted""=false and h.""ReferenceType""='{referenceType}'";
                        break;
                    default:
                        break;
                }
            }

            var list = await _queryRepo.ExecuteQueryList<HybridHierarchyViewModel>(query, null);

            return list;
        }
        public async Task<List<UserHierarchyPermissionViewModel>> GetBusinessHierarchyRootPermissionData(string PermissionId = null, string UserId = null)
        {
            var query = $@"select uhp.""Id"",u.""Id"" as ""UserId"",h.""Id"" as ""CustomRootId"",h.""ReferenceType"" as ""ReferenceType"",r.""Name"",u.""Name"" as ""UserName"" 
             from public.""UserHierarchyPermission"" uhp 
            join public.""HierarchyMaster"" hm on uhp.""HierarchyId""=hm.""Id"" and hm.""Code""='BUSINESS_HIERARCHY'
            join public.""HybridHierarchy"" h on uhp.""CustomRootId""=h.""Id""
            join public.""User"" u on uhp.""UserId""=u.""Id""
			left join
			(
				select ""Id"",null as EmployeeId,""DepartmentName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""CareerLevel"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_CareerLevel"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""JobTitle"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false
		        union
		        select ""Id"",null as EmployeeId,""PersonFullName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPerson"" where ""IsDeleted""=false
			    union
		        select ""Id"",null as EmployeeId,""PositionName"" as ""Name"",""NtsNoteId"" from cms.""N_CoreHR_HRPosition"" where ""IsDeleted""=false
			    union
		        select '-1' ""Id"",null as EmployeeId,""Name"" as ""Name"",null ""NtsNoteId"" from public.""HierarchyMaster"" where ""IsDeleted""=false 
			and ""Code""='BUSINESS_HIERARCHY'
			) r on coalesce(h.""ReferenceId"",'-1')=r.""Id"" 
            where uhp.""IsDeleted""=false and h.""IsDeleted""=false  and u.""IsDeleted""=false  and hm.""IsDeleted""=false  #WHERE# ";
            var where = "";
            if (PermissionId.IsNotNullAndNotEmpty() && UserId.IsNotNullAndNotEmpty())
            {
                where = $@"and uhp.""Id""='{PermissionId}' and u.""Id""='{UserId}' ";
            }

            query = query.Replace("#WHERE#", where);

            var list = await _queryRepo.ExecuteQueryList<UserHierarchyPermissionViewModel>(query, null);
            return list;
        }
        public async Task<List<BusinessHierarchyAORViewModel>> GetAllAORBusinessHierarchyListData()
        {
            var query = "";
            query = $@"select distinct
                            hh.""Id"" as BusinessHierarchyId,
                            hh.""ReferenceType"" as ReferenceType,
                            aor.""Id"" as Id ,
                            aor.""NtsNoteId"" as NtsNoteId ,
                            case 
                            when hr.""Id"" is not null then hr.""Id"" 
                            --when cl.""Id"" is not null then cl.""Id""
                           -- when j.""Id"" is not null then j.""Id""
                           -- when p.""Id"" is not null then p.""Id""
                            end as ReferenceId,
                            case 
                            when hr.""DepartmentName"" is not null then hr.""DepartmentName"" 
                           -- when cl.""CareerLevel"" is not null then cl.""CareerLevel""
                          --  when j.""JobTitle"" is not null then j.""JobTitle""
                          --  when p.""PersonFullName"" is not null then p.""PersonFullName""
                            end as ReferenceName,
                            case 
                            when hr1.""Id"" is not null then hr1.""Id"" 
                    --        when cl1.""Id"" is not null then cl1.""Id""
                        --    when j1.""Id"" is not null then j1.""Id""
                        --    when p1.""Id"" is not null then p1.""Id""
                            end as ParentId,
                            case 
                            when hr1.""DepartmentName"" is not null then hr1.""DepartmentName"" 
                       --     when cl1.""CareerLevel"" is not null then cl1.""CareerLevel""
                        --    when j1.""JobTitle"" is not null then j1.""JobTitle""
                        --    when p1.""PersonFullName"" is not null then p1.""PersonFullName""
                            end as ParentName,
                            u. ""Id"" as UserId,
                            u. ""Name"" as UserName
                            from
                            cms.""N_HR_BusinessHierarchyAOR"" as aor
                            left join public.""HybridHierarchy"" as hh 
                            on hh.""Id"" = aor.""BusinessHierarchyId""
                            left join public.""HybridHierarchy"" as hp 
                            on hp.""Id"" = hh.""ParentId""
                            left join cms.""N_CoreHR_HRDepartment"" as hr
                            on hh.""ReferenceId"" = hr.""Id"" and 
                            (hh.""ReferenceType"" = 'OrgLevel1' or hh.""ReferenceType"" = 'OrgLevel2' or hh.""ReferenceType"" = 'OrgLevel3' or hh.""ReferenceType"" = 'OrgLevel4'
                            or hh.""ReferenceType"" = 'Brand' or hh.""ReferenceType"" = 'Market' or hh.""ReferenceType"" = 'Province' or  hh.""ReferenceType"" = 'Department')
                           -- left join cms.""N_CoreHR_CareerLevel"" as cl
                           -- on hh.""ReferenceId"" = cl.""Id"" and 
                           -- (hh.""ReferenceType"" = 'CareerLevel')
                           -- left join cms.""N_CoreHR_HRJob"" as j
                           -- on hh.""ReferenceId"" = j.""Id"" and
                            --(hh.""ReferenceType"" = 'Job')
                            --left join cms.""N_CoreHR_HRPerson"" as p
                          --  on hh.""ReferenceId"" =p.""Id"" and 
                           -- (hh.""ReferenceType"" = 'Employee')

                            left join cms.""N_CoreHR_HRDepartment"" as hr1
                            on hp.""ReferenceId"" = hr1.""Id"" and 
                            (hp.""ReferenceType"" = 'OrgLevel1' or hp.""ReferenceType"" = 'OrgLevel2' or hp.""ReferenceType"" = 'OrgLevel3' or hp.""ReferenceType"" = 'OrgLevel4'
                            or hp.""ReferenceType"" = 'Brand' or hp.""ReferenceType"" = 'Market' or hp.""ReferenceType"" = 'Province' or  hp.""ReferenceType"" = 'Department')
                          --  left join cms.""N_CoreHR_CareerLevel"" as cl1
                          --  on hp.""ReferenceId"" = cl1.""Id"" and 
                          --  (hp.""ReferenceType"" = 'CareerLevel')
                          --  left join cms.""N_CoreHR_HRJob"" as j1
                          --  on hp.""ReferenceId"" = j1.""Id"" and
                          --  (hp.""ReferenceType"" = 'Job')
                           -- left join cms.""N_CoreHR_HRPerson"" as p1
                          --  on hp.""ReferenceId"" =p1.""Id"" and 
                           -- (hp.""ReferenceType"" = 'Employee')

                            left join public.""User"" as u
                            on u.""Id"" = aor.""UserId""

                            where hh.""PortalId"" = '{_userContext.PortalId}' and aor.""IsDeleted"" = false
                            --and hh.""ReferenceId"" is not null
                            --and hh.""ReferenceId"" = '44bb63d9-0e40-4292-9f05-de169ebdc874' --and u.""Id"" = '201293ba-fd81-409b-b7b2-74edd69d21e4'
                ";
            var list = await _queryRepo.ExecuteQueryList<BusinessHierarchyAORViewModel>(query, null);
            return list;
        }
        public async Task<List<LegalEntityViewModel>> GetData()
        {

            string query = @$"SELECT c.*, co.""Name"" as CountryName
                            FROM public.""LegalEntity"" as c                           
                            LEFT JOIN cms.""Country"" as co ON co.""Id"" = c.""CountryId"" and co.""IsDeleted""=false
                            where c.""IsDeleted"" = false";
            var list = await _queryRepo.ExecuteQueryList<LegalEntityViewModel>(query, null);
            return list;
        }
        public async Task<LegalEntityViewModel> GetFinancialYearData()
        {

            var cypher = $@"SELECT l.* FROM public.""LegalEntity"" as l
            join public.""User"" as u on u.""LegalEntityId"" = l.""Id""  and u.""IsDeleted""=false
            where u.""Id""='{_repo.UserContext.UserId}' and l.""IsDeleted""=false";

            var legalEntity = await _queryRepo.ExecuteQuerySingle<LegalEntityViewModel>(cypher, null);
            return legalEntity;
        }
        public async Task<List<MenuGroupViewModel>> GetMenuGroupListData()
        {
            var query = @$"select mg.*,p.""Name"" as ""PortalName"" from public.""MenuGroup"" as mg
            --left join public.""SubModule"" as sm on mg.""SubModuleId""=sm.""Id"" and sm.""IsDeleted""=false
            left join public.""Portal"" as p on mg.""PortalId""=p.""Id"" and p.""IsDeleted""=false
            where mg.""IsDeleted""=false";
            return await _queryRepo.ExecuteQueryList<MenuGroupViewModel>(query, null);
        }
        public async Task<List<MenuGroupViewModel>> GetMenuGroupListPortalAdminData(string PortalId, string LegalEntityId)
        {
            var query = @$"select mg.*,sm.""Name"" as ""SubModule"",p.""Name"" as ""PortalName"" from public.""MenuGroup"" as mg
            left join public.""SubModule"" as sm on mg.""SubModuleId""=sm.""Id"" and sm.""IsDeleted""=false and mg.""CompanyId"" = '{_repo.UserContext.CompanyId}' and sm.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            left join public.""Portal"" as p on mg.""PortalId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            where mg.""IsDeleted""=false  and mg.""PortalId""='{PortalId}' and mg.""LegalEntityId""='{LegalEntityId}'
             and sm.""PortalId""='{PortalId}' and sm.""LegalEntityId""='{LegalEntityId}'";
            return await _queryRepo.ExecuteQueryList<MenuGroupViewModel>(query, null);
        }
        public async Task<List<MenuGroupViewModel>> GetMenuGroupListparentPortalAdminData(string PortalId, string LegalEntityId, string Id)
        {
            var query = @$"select mg.*,p.""Name"" as ""SubModule"",p.""Name"" as ""PortalName"" from public.""MenuGroup"" as mg
            left join public.""SubModule"" as sm on mg.""SubModuleId""=sm.""Id"" and sm.""IsDeleted""=false and mg.""CompanyId"" = '{_repo.UserContext.CompanyId}' and sm.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            left join public.""Portal"" as p on mg.""PortalId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            where mg.""IsDeleted""=false  and mg.""PortalId""='{PortalId}' and mg.""LegalEntityId""='{LegalEntityId}'
             and sm.""PortalId""='{PortalId}' and sm.""LegalEntityId""='{LegalEntityId}' and mg.""Id""!='{Id}'";
            return await _queryRepo.ExecuteQueryList<MenuGroupViewModel>(query, null);
        }
        public async Task<List<TagCategoryViewModel>> GetNtsTagData(NtsTypeEnum ntstype, string ntsId)
        {
            var query = @$"  select tc.""TagCategoryName"" as Name,nts.""Id"" as Id,n.""NoteSubject"" as TagName,
nts.""CreatedDate"" as CreatedDate,u.""Name"" as CreatedByName,nts.""TagId"" as TagId,p.""Id"" as ParentNoteId,
nts.""LastUpdatedDate"" as LastUpdatedDate,lu.""Name"" as LastUpdatedByName
from public.""NtsTag"" as nts
join public.""User"" as u on u.""Id""=nts.""CreatedBy"" and nts.""IsDeleted""=false and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as lu on lu.""Id""=nts.""LastUpdatedBy"" and lu.""IsDeleted""=false and lu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as p on p.""Id"" = nts.""TagCategoryId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_General_TagCategory"" as tc on tc.""NtsNoteId""=p.""Id"" and tc.""IsDeleted""=false  and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on n.""Id"" = nts.""TagId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_General_Tag"" as t on t.""NtsNoteId""=n.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
where nts.""NtsType""='{(int)((NtsTypeEnum)Enum.Parse(typeof(NtsTypeEnum), ntstype.ToString()))}' and nts.""NtsId""='{ntsId}' and nts.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<TagCategoryViewModel> GetTagCategoryDataByIdData(string categoryId)
        {
            var query = @$"  select *,""NtsNoteId"" as NoteId from  cms.""N_General_TagCategory"" where ""NtsNoteId""='{categoryId}' and ""IsDeleted""=false";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<TagCategoryViewModel> GetTagCategoryDataByNoteIdData(string NoteId)
        {
            var query = @$"  select *,""NtsNoteId"" as NoteId from  cms.""N_General_TagCategory"" where ""NtsNoteId""='{NoteId}' and ""IsDeleted""=false";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<TagViewModel> GetTagByNoteIdData(string NoteId)
        {
            var query = @$"  select *,""NtsNoteId"" as NoteId from  cms.""N_General_Tag"" where ""NtsNoteId""='{NoteId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TagViewModel>> TagListByCategoryNoteIdData(string NoteId)
        {
            var query = @$"  select tag.*,c.""NoteSubject"" as NoteSubject,tag.""NtsNoteId"" as NoteId,tc.""NtsNoteId"" as ParentNoteId from 
public.""NtsNote"" as p join 
cms.""N_General_TagCategory"" as tc on tc.""NtsNoteId""=p.""Id"" and p.""Id""='{NoteId}' and tc.""IsDeleted""=false
join public.""NtsNote"" as c on c.""ParentNoteId""= p.""Id""
join cms.""N_General_Tag"" as tag on tag.""NtsNoteId""=c.""Id""
and tag.""IsDeleted""=false";
            var querydata = await _queryRepo.ExecuteQueryList<TagViewModel>(query, null);
            return querydata;
        }
        public async Task<List<PageViewModel>> GetMenuItemsData(Portal portal, string userId, string companyId, string groupCode = null)
        {
            var language = Helper.GetLanguage(_repo.UserContext.CultureName);
            var uId = _repo.UserContext.UserId.IsNullOrEmpty() ? userId : _repo.UserContext.UserId;
            var cId = _repo.UserContext.CompanyId.IsNullOrEmpty() ? companyId : _repo.UserContext.CompanyId;
            var query = @$"select pa.*,upe.""Id"" as UserPermissionId
                ,coalesce(""rlTitle"".""{language}"",pa.""Title"") as ""Title""
                ,coalesce(""rlMenuName"".""{language}"",pa.""MenuName"") as ""MenuName""
                ,coalesce(""rlMenuGroupName"".""{language}"",mg.""Name"") as ""MenuGroupName""
                ,up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss,mg.""ShortName"" as ""MenuGroupShortName""
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle' and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName' and ""rlMenuName"".""IsDeleted""=false
                
                left join public.""UserPortal"" as up on po.""Id""=up.""PortalId""
                and up.""IsDeleted""=false and up.""UserId""='{uId}' 
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName' and ""rlMenuGroupName"".""IsDeleted""=false
                left join public.""UserPermission"" as upe on pa.""Id""=upe.""PageId"" 
                and upe.""IsDeleted""=false and upe.""UserId""='{uId}'
                where  pa.""PortalId""='{portal.Id}' and pa.""CompanyId""='{cId}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false #GCWHERE#
                union
               select pa.*,upe.""Id"" as UserPermissionId
                ,coalesce(""rlTitle"".""{language}"",pa.""Title"") as ""Title""
                ,coalesce(""rlMenuName"".""{language}"",pa.""MenuName"") as ""MenuName""
                ,coalesce(""rlMenuGroupName"".""{language}"",mg.""Name"") as ""MenuGroupName""
                ,up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss,mg.""ShortName"" as ""MenuGroupShortName""
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle'  and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName'  and ""rlMenuName"".""IsDeleted""=false
                
                left join 
                ( select up.* from 
                public.""UserRolePortal"" as up 
                join public.""UserRole"" as ur on up.""UserRoleId""=ur.""Id""
                and ur.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru on ur.""Id""=uru.""UserRoleId""
                and uru.""IsDeleted""=false and uru.""UserId""='{uId}' 
                where up.""PortalId""='{portal.Id}' and up.""IsDeleted""=false 
                ) as up on po.""Id""=up.""PortalId""
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName'  and ""rlMenuGroupName"".""IsDeleted""=false
                left join 
                ( select upe.*
                from public.""UserRolePermission"" as upe 
                join public.""UserRole"" as ur2 on upe.""UserRoleId""=ur2.""Id""
                and ur2.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru2 on ur2.""Id""=uru2.""UserRoleId""
                and uru2.""IsDeleted""=false and uru2.""UserId""='{uId}' 
                where  upe.""IsDeleted""=false 
                )upe on pa.""Id""=upe.""PageId""
                where  pa.""PortalId""='{portal.Id}' and pa.""CompanyId""='{cId}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false #GCWHERE#";
            if (_repo.UserContext.IsSystemAdmin)
            {
                query = @$"select pa.* ,null as UserPermissionId,true as ""IsSystemAdmin""
                ,coalesce(""rlTitle"".""{language}"",pa.""Title"") as ""Title""
                ,coalesce(""rlMenuName"".""{language}"",pa.""MenuName"") as ""MenuName""
                ,coalesce(""rlMenuGroupName"".""{language}"",mg.""Name"") as ""MenuGroupName""
                ,po.""Id"" as UserPortalId,null as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss,mg.""ShortName"" as ""MenuGroupShortName""
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle' and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName' and ""rlMenuName"".""IsDeleted""=false
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName' and ""rlMenuGroupName"".""IsDeleted""=false
                --left join  public.""UserPermission"" as upe on pa.""Id""=upe.""PageId"" 
                --and upe.""IsDeleted""=false and upe.""UserId""='{uId}'
                where  pa.""PortalId""='{portal.Id}' and pa.""CompanyId""='{cId}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false #GCWHERE#";
            }

            string gcwhere = "";
            if (groupCode.IsNotNullAndNotEmpty())
            {
                gcwhere = $@" and pa.""GroupCode""='{groupCode}' ";
            }
            query = query.Replace("#GCWHERE#", gcwhere);

            var pages = await _queryRepo.ExecuteQueryList<PageViewModel>(query, null);
            return pages;
        }
        public async Task<List<MenuGroupDetailsViewModel>> GetMenuItemsData1(string ids)
        {
            var menGroupSlides = await _queryRepo.ExecuteQueryList<MenuGroupDetailsViewModel>(
                $@"select * from public.""MenuGroupDetails""  where ""IsDeleted""=false and ""MenuGroupId"" in ({ids})", null);
            return menGroupSlides;
        }
        public async Task<List<PageDetailsViewModel>> GetMenuItemsData2(string ids)
        {
            var menuPageSlides = await _queryRepo.ExecuteQueryList<PageDetailsViewModel>(
                  $@"select * from public.""PageDetails""  where  ""IsDeleted""=false and ""PageId"" in ({ids})", null);
            return menuPageSlides;

        }
        public async Task<List<PageViewModel>> GetPortalDiagramData(PortalViewModel portal, string userId, string companyId)
        {
            var pages = await _queryRepo.ExecuteQueryList<PageViewModel>(
            @$"select mg.""Id"" as ""TempId"",null as ""BoxParentId"",mg.""Name"" as ""TempName"" , 'MenuGroup' as ""BoxType"" 
                    from public.""MenuGroup"" mg where  
                    mg.""PortalId""='{portal.Id}' and mg.""IsDeleted""=false 
                    union
                    select pa.""Id"" as ""TempId"",mg.""Id"" as ""BoxParentId"",pa.""Name"" as ""TempName"" , 'Menu' as ""BoxType"" 
                    from public.""MenuGroup"" mg join public.""Page"" as pa on mg.""Id""=pa.""MenuGroupId"" and pa.""IsDeleted""=false 
                    where  mg.""PortalId""='{portal.Id}' and mg.""IsDeleted""=false 
                    union
                    select tmpc.""Id"" as ""TempId"",pa.""Id"" as ""BoxParentId"",tmpc.""Name"" as ""TempName"" , 'CATEGORY' as ""BoxType"" 
                    from public.""MenuGroup"" mg join public.""Page"" as pa on mg.""Id""=pa.""MenuGroupId"" and pa.""IsDeleted""=false
                    join public.""Template"" tmp on pa.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false and tmp.""TemplateType""=7
                    join public.""CustomTemplate"" ctmp on tmp.""Id""=ctmp.""TemplateId"" and ctmp.""IsDeleted""=false  
                    join public.""TemplateCategory"" tmpc on tmpc.""Id"" = Any ( ctmp.""AllowedCategories"") and tmpc.""IsDeleted""=false  
                    where  mg.""PortalId""='{portal.Id}' and mg.""IsDeleted""=false 
                    union
                    select tmpctmp.""Id"" as ""TempId"",tmpc.""Id"" as ""BoxParentId"",tmpctmp.""Name"" as ""TempName"" , 'TEMPLATE' as ""BoxType"" 
                    from public.""MenuGroup"" mg join public.""Page"" as pa on mg.""Id""=pa.""MenuGroupId"" and pa.""IsDeleted""=false
                    join public.""Template"" tmp on pa.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false and tmp.""TemplateType""=7
                    join public.""CustomTemplate"" ctmp on tmp.""Id""=ctmp.""TemplateId"" and ctmp.""IsDeleted""=false  
                    join public.""TemplateCategory"" tmpc on tmpc.""Id"" = Any ( ctmp.""AllowedCategories"") and tmpc.""IsDeleted""=false
                    join public.""Template"" tmpctmp on tmpc.""Id""=tmpctmp.""TemplateCategoryId"" and tmpctmp.""IsDeleted""=false  
                    where  mg.""PortalId""='{portal.Id}' and mg.""IsDeleted""=false 
                    union
                    select tmpc.""Id"" as ""TempId"",pa.""Id"" as ""BoxParentId"",tmpc.""Name"" as ""TempName"" , 'TEMPLATE' as ""BoxType"" 
                    from public.""MenuGroup"" mg join public.""Page"" as pa on mg.""Id""=pa.""MenuGroupId"" and pa.""IsDeleted""=false
                    join public.""Template"" tmp on pa.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false and tmp.""TemplateType""=7
                    join public.""CustomTemplate"" ctmp on tmp.""Id""=ctmp.""TemplateId"" and ctmp.""IsDeleted""=false  
                    join public.""Template"" tmpc on tmpc.""Id"" = Any ( ctmp.""AllowedTemplates"") and tmpc.""IsDeleted""=false  
                    where  mg.""PortalId""='{portal.Id}' and mg.""IsDeleted""=false"
            , null);



            return pages;
        }
        public async Task<List<IdNameViewModel>> GetPortalForUserData(string portalIds)
        {

            var query = $@"SELECT p.""Id"" as Id, p.""Name"" as Name FROM public.""Portal"" as p

 where p.""Id"" in ('{portalIds}')
ORDER BY p.""CreatedDate"" ASC";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetPortalsData(string id)
        {
            var query = @"SELECT p.""Id"" as Id, p.""Name"" as Name FROM public.""Portal"" as p where p.""IsDeleted""=false ORDER BY p.""CreatedDate"" ASC";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<PortalViewModel>> GetPortalListByPortalNames(string portalNames)
        {
            var portals = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                var namelist = portalNames.Split(',').ToList();
                if (namelist.IsNotNull() && namelist.Count > 0)
                {
                    foreach (var n in namelist)
                    {
                        portals += $"'{n}',";
                    }
                    portals = portals.Trim(',');
                }
            }
            var query = @$"SELECT p.* FROM public.""Portal"" as p 
                        where p.""IsDeleted""=false and p.""Name"" in ({portals})
                        ORDER BY p.""Name"" ASC";
            var queryData = await _queryRepo.ExecuteQueryList<PortalViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetAllowedPortalsData(string portalIds)
        {
            var query = $@"SELECT p.""Id"" as Id, p.""Name"" as Name FROM public.""Portal"" as p

 where p.""Id"" in ('{portalIds}')
ORDER BY p.""CreatedDate"" ASC";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<PageViewModel>> GetMenuGroupOfPortalData(string portalIds)
        {
            var query = $@"select mg.""Name"" as Name,sm.""Name"" as SubModuleName,m.""Name"" as ModuleName
,null as ParentId,mg.""Id"" as Id,'MenuGroup' as Type,mg.""Id"" as key,'' as title,true as lazy
from public.""MenuGroup"" as mg
left join public.""SubModule"" as sm on sm.""Id""= mg.""SubModuleId"" and sm.""IsDeleted""=false --and sm.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""Module"" as m on m.""Id""= sm.""ModuleId"" and m.""IsDeleted""=false --and m.""PortalId"" ='{_repo.UserContext.PortalId}'
where mg.""IsDeleted""=false and mg.""PortalId"" in ('{portalIds}') ";

            var menugroup = await _queryRepo.ExecuteQueryList<PageViewModel>(query, null);
            return menugroup;
        }
        public async Task<List<PageViewModel>> GetMenuGroupOfPortalData1(string portalIds, string item)
        {
            var query1 = $@"select p.""Title"" as Name,mg.""Name"" as MenuGroupName,sm.""Name"" as SubModuleName,m.""Name"" as ModuleName,
t.""DisplayName"" as TemplateName,p.""PageType"",'{item}' as ParentId,p.""Id"" as Id,'Page' as Type,mg.""Id"" as key,'' as title,true as lazy
from public.""Page"" as p
join public.""MenuGroup"" as mg on mg.""Id""= p.""MenuGroupId"" and mg.""IsDeleted""=false --and mg.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""SubModule"" as sm on sm.""Id""= mg.""SubModuleId"" and sm.""IsDeleted""=false --and sm.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""Module"" as m on m.""Id""= sm.""ModuleId"" and m.""IsDeleted""=false --and m.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""Template"" as t on t.""Id""= p.""TemplateId"" and t.""IsDeleted""=false --and t.""PortalId"" ='{_repo.UserContext.PortalId}'
where p.""IsDeleted""=false and mg.""Id""='{item}' and p.""PortalId"" in ('{portalIds}') ";
            var page = await _queryRepo.ExecuteQueryList<PageViewModel>(query1, null);
            return page;
        }
        public async Task SetAllTaskNotificationReadData(string userId, string taskId)
        {

            string query = @$"update public.""Notification"" set ""ReadStatus""='1' where ""ToUserId"" ='{userId}' and ""ReferenceTypeId""='{taskId}'";

            await _queryRepo.ExecuteScalar<bool?>(query, null);
        }
        public async Task<IList<NotificationViewModel>> GetNotificationListData(string userId)
        {
            var query = $@" Select n.*, u.""PhotoId"" as PhotoId,CONCAT( u.""Name"",'<',u.""Email"",'>') as From
            from public.""Notification"" as n 
            left join public.""User"" as u on u.""Id""=n.""FromUserId"" or n.""From""=u.""Email"" and u.""IsDeleted""=false
            where n.""ToUserId""='{userId}' and n.""ReadStatus""='0' and n.""IsDeleted""=false
            order by n.""CreatedDate"" desc";
            var data = await _queryRepo.ExecuteQueryList<NotificationViewModel>(query, null);
            return data;
        }
        public async Task SetAllNotificationReadData(string userId)
        {
            string query = @$"update public.""Notification"" set ""ReadStatus""='1' where ""ToUserId"" ='{userId}'";

            await _queryRepo.ExecuteScalar<bool?>(query, null);
        }
        public async Task<IList<IdNameViewModel>> GetComponentListData()
        {
            string query = @$"SELECT ""Id"",""ComponentType"" as Name  
                            FROM public.""Component""
                            where ""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<IList<SubModuleViewModel>> GetSubModuleListData()
        {
            string query = @$"select s.*,m.""Name"" as ""ModuleName"" from public.""SubModule"" as s
                             join public.""Module"" as m on m.""Id""=s.""ModuleId""
                              where s.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<SubModuleViewModel>(query, null);
            return list;
        }
        public async Task<IList<SubModuleViewModel>> GetPortalSubModuleListData()
        {
            string query = @$"select s.*,m.""Name"" as ""ModuleName"" from public.""SubModule"" as s
                             join public.""Module"" as m on m.""Id""=s.""ModuleId""
                              where s.""IsDeleted""=false and s.""PortalId""='{_repo.UserContext.PortalId}' and s.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'";
            var list = await _queryRepo.ExecuteQueryList<SubModuleViewModel>(query, null);
            return list;
        }
        public async Task<DataTable> GetTableData(string tableMetadataId, string recordId, string name, string schema)
        {
            var query = $@"select * from {schema}.""{name}"" where ""Id""='{recordId}' limit 1";
            var dt = await _queryRepo.ExecuteQueryDataTable(query, null);
            return dt;
        }
        public async Task<DataTable> GetTableDataByColumnData(string schema, string name, string udfName, string udfValue)
        {
            var selectQuery = @$"select * from {schema}.""{name}"" where  ""IsDeleted""=false and ""{udfName}""='{udfValue}' limit 1";
            var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
            return dt;
        }
        public async Task<DataTable> GetTableDataByHeaderIdData(string schema, string name, string fieldName, string headerId)
        {
            var selectQuery = @$"select * from {schema}.""{name}"" where  ""IsDeleted""=false and ""{fieldName}""='{headerId}' limit 1";
            var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
            return dt;
        }
        public async Task<DataTable> DeleteTableDataByHeaderIdData(string schema, string name, string fieldName, string headerId)
        {
            var selectQuery = @$"update {schema}.""{name}"" set  ""IsDeleted""=true where ""{fieldName}""='{headerId}' ";
            var dt = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
            return dt;
        }
        public async Task EditTableDataByHeaderIdData(string schema, string name, List<string> columnKeys, string fieldName, string headerId)
        {
            var selectQuery = @$"update {schema}.""{name}"" set {string.Join(",", columnKeys)} where ""{fieldName}""='{headerId}' ";
            await _queryRepo.ExecuteCommand(selectQuery, null);
        }
        public async Task<DataTable> GetViewColumnByTableNameData(string schema, string tableName)
        {

            var query = $@"select * from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' and TABLE_SCHEMA='{schema}'";
            var columns = await _queryRepo.ExecuteQueryDataTable(query, null);
            return columns;
        }
        public async Task<IList<TeamViewModel>> GetTeamByOwnerData(string userId)
        {
            var cypher = $@" select t.""Id"" as Id,t.""Name"" as Name  from 
public.""User"" as u 
join public.""TeamUser"" as tu on tu.""UserId""=u.""Id"" and tu.""IsTeamOwner""=true and tu.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}' and  u.""IsDeleted""=false

               ";
            var result = await _queryRepo.ExecuteQueryList<TeamViewModel>(cypher, null);
            return result;
        }
        public async Task<IList<TeamViewModel>> GetTeamByUserData(string userId)
        {
            var cypher = $@" select t.""Id"" as Id,t.""Name"" as Name  from 
public.""User"" as u 
join public.""TeamUser"" as tu on tu.""UserId""=u.""Id""  and tu.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}'

               ";

            var result = await _queryRepo.ExecuteQueryList<TeamViewModel>(cypher, null);
            return result;
        }
        public async Task<IList<TeamViewModel>> GetTeamMemberListData(string teamId)
        {
            var cypher = $@" select u.""Id"" as Id, u.""Name"" as Name,tu.""IsTeamOwner"" as IsTeamOwner,u.""PhotoId"",u.""JobTitle"",
case when tu.""IsTeamOwner""=true then u.""Id"" else null end as TeamOwnerId
from public.""Team"" as t
join public.""TeamUser"" as tu on tu.""TeamId""=t.""Id"" and t.""Id""='{teamId}' and t.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' where t.""IsDeleted""=false and tu.""IsDeleted""=false and u.""IsDeleted""=false
               ";
            var result = await _queryRepo.ExecuteQueryList<TeamViewModel>(cypher, null);
            return result;
        }
        public async Task<TeamViewModel> GetTeamOwnerData(string teamId)
        {
            var cypher = $@" select u.""Id"" as Id, u.""Name"" as Name
from public.""Team"" as t
join public.""TeamUser"" as tu on tu.""TeamId""=t.""Id"" and t.""Id""='{teamId}' and t.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' where t.""IsDeleted""=false and tu.""IsDeleted""=false and u.""IsDeleted""=false and tu.""IsTeamOwner""=true
               ";
            var result = await _queryRepo.ExecuteQuerySingle<TeamViewModel>(cypher, null);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetTeamByGroupCodeData(string groupCode)
        {

            var query = $@" select t.""Id"" as Id,t.""Name"" as Name  from 
                            public.""Team"" as t 
                            where t.""IsDeleted""=false and t.""GroupCode""='{groupCode}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetTeamUsersByCodeData(string Code)
        {

            var query = $@" select u.""Id"" as Id,u.""Name"" as Name  from 
                            public.""Team"" as t 
join public.""TeamUser"" as tu on tu.""TeamId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
                            where t.""IsDeleted""=false and t.""Code""='{Code}' ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<List<TeamViewModel>> GetTeamWithPortalIdsData()
        {

            var Query = $@"SELECT distinct u.* FROM public.""Team"" as u 
            join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and array[u.""AllowedPortalIds""] <@ array[c.""LicensedPortalIds""] and c.""IsDeleted""=false
            where u.""IsDeleted""=false and u.""AllowedPortalIds"" is not null";
            var list = await _queryRepo.ExecuteQueryList<TeamViewModel>(Query, null);
            return list;

        }
        public async Task<List<TeamViewModel>> GetTeamsBasedOnAllowedPortalsData(string portalId)
        {

            var Query = $@"select * from public.""Team"" as t
                         where '{portalId}' = any(t.""AllowedPortalIds"")";
            var list = await _queryRepo.ExecuteQueryList<TeamViewModel>(Query, null);
            return list;

        }
        public async Task<List<TemplateCategoryViewModel>> GetCategoryListData(string tCode, string tcCode, string mCodes, string categoryIds, string templateIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null)
        {
            var query = $@"select tc.*,t.""ViewType"" as ViewType from public.""TemplateCategory"" as tc
                            left join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false
                            left join public.""Module"" as m on tc.""ModuleId""=m.""Id"" and m.""IsDeleted""=false
                            left join public.""Portal"" as p on t.""PortalId""=p.""Id""
                            where tc.""TemplateCategoryType""={(int)categoryType} and tc.""IsDeleted""=false --and t.""PortalId""='{_repo.UserContext.PortalId}'
                            #templatetype# #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# 
                            #categoryIdSearch# #PORTALWHERE#
                            group by tc.""Id"",t.""ViewType"" order by tc.""SequenceOrder"" ";

            var portalWhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalWhere = $@" and p.""Name"" in ('{portalNames.Replace(",", "','")}') ";
            }
            else
            {
                //portalWhere = $@" and p.""Id"" = '{_repo.UserContext.PortalId}' ";
                portalWhere = $@" and '{_repo.UserContext.PortalId}' = any(tc.""AllowedPortalIds"") ";
            }
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var ttypesearch = "";
            if (templateType == TemplateTypeEnum.Service)
            {
                ttypesearch = $@" and tc.""TemplateType"" in ({(int)TemplateTypeEnum.Service},{(int)TemplateTypeEnum.Custom}) ";
            }
            else
            {
                ttypesearch = $@" and tc.""TemplateType""={(int)templateType} ";
            }
            query = query.Replace("#templatetype#", ttypesearch);

            var templatecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = $@" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);

            var modulecodeSearch = "";
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);

            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);

            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }
            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateCategoryViewModel>(query, null);
            return list;
        }

        public async Task<List<TemplateCategoryViewModel>> GetModuleBasedCategory(string tCode, string tcCode, string mCodes, string categoryIds, string templateIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null)
        {
            var query = $@"select tc.*,t.""ViewType"" as ViewType,m.""Name"" as ModuleName,m.""Code"" as ModuleCode 
                             from public.""TemplateCategory"" as tc
                             join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false
                             join public.""Module"" as m on tc.""ModuleId""=m.""Id"" and m.""IsDeleted""=false
                             join public.""Portal"" as p on t.""PortalId""=p.""Id""
                            where tc.""TemplateCategoryType""={(int)categoryType} and tc.""IsDeleted""=false --and t.""PortalId""='{_repo.UserContext.PortalId}'
                            #templatetype# #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# 
                            #categoryIdSearch# #PORTALWHERE#
                            group by tc.""Id"",t.""ViewType"",m.""Name"",m.""Code""  order by tc.""SequenceOrder"" ";

            var portalWhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalWhere = $@" and p.""Name"" in ('{portalNames.Replace(",", "','")}') ";
            }
            else
            {
                //portalWhere = $@" and p.""Id"" = '{_repo.UserContext.PortalId}' ";
                portalWhere = $@" and '{_repo.UserContext.PortalId}' = any(tc.""AllowedPortalIds"") ";
            }
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var ttypesearch = "";
            if (templateType == TemplateTypeEnum.Service)
            {
                ttypesearch = $@" and tc.""TemplateType"" in ({(int)TemplateTypeEnum.Service},{(int)TemplateTypeEnum.Custom}) ";
            }
            else
            {
                ttypesearch = $@" and tc.""TemplateType""={(int)templateType} ";
            }
            query = query.Replace("#templatetype#", ttypesearch);

            var templatecodeSearch = "";
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = $@" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);

            var modulecodeSearch = "";
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);

            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);

            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }
            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateCategoryViewModel>(query, null);
            return list;
        }
        public async Task<AssignmentViewModel> GetUserDetailsData(string user)
        {
            string query = $@"select a.* from  cms.""N_CoreHR_HRAssignment"" as a
            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false
            join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false
            where u.""Id""='{user}' and a.""IsDeleted""=false";
            var assignment = await _queryRepo.ExecuteQuerySingle<AssignmentViewModel>(query, null);
            return assignment;
        }
        public async Task<IList<UserViewModel>> GetUserListData()
        {
            string query = @$"SELECT * ,CONCAT(""Name"",'<',""Email"",'>') as Name
                            FROM public.""User""
                            where ""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserListForPortalData()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""PortalId""='{_repo.UserContext.PortalId}' and ""LegalEntityId""='{_repo.UserContext.LegalEntityId}' and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetActiveUserListData()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""Status""=1 ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<PagedList<UserViewModel>> GetActiveUserListForSwitchData(string filter, int pageSize, int pageNumber, string indexedItemId = null)
        {


            int offset = (pageNumber - 1) * pageSize;
            string baseQuery = $@" FROM public.""User""
                          where ""IsDeleted""=false and ""Status""=1  #WHERE# 
";
            var where = "";

            if (filter.IsNotNullAndNotEmpty())
            {
                where = $@" and (""Name"" ILIKE '%{filter}%' COLLATE ""tr-TR-x-icu"" or u.""Email"" ILIKE '%{filter}%' COLLATE ""tr-TR-x-icu"") ";
            }
            baseQuery = baseQuery.Replace("#WHERE#", where);

            string query1 = $@" Select Count(""Id"") {baseQuery}";
            string query2 = $@" select *
 {baseQuery} order by ""Name""   OFFSET {offset}  LIMIT {pageSize}";
            if (indexedItemId.IsNotNullAndNotEmpty())
            {
                var indexQuery = $@"With cte as (select u.""Id"", ROW_NUMBER () OVER (ORDER BY ""Name"",""Id"") as rowNumber
{baseQuery} order by ""Name"" )select ""rowNumber"" from cte where ""Id""='{indexedItemId}' ";
                var index = await _queryRepo.ExecuteScalar<long>(indexQuery, null);
                return PagedList<UserViewModel>.Instance(index);
                // ROW_NUMBER() OVER(ORDER BY product_id)
            }
            var total = await _queryRepo.ExecuteScalar<long>(query1, null);
            var list2 = await _queryRepo.ExecuteQueryList<UserViewModel>(query2, null);
            return PagedList<UserViewModel>.Instance(total, list2);
        }
        public async Task<IList<UserViewModel>> GetActiveUserListForSwitchProfileData()
        {
            string query = @$"SELECT u.* 
                            FROM public.""User"" as u 
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                            where u.""IsDeleted""=false and u.""Status""=1 and up.""PortalId""='{_userContext.PortalId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<PagedList<UserViewModel>> GetActiveUsersListForSwitchProfileData(string filter, int pageSize, int pageNumber, string indexedItemId = null)
        {
            int offset = (pageNumber - 1) * pageSize;
            string baseQuery = $@" FROM public.""User"" as u 
            join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                                        where u.""IsDeleted""=false and u.""Status""=1 and up.""PortalId""='{_userContext.PortalId}'  #WHERE# 
";
            var where = "";

            if (filter.IsNotNullAndNotEmpty())
            {
                where = $@" and (u.""Name"" ILIKE '%{filter}%' COLLATE ""tr-TR-x-icu"" or u.""Email"" ILIKE '%{filter}%' COLLATE ""tr-TR-x-icu"")  ";
            }
            baseQuery = baseQuery.Replace("#WHERE#", where);

            string query1 = $@" Select Count(u.""Id"") {baseQuery}";
            string query2 = $@" select u.*
 {baseQuery} order by u.""Name""   OFFSET {offset}  LIMIT {pageSize}";
            if (indexedItemId.IsNotNullAndNotEmpty())
            {
                var indexQuery = $@"With cte as (select u.""Id"", ROW_NUMBER () OVER (ORDER BY u.""Name"",u.""Id"") as rowNumber
{baseQuery} order by u.""Name"" )select ""rowNumber"" from cte where ""Id""='{indexedItemId}' ";
                var index = await _queryRepo.ExecuteScalar<long>(indexQuery, null);
                return PagedList<UserViewModel>.Instance(index);
                // ROW_NUMBER() OVER(ORDER BY product_id)
            }
            var total = await _queryRepo.ExecuteScalar<long>(query1, null);
            var list2 = await _queryRepo.ExecuteQueryList<UserViewModel>(query2, null);
            return PagedList<UserViewModel>.Instance(total, list2);
        }
        public async Task<IList<UserViewModel>> GetActiveUserListForPortalData()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""Status""=1 and ""PortalId""='{_repo.UserContext.PortalId}' and ""LegalEntityId""='{_repo.UserContext.LegalEntityId}'  and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserTeamListData(string id)
        {
            string query = @$"SELECT u.* ,CONCAT(u.""Name"",'<',u.""Email"",'>') as Name
                            FROM public.""TeamUser"" as tu
                            inner join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false
                            inner join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
                            where tu.""IsDeleted""=false and t.""Id""='{id}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserTeamListForPortalData()
        {
            string query = @$"SELECT u.* 
                            FROM public.""TeamUser"" as tu
                            inner join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false and tu.""IsDeleted""=false and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
                            inner join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where u.""IsDeleted""=false and u.""PortalId""='{_repo.UserContext.PortalId}' and u.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserListForEmailSummaryData()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""EnableSummaryEmail""=true 
                            and ""Id"" not in (select ""ToUserId"" from public.""Notification"" where ""Subject"" Like 'Synergy Summary%' COLLATE ""tr-TR-x-icu"" and date(""CreatedDate"") =CURRENT_DATE  
                            ) ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetSwitchToUserListData(string userId)
        {
            string query = @$"SELECT u.*,coalesce(ga.""CreatedBy"",u.""Id"") as CreatedBy
                            FROM public.""GrantAccess"" as ga
                            right join public.""User"" as u on u.""Id""=ga.""UserId"" and u.""IsDeleted""=false
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                            where u.""Id""='{userId}' and (ga.""GrantStatus""=0 or ga.""GrantStatus"" is null) and ga.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}'";
            var grantedList = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return grantedList;
        }
        public async Task<List<UserViewModel>> GetSwitchUserListData(string userId)
        {
            string query = @$"SELECT u.*,coalesce(ga.""CreatedBy"",u.""Id"") as CreatedBy
                            FROM public.""GrantAccess"" as ga
                            right join public.""User"" as u on u.""Id""=ga.""UserId"" and u.""IsDeleted""=false
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                            where u.""Id""='{userId}' and (ga.""GrantStatus""=0 or ga.""GrantStatus"" is null) and ga.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}'";
            var grantedList = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return grantedList;
        }
        public async Task<IdNameViewModel> GetPersonWithSponsorData(string userId)
        {
            var query = $@"Select p.Id as Id, sp.Code as Code
from public.""User"" as u 
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and p.""IsDeleted""=false
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false
left join cms.""N_CoreHR_HRSponsor"" as sp on c.""SponsorId""=s.""Id"" and sp.""IsDeleted""=false
where c.EffectiveStartDate::TIMESTAMP::DATE <= Now()::TIMESTAMP::DATE <= c.EffectiveEndDate::TIMESTAMP::DATE
and u.""Id""='{userId}'

";
            return await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
        }
        public async Task<IList<UserListOfValue>> ViewModelListData(string userId)
        {

            var cypher = $@"Select u.""Id"" as Id,u.""Name"" as UserName,u.""Id"" as UserId,p.""Id"" as PersonId
                ,p.""PersonFullName"" as Name
                ,p.""SponsorshipNo"" as SponsorshipNo,u.""Email"" as Email,p.""PersonNo"" as PersonNo
                ,po.""PositionName"" as PositionName,o.""DepartmentName"" as OrganizationName,j.""JobTitle"" as JobName,g.""GradeName"" as GradeName
                ,u.""PhotoId"" as PhotoId,po.""Id"" as PositionId
from   public.""User"" as u 
left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId"" and j.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false
and  o.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date 
and '{DateTime.Now.ApplicationNow().Date}'::Date <= o.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false
and  g.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date 
and '{DateTime.Now.ApplicationNow().Date}'::Date <= g.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRPosition"" as po on po.""Id""=a.""PositionId"" and po.""IsDeleted""=false
and  po.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date 
and '{DateTime.Now.ApplicationNow().Date}'::Date <= po.""EffectiveEndDate""::Date
where u.""Id""='{userId}' and u.""IsDeleted""=false
";
            return await _queryRepo.ExecuteQueryList<UserListOfValue>(cypher, null);

        }
        public async Task<string> GetHierarchyRootIdData(string userHierarchyId, string hierarchy)
        {
            var query = $@"select ""ParentPositionId""
                        from cms.""UserHierarchy""
                        where ""UserId"" = '{userHierarchyId}' and ""HierarchyMasterId"" = '{hierarchy}' and ""IsDeleted""=false ";
            var parentPositionRoot = await _queryRepo.ExecuteScalar<string>(query, null);
            return parentPositionRoot;
        }
        public async Task<string> GetHierarchyRootIdData1(string userHierarchyId, string hierarchy)
        {
            var query = $@"select ""ParentUserId""
                        from public.""UserHierarchy""
                        where ""PositionId"" = '{userHierarchyId}' and ""HierarchyId"" = '{hierarchy}' and ""IsDeleted""=false ";
            var parentPositionRoot = await _queryRepo.ExecuteScalar<string>(query, null);
            return parentPositionRoot;
        }
        public async Task<string> GetHierarchyRootIdData2(string userHierarchyId, string hierarchy)
        {
            var query = $@"select ""ParentDepartmentId""
                        from cms.""N_CoreHR_HRDepartmentHierarchy""
                        where ""DepartmentId"" = '{userHierarchyId}' and ""HierarchyId"" = '{hierarchy}' and ""IsDeleted""=false ";
            var parentOrganizationRoot = await _queryRepo.ExecuteScalar<string>(query, null);
            return parentOrganizationRoot;
        }
        public async Task<List<double>> GetUserNodeLevelData(string userId)
        {
            string query = $@"   WITH RECURSIVE Users AS(
                                 select d.""UserId"" as ""Id"",d.""UserId"" as ""ParentId"",'Parent' as Type
                                from public.""UserHierarchy"" as d
                                where d.""UserId"" = '{userId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""UserId"" as Id,d.""ParentUserId"" as ""ParentId"",'Child' as Type
                                from public.""UserHierarchy"" as d                                
                                where  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select Count(""Id""),""ParentId"" from Users  where Type = 'Child' group by ""ParentId""
						
                            ";



            var queryData = await _queryRepo.ExecuteScalarList<double>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<PositionChartViewModel>> GetUserHierarchyData(string parentId, int levelUpto, string hierarchyId, int level = 1, int option = 1)
        {

            string query = $@"with recursive hrchy as(
                                select u.""Id"" as ""Id"",'' COLLATE ""tr-TR-x-icu"" as ""ParentId"",'Parent' as Type,0 As level
                                from public.""User"" as u where u.""Id"" = '{parentId}' and  u.""IsDeleted""=false 
                                union all
                                select  uh.""UserId"" as ""Id"",uh.""ParentUserId"" as ""ParentId"",'Child' as Type,hrchy.level+ 1 as level
                                from public.""HierarchyMaster"" as h
								join public.""UserHierarchy"" as uh on ""uh"".""HierarchyMasterId""=h.""Id"" and  uh.""IsDeleted""=false 
                                join hrchy  on hrchy.""Id""=uh.""ParentUserId"" 
                                where uh.""LevelNo""={level} and uh.""OptionNo""={option} and  h.""Id""='{hierarchyId}'  and  h.""IsDeleted""=false 
                             )select u.""Id"" as Id,j.""Id"" as JobId,j.""JobTitle"" as JobName,
                            o.""Id"" as OrganizationId, o.""DepartmentName"" as OrganizationName,g.""GradeName"" as GradeName,
                            hrchy.""ParentId"" as ParentId,
                            coalesce(dc.""DC"",0) as DirectChildCount,
                            u.""PhotoId"" as PhotoId,
                            u.""Name"" as DisplayName,
                            u.""Id"" as UserId,
                            '{hierarchyId}' as HierarchyId,
                            Case when p.""Id"" is not null then 'org-node-1' else 'org-node-3' end as CssClass,
                            pd.""PerformanceStageName"",pd.""GoalCount"",pd.""CompetencyCount""
                            from hrchy 
							join public.""User"" as u on hrchy.""Id""=u.""Id""
                            left join 
                            (
                                select uh.""ParentUserId"" as ""UserId"",count(uh.""UserId"") as ""DC""
                                from public.""HierarchyMaster"" as h
								join public.""UserHierarchy"" as uh on ""uh"".""HierarchyMasterId""=h.""Id"" and  uh.""IsDeleted""=false 
                                where uh.""LevelNo""={level} and uh.""OptionNo""={option} and  h.""Id""='{hierarchyId}'  
                                and  h.""IsDeleted""=false 
                                group by uh.""ParentUserId""
                            )as dc on u.""Id""=dc.""UserId""
                            left join 
                            (
                                select  pdn.""OwnerUserId"" as ""UserId"",max(pdms.""Name"") as ""PerformanceStageName""
                                ,max(g.""GC"") as ""GoalCount"",max(c.""CC"") as ""CompetencyCount""
                                from cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd 
                                join public.""NtsService"" as pdn on pdn.""UdfNoteTableId""=pd.""Id""
                                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id"" = pd.""DocumentMasterId"" and pdm.""IsDeleted"" = false
                                join public.""NtsNote"" as pdmn on pdm.""NtsNoteId""=pdmn.""Id""
                                join public.""NtsNote"" as pdmsn on pdmsn.""ParentNoteId""=pdmn.""Id""
                                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" pdms on pdms.""NtsNoteId""=pdmsn.""Id""
                                join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" pds on pds.""DocumentMasterStageId""=pdms.""Id""
                                left join (
                                    select gs.""OwnerUserId"", gs.""ParentServiceId"", count(g.""Id"") as ""GC"" 
	                                from public.""NtsService"" gs
                                    join cms.""N_PerformanceDocument_PMSGoal"" g on gs.""UdfNoteTableId""=g.""Id"" and g.""IsDeleted""=false
	                                where gs.""IsDeleted""=false-- and gs.""TemplateCode""='df'

                                    group by gs.""OwnerUserId"", gs.""ParentServiceId""
                                ) as g on g.""OwnerUserId""=pdn.""OwnerUserId"" and g.""ParentServiceId""=pdn.""Id""
                                left join (
                                    select gs.""OwnerUserId"", gs.""ParentServiceId"", count(g.""Id"") as ""CC"" 
	                                from public.""NtsService"" gs
                                    join cms.""N_PerformanceDocument_PMSCompentency"" g on gs.""UdfNoteTableId""=g.""Id"" and g.""IsDeleted""=false
	                                where gs.""IsDeleted""=false-- and gs.""TemplateCode""='df'

                                    group by gs.""OwnerUserId"", gs.""ParentServiceId""
                                )as c on c.""OwnerUserId""=pdn.""OwnerUserId"" and c.""ParentServiceId""=pdn.""Id""
                                where pdm.""Year""='{DateTime.Today.Year}' and pdms.""StartDate""<='{DateTime.Today.ToDatabaseDateFormat()}' and pdms.""EndDate"">='{DateTime.Today.ToDatabaseDateFormat()}'
                                group by pdn.""OwnerUserId""
                            )as pd on u.""Id""=pd.""UserId""
                            Left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId"" and j.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false 
                            where hrchy.""level""<={levelUpto}";
            var queryData = await _queryRepo.ExecuteQueryList<PositionChartViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<string> GetPersonDateOfJoiningData(string userId)
        {
            var query = $@"Select a.""DateOfJoin"" From public.""User"" as u
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false
                            Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false
                            where u.""Id""='{userId}' and u.""IsDeleted""=false";

            var joinDate = await _queryRepo.ExecuteScalar<string>(query, null);
            return joinDate;

        }
        public async Task<List<LegalEntityViewModel>> GetEntityByIdsData(string legalEntity)
        {
            string query = @$"select *
                        from public.""LegalEntity""
                        where ""Id"" in ({legalEntity}) and ""IsDeleted""=false order by ""Name""";
            var list = await _queryRepo.ExecuteQueryList<LegalEntityViewModel>(query, null);
            return list;
        }
        public async Task<List<UserPermissionViewModel>> ViewUserPermissionsData(string userId)
        {
            string query = @$"select u.""Name"" as UserName,po.""Name"" as PortalName,p.""Name"" as PageName,up.""Permissions"",'User' as Type from public.""UserPermission"" as up
join public.""Page"" as p on p.""Id""=up.""PageId"" and p.""IsDeleted""=false and up.""CompanyId""='{_repo.UserContext.CompanyId}' and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Portal"" as po on po.""Id""=p.""PortalId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=up.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}'

union
select u.""Name"" as UserName,po.""Name"" as PortalName,p.""Name"" as PageName,up.""Permissions"",Concat('UserRole-',ur.""Name"") from public.""UserRolePermission"" as up
join public.""Page"" as p on p.""Id""=up.""PageId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}' and up.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Portal"" as po on po.""Id""=p.""PortalId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""UserRole"" as ur on ur.""Id""=up.""UserRoleId"" and ur.""IsDeleted""=false and ur.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""UserRoleUser"" as uru on uru.""UserRoleId""=up.""UserRoleId"" and uru.""IsDeleted""=false and uru.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=uru.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}'";
            var list = await _queryRepo.ExecuteQueryList<UserPermissionViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserListWithEmailTextData()
        {
            string query = @$"SELECT *, concat('""',""Name"",'"" ','<',""Email"",'>') as EmailText
                            FROM public.""User""
                            where ""IsDeleted""=false and ""Status""=1";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<List<UserViewModel>> GetAllUsersWithPortalIdData(string PortalId)
        {
            var Query = $@"SELECT u.* FROM public.""User"" as u 
 join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
  where u.""IsDeleted""=false  and up.""PortalId""='{PortalId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(Query, null);
            return list;

        }
        public async Task<List<UserViewModel>> GetUsersWithPortalIdsData()
        {

            var Query = $@"SELECT distinct u.* FROM public.""User"" as u 
 join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and c.""IsDeleted""=false and up.""PortalId""= '{_userContext.PortalId}'--Any(c.""LicensedPortalIds"") 
  where u.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(Query, null);
            return list;

        }
        public async Task<List<PortalViewModel>> GetAllowedPortalListData(string userId)
        {
            var Query = $@"select p.* from public.""User"" as u 
            join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
            join public.""Portal"" as p on up.""PortalId""=p.""Id"" and p.""IsDeleted""=false
            where u.""IsDeleted""=false  and u.""Id""='{userId}' order by p.""Name""";
            var list = await _queryRepo.ExecuteQueryList<PortalViewModel>(Query, null);
            return list;
        }
        public async Task<List<UserViewModel>> GetDMSPermiisionusersListData(string PortalId, string LegalEntityId)
        {
            var Query = $@"SELECT u.*  FROM public.""User"" as u 
 join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
  where u.""IsDeleted""=false  and up.""PortalId""='{PortalId}'
    and '{LegalEntityId}' = any ( u.""LegalEntityIds"")";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(Query, null);
            return list;

        }
        public async Task<string> GetUserHierarchyRootIdData(string userHierarchyId, string hierarchy)
        {
            var query = $@"select ""ParentUserId""
                        from cms.""N_GENERAL_UserHierarchy""
                        where ""UserId"" = '{userHierarchyId}' and ""HierarchyId"" = '{hierarchy}' and ""IsDeleted""=false ";
            var parentOrganizationRoot = await _queryRepo.ExecuteScalar<string>(query, null);
            return parentOrganizationRoot;
        }
        public async Task<IList<IdNameViewModel>> GetColunmMetadataListByPageData(string pageId)
        {
            string query = @$"select cm.""Id"" as Id, cm.""Name"" as Name from public.""Page"" as pg
                            left join public.""Template"" as pt on pt.""Id"" = pg.""TemplateId"" and pt.""IsDeleted""=false
                            left join public.""TableMetadata"" as tm on tm.""Id"" = pt.""TableMetadataId"" and tm.""IsDeleted""=false
                            left join public.""ColumnMetadata"" as cm on cm.""TableMetadataId"" = tm.""Id"" and cm.""IsDeleted""=false
                            where pg.""Id"" ='{pageId}' and pg.""IsDeleted""=false";

            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserDataPermissionViewModel>> GetUserDataPermissionByPageIdData(string pageId, string portalId)
        {
            string query = @$"select udp.*, u.""Name"" as UserName, pa.""Name"" as PageName, cm.""Name"" as ColumnMetadataName,
                                cm2.""Name"" as ColumnMetadataName2
                                from public.""UserDataPermission"" as udp
                                left join public.""User"" as u on u.""Id"" = udp.""UserId"" and u.""IsDeleted""=false
                                left join public.""Page"" as pa on pa.""Id"" = udp.""PageId"" and pa.""IsDeleted""=false
                                left join public.""ColumnMetadata"" as cm on cm.""Id""  = udp.""ColumnMetadataId"" and cm.""IsDeleted""=false
                                left join public.""ColumnMetadata"" as cm2 on cm2.""Id""  = udp.""ColumnMetadataId2"" and cm2.""IsDeleted""=false
                                where udp.""PageId"" = '{pageId}' and udp.""IsDeleted"" = false #WHERE#";
            var search = "";
            if (portalId.IsNotNullAndNotEmpty())
            {
                search = $@" and udp.""PortalId""='{portalId}'";
            }
            query = query.Replace("#WHERE#", search);
            var list = await _queryRepo.ExecuteQueryList<UserDataPermissionViewModel>(query, null);
            return list;
        }

        #region Workboard
        public async Task<List<WorkBoardViewModel>> GetWorkboardList(WorkBoardstatusEnum status, string id = null)
        {
            var query1 = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , wb.""IconFileId"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'
                            and wb.""Id"" != '{id}' ";
            var querydata1 = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query1, null);
            return querydata1;
        }
        public async Task<List<WorkBoardViewModel>> GetWorkboardListData(WorkBoardstatusEnum status, string id = null)
        {
            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
                            join public.""NtsNote"" as n on n.""Id""=wb.""NtsNoteId"" and n.""IsDeleted""=false and n.""OwnerUserId""='{_userContext.UserId}'
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<List<WorkBoardViewModel>> GetWorkboardTaskList()
        {

            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
                            where wb.""IsDeleted"" = false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            return querydata;


        }

        public async Task<bool> UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status)
        {
            var query = @$"UPDATE cms.""N_WORKBOARD_WorkBoard""
                                SET ""WorkBoardStatus"" = '{(int)status}'
                                WHERE ""Id"" = '{id}';";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<WorkBoardSectionViewModel> GetWorkBoardSectionDetails(string sectionId)
        {
            var query = @$"select wbs.*,wbs.""Id"" as ""WorkBoardSectionId"",wbs.""NtsNoteId"" as NoteId
                            from cms.""N_WORKBOARD_WorkBoardSection"" as wbs
                            where wbs.""Id""='{sectionId}' and wbs.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardSectionViewModel>(query, null);
            return querydata;
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionListByWorkbBoardId(string workboardId)
        {
            var query = @$"select wbs.*,wbs.""NtsNoteId"" as ""NoteId""
                            from cms.""N_WORKBOARD_WorkBoardSection"" as wbs
                            where wbs.""WorkBoardId""='{workboardId}' and wbs.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return querydata;
        }

        public async Task<WorkBoardItemViewModel> GetWorkBoardItemDetails(string itemId)
        {
            var query = @$"select *,""NtsNoteId"" as NoteId
                            from cms.""N_WORKBOARD_WorkBoardItem"" 
                            where ""Id""='{itemId}' and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return querydata;
        }

        public async Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemBySectionId(string sectionId)
        {
            var query = @$"select *
                            from cms.""N_WORKBOARD_WorkBoardItem"" 
                            where ""WorkBoardSectionId""='{sectionId}' and (""ParentId"" is null or ""ParentId""='')  and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query, null);
            return querydata;
        }

        public async Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemByParentId(string parentId)
        {
            var query = @$"select *,""NtsNoteId"" as NoteId
                            from cms.""N_WORKBOARD_WorkBoardItem"" 
                            where ""ParentId""='{parentId}'  and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query, null);
            return querydata;
        }
        public async Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId)
        {
            var query = @$"select *,""NtsNoteId"" as NoteId
                            from cms.""N_WORKBOARD_WorkBoardItem"" 
                            where ""WorkBoardSectionId""='{sectionId}' and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query, null);
            return querydata;
        }
        public async Task DeleteItem(string itemId)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""IsDeleted""=true
                            where ""Id""='{itemId}' ";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task DeleteSection(string itemId)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""IsDeleted""=true
                            where ""Id""='{itemId}' ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardJson(WorkBoardViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoard"" set ""JsonContent""='{data.JsonContent /*JsonConvert.SerializeObject(data.JsonContent)*/}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.WorkboardId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""SequenceOrder""='{data.SequenceOrder}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""SequenceOrder""='{data.SequenceOrder}',""ParentId""='{data.ParentId}',""ColorCode""='{data.ColorCode}',""WorkBoardSectionId""='{data.WorkBoardSectionId}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""SequenceOrder""='{data.SequenceOrder}',""ParentId""='{data.ParentId}',""ColorCode""='{data.ColorCode}',""WorkBoardItemShape""='{data.WorkBoardItemShape}',""WorkBoardItemSize""='{data.WorkBoardItemSize}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data)
        {
            //var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""WorkBoardSectionId""='{data.WorkBoardSectionId}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
            //                where ""NtsNoteId""='{data.WorkBoardItemId}' and ""IsDeleted""=false ";
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""ParentId""='{data.ParentId}',""ColorCode""='{data.ColorCode}',""WorkBoardSectionId""='{data.WorkBoardSectionId}',""WorkBoardItemShape""='{data.WorkBoardItemShape}',""WorkBoardItemSize""='{data.WorkBoardItemSize}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.WorkBoardItemId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task<List<WorkBoardInviteDetailsViewModel>> GetWorkBoardInvites(string workBoradId)
        {
            var query = @$"select * 
                            from cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_WorkBoardShare"" as i on i.""WorkBoardId""=wb.""Id"" and i.""IsDeleted""=false                           
                            where wb.""Id""='{workBoradId}' and wb.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardInviteDetailsViewModel>(query, null);
            return querydata;
        }

        public async Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId)
        {
            var query = @$"select wb.*,wb.""NtsNoteId"" as NoteId,wbtt.""TemplateTypeName"" as TemplateTypeName, wbtt.""TemplateTypeCode"" as TemplateTypeCode,
                            n.""RequestedByUserId"",n.""OwnerUserId"",wb.""WorkBoardTeamIds"" as WorkBoardTeams
                            from cms.""N_WORKBOARD_WorkBoard"" as wb
                            join public.""NtsNote"" as n on wb.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
                            left join cms.""N_WORKBOARD_TemplateType"" as wbtt on wbtt.""Id""=wb.""TemplateTypeId"" and wbtt.""IsDeleted""=false
                            where wb.""Id""='{workBoradId}' and wb.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetail(string workBoradId)
        {
            var query = @$"select wb.""Id"",wb.""WorkBoardName"",wb.""WorkboardType"" as ""WorkBoardType"",wb.""NtsNoteId"" as NoteId,wbtt.""TemplateTypeName"" as TemplateTypeName, wbtt.""TemplateTypeCode"" as TemplateTypeCode,
                            n.""RequestedByUserId"",n.""OwnerUserId"",wb.""WorkBoardTeamIds"" as WorkBoardTeams
                            from cms.""N_WORKBOARD_WorkBoard"" as wb
                            join public.""NtsNote"" as n on wb.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
                            left join cms.""N_WORKBOARD_TemplateType"" as wbtt on wbtt.""Id""=wb.""TemplateTypeId"" and wbtt.""IsDeleted""=false
                            where wb.""Id""='{workBoradId}' and wb.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetailsByIdKey(string workBoardUniqueId, string shareKey)
        {
            var query = @$"select wb.""Id"" as WorkboardId 
                            from cms.""N_WORKBOARD_WorkBoard"" as wb
                            where wb.""WorkboardUniqueId""='{workBoardUniqueId}' and wb.""ShareKey""='{shareKey}' and wb.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardViewModel>(query, null);
            return querydata;
        }

        public async Task<List<WorkBoardTemplateViewModel>> GetTemplateList()
        {
            var query = $@"SELECT wt.""Id"" as TemplateId, wt.""NtsNoteId"" as NoteId, wt.""TemplateDescription"" as TemplateDescription,
                            wt.""SampleContent"" as SamplecContent, wt.""ContentImage"" as ContentImage, wt.""TemplateTypeCode"" as TemplateTypeCode,
                            wt.""TemplateTypeName"" as TemplateTypeName, lov.""Name"" as TemplateDisplayName
                            FROM cms.""N_WORKBOARD_TemplateType"" as wt
                            join public.""LOV"" as lov on lov.""Id"" = wt.""TemplateCategoryId"" where lov.""IsDeleted""=false and lov.""Name"" NOT IN ('Frameworks','Categories','Use Cases', 'Teams')
                            and wt.""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQueryList<WorkBoardTemplateViewModel>(query, null);
            return result;
        }

        public async Task<List<LOVViewModel>> GetTemplateCategoryList()
        {
            var query = $@"select lov.""Id"" as TemplateCategoryId, lov.""Name"" as Name from public.""LOV"" as lov where ""LOVType"" = 'WB_TMPLT_CATEGORY' and lov.""Name"" NOT IN ('Frameworks','Categories','Use Cases', 'Teams') and ""IsDeleted""=false;";
            var result = await _queryRepo.ExecuteQueryList<LOVViewModel>(query, null);
            return result;
        }
        public async Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string[] values)
        {
            string s1 = string.Join("|", values);

            var query = $@"SELECT wt.""Id"" as TemplateId, wt.""NtsNoteId"" as NoteId, wt.""TemplateDescription"" as TemplateDescription,
                            wt.""SampleContent"" as SamplecContent, wt.""ContentImage"" as ContentImage, wt.""TemplateTypeCode"" as TemplateTypeCode,
                            wt.""TemplateTypeName"" as TemplateTypeName, lov.""Name"" as TemplateDisplayName
                            FROM cms.""N_WORKBOARD_TemplateType"" as wt
                            join public.""LOV"" as lov on lov.""Id"" = wt.""TemplateCategoryId"" 
                            where lov.""IsDeleted""=false 
                                  and lov.""Name"" NOT IN ('Frameworks','Categories','Use Cases', 'Teams')
                                  and wt.""IsDeleted""=false
                                  and ""TemplateDescription"" ~* '({s1})' COLLATE ""tr-TR-x-icu"" ";

            var result = await _queryRepo.ExecuteQueryList<WorkBoardTemplateViewModel>(query, null);
            return result;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetJsonContent(string workBoardId, List<WorkBoardSectionViewModel> list)
        {
            var query = $@"select n.""OwnerUserId"",wbs.* from cms.""N_WORKBOARD_WorkBoardSection"" as wbs
                        join public.""NtsNote"" as n on wbs.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
                        where wbs.""WorkBoardId""='{workBoardId}' and wbs.""IsDeleted""=false order by wbs.""SequenceOrder""";
            list = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return list;
        }
        public async Task<List<WorkBoardItemViewModel>> GetJsonContentData(List<WorkBoardSectionViewModel> list)
        {
            List<WorkBoardItemViewModel> itemlist = new List<WorkBoardItemViewModel>();

            foreach (var section in list)
            {
                var query1 = $@"select n.""OwnerUserId"",wbi.* from cms.""N_WORKBOARD_WorkBoardItem"" as wbi
                                join public.""NtsNote"" as n on wbi.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
                                where wbi.""WorkBoardSectionId""='{section.Id}' and wbi.""IsDeleted""=false order by wbi.""SequenceOrder""";
                section.item = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query1, null);
                itemlist.AddRange(section.item);

            }
            return itemlist;
        }
        public async Task<List<WorkBoardItemViewModel>> GetWorkBoardSectionForIndex(WorkBoardItemViewModel item)
        {
            var query = $@"select * from cms.""N_WORKBOARD_WorkBoardItem"" where ""WorkBoardSectionId"" = '{item.WorkBoardSectionId}' and ""IsDeleted"" = false ";
            var result = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query, null);
            return result;
        }
        public async Task<WorkBoardTemplateViewModel> GetWorkBoardTemplateById(string templateTypeId)
        {
            var query = $@"Select * from cms.""N_WORKBOARD_TemplateType"" where ""Id""='{templateTypeId}' and ""IsDeleted""=false";
            return await _queryRepo.ExecuteQuerySingle<WorkBoardTemplateViewModel>(query, null);
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasic()
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 3 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasicData()
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 3 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasicDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections)
        {
            var i = 1;
            string uquery = string.Empty;
            foreach (var id in sections.Select(x => x.Id))
            {
                // Change Section Name as Section 1 and Digit as 1(lly for others)
                uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='Section {i}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                i++;
            }
            await _queryRepo.ExecuteCommand(uquery, null);
            // BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
            var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
            await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
            var query2 = $@"select * from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
            return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthly(int days)
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST {days} ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthlyData(int days)
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST {days} ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthlyDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections, DateTime date)
        {
            var i = 1;
            string uquery = string.Empty;
            foreach (var id in sections.Select(x => x.Id))
            {
                var SectionName = new DateTime(date.Year, date.Month, i);
                // Change Section Name as 1 March 2022 and Digit as 1(lly for others)
                uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='{SectionName.ToString("dd MMMM yyyy")}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                i++;
            }
            await _queryRepo.ExecuteCommand(uquery, null);
            //BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
            var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
            await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
            var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
            return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeekly()
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 7 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeeklyData()
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 7 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeeklyDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections)
        {
            var i = 0;
            string uquery = string.Empty;
            foreach (var id in sections.Select(x => x.Id))
            {
                // Change Section Name as Section 1 and Digit as 1(lly for others)
                uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='{Enum.GetName(typeof(DayOfWeek), i)}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                i++;
            }
            await _queryRepo.ExecuteCommand(uquery, null);
            // job to generateDummyWorkBoardSections(No Of Items)
            //BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
            var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
            await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
            var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
            return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearly()
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 12 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearlyData()
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 12 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearlyDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections)
        {
            var i = 1;
            string uquery = string.Empty;
            foreach (var id in sections.Select(x => x.Id))
            {
                // Change Section Name as Section 1 and Digit as 1(lly for others)
                uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='{Enum.GetName(typeof(MonthEnum), i)}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                i++;
            }
            await _queryRepo.ExecuteCommand(uquery, null);
            var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
            await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
            var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
            return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id)
        {
            var query = $@"Select sec.""Id"" as ""Id"", sec.""SectionName"" as ""SectionName"" ,""NtsNoteId"" as NoteId
                            from cms.""N_WORKBOARD_WorkBoardSection"" as sec
                            where sec.""IsDeleted"" = false and sec.""WorkBoardId"" = '{id}' ";
            var result = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkBoardItemByNtsNoteId(string ntsNoteId)
        {
            var query = $@"Select wbi.*
                        ,case when wbi.""ItemType""='2' then wbi.""ItemContent"" end as ItemContentIndex
                        ,case when wbi.""ItemType""='6' then wbi.""ItemFileId"" end as ItemFileFileId
from cms.""N_WORKBOARD_WorkBoardItem"" as wbi
                            where wbi.""IsDeleted"" = false and wbi.""NtsNoteId"" = '{ntsNoteId}' ";
            var result = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkboardItemById(string id)
        {
            var query = $@"Select *,""Id"" ""WorkBoardItemId"" from cms.""N_WORKBOARD_WorkBoardItem""
                            where ""IsDeleted"" = false and ""Id"" = '{id}' ";
            var result = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id)
        {
            var query = $@"Select *,""Id"" ""WorkBoardItemId"" from cms.""N_WORKBOARD_WorkBoardItem""
                            where ""IsDeleted"" = false and ""NtsNoteId"" = '{id}' ";
            var result = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return result;
        }
        public async Task<bool> UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId)
        {
            var query = @$"UPDATE cms.""N_WORKBOARD_WorkBoardItem""
                                SET ""WorkBoardId"" = '{workboardId}', ""WorkBoardSectionId"" = '{sectionId}'
                                WHERE ""Id"" = '{workboardItemId}';";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }

        public async Task<IList<UserViewModel>> GetUserList(string noteId)
        {
            string query = @$"SELECT * ,CONCAT(""Name"",'<',""Email"",'>') as Name
                            FROM public.""User"" as t
                            join public.""UserPortal"" as up on up.""UserId""=t.""Id"" and up.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}'
                            where t.""Id"" not in (select ""SharedWithUserId"" from public.""NtsNoteShared"" where ""NtsNoteId""= '{noteId}' and ""IsDeleted""=false)
                                and t.""IsDeleted"" = false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }

        public async Task<List<WorkBoardViewModel>> GetSharedWorkboardList(WorkBoardstatusEnum status, string sharedWithUserId)
        {
            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM public.""NtsNoteShared"" as t
                            join cms.""N_WORKBOARD_WorkBoard"" as wb on t.""NtsNoteId"" = wb.""NtsNoteId""
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId"" and tt.""IsDeleted"" = false                            
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}' 
                            and t.""SharedWithUserId"" = '{sharedWithUserId}' and t.""IsDeleted"" = false ";

            //Union
            //    SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
            //    wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
            //    wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
            //    FROM cms.""N_WORKBOARD_WorkBoard"" as wb
            //    join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId"" and tt.""IsDeleted"" = false
            //    join cms.""N_WORKBOARD_WorkBoardShare"" as wbs on wbs.""WorkBoardId""=wb.""Id"" and wbs.""IsDeleted""=false
            //    join public.""User"" as u on u.""Email""=wbs.""EmailAddress"" and u.""IsDeleted""=false and u.""Id""='{sharedWithUserId}'
            //    where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'

            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            return querydata;
        }

        #endregion
        #region UserRoleUserBusiness
        public async Task<List<UserRoleUserViewModel>> GetUserRoleByUser(string userId)
        {
            var query = @$"SELECT ur.""Id"", ur.""UserRoleId"" as UserRoleId FROM public.""UserRoleUser"" as ur where ur.""UserId"" = '{userId}' and ur.""IsDeleted"" = false and ur.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserRoleUserViewModel>(query, null);
            return queryData;
        }
        #endregion

        #region UserRoleStageParentBusiness

        public async Task<List<UserRoleStageParentViewModel>> GetUserRoleStageParentList()
        {
            string query = @$"select ursp.*,ur.""Name"" as UserRoleName
                FROM public.""UserRoleStageParent"" as ursp
                left join public.""UserRole"" as ur on ur.""Id""=ursp.""UserRoleId"" and ur.""IsDeleted""=false
                            where ursp.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<UserRoleStageParentViewModel>(query, null);
            return list;
        }
        #endregion

        #region UserRolePortalBusiness

        public async Task<List<UserRolePortalViewModel>> GetPortalByUserRole(string userRoleId)
        {
            var query = @$"SELECT p.""Id"", p.""PortalId"" as PortalId FROM public.""UserRolePortal"" as p where p.""UserRoleId"" = '{userRoleId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserRolePortalViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserRoleViewModel>> GetUserRoleByPortal(string portalId)
        {
            var query = @$"SELECT u.*, p.""PortalId"" as PortalId
FROM public.""UserRolePortal"" as p
join public.""UserRole"" as u  on u.""Id""=p.""UserRoleId"" and  u.""IsDeleted"" = false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  where p.""PortalId"" = '{portalId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            var queryData = await _queryRepo.ExecuteQueryList<UserRoleViewModel>(query, null);
            return queryData;
        }

        #endregion
        #region UserRoleDataPermissionBusiness

        public async Task<IList<IdNameViewModel>> GetColunmMetadataListByPageForUserRole(string pageId)
        {
            string query = @$"select cm.""Id"" as Id, cm.""Name"" as Name from public.""Page"" as pg
                            left join public.""Template"" as pt on pt.""Id"" = pg.""TemplateId"" and pt.""IsDeleted""=false
                            left join public.""TableMetadata"" as tm on tm.""Id"" = pt.""TableMetadataId"" and tm.""IsDeleted""=false
                            left join public.""ColumnMetadata"" as cm on cm.""TableMetadataId"" = tm.""Id"" and cm.""IsDeleted""=false
                            where pg.""Id"" ='{pageId}' and pg.""IsDeleted""=false";

            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }

        public async Task<IList<UserRoleDataPermissionViewModel>> GetUserRoleDataPermissionByPageId(string pageId, string portalId)
        {
            string query = @$"select udp.*, u.""Name"" as UserRoleName, pa.""Name"" as PageName, cm.""Name"" as ColumnMetadataName,
                                cm2.""Name"" as ColumnMetadataName2
                                from public.""UserRoleDataPermission"" as udp
                                left join public.""UserRole"" as u on u.""Id"" = udp.""UserRoleId"" and u.""IsDeleted""=false
                                left join public.""Page"" as pa on pa.""Id"" = udp.""PageId"" and pa.""IsDeleted""=false
                                left join public.""ColumnMetadata"" as cm on cm.""Id""  = udp.""ColumnMetadataId"" and cm.""IsDeleted""=false
                                left join public.""ColumnMetadata"" as cm2 on cm2.""Id""  = udp.""ColumnMetadataId2"" and cm2.""IsDeleted""=false
                                where udp.""PageId"" ='{pageId}' and udp.""IsDeleted"" = false";

            var list = await _queryRepo.ExecuteQueryList<UserRoleDataPermissionViewModel>(query, null);
            return list;
        }
        #endregion


        #region UserRoleBusiness
        public async Task<List<UserViewModel>> GetUserRoleForUser(string id)
        {
            var query = @"SELECT ur.""Id"" as Id, ur.""Name"" as Name FROM public.""UserRole"" as ur";
            var queryData = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserRoleViewModel>> GetUserRolesWithPortalIds()
        {

            var Query = $@"SELECT distinct u.* FROM public.""UserRole"" as u 
 join public.""UserRolePortal"" as up on up.""UserRoleId""=u.""Id"" and up.""IsDeleted""=false
join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and c.""IsDeleted""=false and up.""PortalId""= '{_userContext.PortalId}' --Any(c.""LicensedPortalIds"") 
  where u.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<UserRoleViewModel>(Query, null);
            return list;

        }

        #endregion

        #region UserPortalBusiness
        public async Task<List<UserPortalViewModel>> GetPortalByUser(string userId)
        {
            var query = @$"SELECT p.""Id"", p.""PortalId"" as PortalId FROM public.""UserPortal"" as p where p.""UserId"" = '{userId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserPortalViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserRolePreferenceViewModel>> GetUserRolePreferences(string userRoleId)
        {
            var query = @$"SELECT p.*,pt.""Name"" as PreferencePortalName,pg.""Name"" as DefaultLandingPageName
                               FROM public.""UserRolePreference"" as p 
                              join public.""Portal"" as pt on pt.""Id"" = p.""PreferencePortalId"" and pt.""IsDeleted"" = false
                              join public.""Page"" as pg on pg.""Id"" = p.""DefaultLandingPageId"" and pg.""IsDeleted"" = false
                         where p.""UserRoleId"" = '{userRoleId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserRolePreferenceViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserPreferenceViewModel>> GetUserPreferences(string userId)
        {
            var query = @$"SELECT p.*,pt.""Name"" as PreferencePortalName,pg.""Name"" as DefaultLandingPageName
                               FROM public.""UserPreference"" as p 
                              join public.""Portal"" as pt on pt.""Id"" = p.""PreferencePortalId"" and pt.""IsDeleted"" = false
                              join public.""Page"" as pg on pg.""Id"" = p.""DefaultLandingPageId"" and pg.""IsDeleted"" = false
                         where p.""UserId"" = '{userId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserPreferenceViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserViewModel>> GetUserByPortal(string portalId)
        {
            var query = @$"SELECT u.*, p.""PortalId"" as PortalId
FROM public.""UserPortal"" as p 
join public.""User"" as u  on u.""Id""=p.""UserId"" and  u.""IsDeleted"" = false and u.""CompanyId""='{_userContext.CompanyId}' where p.""PortalId"" = '{portalId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'
";
            var queryData = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return queryData;
        }
        #endregion

        #region UserHierarchyPermissionBusiness
        public async Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId)
        {
            var query = @$"SELECT up.*,h.""Name"" as HierarchyName
                   FROM public.""UserHierarchyPermission"" as up 
                
                   left join public.""HierarchyMaster"" as h on h.""Id""= up.""HierarchyId""
                   where up.""UserId"" = '{userId}' and up.""IsDeleted"" = false";
            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyPermissionViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchyForPortal(string userId)
        {
            var query = @$"SELECT up.*,h.""Name"" as HierarchyName
                   FROM public.""UserHierarchyPermission"" as up 
                
                   left join public.""HierarchyMaster"" as h on h.""Id""= up.""HierarchyId"" and up.""CompanyId"" = '{_repo.UserContext.CompanyId}' and h.""CompanyId"" = '{_repo.UserContext.CompanyId}' and h.""IsDeleted""=false
                   where up.""UserId"" = '{userId}' and up.""IsDeleted"" = false and h.""PortalId"" = '{_repo.UserContext.PortalId}' and h.""LegalEntityId"" = '{_repo.UserContext.LegalEntityId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyPermissionViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetUserNotInPermissionHierarchy(string UserId)
        {
            var query = @$"SELECT distinct u.*
                       FROM public.""User"" as u
                       join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                       left join public.""UserHierarchyPermission"" as uhp on uhp.""UserId""=u.""Id"" and uhp.""IsDeleted""=false
                       left join public.""HierarchyMaster"" as h on h.""Id""= uhp.""HierarchyId"" and h.""Code""='BUSINESS_HIERARCHY' and h.""IsDeleted""=false
                       where u.""IsDeleted"" = false and up.""PortalId""='{_userContext.PortalId}'  #WHERE# ";
            var where = "";
            if (UserId.IsNotNullAndNotEmpty())
            {
                where = $@" and u.""Id"" NOT IN (select distinct uhp.""UserId"" from public.""UserHierarchyPermission"" as uhp

                        join public.""UserPortal"" as up on up.""UserId""=uhp.""UserId"" and up.""IsDeleted""=false	 WHERE uhp.""UserId"" IS NOT NULL and uhp.""UserId""<>'{UserId}' 
                            and uhp.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}')";
            }
            else
            {
                where = $@" and u.""Id"" NOT IN (select distinct uhp.""UserId"" from public.""UserHierarchyPermission"" as uhp

                        join public.""UserPortal"" as up on up.""UserId""=uhp.""UserId"" and up.""IsDeleted""=false																										 
        WHERE uhp.""UserId"" IS NOT NULL and uhp.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}')";
            }
            query = query.Replace("#WHERE#", where);
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        #endregion
        #region UserRoleHierarchyPermissionBusiness
        public async Task<List<UserRoleHierarchyPermissionViewModel>> GetUserRolePermissionHierarchy(string userRoleId)
        {
            var query = @$"SELECT up.*,h.""Name"" as HierarchyName
                   FROM public.""UserRoleHierarchyPermission"" as up 
                
                   left join public.""HierarchyMaster"" as h on h.""Id""= up.""HierarchyId""
                   where up.""UserRoleId"" = '{userRoleId}' and up.""IsDeleted"" = false";
            var queryData = await _queryRepo.ExecuteQueryList<UserRoleHierarchyPermissionViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserRoleHierarchyPermissionViewModel>> GetUserRolePermissionHierarchyForPortal(string userRoleId)
        {
            var query = @$"SELECT up.*,h.""Name"" as HierarchyName
                   FROM public.""UserRoleHierarchyPermission"" as up 
                
                   left join public.""HierarchyMaster"" as h on h.""Id""= up.""HierarchyId"" and up.""CompanyId"" = '{_repo.UserContext.CompanyId}' and h.""CompanyId"" = '{_repo.UserContext.CompanyId}' and h.""IsDeleted""=false
                   where up.""UserRoleId"" = '{userRoleId}' and up.""IsDeleted"" = false and h.""PortalId"" = '{_repo.UserContext.PortalId}' and h.""LegalEntityId"" = '{_repo.UserContext.LegalEntityId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserRoleHierarchyPermissionViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetUserRoleNotInPermissionHierarchy(string UserRoleId)
        {
            var query = @$"SELECT distinct u.*
                       FROM public.""User"" as u
                       join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
left join public.""UserRole"" as ur on ur.""UserId""=u.""Id"" and uhp.""IsDeleted""=false
                       left join public.""UserRoleHierarchyPermission"" as uhp on uhp.""UserRoleId""=ur.""Id"" and uhp.""IsDeleted""=false
                       left join public.""HierarchyMaster"" as h on h.""Id""= uhp.""HierarchyId"" and h.""Code""='BUSINESS_HIERARCHY' and h.""IsDeleted""=false
                       where u.""IsDeleted"" = false and up.""PortalId""='{_userContext.PortalId}'  #WHERE# ";
            var where = "";
            if (UserRoleId.IsNotNullAndNotEmpty())
            {
                where = $@" and u.""Id"" NOT IN (select distinct uhp.""UserId"" from public.""UserHierarchyPermission"" as uhp

                        join public.""UserPortal"" as up on up.""UserId""=uhp.""UserId"" and up.""IsDeleted""=false	 WHERE uhp.""UserId"" IS NOT NULL and uhp.""UserId""<>'{UserRoleId}' 
                            and uhp.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}')";
            }
            else
            {
                where = $@" and u.""Id"" NOT IN (select distinct uhp.""UserId"" from public.""UserRoleHierarchyPermission"" as uhp
 join public.""UserRole"" as ur on ur.""UserId""=uhp.""UserRoleId"" and ur.""IsDeleted""=false	
                        join public.""UserPortal"" as up on up.""UserId""=uhp.""UserId"" and up.""IsDeleted""=false																										 
        WHERE uhp.""UserId"" IS NOT NULL and uhp.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}')";
            }
            query = query.Replace("#WHERE#", where);
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        #endregion

        #region UserHierarchyBusiness
        public async Task<IList<UserHierarchyViewModel>> GetHierarchyListForAllPortals(string HierarchyId)
        {
            var query = @$"select u.""Id"" as UserIds,u.""Name"" as UserName,ur11.""ParentUserId"" as Level1ApproverOption1UserId,ur11.""username"" as Level1ApproverOption1UserName,ur12.""ParentUserId"" as Level1ApproverOption2UserId,ur12.""username"" as Level1ApproverOption2UserName,ur13.""ParentUserId"" as Level1ApproverOption3UserId,ur13.""username"" as Level1ApproverOption3UserName,
ur21.""ParentUserId"" as Level2ApproverOption1UserId,ur21.""username"" as Level2ApproverOption1UserName,ur22.""ParentUserId"" as Level2ApproverOption2UserId,ur22.""username"" as Level2ApproverOption2UserName,ur23.""ParentUserId"" as Level2ApproverOption3UserId,ur23.""username"" as Level2ApproverOption3UserName,
ur31.""ParentUserId"" as Level3ApproverOption1UserId,ur31.""username"" as Level3ApproverOption1UserName,ur32.""ParentUserId"" as Level3ApproverOption2UserId,ur32.""username"" as Level3ApproverOption2UserName,ur33.""ParentUserId"" as Level3ApproverOption3UserId,ur33.""username"" as Level3ApproverOption3UserName,
ur41.""ParentUserId"" as Level4ApproverOption1UserId,ur41.""username"" as Level4ApproverOption1UserName,ur42.""ParentUserId"" as Level4ApproverOption2UserId,ur42.""username"" as Level4ApproverOption2UserName,ur43.""ParentUserId"" as Level4ApproverOption3UserId,ur43.""username"" as Level4ApproverOption3UserName,
ur51.""ParentUserId"" as Level5ApproverOption1UserId,ur51.""username"" as Level5ApproverOption1UserName,ur52.""ParentUserId"" as Level5ApproverOption2UserId,ur52.""username"" as Level5ApproverOption2UserName,ur53.""ParentUserId"" as Level5ApproverOption3UserId,ur53.""username"" as Level5ApproverOption3UserName
from public.""User"" as u
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false --and up.""PortalId""='{_userContext.PortalId}' and u.""IsDeleted""=false
 join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	  u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur11   on ur11.""UserId"" = u.""Id""
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur12   on ur12.""UserId"" = u.""Id""
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur13   on ur13.""UserId"" = u.""Id""	

		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur21   on ur21.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur22   on ur22.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur23   on ur23.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur31   on ur31.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur32   on ur32.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur33   on ur33.""UserId"" = u.""Id""		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur41   on ur41.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur42   on ur42.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur43   on ur43.""UserId"" = u.""Id""	
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur51   on ur51.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur52   on ur52.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur53   on ur53.""UserId"" = u.""Id""		

       
                                ";


            var list = await _queryRepo.ExecuteQueryList<UserHierarchyViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserHierarchyViewModel>> GetHierarchyList(string HierarchyId, string userId)
        {
            var query = @$"select u.""Id"" as UserIds,u.""Name"" as UserName,ur11.""ParentUserId"" as Level1ApproverOption1UserId,ur11.""username"" as Level1ApproverOption1UserName,ur12.""ParentUserId"" as Level1ApproverOption2UserId,ur12.""username"" as Level1ApproverOption2UserName,ur13.""ParentUserId"" as Level1ApproverOption3UserId,ur13.""username"" as Level1ApproverOption3UserName,
ur21.""ParentUserId"" as Level2ApproverOption1UserId,ur21.""username"" as Level2ApproverOption1UserName,ur22.""ParentUserId"" as Level2ApproverOption2UserId,ur22.""username"" as Level2ApproverOption2UserName,ur23.""ParentUserId"" as Level2ApproverOption3UserId,ur23.""username"" as Level2ApproverOption3UserName,
ur31.""ParentUserId"" as Level3ApproverOption1UserId,ur31.""username"" as Level3ApproverOption1UserName,ur32.""ParentUserId"" as Level3ApproverOption2UserId,ur32.""username"" as Level3ApproverOption2UserName,ur33.""ParentUserId"" as Level3ApproverOption3UserId,ur33.""username"" as Level3ApproverOption3UserName,
ur41.""ParentUserId"" as Level4ApproverOption1UserId,ur41.""username"" as Level4ApproverOption1UserName,ur42.""ParentUserId"" as Level4ApproverOption2UserId,ur42.""username"" as Level4ApproverOption2UserName,ur43.""ParentUserId"" as Level4ApproverOption3UserId,ur43.""username"" as Level4ApproverOption3UserName,
ur51.""ParentUserId"" as Level5ApproverOption1UserId,ur51.""username"" as Level5ApproverOption1UserName,ur52.""ParentUserId"" as Level5ApproverOption2UserId,ur52.""username"" as Level5ApproverOption2UserName,ur53.""ParentUserId"" as Level5ApproverOption3UserId,ur53.""username"" as Level5ApproverOption3UserName
from public.""User"" as u
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	  u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur11   on ur11.""UserId"" = u.""Id""
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur12   on ur12.""UserId"" = u.""Id""
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur13   on ur13.""UserId"" = u.""Id""	

		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur21   on ur21.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur22   on ur22.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur23   on ur23.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur31   on ur31.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur32   on ur32.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur33   on ur33.""UserId"" = u.""Id""		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur41   on ur41.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur42   on ur42.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur43   on ur43.""UserId"" = u.""Id""	
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur51   on ur51.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur52   on ur52.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur53   on ur53.""UserId"" = u.""Id""		

        #Where#
                                ";

            var where = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                where = @$"  where u.""Id""='{userId}' ";
            }
            query = query.Replace("#Where#", where);
            var list = await _queryRepo.ExecuteQueryList<UserHierarchyViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserHierarchyViewModel>> GetHierarchyListForPortal(string HierarchyId, string userId)
        {
            var query = @$"select u.""Id"" as UserIds,u.""Name"" as UserName,ur11.""ParentUserId"" as Level1ApproverOption1UserId,ur11.""username"" as Level1ApproverOption1UserName,ur12.""ParentUserId"" as Level1ApproverOption2UserId,ur12.""username"" as Level1ApproverOption2UserName,ur13.""ParentUserId"" as Level1ApproverOption3UserId,ur13.""username"" as Level1ApproverOption3UserName,
ur21.""ParentUserId"" as Level2ApproverOption1UserId,ur21.""username"" as Level2ApproverOption1UserName,ur22.""ParentUserId"" as Level2ApproverOption2UserId,ur22.""username"" as Level2ApproverOption2UserName,ur23.""ParentUserId"" as Level2ApproverOption3UserId,ur23.""username"" as Level2ApproverOption3UserName,
ur31.""ParentUserId"" as Level3ApproverOption1UserId,ur31.""username"" as Level3ApproverOption1UserName,ur32.""ParentUserId"" as Level3ApproverOption2UserId,ur32.""username"" as Level3ApproverOption2UserName,ur33.""ParentUserId"" as Level3ApproverOption3UserId,ur33.""username"" as Level3ApproverOption3UserName,
ur41.""ParentUserId"" as Level4ApproverOption1UserId,ur41.""username"" as Level4ApproverOption1UserName,ur42.""ParentUserId"" as Level4ApproverOption2UserId,ur42.""username"" as Level4ApproverOption2UserName,ur43.""ParentUserId"" as Level4ApproverOption3UserId,ur43.""username"" as Level4ApproverOption3UserName,
ur51.""ParentUserId"" as Level5ApproverOption1UserId,ur51.""username"" as Level5ApproverOption1UserName,ur52.""ParentUserId"" as Level5ApproverOption2UserId,ur52.""username"" as Level5ApproverOption2UserName,ur53.""ParentUserId"" as Level5ApproverOption3UserId,ur53.""username"" as Level5ApproverOption3UserName
from public.""User"" as u
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	  u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and ur.""CompanyId""='{_repo.UserContext.CompanyId}'
	where ur.""LevelNo""=1 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}' 
        ) as ur11   on ur11.""UserId"" = u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur12   on ur12.""UserId"" = u.""Id""
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false 
	where ur.""LevelNo""=1 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur13   on ur13.""UserId"" = u.""Id""	

		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur21   on ur21.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur22   on ur22.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur23   on ur23.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur31   on ur31.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur32   on ur32.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur33   on ur33.""UserId"" = u.""Id""		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur 
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur41   on ur41.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur42   on ur42.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur43   on ur43.""UserId"" = u.""Id""	
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""   and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur51   on ur51.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur52   on ur52.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false 
	where ur.""LevelNo""=5 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur53   on ur53.""UserId"" = u.""Id""		
where  u.""LegalEntityId""='{_repo.UserContext.LegalEntityId}' and u.""PortalId""='{_repo.UserContext.PortalId}'
        #Where#
                                ";

            var where = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                where = @$"  and u.""Id""='{userId}' ";
            }
            query = query.Replace("#Where#", where);
            var list = await _queryRepo.ExecuteQueryList<UserHierarchyViewModel>(query, null);
            return list;
        }
        public async Task<UserHierarchyViewModel> GetHierarchyUser(string HierarchyId, string userId)
        {
            var query = @$"select u.""Id"" as UserIds,u.""Name"" as UserName,ur11.""ParentUserId"" as Level1ApproverOption1UserId,ur11.""username"" as Level1ApproverOption1UserName,ur12.""ParentUserId"" as Level1ApproverOption2UserId,ur12.""username"" as Level1ApproverOption2UserName,ur13.""ParentUserId"" as Level1ApproverOption3UserId,ur13.""username"" as Level1ApproverOption3UserName,
ur21.""ParentUserId"" as Level2ApproverOption1UserId,ur21.""username"" as Level2ApproverOption1UserName,ur22.""ParentUserId"" as Level2ApproverOption2UserId,ur22.""username"" as Level2ApproverOption2UserName,ur23.""ParentUserId"" as Level2ApproverOption3UserId,ur23.""username"" as Level2ApproverOption3UserName,
ur31.""ParentUserId"" as Level3ApproverOption1UserId,ur31.""username"" as Level3ApproverOption1UserName,ur32.""ParentUserId"" as Level3ApproverOption2UserId,ur32.""username"" as Level3ApproverOption2UserName,ur33.""ParentUserId"" as Level3ApproverOption3UserId,ur33.""username"" as Level3ApproverOption3UserName,
ur41.""ParentUserId"" as Level4ApproverOption1UserId,ur41.""username"" as Level4ApproverOption1UserName,ur42.""ParentUserId"" as Level4ApproverOption2UserId,ur42.""username"" as Level4ApproverOption2UserName,ur43.""ParentUserId"" as Level4ApproverOption3UserId,ur43.""username"" as Level4ApproverOption3UserName,
ur51.""ParentUserId"" as Level5ApproverOption1UserId,ur51.""username"" as Level5ApproverOption1UserName,ur52.""ParentUserId"" as Level5ApproverOption2UserId,ur52.""username"" as Level5ApproverOption2UserName,ur53.""ParentUserId"" as Level5ApproverOption3UserId,ur53.""username"" as Level5ApproverOption3UserName
from public.""User"" as u
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	  u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur11   on ur11.""UserId"" = u.""Id""
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur12   on ur12.""UserId"" = u.""Id""
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur13   on ur13.""UserId"" = u.""Id""	

		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur21   on ur21.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur22   on ur22.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur23   on ur23.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur31   on ur31.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur32   on ur32.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur33   on ur33.""UserId"" = u.""Id""		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur41   on ur41.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur42   on ur42.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur43   on ur43.""UserId"" = u.""Id""	
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur51   on ur51.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur52   on ur52.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur53   on ur53.""UserId"" = u.""Id""		

        #Where#
                                ";

            var where = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                where = @$"  where u.""Id""='{userId}' ";
            }
            query = query.Replace("#Where#", where);
            var list = await _queryRepo.ExecuteQuerySingle<UserHierarchyViewModel>(query, null);
            return list;
        }

        public async Task<List<UserViewModel>> GetHierarchyUsers(string hierarchyCode, string parentUserId, int level, int option)
        {
            var query = @$" select u.*
from public.""HierarchyMaster"" as hm
join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false
join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
where hm.""Code""='{hierarchyCode}' and uh.""ParentUserId""='{parentUserId}'
and uh.""LevelNo""={level} and uh.""OptionNo""={option}   and hm.""IsDeleted""=false ";


            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<List<UserHierarchyViewModel>> GetPerformanceHierarchyUsers(string parentUserId)
        {
            var query = @$" WITH RECURSIVE pos AS(
                                select uh.*,'DIRECT' as Type
from public.""HierarchyMaster"" as hm
join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false
join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
where hm.""Code""='PERFORMANCE_HIERARCHY' and uh.""ParentUserId""='{parentUserId}'
and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false


                              union all

                                  select uh.*,'INDIRECT' as Type
from public.""HierarchyMaster"" as hm
join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false	 
join pos as po on po.""UserId""=uh.""ParentUserId""	 
join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
where hm.""Code""='PERFORMANCE_HIERARCHY' 
and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false
                             )
                            select* from pos ";


            var list = await _queryRepo.ExecuteQueryList<UserHierarchyViewModel>(query, null);
            return list;
        }

        public async Task<List<UserHierarchyViewModel>> GetUserHierarchyByCode(string code, string parentUserId)
        {
            var query = @$" WITH RECURSIVE pos AS(
                                select uh.*,'DIRECT' as Type
                from public.""HierarchyMaster"" as hm
                join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false
                join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
                where hm.""Code""='{code}' and uh.""ParentUserId""='{parentUserId}'
                and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false


                                              union all

                                                  select uh.*,'INDIRECT' as Type
                from public.""HierarchyMaster"" as hm
                join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false	 
                join pos as po on po.""UserId""=uh.""ParentUserId""	 
                join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
                where hm.""Code""='{code}' 
                and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false
                                             )
                            select* from pos ";


            var list = await _queryRepo.ExecuteQueryList<UserHierarchyViewModel>(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetNonExistingUser(string hierarchyId, string userId)
        {
            string query = $@" select u.""Id"" as Id ,u.""Name"" as Name
                            from public.""User"" as u
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
where ((u.""IsDeleted""=false and  u.""CompanyId""='{_repo.UserContext.CompanyId}') and u.""Id"" not in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}') and u.""Id"" not in 
	  (SELECT ""UserId"" FROM public.""UserHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}') 
and u.""Id"" not in 	  (SELECT ""ParentUserId"" FROM public.""UserHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' and ""IsDeleted""=false and ""ParentUserId"" is not null))  
and up.""PortalId""='{_repo.UserContext.PortalId}'";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }

        #endregion

        #region UserGroupBusiness
        public async Task<List<TagCategoryViewModel>> GetAllTagCategory()
        {
            var query = $@"SELECT Ns.""TagCategoryType"", Ns.""TagCategoryCode"", Ns.""TagCategoryName"", 
         Ns.""EnableAutoTag"", Ns""TagSourceId"", N.""Id""

    FROM cms.""N_General_TagCategory"" as Ns   inner join public.""NtsNote"" N on Ns.""NtsNoteId""=N.""Id"" and NS.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""CompanyId""='{_repo.UserContext.CompanyId}' and NS.""IsDeleted""=false and N.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);




            //var list = new List<TagCategoryViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new TagCategoryViewModel { TagCategoryName = x.TagCategoryName, TagCategoryCode = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return queryData;
        }

        public async Task<TagCategoryViewModel> GetTagCategoryDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_General_TagCategory"" as TC inner join public.""NtsNote"" as N on TC.""NtsNoteId""=N.""Id"" and TC.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""CompanyId""='{_repo.UserContext.CompanyId}' where TC.""Id""='{Id}' and  TC.""IsDeleted""=false and  N.""IsDeleted""=false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllSourceID()
        {
            var query = $@"SELECT ""Id"",  ""DisplayName"" as Name  FROM public.""TableMetadata"" where  ""IsDeleted""='false' and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);

            var list = new List<IdNameViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return queryData;
        }

        public async Task DeleteTagCategory(string Id)
        {
            var query = $@"Update cms.""N_General_TagCategory"" set ""IsDeleted""='True' where ""Id""='{Id}'";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task DeleteTag(string Id)
        {
            var query = $@"Update cms.""N_General_Tag"" set ""IsDeleted""='True' where ""Id""='{Id}'";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<TagCategoryViewModel> IsTagCategoryNameExist(string TagCategoryName, string Id)
        {
            var query = $@"Select * from cms.""N_General_TagCategory"" where ""TagCategoryName""='{TagCategoryName}' and ""IsDeleted""='false' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and ""NtsNoteId"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return queryData;
        }


        public async Task<TagCategoryViewModel> IsTagCategoryCodeExist(string TagCategoryCode, string Id)
        {
            var query = $@"Select * from cms.""N_General_TagCategory"" where ""TagCategoryCode""='{TagCategoryCode}' and ""IsDeleted""='false' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and ""NtsNoteId"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return queryData;
        }


        public async Task<TagCategoryViewModel> IsParentAssignTosourceTagExist(string ParentId, string TagSourceId, string Id)
        {
            var query = $@"select * from cms.""N_General_TagCategory"" As TC
                           inner join public.""NtsNote"" as N on TC.""NtsNoteId""=N.""Id"" and N .""IsDeleted""='false'
	         where N.""ParentNoteId""='{Id}' and TC.""NtsNoteId""='{ParentId}'  and TC .""IsDeleted""='false'";

            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return queryData;
        }



        public async Task<List<IdNameViewModel>> GetParentTagCategory()
        {
            var query = $@"select ""NtsNoteId"" as Id,""TagCategoryName"" as Name from   cms.""N_General_TagCategory"" where  ""IsDeleted""='false' and ""CompanyId""='{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }


        //Tag


        public async Task<List<TagViewModel>> GetTagList(string CategoryId)
        {
            var query = $@"Select  T.*,T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_Tag"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and T.""CompanyId""='{_repo.UserContext.CompanyId}'
            where N.""ParentNoteId"" = '{CategoryId}'  and T.""IsDeleted"" = false and N.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQueryList<TagViewModel>(query, null);
            return queryData;
        }


        public async Task<TagViewModel> GetTagEdit(string NoteId)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_General_Tag"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and T.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""CompanyId""='{_repo.UserContext.CompanyId}'
            where T.""Id"" = '{NoteId}' and T.""IsDeleted"" = 'false'";

            var queryData = await _queryRepo.ExecuteQuerySingle<TagViewModel>(query, null);
            return queryData;
        }


        public async Task<TagViewModel> GetNoteId(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_General_TagCategory"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" 
            where T.""Id"" = '{Id}' and T.""IsDeleted"" = 'false'";

            var queryData = await _queryRepo.ExecuteQuerySingle<TagViewModel>(query, null);
            return queryData;
        }

        public async Task<TagViewModel> IsTagNameExist(string Parentid, string TagName, string Id)
        {
            var query = $@"Select  T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_Tag"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" 
            where N.""ParentNoteId"" = '{Parentid}' and  N.""NoteSubject""='{TagName}'  and T.""IsDeleted"" = 'false' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and ""NtsNoteId"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryRepo.ExecuteQuerySingle<TagViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserGroupViewModel>> GetTeamWithPortalIds()
        {

            var Query = $@"SELECT distinct u.* FROM public.""UserGroup"" as u 
            join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and array[u.""AllowedPortalIds""] <@ array[c.""LicensedPortalIds""] and c.""IsDeleted""=false
            where u.""IsDeleted""=false and u.""AllowedPortalIds"" is not null";
            var list = await _queryRepo.ExecuteQueryList<UserGroupViewModel>(Query, null);
            return list;

        }
        #endregion


        public async Task<List<IdNameViewModel>> GetHierarchyData(HierarchyTypeEnum type)
        {
            if (type == HierarchyTypeEnum.User)
            {
                var query = @$"SELECT up.""Id"",u.""Name""
                   FROM cms.""N_GENERAL_UserHierarchy"" as up 
                
                    join public.""User"" as u on u.""Id""= up.""UserId"" and u.""IsDeleted""=false
                   where  up.""IsDeleted"" = false";
                var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                return queryData;
            }
            else if (type == HierarchyTypeEnum.Position)
            {
                var query = @$"SELECT up.""Id"",u.""PositionName""  as ""Name""
                   FROM cms.""N_CoreHR_PositionHierarchy"" as up 
                
                    join cms.""N_CoreHR_HRPosition"" as u on u.""Id""= up.""PositionId"" and u.""IsDeleted""=false
                   where  up.""IsDeleted"" = false";
                var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                return queryData;
            }
            else if (type == HierarchyTypeEnum.Organization)
            {
                var query = @$"SELECT up.""Id"",u.""DepartmentName"" as ""Name""
                   FROM cms.""N_CoreHR_PositionHierarchy"" as up 
                
                    join cms.""N_CoreHR_HRDepartmentHierarchy"" as u on u.""Id""= up.""DepartmentId"" and u.""IsDeleted""=false
                   where  up.""IsDeleted"" = false";
                var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                return queryData;
            }
            else if (type == HierarchyTypeEnum.Hybrid)
            {
                var query = @$"SELECT up.""Id"",h.""DepartmentName"" as ""Name""
                   FROM public.""HybridHierarchy"" as up 
                
                    join cms.""N_CoreHR_HRDepartmentHierarchy"" as u on u.""Id""= up.""DepartmentId"" and u.""IsDeleted""=false
                   where  up.""IsDeleted"" = false";
                var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                return queryData;
            }
            else
            {
                return new List<IdNameViewModel>();
            }
        }

        public async Task<List<UserViewModel>> GetAllCSCUsersList()
        {
            var Query = $@"SELECT u.* FROM public.""User"" as u 
 join public.""UserRoleUser"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
 join public.""UserRole"" as r on up.""UserRoleId""=r.""Id"" and r.""IsDeleted""=false
  where u.""IsDeleted""=false  and r.""Code""='CSC_USER'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(Query, null);
            return list;

        }
        public async Task<List<PositionChartViewModel>> GetUserHierarchyChartData(string parentId, int levelUpto, string hierarchyId, int level = 1, int option = 1)
        {

            string query = $@"with recursive hrchy as(
                                select u.""Id"" as ""Id"",'' COLLATE ""tr-TR-x-icu"" as ""ParentId"",'Parent' as Type,0 As level
                                from public.""User"" as u where u.""Id"" = '{parentId}' and  u.""IsDeleted""=false 
                                union all
                                select  uh.""UserId"" as ""Id"",uh.""ParentUserId"" as ""ParentId"",'Child' as Type,hrchy.level+ 1 as level
                                from public.""HierarchyMaster"" as h
								join public.""UserHierarchy"" as uh on ""uh"".""HierarchyMasterId""=h.""Id"" and  uh.""IsDeleted""=false 
                                join hrchy  on hrchy.""Id""=uh.""ParentUserId"" 
                                where uh.""LevelNo""={level} and uh.""OptionNo""={option} and  h.""Id""='{hierarchyId}'  and  h.""IsDeleted""=false 
                             )select u.""Id"" as Id,j.""Id"" as JobId,j.""JobTitle"" as JobName,
                            o.""Id"" as OrganizationId, o.""DepartmentName"" as OrganizationName,g.""GradeName"" as GradeName,
                            hrchy.""ParentId"" as ParentId,
                            coalesce(dc.""DC"",0) as DirectChildCount,
                            u.""PhotoId"" as PhotoId,
                            u.""Name"" as DisplayName,
                            u.""Id"" as UserId,
                            '{hierarchyId}' as HierarchyId,
                            Case when p.""Id"" is not null then 'org-node-1' else 'org-node-3' end as CssClass
                          
                            from hrchy 
							join public.""User"" as u on hrchy.""Id""=u.""Id""
                            left join 
                            (
                                select uh.""ParentUserId"" as ""UserId"",count(uh.""UserId"") as ""DC""
                                from public.""HierarchyMaster"" as h
								join public.""UserHierarchy"" as uh on ""uh"".""HierarchyMasterId""=h.""Id"" and  uh.""IsDeleted""=false 
                                where uh.""LevelNo""={level} and uh.""OptionNo""={option} and  h.""Id""='{hierarchyId}'  
                                and  h.""IsDeleted""=false 
                                group by uh.""ParentUserId""
                            )as dc on u.""Id""=dc.""UserId""
                            
                            Left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId"" and j.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false 
                            where hrchy.""level""<={levelUpto}";
            var queryData = await _queryRepo.ExecuteQueryList<PositionChartViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<IdNameViewModel>> GetStepTaskTemplateList(string serviceTemplateId)
        {
            //string query = @$"select comp.""Id"" as Id,t.""DisplayName"" as Name,comp.""ParentId"" as Code FROM  public.""Template"" as t 
            //join public.""StepTaskComponent"" as stc on t.""Id""=stc.""TemplateId"" and stc.""IsDeleted"" = false
            //join public.""Component"" as comp on comp.""Id""=stc.""ComponentId"" and comp.""IsDeleted"" = false
            //join public.""ProcessDesign"" as p on p.""Id""=comp.""ProcessDesignId"" and p.""IsDeleted""=false
            //WHERE t.""IsDeleted"" = false and p.""TemplateId""='{serviceTemplateId}' ";
            string query = @$"select comp.""Id"" as Id,comp.""Name"" as Name,comp.""ParentId"" as Code
                 FROM  public.""Template"" as ""T""
			    join public.""ProcessDesign"" as ""PD"" on ""T"".""Id""=""PD"".""TemplateId""
            join public.""Component"" as comp on ""PD"".""Id""=comp.""ProcessDesignId""
			 WHERE ""T"".""IsDeleted"" = false and ""T"".""Id""='{serviceTemplateId}'";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<TemplateViewModel>> GetMasterList(string groupCode)
        {
            string query = $@"Select t.""DisplayName"",t.""Code"",t.""TemplateType"",t.""TemplateCategoryId"",tc.""Name"" as TemplateCategoryName,t.""DisplayName"" as PageName
            from public.""Template"" as t
            join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
            where t.""GroupCode""='{groupCode}' and t.""IsDeleted""=false ";

            var result = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return result;
        }

        public async Task<List<TemplateViewModel>> GetTemplatesList(string portalId, TemplateTypeEnum templateType)
        {
            string portalIds = portalId.Replace(",", "\",\"");
            //portalIds = portalIds.Replace("'", "\"");
            portalIds = String.Concat("{\"", portalIds, "\"}");
            //string query = $@"Select t.""Id"", t.""DisplayName"",t.""Code"",t.""TemplateType"",t.""TemplateCategoryId"",tc.""Name"" as TemplateCategoryName,t.""DisplayName"" as PageName
            //from public.""Template"" as t
            //join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
            //where '{portalId}'=any(tc.""AllowedPortalIds"") and t.""TemplateType""='{(int)templateType}' and t.""IsDeleted""=false ";

            //         string query = $@"Select t.""Id"", t.""DisplayName"",t.""Code"",t.""TemplateType"",tc.""Id"" as TemplateCategoryId,tc.""Name"" as TemplateCategoryName,t.""DisplayName"" as PageName
            //         from public.""TemplateCategory"" as tc			
            //left join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""TemplateType""='{(int)templateType}'
            //         where '{portalId}'=any(tc.""AllowedPortalIds"") and tc.""TemplateType""='{(int)templateType}' and tc.""IsDeleted""=false ";

            string query = $@"Select t.""Id"", t.""DisplayName"",t.""Code"",t.""TemplateType"",tc.""Id"" as TemplateCategoryId,tc.""Name"" as TemplateCategoryName,t.""DisplayName"" as PageName
            from public.""TemplateCategory"" as tc			
			left join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""TemplateType""='{(int)templateType}'
            where tc.""AllowedPortalIds"" && '{portalIds}' and tc.""TemplateType""='{(int)templateType}' and tc.""IsDeleted""=false ";


            var result = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetCSCOfficeType(string templateCode)
        {

            var appLanguage = _userContext.CultureName;
            var localizedColumn = @$"ot.""OfficeName"" as ""Name""";
            switch (appLanguage)
            {
                case "ar-SA":
                    localizedColumn = @$"coalesce(lang.""Arabic"",ot.""OfficeName"") as ""Name""";
                    break;
                case "hi-IN":
                    localizedColumn = @$"coalesce(lang.""Hindi"",ot.""OfficeName"") as ""Name""";
                    break;
                default:
                    break;
            }
            var query = $@"select  ot.""Id"",{localizedColumn}
from public.""Template"" as t 
join cms.""F_CSC_SETTINGS_OfficeServiceMapping"" as om on om.""TemplateId""=t.""Id"" and om.""IsDeleted""=false
join cms.""F_CSC_SETTINGS_OfficeType"" as ot on ot.""Id""=om.""OfficeId"" and ot.""IsDeleted""=false
left join public.""FormResourceLanguage"" as lang on ""ot"".""Id""=lang.""FormTableId""  and lang.""IsDeleted""=false
where t.""Code""='{templateCode}' and t.""IsDeleted""=false";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetCSCSubfficeType(string officeId, string districtId)
        {
            IList<IdNameViewModel> list = new List<IdNameViewModel>();
            var query = $@"select  ot.""Id"",ot.""OfficeName"" as ""Name"",ot.""TableName"" as Code
            from  cms.""F_CSC_SETTINGS_OfficeType"" as ot where ot.""Id""='{officeId}' and ot.""IsDeleted""=false";
            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            if (queryData != null)
            {
                var appLanguage = _userContext.CultureName;
                var localizedColumn = @$"";
                switch (queryData.Code)
                {
                    case "F_CSC_GEOGRAPHIC_SubDistrict":
                        localizedColumn = @$"ot.""SubDistrictName"" as ""Name""";
                        switch (appLanguage)
                        {
                            case "ar-SA":
                                localizedColumn = @$"coalesce(lang.""Arabic"",ot.""SubDistrictName"") as ""Name""";
                                break;
                            case "hi-IN":
                                localizedColumn = @$"coalesce(lang.""Hindi"",ot.""SubDistrictName"") as ""Name""";
                                break;
                            default:
                                break;
                        }
                        query = $@"select  ot.""Id"",{localizedColumn} from  cms.""{queryData.Code}"" as ot 
                        left join public.""FormResourceLanguage"" as lang on ""ot"".""Id""=lang.""FormTableId""  and lang.""IsDeleted""=false
                        where ot.""DistrictId""='{districtId}' and  ot.""IsDeleted""=false";
                        list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                        break;
                    case "F_CSC_GEOGRAPHIC_Municipality":
                        localizedColumn = @$"ot.""MunicipalityName"" as ""Name""";
                        switch (appLanguage)
                        {
                            case "ar-SA":
                                localizedColumn = @$"coalesce(lang.""Arabic"",ot.""MunicipalityName"") as ""Name""";
                                break;
                            case "hi-IN":
                                localizedColumn = @$"coalesce(lang.""Hindi"",ot.""MunicipalityName"") as ""Name""";
                                break;
                            default:
                                break;
                        }
                        query = $@"select  ot.""Id"",{localizedColumn} from  cms.""{queryData.Code}"" as ot 
                        left join public.""FormResourceLanguage"" as lang on ""ot"".""Id""=lang.""FormTableId""  and lang.""IsDeleted""=false
                        where ot.""DistrictId""='{districtId}' and  ot.""IsDeleted""=false";
                        list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                        break;
                    case "F_CSC_GEOGRAPHIC_Corporation":
                        localizedColumn = @$"ot.""CorporationName"" as ""Name""";
                        switch (appLanguage)
                        {
                            case "ar-SA":
                                localizedColumn = @$"coalesce(lang.""Arabic"",ot.""CorporationName"") as ""Name""";
                                break;
                            case "hi-IN":
                                localizedColumn = @$"coalesce(lang.""Hindi"",ot.""CorporationName"") as ""Name""";
                                break;
                            default:
                                break;
                        }
                        query = $@"select  ot.""Id"",{localizedColumn} as ""Name"" from  cms.""{queryData.Code}"" as ot 
                        left join public.""FormResourceLanguage"" as lang on ""ot"".""Id""=lang.""FormTableId""  and lang.""IsDeleted""=false
                        where ot.""DistrictId""='{districtId}' and  ot.""IsDeleted""=false";
                        list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                        break;
                    default:
                        break;
                }


            }
            return list.ToList();
        }

        public async Task<List<IdNameViewModel>> GetRevenueVillage(string officeId, string subDistrictId)
        {
            IList<IdNameViewModel> list = new List<IdNameViewModel>();
            var query = $@"select  ot.""Id"",ot.""OfficeName"" as ""Name"",ot.""TableName"" as Code
            from  cms.""F_CSC_SETTINGS_OfficeType"" as ot where ot.""Id""='{officeId}' and ot.""IsDeleted""=false";
            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            if (queryData != null)
            {
                var appLanguage = _userContext.CultureName;
                var localizedColumn = @$"ot.""RevenueVillageName"" as ""Name""";
                switch (appLanguage)
                {
                    case "ar-SA":
                        localizedColumn = @$"coalesce(lang.""Arabic"",ot.""RevenueVillageName"") as ""Name""";
                        break;
                    case "hi-IN":
                        localizedColumn = @$"coalesce(lang.""Hindi"",ot.""RevenueVillageName"") as ""Name""";
                        break;
                    default:
                        break;
                }
                switch (queryData.Code)
                {
                    case "F_CSC_GEOGRAPHIC_SubDistrict":
                        query = $@"select  ot.""Id"",{localizedColumn} from  cms.""F_CSC_GEOGRAPHIC_RevenueVillage"" as ot 
                        left join public.""FormResourceLanguage"" as lang on ""ot"".""Id""=lang.""FormTableId""  and lang.""IsDeleted""=false
                        where ot.""SubDistrictId""='{subDistrictId}' and  ot.""IsDeleted""=false";
                        list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                        break;
                    default:
                        query = $@"select  ot.""Id"",{localizedColumn} from  cms.""F_CSC_GEOGRAPHIC_RevenueVillage"" as ot 
                        left join public.""FormResourceLanguage"" as lang on ""ot"".""Id""=lang.""FormTableId""  and lang.""IsDeleted""=false
                        where ot.""IsDeleted""=false";
                        list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
                        break;
                }


            }
            return list.ToList();
        }

        public async Task UpdateMarriageCerfificateFile(string referenceId, string fileId)
        {
            try
            {
                string query = @$"UPDATE cms.""N_SNC_CSC_SERVICES_Csc_Marriage_Certificate""
                SET  ""MarriageCertificateFileId"" = '{fileId}' FROM public.""NtsService""
                WHERE cms.""N_SNC_CSC_SERVICES_Csc_Marriage_Certificate"".""Id"" = public.""NtsService"".""UdfNoteTableId""
                and public.""NtsService"".""Id""='{referenceId}';";
                await _queryRepo.ExecuteCommand(query, null);
            }
            catch (Exception)
            {

            }
        }
        public async Task<List<DynamicGridViewModel>> GetDataGridValue(string parentId)
        {
            var Query = $@"Select ""Department"" as department, ""services"" as service from cms.""N_CMS_SEBI_SEBIInternalServices"" where ""ParentId""='{parentId}'";
            return await _queryRepo.ExecuteQueryList<DynamicGridViewModel>(Query, null);
        }

        public async Task<List<TeamViewModel>> GetTeamWithUsersByPortalId(string portalId, string teamIds)
        {
            string query = $@"select t.""Id"",t.""Name"",t.""Description"",t.""CreatedDate"",t.""LogoId"",u.""Name"" as UserName,u.""PhotoId"",tu.""IsTeamOwner""
from public.""Team"" as t
join public.""TeamUser"" as tu on t.""Id""=tu.""TeamId"" and tu.""IsDeleted""=false
join public.""User"" as u on tu.""UserId""=u.""Id"" and u.""IsDeleted""=false
where t.""IsDeleted""=false and t.""PortalId""='{portalId}' and t.""Id"" in ('{teamIds.Replace(",", "','")}')
order by t.""CreatedDate"" ";

            var list = await _queryRepo.ExecuteQueryList<TeamViewModel>(query, null);

            return list;
        }

        public async Task<PageViewModel> GetUserLandingPage(string userId, string portalId)
        {
            var query = @$"select pa.*
                from public.""Page"" as pa 
                join public.""UserPreference"" as up on pa.""Id""=up.""DefaultLandingPageId""  and ""up"".""IsDeleted""=false
                where  up.""PreferencePortalId""='{portalId}' and up.""UserId""='{userId}' 
                and pa.""IsDeleted""=false";
            var page = await _queryRepo.ExecuteQuerySingle<PageViewModel>(query, null);
            //if (page != null)
            //{
            //    return new MenuViewModel
            //    {
            //        Id = page.Id,
            //        Name = page.Name,
            //        DisplayName = page.Name
            //    };
            //}
            return page;
        }
        public async Task<IList<UserEntityPermissionViewModel>> GetUserPermissionList(string userId, string userRole, string permission)
        {
            string query = @$"SELECT po.""Name"" as Portal,po.""Id"" as PortalId,pa.""Title"" as Page,pa.""Id"" as PageId,ARRAY[''] as Permissions
	        FROM public.""User"" as u 
	        join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
	        join public.""Portal"" as po on po.""Id""=up.""PortalId"" and po.""IsDeleted""=false
	        join public.""Page"" as pa on pa.""PortalId""=up.""PortalId"" and ""AllowIfPortalAccess""=true
	        where u.""IsDeleted""=false and u.""Status""=1 and u.""Id""='{userId}'
	        union 
	        SELECT po.""Name"" as Portal,po.""Id"" as PortalId,pa.""Title"" as Page,pa.""Id"" as PageId,up.""Permissions"" as Permissions
	        FROM public.""User"" as u 
	        join public.""UserPermission"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
	        join public.""Page"" as pa on pa.""Id""=up.""PageId"" 
	        join public.""Portal"" as po on po.""Id""=pa.""PortalId"" 	
	        where u.""IsDeleted""=false and u.""Status""=1 and u.""Id""='{userId}'
            union
	        SELECT po.""Name"" as Portal,po.""Id"" as PortalId,pa.""Title"" as Page,pa.""Id"" as PageId,ARRAY[''] as Permissions
	        FROM public.""UserRole"" as u 
	        join public.""UserRolePortal"" as up on up.""UserRoleId""=u.""Id"" and up.""IsDeleted""=false
	        join public.""Portal"" as po on po.""Id""=up.""PortalId"" and po.""IsDeleted""=false
	        join public.""Page"" as pa on pa.""PortalId""=up.""PortalId"" and ""AllowIfPortalAccess""=true
	        where u.""IsDeleted""=false and u.""Status""=1 and u.""Id"" in ('{userRole}')
	        union 
	        SELECT po.""Name"" as Portal,po.""Id"" as PortalId,pa.""Title"" as Page,pa.""Id"" as PageId,up.""Permissions"" as Permissions
	        FROM public.""UserRole"" as u 
	        join public.""UserRolePermission"" as up on up.""UserRoleId""=u.""Id"" and up.""IsDeleted""=false
	        join public.""Page"" as pa on pa.""Id""=up.""PageId"" 
	        join public.""Portal"" as po on po.""Id""=pa.""PortalId"" 	
	        where u.""IsDeleted""=false and u.""Status""=1 and u.""Id"" in ('{userRole}')
                            
                            ";
            if (permission.IsNotNullAndNotEmpty())
            {
                query = @$"SELECT po.""Name"" as Portal,po.""Id"" as PortalId,pa.""Title"" as Page,pa.""Id"" as PageId,ARRAY[''] as Permissions
	        FROM public.""User"" as u 
	        join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
	        join public.""Portal"" as po on po.""Id""=up.""PortalId"" and po.""IsDeleted""=false
	        join public.""Page"" as pa on pa.""PortalId""=up.""PortalId"" and ""AllowIfPortalAccess""=true
	        where u.""IsDeleted""=false and u.""Status""=1 and u.""Id""='{userId}'	        
            union
	        SELECT po.""Name"" as Portal,po.""Id"" as PortalId,pa.""Title"" as Page,pa.""Id"" as PageId,ARRAY[''] as Permissions
	        FROM public.""UserRole"" as u 
	        join public.""UserRolePortal"" as up on up.""UserRoleId""=u.""Id"" and up.""IsDeleted""=false
	        join public.""Portal"" as po on po.""Id""=up.""PortalId"" and po.""IsDeleted""=false
	        join public.""Page"" as pa on pa.""PortalId""=up.""PortalId"" and ""AllowIfPortalAccess""=true
	        where u.""IsDeleted""=false and u.""Status""=1 and u.""Id"" in ('{userRole}')
	        union 
	        SELECT po.""Name"" as Portal,po.""Id"" as PortalId,pa.""Title"" as Page,pa.""Id"" as PageId,up.""Permissions"" as Permissions
	        FROM public.""UserRole"" as u 
	        join public.""UserRolePermission"" as up on up.""UserRoleId""=u.""Id"" and up.""IsDeleted""=false
	        join public.""Page"" as pa on pa.""Id""=up.""PageId"" 
	        join public.""Portal"" as po on po.""Id""=pa.""PortalId"" 	
	        where u.""IsDeleted""=false and u.""Status""=1 and u.""Id"" in ('{userRole}')";

            }
            var list = await _queryRepo.ExecuteQueryList<UserEntityPermissionViewModel>(query, null);
            return list;
        }
        public async Task<PageViewModel> GetUserRoleLandingPage(string userId, string portalId)
        {
            var query = @$"select pa.*
                from public.""Page"" as pa 
                join public.""UserRolePreference"" as up on pa.""Id""=up.""DefaultLandingPageId""  and ""up"".""IsDeleted""=false
                join public.""UserRoleUser"" as uru on up.""UserRoleId""=uru.""UserRoleId""  and ""uru"".""IsDeleted""=false
                where up.""PreferencePortalId""='{portalId}' and uru.""UserId""='{userId}' 
                and pa.""IsDeleted""=false limit 1";
            var page = await _queryRepo.ExecuteQuerySingle<PageViewModel>(query, null);
            //if (page != null)
            //{
            //    return new MenuViewModel
            //    {
            //        Id = page.Id,
            //        Name = page.Name,
            //        DisplayName = page.Name
            //    };
            //}
            return null;
        }

        public async Task<List<StepTaskEscalationViewModel>> GetStepTaskEscalation(string stepTaskComponentId)
        {
            string query = @$"SELECT st.*,  pst.""Name"" as ParentName,u.""Name"" as Assignee,nt.""Name"" as NotificationtemplateName,ent.""Name"" as EscalatedNotificationtemplateName
                            FROM public.""StepTaskEscalation"" as st
                            left join public.""StepTaskEscalation"" as pst on pst.""Id""=st.""ParentStepTaskEscalationId"" and pst.""IsDeleted""=false
                            left join public.""User"" as u on u.""Id""=st.""AssignedToUserId"" and u.""IsDeleted""=false
                            left join public.""NotificationTemplate"" as nt on nt.""Id""=st.""NotificationTemplateId"" and nt.""IsDeleted""=false
                            left join public.""NotificationTemplate"" as ent on ent.""Id""=st.""EscalatedToNotificationTemplateId"" and ent.""IsDeleted""=false
                            where st.""IsDeleted""=false and st.""StepTaskComponentId""='{stepTaskComponentId}'";
            var list = await _queryRepo.ExecuteQueryList<StepTaskEscalationViewModel>(query, null);
            return list;
        }
        public async Task<List<PositionChartIndexViewModel>> GetCMDBHierarchyData(string parentId, int levelUpto)
        {
            var query = $@"with recursive hrchy AS(
	         select f.""Id"" as ""Id"",null COLLATE ""tr-TR-x-icu"" as ""ParentId"",f.""Name"" as ""Name"",0 As level 
                from cms.""F_CSM_MASTER_CMDB_HIERARCHY"" as f where ""IsDeleted""=false 
			and f.""Id""='{parentId}'
             union all
	         select h.""Id"" as ""Id"",h.""ParentId"" as ""ParentId"",h.""Name"" as ""Name"",hrchy.level+ 1
            from cms.""F_CSM_MASTER_CMDB_HIERARCHY""  as h
	         join hrchy on h.""ParentId""=hrchy.""Id"" where h.""IsDeleted""=false 
	         )select hrchy.""Id"",hrchy.""ParentId"",hrchy.""Name"", 
            coalesce(dc.""DC"",0) as DirectChildCount from hrchy 
            left join 
                (
                    select h.""ParentId"" as ""Id"",count(h.""Id"") as ""DC""
                    from cms.""F_CSM_MASTER_CMDB_HIERARCHY"" as h								 
                    where h.""IsDeleted""=false 
                    group by h.""ParentId""
                )as dc on hrchy.""Id""=dc.""Id""
                where hrchy.""level""<={levelUpto}";
            //var parentqr = "";
            //if (parentId.IsNotNullAndNotEmpty())
            //{
            //    parentqr = $@"and f.""ParentId""='{parentId}'";
            //}
            //query = query.Replace("#PARENT#", parentqr);
            var list = await _queryRepo.ExecuteQueryList<PositionChartIndexViewModel>(query, null);
            return list;
        }
        public async Task<List<StepTaskEscalationViewModel>> GetTaskListWithEscalation()
        {
            string query = @$"SELECT ste.*,l.""Code"" as TaskStatusCode,t.""Id"" as TaskId,t.""ParentServiceId""
	FROM public.""StepTaskEscalation"" as ste
	left join public.""StepTaskComponent"" as stc on stc.""Id""=ste.""StepTaskComponentId""  and stc.""IsDeleted""=false
	left join public.""Component"" as c on c.""Id""=stc.""ComponentId""  and c.""IsDeleted""=false
	left join public.""NtsTask"" as t on t.""StepTaskComponentId""=stc.""Id"" and t.""IsDeleted""=false
	left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false
	where ste.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<StepTaskEscalationViewModel>(query, null);
            return list;
        }

        public async Task<List<PropertyViewModel>> GetPropertyData(string userId)
        {

            var Query = $@"
                        Select se.""Id"" as Id,propType.""Name"" as ProperyTypeName,ownerType.""Name"" as OwnerTypwName, 
                        se.""HouseNo"" as HouseNo, se.""Zone"" as Zone, se.""Colony"" as Colony,
                        se.""Country"" as Country, se.""PinCode"" as Pincode,se.""CreatedDate"" as CreatedDate, se.""Status"" as Status
                        from cms.""N_SNC_EGOV_PROP_MGMT_TAX_NEW_PROPERTY_JAMMU"" as se
                        --join public.""NtsService"" as s on s.""Id"" = se.""NtsServiceId""
                        join public.""LOV"" as propType on propType.""Id"" = se.""PropertyTypeId""
                        join public.""LOV"" as ownerType on ownerType.""Id"" = se.""OwnerShipType""
                        join public.""User"" as u on u.""Id"" = se.""CreatedBy""
                        where u.""Id"" = '{userId}'";
            return await _queryRepo.ExecuteQueryList<PropertyViewModel>(Query, null);
        }
        public async Task<List<StepTaskEscalationDataViewModel>> GetPortalTaskListWithEscalationData(string portalNames, string escalatUser)
        {
            var ptname = portalNames.Replace(",", "','");
            string query = @$"SELECT sted.*,s.""ServiceNo"",temp.""DisplayName"" as TemplateName,t.""TaskNo"",t.""TemplateCode"" as TaskTemplateCode,
t.""StartDate"",t.""DueDate"",l.""Name"" as TaskStatus,sted.""EscalatedDate"",temp.""Code"" as TemplateCode,t.""Id"" as TaskId
	FROM public.""StepTaskEscalationData"" as sted
	join public.""NtsService"" as s ON s.""Id"" = sted.""NtsServiceId"" and s.""IsDeleted""=false
	join public.""NtsTask"" as t ON t.""ParentServiceId"" = s.""Id"" and t.""IsDeleted""=false
	join public.""Template"" as temp on temp.""Id"" = s.""TemplateId"" and temp.""IsDeleted""=false
    join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false
    join public.""Portal"" as p on p.""Id"" = s.""PortalId"" and p.""IsDeleted""=false
	where sted.""EscalatedToUserId""='{escalatUser}'  and p.""Name"" in ({ptname})
    and sted.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<StepTaskEscalationDataViewModel>(query, null);
            return list;
        }

        public async Task<List<StepTaskEscalationDataViewModel>> MyTasksEscalatedDataList(string portalNames, string assigneeUser)
        {
            var ptname = portalNames.Replace(",", "','");
            string query = @$"SELECT sted.*,s.""ServiceNo"",temp.""DisplayName"" as TemplateName,t.""TaskNo"",t.""TemplateCode"" as TaskTemplateCode,
    t.""StartDate"",t.""DueDate"",l.""Name"" as TaskStatus,sted.""EscalatedDate"",temp.""Code"" as TemplateCode,t.""Id"" as TaskId
	FROM public.""StepTaskEscalationData"" as sted
	join public.""NtsService"" as s ON s.""Id"" = sted.""NtsServiceId"" and s.""IsDeleted""=false
	join public.""NtsTask"" as t ON t.""ParentServiceId"" = s.""Id"" and t.""IsDeleted""=false
	join public.""Template"" as temp on temp.""Id"" = s.""TemplateId"" and temp.""IsDeleted""=false
    join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false
    join public.""Portal"" as p on p.""Id""=s.""PortalId"" and p.""IsDeleted""=false
	where t.""AssignedToUserId""='{assigneeUser}' and p.""Name"" in ({ptname})
    and sted.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<StepTaskEscalationDataViewModel>(query, null);
            return list;
        }

        public async Task<List<StepTaskEscalationDataViewModel>> AllEscalatedTasks(string portalNames)
        {
            var ptname = portalNames.Replace(",", "','");
            string query = @$"SELECT sted.*,s.""ServiceSubject"" as ServiceName,u.""Name"" as RequestedBy,s.""ServiceNo"",temp.""DisplayName"" as TemplateName,t.""TaskNo"",t.""TemplateCode"" as TaskTemplateCode,
    t.""StartDate"",t.""DueDate"",l.""Name"" as TaskStatus,sted.""EscalatedDate"",
	temp.""Code"" as TemplateCode,t.""Id"" as TaskId,tc.""Name"" as CategoryName,t.""TaskSubject"",
	tu.""Name"" as TaskAssignee,eu.""Name"" as EscalatedToUserName,sl.""Name"" as ServiceStatus,s.""CreatedDate"" as ServiceCreatedDate
	FROM public.""StepTaskEscalationData"" as sted
	join public.""NtsService"" as s ON s.""Id"" = sted.""NtsServiceId"" and s.""IsDeleted""=false
	join public.""NtsTask"" as t ON t.""ParentServiceId"" = s.""Id"" and t.""IsDeleted""=false
	join public.""Template"" as temp on temp.""Id"" = s.""TemplateId"" and temp.""IsDeleted""=false
	join public.""TemplateCategory"" as tc on tc.""Id"" = temp.""TemplateCategoryId"" and tc.""IsDeleted""=false
    join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false
    join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false
	join public.""User"" as u on u.""Id"" = s.""RequestedByUserId"" and u.""IsDeleted""=false
	join public.""User"" as tu on tu.""Id"" = t.""AssignedToUserId"" and tu.""IsDeleted""=false
	join public.""User"" as eu on eu.""Id"" = sted.""EscalatedToUserId"" and eu.""IsDeleted""=false
	join public.""Portal"" as p on p.""Id"" = s.""PortalId"" and p.""IsDeleted""=false
	where p.""Name"" in ({ptname}) and
     sted.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<StepTaskEscalationDataViewModel>(query, null);
            return list;
        }

        public async Task<List<StepTaskEscalationDataViewModel>> EscalatedTaskDataByTaskId(string taskId)
        {
            string query = @$"SELECT sted.*
	FROM public.""StepTaskEscalationData"" as sted
	join public.""NtsTask"" as t ON t.""Id"" = sted.""NtsTaskId"" and t.""IsDeleted""=false	
	where sted.""NtsTaskId""='{taskId}' and sted.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<StepTaskEscalationDataViewModel>(query, null);
            return list;
        }

        public async Task<List<StepTaskEscalationViewModel>> GetOverDueTaskListForEscalation()
        {
            string query = @$"SELECT ste.*,t.""ParentServiceId"",t.""Id"" as TaskId
	        FROM public.""NtsTask"" as t
	         join public.""StepTaskComponent"" as stc on stc.""Id""=t.""StepTaskComponentId""  and stc.""IsDeleted""=false
	         join public.""Component"" as c on c.""Id""=stc.""ComponentId""  and c.""IsDeleted""=false
             join public.""StepTaskEscalation"" as ste on ste.""StepTaskComponentId""=stc.""Id""  and ste.""IsDeleted""=false
	         join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false
	        where t.""IsDeleted""=false and  l.""Code""='TASK_STATUS_OVERDUE' 
            and DATE_PART('day', '{DateTime.Now.ToDatabaseDateFormat()}'-(t.""DueDate"" + make_interval(days=>ste.""TriggerDaysAfterOverDue""))) between 0 and 10";
            var list = await _queryRepo.ExecuteQueryList<StepTaskEscalationViewModel>(query, null);
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetTriggeringStepTaskComponentList(string templateId)
        {
            string query = $@"select stc.""Id"",stct.""DisplayName"" as Name 
from public.""Template"" as t
join public.""ProcessDesign"" as pd on t.""Id""=pd.""TemplateId"" and pd.""IsDeleted""=false
join public.""Component"" as c on pd.""Id""=c.""ProcessDesignId"" and c.""IsDeleted""=false
join public.""StepTaskComponent"" as stc on c.""Id""=stc.""ComponentId"" and stc.""IsDeleted""=false and stc.""IsRuntimeComponent""=true
join public.""Template"" as stct on stc.""TemplateId""=stct.""Id"" and stct.""IsDeleted""=false
where t.""Id""='{templateId}' and t.""IsDeleted""=false";

            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<FormTemplateViewModel> GetSelectQueryData(TableMetadataViewModel tableMetaData)
        {
            var formTemlateViewModel = await _queryRepo.ExecuteQuerySingle<FormTemplateViewModel>
        (@$"select nt.* from public.""FormTemplate"" nt 
                join public.""Template"" t on nt.""TemplateId""=t.""Id"" and t.""IsDeleted""=false 
                where nt.""IsDeleted""=false and t.""TableMetadataId""='{tableMetaData.Id}' ", null);
            return formTemlateViewModel;

        }
        public async Task<List<RuntimeWorkflowDataViewModel>> GetRuntimeWorkflowDataList(string runtimeWorkflowDataId)
        {
            string query = $@"select wfd.*,l.""Code"" as AssignedToTypeCode,l.""Name"" as AssignedToTypeName,u.""Name"" as AssignedToUserName,t.""Name"" as AssignedToTeamName,
h.""Name"" as AssignedToHierarchyMasterName,
case when wfd.""AssignedToHierarchyMasterLevelId""=1 then hl.""Level1Name""
when wfd.""AssignedToHierarchyMasterLevelId""=2 then hl.""Level2Name""
when wfd.""AssignedToHierarchyMasterLevelId""=3 then hl.""Level3Name""
when wfd.""AssignedToHierarchyMasterLevelId""=4 then hl.""Level4Name""
when wfd.""AssignedToHierarchyMasterLevelId""=5 then hl.""Level5Name""
end as AssignedToHierarchyMasterLevelName
from public.""RuntimeWorkflowData"" as wfd
left join public.""LOV"" as l on wfd.""AssignedToTypeId""=l.""Id"" and l.""IsDeleted""=false
left join public.""User"" as u on wfd.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false
left join public.""Team"" as t on wfd.""AssignedToTeamId""=t.""Id"" and t.""IsDeleted""=false
left join public.""HierarchyMaster"" as h on wfd.""AssignedToHierarchyMasterId""=h.""Id"" and h.""IsDeleted""=false
left join public.""HierarchyMaster"" as hl on h.""Id""=hl.""Id"" and hl.""IsDeleted""=false
where wfd.""RuntimeWorkflowId""='{runtimeWorkflowDataId}' and wfd.""IsDeleted""=false order by wfd.""SequenceOrder"" ";

            var list = await _queryRepo.ExecuteQueryList<RuntimeWorkflowDataViewModel>(query, null);
            return list;

        }

        public async Task<List<LegalEntityViewModel>> GetLegalEntityByLocationData()
        {
            string query = @$"SELECT l.*,c.""CityName"",s.""StateName""
                            FROM public.""LegalEntity"" as l
                            left join cms.""F_BLS_MASTER_BLS_City"" as c on l.""CityName""=c.""Id"" and c.""IsDeleted""=false
                            left join cms.""F_BLS_MASTER_BLS_State"" as s on l.""StateName""=s.""Id"" and s.""IsDeleted""=false
                            where l.""IsDeleted"" = false and l.""PortalId""='{_userContext.PortalId}' ";
            var list = await _queryRepo.ExecuteQueryList<LegalEntityViewModel>(query, null);
            return list;
        }

        public async Task<string> GetForeignKeyId(ColumnMetadataViewModel col, string val)
        {
            string query = $@"Select ""Id"" from ""{col.ForeignKeyTableSchemaName}"".""{col.ForeignKeyTableName}"" where ""{col.ForeignKeyDisplayColumnName}""='{val}' ";
            var res = await _queryRepo.ExecuteScalar<string>(query, null);
            return res;
        }

        public async Task<List<MenuGroupViewModel>> GetParentMenuGroups(List<string> list)
        {
            var ids = string.Join("','", list);
            var query = $@"Select * from public.""MenuGroup""  where  ""Id"" in(select ""ParentId"" from public.""MenuGroup"" where  ""Id"" in ('{ids}'))";
            var res = await _queryRepo.ExecuteQueryList<MenuGroupViewModel>(query, null);
            res = res.Where(x => list.All(y => y != x.Id)).ToList();
            return res;
            //  throw new NotImplementedException();
        }
    }
}


