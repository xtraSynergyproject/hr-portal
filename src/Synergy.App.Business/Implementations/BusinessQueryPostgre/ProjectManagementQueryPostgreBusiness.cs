using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class ProjectManagementQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IProjectManagementQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IUserHierarchyBusiness _userHierBusiness;
        public ProjectManagementQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness
            , IUserHierarchyBusiness userHierBusiness
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _userHierBusiness = userHierBusiness;
        }

        public async Task<List<IdNameViewModel>> GetProjectsListData()
        {

            string query = @$"select s.""ServiceSubject"" as Name, s.""Id"" as Id 
                            from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectEmailSetupViewModel>> GetEmailSetupListData()
        {

            string query = @$"select p.*,u.""Name"" as UserName, s.""ServiceSubject"" as ServiceName,p.""Id"" 
                            from public.""ProjectEmailSetup"" as p
                            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            left join public.""NtsService"" as s on s.""Id""=p.""ServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
							where p.""IsDeleted"" = false and p.""CompanyId""='{_uc.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectEmailSetupViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ProjectEmailSetupViewModel>> GetProjectEmailSetupData()
        {
            string query = @$"select concat('""',u.""Name"",'"" ','<',p.""SmtpUserId"",'>') as EmailText,p.*,2 as SequenceOrder
                            from public.""ProjectEmailSetup"" as p
                            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            join public.""NtsService"" as s on s.""Id""=p.""ServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
							where p.""IsDeleted"" = false and u.""Id""='{_uc.UserId}' and p.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectEmailSetupViewModel>(query, null);
            return queryData;
        }

        public async Task<ProjectEmailSetupViewModel> GetSingleProjectEmailSetupData()
        {
            var query1 = $@"select concat('""',u.""Name"",'"" ','<',p.""SmtpUserId"",'>') as EmailText,u.""Id"" as UserId,
                            p.""SmtpUserId"" as SmtpUserId
                            from public.""Company"" as p
                            join public.""User"" as u on u.""CompanyId""=p.""Id"" and u.""Id""='{_uc.UserId}' and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                             where p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}' ";
            var companydata = await _queryRepo.ExecuteQuerySingle<ProjectEmailSetupViewModel>(query1, null);
            return companydata;
        }

        public async Task<List<WorkBoardViewModel>> GetWorkboardTaskListData()
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

        public async Task UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status)
        {
            var query = @$"UPDATE cms.""N_WORKBOARD_WorkBoard""
                                SET ""WorkBoardStatus"" = '{(int)status}'
                                WHERE ""Id"" = '{id}';";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<WorkBoardSectionViewModel> GetWorkBoardSectionData(string sectionId)
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

        public async Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId)
        {
            var query = @$"select *
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
            var query = @$"Update cms.""N_WORKBOARD_WorkBoard"" set ""JsonContent""='{data.JsonContent /*JsonConvert.SerializeObject(data.JsonContent)*/}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_uc.UserId}'
                            where ""Id""='{data.WorkboardId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""SequenceOrder""='{data.SequenceOrder}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_uc.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""SequenceOrder""='{data.SequenceOrder}',""ParentId""='{data.ParentId}',""ColorCode""='{data.ColorCode}',""WorkBoardSectionId""='{data.WorkBoardSectionId}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_uc.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""SequenceOrder""='{data.SequenceOrder}',""ParentId""='{data.ParentId}',""ColorCode""='{data.ColorCode}',""WorkBoardItemShape""='{data.WorkBoardItemShape}',""WorkBoardItemSize""='{data.WorkBoardItemSize}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_uc.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""WorkBoardSectionId""='{data.WorkBoardSectionId}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_uc.UserId}'
                            where ""NtsNoteId""='{data.WorkBoardItemId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId)
        {
            var query = @$"select wb.*,wb.""NtsNoteId"" as NoteId,wbtt.""TemplateTypeName"" as TemplateTypeName, wbtt.""TemplateTypeCode"" as TemplateTypeCode 
                            from cms.""N_WORKBOARD_WorkBoard"" as wb
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

        public async Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string values)
        {
            var query = $@"SELECT wt.""Id"" as TemplateId, wt.""NtsNoteId"" as NoteId, wt.""TemplateDescription"" as TemplateDescription,
                            wt.""SampleContent"" as SamplecContent, wt.""ContentImage"" as ContentImage, wt.""TemplateTypeCode"" as TemplateTypeCode,
                            wt.""TemplateTypeName"" as TemplateTypeName, lov.""Name"" as TemplateDisplayName
                            FROM cms.""N_WORKBOARD_TemplateType"" as wt
                            join public.""LOV"" as lov on lov.""Id"" = wt.""TemplateCategoryId"" 
                            where lov.""IsDeleted""=false 
                                  and lov.""Name"" NOT IN ('Frameworks','Categories','Use Cases', 'Teams')
                                  and wt.""IsDeleted""=false
                                  and ""TemplateDescription"" ~* '({values})' COLLATE ""tr-TR-x-icu"" ";

            var result = await _queryRepo.ExecuteQueryList<WorkBoardTemplateViewModel>(query, null);
            return result;
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

        public async Task<IList<UserViewModel>> GetUserList(string noteId)
        {
            string query = @$"SELECT * ,CONCAT(""Name"",'<',""Email"",'>') as Name
                            FROM public.""User"" as t
                            join public.""UserPortal"" as up on up.""UserId""=t.""Id"" and up.""IsDeleted""=false and up.""PortalId""='{_uc.PortalId}'
                            where t.""Id"" not in (select ""SharedWithUserId"" from public.""NtsNoteShared"" where ""NtsNoteId""= '{noteId}' and ""IsDeleted""=false)
                                and t.""IsDeleted"" = false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }

        public async Task UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId)
        {
            var query = @$"UPDATE cms.""N_WORKBOARD_WorkBoardItem""
                                SET ""WorkBoardId"" = '{workboardId}', ""WorkBoardSectionId"" = '{sectionId}'
                                WHERE ""Id"" = '{workboardItemId}';";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id)
        {
            var query = $@"Select *,""Id"" ""WorkBoardItemId"" from cms.""N_WORKBOARD_WorkBoardItem""
                            where ""IsDeleted"" = false and ""NtsNoteId"" = '{id}' ";
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

        public async Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id)
        {
            var query = $@"Select sec.""Id"" as ""Id"", sec.""SectionName"" as ""SectionName"" 
                            from cms.""N_WORKBOARD_WorkBoardSection"" as sec
                            where sec.""IsDeleted"" = false and sec.""WorkBoardId"" = '{id}' ";
            var result = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return result;
        }

        public async Task<List<WorkBoardViewModel>> GetOtherWorkboardList(WorkBoardstatusEnum status, string id)
        {
            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , wb.""IconFileId"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'
                            and wb.""Id"" != '{id}' ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            return querydata;
        }

        public async Task<IList<ProgramDashboardViewModel>> GetProjectData()
        {

            string query = @$"select  s.""Id"" as Id,ucount.""Count"" as UserCount,
                s.""ServiceSubject"" as Name,t.""Count"" as AllTaskCount,(t.""CompletedCount""::float / t.""Count""::float)*100 as Percentage,
                false as hasChildren,t.""InProgreessCount"" as InProgreessCount,t.""DraftCount"" as DraftCount,t.""CompletedCount"" as CompletedCount,t.""OverDueCount"" as OverDueCount,
t.""PlannedCount"" as PlannedCount,t.""PlannedOverDueCount"" as PlannedOverDueCount,               
--'PROJECT_SUPER_SERVICE' as ParentId,'STAGE' as Type

                s.""ServiceSubject"" as ProjectName,s.""StartDate"" as StartDate,s.""ServiceNo"" as ServiceNo,s.""ServiceDescription"" Description,
				s.""DueDate"" as DueDate,s.""CreatedDate"" as CreatedOn,
s.""LastUpdatedDate"" as UpdatedOn,u.""Name"" as CreatedBy,
lov.""Name"" as ProjectStatus,lovp.""Name"" as Priority
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
				left join public.""User"" as u on u.""Id""=s.""CreatedBy"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId"" and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_uc.CompanyId}'
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid""

                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId"" = '{_repo.UserContext.UserId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'


                        union all
                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid"" 

                     FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

                          left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'     
                    -- join public.""User"" as u on s.""OwnerUserId""=u.""Id""
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code""--, t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					-- left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""
                        where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )

	           		
                    SELECT count(""Id"") as ""Count"",
					  sum(case when ""Code""='TASK_STATUS_INPROGRESS' and ""ServiceId"" is not null then 1 else 0 end) as ""InProgreessCount"",
					  sum(case when ""Code"" = 'TASK_STATUS_DRAFT' and ""ServiceId"" is not null then 1 else 0 end) as ""DraftCount"",
					sum(case when ""Code"" = 'TASK_STATUS_COMPLETE' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
					sum(case when ""Code"" = 'TASK_STATUS_OVERDUE' and ""ServiceId"" is not null then 1 else 0 end) as ""OverDueCount"",
                    sum(case when ""Code"" = 'TASK_STATUS_PLANNED' and ""ServiceId"" is not null then 1 else 0 end) as ""PlannedCount"",
                    sum(case when ""Code"" = 'TASK_STATUS_PLANNED_OVERDUE' and ""ServiceId"" is not null then 1 else 0 end) as ""PlannedOverDueCount"",
					 
					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""--,""auid""
                 
                ) t on s.""Id""=t.""ServiceId""
       left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and (s.""OwnerUserId""='{_repo.UserContext.UserId}' or s.""RequestedByUserId"" = '{_repo.UserContext.UserId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'


                        union all
                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                          join NtsService ns on s.""ParentServiceId""=ns.""Id""

                            left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'    
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code"",""auid""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code"", t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )

	           		
                    SELECT count(distinct ""auid"") as ""Count"",
					  
					 
					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""
                 
                ) ucount on s.""Id""=ucount.""ServiceId""
                Where tt.""Code""='PROJECT_SUPER_SERVICE' and (s.""OwnerUserId"" = '{_repo.UserContext.UserId}' or s.""RequestedByUserId"" = '{_repo.UserContext.UserId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  --and t.""Count""!=0
              --  GROUP BY s.""Id"", s.""ServiceSubject"", t.""Count""     ";
            var queryData = await _queryRepo.ExecuteQueryList<ProgramDashboardViewModel>(query, null);


            return queryData;
        }

        public async Task<IList<ProgramDashboardViewModel>> GetProjectSharedData()
        {

            string query = @$"select  s.""Id"" as Id,ucount.""Count"" as UserCount,
                s.""ServiceSubject"" as Name,t.""Count"" as AllTaskCount,(t.""CompletedCount""::float / t.""Count""::float)*100 as Percentage,
                false as hasChildren,t.""InProgreessCount"" as InProgreessCount,t.""DraftCount"" as DraftCount,t.""CompletedCount"" as CompletedCount,t.""OverDueCount"" as OverDueCount,
t.""PlannedCount"" as PlannedCount,t.""PlannedOverDueCount"" as PlannedOverDueCount,                
--'PROJECT_SUPER_SERVICE' as ParentId,'STAGE' as Type

                s.""ServiceNo"" as ServiceNo, s.""ServiceSubject"" as ProjectName,s.""StartDate"" as StartDate,
				s.""DueDate"" as DueDate,s.""CreatedDate"" as CreatedOn,
s.""LastUpdatedDate"" as UpdatedOn,u.""Name"" as CreatedBy,
lov.""Name"" as ProjectStatus,lovp.""Name"" as Priority
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
				left join public.""User"" as u on u.""Id""=s.""CreatedBy"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId"" and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_uc.CompanyId}'
            left join public.""NtsServiceShared"" as nss on nss.""NtsServiceId""=s.""Id"" and nss.""IsDeleted""=false and nss.""CompanyId""='{_uc.CompanyId}'
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid""

                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' --and s.""OwnerUserId""='{_repo.UserContext.UserId}'


                        union all
                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid"" 

                     FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                          left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'    
                    -- join public.""User"" as u on s.""OwnerUserId""=u.""Id""
                        where  s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code""--, t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					-- left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" 
                        where  t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )

	           		
                    SELECT count(""Id"") as ""Count"",
					  sum(case when ""Code""='TASK_STATUS_INPROGRESS' and ""ServiceId"" is not null then 1 else 0 end) as ""InProgreessCount"",
					  sum(case when ""Code"" = 'TASK_STATUS_DRAFT' and ""ServiceId"" is not null then 1 else 0 end) as ""DraftCount"",
					sum(case when ""Code"" = 'TASK_STATUS_COMPLETE' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
					sum(case when ""Code"" = 'TASK_STATUS_OVERDUE' and ""ServiceId"" is not null then 1 else 0 end) as ""OverDueCount"",
					 sum(case when ""Code"" = 'TASK_STATUS_PLANNED' and ""ServiceId"" is not null then 1 else 0 end) as ""PlannedCount"",
                    sum(case when ""Code"" = 'TASK_STATUS_PLANNED_OVERDUE' and ""ServiceId"" is not null then 1 else 0 end) as ""PlannedOverDueCount"",
					
					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""--,""auid""
                 
                ) t on s.""Id""=t.""ServiceId""
       left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", s.""Id"" as ""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' --and s.""OwnerUserId""='{_repo.UserContext.UserId}'


                        union all
                        SELECT s.""Id"", ns.""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                          join NtsService ns on s.""ParentServiceId""=ns.""Id""

                            left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'    
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId"",""Code"",""auid""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code"", t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )

	           		
                    SELECT count(distinct ""auid"") as ""Count"",
					  
					 
					 ""ServiceId"" from Nts where Level = 'Child' group by ""ServiceId""
                 
                ) ucount on s.""Id""=ucount.""ServiceId""
                Where tt.""Code""='PROJECT_SUPER_SERVICE' and nss.""SharedWithUserId"" = '{_repo.UserContext.UserId}'
                and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'           
