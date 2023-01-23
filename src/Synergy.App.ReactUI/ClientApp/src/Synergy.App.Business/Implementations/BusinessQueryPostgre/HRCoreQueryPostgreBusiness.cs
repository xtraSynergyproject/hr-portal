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
    public class HRCoreQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IHRCoreQueryBusiness
    {

        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        public HRCoreQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
        }

        public async Task<OrganizationChartIndexViewModel> GetOrgHierarchyParentId(string personId)
        {
            string query = $@"select a.""DepartmentId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as u on p.""UserId""=u.""Id"" and u.""Id""='{_repo.UserContext.UserId}' and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            where a.""IsDeleted""=false and a.""CompanyId""='{_uc.CompanyId}'
                            ";

            if (personId.IsNotNullAndNotEmpty())
            {
                query = $@"select a.""DepartmentId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            where a.""IsDeleted""=false and a.""CompanyId""='{_uc.CompanyId}'
                            ";
            }

            var queryData = await _queryRepo.ExecuteQuerySingle<OrganizationChartIndexViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<OrganizationChartViewModel>> GetOrgHierarchy(string parentId, int levelUpto)
        {
            string query = $@"  select d.""Id"" as Id,d.""NtsNoteId"" as NoteId,h.""NtsNoteId"" as OrgHierarchyNoteId
                            ,d.""DepartmentName"" as OrganizationName ,c.""CostCenterName"" as CostCenter,h.""ParentDepartmentId"" as ParentId
                           ,coalesce(t.""Count"",0) as DirectChildCount,h.""HierarchyId"" as HierarchyId
                            from cms.""N_CoreHR_HRDepartment"" as d
                            join cms.""N_CoreHR_HRCostCenter"" as c on d.""CostCenterId"" = c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_uc.CompanyId}'

                            left join cms.""N_CoreHR_HRDepartmentHierarchy"" as h on d.""Id"" = h.""DepartmentId"" and h.""IsDeleted""=false and h.""CompanyId""='{_uc.CompanyId}'
                            left join(
                            WITH RECURSIVE List AS(

                             WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                where h.""IsDeleted""=false and h.""CompanyId""='{_uc.CompanyId}'
                        
                             )
                            select ""Id"",""ParentId"",Type from Department
								
                            )
                            SELECT Count(""Id"") as ""Count"",""ParentId"" from List where Type = 'Child' group by ""ParentId""
                            )
                            t on d.""Id"" = t.""ParentId""
                            left join(
                            WITH RECURSIVE List1 AS(

                             WITH RECURSIVE Department1 AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type,0 As level
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type,ns1.level+ 1
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'
                                join Department1 ns1 on h.""ParentDepartmentId"" = ns1.""Id""
                               where h.""IsDeleted""=false and h.""CompanyId""='{_uc.CompanyId}'
                             )
                            select ""Id"",""ParentId"",level from Department1
								
                            )
                            SELECT ""Id"",""ParentId"",level from List1 
                            )
                            t1 on d.""Id"" = t1.""Id""
                            where t1.level <={levelUpto} and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'
                            ";



            var queryData = await _queryRepo.ExecuteQueryList<OrganizationChartViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<PositionChartIndexViewModel> GetPostionHierarchyParentId(string personId)
        {
            string query = $@"select a.""PositionId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}'
                            join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                              where u.""Id""='{_repo.UserContext.UserId}' and a.""IsDeleted""=false and a.""CompanyId""='{_uc.CompanyId}'
                            ";
            if (personId.IsNotNullAndNotEmpty())
            {
                query = $@"select a.""PositionId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            where p.""Id""='{personId}' and a.""IsDeleted""=false and a.""CompanyId""='{_uc.CompanyId}'
                            ";
            }

            var queryData = await _queryRepo.ExecuteQuerySingle<PositionChartIndexViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<PositionChartViewModel>> GetPositionHierarchy(string parentId, int levelUpto)
        {
            string query = $@"  select distinct d.""Id"" as Id,d.""NtsNoteId"" as NoteId,d.""PositionName"" as PositionName ,c.""Id"" as JobId,p.""NtsNoteId"" as PersonNoteId,a.""NtsNoteId"" as AssignmentNoteId,c.""NtsNoteId"" as JobNoteId, c.""JobTitle"" as JobName, x.""Id"" as OrganizationId, x.""DepartmentName"" as OrganizationName
                            ,p.""Id"" as PersonId,h.""ParentPositionId"" as ParentId,coalesce(t.""Count"",0) as DirectChildCount,u.""PhotoId"" as PhotoId,u.""Name"" as DisplayName,case when p.""Id"" is not null then true else false end as HasUser
                            , u.""Id"" as UserId,h.""NtsNoteId"" as PosHierarchyNoteId,h.""HierarchyId"" as HierarchyId,
                            case when p.""Id"" is not null then 'org-node-1' else 'org-node-3' end as CssClass
                            from cms.""N_CoreHR_HRPosition"" as d
                            Left join cms.""N_CoreHR_HRJob"" as c on d.""JobId"" = c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_uc.CompanyId}'
                            Left join cms.""N_CoreHR_HRDepartment"" as x on d.""DepartmentId"" = x.""Id"" and x.""IsDeleted""=false and x.""CompanyId""='{_uc.CompanyId}'
                            left join cms.""N_CoreHR_HRAssignment"" as a on d.""Id""=a.""PositionId"" and a.""IsDeleted""=false and a.""CompanyId""='{_uc.CompanyId}'
                            left join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId"" = p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_uc.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_uc.CompanyId}'
                            left join cms.""N_CoreHR_PositionHierarchy"" as h on d.""Id"" = h.""PositionId"" and h.""IsDeleted""=false and h.""CompanyId""='{_uc.CompanyId}'
                            left join(
                            WITH RECURSIVE List AS(

                             WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRPosition"" as d
                                where d.""Id"" = '{parentId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentPositionId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_PositionHierarchy"" as h
                                join cms.""N_CoreHR_HRPosition"" as d on h.""PositionId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'
                                join Department ns on h.""ParentPositionId"" = ns.""Id""
                                where  h.""IsDeleted""=false and h.""CompanyId""='{_uc.CompanyId}'
                             )
                            select ""Id"",""ParentId"",Type from Department
								
                            )
                            SELECT Count(""Id"") as ""Count"",""ParentId"" from List where Type = 'Child' group by ""ParentId""
                            )
                            t on d.""Id"" = t.""ParentId""
                            left join(
                            WITH RECURSIVE List1 AS(

                             WITH RECURSIVE Department1 AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type,0 As level
                                from cms.""N_CoreHR_HRPosition"" as d
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentPositionId"" as ""ParentId"",'Child' as Type,ns1.level+ 1
                                from cms.""N_CoreHR_PositionHierarchy"" as h
                                join cms.""N_CoreHR_HRPosition"" as d on h.""PositionId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_uc.CompanyId}'
                                join Department1 ns1 on h.""ParentPositionId"" = ns1.""Id""
                               where  h.""IsDeleted""=false and h.""CompanyId""='{_uc.CompanyId}'
                             )
                            select ""Id"",""ParentId"",Type,level from Department1
								
                            )
                            SELECT ""Id"",""ParentId"",level from List1 
                            )
                            t1 on d.""Id"" = t1.""Id""
                            where t1.level <={levelUpto} and d.""CompanyId""='{_uc.CompanyId}'
                            ";



            var queryData = await _queryRepo.ExecuteQueryList<PositionChartViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<AssignmentViewModel>> GetEmployeeDirectoryData()
        {
            string query = $@"Select p.""PersonFullName"" as ""PersonFullName"",p.""PersonNo"" as ""PersonNo"",j.""JobTitle"" as ""JobName"" 
,d.""DepartmentName"" as ""DepartmentName"" ,u.""Email"" as ""Email"",u.""Mobile"" as ""Mobile""
from cms.""N_CoreHR_HRPerson"" as p
join cms.""N_CoreHR_HRAssignment"" as assi on p.""Id"" = assi.""EmployeeId"" and assi.""IsDeleted"" = false
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false
join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=assi.""DepartmentId"" and d.""IsDeleted""=false
join cms.""N_CoreHR_HRJob"" as j on j.""Id""=assi.""JobId"" and j.""IsDeleted""=false 
                            ";



            var queryData = await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<EmployeeTransferItemViewModel>> GeEmployeeFormTransferItemList(string employeeId, string transferId)
        {
            string query = $@"SELECT * FROM cms.""F_HR_EmployeeTransferRequestItems"" as i where i.""EmployeeId"" = '{employeeId}' and i.""RequestType"" = '{transferId}' ORDER BY ""Id"" ASC ";
            var queryData = await _queryRepo.ExecuteQueryList<EmployeeTransferItemViewModel>(query, null);
            return queryData;
        }
        public async Task<List<EmployeeTransferItemViewModel>> GetEmployeeTransferItemList(string employeeId)
        {
            var listOfItems = new List<EmployeeTransferItemViewModel>();
            string query = $@"select dsi.*,n.""Id"" as NoteId,i.""ItemName"" as ItemName,i.""Id"" as ItemId,dsi.""ReferenceHeaderItemId"" as ReferenceHeaderItemId,
                                dsi.""ReferenceHeaderId"" as ReferenceHeaderId,ri.""ItemQuantity"" as ItemQuantity,dsi.""IssuedQuantity"" as IssuedQuantity,
                                n.""NoteNo"" as NoteNo
                                ,u.""Name"" as UserName
                                from cms.""N_IMS_RequisitionIssueItem"" as dsi
                                join cms.""N_IMS_RequisitionItems"" as ri on ri.""Id""=dsi.""ReferenceHeaderItemId"" and ri.""IsDeleted""=false 
                                join public.""NtsNote"" as n on n.""Id""=dsi.""NtsNoteId"" and n.""IsDeleted""=false 
                                join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""Code""='NOTE_STATUS_INPROGRESS' 
                                join cms.""N_IMS_RequisitionIssue"" as ds on ds.""Id""=dsi.""RequisitionIssueId"" and ds.""IsDeleted""=false and ds.""IssueReferenceType""='0'
                                join cms.""N_IMS_IMS_ITEM_MASTER"" as i on i.""Id""=dsi.""ItemId"" and i.""IsDeleted""=false
                                left join public.""User"" as u on u.""Id""=ds.""IssueTo"" and u.""IsDeleted""=false
                                where u.""Id""='{employeeId}' and dsi.""IsDeleted""=false";
            var queryData = await _queryRepo.ExecuteQueryList<EmployeeTransferItemViewModel>(query, null);



            string query1 = $@"SELECT * FROM cms.""F_HR_EmployeeTransferRequestItems"" as i where i.""EmployeeId"" = '{employeeId}' ORDER BY ""Id"" ASC ";
            var queryData1 = await _queryRepo.ExecuteQueryList<EmployeeTransferItemViewModel>(query1, null);

            foreach (var i in queryData)
            {
                if (queryData1.Count > 0)
                {
                    // Loop for array2
                    foreach (var j in queryData1)
                    {
                        // Compare the element of each and
                        // every element from both of the
                        // arrays
                        if (i.ItemId == j.ItemId)
                        {
                            j.TransferItemId = j.Id;
                            // Return if common element found
                            bool containsItem = listOfItems.Any(item => item.ItemId == j.ItemId);
                            if (containsItem == false)
                            {
                                listOfItems.Add(j);
                            }
                        }
                        else
                        {
                            bool containsItem = listOfItems.Any(item => item.ItemId == i.ItemId);
                            if (containsItem == false)
                            {
                                listOfItems.Add(i);
                            }
                        }
                    }
                } else
                {
                    bool containsItem = listOfItems.Any(item => item.ItemId == i.ItemId);
                    if (containsItem == false)
                    {
                        listOfItems.Add(i);
                    }
                } 
            }
            var list = listOfItems;
            return list;
        }
    }
}
