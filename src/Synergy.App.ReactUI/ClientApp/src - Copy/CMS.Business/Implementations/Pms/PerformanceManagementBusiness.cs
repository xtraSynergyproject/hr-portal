using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Hangfire;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Business
{
    public class PerformanceManagementBusiness : BusinessBase<ServiceViewModel, NtsService>, IPerformanceManagementBusiness
    {
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<StageViewModel> _queryStageRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<ProgramDashboardViewModel> _queryPDRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryGantt;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryDCRepo;
        private readonly IRepositoryQueryBase<PerformanceDashboardViewModel> _queryProjDashRepo;
        private readonly IRepositoryQueryBase<ProjectDashboardChartViewModel> _queryProjDashChartRepo;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryTaskRepo;
        private readonly IRepositoryQueryBase<MailViewModel> _queryMailTaskRepo;
        private readonly IRepositoryQueryBase<PerformanceDocumentViewModel> _queryPerDoc;
        private readonly IRepositoryQueryBase<PerformanceDocumentStageViewModel> _queryPerDocStage;
        private readonly IRepositoryQueryBase<GoalViewModel> _queryGoal;
        private readonly IRepositoryQueryBase<NoteTemplateViewModel> _queryNoteTemplate;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserContext _userContext;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IStepTaskComponentBusiness _stepCompBusiness;
        private readonly IRepositoryQueryBase<PerformaceRatingViewModel> _queryPerformanceRatingRepo;
        private readonly IRepositoryQueryBase<PerformanceRatingItemViewModel> _queryPerformanceRatingitemRepo;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IRepositoryQueryBase<CompetencyViewModel> _queryComp;
        private readonly IUserHierarchyBusiness _userHierBusiness;

        private readonly IRepositoryQueryBase<CompetencyCategoryViewModel> _queryCompeencyCategory;
        public PerformanceManagementBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<ServiceViewModel> queryRepo,
            IRepositoryQueryBase<ProgramDashboardViewModel> queryPDRepo,
            IRepositoryQueryBase<StageViewModel> queryStageRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<DashboardCalendarViewModel> queryDCRepo,
            IRepositoryQueryBase<ProjectGanttTaskViewModel> queryGantt,
             IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo,
             IRepositoryQueryBase<PerformanceDashboardViewModel> queryProjDashRepo,
             IRepositoryQueryBase<ProjectDashboardChartViewModel> queryProjDashChartRepo,
             IRepositoryQueryBase<TaskViewModel> queryTaskRepo, INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness, ITableMetadataBusiness tableMetadataBusiness,
            IMapper autoMapper, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, IServiceBusiness serviceBusiness, IRepositoryQueryBase<MailViewModel> queryMailTaskRepo,
            IRepositoryQueryBase<PerformanceDocumentViewModel> queryPerDoc, IRepositoryQueryBase<GoalViewModel> queryGoal, IRepositoryQueryBase<PerformanceDocumentStageViewModel> queryPerDocStage
            , IHRCoreBusiness hrCoreBusiness, IUserContext userContext, IRepositoryQueryBase<NoteTemplateViewModel> queryNoteTemplate, IComponentResultBusiness componentResultBusiness, ILOVBusiness lovBusiness
            , ITemplateBusiness templateBusiness, IStepTaskComponentBusiness stepCompBusiness, IRepositoryQueryBase<PerformaceRatingViewModel> queryPerformaceRating,
            IRepositoryQueryBase<PerformanceRatingItemViewModel> queryPerformaceRatingitem, IRepositoryQueryBase<CompetencyViewModel> queryComp, ICmsBusiness cmsBusiness, IRepositoryQueryBase<CompetencyCategoryViewModel> queryComptetencyCategory
            , IUserHierarchyBusiness userHierBusiness) : base(repo, autoMapper)
        {
            _queryStageRepo = queryStageRepo;
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _queryPDRepo = queryPDRepo;
            _queryDCRepo = queryDCRepo;
            _queryGantt = queryGantt;
            _queryTWRepo = queryTWRepo;
            _queryProjDashRepo = queryProjDashRepo;
            _queryProjDashChartRepo = queryProjDashChartRepo;
            _queryTaskRepo = queryTaskRepo;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _queryMailTaskRepo = queryMailTaskRepo;
            _queryPerDoc = queryPerDoc;
            _queryPerDocStage = queryPerDocStage;
            _hrCoreBusiness = hrCoreBusiness;
            _componentResultBusiness = componentResultBusiness;
            _queryGoal = queryGoal;
            _userContext = userContext;
            _queryNoteTemplate = queryNoteTemplate;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _stepCompBusiness = stepCompBusiness;
            _queryComp = queryComp;
            _queryPerformanceRatingRepo = queryPerformaceRating;
            _queryPerformanceRatingitemRepo = queryPerformaceRatingitem;
            _cmsBusiness = cmsBusiness;
            _queryCompeencyCategory = queryComptetencyCategory;
            _userHierBusiness = userHierBusiness;
        }
        public async override Task<CommandResult<ServiceViewModel>> Create(ServiceViewModel model)
        {

            return CommandResult<ServiceViewModel>.Instance();
        }
        public async override Task<CommandResult<ServiceViewModel>> Edit(ServiceViewModel model)
        {
            return CommandResult<ServiceViewModel>.Instance();
        }
        private async Task<CommandResult<ServiceViewModel>> IsNameExists(ServiceViewModel model)
        {
            return CommandResult<ServiceViewModel>.Instance();
        }

        //        public async Task<IList<ProgramDashboardViewModel>> GetProjectDataOld()
        //        {

        //            string query = @$"select ss.""ProjectCategory"" as TemplateName,ss.""Id"" as SuperServiceId,
        //nts.""Id"" as Id,nts.""ServiceSubject"" as ProjectName,nts.""StartDate"" as StartDate,
        //nts.""DueDate"" as DueDate,nts.""CreatedDate"" as CreatedOn,
        //nts.""LastUpdatedDate"" as UpdatedOn,u.""Name"" as ""CreatedBy"",
        //lov.""Name"" as ProjectStatus,lovp.""Name"" as Priority
        //from cms.""S_ProjectService_SuperService"" as ss
        //left join public.""NtsService"" as nts on nts.""Id""=ss.""NtsServiceId""
        //left join public.""User"" as u on u.""Id""=nts.""CreatedBy""
        //left join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId""
        //left join public.""LOV"" as lovp on lovp.""Id""=nts.""ServicePriorityId""
        //where nts.""OwnerUserId""='{_repo.UserContext.UserId}' ";
        //            var queryData = await _queryPDRepo.ExecuteQueryList(query, null);
        //            foreach (var item in queryData)
        //            {
        //                var query1 = $@"select t.""Count"" as AllTaskCount,t.""InProgreessCount"" as InProgreessCount,t.""DraftCount"" as DraftCount,
        //                      t.""CompletedCount"" as CompletedCount,t.""OverDueCount"" as OverDueCount,(t.""CompletedCount""/t.""Count"")*100 as Percentage,
        //                s.""Id"" as id,
        //                true as hasChildren
        //                FROM public.""NtsService"" as s
        //                left join(
        //	            WITH RECURSIVE NtsService AS ( 
        //                    SELECT t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"",lov.""Code"" as code 
        //                        FROM public.""NtsService"" as s                      
        //						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId""

        //                        where s.""ParentServiceId""='{item.Id}' 
        //						union all
        //                        SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"",lov.""Code"" as code 
        //                        FROM public.""NtsService"" as s
        //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	                             left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId""
        //	                   -- where s.""OwnerUserId""='{_repo.UserContext.UserId}'                        
        //                    )
        //                    SELECT count(ts) as ""Count"",
        //	sum(case when ""code""='TASK_STATUS_INPROGRESS' and ""ServiceId"" is not null then 1 else 0 end) as ""InProgreessCount"",

        //                    sum(case when ""code"" = 'TASK_STATUS_DRAFT' and ""ServiceId"" is not null then 1 else 0 end) as ""DraftCount"",
        //					sum(case when ""code"" = 'TASK_STATUS_COMPLETED' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
        //					sum(case when ""code"" = 'TASK_STATUS_OVERDUE' and ""ServiceId"" is not null then 1 else 0 end) as ""OverDueCount"",

        //""ServiceId"" from NtsService group by ""ServiceId"",code

        //                ) t on s.""Id""=t.""ServiceId""
        //                where s.""ParentServiceId""='{item.Id}'
        //                order by s.""SequenceOrder"" asc";
        //                var queryData1 = await _queryPDRepo.ExecuteQuerySingle(query1, null);
        //                if (queryData1 != null)
        //                {
        //                    item.AllTaskCount = queryData1.AllTaskCount;
        //                    item.InProgreessCount = queryData1.InProgreessCount;
        //                    item.DraftCount = queryData1.DraftCount;
        //                    item.CompletedCount = queryData1.CompletedCount;
        //                    item.OverDueCount = queryData1.OverDueCount;
        //                    item.Percentage = queryData1.Percentage;
        //                }

        //                var query2 = $@"select count(distinct t.""Count"")
        //                FROM public.""NtsService"" as s
        //                left join(
        //	            WITH RECURSIVE NtsService AS ( 
        //                    SELECT t.""AssignedToUserId"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
        //                        FROM public.""NtsService"" as s                      
        //						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //						where s.""ParentServiceId""='{item.Id}' 
        //						union all
        //                        SELECT t.""AssignedToUserId"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
        //                        FROM public.""NtsService"" as s
        //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""

        //	                   -- where s.""OwnerUserId""='{_repo.UserContext.UserId}'                        
        //                    )
        //                    SELECT ts as ""Count"",""ServiceId"" from NtsService 

        //                ) t on s.""Id""=t.""ServiceId""
        //                where s.""ParentServiceId""='{item.Id}'";
        //                var queryData2 = await _queryPDRepo.ExecuteScalar<long?>(query2, null);
        //                if (queryData2 != null && queryData2.Value.IsNotNull())
        //                {
        //                    item.UserCount = queryData2.Value;
        //                }
        //            }
        //            return queryData;
        //        }


        //            var query1 = $@"select count(distinct cnt.""AssignedToUserId"") as UserCount
        //                from public.""NtsService"" as nts
        //left join cms.""S_ProjectService_SuperService"" as ss on nts.""Id""=ss.""NtsServiceId""
        //left join (
        //  WITH RECURSIVE NtsService AS (
        //                             SELECT t.""TaskSubject"" as ts, t.""Id"" as taskid, s.""Id"", t.""AssignedToUserId"", s.""ParentServiceId"", t.""ParentServiceId"" as tps
        //                             FROM public.""NtsService"" as s
        //                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 

        //                             left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //						     where s.""OwnerUserId""='{_repo.UserContext.UserId}'
        //						     union all
        //                             SELECT t.""TaskSubject"" as ts, t.""Id"" as taskid, s.""Id"", t.""AssignedToUserId"", s.""ParentServiceId"", t.""ParentServiceId"" as tps
        //                             FROM public.""NtsService"" as s
        //                             inner join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //							 join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""

        //	                        where s.""OwnerUserId""='{_repo.UserContext.UserId}'
        //                        )
        //                        SELECT * from NtsService

        //) as cnt on cnt.""Id""=nts.""Id"" 
        //where nts.""OwnerUserId""= '{_repo.UserContext.UserId}' ";
        //            var queryData1 = await _queryPDRepo.ExecuteQueryList(query1, null);
        //            //var queryData3 = await _queryPDRepo.ExecuteQueryList(query1, null);


        //            var query2 = $@"select count(distinct cnt.""taskid"") as AllTaskCount
        //                from public.""NtsService"" as nts
        //left join cms.""S_ProjectService_SuperService"" as ss on nts.""Id""=ss.""NtsServiceId""
        //left join (
        //  WITH RECURSIVE NtsService AS (
        //                             SELECT t.""TaskSubject"" as ts, t.""Id"" as taskid, s.""Id"", t.""AssignedToUserId"", s.""ParentServiceId"", t.""ParentServiceId"" as tps
        //                             FROM public.""NtsService"" as s
        //                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 

        //                             left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //						     where s.""OwnerUserId""='{_repo.UserContext.UserId}'
        //						     union all
        //                             SELECT t.""TaskSubject"" as ts, t.""Id"" as taskid, s.""Id"", t.""AssignedToUserId"", s.""ParentServiceId"", t.""ParentServiceId"" as tps
        //                             FROM public.""NtsService"" as s
        //                             inner join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //							 join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""

        //	                        where s.""OwnerUserId""='{_repo.UserContext.UserId}'
        //                        )
        //                        SELECT * from NtsService

        //) as cnt on cnt.""Id""=nts.""Id"" 
        //where nts.""OwnerUserId""= '{_repo.UserContext.UserId}' ";
        //            var queryData2 = await _queryPDRepo.ExecuteQueryList(query2, null);



        //        public async Task<IList<ProgramDashboardViewModel>> GetPerformanceData()
        //        {

        //            string query = @$"select distinct s.""Id"" as Id,ucount.""Count"" as UserCount,
        //                pdudf.""Name"" as Name,t.""Count"" as AllTaskCount,(t.""CompletedCount"" / t.""Count"")*100 as Percentage,
        //                false as hasChildren,t.""InProgreessCount"" as InProgreessCount,t.""DraftCount"" as DraftCount,t.""CompletedCount"" as CompletedCount,t.""OverDueCount"" as OverDueCount
        //                ,pdms.""Name"" as MasterStageName,pdms.""Id"" as MasterStageId,
        //                --'PROJECT_SUPER_SERVICE' as ParentId,'STAGE' as Type

        //                pdudf.""Name"" as ProjectName,s.""StartDate"" as StartDate,
        //				s.""DueDate"" as DueDate,s.""CreatedDate"" as CreatedOn,
        //s.""LastUpdatedDate"" as UpdatedOn,u.""Name"" as CreatedBy,
        //lov.""Name"" as ProjectStatus,lovp.""Name"" as Priority,lov.""Code"" as ProjectStatusCode,pdmudf.""DocumentStatus"" as DocumentStatus
        //                FROM public.""NtsService"" as s
        //                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""           and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
        //				left join public.""User"" as u on u.""Id""=s.""CreatedBy""              and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
        //				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""     and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
        //				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId"" and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_userContext.CompanyId}' 
        //				left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pdudf on pdudf.""NtsNoteId""=s.""UdfNoteId"" and pdudf.""IsDeleted""=false and pdudf.""CompanyId""='{_userContext.CompanyId}'
        //                left join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdmudf on pdmudf.""Id"" = pdudf.""DocumentMasterId"" and pdmudf.""IsDeleted"" = false and pdmudf.""CompanyId"" ='{_userContext.CompanyId}'

        //                join  public.""NtsNote"" n on n.""ParentNoteId""=pdmudf.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72' 
        //                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on n.""Id"" = pdms.""NtsNoteId"" and pdms.""IsDeleted"" = false and pdms.""CompanyId"" = '5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
        //              --left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""DocumentMasterStageId"" = pdms.""Id"" and pds.""IsDeleted"" = false and pds.""CompanyId"" = '5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'

        //                 left join(
        //                WITH RECURSIVE Nts AS(

        //                 WITH RECURSIVE NtsService AS(
        //                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid""

        //                     FROM public.""NtsService"" as s
        //                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
        //						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
        //						     where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
        //                            and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 

        //                        union all
        //                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid"" 

        //                     FROM public.""NtsService"" as s
        //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

        //                          left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
        //                    -- join public.""User"" as u on s.""OwnerUserId""=u.""Id""
        //                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
        //                 )
        //                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code""  from NtsService


        //                 union all

        //                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code""--, t.""AssignedToUserId"" as ""auid""
        //                     FROM  public.""NtsTask"" as t
        //                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
        //	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
        //					-- left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" 
        //                   where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'  )


        //                    SELECT count(""Id"") as ""Count"",
        //					  sum(case when ""Code""='TASK_STATUS_INPROGRESS' and ""ServiceId"" is not null then 1 else 0 end) as ""InProgreessCount"",
        //					  sum(case when ""Code"" = 'TASK_STATUS_DRAFT' and ""ServiceId"" is not null then 1 else 0 end) as ""DraftCount"",
        //					sum(case when ""Code"" = 'TASK_STATUS_COMPLETE' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
        //					sum(case when ""Code"" = 'TASK_STATUS_OVERDUE' and ""ServiceId"" is not null then 1 else 0 end) as ""OverDueCount"",

        //					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""--,""auid""

        //                ) t on s.""Id""=t.""ServiceId""
        //       left join(
        //                WITH RECURSIVE Nts AS(

        //                 WITH RECURSIVE NtsService AS(
        //                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
        //                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""         and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
        //						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""   and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
        //						     where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
        //                            and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 

        //                        union all
        //                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
        //                          join NtsService ns on s.""ParentServiceId""=ns.""Id""

        //                            left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""   and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'    
        //                     join public.""User"" as u on s.""OwnerUserId""=u.""Id""                      and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
        //                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
        //                 )
        //                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code"",""auid""  from NtsService


        //                 union all

        //                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code"", t.""AssignedToUserId"" as ""auid""
        //                     FROM  public.""NtsTask"" as t
        //                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
        //	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'  
        //					 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""   and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
        //                 where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'    )


        //                    SELECT count(distinct ""auid"") as ""Count"",


        //					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""

        //                ) ucount on s.""Id""=ucount.""ServiceId""
        //                Where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  and s.""OwnerUserId"" = '{_repo.UserContext.UserId}' --and t.""Count""!=0
        //             ORDER BY s.""LastUpdatedDate"" desc    --  GROUP BY s.""Id"", s.""ServiceSubject"", t.""Count""     ";
        //            var queryData = await _queryPDRepo.ExecuteQueryList(query, null);


        //            return queryData;
        //        }

        public async Task<IList<ProgramDashboardViewModel>> GetPerformanceData(string userId)
        {

            string query = @$"select distinct s.""Id"" as Id,
                pdudf.""Name"" as Name,
                pdms.""Name"" as MasterStageName,pdms.""Id"" as MasterStageId,
                pdudf.""Name"" as ProjectName,s.""StartDate"" as StartDate,
				s.""DueDate"" as DueDate,s.""CreatedDate"" as CreatedOn,
                s.""LastUpdatedDate"" as UpdatedOn,u.""Name"" as CreatedBy,
                lov.""Name"" as ProjectStatus,lovp.""Name"" as Priority,lov.""Code"" as ProjectStatusCode,pdmudf.""DocumentStatus"" as DocumentStatus
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""           and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
				left join public.""User"" as u on u.""Id""=s.""CreatedBy""              and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""     and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId"" and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_userContext.CompanyId}' 
				left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pdudf on pdudf.""NtsNoteId""=s.""UdfNoteId"" and pdudf.""IsDeleted""=false and pdudf.""CompanyId""='{_userContext.CompanyId}'
                left join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdmudf on pdmudf.""Id"" = pdudf.""DocumentMasterId"" and pdmudf.""IsDeleted"" = false and pdmudf.""CompanyId"" ='{_userContext.CompanyId}'
                join  public.""NtsNote"" n on n.""ParentNoteId""=pdmudf.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}'
                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on n.""Id"" = pdms.""NtsNoteId"" and pdms.""IsDeleted"" = false and pdms.""CompanyId"" = '{_userContext.CompanyId}'
                where s.""OwnerUserId"" = '{userId}'";


            var queryData = await _queryPDRepo.ExecuteQueryList(query, null);


            foreach (var st in queryData)
            {
                var stages = await GetPerformanceStageData(userId, st.Id);
                st.StageList = new List<StageViewModel>();
                st.StageList.AddRange(stages);
            }

            return queryData;
        }

        public async Task<IList<StageViewModel>> GetPerformanceStageData(string userId, string performanceDocumentId)
        {

            string query = $@"select distinct s.""ServiceSubject"" as StageName, s.""Id"" as StageId, s.""ParentServiceId"" as PerformanceDocumentId,
                            lov.""Name"" as Status, s.""StartDate"" as StartDate, s.""DueDate"" as EndDate
                            from public.""NtsService"" as s 
                            left join public.""LOV"" as lov on lov.""Id"" =  s.""ServiceStatusId""
                            where s.""TemplateCode"" = 'PMS_PERFORMANCE_DOCUMENT_STAGE' and s.""ParentServiceId"" = '{performanceDocumentId}'
                            and s.""IsDeleted""  = false and s.""OwnerUserId"" = '{userId}'";


            var queryData = await _queryStageRepo.ExecuteQueryList(query, null);
            foreach (var st in queryData)
            {
                var goalsComptency = await GetGoalandCompetencyCountByPerformanceAndStageId(performanceDocumentId, st.StageId, userId);
                if (goalsComptency.IsNotNull())
                {
                    st.Goals = goalsComptency.Where(x => x.TemplateCode == "PMS_GOAL").ToList().Count;
                    st.Competency = goalsComptency.Where(x => x.TemplateCode == "PMS_COMPENTENCY").ToList().Count;
                }
            }

            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> GetGoalandCompetencyCountByPerformanceAndStageId(string performanceId, string stageId, string userId)
        {
            var query = $@"
select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""GoalName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            ,g.""GoalStartDate"" as ""StartDate"",g.""GoalEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,g.""ManagerComments"" as ManagerComments
            ,g.""EmployeeRating"",g.""EmployeeComments"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pdsns.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'

union

select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""CompetencyName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            ,g.""CompetencyStartDate"" as ""StartDate"",g.""CompetencyEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,g.""ManagerComment"" as ManagerComments
            ,g.""EmployeeRating"",g.""EmployeeComment"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pdsns.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'
";

            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }



        public async Task<IList<ProgramDashboardViewModel>> GetPerformanceSharedData()
        {

            string query = @$"select distinct s.""Id"" as Id,ucount.""Count"" as UserCount,
                pdudf.""Name"" as Name,t.""Count"" as AllTaskCount,(t.""CompletedCount"" / t.""Count"")*100 as Percentage,
                false as hasChildren,t.""InProgreessCount"" as InProgreessCount,t.""DraftCount"" as DraftCount,t.""CompletedCount"" as CompletedCount,t.""OverDueCount"" as OverDueCount
                ,pdms.""Name"" as MasterStageName,pdms.""Id"" as MasterStageId,
                --'PROJECT_SUPER_SERVICE' as ParentId,'STAGE' as Type

                pdudf.""Name"" as ProjectName,s.""StartDate"" as StartDate,
				s.""DueDate"" as DueDate,s.""CreatedDate"" as CreatedOn,
s.""LastUpdatedDate"" as UpdatedOn,u.""Name"" as CreatedBy,
lov.""Name"" as ProjectStatus,lovp.""Name"" as Priority,lov.""Code"" as ProjectStatusCode
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""            and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
				left join public.""User"" as u on u.""Id""=s.""CreatedBy""               and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""      and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId""  and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_userContext.CompanyId}' 
            left join public.""NtsServiceShared"" as nss on nss.""NtsServiceId""=s.""Id"" and nss.""IsDeleted""=false and nss.""CompanyId""='{_userContext.CompanyId}' 
			left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pdudf on pdudf.""NtsNoteId""=s.""UdfNoteId"" and pdudf.""IsDeleted""=false and pdudf.""CompanyId""='{_userContext.CompanyId}'
                left join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdmudf on pdmudf.""Id"" = pdudf.""DocumentMasterId"" and pdmudf.""IsDeleted"" = false and pdmudf.""CompanyId"" ='{_userContext.CompanyId}'

                join  public.""NtsNote"" n on n.""ParentNoteId""=pdmudf.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}' 
                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on n.""Id"" = pdms.""NtsNoteId"" and pdms.""IsDeleted"" = false and pdms.""CompanyId"" = '{_userContext.CompanyId}'
              --left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""DocumentMasterStageId"" = pdms.""Id"" and pds.""IsDeleted"" = false and pds.""CompanyId"" = '{_userContext.CompanyId}'
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid""

                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
						     where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  --and s.""OwnerUserId""='{_repo.UserContext.UserId}'


                        union all
                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid"" 

                     FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

                          left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""    and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'  
                    -- join public.""User"" as u on s.""OwnerUserId""=u.""Id""
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code""--, t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
					-- left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" 
                   where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'  )

	           		
                    SELECT count(""Id"") as ""Count"",
					  sum(case when ""Code""='TASK_STATUS_INPROGRESS' and ""ServiceId"" is not null then 1 else 0 end) as ""InProgreessCount"",
					  sum(case when ""Code"" = 'TASK_STATUS_DRAFT' and ""ServiceId"" is not null then 1 else 0 end) as ""DraftCount"",
					sum(case when ""Code"" = 'TASK_STATUS_COMPLETE' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
					sum(case when ""Code"" = 'TASK_STATUS_OVERDUE' and ""ServiceId"" is not null then 1 else 0 end) as ""OverDueCount"",
					 
					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""--,""auid""
                 
                ) t on s.""Id""=t.""ServiceId""
       left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
						     where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  --and s.""OwnerUserId""='{_repo.UserContext.UserId}'


                        union all
                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                          join NtsService ns on s.""ParentServiceId""=ns.""Id""

                            left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'    
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code"",""auid""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code"", t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'    
					 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                   where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'  )

	           		
                    SELECT count(distinct ""auid"") as ""Count"",
					  
					 
					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""
                 
                ) ucount on s.""Id""=ucount.""ServiceId""
                Where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  and nss.""SharedWithUserId"" = '{_repo.UserContext.UserId}' --and t.""Count""!=0
ORDER BY s.""LastUpdatedDate"" desc              
--  GROUP BY s.""Id"", s.""ServiceSubject"", t.""Count""  ";
            var queryData = await _queryPDRepo.ExecuteQueryList(query, null);

            return queryData;
        }


        public async Task<IList<ProjectGanttTaskViewModel>> ReadWBSTimelineGanttChartData(string userId, string performanceId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            //var search = @$" where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}'";
            //var Assignee = "";
            //if (userRole.Contains("PROJECT_USER"))
            //{
            //    search = @$" where tt.""Code""='PROJECT_SUPER_SERVICE'";
            //    Assignee = @$" where t.""AssignedToUserId""='{userId}' ";
            //}
            //if (projectId.IsNotNull())
            //{
            //    search = @$"  where s.""Id""='{projectId}' ";

            //}

            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",'Service' as ""Type"", s.""LockStatusId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""     and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					     join public.""User"" as u on s.""OwnerUserId""=u.""Id""           and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",'Stage' as ""Type"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id""                and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
              where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'   )
                 SELECT ""Id"",""ServiceSubject"" as Title,""Type"" ,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",true as Summary,""Priority"",""NtsStatus"" from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title,'Task' as ""Type"", t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",true as Summary, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                 where  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'     #OwnerWhere#  #AssigneeWhere# #StatusWhere# #DateWhere#      
                    )
                 SELECT * from Nts";

            //       if (projectId.IsNotNullAndNotEmpty())
            //       {

            //           query = @$"select t.""Id"",t.""ServiceSubject"" as Title,t.""StartDate"" as Start,t.""DueDate"" as End,""ParentId"",'' as ""UserName"",true as Summary,t.""Priority"",t.""NtsStatus"" FROM public.""NtsService"" as s 
            //                   join(
            //            WITH RECURSIVE NtsService AS(
            //            SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            //                   FROM public.""NtsService"" as s
            //                    join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
            //                       join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
            //                   where s.""Id""='{projectId}' 
            //	union all
            //                   SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            //                   FROM public.""NtsService"" as s
            //                   join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
            //                    join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
            //                       join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
            //            )
            //            SELECT ""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
            //where s.""Id""='{projectId}' 

            //union
            //            select t.""Id"",t.""TaskSubject"" as Title,t.""StartDate"" as Start,t.""DueDate"" as End,""ParentId"",t.""UserName"" as ""UserName"",true as Summary,t.""Priority"",t.""NtsStatus"" FROM public.""NtsService"" as s
            //            join(
            //           WITH RECURSIVE NtsService AS (
            //               SELECT t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"",u.""Name"" as ""UserName"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            //                   FROM public.""NtsService"" as s
            //                   join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                   left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
            //                   left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
            //                   left join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //	where s.""Id""='{projectId}' 
            //	union all
            //                   SELECT t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"",u.""Name"" as ""UserName"" ,'{projectId}' as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            //                   FROM public.""NtsService"" as s
            //                   join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
            //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                   join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
            //                   join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
            //                   join public.""User"" as u on t.""AssignedToUserId""=u.""Id""

            //               )
            //            SELECT ""Id"",""TaskSubject"",""StartDate"",""DueDate"",""ParentId"",""UserName"",tmp,""Priority"",""NtsStatus"" from NtsService where ""Id"" is not null ) t on t.tmp=s.""Id""
            //where s.""Id""='{projectId}'";
            //       }
            //query = query.Replace("#SEARCH#", search);
            //query = query.Replace("#ASSIGNEE#", Assignee);
            var projectSerach = "";

            if (performanceId.IsNotNull())
            {
                projectSerach = @$" and s.""Id""='{performanceId}' ";

            }
            else if (projectIds.IsNotNull() && projectIds.Count > 0)
            {
                string pids = null;
                foreach (var i in projectIds)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    projectSerach = @" and s.""Id"" in (" + pids + ") ";
                }
            }
            if (performanceId.IsNullOrEmpty() && projectIds.Count == 0)
            {
                projectSerach = @$" and tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}'";
            }
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {

                    ownerSerach = @" and t.""OwnerUserId"" in (" + oids + ") ";

                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var Assignee = "";
            if (userRole.Contains("PROJECT_USER"))
            {
                projectSerach = @$" and tt.""Code""='PMS_PERFORMANCE_DOCUMENT'";
                if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                {
                    Assignee = @$" and t.""AssignedToUserId""='{userId}' ";
                }
                else
                {
                    Assignee = @$" and t.""AssignedToUserId""='{userId}' ";
                }
            }
            else if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                    {
                        Assignee = @" and t.""AssignedToUserId"" in (" + aids + ") ";
                    }
                    else
                    {
                        Assignee = @" and t.""AssignedToUserId"" in (" + aids + ") ";
                    }

                }
            }
            query = query.Replace("#AssigneeWhere#", Assignee);
            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0 && Assignee.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0 && Assignee.IsNotNullAndNotEmpty()) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }
            query = query.Replace("#DateWhere#", dateSerach);
            query = query.Replace("#SEARCH#", projectSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
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
                query = $@"  
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""  and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
						     and usp.""UserRoleId"" in ({roleText}) where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
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
                       where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT count(""Id"") from Nts where Level='Child' ";
                var count = await _queryRepo.ExecuteScalar<long?>(query, null);

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
                query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                left join(

                 WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""UserRoleId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						     and usp.""UserRoleId"" in ({roleText}) where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
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
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT count(""Id"") as ""Count"",""UserRoleId"" from Nts where Level='Child' group by ""UserRoleId""

						
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

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

                query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
                , usp.""InboxStageName"" as id, '{id}' as ParentId, 'PROJECTSTAGE' as Type,
                true as hasChildren, '{userRoleId}' as UserRoleId,usp.""TemplateCode"" as StageName
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
              
                 left join(
                    WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""InboxStageName""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
						     where usp.""UserRoleId"" = '{userRoleId}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""InboxStageName"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                     where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        
                 )
                 SELECT ""Id"",'Parent' as Level,""InboxStageName""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""InboxStageName"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
						
                    SELECT count(""Id"") as ""Count"",""InboxStageName"" from Nts where Level='Child' group by ""InboxStageName""
					) nt on usp.""InboxStageName""=nt.""InboxStageName""
                where ur.""Id"" = '{userRoleId}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
                Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



                var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PROJECTSTAGE")
            {
                query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{id}' as ParentId,                
                'PROJECT' as Type,'{userRoleId}' as UserRoleId,
                true as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                left join(

                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""TemplateCode""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
						     and usp.""UserRoleId"" = '{userRoleId}' where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
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
                          where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )

	          
                    SELECT count(""Id"") as ""Count"",""TemplateCode"" from Nts where Level='Child' group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{userRoleId}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' and usp.""InboxStageName"" = '{id}'
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "PROJECT").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PROJECT")
            {
                query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,
                false as hasChildren,s.""Id"" as ProjectId,
                '{id}' as ParentId,'STAGE' as Type
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    
						     where tt.""Code""='{id}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
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
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )

	           
                    SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{id}' and s.""OwnerUserId"" = '{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' and t.""Count""!=0
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


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

            //    else if (type == "STAGE")
            //    {
            //        query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
            //        s.""Id"" as id,
            //        true as hasChildren
            //        FROM public.""NtsService"" as s
            //        left join(
            //        WITH RECURSIVE Nts AS(

            //         WITH RECURSIVE NtsService AS(
            //         SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s


            //where s.""ParentServiceId""='{id}' 

            //                union all
            //                SELECT s.""Id"", ns.""ServiceId"" FROM public.""NtsService"" as s
            //                join NtsService ns on s.""ParentServiceId""=ns.""Id""

            //             join public.""User"" as u on s.""OwnerUserId""=u.""Id""

            //         )
            //         SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService


            //         union all

            //         select t.""Id"",'Child' as Level,nt.""ServiceId"" 
            //             FROM  public.""NtsTask"" as t
            //                join Nts as nt on t.""ParentServiceId"" =nt.""Id""  

            //            )

            //            SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""

            //        ) t on s.""Id""=t.""ServiceId""
            //        where s.""ParentServiceId""='{id}'
            //        order by s.""SequenceOrder"" asc";

            //        list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

            //    }
            else
            {

                //query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                //'{parentId}' as StageId,s.""StatusCode"" as StatusCode,
                //false as hasChildren
                //FROM public.""UserRoleStageChild"" as s
                //--left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                //left join(
                //    select case when task.""TaskStatusCode""='OVERDUE' then 'INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                //    FROM public.""RecTask"" as s
                //    join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
                //    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                //    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""

                //    Where tmp.""TemplateCode""='{parentId}' and task.""AssigneeUserId"" = '{userId}'
                //    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 

                //    group by TaskStatusCode  
                //) t on t.TaskStatusCode=ANY(s.""StatusCode"")
                //where s.""InboxStageId""='{stageId}'
                //order by s.""SequenceOrder"" asc";

                //list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

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
        public async Task<IList<TreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string performanceId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    performanceId = obj.PerformanceId;
                    stageId = obj.StageId;
                }
            }

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
                query = $@"  
               
                
                 SELECT count(t) as count FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMS' and usp.""ChildSequence""!='1'
						    and usp.""UserRoleId"" in ({roleText}) and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'

					 
                       
                        
                 ";
                var count = await _queryRepo.ExecuteScalar<long?>(query, null);

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
                if (count != null)
                {
                    item.Name = item.DisplayName = $"Inbox ({count })";
                }
                list.Add(item);


                var item1 = new TreeViewViewModel
                {
                    id = "GENERAL",
                    Name = "General Task",
                    DisplayName = "General Task",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "GENERAL"
                };
                if (count != null)
                {
                    item1.Name = item1.DisplayName = $"General Task";
                }
                list.Add(item1);

            }
            else if (id == "GENERAL")
            {

                var rquery = @$"
                        

                 select count(t) as count
                     FROM  public.""NtsTask"" as t
                        
                        join public.""Template"" as tt on tt.""Id"" =t.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         and (md.""Code""='PerformanceDocument' or md.""TemplateCategoryType""=0)
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and t.""AssignedToUserId""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""NtsService"" as nt on t.""ParentServiceId"" =nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_userContext.CompanyId}'
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                  ";
                var rcount = await _queryRepo.ExecuteScalar<long?>(rquery, null);

                var squery = @$"
                        

                 select count(t) as count 
                     FROM  public.""NtsTask"" as t
                        
                        join public.""Template"" as tt on tt.""Id"" =t.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                        and md.""TemplateCategoryType""=0
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'                      
                        join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and t.""OwnerUserId""='{userId}' and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                        left join public.""NtsService"" as nt on t.""ParentServiceId"" =nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_userContext.CompanyId}'
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                  ";
                var scount = await _queryRepo.ExecuteScalar<long?>(squery, null);

                var item1 = new TreeViewViewModel
                {
                    id = "RECEIVED",
                    Name = "Received",
                    DisplayName = "Received",
                    ParentId = "GENERAL",
                    hasChildren = true,
                    expanded = true,
                    Type = "RECEIVED"
                };
                if (rcount != null)
                {
                    item1.Name = item1.DisplayName = $"Received ({rcount })";
                }

                list.Add(item1);

                var item2 = new TreeViewViewModel
                {
                    id = "SENT",
                    Name = "Sent",
                    DisplayName = "Sent",
                    ParentId = "GENERAL",
                    hasChildren = true,
                    expanded = true,
                    Type = "SENT"
                };
                if (scount != null)
                {
                    item2.Name = item2.DisplayName = $"Sent ({scount })";
                }

                list.Add(item2);
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
                query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || COALESCE(nt.""Count"",0)|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMS' 
               and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                left join(

                   SELECT count(t) as ""Count"",usp.""UserRoleId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" 
                          and usp.""InboxCode""='PMS' and usp.""ChildSequence""!='1' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'

                            and usp.""UserRoleId"" in ({roleText}) 
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' group by ""UserRoleId""

						
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

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

                query = $@"Select usp.""InboxStageName"" ||' (' || COALESCE(nt.""Count"",0)|| ')' as Name
                , usp.""TemplateCode"" as id, '{id}' as ParentId, 'PERFORMANCE' as Type,usp.""Id"" as StageId,
                true as hasChildren, '{userRoleId}' as UserRoleId,usp.""TemplateCode"" as StageName
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMS' 
and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' and usp.""ChildSequence""='1'
              
                 left join(
                     SELECT count(t) as ""Count"",'PMS_PERFORMANCE_DOCUMENT' as ""InboxStageName"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMS' and usp.""ChildSequence""!='1'
						    and usp.""UserRoleId"" = '{userRoleId}' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' group by ""InboxStageName""
						
					) nt on usp.""TemplateCode""=nt.""InboxStageName""
                where ur.""Id"" = '{userRoleId}' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                Group By nt.""Count"",usp.""InboxStageName"",usp.""Id"", usp.""StageSequence"",usp.""TemplateCode""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



                var obj = expObj.Where(x => x.Type == "PERFORMANCE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            // else if (type == "PERFORMANCETYPE")
            // {
            //     query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
            //     usp.""TemplateCode""  as id, '{id}' as ParentId,  '{stageId}' as StageId ,          
            //     'PERFORMANCE' as Type,'{userRoleId}' as UserRoleId,
            //     true as hasChildren,usp.""TemplateCode"" as StageName
            //     from public.""UserRole"" as ur
            //     join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false
            //     left join(

            //     WITH RECURSIVE Nts AS(

            //      WITH RECURSIVE NtsService AS(
            //      SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""TemplateCode""   FROM public.""NtsService"" as s
            //              join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false
            //and usp.""UserRoleId"" = '{userRoleId}' 

            //             union all
            //             SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""TemplateCode"" FROM public.""NtsService"" as s
            //             join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

            //          join public.""User"" as u on s.""OwnerUserId""=u.""Id""

            //      )
            //      SELECT ""Id"",'Parent' as Level,""TemplateCode""  from NtsService


            //      union all

            //      select t.""Id"",'Child' as Level,nt.""TemplateCode"" 
            //          FROM  public.""NtsTask"" as t
            //             join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
            //             join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //          where t.""AssignedToUserId"" = '{userId}'          
            //         )


            //         SELECT count(""Id"") as ""Count"",""TemplateCode"" from Nts where Level='Child' group by ""TemplateCode""

            //     ) t on usp.""TemplateCode""=t.""TemplateCode""

            //     where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}'
            //     Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
            //     order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
            //     list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


            //     var obj = expObj.Where(x => x.Type == "PERFORMANCE").FirstOrDefault();
            //     if (obj.IsNotNull())
            //     {
            //         var data = list.Where(x => x.id == obj.id).FirstOrDefault();
            //         if (data.IsNotNull())
            //         {
            //             data.expanded = true;
            //         }
            //     }

            // }
            else if (type == "PERFORMANCE")
            {
                query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0) || ')' as Name,
                true as hasChildren,s.""Id"" as PerformanceId, '{stageId}' as StageId , '{userRoleId}' as UserRoleId,     
                '{id}' as ParentId,'STAGETYPE' as Type,s.""TemplateCode"" as StageName
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                 left join(

                     SELECT count(t) as ""Count"",s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""Code""='{id}' and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        join public.""NtsService"" as g on g.""ParentServiceId""=s.""Id"" and g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}'  
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  group by ""ServiceId""
               
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{id}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "STAGETYPE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "STAGETYPE")
            {
                query = $@"Select usp.""InboxStageName"" ||' (' || COALESCE(nt.""Count"",0)|| ')' as Name
                , usp.""TemplateCode"" as id, '{id}' as ParentId, 'STAGE' as Type,usp.""Id"" as StageId,'{performanceId}' as PerformanceId,
                true as hasChildren, '{userRoleId}' as UserRoleId,usp.""TemplateCode"" as StageName
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMS'
                and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' and usp.""ChildSequence""!='1'
              
                 left join(
                    SELECT count(t) as ""Count"",usp.""InboxStageName"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMS' and usp.""ChildSequence""!='1'
						    and usp.""UserRoleId"" = '{userRoleId}' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where s.""OwnerUserId""='{userId}' and s.""ParentServiceId""='{performanceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' group by ""InboxStageName""
               
                 
					) nt on usp.""InboxStageName""=nt.""InboxStageName""
                where ur.""Id"" = '{userRoleId}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
                Group By nt.""Count"",usp.""InboxStageName"",usp.""Id"", usp.""StageSequence"",usp.""TemplateCode""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";



                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


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
            else if (type == "STAGE")
            {
                query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0) || ')' as Name,
                true as hasChildren, '{stageId}' as StageId , '{performanceId}' as PerformanceId,     
                '{id}' as ParentId,'STATUS' as Type,s.""TemplateCode"" as StageName
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                 left join(
                    SELECT count(t) as ""Count"",s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""Code""='{id}' and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where s.""OwnerUserId""='{userId}' and s.""ParentServiceId""= '{performanceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""
                where tt.""Code""='{id}' and s.""ParentServiceId""= '{performanceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                --Where t.""Count""!=0
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "STATUS").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "STATUS")
            {
                query = $@"select 'TASK' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                '{id}' as ParentId,s.""StatusCode"" as StatusCode,
                false as hasChildren
                FROM public.""UserRoleStageChild"" as s
                  left join(

                    SELECT case when ts.""Code""='TASK_STATUS_OVERDUE' then 'TASK_STATUS_INPROGRESS' else ts.""Code"" end as TaskStatusCode,count(t.""Id"")  as ""Count"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                            where s.""Id""='{id}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' group by TaskStatusCode
               
                ) t on t.TaskStatusCode=ANY(s.""StatusCode"")

                where s.""InboxStageId""='{stageId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 order by s.""SequenceOrder"" asc";


                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "TASK").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


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

        //   public async Task<IList<TreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        //   {
        //       var expObj = new List<TreeViewViewModel>();
        //       if (expandingList != null)
        //       {
        //           expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
        //           var obj = expObj.Where(x => x.id == id).FirstOrDefault();
        //           if (obj.IsNotNull())
        //           {
        //               type = obj.Type;
        //               parentId = obj.ParentId;
        //               userRoleId = obj.UserRoleId;
        //               projectId = obj.ProjectId;
        //               stageId = obj.StageId;
        //           }
        //       }

        //       var list = new List<TreeViewViewModel>();
        //       var query = "";
        //       if (id.IsNullOrEmpty())
        //       {
        //           var roles = userRoleIds.Split(",");
        //           var roleText = "";
        //           foreach (var i in roles)
        //           {
        //               roleText += $"'{i}',";
        //           }
        //           roleText = roleText.Trim(',');
        //           query = $@"  WITH RECURSIVE NtsService AS ( 
        //	     SELECT s.""ServiceSubject"" as ts,s.""Id"",'Parent'  as Type
        //                        FROM public.""NtsService"" as s
        //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
        //	     and usp.""UserRoleId"" in ({roleText})

        //                        --left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	     --where s.""OwnerUserId""='{userId}'
        //	     union all
        //                        SELECT t.""TaskSubject"" as ts, s.""Id"",'Child'  as Type
        //                        FROM public.""NtsService"" as s
        //                        inner join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //		 join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" 

        //                       where t.""AssignedToUserId""='{userId}'

        //                     --where s.""OwnerUserId""='{userId}'
        //                   )
        //                   SELECT count(ts) from NtsService where Type!='Parent'";
        //           var count = await _queryRepo.ExecuteScalar<long?>(query, null);

        //           var item = new TreeViewViewModel
        //           {
        //               id = "INBOX",
        //               Name = "Inbox",
        //               DisplayName = "Inbox",
        //               ParentId = null,
        //               hasChildren = true,
        //               expanded = true,
        //               Type = "INBOX"
        //           };
        //           if (count != null)
        //           {
        //               item.Name = item.DisplayName = $"Inbox ({count })";
        //           }
        //           list.Add(item);

        //       }
        //       else if (id == "INBOX")
        //       {
        //           var roles = userRoleIds.Split(",");
        //           var roleText = "";
        //           foreach (var item in roles)
        //           {
        //               roleText += $"'{item}',";
        //           }
        //           roleText = roleText.Trim(',');
        //           query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
        //           , 'INBOX' as ParentId, 'USERROLE' as Type,
        //           true as hasChildren, ur.""Id"" as UserRoleId
        //           from public.""UserRole"" as ur
        //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'
        //           left join(
        //	 WITH RECURSIVE NtsService AS ( 
        //	     SELECT distinct t.""TaskSubject"" as ts,s.""Id"",usp.""UserRoleId"",'Parent'  as Type
        //                        FROM public.""NtsService"" as s
        //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
        //	     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	     where usp.""UserRoleId"" in ({roleText}) and t.""AssignedToUserId""='{userId}'
        //	     union all
        //                        SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""UserRoleId"",'Child'  as Type
        //                        FROM public.""NtsService"" as s
        //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" 
        //                          where t.""AssignedToUserId""='{userId}'

        //                     --where s.""OwnerUserId""='{userId}'
        //                   )
        //                   SELECT count(ts) as ""Count"",""UserRoleId"" from NtsService where Type!='Parent' group by ""UserRoleId""
        //) nt on nt.""UserRoleId""=ur.""Id""
        //           where ur.""Id"" in ({roleText})
        //           --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
        //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
        //           //expanded -> type= userrole - from coming list find type as userRole
        //           // if found then find the item in list as selcted item id
        //           //make expanded true

        //           var obj = expObj.Where(x => x.Type == "USERROLE").FirstOrDefault();
        //           if (obj.IsNotNull())
        //           {
        //               var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
        //               if (data.IsNotNull())
        //               {
        //                   data.expanded = true;
        //               }
        //           }

        //       }
        //       else if (type == "USERROLE")
        //       {

        //           query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
        //           , usp.""InboxStageName"" as id, '{id}' as ParentId, 'PROJECTSTAGE' as Type,
        //           true as hasChildren, '{userRoleId}' as UserRoleId
        //           from public.""UserRole"" as ur
        //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'

        //            left join(
        //	WITH RECURSIVE NtsService AS ( 
        //                       SELECT distinct t.""TaskSubject"" as ts,s.""Id"",usp.""InboxStageName"" ,'Parent'  as Type
        //                        FROM public.""NtsService"" as s
        //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
        //	     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	     where usp.""UserRoleId"" = '{userRoleId}' and t.""AssignedToUserId""='{userId}'
        //	     union all
        //                        SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""InboxStageName"",'Child'  as Type
        //                        FROM public.""NtsService"" as s
        //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                      where t.""AssignedToUserId""='{userId}'
        //                     --where s.""OwnerUserId""='{userId}'                        
        //               )
        //               SELECT count(ts) as ""Count"",""InboxStageName"" from NtsService where Type!='Parent' group by ""InboxStageName""
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
        //       else if (type == "PROJECTSTAGE")
        //       {
        //           query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
        //           usp.""TemplateCode""  as id, '{id}' as ParentId,                
        //           'PROJECT' as Type,'{userRoleId}' as UserRoleId,
        //           true as hasChildren
        //           from public.""UserRole"" as ur
        //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'
        //           left join(
        //        WITH RECURSIVE NtsService AS ( 
        //               SELECT distinct t.""TaskSubject"" as ts,s.""Id"",usp.""TemplateCode"" ,'Parent'  as Type
        //                   FROM public.""NtsService"" as s
        //                   join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //	join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
        //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	where usp.""UserRoleId"" = '{userRoleId}' and t.""AssignedToUserId""='{userId}'
        //	union all
        //                   SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""TemplateCode"",'Child'  as Type
        //                   FROM public.""NtsService"" as s
        //                   join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                 where t.""AssignedToUserId""='{userId}'     
        //               --where s.""OwnerUserId""='{userId}'                        
        //               )
        //               SELECT count(ts) as ""Count"",""TemplateCode"" from NtsService where Type!='Parent' group by ""TemplateCode""

        //           ) t on usp.""TemplateCode""=t.""TemplateCode""

        //           where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}'
        //           Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
        //           order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
        //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


        //           var obj = expObj.Where(x => x.Type == "PROJECT").FirstOrDefault();
        //           if (obj.IsNotNull())
        //           {
        //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
        //               if (data.IsNotNull())
        //               {
        //                   data.expanded = true;
        //               }
        //           }

        //       }
        //       else if (type == "PROJECT")
        //       {
        //           query = $@"select  s.""Id"" as id,
        //           s.""ServiceSubject"" ||' (' || case when t.""Count"" is not null then t.""Count"" else 0 end || ')' as Name,
        //           true as hasChildren,s.""Id"" as ProjectId,
        //           '{id}' as ParentId,'STAGE' as Type
        //           FROM public.""NtsService"" as s
        //           join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //            left join(
        //        WITH RECURSIVE NtsService AS ( 
        //               SELECT distinct t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" ,'Parent'  as Type
        //                   FROM public.""NtsService"" as s
        //                   join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	where tt.""Code""='{id}' and t.""AssignedToUserId""='{userId}'
        //	union all
        //                   SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" ,'Child'  as Type
        //                   FROM public.""NtsService"" as s
        //                   join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                 where t.""AssignedToUserId""='{userId}'     
        //               -- where s.""OwnerUserId""='{userId}'                        
        //               )
        //               SELECT count(ts) as ""Count"",""ServiceId"" from NtsService where Type!='Parent' group by ""ServiceId""

        //           ) t on s.""Id""=t.""ServiceId""

        //           Where tt.""Code""='{id}' --and s.""OwnerUserId"" = '{userId}' 
        //           GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


        //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


        //           var obj = expObj.Where(x => x.Type == "STAGE").FirstOrDefault();
        //           if (obj.IsNotNull())
        //           {
        //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
        //               if (data.IsNotNull())
        //               {
        //                   data.expanded = true;
        //               }
        //           }


        //       }

        //       else if (type == "STAGE")
        //       {
        //           query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
        //           s.""Id"" as id,
        //           true as hasChildren
        //           FROM public.""NtsService"" as s
        //           left join(
        //        WITH RECURSIVE NtsService AS ( 
        //               SELECT distinct t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
        //                   FROM public.""NtsService"" as s                      
        //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	where s.""ParentServiceId""='{id}' and t.""AssignedToUserId""='{userId}'
        //	union all
        //                   SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
        //                   FROM public.""NtsService"" as s
        //                   join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                where t.""AssignedToUserId""='{userId}'      
        //                --where s.""OwnerUserId""='{userId}'                        
        //               )
        //               SELECT count(ts) as ""Count"",""ServiceId"" from NtsService group by ""ServiceId""

        //           ) t on s.""Id""=t.""ServiceId""
        //           where s.""ParentServiceId""='{id}'
        //           order by s.""SequenceOrder"" asc";

        //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

        //       }
        //       else
        //       {

        //           //query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
        //           //'{parentId}' as StageId,s.""StatusCode"" as StatusCode,
        //           //false as hasChildren
        //           //FROM public.""UserRoleStageChild"" as s
        //           //--left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
        //           //left join(
        //           //    select case when task.""TaskStatusCode""='OVERDUE' then 'INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
        //           //    FROM public.""RecTask"" as s
        //           //    join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //           //    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //           //    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""

        //           //    Where tmp.""TemplateCode""='{parentId}' and task.""AssigneeUserId"" = '{userId}'
        //           //    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 

        //           //    group by TaskStatusCode  
        //           //) t on t.TaskStatusCode=ANY(s.""StatusCode"")
        //           //where s.""InboxStageId""='{stageId}'
        //           //order by s.""SequenceOrder"" asc";

        //           //list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

        //       }
        //       return list;
        //   }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceDashboardData(string userId, string performanceId, string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null, string stageId = null)
        {


            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,pd.""Name"" as ""ProjectName"",pd.""Name"" as ""ServiceStage"", s.""Id"" , s.""TemplateCode"" as TemplateCode
                          FROM public.""NtsService"" as s
                         left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					  where s.""Id""='{performanceId}'  and tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",case when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"", s.""TemplateCode"" as TemplateCode FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                           left join cms.""N_PerformanceDocument_PMSGoal"" as pg on pg.""Id""=s.""UdfNoteTableId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_userContext.CompanyId}'
                             left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
    left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'
                       join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' #SEARCH#
                 )
                 SELECT ""Code"",""TemplateId"",""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level,""TemplateCode"" from NtsService


                 union all

                 select tt.""Code"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level,t.""TemplateCode"" as TemplateCode
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""Template"" as tt on tt.""Id"" =nt.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
	                    where 1=1 and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' #AssigneeWhere# #StatusWhere#-- #TypeWhere# 
                    )
                 SELECT * from Nts where Level='Child'";

            var projectSerach = "";


            if (projectIds.IsNotNullAndNotEmpty())
            {
                string pids = projectIds.Replace(",", "','");
                //foreach (var i in projectIds)
                //{
                //    pids += $"'{i}',";
                //}
                //pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    projectSerach = @" and s.""Id"" in ('" + pids + "') ";
                }
            }
            //if (performanceId.IsNullOrEmpty() && projectIds.Count == 0)
            //{
            //    projectSerach = @$" where  s.""OwnerUserId""='{userId}'";
            //}
            //var ownerSerach = "";
            //if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            //{
            //    string oids = null;
            //    foreach (var i in ownerIds)
            //    {
            //        oids += $"'{i}',";
            //    }
            //    oids = oids.Trim(',');
            //    if (oids.IsNotNull())
            //    {

            //        ownerSerach = @" where t.""OwnerUserId"" in (" + oids + ") ";

            //    }
            //}
            //query = query.Replace("#OwnerWhere#", ownerSerach);

            var Assignee = "";
            //if (userRole.Contains("PROJECT_USER"))
            //{
            //    projectSerach = @$" where tt.""Code""='PMS_PERFORMANCE_DOCUMENT'";
            //    if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            //    {
            //        Assignee = @$" and t.""AssignedToUserId""='{userId}' ";
            //    }
            //    else
            //    {
            //        Assignee = @$" where t.""AssignedToUserId""='{userId}' ";
            //    }
            //}
            if (assigneeIds.IsNotNullAndNotEmpty())
            {
                string aids = assigneeIds.Replace(",", "','");
                //foreach (var i in assigneeIds)
                //{
                //    aids += $"'{i}',";
                //}
                //aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    //if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                    //{
                    //    Assignee = @" and t.""AssignedToUserId"" in (" + aids + ") ";
                    //}
                    //else
                    //{
                    //    Assignee = @" where t.""AssignedToUserId"" in (" + aids + ") ";
                    //}
                    Assignee = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                }
            }
            query = query.Replace("#AssigneeWhere#", Assignee);
            var statusSerach = "";
            if (tasksStatus.IsNotNullAndNotEmpty())
            {
                string sids = tasksStatus.Replace(",", "','");
                //foreach (var i in tasksStatus)
                //{
                //    sids += $"'{i}',";
                //}
                //sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    //if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0 && Assignee.IsNotNullAndNotEmpty()))
                    //{
                    //    statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    //}
                    //else
                    //{
                    //    statusSerach = @" where t.""TaskStatusId"" in (" + sids + ") ";
                    //}

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var typeSerach = "";
            if (type.IsNotNullAndNotEmpty())
            {
                string oids = type.Replace(",", "','");
                //foreach (var i in type)
                //{
                //    oids += $"'{i}',";
                //}
                //oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    //if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0 && Assignee.IsNotNullAndNotEmpty()))
                    //{
                    //    typeSerach = @" and tt.""Code"" in (" + oids + ") ";
                    //}
                    //else
                    //{
                    //    typeSerach = @" where tt.""Code"" in (" + oids + ") ";
                    //}

                    typeSerach = @" and tt.""Code"" in ('" + oids + "') ";
                }
            }
            query = query.Replace("#TypeWhere#", typeSerach);
            //var dateSerach = "";
            //if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            //{


            //    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0 && Assignee.IsNotNullAndNotEmpty()) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
            //    {
            //        if (column == FilterColumnEnum.DueDate)
            //        {
            //            dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
            //        }
            //        else
            //        {
            //            dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
            //        }

            //    }
            //    else
            //    {
            //        if (column == FilterColumnEnum.DueDate)
            //        {
            //            dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
            //        }
            //        else
            //        {
            //            dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
            //        }
            //    }


            //}
            //query = query.Replace("#DateWhere#", dateSerach);
            query = query.Replace("#SEARCH#", projectSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            //if (performanceId.IsNotNull() && projectIds.IsNotNull() && projectIds.Count > 0)
            //{
            //    queryData = queryData.Where(x => x.ParentId == projectIds[0]).ToList();
            //}
            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceDashboardTaskData(string userId, string performanceId, string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null,string stageId=null)
        {
            var query = $@"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,pd.""Name"" as ""ProjectName"",pd.""Name"" as ""ServiceStage"",pd.""Id"" as ""StageId"" , s.""TemplateCode"" as TemplateCode
                          FROM public.""NtsService"" as s
                         left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					  where s.""Id""='{performanceId}'  and tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",case when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"",case when pg.""Id"" is not null then pg.""StageId"" when pc.""Id"" is not null then pc.""StageId"" when pds.""Id"" is not null then pds.""Id"" end as ""StageId"", s.""TemplateCode"" as TemplateCode 
FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                           left join (select g.*,pdn.""Id"" as ""StageId"" from cms.""N_PerformanceDocument_PMSGoal"" as g
									  join public.""NtsService"" as gns on g.""Id""=gns.""UdfNoteTableId"" and gns.""IsDeleted""=false
									  join public.""NtsService"" as pds on pds.""Id""=gns.""ParentServiceId"" and pds.""IsDeleted""=false
									   --join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pds.""UdfNoteId""=pd.""NtsNoteId""  and pd.""IsDeleted""=false
									    join public.""NtsService"" as pdss on pds.""Id""=pdss.""ParentServiceId"" and pdss.""IsDeleted""=false
									  join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pdn on pdss.""UdfNoteId""=pdn.""NtsNoteId"" and pdn.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pdn.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
									  where  pdn.""Id""='{stageId}' and  g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}' and (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   
									  
						   ) as pg on pg.""Id""=s.""UdfNoteTableId""
					   left join (select g.*,pdn.""Id"" as ""StageId"" from cms.""N_PerformanceDocument_PMSCompentency"" as g
									  join public.""NtsService"" as gns on g.""Id""=gns.""UdfNoteTableId"" and gns.""IsDeleted""=false
									  join public.""NtsService"" as pds on pds.""Id""=gns.""ParentServiceId"" and pds.""IsDeleted""=false
									  
									    join public.""NtsService"" as pdss on pds.""Id""=pdss.""ParentServiceId"" and pdss.""IsDeleted""=false
									  join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pdn on pdss.""UdfNoteId""=pdn.""NtsNoteId"" and pdn.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pdn.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
									  where pdn.""Id""='{stageId}'  and g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}' and (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate"" 
									  
						   ) as pc on pc.""Id""=s.""UdfNoteTableId""
                             --left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
    left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'  and pds.""Id""='{stageId}' 
                       join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Code"",""TemplateId"",""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level,""TemplateCode"" from NtsService --where ""StageId""='{stageId}'


                 union all

                 select tt.""Code"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level,t.""TemplateCode"" as TemplateCode
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""Template"" as tt on tt.""Id"" =nt.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
	                    where 1=1 and t.""IsDeleted""=false and t.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'   #AssigneeWhere# #StatusWhere#
                    )
                 SELECT * from Nts where Level='Child'";
            #region
            //            var query = @$"
            //                           WITH RECURSIVE Nts AS(

            //                 WITH RECURSIVE NtsService AS(
            //                 SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
            //                       ,pd.""Name"" as ""ProjectName"",pd.""Name"" as ""ServiceStage"" , s.""TemplateCode"" as TemplateCode
            //                          FROM public.""NtsService"" as s
            //                         left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //					  where s.""Id""='{performanceId}'  and tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
            //                        union all
            //                        SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
            //                        ,ns.""ProjectName"",case when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"", s.""TemplateCode"" as TemplateCode FROM public.""NtsService"" as s
            //                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
            //                           left join cms.""N_PerformanceDocument_PMSGoal"" as pg on pg.""Id""=s.""UdfNoteTableId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_userContext.CompanyId}'
            //                             left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
            //    left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'
            //                       join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
            //                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' #SEARCH#
            //                 )
            //                 SELECT ""Code"",""TemplateId"",""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level,""TemplateCode"" from NtsService


            //                 union all

            //                 select tt.""Code"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level,t.""TemplateCode"" as TemplateCode
            //                     FROM  public.""NtsTask"" as t
            //                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
            //                        join public.""Template"" as tt on tt.""Id"" =nt.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
            //	                    where 1=1 and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' #AssigneeWhere# #StatusWhere#-- #TypeWhere# 
            //                    )
            //                 SELECT * from Nts where Level='Child'";

            //            var projectSerach = "";


            //            if (projectIds.IsNotNullAndNotEmpty())
            //            {
            //                string pids = projectIds.Replace(",", "','");

            //                if (pids.IsNotNull())
            //                {
            //                    projectSerach = @" and s.""Id"" in ('" + pids + "') ";
            //                }
            //            }
            #endregion
            var Assignee = "";            
            if (assigneeIds.IsNotNullAndNotEmpty())
            {
                string aids = assigneeIds.Replace(",", "','");               
                if (aids.IsNotNull())
                {                    
                    Assignee = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                }
            }
            query = query.Replace("#AssigneeWhere#", Assignee);
            var statusSerach = "";
            if (tasksStatus.IsNotNullAndNotEmpty())
            {
                string sids = tasksStatus.Replace(",", "','");                
                if (sids.IsNotNull())
                {
                    statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var typeSerach = "";
            if (type.IsNotNullAndNotEmpty())
            {
                string oids = type.Replace(",", "','");                
                if (oids.IsNotNull())
                {                    
                    typeSerach = @" and tt.""Code"" in ('" + oids + "') ";
                }
            }
            query = query.Replace("#TypeWhere#", typeSerach);     
            //query = query.Replace("#SEARCH#", projectSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);           
            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadMindMapData(string projectId)
        {
            var query = @$"select t.""Id"",t.""ServiceSubject"" as Title,t.""StartDate"" as Start,t.""DueDate"" as End,""ParentId"",'' as ""UserName"",t.""Priority"",t.""NtsStatus"",t.""Type"" FROM public.""NtsService"" as s 
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",'PROJECT' as ""Type""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",'STAGE' as ""Type""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"",""Type"" from NtsService ) t on t.tmp=s.""Id""
				 where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'

				 union
                 select t.""Id"",t.""TaskSubject"" as Title,t.""StartDate"" as Start,t.""DueDate"" as End,""ParentId"",t.""UserName"" as ""UserName"",t.""Priority"",t.""NtsStatus"",t.""Type"" FROM public.""NtsService"" as s
                 join(
                WITH RECURSIVE NtsService AS (
                    SELECT t.*, s.""Id"" as ""ServiceId"", case when t.""ParentTaskId"" is null then s.""Id"" else t.""ParentTaskId"" end as ""ParentId"",u.""Name"" as ""UserName"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",case when t.""ParentTaskId"" is null then 'TASK' else 'SUBTASK' end as ""Type""
                        FROM public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT t.*, s.""Id"" as ""ServiceId"",case when t.""ParentTaskId"" is null then s.""Id"" else t.""ParentTaskId"" end as  ""ParentId"",u.""Name"" as ""UserName"" ,'{projectId}' as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",case when t.""ParentTaskId"" is null then 'TASK' else 'SUBTASK' end as ""Type""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId"" 
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT ""Id"",""TaskSubject"",""StartDate"",""DueDate"",""ParentId"",""UserName"",tmp,""Priority"",""NtsStatus"",""Type"" from NtsService where ""Id"" is not null ) t on t.tmp=s.""Id""
				 where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";


            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceTaskGridViewData(string userId, string performanceId, string objectiveId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> type = null, InboxTypeEnum? inboxType = null)
        {

            var query = @$"
                          

                 select t.""TemplateCode"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ServiceSubject"" as ""PerformanceName"",s.""ServiceSubject"" as ""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                  
                 FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
               and usp.""InboxCode""='PMS' and usp.""ChildSequence""!='1'
						    
                            join public.""NtsTag"" as tag on tag.""TagId""=s.""Id"" and tag.""NtsType""=1 and tag.""IsDeleted""=false and tag.""CompanyId""='{_userContext.CompanyId}'
                            join  public.""NtsTask"" as t on t.""Id""=tag.""NtsId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                       
                        join public.""NtsService"" as nt on s.""ParentServiceId"" =nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_userContext.CompanyId}'
                        
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
	                    where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  #SEARCH#  #OwnerWhere#  #AssigneeWhere# #StatusWhere# #TypeWhere# #DateWhere#   
                    ";

            //            var query = @$"
            //                           WITH RECURSIVE Nts AS(

            //                 WITH RECURSIVE NtsService AS(
            //                 SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
            //                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
            //                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
            //                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
            //					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id""
            //					  #SEARCH#
            //                        union all
            //                        SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
            //                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
            //                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
            //join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //                     join public.""User"" as u on s.""OwnerUserId""=u.""Id""
            //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
            //                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
            //                 )
            //                 SELECT ""Code"",""TemplateId"",""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


            //                 union all

            //                 select tt.""Code"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
            //                     FROM  public.""NtsTask"" as t
            //                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
            //join public.""Template"" as tt on tt.""Id"" =nt.""TemplateId"" 
            //                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
            //                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
            //                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""
            //	                      #OwnerWhere#  #AssigneeWhere# #StatusWhere# #TypeWhere# #DateWhere#   
            //                    )
            //                 SELECT * from Nts where Level='Child'";

            if (inboxType == InboxTypeEnum.RECEIVED)
            {
                query = @$"
                        

                 select t.""TemplateCode"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ServiceSubject"" as ""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        
                        join public.""Template"" as tt on tt.""Id"" =t.""TemplateId"" and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and (md.""Code""='PerformanceDocument' or md.""TemplateCategoryType""=0)
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and t.""AssignedToUserId""='{userId}'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""
                        left join public.""NtsService"" as nt on t.""ParentServiceId"" =nt.""Id""
	                      #OwnerWhere#  #AssigneeWhere# #StatusWhere# #TypeWhere# #DateWhere#   
                  ";
            }
            if (inboxType == InboxTypeEnum.SENT)
            {
                query = @$"
                        

                 select t.""TemplateCode"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ServiceSubject"" as ""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        
                        join public.""Template"" as tt on tt.""Id"" =t.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""TemplateCategoryType""=0 and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'                       
                        join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and t.""OwnerUserId""='{userId}' and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""NtsService"" as nt on t.""ParentServiceId"" =nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_userContext.CompanyId}'
	                     where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' #OwnerWhere#  #AssigneeWhere# #StatusWhere# #TypeWhere# #DateWhere#   
                  ";
            }

            var projectSerach = "";

            if (performanceId.IsNotNull())
            {
                projectSerach = @$" and s.""ParentServiceId""='{performanceId}' ";

            }
            if (objectiveId.IsNotNull())
            {
                projectSerach = @$" and s.""Id""='{objectiveId}' ";

            }
            else if (projectIds.IsNotNull() && projectIds.Count > 0)
            {
                string pids = null;
                foreach (var i in projectIds)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    projectSerach = @" and s.""Id"" in (" + pids + ") ";
                }
            }
            //if (performanceId.IsNullOrEmpty() && projectIds.Count == 0 && inboxType != InboxTypeEnum.RECEIVED && inboxType != InboxTypeEnum.SENT)
            //{
            //    projectSerach = @$" and tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}'";
            //}
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {

                    ownerSerach = @" and t.""OwnerUserId"" in (" + oids + ") ";

                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);

            var Assignee = "";

            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    Assignee = @" and t.""AssignedToUserId"" in (" + aids + ") ";


                }
            }
            query = query.Replace("#AssigneeWhere#", Assignee);
            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var typeSerach = "";
            if (type.IsNotNull() && type.Count > 0)
            {
                string oids = null;
                foreach (var i in type)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    typeSerach = @" and tt.""Code"" in (" + oids + ") ";

                }
            }
            query = query.Replace("#TypeWhere#", typeSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0 && Assignee.IsNotNullAndNotEmpty()) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }
            query = query.Replace("#DateWhere#", dateSerach);
            query = query.Replace("#SEARCH#", projectSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            if (performanceId.IsNotNull() && projectIds.IsNotNull() && projectIds.Count > 0)
            {
                queryData = queryData.Where(x => x.ParentId == projectIds[0]).ToList();
            }
            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceUserWorkloadGridViewData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var users = await _repo.GetListGlobal<UserViewModel, User>(x => x.LineManagerId == userId);
            if (users.Count == 0)
            {
                return new List<ProjectGanttTaskViewModel>();
            }
            string userIds = null;
            if (users.IsNotNull() && users.Count > 0)
            {
                var Ids = users.Select(x => x.Id).ToList();
                foreach (var i in Ids)
                {
                    userIds += $"'{i}',";
                }
                userIds = userIds.Trim(',');
            }

            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' #ProjectWhere#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                       where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id  and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
	                    #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere# and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'       
                    )
                 SELECT * from Nts where Level='Child'";
            //      var query = @$"select distinct t.""Id"",t.""TaskSubject"" as ""Title"",t.""StartDate"" as ""Start"",t.""DueDate"" as ""End"",
            //                  t.""ServiceStage"" as ""ServiceStage"",""ParentId"",t.""UserName"" as ""UserName"",t.""OwnerName"" as ""OwnerName"",true as ""Summary"",
            //                  t.""Priority"",t.""NtsStatus"",pj.""ServiceSubject"" as ""ProjectName""
            //                  FROM public.""NtsService"" as s
            //                  join(
            //                  WITH RECURSIVE NtsService AS (
            //                  SELECT  t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",s.""Id""  as tmp,
            //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
            //                  FROM public.""NtsService"" as s
            //                  left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                  left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
            //                  left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
            //                  left join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //                  left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""
            //where  t.""AssignedToUserId"" in ({userIds}) #ProjectWhere# #OwnerWhere# #AssigneeWhere# #StatusWhere#

            //                  union all
            //                  SELECT  t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"" ,s.""Id"" as tmp,
            //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
            //                  FROM public.""NtsService"" as s
            //                  join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
            //                  join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                  join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
            //                  join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
            //                  join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //                  join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""
            //                  where  t.""AssignedToUserId"" in ({userIds}) #ProjectWhere# #OwnerWhere# #AssigneeWhere# #StatusWhere#
            //                  )
            //                   SELECT distinct ""Id"",""TaskSubject"",""StartDate"",""DueDate"",""ParentId"",""UserName"",""OwnerName"",tmp,""Priority"",""NtsStatus"",
            //                  ""ServiceStage"" from NtsService where ""Id"" is not null ) t on t.tmp=s.""Id""
            //      left join public.""NtsService"" as pj on pj.""Id""=tmp 

            //                  ";
            var projectSerach = "";
            if (projectIds.IsNotNull() && projectIds.Count > 0)
            {
                string pids = null;
                foreach (var i in projectIds)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    projectSerach = @" s.""Id"" in (" + pids + ") ";
                }
            }
            else
            {
                projectSerach = @$" tt.""Code""='PROJECT_SUPER_SERVICE'";
            }
            query = query.Replace("#ProjectWhere#", projectSerach);
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" where t.""OwnerUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in (" + aids + ") ";
                    }
                    else
                    {
                        assigneeSerach = @" where t.""AssignedToUserId"" in (" + aids + ") ";
                    }

                }
            }
            query = query.Replace("#AssigneeWhere#", assigneeSerach);
            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" where t.""TaskStatusId"" in (" + sids + ") ";
                    }

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }
            query = query.Replace("#DateWhere#", dateSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceTaskUserGridViewData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null, List<string> stageIds = null)
        {
            var users = await _repo.GetListGlobal<UserViewModel, User>(x => x.LineManagerId == userId);
            //var position = await _hrCoreBusiness.GetPostionHierarchyParentId(null);
            //if (position != null && position.Id.IsNotNullAndNotEmpty())
            //{
            //    var positionchildList = await _hrCoreBusiness.GetPositionHierarchy(position.Id, 1);
            //    if (positionchildList != null && positionchildList.Count() > 0)
            //    {
            //        users = positionchildList.ConvertAll(x => new UserViewModel
            //        {
            //            Id = x.UserId,
            //            Name = x.DisplayName
            //        }).Distinct().Where(x => x.Id.IsNotNullAndNotEmpty()).ToList();
            //    }

            //}
            users = await GetUserHierarchy();
            if (users.Count == 0)
            {
                return new List<ProjectGanttTaskViewModel>();
            }
            string userIds = null;
            if (users.IsNotNull() && users.Count > 0)
            {
                var Ids = users.Select(x => x.Id).Distinct().ToList();
                foreach (var i in Ids)
                {
                    userIds += $"'{i}',";
                }
                userIds = userIds.Trim(',');
            }

            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,pd.""Name"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"",tt.""Code"" as ""Type"" FROM public.""NtsService"" as s
                       join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id"" =s.""UdfNoteTableId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'   
join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where #ProjectWhere#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",case when tt1.""Code""='PMS_PERFORMANCE_DOCUMENT' then s.""ServiceSubject"" when tt1.""Code""='PMS_GOAL' then g.""GoalName"" when tt1.""Code""='PMS_COMPENTENCY' then c.""CompetencyName""
                when tt1.""Code"" = 'PMS_DEVELOPMENT' then d.""DevelopmentName"" when tt1.""Code"" = 'PMS_PEER_REVIEW' then s.""ServiceSubject"" END as ""ServiceStage"",tt1.""Code"" as ""Type"" 
FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                        join public.""Template"" as tt1 on tt1.""Id"" =s.""TemplateId"" and  tt1.""IsDeleted""=false and tt1.""CompanyId""='{_userContext.CompanyId}'
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                     left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and  pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'

                     left join cms.""N_PerformanceDocument_PMSDevelopment"" as d on d.""Id"" = s.""UdfNoteTableId"" and d.""IsDeleted"" = false and d.""CompanyId"" = '{_userContext.CompanyId}'

                      left join cms.""N_PerformanceDocument_PMSCompentency"" as c on c.""Id"" = s.""UdfNoteTableId"" and c.""IsDeleted"" = false and c.""CompanyId"" = '{_userContext.CompanyId}'

                       left join cms.""N_PerformanceDocument_PMSGoal"" as g on g.""Id"" = s.""UdfNoteTableId"" and g.""IsDeleted"" = false and g.""CompanyId"" = '{_userContext.CompanyId}'
where 1=1  #PmsTypeWhere#  
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level,
                case when ""Type""='PMS_PERFORMANCE_DOCUMENT' then 'Performance Document' when ""Type""='PMS_GOAL' then 'Goal' when ""Type""='PMS_COMPENTENCY' then 'Competency'
                when ""Type""='PMS_DEVELOPMENT' then 'Development' when ""Type""='PMS_PEER_REVIEW' then 'PeerReview' END as ""Type""
                from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level,
                --case when nt.""Type""='PMS_PERFORMANCE_DOCUMENT' then 'Performance Document' when nt.""Type""='PMS_GOAL' then 'Goal' when nt.""Type""='PMS_COMPENTENCY' then 'Competency'
                --when nt.""Type""='PMS_DEVELOPMENT' then 'Development' when nt.""Type""='PMS_PEER_REVIEW' then 'PeerReview' END as ""Type""
                nt.""Type""
                FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and  ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
	                   where t.""IsDeleted""=false #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere# #StagesWhere#       
                    )
                 SELECT * from Nts where Level='Child'";
            //      var query = @$"select distinct t.""Id"",t.""TaskSubject"" as ""Title"",t.""StartDate"" as ""Start"",t.""DueDate"" as ""End"",
            //                  t.""ServiceStage"" as ""ServiceStage"",""ParentId"",t.""UserName"" as ""UserName"",t.""OwnerName"" as ""OwnerName"",true as ""Summary"",
            //                  t.""Priority"",t.""NtsStatus"",pj.""ServiceSubject"" as ""ProjectName""
            //                  FROM public.""NtsService"" as s
            //                  join(
            //                  WITH RECURSIVE NtsService AS (
            //                  SELECT  t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",s.""Id""  as tmp,
            //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
            //                  FROM public.""NtsService"" as s
            //                  left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                  left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
            //                  left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
            //                  left join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //                  left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""
            //where  t.""AssignedToUserId"" in ({userIds}) #ProjectWhere# #OwnerWhere# #AssigneeWhere# #StatusWhere#

            //                  union all
            //                  SELECT  t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"" ,s.""Id"" as tmp,
            //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
            //                  FROM public.""NtsService"" as s
            //                  join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
            //                  join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                  join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
            //                  join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
            //                  join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
            //                  join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""
            //                  where  t.""AssignedToUserId"" in ({userIds}) #ProjectWhere# #OwnerWhere# #AssigneeWhere# #StatusWhere#
            //                  )
            //                   SELECT distinct ""Id"",""TaskSubject"",""StartDate"",""DueDate"",""ParentId"",""UserName"",""OwnerName"",tmp,""Priority"",""NtsStatus"",
            //                  ""ServiceStage"" from NtsService where ""Id"" is not null ) t on t.tmp=s.""Id""
            //      left join public.""NtsService"" as pj on pj.""Id""=tmp 

            //                  ";
            var projectSerach = "";
            if (projectIds.IsNotNull() && projectIds.Count > 0)
            {
                string pids = null;
                foreach (var i in projectIds)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    projectSerach = @" s.""Id"" in (" + pids + ") ";
                }
            }
            else
            {
                projectSerach = @$" tt.""Code""='PMS_PERFORMANCE_DOCUMENT'";
            }
            query = query.Replace("#ProjectWhere#", projectSerach);
            var pmstypesearch = "";
            if (pmsTypes.IsNotNull() && pmsTypes.Count > 0)
            {
                string ptypes = null;
                foreach (var i in pmsTypes)
                {
                    ptypes += $"'{i}',";
                }
                ptypes = ptypes.Trim(',');
                if (ptypes.IsNotNull())
                {
                    pmstypesearch = @" and tt1.""Code"" in (" + ptypes + ") ";
                }
                //if (pmsType == "PMS_GOAL")
                //{
                //    pmstypesearch = @$" and tt1.""Code""='PMS_GOAL'";
                //}
                //else if (pmsType == "PMS_COMPENTENCY")
                //{
                //    pmstypesearch = @$" and tt1.""Code""='PMS_COMPENTENCY'";
                //}
            }
            query = query.Replace("#PmsTypeWhere#", pmstypesearch);
            var stagessearch = "";
            if (stageIds.IsNotNull() && stageIds.Count > 0)
            {
                string stages = null;
                foreach (var i in stageIds)
                {
                    stages += $"'{i}',";
                }
                stages = stages.Trim(',');
                if (stages.IsNotNull())
                {
                    //stagessearch = @" and tt1.""Id"" in (" + stages + ") ";
                    stagessearch = @" and t.""ParentServiceId""  in (" + stages + ") ";
                }
            }
            query = query.Replace("#StagesWhere#", stagessearch);
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""OwnerUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in (" + aids + ") ";
                    }
                    else
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in (" + aids + ") ";
                    }

                }
            }
            else if (userIds.IsNotNullAndNotEmpty())
            {
                userIds = userIds.Trim(',');
                assigneeSerach = @" and t.""AssignedToUserId"" in (" + userIds + ") ";
            }
            query = query.Replace("#AssigneeWhere#", assigneeSerach);
            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }
            query = query.Replace("#DateWhere#", dateSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;

        }
        public async Task<IList<IdNameViewModel>> GetSubordinatesIdNameList()
        {
            var list = new List<IdNameViewModel>();
            //var position = await _hrCoreBusiness.GetPostionHierarchyParentId(null);
            //if (position != null && position.Id.IsNotNullAndNotEmpty())
            //{
            //    var positionchildList = await _hrCoreBusiness.GetPositionHierarchy(position.Id, 1);
            //    if (positionchildList != null && positionchildList.Count() > 0)
            //    {
            //        //list = positionchildList.ConvertAll(x => new IdNameViewModel
            //        //{
            //        //    Id = x.UserId,
            //        //    Name = x.DisplayName
            //        //}).Distinct().Where(x => x.Id.IsNotNullAndNotEmpty()).ToList();

            //        //list = positionchildList.Select(x => new IdNameViewModel
            //        //{
            //        //    Id = x.UserId,
            //        //    Name = x.DisplayName
            //        //}).Where(x => x.Id.IsNotNullAndNotEmpty()).Distinct().ToList();

            //        var list1 = positionchildList.GroupBy(x => new
            //        {
            //            Id = x.UserId,
            //            Name = x.DisplayName
            //        }).Select(g => g.FirstOrDefault()).Where(g => g.UserId.IsNotNullAndNotEmpty()).ToList();

            //        if (list1 != null && list1.Count > 0)
            //        {
            //            list = list1.Select(x => new IdNameViewModel
            //            {
            //                Id = x.UserId,
            //                Name = x.DisplayName
            //            }).ToList();
            //        }
            //    }
            //}
            var userlist = await GetUserHierarchy();
            list = userlist.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return list;
        }

        public async Task<List<UserViewModel>> GetUserHierarchy()
        {
            string query = $@"   WITH RECURSIVE ChildUserHierarchy AS(
          select  u.""Name"",u.""Id"",uh.""UserId""
from public.""UserHierarchy"" as uh
join public.""HierarchyMaster"" as hm on hm.""Id""=uh.""HierarchyMasterId"" and hm.""Code""='PERFORMANCE_HIERARCHY' and  hm.""IsDeleted""=false and hm.""CompanyId""='{_userContext.CompanyId}'
 join public.""User"" as u on u.""Id""=uh.""UserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
	 where uh.""ParentUserId""='{_userContext.UserId}' and uh.""LevelNo""=1 and uh.""OptionNo""=1 and  uh.""IsDeleted""=false and uh.""CompanyId""='{_userContext.CompanyId}'

                              union all

                                 select u.""Name"",u.""Id"",uh.""UserId""
from public.""UserHierarchy"" as uh
join ChildUserHierarchy as cuh on uh.""ParentUserId""=cuh.""UserId""
join public.""HierarchyMaster"" as hm on hm.""Id""=uh.""HierarchyMasterId"" and hm.""Code""='PERFORMANCE_HIERARCHY' and  hm.""IsDeleted""=false and hm.""CompanyId""='{_userContext.CompanyId}'
     join public.""User"" as u on u.""Id""=uh.""UserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
	 where  uh.""LevelNo""=1 and uh.""OptionNo""=1       and  uh.""IsDeleted""=false and uh.""CompanyId""='{_userContext.CompanyId}'          )
                            select * from ChildUserHierarchy
                            ";



            var queryData = await _queryGantt.ExecuteQueryList<UserViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceObjectivesGridViewData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null, List<string> stageIds = null, string statusCodes = null)
        {
            var users = await _repo.GetListGlobal<UserViewModel, User>(x => x.LineManagerId == userId);
            //var position = await _hrCoreBusiness.GetPostionHierarchyParentId(null);
            //if (position != null && position.Id.IsNotNullAndNotEmpty())
            //{
            //    var positionchildList = await _hrCoreBusiness.GetPositionHierarchy(position.Id, 1);
            //    if (positionchildList != null && positionchildList.Count() > 0)
            //    {
            //        users = positionchildList.ConvertAll(x => new UserViewModel
            //        {
            //            Id = x.UserId,
            //            Name = x.DisplayName
            //        }).Distinct().Where(x => x.Id.IsNotNullAndNotEmpty()).ToList();
            //    }

            //}
            users = await GetUserHierarchy();
            if (users.Count == 0)
            {
                return new List<ProjectGanttTaskViewModel>();
            }
            string userIds = null;
            if (users.IsNotNull() && users.Count > 0)
            {
                var Ids = users.Select(x => x.Id).Distinct().ToList();
                foreach (var i in Ids)
                {
                    userIds += $"'{i}',";
                }
                userIds = userIds.Trim(',');
            }

            //            var query = @$"
            //                           WITH RECURSIVE Nts AS(

            //                 WITH RECURSIVE NtsService AS(
            //                 SELECT s.*,pd.""Year"" as ""Year"", s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",tt.""Code"" as TemplateCode
            //                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",tt.""Code"" as ""Type""
            //FROM public.""NtsService"" as s
            // join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=s.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                            join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'
            //					 where #ProjectWhere#
            //                        union all
            //                        SELECT s.*,pd.""Year"" as ""Year"", s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",tt1.""Code"" as TemplateCode
            //                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",tt1.""Code"" as ""Type"" 
            //FROM public.""NtsService"" as s
            //                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
            // join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=ns.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""Template"" as tt1 on tt1.""Id"" =s.""TemplateId"" and  tt1.""IsDeleted""=false and tt1.""CompanyId""='{_userContext.CompanyId}'
            //                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                     join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'
            //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                    where 1=1  #DateWhere#  and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
            //                 )
            //                 SELECT ""Id"",""Year"",""ServiceId"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""OwnerUserId"",""ServiceStatusId"",""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusCode"",TemplateCode,'Parent' as Level,
            //                case when ""Type""='PMS_PERFORMANCE_DOCUMENT' then 'Performance Document' when ""Type""='PMS_GOAL' then 'Goal' when ""Type""='PMS_COMPENTENCY' then 'Competency'
            //                when ""Type""='PMS_DEVELOPMENT' then 'Development' when ""Type""='PMS_PEER_REVIEW' then 'PeerReview' END as ""Type""
            //                from NtsService
            //                where 1=1 #OwnerWhere# #PmsTypeWhere# #StatusWhere# #StagesWhere# #StatusCodeWhere#
            //                    )
            //                 SELECT * from Nts where Level='Parent' and ""Type"" is not null ";

            var query = @$"SELECT s.""Id"" as Id,pd.""Year"" as ""Year"", s.""Id"" as ""ServiceId"",u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",tt.""Code"" as TemplateCode
                                   ,s.""ServiceSubject"" as ""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",tt.""Code"" as ""Type"",to_char(s.""StartDate"", 'yyyy-MM-dd') as Start,to_char(s.""DueDate"", 'yyyy-MM-dd') as End
            FROM public.""NtsService"" as s
             join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=s.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                                     join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                                     join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                                     join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                                        join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'
            					 where  s.""OwnerUserId"" in ({userIds}) 

union

 select s.""Id"" as Id,pd.""Year"" as ""Year"", s.""Id"" as ""ServiceId"",u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",s.""TemplateCode"" as TemplateCode
                                   ,pm.""ServiceSubject"" as ""ProjectName"",pg.""GoalName"" as ""ServiceStage"",s.""TemplateCode"" as ""Type"",pg.""GoalStartDate"" as Start,pg.""GoalEndDate"" as End
                              		 FROM public.""NtsService"" as s 
                          join cms.""N_PerformanceDocument_PMSGoal"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
                            join public.""NtsService"" as pm on s.""ParentServiceId""=pm.""Id""
                        join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=pm.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
             
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
 left join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'
                where s.""TemplateCode""='PMS_GOAL' 
and  s.""IsDeleted""=false and pm.""OwnerUserId"" in ({userIds})

union

 select s.""Id"" as Id,pd.""Year"" as ""Year"",s.""Id"" as ""ServiceId"",u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",s.""TemplateCode"" as TemplateCode
                                   ,pm.""ServiceSubject"" as ""ProjectName"",pg.""CompetencyName"" as ""ServiceStage"",s.""TemplateCode"" as ""Type"",pg.""CompetencyStartDate"" as Start,pg.""CompetencyEndDate"" as End
                              		 FROM public.""NtsService"" as s 
                          join cms.""N_PerformanceDocument_PMSCompentency"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
                            join public.""NtsService"" as pm on s.""ParentServiceId""=pm.""Id""
                        join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=pm.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
             
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
 left join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'
                where s.""TemplateCode""='PMS_COMPENTENCY' 
and  s.""IsDeleted""=false and pm.""OwnerUserId"" in ({userIds})

union

 select s.""Id"" as Id,pd.""Year"" as ""Year"",s.""Id"" as ""ServiceId"",u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",s.""TemplateCode"" as TemplateCode
                                   ,pm.""ServiceSubject"" as ""ProjectName"",pg.""DevelopmentName"" as ""ServiceStage"",s.""TemplateCode"" as ""Type"",pg.""DevelopmentStartDate"" as Start,pg.""DevelopmentEndDate"" as End
                              		 FROM public.""NtsService"" as s 
                          join cms.""N_PerformanceDocument_PMSDevelopment"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
                            join public.""NtsService"" as pm on s.""ParentServiceId""=pm.""Id""
                        join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=pm.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
             
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
 left join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'    
                where s.""TemplateCode""='PMS_DEVELOPMENT' 
and  s.""IsDeleted""=false and pm.""OwnerUserId"" in ({userIds})

";

            //var projectSerach = "";
            //if (projectIds.IsNotNull() && projectIds.Count > 0)
            //{
            //    string pids = null;
            //    foreach (var i in projectIds)
            //    {
            //        pids += $"'{i}',";
            //    }
            //    pids = pids.Trim(',');
            //    if (pids.IsNotNull())
            //    {
            //        projectSerach = @" s.""Id"" in (" + pids + ") ";
            //    }
            //}
            //else
            //{
            //    projectSerach = @$" tt.""Code""='PMS_PERFORMANCE_DOCUMENT'";
            //}
            //query = query.Replace("#ProjectWhere#", projectSerach);

            //var pmstypesearch = "";
            //if (pmsTypes.IsNotNull() && pmsTypes.Count > 0)
            //{
            //    string ptypes = null;
            //    foreach (var i in pmsTypes)
            //    {
            //        ptypes += $"'{i}',";
            //    }
            //    ptypes = ptypes.Trim(',');
            //    if (ptypes.IsNotNull())
            //    {
            //        pmstypesearch = @" and ""Type"" in (" + ptypes + ") ";
            //    }

            //}
            //query = query.Replace("#PmsTypeWhere#", pmstypesearch);

            //var stagessearch = "";
            //if (stageIds.IsNotNull() && stageIds.Count > 0)
            //{
            //    string stages = null;
            //    foreach (var i in stageIds)
            //    {
            //        stages += $"'{i}',";
            //    }
            //    stages = stages.Trim(',');
            //    if (stages.IsNotNull())
            //    {
            //        //stagessearch = @" and tt1.""Id"" in (" + stages + ") ";
            //        stagessearch = @" and ""ServiceId""  in (" + stages + ") ";
            //    }
            //}
            //query = query.Replace("#StagesWhere#", stagessearch);
            //var ownerSerach = "";
            //if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            //{
            //    string oids = null;
            //    foreach (var i in ownerIds)
            //    {
            //        oids += $"'{i}',";
            //    }
            //    oids = oids.Trim(',');
            //    if (oids.IsNotNull())
            //    {
            //        ownerSerach = @" AND (""OwnerUserId"" in (" + oids + ") #AssigneeWhere#)";
            //    }
            //}
            //else if (userIds.IsNotNullAndNotEmpty())
            //{
            //    userIds = userIds.Trim(',');
            //    ownerSerach = @" AND (""OwnerUserId"" in (" + userIds + ") #AssigneeWhere#)";
            //}
            //query = query.Replace("#OwnerWhere#", ownerSerach);
            //var assigneeSerach = "";
            //if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            //{
            //    string aids = null;
            //    foreach (var i in assigneeIds)
            //    {
            //        aids += $"'{i}',";
            //    }
            //    aids = aids.Trim(',');
            //    if (aids.IsNotNull())
            //    {
            //        if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            //        {
            //            assigneeSerach = @" and ""RequestedByUserId"" in (" + aids + ") ";
            //        }
            //        else
            //        {
            //            assigneeSerach = @" and ""RequestedByUserId"" in (" + aids + ") ";
            //        }

            //    }
            //}
            //else if (userIds.IsNotNullAndNotEmpty())
            //{
            //    userIds = userIds.Trim(',');
            //    if(ownerIds.IsNotNull() && ownerIds.Count > 0)
            //    {
            //        assigneeSerach = @" and ""RequestedByUserId"" in (" + userIds + ") ";
            //    }
            //    else
            //    {
            //        assigneeSerach = @" or ""RequestedByUserId"" in (" + userIds + ") ";
            //    }

            //}
            //query = query.Replace("#AssigneeWhere#", assigneeSerach);
            //var statusSerach = "";
            //if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            //{
            //    string sids = null;
            //    foreach (var i in tasksStatus)
            //    {
            //        sids += $"'{i}',";
            //    }
            //    sids = sids.Trim(',');
            //    if (sids.IsNotNull())
            //    {
            //        if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0))
            //        {
            //            statusSerach = @" and ""ServiceStatusId"" in (" + sids + ") ";
            //        }
            //        else
            //        {
            //            statusSerach = @" and ""ServiceStatusId"" in (" + sids + ") ";
            //        }

            //    }
            //}
            //query = query.Replace("#StatusWhere#", statusSerach);

            //var scodeswhere = "";
            //if (statusCodes.IsNotNullAndNotEmpty())
            //{
            //    scodeswhere = $@" and ""NtsStatusCode"" in ('{statusCodes.Replace(",","','")}')";
            //}
            //query = query.Replace("#StatusCodeWhere#", scodeswhere);

            //var dateSerach = "";
            //if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            //{


            //    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
            //    {
            //        if (column == FilterColumnEnum.DueDate)
            //        {
            //            dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""DueDate"" and s.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
            //        }
            //        else
            //        {
            //            dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""StartDate"" and s.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
            //        }

            //    }
            //    else
            //    {
            //        if (column == FilterColumnEnum.DueDate)
            //        {
            //            dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""DueDate"" and s.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
            //        }
            //        else
            //        {
            //            dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""StartDate"" and s.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
            //        }
            //    }


            //}
            //query = query.Replace("#DateWhere#", dateSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            if (projectIds.IsNotNull() && projectIds.Count > 0)
            {
                //queryData = queryData.Where(x=>x.s)
            }
            if (pmsTypes.IsNotNull() && pmsTypes.Count > 0)
            {
                queryData = queryData.Where(x => pmsTypes.Contains(x.Type)).ToList();
            }
            if (stageIds.IsNotNull() && stageIds.Count > 0)
            {
                queryData = queryData.Where(x => stageIds.Contains(x.ServiceId)).ToList();
            }
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                queryData = queryData.Where(x => ownerIds.Contains(x.OwnerUserId)).ToList();
            }
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                queryData = queryData.Where(x => assigneeIds.Contains(x.AssigneeUserId)).ToList();
            }
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                queryData = queryData.Where(x => tasksStatus.Contains(x.TaskStatusId)).ToList();
            }
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {
                if (column == FilterColumnEnum.DueDate)
                {
                    queryData = queryData.Where(x => startDate <= x.End && x.End < dueDate).ToList();
                }
                else
                {
                    queryData = queryData.Where(x => startDate <= x.Start && x.Start < dueDate).ToList();
                }
            }


            return queryData;

        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadEmployeePerformanceObjectivesData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null, List<string> stageIds = null, string statusCodes = null)
        {


            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*,pd.""Year"" as ""Year"", s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",tt.""Code"" as TemplateCode
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",tt.""Code"" as ""Type""
FROM public.""NtsService"" as s
 join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=s.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'
					 where #ProjectWhere#
                        union all
                        SELECT s.*,pd.""Year"" as ""Year"", s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u1.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Code"" as ""NtsStatusCode"",tt1.""Code"" as TemplateCode
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",tt1.""Code"" as ""Type"" 
FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""NtsNoteId""=ns.""UdfNoteId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" as tt1 on tt1.""Id"" =s.""TemplateId"" and  tt1.""IsDeleted""=false and tt1.""CompanyId""='{_userContext.CompanyId}'
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                     join public.""User"" as u1 on s.""RequestedByUserId""=u1.""Id"" and  u1.""IsDeleted""=false and u1.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                    where 1=1  #DateWhere#  and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""Year"",""ServiceId"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""OwnerUserId"",""ServiceStatusId"",""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusCode"",TemplateCode,'Parent' as Level,
                case when ""Type""='PMS_PERFORMANCE_DOCUMENT' then 'Performance Document' when ""Type""='PMS_GOAL' then 'Goal' when ""Type""='PMS_COMPENTENCY' then 'Competency'
                when ""Type""='PMS_DEVELOPMENT' then 'Development' when ""Type""='PMS_PEER_REVIEW' then 'PeerReview' END as ""Type""
                from NtsService
                where 1=1 and ""Type""<> 'PMS_PERFORMANCE_DOCUMENT' #OwnerWhere# #PmsTypeWhere# #StatusWhere# #StagesWhere# #StatusCodeWhere#
                    )
                 SELECT * from Nts where Level='Parent'   and  ""Type"" is not null ";

            var projectSerach = "";
            if (projectIds.IsNotNull() && projectIds.Count > 0)
            {
                string pids = null;
                foreach (var i in projectIds)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    projectSerach = @" s.""Id"" in (" + pids + ") ";
                }
            }
            else
            {
                projectSerach = @$" tt.""Code""='PMS_PERFORMANCE_DOCUMENT'";
            }
            query = query.Replace("#ProjectWhere#", projectSerach);

            var pmstypesearch = "";
            if (pmsTypes.IsNotNull() && pmsTypes.Count > 0)
            {
                string ptypes = null;
                foreach (var i in pmsTypes)
                {
                    ptypes += $"'{i}',";
                }
                ptypes = ptypes.Trim(',');
                if (ptypes.IsNotNull())
                {
                    pmstypesearch = @" and ""Type"" in (" + ptypes + ") ";
                }
            }
            query = query.Replace("#PmsTypeWhere#", pmstypesearch);

            var stagessearch = "";
            if (stageIds.IsNotNull() && stageIds.Count > 0)
            {
                string stages = null;
                foreach (var i in stageIds)
                {
                    stages += $"'{i}',";
                }
                stages = stages.Trim(',');
                if (stages.IsNotNull())
                {
                    //stagessearch = @" and tt1.""Id"" in (" + stages + ") ";
                    stagessearch = @" and ""ServiceId""  in (" + stages + ") ";
                }
            }
            query = query.Replace("#StagesWhere#", stagessearch);
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" AND (""OwnerUserId"" in (" + oids + ") #AssigneeWhere#)";
                }
            }
            else if (userId.IsNotNullAndNotEmpty())
            {
                ownerSerach = $@" AND (""OwnerUserId""='{userId}' #AssigneeWhere#)";
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                    {
                        assigneeSerach = @" and ""RequestedByUserId"" in (" + aids + ") ";
                    }
                    else
                    {
                        assigneeSerach = @" and ""RequestedByUserId"" in (" + aids + ") ";
                    }

                }
            }
            else if (userId.IsNotNullAndNotEmpty())
            {
                if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                {
                    assigneeSerach = $@" and ""RequestedByUserId""='{userId}'";
                }
                else
                {
                    assigneeSerach = $@" or ""RequestedByUserId""='{userId}' ";
                }

            }
            query = query.Replace("#AssigneeWhere#", assigneeSerach);
            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0))
                    {
                        statusSerach = @" and ""ServiceStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" and ""ServiceStatusId"" in (" + sids + ") ";
                    }

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var scodeswhere = "";
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                scodeswhere = $@" and ""NtsStatusCode"" in ('{statusCodes.Replace(",", "','")}')";
            }
            query = query.Replace("#StatusCodeWhere#", scodeswhere);

            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""DueDate"" and s.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""StartDate"" and s.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""DueDate"" and s.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= s.""StartDate"" and s.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }
            query = query.Replace("#DateWhere#", dateSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;

        }
        public async Task<IList<IdNameViewModel>> GetPerformanceSharedList(string userId)
        {
            string query = $@"select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        
                        union
                        select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""NtsServiceShared"" as ss
                        join public.""NtsService"" as s on s.""Id""=ss.""NtsServiceId"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and ss.""SharedWithUserId""='{userId}' and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'

                        union
                        select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""NtsTask"" as t 
                        join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and t.""AssignedToUserId""='{userId}' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 

                        union
                        select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""Team"" as tm
                        join public.""NtsServiceShared"" as ss on ss.""SharedWithTeamId""=tm.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""NtsService"" as s on s.""Id""=ss.""NtsServiceId"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and  tm.""IsDeleted""=false and tm.""CompanyId""='{_userContext.CompanyId}'

            ";
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);

            return queryData;
        }
        public async Task<PerformanceDashboardViewModel> GetPerformanceDashboardDetails(string projectId, string stageId = null)
        {
            var query = @$"select count(t.""Id"") as TaskCount, p.""Id"" as ""Id"", p.""ServiceSubject"" as ""Name"",p.""ServiceNo"" as ""ServiceNo"",sp.""Name"" as ""Priority"",
                        u.""Id"" as ""UserId"",u.""Name"" as ""CreatedBy"",p.""LastUpdatedDate"" as ""UpdatedOn"",p.""CreatedDate"" as ""CreatedOn"",
                        p.""StartDate"" as ""StartDate"",p.""DueDate"" as ""DueDate"",  ss.""Name"" as ""ProjectStatus"",p.""ServiceDescription"" as ""Description"",
                        tt.""Id"" as ""TemplateId"",null as ""WbsTemplateId""
                            from public.""NtsService"" as p
                            join public.""LOV"" as sp on sp.""Id"" = p.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = p.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""NtsService"" as stage on stage.""ParentServiceId"" =  p.""Id"" and  stage.""IsDeleted""=false and stage.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""Template"" as tt on tt.""Id"" =p.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on u.""Id""=p.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""NtsTask"" as t on t.""ParentServiceId""=p.""Id"" and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where p.""Id"" = '{projectId}' and  p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' #StageFilter#
                            group by p.""Id"",sp.""Name"",u.""Id"",ss.""Name"",tt.""Id""
                            ";
            if (stageId.IsNotNullAndNotEmpty())
            {
                query = query.Replace("#StageFilter#", @$"and stage.""Id"" = '{stageId}'");
            }
            else
            {
                query = query.Replace("#StageFilter#", "");
            }
            var queryData = await _queryProjDashRepo.ExecuteQuerySingle(query, null);
            if (queryData != null)
            {
                queryData.TemplateUserType = queryData.UserId.IsNotNullAndNotEmpty() ? NtsUserTypeEnum.Owner : NtsUserTypeEnum.Shared;
            }

            var query1 = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					    where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusId"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT * from Nts where Level='Child'";
            var queryData1 = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query1, null);
            //queryData.TaskCount = queryData1.Count();
            return queryData;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatus(string userId, string projectId,string stageId=null)
        {

            var Ownersearch = "";
            var search = @$" and s.""Id""='{projectId}' ";

            if (projectId.IsNotNull())
            {
                search = @$" and s.""Id""='{projectId}' ";

            }
            if (userId.IsNotNull())
            {
                Ownersearch = @$" and  s.""OwnerUserId""='{userId}' ";

            }
            var query = $@"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,pd.""Name"" as ""ProjectName"",pd.""Name"" as ""ServiceStage"",pd.""Id"" as ""StageId"" , s.""TemplateCode"" as TemplateCode
                          FROM public.""NtsService"" as s
                         left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					  where 1=1 #SEARCH# #OWNERSEARCH#  and tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",case when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"",case when pg.""Id"" is not null then pg.""StageId"" when pc.""Id"" is not null then pc.""StageId"" when pds.""Id"" is not null then pds.""Id"" end as ""StageId"", s.""TemplateCode"" as TemplateCode 
FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                           left join (select g.*,pdn.""Id"" as ""StageId"" from cms.""N_PerformanceDocument_PMSGoal"" as g
									  join public.""NtsService"" as gns on g.""Id""=gns.""UdfNoteTableId"" and gns.""IsDeleted""=false
									  join public.""NtsService"" as pds on pds.""Id""=gns.""ParentServiceId"" and pds.""IsDeleted""=false
									   --join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pds.""UdfNoteId""=pd.""NtsNoteId""  and pd.""IsDeleted""=false
									    join public.""NtsService"" as pdss on pds.""Id""=pdss.""ParentServiceId"" and pdss.""IsDeleted""=false
									  join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pdn on pdss.""UdfNoteId""=pdn.""NtsNoteId"" and pdn.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pdn.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
									  where  pdn.""Id""='{stageId}' and  g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}' and (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   
									  
						   ) as pg on pg.""Id""=s.""UdfNoteTableId""
					   left join (select g.*,pdn.""Id"" as ""StageId"" from cms.""N_PerformanceDocument_PMSCompentency"" as g
									  join public.""NtsService"" as gns on g.""Id""=gns.""UdfNoteTableId"" and gns.""IsDeleted""=false
									  join public.""NtsService"" as pds on pds.""Id""=gns.""ParentServiceId"" and pds.""IsDeleted""=false
									  
									    join public.""NtsService"" as pdss on pds.""Id""=pdss.""ParentServiceId"" and pdss.""IsDeleted""=false
									  join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pdn on pdss.""UdfNoteId""=pdn.""NtsNoteId"" and pdn.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pdn.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
									  where pdn.""Id""='{stageId}'  and g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}' and (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate"" 
									  
						   ) as pc on pc.""Id""=s.""UdfNoteTableId""
                             --left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
    left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'  and pds.""Id""='{stageId}' 
                       join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
where 1=1 #OWNERSEARCH#  and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Code"",""TemplateId"",""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level,""TemplateCode"" from NtsService --where ""StageId""='{stageId}'


                 union all

                 select tt.""Code"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level,t.""TemplateCode"" as TemplateCode
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""Template"" as tt on tt.""Id"" =nt.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
	                    where 1=1 and t.""IsDeleted""=false and t.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'   
                    )
                 SELECT * from Nts where Level='Child'";
            //           var query = @$"
            //                          WITH RECURSIVE Nts AS(

            //                WITH RECURSIVE NtsService AS(
            //                SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
            //                      ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
            //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //				 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //				  where 1=1 #SEARCH# #OWNERSEARCH# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
            //                       union all
            //                       SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
            //                       ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
            //                       join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

            //                    join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                           join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //where 1=1 #OWNERSEARCH# 
            //                )
            //                SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusId"",'Parent' as Level from NtsService


            //                union all

            //                select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Child' as Level 
            //                    FROM  public.""NtsTask"" as t
            //                       join Nts as nt on t.""ParentServiceId"" =nt.""Id""
            //                       join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                       join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                  where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'

            //                   )
            //                SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            query = query.Replace("#OWNERSEARCH#", Ownersearch);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
           var data = new List<ProjectGanttTaskViewModel>();
            var serviceDetails = await _serviceBusiness.GetSingleById(stageId);
            if (serviceDetails.IsNotNull())
            {
                var udfNoteTableId = serviceDetails.UdfNoteTableId;
                if (udfNoteTableId.IsNotNullAndNotEmpty())
                {
                    var a = await GetTaskListByType("", projectId, userId, udfNoteTableId);
                    var b = await GetStageTaskList("", projectId, userId, stageId);
                    data.AddRange(a);
                    data.AddRange(b);
                }
            }

            var list1 = stageId.IsNotNullAndNotEmpty() ?  data.GroupBy(x => x.NtsStatusCode).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.TaskStatusId).FirstOrDefault() }).ToList():
                queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList(); ;
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetPerformanceServicesStatus(string userId, string projectId, string type,string stageId=null)
        {


            var search = @$" and s.""Id""='{projectId}' ";
            var Ownersearch = "";
            if (projectId.IsNotNull())
            {
                search = @$" and  s.""Id""='{projectId}' ";

            }
            if (userId.IsNotNull())
            {
                Ownersearch = @$" and  s.""OwnerUserId""='{userId}' ";

            }

            var query = @$"
                          

                 WITH RECURSIVE NtsService AS(
                 SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" 
FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""  and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
 join public.""TemplateCategory"" as md on md.""Id"" =tt.""TemplateCategoryId"" and md.""Code""='PerformanceDocument' and  md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where 1=1 #SEARCH# #OWNERSEARCH#
                        union all
                        SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
   join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
 where 1=1 #OWNERSEARCH#
                 )
                 SELECT ""Code"" as TemplateCode,""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusId"",'Parent' as Level from NtsService


               ";
            query = query.Replace("#SEARCH#", search);
            query = query.Replace("#OWNERSEARCH#", Ownersearch);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            queryData = queryData.Where(x => x.TemplateCode == type).ToList();
            var list = new List<ProjectDashboardChartViewModel>();

            IList<TeamWorkloadViewModel> data = new List<TeamWorkloadViewModel>();
            var serviceDetails = await _serviceBusiness.GetSingleById(stageId);
            if (serviceDetails.IsNotNull())
            {
                var udfNoteTableId = serviceDetails.UdfNoteTableId;
                if (udfNoteTableId.IsNotNullAndNotEmpty())
                {
                    if (type == "PMS_GOAL")
                    {
                        data = await ReadManagerPerformanceGoalViewData(projectId, udfNoteTableId, userId);
                    }
                    else if (type == "PMS_COMPENTENCY")
                    {
                        data = await ReadPerformanceCompetencyStageViewData(projectId, udfNoteTableId, userId);
                    }
                    else if (type == "PMS_DEVELOPMENT")
                    {
                        data = await ReadPerformanceDevelopmentViewData(projectId, udfNoteTableId, userId);
                    }
                }
            }


            var list1 = stageId.IsNotNullAndNotEmpty() ? data.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList() :
                queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList(); ;
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetPerformanceServiceStatus(string userId, string projectId, string type, string stageId = null)
        {
            var query = "";
            if (type== "PMS_GOAL") 
            {
                 query = @$" SELECT gns.""TemplateCode"" as TemplateCode,gns.""Id"",g.""GoalName"" as Title,g.""GoalStartDate"" as Start,g.""GoalEndDate"" as End,gns.""ParentServiceId"" as ParentId,u.""Name"" as ""UserName"",Pdm.""Name"" as ProjectName,Pdms.""Name"" as ""ServiceStage"",sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Parent' as Level
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{projectId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'
";
            }
            if (type == "PMS_COMPENTENCY")
            {
                query = @$" SELECT gns.""TemplateCode"" as TemplateCode,gns.""Id"",g.""CompetencyName"" as Title,g.""CompetencyStartDate"" as Start,g.""CompetencyEndDate"" as End,gns.""ParentServiceId"" as ParentId,u.""Name"" as ""UserName"",Pdm.""Name"" as ProjectName,Pdms.""Name"" as ""ServiceStage"",sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Parent' as Level
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{projectId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'
";
            }
            if (type == "PMS_DEVELOPMENT")
            {
                query = @$" SELECT gns.""TemplateCode"" as TemplateCode,gns.""Id"",g.""DevelopmentName"" as Title,g.""DevelopmentStartDate"" as Start,g.""DevelopmentEndDate"" as End,gns.""ParentServiceId"" as ParentId,u.""Name"" as ""UserName"",Pdm.""Name"" as ProjectName,Pdms.""Name"" as ""ServiceStage"",sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Parent' as Level
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_DEVELOPMENT' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSDevelopment"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""DevelopmentStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""DevelopmentEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{projectId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'
";
            }
          
           
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            queryData = queryData.Where(x => x.TemplateCode == type).ToList();
            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }

        /// <summary>
        /// Code For Chart that Bring Requested by me
        /// </summary>
        /// <param name="TemplateID"></param>
        /// /// <param name="UserID"></param>
        /// <returns></returns>
        /// 


        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMe(string TemplateID, string UserID)
        {


            var query = @$"select LOV.""Name"" as Type,Count(*) as Value  from public.""NtsTask"" as NTS left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId"" and  LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}' 
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and  ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and  person.""IsDeleted""=false and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and  Assign.""IsDeleted""=false and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and  Positions.""IsDeleted""=false and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and  Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and  person2.""IsDeleted""=false and person2.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}'
 and ""TemplateId"" = '{TemplateID}' and  NTS.""IsDeleted""=false and NTS.""CompanyId""='{_userContext.CompanyId}'
group by  LOV.""Name""
                          ";
            //query = query.Replace("#RequesUserID#",UserID).Replace("#TemplateID#",TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduserid(string TemplateID, string UserID)
        {


            var query = @$"select Ur.""Name"" as Type,Count(*) as Value, Ur.""Id"" 
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId"" and  LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and  Ur.""IsDeleted""=false and Ur.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and  person.""IsDeleted""=false and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and  Assign.""IsDeleted""=false and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and  Positions.""IsDeleted""=false and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and  Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and  person2.""IsDeleted""=false and person2.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}'
 and ""TemplateId"" = '{TemplateID}' and  NTS.""IsDeleted""=false and NTS.""CompanyId""='{_userContext.CompanyId}'

group by  Ur.""Name"", Ur.""Id""";
            //query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> MdlassignUser(string TemplateID, string UserID)
        {


            var query = @$"select Distinct(Ur.""Name"") as Type,Ur.""Id"" as RefId
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId"" and  LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}' 
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and  Ur.""IsDeleted""=false and Ur.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and  person.""IsDeleted""=false and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and  Assign.""IsDeleted""=false and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and  Positions.""IsDeleted""=false and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and  Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and  person2.""IsDeleted""=false and person2.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}'
 and ""TemplateId"" = '{TemplateID}' and  NTS.""IsDeleted""=false and NTS.""CompanyId""='{_userContext.CompanyId}'";
            ///  query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, AssigneeId = x.RefId }).ToList();
            return list;
        }

        public async Task<List<NtsTaskChartList>> GetGridList(string TemplateID, string UserID, List<string> assigneeIds = null, List<string> StatusIDs = null)
        {


            var query = @$"select NTS.""Id"", ""TaskSubject"",""TaskNo"",Ur.""Name"" as AssignName,LOV.""Name"" as Priority,LOVS.""Name"" as Status,NTS.""StartDate"",NTS.""DueDate""
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskPriorityId"" and  LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as LOVS on LOVS.""Id""=NTS.""TaskStatusId"" and  LOVS.""IsDeleted""=false and LOVS.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and  Ur.""IsDeleted""=false and Ur.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and  person.""IsDeleted""=false and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and  Assign.""IsDeleted""=false and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and  Positions.""IsDeleted""=false and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and  Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and  person2.""IsDeleted""=false and person2.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}'
 and ""TemplateId"" = '{TemplateID}' and  NTS.""IsDeleted""=false and NTS.""CompanyId""='{_userContext.CompanyId}' #AssigneeUser# #TaskStatusId# ";

            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    assigneeSerach = @"and NTS.""AssignedToUserId"" in (" + aids + ") ";
                }
            }
            query = query.Replace("#AssigneeUser#", assigneeSerach);


            var StatusSerach = "";
            if (StatusIDs.IsNotNull() && StatusIDs.Count > 0)
            {
                string aids = null;
                foreach (var i in StatusIDs)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    StatusSerach = @"and NTS.""TaskStatusId"" in (" + aids + ") ";
                }
            }
            query = query.Replace("#TaskStatusId#", StatusSerach);

            //query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<NtsTaskChartList>(query, null);

            //var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetDatewiseTask(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null)
        {


            var query = @$"select CAST(NTS.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(NTS.""TaskSLA"") as Days from public.""NtsTask"" as NTS 
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId"" and  LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and  Ur.""IsDeleted""=false and Ur.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and  person.""IsDeleted""=false and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and  Assign.""IsDeleted""=false and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and  Positions.""IsDeleted""=false and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and  Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and  person2.""IsDeleted""=false and person2.""CompanyId""='{_userContext.CompanyId}'
where NTS.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and NTS.""DueDate""::TIMESTAMP::DATE >='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE 
and NTS.""TaskStatusId""='370d3df9-5e97-4127-81e1-d7dd8fe8836a' and  NTS.""IsDeleted""=false and NTS.""CompanyId""='{_userContext.CompanyId}'
 Group by NTS.""DueDate""::TIMESTAMP::DATE ";
            //query = query.Replace("#RequesUserID#",UserID).Replace("#TemplateID#",TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetTaskType(string userId, string performanceId,string stageId=null)
        {


            var Ownersearch = "";
            var search = @$" and s.""Id""='{performanceId}' ";

            if (performanceId.IsNotNull())
            {
                search = @$" and s.""Id""='{performanceId}' ";

            }
            if (userId.IsNotNull())
            {
                Ownersearch = @$" and  s.""OwnerUserId""='{userId}' ";

            }
            var query = $@"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,pd.""Name"" as ""ProjectName"",pd.""Name"" as ""ServiceStage"",pd.""Id"" as ""StageId"" , s.""TemplateCode"" as TemplateCode
                          FROM public.""NtsService"" as s
                         left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					  where 1=1 #SEARCH# #OWNERSEARCH#  and tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",case when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"",case when pg.""Id"" is not null then pg.""StageId"" when pc.""Id"" is not null then pc.""StageId"" when pds.""Id"" is not null then pds.""Id"" end as ""StageId"", s.""TemplateCode"" as TemplateCode 
FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                           left join (select g.*,pdn.""Id"" as ""StageId"" from cms.""N_PerformanceDocument_PMSGoal"" as g
									  join public.""NtsService"" as gns on gns.""Id""=gns.""UdfNoteTableId"" and gns.""IsDeleted""=false
									  join public.""NtsService"" as pds on pds.""Id""=gns.""ParentServiceId"" and pds.""IsDeleted""=false
									   --join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pds.""UdfNoteId""=pd.""NtsNoteId""  and pd.""IsDeleted""=false
									    join public.""NtsService"" as pdss on pds.""Id""=pdss.""ParentServiceId"" and pdss.""IsDeleted""=false
									  join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pdn on pdss.""UdfNoteId""=pdn.""NtsNoteId"" and pdn.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pdn.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
									  where  pdn.""Id""='{stageId}' and  g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}' and (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   
									  
						   ) as pg on pg.""Id""=s.""UdfNoteTableId""
					   left join (select g.*,pdn.""Id"" as ""StageId"" from cms.""N_PerformanceDocument_PMSCompentency"" as g
									  join public.""NtsService"" as gns on gns.""Id""=gns.""UdfNoteTableId"" and gns.""IsDeleted""=false
									  join public.""NtsService"" as pds on pds.""Id""=gns.""ParentServiceId"" and pds.""IsDeleted""=false
									  
									    join public.""NtsService"" as pdss on pds.""Id""=pdss.""ParentServiceId"" and pdss.""IsDeleted""=false
									  join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pdn on pdss.""UdfNoteId""=pdn.""NtsNoteId"" and pdn.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pdn.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
									  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
									  where pdn.""Id""='{stageId}'  and g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}' and (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate"" 
									  
						   ) as pc on pc.""Id""=s.""UdfNoteTableId""
                             --left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
    left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'  and pds.""Id""='{stageId}' 
                       join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
where 1=1 #OWNERSEARCH#  and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Code"",""TemplateId"",""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level,""TemplateCode"" from NtsService where ""StageId""='{stageId}'


                 union all

                 select tt.""Code"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level,t.""TemplateCode"" as TemplateCode
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""Template"" as tt on tt.""Id"" =nt.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'
	                    where 1=1 and t.""IsDeleted""=false and t.""CompanyId""='5e44cd63-68f0-41f2-b708-0eb3bf9f4a72'   
                    )
                 SELECT * from Nts where Level='Child'";
            //           var query = @$"
            //                          WITH RECURSIVE Nts AS(

            //                WITH RECURSIVE NtsService AS(
            //                SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
            //                      ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
            //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
            //                        join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //				 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //				 where 1=1 #SEARCH# #OWNERSEARCH# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
            //                       union all
            //                       SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
            //                       ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
            //                       join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

            //                    join public.""User"" as u on s.""OwnerUserId""=u.""Id""and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                        join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                           join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //where 1=1  #OWNERSEARCH#
            //                )
            //                SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


            //                union all

            //                select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
            //                    FROM  public.""NtsTask"" as t
            //                       join Nts as nt on t.""ParentServiceId"" =nt.""Id"" 
            //                       join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                       join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            //                       where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'

            //                   )
            //                SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            query = query.Replace("#OWNERSEARCH#", Ownersearch);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();


            var data = new List<ProjectGanttTaskViewModel>();
            var serviceDetails = await _serviceBusiness.GetSingleById(stageId);
            if (serviceDetails.IsNotNull())
            {
                var udfNoteTableId = serviceDetails.UdfNoteTableId;
                if (udfNoteTableId.IsNotNullAndNotEmpty())
                {
                    //var a = await GetTaskListByType("", performanceId, userId, udfNoteTableId);
                    //var b = await GetStageTaskList("", performanceId, userId, stageId);
                    //data.AddRange(a);
                    //data.AddRange(b);
                }
            }

            var list1 =  queryData.GroupBy(x => x.UserName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<IList<ProjectDashboardChartViewModel>> ReadPerformanceStageChartData(string userId, string projectId)
        {
            var search = @$" tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}'";

            if (projectId.IsNotNull())
            {
                search = @$" s.""Id""='{projectId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                          where #SEARCH# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.ServiceStage).Select(group => new { Value = group.Count(), Type = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()), Id = group.Select(x => x.ParentId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;

        }

        public async Task<IList<IdNameViewModel>> GetPerformanceStageIdNameList(string userId, string projectId)
        {
            var search = @$" tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}'";

            if (projectId.IsNotNull())
            {
                search = @$" s.""Id""='{projectId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where #SEARCH# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                       where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT * from Nts where Level='Parent'";
            query = query.Replace("#SEARCH#", search);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.ServiceStage).Select(group => new IdNameViewModel { Id = group.Select(x => x.Id).FirstOrDefault(), Name = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()) }).ToList();
            return list;

        }
        public async Task<IList<IdNameViewModel>> GetPerformanceObjectiveList(string userId, string projectId)
        {
            //var search = @$" tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}'";
            var search = "";
            var Ownersearch = "";
            if (projectId.IsNotNull())
            {
                search = @$" and s.""Id""='{projectId}' ";

            }
            if (userId.IsNotNull())
            {
                Ownersearch = @$" and s.""OwnerUserId""='{userId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",case when pd.""Id"" is not null then pd.""Name"" when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"", tt.""Code"" as ""Type"" FROM public.""NtsService"" as s
                             left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_PerformanceDocument_PMSGoal"" as pg on pg.""Id""=s.""UdfNoteTableId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_userContext.CompanyId}'
                             left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
                             left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where 1=1 #SEARCH# #OWNERSEARCH# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",case when pd.""Id"" is not null then pd.""Name"" when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"",tt1.""Code"" as ""Type"" FROM public.""NtsService"" as s
                                   left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
                        left join cms.""N_PerformanceDocument_PMSGoal"" as pg on pg.""Id""=s.""UdfNoteTableId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_userContext.CompanyId}'
                             left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
                           left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                        join public.""Template"" as tt1 on tt1.""Id"" =s.""TemplateId"" and  tt1.""IsDeleted""=false and tt1.""CompanyId""='{_userContext.CompanyId}'
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                     where 1=1 #OWNERSEARCH# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""OwnerUserId"",""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level 
                    from NtsService
                   -- where 1=1 #OwnerWhere#

                
	                             
                    )
                 SELECT * from Nts where Level='Parent'";
            query = query.Replace("#SEARCH#", search);
            query = query.Replace("#OWNERSEARCH#", Ownersearch);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.ServiceStage).Select(group => new IdNameViewModel { Id = group.Select(x => x.Id).FirstOrDefault(), Name = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()) }).ToList();
            return list;

        }
        public async Task<IList<IdNameViewModel>> GetPerformanceStageIdNameList(string userId, string projectId, List<string> ownerIds = null, List<string> types = null)
        {
            var search = @$" tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""OwnerUserId""='{userId}'";

            if (projectId.IsNotNull())
            {
                search = @$" s.""Id""='{projectId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"",tt.""Code"" as ""Type"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where #SEARCH# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",tt1.""Code"" as ""Type"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId"" 
                        join public.""Template"" as tt1 on tt1.""Id"" =s.""TemplateId"" and  tt1.""IsDeleted""=false and tt1.""CompanyId""='{_userContext.CompanyId}' 
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                     where 1=1 #TypeWhere# and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""OwnerUserId"",""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level 
                    from NtsService
                    where 1=1 #OwnerWhere# 

                 --union all

                 --select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     --FROM  public.""NtsTask"" as t
                        --join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        --join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        --join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        --join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT * from Nts where Level='Parent'";
            query = query.Replace("#SEARCH#", search);
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" AND ""OwnerUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var typesearch = "";
            if (types.IsNotNull() && types.Count > 0)
            {
                string ptypes = null;
                foreach (var i in types)
                {
                    ptypes += $"'{i}',";
                }
                ptypes = ptypes.Trim(',');
                if (ptypes.IsNotNull())
                {
                    typesearch = @" and tt1.""Code"" in (" + ptypes + ") ";
                }
            }
            query = query.Replace("#TypeWhere#", typesearch);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.ServiceStage).Select(group => new IdNameViewModel { Id = group.Select(x => x.Id).FirstOrDefault(), Name = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()) }).ToList();
            return list;

        }
        public async Task<IList<TaskViewModel>> ReadTaskOverdueData(string projectId)
        {
            var query = @$" select t.*,concat(u.""Name"",' ',u.""Email"") as ""AssigneeDisplayName""
                        from public.""NtsTask"" as t
                        join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on ts.""Id""=t.""TaskStatusId"" and  ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' AND ts.""Code""='TASK_STATUS_OVERDUE' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        ";
            var queryData = await _queryTaskRepo.ExecuteQueryList(query, null);
            return queryData;

        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectSubTaskViewData(string taskId, string id, string status)
        {
            var query = "";
            //var parenttaskid = "";
            if (id == null)
            {
                query = @$"
                 select t.""Id"" as TaskId,t.""TemplateCode"" as Code,s.""TemplateCode"" as TemplateCode,t.""TaskSubject"" as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority,
                l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,count(st) as SubTaskCount,null as parentId,s.""OwnerUserId"" as ProjectOwnerId,
				case when count(st)=0 then false else true end as HasSubFolders,l.""Code""  as TaskStatusCode,s.""Id"" as ProjectId
               FROM public.""NtsTask"" as t
             join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
              left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
               left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and  l.""IsDeleted""=false and l.""CompanyId""='{_userContext.CompanyId}'
               left join public.""NtsTask"" as st on st.""ParentTaskId""=t.""Id"" and  st.""IsDeleted""=false and st.""CompanyId""='{_userContext.CompanyId}'
				 where t.""Id""='{taskId}' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
				 group by t.""Id"",t.""TaskSubject"" ,t.""StartDate"" ,t.""DueDate"",u.""Name"",t.""TaskPriorityId"",
                l.""Name"",u.""PhotoId"",s.""OwnerUserId"",s.""TemplateCode"",l.""Code"",s.""Id"",t.""TemplateCode"" ";
            }
            else
            {
                query = @$"
                 select t.""Id"" as TaskId,t.""TemplateCode"" as Code,s.""TemplateCode"" as TemplateCode,t.""TaskSubject"" as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority,
                l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,count(st) as SubTaskCount,'{id}' as parentId,s.""OwnerUserId"" as ProjectOwnerId,
				case when count(st)=0 then false else true end as HasSubFolders,s.""Id"" as ProjectId,l.""Code""  as TaskStatusCode
               FROM public.""NtsTask"" as t
             join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
              left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
               left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and  l.""IsDeleted""=false and l.""CompanyId""='{_userContext.CompanyId}'
               left join public.""NtsTask"" as st on st.""ParentTaskId""=t.""Id"" and  st.""IsDeleted""=false and st.""CompanyId""='{_userContext.CompanyId}'
				 where t.""ParentTaskId""='{id}' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
				 group by t.""Id"",t.""TaskSubject"" ,t.""StartDate"" ,t.""DueDate"",u.""Name"",t.""TaskPriorityId"",
                l.""Name"",u.""PhotoId"",s.""OwnerUserId"",s.""Id"",s.""TemplateCode"",l.""Code"",t.""TemplateCode"" ";

            }



            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            if (status.IsNotNullAndNotEmpty() && id.IsNotNullAndNotEmpty())
            {
                if (status == "Pending")
                {
                    queryData = queryData.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").ToList();
                }
                else if (status == "Completed")
                {
                    queryData = queryData.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").ToList();

                }

            }
            //foreach(var item in queryData)
            //{
            //    var subtask = await _taskBusiness.GetList(x => x.ParentTaskId == item.TaskId);
            //    if (subtask.Count > 0)
            //    {
            //        item.HasSubFolders = true;
            //    }
            //}
            return queryData;
        }



        public async Task<ProgramDashboardViewModel> ReadProjectTotalTaskData(string projectId, string templatecode)
        {

            var query1 = $@"select count(p.""ts"") as AllTaskCount,s.""Id"" as id,s.""DueDate"" as DueDate,s.""OwnerUserId"" as OwnerUserId,s.""RequestedByUserId"" as RequestedByUserId,
  sum(case when lov.""Code"" = 'TASK_STATUS_INPROGRESS' and p.""tsid"" is not null then 1 else 0 end) as ""InProgreessCount"",
  sum(case when lov.""Code"" = 'TASK_STATUS_COMPLETED' and p.""tsid"" is not null then 1 else 0 end) as ""CompletedCount""
  
  FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
 left join public.""User"" as o on o.""Id"" =s.""OwnerUserId"" and  o.""IsDeleted""=false and o.""CompanyId""='{_userContext.CompanyId}'

                     left join public.""User"" as r on r.""Id"" =s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
 where s.""TemplateCode""='{templatecode}' and s.""OwnerUserId""='{_repo.UserContext.UserId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""num"",""Id"",""OwnerUserId"",""RequestedByUserId"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid
                        FROM public.""NtsTask"" as t
                        --join NtsService ns on s.""ParentServiceId""=ns.""Id""
                       -- join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
	                    --left join public.""User"" as u on u.""Id"" =t.""AssignedToUserId""  
                         where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
				)p on p.pid=t.""Id""
				left join public.""LOV"" as lov on lov.""Id"" = p.""tsid"" and  lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
				where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
               group by s.""Id"",s.""DueDate"",s.""OwnerUserId"",s.""RequestedByUserId""
";
            var queryData1 = await _queryPDRepo.ExecuteQuerySingle(query1, null);
            return queryData1;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var query = @$"
                 select count(st) as SubTaskCount,'{projectId}' as ProjectId,t.""TaskStatusId"" as TaskStatusId,l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,u.""Id"" as UserId,u.""Name"" as UserName,t.""Id"" as TaskId,SUBSTRING (t.""TaskSubject"", 1, 30)  as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority
               FROM public.""NtsTask"" as t
                 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                    left join public.""NtsTask"" as st on st.""ParentTaskId""=t.""Id"" and  st.""IsDeleted""=false and st.""CompanyId""='{_userContext.CompanyId}'
                      left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and  l.""IsDeleted""=false and l.""CompanyId""='{_userContext.CompanyId}'
                 where t.""ParentServiceId""='{projectId}' and t.""TemplateCode""='PMS_GOAL_ADHOC_TASK'  #AssigneeUser# #StatusWhere# #OwnerUser# #DateWhere#
                and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
   group by t.""Id"",t.""TaskSubject"",t.""StartDate"",t.""DueDate"",u.""Name"",t.""TaskPriorityId"",u.""PhotoId"",
   u.""Id"",u.""Name"",l.""Name""

               
";
            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    assigneeSerach = @"and t.""AssignedToUserId"" in (" + aids + ") ";
                }
            }
            query = query.Replace("#AssigneeUser#", assigneeSerach);
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string aids = null;
                foreach (var i in ownerIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    ownerSerach = @"and t.""OwnerUserId"" in (" + aids + ") ";
                }
            }
            query = query.Replace("#OwnerUser#", ownerSerach);
            var statusSerach = "";
            if (statusIds.IsNotNull() && statusIds.Count > 0)
            {
                string sids = null;
                foreach (var i in statusIds)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (statusIds.IsNotNull() && statusIds.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }
            query = query.Replace("#DateWhere#", dateSerach);
            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceTaskViewData(string projectId, string performanceUser, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null)
        {
            var query = @$"             
            select st.""SubTaskCount"" as SubTaskCount,t.""TaskType"" as TaskType,
st.""Pending"" as PendingCount,
 st.""Complete"" as TotalCompletedCount,
nts.""TemplateCode"" as TemplateCode,tr.""Code"" as Code,'{projectId}' as ProjectId,t.""TaskStatusId"" as TaskStatusId,l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,
u.""Id"" as UserId,o.""Id"" as ProjectOwnerId,r.""Id"" as RequestedByUserId,u.""Name"" as UserName,t.""Id"" as TaskId,SUBSTRING (t.""TaskSubject"", 1, 30)  as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority,l.""Code"" as TaskStatusCode
               FROM public.""NtsTask"" as t
 left join public.""Template"" as tr on tr.""Id""=t.""TemplateId"" and  tr.""IsDeleted""=false and tr.""CompanyId""='{_userContext.CompanyId}'
                 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
 left join public.""User"" as o on o.""Id""=t.""OwnerUserId"" and  o.""IsDeleted""=false and o.""CompanyId""='{_userContext.CompanyId}'
 left join public.""User"" as r on r.""Id""=t.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
                    left join(
			with recursive task as(
            select nt.""TaskSubject"" as TaskSubject,nt.""Id"",nt.""Id"" as ""TaskId"",tl.""Code"",nt.""TaskStatusId"",'parent' as type
            from public.""NtsTask"" as nt 
	         left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and  tl.""IsDeleted""=false and tl.""CompanyId""='{_userContext.CompanyId}'
			where nt.""ParentServiceId""='{projectId}' and nt.""ParentTaskId"" is null and  nt.""IsDeleted""=false and nt.""CompanyId""='{_userContext.CompanyId}'
            union all
            select nt.""TaskSubject"" as TaskSubject,nt.""Id"",em.""TaskId"",tl.""Code"",nt.""TaskStatusId"" ,'child' as type
            from public.""NtsTask"" as nt 
            join task as em on em.""Id""=nt.""ParentTaskId""    
	        left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId""	and  tl.""IsDeleted""=false and tl.""CompanyId""='{_userContext.CompanyId}'				
            )select count(""Id"") as ""SubTaskCount"",
			sum(case when ""Code""='TASK_STATUS_INPROGRESS'  then 1 else 0 end) as ""Pending"",
			sum(case when ""Code""='TASK_STATUS_COMPLETE' then 1 else 0 end) as ""Complete"",""TaskId""
						  from task
						  where type='child'
					group by ""TaskId""
					
					)as st on st.""TaskId""=t.""Id""
                      left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and  l.""IsDeleted""=false and l.""CompanyId""='{_userContext.CompanyId}'
                    left join public.""NtsService"" as nts on nts.""Id""=t.""ParentServiceId"" and  nts.""IsDeleted""=false and nts.""CompanyId""='{_userContext.CompanyId}'
   join public.""Template"" as tt on tt.""Id""=nts.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                 where t.""ParentServiceId""='{projectId}' and t.""ParentTaskId"" is null and nts.""OwnerUserId""='{performanceUser}' #AssigneeUser#  #OwnerUser# #StatusWhere# #DateWhere#
           and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
   group by t.""Id"",t.""TaskSubject"",t.""StartDate"",t.""DueDate"",u.""Name"",t.""TaskPriorityId"",u.""PhotoId"",
   u.""Id"",u.""Name"",l.""Name"",tr.""Code"",o.""Id"",r.""Id"",l.""Code"",nts.""TemplateCode"",st.""SubTaskCount"",st.""Pending"",st.""Complete""




";




            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $",'{i}'";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    assigneeSerach = @"and t.""AssignedToUserId"" in (" + aids + ") ";

                }
            }
            query = query.Replace("#AssigneeUser#", assigneeSerach);
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string aids = null;
                foreach (var i in ownerIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    ownerSerach = @"and t.""OwnerUserId"" in (" + aids + ") ";
                }
            }
            query = query.Replace("#OwnerUser#", ownerSerach);
            var statusSerach = "";
            if (statusIds.IsNotNull() && statusIds.Count > 0)
            {
                string sids = null;
                foreach (var i in statusIds)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (statusIds.IsNotNull() && statusIds.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }
            query = query.Replace("#DateWhere#", dateSerach);
            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }



        public async Task<IList<ServiceViewModel>> ReadPerformanceDocumentStagesData(string performanceId, string stageId = null)
        {
            var query = @$"
               select s.""Id"",s.""ServiceSubject"" as ServiceSubject,udf.""StartDate"" as StartDate ,udf.""EndDate"" as DueDate,
              sp.""Name"" as ""Priority"",udf.""DocumentStageStatus"" as ""PerformanceDocumentStatus"",ss.""Code"" as ServiceStatusCode,u.""Name"" as OwnerDisplayName 
                              		 FROM public.""NtsService"" as s 
									 join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""Code""<>'SERVICE_STATUS_DRAFT' and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as udf on udf.""NtsNoteId"" = s.""UdfNoteId"" and  udf.""IsDeleted""=false and udf.""CompanyId""='{_userContext.CompanyId}'
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId""  and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'      
                where s.""ParentServiceId""='{performanceId}' #STAGE# and s.""TemplateCode""='PMS_PERFORMANCE_DOCUMENT_STAGE' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' ";

            var stageFilter = "";
            if (stageId.IsNotNullAndNotEmpty())
            {
                stageFilter = $@" and s.""Id""='{stageId}'";
            }
            query = query.Replace("#STAGE#", stageFilter);
            var queryData = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<ServiceViewModel>> GetPerformanceDocumentStageDataByServiceId(string performanceServiceId, string ownerUserId, string stageId = null)
        {
            var query = @$"
               select distinct pms.""Id"" as StageId ,pds.""Name"" as StageName,ps.""Id"",r.""Name"" as OwnerDisplayName
from public.""NtsService"" as s
join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on s.""UdfNoteId"" = pd.""NtsNoteId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}' 
 join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pd.""DocumentMasterId""=pdm.""Id"" and pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'
 join  public.""NtsNote"" n on n.""ParentNoteId""=pdm.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}' 
 join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pds on n.""Id""=pds.""NtsNoteId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'
 join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pms on pms.""DocumentMasterStageId""=pds.""Id"" and pms.""IsDeleted""=false and pms.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as ps on ps.""UdfNoteId""=pms.""NtsNoteId"" and  ps.""IsDeleted""=false and ps.""CompanyId""='{_userContext.CompanyId}'
join public.""User"" as r on r.""Id"" = ps.""OwnerUserId""  and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}' 
where   s.""IsDeleted""=false and ps.""OwnerUserId""='{ownerUserId}' 
and s.""CompanyId""='{_userContext.CompanyId}' and s.""Id""='{performanceServiceId}' #STAGE#";

            var stageFilter = "";
            if (stageId.IsNotNullAndNotEmpty())
            {
                stageFilter = $@" and pms.""Id""='{stageId}'";
            }
            query = query.Replace("#STAGE#", stageFilter);
            var queryData = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> GetAllApprovedGoalForManager(string performanceId)
        {
            var query = @$"
               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""GoalName"" as StageName,s.""TemplateCode"" as TemplateCode,
              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""GoalStartDate"" as StartDate,pg.""GoalEndDate"" as DueDate,s.""ParentServiceId"",r.""Id"" as RequestedByUserId,pg.""SuccessCriteria"" as SuccessCriteria,pg.""EmployeeRating"",pg.""EmployeeComments"" as EmployeeComment,pg.""ManagerRating"" as ManagerRating
,pg.""ManagerComments"" as ManagerComment
                              		 FROM public.""NtsService"" as s 
                          join cms.""N_PerformanceDocument_PMSGoal"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'        
                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_GOAL' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' and ss.""Code""='SERVICE_STATUS_COMPLETE'";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerPerformanceGoalViewData(string performanceId, string stageId, string userId)
        {
            //            var oldquery = @$"
            //               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""GoalName"" as StageName,s.""TemplateCode"" as TemplateCode,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""GoalStartDate"" as StartDate,pg.""GoalEndDate"" as DueDate,s.""ParentServiceId"",r.""Id"" as RequestedByUserId,pg.""SuccessCriteria"" as SuccessCriteria,pg.""EmployeeRating"",pg.""EmployeeComments"" as EmployeeComment,pg.""ManagerRating"" as ManagerRating
            //,pg.""ManagerComments"" as ManagerComment
            //                              		 FROM public.""NtsService"" as s 
            //                          join cms.""N_PerformanceDocument_PMSGoal"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'        
            //                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_GOAL' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
            //and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";

            var query1 = $@"
            select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""GoalName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",ss.""Code"" as ServiceStatusCode
            ,g.""GoalStartDate"" as ""StartDate"",g.""GoalEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,g.""ManagerComments"" as ManagerComments
            ,g.""EmployeeRating"",g.""EmployeeComments"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'

";

            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query1, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadGoalServiceData(string performanceId)
        {
            var query = @$"
               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,s.""ServiceSubject"" as StageName,s.""TemplateCode"" as TemplateCode,
              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",s.""StartDate"",s.""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId,g.""SuccessCriteria"" as SuccessCriteria,g.""EmployeeRating"",g.""EmployeeComments"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
,g.""ManagerComments"" as ManagerComment
                              		 FROM public.""NtsService"" as s 
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'   
left join cms.""N_PerformanceDocument_PMSGoal"" as g on g.""Id"" = s.""UdfNoteTableId"" and  g.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'   
                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_GOAL' 
and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceCompetencyStageViewData(string performanceId, string stageId, string userId)
        {
            //            var query = @$"
            //               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""CompetencyName"" as StageName,s.""TemplateCode"" as TemplateCode,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""CompetencyStartDate"" as ""StartDate"",pg.""CompetencyEndDate"" as ""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId,pg.""EmployeeRating"",pg.""EmployeeComment"" as EmployeeComment,pg.""ManagerRating"" as ManagerRating
            //,pg.""ManagerComment"" as ManagerComment
            //                              		 FROM public.""NtsService"" as s 
            // join cms.""N_PerformanceDocument_PMSCompentency"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
            //                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_COMPENTENCY' and s.""OwnerUserId""='{_repo.UserContext.UserId}' 
            //           and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";

            var query1 = $@"
select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""CompetencyName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",ss.""Code"" as ServiceStatusCode
            ,g.""CompetencyStartDate"" as ""StartDate"",g.""CompetencyEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,g.""ManagerComment"" as ManagerComments
            ,g.""EmployeeRating"",g.""EmployeeComment"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'

";

            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query1, null);
            return queryData;
        }


        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceCompetencyServiceData(string performanceId)
        {
            var query = @$"
               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,s.""ServiceSubject"" as StageName,s.""TemplateCode"" as TemplateCode,
              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",s.""StartDate"",s.""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId,g.""EmployeeRating"",g.""EmployeeComment"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
,g.""ManagerComment"" as ManagerComment
                              		 FROM public.""NtsService"" as s 
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
               left join cms.""N_PerformanceDocument_PMSCompentency"" as g on g.""Id"" = s.""UdfNoteTableId"" and  g.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'  
where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_COMPENTENCY' 
           and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> GetAllApprovedCompetenciesForManager(string performanceId)
        {
            var query = @$"
               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,s.""ServiceSubject"" as StageName,s.""TemplateCode"" as TemplateCode,
              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",s.""StartDate"",s.""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId,g.""EmployeeRating"",g.""EmployeeComment"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
,g.""ManagerComment"" as ManagerComment
                              		 FROM public.""NtsService"" as s 
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
               left join cms.""N_PerformanceDocument_PMSCompentency"" as g on g.""Id"" = s.""UdfNoteTableId"" and  g.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'  
where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_COMPENTENCY' and ss.""Code""='SERVICE_STATUS_COMPLETE' 
           and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceDevelopmentViewData(string performanceId, string stageId, string userId)
        {
            //            var query = @$"
            //               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""DevelopmentName"" as StageName,s.""TemplateCode"" as TemplateCode,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""DevelopmentStartDate"" as ""StartDate"",pg.""DevelopmentEndDate"" as ""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId
            //                              		 FROM public.""NtsService"" as s 
            //                                     join cms.""N_PerformanceDocument_PMSDevelopment"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
            //                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_DEVELOPMENT' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
            //and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";

            var query1 = $@"
select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""DevelopmentName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",ss.""Code"" as ServiceStatusCode
            ,g.""DevelopmentStartDate"" as ""StartDate"",g.""DevelopmentEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription""
           
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_DEVELOPMENT' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSDevelopment"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""DevelopmentStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""DevelopmentEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'

";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query1, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceAllData(string performanceId, string stageId, string userId)
        {
            //            var query = @$"
            //                select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""GoalName"" as StageName,s.""TemplateCode"" as TemplateCode,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""GoalStartDate"" as StartDate,pg.""GoalEndDate"" as DueDate,s.""ParentServiceId"",r.""Id"" as RequestedByUserId
            //                              		 FROM public.""NtsService"" as s 
            //                          join cms.""N_PerformanceDocument_PMSGoal"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'        
            //                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_GOAL' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
            //and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
            //union
            //  select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""CompetencyName"" as StageName,s.""TemplateCode"" as TemplateCode,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""CompetencyStartDate"" as ""StartDate"",pg.""CompetencyEndDate"" as ""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId
            //                              		 FROM public.""NtsService"" as s 
            // join cms.""N_PerformanceDocument_PMSCompentency"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
            //                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_COMPENTENCY' and s.""OwnerUserId""='{_repo.UserContext.UserId}' 
            //           and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
            //union

            //   select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""DevelopmentName"" as StageName,s.""TemplateCode"" as TemplateCode,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""DevelopmentStartDate"" as ""StartDate"",pg.""DevelopmentEndDate"" as ""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId
            //                              		 FROM public.""NtsService"" as s 
            //                                     join cms.""N_PerformanceDocument_PMSDevelopment"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
            //                where s.""ParentServiceId""='{performanceId}' and s.""TemplateCode""='PMS_DEVELOPMENT' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
            //and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'



            //";

            var query = $@"
select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""GoalName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Code"" as ServiceStatusCode
            ,g.""GoalStartDate"" as ""StartDate"",g.""GoalEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,g.""ManagerComments"" as ManagerComments
            ,g.""EmployeeRating"",g.""EmployeeComments"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'

union

select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""CompetencyName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Code"" as ServiceStatusCode
            ,g.""CompetencyStartDate"" as ""StartDate"",g.""CompetencyEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,g.""ManagerComment"" as ManagerComments
            ,g.""EmployeeRating"",g.""EmployeeComment"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'

union

select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""DevelopmentName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ss.""Code"" as ServiceStatusCode
            ,g.""DevelopmentStartDate"" as ""StartDate"",g.""DevelopmentEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            null as Weightage,null as SuccessCriteria,null as ManagerComments
            ,null,null as EmployeeComment,null as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_DEVELOPMENT' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSDevelopment"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""DevelopmentStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""DevelopmentEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'


";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectStageViewData(string projectId)
        {
            var query = @$"
                select t.""Id"" as Id,t.""ProjectOwnerId"" as ProjectOwnerId,t.""OwnerName"" as ProjectOwnerName,t.""ServiceSubject"" as StageName,(t.""num""*44)-44 as Level FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                       left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT  ""OwnerName"",""ProjectOwnerId"",""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
                left join public.""NtsTask"" as nt on nt.""ParentServiceId""=t.""Id"" and  nt.""IsDeleted""=false and nt.""CompanyId""='{_userContext.CompanyId}'				 
                where s.""Id""='{projectId}' and nt.""AssignedToUserId""='{_repo.UserContext.UserId}' and s.""TemplateCode""='PMS_GOAL' 
                where  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                
                union
                
                 select t.""Id"" as Id,t.""ProjectOwnerId"" as ProjectOwnerId,t.""OwnerName"" as ProjectOwnerName,t.""ServiceSubject"" as StageName,(t.""num""*44)-44 as Level FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                       left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                       where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT  ""OwnerName"",""ProjectOwnerId"",""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
               		 
                where s.""Id""='{projectId}' and s.""OwnerUserId""='{_repo.UserContext.UserId}' and s.""TemplateCode""='PMS_GOAL' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'


";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        //        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedDataOld(string projectId)
        //        {
        //            var query = $@"select  s.""Id"" as id,
        //                 sum(t.""Count"")::integer as UserTaskCount,
        //                s.""Id"" as ProjectId,t.""pid"" as PhotoId,t.""username"" as UserName
        //                FROM public.""NtsService"" as s
        //                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //                  left join( 

        //	          select t.""PCount"" as ""Count"",s.""Id"",t.""Ppid"" as ""pid"",t.""Pusername"" as username from public.""NtsService"" as s
        //join(
        //SELECT count(t.""TaskSubject"") as ""PCount"", s.""Id"", u.""PhotoId"" as ""Ppid"", u.""Name"" as ""Pusername""
        //                        FROM public.""NtsService"" as s
        //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //						join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //						left join public.""User"" as u on u.""Id"" =t.""AssignedToUserId""
        //					where tt.""Code""='PROJECT_SUPER_SERVICE'
        //						group by s.""Id"", u.""PhotoId"", u.""Name"")t on t.""Id""=s.""Id""
        //						where s.""Id"" = '{projectId}' 
        //union all
        //select t.""Count"" as ""Count"", s.""Id"", t.""pid"" as ""pid"", t.""username"" as username from public.""NtsService"" as s
        //   join(
        //      WITH RECURSIVE NtsService AS (
        //                    SELECT s.""ServiceSubject"" as ts, s.""Id"", s.""Id"" as ""ServiceId"" , u.""PhotoId"" as pid, u.""Name"" as username,'Parent' as Type
        //                        FROM public.""NtsService"" as s
        //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //						--left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //						left join public.""User"" as u on u.""Id"" =s.""OwnerUserId""

        //                        union all
        //                        SELECT t.""TaskSubject"" as ts, s.""Id"", ns.""ServiceId"" as ""ServiceId"" , u.""PhotoId"" as pid, u.""Name"" as username,'Child' as Type
        //                        FROM public.""NtsService"" as s
        //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
        //                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //	                    left join public.""User"" as u on u.""Id"" =t.""AssignedToUserId""     

        //                    )
        //                    SELECT count(distinct ts) as ""Count"",""ServiceId"",""pid"",""username"" from NtsService
        // where Type!='Parent'
        //group by ""ServiceId"",""pid"",""username""

        //                ) t on s.""Id""=t.""ServiceId""
        //						where s.""Id"" = '{projectId}' 



        //                ) t on s.""Id""=t.""Id""

        //                Where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""Id"" = '{projectId}' 
        //               GROUP BY s.""Id"", t.""pid"",t.""username"" ";


        //            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
        //            return queryData;
        //        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskAssignedData(string projectId, string templatecode)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        where s.""TemplateCode""='{templatecode}' and s.""OwnerUserId""='{_repo.UserContext.UserId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid
                        FROM public.""NtsTask"" as t
                        where  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                       
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
				where s.""Id""='{projectId}' 
               group by s.""Id"", s.""DueDate"", u.""Id"" ";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerPerformanceTaskAssignedData(string projectId, string templatecode)
        {
            var query = $@" SELECT count(u) as UserTaskCount,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
                        FROM public.""NtsTask"" as t
						
						join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						
						where s.""ParentServiceId""='{projectId}' and t.""TemplateCode""='{templatecode}' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
						group by  u.""PhotoId"" ,u.""Name"" ,u.""Id""
                ";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedData(string projectId, string templatecode)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
                        where s.""Id""='{projectId}' 
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
                          where s.""TemplateCode""='{templatecode}'
                 )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid
                        FROM public.""NtsTask"" as t
                       
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid""
				where s.""Id""='{projectId}' and p.""auid""='{_repo.UserContext.UserId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

                union

                select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
                        where s.""Id""='{projectId}' 
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
                         where s.""TemplateCode""='{templatecode}'
                 )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"",""OwnerUserId"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid
                        FROM public.""NtsTask"" as t
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid""
				where s.""Id""='{projectId}' and t.""OwnerUserId""='{_repo.UserContext.UserId}' and p.""auid""<>'{_repo.UserContext.UserId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskOwnerData(string projectId, string templatecode)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         where s.""TemplateCode""='{templatecode}' and s.""OwnerUserId""='{_repo.UserContext.UserId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""OwnerUserId"" as auid
                        FROM public.""NtsTask"" as t
                   where  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
				where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
               group by s.""Id"", s.""DueDate"", u.""Id"" ";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskOwnerData(string projectId, string templatecode)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        where s.""TemplateCode""='{templatecode}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid ,t.""OwnerUserId"" as ouid
                        FROM public.""NtsTask"" as t
                and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""ouid"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
				where s.""Id""='{projectId}' and p.""auid""='{_repo.UserContext.UserId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

                union

                select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                          where s.""TemplateCode""='{templatecode}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"",""OwnerUserId"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid ,t.""OwnerUserId"" as ouid
                        FROM public.""NtsTask"" as t
                    where  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""ouid"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
				where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' and t.""OwnerUserId""='{_repo.UserContext.UserId}' and p.""auid""<>'{_repo.UserContext.UserId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<DashboardCalendarViewModel>> ReadPerformanceCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string performanceStageId = null)
        {
            var query = @$"
                  WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""TemplateCode"" as temp,  pd.""Name"" as ""UdfName"", pd.""StartDate""::TIMESTAMP::DATE as ""ServiceStart"", pd.""EndDate""::TIMESTAMP::DATE as ""ServiceEnd"",s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                     ,s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId""
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where  tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and  s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",s.""TemplateCode"" as temp, pd.""Name"" as ""UdfName"", pd.""StartDate""::TIMESTAMP::DATE as ""ServiceStart"", pd.""EndDate""::TIMESTAMP::DATE as ""ServiceEnd"",s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""UdfName""FROM public.""NtsService"" as s
                     left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId""
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                 )
                  SELECT ""Id"",""Id"" as ServiceId, ""TemplateCode"",""UdfName"" as ""Title"",""ServiceStart"" as Start,""ServiceEnd"" as End,""OwnerName"" as UserName,""OwnerUserId"" as ""AssigneeUserId"",""OwnerUserId"" as ""OwnerUserId"",""NtsStatus"" as StatusName,""ParentId"" as TaskStatusId,'Parent' as Level from NtsService


                 union all

                 select t.""Id"",t.""TemplateCode"",null as ServiceId,t.""TaskSubject"" as ""Title"", t.""StartDate""::TIMESTAMP::DATE as Start, t.""DueDate""::TIMESTAMP::DATE as End, u.""Name"" as UserName,t.""AssignedToUserId"" as AssigneeUserId,t.""OwnerUserId"" as OwnerUserId,ss.""Name"" as StatusName,t.""TaskStatusId"" as TaskStatusId,'Child' as Level 
                     FROM  public.""NtsTask"" as t
                       	left  join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=t.""UdfNoteTableId""
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id"" 
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                     where t.""AssignedToUserId""='{_repo.UserContext.UserId}' or nt.""OwnerUserId""='{_repo.UserContext.UserId}'  and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                     --#OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere#    
                    )
                 SELECT * from Nts --where Level='Child'

               
";

            var queryData = await _queryDCRepo.ExecuteQueryList<DashboardCalendarViewModel>(query, null);

            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                queryData = queryData.Where(x => ownerIds.Contains(x.OwnerUserId)).ToList();
            }

            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                queryData = queryData.Where(x => assigneeIds.Contains(x.AssigneeUserId)).ToList();
            }

            if (statusIds.IsNotNull() && statusIds.Count > 0)
            {
                queryData = queryData.Where(x => statusIds.Contains(x.TaskStatusId)).ToList();
            }
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (statusIds.IsNotNull() && statusIds.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        queryData = queryData.Where(x => startDate <= x.End.Date && x.End.Date < dueDate).ToList();
                    }
                    else
                    {
                        queryData = queryData.Where(x => startDate <= x.Start.Date && x.Start.Date < dueDate).ToList();
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {

                        queryData = queryData.Where(x => startDate <= x.End.Date && x.End.Date < dueDate).ToList();
                    }
                    else
                    {
                        queryData = queryData.Where(x => startDate <= x.Start.Date && x.Start.Date < dueDate).ToList();
                    }
                }


            }

            //if(performanceStageId.IsNotNullAndNotEmpty())
            //{
            //    queryData = queryData.Where(x => x.ServiceId == performanceStageId).ToList();
            //}




            return queryData;
        }
        //        public async Task<IList<DashboardCalendarViewModel>> ReadManagerPerformanceCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        //        {
        //            var query = @$"
        //                 WITH RECURSIVE Nts AS(

        //                 WITH RECURSIVE NtsService AS(
        //                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
        //                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
        //                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
        //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
        //                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
        //					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id""
        //					 	 where  tt.""Code""='PROJECT_SUPER_SERVICE' and  s.""Id""='{projectId}'
        //                        union all
        //                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
        //                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
        //                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

        //                     join public.""User"" as u on s.""OwnerUserId""=u.""Id""
        //                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""
        //                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""
        //                 )
        //                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""OwnerName"" as UserName,""NtsStatus"" as StatusName,""ParentId"",'Parent' as Level from NtsService


        //                 union all

        //                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End,u.""Name"" as UserName ,ss.""Name"" as StatusName,t.""TaskStatusId"" as TaskStatusId,'Child' as Level 
        //                     FROM  public.""NtsTask"" as t
        //                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
        //                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
        //                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
        //                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
        //                        --join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""
        //	                    #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere#        
        //                    )
        //                 SELECT * from Nts where Level='Child'



        //";
        //            var ownerSerach = "";
        //            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
        //            {
        //                string oids = null;
        //                foreach (var i in ownerIds)
        //                {
        //                    oids += $"'{i}',";
        //                }
        //                oids = oids.Trim(',');
        //                if (oids.IsNotNull())
        //                {
        //                    ownerSerach = @" where t.""OwnerUserId"" in (" + oids + ") ";
        //                }
        //            }
        //            query = query.Replace("#OwnerWhere#", ownerSerach);
        //            var assigneeSerach = "";
        //            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
        //            {
        //                string aids = null;
        //                foreach (var i in assigneeIds)
        //                {
        //                    aids += $"'{i}',";
        //                }
        //                aids = aids.Trim(',');
        //                if (aids.IsNotNull())
        //                {
        //                    if (ownerIds.IsNotNull() && ownerIds.Count > 0)
        //                    {
        //                        assigneeSerach = @" and t.""AssignedToUserId"" in (" + aids + ") ";
        //                    }
        //                    else
        //                    {
        //                        assigneeSerach = @" where t.""AssignedToUserId"" in (" + aids + ") ";
        //                    }

        //                }
        //            }
        //            query = query.Replace("#AssigneeWhere#", assigneeSerach);
        //            var statusSerach = "";
        //            if (statusIds.IsNotNull() && statusIds.Count > 0)
        //            {
        //                string sids = null;
        //                foreach (var i in statusIds)
        //                {
        //                    sids += $"'{i}',";
        //                }
        //                sids = sids.Trim(',');
        //                if (sids.IsNotNull())
        //                {
        //                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0))
        //                    {
        //                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
        //                    }
        //                    else
        //                    {
        //                        statusSerach = @" where t.""TaskStatusId"" in (" + sids + ") ";
        //                    }

        //                }
        //            }
        //            query = query.Replace("#StatusWhere#", statusSerach);
        //            var dateSerach = "";
        //            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
        //            {


        //                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (statusIds.IsNotNull() && statusIds.Count > 0))
        //                {
        //                    if (column == FilterColumnEnum.DueDate)
        //                    {
        //                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
        //                    }
        //                    else
        //                    {
        //                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
        //                    }

        //                }
        //                else
        //                {
        //                    if (column == FilterColumnEnum.DueDate)
        //                    {
        //                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
        //                    }
        //                    else
        //                    {
        //                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
        //                    }
        //                }


        //            }
        //            query = query.Replace("#DateWhere#", dateSerach);
        //            var queryData = await _queryDCRepo.ExecuteQueryList<DashboardCalendarViewModel>(query, null);
        //            return queryData;
        //        }

        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentList(string userId)
        {
            string query = "";

            query = @$"Select s.""Id"" as Id,pdm.""Name"" as MasterName, pd.""DocumentMasterId"" as MasterId
                    ,pdms.""Name"" as MasterStageName,pdms.""Id"" as MasterStageId
,ss.""Name"" as ServiceStatusName,
pd.""Name"" as Name,o.""Id"" as OwnerUserId, o.""Name"" as OwnerUserName,pd.""Year"" as Year,s.""StartDate"" as StartDate, s.""DueDate"" as EndDate,
hj.""JobTitle"" as JobName, hd.""DepartmentName"" as DepartmentName,hp.""PersonNo"" as EmployeeNo,
pd.""CalculatedRating"" as CalculatedRating,pd.""CalculatedRatingRounded"" as CalculatedRatingRounded,pd.""AdjustedRating"" as AdjustedRating,
pd.""AdjustedRatingRounded"" as AdjustedRatingRounded
,pd.""FinalRating"" as FinalRating,
pd.""FinalRatingRounded"" as FinalRatingRounded
from public.""NtsService"" as s
  join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
join public.""User"" as o on o.""Id""=s.""OwnerUserId"" and  o.""IsDeleted""=false and o.""CompanyId""='{_userContext.CompanyId}'
 join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PMS_PERFORMANCE_DOCUMENT' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
  join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id""=pd.""DocumentMasterId"" and  pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'
  join public.""NtsNote"" as pdmnote on pdm.""NtsNoteId""=pdmnote.""ParentNoteId"" and pdmnote.""IsDeleted""=false and pdmnote.""CompanyId""='{_userContext.CompanyId}'
  join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdmnote.""Id"" = pdms.""NtsNoteId"" and pdms.""EnableReview""='True' and pdms.""IsDeleted""=false and pdms.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=o.""Id"" and  hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and  assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and  hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and  hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
                            --where s.""OwnerUserId""='{userId}'
where  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryPerDoc.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<ProjectGanttTaskViewModel>> GetPerformanceDocumentTaskList()
        {
            string query = "";

            query = @$"WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT pd.""Name"" as ""MasterName"",s.""ServiceNo"" as ""TaskNo"",
hj.""JobTitle"" as ""JobName"", hd.""DepartmentName"" as ""DepartmentName"",hp.""PersonNo"" as ""EmployeeNo""
,tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Name"" as ""TaskOwnerName""
,u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,pd.""Name"" as ""ProjectName"",pd.""Name"" as ""ServiceStage"" 
                          FROM public.""NtsService"" as s
  join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and  pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'

                       join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id"" = pd.""DocumentMasterId"" and  pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and  hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and  assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and  hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and  hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
					  where   tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT ns.""MasterName"",s.""ServiceNo"" as ""TaskNo"",
ns.""JobName"",ns.""DepartmentName"",ns.""EmployeeNo""
,tt.""Code"",s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",u.""Name"" as ""TaskOwnerName"",
u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",case when pg.""Id"" is not null then pg.""GoalName"" when pc.""Id"" is not null then pc.""CompetencyName"" when pds.""Id"" is not null then pds.""Name"" end as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                        left join cms.""N_PerformanceDocument_PMSGoal"" as pg on pg.""Id""=s.""UdfNoteTableId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_userContext.CompanyId}'
                             left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=s.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
    left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""Id""=s.""UdfNoteTableId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'
    join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'

                 )
                 SELECT ""MasterName"",""TaskNo"",""JobName"",""DepartmentName"",""EmployeeNo"",""Code"",""TemplateId"",""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"", ""TaskOwnerName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select nt.""MasterName"",t.""TaskNo"" as ""TaskNo"",
nt.""JobName"",nt.""DepartmentName"",nt.""EmployeeNo""
,tt.""Code"",tt.""Id"" as TemplateId,t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", nt.""OwnerName"" as ""OwnerName"",ou.""Name"" as ""TaskOwnerName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
join public.""Template"" as tt on tt.""Id"" =nt.""TemplateId"" and  tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and  ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
	                    where 1=1  and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                    )
                 SELECT * from Nts where Level='Child'";

            var queryData = await _queryGantt.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetTaskListByType(string templateCodes, string pdmId = null, string ownerId = null, string stageId = null)
        {
            //   var query = $@"with recursive task as(
            //   select nt.""Id"",t.""Code"" as TemplateCode,tl.""Code"" as NtsStatusCode,nt.""TaskStatusId""
            //   from public.""NtsService"" as s
            //   join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code"" in ('{templateCodes.Replace(",", "','")}')
            //   join public.""NtsTask"" as nt on s.""Id""=nt.""ParentServiceId"" and nt.""ParentTaskId"" is null and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            //   left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
            //   where s.""IsDeleted""=false #OwnerWhere# #WHERE#
            //   union all
            //   select nt.""Id"", nt.""TemplateCode"", tl.""Code"" as NtsStatusCode, nt.""TaskStatusId""
            //   from public.""NtsTask"" as nt
            //   join task as em on em.""Id""=nt.""ParentTaskId""    
            //left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false)
            //   select * from task";

            //   var ownerwhere = "";
            //   if (ownerId.IsNotNullAndNotEmpty())
            //   {
            //       ownerwhere = $@" and s.""OwnerUserId""='{ownerId}'";
            //   }
            //   else
            //   {
            //       ownerwhere = $@" and s.""OwnerUserId""='{_repo.UserContext.UserId}'";
            //   }
            //   query = query.Replace("#OwnerWhere#", ownerwhere);

            //   var where = "";
            //   if (pdmId.IsNotNullAndNotEmpty())
            //   {
            //       where = $@" and s.""ParentServiceId""='{pdmId}'";
            //   }
            //   query = query.Replace("#WHERE#", where);

            var queryG = $@"with recursive task as(
               select nt.""Id"",nt.""TemplateCode"" as TemplateCode,tl.""Code"" as NtsStatusCode,nt.""TaskStatusId""
               from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
               join public.""NtsTask"" as nt on gns.""Id""=nt.""ParentServiceId"" and nt.""ParentTaskId"" is null and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
               left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
                where pds.""IsDeleted""=false and pdns.""Id""='{pdmId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{ownerId}'
               union all
               select nt.""Id"", nt.""TemplateCode"", tl.""Code"" as NtsStatusCode, nt.""TaskStatusId""
               from public.""NtsTask"" as nt
               join task as em on em.""Id""=nt.""ParentTaskId""    
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false)
               select * from task
              

";
            var queryC = $@"with recursive task as(
               select nt.""Id"",nt.""TemplateCode"" as TemplateCode,tl.""Code"" as NtsStatusCode,nt.""TaskStatusId""
               from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
               join public.""NtsTask"" as nt on gns.""Id""=nt.""ParentServiceId"" and nt.""ParentTaskId"" is null and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
               left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
                where pds.""IsDeleted""=false and pdns.""Id""='{pdmId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{ownerId}'
               union all
               select nt.""Id"", nt.""TemplateCode"", tl.""Code"" as NtsStatusCode, nt.""TaskStatusId""
               from public.""NtsTask"" as nt
               join task as em on em.""Id""=nt.""ParentTaskId""    
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false)
               select * from task
              

";
            var queryD = $@"with recursive task as(
               select nt.""Id"",nt.""TemplateCode"" as TemplateCode,tl.""Code"" as NtsStatusCode,nt.""TaskStatusId""
               from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_DEVELOPMENT' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSDevelopment"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""DevelopmentStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""DevelopmentEndDate""   and g.""IsDeleted""=false
               join public.""NtsTask"" as nt on gns.""Id""=nt.""ParentServiceId"" and nt.""ParentTaskId"" is null and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
               left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
                where pds.""IsDeleted""=false and pdns.""Id""='{pdmId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{ownerId}'
               union all
               select nt.""Id"", nt.""TemplateCode"", tl.""Code"" as NtsStatusCode, nt.""TaskStatusId""
               from public.""NtsTask"" as nt
               join task as em on em.""Id""=nt.""ParentTaskId""    
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false)
               select * from task";
            var queryA = "";
            var queryData = new List<ProjectGanttTaskViewModel>();
            if (templateCodes == "PMS_COMPENTENCY")
            {

                queryData = await _queryGantt.ExecuteQueryList(queryC, null);
            }
            else if (templateCodes == "PMS_GOAL")
            {

                queryData = await _queryGantt.ExecuteQueryList(queryG, null);
            }

            else if (templateCodes == "PMS_DEVELOPMENT")
            {


                queryData = await _queryGantt.ExecuteQueryList(queryD, null);
            }

            else
            {
                queryA = queryG + " union ( " + queryC + " ) union (" + queryD + " )";
                queryData = await _queryGantt.ExecuteQueryList(queryA, null);
            }


            var list = queryData.ToList();
            return list;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetStageTaskList(string templateCodes, string pdmId = null, string ownerId = null, string stageId = null)
        {
            var query = $@"with recursive task as(
               select nt.""Id"",nt.""TemplateCode"" as TemplateCode,tl.""Code"" as NtsStatusCode,nt.""TaskStatusId""
               from public.""NtsService"" as s
               join public.""NtsTask"" as nt on s.""Id""=nt.""ParentServiceId"" and nt.""ParentTaskId"" is null and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
               left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
               where s.""IsDeleted""=false and s.""ParentServiceId""='{pdmId}' and s.""Id""='{stageId}' and s.""OwnerUserId""='{ownerId}' --and nt.""AssignedToUserId""='{ownerId}'
               union all
               select nt.""Id"", nt.""TemplateCode"", tl.""Code"" as NtsStatusCode, nt.""TaskStatusId""
               from public.""NtsTask"" as nt
               join task as em on em.""Id""=nt.""ParentTaskId""    
            left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false)
               select * from task";




            var queryData = await _queryGantt.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetPerformanceList(string userId, bool isProjectManager, string year = null)
        {
            string query = "";
            // if (isProjectManager)
            // {
            query = @$"select s.""ServiceSubject"" as Name, s.""Id"" as Id from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PMS_PERFORMANCE_DOCUMENT' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            --join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on s.""UdfNoteId"" = pd.""NtsNoteId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}' 
                            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pd.""DocumentMasterId""=pdm.""Id"" and pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'
                            where s.""OwnerUserId""='{userId}' and  s.""IsDeleted""=false #WHERE# and s.""CompanyId""='{_userContext.CompanyId}'";

            var where = "";
            if (year.IsNotNullAndNotEmpty())
            {
                where = $@" and pd.""Year"" = '{year}'";
            }
            query = query.Replace("#WHERE#", where);

            // }
            //else
            //{
            //    query = $@"with recursive ntsservice as(
            //             select t.""AssignedToUserId"",s.""Id"",s.""Id"" as ""ParentServiceId"", s.""ServiceSubject"" from ""NtsService"" as s
            //                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""
            //             left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""AssignedToUserId"" = '{userId}' and t.""ParentServiceId"" is not null
            //                where tt.""Code""='PMS_PERFORMANCE_DOCUMENT' 

            //                union all

            //                select t.""AssignedToUserId"", s.""Id"", ns.""ParentServiceId"" as ""ParentServiceId"", ns.""ServiceSubject"" from ""NtsService"" as s
            //                inner join ntsservice as ns on s.""ParentServiceId"" = ns.""Id""
            //                left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""AssignedToUserId"" = '{userId}'  and t.""ParentServiceId"" is not null
            //                )
            //                select ""ParentServiceId"" as Id, ""ServiceSubject"" as Name from ntsservice
            //                where ""AssignedToUserId"" is not null
            //                group by ""ParentServiceId"", ""ServiceSubject"" ";
            //}
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetPDMList(string year = null)
        {

            var query = @$"select pdm.""Name"", pdm.""Id"" as Id 
                            from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm
                            join public.""NtsNote"" as n on pdm.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}'
                            join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""Code""='PERFORMANCE_DOCUMENT_MASTER' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'                            
                            where pdm.""IsDeleted""=false #WHERE# and pdm.""CompanyId""='{_userContext.CompanyId}'";

            var where = "";
            if (year.IsNotNullAndNotEmpty())
            {
                where = $@" and pdm.""Year"" = '{year}'";
            }
            query = query.Replace("#WHERE#", where);

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<ServiceViewModel> GetPDMDetails(string pdmId)
        {
            var query = $@"Select pdm.""StartDate"",pdm.""EndDate"" as DueDate,pdm.""NtsNoteId"" as UdfNoteId,pdm.""DocumentStatus"" as PerformanceDocumentStatus 
                            from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm
                            where pdm.""Id""='{pdmId}' and pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<ServiceViewModel> GetPerformanceDetails(string projectId)
        {
            var query = @$"select p.""StartDate"", p.""DueDate"" ,""ServiceNo"", sp.""Name"" as Priority, ss.""Name"" as ServiceStatusName
                            from public.""NtsService"" as p
                            join public.""LOV"" as sp on sp.""Id"" = p.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = p.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                            where p.""Id"" = '{projectId}' and  p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamWorkloadData(string projectId, string userId, bool isProjectManager = false)
        {
            string query;
            if (isProjectManager)
            {
                query = $@"select u.""Id"" as UserId, u.""Name"" as UserName, u.""PhotoId"" from public.""User"" as u
                        join(
                        WITH RECURSIVE NtsService AS(
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"" as ts, s.""Id"", s.""Id"" as ""ServiceId"", s.""ServiceSubject""
                        FROM public.""NtsService"" as s                        
						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
						where  s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'

                        union all
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"" as ts, s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id"" 
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        )
                    SELECT ""AssignedToUserId"" as UserId from NtsService
                    where ""AssignedToUserId"" is not null group by ""AssignedToUserId"") p
                    on u.""Id""=p.""userid"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  ";
            }
            else
            {
                query = $@"select u.""Id"" as UserId, u.""Name"" as UserName, u.""PhotoId"" from public.""User"" as u
                        join(
                        WITH RECURSIVE NtsService AS(
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"" as ts, s.""Id"", s.""Id"" as ""ServiceId"", s.""ServiceSubject""
                        FROM public.""NtsService"" as s
                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""AssignedToUserId""='{userId}' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
						where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'

                        union all
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"" as ts, s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""AssignedToUserId""='{userId}' and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        )
                    SELECT ""AssignedToUserId"" as UserId from NtsService
                    where ""AssignedToUserId"" is not null group by ""AssignedToUserId"") p
                    on u.""Id""=p.""userid"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' ";
            }
            var queryData = await _queryTWRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByUser(string projectId, string userId)
        {
            //       var query = $@"WITH RECURSIVE NtsService AS ( 
            //                   SELECT t.""AssignedToUserId"",t.""CreatedDate"", t.""TaskSubject"", t.""Id"" as taskid,t.""TaskStatusId"",s.""Id"",s.""Id"" as ServiceId ,s.""ServiceSubject""
            //                   FROM public.""NtsService"" as s

            //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //	where s.""Id""='{projectId}'
            //	union all
            //                   SELECT t.""AssignedToUserId"",t.""CreatedDate"", t.""TaskSubject"", t.""Id"" as taskid, t.""TaskStatusId"", s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
            //                   FROM public.""NtsService"" as s
            //                   join NtsService ns on s.""ParentServiceId""=ns.""Id""
            //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""	                         

            //               )
            //               SELECT distinct p.""AssignedToUserId"",p.""CreatedDate"", p.""taskid"" as TaskId, p.""TaskSubject"" as TaskName, lov.""Name"" as TaskStatus from NtsService as p
            //               join public.""LOV"" as lov on lov.""Id""=p.""TaskStatusId""
            //where p.""AssignedToUserId""='{userId}' order by p.""CreatedDate"" desc ";

            var query = $@"select count(ts) as SubTaskId, t.*
                        from public.""NtsTask"" as nt
                        join(
                        WITH RECURSIVE NtsService AS(
                        SELECT t.""AssignedToUserId"", t.""CreatedDate"", t.""TaskSubject"", t.""Id"" as taskid, t.""TaskStatusId"", s.""Id"", s.""Id"" as ServiceId, s.""ServiceSubject""
                        FROM public.""NtsService"" as s

                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
						where s.""Id""='{projectId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
						union all
                        SELECT t.""AssignedToUserId"", t.""CreatedDate"", t.""TaskSubject"", t.""Id"" as taskid, t.""TaskStatusId"", s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""	and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                         where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        )
                    SELECT distinct p.""AssignedToUserId"",p.""CreatedDate"", p.""taskid"" as TaskId, p.""TaskSubject"" as TaskName, lov.""Name"" as TaskStatus from NtsService as p
                    join public.""LOV"" as lov on lov.""Id""=p.""TaskStatusId"" and  lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
					where p.""AssignedToUserId""='{userId}' and  p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                    )
                    t on t.""taskid"" = nt.""Id""
	                left join public.""NtsTask"" as ts on ts.""ParentTaskId""=t.""taskid"" and  ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
	                group by t.""AssignedToUserId"",t.""CreatedDate"", t.""taskid"", t.""taskname"", t.""taskstatus"" order by t.""CreatedDate"" desc ";

            var queryData = await _queryTWRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDateData(string projectId)
        {
            string query = "";

            query = $@"	WITH RECURSIVE NtsService AS(
                        SELECT t.""AssignedToUserId"",t.""StartDate"", t.""TaskSubject"" as ts, s.""Id"", t.""Id"" as ""TaskId"", s.""ServiceSubject""
                        FROM public.""NtsService"" as s

                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
						where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'

                        union all
                        SELECT t.""AssignedToUserId"", t.""StartDate"", t.""TaskSubject"" as ts, s.""Id"", t.""Id"" as ""TaskId"", s.""ServiceSubject""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                    
	                                          
                    )
                    SELECT distinct ""StartDate"" as StartDate,""TaskId"",""Id"" from NtsService
                    order by ""StartDate"" desc";

            var queryData = await _queryTWRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByDate(string projectId, DateTime startDate, bool isProjectManager = false)
        {
            string query = "";
            if (isProjectManager)
            {
                query = $@"WITH RECURSIVE NtsService AS ( 
                         SELECT t.""AssignedToUserId"", t.""TaskSubject"",t.""StartDate"", t.""Id"" as taskid,t.""TaskStatusId"",s.""Id"",s.""Id"" as ServiceId ,s.""ServiceSubject""

                        FROM public.""NtsService"" as s

                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
	                    
						where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						union all
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"", t.""StartDate"" , t.""Id"" as taskid, t.""TaskStatusId"", s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""	and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
	                                           
                    )
                    SELECT  distinct p.""StartDate"", p.""taskid"" as TaskId, p.""TaskSubject"" as TaskName, lov.""Name"" as TaskStatus,
                     u.""Id"" as UserId, u.""Name"" as UserName, u.""PhotoId""
                    from NtsService as p
                    
                    join public.""LOV"" as lov on lov.""Id""=p.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}' 
                    join public.""User"" as u on u.""Id""=p.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 

                    where p.""StartDate""='{startDate}' and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' ";

            }
            var queryData = await _queryTWRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<TreeViewViewModel>> GetWBSItemData(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
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
            var list = new List<TreeViewViewModel>();
            var userRoleList = new List<TreeViewViewModel>();
            var projectStageList = new List<TreeViewViewModel>();
            var projectList = new List<TreeViewViewModel>();
            var pStageList = new List<TreeViewViewModel>();
            var stageList = new List<TreeViewViewModel>();
            var query = "";
            id = "INBOX";
            if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');
                query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'
                left join(
						 WITH RECURSIVE NtsService AS ( 
						     SELECT t.""TaskSubject"" as ts,s.""Id"",usp.""UserRoleId"" 
                             FROM public.""NtsService"" as s
                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""  and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
						     where usp.""UserRoleId"" in ({roleText}) and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						     union all
                             SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""UserRoleId""
                             FROM public.""NtsService"" as s
                             join NtsService ns on s.""ParentServiceId""=ns.""Id""
                             join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
	                          
	                         where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                        )
                        SELECT count(ts) as ""Count"",""UserRoleId"" from NtsService group by ""UserRoleId""
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' 
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
                userRoleList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            }

            if (userRoleList.IsNotNull())
            {
                foreach (var l in userRoleList)
                {
                    if (l.Type == "USERROLE")
                    {

                        query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
                , usp.""InboxStageName"" as id, '{l.id}' as ParentId, 'PROJECTSTAGE' as Type,
                true as hasChildren, '{l.UserRoleId}' as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
              
                 left join(
						WITH RECURSIVE NtsService AS ( 
                            SELECT t.""TaskSubject"" as ts,s.""Id"",usp.""InboxStageName"" 
                             FROM public.""NtsService"" as s
                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
						     where usp.""UserRoleId"" = '{l.UserRoleId}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						     union all
                             SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""InboxStageName""
                             FROM public.""NtsService"" as s
                             join NtsService ns on s.""ParentServiceId""=ns.""Id""  
                             join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
	                          
	                         where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'                         
                    )
                    SELECT count(ts) as ""Count"",""InboxStageName"" from NtsService group by ""InboxStageName""
					) nt on usp.""InboxStageName""=nt.""InboxStageName""
                where ur.""Id"" = '{l.UserRoleId}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' 
                Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

                        projectStageList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                        if (projectStageList.IsNotNull())
                        {
                            foreach (var p in projectStageList)
                            {
                                if (p.Type == "PROJECTSTAGE")
                                {
                                    query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{p.id}' as ParentId,                
                'PROJECT' as Type,'{p.UserRoleId}' as UserRoleId,
                true as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
                left join(
	            WITH RECURSIVE NtsService AS ( 
                    SELECT t.""TaskSubject"" as ts,s.""Id"",usp.""TemplateCode"" 
                        FROM public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}' 
						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
						where usp.""UserRoleId"" = '{p.UserRoleId}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						union all
                        SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""TemplateCode""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
	                          
	                    where s.""OwnerUserId""='{p.UserRoleId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'                      
                    )
                    SELECT count(ts) as ""Count"",""TemplateCode"" from NtsService group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{p.UserRoleId}' and usp.""InboxStageName"" = '{p.id}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' 
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
                                    projectList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                                    if (projectList.IsNotNull())
                                    {
                                        foreach (var pr in projectList)
                                        {
                                            if (pr.Type == "PROJECT")
                                            {
                                                query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,
                true as hasChildren,s.""Id"" as ProjectId,
                '{pr.id}' as ParentId,'STAGE' as Type
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                 left join(
	            WITH RECURSIVE NtsService AS ( 
                    SELECT t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
                        FROM public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
						where tt.""Code""='{pr.id}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						union all
                        SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
	                          
	                    where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'                        
                    )
                    SELECT count(ts) as ""Count"",""ServiceId"" from NtsService group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{pr.id}' and s.""OwnerUserId"" = '{userId}' 
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


                                                pStageList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                                                if (pStageList.IsNotNull())
                                                {
                                                    foreach (var ps in pStageList)
                                                    {
                                                        if (ps.Type == "STAGE")
                                                        {
                                                            query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                                                                        s.""Id"" as id,
                                                                        true as hasChildren
                                                                        FROM public.""NtsService"" as s
                                                                        left join(
	                                                                    WITH RECURSIVE NtsService AS ( 
                                                                            SELECT t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
                                                                                FROM public.""NtsService"" as s                      
						                                                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
						                                                        where s.""ParentServiceId""='{ps.id}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
						                                                        union all
                                                                                SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
                                                                                FROM public.""NtsService"" as s
                                                                                join NtsService ns on s.""ParentServiceId""=ns.""Id""
                                                                                join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
	                          
	                                                                            where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'                        
                                                                            )
                                                                            SELECT count(ts) as ""Count"",""ServiceId"" from NtsService group by ""ServiceId""
                 
                                                                        ) t on s.""Id""=t.""ServiceId""
                                                                        where s.""ParentServiceId""='{ps.id}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                                                                        order by s.""SequenceOrder"" asc";

                                                            stageList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            list.AddRange(userRoleList);
            list.AddRange(projectStageList);
            list.AddRange(projectList);
            list.AddRange(pStageList);
            list.AddRange(stageList);
            return list;
        }

        public async Task<bool> CreateMindMap(string model)
        {
            var mappingParent = new List<IdNameViewModel>();
            var data = JsonConvert.DeserializeObject<IList<ProjectGanttTaskViewModel>>(model);
            foreach (var d in data)
            {
                if (d.Type == "Service")
                {
                    //add service
                    var serviceTemplate = new ServiceTemplateViewModel();
                    serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
                    serviceTemplate.TemplateCode = "PROJECT_SUPER_SERVICE";
                    var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);
                    service.ServiceSubject = d.Title;
                    service.OwnerUserId = d.OwnerUserId;
                    service.StartDate = d.Start;
                    service.DueDate = d.End;
                    service.RequestedByUserId = d.UserId;
                    service.DataAction = DataActionEnum.Create;
                    service.Json = "{}";
                    if (d.RefId.IsNullOrEmpty())
                    {
                        service.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        service.Id = d.Id;
                        service.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _serviceBusiness.ManageService(service);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.ServiceId, Code = "Service" });
                }
                else if (d.Type == "Stage")
                {
                    //add stage
                    //add service
                    var serviceTemplate = new ServiceTemplateViewModel();
                    serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
                    serviceTemplate.TemplateCode = "PROJECT_ADHOC_SERVICE";
                    var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);
                    service.ServiceSubject = d.Title;
                    service.OwnerUserId = d.OwnerUserId;
                    service.StartDate = d.Start;
                    service.DueDate = d.End;
                    service.RequestedByUserId = d.UserId;
                    service.DataAction = DataActionEnum.Create;
                    service.Json = "{}";
                    if (d.ParentId.IsNotNullAndNotEmpty())
                    {
                        service.ParentServiceId = mappingParent.Where(x => x.Id == d.ParentId).FirstOrDefault().Name;
                    }
                    if (d.RefId.IsNullOrEmpty())
                    {
                        service.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        service.Id = d.Id;
                        service.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _serviceBusiness.ManageService(service);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.ServiceId, Code = "Service" });
                    //if (d.Predeccessor.Count > 0)
                    //{
                    //    foreach (var id in d.Predeccessor)
                    //    {
                    //        var prec = new NTSSERVICEP
                    //        {
                    //            NtsTaskId = res.Item.ServiceId,
                    //            PrecedenceRelationshipType = PrecedenceRelationshipTypeEnum.FinishToStart,
                    //            PredecessorType = NtsTypeEnum.Service,
                    //            PredecessorId = id
                    //        };
                    //        var result = await _ntsTaskPrecedenceBusiness.Create(prec);
                    //    }
                    //}
                }
                else if (d.Type == "Task")
                {
                    //add Task
                    var taskTemplate = new TaskTemplateViewModel();
                    taskTemplate.ActiveUserId = _repo.UserContext.UserId;
                    taskTemplate.TemplateCode = "PROJECT_ADHOC_TASK";
                    var task = await _taskBusiness.GetTaskDetails(taskTemplate);
                    task.TaskSubject = d.Title;
                    task.OwnerUserId = d.OwnerUserId;
                    task.StartDate = d.Start;
                    task.DueDate = d.End;
                    task.AssignedToUserId = d.UserId;
                    task.DataAction = DataActionEnum.Create;
                    task.Json = "{}";
                    if (d.ParentId.IsNotNullAndNotEmpty())
                    {
                        task.ParentServiceId = mappingParent.Where(x => x.Id == d.ParentId).FirstOrDefault().Name;
                    }
                    if (d.RefId.IsNullOrEmpty())
                    {
                        task.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        task.Id = d.Id;
                        task.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _taskBusiness.ManageTask(task);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.TaskId, Code = "Task" });
                    if (d.Predeccessor.Count > 0)
                    {
                        foreach (var id in d.Predeccessor)
                        {
                            var predData = mappingParent.Where(x => x.Id == id).FirstOrDefault();
                            if (predData.IsNotNull())
                            {
                                var predType = NtsTypeEnum.Task;
                                if (predData.Code == "Task")
                                {
                                    predType = NtsTypeEnum.Task;
                                }
                                else if (predData.Code == "Service")
                                {
                                    predType = NtsTypeEnum.Service;
                                }

                                var prec = new NtsTaskPrecedenceViewModel
                                {
                                    NtsTaskId = res.Item.TaskId,
                                    PrecedenceRelationshipType = PrecedenceRelationshipTypeEnum.FinishToStart,
                                    PredecessorType = predType,//NtsTypeEnum.Task,
                                    PredecessorId = predData.Name
                                };
                                var result = await _ntsTaskPrecedenceBusiness.Create(prec);
                            }
                        }
                    }
                }
                else if (d.Type == "SubTask")
                {
                    //add Task
                    var taskTemplate = new TaskTemplateViewModel();
                    taskTemplate.ActiveUserId = _repo.UserContext.UserId;
                    taskTemplate.TemplateCode = "PROJECT_ADHOC_TASK";
                    var task = await _taskBusiness.GetTaskDetails(taskTemplate);
                    task.TaskSubject = d.Title;
                    task.OwnerUserId = d.OwnerUserId;
                    task.StartDate = d.Start;
                    task.DueDate = d.End;
                    task.AssignedToUserId = d.UserId;
                    task.DataAction = DataActionEnum.Create;
                    task.ParentTaskId = d.ParentId;
                    task.Json = "{}";
                    if (d.ParentId.IsNotNullAndNotEmpty())
                    {
                        task.ParentTaskId = mappingParent.Where(x => x.Id == d.ParentId).FirstOrDefault().Name;
                    }
                    if (d.RefId.IsNullOrEmpty())
                    {
                        task.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        task.Id = d.Id;
                        task.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _taskBusiness.ManageTask(task);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.TaskId, Code = "SubTask" });
                    if (d.Predeccessor.Count > 0)
                    {
                        foreach (var id in d.Predeccessor)
                        {
                            var predData = mappingParent.Where(x => x.Id == id).FirstOrDefault();
                            if (predData.IsNotNull())
                            {
                                var predType = NtsTypeEnum.Task;
                                if (predData.Code == "Task")
                                {
                                    predType = NtsTypeEnum.Task;
                                }
                                else if (predData.Code == "Service")
                                {
                                    predType = NtsTypeEnum.Service;
                                }

                                var prec = new NtsTaskPrecedenceViewModel
                                {
                                    NtsTaskId = res.Item.TaskId,
                                    PrecedenceRelationshipType = PrecedenceRelationshipTypeEnum.FinishToStart,
                                    PredecessorType = predType,
                                    PredecessorId = predData.Name
                                };
                                var result = await _ntsTaskPrecedenceBusiness.Create(prec);
                            }
                        }
                    }
                }
            }
            return true;
        }
        //  public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGrid(string projectId)
        //  {
        //      var query = @$"select t.""Id"",t.""TaskSubject"" as ""Title"",t.""StartDate"" as ""Start"",t.""DueDate"" as ""End"",
        //                  t.""ServiceStage"" as ""ServiceStage"",""ParentId"",t.""UserName"" as ""UserName"",t.""OwnerName"" as ""OwnerName"",true as ""Summary"",
        //                  t.""Priority"",t.""NtsStatus"",pj.""ServiceSubject"" as ""ProjectName""
        //                  FROM public.""NtsService"" as s
        //                  join(
        //                  WITH RECURSIVE NtsService AS (
        //                  SELECT t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", n.""Name"" as ""OwnerName"",'{projectId}'  as tmp,
        //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
        //                  FROM public.""NtsService"" as s
        //                  left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                  left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
        //                  left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
        //                  left join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
        //                   left join public.""User"" as n on t.""OwnerUserId""=n.""Id""

        //                  where s.""Id""='{projectId}' 
        //union all
        //                  SELECT t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"" , n.""Name"" as ""OwnerName"",'{projectId}' as tmp,
        //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
        //                  FROM public.""NtsService"" as s
        //                  join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
        //                  join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                  join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
        //                  join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
        //                  join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
        //                  join public.""User"" as n on t.""OwnerUserId""=n.""Id""
        //                  )
        //                   SELECT ""Id"",""TaskSubject"",""StartDate"",""DueDate"",""ParentId"",""UserName"",""OwnerName"",tmp,""Priority"",""NtsStatus"",
        //                  ""ServiceStage"" from NtsService where ""Id"" is not null ) t on t.tmp=s.""Id""
        //      left join public.""NtsService"" as pj on pj.""Id""='{projectId}' 
        //      where s.""Id""='{projectId}'
        //                  ";
        //      var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
        //      return queryData;

        //  }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTask(string userId, string projectId, bool isProjectManager = false, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var query = "";
            // var search = @$" tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}'";

            //if (projectId.IsNotNull())
            //{
            //    search = @$" s.""Id""='{projectId}' ";

            //}

            if (isProjectManager)
            {
                query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}' 
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                         where  tt.""Code""='PROJECT_SUPER_SERVICE' and  s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
			
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}' 
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}' 
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}' 
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                         #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere#  and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'  
	                             
                    )
                 SELECT * from Nts where Level='Child'";
            }
            else
            {
                query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}' 
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                          where  tt.""Code""='PROJECT_SUPER_SERVICE' and  s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
					
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}' 
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}' 
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerUserId"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",nt.""OwnerUserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}' 
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}' 
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
	                     where t.""AssignedToUserId""='{userId}' or nt.""OwnerUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
                         #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere# 
                    )
                 SELECT * from Nts where Level='Child'";

            }



            //if (projectIds.IsNotNull() && projectIds.Count > 0)
            //{
            //    string pids = null;
            //    foreach (var i in projectIds)
            //    {
            //        pids += $"'{i}',";
            //    }
            //    pids = pids.Trim(',');
            //    if (pids.IsNotNull())
            //    {
            //        projectSerach = @" s.""Id"" in (" + pids + ") ";
            //    }
            //}

            //query = query.Replace("#ProjectWhere#", projectSerach);
            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" where t.""OwnerUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    if (ownerIds.IsNotNull() && ownerIds.Count > 0)
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in (" + aids + ") ";
                    }
                    else
                    {
                        assigneeSerach = @" where t.""AssignedToUserId"" in (" + aids + ") ";
                    }

                }
            }
            query = query.Replace("#AssigneeWhere#", assigneeSerach);
            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" where t.""TaskStatusId"" in (" + sids + ") ";
                    }

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (tasksStatus.IsNotNull() && tasksStatus.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""DueDate"" and t.""DueDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                    }
                    else
                    {
                        dateSerach = @$" where '{ startDate.ToYYYY_MM_DD_DateFormat() }' <= t.""StartDate"" and t.""StartDate"" < '{ dueDate.ToYYYY_MM_DD_DateFormat() }' "; ;
                    }
                }


            }

            query = query.Replace("#DateWhere#", dateSerach);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;


        }

        public Task<IList<IdNameViewModel>> GetPerformanceUserIdNameList(string projectId)
        {
            throw new NotImplementedException();
        }
        public async Task<IList<MailViewModel>> ReadEmailTaskData(string userId)
        {
            string query = "";

            query = $@"	SELECT distinct t.""Id"" as Id,t.""TaskSubject"" as Subject,s.""ServiceSubject"" as Project,tu.""Email"" as From ,t.""StartDate"" as StartDate FROM public.""Template""  as tt
                    join public.""NtsTask"" as t on tt.""Id""=t.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
                    join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
                    join public.""ProjectEmailSetup"" as es on es.""ServiceId""=s.""Id"" and es.""IsDeleted""=false and es.""CompanyId""='{_userContext.CompanyId}' 
                    join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                    left join public.""User"" as tu on t.""OwnerUserId""=tu.""Id"" and tu.""IsDeleted""=false and tu.""CompanyId""='{_userContext.CompanyId}' 
                    where ""Code""='EMAIL_TASK' and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryMailTaskRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<WBSViewModel>> ReadProjectTaskForEmailList(string projectId)
        {
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""ServiceNo"" as ServiceNo,'Project' as ""Type"", s.""LockStatusId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 	join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					    where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",s.""ServiceNo"" as ServiceNo,'Stage' as ""Type"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                          where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceNo"" as TaskNo,""ServiceSubject"" as Title,""Type"" ,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",true as Summary,""Priority""--,""NtsStatus"" 
                from NtsService


                 union all

                 select t.""Id"",t.""TaskNo"" as TaskNo, t.""TaskSubject"" as Title,'Task' as ""Type"", t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",true as Summary, sp.""Name"" as ""Priority""--, ss.""Name"" as ""NtsStatus"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        --join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                          
                    )
                 SELECT ""Id"" as Id,""taskno"" as ItemNo,""ParentId"" as ParentId,""title"" as Subject,""start"" as StartDate,""end"" as DueDate,""Type"" as Type from Nts";

            var queryData = await _queryGantt.ExecuteQueryList<WBSViewModel>(query, null);

            return queryData;


        }
        public async Task<IList<IdNameViewModel>> ReadPerformanceTaskUserData(string projectId)
        {
            var cypher = $@" WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""ServiceNo"" as ServiceNo,'Project' as ""Type"", s.""LockStatusId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as NtsStatus
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 	join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					    where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ServiceNo"" as ServiceNo,'Stage' as ""Type"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", u.""Id"" as ""UserId"", sp.""Name"" as Priority, ss.""Name"" as NtsStatus
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                          where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceNo"" as TaskNo,""ServiceSubject"" as Title,""Type"" ,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"", ""UserId"",true as Summary,""Priority""--,""NtsStatus"" 
                from NtsService


                 union all

                 select t.""Id"", t.""TaskNo"" as TaskNo, t.""TaskSubject"" as Title,'Task' as ""Type"", t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", u.""Id"" as ""UserId"",true as Summary, sp.""Name"" as ""Priority""--, ss.""Name"" as ""NtsStatus"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        --join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                          
                    )
                 --SELECT ""Id"" as Id,""taskno"" as ItemNo,""ParentId"" as ParentId,""title"" as Subject,""start"" as StartDate,""end"" as DueDate,""Type"" as Type,""UserName"",""UserId""  from Nts

                 SELECT Distinct ""UserName"" as Name,""UserId"" as Id from Nts ";


            var list = await _queryRepo1.ExecuteQueryList(cypher, null);
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplate(string templateId, string userId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskByUsers(string templateId, string userId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetSLADetails(string templateId, string userId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var query = @$"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(t.""TaskSLA"") as Days, Avg(t.""ActualSLA"") as ActualSLA
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        and t.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and t.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and ts.""Code"" = 'TASK_STATUS_COMPLETE' group by t.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewData(string templateId, string userId, List<string> tasksStatus = null, List<string> ownerIds = null)
        {

            var query = @$"select t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""OwnerName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' 
                        #OwnerWhere# #StatusWhere# ";

            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""RequestedByUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);

            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (ownerIds.IsNotNull() && ownerIds.Count > 0 && ownerSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersList(string templateId, string userId)
        {
            var query = $@"select distinct t.""RequestedByUserId"" as Id, u.""Name""
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<IList<IdNameViewModel>> GetTaskUsersList(string templateId)
        {
            var query = $@"select distinct t.""AssignedToUserId"" as Id, u.""Name""
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }
        //For PM
        public async Task<List<ProjectDashboardChartViewModel>> GetPMTaskStatusByTemplate(string templateId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetPMTaskByUsers(string templateId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.UserName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetPMSLADetails(string templateId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var query = @$"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(t.""TaskSLA"") as Days, Avg(t.""ActualSLA"") as ActualSLA
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        and t.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and t.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and ts.""Code"" = 'TASK_STATUS_COMPLETE' group by t.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadPMTaskGridViewData(string templateId, List<string> tasksStatus = null, List<string> userIds = null)
        {

            var query = @$"select t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                        #UserWhere# #StatusWhere# ";

            var userSerach = "";
            if (userIds.IsNotNull() && userIds.Count > 0)
            {
                string uids = null;
                foreach (var i in userIds)
                {
                    uids += $"'{i}',";
                }
                uids = uids.Trim(',');
                if (uids.IsNotNull())
                {
                    userSerach = @" and t.""AssignedToUserId"" in (" + uids + ") ";
                }
            }
            query = query.Replace("#UserWhere#", userSerach);

            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((userIds.IsNotNull() && userIds.Count > 0) || (userIds.IsNotNull() && userIds.Count > 0 && userSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }


        // For Group Template


        public async Task<List<ProjectDashboardChartViewModel>> GetGroupTemplate()
        {


            var query = @$"select GRT.""Id"",GR.""Name"" as GroupName from  public.""NtsGroup"" GR left join  public.""NtsGroupTemplate"" GRT on GR.""Id""=GRT.""NtsGroupId""";
            ///  query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, GroupName = x.GroupName }).ToList();
            return list;
        }






        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeGroup(string TemplateID, string UserID, string StatusLOV)
        {


            //var query = @$"select LOV.""Name"" as Type,Count(*) as Value  from public.""NtsTask"" as NTS left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId""  
            //left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id""
            //left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId""
            //left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId""
            //left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id""
            //left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" 
            //left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""
            //left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId"" 
            //where person2.""UserId""='{UserID}'
            // and GRT.""NtsGroupId"" = '{TemplateID}'
            //group by  LOV.""Name""";

            var query = @$"select Tm.""Id"" as Id,  Tm.""Name"" as Type,Count(*) as Value from public.""NtsTask"" as NTS left join public.""LOV"" as LOV 
on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false' and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_userContext.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}' and GRT.""NtsGroupId"" = '{TemplateID}' and  NTS.""IsDeleted""='false' and NTS.""CompanyId""='{_userContext.CompanyId}' #StatusLOV#
group by  Tm.""Name"",Tm.""Id""";
            //query = query.Replace("#RequesUserID#",UserID).Replace("#TemplateID#",TemplateID);

            var sr = "";
            if (StatusLOV != null)
            {
                sr = $@"and LOV.""Id""='{StatusLOV}'";

            }
            else
            {
                sr = $@"and LOV.""Code""='TASK_STATUS_INPROGRESS'";
            }

            query = query.Replace("#StatusLOV#", sr);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridGroup(string TemplateID, string UserID, string StatusTemplateId, string StatusLOV)
        {


            //var query = @$"select Ur.""Name"" as Type,Count(*) as Value, Ur.""Id"" 
            //from public.""NtsTask"" as NTS
            //left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId""  
            //left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id""
            //left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId""
            //left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId""
            //left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id""
            //left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" 
            //left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""
            //left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId"" 
            //where person2.""UserId""='{UserID}'
            // and GRT.""NtsGroupId"" = '{TemplateID}'
            //group by  Ur.""Name"", Ur.""Id""";

            var query = $@"select Ur.""Name"" as Type,Count(*) as Value, Ur.""Id"" 
from public.""NtsTask"" as NTS left join public.""LOV"" as LOV on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false' and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and ur.""IsDeleted""='false' and ur.""CompanyId""='{_userContext.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_userContext.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}' and GRT.""NtsGroupId"" = '{TemplateID}'and  NTS.""IsDeleted""='false' and NTS.""CompanyId""='{_userContext.CompanyId}'  #StatusLOV#  #StatusTemplateid#
group by  Ur.""Name"", Ur.""Id""  limit 5";

            var str = "";

            if (StatusTemplateId != null)
            {
                str = $@" and NTS.""TemplateId""='{StatusTemplateId}'";

            }
            query = query.Replace("#StatusTemplateid#", str);
            var status = "";

            if (StatusLOV != null)
            {

                status = $@" and LOV.""Id""='{StatusLOV}'";
            }
            else { status = $@" and LOV.""Code""='TASK_STATUS_INPROGRESS'"; }

            query = query.Replace("#StatusLOV#", status);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> MdlassignUserGroup(string TemplateID, string UserID)
        {


            var query = @$"select Distinct(Ur.""Name"") as Type,Ur.""Id"" as RefId
from public.""NtsTask"" as NTS left join public.""LOV"" as LOV 
on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false' and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id""  and Ur.""IsDeleted"" = 'false' and Ur.""CompanyId""='{_userContext.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_userContext.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}'
 and GRT.""NtsGroupId"" = '{TemplateID}' and NTS.""IsDeleted""='false' and NTS.""CompanyId""='{_userContext.CompanyId}'";
            ///  query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, AssigneeId = x.RefId }).ToList();
            return list;
        }

        public async Task<List<NtsTaskChartList>> GetGridListGroup(string TemplateID, string UserID, List<string> assigneeIds = null, List<string> StatusIDs = null)
        {


            var query = @$"select Tm.""Name"" as TemplateName, NTS.""Id"", ""TaskSubject"",""TaskNo"",Ur.""Name"" as AssignName,LOV.""Name"" as Priority,LOVS.""Name"" as Status,NTS.""StartDate"",NTS.""DueDate""
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskPriorityId"" and LOV.""IsDeleted""='false' and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as LOVS on LOVS.""Id""=NTS.""TaskStatusId""  and LOVS.""IsDeleted""='false' and LOVS.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=Ur.""Id""  and Ur.""IsDeleted"" = 'false' and Ur.""CompanyId""='{_userContext.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_userContext.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
where person2.""UserId""='{UserID}'
 and GRT.""NtsGroupId"" = '{TemplateID}' and NTS.""IsDeleted""='false' and NTS.""CompanyId""='{_userContext.CompanyId}' #AssigneeUser# #TaskStatusId# ";

            var assigneeSerach = "";
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                string aids = null;
                foreach (var i in assigneeIds)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    assigneeSerach = @"and NTS.""AssignedToUserId"" in (" + aids + ") ";
                }
            }
            query = query.Replace("#AssigneeUser#", assigneeSerach);


            var StatusSerach = "";
            if (StatusIDs.IsNotNull() && StatusIDs.Count > 0)
            {
                string aids = null;
                foreach (var i in StatusIDs)
                {
                    aids += $"'{i}',";
                }
                aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    StatusSerach = @"and NTS.""TaskStatusId"" in (" + aids + ") ";
                }
            }
            query = query.Replace("#TaskStatusId#", StatusSerach);

            //query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<NtsTaskChartList>(query, null);

            //var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetDatewiseTaskGroup(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null)
        {


            var query = @$"select CAST(NTS.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(NTS.""TaskSLA"") as Days,Avg(NTS.""ActualSLA"") as ActualSLA 
from public.""NtsTask"" as NTS left join public.""LOV"" as LOV 
on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false' and LOV.""CompanyId""='{_userContext.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_userContext.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_userContext.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
where NTS.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and NTS.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE 
and LOV.""Code""='TASK_STATUS_COMPLETE'
and person2.""UserId""='{UserID}' and GRT.""NtsGroupId"" = '{TemplateID}' and   NTS.""IsDeleted""='false' and NTS.""CompanyId""='{_userContext.CompanyId}'
 Group by NTS.""DueDate""::TIMESTAMP::DATE ";
            //query = query.Replace("#RequesUserID#",UserID).Replace("#TemplateID#",TemplateID);
            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        //for project manager
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeProjectGroup(string TemplateID, string StatusLOV = null)
        {

            var query = @$"select Tm.""Id"",Tm.""Name"" as NtsStatus, t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" Tm on Tm.""Id""=t.""TemplateId"" and   Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
        
						where GRT.""NtsGroupId""='{TemplateID}' and   t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}' #StatusLOV#";

            var sr = "";
            if (StatusLOV != null)
            {
                sr = $@"and ts.""Id""='{StatusLOV}'";

            }
            else
            {
                sr = $@"and ts.""Code""='TASK_STATUS_INPROGRESS'";
            }

            query = query.Replace("#StatusLOV#", sr);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.Id).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetChartByAssigneduserProjectGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id"" and   ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' and   t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}' #StatusTemplateid#  #StatusLOV# ";


            var str = "";

            if (StatusTemplateID != null)
            {
                str = $@" and t.""TemplateId""='{StatusTemplateID}'";

            }
            query = query.Replace("#StatusTemplateid#", str);
            var status = "";

            if (StatusLOV != null)
            {

                status = $@" and ts.""Id""='{StatusLOV}'";
            }
            else { status = $@" and ts.""Code""='TASK_STATUS_INPROGRESS'"; }

            query = query.Replace("#StatusLOV#", status);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> GetGridListProjectGroup(string templateId, List<string> tasksStatus = null, List<string> ownerIds = null)
        {

            var query = @$"select Tm.""Name"" as GroupName, t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""OwnerName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                          join public.""Template"" Tm on Tm.""Id""=t.""TemplateId"" and   Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                          join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' and   t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}'
                        #OwnerWhere# #StatusWhere# ";

            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""AssignedToUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);

            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (ownerIds.IsNotNull() && ownerIds.Count > 0 && ownerSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridProjectGroup(string templateId)
        {
            var query = $@"select distinct t.""AssignedToUserId"" as Id, u.""Name"" as Value
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
            join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
            where  GRT.""NtsGroupId""='{templateId}' and   t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseTaskProjectGroup(string templateId, DateTime? FromDate, DateTime? ToDate)
        {
            var query = $@"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(t.""TaskSLA"") as Days,Avg(t.""ActualSLA"") as ActualSLA
                                FROM public.""NtsTask"" as t
                                join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'

                        where t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}' and t.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and t.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and ts.""Code""='TASK_STATUS_COMPLETE'  and  GRT.""NtsGroupId""='{templateId}'
                        Group by t.""DueDate""::TIMESTAMP::DATE";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }







        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplateGroup(string templateId, string userId, string StatusLOV = null)
        {

            var query = @$"select  Tm.""Id"",Tm.""Name"" as NtsStatus, t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                       join public.""Template"" Tm on Tm.""Id""=t.""TemplateId"" and   Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
						where t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}' and GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}' #StatusLOV# ";


            var sr = "";
            if (StatusLOV != null)
            {
                sr = $@"and ts.""Id""='{StatusLOV}'";

            }
            else
            {
                sr = $@"and ts.""Code""='TASK_STATUS_INPROGRESS'";
            }

            query = query.Replace("#StatusLOV#", sr);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskByUsersGroup(string templateId, string userId, string StatusTemplateID = null, string StatusLOV = null)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}' 
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id"" and   ou.""IsDeleted""='false' and ou.""CompanyId""='{_userContext.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
						where t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}' and GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}' #StatusTemplateid#  #StatusLOV# ";


            var str = "";

            if (StatusTemplateID != null)
            {
                str = $@" and t.""TemplateId""='{StatusTemplateID}'";

            }
            query = query.Replace("#StatusTemplateid#", str);
            var status = "";

            if (StatusLOV != null)
            {

                status = $@" and ts.""Id""='{StatusLOV}'";
            }
            else { status = $@" and ts.""Code""='TASK_STATUS_INPROGRESS'"; }

            query = query.Replace("#StatusLOV#", status);


            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewDataGroup(string templateId, string userId, List<string> tasksStatus = null, List<string> ownerIds = null)
        {

            var query = @$"select Tm.""Name"" as GroupName, t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""OwnerName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                         join public.""Template"" Tm on Tm.""Id""=t.""TemplateId"" and   Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                          join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}' and   t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}'
                        #OwnerWhere# #StatusWhere# ";

            var ownerSerach = "";
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                string oids = null;
                foreach (var i in ownerIds)
                {
                    oids += $"'{i}',";
                }
                oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""RequestedByUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);

            var statusSerach = "";
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                string sids = null;
                foreach (var i in tasksStatus)
                {
                    sids += $"'{i}',";
                }
                sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (ownerIds.IsNotNull() && ownerIds.Count > 0 && ownerSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in (" + sids + ") ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersListGroup(string templateId, string userId)
        {
            var query = $@"select distinct t.""RequestedByUserId"" as Id, u.""Name""
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
            join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'
            where  GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}' and   t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}'  ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseSingleGroup(string templateId, string userId, DateTime? FromDate, DateTime? ToDate)
        {
            var query = $@"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(t.""TaskSLA"") as Days,Avg(t.""ActualSLA"") as ActualSLA
                                FROM public.""NtsTask"" as t
                                join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and   tp.""IsDeleted""='false' and tp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and   ts.""IsDeleted""='false' and ts.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and   u.""IsDeleted""='false' and u.""CompanyId""='{_userContext.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and   GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_userContext.CompanyId}'

                        where t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}' and t.""DueDate""::TIMESTAMP::DATE >='2021-05-01'::TIMESTAMP::DATE and t.""DueDate""::TIMESTAMP::DATE <='2021-05-21'::TIMESTAMP::DATE
and ts.""Code""='TASK_STATUS_COMPLETE'  and  GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}'
                        Group by t.""DueDate""::TIMESTAMP::DATE";

            var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryPerDoc.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<List<PerformanceDocumentViewModel>> GetPerformanceDocumentsList()
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" where --""DocumentStatus""='{(int)((PerformanceDocumentStatusEnum)Enum.Parse(typeof(PerformanceDocumentStatusEnum), PerformanceDocumentStatusEnum.Publishing.ToString()))}' and
 ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryPerDoc.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task UpdatePerformanceDocumentMasterStatus(string Id, PerformanceDocumentStatusEnum status)
        {
            var statusValue = (int)((PerformanceDocumentStatusEnum)Enum.Parse(typeof(PerformanceDocumentStatusEnum), status.ToString()));
            string query = @$"update cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" set ""DocumentStatus""='{statusValue}' where ""Id""='{Id}'";
            var result = await _queryPerDoc.ExecuteScalar<bool?>(query, null);
        }
        public async Task UpdatePerformanceDocumentMasterStageStatus(string Id, PerformanceDocumentStatusEnum status)
        {
            var statusValue = (int)((PerformanceDocumentStatusEnum)Enum.Parse(typeof(PerformanceDocumentStatusEnum), status.ToString()));
            string query = @$"update cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" set ""DocumentStageStatus""='{statusValue}' where ""Id""='{Id}'";
            var result = await _queryPerDoc.ExecuteScalar<bool?>(query, null);
        }

        public async Task<PerformanceDocumentViewModel> IsDocNameExist(string docName, string docId)
        {
            var query = $@"Select * from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" where ""Name""='{docName}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' #IdWhere# ";

            var where = "";
            if (docId.IsNotNullAndNotEmpty())
            {
                where = $@" and ""Id"" !='{docId}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryPerDoc.ExecuteQuerySingle(query, null);
            return queryData;
        }

        //public async Task<PerformanceDocumentStageViewModel> IsDocStageNameExist(string docStageName, string docStageId)
        //{
        //    var query = $@"Select * from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" where ""Name""='{docStageName}' #IdWhere# ";

        //    var where = "";
        //    if (docStageId.IsNotNullAndNotEmpty())
        //    {
        //        where = $@" and ""Id"" !='{docStageId}' ";
        //    }
        //    query = query.Replace("#IdWhere#", where);
        //    var queryData = await _queryPerDocStage.ExecuteQuerySingle(query, null);
        //    return queryData;
        //}

        public async Task<IList<PerformanceDocumentStageViewModel>> GetPerformanceDocumentStageData(string parentNoteId, string noteId, string udfNoteId, bool isEnableReview = false)
        {
            //var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" where ""PerformanceDocumentId""='{perDocId}' ";
            var query = $@"SELECT ds.*, N.""Id"" as NoteId, N.""ParentNoteId"" as ParentNoteId,m.""Name"" as MasterName
                            FROM  public.""NtsNote"" N
                            inner join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" ds on  N.""Id"" =ds.""NtsNoteId"" and   ds.""IsDeleted""='false' and ds.""CompanyId""='{_userContext.CompanyId}'
 inner join public.""NtsNote"" p on  p.""Id"" =N.""ParentNoteId"" and   p.""IsDeleted""='false' and p.""CompanyId""='{_userContext.CompanyId}'
 inner join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" m on  m.""NtsNoteId"" =p.""Id"" and   m.""IsDeleted""='false' and m.""CompanyId""='{_userContext.CompanyId}'
                            where   N.""IsDeleted""='false' and N.""CompanyId""='{_userContext.CompanyId}'#WHERE# ";

            string where = "";
            if (parentNoteId.IsNotNullAndNotEmpty())
            {
                where = $@" and N.""ParentNoteId""='{parentNoteId}'";
            }
            if (noteId.IsNotNullAndNotEmpty())
            {
                where = $@" and ds.""NtsNoteId""='{noteId}' ";
            }
            if (udfNoteId.IsNotNullAndNotEmpty())
            {
                where = $@" and ds.""Id""='{udfNoteId}' ";
            }
            if (isEnableReview)
            {
                where = $@" and ds.""EnableReview""='{isEnableReview}' ";
            }
            query = query.Replace("#WHERE#", where);

            var queryData = await _queryPerDocStage.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentStage(string Id)
        {
            //var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" where ""PerformanceDocumentId""='{perDocId}' ";
            var query = $@"SELECT ds.*, N.""Id"" as NoteId, N.""ParentNoteId"" as ParentNoteId,s.""ParentServiceId"" as ParentServiceId,s.""OwnerUserId""
                            FROM  public.""NtsNote"" N
                            inner join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as ds on  N.""Id"" =ds.""NtsNoteId"" and   ds.""IsDeleted""='false' and ds.""CompanyId""='{_userContext.CompanyId}'
inner join public.""NtsService"" as s on  s.""UdfNoteTableId"" =ds.""Id"" and   s.""IsDeleted""='false' and s.""CompanyId""='{_userContext.CompanyId}'
                            where ds.""Id""='{Id}' and  N.""IsDeleted""='false' and N.""CompanyId""='{_userContext.CompanyId}' ";



            var queryData = await _queryPerDocStage.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceGradeRatingData(string parentNoteId, string udfNoteId, string noteId = null)
        {
            var query = $@"select rp.*, rp.""NtsNoteId"" as NoteId, n.""ParentNoteId"",pr.""PerformanceRatingId"", g.""GradeName"",r.""Name"" as RatingName
                            From public.""NtsNote"" as n
                            join cms.""N_PerformanceDocumentMaster_PerformanceGradeRatingPercentage"" as rp on rp.""NtsNoteId"" = n.""Id"" and   rp.""IsDeleted""='false' and rp.""CompanyId""='{_userContext.CompanyId}'
							join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=rp.""GradeId"" and   g.""IsDeleted""='false' and g.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_General_PerformanceRatingItem"" as r on r.""Id""=rp.""RatingId"" and   r.""IsDeleted""='false' and r.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pr on pr.""NtsNoteId""='{parentNoteId}' and  pr.""IsDeleted""='false' and pr.""CompanyId""='{_userContext.CompanyId}'
                            where n.""ParentNoteId""='{parentNoteId}' and   n.""IsDeleted""='false' and n.""CompanyId""='{_userContext.CompanyId}' #NOTEWHERE# #IDWHERE#";

            string notewhere = "";
            if (noteId.IsNotNullAndNotEmpty())
            {
                notewhere = $@" and rp.""NtsNoteId""='{noteId}'";
            }
            query = query.Replace("#NOTEWHERE#", notewhere);
            string idwhere = "";
            if (udfNoteId.IsNotNullAndNotEmpty())
            {
                idwhere = $@" and rp.""Id""='{udfNoteId}'";
            }
            query = query.Replace("#IDWHERE#", idwhere);

            var queryData = await _queryPerDoc.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetPerformanceGradeRatingList(string perRatingId)
        {
            var ratingquery = $@"Select ""NtsNoteId"" as Id from cms.""N_General_PerformanceRating"" where ""Id""='{perRatingId}' ";
            var ratdetails = await _queryRepo1.ExecuteQuerySingle(ratingquery, null);

            var query = $@"SELECT gr.""Name"" as Name, gr.""Id"" as Id
                            FROM  public.""NtsNote"" N
                            inner join cms.""N_General_PerformanceRatingItem"" gr on  N.""Id"" =gr.""NtsNoteId"" and   gr.""IsDeleted""='false' and gr.""CompanyId""='{_userContext.CompanyId}'
                            where N.""ParentNoteId""='{ratdetails.Id}' and   N.""IsDeleted""='false' and N.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }
        //public async Task<IList<IdNameViewModel>> GetPerformanceGradeRatingList()
        //{
        //    var query = $@"SELECT gr.""Name"" as Name, gr.""NtsNoteId"" as Id
        //                    FROM  public.""NtsNote"" N
        //                    inner join cms.""N_General_PerformanceRatingItem"" gr on  N.""Id"" =gr.""NtsNoteId"" and N.""IsDeleted""=false
        //                    where N.""ParentNoteId""='{perRatingId}' and gr.""IsDeleted""=false ";

        //    var queryData = await _queryRepo1.ExecuteQueryList(query, null);
        //    return queryData;
        //}
        private async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentMasterId(string noteId)
        {

            var query = $@"SELECT ds.*, N.""Id"" as NoteId, N.""ParentNoteId"" as ParentNoteId
                            FROM  public.""NtsNote"" N
                            inner join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" ds on  N.""Id"" =ds.""NtsNoteId"" and  ds.""IsDeleted""='false' and ds.""CompanyId""='{_userContext.CompanyId}'
                            where ds.""Id""='{noteId}' and N.""IsDeleted""='false' and N.""CompanyId""='{_userContext.CompanyId}' ";


            var queryData = await _queryPerDocStage.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentMasterStageById(string noteId)
        {

            var query = $@"SELECT ds.*, N.""Id"" as NoteId, N.""ParentNoteId"" as ParentNoteId
                            FROM  public.""NtsNote"" N
                            inner join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as ds on  N.""Id"" =ds.""NtsNoteId"" and  ds.""IsDeleted""='false' and ds.""CompanyId""='{_userContext.CompanyId}'
                            where ds.""Id""='{noteId}' and N.""IsDeleted""='false' and N.""CompanyId""='{_userContext.CompanyId}' ";


            var queryData = await _queryPerDocStage.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByNoteId(string noteId)
        {

            var query = $@"SELECT ds.*
                            FROM  cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as ds 
                            where ds.""NtsNoteId""='{noteId}' and   ds.""IsDeleted""='false' and ds.""CompanyId""='{_userContext.CompanyId}' ";


            var queryData = await _queryPerDoc.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByServiceId(string serviceId)
        {

            var query = $@"select pdm.*,pdm.""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm
                        inner join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""DocumentMasterId"" = pdm.""Id"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}' 
                        inner join public.""NtsService"" as s1 on s1.""UdfNoteTableId""=pd.""Id"" and s1.""IsDeleted""=false and s1.""CompanyId""='{_userContext.CompanyId}' 
                        inner join public.""NtsService"" as s2 on s2.""ParentServiceId""=s1.""Id"" and s2.""IsDeleted""=false and s2.""CompanyId""='{_userContext.CompanyId}' 
                        Where pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'  and s2.""Id""='{serviceId}'
                        ";
            var queryData = await _queryPerDoc.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByDocServiceId(string serviceId)
        {

            var query = $@"select pdm.*,pdm.""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm
                        inner join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""DocumentMasterId"" = pdm.""Id"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}' 
                        inner join public.""NtsService"" as s1 on s1.""UdfNoteTableId""=pd.""Id"" and s1.""IsDeleted""=false and s1.""CompanyId""='{_userContext.CompanyId}' 
                        inner join public.""NtsService"" as s2 on s2.""ParentServiceId""=s1.""Id"" and s2.""IsDeleted""=false and s2.""CompanyId""='{_userContext.CompanyId}' 
                        Where pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'  and s1.""Id""='{serviceId}'
                        ";
            var queryData = await _queryPerDoc.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<PerformanceDocumentViewModel> GetPDMByServiceId(string serviceId)
        {
            var query = $@"select pdm.*,pdm.""NtsNoteId"" as NoteId 
                        from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm
                        join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""DocumentMasterId"" = pdm.""Id"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}' 
                        join public.""NtsNote"" as n on pd.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
                        join public.""NtsService"" as s on s.""UdfNoteId""=n.""Id"" and ss.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'                         
                        Where pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}' and ss.""Id""='{serviceId}'
                        ";
            var queryData = await _queryPerDoc.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<IList<TreeViewViewModel>> GetDiagramMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
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

            var list = new List<TreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');
                query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || COALESCE(nt.""Count"",0)|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur 
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                left join(

                 WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""UserRoleId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""  and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
						     and usp.""UserRoleId"" in ({roleText}) 
					 
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
                where ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' and ur.""Id"" in ({roleText})
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

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

                query = $@"Select usp.""InboxStageName"" ||' (' || COALESCE(nt.""Count"",0)|| ')' as Name
                , usp.""InboxStageName"" as id, '{id}' as ParentId, 'PERFORMANCETYPE' as Type,
                true as hasChildren, '{userRoleId}' as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
              
                 left join(
                    WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""InboxStageName""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
						     where usp.""UserRoleId"" = '{userRoleId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""InboxStageName"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        
                 )
                 SELECT ""Id"",'Parent' as Level,""InboxStageName""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""InboxStageName"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                      join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	                    where t.""AssignedToUserId"" = '{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'         
                    )
						
                    SELECT count(""Id"") as ""Count"",""InboxStageName"" from Nts where Level='Child' group by ""InboxStageName""
					) nt on usp.""InboxStageName""=nt.""InboxStageName""
                where ur.""Id"" = '{userRoleId}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}'
                Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



                var obj = expObj.Where(x => x.Type == "PERFORMANCETYPE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PERFORMANCETYPE")
            {
                query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{id}' as ParentId,                
                'PERFORMANCE' as Type,'{userRoleId}' as UserRoleId,
                true as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
                left join(

                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""TemplateCode""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMS' and usp.""IsDeleted""=false and usp.""CompanyId""='{_userContext.CompanyId}'
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
	                    where t.""AssignedToUserId"" = '{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'        
                    )

	          
                    SELECT count(""Id"") as ""Count"",""TemplateCode"" from Nts where Level='Child' group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{userRoleId}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_userContext.CompanyId}' and usp.""InboxStageName"" = '{id}'
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "PERFORMANCE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PERFORMANCE")
            {
                query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0) || ')' as Name,
                false as hasChildren,s.""Id"" as ProjectId,
                '{id}' as ParentId,'STAGE' as Type
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
						    
						     where tt.""Code""='{id}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  
					 
                        union all
                        SELECT s.""Id"", ns.""ServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""ServiceId"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                           where  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                    --where t.""AssignedToUserId"" = '{userId}'          
                    )

	           
                    SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{id}' and  t.""Count""!=0 and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


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

            return list;
        }

        public async Task<IList<PerformanceDiagramViewModel>> GetPerformanceDiagram(string performanceDocumentId)
        {
            var list = new List<PerformanceDiagramViewModel>();
            var query = $@"select s.""Id"" as Id, s.""Id"" as ReferenceId, s.""ServiceSubject"" as Title, s.""ServiceDescription"" as Description, 6 as TemplateType ,'SERVICE' as ""Type"",
                            3 as NodeShape, null as ParentId
                               from public.""NtsService"" as s
                            where s.""Id""='{performanceDocumentId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                            union
                            select s.""Id"" as Id, s.""Id"" as ReferenceId, PG.""GoalName"" as Title, s.""ServiceDescription"" as Description, 6 as TemplateType ,'GOAL' as ""Type"",
                            3 as NodeShape , 'GOAL' as ParentId
                               from public.""NtsService"" as s
inner join  cms.""N_PerformanceDocument_PMSGoal"" as  PG on  s.""UdfNoteId"" =PG.""NtsNoteId"" and PG.""IsDeleted""=false and PG.""CompanyId""='{_userContext.CompanyId}'
                            where s.""ParentServiceId""='{performanceDocumentId}' and s.""TemplateCode"" = 'PMS_GOAL' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                            union
                            select s.""Id"" as Id, s.""Id"" as ReferenceId, PG.""CompetencyName"" as Title, s.""ServiceDescription"" as Description, 6 as TemplateType ,'COMPENTENCY' as ""Type"",
                            3 as NodeShape , 'COMPETENCY' as ParentId
                               from public.""NtsService"" as s
inner join  cms.""N_PerformanceDocument_PMSCompentency"" as  PG on  s.""UdfNoteId"" =PG.""NtsNoteId"" and PG.""IsDeleted""=false and PG.""CompanyId""='{_userContext.CompanyId}'
                            where s.""ParentServiceId""='{performanceDocumentId}' and s.""TemplateCode"" = 'PMS_COMPENTENCY' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                            union
                            select s.""Id"" as Id, s.""Id"" as ReferenceId, PG.""DevelopmentName"" as Title, s.""ServiceDescription"" as Description, 6 as TemplateType ,'DEVELOPMENT' as ""Type"",
                            3 as NodeShape , 'DEVELOPMENT' as ParentId
                               from public.""NtsService"" as s
inner join  cms.""N_PerformanceDocument_PMSDevelopment"" as  PG on  s.""UdfNoteId"" =PG.""NtsNoteId"" and PG.""IsDeleted""=false and PG.""CompanyId""='{_userContext.CompanyId}'
                            where s.""ParentServiceId""='{performanceDocumentId}' and s.""TemplateCode"" = 'PMS_DEVELOPMENT' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                            union
                            select s.""Id"" as Id, s.""Id"" as ReferenceId, s.""ServiceSubject"" as Title, s.""ServiceDescription"" as Description, 6 as TemplateType ,'PEERREVIEW' as ""Type"",
                            3 as NodeShape , 'PEERREVIEW' as ParentId
                               from public.""NtsService"" as s
                            where s.""ParentServiceId""='{performanceDocumentId}' and s.""TemplateCode"" = 'PMS_PEER_REVIEW' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";

            list = await _queryRepo1.ExecuteQueryList<PerformanceDiagramViewModel>(query, null);

            var adhoc = list.Where(x => x.Type != "SERVICE").Select(x => new PerformanceDiagramViewModel
            {
                Id = x.Id + "_AdhocTask",
                Title = "Tasks",
                Description = x.Title,
                ReferenceId = x.Id,
                ParentId = x.Id,
                Type = "ADHOCROOT",
                TemplateType = TemplateTypeEnum.Service,
                NodeShape = NodeShapeEnum.Rectangle
            });

            var step = list.Where(x => x.Type != "SERVICE").Select(x => new PerformanceDiagramViewModel
            {
                Id = x.Id + "_StepTask",
                Title = "Activities",
                Description = x.Title,
                ReferenceId = x.Id,
                ParentId = x.Id,
                Type = "STEPROOT",
                TemplateType = TemplateTypeEnum.Service,
                NodeShape = NodeShapeEnum.Rectangle
            });
            list.AddRange(adhoc.ToList());

            list.AddRange(step.ToList());

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "GOAL",
                Title = "Goal",
                Description = "Goal",
                ReferenceId = "GOAL_ROOT",
                ParentId = performanceDocumentId,
                Type = "GOAL_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "COMPETENCY",
                Title = "Competency",
                Description = "Competency",
                ReferenceId = "COMPENTENCY_ROOT",
                ParentId = performanceDocumentId,
                Type = "COMPENTENCY_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "DEVELOPMENT",
                Title = "Development",
                Description = "Development",
                ReferenceId = "DEVELOPMENT_ROOT",
                ParentId = performanceDocumentId,
                Type = "DEVELOPMENT_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "PEERREVIEW",
                Title = "Peer Review",
                Description = "Peer Review",
                ReferenceId = "PEERREVIEW_ROOT",
                ParentId = performanceDocumentId,
                Type = "PEERREVIEW_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            //list.Add(new PerformanceDiagramViewModel
            //{
            //    Id = "STEPTASK",
            //    Title = "Step Task",
            //    Description = "Step Task",
            //    ReferenceId = "STEPTASK_ROOT",
            //    ParentId = performanceDocumentId,
            //    Type = "STEPTASK_ROOT",
            //    TemplateType = TemplateTypeEnum.Custom,
            //    NodeShape = NodeShapeEnum.Rectangle
            //});

            var taskquery = $@"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
					 where  s.""Id""='{performanceDocumentId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                           where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title, ""TemplateCode"" as Code,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusId"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""TemplateCode"" as Code, t.""StartDate"" as Start, t.""DueDate"" as End,
                        Case when t.""ParentTaskId"" is not null then t.""ParentTaskId"" else  nt.""Id"" end  as ""ParentId"",
                        u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id"" 
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        where t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
	                             
                    )
                 SELECT * from Nts where Level='Child'";
            var taskResult = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(taskquery, null);


            if (taskResult.IsNotNull())
            {
                var taskList = taskResult.Select(x => new PerformanceDiagramViewModel()
                {
                    Id = x.Id + "_AdhocTask",
                    Title = x.Title,
                    Description = x.Title,
                    ReferenceId = x.ParentId,
                    ParentId = GetNodeParentId(x),
                    Type = x.Code,
                    TemplateType = TemplateTypeEnum.Task,
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                //var subTaskList = taskList.Select(x => new  PerformanceDiagramViewModel()
                //{
                //    Id = x.Id + "_AdhocTask",
                //    Title = "Sub Tasks",
                //    Description = x.Title,
                //    ReferenceId = x.Id,
                //    ParentId = x.Id,
                //    Type = "SUBTASKROOT",
                //    TemplateType = TemplateTypeEnum.Service,
                //    NodeShape = NodeShapeEnum.Rectangle
                //});

                list.AddRange(taskList);
                //list.AddRange(subTaskList);
            }

            var stepRoots = new List<PerformanceDiagramViewModel>();
            foreach (var l in list)
            {
                if (l.Type == "STEPROOT" || l.Type == "SUBTASKROOT")
                {
                    var lst = list.Where(x => x.ParentId == l.Id).ToList();
                    if (lst.IsNotNull() && lst.Count == 0)
                    {
                        stepRoots.Add(l);
                    }
                }
            }

            list = list.Except(stepRoots).ToList();

            list = list.Where(x => !x.Id.Contains("AdhocTask_StepTask")).ToList();

            return list;
        }

        public static string GetNodeParentId(ProjectGanttTaskViewModel model)
        {
            if (model.Code == "PMS_GOAL_ADHOC_TASK" || model.Code == "PMS_COMPENTENCY_ADHOC_TASK" || model.Code == "PMS_DEVELOPMENT_ADHOC_TASK" || model.Code == "PMS_REVIEW_TASK")
            {
                return model.ParentId + "_AdhocTask";
            }
            else
            {
                return model.ParentId + "_StepTask";
            }
        }

        public async Task<IList<GoalViewModel>> GetGoalWeightageByPerformanceId(string Id, string stageId, string userId)
        {

            //            string query = @$" select pg1.""GoalName"" as GoalName ,NS.""Id"" as ServiceId,PG.""Id"",PG.""Weightage""  FROM  public.""NtsService"" NS
            //left join cms.""N_PerformanceDocument_PMSGoal"" as pg1 on pg1.""Id""=NS.""UdfNoteTableId"" and pg1.""IsDeleted""=false and pg1.""CompanyId""='{_userContext.CompanyId}'
            //    inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            //	inner join  cms.""N_PerformanceDocument_PMSGoal"" as  PG on  N.""Id"" =PG.""NtsNoteId"" and PG.""IsDeleted""=false and PG.""CompanyId""='{_userContext.CompanyId}'

            //	where NS.""ParentServiceId""='{Id}' and NS.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}'";

            var query = $@"

select gns.""Id"" as ServiceId,g.""GoalName"" as GoalName,g.""Id"",g.""Weightage"" as Weightage
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate"" and g.""IsDeleted""=false
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
            where pds.""IsDeleted""=false and pdns.""Id""='{Id}' #Stage# #Owner#
";
            var stageFilter = "";
            var ownerFilter = "";
            if (stageId.IsNotNullAndNotEmpty())
            {
                stageFilter = $@"  and pds.""Id""='{stageId}'";
            }
            if (userId.IsNotNullAndNotEmpty())
            {
                ownerFilter = $@"  and pdns.""OwnerUserId""='{userId}'";
            }
            query = query.Replace("#Stage#", stageFilter);
            query = query.Replace("#Owner#", ownerFilter);
            var queryData = await _queryGoal.ExecuteQueryList(query, null);

            return queryData;
        }


        public async Task<IList<GoalViewModel>> GetCompentencyWeightageByPerformanceId(string Id, string stageId, string userId)
        {

            //           string query = @$" select pc.""CompetencyName"" as GoalName ,NS.""Id"" as ServiceId,PG.""Id"",PG.""Weightage""  FROM  public.""NtsService"" NS
            //left join cms.""N_PerformanceDocument_PMSCompentency"" as pc on pc.""Id""=NS.""UdfNoteTableId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_userContext.CompanyId}'
            //   inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            //inner join  cms.""N_PerformanceDocument_PMSCompentency"" as  PG on  N.""Id"" =PG.""NtsNoteId"" and PG.""IsDeleted""=false and PG.""CompanyId""='{_userContext.CompanyId}'
            //where NS.""ParentServiceId""='{Id}' and NS.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}'";

            var query = $@"
select gns.""Id"" as ServiceId,g.""CompetencyName"" as GoalName,g.""Id"",g.""Weightage"" as Weightage
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{Id}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'

";
            var stageFilter = "";
            var ownerFilter = "";
            if (stageId.IsNotNullAndNotEmpty())
            {
                stageFilter = $@"  and pds.""Id""='{stageId}'";
            }
            if (userId.IsNotNullAndNotEmpty())
            {
                ownerFilter = $@"  and pdns.""OwnerUserId""='{userId}'";
            }
            query = query.Replace("#Stage#", stageFilter);
            query = query.Replace("#Owner#", ownerFilter);
            var queryData = await _queryGoal.ExecuteQueryList(query, null);

            return queryData;
        }

        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentMappedUserData(string perDocId, string userid)
        {
            var query = $@"select  u.""Name"" as OwnerUserName,u.""Email"" as OwnerUserEmail,nt.""Id"",nt.""ParentNoteId"",nt.""OwnerUserId"",
hd.""DepartmentName"" as DepartmentName,hj.""JobTitle"" as JobTitle,hd.""Id"" as DepartmentId
from public.""NtsNote"" as nt
join public.""User"" as u on nt.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterUsers"" as pmu on pmu.""NtsNoteId""=nt.""Id"" and pmu.""IsDeleted""=false and pmu.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
  join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
  join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
 left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
where nt.""ParentNoteId""='{perDocId}' and nt.""TemplateCode""='PERFORMANCE_DOCUMENT_MASTER_USERS' and nt.""IsDeleted""=false and nt.""CompanyId""='{_userContext.CompanyId}' #USER# ";

            var user = "";
            if (userid.IsNotNullAndNotEmpty())
            {
                user = $@" and nt.""OwnerUserId""='{userid}'";
            }
            query = query.Replace("#USER#", user);
            var queryData = await _queryPerDoc.ExecuteQueryList(query, null);
            return queryData;
        }

        private async Task<CommandResult<PerformanceDocumentViewModel>> ChangeStatusforDocumentMaster(PerformanceDocumentStatusEnum status, PerformanceDocumentViewModel model)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            model.DocumentStatus = status;
            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, result.Messages);
        }

        public async Task<CommandResult<PerformanceDocumentStageViewModel>> ChangeStatusforDocumentMasterStage(PerformanceDocumentStatusEnum status, PerformanceDocumentStageViewModel model)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            //var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            //var k = rowData1.FirstOrDefault(x => x.Key == "DocumentStageStatus");
            //if (k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
            //{
            //    rowData1[k.Key] = status;
            //}
            model.DocumentStageStatus = status;
            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, result.Messages);
        }

        public async Task<CommandResult<PerformanceDocumentViewModel>> CreatePerDoc(PerformanceDocumentViewModel model)
        {
            var validateName = await IsDocNameExist(model.Name, model.Id);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Name Already Exist");
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "PERFORMANCE_DOCUMENT_MASTER";
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            model.LetterTemplate = HttpUtility.HtmlDecode(model.LetterTemplate);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CommandResult<PerformanceDocumentViewModel>> EditPerDoc(PerformanceDocumentViewModel model)
        {
            var validateName = await IsDocNameExist(model.Name, model.Id);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Name Already Exist");
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            model.LetterTemplate = HttpUtility.HtmlDecode(model.LetterTemplate);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<PerformanceDocumentViewModel>> CreatePerGradeRating(PerformanceDocumentViewModel model)
        {

            var validateName = await IsRatingExist(model);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Record Already Exist");
            }
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "PERFORMANCE_GRADE_RATING_PERCENTAGE";
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CommandResult<PerformanceDocumentViewModel>> EditPerGradeRating(PerformanceDocumentViewModel model)
        {
            var validateName = await IsRatingExist(model);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Record Already Exist");
            }
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<PerformanceDocumentViewModel> IsRatingExist(PerformanceDocumentViewModel model)
        {
            var ratings = await GetPerformanceGradeRatingData(model.ParentNoteId, model.NoteId, model.Id);
            var exist = ratings.Where(x => x.RatingId == model.RatingId && x.GradeId == model.GradeId && x.Id != model.Id).FirstOrDefault();
            return exist;

        }
        public async Task<CommandResult<PerformanceDocumentStageViewModel>> IsStageNameExists(PerformanceDocumentStageViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            var docStageModel = await GetPerformanceDocumentStageData(model.ParentNoteId, null, null);
            if (docStageModel.Count > 0)
            {
                var stagemodel = docStageModel.Select(x => x.Name == model.Name && x.Id != model.Id);
                foreach (var item in stagemodel)
                {
                    if (item)
                    {
                        errorList.Add("Name", "Name already exist.");
                    }
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance();
        }
        private async Task<CommandResult<PerformanceDocumentStageViewModel>> ValidateDate(PerformanceDocumentStageViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            var docModel = await GetPerformanceDocumentDetails(model.ParentNoteId);
            if (model.StartDate > model.EndDate)
            {
                errorList.Add("Date", "Start date should be less than End Date");
            }
            if (model.StartDate < docModel.StartDate)
            {
                errorList.Add("StartDate", "Stage Start date should be greater than " + docModel.StartDate.ToDefaultDateFormat());
            }
            if (model.EndDate > docModel.EndDate)
            {
                errorList.Add("EndDate", "Stage End date should be less than " + docModel.EndDate.ToDefaultDateFormat());
            }
            if (errorList.Count > 0)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance();
        }


        public async Task<CommandResult<PerformanceDocumentStageViewModel>> CreatePerDocStage(PerformanceDocumentStageViewModel model)
        {
            var validateName = await IsStageNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateName.Messages);
            }

            var validateDate = await ValidateDate(model);
            if (!validateDate.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateDate.Messages);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "PERFORMACE_DOCUMENT_MASTER_STAGE";
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<PerformanceDocumentStageViewModel>> EditPerDocStage(PerformanceDocumentStageViewModel model)
        {
            var validateName = await IsStageNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateName.Messages);
            }

            var validateDate = await ValidateDate(model);
            if (!validateDate.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateDate.Messages);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<bool> DeleteDocumentStage(string noteId)
        {
            var query = $@"update cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" set ""IsDeleted""=true where ""NtsNoteId""='{noteId}'";
            await _queryRepo.ExecuteCommand(query, null);

            //await Delete(noteId);
            return true;
        }

        public async Task<List<IdNameViewModel>> GetPerformanceDocumentGoalTemplates()
        {
            var query = $@"select tt.""Code"",tt.""Id"",tt.""DisplayName"" as Name
from public.""Template"" as t
join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_userContext.CompanyId}'
left join public.""TaskTemplate"" as taskt on taskt.""Id""=any(st.""AdhocTaskTemplateIds"") and taskt.""IsDeleted""=false and taskt.""CompanyId""='{_userContext.CompanyId}'
left join public.""Template"" as tt on tt.""Id""=taskt.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
where t.""Code""='PMS_GOAL' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'";
            var data = await _queryRepo1.ExecuteQueryList(query, null);

            //await Delete(noteId);
            return data;
        }

        public async Task<List<IdNameViewModel>> GetPerformanceDocumentCompetencyTemplates()
        {
            var query = $@"select tt.""Code"",tt.""Id"",tt.""DisplayName"" as Name
from public.""Template"" as t
join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_userContext.CompanyId}'
left join public.""TaskTemplate"" as taskt on taskt.""Id""=any(st.""AdhocTaskTemplateIds"") and taskt.""IsDeleted""=false and taskt.""CompanyId""='{_userContext.CompanyId}'
left join public.""Template"" as tt on tt.""Id""=taskt.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
where t.""Code""='PMS_COMPENTENCY' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'";
            var data = await _queryRepo1.ExecuteQueryList(query, null);

            //await Delete(noteId);
            return data;
        }

        public async Task<List<IdNameViewModel>> GetEmployeeReviewTemplate()
        {
            var query = $@"select t.""Code"",t.""Id"",t.""DisplayName"" as Name
from public.""Template"" as t
join public.""TaskTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_userContext.CompanyId}'

where t.""Code""='PMS_EMPLOYEE_REVIEW' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'";
            var data = await _queryRepo1.ExecuteQueryList(query, null);


            return data;
        }
        public async Task<List<IdNameViewModel>> GetManagerReviewTemplate()
        {
            var query = $@"select t.""Code"",t.""Id"",t.""DisplayName"" as Name
from public.""Template"" as t
join public.""TaskTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_userContext.CompanyId}'

where t.""Code""='PMS_MANAGER_REVIEW' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'";
            var data = await _queryRepo1.ExecuteQueryList(query, null);


            return data;
        }
        public async Task<List<IdNameViewModel>> GetPerformanceRatingsList()
        {
            var query = $@"select ""Id"", ""Name"" from cms.""N_General_PerformanceRating"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
            var data = await _queryRepo1.ExecuteQueryList(query, null);
            return data;
        }

        public async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentStageByMaster(string parentServiceId, string docMasterStageId)
        {
            //var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" where ""PerformanceDocumentId""='{perDocId}' ";
            var query = $@"SELECT pds.* ,s.""Id"" as ServiceId
FROM cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
join public.""NtsService"" as s on s.""UdfNoteTableId""=pds.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
where s.""ParentServiceId""='{parentServiceId}' and pds.""DocumentMasterStageId""='{docMasterStageId}' and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}' ";


            var queryData = await _queryPerDocStage.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentByMaster(string ownerUserId, string docMasterId)
        {
            //var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" where ""PerformanceDocumentId""='{perDocId}' ";
            var query = $@"SELECT pds.*,s.""Id"" as ServiceId
                FROM cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pds
                join public.""NtsService"" as s on s.""UdfNoteTableId""=pds.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                where s.""OwnerUserId""='{ownerUserId}' and pds.""DocumentMasterId""='{docMasterId}' and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}' ";


            var queryData = await _queryPerDoc.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<List<TeamWorkloadViewModel>> GetAllApprovedGoals(string ownerUserId, string docServiceId, string stageId)
        {

            //            var query = @$"
            //               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""GoalName"" as StageName,s.""TemplateCode"" as TemplateCode,'{stageId }' as StageId,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",g.""GoalStartDate"" as StartDate,g.""GoalEndDate"" as DueDate,s.""ParentServiceId"",r.""Id"" as RequestedByUserId,g.""SuccessCriteria"" as SuccessCriteria,gr.""EmployeeRating"",gr.""EmployeeComment"" as EmployeeComment,gr.""ManagerRating"" as ManagerRating
            //,gr.""ManagerComment"" as ManagerComment,gr.""Id"" as ReviewId,g.""Id"" as GoalId
            //                              		 FROM public.""NtsService"" as s 
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'   
            //left join cms.""N_PerformanceDocument_PMSGoal"" as g on g.""Id"" = s.""UdfNoteTableId"" and  g.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'   
            //left join cms.""N_PerformanceDocument_PMSStageGoalReview"" as gr on gr.""GoalId"" = g.""Id"" and  gr.""IsDeleted""=false and gr.""CompanyId""='{_userContext.CompanyId}' and gr.""UserId""=s.""OwnerUserId"" and gr.""StageId""='{stageId}'
            //                where s.""ParentServiceId""='{docServiceId}'  and s.""TemplateCode""='PMS_GOAL' and ss.""Code""='SERVICE_STATUS_COMPLETE'
            //and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' #WHERE#";
            //            var search = "";
            //            if (ownerUserId.IsNotNullAndNotEmpty())
            //            {
            //                search = $@" and s.""OwnerUserId""='{ownerUserId}' ";
            //            }
            //            query = query.Replace("#WHERE#", search);

            //            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            //            return queryData;
            var query1 = $@"
select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""GoalName"" as StageName,gns.""TemplateCode"" as TemplateCode,'{stageId }' as StageId,
            gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            ,g.""GoalStartDate"" as ""StartDate"",g.""GoalEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,gr.""ManagerComment"" as ManagerComment
            ,gr.""EmployeeRating"",gr.""EmployeeComment"" as EmployeeComment,gr.""ManagerRating"" as ManagerRating,gr.""Id"" as ReviewId,g.""Id"" as GoalId
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
left join cms.""N_PerformanceDocument_PMSStageGoalReview"" as gr on gr.""GoalId"" = g.""Id"" and  gr.""IsDeleted""=false and gr.""CompanyId""='{_userContext.CompanyId}' and gr.""UserId""=gns.""OwnerUserId"" and gr.""StageId""='{stageId}'
            where pds.""IsDeleted""=false and pdns.""Id""='{docServiceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{ownerUserId}' and ss.""Code""='SERVICE_STATUS_COMPLETE'

";

            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query1, null);
            return queryData;

        }
        public async Task<List<TeamWorkloadViewModel>> GetAllApprovedCompetencies(string ownerUserId, string docServiceId, string stageId)
        {

            //            var query = @$"
            //               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""CompetencyName"" as StageName,s.""TemplateCode"" as TemplateCode,'{stageId }' as StageId,
            //              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""CompetencyStartDate"" as ""StartDate"",pg.""CompetencyEndDate"" as ""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId,gr.""EmployeeRating"",gr.""EmployeeComment"" as EmployeeComment,gr.""ManagerRating"" as ManagerRating
            //,gr.""ManagerComment"" as ManagerComment,gr.""Id"" as ReviewId,pg.""Id"" as CompetencyId
            //                              		 FROM public.""NtsService"" as s 
            // join cms.""N_PerformanceDocument_PMSCompentency"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
            //									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
            //                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
            //                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
            // left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
            //        left join cms.""N_PerformanceDocument_PMSStageCompetencyReview"" as gr on gr.""CompetencyId"" = pg.""Id"" and  gr.""IsDeleted""=false and gr.""CompanyId""='{_userContext.CompanyId}' and gr.""UserId""=s.""OwnerUserId"" and gr.""StageId""='{stageId}'  
            //where s.""ParentServiceId""='{docServiceId}' and s.""TemplateCode""='PMS_COMPENTENCY' and s.""OwnerUserId""='{ownerUserId}' and ss.""Code""='SERVICE_STATUS_COMPLETE'
            //           and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";

            var query1 = $@"
select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""CompetencyName"" as StageName,gns.""TemplateCode"" as TemplateCode,'{stageId }' as StageId,
            gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            ,g.""CompetencyStartDate"" as ""StartDate"",g.""CompetencyEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,gr.""ManagerComment"" as ManagerComment
            ,gr.""EmployeeRating"",gr.""EmployeeComment"" as EmployeeComment,gr.""ManagerRating"" as ManagerRating,gr.""Id"" as ReviewId,g.""Id"" as CompetencyId
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
left join cms.""N_PerformanceDocument_PMSStageCompetencyReview"" as gr on gr.""CompetencyId"" = g.""Id"" and  gr.""IsDeleted""=false and gr.""CompanyId""='{_userContext.CompanyId}' and gr.""UserId""=gns.""OwnerUserId"" and gr.""StageId""='{stageId}'
            where pds.""IsDeleted""=false and pdns.""Id""='{docServiceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{ownerUserId}' and ss.""Code""='SERVICE_STATUS_COMPLETE'

";

            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query1, null);
            return queryData;


        }

        public async Task<List<TeamWorkloadViewModel>> GetAllStageGoals(string ownerUserId, string docServiceId, string stageId, DateTime? startDate, DateTime? endDate)
        {
            //var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" where ""PerformanceDocumentId""='{perDocId}' ";
            var query = @$"
               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""GoalName"" as StageName,s.""TemplateCode"" as TemplateCode,'{stageId }' as StageId,
              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",g.""GoalStartDate"" as StartDate,g.""GoalEndDate"" as DueDate,s.""ParentServiceId"",r.""Id"" as RequestedByUserId,g.""SuccessCriteria"" as SuccessCriteria,gr.""EmployeeRating"",gr.""EmployeeComment"" as EmployeeComment,gr.""ManagerRating"" as ManagerRating
,gr.""ManagerComment"" as ManagerComment,gr.""Id"" as ReviewId,g.""Id"" as GoalId
                              		 FROM public.""NtsService"" as s 
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'   
left join cms.""N_PerformanceDocument_PMSGoal"" as g on g.""Id"" = s.""UdfNoteTableId"" and  g.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}' 
left join cms.""N_PerformanceDocument_PMSStageGoalReview"" as gr on gr.""GoalId"" = g.""Id"" and  gr.""IsDeleted""=false and gr.""CompanyId""='{_userContext.CompanyId}' and gr.""UserId""=s.""OwnerUserId"" and gr.""StageId""='{stageId}'
                where  s.""ParentServiceId""='{docServiceId}' and s.""TemplateCode""='PMS_GOAL' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' 
and g.""GoalStartDate"">='{startDate}' and g.""GoalStartDate""<='{endDate}'  #WHERE#
";
            var search = "";
            if (ownerUserId.IsNotNullAndNotEmpty())
            {
                search = $@" and s.""OwnerUserId""='{ownerUserId}' ";
            }
            query = query.Replace("#WHERE#", search);
            var goals = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return goals;
        }
        public async Task<List<TeamWorkloadViewModel>> GetAllStageCompetencies(string ownerUserId, string docServiceId, string stageId, DateTime? startDate, DateTime? endDate)
        {
            //var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" where ""PerformanceDocumentId""='{perDocId}' ";
            var compquery = @$"
               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,pg.""CompetencyName"" as StageName,s.""TemplateCode"" as TemplateCode,'{stageId }' as StageId,
              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",pg.""CompetencyStartDate"" as ""StartDate"",pg.""CompetencyEndDate"" as ""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId,gr.""EmployeeRating"",gr.""EmployeeComment"" as EmployeeComment,gr.""ManagerRating"" as ManagerRating
,gr.""ManagerComment"" as ManagerComment,gr.""Id"" as ReviewId,pg.""Id"" as CompetencyId
                              		 FROM public.""NtsService"" as s 
 join cms.""N_PerformanceDocument_PMSCompentency"" as pg on s.""UdfNoteId""=pg.""NtsNoteId""
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and  sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and  r.""IsDeleted""=false and r.""CompanyId""='{_userContext.CompanyId}'
               left join cms.""N_PerformanceDocument_PMSStageCompetencyReview"" as gr on gr.""CompetencyId"" = g.""Id"" and  gr.""IsDeleted""=false and gr.""CompanyId""='{_userContext.CompanyId}' and gr.""UserId""=s.""OwnerUserId"" and gr.""StageId""='{stageId}'    
where s.""ParentServiceId""='{docServiceId}' and s.""TemplateCode""='PMS_COMPENTENCY' and s.""OwnerUserId""='{ownerUserId}' and 
and pg.""CompetencyStartDate"">='{startDate}' and pg.""CompetencyStartDate""<='{endDate}'
           and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";
            var goals = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(compquery, null);
            return goals;
        }

        public async Task<List<ServiceViewModel>> GetPerformanceDocumentServiceOwners(string docMasterId, string users)
        {
            var query = $@"select s.""OwnerUserId"",s.""ServiceStatusId"",lovs.""Code"",s.""Id""
           from public.""NtsService"" as s
           join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as npm on npm.""Id""=s.""UdfNoteTableId"" and npm.""IsDeleted""=false and npm.""CompanyId""='{_userContext.CompanyId}'
           join public.""LOV"" as lovs on lovs.""Id""=s.""ServiceStatusId"" and lovs.""IsDeleted""=false and lovs.""CompanyId""='{_userContext.CompanyId}'
           where npm.""DocumentMasterId""='{docMasterId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' and s.""OwnerUserId"" not in ('{users}')
            and lovs.""Code""='SERVICE_STATUS_INPROGRESS'
 ";


            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }


        public async Task<CommandResult<PerformanceDocumentViewModel>> PublishDocumentMaster(string pdmId)
        {
            var model = await GetPerformanceDocumentDetails(pdmId);
            if (model.IsNotNull())
            {
                // update the Document master 
                var result = await ChangeStatusforDocumentMaster(PerformanceDocumentStatusEnum.Publishing, model);
                if (result.IsSuccess)
                {
                    await GeneratePerformanceDocument(pdmId);
                    // BackgroundJob.Enqueue<HangfireScheduler>(x => x.GeneratePerformanceDocument(pdmId, _userContext.ToIdentityUser()));
                }
                return CommandResult<PerformanceDocumentViewModel>.Instance(model);
            }

            return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Performance Document Does Not Available");
        }

        public async Task<bool> GeneratePerformanceDocumentStage(string docid, string userId, string parentServiceId)
        {
            var documentstage = await GetPerformanceDocumentStageData(docid, null, null);
            documentstage = documentstage.Where(x => x.DocumentStageStatus == PerformanceDocumentStatusEnum.Active && x.StartDate <= DateTime.Today).OrderBy(x => x.StartDate).ToList();
            foreach (var stage in documentstage)
            {
                var existingstage = await GetPerformanceDocumentStageByMaster(parentServiceId, stage.Id);
                if (existingstage == null)
                {
                    var serviceStageTempModel = new ServiceTemplateViewModel();
                    var performanceDocumentStageView = new PerformanceDocumentStageViewModel();
                    // var udfdic = new Dictionary<string, string>();
                    performanceDocumentStageView.Name = stage.Name;
                    performanceDocumentStageView.Description = stage.Description;
                    performanceDocumentStageView.StartDate = stage.StartDate;
                    performanceDocumentStageView.EndDate = stage.EndDate;
                    performanceDocumentStageView.Year = stage.Year;
                    performanceDocumentStageView.DocumentStageStatus = stage.DocumentStageStatus;
                    // performanceDocumentStageView.StageLinkId = stage.StageLinkId;
                    performanceDocumentStageView.DocumentMasterStageId = stage.Id;
                    //  udfdic.Add("Name",stage.Name);
                    // serviceStageTempModel.Udfs = udfdic;
                    serviceStageTempModel.ActiveUserId = _userContext.UserId;
                    serviceStageTempModel.TemplateCode = "PMS_PERFORMANCE_DOCUMENT_STAGE";
                    var serviceStagemodel = await _serviceBusiness.GetServiceDetails(serviceStageTempModel);
                    serviceStagemodel.ParentServiceId = parentServiceId;
                    serviceStagemodel.OwnerUserId = userId;
                    serviceStagemodel.DataAction = DataActionEnum.Create;
                    serviceStagemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    serviceStagemodel.ServiceSubject = stage.Name;
                    serviceStagemodel.ServiceDescription = stage.Description;
                    serviceStagemodel.Json = JsonConvert.SerializeObject(performanceDocumentStageView);
                    var stagecreate = await _serviceBusiness.ManageService(serviceStagemodel);
                    if (!stagecreate.IsSuccess)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public async Task<CommandResult<PerformanceDocumentViewModel>> GeneratePerformanceDocument(string pdmId)
        {
            var model = await GetPerformanceDocumentDetails(pdmId);
            var mappedusers = "";
            if (model.IsNotNull())

            {
                var users = await GetPerformanceDocumentMappedUserData(model.NoteId, null);
                model.DocumentMasterId = model.Id;
                foreach (var user in users)
                {
                    // var exisiting = await _tableMetadataBusiness.GetTableDataByColumn("PMS_PERFORMANCE_DOCUMENT", null, "DocumentMasterId", model.Id);
                    mappedusers += user.OwnerUserId + "','";
                    var exisiting = await GetPerformanceDocumentByMaster(user.OwnerUserId, model.Id);
                    if (exisiting == null)
                    {


                        var serviceTempModel = new ServiceTemplateViewModel();


                        serviceTempModel.ActiveUserId = _userContext.UserId;
                        serviceTempModel.TemplateCode = "PMS_PERFORMANCE_DOCUMENT";
                        var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                        servicemodel.OwnerUserId = user.OwnerUserId;
                        servicemodel.DataAction = DataActionEnum.Create;
                        servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                        servicemodel.ServiceSubject = model.Name;
                        servicemodel.ServiceDescription = model.Description;
                        servicemodel.Json = JsonConvert.SerializeObject(model);
                        var servicecreate = await _serviceBusiness.ManageService(servicemodel);
                        if (servicecreate.IsSuccess)
                        {
                            //bool stagecreation = await GeneratePerformanceDocumentStage(model.NoteId, user.OwnerUserId, servicecreate.Item.ServiceId);
                            //if (!stagecreation)
                            //{
                            //    return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Cannot Generate Stage Document");
                            //}
                        }
                    }

                    else
                    {
                        //bool stagecreation = await GeneratePerformanceDocumentStage(model.NoteId, user.OwnerUserId, exisiting.ServiceId);
                        //if (!stagecreation)
                        //{
                        //    return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Cannot Generate Stage Document");
                        //}
                    }
                }
                mappedusers = mappedusers.TrimEnd(',');
                var servicecreatedusers = await GetPerformanceDocumentServiceOwners(model.Id, mappedusers);
                if (servicecreatedusers.Count > 0)
                {
                    foreach (var createdusers in servicecreatedusers)
                    {
                        var serviceTempModel = new ServiceTemplateViewModel();
                        serviceTempModel.ActiveUserId = _userContext.UserId;
                        serviceTempModel.DataAction = DataActionEnum.Edit;
                        serviceTempModel.ServiceId = createdusers.Id;
                        var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                        servicemodel.OwnerUserId = createdusers.OwnerUserId;
                        servicemodel.ServiceStatusCode = "SERVICE_STATUS_CANCEL";
                        servicemodel.ServiceSubject = model.Name;
                        servicemodel.ServiceDescription = model.Description;
                        servicemodel.Json = JsonConvert.SerializeObject(model);
                        var serviceedited = await _serviceBusiness.ManageService(servicemodel);
                        if (!serviceedited.IsSuccess)
                        {
                            return CommandResult<PerformanceDocumentViewModel>.Instance(model);
                        }
                    }

                }


                await ChangeStatusforDocumentMaster(PerformanceDocumentStatusEnum.Active, model);

            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, true, "Success");
        }


        public async Task updateGoalWeightaged(string Id, string Weightage)
        {

            string query = @$"Update cms.""N_PerformanceDocument_PMSGoal"" set ""Weightage""='{Weightage}' where ""Id""='{Id}'";
            await _queryGoal.ExecuteCommand(query, null);

        }

        public async Task updateCompentancyWeightaged(string Id, string Weightage)
        {

            string query = @$"Update cms.""N_PerformanceDocument_PMSCompentency"" set ""Weightage""='{Weightage}' where ""Id""='{Id}'";
            await _queryGoal.ExecuteCommand(query, null);

        }

        public async Task updateGoalRating(string Id, string RatingId, string type)
        {

            string query = "";


            if (type == "EMP_MID_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSGoal"" set ""EmployeeMidYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            else if (type == "MNG_MID_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSGoal"" set ""ManagerMidYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            else if (type == "EMP_END_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSGoal"" set ""EmployeeEndYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            else if (type == "MNG_END_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSGoal"" set ""ManagerEndYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            if (query.IsNotNullAndNotEmpty())
            {
                await _queryGoal.ExecuteCommand(query, null);
            }

        }

        public async Task updateCompentancyRating(string Id, string RatingId, string type)
        {
            string query = "";

            if (type == "EMP_MID_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSCompentency"" set ""EmployeeMidYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            else if (type == "MNG_MID_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSCompentency"" set ""ManagerMidYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            else if (type == "EMP_END_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSCompentency"" set ""EmployeeEndYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            else if (type == "MNG_END_YEAR")
            {
                query = @$"Update cms.""N_PerformanceDocument_PMSCompentency"" set ""ManagerEndYearRatingId""='{RatingId}' where ""Id""='{Id}'";
            }
            if (query.IsNotNullAndNotEmpty())
            {
                await _queryGoal.ExecuteCommand(query, null);
            }

        }
        public async Task<IList<ServiceViewModel>> ReadPerformanceDocumentGoalData(string performanceId, string userId, string masterStageId)
        {
            var query = @$"select gns.""Id"" as Id,u.""Id"" as OwnerUserId,u.""Name"" as OwnerUserUserName
            --,gns.""ServiceSubject"" as ServiceSubject
            ,g.""GoalName"" as ServiceSubject
            ,sp.""Name"" as ""Priority"",ss.""Name"" as ServiceStatusName
            ,g.""GoalStartDate"" as ""StartDate"",g.""GoalEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,pri.""code"" as ManagerScore
            ,pri.""Name"" as ManagerScoreValue,gr.""ManagerComment"" as ManagerComments
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
            left join cms.""N_PerformanceDocument_PMSStageGoalReview"" as gr on gr.""GoalId""=g.""Id"" and gr.""UserId""=gns.""OwnerUserId"" and gr.""StageId""=pds.""Id"" and  gr.""IsDeleted""=false
            left join cms.""N_General_PerformanceRatingItem"" as pri on pri.""Id""=gr.""ManagerRating"" and pri.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pdns.""OwnerUserId""='{userId}' and pdms.""Id""='{masterStageId}' ";
            var queryData = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<ServiceViewModel>> ReadPerformanceDocumentCompetencyData(string performanceId, string userId, string masterStageId)
        {
            var query = @$"select gns.""Id"" as Id,u.""Id"" as OwnerUserId,u.""Name"" as OwnerUserUserName
            --,gns.""ServiceSubject"" as ServiceSubject
            ,g.""CompetencyName"" as ServiceSubject
            ,sp.""Name"" as ""Priority"",ss.""Name"" as ServiceStatusName
            ,g.""CompetencyStartDate"" as ""StartDate"",g.""CompetencyEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,pri.""code"" as ManagerScore
            ,pri.""Name"" as ManagerScoreValue,gr.""ManagerComment"" as ManagerComments
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_COMPENTENCY' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSCompentency"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""CompetencyStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""CompetencyEndDate""   and g.""IsDeleted""=false
            left join cms.""N_PerformanceDocument_PMSStageCompetencyReview"" as gr on gr.""CompetencyId""=g.""Id"" and gr.""UserId""=gns.""OwnerUserId"" and gr.""StageId""=pds.""Id"" and  gr.""IsDeleted""=false
            left join cms.""N_General_PerformanceRatingItem"" as pri on pri.""Id""=gr.""ManagerRating"" and pri.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
            left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pdns.""OwnerUserId""='{userId}' and pdms.""Id""='{masterStageId}' ";

            var queryData = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            return queryData;
        }

        public async Task TriggerReviewGoal(ServiceTemplateViewModel viewModel)
        {
            try
            {
                DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);

                if (stage != null)
                {
                    string rowValue = stage["DocumentMasterStageId"].ToString();
                    var documentMaster = await GetPerformanceDocumentMasterId(rowValue);
                    var DocumentMasterData = await GetPerformanceDocumentMasterByNoteId(documentMaster.ParentNoteId);
                    //var master = await GetPerformanceDocumentMasterId(documentMaster.ParentNoteId);

                    var users = await GetPerformanceDocumentMappedUserData(documentMaster.ParentNoteId, null);

                    var masterList = await GetPerformanceGradeRatingData(documentMaster.ParentNoteId, null, null);
                    var master = masterList.FirstOrDefault();

                    var documentstagelist = await GetPerformanceDocumentStageData(documentMaster.ParentNoteId, null, rowValue);
                    var documentstage = documentstagelist.FirstOrDefault();
                    foreach (var user in users)
                    {
                        var goalquery = @$"
                        select s.*
                        FROM public.""NtsService"" as s 
                        join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
						left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'                                   
                        where  s.""OwnerUserId""='{user.OwnerUserId}' and s.""ParentServiceId""='{viewModel.ParentServiceId}' and s.""TemplateCode""='PMS_GOAL' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' ";


                        var goals = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(goalquery, null);

                        // var linemanager = await _hrCoreBusiness.GetUserPerformanceDocumentInfo(user.OwnerUserId, viewModel.ParentServiceId);
                        // var line = linemanager.FirstOrDefault();
                        var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(user.OwnerUserId);

                        foreach (var goal in goals)
                        {
                            var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == goal.Id && x.TemplateId == documentstage.ManagerGoalStageTemplateId);

                            if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerGoalStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = line.ManagerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = goal.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);

                            }

                            var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == goal.Id && x.TemplateId == documentstage.EmployeeGoalStageTemplateId);

                            if (existEmpTask == null /*&& line.IsNotNull()*/)
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeGoalStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = user.OwnerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = goal.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);
                            }


                        }

                        //Trigger Compentency

                        var compquery = @$"
                        select s.*
                        FROM public.""NtsService"" as s 
                        join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
						left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'                                                 
                        where  s.""OwnerUserId""='{user.OwnerUserId}' and s.""ParentServiceId""='{viewModel.ParentServiceId}' and s.""TemplateCode""='PMS_COMPENTENCY'
                          and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";


                        var compentency = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(compquery, null);


                        foreach (var comp in compentency)
                        {
                            var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == comp.Id && x.TemplateId == documentstage.ManagerCompetencyStageTemplateId);

                            if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerCompetencyStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = line.ManagerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = comp.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);
                            }

                            var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == comp.Id && x.TemplateId == documentstage.EmployeeCompetencyStageTemplateId);

                            if (existEmpTask == null /*&& line.IsNotNull()*/)
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeCompetencyStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = user.OwnerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = comp.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task CalculatePerformanceRating(string documentMasterId, string masterStageId)
        {
            var performanceId = "";
            var stageId = "";
            var userId = "";
            var query = $@"
            select gns.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,g.""GoalName"" as StageName,gns.""TemplateCode"" as TemplateCode
            ,gns.""ServiceSubject"" as ServiceSubject,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus""
            ,g.""GoalStartDate"" as ""StartDate"",g.""GoalEndDate""as DueDate,gns.""ParentServiceId""
            ,r.""Id"" as RequestedByUserId,r.""Name"" as RequestedByUserName,gns.""CompletedDate"",gns.""ServiceDescription"",
            g.""Weightage"" as Weightage,g.""SuccessCriteria"" as SuccessCriteria,g.""ManagerComments"" as ManagerComments
            ,g.""EmployeeRating"",g.""EmployeeComments"" as EmployeeComment,g.""ManagerRating"" as ManagerRating
            from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
            join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
            join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
            left join 
            (
	            select  
                from cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds
                join public.""NtsService"" as pdsns on pdsns.""UdfNoteId""=pds.""NtsNoteId"" and pdsns.""IsDeleted""=false
                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on pdms.""Id""=pds.""DocumentMasterStageId"" and pdms.""IsDeleted""=false
                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdms.""DocumentMasterId""=pdm.""Id""  and pdm.""IsDeleted""=false
                join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pdm.""Id""=pd.""DocumentMasterId""  and pd.""IsDeleted""=false
                join public.""NtsService"" as pdns on pdns.""UdfNoteId""=pd.""NtsNoteId""  and pdns.""IsDeleted""=false
                join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
                join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
                (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
                when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
                else pdm.""EndDate"" end)>g.""GoalStartDate"" and
                (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
                when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
                else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
                
                where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'
            ) as goal on s.""Id""=goal.""ParentServiceId""

            join public.""NtsService"" as gns on gns.""ParentServiceId""=pdns.""Id"" and gns.""TemplateCode""='PMS_GOAL' and gns.""OwnerUserId""=pdsns.""OwnerUserId""  and gns.""IsDeleted""=false
            join cms.""N_PerformanceDocument_PMSGoal"" as g on gns.""UdfNoteId""=g.""NtsNoteId"" and 
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""EndDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""EndDate""
            else pdm.""EndDate"" end)>g.""GoalStartDate"" and
            (case when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='1' then pdm.""StartDate""
            when pdms.""EnableReview""='True' and pdms.""PerformanceStageObjective""='2' then pdms.""StartDate""
            else pdm.""StartDate"" end)<g.""GoalEndDate""   and g.""IsDeleted""=false
            left join public.""LOV"" as sp on sp.""Id"" = gns.""ServicePriorityId"" and sp.""IsDeleted""=false 
            left join public.""LOV"" as ss on ss.""Id"" = gns.""ServiceStatusId"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id"" = gns.""OwnerUserId"" and u.""IsDeleted""=false 
             left join public.""User"" as r on r.""Id"" = gns.""RequestedByUserId"" and r.""IsDeleted""=false  
            where pds.""IsDeleted""=false and pdns.""Id""='{performanceId}' and pds.""Id""='{stageId}' and pdns.""OwnerUserId""='{userId}'






with cte as(
            select s.""ServiceNo"",s.""Id"" as ""ServiceId"",pd.""Id"" as ""PerformanceDocumentId""
            ,pdm.""Id"" as ""PerformanceDocumentMasterId"",rsource.""Code"" as ""FinalRatingSourceCode""
            ,coalesce(goal.""Rating"",0) as ""GoalRating"",coalesce(comp.""Rating"",0) as ""CompRating""
            ,((coalesce(goal.""Rating"",0))*((coalesce(cast(pd.""GoalWeightage"" as float),0))/100.00))+
            ((coalesce(comp.""Rating"",0))*((coalesce(cast(pd.""CompetencyWeightage"" as float),0))/100.00)) as ""CalculatedRating""
            FROM public.""NtsService"" as s
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false 
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id"" = pd.""DocumentMasterId"" and pdm.""IsDeleted""=false 
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false 
            left join public.""LOV"" as rsource on rsource.""Id"" =pdm.""FinalRatingSourceId"" and rsource.""IsDeleted""=false 
            left join 
            (
	            select child.""ParentServiceId"",sum((coalesce(cast(udf.""Weightage"" as float),0))*((coalesce(cast(mResult.""code"" as float),0)/100.00))) as ""Rating""   
	            from public.""NtsService"" as child 
	            join cms.""N_PerformanceDocument_PMSGoal"" as udf on udf.""Id""=child.""UdfNoteTableId""  and udf.""IsDeleted""=false 
                
                join public.""Template"" as ctt on ctt.""Id"" =child.""TemplateId"" and ctt.""Code""='PMS_GOAL'  and ctt.""IsDeleted""=false 
	            join public.""LOV"" as lov on lov.""Id""=child.""ServiceStatusId"" and lov.""Code""<> 'SERVICE_STATUS_DRAFT' 
	            and lov.""Code""<> 'SERVICE_STATUS_CANCEL'  and lov.""IsDeleted""=false 
                --left join cms.""N_PerformanceDocument_PMSGoalResult"" as result on udf.""Id""=result.""GoalId""  and result.""IsDeleted""=false                
                --left join cms.""N_General_PerformanceRatingItem"" eResult on result.""EmployeeRating""=eResult.""Id""  and eResult.""IsDeleted""=false   
                left join cms.""N_General_PerformanceRatingItem"" as mResult on udf.""ManagerRating""=mResult.""Id""  and mResult.""IsDeleted""=false   
                where   child.""IsDeleted""=false 
                group by child.""ParentServiceId""
            ) as goal on s.""Id""=goal.""ParentServiceId""
            left join 
            (
	            select child.""ParentServiceId"",sum((coalesce(cast(udf.""Weightage"" as float),0))*((coalesce(cast(mResult.""code"" as float),0)/100.00))) as ""Rating""   
	            from public.""NtsService"" as child 
	            join cms.""N_PerformanceDocument_PMSCompentency"" as udf on udf.""Id""=child.""UdfNoteTableId"" and udf.""IsDeleted""=false  
	            join public.""Template"" as ctt on ctt.""Id"" =child.""TemplateId"" and ctt.""Code""='PMS_COMPENTENCY' and ctt.""IsDeleted""=false  
	            join public.""LOV"" as lov on lov.""Id""=child.""ServiceStatusId"" and lov.""Code""<> 'SERVICE_STATUS_DRAFT'
	            and lov.""Code""<> 'SERVICE_STATUS_CANCEL' and lov.""IsDeleted""=false  
	            --left join cms.""N_PerformanceDocument_PMSCompetencyResult"" as result on udf.""Id""=result.""CompetencyId""  and result.""IsDeleted""=false                
                --left join cms.""N_General_PerformanceRatingItem"" eResult on result.""EmployeeRating""=eResult.""Id""  and eResult.""IsDeleted""=false   
                left join cms.""N_General_PerformanceRatingItem"" as mResult on udf.""ManagerRating""=mResult.""Id""  and mResult.""IsDeleted""=false 
                where   child.""IsDeleted""=false 
                group by child.""ParentServiceId""
            ) as comp on s.""Id""=comp.""ParentServiceId""
            where pd.""DocumentMasterId""='{documentMasterId}' and s.""IsDeleted""=false 
            ) update cms.""N_PerformanceDocument_PMSPerformanceDocument"" 
            SET ""CalculatedGoalRating"" = cte.""GoalRating"",
            ""CalculatedCompetencyRating"" = cte.""CompRating"",
            ""CalculatedRating"" = cte.""CalculatedRating"",
            ""CalculatedRatingRounded"" =round(cast(cte.""CalculatedRating"" as decimal),0),
            ""FinalRating""=coalesce(case when cte.""FinalRatingSourceCode""='ADJUSTED_RATING' then cast(cte.""CalculatedRating"" as decimal)
            else  cast(cte.""CalculatedRating"" as decimal) end,0),
            ""FinalRatingRounded""=round(coalesce(case when cte.""FinalRatingSourceCode""='ADJUSTED_RATING' then cast(cte.""CalculatedRating"" as decimal)
            else  cast(cte.""CalculatedRating"" as decimal) end,0),0)
            from cte
            where cte.""PerformanceDocumentId""=cms.""N_PerformanceDocument_PMSPerformanceDocument"".""Id""";
            await _queryPDRepo.ExecuteCommand(query, null);
        }

        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceFinalReport(string documentMasterId, string departmentId = null, string userId = null)
        {
            var query = $@"select pd.*,cast(pd.""FinalRating""as float) as ""RatingCode"" --cast(ratingItem.""code"" as float) as ""RatingCode"",ratingItem.""Name"" as ""RatingName"" 
            FROM public.""NtsService"" as s
            join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false  
            join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id"" = pd.""DocumentMasterId"" and pdm.""IsDeleted""=false  
            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false  
            --join cms.""N_General_PerformanceRatingItem"" as ratingItem on ratingItem.""Id""=pd.""FinalRating"" and ratingItem.""IsDeleted""=false 
            join  public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false  
            join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false  
            join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false 
            left join public.""LOV"" as lovs on lovs.""Id""=assi.""AssignmentStatusId"" and lovs.""IsDeleted""=false 
            join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false 
            join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false 
            where pdm.""Id""='{documentMasterId}' ";
            if (departmentId.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and hd.""Id""='{departmentId}'";
            }
            if (userId.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and u.""Id""='{userId}'";
            }
            var result = await _queryPDRepo.ExecuteQueryList<PerformanceDocumentViewModel>(query, null);
            return result;
        }

        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentDetailsData(string DocumentMasterId, string deptId = null, string userId = null, string pdmStageId = null)
        {
            var query = $@" SELECT distinct s.""Id"",u.""Name"" as OwnerUserName,u.""Id"" as UserId,pdm.""Year"",pdm.""Name"" as MasterName,ss.""Name"" as ServiceStatusName,pdms.""Name"" as Name,hj.""JobTitle"" as JobName, hd.""DepartmentName"" as DepartmentName,hd.""Id"" as DepartmentId
,hp.""PersonNo"" as EmployeeNo,s.""ServiceSubject"",ss.""Name"" as ServiceStatusName,count(""goal"") as TotalGoal,count(""competency"") as TotalCompetency
		,goal.""weightage"" as TotalGoalWeightage,competency.""weightage"" as TotalCompetencyWeightage,competency.""NoOfMidYearReviewCompleted"" as  NoOfMidYearReviewCompletedCompetency,
		   competency.""NoOfEndYearReviewCompleted"" as NoOfEndYearReviewCompletedCompetency,goal.""NoOfMidYearReviewCompleted""  as NoOfMidYearReviewCompletedGoal,
		   goal.""NoOfEndYearReviewCompleted"" as NoOfEndYearReviewCompletedGoal,pds.""CalculatedRating"" as CalculatedRating,pds.""CalculatedRatingRounded"" as CalculatedRatingRounded,pds.""AdjustedRating"" as AdjustedRating,
pds.""AdjustedRatingRounded"" as AdjustedRatingRounded
,pds.""FinalRating"" as FinalRating,
pds.""FinalRatingRounded"" as FinalRatingRounded
,pdms.""Id"" as MasterStageId,pdms.""Name"" as MasterStageName                 
                          FROM public.""NtsService"" as s
  join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd on pd.""Id""=s.""UdfNoteTableId"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'

                       join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id"" = pd.""DocumentMasterId"" and pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'
                        join public.""NtsNote"" as note on pdm.""NtsNoteId""=note.""ParentNoteId"" and note.""IsDeleted""=false and note.""CompanyId""='{_userContext.CompanyId}'
                       join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on note.""Id"" = pdms.""NtsNoteId"" and pdms.""IsDeleted"" = false and pdms.""CompanyId""='{_userContext.CompanyId}'
                        join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pdms.""Id""=pds.""DocumentMasterStageId"" and pds.""IsDeleted""=false and pds.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                                                         join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
left join (select child.""ParentServiceId"",sum(cast(udf.""Weightage"" as float)) as weightage,mr.""Number"" as ""NoOfMidYearReviewCompleted"",
		   er.""Number"" as ""NoOfEndYearReviewCompleted""  from public.""NtsService"" as child 
join cms.""N_PerformanceDocument_PMSGoal"" as udf on udf.""NtsNoteId""=child.""UdfNoteId"" and udf.""IsDeleted""=false and udf.""CompanyId""='{_userContext.CompanyId}'
join public.""Template"" as ctt on ctt.""Id"" =child.""TemplateId"" and ctt.""Code""='PMS_GOAL' and ctt.""IsDeleted""=false and ctt.""CompanyId""='{_userContext.CompanyId}' 
		   join public.""LOV"" as lov on lov.""Id""=child.""ServiceStatusId"" and (lov.""Code""<> 'SERVICE_STATUS_DRAFT'
																				 and lov.""Code""<> 'SERVICE_STATUS_CANCEL') and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
		   left join cms.""N_PerformanceDocument_PMSGoalMidYearManager"" as md on md.""Id""=udf.""ManagerMidYearRatingId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_Performance_Rating"" as mr on mr.""Id""=md.""RatingId"" and mr.""Number"" is not null and mr.""Number"">'0' and mr.""IsDeleted""=false and mr.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSGoalEndYearManager"" as ed on ed.""Id""=udf.""ManagerEndYearRatingId"" and ed.""IsDeleted""=false and ed.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_Performance_Rating"" as er on er.""Id""=ed.""RatingId"" and er.""Number"" is not null and er.""Number"">'0' and er.""IsDeleted""=false and er.""CompanyId""='{_userContext.CompanyId}'
		   group by child.""ParentServiceId"",udf.""NtsNoteId"",mr.""Number"",er.""Number""
		  )as ""goal"" on  ""goal"".""ParentServiceId""=s.""Id""
left join (select child.""ParentServiceId"",sum(cast(udf.""Weightage"" as int)) as weightage,mr.""Number"" as ""NoOfMidYearReviewCompleted"",
		   er.""Number"" as ""NoOfEndYearReviewCompleted""
		     from public.""NtsService"" as child 
 join cms.""N_PerformanceDocument_PMSCompentency"" as udf on udf.""NtsNoteId""=child.""UdfNoteId"" and udf.""IsDeleted""=false and udf.""CompanyId""='{_userContext.CompanyId}'
join public.""Template"" as ctt on ctt.""Id"" =child.""TemplateId"" and ctt.""Code""='PMS_COMPENTENCY'  and ctt.""IsDeleted""=false and ctt.""CompanyId""='{_userContext.CompanyId}'
		   join public.""LOV"" as lov on lov.""Id""=child.""ServiceStatusId"" and (lov.""Code""<> 'SERVICE_STATUS_DRAFT'
																				 and lov.""Code""<> 'SERVICE_STATUS_CANCEL') and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
		   left join cms.""N_PerformanceDocument_PMSCompentencyMidYearManager"" as md on md.""Id""=udf.""ManagerMidYearRatingId"" and md.""IsDeleted""=false and md.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_Performance_Rating"" as mr on mr.""Id""=md.""RatingId"" and mr.""Number"" is not null and mr.""Number"">'0' and mr.""IsDeleted""=false and mr.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSCompentencyEndYearManager"" as ed on ed.""Id""=udf.""ManagerEndYearRatingId"" and ed.""IsDeleted""=false and ed.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_Performance_Rating"" as er on er.""Id""=ed.""RatingId"" and er.""Number"" is not null and er.""Number"">'0' and er.""IsDeleted""=false and er.""CompanyId""='{_userContext.CompanyId}'
		   group by child.""ParentServiceId"",udf.""NtsNoteId"",mr.""Number"",er.""Number""
		  )as ""competency"" on  ""competency"".""ParentServiceId""=s.""Id""
where   tt.""Code""='PMS_PERFORMANCE_DOCUMENT' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
and pdm.""NtsNoteId""='{DocumentMasterId}' and pdms.""Id""='{pdmStageId}'
  group by s.""Id"",hj.""JobTitle"",ss.""Name"", hd.""DepartmentName"" ,hp.""PersonNo"",ss.""Name"",goal.""weightage"",competency.""weightage"",
competency.""NoOfMidYearReviewCompleted"" ,pdm.""Name"",u.""Name"",u.""Id"",hd.""Id"",
		   competency.""NoOfEndYearReviewCompleted"" ,goal.""NoOfMidYearReviewCompleted"" ,
		   goal.""NoOfEndYearReviewCompleted"" ,pds.""CalculatedRating"" ,pds.""CalculatedRatingRounded"" ,pds.""AdjustedRating"" ,
pds.""AdjustedRatingRounded"" 
,pds.""FinalRating"" ,
pds.""FinalRatingRounded"", pdms.""Name"",pdm.""Year"",pdms.""Id"",pdms.""Name""

";
            var queryData = await _queryPDRepo.ExecuteQueryList<PerformanceDocumentViewModel>(query, null);
            foreach (var data in queryData)
            {
                var comment = "";
                if (data.TotalGoalWeightage != 100)
                {
                    comment = comment + "Total goal weightage not equal to 100.</br>";
                }
                if (data.TotalCompetencyWeightage != 100)
                {
                    comment = comment + "Total competency weightage not equal to 100.</br>";
                }
                if (data.TotalGoal.ToString() != data.NoOfEndYearReviewCompletedGoal)
                {
                    comment = comment + "No. of end year review completed for goal does not equal to total goal.</br>";
                }
                if (data.TotalCompetency.ToString() != data.NoOfEndYearReviewCompletedGoal)
                {
                    comment = comment + "No. of end year review completed for competency does not equal to total competency.</br>";
                }
                data.ReadyForPerformanceRating = comment;
            }
            return queryData;
        }



        public async Task<PerformaceRatingViewModel> GetPerformanceRatingDetails(string Id)
        {
            var query = $@"Select PR.""Status"", *, ""NtsNoteId"" as NoteId from cms.""N_General_PerformanceRating"" as PR
                             inner join public.""NtsNote"" as N on PR.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
                             where PR.""Id""='{Id}'and PR.""IsDeleted""=false and PR.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryPerformanceRatingRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<PerformanceRatingItemViewModel> GetPerformanceRatingItemDetails(string Id)
        {
            var query = $@"Select PR.""Status"",  *, ""NtsNoteId"" as NoteId from cms.""N_General_PerformanceRatingItem"" as PR 
               inner join public.""NtsNote"" as N on PR.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
                where PR.""Id""='{Id}' and PR.""IsDeleted""=false and PR.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryPerformanceRatingitemRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task DeletePerformanceRating(string Id)
        {
            var query = $@"Update cms.""N_General_PerformanceRating"" set ""IsDeleted""='True' where ""Id""='{Id}'";
            await _queryPerformanceRatingRepo.ExecuteCommand(query, null);
        }


        public async Task DeletePerformanceRatingItem(string Id)
        {
            var query = $@"Update cms.""N_General_PerformanceRatingItem"" set ""IsDeleted""='True' where ""Id""='{Id}'";
            await _queryPerformanceRatingRepo.ExecuteCommand(query, null);
        }

        public async Task<List<PerformanceRatingItemViewModel>> GetPerformanceRatingItemList(string ParentNodeId)
        {
            var query = $@"Select N.""Status"", T.""Name"",T.""code"",T.""NtsNoteId"",T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_PerformanceRatingItem"" as T 
            inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where N.""ParentNoteId"" = '{ParentNodeId}'  and T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryPerformanceRatingitemRepo.ExecuteQueryList<PerformanceRatingItemViewModel>(query, null);
            return queryData;
        }


        public async Task<List<PerformaceRatingViewModel>> GetPerformanceRatingList()
        {
            var query = $@"Select T.""Name"",T.""NtsNoteId"" as NtsNoteIds,T.""Id"" as Id,N.""Status"",N.""NoteSubject"" from cms.""N_General_PerformanceRating"" as T 
            inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where  T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryPerformanceRatingRepo.ExecuteQueryList<PerformaceRatingViewModel>(query, null);
            return queryData;
        }

        public async Task<PerformanceRatingItemViewModel> IsRatingItemExist(string Parentid, string Name, string Id)
        {
            var query = $@"Select  T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_PerformanceRatingItem"" as T
                          inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where N.""ParentNoteId"" = '{Parentid}' and  T.""Name""='{Name}'  and T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and T.""Id"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryPerformanceRatingitemRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<PerformanceRatingItemViewModel> IsRatingItemCodeExist(string Parentid, string code, string Id)
        {
            var query = $@"Select  T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_PerformanceRatingItem"" as T
                         inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where N.""ParentNoteId"" = '{Parentid}' and  T.""code""='{code}'  and T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and T.""Id"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryPerformanceRatingitemRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<PerformaceRatingViewModel> IsRatingNameExist(string Name, string Id)
        {
            var query = $@"Select  T.""Id"" as Id,N.""NoteSubject"" from cms.""N_General_PerformanceRating"" as T
                        inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where   T.""Name""='{Name}'  and T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and T.""Id"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryPerformanceRatingRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<List<CompetencyCategoryViewModel>> GetPerformanceTaskCompetencyCategory()
        {
            var query = $@"Select  T.* from cms.""N_PerformanceDocument_CompetencyCategory"" as T where T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}'";
            var queryData = await _queryCompeencyCategory.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<List<CompetencyViewModel>> GetPerformanceTaskCompetencyMaster(string templateCode, string categoryCode)
        {
            var query = $@"SELECT cm.*, cmj.""ProficiencyLevelId"" as ProficiencyLevelId FROM cms.""N_PerformanceDocument_CompetencyMaster"" as cm
                           inner join public.""NtsNote"" as cn on cn.""Id""=cm.""NtsNoteId""  and cn.""IsDeleted""=false and cn.""CompanyId""='{_userContext.CompanyId}'
                           inner join cms.""N_PerformanceDocument_CompetencyCategory"" as cc on cc.""NtsNoteId""=cn.""ParentNoteId"" and cc.""IsDeleted""=false and cc.""CompanyId""='{_userContext.CompanyId}'
                           inner join cms.""N_PerformanceDocument_CompetencyMasterJob"" as cmj on cmj.""MasterId""=cm.""Id"" and cmj.""IsDeleted""=false and cmj.""CompanyId""='{_userContext.CompanyId}'
                           inner join public.""LOV"" as lov on lov.""Id""=cmj.""ProficiencyLevelId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
                            Where 1=1 #TemplateWhere# #CategoryWhere# and cm.""IsDeleted""=false and cm.""CompanyId""='{_userContext.CompanyId}' ";
            var templateWhere = "";
            //if (templateCode.IsNotNullAndNotEmpty())
            //{
            //    templateWhere = $@" and M.""CompetencyCode""='{templateCode}' ";
            //}
            query = query.Replace("#TemplateWhere#", templateWhere);

            var categoryWhere = "";
            if (categoryCode.IsNotNullAndNotEmpty())
            {
                categoryWhere = $@" and cc.""CategoryCode""='{categoryCode}' ";
            }
            query = query.Replace("#CategoryWhere#", categoryWhere);

            var queryData = await _queryComp.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<CompetencyCategoryViewModel> IsCompetencyCategoryNameExist(string Name, string Id)
        {
            var query = $@"Select  T.""Id"" as Id,N.""NoteSubject"" from cms.""N_PerformanceDocument_CompetencyCategory"" as T 
                          inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where   T.""CategoryName""='{Name}'  and T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and T.""Id"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryCompeencyCategory.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<CompetencyCategoryViewModel> IsCompetencyCategoryCodeExist(string Code, string Id)
        {
            var query = $@"Select  T.""Id"" as Id,N.""NoteSubject"" from cms.""N_PerformanceDocument_CompetencyCategory"" as T
                      inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where   T.""CategoryCode""='{Code}'  and T.""IsDeleted""=false and T.""CompanyId""='{_userContext.CompanyId}' #IdWhere# ";

            var where = "";
            if (Id.IsNotNullAndNotEmpty())
            {
                where = $@" and T.""Id"" !='{Id}' ";
            }
            query = query.Replace("#IdWhere#", where);
            var queryData = await _queryCompeencyCategory.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<CompetencyCategoryViewModel> GetcompetencyCategoryDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_PerformanceDocument_CompetencyCategory"" as TC 
                         inner join public.""NtsNote"" as N on TC.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
                           where TC.""Id""='{Id}'and TC.""IsDeleted""=false and TC.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryCompeencyCategory.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetParentCompatencyCategory()
        {
            var query = $@"select ""NtsNoteId"" as Id,""CategoryName"" as Name from   cms.""N_PerformanceDocument_CompetencyCategory"" where  ""IsDeleted""='false' and ""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryCompeencyCategory.ExecuteQueryList<IdNameViewModel>(query, null);

            var list = new List<IdNameViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return queryData;
        }


        public async Task<CompetencyCategoryViewModel> IsParentAssignToCompetencyCategoryExist(string ParentId, string Id)
        {
            var query = $@"select * from cms.""N_PerformanceDocument_CompetencyCategory"" As TC
                        inner join public.""NtsNote"" as N on TC.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	         where N.""ParentNoteId""='{Id}' and TC.""NtsNoteId""='{ParentId}'  and TC.""IsDeleted""=false and TC.""CompanyId""='{_userContext.CompanyId}'";

            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _queryCompeencyCategory.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task DeleteCompetencyCategory(string Id)
        {
            var query = $@"Update cms.""N_PerformanceDocument_CompetencyCategory"" set ""IsDeleted""='True' where ""Id""='{Id}'";
            await _queryCompeencyCategory.ExecuteCommand(query, null);
        }

        public async Task<IList<CompetencyCategoryViewModel>> GetCompetencyMaster()
        {
            var query = $@"SELECT cm.""CompetencyName"",cm.""Id"" as MasterId,cc.""CategoryName"" FROM cms.""N_PerformanceDocument_CompetencyMaster"" as cm
left join public.""NtsNote"" as notem on notem.""Id""=cm.""NtsNoteId"" and notem.""IsDeleted""=false and notem.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as notec on notec.""Id""=notem.""ParentNoteId"" and notec.""IsDeleted""=false and notec.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_CompetencyCategory"" as cc on cc.""NtsNoteId""=notec.""Id"" and cc.""IsDeleted""=false and cc.""CompanyId""='{_userContext.CompanyId}'
where cm.""IsDeleted""=false and cm.""CompanyId""='{_userContext.CompanyId}'";


            var queryData = await _queryCompeencyCategory.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<CompetencyCategoryViewModel>> ReadCompetencyMasterJob(string noteid)
        {
            var query = $@"SELECT cm.""CompetencyName"",j.""JobTitle"",cc.""CategoryName"",cmj.""NtsNoteId"" as NoteId,lov.""Name"" as ProficiencyLevelName,
cmj.""MasterId"" as MasterId,cmj.""JobId"" as JobId,cmj.""ProficiencyLevelId"" as ProficiencyLevelId
FROM cms.""N_PerformanceDocument_CompetencyMasterJob"" as cmj
join cms.""N_PerformanceDocument_CompetencyMaster"" as cm on cm.""Id""=cmj.""MasterId"" and cm.""IsDeleted""=false and cm.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on cmj.""JobId""=j.""Id"" and cmj.""IsDeleted""=false and cmj.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as notem on notem.""Id""=cm.""NtsNoteId"" and notem.""IsDeleted""=false and notem.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as notec on notec.""Id""=notem.""ParentNoteId"" and notec.""IsDeleted""=false and notec.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_CompetencyCategory"" as cc on cc.""NtsNoteId""=notec.""Id"" and cc.""IsDeleted""=false and cc.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as lov on lov.""Id""=cmj.""ProficiencyLevelId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
where cmj.""IsDeleted""=false and cmj.""CompanyId""='{_userContext.CompanyId}' #NOTE#
 ";


            var where = "";
            if (noteid.IsNotNullAndNotEmpty())
            {
                where = $@" and cmj.""NtsNoteId""='{noteid}' ";
            }
            query = query.Replace("#NOTE#", where);
            var queryData = await _queryCompeencyCategory.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<CompetencyViewModel>> GetCompetencyData(string parentNoteId, string udfNoteId = null, string noteId = null)
        {
            var query = $@"SELECT c.*, N.""Id"" as NoteId, N.""ParentNoteId"" as ParentNoteId,lv.""Name"" as CompetencyType,c.""CompetencyType"" as CompetencyTypeId
                            FROM  public.""NtsNote"" N
                            inner join cms.""N_PerformanceDocument_CompetencyMaster"" c on  N.""Id"" =c.""NtsNoteId"" and c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as lv on lv.""Id""=c.""CompetencyType"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_userContext.CompanyId}'
                            where N.""ParentNoteId""='{parentNoteId}' and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}' #NOTEIDWHERE# #IDWHERE#";

            string noteidwhere = "";
            if (noteId.IsNotNullAndNotEmpty())
            {
                noteidwhere = $@" and c.""NtsNoteId""='{noteId}'";
            }
            query = query.Replace("#NOTEIDWHERE#", noteidwhere);

            string idwhere = "";
            if (udfNoteId.IsNotNullAndNotEmpty())
            {
                idwhere = $@" and c.""Id""='{udfNoteId}'";
            }
            query = query.Replace("#IDWHERE#", idwhere);

            var queryData = await _queryComp.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<CommandResult<CompetencyViewModel>> CreateComp(CompetencyViewModel model)
        {
            var validateName = await IsCompNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, validateName.Message);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "COMPETENCY_MASTER";
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<CompetencyViewModel>> EditComp(CompetencyViewModel model)
        {
            var validateName = await IsCompNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, validateName.Message);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<bool> DeleteComp(string Id)
        {
            var query = $@"update cms.""N_PerformanceDocument_CompetencyMaster"" set ""IsDeleted""=true where ""Id""='{Id}'";
            await _queryComp.ExecuteCommand(query, null);
            return true;
        }

        public async Task<CommandResult<CompetencyViewModel>> IsCompNameExists(CompetencyViewModel model)
        {
            var ratings = await GetCompetencyData(model.ParentNoteId);
            var nameexist = ratings.Where(x => x.CompetencyName == model.CompetencyName && x.Id != model.Id);
            if (nameexist.Count() > 0)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, "Name Already Exist");
            }
            var codeexist = ratings.Where(x => x.CompetencyCode == model.CompetencyCode && x.Id != model.Id);
            if (codeexist.Count() > 0)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, "Code Already Exist");
            }

            return CommandResult<CompetencyViewModel>.Instance(model, true, "");
        }

        public async Task<List<CompetencyCategoryViewModel>> GetCompotencyDetails()
        {
            var query = $@"select TC.*,""ParentNoteId"" from cms.""N_PerformanceDocument_CompetencyCategory"" As TC
                      inner join public.""NtsNote"" as N on TC.""NtsNoteId""=N.""Id"" and N .""IsDeleted""='false' and N.""CompanyId""='{_userContext.CompanyId}'
	         where TC .""IsDeleted""='false' and TC.""CompanyId""='{_userContext.CompanyId}'";

            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _queryCompeencyCategory.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllPerformanceDocument()
        {
            var query = $@"SELECT ""Id"",  ""NtsNoteId"", ""Name"" FROM cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" where ""IsDeleted"" = 'false' and ""CompanyId""='{_userContext.CompanyId}' ";

            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllDepartment()
        {
            var query = $@"SELECT ""Id"", ""NtsNoteId"", ""DepartmentName"" as Name FROM cms.""N_CoreHR_HRDepartment"" where ""IsDeleted"" = 'false' and ""CompanyId""='{_userContext.CompanyId}'";

            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<PerformanceDashboardViewModel>> GetPerformanceSummaryData(string filter)
        {
            var finalres = new List<PerformanceDashboardViewModel>();
            var query = @$"
               select udf.""Name"" as Name,ss.""Code"" as Status,u.""Id"" as UserId
                              		 FROM public.""NtsService"" as s 
									   join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""Code""<>'SERVICE_STATUS_DRAFT' and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                          join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
 join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as udf on udf.""NtsNoteId"" = s.""UdfNoteId"" and  udf.""IsDeleted""=false and udf.""CompanyId""='{_userContext.CompanyId}'
 
                where s.""TemplateCode""='PMS_PERFORMANCE_DOCUMENT_STAGE' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' ";

            var userhier = await _userHierBusiness.GetPerformanceHierarchyUsers(_userContext.UserId);
            var queryData = await _queryRepo.ExecuteQueryList<PerformanceDashboardViewModel>(query, null);

            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PERFORMANCE_MANAGER"))
            {

            }
            else if (filter == "ALL")
            {
                var ids = new List<string>();
                ids = userhier.Select(x => x.UserId).Distinct().ToList();
                queryData = queryData.Where(x => ids.Contains(x.UserId)).ToList();
            }
            else if (filter == "DIRECT")
            {
                var ids = new List<string>();
                ids = userhier.Where(x => x.Type == "DIRECT").Select(x => x.UserId).Distinct().ToList();
                queryData = queryData.Where(x => ids.Contains(x.UserId)).ToList();
            }
            else if (filter == "INDIRECT")
            {
                var ids = new List<string>();
                ids = userhier.Where(x => x.Type == "INDIRECT").Select(x => x.UserId).Distinct().ToList();
                queryData = queryData.Where(x => ids.Contains(x.UserId)).ToList();
            }
            else
            {
                queryData = new List<PerformanceDashboardViewModel>();
            }



            var result = queryData.Select(x => x.Name).Distinct();
            foreach (var item in result)
            {
                var data = queryData.Where(x => x.Name == item).ToList();
                var performance = new PerformanceDashboardViewModel { Name = item };
                performance.InProgreessCount = data.Where(x => x.Status == "SERVICE_STATUS_INPROGRESS").GroupBy(x => x.UserId).Count();
                performance.OverDueCount = data.Where(x => x.Status == "SERVICE_STATUS_OVERDUE").GroupBy(x => x.UserId).Count();
                performance.CompletedCount = data.Where(x => x.Status == "SERVICE_STATUS_COMPLETE").GroupBy(x => x.UserId).Count();
                performance.CancelledCount = data.Where(x => x.Status == "SERVICE_STATUS_CANCEL").GroupBy(x => x.UserId).Count();
                performance.TotalCount = data.GroupBy(x => x.UserId).Count();
                finalres.Add(performance);
            }

            return finalres;
        }
        public async Task<IList<ServiceViewModel>> GetPerformanceSummaryDetail(string filter, string status, string service)
        {

            var query = @$"
               select s.""Id"" as Id,s.""ServiceSubject"" as ServiceSubject,s.""ServiceNo"" as ServiceNo,s.""StartDate"" as StartDate ,s.""DueDate"" as DueDate,
              ss.""Code"" as ServiceStatusCode,ss.""Name"" as ServiceStatusName,u.""Name"" as OwnerDisplayName,s.""TemplateCode"" as TemplateCode,u.""Id"" as OwnerUserId
                              		 FROM public.""NtsService"" as s 
									   join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""Code""<>'SERVICE_STATUS_DRAFT' and  ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'
                          join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
 join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as udf on udf.""NtsNoteId"" = s.""UdfNoteId"" and  udf.""IsDeleted""=false and udf.""CompanyId""='{_userContext.CompanyId}'
 
                where s.""ServiceSubject""='{service}' and ss.""Code""='{status}' and s.""TemplateCode""='PMS_PERFORMANCE_DOCUMENT_STAGE' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ServiceViewModel>(query, null);
            var userhier = await _userHierBusiness.GetPerformanceHierarchyUsers(_userContext.UserId);
            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PERFORMANCE_MANAGER"))
            {

            }
            else if (filter == "ALL")
            {
                var ids = userhier.Select(x => x.UserId).Distinct();
                queryData = queryData.Where(x => ids.Contains(x.OwnerUserId)).ToList();
            }
            else if (filter == "DIRECT")
            {
                var ids = userhier.Where(x => x.Type == "DIRECT").Select(x => x.UserId).Distinct();
                queryData = queryData.Where(x => ids.Contains(x.OwnerUserId)).ToList();
            }
            else if (filter == "INDIRECT")
            {
                var ids = userhier.Where(x => x.Type == "INDIRECT").Select(x => x.UserId).Distinct();
                queryData = queryData.Where(x => ids.Contains(x.OwnerUserId)).ToList();
            }
            else
            {
                queryData = new List<ServiceViewModel>();
            }

            return queryData;
        }


        public async Task<List<IdNameViewModel>> GetYearByUserId(string userId)
        {
            var query = $@"select distinct pd.""Year"" as Id,pd.""Year"" as Name
FROM cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd
join public.""NtsService"" as s on s.""UdfNoteId""=pd.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
where s.""OwnerUserId""='{userId}' and pd.""IsDeleted"" = 'false' and pd.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }


        public Task GetInboxMenuItem(string id, string userRoleCodes)
        {
            throw new NotImplementedException();
        }

        public Task GetInboxMenuItemByUser(string id, string userRoleCodes)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IdNameViewModel>> GetRatingDetailsFromDocumentMaster(string Id)
        {
            var query = $@" select i.""Id"",i.""Name"" from cms.""N_General_PerformanceRatingItem"" as i
 join public.""NtsNote"" as n on n.""Id""=i.""NtsNoteId"" and n.""IsDeleted""=false
 join cms.""N_General_PerformanceRating"" as r on r.""NtsNoteId""=n.""ParentNoteId"" and r.""IsDeleted""=false
 join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as d on d.""PerformanceRatingId""=r.""Id"" and d.""IsDeleted""=false
 where d.""Id""='{Id}' and i.""IsDeleted""=false
";
            var data = await _queryRepo1.ExecuteQueryList(query, null);
            return data;
        }
        public async Task<List<IdNameViewModel>> GetParentGoalByDepartment(string departmentId)
        {
            var result = new List<IdNameViewModel>();
            var parent = await _hrCoreBusiness.GetParentOrgByOrg(departmentId);
            if (parent != null)
            {
                var query = $@"select pd.""Id"" as Id,s.""ServiceSubject"" as Name
                FROM cms.""N_PerformanceDocument_DepartmentGoal"" as pd
                    join public.""NtsService"" as s on s.""UdfNoteId""=pd.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
                    where pd.""DepartmentId""='{parent.Id}' and pd.""IsDeleted"" = 'false' and pd.""CompanyId""='{_userContext.CompanyId}'";

                result = await _queryRepo1.ExecuteQueryList(query, null);
            }
            return result;
        }
        public async Task<string> GetParentGoal(string goalId)
        {

            var query = $@" WITH RECURSIVE parent AS(
                                select pd.""Id"" as Id,s.""ServiceSubject"" as GoalName,pd.""ParentGoalId"",'P' as ""Type""
                FROM cms.""N_PerformanceDocument_DepartmentGoal"" as pd
                    join public.""NtsService"" as s on s.""UdfNoteId""=pd.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
                    where pd.""Id""='{goalId}'

                     union all

                     select pd.""Id"" as Id,s.""ServiceSubject"" as GoalName,pd.""ParentGoalId"",'C' as ""Type""
                FROM cms.""N_PerformanceDocument_DepartmentGoal"" as pd
                    join parent as p on p.""ParentGoalId""=pd.""Id""
                    join public.""NtsService"" as s on s.""UdfNoteId""=pd.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
                    
                             )
                            select * from parent where ""Type""='C' ";

            var result = await _queryRepo1.ExecuteQueryList<GoalViewModel>(query, null);

            var goals = result.Select(x => x.GoalName);
            var parentGoal = "";
            if (goals.Count() > 0)
            {
                parentGoal = string.Join(" >>", goals);
            }

            return parentGoal;
        }

        public async Task<List<IdNameViewModel>> GetDepartmentGoal(string departmentId)
        {


            var query = $@"select pd.""Id"" as Id,s.""ServiceSubject"" as Name
                FROM cms.""N_PerformanceDocument_DepartmentGoal"" as pd
                    join public.""NtsService"" as s on s.""UdfNoteId""=pd.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
                    where pd.""DepartmentId""='{departmentId}' and pd.""IsDeleted"" = 'false' and pd.""CompanyId""='{_userContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteQueryList(query, null);

            return result;
        }
        public async Task<List<IdNameViewModel>> GetPerformanceMasterByDepatment(string departmentId, string year)
        {

            var query = $@"select pdm.""Name"", pdm.""Id"" as Id 
                            from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm 
                            where pdm.""DepartmentId""='{departmentId}' and pdm.""IsDeleted""=false  #YearWhere#";
            var where = "";
            if (year.IsNotNullAndNotEmpty())
            {
                where = $@" and pdm.""Year""='{year}' ";
            }
            query = query.Replace("#YearWhere#", where);

            var data = await _queryRepo1.ExecuteQueryList(query, null);

            return data;
        }

        public async Task<List<IdNameViewModel>> GetYearByDepartment(string departmentId)
        {
            var query = $@"Select pdm.""Year"" as Name, pdm.""Year"" as Id 
                            from cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm 
                            where pdm.""DepartmentId""='{departmentId}' and pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}' order by pdm.""Year"" ";

            var data = await _queryRepo1.ExecuteQueryList(query, null);

            return data;
        }

        public async Task<List<GoalViewModel>> GetDepartmentGoalByDepartment(string departmentId, string masterId)
        {

            var query = $@"select s.""Id"" as Id,s.""ServiceSubject"" as GoalName,s.""StartDate"" as StartDate,s.""DueDate"" as DueDate,
                pd.""Weightage"" as Weightage,pd.""SuccessCriteria"" as SuccessCriteria,ps.""ServiceSubject"" as ParentGoal,lv.""Name"" as GoalStatus
                ,d.""DepartmentName"" as Department, pdm.""Name"" as PerformanceMaster, pdm.""Year"" as Year
                FROM cms.""N_PerformanceDocument_DepartmentGoal"" as pd
                    left join  cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id""=pd.""PerformanceDocumentMasterId"" 
                    join public.""NtsService"" as s on s.""UdfNoteId""=pd.""NtsNoteId"" and s.""IsDeleted"" = 'false' and s.""CompanyId""='{_userContext.CompanyId}'
                    left join  public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId""
                    left join  cms.""N_PerformanceDocument_DepartmentGoal"" as pg on pg.""Id""=pd.""ParentGoalId"" and pd.""IsDeleted"" = 'false'
                    left join  public.""NtsService"" as ps on ps.""UdfNoteId""=pg.""NtsNoteId"" and ps.""IsDeleted"" = 'false'
                    left join  cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=pd.""DepartmentId"" and d.""IsDeleted"" = 'false'
                    where  pd.""IsDeleted"" = 'false' and pd.""CompanyId""='{_userContext.CompanyId}' #OrgWhere#  #MasterWhere# ";


            var orgwhere = "";
            var materwhere = "";
            if (departmentId.IsNotNullAndNotEmpty())
            {
                orgwhere = $@" and pd.""DepartmentId""='{departmentId}' ";
            }
            query = query.Replace("#OrgWhere#", orgwhere);

            if (masterId.IsNotNullAndNotEmpty())
            {
                materwhere = $@" and pd.""PerformanceDocumentMasterId""='{masterId}' ";
            }
            query = query.Replace("#MasterWhere#", materwhere);

            var result = await _queryRepo1.ExecuteQueryList<GoalViewModel>(query, null);

            return result;
        }
        public async Task<List<IdNameViewModel>> GetDepartmentBasedOnUser()
        {
            var result = new List<IdNameViewModel>();
            if (_userContext.IsSystemAdmin || _userContext.UserRoleCodes.Contains("PERFORMANCE_MANAGER"))
            {
                result = await _hrCoreBusiness.GetAllOrganisation();
            }
            else
            {
                var query = $@"WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",d.""DepartmentName"" as Name
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""IsDeleted""=false and d.""DepartmentOwnerUserId""='{_userContext.UserId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",d.""DepartmentName"" as Name
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                where  h.""IsDeleted""=false
                             )
                            select ""Id"",""DepartmentName"" from Department ";

                result = await _queryRepo1.ExecuteQueryList(query, null);
            }

            return result;
        }
        public async Task TriggerAdhocTasksGoals(ServiceTemplateViewModel viewModel)
        {
            try
            {

                var DocumentMasterData = await GetPerformanceDocumentMasterByDocServiceId(viewModel.ParentServiceId);
                var documentstageList = await GetPerformanceDocumentStageData(DocumentMasterData.NoteId, null, null);
                var stage = documentstageList.Where(x => x.DocumentStageStatus == PerformanceDocumentStatusEnum.Active && x.StartDate <= DateTime.Today).OrderBy(x => x.StartDate).FirstOrDefault();
                if (stage != null)
                {
                    string rowValue = stage.Id;//stage["DocumentMasterStageId"].ToString();                    
                    var masterList = await GetPerformanceGradeRatingData(DocumentMasterData.NoteId, null, null);
                    var master = masterList.FirstOrDefault();

                    //var linemanager = await _hrCoreBusiness.GetUserPerformanceDocumentInfo(viewModel.OwnerUserId, viewModel.ParentServiceId);
                    //var line = linemanager.FirstOrDefault();
                    var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(viewModel.OwnerUserId);
                    var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.ManagerGoalStageTemplateId);
                    if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.ManagerGoalStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = line.ManagerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);

                    }

                    var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.EmployeeGoalStageTemplateId);
                    if (existEmpTask == null /*&& line.IsNotNull()*/)
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.EmployeeGoalStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = viewModel.OwnerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }


                }

            }
            catch (Exception ex)
            {

            }
        }
        public async Task TriggerAdhocTasksCompetency(ServiceTemplateViewModel viewModel)
        {
            try
            {
                var DocumentMasterData = await GetPerformanceDocumentMasterByDocServiceId(viewModel.ParentServiceId);
                var documentstageList = await GetPerformanceDocumentStageData(DocumentMasterData.NoteId, null, null);
                var stage = documentstageList.Where(x => x.DocumentStageStatus == PerformanceDocumentStatusEnum.Active && x.StartDate <= DateTime.Today).OrderBy(x => x.StartDate).FirstOrDefault();
                if (stage != null)
                {
                    string rowValue = stage.Id;
                    var masterList = await GetPerformanceGradeRatingData(DocumentMasterData.NoteId, null, null);
                    var master = masterList.FirstOrDefault();
                    //var documentstagelist = await GetPerformanceDocumentStageData(DocumentMasterData.NoteId, null, rowValue);
                    // var documentstage = documentstagelist.FirstOrDefault();
                    //var linemanager = await _hrCoreBusiness.GetUserPerformanceDocumentInfo(viewModel.OwnerUserId, viewModel.ParentServiceId);
                    //var line = linemanager.FirstOrDefault();
                    var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(viewModel.OwnerUserId);
                    //Trigger Compentency                   
                    var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.ManagerCompetencyStageTemplateId);
                    if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.ManagerCompetencyStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = line.ManagerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }

                    var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.EmployeeCompetencyStageTemplateId);
                    if (existEmpTask == null /*&& line.IsNotNull()*/)
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.EmployeeCompetencyStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = viewModel.OwnerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        //  public async Task TriggerReviewAdhocTasks(PerformanceDocumentStageViewModel masterStage,string userId)
        //  {
        //      try
        //      {
        //          //DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
        //          if (masterStage != null)
        //          {
        //              string rowValue = masterStage.Id;
        //              var documentMaster = await GetPerformanceDocumentMasterId(rowValue);
        //              var DocumentMasterData = await GetPerformanceDocumentMasterByNoteId(documentMaster.ParentNoteId);


        //             // var users = await GetPerformanceDocumentMappedUserData(documentMaster.ParentNoteId, null);

        //              var masterList = await GetPerformanceGradeRatingData(documentMaster.ParentNoteId, null, null);
        //              var master = masterList.FirstOrDefault();

        //              var documentstagelist = await GetPerformanceDocumentStageData(documentMaster.ParentNoteId, null, rowValue);
        //              var documentMasterstage = documentstagelist.FirstOrDefault();
        //              //foreach (var user in users)
        //             // {
        //                  var document = await GetPerformanceDocumentByMaster(userId, DocumentMasterData.Id);
        //                  var documentstage = await GetPerformanceDocumentStageByMaster(document.ServiceId, documentMasterstage.Id);
        //              var goalquery = @$"
        //                  select s.*
        //                  FROM public.""NtsService"" as s 
        //                  join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
        //left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
        //                  left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'                                   
        //                  where  s.""OwnerUserId""='{userId}' and s.""ParentServiceId""='{document.ServiceId}' and s.""TemplateCode""='PMS_GOAL' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' ";


        //              var goals = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(goalquery, null);

        //              // var linemanager = await _hrCoreBusiness.GetUserPerformanceDocumentInfo(user.OwnerUserId, viewModel.ParentServiceId);
        //              // var line = linemanager.FirstOrDefault();
        //              var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(userId);

        //                  foreach (var goal in goals)
        //                  {
        //                      var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == goal.Id && x.TemplateId == documentstage.ManagerReviewTemplate/*.ManagerGoalStageTemplateId*/);

        //                      if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
        //                      {
        //                          var taskTempModel = new TaskTemplateViewModel();
        //                          var tasktemplatecode = await _templateBusiness.GetSingleById(documentMasterstage.ManagerReviewTemplate);
        //                          taskTempModel.TemplateCode = tasktemplatecode.Code;
        //                          var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
        //                          // for line manager line.ManagerUserId
        //                          stepmodel.AssignedToUserId = line.ManagerUserId;
        //                          stepmodel.OwnerUserId = userId;
        //                          //stepmodel.StartDate = viewModel.StartDate;
        //                          //stepmodel.DueDate = viewModel.DueDate;
        //                          stepmodel.DataAction = DataActionEnum.Create;
        //                          stepmodel.ParentServiceId = goal.Id;
        //                          stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
        //                          stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
        //                          //stepmodel.Json = "{}";
        //                          dynamic exo = new System.Dynamic.ExpandoObject();
        //                          if (master.IsNotNull())
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
        //                          }
        //                          if (documentstage.IsNotNull()) 
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("DocumentStageId", documentstage.Id);
        //                          }
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
        //                          stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
        //                          await _taskBusiness.ManageTask(stepmodel);

        //                      }

        //                      var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == goal.Id && x.TemplateId == documentMasterstage.EmployeeReviewTemplate);

        //                      if (existEmpTask == null /*&& line.IsNotNull()*/)
        //                      {
        //                          var taskTempModel = new TaskTemplateViewModel();
        //                          var tasktemplatecode = await _templateBusiness.GetSingleById(documentMasterstage.EmployeeReviewTemplate);
        //                          taskTempModel.TemplateCode = tasktemplatecode.Code;
        //                          var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
        //                          // for line manager line.ManagerUserId
        //                          stepmodel.AssignedToUserId = userId;
        //                          stepmodel.OwnerUserId = userId;
        //                          //stepmodel.StartDate = viewModel.StartDate;
        //                         // stepmodel.DueDate = viewModel.DueDate;
        //                          stepmodel.DataAction = DataActionEnum.Create;
        //                          stepmodel.ParentServiceId = goal.Id;
        //                          stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
        //                          stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
        //                          //stepmodel.Json = "{}";
        //                          dynamic exo = new System.Dynamic.ExpandoObject();
        //                          if (master.IsNotNull())
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
        //                          }
        //                          if (documentstage.IsNotNull())
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("DocumentStageId", documentstage.Id);
        //                          }
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
        //                          stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
        //                          await _taskBusiness.ManageTask(stepmodel);
        //                      }


        //                  }

        //                  //Trigger Compentency

        //                  var compquery = @$"
        //                  select s.*
        //                  FROM public.""NtsService"" as s 
        //                  join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
        //left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
        //                  left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_userContext.CompanyId}'                                                 
        //                  where  s.""OwnerUserId""='{userId}' and s.""ParentServiceId""='{document.ServiceId}' and s.""TemplateCode""='PMS_COMPENTENCY'
        //                    and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'";


        //                  var compentency = await _queryTWRepo.ExecuteQueryList<ServiceViewModel>(compquery, null);


        //                  foreach (var comp in compentency)
        //                  {
        //                      var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == comp.Id && x.TemplateId == documentstage.ManagerReviewTemplate);

        //                      if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
        //                      {
        //                          var taskTempModel = new TaskTemplateViewModel();
        //                          var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerReviewTemplate);
        //                          taskTempModel.TemplateCode = tasktemplatecode.Code;
        //                          var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
        //                          // for line manager line.ManagerUserId
        //                          stepmodel.AssignedToUserId = line.ManagerUserId;
        //                          stepmodel.OwnerUserId = userId;
        //                          //stepmodel.StartDate = viewModel.StartDate;
        //                          //stepmodel.DueDate = viewModel.DueDate;
        //                          stepmodel.DataAction = DataActionEnum.Create;
        //                          stepmodel.ParentServiceId = comp.Id;
        //                          stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
        //                          stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
        //                          //stepmodel.Json = "{}";
        //                          dynamic exo = new System.Dynamic.ExpandoObject();
        //                          if (master.IsNotNull())
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
        //                          }
        //                          if (documentstage.IsNotNull())
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("DocumentStageId", documentstage.Id);
        //                          }
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
        //                          stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
        //                          await _taskBusiness.ManageTask(stepmodel);
        //                      }

        //                      var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == comp.Id && x.TemplateId == documentstage.EmployeeReviewTemplate);

        //                      if (existEmpTask == null /*&& line.IsNotNull()*/)
        //                      {
        //                          var taskTempModel = new TaskTemplateViewModel();
        //                          var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeReviewTemplate);
        //                          taskTempModel.TemplateCode = tasktemplatecode.Code;
        //                          var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
        //                          // for line manager line.ManagerUserId
        //                          stepmodel.AssignedToUserId = userId;
        //                          stepmodel.OwnerUserId = userId;
        //                          //stepmodel.StartDate = viewModel.StartDate;
        //                          //stepmodel.DueDate = viewModel.DueDate;
        //                          stepmodel.DataAction = DataActionEnum.Create;
        //                          stepmodel.ParentServiceId = comp.Id;
        //                          stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
        //                          stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
        //                          //stepmodel.Json = "{}";
        //                          dynamic exo = new System.Dynamic.ExpandoObject();
        //                          if (master.IsNotNull())
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
        //                          }
        //                          if (documentstage.IsNotNull())
        //                          {
        //                              ((IDictionary<String, Object>)exo).Add("DocumentStageId", documentstage.Id);
        //                          }
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
        //                          ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
        //                          stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
        //                          await _taskBusiness.ManageTask(stepmodel);
        //                      }

        //                  }


        //          }
        //      }
        //      catch (Exception ex)
        //      {

        //      }
        //  }


        public async Task<IList<IdNameViewModel>> GetDepartmentList()
        {
            var query = $@"select d.*,d.""Id"" as ""Id"",d.""DepartmentName"" as ""Name""
                            FROM cms.""N_CoreHR_HRDepartment"" as d
                            where d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task MapDepartmentUser(NoteTemplateViewModel viewModel)
        {
            try
            {
                var udfData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                // Get All the  mapped users
                var existingUsers = await GetPerformanceDocumentMappedUserData(viewModel.NoteId, null);
                if (Convert.ToString(udfData["DepartmentId"]).IsNotNullAndNotEmpty())
                {
                    // Get All Users Of Department
                    var departmentUsers = await _hrCoreBusiness.GetUsersInfo(Convert.ToString(udfData["DepartmentId"]));
                    if (departmentUsers.Count > 0)
                    {
                        foreach (var user in departmentUsers)
                        {
                            if (existingUsers.Any(x => x.DepartmentId == Convert.ToString(udfData["DepartmentId"]) && x.OwnerUserId == user.UserId))
                            {
                                // department user already mapped to document Master
                                // Do Nothing
                            }
                            else
                            {
                                var noteTempModel = new NoteTemplateViewModel();
                                noteTempModel.DataAction = DataActionEnum.Create;
                                noteTempModel.ActiveUserId = _userContext.UserId;
                                noteTempModel.TemplateCode = "PERFORMANCE_DOCUMENT_MASTER_USERS";
                                noteTempModel.ParentNoteId = viewModel.NoteId;
                                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                                notemodel.OwnerUserId = user.UserId;
                                notemodel.StartDate = DateTime.Now;
                                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                //notemodel.Json = JsonConvert.SerializeObject(notemodel);
                                var result = await _noteBusiness.ManageNote(notemodel);
                            }
                        }
                    }
                }
                // remove All users which were mapped and now does not belong to this user
                var removeUserList = existingUsers.Where(x => x.DepartmentId != Convert.ToString(udfData["DepartmentId"]));
                if (removeUserList.IsNotNull())
                {
                    // existing user have another users
                    foreach (var user in removeUserList)
                    {
                        var deletemapuser = await _tableMetadataBusiness.DeleteTableDataByHeaderId("PERFORMANCE_DOCUMENT_MASTER_USERS", null, user.Id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<IdNameViewModel>> GetDepartmentListByOrganization(string organizationId)
        {
            var query = $@"WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",d.""DepartmentName"" as ""Name"" ,'Parent' as Type
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""IsDeleted""=false and d.""Id""='{organizationId}'
                              union all
                                 select distinct d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",d.""DepartmentName"" as ""Name"" ,'Child' as Type
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                where  h.""IsDeleted""=false
                             ) select ""ParentId"" as Id, ""Name"" as Name from Department  where Type = 'Child'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllYearFromPerformanceMaster(string departmentId)
        {
            var query = $@"SELECT distinct pdm.""Year"" as ""Id"", pdm.""Year"" as ""Name"" FROM cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm
                           where pdm.""DepartmentId"" = '{departmentId}' ORDER BY ""Id"" ASC";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<List<ServiceViewModel>> GetServiceListByPDMId(string pdmId, string templateCodes, string ownerUserId)
        {
            var query = $@"select s.""Id"",s.""TemplateCode"",ss.""Code"" as ServiceStatusCode,s.""OwnerUserId""
from public.""NtsService"" as s
join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and t.""Code"" in ('{templateCodes.Replace(",", "','")}') and t.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""ParentServiceId""='{pdmId}' and s.""OwnerUserId""= '{ownerUserId}' ";

            var result = await _queryRepo.ExecuteQueryList(query, null);
            return result;

        }
        public async Task TriggerReviewAdhocTasks(ServiceTemplateViewModel viewModel)
        {
            try
            {
                var EmployeeReviewTemplate = await _templateBusiness.GetSingle(x => x.Code == "PMS_EMPLOYEE_REVIEW");
                var ManagerReviewTemplate = await _templateBusiness.GetSingle(x => x.Code == "PMS_EMPLOYEE_REVIEW");
                DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
                if (stage != null)
                {
                    string rowValue = stage["DocumentMasterStageId"].ToString();
                    var documentMaster = await GetPerformanceDocumentMasterId(rowValue);
                    var DocumentMasterData = await GetPerformanceDocumentMasterByNoteId(documentMaster.ParentNoteId);
                    //var users = await GetPerformanceDocumentMappedUserData(documentMaster.ParentNoteId, null);

                    //var masterList = await GetPerformanceGradeRatingData(documentMaster.ParentNoteId, null, null);
                    //var master = masterList.FirstOrDefault();

                    var documentstagelist = await GetPerformanceDocumentStageData(documentMaster.ParentNoteId, null, rowValue);
                    var documentstage = documentstagelist.FirstOrDefault();

                    var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(viewModel.OwnerUserId);
                    var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == ManagerReviewTemplate.Id);
                    if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        //var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerReviewTemplate);
                        taskTempModel.TemplateCode = ManagerReviewTemplate.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = line.ManagerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : ManagerReviewTemplate.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        //if (master.IsNotNull())
                        //{
                        //    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        //}
                        if (viewModel.UdfNoteTableId.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("DocumentStageId", viewModel.UdfNoteTableId);
                        }
                       ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);

                    }

                    var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == EmployeeReviewTemplate.Id);

                    if (existEmpTask == null /*&& line.IsNotNull()*/)
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        //var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeReviewTemplate);
                        taskTempModel.TemplateCode = EmployeeReviewTemplate.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = viewModel.OwnerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : EmployeeReviewTemplate.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        //if (master.IsNotNull())
                        //{
                        //    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        //}
                        if (viewModel.UdfNoteTableId.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("DocumentStageId", viewModel.UdfNoteTableId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }

                    //////Trigger Compentency
                    //if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                    //{
                    //    var taskTempModel = new TaskTemplateViewModel();
                    //    var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerCompetencyStageTemplateId);
                    //    taskTempModel.TemplateCode = tasktemplatecode.Code;
                    //    var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                    //    // for line manager line.ManagerUserId
                    //    stepmodel.AssignedToUserId = line.ManagerUserId;
                    //    stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                    //    stepmodel.StartDate = viewModel.StartDate;
                    //    stepmodel.DueDate = viewModel.DueDate;
                    //    stepmodel.DataAction = DataActionEnum.Create;
                    //    stepmodel.ParentServiceId = viewModel.ServiceId;
                    //    stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                    //    stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                    //    //stepmodel.Json = "{}";
                    //    dynamic exo = new System.Dynamic.ExpandoObject();
                    //    //if (master.IsNotNull())
                    //    //{
                    //    //    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                    //    //}
                    //    if (viewModel.UdfNoteTableId.IsNotNull())
                    //    {
                    //        ((IDictionary<String, Object>)exo).Add("DocumentStageId", viewModel.UdfNoteTableId);
                    //    }
                    //    ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                    //    ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                    //    stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    //    await _taskBusiness.ManageTask(stepmodel);
                    //}
                    //if (existEmpTask == null /*&& line.IsNotNull()*/)
                    //{
                    //    var taskTempModel = new TaskTemplateViewModel();
                    //    var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeCompetencyStageTemplateId);
                    //    taskTempModel.TemplateCode = tasktemplatecode.Code;
                    //    var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                    //    // for line manager line.ManagerUserId
                    //    stepmodel.AssignedToUserId = viewModel.OwnerUserId;
                    //    stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                    //    stepmodel.StartDate = viewModel.StartDate;
                    //    stepmodel.DueDate = viewModel.DueDate;
                    //    stepmodel.DataAction = DataActionEnum.Create;
                    //    stepmodel.ParentServiceId = viewModel.ServiceId;
                    //    stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                    //    stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                    //    //stepmodel.Json = "{}";
                    //    dynamic exo = new System.Dynamic.ExpandoObject();
                    //    //if (master.IsNotNull())
                    //    //{
                    //    //    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                    //    //}
                    //    ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                    //    ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                    //    if (viewModel.UdfNoteTableId.IsNotNull())
                    //    {
                    //        ((IDictionary<String, Object>)exo).Add("DocumentStageId", viewModel.UdfNoteTableId);
                    //    }
                    //    stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    //    await _taskBusiness.ManageTask(stepmodel);
                    //}
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> GeneratePerformanceDocumentStages(string docMasterStageId)
        {

            var masterStage = await GetPerformanceDocumentMasterStageById(docMasterStageId);
            // update the status as publishing 
            await ChangeStatusforDocumentMasterStage(PerformanceDocumentStatusEnum.Publishing, masterStage);
            var documentstage = await GetPerformanceDocumentStageData(masterStage.ParentNoteId, null, null);
            var users = await GetPerformanceDocumentMappedUserData(masterStage.ParentNoteId, null);


            foreach (var user in users)
            {
                var master = await GetPerformanceDocumentMasterByNoteId(masterStage.ParentNoteId);
                var document = await GetPerformanceDocumentByMaster(user.OwnerUserId, master.Id);
                var existingstage = await GetPerformanceDocumentStageByMaster(document.ServiceId, masterStage.Id);
                if (existingstage == null)
                {
                    var serviceStageTempModel = new ServiceTemplateViewModel();
                    var performanceDocumentStageView = new PerformanceDocumentStageViewModel();
                    // var udfdic = new Dictionary<string, string>();
                    performanceDocumentStageView.Name = masterStage.Name;
                    performanceDocumentStageView.Description = masterStage.Description;
                    performanceDocumentStageView.StartDate = masterStage.StartDate;
                    performanceDocumentStageView.EndDate = masterStage.EndDate;
                    performanceDocumentStageView.Year = masterStage.Year;
                    performanceDocumentStageView.DocumentStageStatus = masterStage.DocumentStageStatus;
                    // performanceDocumentStageView.StageLinkId = stage.StageLinkId;
                    performanceDocumentStageView.DocumentMasterStageId = masterStage.Id;
                    //  udfdic.Add("Name",stage.Name);
                    // serviceStageTempModel.Udfs = udfdic;
                    serviceStageTempModel.ActiveUserId = _userContext.UserId;
                    serviceStageTempModel.TemplateCode = "PMS_PERFORMANCE_DOCUMENT_STAGE";
                    var serviceStagemodel = await _serviceBusiness.GetServiceDetails(serviceStageTempModel);
                    serviceStagemodel.ParentServiceId = document.ServiceId;
                    serviceStagemodel.OwnerUserId = user.OwnerUserId;
                    serviceStagemodel.DataAction = DataActionEnum.Create;
                    serviceStagemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    serviceStagemodel.ServiceSubject = masterStage.Name;
                    serviceStagemodel.ServiceDescription = masterStage.Description;
                    serviceStagemodel.Json = JsonConvert.SerializeObject(performanceDocumentStageView);
                    var stagecreate = await _serviceBusiness.ManageService(serviceStagemodel);
                    if (stagecreate.IsSuccess)
                    {
                        // await TriggerReviewAdhocTasks(stagecreate.Item);
                        existingstage = await GetPerformanceDocumentStageByMaster(document.ServiceId, masterStage.Id);

                    }
                    else
                    {
                        return false;
                    }

                }
                if (existingstage.IsNotNull())
                {
                    var serviceStageTempModel = new ServiceTemplateViewModel();
                    serviceStageTempModel.ServiceId = existingstage.ServiceId;
                    serviceStageTempModel.DataAction = DataActionEnum.Edit;
                    var serviceStagemodel = await _serviceBusiness.GetServiceDetails(serviceStageTempModel);
                    await TriggerReviewAdhocTasks(serviceStagemodel);
                }
            }

            // update status to Active
            // update the status as publishing 
            await ChangeStatusforDocumentMasterStage(PerformanceDocumentStatusEnum.Active, masterStage);
            return true;
        }

        public Task<List<ProjectDashboardChartViewModel>> GetTaskStatus(string userId, string projectId)
        {
            throw new NotImplementedException();
        }
    }
}
