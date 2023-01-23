using AutoMapper;
using Newtonsoft.Json;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class RecQueryPostgreBusiness : BusinessBase<ServiceViewModel, NtsService>, IRecQueryBusiness
    {
        private readonly IUserContext _userContext;
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<JobDescriptionCriteriaViewModel> _queryRepoCri;
        private readonly IRepositoryQueryBase<JobAdvertisementViewModel> _queryJobAdv;
        private readonly IRepositoryQueryBase<JobAdvertisementViewModel> _queryRepoJob;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryIdName;
        private readonly IRepositoryQueryBase<ManpowerRecruitmentSummaryViewModel> _queryMPRSummary;
        private readonly IRepositoryQueryBase<ApplicationViewModel> _queryApp;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        public RecQueryPostgreBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo
            , IMapper autoMapper
            , IUserContext userContext
            , ILOVBusiness lovBusiness
            , INoteBusiness noteBusiness
            , IRepositoryQueryBase<ServiceViewModel> queryRepo, IRepositoryQueryBase<JobAdvertisementViewModel> queryJobAdv,
            IRepositoryQueryBase<JobDescriptionCriteriaViewModel> queryRepoCri,
            IRepositoryQueryBase<JobAdvertisementViewModel> queryRepoJob, IRepositoryQueryBase<IdNameViewModel> queryIdName,
            IRepositoryQueryBase<ManpowerRecruitmentSummaryViewModel> queryMPRSummary,
            IRepositoryQueryBase<ApplicationViewModel> queryApp, ITemplateBusiness templateBusiness
            , ITemplateCategoryBusiness templateCategoryBusiness,ITableMetadataBusiness tableMetadataBusiness) : base(repo, autoMapper)
        {
            _userContext = userContext;
            _queryRepo = queryRepo;
            _queryRepoCri = queryRepoCri;
            _queryJobAdv = queryJobAdv;
            _queryRepoJob = queryRepoJob;
            _lovBusiness = lovBusiness;
            _queryIdName = queryIdName;
            _queryMPRSummary = queryMPRSummary;
            _queryApp = queryApp;
            _noteBusiness = noteBusiness;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;

        }

        #region N

        public async Task<List<ListOfValueViewModel>> GetJobAdvertisementListWithCount(string agency)
        {
            var agencyid = "";
            var agencys = await _repo.GetSingleGlobal<AgencyViewModel, Agency>(x => x.UserId == agency);
            if (agencys != null)
            {
                agencyid = agencys.Id;
            }

            //doubt in query - ActionId udf??? - USE SERVICESTATUS

            string query = @$"select cat.*,CAST (count(jd.""Id"") AS TEXT) as JobCount,par.""Name"" as ParentName
            from public.""LOV"" as cat
            left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobCategoryId""=cat.""Id""
            and jd.""IsDeleted""=false and jd.""Status""=1 and (jd.""ExpiryDate"" is null or jd.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}')

            --left join public.""LOV"" as st on jd.""ActionId""=st.""Id"" and st.""Code""='APPROVE'
            left join public.""LOV"" as par on par.""Id""=cat.""ParentId""
            where cat.""LOVType""= 'REC_JOB_CATEGORY' and (jd.""AgencyId"" is null ";

            query = query + @"or jd.""AgencyId""='{}' ";

            query = @$"{query} 
            --or '{agencyid}'=ANY(jd.""AgencyId"")
            )
            --and cat.""SequenceOrder"" is not null
            group by cat.""Id"" ,par.""Name""
           ";
            var queryData = await _queryRepo.ExecuteQueryList<ListOfValueViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JobAdvertisementViewModel>> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerTypeId, string agency)
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

            string query = @"select jd.*,j.""JobTitle"" as JobName,mt.""Name"" as ManpowerTypeName,mt.""Id"" as ManpowerType,
            cat.""Name"" as JobCategoryName,loc.""LocationName"" as LocationName
            from cms.""N_REC_JobAdvertisement"" as jd  
            
            join public.""NtsService"" as s on s.""UdfNoteTableId"" = jd.""Id""
            join public.""LOV"" as st on s.""ServiceStatusId""=st.""Id"" and st.""Code""='SERVICE_STATUS_COMPLETE'

            left join cms.""N_CoreHR_HRJob"" as j on  jd.""JobId"" =j.""Id"" 
            left join public.""LOV"" as mt on mt.""LOVType""='REC_MANPOWER' and mt.""Id"" = j.""ManpowerTypeId""
            left join public.""LOV"" as cat on jd.""JobCategoryId""=cat.""Id""
            left join cms.""N_CoreHR_HRLocation"" as loc on jd.""JobLocationId""=loc.""Id"" 
            where jd.""IsDeleted""=false and jd.""Status""=1 
            --and (jd.""AgencyId"" is null or jd.""AgencyId""='{}' ";

            query = $@"{query} 
                    -- or '{agencyid}'=ANY(jd.""AgencyId""))
                    and (jd.""ExpiryDate"" is null or jd.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}') ";

            if (keyWord.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and j.""JobTitle"" ILIKE '%{keyWord}%' COLLATE ""tr-TR-x-icu"" ";
            }
            if (categoryId.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and jd.""JobCategoryId""='{categoryId}'";
            }
            if (locationId.IsNotNullAndNotEmpty())
            {
                query = @$"{query} and jd.""LocationId""='{locationId}'";
            }
            if (manpowerTypeId.IsNotNullAndNotEmpty() && manpowerTypeId != "0")
            {
                query = @$"{query} and mt.""Id""='{manpowerTypeId}'";
            }
            query = @$"{query} order by jd.""CreatedDate"" desc";
            var queryData = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query, null);
            return queryData;
        }

        public async Task<List<JobAdvertisementViewModel>> GetJobAdvertisementListByJobId(string jobId) 
        {
            string query = $@"Select * from cms.""N_REC_JobAdvertisement"" where ""JobId""='{jobId}' ";
            var queryData = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query, null);
            return queryData;
        }

        public async Task<JobAdvertisementViewModel> GetNameById(string jobAdvId)
        {
            string query = @$"SELECT c.*,
                                ct.""LocationName"" as LocationName, q.""Name"" as QualificationName, j.""Name"" as JobCategoryName, jo.""JobTitle"" as JobName
                                FROM cms.""N_REC_JobAdvertisement"" as c
                                LEFT JOIN cms.""N_CoreHR_HRLocation"" as ct ON ct.""Id"" = c.""JobLocationId""
                                LEFT JOIN cms.""N_CoreHR_HRJob"" as jo ON jo.""Id"" = c.""JobId""
                                LEFT JOIN public.""LOV"" as q ON q.""Id"" = c.""QualificationId""
                                LEFT JOIN public.""LOV"" as j ON j.""Id"" = c.""JobCategoryId""
                                WHERE c.""IsDeleted""=false AND c.""Id""='{jobAdvId}'";


            var queryData = await _queryRepo.ExecuteQuerySingle<JobAdvertisementViewModel>(query, null);
            return queryData;
        }

        public async Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string JobId)
        {

            string query = @$"select j.""Id"" as Id ,j.""JobTitle""  as JobName,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode 
                         from cms.""N_CoreHR_HRJob"" as j
                                            
                        inner join public.""LOV"" as lov on lov.""LOVType""='REC_MANPOWER' and lov.""Id""=j.""ManpowerTypeId""
                        WHERE j.""Id""='{JobId}'";
            var queryData = await _queryRepo.ExecuteQuerySingle<JobAdvertisementViewModel>(query, null);
            return queryData;
        }

        public async Task<List<CandidateExperienceViewModel>> GetListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT c.*, f.""FileName"" as AttachmentName	
                                FROM cms.""N_REC_CANDIDATE_EXPERIENCE"" as c                                
                                LEFT JOIN public.""File"" as f ON f.""Id"" = c.""AttachmentId""
                                where c.""CandidateId""='{candidateProfileId}' and c.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceViewModel>(query, null);
            //var list = new List<CandidateEducationalViewModel>();

            return queryData;
        }

        public async Task<List<CandidateEducationalViewModel>> GetListByCandidate(QualificationTypeEnum qualificationType, string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                            q.""Name"" as QualificationName,
                              s.""Name"" as SpecializationName, e.""Name"" as EducationTypeName,
                                ct.""CountryName"" as CountryName, d.""FileName"" as AttachmentName	
                                FROM cms.""N_REC_CANDIDATE_EDUCATIONAL"" as c
                                LEFT JOIN public.""LOV"" as q ON q.""Id"" = c.""QualificationId""
                                LEFT JOIN public.""LOV"" as s ON s.""Id"" = c.""SpecializationId""
                                LEFT JOIN public.""LOV"" as e ON e.""Id"" = c.""EducationTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as ct ON ct.""Id"" = c.""CountryId""
                                LEFT JOIN public.""File"" as d ON d.""Id"" = c.""AttachmentId""
                                WHERE c.""IsDeleted""='false' AND c.""CandidateId""='" + candidateProfileId + "'";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList<CandidateEducationalViewModel>(query, null);
            var list = queryData.Where(x => x.QualificationTypeId == qualificationType).ToList();
            return list;
        }

        public async Task<Tuple<CandidateProfileViewModel, bool>> IsCandidateProfileFilled()
        {
            var result = new Tuple<CandidateProfileViewModel, bool>(null, false);
            string query = @$"select cp.* From cms.""N_REC_REC_CANDIDATE"" as cp
            Join public.""User"" as u on cp.""UserId"" = u.""Id""
            where u.""Id"" = '{_repo.UserContext.UserId}'";
            var data = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);

            if (data != null)
            {
                bool IsCandExp = false;
                bool IsCandEdu = false;
                var candExp = await GetListByCandidate(data.Id);
                if (candExp.IsNotNull() && candExp.Count() > 0)
                {
                    IsCandExp = true;
                }
                var candEdu = await GetListByCandidate(QualificationTypeEnum.Educational, data.Id);
                if (candEdu.IsNotNull() && candEdu.Count() > 0)
                {
                    IsCandEdu = true;
                }
                result = new Tuple<CandidateProfileViewModel, bool>(data, false);
                if (
                    data.FirstName.IsNotNullAndNotEmpty()
                    && data.LastName.IsNotNullAndNotEmpty()
                    && data.Age.IsNotNull()
                    && data.BirthDate != null
                    && data.BirthPlace.IsNotNullAndNotEmpty()
                    //&& data.BloodGroup.IsNotNullAndNotEmpty()
                    && data.GenderId.IsNotNullAndNotEmpty()
                    && data.MaritalStatusId.IsNotNullAndNotEmpty()
                    && data.PassportNumber.IsNotNullAndNotEmpty()
                    && data.PassportIssueCountryId.IsNotNullAndNotEmpty()
                    && data.PassportExpiryDate != null
                    && data.ResumeId.IsNotNullAndNotEmpty()
                    && data.ContactPhoneLocal.IsNotNullAndNotEmpty()
                    && data.Email.IsNotNullAndNotEmpty()
                    && IsCandExp == true
                    && data.NetSalary.IsNotNullAndNotEmpty()
                    && data.OtherAllowances.IsNotNullAndNotEmpty()
                    && data.TimeRequiredToJoin != null
                    && IsCandEdu == true
                    )
                {
                    result = new Tuple<CandidateProfileViewModel, bool>(data, true);
                }
            }
            return result;
        }

        public async Task<ApplicationViewModel> GetApplicationData(string Id, string jobAdvId)
        {
            var query = $@" select * from cms.""N_REC_APPLICATION"" where ""CandidateId"" = '{Id}' and ""JobAdvertisementId"" = '{jobAdvId}' and ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<ApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<ApplicationViewModel> GetApplicationDataByCandidateIdandJobId(string Id, string jobId)
        {
            var query = $@" select * from cms.""N_REC_APPLICATION"" where ""CandidateId"" = '{Id}' and ""JobId"" = '{jobId}' and ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<ApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> GetCandidateDataByUserId(string userId)
        {
            var query = $@" select *, ""NtsNoteId"" as CandidateNoteId
                            from cms.""N_REC_REC_CANDIDATE"" where ""UserId"" = '{userId}' and ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ApplicationViewModel>> GetApplicationListByCandidateId(string candidateId)
        {
            var query = $@" select *, ""NtsNoteId"" as ApplicationNoteId 
                            from cms.""N_REC_APPLICATION"" 
                            where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<RecApplicationViewModel> GetApplicationDetailsById(string appId)
        {
            var query = $@" select app.*, app.""NtsNoteId"" as ApplicationNoteId ,c.""UserId"" as ApplicationUserId
                            from cms.""N_REC_APPLICATION"" as app
                            join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId"" 
                            where app.""Id"" = '{appId}' and app.""IsDeleted"" = false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<RecApplicationViewModel> GetAppointmentDetailsById(string appId)
        {
            var query = $@" select * from cms.""N_REC_CRPFAppointment""  
                            where ""Id"" = '{appId}' and ""IsDeleted"" = false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<RecApplicationViewModel> GetApplicationDetail(string applicationId)
        {
          string query = @$"select cp.*, vt.""Code"" as VisaCategoryCode,mt.""Code"" as ManpowerTypeCode,mt.""Name"" as ManpowerTypeName
                                From cms.""N_REC_APPLICATION"" as cp
                                left join public.""LOV"" as vt on vt.""Id""=cp.""VisaCategoryId""
                                 left join cms.""N_CoreHR_HRJob"" as j on  cp.""JobId"" =j.""Id"" 
                                left join public.""LOV"" as mt on mt.""LOVType""='REC_MANPOWERTYPE' and mt.""Code"" = j.""ManpowerTypeId""
           

                                where cp.""Id"" = '{applicationId}'";


            var querydocData = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return querydocData;
        }
        public async Task<ApplicationViewModel> GetApplicationById(string appId)
        {
            var query = $@" select pl.*, pl.""NtsNoteId"" as ApplicationNoteId,pc.""CountryName"" as PermanentAddressCountryName
                            from cms.""N_REC_APPLICATION"" as pl
                            LEFT JOIN cms.""N_CoreHR_HRCountry"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                            where pl.""Id"" = '{appId}' and pl.""IsDeleted"" = false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<ApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> GetApplicationforOfferById(string appId)
        {
            var query = $@" select pl.*,c.*, pl.""NtsNoteId"" as ApplicationNoteId,pc.""CountryName"" as PermanentAddressCountryName
                            from cms.""N_REC_APPLICATION"" as pl
                            join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=pl.""CandidateId"" 
                            LEFT JOIN cms.""N_CoreHR_HRCountry"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                            where pl.""Id"" = '{appId}' and pl.""IsDeleted"" = false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        public async Task<long> GenerateFinalOfferSeq()
        {
            string query = @$"SELECT count(*) FROM cms.""N_REC_APPLICATION"" WHERE ""FinalOfferReference"" IS Not NULL and ""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteScalar<long>(query, null);
            return result;
        }

        public async Task<CandidateProfileViewModel> GetApplicationDetails(string candidateProfileId, string jobAdvId)
        {
            string query = @$"select pl.*, n.""NationalityName"" as NationalityName, g.""Name"" as GenderName, m.""Name"" as MaritalStatusName,
                                pp.""CountryName"" as PassportIssueCountryName, vc.""CountryName"" as VisaCountryName, vt.""Name"" as VisaTypeName, ocv.""CountryName"" as OtherCountryVisaName,
                                ocvt.""Name"" as OtherCountryVisaTypeName, cc.""CountryName"" as CurrentAddressCountryName, pc.""CountryName"" as PermanentAddressCountryName
                                FROM cms.""N_REC_APPLICATION"" as pl
                                LEFT JOIN cms.""N_CoreHR_HRNationality"" as n ON n.""Id"" = pl.""NationalityId""
                                LEFT JOIN public.""LOV"" as g ON g.""Id"" = pl.""GenderId""
                                LEFT JOIN public.""LOV"" as m ON m.""Id"" = pl.""MaritalStatusId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as pp ON pp.""Id"" = pl.""PassportIssueCountryId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as vc ON vc.""Id"" = pl.""VisaCountry""
                                LEFT JOIN public.""LOV"" as vt ON vt.""Id"" = pl.""VisaTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as ocv ON ocv.""Id"" = pl.""OtherCountryVisa""
                                LEFT JOIN public.""LOV"" as ocvt ON ocvt.""Id"" = pl.""OtherCountryVisaTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as cc ON cc.""Id"" = pl.""CurrentAddressCountryId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                                WHERE pl.""CandidateId"" = '{candidateProfileId}' and pl.""IsDeleted"" = false #WHERE# ";

            var where = $@" ";
            if (jobAdvId.IsNotNullAndNotEmpty())
            {
                where = @$" and pl.""JobAdvertisementId"" = '{jobAdvId}' ";
            }

            query = query.Replace("#WHERE#", where);

            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> GetApplicationDetailsUsingAppId(string applicationId, string jobAdvId)
        {
            string query = @$"select pl.*, n.""NationalityName"" as NationalityName, g.""Name"" as GenderName, m.""Name"" as MaritalStatusName,
                                pp.""CountryName"" as PassportIssueCountryName, vc.""CountryName"" as VisaCountryName, vt.""Name"" as VisaTypeName, ocv.""CountryName"" as OtherCountryVisaName,
                                ocvt.""Name"" as OtherCountryVisaTypeName, cc.""CountryName"" as CurrentAddressCountryName, pc.""CountryName"" as PermanentAddressCountryName
                                FROM cms.""N_REC_APPLICATION"" as pl
                                LEFT JOIN cms.""N_CoreHR_HRNationality"" as n ON n.""Id"" = pl.""NationalityId""
                                LEFT JOIN public.""LOV"" as g ON g.""Id"" = pl.""GenderId""
                                LEFT JOIN public.""LOV"" as m ON m.""Id"" = pl.""MaritalStatusId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as pp ON pp.""Id"" = pl.""PassportIssueCountryId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as vc ON vc.""Id"" = pl.""VisaCountry""
                                LEFT JOIN public.""LOV"" as vt ON vt.""Id"" = pl.""VisaTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as ocv ON ocv.""Id"" = pl.""OtherCountryVisa""
                                LEFT JOIN public.""LOV"" as ocvt ON ocvt.""Id"" = pl.""OtherCountryVisaTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as cc ON cc.""Id"" = pl.""CurrentAddressCountryId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                                WHERE pl.""Id"" = '{applicationId}' and pl.""IsDeleted"" = false #WHERE# ";

            var where = $@" ";
            if (jobAdvId.IsNotNullAndNotEmpty())
            {
                where = @$" and pl.""JobAdvertisementId"" = '{jobAdvId}' ";
            }

            query = query.Replace("#WHERE#", where);

            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        // ApplicationJobCriteria == N_REC_APPLICATION_JOB_CRITERIA
        public async Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByApplicationIdAndType(string ApplicationId, string type)
        {

            string query1 = @$"SELECT a.*,g.""Code"" as CriteriaTypeCode            
                            FROM cms.""N_REC_APPLICATION_JOB_CRITERIA"" as a
                            LEFT JOIN public.""LOV"" as g ON g.""Id"" = a.""CriteriaTypeId""
                            where a.""ApplicationId""='{ApplicationId}' and a.""Type""='{type}'
                            ";
            var list = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query1, null);
            return list;
        }

        public async Task<CandidateProfileViewModel> GetCandProfileDetails(string candidateProfileId)
        {
            string query = @$"select pl.*, pl.""NtsNoteId"" as CandidateNoteId ,TRUNC(pl.""TotalWorkExperience""::decimal,0) as TotalWorkExperienceYear,t.""Name"" as TitleName, n.""NationalityName"" as NationalityName, g.""Name"" as GenderName, m.""Name"" as MaritalStatusName,
                                pp.""CountryName"" as PassportIssueCountryName, vc.""CountryName"" as VisaCountryName, vt.""Name"" as VisaTypeName, ocv.""CountryName"" as OtherCountryVisaName,
                                ocvt.""Name"" as OtherCountryVisaTypeName, cc.""CountryName"" as CurrentAddressCountryName, pc.""CountryName"" as PermanentAddressCountryName, sc.""Name"" as SalaryCurrencyName

                                FROM cms.""N_REC_REC_CANDIDATE"" as pl
                                LEFT JOIN public.""LOV"" as t ON t.""Id"" = pl.""TitleId""
                                LEFT JOIN cms.""N_CoreHR_HRNationality"" as n ON n.""Id"" = pl.""nationality""
                                LEFT JOIN public.""LOV"" as g ON g.""Id"" = pl.""GenderId""
                                LEFT JOIN public.""LOV"" as m ON m.""Id"" = pl.""MaritalStatusId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as pp ON pp.""Id"" = pl.""PassportIssueCountryId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as vc ON vc.""Id"" = pl.""VisaCountry""
                                LEFT JOIN public.""LOV"" as vt ON vt.""Id"" = pl.""VisaTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as ocv ON ocv.""Id"" = pl.""OtherCountryVisa""
                                LEFT JOIN public.""LOV"" as ocvt ON ocvt.""Id"" = pl.""OtherCountryVisaTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as cc ON cc.""Id"" = pl.""CurrentAddressCountryId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                                LEFT JOIN cms.""N_CoreHR_HRCurrency"" as sc ON sc.""Id"" = pl.""NetSalaryCurrency""
                                WHERE pl.""Id"" = '{candidateProfileId}' and pl.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        // JobCriteria == N_REC_JOB_ADV_CRITERIA
        public async Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByJobAndType(string JobAdvertisementId, string type)
        {

            string query1 = @$"SELECT j.""Id"" as Id,j.""Criteria"" as Criteria,j.""CriteriaTypeId"" as CriteriaType,g.""Code"" as CriteriaTypeCode,j.""Type"" as Type,j.""Weightage"" as Weightage,l.""Code"" as ListOfValueType              
                            ,j.""ListOfValueTypeId"" as ListOfValueTypeId,l.""Description"" as Description
                            --, l.""EnableDescription"" as EnableDescription
                            FROM cms.""N_REC_JOB_ADV_CRITERIA"" as j
                            LEFT JOIN public.""LOV"" as g ON g.""Id"" = j.""CriteriaTypeId""
                            LEFT JOIN public.""LOV"" as l ON l.""Id"" = j.""ListOfValueTypeId""
                            where j.""JobAdvertisementId""='{JobAdvertisementId}' and j.""Type""='{type}' and j.""IsDeleted"" = false
                            ";
            var list = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query1, null);
            if (type == "OtherInformation")
            {
                var criterialist = new List<ApplicationJobCriteriaViewModel>();
                var i = 1;
                foreach (var item in list)
                {
                    item.SequenceOrder = i;
                    if (item.CriteriaTypeCode == "LISTOFVALUE")
                    {
                        if (item.ListOfValueTypeId != null)
                        {
                            var lov = await _lovBusiness.GetList(x => x.ParentId == item.ListOfValueTypeId);
                            var temp = lov.Select(x => x.Id);
                            var child = await _lovBusiness.GetList(x => temp.Contains(x.ParentId));
                            if (child.Count > 0)
                            {
                                var criteria = new ApplicationJobCriteriaViewModel
                                {
                                    CriteriaTypeCode = "LISTOFVALUE",
                                    Type = "OtherInformation",
                                    SequenceOrder = ++i,
                                };
                                criterialist.Add(criteria);
                            }
                        }
                    }
                    i++;
                }
                if (criterialist.Count > 0)
                    list.AddRange(criterialist);
            }
            return list.OrderBy(x => x.SequenceOrder).ToList();
        }

        public async Task<List<ApplicationExperienceViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT a.*,TRUNC(a.""Duration""::decimal,1) as ""TotalDuration"", f.""FileName"" as AttachmentName	
                                FROM cms.""N_REC_APPLICATION_EXPERIENCE"" as a                                
                                LEFT JOIN public.""File"" as f ON f.""Id"" = a.""AttachmentId""
                                where a.""ApplicationId""='{candidateProfileId}' and a.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationExperienceViewModel>(query, null);
            //var list = new List<CandidateEducationalViewModel>();

            return queryData;
        }

        public async Task<List<ApplicationExperienceByCountryViewModel>> GetApplicationExpByCountryList(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                                ct.""Name"" as CountryName	
                                FROM cms.""N_REC_APPLICATION_EXPERIENCE_COUNTRY"" as c
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as ct ON ct.""Id"" = c.""CountryId""
                                WHERE c.""IsDeleted""=false AND c.""ApplicationId""='" + candidateProfileId + "'";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationExperienceByCountryViewModel>(query, null);
            return queryData;
        }


        public async Task<List<ApplicationExperienceByJobViewModel>> GetApplicationExpByJobList(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                                ct.""Name"" as JobName	
                                FROM cms.""N_REC_APPLICATION_EXPERIENCE_JOB"" as c
                                LEFT JOIN cms.""N_CoreHR_HRJob"" as ct ON ct.""Id"" = c.""JobId""
                                WHERE c.""IsDeleted""=false AND c.""ApplicationId""='" + candidateProfileId + "'";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationExperienceByJobViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ApplicationeExperienceByNatureViewModel>> GetApplicationExpByNatureList(string candidateProfileId)
        {
            string query = @$"SELECT c.*	
                                FROM cms.""N_REC_APPLICATION_EXPERIENCE_NATURE"" as c
                                WHERE c.""IsDeleted""=false AND c.""ApplicationId""='{candidateProfileId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationeExperienceByNatureViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ApplicationExperienceBySectorViewModel>> GetApplicationListBySector(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                            s.""Name"" as SectorName, i.""Name"" as IndustryName, c.""Name"" as CategoryName
                                FROM cms.""N_REC_APPLICATION_EXPERIENCE_SECTOR"" as l
                                LEFT JOIN public.""LOV"" as s ON s.""Id"" = l.""SectorId""
                                LEFT JOIN public.""LOV"" as i ON i.""Id"" = l.""IndustryId""
                                LEFT JOIN public.""LOV"" as c ON c.""Id"" = l.""CategoryId""
                                WHERE l.""ApplicationId"" = '{candidateProfileId}' and l.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationExperienceBySectorViewModel>(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<List<ApplicationProjectViewModel>> GetApplicationProjectList(string candidateProfileId)
        {
            string query = @$"SELECT c.*	
                                FROM cms.""N_REC_APPLICATION_PROJECT"" as c
                                WHERE c.""IsDeleted""=false AND c.""ApplicationId""='{candidateProfileId}' ";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationProjectViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ApplicationEducationalViewModel>> GetApplicantsEducationInfoList(QualificationTypeEnum qualificationType, string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                            q.""Name"" as QualificationName,
                              s.""Name"" as SpecializationName, e.""Name"" as EducationTypeName,
                                ct.""Name"" as CountryName, d.""FileName"" as AttachmentName	
                                FROM cms.""N_REC_APPLICATION_EDUCATIONAL"" as c
                                LEFT JOIN public.""LOV"" as q ON q.""Id"" = c.""QualificationId""
                                LEFT JOIN public.""LOV"" as s ON s.""Id"" = c.""SpecializationId""
                                LEFT JOIN public.""LOV"" as e ON e.""Id"" = c.""EducationTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as ct ON ct.""Id"" = c.""CountryId""
                                LEFT JOIN public.""File"" as d ON d.""Id"" = c.""AttachmentId""
                                WHERE c.""ApplicationId""='{candidateProfileId}' ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList<ApplicationEducationalViewModel>(query, null);
            var list = queryData.Where(x => x.QualificationType == qualificationType).ToList();
            return list;
        }

        public async Task<List<ApplicationComputerProficiencyViewModel>> GetApplicationCompProfList(string candidateProfileId)
        {
            string query = @$"SELECT c.*, lov.""Name"" as ProficiencyLevelName 
                            FROM cms.""N_REC_APPLICATION_COMP_PROFICIENCY"" as c
                            LEFT JOIN public.""LOV"" as lov ON lov.""Id"" = c.""ProficiencyLevelId""
                            WHERE c.""ApplicationId"" = '{candidateProfileId}' AND c.""IsDeleted""=false order by c.""SequenceOrder"" ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList<ApplicationComputerProficiencyViewModel>(query, null);
            return queryData;
        }

        public async Task<List<CandidateComputerProficiencyViewModel>> GetCandidateCompProfList(string candidateProfileId)
        {
            string query = @$"SELECT c.*,c.""NtsNoteId"" as ""NoteId"", lov.""Name"" as ProficiencyLevelName 
                            FROM cms.""N_REC_CANDIDATE_COMP_PROFICIENCY"" as c
                            LEFT JOIN public.""LOV"" as lov ON lov.""Id"" = c.""ProficiencyLevelId""
                           WHERE c.""CandidateId"" = '{candidateProfileId}' AND 
                             c.""IsDeleted""=false order by c.""SequenceOrder"" ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList<CandidateComputerProficiencyViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ApplicationLanguageProficiencyViewModel>> GetApplicationLangProfList(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                            l.""Name"" as LanguageName,
                              p.""Name"" as ProficiencyLevelName	
                                FROM cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as c
                                LEFT JOIN public.""LOV"" as l ON l.""Id"" = c.""LanguageId""
                                LEFT JOIN public.""LOV"" as p ON p.""Id"" = c.""ProficiencyLevelId""
                                WHERE c.""ApplicationId""='{ candidateProfileId }' AND c.""IsDeleted""=false order by c.""SequenceOrder"" ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList<ApplicationLanguageProficiencyViewModel>(query, null);
            return queryData;
        }

        public async Task<List<CandidateLanguageProficiencyViewModel>> GetCandidateLangProfList(string candidateProfileId)
        {
            string query = @$"SELECT c.*,c.""NtsNoteId"" as ""NoteId"",
                            l.""Name"" as LanguageName,
                              p.""Name"" as ProficiencyLevelName	
                                FROM cms.""N_REC_CANDIDATE_LANG_PROFICIENCY"" as c
                                LEFT JOIN public.""LOV"" as l ON l.""Id"" = c.""LanguageId""
                                LEFT JOIN public.""LOV"" as p ON p.""Id"" = c.""ProficiencyLevelId""
                                WHERE c.""CandidateId""='{ candidateProfileId }' and c.""IsDeleted""=false order by c.""SequenceOrder"" ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList<CandidateLanguageProficiencyViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ApplicationDrivingLicenseViewModel>> GetLicenseListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                            c.""Name"" as CountryName, lt.""Name"" as LicenseTypeName
                                FROM cms.""N_REC_APPLICATION_DRIVING_LICENSE"" as l
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as c ON c.""Id"" = l.""CountryId""
                                LEFT JOIN public.""LOV"" as lt ON lt.""Id"" = l.""LicenseTypeId""                                
                                WHERE l.""ApplicationId"" = '{candidateProfileId}' and l.""IsDeleted"" = false order by l.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationDrivingLicenseViewModel>(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<List<CandidateDrivingLicenseViewModel>> GetLicenseListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT l.*,l.""NtsNoteId"" as ""NoteId"",
                            c.""CountryName"" as CountryName, lt.""Name"" as LicenseTypeName
                                FROM cms.""N_REC_CANDIDATE_DRIVING_LICENSE"" as l
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as c ON c.""Id"" = l.""CountryId""
                                LEFT JOIN public.""LOV"" as lt ON lt.""Id"" = l.""LicenseTypeId""                                
                                WHERE l.""CandidateId""='{ candidateProfileId }' and l.""IsDeleted"" = false order by l.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateDrivingLicenseViewModel>(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<List<ApplicationReferencesViewModel>> GetApplicationRefList(string candidateProfileId)
        {
            string query = @$"SELECT l.*
                                FROM cms.""N_REC_APPLICATION_REFERENCES"" as l
                                WHERE l.""ApplicationId"" = '{candidateProfileId}' and l.""IsDeleted"" = false order by l.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationReferencesViewModel>(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<CandidateProfileViewModel> GetCandidateById(string id)
        {
            var query = $@" select *, ""NtsNoteId"" as CandidateNoteId from cms.""N_REC_REC_CANDIDATE"" where ""Id"" = '{id}' and ""IsDeleted"" = false ";
            var result = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return result;
        }

        


        public async Task<IdNameViewModel> GetNationalityIdByName()
        {
            var query = @$"SELECT ""Id"" FROM cms.""N_CoreHR_HRCountry"" where ""Code""='India' and ""IsDeleted""=false and ""Status""=1";
            var name = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return name;
        }

        public async Task<CandidateProfileViewModel> GetCandidateByUser()
        {
            string query = @$"select cp.* From cms.""N_REC_REC_CANDIDATE"" as cp
            Join public.""User"" as u on cp.""UserId"" = u.""Id""
            where u.""Id"" = '{_repo.UserContext.UserId}'";
            var result = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return result;
        }

        public async Task<CandidateProfileViewModel> GetCandidateByEmail()
        {
            string query = @$"select cp.* From cms.""N_REC_REC_CANDIDATE"" as cp
            Join public.""User"" as u on cp.""Email"" = u.""Email""
            where u.""Email"" = '{_repo.UserContext.Email}'";
            var result = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            if (result != null)
            {
                if (result.UserId.IsNullOrEmpty())
                {
                    result.UserId = _repo.UserContext.UserId;
                }
                if (result.SourceFrom.IsNullOrEmpty())
                {
                    result.SourceFrom = SourceTypeEnum.CareerPortal.ToString();
                }
            }
            return result;
        }

        public async Task<CandidateProfileViewModel> GetCandidateByPassportNo(string passportNo)
        {
            var query = $@" select * from cms.""N_REC_REC_CANDIDATE"" where ""PassportNumber"" = '{passportNo}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return res;
        }

        public async Task<CandidateProfileViewModel> CheckCandExitsByIdnPassportNo(string id, string passportNo)
        {
            var query = $@" select * from cms.""N_REC_REC_CANDIDATE"" where ""Id"" != '{id}' and ""PassportNumber"" = '{passportNo}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return res;
        }

        public async Task<CandidateExperienceViewModel> GetCandidateExperienceDuration(string candidateProfileId)
        {
            //string query = @$"SELECT sum(c.""Duration"") as TotalDuration	
            //                    FROM rec.""CandidateExperience"" as c                                
            //                    where c.""CandidateProfileId""='{candidateProfileId}' and c.""IsDeleted""=false";
            //string query = @$"SELECT sum(TRUNC(c.""Duration""::decimal,0)) as TotalDuration	
            //                    FROM rec.""CandidateExperience"" as c                                
            //                    where c.""CandidateProfileId""='{candidateProfileId}' and c.""IsDeleted""=false";

            string query = @$"SELECT ""From"" as From,""To"" as To	
                                FROM cms.""N_REC_CANDIDATE_EXPERIENCE"" as c                                
                                where c.""CandidateId""='{candidateProfileId}' and c.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceViewModel>(query, null);
            var day = 0;
            var month = 0;

            foreach (var item in queryData)
            {
                if (item.To != null && item.From != null)
                {
                    day += (item.To.Value - item.From.Value).Days;
                }
            }
            double years = (double)day / 365;
            var exp = new CandidateExperienceViewModel { TotalDuration = Math.Round(years, 1) };

            return exp;
        }

        public async Task UpdateTable(string type, string applicationId)
        {
            var query = "";

            if (type == "applicationExperience")
            {
                query = $@" Update cms.""N_REC_APPLICATION_EXPERIENCE"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationExperienceByCountry")
            {
                query = $@" Update cms.""N_REC_APPLICATION_EXPERIENCE_COUNTRY"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationExperienceBySector")
            {
                query = $@" Update cms.""N_REC_APPLICATION_EXPERIENCE_SECTOR"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationExperienceByNature")
            {
                query = $@" Update cms.""N_REC_APPLICATION_EXPERIENCE_NATURE"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationExperienceByJob")
            {
                query = $@" Update cms.""N_REC_APPLICATION_EXPERIENCE_JOB"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationExperienceByOther")
            {
                query = $@" Update cms.""N_REC_APPLICATION_EXPERIENCE_OTHER"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationEducational")
            {
                query = $@" Update cms.""N_REC_APPLICATION_EDUCATIONAL"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationComputerProficiency")
            {
                query = $@" Update cms.""N_REC_APPLICATION_COMP_PROFICIENCY"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationLanguageProficiency")
            {
                query = $@" Update cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationDrivingLicense")
            {
                query = $@" Update cms.""N_REC_APPLICATION_DRIVING_LICENSE"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationProject")
            {
                query = $@" Update cms.""N_REC_APPLICATION_PROJECT"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }

            else if (type == "applicationReferences")
            {
                query = $@" Update cms.""N_REC_APPLICATION_REFERENCES"" 
                            set ""IsDeleted"" = true
                            where ""ApplicationId"" = '{applicationId}' ";
            }


            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<List<CandidateExperienceViewModel>> GetCandidateExpByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_EXPERIENCE"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateExperienceViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateExperienceByCountryViewModel>> GetCandidateExpCountryByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_EXPERIENCE_COUNTRY"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateExperienceByCountryViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateExperienceBySectorViewModel>> GetCandidateExpSectorByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_EXPERIENCE_SECTOR"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateExperienceBySectorViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateExperienceByNatureViewModel>> GetCandidateExpNatureByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_EXPERIENCE_NATURE"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateExperienceByNatureViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateExperienceByJobViewModel>> GetCandidateExpJobByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_EXPERIENCE_JOB"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateExperienceByJobViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateExperienceByOtherViewModel>> GetCandidateExpOtherByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_EXPERIENCE_OTHER"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateExperienceByOtherViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateReferencesViewModel>> GetCandidateReferencesByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_REFERENCES"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateReferencesViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateProjectViewModel>> GetCandidateProjectByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_PROJECT"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateProjectViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateDrivingLicenseViewModel>> GetCandidateDrivingLicenceByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_DRIVING_LICENSE"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateDrivingLicenseViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateLanguageProficiencyViewModel>> GetCandidateLangProfByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_LANG_PROFICIENCY"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateLanguageProficiencyViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateComputerProficiencyViewModel>> GetCandidateCompProfByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_COMP_PROFICIENCY"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateComputerProficiencyViewModel>(query, null);
            return res;
        }

        public async Task<List<CandidateEducationalViewModel>> GetCandidateEduByCandidateId(string candidateId)
        {
            var query = $@" select * from cms.""N_REC_CANDIDATE_EDUCATIONAL"" where ""CandidateId"" = '{candidateId}' and ""IsDeleted"" = false ";
            var res = await _queryRepo.ExecuteQueryList<CandidateEducationalViewModel>(query, null);
            return res;
        }

        // My Application Page Queries

        // dbt -  ApplicationState and ApplicationStatus
        public async Task<List<ApplicationViewModel>> GetApplicationListByCandidate(string candidateId)
        {
            string query = @$"select pl.*,apstate.""Name"" as ApplicationStateName,apstate.""Code"" as ApplicationStateCode,j.""JobTitle"" as JobTitle
            ,cat.""Name"" as JobCategoryName,loc.""LocationName"" as LocationName,apst.""Name"" as ApplicationStatusName
            FROM cms.""N_REC_APPLICATION"" as pl
            
            left join cms.""N_CoreHR_HRJob"" as j on  pl.""JobId"" =j.""Id"" 
            left join cms.""N_REC_JobAdvertisement"" as jd  on  pl.""JobId"" =jd.""JobId"" and jd.""Status"" = 1
            left join public.""LOV"" as cat on jd.""JobCategoryId""=cat.""Id""
            left join cms.""N_CoreHR_HRLocation"" as loc on jd.""JobLocationId""=loc.""Id"" 
            LEFT JOIN public.""LOV"" as apstate ON pl.""ApplicationStateId"" = apstate.""Id"" 
            LEFT JOIN public.""LOV"" as apst ON pl.""ApplicationStatusId"" = apst.""Id""
            WHERE pl.""CandidateId"" = '{candidateId}' and pl.""IsDeleted"" = false";
            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<List<ApplicationViewModel>> GetBookmarksJobList(string jobIds)
        {
            string query = @$"select jadv.""Id"", j.""JobTitle"" as JobName,j.""ManpowerTypeId"", jc.""Name"" as JobCategoryName, loc.""LocationName"" as LocationName
                                from cms.""N_REC_JobAdvertisement"" as jadv
                                left join cms.""N_CoreHR_HRJob"" as j on jadv.""JobId"" = j.""Id""
                                left join public.""LOV"" as jc on jadv.""JobCategoryId"" = jc.""Id""
                                left join cms.""N_CoreHR_HRLocation"" as loc on jadv.""JobLocationId"" = loc.""Id""
                                where jadv.""Id"" in ({jobIds}) ";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
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


            string query = @$"select jd.""Id"",j.""JobTitle"" as JobName,mt.""Code"" as ManpowerTypeCode,mt.""Id"" as ManpowerType,
            cat.""Name"" as JobCategoryName,loc.""LocationName"" as LocationName
            from cms.""N_REC_JobAdvertisement"" as jd  
            --join public.""LOV"" as st on jd.""ActionId""=st.""Id"" and st.""Code""='APPROVE'
            left join cms.""N_CoreHR_HRJob"" as j on  jd.""JobId"" =j.""Id"" 
            left join public.""LOV"" as mt on mt.""LOVType""='REC_MANPOWERTYPE' and mt.""Code"" = j.""ManpowerTypeId""
            left join public.""LOV"" as cat on jd.""JobCategoryId""=cat.""Id""
            left join cms.""N_CoreHR_HRLocation"" as loc on jd.""JobLocationId""=loc.""Id"" 
            where jd.""IsDeleted""=false and jd.""Status""=1 and (jd.""ExpiryDate"" is null or jd.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}')
             --and '{agencyid}'=ANY(jd.""AgencyId"")
            ";


            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> GetDocumentsByCandidate(string candidateProfileId)
        {
            string query = @$"select cp.*, pp.""FileName"" as PassportAttachmentName
, pp2.""FileName"" as PassportAttachmentName2
, pp3.""FileName"" as PassportAttachmentName3
, pp4.""FileName"" as PassportAttachmentName4
, pp5.""FileName"" as PassportAttachmentName5
, ac.""FileName"" as AcademicCertificateName
, ac2.""FileName"" as AcademicCertificateName2
, ac3.""FileName"" as AcademicCertificateName3
, ac4.""FileName"" as AcademicCertificateName4
, ac5.""FileName"" as AcademicCertificateName5
, oc.""FileName"" as OtherCertificateName
, oc2.""FileName"" as OtherCertificateName2
, oc3.""FileName"" as OtherCertificateName3
, oc4.""FileName"" as OtherCertificateName4
, oc5.""FileName"" as OtherCertificateName5
, cv.""FileName"" as ResumeAttachmentName
, cl.""FileName"" as CoverLetterAttachmentName

From cms.""N_REC_REC_CANDIDATE"" as cp
Left Join public.""File"" as pp on pp.""Id"" = cp.""PassportAttachmentId""
Left Join public.""File"" as pp2 on pp.""Id"" = cp.""PassportAttachmentId2""
Left Join public.""File"" as pp3 on pp.""Id"" = cp.""PassportAttachmentId3""
Left Join public.""File"" as pp4 on pp.""Id"" = cp.""PassportAttachmentId4""
Left Join public.""File"" as pp5 on pp.""Id"" = cp.""PassportAttachmentId5""
                                Left Join public.""File"" as ac on ac.""Id"" = cp.""AcademicCertificateId""
Left Join public.""File"" as ac2 on ac.""Id"" = cp.""AcademicCertificateId2""
Left Join public.""File"" as ac3 on ac.""Id"" = cp.""AcademicCertificateId3""
Left Join public.""File"" as ac4 on ac.""Id"" = cp.""AcademicCertificateId4""
Left Join public.""File"" as ac5 on ac.""Id"" = cp.""AcademicCertificateId5""
                                Left Join public.""File"" as oc on oc.""Id"" = cp.""OtherCertificateId""
Left Join public.""File"" as oc2 on oc.""Id"" = cp.""OtherCertificateId2""
Left Join public.""File"" as oc3 on oc.""Id"" = cp.""OtherCertificateId3""
Left Join public.""File"" as oc4 on oc.""Id"" = cp.""OtherCertificateId4""
Left Join public.""File"" as oc5 on oc.""Id"" = cp.""OtherCertificateId5""
                                Left Join public.""File"" as cv on cv.""Id"" = cp.""ResumeId""
                                Left Join public.""File"" as cl on cl.""Id"" = cp.""CoverLetterId""
                                where cp.""Id"" = '{candidateProfileId}'
";

            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> GetDocumentsByApplication(string applicationId)
        {
            string query = @$"select cp.*
                            , pp.""FileName"" as PassportAttachmentName
                            , ac.""FileName"" as AcademicCertificateName
                            , oc.""FileName"" as OtherCertificateName 
                            , cv.""FileName"" as ResumeAttachmentName
                            , cl.""FileName"" as CoverLetterAttachmentName 
                                From cms.""N_REC_APPLICATION"" as cp
                                Left Join public.""File"" as pp on pp.""Id"" = cp.""PassportAttachmentId""
                                Left Join public.""File"" as ac on ac.""Id"" = cp.""AcademicCertificateId""
                                Left Join public.""File"" as oc on oc.""Id"" = cp.""OtherCertificateId""
                                Left Join public.""File"" as cv on cv.""Id"" = cp.""ResumeId""
                                Left Join public.""File"" as cl on cl.""Id"" = cp.""CoverLetterId""
                                where cp.""Id"" = '{applicationId}' and cp.""IsDeleted""=false";

            var querydocData = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return querydocData;
        }

        public async Task<long> GenerateNextDatedApplicationId()
        {
            string query = @$"SELECT  count(*) as cc FROM cms.""N_REC_APPLICATION"" as app
                                where Date(app.""CreatedDate"")=Date('{ToDD_MMM_YYYY_HHMMSS(DateTime.Now)}') and app.""IsDeleted""=false
                            ";
            var result = await _queryRepo.ExecuteScalar<long>(query, null);
            return result;
        }

        public string ToDD_MMM_YYYY_HHMMSS(DateTime value)
        {
            return String.Format("{0:yyyy-MM-dd}", value);
        }

        public async Task<IdNameViewModel> GetGrade(string Id)
        {
            string query = @$"select ""GradeName"" as Name
                                from  cms.""N_CoreHR_HRGrade""
                                where ""IsDeleted""=false and ""Status""=1 and ""Id"" = '{Id}'  order by ""GradeName""";

            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }

        #endregion


        #region s
        public async Task<bool> DeleteCandExpByCountry(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_EXPERIENCE_COUNTRY"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateExperienceByCountryViewModel> GetCandidateExperiencebyCountryDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EXPERIENCE_COUNTRY"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateExperienceByCountryViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateExperienceByCountryViewModel>> ReadCandidateExperiencebyCountry(string candidateProfileId)
        {
            var query = $@"Select ce.*, ce.""NtsNoteId"" as ""NoteId"",c.""CountryName"" as ""CountryName""
                          from cms.""N_REC_CANDIDATE_EXPERIENCE_COUNTRY""  as ce
                          join cms.""N_CoreHR_HRCountry"" as c on c.""Id""= ce.""CountryId""
                          where ce.""CandidateId""='{candidateProfileId}' and ce.""IsDeleted""=false  ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceByCountryViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandExpByJob(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_EXPERIENCE_JOB"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateExperienceByJobViewModel> GetCandidateExperiencebyJobDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EXPERIENCE_JOB"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateExperienceByJobViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateExperienceByJobViewModel>> ReadCandidateExperiencebyJob(string candidateProfileId)
        {
            var query = $@"Select je.*, je.""NtsNoteId"" as ""NoteId"",j.""JobTitle"" as ""JobName""
                          from cms.""N_REC_CANDIDATE_EXPERIENCE_JOB""  as je
                          join cms.""N_CoreHR_HRJob"" as j on j.""Id""= je.""JobId""
                          where je.""CandidateId""='{candidateProfileId}' and je.""IsDeleted""=false  ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceByJobViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandExpByNature(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_EXPERIENCE_NATURE"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateExperienceByNatureViewModel> GetCandidateExperiencebyNatureDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EXPERIENCE_NATURE"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateExperienceByNatureViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateExperienceByNatureViewModel>> ReadCandidateExperiencebyNature(string candidateProfileId)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EXPERIENCE_NATURE"" where ""CandidateId""='{candidateProfileId}' and ""IsDeleted""=false  ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceByNatureViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandExpByOther(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_EXPERIENCE_OTHER"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateExperienceByOtherViewModel> GetCandidateExperiencebyOtherDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EXPERIENCE_OTHER"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateExperienceByOtherViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateExperienceByOtherViewModel>> ReadCandidateExperiencebyOther(string candidateProfileId)
        {
            var query = $@"Select oe.*, oe.""NtsNoteId"" as ""NoteId"",lov.""Name"" as ""OtherTypeName""
                          from cms.""N_REC_CANDIDATE_EXPERIENCE_OTHER""  as oe
                         join public.""LOV"" as lov on lov.""Id""= oe.""OtherTypeId""
                          where oe.""CandidateId""='{candidateProfileId}' and oe.""IsDeleted""=false  ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceByOtherViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandExpBySector(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_EXPERIENCE_SECTOR"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateExperienceBySectorViewModel> GetCandidateExperiencebySectorDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EXPERIENCE_SECTOR"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateExperienceBySectorViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateExperienceBySectorViewModel>> ReadCandidateExperiencebySector(string candidateProfileId)
        {
            var query = $@"Select se.*, se.""NtsNoteId"" as ""NoteId"",lov1.""Name"" as ""SectorName"",lov2.""Name"" as ""IndustryName"",lov3.""Name"" as ""CategoryName""
                          from cms.""N_REC_CANDIDATE_EXPERIENCE_SECTOR""  as se
                          join public.""LOV"" as lov1 on lov1.""Id""= se.""SectorId""
                          join public.""LOV"" as lov2 on lov2.""Id""= se.""IndustryId""
                          join public.""LOV"" as lov3 on lov3.""Id""= se.""CategoryId""

                          where se.""CandidateId""='{candidateProfileId}' and se.""IsDeleted""=false   ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceBySectorViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandExpByProject(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_PROJECT"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateProjectViewModel> GetCandidateExperiencebyProjectDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_PROJECT"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateProjectViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateProjectViewModel>> ReadCandidateExperiencebyProject(string candidateProfileId)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_PROJECT"" where ""CandidateId""='{candidateProfileId}' and ""IsDeleted""=false  ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateProjectViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateReferencesViewModel>> ReadCandidateReference(string candidateProfileId)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId""
                          from cms.""N_REC_CANDIDATE_REFERENCES""  where  ""CandidateId""='{candidateProfileId}' and ""IsDeleted""=false  ";




            var queryData = await _queryRepo.ExecuteQueryList<CandidateReferencesViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandRefer(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_REFERENCES"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateReferencesViewModel> GetCandidateReferenceDetails(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_REFERENCES"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateReferencesViewModel>(query, null);
            return queryData;
        }
        public async Task<CandidateEducationalViewModel> GetCandidateEducationalbyId(string Id)
        {

            var query = $@"Select * from cms.""N_REC_CANDIDATE_EDUCATIONAL"" where ""CandidateId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateEducationalViewModel>(query, null);
            return queryData;
        }
        public async Task<CandidateEducationalViewModel> GetCandidateEducational(string Id)
        {

            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EDUCATIONAL"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateEducationalViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandidateEducational(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_EDUCATIONAL"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        //public async Task<List<CandidateEducationalViewModel>> ReadCandidateEducational(string candidateProfileId)
        //{
        //    var query = $@"Select oe.*, oe.""NtsNoteId"" as ""NoteId""--,c.""JobTitle"" as ""JobTitle"",d.""LocationName"" as ""Location""
        //                  from cms.""N_REC_CANDIDATE_EDUCATIONAL""  as oe
        //                  --join cms.""N_CoreHR_HRJob"" as c on c.""Id""= oe.""JobTitle""
        //                  --join cms.""N_CoreHR_HRLocation"" as d on d.""Id""= oe.""LocationId""
        //                  where oe.""CandidateId""='{candidateProfileId}' and oe.""IsDeleted""=false  ";

        //    var queryData = await _queryRepo.ExecuteQueryList<CandidateEducationalViewModel>(query, null);
        //    return queryData;
        //}
        public async Task<List<CandidateEducationalViewModel>> ReadCandidateEducational(QualificationTypeEnum qualificationType, string candidateProfileId)
        {
            string query = @$"SELECT c.*,c.""NtsNoteId"" as ""NoteId"",
                            q.""Name"" as QualificationName,
                              s.""Name"" as SpecializationName, e.""Name"" as EducationTypeName,
                                ct.""CountryName"" as CountryName, d.""FileName"" as AttachmentName	
                                FROM cms.""N_REC_CANDIDATE_EDUCATIONAL"" as c
                                LEFT JOIN public.""LOV"" as q ON q.""Id"" = c.""QualificationId""
                                LEFT JOIN public.""LOV"" as s ON s.""Id"" = c.""SpecializationId""
                                LEFT JOIN public.""LOV"" as e ON e.""Id"" = c.""EducationTypeId""
                                LEFT JOIN cms.""N_CoreHR_HRCountry"" as ct ON ct.""Id"" = c.""CountryId""
                                LEFT JOIN public.""File"" as d ON d.""Id"" = c.""AttachmentId""
                                WHERE c.""CandidateId""='{candidateProfileId}' and c.""IsDeleted""= false ";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList<CandidateEducationalViewModel>(query, null);
            var list = queryData.Where(x => x.QualificationTypeId == qualificationType).ToList();
            return list;
        }
        public async Task<bool> DeleteCandidateComputerProf(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_COMP_PROFICIENCY"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateComputerProficiencyViewModel> GetCandidateComputerProf(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_COMP_PROFICIENCY"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateComputerProficiencyViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandidateLanguageProf(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_LANG_PROFICIENCY"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateLanguageProficiencyViewModel> GetCandidateLanguageProf(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_LANG_PROFICIENCY"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateLanguageProficiencyViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandidateDrivingLicense(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_LANG_PROFICIENCY"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<CandidateDrivingLicenseViewModel> GetCandidateDrivingLicense(string Id)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_DRIVING_LICENSE"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateDrivingLicenseViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ApplicationJobCriteriaViewModel>> GetCriteriaData(string applicationId, string type)
        {
            //string query = @$"select jc.*, lv.""Name"" as CriteriaValue,lv1.""Name"" as ListOfValueType,lv.""Description"" as Description from rec.""ApplicationJobCriteria"" as jc
            //                    left join rec.""ListOfValue"" as lv on lv.""Id"" = jc.""Value""
            //                     left join rec.""ListOfValue"" as lv1 on lv1.""Id"" = jc.""ListOfValueTypeId""
            //                    where jc.""ApplicationId"" = '{applicationId}' and jc.""Type"" = '{type}' and jc.""IsDeleted"" = false";

            string query = @$"SELECT a.*,lv.""Name"" as CriteriaValue,lv1.""Name"" as ListOfValueType,lv.""Description"" as Description            
                            FROM cms.""N_REC_APPLICATION_JOB_CRITERIA"" as a
                            LEFT JOIN public.""LOV"" as lv ON lv.""Id"" = a.""Value""
                           LEFT JOIN public.""LOV"" as lv1 ON lv1.""Id"" = a.""ListOfValueTypeId""
                            where a.""ApplicationId""='{applicationId}' and a.""Type""='{type}' and a.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaList(string applicationId, string type)
        {
         string query = @$"SELECT a.*            
                            FROM cms.""N_REC_APPLICATION_JOB_CRITERIA"" as a
                      where a.""ApplicationId""='{applicationId}' and a.""Type""='{type}' and a.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query, null);
            return queryData;
        }
        public async Task<ApplicationViewModel> GetAppDetailsById(string appId)
        {
            var query = $@" select *, ""NtsNoteId"" as ""NoteId"" 
                            from cms.""N_REC_APPLICATION"" 
                            where ""Id"" = '{appId}' and ""IsDeleted"" = false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<ApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<IdNameViewModel> GetNationalitybyId(string id)
        {
            var query = $@" select n.*,n.""NationalityName"" as ""Name"" from cms.""N_CoreHR_HRNationality"" as n where n.""Id"" = '{id}' and ""IsDeleted"" = false ";
            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<IdNameViewModel> GetTitlebyId(string id)
        {
            var query = $@" select * from public.""LOV"" where ""LOVType"" = 'PERSON_TITLE' AND ""Id"" = '{id}' and ""IsDeleted"" = false ";
            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<RecApplicationViewModel> GetApplicationDeclarationData(string applicationId)
        {
           
            string query = @$"SELECT app.""Id"",app.""GaecNo"",app.""WitnessName1"",app.""WitnessDesignation1"",
app.""WitnessDate1"",app.""WitnessGAEC1"",app.""WitnessName2"",app.""WitnessDesignation2"",app.""WitnessDate2"",app.""WitnessGAEC2"",
                            CONCAT( app.""FirstName"",' ',app.""MiddleName"" ,' ',app.""LastName"") as FullName,
                                    d.""Name"" as DivisionName, t.""Name"" as TitleName                                   
                                    FROM cms.""N_REC_APPLICATION"" as app                                    
                                    LEFT JOIN public.""LOV"" as d ON d.""Id"" = app.""DivisionId""
                                    LEFT JOIN Public.""LOV"" as t ON t.""Id"" = app.""TitleId""
                                    WHERE app.""Id"" = '{applicationId}' and app.""IsDeleted""=false";



            var queryData = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);

            return queryData;
        }
        public async Task<RecApplicationViewModel> GetConfidentialAgreementDetails(string applicationId)
        {
            string query = @$"select concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName, a.""PassportNumber"",  t.""Name"" as TitleName from cms.""N_REC_APPLICATION"" as a
                           left join public.""LOV"" as t on t.""Id"" = a.""TitleId""
                           where a.""Id""='{applicationId}' and a.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return list;
        }
        public async Task<RecApplicationViewModel> GetCompetenceMatrixDetails(string applicationId)
        {
            string query = @$"select a.*, p.""JobTitle"" as PositionName, t.""Name"" as TitleName
                            from cms.""N_REC_APPLICATION"" as a 
                            left join cms.""N_CoreHR_HRJob"" as p on p.""Id"" = a.""JobId""
                            left join public.""LOV"" as t on t.""Id"" = a.""TitleId""
                            where a.""Id""='{applicationId}' and a.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return list;
        }
        public async Task<IdNameViewModel> GetUserSign()
        {
            string query = @$"select u.""SignatureId"" as Id
        from public.""UserRoleUser"" as uru
        join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
        join public.""User"" as u on u.""Id""=uru.""UserId""
            where ur.""Code""='ED' ";

            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<IdNameViewModel> GetJobManpowerType(string Id)
        {
           

            string query = @$"select mt.""Code"" as Code
            from cms.""N_CoreHR_HRJob"" as j 
            left join public.""LOV"" as mt on  mt.""LOVType""='LOV_MANPOWERTYPE' and mt.""Id"" = j.""ManpowerTypeId""
                                where j.""Id"" = '{Id}'";


            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<RecApplicationViewModel> GetStaffJoiningDetails(string applicationId)
        {
            string query = @$"select concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName, a.""GaecNo"", a.""JobNo"",
            a.""OfferDesigination"", a.""JoiningDate"" as CandJoiningDate, n.""NationalityName"" as NationalityName, t.""Name"" as ""TitleName"", od.""JobTitle"" as OfferDesigination
            from rec.""N_REC_APPLICATION"" as a
            left join cms.""N_CoreHR_HRJob"" as od on od.""Id"" = a.""JobId""
            LEFT JOIN cms.""N_CoreHR_HRNationality"" as n ON n.""Id"" = a.""NationalityId""
            LEFT JOIN public.""LOV"" as t ON t.""Id"" = a.""TitleId""             
            WHERE a.""Id"" = '{applicationId}' and a.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<RecApplicationViewModel> GetCandidateAppDetails(string canId,string jobId)
        {
            
            string query = @$"select a.*, j.""JobTitle"" as JobName, o.""DepartmentName"" as OrganizationName
                            ,apst.""Name"" as ApplicationStatusName, apstate.""Name"" as ApplicationStateName, b.""BatchName"" as BatchName
                                from cms.""N_REC_APPLICATION"" as a
                                left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId""
                                left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id"" = a.""OrganisationId""
								left join public.""LOV"" as apst on apst.""Id"" = a.""ApplicationStatusId""
                                left join public.""LOV"" as apstate on apstate.""Id"" = a.""ApplicationStateId""
                                left join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = a.""BatchId""
                                WHERE a.""CandidateId"" = '{canId}' and a.""JobId""='{jobId}' and a.""IsDeleted""=false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<List<CandidateProfileViewModel>> GetStaffList(string id)
        {
            //var query = @"select distinct pl.*,concat_ws('_',j.""JobTitle"", app.""JobId"") as JobAdvertisement FROM rec.""CandidateProfile"" as pl
            //            left join rec.""Application"" as app on app.""CandidateProfileId"" = pl.""Id""
            //            left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1                        
            //            left join cms.""Job"" as j on j.""Id"" = app.""JobId""                      
            //            where /*j.""ManpowerTypeCode"" != 'Staff' and*/ pl.""SourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + id + "'";
            var query = @"select distinct pl.*,concat_ws('_',j.""Name"", app.""JobId"") as JobAdvertisement FROM cms.""N_REC_REC_CANDIDATE"" as pl
                        left join cms.""N_REC_APPLICATION"" as app on app.""CandidateId"" = pl.""Id""
                        left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1                        
                        left join cms.""Job"" as j on j.""Id"" = app.""JobId""                      
                        where /*j.""ManpowerTypeCode"" == 'Staff' and*/ pl.""sourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + id + "'";
            var queryData = await _queryRepo.ExecuteQueryList<CandidateProfileViewModel>(query, null);
            return queryData;
        }
        public async Task<IdNameViewModel> GetCountrybyId(string id)
        {
            var query = $@" select c.*,c.""CountryName"" as ""Name"" from cms.""N_CoreHR_HRcountry"" as c where c.""Id"" = '{id}' and c.""IsDeleted"" = false ";
            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<CandidateExperienceViewModel> GetCandidateExperiencebyId(string Id)
        {

            var query = $@"Select * from cms.""N_REC_CANDIDATE_EXPERIENCE"" where ""CandidateId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateExperienceViewModel>(query, null);
            return queryData;
        }

        public async Task<bool> DeleteBeneficiary(string NoteId)
        {
            var query = $@"update  cms.""N_REC_WF_NOMINATION_BENEFITS"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<ApplicationBeneficiaryViewModel> GetBeneficiaryDataByid(string Id, string referId)
        {
            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_WF_NOMINATION_BENEFITS"" where ""NtsNoteId""='{Id}' and ""ReferenceId""='{referId}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<ApplicationBeneficiaryViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ApplicationBeneficiaryViewModel>> ReadBeneficiaryData(string referId)
        {
            var query = $@"Select nb.*, nb.""NtsNoteId"" as ""NoteId""
                          from cms.""N_REC_WF_NOMINATION_BENEFITS""  as nb
                          where nb.""ReferenceId""='{referId}' and nb.""IsDeleted""=false  ";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationBeneficiaryViewModel>(query, null);
            return queryData;
        }
        #endregion s

        #region p

        public async Task<CandidateExperienceViewModel> GetCandidateExperience(string Id)
        {

            var query = $@"Select *, ""NtsNoteId"" as ""NoteId"" from cms.""N_REC_CANDIDATE_EXPERIENCE"" where ""NtsNoteId""='{Id}' and ""IsDeleted""=false  ";
            var queryData = await _queryRepo.ExecuteQuerySingle<CandidateExperienceViewModel>(query, null);
            return queryData;
        }
        public async Task<bool> DeleteCandidateExperience(string NoteId)
        {
            var query = $@"update  cms.""N_REC_CANDIDATE_EXPERIENCE"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<List<CandidateExperienceViewModel>> ReadCandidateExperience(string candidateProfileId)
        {
            var query = $@"Select oe.*, oe.""NtsNoteId"" as ""NoteId"",c.""JobTitle"" as ""JobTitle"",d.""LocationName"" as ""Location""
                          from cms.""N_REC_CANDIDATE_EXPERIENCE""  as oe
                          join cms.""N_CoreHR_HRJob"" as c on c.""Id""= oe.""JobTitle""
                          join cms.""N_CoreHR_HRLocation"" as d on d.""Id""= oe.""LocationId""
                          where oe.""CandidateId""='{candidateProfileId}' and oe.""IsDeleted""=false  ";

            var queryData = await _queryRepo.ExecuteQueryList<CandidateExperienceViewModel>(query, null);
            return queryData;
        }


        #endregion p

        #region j
        public async Task<IList<JobAdvertisementViewModel>> GetManpowerUniqueJobData()
        {
            string query = @$"select distinct jb.""Id"" as Id,jb.""Id"" as JobId,jb.""JobTitle"" as JobName,count(t) as Count,mt.""Name"" as ManpowerType
                            from cms.""N_CoreHR_HRJob"" as jb
                            --join cms.""Job"" as jb on jb.""Id"" = c.""JobId""
                            left join public.""LOV"" as mt on mt.""LOVType""='REC_MANPOWER' and mt.""Id"" = jb.""ManpowerTypeId""
                            left join cms.""N_REC_JobDescriptionTask"" as jdt on jb.""Id""=jdt.""JobId"" and jdt.""IsDeleted""=false AND jdt.""ReferenceTypeCode""='77'
                            Left join public.""NtsTask"" as t on jdt.""NtsNoteId""=t.""Id""
                            where jb.""Status"" = 1 and jb.""IsDeleted"" = false
                            group by jb.""Id"",mt.""Name"" ";
            var list1 = await _queryRepoJob.ExecuteQueryList(query, null);

            string query1 = @$"SELECT c.""Id"",c.""JobId"",  c.""CreatedDate"" as CreateDate,c.""NtsNoteId"" as JobAdvNoteId,s.""Id"" as ServiceId,ss.""Code"" as ServiceStatusCode,
                            (sum(case when aps.""Code""='ShortListByHr' then 1 else 0 end)) as ShortlistedByHr,
                            (sum(case when aps.""Code"" = 'ShortListByHm' then 1 else 0 end)) as ShortlistedForInterview,
                            (sum(case when aps.""Code"" = 'DirectHiring' then 1 else 0 end)) as DirectHiring,
                            (sum(case when aps.""Code"" = 'InterviewsCompleted' then 1 else 0 end)) as InterviewCompleted,
                            (sum(case when aps.""Code"" = 'IntentToOffer' then 1 else 0 end)) as IntentToOfferSent,
                            (sum(case when aps.""Code"" = 'FinalOffer' then 1 else 0 end)) as FinalOfferSent,
                            (sum(case when aps.""Code"" = 'VisaTransfer' then 1 else 0 end)) as VisaTransfer,
                            (sum(case when aps.""Code"" = 'BusinessVisa' then 1 else 0 end)) as BusinessVisa,
                            (sum(case when aps.""Code"" = 'WorkVisa' then 1 else 0 end)) as WorkVisa,
                            (sum(case when aps.""Code"" = 'StaffJoined' then 1 else 0 end)) as CandidateJoined  , 
                            (sum(case when aps.""Code"" = 'WorkerJoined' then 1 else 0 end)) as WorkerJoined ,
                            (sum(case when aps.""Code"" = 'WorkerPool' then 1 else 0 end)) as WorkerPool ,
                            (sum(case when aps.""Code"" = 'WorkPermit' then 1 else 0 end)) as WorkPermit ,
                            (sum(case when aps.""Code"" = 'PostWorkerJoined' then 1 else 0 end)) as PostWorkerJoined ,
                            (sum(case when aps.""Code"" = 'PostStaffJoined' then 1 else 0 end)) as PostStaffJoined ,
                            --(sum(case when aps.""Code"" = 'Joined' then 1 else 0 end)) as Joined 
                            (sum(case when (aps.""Code"" = 'PostWorkerJoined' or aps.""Code"" = 'PostStaffJoined' or aps.""Code"" = 'Joined') then 1 else 0 end)) as Joined 
                            FROM cms.""N_REC_JobAdvertisement"" as c
                            left join public.""NtsService"" as s on c.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                            left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
                            --LEFT join cmc.""N_REC_APPLICATION"" as ap on ap.""JobAdvertisementId"" =c.""Id"" and ap.""IsDeleted""=false
                            LEFT join cms.""N_REC_APPLICATION"" as ap on ap.""JobId"" =c.""JobId"" and ap.""IsDeleted""=false
                            LEFT join public.""LOV"" as aps on aps.""Id"" = ap.""ApplicationStateId""
                            where c.""Status""=1
                            group by c.""Id"",s.""Id"",ss.""Code""
                            ";

            //string query1 = @$"SELECT c.""Id"",                           

            //                (sum(case when aps.""Code"" = 'Joined'  then 1 else 0 end) +(case when  c.""CandidateJoined"" is null then 0 else c.""CandidateJoined"" end)) as CJ                           
            //                FROM rec.""JobAdvertisement"" as c                           
            //                LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" =c.""Id""
            //                LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
            //                group by c.""Id""
            //                ";
            var list2 = await _queryRepoJob.ExecuteQueryList(query1, null);

            string query3 = @$"SELECT ja.""JobId"",                        
                            sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
                            sum(case when ja.""Status"" = 2 then 1 else 0 end) as InActive                          
                            FROM cms.""N_REC_JobAdvertisement"" as ja                            
                            where ja.""IsDeleted""=false group by ja.""JobId"" ";

            // var list3 = await _queryRepo.ExecuteQueryList(query1, null);
            var list3 = await _queryRepoJob.ExecuteQueryList(query3, null);


            var list4 = from a in list1
                        join b in list2
                        on a.Id equals b.JobId into gj
                        from sub in gj.DefaultIfEmpty()
                        select new JobAdvertisementViewModel
                        {
                            // Id = sub.Id,
                            Id = sub?.Id ?? null,
                            JobId = a.JobId,
                            JobName = a.JobName,
                            //VersionNo = a.VersionNo,                          
                            CreateDate = sub?.CreateDate ?? null,
                            //Active=b.Active,
                            //InActive=b.InActive,
                            ShortlistedByHr = sub?.ShortlistedByHr ?? 0,
                            ShortlistedForInterview = sub?.ShortlistedForInterview ?? 0,
                            DirectHiring = sub?.DirectHiring ?? 0,
                            IntentToOfferSent = sub?.IntentToOfferSent ?? 0,
                            FinalOfferSent = sub?.FinalOfferSent ?? 0,
                            WorkerPool = sub?.WorkerPool ?? 0,
                            BusinessVisa = sub?.BusinessVisa ?? 0,
                            WorkVisa = sub?.WorkVisa ?? 0,
                            VisaTransfer = sub?.VisaTransfer ?? 0,
                            WorkPermit = sub?.WorkPermit ?? 0,
                            CandidateJoined = sub?.CandidateJoined ?? 0,
                            WorkerJoined = sub?.WorkerJoined ?? 0,
                            Joined = sub?.Joined ?? 0,
                            PostStaffJoined = sub?.PostStaffJoined ?? 0,
                            PostWorkerJoined = sub?.PostWorkerJoined ?? 0,
                            Count = a.Count,
                            ManpowerType = a.ManpowerType,
                            JobAdvNoteId = sub?.JobAdvNoteId ?? null,
                            ServiceId = sub?.ServiceId ?? null,
                            ServiceStatusCode = sub?.ServiceStatusCode ?? null
                        };

            var list = from a in list4
                       join b in list3
                       on a.JobId equals b.JobId into gj
                       from sub in gj.DefaultIfEmpty()
                       select new JobAdvertisementViewModel
                       {
                           Id = a.Id,
                           JobId = a.JobId,
                           JobName = a.JobName,

                           //VersionNo = a.VersionNo,

                           CreateDate = a.CreateDate,
                           Active = sub?.Active ?? 0,
                           InActive = sub?.InActive ?? 0,
                           //ShortlistedByHr = sub?.ShortlistedByHr ?? 0,
                           //ShortlistedForInterview = sub?.ShortlistedForInterview ?? 0,
                           //IntentToOfferSent = sub?.IntentToOfferSent ?? 0,
                           //FinalOfferSent = sub?.FinalOfferSent ?? 0,
                           //WorkerPool = sub?.WorkerPool ?? 0,
                           //BusinessVisa = sub?.BusinessVisa ?? 0,
                           //WorkVisa = sub?.WorkVisa ?? 0,
                           //VisaTransfer = sub?.VisaTransfer ?? 0,
                           //WorkPermit = sub?.WorkPermit ?? 0,
                           //CandidateJoined = sub?.CandidateJoined ?? 0,
                           //WorkerJoined = sub?.WorkerJoined ?? 0,
                           //Joined = sub?.Joined ?? 0,
                           ShortlistedByHr = a.ShortlistedByHr,
                           ShortlistedForInterview = a.ShortlistedForInterview,
                           DirectHiring = a.DirectHiring,
                           IntentToOfferSent = a.IntentToOfferSent,
                           FinalOfferSent = a.FinalOfferSent,
                           WorkerPool = a.WorkerPool,
                           BusinessVisa = a.BusinessVisa,
                           WorkVisa = a.WorkVisa,
                           VisaTransfer = a.VisaTransfer,
                           WorkPermit = a.WorkPermit,
                           CandidateJoined = a.CandidateJoined,
                           WorkerJoined = a.WorkerJoined,
                           Joined = a.Joined,
                           PostStaffJoined = a.PostStaffJoined,
                           PostWorkerJoined = a.PostWorkerJoined,
                           Count = a.Count,
                           ManpowerType = a.ManpowerType,
                           JobAdvNoteId = a.JobAdvNoteId,
                           ServiceId = a.ServiceId,
                           ServiceStatusCode = a.ServiceStatusCode
                       };

            return list.ToList();
        }

        public async Task<IList<JobDescriptionCriteriaViewModel>> GetJobDescCriteriaList(string type, string jobdescid)
        {
            string query = @$"Select jc.*,case when lov.""Name"" is null then '' else lov.""Name"" end  as CriteriaTypeName,
case when lovother.""Name"" is null then '' else lovother.""Name"" end  as LovTypeName
from cms.""N_REC_JobDescriptionCriteria"" as jc
left join public.""LOV"" as lov on lov.""Id"" = jc.""CriteriaTypeId""
left join public.""LOV"" as lovother on lovother.""Id"" = jc.""ListOfValueTypeId""
where jc.""Type"" = '{type}' and jc.""JobDescriptionId"" = '{jobdescid}' and jc.""IsDeleted""=false ";

            var queryData = await _queryRepoCri.ExecuteQueryList(query, null);
            return queryData;
        }

        public async Task<JobDescriptionViewModel> GetJobDescriptionData(string jobId)
        {
            string query = $@"Select * from cms.""N_REC_RecJobDescription"" where ""JobId""='{jobId}' ";

            var data = await _queryRepoCri.ExecuteQuerySingle<JobDescriptionViewModel>(query, null);
            return data;
        }
        public async Task<JobAdvertisementViewModel> GetJobAdvertisementData(string id)
        {
            string query = @$"SELECT ja.*, ja.""NtsNoteId"" as ApplicationNoteId FROM cms.""N_REC_JobAdvertisement"" as ja    
where ja.""Id"" = '{id}' and ja.""IsDeleted""=false ";

            var queryData = await _queryJobAdv.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<IList<JobCriteriaViewModel>> GetJobCriteriaList(string type, string jobadvtid)
        {
            string query = @$"Select jc.*,case when lov.""Name"" is null then '' else lov.""Name"" end  as CriteriaTypeName,
case when lovother.""Name"" is null then '' else lovother.""Name"" end  as LovTypeName
from cms.""N_REC_JOB_ADV_CRITERIA"" as jc
join public.""NtsNote"" as n on jc.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
left join public.""LOV"" as lov on lov.""Id"" = jc.""CriteriaTypeId""
left join public.""LOV"" as lovother on lovother.""Id"" = jc.""ListOfValueTypeId""
where jc.""Type"" = '{type}' and jc.""JobAdvertisementId"" = '{jobadvtid}' and jc.""IsDeleted""=false ";

            var queryData = await _queryRepoCri.ExecuteQueryList<JobCriteriaViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentSummaryData()
        {
            
            var filter = "";

            string query = @$"SELECT c.*,jd.""Id"" as JobDescriptionId,mt.""Name"" as ""ManpowerType"",s.""Id"" as ServiceId,
                            sum(case when ur.""Code""='ORG_UNIT' and c.""Id"" is not null then 1 else 0 end) as OrgUnit,
                            sum(case when ur.""Code"" = 'PLANNING' and c.""Id"" is not null then 1 else 0 end) as PlanningUnit,
                            sum(case when ur.""Code"" = 'HR' and c.""Id"" is not null then 1 else 0 end) as Hr,
                            u.""Name"" as CreatedByName,j.""JobTitle"",o.""DepartmentName"" as OrganizationName
                            FROM cms.""N_REC_ManpowerRequirement"" as c
                            join public.""NtsService"" as s on c.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                            left join public.""NtsServiceComment"" as sc on s.""Id""=sc.""NtsServiceId"" and sc.""IsDeleted""=false
                            --LEFT JOIN rec.""ManpowerSummaryComment"" as q ON q.""ManpowerRecruitmentSummaryId"" = c.""Id""
                            left join cms.""N_REC_RecJobDescription"" as jd on jd.""JobId""=c.""JobId""
                            
                            LEFT JOIN public.""UserRole"" as ur ON ur.""Id"" = sc.""CommentedByUserId""
                            LEFT JOIN public.""User"" as u ON u.""Id"" = s.""OwnerUserId""
                            LEFT JOIN cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = c.""JobId""
                            left join public.""LOV"" as mt on mt.""LOVType""='REC_MANPOWER' and mt.""Id"" = j.""ManpowerTypeId""
                            LEFT JOIN cms.""N_CoreHR_HRDepartment"" as o ON o.""Id"" = c.""OrganizationId"" 
                            --Left join public.""RecTask"" as t on t.""ReferenceTypeId""=c.""Id"" AND t.""ReferenceTypeCode""='76'
                            where c.""IsDeleted"" = false #FILTER#
                            group by c.""Id"",u.""Name"",j.""JobTitle"",o.""DepartmentName"",jd.""Id"",mt.""Name"",s.""Id""
                            ";

            //if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
            //{
            //    var orglist = await _hmBusiness.GetHODOrg(_repo.UserContext.UserId);
            //    var orgs = orglist.Select(x => x.Id);
            //    orgId = string.Join(",", orgs).TrimEnd(',');
            //    orgId = orgId.Replace(",", "','");
            //    filter = @$" and c.""OrganizationId"" in ('{orgId}') ";
            //}

            if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
            {
                var orgquery = $@"SELECT c.""OrganisationId"",j.""JobTitle"" as Name
                                from cms.""N_REC_HEAD_OF_DEPARTMENT"" as c
                                join cms.""N_CoreHR_HRJob"" as j on c.""DesignationId""=j.""Id"" and j.""IsDeleted""=false
                                WHERE c.""IsDeleted""=false and c.""UserId""='{_repo.UserContext.UserId}' ";

                var orgdata = await _queryIdName.ExecuteQuerySingle(orgquery, null);                
                
                string orgId = orgdata.Id.Replace(",", "','");
                filter = @$" and c.""OrganizationId"" in ('{orgId}') ";
            }

            query = query.Replace("#FILTER#", filter);
            var list1 = await _queryMPRSummary.ExecuteQueryList(query, null);

            //string query4 = $@"SELECT c.""Id"",count(t) as Count
            //                FROM rec.""ManpowerRecruitmentSummary"" as c
            //                Left join public.""RecTask"" as t on t.""ReferenceTypeId""=c.""Id"" AND t.""ReferenceTypeCode""='76'
            //                where c.""IsDeleted"" = false 
            //                group by c.""Id"" ";

            //var list5 = await _queryRepo.ExecuteQueryList(query4, null);
            
            string query1 = @$"SELECT c.""Id"",c.""Id"" as JobId ,b.""OrganizationId"",                         
                          
                            (sum(case when aps.""Code""='ShortListByHr'  then 1 else 0 end) ) as ShortlistedByHr,
                            (sum(case when aps.""Code"" = 'ShortListByHm'  then 1 else 0 end)) as ShortlistedForInterview,
                            (sum(case when aps.""Code"" = 'DirectHiring'  then 1 else 0 end)) as DirectHiring,
                            (sum(case when aps.""Code"" = 'InterviewsCompleted' then 1 else 0 end) ) as InterviewCompleted,
                            --(sum(case when aps.""Code"" = 'FinalOfferAccepted' then 1 else 0 end) ) as FinalOfferAccepted,
                            --(sum(case when aps.""Code"" = 'Joined'  then 1 else 0 end)) as CandidateJoined
                            (sum(case when aps.""Code"" = 'IntentToOffer' then 1 else 0 end)) as IntentToOffer,
                            (sum(case when aps.""Code"" = 'FinalOffer' then 1 else 0 end)) as FinalOffer,
                            (sum(case when aps.""Code"" = 'FinalOfferAccepted' then 1 else 0 end)) as FinalOfferAccepted,
                            (sum(case when aps.""Code"" = 'VisaTransfer' then 1 else 0 end)) as VisaTransfer,
                            (sum(case when aps.""Code"" = 'BusinessVisa' then 1 else 0 end)) as BusinessVisa,
                            (sum(case when aps.""Code"" = 'VisaAppointmentTaken' then 1 else 0 end)) as VisaAppointmentTaken,
                            (sum(case when aps.""Code"" = 'BiometricCompleted' then 1 else 0 end)) as BiometricCompleted,
                            (sum(case when aps.""Code"" = 'VisaApproved' then 1 else 0 end)) as VisaApproved,
                            (sum(case when aps.""Code"" = 'WorkVisa' then 1 else 0 end)) as WorkerVisa,
                            (sum(case when aps.""Code"" = 'FlightTicketsBooked' then 1 else 0 end)) as FightTicketsBooked,
                            (sum(case when aps.""Code"" = 'CandidateArrived' then 1 else 0 end)) as CandidateArrived,
                            (sum(case when aps.""Code"" = 'StaffJoined' then 1 else 0 end)) as CandidateJoined  , 
                            (sum(case when aps.""Code"" = 'WorkerJoined' then 1 else 0 end)) as WorkerJoined ,
                            (sum(case when aps.""Code"" = 'WorkerPool' then 1 else 0 end)) as WorkerPool ,
                            (sum(case when aps.""Code"" = 'WorkPermit' then 1 else 0 end)) as WorkPermit ,
                            (sum(case when aps.""Code"" = 'PostWorkerJoined' then 1 else 0 end)) as PostWorkerJoined ,
                            (sum(case when aps.""Code"" = 'PostStaffJoined' then 1 else 0 end)) as PostStaffJoined ,
                            --(sum(case when aps.""Code"" = 'Joined' then 1 else 0 end)) as Joined 
                            (sum(case when (aps.""Code"" = 'PostWorkerJoined' or aps.""Code"" = 'PostStaffJoined' or aps.""Code"" = 'Joined') then 1 else 0 end)) as Joined 

                            from cms.""N_CoreHR_HRJob"" as c 
                           
                             join cms.""N_REC_APPLICATION"" as ap on ap.""JobId"" = c.""Id"" and ap.""IsDeleted""=false                        
                             left join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = ap.""BatchId"" and b.""IsDeleted""=false                         
                             left join public.""LOV"" as aps on aps.""Id"" = ap.""ApplicationStateId"" and aps.""LOVType""='APPLICATION_STATE' and aps.""IsDeleted""=false
                             LEFT join public.""LOV"" as apst on apst.""Id"" = ap.""CurrentStatusId"" and apst.""LOVType""='APPLICATION_STATUS' and apst.""IsDeleted""=false
                            where apst.""Code""!='REJECTED' and apst.""Code""!='RejectedHM' and apst.""Code""!='WAITLISTED'
                            group by c.""Id"",b.""OrganizationId""
                            ";
            var list2 = await _queryMPRSummary.ExecuteQueryList(query1, null);

            string query3 = @$"SELECT c.""Id"",ja.""Id"" as JobAdvertisementId,                           
                            sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
                            sum(case when ja.""Status""=2 then 1 else 0 end) as InActive                          
                            FROM cms.""N_REC_ManpowerRequirement"" as c 
                            join cms.""N_REC_JobAdvertisement"" as ja on ja.""JobId""=c.""JobId"" where ja.""IsDeleted"" = false                           
                            group by c.""Id"",ja.""Id""
                            ";

            var list3 = await _queryMPRSummary.ExecuteQueryList(query3, null);

            //var list6 = from a in list1
            //            join b in list5
            //            on a.Id equals b.Id
            //            select new ManpowerRecruitmentSummaryViewModel
            //            {
            //                Id = a.Id,
            //                JobId = a.JobId,
            //                JobTitle = a.JobTitle,
            //                OrganizationId = a.OrganizationId,
            //                OrganizationName = a.OrganizationName,
            //                Requirement = a.Requirement,
            //                Seperation = a.Seperation,
            //                Available = a.Available,
            //                Planning = a.Planning,
            //                Transfer = a.Transfer,
            //                Balance = a.Balance,
            //                VersionNo = a.VersionNo,
            //                CreatedByName = a.CreatedByName,
            //                CreatedDate = a.CreatedDate,
            //                OrgUnit = a.OrgUnit,
            //                PlanningUnit = a.PlanningUnit,
            //                Hr = a.Hr,
            //                Count = b.Count,
            //                JobDescriptionId = a.JobDescriptionId,
            //                ManpowerType = a.ManpowerType,
            //            };

            var list4 = from a in list1
                        join b in list2
                        on new { X1 = a.JobId, X2 = a.OrganizationId } equals new { X1 = b.JobId, X2 = b.OrganizationId } into gj
                        //on a.Id equals b.Id into gj
                        from sub in gj.DefaultIfEmpty()
                        select new ManpowerRecruitmentSummaryViewModel
                        {
                            Id = a.Id,
                            JobId = a.JobId,
                            JobTitle = a.JobTitle,
                            JobDescriptionId = a.JobDescriptionId,
                            OrganizationId = a.OrganizationId,
                            OrganizationName = a.OrganizationName,
                            Requirement = a.Requirement,
                            Seperation = a.Seperation,
                            Available = a.Available,
                            SubContract = a.SubContract,
                            Transfer = a.Transfer,
                            Balance = a.Balance,
                            VersionNo = a.VersionNo,
                            CreatedByName = a.CreatedByName,
                            CreatedDate = a.CreatedDate,
                            OrgUnit = a.OrgUnit,
                            PlanningUnit = a.PlanningUnit,
                            Hr = a.Hr,
                            // Active = b.Active,
                            //InActive = b.InActive,
                            ShortlistedByHr = sub?.ShortlistedByHr ?? 0,
                            ShortlistedForInterview = sub?.ShortlistedForInterview ?? 0,
                            DirectHiring = sub?.DirectHiring ?? 0,
                            IntentToOffer = sub?.IntentToOffer ?? 0,
                            VisaTransfer = sub?.VisaTransfer ?? 0,
                            BusinessVisa = sub?.BusinessVisa ?? 0,
                            WorkerJoined = sub?.WorkerJoined ?? 0,
                            WorkPermit = sub?.WorkPermit ?? 0,
                            WorkerVisa = sub?.WorkerVisa ?? 0,
                            FinalOffer = sub?.FinalOffer ?? 0,
                            InterviewCompleted = sub?.InterviewCompleted ?? 0,
                            FinalOfferAccepted = sub?.FinalOfferAccepted ?? 0,
                            CandidateJoined = sub?.CandidateJoined ?? 0,
                            WorkerPool = sub?.WorkerPool ?? 0,
                            Joined = sub?.Joined ?? 0,
                            PostStaffJoined = sub?.PostStaffJoined ?? 0,
                            PostWorkerJoined = sub?.PostWorkerJoined ?? 0,                            
                            JobAdvertisementId = sub?.Id ?? null,
                            ManpowerType = a.ManpowerType,
                            ServiceId = a.ServiceId
                        };

            var list = from a in list4
                       join b in list3
                       on a.Id equals b.Id into gj
                       from sub in gj.DefaultIfEmpty()
                       select new ManpowerRecruitmentSummaryViewModel
                       {
                           Id = a.Id,
                           JobId = a.JobId,
                           JobTitle = a.JobTitle,
                           JobDescriptionId = a.JobDescriptionId,
                           OrganizationId = a.OrganizationId,
                           OrganizationName = a.OrganizationName,
                           Requirement = a.Requirement,
                           Seperation = a.Seperation,
                           Available = a.Available,
                           SubContract = a.SubContract,
                           Transfer = a.Transfer,
                           Balance = a.Balance,
                           VersionNo = a.VersionNo,
                           CreatedByName = a.CreatedByName,
                           CreatedDate = a.CreatedDate,
                           OrgUnit = a.OrgUnit,
                           PlanningUnit = a.PlanningUnit,
                           Hr = a.Hr,
                           Active = sub?.Active ?? 0,
                           InActive = sub?.InActive ?? 0,
                           ShortlistedByHr = a.ShortlistedByHr,
                           ShortlistedForInterview = a.ShortlistedForInterview,
                           DirectHiring = a.DirectHiring,
                           IntentToOffer = a.IntentToOffer,
                           VisaTransfer = a.VisaTransfer,
                           BusinessVisa = a.BusinessVisa,
                           WorkerJoined = a.WorkerJoined,
                           WorkPermit = a.WorkPermit,
                           WorkerVisa = a.WorkerVisa,
                           FinalOffer = a.FinalOffer,
                           InterviewCompleted = a.InterviewCompleted,
                           FinalOfferAccepted = a.FinalOfferAccepted,
                           CandidateJoined = a.CandidateJoined,
                           WorkerPool = a.WorkerPool,
                           Joined = a.Joined,
                           PostStaffJoined = a.PostStaffJoined,
                           PostWorkerJoined = a.PostWorkerJoined,
                           Count = a.Count,
                           JobAdvertisementId = sub?.JobAdvertisementId ?? null,
                           ManpowerType = a.ManpowerType,
                           ServiceId = a.ServiceId
                       };

            return list.ToList();
            //IList<ManpowerRecruitmentSummaryViewModel> newlist = new List<ManpowerRecruitmentSummaryViewModel>();
            //return newlist;
        }

        public async Task<JobAdvertisementViewModel> GetState(string serId)
        {
            string query = @$"select v.""Name"" as ActionName from cms.""N_REC_JobAdvertisement"" as ja
                                left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = ja.""JobId""
                                left join public.""NtsService"" as jas on ja.""Id""=jas.""UdfNoteTableId"" and jas.""IsDeleted""=false
                                left join rec.""LOV"" as v on v.""Id"" = jas.""ServiceStatusId"" and v.""IsDeleted""=false
                                where ja.""Status"" = 1 and ja.""JobId"" = (select mpr.""JobId"" from public.""NtsService"" as s 
                                    join cms.""N_REC_ManpowerRequirement"" as mpr on s.""UdfNoteTableId""=mpr.""Id"" and mpr.""IsDeleted""=false where s.""Id"" = '{serId}')";
            var queryData = await _queryRepoJob.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<IList<ApplicationViewModel>> GetViewApplicationDetails(string jobId, string orgId)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
--app.""QatarId"" as QatarId,
lov.""Name"" as Gender, mar.""Name"" as MaritalStatusName,
app.""PassportNumber"" as PassportNumber, pic.""CountryName"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,job.""JobTitle"" as JobName,--app.""GAECNo"" as GaecNo,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score, --app.""IsLocalCandidate"" as IsLocalCandidate,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,
c.""UserId"" as ApplicationUserId,
apps.""Name"" as ApplicationState, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo
FROM cms.""N_REC_APPLICATION"" as app

left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""CurrentStatusId""

left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId"" 

left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
--left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategory""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left JOIN public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 

left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId"" 
join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
where app.""IsDeleted""=false
#WHERE# ";

                var where = $@" ";
                if (jobId.IsNotNullAndNotEmpty() && orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" and app.""JobId""='{jobId}' AND batch.""OrganizationId""='{orgId}' ";
                }
                else if (orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" and batch.""OrganizationId""='{orgId}' ";
                }
                else if (jobId.IsNotNullAndNotEmpty())
                {
                    where = @$" and app.""JobId""='{jobId}' ";
                }
                query = query.Replace("#WHERE#", where);
                var allList = await _queryApp.ExecuteQueryList(query, null);
                return allList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<IList<IdNameViewModel>> GetGradeIdNameList(string code=null)
        {
            string query = @$"SELECT ""Id"", ""GradeName"" as Name
                                FROM cms.""N_CoreHR_HRGrade""
                                where ""IsDeleted"" = false and ""Status"" = 1 #CodeWhere# order by ""GradeName"" ";

            string where = code.IsNotNullAndNotEmpty() ? $@" and ""Code""='{code}' " : "";
            query = query.Replace("#CodeWhere#", where);

            var queryData = await _queryIdName.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<RecCandidateElementInfoViewModel>> GetElementData(string appid)
        { 
            string query = @$"SELECT rc.*, rp.""ElementName"",rp.""Id"" as PayId,rc.""NtsNoteId"" as NoteId
                            FROM cms.""N_REC_PAY_ELEMENT"" as rp
                            left join cms.""N_REC_PAY_ELEMENT_CANDIDATE"" as rc on rp.""Id"" = rc.""ElementId""
                            and rc.""IsDeleted"" = false and rc.""ReferenceId"" = '{appid}'
                            where rp.""IsDeleted"" = false  order by rp.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList<RecCandidateElementInfoViewModel>(query, null);
            var list = queryData.ToList();

            return list;
        }

        public async Task<IList<RecCandidatePayElementViewModel>> GetPayElementData(string appid)
        {
            string query = @$"SELECT rp.""ElementName"",rc.""Value""
                            FROM cms.""N_REC_PAY_ELEMENT_CANDIDATE"" as rc
                            join cms.""N_REC_PAY_ELEMENT"" as rp on rp.""Id"" = rc.""ElementId""
                            and rp.""IsDeleted"" = false 
                            where rp.""IsDeleted"" = false and rc.""ReferenceId"" = '{appid}'  order by rp.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList<RecCandidatePayElementViewModel>(query, null);
            var list = queryData.ToList();

            return list;
        }

        public async Task<IList<RecApplicationViewModel>> GetTotalApplication(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
--app.""QatarId"" as QatarId,
lov.""Name"" as Gender, mar.""Name"" as MaritalStatusName,
app.""PassportNumber"" as PassportNumber, pic.""CountryName"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
TRUNC(app.""TotalWorkExperience""::decimal,0) as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, job.""JobTitle"" as PositionName,batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score, --app.""IsLocalCandidate"" as IsLocalCandidate,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,

--task.""Id"" as TaskId,task.""TemplateCode"" as TaskTemplateCode,task.""TaskStatusCode"" as TaskStatusCode,
--taskn.""Id"" as NextTaskId,taskn.""TemplateCode"" as NextTaskTemplateCode,taskn.""TaskStatusCode"" as NextTaskStatusCode,
--taskv.""Id"" as VisaTaskId,taskv.""TemplateCode"" as VisaTaskTemplateCode,taskv.""TaskStatusCode"" as VisaTaskStatusCode,
c.""UserId"" as ApplicationUserId,
apps.""Name"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo
FROM cms.""N_REC_APPLICATION"" as app

left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""CurrentStatusId""

--left join public.""RecTask"" as task on task.""ReferenceTypeId""=app.""Id"" AND task.""ReferenceTypeCode""='32' AND task.""TemplateCode""='{tempcode}'
--left join public.""RecTask"" as taskn on taskn.""ReferenceTypeId""=app.""Id"" AND taskn.""ReferenceTypeCode""='32' AND taskn.""TemplateCode""='{nexttempcode}'
--left join public.""RecTask"" as taskv on taskv.""ReferenceTypeId""=app.""Id"" AND taskv.""ReferenceTypeCode""='32' AND taskv.""TemplateCode""='{visatempcode}'
left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId"" 

left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategoryId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left JOIN public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 

left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId"" 
join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
where app.""IsDeleted""=false
#WHERE#
 ";
                var where = "";
                if (jobid.IsNotNullAndNotEmpty())
                {
                    where = $@" AND app.""JobId""='{jobid}' ";
                }
                if (orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" AND app.""JobId""='{jobid}' AND batch.""OrganizationId""='{orgId}' ";
                }
                query = query.Replace("#WHERE#", where);
                
                var allList = await _queryRepo.ExecuteQueryList<RecApplicationViewModel>(query, null);

                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<RecDashboardViewModel> GetManpowerRequirement(string jobId, string organizationId)
        {
            string query = @$"SELECT c.""JobId"" as JobId,
                            sum(c.""Requirement""::INTEGER) as Requirement,
                            sum(c.""Separation""::INTEGER) as Seperation,
                            sum(c.""Available""::INTEGER) as Available,
                            sum(c.""SubContract""::INTEGER) as Planning,
                            sum(c.""Transfer""::INTEGER) as Transfer,
                            sum(c.""Balance""::INTEGER) as Balance
                            FROM cms.""N_REC_ManpowerRequirement"" as c
                            JOIN cms.""N_CoreHR_HRJob"" as ja ON ja.""Id""=c.""JobId""
                            where c.""IsDeleted""=false #WHERE# group by c.""JobId""                           
                            ";
            var where = jobId.IsNotNullAndNotEmpty()? @$" and c.""JobId""='{jobId}' " : "";

            var orgwhere = organizationId.IsNotNullAndNotEmpty()? @$" and c.""OrganizationId""='{organizationId}' ":"";

            query = query.Replace("#WHERE#", String.Concat(where,orgwhere));

            var data = await _queryRepo.ExecuteQuerySingle<RecDashboardViewModel>(query, null);
            return data;
        }

        public async Task<IList<RecApplicationViewModel>> GetJobAdvertismentState(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode, string status)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,--app.""GaecNo"" as GaecNo,
app.""JoiningDate"" as JoiningDate,
app.""LastName"" as LastName, app.""Age"" as Age,
--app.""QatarId"" as QatarId,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
lov.""Name"" as Gender, mar.""Name"" as MaritalStatusName,
app.""PassportNumber"" as PassportNumber, pic.""CountryName"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
TRUNC(app.""TotalWorkExperience""::decimal,0) as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score, --app.""IsLocalCandidate"" as IsLocalCandidate,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,

--task.""Id"" as TaskId,task.""TemplateCode"" as TaskTemplateCode,task.""TaskStatusCode"" as TaskStatusCode,
--taskn.""Id"" as NextTaskId,taskn.""TemplateCode"" as NextTaskTemplateCode,taskn.""TaskStatusCode"" as NextTaskStatusCode,
--taskv.""Id"" as VisaTaskId,taskv.""TemplateCode"" as VisaTaskTemplateCode,taskv.""TaskStatusCode"" as VisaTaskStatusCode,
c.""UserId"" as ApplicationUserId,
apps.""Name"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo,batch.""BatchName"" as BatchName
,uhm.""Name"" as HiringManagerName
FROM cms.""N_REC_APPLICATION"" as app
#APPLICATIONSTATE#
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""

left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""

--left join public.""RecTask"" as task on task.""ReferenceTypeId""=app.""Id"" AND task.""ReferenceTypeCode""='32' AND task.""TemplateCode""='{tempcode}'
--left join public.""RecTask"" as taskn on taskn.""ReferenceTypeId""=app.""Id"" AND taskn.""ReferenceTypeCode""='32' AND taskn.""TemplateCode""='{nexttempcode}'
--left join public.""RecTask"" as taskv on taskv.""ReferenceTypeId""=app.""Id"" AND taskv.""ReferenceTypeCode""='32' AND taskv.""TemplateCode""='{visatempcode}'
left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId"" 
left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategoryId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
INNER JOIN public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" OR lovb.""Code"" <> 'Draft'
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""User"" as uhm ON uhm.""Id""=batch.""HiringManager""
where app.""IsDeleted""=false
#WHERE# ";
                
                var where = $@" and appStatus.""Code""!='REJECTED' and appStatus.""Code""!='RejectedHM' and appStatus.""Code""!='WAITLISTED' ";
                if (jobid.IsNotNullAndNotEmpty())
                {
                    where += $@" and app.""JobId""='{jobid}' ";
                }

                if (orgId.IsNotNullAndNotEmpty())
                {                    
                    where = @$" and app.""JobId""='{jobid}' AND batch.""OrganizationId""='{orgId}' and appStatus.""Code""!='REJECTED' and appStatus.""Code""!='RejectedHM' and appStatus.""Code""!='WAITLISTED' ";
                }
                else
                {
                    if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
                    {
                        var orglist = await GetHODOrg(_repo.UserContext.UserId);
                        //var orgs = orglist.Select(x => x.Id);
                        //var orgId1 = string.Join(",", orgs).TrimEnd(',');
                        var orgId1 = orglist.Id.Replace(",", "','");
                        where += @$" and batch.""OrganizationId"" in ('{orgId1}') ";
                    }
                    else if (_repo.UserContext.UserRoleCodes.Contains("HM"))
                    {
                        var orglist = await GetHmOrg(_repo.UserContext.UserId);
                        //var orgs = orglist.Select(x => x.Id);
                        //var orgId1 = string.Join(",", orgs).TrimEnd(',');
                        var orgId1 = orglist.Id.Replace(",", "','");
                        where += @$" and batch.""OrganizationId"" in ('{orgId1}') ";
                    }
                }
                if (status.IsNotNullAndNotEmpty())
                {
                    where += $@" and appStatus.""Code""='{status}' ";
                }

                query = query.Replace("#WHERE#", where);
                var applicationState = "";
                if (jobadvtstate.IsNotNullAndNotEmpty())
                {
                    if (jobadvtstate == "Joined")
                    {
                        applicationState = @"join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId"" and (apps.""Code"" = 'PostWorkerJoined' or apps.""Code"" = 'PostStaffJoined' or apps.""Code"" =  '" + jobadvtstate + "') ";
                    }
                    else
                    {
                        applicationState = @"join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId"" and apps.""Code"" =  '" + jobadvtstate + "' ";
                    }
                }
                else
                {
                    applicationState = @"join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId"" ";

                }
                query = query.Replace("#APPLICATIONSTATE#", applicationState);
                var allList = await _queryRepo.ExecuteQueryList<RecApplicationViewModel>(query, null);

                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<IList<RecApplicationViewModel>> GetDirictHiringData(string jobid, string orgId)
        {
            try
            {
                var query = @$"Select rt.""Id"" as ServiceId,rt.""ServiceNo"",'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,app.""Email"" as Email,app.""ApplicationNo"" as ApplicationNo,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,batch.""BatchName"" as BatchName,org.""DepartmentName"" as OrganizationName,job.""JobTitle"" as JobName,job.""JobTitle"" as PositionName,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
lov.""Name"" as Gender, app.""MaritalStatusId"" as MaritalStatus,mar.""Name"" as MaritalStatusName,
 app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score,
apps.""Name"" as ApplicationState, appStatus.""Name"" as ApplicationStatus
from public.""NtsService"" as rt
left join cms.""N_REC_DIRECT_HIRING"" as udf on rt.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false
left join cms.""N_REC_APPLICATION"" as app on app.""Id""=udf.""ApplicationId"" and app.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""CurrentStatusId""
left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId"" 
left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left JOIN public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" AND lovb.""Code"" <> 'Draft'
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId"" 
where rt.""TemplateCode""='REC_DIRECT_HIRING'  #WHERE#";
                var where = "";
                if (jobid.IsNotNull())
                {
                    where = @$" and job.""Id""='{jobid}'";
                }
                if (jobid.IsNotNull() && orgId.IsNotNull())
                {
                    where = @$" and job.""Id""='{jobid}' and org.""Id""='{orgId}'";
                }
                else
                {
                    if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
                    {
                        var orglist = await GetHODOrg(_repo.UserContext.UserId);
                        //var orgs = orglist.Select(x => x.Id);
                        //var orgId1 = string.Join(",", orgs).TrimEnd(',');
                        var orgId1 = orglist.Id.Replace(",", "','");
                        where += @$" and batch.""OrganizationId"" in ('{orgId1}') ";
                    }
                    else if (_repo.UserContext.UserRoleCodes.Contains("HM"))
                    {
                        var orglist = await GetHmOrg(_repo.UserContext.UserId);
                        //var orgs = orglist.Select(x => x.Id);
                        //var orgId1 = string.Join(",", orgs).TrimEnd(',');
                        var orgId1 = orglist.Id.Replace(",", "','");
                        where += @$" and batch.""OrganizationId"" in ('{orgId1}') ";
                    }
                }

                query = query.Replace("#WHERE#", where);
                var allList = await _queryRepo.ExecuteQueryList<RecApplicationViewModel>(query, null);
                if (allList.Count() > 0)
                {
                    var allIds = string.Join(",", allList.Select(x => x.ServiceId));
                    allIds = allIds.Replace(",", "','");

                    //var taskquery = @$"SELECT task.""Id"" as Id,task.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName 
                    //        FROM public.""RecTask"" as s

                    //    left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                    //    left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                    //    where s.""TemplateCode"" ='DIRECT_HIRING' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and task.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                    var taskquery = @$"SELECT task.""Id"" as Id,task.""ParentServiceId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo, au.""Name"" as AssigneeUserName, ts.""Name"" as TaskStatusName 
                            FROM public.""NtsService"" as s                                   
                        left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
                        left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
                        left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and (ss.""Code""='SERVICE_STATUS_INPROGRESS' or ss.""Code""='SERVICE_STATUS_OVERDUE' or ss.""Code""='SERVICE_STATUS_COMPLETED') 
                        left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false
                        where s.""TemplateCode"" ='REC_DIRECT_HIRING' 
                        and s.""Id"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                    var tasklist = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(taskquery, null);

                    foreach (var item in allList)
                    {
                        var i = 1;
                        var tasks = tasklist.Where(x => x.ReferenceTypeId == item.ServiceId);
                        foreach (var item1 in tasks)
                        {
                            var Col1 = string.Concat("Step", i);
                            ApplicationExtension.SetPropertyValue(item, Col1, item1.Id);

                            var Col2 = string.Concat("StepNo", i);
                            ApplicationExtension.SetPropertyValue(item, Col2, item1.TaskStatusName + "-" + item1.AssigneeUserName);

                            i++;
                        }                        
                    }
                }
                return allList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IList<RecApplicationViewModel>> GetJobAdvertismentApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode=null, string templateCodeOther=null)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,app.""Email"" as Email,app.""ApplicationNo"" as ApplicationNo,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
lov.""Name"" as Gender, app.""MaritalStatusId"" as MaritalStatus,mar.""Name"" as MaritalStatusName,
 app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score,
app.""ApplicationStateId"" as ApplicationState, appStatus.""Name"" as ApplicationStatus,
job.""JobTitle"" as PositionName,org.""DepartmentName"" as OrganizationName,batch.""Id"" as BatchId,batch.""BatchName"" as BatchName
,lovb.""Name"" as BatchStatusName
FROM cms.""N_REC_APPLICATION"" as app
#APPLICATIONSTATE#
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""CurrentStatusId""
--left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId"" 
left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
INNER JOIN public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" AND lovb.""Code"" <> 'Draft'
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId"" 
where app.""IsDeleted"" = false
#WHERE# ";
                var where = "";
                if (jobadvtid.IsNotNullAndNotEmpty())
                {
                    where = $@" and batch.""JobId""='{jobadvtid}' ";
                }
                if (orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" and batch.""JobId""='{jobadvtid}' AND batch.""OrganizationId""='{orgId}' ";
                }
                else
                {
                    if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
                    {                        
                        var orglist = await GetHODOrg(_repo.UserContext.UserId);
                        //var orgs = orglist.Select(x => x.Id);
                        //var orgId1 = string.Join(",", orgs).TrimEnd(',');
                        var orgId1 = orglist.Id.Replace(",", "','");
                        where += @$" and batch.""OrganizationId"" in ('{orgId1}') ";
                    }
                    else if (_repo.UserContext.UserRoleCodes.Contains("HM"))
                    {
                        var orglist = await GetHmOrg(_repo.UserContext.UserId);
                        //var orgs = orglist.Select(x => x.Id);
                        //var orgId1 = string.Join(",", orgs).TrimEnd(',');
                        var orgId1 = orglist.Id.Replace(",", "','");
                        where += @$" and batch.""OrganizationId"" in ('{orgId1}') ";
                    }
                }
                query = query.Replace("#WHERE#", where);
                var applicationState = "";
                if (jobadvtstate.IsNotNullAndNotEmpty())
                {
                    applicationState = @"join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId"" and apps.""Code"" =  '" + jobadvtstate + "' ";
                }
                else
                {
                    applicationState = @" left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId"" ";
                }                

                query = query.Replace("#APPLICATIONSTATE#", applicationState);
                var allList = await _queryRepo.ExecuteQueryList<RecApplicationViewModel>(query, null);

                if (allList.Count() > 0)
                {
                    var allIds = string.Join(",", allList.Select(x => x.ApplicationId));
                    allIds = allIds.Replace(",", "','");
                    var taskquery = "";
                    if (templateCode.IsNotNullAndNotEmpty())
                    {
                        var template = await _templateBusiness.GetSingle(x => x.Code == templateCode);
                        var tableMetaData = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == template.UdfTableMetadataId);
                        
                        //var taskquery = @$"SELECT task.""Id"" as Id,s.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName 
                        //    FROM public.""RecTask"" as s                                   
                        //    left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                        //    left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        //    where s.""TemplateCode"" ='{templateCode}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and s.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                        taskquery = @$"SELECT t.""Id"" as Id,udf.""ApplicationId"" as ReferenceTypeId,t.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, ts.""Name"" as TaskStatusName 
                        , t.""TemplateCode"" as TemplateCode, t.""CreatedDate""
                        FROM public.""NtsService"" as s
                        left join cms.""{tableMetaData.Name}"" as udf on s.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false
                        left join public.""NtsTask"" as t on t.""ParentServiceId"" = s.""Id"" 
                        left join public.""User"" as au on  au.""Id"" = t.""AssignedToUserId""
                        left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and (ss.""Code""='SERVICE_STATUS_INPROGRESS' or ss.""Code""='SERVICE_STATUS_OVERDUE' or ss.""Code""='SERVICE_STATUS_COMPLETE')
                        left join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false
                        where s.""TemplateCode"" ='{templateCode}' and udf.""ApplicationId"" in ('{allIds}') order by t.""CreatedDate"" asc ";

                        if (templateCode == "EMPLOYEE_APPOINTMENT")
                        {
                            //taskquery = @$"SELECT task.""Id"" as Id,s.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName,task.""CreatedDate""  as ""CreatedDate"" 
                            //FROM public.""RecTask"" as s

                            //left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                            //left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                            //where s.""TemplateCode"" ='{templateCode}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and s.""ReferenceTypeId"" in ('{allIds}')
                            //union
                            //SELECT task.""Id"" as Id,task.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName,task.""CreatedDate""  as ""CreatedDate"" 
                            //FROM public.""RecTask"" as s
                            //join rec.""Application"" as ap on  s.""ReferenceTypeId"" = ap.""Id"" 
                            //join public.""RecTask"" as task on  task.""ReferenceTypeId"" = ap.""Id"" and task.""TemplateCode""='REVISING_INTENT_TO_OFFER_HOD'
                            //join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                            //where s.""TemplateCode"" ='{templateCode}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and task.""ReferenceTypeId"" in ('{allIds}') 
                            //order by ""CreatedDate"" asc ";

                            taskquery = @$"SELECT task.""Id"" as Id,udf.""ApplicationId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, ts.""Name"" as TaskStatusName,task.""CreatedDate""  as ""CreatedDate"" 
                        , task.""TemplateCode"" as TemplateCode
                        FROM public.""NtsService"" as s  
                        left join cms.""{tableMetaData.Name}"" as udf on s.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false
                        left join public.""NtsTask"" as task on task.""ParentServiceId"" = s.""Id"" 
                        left join public.""User"" as au on au.""Id"" = task.""AssignedToUserId""
                        left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and (ss.""Code""='SERVICE_STATUS_INPROGRESS' or ss.""Code""='SERVICE_STATUS_OVERDUE' or ss.""Code""='SERVICE_STATUS_COMPLETE')
                        left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false                        
                        where s.""TemplateCode"" ='{templateCode}' and udf.""ApplicationId"" in ('{allIds}')
                        
                        union

                        SELECT task.""Id"" as Id,udf.""ApplicationId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, ts.""Name"" as TaskStatusName,task.""CreatedDate""  as ""CreatedDate"" 
                        , task.""TemplateCode"" as TemplateCode
                        FROM public.""NtsService"" as s
                        left join cms.""{tableMetaData.Name}"" as udf on s.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false           
                        join cms.""N_REC_APPLICATION"" as ap on  udf.""ApplicationId"" = ap.""Id"" 
                        join public.""NtsTask"" as task on  task.""ParentServiceId"" = ap.""Id"" and task.""TemplateCode""='REVISING_INTENT_TO_OFFER_HOD'
                        join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
                        left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and (ss.""Code""='SERVICE_STATUS_INPROGRESS' or ss.""Code""='SERVICE_STATUS_OVERDUE' or ss.""Code""='SERVICE_STATUS_COMPLETE')
                        left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false                        
                        where s.""TemplateCode"" ='{templateCode}' and udf.""ApplicationId"" in ('{allIds}') 
                        order by ""CreatedDate"" asc ";
                        }
                    }                    

                    var otherTaskList = new List<RecTaskViewModel>();
                    if (templateCodeOther.IsNotNullAndNotEmpty())
                    {
                        //var taskquery1 = @$"SELECT task.""Id"" as Id,s.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName 
                        //FROM public.""RecTask"" as s
                        //left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                        //left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        //where s.""VersionNo""=cast(task.""ParentVersionNo"" as int) and s.""TemplateCode"" ='{templateCodeOther}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and s.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                        var templateother = await _templateBusiness.GetSingle(x => x.Code == templateCodeOther);
                        var tableMetaDataOther = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == templateother.UdfTableMetadataId);

                        var taskquery1 = @$"SELECT task.""Id"" as Id,udf.""ApplicationId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, ts.""Name"" as TaskStatusName 
                        , task.""TemplateCode"" as TemplateCode, task.""CreatedDate""
                        FROM public.""NtsService"" as s
                        left join cms.""{tableMetaDataOther.Name}"" as udf on s.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false
                        left join public.""NtsTask"" as task on task.""ParentServiceId"" = s.""Id"" and task.""IsDeleted""=false
                        left join public.""User"" as au on au.""Id"" = task.""AssignedToUserId"" and au.""IsDeleted""=false
                        left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and (ss.""Code""='SERVICE_STATUS_INPROGRESS' or ss.""Code""='SERVICE_STATUS_OVERDUE' or ss.""Code""='SERVICE_STATUS_COMPLETE')
                        left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false                        
                        where s.""TemplateCode"" ='{templateCodeOther}' and udf.""ApplicationId"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                        otherTaskList = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(taskquery1, null);
                    }
                    var tasklist = new List<RecTaskViewModel>();
                    if (taskquery.IsNotNullAndNotEmpty())
                    {
                       tasklist = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(taskquery, null);
                    }

                    foreach (var item in allList)
                    {
                        var i = 1;
                        var tasks = tasklist.Where(x => x.ReferenceTypeId == item.ApplicationId);
                        tasks = tasks.OrderBy(x => x.TaskNo);
                        foreach (var item1 in tasks)
                        {
                            var Col1 = string.Concat("Step", i);
                            ApplicationExtension.SetPropertyValue(item, Col1, item1.Id);

                            var Col2 = string.Concat("StepNo", i);
                            ApplicationExtension.SetPropertyValue(item, Col2, item1.TaskStatusName + "-" + item1.AssigneeUserName);

                            ApplicationExtension.SetPropertyValue(item, "TaskTemplateCode", item1.TemplateCode);

                            i++;
                        }
                        if (templateCodeOther.IsNotNullAndNotEmpty())
                        {
                            var othertasks = otherTaskList.Where(x => x.ReferenceTypeId == item.ApplicationId);
                            foreach (var item1 in othertasks)
                            {
                                var Col1 = string.Concat("Step", i);
                                ApplicationExtension.SetPropertyValue(item, Col1, item1.Id);

                                var Col2 = string.Concat("StepNo", i);
                                ApplicationExtension.SetPropertyValue(item, Col2, item1.TaskStatusName + "-" + item1.AssigneeUserName);

                                i++;
                            }
                        }
                    }
                }
                return allList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IList<ApplicationStateTrackDetailViewModel>> GetAppStateTrackDetailsByCand(string applicationId)
        {
            string query = @$"select ast.""ChangedDate"", ast.""ChangedBy"", ast.""ApplicationStateId""
            , concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName
            , a.""AppliedDate"" as AppliedDate,--a.""ShortlistByHMComment"",
            u.""Name"" as ChangedByName,apstatus.""Code"" as ApplicationStatusCode
            , apst.""Code"" as StateCode,apstatus.""Code"" as StatusCode,apstatus.""Name"" as StatusName,aps.""Name"" as ApplicationStatusName
            ,t.""Id"" as TaskId,t.""TaskNo"" as TaskNo,t.""TaskSubject"" as TaskSubject,t.""StartDate"" as TaskStartDate
            ,t.""DueDate"" as TaskDueDate, t.""TaskSLA"" as TaskSLA,t.""CreatedDate"" as TaskSubmittedDate
            ,t.""CompletedDate"" as TaskCompletionDate,t.""RejectedDate"" as TaskRejectedDate	
            ,t.""AssignedToUserId"" as TaskAssignedToUserId,tu.""Email"" as TaskAssignedToEmail
            ,ts.""Code"" as TaskStatusCode,ts.""Name"" as TaskStatusName
            ,tt.""DisplayName"" as TaskTemplateSubject,
            --t.""TextValue1"",t.""TextValue2"",t.""TextValue3""
            --,t.""TextValue4"",t.""TextValue5"",t.""TextValue6"",t.""TextValue7"",t.""TextValue8""
            --,t.""TextValue9"",t.""TextValue10"",
            tu.""Name"" as AssigneeName
            from cms.""N_REC_APPLICATION_STATE_TRACK"" as ast
            join cms.""N_REC_APPLICATION"" as a on a.""Id"" = ast.""ApplicationId"" and a.""IsDeleted""=false
            left join public.""User"" as u on u.""Id"" = ast.""ChangedBy""
            left join public.""LOV"" as apst on apst.""Id"" = ast.""ApplicationStateId""
            left join public.""LOV"" as apstatus on apstatus.""Id"" = ast.""ApplicationStatusId""
            left join public.""LOV"" as aps on aps.""Id""=a.""CurrentStatusId""
            left join public.""NtsTask"" as t on ast.""TaskReferenceId""=t.""Id""
            left join public.""User"" as tu on t.""AssignedToUserId"" = tu.""Id""
            left join public.""LOV"" as ts on t.""TaskStatusId""=ts.""Id""
            left join public.""Template"" as tt on ast.""ApplicationStatusCode""=tt.""Code"" and t.""Id"" is not null
            where ast.""ApplicationId""='{applicationId}'";

            var states = await _queryApp.ExecuteQueryList<ApplicationStateTrackDetailViewModel>(query, null);
            
            return states;
        }

        public async Task<IList<ApplicationStateTrackDetailViewModel>> GetTaskCommentsList(string applicationId)
        {
            string query = $@"select ie.""RecruiterComment"" as TextValue1, ie.""HiringManagerRemarks"" as TextValue2, ie.""HodComment"" as TextValue3, null as TextValue4,
null as TextValue5, null as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_InterviewEvaluation"" as ie
join public.""NtsService"" as s on ie.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where ie.""ApplicationId""='{applicationId}'
union
select ito.""SalaryRevisionComment"" as TextValue1, ito.""CandidateComment"" as TextValue2, ito.""HodComment"" as TextValue3, ito.""ReviewerComment"" as TextValue4,
ito.""HrHeadComment"" as TextValue5, ito.""EdComment"" as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_StaffAppointmentrequest"" as ito
join public.""NtsService"" as s on ito.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where ito.""ApplicationId""='{applicationId}'
union
select war.""hodComment"" as TextValue1, war.""ReviewHRComment"" as TextValue2, war.""HRHeadComment"" as TextValue3, war.""EDComment"" as TextValue4,
war.""HRComment"" as TextValue5, war.""CandidateComment"" as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_WorkerAppointmentrequestapproval"" as war
join public.""NtsService"" as s on war.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where war.""ApplicationId""='{applicationId}'
union
select fo.""SalaryRevisionComment"" as TextValue1, fo.""HRHeadComment"" as TextValue2, fo.""EDComment"" as TextValue3, fo.""CandidateComment"" as TextValue4,
fo.""ReviwerComment"" as TextValue5, null as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_FINAL_OFFER"" as fo
join public.""NtsService"" as s on fo.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where fo.""ApplicationId""='{applicationId}'
union
select bv.""CandidateComment"" as TextValue1, bv.""PROComment"" as TextValue2, bv.""PROVisaComment"" as TextValue3, bv.""TravellngComment"" as TextValue4,
null as TextValue5, null as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_OverseasBusinessVISA"" as bv
join public.""NtsService"" as s on bv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where bv.""ApplicationId""='{applicationId}'
union
select wv.""ProCommentMol"" as TextValue1, wv.""ProCommentQvc"" as TextValue2, wv.""CandidateCommentMedical"" as TextValue3, wv.""ProCommentVisa"" as TextValue4,
wv.""CandidateCommentVisa"" as TextValue5, null as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_OverseasworkVisa"" as wv
join public.""NtsService"" as s on wv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where wv.""ApplicationId""='{applicationId}'
union
select vt.""CandidateCommentSubmit"" as TextValue1, vt.""RecruiterCommentVerifyDocuments"" as TextValue2, vt.""ProComment"" as TextValue3, vt.""RecruiterCommentVerifyVisa"" as TextValue4
,vt.""CandidateCommentReceive"" as TextValue5
, null as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_LOCAL_VISA_TRANSFER"" as vt
join public.""NtsService"" as s on vt.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where vt.""ApplicationId""='{applicationId}'
union
select lwp.""CandidateCommentSubmit"" as TextValue1, lwp.""RecruiterCommentVerifyDocument"" as TextValue2, lwp.""ProComment"" as TextValue3, lwp.""RecruiterCommentVerifyWp"" as TextValue4,
lwp.""CandidateCommentReceive"" as TextValue5, null as TextValue6, null as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_LocalWorkPermit"" as lwp
join public.""NtsService"" as s on lwp.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where lwp.""ApplicationId""='{applicationId}'
union
select sjf.""HRCommentProvideCash"" as TextValue1, sjf.""HRCommentSendEmail"" as TextValue2, sjf.""HRCommentSendFRAHRA"" as TextValue3, sjf.""HRCommentUploadPassport"" as TextValue4,
sjf.""HRCommentUpdateEmployee"" as TextValue5, sjf.""HRCommentConfirmInduction"" as TextValue6, sjf.""HMCommentConfirmProbation"" as TextValue7, sjf.""HRCommentConfirmProbation"" as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_Staff_Joining"" as sjf
join public.""NtsService"" as s on sjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where sjf.""ApplicationId""='{applicationId}'
union
select wjf.""HRCommentFillDetails"" as TextValue1, wjf.""HRCommentProvideCash"" as TextValue2, wjf.""HRCommentUploadPassport"" as TextValue3, wjf.""HRCommentUpdateEmployee"" as TextValue4,
wjf.""HRCommentConductInduction"" as TextValue5, wjf.""HMCommentExtendProbation"" as TextValue6, wjf.""HRCommentExtendProbation"" as TextValue7, null as TextValue8,null as TextValue9, null as TextValue10
from cms.""N_REC_WF_WORKER_JOINING"" as wjf
join public.""NtsService"" as s on wjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
where wjf.""ApplicationId""='{applicationId}' ";

            var comments = await _queryApp.ExecuteQueryList<ApplicationStateTrackDetailViewModel>(query, null);

            return comments;
        }
        public async Task<List<HiringManagerViewModel>> GetHiringManagersList()
        {
            string query = @$" SELECT l.""Id"",l.""Name"",l.""UserId"",l.""DesignationId"",l.""OrganizationId"" as ""OrganizationIds"", d.""Name"" as DepartmentName,p.""JobTitle"" as DesignationName
                               --,string_agg(o.""Name"", ',') as organizationName
                                FROM cms.""N_REC_HIRING_MANAGER"" as l
                                left JOIN cms.""N_CoreHR_HRJob"" as p on p.""Id"" = l.""DesignationId""
                                --LEFT JOIN cms.""N_CoreHR_HRDepartment"" as o ON o.""Id"" IN l.""OrganizationId""
                                LEFT JOIN public.""LOV"" as d on d.""Id"" = l.""DepartmentId""
                                WHERE l.""IsDeleted"" = false  group by l.""Id"", d.""Id"", p.""Id"" order by l.""CreatedDate"" desc ";
            var queryData = await _queryApp.ExecuteQueryList<HiringManagerViewModel>(query, null);

            if (queryData.IsNotNull())
            {
                foreach (var item in queryData)
                {
                    if (item.OrganizationIds.IsNotNullAndNotEmpty())
                    {
                        var orgs = item.OrganizationIds.Trim('[', ']');
                        orgs = orgs.Replace("\"", "\'");
                        var query1 = $@" select string_agg(o.""DepartmentName""::text, ', ') as orgname
                                    from cms.""N_CoreHR_HRDepartment"" as o
                                    where o.""IsDeleted"" = false and o.""Id"" IN ({orgs})
                                    ";
                        var data1 = await _queryRepo.ExecuteScalar<string>(query1, null);
                        if (data1.IsNotNullAndNotEmpty())
                        {
                            item.OrganizationName = data1;
                        }
                    }
                }
            }
            return queryData;
        }
        public async Task<RecApplicationViewModel> GetApplicationEvaluationDetails(string applicationId)
        {
            string query = @$"SELECT app.*,
                            CONCAT( app.""FirstName"",' ',app.""MiddleName"" ,' ',app.""LastName"") as FullName,
                                    p.""JobTitle"" as PositionName,d.""Name"" as DivisionName,a.""Name"" as AccommodationName,
                                    st.""Name"" as SelectedThroughName,
                                    u.""Name"" as InterviewByUserName,g.""GradeName"" as GradeName
                                    FROM cms.""N_REC_APPLICATION"" as app
                                    LEFT JOIN cms.""N_CoreHR_HRJob"" as p ON p.""Id"" = app.""JobId""
                                    LEFT JOIN cms.""N_CoreHR_HRGrade"" as g ON g.""Id"" = app.""OfferGrade""
                                    LEFT JOIN public.""LOV"" as d ON d.""Id"" = app.""DivisionId""
                                    LEFT JOIN public.""LOV"" as a ON a.""Id"" = app.""AccommodationId""
                                    LEFT JOIN public.""LOV"" as st ON st.""Id"" = app.""SelectedThroughId""
                                    LEFT JOIN public.""User"" as u ON u.""Id""=app.""InterviewByUserId""
                                    WHERE app.""Id"" = '{applicationId}' and app.""IsDeleted""=false";

            var queryData = await _queryApp.ExecuteQuerySingle<RecApplicationViewModel>(query, null);

            return queryData;
        }
        public async Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationDetails(string applicationId)
        {
            string query = @$"SELECT evat.*,false as ""IsTemplate"",evat.""NtsNoteId"" as ""CandidateEvaluationNoteId""
                                    FROM cms.""N_REC_REC_CANDIDATE_EVALUTION"" as evat
                                    WHERE evat.""IsDeleted""=false and evat.""ApplicationId""='{applicationId}'";
            var queryData = await _queryApp.ExecuteQueryList<RecCandidateEvaluationViewModel>(query, null);
            return queryData;
        }
        public async Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationTemplateData()
        {
            string query = @$"SELECT evat.*,true as ""IsTemplate""
                                    FROM cms.""N_REC_REC_CANDIDATE_EVALUTION_TEMPLATE"" as evat
                                    WHERE evat.""IsDeleted""=false";
            var queryData = await _queryApp.ExecuteQueryList<RecCandidateEvaluationViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<RecApplicationViewModel>> GetCandiadteShortListApplicationData()
        {
            var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as Id,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
app.""GenderId"" as Gender,glov.""Name"" as GenderName, app.""MaritalStatusId"" as MaritalStatus,mlov.""Name"" as MaritalStatusName,
app.""PassportNumber"" as PassportNumber, pic.""CountryName"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
TRUNC(app.""TotalWorkExperience""::decimal,0) as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Id"" as ApplicationStatus,appStatus.""Code"" as ApplicationStatusCode, appStatus.""Name"" as ApplicationStatusName,
apps.""Id"" as ApplicationState,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
app.""Score"" as Score,
mplov.""Name"" as ManpowerTypeName,mplov.""Code"" as ManpowerTypeCode,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,cu.""Name"" as NetSalaryCurrency,
app.""BatchId"" as BatchId,bch.""BatchName"" as BatchName,
s.""Id"" as TaskId,s.""TemplateCode"" as TaskTemplateCode,ss.""Code"" as TaskStatusCode,
bchlov.""Id"" as BatchStatus, bchlov.""Name"" as BatchStatusName, bchlov.""Code"" as BatchStatusCode,
app.""TimeRequiredToJoin"" as TimeRequiredToJoin,app.""AdditionalInformation"" as AdditionalInformation,
app.""OptionForAnotherPosition"" as OptionForAnotherPosition,app.""NoOfChildren"" as NoOfChildren

FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRCurrency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_REC_BATCH"" as bch on bch.""Id""=app.""BatchId""
left join public.""LOV"" as bchlov on bchlov.""Id""=bch.""BatchStatus""
left join public.""LOV"" as glov on glov.""Id""=app.""GenderId""
left join public.""LOV"" as mlov on mlov.""Id""=app.""MaritalStatusId""
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_REC_JobAdvertisement"" as mjd on mjd.""Id"" = app.""JobAdvertisementId""
left join cms.""N_REC_APPLICATION_STATE_TRACK"" as ast on app.""Id""=ast.""ApplicationId""
--join public.""LOV"" as appst on app.""ApplicationStatusId""=appst.""Id"" --and appst.""Code""='SHORTLISTED'

Left join public.""LOV"" as mplov ON mplov.""LOVType""='REC_MANPOWER' and mplov.""Id"" = job.""ManpowerTypeId""
left join cms.""N_REC_InterviewEvaluation"" as ie on app.""Id""=ie.""ApplicationId"" and ie.""IsDeleted""=false
left join public.""NtsService"" as s on ie.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false --AND s.""TemplateCode""=''
left join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""Language""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LANGUAGES') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'en-US'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""Language""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LANGUAGES') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'ar-SA'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""Language""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LANGUAGES') as appcpc on appcpc.""ApplicationId"" = app.""Id""
and appcpc.""Code"" = 'ComputerProficiency'
where app.""IsDeleted""=false
";

            var allList = await _queryApp.ExecuteQueryList<RecApplicationViewModel>(query, null);

            return allList;
        }

        public async Task<List<RecBatchViewModel>> GetBatchHmData(string jobid, string orgId, string HmId, BatchTypeEnum type, string batchId)
        {
            var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"SELECT c.*,j.""JobTitle"" as JobName, o.""DepartmentName"" as Organization, l.""Name"" as BatchStatusName,l.""Code"" as BatchStatusCode,
                            sum(case when q.""Id"" is not null then 1 else 0 end) as NoOfApplication,
							(sum(case when (aps.""Code"" = 'ShortListByHm' AND appst.""Code""='NotShortlisted') then 1 else 0 end)) as NotShortlistByHM,
							(sum(case when (aps.""Code"" = 'ShortListByHm' AND appst.""Code""='ShortlistedHM') then 1 else 0 end)) as ShortlistByHM,
							(sum(case when (aps.""Code"" = 'ShortListByHm' AND appst.""Code""='Interview') then 1 else 0 end)) as ConfirmInterview
							--,(sum(case when task3.""TaskStatusCode"" = 'COMPLETED' then 1 else 0 end)) as Evaluated
                            ,hm.""Name"" as HiringManagerName,hod.""Name"" as HeadOfDepartmentName
                            FROM cms.""N_REC_REC_BATCH"" as c                           
                            JOIN cms.""N_REC_APPLICATION"" as q ON q.""BatchId"" = c.""Id"" and q.""IsDeleted""=false
                            left join cms.""N_REC_HIRING_MANAGER"" as hm on c.""HiringManager""=hm.""UserId""
                            left join cms.""N_REC_HEAD_OF_DEPARTMENT"" as hod on c.""HeadOfDepartment""=hod.""UserId""
                            JOIN public.""LOV"" as aps on aps.""Id"" = q.""ApplicationStateId""
                            LEFT JOIN public.""LOV"" as appst on appst.""Id"" = q.""ApplicationStatusId""
                           -- LEFT JOIN public.""RecTask"" as service ON service.""ReferenceTypeId""=q.""Id"" AND service.""NtsType"" = 2
                           -- LEFT JOIN public.""RecTask"" as task2 ON task2.""ReferenceTypeId""=service.""Id"" AND task2.""TemplateCode""='SCHEDULE_INTERVIEW_CANDIDATE'
							--LEFT JOIN public.""RecTask"" as task3 ON task3.""ReferenceTypeId""=service.""Id"" AND task3.""TemplateCode""='INTERVIEW_EVALUATION_HM'

                            LEFT JOIN cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = c.""JobId""
                            LEFT JOIN cms.""N_CoreHR_HRDepartment"" as o ON o.""Id"" = c.""OrganizationId""
                            LEFT JOIN public.""LOV"" as l ON l.""Id"" = c.""BatchStatus""
                            #WHERE#
                            group by c.""Id"",j.""JobTitle"",o.""Name"",l.""Name"",l.""Code"",hm.""Name"",hod.""Name""
                            ";
            var where = $@" where c.""HiringManager""='{HmId}' and c.""BatchType""={batchtype} ";
            if (jobid.IsNotNullAndNotEmpty())
            {
                where = $@" where c.""JobId""='{jobid}' and c.""HiringManager""='{HmId}' and c.""BatchType""={batchtype} ";
            }
            if (orgId.IsNotNullAndNotEmpty())
            {
                where = $@" where c.""JobId""='{jobid}' and c.""OrganizationId""='{orgId}'  and c.""HiringManager""='{HmId}' and c.""BatchType""={batchtype} ";
            }
            if (batchId.IsNotNullAndNotEmpty())
            {
                where = $@" where c.""Id""='{batchId}' ";
            }
            query = query.Replace("#WHERE#", where);

            var list = await _queryRepo.ExecuteQueryList<RecBatchViewModel>(query, null);
            
            return list;
        }

        public async Task<IList<RecApplicationViewModel>> GetWorkerPoolBatchData(string batchid)
        {
            try
            {
                var query = "";

                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganizationName,
app.""LastName"" as LastName, app.""Age"" as Age, app.""ApplicationNo"" as ApplicationNo, bt.""BatchName"" as BatchName,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountryId,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
TRUNC(app.""TotalWorkExperience""::decimal,0) as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
apps.""Id"" as ApplicationState,
app.""Score"" as Score,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
bs.""Code"" as BatchStatusCode,bs.""Id"" as BatchId,app.""WorkerBatchId"" as WorkerBatchId

FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRCurrency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRDepartment"" as org on org.""Id""=app.""OrganizationId""
left join cms.""N_REC_REC_BATCH"" as bt on bt.""Id""= app.""WorkerBatchId""
left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join public.""LOV"" as gen on gen.""Id""=app.""GenderId"" and gen.""LOVType""='LOV_GENDER'
left join public.""LOV"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatusId"" and maritalstatus.""LOVType""='LOV_MARITALSTATUS'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_REC_APPLICATION_DRIVING_LICENSE"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as dlc on dlc.""Id"" = adl.""CountryId""
left join cms.""N_REC_APPLICATION_EDUCATIONAL"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join cms.""N_REC_APPLICATION_EXPERIENCE"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
left join cms.""N_REC_APPLICATION_EXPERIENCE_COUNTRY"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as aec on aec.""Id"" = appexpc.""CountryId""
left join rec.""N_REC_APPLICATION_EXPERIENCE_JOB"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRJob"" as ej on ej.""Id"" = appexpj.""JobId""
left join cms.""N_REC_APPLICATION_EXPERIENCE_SECTOR"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
left join 
(select appexpbyc.*
 from cms.""N_REC_APPLICATION_EXPERIENCE_COUNTRY"" as appexpbyc 
    inner join cms.""N_CoreHR_HRCountry"" cou on cou.""Id"" = appexpbyc.""CountryId"" and cou.""Code"" = 'IN'
) as appexpbyc on appexpbyc.""ApplicationId"" = app.""Id""
left join 
(select appexpba.*, ct.""Code""
 from cms.""N_REC_APPLICATION_EXPERIENCE_OTHER"" as appexpba
 join public.""LOV"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
 where ct.""LOVType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
and appexpboa.""Code"" = 'Abroad'
left join
(select appexpbg.*, ctt.""Code""
 from cms.""N_REC_APPLICATION_EXPERIENCE_OTHER"" as appexpbg
 join public.""LOV"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""LOVType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf'
 left join 
(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""Language""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LANGUAGES') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join
(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""Language""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LANGUAGES') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'
left join cms.""N_REC_APPLICATION_COMP_PROFICIENCY"" as appcpc on appcpc.""ApplicationId""=app.""Id""
left join public.""LOV"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevelId""
left join cms.""N_REC_APPLICATION_REFERENCES"" as appr on appr.""ApplicationId"" = app.""Id"" 
left join cms.""N_REC_APPLICATION_EXPERIENCE_NATURE"" as appebn on appebn.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationStateComment"" as appc on appc.""ApplicationId""=app.""Id""
left join cms.""N_REC_REC_BATCH"" as b on b.""Id""=app.""BatchId""
left join public.""LOV"" as bs on bs.""Id"" = b.""BatchStatus"" and bs.""LOVType"" = 'BatchStatus'
where app.""WorkerBatchId""='{batchid}' and app.""IsDeleted""=false ";

                var allList = await _queryApp.ExecuteQueryList<RecApplicationViewModel>(query, null);
                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IList<RecApplicationViewModel>> GetBatchData(string batchid)
        {
            try
            {
                var query = "";

                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganizationName,
app.""LastName"" as LastName, app.""Age"" as Age, app.""ApplicationNo"" as ApplicationNo, bt.""BatchName"" as BatchName,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Nationality"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountryId,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
TRUNC(app.""TotalWorkExperience""::decimal,0) as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
apps.""Id"" as ApplicationState,
app.""Score"" as Score,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
bs.""Code"" as BatchStatusCode,bt.""Id"" as BatchId,app.""WorkerBatchId"" as WorkerBatchId

FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRCurrency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""

left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join public.""LOV"" as gen on gen.""Id""=app.""GenderId"" and gen.""LOVType""='LOV_GENDER'
left join public.""LOV"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""LOVType""='LOV_MARITALSTATUS'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_REC_APPLICATION_DRIVING_LICENSE"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as dlc on dlc.""Id"" = adl.""CountryId""
left join cms.""N_REC_APPLICATION_EDUCATIONAL"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join cms.""N_REC_APPLICATION_EXPERIENCE"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
left join cms.""N_REC_APPLICATION_EXPERIENCE_COUNTRY"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as aec on aec.""Id"" = appexpc.""CountryId""
left join cms.""N_REC_APPLICATION_EXPERIENCE_JOB"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRJob"" as ej on ej.""Id"" = appexpj.""JobId""
left join cms.""N_REC_APPLICATION_EXPERIENCE_SECTOR"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
left join 
(select appexpbyc.*
 from cms.""N_REC_APPLICATION_EXPERIENCE_COUNTRY"" as appexpbyc 
inner join cms.""N_CoreHR_HRCountry"" cou on cou.""Id"" = appexpbyc.""CountryId""  and cou.""Code"" = 'IN'
) as appexpbyc   on appexpbyc.""ApplicationId"" = app.""Id""
 left join 
(select appexpba.*, ct.""Code""
 from cms.""N_REC_APPLICATION_EXPERIENCE_OTHER"" as appexpba
 join public.""LOV"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
 where ct.""LOVType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
and appexpboa.""Code"" = 'Abroad'
left join
(select appexpbg.*, ctt.""Code""
 from cms.""N_REC_APPLICATION_EXPERIENCE_OTHER"" as appexpbg
 join public.""LOV"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""LOVType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf'
 left join 
(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""Language""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""Language""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'

left join cms.""N_REC_APPLICATION_COMP_PROFICIENCY"" as appcpc on appcpc.""ApplicationId""=app.""Id""
left join public.""LOV"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevelId""
left join cms.""N_REC_APPLICATION_REFERENCES"" as appr on appr.""ApplicationId"" = app.""Id"" 
left join cms.""N_REC_APPLICATION_EXPERIENCE_NATURE"" as appebn on appebn.""ApplicationId"" = app.""Id"" 

left join cms.""N_REC_REC_BATCH"" as bt on bt.""Id""= app.""BatchId""
left join cms.""Organization"" as org on org.""Id"" = bt.""OrganizationId""

left join public.""LOV"" as bs on bs.""Id"" = bt.""BatchStatus"" and bs.""LOVType"" = 'BatchStatus'
where app.""BatchId""='{batchid}' and app.""IsDeleted""=false ";

                var allList = await _queryApp.ExecuteQueryList<RecApplicationViewModel>(query, null);
                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion j

        #region d
        public async Task<IList<RecTaskViewModel>> GetPendingTaskListByUserId(string userId)
        {
            var list = new List<RecTaskViewModel>();
            string query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( ou.""Name"" for 1) as TaskOwnerFirstLetter,
                        app.""FirstName"" as CandidateName,case when job.""Name"" is not null then job.""Name"" else hjob.""Name"" end as JobName,case when org.""Name"" is not null then org.""Name"" else horg.""Name"" end as OrgUnitName,app.""GaecNo"" as GaecNo,
                        tt.""SequenceOrder"" as SequenceOrder
                        FROM public.""RecTask"" as task
                        left join public.""RecTask"" as service on  service.""Id"" = task.""ReferenceTypeId"" 
                        left join rec.""Application"" as app on app.""Id"" = service.""ReferenceTypeId"" and app.""IsDeleted""=false
                        left join cms.""Job"" as job on  job.""Id"" = app.""JobId""
                        left join rec.""Batch"" as bt on bt.""Id"" = app.""BatchId""
                        left join cms.""Organization"" as org on org.""Id"" = bt.""OrganizationId""
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        left join public.""RecTaskTemplate"" as tt on tt.""TemplateCode""=task.""TemplateCode""  
                        left join cms.""Job"" as hjob on  hjob.""Id"" = task.""DropdownValue1""
                        left join cms.""Organization"" as horg on horg.""Id"" = task.""DropdownValue2""
                        where (task.""AssigneeUserId"" ='{userId}') and (task.""NtsType"" is null or task.""NtsType""=1)";

            var result = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(query, null);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return result;
        }
        public async Task<DataTable> GetJobByOrgUnit(string userId)
        {

            var orgQry = $@" select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
	                FROM public.""RecTask"" as task 
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""	                
                    --join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId"" 
                    --join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId"" and app.""IsDeleted""=false
	                join rec.""Batch"" as b on b.""Id"" = app.""BatchId""                   
                    join cms.""Organization"" as o on o.""Id"" = app.""OrganizationId""
                    join cms.""Job"" as j on j.""Id"" = app.""JobId""
                   
	                where task.""AssigneeUserId""='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    b.""IsDeleted""=false
                    GROUP BY j.""Id"",o.""Id""
                    union
                     select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
	                FROM public.""RecTask"" as task 
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""	                
                    --join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId"" 
                    --join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""	                               
                    join cms.""Organization"" as o on o.""Id"" = task.""DropdownValue2""
                    join cms.""Job"" as j on j.""Id"" = task.""DropdownValue1""
                   
	                where task.""AssigneeUserId""='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and task.""IsDeleted""=false 
                    
                    GROUP BY j.""Id"",o.""Id""";
            var result = await _queryRepo.ExecuteQueryList<ManpowerRecruitmentSummaryViewModel>(orgQry, null);
            var Columns = result.Select(x => new ManpowerRecruitmentSummaryViewModel { OrganizationId = x.OrganizationId, OrganizationName = x.OrganizationName });
            Columns = Columns.GroupBy(x => x.OrganizationId).Select(x => x.FirstOrDefault());

            DataTable dataTable = new DataTable("Job_Org");
            try
            {
                if (Columns != null)
                {
                    var length = 2;
                    dataTable.Columns.Add("Position");
                    dataTable.Columns.Add("All");
                    foreach (var item in Columns)
                    {
                        dataTable.Columns.Add("_" + item.OrganizationName);
                        length++;
                    }

                }
            }
            catch (Exception e)
            {

            }
            return dataTable;
        }
        public async Task<IList<IdNameViewModel>> GetIdNameList(string Type)
        {
            var query = "";
            var list = new List<IdNameViewModel>();
            try
            {
                //if (Type == "Position")
                //{
                //    query = @$"SELECT ""Id"",""Name"" FROM cms.""Position"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                //}
                if (Type == "Organization")
                {
                    query = @$"SELECT ""Id"",""DepartmentName"" as Name FROM cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false and ""Status""=1 order by ""DepartmentName""";
                }
                else if (Type == "Country")
                {
                    query = @$"SELECT ""Id"",""CountryName"" as Name , ""Code"" FROM cms.""N_CoreHR_HRCountry"" where ""IsDeleted""=false and ""Status""=1 order by ""CountryName""";
                }
                else if (Type == "Job")
                {
                    query = @$"SELECT ""Id"",""JobTitle"" as Name FROM cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false and ""Status""=1 order by ""JobName""";
                }
                else if (Type == "Position")
                {
                    query = @$"SELECT ""Id"",""PositionName"" as Name FROM cms.""N_CoreHR_HRPosition"" where ""IsDeleted""=false and ""Status""=1 order by ""PositionName""";
                }
                else if (Type == "Location")
                {
                    query = @$"SELECT ""Id"",""LocationName"" as Name FROM cms.""N_CoreHR_HRLocation"" where ""IsDeleted""=false and ""Status""=1 order by ""LocationName""";
                }
                else if (Type == "Nationality")
                {
                    query = @$"SELECT ""Id"",""NationalityName"" as Name FROM cms.""N_CoreHR_HRNationality"" where ""IsDeleted""=false and ""Status""=1 order by ""NationalityName""";
                }
                else if (Type == "Currency")
                {
                    query = @$"SELECT ""Id"",""Name"" FROM cms.""Currency"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }

                list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            catch (Exception)
            {

            }
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetCountryIdNameList()
        {
            string query = @$"SELECT ""Id"", ""Name""
                                FROM cms.""Country"" where ""IsDeleted""=false";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<DataTable> GetTaskByOrgUnit(string userId, string userroleId)
        {
            var result = new List<ManpowerRecruitmentSummaryViewModel>();
            var userCode = "";

            var query = $@" select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
	                FROM public.""RecTask"" as task 
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""	                
                    --join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId"" and ur.""UserRoleId""= '{userroleId}'
                    --join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId"" and app.""IsDeleted""=false
	                join rec.""Batch"" as b on b.""Id"" = app.""BatchId""                   
                    join cms.""Organization"" as o on o.""Id"" = app.""OrganizationId""
                    join cms.""Job"" as j on j.""Id"" = app.""JobId""
                   
	                where task.""AssigneeUserId""='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    b.""IsDeleted""=false
                    GROUP BY j.""Id"",o.""Id""
                    union
                     select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
	                FROM public.""RecTask"" as task 
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""	                
                    --join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId"" and ur.""UserRoleId""= '{userroleId}'
                    --join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""	                               
                    join cms.""Organization"" as o on o.""Id"" = task.""DropdownValue2""
                    join cms.""Job"" as j on j.""Id"" = task.""DropdownValue1""
                   
	                where task.""AssigneeUserId""='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and task.""IsDeleted""=false 
                    
                    GROUP BY j.""Id"",o.""Id""
                    union
                    select j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count"" 
                    FROM public.""RecTask"" as task
                    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode"" and tmp.""TemplateCode""='SCHEDULE_INTERVIEW_RECRUITER'
                    join rec.""Application"" as app on app.""Id"" = task.""ReferenceTypeId"" and app.""IsDeleted""=false
                    left join cms.""Job"" as j on  j.""Id"" = app.""JobId""
                    left join rec.""Batch"" as bt on bt.""Id"" = app.""BatchId""
                    left join cms.""Organization"" as o on o.""Id"" = bt.""OrganizationId""
                    where task.""AssigneeUserId""='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and task.""IsDeleted""=false 
                    GROUP BY j.""Id"", o.""Id""
                    union
                    select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
                    FROM public.""RecTask"" as task
                    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode"" and tmp.""TemplateCode""='TASK_DIRECT_HIRING'                 
                    join cms.""Job"" as j on j.""Id"" = task.""DropdownValue1""
                    join cms.""Organization"" as o on o.""Id"" = task.""DropdownValue2""
                    where task.""AssigneeUserId""='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and task.""IsDeleted""=false 
                    GROUP BY j.""Id"", o.""Id""
                    union
                    select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
                    FROM public.""RecTask"" as task
                    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode"" and tmp.""TemplateCode""='JOBDESCRIPTION_HM'                 
                    join cms.""Job"" as j on j.""Id"" = task.""DropdownValue1""
                    left join cms.""Organization"" as o on o.""Id"" = task.""DropdownValue2""
                    where task.""AssigneeUserId""='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and task.""IsDeleted""=false 
                    GROUP BY j.""Id"", o.""Id""

                    ";
            result = await _queryRepo.ExecuteQueryList<ManpowerRecruitmentSummaryViewModel>(query, null);

            var Rows = result.Select(x => new ManpowerRecruitmentSummaryViewModel { JobId = x.JobId, JobTitle = x.JobTitle });
            var Columns = result.Select(x => new ManpowerRecruitmentSummaryViewModel { OrganizationId = x.OrganizationId, OrganizationName = x.OrganizationName });
            Columns = Columns.GroupBy(x => x.OrganizationId).Select(x => x.FirstOrDefault());

            DataTable dataTable = new DataTable("Job_Org");
            try
            {
                if (Columns != null)
                {
                    //var rec = (IDictionary<string, object>)Columns;
                    var length = 2;
                    dataTable.Columns.Add("Position");
                    dataTable.Columns.Add("All");
                    foreach (var item in Columns)
                    {
                        //Setting column names as Property names
                        //var test = item.Key;
                        dataTable.Columns.Add("_" + item.OrganizationName);
                        length++;
                    }
                    if (true)
                    {
                        foreach (var r in Rows)
                        {
                            var values = new object[length];
                            var rows = result.Where(x => x.JobId == r.JobId);
                            var total = rows.Sum(x => x.Count);
                            if (total != 0)
                            {
                                int i = 2;
                                values[0] = r.JobTitle;
                                values[1] = total;
                                foreach (var row in Columns)
                                {
                                    Object obj = 0;
                                    foreach (var row1 in rows)
                                    {
                                        if (row.OrganizationId == row1.OrganizationId)
                                        {
                                            obj = row1.Count;
                                        }

                                    }
                                    values[i] = obj;
                                    i++;
                                }
                                dataTable.Rows.Add(values);
                            }
                        }
                        var sum = new object[length];
                        sum[0] = "Total";
                        //sum[1] = dataTable.Compute("SUM(All)", string.Empty);
                        var c = dataTable.AsEnumerable().Sum(x => Convert.ToInt32(x["All"]));
                        sum[1] = c;
                        int k = 0;
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var cName = column.ColumnName;
                            if (k >= 2)
                            {
                                var d = dataTable.AsEnumerable().Sum(x => Convert.ToInt32(x[cName]));
                                sum[k] = d;
                            }
                            k++;
                        }
                        dataTable.Rows.Add(sum);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return dataTable;

        }
        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameDashboardList()
        {

            string query = @$"with cte as (
            SELECT distinct j.""Id"" ,j.""JobTitle"" ,ja.""Status""                  
                            FROM cms.""N_REC_JobAdvertisement"" as ja
                            inner join cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = ja.""JobId""
                            --inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                           -- and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

            and ja.""Status"" = '1' and ja.""Id"" not in (select ""Id"" from cms.""N_REC_JobAdvertisement""
                                                          where ""JobId"" <> ja.""JobId"") union
                  SELECT      distinct j.""Id"",j.""JobTitle"" ,ja.""Status"" 
                            FROM cms.""N_REC_JobAdvertisement"" as ja
                            inner join cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = ja.""JobId""
                            --inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

            and ja.""Status"" = '2' and j.""Id"" not in (SELECT      distinct   j.""Id""  
                            FROM cms.""N_REC_JobAdvertisement"" as ja
                            inner join cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = ja.""JobId""
                            -- inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

            and ja.""Status"" = '1' and ja.""Id"" not in (select ""Id"" from cms.""N_REC_JobAdvertisement""
                                                          where ""JobId"" <> ja.""JobId"")) )
							select cte.""Id"" as Id,cte.""JobTitle"" as JobName
                            from cte
                            ";
            var queryData = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetOrgByJobAddId(string JobId)
        {
            var query = "";
            var list = new List<IdNameViewModel>();
            try
            {
                query = @$"SELECT org.""DepartmentName"" as Name ,org.""Id""
                        FROM cms.""N_CoreHR_HRJob"" as ja
                        inner join cms.""N_REC_ManpowerRequirement"" as mrc on mrc.""JobId""=ja.""Id"" and mrc.""IsDeleted""=false
                        inner join cms.""N_CoreHR_HRDepartment"" as org on org.""Id""=mrc.""OrganizationId"" and org.""IsDeleted""=false
                        where ja.""Id""='{JobId}' and ja.""IsDeleted""=false and ja.""Status""=1";
                list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public async Task<IdNameViewModel> GetHmOrg(string userId)
        {
            //string query = @$"SELECT o.""Id"",o.""Name"" as DesignationName
            //from rec.""HiringManagerOrganization"" as hmo 
            //                    join  cms.""Organization"" as o ON o.""Id"" = hmo.""OrganizationId"" and o.""IsDeleted""=false
            //                    join rec.""HiringManager"" as c on  c.""Id""=hmo.""HiringManagerId"" and c.""IsDeleted""=false                                
            //                    WHERE c.""IsDeleted""=false and c.""UserId""='{userId}'";

            string query = $@"SELECT c.""OrganizationId"" as Id,j.""JobTitle"" as Name
                                from cms.""N_REC_HIRING_MANAGER"" as c
                                join cms.""N_CoreHR_HRJob"" as j on c.""DesignationId""=j.""Id"" and j.""IsDeleted""=false
                                WHERE c.""IsDeleted""=false and c.""UserId""='{userId}' ";

            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<IdNameViewModel> GetHODOrg(string userId)
        {
            //string query = @$"SELECT o.""Id"",o.""Name"" as DesignationName
            //                    from rec.""HeadOfDepartmentOrganization"" as hmo 
            //                    join  cms.""Organization"" as o ON o.""Id"" = hmo.""OrganizationId"" and o.""IsDeleted""=false
            //                    join rec.""HeadOfDepartment"" as c on  c.""Id""=hmo.""HeadOfDepartmentId"" and c.""IsDeleted""=false                              
            //                    WHERE c.""IsDeleted""=false and c.""UserId""='{userId}'";

            string query = $@"SELECT c.""OrganisationId"" as Id,j.""JobTitle"" as Name
                                from cms.""N_REC_HEAD_OF_DEPARTMENT"" as c
                                join cms.""N_CoreHR_HRJob"" as j on c.""DesignationId""=j.""Id"" and j.""IsDeleted""=false
                                WHERE c.""IsDeleted""=false and c.""UserId""='{userId}' ";

            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<JobAdvertisementViewModel> GetRecruitmentDashobardCount(string orgId)
        {

            string query = @$"with cte as (
            SELECT      distinct   j.""Id"",j.""JobTitle"",ja.""Status""                   
                            FROM cms.""N_REC_JobAdvertisement"" as ja
                            inner join cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = ja.""JobId""
                            -- inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')
            and ja.""Status"" = '1' and ja.""Id"" not in (select ""Id"" from rec.""JobAdvertisement""
                                                          where ""JobId"" <> ja.""JobId"") union
                  SELECT      distinct j.""Id"",j.""JobTitle"",ja.""Status""
                            FROM cms.""N_REC_JobAdvertisement"" as ja
                            inner join cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = ja.""JobId""
                           -- inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')
            and ja.""Status"" = '2' and j.""Id"" not in (SELECT      distinct   j.""Id""               
                            FROM cms.""N_REC_JobAdvertisement"" as ja
                            inner join cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = ja.""JobId""
                           -- inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

            and ja.""Status"" = '1' and ja.""Id"" not in (select ""Id"" from cms.""N_REC_JobAdvertisement""
                                                          where ""JobId"" <> ja.""JobId"")) )
							select
                              sum(case when cte.""Status"" = 1 then 1 else 0 end) as Active,
                            sum(case when cte.""Status"" = 2 then 1 else 0 end) as InActive
                            from cte
                            ";
            var list = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query, null);

            return list.FirstOrDefault();
        }
        public async Task<IList<System.Dynamic.ExpandoObject>> GetPendingTaskDetailsForUser(string userId, string orgId, string userRoleCodes)
        {
            var list = new List<System.Dynamic.ExpandoObject>();
            string query = "";
            var isHR = false;
            var userRoles = userRoleCodes.Split(",");
            //await _repo.GetListGlobal<UserRoleUser, UserRoleUser>(x => x.UserId == userId,
            //  x => x.UserRole);
            if (userRoles.IsNotNull())
            {
                var roles1 = userRoles.Select(x => x).Where(x => x == "HR" || x == "ED");
                if (roles1.IsNotNull() && roles1.Count() > 0)
                {
                    query = @$"SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, batch.""OrganizationId"" as OrgId FROM public.""RecTask"" as task
                        join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                        join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                        join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
                        join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId"" and ap.""IsDeleted""=false -- app id
                        Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
                        join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                        where (task.""NtsType"" is null or task.""NtsType""=1)
                        and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                        GROUP BY url.""Code"", j.""Id"", batch.""OrganizationId""
                        union
                        SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, o.""Id"" as OrgId 
                        FROM public.""RecTask"" as task
                        join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                        join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                        join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode"" and tmp.""TemplateCode""='TASK_DIRECT_HIRING'
                        join cms.""Job"" as j on j.""Id"" = task.""DropdownValue1""
                        join cms.""Organization"" as o on o.""Id"" = task.""DropdownValue2""
                        where task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                        GROUP BY url.""Code"", j.""Id"", o.""Id""
                        union
                        SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, o.""Id"" as OrgId 
                        FROM public.""RecTask"" as task
                        join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                        join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                        join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode"" and tmp.""TemplateCode""='SCHEDULE_INTERVIEW_RECRUITER'
                        join rec.""Application"" as app on app.""Id"" = task.""ReferenceTypeId"" and app.""IsDeleted""=false
                        left join cms.""Job"" as j on j.""Id"" = app.""JobId""
                        left join rec.""Batch"" as bt on bt.""Id"" = app.""BatchId""
                        left join cms.""Organization"" as o on o.""Id"" = bt.""OrganizationId""
                        where task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                        GROUP BY url.""Code"", j.""Id"", o.""Id""
                        union
                        SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, o.""Id"" as OrgId 
                        FROM public.""RecTask"" as task
                        join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                        join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                        join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode"" and tmp.""TemplateCode""='JOBDESCRIPTION_HM'
                        left join cms.""Job"" as j on j.""Id"" = task.""DropdownValue1""
                        left join cms.""Organization"" as o on o.""Id"" = task.""DropdownValue2""
                        where task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                        GROUP BY url.""Code"", j.""Id"", o.""Id""
                        ";
                    isHR = true;
                }
                else
                {
                    query = @$"SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, batch.""OrganizationId"" as OrgId FROM public.""RecTask"" as task
                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
                join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId"" and ap.""IsDeleted""=false -- app id
                Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
                join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                where (task.""AssigneeUserId"" ='{userId}') and(task.""NtsType"" is null or task.""NtsType""=1)
                and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                GROUP BY url.""Code"",j.""Id"", batch.""OrganizationId""
                union
                SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, o.""Id"" as OrgId 
                FROM public.""RecTask"" as task
                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode"" and tmp.""TemplateCode""='JOBDESCRIPTION_HM'
                left join cms.""Job"" as j on j.""Id"" = task.""DropdownValue1""
                left join cms.""Organization"" as o on o.""Id"" = task.""DropdownValue2""
                where task.""AssigneeUserId"" ='{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                GROUP BY url.""Code"", j.""Id"", o.""Id""
                ";
                }
            }

            var result = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(query, null);
            if (result.IsNotNull())
            {

                if (orgId.IsNotNullAndNotEmpty())
                {
                    result = result.Where(x => x.OrgId == orgId).ToList();
                }

                var positions = result.Where(x => x.Position != "").Select(x => x.Position).Distinct().ToList();
                positions.Add("Count");
                var roleString = "";
                if (!isHR)
                {
                    roleString = result.Select(x => x.UserRole).FirstOrDefault();
                }
                else
                {
                    //var allRoles = await _userRoleBusiness.GetList();
                    //if (allRoles.IsNotNull())
                    //{
                    //    var r = allRoles.Select(x => x.Code);
                    //    roleString = String.Join(",", r);
                    //}

                    roleString = "ORG_UNIT,HRHEAD,ED,HR,HM,PRO,TICKETING,ADMIN,PLANT,CANDIDATE,AGENCY";
                }
                List<string> roles = new List<string>();
                if (roleString.IsNotNullAndNotEmpty())
                {
                    roles = roleString.Split(",").ToList();
                }

                foreach (var p in positions)
                {
                    var hrCount = 0;
                    var planningCount = 0;
                    var hMCount = 0;
                    var orgCount = 0;
                    var edCount = 0;
                    var candidateCount = 0;
                    var proCount = 0;
                    var tktCount = 0;
                    var adminCount = 0;
                    var plantCount = 0;
                    var agencyCount = 0;
                    var hrHeadCount = 0;
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    var count = list.Cast<System.Dynamic.ExpandoObject>()
                   .Where(x => x.Any(y => y.Value != null && y.Value.ToString().IndexOf(p) >= 0))
                   .Count();
                    if (count == 0)
                    {
                        ((IDictionary<String, Object>)exo).Add("Position", p);
                    }
                    var tasks = new List<RecTaskViewModel>();
                    if (p != "Count")
                    {
                        tasks = result.Where(x => x.Position == p).ToList();
                    }
                    else
                    {
                        tasks = result.Select(x => new RecTaskViewModel { UserRole = x.UserRole, Count = x.Count }).ToList();

                        tasks = tasks.GroupBy(x => x.UserRole).Select(x => new RecTaskViewModel { UserRole = x.Key, Count = x.Sum(y => y.Count) }).ToList();
                    }
                    foreach (var r in roles)
                    {
                        var tr = tasks.Where(x => x.UserRole == r).FirstOrDefault();

                        if (tr != null)
                        {
                            AddPropertyinExpndoObject(exo, r, tr.Count);
                        }
                        else
                        {
                            AddPropertyinExpndoObject(exo, r, 0);
                        }

                        var expandoDict = exo as IDictionary<string, object>;
                        if (expandoDict.ContainsKey("Position"))
                        {

                            list.Add(exo);
                        }

                    }

                }
                var resList = list.Distinct().ToList();
                return resList;
            }
            return list;
        }
        public static void AddPropertyinExpndoObject(System.Dynamic.ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        public async Task<RecruitmentDashboardViewModel> GetManpowerRecruitmentSummaryByOrgJob(string organizationId, string jobId, string permission = "")
        {
            string query = @$"SELECT c.""JobId"" as JobId,
                            sum(c.""Requirement""::int) as Requirement,
                            sum(c.""Separation""::int) as Seperation,
                            sum(c.""Available""::int) as Available,
                            --sum(c.""Planning""::int) as Planning,
                            sum(c.""Transfer""::int) as Transfer,
                            sum(c.""Balance""::int) as Balance
                            FROM cms.""N_REC_ManpowerRequirement"" as c
                            JOIN cms.""N_CoreHR_HRJob"" as ja ON ja.""Id""=c.""JobId""
                            #WHERE#                            
                            ";
            var where = @$" WHERE c.""JobId""='{jobId}' and c.""IsDeleted""=false group by c.""JobId""";

            if (organizationId.IsNotNullAndNotEmpty())
            {
                where = @$" WHERE c.""JobId""='{jobId}' AND c.""OrganizationId""='{organizationId}'  and c.""IsDeleted""=false group by c.""JobId""";
            }

            if (organizationId.IsNullOrEmpty() && jobId.IsNullOrEmpty())
            {
                query = @$"SELECT 
                            sum(c.""Requirement""::int) as Requirement,
                            sum(c.""Separation""::int) as Seperation,
                            sum(c.""Available""::int) as Available,
                            --sum(c.""Planning""::int) as Planning,
                            sum(c.""Transfer""::int) as Transfer,
                            sum(c.""Balance""::int) as Balance
                            FROM cms.""N_REC_ManpowerRequirement"" as c
                            --JOIN cms.""N_CoreHR_HRJob"" as ja ON ja.""Id""=c.""JobId""
                            where c.""IsDeleted""=false                     
                            ";
                where = "";
            }
            query = query.Replace("#WHERE#", where);
            var list1 = await _queryRepo.ExecuteQueryList<ManpowerRecruitmentSummaryViewModel>(query, null);

            string query1 = @$"SELECT ja.""Id"" as Id,ja.""Id"" as JobId,                            
                            sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
                            sum(case when ja.""Status"" = 2 then 1 else 0 end) as InActive,
                            (sum(case when aps.""Code""='UnReviewed' then 1 else 0 end)) as UnReviewed,
                            (sum(case when aps.""Code""='ShortListByHr' then 1 else 0 end)) as ShortlistedByHr,
                            (sum(case when aps.""Code"" = 'ShortListByHm' then 1 else 0 end)) as ShortlistedForInterview,
                            (sum(case when aps.""Code"" = 'InterviewsCompleted' then 1 else 0 end)) as InterviewCompleted,
                            (sum(case when aps.""Code"" = 'InterviewsCompleted' and apst.""Code"" = 'WAITLISTED' then 1 else 0 end)) as WaitlistByHM,
                            (sum(case when aps.""Code"" = 'IntentToOffer' then 1 else 0 end)) as IntentToOfferSent,
                            (sum(case when aps.""Code"" = 'FinalOffer' then 1 else 0 end)) as FinalOfferSent,
                            (sum(case when aps.""Code"" = 'FinalOfferAccepted' then 1 else 0 end)) as FinalOfferAccepted,
                            (sum(case when aps.""Code"" = 'VisaTransfer' then 1 else 0 end)) as VisaTransfer,
                            (sum(case when aps.""Code"" = 'BusinessVisa' then 1 else 0 end)) as BusinessVisa,
                            (sum(case when aps.""Code"" = 'VisaAppointmentTaken' then 1 else 0 end)) as VisaAppointmentTaken,
                            (sum(case when aps.""Code"" = 'BiometricCompleted' then 1 else 0 end)) as BiometricCompleted,
                            (sum(case when aps.""Code"" = 'VisaApproved' then 1 else 0 end)) as VisaApproved,
                            (sum(case when aps.""Code"" = 'WorkVisa' then 1 else 0 end)) as WorkVisa,
                            (sum(case when aps.""Code"" = 'FlightTicketsBooked' then 1 else 0 end)) as FightTicketsBooked,
                            (sum(case when aps.""Code"" = 'CandidateArrived' then 1 else 0 end)) as CandidateArrived,
                            (sum(case when aps.""Code"" = 'StaffJoined' then 1 else 0 end)) as CandidateJoined  , 
                            (sum(case when aps.""Code"" = 'WorkerJoined' then 1 else 0 end)) as WorkerJoined ,
                            (sum(case when aps.""Code"" = 'WorkerPool' then 1 else 0 end)) as WorkerPool ,
                            (sum(case when aps.""Code"" = 'WorkPermit' then 1 else 0 end)) as WorkPermit ,
                            (sum(case when aps.""Code"" = 'PostWorkerJoined' then 1 else 0 end)) as PostWorkerJoined ,
                            (sum(case when aps.""Code"" = 'PostStaffJoined' then 1 else 0 end)) as PostStaffJoined ,
                            (sum(case when (aps.""Code"" = 'PostWorkerJoined' or aps.""Code"" = 'PostStaffJoined' or aps.""Code"" = 'Joined') then 1 else 0 end)) as Joined ,
                             count(ap.""Id"") as NoOfApplication 
                            FROM cms.""N_CoreHR_HRJob"" as ja 
                            join cms.""N_REC_APPLICATION"" as ap on ap.""JobId"" =ja.""Id"" and ap.""IsDeleted""=false
                            LEFT join public.""LOV"" as aps on aps.""Id"" = ap.""ApplicationStateId""
                            LEFT join public.""LOV"" as apst on apst.""Id"" = ap.""ApplicationStatusId""
                            LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=ap.""BatchId""
                            LEFT JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = batch.""BatchStatus"" 
                            WHERE ap.""JobId""='{jobId}' and ja.""Id""='{jobId}' and apst.""Code""!='REJECTED' and apst.""Code""!='RejectedHM' and apst.""Code""!='WAITLISTED' 
                            #WHERE#
                             group by ja.""Id""
                            ";
            var where1 = "";
            //var where1 = @$" WHERE batch.""JobId""='{jobId}' and ja.""Id""='{jobId}' group by ja.""Id""";
            //  var where1 = @$" WHERE ap.""JobId""='{jobId}' and ja.""Id""='{jobId}' and apst.""Code""!='REJECTED' and apst.""Code""!='RejectedHM' and apst.""Code""!='WAITLISTED' group by ja.""Id""";

            if (organizationId.IsNotNullAndNotEmpty())
            {
                //where1 = @$" WHERE batch.""JobId""='{jobId}' and ja.""Id""='{jobId}' AND batch.""OrganizationId""='{organizationId}' group by ja.""Id""";
                //where1 = @$" WHERE ap.""JobId""='{jobId}' and ja.""Id""='{jobId}' AND ap.""OrganizationId""='{organizationId}' group by ja.""Id""";
                where1 = @$" AND batch.""OrganizationId""='{organizationId}'";
            }
            if (organizationId.IsNullOrEmpty() && jobId.IsNullOrEmpty())
            {
                query1 = @$"SELECT 
                            (sum(case when aps.""Code""='UnReviewed' then 1 else 0 end)) as UnReviewed,
                            (sum(case when aps.""Code""='ShortListByHr' then 1 else 0 end)) as ShortlistedByHr,
                            (sum(case when aps.""Code"" = 'ShortListByHm' then 1 else 0 end)) as ShortlistedForInterview,
                            (sum(case when aps.""Code"" = 'InterviewsCompleted' then 1 else 0 end)) as InterviewCompleted,
                            (sum(case when aps.""Code"" = 'InterviewsCompleted' and apst.""Code"" = 'WAITLISTED' then 1 else 0 end)) as WaitlistByHM,
                            (sum(case when aps.""Code"" = 'IntentToOffer' then 1 else 0 end)) as IntentToOfferSent,
                            (sum(case when aps.""Code"" = 'FinalOffer' then 1 else 0 end)) as FinalOfferSent,
                            (sum(case when aps.""Code"" = 'FinalOfferAccepted' then 1 else 0 end)) as FinalOfferAccepted,
                            (sum(case when aps.""Code"" = 'VisaTransfer' then 1 else 0 end)) as VisaTransfer,
                            (sum(case when aps.""Code"" = 'BusinessVisa' then 1 else 0 end)) as BusinessVisa,
                            (sum(case when aps.""Code"" = 'VisaAppointmentTaken' then 1 else 0 end)) as VisaAppointmentTaken,
                            (sum(case when aps.""Code"" = 'BiometricCompleted' then 1 else 0 end)) as BiometricCompleted,
                            (sum(case when aps.""Code"" = 'VisaApproved' then 1 else 0 end)) as VisaApproved,
                            (sum(case when aps.""Code"" = 'WorkVisa' then 1 else 0 end)) as WorkVisa,
                            (sum(case when aps.""Code"" = 'FlightTicketsBooked' then 1 else 0 end)) as FightTicketsBooked,
                            (sum(case when aps.""Code"" = 'CandidateArrived' then 1 else 0 end)) as CandidateArrived,
                            (sum(case when aps.""Code"" = 'StaffJoined' then 1 else 0 end)) as CandidateJoined  , 
                            (sum(case when aps.""Code"" = 'WorkerJoined' then 1 else 0 end)) as WorkerJoined ,
                            (sum(case when aps.""Code"" = 'WorkerPool' then 1 else 0 end)) as WorkerPool ,
                            (sum(case when aps.""Code"" = 'WorkPermit' then 1 else 0 end)) as WorkPermit ,
                            (sum(case when aps.""Code"" = 'PostWorkerJoined' then 1 else 0 end)) as PostWorkerJoined ,
                            (sum(case when aps.""Code"" = 'PostStaffJoined' then 1 else 0 end)) as PostStaffJoined ,
                             (sum(case when (aps.""Code"" = 'PostWorkerJoined' or aps.""Code"" = 'PostStaffJoined' or aps.""Code"" = 'Joined') then 1 else 0 end)) as Joined ,
                             count(ap.""Id"") as NoOfApplication 
                            FROM cms.""N_CoreHR_HRJob"" as ja 
                            join cms.""N_REC_APPLICATION"" as ap on ap.""JobId"" =ja.""Id"" and ap.""IsDeleted""=false
                            LEFT join public.""LOV"" as aps on aps.""Id"" = ap.""ApplicationStateId""
                            LEFT join public.""LOV"" as apst on apst.""Id"" = ap.""ApplicationStatusId""
                            LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=ap.""BatchId""
                            where apst.""Code""!='REJECTED' and apst.""Code""!='RejectedHM' and apst.""Code""!='WAITLISTED'
                             #WHERE#
                            ";
                where1 = "";


            }
            var wherehire = "";
            if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT") && !permission.Contains("ViewManpowerDashboard"))
            {
                var orglist = await GetHODOrg(_repo.UserContext.UserId);
                //var orgs = orglist.Select(x => x.Id);
                //var orgId = string.Join(",", orgs).TrimEnd(',');
                var orgId = orglist.Id.Replace(",", "','");
                where1 = @$" and batch.""OrganizationId"" in ('{orgId}') ";
                wherehire = @$" and o.""Id"" in ('{orgId}') ";
            }
            else if (_repo.UserContext.UserRoleCodes.Contains("HM") && !permission.Contains("ViewManpowerDashboard"))
            {
                var orglist = await GetHmOrg(_repo.UserContext.UserId);
                //var orgs = orglist.Select(x => x.Id);
                //var orgId = string.Join(",", orgs).TrimEnd(',');
                var orgId = orglist.Id.Replace(",", "','");
                where1 = @$" and batch.""OrganizationId"" in ('{orgId}') ";
                wherehire = @$" and o.""Id"" in ('{orgId}') ";
            }
            query1 = query1.Replace("#WHERE#", where1);
            var list2 = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query1, null);

            string query3 = @$"SELECT count(ap.""Id"") as NoOfApplication
                            FROM cms.""N_CoreHR_HRJob"" as ja 
                            join cms.""N_REC_APPLICATION"" as ap on ap.""JobId"" =ja.""Id"" and ap.""IsDeleted""=false
                            LEFT join public.""LOV"" as aps on aps.""Id"" = ap.""ApplicationStateId""
                            LEFT join public.""LOV"" as apst on apst.""Id"" = ap.""ApplicationStatusId""
                            LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=ap.""BatchId""
                            #WHERE#
                            ";
            var where3 = "";
            if (jobId.IsNotNullAndNotEmpty())
            {
                where3 = @$"WHERE ap.""JobId"" = '{jobId}' and ja.""Id"" = '{jobId}' group by ja.""Id""";
            }

            if (organizationId.IsNotNullAndNotEmpty())
            {
                where3 = @$" WHERE ap.""JobId""='{jobId}' and ja.""Id""='{jobId}' AND batch.""OrganizationId""='{organizationId}' group by ja.""Id""";
            }
            query3 = query3.Replace("#WHERE#", where3);
            var list3 = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query3, null);

            var DirecthireQuery = $@" select count(task.""Id"") as NoOfApplication 
                    FROM public.""NtsTask"" as task  
                    inner join public.""TaskTemplate"" as tmp on tmp.""TemplateId"" = task.""TemplateId"" 
                    left join cms.""N_REC_APPLICATION"" as app on app.""Id"" = task.""ReferenceId"" and app.""IsDeleted""=false
                    left join cms.""N_CoreHR_HRJob"" as j on  j.""Id"" = app.""JobId""
                    left join cms.""N_REC_REC_BATCH"" as bt on bt.""Id"" = app.""BatchId""
                    left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id"" = bt.""OrganizationId""
                  where task.""TemplateCode""='DIRECT_HIRING' and task.""IsDeleted""=false #WHERE#";

            if (jobId.IsNotNullAndNotEmpty())
            {
                wherehire = @$"and j.""Id"" = '{jobId}'";
            }
            if (organizationId.IsNotNullAndNotEmpty())
            {
                wherehire = @$" and j.""Id""='{jobId}' AND o.""Id""='{organizationId}'";
            }
            DirecthireQuery = DirecthireQuery.Replace("#WHERE#", wherehire);
            var listhire = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(DirecthireQuery, null);

            var a = list1.FirstOrDefault();
            if (a == null)
            {
                a = new ManpowerRecruitmentSummaryViewModel();
                a.Requirement = 0;
                a.Seperation = 0;
                a.Available = 0;
                a.Planning = 0;
                a.Transfer = 0;
                a.Balance = 0;
                a.OrganizationId = organizationId;
            }
            var b = list2.FirstOrDefault();
            if (b == null)
            {
                b = new JobAdvertisementViewModel();
                b.UnReviewed = 0;
                b.ShortlistedByHr = 0;
                b.ShortlistedForInterview = 0;
                b.WaitlistByHM = 0;
                b.InterviewCompleted = 0;
                b.IntentToOfferSent = 0;
                b.FinalOfferSent = 0;
                b.FinalOfferAccepted = 0;
                b.VisaTransfer = 0;
                b.MedicalCompleted = 0;
                b.VisaAppointmentTaken = 0;
                b.BiometricCompleted = 0;
                b.VisaApproved = 0;
                b.VisaSentToCandidates = 0;
                b.FightTicketsBooked = 0;
                b.CandidateArrived = 0;
                b.CandidateJoined = 0;
                b.WorkerJoined = 0;
                b.BusinessVisa = 0;
                b.WorkVisa = 0;
                b.WorkerPool = 0;
                b.WorkPermit = 0;
                b.Joined = 0;
                b.PostStaffJoined = 0;
                b.PostWorkerJoined = 0;
                b.NoOfApplication = 0;
            }
            var c = list3.FirstOrDefault();
            if (c == null)
            {
                c = new JobAdvertisementViewModel();
                c.NoOfApplication = 0;
            }
            var h = listhire.FirstOrDefault();
            var data = new RecruitmentDashboardViewModel
            {
                DirectHiring = h.IsNotNull() ? h.NoOfApplication : 0,
                Requirement = a.Requirement,
                Seperation = a.Seperation,
                Available = a.Available,
                Planning = a.Planning,
                Transfer = a.Transfer,
                Balance = a.Balance,
                OrganizationId = a.OrganizationId,
                NoOfApplication = c.NoOfApplication,
                //NoOfApplication = (c.UnReviewed + b.ShortlistedByHr + b.ShortlistedForInterview + b.InterviewCompleted +
                //           b.IntentToOfferSent + b.FinalOfferSent + b.FinalOfferAccepted + b.VisaTransfer + b.MedicalCompleted +
                //           b.VisaAppointmentTaken + b.BiometricCompleted + b.VisaApproved + b.VisaSentToCandidates +
                //           +b.FightTicketsBooked + b.CandidateArrived + b.CandidateJoined),
                UnReviewed = b.UnReviewed,
                ShortlistedByHr = b.ShortlistedByHr,
                ShortlistedForInterview = b.ShortlistedForInterview,
                InterviewCompleted = b.InterviewCompleted,
                WaitlistByHM = b.WaitlistByHM,
                IntentToOfferSent = b.IntentToOfferSent,
                IntentToOfferAccepted = b.FinalOfferSent,
                FinalOfferAccepted = b.FinalOfferAccepted,
                VisaTransfer = b.VisaTransfer,
                MedicalCompleted = b.MedicalCompleted,
                VisaAppointmentTaken = b.VisaAppointmentTaken,
                BiometricCompleted = b.BiometricCompleted,
                VisaApproved = b.VisaApproved,
                VisaSentToCandidates = b.VisaSentToCandidates,
                FightTicketsBooked = b.FightTicketsBooked,
                CandidateArrived = b.CandidateArrived,
                CandidateJoined = b.CandidateJoined,
                WorkerJoined = b.WorkerJoined,
                WorkVisa = b.WorkVisa,
                BusinessVisa = b.BusinessVisa,
                WorkPermit = b.WorkPermit,
                WorkerPool = b.WorkerPool,
                Joined = b.Joined,
                PostWorkerJoined = b.PostWorkerJoined,
                PostStaffJoined = b.PostStaffJoined,
            };
            return data;
        }

        public async Task<IList<ApplicationViewModel>> GetApplicationPendingTask(string userId)
        {
            try
            {
                var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "REC" && x.TemplateType == TemplateTypeEnum.Service);
                var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);
                var selectQry = "";
                var i = 1;
                foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);
                    if (item.Code == "REC_JOB_ADVERTISEMENT" || item.Code == "REC_MANPOWER_REQUIREMENT")
                    {

                    }
                    else
                    { 
                        if (i != 1)
                        {
                            selectQry += " union ";
                        }
                        selectQry = @$" {selectQry}
                                  SELECT task.""Id"" as Id,udf.""ApplicationId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo, task.""TaskSubject""  as Subject,  au.""Name"" as AssigneeUserName, ts.""Name"" as TaskStatusName,task.""CreatedDate""  as ""CreatedDate"" 
                            , task.""TemplateCode"" as TemplateCode
                            FROM public.""NtsService"" as s  
                            join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false                        
                            join public.""NtsTask"" as task on task.""ParentServiceId"" = s.""Id"" and task.""IsDeleted""=false
                            join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' or ts.""Code""='TASK_STATUS_OVERDUE')                        
                            join public.""User"" as au on au.""Id"" = task.""AssignedToUserId"" and au.""IsDeleted""=false and au.""Id""='{userId}'                            
                            join cms.""{tableMeta.Name}"" as udf on s.""UdfNoteTableId""=udf.""Id"" and udf.""IsDeleted""=false
                            where s.""IsDeleted""=false 
                            ";
                        i++;
                    }
                }

                var taskData = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(selectQry, null);
                var appIds = "";

                if (taskData.IsNotNull())
                {
                    var appid = "";
                    var appidlist = taskData.Select(x => x.ReferenceTypeId).ToList();
                    foreach (var app in appidlist)
                    {
                        appid += $"'{app}',";
                    }
                    if (appid.IsNotNull())
                    {
                        appIds = appid.Trim(',');
                    }
                }
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateId"" as CandidateProfileId, 
                app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
                app.""LastName"" as LastName, app.""Age"" as Age,
                app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
                n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
                app.""QatarId"" as QatarId,
                lov.""Name"" as Gender, mar.""Name"" as MaritalStatusName,
                app.""PassportNumber"" as PassportNumber, pic.""CountryName"" as PassportIssueCountry,
                app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
                app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
                app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
                app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
                app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
                app.""TotalWorkExperience"" as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
                app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, batch.""BatchName"" as BatchName,app.""OrganisationId"" as OrganizationId, org.""DepartmentName"" as OrganizationName, 
                app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,job.""JobTitle"" as JobName,app.""GaecNo"" as GaecNo,
                appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
                app.""Score"" as Score,
                --app.""IsLocalCandidate"" as IsLocalCandidate,
                app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
                app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,
                c.""UserId"" as ApplicationUserId,
                app.""ApplicationStateId"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
                app.""ApplicationNo"" as ApplicationNo
                --,
                --task.""Id"" as TaskId,task.""TaskNo"" as TaskNo,task.""Subject"" as TaskSubject
                FROM cms.""N_REC_APPLICATION"" as app
                left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
                left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
                left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
                left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
                left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
                left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
                left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
                left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
                left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""CurrentStatusId""
                left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId"" 
                left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
                left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategoryId""
                LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
                left JOIN public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 
                left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=app.""OrganisationId"" 
                left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
                --left join public.""RecTask"" as service ON service.""ReferenceTypeId""=app.""Id"" AND service.""NtsType""=2
                --left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = service.""Id"" AND (task.""TaskStatusCode""='INPROGRESS' OR task.""TaskStatusCode""='OVERDUE')
                --left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                where app.""IsDeleted""=false
                and app.""Id"" IN ({appIds})
                --#WHERE# 
                ";

                var where = $@" and au.""Id""='{userId}' ";
                //if (jobId.IsNotNullAndNotEmpty() && orgId.IsNotNullAndNotEmpty())
                //{
                //    where = @$" where app.""JobId""='{jobId}' AND app.""OrganizationId""='{orgId}' ";
                //}
                //else if (orgId.IsNotNullAndNotEmpty())
                //{
                //    where = @$" where app.""OrganizationId""='{orgId}' ";
                //}
                //else if (jobId.IsNotNullAndNotEmpty())
                //{
                //    where = @$" where app.""JobId""='{jobId}' ";
                //}
                query = query.Replace("#WHERE#", where);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                if (allList.IsNotNull())
                {
                    foreach (var item in allList)
                    {
                        var tdata = taskData.Where(x => x.ReferenceTypeId == item.ApplicationId).FirstOrDefault();
                        if (tdata.IsNotNull())
                        {
                            item.TaskId = tdata.Id;
                            item.TaskNo = tdata.TaskNo;
                            item.TaskSubject = tdata.Subject;
                        }
                    }
                }
                return allList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<IList<ApplicationViewModel>> GetApplicationWorkerPoolNotUnderApproval()
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
                app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
                app.""LastName"" as LastName, app.""Age"" as Age,
                app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
                n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
                app.""QatarId"" as QatarId,
                lov.""Name"" as Gender, mar.""Name"" as MaritalStatusName,
                app.""PassportNumber"" as PassportNumber, pic.""Name"" as PassportIssueCountry,
                app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
                app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
                app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
                app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
                app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
                app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
                app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, batch.""Name"" as BatchName, org.""Name"" as OrganizationName, 
                app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,job.""Name"" as JobName,app.""GaecNo"" as GaecNo,
                appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
                app.""Score"" as Score, app.""IsLocalCandidate"" as IsLocalCandidate,
                app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
                app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,
                c.""UserId"" as ApplicationUserId,
                app.""ApplicationState"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
                app.""ApplicationNo"" as ApplicationNo,
                '' as TaskId,'' as TaskNo,'' as TaskSubject
                FROM rec.""Application"" as app
                left join cms.""Job"" as job on job.""Id"" = app.""JobId""
                left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
                left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
                left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
                left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
                left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
                left join cms.""Currency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
                left join rec.""ListOfValue"" as mar on mar.""Id"" = app.""MaritalStatus""
                left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
                left join rec.""CandidateProfile"" as c ON c.""Id""=app.""CandidateProfileId"" 
                left join rec.""ListOfValue"" as lov on lov.""Id""=app.""Gender""
                left join rec.""ListOfValue"" as vt on vt.""Id""=app.""VisaCategory""
                LEFT Join rec.""Batch"" as batch on batch.""Id""=app.""BatchId""
                left JOIN rec.""ListOfValue"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 
                left join cms.""Organization"" as org ON org.""Id""=app.""OrganizationId"" 
                left join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState""
                where app.""IsDeleted""=false
                #WHERE# ";

                var where = $@" and apps.""Code""='WorkerPool' AND  appStatus.""Code""<>'UnderApproval' ";
                //if (jobId.IsNotNullAndNotEmpty() && orgId.IsNotNullAndNotEmpty())
                //{
                //    where = @$" where app.""JobId""='{jobId}' AND app.""OrganizationId""='{orgId}' ";
                //}
                //else if (orgId.IsNotNullAndNotEmpty())
                //{
                //    where = @$" where app.""OrganizationId""='{orgId}' ";
                //}
                //else if (jobId.IsNotNullAndNotEmpty())
                //{
                //    where = @$" where app.""JobId""='{jobId}' ";
                //}
                query = query.Replace("#WHERE#", where);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                return allList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<IdNameViewModel> GetJobNameById(string jobId)
        {
            var query = "";
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""Id"",""JobTitle"" as Name FROM cms.""N_CoreHR_HRJob"" where ""Id""='{jobId}' and ""IsDeleted""=false and ""Status""=1";
                name = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }
        public async Task<List<IdNameViewModel>> GetJobIdNameDataList()
        {
            var list = new List<IdNameViewModel>();
            try
            {
                var query = @$"SELECT ""Id"",""JobTitle"" as Name FROM cms.""N_CoreHR_HRJob"" where ""IsDeleted""=false and ""Status""=1";
                list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            }
            catch (Exception)
            {

            }
            return list;
        }
        public async Task<IdNameViewModel> GetApplicationStateByCode(string stateCode)
        {
            var query = $@"SELECT ""Id"",""Name"",""Code"" rec.""ApplicationState"" where ""Code""='{stateCode}' and ""IsDeleted""=false ";
            var queryData = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return queryData;
        }
        #endregion d

        #region NH
        public async Task<IList<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    id = obj.id;
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    stageName = obj.StageName;
                    stageId = obj.StageId;
                    batchId = obj.BatchId;
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
                var udfs = await _noteBusiness.GetUdfQuery(null, "REC", null, null, "ApplicationId,ApplicationId");
               
                query = $@" select  COALESCE(t.""Count"",0) + COALESCE(hmaps.""Count"",0)  as ""Count""
	                FROM public.""User"" as ur
                    left join(
                    select 'USER' as ""Code"",count(task.""Id"") as ""Count""
                    FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""NtsTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""NtsService"" as s on  task.""ParentServiceId"" = s.""Id""
	                join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
 left join (" + udfs + $@") as udf on udf.""NtsNoteId""=s.""UdfNoteId"" 
	                join cms.""N_REC_APPLICATION"" as app on app.""Id"" = udf.""ApplicationId"" and app.""IsDeleted""=false
	                join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId""
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
where  task.""AssignedToUserId"" = '{userId}'  and ts.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
)t on 'USER'=t.""Code""
                   
                    left join(
	                 select 'HM' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM cms.""N_REC_REC_BATCH"" as b               
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested','ShortlistForFuture')
                    ) hmaps on 'HM'=hmaps.""Code""
	                where ur.""Id"" in ('{userId}') 
                    ";
                var count = await _queryRepo.ExecuteScalar<long?>(query, null);


                //var jdQry = $@"select count(distinct task.""Id"") as ""Count""
                //    from public.""NtsTask"" as task
                //    where (task.""TemplateCode""='JOBDESCRIPTION_HM' or task.""TemplateCode"" ='TASK_DIRECT_HIRING' or ""TemplateCode""='REVISING_INTENT_TO_OFFER_HOD') and task.""AssigneeUserId"" = '{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE') and    task.""IsDeleted"" = false";


                //var countjd = await _queryRepo.ExecuteScalar<long?>(jdQry, null);
                long? counthr = 0;

                var roles1 = new List<string>();
                var userRoles = userroleCode.Split(",");
                if (userRoles.IsNotNull())
                {
                    roles1 = userRoles.Select(x => x).Where(x => x == "HR").ToList();
                }
                //userroleCode.Contains("HR")

                if (roles1.IsNotNull() && roles1.Count() > 0)
                {
                    var hrQry = $@"select  count(distinct app.""Id"") as ""Count""
                        FROM cms.""N_REC_REC_BATCH"" as b
                        JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                        join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                        join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                        join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                        where l.""Code"" in('PendingWithHM', 'Draft', 'Close')";

                    counthr = await _queryRepo.ExecuteScalar<long?>(hrQry, null);
                }


                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX",
                    children = true,                    
                    parent = "#",
                    
                };
                if (count != null)
                {
                    item.Name = item.DisplayName = $"Inbox ({count /*+ countjd*/ + counthr})";
                    item.text = item.DisplayName = $"Inbox ({count /*+ countjd*/ + counthr})";
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
                query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || COALESCE(t.""Count"",0)+COALESCE(hraps.""Count"",0)+COALESCE(hmaps.""Count"",0)|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,'INBOX' as Parent,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
                left join(
	                select ur.""Id"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""NtsTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""NtsService"" as s on  task.""ParentServiceId"" = s.""Id""
	                join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
	                join cms.""N_REC_APPLICATION"" as app on app.""Id"" = s.""ReferenceId"" and app.""IsDeleted""=false
	                join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId""
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                     join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
	                where  task.""AssignedToUserId"" = '{userId}'  and ts.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
	                group by ur.""Id""   
                ) t on ur.""Id""=t.""Id""
                left join(
	                select     'HR' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM cms.""N_REC_REC_BATCH"" as b
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                    where l.""Code"" in('PendingWithHM', 'Draft', 'Close')
                ) hraps on ur.""Code""=hraps.""Code""
                left join(
	                 select 'HM' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM cms.""N_REC_REC_BATCH"" as b               
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested','ShortlistForFuture')
                ) hmaps on ur.""Code""=hmaps.""Code""
                left join(
	                select ur.""Id"",count(task.""Id"")  as ""Count""                    
	                from public.""NtsTask"" as task   
                    join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
                    join public.""UserRoleUser"" as uru on uru.""UserId"" = task.""AssignedToUserId""
                    join public.""UserRole"" as ur on ur.""Id"" = uru.""UserRoleId"" and ur.""Id"" in ({roleText})
                     join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
	                where ((task.""TemplateCode""='JOBDESCRIPTION_HM' and ur.""Code""='HM') or task.""TemplateCode"" ='TASK_DIRECT_HIRING' or ""TemplateCode""='REVISING_INTENT_TO_OFFER_HOD')  and task.""AssignedToUserId"" = '{userId}'  and ts.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and task.""IsDeleted""=false 
	                group by ur.""Id""  
                ) jd on ur.""Id""=jd.""Id""
                where ur.""Id"" in ({roleText})
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc
                 ";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                foreach(var listItem in list)
                {
                    listItem.children = listItem.hasChildren;
                    listItem.DisplayName = listItem.Name;
                    listItem.text = listItem.Name;
                }
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

                query = $@"Select usp.""InboxStageName"" ||' (' || COALESCE(t.""Count"",0)+COALESCE(hraps.""Count"",0)+COALESCE(hmaps.""Count"",0)|| ')' as Name
                , usp.""InboxStageName"" as id, '{id}' as ParentId,'{id}' as Parent, 'STAGE' as Type,
                true as hasChildren, '{userRoleId}' as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
                left join(
	                select usp.""InboxStageName"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""NtsTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""NtsService"" as s on  task.""ParentServiceId"" = s.""Id""
	                join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
	                join cms.""N_REC_APPLICATION"" as app on app.""Id"" = s.""ReferenceId"" and app.""IsDeleted""=false
	                join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId""
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join public.""LOV"" as aps on aps.""Id"" = app.""Id""
                     join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
	                where ur.""Id"" = '{userRoleId}' and  task.""AssignedToUserId"" = '{userId}'  and ts.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
	                group by usp.""InboxStageName""   
                ) t on usp.""InboxStageName""=t.""InboxStageName""
                left join(
	                select     'Shortlist by HR' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM cms.""N_REC_REC_BATCH"" as b
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" -- and l.""Code""='PendingWithHM'
                    join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                    where l.""Code"" in('PendingWithHM', 'Draft', 'Close')
                ) hraps on usp.""InboxStageName""=hraps.""Code""
                left join(
	                select 'Shortlist by HM' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM cms.""N_REC_REC_BATCH"" as b               
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested','ShortlistForFuture')
                ) hmaps on usp.""InboxStageName""=hmaps.""Code""
                 left join(
	              select usp.""InboxStageName"",count(task.""Id"")  as ""Count""
	               FROM public.""UserRole"" as ur
                   join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and (usp.""TemplateCode"" ='JOBDESCRIPTION_HM' or usp.""TemplateCode"" ='TASK_DIRECT_HIRING' or ""TemplateCode""='REVISING_INTENT_TO_OFFER_HOD') and usp.""InboxCode""='RECRUITMENT'
	               join public.""NtsTask"" as task on task.""TemplateCode"" =usp.""TemplateCode"" 
                    join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
	                where ur.""Id"" = '{userRoleId}' and  task.""AssignedToUserId"" = '{userId}'  and ts.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false 
	                group by usp.""InboxStageName""   
                ) jd on usp.""InboxStageName""=jd.""InboxStageName""
                where ur.""Id"" = '{userRoleId}'
                Group By hmaps.""Count"",hraps.""Count"",t.""Count"",jd.""Count"",usp.""InboxStageName"", usp.""StageSequence"", usp.""InboxStageName""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                foreach (var listItem in list)
                {
                    listItem.children = listItem.hasChildren;
                    listItem.DisplayName = listItem.Name;
                    listItem.text = listItem.Name;
                }

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
                query = $@"Select  case when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotSelected' or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='Rejected' then usp.""TemplateShortName"" 
                else usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')' end  as Name,
                coalesce(usp.""TemplateCode"", usp.""TemplateName"") as id, '{id}' as ParentId, '{id}' as Parent,
                case when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='OpenBatches' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='DraftBatches'
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='ClosedBatches' then 'ShortlistByHR'
                when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotShortlisted' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='ShortlistedByHM' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='InterviewRequested' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='ShortlistForFuture' then 'ShortlistByHM'
                when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotSelected' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='Rejected' then 'NotSelected'
                else 'TEMPLATE' end as Type,'{userRoleId}' as UserRoleId,
                case when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotSelected' or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='Rejected' then false else true end as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
                left join(
	                select tmp.""Code"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""NtsTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""NtsService"" as s on  task.""ParentServiceId"" = s.""Id""
	                join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
	                join cms.""N_REC_APPLICATION"" as app on app.""Id"" = s.""ReferenceId"" and app.""IsDeleted""=false
	                join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId""
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
	                where ur.""Id"" = '{userRoleId}' and  task.""AssignedToUserId"" = '{userId}' and usp.""InboxStageName"" = '{id}'  and ts.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
	                group by tmp.""Code""   
                    union
                    select    case when  l.""Code""='PendingWithHM' then 'OpenBatches'
                    when l.""Code"" = 'Draft' then 'DraftBatches'
                    when l.""Code"" = 'Close' then 'ClosedBatches' end as ""TemplateCode"",count(distinct app.""Id"") as ""Count""
                    FROM cms.""N_REC_REC_BATCH"" as b
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                    where l.""Code"" in('PendingWithHM', 'Draft', 'Close')
                    GROUP BY l.""Code"", b.""SequenceOrder""
                    union
                    select    case when  apst.""Code""='NotShortlisted' then 'NotShortlisted'
                    when apst.""Code"" = 'ShortlistedHM' then 'ShortlistedByHM'
                    when apst.""Code"" = 'InterviewRequested' then 'InterviewRequested' 
                    when apst.""Code"" = 'ShortlistForFuture' then 'ShortlistForFuture' end as ""TemplateCode"",count(distinct app.""Id"") as ""Count""
                    FROM cms.""N_REC_REC_BATCH"" as b               
                    JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested','ShortlistForFuture')
                    GROUP BY apst.""Code""
                    union
                    select usp.""TemplateCode"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and (usp.""TemplateCode"" ='JOBDESCRIPTION_HM' or usp.""TemplateCode"" ='TASK_DIRECT_HIRING' or ""TemplateCode""='REVISING_INTENT_TO_OFFER_HOD') and usp.""InboxCode""='RECRUITMENT'
	                join public.""NtsTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                     join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
	                where ur.""Id"" = '{userRoleId}' and  task.""AssignedToUserId"" = '{userId}' and usp.""InboxStageName"" = '{id}'  and ts.""Code"" in ('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false 
	                group by usp.""TemplateCode"" 
                 
                ) t on coalesce(usp.""TemplateCode"", usp.""TemplateName"")=t.""Code""

                where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}'
                Group By t.""Count"",usp.""TemplateShortName"", usp.""TemplateCode"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                foreach (var listItem in list)
                {
                    listItem.children = listItem.hasChildren;
                    listItem.DisplayName = listItem.Name;
                    listItem.text = listItem.Name;
                }


                var obj = expObj.Where(x => x.Type == "TEMPLATE" || x.Type == "ShortlistByHM" || x.Type == "ShortlistByHR" || x.Type == "NotSelected" || x.Type == "Rejected").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "TEMPLATE")
            {
                var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "REC" && x.TemplateType == TemplateTypeEnum.Service);
                var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);
                templateList = templateList.Where(x => x.Code != "REC_JOB_ADVERTISEMENT" && x.Code != "REC_MANPOWER_REQUIREMENT").ToList();
               // var selectQry = "";
                var i = 1;
                foreach (var item in templateList)
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                    if (i != 1)
                    {
                        query += " union ";
                    }

                    query = $@" {query} select  b.""Id"" as id, '{id}' as Parent,
                b.""BatchName"" ||' (' || Count( distinct task.""Id"") || ')' as Name,
                true as hasChildren,b.""Id"" as BatchId,
                '{id}' as ParentId,'BATCH' as Type,sp.""Id"" as StageId
                FROM public.""NtsService"" as s
                join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id""
                join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
                join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
join  cms.""{tableMeta.Name}"" as  RT on  s.""UdfNoteId"" =RT.""NtsNoteId"" and RT.""IsDeleted""=false and RT.""CompanyId""='{_userContext.CompanyId}'
                join cms.""N_REC_APPLICATION"" as app on app.""Id"" = RT.""ApplicationId"" and app.""IsDeleted""=false
                join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId""
                JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
                join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
                join public.""UserRoleStageParent"" as sp on sp.""TemplateCode"" = tmp.""Code"" and sp.""UserRoleId"" = '{userRoleId}' and sp.""InboxCode""='RECRUITMENT'
                Where tmp.""Code""='{id}' and ts.""Code"" in('TASK_STATUS_INPROGRESS','TASK_STATUS_OVERDUE') 
                and app.""Id"" is not null and task.""AssignedToUserId"" = '{userId}' and sp.""InboxStageName"" = '{parentId}'
                GROUP BY b.""Id"", b.""BatchName"", b.""SequenceOrder"",sp.""Id"" ";
                    i++;
                }


                //query = $@"
                //order by b.""SequenceOrder"" asc ";

                if (id == "JOBDESCRIPTION_HM" || id == "TASK_DIRECT_HIRING" || id == "DIRECTHIRING_EVALUATIONFORM" || id == "REVISING_INTENT_TO_OFFER_HOD")
                {
                    query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                '{id}' as StageId,s.""StatusCode"" as StatusCode,'' as BatchId,
                false as hasChildren,'{id}' as Parent, '{id}' as ParentId, s.""StatusLabel"" as Id 
                FROM public.""UserRoleStageChild"" as s
                join public.""UserRoleStageParent"" as sp on sp.""Id"" = s.""InboxStageId"" and sp.""TemplateCode"" = '{id}' and sp.""UserRoleId"" = '{userRoleId}' and sp.""InboxCode""='RECRUITMENT'
                --left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                left join(
                    select case when ts.""Code""='TASK_STATUS_OVERDUE' then 'TASK_STATUS_INPROGRESS' else task.""TemplateCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                    FROM public.""NtsTask"" as task
                    join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
                    join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
                    join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
                  
                    Where tmp.""Code""='{id}' and task.""AssignedToUserId"" = '{userId}'
                    and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 
                    
                    group by TaskStatusCode  
                ) t on t.TaskStatusCode=ANY(s.""StatusCode"")
               
                order by s.""SequenceOrder"" asc";
                }

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                foreach (var listItem in list)
                {
                    listItem.children = listItem.hasChildren;
                    listItem.DisplayName = listItem.Name;
                    listItem.text = listItem.Name;
                }

                var obj = expObj.Where(x => x.Type == "BATCH").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "ShortlistByHR")
            {
                query = $@"select  b.""Id"" as id,
                b.""BatchName"" ||' (' || Count( distinct app.""Id"") || ')' as Name,
                false as hasChildren,b.""Id"" as BatchId,
                '{id}' as ParentId,'HRBATCH' as Type,'{id}' as Parent
                FROM cms.""N_REC_REC_BATCH"" as b               
                JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus""
                join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false             
                join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                #WHERE#
                GROUP BY b.""Id"", b.""BatchName"", b.""SequenceOrder""
                order by b.""SequenceOrder"" asc ";
                string where = "";
                if (id == "DraftBatches")
                {
                    where = $@" where l.""Code""='Draft'";
                }
                else if (id == "OpenBatches")
                {
                    where = $@" where l.""Code""='PendingWithHM'";
                }
                else if (id == "ClosedBatches")
                {
                    where = $@" where l.""Code""='Close'";
                }
                query = query.Replace("#WHERE#", where);
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                foreach (var listItem in list)
                {
                    listItem.children = listItem.hasChildren;
                    listItem.DisplayName = listItem.Name;
                    listItem.text = listItem.Name;
                }


                var obj = expObj.Where(x => x.Type == "HRBATCH").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }
            }
            else if (type == "ShortlistByHM")
            {
                query = $@"select  b.""Id"" as id,
                b.""BatchName"" ||' (' || Count( distinct app.""Id"") || ')' as Name,
                false as hasChildren,b.""Id"" as BatchId,
                '{id}' as ParentId,'HMBATCH' as Type, '{id}' as Parent
                FROM cms.""N_REC_REC_BATCH"" as b               
                JOIN public.""LOV"" as l ON l.""Id"" = b.""BatchStatus""
                join cms.""N_REC_APPLICATION"" as app on app.""BatchId"" = b.""Id"" and app.""IsDeleted""=false             
                join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                join public.""LOV"" as apst on apst.""Id"" = app.""ApplicationStatusId""
                where b.""HiringManager""='{userId}' and l.""Code""='PendingWithHM'
                #WHERE#
                GROUP BY b.""Id"", b.""BatchName"", b.""SequenceOrder""
                order by b.""SequenceOrder"" asc ";
                string where = "";
                if (id == "NotShortlisted")
                {
                    where = $@" and apst.""Code""='NotShortlisted'";
                }
                else if (id == "ShortlistedByHM")
                {
                    where = $@" and apst.""Code""='ShortlistedHM'";
                }
                else if (id == "InterviewRequested")
                {
                    where = $@" and apst.""Code""='InterviewRequested'";
                }
                else if (id == "ShortlistForFuture")
                {
                    where = $@" and apst.""Code""='ShortlistForFuture'";
                }
                query = query.Replace("#WHERE#", where);
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                foreach (var listItem in list)
                {
                    listItem.children = listItem.hasChildren;
                    listItem.DisplayName = listItem.Name;
                    listItem.text = listItem.Name;
                }


                var obj = expObj.Where(x => x.Type == "ShortlistByHM").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }
            }
            else if (type == "BATCH")
            {
                var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "REC" && x.TemplateType == TemplateTypeEnum.Service);
                var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);
                templateList = templateList.Where(x => x.Code != "REC_JOB_ADVERTISEMENT" && x.Code != "REC_MANPOWER_REQUIREMENT").ToList();
                // var selectQry = "";
                var i = 1;
                foreach (var item in templateList)
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                    if (i != 1)
                    {
                        query += " union ";
                    }

                    query = $@" {query} select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                '{batchId}' as BatchId,'{batchId}' as Parent,'{batchId}' as ParentId,'{parentId}' as StageId,s.""StatusCode"" as StatusCode,
                false as hasChildren,s.""Id"" as id
                FROM public.""UserRoleStageChild"" as s
                --left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                left join(
                    select case when ts.""Code""='TASK_STATUS_OVERDUE' then 'TASK_STATUS_INPROGRESS' else ts.""Code"" end as ""TaskStatusCode"",count(task.""Id"")  as ""Count""
                    FROM public.""NtsService"" as s
                    join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id""
                    join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
                    join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
join  cms.""{tableMeta.Name}"" as  RT on  s.""UdfNoteId"" =RT.""NtsNoteId"" and RT.""IsDeleted""=false and RT.""CompanyId""='{_userContext.CompanyId}'
                    join cms.""N_REC_APPLICATION"" as app on app.""Id"" = RT.""ApplicationId"" and app.""IsDeleted""=false
                    join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId""
                    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                    join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
                    Where tmp.""Code""='{parentId}' and b.""Id""='{batchId}' and task.""AssignedToUserId"" = '{userId}'
                    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false and
                    app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
                    group by ""TaskStatusCode""  
                ) t on t.""TaskStatusCode""=ANY(s.""StatusCode"")
                where s.""InboxStageId""='{stageId}' and s.""IsDeleted""=false ";
                    i++;
                }
                //query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                //'{batchId}' as BatchId,'{parentId}' as StageId,s.""StatusCode"" as StatusCode,
                //false as hasChildren
                //FROM public.""UserRoleStageChild"" as s
                //--left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                //left join(
                //    select case when ts.""Code""='TASK_STATUS_OVERDUE' then 'TASK_STATUS_INPROGRESS' else ts.""Code"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                //    FROM public.""NtsService"" as s
                //    join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id""
                //    join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
                //    join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
                //    join cms.""N_REC_APPLICATION"" as app on app.""Id"" = s.""ReferenceId"" and app.""IsDeleted""=false
                //    join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId""
                //    join public.""LOV"" as aps on aps.""Id"" = app.""ApplicationStateId""
                //    join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
                //    Where tmp.""TemplateCode""='{parentId}' and b.""Id""='{batchId}' and task.""AssignedToUserId"" = '{userId}'
                //    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false and
                //    app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
                //    group by TaskStatusCode  
                //) t on t.TaskStatusCode=ANY(s.""StatusCode"")
                //where s.""InboxStageId""='{stageId}' and s.""IsDeleted""=false
                //order by s.""SequenceOrder"" asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                list = list.DistinctBy(x => x.id).ToList();
                foreach (var listItem in list)
                {
                    listItem.children = listItem.hasChildren;
                    listItem.DisplayName = listItem.Name;
                    listItem.text = listItem.Name;
                }
                //var obj = expObj.Where(x => x.Name == type);
                //if (obj.Any())
                //{
                //    list.Find(x => x.id == obj.FirstOrDefault().Id).expanded = true;
                //}

                //var pending = new TreeViewViewModel
                //{
                //    id = "INPROGRESS",
                //    Type = "STATUS",
                //    ParentId = id,
                //    hasChildren = false,
                //    DisplayName = "Pending (0)",
                //    Name = "Pending (0)"
                //};

                //long pendingCount = 0;

                //var pendingItem = listItems.FirstOrDefault(x => x.id == "INPROGRESS");
                //if (pendingItem != null)
                //{
                //    pendingCount += pendingItem.RootId ?? 0;
                //}
                //var overDueItem = listItems.FirstOrDefault(x => x.id == "OVERDUE");
                //if (overDueItem != null)
                //{
                //    pendingCount += overDueItem.RootId ?? 0;
                //}
                //pending.DisplayName = $"Pending ({pendingCount})";
                //pending.Name = $"Pending ({pendingCount})";
                //list.Add(pending);

                //var completed = new TreeViewViewModel
                //{
                //    id = "COMPLETED",
                //    Type = "STATUS",
                //    ParentId = id,
                //    hasChildren = false,
                //    DisplayName = "Completed (0)",
                //    Name = "Completed (0)"
                //};
                //var completedItem = listItems.FirstOrDefault(x => x.id == "COMPLETED");
                //if (completedItem != null)
                //{
                //    completed.DisplayName = $"Completed ({completedItem.RootId ?? 0})";
                //    completed.Name = $"Completed ({completedItem.RootId ?? 0})";
                //}
                //list.Add(completed);

                //var rejected = new TreeViewViewModel
                //{
                //    id = "REJECTED",
                //    Type = "STATUS",
                //    ParentId = id,
                //    hasChildren = false,
                //    DisplayName = "Rejected (0)",
                //    Name = "Rejected (0)"
                //};
                //var rejectedItem = listItems.FirstOrDefault(x => x.id == "REJECTED");
                //if (rejectedItem != null)
                //{
                //    rejected.DisplayName = $"Rejected ({rejectedItem.RootId ?? 0})";
                //    rejected.Name = $"Rejected ({rejectedItem.RootId ?? 0})";
                //}
                //list.Add(rejected);


            }
            else
            {

                //          query = @$"select  
                //                          s.""StatusLabel"" as Name,
                //                          --tmp.""Subject"" ||' (' || Count( distinct b.""Id"") || ')' as Name,
                //                          tmp.""TemplateCode"" as id,
                //                          false as hasChildren
                //                          FROM rec.""UserRoleStageChild"" as s
                //                 -- left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
                //                  --left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                //--left join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
                //--left join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                //--left join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                //                 -- left join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                //--Where task.""TaskStatusCode"" = 'INPROGRESS' and app.""Id"" is not null and  task.""AssigneeUserId"" = '{userId}'
                //                  where s.""InboxStageId""='{stageId}'

                //                  order by s.""SequenceOrder"" asc";
                query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                '{parentId}' as StageId,s.""StatusCode"" as StatusCode,
                false as hasChildren
                FROM public.""UserRoleStageChild"" as s
                --left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                left join(
                    select case when ts.""Code""='TASK_STATUS_OVERDUE' then 'TASK_STATUS_INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                    FROM public.""NtsService"" as s
                    join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id""
                    join public.""LOV"" as ts on ts.""Id"" = task.""TaskStatusId""
                    join public.""Template"" as tmp on tmp.""Code"" = task.""TemplateCode""
                    join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
                  
                    Where tmp.""Code""='{parentId}' and task.""AssignedToUserId"" = '{userId}'
                    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 
                    
                    group by TaskStatusCode  
                ) t on t.TaskStatusCode=ANY(s.""StatusCode"")
                where s.""InboxStageId""='{stageId}'
                order by s.""SequenceOrder"" asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



            }
            return list;
        }


        #endregion NH

        #region
        private async Task<List<RecApplicationViewModel>> GetBatchApplicantList(string batchId)
        {
            string query = @$"SELECT q.*
                            FROM cms.""N_REC_REC_BATCH"" as c
                            JOIN cms.""N_REC_APPLICATION"" as q ON q.""BatchId"" = c.""Id"" and q.""IsDeleted""=false
                            where c.""Id"" = '{batchId}' ";
            var list = await _queryRepo.ExecuteQueryList<RecApplicationViewModel>(query, null);
            return list;
        }
        public async Task UpdateApplicationtStatus(string users, string type)
        {
            var state1 = await _repo.GetSingleGlobal<LOVViewModel, LOV>(x => x.Code == type);
            string state = state1.Id;
            var str = users.Trim(',').Replace(",", "','");
            var where = String.Concat("'", str, "'");
            //var applicationstate = type;

            string query = @$"update cms.""N_REC_APPLICATION"" set ""ApplicationStatusId""='{state}' where ""Id"" in ( {where} )";
            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);

        }
        public async Task UpdateStatus(string batchId, string code)
        {
            var status = await _repo.GetSingleGlobal<LOVViewModel, LOV>(x => x.LOVType == "BatchStatus" && x.Code == "PendingwithHM");
            if (code.IsNotNullAndNotEmpty())
            {
                status = await _repo.GetSingleGlobal<LOVViewModel, LOV>(x => x.LOVType == "BatchStatus" && x.Code == code);
            }
            string query = @$"update cms.""N_REC_REC_BATCH"" set ""BatchStatus""='{status.Id}' where ""Id""='{batchId}'";

            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);
            if (code != "Close")
            {
                var apps = await GetBatchApplicantList(batchId);
                if (apps != null && apps.Count > 0)
                {
                    foreach (var item in apps)
                    {
                       // await _applicationBusiness.CreateApplicationStatusTrack(item.Id, "SL_BATCH_SEND");
                        if (code == "Close")
                        {
                        }
                        else
                        {
                            await UpdateApplicationState(item.Id, "ShortListByHm");
                            await UpdateApplicationtStatus(item.Id, "NotShortlisted");
                        }
                    }
                }
            }
        }
        public async Task UpdateApplicationState(string users, string type)
        {
            var state1 = await _repo.GetSingleGlobal<LOVViewModel, LOV>(x => x.Code == type);
            string state = state1.Id;
            var str = users.Trim(',').Replace(",", "','");
            var where = String.Concat("'", str, "'");
            //var applicationstate = type;

            string query = @$"update cms.""N_REC_APPLICATION"" set ""ApplicationStateId""='{state}' where ""Id"" in ( {where} )";
            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);

        }
        public async Task<RecBatchViewModel> GetBatchApplicantCount(string Id)
        {
            string query = @$"SELECT c.*,count( q.""Id"" ) as NoOfApplication
                            FROM cms.""N_REC_REC_BATCH"" as c
                            left JOIN cms.""N_REC_APPLICATION"" as q ON q.""BatchId"" = c.""Id"" and q.""IsDeleted""=false
                            where c.""Id"" = '{Id}'
                            group by c.""Id""
                            ";
            var list = await _queryRepo.ExecuteQuerySingle<RecBatchViewModel>(query, null);
            return list;
        }
        public async Task<List<RecBatchViewModel>> GetBatchData(string jobid, BatchTypeEnum type, string orgId)
        {
            var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"SELECT c.*, o.""DepartmentName"" as Organization,l.""Code"" as BatchStatusCode, l.""Name"" as BatchStatusName,hm.""Name"" as HiringManagerName,hod.""Name"" as HeadOfDepartmentName,
                            sum(case when q.""Id"" is not null then 1 else 0 end) as NoOfApplication
                            FROM cms.""N_REC_REC_BATCH"" as c                           
                            LEFT JOIN cms.""N_REC_APPLICATION"" as q ON q.""BatchId"" = c.""Id""
                            LEFT JOIN cms.""N_CoreHR_HRDepartment"" as o ON o.""Id"" = c.""OrganizationId""
                            LEFT JOIN public.""LOV"" as l ON l.""Id"" = c.""BatchStatus""
                            left join  public.""User"" as hm on c.""HiringManager""=hm.""UserId""
                            left join  public.""User"" as hod on c.""HeadOfDepartment""=hod.""UserId""
                            where c.""JobId""='{jobid}' and c.""BatchType""='{batchtype}'
                            --and c.""OrganizationId""='{orgId}'
                            group by c.""Id"",o.""DepartmentName"",l.""Name"",l.""Code"",hm.""Name"",hod.""Name""
                            ";
            var list = await _queryRepo.ExecuteQueryList<RecBatchViewModel>(query, null);
            return list;
        }
        public async Task<RecBatchViewModel> GetBatchDataById(string batchId)
        {
            ///var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"select * from  cms.""N_REC_REC_BATCH"" where ""Id""='{batchId}'";
            var list = await _queryRepo.ExecuteQuerySingle<RecBatchViewModel>(query, null);
            return list;
        }
        public async Task<List<ManpowerRecruitmentSummaryViewModel>> GetBatchDataByJobId(string jobId)
        {
            ///var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"select * from  cms.""N_REC_ManpowerRequirement"" where ""JobId""='{jobId}' and ""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<ManpowerRecruitmentSummaryViewModel>(query, null);
            return list;
        }
        public async Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search)
        {
            try
            {
                var query = "";
                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId,app.""ApplicationNo"" as ApplicationNo, app.""CandidateId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""CountryName"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
TRUNC(app.""TotalWorkExperience""::decimal,0) as TotalWorkExperience, pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
apps.""Id"" as ApplicationState,appStatus.""Id"" as ApplicationStatus,
app.""Score"" as Score, app.""QatarId"" as QatarId,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
--,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
--appexpbyc.""NoOfYear"" as TotalIndianExperience
,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
bs.""Code"" as BatchStatusCode,#APPCOMMENT1# b.""BatchName"" as BatchName

FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRCurrency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join public.""LOV"" as gen on gen.""Id""=app.""GenderId"" and gen.""LOVType""='LOV_GENDER'
left join public.""LOV"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatusId"" and maritalstatus.""LOVType""='LOV_MARITALSTATUS'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_REC_APPLICATION_DRIVING_LICENSE"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as dlc on dlc.""Id"" = adl.""CountryId""
left join cms.""N_REC_APPLICATION_EDUCATIONAL"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join cms.""N_REC_APPLICATION_EXPERIENCE"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
#EXPBYCOUNTRYAPP#
#JOBEXPAPP#
left join cms.""N_REC_APPLICATION_EXPERIENCE_SECTOR"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
#GCCEXPAPP#
#EXPENGAPP#
 #EXPARABICAPP#
left join cms.""N_REC_APPLICATION_COMP_PROFICIENCY"" as appcpc on appcpc.""ApplicationId""=app.""Id""
left join public.""LOV"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevelId""
--left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
--left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 
#APPCOMMENT#
left join cms.""N_REC_REC_BATCH"" as b on b.""Id""=app.""BatchId""
left join public.""LOV"" as bs on bs.""Id"" = b.""BatchStatus"" and bs.""LOVType"" = 'BatchStatus'
where app.""JobId""='" + search.JobTitleForHiring + @"' and app.""IsDeleted""=false
 
and ((apps.""Code""='ShortListByHR' and appStatus.""Code"" in ('WAITLISTED' ,'REJECTED')) or (apps.""Code""='ShortListByHm' and appStatus.""Code"" in ('RejectedHM')) or apps.""Code""='Rereviewed' or apps.""Code""='UnReviewed' )#WHERE# 

union

Select distinct 'CandidateProfile' as CandidateType,'' as ApplicationId,application.""ApplicationNo"" as ApplicationNo,app.""Id"" as CandidateProfileId,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName, app.""LastName"" as LastName,
app.""Age"" as Age, app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""NationalityName"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""CountryName"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate,
app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""CountryName"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet, app.""PermanentAddressCity"" as PermanentAddressCity,
app.""PermanentAddressState"" as PermanentAddressState, TRUNC(app.""TotalWorkExperience""::decimal,0) as TotalWorkExperience,
pac.""CountryName"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, app.""ContactPhoneLocal"" as ContactPhoneLocal,
'' as JobId,
'UnReviewed' as ApplicationStatusCode,'UnReviewed' as ApplicationStateName,  '' as ApplicationStateCode,
'' as ApplicationState,'' as ApplicationStatus,
--CAST ('0.0' AS DOUBLE PRECISION) as Score
'0.0' as Score
,app.""QatarId"" as QatarId,
 app.""NetSalary"" as NetSalary,
app.""OtherAllowances"" as OtherAllowances
--,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
--appexpbyc.""NoOfYear"" as TotalIndianExperience
,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
'' as BatchStatusCode,'' as Comment,'' as BatchName
FROM cms.""N_REC_REC_CANDIDATE"" as app
left join cms.""N_CoreHR_HRCurrency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""N_REC_APPLICATION"" as application on application.""CandidateId""=app.""Id""  and application.""IsDeleted""=false

left join cms.""N_CoreHR_HRCountry"" pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""N_CoreHR_HRCountry"" pic on pic.""Id"" = app.""PassportIssueCountryId""
left join public.""LOV"" as gen on gen.""Id""=app.""GenderId"" and gen.""LOVType""='LOV_GENDER'
left join public.""LOV"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatusId"" and maritalstatus.""LOVType""='LOV_MARITALSTATUS'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""nationality""
left join cms.""N_REC_CANDIDATE_DRIVING_LICENSE"" as adl on adl.""CandidateId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as dlc on dlc.""Id"" = adl.""CountryId""
left join cms.""N_REC_CANDIDATE_EDUCATIONAL"" as aed on aed.""CandidateId"" = app.""Id"" 
left join cms.""N_REC_CANDIDATE_EXPERIENCE"" as appexp on appexp.""CandidateId"" = app.""Id""
#EXPBYCOUNTRYCAN#
#JOBEXPCAN#
left join cms.""N_REC_CANDIDATE_EXPERIENCE_SECTOR"" as appexpsec on appexpsec.""CandidateId"" = app.""Id"" 
#GCCEXPCAN#
 #EXPENGCAN#
 #EXPARABICCAN#
left join cms.""N_REC_CANDIDATE_COMP_PROFICIENCY"" as appcpc on appcpc.""CandidateId""=app.""Id""
left join public.""LOV"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevelId""
--left join rec.""CandidateReferences"" as appr on appr.""CandidateId"" = app.""Id"" 
--left join rec.""CandidateExperienceByNature"" as appebn on appebn.""CandidateId"" = app.""Id"" 
where application.""Id"" is null 

and app.""SourceFrom""='Migrated' #WHERE#";
                // }

                var where = "";
                var appcomment1 = @" '' as Comment, ";
                var appcomment = "";
                var gcca = "";
                var gcc = "";
                var jea = "";
                var jec = "";
                var expengcan = "";
                var expengapp = "";
                var exparabiccan = "";
                var exparabicapp = "";
                var expbycoapp = "";
                var expbycocan = "";
                if (search.TotalExperience.IsNotNull())
                {
                    where += @" and app.""TotalWorkExperience"">='" + search.TotalExperience + "'";// and app.""TotalWorkExperience""='{search.TotalExperience}'";
                }
                if (search.TotalGulfExperience.IsNotNull())
                {
                    gcca = @" left join(select appexpbg.*, ctt.""Code""
 from rec.""N_REC_APPLICATION_EXPERIENCE_OTHER"" as appexpbg
 join public.""LOV"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""LOVType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf' ";
                    gcc = @" left join(select appexpbg.*, ctt.""Code""
 from rec.""N_REC_CANDIDATE_EXPERIENCE_OTHER"" as appexpbg
 join public.""LOV"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""LOVType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""CandidateId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf' ";
                    where += @" and appexpbog.""NoOfYear"">='" + search.TotalGulfExperience + "'";// and app.""TotalWorkExperience""='{search.TotalExperience}'";
                }
                if (search.JobTitle.IsNotNullAndNotEmpty())
                {
                    where += @" and ej.""Name""='" + search.JobTitle + "'";
                }
                if (search.YearOfJobExperience.IsNotNull())
                {
                    jea = @" left join cms.""N_REC_APPLICATION_EXPERIENCE_JOB"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRJob"" as ej on ej.""Id"" = appexpj.""JobId"" ";
                    jec = @" left join cms.""N_REC_CANDIDATE_EXPERIENCE_JOB"" as appexpj on appexpj.""CandidateId"" = app.""Id"" 
left join cms.""N_CoreHR_HRJob"" as ej on ej.""Id"" = appexpj.""JobId"" ";
                    where += @" and appexpj.""NoOfYear"" >= '" + search.YearOfJobExperience + "'";
                }
                if (search.OtherExperience.IsNotNullAndNotEmpty() || search.YearOfOtherCountryExperience.IsNotNull())
                {
                    expbycoapp = @" left join cms.""N_REC_APPLICATION_EXPERIENCE_COUNTRY"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as aec on aec.""Id"" = appexpc.""CountryId"" ";
                    expbycocan = @" left join cms.""N_REC_CANDIDATE_EXPERIENCE_COUNTRY"" as appexpc on appexpc.""CandidateId"" = app.""Id"" 
left join cms.""N_CoreHR_HRCountry"" as aec on aec.""Id"" = appexpc.""CountryId"" ";
                }
                if (search.OtherExperience.IsNotNullAndNotEmpty())
                {
                    where += @" and appexpc.""CountryId"" =  '" + search.OtherExperience + "'";
                }
                if (search.YearOfOtherCountryExperience.IsNotNull())
                {
                    where += @" and appexpc.""NoOfYear"" >=  '" + search.YearOfOtherCountryExperience + "'";
                }
                if (search.Industry.IsNotNullAndNotEmpty())
                {
                    where += @" and appexpsec.""IndustryId""='" + search.Industry + "'";
                }
                if (search.Category.IsNotNullAndNotEmpty())
                {
                    where += @" and appexpsec.""CategoryId"" =  '" + search.Category + "'";
                }
                if (search.YearOfIndustryExperience.IsNotNull())
                {
                    where += @" and appexpsec.""NoOfYear"" >=  '" + search.YearOfIndustryExperience + "'";
                }
                if (search.CategoryExperience.IsNotNull())
                {
                    where += @" and appexpsec.""NoOfYear"" >=  '" + search.CategoryExperience + "'";
                }
                if (search.IsEnglishProficiency == true)
                {
                    if (search.EnglishProficiency.IsNotNull())
                    {
                        expengcan = @"left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_CANDIDATE_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""LanguageId""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""CandidateId""=app.""Id""
and appcpe.""Code"" = 'English'";
                        expengapp = @" left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""LanguageId""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'";
                        where += @" and appcpe.""cpl"" =  '" + search.EnglishProficiency + "'";
                    }
                }
                if (search.IsArabicProficiency == true)
                {
                    if (search.ArabicProficiency.IsNotNull())
                    {
                        exparabicapp = @"left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""N_REC_APPLICATION_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""LanguageId""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic' ";
                        exparabiccan = @" left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from cms.""N_REC_CANDIDATE_LANG_PROFICIENCY"" as cp
 join public.""LOV"" as lv on lv.""Id"" = cp.""LanguageId""
 join public.""LOV"" as pl on pl.""Id"" = cp.""ProficiencyLevelId""
 where pl.""LOVType"" = 'LOV_PROFICIENCYLEVEL' and lv.""LOVType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""CandidateProfileId"" = app.""Id""
and appcpa.""Code"" = 'Arabic' ";
                        where += @" and appcpa.""cpl"" =  '" + search.ArabicProficiency + "'";
                    }
                }
                if (search.IsComputerLiteratureProficiency == true)
                {
                    if (search.ComputerLiteratureProficiency.IsNotNull())
                    {
                        where += @" and cplv.""Code"" =  '" + search.ComputerLiteratureProficiency + "'";
                    }
                }

                if (search.Qualification.IsNotNullAndNotEmpty())
                {
                    where += @" and aed.""QualificationId"" = '" + search.Qualification + "'";
                }
                if (search.Specialization.IsNotNullAndNotEmpty())
                {
                    where += @" and aed.""SpecializationId"" = '" + search.Specialization + "'";
                }
                if (search.Duration.IsNotNullAndNotEmpty())
                {
                    where += @" and aed.""Duration"" = '" + search.Duration + "'";
                }
                if (search.PassingYear.IsNotNullAndNotEmpty())
                {
                    where += @" and aed.""PassingYear"" = '" + search.PassingYear + "'";
                }
                if (search.Marks.IsNotNullAndNotEmpty())
                {
                    where += @" and aed.""Marks"" = '" + search.Marks + "'";
                }
                if (search.DL == "YES")
                {
                    if (search.Country.IsNotNullAndNotEmpty())
                    {
                        where += @" and adl.""CountryId"" = '" + search.Country + "'";
                    }
                    if (search.Type.IsNotNullAndNotEmpty())
                    {
                        where += @" and adl.""LicenseType"" ='" + search.Type + "'";
                    }
                    if (search.IssueDate.IsNotNull())
                    {
                        where += @" and adl.""IssueDate""::TIMESTAMP::DATE <= '" + search.IssueDate + "'::TIMESTAMP::DATE ";
                    }
                    if (search.ExpiryDate.IsNotNull())
                    {
                        where += @" and adl.""ValidUpTo""::TIMESTAMP::DATE>='" + search.ExpiryDate + "'::TIMESTAMP::DATE";
                    }
                }
                if (search.BirthDate.IsNotNull())
                {
                    where += @" and app.""BirthDate"" = '" + search.BirthDate + "'";
                }
                if (search.PassportNumber.IsNotNullAndNotEmpty())
                {
                    where += @" and app.""PassportNumber"" = '" + search.PassportNumber + "'";
                }
                if (search.NetSalary.IsNotNullAndNotEmpty())
                {
                    where += @" and app.""NetSalary"" = '" + search.NetSalary + "'";
                }
                if (search.Comment.IsNotNullAndNotEmpty())
                {
                    where += @" and appc.""Comment"" like '%" + search.Comment + "%'";
                }
                if (search.Nationality.IsNotNullAndNotEmpty())
                {
                    where += @" and n.""Id"" = '" + search.Nationality + "'";
                }
                if (search.Age.IsNotNullAndNotEmpty())
                {
                    where += @" and app.""Age"" = '" + search.Age + "'";
                }
                if (search.Gender.IsNotNullAndNotEmpty())
                {
                    where += @" and gen.""Id"" = '" + search.Gender + "'";
                }
                query = query.Replace("#WHERE#", where);
                if (search.Comment.IsNotNullAndNotEmpty())
                {
                    appcomment1 = @" appc.""Comment"" as Comment, ";
                    appcomment = @" left join cms.""ApplicationStateComment"" as appc on appc.""ApplicationId""=app.""Id"" ";

                }
                query = query.Replace("#APPCOMMENT1#", appcomment1);
                query = query.Replace("#APPCOMMENT#", appcomment);
                query = query.Replace("#GCCEXPAPP#", gcca);
                query = query.Replace("#GCCEXPCAN#", gcc);
                query = query.Replace("#JOBEXPAPP#", jea);
                query = query.Replace("#JOBEXPCAN#", jec);
                query = query.Replace("#EXPBYCOUNTRYCAN#", expbycocan);
                query = query.Replace("#EXPBYCOUNTRYAPP#", expbycoapp);
                query = query.Replace("#EXPENGCAN#", expengcan);
                query = query.Replace("#EXPENGAPP#", expengapp);
                query = query.Replace("#EXPARABICCAN#", exparabiccan);
                query = query.Replace("#EXPARABICAPP#", exparabicapp);

                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);

                if (search.CandidateProfileSearch == true && search.JobApplicationSearch == false)
                {
                    List<ApplicationViewModel> list = new List<ApplicationViewModel>();

                    return allList.Where(x => x.CandidateType == "CandidateProfile").ToList();


                }
                else if (search.CandidateProfileSearch == false && search.JobApplicationSearch == true)
                {
                    List<ApplicationViewModel> list = new List<ApplicationViewModel>();
                    if (search.StageId.IsNotNullAndNotEmpty())
                    {
                        if (search.ApplicationStatusId.IsNotNullAndNotEmpty())
                        {
                            list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationState == search.StageId && x.ApplicationStatus == search.ApplicationStatusId).ToList());
                        }
                        else
                        {
                            list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationState == search.StageId).ToList());
                        }
                    }
                    else
                    {
                        if (search.ApplicationStatusId.IsNotNullAndNotEmpty())
                        {
                            list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStatus == search.ApplicationStatusId).ToList());
                        }
                        else
                        {
                            list.AddRange(allList.Where(x => x.CandidateType == "JobApplication").ToList());
                        }
                    }
                    //if (!search.AllCandidateApplication && !search.ShortlistedCandidateApplication && !search.RejectedCandidateSearch && !search.WaitlistedCandidateSearch)
                    //{
                    //    list.AddRange(allList.Where(x =>/*  x.ApplicationStateCode == "ShortListByHr" &&*/ x.CandidateType == "JobApplication").ToList());
                    //}
                    //if (search.AllCandidateApplication)
                    //{
                    //    list.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" && x.CandidateType == "JobApplication").ToList());
                    //}

                    //if (search.ShortlistedCandidateApplication)
                    //{

                    //    list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "SHORTLISTED").ToList());
                    //}
                    //if (search.RejectedCandidateSearch)
                    //{

                    //    list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "REJECTED").ToList());
                    //}
                    //if (search.WaitlistedCandidateSearch)
                    //{
                    //    list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "WAITLISTED").ToList());

                    //}
                    return list;

                }
                else if (search.CandidateProfileSearch == true && search.JobApplicationSearch == true)
                {

                    List<ApplicationViewModel> Newlist = new List<ApplicationViewModel>();
                    Newlist.AddRange(allList.Where(x => x.CandidateType == "CandidateProfile").ToList());
                    if (search.StageId.IsNotNullAndNotEmpty())
                    {
                        if (search.ApplicationStatusId.IsNotNullAndNotEmpty())
                        {
                            Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationState == search.StageId && x.ApplicationStatus == search.ApplicationStatusId).ToList());
                        }
                        else
                        {
                            Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationState == search.StageId).ToList());
                        }
                    }
                    else
                    {
                        if (search.ApplicationStatusId.IsNotNullAndNotEmpty())
                        {
                            Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStatus == search.ApplicationStatusId).ToList());
                        }
                        else
                        {
                            Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication").ToList());
                        }
                    }
                    //if (!search.AllCandidateApplication && !search.ShortlistedCandidateApplication && !search.RejectedCandidateSearch && !search.WaitlistedCandidateSearch)
                    //{
                    //    Newlist.AddRange(allList.Where(x => /*x.ApplicationStateCode == "UnReviewed" && x.ApplicationStateCode == "ShortListByHr" &&*/ x.CandidateType == "JobApplication").ToList());
                    //}
                    //if (search.AllCandidateApplication)
                    //{
                    //    Newlist.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" && x.CandidateType == "JobApplication").ToList());
                    //}
                    //if (search.ShortlistedCandidateApplication)
                    //{
                    //    Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "SHORTLISTED").ToList());
                    //}
                    //if (search.RejectedCandidateSearch)
                    //{
                    //    Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "REJECTED").ToList());

                    //}
                    //if (search.WaitlistedCandidateSearch)
                    //{
                    //    Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "WAITLISTED").ToList());
                    //}

                    if (Newlist.Count() > 0)
                    {

                        //return Newlist;
                    }
                    else
                    {
                        Newlist = allList.ToList();
                        // return allList;
                    }
                    return Newlist;
                }
                else
                {
                    List<ApplicationViewModel> Newlist = new List<ApplicationViewModel>();
                    return Newlist;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<ApplicationStateCommentViewModel>> GetApplicationStateCommentData(string appId, string appStateId)
        {
            string query = $@"select c.*,app.""ApplicationNo"", appstate.""Code"" as ApplicationStateCode, u.""Name"" as CommentedBy
                              from cms.""N_REC_APPLICATION_STATE_COMMENT"" as c
                              left join cms.""N_REC_APPLICATION"" as app on app.""Id""=c.""ApplicationId"" and app.""IsDeleted""=false
                              left join public.""LOV"" as appstate on appstate.""Id""=c.""ApplicationStateId"" and appstate.""IsDeleted""=false
                              left join public.""User"" as u on u.""Id""=c.""CreatedBy"" and u.""IsDeleted""=false  
                              where c.""IsDeleted""=false and c.""ApplicationId""='{appId}' and c.""ApplicationStateId""='{appStateId}'  ";
            var queryData = await _queryRepo.ExecuteQueryList<ApplicationStateCommentViewModel>(query, null);
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
            //string query = @$"select j.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
            //            NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode , jd.""Id"" as JobAdvId
            //           from rec.""JobAdvertisement""
            //            as jd inner join cms.""Job"" as j
            //            on j.""Id"" = jd.""JobId""
            //            inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
            //            left join rec.""ListOfValue"" as lov on lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code""=j.""ManpowerTypeCode""
            //            WHERE jd.""Status"" = '1' AND jd.""IsDeleted""='false' AND j.""IsDeleted""='false' 
            //             ";
            // for No of position Correction use below one
            string query = $@"select j.""Id"" as Id ,j.""JobTitle"" as JobName,case when c.""Requirement"" is not null then c.""Requirement"" else 0 end as
                       NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode , jd.""Id"" as JobAdvId
                       from cms.""N_REC_JobAdvertisement""
                        as jd inner join cms.""N_CoreHR_HRJob"" as j
                        on j.""Id"" = jd.""JobId""
inner join public.""NtsService"" as n on n.""UdfNoteTableId"" = jd.""Id""  
                        inner join public.""LOV"" as actlov on actlov.""Id"" = n.""ServiceStatusId"" AND actlov.""Code"" != 'SERVICE_STATUS_DRAFT'
                        left join public.""LOV"" as lov on lov.""LOVType"" = 'REC_MANPOWER' and lov.""Id"" = j.""ManpowerTypeId""

                        left join(select sum(c.""Requirement""::DECIMAL) as ""Requirement"",c.""JobId"" from cms.""N_REC_ManpowerRequirement"" as c

                                  where c.""IsDeleted"" = false

                        group by c.""JobId"" )as c on c.""JobId"" = j.""Id""
                        WHERE jd.""Status"" = '1' AND jd.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                         ";
            var queryData = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentList(string jobId, string orgId)
        {
            ///var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"select * from  cms.""N_REC_ManpowerRequirement"" where ""IsDeleted""=false ";

            if (jobId.IsNotNullAndNotEmpty())
            {
                query = string.Concat(query, $@"and ""JobId""='{jobId}'");
            }
            if (orgId.IsNotNullAndNotEmpty())
            {
                query = string.Concat(query, $@"and ""OrganizationId""='{orgId}'");
            }

            var list = await _queryRepo.ExecuteQueryList<ManpowerRecruitmentSummaryViewModel>(query, null);
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId)
        {
            string query = @$"select ""Id"" as Id,""BatchName"" as Name from  cms.""N_REC_REC_BATCH"" where ""IsDeleted""=false and ""BatchType""='{(int) batchType}' and ""JobId""='{JobAddId}'";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<RecBatchViewModel>> GenerateNextBatchNo(string Name)
        {
            string query = @$"SELECT *  FROM cms.""N_REC_REC_BATCH"" as app
                                where app.""BatchName"" like '{Name}%' COLLATE ""tr-TR-x-icu""
                            ";
            var result = await _queryRepo.ExecuteQueryList<RecBatchViewModel>(query, null);
            return result;
        }

        public async Task<List<RecHiringManagerViewModel>> GetHMListByOrgId(string orgId)
        {
            string query = @$"SELECT c.*,p.""JobTitle"" as DesignationName
                                FROM cms.""N_REC_HIRING_MANAGER"" as c
                                left JOIN cms.""N_CoreHR_HRJob"" as p on p.""Id"" = c.""DesignationId""
                                --inner join rec.""HiringManagerOrganization"" as hmo on c.""Id""=hmo.""HiringManagerId""
                                LEFT JOIN public.""User"" as u ON u.""Id"" = c.""UserId""
                                WHERE c.""IsDeleted""=false and c.""OrganizationId"" like '%{orgId}%'";

            var queryData = await _queryRepo.ExecuteQueryList<RecHiringManagerViewModel>(query, null);
            return queryData;
        }
        public async Task<List<RecHeadOfDepartmentViewModel>> GetHODListByOrgId(string orgId)
        {
            string query = @$"SELECT c.*,p.""JobTitle"" as DesignationName
                                FROM cms.""N_REC_HEAD_OF_DEPARTMENT"" as c
                                left JOIN cms.""N_CoreHR_HRJob"" as p on p.""Id"" = c.""DesignationId""
                                --inner join rec.""HeadOfDepartmentOrganization"" as hmo on c.""Id""=hmo.""HeadOfDepartmentId""
                                LEFT JOIN public.""User"" as u ON u.""Id"" = c.""UserId""
                                WHERE c.""IsDeleted""=false and c.""OrganisationId"" like '%{orgId}%'";

            var queryData = await _queryRepo.ExecuteQueryList<RecHeadOfDepartmentViewModel>(query, null);
            return queryData;
        }
        #endregion

        public async Task<List<CandidateProfileViewModel>> GetWorkerList(string id)
        {
            //var query = @"select distinct pl.*,concat_ws('_',j.""Name"", app.""JobId"") as JobAdvertisement FROM rec.""CandidateProfile"" as pl
            //            left join rec.""Application"" as app on app.""CandidateProfileId"" = pl.""Id""
            //            left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1                        
            //            left join cms.""Job"" as j on j.""Id"" = app.""JobId""                      
            //            where /*j.""ManpowerTypeCode"" != 'Staff' and*/ pl.""SourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + id + "'";

            var query = @"select distinct pl.*,concat_ws('_',j.""JobTitle"", app.""JobId"") as JobAdvertisement FROM cms.""N_REC_REC_CANDIDATE"" as pl
                        left join cms.""N_REC_APPLICATION"" as app on app.""CandidateId"" = pl.""Id""
                        left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1                        
                        left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = app.""JobId""                      
                        where /*j.""ManpowerTypeCode"" != 'Staff' and*/ pl.""SourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + id + "'";


            var queryData = await _queryRepo.ExecuteQueryList<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<JobAdvertisementViewModel>> GetJobIdNameList()
        {
            var agencyid = "";
            var agency = await _repo.GetSingleGlobal<AgencyViewModel, Agency>(x => x.UserId == _repo.UserContext.UserId);
            if (agency != null)
            {
                agencyid = agency.Id;
            }
            //string query = @$"select j.""Id"" as Id ,j.""Name""  as JobName,jd.""NoOfPosition"" as
            //            NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode , jd.""Id"" as JobAdvId
            //           from rec.""JobAdvertisement""
            //            as jd inner join cms.""Job"" as j
            //            on j.""Id"" = jd.""JobId""
            //            inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
            //            left join rec.""ListOfValue"" as lov on lov.""ListOfValueType""='LOV_MANPOWERTYPE' and lov.""Code""=j.""ManpowerTypeCode""
            //            WHERE jd.""Status"" = '1' AND jd.""IsDeleted""='false' AND j.""IsDeleted""='false' 
            //           -- and (jd.""AgencyId"" is null or '{agencyid}'=ANY(jd.""AgencyId"")) ";
            string query = @$"select j.""Id"" as Id ,j.""JobTitle""  as JobName,jd.""NoOfPosition"" as
                        NoOfPosition,jd.""JobId"" as JobId,lov.""Name"" as ManpowerTypeName,lov.""Code"" as ManpowerTypeCode , jd.""Id"" as JobAdvId
                       from cms.""N_REC_JobAdvertisement""
                        as jd inner join cms.""N_CoreHR_HRJob"" as j
                        on j.""Id"" = jd.""JobId""
                        --inner join public.""LOV"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'
                        left join public.""LOV"" as lov on lov.""LOVType""='LOV_MANPOWERTYPE' and lov.""Id""=j.""ManpowerTypeId""
                        WHERE jd.""Status"" = '1' AND jd.""IsDeleted""='false' AND j.""IsDeleted""='false' 
                       -- and (jd.""AgencyId"" is null or '{agencyid}'=ANY(jd.""AgencyId"")) ";
            var queryData = await _queryRepo.ExecuteQueryList<JobAdvertisementViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<RecTaskViewModel>> GetRecTaskList(string search)
        {
            var tempCategory = await _templateCategoryBusiness.GetSingle(x => x.Code == "REC" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategory.Id);
            templateList = templateList.Where(x => x.Code != "REC_JOB_ADVERTISEMENT" && x.Code != "REC_MANPOWER_REQUIREMENT").ToList();
            var selectQry = "";
            var i = 1;
            foreach (var item in templateList)
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (i != 1)
                {
                    selectQry += " union ";
                }

                selectQry = $@" {selectQry} SELECT task.""TaskSubject"" as Subject,task.""TaskNo"" as TaskNo,task.""DueDate"" as DisplayDueDate,task.""StartDate"",
                au.""Name"" as AssigneeUserName,app.""GaecNo"",app.""FirstName"" as CandidateName,j.""JobTitle"" as JobName,
                o.""DepartmentName"" as OrgUnitName,t.""Code"" as TemplateCode,task.""Id"" as Id FROM  public.""NtsService"" NS                
                 join public.""NtsNote"" N on NS.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false and N.""CompanyId""='{_userContext.CompanyId}'
	             join  cms.""{tableMeta.Name}"" as  RT on  N.""Id"" =RT.""NtsNoteId"" and RT.""IsDeleted""=false and RT.""CompanyId""='{_userContext.CompanyId}'
                 join public.""NtsTask"" as task on NS.""Id"" =task.""ParentServiceId"" and task.""AssignedToUserId""='{_userContext.UserId}' and task.""IsDeleted""=false
                 join public.""User"" as au on task.""AssignedToUserId""=au.""Id"" and au.""IsDeleted""=false
                 join public.""LOV"" as LOV on task.""TaskStatusId""=LOV.""Id"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_userContext.CompanyId}'
                 left join cms.""N_REC_APPLICATION"" as app on app.""Id""=RT.""ApplicationId"" and app.""IsDeleted""=false 
                left join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId"" and b.""IsDeleted""=false and b.""CompanyId""='{_userContext.CompanyId}'
                LEFT JOIN cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = b.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_userContext.CompanyId}'
                LEFT JOIN cms.""N_CoreHR_HRDepartment"" as o ON o.""Id"" = b.""OrganizationId"" and o.""IsDeleted""=false and o.""CompanyId""='{_userContext.CompanyId}'
                 
                 join public.""Template"" as t on t.""Id"" = task.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                where NS.""IsDeleted""='false' and  NS.""CompanyId""='{_userContext.CompanyId}' and LOV.""Code""='{search}' ";
                i++;
            }

            var result = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(selectQry, null);
            result = result.OrderByDescending(x => x.CreatedDate).ToList();
            return result;
        }
        public async Task<List<ManpowerRecruitmentSummaryViewModel>> GetActiveManpowerRecruitmentSummaryData()
        {

            var filter = "";

            string query = @$"SELECT c.*,jd.""Id"" as JobDescriptionId,mt.""Name"" as ""ManpowerType"",s.""Id"" as ServiceId,
                            sum(case when ur.""Code""='ORG_UNIT' and c.""Id"" is not null then 1 else 0 end) as OrgUnit,
                            sum(case when ur.""Code"" = 'PLANNING' and c.""Id"" is not null then 1 else 0 end) as PlanningUnit,
                            sum(case when ur.""Code"" = 'HR' and c.""Id"" is not null then 1 else 0 end) as Hr,
                            u.""Name"" as CreatedByName,j.""JobTitle"",o.""DepartmentName"" as OrganizationName
                            FROM cms.""N_REC_ManpowerRequirement"" as c
                            join public.""NtsService"" as s on c.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
                            left join public.""NtsServiceComment"" as sc on s.""Id""=sc.""NtsServiceId"" and sc.""IsDeleted""=false
                            --LEFT JOIN rec.""ManpowerSummaryComment"" as q ON q.""ManpowerRecruitmentSummaryId"" = c.""Id""
                            left join cms.""N_REC_RecJobDescription"" as jd on jd.""JobId""=c.""JobId""
                            
                            LEFT JOIN public.""UserRole"" as ur ON ur.""Id"" = sc.""CommentedByUserId""
                            LEFT JOIN public.""User"" as u ON u.""Id"" = s.""OwnerUserId""
                            LEFT JOIN cms.""N_CoreHR_HRJob"" as j ON j.""Id"" = c.""JobId""
                            left join public.""LOV"" as mt on mt.""LOVType""='REC_MANPOWER' and mt.""Id"" = j.""ManpowerTypeId""
                            LEFT JOIN cms.""N_CoreHR_HRDepartment"" as o ON o.""Id"" = c.""OrganizationId"" 
                            LEFT JOIN cms.""N_REC_APPLICATION"" as app ON app.""JobId"" = c.""JobId"" and app.""IsDeleted""=false
                            left join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = app.""BatchId"" and b.""OrganizationId"" = c.""OrganizationId""
                            where c.""IsDeleted"" = false #FILTER#
                            group by c.""Id"",u.""Name"",j.""JobTitle"",o.""DepartmentName"",jd.""Id"",mt.""Name"",s.""Id""
                            ";

            //if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
            //{
            //    var orglist = await _hmBusiness.GetHODOrg(_repo.UserContext.UserId);
            //    var orgs = orglist.Select(x => x.Id);
            //    orgId = string.Join(",", orgs).TrimEnd(',');
            //    orgId = orgId.Replace(",", "','");
            //    filter = @$" and c.""OrganizationId"" in ('{orgId}') ";
            //}

            if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
            {
                var orgquery = $@"SELECT c.""OrganisationId"",j.""JobTitle"" as Name
                                from cms.""N_REC_HEAD_OF_DEPARTMENT"" as c
                                join cms.""N_CoreHR_HRJob"" as j on c.""DesignationId""=j.""Id"" and j.""IsDeleted""=false
                                WHERE c.""IsDeleted""=false and c.""UserId""='{_repo.UserContext.UserId}' ";

                var orgdata = await _queryIdName.ExecuteQuerySingle(orgquery, null);

                string orgId = orgdata.Id.Replace(",", "','");
                filter = @$" and c.""OrganizationId"" in ('{orgId}') ";
            }

            query = query.Replace("#FILTER#", filter);
            var list1 = await _queryMPRSummary.ExecuteQueryList(query, null);

            //string query4 = $@"SELECT c.""Id"",count(t) as Count
            //                FROM rec.""ManpowerRecruitmentSummary"" as c
            //                Left join public.""RecTask"" as t on t.""ReferenceTypeId""=c.""Id"" AND t.""ReferenceTypeCode""='76'
            //                where c.""IsDeleted"" = false 
            //                group by c.""Id"" ";

            //var list5 = await _queryRepo.ExecuteQueryList(query4, null);

            string query1 = @$"SELECT c.""Id"",c.""Id"" as JobId ,b.""OrganizationId"",                         
                          
                            (sum(case when aps.""Code""='ShortListByHr'  then 1 else 0 end) ) as ShortlistedByHr,
                            (sum(case when aps.""Code"" = 'ShortListByHm'  then 1 else 0 end)) as ShortlistedForInterview,
                            (sum(case when aps.""Code"" = 'DirectHiring'  then 1 else 0 end)) as DirectHiring,
                            (sum(case when aps.""Code"" = 'InterviewsCompleted' then 1 else 0 end) ) as InterviewCompleted,
                            --(sum(case when aps.""Code"" = 'FinalOfferAccepted' then 1 else 0 end) ) as FinalOfferAccepted,
                            --(sum(case when aps.""Code"" = 'Joined'  then 1 else 0 end)) as CandidateJoined
                            (sum(case when aps.""Code"" = 'IntentToOffer' then 1 else 0 end)) as IntentToOffer,
                            (sum(case when aps.""Code"" = 'FinalOffer' then 1 else 0 end)) as FinalOffer,
                            (sum(case when aps.""Code"" = 'FinalOfferAccepted' then 1 else 0 end)) as FinalOfferAccepted,
                            (sum(case when aps.""Code"" = 'VisaTransfer' then 1 else 0 end)) as VisaTransfer,
                            (sum(case when aps.""Code"" = 'BusinessVisa' then 1 else 0 end)) as BusinessVisa,
                            (sum(case when aps.""Code"" = 'VisaAppointmentTaken' then 1 else 0 end)) as VisaAppointmentTaken,
                            (sum(case when aps.""Code"" = 'BiometricCompleted' then 1 else 0 end)) as BiometricCompleted,
                            (sum(case when aps.""Code"" = 'VisaApproved' then 1 else 0 end)) as VisaApproved,
                            (sum(case when aps.""Code"" = 'WorkVisa' then 1 else 0 end)) as WorkerVisa,
                            (sum(case when aps.""Code"" = 'FlightTicketsBooked' then 1 else 0 end)) as FightTicketsBooked,
                            (sum(case when aps.""Code"" = 'CandidateArrived' then 1 else 0 end)) as CandidateArrived,
                            (sum(case when aps.""Code"" = 'StaffJoined' then 1 else 0 end)) as CandidateJoined  , 
                            (sum(case when aps.""Code"" = 'WorkerJoined' then 1 else 0 end)) as WorkerJoined ,
                            (sum(case when aps.""Code"" = 'WorkerPool' then 1 else 0 end)) as WorkerPool ,
                            (sum(case when aps.""Code"" = 'WorkPermit' then 1 else 0 end)) as WorkPermit ,
                            (sum(case when aps.""Code"" = 'PostWorkerJoined' then 1 else 0 end)) as PostWorkerJoined ,
                            (sum(case when aps.""Code"" = 'PostStaffJoined' then 1 else 0 end)) as PostStaffJoined ,
                            --(sum(case when aps.""Code"" = 'Joined' then 1 else 0 end)) as Joined 
                            (sum(case when (aps.""Code"" = 'PostWorkerJoined' or aps.""Code"" = 'PostStaffJoined' or aps.""Code"" = 'Joined') then 1 else 0 end)) as Joined 

                            from cms.""N_REC_JobAdvertisement"" as c 
                           
                             join cms.""N_REC_APPLICATION"" as ap on ap.""JobId"" = c.""Id"" and ap.""IsDeleted""=false                        
                             left join cms.""N_REC_REC_BATCH"" as b on b.""Id"" = ap.""BatchId"" and b.""IsDeleted""=false                         
                             left join public.""LOV"" as aps on aps.""Id"" = ap.""ApplicationStateId"" and aps.""LOVType""='APPLICATION_STATE' and aps.""IsDeleted""=false
                             --LEFT join public.""LOV"" as apst on apst.""Id"" = ap.""CurrentStatusId"" and apst.""LOVType""='APPLICATION_STATUS' and apst.""IsDeleted""=false
                            --where apst.""Code""!='REJECTED' and apst.""Code""!='RejectedHM' and apst.""Code""!='WAITLISTED'
                            where c.""Status""=1 and c.""IsDeleted"" = false and ap.""Id"" is not null and b.""OrganizationId"" is not null         
                            group by c.""Id"",b.""OrganizationId""
                            ";
            var list2 = await _queryMPRSummary.ExecuteQueryList(query1, null);

            string query3 = @$"SELECT c.""Id"",ja.""Id"" as JobAdvertisementId,                           
                            sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
                            sum(case when ja.""Status""=2 then 1 else 0 end) as InActive                          
                            FROM cms.""N_REC_ManpowerRequirement"" as c 
                            join cms.""N_REC_JobAdvertisement"" as ja on ja.""JobId""=c.""JobId"" where ja.""IsDeleted"" = false                           
                            group by c.""Id"",ja.""Id""
                            ";

            var list3 = await _queryMPRSummary.ExecuteQueryList(query3, null);

            //var list6 = from a in list1
            //            join b in list5
            //            on a.Id equals b.Id
            //            select new ManpowerRecruitmentSummaryViewModel
            //            {
            //                Id = a.Id,
            //                JobId = a.JobId,
            //                JobTitle = a.JobTitle,
            //                OrganizationId = a.OrganizationId,
            //                OrganizationName = a.OrganizationName,
            //                Requirement = a.Requirement,
            //                Seperation = a.Seperation,
            //                Available = a.Available,
            //                Planning = a.Planning,
            //                Transfer = a.Transfer,
            //                Balance = a.Balance,
            //                VersionNo = a.VersionNo,
            //                CreatedByName = a.CreatedByName,
            //                CreatedDate = a.CreatedDate,
            //                OrgUnit = a.OrgUnit,
            //                PlanningUnit = a.PlanningUnit,
            //                Hr = a.Hr,
            //                Count = b.Count,
            //                JobDescriptionId = a.JobDescriptionId,
            //                ManpowerType = a.ManpowerType,
            //            };

            var list4 = from a in list1
                        join b in list2
                        on new { X1 = a.JobId, X2 = a.OrganizationId } equals new { X1 = b.JobId, X2 = b.OrganizationId } into gj
                        //on a.Id equals b.Id into gj
                        from sub in gj.DefaultIfEmpty()
                        select new ManpowerRecruitmentSummaryViewModel
                        {
                            Id = a.Id,
                            JobId = a.JobId,
                            JobTitle = a.JobTitle,
                            JobDescriptionId = a.JobDescriptionId,
                            OrganizationId = a.OrganizationId,
                            OrganizationName = a.OrganizationName,
                            Requirement = a.Requirement,
                            Seperation = a.Seperation,
                            Available = a.Available,
                            SubContract = a.SubContract,
                            Transfer = a.Transfer,
                            Balance = a.Balance,
                            VersionNo = a.VersionNo,
                            CreatedByName = a.CreatedByName,
                            CreatedDate = a.CreatedDate,
                            OrgUnit = a.OrgUnit,
                            PlanningUnit = a.PlanningUnit,
                            Hr = a.Hr,
                            // Active = b.Active,
                            //InActive = b.InActive,
                            ShortlistedByHr = sub?.ShortlistedByHr ?? 0,
                            ShortlistedForInterview = sub?.ShortlistedForInterview ?? 0,
                            DirectHiring = sub?.DirectHiring ?? 0,
                            IntentToOffer = sub?.IntentToOffer ?? 0,
                            VisaTransfer = sub?.VisaTransfer ?? 0,
                            BusinessVisa = sub?.BusinessVisa ?? 0,
                            WorkerJoined = sub?.WorkerJoined ?? 0,
                            WorkPermit = sub?.WorkPermit ?? 0,
                            WorkerVisa = sub?.WorkerVisa ?? 0,
                            FinalOffer = sub?.FinalOffer ?? 0,
                            InterviewCompleted = sub?.InterviewCompleted ?? 0,
                            FinalOfferAccepted = sub?.FinalOfferAccepted ?? 0,
                            CandidateJoined = sub?.CandidateJoined ?? 0,
                            WorkerPool = sub?.WorkerPool ?? 0,
                            Joined = sub?.Joined ?? 0,
                            PostStaffJoined = sub?.PostStaffJoined ?? 0,
                            PostWorkerJoined = sub?.PostWorkerJoined ?? 0,
                            JobAdvertisementId = sub?.Id ?? null,
                            ManpowerType = a.ManpowerType,
                            ServiceId = a.ServiceId
                        };

            var list = from a in list4
                       join b in list3
                       on a.Id equals b.Id into gj
                       from sub in gj.DefaultIfEmpty()
                       select new ManpowerRecruitmentSummaryViewModel
                       {
                           Id = a.Id,
                           JobId = a.JobId,
                           JobTitle = a.JobTitle,
                           JobDescriptionId = a.JobDescriptionId,
                           OrganizationId = a.OrganizationId,
                           OrganizationName = a.OrganizationName,
                           Requirement = a.Requirement,
                           Seperation = a.Seperation,
                           Available = a.Available,
                           SubContract = a.SubContract,
                           Transfer = a.Transfer,
                           Balance = a.Balance,
                           VersionNo = a.VersionNo,
                           CreatedByName = a.CreatedByName,
                           CreatedDate = a.CreatedDate,
                           OrgUnit = a.OrgUnit,
                           PlanningUnit = a.PlanningUnit,
                           Hr = a.Hr,
                           Active = sub?.Active ?? 0,
                           InActive = sub?.InActive ?? 0,
                           ShortlistedByHr = a.ShortlistedByHr,
                           ShortlistedForInterview = a.ShortlistedForInterview,
                           DirectHiring = a.DirectHiring,
                           IntentToOffer = a.IntentToOffer,
                           VisaTransfer = a.VisaTransfer,
                           BusinessVisa = a.BusinessVisa,
                           WorkerJoined = a.WorkerJoined,
                           WorkPermit = a.WorkPermit,
                           WorkerVisa = a.WorkerVisa,
                           FinalOffer = a.FinalOffer,
                           InterviewCompleted = a.InterviewCompleted,
                           FinalOfferAccepted = a.FinalOfferAccepted,
                           CandidateJoined = a.CandidateJoined,
                           WorkerPool = a.WorkerPool,
                           Joined = a.Joined,
                           PostStaffJoined = a.PostStaffJoined,
                           PostWorkerJoined = a.PostWorkerJoined,
                           Count = a.Count,
                           JobAdvertisementId = sub?.JobAdvertisementId ?? null,
                           ManpowerType = a.ManpowerType,
                           ServiceId = a.ServiceId
                       };

            return list.ToList();
            //IList<ManpowerRecruitmentSummaryViewModel> newlist = new List<ManpowerRecruitmentSummaryViewModel>();
            //return newlist;
        }


        public async Task<CandidateProfileViewModel> UpdateCandidateProfileDetails(CandidateProfileViewModel model)
        {
            var query = $@"update cms.""N_REC_REC_CANDIDATE"" set ""UserId""='{model.UserId}' where ""Id""='{model.Id}'";

            var result = await _queryRepo.ExecuteQuerySingle<CandidateProfileViewModel>(query, null);
            return result;
        }

        public async Task<RecBatchViewModel> GetBatchDetailsById(string id)
        {
            var query = $@" select *,""NtsNoteId"" as BatchNoteId from cms.""N_REC_REC_BATCH"" where ""Id"" = '{id}' and ""IsDeleted"" = false ";

            var result = await _queryRepo.ExecuteQuerySingle<RecBatchViewModel>(query, null);
            return result;

        }

        public async Task UpdateApplicationStatusForInterview(dynamic udf, string statusId)
        {
            string query = @$"update cms.""N_REC_APPLICATION"" set ""ApplicationStatusId""='{statusId}' where ""Id"" = '{udf.ApplicationId}' ";
            await _queryRepo.ExecuteCommand(query, null);

        } 
        public async Task UpdateApplicationBatch(string applicationId, string statusId,string stateId,string batchId,string orgId,string jobId)
        {
            string query = @$"update cms.""N_REC_APPLICATION"" set ""ApplicationStatusId""='{statusId}', ""ApplicationStateId""='{stateId}'
            ,""BatchId""='{batchId}',""OrganisationId""='{orgId}',""JobId""='{jobId}'
            where ""Id"" = '{applicationId}' ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<List<ApplicationViewModel>> CandidateReportData()
        {
            string query = @$"Select app.""SourceFrom"" as ""SourceFrom"",au.""AgencyName"" as ""AgencyName"",
app.""AppliedDate"" as ""AppliedDate"",job.""JobTitle"" as ""JobName"", batch.""BatchName"" as ""BatchName"",
org.""DepartmentName"" as ""OrganizationName"",app.""ApplicationNo"" as ""ApplicationNo"",
apps.""Name"" as ""ApplicationStateName"", appStatus.""Name"" as ""ApplicationStatus"", app.""FirstName"" as ""FirstName"", app.""MiddleName"" as ""MiddleName"",app.""LastName"" as ""LastName"",
app.""Email"" as ""Email"", TRUNC(app.""TotalWorkExperience""::decimal, 0) as ""TotalWorkExperience"", app.""ContactPhoneHome"" as ""ContactPhoneHome"",
app.""Score"" as ""Score"",lov.""Name"" as ""Gender"",app.""BirthDate"" as ""BirthDate"",app.""Age"" as ""Age"", app.""BirthPlace"" as ""BirthPlace"",
n.""NationalityName"" as ""Nationality"", app.""BloodGroup"" as ""BloodGroup"",mar.""Name"" as ""MaritalStatusName"",
app.""PassportNumber"" as ""PassportNumber"", pic.""CountryName"" as ""PassportIssueCountry"",app.""PassportExpiryDate"" as ""PassportExpiryDate"",
app.""QatarId"", app.""CurrentAddressHouse"" as ""CurrentAddressHouse"",
 app.""CurrentAddressStreet"" as ""CurrentAddressStreet"", app.""CurrentAddressCity"" as ""CurrentAddressCity"",
 app.""CurrentAddressState"" as ""CurrentAddressState"", cac.""CountryName"" as ""CurrentAddressCountryName"",
 app.""PermanentAddressHouse"" as ""PermanentAddressHouse"", app.""PermanentAddressStreet"" as ""PermanentAddressStreet"",
 app.""PermanentAddressCity"" as ""PermanentAddressCity"", app.""PermanentAddressState"" as ""PermanentAddressState"",
 pac.""CountryName"" as ""PermanentAddressCountryName"", app.""ContactPhoneLocal"" as ""ContactPhoneLocal"",curr.""Name"" as ""SalaryCurrencyName"",
 app.""NetSalary"" as ""NetSalary"", app.""OtherAllowances"" as ""OtherAllowances""
FROM cms.""N_REC_APPLICATION"" as app
                            left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
                            left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
                            left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
                            left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
                           left join  public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
                            left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId""
                            left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
                            left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategoryId""
                            LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
                            left JOIN  public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 
                           left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=app.""OrganisationId"" 
                            left join  public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""                          
                            left join cms.""N_REC_AGENCY"" as au on  au.""UserId"" = app.""AgencyId""
                            where app.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
             return queryData;
        }
        public async Task<List<ApplicationViewModel>> PendingforHM()
        {
            string query = @$"Select job.""JobTitle"" as ""JobName"", batch.""BatchName"" as ""BatchName"",hm.""Name"" as ""HiringManagerName"",
org.""DepartmentName"" as ""OrganizationName"",app.""ApplicationNo"" as ""ApplicationNo"",
apps.""Name"" as ""ApplicationStateName"", appStatus.""Name"" as ""ApplicationStatus"", app.""FirstName"" as ""FirstName"", app.""MiddleName"" as ""MiddleName"",app.""LastName"" as ""LastName"",
app.""Email"" as ""Email"", TRUNC(app.""TotalWorkExperience""::decimal, 0) as ""TotalWorkExperience"", app.""ContactPhoneHome"" as ""ContactPhoneHome"",
app.""Score"" as ""Score"",lov.""Name"" as ""Gender"",app.""BirthDate"" as ""BirthDate"",app.""Age"" as ""Age"", app.""BirthPlace"" as ""BirthPlace"",
n.""NationalityName"" as ""Nationality"", app.""BloodGroup"" as ""BloodGroup"",mar.""Name"" as ""MaritalStatusName"",
app.""PassportNumber"" as ""PassportNumber"", pic.""CountryName"" as ""PassportIssueCountry"",app.""PassportExpiryDate"" as ""PassportExpiryDate"",
app.""QatarId"", app.""CurrentAddressHouse"" as ""CurrentAddressHouse"",
 app.""CurrentAddressStreet"" as ""CurrentAddressStreet"", app.""CurrentAddressCity"" as ""CurrentAddressCity"",
 app.""CurrentAddressState"" as ""CurrentAddressState"", cac.""CountryName"" as ""CurrentAddressCountryName"",
 app.""PermanentAddressHouse"" as ""PermanentAddressHouse"", app.""PermanentAddressStreet"" as ""PermanentAddressStreet"",
 app.""PermanentAddressCity"" as ""PermanentAddressCity"", app.""PermanentAddressState"" as ""PermanentAddressState"",
 pac.""CountryName"" as ""PermanentAddressCountryName"", app.""ContactPhoneLocal"" as ""ContactPhoneLocal"",curr.""Name"" as ""SalaryCurrencyName"",
 app.""NetSalary"" as ""NetSalary"", app.""OtherAllowances"" as ""OtherAllowances""
FROM cms.""N_REC_APPLICATION"" as app
                            left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
                            left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
                            left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
                            left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
                           left join  public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
                            left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId""
                            left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
                            left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategoryId""
                            LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
                            left JOIN  public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 
                           left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=app.""OrganisationId"" 
                            left join  public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""                          
                            left join cms.""N_REC_AGENCY"" as au on  au.""UserId"" = app.""AgencyId""
							left join cms.""N_REC_HIRING_MANAGER"" as hm on  hm.""UserId"" = batch.""HiringManager""
                            where app.""IsDeleted""=false and (appStatus.""Code"" = 'NotShortlisted' or appStatus.""Code"" = 'ShortlistedHM' or appStatus.""Code"" = 'InterviewRequested')";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ApplicationViewModel>> RejectedforHM()
        {
            string query = @$"Select job.""JobTitle"" as ""JobName"", batch.""BatchName"" as ""BatchName"",hm.""Name"" as ""HiringManagerName"",
org.""DepartmentName"" as ""OrganizationName"",app.""ApplicationNo"" as ""ApplicationNo"",
apps.""Name"" as ""ApplicationStateName"", appStatus.""Name"" as ""ApplicationStatus"", app.""FirstName"" as ""FirstName"", app.""MiddleName"" as ""MiddleName"",app.""LastName"" as ""LastName"",
app.""Email"" as ""Email"", TRUNC(app.""TotalWorkExperience""::decimal, 0) as ""TotalWorkExperience"", app.""ContactPhoneHome"" as ""ContactPhoneHome"",
app.""Score"" as ""Score"",lov.""Name"" as ""Gender"",app.""BirthDate"" as ""BirthDate"",app.""Age"" as ""Age"", app.""BirthPlace"" as ""BirthPlace"",
n.""NationalityName"" as ""Nationality"", app.""BloodGroup"" as ""BloodGroup"",mar.""Name"" as ""MaritalStatusName"",
app.""PassportNumber"" as ""PassportNumber"", pic.""CountryName"" as ""PassportIssueCountry"",app.""PassportExpiryDate"" as ""PassportExpiryDate"",
app.""QatarId"", app.""CurrentAddressHouse"" as ""CurrentAddressHouse"",
 app.""CurrentAddressStreet"" as ""CurrentAddressStreet"", app.""CurrentAddressCity"" as ""CurrentAddressCity"",
 app.""CurrentAddressState"" as ""CurrentAddressState"", cac.""CountryName"" as ""CurrentAddressCountryName"",
 app.""PermanentAddressHouse"" as ""PermanentAddressHouse"", app.""PermanentAddressStreet"" as ""PermanentAddressStreet"",
 app.""PermanentAddressCity"" as ""PermanentAddressCity"", app.""PermanentAddressState"" as ""PermanentAddressState"",
 pac.""CountryName"" as ""PermanentAddressCountryName"", app.""ContactPhoneLocal"" as ""ContactPhoneLocal"",curr.""Name"" as ""SalaryCurrencyName"",
 app.""NetSalary"" as ""NetSalary"", app.""OtherAllowances"" as ""OtherAllowances""
FROM cms.""N_REC_APPLICATION"" as app
                            left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
                            left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
                            left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
                            left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
                           left join  public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
                            left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId""
                            left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
                            left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategoryId""
                            LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
                            left JOIN  public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 
                           left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=app.""OrganisationId"" 
                            left join  public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""                          
                            left join cms.""N_REC_AGENCY"" as au on  au.""UserId"" = app.""AgencyId""
							left join cms.""N_REC_HIRING_MANAGER"" as hm on  hm.""UserId"" = batch.""HiringManager""
                            where app.""IsDeleted""=false and appStatus.""Code"" = 'RejectedHM' ";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ApplicationViewModel>> ShortlistforFuture()
        {
            string query = @$"Select job.""JobTitle"" as ""JobName"", batch.""BatchName"" as ""BatchName"",hm.""Name"" as ""HiringManagerName"",
org.""DepartmentName"" as ""OrganizationName"",app.""ApplicationNo"" as ""ApplicationNo"",
apps.""Name"" as ""ApplicationStateName"", appStatus.""Name"" as ""ApplicationStatus"", app.""FirstName"" as ""FirstName"", app.""MiddleName"" as ""MiddleName"",app.""LastName"" as ""LastName"",
app.""Email"" as ""Email"", TRUNC(app.""TotalWorkExperience""::decimal, 0) as ""TotalWorkExperience"", app.""ContactPhoneHome"" as ""ContactPhoneHome"",
app.""Score"" as ""Score"",lov.""Name"" as ""Gender"",app.""BirthDate"" as ""BirthDate"",app.""Age"" as ""Age"", app.""BirthPlace"" as ""BirthPlace"",
n.""NationalityName"" as ""Nationality"", app.""BloodGroup"" as ""BloodGroup"",mar.""Name"" as ""MaritalStatusName"",
app.""PassportNumber"" as ""PassportNumber"", pic.""CountryName"" as ""PassportIssueCountry"",app.""PassportExpiryDate"" as ""PassportExpiryDate"",
app.""QatarId"", app.""CurrentAddressHouse"" as ""CurrentAddressHouse"",
 app.""CurrentAddressStreet"" as ""CurrentAddressStreet"", app.""CurrentAddressCity"" as ""CurrentAddressCity"",
 app.""CurrentAddressState"" as ""CurrentAddressState"", cac.""CountryName"" as ""CurrentAddressCountryName"",
 app.""PermanentAddressHouse"" as ""PermanentAddressHouse"", app.""PermanentAddressStreet"" as ""PermanentAddressStreet"",
 app.""PermanentAddressCity"" as ""PermanentAddressCity"", app.""PermanentAddressState"" as ""PermanentAddressState"",
 pac.""CountryName"" as ""PermanentAddressCountryName"", app.""ContactPhoneLocal"" as ""ContactPhoneLocal"",curr.""Name"" as ""SalaryCurrencyName"",
 app.""NetSalary"" as ""NetSalary"", app.""OtherAllowances"" as ""OtherAllowances""
FROM cms.""N_REC_APPLICATION"" as app
                            left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCountry"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
                            left join cms.""N_CoreHR_HRCountry"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
                            left join cms.""N_CoreHR_HRNationality"" as n on n.""Id"" = app.""NationalityId""
                            left join cms.""N_REC_JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
                            left join cms.""N_CoreHR_HRCurrency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""
                            left join public.""LOV"" as mar on mar.""Id"" = app.""MaritalStatusId""
                           left join  public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
                            left join cms.""N_REC_REC_CANDIDATE"" as c ON c.""Id""=app.""CandidateId""
                            left join public.""LOV"" as lov on lov.""Id""=app.""GenderId""
                            left join public.""LOV"" as vt on vt.""Id""=app.""VisaCategoryId""
                            LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
                            left JOIN  public.""LOV"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 
                           left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=app.""OrganisationId"" 
                            left join  public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""                          
                            left join cms.""N_REC_AGENCY"" as au on  au.""UserId"" = app.""AgencyId""
							left join cms.""N_REC_HIRING_MANAGER"" as hm on  hm.""UserId"" = batch.""HiringManager""
                            where app.""IsDeleted""=false and appStatus.""Code"" = 'ShortlistForFuture'";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ApplicationViewModel>> PendingforUser()
        {
            string query = @$"Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_InterviewEvaluation"" as ie on ie.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on ie.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
 left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_StaffAppointmentrequest"" as ito on ito.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on ito.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
 left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_WorkerAppointmentrequestapproval"" as war on war.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on war.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
 left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
  left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_FINAL_OFFER"" as fo on fo.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on fo.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
  left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_OverseasBusinessVISA"" as bv on bv.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on bv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
  left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_OverseasworkVisa"" as wv on wv.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on wv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id""
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and(ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
  left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_LOCAL_VISA_TRANSFER"" as vt on vt.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on vt.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and(ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
  left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_LocalWorkPermit"" as lwp on lwp.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on lwp.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and(ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
  left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_Staff_Joining"" as sjf on sjf.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on sjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id""
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and(ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_WF_WORKER_JOINING"" as wjf on wjf.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on wjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId"" 
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
  union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_Travelling"" as t on t.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on t.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_ShortlistByHM"" as hm on hm.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on hm.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_ShortlistByHR"" as hr on hr.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on hr.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 union
Select job.""JobTitle"" as PositionName, batch.""BatchName"" as BatchName, org.""DepartmentName"" as OrganizationName, 
apps.""Name"" as ApplicationStateName,au.""Name"" as AssignedUser
,appStatus.""Name"" as ApplicationStatusName,ts.""Name"" as TaskStatus
FROM cms.""N_REC_APPLICATION"" as app
left join public.""LOV"" as apps on apps.""Id"" = app.""ApplicationStateId""
left join public.""LOV"" as appStatus on appStatus.""Id"" = app.""ApplicationStatusId""
left join cms.""N_CoreHR_HRJob"" as job on job.""Id""=app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join cms.""N_REC_DIRECT_HIRING"" as DH on DH.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on DH.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
 left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
where app.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<List<ApplicationViewModel>> PendingforHR()
    {
        string query = @$"Select au.""Name"" as AssignedUser,job.""JobTitle"" as JobName,org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo,task.""TaskNo"" as TaskNo,task.""TaskSubject"" as TaskSubject,ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id"" = app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id"" = batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_InterviewEvaluation"" as ie on ie.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on ie.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_StaffAppointmentrequest"" as ito on ito.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on ito.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_WorkerAppointmentrequestapproval"" as war on war.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on war.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_FINAL_OFFER"" as fo on fo.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on fo.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_OverseasBusinessVISA"" as bv on bv.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on bv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_OverseasworkVisa"" as wv on wv.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on wv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_LOCAL_VISA_TRANSFER"" as vt on vt.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on vt.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_LocalWorkPermit"" as lwp on lwp.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on lwp.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_Staff_Joining"" as sjf on sjf.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on sjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_WF_WORKER_JOINING"" as wjf on wjf.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on wjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_Travelling"" as t on t.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on t.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_ShortlistByHM"" as hm on hm.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on hm.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_ShortlistByHR"" as hr on hr.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on hr.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_DIRECT_HIRING"" as DH on DH.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on DH.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='HR'";

        var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
        return queryData;
    }
        public async Task<List<ApplicationViewModel>> PendingforED()
        {
            string query = @$"Select au.""Name"" as AssignedUser,job.""JobTitle"" as JobName,org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo,task.""TaskNo"" as TaskNo,task.""TaskSubject"" as TaskSubject,ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id"" = app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id"" = batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_InterviewEvaluation"" as ie on ie.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on ie.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_StaffAppointmentrequest"" as ito on ito.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on ito.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_WorkerAppointmentrequestapproval"" as war on war.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on war.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_FINAL_OFFER"" as fo on fo.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on fo.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_OverseasBusinessVISA"" as bv on bv.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on bv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_OverseasworkVisa"" as wv on wv.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on wv.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_LOCAL_VISA_TRANSFER"" as vt on vt.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on vt.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_LocalWorkPermit"" as lwp on lwp.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on lwp.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_Staff_Joining"" as sjf on sjf.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on sjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_WF_WORKER_JOINING"" as wjf on wjf.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on wjf.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_Travelling"" as t on t.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on t.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_ShortlistByHM"" as hm on hm.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on hm.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_ShortlistByHR"" as hr on hr.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on hr.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'
union
Select au.""Name"" as AssignedUser, job.""JobTitle"" as JobName, org.""DepartmentName"" as OrganisationName,
apps.""Name"" as ApplicationStateName, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo, task.""TaskNo"" as TaskNo, task.""TaskSubject"" as TaskSubject, ts.""Name"" as TaskStatus,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Email"" as Email
 FROM cms.""N_REC_APPLICATION"" as app
left join cms.""N_CoreHR_HRJob"" as job on job.""Id"" = app.""JobId""
LEFT Join cms.""N_REC_REC_BATCH"" as batch on batch.""Id""=app.""BatchId""
left join cms.""N_CoreHR_HRDepartment"" as org ON org.""Id""=batch.""OrganizationId""
left join public.""LOV"" as appStatus on appStatus.""Id""= app.""ApplicationStatusId""
left join public.""LOV"" as apps on apps.""Id""=app.""ApplicationStateId""
left join cms.""N_REC_DIRECT_HIRING"" as DH on DH.""ApplicationId""=app.""Id""
left join public.""NtsService"" as s on DH.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
left join public.""NtsTask"" as task on  task.""ParentServiceId"" = s.""Id"" 
 left join public.""LOV"" as ts on task.""TaskStatusId""=ts.""Id"" and ts.""IsDeleted""=false and (ts.""Code""='TASK_STATUS_INPROGRESS' OR ts.""Code""='TASK_STATUS_OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssignedToUserId""
left join public.""UserRoleUser"" as uru on uru.""UserId""=au.""Id""
left join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
where app.""IsDeleted""=false and ur.""Code""='ED'";

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }


        public async Task<RecApplicationViewModel> GetApplicationForJobAdv(string applicationId)
        {
            string query = $@"select pl.*,b.""OrganizationId"" as OrganizationId, b.""HiringManager"" as HiringManagerId,b.""CreatedBy"" as RecruiterId
                                FROM cms.""N_REC_APPLICATION"" as pl
                                LEFT JOIN cms.""N_REC_REC_BATCH"" as b on b.""Id"" = pl.""BatchId""
                                --LEFT JOIN rec.""HiringManager"" as hm on hm.""UserId"" = b.""HiringManager""
                                WHERE pl.""Id"" = '{applicationId}' and pl.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQuerySingle<RecApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task UpdateJobAdvertisementStatus(string serviceId)
        {
            string query = $@"select pl.""JobAdvertisementId"" as Id
                                FROM cms.""F_CRPF_RECRUITMENT_SelectedJobAdvertisement"" as pl
                              WHERE pl.""VacancyListId"" = '{serviceId}' and pl.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var complete = await _lovBusiness.GetSingle(x => x.Code == "SERVICE_STATUS_COMPLETE");
            if (queryData.Count > 0)
            {
                var ides = queryData.Select(x => x.Id).ToList();
                var id = string.Join("','", ides);

                string query1 = @$"update public.""NtsService"" set ""ServiceStatusId""='{complete.Id}' where ""Id"" in ('{id}') ";
                await _queryRepo.ExecuteCommand(query1, null);
            }

        }

        public async Task<IList<RecApplicationViewModel>> GetJobApplicationList()
        {
            var query = $@" select cp.*,j.""JobTitle"" as JobName
                            from cms.""N_REC_APPLICATION"" as cp
                             left join cms.""N_CoreHR_HRJob"" as j on  cp.""JobId"" =j.""Id""
                            left join cms.""N_REC_CRPFApplicationCollectionCentre"" as ac on  cp.""Id"" =ac.""ApplicationId""
                            where cp.""IsDeleted"" = false and ac.""Id"" is null";

            var queryData = await _queryRepo.ExecuteQueryList<RecApplicationViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<IdNameViewModel>> GetExamCenter()
        {
            string query = @$"SELECT ""Id"",""Name""
                                FROM cms.""F_CRPF_RECRUITMENT_ExamCenter"" where ""IsDeleted""=false";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<MedFitCertificateViewModel> GetMedicalFitnessData(string appId)
        {
            var query = $@" select *
                            from cms.""F_CRPF_RECRUITMENT_MedicalFitnessCertificate"" 
                            where ""ApplicationId"" = '{appId}' and ""IsDeleted"" = false ";

            var queryData = await _queryRepo.ExecuteQuerySingle<MedFitCertificateViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<ApplicationViewModel>> GetResultData(string eId)
        {
            var query = $@" select a.""Id"",a.""FirstName"",a.""ApplicationNo"",acc.""IsWrittenPass"",acc.""IsPhysicalPass"",ac.""ExamCenter""
from cms.""N_REC_APPLICATION"" as a
left join cms.""N_REC_CRPFApplicationCollectionCentre"" as acc on acc.""ApplicationId"" = a.""Id"" and acc.""IsDeleted"" = false
left join cms.""N_REC_AdmitCard"" as ac on ac.""ApplicationId"" = a.""Id"" and ac.""IsDeleted"" = false
where acc.""IsWrittenPass"" = 'True' and acc.""IsPhysicalPass"" = 'True' and a.""IsDeleted"" = false #EXAMWHERE# ";
            var exam = "";
            if (eId.IsNotNullAndNotEmpty())
            {
                exam = $@" and ac.""ExamCenter""='{eId}'";
            }
            query = query.Replace("#EXAMWHERE#", exam);

            var queryData = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return queryData;
        }

    }
}
