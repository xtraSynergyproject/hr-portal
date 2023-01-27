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

namespace Synergy.App.Business.Implementations.BusinessQueryPostgre
{
    class RecruitmentQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IRecruitmentQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<JobAdvertisementViewModel> _queryRepoJob;
        public RecruitmentQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<JobAdvertisementViewModel> queryRepoJob) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
            _queryRepoJob = queryRepoJob;
        }

        public async Task<IList<JobAdvertisementViewModel>> GetJobAdvertisement(string jobid, string rolid, StatusEnum status)
        {
            var jobstatus = 1;
            if (status == StatusEnum.Inactive)
            {
                jobstatus = 2;
            }

            string query = @$"SELECT ja.*,job.""JobTitle"" as JobName,nat.""NationalityName"" ,loc.""LocationName"",
lovQ.""Name"" as QualificationName,lovJc.""Name"" as JobCategoryName,lovM.""Name"" as ManpowerTypeName
    FROM cms.""N_REC_JobAdvertisement"" as ja
    left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = ja.""JobId""
    left join cms.""N_CoreHR_HRNationality"" as nat on nat.""Id"" = ja.""NationalityId""
    left join cms.""N_CoreHR_HRLocation"" as loc on loc.""Id"" = ja.""JobLocationId""
    left join public.""LOV"" as lovQ on lovQ.""Id"" = ja.""QualificationId""
    left join public.""LOV"" as lovJc on lovJc.""Id"" = ja.""JobCategoryId""
    left join public.""LOV"" as lovM on lovM.""LOVType""='REC_MANPOWER' and lovM.""Id"" = job.""ManpowerTypeId""
where ja.""JobId"" = '{jobid}' and ja.""Status""='{jobstatus}' ";

            var queryData = await _queryRepoJob.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<JobAdvertisementViewModel>> GetSelectedJobAdvertisement(string vacancyId)
        {            
            string query = @$"SELECT ja.*,p.""PositionName"",job.""JobTitle"" as JobName,nat.""NationalityName"" ,loc.""LocationName"",
lovQ.""Name"" as QualificationName,lovJc.""Name"" as JobCategoryName,lovM.""Name"" as ManpowerTypeName
    FROM cms.""F_CRPF_RECRUITMENT_SelectedJobAdvertisement"" as sja
	join public.""NtsService"" as s on sja.""JobAdvertisementId""=s.""Id"" and s.""IsDeleted""=false
	join cms.""N_REC_JobAdvertisement"" as ja on s.""UdfNoteTableId""=ja.""Id"" and ja.""IsDeleted""=false
	left join cms.""N_CoreHR_HRPosition"" as p on ja.""PositionId""=p.""Id"" and p.""IsDeleted""=false
    left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = ja.""JobId""
    left join cms.""N_CoreHR_HRNationality"" as nat on nat.""Id"" = ja.""NationalityId""
    left join cms.""N_CoreHR_HRLocation"" as loc on loc.""Id"" = ja.""JobLocationId""
    left join public.""LOV"" as lovQ on lovQ.""Id"" = ja.""QualificationId""
    left join public.""LOV"" as lovJc on lovJc.""Id"" = ja.""JobCategoryId""
    left join public.""LOV"" as lovM on lovM.""LOVType""='REC_MANPOWER' and lovM.""Id"" = job.""ManpowerTypeId""
where sja.""VacancyListId""='{vacancyId}' and sja.""IsDeleted""=false ";

            var queryData = await _queryRepoJob.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<IList<JobAdvertisementViewModel>> GetJobAdvList()
        {          

            string query = @$"SELECT ja.*,s.""Id"" as ServiceId, job.""JobTitle"" as JobName,nat.""NationalityName"" ,loc.""LocationName"",
lovQ.""Name"" as QualificationName,lovJc.""Name"" as JobCategoryName,lovM.""Name"" as ManpowerTypeName
    FROM cms.""N_REC_JobAdvertisement"" as ja
    join public.""NtsService"" as s on ja.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
    left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = ja.""JobId""
    left join cms.""N_CoreHR_HRNationality"" as nat on nat.""Id"" = ja.""NationalityId""
    left join cms.""N_CoreHR_HRLocation"" as loc on loc.""Id"" = ja.""JobLocationId""
    left join public.""LOV"" as lovQ on lovQ.""Id"" = ja.""QualificationId""
    left join public.""LOV"" as lovJc on lovJc.""Id"" = ja.""JobCategoryId""
    left join public.""LOV"" as lovM on lovM.""LOVType""='REC_MANPOWER' and lovM.""Id"" = job.""ManpowerTypeId""
join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE')
where ja.""IsDeleted""=false ";

            var queryData = await _queryRepoJob.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
