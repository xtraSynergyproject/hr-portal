using AutoMapper;
using MySql.Data.MySqlClient;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
////using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class NtsQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, INtsQueryBusiness
    {
        private readonly IUserContext _userContext;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<NtsLogViewModel> _queryNtsLog;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryDashboardCalendar;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly IServiceProvider _serviceProvider;

        public NtsQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext userContext, IRepositoryQueryBase<NtsLogViewModel> queryNtsLog
            , IRepositoryQueryBase<NoteViewModel> queryRepo
            , IRepositoryQueryBase<DashboardCalendarViewModel> queryDashboardCalendar,
            ITemplateCategoryBusiness templateCategoryBusiness
            , IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _userContext = userContext;
            _queryRepo = queryRepo;
            _queryNtsLog = queryNtsLog;
            _queryDashboardCalendar = queryDashboardCalendar;
            _templateCategoryBusiness = templateCategoryBusiness;
            _serviceProvider = serviceProvider;
        }

        #region ServiceBusiness
        public async Task<IList<NTSMessageViewModel>> GetServiceAttachedReplies(string userId, string serviceId)
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
            from public.""NtsServiceComment"" as ""C""  
            left join public.""User"" as ""CBU"" on ""C"".""CommentedByUserId""=""CBU"".""Id"" and ""CBU"".""IsDeleted""=false 
            left join public.""User"" as ""CTU"" on ""C"".""CommentToUserId""=""CTU"".""Id"" and ""CTU"".""IsDeleted""=false 
            where ""C"".""NtsServiceId""='{serviceId}' and ""C"".""IsDeleted""=false ";


            var comments = await _queryRepo.ExecuteQueryList<NTSMessageViewModel>(query, null);
            return comments;
        }

        public async Task UpdateBookSequenceOrder(string parentServiceId, long? sequenceOrder, string serviceId, string type)
        {
            if (type == "Service")
            {
                var query = $@"Update public.""NtsService"" set ""SequenceOrder""=(""SequenceOrder""+1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false and ""Id""<> '{serviceId}'";
                await _queryDashboardCalendar.ExecuteCommand(query, null);
            }
            if (type == "Note")
            {
                var query1 = $@"Update public.""NtsNote"" set ""SequenceOrder""=(""SequenceOrder""+1) where ""ParentNoteId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
                await _queryDashboardCalendar.ExecuteCommand(query1, null);
            }

            //var query2 = $@"Update public.""NtsTask"" set ""SequenceOrder""=(""SequenceOrder""+1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
            //await _queryCal.ExecuteCommand(query2, null);
        }

        public async Task UpdateCategorySequenceOrderOnDelete(string parentServiceId, long? sequenceOrder, string serviceId, string TemplateCode)
        {
            var query = $@"Update public.""NtsService"" set ""SequenceOrder""=(""SequenceOrder""-1) where  ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=true and ""Id""<>'{serviceId}' and ""TemplateCode""='{TemplateCode}'";
            await _queryDashboardCalendar.ExecuteCommand(query, null);
        }
        public async Task UpdateBookSequenceOrderOnDelete(string parentServiceId, long? sequenceOrder, string serviceId)
        {
            var query = $@"Update public.""NtsService"" set ""SequenceOrder""=(""SequenceOrder""-1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=true and ""Id""<>'{serviceId}'";
            await _queryDashboardCalendar.ExecuteCommand(query, null);
            //var query1 = $@"Update public.""NtsNote"" set ""SequenceOrder""=(""SequenceOrder""-1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
            //await _queryCal.ExecuteCommand(query1, null);
            //var query2 = $@"Update public.""NtsTask"" set ""SequenceOrder""=(""SequenceOrder""-1) where ""ParentServiceId""='{parentServiceId}' and ""SequenceOrder"">='{sequenceOrder}' and ""IsDeleted""=false ";
            //await _queryCal.ExecuteCommand(query2, null);
        }
        public async Task RollBackServiceData(TableMetadataViewModel table, ServiceTemplateViewModel model)
        {
            var query = @$" delete from {table.Schema}.""{ table.Name}"" where ""Id""='{model.UdfNoteTableId}';
                    delete from public.""NtsService"" where ""Id""='{model.ServiceId}';
                    delete from public.""NtsNote"" where ""Id""='{model.UdfNoteId}'; ";

            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<TableMetadataViewModel> GetTableMetadataByServiceId(string serviceId)
        {
            var query = @$"select ""TM"".* from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""UdfTableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsService"" as ""S"" on ""T"".""Id""=""S"".""TemplateId"" and ""S"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false  and ""S"".""Id""='{serviceId}'";

            var tableMetadata = await _queryRepo.ExecuteQuerySingle<TableMetadataViewModel>(query, null);
            return tableMetadata;
        }
        public async Task<ServiceTemplateViewModel> GetSelectQueryServiceTemplateData(string selectQuery)
        {
            var data = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(selectQuery, null);
            return data;
        }
        public async Task<DataTable> GetSelectQueryServiceTemplateDataTable(string selectQuery)
        {
            var datatable = await _queryRepo.ExecuteQueryDataTable(selectQuery, null);
            return datatable;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableColumnsPrimaryKeyColumns(TableMetadataViewModel tableMetaData)
        {
            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
                where 
                (
                    (t.""Schema""='{tableMetaData.Schema}' and t.""Name""='{tableMetaData.Name}' and (c.""IsUdfColumn""=true )) or
                    (t.""Schema""='public' and t.""Name""='ServiceTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsService')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false ";
            var columndata = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return columndata;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableColumnsForeignKeyColumns(TableMetadataViewModel tableMetaData)
        {
            var query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
                ,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
                ,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false 
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false 
                where c.""TableMetadataId""='{tableMetaData.Id}'
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  and c.""IsUdfColumn""=true 
                and (fc.""IsSystemColumn""=false or fc.""IsPrimaryKey""=true)";

            var columndata = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return columndata;
        }
        public async Task<string> GenerateNextServiceNo(ServiceTemplateViewModel model)
        {
            if (model.NumberGenerationType != NumberGenerationTypeEnum.SystemGenerated)
            {
                return "";
            }
            string prefix = "S";
            var today = DateTime.Today;
            var nextId = await GetNextServiceNo(today);
            //var query = $@"update public.""NtsServiceSequence"" SET ""NextId"" = ""NextId""+1,
            //""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            //WHERE ""SequenceDate"" = '{today.ToDatabaseDateFormat()}'
            //RETURNING ""NextId""-1";
            //var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            //if (nextId == null)
            //{
            //    nextId = 1;
            //    await _repo.Create<NtsServiceSequence, NtsServiceSequence>(
            //        new NtsServiceSequence
            //        {
            //            SequenceDate = today,
            //            NextId = 2,
            //            CreatedBy = _repo.UserContext.UserId,
            //            CreatedDate = DateTime.Now,
            //            LastUpdatedBy = _repo.UserContext.UserId,
            //            LastUpdatedDate = DateTime.Now,
            //            Status = StatusEnum.Active,
            //        });
            //}

            return $"{prefix}-{today.ToSequenceNumberFormat()}-{nextId}";
        }
        public async Task<string> GetNextServiceNo(DateTime asofDate, string templateId = null)
        {
            
            var query = $@"update public.""NtsServiceSequence"" SET ""NextId"" = ""NextId""+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""SequenceDate"" = '{asofDate.ToDatabaseDateFormat()}' <<templateWhere>>
            RETURNING ""NextId""-1";
            var templateWhere = "";
            if (templateId.IsNotNullAndNotEmpty())
            {
                templateWhere = $@" and ""TemplateId""='{templateId}' ";
            }
            else
            {
                templateWhere = $@" and ""TemplateId"" is null ";
            }
            query = query.Replace("<<templateWhere>>", templateWhere);
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            if (nextId == null)
            {
                nextId = 1;
                await _repo.Create<NtsServiceSequence, NtsServiceSequence>(
                    new NtsServiceSequence
                    {
                        SequenceDate = asofDate,
                        NextId = 2,
                        CreatedBy = _repo.UserContext.UserId,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = _repo.UserContext.UserId,
                        LastUpdatedDate = DateTime.Now,
                        Status = StatusEnum.Active,
                        TemplateId =templateId
                    });
            }

            return Convert.ToString(nextId);
        }
        public async Task<string> GetNextGrievanceServiceNo(int year,string department,string ward)
        {
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
            var query = $@"update cms.""F_JSC_MASTER_COMPLAINTS_SEQUENCE"" SET ""NextId"" = ""NextId""::int+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'            
            RETURNING ""NextId""::int-1";
           
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            if (nextId == null)
            {
                nextId = 1;
                var formTempModel = new FormTemplateViewModel();
                formTempModel.DataAction = DataActionEnum.Create;
                formTempModel.TemplateCode = "COMPLAINTS_SEQUENCE";
                var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("DepartmentId", department);
                ((IDictionary<String, Object>)exo).Add("WardId", ward);
                ((IDictionary<String, Object>)exo).Add("Year", year);
                ((IDictionary<String, Object>)exo).Add("NextId", nextId);
                formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _cmsBusiness.ManageForm(formmodel);
            }

            return Convert.ToString(nextId);
        }
        public async Task<string> GetNextBLSFileNumber(int year)
        {
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
            var query = $@"update cms.""F_BLS_MASTER_FILENUMBER_SEQUENCE"" SET ""NextId"" = ""NextId""::int+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""Year"" = '{year}' 
            RETURNING ""NextId""::int-1";

            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            if (nextId == null)
            {
                nextId = 1;
                var formTempModel = new FormTemplateViewModel();
                formTempModel.DataAction = DataActionEnum.Create;
                formTempModel.TemplateCode = "FILENUMBER_SEQUENCE";
                var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("Year", year);
                ((IDictionary<String, Object>)exo).Add("NextId", nextId);
                formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _cmsBusiness.ManageForm(formmodel);
            }

            return Convert.ToString(nextId);
        }
        public async Task<ServiceTemplateViewModel> GetServiceTemplateForNewService(ServiceTemplateViewModel vm)
        {
            var query = @$"select ""TT"".*,
            ""NT"".""Json"" as ""Json"",
            ""T"".""UdfTemplateId"" as ""UdfTemplateId"",
            ""T"".""ViewType"" as ""ViewType"",
            ""T"".""Id"" as ""TemplateId"",
            ""T"".""TableMetadataId"" as ""TableMetadataId"",
            ""T"".""UdfTableMetadataId"" as ""UdfTableMetadataId"",
            ""T"".""Name"" as ""TemplateName"",
            ""T"".""DisplayName"" as ""TemplateDisplayName"",
            ""T"".""Code"" as ""TemplateCode"",
            ""TC"".""Id"" as ""TemplateCategoryId"",
            ""TC"".""Code"" as ""TemplateCategoryCode"",
            ""TC"".""Name"" as ""TemplateCategoryName"",
            ""TC"".""TemplateType"" as ""TemplateType"" ,
            ""TS"".""Id"" as ""ServiceStatusId"" ,
            ""TS"".""Code"" as ""ServiceStatusCode"" ,
            ""TS"".""Name"" as ""ServiceStatusName"" ,
            ""TA"".""Id"" as ""ServiceActionId"" ,
            ""TA"".""Code"" as ""ServiceActionCode"" ,
            ""TA"".""Name"" as ""ServiceActionName"" ,
            ""TP"".""Id"" as ""ServicePriorityId"" ,
            ""TP"".""Code"" as ""ServicePriorityCode"" ,
            ""TP"".""Name"" as ""ServicePriorityName"" 
            from public.""Template"" as ""T""
            join public.""Template"" as ""NT"" on ""T"".""UdfTemplateId""=""NT"".""Id"" and ""NT"".""IsDeleted""=false 
            join public.""TemplateCategory"" as ""TC"" on ""T"".""TemplateCategoryId""=""TC"".""Id"" and ""TC"".""IsDeleted""=false 
            join public.""ServiceTemplate"" as ""TT"" on ""TT"".""TemplateId""=""T"".""Id"" and ""TT"".""IsDeleted""=false 
            join public.""LOV"" as ""TS"" on ""TS"".""LOVType""='LOV_SERVICE_STATUS' and ""TS"".""Code""='SERVICE_STATUS_DRAFT'
            and ""TS"".""IsDeleted""=false 
            join public.""LOV"" as ""TA"" on ""TA"".""LOVType""='LOV_SERVICE_ACTION' and ""TA"".""Code""='SERVICE_ACTION_DRAFT' 
            and ""TA"".""IsDeleted""=false 
            join public.""LOV"" as ""TP"" on ""TP"".""LOVType""='SERVICE_PRIORITY' and ""TP"".""IsDeleted""=false 
            and (""TP"".""Id""=""TT"".""PriorityId"" or (""TT"".""PriorityId"" is null and ""TP"".""Code""='SERVICE_PRIORITY_MEDIUM'))
            where ""T"".""IsDeleted""=false  and (""T"".""Id""='{vm.TemplateId}' or ""T"".""Code""='{vm.TemplateCode}')";
            var data = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            if (data != null)
            {
                data.ColumnList = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == data.UdfTableMetadataId
                && x.IsVirtualColumn == false && x.IsHiddenColumn == false && x.IsLogColumn == false && x.IsPrimaryKey == false);
            }
            return data;
        }
        public async Task<List<UserViewModel>> SetSharedList(ServiceTemplateViewModel model)
        {
            string query = @$"select n.""Id"" as Id,u.""Name"" as UserName,u.""PhotoId"" as PhotoId,u.""Email"" as Email
                              from public.""NtsServiceShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false 
                              
                              where n.""NtsServiceId""='{model.ServiceId}' and n.""IsDeleted""=false 
                                union select n.""Id"" as Id,t.""Name"" as UserName, t.""LogoId"" as PhotoId,t.""Name"" as Email
                              from public.""NtsServiceShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsServiceId""='{model.ServiceId}' and n.""IsDeleted""=false ";
            var userlist = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return userlist;
            //new List<UserViewModel> { { new UserViewModel { UserName = "Shafi", Email = "shaficmd@gmail.com" } } ,
            //{ new UserViewModel { UserName = "Shafi2", Email = "shaficmd@gmail.com2" } } };
        }
        public async Task DeleteService(ServiceTemplateViewModel model, TableMetadataViewModel tableMetaData)
        {
            var selectQuery = new StringBuilder(@$"update {ApplicationConstant.Database.Schema.Cms}.""{tableMetaData.Name}"" set ");
            selectQuery.Append(Environment.NewLine);
            selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
            selectQuery.Append(@$"where ""NtsNoteId""='{model.UdfNoteId}';{Environment.NewLine}");
            selectQuery.Append(@$"update public.""NtsNote"" set ");
            selectQuery.Append(@$"""IsDeleted""=true,{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',{Environment.NewLine}");
            selectQuery.Append(@$"""LastUpdatedBy""='{_repo.UserContext.UserId}'{Environment.NewLine}");
            selectQuery.Append(@$"where ""Id""='{model.UdfNoteId}'");

            var queryText = selectQuery.ToString();

            await _queryRepo.ExecuteCommand(queryText, null);

        }
        public async Task<List<ServiceTemplateViewModel>> GetServiceIndexPageCount(ServiceIndexPageTemplateViewModel model, PageViewModel page)
        {
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode"",count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false  
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedWithUserId"",l.""Code""";

            var udfjoin = "";
            var template = await _repo.GetSingleById<TemplateViewModel, Template>(page.TemplateId);
            if (template.IsNotNull())
            {
                var udfTableMetadataId = template.UdfTableMetadataId;
                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(udfTableMetadataId);
                if (tableMetadata.IsNotNull())
                {
                    udfjoin = $@"join {tableMetadata.Schema}.""{tableMetadata.Name}"" as udf on udf.""NtsNoteId""=n.""Id"" and udf.""IsDeleted""=false";
                }
            }

            query = query.Replace("#UDFJOIN#", udfjoin);
            var result = await _queryRepo.ExecuteQueryList<ServiceTemplateViewModel>(query, null);
            return result;
        }
        public async Task<IList<ServiceViewModel>> GetNtsServiceIndexPageGridData(NtsActiveUserTypeEnum ownerType, string serviceStatusCode, string categoryCode, string templateCode, string moduleCode)
        {

            var query = @$" Select t.*,tmpc.""Code"" as ""TemplateCategoryCode"",ou.""Name"" as ""OwnerUserName"",ru.""Name"" as ""RequestedByUserName""
                        ,cu.""Name"" as ""CreatedByUserName"",lu.""Name"" as ""LastUpdatedByUserName""
                        , tlov.""Name"" as ""ServiceStatusName"",tlov.""Code"" as ""ServiceStatusCode"",m.""Name"" as Module
from public.""NtsService"" as t
                        left join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false
                         left join public.""Module"" as m ON m.""Id""=tmp.""ModuleId"" and m.""IsDeleted""=false 
left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
                        left join public.""LOV"" as tlov on t.""ServiceStatusId""=tlov.""Id"" and tlov.""IsDeleted""=false 
                       
                        left join public.""User"" as ou ON ou.""Id""=t.""OwnerUserId"" and ou.""IsDeleted""=false 
                        left join public.""User"" as ru ON ru.""Id""=t.""RequestedByUserId"" and ru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=t.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=t.""LastUpdatedBy"" and lu.""IsDeleted""=false 
                        #WHERE# 
                        ORDER BY t.""LastUpdatedDate"" DESC ";
            var where = @$" WHERE 1=1 ";
            switch (ownerType)
            {
                case NtsActiveUserTypeEnum.Assignee:
                    where += @$" and t.""AssignedToUserId""='{_repo.UserContext.UserId}'";
                    break;
                case NtsActiveUserTypeEnum.OwnerOrRequester:
                    where += @$" and (t.""OwnerUserId""='{_repo.UserContext.UserId}' or t.""RequestedByUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.Requester:
                    where += @$" and (t.""OwnerUserId""<>'{_repo.UserContext.UserId}' and t.""RequestedByUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.Owner:
                    where += @$" and (t.""OwnerUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.SharedWith:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsServiceId"" from public.""NtsServiceShared"" as ts where ts.""IsDeleted""=false and ts.""SharedWithUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                case NtsActiveUserTypeEnum.SharedBy:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsServiceId"" from public.""NtsServiceShared"" as ts where ts.""IsDeleted""=false  and ts.""SharedByUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                default:
                    break;
            }
            if (serviceStatusCode.IsNotNullAndNotEmpty())
            {
                var servicestatus = $@" and tlov.""Code""='{serviceStatusCode}' ";
                where = where + servicestatus;
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
            var querydata = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return querydata;
        }
        public async Task<List<ServiceTemplateViewModel>> GetNtsServiceIndexPageCount(NtsServiceIndexPageViewModel model)
        {
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode"",count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" 
            join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",l.""Code"" 
            
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
            join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  ts.""SharedByUserId"",l.""Code"" 
            
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""ServiceStatusCode""
            ,count(distinct t.""Id"") as ""ServiceCount""      
            from public.""NtsService"" as t
            join public.""NtsServiceShared"" as ts on t.""Id""=ts.""NtsServiceId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""ServiceStatusId"" and l.""IsDeleted""=false 
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
            var result = await _queryRepo.ExecuteQueryList<ServiceTemplateViewModel>(query, null);
            return result;
        }
        public async Task<List<ServiceViewModel>> GetWorklistDashboardCountServiceGoal(string userId, string moduleCodes = null, string templateCategoryCode = null, string taskTemplateIds = null, string serviceTemplateIds = null)
        {
            var cypher = $@"Select ns.""Code"" as ServiceStatusCode,u.""Id"" as OwnerUserId
                            from public.""NtsService"" as s                         
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false  
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false 
                            join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
                           left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #SERVICETEMPLATESEARCH#";
            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            var Templatesearch = "";

            if (serviceTemplateIds.IsNotNullAndNotEmpty())
            {
                serviceTemplateIds = serviceTemplateIds.Replace(",", "','");
                Templatesearch = $@" and tr.""Id"" in ('{serviceTemplateIds}')";
            }
            var taskTemplatesearch = "";
            if (taskTemplateIds.IsNotNullAndNotEmpty())
            {
                taskTemplateIds = taskTemplateIds.Replace(",", "','");
                taskTemplatesearch = $@" and tr.""Id"" in ('{taskTemplateIds}')";
            }
            cypher = cypher.Replace("#WHERE#", search);
            cypher = cypher.Replace("#SERVICETEMPLATESEARCH#", Templatesearch);

            var goalservice = await _queryRepo.ExecuteQueryList<ServiceViewModel>(cypher, null);
            return goalservice;
        }
        public async Task<List<ServiceViewModel>> GetWorklistDashboardCountServiceShare(string userId, string moduleCodes = null, string templateCategoryCode = null, string taskTemplateIds = null, string serviceTemplateIds = null)
        {
            var cypher1 = $@"Select ns.""Code"" as ServiceStatusCode
                from public.""NtsService"" as s
                join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and s.""IsDeleted""=false and ss.""IsDeleted""=false 
                join public.""LOV"" as sst on sst.""Id""=ss.""ServiceSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_USER' 
                join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""IsDeleted""=false 
                join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false  
                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
                left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false   where u.""Id""='{userId}' and s.""PortalId""='{_userContext.PortalId}' #WHERE# #SERVICETEMPLATESEARCH#
                
                union
                Select ns.""Code"" as ServiceStatusCode
                from public.""User"" as u
                join public.""TeamUser"" as tu on tu.""UserId""=u.""Id"" and u.""IsDeleted""=false and tu.""IsDeleted""=false  
                join public.""NtsServiceShared"" as ss on ss.""SharedWithTeamId""=tu.""TeamId"" and ss.""IsDeleted""=false 
                join public.""LOV"" as sst on sst.""Id""=ss.""ServiceSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_TEAM' 
                join public.""NtsService"" as s on s.""Id""=ss.""NtsServiceId"" and s.""IsDeleted""=false 
                join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false 
                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""IsDeleted""=false
                join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
                left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where u.""Id""='{userId}' and s.""PortalId""='{_userContext.PortalId}' #WHERE# #SERVICETEMPLATESEARCH# ";

            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            var Templatesearch = "";

            if (serviceTemplateIds.IsNotNullAndNotEmpty())
            {
                serviceTemplateIds = serviceTemplateIds.Replace(",", "','");
                Templatesearch = $@" and tr.""Id"" in ('{serviceTemplateIds}')";
            }
            var taskTemplatesearch = "";
            if (taskTemplateIds.IsNotNullAndNotEmpty())
            {
                taskTemplateIds = taskTemplateIds.Replace(",", "','");
                taskTemplatesearch = $@" and tr.""Id"" in ('{taskTemplateIds}')";
            }
            cypher1 = cypher1.Replace("#WHERE#", search);
            cypher1 = cypher1.Replace("#SERVICETEMPLATESEARCH#", Templatesearch);

            var serviceShare = await _queryRepo.ExecuteQueryList<ServiceViewModel>(cypher1, null);
            return serviceShare;
        }
        public async Task<List<TaskViewModel>> GetWorklistDashboardCountTask(string userId, string moduleCodes = null, string templateCategoryCode = null, string taskTemplateIds = null, string serviceTemplateIds = null)
        {
            var cypher2 = $@"Select ns.""Code"" as TaskStatusCode,u.""Id"" as OwnerUserId,ua.""Id"" as AssignedToUserId
                            from public.""NtsTask"" as s
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false  
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false
							left join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
                            left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #TASKTEMPLATESEARCH#
                           ";

            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            var Templatesearch = "";

            if (serviceTemplateIds.IsNotNullAndNotEmpty())
            {
                serviceTemplateIds = serviceTemplateIds.Replace(",", "','");
                Templatesearch = $@" and tr.""Id"" in ('{serviceTemplateIds}')";
            }
            var taskTemplatesearch = "";
            if (taskTemplateIds.IsNotNullAndNotEmpty())
            {
                taskTemplateIds = taskTemplateIds.Replace(",", "','");
                taskTemplatesearch = $@" and tr.""Id"" in ('{taskTemplateIds}')";
            }
            cypher2 = cypher2.Replace("#WHERE#", search);
            cypher2 = cypher2.Replace("#TASKTEMPLATESEARCH#", taskTemplatesearch);

            var tasklist = await _queryRepo.ExecuteQueryList<TaskViewModel>(cypher2, null);
            return tasklist;
        }
        public async Task<List<TaskViewModel>> GetWorklistDashboardCountTaskShare(string userId, string moduleCodes = null, string templateCategoryCode = null, string taskTemplateIds = null, string serviceTemplateIds = null)
        {
            var cypher3 = $@"Select ns.""Code"" as TaskStatusCode,s.""Id""
                from public.""NtsTask"" as s
                join public.""NtsTaskShared"" as ss on ss.""NtsTaskId""=s.""Id""  and s.""IsDeleted""=false  and ss.""IsDeleted""=false 
                join public.""LOV"" as sst on sst.""Id""=ss.""TaskSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_USER' 
                join public.""User"" as u on u.""Id""=ss.""SharedWithUserId""  and u.""IsDeleted""=false and u.""Id""='{userId}' 
                join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
                left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #TASKTEMPLATESEARCH# 
                
                union
                Select ns.""Code"" as TaskStatusCode,s.""Id""
                from public.""User"" as u
                join public.""TeamUser"" as tu on tu.""UserId""=u.""Id""  and tu.""IsDeleted""=false  and u.""IsDeleted""=false and u.""Id""='{userId}' 
                join public.""NtsTaskShared"" as ss on ss.""SharedWithTeamId""=tu.""TeamId""   and ss.""IsDeleted""=false 
                join public.""LOV"" as sst on sst.""Id""=ss.""TaskSharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_TEAM' 
                join public.""NtsTask"" as s on s.""Id""=ss.""NtsTaskId""  and s.""IsDeleted""=false 
                join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
                left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 and s.""PortalId""='{_userContext.PortalId}' #WHERE# #TASKTEMPLATESEARCH# ";

            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            var Templatesearch = "";

            if (serviceTemplateIds.IsNotNullAndNotEmpty())
            {
                serviceTemplateIds = serviceTemplateIds.Replace(",", "','");
                Templatesearch = $@" and tr.""Id"" in ('{serviceTemplateIds}')";
            }
            var taskTemplatesearch = "";
            if (taskTemplateIds.IsNotNullAndNotEmpty())
            {
                taskTemplateIds = taskTemplateIds.Replace(",", "','");
                taskTemplatesearch = $@" and tr.""Id"" in ('{taskTemplateIds}')";
            }
            cypher3 = cypher3.Replace("#WHERE#", search);
            cypher3 = cypher3.Replace("#TASKTEMPLATESEARCH#", taskTemplatesearch);

            var taskShare = await _queryRepo.ExecuteQueryList<TaskViewModel>(cypher3, null);
            return taskShare;
        }
        public async Task<List<ServiceViewModel>> GetSearchResult(ServiceSearchViewModel searchModel)
        {
            var userId = searchModel.UserId;
            var cypher = $@"select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
                 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
                 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
                 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
                ,'Owner' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
                from
                public.""NtsService"" as n
                    join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false and n.""IsDeleted""=false 
                  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL#
                                    union
                                   select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
                 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
                 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
                 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
                ,'Requester' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
                from
                public.""NtsService"" as n
                join public.""User"" as nru on n.""RequestedByUserId""=nru.""Id""	 and nru.""Id""='{userId}' and n.""IsDeleted""=false 
                  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                 left  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL#
                                    union
                                                     select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
                 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
                 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
                 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
                ,'Shared' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
                from
                public.""NtsService"" as n
                join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=n.""Id"" and ss.""IsDeleted""=false and n.""IsDeleted""=false 
                join public.""LOV"" as sw on  sw.""LOVType""='SHARED_TYPE' and sw.""Code""='SHARED_TYPE_USER' and ss.""ServiceSharedWithTypeId""=sw.""Id"" 
                join public.""User"" as nsu on ss.""SharedWithUserId"" = nsu.""Id"" and nsu.""IsDeleted""=false  and nsu.""Id""='{userId}'  
                  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                 left  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                left join public.""User"" as nru on n.""RequestedByUserId""=nru.""Id""
                    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL#
                                    union
                                   select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
                 ns.""Name"" as ServiceStatusName,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceSubject,nou.""Name"" as OwnerUserUserName,
                 nou.""Id"" as OwnerUserId,n.""ServiceSLA"" as ServiceSLA,n.""StartDate"" as StartDate,n.""DueDate"" as DueDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate, n.""ClosedDate"" as ClosedDate,
                 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
                ,'Shared' as TemplateUserType,tr.""DisplayName"" as TemplateDisplayName,m.""Code"" as ModuleCode
                from
                public.""NtsService"" as n
                  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false and n.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                   join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=n.""Id""  
                join public.""LOV"" as sw on  sw.""LOVType""='SHARED_TYPE' and sw.""Code""='SHARED_TYPE_TEAM' and ss.""ServiceSharedWithTypeId""=sw.""Id""
                join public.""Team"" as ts on ss.""SharedWithTeamId"" = ts.""Id"" 
                join public.""TeamUser"" as tu on ts.""Id"" = tu.""Id"" 
                join public.""User"" as nsu on tu.""UserId"" = nsu.""Id"" 
                 left  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false
                left join public.""User"" as nru on n.""RequestedByUserId""=nru.""Id""	
                    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false #PORTAL# ";

            var portalwhere = "";
            if (searchModel != null && searchModel.PortalNames.IsNotNullAndNotEmpty())
            {
                portalwhere = $@" where n.""PortalId"" in ('{searchModel.PortalNames}') ";
            }
            else
            {
                portalwhere = $@" where n.""PortalId"" in ('{_repo.UserContext.PortalId}') ";
            }
            cypher = cypher.Replace("#PORTAL#", portalwhere);

            var list = await _queryRepo.ExecuteQueryList<ServiceViewModel>(cypher, null);
            return list;
        }
        public async Task<bool> IsServiceSubjectUnique(string templateId, string subject, string serviceId)
        {
            bool flg = false;
            var query = $@"select * from public.""NtsService"" 
                where ""TemplateId""='{templateId}' and  LOWER(""ServiceSubject"")=LOWER('{subject}') and ""Id""<> '{serviceId}' and ""IsDeleted""=false limit 1";
            var data = await _queryRepo.ExecuteQuerySingle<ServiceViewModel>(query, null);
            if (data != null)
            {
                flg = true;
            }
            return flg;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetDatewiseServiceSLA(ServiceSearchViewModel searchModel)
        {
            var userId = searchModel.UserId;
            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where n.""ServiceStatusId""='68fabc87-83fd-4975-afd6-e89fa74d51c4' and n.""IsDeleted""=false  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                            group by n.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetSharedList(string ServiceId)
        {
            string query = @$"select n.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsServiceShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" AND u.""IsDeleted""= false 
                              
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false 
                                union select u.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsServiceShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" AND t.""IsDeleted""= false 
                                join public.""TeamUser"" as tu ON tu.""TeamId"" = n.""SharedWithTeamId"" AND tu.""IsDeleted""= false 
                                join public.""User"" as u ON u.""Id"" = tu.""UserId"" AND u.""IsDeleted""= false 
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<List<ServiceViewModel>> IsExitEntryFeeAvailed(string userId)
        {
            var query = $@"Select s.* from 
                        public.""Template"" as t 
                        join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and t.""Code""='AnnualLeave' and s.""IsDeleted""=false 
                        join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false 
                        join cms.""N_Leave_AnnualLeave"" as al on al.""NtsNoteId""=s.""UdfNoteId"" and al.""IsDeleted""=false
                        where lov.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and s.""OwnerUserId""='{userId}'
                        and t.""IsDeleted""=false
                        union
                        Select s.* from 
                        public.""Template"" as t 
                        join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and t.""Code""='ANNUAL_LEAVE_ADV' and s.""IsDeleted""=false 
                        join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false 
                        --join cms.""N_Leave_AnnualLeaveADV"" as al on al.""NtsNoteId""=s.""UdfNoteId"" 
                        where lov.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and s.""OwnerUserId""='{userId}'
                        and t.""IsDeleted""=false            
                        ";
            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return result;

        }
        public async Task<List<ServiceViewModel>> GetFBDashboardCountService(string userId, string moduleId = null)
        {
            var cypher = $@"select ns.""Code"" as ServiceStatusCode,u.""Id"" as OwnerUserId
                        from public.""NtsService"" as s
                        join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false 
                        join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId"" and ns.""IsDeleted""=false 
                        join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false 
                        where u.""Id""='{userId}' and s.""IsDeleted""=false  #WHERE#  
                        ";

            var search = "";
            if (moduleId.IsNotNullAndNotEmpty())
            {
                search = $@" and t.ModuleId='{moduleId}'";

            }
            cypher = cypher.Replace("#WHERE#", search);
            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(cypher, null);
            return result;
        }
        public async Task<List<TaskViewModel>> GetFBDashboardCountTask(string userId, string moduleId = null)
        {
            var cypher2 = $@"select ns.""Code"" as TaskStatusCode,u.""Id"" as OwnerUserId,ua.""Id"" as AssignedToUserId
                            from public.""NtsTask"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false 
                            left join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId"" and ns.""IsDeleted""=false 
                            left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false 
                            left join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
                              #WHERE# and s.""IsDeleted""=false  ";

            var search2 = "";
            if (moduleId.IsNotNullAndNotEmpty())
            {
                search2 = $@" where t.ModuleId='{moduleId}'";
            }
            cypher2 = cypher2.Replace("#WHERE#", search2);
            var tasklist = await _queryRepo.ExecuteQueryList<TaskViewModel>(cypher2, null);
            return tasklist;
        }
        public async Task<List<NoteViewModel>> GetFBDashboardCountNote(string userId, string moduleId = null)
        {
            var cyphernote = $@"select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId--,s.ReferenceType as ReferenceType
                            from public.""NtsNote"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false
                            left join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId"" and ns.""IsDeleted""=false 
                            left join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false 
                            where u.""Id""='{userId}' and s.""IsDeleted""=false";
            var notelist = await _queryRepo.ExecuteQueryList<NoteViewModel>(cyphernote, null);
            return notelist;
        }
        public async Task<List<ServiceViewModel>> GetServiceByUser(string userId)
        {
            var taskQry = $@"select  s.*,lv.""Name"" as ServiceStatusName,u.""Name"" as OwnerDisplayName
                from public.""User"" as u 
                join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""IsDeleted""=false 
                join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false
                and (lv.""Code""='SERVICE_STATUS_OVERDUE' or lv.""Code""='SERVICE_STATUS_INPROGRESS'
                or lv.""Code""='SERVICE_STATUS_NOTSTARTED') where u.""Id""='{userId}' and u.""IsDeleted""=false"
                ;

            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(taskQry, null);
            return result;
        }
        public async Task<List<NtsLogViewModel>> GetVersionDetails(string serviceId)
        {

            var query = $@"select l.""ServiceSubject"" as Subject, l.* from log.""NtsServiceLog""  as l
            join public.""NtsService"" as n on l.""RecordId""=n.""Id"" and n.""IsDeleted""=false and l.""VersionNo""<>n.""VersionNo""
            where l.""IsDeleted""=false and l.""RecordId"" = '{serviceId}' and l.""IsVersionLatest""=true order by l.""VersionNo"" desc";
            var result = await _queryNtsLog.ExecuteQueryList(query, null);
            return result;
        }
        public async Task<List<DashboardCalendarViewModel>> GetWorkPerformanceCount(string userId, string moduleCodes = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = $@"select n.""DueDate""::TIMESTAMP::DATE as Start, n.""DueDate""::TIMESTAMP::DATE as End,count(n.""Id""), 'OverDue' as StatusName
                from public.""NtsService"" as n
                  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false
                  join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false 
                  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false
                  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='SERVICE_STATUS_OVERDUE'
                  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false   
                  #WHERE#
                  where n.""IsDeleted""=false  and n.""PortalId""='{_userContext.PortalId}' group by n.""DueDate""::TIMESTAMP::DATE
                union
                select n.""StartDate""::TIMESTAMP::DATE as Start,n.""StartDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'Inprogress' as StatusName
                from public.""NtsService"" as n
                  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
                join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false 
                join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='SERVICE_STATUS_INPROGRESS'
                  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false   
                  #WHERE# 
                  where n.""IsDeleted""=false and n.""PortalId""='{_userContext.PortalId}' group by n.""StartDate""::TIMESTAMP::DATE
                union
                select n.""ClosedDate""::TIMESTAMP::DATE as Start,n.""ClosedDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'Closed' as StatusName
                 from public.""NtsService"" as n
                  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
                join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false  
                join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false and ns.""Code""='SERVICE_STATUS_CLOSE'
                  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false   
                  #WHERE# 
                  where n.""IsDeleted""=false  and n.""PortalId""='{_userContext.PortalId}' group by n.""ClosedDate""::TIMESTAMP::DATE
                union
                select n.""StartDate""::TIMESTAMP::DATE as Start,n.""StartDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'NotStarted' as StatusName
                 from public.""NtsService"" as n
                  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false 
                join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false  
                join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false and ns.""Code""='SERVICE_STATUS_NOTSTARTED'
                  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false    
                  #WHERE# 
                  where n.""IsDeleted""=false and n.""PortalId""='{_userContext.PortalId}' group by n.""StartDate""::TIMESTAMP::DATE
                union
                select n.""ReminderDate""::TIMESTAMP::DATE as Start, n.""ReminderDate""::TIMESTAMP::DATE as End, count(n.""Id""), 'Reminder' as StatusName
                 from public.""NtsService"" as n
                  join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""Id""='{userId}' and nou.""IsDeleted""=false
                join public.""User"" as nou1 on n.""RequestedByUserId""=nou1.""Id"" and nou1.""Id""='{userId}' and nou1.""IsDeleted""=false  
                join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
                  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                  join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                  left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false  
                #WHERE# 
                where n.""IsDeleted""=false and n.""PortalId""='{_userContext.PortalId}' and n.""ReminderDate"" is not null group by n.""ReminderDate""::TIMESTAMP::DATE 
                union
                select nf.""CreatedDate""::TIMESTAMP::DATE as Start,nf.""CreatedDate""::TIMESTAMP::DATE as End, count(nf.""Id""), 'Notification' as StatusName
                from public.""NtsService"" as n
                join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false
                join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
                left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                #WHERE#  
                join public.""Notification"" as nf on n.""Id""=nf.""ReferenceTypeId""
                where nf.""IsDeleted"" = false and nf.""ToUserId""='{userId}' and nf.""PortalId""='{_userContext.PortalId}' group by nf.""CreatedDate""::TIMESTAMP::DATE  ";

            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            query = query.Replace("#WHERE#", search);
            var result = await _queryDashboardCalendar.ExecuteQueryList(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetCMSExternalRequest(string templatesin)
        {
            var query = $@"select sr.""Id"" as Id ,sr.""ServiceSubject"" as Name 
                        from Public.""NtsService"" as sr inner join ""NtsNote"" as n on n.""Id""=sr.""UdfNoteId""  and n.""IsDeleted""=false
                        where sr.""TemplateId"" in ({templatesin})  and sr.""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<List<ServiceViewModel>> GetServiceList(string portalId, string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string statusCodes = null, string parentServiceId = null)
        {
            string query = @$"Select s.""Id"",s.""ServiceNo"",s.""ServiceSubject"",s.""TemplateCode"",o.""Name"" as OwnerDisplayName,ts.""Name"" as ServiceStatusName,ts.""Code"" as ServiceStatusCode,
                            s.""StartDate"",s.""DueDate"",s.""RequestedByUserId"", tem.""DisplayName"" as TemplateDisplayName,s.""UdfNoteTableId"" as udfNoteTableId,
                            s.""CreatedDate"",s.""CompletedDate"" as CompletedDate, s.""RejectedDate"" as RejectedDate, s.""CanceledDate"" as CanceledDate, s.""ClosedDate"" as ClosedDate,s.""LastUpdatedDate"",stemp.""IconFileId""
                            From public.""NtsService"" as s
                            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  #TemCodeWhere#
                            Join public.""ServiceTemplate"" as stemp on stemp.""TemplateId"" = tem.""Id"" and stemp.""IsDeleted"" = false
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  #ModCodeWhere#
                            left Join public.""User"" as o on s.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on s.""ServiceStatusId""=ts.""Id"" and ts.""IsDeleted""=false #StatusWhere#
                            where s.""PortalId"" in ('{portalId.Replace(",", "','")}') and s.""IsDeleted""=false #OWNERWHERE# #ParentWhere# order by s.""CreatedDate"" desc";

            string ownerWhere = "";
            if (!showAllOwnersService || (requestby.IsNotNullAndNotEmpty() && requestby == "RequestedByMe"))
            {
                ownerWhere = $@" and (s.""RequestedByUserId""='{_repo.UserContext.UserId}' or s.""OwnerUserId""='{_repo.UserContext.UserId}') ";
            }
            query = query.Replace("#OWNERWHERE#", ownerWhere);

            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and tem.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            string statusCodeWhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                var codes = statusCodes.Trim(',').Replace(",", "','");
                statusCodeWhere = $@" and ts.""Code"" in ('{codes}')";
            }
            query = query.Replace("#StatusWhere#", statusCodeWhere);

            string parentWhere = "";
            if (parentServiceId.IsNotNullAndNotEmpty())
            {
                parentWhere = $@" and s.""ParentServiceId""='{parentServiceId}' ";
            }
            query = query.Replace("#ParentWhere#", parentWhere);

            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return result;
        }
        public async Task<List<ServiceViewModel>> GetInternalServiceListFromExternalRequestId(string serviceId, string udfs)
        {
            var query = $@"
                        select is.*,lov.""Code"" as ServiceStatusCode from public.""NtsService"" as s 
                        join (" + udfs + $@") as udf on udf.""externalServiceRequest""=s.""Id""
                        join public.""NtsService"" as is on is.""UdfNoteId""=udf.""NtsNoteId""
                        join public.""LOV"" as lov on lov.""Id""=is.""ServiceStatusId""
                        where s.""IsDeleted""=false and s.""Id""='{serviceId}' and is.""IsDeleted""=false
                        ";
            var data = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetExternalServiceChartByStatus()
        {
            var query = $@"
            select count(s) as ""Value"",case when tl.""Code"" is null then lov.""Name"" else task.""TaskSubject"" end as ""Type""
            --,s.""ServiceStatusId"" as ""Id"" 
            ,s.""TemplateCode""
          from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id""
            left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
            group by ""Type"",s.""TemplateCode"" --,s.""ServiceStatusId""
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetExternalUserServiceChartByStatus()
        {
            var query = $@"
            select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"",tc.""Code"",
            case when lov.""Code""='SERVICE_STATUS_DRAFT' then '#17a2b8'
when lov.""Code""='SERVICE_STATUS_COMPLETE' then '#13b713'
when lov.""Code""='SERVICE_STATUS_INPROGRESS' then '#007bff'
when lov.""Code""='SERVICE_STATUS_OVERDUE' then '#ffc107'
when lov.""Code""='SERVICE_STATUS_REJECT' then '#f10b0b'
end as StatusColor
          from public.""NtsService"" as s 
            
            --join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false
            join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_EXT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            --left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""IsDeleted""=false
           -- left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false    and s.""OwnerUserId""='{_userContext.UserId}' 
            group by s.""ServiceStatusId"",""Type"",lov.""Code"",tc.""Code""
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetInternalServiceChartByStatus()
        {
            var query = $@"
            select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"",tc.""Code"",
case when lov.""Code""='SERVICE_STATUS_DRAFT' then '#17a2b8'
when lov.""Code""='SERVICE_STATUS_COMPLETE' then '#13b713'
when lov.""Code""='SERVICE_STATUS_INPROGRESS' then '#007bff'
when lov.""Code""='SERVICE_STATUS_OVERDUE' then '#ffc107'
when lov.""Code""='SERVICE_STATUS_REJECT' then '#f10b0b'
end as StatusColor
            from public.""NtsService"" as s 
            
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_INT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false 
            group by ""Type"",s.""ServiceStatusId"",lov.""Code"",tc.""Code""
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetExternalUserInternalServiceChartByStatus()
        {
            var query = $@"
                   select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"" ,tc.""Code""
          from public.""NtsService"" as s 
            
            --join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_EXT'
            
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            --left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""IsDeleted""=false
            --left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false    and s.""OwnerUserId""='{_userContext.UserId}'  and lov.""Code"" in ('SERVICE_STATUS_COMPLETE','SERVICE_STATUS_CLOSE')
            group by s.""ServiceStatusId"",""Type"",tc.""Code""         
            ";
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetInternalDashboardChartByStatus(ServiceSearchViewModel search)
        {
            var query = $@"
                        select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"",
case when lov.""Code""='SERVICE_STATUS_DRAFT' then '#17a2b8'
when lov.""Code""='SERVICE_STATUS_COMPLETE' then '#13b713'
when lov.""Code""='SERVICE_STATUS_INPROGRESS' then '#007bff'
when lov.""Code""='SERVICE_STATUS_OVERDUE' then '#ffc107'
when lov.""Code""='SERVICE_STATUS_REJECT' or lov.""Code""='SERVICE_STATUS_CANCEL' then '#f10b0b'
end as StatusColor
from public.""NtsService"" as s 
                        Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
                        Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
                        join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
                        where s.""IsDeleted""=false and lov.""Code""<>'SERVICE_STATUS_DRAFT' #CATEGORY# #TEMPLATE# 
                        group by ""Type"",s.""ServiceStatusId"",lov.""Code""
                        ";

            var category = "";
            var template = "";
            if (search.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = search.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (search.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = search.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tem.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);

            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetInternalDashboardTaskChart(ServiceSearchViewModel search)
        {
            var query = $@"
            select count(t) as ""Value"",lovt.""Name"" as ""Type"",t.""TaskStatusId"" as ""Id"", 
case when lovt.""Code""='TASK_STATUS_DRAFT' then '#17a2b8'
when lovt.""Code""='TASK_STATUS_COMPLETE' then '#13b713'
when lovt.""Code""='TASK_STATUS_INPROGRESS' then '#007bff'
when lovt.""Code""='TASK_STATUS_OVERDUE' then '#ffc107'
when lovt.""Code""='TASK_STATUS_REJECT' or lovt.""Code""='TASK_STATUS_CANCEL' then '#f10b0b'
end as StatusColor
            from public.""NtsService"" as s             
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            Join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false  and t.""TaskType""=2
            join public.""LOV"" as lovt on lovt.""Id""=t.""TaskStatusId""
            where s.""IsDeleted""=false and lovt.""Code""<>'TASK_STATUS_DRAFT' #CATEGORY# #TEMPLATE# 
            group by ""Type"",t.""TaskStatusId"",lovt.""Code""
            ";

            var category = "";
            var template = "";
            if (search.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = search.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (search.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = search.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tem.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);
            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetExternalServiceSLA(ServiceSearchViewModel searchModel)
        {
            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""in ('CMS_SEBI_EXT','CMS_SEBI_INT')
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and n.""IsDeleted""=false  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                            group by n.""DueDate""::TIMESTAMP::DATE order by n.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetRequestSLA(ServiceSearchViewModel searchModel)
        {

            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false --and tc.""Code""='CMS_SEBI'
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and n.""IsDeleted""=false  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                           #CATEGORY# #TEMPLATE#  group by n.""DueDate""::TIMESTAMP::DATE ";

            var category = "";
            var template = "";
            if (searchModel.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = searchModel.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (searchModel.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = searchModel.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tr.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetExternalUserExternalServiceSLA(ServiceSearchViewModel searchModel)
        {

            var query = $@"select  CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""ServiceSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                            from public.""NtsService"" as n
                            join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
                            join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
  	                        join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_EXT'
                            join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false 
                            left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
                            where ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and n.""IsDeleted""=false and n.""OwnerUserId""='{_userContext.UserId}'  
                            and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                            group by n.""DueDate""::TIMESTAMP::DATE order by n.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;

        }
        public async Task<List<ServiceViewModel>> GetSEBIServiceList()
        {
            var query = $@"
            select s.*,case when tl.""Code"" is null then lov.""Name"" else task.""TaskSubject"" end as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id""
            left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
            union
            select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_INT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false  order by ""CreatedDate"" desc
            ";
            var data = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return data;
        }
        public async Task<List<ServiceViewModel>> GetSEBIExternalServiceList(string tasktemplatecode)
        {
            var query = $@"
            select s.*,tl.""Name"" as ""ServiceStatusName"",tl.""Code"" as ""ServiceStatusCode"",nou.""Name"" as ""OwnerDisplayName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""TemplateCode""='{tasktemplatecode}'
            join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and s.""OwnerUserId""='{_userContext.UserId}'
           
            ";
            var data = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return data;
        }
        public async Task<List<ServiceViewModel>> GetExternalSEBIServiceList()
        {
            var query = $@"
            select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" 
                   from public.""NtsService"" as s 
            
            --join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_EXT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            --left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id"" and task.""IsDeleted""=false
            --left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and s.""OwnerUserId""='{_userContext.UserId}'  --and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
            --union
            --select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            --join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            --Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            --Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""Code""='CMS_SEBI_INT'
            --join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            --where s.""IsDeleted""=false and s.""OwnerUserId""='{_userContext.UserId}'
            order by ""CreatedDate"" desc";
            var data = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return data;
        }
        public async Task<List<ServiceViewModel>> GetInternalDashboardServiceList(ServiceSearchViewModel searchModel)
        {
            var query = $@"
            select s.*,case when tl.""Code"" is null then lov.""Name"" else task.""TaskSubject"" end as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" 
            , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            
            join cms.""N_CMS_SEBI_EXT_GENERAL"" as udf on udf.""NtsNoteId""=s.""UdfNoteId""
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            left join public.""NtsTask"" as task on task.""ParentServiceId""=s.""Id""
            left join public.""LOV"" as tl on tl.""Id""=task.""TaskStatusId""
            where s.""IsDeleted""=false and task.""IsDeleted""=false and tl.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')  #TEMPLATE# 
            union
            select s.*,lov.""Name"" as ""ServiceStatusName"",nou.""Name"" as ""OwnerUserUserName"" , tem.""DisplayName"" as ""TemplateDisplayName"" from public.""NtsService"" as s 
            join public.""User"" as nou on s.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false 
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  
            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false --and tc.""Code""='CMS_SEBI_INT'
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
           
            where s.""IsDeleted""=false and lov.""Code""<>'SERVICE_STATUS_DRAFT'  #CATEGORY# #TEMPLATE# 
            ";

            var category = "";
            var template = "";
            if (searchModel.TemplateCategoryCode.IsNotNullAndNotEmpty())
            {
                var catCodes = searchModel.TemplateCategoryCode.Trim(',').Replace(",", "','");
                category = $@" and tc.""Code"" in ('{catCodes}') ";
            }

            if (searchModel.TemplateMasterCode.IsNotNullAndNotEmpty())
            {
                var temCodes = searchModel.TemplateMasterCode.Trim(',').Replace(",", "','");
                template = $@" and tem.""Code"" in ('{temCodes}') ";
            }
            query = query.Replace("#CATEGORY#", category);
            query = query.Replace("#TEMPLATE#", template);
            var data = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return data;
        }
        public async Task<List<NtsLogViewModel>> GetServiceLog(string ServiceId, string TemplateCode)
        {
            var Query = $@"Select L.""CreatedDate"" as LogStartDate 
                            from Log.""NtsServiceLog"" as L 
                            inner join public.""NtsService"" as s on L.""RecordId""=s.""Id"" 
                            where L.""RecordId""='{ServiceId}'";
            var result = await _queryNtsLog.ExecuteQueryList(Query, null);
            return result;
        }
        public async Task<string> GetUdfQuery(string TemplateType, string categoryCode, string templateCodes, string excludeTemplateCodes, params string[] columns)
        {

            string Metadeta = "", MianTable = "", Eventid = "", LefCol = "";

            switch (TemplateType)
            {
                case "Service":
                    Metadeta = "UdfTableMetadataId";
                    MianTable = "NtsServiceLog";
                    Eventid = "ServiceEventId";
                    LefCol = "UdfNoteId";
                    break;
                case "Note":
                    Metadeta = "TableMetadataId";
                    MianTable = "NtsNoteLog";
                    Eventid = "NoteEventId";
                    LefCol = "RecordId";
                    break;
                case "Task":
                    Metadeta = "UdfTableMetadataId";
                    MianTable = "NtsTaskLog";
                    Eventid = "TaskEventId";
                    LefCol = "UdfNoteId";
                    break;
            }

            var query = @$"select cm.*,tm.""Name"" as ""TableName"",tm.""Schema"" as ""TableSchemaName"",
            t.""Id"" as ""TemplateId"" from public.""Template"" t
            join public.""TemplateCategory"" tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
            join public.""TableMetadata"" tm on t.""{Metadeta}""=tm.""Id"" and tm.""IsDeleted""=false 
            join public.""ColumnMetadata"" cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsDeleted""=false  and cm.""IsUdfColumn""=true
            where t.""IsDeleted""=false  ";


            if (templateCodes.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and t.""Code""='{templateCodes}'";
            }
            var columnList = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            var q = "";
            var queryList = new List<string>();

            if (columnList.Count > 0)
            {
                q = @$"Select Distinct L.""CreatedDate"" as ""LogStartDate"",L.""LogStartDateTime"",u.""Name"" as ""OwnerName"",lov.""Name"" as ""EventName"", ";
                var tables = columnList.GroupBy(x => x.TableName).Select(x => x.FirstOrDefault()).ToList();
                foreach (var table in columnList)
                {
                    if (table.Name != null)
                    {
                        q = @$"{q} udf.""{table.Name}"" as ""{table.Name}"",";
                    }
                    else
                    {
                        q = @$"{q} null as ""{null}"",";
                    }
                }
                var qery = @$"from Log.""{MianTable}"" as L left join log.""{columnList[0].TableName + "Log"}"" as udf on L.""{LefCol}"" = udf.""NtsNoteId""   and L.""VersionNo"" =udf.""VersionNo""
         left join public.""User"" as u on L.""CreatedBy""=u.""Id""
         left join public.""LOV"" as lov on lov.""Id""=L.""{Eventid}""";
                queryList.Add(@$"{q.Trim(',')} {qery}");
            }
            else
            {
                q = @$"Select Distinct L.""CreatedDate"" as ""LogStartDate"",L.""LogStartDateTime"",u.""Name"" as ""OwnerName"",lov.""Name"" as ""EventName""";
                var qery = @$"from Log.""{MianTable}"" as L 
         left join public.""User"" as u on L.""CreatedBy""=u.""Id""
         left join public.""LOV"" as lov on lov.""Id""=L.""{Eventid}""";
                queryList.Add(@$"{q.Trim(',')} {qery}");

            }

            if (queryList.Any())
            {
                return string.Join($"{Environment.NewLine} union {Environment.NewLine}", queryList);
            }
            return "";
        }
        public async Task<List<string>> GetUdfColumnList(string TemplateType, string categoryCode, string templateCodes, string excludeTemplateCodes, params string[] columns)
        {

            string Metadeta = "", MianTable = "";

            switch (TemplateType)
            {
                case "Service":
                    Metadeta = "UdfTableMetadataId";

                    break;
                case "Note":
                    Metadeta = "TableMetadataId";
                    break;
                case "Task":
                    Metadeta = "UdfTableMetadataId";
                    break;
            }
            var query = @$"select cm.*,tm.""Name"" as ""TableName"",tm.""Schema"" as ""TableSchemaName"",
            t.""Id"" as ""TemplateId"" from public.""Template"" t
            join public.""TemplateCategory"" tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
            join public.""TableMetadata"" tm on t.""{Metadeta}""=tm.""Id"" and tm.""IsDeleted""=false 
            join public.""ColumnMetadata"" cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsDeleted""=false  and cm.""IsUdfColumn""=true
            where t.""IsDeleted""=false  ";


            if (templateCodes.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and t.""Code""='{templateCodes}'";
            }
            var columnList = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);

            var tables = columnList.GroupBy(x => x.TableName).Select(x => x.FirstOrDefault()).ToList();
            var q = "";
            q = @$"select ";
            var queryList = new List<string>();

            foreach (var table in columnList)
            {
                if (table.Name != null)
                {
                    //q = @$"{q} ""{table.Name}"" as ""{table.LabelName}"",";

                    queryList.Add(table.Name);
                }

            }

            return queryList;
        }
        public async Task<List<IdNameViewModel>> GetServiceUserList(string serviceId)
        {
            string query = @$"select u.""Id"",u.""Name""
                    from public.""NtsService"" as s
                    join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false
                    where s.""Id""='{serviceId}' and s.""IsDeleted""=false
                    union
                    select u.""Id"",u.""Name""
                    from public.""NtsService"" as s
                    join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
                    where s.""Id""='{serviceId}' and s.""IsDeleted""=false
                    union
                    
                    select u.""Id"",u.""Name""
                    from public.""NtsService"" as s
                    join public.""NtsTask"" as t on t.""ParentServiceId""=s.""Id"" and t.""IsDeleted""=false
                    join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false
                    where s.""Id""='{serviceId}' and s.""IsDeleted""=false
                    
                    union
                    
                    select u.""Id"",u.""Name""
                    from public.""NtsService"" as s
                    join public.""NtsServiceShared"" as t on t.""NtsServiceId""=s.""Id"" and t.""IsDeleted""=false
                    join public.""User"" as u on u.""Id""=t.""SharedWithUserId"" and u.""IsDeleted""=false
                    where s.""Id""='{serviceId}' and s.""IsDeleted""=false
                    union
                    
                    select u.""Id"",u.""Name""
                    from public.""NtsService"" as s
                    join public.""NtsServiceShared"" as t on t.""NtsServiceId""=s.""Id"" and t.""IsDeleted""=false
                    join public.""Team"" as tm on tm.""Id""=t.""SharedWithTeamId"" and tm.""IsDeleted""=false
                    join public.""TeamUser"" as tu on tu.""TeamId""=tm.""Id"" and tu.""IsDeleted""=false
                    join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
                    where s.""Id""='{serviceId}' and s.""IsDeleted""=false
                    
                     ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;

        }
        public async Task<List<TreeViewViewModel>> GetDocumentTreeviewListByService(string serviceId)
        {
            var list = new List<TreeViewViewModel>();
            var query = @$"Select s.""Id"" as ""Id"",s.""ServiceSubject"" as Name,'Service' as Type,true as hasChildren from public.""NtsService"" as s                        
                        where s.""Id""='{serviceId}'
                        union
                        Select s.""Id"" as ""Id"",s.""TaskSubject"" as Name,'Task' as Type,true as hasChildren from public.""NtsTask"" as s                        
                        where s.""ParentServiceId""='{serviceId}'
                        union
                        Select s.""Id"" as ""Id"",s.""NoteSubject"" as Name,'Note' as Type,true as hasChildren from public.""NtsNote""  as s                        
                        where s.""ParentServiceId""='{serviceId}'
                        ";

            list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }
        public async Task<List<TreeViewViewModel>> GetDocumentIndexTreeviewListByService(string docServiceId)
        {
            var list = new List<TreeViewViewModel>();
            var query = @$"Select distinct s.""Id"" as id,s.""ServiceSubject"" as Name, 'SERVICE' as ParentId, 'SERVICE' as Type,
                        true as hasChildren
                        FROM public.""NtsService"" as s
                        Where s.""IsDeleted""=false and s.""Id""='{docServiceId}'
                        union
                        Select distinct s.""Id"" as id,s.""ServiceSubject"" as Name, '{docServiceId}' as ParentId, 'SERVICE' as Type,
                        true as hasChildren
                        FROM public.""NtsService"" as s
                        Where s.""IsDeleted""=false and s.""ParentServiceId""='{docServiceId}'
                        union
                        select distinct t.""Id"" as id,t.""TaskSubject"" as Name, '{docServiceId}' as ParentId, 'TASK' as Type,
                        true as hasChildren
                        FROM public.""NtsTask"" as t
                        where t.""IsDeleted""=false and t.""ParentServiceId""='{docServiceId}'
                        union
                        select distinct n.""Id"" as id,n.""NoteSubject"" as Name, '{docServiceId}' as ParentId, 'NOTE' as Type,
                        false as hasChildren
                        FROM public.""NtsNote"" as n
                        where n.""IsDeleted""=false and n.""ParentServiceId""='{docServiceId}'
                        ";
            list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }
        public async Task<ServiceTemplateViewModel> GetBookDetails(string serviceId)
        {
            var query = @$"select ""TM"".*,""S"".""Id"" as ""ServiceId"",""T"".""DisplayName"" as ""TemplateDisplayName""
            from ""public"".""ServiceTemplate"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""Id""=""TM"".""TemplateId"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsService"" as ""S"" on ""T"".""Id""=""S"".""TemplateId"" and ""S"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false  and ""S"".""Id""='{serviceId}'";
            var result = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            return result;
        }
        public async Task<List<NtsViewModel>> GetBookList(string serviceId, string templateId, bool includeitemDetails = false)
        {
            string query = @$"select t.""DisplayName"" as ""TemplateName"",s.""ServiceSubject"" as ""Subject"",s.""ServiceDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,null as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,0 as ""Level"",'Service' as ""NtsType"",s.""Id"" as ""Id"",1 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""ServiceNo"" as ""NtsNo"",true as ""AutoLoad"",0 as ""SequenceOrder"",'1' as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsService"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""ServiceTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
	        left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""ServicePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsServiceShared"" as shr on shr.""NtsServiceId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'            
            where s.""Id""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            union all
           ( select t.""DisplayName"" as ""TemplateName"",s.""TaskSubject"" as ""Subject"",s.""TaskDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,'-1' as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Task' as ""NtsType"",s.""Id"" as ""Id"",2 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""TaskNo"" as ""NtsNo"",false as ""AutoLoad"",stc.""SequenceOrder"" as ""SequenceOrder"",'1' as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            FROM public.""NtsTask"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""TaskTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""StepTaskComponent"" as stc on s.""StepTaskComponentId""=stc.""Id"" and  stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lv on lv.""Id""=s.""TaskStatusId"" and lv.""IsDeleted"" = false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""TaskPriorityId"" and lp.""IsDeleted"" = false  and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and u.""IsDeleted"" = false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsTaskShared"" as shr on shr.""NtsTaskId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'            
            WHERE  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""ParentServiceId""='{serviceId}' 
                        and s.""TaskType""='2' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or s.""AssignedToUserId""='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            order by stc.""SequenceOrder"" )
             union all
            select t.""DisplayName"" as ""TemplateName"",s.""NoteSubject"" as ""Subject"",s.""NoteDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentNoteId"",s.""ParentTaskId"",s.""ParentServiceId"") as parentId,t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Note' as NtsType,s.""Id"" as Id,14 as ItemType,u.""Name"" as AssigneeOrOwner
            ,s.""ExpiryDate"" as DueDate,s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""NoteNo"" as NtsNo,false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as StartDate,s.""ReminderDate"" as ReminderDate,s.""CompletedDate"" as CompletedDate
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsNote"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and (t.""ViewType"" is null or t.""ViewType""<>2) and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""NoteTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""NoteStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""NotePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsNoteShared"" as shr on shr.""NtsNoteId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'             
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""TaskSubject"" as ""Subject"",s.""TaskDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentNoteId"",s.""ParentTaskId"",s.""ParentServiceId"") as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Task' as ""NtsType"",s.""Id"" as ""Id"",4 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""TaskNo"" as ""NtsNo"",false as ""AutoLoad"",s.""SequenceOrder"" as ""SequenceOrder"",null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsTask"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""TaskTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""TaskStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""TaskPriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsTaskShared"" as shr on shr.""NtsTaskId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                        
            where s.""ServicePlusId""='{serviceId}' and s.""TaskType""=4 and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or s.""AssignedToUserId""='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')            
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""ServiceSubject"" as ""Subject"",s.""ServiceDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,coalesce(s.""ParentNoteId"",s.""ParentTaskId"",s.""ParentServiceId"") as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Service' as ""NtsType"",s.""Id"" as ""Id"",13 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""ServiceNo"" as ""NtsNo"",false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsService"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and (t.""ViewType"" is null or t.""ViewType""<>2) and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""ServiceTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""ServicePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsServiceShared"" as shr on shr.""NtsServiceId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                        
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""NoteSubject"" as ""Subject"",s.""NoteDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,'-2' as parentId,t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Note' as NtsType,s.""Id"" as Id,16 as ItemType,u.""Name"" as AssigneeOrOwner
            ,s.""ExpiryDate"" as DueDate,s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""NoteNo"" as NtsNo,false as ""AutoLoad"",s.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as StartDate,s.""ReminderDate"" as ReminderDate,s.""CompletedDate"" as CompletedDate
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsNote"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""ViewType""=2 and  t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""NoteTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""TableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""NoteStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""NotePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsNoteShared"" as shr on shr.""NtsNoteId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                         
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')           
            union all
            select t.""DisplayName"" as ""TemplateName"",s.""ServiceSubject"" as ""Subject"",s.""ServiceDescription"" as ""Description"",lv.""Code"" as ""StatusCode""
            ,'-2' as ""parentId"",t.""SequenceOrder"" as ""TemplateSequence""
            ,1 as ""Level"",'Service' as ""NtsType"",s.""Id"" as ""Id"",16 as ""ItemType"",u.""Name"" as ""AssigneeOrOwner""
            ,s.""DueDate"" as ""DueDate"",s.""TemplateId"" as ""TemplateId"",t.""Code"" as ""TemplateCode""
            ,s.""ServiceNo"" as ""NtsNo"",false as ""AutoLoad"",nt.""SequenceOrder"" as SequenceOrder,null as ""ItemNo""
            ,lv.""Name"" as ""StatusName"",s.""StartDate"" as ""StartDate"",s.""ReminderDate"" as ""ReminderDate"",s.""CompletedDate"" as ""CompletedDate""
            ,lp.""Name"" as ""PriorityName"",cm.""UdfCount"" as ""UdfCount""
            ,nt.""HideHeader"" as ""HideHeader"",nt.""HideSubject"" as ""HideSubject"",nt.""HideDescription"" as ""HideDescription""
            from public.""NtsService"" as s
            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""ViewType""=2 and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""ServiceTemplate"" as nt on t.""Id""=nt.""TemplateId"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join
			(
				select cm.""TableMetadataId"" as ""TableMetadataId"",count(cm.""IsUdfColumn"") as ""UdfCount""
                from public.""TableMetadata"" as tm join public.""ColumnMetadata"" 
				as cm on tm.""Id""=cm.""TableMetadataId"" and cm.""IsUdfColumn""=true
				group by cm.""TableMetadataId""
			) cm on t.""UdfTableMetadataId""=cm.""TableMetadataId""
            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and  lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""LOV"" as lp on lp.""Id""=s.""ServicePriorityId"" and  lp.""IsDeleted""=false and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
            --left join public.""NtsServiceShared"" as shr on shr.""NtsServiceId"" = s.""Id"" and shr.""IsDeleted"" = false and shr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and shr.""SharedWithUserId""='{_repo.UserContext.UserId}'                        
            where s.""ServicePlusId""='{serviceId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            --and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" ='{_repo.UserContext.UserId}' or shr.""SharedWithUserId"" ='{_repo.UserContext.UserId}')
            ";
            var list = await _queryRepo.ExecuteQueryList<NtsViewModel>(query, null);
            return list;
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
        public async Task<List<ServiceViewModel>> ReadServiceBookData(string moduleCodes, string templateCodes, string categoryCodes)
        {
            string query = $@"Select s.""Id"",s.""ServiceNo"",s.""ServiceSubject"",s.""TemplateCode"",o.""Name"" as OwnerDisplayName,ts.""Name"" as ServiceStatusName,ts.""Code"" as ServiceStatusCode,
                             s.""ParentServiceId"",s.""StartDate"",s.""DueDate"",s.""RequestedByUserId"", tem.""DisplayName"" as TemplateDisplayName,
                            s.""CreatedDate"",s.""CompletedDate"" as CompletedDate, s.""RejectedDate"" as RejectedDate, s.""CanceledDate"" as CanceledDate, s.""ClosedDate"" as ClosedDate
                            ,s.""Id"" as key,s.""ServiceNo"" as title,true as lazy                     
                            From public.""NtsService"" as s
                            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false #TemCodeWhere# 
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false #ModCodeWhere# 
                            Join public.""User"" as o on s.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""LOV"" as ts on s.""ServiceStatusId""=ts.""Id"" and ts.""IsDeleted""=false 
                            where s.""IsDeleted""=false and s.""PortalId""='{_userContext.PortalId}'";

            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and tem.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return result;
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
        public async Task<List<ColumnMetadataViewModel>> GetServiceViewableColumnsPrimaryKeyColumns()
        {
            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
                where 
                (
                    (t.""Schema""='public' and t.""Name""='ServiceTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsService')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false ";

            var columnList = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return columnList;
        }
        public async Task<ServiceTemplateViewModel> GetFormIoDataByService(string serviceId)
        {
            var query = @$"select ""NT"".*,""TM"".""Name"" as ""TableName"" ,""lov"".""Code"" as ""ServiceStatusCode""
            ,""TM"".""Id"" as ""TableMetadataId"",""T"".""Json"" as ""Json""
            from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""TableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""ServiceTemplate"" as ""NT"" on ""T"".""Id""=""NT"".""TemplateId"" and ""NT"".""IsDeleted""=false 
            join ""public"".""NtsService"" as ""N"" on ""T"".""Id""=""N"".""TemplateId"" and ""N"".""IsDeleted""=false 
            join ""public"".""LOV"" as ""lov"" on ""N"".""ServiceStatusId""=""lov"".""Id"" and ""lov"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false   and ""N"".""Id""='{serviceId}'";

            var result = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            return result;
        }
        public async Task<DataRow> GetFormIoDataRow(ServiceTemplateViewModel data, string serviceId)
        {
            var selectQuery = @$"select * from cms.""{data.TableName}"" where ""NtsServiceId""='{serviceId}' limit 1";
            var dr = await _queryRepo.ExecuteQueryDataRow(selectQuery, null);
            return dr;
        }
        public async Task<List<BookViewModel>> GetGroupBookByCategoryId(string categoryId)
        {
            var list = new List<BookViewModel>();

            var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and ps.""UdfNoteTableId"" ='{categoryId}'
             left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";


            list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            return list;
        }
        public async Task<List<BookViewModel>> GetAllBook(string templateCodes, string templateIds, string bookIds, string search, string categoryIds, string permission)
        {
            var list = new List<BookViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                if (_userContext.IsSystemAdmin || (permission != null && permission.Contains("CanViewAllBooks")))
                {
                    templateCodes = templateCodes.Replace(",", "','");
                    var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
                            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
                            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
                            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
                            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
                            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
                            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
                            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
                            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                                         and tt.""Code"" in ('{templateCodes}')
                            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
                            left join (
                                select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",mp.""BookId"" as ""BookId"" from cms.""N_SNC_TECProcess_ProcessItem"" as c
                                 join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=c.""Id""  and mp.""IsDeleted"" = 'false'
                                   group by mp.""BookId""  
                              
                            ) as Item on ""BookId""=bk.""Id""
                            left join (
                                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                                where ""IsDeleted"" = false group by ""NtsId""
                            ) as r on ""NtsId""=s.""Id""
                            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
                            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
                            order by ""LastUpdatedDate"" desc ";
                    var where = "";
                    var categorywhere = "";
                    var groupwhere = "";
                    if (search.IsNotNullAndNotEmpty())
                    {
                        where = $@"and (lower(bk.""ProcessGroupName"") ILIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") ILIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""ProcessGroupDescription"") ILIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"")";
                    }
                    if (bookIds.IsNotNullAndNotEmpty())
                    {
                        bookIds = bookIds.Replace(",", "','");
                        categorywhere = $@" and bk.""Id"" in ('{bookIds}')";
                    }
                    if (categoryIds.IsNotNullAndNotEmpty())
                    {
                        categoryIds = categoryIds.Replace(",", "','");
                        groupwhere = $@" and ps.""UdfNoteTableId"" in ('{categoryIds}')";
                    }
                    query = query.Replace("#SEARCHWHERE#", where);
                    query = query.Replace("#CATEGORYWHERE#", categorywhere);
                    query = query.Replace("#GROUPWHERE#", groupwhere);
                    list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
                }
                else
                {
                    templateCodes = templateCodes.Replace(",", "','");
                    var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
            union

            select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
            union
            
            select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
            union

            select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'            
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' #SEARCHWHERE# #CATEGORYWHERE# #GROUPWHERE#
            order by ""LastUpdatedDate"" desc 

            ";
                    var where = "";
                    var categorywhere = "";
                    var groupwhere = "";
                    if (search.IsNotNullAndNotEmpty())
                    {
                        where = $@"and (lower(bk.""ProcessGroupName"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""ProcessGroupDescription"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"")";
                    }
                    if (bookIds.IsNotNullAndNotEmpty())
                    {
                        bookIds = bookIds.Replace(",", "','");
                        categorywhere = $@" and bk.""Id"" in ('{bookIds}')";
                    }
                    if (categoryIds.IsNotNullAndNotEmpty())
                    {
                        categoryIds = categoryIds.Replace(",", "','");
                        groupwhere = $@" and ps.""UdfNoteTableId"" in ('{categoryIds}')";
                    }
                    query = query.Replace("#SEARCHWHERE#", where);
                    query = query.Replace("#CATEGORYWHERE#", categorywhere);
                    query = query.Replace("#GROUPWHERE#", groupwhere);
                    list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
                }
            }
            else if (templateIds.IsNotNullAndNotEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
             join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Id"" in ('{templateIds}')
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
             left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'           
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Id"" in ('{templateIds}') where c.""IsDeleted"" = 'false' group by ""ServiceId""
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by ""LastUpdatedDate"" desc";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else if (bookIds.IsNotNullAndNotEmpty())
            {
                bookIds = bookIds.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" ='false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'  and ps.""CompanyId""='{_userContext.CompanyId}'
             join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
               left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'  
            where s.""Id"" in ('{bookIds}') and c.""IsDeleted"" = 'false'
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where s.""Id"" in ('{bookIds}') and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by ""LastUpdatedDate"" desc";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else if (search.IsNotNullAndNotEmpty())
            {

                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName,
            bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'  and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
            select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId"" from public.""NtsService"" as c
            join public.""NtsService"" as s on s.""Id""=c.""ParentServiceId"" and s.""IsDeleted"" = 'false'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bk on s.""UdfNoteId""=bk.""NtsNoteId"" and bk.""IsDeleted"" = 'false' 
            where (lower(bk.""ProcessGroupName"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"")  and c.""IsDeleted"" = 'false'
            ) as Item on ""ServiceId""=s.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where (lower(bk.""ProcessGroupName"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"" or lower(bk.""HtmlContent"") LIKE  lower('%{search}%') COLLATE ""tr-TR-x-icu"") and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by ""LastUpdatedDate"" desc";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }

            return list;
        }
        public async Task<List<IdNameViewModel>> GetProcessBookType(string templateCodes)
        {
            var list = new List<IdNameViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""CategoryName"" as Name,bk.""Id"" as Id
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

                list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            return list;
        }
        public async Task<List<IdNameViewModel>> GetProcessBook(string templateCodes)
        {
            var list = new List<IdNameViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as Name,bk.""Id"" as Id
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =ps.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and md.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

                list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            return list;
        }
        public async Task<List<IdNameViewModel>> GetBookAllPages(string bookid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bk.""ItemName"" as Name,bk.""Id"" as Id, s.""Id"" as Code, mp.""SequenceOrder"" as Count
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            --join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by mp.""SequenceOrder"" ";

            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetBookAllDirectPages(string bookid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bk.""ItemName"" as Name,bk.""Id"" as Id, s.""Id"" as Code
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk  
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId""  and bg.""Id""='{bookid}'  and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""IsDeleted"" = 'false'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";

            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetChildPageList(string bookid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bk.""ItemName"" as Name,bk.""Id"" as Id, s.""Id"" as Code, mp.""SequenceOrder"" as Count
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
             join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by s.""SequenceOrder"" ";
            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetBookByPageIdPages(string pageid)
        {
            var list = new List<IdNameViewModel>();

            var query = @$"select bg.""ProcessGroupName"" as Name,bg.""Id"" as Id, s.""Id"" as Code, mp.""SequenceOrder"" as Count
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""PageId""='{pageid}' and mp.""IsDeleted"" = 'false'
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by mp.""SequenceOrder"" ";

            list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<BookViewModel> GetBookDetail(string bookid)
        {
            var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""ProcessGroupDescription"" as BookDescription,c.""CategoryName"" as CategoryName, c.""Id""
            as CategoryId,bk.""ProcessGroupImage"" as BookImage,bk.""CreatedDate"",ou.""Name"" as CreatedBy,bk.""Id"" as Id,s.""SequenceOrder"" as SequenceOrder,s.""ServiceNo"" as ServiceNo,Item.Count as TotalPages
            ,Greatest(bk.""LastUpdatedDate"",Item.""LastUpdatedDate"") as ""LastUpdatedDate"",s.""Id"" as ""ServiceId""
            ,coalesce(r.""RatingCount"",0) as ""RatingCount"",coalesce(round(r.""Rating"",1),0) as Rating,coalesce(mr.""Rating"",0) as ""MyRating"",mr.""RatingComment"" as MyRatingComment,mr.""Id"" as ""MyRatingId""
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'  and ps.""CompanyId""='{_userContext.CompanyId}'
			             join cms.""N_SNC_TECProcess_ProcessCategory"" as c on ps.""UdfNoteId""=c.""NtsNoteId"" and c.""IsDeleted"" = 'false' and c.""CompanyId""='{_userContext.CompanyId}'

             left join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
            left join (
                select count(c.*) as Count,max(c.""LastUpdatedDate"") as ""LastUpdatedDate"",mp.""BookId"" as ""BookId"" from cms.""N_SNC_TECProcess_ProcessItem"" as c
                 join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=c.""Id""  and mp.""IsDeleted"" = 'false'
                   group by mp.""BookId""  
              
            ) as Item on ""BookId""=bk.""Id""
            left join (
                select avg(""Rating"") as ""Rating"",count(""Id"") as ""RatingCount"",""NtsId"" as ""NtsId"" from public.""NtsRating""
                where ""IsDeleted"" = false group by ""NtsId""
            ) as r on ""NtsId""=s.""Id""
            left join public.""NtsRating"" as mr on mr.""NtsId""=s.""Id"" and mr.""RatedByUserId""='{_userContext.UserId}' and mr.""IsDeleted"" = false
            where bk.""Id""='{bookid}' and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

            var data = await _queryRepo.ExecuteQuerySingle<BookViewModel>(query, null);
            return data;
        }
        public async Task<List<BookViewModel>> GetBookPageDetail(string bookid, string currentPageId)
        {
            var query = @$"Select bk.""File"" as ""File"",bk.""ContentType"" as ""ContentType"",bk.""HtmlContent"" as HtmlContent
            ,bk.""HtmlCssField"" as HtmlCssField, bk.""ItemName""  as PageName, bk.""ItemDescription"" as PageDescription 
            ,mp.""SequenceOrder"" as SequenceOrder,bk.""Id"" as ""Id"", s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
             join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'
            
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            -- join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";
            var list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            return list;
        }
        public async Task<List<BookViewModel>> GetAllCategoryByPermission(string templateCodes, string permission)
        {
            var list = new List<BookViewModel>();
            if (_userContext.IsSystemAdmin || (permission != null && permission.Contains("CanViewAllBooks")))
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            and tt.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else
            {
                var query = @$"select bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select distinct bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as cs on cs.""ParentServiceId""=s.""Id"" and cs.""IsDeleted"" = 'false' and cs.""CompanyId""='{_userContext.CompanyId}'            
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=cs.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
             select distinct bk.""CategoryName"" as CategoryName,bk.""Id"" as Id,s.""Id"" as ServiceId
            FROM cms.""N_SNC_TECProcess_ProcessCategory"" as bk
            join  public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
             join  public.""NtsService"" as cs on cs.""ParentServiceId""=s.""Id"" and cs.""IsDeleted"" = 'false' and cs.""CompanyId""='{_userContext.CompanyId}'   
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=cs.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            ";
                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            return list;
        }
        public async Task<List<BookViewModel>> GetGroupBookByCategoryPermission(string categoryId, string permission)
        {
            var list = new List<BookViewModel>();

            if (_userContext.IsSystemAdmin || (permission != null && permission.Contains("CanViewAllBooks")))
            {
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
              FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'
          
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' ";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            else
            {
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
             FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
              FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
            select distinct bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'          
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false 
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            union
             select distinct bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,s.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId
             FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""UdfNoteTableId"" ='{categoryId}' and ps.""IsDeleted"" = 'false'   
            join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_userContext.UserId}' and tu.""IsDeleted""=false  
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' 
            ";
                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            return list;
        }
        public async Task<List<BookViewModel>> GetBookAllPagesByBookId(string bookid)
        {
            var list = new List<BookViewModel>();
            var query = @$"select bk.""ItemName"" as BookName,bk.""ItemDescription"" as BookDescription,bk.""Id"" as Id, s.""Id"" as ServiceId, mp.""SequenceOrder"" as SequenceOrder,s.""ParentServiceId"" as ParentServiceId, bk.""HtmlContent"" as HtmlContent,bk.""HtmlCssField"" as HtmlCssField
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""BookId""='{bookid}' and mp.""IsDeleted"" = 'false'            
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            --join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on ps.""UdfNoteId""=bg.""NtsNoteId"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by mp.""SequenceOrder"" ";
            list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            return list;
        }
        public async Task<List<BookViewModel>> GetAllBookPages()
        {
            var list = new List<BookViewModel>();
            var query = @$"select bk.""ItemName"" as PageName,bg.""ProcessGroupName"" as BookName,bk.""Id"" as Id, s.""Id"" as ServiceId, s.""SequenceOrder"" as SequenceOrder,ps.""ParentServiceId"" as ParentServiceId,c.""CategoryName"" as CategoryName
            FROM cms.""N_SNC_TECProcess_ProcessItem"" as bk
            join cms.""F_TECProcess_BookPageMapping"" as mp on mp.""PageId""=bk.""Id"" and mp.""IsDeleted"" = 'false'        
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            --join public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false' and ps.""CompanyId""='{_userContext.CompanyId}'
            join cms.""N_SNC_TECProcess_ProcessGroup"" as bg on mp.""BookId""=bg.""Id"" and bg.""IsDeleted"" = 'false' and bg.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as gs on gs.""Id""=bg.""UdfNoteTableId"" and gs.""IsDeleted"" = 'false'
            join  public.""NtsService"" as pgs on pgs.""Id""=gs.""ParentServiceId"" and pgs.""IsDeleted"" = 'false'
            join  cms.""N_SNC_TECProcess_ProcessCategory"" as c on c.""NtsNoteId""=pgs.""UdfNoteId"" and c.""IsDeleted"" = 'false'
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}' order by s.""SequenceOrder"" ";

            list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            return list;
        }
        public async Task<List<BookViewModel>> GetAllProcessBook(string templateCodes)
        {
            var list = new List<BookViewModel>();
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                var query = @$"select bk.""ProcessGroupName"" as BookName,bk.""Id"" as Id,s.""Id"" as ServiceId,c.""CategoryName"" as CategoryName
            FROM cms.""N_SNC_TECProcess_ProcessGroup"" as bk
            join public.""NtsService"" as s on s.""UdfNoteId""=bk.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
            join  public.""NtsService"" as ps on ps.""Id""=s.""ParentServiceId"" and ps.""IsDeleted"" = 'false'
            join  cms.""N_SNC_TECProcess_ProcessCategory"" as c on c.""NtsNoteId""=ps.""UdfNoteId"" and c.""IsDeleted"" = 'false'
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""='false' and tt.""CompanyId""='{_userContext.CompanyId}'
            join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and tt.""Code"" in ('{templateCodes}')
            where bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'";

                list = await _queryRepo.ExecuteQueryList<BookViewModel>(query, null);
            }
            return list;
        }
        public async Task<List<BookRealtionViewModel>> GetAllBookRelationBySourceId(string sourceId)
        {
            var list = new List<BookRealtionViewModel>();
            var query = @$"select *
                            FROM cms.""N_SNC_TECProcess_BookRelation"" as bk
                            where bk.""SourceTableId""='{sourceId}' and bk.""IsDeleted"" = 'false' and bk.""CompanyId""='{_userContext.CompanyId}'  ";
            list = await _queryRepo.ExecuteQueryList<BookRealtionViewModel>(query, null);
            return list;
        }
        public async Task DeleteBookPageMapping(string bookId, string pageId)
        {
            var query = $@"Update cms.""F_TECProcess_BookPageMapping"" set ""IsDeleted""=true where ""BookId""='{bookId}' and ""PageId""='{pageId}'";
            await _queryRepo.ExecuteCommand(query, null);
        }
        #endregion

        public async Task<List<TableMetadataViewModel>> ManageBulkNoteData(string id)
        {
            var tquery = @$"select tm.*,t.""Id"" as TemplateId from public.""TableMetadata"" as tm
                            join public.""Template"" as t on t.""TableMetadataId""=tm.""Id""
                            where t.""Id"" in ('{id}') limit 1";
            var tableMetaDatas = await _queryRepo.ExecuteQueryList<TableMetadataViewModel>(tquery, null);
            return tableMetaDatas;


        }

        public async Task<List<ColumnMetadataViewModel>> GetUdfQueryData(string categoryId, string categoryCode)
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
            return columnList;


        }
        public async Task<List<TableMetadataViewModel>> ValidateBulkNoteData(string id)
        {
            var tquery = @$"select tm.*,t.""Id"" as TemplateId from public.""TableMetadata"" as tm
                            join public.""Template"" as t on t.""TableMetadataId""=tm.""Id""
                            where t.""Id"" in ('{id}') limit 1";
            var tableMetaDatas = await _queryRepo.ExecuteQueryList<TableMetadataViewModel>(tquery, null);
            return tableMetaDatas;
        }
        public async Task<bool> InsertIntoLogTableData(TableMetadataViewModel tableMetaData)

        {
            var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                (
	                select 1
	                from information_schema.tables
	                where table_schema = '{ApplicationConstant.Database.Schema.Log}'
	                and table_name = '{tableMetaData.Name}Log'
                ) ", null);
            return tableExists;
        }
        public async Task<bool> createLogTableData(TableMetadataViewModel tableMetadata)

        {
            var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                (
	                select 1
	                from information_schema.tables
	                where table_schema = 'log'
	                and table_name = '{tableMetadata.Name}Log'
                ) ", null);
            return tableExists;
        }
        public async Task<List<TableMetadataViewModel>> BulkInsertNoteUdfTableData(string tempid)
        {
            var tquery = @$"select tm.*,t.""Id"" as TemplateId from public.""TableMetadata"" as tm
                            join public.""Template"" as t on t.""TableMetadataId""=tm.""Id""
                            where t.""Id"" in ('{tempid}') limit 1";
            var tableMetaDatas = await _queryRepo.ExecuteQueryList<TableMetadataViewModel>(tquery, null);
            return tableMetaDatas;
        }
        public async Task<bool> EditUdfTableLogData(TableMetadataViewModel tableMetaData)
        {
            var tableExists = await _queryRepo.ExecuteScalar<bool>(@$"select exists 
                (
	                select 1
	                from information_schema.tables
	                where table_schema = '{ApplicationConstant.Database.Schema.Log}'
	                and table_name = '{tableMetaData.Name}Log'
                ) ", null);
            return tableExists;
        }
        public async Task CreateAttachmentsData(NoteTemplateViewModel model, string attachments)
        {
            var query = @$"update public.""File"" set ""ReferenceTypeCode""='Note',""ReferenceTypeId""='{model.NoteId}'
            where ""Id"" in ('{attachments}')";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<TableMetadataViewModel> GetNoteTemplateByIdData(NoteTemplateViewModel viewModel)
        {
            var query = @$"select ""TM"".* from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""TableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsNote"" as ""N"" on ""T"".""Id""=""N"".""TemplateId"" and ""N"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false   and ""N"".""Id""='{viewModel.NoteId}'";
            var tableMetadata = await _queryRepo.ExecuteQuerySingle<TableMetadataViewModel>(query, null);
            return tableMetadata;

        }
        public async Task<NoteTemplateViewModel> GetNoteTemplateForNewNoteData(NoteTemplateViewModel vm)
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
            return data;
        }
        public async Task<long?> GenerateNextNoteNoData()
        {
            var query = $@"update public.""NtsNoteSequence"" SET ""NextId"" = ""NextId""+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""SequenceDate"" = '{DateTime.Today.ToDatabaseDateFormat()}'
            RETURNING ""NextId""-1";
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            return nextId;
        }
        public async Task<long?> GenerateNextNoteNoData(long count)
        {
            var query = $@"update public.""NtsNoteSequence"" SET ""NextId"" = ""NextId""+{count},
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""SequenceDate"" = '{DateTime.Today.ToDatabaseDateFormat()}'
            RETURNING ""NextId""-{count}";
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            return nextId;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableColumnsData(TableMetadataViewModel tableMetaData)
        {
            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"",
            case when t.""Schema""='{tableMetaData.Schema}' and t.""Name""='{tableMetaData.Name}' then true else false end as ""IsPrimaryTableColumn""
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
            return pkColumns;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableColumnsData2(TableMetadataViewModel tableMetaData)
        {
            var query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
                ,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
                ,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false 
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false 
                where c.""TableMetadataId""='{tableMetaData.Id}'
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  and c.""IsUdfColumn""=true 
                and (fc.""IsSystemColumn""=false or fc.""IsPrimaryKey""=true)";
            var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return result;
        }
        public async Task<List<ColumnMetadataViewModel>> GetNoteViewableColumnsData()
        {

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
            return pkColumns;
        }
        public async Task<NoteTemplateViewModel> GetSelectQueryData(TableMetadataViewModel tableMetaData)
        {
            var noteTemlateViewModel = await _queryRepo.ExecuteQuerySingle<NoteTemplateViewModel>
        (@$"select nt.* from public.""NoteTemplate"" nt 
                join public.""Template"" t on nt.""TemplateId""=t.""Id"" and t.""IsDeleted""=false 
                where nt.""IsDeleted""=false and t.""TableMetadataId""='{tableMetaData.Id}' ", null);
            return noteTemlateViewModel;

        }
        public async Task<string> GetNoteIndexPageCountData(PageViewModel page)
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
            return query;
        }
        public async Task<List<NtsTaskSharedViewModel>> GetNoteIndexPageGridData(string userId)
        {
            var sharedwithquery = $@" Select ts.* from public.""NtsNoteShared"" as ts where ts.""SharedWithUserId""='{userId}' and ts.""IsDeleted""=false  ";
            var sharedwithlist = await _queryRepo.ExecuteQueryList<NtsTaskSharedViewModel>(sharedwithquery, null);
            return sharedwithlist;
        }
        public async Task<List<NtsNoteSharedViewModel>> GetNoteIndexPageGridData1(string userId)
        {
            var sharedbyquery = $@" Select ts.* from public.""NtsNoteShared"" as ts where ts.""SharedByUserId""='{userId}' and ts.""IsDeleted""=false  ";
            var sharedbylist = await _queryRepo.ExecuteQueryList<NtsNoteSharedViewModel>(sharedbyquery, null);
            return sharedbylist;
        }
        public async Task<List<UserViewModel>> SetsharedListData(NoteTemplateViewModel model)
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
        }
        public async Task<List<NTSMessageViewModel>> GetNoteMessageListData(string userId, string noteId)
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
            return comments;
        }
        public async Task<List<NoteTemplateViewModel>> GetNtsNoteIndexPageCountData(NtsNoteIndexPageViewModel model)
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
            return result;
        }
        public async Task<IList<NoteViewModel>> GetNtsNoteIndexPageGridData(NtsActiveUserTypeEnum ownerType, string noteStatusCode, string categoryCode, string templateCode, string moduleCode, NtsViewTypeEnum? ntsViewType)
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
            var where = @$" WHERE 1=1 and t.""PortalId""='{_userContext.PortalId}'";
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
        public async Task<List<NoteViewModel>> NotesDashboardCountData(string userId, string type = "ALL", string moduleName = null, string noteTemplateIds = null)
        {
            var cypher = $@"Select ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
                            from public.""NtsNote"" as s                         
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
                            join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""Id""='{userId}'  and u.""IsDeleted""=false 
                            join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
                           left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
                           where 1=1 and s.""PortalId""='{_userContext.PortalId}' and s.""IsDeleted""=false  #WHERE# #NOTETEMPLATESEARCH#";

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
            return note;
        }
        public async Task<List<NoteViewModel>> NotesDashboardCountData1(string userId, string type = "ALL", string moduleName = null, string noteTemplateIds = null)
        {


            var cypher1 = $@"Select s.""Id"" as Id,ns.""Code"" as NoteStatusCode,u.""Id"" as OwnerUserId
from public.""NtsNote"" as s
join public.""NtsNoteShared"" as ss on ss.""NtsNoteId""=s.""Id""  and ss.""IsDeleted""=false 
--join public.""LOV"" as sst on sst.""Id""=ss.""NoteharedWithTypeId"" and sst.""LOVType""='SHARED_TYPE' and sst.""Code""='SHARED_TYPE_USER' and sst.""IsDeleted""=false 
join public.""User"" as u on u.""Id""=ss.""SharedWithUserId""  and u.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false 
where u.""Id""='{userId}' and s.""PortalId""='{_userContext.PortalId}' and s.""IsDeleted""=false  #WHERE# #NOTETEMPLATESEARCH#

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
where u.""Id""='{userId}' and s.""PortalId""='{_userContext.PortalId}' and u.""IsDeleted""=false  #WHERE# #NOTETEMPLATESEARCH# ";

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

            cypher1 = cypher1.Replace("#WHERE#", search);
            cypher1 = cypher1.Replace("#NOTETEMPLATESEARCH#", noteTemplatesearch);
            var noteshared = await _queryRepo.ExecuteQueryList(cypher1, null);
            return noteshared;
        }
        public async Task<List<NoteViewModel>> NotesDashboardCountData2(string userId, string type = "ALL", string moduleName = null, string noteTemplateIds = null)
        {
            var cypher2 = $@"Select distinct ns.""Code"" as NoteStatusCode,s.""Id"" as Id
from public.""User"" as u
join public.""NtsNote"" as s on s.""OwnerUserId""=u.""Id"" and s.""IsDeleted""=false  
join public.""NtsNoteShared"" as ss on ss.""CreatedBy""=u.""Id"" and s.""Id""=ss.""NtsNoteId"" and ss.""IsDeleted""=false 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
join public.""LOV"" as ns on ns.""Id""=s.""NoteStatusId""  and ns.""IsDeleted""=false 
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
where u.""Id""='{userId}' and u.""IsDeleted""=false  and s.""PortalId""='{_userContext.PortalId}' #WHERE#";

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
            cypher2 = cypher2.Replace("#WHERE#", search);
            cypher2 = cypher2.Replace("#NOTETEMPLATESEARCH#", noteTemplatesearch);
            var notesharebyme = await _queryRepo.ExecuteQueryList(cypher2, null);
            return notesharebyme;
        }
        public async Task<List<NoteViewModel>> GetSearchResultData(NoteSearchViewModel searchModel)
        {
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

            if (searchModel.Mode.IsNotNullAndNotEmpty())
            {
                var modes = searchModel.Mode.Split(',');
                if (modes.Any(y => y == "REQ_BY"))
                    where += @$" and (t.""OwnerUserId""='{searchModel.UserId}')";
                if (modes.Any(y => y == "SHARE_TO"))
                    where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false and ts.""SharedWithUserId""='{searchModel.UserId}' ) ) ";
                if (modes.Any(y => y == "ASSIGN_BY"))
                    where += @$" and (t.""Id"" in ( Select ts.""NtsNoteId"" from public.""NtsNoteShared"" as ts where ts.""IsDeleted""=false and ts.""SharedByUserId""='{searchModel.UserId}' ) ) ";

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


            query = query.Replace("#WHERE#", where);
            var querydata = await _queryRepo.ExecuteQueryList(query, null);
            return querydata;
        }
        public async Task RollBackData1(NoteTemplateViewModel model)
        {
            var table = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(model.TableMetadataId);
            if (table != null)
            {
                var query = @$"delete from {table.Schema}.""{ table.Name}"" where ""Id""='{model.UdfNoteTableId}';
                    delete from public.""NtsNote"" where ""Id""='{model.NoteId}';";
                await _queryRepo.ExecuteCommand(query, null);
            }
        }
        public async Task<List<SynergySchemaViewModel>> GetSyneryListData()
        {
            var query = $@"select S.""SchemaName"",S.""Query"",S.""Metadata"",case when (S.""ElsasticDB"" is null or S.""ElsasticDB""='False') then false else true end as ElsasticDB,N.""Id"" as Id, N.""Id"" as NoteId,u.""Name"" as CreatedBy from  ""NtsNote""  N 
                    inner join cms.""N_BusinessAnalytics_SynergySchema"" S on S.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false 
                    left join public.""User"" as u on u.""Id""=N.""CreatedBy"" and u.""IsDeleted""=false 
                    where S.""IsDeleted""= false";

            var result = await _queryRepo.ExecuteQueryList<SynergySchemaViewModel>(query, null);
            return result;
        }
        public async Task<DateTime> GetLastUpdatedSynerySchemaData()
        {
            var query = $@"select cn.""LastUpdatedDate"" as LastUpdatedDate 
                    from  ""NtsNote""  N 
                    inner join cms.""N_BusinessAnalytics_SynergySchema"" S on S.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false
                    inner join ""NtsNote""  cn on cn.""ParentNoteId""=N.""Id""
                    where S.""IsDeleted""= false order by cn.""LastUpdatedDate"" desc limit 1";

            var result = await _queryRepo.ExecuteScalar<DateTime>(query, null);
            return result;
        }
        public async Task<SynergySchemaViewModel> GetSynerySchemaByIdData(string Id)
        {
            var query = $@"select S.""SchemaName"",S.""Query"",S.""RefreshKey"",S.""Metadata"",case when (S.""ElsasticDB"" is null or S.""ElsasticDB""='False') then false else true end as ElsasticDB ,N.""Id"" as Id, N.""Id"" as NoteId from  ""NtsNote""  N inner join cms.""N_BusinessAnalytics_SynergySchema"" S on S.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false where S.""IsDeleted""= false and N.""Id""='{Id}'";


            var result = await _queryRepo.ExecuteQuerySingle<SynergySchemaViewModel>(query, null);
            return result;
        }
        public async Task<bool> DeleteSchemaData(string NoteId)
        {

            var query = $@"update cms.""N_BusinessAnalytics_SynergySchema"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<IList<NoteViewModel>> GetAllDashboardMasterData()
        {
            var query = @$" select n.* 
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_DashboardMaster"" as dm on dm.""NtsNoteId""=n.""Id"" and (dm.""gridStack""='False' or dm.""gridStack"" is null) and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_DashboardMaster' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList(query, null);
            return querydata;
        }
        public async Task<IList<DashboardMasterViewModel>> GetAllGridStackDashboardData()
        {
            var query = @$" select n.*,dm.""layoutMetadata"" as ""layoutMetadata"" 
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false  and n.""ParentNoteId"" is null
                        join cms.""N_CoreHR_DashboardMaster"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""gridStack""='True' and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_DashboardMaster' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<DashboardMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<KanbanBoardViewModel>> GetKanbanBoardData()
        {
            var query = @$" select n.*,dm.""boards"" as ""boards"" ,dm.""kanbanTemplate"" as kanbanTemplate
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_BusinessAnalytics_KanbanBoard"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Name""='N_BusinessAnalytics_KanbanBoard' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<KanbanBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<KanbanBoardViewModel> GetKanbanBoardDetailsData(string noteId)
        {
            var query = @$" select dm.""boards"" as ""boards"",dm.""kanbanTemplate"" as kanbanTemplate
                        from cms.""N_BusinessAnalytics_KanbanBoard"" as dm                         
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<KanbanBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<DashboardMasterViewModel> GetDashboardMasterDetailsData(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""layoutMetadata"" as layoutMetadata                         
                        from cms.""N_CoreHR_DashboardMaster"" as dm 
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardMasterViewModel>(query, null);
            return querydata;
        }
        public async Task UpdateModelData(string data, string id)
        {
            var updateQuery = $@"Update cms.""N_CoreHR_DashboardMaster"" set ""layoutMetadata"" = '{data}' where ""NtsNoteId"" = '{id}' and ""IsDeleted"" = false ";
            await _queryRepo.ExecuteCommand(updateQuery, null);

        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemMasterDetailsData(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId, dm.""chartMetadata"" as chartMetadata,
                        dm.""measuresField"" as measuresField,dm.""dimensionsField"" as dimensionsField,dm.""segmentsField"" as segmentsField,
                        dm.""filterField"" as filterField,dm.""mapUrl"" as mapUrl,dm.""mapLayer"" as mapLayer,ct.""Help"" as Help,
                        dm.""ThemeMode"" as ThemeMode,dm.""Palette"" as Palette,dm.""MonocromeColor"" as MonocromeColor,
                        dm.""onChartClickFunction"" as onChartClickFunction,dm.""DynamicMetadata"" as DynamicMetadata,dm.""isLibrary"" as isLibrary,
                        dm.""Xaxis"" as Xaxis,dm.""Yaxis"" as Yaxis,dm.""Count"" as Count,dm.""timeDimensionsField"" as timeDimensionsField,
                        dm.""filters"" as filters,dm.""timeField"" as timeField,dm.""granularity"" as granularity,dm.""rangeType"" as rangeType
                        from cms.""N_CoreHR_DashboardItem"" as dm 
                        left join cms.""N_CoreHR_ChartTemplate"" as ct on ct.""NtsNoteId""=dm.""chartTypeId"" and ct.""IsDeleted""=false
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardItemMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<MapLayerItemViewModel> GetMapLayerItemDetailsData(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""MapUrl"" as ""MapUrl"", dm.""MapLayer"" as ""MapLayer"",
                        dm.""MapTransparency"" as ""MapTransparency"", dm.""MapFormat"" as ""MapFormat"",
                        dm.""MapOpacity"" as ""MapOpacity"", dm.""IsBaseMap"" as ""IsBaseMap""
                        from cms.""N_BusinessAnalytics_MapLayerItem"" as dm                        
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<MapLayerItemViewModel>(query, null);
            return querydata;
        }
        public async Task<List<MapLayerItemViewModel>> GetMapLayerItemListData(string parentNoteId)
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
        public async Task<WidgetItemViewModel> GetWidgetItemDetailsData(string noteId)
        {
            var query = @$" select dm.""NtsNoteId"" as NoteId, dm.""keyword"" as keyword, dm.""socialMediaType"" as socialMediaType,
                        dm.""from"" as from,dm.""to"" as to,dm.""height"" as height,dm.""width"" as width,dm.""chartMetadata"" as chartMetadata,dm.""chartTypeId"" as chartTypeId
                        from cms.""N_SWS_WidgetItem"" as dm 
                        where dm.""NtsNoteId""='{noteId}' and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WidgetItemViewModel>(query, null);
            return querydata;
        }
        public async Task<List<WidgetItemViewModel>> GetWidgetItemListData(string parentId)
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
        public async Task<WidgetItemViewModel> GetWidgetItemDetailsByNameData(string name)
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
        public async Task<List<DashboardItemMasterViewModel>> GetDashboardItemMasterListData(string parentId)
        {
            var query = @$" select m.*,dm.""NtsNoteId"" as NoteId, dm.""chartTypeId"" as chartTypeId, dm.""chartMetadata"" as chartMetadata,
                        dm.""measuresField"" as measuresField,dm.""dimensionsField"" as dimensionsField,dm.""segmentsField"" as segmentsField,
                        dm.""filterField"" as filterField,dm.""mapUrl"" as mapUrl,dm.""mapLayer"" as mapLayer,dm.""Help"" as Help,
                        dm.""ThemeMode"" as ThemeMode,dm.""Palette"" as Palette,dm.""MonocromeColor"" as MonocromeColor,
                        dm.""Xaxis"" as Xaxis,dm.""Yaxis"" as Yaxis,dm.""Count"" as Count,c.""NoteSubject"" as ChartName
                        from cms.""N_CoreHR_DashboardItem"" as dm 
                        join public.""NtsNote"" as m on m.""Id""=dm.""NtsNoteId"" and m.""IsDeleted""=false 
                        join public.""NtsNote"" as c on c.""Id""=dm.""chartTypeId"" and c.""IsDeleted""=false 
                        where m.""ParentNoteId""='{parentId}' and dm.""IsDeleted""=false 
                        order by m.""CreatedDate"" ";
            var querydata = await _queryRepo.ExecuteQueryList<DashboardItemMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsWithChartTemplateData(string noteId)
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
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsByNameData(string name)
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
        public async Task<DashboardItemMasterViewModel> GetDashboardItemDetailsByIdData(string id)
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
        public async Task<IList<IdNameViewModel>> GetAllChartTemplateData()
        {
            var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as Name,dm.""Help"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_ChartTemplate' and t.""IsDeleted""=false   order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetChartTemplateByIdData(string id)
        {
            var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as Name,dm.""boilerplateCode"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_ChartTemplate' and n.""Id""='{id}' and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetAllDashboardItemDetailsWithDashboardData()
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
        public async Task<List<IdNameViewModel>> GetLibraryDashboardItemDetailsWithDashboardData()
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
        public async Task<List<IdNameViewModel>> GetTagIdNameListData(string portalId)
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
        public async Task<List<TagCategoryViewModel>> GetTagCategoryListData(string TemplateId)
        {
            var query = @$"  select tg.""TagCategoryName"" as Name,n.""Id"" as Id,temp.""Id"" as TemplateId
from public.""Template"" as temp
join public.""NtsNote"" as n on n.""Id"" = Any(temp.""AllowedTagCategories"")  and n.""IsDeleted""=false 
join cms.""N_General_TagCategory"" as tg on tg.""NtsNoteId""=n.""Id""  and tg.""IsDeleted""=false 
where temp.""Id""='{TemplateId}' and temp.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TagCategoryViewModel>> GetTagListByCategoryIdData(string CategoryId)
        {
            var query = @$" select n.""NoteSubject""  as Name,n.""Id"" as Id               
                        from public.""NtsNote"" as n
                        join cms.""N_General_Tag"" as tg on tg.""NtsNoteId""=n.""Id""  and tg.""IsDeleted""=false  
                        where  n.""IsDeleted""=false  and n.""ParentNoteId""='{CategoryId}'";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<TagCategoryViewModel> GetCategoryByTagIdData(string tagId)
        {
            var query = @$" select n.""NoteSubject""  as Name,n.""Id"" as Id               
                        from public.""NtsNote"" as n
                       join public.""Template"" as m on n.""TemplateId""=m.""Id""   and m.""IsDeleted""=false      
                        join public.""NtsNote"" as tg on tg.""ParentNoteId""=n.""Id""  and tg.""IsDeleted""=false      
                        where  n.""IsDeleted""=false  and m.""Code""='TAG_CATEGORY' and tg.""Id""='{tagId}'";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<NoteViewModel>> GetNoteDataListByTemplateCodeData(string templateCode)
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
        public async Task<IList<DirectoryContent>> GetAllDocumentsData(string parentId)
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
        public async Task<List<DirectoryContent>> GetAllChildDocumentsData(string parentId)
        {
            string query = $@"  select ""Id"" as id,""NoteSubject"" as name,""ParentNoteId"" as parentId,""CreatedDate"" as dateCreated,true as hasChild,""TemplateCode"" as TemplateCode
                            
                                from public.""NtsNote""                               
                                where ""ParentNoteId"" = '{parentId}' and ""IsDeleted""=false  and ""IsArchived""=false
                            ";



            var queryData = await _queryRepo.ExecuteQueryList<DirectoryContent>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<DirectoryContent>> GetAllFolderDocumentsData(string parentId)
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
        public async Task<TemplateViewModel> GetAllDocumentFilesData(string documentId)
        {
            string query = $@"select t.* from public.""NtsNote"" as n
                            join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false 
                            where n.""Id""= '{documentId}' and n.""IsDeleted""=false  and n.""IsArchived""=false
                            ";
            var tableName = await _queryRepo.ExecuteQuerySingle<TemplateViewModel>(query, null);
            return tableName;
        }
        public async Task<DirectoryContent> GetAllDocumentFilesData1(string field, string tableName, string documentId)
        {
            string query1 = $@"select d.""{field}"" as id,f.""FileName"" as name,n.""Id"" as parentId,'File' as FolderType,'0' as Count,
                            d.""CreatedDate"" as dateCreated,d.""LastUpdatedDate"" as dateModified,true as hasChild,'FILE' as TemplateCode
                            ,n.""NoteNo"" as NoteNo,u.""Name"" as CreatedBy
                            from cms.""{tableName}""  as d
                            join public.""NtsNote"" as n on n.""Id""=d.""NtsNoteId"" and n.""IsDeleted""=false 
                            left join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false
                            left join public.""File"" as f on f.""Id""=d.""{field}"" and f.""IsDeleted""=false 
                            where n.""Id""= '{documentId}' and d.""IsDeleted""=false 
                            ";
            var queryData = await _queryRepo.ExecuteQuerySingle<DirectoryContent>(query1, null);
            return queryData;
        }
        public async Task<bool> IsNoteSubjectUniqueData(string templateId, string subject, string noteId)
        {
            var query = $@"select * from public.""NtsNote"" 
where ""TemplateId""='{templateId}' and  LOWER(""NoteSubject"")=LOWER('{subject}') and ""Id""<> '{noteId}' and ""IsDeleted""=false  limit 1";
            var data = await _queryRepo.ExecuteQuerySingle(query, null);
            return true;
        }
        public async Task<IList<SocialWebsiteViewModel>> GetAllSocialWebsiteData()
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
        public async Task<DbConnectionViewModel> GetDbConnectionDetailsData(string noteId)
        {
            var query = @$" select db.""NtsNoteId"" as NoteId,db.""hostName"" as hostName,db.""port"" as port,db.""maintenanceDatabase"" as maintenanceDatabase,
                        db.""username"" as username,db.""role"" as role,db.""service"" as service,db.""password"" as password,db.""databaseType"" as databaseType
                        from cms.""N_SWS_DbConnection"" as db 
                        where db.""NtsNoteId""='{noteId}' and db.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DbConnectionViewModel>(query, null);
            return querydata;
        }
        public async Task<ApiConnectionViewModel> GetApiConnectionDetailsData(string noteId)
        {
            var query = @$" select db.""NtsNoteId"" as NoteId,db.""restApiUrl"" as restApiUrl,db.""parameters"" as parameters,db.""userName"" as userName,
                        db.""password"" as password,db.""apiKey"" as apiKey,db.""pollingTime"" as pollingTime
                        from cms.""N_SWS_ApiConnection"" as db 
                        where db.""NtsNoteId""='{noteId}' and db.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<ApiConnectionViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetSharedListData(string NoteId)
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
        public async Task UpdateScheduleSyncData(string id, DateTime trackingDate, string content)
        {
            var query = @$"UPDATE cms.""N_SWS_SchedulerSyncMaster"" 
                           set ""trackingDate""='{trackingDate}',""logstashContent""='{content}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_SchedulerSyncMaster"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<RssFeedViewModel> GetRssFeedDetailsData(string noteId)
        {
            var query = @$" select db.""NtsNoteId"" as NoteId,db.""feedName"" as feedName,db.""feedUrl"" as feedUrl
                        from cms.""N_SWS_RssFeedMaster"" as db 
                        where db.""NtsNoteId""='{noteId}' and db.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<RssFeedViewModel>(query, null);
            return querydata;
        }
        public async Task<AlertViewModel> GetNotificationALertDetailsData(string noteId)
        {
            var query = @$" select m.*,n.""queryTableId"" as QueryTableId,n.""query"" as Query,
                        n.""conditionFunction"" as ConditionFunction,n.""conditionType"" as ConditionType,
                        n.""conditionValue"" as ConditionValue,n.""evaluateTime"" as EvaluateTime,
                        n.""summary"" as Summary,n.""colorCode"" as ColorCode,n.""cubeJsFilter"" as CubeJsFilter,
                        n.""queryColumns"" as queryColumns,n.""columnReferenceId"" as columnReferenceId,
                        n.""timeDimensionId"" as timeDimensionId,n.""timeDimensionDuration"" as timeDimensionDuration,
                        n.""timeDimensionFilter"" as timeDimensionFilter,n.""granularity"" as granularity,
                        n.""chartTypeId"" as chartTypeId,n.""userRole"" as userRole,
                        case when (n.""isReporting"" is null or n.""isReporting""='False') then false else true end as isReporting,
                        n.""groupbyColumns"" as groupbyColumns,n.""frequency"" as frequency,n.""limit"" as limit,n.""groupFilters"" as groupFilters
                        from cms.""N_SOCIAL_SCRAPPING_NotificationAlert"" as n 
                        join public.""NtsNote"" as m on m.""Id""=n.""NtsNoteId"" and m.""IsDeleted""=false 
                        where n.""NtsNoteId""='{noteId}' and n.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<AlertViewModel>(query, null);
            return querydata;
        }
        public async Task<WatchlistViewModel> GetWatchlistDetailsData(string noteId)
        {
            var query = @$" select case when (n.""isAdvance"" is null or n.""isAdvance""='False') then false else true end as isAdvance,
                        case when (n.""plainSearch"" is null or n.""plainSearch""='False') then false else true end as plainSearch,
                        n.""dateFilterType"" as dateFilterType,n.""startDate"" as startDate,n.""endDate"" as endDate                        
                        from cms.""N_SWS_SocialMediaHighlight"" as n 
                        join public.""NtsNote"" as m on m.""Id""=n.""NtsNoteId"" and m.""IsDeleted""=false 
                        where n.""NtsNoteId""='{noteId}' and n.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WatchlistViewModel>(query, null);
            return querydata;
        }
        public async Task<List<AlertViewModel>> GetAlertRulelistData()
        {
            var query = @$" select m.*,n.""queryTableId"" as QueryTableId,n.""query"" as Query,
                        n.""conditionFunction"" as ConditionFunction,n.""conditionType"" as ConditionType,
                        n.""conditionValue"" as ConditionValue,n.""evaluateTime"" as EvaluateTime,
                        n.""summary"" as Summary,n.""colorCode"" as ColorCode,n.""cubeJsFilter"" as CubeJsFilter,
                        n.""queryColumns"" as queryColumns,n.""columnReferenceId"" as columnReferenceId,
                        n.""timeDimensionId"" as timeDimensionId,n.""timeDimensionDuration"" as timeDimensionDuration,
                        n.""timeDimensionFilter"" as timeDimensionFilter,n.""granularity"" as granularity,
                        n.""chartTypeId"" as chartTypeId,n.""userRole"" as userRole,
                        case when (n.""isReporting"" is null or n.""isReporting""='False') then false else true end as isReporting,
                        n.""groupbyColumns"" as groupbyColumns,n.""frequency"" as frequency,n.""limit"" as limit,n.""groupFilters"" as groupFilters
                        from cms.""N_SOCIAL_SCRAPPING_NotificationAlert"" as n 
                        join public.""NtsNote"" as m on m.""Id""=n.""NtsNoteId"" and m.""IsDeleted""=false 
                        where n.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<AlertViewModel>(query, null);
            return querydata;
        }
        public async Task<List<string>> GetKeywordListByTrackIdData(string noteId)
        {
            var query = @$" select m.""NoteSubject"" from cms.""N_SWS_TrackMaster"" as t
                        join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false  and n.""Id""='{noteId}'
                        join cms.""N_SWS_Keyword"" as k on k.""track""=t.""Id"" and k.""IsDeleted""=false 
                        join public.""NtsNote"" as m on m.""Id""=k.""NtsNoteId"" and m.""IsDeleted""=false 
                         where and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteScalarList<string>(query, null);
            return querydata;
        }
        public async Task<List<string>> GetCanAlertKeywordListByTrackIdData(string noteId)
        {
            var query = @$" select m.""NoteSubject"" from cms.""N_SWS_TrackMaster"" as t
                        join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false  and n.""Id""='{noteId}'
                        join cms.""N_SWS_Keyword"" as k on k.""track""=t.""Id"" and k.""canAlert""='True' and k.""IsDeleted""=false 
                        join public.""NtsNote"" as m on m.""Id""=k.""NtsNoteId"" and m.""IsDeleted""=false 
                         where t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteScalarList<string>(query, null);
            return querydata;
        }
        public async Task<IList<MapMarkerViewModel>> GetDistictByStateData(string stateId)
        {
            var query = @$"SELECT d.""Id"" as Id, n.""NoteSubject"" as Name,d.""pinLatitude"" as Latitude,d.""resultCount"" as Count,
                        d.""pinLongitude"" as Longitude, 'Madhya Pradesh' as State FROM cms.""N_SWS_District"" as d 
                        left join public.""NtsNote"" as n on n.""Id"" = d.""NtsNoteId"" and n.""IsDeleted""=false 
                        where  d.""state"" = '{stateId}'  and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<MapMarkerViewModel>> GetPoliceStationByDistictData(string districtId)
        {
            var query = @$"SELECT d.""Id"" as Id, n.""NoteSubject"" as Name,d.""pinLatitude"" as Latitude,d.""resultCount"" as Count,
                        d.""pinLongitude"" as Longitude, 'Madhya Pradesh' as State FROM cms.""N_SWS_PoliceStation"" as d 
                        left join public.""NtsNote"" as n on n.""Id"" = d.""NtsNoteId"" and n.""IsDeleted""=false 
                        where  d.""district"" = '{districtId}'  and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<MapMarkerViewModel>> GetLocationByPoliceStationData(string policeStationId)
        {
            var query = @$"SELECT d.""Id"" as Id, n.""NoteSubject"" as Name,d.""pinLatitude"" as Latitude,d.""resultCount"" as Count,
                        d.""pinLongitude"" as Longitude, 'Madhya Pradesh' as State FROM cms.""N_SWS_Location"" as d 
                        left join public.""NtsNote"" as n on n.""Id"" = d.""NtsNoteId"" and n.""IsDeleted""=false 
                        where  d.""policeStation"" = '{policeStationId}'  and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MapMarkerViewModel>(query, null);
            return querydata;
        }
        public async Task UpdateLocationData(string id, string count)
        {
            var query = @$"UPDATE cms.""N_SWS_Location"" 
                           set ""resultCount""='{count}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_Location"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}' ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdatePoliceStationData(string id, string count)
        {
            var query = @$"UPDATE cms.""N_SWS_PoliceStation"" 
                           set ""resultCount""='{count}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_PoliceStation"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateDistrictData(string id, string count)
        {
            var query = @$"UPDATE cms.""N_SWS_District"" 
                           set ""resultCount""='{count}'
                        from public.""NtsNote"" as n 
                        where  n.""Id"" = cms.""N_SWS_District"".""NtsNoteId"" 
                        and  n.""Id"" = '{id}'";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<IList<MapMarkerViewModel>> GetMarkersByDistrictData(string locationId)
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
        public async Task<List<TagCategoryViewModel>> GetTrackListData()
        {
            var query = @$"SELECT tm.""Id"" as Id, n.""NoteSubject"" as Name FROM cms.""N_SWS_TrackMaster"" as tm
                            join public.""NtsNote"" as n on n.""Id"" = tm.""NtsNoteId"" and n.""IsDeleted""=false 
                            where tm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TagCategoryViewModel>> GetKeywordListData(string trackId)
        {
            var query = @$"SELECT tm.""Id"" as Id, n.""NoteSubject"" as Name FROM cms.""N_SWS_Keyword"" as tm
                            join public.""NtsNote"" as n on n.""Id"" = tm.""NtsNoteId"" and n.""IsDeleted""=false 
                            where tm.""track"" = '{trackId}' and tm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<List<NoteViewModel>> GetDocumentByNoteAndRevisionData(string tableMetadata, string templateId, string noteno, string revData)
        {


            var query = @$"select distinct n.* from public.""NtsNote"" as n 
                                join public.""Template"" as t on t.""Id"" = n.""TemplateId"" and t.""IsDeleted""=false 
                                join cms.""{tableMetadata}"" p ON n.""Id"" = p.""NtsNoteId"" 
                                where n.""NoteNo"" = '{noteno}' and t.""Id"" ='{templateId}' and p.""revision"" = '{revData}' and n.""IsDeleted""=false  ";

            var querydata = await _queryRepo.ExecuteQueryList<NoteViewModel>(query, null);
            return querydata;
        }
        public async Task<List<ColumnMetadataViewModel>> DynamicUdfColumnsData(string templateId)
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
        public async Task<string> GetServiceDocumentId(string serviceId, string udfs)
        {
            var query = $@" select udf.""documentId"" as ""Id"" from public.""NtsService"" as s
                                join (" + udfs + $@") as udf on udf.""NtsNoteId""=s.""UdfNoteId"" 
                                where s.""IsDeleted""=false  and s.""Id""='{serviceId}' ";
            var result = await _queryRepo.ExecuteScalar<string>(query, null);
            return result;
        }
        public async Task<IList<FileViewModel>> GetInlineCommentResultData(string NoteId, string udfs)
        {
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
            return files;
        }
        public async Task<NoteViewModel> GetWorkspaceIdData(string noteId)
        {
            var cypher = $@"select n.""Id"" as Id from 
                         cms.""N_GENERAL_FOLDER_WORKSPACE"" as n 
                         where n.""NtsNoteId""='{noteId}' and n.""IsDeleted""=false 
                           ";

            var value = await _queryRepo.ExecuteQuerySingle<NoteViewModel>(cypher, null);
            return value;

        }
        public async Task<bool> DeleteWorkspaceData(string NoteId)
        {
            var query = $@"update  cms.""N_GENERAL_FOLDER_WORKSPACE"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<string> GetFolderWorkspaceData(string id)
        {
            var cypher = $@"select wp.""WorkspaceId""  from cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" and n.""IsDeleted""=false 
                            where n.""Id"" = '{id}'and wp.""IsDeleted""=false  ";
            var value = await _queryRepo.ExecuteScalar<string>(cypher, null);
            return value;
        }
        public async Task<List<NtsLogViewModel>> GetVersionDetailsData(string noteId)
        {
            var query = $@"select l.""NoteSubject"" as Subject, l.* from log.""NtsNoteLog""  as l
            join public.""NtsNote"" as n on l.""RecordId""=n.""Id"" and n.""IsDeleted""=false  and l.""VersionNo""<>n.""VersionNo""
            where l.""IsDeleted""=false  and  l.""RecordId"" = '{noteId}' and l.""IsVersionLatest""=true order by l.""VersionNo"" desc";
            var result = await _queryNtsLog.ExecuteQueryList(query, null);
            return result;

        }
        public async Task<NoteTemplateViewModel> GetBookDetailsData(string noteId)
        {
            var query = @$"select ""TM"".*,""S"".""Id"" as ""NoteId"",""T"".""DisplayName"" as ""TemplateDisplayName""
            from ""public"".""NoteTemplate"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""Id""=""TM"".""TemplateId"" and ""T"".""IsDeleted""=false 
            join ""public"".""NtsNote"" as ""S"" on ""T"".""Id""=""S"".""TemplateId"" and ""S"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false  and ""S"".""Id""='{noteId}'";
            var model = await _queryRepo.ExecuteQuerySingle<NoteTemplateViewModel>(query, null);
            return model;
        }
        public async Task<List<NtsViewModel>> GetBookListData(string noteId)
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
            return list;
        }
        public async Task<NtsBookItemViewModel> GetBookByIdData(string id)
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
        public async Task<List<NtsBookItemViewModel>> GetBookItemChildListData(string serviceId, string noteId, string taskId)
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
        public async Task<NoteTemplateViewModel> GetFormIoData(string noteId)
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
            return data;
        }
        public async Task<DataRow> GetFormIoData1(string data, string noteId)
        {
            var selectQuery = @$"select * from cms.""{data}"" where ""NtsNoteId""='{noteId}' limit 1";
            var dr = await _queryRepo.ExecuteQueryDataRow(selectQuery, null);
            return dr;
        }
        public async Task<List<NoteViewModel>> NotesCountForDashboardData(string userId, string bookId)
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
and n.""PortalId""='{_userContext.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE# ";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var note = await _queryRepo.ExecuteQueryList(cypher, null);
            return note;
        }
        public async Task<List<NoteViewModel>> ProcessBookCountForDashboardData(string userId, string bookId)
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
and n.""PortalId""='{_userContext.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE# ";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var note = await _queryRepo.ExecuteQueryList(cypher, null);
            return note;
        }
        public async Task<List<NoteViewModel>> ProcessStageCountForDashboardData(string userId, string bookId)
        {
            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType
 from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""Id""=n.""NotePlusId"" and pn.""IsDeleted"" = false                          
join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
join public.""NtsService"" as pn on pn.""Id""=n.""ParentServiceId"" and pn.""Id""=n.""ServicePlusId"" and pn.""IsDeleted"" = false
join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #WHERE# ";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var note = await _queryRepo.ExecuteQueryList(cypher, null);
            return note;
        }
        public async Task<SynergyWebsiteViewModel> GetSynergyWebsiteData(string id)
        {
            var cypher = $@"select wp.*  from cms.""N_BusinessAnalytics_SynergyWebsite"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" and n.""IsDeleted""=false 
                            where n.""Id"" = '{id}'and wp.""IsDeleted""=false  ";
            var value = await _queryRepo.ExecuteQuerySingle<SynergyWebsiteViewModel>(cypher, null);
            return value;
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsiteData()
        {
            var cypher = $@"select wp.*,n.""Id"" as NoteId,n.""NoteSubject"" as Name  from cms.""N_BusinessAnalytics_SynergyWebsite"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" 
                            where n.""IsDeleted""=false  and wp.""IsDeleted""=false  ";
            var list = await _queryRepo.ExecuteQueryList<SynergyWebsiteViewModel>(cypher, null);
            return list.ToList();
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsiteNoteData()
        {
            var cypher = $@"select wp.*,n.""Id"" as Id,n.""Id"" as NoteId,n.""NoteSubject"" as Name  from cms.""N_BusinessAnalytics_SynergyWebsite"" as wp
                            join public.""NtsNote"" as n on n.""Id"" = wp.""NtsNoteId"" 
                            where n.""IsDeleted""=false  and wp.""IsDeleted""=false and wp.""IsTemplate""='False' ";
            var list = await _queryRepo.ExecuteQueryList<SynergyWebsiteViewModel>(cypher, null);
            return list;
        }
        public async Task<List<MeasuresViewModel>> GetMeasuresData()
        {
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.count') as ""name"",CONCAT(ss.""SchemaName"",'.count') as ""title""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MeasuresViewModel>(query, null);
            return querydata;
        }
        public async Task<List<MeasuresViewModel>> GetMeasuresDataDisplay()
        {
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.count') as ""name"",ss.""SchemaName"" as ""title""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<MeasuresViewModel>(query, null);
            return querydata;
        }
        public async Task<List<DimensionsViewModel>> GetDimensionsData()
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
        public async Task<List<DimensionsViewModel>> GetDimensionsByMeasueData(string measure)
        {
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""name"",
                            CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""title"",d.""NoteDescription"" as ""dataType""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id"" and CONCAT(ss.""SchemaName"",'.count')='{measure}'
                            join public.""NtsNote"" as d on d.""ParentNoteId""=n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<DimensionsViewModel>(query, null);
            return querydata;
        }
        public async Task<List<DimensionsViewModel>> GetDimensionsByMeasueDataDisplay(string measure)
        {
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""name"",
                            d.""NoteSubject"" as ""title"",d.""NoteDescription"" as ""dataType""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id"" and CONCAT(ss.""SchemaName"",'.count')='{measure}'
                            join public.""NtsNote"" as d on d.""ParentNoteId""=n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<DimensionsViewModel>(query, null);
            return querydata;
        }
        public async Task<List<DimensionsColumnViewModel>> GetDimensionsColumnByMeasueData(string measure)
        {
            var query = @$" SELECT CONCAT(ss.""SchemaName"",'.',d.""NoteSubject"") as ""id"",
                            d.""NoteSubject"" as ""label"",'string' as ""type""
                            FROM public.""NtsNote"" as n
                            join cms.""N_BusinessAnalytics_SynergySchema"" as ss on ss.""NtsNoteId"" = n.""Id"" and CONCAT(ss.""SchemaName"",'.count')='{measure}'
                            join public.""NtsNote"" as d on d.""ParentNoteId""=n.""Id""
                            where n.""TemplateCode"" = 'SynergySchema' and n.""IsDeleted""=false and d.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<DimensionsColumnViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<SocailScrappingApiViewModel>> GetAllCCTNSApiMethodsData()
        {
            var query = @$" select n.*,dm.""url"" as Url,dm.""parameter"" as Parameters, dm.""FromDate"" as FromDate,dm.""toDate"" as ToDate,
                        dm.""apiAuthorization"" as ApiAuthorization,dm.""filterColumn"" as FilterColumn,dm.""responseToken"" as ResponseToken
                        ,dm.""dateFormat"" as DateFormat,dm.""idColumn"" as IdColumn,dm.""batchDays"" as BatchDays,dm.""scheduleTime"" as ScheduleTime
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SOCIAL_SCRAPPING_SocailScrappingApi"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='SOCAIL_SCRAPPING_API' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<SocailScrappingApiViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllDistrictData()
        {
            var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as Name,dm.""districtCode"" as Code,dm.""dial100DistrictCode"" as title
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_District"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='SM_DISTRICT' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllVDPDistrictData()
        {
            var query = @$" select n.""NoteSubject"" as Id,dm.""district"" as Name,dm.""districtCode"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_VdpCredential"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='VDP_CREDENTIAL' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetVDPDistrictDataByCode(string code)
        {
            var query = @$" select n.""NoteSubject"" as Id,dm.""district"" as Name,dm.""districtCode"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_VdpCredential"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='VDP_CREDENTIAL' and dm.""districtCode""='{code}' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetVDPDistrictDataById(string id)
        {
            var query = @$" select n.""NoteSubject"" as Id,dm.""district"" as Name,dm.""districtCode"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_VdpCredential"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='VDP_CREDENTIAL' and n.""NoteSubject""='{id}' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<SchedulerLogViewModel> GetSchedulerLogData(string subject, string districtCode)
        {
            if (districtCode.IsNotNullAndNotEmpty())
            {
                var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as NoteSubject,dm.""districtCode"" as districtCode,
                        dm.""response"" as response,dm.""error"" as error,dm.""fromDate"" as fromDate,dm.""toDate"" as toDate,
                        case when (dm.""success"" is null or dm.""success""='True') then true else false end as success
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_SchedulerLog"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='SCHEDULER_LOG' and n.""NoteSubject""='{subject}' and dm.""districtCode""='{districtCode}' and t.""IsDeleted""=false  order by n.""CreatedDate"" desc limit 1 ";
                var querydata = await _queryRepo.ExecuteQuerySingle<SchedulerLogViewModel>(query, null);
                return querydata;
            }
            else
            {
                var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as NoteSubject,dm.""districtCode"" as districtCode,
                        dm.""response"" as response,dm.""error"" as error,dm.""fromDate"" as fromDate,dm.""toDate"" as toDate,
                        case when (dm.""success"" is null or dm.""success""='True') then true else false end as success
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_SchedulerLog"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='SCHEDULER_LOG' and n.""NoteSubject""='{subject}'  and t.""IsDeleted""=false  order by n.""CreatedDate"" desc limit 1 ";
                var querydata = await _queryRepo.ExecuteQuerySingle<SchedulerLogViewModel>(query, null);
                return querydata;
            }

        }
        public async Task<SocailScrappingApiViewModel> GetAllCCTNSApiMethodsDetails(string id)
        {
            var query = @$" select n.*,dm.""url"" as Url,dm.""parameter"" as Parameters, dm.""FromDate"" as FromDate,dm.""toDate"" as ToDate,
                        dm.""apiAuthorization"" as ApiAuthorization,dm.""filterColumn"" as FilterColumn,dm.""responseToken"" as ResponseToken
                        ,dm.""dateFormat"" as DateFormat,dm.""idColumn"" as IdColumn,dm.""batchDays"" as BatchDays
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SOCIAL_SCRAPPING_SocailScrappingApi"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where n.""Id""='{id}' and t.""Code""='SOCAIL_SCRAPPING_API' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQuerySingle<SocailScrappingApiViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<SocailScrappingApiParameterViewModel>> GetAllCCTNSApiMethodsParameterData(string parameterIds)
        {
            var query = @$" select n.""NoteSubject"" as ParameterName,dm.""defaultValue"" as DefaultValue,dm.""sequenceNo"" as SequenceNo
                        from public.""NtsNote"" as n    
                        join cms.""N_SOCIAL_SCRAPPING_SocailScrappingParameter"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where dm.""Id"" in {parameterIds} and n.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<SocailScrappingApiParameterViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAdhocNoteTemplateListData()
        {
            string query = @$"select t.""Id"", t.""DisplayName"" as Name from public.""NoteTemplate"" as nt
                                inner join public.""Template"" as t on t.""Id""=nt.""TemplateId"" and t.""TemplateType""=4
                              where nt.""NoteTemplateType""=4
                               order by nt.""CreatedDate"" desc ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<IList<NotificationViewModel>> GetNotificationListData(string userId, string portalId, long count = 20, string referenceId = null, string id = null)
        {
            var query = $@"select n.*, u.""PhotoId"" as PhotoId,u.""JobTitle"" as JobTitle,concat( u.""Name"",'<',u.""Email"",'>')  as From,nt.""ActionType"" as ActionType,lv.""Code"" as RefStatus
            from public.""Notification"" as n
left join public.""NotificationTemplate"" as nt on nt.""Id""=n.""NotificationTemplateId"" and nt.""IsDeleted""=false
            left join public.""User"" as u on (u.""Id""=n.""FromUserId"" or n.""From""=u.""Email"") and u.""IsDeleted""=false             
                    left join (select ""Id"",""ServiceStatusId"" as ""StatusId"" from public.""NtsService""

                       union select ""Id"",""TaskStatusId"" as ""StatusId"" from public.""NtsTask""
					   union select ""Id"",""NoteStatusId""  as ""StatusId"" from public.""NtsNote""
					  ) as t on t.""Id""=n.""ReferenceTypeId""
					  left join public.""LOV"" as lv on lv.""Id""=t.""StatusId"" and lv.""IsDeleted""=false
where n.""ToUserId""='{userId}'  and n.""IsDeleted""=false and n.""PortalId""='{portalId}' #REFERENCE# #Id#
            order by n.""CreatedDate"" desc";
            var search = "";
            var Idsearch = "";
            if (referenceId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""ReferenceTypeId""='{referenceId}' ";
            }
            if (id.IsNotNullAndNotEmpty())
            {
                Idsearch = $@" and n.""Id""='{id}' ";
            }
            query = query.Replace("#REFERENCE#", search);
            query = query.Replace("#Id#", Idsearch);
            var data = await _queryRepo.ExecuteQueryList<NotificationViewModel>(query, null);
            return data;
        }
        public async Task<IList<NotificationViewModel>> GetAllNotificationsData(string userId, string portalId, int? count = null, bool? isArchived = null, ReadStatusEnum? readStatus = null)
        {
            var limit = "";
            if (count.HasValue)
            {
                limit = $" limit {count}";
            }
            var archived = "";
            if (isArchived.HasValue)
            {
                archived = @$" and ""IsArchived""= {isArchived.ToString().ToLower()} ";
            }
            var read = "";
            if (readStatus.HasValue)
            {
                read = @$" and ""ReadStatus""= {(int)readStatus} ";
            }
            var query = $@"select n.*, u.""PhotoId"" as PhotoId, u.""Name"" as From
            from public.""Notification"" as n 
            left join public.""User"" as u on (u.""Id""=n.""FromUserId"" or n.""From""=u.""Email"") and u.""IsDeleted""=false 
            where n.""ToUserId""='{userId}' and n.""IsDeleted""=false and n.""PortalId""='{portalId}'
            {archived}{read}
            order by n.""CreatedDate"" desc {limit}";
            var data = await _queryRepo.ExecuteQueryList<NotificationViewModel>(query, null);
            return data;
        }
        public async Task<NotificationViewModel> GetNotificationDetailsData(string notificationId)
        {
            var query = $@"select n.*, u.""PhotoId"" as PhotoId,concat( u.""Name"",'<',u.""Email"",'>') as From
            from public.""Notification"" as n 
            left join public.""User"" as u on (u.""Id""=n.""FromUserId"" or n.""From""=u.""Email"") and u.""IsDeleted""=false 
            where n.""Id""='{notificationId}'";

            var data = await _queryRepo.ExecuteQuerySingle<NotificationViewModel>(query, null);
            return data;
        }
        public async Task<long> GetNotificationCountData(string userId, string portalId)
        {
            var query = $@"select count(n.""Id"")  from public.""Notification"" as n 
            where n.""ToUserId""='{userId}' and n.""ReadStatus""='0' and n.""IsDeleted""=false  and n.""PortalId""='{portalId}'";
            var data = await _queryRepo.ExecuteScalar<long>(query, null);
            return data;
        }
        public async Task MarkAllNotificationAsRead(string userId, string portalId)
        {
            var query = $@" update public.""Notification"" set ""ReadStatus"" = 1 where ""PortalId"" = '{portalId}' and ""ToUserId"" = '{userId}' and ""IsDeleted"" = false  ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task MarkNotificationAsReadData(string id)
        {
            var query = $@"update public.""Notification"" set ""ReadStatus""=1 where ""Id""='{id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task MarkNotificationAsNotReadData(string id)
        {
            var query = $@"update public.""Notification"" set ""ReadStatus""=0 where ""Id""='{id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task ArchiveNotificationData(string id)
        {
            var query = $@"update public.""Notification"" set ""IsArchived""=true where ""Id""='{id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task UnArchiveNotificationData(string id)
        {
            var query = $@"update public.""Notification"" set ""IsArchived""=false where ""Id""='{id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task StarNotificationData(string id)
        {
            var query = $@"update public.""Notification"" set ""IsStarred""=true where ""Id""='{id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task UnStarNotificationData(string id)
        {
            var query = $@"update public.""Notification"" set ""IsStarred""=false where ""Id""='{id}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<List<NotificationViewModel>> GetNotificationListData(DateTime date, DateTime FirstDate, DateTime LastDate, ReferenceTypeEnum? refType = null, string refTypeId = null)
        {


            var query = $@"select m.""Name"" as Module,mg.""Name"" as MenuGroup,p.""Name"" as PageName,t.""DisplayName"" as Template,reference.""referenceno"" as ReferenceNo,
case when reference.""subject"" is not null then reference.""subject"" else n.""Subject"" end as Subject,reference.""Id"" as ReferenceTypeId,n.""Id"",n.""ReferenceType"",n.""CreatedDate""
,n.""ReadStatus"",n.""IsArchived"",n.""ActionStatus""
            from public.""Notification"" as n 
			 left join(select note.""TemplateId"",note.""Id"",note.""NoteNo"" as ReferenceNo,note.""NoteSubject"" as Subject from  public.""NtsNote"" as note
					   union 
					   select note.""TemplateId"",note.""Id"",note.""TaskNo"" as ReferenceNo,note.""TaskSubject"" as Subject  from  public.""NtsTask"" as note
					    union 
					   select note.""TemplateId"",note.""Id"",note.""ServiceNo"" as ReferenceNo,note.""ServiceSubject"" as Subject  from  public.""NtsService"" as note
					
					  ) as reference on reference.""Id"" = n.""ReferenceTypeId""
			 left join public.""Template"" as t on t.""Id""=reference.""TemplateId"" and t.""IsDeleted""=false
			 left join public.""Page"" as p on t.""Id""=p.""TemplateId"" and p.""IsDeleted""=false
			 left join public.""Module"" as m on m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false
			 left  join public.""SubModule"" as sm on sm.""ModuleId""=m.""Id"" and sm.""IsDeleted""=false
			 left  join public.""MenuGroup"" as mg on mg.""SubModuleId""=sm.""Id"" and mg.""IsDeleted""=false
            left join public.""User"" as u on (u.""Id""=n.""FromUserId"" or n.""From""=u.""Email"") and u.""IsDeleted""=false 
            where n.""ToUserId""='{_repo.UserContext.UserId}' and n.""IsDeleted""=false and n.""PortalId""='{_repo.UserContext.PortalId}'
            and n.""CreatedDate""::TIMESTAMP::DATE<='{LastDate}'  and n.""CreatedDate""::TIMESTAMP::DATE>='{FirstDate}' #REFWHERE# #REFIDWHERE#
              -- order by n.""CreatedDate"" desc 
			group by m.""Name"",mg.""Name"",p.""Name"",t.""DisplayName"",reference.""referenceno"",reference.""subject"",n.""Subject"",n.""Id"",reference.""Id"" ";

            var refwhere = "";
            if (refType.IsNotNull())
            {
                refwhere = $@" and n.""ReferenceType""='{(int)refType}'";
            }
            query = query.Replace("#REFWHERE#", refwhere);

            var refidwhere = "";
            if (refTypeId.IsNotNull())
            {
                refidwhere = $@" and n.""ReferenceTypeId""='{refTypeId}'";
            }
            query = query.Replace("#REFIDWHERE#", refidwhere);

            var data = await _queryRepo.ExecuteQueryList<NotificationViewModel>(query, null);
            return data;
        }

        public async Task<List<ServiceViewModel>> UpdateNotStartedNtsData()
        {
            var query = @$"select s.* from public.""NtsService"" as s 
            join public.""LOV"" as l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            where s.""StartDate""<='{DateTime.Now.ToDatabaseDateFormat()}' and s.""IsDeleted""=false and l.""Code""='SERVICE_STATUS_NOTSTARTED'";
            var data = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return data;

        }
        public async Task<List<TaskViewModel>> UpdateNotStartedNtsData2()
        {
            var query = @$"select s.* from public.""NtsTask"" as s 
            join public.""LOV"" as l on s.""TaskStatusId""=l.""Id"" and l.""IsDeleted""=false
            where s.""StartDate""<'{DateTime.Now.ToDatabaseDateFormat()}' and s.""IsDeleted""=false and l.""Code""='TASK_STATUS_NOTSTARTED'";
            var taskList = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return taskList;

        }
        public async Task<List<ServiceViewModel>> UpdateOverdueNtsData()
        {
            var query = @$"select s.* from public.""NtsService"" as s 
            join public.""LOV"" as l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            where s.""DueDate""<'{DateTime.Now.ToDatabaseDateFormat()}' and s.""IsDeleted""=false and l.""Code""='SERVICE_STATUS_INPROGRESS'";
            var serviceList = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return serviceList;
        }
        public async Task<List<TaskViewModel>> UpdateOverdueNtsData1()
        {
            var query = @$"select s.* from public.""NtsTask"" as s 
            join public.""LOV"" as l on s.""TaskStatusId""=l.""Id"" and l.""IsDeleted""=false
            where s.""DueDate""<'{DateTime.Now.ToDatabaseDateFormat()}' and s.""IsDeleted""=false and l.""Code""='TASK_STATUS_INPROGRESS'";
            var taskList = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return taskList;
        }

        public async Task<List<ServiceViewModel>> DisbaleGrievenceReopenServiceData(DateTime dateTime)
        {
            var query = @$"select s.* from public.""NtsService"" as s 
             join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false and t.""Code""='EGOV_PUBLIC_GRIEVANCE_REGISTRATION'
            join public.""LOV"" as l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            where  s.""IsDeleted""=false and (l.""Code""='SERVICE_STATUS_COMPLETE' or l.""Code""='SERVICE_STATUS_CANCEL') and  ((DATE_PART('day', '{dateTime}'::Date - s.""CompletedDate""::Date)>5) or (DATE_PART('day', '{dateTime}'::Date - s.""CanceledDate""::Date)>5))";
            var serviceList = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return serviceList;
        }
        public async Task DisbaleGrievenceReopenServiceData1()
        {
            var query1 = $@"update public.""NtsService"" set ""DisableReopen"" =true";
            await _queryRepo.ExecuteCommand(query1, null);
        }
        public async Task<List<TaskViewModel>> CancelCommunityHallBookingOnExpiredData(DateTime dateTime)
        {
            var query = @$"select task.* from public.""NtsService"" as s 
             join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""IsDeleted""=false and t.""Code""='CommunityHallBooking'
            join public.""NtsTask"" as task on s.""Id""=task.""ParentServiceId"" and task.""IsDeleted""=false
            join public.""Template"" as tt on tt.""Id""=task.""TemplateId"" and tt.""IsDeleted""=false and tt.""Code""='CitizenCommunityHallBooking'
            where  s.""IsDeleted""=false  and  ((DATE_PART('day', '{dateTime}'::Date - task.""StartDate""::Date)>2))";
            var taskList = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return taskList;
        }
        public async Task UpdateRentServiceData(string noteId, string ColumnName)
        {
            var query = $@"update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""{ColumnName}""=True, ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_repo.UserContext.UserId}' where ""NtsNoteId""='{noteId}'";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task UpdateRentalStatusForVacatingData(string rentalstatus, string rentalAgreementNumber, string reason)
        {
            var query = $@"update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""RentalPropertyStatus""='{rentalstatus}', ""ReasonForVacating""='{reason}',""LastUpdatedBy""='{_repo.UserContext.UserId}' 
                                where ""RentalAgreementNumber""='{rentalAgreementNumber}'";

            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<List<NtsNoteCommentViewModel>> GetSearchResultData(string NoteId)
        {

            string query = @$"select n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName
                              from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id""  and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsNoteComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
 where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
group by  n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"" ";
            ;
            var list = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
            return list;
        }
        public async Task<List<NtsNoteCommentViewModel>> GetCommentTreeData(string NoteId, string Id = null)
        {

            string query = "";
            if (Id == null)
            {
                query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsNoteComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"",f.""FileName"" ";

            }

            else
            {
                query = $@" with recursive cmn as(
select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false and n.""ParentCommentId""='{Id}'
and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	union
	
	select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false
 and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


)select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
from cmn
group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



";
            }
            var list = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
            return list;
        }
        public async Task<List<NtsNoteCommentViewModel>> GetAllCommentTreeData(string NoteId)
        {
            string query = "";
            query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsNoteComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"",f.""FileName"" ";
            var result = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
            return result;
        }
        public async Task<List<NtsNoteCommentViewModel>> GetAllCommentTreeData1(string p)
        {
            var query = $@" with recursive cmn as(
select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false and n.""ParentCommentId""='{p}'
and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	union
	
	select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false
 and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


)select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
from cmn
group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



";
            var result1 = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
            return result1;
        }
        public async Task<List<NtsNoteSharedViewModel>> GetSearchResult(string NoteId)
        {

            string query = @$"select n.""Id"" as Id,u.""Name"" as Name,'User' as Type,u.""PhotoId"" as PhotoId
                              from public.""NtsNoteShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false
                              
                              where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false
union select n.""Id"" as Id,t.""Name"" as Name,'Team' as Type, t.""LogoId"" as PhotoId
                              from public.""NtsNoteShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false";
            var list = await _queryRepo.ExecuteQueryList<NtsNoteSharedViewModel>(query, null);
            return list;
        }
        public async Task<List<NtsServiceCommentViewModel>> GetSearchResultData1(string ServiceId)
        {

            string query = @$"select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName
                from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsServiceComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
 where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"" ";

            var list = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            return list;
        }
        public async Task<List<NtsServiceCommentViewModel>> GetCommentTreeData1(string ServiceId, string Id = null)
        {

            string query = "";
            if (Id == null)
            {
                query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsServiceComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"",f.""FileName"" ";

            }

            else
            {
                query = $@" with recursive cmn as(
select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			   from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false and n.""ParentCommentId""='{Id}'
and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	union
	
	select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			   from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false
 and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


)select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
from cmn
group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



";
            }
            var list = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            return list;
        }
        public async Task<List<NtsServiceCommentViewModel>> GetAllCommentTree(string serviceId)
        {
            var query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
                            n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
                            case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
                            null as ParentId,true as hasChildren,true as expanded, uc.""JobTitle"" as JobTitle
                                            from public.""NtsServiceComment"" as n
                            join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                            left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                             left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
                            left join public.""NtsServiceComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
                            left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                             left join public.""User"" as uc ON uc.""Id"" = n.""CommentedByUserId"" and uc.""IsDeleted""=false
                             where n.""NtsServiceId""='{serviceId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
                            or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
                             group by n.""Id"",ub.""Name"",ub.""PhotoId"",
                            n.""CommentedDate"",n.""Comment"",f.""FileName"", uc.""JobTitle"" order by n.""CommentedDate"" desc  ";
            var result = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            return result;
        }
        public async Task<List<NtsServiceCommentViewModel>> GetAllCommentTree1(string p)
        {
            var query = $@" with recursive cmn as(
                        select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                        n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                        ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                        n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                        n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			                           from public.""NtsServiceComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                        left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                         left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                        left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                         where  n.""IsDeleted""= false and n.""ParentCommentId""='{p}'
                        and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	                        union
	
	                        select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                        n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                        ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                        n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                        n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			                           from public.""NtsServiceComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	                        join cmn as p on p.""Id""=n.""ParentCommentId""
                        left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                         left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                        left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                         where  n.""IsDeleted""= false
                         and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


                        )select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
                        from cmn
                        group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
                        ,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



                        ";
            var result1 = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            return result1;
        }
        public async Task<List<NtsServiceSharedViewModel>> GetSearchSharedResult(string ServiceId)
        {

            string query = @$"select n.""Id"" as Id,u.""Name"" as Name,'User' as Type,u.""PhotoId"" as PhotoId
                              from public.""NtsServiceShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false
                              
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false
union select n.""Id"" as Id,t.""Name"" as Name,'Team' as Type, t.""LogoId"" as PhotoId
                              from public.""NtsServiceShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false";
            var list = await _queryRepo.ExecuteQueryList<NtsServiceSharedViewModel>(query, null);
            return list;
        }
        public async Task<bool> UpdateStagingByBatchIdData(string batchId)
        {
            var query = @$" update public.""NtsStaging"" set ""StageStatus""=1
                        where ""BatchId""='{batchId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<List<NtsTaskCommentViewModel>> GetSearchCommentResultData(string TaskId)
        {
            string query = @$"select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName
                              from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id""
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsTaskComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
 where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"" ";
            var list = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
            return list;
        }
        public async Task<List<NtsTaskCommentViewModel>> GetTaskCommentTreeData(string TaskId, string Id = null)
        {

            string query = "";
            if (Id == null)
            {
                query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsTaskComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"",f.""FileName"" ";

            }

            else
            {
                query = $@" with recursive cmn as(
select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			   from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false and n.""ParentCommentId""='{Id}'
and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	union
	
	select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			   from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false
 and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


)select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
from cmn
group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



";
            }
            var list = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetTaskCommentUserListData(string TaskId)
        {

            string query = @$"select ub.""CommentToUserId"" as Id
                                              from public.""NtsTaskComment"" as n
                join public.""NtsTaskComment"" as ub ON ub.""NtsTaskCommentId"" = n.""Id"" and ub.""IsDeleted""=false
                
                 where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false";
            ;
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);

            return list;
            ///return list;
        }
        public async Task<List<NtsTaskCommentViewModel>> GetAllTaskCommentTreeData(string taskId)
        {
            var query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
                        n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
                        case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
                        null as ParentId,true as hasChildren,true as expanded
                                        from public.""NtsTaskComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                        left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                         left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
                        left join public.""NtsTaskComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
                        left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                         where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
                        or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
                         group by n.""Id"",ub.""Name"",ub.""PhotoId"",
                        n.""CommentedDate"",n.""Comment"",f.""FileName"" ";
            var result = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
            return result;
        }
        public async Task<List<NtsTaskCommentViewModel>> GetAllTaskCommentTreeData1(string p)
        {
            var query = $@" with recursive cmn as(
                            select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                            n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                            ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                            n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                            n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			                               from public.""NtsTaskComment"" as n
                            join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                            left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                             left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                            left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                             where  n.""IsDeleted""= false and n.""ParentCommentId""='{p}'
                            and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	                            union
	
	                            select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                            n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                            ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                            n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                            n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			                               from public.""NtsTaskComment"" as n
                            join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	                            join cmn as p on p.""Id""=n.""ParentCommentId""
                            left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                             left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                            left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                             where  n.""IsDeleted""= false
                             and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


                            )select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
                            from cmn
                            group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
                            ,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



                            ";
            var result1 = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
            return result1;
        }
        public async Task<List<NtsTaskPrecedenceViewModel>> GetSearchTaskPrecedenceResult(string taskId)
        {

            string query = @$"select n.*
                            from public.""NtsTaskPrecedence"" as n  
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false";
            ;
            var result = await _queryRepo.ExecuteQueryList<NtsTaskPrecedenceViewModel>(query, null);
            return result;

        }
        public async Task<List<NtsTaskPrecedenceViewModel>> GetTaskPredecessorData(string taskId)
        {

            string query = @$"select  t.""TaskSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""TaskNo"" as TaskNo
                            from public.""NtsTaskPrecedence"" as n  
join public.""NtsTask"" as t on t.""Id""=n.""PredecessorId"" and t.""IsDeleted""=false
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""PredecessorType""='1'
Union
select t.""ServiceSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""ServiceNo"" as TaskNo
                            from public.""NtsTaskPrecedence"" as n
join public.""NtsService"" as t on t.""Id""=n.""PredecessorId"" and t.""IsDeleted""=false
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""PredecessorType""='2'

Union
select t.""NoteSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""NoteNo"" as TaskNo
                            from public.""NtsTaskPrecedence"" as n
join public.""NtsNote"" as t on t.""Id""=n.""PredecessorId"" and t.""IsDeleted""=false
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""PredecessorType""='0'

";
            ;
            var result = await _queryRepo.ExecuteQueryList<NtsTaskPrecedenceViewModel>(query, null);
            return result;

        }
        public async Task<List<NtsTaskPrecedenceViewModel>> GetTaskSuccessorData(string taskId)
        {

            string query = @$"select  t.""TaskSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""TaskNo"" as TaskNo
                            from  public.""NtsTaskPrecedence"" as n 
join  public.""NtsTask"" as t   on t.""Id""=n.""NtsTaskId"" and t.""IsDeleted""=false
                             where n.""PredecessorId""='{taskId}' AND n.""IsDeleted""= false";

            var result = await _queryRepo.ExecuteQueryList<NtsTaskPrecedenceViewModel>(query, null);
            return result;

        }
        public async Task<List<NtsTaskSharedViewModel>> GetSearchTaskCommentResult(string TaskId)
        {

            string query = @$"select n.""Id"" as Id,u.""Name"" as Name,'User' as Type,u.""PhotoId"" as PhotoId
                              from public.""NtsTaskShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false
                              
                              where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false
union select n.""Id"" as Id,t.""Name"" as Name,'Team' as Type, t.""LogoId"" as PhotoId
                              from public.""NtsTaskShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false";
            var list = await _queryRepo.ExecuteQueryList<NtsTaskSharedViewModel>(query, null);
            return list;
        }
        public async Task<List<TaskTimeEntryViewModel>> GetSearchTaskTimeEntryResult(string taskId)
        {

            string query = @$"select n.*,u.""Name"" as UserName,u.""Email"" as UserEmail
                            from public.""NtsTaskTimeEntry"" as n                            
                            join public.""User"" as u ON u.""Id"" = n.""UserId"" and u.""IsDeleted""=false                          
                            
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false";
            ;
            var result = await _queryRepo.ExecuteQueryList<TaskTimeEntryViewModel>(query, null);
            return result;

        }
        public async Task<List<TaskTimeEntryViewModel>> GetTimeEntriesData(string serviceId, DateTime timelog, string userId = null)
        {

            string query = @$"select n.*,u.""Name"" as UserName,u.""Email"" as UserEmail,t.""TaskNo"" as TaskNo,t.""TaskSubject"" as TaskName,n.""Comment"" as Comment,s.""Id"" as NtsServiceId,s.""ServiceSubject"" as ProjectName
                            from public.""NtsTaskTimeEntry"" as n                            
                            join public.""User"" as u ON u.""Id"" = n.""UserId"" and u.""IsDeleted""=false     
							join public.""NtsTask"" as t on t.""Id""=n.""NtsTaskId"" and t.""IsDeleted""=false  
							join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" or s.""Id""=t.""ServicePlusId"" and s.""IsDeleted""=false  
                            where s.""Id""='{serviceId}' #SEARCHDATE# #SEARCHUSER# and n.""IsDeleted""= false";
            var searchDate = "";
            if (timelog != ApplicationConstant.DateAndTime.MinDate)
            {
                searchDate = @$" AND n.""StartDate""::DATE <= '{timelog}'::DATE and '{timelog}'::DATE <=n.""EndDate""::DATE ";
            }
            var searchUser = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                searchUser = $@" AND u.""Id""='{userId}' ";
            }
            query = query.Replace("#SEARCHDATE#", searchDate);
            query = query.Replace("#SEARCHUSER#", searchUser);
            var result = await _queryRepo.ExecuteQueryList<TaskTimeEntryViewModel>(query, null);
            return result;

        }
        public async Task<IList<IdNameViewModel>> GetAdhocServiceTemplateListData()
        {
            string query = @$"select t.""Id"", t.""DisplayName"" as Name from public.""ServiceTemplate"" as st
                                inner join public.""Template"" as t on t.""Id""=st.""TemplateId"" and t.""TemplateType""=6
                                where st.""ServiceTemplateType""=4
                               order by st.""CreatedDate"" desc ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        #region TaskBusiness
        public async Task RollBackTaskData(TaskTemplateViewModel model, TableMetadataViewModel table)
        {

            var query = @$"delete from {table.Schema}.""{ table.Name}"" where ""Id""='{model.UdfNoteTableId}';
                    delete from public.""NtsTask"" where ""Id""='{model.TaskId}';
                    delete from public.""NtsNote"" where ""Id""='{model.UdfNoteId}';";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<List<UserViewModel>> SetSharedTaskListData(TaskTemplateViewModel model)
        {
            string query = @$"select u.""Id"" as Id,u.""Name"" as UserName,u.""Email"" as Email,u.""PhotoId"" as PhotoId
                              from public.""NtsTaskShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" AND u.""IsDeleted""= false 
                              
                              where n.""NtsTaskId""='{model.TaskId}' AND n.""IsDeleted""= false 
                                union select t.""Id"" as Id,t.""Name"" as UserName,t.""Name"" as Email, t.""LogoId"" as PhotoId
                              from public.""NtsTaskShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" AND t.""IsDeleted""= false 
                              where n.""NtsTaskId""='{model.TaskId}' AND n.""IsDeleted""= false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
            //new List<UserViewModel> { { new UserViewModel { UserName = "Shafi", Email = "shaficmd@gmail.com" } } ,
            //{ new UserViewModel { UserName = "Shafi2", Email = "shaficmd@gmail.com2" } } };
        }
        public async Task<List<TaskTemplateViewModel>> GetTaskIndexPageCountData(TaskIndexPageTemplateViewModel model, PageViewModel page)
        {
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",t.""AssignedToUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""TaskStatusCode"",count(distinct t.""Id"") as ""TaskCount""      
            from public.""NtsTask"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""= false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",t.""AssignedToUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId"",null as ""AssignedToUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""TaskStatusCode""
            ,count(distinct t.""Id"") as ""TaskCount""      
            from public.""NtsTask"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""NtsTaskShared"" as ts on t.""Id""=ts.""NtsTaskId"" and ts.""IsDeleted""= false  
            join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""= false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedByUserId"",l.""Code"" 
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId"",null as ""AssignedToUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""TaskStatusCode""
            ,count(distinct t.""Id"") as ""TaskCount""      
            from public.""NtsTask"" as t
            join public.""NtsNote"" as n on n.""Id""=t.""UdfNoteId"" and n.""IsDeleted""=false 
            #UDFJOIN#
            join public.""NtsTaskShared"" as ts on t.""Id""=ts.""NtsTaskId"" and ts.""IsDeleted""= false 
            join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""= false 
            where t.""TemplateId""='{page.TemplateId}' and t.""IsDeleted""=false 
            group by  ts.""SharedWithUserId"",l.""Code""";
            var udfjoin = "";
            var template = await _repo.GetSingleById<TemplateViewModel, Template>(page.TemplateId);
            if (template.IsNotNull())
            {
                var udfTableMetadataId = template.UdfTableMetadataId;
                if (udfTableMetadataId.IsNullOrEmpty())
                {
                    var stepTask = await _repo.GetSingle<StepTaskComponentViewModel, StepTaskComponent>(x => x.TemplateId == template.Id);
                    udfTableMetadataId = stepTask?.UdfTableMetadataId;
                }

                var tableMetadata = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(udfTableMetadataId);
                if (tableMetadata.IsNotNull())
                {
                    udfjoin = $@"join {tableMetadata.Schema}.""{tableMetadata.Name}"" as udf on udf.""NtsNoteId""=n.""Id"" and udf.""IsDeleted""=false";
                }
            }

            query = query.Replace("#UDFJOIN#", udfjoin);
            var result = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);
            return result;
        }
        public async Task<TableMetadataViewModel> GetTaskTemplateByIdData(TaskTemplateViewModel viewModel)
        {
            var query = "";
            if (viewModel.TaskTemplateType == TaskTypeEnum.StepTask)
            {
                query = @$"select ""TM"".* from ""public"".""NtsTask"" as ""TA"" 
                join ""public"".""StepTaskComponent"" as ""ST"" on ""TA"".""StepTaskComponentId""=""ST"".""Id"" and ""ST"".""IsDeleted""=false 
                join ""public"".""Template"" as ""T"" on ""TA"".""TemplateId""=""T"".""Id"" and ""T"".""IsDeleted""=false 
                join ""public"".""TableMetadata"" as ""TM"" on ""ST"".""UdfTableMetadataId""=""TM"".""Id"" and ""TM"".""IsDeleted""=false 
                where ""TA"".""IsDeleted""=false  
                and ""TA"".""Id""='{viewModel.TaskId}'";
            }
            else
            {
                query = @$"select ""TM"".* from ""public"".""TableMetadata"" as ""TM"" 
                join ""public"".""Template"" as ""T"" on ""T"".""UdfTableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
                join ""public"".""NtsTask"" as ""TA"" on ""T"".""Id""=""TA"".""TemplateId"" and ""TA"".""IsDeleted""=false  
                where ""TM"".""IsDeleted""=false 
                and ""TA"".""Id""='{viewModel.TaskId}'";
            }
            var tableMetadata = await _queryRepo.ExecuteQuerySingle<TableMetadataViewModel>(query, null);
            return tableMetadata;
        }
        public async Task<dynamic> GetTaskTemplateByIdData1(TaskTemplateViewModel viewModel)
        {
            var log = await _queryRepo.ExecuteQuerySingleDynamicObject(@$"select ""IsLatest"" from log.""NtsTaskLog"" where  ""Id""='{viewModel.LogId}'", null);
            return log;
        }
        public async Task<List<NTSMessageViewModel>> GetTaskMessageListData(string userId, string taskId)
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
            from public.""NtsTaskComment"" as ""C""  
            left join public.""User"" as ""CBU"" on ""C"".""CommentedByUserId""=""CBU"".""Id"" and ""CBU"".""IsDeleted""=false 
            left join public.""User"" as ""CTU"" on ""C"".""CommentToUserId""=""CTU"".""Id"" and ""CTU"".""IsDeleted""=false 
            where ""C"".""NtsTaskId""='{taskId}' and ""C"".""IsDeleted""=false ";

            var comments = await _queryRepo.ExecuteQueryList<NTSMessageViewModel>(query, null);
            return comments;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableTaskColumnsData(TableMetadataViewModel tableMetaData)
        {
            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
                where 
                (
                    (t.""Schema""='{tableMetaData.Schema}' and t.""Name""='{tableMetaData.Name}' and (c.""IsUdfColumn""=true)) or
                    (t.""Schema""='public' and t.""Name""='TaskTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsTask')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false ";
            var pkColumns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return pkColumns;
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableTaskColumnsData1(TableMetadataViewModel tableMetaData)
        {
            var query = $@"SELECT  fc.*,true as ""IsForeignKeyTableColumn"",c.""ForeignKeyTableName"" as ""TableName""
                ,c.""ForeignKeyTableSchemaName"" as ""TableSchemaName"",c.""ForeignKeyTableAliasName"" as  ""TableAliasName""
                ,c.""Name"" as ""ForeignKeyColumnName"",c.""IsUdfColumn"" as ""IsUdfColumn""
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" ft on c.""ForeignKeyTableId""=ft.""Id"" and ft.""IsDeleted""=false 
                join public.""ColumnMetadata"" fc on ft.""Id""=fc.""TableMetadataId""  and fc.""IsDeleted""=false 
                where c.""TableMetadataId""='{tableMetaData.Id}'
                and c.""IsForeignKey""=true and c.""IsDeleted""=false  and c.""IsUdfColumn""=true 
                and (fc.""IsSystemColumn""=false or fc.""IsPrimaryKey""=true)";
            var result = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return result;
        }
        public async Task<List<ColumnMetadataViewModel>> GetTaskViewableColumnsData()
        {

            var query = $@"SELECT  c.*,t.""Name"" as ""TableName"" 
                FROM public.""ColumnMetadata"" c
                join public.""TableMetadata"" t on c.""TableMetadataId""=t.""Id"" and t.""IsDeleted""=false 
                where 
                (
                    (t.""Schema""='public' and t.""Name""='TaskTemplate' and (c.""IsSystemColumn""=false or c.""IsPrimaryKey""=true)) or
                    (t.""Schema""='public' and t.""Name""='NtsTask')
                )
                and c.""IsVirtualColumn""=false and c.""IsDeleted""=false ";
            var pkColumns = await _queryRepo.ExecuteQueryList<ColumnMetadataViewModel>(query, null);
            return pkColumns;
        }
        public async Task<long?> GenerateNextTaskNoData()
        {


            var query = $@"update public.""NtsTaskSequence"" SET ""NextId"" = ""NextId""+1,
            ""LastUpdatedDate"" ='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_repo.UserContext.UserId}'
            WHERE ""SequenceDate"" = '{DateTime.Today.ToDatabaseDateFormat()}'
            RETURNING ""NextId""-1";
            var nextId = await _queryRepo.ExecuteScalar<long?>(query, null);
            return nextId;
        }
        public async Task<TaskTemplateViewModel> GetTaskTemplateForNewTaskData(TaskTemplateViewModel vm)
        {
            var query = @$"select ""TT"".*,
            ""NT"".""Json"" as ""Json"",
            ""T"".""UdfTemplateId"" as ""UdfTemplateId"",
            ""T"".""Id"" as ""TemplateId"",
            ""T"".""TableMetadataId"" as ""TableMetadataId"",
            ""T"".""UdfTableMetadataId"" as ""UdfTableMetadataId"",
            ""T"".""Name"" as ""TemplateName"",
            ""T"".""DisplayName"" as ""TemplateDisplayName"",
            ""T"".""Code"" as ""TemplateCode"",
            ""TC"".""Id"" as ""TemplateCategoryId"",
            ""TC"".""Code"" as ""TemplateCategoryCode"",
            ""TC"".""Name"" as ""TemplateCategoryName"",
            ""TC"".""TemplateType"" as ""TemplateType"" ,
            ""TS"".""Id"" as ""TaskStatusId"" ,
            ""TS"".""Code"" as ""TaskStatusCode"" ,
            ""TS"".""Name"" as ""TaskStatusName"" ,
            ""TA"".""Id"" as ""TaskActionId"" ,
            ""TA"".""Code"" as ""TaskActionCode"" ,
            ""TA"".""Name"" as ""TaskActionName"" ,
            ""TP"".""Id"" as ""TaskPriorityId"" ,
            ""TP"".""Code"" as ""TaskPriorityCode"" ,
            ""TP"".""Name"" as ""TaskPriorityName"" 
            from public.""Template"" as ""T""
            join public.""Template"" as ""NT"" on coalesce(""T"".""UdfTemplateId"",
            (select ""UdfTemplateId"" from public.""StepTaskComponent"" where ""Id""='{vm.StepTaskComponentId}' limit 1) )=""NT"".""Id"" and ""NT"".""IsDeleted""=false 
            join public.""TemplateCategory"" as ""TC"" on ""T"".""TemplateCategoryId""=""TC"".""Id"" and ""TC"".""IsDeleted""=false 
            join public.""TaskTemplate"" as ""TT"" on ""TT"".""TemplateId""=""T"".""Id"" and ""TT"".""IsDeleted""=false 
            join public.""LOV"" as ""TS"" on ""TS"".""LOVType""='LOV_TASK_STATUS' and ""TS"".""Code""='TASK_STATUS_DRAFT'
            and ""TS"".""IsDeleted""=false 
            join public.""LOV"" as ""TA"" on ""TA"".""LOVType""='LOV_TASK_ACTION' and ""TA"".""Code""='TASK_ACTION_DRAFT' 
            and ""TA"".""IsDeleted""=false
            join public.""LOV"" as ""TP"" on ""TP"".""LOVType""='TASK_PRIORITY' and ""TP"".""IsDeleted""=false 
            and (""TP"".""Id""=""TT"".""PriorityId"" or (""TT"".""PriorityId"" is null and ""TP"".""Code""='TASK_PRIORITY_MEDIUM'))
            where ""T"".""IsDeleted""=false  and (""T"".""Id""='{vm.TemplateId}' or ""T"".""Code""='{vm.TemplateCode}')";
            var data = await _queryRepo.ExecuteQuerySingle<TaskTemplateViewModel>(query, null);
            return data;
        }
        public async Task<IList<TaskViewModel>> GetNtsTaskIndexPageGridData(DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string taskStatusCode, string categoryCode, string templateCode, string moduleCode)
        {

            var query = @$" Select t.*,tmpc.""Code"" as ""TemplateCategoryCode"",tmpc.""Name"" as ""TemplateCategoryName"",ou.""Name"" as ""OwnerUserName"",ru.""Name"" as ""RequestedByUserName""
                        ,cu.""Name"" as ""CreatedByUserName"",lu.""Name"" as ""LastUpdatedByUserName"",au.""Name"" as ""AssigneeUserName""
                        , tlov.""Name"" as ""TaskStatusName"", tlov.""Code"" as ""TaskStatusCode"",m.""Name"" as Module
from public.""NtsTask"" as t
                        left join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
                        left join public.""Module"" as m ON m.""Id""=tmp.""ModuleId"" and m.""IsDeleted""=false 
                        left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
                        left join public.""LOV"" as tlov on t.""TaskStatusId""=tlov.""Id"" and tlov.""IsDeleted""=false 
                        left join public.""User"" as au ON au.""Id""=t.""AssignedToUserId"" and au.""IsDeleted""=false 
                        left join public.""User"" as ou ON ou.""Id""=t.""OwnerUserId"" and ou.""IsDeleted""=false 
                        left join public.""User"" as ru ON ru.""Id""=t.""RequestedByUserId"" and ru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=t.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=t.""LastUpdatedBy"" and lu.""IsDeleted""=false 
                        #WHERE# 
                        ORDER BY t.""LastUpdatedDate"" DESC ";
            var where = @$" WHERE 1=1 ";
            switch (ownerType)
            {
                case NtsActiveUserTypeEnum.Assignee:
                    where += @$" and t.""AssignedToUserId""='{_repo.UserContext.UserId}'";
                    break;
                case NtsActiveUserTypeEnum.OwnerOrRequester:
                    where += @$" and (t.""OwnerUserId""='{_repo.UserContext.UserId}' or t.""RequestedByUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.Requester:
                    where += @$" and (t.""OwnerUserId""<>'{_repo.UserContext.UserId}' and t.""RequestedByUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.Owner:
                    where += @$" and (t.""OwnerUserId""='{_repo.UserContext.UserId}')";
                    break;
                case NtsActiveUserTypeEnum.SharedWith:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsTaskId"" from public.""NtsTaskShared"" as ts where ts.""IsDeleted""=false and ts.""SharedWithUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                case NtsActiveUserTypeEnum.SharedBy:
                    where += @$" and (t.""Id"" in ( Select ts.""NtsTaskId"" from public.""NtsTaskShared"" as ts where ts.""IsDeleted""=false and ts.""SharedByUserId""='{_repo.UserContext.UserId}') ) ";
                    break;
                default:
                    break;
            }
            if (taskStatusCode.IsNotNullAndNotEmpty())
            {
                var taskItems = taskStatusCode.Split(',');
                var taskText = "";
                foreach (var item in taskItems)
                {
                    taskText = $"{taskText},'{item}'";
                }
                //var taskstatus = $@" and tlov.""Code""='{taskStatusCode}' ";
                var taskstatus = $@" and tlov.""Code"" IN ({taskText.Trim(',')}) ";
                where = where + taskstatus;
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
            var querydata = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TaskTemplateViewModel>> GetNtsTaskIndexPageCountData(NtsTaskIndexPageViewModel model)
        {
            var query = $@"select t.""OwnerUserId"",t.""RequestedByUserId"",t.""AssignedToUserId"",null as  ""SharedByUserId""
            ,null as ""SharedWithUserId"",l.""Code"" as ""TaskStatusCode"",count(distinct t.""Id"") as ""TaskCount""      
            from public.""NtsTask"" as t
            join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false 
            join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  t.""OwnerUserId"",t.""RequestedByUserId"",t.""AssignedToUserId"",l.""Code"" 
            
            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId"",null as ""AssignedToUserId""
            ,ts.""SharedByUserId"",null as ""SharedWithUserId"",l.""Code"" as ""TaskStatusCode""
            ,count(distinct t.""Id"") as ""TaskCount""      
            from public.""NtsTask"" as t
            join public.""NtsTaskShared"" as ts on t.""Id""=ts.""NtsTaskId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false 
            join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
            left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
            where t.""IsDeleted""=false 
            #WHERE#
            group by  ts.""SharedByUserId"",l.""Code"" 

            union
            select  null as ""OwnerUserId"",null as ""RequestedByUserId"",null as ""AssignedToUserId""
            ,null as ""SharedByUserId"",ts.""SharedWithUserId"",l.""Code"" as ""TaskStatusCode""
            ,count(distinct t.""Id"") as ""TaskCount""      
            from public.""NtsTask"" as t
            join public.""NtsTaskShared"" as ts on t.""Id""=ts.""NtsTaskId"" and ts.""IsDeleted""=false 
            join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false 
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
            var result = await _queryRepo.ExecuteQueryList<TaskTemplateViewModel>(query, null);
            return result;
        }
        public async Task<IList<NtsTaskIndexPageViewModel>> GetTaskCountByServiceTemplateCodesData(string categoryCodes = null, string portalId = null, bool showAllTaskForAdmin = false, string templateCodes = null, string portalNames = null, string statusCodes = null, string userId = null)
        {
            if (categoryCodes.IsNullOrEmpty())
            {
                var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
                string[] codes = data.Select(x => x.Code).ToArray();
                categoryCodes = String.Join(",", codes);
            }

            userId = userId.IsNullOrEmpty() ? _userContext.UserId : userId;

            var query = "";
            if (_repo.UserContext.IsSystemAdmin && showAllTaskForAdmin)
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'TASK_STATUS_INPROGRESS' or l.""Code"" = 'TASK_STATUS_OVERDUE' then 1 end) as AssignedToMeInProgreessOverDueCount,
count(case when l.""Code"" = 'TASK_STATUS_COMPLETE' then 1 end) as AssignedToMeCompleteCount,
count(case when l.""Code"" = 'TASK_STATUS_REJECT' or l.""Code"" = 'TASK_STATUS_CANCEL' then 1 end) as AssignedToMeRejectCancelCloseCount,
count(t.""Id"") as TotalCount

from public.""NtsTask"" as t
join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE# #TEMPCODEWHERE# #STATUSWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }
            else
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'TASK_STATUS_INPROGRESS' or l.""Code"" = 'TASK_STATUS_OVERDUE' then 1 end) as AssignedToMeInProgreessOverDueCount,
count(case when l.""Code"" = 'TASK_STATUS_COMPLETE' then 1 end) as AssignedToMeCompleteCount,
count(case when l.""Code"" = 'TASK_STATUS_REJECT' or l.""Code"" = 'TASK_STATUS_CANCEL' then 1 end) as AssignedToMeRejectCancelCloseCount,
count(t.""Id"") as TotalCount

from public.""NtsTask"" as t
join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE# #TEMPCODEWHERE# #STATUSWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }

            //var catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}') ";
            var catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                categoryCodes = categoryCodes.Replace(",", "','");
                catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes}') ";
            }
            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                temCodeWhere = $@"and st.""Code"" in ('{templateCodes}')";
            }
            query = query.Replace("#TEMPCODEWHERE#", temCodeWhere);

            var statuswhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                statusCodes = statusCodes.Replace(",", "','");
                statuswhere = $@" and l.""Code"" in ('{statusCodes}') ";
            }
            query = query.Replace("#STATUSWHERE#", statuswhere);

            var result = await _queryRepo.ExecuteQueryList<NtsTaskIndexPageViewModel>(query, null);

            return result;
        }
        public async Task<IList<TaskViewModel>> GetTaskListByServiceCategoryCodesData(string categoryCodes = null, string status = null, string portalId = null, bool showAllTaskForAdmin = false, string templateCodes = null, string portalNames = null, string userId = null)
        {
            if (categoryCodes.IsNullOrEmpty())
            {
                var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
                string[] codes = data.Select(x => x.Code).ToArray();
                categoryCodes = String.Join(",", codes);
            }

            userId = userId.IsNullOrEmpty() ? _userContext.UserId : userId;

            var query = "";
            if (_repo.UserContext.IsSystemAdmin && showAllTaskForAdmin)
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                t.""Id"" as TaskActionId, so.""Name"" as ServiceOwner,l.""Name"" as TaskStatusName,u.""Name"" as AssigneeUserName
                ,t.""TaskNo"" as TaskNo,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName
                ,t.""TemplateCode"" as TemplateMasterCode,t.""TaskSubject""
                from public.""NtsService"" as s
                join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'            
                join public.""User"" as so on s.""RequestedByUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'          
                join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'            
			    join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                where s.""IsDeleted""=false and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' --and t.""AssignedToUserId""='{_repo.UserContext.UserId}' 
                #CODEWHERE# #TEMPCODEWHERE# #STATUSWHERE# order by s.""CreatedDate"" desc, t.""CreatedDate"" desc ";
            }
            else
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                t.""Id"" as TaskActionId, so.""Name"" as ServiceOwner,l.""Name"" as TaskStatusName,u.""Name"" as AssigneeUserName
                ,t.""TaskNo"" as TaskNo,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName
                ,t.""TemplateCode"" as TemplateMasterCode,t.""TaskSubject""
                from public.""NtsService"" as s
                join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""User"" as so on s.""RequestedByUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""NtsTask"" as t on s.""Id""=t.""ParentServiceId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'          
                join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'            
			    join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                where s.""IsDeleted""=false and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""AssignedToUserId""='{userId}' 
                #CODEWHERE# #TEMPCODEWHERE# #STATUSWHERE# order by s.""CreatedDate"" desc, t.""CreatedDate"" desc  ";
            }


            //var catCodeWhere = $@"and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}')";
            var catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                categoryCodes = categoryCodes.Replace(",", "','");
                catCodeWhere = $@"and stc.""Code"" in ('{categoryCodes}')";
            }
            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                templateCodes = templateCodes.Replace(",", "','");
                temCodeWhere = $@"and st.""Code"" in ('{templateCodes}')";
            }
            query = query.Replace("#TEMPCODEWHERE#", temCodeWhere);

            var statuswhere = "";
            if (status.IsNotNullAndNotEmpty())
            {
                status = status.Replace(",", "','");
                statuswhere = $@" and l.""Code"" in ('{status}') ";
            }
            query = query.Replace("#STATUSWHERE#", statuswhere);

            var result = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return result;
        }

        public async Task<IList<NtsServiceIndexPageViewModel>> GetServiceCountByServiceTemplateCodesData(string categoryCodes, string portalId, bool isIncluded = false)
        {
            if (categoryCodes.IsNullOrEmpty())
            {
                var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
                string[] codes = data.Select(x => x.Code).ToArray();
                categoryCodes = String.Join(",", codes);
            }

            var query = "";
            if (_repo.UserContext.IsSystemAdmin)
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'SERVICE_STATUS_INPROGRESS' or l.""Code"" = 'SERVICE_STATUS_OVERDUE' #DRAFT# then 1 end) as CreatedByMeInProgreessOverDueCount,
count(case when l.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CreatedByMeCompleteCount,
count(case when l.""Code"" = 'SERVICE_STATUS_REJECT' or l.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as CreatedOrRequestedByMeRejectCancelCloseCount,
count(s.""Id"") as TotalCount

from public.""NtsService"" as s
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }
            else
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'SERVICE_STATUS_INPROGRESS' or l.""Code"" = 'SERVICE_STATUS_OVERDUE' #DRAFT# then 1 end) as CreatedByMeInProgreessOverDueCount,
count(case when l.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CreatedByMeCompleteCount,
count(case when l.""Code"" = 'SERVICE_STATUS_REJECT' or l.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as CreatedOrRequestedByMeRejectCancelCloseCount,
count(s.""Id"") as TotalCount

from public.""NtsService"" as s
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId""='{_repo.UserContext.UserId}') and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }

            var draftCode = "";
            if (isIncluded)
            {
                draftCode = $@" or l.""Code"" = 'SERVICE_STATUS_DRAFT' ";
            }

            query = query.Replace("#DRAFT#", draftCode);

            var catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}') ";
            //if (categoryCodes.IsNotNullAndNotEmpty())
            //{
            //    categoryCodes = categoryCodes.Replace(",", "','");
            //    //serTempCodes = String.Concat("'", serTempCodes, "'");
            //    catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes}') ";
            //}
            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var result = await _queryRepo.ExecuteQueryList<NtsServiceIndexPageViewModel>(query, null);

            return result;
        }
        public async Task<IList<ServiceViewModel>> GetServiceListByServiceCategoryCodesData(string categoryCodes, string status, string portalId)
        {
            if (categoryCodes.IsNullOrEmpty())
            {
                var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
                string[] codes = data.Select(x => x.Code).ToArray();
                categoryCodes = String.Join(",", codes);
            }

            var query = "";
            if (_repo.UserContext.IsSystemAdmin)
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                 so.""Name"" as OwnerDisplayName,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName, sl.""Code"" as ServiceStatusCode
,s.""TemplateCode"" as TemplateMasterCode
            from public.""NtsService"" as s
join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'            
join public.""User"" as so on s.""OwnerUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'

            where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            #CODEWHERE# #STATUSWHERE# order by s.""CreatedDate"" desc ";
            }
            else
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                so.""Name"" as OwnerDisplayName,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName, sl.""Code"" as ServiceStatusCode
,s.""TemplateCode"" as TemplateMasterCode
            from public.""NtsService"" as s
