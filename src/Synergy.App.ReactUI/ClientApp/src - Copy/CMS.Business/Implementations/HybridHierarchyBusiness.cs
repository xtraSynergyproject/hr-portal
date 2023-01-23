using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.IO;
using SpreadsheetLight;
using CMS.Common.Utilities;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CMS.Business
{
    public class HybridHierarchyBusiness : BusinessBase<HybridHierarchyViewModel, HybridHierarchy>, IHybridHierarchyBusiness
    {
        IRepositoryQueryBase<HybridHierarchyViewModel> _repoQuery;
        IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        IRepositoryQueryBase<UserHierarchyPermissionViewModel> _userHierarchyPermission;
        IUserContext _userContext;
        public HybridHierarchyBusiness(IRepositoryBase<HybridHierarchyViewModel, HybridHierarchy> repo, IMapper autoMapper
            , IRepositoryQueryBase<HybridHierarchyViewModel> repoQuery, IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<UserHierarchyPermissionViewModel> userHierarchyPermission, IUserContext userContext) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
            _queryRepo1 = queryRepo1;
            _userHierarchyPermission = userHierarchyPermission;
            _userContext = userContext;

        }
        public async override Task<CommandResult<HybridHierarchyViewModel>> Create(HybridHierarchyViewModel model)
        {
            var result = await base.Create(model);
            if (result.IsSuccess)
            {
                if (model.HierarchyPath == null || !model.HierarchyPath.Any())
                {
                    var hrchyPath = await GetHierarchyPath(result.Item.Id);
                    result.Item.HierarchyPath = hrchyPath.ToArray();
                    await UpdateHierarchyPath(result.Item);
                }
            }

            return CommandResult<HybridHierarchyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async override Task<CommandResult<HybridHierarchyViewModel>> Edit(HybridHierarchyViewModel model)
        {
            var result = await base.Edit(model);

            return CommandResult<HybridHierarchyViewModel>.Instance(model, result.IsSuccess, result.Messages);
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
            var result = await _repoQuery.ExecuteQueryList<TaskViewModel>(query, null);
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

            var result = await _repoQuery.ExecuteQueryList<TaskViewModel>(query, null);
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
            var list = await _repoQuery.ExecuteQueryList<BusinessHierarchyPermissionViewModel>(query, null);
            if (list.IsNotNull())
            {
                foreach (var item in list)
                {
                    if (item.UserId.IsNotNullAndNotEmpty())
                    {
                        var users = item.UserId.Trim('[', ']');
                        users = users.Replace("\"", "\'");
                        var query1 = $@" select string_agg(u.""Name""::text, ', ') as username
                                    from public.""User"" as u
                                    where u.""IsDeleted"" = false and u.""Id"" IN ({users})
                                    ";
                        var data = await _repoQuery.ExecuteScalar<string>(query1, null);
                        if (data.IsNotNullAndNotEmpty())
                        {
                            item.UserName = data;
                        }
                    }
                }
            }
            return list;
        }

        public async Task<bool> DeleteBusinessHierarchyPermission(BusinessHierarchyPermissionViewModel model)
        {
            var query = $@" update cms.""N_HR_BusinessHierarchyPermission""
                            set ""IsDeleted""=true
                            where ""Id""='{model.Id}' and ""NtsNoteId""='{model.NtsNoteId}' ";
            await _repoQuery.ExecuteCommand(query, null);
            return true;
        }
        public async Task<List<string>> GetHierarchyPath(string hierarchyItemId)
        {
            var query = $@"with recursive hrchy AS(
	         select ""Id"",""ParentId"" from public.""HybridHierarchy"" where ""IsDeleted""=false 
			and ""Id""='{hierarchyItemId}'
             union all
	         select h.""Id"",h.""ParentId"" from public.""HybridHierarchy""  as h
	         join hrchy on h.""Id""=hrchy.""ParentId"" where h.""IsDeleted""=false 
	         )select ""Id"",""ParentId"" from hrchy where ""ParentId"" is not null ";
            var list = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            var items = list.Select(x => x.ParentId).ToList();
            items.Reverse();
            return items;

        }
        public async Task<List<HybridHierarchyViewModel>> GetHierarchyParentDetails(string hierarchyItemId)
        {
            var query = $@"with recursive hrchy AS(
	         select * from public.""HybridHierarchy"" where ""IsDeleted""=false 
			and ""Id""='{hierarchyItemId}'
             union all
	         select h.* from public.""HybridHierarchy""  as h
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
			 ) r on coalesce(hrchy.""ReferenceId"",'-1')=r.""Id""";
            var list = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            list.Reverse();
            return list;

        }
        public async Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyChildList(string parentId, int level, int levelupto, bool enableAOR, string bulkRequestId, bool includeParent)
        {
            var userId = _repo.UserContext.UserId;
            var isAdmin = _repo.UserContext.IsSystemAdmin;

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
            var list = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            //if (enableAOR)
            //{
            //    // list = list.Where(x => x.HasResponibility).ToList();
            //}
            //var root = list.FirstOrDefault(x => x.Id == "-1");
            //if (root != null)
            //{
            //    var hrchyMaster = await _repo.GetSingle<HierarchyMasterViewModel, HierarchyMaster>(x => x.Code == "BUSINESS_HIERARCHY");
            //    //long dc = 0;
            //    //long ac = 0;
            //    //var child = list.Where(x => x.ParentId == "-1");
            //    //if (child.Any())
            //    //{
            //    //    dc = child.Count();
            //    //    ac = child.Sum(x => x.AllChildCount) + dc;
            //    //}

            //    if (hrchyMaster != null)
            //    {
            //        root.Name = hrchyMaster.Name;
            //    }

            //}
            return list;
        }

        public async Task RemoveFromBusinessHierarchy(string id)
        {
            await _repo.Delete(id);
        }

        public async Task<List<HybridHierarchyViewModel>> GetHourReportProjectData()
        {
            string query = "";
            //            string query = @$"select s.""servicesubject"" as projectname,s.""id"" as projectid,t.""tasksubject"" as taskname,t.""taskno"" as taskno,lov.""name"" as taskstatusname,
            //tu.""id"" as assigneeid,s.""startdate"" as taskstartdate,t.""tasksla"" as sla,t.""id""  as taskid,
            //s.""duedate"" as taskduedate,tu.""name"" as assigneename
            //                            from public.""ntstasktimeentry""
            //        };
            var queryData = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);
            return queryData;
        }
        public async Task<MemoryStream> DownloadHybridHierarchy()
        {

            var ms = new MemoryStream();
            using (var sl = new SLDocument())
            {
                sl.AddWorksheet("Business Hierarchy");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);


                sl.SetColumnWidth("A", 20);
                sl.SetColumnWidth("B", 20);
                sl.SetColumnWidth("C", 20);
                sl.SetColumnWidth("D", 20);
                sl.SetColumnWidth("E", 20);
                sl.SetColumnWidth("F", 20);
                sl.SetColumnWidth("G", 20);
                sl.SetColumnWidth("H", 20);
                sl.SetColumnWidth("I", 20);
                sl.SetColumnWidth("J", 20);
                sl.SetColumnWidth("K", 20);
                sl.SetColumnWidth("L", 20);
                sl.SetColumnWidth("M", 20);
                sl.SetColumnWidth("N", 20);
                sl.SetColumnWidth("O", 20);
                sl.SetColumnWidth("P", 20);
                sl.SetColumnWidth("R", 20);

                sl.MergeWorksheetCells("A1", "B1");
                sl.SetCellValue("A1", "");
                sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                sl.MergeWorksheetCells("I1", "J1");
                sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                sl.MergeWorksheetCells("A2", "H3");
                sl.SetCellValue("A2", "Business Hierarchy : " + DateTime.Today.ToDefaultDateFormat());
                sl.SetCellStyle("A2", "H3", ExcelHelper.GetReportHeadingStyle(sl));

                int row = 5;

                sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                sl.SetCellValue(string.Concat("A", row), "OrgLevel1");
                sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                sl.SetCellValue(string.Concat("B", row), "OrgLevel2");
                sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                sl.SetCellValue(string.Concat("C", row), "OrgLevel3");
                sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                sl.SetCellValue(string.Concat("D", row), "OrgLevel4");
                sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                sl.SetCellValue(string.Concat("E", row), "Brand");
                sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                sl.SetCellValue(string.Concat("F", row), "Market");
                sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                sl.SetCellValue(string.Concat("G", row), "Province");
                sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                sl.SetCellValue(string.Concat("H", row), "Department");
                sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                sl.SetCellValue(string.Concat("I", row), "CareerLevel");
                sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                sl.SetCellValue(string.Concat("J", row), "Job");
                sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                sl.SetCellValue(string.Concat("K", row), "Position");
                sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                sl.SetCellValue(string.Concat("L", row), "Employee");
                sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                var model = await PrepareHybridHierarchyExcel();

                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level1", OrgLevel2 = "Level1", OrgLevel3 = "Level1", OrgLevel4 = "Level1", Brand = "Level1", Market = "Level1", Province = "Level1", CareerLevel = "Level1", Department = "Level1", Job = "Level1", Employee = "Level1" });
                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level2", OrgLevel2 = "Level2", OrgLevel3 = "Level2", OrgLevel4 = "Level2", Brand = "Level2", Market = "Level2", Province = "Level2", CareerLevel = "Level2", Department = "Level1", Job = "Level1", Employee = "Level2" });
                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level3", OrgLevel2 = "Level3", OrgLevel3 = "Level3", OrgLevel4 = "Level3", Brand = "Level3", Market = "Level3", Province = "Level3", CareerLevel = "Level3", Department = "Level3", Job = "Level3", Employee = "Level3" });
                //model.Add(new BusinessHierarchyExcelViewModel { OrgLevel1 = "Level4", OrgLevel2 = "Level4", OrgLevel3 = "Level4", OrgLevel4 = "Level4", Brand = "Level4", Market = "Level4", Province = "Level4", CareerLevel = "Level4", Department = "Level4", Job = "Level4", Employee = "Level4" });

                //        var model = new List<BusinessHierarchyExcelViewModel>
                //{
                //    new  {OrgLevel1 = "Level1", OrgLevel2 = "Level1",OrgLevel3 ="Level1",OrgLevel4="Level1",Brand="Level1",Market="Level1",Province="Level1",CareerLevel="Level1",Department="Level1",Job="Level1",Employee="Level1"},
                //    new  {OrgLevel1 = "Level2", OrgLevel2 = "Level2",OrgLevel3 ="Level2",OrgLevel4="Level2",Brand="Level2",Market="Level2",Province="Level2",CareerLevel="Level2",Department="Level1",Job="Level1",Employee="Level2"},
                //    new  {OrgLevel1 = "Level3", OrgLevel2 = "Level3",OrgLevel3 ="Level3",OrgLevel4="Level3",Brand="Level3",Market="Level3",Province="Level3",CareerLevel="Level3",Department="Level3",Job="Level3",Employee="Level3"},
                //    new  {OrgLevel1 = "Level4", OrgLevel2 = "Level4",OrgLevel3 ="Level4",OrgLevel4="Level4",Brand="Level4",Market="Level4",Province="Level4",CareerLevel="Level4",Department="Level4",Job="Level4",Employee="Level4"}
                //};

                row++;
                //var projectList = await GetHourReportProjectData();
                foreach (var modelData in model)
                {
                    if (modelData.IsNotNull())
                    {
                        sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                        sl.SetCellValue(string.Concat("A", row), modelData.OrgLevel1.IsNotNull() ? modelData.OrgLevel1 : "");
                        sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                        sl.SetCellValue(string.Concat("B", row), modelData.OrgLevel2.IsNotNull() ? modelData.OrgLevel2 : "");
                        sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                        sl.SetCellValue(string.Concat("C", row), modelData.OrgLevel3.IsNotNull() ? modelData.OrgLevel3 : "");
                        sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                        sl.SetCellValue(string.Concat("D", row), modelData.OrgLevel4.IsNotNull() ? modelData.OrgLevel4 : "");
                        sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                        sl.SetCellValue(string.Concat("E", row), modelData.Brand.IsNotNull() ? modelData.Brand : "");
                        sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                        sl.SetCellValue(string.Concat("F", row), modelData.Market.IsNotNull() ? modelData.Market : "");
                        sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                        sl.SetCellValue(string.Concat("G", row), modelData.Province.IsNotNull() ? modelData.Province : "");
                        sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                        sl.SetCellValue(string.Concat("H", row), modelData.Department.IsNotNull() ? modelData.Department : "");
                        sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                        sl.SetCellValue(string.Concat("I", row), modelData.CareerLevel.IsNotNull() ? modelData.CareerLevel : "");
                        sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                        sl.SetCellValue(string.Concat("J", row), modelData.Job.IsNotNull() ? modelData.Job : "");
                        sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                        sl.SetCellValue(string.Concat("K", row), modelData.Position.IsNotNull() ? modelData.Position : "");
                        sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                        sl.SetCellValue(string.Concat("L", row), modelData.Employee.IsNotNull() ? modelData.Employee : "");
                        sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        row++;
                        //        //    string query = @$"SELECT ""Id"" as Id FROM public.""HybridHierarchy"" where ""IsDeleted""=false";
                        //        //var name = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);

                        //        //var list = name;
                        //        //return list;
                    }
                }
                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }


        public async Task<MemoryStream> DownloadAORdata(List<BusinessHierarchyAORViewModel> aorList)
        {
            List<string> usersList = aorList.Select(x => x.UserName).Distinct().ToList();
            List<string> boxList = aorList.Select(x => x.ReferenceName).Distinct().ToList();

            var ms = new MemoryStream();
            using (var sl = new SLDocument())
            {
                //box-row; users-columns ----- 2nd worksheet


                // first worksheet
                sl.SelectWorksheet("Sheet1");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);
                var cellStyle = sl.CreateStyle();
                cellStyle.Fill.SetPatternBackgroundColor(SLThemeColorIndexValues.Dark1Color);
                cellStyle.Fill.SetPatternType(PatternValues.Solid);

                var row = 1;
                var column = 1;
                foreach (var user in usersList)
                {
                    column = 1;
                    if (column == 1)
                    {

                        sl.SetCellValue(row + 1, column, user);
                        sl.AutoFitColumn(column);
                    }
                    foreach (var box in boxList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, box);
                        }

                        var exist = aorList.Exists(x => x.UserName == user && x.ReferenceName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }


                sl.AddWorksheet("Sheet2");
                // second worksheet
                sl.SelectWorksheet("Sheet2");

                row = 1;
                column = 1;
                foreach (var box in boxList)
                {
                    column = 1;
                    if (column == 1)
                    {
                        sl.SetCellValue(row + 1, column, box);
                    }
                    foreach (var user in usersList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, user);
                        }
                        var exist = aorList.Exists(x => x.UserName == user && x.ReferenceName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }
                sl.SelectWorksheet("Sheet1");
                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }
        public async Task<MemoryStream> DownloadBusinessPartnerMappingExcel(List<BusinessHierarchyAORViewModel> mappingList)
        {
            List<string> usersList = mappingList.Select(x => x.BusinessPartnerName).Distinct().ToList();
            List<string> boxList = mappingList.Select(x => x.DepartmentName).Distinct().ToList();

            var ms = new MemoryStream();
            using (var sl = new SLDocument())
            {
                //box-row; users-columns ----- 2nd worksheet


                // first worksheet
                sl.SelectWorksheet("Sheet1");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);
                var cellStyle = sl.CreateStyle();
                cellStyle.Fill.SetPatternBackgroundColor(SLThemeColorIndexValues.Dark1Color);
                cellStyle.Fill.SetPatternType(PatternValues.Solid);

                var row = 1;
                var column = 1;
                foreach (var user in usersList)
                {
                    column = 1;
                    if (column == 1)
                    {

                        sl.SetCellValue(row + 1, column, user);
                        sl.AutoFitColumn(column);
                    }
                    foreach (var box in boxList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, box);
                        }

                        var exist = mappingList.Exists(x => x.BusinessPartnerName == user && x.DepartmentName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }


                sl.AddWorksheet("Sheet2");
                // second worksheet
                sl.SelectWorksheet("Sheet2");

                row = 1;
                column = 1;
                foreach (var box in boxList)
                {
                    column = 1;
                    if (column == 1)
                    {
                        sl.SetCellValue(row + 1, column, box);
                    }
                    foreach (var user in usersList)
                    {
                        if (row == 1)
                        {
                            sl.SetCellValue(row, column + 1, user);
                        }
                        var exist = mappingList.Exists(x => x.BusinessPartnerName == user && x.DepartmentName == box);
                        if (exist)
                        {
                            sl.SetCellValue(row + 1, column + 1, "YES");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetApprovedStyle(sl));

                        }
                        else
                        {
                            sl.SetCellValue(row + 1, column + 1, "NO");
                            sl.SetCellStyle(row + 1, column + 1, ExcelHelper.GetRejectedStyle(sl));
                        }
                        column++;
                    }

                    row++;

                }
                sl.SelectWorksheet("Sheet1");
                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }
        private async Task<List<BusinessHierarchyExcelViewModel>> PrepareHybridHierarchyExcel()
        {
            var result = new List<BusinessHierarchyExcelViewModel>();
            var data = await GetBusinessHierarchyChildList("-1", 0, 1000, false, null, true);
            var parentIds = data.Select(x => x.ParentId).Distinct().ToList();
            var leafNodes = data.Where(x => !parentIds.Contains(x.Id)).OrderBy(x => x.SequenceOrder).ToList();
            foreach (var item in leafNodes)
            {
                AddItemsToHybridHierarchyExcel(item, data, result);
            }
            return result;
        }

        private void AddItemsToHybridHierarchyExcel(HybridHierarchyViewModel item, List<HybridHierarchyViewModel> data, List<BusinessHierarchyExcelViewModel> result)
        {
            var model = new BusinessHierarchyExcelViewModel();
            while (item != null)
            {
                switch (item.ReferenceType)
                {
                    case "LEVEL1":
                        model.OrgLevel1 = item.Name;
                        break;
                    case "LEVEL2":
                        model.OrgLevel2 = item.Name;
                        break;
                    case "LEVEL3":
                        model.OrgLevel3 = item.Name;
                        break;
                    case "LEVEL4":
                        model.OrgLevel4 = item.Name;
                        break;
                    case "BRAND":
                        model.Brand = item.Name;
                        break;
                    case "MARKET":
                        model.Market = item.Name;
                        break;
                    case "PROVINCE":
                        model.Province = item.Name;
                        break;
                    case "DEPARTMENT":
                        model.Department = item.Name;
                        break;
                    case "CAREER_LEVEL":
                        model.CareerLevel = item.Name;
                        break;
                    case "JOB":
                        model.Job = item.Name;
                        break;
                    case "POSITION":
                        model.Position = item.Name;
                        break;
                    case "EMPLOYEE":
                        model.Employee = item.Name;
                        break;
                    default:
                        break;
                }
                item = data.FirstOrDefault(x => x.Id == item.ParentId);
            }
            if (model.OrgLevel2.IsNullOrEmpty())
            {
                model.OrgLevel2 = model.OrgLevel1;
            }
            if (model.OrgLevel3.IsNullOrEmpty())
            {
                model.OrgLevel3 = model.OrgLevel2;
            }
            if (model.OrgLevel4.IsNullOrEmpty())
            {
                model.OrgLevel4 = model.OrgLevel3;
            }
            result.Add(model);
        }

        public async Task UpdateHierarchyPath(HybridHierarchyViewModel hybridmodel)
        {
            var values = string.Join("\", \"", hybridmodel.HierarchyPath);
            values = string.Concat("{\"", values, "\"}");
            var query = $@" update public.""HybridHierarchy""
                            set ""HierarchyPath""='{values}',
                            ""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',
                            ""LastUpdatedBy""='{_repo.UserContext.UserId}'
                            where ""Id""='{hybridmodel.Id}'";
            await _repoQuery.ExecuteCommand(query, null);
        }
        //public async Task<List<IdNameViewModel>> GetAllBusinessHierarchyList()
        //{
        //    var query = "";
        //    var name = new List<IdNameViewModel>();
        //    try
        //    {
        //        query = $@"select distinct
        //            hh.""Id"" as Id,
        //            case 
        //            when hr.""DepartmentName"" is not null then hr.""DepartmentName"" 
        //            when cl.""CareerLevel"" is not null then cl.""CareerLevel""
        //            when j.""JobTitle"" is not null then j.""JobTitle""
        //            when p.""PersonFullName"" is not null then p.""PersonFullName""
        //            end as Name,
        //            hh. ""ReferenceType"" as Code
        //            from public.""HybridHierarchy"" as hh 
        //            left join cms.""N_CoreHR_HRDepartment"" as hr
        //            on hh.""ReferenceId"" = hr.""Id"" and --hr.""DepartmentName"" is not null and 
        //            (hh.""ReferenceType"" = 'OrgLevel1' or hh.""ReferenceType"" = 'OrgLevel2' or hh.""ReferenceType"" = 'OrgLevel3' or hh.""ReferenceType"" = 'OrgLevel4'
        //            or hh.""ReferenceType"" = 'Brand' or hh.""ReferenceType"" = 'Market' or hh.""ReferenceType"" = 'Province' or  hh.""ReferenceType"" = 'Department')
        //            left join cms.""N_CoreHR_CareerLevel"" as cl
        //            on hh.""ReferenceId"" = cl.""Id"" and --cl.""CareerLevel"" is not null and
        //            (hh.""ReferenceType"" = 'CareerLevel')
        //            left join cms.""N_CoreHR_HRJob"" as j
        //            on hh.""ReferenceId"" = j.""Id"" and --cl.""CareerLevel"" is not null and
        //            (hh.""ReferenceType"" = 'Job')
        //            left join cms.""N_CoreHR_HRPerson"" as p
        //            on hh.""ReferenceId"" =p.""Id"" and --cl.""CareerLevel"" is not null and
        //            (hh.""ReferenceType"" = 'Employee')
        //           -- where hh.""PortalId"" = '{_userContext.PortalId}' and hh.""ReferenceId"" is not null 
        //        ";
        //        name = await _queryRepo1.ExecuteQueryList(query, null);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return name;
        //}
        //public async Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyDetails()
        //{
        //    var query = "";
        //    var name = new List<HybridHierarchyViewModel>();
        //    try
        //    {
        //        query = $@"select distinct
        //            hh.""Id"" as Id,
        //            case 
        //            when hr.""DepartmentName"" is not null then hr.""DepartmentName"" 
        //            when cl.""CareerLevel"" is not null then cl.""CareerLevel""
        //            when j.""JobTitle"" is not null then j.""JobTitle""
        //            when p.""PersonFullName"" is not null then p.""PersonFullName""
        //            end as ""Name"",
        //            hh. ""ReferenceType"" as ""ItemType""
        //            from public.""HybridHierarchy"" as hh 
        //            left join cms.""N_CoreHR_HRDepartment"" as hr
        //            on hh.""ReferenceId"" = hr.""Id"" and --hr.""DepartmentName"" is not null and 
        //            (hh.""ReferenceType"" = 'OrgLevel1' or hh.""ReferenceType"" = 'OrgLevel2' or hh.""ReferenceType"" = 'OrgLevel3' or hh.""ReferenceType"" = 'OrgLevel4'
        //            or hh.""ReferenceType"" = 'Brand' or hh.""ReferenceType"" = 'Market' or hh.""ReferenceType"" = 'Province' or  hh.""ReferenceType"" = 'Department')
        //            left join cms.""N_CoreHR_CareerLevel"" as cl
        //            on hh.""ReferenceId"" = cl.""Id"" and --cl.""CareerLevel"" is not null and
        //            (hh.""ReferenceType"" = 'CareerLevel')
        //            left join cms.""N_CoreHR_HRJob"" as j
        //            on hh.""ReferenceId"" = j.""Id"" and --cl.""CareerLevel"" is not null and
        //            (hh.""ReferenceType"" = 'Job')
        //            left join cms.""N_CoreHR_HRPerson"" as p
        //            on hh.""ReferenceId"" =p.""Id"" and --cl.""CareerLevel"" is not null and
        //            (hh.""ReferenceType"" = 'Employee')
        //            where hh.""ReferenceId"" is not null -- hh.""PortalId"" = '{_userContext.PortalId}' and 
        //        ";
        //        name = await _repoQuery.ExecuteQueryList(query, null);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return name;
        //}


        public async Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyList(string referenceType = null, string searckKey = null, bool bindPath = false)
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

            var list = await _repoQuery.ExecuteQueryList<HybridHierarchyViewModel>(query, null);

            return list;
        }
        public async Task<List<UserHierarchyPermissionViewModel>> GetBusinessHierarchyRootPermission(string PermissionId = null,string UserId = null)
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

            var list = await _userHierarchyPermission.ExecuteQueryList<UserHierarchyPermissionViewModel>(query, null);
            return list;
        }

        public async Task MoveItemToNewParent(string cureNodeId, string newParentId)
        {
            var hierarchy = await GetSingleById(cureNodeId);
            hierarchy.ParentId = newParentId;
            await Edit(hierarchy);
            var path = await GetHierarchyPath(cureNodeId);
            hierarchy.HierarchyPath = path.ToArray();

            await UpdateHierarchyPath(hierarchy);

            var childlist = await GetBusinessHierarchyChildList(cureNodeId, 0, 1000, false, "", false);

            foreach (var item in childlist)
            {
                path = await GetHierarchyPath(item.Id);
                item.HierarchyPath = path.ToArray();
                await UpdateHierarchyPath(item);
            }


        }

        public async Task<List<BusinessHierarchyAORViewModel>> GetAllAORBusinessHierarchyList()
        {
            var query = "";
            var list = new List<BusinessHierarchyAORViewModel>();
            try
            {
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
                list = await _repoQuery.ExecuteQueryList<BusinessHierarchyAORViewModel>(query, null);
            }
            catch (Exception)
            {

            }
            return list;
        }

    }
}