--and t.""Count""!=0
              --  GROUP BY s.""Id"", s.""ServiceSubject"", t.""Count""  ";
            var queryData = await _queryRepo.ExecuteQueryList<ProgramDashboardViewModel>(query, null);

            return queryData;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetSectionListOrderBySequenceNo(string workBoardId)
        {
            var query = $@"select * from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
            var list = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return list;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetSectionsIdOrderBySequenceNo(string workBoardId)
        {
            
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
            var list = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return list;
        }

        public async Task<List<WorkBoardItemViewModel>> GetItemsInSection(string sectionId)
        {
            var query1 = $@"select * from cms.""N_WORKBOARD_WorkBoardItem"" where ""WorkBoardSectionId""='{sectionId}' and ""IsDeleted""=false order by ""SequenceOrder""";
            var itemList = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query1, null);
            return itemList;
        }
        public async Task<WorkBoardTemplateViewModel> GetWorkBoardTemplateById(string templateTypeId)
        {
            var query = $@"Select * from cms.""N_WORKBOARD_TemplateType"" where ""Id""='{templateTypeId}' and ""IsDeleted""=false";
            return await _queryRepo.ExecuteQuerySingle<WorkBoardTemplateViewModel>(query, null);
        }

        public async Task<List<WorkBoardSectionViewModel>> GetParticularNumberOfSections(int number)
        {
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST {number} ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return sections;
        }

        public async Task UpdateSectionsforBasicTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId)
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
        }
        
        public async Task UpdateSectionsforMonthlyTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId, DateTime date)
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
        }
        
        public async Task UpdateSectionsforWeeklyTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId)
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
        }

        public async Task UpdateSectionsforYearlyTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId)
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
        }


        // ------------------ ProjectManagementBusiness Methods ---------------------------------------------
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTask(string userId, string projectId, bool isProjectManager = false, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
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
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         where  tt.""Code""='PROJECT_SUPER_SERVICE' and  s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
			
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
               where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level
                ,""ActualStartDate"",""ActualEndDate"",""StatusGroupCode""
                from NtsService

                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                        ,t.""ActualStartDate"" as ""ActualStartDate"",coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere#    
	                             
                    )
                 SELECT * from Nts where Level='Child'";
            }
            else
            {
                query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                            FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                          where  tt.""Code""='PROJECT_SUPER_SERVICE' and  s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
					
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerUserId"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level
                ,""ActualStartDate"",""ActualEndDate"",""StatusGroupCode""
                from NtsService

                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"",nt.""OwnerUserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                    ,t.""ActualStartDate"" as ""ActualStartDate"",coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                     where t.""AssignedToUserId""='{userId}' or nt.""OwnerUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
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
            if (ownerIds.IsNotNullAndNotEmpty())
            {
                string oids = ownerIds.Replace(",", "','");
                //foreach (var i in ownerIds)
                //{
                //    oids += $"'{i}',";
                //}
                //oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""OwnerUserId"" in ('" + oids + "') ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var assigneeSerach = "";
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
                    if (ownerIds.IsNotNullAndNotEmpty())
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                    }
                    else
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                    }
                }
            }
            query = query.Replace("#AssigneeWhere#", assigneeSerach);
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
                    if ((ownerIds.IsNotNullAndNotEmpty()) || (assigneeIds.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {
                if ((ownerIds.IsNotNullAndNotEmpty()) || (assigneeIds.IsNotNullAndNotEmpty()) || (tasksStatus.IsNotNullAndNotEmpty()))
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
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetProjectUserIdNameList(string projectId)
        {
            var search = @$" and s.""Id""='{projectId}' ";

            if (projectId.IsNotNull())
            {
                search = @$" and s.""Id""='{projectId}' ";
            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					 where  s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level
                    ,""ActualStartDate"",""ActualEndDate"",""StatusGroupCode""
                    from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level
                    ,t.""ActualStartDate"" as ""ActualStartDate"",coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                             where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )
                 SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<MailViewModel>> ReadEmailTaskData(string userId)
        {
            string query = "";

            query = $@"	SELECT distinct t.""Id"" as Id,t.""TaskSubject"" as Subject,s.""ServiceSubject"" as Project,tu.""Email"" as From ,t.""StartDate"" as StartDate FROM public.""Template""  as tt
                    join public.""NtsTask"" as t on tt.""Id""=t.""TemplateId""
                    join public.""NtsService"" as s on t.""ParentServiceId""=s.""Id"" and s.""IsDeleted"" = false and  s.""CompanyId""='{_uc.CompanyId}'
                    join public.""ProjectEmailSetup"" as es on es.""ServiceId""=s.""Id"" and es.""IsDeleted""=false and es.""CompanyId""='{_uc.CompanyId}'
                    join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    left join public.""User"" as tu on t.""OwnerUserId""=tu.""Id"" and tu.""IsDeleted""=false and tu.""CompanyId""='{_uc.CompanyId}'
                    where ""Code""='EMAIL_TASK' and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<MailViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<WBSViewModel>> ReadProjectTaskForEmailList(string projectId)
        {
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""ServiceNo"" as ServiceNo,'Project' as ""Type"", s.""LockStatusId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 	join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					    where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",s.""ServiceNo"" as ServiceNo,'Stage' as ""Type"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceNo"" as TaskNo,""ServiceSubject"" as Title,""Type"" ,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",true as Summary,""Priority""--,""NtsStatus"" 
                from NtsService


                 union all

                 select t.""Id"",t.""TaskNo"" as TaskNo, t.""TaskSubject"" as Title,'Task' as ""Type"", t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",true as Summary, sp.""Name"" as ""Priority""--, ss.""Name"" as ""NtsStatus"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        --join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                          
                    )
                 SELECT ""Id"" as key,true as lazy,""taskno"" as title,""Id"" as Id,""taskno"" as ItemNo,""ParentId"" as ParentId,""title"" as Subject,""start"" as StartDate,""end"" as DueDate,""Type"" as Type from Nts";

            var queryData = await _queryRepo.ExecuteQueryList<WBSViewModel>(query, null);

            return queryData;


        }
        public async Task<IList<IdNameViewModel>> ReadProjectTaskUserData(string projectId)
        {
            var cypher = $@" WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""ServiceNo"" as ServiceNo,'Project' as ""Type"", s.""LockStatusId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as NtsStatus
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 	join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					    where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ServiceNo"" as ServiceNo,'Stage' as ""Type"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", u.""Id"" as ""UserId"", sp.""Name"" as Priority, ss.""Name"" as NtsStatus
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceNo"" as TaskNo,""ServiceSubject"" as Title,""Type"" ,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"", ""UserId"",true as Summary,""Priority""--,""NtsStatus"" 
                from NtsService


                 union all

                 select t.""Id"", t.""TaskNo"" as TaskNo, t.""TaskSubject"" as Title,'Task' as ""Type"", t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", u.""Id"" as ""UserId"",true as Summary, sp.""Name"" as ""Priority""--, ss.""Name"" as ""NtsStatus"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        --join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                      where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'    
                    )
                 --SELECT ""Id"" as Id,""taskno"" as ItemNo,""ParentId"" as ParentId,""title"" as Subject,""start"" as StartDate,""end"" as DueDate,""Type"" as Type,""UserName"",""UserId""  from Nts

                 SELECT Distinct ""UserName"" as Name,""UserId"" as Id from Nts ";


            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(cypher, null);
            return list;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatusByTemplate(string templateId, string userId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetTaskByUsers(string templateId, string userId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetSLADetails(string templateId, string userId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var query = @$"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(t.""TaskSLA"") as Days, Avg(t.""ActualSLA"") as ActualSLA
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""   and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        and t.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and t.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and ts.""Code"" = 'TASK_STATUS_COMPLETE' group by t.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewData(string templateId, string userId, string tasksStatus = null, string ownerIds = null)
        {

            var query = @$"select t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""OwnerName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        #OwnerWhere# #StatusWhere# ";

            var ownerSerach = "";
            if (ownerIds.IsNotNullAndNotEmpty())
            {
                string oids = ownerIds.Replace(",", "','");
                //foreach (var i in ownerIds)
                //{
                //    oids += $"'{i}',";
                //}
                //oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""RequestedByUserId"" in (" + oids + ") ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);

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
                    if ((ownerIds.IsNotNullAndNotEmpty()) || (ownerIds.IsNotNullAndNotEmpty() && ownerSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersList(string templateId, string userId)
        {
            var query = $@"select distinct t.""RequestedByUserId"" as Id, u.""Name""
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            where t.""TemplateId""='{templateId}' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<IdNameViewModel>> GetTaskUsersList(string templateId)
        {
            var query = $@"select distinct t.""AssignedToUserId"" as Id, u.""Name""
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        //For PM
        public async Task<List<ProjectGanttTaskViewModel>> GetPMTaskStatusByTemplate(string templateId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetPMTaskByUsers(string templateId)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetPMSLADetails(string templateId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var query = @$"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type, Avg(t.""TaskSLA"") as Days, Avg(t.""ActualSLA"") as ActualSLA
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        and t.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and t.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and ts.""Code"" = 'TASK_STATUS_COMPLETE' group by t.""DueDate""::TIMESTAMP::DATE ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadPMTaskGridViewData(string templateId, string tasksStatus = null, string userIds = null)
        {

            var query = @$"select t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
						where t.""TemplateId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        #UserWhere# #StatusWhere# ";

            var userSerach = "";
            if (userIds.IsNotNullAndNotEmpty())
            {
                string uids = userIds.Replace(",", "','");
                //foreach (var i in userIds)
                //{
                //    uids += $"'{i}',";
                //}
                //uids = uids.Trim(',');
                if (uids.IsNotNull())
                {
                    userSerach = @" and t.""AssignedToUserId"" in ('" + uids + "') ";
                }
            }
            query = query.Replace("#UserWhere#", userSerach);

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
                    if ((userIds.IsNotNullAndNotEmpty()) || (userIds.IsNotNullAndNotEmpty() && userSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }


        // For Group Template


        public async Task<List<ProjectGanttTaskViewModel>> GetGroupTemplate()
        {


            var query = @$"select GRT.""Id"",GR.""Name"" as GroupName 
from  public.""NtsGroup"" GR 
left join  public.""NtsGroupTemplate"" GRT on GR.""Id""=GRT.""NtsGroupId"" and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_uc.CompanyId}'
where GR.""IsDeleted""='false' and GR.""CompanyId""='{_uc.CompanyId}' ";
            ///  query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }






        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatusRequestedByMeGroup(string TemplateID, string UserID, string StatusLOV)
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
on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false' and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false'       and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false'         and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false'   and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false'  and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_uc.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false'           and GRT.""CompanyId""='{_uc.CompanyId}'
where person2.""UserId""='{UserID}' and GRT.""NtsGroupId"" = '{TemplateID}' and  NTS.""IsDeleted""='false'  and NTS.""CompanyId""='{_uc.CompanyId}' #StatusLOV#
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

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatusAssigneduseridGroup(string TemplateID, string UserID, string StatusTemplateId, string StatusLOV)
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
from public.""NtsTask"" as NTS left join public.""LOV"" as LOV on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false'  and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=Ur.""Id"" and Ur.""IsDeleted""='false'   and Ur.""CompanyId""='{_uc.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false'  and Tm.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false'       and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false'         and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false'                  and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false'  and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_uc.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false'           and GRT.""CompanyId""='{_uc.CompanyId}'
where person2.""UserId""='{UserID}' and GRT.""NtsGroupId"" = '{TemplateID}'and  NTS.""IsDeleted""='false'  and NTS.""CompanyId""='{_uc.CompanyId}' #StatusLOV#  #StatusTemplateid#
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

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> MdlassignUserGroup(string TemplateID, string UserID)
        {
            var query = @$"select Distinct(Ur.""Name"") as Type,Ur.""Id"" as RefId
from public.""NtsTask"" as NTS left join public.""LOV"" as LOV 
on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false' and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id""  and Ur.""IsDeleted"" = 'false' and Ur.""CompanyId""='{_uc.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false'      and Tm.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_uc.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_uc.CompanyId}'
where person2.""UserId""='{UserID}' and NTS.""CompanyId""='{_uc.CompanyId}'
 and GRT.""NtsGroupId"" = '{TemplateID}' and NTS.""IsDeleted""='false'";
            ///  query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<List<NtsTaskChartList>> GetGridListGroup(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs = null)
        {
            var query = @$"select Tm.""Name"" as TemplateName, NTS.""Id"", ""TaskSubject"",""TaskNo"",Ur.""Name"" as AssignName,LOV.""Name"" as Priority,LOVS.""Name"" as Status,NTS.""StartDate"",NTS.""DueDate""
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskPriorityId"" and LOV.""IsDeleted""='false' and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""LOV"" as LOVS on LOVS.""Id""=NTS.""TaskStatusId""  and LOVS.""IsDeleted""='false' and LOVS.""CompanyId""='{_uc.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id""  and Ur.""IsDeleted"" = 'false' and Ur.""CompanyId""='{_uc.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_uc.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_uc.CompanyId}'
where person2.""UserId""='{UserID}'
 and GRT.""NtsGroupId"" = '{TemplateID}' and NTS.""IsDeleted""='false' and NTS.""CompanyId""='{_uc.CompanyId}'  #AssigneeUser# #TaskStatusId# ";

            var assigneeSerach = "";
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
                    assigneeSerach = @"and NTS.""AssignedToUserId"" in ('" + aids + "') ";
                }
            }
            query = query.Replace("#AssigneeUser#", assigneeSerach);

            var StatusSerach = "";
            if (StatusIDs.IsNotNullAndNotEmpty())
            {
                string aids = StatusIDs.Replace(",", "','");
                //foreach (var i in StatusIDs)
                //{
                //    aids += $"'{i}',";
                //}
                //aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    StatusSerach = @"and NTS.""TaskStatusId"" in ('" + aids + "') ";
                }
            }
            query = query.Replace("#TaskStatusId#", StatusSerach);

            //query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<NtsTaskChartList>(query, null);

            //var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetDatewiseTaskGroup(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null)
        {


            var query = @$"select CAST(NTS.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(NTS.""TaskSLA"") as Days,Avg(NTS.""ActualSLA"") as ActualSLA 
from public.""NtsTask"" as NTS left join public.""LOV"" as LOV 
on LOV.""Id"" = NTS.""TaskStatusId""  and LOV.""IsDeleted"" = 'false' and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""Template"" Tm on Tm.""Id""=NTS.""TemplateId"" and Tm.""IsDeleted""='false' and Tm.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on NTS.""AssignedToUserId""=person.""UserId"" and person.""IsDeleted""='false' and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""='false' and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""='false' and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId""   and Assign2.""IsDeleted""='false' and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id""   and person2.""IsDeleted""='false' and person2.""CompanyId""='{_uc.CompanyId}'
left join Public.""NtsGroupTemplate"" GRT on NTS.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""='false' and GRT.""CompanyId""='{_uc.CompanyId}'
where NTS.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and NTS.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE 
and LOV.""Code""='TASK_STATUS_COMPLETE'
and person2.""UserId""='{UserID}' and GRT.""NtsGroupId"" = '{TemplateID}' and   NTS.""IsDeleted""='false' and NTS.""CompanyId""='{_uc.CompanyId}'
 Group by NTS.""DueDate""::TIMESTAMP::DATE ";
            //query = query.Replace("#RequesUserID#",UserID).Replace("#TemplateID#",TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        //for project manager
        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatusRequestedByMeProjectGroup(string TemplateID, string StatusLOV = null)
        {

            var query = @$"select Tm.""Id"",Tm.""Name"" as NtsStatus, t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""
                        join public.""Template"" Tm on Tm.""Id""=t.""TemplateId""    and Tm.""IsDeleted""=false and Tm.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""    and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
                                                                                                    
						where GRT.""NtsGroupId""='{TemplateID}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'  #StatusLOV#";

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

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetChartByAssigneduserProjectGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""    and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id""             and ou.""IsDeleted""=false and ou.""CompanyId""='{_uc.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' #StatusTemplateid#  #StatusLOV# ";


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

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> GetGridListProjectGroup(string templateId, string tasksStatus = null, string ownerIds = null)
        {

            var query = @$"select Tm.""Name"" as GroupName, t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""OwnerName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                          join public.""Template"" Tm on Tm.""Id""=t.""TemplateId""
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                          join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' t GRT.""IsDeleted""=false t GRT.""CompanyId""='{_uc.CompanyId}'
                         #OwnerWhere# #StatusWhere# ";

            var ownerSerach = "";
            if (ownerIds.IsNotNullAndNotEmpty())
            {
                string oids = ownerIds.Replace(",", "','");
                //foreach (var i in ownerIds)
                //{
                //    oids += $"'{i}',";
                //}
                //oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""AssignedToUserId"" in ('" + oids + "') ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);

            var statusSerach = "";
            if (tasksStatus.IsNotNullAndNotEmpty())
            {
                string sids = tasksStatus.Replace(",", "','"); ;
                //foreach (var i in tasksStatus)
                //{
                //    sids += $"'{i}',";
                //}
                //sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNullAndNotEmpty()) || (ownerIds.IsNotNullAndNotEmpty() && ownerSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> GetTaskStatusAssigneduseridProjectGroup(string templateId)
        {
            var query = $@"select distinct t.""AssignedToUserId"" as Id, u.""Name"" as Value
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
            join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
            where  GRT.""NtsGroupId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> GetDatewiseTaskProjectGroup(string templateId, DateTime? FromDate, DateTime? ToDate)
        {
            var query = $@"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(t.""TaskSLA"") as Days,Avg(t.""ActualSLA"") as ActualSLA
                                FROM public.""NtsTask"" as t
                                join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""            and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""                and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'

                        where t.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and t.""DueDate""::TIMESTAMP::DATE <='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE
                        and ts.""Code""='TASK_STATUS_COMPLETE'  and  GRT.""NtsGroupId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        Group by t.""DueDate""::TIMESTAMP::DATE";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }







        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatusByTemplateGroup(string templateId, string userId, string StatusLOV = null)
        {

            var query = @$"select  Tm.""Id"",Tm.""Name"" as NtsStatus, t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"",
                            tp.""Name"" as ""Priority""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""    and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                       join public.""Template"" Tm on Tm.""Id""=t.""TemplateId""     and Tm.""IsDeleted""=false and Tm.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' and t.""AssignedToUserId""='{userId}' #StatusLOV# ";


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

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetTaskByUsersGroup(string templateId, string userId, string StatusTemplateID = null, string StatusLOV = null)
        {

            var query = @$"select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",
                            tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                            FROM public.""NtsTask"" as t
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""    and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as ou on t.""RequestedByUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_uc.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId"" and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' and t.""AssignedToUserId""='{userId}' #StatusTemplateid#  #StatusLOV# ";


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


            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewDataGroup(string templateId, string userId, string tasksStatus = null, string ownerIds = null)
        {

            var query = @$"select Tm.""Name"" as GroupName, t.""Id"",t.""TaskNo"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, u.""Name"" as ""OwnerName"",
                        tp.""Name"" as ""Priority"", ts.""Name"" as ""NtsStatus"",ts.""Id"" as ""NtsStatusId""
                        FROM public.""NtsTask"" as t
                         join public.""Template"" Tm on Tm.""Id""=t.""TemplateId""    and tm.""IsDeleted""=false and tm.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""   and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""     and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""RequestedByUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                          join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
						where GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        #OwnerWhere# #StatusWhere# ";

            var ownerSerach = "";
            if (ownerIds.IsNotNullAndNotEmpty())
            {
                string oids = ownerIds.Replace(",", "','");
                //foreach (var i in ownerIds)
                //{
                //    oids += $"'{i}',";
                //}
                //oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""RequestedByUserId"" in ('" + oids + "') ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);

            var statusSerach = "";
            if (tasksStatus.IsNotNullAndNotEmpty())
            {
                string sids = tasksStatus.Replace(",", "','"); ;
                //foreach (var i in tasksStatus)
                //{
                //    sids += $"'{i}',";
                //}
                //sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNullAndNotEmpty()) || (ownerIds.IsNotNullAndNotEmpty() && ownerSerach.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersListGroup(string templateId, string userId)
        {
            var query = $@"select distinct t.""RequestedByUserId"" as Id, u.""Name""
                            FROM public.""NtsTask"" as t
                            join public.""User"" as u on t.""RequestedByUserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
            join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId""   and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'
            where  GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> GetDatewiseSingleGroup(string templateId, string userId, DateTime? FromDate, DateTime? ToDate)
        {
            var query = $@"select CAST(t.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(t.""TaskSLA"") as Days,Avg(t.""ActualSLA"") as ActualSLA
                                FROM public.""NtsTask"" as t
                                join public.""LOV"" as tp on t.""TaskPriorityId""=tp.""Id""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""            and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id""         and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        join Public.""NtsGroupTemplate"" GRT on t.""TemplateId""=GRT.""TemplateId""  and GRT.""IsDeleted""=false and GRT.""CompanyId""='{_uc.CompanyId}'

                        where t.""DueDate""::TIMESTAMP::DATE >='2021-05-01'::TIMESTAMP::DATE and t.""DueDate""::TIMESTAMP::DATE <='2021-05-21'::TIMESTAMP::DATE
and ts.""Code""='TASK_STATUS_COMPLETE'  and  GRT.""NtsGroupId""='{templateId}' and t.""AssignedToUserId""='{userId}'
       and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'  Group by t.""DueDate""::TIMESTAMP::DATE";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }


        public async Task<ProgramDashboardViewModel> GetPerformanceDashboard()
        {

            string query = @$"select  Count(s.""Id"") as ProjectCount ,
                 cast( sum( t.""Count"") as bigint) as AllTaskCount,

                cast( sum( t.""Count"") as bigint) as AllTaskCount,

               cast(sum(t.""CompletedCount"") as bigint) as ClosedTaskCount,
			   cast(23 as bigint) as AllTime,cast(0 as bigint) as FileCount,
			   cast(sum(ucount.""Count"") as bigint) UserCount


                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""  and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
				left join public.""User"" as u on u.""Id""=s.""CreatedBy""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId""  and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_uc.CompanyId}'
                 left join(


                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",'Parent' as Level, s.""Id"" as ""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid""

                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""            and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""   and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_repo.UserContext.UserId}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'





                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code""--, t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join NtsService as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					-- left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" 
                    where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' )

	           		
                    SELECT count(""Id"") as ""Count"",
										sum(case when ""Code"" = 'TASK_STATUS_COMPLETE' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
					 
					 ""ServiceId"" from NtsService where Level = 'Child' group by ""ServiceId""--,""auid""

                 
                ) t on s.""Id""=t.""ServiceId"" 
       left join(


                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", 'Parent' as Level, s.""Id"" as ""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""  and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_repo.UserContext.UserId}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                           union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code"", t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join NtsService as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    where  t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}')

	           		
                    SELECT count(distinct ""auid"") as ""Count"",
					  
					 
					 ""ServiceId"" from NtsService where Level = 'Child' group by ""ServiceId""


                 
                 
                ) ucount on s.""Id""=ucount.""ServiceId""
                Where tt.""Code""='PROJECT_SUPER_SERVICE'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' and s.""OwnerUserId"" = '{_repo.UserContext.UserId}' --and t.""Count""!=0
               --GROUP BY s.""Id"", t.""Count""     ";
            var queryData = await _queryRepo.ExecuteQuerySingle<ProgramDashboardViewModel>(query, null);


            return queryData;
        }


        public async Task<List<ProjectDashboardChartViewModel>> GetProjectStatus()
        {

            var query = @$"Select lov.""Name"" as Type ,Count(*) as Value  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""  and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId"" = '{_repo.UserContext.UserId}'
  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by lov.""Name"" ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);

            //var list = new List<ProjectDashboardChartViewModel>();
            ///var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = list.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }


        public async Task<IList<ProjectDashboardChartViewModel>> GetTopfiveProject()
        {
            var query = @$"select s.""ServiceSubject"" as ProjectName, s.""Id"" as Id, s.""StartDate""::Date as ProjectStartDate
                            ,u.""Name"" as OwnerUserName, lov.""Name"" as StatusName
                            from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE'  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
                            left join public.""User"" as u on u.""Id""=s.""OwnerUserId""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            where s.""IsDeleted""=false #OWNERUSER# and s.""CompanyId""='{_uc.CompanyId}'
                            and s.""ServiceSubject""!='' and s.""IsDeleted""=false order by s.""StartDate"" desc";
            
            var owneruser = "";
            if (!_repo.UserContext.IsSystemAdmin)
            {
                owneruser = $@" and s.""OwnerUserId""='{_repo.UserContext.UserId}' ";
            }
            query = query.Replace("#OWNERUSER#", owneruser);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);
            return queryData;
        }


        public async Task<IList<ProjectGanttTaskViewModel>> GetTimeLog()
        {
            var query = $@"
select count(*) as Value, case when ""IsBillable""='false' then 'Non Billable' else 'Billable' end as Type,sum(""Duration"") as Days 
from public.""NtsTaskTimeEntry""
where ""IsDeleted""=false and ""CompanyId""='{_uc.CompanyId}' and ""NtsTaskId"" in (
WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", 'Parent' as Level, s.""Id"" as ""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid""
                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""          and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_repo.UserContext.UserId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'



                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code""--, t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join NtsService as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" 	and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
                     -- left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""   
              where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )	           		
                    SELECT ""Id""					 
				  from NtsService where Level = 'Child' group by ""Id"",""ServiceId""--,""auid""
                  )group by ""IsBillable""";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;
        }


        public async Task<IList<ProgramDashboardViewModel>> GetProjectwiseUsers()
        {

            string query = @$"select s.""ServiceSubject"" as ProjectName,coalesce(ucount.""Count"", 0) as UserCount

                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""           and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
				left join public.""User"" as u on u.""Id""=s.""CreatedBy""              and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""     and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId"" and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_uc.CompanyId}'
                 left join(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",'Parent' as Level, s.""Id"" as ""ServiceId"", lov.""Code""--, s.""OwnerUserId"" as ""auid""

                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""            and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""   and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_repo.UserContext.UserId}'

                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code""--, t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join NtsService as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId""  and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					-- left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""  
                          where    t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )
	           		
                    SELECT count(""Id"") as ""Count"",
										sum(case when ""Code"" = 'TASK_STATUS_COMPLETED' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
					 
					 ""ServiceId"" from NtsService where Level = 'Child' group by ""ServiceId""--,""auid""

                 
                ) t on s.""Id""=t.""ServiceId"" 
       left join(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"", 'Parent' as Level, s.""Id"" as ""ServiceId"", lov.""Code"", s.""OwnerUserId"" as ""auid""  FROM public.""NtsService"" as s
                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""        and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""   and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_repo.UserContext.UserId}'

                           union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code"", t.""AssignedToUserId"" as ""auid""
                     FROM  public.""NtsTask"" as t
                        join NtsService as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""   and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' )
	           		
                    SELECT count(distinct ""auid"") as ""Count"",
					 
					 ""ServiceId"" from NtsService where Level = 'Child' group by ""ServiceId""
                 
                ) ucount on s.""Id""=ucount.""ServiceId""
                Where tt.""Code""='PROJECT_SUPER_SERVICE' 
				and s.""OwnerUserId"" = '{_repo.UserContext.UserId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                and s.""ServiceSubject""!='' and lov.""Code"" IN ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE')
                and ucount.""Count"">0
               --GROUP BY s.""Id"", t.""Count""       ";
            var queryData = await _queryRepo.ExecuteQueryList<ProgramDashboardViewModel>(query, null);


            return queryData;
        }



        public async Task<IList<ProgramDashboardViewModel>> GetTaskDetails()
        {

            string query = @$"
          select    T.*


                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""             and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
				left join public.""User"" as u on u.""Id""=s.""CreatedBy""                and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""       and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId""   and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_uc.CompanyId}'
                 left join(


                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",'Parent' as Level, s.""Id"" as ""ServiceId"", lov.""Code"", s.""ServiceSubject"",
                     s.""ServiceSubject"" as ProjectName,
                     s.""ServiceNo"" as TaskNo, s.""DueDate"", s.""ServiceSubject"" as ""OwnerUserId""

                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""          and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_uc.UserId}'
                                and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'




                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Code"",nt.""ServiceSubject""
					 , t.""TaskSubject"" as ProjectName, t.""TaskNo"", t.""DueDate"", u.""Name"" as OwnerUserId
                 FROM  public.""NtsTask"" as t
                        join NtsService as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
			          left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                   where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' )

	           		
                    SELECT*

                      from NtsService where Level = 'Child'

                 
                ) t on s.""Id""=t.""ServiceId"" 
      
                Where tt.""Code""='PROJECT_SUPER_SERVICE'  and s.""OwnerUserId"" = '{_uc.UserId}' 

                and Level = 'Child' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                --and t.""Count""!=0
               --GROUP BY s.""Id"", t.""Count""       ";
            var queryData = await _queryRepo.ExecuteQueryList<ProgramDashboardViewModel>(query, null);


            return queryData;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetProjecTaskStatus()
        {

            var query = @$"
          select Count(*) as Value,t.""Name"" as Type 
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""           and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
				left join public.""User"" as u on u.""Id""=s.""CreatedBy""              and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""     and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
				left join public.""LOV"" as lovp on lovp.""Id""=s.""ServicePriorityId"" and lovp.""IsDeleted""=false and lovp.""CompanyId""='{_uc.CompanyId}'
                 left join(


                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",'Parent' as Level, s.""Id"" as ""ServiceId"", lov.""Name""

                     FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						     where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_uc.UserId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                 union all

                 select t.""Id"",'Child' as Level, nt.""ServiceId"", lov.""Name""
                 FROM  public.""NtsTask"" as t
                        join NtsService as nt on t.""ParentServiceId"" =nt.""Id""  
	                    left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
			          left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                   where  t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' )
	           		
                    SELECT*

                      from NtsService where Level = 'Child' 
                 
                ) t on s.""Id""=t.""ServiceId"" 
      
                Where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId"" = '{_uc.UserId}' 

                and Level = 'Child' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  group by t.""Name""
				
				--and t.""Count""!=0
               --GROUP BY s.""Id"", t.""Count""      ";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectDashboardChartViewModel>(query, null);

            //var list = new List<ProjectDashboardChartViewModel>();
            ///var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = list.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }
        public async Task<List<TaskWorkTimeViewModel>> GetHourReportTaskData(string serviceId)
        {

            string query = @$"Select n.*,s.""ServiceSubject"" as ProjectName,t.""TaskSubject"" as TaskName,t.""TaskNo"" as TaskNo,s.""Id"" as ProjectId,t.""Id"" as TaskId,
tu.""Id"" as AssigneeId,s.""StartDate"" as TaskStartDate,n.""Comment"" as WorkComment,t.""TaskSLA"" as SLA,
s.""DueDate"" as TaskDueDate,n.""StartDate"" as WSDate,n.""EndDate"" as WEDate,tu.""Name"" as AssigneeName,lov.""Name"" as TaskStatusName
                            from public.""NtsTaskTimeEntry"" as n 
							join public.""NtsTask"" as t on t.""Id""=n.""NtsTaskId"" and t.""IsDeleted""=false  
join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false  
							join public.""User"" as tu ON tu.""Id"" = t.""AssignedToUserId"" and tu.""IsDeleted""=false     
							join public.""NtsService"" as s on  s.""Id""=t.""ServicePlusId"" or s.""Id""=t.""ParentServiceId"" 
							join public.""User"" as u on  u.""Id""=s.""OwnerUserId"" 
							join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""Code""='PROJECT_SUPER_SERVICE'
							and s.""IsDeleted""=false                              
                             where
							 --s.""Id""='{serviceId}'  and
							 n.""IsDeleted""= false";
            ;
            var result = await _queryRepo.ExecuteQueryList<TaskWorkTimeViewModel>(query, null);
            return result;

        }
        public async Task<List<TaskWorkTimeViewModel>> GetHourReportProjectData(string projectId, string assigneeId, string sdate, string edate)
        {

            string query = @$"Select s.""ServiceSubject"" as ProjectName,s.""Id"" as ProjectId,t.""TaskSubject"" as TaskName,t.""TaskNo"" as TaskNo,lov.""Name"" as TaskStatusName,
tu.""Id"" as AssigneeId,s.""StartDate"" as TaskStartDate,t.""TaskSLA"" as SLA,t.""Id""  as TaskId,
s.""DueDate"" as TaskDueDate,tu.""Name"" as AssigneeName
                            from public.""NtsTaskTimeEntry"" as n 
							join public.""NtsTask"" as t on t.""Id""=n.""NtsTaskId"" and t.""IsDeleted""=false  
join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false  
							join public.""User"" as tu ON tu.""Id"" = t.""AssignedToUserId"" and tu.""IsDeleted""=false     
							join public.""NtsService"" as s on  s.""Id""=t.""ServicePlusId"" or s.""Id""=t.""ParentServiceId"" 
							join public.""User"" as u on  u.""Id""=s.""OwnerUserId"" 
							join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""Code""='PROJECT_SUPER_SERVICE'
							and s.""IsDeleted""=false                              
                             where
							
							 n.""IsDeleted""= false #WHERE# #ASSIGNEEWHERE# #DATEWHERE#
group by s.""ServiceSubject"",s.""Id"",tu.""Id"",lov.""Name"",t.""TaskSubject"",t.""TaskNo"",t.""TaskSLA"",
							 s.""StartDate"",s.""DueDate"",tu.""Name"",t.""Id""
							 
							 
							 
							 ";
            var search = "";
            var assigneeSearch = "";
            var dateSearch = "";
            if (assigneeId.IsNotNullAndNotEmpty())
            {
                assigneeSearch = $@" and tu.""Id""='{assigneeId}'";
                //result = result.Where(x => x.AssigneeId == assigneeId).ToList();
            }
            if (projectId.IsNotNullAndNotEmpty())
            {
                search = $@" and s.""Id""='{projectId}'";
                // result = result.Where(x => x.ProjectId == projectId).ToList();
            }
            if (sdate.IsNotNullAndNotEmpty() && sdate != "null")
            {
                var date = Convert.ToDateTime(sdate);
                dateSearch = $@" and n.""StartDate""::DATE >= '{date}'::DATE";
                //result = result.Where(x => x.WSDate.Value.Date >= sd.Date).ToList();

            }
            if (edate.IsNotNullAndNotEmpty() && edate != "null")
            {
                var date = Convert.ToDateTime(edate);
                dateSearch = string.Concat(dateSearch, $@" and n.""EndDate""::DATE <= '{date}'::DATE");
                // result = result.Where(x => x.WEDate.Value.Date < ed.Date).ToList();


            }
            query = query.Replace("#WHERE#", search);
            query = query.Replace("#ASSIGNEEWHERE#", assigneeSearch);
            query = query.Replace("#DATEWHERE#", dateSearch);
            var result = await _queryRepo.ExecuteQueryList<TaskWorkTimeViewModel>(query, null);

            return result;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTimelineData()
        {

            var query = @$" SELECT s.""Id"" as Id,s.""ServiceSubject"" as Title,u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as NtsStatus
                        ,s.""StartDate"" as Start,s.""DueDate"" as End
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""Code""='PROJECT_SUPER_SERVICE' and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					    join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					  where s.""IsDeleted""=false and s.""OwnerUserId""='{_uc.UserId}' and s.""DueDate"" is not null and s.""CompanyId""='{_uc.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForLevelUpto0()
        {
            string query = "";
            if (_uc.IsSystemAdmin)
            {
                query = @$"select '-1' as ""ParentId"",'MonthYear' as ""NodeType"",pc.count as DirectChildCount,to_char(s.""StartDate"",'MONTH-yyyy') as Name,to_char(s.""StartDate"",'MM-yyyy') as Id,'Parent' as Type, 1 as Level from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            left join (select count(*) as count,sub.MonthYear from (select to_char(s.""StartDate"",'MM-yyyy') as MonthYear from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false    
                            where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by sp.""Id"", to_char(s.""StartDate"",'MM-yyyy')) as sub group by sub.MonthYear) as pc on pc.MonthYear=to_char(s.""StartDate"",'MM-yyyy')


                            where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id,pc.count ";

            }
            else
            {
                var users = await _userHierBusiness.GetUserHierarchyByCode("PROJECT_USER_HIERARCHY", _uc.UserId);

                string userIds = null;
                if (users.IsNotNull() && users.Count > 0)
                {
                    var Ids = users.Select(x => x.UserId).ToList();

                    userIds = string.Join("','", Ids);
                }
                query = @$"select '-1' as ""ParentId"",'MonthYear' as ""NodeType"",pc.count as DirectChildCount,to_char(s.""StartDate"",'MONTH-yyyy') as Name,to_char(s.""StartDate"",'MM-yyyy') as Id,'Parent' as Type, 1 as Level  from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                              left join (select count(*) as count,sub.MonthYear from (select to_char(s.""StartDate"",'MM-yyyy') as MonthYear from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false    
                            where (s.""OwnerUserId""='{_uc.UserId}' or s.""RequestedByUserId""='{_uc.UserId}' or s.""OwnerUserId"" in ('{userIds}')) and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by sp.""Id"", to_char(s.""StartDate"",'MM-yyyy')) as sub group by sub.MonthYear) as pc on pc.MonthYear=to_char(s.""StartDate"",'MM-yyyy')

                            where (s.""OwnerUserId""='{_uc.UserId}' or s.""RequestedByUserId""='{_uc.UserId}' or s.""OwnerUserId"" in ('{userIds}')) and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id,pc.count ";
            }


            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForLevel1(string parentId)
        {
            var query = "";
            if (_uc.IsSystemAdmin)
            {
                query = @$"select to_char(s.""StartDate"",'MM-yyyy') as ""ParentId"",'Status' as ""NodeType"",pc.count as DirectChildCount,sp.""Name"" as Name,to_char(s.""StartDate"",'MM-yyyy')||'$'||sp.""Code"" as Id, 2 as Level  from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false  
                        left join (select count(s.*) as count,sp.""Name"",to_char(s.""StartDate"",'MM-yyyy') as MonthYear from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false 
                        join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false    
                        where s.""IsDeleted""=false and to_char(s.""StartDate"",'MM-yyyy')='{parentId}'  group by sp.""Id"", to_char(s.""StartDate"",'MM-yyyy')) as pc on pc.""Name""=sp.""Name"" and pc.MonthYear=to_char(s.""StartDate"",'MM-yyyy') 
                        where  s.""IsDeleted""=false and to_char(s.""StartDate"",'MM-yyyy')='{parentId}' and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id,""ParentId"",pc.count, to_char(s.""StartDate"",'MM-yyyy') ";

            }
            else
            {
                var users = await _userHierBusiness.GetUserHierarchyByCode("PROJECT_USER_HIERARCHY", _uc.UserId);

                string userIds = null;
                if (users.IsNotNull() && users.Count > 0)
                {
                    var Ids = users.Select(x => x.UserId).ToList();

                    userIds = string.Join("','", Ids);
                }
                query = @$"select to_char(s.""StartDate"",'MM-yyyy') as ""ParentId"",'Status' as ""NodeType"",pc.count as DirectChildCount,sp.""Name"" as Name,to_char(s.""StartDate"",'MM-yyyy')||'$'||sp.""Code"" as Id , 2 as Level from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                             join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                            left join (select count(s.*) as count,sp.""Name"",to_char(s.""StartDate"",'MM-yyyy') as MonthYear from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false 
                        join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false    
                        where (s.""OwnerUserId""='{_uc.UserId}' or s.""RequestedByUserId""='{_uc.UserId}' or s.""OwnerUserId"" in ('{userIds}')) and to_char(s.""StartDate"",'MM-yyyy')='{parentId}' and s.""IsDeleted""=false  group by sp.""Id"", to_char(s.""StartDate"",'MM-yyyy')) as pc on pc.""Name""=sp.""Name"" and pc.MonthYear=to_char(s.""StartDate"",'MM-yyyy') 
                        
                            where (s.""OwnerUserId""='{_uc.UserId}' or s.""RequestedByUserId""='{_uc.UserId}' or s.""OwnerUserId"" in ('{userIds}')) and to_char(s.""StartDate"",'MM-yyyy')='{parentId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id,""ParentId"",pc.count, to_char(s.""StartDate"",'MM-yyyy')";
            }

            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForLevel2(string parentId)
        {
            var parent = parentId.Split("$");
            var query = "";
            if (_uc.IsSystemAdmin)
            {
                query = @$"select to_char(s.""StartDate"",'MM-yyyy')||'$'||sp.""Code"" as ""ParentId"",'Project' as ""NodeType"",pc.count+sc.count as DirectChildCount,s.""ServiceSubject""||'-'||ou.""Name""||'-'||s.""StartDate"" as Name, s.""Id"" as Id, 3 as Level  from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                            left join(select count(*) as count,sub.""ParentServiceId"" from (select ou.""Name"",s.""ParentServiceId"" from public.""NtsTask"" as s
                        join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id""
						join public.""NtsService"" as ss on ss.""Id""=s.""ParentServiceId"" and ss.""TemplateCode""='PROJECT_SUPER_SERVICE' and ss.""IsDeleted""=false 													 
                        where s.""IsDeleted""=false  group by ou.""Id"", s.""ParentServiceId"") as sub group by sub.""ParentServiceId"") as pc on pc.""ParentServiceId""=s.""Id""
                            left join(select count(*) as count,sub.""ParentServiceId"" from (select s.""ServiceSubject"",s.""ParentServiceId"" from public.""NtsService"" as s
						join public.""NtsService"" as ss on ss.""Id""=s.""ParentServiceId"" and ss.""TemplateCode""='PROJECT_SUPER_SERVICE' and ss.""IsDeleted""=false 													 
                        where s.""IsDeleted""=false  group by s.""ParentServiceId"", s.""ServiceSubject"") as sub group by sub.""ParentServiceId"") as sc on sc.""ParentServiceId""=s.""Id""
                            where to_char(s.""StartDate"",'MM-yyyy')='{parent[0]}' and sp.""Code""='{parent[1]}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'";

            }
            else
            {
                var users = await _userHierBusiness.GetUserHierarchyByCode("PROJECT_USER_HIERARCHY", _uc.UserId);

                string userIds = null;
                if (users.IsNotNull() && users.Count > 0)
                {
                    var Ids = users.Select(x => x.UserId).ToList();

                    userIds = string.Join("','", Ids);
                }
                query = @$"select to_char(s.""StartDate"",'MM-yyyy')||'$'||sp.""Code"" as ""ParentId"",'Project' as ""NodeType"",pc.count+sc.count as DirectChildCount,s.""ServiceSubject""||'-'||ou.""Name""||'-'||s.""StartDate"" as Name, s.""Id"" as Id, 3 as Level   from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                             left join(select count(*) as count,sub.""ParentServiceId"" from (select ou.""Name"",s.""ParentServiceId"" from public.""NtsTask"" as s
                        join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id""
						join public.""NtsService"" as ss on ss.""Id""=s.""ParentServiceId"" and ss.""TemplateCode""='PROJECT_SUPER_SERVICE' and ss.""IsDeleted""=false 													 
                        where s.""IsDeleted""=false  group by ou.""Id"", s.""ParentServiceId"") as sub group by sub.""ParentServiceId"") as pc on pc.""ParentServiceId""=s.""Id""
                            left join(select count(*) as count,sub.""ParentServiceId"" from (select s.""ServiceSubject"",s.""ParentServiceId"" from public.""NtsService"" as s
						join public.""NtsService"" as ss on ss.""Id""=s.""ParentServiceId"" and ss.""TemplateCode""='PROJECT_SUPER_SERVICE' and ss.""IsDeleted""=false 													 
                        where s.""IsDeleted""=false  group by s.""ParentServiceId"", s.""ServiceSubject"") as sub  group by sub.""ParentServiceId"") as sc on sc.""ParentServiceId""=s.""Id""
                            where (s.""OwnerUserId""='{ _uc.UserId}' or s.""RequestedByUserId""='{ _uc.UserId}' or s.""OwnerUserId"" in ('{userIds}')) and to_char(s.""StartDate"",'MM-yyyy')='{parent[0]}' and sp.""Code""='{parent[1]}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'";
            }



            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForNodeTypeProjectnStage(string parentId)
        {
            var query = @$"select s.""ParentServiceId"" as ""ParentId"",'Stage' as ""NodeType"",pc.count as DirectChildCount,s.""ServiceSubject"" as Name, s.""Id"" as Id from public.""NtsService"" as s
                      left join(select count(*) as count,sub.""ParentServiceId"" from (select ou.""Name"",s.""ParentServiceId"" from public.""NtsTask"" as s
                        join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id""
                        join public.""NtsService"" as ss on ss.""Id""=s.""ParentServiceId""
                        where ss.""ParentServiceId""='{parentId}' and s.""IsDeleted""=false  group by ou.""Id"", s.""ParentServiceId"") as sub group by sub.""ParentServiceId"") as pc on pc.""ParentServiceId""=s.""Id""   
                     where s.""ParentServiceId""='{parentId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                     union
                     select s.""ParentServiceId"" as ""ParentId"",'TaskUser' as ""NodeType"",pc.count as DirectChildCount,ou.""Name"" as Name, ou.""Id""||'$'||s.""ParentServiceId"" as Id from public.""NtsTask"" as s
                     join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id"" 
                      left join(select count(*) as count,sub.""ParentServiceId"", sub.""Id"" from (select sp.""Name"",ou.""Id"",s.""ParentServiceId"" from public.""NtsTask"" as s
                        join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id""
                        join public.""LOV"" as sp on s.""TaskStatusId""=sp.""Id""  and sp.""IsDeleted""=false
                        where s.""ParentServiceId""='{parentId}' and s.""IsDeleted""=false  group by sp.""Name"",ou.""Id"", s.""ParentServiceId"") as sub group by sub.""Id"",sub.""ParentServiceId"") as pc on pc.""ParentServiceId""=s.""ParentServiceId"" and pc.""Id""=ou.""Id"" 
                     where s.""ParentServiceId""='{parentId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by ou.""Name"",ou.""Id"",s.""ParentServiceId"",pc.count
                    ";



            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForNodeTypeTaskUser(string parentId)
        {
            var parent = parentId.Split("$");
            var query = @$"
                     select ou.""Id""||'$'||s.""ParentServiceId"" as ""ParentId"",'TaskStatus' as ""NodeType"",pc.count as DirectChildCount,sp.""Name"" as Name, sp.""Id""||'$'||ou.""Id""||'$'||s.""ParentServiceId"" as ""Id"" from public.""NtsTask"" as s
                     join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id""  
                      join public.""LOV"" as sp on s.""TaskStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                       left join(select count(*) as count,sp.""Id"" from  public.""NtsTask"" as s
                        join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id""
                        join public.""LOV"" as sp on s.""TaskStatusId""=sp.""Id""  and sp.""IsDeleted""=false 																			 
                        where s.""ParentServiceId""='{parent[1]}' and ou.""Id""='{parent[0]}' and s.""IsDeleted""=false  group by sp.""Id"") as pc on pc.""Id""=sp.""Id"" 
                     where s.""ParentServiceId""='{parent[1]}' and ou.""Id""='{parent[0]}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by sp.""Name"",sp.""Id"",ou.""Id"",s.""ParentServiceId"",pc.count
                    ";



            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);
            return queryData;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForNodeTypeTaskStatus(string parentId)
        {
            var parent = parentId.Split("$");
            var query = @$"
                     select sp.""Id""||'$'||ou.""Id""||'$'||s.""ParentServiceId"" as ""ParentId"",'Task' as ""NodeType"",s.""TaskSubject"" as Name, s.""Id"" as ""Id"" from public.""NtsTask"" as s
                     join public.""User"" as ou on s.""AssignedToUserId""=ou.""Id""  
                      join public.""LOV"" as sp on s.""TaskStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                     where s.""ParentServiceId""='{parent[2]}' and sp.""Id""='{parent[0]}' and ou.""Id""='{parent[1]}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                    ";



            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);
            return queryData;
        }

        public async Task<List<TreeViewViewModel>> GetUserRoleData(TreeViewViewModel l, string userId)
        {
            var query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
                , usp.""InboxStageName"" as id, '{l.id}' as ParentId, 'PROJECTSTAGE' as Type,
                true as hasChildren, '{l.UserRoleId}' as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
              
                 left join(
						WITH RECURSIVE NtsService AS ( 
                            SELECT t.""TaskSubject"" as ts,s.""Id"",usp.""InboxStageName"" 
                             FROM public.""NtsService"" as s
                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
						     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						     where usp.""UserRoleId"" = '{l.UserRoleId}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						     union all
                             SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""InboxStageName""
                             FROM public.""NtsService"" as s
                             join NtsService ns on s.""ParentServiceId""=ns.""Id""
                             join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
	                          
	                         where s.""OwnerUserId""='{userId}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'                      
                    )
                    SELECT count(ts) as ""Count"",""InboxStageName"" from NtsService group by ""InboxStageName""
					) nt on usp.""InboxStageName""=nt.""InboxStageName""
                where ur.""Id"" = '{l.UserRoleId}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_uc.CompanyId}'
                Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

            var projectStageList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return projectStageList;
        }

        public async Task<List<TreeViewViewModel>> GetProjectStageData(string userId, TreeViewViewModel p)
        {
            var query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{p.id}' as ParentId,                
                'PROJECT' as Type,'{p.UserRoleId}' as UserRoleId,
                true as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
                left join(
	            WITH RECURSIVE NtsService AS ( 
                    SELECT t.""TaskSubject"" as ts,s.""Id"",usp.""TemplateCode"" 
                        FROM public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT'  and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						where usp.""UserRoleId"" = '{p.UserRoleId}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""TemplateCode""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
	                          
	                    where s.""OwnerUserId""='{p.UserRoleId}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'                      
                    )
                    SELECT count(ts) as ""Count"",""TemplateCode"" from NtsService group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{p.UserRoleId}' and usp.""InboxStageName"" = '{p.id}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_uc.CompanyId}'
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
            var projectList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return projectList;
        }

        public async Task<List<TreeViewViewModel>> GetProjectData(string userId, TreeViewViewModel pr)
        {
            var query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,
                true as hasChildren,s.""Id"" as ProjectId,
                '{pr.id}' as ParentId,'STAGE' as Type
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                 left join(
	            WITH RECURSIVE NtsService AS ( 
                    SELECT t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
                        FROM public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						where tt.""Code""='{pr.id}' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
	                          
	                    where s.""OwnerUserId""='{userId}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'                      
                    )
                    SELECT count(ts) as ""Count"",""ServiceId"" from NtsService group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{pr.id}' and s.""OwnerUserId"" = '{userId}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


            var pStageList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return pStageList;
        }

        public async Task<List<TreeViewViewModel>> GetStageData(string userId, TreeViewViewModel ps)
        {
            var query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                                                                        s.""Id"" as id,
                                                                        true as hasChildren
                                                                        FROM public.""NtsService"" as s
                                                                        left join(
	                                                                    WITH RECURSIVE NtsService AS ( 
                                                                            SELECT t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
                                                                                FROM public.""NtsService"" as s                      
						                                                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						                                                        where s.""ParentServiceId""='{ps.id}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						                                                        union all
                                                                                SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
                                                                                FROM public.""NtsService"" as s
                                                                                join NtsService ns on s.""ParentServiceId""=ns.""Id""
                                                                                join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
	                          
	                                                                            where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'                       
                                                                            )
                                                                            SELECT count(ts) as ""Count"",""ServiceId"" from NtsService group by ""ServiceId""
                 
                                                                        ) t on s.""Id""=t.""ServiceId""
                                                                        where s.""ParentServiceId""='{ps.id}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                                                                        order by s.""SequenceOrder"" asc";

            var stageList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return stageList;
        }

        public async Task<List<TreeViewViewModel>> GetUserRoleList(string userId, string roleText)
        {
            var query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' and usp.""InboxCode""='PMT'
                left join(
						 WITH RECURSIVE NtsService AS ( 
						     SELECT t.""TaskSubject"" as ts,s.""Id"",usp.""UserRoleId"" 
                             FROM public.""NtsService"" as s
                             join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' 
						     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						     where usp.""UserRoleId"" in ({roleText}) and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						     union all
                             SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""UserRoleId""
                             FROM public.""NtsService"" as s
                             join NtsService ns on s.""ParentServiceId""=ns.""Id""
                             join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
	                          
	                         where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        )
                        SELECT count(ts) as ""Count"",""UserRoleId"" from NtsService group by ""UserRoleId""
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_uc.CompanyId}'
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
            var userRoleList = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return userRoleList;
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

                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT t.""AssignedToUserId"", t.""CreatedDate"", t.""TaskSubject"", t.""Id"" as taskid, t.""TaskStatusId"", s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""	 and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                    SELECT distinct p.""AssignedToUserId"",p.""CreatedDate"", p.""taskid"" as TaskId, p.""TaskSubject"" as TaskName, lov.""Name"" as TaskStatus from NtsService as p
                    join public.""LOV"" as lov on lov.""Id""=p.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
					where p.""AssignedToUserId""='{userId}'
                    )
                    t on t.""taskid"" = nt.""Id""
	                left join public.""NtsTask"" as ts on ts.""ParentTaskId""=t.""taskid"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
	               where nt.""IsDeleted""=false and nt.""CompanyId""='{_uc.CompanyId}' group by t.""AssignedToUserId"",t.""CreatedDate"", t.""taskid"", t.""taskname"", t.""taskstatus"" order by t.""CreatedDate"" desc ";

            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDateData(string projectId)
        {
            string query = "";

            query = $@"	WITH RECURSIVE NtsService AS(
                        SELECT t.""AssignedToUserId"",t.""StartDate"", t.""TaskSubject"" as ts, s.""Id"", t.""Id"" as ""TaskId"", s.""ServiceSubject""
                        FROM public.""NtsService"" as s

                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                        union all
                        SELECT t.""AssignedToUserId"", t.""StartDate"", t.""TaskSubject"" as ts, s.""Id"", t.""Id"" as ""TaskId"", s.""ServiceSubject""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
	                                          
                    )
                    SELECT distinct ""StartDate"" as StartDate,""TaskId"",""Id"" from NtsService
                    order by ""StartDate"" desc";

            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByDate(string projectId, DateTime startDate, bool isProjectManager = false)
        {
            string query = "";
            if (isProjectManager)
            {
                query = $@"WITH RECURSIVE NtsService AS ( 
                         SELECT t.""AssignedToUserId"", t.""TaskSubject"",t.""StartDate"", t.""Id"" as taskid,t.""TaskStatusId"",s.""Id"",s.""Id"" as ServiceId ,s.""ServiceSubject""

                        FROM public.""NtsService"" as s

                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
	                    
						where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"", t.""StartDate"" , t.""Id"" as taskid, t.""TaskStatusId"", s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""	 and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'                         
	                          where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'                 
                    )
                    SELECT  distinct p.""StartDate"", p.""taskid"" as TaskId, p.""TaskSubject"" as TaskName, lov.""Name"" as TaskStatus,
                     u.""Id"" as UserId, u.""Name"" as UserName, u.""PhotoId""
                    from NtsService as p
                    
                    join public.""LOV"" as lov on lov.""Id""=p.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
                    join public.""User"" as u on u.""Id""=p.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'

                    where p.""StartDate""='{startDate}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' ";

            }
            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            
            return queryData;
        }

        public async Task<ServiceViewModel> GetProjectDetails(string projectId)
        {
            var query = @$"select p.""StartDate"", p.""DueDate"" ,""ServiceNo"", sp.""Name"" as Priority, ss.""Name"" as ServiceStatusName
                            from public.""NtsService"" as p
                            join public.""LOV"" as sp on sp.""Id"" = p.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = p.""ServiceStatusId""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                            where p.""Id"" = '{projectId}' and p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQuerySingle<ServiceViewModel>(query, null);
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
						left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						where  s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                        union all
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"" as ts, s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                       where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                    SELECT ""AssignedToUserId"" as UserId from NtsService
                    where ""AssignedToUserId"" is not null group by ""AssignedToUserId"") p
                    on u.""Id""=p.""userid"" where u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'";
            }
            else
            {
                query = $@"select u.""Id"" as UserId, u.""Name"" as UserName, u.""PhotoId"" from public.""User"" as u
                        join(
                        WITH RECURSIVE NtsService AS(
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"" as ts, s.""Id"", s.""Id"" as ""ServiceId"", s.""ServiceSubject""
                        FROM public.""NtsService"" as s
                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""AssignedToUserId""='{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                        union all
                        SELECT t.""AssignedToUserId"", t.""TaskSubject"" as ts, s.""Id"", ns.""Id"" as ""ServiceId"", s.""ServiceSubject"" 
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""AssignedToUserId""='{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                       where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                    SELECT ""AssignedToUserId"" as UserId from NtsService
                    where ""AssignedToUserId"" is not null group by ""AssignedToUserId"") p
                    on u.""Id""=p.""userid"" where u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' ";
            }
            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetProjectsList(string userId, bool isProjectManager)
        {
            string query = "";
            if (_uc.IsSystemAdmin)
            {
                query = @$"select s.""ServiceSubject"" as Name, s.""Id"" as Id from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                            where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'";

            }
            else if (isProjectManager)
            {
                query = @$"select s.""ServiceSubject"" as Name, s.""Id"" as Id from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                            where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'";
            }
            else
            {
                query = $@"with recursive ntsservice as(
	                        select t.""AssignedToUserId"",s.""OwnerUserId"",ou.""Name"" as ""OwnerName"",s.""RequestedByUserId"",s.""Id"",s.""Id"" as ""ParentServiceId"", s.""ServiceSubject"" from ""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
	                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                            where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                            union all

                            select t.""AssignedToUserId"",s.""OwnerUserId"",ns.""OwnerName"" as ""OwnerName"",s.""RequestedByUserId"", s.""Id"", ns.""ParentServiceId"" as ""ParentServiceId"", ns.""ServiceSubject""
from ""NtsService"" as s
                            inner join ntsservice as ns on s.""ParentServiceId"" = ns.""Id""
  join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""IsDeleted""=false  
                            left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and (t.""AssignedToUserId"" = '{userId}'  or s.""OwnerUserId""='{userId}' )
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                           where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
and (temp.""ViewType"" is null or   temp.""ViewType""=1 ) )
                            select ""ParentServiceId"" as Id, ""ServiceSubject"" as Name from ntsservice
                            where --""AssignedToUserId"" is not null
                      (""AssignedToUserId"" = '{userId}'  or ""OwnerUserId""='{userId}' or ""RequestedByUserId""='{userId}')
                            group by ""ParentServiceId"", ""ServiceSubject"" ";
            }
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadWBSTimelineGanttChartData(string userId, string projectId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
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
                 SELECT s.*, s.""Id"" as ""ServiceId"",'Service' as ""Type"", s.""LockStatusId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",s.""Id"" as ""ProjectId""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"",s.""DueDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode"" 
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					  where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",'Stage' as ""Type"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ns.""ProjectId"" as ""ProjectId""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"",s.""DueDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                  where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                            )
                 SELECT ""Id"",""ServiceSubject"" as Title,""Type"" ,""StartDate"" as PlannedStart,""DueDate"" as PlannedEnd,""ParentId"",""UserName"",true as Summary,""Priority"",""NtsStatus"",""ProjectId""
                    ,""ActualStartDate"" as Start,""ActualEndDate"" as End,""StatusGroupCode""
                    from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title,'Task' as ""Type"", t.""StartDate"" as Start, t.""DueDate"" as End,case when t.""ParentTaskId"" is null then nt.""Id"" else t.""ParentTaskId"" end as ""ParentId"", u.""Name"" as ""UserName"",true as Summary, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",nt.""ProjectId"" as ""ProjectId"" 
                    ,coalesce(t.""ActualStartDate"",t.""StartDate"") as ""ActualStartDate"",coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"",t.""ActualStartDate"",t.""StartDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                     where  t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' #OwnerWhere#  #AssigneeWhere# #StatusWhere# #DateWhere#      
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

            if (projectId.IsNotNull())
            {
                projectSerach = @$" and s.""Id""='{projectId}' ";

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
            if (projectId.IsNullOrEmpty() && projectIds.Count == 0)
            {
                projectSerach = @$" and tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}'";
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
            if (userRole.IsNotNull() && userRole.Contains("PROJECT_USER"))
            {
                projectSerach = @$" and tt.""Code""='PROJECT_SUPER_SERVICE'";
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
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel3userIfAdmin(string monthYear, string status)
        {
            var query = @$"select s.""ServiceSubject""||'-'||ou.""Name""||'-'||to_char(s.""StartDate"",'dd-MM-yyyy') as Name, s.""Id"" as Id from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                            where to_char(s.""StartDate"",'MM-yyyy')='{monthYear}' and sp.""Code""='{status}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel3userIfProjectManager(string monthYear, string status, string userId, string userIds)
        {
            var query = @$"select s.""ServiceSubject""||'-'||ou.""Name""||'-'||to_char(s.""StartDate"",'dd-MM-yyyy') as Name, s.""Id"" as Id from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                            where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}' or s.""OwnerUserId"" in ('{userIds}')) and to_char(s.""StartDate"",'MM-yyyy')='{monthYear}' and sp.""Code""='{status}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel3(string monthYear, string status, string userId)
        {
            var query = $@"with recursive ntsservice as(
	                        select t.""AssignedToUserId"",s.""OwnerUserId"",ou.""Name"" as ""OwnerName"",s.""RequestedByUserId"",s.""Id"",s.""Id"" as ""ParentServiceId"", s.""ServiceSubject"",s.""StartDate"" from ""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as ou on s.""OwnerUserId""=ou.""Id"" 
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
	                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                            where tt.""Code""='PROJECT_SUPER_SERVICE' and to_char(s.""StartDate"",'MM-yyyy')='{monthYear}' and sp.""Code""='{status}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                            union all

                            select t.""AssignedToUserId"",s.""OwnerUserId"",ns.""OwnerName"" as ""OwnerName"",s.""RequestedByUserId"", s.""Id"", ns.""ParentServiceId"" as ""ParentServiceId"", ns.""ServiceSubject"",ns.""StartDate""
from ""NtsService"" as s
                            inner join ntsservice as ns on s.""ParentServiceId"" = ns.""Id""
  join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""IsDeleted""=false  
                            left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and (t.""AssignedToUserId"" = '{userId}'  or s.""OwnerUserId""='{userId}' )
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                           where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
and (temp.""ViewType"" is null or   temp.""ViewType""=1 ) )
                            select ""ParentServiceId"" as Id, ""ServiceSubject""||'-'||""OwnerName""||'-'||to_char(""StartDate"",'dd-MM-yyyy') as Name from ntsservice
                            where --""AssignedToUserId"" is not null
                      (""AssignedToUserId"" = '{userId}'  or ""OwnerUserId""='{userId}' or ""RequestedByUserId""='{userId}')
                            group by ""ParentServiceId"", ""ServiceSubject"",""OwnerName"",""StartDate"" ";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel2userIfAdmin(string monthYear)
        {
            var query = @$"select sp.""Name"" as Name,'LEVEL3#'||to_char(s.""StartDate"",'MM-yyyy')||'#'||sp.""Code"" as Id,true as HasChildren from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false    
                        where  s.""IsDeleted""=false and to_char(s.""StartDate"",'MM-yyyy')='{monthYear}' and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel2userIfProjectManager(string monthYear, string userId, string userIds)
        {
            var query = @$"select sp.""Name"" as Name,'LEVEL3#'||to_char(s.""StartDate"",'MM-yyyy')||'#'||sp.""Code"" as Id,true as HasChildren from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                             join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false 
                            where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}' or s.""OwnerUserId"" in ('{userIds}')) and to_char(s.""StartDate"",'MM-yyyy')='{monthYear}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel2(string monthYear, string userId)
        {
            var query = $@"with recursive ntsservice as(
	                        select t.""AssignedToUserId"",s.""OwnerUserId"",s.""RequestedByUserId"",s.""Id"",s.""StartDate"",sp.""Code"" as ""Status"",sp.""Name"" as ""StatusName"" from ""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false   
	                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                            where tt.""Code""='PROJECT_SUPER_SERVICE' and to_char(s.""StartDate"",'MM-yyyy')='{monthYear}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                            union all

                            select t.""AssignedToUserId"",s.""OwnerUserId"",s.""RequestedByUserId"", s.""Id"",ns.""StartDate"",ns.""Status"" as ""Status"",ns.""StatusName"" as ""StatusName""
from ""NtsService"" as s
                            inner join ntsservice as ns on s.""ParentServiceId"" = ns.""Id""
  join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""IsDeleted""=false 
join public.""LOV"" as sp on s.""ServiceStatusId""=sp.""Id""  and sp.""IsDeleted""=false   
                            left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and (t.""AssignedToUserId"" = '{userId}'  or s.""OwnerUserId""='{userId}' )
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                           where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
and (temp.""ViewType"" is null or   temp.""ViewType""=1 ) )
                            select ""StatusName"" as Name,'LEVEL3#'||to_char(""StartDate"",'MM-yyyy')||'#'||""Status"" as Id,true as HasChildren from ntsservice
                            where --""AssignedToUserId"" is not null
                      (""AssignedToUserId"" = '{userId}'  or ""OwnerUserId""='{userId}' or ""RequestedByUserId""='{userId}')
                            group by Name,Id ";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel1userIfAdmin()
        {
            var query = @$"select to_char(s.""StartDate"",'MONTH-yyyy') as Name,'LEVEL2#'||to_char(s.""StartDate"",'MM-yyyy') as Id,true as HasChildren from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            where  s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel1userIfProjectManager(string userId, string userIds)
        {
            var query = @$"select to_char(s.""StartDate"",'MONTH-yyyy') as Name,'LEVEL2#'||to_char(s.""StartDate"",'MM-yyyy') as Id,true as HasChildren from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                            where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}' or s.""OwnerUserId"" in ('{userIds}')) and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' group by Name,Id";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetPrjLevel1(string userId)
        {
            var query = $@"with recursive ntsservice as(
	                        select t.""AssignedToUserId"",s.""OwnerUserId"",s.""RequestedByUserId"",s.""Id"",s.""StartDate"" from ""NtsService"" as s
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
	                        left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                            where tt.""Code""='PROJECT_SUPER_SERVICE'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'

                            union all

                            select t.""AssignedToUserId"",s.""OwnerUserId"",s.""RequestedByUserId"", s.""Id"",ns.""StartDate""
from ""NtsService"" as s
                            inner join ntsservice as ns on s.""ParentServiceId"" = ns.""Id""
  join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""IsDeleted""=false  
                            left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
--and (t.""AssignedToUserId"" = '{userId}'  or s.""OwnerUserId""='{userId}' )
--and t.""AssignedToUserId"" = '{userId}' 
and t.""ParentServiceId"" is not null
                           where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
and (temp.""ViewType"" is null or   temp.""ViewType""=1 ) )
                            select to_char(""StartDate"",'MONTH-yyyy') as Name,'LEVEL2#'||to_char(""StartDate"",'MM-yyyy') as Id,true as HasChildren from ntsservice
                            where --""AssignedToUserId"" is not null
                      (""AssignedToUserId"" = '{userId}'  or ""OwnerUserId""='{userId}' or ""RequestedByUserId""='{userId}')
                            group by Name,Id ";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TaskViewModel>> ReadTaskOverdueData(string projectId)
        {
            var query = @$" select t.*,concat(u.""Name"",' ',u.""Email"") as ""AssigneeDisplayName""
                        from public.""NtsTask"" as t
                        join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ts on ts.""Id""=t.""TaskStatusId"" and ts.""IsDeleted""=false and ts.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' AND ts.""Code""='TASK_STATUS_OVERDUE' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                        ";
            var queryData = await _queryRepo.ExecuteQueryList<TaskViewModel>(query, null);
            return queryData;

        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectSubTaskViewData(string taskId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null)
        {
            var query = @$"
                 select t.""Id"",t.""TaskSubject"" as TaskName,l.""Name"" as TaskStatus,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,l.""Code"" as ""TaskStatusCode""
              ,t.""TaskPriorityId"" as Priority,t.""TaskStatusId"" as TaskStatusId,t.""Id"" as id,t.""ParentTaskId"" as parentId,t.""TaskSubject"" as text,t.""ParentTaskId"" as parent
               FROM public.""NtsTask"" as t
              left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
               left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_uc.CompanyId}'
				 where t.""ParentTaskId""='{taskId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' #AssigneeUser#  #OwnerUser# #StatusWhere#";


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

            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<ProgramDashboardViewModel> ReadProjectTotalTaskDataOld(string projectId)
        {

            var query1 = $@"select  s.""Id"" as id,s.""DueDate"" as DueDate,
                     (t.""Count""+pt.""PCount"") as AllTaskCount,
                       s.""Id"" as ProjectId,(t.""CompletedCount""+pt.""PCompletedCount"") as CompletedCount

                      FROM public.""NtsService"" as s
                      join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                     left join (
				
					 select s.""Id"",

                    count(t.""TaskSubject"") as ""PCount"",
					sum(case when lov.""Code"" = 'TASK_STATUS_COMPLETED' and s.""Id"" is not null then 1 else 0 end) as ""PCompletedCount""	
                     from public.""NtsService"" as s
                     join public.""NtsTask"" as t on t.""ParentServiceId""=s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
					  join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
					left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId""   and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
						where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{_repo.UserContext.UserId}'
                        and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                         group by s.""Id""
				) pt on pt.""Id""=s.""Id""
                       left join(
                   WITH RECURSIVE NtsService AS ( 
                          SELECT s.""ServiceSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"",lov.""Code"" as code ,'Parent' as Type
                              FROM public.""NtsService"" as s
                              join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
           -- left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                     left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
            union all
                              SELECT t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" ,lov.""Code"" as code ,'Child' as Type
                              FROM public.""NtsService"" as s
                              join NtsService ns on s.""ParentServiceId""=ns.""Id""
                              join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                                left join public.""LOV"" as lov on lov.""Id""=t.""TaskStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                          )
                          SELECT count(ts) as ""Count"",
                        	sum(case when ""code"" = 'TASK_STATUS_COMPLETED' and ""ServiceId"" is not null then 1 else 0 end) as ""CompletedCount"",
                    ""ServiceId"" from NtsService  where Type!='Parent' group by ""ServiceId"",""code""

                      ) t on s.""Id""=t.""ServiceId""

                      Where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                     --GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count"" ";
            var queryData1 = await _queryRepo.ExecuteQuerySingle<ProgramDashboardViewModel>(query1, null);
            return queryData1;
        }

        public async Task<ProgramDashboardViewModel> ReadProjectTotalTaskData(string projectId)
        {

            var query1 = $@"select count(p.""ts"") as AllTaskCount,s.""Id"" as id,s.""DueDate"" as DueDate,
  sum(case when lov.""Code"" = 'TASK_STATUS_INPROGRESS' and p.""tsid"" is not null then 1 else 0 end) as ""InProgreessCount"",
  sum(case when lov.""Code"" = 'TASK_STATUS_COMPLETE' and p.""tsid"" is not null then 1 else 0 end) as ""CompletedCount""
  
  FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
               where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid
                        FROM public.""NtsTask"" as t
                        --join NtsService ns on s.""ParentServiceId""=ns.""Id""
                       -- join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
	                    --left join public.""User"" as u on u.""Id"" =t.""AssignedToUserId""  
				where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' )p on p.pid=t.""Id""
				left join public.""LOV"" as lov on lov.""Id"" = p.""tsid"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_uc.CompanyId}'
				where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
               group by s.""Id"",s.""DueDate""
";
            var queryData1 = await _queryRepo.ExecuteQuerySingle<ProgramDashboardViewModel>(query1, null);
            return queryData1;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var query = @$"
                 select count(st) as SubTaskCount,'{projectId}' as ProjectId,t.""TaskStatusId"" as TaskStatusId,l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,u.""Id"" as UserId,u.""Name"" as UserName,t.""Id"" as TaskId,SUBSTRING (t.""TaskSubject"", 1, 30)  as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority
   ,l.""Code"" as TaskStatusCode           
FROM public.""NtsTask"" as t
                 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    left join public.""NtsTask"" as st on st.""ParentTaskId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_uc.CompanyId}'
                      left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_uc.CompanyId}'
                 where t.""ParentServiceId""='{projectId}' and t.""ParentTaskId"" is null and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'  #AssigneeUser# #StatusWhere# #OwnerUser# #DateWhere#
   group by t.""Id"",t.""TaskSubject"",t.""StartDate"",t.""DueDate"",u.""Name"",t.""TaskPriorityId"",u.""PhotoId"",
   u.""Id"",u.""Name"",l.""Name"",l.""Code""

               
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
            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var query = @$"
                 select count(st) as SubTaskCount,'{projectId}' as ProjectId,t.""TaskStatusId"" as TaskStatusId,l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,u.""Id"" as UserId,u.""Name"" as UserName,t.""Id"" as TaskId,SUBSTRING (t.""TaskSubject"", 1, 30)  as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority
 ,l.""Code"" as TaskStatusCode               
FROM public.""NtsTask"" as t
                 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""     and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    left join public.""NtsTask"" as st on st.""ParentTaskId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_uc.CompanyId}'
                      left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId""     and l.""IsDeleted""=false and l.""CompanyId""='{_uc.CompanyId}'
                 where t.""ParentServiceId""='{projectId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' and t.""ParentTaskId"" is null  and (t.""AssignedToUserId"" ='{_repo.UserContext.UserId}' or st.""AssignedToUserId"" ='{_repo.UserContext.UserId}')  #AssigneeUser#  #OwnerUser# #StatusWhere# #DateWhere#
   group by t.""Id"",t.""TaskSubject"",t.""StartDate"",t.""DueDate"",u.""Name"",t.""TaskPriorityId"",u.""PhotoId"",
   u.""Id"",u.""Name"",l.""Name"",l.""Code""
       
                        union

            select count(st) as SubTaskCount,'{projectId}' as ProjectId,t.""TaskStatusId"" as TaskStatusId,l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,u.""Id"" as UserId,u.""Name"" as UserName,t.""Id"" as TaskId,SUBSTRING (t.""TaskSubject"", 1, 30)  as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority
,l.""Code"" as TaskStatusCode                    
FROM public.""NtsTask"" as t
                 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId""       and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    left join public.""NtsTask"" as st on st.""ParentTaskId""=t.""Id""   and st.""IsDeleted""=false and st.""CompanyId""='{_uc.CompanyId}'
                      left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId""       and l.""IsDeleted""=false and l.""CompanyId""='{_uc.CompanyId}'
                    left join public.""NtsService"" as nts on nts.""Id""=t.""ParentServiceId"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_uc.CompanyId}'
                 where t.""ParentServiceId""='{projectId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' and t.""ParentTaskId"" is null and nts.""OwnerUserId""='{_repo.UserContext.UserId}' #AssigneeUser#  #OwnerUser# #StatusWhere# #DateWhere#
   group by t.""Id"",t.""TaskSubject"",t.""StartDate"",t.""DueDate"",u.""Name"",t.""TaskPriorityId"",u.""PhotoId"",
   u.""Id"",u.""Name"",l.""Name"",l.""Code""
                


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
            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectStageViewData(string projectId)
        {
            var query = @$"
                select t.""Id"" as Id,t.""RequestByUserId"" as RequestedByUserId,t.""ProjectOwnerId"" as ProjectOwnerId,t.""OwnerName"" as ProjectOwnerName,t.""ServiceSubject"" as StageName,(t.""num""*44)-44 as Level,t.""ParentId"" as parentId FROM public.""NtsService"" as s
                    
join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""RequestedByUserId"" as ""RequestByUserId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s

                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",s.""RequestedByUserId"" as ""RequestByUserId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
 join public.""Template"" as temp on temp.""Id""=s.""TemplateId"" and temp.""IsDeleted""=false
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""    and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""   and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                       left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' and (temp.""ViewType"" is null or   temp.""ViewType""=1 ) )
                 SELECT  ""OwnerName"",""ProjectOwnerId"",""RequestByUserId"",""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
               		
                where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'       ";


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectStageViewData(string projectId)
        {
            var query = @$"
                select t.""Id"" as Id,t.""RequestByUserId"" as RequestedByUserId,t.""ProjectOwnerId"" as ProjectOwnerId,t.""OwnerName"" as ProjectOwnerName,t.""ServiceSubject"" as StageName,(t.""num""*44)-44 as Level,t.""ParentId"" as parentId FROM public.""NtsService"" as s
                   
join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""RequestedByUserId"" as ""RequestByUserId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",2 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",s.""RequestedByUserId"" as ""RequestByUserId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
 join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""IsDeleted""=false                           
join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                       left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
               where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'    and (temp.""ViewType"" is null or   temp.""ViewType""=1 )   )
                 SELECT  ""OwnerName"",""ProjectOwnerId"",""RequestByUserId"",""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
                left join public.""NtsTask"" as nt on nt.""ParentServiceId""=t.""Id""

                where s.""Id""='{projectId}' 
and nt.""AssignedToUserId""='{_repo.UserContext.UserId}' 
and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
             
                union
                
                 select t.""Id"" as Id,t.""RequestByUserId"" as RequestedByUserId,t.""ProjectOwnerId"" as ProjectOwnerId,t.""OwnerName"" as ProjectOwnerName,t.""ServiceSubject"" as StageName,(t.""num""*44)-44 as Level,t.""ParentId"" as parentId FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"",s.""RequestedByUserId"" as ""RequestByUserId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",2 as ""num""
                        FROM public.""NtsService"" as s
                 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"",s.""RequestedByUserId"" as ""RequestByUserId"",u.""Name"" as ""OwnerName"",u.""Id"" as ""ProjectOwnerId"" ,s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                       join public.""Template"" as temp on temp.""Id""=s.""TemplateId""  and temp.""IsDeleted""=false  
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""    and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""   and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                       left join public.""User"" as u on u.""Id"" = s.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
               where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  and (temp.""ViewType"" is null or   temp.""ViewType""=1 )   )
                 SELECT  ""OwnerName"",""ProjectOwnerId"",""RequestByUserId"",""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
               		
                where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
--and s.""OwnerUserId""='{_repo.UserContext.UserId}'
and (t.""ProjectOwnerId""='{_repo.UserContext.UserId}' or t.""RequestByUserId""='{_repo.UserContext.UserId}')

union

 select t.""Id"" as Id,t.""RequestedByUserId"" as RequestByUserId,t.""OwnerUserId"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,t.""ServiceSubject"" as StageName,44 as Level ,t.""ParentServiceId"" as parentId
FROM public.""NtsService"" as t
 join public.""Template"" as temp on temp.""Id""=t.""TemplateId""  and temp.""IsDeleted""=false  
 left join public.""User"" as u on u.""Id"" = t.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'              
where t.""Id""='{projectId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 

";


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData.OrderBy(x => x.Level).ToList();
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedDataOld(string projectId)
        {
            var query = $@"select  s.""Id"" as id,
                 sum(t.""Count"")::integer as UserTaskCount,
                s.""Id"" as ProjectId,t.""pid"" as PhotoId,t.""username"" as UserName
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                  left join( 
					 
	          select t.""PCount"" as ""Count"",s.""Id"",t.""Ppid"" as ""pid"",t.""Pusername"" as username from public.""NtsService"" as s
join(
SELECT count(t.""TaskSubject"") as ""PCount"", s.""Id"", u.""PhotoId"" as ""Ppid"", u.""Name"" as ""Pusername""
                        FROM public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""      and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""    and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						left join public.""User"" as u on u.""Id"" =t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					where tt.""Code""='PROJECT_SUPER_SERVICE'
						group by s.""Id"", u.""PhotoId"", u.""Name"")t on t.""Id""=s.""Id""
						where s.""Id"" = '{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
union all
select t.""Count"" as ""Count"", s.""Id"", t.""pid"" as ""pid"", t.""username"" as username from public.""NtsService"" as s
   join(
      WITH RECURSIVE NtsService AS (
                    SELECT s.""ServiceSubject"" as ts, s.""Id"", s.""Id"" as ""ServiceId"" , u.""PhotoId"" as pid, u.""Name"" as username,'Parent' as Type
                        FROM public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId""           and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						--left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
						left join public.""User"" as u on u.""Id"" =s.""OwnerUserId""           and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                    where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        union all
                        SELECT t.""TaskSubject"" as ts, s.""Id"", ns.""ServiceId"" as ""ServiceId"" , u.""PhotoId"" as pid, u.""Name"" as username,'Child' as Type
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""           
                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""     and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
	                    left join public.""User"" as u on u.""Id"" =t.""AssignedToUserId""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'   
	                      where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'               
                    )
                    SELECT count(distinct ts) as ""Count"",""ServiceId"",""pid"",""username"" from NtsService
 where Type!='Parent'
group by ""ServiceId"",""pid"",""username""
                 
                ) t on s.""Id""=t.""ServiceId""
						where s.""Id"" = '{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						
						
                 
                ) t on s.""Id""=t.""Id""

                Where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""Id"" = '{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
               GROUP BY s.""Id"", t.""pid"",t.""username"" ";


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskAssignedData(string projectId)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""    and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""   and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid
                        FROM public.""NtsTask"" as t
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
               group by s.""Id"", s.""DueDate"", u.""Id"" ";


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedData(string projectId)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""     and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid
                        FROM public.""NtsTask"" as t
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				where s.""Id""='{projectId}' and p.""auid""='{_repo.UserContext.UserId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

                union

                select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""    and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""   and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"",""OwnerUserId"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid
                        FROM public.""NtsTask"" as t
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' and t.""OwnerUserId""='{_repo.UserContext.UserId}' and p.""auid""<>'{_repo.UserContext.UserId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

";


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskOwnerData(string projectId)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""OwnerUserId"" as auid
                        FROM public.""NtsTask"" as t
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""auid"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
               group by s.""Id"", s.""DueDate"", u.""Id"" ";


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskOwnerData(string projectId)
        {
            var query = $@"
               select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
              where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'   )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid ,t.""OwnerUserId"" as ouid
                        FROM public.""NtsTask"" as t
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""ouid""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				where s.""Id""='{projectId}' and p.""auid""='{_repo.UserContext.UserId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

                union

                select count(p.""ts"") as UserTaskCount,s.""Id"" as id,u.""PhotoId"" as PhotoId,u.""Name"" as UserName,u.""Id"" as UserId
              FROM public.""NtsService"" as s
                        join(
                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",1 as ""num""
                        FROM public.""NtsService"" as s
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        where s.""Id""='{projectId}'  and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
						union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp, sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ns.""num""+1 as ""num""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""num"",""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"",""OwnerUserId"" from NtsService ) t on t.tmp=s.""Id""
				join(
                 SELECT t.""ParentServiceId"" as pid, t.""TaskSubject"" as ts, t.""TaskStatusId"" as tsid, t.""AssignedToUserId"" as auid ,t.""OwnerUserId"" as ouid
                        FROM public.""NtsTask"" as t
                        
				)p on p.pid=t.""Id""
				
				left join public.""User"" as u on u.""Id""=p.""ouid"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
				where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' and t.""OwnerUserId""='{_repo.UserContext.UserId}' and p.""auid""<>'{_repo.UserContext.UserId}'
               group by s.""Id"", s.""DueDate"", u.""Id""

";


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<DashboardCalendarViewModel>> ReadProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var query = @$"
                  WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					 where  tt.""Code""='PROJECT_SUPER_SERVICE' and  s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                  SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""OwnerName"" as UserName,""OwnerUserId"" as ""AssigneeUserId"",""OwnerUserId"" as ""OwnerUserId"",""NtsStatus"" as StatusName,""ParentId"" as TaskStatusId,'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End,u.""Name"" as UserName,t.""AssignedToUserId"" as AssigneeUserId,t.""OwnerUserId"" as OwnerUserId,ss.""Name"" as StatusName,t.""TaskStatusId"" as TaskStatusId,'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                     where (t.""AssignedToUserId""='{_repo.UserContext.UserId}' or nt.""OwnerUserId""='{_repo.UserContext.UserId}') and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'  
                     --#OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere#    
                    )
                 SELECT * from Nts where Level='Child'

               
";

            var queryData = await _queryRepo.ExecuteQueryList<DashboardCalendarViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<DashboardCalendarViewModel>> ReadManagerProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var query = @$"
                 WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					 	 where  tt.""Code""='PROJECT_SUPER_SERVICE' and  s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""OwnerName"" as UserName,""NtsStatus"" as StatusName,""ParentId"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End,u.""Name"" as UserName ,ss.""Name"" as StatusName,t.""TaskStatusId"" as TaskStatusId,'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        --join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" 
	                 where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'   #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere#        
                    )
                 SELECT * from Nts where Level='Child'
  

               
";
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
            query = query.Replace("#AssigneeWhere#", assigneeSerach);
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
            var queryData = await _queryRepo.ExecuteQueryList<DashboardCalendarViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetSubordinatesUserIdNameListuserIfAdmin()
        {
            string query = $@"select u.""Name"",u.""Id"" 
                                from public.""User"" as u
                                join public.""UserPortal"" up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                                where u.""IsDeleted""=false and up.""PortalId""='{_uc.PortalId}'
                                ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetSubordinatesUserIdNameList()
        {
            string query1 = $@"WITH RECURSIVE ChildUserHierarchy AS(
                                select  u.""Name"",u.""Id"",uh.""UserId""
                                from public.""UserHierarchy"" as uh
                                join public.""HierarchyMaster"" as hm on hm.""Id""=uh.""HierarchyMasterId"" and hm.""Code""='PROJECT_USER_HIERARCHY' and  hm.""IsDeleted""=false and hm.""CompanyId""='{_uc.CompanyId}'
                                join public.""User"" as u on u.""Id""=uh.""UserId"" and  u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                            where uh.""ParentUserId""='{_uc.UserId}' and uh.""LevelNo""=1 and uh.""OptionNo""=1 and  uh.""IsDeleted""=false and uh.""CompanyId""='{_uc.CompanyId}'

                                  union all

                                select u.""Name"",u.""Id"",uh.""UserId""
                                from public.""UserHierarchy"" as uh
                                join ChildUserHierarchy as cuh on uh.""ParentUserId""=cuh.""UserId""
                                join public.""HierarchyMaster"" as hm on hm.""Id""=uh.""HierarchyMasterId"" and hm.""Code""='PROJECT_USER_HIERARCHY' and  hm.""IsDeleted""=false and hm.""CompanyId""='{_uc.CompanyId}'
                                join public.""User"" as u on u.""Id""=uh.""UserId""  and  u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                            where  uh.""LevelNo""=1 and uh.""OptionNo""=1 and  uh.""IsDeleted""=false and uh.""CompanyId""='{_uc.CompanyId}'          )
                            select * from ChildUserHierarchy
                            ";
            
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query1, null);
            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatusRequestedByMe(string TemplateID, string UserID)
        {


            var query = @$"select LOV.""Name"" as Type,Count(*) as Value  
from public.""NtsTask"" as NTS 
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_uc.CompanyId}'  
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and Ur.""IsDeleted""=false and Ur.""CompanyId""='{_uc.CompanyId}' 
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and person.""IsDeleted""=false and person.""CompanyId""='{_uc.CompanyId}' 
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""=false and Assign.""CompanyId""='{_uc.CompanyId}' 
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""=false and Positions.""CompanyId""='{_uc.CompanyId}' 
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_uc.CompanyId}'  
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and person2.""IsDeleted""=false and person2.""CompanyId""='{_uc.CompanyId}' 
where person2.""UserId""='{UserID}'
 and ""TemplateId"" = '{TemplateID}' and NTS.""IsDeleted""=false and NTS.""CompanyId""='{_uc.CompanyId}' 
group by  LOV.""Name""
                          ";
            //query = query.Replace("#RequesUserID#",UserID).Replace("#TemplateID#",TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatusAssigneduserid(string TemplateID, string UserID)
        {


            var query = @$"select Ur.""Name"" as Type,Count(*) as Value, Ur.""Id"" 
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and Ur.""IsDeleted""=false and Ur.""CompanyId""='{_uc.CompanyId}' 
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and person.""IsDeleted""=false and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""=false and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""=false and Positions.""CompanyId""='{_uc.CompanyId}' 
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and person2.""IsDeleted""=false and person2.""CompanyId""='{_uc.CompanyId}'
where person2.""UserId""='{UserID}'
 and ""TemplateId"" = '{TemplateID}' and NTS.""IsDeleted""=false and NTS.""CompanyId""='{_uc.CompanyId}'

group by  Ur.""Name"", Ur.""Id""";
            //query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> MdlassignUser(string TemplateID, string UserID)
        {


            var query = @$"select Distinct(Ur.""Name"") as Type,Ur.""Id"" as RefId
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId""  and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id""  and Ur.""IsDeleted""=false and Ur.""CompanyId""='{_uc.CompanyId}' 
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and person.""IsDeleted""=false and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""=false and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id""  and Positions.""IsDeleted""=false and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and person2.""IsDeleted""=false and person2.""CompanyId""='{_uc.CompanyId}'
where person2.""UserId""='{UserID}' and NTS.""IsDeleted""=false and NTS.""CompanyId""='{_uc.CompanyId}'
 and ""TemplateId"" = '{TemplateID}'";
            ///  query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<List<NtsTaskChartList>> GetGridList(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs = null)
        {
            var query = @$"select NTS.""Id"", ""TaskSubject"",""TaskNo"",Ur.""Name"" as AssignName,LOV.""Name"" as Priority,LOVS.""Name"" as Status,NTS.""StartDate"",NTS.""DueDate""
from public.""NtsTask"" as NTS
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskPriorityId"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""LOV"" as LOVS on LOVS.""Id""=NTS.""TaskStatusId"" and LOVS.""IsDeleted""=false and LOVS.""CompanyId""='{_uc.CompanyId}'  
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id"" and Ur.""IsDeleted""=false and Ur.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and person.""IsDeleted""=false and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""=false and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""=false and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_uc.CompanyId}' 
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and person2.""IsDeleted""=false and person2.""CompanyId""='{_uc.CompanyId}'
where person2.""UserId""='{UserID}' and NTS.""IsDeleted""=false and NTS.""CompanyId""='{_uc.CompanyId}'
 and ""TemplateId"" = '{TemplateID}' #AssigneeUser# #TaskStatusId# ";

            var assigneeSerach = "";
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
                    assigneeSerach = @"and NTS.""AssignedToUserId"" in ('" + aids + "') ";
                }
            }
            query = query.Replace("#AssigneeUser#", assigneeSerach);


            var StatusSerach = "";
            if (StatusIDs.IsNotNullAndNotEmpty())
            {
                string aids = StatusIDs.Replace(",", "','");
                //foreach (var i in StatusIDs)
                //{
                //    aids += $"'{i}',";
                //}
                //aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    StatusSerach = @"and NTS.""TaskStatusId"" in ('" + aids + "') ";
                }
            }
            query = query.Replace("#TaskStatusId#", StatusSerach);

            //query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<NtsTaskChartList>(query, null);

            //var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetDatewiseTask(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null)
        {


            var query = @$"select CAST(NTS.""DueDate""::TIMESTAMP::DATE AS VARCHAR) as Type,Avg(NTS.""TaskSLA"") as Days from public.""NtsTask"" as NTS 
left join public.""LOV"" as LOV on LOV.""Id""=NTS.""TaskStatusId""  and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_uc.CompanyId}'
left join public.""User"" Ur on NTS.""AssignedToUserId""=ur.""Id""  and Ur.""IsDeleted""=false and Ur.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person on ur.""Id""=person.""UserId"" and person.""IsDeleted""=false and person.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRAssignment""  as Assign on person.""Id""=Assign.""EmployeeId"" and Assign.""IsDeleted""=false and Assign.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as Positions on Assign.""PositionId""= Positions.""Id"" and Positions.""IsDeleted""=false and Positions.""CompanyId""='{_uc.CompanyId}'
left join  cms.""N_CoreHR_HRAssignment""  as Assign2 on Positions.""ParentPositionId""=Assign2.""PositionId"" and Assign2.""IsDeleted""=false and Assign2.""CompanyId""='{_uc.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as person2 on Assign2.""EmployeeId""=person2.""Id"" and person2.""IsDeleted""=false and person2.""CompanyId""='{_uc.CompanyId}'
where NTS.""DueDate""::TIMESTAMP::DATE >='{FromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and NTS.""DueDate""::TIMESTAMP::DATE >='{ToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE 
and NTS.""TaskStatusId""='370d3df9-5e97-4127-81e1-d7dd8fe8836a' and NTS.""IsDeleted""=false and NTS.""CompanyId""='{_uc.CompanyId}'
 Group by NTS.""DueDate""::TIMESTAMP::DATE ";
            //query = query.Replace("#RequesUserID#",UserID).Replace("#TemplateID#",TemplateID);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetTaskType(string projectId)
        {
            //var query = @$" select count(u.""Id"") as ""Value"",u.""Name"" as ""Type"",u.""Id"" as ""Id""
            //            from public.""NtsTask"" as t
            //            join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId""
            //            join public.""LOV"" as ts on ts.""Id""=t.""TaskStatusId""
            //            join public.""User"" as u on u.""Id""=t.""AssignedToUserId""
            //            where s.""Id""='{projectId}' AND (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
            //            group by u.""Id"",u.""Name""
            //            ";
            //var queryData = await _queryProjDashChartRepo.ExecuteQueryList(query, null);
            //return queryData;

            var search = @$" and s.""Id""='{projectId}' ";

            if (projectId.IsNotNull())
            {
                search = @$" and s.""Id""='{projectId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" 
                         FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Id"" as ""UserId"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}')
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""UserId"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Id"" as ""UserId"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                       where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'      
                    )
                 SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ProjectGanttTaskViewModel>> ReadProjectStageChartData(string userId, string projectId)
        {
            var search = @$" and tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}'";

            if (projectId.IsNotNull())
            {
                search = @$" and s.""Id""='{projectId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}')
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                        where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'     
                    )
                 SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;

        }

        public async Task<List<ProjectGanttTaskViewModel>> GetProjectStageIdNameList(string userId, string projectId)
        {
            var search = @$"and tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}'";

            if (projectId.IsNotNull())
            {
                search = @$"and s.""Id""='{projectId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
					 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId""  and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
               where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""   and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}'
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""     and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}'
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                             
                    )
                 SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;

        }

        public async Task<List<ProjectGanttTaskViewModel>> GetTaskStatus(string projectId)
        {
            //var query = @$" select count(ts.""Code"") as ""Value"",ts.""Name"" as ""Type"",ts.""Code"" as ""Code""
            //            from public.""NtsTask"" as t
            //            join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId""
            //            join public.""LOV"" as ts on ts.""Id""=t.""TaskStatusId""
            //            where s.""Id""='{projectId}'
            //            group by ts.""Code"",ts.""Name""
            //            ";
            //var queryData = await _queryProjDashChartRepo.ExecuteQueryList(query, null);
            //return queryData;

            var search = @$" and s.""Id""='{projectId}' ";

            if (projectId.IsNotNull())
            {
                search = @$" and s.""Id""='{projectId}' ";

            }
            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
					 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusId"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""   and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                       where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'       
                    )
                 SELECT * from Nts where Level='Child'";
            query = query.Replace("#SEARCH#", search);
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            return queryData;
        }

        public async Task<ProjectDashboardViewModel> GetPrjDashboardDetails(string projectId)
        {
            var query = @$"select count(t.""Id"") as TaskCount, p.""Id"" as ""Id"", p.""ServiceSubject"" as ""Name"",p.""ServiceNo"" as ""ServiceNo"",sp.""Name"" as ""Priority"",
                        u.""Id"" as ""UserId"",u.""Name"" as ""CreatedBy"",p.""LastUpdatedDate"" as ""UpdatedOn"",p.""CreatedDate"" as ""CreatedOn"",
                        p.""StartDate"" as ""StartDate"",p.""DueDate"" as ""DueDate"",  ss.""Name"" as ""ProjectStatus"",p.""ServiceDescription"" as ""Description"",
                        tt.""Id"" as ""TemplateId"",null as ""WbsTemplateId""
                            from public.""NtsService"" as p
                            join public.""LOV"" as sp on sp.""Id"" = p.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                            join public.""LOV"" as ss on ss.""Id"" = p.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                            left join public.""Template"" as tt on tt.""Id"" =p.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                            left join public.""User"" as u on u.""Id""=p.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                            left join public.""NtsTask"" as t on t.""ParentServiceId""=p.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
                            where p.""Id"" = '{projectId}' and p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}' 
                            group by p.""Id"",sp.""Name"",u.""Id"",ss.""Name"",tt.""Id""
                            ";

            var queryData = await _queryRepo.ExecuteQuerySingle<ProjectDashboardViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetPrjTaskDetails(string projectId)
        {
            var query1 = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
					    where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", ss.""Id"" as ""NtsStatusId""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                 )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",""NtsStatusId"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",ss.""Id"" as ""NtsStatusId"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                      where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'        
                    )
                 SELECT * from Nts where Level='Child'";
            var queryData1 = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query1, null);
            return queryData1;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadMindMapData(string projectId)
        {
            var query = @$"select t.""Id"",t.""ServiceSubject"" as Title,t.""StartDate"" as Start,t.""DueDate"" as End,""ParentId"",'' as ""UserName"",'' as ""OwnerName"",t.""Priority"",t.""NtsStatus"",t.""Type"",t.""ServiceId""
                        ,t.""TaskStatusCode""
                        FROM public.""NtsService"" as s 
                               join(
                        WITH RECURSIVE NtsService AS(
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""LockStatusId"" as ""ParentId"", '{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",'PROJECT' as ""Type""
                                ,ss.""Code"" as ""TaskStatusCode""
                               FROM public.""NtsService"" as s
                                join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                                   join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                               where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
            	union all
                               SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",'STAGE' as ""Type""
                                ,ss.""Code"" as ""TaskStatusCode""
                               FROM public.""NtsService"" as s
                               join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                                join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                                   join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                                    )
                        SELECT ""Id"",""ServiceSubject"",""StartDate"",""DueDate"",""ParentId"",tmp,""Priority"",""NtsStatus"",""Type"",""ServiceId"",""TaskStatusCode"" from NtsService ) t on t.tmp=s.""Id""
            where s.""Id""='{projectId}' 

            union
                        select t.""Id"",t.""TaskSubject"" as Title,t.""StartDate"" as Start,t.""DueDate"" as End,""ParentId"",t.""UserName"" as ""UserName"",t.""OwnerName"" as ""OwnerName"",t.""Priority"",t.""NtsStatus"",t.""Type"",t.""ServiceId""
                        ,t.""TaskStatusCode"" 
                        FROM public.""NtsService"" as s
                        join(
                       WITH RECURSIVE NtsService AS (
                           SELECT t.*, s.""Id"" as ""ServiceId"", case when t.""ParentTaskId"" is null then s.""Id"" else t.""ParentTaskId"" end as ""ParentId"",u.""Name"" as ""UserName"",u1.""Name"" as ""OwnerName"",'{projectId}'  as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",case when t.""ParentTaskId"" is null then 'TASK' else 'SUBTASK' end as ""Type""
                                ,ss.""Code"" as ""TaskStatusCode""
                               FROM public.""NtsService"" as s
                               join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
            	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""User"" as u1 on t.""OwnerUserId""=u1.""Id"" and u1.""IsDeleted""=false and u1.""CompanyId""='{_uc.CompanyId}' 
            	where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  
            	union all
                               SELECT t.*, s.""Id"" as ""ServiceId"",s.""Id"" as  ""ParentId"",u.""Name"" as ""UserName"" ,u1.""Name"" as ""OwnerName"",'{projectId}' as tmp,sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",case when t.""ParentTaskId"" is null then 'TASK' else 'SUBTASK' end as ""Type""
                                ,ss.""Code"" as ""TaskStatusCode""
                               FROM public.""NtsService"" as s
                               join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                               left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                               left join public.""User"" as u1 on t.""OwnerUserId""=u1.""Id"" and u1.""IsDeleted""=false and u1.""CompanyId""='{_uc.CompanyId}' 
                                where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                           )
                        SELECT ""Id"",""TaskSubject"",""StartDate"",""DueDate"",""ParentId"",""UserName"", ""OwnerName"",tmp,""Priority"",""NtsStatus"",""Type"",""ServiceId"",""TaskStatusCode"" from NtsService where ""Id"" is not null ) t on t.tmp=s.""Id""
            where s.""Id""='{projectId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' ";


            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);


            return queryData;
        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGridViewData(string userId, string projectId, string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {

            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",s.""TemplateCode"" as TemplateMasterCode
                            ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                            FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id""   and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
					  where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' and tt.""Code""='PROJECT_SUPER_SERVICE' #SEARCH#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"",s.""TemplateCode"" as TemplateMasterCode
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
                            join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                            where  s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level ,""TemplateCode"" as TemplateMasterCode
                        ,""ActualStartDate"",""ActualEndDate"",""StatusGroupCode""
                        from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level,t.""TemplateCode"" as TemplateMasterCode
                    ,t.""ActualStartDate"" as ""ActualStartDate"",coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                         join public.""Template"" as tt on tt.""Id"" =t.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                        left join public.""User"" as ou on t.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_uc.CompanyId}' 
	                     where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'  #OwnerWhere#  #AssigneeWhere# #StatusWhere# #DateWhere#   
                    )
                 SELECT * from Nts where Level='Child'";

            var projectSerach = "";

            if (projectId.IsNotNull())
            {
                projectSerach += @$" and s.""Id""='{projectId}' ";

            }
            else if (projectIds.IsNotNullAndNotEmpty())
            {
                string pids = projectIds.Replace(",", "','");
                //foreach (var i in projectIds)
                //{
                //    pids += $"'{i}',";
                //}
                //pids = pids.Trim(',');

                if (pids.IsNotNull())
                {
                    projectSerach += @" and s.""Id"" in ('" + pids + "') ";
                }
            }

            if (projectId.IsNullOrEmpty() && projectIds.IsNotNullAndNotEmpty())
            {
                //projectSerach = @$" and tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}'";
                projectSerach += projectSerach + @$" and s.""OwnerUserId""='{userId}'";
            }
            var ownerSerach = "";
            if (ownerIds.IsNotNullAndNotEmpty())
            {
                string oids = ownerIds.Replace(",", "','");
                //foreach (var i in ownerIds)
                //{
                //    oids += $"'{i}',";
                //}
                //oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""OwnerUserId"" in ('" + oids + "') ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var Assignee = "";
            if (userRole.IsNotNull() && userRole.Contains("PROJECT_USER"))
            {
                projectSerach += @$" and tt.""Code""='PROJECT_SUPER_SERVICE'";
                if (assigneeIds.IsNotNullAndNotEmpty())
                {
                    Assignee = @$" and t.""AssignedToUserId""='{userId}' ";
                }
                else
                {
                    Assignee = @$" and t.""AssignedToUserId""='{userId}' ";
                }
            }
            else if (assigneeIds.IsNotNullAndNotEmpty())
            {
                string aids = assigneeIds.Replace(",", "','");
                //foreach (var i in assigneeIds)
                //{
                //    aids += $"'{i}',";
                //}
                //aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    if (ownerIds.IsNotNullAndNotEmpty())
                    {
                        Assignee = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                    }
                    else
                    {
                        Assignee = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                    }

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
                    if ((ownerIds.IsNotNull()) || (assigneeIds.IsNotNull() && Assignee.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }

                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull()) || (assigneeIds.IsNotNull() && Assignee.IsNotNullAndNotEmpty()) || (tasksStatus.IsNotNull()))
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
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
            
            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectUserWorkloadGridViewData(string userId, List<UserHierarchyViewModel> users, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            
            //if (users.Count == 0)
            //{
            //    return new List<ProjectGanttTaskViewModel>();
            //}
            string userIds = null;
            if (users.IsNotNull() && users.Count > 0)
            {
                var Ids = users.Select(x => x.UserId).ToList();

                userIds = string.Join("','", Ids);
            }

            var query = @$"
                           WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"",u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                       ,s.""ServiceSubject"" as ""ProjectName"",s.""ParentServiceId"" as ""ServiceStage""
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode"",s.""ServiceNo"" as ""ProjectNo""
                        FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
					 		join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
					 where  s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'  #ProjectWhere#
                        union all
                        SELECT s.*, s.""Id"" as ""ServiceId"", s.""ParentServiceId"" as ""ParentId"", u.""Name"" as ""UserName"",u.""Name"" as ""OwnerName"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus""
                        ,ns.""ProjectName"",s.""ServiceSubject"" as ""ServiceStage"" 
                        ,s.""StartDate"" as ""ActualStartDate"",coalesce(s.""CompletedDate"",s.""RejectedDate"",s.""CanceledDate"",s.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode"",ns.""ServiceNo"" as ""ProjectNo""
                        FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                         join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                            join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                 where  s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' )
                 SELECT ""Id"",""ServiceSubject"" as Title,""StartDate"" as Start,""DueDate"" as End,""ParentId"",""UserName"",""OwnerName"",""ProjectName"",""ServiceStage"",""Priority"",""NtsStatus"",'Parent' as Level 
                        ,""ActualStartDate"",""ActualEndDate"",""StatusGroupCode"",""ProjectNo"",""ServiceNo"" as ""TaskNo""
                        from NtsService


                 union all

                 select t.""Id"", t.""TaskSubject"" as Title, t.""StartDate"" as Start, t.""DueDate"" as End, nt.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", ou.""Name"" as ""OwnerName"",nt.""ProjectName"",nt.""ServiceStage"", sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"",'Child' as Level
                    ,t.""ActualStartDate"" as ""ActualStartDate"",coalesce(t.""CompletedDate"",t.""RejectedDate"",t.""CanceledDate"",t.""ClosedDate"") as ""ActualEndDate"",ss.""GroupCode"" as ""StatusGroupCode"",nt.""ProjectNo"",t.""TaskNo"" as ""TaskNo""
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""
                        join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""  and sp.""IsDeleted""=false and sp.""CompanyId""='{_uc.CompanyId}' 
                        join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""    and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                        join public.""User"" as ou on t.""OwnerUserId""=ou.""Id""    and ou.""IsDeleted""=false and ou.""CompanyId""='{_uc.CompanyId}' 
	                   where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' and (true={_uc.IsSystemAdmin} or t.""AssignedToUserId"" in ('{userIds}'))  #OwnerWhere# #AssigneeWhere# #StatusWhere# #DateWhere#        
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
            //if(projectIds.IsNotNullAndNotEmpty())
            if (projectIds.IsNotNullAndNotEmpty())
            {
                string pids = projectIds.Replace(",","','");  
                //string pids = null;
                //foreach (var i in projectIds)
                //{
                //    pids += $"{i},";
                //}
                //pids = pids.Trim(',');

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
            else
            {
                projectSerach = @$" and tt.""Code""='PROJECT_SUPER_SERVICE'";
            }
            query = query.Replace("#ProjectWhere#", projectSerach);
            var ownerSerach = "";
            if (ownerIds.IsNotNullAndNotEmpty())
            {
                string oids = ownerIds.Replace(",", "','");
                //string oids = "";
                //foreach (var i in ownerIds)
                //{
                //    oids += $"{i},";
                //}
                //oids = oids.Trim(',');
                if (oids.IsNotNull())
                {
                    ownerSerach = @" and t.""OwnerUserId"" in ('" + oids + "') ";
                }
            }
            query = query.Replace("#OwnerWhere#", ownerSerach);
            var assigneeSerach = "";
            if (assigneeIds.IsNotNullAndNotEmpty())
            {
                string aids = "";
                aids = assigneeIds.Replace(",", "','");
                //foreach (var i in assigneeIds)
                //{
                //    aids += $"{i},";
                //}
                //aids = aids.Trim(',');
                if (aids.IsNotNull())
                {
                    if (ownerIds.IsNotNullAndNotEmpty())
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                    }
                    else
                    {
                        assigneeSerach = @" and t.""AssignedToUserId"" in ('" + aids + "') ";
                    }
                }
            }
            query = query.Replace("#AssigneeWhere#", assigneeSerach);
            var statusSerach = "";
            if (tasksStatus.IsNotNullAndNotEmpty())
            {
                string sids = tasksStatus.Replace(",", "','");
                //string sids = "";
                //foreach (var i in tasksStatus)
                //{
                //    sids += $"{i},";
                //}
                //sids = sids.Trim(',');
                if (sids.IsNotNull())
                {
                    if ((ownerIds.IsNotNullAndNotEmpty()) || (assigneeIds.IsNotNullAndNotEmpty()))
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                    else
                    {
                        statusSerach = @" and t.""TaskStatusId"" in ('" + sids + "') ";
                    }
                }
            }
            query = query.Replace("#StatusWhere#", statusSerach);
            var dateSerach = "";
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {
                if ((ownerIds.IsNotNull()) || (assigneeIds.IsNotNull()) || (tasksStatus.IsNotNull()))
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
            var queryData = await _queryRepo.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);

            return queryData;

        }
        public async Task<IList<IdNameViewModel>> GetProjectSharedList(string userId)
        {
            string query = $@"select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""NtsService"" as s
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                        where tt.""Code""='PROJECT_SUPER_SERVICE' and s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                        
                        union
                        select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""NtsServiceShared"" as ss
                        join public.""NtsService"" as s on s.""Id""=ss.""NtsServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                        where tt.""Code""='PROJECT_SUPER_SERVICE' and ss.""SharedWithUserId""='{userId}' and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 

                        union
                        select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""NtsTask"" as t 
                        join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                        where tt.""Code""='PROJECT_SUPER_SERVICE' and t.""AssignedToUserId""='{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}' 

                        union
                        select s.""Id"" as ""Id"",s.""ServiceSubject"" as ""Name""
                        from public.""Team"" as tm
                        join public.""NtsServiceShared"" as ss on ss.""SharedWithTeamId""=tm.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_uc.CompanyId}' 
                        join public.""NtsService"" as s on s.""Id""=ss.""NtsServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
                        where tt.""Code""='PROJECT_SUPER_SERVICE' and tm.""IsDeleted""=false and tm.""CompanyId""='{_uc.CompanyId}' 

            ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);

            return queryData;
        }

        public async Task<long?> GetInboxMenuItemByUsercount(string roleText, string userId)
        {
            var query = $@"  
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' 
						     and usp.""UserRoleId"" in ({roleText}) 
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""                       
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'    
                    )
                 SELECT count(""Id"") from Nts where Level='Child' ";
            var count = await _queryRepo.ExecuteScalar<long?>(query, null);
            return count;
        }

        public async Task<List<TreeViewViewModel>> GetInboxMenuItemByUser(string roleText, string userId)
        {
            var query = $@"Select distinct ur.""Id"" as UserRoleId,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
                , 'INBOX' as ParentId, 'PROJECTSTAGE' as Type,usp.""InboxStageName"" as id,
                true as hasChildren, ur.""Id"" as UserRoleId,true as children,ur.""Name"" ||' (' || nt.""Count""|| ')' as text , 'INBOX' as parent   
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' 
                left join(

                 WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""UserRoleId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' 
						     and usp.""UserRoleId"" in ({roleText}) 
					  where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""UserRoleId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""UserRoleId""  from NtsService


                 union all

                 select t.""Id"",'Child',nt.""UserRoleId"" as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}' and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'          
                    )
                 SELECT count(""Id"") as ""Count"",""UserRoleId"" from Nts where Level='Child' group by ""UserRoleId""

						
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_uc.CompanyId}' 
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
            var list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }

        public async Task<List<TreeViewViewModel>> GetProjectStageData2(string id, string userRoleId, string userId)
        {
            var query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{id}' as ParentId,                
                'PROJECT' as Type,'{userRoleId}' as UserRoleId,
                true as hasChildren,true as children,usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as text , '{id}' as parent   
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' 
                left join(

                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""TemplateCode""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' 
						     and usp.""UserRoleId"" = '{userRoleId}' 
					 where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""TemplateCode"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""TemplateCode""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""TemplateCode"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}'   and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'        
                    )

	          
                    SELECT count(""Id"") as ""Count"",""TemplateCode"" from Nts where Level='Child' group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_uc.CompanyId}' 
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
            var list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }

        public async Task<List<TreeViewViewModel>> GetProjectData2(string id, string userId)
        {
            var query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,
                false as hasChildren,s.""Id"" as ProjectId,
                '{id}' as ParentId,'STAGE' as Type,false as children,s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,  as text , '{id}' as parent
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'  
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}' 
						    
						     where tt.""Code""='{id}' and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
					 
                        union all
                        SELECT s.""Id"", ns.""ServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""ServiceId"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                       join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}' 
	                    where t.""AssignedToUserId"" = '{userId}'  and t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'         
                    )

	           
                    SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{id}' and  t.""Count""!=0 and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}' 
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


            var list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }

        public async Task<long?> GetInboxMenuItemcount(string roleText, string userId)
        {
            var query = $@"  
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}' 
						     and usp.""UserRoleId"" in ({roleText}) where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level from NtsService


                 union all

                 select t.""Id"",'Child' as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""                       
                        join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                             where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'
                    )
                 SELECT count(""Id"") from Nts where Level='Child' ";
            var count = await _queryRepo.ExecuteScalar<long?>(query, null);
            return count;
        }

        public async Task<List<TreeViewViewModel>> GetInboxMenuItem(string roleText, string userId)
        {
            var query = $@"Select distinct ur.""Id"" as UserRoleId,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
                , 'INBOX' as ParentId, 'PROJECTSTAGE' as Type,usp.""InboxStageName"" as id,
                true as hasChildren, ur.""Id"" as UserRoleId,true as children,ur.""Name"" ||' (' || nt.""Count""|| ')' as text , 'INBOX' as parent
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
                left join(

                 WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""UserRoleId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
						     and usp.""UserRoleId"" in ({roleText}) 
                       where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
					   
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""UserRoleId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""UserRoleId""  from NtsService


                 union all

                 select t.""Id"",'Child',nt.""UserRoleId"" as Level 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id"" 
 join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                     where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'        
                    )
                 SELECT count(""Id"") as ""Count"",""UserRoleId"" from Nts where Level='Child' group by ""UserRoleId""

						
					) nt on nt.""UserRoleId""=ur.""Id""
                where ur.""Id"" in ({roleText}) and ur.""IsDeleted""=false and ur.""CompanyId""='{_uc.CompanyId}'
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
            var list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }

        public async Task<List<TreeViewViewModel>> GetProjectStageData3(string id, string userRoleId, string userId)
        {
            var query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
                usp.""TemplateCode""  as id, '{id}' as ParentId,                
                'PROJECT' as Type,'{userRoleId}' as UserRoleId,
                true as hasChildren,true as children,usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')' as text , '{id}' as parent
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
                left join(

                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""TemplateCode""   FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' and usp.""IsDeleted""=false and usp.""CompanyId""='{_uc.CompanyId}'
						     and usp.""UserRoleId"" = '{userRoleId}' where (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""TemplateCode"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""TemplateCode""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""TemplateCode"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
 join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                      where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'       
                    )

	          
                    SELECT count(""Id"") as ""Count"",""TemplateCode"" from Nts where Level='Child' group by ""TemplateCode""
                 
                ) t on usp.""TemplateCode""=t.""TemplateCode""

                where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}' and ur.""IsDeleted""=false and ur.""CompanyId""='{_uc.CompanyId}'
                Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
            var list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }

        public async Task<List<TreeViewViewModel>> GetProjectData3(string id, string userId)
        {
            var query = $@"select  s.""Id"" as id,
                s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as Name,
                false as hasChildren,s.""Id"" as ProjectId,
                '{id}' as ParentId,'STAGE' as Type,false as children, s.""ServiceSubject"" ||' (' || t.""Count"" || ')' as text , '{id}' as parent
                FROM public.""NtsService"" as s
                join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
                 left join(
                WITH RECURSIVE Nts AS(

                 WITH RECURSIVE NtsService AS(
                 SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s
                         join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_uc.CompanyId}'
						    
						     where tt.""Code""='{id}' and (s.""OwnerUserId""='{userId}' or s.""RequestedByUserId""='{userId}') and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
					 
                        union all
                        SELECT s.""Id"", ns.""ServiceId"" FROM public.""NtsService"" as s
                        join NtsService ns on s.""ParentServiceId""=ns.""Id""

                     join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                        where s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                 )
                 SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService


                 union all

                 select t.""Id"",'Child' as Level,nt.""ServiceId"" 
                     FROM  public.""NtsTask"" as t
                        join Nts as nt on t.""ParentServiceId"" =nt.""Id"" 
 join public.""User"" as u on t.""AssignedToUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
	                           where t.""IsDeleted""=false and t.""CompanyId""='{_uc.CompanyId}'  
                    )

	           
                    SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""
                 
                ) t on s.""Id""=t.""ServiceId""

                Where tt.""Code""='{id}' and (s.""OwnerUserId"" = '{userId}' or s.""RequestedByUserId""='{userId}') and t.""Count""!=0 and s.""IsDeleted""=false and s.""CompanyId""='{_uc.CompanyId}'
                GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


            var list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            return list;
        }


    }
}