join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as so on s.""OwnerUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'

            where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' and (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId""='{_repo.UserContext.UserId}')
            #CODEWHERE# #STATUSWHERE# order by s.""CreatedDate"" desc ";
            }

            var catCodeWhere = $@"and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}')";
            //if (categoryCodes.IsNotNullAndNotEmpty())
            //{
            //    categoryCodes = categoryCodes.Replace(",", "','");
            //    //serTempCodes = String.Concat("'", serTempCodes, "'");

            //    catCodeWhere = $@"and stc.""Code"" in ('{categoryCodes}')";
            //}
            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var statuswhere = "";
            if (status.IsNotNullAndNotEmpty())
            {
                status = status.Replace(",", "','");
                statuswhere = $@" and sl.""Code"" in ('{status}') ";
            }
            query = query.Replace("#STATUSWHERE#", statuswhere);

            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return result;
        }

        public async Task<IList<ServiceViewModel>> GetServiceListWithDepartment(string categoryCodes = null, string templateCodes = null, string portalNames = null, string moduleCodes = null, string status = null, string departmentId = null, string templateId = null, string userId = null)
        {
            //if (categoryCodes.IsNullOrEmpty())
            //{
            //    var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
            //    string[] codes = data.Select(x => x.Code).ToArray();
            //    categoryCodes = String.Join(",", codes);
            //}

            var query = "";
            if (_repo.UserContext.IsSystemAdmin)
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                 so.""Name"" as OwnerDisplayName,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName, sl.""Code"" as ServiceStatusCode
,s.""TemplateCode"" as TemplateMasterCode,d.""DepartmentName""
            from public.""NtsService"" as s
join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Module"" as m on st.""ModuleId""=m.""Id"" and m.""IsDeleted""=false and m.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'            
join public.""User"" as so on s.""OwnerUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as p on so.""Id""=p.""UserId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as d on a.""DepartmentId""=d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Portal"" as por on s.""PortalId""=por.""Id"" and por.""IsDeleted""=false and por.""CompanyId""='{_repo.UserContext.CompanyId}'

            where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            #CODEWHERE# #TEMPCODEWHERE# #MODCODEWHERE# #PORTALWHERE# #STATUSWHERE# #DEPTWHERE# #TEMPWHERE# #USERWHERE# order by s.""CreatedDate"" desc ";
            }
            else
            {
                query = $@"select s.""ServiceNo"" as ServiceNo,s.""Id"",s.""CreatedDate"",s.""DueDate"",st.""DisplayName"" as ServiceName,st.""Code"" as TemplateCode,
                so.""Name"" as OwnerDisplayName,coalesce(s.""WorkflowStatus"",sl.""Name"") as WorkflowStatus, sl.""Name"" as ServiceStatusName, sl.""Code"" as ServiceStatusCode
,s.""TemplateCode"" as TemplateMasterCode,d.""DepartmentName""
            from public.""NtsService"" as s
join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Module"" as m on st.""ModuleId""=m.""Id"" and m.""IsDeleted""=false and m.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""TemplateCategory"" as stc on st.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""ServiceTemplate"" as sert on st.""Id""=sert.""TemplateId"" and sert.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as sl on sl.""Id""=s.""ServiceStatusId"" and sl.""IsDeleted""=false and sl.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as so on s.""OwnerUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as p on so.""Id""=p.""UserId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as d on a.""DepartmentId""=d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Portal"" as por on s.""PortalId""=por.""Id"" and por.""IsDeleted""=false and por.""CompanyId""='{_repo.UserContext.CompanyId}'
            where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' and (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId""='{_repo.UserContext.UserId}')
            #CODEWHERE# #TEMPCODEWHERE# #MODCODEWHERE# #PORTALWHERE# #STATUSWHERE# #DEPTWHERE# #TEMPWHERE# #USERWHERE# order by s.""CreatedDate"" desc ";
            }

            var catCodeWhere = categoryCodes.IsNotNullAndNotEmpty() ? $@"and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}')" : "";

            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var tempCodeWhere = templateCodes.IsNotNullAndNotEmpty() ? $@"and st.""Code"" in ('{templateCodes.Replace(",", "','")}')" : "";

            query = query.Replace("#TEMPCODEWHERE#", tempCodeWhere);

            string modCodeWhere = moduleCodes.IsNotNullAndNotEmpty() ? $@" and m.""Code"" in ('{ moduleCodes.Trim(',').Replace(",", "','")}')" : "";

            query = query.Replace("#MODCODEWHERE#", modCodeWhere);

            var portalWhere = portalNames.IsNotNullAndNotEmpty() ? $@" and por.""Name"" in ('{portalNames.Replace(",", "','")}')" : "";

            query = query.Replace("#PORTALWHERE#", portalWhere);

            var statuswhere = status.IsNotNullAndNotEmpty() ? $@" and sl.""Code"" in ('{status.Replace(",", "','")}') " : "";

            query = query.Replace("#STATUSWHERE#", statuswhere);

            var deptwhere = departmentId.IsNotNullAndNotEmpty() ? $@" and d.""Id""='{departmentId}' " : "";
            query = query.Replace("#DEPTWHERE#", deptwhere);

            var tempwhere = templateId.IsNotNullAndNotEmpty() ? $@" and st.""Id""='{templateId}' " : "";
            query = query.Replace("#TEMPWHERE#", tempwhere);

            var userwhere = userId.IsNotNullAndNotEmpty() ? $@" and (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') " : "";
            query = query.Replace("#USERWHERE#", userwhere);

            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return result;
        }

        public async Task<IList<NtsServiceIndexPageViewModel>> GetServiceCountWithDepartment(string categoryCodes = null, string templateCodes = null, string moduleCodes = null, string portalNames = null, string departmentId = null, string templateId = null, string userId = null)
        {
            //if (categoryCodes.IsNullOrEmpty())
            //{
            //    var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
            //    string[] codes = data.Select(x => x.Code).ToArray();
            //    categoryCodes = String.Join(",", codes);
            //}

            var query = "";
            if (_repo.UserContext.IsSystemAdmin)
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'SERVICE_STATUS_INPROGRESS' or l.""Code"" = 'SERVICE_STATUS_OVERDUE' then 1 end) as CreatedByMeInProgreessOverDueCount,
count(case when l.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CreatedByMeCompleteCount,
count(case when l.""Code"" = 'SERVICE_STATUS_REJECT' or l.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as CreatedOrRequestedByMeRejectCancelCloseCount,
count(s.""Id"") as TotalCount

from public.""NtsService"" as s
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false and m.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as so on s.""OwnerUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as p on so.""Id""=p.""UserId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as d on a.""DepartmentId""=d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Portal"" as por on s.""PortalId""=por.""Id"" and por.""IsDeleted""=false and por.""CompanyId""='{_repo.UserContext.CompanyId}'

where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE# #TEMPCODEWHERE# #MODCODEWHERE# #PORTALWHERE# #DEPTWHERE# #TEMPWHERE# #USERWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }
            else
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
count(case when l.""Code"" = 'SERVICE_STATUS_INPROGRESS' or l.""Code"" = 'SERVICE_STATUS_OVERDUE' then 1 end) as CreatedByMeInProgreessOverDueCount,
count(case when l.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CreatedByMeCompleteCount,
count(case when l.""Code"" = 'SERVICE_STATUS_REJECT' or l.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as CreatedOrRequestedByMeRejectCancelCloseCount,
count(s.""Id"") as TotalCount

from public.""NtsService"" as s
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false and m.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as so on s.""OwnerUserId""=so.""Id"" and so.""IsDeleted""=false and so.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as p on so.""Id""=p.""UserId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as d on a.""DepartmentId""=d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Portal"" as por on s.""PortalId""=por.""Id"" and por.""IsDeleted""=false and por.""CompanyId""='{_repo.UserContext.CompanyId}'
where (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId""='{_repo.UserContext.UserId}') and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE# #TEMPCODEWHERE# #MODCODEWHERE# #PORTALWHERE# #DEPTWHERE# #TEMPWHERE# #USERWHERE#
group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }

            //var draftCode = "";
            //if (isIncluded)
            //{
            //    draftCode = $@" or l.""Code"" = 'SERVICE_STATUS_DRAFT' ";
            //}

            //query = query.Replace("#DRAFT#", draftCode);

            var catCodeWhere = categoryCodes.IsNotNullAndNotEmpty() ? $@" and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}') " : "";
            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var tempCodeWhere = templateCodes.IsNotNullAndNotEmpty() ? $@" and tem.""Code"" in ('{templateCodes.Replace(",", "','")}')" : "";
            query = query.Replace("#TEMPCODEWHERE#", tempCodeWhere);

            string modCodeWhere = moduleCodes.IsNotNullAndNotEmpty() ? $@" and m.""Code"" in ('{ moduleCodes.Trim(',').Replace(",", "','")}')" : "";
            query = query.Replace("#MODCODEWHERE#", modCodeWhere);

            var portalWhere = portalNames.IsNotNullAndNotEmpty() ? $@" and por.""Name"" in ('{portalNames.Replace(",", "','")}')" : "";
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var deptwhere = departmentId.IsNotNullAndNotEmpty() ? $@" and d.""Id""='{departmentId}' " : "";
            query = query.Replace("#DEPTWHERE#", deptwhere);

            var tempwhere = templateId.IsNotNullAndNotEmpty() ? $@" and tem.""Id""='{templateId}' " : "";
            query = query.Replace("#TEMPWHERE#", tempwhere);

            var userwhere = userId.IsNotNullAndNotEmpty() ? $@" and (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') " : "";
            query = query.Replace("#USERWHERE#", userwhere);

            var result = await _queryRepo.ExecuteQueryList<NtsServiceIndexPageViewModel>(query, null);

            return result;
        }
        public async Task<IList<TaskViewModel>> GetServiceAdhocTaskGridData(DataSourceRequest request, string adhocTaskTemplateIds, string serviceId)
        {

            var query = @$" Select t.*,tmpc.""Code"" as ""TemplateCategoryCode"",tmpc.""Name"" as ""TemplateCategoryName"",ou.""Name"" as ""OwnerUserName"",ru.""Name"" as ""RequestedByUserName""
                        ,cu.""Name"" as ""CreatedByUserName"",lu.""Name"" as ""LastUpdatedByUserName"",au.""Name"" as ""AssigneeUserName""
                        , tlov.""Name"" as ""TaskStatusName"", tlov.""Code"" as ""TaskStatusCode"", 
                        tmp.""DisplayName"" as TemplateName
                        from public.""NtsTask"" as t
                        left join public.""Template"" as tmp ON t.""TemplateId""=tmp.""Id"" and tmp.""IsDeleted""=false 
                        left join public.""TemplateCategory"" as tmpc ON tmp.""TemplateCategoryId""=tmpc.""Id"" and tmpc.""IsDeleted""=false 
                        left join public.""TaskTemplate"" as tt ON tt.""TemplateId""=tmp.""Id"" and tt.""IsDeleted"" = false 
                        left join public.""LOV"" as tlov on t.""TaskStatusId""=tlov.""Id"" and tlov.""IsDeleted""=false 
                        left join public.""User"" as au ON au.""Id""=t.""AssignedToUserId"" and au.""IsDeleted""=false 
                        left join public.""User"" as ou ON ou.""Id""=t.""OwnerUserId"" and ou.""IsDeleted""=false 
                        left join public.""User"" as ru ON ru.""Id""=t.""RequestedByUserId"" and ru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=t.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=t.""LastUpdatedBy"" and lu.""IsDeleted""=false 
                        #WHERE# 
                        ORDER BY t.""LastUpdatedDate"" DESC ";
            var where = @$" WHERE 1=1 ";
            //var where = @$" WHERE 1=1 AND tt.""TaskTemplateType""=4 ";
            if (adhocTaskTemplateIds.IsNotNullAndNotEmpty())
            {
                var adhocItems = adhocTaskTemplateIds.Split(',');
                var adhocText = "";
                foreach (var item in adhocItems)
                {
                    adhocText = $"{adhocText},'{item}'";
                }
                var adhocId = $@" and tt.""Id"" IN ({adhocText.Trim(',')}) ";
                where = where + adhocId;
            }
            else
            {
                var adhocId = $@" and tt.""Id"" IN ('') ";
                where = where + adhocId;
            }
            where = where + $@" and t.""IsDeleted"" = false and t.""ParentServiceId"" = '{serviceId}'";
            query = query.Replace("#WHERE#", where);
            var querydata = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<TaskViewModel>> GetTaskSearchResultData(TaskSearchViewModel searchModel)
        {
            //var hour = GeneralExtension.ServerToLocalTimeDiff(LegalEntityCode);
            var userId = searchModel.UserId;
            var cypher = $@"Select n.""TaskNo"" as TaskNo,n.""Id"" as Id,n.""ActualSLA"",n.""TaskSLA"",n.""TaskStatusId"",s.""Id"" as ParentServiceId,n.""TaskTemplateId"" as TemplateId,tr.""DisplayName"" as TemplateName,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode,

                         ns.""Name"" as TaskStatusName,ns.""Code"" as TaskStatusCode,n.""TaskSubject"" as TaskSubject, n.""TaskDescription"" as Description, nau.""Name"" as AssigneeDisplayName ,
	                     nou.""Name"" as OwnerUserName,nou.""Id"" as OwnerUserId,nau.""Id"" as AssignedToUserId,n.""StartDate"" as StartDate,
	                     n.""DueDate"" as DueDate,n.""LastUpdatedDate"" as LastUpdatedDate,'Owner' as TemplateUserType,m.""Name"" as ModuleName,m.""Id"" as ModuleId,
	                      n.""CompletedDate"" as CompletionDate, n.""CanceledDate"" as CancelDate, n.""CreatedDate"" as CreatedDate, n.""ClosedDate"", n.""ReminderDate"",
                        s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"" as ServiceName, su.""Name"" as ServiceOwner,m.""Code"" as ModuleCode,n.""PortalId"" as PortalId
                        from public.""NtsTask"" as n
                        join public.""User"" as nou ON nou.""Id""=n.""OwnerUserId"" and nou.""IsDeleted""=false and nou.""Id""='{userId}' and n.""IsDeleted""=false 
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false 
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
                        left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false                        
                        left join public.""LOV"" as ns on n.""TaskStatusId""=ns.""Id"" and ns.""IsDeleted""=false 
                        left join public.""User"" as nau ON nau.""Id""=n.""AssignedToUserId"" and nau.""IsDeleted""=false                       
                        left join public.""User"" as nru ON nru.""Id""=n.""RequestedByUserId"" and nru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=n.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=n.""LastUpdatedBy"" and lu.""IsDeleted""=false 
						left join public.""NtsService"" as s ON s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false 
						left join public.""User"" as su ON su.""Id""=s.""OwnerUserId"" and su.""IsDeleted""=false
                        join public.""Portal"" as p on p.""Id""=n.""PortalId"" #PORTALWHERE#
                        
                    Union
Select n.""TaskNo"" as TaskNo,n.""Id"" as Id,n.""ActualSLA"",n.""TaskSLA"",n.""TaskStatusId"",s.""Id"" as ParentServiceId,n.""TaskTemplateId"" as TemplateId,tr.""DisplayName"" as TemplateName,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode,

                         ns.""Name"" as TaskStatusName,ns.""Code"" as TaskStatusCode,n.""TaskSubject"" as TaskSubject, n.""TaskDescription"" as Description, nau.""Name"" as AssigneeDisplayName ,
	                     nou.""Name"" as OwnerUserName,nou.""Id"" as OwnerUserId,nau.""Id"" as AssignedToUserId,n.""StartDate"" as StartDate,
	                     n.""DueDate"" as DueDate,n.""LastUpdatedDate"" as LastUpdatedDate,'Assignee' as TemplateUserType,m.""Name"" as ModuleName,m.""Id"" as ModuleId,
	                      n.""CompletedDate"" as CompletionDate, n.""CanceledDate"" as CancelDate, n.""CreatedDate"" as CreatedDate, n.""ClosedDate"", n.""ReminderDate"",
                        s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"" as ServiceName, su.""Name"" as ServiceOwner,m.""Code"" as ModuleCode,n.""PortalId"" as PortalId

from public.""NtsTask"" as n
   join public.""User"" as nau ON nau.""Id""=n.""AssignedToUserId"" and nau.""IsDeleted""=false  and nau.""Id""='{userId}' and n.""IsDeleted""=false                        
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false 
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
                        left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false                       
                        left join public.""LOV"" as ns on n.""TaskStatusId""=ns.""Id"" and ns.""IsDeleted""=false 
                        left join public.""User"" as nou ON nou.""Id""=n.""OwnerUserId"" and nou.""IsDeleted""=false       
                        left join public.""User"" as nru ON nru.""Id""=n.""RequestedByUserId"" and nru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=n.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=n.""LastUpdatedBy"" and lu.""IsDeleted""=false 
						left join public.""NtsService"" as s ON s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false 
						left join public.""User"" as su ON su.""Id""=s.""OwnerUserId"" and su.""IsDeleted""=false  
                        join public.""Portal"" as p on p.""Id""=n.""PortalId"" #PORTALWHERE#
                        where ns.""Code""<>'TASK_STATUS_DRAFT'
                        
                    Union
                    Select n.""TaskNo"" as TaskNo,n.""Id"" as Id,n.""ActualSLA"",n.""TaskSLA"",n.""TaskStatusId"",s.""Id"" as ParentServiceId,n.""TaskTemplateId"" as TemplateId,tr.""DisplayName"" as TemplateName,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode,

                         ns.""Name"" as TaskStatusName,ns.""Code"" as TaskStatusCode,n.""TaskSubject"" as TaskSubject, n.""TaskDescription"" as Description, nau.""Name"" as AssigneeDisplayName ,
	                     nou.""Name"" as OwnerUserName,nou.""Id"" as OwnerUserId,nau.""Id"" as AssignedToUserId,n.""StartDate"" as StartDate,
	                     n.""DueDate"" as DueDate,n.""LastUpdatedDate"" as LastUpdatedDate,'Shared' as TemplateUserType,m.""Name"" as ModuleName,m.""Id"" as ModuleId,
	                      n.""CompletedDate"" as CompletionDate, n.""CanceledDate"" as CancelDate, n.""CreatedDate"" as CreatedDate, n.""ClosedDate"", n.""ReminderDate"",
                        s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"" as ServiceName, su.""Name"" as ServiceOwner,m.""Code"" as ModuleCode,n.""PortalId"" as PortalId

from public.""NtsTask"" as n
join public.""NtsTaskShared"" as nts on nts.""NtsTaskId""=n.""Id"" and nts.""IsDeleted""=false 
join public.""User"" as tsu on tsu.""Id""=nts.""SharedWithUserId"" and tsu.""IsDeleted""=false and tsu.""Id""='{userId}' and n.""IsDeleted""=false  
left join public.""User"" as nau ON nau.""Id""=n.""AssignedToUserId"" and nau.""IsDeleted""=false                   
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false 
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
                        left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false                         
                        left join public.""LOV"" as ns on n.""TaskStatusId""=ns.""Id"" and ns.""IsDeleted""=false 
                        left join public.""User"" as nou ON nou.""Id""=n.""OwnerUserId"" and nou.""IsDeleted""=false      
                        left join public.""User"" as nru ON nru.""Id""=n.""RequestedByUserId"" and nru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=n.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=n.""LastUpdatedBy"" and lu.""IsDeleted""=false
						left join public.""NtsService"" as s ON s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false
						left join public.""User"" as su ON su.""Id""=s.""OwnerUserId"" and su.""IsDeleted""=false
                        join public.""Portal"" as p on p.""Id""=n.""PortalId"" #PORTALWHERE#
                        
                    Union
                                    Select n.""TaskNo"" as TaskNo,n.""Id"" as Id,n.""ActualSLA"",n.""TaskSLA"",n.""TaskStatusId"",s.""Id"" as ParentServiceId,n.""TaskTemplateId"" as TemplateId,tr.""DisplayName"" as TemplateName,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode,

                         ns.""Name"" as TaskStatusName,ns.""Code"" as TaskStatusCode,n.""TaskSubject"" as TaskSubject, n.""TaskDescription"" as Description, nau.""Name"" as AssigneeDisplayName ,
	                     nou.""Name"" as OwnerUserName,nou.""Id"" as OwnerUserId,nau.""Id"" as AssignedToUserId,n.""StartDate"" as StartDate,
	                     n.""DueDate"" as DueDate,n.""LastUpdatedDate"" as LastUpdatedDate,'Shared' as TemplateUserType,m.""Name"" as ModuleName,m.""Id"" as ModuleId,
	                      n.""CompletedDate"" as CompletionDate, n.""CanceledDate"" as CancelDate, n.""CreatedDate"" as CreatedDate, n.""ClosedDate"", n.""ReminderDate"",
                        s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"" as ServiceName, su.""Name"" as ServiceOwner,m.""Code"" as ModuleCode,n.""PortalId"" as PortalId

from public.""NtsTask"" as n
join public.""NtsTaskShared"" as nts on nts.""NtsTaskId""=n.""Id"" and nts.""IsDeleted""=false 
join public.""Team"" as ts on ts.""Id""=nts.""SharedWithTeamId"" and ts.""IsDeleted""=false  
join public.""TeamUser"" as tu on tu.""TeamId""=ts.""Id"" and tu.""IsDeleted""=false 
join public.""User"" as tsu on tsu.""Id""=tu.""UserId"" and tsu.""IsDeleted""=false and tsu.""Id""='{userId}' and n.""IsDeleted""=false 
left join public.""User"" as nau ON nau.""Id""=n.""AssignedToUserId"" and nau.""IsDeleted""=false                    
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false 
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
                        left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false                       
                        left join public.""LOV"" as ns on n.""TaskStatusId""=ns.""Id"" and ns.""IsDeleted""=false 
                        left join public.""User"" as nou ON nou.""Id""=n.""OwnerUserId"" and nou.""IsDeleted""=false       
                        left join public.""User"" as nru ON nru.""Id""=n.""RequestedByUserId"" and nru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=n.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=n.""LastUpdatedBy"" and lu.""IsDeleted""=false 
						left join public.""NtsService"" as s ON s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false 
						left join public.""User"" as su ON su.""Id""=s.""OwnerUserId"" and su.""IsDeleted""=false 
                        join public.""Portal"" as p on p.""Id""=n.""PortalId"" #PORTALWHERE#

 Union
                                    Select n.""TaskNo"" as TaskNo,n.""Id"" as Id,n.""ActualSLA"",n.""TaskSLA"",n.""TaskStatusId"",s.""Id"" as ParentServiceId,n.""TaskTemplateId"" as TemplateId,tr.""DisplayName"" as TemplateName,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode,

                         ns.""Name"" as TaskStatusName,ns.""Code"" as TaskStatusCode,n.""TaskSubject"" as TaskSubject, n.""TaskDescription"" as Description, nau.""Name"" as AssigneeDisplayName ,
	                     nou.""Name"" as OwnerUserName,nou.""Id"" as OwnerUserId,nau.""Id"" as AssignedToUserId,n.""StartDate"" as StartDate,
	                     n.""DueDate"" as DueDate,n.""LastUpdatedDate"" as LastUpdatedDate,'Shared' as TemplateUserType,m.""Name"" as ModuleName,m.""Id"" as ModuleId,
	                      n.""CompletedDate"" as CompletionDate, n.""CanceledDate"" as CancelDate, n.""CreatedDate"" as CreatedDate, n.""ClosedDate"", n.""ReminderDate"",
                        s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"" as ServiceName, su.""Name"" as ServiceOwner,m.""Code"" as ModuleCode,n.""PortalId"" as PortalId

from public.""NtsTask"" as n

join public.""Team"" as ts on ts.""Id""=n.""AssignedToTeamId"" and ts.""IsDeleted""=false  
join public.""TeamUser"" as tu on tu.""TeamId""=ts.""Id"" and tu.""IsDeleted""=false 
join public.""User"" as tsu on tsu.""Id""=tu.""UserId"" and tsu.""IsDeleted""=false and tsu.""Id""='{userId}' and n.""IsDeleted""=false 
left join public.""User"" as nau ON nau.""Id""=n.""AssignedToUserId"" and nau.""IsDeleted""=false                    
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false 
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
                        left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false                       
                        left join public.""LOV"" as ns on n.""TaskStatusId""=ns.""Id"" and ns.""IsDeleted""=false 
                        left join public.""User"" as nou ON nou.""Id""=n.""OwnerUserId"" and nou.""IsDeleted""=false       
                        left join public.""User"" as nru ON nru.""Id""=n.""RequestedByUserId"" and nru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=n.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=n.""LastUpdatedBy"" and lu.""IsDeleted""=false 
						left join public.""NtsService"" as s ON s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false 
						left join public.""User"" as su ON su.""Id""=s.""OwnerUserId"" and su.""IsDeleted""=false
                        join public.""Portal"" as p on p.""Id""=n.""PortalId"" #PORTALWHERE#


";
            var portalWhere = "";
            if (searchModel.PortalNames.IsNotNullAndNotEmpty())
            {
                var names = searchModel.PortalNames.Replace(",", "','");
                portalWhere = $@" and p.""Name"" in ('{names}')";
            }
            else
            {
                portalWhere = $@" and p.""Id""='{_userContext.PortalId}'";
            }
            cypher = cypher.Replace("#PORTALWHERE#", portalWhere);


            var list = await _queryRepo.ExecuteQueryList<TaskViewModel>(cypher, null);
            return list;
        }
        public async Task<NoteViewModel> IsTaskSubjectUniqueData(string templateId, string subject, string taskId)
        {
            var query = $@"select * from public.""NtsTask"" 
where ""TemplateId""='{templateId}' and  LOWER(""TaskSubject"")=LOWER('{subject}') and ""Id""<> '{taskId}' and ""IsDeleted""=false limit 1";
            var data = await _queryRepo.ExecuteQuerySingle<NoteViewModel>(query, null);
            return data;



        }
        public async Task<IList<ProjectGanttTaskViewModel>> GetDatewiseTaskSLAData(TaskSearchViewModel searchModel, string userId)
        {

            var query = $@"Select CAST(n.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(n.""TaskSLA"") as Days, Avg(n.""ActualSLA"") as ActualSLA
                        from public.""NtsTask"" as n
                        join public.""User"" as u on u.""Id""=n.""AssignedToUserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false 
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false 
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
                        left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false                       
                        left join public.""LOV"" as ns on n.""TaskStatusId""=ns.""Id"" and ns.""IsDeleted""=false 
                        left join public.""User"" as nou ON nou.""Id""=n.""OwnerUserId"" and nou.""IsDeleted""=false     
                        left join public.""User"" as nru ON nru.""Id""=n.""RequestedByUserId"" and nru.""IsDeleted""=false 
                        left join public.""User"" as cu ON cu.""Id""=n.""CreatedBy"" and cu.""IsDeleted""=false 
                        left join public.""User"" as lu ON lu.""Id""=n.""LastUpdatedBy"" and lu.""IsDeleted""=false 
						left join public.""NtsService"" as s ON s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false 
						left join public.""User"" as su ON su.""Id""=s.""OwnerUserId"" and su.""IsDeleted""=false 
						where n.""TaskStatusId""='370d3df9-5e97-4127-81e1-d7dd8fe8836a' and n.""IsDeleted""=false and n.""PortalId""='{_repo.UserContext.PortalId}'
                        and n.""DueDate""::TIMESTAMP::DATE >='{searchModel.StartDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and n.""DueDate""::TIMESTAMP::DATE <='{searchModel.DueDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        group by n.""DueDate"" ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetTaskSharedListData(string TaskId)
        {
            string query = @$"select n.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsTaskShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" AND u.""IsDeleted""= false 
                              
                              where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false 
                                union select u.""Id"" as Id,u.""Name"" as Name
                              from public.""NtsTaskShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" AND t.""IsDeleted""= false 
 join public.""TeamUser"" as tu ON tu.""TeamId"" = n.""SharedWithTeamId"" AND tu.""IsDeleted""= false 
join public.""User"" as u ON u.""Id"" = tu.""UserId"" AND u.""IsDeleted""= false 
                              where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<IList<TaskViewModel>> GetTaskByUserData(string userId)
        {

            var taskQry = string.Concat($@"select  t.*,lv.""Name"" as TaskStatusName,u.""Name"" as AssigneeDisplayName
from public.""User"" as u 
join public.""NtsTask"" as t on t.""AssignedToUserId""=u.""Id"" and t.""IsDeleted""=false 
join public.""LOV"" as lv on lv.""Id""=t.""TaskStatusId"" and lv.""IsDeleted""=false  and (lv.""Code""='TASK_STATUS_OVERDUE' or lv.""Code""='TASK_STATUS_INPROGRESS' or lv.""Code""='TASK_STATUS_DRAFT'
            or lv.""Code""='TASK_STATUS_NOTSTARTED') where u.""Id""='{userId}' and u.""IsDeleted""=false "
    );
            var task = await _queryRepo.ExecuteQueryList<TaskViewModel>(taskQry, null);
            return task;
        }
        public async Task<IList<TaskViewModel>> GetTaskListData(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null, string serId = null, string serTempCodes = null)
        {

            string query = @$"Select w.""WorkBoardName"" as WorkBoardName, t.""Id"",t.""TaskNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,a.""Name"" as AssigneeDisplayName,tl.""Name"" as TaskStatusName,tl.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",t.""ActualStartDate"",t.""CreatedDate""
                            ,coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",tl.""GroupCode"" as ""StatusGroupCode""
                            From public.""NtsTask"" as t
                            Join public.""Template"" as tem on t.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  #TemCodeWhere#
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  #ModCodeWhere#
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""User"" as a on t.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false 
                            Join public.""LOV"" as tl on t.""TaskStatusId""=tl.""Id"" and tl.""IsDeleted""=false
                            left Join public.""NtsNote"" as n on n.""Id""=t.""ParentNoteId"" and n.""IsDeleted""=false      
                            left join cms.""N_WORKBOARD_WorkBoardItem"" as wbi on wbi.""NtsNoteId"" = n.""Id""
                            left join cms.""N_WORKBOARD_WorkBoard"" as w on w.""Id"" = wbi.""WorkBoardId""
where t.""PortalId"" in ('{portalId.Replace(",", "','")}') and t.""IsDeleted""=false #UserWhere# #Where# #StatusWhere# order by t.""CreatedDate"" desc ";

            if (serTempCodes.IsNotNullAndNotEmpty())
            {
                query = @$"Select w.""WorkBoardName"" as WorkBoardName, t.""Id"",t.""TaskNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,a.""Name"" as AssigneeDisplayName,tl.""Name"" as TaskStatusName,tl.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",t.""ActualStartDate"",t.""CreatedDate""
                            ,coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",tl.""GroupCode"" as ""StatusGroupCode""
                            From public.""NtsTask"" as t
                            Join public.""Template"" as tem on t.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false 
                            join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""Template"" as st on s.""TemplateId""=st.""Id"" and st.""IsDeleted""=false #SerTemCodeWhere#
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false 
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""User"" as a on t.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false 
                            Join public.""LOV"" as tl on t.""TaskStatusId""=tl.""Id"" and tl.""IsDeleted""=false
                            left Join public.""NtsNote"" as n on n.""Id""=t.""ParentNoteId"" and n.""IsDeleted""=false      
                            left join cms.""N_WORKBOARD_WorkBoardItem"" as wbi on wbi.""NtsNoteId"" = n.""Id""
                            left join cms.""N_WORKBOARD_WorkBoard"" as w on w.""Id"" = wbi.""WorkBoardId""
                            where t.""PortalId"" in ('{portalId.Replace(",", "','")}') and t.""IsDeleted""=false #UserWhere# #Where# #StatusWhere# order by t.""CreatedDate"" desc ";
            }

            if (parentServiceId.IsNotNullAndNotEmpty())
            {
                query = $@"with recursive task as(
            select nt.""Id"",nt.""TaskNo"",nt.""TaskSubject"",nt.""CreatedDate"",nt.""DueDate"",t.""Code"" as TemplateCode,tl.""Code"" as TaskStatusCode,tl.""Name"" as TaskStatusName,nt.""TaskStatusId"",nt.""ParentServiceId"",
            o.""Name"" as OWnerUserName, a.""Name"" as AssigneeDisplayName,nt.""ActualStartDate""
            ,coalesce(nt.""CompletedDate"",nt.""RejectedDate"",nt.""CanceledDate"",nt.""ClosedDate"") as ""ActualEndDate"",tl.""GroupCode"" as ""StatusGroupCode""
            from public.""NtsService"" as s
            join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}' #TemCodeWhere# 
            join public.""NtsTask"" as nt on s.""Id""=nt.""ParentServiceId"" and nt.""ParentTaskId"" is null and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""Template"" as t on nt.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'         
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
            Join public.""User"" as o on nt.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
            Join public.""User"" as a on nt.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
            where s.""PortalId"" in ('{portalId.Replace(",", "','")}') and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""OwnerUserId""='{userId}'  and s.""ParentServiceId""= '{parentServiceId}'
            --#ServiceIdWhere#
            #StatusWhere#
            union all
            select nt.""Id"", nt.""TaskNo"",nt.""TaskSubject"",nt.""CreatedDate"",nt.""DueDate"", nt.""TemplateCode"", tl.""Code"" as TaskStatusCode,tl.""Name"" as TaskStatusName, nt.""TaskStatusId"", nt.""ParentServiceId"",
            o.""Name"" as OWnerUserName, a.""Name"" as AssigneeDisplayName,nt.""ActualStartDate""
            ,coalesce(nt.""CompletedDate"",nt.""RejectedDate"",nt.""CanceledDate"",nt.""ClosedDate"") as ""ActualEndDate"",tl.""GroupCode"" as ""StatusGroupCode""
            from public.""NtsTask"" as nt
            join task as em on em.""Id""=nt.""ParentTaskId""    
	        left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false 
            Join public.""User"" as o on nt.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
            Join public.""User"" as a on nt.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
            )
            select * from task order by ""CreatedDate"" desc";
            }

            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and tem.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            string statusCodeWhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                var status = statusCodes.Trim(',').Replace(",", "','");
                statusCodeWhere = $@" and tl.""Code"" in ('{status}')";
            }
            query = query.Replace("#StatusWhere#", statusCodeWhere);

            string parentWhere = "";
            if (parentNoteId.IsNotNullAndNotEmpty())
            {
                parentWhere = $@" and t.""ParentNoteId""='{parentNoteId}' ";
            }
            query = query.Replace("#Where#", parentWhere);

            string userWhere = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                userWhere = $@" and t.""AssignedToUserId""='{userId}' ";
            }
            query = query.Replace("#UserWhere#", userWhere);

            var sertempcodeWhere = serTempCodes.IsNotNullAndNotEmpty() ? $@" and st.""Code"" in ('{serTempCodes.Trim(',').Replace(",", "','")}') " : "";

            query = query.Replace("#SerTemCodeWhere#", sertempcodeWhere);
            //string serIdWhere = "";
            //if (serId.IsNotNullAndNotEmpty())
            //{
            //    serIdWhere = $@" and s.""Id""='{serId}' ";
            //}
            //query = query.Replace("#ServiceIdWhere#", serIdWhere);

            var result = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return result;
        }
        public async Task<IList<TaskViewModel>> GetWorkboardTaskListData(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null)
        {
            var list = new List<TaskViewModel>();
            string query = @$"Select w.""WorkBoardName"" as WorkBoardName, t.""Id"",t.""TaskNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,a.""Name"" as AssigneeDisplayName,ts.""Name"" as TaskStatusName,ts.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",t.""ActualStartDate"",t.""CreatedDate""
                            ,coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",ts.""GroupCode"" as ""StatusGroupCode""
                            From public.""NtsTask"" as t
                            Join public.""Template"" as tem on t.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  #TemCodeWhere#
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  #ModCodeWhere#
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""User"" as a on t.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false 
                            Join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false                            
                            left Join public.""NtsNote"" as n on n.""Id""=t.""ParentNoteId"" and n.""IsDeleted""=false      
                            left join cms.""N_WORKBOARD_WorkBoardItem"" as wbi on wbi.""NtsNoteId"" = n.""Id""
                            left join cms.""N_WORKBOARD_WorkBoard"" as w on w.""Id"" = wbi.""WorkBoardId""
                            where t.""PortalId"" in ('{portalId}') and t.""IsDeleted""=false and t.""AssignedToUserId""='{_userContext.UserId}' #Where# ";

            if (parentServiceId.IsNotNullAndNotEmpty())
            {
                query = $@"with recursive task as(
            select nt.""Id"",nt.""TaskNo"",nt.""TaskSubject"",nt.""CreatedDate"",nt.""DueDate"",t.""Code"" as TemplateCode,tl.""Code"" as TaskStatusCode,tl.""Name"" as TaskStatusName,nt.""TaskStatusId"",nt.""ParentServiceId"",
            o.""Name"" as OWnerUserName, a.""Name"" as AssigneeUserName,nt.""ActualStartDate""
            ,coalesce(nt.""CompletedDate"",nt.""RejectedDate"",nt.""CanceledDate"",nt.""ClosedDate"") as ""ActualEndDate"",tl.""GroupCode"" as ""StatusGroupCode""
            from public.""NtsService"" as s
            join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}' #TemCodeWhere# 
            join public.""NtsTask"" as nt on s.""Id""=nt.""ParentServiceId"" and nt.""ParentTaskId"" is null and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""Template"" as t on nt.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'         
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
            Join public.""User"" as o on nt.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
            Join public.""User"" as a on nt.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
            where s.""PortalId"" in ('{portalId}') and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""OwnerUserId""='{_userContext.UserId}'  and s.""ParentServiceId""= '{parentServiceId}'
            #StatusWhere#
            union all
            select nt.""Id"", nt.""TaskNo"",nt.""TaskSubject"",nt.""CreatedDate"",nt.""DueDate"", nt.""TemplateCode"", tl.""Code"" as TaskStatusCode,tl.""Name"" as TaskStatusName, nt.""TaskStatusId"", nt.""ParentServiceId"",
            o.""Name"" as OWnerUserName, a.""Name"" as AssigneeUserName,nt.""ActualStartDate""
            ,coalesce(nt.""CompletedDate"",nt.""RejectedDate"",nt.""CanceledDate"",nt.""ClosedDate"") as ""ActualEndDate"",tl.""GroupCode"" as ""StatusGroupCode""
            from public.""NtsTask"" as nt
            join task as em on em.""Id""=nt.""ParentTaskId""    
	        left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false 
            Join public.""User"" as o on nt.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
            Join public.""User"" as a on nt.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
            )
            select * from task order by ""CreatedDate"" desc";
            }

            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and tem.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            string statusCodeWhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                var status = statusCodes.Trim(',').Replace(",", "','");
                statusCodeWhere = $@" and tl.""Code"" in ('{status}')";
            }
            query = query.Replace("#StatusWhere#", statusCodeWhere);

            string parentWhere = "";
            if (parentNoteId.IsNotNullAndNotEmpty())
            {
                parentWhere = $@" and t.""ParentNoteId""='{parentNoteId}' ";
            }
            query = query.Replace("#Where#", parentWhere);

            var result = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return result;
        }
        public async Task<IList<TaskViewModel>> GetWorkPerformanceTaskListData(TaskSearchViewModel search)
        {
            var query = $@" select  t.*
                        ,lv.""Name"" as ""TaskStatusName"",lv.""Code"" as ""TaskStatusCode""
                        ,u.""Name"" as ""AssigneeDisplayName""
                    from public.""NtsTask"" as t
                    join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false 
                    join public.""LOV"" as lv on lv.""Id""=t.""TaskStatusId"" and lv.""IsDeleted""=false 
                    where t.""IsDeleted""=false  and u.""Id""='{search.UserId}' and t.""PortalId""='{_repo.UserContext.PortalId}'
                    and t.""StartDate""::Date>='{search.StartDate}'::Date and t.""StartDate""::Date<='{search.DueDate}'::Date
                    order by t.""StartDate""
                    ";
            var tasklist = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return tasklist;
        }
        public async Task<List<NtsLogViewModel>> GetVersionDetailsTaskData(string taskId)
        {
            var query = $@"select l.""TaskSubject"" as Subject, l.* from log.""NtsTaskLog""  as l
            join public.""NtsTask"" as n on l.""RecordId""=n.""Id"" and n.""IsDeleted""=false and l.""VersionNo""<>n.""VersionNo""
            where l.""IsDeleted""=false and l.""RecordId"" = '{taskId}' and l.""IsVersionLatest""=true order by l.""VersionNo"" desc";
            var result = await _queryRepo.ExecuteQueryList<NtsLogViewModel>(query, null);
            return result;

        }
        public async Task<List<DashboardCalendarViewModel>> GetWorkPerformanceCountData(string userId, string moduleCodes = null, DateTime? fromDate = null, DateTime? toDate = null)
        {

            var query = $@"select count(n.""Id""),n.""DueDate""::TIMESTAMP::DATE as start, n.""DueDate""::TIMESTAMP::DATE as end, 'OverDue' as StatusName 
    from public.""NtsTask"" as n
    join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
    join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
    join public.""LOV"" as ns on ns.""Id"" = n.""TaskStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='TASK_STATUS_OVERDUE'
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false    
    #WHERE#
    where n.""IsDeleted""=false and n.""AssignedToUserId""='{userId}'  and n.""PortalId""='{_userContext.PortalId}' 
    group by n.""DueDate""::TIMESTAMP::DATE
union
    select count(n.""Id""),n.""StartDate""::TIMESTAMP::DATE as start, n.""StartDate""::TIMESTAMP::DATE as end, 'Inprogress' as StatusName 
    from public.""NtsTask"" as n
    join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
    join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
    join public.""LOV"" as ns on ns.""Id"" = n.""TaskStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='TASK_STATUS_INPROGRESS'
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false  
    #WHERE#
    where n.""IsDeleted""=false and n.""AssignedToUserId""='{userId}'  and n.""PortalId""='{_userContext.PortalId}' 
    group by n.""StartDate""::TIMESTAMP::DATE
union
    select count(n.""Id""),n.""ClosedDate""::TIMESTAMP::DATE as start, n.""ClosedDate""::TIMESTAMP::DATE as end, 'Closed' as StatusName 
    from public.""NtsTask"" as n
    join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
    join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
    join public.""LOV"" as ns on ns.""Id"" = n.""TaskStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='TASK_STATUS_CLOSE'
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false    
    #WHERE#
    where n.""IsDeleted""=false and n.""AssignedToUserId""='{userId}'  and n.""PortalId""='{_userContext.PortalId}' 
    group by n.""ClosedDate""::TIMESTAMP::DATE
union
    select count(n.""Id""),n.""StartDate""::TIMESTAMP::DATE as start, n.""StartDate""::TIMESTAMP::DATE as end, 'NotStarted' as StatusName 
    from public.""NtsTask"" as n
    join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
    join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
    join public.""LOV"" as ns on ns.""Id"" = n.""TaskStatusId"" and ns.""IsDeleted""=false  and ns.""Code""='TASK_STATUS_NOTSTARTED'
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false   
    #WHERE#
    where n.""IsDeleted""=false and n.""AssignedToUserId""='{userId}'  and n.""PortalId""='{_userContext.PortalId}' 
    group by n.""StartDate""::TIMESTAMP::DATE
union    
    select count(n.""Id""), n.""ReminderDate""::TIMESTAMP::DATE as Start, n.""ReminderDate""::TIMESTAMP::DATE as End, 'Reminder' as StatusName
    from public.""NtsTask"" as n
    join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
    join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false   
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
    #WHERE# 
    where n.""IsDeleted""=false and n.""AssignedToUserId""='{userId}' and n.""PortalId""='{_userContext.PortalId}' and n.""ReminderDate"" is not null group by n.""ReminderDate""::TIMESTAMP::DATE 
union
    select count(nf.""Id""), nf.""CreatedDate""::TIMESTAMP::DATE as Start,nf.""CreatedDate""::TIMESTAMP::DATE as End, 'Notification' as StatusName
    from public.""NtsTask"" as n
    join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false 
    join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false 
    join public.""LOV"" as ns on ns.""Id"" = n.""TaskStatusId"" and ns.""IsDeleted""=false 
    join public.""Notification"" as nf on n.""Id""=nf.""ReferenceTypeId""
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false 
    #WHERE#          
    where nf.""IsDeleted"" = false and nf.""ToUserId""='{userId}' and nf.""PortalId""='{_userContext.PortalId}' 
    group by nf.""CreatedDate""::TIMESTAMP::DATE  ";

            var search = "";
            string codes = null;
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modules = moduleCodes.Split(",");
                foreach (var i in modules)
                {
                    codes += $"'{i}',";
                }
                codes = codes.Trim(',');
                if (codes.IsNotNullAndNotEmpty())
                {
                    search = $@" and m.""Code"" in (" + codes + ")";
                }
            }
            query = query.Replace("#WHERE#", search);

            var result = await _queryRepo.ExecuteQueryList<DashboardCalendarViewModel>(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetTaskUserListData(string taskId)
        {
            string query = @$"select u.""Id"",u.""Name""
from public.""NtsTask"" as s
join public.""User"" as u on u.""Id""=s.""RequestedByUserId"" and u.""IsDeleted""=false
where s.""Id""='{taskId}' and s.""IsDeleted""=false
union
select u.""Id"",u.""Name""
from public.""NtsTask"" as s
join public.""User"" as u on u.""Id""=s.""OwnerUserId"" and u.""IsDeleted""=false
where s.""Id""='{taskId}' and s.""IsDeleted""=false
union

select u.""Id"",u.""Name""
from public.""NtsTask"" as s
--join public.""NtsTask"" as t on t.""ParentServiceId""=s.""Id"" and t.""IsDeleted""=false
join public.""User"" as u on u.""Id""=s.""AssignedToUserId"" and u.""IsDeleted""=false
where s.""Id""='{taskId}' and s.""IsDeleted""=false

union

select u.""Id"",u.""Name""
from public.""NtsTask"" as s
join public.""NtsTaskShared"" as t on t.""NtsTaskId""=s.""Id"" and t.""IsDeleted""=false
join public.""User"" as u on u.""Id""=t.""SharedWithUserId"" and u.""IsDeleted""=false
where s.""Id""='{taskId}' and s.""IsDeleted""=false
union

select u.""Id"",u.""Name""
from public.""NtsTask"" as s
join public.""NtsTaskShared"" as t on t.""NtsTaskId""=s.""Id"" and t.""IsDeleted""=false
join public.""Team"" as tm on tm.""Id""=t.""SharedWithTeamId"" and tm.""IsDeleted""=false
join public.""TeamUser"" as tu on tu.""TeamId""=tm.""Id"" and tu.""IsDeleted""=false
join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
where s.""Id""='{taskId}' and s.""IsDeleted""=false

 ";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<TaskTemplateViewModel> GetFormIoData(string templateId, string taskId, string userId)
        {
            var query = @$"select ""NT"".*,""TM"".""Name"" as ""TableName"" ,""lov"".""Code"" as ""TaskStatusCode""
            ,""TM"".""Id"" as ""TableMetadataId"",""T"".""Json"" as ""Json""
            from ""public"".""TableMetadata"" as ""TM"" 
            join ""public"".""Template"" as ""T"" on ""T"".""TableMetadataId""=""TM"".""Id"" and ""T"".""IsDeleted""=false 
            join ""public"".""TaskTemplate"" as ""NT"" on ""T"".""Id""=""NT"".""TemplateId"" and ""NT"".""IsDeleted""=false 
            join ""public"".""NtsTask"" as ""N"" on ""T"".""Id""=""N"".""TemplateId"" and ""N"".""IsDeleted""=false 
            join ""public"".""LOV"" as ""lov"" on ""N"".""TaskStatusId""=""lov"".""Id"" and ""lov"".""IsDeleted""=false 
            where ""TM"".""IsDeleted""=false   and ""N"".""Id""='{taskId}'";

            var data = await _queryRepo.ExecuteQuerySingle<TaskTemplateViewModel>(query, null);
            return data;
        }
        public async Task<DataRow> GetFormIoTaskData(string data, string taskId)
        {
            var selectQuery = @$"select * from cms.""{data}"" where ""NtsTaskId""='{taskId}' limit 1";
            var dr = await _queryRepo.ExecuteQueryDataRow(selectQuery, null);
            return dr;
        }
        public async Task<List<TaskViewModel>> TaskCountForDashboardData(string userId, string bookId)
        {
            var cypher = $@"Select ns.""Code"" as TaskStatusCode,u.""Id"" as OwnerUserId,ua.""Id"" as AssignedToUserId,s.""Id""
 from public.""NtsNote"" as n
 join  public.""Template"" as t on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""NtsTask"" as s on s.""NotePlusId""=n.""Id""
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							 join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and s.""PortalId""='{_userContext.PortalId}' and   (ua.""Id""='{userId}' or u.""Id""='{userId}') #WHERE#
Union
Select  ns.""Code"" as TaskStatusCode,u.""Id"" as OwnerUserId,ua.""Id"" as AssignedToUserId,s.""Id""
 from public.""NtsService"" as n
 join  public.""Template"" as t on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""NtsTask"" as s on s.""ServicePlusId""=n.""Id"" or s.""ParentServiceId""=n.""Id"" 
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							 join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and s.""PortalId""='{_userContext.PortalId}' and   (ua.""Id""='{userId}' or u.""Id""='{userId}') #WHERE#
                           ";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var task = await _queryRepo.ExecuteQueryList<TaskViewModel>(cypher, null);
            return task;
        }
        public async Task<List<TaskViewModel>> TaskDashboardIndexData(string userId, string statusFilter = null)
        {
            var cypher = $@"Select s.""Id"" as Id,n.""Id"" as BookId, ns.""Name"" as TaskStatusName,ns.""Code"" as  TaskStatusCode,s.*,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,tr.""Code"" as TemplateCode,t.""Code"" as BookTemplateCode,'Note' as BookType
,s.""TaskDescription"" as TaskDescription 
from public.""NtsNote"" as n
 join  public.""Template"" as t on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""NtsTask"" as s on s.""NotePlusId""=n.""Id""
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							 join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and s.""PortalId""='{_userContext.PortalId}' and  (ua.""Id""='{userId}' or u.""Id""='{userId}') #Filter#

Union
Select s.""Id"" as Id,n.""Id"" as BookId, ns.""Name"" as TaskStatusName,ns.""Code"" as  TaskStatusCode,s.*,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,tr.""Code"" as TemplateCode,t.""Code"" as BookTemplateCode,'Service' as BookType
,s.""TaskDescription"" as TaskDescription 
 from public.""NtsService"" as n
 join  public.""Template"" as t on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""NtsTask"" as s on s.""ServicePlusId""=n.""Id"" or s.""ParentServiceId""=n.""Id"" 
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							 join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and s.""PortalId""='{_userContext.PortalId}' and   (ua.""Id""='{userId}' or u.""Id""='{userId}') #Filter#

                           ";

            var searchN = "";
            if (statusFilter == "PendingOverdue")
            {
                searchN = $@"and ns.""Code"" in ('TASK_STATUS_OVERDUE','TASK_STATUS_INPROGRESS')";
            }
            else if (statusFilter == "Complete")
            {
                searchN = $@"and ns.""Code"" ='TASK_STATUS_COMPLETE'";
            }
            else if (statusFilter == "Rejected")
            {
                searchN = $@"and ns.""Code"" ='TASK_STATUS_REJECT'";
            }
            cypher = cypher.Replace("#Filter#", searchN);
            var task = await _queryRepo.ExecuteQueryList<TaskViewModel>(cypher, null);
            return task;
        }
        public async Task<List<NoteViewModel>> LoadWorkBooksData(string userId, string statusFilter = null)
        {
            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,n.""NoteDescription"" as NoteDescription,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType,coalesce(n.""ParentNoteId"",n.""ParentServiceId"",n.""ParentTaskId"") as ParentId
 from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #NoteFilter#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,n.""ServiceDescription"" as NoteDescription,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType,coalesce(n.""ParentNoteId"",n.""ParentServiceId"",n.""ParentTaskId"") as ParentId
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #ServiceFilter#
                           ";
            var searchN = "";
            var searchS = "";
            if (statusFilter == "Active")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_INPROGRESS'";
                searchS = $@"and ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE','SERVICE_STATUS_COMPLETE')";
            }
            else if (statusFilter == "OverDue")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_INPROGRESS'";
                searchS = $@"and ns.""Code"" in ('SERVICE_STATUS_OVERDUE')";
            }
            else if (statusFilter == "Draft")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_DRAFT'";
                searchS = $@"and ns.""Code"" ='SERVICE_STATUS_DRAFT'";
            }
            else if (statusFilter == "Expired")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_EXPIRE'";
                searchS = $@"and ns.""Code"" ='SERVICE_STATUS_CANCEL'";
            }
            cypher = cypher.Replace("#NoteFilter#", searchN);
            cypher = cypher.Replace("#ServiceFilter#", searchS);
            var task = await _queryRepo.ExecuteQueryList<NoteViewModel>(cypher, null);
            return task;
        }
        public async Task<List<NoteViewModel>> LoadProcessBooksData(string userId, string statusFilter = null)
        {
            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,n.""NoteDescription"" as NoteDescription,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType,coalesce(n.""ParentNoteId"",n.""ParentServiceId"",n.""ParentTaskId"") as ParentId
 from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #NoteFilter#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,n.""ServiceDescription"" as NoteDescription,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType,coalesce(n.""ParentNoteId"",n.""ParentServiceId"",n.""ParentTaskId"") as ParentId
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
                             join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #ServiceFilter#
                           ";
            var searchN = "";
            var searchS = "";
            if (statusFilter == "Active")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_INPROGRESS'";
                searchS = $@"and ns.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE')";
            }
            else if (statusFilter == "OverDue")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_INPROGRESS'";
                searchS = $@"and ns.""Code"" in ('SERVICE_STATUS_OVERDUE')";
            }
            else if (statusFilter == "Completed")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_INPROGRESS'";
                searchS = $@"and ns.""Code"" in ('SERVICE_STATUS_COMPLETE')";
            }
            else if (statusFilter == "Draft")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_DRAFT'";
                searchS = $@"and ns.""Code"" ='SERVICE_STATUS_DRAFT'";
            }
            else if (statusFilter == "Expired")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_EXPIRE'";
                searchS = $@"and ns.""Code"" ='SERVICE_STATUS_CANCEL'";
            }
            else if (statusFilter == "Cancelled")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_CANCEL'";
                searchS = $@"and ns.""Code"" ='SERVICE_STATUS_CANCEL'";
            }
            cypher = cypher.Replace("#NoteFilter#", searchN);
            cypher = cypher.Replace("#ServiceFilter#", searchS);
            var task = await _queryRepo.ExecuteQueryList<NoteViewModel>(cypher, null);
            return task;
        }
        public async Task<List<NoteViewModel>> LoadProcessStageData(string userId, string statusFilter = null)
        {
            var cypher = $@"Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,n.""NoteDescription"" as NoteDescription,ns.""Code"" as NoteStatusCode ,n.""NoteSubject"" as  NoteSubject,n.""NoteNo"" as NoteNo,t.""Code"" as TemplateCode,'Note' as BookType,coalesce(n.""ParentNoteId"",n.""ParentServiceId"",n.""ParentTaskId"") as ParentId
,pn.""NoteSubject"" as  ParentSubject,pn.""NoteNo"" as ParentNo,pn.""TemplateCode"" as ParentTemplateCode,pn.""Id"" as ParentId 
from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""Id""=n.""NotePlusId"" and pn.""IsDeleted"" = false                           
join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and   ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #NoteFilter#

union 
Select n.""Id"" as Id, ns.""Name"" as NoteStatusName,n.""ServiceDescription"" as NoteDescription,ns.""Code"" as  NoteStatusCode,n.""ServiceSubject"" as  NoteSubject,n.""ServiceNo"" as NoteNo,t.""Code"" as TemplateCode,'Service' as BookType,coalesce(n.""ParentNoteId"",n.""ParentServiceId"",n.""ParentTaskId"") as ParentId
,pn.""ServiceSubject"" as  ParentSubject,pn.""ServiceNo"" as ParentNo,pn.""TemplateCode"" as ParentTemplateCode,pn.""Id"" as ParentId 
from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'                           
join public.""NtsService"" as pn on pn.""Id""=n.""ParentServiceId"" and pn.""Id""=n.""ServicePlusId"" and pn.""IsDeleted"" = false                           
join public.""LOV"" as ns on ns.""Id""=n.""ServiceStatusId""  and ns.""IsDeleted""=false 
							
							 join public.""User"" as u on u.""Id""=n.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and  ( n.""RequestedByUserId""='{userId}' or n.""OwnerUserId""='{userId}') #ServiceFilter#
                           ";
            var searchN = "";
            var searchS = "";
            if (statusFilter == "Active")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_INPROGRESS'";
                searchS = $@"and ns.""Code"" in ('SERVICE_STATUS_INPROGRESS')";
            }
            else if (statusFilter == "OverDue")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_INPROGRESS'";
                searchS = $@"and ns.""Code"" in ('SERVICE_STATUS_OVERDUE')";
            }

            else if (statusFilter == "Draft")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_DRAFT'";
                searchS = $@"and ns.""Code"" ='SERVICE_STATUS_DRAFT'";
            }
            else if (statusFilter == "Expired")
            {
                searchN = $@"and ns.""Code"" ='NOTE_STATUS_EXPIRE'";
                searchS = $@"and ns.""Code"" ='SERVICE_STATUS_CANCEL'";
            }
            cypher = cypher.Replace("#NoteFilter#", searchN);
            cypher = cypher.Replace("#ServiceFilter#", searchS);
            var task = await _queryRepo.ExecuteQueryList<NoteViewModel>(cypher, null);
            return task;
        }
        public async Task<List<NoteViewModel>> GetNextNoteSequenceNoData(string notePlusId)
        {

            var cypher = $@"Select n.""Id"" as Id
 from  public.""Template"" as t 
 join public.""NtsNote"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='2'                           
where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and n.""NotePlusId""='{notePlusId}'

union 
Select n.""Id"" as Id
 from  public.""Template"" as t 
 join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='2'                           
 where 1=1 
and n.""PortalId""='{_userContext.PortalId}' and n.""ServicePlusId""='{notePlusId}'

                           ";

            var task = await _queryRepo.ExecuteQueryList<NoteViewModel>(cypher, null);
            return task;
        }
        public async Task<long> GetNextServiceSequenceNoData(string servicePlusId)
        {
            var cypher = $@"Select count(*)
 from  public.""Template"" as t 
 left join public.""NtsService"" as n on t.""Id""=n.""TemplateId""  and n.""IsDeleted""=false  
 left join public.""NtsNote"" as nn on t.""Id""=nn.""TemplateId""   and nn.""IsDeleted""=false 
-- left join public.""NtsTask"" as nt on t.""Id""=nt.""TemplateId""  and nt.""IsDeleted""=false 
  where 1=1 and n.""PortalId""='{_userContext.PortalId}'  and t.""IsDeleted""=false
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'  
and (n.""ServicePlusId""='{servicePlusId}' or nt.""ServicePlusId""='{servicePlusId}' or nn.""ServicePlusId""='{servicePlusId}' )

                           ";

            var task = await _queryRepo.ExecuteScalar<long>(cypher, null);
            return task;
        }
        public async Task<List<NotificationViewModel>> NotificationDashboardIndexData(string userId, string bookId)
        {
            var cypher = $@"Select n1.*,n.""NoteSubject"" as BookName,n.""NoteNo"" as BookNo,f.""Name"" as From,too.""Name"" as To,tr.""Code"" as ReferenceTemplateCode,tr.""Code"" as BookTemplateCode,'Note' as PageType,n.""Id"" as BookId
                            from public.""NtsNote"" as n                         
                            join public.""Template"" as tr on tr.""Id""=n.""TemplateId""  and tr.""IsDeleted""=false and tr.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""   and tc.""IsDeleted""=false 
                            join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false 
                            join public.""LOV"" as ns on ns.""Id""=n.""NoteStatusId""  and ns.""IsDeleted""=false 
							join public.""Notification"" as n1 on n1.""ReferenceTypeId""=n.""Id""
join public.""User"" as f on n1.""FromUserId""=f.""Id"" and f.""IsDeleted""=false 
join public.""User"" as too on n1.""ToUserId""=too.""Id"" and too.""IsDeleted""=false 
                           left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  
                           where 1=1 and n.""PortalId""='{_userContext.PortalId}' and n.""IsDeleted""=false and n1.""ToUserId""='{userId}' #WHERE#
						   union 
						   Select n1.*,n.""NoteSubject"" as BookName,n.""NoteNo"" as BookNo,f.""Name"" as From,too.""Name"" as To,tr.""Code"" as ReferenceTemplateCode,t.""Code"" as BookTemplateCode,'Note' as PageType,n.""Id"" as BookId
 from public.""NtsNote"" as n
 join  public.""Template"" as t on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""NtsTask"" as s on s.""NotePlusId""=n.""Id""							
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							 join public.""Notification"" as n1 on n1.""ReferenceTypeId""=s.""Id""
join public.""User"" as f on n1.""FromUserId""=f.""Id"" and f.""IsDeleted""=false 
join public.""User"" as too on n1.""ToUserId""=too.""Id"" and too.""IsDeleted""=false 
							 join public.""User"" as ua on ua.""Id""=s.""AssignedToUserId""  and ua.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and s.""PortalId""='{_userContext.PortalId}' 
and n1.""ToUserId""='{userId}' #WHERE#

union 
						   Select n1.*,n.""NoteSubject"" as BookName,n.""NoteNo"" as BookNo,f.""Name"" as From,too.""Name"" as To,tr.""Code"" as ReferenceTemplateCode,t.""Code"" as BookTemplateCode,'Note' as PageType,n.""Id"" as BookId
 from public.""NtsNote"" as n
 join  public.""Template"" as t on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""NtsService"" as s on s.""NotePlusId""=n.""Id""							
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""ServiceStatusId""  and ns.""IsDeleted""=false 
							 join public.""Notification"" as n1 on n1.""ReferenceTypeId""=s.""Id""
							join public.""User"" as f on n1.""FromUserId""=f.""Id"" and f.""IsDeleted""=false 
join public.""User"" as too on n1.""ToUserId""=too.""Id"" and too.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and s.""PortalId""='{_userContext.PortalId}' and n1.""ToUserId""='{userId}' #WHERE#

union 
						   Select n1.*,n.""ServiceSubject"" as BookName,n.""ServiceNo"" as BookNo,f.""Name"" as From,too.""Name"" as To,tr.""Code"" as ReferenceTemplateCode,t.""Code"" as BookTemplateCode,'Service' as PageType,n.""Id"" as BookId
 from public.""NtsService"" as n
 join  public.""Template"" as t on t.""Id""=n.""TemplateId""  and t.""IsDeleted""=false  and n.""IsDeleted""=false 
and t.""ViewType"" ='{(int)((NtsViewTypeEnum)Enum.Parse(typeof(NtsViewTypeEnum), NtsViewTypeEnum.Book.ToString()))}'
                            join public.""NtsTask"" as s on s.""ServicePlusId""=n.""Id"" or s.""ParentServiceId""=n.""Id"" 						
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
                            join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId""  and tc.""IsDeleted""=false 
                             join public.""LOV"" as ns on ns.""Id""=s.""TaskStatusId""  and ns.""IsDeleted""=false 
							 join public.""Notification"" as n1 on n1.""ReferenceTypeId""=s.""Id""
							join public.""User"" as f on n1.""FromUserId""=f.""Id"" and f.""IsDeleted""=false 
join public.""User"" as too on n1.""ToUserId""=too.""Id"" and too.""IsDeleted""=false 
							left join public.""User"" as u on u.""Id""=s.""OwnerUserId""   and u.""IsDeleted""=false							
left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  where 1=1 
and s.""PortalId""='{_userContext.PortalId}' and n1.""ToUserId""='{userId}' #WHERE#

";
            var search = "";
            if (bookId.IsNotNullAndNotEmpty())
            {
                search = $@" and n.""Id""='{bookId}'";
            }
            cypher = cypher.Replace("#WHERE#", search);
            var task = await _queryRepo.ExecuteQueryList<NotificationViewModel>(cypher, null);
            return task;
        }
        public async Task<bool> UpdateStepTaskAssigneeData(string taskId, string ownerUserId)
        {
            var query = $@"Update public.""NtsTask"" set ""AssignedToUserId""='{ownerUserId}' where ""Id""='{taskId}' ";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        #endregion

        public async Task<List<IIPCameraViewModel>> GetIIPCameraData()
        {
            var query = @$" select n.*,dm.""city"" as City, dm.""locationName"" as Location,dm.""policeStation"" as PoliceStation,
                        dm.""longitude"" as Longitude,dm.""latitude"" as Latitude,dm.""ipAddress"" as IpAddress,
                        dm.""switchHostName"" as SwitchHostName,dm.""rtspLink"" as RtspLink,dm.""typeOfCamera"" as TypeOfCamera,
                        dm.""make"" as Make
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SOCIAL_SCRAPPING_DewasCamera"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='DEWAS_CAMERA' and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<IIPCameraViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IIPCameraViewModel>> GetIIPCameraDatabyIds(string ids)
        {
            var query = @$" select n.*,dm.""city"" as City, dm.""locationName"" as Location,dm.""policeStation"" as PoliceStation,
                        dm.""longitude"" as Longitude,dm.""latitude"" as Latitude,dm.""ipAddress"" as IpAddress,
                        dm.""switchHostName"" as SwitchHostName,dm.""rtspLink"" as RtspLink,dm.""typeOfCamera"" as TypeOfCamera,
                        dm.""make"" as Make
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SOCIAL_SCRAPPING_DewasCamera"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='DEWAS_CAMERA' and n.""Id"" in ('{ids}') and t.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<IIPCameraViewModel>(query, null);
            return querydata;
        }
        public async Task<List<CctvCameraViewModel>> GetCctvCameraData(DateTime? lastUpdatedDate = null)
        {
            if (lastUpdatedDate.IsNotNull())
            {
                var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as CameraName,dm.""city"" as City, dm.""locationName"" as Location,dm.""policeStation"" as PoliceStation,
                        dm.""longitude"" as Longitude,dm.""latitude"" as Latitude,dm.""ipAddress"" as IpAddress,
                        dm.""switchHostName"" as SwitchHostName,dm.""rtspLink"" as RtspLink,dm.""typeOfCamera"" as TypeOfCamera,
                        dm.""make"" as Make,n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SOCIAL_SCRAPPING_DewasCamera"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='DEWAS_CAMERA' and n.""LastUpdatedDate"">'{lastUpdatedDate}' and t.""IsDeleted""=false";
                var querydata = await _queryRepo.ExecuteQueryList<CctvCameraViewModel>(query, null);
                return querydata;
            }
            else
            {
                var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as CameraName,dm.""city"" as City, dm.""locationName"" as Location,dm.""policeStation"" as PoliceStation,
                        dm.""longitude"" as Longitude,dm.""latitude"" as Latitude,dm.""ipAddress"" as IpAddress,
                        dm.""switchHostName"" as SwitchHostName,dm.""rtspLink"" as RtspLink,dm.""typeOfCamera"" as TypeOfCamera,
                        dm.""make"" as Make,n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SOCIAL_SCRAPPING_DewasCamera"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='DEWAS_CAMERA' and t.""IsDeleted""=false ";
                var querydata = await _queryRepo.ExecuteQueryList<CctvCameraViewModel>(query, null);
                return querydata;
            }

        }

        public async Task<NtsSummaryViewModel> GetTaskSummary(string portalId, string userId)
        {
            var tquery = @$"select ts.""Code"" as ""Code"",count(s.""Id"") as ""Count""
            from public.""NtsTask"" as s
            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
            join public.""LOV"" as ts on ts.""LOVType""='LOV_TASK_STATUS' and ts.""Id""=s.""TaskStatusId"" and ts.""IsDeleted""=false 
            where s.""PortalId""='{portalId}' and s.""AssignedToUserId""='{userId}'
            group by ts.""Code""";
            var data = await _queryRepo.ExecuteQueryList<IdNameViewModel>(tquery, null);
            var result = new NtsSummaryViewModel();

            if (data != null)
            {
                var draft = data.FirstOrDefault(x => x.Code == "TASK_STATUS_DRAFT");
                if (draft != null)
                {
                    result.DraftCount = draft.Count;
                }
                var co = data.FirstOrDefault(x => x.Code == "TASK_STATUS_COMPLETE");
                if (co != null)
                {
                    result.CompletedCount = co.Count;
                }
                var ip = data.FirstOrDefault(x => x.Code == "TASK_STATUS_INPROGRESS");
                if (ip != null)
                {
                    result.InProgressCount = ip.Count;
                }
                var od = data.FirstOrDefault(x => x.Code == "TASK_STATUS_OVERDUE");
                if (od != null)
                {
                    result.OverDueCount = od.Count;
                }
                var rj = data.FirstOrDefault(x => x.Code == "TASK_STATUS_REJECT");
                if (rj != null)
                {
                    result.RejectedCount = rj.Count;
                }
                var cn = data.FirstOrDefault(x => x.Code == "TASK_STATUS_CANCEL");
                if (cn != null)
                {
                    result.CanceledCount = cn.Count;
                }

            }
            return result;

        }
        public async Task<NtsSummaryViewModel> GetServiceSummary(string portalId, string userId)
        {
            var tquery = @$"select ts.""Code"" as ""Code"",count(s.""Id"") as ""Count""
            from public.""NtsService"" as s
            join public.""Template"" as tr on tr.""Id""=s.""TemplateId""  and tr.""IsDeleted""=false  and s.""IsDeleted""=false 
            join public.""LOV"" as ts on ts.""LOVType""='LOV_SERVICE_STATUS' and ts.""Id""=s.""ServiceStatusId"" and ts.""IsDeleted""=false 
            where s.""PortalId""='{portalId}' and (s.""RequestedByUserId""='{userId}' or s.""OwnerUserId""='{userId}')
            group by ts.""Code""";
            var data = await _queryRepo.ExecuteQueryList<IdNameViewModel>(tquery, null);
            var result = new NtsSummaryViewModel();

            if (data != null)
            {
                var draft = data.FirstOrDefault(x => x.Code == "SERVICE_STATUS_DRAFT");
                if (draft != null)
                {
                    result.DraftCount = draft.Count;
                }
                var co = data.FirstOrDefault(x => x.Code == "SERVICE_STATUS_COMPLETE");
                if (co != null)
                {
                    result.CompletedCount = co.Count;
                }
                var ip = data.FirstOrDefault(x => x.Code == "SERVICE_STATUS_INPROGRESS");
                if (ip != null)
                {
                    result.InProgressCount = ip.Count;
                }
                var od = data.FirstOrDefault(x => x.Code == "SERVICE_STATUS_OVERDUE");
                if (od != null)
                {
                    result.OverDueCount = od.Count;
                }
                var rj = data.FirstOrDefault(x => x.Code == "SERVICE_STATUS_REJECT");
                if (rj != null)
                {
                    result.RejectedCount = rj.Count;
                }
                var cn = data.FirstOrDefault(x => x.Code == "SERVICE_STATUS_CANCEL");
                if (cn != null)
                {
                    result.CanceledCount = cn.Count;
                }
                var close = data.FirstOrDefault(x => x.Code == "SERVICE_STATUS_CLOSE");
                if (close != null)
                {
                    result.CanceledCount = close.Count;
                }

            }
            return result;

        }
        //public async Task<List<RoipViewModel>> GetRoipData(string host, string user, string database, string port, string password, string table)
        //{
        //    var list = new List<RoipViewModel>();
        //    string connStr = "server=" + host + ";user=" + user + ";database=" + database + ";port=" + port + ";password=" + password;
        //    MySqlConnection conn = new MySqlConnection(connStr);
        //    try
        //    {
        //        conn.Open();
        //        string sql = "select *from " + table;
        //        MySqlCommand cmd = new MySqlCommand(sql, conn);
        //        MySqlDataReader rdr = cmd.ExecuteReader();

        //        while (rdr.Read())
        //        {
        //            var model = new RoipViewModel
        //            {
        //                Id = rdr[0].ToString(),
        //                Heard = Convert.ToBoolean(rdr[1]),
        //                Type = rdr[2].ToString(),
        //                Archive = Convert.ToBoolean(rdr[3]),
        //                ChanelNo = rdr[4].ToString(),
        //                Date = rdr[5].ToString(),
        //                StartTime = rdr[6].ToString(),
        //                EndTime = rdr[7].ToString(),
        //                Duration = rdr[8].ToString(),
        //                PhoneNo = rdr[9].ToString(),
        //                InOut = rdr[10].ToString(),
        //                ImpCall = rdr[11].ToString(),
        //                FileName = rdr[12].ToString(),
        //                Comment1 = rdr[13].ToString(),
        //                Comment2 = rdr[14].ToString(),
        //                Comment3 = rdr[15].ToString(),
        //                Comment4 = rdr[16].ToString(),
        //                Comment5 = rdr[17].ToString(),
        //                Comment6 = rdr[18].ToString(),
        //                Comment7 = rdr[19].ToString(),


        //            };
        //            list.Add(model);
        //            //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
        //        }
        //        rdr.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    conn.Close();
        //    return list;

        //}
        public async Task<IList<TrendingLocationViewModel>> GetAllTrendingLocationData()
        {
            var query = @$" select n.*,dm.""latitude"" as latitude,dm.""longitude"" as longitude, dm.""socialMediaType"" as socialMediaType
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_TrendingLocation"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='TRENDING_LOCATION' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<TrendingLocationViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<string>> GetAllKeywordForHarvesting()
        {
            var query = @$" select dm.""subEventName"" as Name
                        from  public.""NtsNote"" as n 
                        join cms.""N_SWS_SubEventType"" as dm on dm.""NtsNoteId""=n.""Id"" 
                        where dm.""IsDeleted""=false and n.""IsDeleted""=false  
                        union all
                        select n.""NoteSubject"" as Name
                        from  public.""NtsNote"" as n 
                        join cms.""N_SWS_Keyword"" as dm on dm.""NtsNoteId""=n.""Id"" 
                        where dm.""IsDeleted""=false and n.""IsDeleted""=false  
                        union all
                        select n.""NoteSubject"" as Name
                        from  public.""NtsNote"" as n 
                        join cms.""N_SWS_District"" as dm on dm.""NtsNoteId""=n.""Id"" 
                        where dm.""IsDeleted""=false and n.""IsDeleted""=false    ";
            var querydata = await _queryRepo.ExecuteScalarList<string>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllDial100Event()
        {
            var query = @$" select dm.""eventTypeName"" as Name,dm.""eventTypeCode"" as Code
                        from  public.""NtsNote"" as n 
                        join cms.""N_SWS_EventType"" as dm on dm.""NtsNoteId""=n.""Id"" 
                        where dm.""IsDeleted""=false and n.""IsDeleted""=false order by dm.""eventTypeName""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllDial100SubEvent()
        {
            var query = @$" select dm.""subEventName"" as Name,dm.""subEventCode"" as Code
                        from  public.""NtsNote"" as n 
                        join cms.""N_SWS_SubEventType"" as dm on dm.""NtsNoteId""=n.""Id"" 
                        where dm.""IsDeleted""=false and n.""IsDeleted""=false  order by dm.""subEventName""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllDial100SubEvent(string eventCode)
        {
            var query = @$" select dm.""subEventName"" as Name,dm.""subEventCode"" as Code
                        from  public.""NtsNote"" as n 
                        join cms.""N_SWS_SubEventType"" as dm on dm.""NtsNoteId""=n.""Id"" 
                        where dm.""IsDeleted""=false and n.""IsDeleted""=false and dm.""eventCode""='{eventCode}'  order by dm.""subEventName""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetAllTracks()
        {
            var query = @$"SELECT tm.""Id"" as Id, n.""NoteSubject"" as Name FROM cms.""N_SWS_TrackMaster"" as tm
                            join public.""NtsNote"" as n on n.""Id"" = tm.""NtsNoteId"" and n.""IsDeleted""=false 
                            where tm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetAllKeywords(string trackName)
        {
            var query = @$"SELECT tm.""Id"" as Id, n.""NoteSubject"" as Name FROM cms.""N_SWS_Keyword"" as tm
                            join public.""NtsNote"" as n on n.""Id"" = tm.""NtsNoteId"" and n.""IsDeleted""=false 
                            join cms.""N_SWS_TrackMaster"" as t on t.""Id"" = tm.""track"" and t.""IsDeleted""=false 
                            join public.""NtsNote"" as pn on pn.""Id"" = t.""NtsNoteId"" and pn.""IsDeleted""=false 
                            where pn.""NoteSubject"" = '{trackName}' and tm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllFacebookUserData()
        {
            var query = @$" select n.""NoteSubject"" as Name,dm.""userId"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_FacebookUser"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='FACEBOOK_USER' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetFacebookCredentialData()
        {
            var query = @$" select n.""NoteSubject"" as Name,dm.""password"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_FacebookCredential"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='FACEBOOK_CREDENTIAL' and t.""IsDeleted""=false  order by n.""CreatedDate"" desc limit 1 ";
            var querydata = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<IdNameViewModel>> GetSEBIInvestorListedCompanyIdNameList(string entityStatus)
        {
            var query = @$"select ereg.""Id"" as ""Id"", ereg.""nameOfTheCompany"" as ""Name""
                        from cms.""N_SEBIAdmin_SEBIEntityRegistration"" as ereg
                        left join public.""LOV"" as ets on ets.""Id""=ereg.""entityType"" and ets.""IsDeleted""=false
                        where ereg.""IsDeleted""=false #WHEREENTITYTYPE# ";
            var entstatus = "";
            if (entityStatus.IsNotNullAndNotEmpty())
            {
                entstatus = $@" and ets.""Id""='{entityStatus}' ";
            }
            query = query.Replace("#WHEREENTITYTYPE#", entityStatus);
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<SEBIEntityRegistrationViewModel> GetSEBIInvestorListedCompanyData(string listedCompanyId)
        {
            var query = $@" Select s.""Id"" as ServiceId, s.""ServiceNo"" as ServiceNo, ets.""Name"" as EntityTypeName, ereg.""nameOfTheCompany"" as NameOfTheCompany, ereg.""registeredOfficeAddressOfCompany"" as RegisteredOfficeAddressOfCompany
                            , ereg.""selectState"" as SelectState, ereg.""selectStatusOfTheCompany"" as SelectStatusOfTheCompany, ereg.""issueSize"" as IssueSize
                            , ereg.""nameOfRegistrarsTransferAgentIfThroughRta"" as NameOfRegistrarsTransferAgentIfThroughRta, ereg.""stockExchangeForComplaints"" as StockExchangeForComplaints
                            , ereg.""previousNamesOfTheCompany"" as PreviousNamesOfTheCompany, ereg.""bombayStockExchangeLtd"" as BombayStockExchangeLtd
                            , ereg.""calcuttaStockExchangeLtd"" as CalcuttaStockExchangeLtd, ereg.""metroplitanStockExchangeLimitedPreviousNameMcxStockExchangeLtd"" as MetroplitanStockExchangeLimitedPreviousNameMcxStockExchangeLtd
                            , ereg.""nationalStockExchangeOfIndiaLtd"" as NationalStockExchangeOfIndiaLtd, ereg.""panOfTheCompany"" as PANOfTheCompany
                            , ereg.""cinOfTheCompany"" as CINOfTheCompany, ereg.""designation"" as Designation, ereg.""mobileNumber"" as MobileNumber
                            , ereg.""emailAddressPrimary"" as EmailAddressPrimary, ereg.""emailAddressAlternate"" as EmailAddressAlternate, ereg.""dateOfIncorporationOfTheCompany"" as DateOfIncorporationOfTheCompany
                            , ereg.""pincode"" as Pincode, ereg.""compliancesDealingOfficer"" as CompliancesDealingOfficer 
                            from cms.""N_SEBIAdmin_SEBIEntityRegistration"" as ereg
                            join public.""NtsService"" as s on s.""UdfNoteId""=ereg.""NtsNoteId"" and s.""IsDeleted""=false
                            left join public.""LOV"" as ets on ets.""Id""=ereg.""entityType"" and ets.""IsDeleted""=false
                            where ereg.""IsDeleted""=false and ereg.""Id""='{listedCompanyId}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<SEBIEntityRegistrationViewModel>(query, null);
            return querydata;
        }

        public async Task<IList<NtsServiceIndexPageViewModel>> GetTemplatesListWithServiceCount(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllServicesForAdmin = false)
        {
            //if (categoryCodes.IsNullOrEmpty())
            //{
            //    var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
            //    string[] codes = data.Select(x => x.Code).ToArray();
            //    categoryCodes = String.Join(",", codes);
            //}

            var query = $@"select tem.""DisplayName"" as DisplayName,tem.""Code"" as TemplateCode,
count(case when l.""Code"" = 'SERVICE_STATUS_INPROGRESS' or l.""Code"" = 'SERVICE_STATUS_OVERDUE' then 1 end) as CreatedByMeInProgreessOverDueCount,
count(case when l.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CreatedByMeCompleteCount,
count(case when l.""Code"" = 'SERVICE_STATUS_REJECT' or l.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as CreatedOrRequestedByMeRejectCancelCloseCount,
count(case when l.""Code"" != 'SERVICE_STATUS_DRAFT' then 1 end) as TotalCount

from public.""NtsService"" as s
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE# #TEMPCODEWHERE# #GROUPCODEWHERE# #USERWHERE#
group by tem.""DisplayName"", tem.""Code"" ";

            //if (categoryCodes.IsNotNullAndNotEmpty())
            //{
            //    categoryCodes = categoryCodes.Replace(",", "','");
            //    //serTempCodes = String.Concat("'", serTempCodes, "'");
            //    catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes}') ";
            //}

            var catCodeWhere = catCodes.IsNotNullAndNotEmpty() ? $@" and stc.""Code"" in ('{catCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var tempCodeWhere = templateCodes.IsNotNullAndNotEmpty() ? $@" and tem.""Code"" in ('{templateCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#TEMPCODEWHERE#", tempCodeWhere);

            var groupCodeWhere = groupCodes.IsNotNullAndNotEmpty() ? $@" and tem.""GroupCode"" in ('{groupCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#GROUPCODEWHERE#", groupCodeWhere);

            var userwhere = !showAllServicesForAdmin ? $@" and s.""RequestedByUserId""='{_repo.UserContext.UserId}' " : "";

            query = query.Replace("#USERWHERE#", userwhere);

            var result = await _queryRepo.ExecuteQueryList<NtsServiceIndexPageViewModel>(query, null);

            return result;
        }

        public async Task<IList<NtsTaskIndexPageViewModel>> GetTemplatesListWithTaskCount(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllTasksForAdmin = false)
        {
            //if (categoryCodes.IsNullOrEmpty())
            //{
            //    var data = await _templateCategoryBusiness.GetList(x => x.AllowedPortalIds.Contains(portalId) && x.TemplateType == TemplateTypeEnum.Service);
            //    string[] codes = data.Select(x => x.Code).ToArray();
            //    categoryCodes = String.Join(",", codes);
            //}

            var query = $@"select tem.""DisplayName"" as DisplayName,tem.""Code"" as TemplateCode,
count(case when l.""Code"" = 'TASK_STATUS_INPROGRESS' or l.""Code"" = 'TASK_STATUS_OVERDUE' then 1 end) as AssignedToMeInProgreessOverDueCount,
count(case when l.""Code"" = 'TASK_STATUS_COMPLETE' then 1 end) as AssignedToMeCompleteCount,
count(case when l.""Code"" = 'TASK_STATUS_REJECT' or l.""Code"" = 'TASK_STATUS_CANCEL' then 1 end) as AssignedToMeRejectCancelCloseCount,
count(case when l.""Code"" != 'TASK_STATUS_DRAFT' then 1 end) as TotalCount

from public.""NtsTask"" as t
join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
#CODEWHERE# #TEMPCODEWHERE# #GROUPCODEWHERE# #USERWHERE#
group by tem.""DisplayName"", tem.""Code""
            ";

            //if (categoryCodes.IsNotNullAndNotEmpty())
            //{
            //    categoryCodes = categoryCodes.Replace(",", "','");
            //    //serTempCodes = String.Concat("'", serTempCodes, "'");
            //    catCodeWhere = $@" and stc.""Code"" in ('{categoryCodes}') ";
            //}

            var catCodeWhere = catCodes.IsNotNullAndNotEmpty() ? $@" and stc.""Code"" in ('{catCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#CODEWHERE#", catCodeWhere);

            var tempCodeWhere = templateCodes.IsNotNullAndNotEmpty() ? $@" and tem.""Code"" in ('{templateCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#TEMPCODEWHERE#", tempCodeWhere);

            var groupCodeWhere = groupCodes.IsNotNullAndNotEmpty() ? $@" and tem.""GroupCode"" in ('{groupCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#GROUPCODEWHERE#", groupCodeWhere);

            var userwhere = !showAllTasksForAdmin ? $@" and t.""AssignedToUserId""='{_repo.UserContext.UserId}' " : "";

            query = query.Replace("#USERWHERE#", userwhere);

            var result = await _queryRepo.ExecuteQueryList<NtsTaskIndexPageViewModel>(query, null);

            return result;
        }


        public async Task<IList<TaskViewModel>> GetTaskListWithHoursSpentData(string portalId, string statusCodes = null, string userId = null)
        {
            //            string query = $@"Select 
            //case when te.""NtsTaskId"" is not null then 
            //TO_CHAR((EXTRACT(EPOCH FROM (te.""EndDate"" - te.""StartDate""))|| ' second')::interval, 'HH24:MI') 
            //else '00:00' end as HoursSpent,TO_CHAR((EXTRACT(EPOCH FROM (t.""TaskSLA""))|| ' second')::interval, 'HH24:MI') as TaskSLAValue,
            //o.""PhotoId"" as OwnerUserPhotoId,o.""Name"" as OwnerUserName,
            //case when ts.""Name""='Draft' then 'bg-cyan'
            //when ts.""Name""='In Progress' then 'bg-blue'
            //when ts.""Name""='Overdue' then 'bg-yellow'
            //when ts.""Name""='Completed' then 'bg-teal'
            //else 'bg-red' end as TitleCSS,
            //(select count(c.""Id"") from public.""NtsTaskComment"" as c where c.""NtsTaskId""=t.""Id"") as CommentsCount
            //,t.*
            //from public.""NtsTask"" as t
            //join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""Code"" in ('{statusCodes.Replace(",", "','")}')
            //left join public.""NtsTaskTimeEntry"" as te on t.""Id""=te.""NtsTaskId"" and te.""IsDeleted""=false
            //join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false
            //where t.""PortalId""='{portalId}' and t.""AssignedToUserId""='{userId}'
            //and t.""IsDeleted""=false";

            string query = $@"Select 
case when TaskTimeSpent is not null then TO_CHAR(TaskTimeSpent,'HH24:MI') else '00:00' end as HoursSpent,
TO_CHAR((EXTRACT(EPOCH FROM (t.""TaskSLA""))|| ' second')::interval, 'HH24:MI') as TaskSLAValue,
o.""PhotoId"" as OwnerUserPhotoId,o.""Name"" as OwnerUserName,
case when ts.""Name""='Draft' then 'bg-cyan'
when ts.""Name""='In Progress' then 'bg-blue'
when ts.""Name""='Overdue' then 'bg-yellow'
when ts.""Name""='Completed' then 'bg-teal'
else 'bg-red' end as TitleCSS,
(select count(c.""Id"") from public.""NtsTaskComment"" as c where c.""NtsTaskId""=t.""Id"") as CommentsCount
,t.*
from public.""NtsTask"" as t
join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""Code"" in ('{statusCodes.Replace(",", "','")}')
left join (select ""NtsTaskId"",sum((EXTRACT(EPOCH FROM (""EndDate"" - ""StartDate""))|| ' second')::interval) as TaskTimeSpent
from public.""NtsTaskTimeEntry"" where ""IsDeleted""=false group by ""NtsTaskId"" ) ntstt on ntstt.""NtsTaskId""=t.""Id""
join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false
where t.""PortalId""='{portalId}' and t.""AssignedToUserId""='{userId}'
and t.""IsDeleted""=false";

            var result = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);

            return result;

        }

        public async Task<List<ProjectDashboardChartViewModel>> GetStatusWiseChartByTemplateCode(string templateCode, string requestby = null)
        {
            var query = $@"Select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"",tem.""Code"",
case when lov.""Code""='SERVICE_STATUS_DRAFT' then '#17a2b8'
when lov.""Code""='SERVICE_STATUS_COMPLETE' then '#13b713'
when lov.""Code""='SERVICE_STATUS_INPROGRESS' then '#007bff'
when lov.""Code""='SERVICE_STATUS_OVERDUE' then '#ffc107'
when lov.""Code""='SERVICE_STATUS_REJECT' or lov.""Code""='SERVICE_STATUS_CANCEL' then '#f10b0b'
end as StatusColor
            from public.""NtsService"" as s             
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""Code""='{templateCode}'             
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""           
            where s.""IsDeleted""=false #OWNERWHERE#
            group by ""Type"",s.""ServiceStatusId"",lov.""Code"",tem.""Code""   ";

            string ownerWhere = "";
            if (requestby.IsNotNullAndNotEmpty() && requestby == "RequestedByMe")
            {
                ownerWhere = $@" and (s.""RequestedByUserId""='{_repo.UserContext.UserId}' or s.""OwnerUserId""='{_repo.UserContext.UserId}') ";
            }
            query = query.Replace("#OWNERWHERE#", ownerWhere);

            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetServiceStatusByTemplateCode(string templateCode, string userId, string portalId)
        {
            var query = $@"Select count(s) as ""Value"",lov.""Name"" as ""Type"",s.""ServiceStatusId"" as ""Id"",tem.""Code"",
case when lov.""Code""='SERVICE_STATUS_DRAFT' then '#17a2b8'
when lov.""Code""='SERVICE_STATUS_COMPLETE' then '#13b713'
when lov.""Code""='SERVICE_STATUS_INPROGRESS' then '#007bff'
when lov.""Code""='SERVICE_STATUS_OVERDUE' then '#ffc107'
when lov.""Code""='SERVICE_STATUS_REJECT' or lov.""Code""='SERVICE_STATUS_CANCEL' then '#f10b0b'
end as StatusColor
            from public.""NtsService"" as s             
            Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""Code""='{templateCode}'             
            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""           
            where s.""IsDeleted""=false and (s.""RequestedByUserId""='{userId}' or s.""OwnerUserId""='{userId}') and s.""PortalId""='{portalId}'
            group by ""Type"",s.""ServiceStatusId"",lov.""Code"",tem.""Code""   ";

            var data = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return data;
        }

        public async Task<IList<NtsServiceIndexPageViewModel>> GetServiceCountByDifferentCodes(string categoryCodes, string templateCodes, string portalNames, string moduleCodes)
        {
            var query = "";
            if (_repo.UserContext.IsSystemAdmin)
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
							count(case when l.""Code"" = 'SERVICE_STATUS_INPROGRESS' or l.""Code"" = 'SERVICE_STATUS_OVERDUE' or l.""Code"" = 'SERVICE_STATUS_DRAFT' then 1 end) as CreatedByMeInProgreessOverDueCount,
							count(case when l.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CreatedByMeCompleteCount,
							count(case when l.""Code"" = 'SERVICE_STATUS_REJECT' or l.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as CreatedOrRequestedByMeRejectCancelCloseCount,
							count(s.""Id"") as TotalCount

							from public.""NtsService"" as s
							join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
							<<portal>>
							join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
							<<module>>
							join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
							where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
							#CATEGORYCODES# #TEMPLATECODES# 
							group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }
            else
            {
                query = $@"select stc.""Name"" as DisplayName,stc.""Code"" as TemplateCode,stc.""IconFileId"",stc.""SequenceOrder"",
							count(case when l.""Code"" = 'SERVICE_STATUS_INPROGRESS' or l.""Code"" = 'SERVICE_STATUS_OVERDUE' or l.""Code"" = 'SERVICE_STATUS_DRAFT' then 1 end) as CreatedByMeInProgreessOverDueCount,
							count(case when l.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CreatedByMeCompleteCount,
							count(case when l.""Code"" = 'SERVICE_STATUS_REJECT' or l.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as CreatedOrRequestedByMeRejectCancelCloseCount,
							count(s.""Id"") as TotalCount

							from public.""NtsService"" as s
							join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false and tem.""CompanyId""='{_repo.UserContext.CompanyId}'
							<<portal>>
							join public.""TemplateCategory"" as stc on tem.""TemplateCategoryId""=stc.""Id"" and stc.""IsDeleted""=false and stc.""CompanyId""='{_repo.UserContext.CompanyId}'
							<<module>>
							join public.""LOV"" as l on l.""Id""=s.""ServiceStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
							where (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId""='{_repo.UserContext.UserId}') and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
							#CATEGORYCODES# #TEMPLATECODES# 
							group by stc.""Name"", stc.""Code"", stc.""IconFileId"", stc.""SequenceOrder"" order by stc.""SequenceOrder"",stc.""Name"" ";
            }


            var categoryCodesWhere = categoryCodes.IsNotNullAndNotEmpty() ? $@" and stc.""Code"" in ('{categoryCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#CATEGORYCODES#", categoryCodesWhere);

            var templateCodesWhere = templateCodes.IsNotNullAndNotEmpty() ? $@" and tem.""Code"" in ('{templateCodes.Replace(",", "','")}') " : "";

            query = query.Replace("#TEMPLATECODES#", templateCodesWhere);


            var portal = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalNames = $"'{portalNames.Replace(",", "','")}'";
                //portal = @$" join public.""Portal"" as port on port.""Name"" in ({portalNames}) and port.""IsDeleted""=false and  port.""Id""=ANY(stc.""AllowedPortalIds"")";
                portal = @$" join public.""Portal"" as port on port.""Name"" in ({portalNames}) and port.""IsDeleted""=false and  port.""Id""= tem.""PortalId""  ";
            }

            var module = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                moduleCodes = $"'{moduleCodes.Replace(",", "','")}'";
                portal = @$" join public.""Module"" as m on m.""Code"" in ({moduleCodes}) and m.""IsDeleted""=false and  m.""Id""=stc.""ModuleId"" ";
            }

            query = query.Replace("<<portal>>", portal).Replace("<<module>>", module);

            var result = await _queryRepo.ExecuteQueryList<NtsServiceIndexPageViewModel>(query, null);

            return result;
        }

        public async Task<List<ServiceViewModel>> GetAllServicesList(string portalId, string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string statusCodes = null, string parentServiceId = null)
        {
            //string query = @$"Select s.""Id"",s.""ServiceNo"",s.""ServiceSubject"",s.""TemplateCode"",o.""Name"" as OwnerDisplayName,ts.""Name"" as ServiceStatusName,ts.""Code"" as ServiceStatusCode,
            //                s.""StartDate"",s.""DueDate"",s.""RequestedByUserId"", tem.""DisplayName"" as TemplateDisplayName,s.""UdfNoteTableId"" as udfNoteTableId,
            //                s.""CreatedDate"",s.""CompletedDate"" as CompletedDate, s.""RejectedDate"" as RejectedDate, s.""CanceledDate"" as CanceledDate, s.""ClosedDate"" as ClosedDate,s.""LastUpdatedDate"",stemp.""IconFileId""
            //                From public.""NtsService"" as s
            //                Join public.""Template"" as tem on s.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  #TemCodeWhere#
            //                Join public.""ServiceTemplate"" as stemp on stemp.""TemplateId"" = tem.""Id"" and stemp.""IsDeleted"" = false
            //                Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
            //                Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  #ModCodeWhere#
            //                left Join public.""User"" as o on s.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
            //                Join public.""LOV"" as ts on s.""ServiceStatusId""=ts.""Id"" and ts.""IsDeleted""=false #StatusWhere#
            //                where s.""PortalId"" in ('{portalId.Replace(",", "','")}') and s.""IsDeleted""=false #OWNERWHERE# #ParentWhere# order by s.""CreatedDate"" desc";

            string query = $@"select t.""DisplayName"" as TemplateDisplayName,stemp.""IconFileId"",
count(case when ss.""Code"" = 'SERVICE_STATUS_INPROGRESS' or ss.""Code"" = 'SERVICE_STATUS_OVERDUE' or ss.""Code"" = 'SERVICE_STATUS_DRAFT' then 1 end) as InprogressCount,
count(case when ss.""Code"" = 'SERVICE_STATUS_COMPLETE' then 1 end) as CompletedCount,
count(case when ss.""Code"" = 'SERVICE_STATUS_REJECT' or ss.""Code"" = 'SERVICE_STATUS_CANCEL' then 1 end) as RejectedCount

from public.""Template"" as t
Join public.""ServiceTemplate"" as stemp on stemp.""TemplateId"" = t.""Id"" and stemp.""IsDeleted"" = false
join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false #CatCodeWhere#
Left Join public.""Module"" as m on t.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  #ModCodeWhere#
left join public.""NtsService"" as s on t.""Id""=s.""TemplateId"" and s.""IsDeleted""=false #OWNERWHERE# #ParentWhere#
left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
where t.""IsDeleted""=false and '{portalId}'=any(tc.""AllowedPortalIds"")
group by t.""DisplayName"",stemp.""IconFileId"" ";


            string ownerWhere = "";
            if (!showAllOwnersService || (requestby.IsNotNullAndNotEmpty() && requestby == "RequestedByMe"))
            {
                ownerWhere = $@" and (s.""RequestedByUserId""='{_repo.UserContext.UserId}' or s.""OwnerUserId""='{_repo.UserContext.UserId}') ";
            }
            query = query.Replace("#OWNERWHERE#", ownerWhere);

            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and t.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            string statusCodeWhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                var codes = statusCodes.Trim(',').Replace(",", "','");
                statusCodeWhere = $@" and ss.""Code"" in ('{codes}')";
            }
            query = query.Replace("#StatusWhere#", statusCodeWhere);

            string parentWhere = "";
            if (parentServiceId.IsNotNullAndNotEmpty())
            {
                parentWhere = $@" and s.""ParentServiceId""='{parentServiceId}' ";
            }
            query = query.Replace("#ParentWhere#", parentWhere);

            var result = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return result;
        }

        public async Task<IList<TaskViewModel>> GetTaskListDataByWithNoteReferenceId(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null, string serId = null, string serTempCodes = null)
        {

            string query = @$"Select w.""WorkBoardName"" as WorkBoardName, t.""Id"",t.""TaskNo"",t.""TaskSubject"",t.""TemplateCode"",o.""Name"" as OwnerUserName,a.""Name"" as AssigneeDisplayName,tl.""Name"" as TaskStatusName,tl.""Code"" as TaskStatusCode,
                            t.""StartDate"",t.""DueDate"",t.""RequestedByUserId"",t.""AssignedToUserId"",t.""ActualStartDate"",t.""CreatedDate""
                            ,coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",tl.""GroupCode"" as ""StatusGroupCode"",
                            ntem.""DisplayName"" as TemplateName,dn.""NoteSubject"" as NoteSubject
                            From public.""NtsTask"" as t
                            Join public.""Template"" as tem on t.""TemplateId""=tem.""Id"" and tem.""IsDeleted""=false  #TemCodeWhere#
                            Join public.""TemplateCategory"" as tc on tem.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false  #CatCodeWhere#
                            Join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false  
                            Join public.""NtsNote"" as dn on s.""ReferenceId""=dn.""Id"" and dn.""IsDeleted""=false 
                            Join public.""Template"" as ntem on dn.""TemplateId""=ntem.""Id"" and ntem.""IsDeleted""=false
                            Left Join public.""Module"" as m on tem.""ModuleId""=m.""Id"" and m.""IsDeleted""=false  #ModCodeWhere#
                            Join public.""User"" as o on t.""OwnerUserId""=o.""Id"" and o.""IsDeleted""=false 
                            Join public.""User"" as a on t.""AssignedToUserId""=a.""Id"" and a.""IsDeleted""=false 
                            Join public.""LOV"" as tl on t.""TaskStatusId""=tl.""Id"" and tl.""IsDeleted""=false
                            left Join public.""NtsNote"" as n on n.""Id""=t.""ParentNoteId"" and n.""IsDeleted""=false      
                            left join cms.""N_WORKBOARD_WorkBoardItem"" as wbi on wbi.""NtsNoteId"" = n.""Id""
                            left join cms.""N_WORKBOARD_WorkBoard"" as w on w.""Id"" = wbi.""WorkBoardId""
where t.""PortalId"" in ('{portalId.Replace(",", "','")}') and t.""IsDeleted""=false #UserWhere# #Where# #StatusWhere# order by t.""CreatedDate"" desc ";

           
            string modCodeWhere = "";
            if (moduleCodes.IsNotNullAndNotEmpty())
            {
                var modIds = moduleCodes.Trim(',').Replace(",", "','");
                modCodeWhere = $@" and m.""Code"" in ('{modIds}')";
            }
            query = query.Replace("#ModCodeWhere#", modCodeWhere);

            string temCodeWhere = "";
            if (templateCodes.IsNotNullAndNotEmpty())
            {
                var temIds = templateCodes.Trim(',').Replace(",", "','");
                temCodeWhere = $@" and tem.""Code"" in ('{temIds}')";
            }
            query = query.Replace("#TemCodeWhere#", temCodeWhere);

            string catCodeWhere = "";
            if (categoryCodes.IsNotNullAndNotEmpty())
            {
                var catIds = categoryCodes.Trim(',').Replace(",", "','");
                catCodeWhere = $@" and tc.""Code"" in ('{catIds}')";
            }
            query = query.Replace("#CatCodeWhere#", catCodeWhere);

            string statusCodeWhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                var status = statusCodes.Trim(',').Replace(",", "','");
                statusCodeWhere = $@" and tl.""Code"" in ('{status}')";
            }
            query = query.Replace("#StatusWhere#", statusCodeWhere);

            string parentWhere = "";
            if (parentNoteId.IsNotNullAndNotEmpty())
            {
                parentWhere = $@" and t.""ParentNoteId""='{parentNoteId}' ";
            }
            query = query.Replace("#Where#", parentWhere);

            string userWhere = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                userWhere = $@" and t.""AssignedToUserId""='{userId}' ";
            }
            query = query.Replace("#UserWhere#", userWhere);

            var sertempcodeWhere = serTempCodes.IsNotNullAndNotEmpty() ? $@" and st.""Code"" in ('{serTempCodes.Trim(',').Replace(",", "','")}') " : "";

            query = query.Replace("#SerTemCodeWhere#", sertempcodeWhere);
            //string serIdWhere = "";
            //if (serId.IsNotNullAndNotEmpty())
            //{
            //    serIdWhere = $@" and s.""Id""='{serId}' ";
            //}
            //query = query.Replace("#ServiceIdWhere#", serIdWhere);

            var result = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return result;
        }


    }
}
