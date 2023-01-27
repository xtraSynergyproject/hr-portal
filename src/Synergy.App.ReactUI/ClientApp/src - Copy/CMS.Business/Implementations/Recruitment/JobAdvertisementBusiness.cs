using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Business
{
    public class JobAdvertisementBusiness : BusinessBase<JobAdvertisementViewModel, JobAdvertisement>, IJobAdvertisementBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepoIdName;
        private readonly IRepositoryQueryBase<JobAdvertisementViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<JobCriteriaViewModel> _queryRepoCri;
        private readonly IRepositoryQueryBase<ApplicationViewModel> _queryAppRepo;
        private readonly IRepositoryQueryBase<ListOfValueViewModel> _querylovRepo;
        public JobAdvertisementBusiness(IRepositoryBase<JobAdvertisementViewModel, JobAdvertisement> repo, IMapper autoMapper, IRepositoryQueryBase<IdNameViewModel> queryRepoIdName,
            IRepositoryQueryBase<JobAdvertisementViewModel> queryRepo, IRepositoryQueryBase<JobCriteriaViewModel> queryRepoCri,
            IRepositoryQueryBase<ApplicationViewModel> queryAppRepo, IRepositoryQueryBase<ListOfValueViewModel> querylovRepo) : base(repo, autoMapper)
        {
            _queryRepoIdName = queryRepoIdName;
            _queryRepo = queryRepo;
            _queryRepoCri = queryRepoCri;
            _queryAppRepo = queryAppRepo;
            _querylovRepo = querylovRepo;
        }


        public async override Task<CommandResult<JobAdvertisementViewModel>> Create(JobAdvertisementViewModel model)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            model.Description = HttpUtility.HtmlDecode(model.Description);
            model.Responsibilities = HttpUtility.HtmlDecode(model.Responsibilities);
            var result = await base.Create(model);

            return CommandResult<JobAdvertisementViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<JobAdvertisementViewModel>> Edit(JobAdvertisementViewModel model)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            model.Description = HttpUtility.HtmlDecode(model.Description);
            model.Responsibilities = HttpUtility.HtmlDecode(model.Responsibilities);
            var result = await base.Edit(model);

            return CommandResult<JobAdvertisementViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<JobAdvertisementViewModel>> IsNameExists(JobAdvertisementViewModel model)
        {

            return CommandResult<JobAdvertisementViewModel>.Instance();
        }

        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameList()
        {
            var agencyid = "";
            var agency = await _repo.GetSingleGlobal<AgencyViewModel, Agency>(x => x.UserId == _repo.UserContext.UserId);
            if(agency!=null)
            {
                agencyid = agency.Id;
            }
            string query = @$"select j.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
                        NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode , jd.""Id"" as JobAdvId
                       from rec.""JobAdvertisement""
                        as jd inner join cms.""Job"" as j
                        on j.""Id"" = jd.""JobId""
                        inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
                        left join rec.""ListOfValue"" as lov on lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code""=j.""ManpowerTypeCode""
                        WHERE jd.""Status"" = '1' AND jd.""IsDeleted""='false' AND j.""IsDeleted""='false' 
                        and (jd.""AgencyId"" is null or '{agencyid}'=ANY(jd.""AgencyId"")) ";
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameListForSelection()
        {
            var agencyid = "";
            var agency = await _repo.GetSingleGlobal<AgencyViewModel, Agency>(x => x.UserId == _repo.UserContext.UserId);
            if (agency != null)
            {
                agencyid = agency.Id;
            }
            string query = @$"select j.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
                        NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode , jd.""Id"" as JobAdvId
                       from rec.""JobAdvertisement""
                        as jd inner join cms.""Job"" as j
                        on j.""Id"" = jd.""JobId""
                        inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
                        left join rec.""ListOfValue"" as lov on lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code""=j.""ManpowerTypeCode""
                        WHERE jd.""Status"" = '1' AND jd.""IsDeleted""='false' AND j.""IsDeleted""='false' 
                         ";
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameDashboardList()
        {
            //            string query = @$"select j.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
            //                        NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode 
            //from rec.""JobAdvertisement""
            //                        as jd inner join cms.""Job"" as j
            //                        on j.""Id"" = jd.""JobId""
            //                        inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
            //                        left join rec.""ListOfValue"" as lov on lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code""=j.""ManpowerTypeCode""
            //                        WHERE jd.""Status"" = '1' AND jd.""IsDeleted""='false' AND j.""IsDeleted""='false' 

            //                        union

            //                        select j.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
            //                        NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode 
            //                        from rec.""JobAdvertisement""
            //                        as jd inner join cms.""Job"" as j
            //                        on j.""Id"" = jd.""JobId""
            //                        inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
            //                        left join rec.""ListOfValue"" as lov on lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code""=j.""ManpowerTypeCode""
            //                        WHERE j.""Status"" = '2' 
            //                        ";
            string query = @$"with cte as (
SELECT      distinct   j.""Id"" ,j.""Name"" ,ja.""Status""                  


                            FROM rec.""JobAdvertisement"" as ja
                            inner join cms.""Job"" as j ON j.""Id"" = ja.""JobId""
                            inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                           -- and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

    and ja.""Status"" = '1' and ja.""Id"" not in (select ""Id"" from rec.""JobAdvertisement""
                                                          where ""JobId"" <> ja.""JobId"") union
                  SELECT      distinct j.""Id"",j.""Name"" ,ja.""Status"" 
                            FROM rec.""JobAdvertisement"" as ja
                            inner join cms.""Job"" as j ON j.""Id"" = ja.""JobId""
                            inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

    and ja.""Status"" = '2' and j.""Id"" not in (SELECT      distinct   j.""Id""  
                            FROM rec.""JobAdvertisement"" as ja
                            inner join cms.""Job"" as j ON j.""Id"" = ja.""JobId""
                            inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

    and ja.""Status"" = '1' and ja.""Id"" not in (select ""Id"" from rec.""JobAdvertisement""
                                                          where ""JobId"" <> ja.""JobId"")) )
							select cte.""Id"" as Id,cte.""Name"" as JobName
                              
                            from cte

                          
                            ";


            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameByOrgIdList(string OrgId)
        {
            string query = @$"select jd.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
                        NoOfPosition,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode from rec.""JobAdvertisement""
                        as jd inner join cms.""Job"" as j
                        on j.""Id"" = jd.""JobId"" inner join rec.""ListOfValue"" as lov on lov.""Id""=jd.""ManpowerTypeId""
                        inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
                        WHERE jd.""Status"" = '1' and jd.""OrganizationId""='{OrgId}'";
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameListByOrg(string organizationId)
        {
            //string query = @$"select jd.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
            //            NoOfPosition,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode from rec.""JobAdvertisement""
            //            as jd inner join cms.""Job"" as j
            //            on j.""Id"" = jd.""JobId"" inner join rec.""ListOfValue"" as lov on lov.""Id""=jd.""ManpowerTypeId""
            //            inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
            //            WHERE jd.""Status"" = '1' AND jd.""OrganizationId""='{organizationId}'";

            string query = @$"select jd.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
                        NoOfPosition,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode from rec.""JobAdvertisement""
                        as jd inner join cms.""Job"" as j
                        on j.""Id"" = jd.""JobId""
                        inner join rec.""ListOfValue"" as lov on  lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code"" = j.""ManpowerTypeCode""
                        inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
                        WHERE jd.""Status"" = '1' AND jd.""OrganizationId""='{organizationId}'";
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<JobCriteriaViewModel>> GetJobCriteriaList(string type,string jobadvtid)
        {
            string query = @$"Select jc.*,case when lov.""Name"" is null then '' else lov.""Name"" end  as CriteriaTypeName,
case when lovother.""Name"" is null then '' else lovother.""Name"" end  as LovTypeName
from rec.""JobCriteria"" as jc
left join rec.""ListOfValue"" as lov on lov.""Id"" = jc.""CriteriaType""
left join rec.""ListOfValue"" as lovother on lovother.""Id"" = jc.""ListOfValueTypeId""
where jc.""Type"" = '{type}' and jc.""JobAdvertisementId"" = '{jobadvtid}' and jc.""IsDeleted""=false ";
            var queryData = await _queryRepoCri.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string JobId)
        {
            //string query = @$"select jd.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
            //            NoOfPosition,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode 
            //            from rec.""JobAdvertisement""
            //            as jd inner join cms.""Job"" as j
            //            on j.""Id"" = jd.""JobId"" inner join rec.""ListOfValue"" as lov on lov.""Id""=jd.""ManpowerTypeId""
            //            WHERE jd.""Id""='{jobAdvertisementId}'";
            //string query = @$"select jd.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
            //            NoOfPosition,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode 
            //             from cms.""Job"" as j
            //            inner join rec.""JobAdvertisement"" as jd on j.""Id""=jd.""JobId"" and Jd.""Status""=1                        
            //            inner join rec.""ListOfValue"" as lov on lov.""Id""=jd.""ManpowerTypeId""
            //            WHERE j.""Id""='{JobId}'";
            string query = @$"select j.""Id"" as Id ,j.""Name""  as JobName,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode 
                         from cms.""Job"" as j
                                            
                        inner join rec.""ListOfValue"" as lov on lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code""=j.""ManpowerTypeCode""
                        WHERE j.""Id""='{JobId}'";
            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }
     
        public async Task<JobAdvertisementViewModel> GetNameById(string jobAdvId)
        {
            string query = @$"SELECT c.*,
                                ct.""Name"" as LocationName, q.""Name"" as QualificationName, j.""Name"" as JobCategoryName, jo.""Name"" as JobName
                                FROM rec.""JobAdvertisement"" as c
                                LEFT JOIN cms.""Location"" as ct ON ct.""Id"" = c.""LocationId""
                                LEFT JOIN cms.""Job"" as jo ON jo.""Id"" = c.""JobId""
                                LEFT JOIN rec.""ListOfValue"" as q ON q.""Id"" = c.""Qualification""
                                LEFT JOIN rec.""ListOfValue"" as j ON j.""Id"" = c.""JobCategoryId""
                                WHERE c.""IsDeleted""=false AND c.""Id""='{jobAdvId}'";


            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<IdNameViewModel> GetJobStatus(string Id)
        {
            string query = @$"select ""Status""
                                from  cms.""Job""
                                where ""Id"" = '{Id}' ";

            var queryData = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<IdNameViewModel> GetJobManpowerType(string Id)
        {
            //string query = @$"select mt.""Code"" as Code
            //from rec.""JobAdvertisement"" as jd 
            //left join rec.""ListOfValue"" as mt on jd.""ManpowerTypeId""=mt.""Id""
            //                    where jd.""JobId"" = '{Id}' and jd.""Status"" = 1 ";

            //string query = @$"select mt.""Code"" as Code
            //from rec.""JobAdvertisement"" as jd 
            //left join cms.""Job"" as j on j.""Id""=jd.""JobId""
            //left join rec.""ListOfValue"" as mt on  mt.""ListOfValueType""='LOV_MANPOWERTYPE' and mt.""Code"" = j.""ManpowerTypeCode""
            //                    where jd.""JobId"" = '{Id}' and jd.""Status"" = 1 ";

            string query = @$"select mt.""Code"" as Code
            from cms.""Job"" as j 
            left join rec.""ListOfValue"" as mt on  mt.""ListOfValueType""='LOV_MANPOWERTYPE' and mt.""Code"" = j.""ManpowerTypeCode""
                                where j.""Id"" = '{Id}'";


            var queryData = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return queryData;
        }
        

        public async Task<IList<JobAdvertisementViewModel>> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerTypeId,string agency)
        {
            //string query = @$"select jd.*,j.""Name"" as JobName,mt.""Name"" as ManpowerTypeName,
            //cat.""Name"" as JobCategoryName,loc.""Name"" as LocationName
            //from rec.""JobAdvertisement"" as jd  
            //join rec.""ListOfValue"" as st on jd.""ActionId""=st.""Id"" and st.""Code""='APPROVE'
            //left join cms.""Job"" as j on  jd.""JobId"" =j.""Id"" 
            //left join rec.""ListOfValue"" as mt on jd.""ManpowerTypeId""=mt.""Id""
            //left join rec.""ListOfValue"" as cat on jd.""JobCategoryId""=cat.""Id""
            //left join cms.""Location"" as loc on jd.""LocationId""=loc.""Id"" 
            //where jd.""IsDeleted""=false and jd.""Status""=1 and (jd.""ExpiryDate"" is null or jd.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}') " ;

            var agencyid = "";
            var agencys = await _repo.GetSingleGlobal<AgencyViewModel, Agency>(x => x.UserId == agency);
            if (agencys != null)
            {
                agencyid = agencys.Id;
            }

            string query = @"select jd.*,j.""Name"" as JobName,mt.""Name"" as ManpowerTypeName,mt.""Id"" as ManpowerType,
            cat.""Name"" as JobCategoryName,loc.""Name"" as LocationName
            from rec.""JobAdvertisement"" as jd  
            join rec.""ListOfValue"" as st on jd.""ActionId""=st.""Id"" and st.""Code""='APPROVE'
            left join cms.""Job"" as j on  jd.""JobId"" =j.""Id"" 
            left join rec.""ListOfValue"" as mt on mt.""ListOfValueType""='LOV_MANPOWERTYPE' and mt.""Code"" = j.""ManpowerTypeCode""
            left join rec.""ListOfValue"" as cat on jd.""JobCategoryId""=cat.""Id""
            left join cms.""Location"" as loc on jd.""LocationId""=loc.""Id"" 
            where jd.""IsDeleted""=false and jd.""Status""=1 and (jd.""AgencyId"" is null or jd.""AgencyId""='{}' ";

            query = $@"{query} or '{agencyid}'=ANY(jd.""AgencyId"")) and (jd.""ExpiryDate"" is null or jd.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}') ";

            if (keyWord.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and j.""Name"" ILIKE '%{keyWord}%' COLLATE ""tr-TR-x-icu"" ";
            }
            if (categoryId.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and jd.""JobCategoryId""='{categoryId}'";
            }
            if (locationId.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and jd.""LocationId""='{locationId}'";
            }
            if (manpowerTypeId.IsNotNullAndNotEmpty() && manpowerTypeId!="0")
            {
                query = @$"{query} and mt.""Id""='{manpowerTypeId}'";
            }
            query = @$"{query} order by jd.""ApprovedDate"" desc";
            var queryData = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<ListOfValueViewModel>> GetJobAdvertisementListWithCount(string agency)
        {
            var agencyid = "";
            var agencys = await _repo.GetSingleGlobal<AgencyViewModel, Agency>(x => x.UserId == agency);
            if (agencys != null)
            {
                agencyid = agencys.Id;
            }

            string query = @$"select cat.*,CAST (count(jd.""Id"") AS TEXT) as JobCount,par.""Name"" as ParentName
            from rec.""ListOfValue"" as cat
            left join rec.""JobAdvertisement"" as jd on jd.""JobCategoryId""=cat.""Id""
            and jd.""IsDeleted""=false and jd.""Status""=1 and (jd.""ExpiryDate"" is null or jd.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}')
            left join rec.""ListOfValue"" as st on jd.""ActionId""=st.""Id"" and st.""Code""='APPROVE'
            left join rec.""ListOfValue"" as par on par.""Id""=cat.""ParentId""
            where cat.""ListOfValueType""= 'JOB_CATEGORY' and (jd.""AgencyId"" is null ";

            query = query+@"or jd.""AgencyId""='{}' ";

            query = @$"{query} or '{agencyid}'=ANY(jd.""AgencyId""))
            --and cat.""SequenceOrder"" is not null
            group by cat.""Id"" ,par.""Name""
           ";
            var queryData = await _querylovRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobAdvertisement(string jobid,string rolid,StatusEnum status)
        {
            var jobstatus = 1;
            if(status==StatusEnum.Inactive)
            {
                jobstatus = 2;
            }
            
//            string query = @$"SELECT ja.*,job.""Name"" as JobName,nat.""Name"" as NationalityName,loc.""Name"" as LocationName,
//lovQ.""Name"" as QualificationName,lovJ.""Name"" as JobCategoryName,lovM.""Name"" as ManpowerTypeName
//    FROM rec.""JobAdvertisement"" as ja
//    left join cms.""Job"" as job on job.""Id"" = ja.""JobId""
//    left join cms.""Nationality"" as nat on nat.""Id"" = ja.""NationalityId""
//    left join cms.""Location"" as loc on loc.""Id"" = ja.""LocationId""
//    left join rec.""ListOfValue"" as lovQ on lovQ.""Id"" = ja.""Qualification""
//    left join rec.""ListOfValue"" as lovJ on lovJ.""Id"" = ja.""JobCategoryId""
//    left join rec.""ListOfValue"" as lovM on lovM.""Id"" = ja.""ManpowerTypeId""
//where ja.""JobId"" = '{jobid}' and ja.""RoleId"" = '{rolid}' and ja.""Status""='{jobstatus}' ";

            string query = @$"SELECT ja.*,job.""Name"" as JobName,nat.""Name"" as NationalityName,loc.""Name"" as LocationName,
lovQ.""Name"" as QualificationName,lovJ.""Name"" as JobCategoryName,lovM.""Name"" as ManpowerTypeName
    FROM rec.""JobAdvertisement"" as ja
    left join cms.""Job"" as job on job.""Id"" = ja.""JobId""
    left join cms.""Nationality"" as nat on nat.""Id"" = ja.""NationalityId""
    left join cms.""Location"" as loc on loc.""Id"" = ja.""LocationId""
    left join rec.""ListOfValue"" as lovQ on lovQ.""Id"" = ja.""Qualification""
    left join rec.""ListOfValue"" as lovJ on lovJ.""Id"" = ja.""JobCategoryId""
    left join rec.""ListOfValue"" as lovM on lovM.""ListOfValueType""='LOV_MANPOWERTYPE' and lovM.""Code"" = job.""ManpowerTypeCode""
where ja.""JobId"" = '{jobid}' and ja.""Status""='{jobstatus}' ";
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<JobAdvertisementViewModel> GetCalculatedData(string id)
        {

            string query1 = @$"SELECT j.*,
                            sum(case when aps.""Code""='ShortListByHr' then 1 else 0 end) as ShortlistedByHrCalculated,
                            sum(case when aps.""Code"" = 'ShortListByHm' then 1 else 0 end) as ShortlistedForInterviewCalculated,
                            sum(case when aps.""Code"" = 'InterViewed' then 1 else 0 end) as InterviewCompletedCalculated,
                            sum(case when aps.""Code"" = 'Selected' then 1 else 0 end) as FinalOfferAcceptedCalculated,
                            sum(case when aps.""Code"" = 'Joined' then 1 else 0 end) as CandidateJoinedCalculated                            
                            FROM rec.""JobAdvertisement"" as j
                            LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" = j.""Id""
                            LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
                            where j.""Id""='{id}'
                            group by j.""Id""";
            var list = await _queryRepo.ExecuteQueryList(query1, null);
            return list.FirstOrDefault();
        }

        public async Task UpdateJobAdvertisementStatus()
        {
            string query = @$"update rec.""JobAdvertisement"" set ""Status""='2' where ""ExpiryDate"" <= '{DateTime.Today.ToDatabaseDateFormat()}' ";
            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);
        }

        public async Task<List<ApplicationViewModel>> GetBookmarksJobList(string jobIds)
        {
            string query = @$"select jadv.""Id"", j.""Name"" as JobName,j.""ManpowerTypeCode"", jc.""Name"" as JobCategoryName, loc.""Name"" as LocationName
                                from rec.""JobAdvertisement"" as jadv
                                left join cms.""Job"" as j on jadv.""JobId"" = j.""Id""
                                left join rec.""ListOfValue"" as jc on jadv.""JobCategoryId"" = jc.""Id""
                                left join cms.""Location"" as loc on jadv.""LocationId"" = loc.""Id""
                                where jadv.""Id"" in ({jobIds}) ";

            var queryData = await _queryAppRepo.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<List<ApplicationViewModel>> GetJobAdvertisementByAgency()
        {
            var agencyid = "";
            var agency = await _repo.GetSingleGlobal<AgencyViewModel, Agency>(x => x.UserId == _repo.UserContext.UserId);
            if (agency != null)
            {
                agencyid = agency.Id;
            }
           

            string query = @$"select jd.""Id"",j.""Name"" as JobName,mt.""Code"" as ManpowerTypeCode,mt.""Id"" as ManpowerType,
            cat.""Name"" as JobCategoryName,loc.""Name"" as LocationName
            from rec.""JobAdvertisement"" as jd  
            join rec.""ListOfValue"" as st on jd.""ActionId""=st.""Id"" and st.""Code""='APPROVE'
            left join cms.""Job"" as j on  jd.""JobId"" =j.""Id"" 
            left join rec.""ListOfValue"" as mt on mt.""ListOfValueType""='LOV_MANPOWERTYPE' and mt.""Code"" = j.""ManpowerTypeCode""
            left join rec.""ListOfValue"" as cat on jd.""JobCategoryId""=cat.""Id""
            left join cms.""Location"" as loc on jd.""LocationId""=loc.""Id"" 
            where jd.""IsDeleted""=false and jd.""Status""=1 and (jd.""ExpiryDate"" is null or jd.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}')
             and '{agencyid}'=ANY(jd.""AgencyId"")";

           
            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }
    }
}
