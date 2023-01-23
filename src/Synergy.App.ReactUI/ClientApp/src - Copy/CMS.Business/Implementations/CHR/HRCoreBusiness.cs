
using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class HRCoreBusiness : BusinessBase<NoteViewModel, NtsNote>, IHRCoreBusiness
    {

        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<BusinessHierarchyAORViewModel> _queryRepoAOR;
        private readonly IRepositoryQueryBase<TeamViewModel> _queryTeamRepo1;
        private readonly IRepositoryQueryBase<ManpowerLeaveSummaryViewModel> _queryManpowerRepo1;
        private readonly IRepositoryQueryBase<TerminatePersonViewModel> _queryTerminatePersonRepo;
        private readonly IRepositoryQueryBase<OrganizationChartIndexViewModel> _queryRepo2;
        private readonly IRepositoryQueryBase<OrganizationChartViewModel> _queryRepo3;
        private readonly IRepositoryQueryBase<PositionChartIndexViewModel> _queryRepoposindex;
        private readonly IRepositoryQueryBase<PositionChartViewModel> _queryRepopos;
        private readonly IRepositoryQueryBase<beneficiaryViewModel> _queryBeneficiary;
        private readonly IRepositoryQueryBase<AssignmentViewModel> _queryAssignment;
        private readonly IRepositoryQueryBase<BusinessTripViewModel> _queryBusinessTrip;
        private readonly IRepositoryQueryBase<DependentViewModel> _queryDependent;
        private readonly IRepositoryQueryBase<MisconductViewModel> _queryMisconduct;
        private readonly IRepositoryQueryBase<JobDetViewModel> _JobDetViewModel;
        private readonly IRepositoryQueryBase<SalaryElementInfoViewModel> _querySalEleInfo;
        private readonly IRepositoryQueryBase<PersonProfileViewModel> _queryProfile;
        private readonly IRepositoryQueryBase<TimePermisssionViewModel> _queryTimePermission;
        private readonly IRepositoryQueryBase<TimeinTimeoutDetailsViewModel> _queryTimeinTimeOutDetail;
        private readonly IRepositoryQueryBase<JobDesriptionViewModel> _queryHRJobDescription;
        private readonly IRepositoryQueryBase<HRJobCriteriaViewModel> _queryHRJobCriteriaViewModel;
        private readonly IRepositoryQueryBase<PunchingViewModel> _queryPunching;
        private readonly IRepositoryQueryBase<AccessLogViewModel> _queryAccessLog;
        private readonly IRepositoryQueryBase<AttendanceViewModel> _queryAttendance;
        private readonly IRepositoryQueryBase<RemoteSignInSignOutViewModel> _queryRemote;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly IRepositoryQueryBase<TaskDetailsViewModel> _QueryTaskDetail;
        private readonly IRepositoryQueryBase<PostMessageViewModel> _queryPostMsg;
        private readonly IRepositoryQueryBase<DataUploadViewModel> _queryDataUpload;
        private readonly IRepositoryQueryBase<PersonViewModel> _queryPerson;
        private readonly IHybridHierarchyBusiness _hybridHierarchyBusiness;
        private readonly IRepositoryQueryBase<HybridHierarchyViewModel> _hybridHierarchy;
        private readonly IRepositoryQueryBase<UserHierarchyPermissionViewModel> _userHierarchyPermission;

        ICmsBusiness _cmsBusiness;
        IServiceProvider _sp;
        ITemplateCategoryBusiness _templateCategoryBusiness;
        ITemplateBusiness _templateBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        INoteBusiness _noteBusiness;
        ILegalEntityBusiness _legalEntityBusiness;
        IUserContext _userContext;
        IServiceBusiness _serviceBusiness;
        IHierarchyMasterBusiness _hierarchyMasterBusiness;
        IUserInfoBusiness _userInfoBusiness;
        //private readonly ILeaveBalanceSheetBusiness _leaveBalanceSheetBusiness;
        private readonly IAttendanceBusiness _attendanceBusiness;
        ILOVBusiness _lovBusiness;
        IUserBusiness _userBusiness;
        IFileBusiness _fileBusiness;



        public HRCoreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<AccessLogViewModel> queryAccessLog,
            IRepositoryQueryBase<OrganizationChartIndexViewModel> queryRepo2, IRepositoryQueryBase<OrganizationChartViewModel> queryRepo3,
            IRepositoryQueryBase<PositionChartIndexViewModel> queryRepoposindex, IRepositoryQueryBase<PositionChartViewModel> queryRepopos
            , IRepositoryQueryBase<beneficiaryViewModel> queryBeneficiary
            , ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness, ITableMetadataBusiness tableMetadataBusiness
            , IRepositoryQueryBase<AssignmentViewModel> queryAssignment
            , IRepositoryQueryBase<BusinessTripViewModel> BusinessTripViewModel, IRepositoryQueryBase<DependentViewModel> queryDependent
            , IRepositoryQueryBase<MisconductViewModel> queryMisconduct
            , IRepositoryQueryBase<TeamViewModel> queryTeamRepo1
            , IRepositoryQueryBase<PersonProfileViewModel> queryProfile
            , IRepositoryQueryBase<JobDetViewModel> queryjobDet, IUserContext userContext
            , IMapper autoMapper, INoteBusiness noteBusiness, IServiceBusiness ServiceBusiness,
        ILegalEntityBusiness legalEntityBusiness, IUserInfoBusiness userInfoBusiness,
            IRepositoryQueryBase<CalendarViewModel> calrepo,
            IRepositoryQueryBase<TimePermisssionViewModel> queryTimePermission,
            IHierarchyMasterBusiness hierarchyMasterBusiness,
            IRepositoryQueryBase<TimeinTimeoutDetailsViewModel> queryTimeinTimeOutDetail,
            IRepositoryQueryBase<JobDesriptionViewModel> queryHRJobDescription,
            IRepositoryQueryBase<HRJobCriteriaViewModel> queryHRJobCriteriaViewModel,
            IRepositoryQueryBase<PunchingViewModel> queryPunching,
            IRepositoryQueryBase<AttendanceViewModel> queryAttendance,
             IRepositoryQueryBase<RemoteSignInSignOutViewModel> queryRemote,
             IAttendanceBusiness attendanceBusiness, IRepositoryQueryBase<PersonViewModel> queryPerson,
              IRepositoryQueryBase<SalaryElementInfoViewModel> querySalEleInfo,
            IServiceProvider sp, ICmsBusiness cmsBusiness, IHybridHierarchyBusiness hybridHierarchyBusiness,
            IRepositoryQueryBase<ManpowerLeaveSummaryViewModel> queryManpowerRepo1
            , IRepositoryQueryBase<TerminatePersonViewModel> queryTerminatePersonRepo
            , IRepositoryQueryBase<TaskDetailsViewModel> QueryTaskDetail
            , IRepositoryQueryBase<DataUploadViewModel> queryDataUpload
            , IUserHierarchyBusiness userHierarchyBusiness, IRepositoryQueryBase<PostMessageViewModel> queryPostMsg, ILOVBusiness lovBusiness
            , IUserBusiness userBusiness, IFileBusiness fileBusiness,
            IRepositoryQueryBase<BusinessHierarchyAORViewModel> queryRepoAOR,
             IRepositoryQueryBase<HybridHierarchyViewModel> hybridHierarchy,
             IRepositoryQueryBase<UserHierarchyPermissionViewModel> userHierarchyPermission) : base(repo, autoMapper)


        {
            _queryRepoAOR = queryRepoAOR;
            _querySalEleInfo = querySalEleInfo;
            _queryRepo1 = queryRepo1;
            _queryRepo2 = queryRepo2;
            _queryRepo3 = queryRepo3;
            _queryRepoposindex = queryRepoposindex;
            _queryRepopos = queryRepopos;
            _queryAssignment = queryAssignment;
            _queryBeneficiary = queryBeneficiary;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _queryBusinessTrip = BusinessTripViewModel;
            _queryDependent = queryDependent;
            _queryMisconduct = queryMisconduct;
            _queryTeamRepo1 = queryTeamRepo1;
            _JobDetViewModel = queryjobDet;
            _noteBusiness = noteBusiness;
            _legalEntityBusiness = legalEntityBusiness;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _attendanceBusiness = attendanceBusiness;
            _userContext = userContext;
            _queryProfile = queryProfile;
            _serviceBusiness = ServiceBusiness;
            _queryTimePermission = queryTimePermission;
            _queryTimeinTimeOutDetail = queryTimeinTimeOutDetail;
            _queryHRJobDescription = queryHRJobDescription;
            _queryHRJobCriteriaViewModel = queryHRJobCriteriaViewModel;
            _queryPunching = queryPunching;
            _queryAccessLog = queryAccessLog;
            _queryAttendance = queryAttendance;
            _sp = sp;
            _queryRemote = queryRemote;
            _cmsBusiness = cmsBusiness;
            _userInfoBusiness = userInfoBusiness;
            _queryManpowerRepo1 = queryManpowerRepo1;
            _queryTerminatePersonRepo = queryTerminatePersonRepo;
            _userHierarchyBusiness = userHierarchyBusiness;
            _QueryTaskDetail = QueryTaskDetail;
            _queryPostMsg = queryPostMsg;
            _lovBusiness = lovBusiness;
            _userBusiness = userBusiness;
            _queryDataUpload = queryDataUpload;
            _fileBusiness = fileBusiness;
            _queryPerson = queryPerson;
            _hybridHierarchyBusiness = hybridHierarchyBusiness;
            _hybridHierarchy = hybridHierarchy;
            _userHierarchyPermission = userHierarchyPermission;

        }

        public async Task<OrganizationChartIndexViewModel> GetOrgHierarchyParentId(string personId)
        {
            string query = $@"select a.""DepartmentId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            join public.""User"" as u on p.""UserId""=u.""Id"" and u.""Id""='{_repo.UserContext.UserId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            where a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            ";

            if (personId.IsNotNullAndNotEmpty())
            {
                query = $@"select a.""DepartmentId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            where a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            ";
            }

            var queryData = await _queryRepo2.ExecuteQuerySingle(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<OrganizationChartViewModel>> GetOrgHierarchy(string parentId, int levelUpto)
        {
            string query = $@"  select d.""Id"" as Id,d.""NtsNoteId"" as NoteId,h.""NtsNoteId"" as OrgHierarchyNoteId
                            ,d.""DepartmentName"" as OrganizationName ,c.""CostCenterName"" as CostCenter,h.""ParentDepartmentId"" as ParentId
                           ,coalesce(t.""Count"",0) as DirectChildCount,h.""HierarchyId"" as HierarchyId
                            from cms.""N_CoreHR_HRDepartment"" as d
                            join cms.""N_CoreHR_HRCostCenter"" as c on d.""CostCenterId"" = c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'

                            left join cms.""N_CoreHR_HRDepartmentHierarchy"" as h on d.""Id"" = h.""DepartmentId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                            left join(
                            WITH RECURSIVE List AS(

                             WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                where h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                        
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
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type,ns1.level+ 1
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department1 ns1 on h.""ParentDepartmentId"" = ns1.""Id""
                               where h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select ""Id"",""ParentId"",level from Department1
								
                            )
                            SELECT ""Id"",""ParentId"",level from List1 
                            )
                            t1 on d.""Id"" = t1.""Id""
                            where t1.level <={levelUpto} and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                            ";



            var queryData = await _queryRepo3.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<PositionChartIndexViewModel> GetPostionHierarchyParentId(string personId)
        {
            string query = $@"select a.""PositionId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                              where u.""Id""='{_repo.UserContext.UserId}' and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            ";
            if (personId.IsNotNullAndNotEmpty())
            {
                query = $@"select a.""PositionId"" as Id 
                            from cms.""N_CoreHR_HRAssignment"" as a
                            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id""  and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            where p.""Id""='{personId}' and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            ";
            }

            var queryData = await _queryRepoposindex.ExecuteQuerySingle(query, null);
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
                            Left join cms.""N_CoreHR_HRJob"" as c on d.""JobId"" = c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'
                            Left join cms.""N_CoreHR_HRDepartment"" as x on d.""DepartmentId"" = x.""Id"" and x.""IsDeleted""=false and x.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRAssignment"" as a on d.""Id""=a.""PositionId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId"" = p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_PositionHierarchy"" as h on d.""Id"" = h.""PositionId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                            left join(
                            WITH RECURSIVE List AS(

                             WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRPosition"" as d
                                where d.""Id"" = '{parentId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentPositionId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_PositionHierarchy"" as h
                                join cms.""N_CoreHR_HRPosition"" as d on h.""PositionId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentPositionId"" = ns.""Id""
                                where  h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
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
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentPositionId"" as ""ParentId"",'Child' as Type,ns1.level+ 1
                                from cms.""N_CoreHR_PositionHierarchy"" as h
                                join cms.""N_CoreHR_HRPosition"" as d on h.""PositionId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department1 ns1 on h.""ParentPositionId"" = ns1.""Id""
                               where  h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select ""Id"",""ParentId"",Type,level from Department1
								
                            )
                            SELECT ""Id"",""ParentId"",level from List1 
                            )
                            t1 on d.""Id"" = t1.""Id""
                            where t1.level <={levelUpto} and d.""CompanyId""='{_userContext.CompanyId}'
                            ";



            var queryData = await _queryRepopos.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }


        public async Task<List<double>> GetPositionNodeLevel(string positiionId)
        {
            string query = $@"   WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRPosition"" as d
                                where d.""Id"" = '{positiionId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentPositionId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_PositionHierarchy"" as h
                                join cms.""N_CoreHR_HRPosition"" as d on h.""PositionId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentPositionId"" = ns.""Id""
                                where  h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select Count(""Id""),""ParentId"" from Department  where Type = 'Child' group by ""ParentId""
						
                            ";



            var queryData = await _queryRepopos.ExecuteScalarList<double>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<double>> GetDepartmentNodeLevel(string deptId)
        {
            string query = $@"   WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""Id"" = '{deptId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                where  h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select Count(""Id""),""ParentId"" from Department  where Type = 'Child' group by ""ParentId""
						
                            ";



            var queryData = await _queryRepopos.ExecuteScalarList<double>(query, null);
            var list = queryData;
            return list;
        }


        public async Task<IdNameViewModel> GetJobNameById(string Id)
        {
            var query = "";
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""JobTitle"" as Name FROM cms.""N_CoreHR_HRJob"" where ""Id""='{Id}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
                name = await _queryRepo1.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        public async Task<List<IdNameViewModel>> GetAllJobs()
        {
            var query = "";
            var name = new List<IdNameViewModel>();
            try
            {
                query = @$"SELECT ""JobTitle"" as Name,""Id"" as Id FROM cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
                name = await _queryRepo1.ExecuteQueryList(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

       
        public async Task<List<BusinessHierarchyAORViewModel>> GetAllAORBusinessHierarchyList()
        {
            var query = "";
            var list = new List<BusinessHierarchyAORViewModel>();
            try
            {
                query = $@"select distinct
                            hh.""Id"" as BusinessHierarchyId,
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
                list = await _queryRepoAOR.ExecuteQueryList(query, null);
            }
            catch (Exception)
            {

            }
            return list;
        }
        public async Task<List<BusinessHierarchyAORViewModel>> GetBusinessPartnerMappingList()
        {
            var query = "";
            var list = new List<BusinessHierarchyAORViewModel>();
            try
            {
                query = $@" select dpt.""BusinessPartnerId"" as BusinessPartnerId,team.""Name"" as BusinessPartnerName
                              ,dpt.""Id"" as DepartmentId,dpt.""DepartmentName"" as DepartmentName
                            from cms.""N_CoreHR_HRDepartment"" as dpt
                            left join public.""Team"" as team on team.""Id""=dpt.""BusinessPartnerId"" and team.""IsDeleted""=false
                            where dpt.""IsDeleted""=false and dpt.""BusinessPartnerId"" is not null ";
                list = await _queryRepoAOR.ExecuteQueryList(query, null);
            }
            catch (Exception)
            {

            }
            return list;
        }
        public async Task<List<IdNameViewModel>> GetAllOrganisation()
        {
            var query = "";
            var name = new List<IdNameViewModel>();
            try
            {
                query = @$"SELECT ""DepartmentName"" as Name,""Id"" as Id, ""DepartmentOwnerUserId"" FROM cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
                var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);
                //query = $@"{companyquery} select d.""Id"" as Id ,d.""DepartmentName"" as Name
                //            from cms.""N_CoreHR_HRDepartment"" as d
                //            join  ""Department"" as dept on dept.""DepartmentId""=d.""Id""
                //            where d.""IsDeleted""=false
                //            ";
                name = await _queryRepo1.ExecuteQueryList(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        public async Task<List<IdNameViewModel>> GetAllPosition()
        {
            var query = "";
            var name = new List<IdNameViewModel>();
            try
            {
                query = @$"SELECT ""PositionName"" as Name,""Id"" as Id FROM cms.""N_CoreHR_HRPosition"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
                name = await _queryRepo1.ExecuteQueryList(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        public async Task<IdNameViewModel> GetPositionByHierarchyAndPosition(string positionId, string hierarchyId)
        {
            var query = "";
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""NtsNoteId"" as Id FROM cms.""N_CoreHR_PositionHierarchy"" 
                   where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' and ""PositionId""='{positionId}' and ""HierarchyId""='{hierarchyId}' ";
                name = await _queryRepo1.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        public async Task<IdNameViewModel> GetUserByHierarchyAndUser(string userId, string hierarchyId)
        {
            var query = "";
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""NtsNoteId"" as Id FROM cms.""N_GENERAL_UserHierarchy"" 
                   where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' and ""UserId""='{userId}' and ""HierarchyId""='{hierarchyId}' ";
                name = await _queryRepo1.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        public async Task<IdNameViewModel> GetOrgByHierarchyAndOrg(string orgId, string hierarchyId)
        {
            var query = "";
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""NtsNoteId"" as Id FROM cms.""N_CoreHR_HRDepartmentHierarchy"" 
                   where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' and ""DepartmentId""='{orgId}' and ""HierarchyId""='{hierarchyId}' ";
                name = await _queryRepo1.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }
        public async Task<IdNameViewModel> GetParentOrgByOrg(string orgId)
        {
            var query = "";
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""ParentDepartmentId"" as Id FROM cms.""N_CoreHR_HRDepartmentHierarchy"" 
                   where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' and ""DepartmentId""='{orgId}' ";
                name = await _queryRepo1.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        //For Beneficiary Details

        public async Task<beneficiaryViewModel> GetBeneficiaryDEt(string Id)
        {
            var query = "";
            var name = new beneficiaryViewModel();
            try
            {
                query = @$"SELECT ""AccountNumber1"", ""Iban1"", ""SwiftCode1"", ""Branch1"" FROM cms.""N_CoreHR_HRBeneficiary"" where ""Id""='{Id}' and ""CompanyId""='{_userContext.CompanyId}'";
                name = await _queryBeneficiary.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }


        public async Task<IdNameViewModel> GetOrgNameById(string Id)
        {
            var query = "";
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""DepartmentName"" as Name FROM cms.""N_CoreHR_HRDepartment"" where ""Id""='{Id}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
                name = await _queryRepo1.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }
        public async Task<string> GenerateNextPositionName(string Name)
        {
            var id = 1;

            var poistionList = await GenerateNextPositionNo(Name);
            if (poistionList.Count > 0)
            {
                for (var i = 0; i < poistionList.Count; i++)
                {
                    var lastNo = poistionList[i].Name.Split('_')[^1];
                    //lastNo = lastNo.Replace("-", string.Empty);
                    var lastNo1 = Convert.ToInt32(lastNo);
                    if (id < lastNo1)
                    {
                        id = lastNo1;
                    }
                }
                return string.Concat(Name, ++id);
            }
            else
            {
                return string.Concat(Name, id);
            }
        }
        public async Task<IList<IdNameViewModel>> GenerateNextPositionNo(string Name)
        {
            string query = @$"SELECT ""Id"",""PositionName"" as Name FROM cms.""N_CoreHR_HRPosition""
                                where ""PositionName"" like '{Name}%' COLLATE ""tr-TR-x-icu"" and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetPositionHierarchyUsers(string parentId, int levelUpto)
        {
            string query = $@"  select distinct u.""Id"" as Id,u.""Name"" as Name 
                            from cms.""N_CoreHR_HRPosition"" as d
                            join cms.""N_CoreHR_HRJob"" as c on d.""JobId"" = c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_CoreHR_HRDepartment"" as x on d.""DepartmentId"" = x.""Id"" and x.""IsDeleted""=false and x.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRAssignment"" as a on d.""Id""=a.""PositionId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId"" = p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_PositionHierarchy"" as h on d.""Id"" = h.""PositionId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                            left join(
                            WITH RECURSIVE List AS(

                             WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRPosition"" as d
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentPositionId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_PositionHierarchy"" as h
                                join cms.""N_CoreHR_HRPosition"" as d on h.""PositionId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentPositionId"" = ns.""Id""
                            where h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
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
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentPositionId"" as ""ParentId"",'Child' as Type,ns1.level+ 1
                                from cms.""N_CoreHR_PositionHierarchy"" as h
                                join cms.""N_CoreHR_HRPosition"" as d on h.""PositionId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department1 ns1 on h.""ParentPositionId"" = ns1.""Id""
                          where h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select ""Id"",""ParentId"",Type,level from Department1
								
                            )
                            SELECT ""Id"",""ParentId"",level from List1 
                            )
                            t1 on d.""Id"" = t1.""Id""
                            where t1.level <={levelUpto} and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                            ";



            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<IdNameViewModel>> GetPersonList()
        {
            string query = $@"select p.""Id"" as Id ,CONCAT( p.""FirstName"",' ',p.""LastName"") as Name
                            from cms.""N_CoreHR_HRPerson"" as p    
                            where p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<PersonViewModel>> GetAllPersonList(string typeId)
        {
            string query = $@"select p.* ,l.""Code"" as Gender
                            from cms.""N_CoreHR_HRPerson"" as p
join public.""LOV"" as l on l.""Id""=p.""GenderId"" and l.""IsDeleted""=false  and l.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
where  p.""IsDeleted""=false  and p.""CompanyId""='{_userContext.CompanyId}'  #WHERE#
                            ";
            var where = "";
            if (typeId.IsNotNullAndNotEmpty())
            {
                where = $@"and a.""AssignmentTypeId""='{typeId}' ";
            }

            query = query.Replace("#WHERE#", where);
            var queryData = await _queryRepo1.ExecuteQueryList<PersonViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<IdNameViewModel>> GetAllActivePersonList()
        {
            string query = $@"select p.""Id"" as Id ,p.""PersonFullName"" as Name
                            from cms.""N_CoreHR_HRPerson"" as p
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
where  p.""IsDeleted""=false  and p.""CompanyId""='{_userContext.CompanyId}' 
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<PersonViewModel>> GetAllPersonAgeList(string typeId)
        {
            string query = $@"select p.* 
                            from cms.""N_CoreHR_HRPerson"" as p  
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false  and p.""CompanyId""='{_userContext.CompanyId}'  #WHERE#
                            ";
            var where = "";
            if (typeId.IsNotNullAndNotEmpty())
            {
                where = $@"and a.""AssignmentTypeId""='{typeId}' ";
            }

            query = query.Replace("#WHERE#", where);
            var queryData = await _queryRepo1.ExecuteQueryList<PersonViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<PersonViewModel>> GetAllPersonSalaryList(string typeId)
        {
            string query = $@"select p.""Id"" as PersonId,sum(e.""Amount""::Double Precision) as Salary
 from cms.""N_CoreHR_HRPerson"" as p 
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=p.""Id"" and si.""IsDeleted""=false  and si.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsNote"" as n on n.""Id""=si.""NtsNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsNote"" as cn on cn.""ParentNoteId""=si.""NtsNoteId"" and cn.""IsDeleted""=false  and cn.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_PayrollHR_SalaryElementInfo"" as e on e.""NtsNoteId""=cn.""Id"" and e.""IsDeleted""=false  and e.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false  and p.""CompanyId""='{_userContext.CompanyId}'  #WHERE#
group by p.""Id""          
                            ";
            var where = "";
            if (typeId.IsNotNullAndNotEmpty())
            {
                where = $@"and a.""AssignmentTypeId""='{typeId}' ";
            }

            query = query.Replace("#WHERE#", where);
            var queryData = await _queryRepo1.ExecuteQueryList<PersonViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<PersonViewModel>> GetAllPersonByCountryList(string typeId)
        {
            string query = $@"select p.* , n.""NationalityName"" as NationalityName
                            from cms.""N_CoreHR_HRPerson"" as p  
join cms.""N_CoreHR_HRNationality"" as n on n.""Id""=p.""NationalityId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id""  and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false  and p.""CompanyId""='{_userContext.CompanyId}' #WHERE#
                            ";
            var where = "";
            if (typeId.IsNotNullAndNotEmpty())
            {
                where = $@"and a.""AssignmentTypeId""='{typeId}' ";
            }

            query = query.Replace("#WHERE#", where);
            var queryData = await _queryRepo1.ExecuteQueryList<PersonViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<PersonViewModel>> GetAllPersonOnRoleBasisList(string typeId)
        {
            string query = $@"select p.* ,j.""JobTitle"" as IqamahJobTitle --r.""Name"" as UserRole
                            from cms.""N_CoreHR_HRPerson"" as p
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""  and j.""IsDeleted""=false and j.""CompanyId""='{_userContext.CompanyId}'
--join public.""User"" as u on u.""Id""=p.""UserId"" 
--join public.""UserRoleUser"" as ur on ur.""UserId""=u.""Id"" and ur.""IsDeleted""=false
--join public.""UserRole"" as r on r.""Id""=ur.""UserRoleId""
where p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'  #WHERE#                  
                            ";
            var where = "";
            if (typeId.IsNotNullAndNotEmpty())
            {
                where = $@"and a.""AssignmentTypeId""='{typeId}' ";
            }

            query = query.Replace("#WHERE#", where);

            var queryData = await _queryRepo1.ExecuteQueryList<PersonViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<IdNameViewModel>> GetPersonListByLegalEntity()
        {
            string query = $@"select p.""Id"" as Id ,CONCAT( p.""FirstName"",' ',p.""LastName"") as Name
                            from cms.""N_CoreHR_HRPerson"" as p 
                            where  p.""IsDeleted""=false and p.""PersonLegalEnityId""='{_userContext.LegalEntityId}' and p.""CompanyId""='{_userContext.CompanyId}'
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetUnAssignedPersonList(string personId)
        {
            string query = $@"select p.""Id"" as Id ,p.""PersonFullName"" as Name
                            from cms.""N_CoreHR_HRPerson"" as p                            
                            left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
                            where  (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and a.""Id"" is null) or p.""Id""='{personId}'
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetAssignedPersonList(string personId)
        {
            string query = $@"select p.""Id"" as Id ,p.""PersonFullName"" as Name
                            from cms.""N_CoreHR_HRPerson"" as p                            
                            join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
                            where  (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and a.""Id"" is not null) --or p.""Id""='{personId}'
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetJobByPerson(string jobId, string personId)
        {
            string query = $@"select j.""Id"" as Id ,j.""JobTitle"" as Name
                            from cms.""N_CoreHR_HRPerson"" as p                            
                            join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false  and j.""CompanyId""='{_userContext.CompanyId}'
                            where  (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and p.""Id""='{personId}') or j.""Id""='{jobId}'
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetDepartmentByPerson(string deptId, string personId)
        {
            string query = $@"select d.""Id"" as Id ,d.""DepartmentName"" as Name
                            from cms.""N_CoreHR_HRPerson"" as p                            
                            join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                            where  (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and p.""Id""='{personId}') or d.""Id""='{deptId}'
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetUnAssignedPositionList(string positionId, string departmentId, string jobId)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);

            string query = $@"{companyquery} select p.""Id"" as Id ,p.""PositionName"" as Name
                            from cms.""N_CoreHR_HRPosition"" as p 
                            join  ""Department"" as dept on dept.""DepartmentId"" = p.""DepartmentId"" 
                            left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""PositionId"" and a.""IsDeleted""=false  and a.""CompanyId""='{_userContext.CompanyId}'
                            where  (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and p.""JobId""='{jobId}' and p.""DepartmentId""='{departmentId}' and a.""Id"" is null) or p.""Id""='{positionId}'
                            ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetUnAssignedContractPersionList(string personId)
        {
            string query = $@"select p.""Id"" as Id ,p.""PersonFullName"" as Name
                            from cms.""N_CoreHR_HRPerson"" as p                            
                            left join cms.""N_CoreHR_HRContract"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            where (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and  a.""Id"" is null) or p.""Id""='{personId}'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetUnAssignedUserList(string userId)
        {
            string query = $@"select p.""Id"" as Id ,p.""Name"" as Name
                            from public.""User"" as p                            
                            left join cms.""N_CoreHR_HRPerson"" as a on p.""Id""=a.""UserId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            where (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and a.""Id"" is null) or p.""Id""='{userId}'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetUnAssignedChildPositionList(string parentId, string positionId)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);

            string query = $@"{companyquery} select p.""Id"" as Id ,p.""PositionName"" as Name
                            from cms.""N_CoreHR_HRPosition"" as p 
                            join  ""Department"" as dept on dept.""DepartmentId"" = p.""DepartmentId"" and dept.""IsDeleted""=false and dept.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_PositionHierarchy"" as a on p.""Id""=a.""PositionId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            where (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and a.""Id"" is null and p.""Id""!='{parentId}') or p.""Id""='{positionId}' ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetUnAssignedChildDepartmentList(string parentId, string departmentId)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);


            string query = $@"{companyquery} select p.""Id"" as Id ,p.""DepartmentName"" as Name
                            from cms.""N_CoreHR_HRDepartment"" as p
                            join  ""Department"" as dept on dept.""DepartmentId"" = p.""Id"" and dept.""IsDeleted""=false and dept.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRDepartmentHierarchy"" as a on p.""Id""=a.""DepartmentId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                            where (p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' and a.""Id"" is null and p.""Id""!='{parentId}') or p.""Id""='{departmentId}'  ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetDepartmentByType(string type, string level)
        {
            //var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);

            var typesearch = "";
            var levelsearch = "";

            string query = $@"select p.""Id"" as Id ,p.""DepartmentName"" as Name--, p.""Code"" as Code
                            from cms.""N_CoreHR_HRDepartment"" as p
                            where p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' #TYPESEARCH# #LEVELSEARCH#";

            //if (type.IsNotNullAndNotEmpty())
            //{

            //}
            if (type.IsNotNullAndNotEmpty())
            {
                var deptype = await _lovBusiness.GetSingle(x => x.LOVType == "DEPARTMENT_TYPE" && x.Code == type);
                if (deptype != null)
                {
                    typesearch = $@" and p.""DepartmentTypeId""='{deptype.Id}'";
                }
            }
            if (level.IsNotNullAndNotEmpty())
            {
                var deplevel = await _lovBusiness.GetSingle(x => x.LOVType == "DEPARTMENT_LEVEL" && x.Code == level);
                if (deplevel != null)
                {
                    levelsearch = $@" and p.""DepartmentLevelId""='{deplevel.Id}'";
                }
            }

            query = query.Replace("#TYPESEARCH#", typesearch);
            query = query.Replace("#LEVELSEARCH#", levelsearch);
            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }



        public async Task<List<UserListOfValue>> GetPersonDetailsForLOV(string searchParam, string includeId = null, bool includeIdOnly = false)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);


            string query = $@"{companyquery} select u.""Id"" as Id,u.""Id"" as UserId,hp.""Id"" as PersonId,hp.""SponsorshipNo"" as SponsorshipNo,
u.""Email"" as Email,hp.""BiometricId"" as BiometricId,
                hp.""MobileNumber"" as Mobile,hp.""PersonNo"" as PersonNo,hd.""DepartmentName"" as OrganizationName,
		hj.""JobTitle"" as JobName,hg.""GradeName"" as GradeName
                ,hp.""PhotoId"" as PhotoId,assi.""DateOfJoin"" as DateOfJoin
                ,cont.""AnnualLeaveEntitlement"" as AnnualLeaveEntitlement
                , hp.""PersonFullName"" as Name , hp.""PersonFullName"" as DisplayName
                            from cms.""N_CoreHR_HRDepartment"" as p
                            join ""Department"" as dept on dept.""DepartmentId"" = p.""Id"" and dept.""IsDeleted""=false and dept.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""DepartmentId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as hp on hp.""Id""=a.""EmployeeId"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false and hpos.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false and hg.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as cont on hp.""Id""=cont.""EmployeeId"" and cont.""IsDeleted""=false and cont.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
";


            var result = await _queryAssignment.ExecuteQueryList<UserListOfValue>(query, null);
            return result;

        }


        public async Task<List<AssignmentViewModel>> GetAssignmentDetails(string personId, string userId, string legalEntityId = null)
        {
            string query = $@"select CONCAT( hp.""FirstName"",' ',hp.""LastName"") as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
substring(assi.""DateOfJoin"",0,11) as DateOfJoin,lovs.""Name"" as AssignmentStatusName,case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,nt.""Id"" as NoteId,assi.""Id"" as AssignmentId,
cont.""Id"" as ContractId,ntc.""Id"" as NoteContractId,nta.""Id"" as NoteAssignmentId,hpos.""Id"" as PositionId,
ph.""Id"" as PositionHierarchyId,hm.""Id"" as HierarchyId,
ntp.""Id"" as NotePositionHierarchyId,
si.""Id"" as SalaryInfoId,ntsi.""Id"" as NoteSalaryInfoId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo,
hp.*
from cms.""N_CoreHR_HRPerson"" as hp
left join public.""NtsNote"" as nt on nt.""Id""=hp.""NtsNoteId"" and nt.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false
left join public.""NtsNote"" as nta on nta.""Id""=assi.""NtsNoteId"" and nta.""IsDeleted""=false
left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false
left join public.""HierarchyMaster"" as hm on hm.""Code""='POS_HIERARCHY' and hm.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false
left join public.""LOV"" as lovs on lovs.""Id""=assi.""AssignmentStatusId"" and lovs.""IsDeleted""=false
left join cms.""N_CoreHR_HRContract"" as cont on hp.""Id""=cont.""EmployeeId"" and cont.""IsDeleted""=false
left join public.""NtsNote"" as ntc on ntc.""Id""=cont.""NtsNoteId"" and ntc.""IsDeleted""=false
left join cms.""N_CoreHR_PositionHierarchy"" as ph on ph.""PositionId""=assi.""PositionId"" and ph.""IsDeleted""=false
left join public.""NtsNote"" as ntp on ntp.""Id""=ph.""NtsNoteId"" and ntp.""IsDeleted""=false
left join cms.""N_PayrollHR_SalaryInfo"" as si on hp.""Id""=si.""PersonId"" and si.""IsDeleted""=false
left join public.""NtsNote"" as ntsi on ntsi.""Id""=si.""NtsNoteId"" and ntsi.""IsDeleted""=false
where hp.""IsDeleted""=false #WHERE#
                            ";
            var where = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                where = $@"and u.""Id""='{userId}' ";
            }
            else if (personId.IsNotNullAndNotEmpty())
            {
                where = $@"and hp.""Id""='{personId}'";
            }
            //if (legalEntityId.IsNotNullAndNotEmpty())
            //{
            //    where=where + $@"and hp.""PersonLegalEntityId""='{legalEntityId}'";
            //}
            query = query.Replace("#WHERE#", where);
            var queryData = await _queryAssignment.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<IdNameViewModel> GetPersonDetailByUserId(string userId)
        {
            string query = @$"SELECT ""Id"" as Id,""PersonFullName"" as Name FROM cms.""N_CoreHR_HRPerson""
                                where ""UserId""='{userId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryRepo1.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetPersonListByOrgId(string orgId)
        {
            string query = @$"select p.""Id"" as Id,p.""PersonFullName"" as Name
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                                where  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}' #ORG#
                            ";
            var where = "";
            if (orgId.IsNotNullAndNotEmpty())
            {
                where = $@"and d.""Id"" in ({orgId}) ";
            }
            query = query.Replace("#ORG#", where);
            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<DateTime?> GetContractEndDateByUser(string userId)
        {
            //var match = string.Concat(@"match(u:ADM_User{ IsDeleted: 0,CompanyId: { CompanyId},Id:{UserId} })-[:R_User_PersonRoot]->(pr: HRS_PersonRoot{ IsDeleted: 0,CompanyId: { CompanyId} })
            //    match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot)
            //    match(cr)<-[:R_ContractRoot]-(c:HRS_Contract)
            //    where c.EffectiveStartDate <= { ESD} <= c.EffectiveEndDate
            //    return c.EffectiveEndDate as EffectiveEndDate");

            var query = $@"Select c.""EffectiveEndDate""::date From cms.""N_CoreHR_HRContract"" as c
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=c.""EmployeeId"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' 
                                where  c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'";

            var result = await _querySalEleInfo.ExecuteScalar<DateTime?>(query, null);

            return result;
        }
        public async Task<IdNameViewModel> GetCompanyOrganization(string userId)
        {
            string query = @$"SELECT d.""Id"" as Id,d.""DepartmentName"" as Name
                                from public.""User"" as u
                                join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                                join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
                                join cms.""N_CoreHR_HRDepartment"" as d on a.""DepartmentId""=d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                where u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryRepo1.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<BannerViewModel>> GetSliderBannerData()
        {
            var query = $@" select sb.""SliderImageId"" as BannerImageId, sb.""SliderContent"" as BannerContent 
                        from cms.""N_CoreHR_SliderBanner"" as sb
                        where sb.""IsDeleted""=false ";
            var list = await _queryRepo1.ExecuteQueryList<BannerViewModel>(query, null);
            return list.ToList();
        }

        public async Task<AssignmentViewModel> GetPersonDetail(string userId)
        {
            string query = @$"SELECT ""Id"" as PersonId,""PersonFullName"" as PersonFullName FROM cms.""N_CoreHR_HRPerson""
                                where ""UserId""='{userId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryAssignment.ExecuteQuerySingle<AssignmentViewModel>(query, null);
            return result;
        }

        public async Task<List<AssignmentViewModel>> CheckPersonWithUserId(string userId)
        {
            string query = @$"SELECT p.""Id"" as PersonId,p.""PersonFullName"" as PersonFullName FROM cms.""N_CoreHR_HRPerson"" as p
                              inner join public.""NtsNote"" as s1 on s1.""Id""=p.""NtsNoteId"" and s1.""IsDeleted""=false and s1.""CompanyId""='{_userContext.CompanyId}'
                                where p.""UserId""='{userId}' and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryAssignment.ExecuteQueryList<AssignmentViewModel>(query, null);
            return result;
        }

        public async Task<List<TeamViewModel>> GetUserTeamDetail(string userId)
        {
            string query = @$"select t.*,u.""Id"",u.""Name"" as UserName,tu.""IsTeamOwner"" as IsTeamOwner
from public.""TeamUser"" as tu
join public.""Team"" as t on tu.""TeamId""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
join public.""User"" as u on tu.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                                where tu.""UserId""='{userId}' and tu.""IsDeleted""=false and tu.""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryTeamRepo1.ExecuteQueryList<TeamViewModel>(query, null);
            return result;
        }

        public async Task<PersonViewModel> GetPersonDetailsById(string personId)
        {
            string query = @$"SELECT * FROM cms.""N_CoreHR_HRPerson""
                                where ""Id""='{personId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";

            var result = await _queryAssignment.ExecuteQuerySingle<PersonViewModel>(query, null);
            return result;
        }

        public async Task<PersonViewModel> GetPersonDetailsByPersonId(string personId)
        {
            string query = @$"SELECT p.*,p.""ContactPersonalEmail"" as PersonalEmail,u.""LineManagerId"" FROM cms.""N_CoreHR_HRPerson"" as p
left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false
                                where p.""Id""='{personId}' and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' ";

            var result = await _queryAssignment.ExecuteQuerySingle<PersonViewModel>(query, null);
            return result;
        }
        public async Task<AssignmentViewModel> GetLeaveBalance(string userId)
        {
            string query = @$"SELECT ""ClosingBalance"" as ClosingBalance FROM cms.""N_TAA_LeaveBalanceSheet""
                                where ""UserId""='{userId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryAssignment.ExecuteQuerySingle<AssignmentViewModel>(query, null);
            return result;
        }
        public async Task<AssignmentViewModel> GetContractDetail(string personId)
        {
            string query = @$"SELECT ""AnnualLeaveEntitlement"" as AnnualLeaveEntitlement FROM cms.""N_CoreHR_HRContract""
                              where ""EmployeeId""='{personId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'
                            ";
            var result = await _queryAssignment.ExecuteQuerySingle<AssignmentViewModel>(query, null);
            return result;
        }
        public async Task<IList<LeaveDetailViewModel>> GetLeaveDetail(string userId)
        {
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);


                if (item.Code == "UndertimeLeave" || item.Code == "LeaveAdjustment" || item.Code == "AnnualLeaveEncashment"
                    || item.Code == "LeaveHandoverService" || item.Code == "LEAVE_CANCEL" || item.Code == "RETURN_TO_WORK" || item.Code == "LeaveAccrual")
                {

                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }

                    selectQry = @$" {selectQry} SELECT ""Template"".""DisplayName"" as LeaveType,""Template"".""Code"" as TemplateCode,""NtsService"".""Id"" as NtsNoteId,""ServiceStatus"".""Name"" as LeaveStatus,""T"".""LeaveDurationCalendarDays"" as DurationText,""T"".""LeaveDurationWorkingDays"" as WorkingDuration,Le.""ServiceNo"" as CancelServiceNo,Le.""Id"" as CancelServiceId
,RW.""ServiceNo"" as ReturnServiceNo,RW.""Id"" as ReturnToWorkServiceId,RW.""ReturnDate"" as ReturnToWork,HO.""ServiceNo"" as HandoverServiceNo,HO.""Id"" as HandoverServiceId
,""T"".""LeaveStartDate"" as StartDate,""T"".""LeaveEndDate"" as EndDate
,""T"".""TelephoneNumber"" as TelephoneNumber,""T"".""AddressDetail"" as AddressDetail,""T"".""OtherInformation"" as OtherInformation
,""NtsService"".""ServiceNo"",""NtsService"".""Id"" as ""ServiceId"",""NtsService"".""ServiceSubject"",""NtsService"".""ServiceDescription"", ""NtsService"".""StartDate"" as AppliedDate FROM cms.""{tableMeta.Name}"" as ""T""
                                --join ""public"".""TableMetadata"" as ""TM"" on ""T"".""UdfTableMetadataId""=""TM"".""Id""
                               join public.""NtsNote"" as ""UdfNote"" on ""T"".""NtsNoteId""=""UdfNote"".""Id"" and ""UdfNote"".""IsDeleted""=false and ""UdfNote"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""NtsService"" as ""NtsService"" on ""UdfNote"".""Id""=""NtsService"".""UdfNoteId"" and ""NtsService"".""IsDeleted""=false and ""NtsService"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""Template"" as ""Template"" on ""NtsService"".""TemplateId""=""Template"".""Id"" and ""Template"".""IsDeleted""=false and ""Template"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""Template"" as ""UdfTemplate"" on ""NtsService"".""UdfTemplateId""=""UdfTemplate"".""Id"" and ""UdfTemplate"".""IsDeleted""=false and ""UdfTemplate"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ""OwnerUser"" on ""NtsService"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false and ""OwnerUser"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ""RequestedByUser"" on ""NtsService"".""RequestedByUserId""=""RequestedByUser"".""Id"" and ""RequestedByUser"".""IsDeleted""=false and ""RequestedByUser"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ""CreatedByUser"" on ""NtsService"".""CreatedBy""=""CreatedByUser"".""Id"" and ""CreatedByUser"".""IsDeleted""=false and ""CreatedByUser"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""User"" as ""UpdatedByUser"" on ""NtsService"".""LastUpdatedBy""=""UpdatedByUser"".""Id"" and ""UpdatedByUser"".""IsDeleted""=false and ""UdfNote"".""LastUpdatedBy""='{_userContext.CompanyId}'
            left join public.""ServiceTemplate"" as ""ServiceTemplate"" on ""ServiceTemplate"".""TemplateId""=""Template"".""Id"" and ""ServiceTemplate"".""IsDeleted""=false   and ""ServiceTemplate"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""LOV"" as ""ServiceStatus"" on ""NtsService"".""ServiceStatusId""=""ServiceStatus"".""Id"" and ""ServiceStatus"".""IsDeleted""=false and ""ServiceStatus"".""CompanyId""='{_userContext.CompanyId}'
            left join public.""LOV"" as ""ServiceAction"" on ""NtsService"".""ServiceActionId""=""ServiceAction"".""Id"" and ""ServiceAction"".""IsDeleted""=false and ""ServiceAction"".""CompanyId""='{_userContext.CompanyId}'
		left join (select ""NtsService"".""Id"", ""NtsService"".""ParentServiceId"",""NtsService"".""ServiceNo"" from cms.""N_Leave_LeaveCancel"" as  ""T"" 

    join public.""NtsNote"" as ""UdfNote"" on ""T"".""NtsNoteId""=""UdfNote"".""Id""
	and ""UdfNote"".""IsDeleted""=false and ""UdfNote"".""CompanyId""='{_userContext.CompanyId}'
    left join public.""NtsService"" as ""NtsService""
	on(""UdfNote"".""Id""=""NtsService"".""UdfNoteId"" 
	and ""NtsService"".""IsDeleted""=false and ""NtsService"".""CompanyId""='{_userContext.CompanyId}')

    left join public.""User"" as ""OwnerUser"" on ""NtsService"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false and ""OwnerUser"".""CompanyId""='{_userContext.CompanyId}'
	where ""OwnerUser"".""Id""='{userId}') as Le on Le.""ParentServiceId""=""NtsService"".""Id""
 left join  (select ""NtsService"".""Id"", ""NtsService"".""ParentServiceId"",""NtsService"".""ServiceNo"",""T"".""DateOfReturnFromLeave""::TIMESTAMP::DATE as ""ReturnDate"" from cms.""N_Leave_ReturnToWork"" as  ""T"" 

    join public.""NtsNote"" as ""UdfNote"" on ""T"".""NtsNoteId""=""UdfNote"".""Id""
	and ""UdfNote"".""IsDeleted""=false and ""UdfNote"".""CompanyId""='{_userContext.CompanyId}'
    left join public.""NtsService"" as ""NtsService""
	on(""UdfNote"".""Id""=""NtsService"".""UdfNoteId"" 
	and ""NtsService"".""IsDeleted""=false and ""NtsService"".""CompanyId""='{_userContext.CompanyId}')

    left join public.""User"" as ""OwnerUser"" on ""NtsService"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false and ""OwnerUser"".""CompanyId""='{_userContext.CompanyId}'
		where ""OwnerUser"".""Id""='{userId}') as RW on RW.""ParentServiceId""=""NtsService"".""Id""
 left join  (select ""NtsService"".""Id"", ""NtsService"".""ParentServiceId"",""NtsService"".""ServiceNo"" from cms.""N_Leave_Leave-Handover-Service"" as  ""T"" 

    join public.""NtsNote"" as ""UdfNote"" on ""T"".""NtsNoteId""=""UdfNote"".""Id""
	and ""UdfNote"".""IsDeleted""=false and ""UdfNote"".""CompanyId""='{_userContext.CompanyId}'
    left join public.""NtsService"" as ""NtsService""
	on(""UdfNote"".""Id""=""NtsService"".""UdfNoteId"" 
	and ""NtsService"".""IsDeleted""=false and ""NtsService"".""CompanyId""='{_userContext.CompanyId}')

    left join public.""User"" as ""OwnerUser"" on ""NtsService"".""OwnerUserId""=""OwnerUser"".""Id"" and ""OwnerUser"".""IsDeleted""=false and ""OwnerUser"".""CompanyId""='{_userContext.CompanyId}'
	where ""OwnerUser"".""Id""='{userId}') as HO on HO.""ParentServiceId""=""NtsService"".""Id""
                     where ""OwnerUser"".""Id""='{userId}' and ""T"".""IsDeleted""=false and ""T"".""CompanyId""='{_userContext.CompanyId}' ";

                    i++;
                }

            }
            var result = await _queryRepo1.ExecuteQueryList<LeaveDetailViewModel>(selectQry, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetAllLocation()
        {
            var query = "";
            var name = new List<IdNameViewModel>();
            try
            {
                query = @$"SELECT ""LocationName"" as Name,""Id"" as Id FROM cms.""N_CoreHR_HRLocation"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
                name = await _queryRepo1.ExecuteQueryList(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }
        public async Task<string> GetIdDialCodeById(string Id)
        {
            var query = $@"Select ""DialCode"" as ContactCountryDialCode from cms.""N_CoreHR_HRCountry"" where ""Id""='{Id}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
            var result = await _queryProfile.ExecuteScalar<string>(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetCountryList()
        {
            var query = $@"Select ""Id"" as Id,""CountryName"" as Name  from cms.""N_CoreHR_HRCountry"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetNationalityList()
        {
            var query = $@"Select ""Id"" as Id,""NationalityName"" as Name  from cms.""N_CoreHR_HRNationality"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetGradeList()
        {
            var query = $@"Select ""Id"" as Id,""GradeName"" as Name  from cms.""N_CoreHR_HRGrade"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetHRMasterList(string templateCode)
        {
            var list = new List<IdNameViewModel>();
            if (templateCode == "HRPerson")
            {
                var query = $@"Select ""Id"" as Id,""FirstName"" as Name  from cms.""N_CoreHR_HRPerson"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRCountry")
            {
                var query = $@"Select ""Id"" as Id,""CountryName"" as Name  from cms.""N_CoreHR_HRCountry"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRCostCenter")
            {
                var query = $@"Select ""Id"" as Id,""CostCenterName"" as Name  from cms.""N_CoreHR_HRCostCenter"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRResponsibilityCenter")
            {
                var query = $@"Select ""Id"" as Id,""ResponsibilityCenterName"" as Name  from cms.""N_CoreHR_HRResponsibilityCenter"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRLocation")
            {
                var query = $@"Select ""Id"" as Id,""LocationName"" as Name  from cms.""N_CoreHR_HRLocation"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRNationality")
            {
                var query = $@"Select ""Id"" as Id,""NationalityName"" as Name  from cms.""N_CoreHRN_CoreHR_HRNationality_HRCountry"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRJob")
            {
                var query = $@"Select ""Id"" as Id,""JobTitle"" as Name  from cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRGrade")
            {
                var query = $@"Select ""Id"" as Id,""GradeName"" as Name  from cms.""N_CoreHR_HRGrade"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "HRDepartment")
            {
                var query = $@"Select ""Id"" as Id,""DepartmentName"" as Name  from cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            if (templateCode == "Career Level")
            {
                var query = $@"Select ""Id"" as Id,""CareerLevel"" as Name  from cms.""N_CoreHR_CareerLevel"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
                list = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            }

            return list;
        }
        public async Task<AccessLogViewModel> UpdateAccessLogDetail(string UserId, DateTime punchTime, PunchingTypeEnum punchingType, DeviceTypeEnum deviceType, string locationId)
        {
            var noteTemplate = new NoteTemplateViewModel();
            string userInfoId = null;

            var queryPerson = $@"Select * from cms.""N_CoreHR_HRPerson"" where ""UserId""='{UserId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
            var persondata = await _queryAccessLog.ExecuteQuerySingle(queryPerson, null);

            var queryDevice = $@"Select ""Id"" as DeviceId, ""Name"" as DeviceName, ""MachineNo"" as DeviceMachineNo, ""ipAddress"" as DeviceIpAddress,
                                    ""PortNo"" as DevicePortNo, ""DeviceSerialNo"" from cms.""N_CLK_Device"" where ""DeviceType""='{deviceType}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
            var devicedata = await _queryAccessLog.ExecuteQuerySingle(queryDevice, null);

            var userInfoModel = await _userInfoBusiness.GetUserInfoDetails(persondata.BiometricId);
            if (userInfoModel == null)
            {
                var userInfo = new UserInfoViewModel
                {
                    PersonId = persondata.Id,
                    DeviceId = devicedata.Id,
                    BiometricId = persondata.BiometricId,
                    Name = persondata.FirstName,
                    Enabled = true,
                    SponsorshipNo = persondata.SponsorshipNo
                };

                noteTemplate.ActiveUserId = _userContext.UserId;
                noteTemplate.TemplateCode = "USERINFO";
                noteTemplate.DataAction = DataActionEnum.Create;
                var usernote = await _noteBusiness.GetNoteDetails(noteTemplate);
                usernote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                usernote.Json = JsonConvert.SerializeObject(userInfo);

                var uIResult = await _noteBusiness.ManageNote(usernote);

                if (uIResult.IsSuccess)
                {
                    userInfoId = uIResult.Item.Id;
                }
            }
            else
            {
                userInfoId = userInfoModel.Id;
            }

            var accessmodel = new AccessLogViewModel()
            {
                UserId = persondata.UserId,
                PersonId = persondata.Id,
                FirstName = persondata.FirstName,
                MiddleName = persondata.MiddleName,
                LastName = persondata.LastName,
                PersonFullName = persondata.PersonFullName,
                SponsorshipNo = persondata.SponsorshipNo,
                PunchingTime = punchTime,
                DevicePunchingType = punchingType,
                SignInLocation = locationId,
                UserInfoId = userInfoId,
                DeviceId = devicedata.DeviceId,
                BiometricId = persondata.BiometricId,
                DeviceName = devicedata.DeviceName,
                DeviceMachineNo = devicedata.DeviceMachineNo,
                DeviceIpAddress = devicedata.DeviceIpAddress,
                DevicePortNo = devicedata.DevicePortNo,
                DeviceSerialNo = devicedata.DeviceSerialNo,
                AccessLogSource = "Service"
            };

            noteTemplate.ActiveUserId = _userContext.UserId;
            noteTemplate.TemplateCode = "ACCESS_LOG";
            noteTemplate.DataAction = DataActionEnum.Create;
            var note = await _noteBusiness.GetNoteDetails(noteTemplate);
            note.RequestedByUserId = note.OwnerUserId = UserId;
            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

            note.Json = JsonConvert.SerializeObject(accessmodel);

            var result = await _noteBusiness.ManageNote(note);

            return null;
        }

        public async Task<AccessLogViewModel> GetAccessLogData(string userId, PunchingTypeEnum punchingType)
        {
            var date = DateTime.Today.Date;
            var query = $@"Select * from cms.""N_CLK_AccessLog"" where ""UserId""='{userId}' and ""DevicePunchingType""='{(int)punchingType}' and ""PunchingTime""::TIMESTAMP::DATE='{date}'::TIMESTAMP::DATE and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";
            var result = await _queryAccessLog.ExecuteQuerySingle(query, null);
            return result;
        }
        public async Task<IList<PersonDocumentViewModel>> GetPersonRequestDocumentList(string userId)
        {
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "PersonDocuments" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);
            string ids = null;
            var arrayId = templateList.Select(x => x.Id);
            ids = string.Join(',', arrayId);

            ids = ids.Replace(",", "','");

            var query = @$" Select s.""Id"" as ServiceId, s.""ServiceNo"",s.""TemplateCode"" as ""TemplateCode"", t.""DisplayName"" as DocumentType, s.""ServiceStatusId"", lov.""Name"" as Status, s.""StartDate"" as IssueDate
                                from public.""NtsService"" as s
                                join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
                                join public.""Template"" as t on t.""Id""=s.""TemplateId"" and s.""TemplateId"" in ('{ids}') and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                                where s.""OwnerUserId""='{userId}' and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' order by s.""CreatedDate"" desc
                                ";

            var result = await _queryRepo1.ExecuteQueryList<PersonDocumentViewModel>(query, null);
            return result;
        }
        public async Task<List<BusinessTripViewModel>> GetAllEmployee()
        {
            var query = "";


            query = $@"SELECT ""UserId"",CONCAT(""FirstName"", ""MiddleName"", ""LastName"") as EmployeeName FROM cms.""N_CoreHR_HRPerson"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
            var queryData = await _queryBusinessTrip.ExecuteQueryList<BusinessTripViewModel>(query, null);

            var list = new List<BusinessTripViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new BusinessTripViewModel { UserId = x.UserId, EmployeeName = x.EmployeeName }).ToList();
            return list;


        }


        public async Task<List<BusinessTripViewModel>> GetBusinessTripbyOwneruserId(string Id)
        {
            var query = "";


            query = $@"SELECT NS.""Id"" as NtsNoteId, B.""Id"",B.""SequenceOrder"",  LOV.""Name"" as Status,LOV.""Code"" as StatusCode, B.""VersionNo"", 
            B.""Purpose"",NS.""StartDate"",NS.""ServiceNo"",B.""BusinessTripStartDate""::TIMESTAMP::DATE as BusinessTripStartDate,B.""BusinessTripEndDate""::TIMESTAMP::DATE as BusinessTripEndDate
            ,cs.""Id"" as ClaimServiceId,cs.""ServiceNo"" as ClaimServiceNo
            FROM  public.""NtsService"" NS  
            inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	        inner join  cms.""N_EmployeeService_BuisnessTrip"" B on  N.""Id"" =B.""NtsNoteId"" and	 B.""IsDeleted""=false and B.""CompanyId""='{_userContext.CompanyId}'
            inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and  LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
            left join public.""NtsService"" as cs on cs.""ParentServiceId""=NS.""Id"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_userContext.CompanyId}'
            where N.""OwnerUserId""='{Id}' and  NS.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}' order by NS.""CreatedDate"" desc";
            var queryData = await _queryBusinessTrip.ExecuteQueryList<BusinessTripViewModel>(query, null);

            var list = new List<BusinessTripViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new BusinessTripViewModel { NtsNoteId = x.NtsNoteId, Id = x.Id, BusinessTripStartDate = x.BusinessTripStartDate, BusinessTripEndDate = x.BusinessTripEndDate, Status = x.Status, StatusCode = x.StatusCode, VersionNo = x.VersionNo, SequenceOrder = x.SequenceOrder, Purpose = x.Purpose, ServiceNo = x.ServiceNo, StartDate = x.StartDate, ClaimServiceId = x.ClaimServiceId, ClaimServiceNo = x.ClaimServiceNo }).ToList();
            return list;


        }




        public async Task<IList<DependentViewModel>> GetDependentList(string personId, string status)
        {
            string query = @$"select d.""Id"" as DependentId, d.""NtsNoteId"" as NoteId, d.""FirstName"", d.""DateOfBirth"", d.""IqamahIdNationalityId"", lov.""Name"" as RelationshipTypeName
                            from cms.""N_CoreHR_HRDependant"" as d
                            join public.""NtsNote"" as nts on nts.""Id""=d.""NtsNoteId"" and  nts.""IsDeleted""=false and nts.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as ntslov on ntslov.""Id""=nts.""NoteStatusId"" and  ntslov.""IsDeleted""=false and ntslov.""CompanyId""='{_userContext.CompanyId}'
                            join public.""LOV"" as lov on lov.""Id""=d.""RelationshipTypeId"" and  lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
                            where d.""EmployeeId""='{personId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}' #STATUSWHERE# order by d.""CreatedDate"" desc ";

            var where = "";
            if (status.IsNotNullAndNotEmpty())
            {
                where = $@" and ntslov.""Code""='{status}' ";
            }
            query = query.Replace("#STATUSWHERE#", where);

            var result = await _queryDependent.ExecuteQueryList<DependentViewModel>(query, null);
            return result;
        }


        public async Task<IList<PersonDocumentViewModel>> GetDependentDocumentList(string dependentId)
        {
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Dependent Documents" && x.TemplateType == TemplateTypeEnum.Note);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);

            var selectQry = "";
            var i = 1;
            foreach (var item in templateList)
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);

                if (i != 1)
                {
                    selectQry += " union ";
                }

                selectQry = @$" {selectQry} select nts.""Id"" as DepNoteId, nts.""NoteNo"", ndep.""CreatedDate"", t.""Code"" as TemplateCode, t.""DisplayName"" as DocumentType, ndep.""ExpireDate"" as ExpiryDate
                                    from cms.""{tableMeta.Name}"" as ndep
                                    join public.""NtsNote"" as nts on nts.""Id""=ndep.""NtsNoteId"" and  nts.""IsDeleted""=false and nts.""CompanyId""='{_userContext.CompanyId}'
                                    join public.""Template"" as t on t.""Id"" = nts.""TemplateId"" and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                                    where ndep.""DependentId""='{dependentId}' and  ndep.""IsDeleted""=false and ndep.""CompanyId""='{_userContext.CompanyId}'
                                ";
                i++;
            }

            var result = await _queryRepo1.ExecuteQueryList<PersonDocumentViewModel>(selectQry, null);
            result = result.OrderByDescending(x => x.CreatedDate).ToList();
            return result;
        }

        public async Task<IList<PersonDocumentViewModel>> GetDependentRequestDocumentList(string userId)
        {
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Dependent Documents" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);
            string ids = null;
            var arrayId = templateList.Select(x => x.Id);
            ids = string.Join(',', arrayId);

            ids = ids.Replace(",", "','");

            var query = @$" Select s.""Id"" as ServiceId, s.""ServiceNo"", t.""Code"" as TemplateCode, t.""DisplayName"" as DocumentType, s.""ServiceStatusId"", lov.""Name"" as Status, s.""StartDate"" as IssueDate
                                from public.""NtsService"" as s
                                join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and  lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
                                join public.""Template"" as t on t.""Id""=s.""TemplateId"" and s.""TemplateId"" in ('{ids}') and  t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                                where s.""OwnerUserId""='{userId}' and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'  order by s.""CreatedDate"" desc
                                ";

            var result = await _queryRepo1.ExecuteQueryList<PersonDocumentViewModel>(query, null);
            return result;
        }
        public async Task<List<MisconductViewModel>> GetMisconductDetails(string Id)
        {
            var query = "";
            query = $@"SELECT m.""Id"",  LOV.""Name"" as MisconductTypeName,LOV1.""Name"" as DisciplinaryActionTakenName, 
                            NS.""ServiceNo"", NS.""StartDate""::TIMESTAMP::DATE as MisconductDate,NS.""Id"" as ServiceId, LOVS.""Name"" as Status
                            FROM  public.""NtsService"" NS
                            inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and  N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	                        inner join  cms.""N_ManpowerService_MisconductRequest"" m on  N.""Id"" =m.""NtsNoteId""	and  m.""IsDeleted""=false and m.""CompanyId""='{_userContext.CompanyId}'
                            inner join public.""LOV"" as LOV on m.""MisconductTypeId""=LOV.""Id"" and  LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
                            inner join public.""LOV"" as LOVS on NS.""ServiceStatusId""=LOVS.""Id"" and  LOVS.""IsDeleted""=false and LOVS.""CompanyId""='{_userContext.CompanyId}'
                            inner join public.""LOV"" as LOV1 on m.""DisciplinaryActionTakenId""=LOV1.""Id"" and  LOV1.""IsDeleted""=false and LOV1.""CompanyId""='{_userContext.CompanyId}'
                            where N.""OwnerUserId""='{Id}' and  NS.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}' order by NS.""CreatedDate"" desc ";
            var queryData = await _queryMisconduct.ExecuteQueryList<MisconductViewModel>(query, null);

            var list = new List<MisconductViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new MisconductViewModel { Id = x.Id, DisciplinaryActionTakenName = x.DisciplinaryActionTakenName, MisconductTypeName = x.MisconductTypeName, ServiceNo = x.ServiceNo, MisconductDate = x.MisconductDate, ServiceId = x.ServiceId, Status = x.Status }).ToList();
            return list;

        }

        public async Task<List<ReimbursementRequestViewModel>> GetReimbursementRequestItemList(string reimbursementRequestItem)
        {
            var query = "";
            query = $@"select  r.""Id"", r.""ReimbursementRequestId"", r.""ItemDate"", r.""ItemDescription"", r.""Amount""
                        from cms.""F_HR_ReimbursementRequestItem"" as r
                        where r.""ReimbursementRequestId"" = '{reimbursementRequestItem}' and r.""IsDeleted"" = false
                        and r.""CompanyId"" = '{_userContext.CompanyId}' order by r.""CreatedDate"" desc  ";
            var list = await _queryMisconduct.ExecuteQueryList<ReimbursementRequestViewModel>(query, null);
            return list;
        }

        public async Task<bool> DeleteReimbursementRequestItem(string id)
        {
            var query = "";
            query = $@"UPDATE cms.""F_HR_ReimbursementRequestItem"" SET ""IsDeleted"" = 'True' WHERE ""Id"" = '{id}'";
            await _queryRepo1.ExecuteCommand(query, null);
            return true;
        }

        public async Task<ReimbursementRequestViewModel> CreateReimbursementRequestItem(ReimbursementRequestViewModel model)
        {
            var query = "";
            query = $@"
                INSERT INTO cms.""F_HR_ReimbursementRequestItem""(ItemDate, ItemDescription, Amount, ReimbursementRequestId)
                VALUES ('{model.ItemDate}', '{model.ItemDescription}', '{model.Amount}', '{model.ReimbursementRequestId}')
                RETURNING *";
            await _queryRepo1.ExecuteCommand(query, null);
            return model;
        }

        public async Task<ReimbursementRequestViewModel> UpdateReimbursementRequestItem(ReimbursementRequestViewModel model)
        {
            var query = "";
            query = $@"UPDATE cms.""F_HR_ReimbursementRequestItem"" SET 
            ""ItemDate"" = '{model.ItemDate}',
            ""ItemDescription"" = '{model.ItemDescription}',
            ""Amount"" = '{model.Amount}',
            WHERE ""Id"" = '{model.Id}'
            RETURNING *";
            await _queryRepo1.ExecuteCommand(query, null);
            return model;
        }

        public async Task<ReimbursementRequestViewModel> GetReimbursementRequestItemData(string id)
        {
            var query = "";
            query = $@"select  r.""Id"", r.""ReimbursementRequestId"", r.""ItemDate"", r.""ItemDescription"", r.""Amount""
                        from cms.""F_HR_ReimbursementRequestItem"" as r
                        where r.""Id"" = '{id}' and r.""IsDeleted"" = false
                        and r.""CompanyId"" = '{_userContext.CompanyId}' order by r.""CreatedDate"" desc  ";
            var data = await _queryMisconduct.ExecuteScalar<ReimbursementRequestViewModel>(query, null);
            return data;

        }
        public async Task<IList<AccessLogViewModel>> GetAccessLogList(string Id, string UserId, string userIds = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var model = await GetPostionHierarchyParentId(Id);
            string sid = "";
            if (model != null)
            {
                var data = await GetPositionHierarchyUsers(model.Id, 100);
                string[] sids = data.Select(x => x.Id).ToArray();

                sid = string.Join(",", sids);
                sid = sid.Trim(',').Replace(",", "','");
            }
            //var str= string.Concat(UserId,",", sid);
            //str = str.Replace(",", "','");

            string query = @$"select a.*,CONCAT( p.""FirstName"",' ',p.""LastName"") as PersonFullName,a.""SponsorshipNo"" as SponsorshipNo,a.""BiometricId"" as BiometricId,
                                substring(a.""PunchingTime"",1,16) as PunchingTime, a.""DevicePunchingType"" as DevicePunchingType,d.""Name"" as DeviceName,a.""PersonId"" as PersonId,
                                a.""NtsNoteId"",u.""Id"" as UserId
                                from cms.""N_CLK_AccessLog"" as a
                                left join public.""NtsNote"" as nts on nts.""Id""=a.""NtsNoteId"" and  nts.""IsDeleted""=false and nts.""CompanyId""='{_userContext.CompanyId}'
                                left join cms.""N_CoreHR_HRPerson"" as p on a.""PersonId"" = p.""Id"" and  p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
                                left join public.""User"" as u on p.""UserId""=u.""Id"" and  u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                                left join cms.""N_CLK_Device"" as d on d.""Id""=a.""DeviceId"" and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                where u.""Id"" in('{sid}') and  a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'  #StartDateWhere# order by a.""PunchingTime"" desc";

            var startdateSearch = "";
            if (startDate.IsNotNull() && dueDate.IsNotNull())
            {
                // startdateSearch = @$" and nts.""StartDate"" ='{startDate.ToYYYY_MM_DD_DateFormat() }'";
                // startdateSearch = @$" and nts.""StartDate""='{ startDate.ToYYYY_MM_DD_DateFormat() }' <=  a.""PunchingTime"" and a.""PunchingTime"" < nts1.""ExpiryDate""='{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                startdateSearch = @$" and '{ startDate.ToYYYY_MM_DD_DateFormat() }'<= a.""PunchingTime""::TIMESTAMP::DATE and a.""PunchingTime""::TIMESTAMP::DATE <= '{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
            }

            query = query.Replace("#StartDateWhere#", startdateSearch);

            var result = await _queryAccessLog.ExecuteQueryList<AccessLogViewModel>(query, null);
            if (userIds.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.UserId == userIds).ToList();
            }
            return result;
        }


        public async Task<IList<AccessLogViewModel>> GetAllAccessLogList(DateTime? startDate = null, DateTime? dueDate = null, string userId = null)
        {
            string query = @$"select a.*,CONCAT( p.""FirstName"",' ',p.""LastName"") as PersonFullName,a.""SponsorshipNo"" as SponsorshipNo,a.""BiometricId"" as BiometricId,
a.""PunchingTime"" as PunchingTime, a.""DevicePunchingType"" as DevicePunchingType,d.""Name"" as DeviceName,a.""PersonId"" as PersonId,
a.""NtsNoteId""
from cms.""N_CLK_AccessLog"" as a
left join public.""NtsNote"" as nts on nts.""Id""=a.""NtsNoteId"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as p on a.""PersonId"" = p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CLK_Device"" as d on d.""Id""=a.""DeviceId"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                          where 1=1 and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}' #UserWhere# #StartDateWhere# --#DueDateWhere# order by a.""CreatedDate"" desc ";


            var startdateSearch = "";
            if (startDate.IsNotNull() && dueDate.IsNotNull())
            {

                // startdateSearch = @$" and nts.""StartDate"" ='{startDate.ToYYYY_MM_DD_DateFormat() }'";
                // startdateSearch = @$" and nts.""StartDate""='{ startDate.ToYYYY_MM_DD_DateFormat() }' <=  a.""PunchingTime"" and a.""PunchingTime"" < nts1.""ExpiryDate""='{ dueDate.ToYYYY_MM_DD_DateFormat() }' ";
                startdateSearch = @$" and '{startDate}'::Date <= a.""PunchingTime""::Date and a.""PunchingTime""::Date <= '{dueDate}'::Date ";
            }
            var userwhere = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                userwhere = $@" and u.""Id""='{userId}' ";
            }
            query = query.Replace("#UserWhere#", userwhere);
            query = query.Replace("#StartDateWhere#", startdateSearch);

            //var duedateSearch = "";
            //if (dueDate.IsNotNull())
            //{

            //    duedateSearch = @$" and nts1.""ExpiryDate"" ='{dueDate.ToYYYY_MM_DD_DateFormat() }'";

            //}

            //query = query.Replace("#DueDateWhere#", duedateSearch);
            var result = await _queryAccessLog.ExecuteQueryList<AccessLogViewModel>(query, null);
            return result;
        }


        public async Task<IList<ResignationTerminationViewModel>> GetResignationTerminationList(string EmpId)
        {
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Separation" && x.TemplateType == TemplateTypeEnum.Note);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id && x.Code == "SN_Resignation" || x.Code == "SN_Termination"); ;

            var selectQry = "";
            var Date = "";
            var i = 1;
            foreach (var item in templateList)
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);

                if (i != 1)
                {
                    selectQry += " union ";
                }




                selectQry = $@" {selectQry} SELECT Ns.""Id"",  RT.""ResignationTerminationDate""::TIMESTAMP::DATE  as ResignationTerminationDate,RT.""LastWorkingDate""::TIMESTAMP::DATE  as LastWorkingDate,t.""DisplayName"", NS.""ServiceNo"" as ServiceNo,N.""NoteSubject"" as Subject,

    LOV.""Name"" as ServiceStatus
    FROM  public.""NtsService"" NS
    inner join public.""NtsNote"" N on NS.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	inner join  cms.""{tableMeta.Name}"" as  RT on  N.""Id"" =RT.""NtsNoteId""	and RT.""IsDeleted""=false and RT.""CompanyId""='{_userContext.CompanyId}'
    inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
 inner join public.""Template"" as t on t.""Id"" = N.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
    where N.""OwnerUserId""='{EmpId}' and NS.""IsDeleted""='false' and  NS.""CompanyId""='{_userContext.CompanyId}' ";
                i++;
            }
            var queryData = await _queryBusinessTrip.ExecuteQueryList<ResignationTerminationViewModel>(selectQry, null);
            var list = new List<ResignationTerminationViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ResignationTerminationViewModel { Id = x.Id, ServiceNo = x.ServiceNo, Subject = x.Subject, ResignationTerminationDate = x.ResignationTerminationDate, LastWorkingDate = x.LastWorkingDate, ServiceStatus = x.ServiceStatus, DisplayName = x.DisplayName }).ToList();
            return list;
        }


        public async Task<JobDetViewModel> GetJobDescription(string organization, string JobId)
        {
            var query = "";
            var name = new JobDetViewModel();
            try
            {
                query = @$" select N.""Id"",JD.""OrganizationId"",JD.""JobId"",D.""DepartmentName"",j.""JobTitle"",
JD.""JobDescription"",JD.""Responsibility""
FROM
    public.""NtsNote"" N
    inner join cms.""N_CoreHR_JobDescription"" JD on  N.""Id"" =JD.""NtsNoteId"" and JD.""IsDeleted""='false' and  JD.""CompanyId""='{_userContext.CompanyId}'
    left join  cms.""N_CoreHR_HRDepartment"" D on D.""Id""=JD.""OrganizationId"" and D.""IsDeleted""='false' and  D.""CompanyId""='{_userContext.CompanyId}'
    left join cms.""N_CoreHR_HRJob"" j on j.""Id""=JD.""JobId"" and j.""IsDeleted""='false' and  j.""CompanyId""='{_userContext.CompanyId}'

    where JD.""OrganizationId""='{organization}' and JD.""JobId""='{JobId}' and N.""IsDeleted""='false' and  N.""CompanyId""='{_userContext.CompanyId}'";
                name = await _JobDetViewModel.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        public async Task CreatePositionHierarchy(NoteTemplateViewModel viewModel)
        {
            var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == "POS_HIERARCHY");
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var Parent = rowData.GetValueOrDefault("ParentPositionId").IsNotNull() ? rowData.GetValueOrDefault("ParentPositionId").ToString() : null;
            if (viewModel.DataAction == DataActionEnum.Create && Parent.IsNotNullAndNotEmpty())
            {

                var position = viewModel.UdfNoteTableId;

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.TemplateCode = "PositionHierarchy";
                var note = await _noteBusiness.GetNoteDetails(noteTemp);

                note.OwnerUserId = _repo.UserContext.UserId;
                note.StartDate = DateTime.Now;
                note.Json = "{}";
                note.DataAction = DataActionEnum.Create;

                //var list = new List<System.Dynamic.ExpandoObject>();
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ParentPositionId", Parent);
                ((IDictionary<String, Object>)exo).Add("PositionId", position);
                if (hierarchy != null)
                {
                    var hierarchyId = hierarchy.Id;
                    ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                }
                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _noteBusiness.ManageNote(note);
            }
            else if (viewModel.DataAction == DataActionEnum.Edit && Parent.IsNotNullAndNotEmpty())
            {
                // var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                // var Parent = rowData.GetValueOrDefault("ParentPositionId");
                var position = viewModel.UdfNoteTableId;
                var existingposhierarchy = await _tableMetadataBusiness.GetTableDataByColumn("PositionHierarchy", null, "PositionId", position);
                if (existingposhierarchy != null)
                {
                    var poshierarchyId = Convert.ToString(existingposhierarchy["NtsNoteId"]);
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "PositionHierarchy";
                    noteTemp.NoteId = poshierarchyId;
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.OwnerUserId = _repo.UserContext.UserId;
                    note.StartDate = DateTime.Now;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Edit;

                    //var list = new List<System.Dynamic.ExpandoObject>();
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("ParentPositionId", Parent);
                    ((IDictionary<String, Object>)exo).Add("PositionId", position);
                    if (hierarchy != null)
                    {
                        var hierarchyId = hierarchy.Id;
                        ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                    }
                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var res = await _noteBusiness.ManageNote(note);
                }

            }


        }

        public async Task CreateDepartmentHierarchy(NoteTemplateViewModel viewModel)
        {
            var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == "ORG_HIERARCHY");
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var Parent = rowData.GetValueOrDefault("ParentDepartmentId").IsNotNull() ? rowData.GetValueOrDefault("ParentDepartmentId").ToString() : null;
            if (viewModel.DataAction == DataActionEnum.Create && Parent.IsNotNullAndNotEmpty())
            {

                var department = viewModel.UdfNoteTableId;

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.TemplateCode = "HRDepartmentHierarchy";
                var note = await _noteBusiness.GetNoteDetails(noteTemp);

                note.OwnerUserId = _repo.UserContext.UserId;
                note.StartDate = DateTime.Now;
                note.Json = "{}";
                note.DataAction = DataActionEnum.Create;
                //var list = new List<System.Dynamic.ExpandoObject>();
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ParentDepartmentId", Parent);
                ((IDictionary<String, Object>)exo).Add("DepartmentId", department);
                if (hierarchy != null)
                {
                    var hierarchyId = hierarchy.Id;
                    ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                }

                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _noteBusiness.ManageNote(note);
            }
            else if (viewModel.DataAction == DataActionEnum.Edit && Parent.IsNotNullAndNotEmpty())
            {
                //  var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                // var Parent = rowData.GetValueOrDefault("ParentDepartmentId");
                var department = viewModel.UdfNoteTableId;
                var existingdephierarchy = await _tableMetadataBusiness.GetTableDataByColumn("HRDepartmentHierarchy", null, "DepartmentId", department);
                // var hierarchyId = "";

                if (existingdephierarchy != null)
                {
                    var dephierarchyId = Convert.ToString(existingdephierarchy["NtsNoteId"]);
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "HRDepartmentHierarchy";
                    noteTemp.NoteId = dephierarchyId;
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.OwnerUserId = _repo.UserContext.UserId;
                    note.StartDate = DateTime.Now;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Edit;

                    //var list = new List<System.Dynamic.ExpandoObject>();
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("ParentDepartmentId", Parent);
                    ((IDictionary<String, Object>)exo).Add("DepartmentId", department);
                    if (hierarchy != null)
                    {
                        var hierarchyId = hierarchy.Id;
                        ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                    }


                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    var res = await _noteBusiness.ManageNote(note);
                }


            }


        }
        public async Task<bool> ValidateUserMappingToPerson(NoteTemplateViewModel viewModel)
        {

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var userId = Convert.ToString(rowData.GetValueOrDefault("UserId"));
            var exisiting = await CheckPersonWithUserId(userId);
            if (exisiting.Count > 1)
            {
                return false;
            }
            else
            {
                return true;
            }



        }
        public async Task<bool> ValidateUniqueDepartment(NoteTemplateViewModel viewModel)
        {
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);

            var exisiting = await GetDepartmentByName(rowData["DepartmentName"].ToString());
            if (exisiting.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<List<DepartmentViewModel>> GetDepartmentByName(string departmentName)
        {
            string query = @$"SELECT dpt.*
                                    FROM cms.""N_CoreHR_HRDepartment"" as dpt
                                where dpt.""IsDeleted""=false and dpt.""DepartmentName""='{departmentName}' 
                            ";
            var result = await _queryRepo1.ExecuteQueryList<DepartmentViewModel>(query, null);
            return result;
        }
        public async Task<bool> ValidateLeaveStartDateandEndDate(ServiceTemplateViewModel viewModel)
        {

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var startDate = Convert.ToString(rowData.GetValueOrDefault("LeaveStartDate"));
            var endDate = Convert.ToString(rowData.GetValueOrDefault("LeaveEndDate"));
            var newId = viewModel.UdfNoteTableId;
            var query =
                $@" select ""Id""
                    from cms.""N_Leave_AnnualLeave"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        	  
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_MaternityLeave""
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        	  
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_PaternityLeave"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        	  
                        union 	  
                         select ""Id""
                        from  cms.""N_Leave_CompassionatelyLeave"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        	   
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_HajjLeave"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}' 
                        	  
                        
                        	  
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_LeaveExamination""
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        union 	  
                        select ""Id""
                        from  cms.""N_Leave_MarriageLeave""
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_PlannedUnpaidLeave""
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_SickLeave"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_UndertimeLeave""
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                                         (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
                        
                        union 	  
                        select ""Id""
                        from cms.""N_Leave_UnpaidLeave""
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""LeaveStartDate"">='{startDate}' and ""LeaveStartDate""<='{endDate}')
                                                    or
                                 (""LeaveEndDate"">='{startDate}' and ""LeaveEndDate""<='{endDate}')) and ""Id""!='{newId}'
								 

								 

                             ";

            var exisiting = await _queryRepo1.ExecuteQueryList(query, null);
            if (exisiting.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<List<IdNameViewModel>> GetNonExistingDepartment(string hierarchyId, string deptId)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);
            string query = $@"{companyquery} select dep.""Id"" as Id ,dep.""DepartmentName"" as Name
                            from cms.""N_CoreHR_HRDepartment"" as dep
                            join  ""Department"" as dept on dept.""DepartmentId""=dep.""Id""
                          
where ((dep.""IsDeleted""=false and  dep.""CompanyId""='{_userContext.CompanyId}' and dep.""ParentDepartmentId"" is null) and dep.""Id"" not in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}') and dep.""Id"" not in 
	  (SELECT ""DepartmentId"" FROM cms.""N_CoreHR_HRDepartmentHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}') 
and dep.""Id"" not in 	  (SELECT ""ParentDepartmentId"" FROM cms.""N_CoreHR_HRDepartmentHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}')) or dep.""Id""='{deptId}' ";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
        }

        public async Task<List<IdNameViewModel>> GetDepartmentWithParent(string hierarchyId, string deptId)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);
            string query = $@"{companyquery} select dep.""Id"" as Id ,dep.""DepartmentName"" as Name
                            from cms.""N_CoreHR_HRDepartment"" as dep
                            join  ""Department"" as dept on dept.""DepartmentId""=dep.""Id""
                          
where ((dep.""IsDeleted""=false and  dep.""CompanyId""='{_userContext.CompanyId}' and dep.""ParentDepartmentId"" is not null) or dep.""Id"" in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'))  or dep.""Id""='{deptId}' ";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
        }

        public async Task<List<IdNameViewModel>> GetUserWithParent(string hierarchyId, string parentuserId, string userId)
        {

            string query = $@"select dep.""Id"" as Id ,dep.""Name"" as Name
                            from  ""User"" as dep 
                          join public.""UserPortal"" as up on up.""UserId""=dep.""Id"" and up.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}'
where ((dep.""IsDeleted""=false and  dep.""CompanyId""='{_userContext.CompanyId}' and dep.""Id""<>'{userId}' ) or dep.""Id"" in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'))  or dep.""Id""='{parentuserId}'   ";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetNonExistingPosition(string hierarchyId, string positionId)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);
            string query = $@"{companyquery} select dep.""PositionName"" as Name,dep.""Id"" as Id
from cms.""N_CoreHR_HRPosition"" as dep
join  ""Department"" as dept on dept.""DepartmentId""=dep.""DepartmentId""
where ((dep.""IsDeleted""=false and  dep.""CompanyId""='{_userContext.CompanyId}' and dep.""ParentPositionId"" is null) and dep.""Id"" not in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}') and dep.""Id"" not in 
	  (SELECT ""PositionId"" FROM cms.""N_CoreHR_PositionHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}')) or dep.""Id""='{positionId}'
";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
        }


        public async Task<List<IdNameViewModel>> GetPositionWithParent(string hierarchyId, string positionId)
        {
            var companyquery = Helper.OrganizationMapping(_userContext.UserId, _userContext.CompanyId, _userContext.LegalEntityId);
            string query = $@"{companyquery} select dep.""PositionName"" as Name,dep.""Id"" as Id
from cms.""N_CoreHR_HRPosition"" as dep
join  ""Department"" as dept on dept.""DepartmentId""=dep.""DepartmentId""
where ((dep.""IsDeleted""=false and dep.""CompanyId""='{_userContext.CompanyId}' and dep.""ParentPositionId"" is not null) or dep.""Id"" in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}')) or dep.""Id""='{positionId}'
";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
        }


        public async Task<List<AssignmentViewModel>> GetUserPerformanceDocumentInfo(string userId, string PerformanceId, string MasterStageId)
        {
            string query = $@" select distinct CONCAT( hp.""FirstName"",' ',hp.""LastName"") as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
assi.""DateOfJoin"",assi.""Id"" as AssignmentId,hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,pu.""Id"" as ""ManagerUserId"",
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,pd.""Id"" as PerformanceDocumentId,
pd.""ServiceSubject"" as PerformanceDocumentName,pd.""StartDate"" as PDStartDate,pd.""DueDate"" as PDEndDate,pdudf.""Year"" as PDYear,
case when pd.""Status""=1 then 'Active' else 'InActive' end as PDStatus,CONCAT( pp.""FirstName"",' ',pp.""LastName"") as ManagerPersonFullName,mj.""JobTitle"" as ManagerJobName
,sp.""ImageId"" as SponsorLogoId
--,pdudf.""FinalRatingRounded"" as FinalScore
,pds.""FinalRatingRounded""  as FinalScore
--,'His deliverables appreciated by the business since he can give attention to all details and provide a complete solution that meets the business needs' as FinalComments
,pds.""ManagerComment"" as FinalComments
from 
public.""User"" as u 
join public.""NtsService"" as pd on pd.""OwnerUserId""=u.""Id"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
join public.""Template"" as temp on temp.""Id""=pd.""TemplateId"" and temp.""Code""='PMS_PERFORMANCE_DOCUMENT' and temp.""IsDeleted""=false and temp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pdudf on pdudf.""NtsNoteId""=pd.""UdfNoteId"" and pdudf.""IsDeleted""=false and pdudf.""CompanyId""='{_userContext.CompanyId}'

left join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdudf.""DocumentMasterId""=pdm.""Id"" and pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'
left join  public.""NtsNote"" n on n.""ParentNoteId""=pdm.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}' 
left join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on n.""Id""=pdms.""NtsNoteId"" and pdms.""IsDeleted""=false and pdms.""CompanyId""= '{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""DocumentMasterStageId""=pdms.""Id"" and pds.""IsDeleted""=false and pds.""CompanyId""= '{_userContext.CompanyId}'

left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false and hpos.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false and hl.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false and hg.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_PositionHierarchy"" as ph on ph.""PositionId""=assi.""PositionId"" and ph.""IsDeleted""=false and ph.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as pa on ph.""ParentPositionId""=pa.""PositionId"" and pa.""IsDeleted""=false and pa.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as mj on mj.""Id""=pa.""JobId"" and mj.""IsDeleted""=false and mj.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as pp on pp.""Id""=pa.""EmployeeId"" and pp.""IsDeleted""=false and pp.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as pu on pu.""Id""=pp.""UserId"" and pu.""IsDeleted""=false and pu.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as sp on sp.""Id""=u.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_userContext.CompanyId}'
 where u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' #WHERE#";
            var search = "";

            if (PerformanceId.IsNotNullAndNotEmpty())
            {
                search = search + @$" and pd.""Id""='{PerformanceId}' ";

            }
            if (MasterStageId.IsNotNullAndNotEmpty())
            {
                search = search + @$" and pdms.""Id""='{MasterStageId}' ";
            }
            query = query.Replace("#WHERE#", search);
            var queryData = await _queryAssignment.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<AssignmentViewModel> GetUserLineManagerFromPerformanceHierarchy(string userId)
        {
            string query = $@"  select uh.*,'DIRECT' as Type,uh.""ParentUserId"" as ManagerUserId
from public.""HierarchyMaster"" as hm
join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false and uh.""CompanyId""='{_userContext.CompanyId}'
join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
	--join public.""User"" as p on p.""Id""=uh.""ParentUserId"" and u.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
where hm.""Code""='PERFORMANCE_HIERARCHY' and uh.""UserId""='{userId}' and hm.""CompanyId""='{_userContext.CompanyId}'
and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false";

            var queryData = await _queryAssignment.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<AssignmentViewModel> GetUserFullInfo(string serviceId)
        {
            string query = $@" select CONCAT( hp.""FirstName"",' ',hp.""LastName"") as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
            assi.""DateOfJoin"",assi.""Id"" as AssignmentId,hpos.""Id"" as PositionId,u.""Id"" as ""UserId"",u.""PhotoId"" as PhotoId,u.""Email"" as ""Email"",hp.""MobileNumber"" as ""WorkPhone"",
            hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,cc.""AnnualLeaveEntitlement"" as AnnualLeaveEntitlement,hp.""PersonNo"" as ""PersonNo""
            from 
            public.""User"" as u 
            join public.""NtsService"" as pd on pd.""OwnerUserId""=u.""Id"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false and hpos.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false and hl.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false and hg.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_PositionHierarchy"" as ph on ph.""PositionId""=assi.""PositionId"" and ph.""IsDeleted""=false and ph.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRAssignment"" as pa on ph.""ParentPositionId""=pa.""PositionId"" and pa.""IsDeleted""=false and pa.""CompanyId""='{_userContext.CompanyId}'
            left join cms.""N_CoreHR_HRContract"" as cc on hp.""Id""=cc.""EmployeeId"" and cc.""IsDeleted""=false and cc.""CompanyId""='{_userContext.CompanyId}'
             where pd.""Id""='{serviceId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'";


            var queryData = await _queryAssignment.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<List<AssignmentViewModel>> GetUsersInfo(string deptId = null)
        {
            string query = $@"select u.""Name"" as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,
                hp.""PersonNo"" as PersonNo,u.""PhotoId"" as PhotoId,u.""Email"" as Email,u.""Id"" as UserId
                from 
                public.""User"" as u 
                 join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
                 join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
                 left join public.""LOV"" as lovs on lovs.""Id""=assi.""AssignmentStatusId"" and lovs.""IsDeleted""=false and  lovs.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
                 join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}' 
                  where u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}' and lovs.""Code""='ASSIGNMENT_STATUS_ACTIVE' #WHERE# ";

            var where = "";
            if (deptId.IsNotNullAndNotEmpty())
            {
                where = $@" and hd.""Id""='{deptId}'";
            }
            query = query.Replace("#WHERE#", where);

            var queryData = await _queryAssignment.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<List<AssignmentViewModel>> GetUserLetterTemplateDetails(string userId, string PerformanceId, string MasterStageId)
        {
            string query = $@" select distinct CONCAT( hp.""FirstName"",' ',hp.""LastName"") as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
assi.""DateOfJoin"",assi.""Id"" as AssignmentId,hpos.""Id"" as PositionId,u.""PhotoId"" as PhotoId,pu.""Id"" as ""ManagerUserId"",
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,pd.""Id"" as PerformanceDocumentId,salelminfo.""Amount"" as BasicSalary,
pd.""ServiceSubject"" as PerformanceDocumentName,pdudf.""StartDate"" as PDStartDate,pdudf.""EndDate"" as PDEndDate,pdudf.""Year"" as PDYear,pdudf.""FinalRatingRounded"" as PDFinalRatingRounded,pdudf.""Bonus"" as PDBonus,pdudf.""Increment"" as PDIncrement,
case when pd.""Status""=1 then 'Active' else 'InActive' end as PDStatus,CONCAT( pp.""FirstName"",' ',pp.""LastName"") as ManagerPersonFullName,mj.""JobTitle"" as ManagerJobName
from 
public.""User"" as u 
join public.""NtsService"" as pd on pd.""OwnerUserId""=u.""Id"" and pd.""IsDeleted""=false and pd.""CompanyId""='{_userContext.CompanyId}'
join public.""Template"" as temp on temp.""Id""=pd.""TemplateId"" and temp.""Code""='PMS_PERFORMANCE_DOCUMENT' and temp.""IsDeleted""=false and temp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pdudf on pdudf.""NtsNoteId""=pd.""UdfNoteId"" and pdudf.""IsDeleted""=false and pdudf.""CompanyId""='{_userContext.CompanyId}'

left join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdudf.""DocumentMasterId""=pdm.""Id"" and pdm.""IsDeleted""=false and pdm.""CompanyId""='{_userContext.CompanyId}'
left join  public.""NtsNote"" n on n.""ParentNoteId""=pdm.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}' 
left join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" as pdms on n.""Id""=pdms.""NtsNoteId"" and pdms.""IsDeleted""=false and pdms.""CompanyId""= '{_userContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" as pds on pds.""DocumentMasterStageId""=pdms.""Id"" and pds.""IsDeleted""=false and pds.""CompanyId""= '{_userContext.CompanyId}'

left join cms.""N_CoreHR_HRPerson"" as hp on hp.""UserId""=u.""Id"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false and hpos.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false and hl.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false and hg.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_PositionHierarchy"" as ph on ph.""PositionId""=assi.""PositionId"" and ph.""IsDeleted""=false and ph.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as pa on ph.""ParentPositionId""=pa.""PositionId"" and pa.""IsDeleted""=false and pa.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as mj on mj.""Id""=pa.""JobId"" and mj.""IsDeleted""=false and mj.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as pp on pp.""Id""=pa.""EmployeeId"" and pp.""IsDeleted""=false and pp.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as pu on pu.""Id""=pp.""UserId"" and pu.""IsDeleted""=false and pu.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryInfo"" as salinfo on salinfo.""PersonId""=hp.""Id"" and salinfo.""IsDeleted""=false and salinfo.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as salnote on salnote.""ParentNoteId""=salinfo.""NtsNoteId"" and salnote.""IsDeleted""=false and salnote.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryElementInfo"" as salelminfo on salelminfo.""NtsNoteId""=salnote.""Id"" and salelminfo.""IsDeleted""=false and salelminfo.""CompanyId""='{_userContext.CompanyId}'
and salelminfo.""ElementId""=(select payelm.""Id"" from cms.""N_PayrollHR_PayrollElement"" as payelm where payelm.""ElementCode""='BASIC' and payelm.""IsDeleted""=false and payelm.""CompanyId""='{_userContext.CompanyId}')
 where u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'  #WHERE#";
            var search = "";

            if (PerformanceId.IsNotNullAndNotEmpty())
            {
                search = search + @$" and pd.""Id""='{PerformanceId}' ";
            }
            if (MasterStageId.IsNotNullAndNotEmpty())
            {
                search = search + @$" and pdms.""Id""='{MasterStageId}' ";
            }
            query = query.Replace("#WHERE#", search);
            var queryData = await _queryAssignment.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<PersonProfileViewModel> GetEmployeeProfile(string personId)
        {
            string query = "";

            if (personId.IsNotNullAndNotEmpty())
            {
                query = $@"select p.*, p.""Id"" as ""PersonId"", CONCAT( p.""FirstName"",' ',p.""LastName"") as PersonFullName, LOV1.""Name"" as Title,
LOV2.""Name"" as Gender, LOV3.""Name"" as Religion, LOV4.""Name"" as MaritalStatus,p.""ContactPersonalEmail"" as PersonalEmail
,p.""CountryCode"" as ContactCountryDialCode,p.""MobileNumber"" as Mobile,c.""CountryName"" as ""ContactCountryName""
,l.""LocationName"",j.""JobTitle"" as JobName,po.""PositionName""
,g.""GradeName"",p.""PersonNo"",p.""DateOfBirth"" as DateOfBirth,assi.""Id"" as AssignmentId,p.""PresentAddressUnitNumber"" as ""PresentUnitNumber""
,p.""PresentAddressBuildingNumber"" as ""PresentBuildingNumber"",p.""PresentAddressStreetName"" as ""PresentStreetName""
,p.""PresentAddressCityOrTown"" as ""PresentCity"",p.""PresentAddressPostalCode"" as ""PresentPostalCode""
,p.""PresentAddressAdditionalNumber"" as ""PresentAdditionalNumber"",c.""CountryName"" as ""PresentCountryName"",
n.""NationalityName"" ,p.""PermanentAddressUnitNumber"" as ""HomeUnitNumber""
,p.""PermanentAddressBuildingNumber"" as ""HomeBuildingNumber"",p.""PermanentAddressStreetName"" as ""HomeStreetName""
,p.""PermanentAddressCityOrTown"" as ""HomeCity"",p.""PermanentAddressPostalCode"" as ""HomePostalCode""
,p.""PermanentAddressAdditionalNumber"" as ""HomeAdditionalNumber"",c2.""CountryName"" as ""HomeCountryName""
,p.""EmergencyContactCountryCode1"" as ""EmergencyContactCountryDialCode1"",LOV5.""Name"" as Relationship1,
p.""EmergencyContactMobileNumber1"" as ""EmergencyContactNo1"",c3.""CountryName"" as ""EmergencyContactCountryName1""
,p.""EmergencyContactCountryCode2"" as ""EmergencyContactCountryDialCode2"",LOV6.""Name"" as Relationship2,
p.""EmergencyContactMobileNumber2"" as ""EmergencyContactNo2"",c4.""CountryName"" as ""EmergencyContactCountryName2"",
Lov7.""Name"" as AssignmentTypeName, u.""PhotoId"" as PhotoName,d.""DepartmentName"" as ""OrganizationName""
from cms.""N_CoreHR_HRPerson"" as p
                         left join cms.""N_CoreHR_HRAssignment"" as assi on p.""Id"" = assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as LOV1 on p.""TitleId""=LOV1.""Id"" and LOV1.""IsDeleted""=false and LOV1.""CompanyId""='{_userContext.CompanyId}'
						 left join public.""LOV"" as LOV2 on p.""GenderId""=LOV2.""Id"" and LOV2.""IsDeleted""=false and LOV2.""CompanyId""='{_userContext.CompanyId}'
						  left join public.""LOV"" as LOV3 on p.""ReligionId""=LOV3.""Id"" and LOV3.""IsDeleted""=false and LOV3.""CompanyId""='{_userContext.CompanyId}'
						   left join public.""LOV"" as LOV4 on p.""MaritalStatusId""=LOV4.""Id"" and LOV4.""IsDeleted""=false and LOV4.""CompanyId""='{_userContext.CompanyId}'
							 left join public.""LOV"" as LOV5 on p.""EmergencyContact1RelationshipId""=LOV5.""Id"" and LOV5.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
							  left join public.""LOV"" as LOV6 on p.""EmergencyContact2RelationshipId""=LOV6.""Id"" and LOV6.""IsDeleted""=false and LOV6.""CompanyId""='{_userContext.CompanyId}'
                             left join public.""LOV"" as LOV7 on assi.""AssignmentTypeId""=LOV7.""Id"" and LOV7.""IsDeleted""=false and LOV7.""CompanyId""='{_userContext.CompanyId}'
                             left join cms.""N_CoreHR_HRNationality"" as n on n.""Id""= p.""NationalityId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRLocation"" as l on l.""Id""=assi.""LocationId"" and l.""IsDeleted""=false and l.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=assi.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRPosition"" as po on po.""Id""=assi.""PositionId"" and po.""IsDeleted""=false and po.""CompanyId""='{_userContext.CompanyId}'
			                    left join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=assi.""DepartmentId"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'

                             left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=assi.""AssignmentGradeId"" and g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRCountry"" as c1 on p.""PresentAddressCountryId""=c1.""Id"" and c1.""IsDeleted""=false and c1.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRCountry"" as c2 on p.""PermanentAddressCountryId""=c2.""Id"" and c2.""IsDeleted""=false and c2.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRCountry"" as c3 on p.""EmergencyContact1CountryId""=c3.""Id"" and c3.""IsDeleted""=false and c3.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRCountry"" as c4 on p.""EmergencyContact2CountryId""=c4.""Id"" and c4.""IsDeleted""=false and c4.""CompanyId""='{_userContext.CompanyId}'
							left join cms.""N_CoreHR_HRCountry"" as c on p.""ContactCountryId""=c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'
                             where p.""Id""='{personId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'";
                var queryProfile = await _queryProfile.ExecuteQuerySingle(query, null);
                return queryProfile;

            }
            return null;
        }




        public async Task TriggerEndOfService(ServiceTemplateViewModel viewModel)
        {
            var model = new EndOfServiceViewModel();
            var serviceTemplate = new ServiceTemplateViewModel();
            serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
            serviceTemplate.TemplateCode = "END_OF_SERVICE";
            var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);

            var template = await _templateBusiness.GetSingle(x => x.Id == viewModel.TemplateId);
            var subject = template.Code == "Resignation" ? "Resignation" : "Termination";

            service.ServiceSubject = subject;
            service.OwnerUserId = viewModel.OwnerUserId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.ActiveUserId = viewModel.ActiveUserId;
            service.DataAction = DataActionEnum.Create;
            service.ParentServiceId = viewModel.ServiceId;
            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            var val = await GetTerminationResignationDetailsbyid(viewModel.ServiceId, subject);
            model.Lastworkingdate = val.LastWorkingDate;
            model.ReasonId = val.Reason;
            model.Comment = val.Comment;
            model.Resignationterminationdate = val.ResignationTerminationDate;

            service.Json = JsonConvert.SerializeObject(model);

            var res = await _serviceBusiness.ManageService(service);
        }


        public async Task TriggerClearanceForm(ServiceTemplateViewModel viewModel)
        {
            var model = new ClearanceFormViewModel();
            var serviceTemplate = new ServiceTemplateViewModel();
            serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
            serviceTemplate.TemplateCode = "CLEARANCE_FORM";
            var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);

            var template = await _templateBusiness.GetSingle(x => x.Id == viewModel.TemplateId);
            var subject = template.Code == "Resignation" ? "Resignation" : "Termination";

            service.ServiceSubject = subject;
            service.OwnerUserId = viewModel.OwnerUserId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.ActiveUserId = viewModel.ActiveUserId;
            service.DataAction = DataActionEnum.Create;
            service.ParentServiceId = viewModel.ServiceId;
            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            model.clearanceNote = viewModel.ServiceNo;
            //var val = await GetTerminationResignationDetailsbyid(viewModel.ServiceId, subject);
            //model.Lastworkingdate = val.LastWorkingDate;
            //model.ReasonId = val.Reason;
            //model.Comment = val.Comment;
            //model.Resignationterminationdate = val.ResignationTerminationDate;

            service.Json = JsonConvert.SerializeObject(model);

            var res = await _serviceBusiness.ManageService(service);
        }

        public async Task<DateTime?> GetCurrentAnniversaryStartDateByUserId(string userId)
        {
            var cypher = $@"select a.* from public.""User"" as u 
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
where u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
";
            //var parameters = new Dictionary<string, object> { { "UserId", userId } };
            //var cypher = @"match (u:ADM_User{IsDeleted: 0,Id:{UserId}})-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot)<-[:R_AssignmentRoot]-(a:HRS_Assignment{IsLatest:true})
            //return a";
            var assignment = await _queryRepo1.ExecuteQuerySingle<AssignmentViewModel>(cypher, null);// ExecuteCypher<AssignmentViewModel>(cypher, parameters);
            if (assignment == null || assignment.DateOfJoin == null)
            {
                return null;
            }
            var anniversaryDate = new DateTime(DateTime.Today.Year, assignment.DateOfJoin.ToSafeDateTime().Month, assignment.DateOfJoin.ToSafeDateTime().Day);
            if (anniversaryDate > DateTime.Today)
            {
                anniversaryDate = anniversaryDate.AddYears(-1);
            }
            return anniversaryDate;
        }

        public async Task<ResignationTerminationViewModel> GetTerminationResignationDetailsbyid(string Noteid, string Name)
        {

            string Query = "";
            if (Name == "Termination")
            {
                Query = $@"select  SP.""LastWorkingDate"" as Lastworkingdate,  SP.""ReasonId"" as Reason, SP.""Comment"" as Comment, SP.""ResignationTerminationDate"" as Resignationterminationdate FROM  public.""NtsService"" NS
    inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	inner join  cms.""N_Separation_Termination"" as  SP on  N.""Id"" =SP.""NtsNoteId"" and SP.""IsDeleted""=false and SP.""CompanyId""='{_userContext.CompanyId}'
	where NS.""Id""='{Noteid}' and NS.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}' ";
            }
            else
            {
                Query = $@"select  SP.""LastWorkingDate"" as Lastworkingdate,  SP.""ReasonId"" as Reason, SP.""Remark"" as Comment, SP.""ResignationTerminationDate"" as Resignationterminationdate FROM  public.""NtsService"" NS
    inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	inner join  cms.""N_Separation_Resignation"" as  SP on  N.""Id"" =SP.""NtsNoteId"" and SP.""IsDeleted""=false and SP.""CompanyId""='{_userContext.CompanyId}'
	where NS.""Id""='{Noteid}' and NS.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}'";
            }

            var queryData = await _queryBusinessTrip.ExecuteQuerySingle<ResignationTerminationViewModel>(Query, null);
            return queryData;

        }
        public async Task<IList<TimePermisssionViewModel>> GetTimePermissionDetailsList(string EmpId)
        {
            //var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "Separation" && x.TemplateType == TemplateTypeEnum.Note);
            // var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id && x.Code == "SN_Resignation" || x.Code == "SN_Termination"); ;

            var selectQry = $@"SELECT Ns.""Id"",  RT.""Date""::TIMESTAMP::DATE  as Date, NS.""ServiceNo"" as ServiceNo,
       U.""Name"" as ServiceOwner , N.""NoteSubject"" as Name,RT.""Hours"",RT.""TimePermissionType"" as TimePermissionType,
    LOV.""Name"" as ServiceStatus,'TimePermissionBusiness' as pagename
    FROM  public.""NtsService"" NS
    inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	inner join  cms.""N_Time Permission_TimePermissionBusiness"" as  RT on  N.""Id"" =RT.""NtsNoteId""	and RT.""IsDeleted""=false and RT.""CompanyId""='{_userContext.CompanyId}'
    inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
    inner join public.""Template"" as t on t.""Id"" = N.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
   left join public.""User"" U on U.""Id""=N.""OwnerUserId"" and U.""IsDeleted""=false and U.""CompanyId""='{_userContext.CompanyId}'
    where N.""OwnerUserId""='{EmpId}' and NS.""IsDeleted""='false' and NS.""CompanyId""='{_userContext.CompanyId}'

    union

    SELECT Ns.""Id"", RT.""Date""::TIMESTAMP::DATE  as Date, NS.""ServiceNo"" as ServiceNo,
U.""Name"" as ServiceOwner , N.""NoteSubject"" as Name, RT.""Hours"", RT.""TimePermissionType"" as TimePermissionType,
    LOV.""Name"" as ServiceStatus,'TimePermissionPersonal' as pagename
    FROM  public.""NtsService"" NS
    inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id"" and N.""IsDeleted""='false' and N.""CompanyId""='{_userContext.CompanyId}'
	inner join  cms.""N_Time Permission_TimePermissionPersonal"" as  RT on  N.""Id"" =RT.""NtsNoteId""	and RT.""IsDeleted""='false' and RT.""CompanyId""='{_userContext.CompanyId}'
    inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and LOV.""IsDeleted""='false' and LOV.""CompanyId""='{_userContext.CompanyId}'
 inner join public.""Template"" as t on t.""Id"" = N.""TemplateId"" and t.""IsDeleted""='false' and t.""CompanyId""='{_userContext.CompanyId}'
  left join public.""User"" U on U.""Id""=N.""OwnerUserId"" and U.""IsDeleted""='false' and U.""CompanyId""='{_userContext.CompanyId}'
    where N.""OwnerUserId""='{EmpId}'  and NS.""IsDeleted""='false' and NS.""CompanyId""='{_userContext.CompanyId}'";


            var queryData = await _queryTimePermission.ExecuteQueryList<TimePermisssionViewModel>(selectQry, null);
            queryData = queryData.OrderByDescending(x => x.ServiceNo).ToList();
            //var list = new List<ResignationTerminationViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ResignationTerminationViewModel { Id = x.Id, ServiceNo = x.ServiceNo, Subject = x.Subject, ResignationTerminationDate = x.ResignationTerminationDate, LastWorkingDate = x.LastWorkingDate, ServiceStatus = x.ServiceStatus, DisplayName = x.DisplayName }).ToList();
            return queryData;
        }


        public async Task<JobDesriptionViewModel> GetHRJobDesciption(string JobId)
        {
            var query = $@"Select *, ""NtsNoteId"" as NoteId from cms.""N_CoreHR_JobDescription"" as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" 
            where T.""JobId"" = '{JobId}' and T.""IsDeleted"" = 'false'";

            var queryData = await _queryHRJobDescription.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<List<HRJobCriteriaViewModel>> GetJobCriteriabyParentID(string ParentNoteId)
        {
            var query = $@"Select T.*, ""NtsNoteId"" as NoteId,L.""Name"" as CriteriaTypeName from cms.""N_CoreHR_JobCriteria"" as T
            inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id""  and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}' 
left join public.""LOV"" as L on T.""CriteriaType""=L.""Id"" and L.""IsDeleted""=false and L.""CompanyId""='{_userContext.CompanyId}'
            where N.""ParentNoteId"" = '{ParentNoteId}' and T.""IsDeleted"" = 'false' and T.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryHRJobCriteriaViewModel.ExecuteQueryList(query, null);
            return queryData;
        }




        public async Task<List<HRJobCriteriaViewModel>> GetJobSkillabyParentID(string ParentNoteId)
        {
            var query = $@"Select T.*, ""NtsNoteId"" as NoteId,L.""Name"" as CriteriaTypeName from cms.""N_CoreHR_SKIlls"" as T 
inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id""  and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as L on T.""CriteriaType""=L.""Id"" and L.""IsDeleted""=false and L.""CompanyId""='{_userContext.CompanyId}'
            where N.""ParentNoteId"" = '{ParentNoteId}' and T.""IsDeleted"" = 'false'  and T.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryHRJobCriteriaViewModel.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<List<HRJobCriteriaViewModel>> GetJobOthernformationParentID(string ParentNoteId)
        {
            var query = $@"Select T.*, ""NtsNoteId"" as NoteId,L.""Name"" as CriteriaTypeName ,LT.""Name"" as LovTypeName,case when T.""ListOfValueTypeId"" is null then '' else T.""ListOfValueTypeId"" end as ""ListOfValueTypeId""  
from cms.""N_CoreHR_OtherInformation"" 
as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id""   and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as L on T.""CriteriaType""=L.""Id"" and L.""IsDeleted""=false and L.""CompanyId""='{_userContext.CompanyId}'
left join rec.""ListOfValue"" as LT on T.""ListOfValueTypeId""=LT.""Id"" and LT.""IsDeleted""=false and LT.""CompanyId""='{_userContext.CompanyId}'
where N.""ParentNoteId"" = '{ParentNoteId}' and T.""IsDeleted"" = 'false' and T.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryHRJobCriteriaViewModel.ExecuteQueryList(query, null);
            return queryData;
        }


        public void DeleteCriteria(String ParentNoteId, List<string> IDs = null)
        {
            var query = $@"update cms.""N_CoreHR_JobCriteria"" set ""IsDeleted""='True' where ""Id"" in (Select T.""Id""  from cms.""N_CoreHR_JobCriteria""
           as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}' 
            where N.""ParentNoteId"" = '{ParentNoteId}' and T.""IsDeleted"" = 'false' and T.""CompanyId""='{_userContext.CompanyId}' #IDs#)";
            if (IDs.IsNotNull() && IDs.Count > 0)
            {
                string pids = null;
                foreach (var i in IDs)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    string values = @" and T.""Id""  Not in (" + pids + ") ";
                    query = query.Replace("#IDs#", values);
                }
            }
            else { query = query.Replace("#IDs#", ""); }

            _queryHRJobCriteriaViewModel.ExecuteCommand(query, null);
        }

        public void DeleteSkill(String ParentNoteId, List<string> IDs = null)
        {
            var query = $@"update cms.""N_CoreHR_SKIlls"" set ""IsDeleted""='True' where ""Id"" in (Select T.""Id""  from cms.""N_CoreHR_SKIlls""
           as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where N.""ParentNoteId"" = '{ParentNoteId}' and T.""IsDeleted"" = 'false' and T.""CompanyId""='{_userContext.CompanyId}' #IDs#)";
            if (IDs.IsNotNull() && IDs.Count > 0)
            {
                string pids = null;
                foreach (var i in IDs)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    string values = @" and T.""Id""  Not in (" + pids + ") ";
                    query = query.Replace("#IDs#", values);
                }

            }
            else { query = query.Replace("#IDs#", ""); }

            _queryHRJobCriteriaViewModel.ExecuteCommand(query, null);
        }

        public void DeleteOtherInformation(String ParentNoteId, List<string> IDs = null)
        {
            var query = $@"update cms.""N_CoreHR_OtherInformation"" set ""IsDeleted""='True' where ""Id"" in (Select T.""Id""  from cms.""N_CoreHR_OtherInformation""
           as T inner join public.""NtsNote"" as N on T.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
            where N.""ParentNoteId"" = '{ParentNoteId}' and T.""IsDeleted"" = 'false' and T.""CompanyId""='{_userContext.CompanyId}'  #IDs#)";
            if (IDs.IsNotNull() && IDs.Count > 0)
            {
                string pids = null;
                foreach (var i in IDs)
                {
                    pids += $"'{i}',";
                }
                pids = pids.Trim(',');
                if (pids.IsNotNull())
                {
                    string values = @" and T.""Id""  Not in (" + pids + ") ";
                    query = query.Replace("#IDs#", values);
                }
            }
            else { query = query.Replace("#IDs#", ""); }

            _queryHRJobCriteriaViewModel.ExecuteCommand(query, null);
        }


        public async Task<PunchingViewModel> GetPunchingDetails(SigninSignoutTypeEnum Type, String UserId, DateTime Attendancedate, String Hours)
        {
            var query = $@"SELECT ""Duty3StartTime"", ""AttendanceDate"", ""Duty3FallsNextDay"", ""Duty1FallsNextDay"", ""Duty3EndTime"", ""Duty3Enabled"", ""Duty2Enabled"", ""Duty2EndTime"", ""OverrideComments"", ""PayrollPostedStatusId"", ""SystemOTHours"", ""AttendanceTypeId"", ""IsApproved"", ""EmployeeComments"", ""IsOverridden"", ""Duty1Enabled"", ""Duty2FallsNextDay"", ""ReferenceNodeId"", ""PayrollPostedDate"", ""OverrideUnderTimeHours"", ""Duty3FallsPreviousDay"", ""ApprovalStatus"", ""AttendanceLeaveTypeId"", ""IsCalculated"", ""Duty2FallsPreviousDay"", ""Duty1FallsPreviousDay"", ""UnderTimeHours"", ""OverrideAttendanceId"", ""Duty1StartTime"", ""Duty1EndTime"", ""Duty2StartTime"", ""TotalHours"", ""OverrideDeductionHours"", ""SystemDeductionHours"", ""UserId"", ""OverrideOTHours"", ""Id"", ""CompanyId"", ""VersionNo"", ""SequenceOrder"", ""CreatedDate"", ""LastUpdatedBy"", ""CreatedBy"", ""LastUpdatedDate"", ""IsDeleted"", ""Status"", ""NtsNoteId""

    FROM cms.""N_TAA_Attendance"" where ""IsDeleted"" = 'false' and ""CompanyId""='{_userContext.CompanyId}' and ""AttendanceDate""::TIMESTAMP::DATE = '{Attendancedate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and ""UserId""='{UserId}'";

            //if (PunchingTypeEnum.Checkin == Type)
            //{
            //    string val = $@"and ""Duty1StartTime""='{Hours}'";
            //    query = query.Replace("#DutyType#", val);
            //}
            //else if (PunchingTypeEnum.Checkout == Type)
            //{
            //    String val = $@"and ""Duty1EndTime""='{Hours}'";
            //    query = query.Replace("#DutyType#", val);
            //}


            var queryData = await _queryPunching.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public void Updatesigninsingnout(SigninSignoutTypeEnum type, string Id, string Hours)
        {

            var Query = "";

            if (SigninSignoutTypeEnum.Checkin == type)
            {
                Query = $@"update cms.""N_TAA_Attendance"" set ""Duty1StartTime""='{Hours}',""LastUpdatedDate""='{DateTime.Now}' where ""Id""='{Id}'";
            }
            else
            {
                Query = $@"update cms.""N_TAA_Attendance"" set ""Duty1EndTime""='{Hours}',""LastUpdatedDate""='{DateTime.Now}' where ""Id""='{Id}'";
            }
            _queryPunching.ExecuteCommand(Query, null);
        }


        public async Task<double> GetAirTicketCostByUser(string userId)
        {

            var cypher = string.Concat($@"select coalesce(case when g.""TravelClass""='Business' then n.""AverageBusinessTicketCost""  else n.""AverageEconomyTicketCost""  end,'0.0') as  TicketAmount
from public.""User"" as u 
join cms.""N_CoreHR_HRPerson"" as pr on pr.""UserId""=u.""Id"" and pr.""IsDeleted""=false and u.""Id""='{userId}' and pr.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as a on pr.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false and g.""CompanyId""='{_userContext.CompanyId}'
  left join cms.""N_CoreHR_HRNationality"" as n on n.""Id""= pr.""NationalityId"" and n.""CompanyId""='{_userContext.CompanyId}' and n.""IsDeleted""=false
where u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
");
            //var cypher = string.Concat(@"
            //match(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})-
            //[:R_User_PersonRoot]->(pr:HRS_PersonRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match (pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //where  a.EffectiveStartDate <= {ESD} <= a.EffectiveEndDate
            //match(a)-[:R_Assignment_GradeRoot]->(gr:HRS_GradeRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //match(gr)<-[:R_GradeRoot]-(g:HRS_Grade{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //where  g.EffectiveStartDate <= {ESD} <= g.EffectiveEndDate
            //match (pr)<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //where  p.EffectiveStartDate <= {ESD} <= p.EffectiveEndDate
            //match(p)-[:R_Person_Nationality]->(n:HRS_Nationality{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})            
            //return coalesce(case when g.TravelClass='Business' then n.AverageBusinessTicketCost else n.AverageEconomyTicketCost end,0.0) as  TicketAmount");

            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "Id", userId },
            //    { "ESD", DateTime.Now.ApplicationNow().Date },
            //    { "EED", DateTime.Now.ApplicationNow().Date }
            //};
            //return ExecuteCypherScalar<double>(cypher, prms);
            var result = await _queryRepo1.ExecuteScalar<string>(cypher, null);
            return Convert.ToDouble(result);
        }
        public async Task<IList<PositionViewModel>> GetPositionByJobId(string JobId)
        {
            var query = $@"select p.* from 
cms.""N_CoreHR_HRJob"" as j join 
cms.""N_CoreHR_HRPosition"" as p on p.""JobId""=j.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
where j.""NtsNoteId""='{JobId}' and j.""IsDeleted""=false and j.""CompanyId""='{_userContext.CompanyId}'";
            return await _queryRepo1.ExecuteQueryList<PositionViewModel>(query, null);
        }
        public async Task<IList<PositionViewModel>> GetPositionByDepartmentId(string DepartmentId)
        {
            var query = $@"select p.* from
cms.""N_CoreHR_HRDepartment"" as d join 
cms.""N_CoreHR_HRPosition"" as p on p.""DepartmentId""=d.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
where d.""NtsNoteId""='{DepartmentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}' ";
            return await _queryRepo1.ExecuteQueryList<PositionViewModel>(query, null);
        }
        public async Task<IdNameViewModel> GetDepartmentNameById(string DepartmentId)
        {
            var query = $@"select ""Id"",""DepartmentName"" as Name from cms.""N_CoreHR_HRDepartment"" 
       where ""Id""='{DepartmentId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";
            return await _queryRepo1.ExecuteQuerySingle<IdNameViewModel>(query, null);
        }

        public async Task<List<IdNameViewModel>> GetJobByDepartment(string DepartmentId)
        {
            var query = $@"select distinct j.""JobTitle"" as Name,j.""Id"" as Id
FROM cms.""N_CoreHR_HRDepartment"" as d
 join cms.""N_CoreHR_HRPosition"" as p on d.""Id"" = p.""DepartmentId"" and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}'
  join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = p.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_userContext.CompanyId}'
  where d.""Id""='{DepartmentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'";
            return await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
        }

        public async Task UpdatePositionName(string name, string id)
        {
            var query = $@"update cms.""N_CoreHR_HRPosition"" set ""PositionName""='{name}' where ""Id""='{id}'";
            await _queryRepo3.ExecuteCommand(query, null);
        }
        public async Task<List<string>> GetParentOrganizationReportingList(string orgId, List<string> ids)
        {
            // var asofDate = DateTime.Now.ApplicationNow().Date;
            //var parameters = new Dictionary<string, object>
            //{
            //    { "OrganizationId", orgId },
            //    { "ESD", DateTime.Now.Date },
            //    { "EED", DateTime.Now.Date},
            //    { "Status",StatusEnum.Active },
            //    { "CompanyId",CompanyId }
            //};
            //var match = @"match  (cr:HRS_OrganizationRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{OrganizationId}})
            //-[r:R_OrganizationRoot_ParentOrganizationRoot*0..]
            //->(pr:HRS_OrganizationRoot{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}}) 
            //WHERE all(rel in r WHERE rel.EffectiveStartDate<={ESD} and rel.EffectiveEndDate>={EED})
            //return pr.Id
            //    ";
            var match = $@"select ""ParentDepartmentId"" from cms.""N_CoreHR_HRDepartment"" where ""Id""='{orgId}'
and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'
                ";
            var result = await _queryRepo1.ExecuteScalar<string>(match, null);
            if (result != null)
            {
                ids.Add(result);
                await GetParentOrganizationReportingList(result, ids);
            }
            return ids;
        }
        public async Task<IList<NoteViewModel>> GetAnnouncements(List<string> orgList)
        {
            //            var noteCypher = @"match  (s:NTS_Note{IsDeleted:0})-[:R_Note_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //                               -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:'ANNOUNCEMENT',IsDeleted: 0,Status:'Active'})
            //                               match (s:NTS_Note{IsDeleted:0})-[:R_Note_TagTo{EnableBroadcast:true}]->(or:HRS_OrganizationRoot{IsDeleted:0})
            //                               optional match(s)-[:R_Attachment_Reference]-(a:GEN_Attachment)
            //optional match(f:GEN_File{Id:a.FileId})
            //                               optional match (s)-[:R_Note_Status_ListOfValue]->(LOV:GEN_ListOfValue)
            //                               where ((s.ExpiryDate is not null and s.StartDate<= {ESD} and s.ExpiryDate>= {EED}) or (s.ExpiryDate is null)) and LOV.Name = {Status} and
            //                               or.Id in [" + string.Join(",", orgList) + @"]
            //                               return s.Subject as Subject, s.Description as Description, s.StartDate as StartDate,s.CreatedDate as CreatedDate, s.Id as Id, or.Id as OrganizationId,a.FileId as FileId,f.FileName as FileName";

            //            var parameters = new Dictionary<string, object>();
            //            parameters.Add("Status", StatusEnum.Active.ToString());
            //            // parameters.Add("CompanyId", CompanyId);
            //            parameters.Add("ESD", DateTime.Now);
            //            parameters.Add("EED", DateTime.Now);
            //var roles = orgList.Split(",");
            var roleText = "";
            foreach (var i in orgList)
            {
                roleText += $"'{i}',";
            }
            roleText = roleText.Trim(',');
            var noteCypher = $@"select s.""NoteSubject"" as NoteSubject, s.""NoteDescription"" as NoteDescription,
s.""StartDate"" as StartDate,s.""CreatedDate"" as CreatedDate, s.""Id"" as Id, o.""Id"" as DepartmentId
--,a.""FileId"" as FileId,f.""FileName"" as FileName
from public.""NtsNote"" as s 
join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='ANNOUNCEMENT' and tc.""IsDeleted""=false and tc.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_ANNOUNCEMENT_GroupAnnouncement"" as ann on ann.""NtsNoteId""=s.""Id"" and ann.""IsDeleted""=false and ann.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=ann.""ReferenceId"" and o.""IsDeleted""=false and o.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as lov on lov.""Id""=s.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
where ((s.""ExpiryDate"" is not null and s.""StartDate""<= '{DateTime.Now}' and s.""ExpiryDate"">= '{DateTime.Now}') or (s.""ExpiryDate"" is null)) and lov.""Code"" = 'NOTE_STATUS_INPROGRESS' and
o.""Id"" in (" + string.Join(",", roleText) + $@") and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'

union

select s.""NoteSubject"" as NoteSubject, s.""NoteDescription"" as NoteDescription,
s.""StartDate"" as StartDate,s.""CreatedDate"" as CreatedDate, s.""Id"" as Id, o.""Id"" as DepartmentId
--,a.""FileId"" as FileId,f.""FileName"" as FileName
from public.""NtsNote"" as s 
join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='ANNOUNCEMENT' and tc.""IsDeleted""=false and tc.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_ANNOUNCEMENT_GroupAnnouncement"" as ann on ann.""NtsNoteId""=s.""Id"" and ann.""IsDeleted""=false and ann.""CompanyId""='{_userContext.CompanyId}'
join public.""Company"" as o on o.""Id""=ann.""ReferenceId"" and o.""IsDeleted""=false and o.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as lov on lov.""Id""=s.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
where ((s.""ExpiryDate"" is not null and s.""StartDate""<= '{DateTime.Now}' and s.""ExpiryDate"">= '{DateTime.Now}') or (s.""ExpiryDate"" is null)) and lov.""Code"" = 'NOTE_STATUS_INPROGRESS' and
o.""Id"" in (" + string.Join(",", roleText) + $@") and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
";



            var result = await _queryRepo2.ExecuteQueryList<NoteViewModel>(noteCypher, null);
            if (result.Count > 0)
            {
                result = result.OrderByDescending(x => x.CreatedDate).ToList();
            }
            return result;
        }
        public async Task<IList<string>> GetChildOrganizationReportingList(string orgId)
        {
            //var asofDate = DateTime.Now.ApplicationNow().Date;
            //var parameters = new Dictionary<string, object>
            //{
            //    { "OrganizationId", orgId },
            //    { "ESD", DateTime.Now.Date },
            //    { "EED", DateTime.Now.Date},
            //    { "Status",StatusEnum.Active },
            //    { "CompanyId",CompanyId }
            //};
            var match = $@"select d.""Id"" as Id
                            from cms.""N_CoreHR_HRDepartment"" as d
                            join cms.""N_CoreHR_HRCostCenter"" as c on d.""CostCenterId"" = c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'

                            left join cms.""N_CoreHR_HRDepartmentHierarchy"" as h on d.""Id"" = h.""DepartmentId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                            left join(
                            WITH RECURSIVE List AS(

                             WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""Id"" = '{orgId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                    where d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
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
                                where d.""Id"" = '{orgId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type,ns1.level+ 1
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department1 ns1 on h.""ParentDepartmentId"" = ns1.""Id""
                                where h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select ""Id"",""ParentId"",level from Department1
								
                            )
                            SELECT ""Id"",""ParentId"",level from List1 
                            )
                            t1 on d.""Id"" = t1.""Id""
                            where t1.""level"" <=100 and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                ";
            return await _queryRepo1.ExecuteScalarList<string>(match, null);

        }
        public async Task<IList<NoteViewModel>> GetGroupMessage(EndlessScrollingRequest searchModel)
        {

            var companyOrg = await GetCompanyOrganization(searchModel.UserId);
            var allChildOrg = await GetChildOrganizationReportingList(searchModel.OrgId);
            List<string> ids = new List<string>();
            ids.Add(searchModel.OrgId);
            var allParentOrg = await GetParentOrganizationReportingList(searchModel.OrgId, ids);
            var allOrgs = allChildOrg.Union(allParentOrg);
            //            var match = @"MATCH (n:NTS_Note{ IsDeleted: 0})-[:R_Note_TagTo{EnableBroadcast:true}]->(T:ADM_Team{ IsDeleted: 0})-[:R_Team_User]->(A:ADM_User{ Id:{ userId}, IsDeleted: 0}) 
            //match (n)-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: { Status} })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{Code:""GROUP_MESSAGE"", IsDeleted: 0,Status: { Status} })                
            //optional match(n)-[rs: R_Note_Status_ListOfValue]-> (s: GEN_ListOfValue)
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru:ADM_User{ IsDeleted: 0})
            //                optional match(n)-[:R_Note_Owner_User]->(ou:ADM_User{ IsDeleted: 0}) 
            //                optional match(n)<-[:R_User_Like_Note]-(lu:ADM_User{ IsDeleted: 0})
            //                optional match(n)<-[:R_User_Like_Note]-(mlu:ADM_User{Id:{ userId}, IsDeleted: 0})
            //                optional match(n)<-[:R_NoteComment_Note]-(ncom:NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId
            //                return n,gen.Id as FileId, gen.FileName as FileName, ou.Id as OwnerUserId,ou.UserName as SourceName,""To"" as Action,T.Name as TargetName,""Owner"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'TEAM' as SourcePost, 0 as OrganizationId
            //                union
            //                MATCH (n:NTS_Note{ IsDeleted: 0})-[rtse:R_Note_SharedTo_Team{SharingMode: 'Manual'}]->(T:ADM_Team{ IsDeleted: 0})-[:R_Team_User]->(A:ADM_User{ Id:{ userId}, IsDeleted: 0}) 
            //match (n)-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: { Status} })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{Code:""GROUP_MESSAGE"", IsDeleted: 0,Status: { Status} })                
            //optional match(n)-[rs: R_Note_Status_ListOfValue]-> (s: GEN_ListOfValue)
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru:ADM_User{ IsDeleted: 0})
            //                optional match(n)-[:R_Note_Owner_User]->(ou:ADM_User{ IsDeleted: 0}) 
            //                optional match(n)<-[:R_User_Like_Note]-(lu:ADM_User{ IsDeleted: 0})
            //                optional match(n)<-[:R_User_Like_Note]-(mlu:ADM_User{Id:{ userId}, IsDeleted: 0})
            //                optional match(n)<-[:R_NoteComment_Note]-(ncom:NTS_NoteComment{ IsDeleted: 0})
            //optional match(a1:ADM_User{Id : rtse.SharedByUserId})
            //                optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId

            // with n, ou, T, lu,mlu, ncom, a1, gen

            //                return n,gen.Id as FileId, gen.FileName as FileName, ou.Id as OwnerUserId,a1.UserName as SourceName,""Share With"" as Action,T.Name as TargetName,""Shared"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'SHARED' as SourcePost, 0 as OrganizationId

            //            union 
            //                match(n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: ""GROUP_MESSAGE"", IsDeleted: 0,Status: 'Active' })
            //                match(n) -[:R_Note_TagTo{ EnableBroadcast: true}]->(or: HRS_OrganizationRoot) < -[:R_OrganizationRoot] - (o: HRS_Organization)
            //                where o.EffectiveStartDate <={ ESD}
            //                and o.EffectiveEndDate >={ EED}
            //                and or.Id in [" + string.Join(",", allOrgs) + @"]
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru: ADM_User{ IsDeleted: 0})
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,""To"" as Action,o.Name as TargetName,""Owner"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'ORG' as SourcePost, or.Id as OrganizationId
            //                union 
            //                match(n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: ""GROUP_MESSAGE"", IsDeleted: 0,Status: 'Active' })
            //                match(n) -[:R_Note_TagTo{ EnableBroadcast: true}]->(or: HRS_OrganizationRoot) < -[:R_OrganizationRoot] - (o: HRS_Organization)
            //                where o.EffectiveStartDate <={ ESD}
            //                and o.EffectiveEndDate >={ EED}
            //                and or.Id = { companyId}
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru: ADM_User{ IsDeleted: 0})
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,""To"" as Action,o.Name as TargetName,""Owner"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'COMPANY' as SourcePost, 0 as OrganizationId
            //                union
            //                match(n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: 'GROUP_MESSAGE', IsDeleted: 0,Status: 'Active' })
            //                match(n)-[rru: R_Note_RequestedBy_User]-> (ru: ADM_User{ Id: {userId}, IsDeleted: 0})
            //                where n.ReferenceType = 'Self'
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: {userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,'To' as Action,'Self' as TargetName,'Owner' as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'SELF' as SourcePost, 0 as OrganizationId
            //                union
            //                MATCH (a:ADM_User {Id: { userId}})-[:R_User_Follow_Post]->(u:ADM_User) 
            //                match(n:NTS_Note)<-[rru: R_Note_RequestedBy_User]->(u) 
            //                match(n)-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: 'GROUP_MESSAGE', IsDeleted: 0,Status: 'Active' })
            //                where n.ReferenceType = 'Self'
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName, ou.Id as OwnerUserId,ou.UserName as SourceName,'To' as Action,'Self' as TargetName,'Owner' as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'FOLLOW' as SourcePost, 0 as OrganizationId";



            //            var parameters = new Dictionary<string, object>();
            //            parameters.Add("userId", searchModel.UserId);
            //            parameters.Add("Status", StatusEnum.Active.ToString());
            //            parameters.Add("orgId", searchModel.OrgId);
            //            parameters.Add("companyId", companyOrg?.Id);
            //            parameters.Add("ESD", DateTime.Now.Date);
            //            parameters.Add("EED", DateTime.Now.Date);
            //var match = $@"Select n.*,gen.""Id"" as FileId, gen.""FileName"" as FileName, ou.""Id"" as OwnerUserId,ou.""Name"" as SourceName,""To"" as Action,T.""Name"" as TargetName,""Owner"" as TemplateUserType,ou.""Name"" as OwnerDisplayName, count(distinct lu.*) as LikesUserCount,  count(distinct mlu.*) as ILiked, count(distinct ncom.*) as CommentsCount, 'TEAM' as SourcePost, '' as OrganizationId
            //from 
            //                union
            //                MATCH (n:NTS_Note{ IsDeleted: 0})-[rtse:R_Note_SharedTo_Team{SharingMode: 'Manual'}]->(T:ADM_Team{ IsDeleted: 0})-[:R_Team_User]->(A:ADM_User{ Id:{ userId}, IsDeleted: 0}) 
            //match (n)-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: { Status} })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{Code:""GROUP_MESSAGE"", IsDeleted: 0,Status: { Status} })                
            //optional match(n)-[rs: R_Note_Status_ListOfValue]-> (s: GEN_ListOfValue)
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru:ADM_User{ IsDeleted: 0})
            //                optional match(n)-[:R_Note_Owner_User]->(ou:ADM_User{ IsDeleted: 0}) 
            //                optional match(n)<-[:R_User_Like_Note]-(lu:ADM_User{ IsDeleted: 0})
            //                optional match(n)<-[:R_User_Like_Note]-(mlu:ADM_User{Id:{ userId}, IsDeleted: 0})
            //                optional match(n)<-[:R_NoteComment_Note]-(ncom:NTS_NoteComment{ IsDeleted: 0})
            //optional match(a1:ADM_User{Id: rtse.SharedByUserId})
            //                optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId

            // with n, ou, T, lu,mlu, ncom, a1, gen

            //                return n,gen.Id as FileId, gen.FileName as FileName, ou.Id as OwnerUserId,a1.UserName as SourceName,""Share With"" as Action,T.Name as TargetName,""Shared"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'SHARED' as SourcePost, 0 as OrganizationId

            //            union
            //                match(n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: ""GROUP_MESSAGE"", IsDeleted: 0,Status: 'Active' })
            //                match(n) -[:R_Note_TagTo{ EnableBroadcast: true}]->(or: HRS_OrganizationRoot) < -[:R_OrganizationRoot] - (o: HRS_Organization)
            //                where o.EffectiveStartDate <={ ESD}
            //and o.EffectiveEndDate >={ EED}
            //and or.Id in [" + string.Join(",", allOrgs) + @"]
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru: ADM_User{ IsDeleted: 0})
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)< -[rn: R_Attachment_Reference] - (att: GEN_Attachment) optional match(gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,""To"" as Action,o.Name as TargetName,""Owner"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'ORG' as SourcePost, or.Id as OrganizationId
            //                union
            //                match(n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: ""GROUP_MESSAGE"", IsDeleted: 0,Status: 'Active' })
            //                match(n) -[:R_Note_TagTo{ EnableBroadcast: true}]->(or: HRS_OrganizationRoot) < -[:R_OrganizationRoot] - (o: HRS_Organization)
            //                where o.EffectiveStartDate <={ ESD}
            //and o.EffectiveEndDate >={ EED}
            //and or.Id = { companyId}
            //optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru: ADM_User{ IsDeleted: 0})
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)< -[rn: R_Attachment_Reference] - (att: GEN_Attachment) optional match(gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,""To"" as Action,o.Name as TargetName,""Owner"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'COMPANY' as SourcePost, 0 as OrganizationId
            //                union
            //                match(n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: 'GROUP_MESSAGE', IsDeleted: 0,Status: 'Active' })
            //                match(n) -[rru: R_Note_RequestedBy_User]-> (ru: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                where n.ReferenceType = 'Self'
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)< -[rn: R_Attachment_Reference] - (att: GEN_Attachment) optional match(gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,'To' as Action,'Self' as TargetName,'Owner' as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'SELF' as SourcePost, 0 as OrganizationId
            //                union
            //                MATCH(a: ADM_User { Id: { userId} })-[:R_User_Follow_Post]->(u: ADM_User)
            //                match(n: NTS_Note) < -[rru: R_Note_RequestedBy_User]->(u)
            //                match(n) -[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{ Code: 'GROUP_MESSAGE', IsDeleted: 0,Status: 'Active' })
            //                where n.ReferenceType = 'Self'
            //                optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
            //                optional match(n)-[:R_Note_Owner_User]->(ou: ADM_User{ IsDeleted: 0}) 
            //                optional match(n)< -[:R_User_Like_Note] - (lu: ADM_User{ IsDeleted: 0})
            //                optional match(n)< -[:R_User_Like_Note] - (mlu: ADM_User{ Id: { userId}, IsDeleted: 0})
            //                optional match(n)< -[:R_NoteComment_Note] - (ncom: NTS_NoteComment{ IsDeleted: 0})
            //                optional match(n)< -[rn: R_Attachment_Reference] - (att: GEN_Attachment) optional match(gen:GEN_File) where gen.Id = att.FileId

            //                return n,gen.Id as FileId, gen.FileName as FileName, ou.Id as OwnerUserId,ou.UserName as SourceName,'To' as Action,'Self' as TargetName,'Owner' as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, 'FOLLOW' as SourcePost, 0 as OrganizationId";



            //            var parameters = new Dictionary<string, object>();
            //            parameters.Add("userId", searchModel.UserId);
            //            parameters.Add("Status", StatusEnum.Active.ToString());
            //            parameters.Add("orgId", searchModel.OrgId);
            //            parameters.Add("companyId", companyOrg?.Id);
            //            parameters.Add("ESD", DateTime.Now.Date);
            //            parameters.Add("EED", DateTime.Now.Date);
            var match = $@" select gm.*,n.""Id"" as NoteId,n.""NoteDescription"" as NoteDescription,ou.""Id"" as OwnerUserId,ou.""Name"" as SourceName ,ou.""PhotoId"" as UserPhotoId ,
case when gm.""ReferenceType""='5' then case when  d.""DepartmentName"" is null then c.""Name"" else d.""DepartmentName"" end else case when gm.""ReferenceTo"" is null then 'self' else t.""Name"" end end as TargetName
,case when gm.""ReferenceType""='5' then case when  d.""DepartmentName"" is null then 'COMPANY' else 'ORG' end else case when gm.""ReferenceTo"" is null then 'SELF' else t.""Name"" end end as SourcePost
,'To' as Action,f.""Id"" as FileId, f.""FileExtension""
                            from cms.""N_CoreHR_GroupMessage"" as gm
                            join public.""NtsNote"" as n on n.""Id""=gm.""NtsNoteId"" and n.""IsDeleted""=false #SEARCH#
                            left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false
left join public.""File"" as f on n.""Id""=f.""ReferenceTypeId"" and f.""IsDeleted""=false
left join public.""NtsNoteShared"" as ns on ns.""NtsNoteId""=n.""Id"" and ns.""IsDeleted""=false and ns.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as su on su.""Id""=ns.""SharedWithUserId"" and su.""IsDeleted""=false and su.""CompanyId""='{_userContext.CompanyId}'
and su.""Id""='{searchModel.UserId}'
left join public.""Team"" as t on gm.""ReferenceTo""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
left join public.""LegalEntity"" as l on gm.""ReferenceTo""=l.""Id"" and l.""IsDeleted""=false and l.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as d on gm.""ReferenceTo""=d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
left join public.""Company"" as c on gm.""ReferenceTo""=c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_userContext.CompanyId}'
                            where gm.""IsDeleted""=false and gm.""CompanyId""='{_userContext.CompanyId}'
                            ";

            var search = "";
            if (searchModel.SearchParam.IsNotNullAndNotEmpty())
            {
                search = $@" and lower(n.""NoteDescription"") like '%{searchModel.SearchParam}%' COLLATE ""tr-TR-x-icu"" ";
            }
            match = match.Replace("#SEARCH#", search);

            var list = await _queryRepo1.ExecuteQueryList<NoteViewModel>(match, null);


            IOrderedEnumerable<NoteViewModel> orderedList;
            if (searchModel.HomeType == "Self")
            {
                orderedList = list.Where(x => x.TargetName == "Self" && x.OwnerUserId == searchModel.UserId).OrderByDescending(x => x.CreatedDate);
            }
            else if (searchModel.HomeType == "UserGuide")
            {
                orderedList = list.Where(x => x.IsUserGuide == true).OrderByDescending(x => x.CreatedDate);
            }
            else if (searchModel.HomeType != null && searchModel.HomeType != "")
            {
                // var homeType = string.Concat(searchModel.HomeType.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
                orderedList = list.Where(x => x.TargetName.Replace(" ", "") == searchModel.HomeType).OrderByDescending(x => x.CreatedDate);
            }
            else
            {
                orderedList = list.Where(x => x.IsUserGuide == false || x.IsUserGuide == null).OrderByDescending(x => x.CreatedDate);

            }

            var fList = orderedList.Where(x => (x.IsPrivate == true && x.CreatedBy == searchModel.LoggendInUserId && x.SourcePost == "SELF") || (x.IsPrivate == null || x.IsPrivate == false) || (x.IsPrivate == true && x.DepartmentId == searchModel.OrgId && x.SourcePost == "ORG")).ToList();
            return fList;
        }

        public async Task<bool> ValidatePostMsgSequenceNo(PostMessageViewModel model)
        {
            var query = $@"Select * from cms.""N_CoreHR_GroupMessage"" where (""IsUserGuide""='true' or ""IsUserGuide""='True') and ""SequenceOrder""={model.SequenceOrder} and ""Id""!='{model.UdfNoteTableId}' and ""IsDeleted""=false ";
            var result = await _queryPostMsg.ExecuteQuerySingle(query, null);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<IdNameViewModel>> GetAllHierarchyUsers(string positionid, string Userid)
        {
            var Query = $@"select U.""Id"",U.""Name"" from  cms.""N_CoreHR_HRPosition"" as d
                            join cms.""N_CoreHR_HRJob"" as c on d.""JobId"" = c.""Id"" and c.""IsDeleted"" = false and c.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_CoreHR_HRDepartment"" as x on d.""DepartmentId"" = x.""Id"" and x.""IsDeleted"" = false and x.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRAssignment"" as a on d.""Id"" = a.""PositionId"" and a.""IsDeleted"" = false and a.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId"" = p.""Id"" and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_PositionHierarchy"" as h on d.""Id"" = h.""PositionId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'

                            where h.""ParentPositionId""='{positionid}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'  and U.""Id"" is not null
Union

select U.""Id"", U.""Name"" from cms.""N_CoreHR_HRPosition"" as d
                            join cms.""N_CoreHR_HRJob"" as c on d.""JobId"" = c.""Id"" and c.""IsDeleted"" = false and c.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_CoreHR_HRDepartment"" as x on d.""DepartmentId"" = x.""Id"" and x.""IsDeleted"" = false and x.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRAssignment"" as a on d.""Id"" = a.""PositionId"" and a.""IsDeleted"" = false and a.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId"" = p.""Id"" and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_PositionHierarchy"" as h on d.""Id"" = h.""PositionId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'

                            where d.""Id""='{positionid}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}' and u.""Id""='{Userid}'";

            var queryData = await _queryRepo1.ExecuteQueryList(Query, null);
            return queryData;

        }

        public async Task<IdNameViewModel> GetPositionID(string Userid)
        {
            var Query = $@"select d.""Id"" from  cms.""N_CoreHR_HRPosition"" as d
                            join cms.""N_CoreHR_HRJob"" as c on d.""JobId"" = c.""Id"" and c.""IsDeleted"" = false and c.""CompanyId""='{_userContext.CompanyId}'
                            join cms.""N_CoreHR_HRDepartment"" as x on d.""DepartmentId"" = x.""Id"" and x.""IsDeleted"" = false and x.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRAssignment"" as a on d.""Id"" = a.""PositionId"" and a.""IsDeleted"" = false and a.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId"" = p.""Id"" and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            left join cms.""N_CoreHR_PositionHierarchy"" as h on d.""Id"" = h.""PositionId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'

                            where u.""Id""='{Userid}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryRepo1.ExecuteQuerySingle(Query, null);
            return queryData;

        }


        public async Task<List<AttendanceViewModel>> GetAttendanceListByDate(string personId, DateTime? searchFromDate, DateTime? searchToDate, string UserId)
        {
            var selectQry = $@"SELECT  p.""PersonFullName"" as EmployeeName, P.""PersonNo"" as PersonNo, p.""SponsorshipNo"" as SponsorshipNo,
j.""JobTitle"" as JobName,x.""DepartmentName"" as OrganizationName ,A.""AttendanceDate""
as AttDate,A.""UserId"",A.""AttendanceDate""::TIMESTAMP::DATE as AttDate,

CONCAT(substring (cast(R.""Duty1StartTime"" as text),0,6), '_',  substring ( cast(R.""Duty1EndTime"" as text),0,6), '<br/>',substring ( cast(R.""Duty2StartTime"" as text),0,6), '_', substring (cast(R.""Duty2EndTime"" as text),0,6), '<br/>', CONCAT( substring (cast(R.""Duty3StartTime"" as text),0,6), '_', substring (cast(R.""Duty3EndTime"" as text),0,6))) as RosterText,


CONCAT(substring(cast(A.""Duty1StartTime"" as text), 0, 6), '_', substring(cast(A.""Duty1EndTime"" as text), 0, 6), '<br/>', substring(cast(A.""Duty2StartTime"" as text), 0, 6), '_', substring(cast(A.""Duty2EndTime"" as text), 0, 6), '<br/>', CONCAT(substring(cast(A.""Duty3StartTime"" as text), 0, 6), '_', substring(cast(A.""Duty3EndTime"" as text), 0, 6))) as ActualText,
A.""TotalHours"" as ActualHours, R.""TotalHours"" as RosterHours,
case when A.""SystemOTHours"" is not null THEN cast(A.""SystemOTHours"" as text) ELSE '0.0' END as SystemOTHoursText,
case when A.""OverrideOTHours"" is not null THEN Cast(A.""OverrideOTHours"" as text) ELSE '0.0' END as OverrideOTHoursText,
case when A.""SystemDeductionHours"" is not null THEN Cast(A.""SystemDeductionHours"" as text) ELSE '0.0' END as SystemDeductionHoursText,
case when A.""OverrideDeductionHours"" is not null THEN Cast(A.""OverrideDeductionHours"" as text) ELSE '0.0' END as OverrideDeductionHoursText




from cms.""N_TAA_Attendance"" as A left join cms.""N_TAA_RosterSchedule""
as R on A.""AttendanceDate""::TIMESTAMP::DATE = R.""RosterDate""::TIMESTAMP::DATE
and A.""UserId"" = R.""UserId""
left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId"" = A.""UserId""  and p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' 
 left join cms.""N_CoreHR_HRAssignment"" as ap  on ap.""EmployeeId"" = p.""Id""  and ap.""IsDeleted""=false and ap.""CompanyId""='{_userContext.CompanyId}'
 left join cms.""N_CoreHR_HRDepartment"" as x on ap.""DepartmentId"" = x.""Id""  and x.""IsDeleted""=false and x.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as j on ap.""JobId"" = j.""Id""  and j.""IsDeleted""=false and j.""CompanyId""='{_userContext.CompanyId}'
where  A.""IsDeleted""=false and A.""CompanyId""='{_userContext.CompanyId}' #UserId#   #fromTo#";


            string Qr = "", userids = "";

            if (personId.IsNotNullAndNotEmpty())
            {
                userids = $@"A.""UserId"" = '{personId}'";

            }
            else
            {
                var positionID = await GetPositionID(UserId);
                if (positionID != null)
                {


                    var GetList = await GetAllHierarchyUsers(positionID.Id, UserId);
                    string pids = null;
                    foreach (var item in GetList)
                    {



                        pids += $"'{item.Id}',";


                    }


                    if (pids.IsNotNull())
                    {
                        pids = pids.Trim(',');
                        userids = @" A.""UserId""  in (" + pids + ") ";
                        // query = query.Replace("#IDs#", values);
                    }

                }
            }

            if (searchFromDate.IsNotNull() && searchToDate.IsNotNull())
            {
                Qr = $@"and A.""AttendanceDate""::TIMESTAMP::DATE>='{searchFromDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and A.""AttendanceDate""::TIMESTAMP::DATE<= '{searchToDate.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE";

            }
            selectQry = selectQry.Replace("#UserId#", userids);
            selectQry = selectQry.Replace("#fromTo#", Qr);
            var queryData = await _queryAttendance.ExecuteQueryList<AttendanceViewModel>(selectQry, null);

            if (queryData.Count > 0)
            {

                var allLeaves = await _sp.GetService<ILeaveBalanceSheetBusiness>().GetAllLeaves(searchFromDate.IsNotNull() ? Convert.ToDateTime(searchFromDate) : DateTime.Now, searchFromDate.IsNotNull() ? Convert.ToDateTime(searchToDate) : DateTime.Now);
                var holidays = await _sp.GetService<ILeaveBalanceSheetBusiness>().GetHolidays(searchFromDate.IsNotNull() ? Convert.ToDateTime(searchFromDate) : DateTime.Now, searchFromDate.IsNotNull() ? Convert.ToDateTime(searchToDate) : DateTime.Now);
                //var timePersmissions = sb.GetAllTimePermissionList();
                var timePersmissions = await _attendanceBusiness.GetReportTimePermissionList();
                var businessTrips = await _attendanceBusiness.GetReportBusinessTripList();



                foreach (var item in queryData)
                {


                    if (item.RosterText.ToLower() == "dayoff")
                    {
                        //  item.LeaveTypeCode = "DAY_OFF";
                        item.SystemAttendanceText = item.RosterText;
                    }
                    else
                    {



                        var uhs = holidays.Where(u => u.UserId == item.UserId && u.FromDate == searchFromDate && u.ToDate == searchToDate).FirstOrDefault();


                        var utp = timePersmissions.Where(x => x.UserId == item.UserId && x.FromDate == Convert.ToDateTime(item.AttDate)).FirstOrDefault();
                        //timePersmissions.FirstOrDefault(x => x. == item.UserId && x.Date == item.AttDate);
                        var uls = allLeaves.Where(u => u.UserId == item.UserId && u.StartDate <= searchFromDate && u.EndDate >= searchToDate).FirstOrDefault();
                        var ubt = businessTrips.Where(u => u.UserId == item.UserId && u.StartDate <= searchFromDate && u.EndDate >= searchToDate).FirstOrDefault();


                        // await allLeaves.FirstOrDefault(x => x.UserId == item.UserId && x.StartDate <= item.AttDate && x.EndDate >= item.AttDate);

                        item.SystemOTHours = item.SystemOTHours ?? new TimeSpan();
                        item.PermittedOTHoursText = new TimeSpan();
                        item.CalculatedOTHoursText = new TimeSpan();

                        item.SystemDeductionHours = item.SystemDeductionHours ?? new TimeSpan();
                        item.PermittedDeductionHoursText = new TimeSpan();

                        if (utp != null && utp.Hours.IsNotNullAndNotEmpty())
                        {
                            var hrs = utp.Hours.Split('.');
                            var hours = 0;
                            var minutes = 0;
                            if (hrs.Length > 0)
                            {
                                hours = hrs[0].ToSafeInt();
                            }
                            if (hrs.Length > 1)
                            {
                                minutes = hrs[1].ToSafeInt();
                            }
                            item.PermittedDeductionHoursText = new TimeSpan(hours, minutes, 0);
                        }
                        var calcDeddHours = item.SystemDeductionHours - item.PermittedDeductionHoursText;
                        item.PermittedDeductionHoursText = (TimeSpan)item.SystemDeductionHours;
                        if (calcDeddHours.HasValue)
                        {
                            if (calcDeddHours.Value.TotalMinutes >= 0)
                            {
                                item.CalculatedDeductionHoursText = calcDeddHours.IsNotNull() ? (TimeSpan)calcDeddHours : null;
                            }
                            else
                            {
                                item.CalculatedDeductionHoursText = TimeSpan.Zero;
                            }

                        }

                        if (uhs != null)
                        {
                            item.LeaveTypeCode = "HOLIDAY";
                            item.SystemAttendanceText = uhs.HolidayName;
                        }
                        else if (utp != null)
                        {
                            item.LeaveTypeCode = utp.LeaveTypeCode;

                            if (utp.TemplateAction == "Complete")
                            {
                                var reason = "";
                                if (utp.TimePermissionType.HasValue)
                                {
                                    reason = utp.TimePermissionType.Value.ToString();
                                }
                                else
                                {
                                    reason = utp.TimePermissionReason;
                                }
                                if (reason.IsNullOrEmpty())
                                {
                                    if (item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_AH")
                                    {
                                        reason = "Permission - Personal";
                                    }
                                    else if (item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_AH")
                                    {
                                        reason = "Permission - Business";
                                    }
                                }
                                item.SystemAttendanceText = string.Concat("Approved ", reason);
                            }
                            else
                            {
                                var reason = "";
                                if (utp.TimePermissionType.HasValue)
                                {
                                    reason = utp.TimePermissionType.Value.ToString();
                                }
                                else
                                {
                                    reason = utp.TimePermissionReason;
                                }
                                if (reason.IsNullOrEmpty())
                                {
                                    if (item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_PERSONAL_AH")
                                    {
                                        reason = "Permission - Personal";
                                    }
                                    else if (item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_UAE" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_KSA" || item.LeaveTypeCode == "TIME_PERMISSION_BUSINESS_AH")
                                    {
                                        reason = "Permission - Business";
                                    }
                                }
                                item.SystemAttendanceText = string.Concat("Pending ", reason);
                            }

                        }
                        else if (uls != null)
                        {
                            item.LeaveTypeCode = uls.LeaveTypeCode;
                            if (uls.TemplateAction == "Complete")
                            {
                                item.SystemAttendanceText = string.Concat("Approved ", uls.LeaveType);
                            }
                            else
                            {
                                item.SystemAttendanceText = string.Concat("Pending ", uls.LeaveType);
                            }

                        }
                        else if (ubt != null)
                        {
                            item.LeaveTypeCode = ubt.LeaveTypeCode;
                            if (ubt.TemplateAction == "Complete")
                            {
                                item.SystemAttendanceText = string.Concat("Approved ", ubt.Name);
                            }
                            else
                            {
                                item.SystemAttendanceText = string.Concat("Pending ", ubt.Name);
                            }


                        }
                        else
                        {
                            if (item.CalculatedDeductionHoursText.HasValue && item.CalculatedDeductionHoursText.Value.TotalMinutes > 0)
                            {
                                item.LeaveTypeCode = "SHORT_TIME";
                                item.SystemAttendanceText = "Late In/Early Out";
                            }
                            else if (item.Duty1StartTime.HasValue && item.Duty1EndTime.HasValue && item.Duty1StartTime.Value == item.Duty1EndTime.Value)
                            {
                                item.LeaveTypeCode = "LOG_MISSING";
                                item.SystemAttendanceText = "Sign In/Out Missing";
                            }
                            //if (item.SystemAttendance == null || item.SystemAttendance == AttendanceTypeEnum.Absent)
                            //{
                            //    var leave = allLeaves.FirstOrDefault(x => x.UserId == item.UserId && x.StartDate <= item.AttDate && x.EndDate >= item.AttDate);
                            //    if (leave != null)
                            //    {
                            //        item.LeaveTypeCode = leave.LeaveTypeCode;

                            //        if (leave.TemplateAction == "Complete")
                            //        {
                            //            item.SystemAttendanceText = string.Concat("Approved ", leave.LeaveType);
                            //        }
                            //        else
                            //        {
                            //            item.SystemAttendanceText = string.Concat("Pending ", leave.LeaveType);
                            //        }

                            //    }
                            //    else
                            //    {
                            //        item.LeaveTypeCode = item.SystemAttendance.ToString();
                            //        item.SystemAttendanceText = item.SystemAttendance.ToString();
                            //    }
                            //}
                            else
                            {
                                item.LeaveTypeCode = item.SystemAttendance.ToString();
                                item.SystemAttendanceText = item.SystemAttendance == AttendanceTypeEnum.Present ? "" : item.SystemAttendance.ToString();
                            }
                        }
                    }


                }
            }
            return queryData;

        }


        public async Task<List<RemoteSignInSignOutViewModel>> GetRemotesigninsignoutDetails(string Id)
        {
            var query = "";
            query = $@"SELECT m.""Id"", NS.""ServiceNo"",NS.""Id"" as ServiceId,U.""Name"" as ServiceOwner, substring(M.""SignInTime"",1,16) as signinTime, 
                        substring(M.""SignOutTime"",1,16) as signOutTime, M.""locationName"" as LocationName, LOVS.""Name"" as Status
                        FROM public.""NtsService"" NS
                        inner join public.""NtsNote"" N on Ns.""UdfNoteId""=N.""Id""  and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	                    inner join  cms.""N_Time Permission_Remotesigninsignout"" m on  N.""Id"" =m.""NtsNoteId""  and m.""IsDeleted""=false and m.""CompanyId""='{_userContext.CompanyId}'
                        inner join public.""LOV"" as LOVS on NS.""ServiceStatusId""=LOVS.""Id""  and LOVS.""IsDeleted""=false and LOVS.""CompanyId""='{_userContext.CompanyId}'
                        inner join public.""User"" as U on U.""Id""=N.""OwnerUserId""  and U.""IsDeleted""=false and U.""CompanyId""='{_userContext.CompanyId}'
                        where N.""OwnerUserId""='{Id}'  and NS.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}' order by NS.""CreatedDate"" desc ";

            var queryData = await _queryRemote.ExecuteQueryList<RemoteSignInSignOutViewModel>(query, null);

            return queryData;
        }

        public async Task<IList<NoteViewModel>> GetOrgGroupMessage(EndlessScrollingRequest searchModel)
        {
            if (searchModel.OrgId.IsNotNullAndNotEmpty())
            {
                var allChildOrg = await GetChildOrganizationReportingList(searchModel.OrgId);
                List<string> ids = new List<string>();
                ids.Add(searchModel.OrgId);
                var allParentOrg = await GetParentOrganizationReportingList(searchModel.OrgId, ids);
                var allOrgs = allChildOrg.Union(allParentOrg);

                //var match = @"match (n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status:'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{Code:""GROUP_MESSAGE"", IsDeleted: 0,Status: 'Active' })
                //match(n) -[:R_Note_TagTo{EnableBroadcast:true}]->(or: HRS_OrganizationRoot) < -[:R_OrganizationRoot] - (o: HRS_Organization)
                //where o.EffectiveStartDate <={ ESD} and o.EffectiveEndDate >={ EED} and or.Id in [" + string.Join(",", allOrgs) + @"]
                //optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
                //optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru:ADM_User{ IsDeleted: 0})
                //optional match(n)-[:R_Note_Owner_User]->(ou:ADM_User{ IsDeleted: 0}) 
                //optional match(n)<-[:R_User_Like_Note]-(lu:ADM_User{ IsDeleted: 0})
                //optional match(n)<-[:R_User_Like_Note]-(mlu:ADM_User{Id:{ userId}, IsDeleted: 0})
                //optional match(n)<-[:R_NoteComment_Note]-(ncom:NTS_NoteComment{ IsDeleted: 0})
                //optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId
                //return n,gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,""To"" as Action,o.Name as TargetName,""Owner"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount, or.Id as OrganizationId
                //";
                //var parameters = new Dictionary<string, object>();
                //parameters.Add("userId", searchModel.UserId);
                //parameters.Add("Status", StatusEnum.Active.ToString());
                //parameters.Add("orgId", searchModel.OrgId);
                //parameters.Add("ESD", DateTime.Now.Date);
                //parameters.Add("EED", DateTime.Now.Date);


                //var orderedList = _repository.ExecuteCypherList<NoteViewModel>(match, parameters);
                var roleText = "";
                foreach (var i in allOrgs)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');
                var match = $@" select gm.*,n.""Id"" as NoteId,n.""NoteDescription"" as NoteDescription,gm.""ReferenceTo"" as DepartmentId,ou.""Id"" as OwnerUserId,ou.""Name"" as SourceName,ou.""PhotoId"" as UserPhotoId ,d.""DepartmentName"" as TargetName,'To' as Action
from cms.""N_CoreHR_GroupMessage"" as gm
                            join public.""NtsNote"" as n on n.""Id""=gm.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNoteShared"" as ns on ns.""NtsNoteId""=n.""Id"" and ns.""IsDeleted""=false and ns.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as su on su.""Id""=ns.""SharedWithUserId"" and su.""IsDeleted""=false and su.""CompanyId""='{_userContext.CompanyId}' 
and su.""Id""='{searchModel.UserId}'
left join cms.""N_CoreHR_HRDepartment"" as d on gm.""ReferenceTo""=d.""Id""
                            where gm.""IsDeleted""=false and NS.""CompanyId""='{_userContext.CompanyId}' and gm.""ReferenceType""='{(int)((NoteReferenceTypeEnum)Enum.Parse(typeof(NoteReferenceTypeEnum), "Organization"))}' and  gm.""ReferenceTo"" in (" + string.Join(",", roleText) + $@")
                            ";
                var orderedList = await _queryRepo1.ExecuteQueryList<NoteViewModel>(match, null);
                if (orderedList.Count > 0)
                {
                    orderedList = orderedList.OrderByDescending(x => x.CreatedDate).ToList();
                }
                var fList = orderedList.Where(x => (x.IsPrivate == null || x.IsPrivate == false) || (x.IsPrivate == true && x.DepartmentId == searchModel.OrgId));
                return fList.ToList();
            }
            else
            {
                return new List<NoteViewModel>();
            }


        }
        public async Task<IList<NoteViewModel>> GetCompanyGroupMessage(EndlessScrollingRequest searchModel)
        {
            if (searchModel.OrgId.IsNotNullAndNotEmpty())
            {
                //var match = @"match (n: NTS_Note{ IsDeleted: 0})-[rt: R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status:'Active' })-[:R_TemplateRoot]->(tr: NTS_TemplateMaster{Code:""GROUP_MESSAGE"", IsDeleted: 0,Status: 'Active' })
                //match(n) -[:R_Note_TagTo{EnableBroadcast:true}]->(or: HRS_OrganizationRoot) < -[:R_OrganizationRoot] - (o: HRS_Organization)
                //where o.EffectiveStartDate <={ ESD} and o.EffectiveEndDate >={ EED} and or.Id = {orgId}
                //optional match(n)-[rc: R_Note_NtsCategory]-> (c: NTS_NtsCategory{ IsDeleted: 0}) 
                //optional match(n)-[rru: R_Note_RequestedBy_User]-> (ru:ADM_User{ IsDeleted: 0})
                //optional match(n)-[:R_Note_Owner_User]->(ou:ADM_User{ IsDeleted: 0}) 
                //optional match(n)<-[:R_User_Like_Note]-(lu:ADM_User{ IsDeleted: 0})
                //optional match(n)<-[:R_User_Like_Note]-(mlu:ADM_User{Id:{ userId}, IsDeleted: 0})
                //optional match(n)<-[:R_NoteComment_Note]-(ncom:NTS_NoteComment{ IsDeleted: 0})
                //optional match(n)<-[rn:R_Attachment_Reference]-(att:GEN_Attachment) optional match (gen:GEN_File) where gen.Id = att.FileId
                //return n, gen.Id as FileId, gen.FileName as FileName,ou.Id as OwnerUserId,ou.UserName as SourceName,""To"" as Action,o.Name as TargetName,""Owner"" as TemplateUserType,ou.UserName as OwnerDisplayName, count(distinct lu) as LikesUserCount,  count(distinct mlu) as ILiked, count(distinct ncom) as CommentsCount
                //";
                //var parameters = new Dictionary<string, object>();
                //parameters.Add("userId", searchModel.UserId);
                //parameters.Add("Status", StatusEnum.Active.ToString());
                //parameters.Add("orgId", searchModel.OrgId);
                //parameters.Add("ESD", DateTime.Now.Date);
                //parameters.Add("EED", DateTime.Now.Date);

                //return _repository.ExecuteCypherList<NoteViewModel>(match, parameters);

                var match = $@" select gm.*,n.""Id"" as NoteId,n.""NoteDescription"" as NoteDescription,gm.""ReferenceTo"" as DepartmentId,ou.""Id"" as OwnerUserId,ou.""Name"" as SourceName,ou.""PhotoId"" as UserPhotoId ,c.""Name"" as TargetName,'To' as Action
from cms.""N_CoreHR_GroupMessage"" as gm
                            join public.""NtsNote"" as n on n.""Id""=gm.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNoteShared"" as ns on ns.""NtsNoteId""=n.""Id"" and ns.""IsDeleted""=false and ns.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as su on su.""Id""=ns.""SharedWithUserId"" and su.""IsDeleted""=false and su.""CompanyId""='{_userContext.CompanyId}'
and su.""Id""='{searchModel.UserId}'
left join public.""Company"" as c on gm.""ReferenceTo""=c.""Id""
                            where gm.""IsDeleted""=false and gm.""CompanyId""='{_userContext.CompanyId}' and gm.""ReferenceType""='{(int)((NoteReferenceTypeEnum)Enum.Parse(typeof(NoteReferenceTypeEnum), "Organization"))}' 
                             and  gm.""ReferenceTo""='{searchModel.OrgId}' 
                            ";
                var orderedList = await _queryRepo1.ExecuteQueryList<NoteViewModel>(match, null);

                var fList = orderedList.Where(x => (x.IsPrivate == null || x.IsPrivate == false) || (x.IsPrivate == true && x.DepartmentId == searchModel.OrgId));
                return fList.ToList();
            }
            else
            {
                return new List<NoteViewModel>();
            }
        }
        public async Task<string> GetAnnouncementTemplateMasterId()
        {
            //var noteCypher = @"match (t:NTS_Template{IsDeleted:0,Status:'Active'})-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //                   -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:'ANNOUNCEMENT',IsDeleted: 0,Status:'Active'})
            //                   return tr.Id";
            var noteCypher = @"select ""Id"" from public.""Template"" where ""Code""='GroupAnnouncement'";

            var result = await _queryRepo1.ExecuteScalar<string>(noteCypher, null);

            return result;
        }
        public async Task<AnnouncementViewModel> GetAnnouncementByNoteId(string Id)
        {
            var noteCypher = $@"select * from cms.""N_ANNOUNCEMENT_GroupAnnouncement"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteQuerySingle<AnnouncementViewModel>(noteCypher, null);

            return result;
        }
        public async Task<PostMessageViewModel> GetGroupMessageByNoteId(string Id)
        {
            var noteCypher = $@"select * from cms.""N_CoreHR_GroupMessage"" where ""NtsNoteId""='{Id}'  and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteQuerySingle<PostMessageViewModel>(noteCypher, null);

            return result;
        }

        public async Task<long> GetLikeCount(string ParentNoteId)
        {
            var noteCypher = $@"select Count(n.*) from  public.""NtsNote"" as n 
where n.""ParentNoteId""='{ParentNoteId}' and n.""IsDeleted""=false  and n.""CompanyId""='{_userContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteScalar<long>(noteCypher, null);

            return result;
        }
        public async Task<long> GetILikeCount(string ParentNoteId, string userId)
        {
            var noteCypher = $@"select Count(n.*) from  public.""NtsNote"" as n 
where n.""ParentNoteId""='{ParentNoteId}' and n.""OwnerUserId""='{userId}' and n.""IsDeleted""=false and n.""CompanyId""='{_userContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteScalar<long>(noteCypher, null);

            return result;
        }
        public async Task DeleteGroupPost(string Id)
        {
            var noteCypher = $@"Update  cms.""N_CoreHR_GroupMessage"" set ""IsDeleted""=true where ""NtsNoteId""='{Id}'";

            var result = await _queryRepo1.ExecuteScalar<bool?>(noteCypher, null);


        }
        public async Task UnlikePost(string Id, string userId)
        {
            var noteCypher = $@"Update  public.""NtsNote"" set ""IsDeleted""=true  where ""ParentNoteId""='{Id}' and ""OwnerUserId""='{userId}'";

            var result = await _queryRepo1.ExecuteScalar<bool?>(noteCypher, null);


        }
        //        public async Task<IList<NoteViewModel>> GetNoteSummaryDetail(NoteSearchViewModel searchModel)
        //        {
        //            var list = new List<NoteViewModel>();
        //            // var userId = Convert.ToInt64(System.Web.HttpContext.Current.Session[Constant.SessionVariable.UserId]);
        //            var referenceNode = "";
        //            var referenceFields = "";
        //            var broadCast = "";
        //            switch (searchModel.TagToType.Value)
        //            {
        //                case NoteReferenceTypeEnum.Self:
        //                    break;
        //                case NoteReferenceTypeEnum.User:
        //                    break;
        ////                case NoteReferenceTypeEnum.Person:
        ////                    referenceNode = @"optional match(n)-[:R_Note_TagTo{ReferenceType:'Person'}]->(refr:HRS_PersonRoot)<-[:R_PersonRoot]
        ////                                -(ref:HRS_Person{IsLatest:true,IsDeleted:0})";
        ////                    referenceFields = "'Person' as ReferenceType,refr.Id as ReferenceTo,ref.FirstName as ReferenceToDisplayName";
        ////                    broadCast = @"union match (refr:HRS_PersonRoot{Id:{TagTo}})
        ////                    match (refr)<-[:R_Note_TagTo{ReferenceType:'Person',EnableBroadcast:true}]-(n:NTS_Note{IsDeleted:0})
        ////                    match (refr)<-[:R_PersonRoot]-(ref:HRS_Person{IsLatest:true})
        ////                    match (n)-[rt: R_Note_Template]->(t: NTS_Template{IsDeleted:0,Status:{Status}})
        ////                    -[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        ////                    -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}})   
        ////                    match (n)-[rnou:R_Note_Owner_User]->(u:ADM_User)
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0}) 
        ////                    return n,'Broadcast' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        ////                    ,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        ////                    ,tr.Id as TemplateMasterId,#REF_FIELDS#";
        ////                    break;
        ////                case NoteReferenceTypeEnum.Position:
        ////                    //referenceNode = @"optional match(n)-[:R_Note_TagTo{ReferenceType:'Position'}]->(refr:HRS_PositionRoot)<-[:R_PositionRoot]
        ////                    //            -(ref:HRS_Position{IsLatest:true,IsDeleted:0})";
        ////                    referenceNode = $@"left join cms.""N_CoreHR_GroupMessage"" as g 
        ////left join cms.""N_CoreHR_HRPosition"" as ref on ref.""Id""=g.""ReferenceTo""";
        ////                    referenceFields = "'Position' as ReferenceType,refr.Id as ReferenceTo,ref.Name as ReferenceToDisplayName";
        ////                    broadCast = @"union match (refr:HRS_PositionRoot{Id:{TagTo}})
        ////                    match (refr)<-[:R_Note_TagTo{ReferenceType:'Position',EnableBroadcast:true}]-(n:NTS_Note{IsDeleted:0})
        ////                    match (refr)<-[:R_PositionRoot]-(ref:HRS_Position{IsLatest:true})
        ////                    match (n)-[rt: R_Note_Template]->(t: NTS_Template{IsDeleted:0,Status:{Status}})
        ////                    -[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        ////                    -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}})   
        ////                    match (n)-[rnou:R_Note_Owner_User]->(u:ADM_User)
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0}) 
        ////                    return n,'Broadcast' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        ////                    ,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        ////                    ,tr.Id as TemplateMasterId,#REF_FIELDS#";
        ////                    break;
        ////                case NoteReferenceTypeEnum.Job:
        ////                    //referenceNode = @"optional match(n)-[:R_Note_TagTo{ReferenceType:'Job'}]->(refr:HRS_JobRoot)<-[:R_JobRoot]-
        ////                    //            (ref:HRS_Job{IsLatest:true,IsDeleted:0})";
        ////                    referenceNode = $@"left join cms.""N_CoreHR_GroupMessage"" as g 
        ////left join cms.""N_CoreHR_HRJob"" as ref on ref.""Id""=g.""ReferenceTo""";
        ////                    referenceFields = @"'Job' as ReferenceType,g.""ReferenceTo"" as ReferenceTo,ref.""JObTitle"" as ReferenceToDisplayName";
        ////                    broadCast = @"union match (refr:HRS_JobRoot{Id:{TagTo}})
        ////                    match (refr)<-[:R_Note_TagTo{ReferenceType:'Job',EnableBroadcast:true}]-(n:NTS_Note{IsDeleted:0})
        ////                    match (refr)<-[:R_JobRoot]-(ref:HRS_Job{IsLatest:true})
        ////                    match (n)-[rt: R_Note_Template]->(t: NTS_Template{IsDeleted:0,Status:{Status}})
        ////                    -[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        ////                    -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}})   
        ////                    match (n)-[rnou:R_Note_Owner_User]->(u:ADM_User)
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        ////                    optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0}) 
        ////                    return n,'Broadcast' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        ////                    ,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        ////                    ,tr.Id as TemplateMasterId,#REF_FIELDS#";
        ////                    break;
        //                case NoteReferenceTypeEnum.Organization:
        //                    //referenceNode = @"optional match(n)-[:R_Note_TagTo{ReferenceType:'Organization'}]->(refr:HRS_OrganizationRoot)
        //                    //                <-[:R_OrganizationRoot]-(ref:HRS_Organization{IsLatest:true,IsDeleted:0})";
        //                    referenceNode = $@"left join cms.""N_CoreHR_GroupMessage"" as g 
        //left join cms.""N_CoreHR_HRDepartment"" as ref on ref.""Id""=g.""ReferenceTo""";
        //                    referenceFields = @"'Organization' as ReferenceType,g.""ReferenceTo"" as ReferenceTo,ref.""DepartmentName"" as ReferenceToDisplayName";
        //                    broadCast = @"union 
        //                    match (refr:HRS_OrganizationRoot{Id:{TagTo}})
        //                    match (refr)<-[:R_Note_TagTo{ReferenceType:'Organization',EnableBroadcast:true}]-(n:NTS_Note{IsDeleted:0})
        //                    match (refr)<-[:R_OrganizationRoot]-(ref:HRS_Organization{IsLatest:true})
        //                    match (n)-[rt: R_Note_Template]->(t: NTS_Template{IsDeleted:0,Status:{Status}})
        //                    -[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        //                    -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}})   
        //                    match (n)-[rnou:R_Note_Owner_User]->(u:ADM_User)
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0}) 
        //                    return n,'Broadcast' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        //                    ,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        //                    ,tr.Id as TemplateMasterId,#REF_FIELDS#
        //                    union
        //                    match (arn:HRS_OrganizationRoot{Id:{TagTo}})                   
        //                    match (arn)<-[:R_OrganizationRoot_ParentOrganizationRoot*1..]-(crn:HRS_OrganizationRoot)                   
        //                    with collect(crn.Id) as o
        //                    match (refr:HRS_OrganizationRoot)
        //                    where refr.Id in o
        //                    match (refr)<-[:R_Note_TagTo{ReferenceType:'Organization',EnableBroadcast:true}]-(n:NTS_Note{IsDeleted:0})
        //                    match (refr)<-[:R_OrganizationRoot]-(ref:HRS_Organization{IsLatest:true})
        //                    match (n)-[rt:R_Note_Template]->(t:NTS_Template{IsDeleted:0,Status:{Status}})
        //                    -[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        //                    -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}})   
        //                    match (n)-[rnou:R_Note_Owner_User]->(u:ADM_User)
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0}) 
        //                    return n,'Broadcast' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        //                    ,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        //                    ,tr.Id as TemplateMasterId,#REF_FIELDS#

        //                    union
        //                    match (arn:HRS_OrganizationRoot{Id:{TagTo}})                   
        //                    match (arn)-[:R_OrganizationRoot_ParentOrganizationRoot*1..]->(crn:HRS_OrganizationRoot)                   
        //                    with collect(crn.Id) as o
        //                    match (refr:HRS_OrganizationRoot)
        //                    where refr.Id in o
        //                    match (refr)<-[:R_Note_TagTo{ReferenceType:'Organization',EnableBroadcast:true}]-(n:NTS_Note{IsDeleted:0})
        //                    match (refr)<-[:R_OrganizationRoot]-(ref:HRS_Organization{IsLatest:true})
        //                    match (n)-[rt:R_Note_Template]->(t:NTS_Template{IsDeleted:0,Status:{Status}})
        //                    -[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        //                    -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}})   
        //                    match (n)-[rnou:R_Note_Owner_User]->(u:ADM_User)
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        //                    optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0}) 
        //                    return n,'Broadcast' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        //                    ,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        //                    ,tr.Id as TemplateMasterId,#REF_FIELDS#

        //                    ";
        //                    break;
        //                case NoteReferenceTypeEnum.Team:
        //                    break;
        //                //case NoteReferenceTypeEnum.Project:
        //                //    referenceNode = @"optional match(n)-[:R_Note_TagTo{ReferenceType:'Project'}]->(ref:PMT_ProjectManagement{IsDeleted:0})";
        //                //    referenceFields = "'Project' as ReferenceType,ref.Id as ReferenceTo,ref.Name as ReferenceToDisplayName";
        //                //    break;
        //                default:
        //                    break;
        //            }
        //            if (searchModel.Type.IsNotNull())
        //            {
        //                var cypher = $@" Select n.*,'Owner' as OwnershipType,u.""Id"" as OwnerUserId,t.""Id"" as TemplateId,t.""Name"" as TemplateName
        //                ,s.""Code"" as NoteStatusCode,s.""Name"" as NoteStatusName,u.""Name"" as OwnerDisplayName,c.""Id"" as NtsCategoryId
        //                ,t.""Id"" as TemplateId,#REF_FIELDS# #BROADCAST#
        //from public.""NtsNote"" as n 
        //join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false
        //join public.""TemplateCategoryId"" as tc on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false
        //join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""Id""='{searchModel.UserId}'
        //left join public.""User"" as ru on ru.""Id""=n.""RequestedByUserId"" and ru.""IsDeleted""=false
        //left join public.""LOV"" as s on s.""Id""=n.""NoteStatusId"" and s.""IsDeleted""=false
        //#REF_NODE# 
        //union 
        //Select n,'Shared' as OwnershipType,u.""Id"" as OwnerUserId,t.""Id"" as TemplateId,t.""Name"" as TemplateName
        //                ,s.""Code"" as NoteStatusCode,s.""Name"" as NoteStatusName,u.""Name"" as OwnerDisplayName,c.""Id"" as NtsCategoryId
        //                ,t.""Id"" as TemplateId,#REF_FIELDS# #BROADCAST#
        //from public.""NtsNote"" as n 
        //join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false
        //join public.""TemplateCategoryId"" as tc on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false
        //join public.""User"" as u on u.""Id""=n.""OwnerUserId""
        //join public.""NtsNoteShared"" as ns on ns.""NtsNoteId""=n.""Id""
        //join public.""User"" as su on su.""Id""=ns.""SharedWithUserId"" 
        //and su.""Id""='{searchModel.UserId}'
        //left join public.""User"" as ru on ru.""Id""=n.""RequestedByUserId"" and ru.""IsDeleted""=false
        //left join public.""LOV"" as s on s.""Id""=n.""NoteStatusId"" and s.""IsDeleted""=false
        //#REF_NODE#
        //                ";
        //                //var cypher = @"match (n:NTS_Note{IsDeleted:0})-[rt: R_Note_Template]->(t: NTS_Template{IsDeleted:0,Status:{Status}})
        //                //-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        //                //-[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}})   
        //                //match (n)-[rnou:R_Note_Owner_User]->(u:ADM_User{Id:{userId}})
        //                //optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        //                //optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        //                //optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0}) 
        //                //#REF_NODE#
        //                //return n,'Owner' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        //                //,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        //                //,tr.Id as TemplateMasterId,#REF_FIELDS#
        //                //union 
        //                //match (n:NTS_Note{IsDeleted:0})-[rt: R_Note_Template]->(t: NTS_Template{IsDeleted:0,Status:{Status}})
        //                //-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:{Status}})
        //                //-[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:{Type},IsDeleted: 0,Status:{Status}}) 
        //                //match (n)-[:R_Note_SharedTo_User]->(su:ADM_User{Id:{userId}})
        //                //match (n)-[:R_Note_Owner_User]->(u:ADM_User)
        //                //optional match(n:NTS_Note{ IsDeleted: 0})-[rs: R_Note_Status_ListOfValue] -> (s: GEN_ListOfValue) 
        //                //optional match(n:NTS_Note{ IsDeleted: 0})-[rc: R_Note_NtsCategory] -> (c: NTS_NtsCategory{ IsDeleted: 0}) 
        //                //optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_RequestedBy_User] -> (ru: ADM_User{ IsDeleted: 0})
        //                //optional match(n:NTS_Note{ IsDeleted: 0})-[rru: R_Note_Owner_User] -> (u: ADM_User{ IsDeleted: 0})               
        //                //#REF_NODE#
        //                //return n,'Shared' as OwnershipType,u.Id as OwnerUserId,t.Id as TemplateId,tr.Name as TemplateName
        //                //,s.Code as NoteStatusCode,s.Name as NoteStatusName,u.UserName as OwnerDisplayName,c.Id as NtsCategoryId
        //                //,tr.Id as TemplateMasterId,#REF_FIELDS# 
        //                //#BROADCAST#
        //                //";
        //                //var prms = new Dictionary<string, object>();
        //                //prms.Add("Status", StatusEnum.Active.ToString());
        //                //prms.Add("CompanyId", CompanyId);
        //                //prms.Add("userId", searchModel.UserId);
        //                //prms.Add("Type", searchModel.Type);
        //                //prms.Add("TagTo", searchModel.TagTo);
        //                //prms.Add("IsAdmin", searchModel.IsAdmin);

        //                cypher = cypher.Replace("#BROADCAST#", broadCast);
        //                cypher = cypher.Replace("#REF_NODE#", referenceNode);
        //                cypher = cypher.Replace("#REF_FIELDS#", referenceFields);

        //                list =await _queryRepo2.ExecuteQueryList<NoteViewModel>(cypher, null);

        //                if (searchModel != null)
        //                {
        //                    if (searchModel.TemplateMasterId.IsNotNull())
        //                    {
        //                        list = list.Where(x => x.TemplateId == searchModel.TemplateMasterId).ToList();
        //                    }
        //                    if (searchModel.TagToType.IsNotNull() && searchModel.TagToType == NoteReferenceTypeEnum.Project)
        //                    {
        //                        //list = list.Where(x => x.ReferenceTo == searchModel.TagTo).ToList();
        //                    }
        //                    if (searchModel.NoteNo.IsNotNullAndNotEmpty())
        //                    {
        //                        list = list.Where(x => x.NoteNo == searchModel.NoteNo).ToList();
        //                    }

        //                    if (searchModel.NoteStatus.IsNotNull())
        //                    {
        //                        list = list.Where(x => x.NoteStatusCode == searchModel.NoteStatus).ToList();
        //                    }

        //                    if (searchModel.Text.IsNotNull())
        //                    {
        //                        list = list.Where(x => x.NoteDescription == searchModel.Text).ToList();
        //                    }
        //                }

        //                list = list.Distinct().ToList();
        //                    //.DistinctBy(x => x.Id).ToList();
        //            }
        //            return list.ToList();
        //        }

        public async Task<List<WorkStructureViewModel>> GetWorkStructureDiagram(string personId)
        {
            var list = new List<WorkStructureViewModel>();
            var assignmentData = await GetPersoneWorkStructureDetails(personId);
            //var personDetails = await _tableMetadataBusiness.GetTableDataByColumn("HRPERSON", null, "Id", personId);
            //var name = Convert.ToString(personDetails["PersonFullName"]);

            if (assignmentData.IsNotNull())
            {
                var a = assignmentData[0];
                var leaveApprovalHeirarchy = await _userHierarchyBusiness.GetLeaveApprovalHierarchyDetailsOfUser(a.UserId);
                var hierarchy = await _hierarchyMasterBusiness.GetList(x => x.HierarchyType == HierarchyTypeEnum.User);

                list.Add(new WorkStructureViewModel
                {
                    Id = "ROOT_USER",
                    Title = "User",
                    Description = a.PersonFullName + " Full Details",
                    ReferenceId = "ROOT_USER",
                    Type = "ROOT_USER",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });
                list.Add(new WorkStructureViewModel
                {
                    Id = "USER_DATA",
                    Title = "User",
                    Description = a.PersonFullName + "- " + a.PersonStatus,
                    ReferenceId = a.NoteId,
                    Type = "USER_DATA",
                    ParentId = "ROOT_USER",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                if (hierarchy.IsNotNull())
                {
                    foreach (var lah in hierarchy)
                    {

                        if (leaveApprovalHeirarchy.IsNotNull())
                        {
                            list.Add(new WorkStructureViewModel
                            {
                                Id = "USER_LEAVEAPPROVALDATA",
                                Title = "Hierarchy",
                                Description = lah.Name,
                                ReferenceId = a.UserId + "$$" + lah.Id,
                                Type = "USER_DATA_LAH_YES",
                                ParentId = "USER_DATA",
                                NodeShape = NodeShapeEnum.SquareWithHeader
                            });
                        }
                        else
                        {
                            list.Add(new WorkStructureViewModel
                            {
                                Id = "USER_LEAVEAPPROVALDATA",
                                Title = "Hierarchy",
                                Description = lah.Name,
                                ReferenceId = a.UserId + "$$" + lah.Id,
                                Type = "USER_DATA_LAH_NO",
                                ParentId = "USER_DATA",
                                NodeShape = NodeShapeEnum.SquareWithHeader
                            });
                        }

                    }
                }

                list.Add(new WorkStructureViewModel
                {
                    Id = "USER_ASSIGNMENT",
                    Title = "ASSIGNMENTS",
                    Description = a.AssignmentStatusName,
                    ReferenceId = a.NoteAssignmentId,
                    Type = "USER_ASSIGNMENT",
                    ParentId = "ROOT_USER",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                list.Add(new WorkStructureViewModel
                {
                    Id = "USER_CONTRACT",
                    Title = "CONTRACT",
                    Description = "Contract Details",
                    ReferenceId = a.NoteContractId,
                    Type = "USER_CONTRACT",
                    ParentId = "ROOT_USER",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                list.Add(new WorkStructureViewModel
                {
                    Id = "USER_SALARYINFO",
                    Title = "SALARY",
                    Description = "Salary Details",
                    ReferenceId = a.SalaryInfoId,
                    Type = "USER_SALARYINFO",
                    ParentId = "ROOT_USER",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                list.Add(new WorkStructureViewModel
                {
                    Id = "CONTRACT_SPONSER",
                    Title = "SPONSER",
                    Description = a.SponsorName,
                    ReferenceId = a.SponsorName,
                    Type = "CONTRACT_SPONSER",
                    ParentId = "USER_CONTRACT",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                list.Add(new WorkStructureViewModel
                {
                    Id = "ASSIGNMENT_JOB",
                    Title = "JOB",
                    Description = a.JobName,
                    ReferenceId = a.NoteJobId,
                    Type = "ASSIGNMENT_JOB",
                    ParentId = "USER_ASSIGNMENT",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                list.Add(new WorkStructureViewModel
                {
                    Id = "ASSIGNMENT_JOB_GRADE",
                    Title = "GRADE",
                    Description = a.GradeName + "- " + a.JobGrade,
                    ReferenceId = a.NoteGradeId,
                    Type = "ASSIGNMENT_JOB_GRADE",
                    ParentId = "ASSIGNMENT_JOB",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                list.Add(new WorkStructureViewModel
                {
                    Id = "ASSIGNMENT_DEPARTMENT",
                    Title = "DEPARTMENT",
                    Description = a.DepartmentName,
                    ReferenceId = a.NoteDepartmentId,
                    Type = "ASSIGNMENT_DEPARTMENT",
                    ParentId = "USER_ASSIGNMENT",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                //list.Add(new WorkStructureViewModel
                //{
                //    Id = "ASSIGNMENT_DEPARTMENT_COSTCENTER",
                //    Title = "COST CENTER",
                //    Description = "",
                //    ReferenceId = "ASSIGNMENT_DEPARTMENT_COSTCENTER",
                //    Type = "ASSIGNMENT_DEPARTMENT_COSTCENTER",
                //    ParentId = "ASSIGNMENT_DEPARTMENT",
                //    NodeShape = NodeShapeEnum.SquareWithHeader
                //});

                list.Add(new WorkStructureViewModel
                {
                    Id = "ASSIGNMENT_LOCATION",
                    Title = "LOCATION",
                    Description = a.LocationName,
                    ReferenceId = a.NoteLocationId,
                    Type = "ASSIGNMENT_LOCATION",
                    ParentId = "USER_ASSIGNMENT",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                list.Add(new WorkStructureViewModel
                {
                    Id = "ASSIGNMENT_POSITION",
                    Title = "POSITION",
                    Description = a.PositionTitle + "- " + a.PositionName,
                    ReferenceId = a.NotePositionId,
                    Type = "ASSIGNMENT_POSITION",
                    ParentId = "USER_ASSIGNMENT",
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });
            }
            return list;

        }

        public async Task<List<AssignmentViewModel>> GetPersoneWorkStructureDetails(string personId)
        {
            string query = $@"select CONCAT( hp.""FirstName"",' ',hp.""LastName"") as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
substring(assi.""DateOfJoin"",0,11) as DateOfJoin,lovs.""Name"" as AssignmentStatusName,case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,nt.""Id"" as NoteId,assi.""Id"" as AssignmentId,
cont.""Id"" as ContractId,ntc.""Id"" as NoteContractId,nta.""Id"" as NoteAssignmentId,hpos.""Id"" as PositionId,
ph.""Id"" as PositionHierarchyId,
ntp.""Id"" as NotePositionHierarchyId,
si.""Id"" as SalaryInfoId,ntsi.""Id"" as NoteSalaryInfoId,u.""PhotoId"" as PhotoId,hl.""NtsNoteId"" as NoteLocationId,
hd.""NtsNoteId"" as NoteDepartmentId,hj.""NtsNoteId"" as NoteJobId,hpos.""NtsNoteId"" as NotePositionId, hg.""NtsNoteId"" as NoteGradeId, 
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo, sp.""SponsorName"" as SponsorName
from cms.""N_CoreHR_HRPerson"" as hp
left join public.""NtsNote"" as nt on nt.""Id""=hp.""NtsNoteId"" and nt.""IsDeleted""=false and  nt.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and  assi.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as nta on nta.""Id""=assi.""NtsNoteId"" and nta.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and  hd.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and  hj.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false and  hpos.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false and  hl.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false and  hg.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as lovs on lovs.""Id""=assi.""AssignmentStatusId"" and lovs.""IsDeleted""=false and  lovs.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as cont on hp.""Id""=cont.""EmployeeId"" and cont.""IsDeleted""=false and  cont.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as ntc on ntc.""Id""=cont.""NtsNoteId"" and ntc.""IsDeleted""=false and  ntc.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=cont.""SponsorId"" and sp.""IsDeleted""=false and  sp.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as spn on spn.""Id""=sp.""NtsNoteId"" and spn.""IsDeleted""=false and  spn.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_PositionHierarchy"" as ph on ph.""PositionId""=assi.""PositionId"" and ph.""IsDeleted""=false and  ph.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as ntp on ntp.""Id""=ph.""NtsNoteId"" and ntp.""IsDeleted""=false and  ntp.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryInfo"" as si on hp.""Id""=si.""PersonId"" and si.""IsDeleted""=false and  si.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsNote"" as ntsi on ntsi.""Id""=si.""NtsNoteId"" and ntsi.""IsDeleted""=false and  ntsi.""CompanyId""='{_userContext.CompanyId}'
where hp.""IsDeleted""=false and  hp.""CompanyId""='{_userContext.CompanyId}' #WHERE#
                            ";
            var where = "";
            if (personId.IsNotNullAndNotEmpty())
            {
                where = $@"and hp.""Id""='{personId}'";
            }
            //if (legalEntityId.IsNotNullAndNotEmpty())
            //{
            //    where=where + $@"and hp.""PersonLegalEntityId""='{legalEntityId}'";
            //}
            query = query.Replace("#WHERE#", where);
            var queryData = await _queryAssignment.ExecuteQueryList(query, null);
            var list = queryData;
            return list;
        }
        public async Task<IList<ManpowerLeaveSummaryViewModel>> GetLeaveRequestDetails(ManpowerLeaveSummaryViewModel searchModel)
        {
            //var codes = "";
            //if (LegalEntityCode == "CAYAN_KSA")
            //{
            //    codes = "['ANNUAL_LEAVE', 'COMPASSIONATE_LEAVE_KSA', 'EXAMINATION_L', 'HAJJ_LEAVE_K', 'LEAVE_HANDOVER_SERVICE', 'MARRIAGE_L', 'MATERNITY_K', 'PATERNITY_K', 'SICK_L_K', 'UNDERTIME_REQUEST']";

            //}
            //else if (LegalEntityCode == "CAYAN_UAE")
            //{
            //    codes = " ['ANNUAL_LEAVE_HD_UAE', 'ANNUAL_LEAVE_UAE', 'HAJJ_LEAVE_U', 'IDDAH_L_UAE', 'MATERNITY_U', 'SICK_L_U', 'UNA_ABSENT_UAE', 'UNPAID_L_UAE']";
            //}
            var query = $@"select * from cms.""N_PayrollHR_PayrollCalendar"" where ""LegalEntityId""='{_repo.UserContext.LegalEntityId}'";
            var data = await _queryAssignment.ExecuteQuerySingle<CalendarViewModel>(query, null);
            var lbb = _sp.GetService<ILeaveBalanceSheetBusiness>();
            double actualWorkingDays = 0;
            if (data.IsNotNull())
            {
                actualWorkingDays = await lbb.GetActualWorkingdays(data.Id, searchModel.StartDate.Value, searchModel.EndDate.Value);
            }
            var match = $@"SELECT distinct per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName,
				al.""annualcount"" as AnnualLeaveDays,ml.""annualcount"" as SickLeaveDays,upl.""annualcount"" as UnpaidLeaveDays,ol.""annualcount"" as OtherLeaveDays,

a.""DateOfJoin"" as DateOfJoin,
				
per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
   FROM 
    public.""User"" as u 
	 join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null and per.""IsDeleted""=false and  per.""CompanyId""='{_userContext.CompanyId}'
left join (with OtherCount as    (
select count(s.*) as AnnualCount,s.""OwnerUserId"" from 
	 
	public.""NtsService"" as s 
  join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and  t.""CompanyId""='{_userContext.CompanyId}' and t.""Code"" not in ('AnnualLeave','SickLeave','UnpaidLeave') and s.""IsDeleted""=false
	-- left join cms.""N_Leave_AnnualLeave"" as al on al.""NtsNoteId""=s.""UdfNoteId""
--and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
	  group by s.""OwnerUserId""
	) select * from OtherCount ) as ol on ol.""OwnerUserId""=u.""Id"" 
left join (with AnnualCount as    (
select count(s.*) as AnnualCount,s.""OwnerUserId"" from 
	 
	public.""NtsService"" as s 
  join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and  t.""CompanyId""='{_userContext.CompanyId}' and t.""Code""='AnnualLeave' and s.""IsDeleted""=false
	 left join cms.""N_Leave_AnnualLeave"" as al on al.""NtsNoteId""=s.""UdfNoteId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}'
where ('{searchModel.StartDate}'::Date<=al.""LeaveStartDate""::Date and al.""LeaveStartDate""::Date <='{searchModel.EndDate}'::Date) and ('{searchModel.StartDate}'::Date<=al.""LeaveEndDate""::Date and al.""LeaveEndDate""::Date<='{searchModel.EndDate}'::Date )
	  group by s.""OwnerUserId""
	) select * from AnnualCount ) as al on al.""OwnerUserId""=u.""Id"" 
	left join (with SickCount as    (
select count(s.*) as AnnualCount,s.""OwnerUserId"" from 
	 
	public.""NtsService"" as s 
  join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and  t.""CompanyId""='{_userContext.CompanyId}' and t.""Code""='SickLeave' and s.""IsDeleted""=false
	  left join cms.""N_Leave_SickLeave"" as al on al.""NtsNoteId""=s.""UdfNoteId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}'
where ('{searchModel.StartDate}'::Date<=al.""LeaveStartDate""::Date and al.""LeaveStartDate""::Date <='{searchModel.EndDate}'::Date) and ('{searchModel.StartDate}'::Date<=al.""LeaveEndDate""::Date and al.""LeaveEndDate""::Date<='{searchModel.EndDate}'::Date)
	  group by s.""OwnerUserId""
	) select * from SickCount ) as ml on ml.""OwnerUserId""=u.""Id"" 
	left join (with UnpaidLeave as    (
select count(s.*) as AnnualCount,s.""OwnerUserId"" from 
	 
	public.""NtsService"" as s 
  join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and  t.""CompanyId""='{_userContext.CompanyId}' and t.""Code""='UnpaidLeave' and s.""IsDeleted""=false
	  left join cms.""N_Leave_UnpaidLeave"" as al on al.""NtsNoteId""=s.""UdfNoteId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}'
where ('{searchModel.StartDate}'::Date<=al.""LeaveStartDate""::Date and al.""LeaveStartDate""::Date <='{searchModel.EndDate}'::Date) and ('{searchModel.StartDate}'::Date<=al.""LeaveEndDate""::Date and al.""LeaveEndDate""::Date<='{searchModel.EndDate}'::Date)
	  group by s.""OwnerUserId""
	) select * from UnpaidLeave ) as upl on upl.""OwnerUserId""=u.""Id"" 
  
   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and  a.""CompanyId""='{_userContext.CompanyId}'
 left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false and  o.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and  j.""CompanyId""='{_userContext.CompanyId}'
left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}' and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
where u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'

";

            //            var match = string.Concat($@"SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, 
            //al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='AnnualLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_AnnualLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='Compassionately Leave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_CompassionatelyLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName,al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='LeaveExamination'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_LeaveExamination"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName,al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='HajjLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_HajjLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName,null as StartDate, 
            //				null as EndDate, null as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='LeaveHandoverService'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_Leave-Handover-Service"" as al on al.""Id""=s.""UdfNoteId""
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", a.""DateOfJoin""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='MarriageLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_MarriageLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='PaternityLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_PaternityLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"",
            //a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='MaternityLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_MaternityLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", 
            //a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, al.""LeaveStartDate"" as StartDate, 
            //				al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='SickLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_SickLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", 
            //a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,
            //--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, 
            //al.""LeaveStartDate"" as StartDate, al.""LeaveEndDate"" as EndDate, al.""Hours"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='UndertimeLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_UndertimeLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", 
            //a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""Hours""
            //union 
            //SELECT per.""SponsorshipNo"" as SponsorshipNo, per.""PersonNo"" as PersonNo,
            //--s.""Id"" as ServiceId,
            //                o.""DepartmentName"" as OrganizationName, j.""JobTitle"" as JobName, 
            //al.""LeaveStartDate"" as StartDate, al.""LeaveEndDate"" as EndDate, al.""LeaveDurationWorkingDays"" as LeaveDuration,
            //a.""DateOfJoin"" as DateOfJoin,
            //				--s.""ServiceDescription"" as Description,
            //per.""PersonFullName""  as PersonName,'{actualWorkingDays}' as TotalWorkingDays
            //   FROM public.""Template"" as t
            //   join public.""NtsService"" as s on s.""TemplateId""=t.""Id"" and s.""IsDeleted""=false and t.""Code""='UnpaidLeave'
            //   join public.""User"" as u on u.""Id""=s.""OwnerUserId"" 
            //   join cms.""N_CoreHR_HRPerson"" as per on per.""UserId""=u.""Id"" and per.""Id"" is not null
            //   left join cms.""N_Leave_UnpaidLeave"" as al on al.""Id""=s.""UdfNoteId""
            //and ('{searchModel.StartDate}'<=al.""LeaveStartDate"" and al.""LeaveStartDate"" <='{searchModel.EndDate}' or '{searchModel.StartDate}' is null) and ('{searchModel.StartDate}'<=al.""LeaveEndDate"" and al.""LeaveEndDate""<='{searchModel.EndDate}' or '{searchModel.EndDate}' is null)
            //   left join cms.""N_CoreHR_HRAssignment"" as a on per.""Id""=a.""EmployeeId""
            //left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId""
            //left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId""
            //left join public.""LegalEntity"" as l on per.""LegalEntityId""=l.""Id"" and l.""IsDeleted""=false and per.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'
            //--left join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""LegalEntityId"" = Per.""LegalEntityId"" and pc.""IsDeleted"" = false
            //#WHERE#
            // group by per.""SponsorshipNo"", per.""PersonNo"",per.""PersonFullName""
            //, o.""DepartmentName"", j.""JobTitle"", 
            //a.""DateOfJoin"",al.""LeaveStartDate"",al.""LeaveEndDate"",al.""LeaveDurationWorkingDays""
            //");

            var search = "";
            if (searchModel.PersonId.IsNotNullAndNotEmpty())
            {
                search = $@" where per.""Id""='{searchModel.PersonId}' ";
            }
            match = match.Replace("#WHERE#", search);
            var result = await _queryManpowerRepo1.ExecuteQueryList<ManpowerLeaveSummaryViewModel>(match, null);
            //var lbb = _sp.GetService<ILeaveBalanceSheetBusiness>();
            //foreach (var data in result)
            //{
            //    if (data.StartDate.IsNotNull() && data.EndDate.IsNotNull()) 
            //    {
            //        var actualWorkingDays = await lbb.GetActualWorkingdays(data.CalendarId, searchModel.StartDate.Value, searchModel.EndDate.Value);
            //        data.TotalWorkingDays = actualWorkingDays;
            //    }

            //}
            return result;
        }
        public async Task<TerminatePersonViewModel> GetPersonInfoForTermination(string personId)
        {
            var cypher = string.Concat($@"select  p.""Id"" as PersonId,u.""Id"" as UserId,u.""Name"" as UserName,o.""DepartmentName"" as OrganizationName,g.""GradeName"" as GradeName
            ,po.""PositionName"" as PositionName,j.""JobTitle"" as JobName
            ,TO_TIMESTAMP(a.""EffectiveStartDate"",'DD-MM-YYYY HH:MI:SS') as AssignmentEffectiveStartDate,TO_TIMESTAMP(a.""EffectiveEndDate"",'DD-MM-YYYY HH:MI:SS') as AssignmentEffectiveEndDate
            ,TO_TIMESTAMP(c.""EffectiveStartDate"",'DD-MM-YYYY HH:MI:SS') as ContractEffectiveStartDate,TO_TIMESTAMP(c.""EffectiveEndDate"",'DD-MM-YYYY HH:MI:SS') as ContractEffectiveEndDate,
			 count(t.*) as PendingTaskCount,count(s.*) as OwnedServiceCount,
            count(uh.*) as UserHierarchyDependentCount,
			u.""Status"" as UserStatus,p.""PersonFullName"" as FullName

from cms.""N_CoreHR_HRPerson"" as p

left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and  a.""CompanyId""='{_userContext.CompanyId}'
left join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false and  o.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""IsDeleted""=false and  j.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false and  g.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as po on po.""Id""=a.""PositionId"" and po.""IsDeleted""=false and  po.""CompanyId""='{_userContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as c on p.""Id""=c.""EmployeeId"" and c.""IsDeleted""=false and  c.""CompanyId""='{_userContext.CompanyId}'
left join public.""NtsTask"" as t on t.""OwnerUserId""=u.""Id"" and t.""IsDeleted""=false and  t.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as lv on lv.""Id""=t.""TaskStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId""='{_userContext.CompanyId}'
and  (lv.""Code""='TASK_STATUS_OVERDUE' or lv.""Code""='TASK_STATUS_INPROGRESS' or lv.""Code""='TASK_STATUS_DRAFT' or lv.""Code""='TASK_STATUS_NOTSTARTED')
left join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""IsDeleted""=false and  s.""CompanyId""='{_userContext.CompanyId}'
left join public.""LOV"" as slv on slv.""Id""=s.""ServiceStatusId"" and slv.""IsDeleted""=false and  slv.""CompanyId""='{_userContext.CompanyId}'
and  (slv.""Code""='SERVICE_STATUS_OVERDUE' or slv.""Code""='SERVICE_STATUS_INPROGRESS' or slv.""Code""='SERVICE_STATUS_NOTSTARTED')
left join public.""UserHierarchy"" as uh on uh.""ParentUserId""=u.""Id"" and uh.""IsDeleted""=false and  uh.""CompanyId""='{_userContext.CompanyId}'
where p.""Id""='{personId}' and p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'
group by p.""Id"" ,u.""Id"" ,u.""Name"",o.""DepartmentName""
            ,po.""PositionName"" ,j.""JobTitle""
            ,a.""EffectiveStartDate"" ,a.""EffectiveEndDate"" 
            ,c.""EffectiveStartDate"" ,c.""EffectiveEndDate"" ,g.""GradeName"",


            u.""Status"" ,p.""PersonFullName""--,t.""Id""
			
			
			

");

            //            var cypher = string.Concat(@"match (pr:HRS_PersonRoot{Id:{personId},IsDeleted:0})
            //            optional match(pr)<-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,IsLatest:true})


            //optional match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{ IsDeleted:0})
            //optional match(pr)<-[:R_User_PersonRoot]-(u:ADM_User{ IsDeleted:0})
            //            optional match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0})
            //            optional match(ar)<-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,IsLatest:true})

            //            optional match(a)-[:R_Assignment_OrganizationRoot]->(or:HRS_OrganizationRoot)<-[:R_OrganizationRoot]-(o:HRS_Organization{ IsLatest:true})           
            //            optional match(a)-[:R_Assignment_PositionRoot]->(por:HRS_PositionRoot)<-[:R_PositionRoot]-(po:HRS_Position{ IsLatest:true})          
            //            optional match(a)-[:R_Assignment_JobRoot]->(jr:HRS_JobRoot)<-[:R_JobRoot]-(j:HRS_Job{ IsLatest:true })

            //            optional match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0})<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0, IsLatest:true})

            //            with pr,p,c,o,j,po,a,u

            //            optional match(u)<-[:R_Task_AssignedTo_User]-(t:NTS_Task{IsDeleted:0})-[R_Task_Status_ListOfValue]->(lv:GEN_ListOfValue) where (lv.Code='OVER_DUE' or lv.Code='IN_PROGRESS'
            //            or lv.Code='DRAFT' or lv.Code='NOT_STARTED')

            //            with pr,p,c,o,j,po,a,u,count(t.Id) as PendingTaskCount

            //            optional match(u)<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
            //            where s.TemplateAction in ['Submit','Overdue','NotStarted']
            //            with pr,p,c,o,j,po,a,u,PendingTaskCount,count(s.Id) as OwnedServiceCount

            //            optional match(u)<-[:R_User_HierarchyLevel_ParentUser | :R_User_ParentUser]-(hu:ADM_User) 

            //            with pr,p,c,o,j,po,a,u,PendingTaskCount,OwnedServiceCount,count(hu.Id) as UserHierarchyDependentCount

            //            return pr.Id as PersonId,u.Id as UserId,u.UserName as UserName,o.Name as OrganizationName
            //            ,po.Name as PositionName,j.Name as JobName,p.EffectiveStartDate as PersonEffectiveStartDate,p.EffectiveEndDate as PersonEffectiveEndDate
            //            ,a.EffectiveStartDate as AssignmentEffectiveStartDate,a.EffectiveEndDate as AssignmentEffectiveEndDate
            //            ,c.EffectiveStartDate as ContractEffectiveStartDate,c.EffectiveEndDate as ContractEffectiveEndDate,PendingTaskCount,OwnedServiceCount
            //            ,UserHierarchyDependentCount,u.Status as UserStatus,"
            //              , Helper.PersonFullNameWithSponsorshipNo("p", " as FullName")
            //              );
            var prms = new Dictionary<string, object>
            {
                { "personId",personId },
            };
            var result = await _queryTerminatePersonRepo.ExecuteQuerySingle<TerminatePersonViewModel>(cypher, null);
            return result;
        }
        public async Task<CommandResult<TerminatePersonViewModel>> UpdatePersonForTermination(TerminatePersonViewModel person)
        {
            var _userBussiness = _sp.GetService<IUserBusiness>();
            // var taskBusiness = _sp.GetService<ITaskBusiness>();
            // var serviceBusiness = _sp.GetService<IServiceBusiness>();

            //var prms = new Dictionary<string, object>
            //{
            //    { "personId",person.PersonId },
            //};
            //Update Contract
            //var cypher = @"
            //match (pr:HRS_PersonRoot{Id:{personId},IsDeleted:0})                       
            //match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0})<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0})      
            //return c,cr.Id as ContractId";
            var cypher = $@"select c.*,c.""Id"" as ContractId
from cms.""N_CoreHR_HRPerson"" as p 
left join cms.""N_CoreHR_HRContract"" as c on p.""Id""=c.""EmployeeId"" and c.""IsDeleted""=false and  c.""CompanyId""='{_userContext.CompanyId}'
where p.""Id""='{person.PersonId}' and p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'";

            var contractList = await _queryRepo2.ExecuteQueryList<ContractViewModel>(cypher, null);

            foreach (var item in contractList)
            {
                if (person.PersonTerminateDate >= item.EffectiveStartDate && person.PersonTerminateDate <= item.EffectiveEndDate)
                {
                    item.EffectiveEndDate = person.PersonTerminateDate;
                    var data = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                    {
                        NoteId = item.NtsNoteId,
                        DataAction = DataActionEnum.Edit
                    });
                    item.LastUpdatedBy = _repo.UserContext.UserId;
                    item.LastUpdatedDate = DateTime.Now;
                    item.Status = StatusEnum.Inactive;
                    data.Json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                    var result = await _noteBusiness.ManageNote(data);
                    // item.IsLatest = true;
                    // item.Status = StatusEnum.Inactive;
                    // var data = BusinessHelper.MapModel<ContractViewModel, HRS_Contract>(item);
                    //data.LastUpdatedDate = DateTime.Now;
                    // data.LastUpdatedBy = UserId;

                }
                else if (item.EffectiveStartDate > person.PersonTerminateDate)
                {
                    //_contractRepo.MarkAsDeletedById(item.Id);
                    var delcypher = $@"update cms.""N_CoreHR_HRContract"" set ""IsDeleted""=true where  ""Id""='{item.Id}'";

                    var result = _queryRepo2.ExecuteCommand(delcypher, null);
                }
            }

            //Update Assignment
            //var cypher1 = @"
            //match (pr:HRS_PersonRoot{Id:{personId},IsDeleted:0})                       
            //match(pr)<-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{IsDeleted:0})<-[:R_AssignmentRoot]-(a:HRS_Assignment{IsDeleted:0})      
            //return a,ar.Id as AssignmentId";
            var cypher1 = $@"select a.*,a.""Id"" as AssignmentId,a.""NtsNoteId"" as NoteId
from cms.""N_CoreHR_HRPerson"" as p 
left join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and  a.""CompanyId""='{_userContext.CompanyId}' 
where p.""Id""='{person.PersonId}' and p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'";

            var assignList = await _queryRepo2.ExecuteQueryList<AssignmentViewModel>(cypher1, null);

            foreach (var item in assignList)
            {
                if (person.PersonTerminateDate >= item.EffectiveStartDate && person.PersonTerminateDate <= item.EffectiveEndDate)
                {
                    item.EffectiveEndDate = person.PersonTerminateDate;
                    var data = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                    {
                        NoteId = item.NoteId,
                        DataAction = DataActionEnum.Edit
                    });
                    item.LastUpdatedBy = _repo.UserContext.UserId;
                    item.LastUpdatedDate = DateTime.Now;
                    item.Status = StatusEnum.Inactive;
                    data.Json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                    var result = await _noteBusiness.ManageNote(data);
                    //item.IsLatest = true;
                    //item.Status = StatusEnum.Inactive;
                    //var data = BusinessHelper.MapModel<AssignmentViewModel, HRS_Assignment>(item);
                    //data.LastUpdatedDate = DateTime.Now;
                    //data.LastUpdatedBy = UserId;
                    // _repositoryAssignment.Edit(data);
                }
                else if (item.EffectiveStartDate > person.PersonTerminateDate)
                {
                    // _repositoryAssignment.MarkAsDeletedById(item.Id);
                    var delcypher = $@"update cms.""N_CoreHR_HRAssignment"" set ""IsDeleted""=true where  ""Id""='{item.Id}'";

                    var result = _queryRepo2.ExecuteCommand(delcypher, null);
                }
            }

            //Update User Hierarchy
            if (person.ProposedUserHierarchyReplacement != null && person.ProposedUserHierarchyReplacement.IsNotNullAndNotEmpty())
            {
                //    var userhierAdminQry = @"match (pr:HRS_PersonRoot{Id:{personId},IsDeleted:0})<-[:R_User_PersonRoot]-(pu:ADM_User{IsDeleted:0}) 
                //    match(pu)<-[r:R_User_ParentUser]-(u:ADM_User{IsDeleted:0}) return r,u.Id as UserId";

                //    var userHierAdmin = await _queryRepo2.ExecuteQueryList<UserHierarchyLevelViewModel>(userhierAdminQry, null);

                //    foreach (var item in userHierAdmin)
                //    {
                //        _userBussiness.UpdateUserHierarchyAdmin(item.HierarchyId, item.UserId, person.ProposedUserHierarchyReplacement);
                //    }


                //var userhierQry = @"match (pr:HRS_PersonRoot{Id:{personId},IsDeleted:0})<-[:R_User_PersonRoot]-(pu:ADM_User{IsDeleted:0}) 
                // match(pu) < -[r: R_User_HierarchyLevel_ParentUser] - (u: ADM_User{ IsDeleted: 0}) return r,u.Id as UserId";
                var userhierQry = $@"select r.*,u.""Id"" as UserId
from cms.""N_CoreHR_HRPerson"" as p 
 join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
 join public.""UserHierarchy"" as r on r.""ParentUserId""=u.""Id"" and r.""IsDeleted""=false and  r.""CompanyId""='{_userContext.CompanyId}'
where p.""Id""='{person.PersonId}' and p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'";
                var userHier = await _queryRepo2.ExecuteQueryList<UserHierarchyViewModel>(userhierQry, null);

                foreach (var item in userHier)
                {
                    await _userBussiness.UpdateHierarchyLevel(item.HierarchyId, item.UserId, person.ProposedUserHierarchyReplacement, item.LevelNo, item.OptionNo, DateTime.Now, DateTime.Now);
                }

            }
            //Update User
            //var cypher2 = @"
            //match (pr:HRS_PersonRoot{Id:{personId},IsDeleted:0})<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0})      
            //return u";
            var cypher2 = $@"Select u.* from cms.""N_CoreHR_HRPerson"" as p
left join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and  u.""CompanyId""='{_userContext.CompanyId}'
where p.""Id""='{person.PersonId}'  and p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'";

            var user = await _queryRepo2.ExecuteQuerySingle<UserViewModel>(cypher2, null);

            if (user != null)
            {
                user.Status = StatusEnum.Inactive;
                //var data = BusinessHelper.MapModel<UserViewModel, ADM_User>(user);
                await _userBussiness.Edit(user);
            }


            //Update Person
            //var cypher3 = @"
            //match (pr:HRS_PersonRoot{Id:{personId},IsDeleted:0})<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0})      
            //return p,pr.Id as PersonId";
            var cypher3 = $@"Select p.* from cms.""N_CoreHR_HRPerson"" as p 
 where p.""Id""='{person.PersonId}'  and p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'";

            var personList = await _queryRepo2.ExecuteQueryList<PersonViewModel>(cypher3, null);

            foreach (var item in personList)
            {
                //if (person.PersonTerminateDate >= item.EffectiveStartDate && person.PersonTerminateDate <= item.EffectiveEndDate)
                //{
                //    //item.EffectiveEndDate = person.PersonTerminateDate;
                //   // item.IsLatest = true;
                //   // item.Status = StatusEnum.Inactive;
                //   // item.EmployeeStatus = EmployeeStatusEnum.Terminated;
                //  //  var data = BusinessHelper.MapModel<PersonViewModel, HRS_Person>(item);
                //   // data.LastUpdatedDate = DateTime.Now;
                //  //  data.LastUpdatedBy = UserId;
                //   // _repository.Edit(data);
                //}
                //else if (item.EffectiveStartDate > person.PersonTerminateDate)
                //{
                // _repository.MarkAsDeletedById(item.Id);
                //}
                //var delcypher = $@"update cms.""N_CoreHR_HRPerson"" set ""IsDeleted""=true where  ""Id""='{item.Id}'";

                //var result = _queryRepo2.ExecuteCommand(delcypher, null);
                var data = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = item.NtsNoteId,
                    DataAction = DataActionEnum.Edit
                });
                item.LastUpdatedBy = _repo.UserContext.UserId;
                item.LastUpdatedDate = DateTime.Now;
                item.Status = StatusEnum.Inactive;
                data.Json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                var result = await _noteBusiness.ManageNote(data);
            }

            return CommandResult<TerminatePersonViewModel>.Instance(person);

        }

        public async Task<IList<IdNameViewModel>> GetAllTaskOwnerList(string Category, string Template)
        {
            var Query = $@"";

            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;
        }


        public async Task<IList<IdNameViewModel>> GetAllTaskAssigneeList(string Category, string Template)
        {
            var Query = "";

            if (Category.IsNotNullAndNotEmpty())
            {
                Category = Category.Replace(",", "','");
            }

            if (Template.IsNotNullAndNotEmpty())
            {
                Template = Template.Replace(",", "','");
            }


            Query = $@"Select Distinct nou.""Id"",nou.""Name""
                        from public.""NtsTask"" as n
                        join public.""User"" as nou ON nou.""Id""=n.""AssignedToUserId"" and nou.""IsDeleted""=false and  nou.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false and  tr.""CompanyId""='{_userContext.CompanyId}'
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and  tc.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""Module"" as m ON m.""Id""=tr.""ModuleId"" and m.""IsDeleted""=false  and  m.""CompanyId""='{_userContext.CompanyId}'                                              
                        left join public.""NtsService"" as s ON s.""Id""=n.""ParentServiceId"" and s.""IsDeleted""=false and  s.""CompanyId""='{_userContext.CompanyId}'
						left join public.""User"" as su ON su.""Id""=s.""OwnerUserId"" and su.""IsDeleted""=false and  su.""CompanyId""='{_userContext.CompanyId}'
                        where n.""IsDeleted""=false and  n.""CompanyId""='{_userContext.CompanyId}' #Category#  #Template# ";
            //where tc.""Id"" in ('{Category}')  and tr.""Id"" in ( '{Template}');

            if (Category.IsNotNull())
            {
                string vl = $@"and tc.""Id"" in ('{Category}')";
                Query = Query.Replace("#Category#", vl);
            }
            else { Query = Query.Replace("#Category#", ""); }

            if (Template.IsNotNull())
            {
                string vl = $@" and tr.""Id"" in ( '{Template}')";
                Query = Query.Replace("#Template#", vl);
            }
            else { Query = Query.Replace("#Template#", ""); }


            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;
        }




        public async Task<IList<IdNameViewModel>> GetCategory()
        {
            var Query = $@"SELECT ""Id"", ""Name""

    FROM public.""TemplateCategory"" where ""TemplateType"" in (5) and ""IsDeleted""=false and  ""CompanyId""='{_userContext.CompanyId}' ";

            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;
        }


        public async Task<IList<TaskDetailsViewModel>> GetTaskDetailList(TaskDetailsViewModel Model)
        {
            var Query = $@"Select n.""TaskNo"" as TaskNo,n.""Id"" as Id,n.""ActualSLA"",n.""TaskSLA"",n.""TaskStatusId"",n.""TaskTemplateId"" as TemplateId,tr.""Name"" as TemplateName,tr.""Code"" as TemplateMasterCode,

                         ns.""Name"" as Status,ns.""Code"" as Code,n.""TaskSubject"" as TaskName, n.""TaskDescription"" as Description,
						 
	                     nou.""Name"" as Assignee,nou.""Id"" as OwnerUserId,n.""StartDate"" ::TIMESTAMP::DATE  as StartDate,
	                     n.""DueDate"" ::TIMESTAMP::DATE  as DueDate,n.""LastUpdatedDate"" as LastUpdatedDate,'Owner' as TemplateUserType,
	                      n.""CompletedDate"" as CompletionDate, n.""CanceledDate"" as CancelDate, n.""CreatedDate"" as CreatedDate,
                        n.""TaskNo"" as TaskNo,tr.""DisplayName"" as Template,tc.""Name"" as Category,
                       n.""TaskSLA"" as SLA,nowner.""Name"" as owner
                        from public.""NtsTask"" as n
                    join public.""User"" as nou ON nou.""Id""=n.""AssignedToUserId"" and nou.""IsDeleted""=false and  nou.""CompanyId""='{_userContext.CompanyId}'
                 join public.""User"" as nowner ON nowner.""Id""=n.""OwnerUserId"" and nowner.""IsDeleted""=false and  nowner.""CompanyId""='{_userContext.CompanyId}'
                        join public.""Template"" as tr ON n.""TemplateId""=tr.""Id"" and tr.""IsDeleted""=false and  tr.""CompanyId""='{_userContext.CompanyId}'
                        join public.""TemplateCategory"" as tc ON tr.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and  tc.""CompanyId""='{_userContext.CompanyId}'
                        left join public.""LOV"" as ns on n.""TaskStatusId""=ns.""Id"" and ns.""IsDeleted""=false  and  ns.""CompanyId""='{_userContext.CompanyId}'
                where n.""IsDeleted""=false and  n.""CompanyId""='{_userContext.CompanyId}' #Category#  #TemplateCode# #Assignee# #Status#";


            if (Model.Category.IsNotNullAndNotEmpty())
            {
                string cats = Model.Category;
                cats = cats.Replace(",", "','");


                string cat = $@" and tc.""Id"" in ('{cats}') ";
                Query = Query.Replace("#Category#", cat);


                if (Model.Template.IsNotNull())
                {

                    string Temp = Model.Template;
                    Temp = Temp.Replace(",", "','");

                    string vl = $@" and tr.""Id"" in ('{Temp}') ";
                    Query = Query.Replace("#TemplateCode#", vl);
                }
                else { Query = Query.Replace("#TemplateCode#", ""); }

                if (Model.Assignee.IsNotNull())
                {
                    string Ass = Model.Assignee;
                    Ass = Ass.Replace(",", "','");
                    string vl = $@" and n.""AssignedToUserId"" in ('{Ass}') ";
                    Query = Query.Replace("#Assignee#", vl);
                }
                else { Query = Query.Replace("#Assignee#", ""); }


                if (Model.Status.IsNotNull())
                {
                    string vl = $@" and ns.""Id""='{Model.Status}' ";
                    Query = Query.Replace("#Status#", vl);
                }
                else { Query = Query.Replace("#Status#", ""); }
            }
            else
            {
                Query = Query.Replace("#Category#", "");
                //Query = Query.Replace("#TemplateCode#", "");
                // Query = Query.Replace("#Assignee#", "");


                if (Model.Template.IsNotNullAndNotEmpty())
                {
                    string Temp = Model.Template;
                    Temp = Temp.Replace(",", "','");

                    string vl = $@" and  tr.""Id"" in ('{Temp}') ";
                    Query = Query.Replace("#TemplateCode#", vl);

                }
                else { Query = Query.Replace("#TemplateCode#", ""); }

                if (Model.Status.IsNotNull())
                {
                    string st = Model.Status, vl = "";
                    st = st.Replace(",", "','");

                    if (Model.Template.IsNotNullAndNotEmpty())
                    {
                        vl = $@" and ns.""Id"" in ('{st}') ";
                    }
                    else
                    {
                        vl = $@"and ns.""Id"" in ('{st}') ";
                    }
                    Query = Query.Replace("#Status#", vl);
                }
                else { Query = Query.Replace("#Status#", ""); }

                if (Model.Assignee.IsNotNull())
                {

                    string vl = "";
                    string Ass = Model.Assignee;
                    Ass = Ass.Replace(",", "','");
                    if (Model.Status.IsNotNull() || Model.Template.IsNotNull())
                    {
                        vl = $@" and n.""AssignedToUserId"" in ('{Ass}') ";
                    }
                    else { vl = $@" and n.""AssignedToUserId"" in ('{Ass}') "; }
                    Query = Query.Replace("#Assignee#", vl);
                }
                else { Query = Query.Replace("#Assignee#", ""); }
            }


            var result = await _QueryTaskDetail.ExecuteQueryList<TaskDetailsViewModel>(Query, null);
            return result;


        }


        public async Task<IList<TaskDetailsViewModel>> GetServiceDetailsList(TaskDetailsViewModel Model)
        {
            var Query = $@"select  n.""ServiceNo"" as ServiceNo,n.""Id"" as Id,n.""ServiceTemplateId"" as TemplateId,tr.""Name"" as TemplateTemplateMasterName,   
 ns.""Name"" as Status,ns.""Id"" as ServiceStatusId,ns.""Code"" as ServiceStatusCode,n.""ServiceSubject"" as ServiceName,nou.""Name"" as owner,
 nou.""Id"" as OwnerUserId, n.""ServiceSLA""  as SLA,n.""StartDate"" ::TIMESTAMP::DATE  as StartDate,n.""DueDate"" ::TIMESTAMP::DATE  as EndDate,n.""CompletedDate"" as CompletionDate,n.""CanceledDate"" as CanceledDate, n.""LastUpdatedDate"" as LastUpdatedDate,
 n.""CreatedDate"" as CreatedDate, m.""Name"" as ModuleName,m.""Id"" as ModuleId,tr.""Code"" as TemplateMasterCode,tc.""Code"" as TemplateCategoryCode, tc.""TemplateCategoryType""
,'Owner' as TemplateUserType,tr.""DisplayName"" as Template,tc.""Name"" as Category
from
public.""NtsService"" as n
    join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false and  nou.""CompanyId""='{_userContext.CompanyId}'
  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false and  tr.""CompanyId""='{_userContext.CompanyId}'
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false and  tc.""CompanyId""='{_userContext.CompanyId}'
   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false and  ns.""CompanyId""='{_userContext.CompanyId}'
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false  and  m.""CompanyId""='{_userContext.CompanyId}'
where n.""IsDeleted""=false and  n.""CompanyId""='{_userContext.CompanyId}' #Category#  #TemplateCode# #Assignee# #Status#";


            if (Model.Category.IsNotNullAndNotEmpty())
            {
                string cats = Model.Category;
                cats = cats.Replace(",", "','");

                string cat = $@" and tc.""Id"" in ('{cats}') ";
                Query = Query.Replace("#Category#", cat);


                if (Model.Template.IsNotNull())
                {
                    string temp = Model.Template;
                    temp = temp.Replace(",", "','");
                    string vl = $@" and tr.""Id""in('{temp}') ";
                    Query = Query.Replace("#TemplateCode#", vl);
                }
                else { Query = Query.Replace("#TemplateCode#", ""); }

                if (Model.owner.IsNotNull())
                {

                    string owner = Model.owner;
                    owner = owner.Replace(",", "','");

                    string vl = $@" and n.""OwnerUserId"" in ('{owner}') ";
                    Query = Query.Replace("#Assignee#", vl);
                }
                else { Query = Query.Replace("#Assignee#", ""); }


                if (Model.Status.IsNotNull())
                {


                    string st = Model.Status;
                    st = st.Replace(",", "','");
                    string vl = $@" and ns.""Id""in ('{st}') ";
                    Query = Query.Replace("#Status#", vl);
                }
                else { Query = Query.Replace("#Status#", ""); }
            }
            else
            {
                Query = Query.Replace("#Category#", "");
                //  Query = Query.Replace("#TemplateCode#", "");
                //Query = Query.Replace("#Assignee#", "");



                if (Model.Template.IsNotNull())
                {
                    string temp = Model.Template, vl = "";
                    temp = temp.Replace(",", "','");

                    vl = $@" and  tr.""Id""in('{temp}') ";
                    Query = Query.Replace("#TemplateCode#", vl);
                }
                else { Query = Query.Replace("#TemplateCode#", ""); }
                if (Model.Status.IsNotNull())
                {
                    string st = Model.Status, vl = "";
                    st = st.Replace(",", "','");
                    if (Model.Template.IsNotNull())
                    {
                        vl = $@" and ns.""Id""in('{st}') ";
                    }
                    else
                    {

                        vl = $@" and ns.""Id""in('{st}') ";
                    }
                    Query = Query.Replace("#Status#", vl);
                }
                else { Query = Query.Replace("#Status#", ""); }


                if (Model.owner.IsNotNull())
                {
                    string vl = "";
                    string owner = Model.owner;
                    owner = owner.Replace(",", "','");
                    if (Model.Status.IsNotNull() || Model.Template.IsNotNull())
                    {
                        vl = $@" and n.""OwnerUserId"" in ('{owner}') ";
                    }
                    else
                    {

                        vl = $@" and n.""OwnerUserId"" in ('{owner}') ";
                    }
                    Query = Query.Replace("#Assignee#", vl);
                }
                else { Query = Query.Replace("#Assignee#", ""); }
            }


            var result = await _QueryTaskDetail.ExecuteQueryList<TaskDetailsViewModel>(Query, null);
            return result;


        }
        public async Task<IList<IdNameViewModel>> GetOwnerNameList(string Category, string Template)
        {



            if (Category.IsNotNullAndNotEmpty())
            {
                Category = Category.Replace(",", "','");
            }
            if (Template.IsNotNullAndNotEmpty())
            {
                Template = Template.Replace(",", "','");
            }
            var Query = $@"select Distinct nou.""Id"",nou.""Name""
from
public.""NtsService"" as n
    join public.""User"" as nou on n.""OwnerUserId""=nou.""Id"" and nou.""IsDeleted""=false and  nou.""CompanyId""='{_userContext.CompanyId}'
  join public.""Template"" as tr on tr.""Id"" =n.""TemplateId"" and tr.""IsDeleted""=false and  tr.""CompanyId""='{_userContext.CompanyId}'
  join public.""TemplateCategory"" as tc on tc.""Id"" =tr.""TemplateCategoryId"" and tc.""IsDeleted""=false and  tc.""CompanyId""='{_userContext.CompanyId}'
   join public.""LOV"" as ns on ns.""Id"" = n.""ServiceStatusId"" and ns.""IsDeleted""=false and  ns.""CompanyId""='{_userContext.CompanyId}'
    left join public.""Module"" as m on m.""Id"" =tr.""ModuleId"" and m.""IsDeleted""=false and  m.""CompanyId""='{_userContext.CompanyId}'
   where n.""IsDeleted""=false and  n.""CompanyId""='{_userContext.CompanyId}' #Category# #Template# ";
            //where tc.""Id""in ( '{Category}') and tr.""Id"" in ('{Template}')";



            if (Category.IsNotNull())
            {
                string vl = $@"and tc.""Id"" in ('{Category}')";
                Query = Query.Replace("#Category#", vl);
            }
            else { Query = Query.Replace("#Category#", ""); }

            if (Template.IsNotNull())
            {
                string vl = $@" and tr.""Id"" in ( '{Template}')";
                Query = Query.Replace("#Template#", vl);
            }
            else { Query = Query.Replace("#Template#", ""); }

            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;
        }


        public async Task<IList<IdNameViewModel>> GetTemplatecategoryWiseList(string Category)

        {
            var Query = "";
            if (Category.IsNotNullAndNotEmpty())
            {
                Category = Category.Replace(",", "','");
                //Category = "'" + Category + "'";
                Query = $@"select ""Id"",""DisplayName"" as Name from  public.""Template""  
               where ""TemplateCategoryId"" in('{Category}') ""IsDeleted""=false and  ""CompanyId""='{_userContext.CompanyId}' ";
            }
            else

            {
                Query = $@"SELECT distinct t.""Id"", t.""DisplayName"" as Name
               FROM public.""TemplateCategory"" as  tc 
inner join public.""Template""  as t on t.""TemplateCategoryId""=tc.""Id"" and t.""IsDeleted""=false and  t.""CompanyId""='{_userContext.CompanyId}'
where tc.""TemplateType""=5 and tc.""IsDeleted""=false and  tc.""CompanyId""='{_userContext.CompanyId}'";
            }


            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;

        }

        public async Task<IList<IdNameViewModel>> GetTemplatecategoryWiseListService(string Category)

        {
            var Query = "";
            if (Category.IsNotNullAndNotEmpty())
            {
                Category = Category.Replace(",", "','");
                //Category = "'" + Category + "'";
                Query = $@"select ""Id"",""DisplayName"" as Name from  public.""Template""  where ""TemplateCategoryId"" in('{Category}') ""IsDeleted""=false and  ""CompanyId""='{_userContext.CompanyId}'";
            }
            else

            {
                Query = $@"SELECT distinct t.""Id"", t.""DisplayName"" as Name
               FROM public.""TemplateCategory"" as  tc 
inner join public.""Template""  as t on t.""TemplateCategoryId""=tc.""Id"" and t.""IsDeleted""=false and  t.""CompanyId""='{_userContext.CompanyId}'
where tc.""TemplateType""=6 abd tc.""IsDeleted""=false and  tc.""CompanyId""='{_userContext.CompanyId}'";
            }


            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;

        }

        public async Task<List<LeaveSummaryViewModel>> GetLeaveSummary(string userId)
        {
            if (userId == null)
            {
                userId = _userContext.UserId;
            }
            var query = $@"select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'AnnualLeave' as LeaveType


from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_AnnualLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}' 
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id"" 
union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'MaternityLeave' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_MaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'PaternityLeave' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_PaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'CompassionatelyLeave' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_CompassionatelyLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId
,'HajjLeave' as LeaveType
from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_HajjLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'  l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'LeaveExamination' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_LeaveExamination"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'MarriageLeave' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_MarriageLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'PlannedUnpaidLeave' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_PlannedUnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'SickLeave' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_SickLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,'0' as LeaveDuration,u.""Id"" as UserId,'UndertimeLeave' as LeaveType
--,al.""LeaveReason"" as LeaveDuration

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_UndertimeLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select substring(al.""LeaveStartDate"",0,11) as StartDate,substring(al.""LeaveEndDate"",0,11) as EndDate
,al.""LeaveDurationWorkingDays"" as LeaveDuration,u.""Id"" as UserId,'UnpaidLeave' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_UnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select null as StartDate,null as EndDate
,'0' as LeaveDuration,u.""Id"" as UserId,'Leave-Handover-Service' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_Leave-Handover-Service"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""

union

select null as StartDate,null as EndDate
,'0' as LeaveDuration,u.""Id"" as UserId,'AnnualLeaveEncashment' as LeaveType

from cms.""N_CoreHR_HRPerson"" as p  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and  nts.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_Leave_AnnualLeaveEncashment"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and  al.""CompanyId""='{_userContext.CompanyId}'
join public.""LOV"" as l on l.""Id""=nts.""ServiceStatusId"" and l.""IsDeleted""=false and  l.""CompanyId""='{_userContext.CompanyId}'
where p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}' and l.""Code""='SERVICE_STATUS_COMPLETE' and u.""Id""='{userId}'
--group by al.""LeaveStartDate"",al.""LeaveEndDate"",u.""Id""


";

            var list = await _queryManpowerRepo1.ExecuteQueryList<LeaveDetailViewModel>(query, null);
            list = list.Where(x => x.StartDate != null && x.EndDate != null).ToList();

            var count = new List<LeaveSummaryViewModel>();

            var now = DateTimeOffset.Now;
            var Months = Enumerable.Range(1, 12).Select(i => now.AddMonths(-i).ToString("MMM-yyyy"));

            foreach (var mon in Months)
            {
                var month = new LeaveSummaryViewModel { Month = mon, Count = 0 };

                DateTime monyear = DateTime.Parse(mon);
                foreach (var item in list)
                {
                    DateTime sd = DateTime.Parse(item.StartDate.ToString());
                    DateTime ed = DateTime.Parse(item.EndDate.ToString());

                    if (sd.Year == monyear.Year && sd.Month == monyear.Month && ed.Month == monyear.Month)
                    {
                        month.Count += (ed - sd).Days + 1;
                        month.Type = item.LeaveType;
                    }
                    else if (sd.Year == monyear.Year && sd.Month == monyear.Month && ed.Month > monyear.Month)
                    {
                        var i = DateTime.DaysInMonth(monyear.Year, monyear.Month);
                        month.Count += (i - sd.Day) + 1;
                        month.Type = item.LeaveType;
                    }
                    else if (sd.Year == monyear.Year && sd.Month < monyear.Month && ed.Month == monyear.Month)
                    {
                        var i = DateTime.DaysInMonth(monyear.Year, monyear.Month);
                        month.Count += ed.Day;
                        month.Type = item.LeaveType;
                    }
                }

                count.Add(month);
            }

            return count;
        }


        public async Task<List<IdNameViewModel>> GetSponsorList()
        {
            var Query = $@"SELECT * FROM cms.""N_CoreHR_HRSponsor"" where ""IsDeleted""= and  ""CompanyId""='{_userContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);

            return result;
        }
        public async Task<AssignmentViewModel> GetAssignmentByPerson(string personId)
        {
            //var cypher = @"match(pr:HRS_PersonRoot{IsDeleted:0,Id:{Id}})<-[:R_AssignmentRoot_PersonRoot]
            //-(ar:HRS_AssignmentRoot)<-[:R_AssignmentRoot]-(a:HRS_Assignment{IsLatest:true,IsDeleted:0})
            //optional match(a)-[:R_Assignment_PositionRoot]->(por:HRS_PositionRoot{ IsDeleted: 0 })
            //return a,pr.Id as PersonId,por.Id as PositionId";
            var cypher = $@"Select a.*,p.""Id"" as PersonId,por.""Id"" as PositionId
from cms.""N_CoreHR_HRPerson"" as p
join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and  a.""CompanyId""='{_userContext.CompanyId}'
join cms.""N_CoreHR_HRPosition"" as por on a.""PositionId""=por.""Id"" and por.""IsDeleted""=false and  por.""CompanyId""='{_userContext.CompanyId}'
where p.""Id""='{personId}' and p.""IsDeleted""=false and  p.""CompanyId""='{_userContext.CompanyId}'
";
            // var parameters = new Dictionary<string, object> { { "Id", personId } };
            return await _queryAssignment.ExecuteQuerySingle<AssignmentViewModel>(cypher, null);
        }

        public async Task<IdNameViewModel> GetPersonLocationId(string PersonId)
        {
            var Query = $@"select L.""Id"",""LocationName"" from cms.""N_CoreHR_HRPerson"" P
inner join cms.""N_CoreHR_HRAssignment"" A on P.""Id"" = A.""EmployeeId"" and A.""IsDeleted""=false and  A.""CompanyId""='{_userContext.CompanyId}'
inner join  cms.""N_CoreHR_HRLocation"" L on L.""Id"" = A.""LocationId"" and L.""IsDeleted""=false and  L.""CompanyId""='{_userContext.CompanyId}'
where P.""Id"" = '{PersonId}' and P.""IsDeleted""=false and  P.""CompanyId""='{_userContext.CompanyId}'";

            var result = await _queryRepo1.ExecuteQuerySingle(Query, null);
            return result;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetUserHierarchy(string parentId, int levelUpto, string hierarchyId)
        {
            string query = $@"  select d.""Id"" as Id,d.""PhotoId"" as PhotoId,h.""NtsNoteId"" as UserHierarchyNoteId
                            ,d.""Name"" as UserName ,h.""ParentUserId"" as ParentId
                           ,coalesce(t.""Count"",0) as DirectChildCount,h.""HierarchyId"" as HierarchyId
                            from public.""User"" as d
                            

                            left join cms.""N_GENERAL_UserHierarchy"" as h on d.""Id"" = h.""UserId"" and h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}'
                            left join(
                            WITH RECURSIVE List AS(

                             WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from public.""User"" as d
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentUserId"" as ""ParentId"",'Child' as Type
                                from cms.""N_GENERAL_UserHierarchy"" as h
                                join public.""User"" as d on h.""UserId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentUserId"" = ns.""Id""
                                where h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}' and h.""HierarchyId""='{hierarchyId}'
                        
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
                                from public.""User"" as d
                                where d.""Id"" = '{parentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select d.""Id"" as Id,h.""ParentUserId"" as ""ParentId"",'Child' as Type,ns1.level+ 1
                                from cms.""N_GENERAL_UserHierarchy"" as h
                                join public.""User"" as d on h.""UserId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department1 ns1 on h.""ParentUserId"" = ns1.""Id""
                               where h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}' and h.""HierarchyId""='{hierarchyId}'
                             )
                            select ""Id"",""ParentId"",level from Department1
								
                            )
                            SELECT ""Id"",""ParentId"",level from List1 
                            )
                            t1 on d.""Id"" = t1.""Id""
                            where t1.level <={levelUpto} and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}' and h.""HierarchyId""='{hierarchyId}'
                            ";



            var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);
            var list = queryData;
            return list;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetObjectHierarchy(string parentId, int levelUpto, string hierarchyId, string permissions)
        {
            string query = "";

            var list = new List<UserHierarchyChartViewModel>();

            if (levelUpto <= 0)
            {
                if (_userContext.IsSystemAdmin || (permissions != null && permissions.Contains("CanViewAllBooks")))
                {
                    query = $@"select s.""Id"" as ""Id"", '-1' as ""ParentId"",'Category' as ""NodeType"",s.""SequenceOrder"" as SequenceOrder, c.""CategoryName"" as Name,TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from public.""NtsService"" as s
                                join cms.""N_SNC_TECProcess_ProcessCategory"" as c on s.""UdfNoteTableId""=c.""Id"" and c.""IsDeleted""=false 
                                left join (
                                    select count(b.*) as Count,bs.""ParentServiceId"" as ServiceId 
                                    from cms.""N_SNC_TECProcess_ProcessGroup"" as b
                                    join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
	                                group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
                                where s.""TemplateCode""='TEC_PROCESS_CATEGORY' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";
                }
                else
                {
                    query = $@"select s.""Id"" as ""Id"", '-1' as ""ParentId"",'Category' as ""NodeType"",s.""SequenceOrder"" as SequenceOrder,c.""CategoryName"" as Name,TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                    from public.""NtsService"" as s
                    join cms.""N_SNC_TECProcess_ProcessCategory"" as c on s.""UdfNoteTableId""=c.""Id"" and c.""IsDeleted""=false 
                    left join (
                                    select count(b.*) as Count, bs.""ParentServiceId"" as ServiceId
                                    from cms.""N_SNC_TECProcess_ProcessGroup"" as b
                                    join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
			left join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=bs.""Id"" and ss.""IsDeleted""=false 
            left join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_repo.UserContext.UserId}' and u.""IsDeleted""=false 

                                    group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
                    join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_repo.UserContext.UserId}' and u.""IsDeleted""=false 
            where s.""IsDeleted"" = 'false' and s.""CompanyId""='{_repo.UserContext.CompanyId}' 
            union
            select s.""Id"" as ""Id"", '-1' as ""ParentId"", c.""CategoryName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
              from public.""NtsService"" as s
              join cms.""N_SNC_TECProcess_ProcessCategory"" as c on s.""UdfNoteTableId""=c.""Id"" and c.""IsDeleted""=false 
            left join (
                                      select count(b.*) as Count, bs.""ParentServiceId"" as ServiceId
                                      from cms.""N_SNC_TECProcess_ProcessGroup"" as b
                                      join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
			   left join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=bs.""Id"" and ss.""IsDeleted""=false 
               left join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_repo.UserContext.UserId}' and tu.""IsDeleted""=false  


                                    group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""            
           join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_repo.UserContext.UserId}' and tu.""IsDeleted""=false  
            where s.""IsDeleted"" = 'false' and s.""CompanyId""='{_repo.UserContext.CompanyId}' 
            union
            select s.""Id"" as ""Id"", '-1' as ""ParentId"", c.""CategoryName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
              from public.""NtsService"" as s
              join cms.""N_SNC_TECProcess_ProcessCategory"" as c on s.""UdfNoteTableId""=c.""Id"" and c.""IsDeleted""=false 
            left join (
                                      select count(b.*) as Count, bs.""ParentServiceId"" as ServiceId
                                      from cms.""N_SNC_TECProcess_ProcessGroup"" as b
                                      join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
				join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=bs.""Id"" and ss.""IsDeleted""=false 
            	join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_repo.UserContext.UserId}' and u.""IsDeleted""=false 	                                

                                    group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""            
            join public.""NtsService"" as cs on cs.""ParentServiceId""=s.""Id"" and cs.""IsDeleted"" = 'false' and cs.""CompanyId""='{_repo.UserContext.CompanyId}'            


           join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=cs.""Id"" and ss.""IsDeleted""=false 
            join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_repo.UserContext.UserId}' and u.""IsDeleted""=false 
            where s.""IsDeleted"" = 'false' and s.""CompanyId""='{_repo.UserContext.CompanyId}' 
            union
            select s.""Id"" as ""Id"", '-1' as ""ParentId"", c.""CategoryName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
              from public.""NtsService"" as s
              join cms.""N_SNC_TECProcess_ProcessCategory"" as c on s.""UdfNoteTableId""=c.""Id"" and c.""IsDeleted""=false 
            left join (
                                      select count(b.*) as Count, bs.""ParentServiceId"" as ServiceId
                                      from cms.""N_SNC_TECProcess_ProcessGroup"" as b
                                      join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
				join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=bs.""Id"" and ss.""IsDeleted""=false 
             	join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_repo.UserContext.UserId}' and tu.""IsDeleted""=false  


                                    group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
            join public.""NtsService"" as cs on cs.""ParentServiceId""=s.""Id"" and cs.""IsDeleted"" = 'false' and cs.""CompanyId""='{_repo.UserContext.CompanyId}'   

           join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=cs.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_repo.UserContext.UserId}' and tu.""IsDeleted""=false  
            where s.""IsDeleted"" = 'false' and s.""CompanyId"" = '{_repo.UserContext.CompanyId}'  ";
                }


                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list.AddRange(queryData);

                var model = new UserHierarchyChartViewModel()
                {
                    Id = "-1",
                    Name = "Books",
                    DirectChildCount = queryData.Count()
                };
                list.Insert(0, model);
            }
            else if (levelUpto == 1)
            {
                //query = $@" WITH RECURSIVE Books AS(
                //                 select s.""Id"" as ""Id"", s.""ParentServiceId"" as ""ParentId"",c.""CategoryName"" as Name,s.""CreatedDate"",'Parent' as Type
                //                from public.""NtsService"" as s
                //                join cms.""N_SNC_TECProcess_ProcessCategory"" as c on s.""UdfNoteTableId""=c.""Id"" and c.""IsDeleted""=false 
                //                where s.""Id"" = '{parentId}' and s.""TemplateCode""='TEC_PROCESS_CATEGORY' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'

                //              union all

                //                select s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"", b.""ProcessGroupName"" as Name, s.""CreatedDate"",'Child' as Type
                //                from public.""NtsService"" as s
                //                join cms.""N_SNC_TECProcess_ProcessGroup"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'
                //                join Books ns on s.""ParentServiceId"" = ns.""Id""
                //                where s.""TemplateCode""='TEC_PROCESS_GROUP' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'                        
                //             )
                //            select ""Id"",""ParentId"",Name,""CreatedDate"",Type from Books ";

                if (_userContext.IsSystemAdmin || (permissions != null && permissions.Contains("CanViewAllBooks")))
                {
                    query = $@"select b.""Id"" as BookId, s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"",b.""ProcessGroupImage"" as BookImage,'Book' as ""NodeType"",s.""SequenceOrder"" as SequenceOrder, b.""ProcessGroupName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Child' as Type,bc.Count as DirectChildCount
                                from public.""NtsService"" as s
                                join cms.""N_SNC_TECProcess_ProcessGroup"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'                                
                                left join (
                                    select count(b.*) as Count,bs.""ParentServiceId"" as ServiceId 
                                    from cms.""N_SNC_TECProcess_ProcessItem"" as b
                                    join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
	                                group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
                        where s.""ParentServiceId""='{parentId}' and s.""TemplateCode""='TEC_PROCESS_GROUP' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' 
                    ";
                }
                else
                {
                    query = $@" select b.""Id"" as BookId, s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"",b.""ProcessGroupImage"" as BookImage,'Book' as ""NodeType"",s.""SequenceOrder"" as SequenceOrder, b.""ProcessGroupName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Child' as Type,bc.Count as DirectChildCount
                                from public.""NtsService"" as ps
                                join public.""NtsService"" as s on ps.""Id""=s.""ParentServiceId"" and s.""IsDeleted""=false
                                join cms.""N_SNC_TECProcess_ProcessGroup"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'                                
                                left join (
                                    select count(b.*) as Count, bs.""ParentServiceId"" as ServiceId
                                    from cms.""N_SNC_TECProcess_ProcessItem"" as b
                                    join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
	                                group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
                        join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
                        join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='{_repo.UserContext.UserId}' and u.""IsDeleted""=false
                        where ps.""Id"" = '{parentId}'

                        and s.""TemplateCode""='TEC_PROCESS_GROUP' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' 

                        union
                        select b.""Id"" as BookId, s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"",b.""ProcessGroupImage"" as BookImage,'Book' as ""NodeType"",s.""SequenceOrder"" as SequenceOrder, b.""ProcessGroupName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Child' as Type,bc.Count as DirectChildCount
                                from public.""NtsService"" as ps
                                join public.""NtsService"" as s on ps.""Id""=s.""ParentServiceId"" and s.""IsDeleted""=false
                                join cms.""N_SNC_TECProcess_ProcessGroup"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'


                                left join (
                                    select count(b.*) as Count, bs.""ParentServiceId"" as ServiceId
                                    from cms.""N_SNC_TECProcess_ProcessItem"" as b
                                    join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
	                                group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
                        join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=ps.""Id"" and ss.""IsDeleted""=false 
                        join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='618e85079069629db015b045' and tu.""IsDeleted""=false  
                        where ps.""Id"" = '{parentId}'

                        union

select b.""Id"" as BookId, s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"", b.""ProcessGroupImage"" as BookImage,'Book' as ""NodeType"",s.""SequenceOrder"" as SequenceOrder,b.""ProcessGroupName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Child' as Type,bc.Count as DirectChildCount
                                from public.""NtsService"" as s
                                join cms.""N_SNC_TECProcess_ProcessGroup"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'
                                
                                left join (
                                    select count(b.*) as Count,bs.""ParentServiceId"" as ServiceId 
                                    from cms.""N_SNC_TECProcess_ProcessItem"" as b
                                    join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
	                                group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
                        join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
                        join public.""User"" as u on u.""Id""=ss.""SharedWithUserId"" and u.""Id""='618e85079069629db015b045' and u.""IsDeleted""=false
                                where s.""ParentServiceId""='{parentId}' and s.""TemplateCode""='TEC_PROCESS_GROUP' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' 
                    union
			select b.""Id"" as BookId, s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"",b.""ProcessGroupImage"" as BookImage,'Book' as ""NodeType"",s.""SequenceOrder"" as SequenceOrder, b.""ProcessGroupName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Child' as Type,bc.Count as DirectChildCount
                                from public.""NtsService"" as s
                                join cms.""N_SNC_TECProcess_ProcessGroup"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'                                
                                left join (
                                    select count(b.*) as Count, bs.""ParentServiceId"" as ServiceId
                                    from cms.""N_SNC_TECProcess_ProcessItem"" as b
                                    join public.""NtsService"" as bs on b.""Id""=bs.""UdfNoteTableId"" and bs.""IsDeleted""=false
	                                group by bs.""ParentServiceId""
                                ) as bc on ServiceId=s.""Id""
			join public.""NtsServiceShared"" as ss on ss.""NtsServiceId""=s.""Id"" and ss.""IsDeleted""=false 
            join public.""TeamUser"" as tu on tu.""TeamId""=ss.""SharedWithTeamId"" and tu.""UserId""='{_repo.UserContext.UserId}' and tu.""IsDeleted""=false  
            where s.""ParentServiceId""='{parentId}' and s.""TemplateCode""='TEC_PROCESS_GROUP' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
			
                              ";
                }

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 2)
            {
                //query = $@" WITH RECURSIVE Pages AS(
                //                 select s.""Id"" as ""Id"", s.""ParentServiceId"" as ""ParentId"",c.""ProcessGroupName"" as Name,s.""CreatedDate"",'Parent' as Type
                //                from public.""NtsService"" as s
                //                join cms.""N_SNC_TECProcess_ProcessGroup"" as c on s.""UdfNoteTableId""=c.""Id"" and c.""IsDeleted""=false 
                //                where s.""Id"" = '{parentId}' and s.""TemplateCode""='TEC_PROCESS_GROUP' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'

                //              union all

                //                select s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"", b.""ItemName"" as Name, s.""CreatedDate"",'Child' as Type
                //                from public.""NtsService"" as s
                //                join cms.""N_SNC_TECProcess_ProcessItem"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'
                //                join Pages ns on s.""ParentServiceId"" = ns.""Id""
                //                where s.""TemplateCode""='TEC_PROCESS_ITEM' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'                        
                //             )
                //            select ""Id"",""ParentId"",Name,""CreatedDate"",Type from Pages ";

                query = $@" select b.""Id"" as PageId,bo.""Id"" as BookId, s.""Id"" as Id, s.""ParentServiceId"" as ""ParentId"",'Page' as ""NodeType"",b.""PageCover"" as BookImage, s.""SequenceOrder"" as SequenceOrder, b.""ItemName"" as Name, TO_CHAR(s.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Child' as Type
                                from public.""NtsService"" as s
                                join cms.""N_SNC_TECProcess_ProcessItem"" as b on s.""UdfNoteTableId"" = b.""Id"" and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}'                                
                                left join public.""NtsService"" as s1  on s1.""Id"" = '{parentId}'                              
                                left join cms.""N_SNC_TECProcess_ProcessGroup"" as bo on s1.""UdfNoteTableId"" = bo.""Id"" and bo.""IsDeleted""=false and bo.""CompanyId""='{_repo.UserContext.CompanyId}'

                                where s.""ParentServiceId""='{parentId}' and s.""TemplateCode""='TEC_PROCESS_ITEM' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'                        
                              ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            foreach (var x in list)
            {
                x.Count = list.Where(x => x.ParentId == x.Id).Count();
            }
            return list;
        }
        public async Task<List<double>> GetUserNodeLevel(string userId, string hierarchyId)
        {
            string query = $@"   WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from public.""User"" as d
                                where d.""Id"" = '{userId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentUserId"" as ""ParentId"",'Child' as Type
                                from cms.""N_GENERAL_UserHierarchy"" as h
                                join public.""User"" as d on h.""UserId"" = d.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                                join Department ns on h.""ParentUserId"" = ns.""Id""
                                where  h.""IsDeleted""=false and h.""CompanyId""='{_userContext.CompanyId}' and h.""HierarchyId""='{hierarchyId}'
                             )
                            select Count(""Id""),""ParentId"" from Department  where Type = 'Child' group by ""ParentId""
						
                            ";



            var queryData = await _queryRepopos.ExecuteScalarList<double>(query, null);
            var list = queryData;
            return list;
        }
        public async Task CreateUserHierarchy(NoteTemplateViewModel viewModel, string hierarchyId)
        {
            //var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == "USER_HIERARCHY");
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var Parent = rowData.GetValueOrDefault("ParentUserId").ToString();
            if (viewModel.DataAction == DataActionEnum.Create && Parent.IsNotNullAndNotEmpty())
            {

                var user = viewModel.UdfNoteTableId;

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.TemplateCode = "USER_HIERARCHY";
                var note = await _noteBusiness.GetNoteDetails(noteTemp);

                note.OwnerUserId = _repo.UserContext.UserId;
                note.StartDate = DateTime.Now;
                note.Json = "{}";
                note.DataAction = DataActionEnum.Create;

                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ParentUserId", Parent);
                ((IDictionary<String, Object>)exo).Add("UserId", user);
                //if (hierarchy != null)
                //{
                //var hierarchyId = hierarchy.Id;
                ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                //}

                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _noteBusiness.ManageNote(note);
            }
            else if (viewModel.DataAction == DataActionEnum.Edit && Parent.IsNotNullAndNotEmpty())
            {
                //  var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                // var Parent = rowData.GetValueOrDefault("ParentDepartmentId");
                var user = viewModel.UdfNoteTableId;
                var existingdephierarchy = await _tableMetadataBusiness.GetTableDataByColumn("USER_HIERARCHY", null, "UserId", user);
                // var hierarchyId = "";

                if (existingdephierarchy != null)
                {
                    var dephierarchyId = Convert.ToString(existingdephierarchy["NtsNoteId"]);
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "USER_HIERARCHY";
                    noteTemp.NoteId = dephierarchyId;
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.OwnerUserId = _repo.UserContext.UserId;
                    note.StartDate = DateTime.Now;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Edit;

                    //var list = new List<System.Dynamic.ExpandoObject>();
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("ParentUserId", Parent);
                    ((IDictionary<String, Object>)exo).Add("UserId", user);
                    //if (hierarchy != null)
                    //{
                    //    var hierarchyId = hierarchy.Id;
                    ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                    //}


                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    var res = await _noteBusiness.ManageNote(note);
                }


            }


        }
        public async Task<List<UserHierarchyChartViewModel>> GetBusinessHierarchy(string parentId, int levelUpto)
        {
            string query = "";

            var list = new List<UserHierarchyChartViewModel>();

            if (levelUpto <= 0)
            {
                query = $@"select distinct d.""Id"" as ""Id"", '-1' as ""ParentId"",'Level1' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""OrgLevel1Id"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""OrgLevel1Id"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""OrgLevel2Id"" and d.""IsDeleted""=false 
	                                group by a.""OrgLevel1Id""
                                ) as bc on Level=a.""OrgLevel1Id""
                                where a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list.AddRange(queryData);

                var model = new UserHierarchyChartViewModel()
                {
                    Id = "-1",
                    Name = "Business Hierarchy",
                    DirectChildCount = queryData.Count()
                };
                list.Insert(0, model);
            }
            else if (levelUpto == 1)
            {
                query = $@"select distinct a.""OrgLevel1Id""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id"" as ""ParentId"",'Level2' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""OrgLevel2Id"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""OrgLevel2Id"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""OrgLevel3Id"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parentId}' group by a.""OrgLevel2Id""
                                ) as bc on Level=a.""OrgLevel2Id""
                                where a.""OrgLevel1Id""='{parentId}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 2)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id"" as ""ParentId"",'Level3' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""OrgLevel3Id"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""OrgLevel3Id"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""OrgLevel4Id"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' group by a.""OrgLevel3Id""
                                ) as bc on Level=a.""OrgLevel3Id""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 3)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id"" as ""ParentId"",'Level4' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""OrgLevel4Id"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""OrgLevel4Id"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""BrandId"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' group by a.""OrgLevel4Id""
                                ) as bc on Level=a.""OrgLevel4Id""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 4)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id"" as ""ParentId"",'Brand' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""BrandId"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""BrandId"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""MarketId"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}'  group by a.""BrandId""
                                ) as bc on Level=a.""BrandId""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 5)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId"" as ""ParentId"",'Market' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""MarketId"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""MarketId"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""ProvinceId"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}'  group by a.""MarketId""
                                ) as bc on Level=a.""MarketId""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 6)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId"" as ""ParentId"",'Province' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""ProvinceId"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""ProvinceId"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_CareerLevel"" as d on d.""Id""=a.""CareerLevelId"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' group by a.""ProvinceId""
                                ) as bc on Level=a.""ProvinceId""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 7)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId""||'$'||c.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId"" as ""ParentId"",'CareerLevel' as ""NodeType"",c.""SequenceOrder"" as SequenceOrder, c.""CareerLevel"" as Name,TO_CHAR(c.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_CareerLevel"" as c on c.""Id""=a.""CareerLevelId"" and c.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""CareerLevelId"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""MarketId""='{parent[5]}' group by a.""CareerLevelId""
                                ) as bc on Level=a.""CareerLevelId""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""MarketId""='{parent[5]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 8)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId""||'$'||a.""CareerLevelId""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId""||'$'||a.""CareerLevelId"" as ""ParentId"",'Department' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""DepartmentName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRDepartment"" as d on d.""Id""=a.""DepartmentId"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""DepartmentId"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRJob"" as d on d.""Id""=a.""JobId"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""MarketId""='{parent[5]}' and a.""ProvinceId""='{parent[6]}' group by a.""DepartmentId""
                                ) as bc on Level=a.""DepartmentId""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""MarketId""='{parent[5]}' and a.""ProvinceId""='{parent[6]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 9)
            {
                var parent = parentId.Split('$');
                query = $@"select distinct a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId""||'$'||a.""CareerLevelId""||'$'||a.""DepartmentId""||'$'||a.""JobId"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId""||'$'||a.""CareerLevelId""||'$'||a.""DepartmentId"" as ""ParentId"",'Job' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""JobTitle"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type,bc.Count as DirectChildCount
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRJob"" as d on d.""Id""=a.""JobId"" and d.""IsDeleted""=false 
                                left join (
                                    select count(distinct d.*) as Count,a.""JobId"" as Level 
                                    from  cms.""N_CoreHR_HRAssignment"" as a
                                    join cms.""N_CoreHR_HRPerson"" as d on d.""Id""=a.""EmployeeId"" and d.""IsDeleted""=false 
	                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""MarketId""='{parent[5]}' and a.""ProvinceId""='{parent[6]}' and a.""CareerLevelId""='{parent[7]}' group by a.""JobId""
                                ) as bc on Level=a.""JobId""
                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""MarketId""='{parent[5]}' and a.""ProvinceId""='{parent[6]}' and a.""CareerLevelId""='{parent[7]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            else if (levelUpto == 10)
            {
                var parent = parentId.Split('$');
                query = $@"select a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId""||'$'||a.""CareerLevelId""||'$'||a.""DepartmentId""||'$'||a.""JobId""||'$'||d.""Id"" as ""Id"", a.""OrgLevel1Id""||'$'||a.""OrgLevel2Id""||'$'||a.""OrgLevel3Id""||'$'||a.""OrgLevel4Id""||'$'||a.""BrandId""||'$'||a.""MarketId""||'$'||a.""ProvinceId""||'$'||a.""CareerLevelId""||'$'||a.""DepartmentId""||'$'||a.""JobId"" as ""ParentId"",'Employee' as ""NodeType"",d.""SequenceOrder"" as SequenceOrder, d.""PersonFullName"" as Name,TO_CHAR(d.""CreatedDate""::TIMESTAMP::DATE, 'dd.mm.yyyy') as CreatedDate,'Parent' as Type
                                from  cms.""N_CoreHR_HRAssignment"" as a
                                join cms.""N_CoreHR_HRPerson"" as d on d.""Id""=a.""EmployeeId"" and d.""IsDeleted""=false 

                                where a.""OrgLevel1Id""='{parent[0]}' and a.""OrgLevel2Id""='{parent[1]}' and a.""OrgLevel3Id""='{parent[2]}' and a.""OrgLevel4Id""='{parent[3]}' and a.""BrandId""='{parent[4]}' and a.""MarketId""='{parent[5]}' and a.""ProvinceId""='{parent[6]}' and a.""CareerLevelId""='{parent[7]}' and a.""DepartmentId""='{parent[8]}' and a.""JobId""='{parent[9]}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                                ";

                var queryData = await _queryRepo3.ExecuteQueryList<UserHierarchyChartViewModel>(query, null);

                list = queryData;
            }
            foreach (var x in list)
            {
                x.Count = list.Where(x => x.ParentId == x.Id).Count();
            }
            return list;
        }
        public async Task<List<IdNameViewModel>> GetNonExistingUser(string hierarchyId, string userId)
        {
            string query = $@" select u.""Id"" as Id ,u.""Name"" as Name
                            from public.""User"" as u
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
where ((u.""IsDeleted""=false and  u.""CompanyId""='{_userContext.CompanyId}') and u.""Id"" not in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}') and u.""Id"" not in 
	  (SELECT ""UserId"" FROM cms.""N_GENERAL_UserHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}') 
and u.""Id"" not in 	  (SELECT ""ParentUserId"" FROM cms.""N_GENERAL_UserHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' and ""IsDeleted""=false and ""ParentUserId"" is not null)) -- or u.""Id""='{userId}' 
and up.""PortalId""='{_userContext.PortalId}'";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
        }
        public async Task BulkUploadEmployeeData(string attachId, string noteId)
        {
            var errorList = new List<string>();
            var noteTempModel = new NoteTemplateViewModel();

            var attachment = await _fileBusiness.GetFileByte(attachId);
            Stream stream = new MemoryStream(attachment);

            noteTempModel.NoteId = noteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var UploadStatus = rowData1.ContainsKey("ExecutionStatus") ? Convert.ToString(rowData1["ExecutionStatus"]) : "";

            var titlelist = await _lovBusiness.GetList(x => x.LOVType == "LOV_PERSON_TITLE");
            var genderlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_GENDER");
            var religionlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_RELIGION");
            var MaritalStatuslist = await _lovBusiness.GetList(x => x.LOVType == "LOV_MARITAL_STATUS");
            var personstatus = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_PERSON_STATUS" && x.Name == "Active");
            var countrylist = await GetCountryList();
            var Nationalitylist = await GetNationalityList();
            var relationlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_RELATIONSHIP");
            try
            {

                using (var sl = new SLDocument(stream))
                {
                    var stats = sl.GetWorksheetStatistics();
                    var i = 2;

                    while (i <= stats.EndRowIndex)
                    {
                        //var fields = parser.ReadFields().ToList();
                        try
                        {
                            var title = sl.GetCellValueAsString(i, 1).Trim(); //fields[0];
                            var firstname = sl.GetCellValueAsString(i, 2).Trim();  //fields[1];
                            var middlename = sl.GetCellValueAsString(i, 3).Trim(); //fields[2];
                            var lastname = sl.GetCellValueAsString(i, 4).Trim(); // fields[3];
                            var email = sl.GetCellValueAsString(i, 5).Trim(); // fields[4];
                            var gender = sl.GetCellValueAsString(i, 6).Trim(); //fields[5];
                            var dateofbirth = sl.GetCellValueAsDateTime(i, 7);  //fields[6];
                            var maritalStatus = sl.GetCellValueAsString(i, 8).Trim(); //fields[7];
                            var nationality = sl.GetCellValueAsString(i, 9).Trim(); //fields[8];
                            var religion = sl.GetCellValueAsString(i, 10).Trim(); //fields[9];
                            var dateofjoin = sl.GetCellValueAsDateTime(i, 11); //fields[10];
                            var iqhamaNo = sl.GetCellValueAsString(i, 12).Trim(); //fields[11];
                            var biometric = sl.GetCellValueAsString(i, 13).Trim();  //fields[12];
                            var personNo = sl.GetCellValueAsString(i, 14).Trim(); //fields[12];
                            var sponsorshipNo = sl.GetCellValueAsString(i, 15).Trim(); // fields[14];
                            var presentunit = sl.GetCellValueAsString(i, 16).Trim(); //fields[15];
                            var presentbuilding = sl.GetCellValueAsString(i, 17).Trim();  //fields[16];
                            var presentstreet = sl.GetCellValueAsString(i, 18).Trim(); //fields[17];
                            var presentcity = sl.GetCellValueAsString(i, 19).Trim(); //fields[18];
                            var presentpostcode = sl.GetCellValueAsString(i, 20).Trim(); //fields[19];
                            var presentAdditionalNumber = sl.GetCellValueAsString(i, 21).Trim(); //fields[20];
                            var PresentNeighbourName = sl.GetCellValueAsString(i, 22).Trim(); //fields[21];
                            var PresentCountry = sl.GetCellValueAsString(i, 23).Trim();  //fields[22];
                            var PermanentUnitNumber = sl.GetCellValueAsString(i, 24).Trim(); //fields[23];
                            var PermanentBuildingNumber = sl.GetCellValueAsString(i, 25).Trim(); // fields[24];
                            var PermanentStreetName = sl.GetCellValueAsString(i, 26).Trim(); //fields[25];
                            var PermanentCity = sl.GetCellValueAsString(i, 27).Trim();  //fields[26];
                            var PermanentPostalCode = sl.GetCellValueAsString(i, 28).Trim(); //fields[27];
                            var PermanentAdditionalNumber = sl.GetCellValueAsString(i, 29).Trim(); //fields[28];
                            var PermanentNeighbourName = sl.GetCellValueAsString(i, 30).Trim(); //fields[29];
                            var PermanentCountry = sl.GetCellValueAsString(i, 31).Trim(); //fields[30];
                            var PersonalEmail = sl.GetCellValueAsString(i, 32).Trim(); //fields[31];
                            var MobileNumber = sl.GetCellValueAsString(i, 33).Trim();  //fields[32];
                            var Country = sl.GetCellValueAsString(i, 34).Trim(); //fields[33];
                            var EmergencyContactName1 = sl.GetCellValueAsString(i, 35).Trim(); // fields[34];
                            var EmergencyContactMobile1 = sl.GetCellValueAsString(i, 36).Trim(); //fields[35];
                            var EmergencyContactRelation1 = sl.GetCellValueAsString(i, 37).Trim();  //fields[36];
                            var EmergencyContactOtherRelation1 = sl.GetCellValueAsString(i, 38).Trim(); //fields[37];
                            var EmergencyContactCountry1 = sl.GetCellValueAsString(i, 39).Trim(); //fields[38];
                            var EmergencyContactName2 = sl.GetCellValueAsString(i, 40).Trim(); //fields[39];
                            var EmergencyContactMobile2 = sl.GetCellValueAsString(i, 41).Trim(); //fields[40];
                            var EmergencyContactRelation2 = sl.GetCellValueAsString(i, 42).Trim(); //fields[41];
                            var EmergencyContactOtherRelation2 = sl.GetCellValueAsString(i, 43).Trim(); //fields[42];
                            var EmergencyContactCountry2 = sl.GetCellValueAsString(i, 44).Trim(); //fields[43];

                            var userId = "";
                            var user = await _userBusiness.GetSingle(x => x.Email == email && x.IsDeleted == false);
                            if (user == null)
                            {
                                var umodel = new UserViewModel
                                {
                                    Name = middlename.IsNotNullAndNotEmpty() ? firstname + " " + middlename + " " + lastname : firstname + " " + lastname,
                                    Email = email
                                };
                                var userresult = await _userBusiness.Create(umodel);
                                if (userresult.IsSuccess)
                                {
                                    userId = userresult.Item.Id;
                                }
                            }
                            else
                            {
                                userId = user.Id;
                            }




                            var cmodel = new NoteTemplateViewModel
                            {
                                TemplateCode = "HRPerson"
                            };
                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            cnote.StartDate = DateTime.Now;
                            cnote.DataAction = Common.DataActionEnum.Create;

                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                            var titleid = titlelist.Where(x => x.Name == title).FirstOrDefault();
                            if (titleid != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("TitleId", titleid.Id);
                            }
                            var fullName = firstname + " " + lastname;
                            if (middlename.IsNotNullAndNotEmpty())
                            {
                                fullName = firstname + " " + middlename + " " + lastname;
                            }
                                ((IDictionary<String, Object>)exo1).Add("FirstName", firstname);
                            ((IDictionary<String, Object>)exo1).Add("MiddleName", middlename);
                            ((IDictionary<String, Object>)exo1).Add("LastName", lastname);
                            ((IDictionary<String, Object>)exo1).Add("PersonFullName", fullName);
                            var genderid = genderlist.Where(x => x.Name == gender).FirstOrDefault();
                            if (genderid != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("GenderId", genderid.Id);
                            }
                            var religionid = religionlist.Where(x => x.Name == religion).FirstOrDefault();
                            if (religionid != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("ReligionId", religionid.Id);
                            }
                                ((IDictionary<String, Object>)exo1).Add("DateOfBirth", dateofbirth);
                            ((IDictionary<String, Object>)exo1).Add("IqamahNoNationalId", iqhamaNo);
                            ((IDictionary<String, Object>)exo1).Add("PersonNo", personNo);
                            var MaritalStatusId = MaritalStatuslist.Where(x => x.Name == maritalStatus).FirstOrDefault();
                            if (MaritalStatusId != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("MaritalStatusId", MaritalStatusId.Id);
                            }

                            var NationalityId = Nationalitylist.Where(x => x.Name == nationality).FirstOrDefault();
                            if (NationalityId != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("NationalityId", NationalityId.Id);
                            }
                                ((IDictionary<String, Object>)exo1).Add("DateOfJoin", dateofjoin);
                            ((IDictionary<String, Object>)exo1).Add("BiometricId", biometric);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressUnitNumber", presentunit);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressBuildingNumber", presentbuilding);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressStreetName", presentstreet);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressCityOrTown", presentcity);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressPostalCode", presentpostcode);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressAdditionalNumber", presentAdditionalNumber);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressNeighbourName", PresentNeighbourName);
                            ((IDictionary<String, Object>)exo1).Add("PresentAddressCountryId", PresentCountry);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressUnitNumber", PermanentUnitNumber);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressBuildingNumber", PermanentBuildingNumber);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressStreetName", PermanentStreetName);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressCityOrTown", PermanentCity);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressPostalCode", PermanentPostalCode);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressAdditionalNumber", PermanentAdditionalNumber);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressNeighbourName", PermanentNeighbourName);
                            ((IDictionary<String, Object>)exo1).Add("PermanentAddressCountryId", PermanentCountry);
                            ((IDictionary<String, Object>)exo1).Add("ContactPersonalEmail", PersonalEmail);
                            ((IDictionary<String, Object>)exo1).Add("MobileNumber", MobileNumber);
                            ((IDictionary<String, Object>)exo1).Add("UserId", userId);

                            var countryl = countrylist.Where(x => x.Name == Country).FirstOrDefault();
                            if (countryl != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("ContactCountryId", countryl.Id);
                            }
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContactName1", EmergencyContactName1);
                            var relation = relationlist.Where(x => x.Name == EmergencyContactRelation1).FirstOrDefault();
                            if (relation != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContact1RelationshipId", relation.Id);
                            }
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContactMobileNumber1", EmergencyContactMobile1);
                            ((IDictionary<String, Object>)exo1).Add("EmergencyContact1OtherRelation", EmergencyContactOtherRelation1);

                            var ecountryl = countrylist.Where(x => x.Name == EmergencyContactCountry1).FirstOrDefault();
                            if (ecountryl != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContact1CountryId", ecountryl.Id);
                            }
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContactName2", EmergencyContactName2);
                            var relation2 = relationlist.Where(x => x.Name == EmergencyContactRelation2).FirstOrDefault();
                            if (relation2 != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContact2RelationshipId", relation2.Id);
                            }
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContactMobileNumber2", EmergencyContactMobile2);
                            ((IDictionary<String, Object>)exo1).Add("EmergencyContact2OtherRelation", EmergencyContactOtherRelation2);
                            var ecountryl2 = countrylist.Where(x => x.Name == EmergencyContactCountry2).FirstOrDefault();
                            if (ecountryl2 != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("EmergencyContact2CountryId", ecountryl2.Id);
                            }
                                ((IDictionary<String, Object>)exo1).Add("SponsorshipNo", sponsorshipNo);

                            if (personstatus != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("PersonalStatusId", personstatus.Id);
                            }

                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                            var result1 = await _noteBusiness.ManageNote(cnote);
                            if (!result1.IsSuccess)
                            {
                                errorList.AddRange(result1.Messages.Values);
                            }

                        }
                        catch (Exception ex)
                        {
                            errorList.Add(string.Concat("Error Person Data :", sl.GetCellValueAsString(i, 1).Trim()));
                            errorList.Add(string.Concat("Error :", ex.Message));
                        }
                        // errorList.Add(string.Concat("Success Questionnaire :", fields[0]));
                        i++;
                    }
                }

                if (UploadStatus.IsNotNull() && errorList.Count() > 0)
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = string.Join(",", errorList);//.ScheduleInprogress;
                    rowData1["ErrorCount"] = errorList.Count();
                    rowData1["ExecutionEndTime"] = DateTime.Now;

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Completed.ToString()));//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }

            }
            catch (Exception ex)
            {
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = ex.Message;//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
            }
        }

        public async Task BulkUploadJobData(string attachId, string noteId)
        {
            var errorList = new List<string>();
            var noteTempModel = new NoteTemplateViewModel();
            var attachment = await _fileBusiness.GetFileByte(attachId);
            Stream stream = new MemoryStream(attachment);
            noteTempModel.NoteId = noteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var UploadStatus = rowData1.ContainsKey("ExecutionStatus") ? Convert.ToString(rowData1["ExecutionStatus"]) : "";
            try
            {

                using (var sl = new SLDocument(stream))
                {
                    var stats = sl.GetWorksheetStatistics();
                    var i = 2;

                    while (i <= stats.EndRowIndex)
                    {
                        //var fields = parser.ReadFields().ToList();
                        try
                        {

                            var job = sl.GetCellValueAsString(i, 1).Trim(); //fields[0];
                            var grade = sl.GetCellValueAsString(i, 2).Trim();  //fields[1];
                            var desc = sl.GetCellValueAsString(i, 3).Trim(); //fields[2];
                            var arabic = sl.GetCellValueAsString(i, 4).Trim(); // fields[3];
                            var comment = sl.GetCellValueAsString(i, 5).Trim(); //fields[4];                           

                            var cmodel = new NoteTemplateViewModel
                            {
                                TemplateCode = "HRJob"
                            };
                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            cnote.StartDate = DateTime.Now;
                            cnote.DataAction = Common.DataActionEnum.Create;

                            dynamic exo1 = new System.Dynamic.ExpandoObject();


                            ((IDictionary<String, Object>)exo1).Add("JobTitle", job);
                            ((IDictionary<String, Object>)exo1).Add("JobTitleInArabic", arabic);
                            var gradelist = await GetGradeList();
                            var gradeid = gradelist.Where(x => x.Name == grade).FirstOrDefault();
                            if (gradeid != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("GradeId", gradeid.Id);
                            }
                            ((IDictionary<String, Object>)exo1).Add("JobDescription", desc);
                            ((IDictionary<String, Object>)exo1).Add("Comment", comment);

                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                            var result1 = await _noteBusiness.ManageNote(cnote);
                            if (!result1.IsSuccess)
                            {
                                errorList.AddRange(result1.Messages.Values);
                            }

                        }

                        catch (Exception ex)
                        {
                            errorList.Add(string.Concat("Error Job Data :", sl.GetCellValueAsString(i, 1).Trim()));
                            errorList.Add(string.Concat("Error :", ex.Message));
                        }
                        // errorList.Add(string.Concat("Success Questionnaire :", fields[0]));
                        i++;
                    }
                }

                if (UploadStatus.IsNotNull() && errorList.Count() > 0)
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = string.Join(",", errorList);//.ScheduleInprogress;
                    rowData1["ErrorCount"] = errorList.Count();
                    rowData1["ExecutionEndTime"] = DateTime.Now;

                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Completed.ToString()));//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }


            }
            catch (Exception ex)
            {
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = ex.Message;//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
            }

        }


        public async Task BulkUploadBusinessHierarchyData(string attachId, string noteId)
        {
            var errorList = new List<string>();
            var noteTempModel = new NoteTemplateViewModel();
            var attachment = await _fileBusiness.GetFileByte(attachId);
            Stream stream = new MemoryStream(attachment);
            noteTempModel.NoteId = noteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var UploadStatus = rowData1.ContainsKey("ExecutionStatus") ? Convert.ToString(rowData1["ExecutionStatus"]) : "";
            try
            {
                var hm = await _repo.GetSingle<HierarchyMasterViewModel, HierarchyMaster>(x => x.Code == "BUSINESS_HIERARCHY");
                var hierarchyMasterId = "";
                var itemId = "";
                var parentId = "";
                if (hm != null)
                {
                    hierarchyMasterId = hm.Id;
                }
                var deparmentQuery = $@"select d.""Id"",d.""DepartmentName"",
                typ.""Code"" ""DepartmentTypeCode"",lvl.""Code"" ""DepartmentLevelCode""
                from cms.""N_CoreHR_HRDepartment"" d
                left join public.""LOV"" typ on d.""DepartmentTypeId""=typ.""Id""
                left join public.""LOV"" lvl on d.""DepartmentLevelId""=lvl.""Id""                
                where d.""IsDeleted"" =false";
                var departmentList = await _queryRepo1.ExecuteQueryList<DepartmentViewModel>(deparmentQuery, null);
                var jobQuery = $@"select ""Id"",""JobTitle"" from cms.""N_CoreHR_HRJob"" where ""IsDeleted"" =false";
                var jobList = await _queryRepo1.ExecuteQueryList<JobViewModel>(jobQuery, null);
                var clQuery = $@"select ""Id"",""CareerLevel"" from cms.""N_CoreHR_CareerLevel"" where ""IsDeleted"" =false";
                var clList = await _queryRepo1.ExecuteQueryList<CareerLevelViewModel>(clQuery, null);
                var personQuery = $@"select ""Id"",""FirstName"" as ""FirstName"",""LastName"",""PersonFullName"",""PersonNo"" from cms.""N_CoreHR_HRPerson"" where ""IsDeleted""=false";
                var personList = await _queryRepo1.ExecuteQueryList<PersonViewModel>(personQuery, null);
                var hierarchyList = await _repo.GetList<HybridHierarchyViewModel, HybridHierarchy>();
                var list = new List<string>();

                using (var sl = new SLDocument(stream))
                {
                    var stats = sl.GetWorksheetStatistics();
                    var i = 2;


                    while (i <= stats.EndRowIndex)
                    {
                        try
                        {
                            var itemName = sl.GetCellValueAsString(i, 1).Trim();
                            var itemType = sl.GetCellValueAsString(i, 2).Trim();
                            var parentName = sl.GetCellValueAsString(i, 3).Trim();
                            var parentType = sl.GetCellValueAsString(i, 4).Trim();
                            itemId = "";
                            parentId = "";
                            var hybridParentId = "";
                            switch (parentType)
                            {
                                case "ROOT":
                                    hybridParentId = "-1";
                                    break;
                                case "LEVEL1":
                                case "LEVEL2":
                                case "LEVEL3":
                                case "LEVEL4":
                                    if (parentName.Contains("|"))
                                    {
                                        var depLevel0 = GetDepartmentParentByPath(parentType, parentName, departmentList, hierarchyList, clList, jobList, personList);
                                        if (depLevel0 != null)
                                        {
                                            hybridParentId = depLevel0.Id;
                                        }
                                    }
                                    else
                                    {
                                        var depLevel = departmentList.FirstOrDefault(x => x.DepartmentLevelCode == parentType && x.DepartmentName == parentName);
                                        if (depLevel != null)
                                        {
                                            parentId = depLevel.Id;
                                        }
                                    }

                                    break;
                                case "BRAND":
                                case "MARKET":
                                case "PROVINCE":
                                case "DEPARTMENT":
                                    if (parentName.Contains("|"))
                                    {
                                        var depLevel0 = GetDepartmentParentByPath(parentType, parentName, departmentList, hierarchyList, clList, jobList, personList);
                                        if (depLevel0 != null)
                                        {
                                            hybridParentId = depLevel0.Id;
                                        }
                                    }
                                    else
                                    {
                                        var depType = departmentList.FirstOrDefault(x => x.DepartmentTypeCode == parentType && x.DepartmentName == parentName);
                                        if (depType != null)
                                        {
                                            parentId = depType.Id;
                                        }
                                    }
                                    break;
                                case "CAREER_LEVEL":
                                    if (parentName.Contains("|"))
                                    {
                                        var career0 = GetDepartmentParentByPath(parentType, parentName, departmentList, hierarchyList, clList, jobList, personList);
                                        if (career0 != null)
                                        {
                                            hybridParentId = career0.Id;
                                        }
                                    }
                                    else
                                    {
                                        var career = clList.FirstOrDefault(x => x.CareerLevel == parentName);
                                        if (career != null)
                                        {
                                            parentId = career.Id;
                                        }
                                    }

                                    break;
                                case "JOB":
                                    if (parentName.Contains("|"))
                                    {
                                        var job0 = GetDepartmentParentByPath(parentType, parentName, departmentList, hierarchyList, clList, jobList, personList);
                                        if (job0 != null)
                                        {
                                            hybridParentId = job0.Id;
                                        }
                                    }
                                    else
                                    {
                                        var job = jobList.FirstOrDefault(x => x.JobTitle == parentName);
                                        if (job != null)
                                        {
                                            parentId = job.Id;
                                        }
                                    }

                                    break;
                                case "EMPLOYEE":
                                    if (parentName.Contains("|"))
                                    {
                                        var emp0 = GetDepartmentParentByPath(parentType, parentName, departmentList, hierarchyList, clList, jobList, personList);
                                        if (emp0 != null)
                                        {
                                            hybridParentId = emp0.Id;
                                        }
                                    }
                                    else
                                    {
                                        var emp = personList.FirstOrDefault(x => x.PersonFullName == parentName);
                                        if (emp != null)
                                        {
                                            parentId = emp.Id;
                                        }
                                    }

                                    break;
                                default:
                                    parentId = "-1";
                                    break;

                            }
                            if (hybridParentId.IsNullOrEmpty())
                            {
                                var hybridParentQuery = $@"select ""Id"" from public.""HybridHierarchy"" where ""ReferenceType"" = '{parentType}' and ""ReferenceId"" = '{parentId}' ";
                                hybridParentId = await _queryRepo1.ExecuteScalar<string>(hybridParentQuery, null);
                            }
                            switch (itemType)
                            {
                                case "ROOT":
                                    itemId = "-1";
                                    break;
                                case "LEVEL1":
                                case "LEVEL2":
                                case "LEVEL3":
                                case "LEVEL4":
                                    var depLevel = departmentList.FirstOrDefault(x => x.DepartmentLevelCode == itemType && x.DepartmentName == itemName);
                                    if (depLevel != null)
                                    {
                                        itemId = depLevel.Id;
                                    }
                                    break;
                                case "BRAND":
                                case "MARKET":
                                case "PROVINCE":
                                case "DEPARTMENT":
                                    var depType = departmentList.FirstOrDefault(x => x.DepartmentTypeCode == itemType && x.DepartmentName == itemName);
                                    if (depType != null)
                                    {
                                        itemId = depType.Id;
                                    }
                                    break;
                                case "CAREER_LEVEL":
                                    var career = clList.FirstOrDefault(x => x.CareerLevel == itemName);
                                    if (career != null)
                                    {
                                        itemId = career.Id;
                                    }
                                    break;
                                case "JOB":
                                    var job = jobList.FirstOrDefault(x => x.JobTitle == itemName);
                                    if (job != null)
                                    {
                                        itemId = job.Id;
                                    }
                                    break;
                                case "EMPLOYEE":
                                    var emp = personList.FirstOrDefault(x => x.PersonNo == itemName);
                                    if (emp != null)
                                    {
                                        itemId = emp.Id;
                                    }
                                    break;
                                default:
                                    itemId = "-1";
                                    break;

                            }
                            if (itemId.IsNotNullAndNotEmpty() && hybridParentId.IsNotNullAndNotEmpty())
                            {
                                var hybridmodel = new HybridHierarchyViewModel()
                                {
                                    HierarchyMasterId = hierarchyMasterId,
                                    ParentId = hybridParentId,
                                    ReferenceType = itemType,
                                    ReferenceId = itemId
                                };
                                await _hybridHierarchyBusiness.Create(hybridmodel);

                            }
                            else
                            {
                                list.Add(string.Concat(parentName, "|", itemName));
                            }
                        }
                        catch (Exception ex)
                        {
                            errorList.Add(string.Concat("Error Business Hierarchy Data :", sl.GetCellValueAsString(i, 1).Trim()));
                            errorList.Add(string.Concat("Error :", ex.Message));
                        }

                        i++;
                    }
                }

                if (UploadStatus.IsNotNull() && errorList.Count() > 0)
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = string.Join(",", errorList);//.ScheduleInprogress;
                    rowData1["ErrorCount"] = errorList.Count();
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Completed.ToString()));//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }

            }
            catch (Exception ex)
            {
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = ex.Message;//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
            }
        }



        //private HybridHierarchyViewModel GetCarerLevelParentByPath(string parentType, string parentName, List<DepartmentViewModel> departmentList, List<HybridHierarchyViewModel> hierarchyList, List<CareerLevelViewModel> clList)
        //{

        //    var parents = parentName.Split('|').ToList();
        //    var cl = parents[0];
        //    parents = parentName.Split('|').ToList().Skip(1).ToList();
        //    parents.Reverse();
        //    var i = 0;
        //    var parent = parents[i++];
        //    var depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent); ;
        //    HybridHierarchyViewModel hyrarchy = null;
        //    List<HybridHierarchyViewModel> children = null;
        //    if (depLevel != null)
        //    {
        //        hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
        //    }
        //    while (i < parents.Count)
        //    {
        //        parent = parents[i++];
        //        if (depLevel != null)
        //        {
        //            hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
        //            if (hyrarchy != null)
        //            {
        //                children = hierarchyList.Where(x => x.ParentId == hyrarchy.Id).ToList();

        //                depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent);
        //                if (depLevel != null)
        //                {
        //                    hyrarchy = children.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
        //                }

        //            }

        //        }
        //    }
        //    if (hyrarchy != null)
        //    {
        //        children = hierarchyList.Where(x => x.ParentId == hyrarchy.Id).ToList();
        //        var careerLevel = clList.FirstOrDefault(x => x.CareerLevel == cl);
        //        if (careerLevel != null)
        //        {
        //            hyrarchy = children.FirstOrDefault(x => x.ReferenceId == careerLevel.Id);
        //        }
        //    }


        //    return hyrarchy;




        //    //var parents = parentName.Split('|').ToList();
        //    //parents.Reverse();
        //    //var i = 0;
        //    //var parent = parents[i++];
        //    //var depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent); ;
        //    //HybridHierarchyViewModel hyrarchy = null;
        //    //List<HybridHierarchyViewModel> children = null;
        //    //if (depLevel != null)
        //    //{
        //    //    hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
        //    //}
        //    //while (i < parents.Count)
        //    //{
        //    //    parent = parents[i++];
        //    //    if (depLevel != null)
        //    //    {
        //    //        hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
        //    //        if (hyrarchy != null)
        //    //        {
        //    //            children = hierarchyList.Where(x => x.ParentId == hyrarchy.Id).ToList();

        //    //            depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent);
        //    //            if (i == parents.Count - 1)
        //    //            {
        //    //                var cl = clList.FirstOrDefault(x => x.CareerLevel == parent);
        //    //                if (cl != null)
        //    //                {
        //    //                    depLevel = new DepartmentViewModel { Id = cl.Id };
        //    //                }
        //    //            }
        //    //            if (depLevel != null)
        //    //            {
        //    //                hyrarchy = children.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
        //    //            }

        //    //        }

        //    //    }
        //    //}

        //    //return hyrarchy;

        //}

        private HybridHierarchyViewModel GetDepartmentParentByPath(string parentType, string parentName, List<DepartmentViewModel> departmentList, List<HybridHierarchyViewModel> hierarchyList, List<CareerLevelViewModel> clList, List<JobViewModel> jobList, List<PersonViewModel> personList)
        {
            var parents = parentName.Split('|').ToList();
            HybridHierarchyViewModel hyrarchy = null;
            hyrarchy = GetHierarchy(0, parents, departmentList, hierarchyList, hyrarchy, clList, jobList, personList);
            //while (i < parents.Count)
            //{
            //    parent = parents[i++];
            //    depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent);
            //    if (depLevel != null)
            //    {
            //        var refType = depLevel.DepartmentLevelCode.IsNotNullAndNotEmpty() ? depLevel.DepartmentLevelCode : depLevel.DepartmentTypeCode;
            //        hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id && x.ReferenceType == refType);
            //        if (hyrarchy != null)
            //        {
            //            children = hierarchyList.Where(x => x.ParentId == hyrarchy.Id).ToList();
            //            if (i < parents.Count)
            //            {
            //                depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parents[i]);
            //                if (depLevel != null)
            //                {
            //                    hyrarchy = children.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
            //                }
            //            }
            //        }
            //    }
            //}
            return hyrarchy;



            //var parents = parentName.Split('|').ToList();
            //parents.Reverse();
            //var i = 0;
            //var parent = parents[i++];
            //var depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent); ;
            //HybridHierarchyViewModel hyrarchy = null;
            //List<HybridHierarchyViewModel> children = null;
            //if (depLevel != null)
            //{
            //    hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
            //}
            //while (i < parents.Count)
            //{
            //    parent = parents[i++];
            //    if (depLevel != null)
            //    {
            //        hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
            //        if (hyrarchy != null)
            //        {
            //            children = hierarchyList.Where(x => x.ParentId == hyrarchy.Id).ToList();

            //            depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent);
            //            if (depLevel != null)
            //            {
            //                hyrarchy = children.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
            //            }

            //        }

            //    }
            //}

            //return hyrarchy;

        }

        private HybridHierarchyViewModel GetHierarchy(int count, List<string> parents, List<DepartmentViewModel> departmentList, List<HybridHierarchyViewModel> hierarchyList, HybridHierarchyViewModel hyrarchy, List<CareerLevelViewModel> clList, List<JobViewModel> jobList, List<PersonViewModel> personList)
        {
            if (parents.Count <= count)
            {
                return hyrarchy;
            }
            var parent = parents[count];
            var depLevel = departmentList.FirstOrDefault(x => x.DepartmentName == parent);
            if (depLevel == null)
            {
                var cLevel = clList.FirstOrDefault(x => x.CareerLevel == parent);
                if (cLevel != null)
                {
                    depLevel = new DepartmentViewModel { Id = cLevel.Id };
                }
            }
            if (depLevel == null)
            {
                var cLevel = jobList.FirstOrDefault(x => x.JobTitle == parent);
                if (cLevel != null)
                {
                    depLevel = new DepartmentViewModel { Id = cLevel.Id };
                }
            }
            if (depLevel != null)
            {
                if (hyrarchy == null)
                {
                    hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id);
                }
                else
                {
                    hyrarchy = hierarchyList.FirstOrDefault(x => x.ReferenceId == depLevel.Id && x.ParentId == hyrarchy.Id);
                }
            }

            hyrarchy = GetHierarchy(count + 1, parents, departmentList, hierarchyList, hyrarchy, clList, jobList, personList);
            return hyrarchy;
        }

        public async Task BulkUploadDepartmentData(string attachId, string noteId)
        {
            var errorList = new List<string>();
            var noteTempModel = new NoteTemplateViewModel();
            var attachment = await _fileBusiness.GetFileByte(attachId);
            Stream stream = new MemoryStream(attachment);
            noteTempModel.NoteId = noteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var UploadStatus = rowData1.ContainsKey("ExecutionStatus") ? Convert.ToString(rowData1["ExecutionStatus"]) : "";
            try
            {

                using (var sl = new SLDocument(stream))
                {
                    var stats = sl.GetWorksheetStatistics();
                    var i = 2;

                    while (i <= stats.EndRowIndex)
                    {
                        //var fields = parser.ReadFields().ToList();
                        try
                        {

                            var name = sl.GetCellValueAsString(i, 1).Trim(); //fields[0];
                            var costcenter = sl.GetCellValueAsString(i, 2).Trim();  //fields[1];
                            var arabic = sl.GetCellValueAsString(i, 3).Trim(); //fields[2];
                            var desc = sl.GetCellValueAsString(i, 4).Trim(); // fields[3];
                            var documentcode = sl.GetCellValueAsString(i, 5).Trim(); //fields[4];
                            var category = sl.GetCellValueAsString(i, 6).Trim();  //fields[1];
                            var owner = sl.GetCellValueAsString(i, 7).Trim(); //fields[2];
                            var responsibity = sl.GetCellValueAsString(i, 8).Trim(); // fields[3];
                            var location = sl.GetCellValueAsString(i, 9).Trim(); //fields[4];                                                                                    //  var job = sl.GetCellValueAsString(i, 1).Trim(); //fields[0];
                            var departmenttype = sl.GetCellValueAsString(i, 10).Trim();  //fields[1];
                            var departmentlevel = sl.GetCellValueAsString(i, 11).Trim(); //fields[2]; 

                            var cmodel = new NoteTemplateViewModel
                            {
                                TemplateCode = "HRDepartment"
                            };
                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            cnote.StartDate = DateTime.Now;
                            cnote.DataAction = Common.DataActionEnum.Create;

                            dynamic exo1 = new System.Dynamic.ExpandoObject();


                            ((IDictionary<String, Object>)exo1).Add("DepartmentName", name);
                            ((IDictionary<String, Object>)exo1).Add("DepartmentNameArabic", arabic);
                            ((IDictionary<String, Object>)exo1).Add("DepartmentDescription", desc);
                            ((IDictionary<String, Object>)exo1).Add("DocumentCode", documentcode);



                            var depcat = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_DEPARTMENT_CATEGORY" && x.Name == category);
                            if (depcat != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("DepartmentCategoryId", depcat.Id);
                            }
                            var depuser = await _userBusiness.GetSingle(x => x.Name == owner);
                            if (depuser != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("DepartmentOwnerUserId", depuser.Id);
                            }
                            var cclist = await GetHRMasterList("HRCostCenter");
                            var costcc = cclist.Where(x => x.Name == costcenter).FirstOrDefault();
                            if (costcc != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("CostCenterId", costcc.Id);
                            }
                            var rclist = await GetHRMasterList("HRResponsibilityCenter");
                            var responsibility = rclist.Where(x => x.Name == responsibity).FirstOrDefault();
                            if (responsibility != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("ResponsibilityCenterId", responsibility.Id);
                            }
                            var locationlist = await GetHRMasterList("HRLocation");
                            var loc = locationlist.Where(x => x.Name == location).FirstOrDefault();
                            if (loc != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("LocationId", loc.Id);
                            }
                            var depType = await _lovBusiness.GetSingle(x => x.LOVType == "DEPARTMENT_TYPE" && x.Name == departmenttype);
                            if (depType != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("DepartmentTypeId", depType.Id);
                            }
                            var deplevel = await _lovBusiness.GetSingle(x => x.LOVType == "DEPARTMENT_LEVEL" && x.Name == departmentlevel);
                            if (deplevel != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("DepartmentLevelId", deplevel.Id);
                            }
                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                            var result1 = await _noteBusiness.ManageNote(cnote);
                            if (!result1.IsSuccess)
                            {
                                errorList.AddRange(result1.Messages.Values);
                            }

                        }

                        catch (Exception ex)
                        {
                            errorList.Add(string.Concat("Error Department Data :", sl.GetCellValueAsString(i, 1).Trim()));
                            errorList.Add(string.Concat("Error :", ex.Message));
                        }
                        // errorList.Add(string.Concat("Success Questionnaire :", fields[0]));
                        i++;
                    }
                }

                if (UploadStatus.IsNotNull() && errorList.Count() > 0)
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = string.Join(",", errorList);//.ScheduleInprogress;
                    rowData1["ErrorCount"] = errorList.Count();
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Completed.ToString()));//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }


            }
            catch (Exception ex)
            {
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = ex.Message;//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
            }

        }

        public async Task BulkUploadAssignmentData(string attachId, string noteId)
        {
            var errorList = new List<string>();
            var noteTempModel = new NoteTemplateViewModel();
            var attachment = await _fileBusiness.GetFileByte(attachId);
            Stream stream = new MemoryStream(attachment);
            noteTempModel.NoteId = noteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var UploadStatus = rowData1.ContainsKey("ExecutionStatus") ? Convert.ToString(rowData1["ExecutionStatus"]) : "";

            var personlist = await GetHRMasterList("HRPerson");

            var departmentlist = await GetHRMasterList("HRDepartment");
            var joblist = await GetHRMasterList("HRJob");
            var positionlist = await GetHRMasterList("HRPosition");
            var locationlist = await GetHRMasterList("HRLocation");
            var gradelist = await GetHRMasterList("HRGrade");
            var assignTypelist = await _lovBusiness.GetList(x => x.LOVType == "LOV_ASSIGNMENT_TYPE");
            var probationlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_PROBATION_PERIOD");
            var assignstatuslist = await _lovBusiness.GetList(x => x.LOVType == "LOV_ASSIGNMENT_STATUS");

            try
            {

                using (var sl = new SLDocument(stream))
                {
                    var stats = sl.GetWorksheetStatistics();
                    var i = 2;

                    while (i <= stats.EndRowIndex)
                    {
                        //var fields = parser.ReadFields().ToList();
                        try
                        {

                            var employee = sl.GetCellValueAsString(i, 1).Trim(); //fields[0];
                            var email = sl.GetCellValueAsString(i, 2).Trim(); //fields[1];
                            var department = sl.GetCellValueAsString(i, 3).Trim();  //fields[2];
                            var job = sl.GetCellValueAsString(i, 4).Trim(); //fields[3];                                
                            var location = sl.GetCellValueAsString(i, 5).Trim();  //fields[4];
                            var grade = sl.GetCellValueAsString(i, 6).Trim(); //fields[5];
                            var assignmenttype = sl.GetCellValueAsString(i, 7).Trim(); // fields[6];                             
                            var dateofjoin = sl.GetCellValueAsString(i, 8).Trim(); //fields[7];
                            var assignmentstatus = sl.GetCellValueAsString(i, 9).Trim(); // fields[8];
                            var probationperiod = sl.GetCellValueAsString(i, 10).Trim(); //fields[9];
                            var noticeperiod = sl.GetCellValueAsString(i, 11).Trim();  //fields[10];


                            var cmodel = new NoteTemplateViewModel
                            {
                                TemplateCode = "HRAssignment"
                            };
                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            cnote.StartDate = DateTime.Now;
                            cnote.DataAction = Common.DataActionEnum.Create;

                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                            if (email.IsNotNullAndNotEmpty())
                            {
                                var user = await _userBusiness.GetSingle(x => x.Email == email);
                                var person = await GetPersonDetailByUserId(user.Id);
                                if (person != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("EmployeeId", person.Id);
                                }
                            }

                            //var person = personlist.Where(x => x.Name == employee).FirstOrDefault();
                            //if (person != null)
                            //{
                            //    ((IDictionary<String, Object>)exo1).Add("EmployeeId", person.Id);
                            //}                                

                            var orgdep = departmentlist.Where(x => x.Name == department).FirstOrDefault();
                            if (orgdep != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("DepartmentId", orgdep.Id);
                            }
                            var orgjob = joblist.Where(x => x.Name == job).FirstOrDefault();
                            if (orgjob != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("JobId", orgjob.Id);
                            }
                            //var orgpos = await GenerateNextPositionName(department + "_" + job + "_");
                            var orgpos = await CreatePosition(orgdep.Id, orgjob.Id);
                            if (orgpos != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("PositionId", orgpos.Id);
                            }
                            var orgloc = locationlist.Where(x => x.Name == location).FirstOrDefault();
                            if (orgloc != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("LocationId", orgloc.Id);
                            }
                            var orggrade = gradelist.Where(x => x.Name == grade).FirstOrDefault();
                            if (orggrade != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("AssignmentGradeId", orggrade.Id);
                            }
                            var orgassign = assignTypelist.Where(x => x.Name == assignmenttype).FirstOrDefault();
                            if (orgassign != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("AssignmentTypeId", orgassign.Id);
                            }
                            var orgprob = probationlist.Where(x => x.Name == probationperiod).FirstOrDefault();
                            if (orgprob != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("ProbationPeriodId", orgprob.Id);
                            }

                            ((IDictionary<String, Object>)exo1).Add("NoticePeriod", noticeperiod);
                            ((IDictionary<String, Object>)exo1).Add("DateOfJoin", dateofjoin);
                            var orgassignstatus = assignstatuslist.Where(x => x.Name == "Active Assignment").FirstOrDefault();
                            if (orgassignstatus != null)
                            {
                                ((IDictionary<String, Object>)exo1).Add("AssignmentStatusId", orgassignstatus.Id);
                            }

                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                            var result1 = await _noteBusiness.ManageNote(cnote);
                            if (!result1.IsSuccess)
                            {
                                errorList.AddRange(result1.Messages.Values);
                            }

                        }

                        catch (Exception ex)
                        {
                            errorList.Add(string.Concat("Error Assignment Data :", sl.GetCellValueAsString(i, 1).Trim()));
                            errorList.Add(string.Concat("Error :", ex.Message));
                        }
                        // errorList.Add(string.Concat("Success Questionnaire :", fields[0]));
                        i++;
                    }
                }

                if (UploadStatus.IsNotNull() && errorList.Count() > 0)
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = string.Join(",", errorList);//.ScheduleInprogress;
                    rowData1["ErrorCount"] = errorList.Count();
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Completed.ToString()));//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }


            }
            catch (Exception ex)
            {
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = ex.Message;//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
            }

        }
        public async Task BulkUploadEmployeeAssignmentData(string attachId, string noteId)
        {
            var errorList = new List<string>();
            var noteTempModel = new NoteTemplateViewModel();
            var attachment = await _fileBusiness.GetFileByte(attachId);
            Stream stream = new MemoryStream(attachment);
            noteTempModel.NoteId = noteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var UploadStatus = rowData1.ContainsKey("ExecutionStatus") ? Convert.ToString(rowData1["ExecutionStatus"]) : "";

            var personlist = await GetHRMasterList("HRPerson");

            var departmentlist = await GetHRMasterList("HRDepartment");
            var joblist = await GetHRMasterList("HRJob");
            var positionlist = await GetHRMasterList("HRPosition");
            var locationlist = await GetHRMasterList("HRLocation");
            var gradelist = await GetHRMasterList("HRGrade");
            var sponsorlist = await GetHRMasterList("HRSponsor");

            var assignTypelist = await _lovBusiness.GetList(x => x.LOVType == "LOV_ASSIGNMENT_TYPE");
            var probationlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_PROBATION_PERIOD");
            var assignstatuslist = await _lovBusiness.GetList(x => x.LOVType == "LOV_ASSIGNMENT_STATUS");
            var titlelist = await _lovBusiness.GetList(x => x.LOVType == "LOV_PERSON_TITLE");
            var genderlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_GENDER");
            var religionlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_RELIGION");
            var MaritalStatuslist = await _lovBusiness.GetList(x => x.LOVType == "LOV_MARITAL_STATUS");
            var personstatus = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_PERSON_STATUS" && x.Name == "Active");
            var contractlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_CONTRACT");
            var renewallist = await _lovBusiness.GetList(x => x.LOVType == "LOV_RENEWABLE");
            var countrylist = await GetCountryList();
            var Nationalitylist = await GetNationalityList();
            var relationlist = await _lovBusiness.GetList(x => x.LOVType == "LOV_RELATIONSHIP");

            try
            {

                using (var sl = new SLDocument(stream))
                {
                    var stats = sl.GetWorksheetStatistics();
                    var i = 2;

                    while (i <= stats.EndRowIndex)
                    {
                        //var fields = parser.ReadFields().ToList();
                        try
                        {

                            var title = sl.GetCellValueAsString(i, 1).Trim(); //fields[0];
                            var firstname = sl.GetCellValueAsString(i, 2).Trim();  //fields[1];

                            var lastname = sl.GetCellValueAsString(i, 3).Trim(); // fields[3];
                            var gender = sl.GetCellValueAsString(i, 4).Trim(); //fields[4];
                            var dateofbirth = sl.GetCellValueAsString(i, 5).Trim();  //fields[5];
                            var maritalStatus = sl.GetCellValueAsString(i, 6).Trim(); //fields[6];
                            var nationality = sl.GetCellValueAsString(i, 7).Trim(); //fields[7];
                            var religion = sl.GetCellValueAsString(i, 8).Trim(); //fields[8];

                            var contracttype = sl.GetCellValueAsString(i, 9).Trim(); //fields[7];
                            var contractrenewal = sl.GetCellValueAsString(i, 10).Trim(); //fields[8];


                            var department = sl.GetCellValueAsString(i, 11).Trim();  //fields[1];
                            var job = sl.GetCellValueAsString(i, 12).Trim(); //fields[2];                           
                            var location = sl.GetCellValueAsString(i, 13).Trim();  //fields[1];
                            var grade = sl.GetCellValueAsString(i, 14).Trim(); //fields[2];
                            var assignmenttype = sl.GetCellValueAsString(i, 15).Trim(); // fields[3];                          
                            var dateofjoin = sl.GetCellValueAsString(i, 16).Trim(); //fields[2];
                            var probationperiod = sl.GetCellValueAsString(i, 17).Trim(); //fields[4];
                            var noticeperiod = sl.GetCellValueAsString(i, 18).Trim();  //fields[1];
                            var anualleave = sl.GetCellValueAsString(i, 19).Trim(); //fields[9];
                            var sponsor = sl.GetCellValueAsString(i, 20).Trim(); //fields[9];

                            var middlename = sl.GetCellValueAsString(i, 21).Trim(); //fields[2];
                            var iqhamaNo = sl.GetCellValueAsString(i, 22).Trim(); //fields[0];
                            var biometric = sl.GetCellValueAsString(i, 23).Trim();  //fields[1];
                            var personNo = sl.GetCellValueAsString(i, 24).Trim(); //fields[2];
                            var sponsorshipNo = sl.GetCellValueAsString(i, 25).Trim(); // fields[3];
                            var presentunit = sl.GetCellValueAsString(i, 26).Trim(); //fields[4];
                            var presentbuilding = sl.GetCellValueAsString(i, 27).Trim();  //fields[5];
                            var presentstreet = sl.GetCellValueAsString(i, 28).Trim(); //fields[6];
                            var presentcity = sl.GetCellValueAsString(i, 29).Trim(); //fields[7];
                            var presentpostcode = sl.GetCellValueAsString(i, 30).Trim(); //fields[8];
                            var presentAdditionalNumber = sl.GetCellValueAsString(i, 31).Trim(); //fields[9];
                            var PresentNeighbourName = sl.GetCellValueAsString(i, 32).Trim(); //fields[0];
                            var PresentCountry = sl.GetCellValueAsString(i, 33).Trim();  //fields[1];
                            var PermanentUnitNumber = sl.GetCellValueAsString(i, 34).Trim(); //fields[2];
                            var PermanentBuildingNumber = sl.GetCellValueAsString(i, 35).Trim(); // fields[3];
                            var PermanentStreetName = sl.GetCellValueAsString(i, 36).Trim(); //fields[4];
                            var PermanentCity = sl.GetCellValueAsString(i, 37).Trim();  //fields[5];
                            var PermanentPostalCode = sl.GetCellValueAsString(i, 38).Trim(); //fields[6];
                            var PermanentAdditionalNumber = sl.GetCellValueAsString(i, 39).Trim(); //fields[7];
                            var PermanentNeighbourName = sl.GetCellValueAsString(i, 40).Trim(); //fields[8];
                            var PermanentCountry = sl.GetCellValueAsString(i, 41).Trim(); //fields[9];
                            var PersonalEmail = sl.GetCellValueAsString(i, 42).Trim(); //fields[0];
                            var MobileNumber = sl.GetCellValueAsString(i, 43).Trim();  //fields[1];
                            var Country = sl.GetCellValueAsString(i, 44).Trim(); //fields[2];
                            var EmergencyContactName1 = sl.GetCellValueAsString(i, 45).Trim(); // fields[3];
                            var EmergencyContactMobile1 = sl.GetCellValueAsString(i, 46).Trim(); //fields[4];
                            var EmergencyContactRelation1 = sl.GetCellValueAsString(i, 47).Trim();  //fields[5];
                            var EmergencyContactOtherRelation1 = sl.GetCellValueAsString(i, 48).Trim(); //fields[6];
                            var EmergencyContactCountry1 = sl.GetCellValueAsString(i, 49).Trim(); //fields[7];
                            var EmergencyContactName2 = sl.GetCellValueAsString(i, 50).Trim(); //fields[8];
                            var EmergencyContactMobile2 = sl.GetCellValueAsString(i, 51).Trim(); //fields[9];
                            var EmergencyContactRelation2 = sl.GetCellValueAsString(i, 52).Trim(); //fields[7];
                            var EmergencyContactOtherRelation2 = sl.GetCellValueAsString(i, 53).Trim(); //fields[8];
                            var EmergencyContactCountry2 = sl.GetCellValueAsString(i, 54).Trim(); //fields[9];

                            var pmodel = new NoteTemplateViewModel
                            {
                                TemplateCode = "HRPerson"
                            };
                            var pnote = await _noteBusiness.GetNoteDetails(pmodel);
                            pnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            pnote.StartDate = DateTime.Now;
                            pnote.DataAction = Common.DataActionEnum.Create;

                            dynamic exop = new System.Dynamic.ExpandoObject();

                            var titleid = titlelist.Where(x => x.Name == title).FirstOrDefault();
                            if (titleid != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("TitleId", titleid.Id);
                            }
                            var fullName = firstname + " " + lastname;
                            if (middlename.IsNotNullAndNotEmpty())
                            {
                                fullName = firstname + " " + middlename + " " + lastname;
                            }
                            ((IDictionary<String, Object>)exop).Add("FirstName", firstname);
                            ((IDictionary<String, Object>)exop).Add("MiddleName", middlename);
                            ((IDictionary<String, Object>)exop).Add("LastName", lastname);
                            ((IDictionary<String, Object>)exop).Add("PersonFullName", fullName);
                            var genderid = genderlist.Where(x => x.Name == gender).FirstOrDefault();
                            if (genderid != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("GenderId", genderid.Id);
                            }
                            var religionid = religionlist.Where(x => x.Name == religion).FirstOrDefault();
                            if (religionid != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("ReligionId", religionid.Id);
                            }
                            ((IDictionary<String, Object>)exop).Add("DateOfBirth", dateofbirth);

                            var MaritalStatusId = MaritalStatuslist.Where(x => x.Name == maritalStatus).FirstOrDefault();
                            if (MaritalStatusId != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("MaritalStatusId", MaritalStatusId.Id);
                            }

                            var NationalityId = Nationalitylist.Where(x => x.Name == nationality).FirstOrDefault();
                            if (NationalityId != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("NationalityId", NationalityId.Id);
                            }
                            ((IDictionary<String, Object>)exop).Add("IqamahNoNationalId", iqhamaNo);
                            ((IDictionary<String, Object>)exop).Add("PersonNo", personNo);
                            ((IDictionary<String, Object>)exop).Add("DateOfJoin", dateofjoin);
                            ((IDictionary<String, Object>)exop).Add("BiometricId", biometric);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressUnitNumber", presentunit);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressBuildingNumber", presentbuilding);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressStreetName", presentstreet);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressCityOrTown", presentcity);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressPostalCode", presentpostcode);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressAdditionalNumber", presentAdditionalNumber);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressNeighbourName", PresentNeighbourName);
                            ((IDictionary<String, Object>)exop).Add("PresentAddressCountryId", PresentCountry);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressUnitNumber", PermanentUnitNumber);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressBuildingNumber", PermanentBuildingNumber);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressStreetName", PermanentStreetName);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressCityOrTown", PermanentCity);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressPostalCode", PermanentPostalCode);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressAdditionalNumber", PermanentAdditionalNumber);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressNeighbourName", PermanentNeighbourName);
                            ((IDictionary<String, Object>)exop).Add("PermanentAddressCountryId", PermanentCountry);
                            ((IDictionary<String, Object>)exop).Add("ContactPersonalEmail", PersonalEmail);
                            ((IDictionary<String, Object>)exop).Add("MobileNumber", MobileNumber);

                            var countryl = countrylist.Where(x => x.Name == Country).FirstOrDefault();
                            if (countryl != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("ContactCountryId", countryl.Id);
                            }
                                    ((IDictionary<String, Object>)exop).Add("EmergencyContactName1", EmergencyContactName1);
                            var relation = relationlist.Where(x => x.Name == EmergencyContactRelation1).FirstOrDefault();
                            if (relation != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("EmergencyContact1RelationshipId", relation.Id);
                            }
                                    ((IDictionary<String, Object>)exop).Add("EmergencyContactMobileNumber1", EmergencyContactMobile1);
                            ((IDictionary<String, Object>)exop).Add("EmergencyContact1OtherRelation", EmergencyContactOtherRelation1);

                            var ecountryl = countrylist.Where(x => x.Name == EmergencyContactCountry1).FirstOrDefault();
                            if (ecountryl != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("EmergencyContact1CountryId", ecountryl.Id);
                            }
                                    ((IDictionary<String, Object>)exop).Add("EmergencyContactName2", EmergencyContactName2);
                            var relation2 = relationlist.Where(x => x.Name == EmergencyContactRelation2).FirstOrDefault();
                            if (relation2 != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("EmergencyContact2RelationshipId", relation2.Id);
                            }
                                    ((IDictionary<String, Object>)exop).Add("EmergencyContactMobileNumber2", EmergencyContactMobile2);
                            ((IDictionary<String, Object>)exop).Add("EmergencyContact2OtherRelation", EmergencyContactOtherRelation2);
                            var ecountryl2 = countrylist.Where(x => x.Name == EmergencyContactCountry2).FirstOrDefault();
                            if (ecountryl2 != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("EmergencyContact2CountryId", ecountryl2.Id);
                            }
                                    ((IDictionary<String, Object>)exop).Add("SponsorshipNo", sponsorshipNo);

                            if (personstatus != null)
                            {
                                ((IDictionary<String, Object>)exop).Add("PersonalStatusId", personstatus.Id);
                            }

                            pnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exop);
                            var result1 = await _noteBusiness.ManageNote(pnote);

                            if (result1.IsSuccess)
                            {
                                //contract
                                var tmodel = new NoteTemplateViewModel
                                {
                                    TemplateCode = "HRContract"
                                };
                                var tnote = await _noteBusiness.GetNoteDetails(tmodel);
                                tnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                tnote.StartDate = DateTime.Now;
                                tnote.DataAction = Common.DataActionEnum.Create;

                                dynamic exot = new System.Dynamic.ExpandoObject();


                                ((IDictionary<String, Object>)exot).Add("EmployeeId", result1.Item.UdfNoteTableId);


                                var contractt = contractlist.Where(x => x.Name == contracttype).FirstOrDefault();
                                if (contractt != null)
                                {
                                    ((IDictionary<String, Object>)exot).Add("ContractTypeId", contractt.Id);
                                }
                                ((IDictionary<String, Object>)exot).Add("AnnualLeaveEntitlement", firstname);

                                var renewal = renewallist.Where(x => x.Name == contractrenewal).FirstOrDefault();
                                if (renewal != null)
                                {
                                    ((IDictionary<String, Object>)exot).Add("ContractRenewable", renewal.Id);
                                }
                                var spons = sponsorlist.Where(x => x.Name == sponsor).FirstOrDefault();
                                if (spons != null)
                                {
                                    ((IDictionary<String, Object>)exot).Add("SponsorId", spons.Id);
                                }



                                tnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exot);
                                var resultt = await _noteBusiness.ManageNote(tnote);
                                if (!resultt.IsSuccess)
                                {
                                    errorList.AddRange(resultt.Messages.Values);
                                }
                                //Assignment

                                var cmodel = new NoteTemplateViewModel
                                {
                                    TemplateCode = "HRAssignment"
                                };
                                var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                                cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                cnote.StartDate = DateTime.Now;
                                cnote.DataAction = Common.DataActionEnum.Create;

                                dynamic exo1 = new System.Dynamic.ExpandoObject();


                                ((IDictionary<String, Object>)exo1).Add("EmployeeId", result1.Item.UdfNoteTableId);

                                var orgdep = departmentlist.Where(x => x.Name == department).FirstOrDefault();
                                if (orgdep != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("DepartmentId", orgdep.Id);
                                }
                                var orgjob = joblist.Where(x => x.Name == job).FirstOrDefault();
                                if (orgjob != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("JobId", orgjob.Id);
                                }

                                //var orgpos = await GenerateNextPositionName(department + "_" + job + "_");
                                var orgpos = await CreatePosition(orgdep.Id, orgjob.Id);
                                if (orgpos != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("PositionId", orgpos.Id);
                                }
                                var orgloc = locationlist.Where(x => x.Name == location).FirstOrDefault();
                                if (orgloc != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("LocationId", orgloc.Id);
                                }
                                var orggrade = gradelist.Where(x => x.Name == grade).FirstOrDefault();
                                if (orggrade != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("AssignmentGradeId", orggrade.Id);
                                }
                                var orgassign = assignTypelist.Where(x => x.Name == assignmenttype).FirstOrDefault();
                                if (orgassign != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("AssignmentTypeId", orgassign.Id);
                                }
                                var orgprob = probationlist.Where(x => x.Name == probationperiod).FirstOrDefault();
                                if (orgprob != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("ProbationPeriodId", orgprob.Id);
                                }

                                ((IDictionary<String, Object>)exo1).Add("NoticePeriod", noticeperiod);
                                ((IDictionary<String, Object>)exo1).Add("DateOfJoin", dateofjoin);
                                var orgassignstatus = assignstatuslist.Where(x => x.Name == "Active Assignment").FirstOrDefault();
                                if (orgassignstatus != null)
                                {
                                    ((IDictionary<String, Object>)exo1).Add("AssignmentStatusId", orgassignstatus.Id);
                                }
                                cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                var resulc = await _noteBusiness.ManageNote(cnote);
                                if (!resulc.IsSuccess)
                                {
                                    errorList.AddRange(resulc.Messages.Values);
                                }
                            }
                            else
                            {
                                errorList.AddRange(result1.Messages.Values);
                            }

                        }

                        catch (Exception ex)
                        {
                            errorList.Add(string.Concat("Error Assignment Data :", sl.GetCellValueAsString(i, 1).Trim()));
                            errorList.Add(string.Concat("Error :", ex.Message));
                        }
                        // errorList.Add(string.Concat("Success Questionnaire :", fields[0]));
                        i++;
                    }
                }

                if (UploadStatus.IsNotNull() && errorList.Count() > 0)
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = string.Join(",", errorList);//.ScheduleInprogress;
                    rowData1["ErrorCount"] = errorList.Count();
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Completed.ToString()));//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }


            }
            catch (Exception ex)
            {
                if (UploadStatus.IsNotNull())
                {
                    rowData1["ExecutionStatus"] = (int)((HrDataExecutionStatus)Enum.Parse(typeof(HrDataExecutionStatus), HrDataExecutionStatus.Error.ToString()));//.ScheduleInprogress;
                    rowData1["Error"] = ex.Message;//.ScheduleInprogress;
                    rowData1["ExecutionEndTime"] = DateTime.Now;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
            }

        }
        public async Task<bool> DeleteUserHierarchy(string NoteId)
        {
            var note = await _repo.GetSingleById<NoteViewModel, NtsNote>(NoteId);
            if (note != null)
            {
                var query = $@"update  cms.""N_GENERAL_UserHierarchy"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryAssignment.ExecuteCommand(query, null);

                await _repo.Delete<NoteViewModel, NtsNote>(NoteId);
                return true;
            }
            return false;
        }
        public async Task<IList<DataUploadViewModel>> GetUploadData()
        {
            //string query = @$"select d.""Id"" as ""DataUploadId"", d.""ExecutionStartTime"" ::TIMESTAMP::DATE as ""ExecutionStartTime"" , d.""ExecutionEndTime"" ::TIMESTAMP::DATE as ""ExecutionEndTime"", d.""ErrorCount"" as ""ErrorCount""
            //           , lov.""Name"" as ""UploadType"" ,lov1.""Name"" as ""ExecutionStatus""
            //                from cms.""N_SNC_CHR_DataUpload"" as d
            //                join public.""NtsService"" as s on s.""UdfNoteTableId""=d.""Id"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
            //                left join public.""LOV"" as lov on lov.""Id""=d.""UploadTypeId"" and  lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
            //                left join public.""LOV"" as lov1 on lov1.""Id""=d.""ExecutionStatusId"" and  lov1.""IsDeleted""=false and lov1.""CompanyId""='{_userContext.CompanyId}'
            //                where d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'  ";

            string query = @$"select d.""Id"" as ""DataUploadId"", d.""ExecutionStartTime"" ::TIMESTAMP::DATE as ""ExecutionStartTime"" , d.""ExecutionEndTime"" ::TIMESTAMP::DATE as ""ExecutionEndTime"", d.""ErrorCount"" as ""ErrorCount""
                       , lov.""Name"" as ""UploadType"" ,d.""ExecutionStatus"" as ""ExecutionStatus"", d.""Error"" as ""Error""
                            from cms.""N_SNC_CHR_DataUpload"" as d
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=d.""Id"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""LOV"" as lov on lov.""Id""=d.""UploadTypeId"" and  lov.""IsDeleted""=false and lov.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""LOV"" as lov1 on lov1.""Id""=d.""ExecutionStatusId"" and  lov1.""IsDeleted""=false and lov1.""CompanyId""='{_userContext.CompanyId}'
                            where d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'  ";



            var result = await _queryDataUpload.ExecuteQueryList<DataUploadViewModel>(query, null);
            return result;
        }

        public async Task<bool> UpdateDepartmentName(dynamic udf)
        {
            var query = $@"update  cms.""N_CoreHR_HRDepartment"" set ""DepartmentName""='{udf.NewDepartmentName}' where ""Id""='{udf.CurrentDepartmentId}' ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }
        public async Task<bool> UpdateDepartmentName(Dictionary<string, object> udf)
        {
            var query = $@"update  cms.""N_CoreHR_HRDepartment"" set ""DepartmentName""='{udf["NewDepartmentName"].ToString()}' where ""Id""='{udf["CurrentDepartmentId"].ToString()}' ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }
        public async Task<bool> UpdateJobName(dynamic udf)
        {
            var query = $@"update  cms.""N_CoreHR_HRJob"" set ""JobTitle""='{udf.NewJobName}' where ""Id""='{udf.CurrentJobId}' ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }
        public async Task<bool> UpdateJobName(Dictionary<string, object> udf)
        {
            var query = $@"update  cms.""N_CoreHR_HRJob"" set ""JobTitle""='{udf["NewJobName"].ToString()}' where ""Id""='{udf["CurrentJobId"].ToString()}' ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }

        public async Task<PositionViewModel> CreatePosition(string departmentId, string jobId)
        {
            //            var posNo = 1;

            //            var query = $@"SELECT p.""PositionName"",j.""JobTitle"",d.""DepartmentName""
            //FROM cms.""N_CoreHR_HRPosition"" p
            //join cms.""N_CoreHR_HRJob"" j on p.""JobId"" = j.""Id""
            //join cms.""N_CoreHR_HRDepartment"" d on p.""DepartmentId"" = d.""Id"" 
            //where p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}' #Dept# ";

            //            string depwhere = "";
            //            if (departmentId.IsNotNull())
            //            {
            //                depwhere = $@" and p.""DepartmentId""='{departmentId}' ";
            //            }
            //            query = query.Replace("#Dept#", depwhere);

            //            string jobwhere = "";
            //            if (jobId.IsNotNull())
            //            {
            //                jobwhere = $@" and p.""JobId""='{jobId}' ";
            //            }
            //            query = query.Replace("#Dept#", jobwhere);

            //            var pos = await _queryRepo1.ExecuteQuerySingle<PositionViewModel>(query, null);

            //            if (pos.IsNotNull())
            //            {
            //                posNo = pos.PositionNo.IsNotNull() ? pos.PositionNo + posNo : posNo;
            //            }

            //var dept = await GetDepartmentNameById(departmentId);
            //var job = await GetJobNameById(jobId);

            var posmodel = new PositionViewModel()
            {
                DepartmentId = departmentId,
                JobId = jobId
            };

            var ntempmodel = new NoteTemplateViewModel()
            {
                TemplateCode = "HRPosition"
            };
            var nmodel = await _noteBusiness.GetNoteDetails(ntempmodel);

            nmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            nmodel.StartDate = DateTime.Now;
            nmodel.DataAction = DataActionEnum.Create;

            nmodel.Json = JsonConvert.SerializeObject(posmodel);
            var nresult = await _noteBusiness.ManageNote(nmodel);

            posmodel = JsonConvert.DeserializeObject<PositionViewModel>(nresult.Item.Json);
            posmodel.Id = nresult.Item.UdfNoteTableId;

            return posmodel;

        }
        public async Task<CommandResult<DataUploadViewModel>> ValidateTemplate(DataUploadViewModel model)
        {
            var query = $@"select d.""Id"" as ""DataUploadId"", d.""UploadTypeId"" as ""UploadTypeId"" 
                           from cms.""N_SNC_CHR_DataUpload"" as d
                            join public.""NtsService"" as s on s.""UdfNoteTableId""=d.""Id"" and  s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
                              where d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}' ";



            var result = await _queryDataUpload.ExecuteQueryList(query, null);
            var exist = result.Where(x => x.UploadTypeId == model.UploadTypeId && x.DataUploadId != model.DataUploadId);

            if (exist.Any())
            {
                return CommandResult<DataUploadViewModel>.Instance(model, false, "Upload Type already exist");
            }

            return CommandResult<DataUploadViewModel>.Instance(model, true, "");

        }
        public async Task<bool> UpdatePersonType(dynamic udf)
        {
            var personlov = await _lovBusiness.GetSingle(x => x.Code == "PERSON_STATUS_TERMINATED");
            var personstatusid = personlov.Id;
            var query = $@"update  cms.""N_CoreHR_HRPerson"" set ""PersonalStatusId""='{personstatusid}' where ""UserId""='{udf.OwnerUserId_Id}' ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }
        public async Task<bool> UpdateAssignmentStatus(dynamic udf)
        {
            var assignlov = await _lovBusiness.GetSingle(x => x.Code == "ASSIGNMENT_STATUS_TERMINATE");
            var assignstatusid = assignlov.Id;
            //var personId = $@"select ""Id"" from cms.""N_CoreHR_HRPerson"" where ""UserId""='{udf.OwnerUserId}'"; 
            var query = $@"update  cms.""N_CoreHR_HRAssignment"" set ""AssignmentStatusId""='{assignstatusid}' where ""EmployeeId""=(select ""Id"" from cms.""N_CoreHR_HRPerson"" where ""UserId""='{udf.OwnerUserId_Id}' and ""IsDeleted""=false) and ""IsDeleted""=false ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }
        public async Task<bool> UpdateUserStatus(dynamic udf)
        {

            var query = $@"update  public.""User""  set ""Status""='{(int)(StatusEnum.Inactive)}' where ""Id""='{udf.OwnerUserId_Id}' ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }
        public async Task<bool> UpdateContract(dynamic udf)
        {
            //var personId = $@"select ""Id"" from cms.""N_CoreHR_HRPerson"" where ""UserId""='{udf.OwnerUserId}'";
            var query = $@"update  cms.""N_CoreHR_HRContract"" set ""EffectiveEndDate""='{udf.LastWorkingDate}' where ""EmployeeId""=(select ""Id"" from cms.""N_CoreHR_HRPerson"" where ""UserId""='{udf.OwnerUserId_Id}' and ""IsDeleted""=false) and ""IsDeleted""=false ";
            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }

        public async Task<bool> UpdatePersonDetails(dynamic udf)
        {
            string perId = udf.EmployeeId;
            var person = await GetPersonDetailsById(perId);

            var perfullname = udf.NewFirstName + " " + udf.NewLastName;
            if (person.MiddleName.IsNotNullAndNotEmpty())
            {
                perfullname = udf.NewFirstName + " " + person.MiddleName + " " + udf.NewLastName;
            }

            var query = $@"update cms.""N_CoreHR_HRPerson"" set ""TitleId""='{udf.NewTitleId}',""FirstName""='{udf.NewFirstName}',""LastName""='{udf.NewLastName}',""PersonFullName""='{perfullname}',""GenderId""='{udf.NewGenderId}',""MaritalStatusId""='{udf.NewMaritalStatusId}',
""NationalityId""='{udf.NewNationalityId}',""ReligionId""='{udf.NewReligionId}',""DateOfJoin""='{udf.NewDateOfJoin}',""ContactPersonalEmail""='{udf.NewEmailId}',""DateOfBirth""='{udf.NewDateOfBirth}'
where ""Id""='{udf.EmployeeId}' ";

            await _queryPerson.ExecuteCommand(query, null);

            //update LM in user

            await UpdateLineManager(udf);

            return true;
        }

        public async Task<bool> UpdateAssignmentDetails(dynamic udf)
        {
            var query = $@"update cms.""N_CoreHR_HRAssignment"" set ""DepartmentId""='{udf.NewDepartmentId}',""JobId""='{udf.NewJobId}',""PositionId""='{udf.NewPositionId}',""LocationId""='{udf.NewLocationId}',""AssignmentGradeId""='{udf.NewAssignmentGradeId}',
""AssignmentTypeId""='{udf.NewAssignmentTypeId}',""CareerLevelId""='{udf.NewCareerLevelId}',""DateOfJoin""='{udf.NewDateOfJoin}',""EmployeeId""='{udf.NewUserId}'
where ""EmployeeId""='{udf.UserId}' ";

            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }

        public async Task<bool> UpdateLineManager(dynamic udf)
        {

            string perId = Convert.ToString(udf.EmployeeId);
            string clmId = Convert.ToString(udf.NewLineManagerId);

            var person = await GetPersonDetailsById(perId);

            var query = $@"update public.""User"" set ""LineManagerId""='{clmId}' where ""Id""='{person.UserId}' ";

            await _queryAssignment.ExecuteCommand(query, null);

            return true;
        }
        public async Task<bool> ValidateFinancialYearStartDateandEndDate(FormTemplateViewModel viewModel)
        {

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var startDate = Convert.ToString(rowData.GetValueOrDefault("StartDate"));
            var endDate = Convert.ToString(rowData.GetValueOrDefault("EndDate"));
            var newId = viewModel.Id;
            var query =
                $@" select ""Id""
                    from cms.""F_PAY_HR_FinancialYearName"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""StartDate"">='{startDate}' and ""StartDate""<='{endDate}')
                                                    or
                                                         (""EndDate"">='{startDate}' and ""EndDate""<='{endDate}')) and ""Id""!='{newId}'";

            var exisiting = await _queryRepo1.ExecuteQueryList(query, null);
            if (exisiting.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<PositionViewModel> GetPositionDetailsById(string Id)
        {
            string query = @$"SELECT * FROM cms.""N_CoreHR_HRPosition""
                                where ""Id""='{Id}' and ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' ";

            var result = await _queryAssignment.ExecuteQuerySingle<PositionViewModel>(query, null);
            return result;
        }
        public async Task CreateDepartment(NoteTemplateViewModel viewModel)
        {
            var _hierarchyMasterbusiness = _sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "HRDepartment";
                model1.ActiveUserId = _userContext.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
        }
        public async Task CreateNewCareerLevel(NoteTemplateViewModel viewModel)
        {
            var _hierarchyMasterbusiness = _sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "Career Level";
                model1.ActiveUserId = _userContext.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
        }
        public async Task CreateNewJob(NoteTemplateViewModel viewModel)
        {
            var _hierarchyMasterbusiness = _sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "HRJob";
                model1.ActiveUserId = _userContext.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
        }
        public async Task CreateNewPosition(NoteTemplateViewModel viewModel)
        {
            var _hierarchyMasterbusiness = _sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "HRPosition";
                model1.ActiveUserId = _userContext.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
        }
        public async Task<CommandResult<NoteTemplateViewModel>> CreateNewPerson(NoteTemplateViewModel viewModel)
        {
            var errorList = new Dictionary<string, string>();
            var _userBusiness = _sp.GetService<IUserBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var _lovBusiness = _sp.GetService<ILOVBusiness>();
            var data = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData = data.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var assignlov = await _lovBusiness.GetSingle(x => x.Code == "ASSIGNMENT_STATUS_ACTIVE");
            var assignstatusid = assignlov.Id;
            var email = rowData["EmailId"].ToString(); //udf.EmailId;
            var userData = await _userBusiness.ValidateUser(email);
            if (userData != null)
            {
                errorList.Add("Validate", "Person already exist with given email.");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            else
            {
                // Create User
                var userModel = new UserViewModel();
                userModel.DataAction = DataActionEnum.Create;
                userModel.Email = email;
                userModel.Name = rowData["FirstName"].ToString() + " " + rowData["LastName"].ToString();//udf.FirstName + " " + udf.LastName;
                userModel.LineManagerId = rowData.ContainsKey("LineManagerId") ? rowData["LineManagerId"].ToString() : null; //udf.LineManagerId;

                var userResult = await _userBusiness.Create(userModel);
                if (userResult.IsSuccess)
                {
                    // Create Person
                    var userid = userResult.Item.Id;
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "HRPerson";
                    noteTempModel.OwnerUserId = _userContext.UserId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.DataAction = DataActionEnum.Create;
                    //dynamic exo = new System.Dynamic.ExpandoObject();
                    //((IDictionary<String, Object>)exo).Add("EmployeeId", viewModel.UdfNoteTableId);
                    var perModel = new PersonViewModel();
                    //{
                    perModel.UserId = userid;
                    perModel.TitleId = rowData["TitleId"].ToString(); //udf.TitleId,
                    perModel.FirstName = rowData["FirstName"].ToString();//udf.FirstName,
                    perModel.LastName = rowData["LastName"].ToString();//udf.LastName,
                    perModel.PersonFullName = rowData["PersonFullName"].IsNotNull() ? rowData["PersonFullName"].ToString() : "";
                    perModel.GenderId = rowData["GenderId"].ToString();// udf.GenderId,
                    perModel.DateOfBirth = Convert.ToDateTime(rowData["DateOfBirth"]);// Convert.ToDateTime(udf.DateOfBirth),
                    perModel.MaritalStatusId = rowData["MaritalStatusId"].ToString();//udf.MaritalStatusId,
                    perModel.NationalityId = rowData["NationalityId"].ToString();//udf.NationalityId,
                    perModel.ReligionId = rowData["ReligionId"].ToString();//udf.ReligionId,
                    perModel.DateOfJoin = Convert.ToDateTime(rowData["DateOfJoin"]);//Convert.ToDateTime(udf.DateOfJoin),
                    perModel.BusinessHierarchyParentId = rowData["BusinessHierarchyParentId"].ToString();//udf.BusinessHierarchyParentId
                                                                                                         // };
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(perModel);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var perResult = await _noteBusiness.ManageNote(notemodel);
                    //var perid = perResult.Item.Id;
                    var perid = perResult.Item.UdfNoteTableId;
                    if (perResult.IsSuccess)
                    {
                        // Create Assignment
                        //var perid = perResult.Item.Id;
                        var assignnoteTempModel = new NoteTemplateViewModel();
                        assignnoteTempModel.ActiveUserId = _userContext.UserId;
                        assignnoteTempModel.TemplateCode = "HRAssignment";
                        assignnoteTempModel.OwnerUserId = _userContext.UserId;
                        var assignnotemodel = await _noteBusiness.GetNoteDetails(assignnoteTempModel);
                        assignnotemodel.DataAction = DataActionEnum.Create;
                        //dynamic exo = new System.Dynamic.ExpandoObject();
                        //((IDictionary<String, Object>)exo).Add("EmployeeId", viewModel.UdfNoteTableId);
                        var assignModel = new AssignmentViewModel();
                        //{
                        assignModel.UserId = userid;
                        assignModel.EmployeeId = perid;
                        assignModel.DepartmentId = rowData.ContainsKey("DepartmentId") && rowData["DepartmentId"].IsNotNull() ? rowData["DepartmentId"].ToString() : null;// udf.DepartmentId,
                        assignModel.JobId = rowData.ContainsKey("JobId") && rowData["JobId"].IsNotNull() ? rowData["JobId"].ToString() : null;// udf.JobId,
                        assignModel.PositionId = rowData.ContainsKey("PositionId") && rowData["PositionId"].IsNotNull() ? rowData["PositionId"].ToString() : null;// udf.PositionId,
                        assignModel.LocationId = rowData.ContainsKey("LocationId") && rowData["LocationId"].IsNotNull() ? rowData["LocationId"].ToString() : null;// udf.LocationId,
                        assignModel.AssignmentGradeId = rowData.ContainsKey("AssignmentGradeId") && rowData["AssignmentGradeId"].IsNotNull() ? rowData["AssignmentGradeId"].ToString() : null;// udf.AssignmentGradeId,
                        assignModel.AssignmentTypeId = rowData.ContainsKey("AssignmentTypeId") && rowData["AssignmentTypeId"].IsNotNull() ? rowData["AssignmentTypeId"].ToString() : null;// udf.AssignmentTypeId,
                        assignModel.OrgLevel1Id = rowData.ContainsKey("OrgLevel1Id") && rowData["OrgLevel1Id"].IsNotNull() ? rowData["OrgLevel1Id"].ToString() : null;// udf.OrgLevel1Id,
                        assignModel.OrgLevel2Id = rowData.ContainsKey("OrgLevel2Id") && rowData["OrgLevel2Id"].IsNotNull() ? rowData["OrgLevel2Id"].ToString() : null;// udf.OrgLevel2Id,
                        assignModel.OrgLevel3Id = rowData.ContainsKey("OrgLevel3Id") && rowData["OrgLevel3Id"].IsNotNull() ? rowData["OrgLevel3Id"].ToString() : null;// udf.OrgLevel3Id,
                        assignModel.OrgLevel4Id = rowData.ContainsKey("OrgLevel4Id") && rowData["OrgLevel4Id"].IsNotNull() ? rowData["OrgLevel4Id"].ToString() : null;// udf.OrgLevel4Id,
                        assignModel.BrandId = rowData.ContainsKey("BrandId") && rowData["BrandId"].IsNotNull() ? rowData["BrandId"].ToString() : null;// udf.BrandId,
                        assignModel.MarketId = rowData.ContainsKey("MarketId") && rowData["MarketId"].IsNotNull() ? rowData["MarketId"].ToString() : null;// udf.MarketId,
                        assignModel.ProvinceId = rowData.ContainsKey("ProvinceId") && rowData["ProvinceId"].IsNotNull() ? rowData["ProvinceId"].ToString() : null;// udf.ProvinceId,
                        assignModel.CareerLevelId = rowData.ContainsKey("CareerLevelId") && rowData["CareerLevelId"].IsNotNull() ? rowData["CareerLevelId"].ToString() : null;// udf.CareerLevelId,
                        assignModel.DateOfJoin = rowData.ContainsKey("DateOfJoin") && rowData["DateOfJoin"].IsNotNull() ? rowData["DateOfJoin"].ToString() : null;// udf.DateOfJoin,
                        assignModel.AssignmentStatusId = assignstatusid;
                        // };
                        assignnotemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(assignModel);
                        assignnotemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var assignResult = await _noteBusiness.ManageNote(assignnotemodel);
                        //var assignid = assignResult.Item.UdfNoteTableId;
                        if (assignResult.IsSuccess)
                        {

                        }
                        else
                        {
                            errorList.Add("Error", assignResult.Message);
                            return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
                        }
                    }
                    else
                    {
                        errorList.Add("Error", perResult.Message);
                        return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
                    }

                }
                else
                {
                    errorList.Add("Error", userResult.Message);
                    return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
                }
            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task UpdatePersonJob(NoteTemplateViewModel viewModel)
        {
            // Get Person Assignment
            var _hrBusiness = _sp.GetService<IHRCoreBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var data = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData = data.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            string personId = rowData["PersonId"].ToString();
            var assignments = await _hrBusiness.GetAssignmentDetails(personId, null);
            var personDetails = assignments.FirstOrDefault();
            // Create New position with the help of New Department and Existing Job Id               
            var position = await _hrBusiness.CreatePosition(personDetails.DepartmentId, rowData["NewJobId"].ToString());
            // Update the Assignment with New DepartmentId and NewpositionId
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = personDetails.NoteAssignmentId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            rowData1["JobId"] = rowData["NewJobId"].ToString();
            rowData1["PositionId"] = position.Id;
            var data1 = JsonConvert.SerializeObject(rowData1);
            model.Json = data1;
            model.DataAction = DataActionEnum.Edit;
            model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            await _noteBusiness.ManageNote(model);
            // var update = await _noteBusiness.EditNoteUdfTable(model, data1, model.UdfNoteTableId);
        }

        public async Task UpdatePersonDepartment(NoteTemplateViewModel viewModel)
        {
            // Get Person Assignment
            var _hrBusiness = _sp.GetService<IHRCoreBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var data = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData = data.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            string personId = rowData["PersonId"].ToString();
            var assignments = await _hrBusiness.GetAssignmentDetails(personId, null);
            var personDetails = assignments.FirstOrDefault();
            // Create New position with the help of New Department and Existing Job Id               
            var position = await _hrBusiness.CreatePosition(rowData["NewDepartmentId"].ToString(), personDetails.JobId);
            // Update the Assignment with New DepartmentId and NewpositionId
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = personDetails.NoteAssignmentId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            rowData1["DepartmentId"] = rowData["NewDepartmentId"].ToString();//udf.NewDepartmentId;
            rowData1["PositionId"] = position.Id;
            var data1 = JsonConvert.SerializeObject(rowData1);
            model.Json = data1;
            model.DataAction = DataActionEnum.Edit;
            model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            await _noteBusiness.ManageNote(model);
            //var update = await _noteBusiness.EditNoteUdfTable(model, data1, model.UdfNoteTableId);
        }
    }
}
