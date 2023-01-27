using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ManpowerRecruitmentSummaryBusiness : BusinessBase<ManpowerRecruitmentSummaryViewModel, ManpowerRecruitmentSummary>, IManpowerRecruitmentSummaryBusiness
    {
        private IUserBusiness _userBusiness;
        private IUserRoleBusiness _userroleBusiness;
        private IRepositoryBase<ManpowerRecruitmentSummaryVersionViewModel, ManpowerRecruitmentSummaryVersion> _repoVersion;
        private readonly IRepositoryQueryBase<ManpowerRecruitmentSummaryViewModel> _queryRepo;        
        private readonly IRepositoryQueryBase<JobAdvertisementViewModel> _queryRepoJob;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepoIdName;
        private readonly IRepositoryQueryBase<RecTaskViewModel> _queryTask;
        private readonly IHiringManagerBusiness _hmBusiness;
        public ManpowerRecruitmentSummaryBusiness(IRepositoryBase<ManpowerRecruitmentSummaryViewModel, ManpowerRecruitmentSummary> repo, IMapper autoMapper,
            IUserBusiness userBusiness, IUserRoleBusiness userroleBusiness, IRepositoryBase<ManpowerRecruitmentSummaryVersionViewModel, ManpowerRecruitmentSummaryVersion> repoVersion,
            IRepositoryQueryBase<ManpowerRecruitmentSummaryViewModel> queryRepo, IRepositoryQueryBase<RecTaskViewModel> queryTask
            ,IRepositoryQueryBase<JobAdvertisementViewModel> queryRepoJob, IRepositoryQueryBase<IdNameViewModel> queryRepoIdName
            , IHiringManagerBusiness hmBusiness) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;
            _repoVersion = repoVersion;
            _queryRepo = queryRepo;
            _queryRepoJob = queryRepoJob;
            _queryRepoIdName = queryRepoIdName;
            _userroleBusiness = userroleBusiness;
            _queryTask = queryTask;
            _hmBusiness = hmBusiness;
        }

        public async override Task<CommandResult<ManpowerRecruitmentSummaryViewModel>> Create(ManpowerRecruitmentSummaryViewModel model)
        {

            //var data = _autoMapper.Map<UserRoleViewModel>(model);
            model.VersionNo = 0;
            model.Balance = (model.Requirement.IsNotNull() ? model.Requirement : 0) - ((model.Available.IsNotNull() ? model.Available : 0) + (model.Transfer.IsNotNull() ? model.Transfer : 0) + (model.Planning.IsNotNull() ? model.Planning : 0) + (model.Seperation.IsNotNull() ? model.Seperation : 0));
            var record = await _repo.GetSingle(x => x.OrganizationId == model.OrganizationId && x.JobId == model.JobId);
            if (record!=null) 
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given Job with Organization already exists. Please select another option");
                return CommandResult<ManpowerRecruitmentSummaryViewModel>.Instance(model, false, obj);
            }
            var result = await base.Create(model);
            return CommandResult<ManpowerRecruitmentSummaryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ManpowerRecruitmentSummaryViewModel>> Edit(ManpowerRecruitmentSummaryViewModel model)
        {
            var existingData = GetSingleById(model.Id).Result;
            if(model.Requirement!=existingData.Requirement || model.Seperation != existingData.Seperation || model.Available !=existingData.Available || model.Planning != existingData.Planning || model.Transfer != existingData.Transfer)
            {
                var data = new ManpowerRecruitmentSummaryVersionViewModel
                {
                    ManpowerRecruitmentSummaryId = existingData.Id,
                    JobId= existingData.JobId,
                    OrganizationId= existingData.OrganizationId,
                    Requirement= existingData.Requirement,
                    Seperation= existingData.Seperation,
                    Available= existingData.Available,
                    Planning= existingData.Planning,
                    Transfer= existingData.Transfer,
                    VersionNo=existingData.VersionNo,
                    Balance= existingData.Balance
                };                
                await _repoVersion.Create(data);
                model.VersionNo += 1;
            }

            model.Balance = (model.Requirement.IsNotNull() ? model.Requirement : 0) - ((model.Available.IsNotNull() ? model.Available : 0) + (model.Transfer.IsNotNull() ? model.Transfer : 0) + (model.Planning.IsNotNull() ? model.Planning : 0) + (model.Seperation.IsNotNull() ? model.Seperation : 0));
            var record = await _repo.GetSingle(x => x.OrganizationId == model.OrganizationId && x.JobId == model.JobId && x.Id !=model.Id);
            if (record != null)
            {
                Dictionary<string, string> obj = new Dictionary<string, string>();
                obj.Add("NameExist", "The given Job with Organization already exists. Please select another option");
                return CommandResult<ManpowerRecruitmentSummaryViewModel>.Instance(model, false, obj);
            }
            var result = await base.Edit(model);
            return CommandResult<ManpowerRecruitmentSummaryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentSummaryData()
        {
            var orgId = "";
            var filter = "";

            string query = @$"SELECT c.*,jd.""Id"" as JobDescriptionId,mt.""Name"" as ""ManpowerType"",
                            sum(case when ur.""Code""='ORG_UNIT' and c.""Id"" is not null then 1 else 0 end) as OrgUnit,
                            sum(case when ur.""Code"" = 'PLANNING' and c.""Id"" is not null then 1 else 0 end) as PlanningUnit,
                            sum(case when ur.""Code"" = 'HR' and c.""Id"" is not null then 1 else 0 end) as Hr,
                            u.""Name"" as CreatedByName,j.""Name"" as JobTitle,o.""Name"" as OrganizationName
                            FROM rec.""ManpowerRecruitmentSummary"" as c
                            left join rec.""JobDescription"" as jd on jd.""JobId""=c.""JobId""
                            LEFT JOIN rec.""ManpowerSummaryComment"" as q ON q.""ManpowerRecruitmentSummaryId"" = c.""Id""
                            LEFT JOIN public.""UserRole"" as ur ON ur.""Id"" = q.""UserRoleId""
                            LEFT JOIN public.""User"" as u ON u.""Id"" = c.""CreatedBy""
                            LEFT JOIN cms.""Job"" as j ON j.""Id"" = c.""JobId""
                             left join rec.""ListOfValue"" as mt on mt.""ListOfValueType""='LOV_MANPOWERTYPE' and mt.""Code"" = j.""ManpowerTypeCode""
                            LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId"" 
                            --Left join public.""RecTask"" as t on t.""ReferenceTypeId""=c.""Id"" AND t.""ReferenceTypeCode""='76'
                            where c.""IsDeleted"" = false #FILTER#
                            group by c.""Id"",u.""Name"",j.""Name"",o.""Name"",jd.""Id"",mt.""Name""
                            ";

            if (_repo.UserContext.UserRoleCodes.Contains("ORG_UNIT"))
            {
                var orglist = await _hmBusiness.GetHODOrg(_repo.UserContext.UserId);
                var orgs = orglist.Select(x=>x.Id);
                orgId = string.Join(",", orgs).TrimEnd(',');
                orgId = orgId.Replace(",", "','");
                filter = @$" and c.""OrganizationId"" in ('{orgId}') ";
            }
            query = query.Replace("#FILTER#", filter);
            var list1 = await _queryRepo.ExecuteQueryList(query, null);

            string query4 = $@"SELECT c.""Id"",count(t) as Count
                            FROM rec.""ManpowerRecruitmentSummary"" as c
                            Left join public.""RecTask"" as t on t.""ReferenceTypeId""=c.""Id"" AND t.""ReferenceTypeCode""='76'
                            where c.""IsDeleted"" = false 
                            group by c.""Id"" ";

            var list5 = await _queryRepo.ExecuteQueryList(query4, null);
            //string query1 = @$"SELECT c.""Id"",                           

            //                (sum(case when aps.""Code""='ShortListByHr' and ja.""Status""=1 then 1 else 0 end) ) as ShortlistedByHr,
            //                (sum(case when aps.""Code"" = 'ShortListByHm' and ja.""Status""=1 then 1 else 0 end)) as ShortlistedForInterview,
            //                (sum(case when aps.""Code"" = 'InterviewsCompleted' and ja.""Status""=1 then 1 else 0 end) ) as InterviewCompleted,
            //                (sum(case when aps.""Code"" = 'FinalOfferAccepted' and ja.""Status""=1 then 1 else 0 end) ) as FinalOfferAccepted,
            //                (sum(case when aps.""Code"" = 'Joined' and ja.""Status""=1 then 1 else 0 end)) as CandidateJoined                            
            //                FROM rec.""ManpowerRecruitmentSummary"" as c 
            //                LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
            //                LEFT JOIN rec.""Batch"" as b ON b.""OrganizationId"" = o.""Id""
            //                LEFT JOIN rec.""JobAdvertisement"" as ja ON ja.""Id"" = b.""JobAdvertisementId"" and ja.""Status""=1
            //                LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" =ja.""Id""
            //                LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState"" where c.""IsDeleted"" = false
            //                group by c.""Id""
            //                ";
            string query1 = @$"SELECT c.""Id"",c.""JobId"" ,b.""OrganizationId"",                         
                          
                            (sum(case when aps.""Code""='ShortListByHr'  then 1 else 0 end) ) as ShortlistedByHr,
                            (sum(case when aps.""Code"" = 'ShortListByHm'  then 1 else 0 end)) as ShortlistedForInterview,
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



                            from rec.""JobAdvertisement"" as c 
                           
                             left join rec.""Application"" as ap on ap.""JobId"" = c.""JobId""                            
                             left join rec.""Batch"" as b on b.""Id"" = ap.""BatchId"" and b.""OrganizationId""=ap.""OrganizationId""                           
                             left join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState"" 
                            where c.""Status""=1 and c.""IsDeleted"" = false
                            group by c.""Id"",b.""OrganizationId""
                            ";
            var list2 = await _queryRepo.ExecuteQueryList(query1, null);

            string query3 = @$"SELECT c.""Id"",                           
                            sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
                            sum(case when ja.""Status""=2 then 1 else 0 end) as InActive                          
                            FROM rec.""ManpowerRecruitmentSummary"" as c 
                            join rec.""JobAdvertisement"" as ja on ja.""JobId""=c.""JobId"" where ja.""IsDeleted"" = false                           
                            group by c.""Id""
                            ";
           
            var list3 = await _queryRepo.ExecuteQueryList(query3, null);

            var list6 = from a in list1
                        join b in list5
                        on a.Id equals b.Id
                        select new ManpowerRecruitmentSummaryViewModel
                        {
                            Id = a.Id,
                            JobId = a.JobId,
                            JobTitle = a.JobTitle,
                            OrganizationId = a.OrganizationId,
                            OrganizationName = a.OrganizationName,
                            Requirement = a.Requirement,
                            Seperation = a.Seperation,
                            Available = a.Available,
                            Planning = a.Planning,
                            Transfer = a.Transfer,
                            Balance = a.Balance,
                            VersionNo = a.VersionNo,
                            CreatedByName = a.CreatedByName,
                            CreatedDate = a.CreatedDate,
                            OrgUnit = a.OrgUnit,
                            PlanningUnit = a.PlanningUnit,
                            Hr = a.Hr,
                            Count = b.Count,
                            JobDescriptionId = a.JobDescriptionId,
                            ManpowerType = a.ManpowerType,
                        };





            var list4 = from a in list6
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
                            Planning = a.Planning,
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
                            ShortlistedByHr = sub?.ShortlistedByHr??0,
                            ShortlistedForInterview = sub?.ShortlistedForInterview??0,
                            IntentToOffer=sub?.IntentToOffer??0,
                            VisaTransfer=sub?.VisaTransfer??0,
                            BusinessVisa=sub?.BusinessVisa??0,
                            WorkerJoined=sub?.WorkerJoined??0,
                            WorkPermit=sub?.WorkPermit??0,
                            WorkerVisa=sub?.WorkerVisa??0,
                            FinalOffer=sub?.FinalOffer??0,
                            InterviewCompleted = sub?.InterviewCompleted??0,
                            FinalOfferAccepted = sub?.FinalOfferAccepted??0,
                            CandidateJoined = sub?.CandidateJoined??0,
                            WorkerPool=sub?.WorkerPool??0,
                            Joined = sub?.Joined??0,
                            PostStaffJoined = sub?.PostStaffJoined ?? 0,
                            PostWorkerJoined = sub?.PostWorkerJoined ?? 0,
                            Count = a.Count,
                            JobAdvertisementId = sub?.Id ?? null,
                           ManpowerType = a.ManpowerType,
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
                           Planning = a.Planning,
                           Transfer = a.Transfer,
                           Balance = a.Balance,
                           VersionNo = a.VersionNo,
                           CreatedByName = a.CreatedByName,
                           CreatedDate = a.CreatedDate,
                           OrgUnit = a.OrgUnit,
                           PlanningUnit = a.PlanningUnit,
                           Hr = a.Hr,
                           Active = sub?.Active??0,
                           InActive = sub?.InActive??0,
                           ShortlistedByHr = a.ShortlistedByHr,
                           ShortlistedForInterview = a.ShortlistedForInterview,
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
                           JobAdvertisementId = a.JobAdvertisementId,
                           ManpowerType = a.ManpowerType,
                       };



            return list.ToList();
        }
        #region previous Query
        //public async Task<RecruitmentDashboardViewModel> GetManpowerRecruitmentSummaryByOrgJob(string organizationId, string jobAdvId)
        //{
        //    string query = @$"SELECT c.""JobId"" as JobId,
        //                    sum(c.""Requirement"") as Requirement,
        //                    sum(c.""Available"") as Available,
        //                    sum(c.""Planning"") as Planning,
        //                    sum(c.""Transfer"") as Transfer,
        //                    sum(c.""Balance"") as Balance
        //                    FROM rec.""ManpowerRecruitmentSummary"" as c
        //                    LEFT JOIN rec.""JobAdvertisement"" as ja ON ja.""JobId""=c.""JobId""
        //                    #WHERE#
        //                    group by c.""JobId""
        //                    ";
        //    var where = @$" WHERE ja.""Id""='{jobAdvId}' ";

        //    if (organizationId.IsNotNullAndNotEmpty())
        //    {
        //        where = @$" WHERE ja.""Id""='{jobAdvId}' AND c.""OrganizationId""='{organizationId}' ";
        //    }
        //    query = query.Replace("#WHERE#", where);
        //    var list1 = await _queryRepo.ExecuteQueryList(query, null);
        //    string query1 = @$"SELECT ja.""Id"" as Id,ja.""JobId"" as JobId,                            
        //                    sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
        //                    sum(case when ja.""Status"" = 2 then 1 else 0 end) as InActive,
        //                    (sum(case when aps.""Code""='UnReviewed' then 1 else 0 end)) as UnReviewed,
        //                    (sum(case when aps.""Code""='ShortListByHr' then 1 else 0 end)) as ShortlistedByHr,
        //                    (sum(case when aps.""Code"" = 'ShortListByHm' then 1 else 0 end)) as ShortlistedForInterview,
        //                    (sum(case when aps.""Code"" = 'InterviewsCompleted' then 1 else 0 end)) as InterviewCompleted,
        //                    (sum(case when aps.""Code"" = 'IntentToOfferSent' then 1 else 0 end)) as IntentToOfferSent,
        //                    (sum(case when aps.""Code"" = 'FinalOfferSent' then 1 else 0 end)) as FinalOfferSent,
        //                    (sum(case when aps.""Code"" = 'FinalOfferAccepted' then 1 else 0 end)) as FinalOfferAccepted,
        //                    (sum(case when aps.""Code"" = 'VisaTransfer' then 1 else 0 end)) as VisaTransfer,
        //                    (sum(case when aps.""Code"" = 'MedicalCompleted' then 1 else 0 end)) as MedicalCompleted,
        //                    (sum(case when aps.""Code"" = 'VisaAppointmentTaken' then 1 else 0 end)) as VisaAppointmentTaken,
        //                    (sum(case when aps.""Code"" = 'BiometricCompleted' then 1 else 0 end)) as BiometricCompleted,
        //                    (sum(case when aps.""Code"" = 'VisaApproved' then 1 else 0 end)) as VisaApproved,
        //                    (sum(case when aps.""Code"" = 'VisaSentToCandidates' then 1 else 0 end)) as VisaSentToCandidates,
        //                    (sum(case when aps.""Code"" = 'FlightTicketsBooked' then 1 else 0 end)) as FightTicketsBooked,
        //                    (sum(case when aps.""Code"" = 'CandidateArrived' then 1 else 0 end)) as CandidateArrived,
        //                    (sum(case when aps.""Code"" = 'Joined' then 1 else 0 end)) as CandidateJoined                            
        //                    FROM rec.""JobAdvertisement"" as ja 
        //                    LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" =ja.""Id""
        //                    LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
        //                    LEFT Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
        //                    INNER JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = batch.""BatchStatus"" AND lov.""Code"" <> 'Draft'
        //                    #WHERE#
        //                    group by ja.""Id""
        //                    ";
        //    var where1 = @$" WHERE batch.""JobAdvertisementId""='{jobAdvId}' ";

        //    if (organizationId.IsNotNullAndNotEmpty())
        //    {
        //        where1 = @$" WHERE batch.""JobAdvertisementId""='{jobAdvId}' AND batch.""OrganizationId""='{organizationId}' ";
        //    }
        //    query1 = query1.Replace("#WHERE#", where1);

        //    var list2 = await _queryRepoJob.ExecuteQueryList(query1, null);

        //    string query3 = @$"SELECT ja.""Id"" as Id,ja.""JobId"" as JobId,                            
        //                    sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
        //                    sum(case when ja.""Status"" = 2 then 1 else 0 end) as InActive,
        //                    (sum(case when aps.""Code""='UnReviewed' then 1 else 0 end)) as UnReviewed
        //                    FROM rec.""JobAdvertisement"" as ja 
        //                    LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" =ja.""Id""
        //                    LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
        //                    WHERE ja.""Id""='{jobAdvId}' 
        //                    group by ja.""Id""
        //                    ";
        //    var list3 = await _queryRepoJob.ExecuteQueryList(query3, null);

        //    var a = list1.FirstOrDefault();
        //    if (a == null)
        //    {
        //        a = new ManpowerRecruitmentSummaryViewModel();
        //        a.Requirement = 0;
        //        a.Available = 0;
        //        a.Planning = 0;
        //        a.Transfer = 0;
        //        a.Balance = 0;
        //        a.OrganizationId = organizationId;
        //    }
        //    var b = list2.FirstOrDefault();
        //    if (b == null)
        //    {
        //        b = new JobAdvertisementViewModel();
        //        b.UnReviewed = 0;
        //        b.ShortlistedByHr = 0;
        //        b.ShortlistedForInterview = 0;
        //        b.InterviewCompleted = 0;
        //        b.IntentToOfferSent = 0;
        //        b.FinalOfferSent = 0;
        //        b.FinalOfferAccepted = 0;
        //        b.VisaTransfer = 0;
        //        b.MedicalCompleted = 0;
        //        b.VisaAppointmentTaken = 0;
        //        b.BiometricCompleted = 0;
        //        b.VisaApproved = 0;
        //        b.VisaSentToCandidates = 0;
        //        b.FightTicketsBooked = 0;
        //        b.CandidateArrived = 0;
        //        b.CandidateJoined = 0;
        //    }
        //    var c = list3.FirstOrDefault();
        //    if (c==null)
        //    {
        //        c = new JobAdvertisementViewModel();
        //        c.UnReviewed = 0;
        //    }
        //    var data = new RecruitmentDashboardViewModel
        //    {
        //        Requirement = a.Requirement,
        //        Available = a.Available,
        //        Planning = a.Planning,
        //        Transfer = a.Transfer,
        //        Balance = a.Balance,
        //        OrganizationId = a.OrganizationId,

        //        NoOfApplication = (c.UnReviewed + b.ShortlistedByHr + b.ShortlistedForInterview + b.InterviewCompleted +
        //                   b.IntentToOfferSent + b.FinalOfferSent + b.FinalOfferAccepted + b.VisaTransfer + b.MedicalCompleted +
        //                   b.VisaAppointmentTaken + b.BiometricCompleted + b.VisaApproved + b.VisaSentToCandidates +
        //                   +b.FightTicketsBooked + b.CandidateArrived + b.CandidateJoined),
        //        UnReviewed = c.UnReviewed,
        //        ShortlistedByHr = b.ShortlistedByHr,
        //        ShortlistedForInterview = b.ShortlistedForInterview,
        //        InterviewCompleted = b.InterviewCompleted,
        //        IntentToOfferSent = b.IntentToOfferSent,
        //        IntentToOfferAccepted = b.FinalOfferSent,
        //        FinalOfferAccepted = b.FinalOfferAccepted,
        //        VisaTransfer = b.VisaTransfer,
        //        MedicalCompleted = b.MedicalCompleted,
        //        VisaAppointmentTaken = b.VisaAppointmentTaken,
        //        BiometricCompleted = b.BiometricCompleted,
        //        VisaApproved = b.VisaApproved,
        //        VisaSentToCandidates = b.VisaSentToCandidates,
        //        FightTicketsBooked = b.FightTicketsBooked,
        //        CandidateArrived = b.CandidateArrived,
        //        CandidateJoined = b.CandidateJoined
        //    };
        //    return data;
        //}
        #endregion
        public async Task<RecruitmentDashboardViewModel> GetManpowerRecruitmentSummaryByOrgJob(string organizationId, string jobId)
        {            
            string query = @$"SELECT c.""JobId"" as JobId,
                            sum(c.""Requirement"") as Requirement,
                            sum(c.""Seperation"") as Seperation,
                            sum(c.""Available"") as Available,
                            sum(c.""Planning"") as Planning,
                            sum(c.""Transfer"") as Transfer,
                            sum(c.""Balance"") as Balance
                            FROM rec.""ManpowerRecruitmentSummary"" as c
                            JOIN cms.""Job"" as ja ON ja.""Id""=c.""JobId""
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
                            sum(c.""Requirement"") as Requirement,
                            sum(c.""Seperation"") as Seperation,
                            sum(c.""Available"") as Available,
                            sum(c.""Planning"") as Planning,
                            sum(c.""Transfer"") as Transfer,
                            sum(c.""Balance"") as Balance
                            FROM rec.""ManpowerRecruitmentSummary"" as c
                            --JOIN cms.""Job"" as ja ON ja.""Id""=c.""JobId""
                            where c.""IsDeleted""=false                     
                            ";
                where = "";
            }
            query = query.Replace("#WHERE#", where);
            var list1 = await _queryRepo.ExecuteQueryList(query, null);
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
                            FROM cms.""Job"" as ja 
                            join rec.""Application"" as ap on ap.""JobId"" =ja.""Id""
                            LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
                            LEFT join rec.""ApplicationStatus"" as apst on apst.""Id"" = ap.""ApplicationStatus""
                            LEFT Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
                            LEFT JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = batch.""BatchStatus"" AND lov.""Code"" <> 'Draft'
                            #WHERE#
                             
                            ";
            //var where1 = @$" WHERE batch.""JobId""='{jobId}' and ja.""Id""='{jobId}' group by ja.""Id""";
            var where1 = @$" WHERE ap.""JobId""='{jobId}' and ja.""Id""='{jobId}' group by ja.""Id""";

            if (organizationId.IsNotNullAndNotEmpty())
            {
                //where1 = @$" WHERE batch.""JobId""='{jobId}' and ja.""Id""='{jobId}' AND batch.""OrganizationId""='{organizationId}' group by ja.""Id""";
                //where1 = @$" WHERE ap.""JobId""='{jobId}' and ja.""Id""='{jobId}' AND ap.""OrganizationId""='{organizationId}' group by ja.""Id""";
                where1 = @$" WHERE ap.""JobId""='{jobId}' and ja.""Id""='{jobId}' AND batch.""OrganizationId""='{organizationId}' group by ja.""Id""";
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
                            FROM cms.""Job"" as ja 
                            join rec.""Application"" as ap on ap.""JobId"" =ja.""Id""
                            LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
                            LEFT join rec.""ApplicationStatus"" as apst on apst.""Id"" = ap.""ApplicationStatus""
                           
                             
                            ";
                where1 = "";
            }
                query1 = query1.Replace("#WHERE#", where1);



            var list2 = await _queryRepoJob.ExecuteQueryList(query1, null);



            //string query3 = @$"SELECT ja.""Id"" as Id,ja.""Id"" as JobId, count(ap.""Id"") as NoOfApplication,                           
            //                sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
            //                sum(case when ja.""Status"" = 2 then 1 else 0 end) as InActive,
            //                (sum(case when aps.""Code""='UnReviewed' then 1 else 0 end)) as UnReviewed
            //                FROM cms.""Job"" as ja 
            //                LEFT join rec.""Application"" as ap on ap.""JobId"" =ja.""Id""
            //                LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
            //                WHERE ja.""Id""='{jobId}' 
            //                group by ja.""Id""
            //                ";
            //var list3 = await _queryRepoJob.ExecuteQueryList(query3, null);

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
            //var c = list3.FirstOrDefault();
            //if (c == null)
            //{
            //    c = new JobAdvertisementViewModel();
            //    c.NoOfApplication = 0;
            //}
            var data = new RecruitmentDashboardViewModel
            {
                Requirement = a.Requirement,
                Seperation = a.Seperation,
                Available = a.Available,
                Planning = a.Planning,
                Transfer = a.Transfer,
                Balance = a.Balance,
                OrganizationId = a.OrganizationId,
                NoOfApplication = b.NoOfApplication,
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
        public async Task<IList<ManpowerRecruitmentSummaryVersionViewModel>> GetManpowerRecruitmentSummaryVersionData(string id)
        {
            string query = @$"SELECT c.*,u.""Name"" as CreatedByName ,j.""Name"" as JobTitle,o.""Name"" as OrganizationName
                            FROM rec.""ManpowerRecruitmentSummaryVersion"" as c
                            LEFT JOIN public.""User"" as u ON u.""Id"" = c.""CreatedBy"" 
                            LEFT JOIN cms.""Job"" as j ON j.""Id"" = c.""JobId""
                            LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
                            where c.""ManpowerRecruitmentSummaryId"" = '{id}'";



            //            string query = @$"SELECT c.""VersionNo"" as VersionNo,u.""Name"" as CreatedByName ,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,
            //c.""Requirement"" as Requirement,c.""Available"" as Available,c.""Planning"" as Planning,c.""Transfer"" as Transfer,c.""Balance"" as Balance,
            //c.""CreatedDate"" as CreatedDate
            //FROM rec.""ManpowerRecruitmentSummaryVersion"" as c
            //                            LEFT JOIN public.""User"" as u ON u.""Id"" = c.""CreatedBy"" 
            //                            LEFT JOIN cms.""Job"" as j ON j.""Id"" = c.""JobId""
            //                            LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
            //                            where c.""ManpowerRecruitmentSummaryId"" = '{id}'";


            var summaryList = await _queryRepo.ExecuteQueryList<ManpowerRecruitmentSummaryVersionViewModel>(query, null);
                       
            return summaryList;
        }
        public async Task<IList<ManpowerSummaryCommentViewModel>> GetManpowerSummaryCommentData(string id, string userRoleCode)
        {
            string query = @$"SELECT c.*,u.""Name"" as CreatedByName
                            FROM rec.""ManpowerSummaryComment"" as c
                            LEFT JOIN public.""UserRole"" as ur ON ur.""Id"" = c.""UserRoleId""
                            LEFT JOIN public.""User"" as u ON u.""Id"" = c.""CreatedBy""                            
                            where c.""ManpowerRecruitmentSummaryId""='{id}' and ur.""Code""='{userRoleCode}'";
            
            var commentList = await _queryRepo.ExecuteQueryList<ManpowerSummaryCommentViewModel>(query, null);
            string query1 = @$"SELECT c.""Id"",c.""JobId"",c.""OrganizationId"",c.""Requirement"",c.""Available"",c.""Planning"",c.""Transfer"",c.""VersionNo"",j.""Name"" as JobTitle,o.""Name"" as OrganizationName FROM rec.""ManpowerRecruitmentSummary"" as c
                            LEFT JOIN cms.""Job"" as j ON j.""Id"" = c.""JobId""
                            LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
                            where c.""Id""='{id}'  
                            union 
                            SELECT  c.""Id"",c.""JobId"",c.""OrganizationId"",c.""Requirement"",c.""Available"",c.""Planning"",c.""Transfer"",c.""VersionNo"",j.""Name"" as JobTitle,o.""Name"" as OrganizationName FROM rec.""ManpowerRecruitmentSummaryVersion"" as c
                            LEFT JOIN cms.""Job"" as j ON j.""Id"" = c.""JobId""
                            LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
                            where c.""ManpowerRecruitmentSummaryId"" = '{id}'";

            var summaryList = await _queryRepo.ExecuteQueryList(query1, null);
            var list = from c in commentList
                       join s in summaryList
                       on c.VersionNo equals s.VersionNo
                       select new ManpowerSummaryCommentViewModel
                       {
                           Id=c.Id,
                           Comment=c.Comment,
                           JobId=s.JobId,
                           JobTitle=s.JobTitle,
                           OrganizationId=s.OrganizationId,
                           OrganizationName=s.OrganizationName,
                           Requirement=s.Requirement,
                           Available=s.Available,
                           Planning=s.Planning,
                           Transfer=s.Transfer,
                           VersionNo=s.VersionNo,
                           CreatedByName=c.CreatedByName,
                           CreatedDate=c.CreatedDate

                       };
            return list.ToList();
        }
        public async Task<IList<IdNameViewModel>> GetJobIdNameList()
        {
            string query = @$"SELECT ""Id"" as Id,""Name"" as Name 
                            FROM cms.""Job""";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetOrganizationIdNameList()
        {
            string query = @$"SELECT ""Id"" as Id,""Name"" as Name 
                            FROM cms.""Organization""";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
        public async Task<ManpowerRecruitmentSummaryViewModel> GetManpowerRecruitmentSummaryCalculatedData(string id)
        {
            
            string query1 = @$"SELECT c.*,
                            sum(case when aps.""Code""='ShortListByHr' then 1 else 0 end) as ShortlistedByHrCalculated,
                            sum(case when aps.""Code"" = 'ShortListByHm' then 1 else 0 end) as ShortlistedForInterviewCalculated,
                            sum(case when aps.""Code"" = 'InterViewed' then 1 else 0 end) as InterviewCompletedCalculated,
                            sum(case when aps.""Code"" = 'Selected' then 1 else 0 end) as FinalOfferAcceptedCalculated,
                            sum(case when aps.""Code"" = 'Joined' then 1 else 0 end) as CandidateJoinedCalculated                            
                            FROM rec.""ManpowerRecruitmentSummary"" as c 
                            LEFT JOIN rec.""JobAdvertisement"" as ja ON ja.""ManpowerRecruitmentSummaryId"" = c.""Id""
                            LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" =ja.""Id""
                            LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
                            where c.""Id""='{id}'
                            group by c.""Id""";
            var list = await _queryRepo.ExecuteQueryList(query1, null);            
            return list.FirstOrDefault();
        }
    
        public async Task<JobAdvertisementViewModel> GetRecruitmentDashobardCount(string orgId)
        {          
            //string query = @$"SELECT                             
            //                sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
            //                sum(case when ja.""Status"" = 2 then 1 else 0 end) as InActive
            //                FROM rec.""JobAdvertisement"" as ja 
            //                inner join cms.""Job"" as j ON j.""Id"" = ja.""JobId""
            //                inner join rec.""ListOfValue"" as actlov on actlov.""Id""=ja.""ActionId"" AND actlov.""Code""='APPROVE'
            //                WHERE ja.""IsDeleted""='false' AND j.""IsDeleted""='false'
            //                and (ja.""ExpiryDate"" is null or ja.""ExpiryDate"">='{DateTime.Today.ToDatabaseDateFormat()}')
            //                ";
            string query = @$"with cte as (
SELECT      distinct   j.""Id"",j.""Name"",ja.""Status""                   


                            FROM rec.""JobAdvertisement"" as ja
                            inner join cms.""Job"" as j ON j.""Id"" = ja.""JobId""
                            inner join rec.""ListOfValue"" as actlov on actlov.""Id"" = ja.""ActionId"" AND actlov.""Code"" = 'APPROVE'
                            WHERE ja.""IsDeleted"" = 'false' AND j.""IsDeleted"" = 'false'
                            --and(ja.""ExpiryDate"" is null or ja.""ExpiryDate"" >= '{DateTime.Today.ToDatabaseDateFormat()}')

    and ja.""Status"" = '1' and ja.""Id"" not in (select ""Id"" from rec.""JobAdvertisement""
                                                          where ""JobId"" <> ja.""JobId"") union
                  SELECT      distinct j.""Id"",j.""Name"",ja.""Status""
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
							select
                              sum(case when cte.""Status"" = 1 then 1 else 0 end) as Active,
                            sum(case when cte.""Status"" = 2 then 1 else 0 end) as InActive
                            from cte

                          
                            ";
            var list = await _queryRepoJob.ExecuteQueryList(query, null);
            
            return list.FirstOrDefault();
        }

        public async Task<IList<JobAdvertisementViewModel>> GetManpowerUniqueJobData()
        {
            string query = @$"select distinct jb.""Id"" as Id,jb.""Id"" as JobId,jb.""Name"" as JobTitle,count(t) as Count,mt.""Name"" as ManpowerType
                            from cms.""Job"" as jb
                            --join cms.""Job"" as jb on jb.""Id"" = c.""JobId""
                            left join rec.""ListOfValue"" as mt on mt.""ListOfValueType""='LOV_MANPOWERTYPE' and mt.""Code"" = jb.""ManpowerTypeCode""
                            Left join public.""RecTask"" as t on t.""ReferenceTypeId""=jb.""Id"" AND t.""ReferenceTypeCode""='77'
                            where jb.""Status"" = 1 and jb.""IsDeleted"" = false
                            group by jb.""Id"",mt.""Name"" ";
            var list1 = await _queryRepo.ExecuteQueryList(query, null);
            string query1 = @$"SELECT c.""Id"",c.""JobId"",  c.""CreatedDate"" as CreateDate,                         

                            (sum(case when aps.""Code""='ShortListByHr' then 1 else 0 end)) as ShortlistedByHr,
                            (sum(case when aps.""Code"" = 'ShortListByHm' then 1 else 0 end)) as ShortlistedForInterview,
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
                            FROM rec.""JobAdvertisement"" as c                         
                            --LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" =c.""Id""
                            LEFT join rec.""Application"" as ap on ap.""JobId"" =c.""JobId""
                            LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
                            where c.""Status""=1
                            group by c.""Id""
                            ";

            //string query1 = @$"SELECT c.""Id"",                           
                          
            //                (sum(case when aps.""Code"" = 'Joined'  then 1 else 0 end) +(case when  c.""CandidateJoined"" is null then 0 else c.""CandidateJoined"" end)) as CJ                           
            //                FROM rec.""JobAdvertisement"" as c                           
            //                LEFT join rec.""Application"" as ap on ap.""JobAdvertisementId"" =c.""Id""
            //                LEFT join rec.""ApplicationState"" as aps on aps.""Id"" = ap.""ApplicationState""
            //                group by c.""Id""
            //                ";
            var list2 = await _queryRepoJob.ExecuteQueryList(query1, null);

            string query3 = @$"SELECT ja.""JobId"" ,                         
                            sum(case when ja.""Status""=1 then 1 else 0 end) as Active,
                            sum(case when ja.""Status"" = 2 then 1 else 0 end) as InActive                          
                            FROM rec.""JobAdvertisement"" as ja                            
                            group by ja.""JobId""
                            ";
            // var list3 = await _queryRepo.ExecuteQueryList(query1, null);
            var list3 = await _queryRepoJob.ExecuteQueryList(query3, null);


            var list4 = from a in list1
                        join b in list2
                        on a.Id equals b.JobId into gj
                        from sub in gj.DefaultIfEmpty()
                        select new JobAdvertisementViewModel
                        {
                           // Id = sub.Id,
                            Id = sub?.Id ?? String.Empty,
                            JobId = a.JobId,
                            JobName = a.JobTitle,
                            //VersionNo = a.VersionNo,                          
                            CreateDate = sub?.CreateDate ?? null,
                            //Active=b.Active,
                            //InActive=b.InActive,
                            ShortlistedByHr = sub?.ShortlistedByHr?? 0,
                            ShortlistedForInterview = sub?.ShortlistedForInterview ?? 0,
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
                            ManpowerType = a.ManpowerType
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
                           ShortlistedByHr = a.ShortlistedByHr ,
                           ShortlistedForInterview =a.ShortlistedForInterview,
                           IntentToOfferSent = a.IntentToOfferSent ,
                           FinalOfferSent = a.FinalOfferSent ,
                           WorkerPool =a.WorkerPool ,
                           BusinessVisa = a.BusinessVisa ,
                           WorkVisa = a.WorkVisa,
                           VisaTransfer = a.VisaTransfer ,
                           WorkPermit = a.WorkPermit ,
                           CandidateJoined = a.CandidateJoined ,
                           WorkerJoined = a.WorkerJoined ,
                           Joined = a.Joined ,
                           PostStaffJoined = a.PostStaffJoined,
                           PostWorkerJoined = a.PostWorkerJoined,
                           Count = a.Count,
                           ManpowerType = a.ManpowerType
                       };

            return list.ToList();
        }

        public async Task<JobAdvertisementViewModel> GetState(string Id)
        {
            string query = @$"select v.""Name"" as ActionName from rec.""JobAdvertisement"" as ja
                                left join cms.""Job"" as j on j.""Id"" = ja.""JobId""
                                left join rec.""ListOfValue"" as v on v.""Id"" = ja.""ActionId""
                                where ja.""Status"" = 1 and ja.""JobId"" = (select ""JobId"" from rec.""ManpowerRecruitmentSummary"" where ""Id"" = '{Id}')";
            var queryData = await _queryRepoJob.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task UpdateManpowerRecruitmentSummaryForAvailable(string applicationId)
        {
            string query = $@"select mrs.*
                            from rec.""Application"" as app
                            inner join rec.""Batch"" as batch ON batch.""Id"" = app.""BatchId""
                            inner join rec.""ManpowerRecruitmentSummary"" as mrs ON mrs.""JobId"" = batch.""JobId"" and mrs.""OrganizationId"" = batch.""OrganizationId""
                            where app.""Id"" = '{applicationId}'";
            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            if (queryData.IsNotNull())
            {
                long newavailable = 0;
                if (queryData.Available.IsNotNull())
                {
                    newavailable = queryData.Available.Value+1;
                }
                else
                {
                    newavailable = 1;
                }
                //string query2 = @$"update rec.""ManpowerRecruitmentSummary"" set ""Available""='{newavailable}' where ""Id""='{queryData.Id}'";
                //var result = await _queryRepo.ExecuteScalar<bool?>(query2, null);
                var model = await GetSingleById(queryData.Id);
                model.Available = newavailable;
                model.DataAction = DataActionEnum.Edit;
                var result = await Edit(model);
                if (result.IsSuccess)
                {

                }
            }
        }


        public async Task<DataTable> GetTaskByOrgUnit(string userId,string userroleId)
        {
            var result = new List<ManpowerRecruitmentSummaryViewModel>();
            var userCode = "";
            //string query = @$"SELECT  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task)+sum(case when lov.""Code""!='Close' then 1 else 0 end) as Count  FROM public.""RecTask"" as task
            //     join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
            //     --join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
            //     --join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
            //      join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
            //     join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
            //    join rec.""Batch"" as b on b.""Id"" = ap.""BatchId""
            //   JOIN rec.""ListOfValue"" as lov ON lov.""Id""=b.""BatchStatus""
            //    join cms.""Organization"" as o on o.""Id"" = b.""OrganizationId""
            //    join cms.""Job"" as j on j.""Id"" = ap.""JobId""
            //    --where (task.""AssigneeUserId"" ='{userId}') and(task.""NtsType"" is null or task.""NtsType""=1)
            //    and (task.""TaskStatusCode"" = 'INPROGRESS' or task.""TaskStatusCode"" = 'OVERDUE' or task.""TaskStatusCode"" = 'REJECTED')
            //    GROUP BY o.""Id"", j.""Id""";

            //if (userroleId.IsNotNullAndNotEmpty())
            //{
            //    var userrole = await _userroleBusiness.GetSingleById(userroleId);
            //    userCode = userrole.Code;
            //    query = @$"SELECT  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task) as Count  FROM public.""RecTask"" as task
            //     join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
            //     join public.""UserRoleUser"" as ur on ur.""UserRoleId""= '{userroleId}' and ur.""UserId"" = task.""AssigneeUserId""
            //     join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
            //      join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
            //     join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
            //    join rec.""Batch"" as b on b.""Id"" = ap.""BatchId""
            //   JOIN rec.""ListOfValue"" as lov ON lov.""Id""=b.""BatchStatus""
            //    join cms.""Organization"" as o on o.""Id"" = b.""OrganizationId""
            //    join cms.""Job"" as j on j.""Id"" = ap.""JobId""
            //    --where (task.""AssigneeUserId"" ='{userId}') and(task.""NtsType"" is null or task.""NtsType""=1)
            //    and (task.""TaskStatusCode"" = 'INPROGRESS' or task.""TaskStatusCode"" = 'OVERDUE' or task.""TaskStatusCode"" = 'REJECTED')
            //    GROUP BY o.""Id"", j.""Id""";

            //    if(userCode=="HR")
            //    {
            //        query = @$"SELECT  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task)+sum(case when lov.""Code""='Draft' then 1 else 0 end) as Count  FROM public.""RecTask"" as task
            //     join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
            //     join public.""UserRoleUser"" as ur on ur.""UserRoleId""= '{userroleId}' and ur.""UserId"" = task.""AssigneeUserId""
            //     join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
            //      join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
            //     join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
            //    join rec.""Batch"" as b on b.""Id"" = ap.""BatchId""
            //   JOIN rec.""ListOfValue"" as lov ON lov.""Id""=b.""BatchStatus""
            //    join cms.""Organization"" as o on o.""Id"" = b.""OrganizationId""
            //    join cms.""Job"" as j on j.""Id"" = ap.""JobId""
            //    --where (task.""AssigneeUserId"" ='{userId}') and(task.""NtsType"" is null or task.""NtsType""=1)
            //    and (task.""TaskStatusCode"" = 'INPROGRESS' or task.""TaskStatusCode"" = 'OVERDUE' or task.""TaskStatusCode"" = 'REJECTED')
            //    GROUP BY o.""Id"", j.""Id""";
            //    }
            //    if (userCode == "HM")
            //    {
            //        query = @$"SELECT  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task)+sum(case when lov.""Code""='PendingwithHM' then 1 else 0 end) as Count  FROM public.""RecTask"" as task
            //     join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
            //     join public.""UserRoleUser"" as ur on ur.""UserRoleId""= '{userroleId}' and ur.""UserId"" = task.""AssigneeUserId""
            //     join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
            //      join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
            //     join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
            //    join rec.""Batch"" as b on b.""Id"" = ap.""BatchId""
            //   JOIN rec.""ListOfValue"" as lov ON lov.""Id""=b.""BatchStatus""
            //    join cms.""Organization"" as o on o.""Id"" = b.""OrganizationId""
            //    join cms.""Job"" as j on j.""Id"" = ap.""JobId""
            //    --where (task.""AssigneeUserId"" ='{userId}') and(task.""NtsType"" is null or task.""NtsType""=1)
            //    and (task.""TaskStatusCode"" = 'INPROGRESS' or task.""TaskStatusCode"" = 'OVERDUE' or task.""TaskStatusCode"" = 'REJECTED')
            //    GROUP BY o.""Id"", j.""Id""";
            //    }

            //}

            //var list1 = await _queryRepo.ExecuteQueryList(query, null);

            //if (userroleId.IsNullOrEmpty() || userCode == "HR")
            //{
            //    string query1 = @$"SELECT  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,sum(case when lov.""Code""='Draft' then 1 else 0 end) as Count 
            //    FROM rec.""Batch"" as b

            //    join rec.""Application"" as ap on  ap.""BatchId"" = b.""Id""

            //    join cms.""Organization"" as o on o.""Id"" = b.""OrganizationId""
            //    join cms.""Job"" as j on j.""Id"" = ap.""JobId""
            //    join rec.""Batch"" as w on w.""Id"" = ap.""WorkerBatchId""
            //    JOIN rec.""ListOfValue"" as lov ON lov.""Id""=w.""BatchStatus""
            //    GROUP BY o.""Id"", j.""Id""";


            //    var list2 = await _queryRepo.ExecuteQueryList(query1, null);

            //    if (list2.Count > 0)
            //    {

            //        result = (from a in list1
            //                  join b in list2
            //                   on new { X1 = a.JobId, X2 = a.OrganizationId } equals new { X1 = b.JobId, X2 = b.OrganizationId } into gj
            //                  from sub in gj.DefaultIfEmpty()
            //                  select new ManpowerRecruitmentSummaryViewModel
            //                  {
            //                      JobId = a.JobId,
            //                      OrganizationId = a.OrganizationId,
            //                      Count = a.Count + sub?.Count ?? 0,
            //                  }).ToList();
            //    }
            //    else
            //    {
            //        result = list1;
            //    }
            //}
            //else
            //{
            //    result = list1;
            //}
           
            var query = $@" select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
	                FROM public.""RecTask"" as task 
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""	                
                    --join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId"" and ur.""UserRoleId""= '{userroleId}'
                    --join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
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
                    
                    GROUP BY j.""Id"",o.""Id""";
            result = await _queryRepo.ExecuteQueryList(query, null);

            //string orgQry = @$"SELECT distinct o.""Id"" as OrganizationId,o.""Name"" as OrganizationName
            //                FROM rec.""ManpowerRecruitmentSummary"" as c                          
            //                JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId"" where c.""IsDeleted"" = false

            //                ";

            //var Columns = await _queryRepo.ExecuteQueryList(orgQry, null);

            //string jobQry = @$"select j.""Id"" as JobId ,j.""Name""  as JobTitle
            //            from rec.""JobAdvertisement""
            //            as jd inner join cms.""Job"" as j
            //            on j.""Id"" = jd.""JobId""
            //            inner join rec.""ListOfValue"" as actlov on actlov.""Id""=jd.""ActionId"" AND actlov.""Code""='APPROVE'

            //            WHERE jd.""Status"" = '1' AND jd.""IsDeleted""='false' AND j.""IsDeleted""='false' 
            //            ";

            //var Rows = await _queryRepo.ExecuteQueryList(jobQry, null);

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
                        dataTable.Columns.Add("_"+item.OrganizationName);
                        length++;
                    }
                    if (true)
                    {
                        foreach (var r in Rows)
                        {
                            var values = new object[length];
                            var rows = result.Where(x => x.JobId==r.JobId);
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

        public async Task<DataTable> GetJobByOrgUnit(string userId)
        {

            //string orgQry = @$"SELECT distinct o.""Id"" as OrganizationId,o.""Name"" as OrganizationName
            //                FROM rec.""ManpowerRecruitmentSummary"" as c                          
            //                LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId"" where c.""IsDeleted"" = false
            //                ";
            var orgQry = $@" select  j.""Id"" as JobId,j.""Name"" as JobTitle,o.""Name"" as OrganizationName,o.""Id"" as OrganizationId,count(task.""Id"") as ""Count""
	                FROM public.""RecTask"" as task 
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""	                
                    --join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId"" 
                    --join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
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
            var result = await _queryRepo.ExecuteQueryList(orgQry, null);
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
                        dataTable.Columns.Add("_"+item.OrganizationName);
                        length++;
                    }

                }
            }
            catch (Exception e)
            {

            }

            return dataTable;

        }

        public async Task<IList<RecTaskViewModel>> GetJobDescriptionTaskList(string manpowerId)
        {
            var list = new List<RecTaskViewModel>();
            string query = "";
           
                query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( au.""Name"" for 1) as TaskOwnerFirstLetter
                        FROM public.""RecTask"" as task                      
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where task.""ReferenceTypeId"" ='{manpowerId}' and task.""NtsType"" = 1 order by task.""CreatedDate"" asc ";
           

            var result = await _queryTask.ExecuteQueryList(query, null);
            return result;
        }
    }
}