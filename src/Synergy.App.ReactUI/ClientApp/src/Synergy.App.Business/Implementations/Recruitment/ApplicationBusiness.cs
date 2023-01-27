using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
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


namespace Synergy.App.Business
{
    public class ApplicationBusiness : BusinessBase<ApplicationViewModel, Application>, IApplicationBusiness
    {
        private readonly IRepositoryQueryBase<ListOfValueViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepoIdName;
        private readonly IRepositoryQueryBase<ApplicationViewModel> _appqueryRepo;
        private readonly IRepositoryQueryBase<JobDescriptionViewModel> _jobdescripqueryRepo;
        private readonly IUserContext _userContext;
        private readonly ICandidateProfileBusiness _candidateProfileBusiness;
        private readonly IRepositoryQueryBase<CandidateProfileViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<ApplicationViewModel> _queryRepoId;
        private readonly IRepositoryQueryBase<ApplicationStateTrackViewModel> _apstqueryRepo;
        private readonly IUserBusiness _userBusiness;
        private readonly IJobAdvertisementBusiness _jobAdvBusiness;
        private readonly IApplicationJobCriteriaBusiness _jobCriteriaBusiness;
        private readonly ICandidateEducationalBusiness _candidateEducationalBusiness;
        private readonly ICandidateExperienceBusiness _candidateExperienceBusiness;
        private readonly IApplicationExperienceBusiness _applicationExperienceBusiness;
        private readonly IApplicationEducationalBusiness _applicationEducationalBusiness;
        private readonly ICandidateExperienceByCountryBusiness _candidateExperienceByCountryBusiness;
        private readonly ICandidateExperienceByOtherBusiness _candidateExperienceByOtherBusiness;
        private readonly IMasterBusiness _masterBusiness;
        private readonly IListOfValueBusiness _lovBusiness;
        private readonly IApplicationExperienceByCountryBusiness _applicationExperienceByCountryBusiness;
        private readonly IApplicationExperienceByOtherBusiness _applicationExperienceByOtherBusiness;
        private readonly IRepositoryQueryBase<RecTaskViewModel> _queryTask;
        public ApplicationBusiness(IRepositoryBase<ApplicationViewModel, Application> repo, IMapper autoMapper, IRepositoryQueryBase<ListOfValueViewModel> queryRepo, IRepositoryQueryBase<ApplicationViewModel> appqueryRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepoIdName, IUserContext userContext, ICandidateProfileBusiness candidateProfileBusiness,
            IRepositoryQueryBase<CandidateProfileViewModel> queryRepo1, IRepositoryQueryBase<ApplicationViewModel> queryRepoId,
            IRepositoryQueryBase<ApplicationStateTrackViewModel> apstqueryRepo,
            IUserBusiness userBusiness, IJobAdvertisementBusiness jobAdvBusiness,
            IApplicationJobCriteriaBusiness jobCriteriaBusiness
            , IRepositoryQueryBase<RecTaskViewModel> queryTask,
            ICandidateEducationalBusiness candidateEducationalBusiness,
            ICandidateExperienceBusiness candidateExperienceBusiness,
            IApplicationEducationalBusiness applicationEducationalBusiness,
            IApplicationExperienceBusiness applicationExperienceBusiness,
            ICandidateExperienceByCountryBusiness candidateExperienceByCountryBusiness,
            IMasterBusiness masterBusiness, IListOfValueBusiness lovBusiness,
            ICandidateExperienceByOtherBusiness candidateExperienceByOtherBusiness,
            IApplicationExperienceByCountryBusiness applicationExperienceByCountryBusiness,
            IApplicationExperienceByOtherBusiness applicationExperienceByOtherBusiness,
            IRepositoryQueryBase<JobDescriptionViewModel> jobdescripqueryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _appqueryRepo = appqueryRepo;
            _queryRepoIdName = queryRepoIdName;
            _userContext = userContext;
            _candidateProfileBusiness = candidateProfileBusiness;
            _queryRepo1 = queryRepo1;
            _queryRepoId = queryRepoId;
            _apstqueryRepo = apstqueryRepo;
            _userBusiness = userBusiness;
            _jobAdvBusiness = jobAdvBusiness;
            _jobCriteriaBusiness = jobCriteriaBusiness;
            _queryTask = queryTask;
            _candidateEducationalBusiness = candidateEducationalBusiness;
            _candidateExperienceBusiness = candidateExperienceBusiness;
            _applicationEducationalBusiness = applicationEducationalBusiness;
            _applicationExperienceBusiness = applicationExperienceBusiness;
            _candidateExperienceByCountryBusiness = candidateExperienceByCountryBusiness;
            _masterBusiness = masterBusiness;
            _lovBusiness = lovBusiness;
            _candidateExperienceByOtherBusiness = candidateExperienceByOtherBusiness;
            _applicationExperienceByCountryBusiness = applicationExperienceByCountryBusiness;
            _applicationExperienceByOtherBusiness = applicationExperienceByOtherBusiness;
            _jobdescripqueryRepo = jobdescripqueryRepo;
        }
        public async Task<List<TreeViewViewModel>> GetHMTreeList(string id)
        {
            var list = new List<TreeViewViewModel>();
           // var data = await GetList();
            list.Add(new TreeViewViewModel
            {
                id = "1",
                Name = "Shortlist by HM",
                DisplayName = "Shortlist by HM",
                ParentId = null,
                hasChildren = true,
                expanded = true,
                Type = "Root"
            });
            list.Add(new TreeViewViewModel
            {
                id = "2",
                Name = "Evaluate Candidate",
                DisplayName = "Evaluate Candidate",
                ParentId = null,
                hasChildren = true,
                expanded = true,
                Type = "Root"

            });
            return list;
        }
        public async override Task<CommandResult<ApplicationViewModel>> Create(ApplicationViewModel model, bool autoCommit = true)
        {
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<ApplicationViewModel>.Instance(model, false, validateName.Messages);
            //}
            model.ApplicationNo = await GenerateNextApplicationNo();
            var result = await base.Create(model,autoCommit);

            return CommandResult<ApplicationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationViewModel>> Edit(ApplicationViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<ApplicationViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<ApplicationViewModel>.Instance(model, false, validateName.Messages);
            //}
            //if (model.InterviewSelectionFeedback.IsNotNull())
            //{
            //    var status = string.Empty;
            //    if (model.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected)
            //    {
            //        if (model.ManpowerTypeCode == "Staff")
            //        {
            //            status = "SHORTLISTED";
            //        }
            //        else
            //        {
            //            status = "SELECTED";
            //        }
            //    }
            //    else if (model.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected)
            //    {
            //        status = "WAITLISTED";
            //    }
            //    else if (model.InterviewSelectionFeedback == InterviewFeedbackEnum.Rejected)
            //    {
            //        status = "REJECTED";
            //    }
            //    if (status.IsNotNullAndNotEmpty())
            //    {
            //        var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == status);
            //        model.ApplicationStatus = status1.Id;
            //    }
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ApplicationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<ApplicationViewModel>> IsNameExists(ApplicationViewModel model)
        {

            return CommandResult<ApplicationViewModel>.Instance();
        }
        public async Task<string> GenerateNextApplicationNo()
        {
            var date = DateTime.Now.Date;
            var id = await GenerateNextDatedApplicationId();
            return string.Concat("AP-", String.Format("{0:dd-MM-yyyy}", date), "-", id);
        }
        public async Task<long> GenerateNextDatedApplicationId()
        {
            string query = @$"SELECT  count(*) as cc FROM Rec.""Application"" as app
                                where Date(app.""CreatedDate"")=Date('{ToDD_MMM_YYYY_HHMMSS(DateTime.Now)}')
                            ";
            var result = await _queryRepoId.ExecuteScalar<long>(query, null);
            return result;
        }

        public string ToDD_MMM_YYYY_HHMMSS(DateTime value)
        {
            return String.Format("{0:yyyy-MM-dd}", value);
        }
        public async Task<CandidateProfileViewModel> GetApplicationDetails(string applicationId)
        {
            //string query = @$"select pl.*, n.""Name"" as NationalityName, g.""Name"" as GenderName, m.""Name"" as MaritalStatusName,
            //                    pp.""Name"" as PassportIssueCountryName, vc.""Name"" as VisaCountryName, vt.""Name"" as VisaTypeName, ocv.""Name"" as OtherCountryVisaName,
            //                    ocvt.""Name"" as OtherCountryVisaTypeName, cc.""Name"" as CurrentAddressCountryName, pc.""Name"" as PermanentAddressCountryName
            //                    FROM rec.""Application"" as pl
            //                    LEFT JOIN cms.""Country"" as n ON n.""Id"" = pl.""NationalityId""
            //                    LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = pl.""Gender""
            //                    LEFT JOIN rec.""ListOfValue"" as m ON m.""Id"" = pl.""MaritalStatus""
            //                    LEFT JOIN cms.""Country"" as pp ON pp.""Id"" = pl.""PassportIssueCountryId""
            //                    LEFT JOIN cms.""Country"" as vc ON vc.""Id"" = pl.""VisaCountry""
            //                    LEFT JOIN rec.""ListOfValue"" as vt ON vt.""Id"" = pl.""VisaType""
            //                    LEFT JOIN cms.""Country"" as ocv ON ocv.""Id"" = pl.""OtherCountryVisa""
            //                    LEFT JOIN rec.""ListOfValue"" as ocvt ON ocvt.""Id"" = pl.""OtherCountryVisaType""
            //                    LEFT JOIN cms.""Country"" as cc ON cc.""Id"" = pl.""CurrentAddressCountryId""
            //                    LEFT JOIN cms.""Country"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""

            //                    WHERE pl.""Id"" = '{applicationId}' and pl.""IsDeleted"" = false";

            string query = $@"select pl.*, t.""Name"" as TitleName, n.""Name"" as NationalityName, g.""Name"" as GenderName, m.""Name"" as MaritalStatusName,
                                pp.""Name"" as PassportIssueCountryName, vc.""Name"" as VisaCountryName, vt.""Name"" as VisaTypeName, ocv.""Name"" as OtherCountryVisaName,
                                ocvt.""Name"" as OtherCountryVisaTypeName, cc.""Name"" as CurrentAddressCountryName, pc.""Name"" as PermanentAddressCountryName,
                                sc.""Name"" as SalaryCurrencyName, b.""Name"" as BatchName, wb.""Name"" as WorkerBatch
                                FROM rec.""Application"" as pl
                                 LEFT JOIN rec.""ListOfValue"" as t ON t.""Id"" = pl.""TitleId""
                                LEFT JOIN cms.""Nationality"" as n ON n.""Id"" = pl.""NationalityId""
                                LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = pl.""Gender""
                                LEFT JOIN rec.""ListOfValue"" as m ON m.""Id"" = pl.""MaritalStatus""
                                LEFT JOIN cms.""Country"" as pp ON pp.""Id"" = pl.""PassportIssueCountryId""
                                LEFT JOIN cms.""Country"" as vc ON vc.""Id"" = pl.""VisaCountry""
                                LEFT JOIN rec.""ListOfValue"" as vt ON vt.""Id"" = pl.""VisaType""
                                LEFT JOIN cms.""Country"" as ocv ON ocv.""Id"" = pl.""OtherCountryVisa""
                                LEFT JOIN rec.""ListOfValue"" as ocvt ON ocvt.""Id"" = pl.""OtherCountryVisaType""
                                LEFT JOIN cms.""Country"" as cc ON cc.""Id"" = pl.""CurrentAddressCountryId""
                                LEFT JOIN cms.""Country"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                                LEFT JOIN cms.""Currency"" as sc ON sc.""Id"" = pl.""NetSalaryCurrency""
                                LEFT JOIN rec.""Batch"" as b on b.""Id"" = pl.""BatchId""
                                LEFT JOIN rec.""Batch"" as wb on wb.""Id"" = pl.""WorkerBatchId""
                                WHERE pl.""Id"" = '{applicationId}' and pl.""IsDeleted"" = false";

            var queryData = await _queryRepo1.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<ApplicationViewModel> GetApplicationForJobAdv(string applicationId)
        {   
            string query = $@"select pl.*,b.""OrganizationId"" as OrganizationId, hm.""Id"" as HiringManagerId,b.""CreatedBy"" as RecruiterId
                                FROM rec.""Application"" as pl
                                LEFT JOIN rec.""Batch"" as b on b.""Id"" = pl.""BatchId""
                                LEFT JOIN rec.""HiringManager"" as hm on hm.""UserId"" = b.""HiringManager""
                                WHERE pl.""Id"" = '{applicationId}' and pl.""IsDeleted"" = false";

            var queryData = await _appqueryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<CandidateProfileViewModel> GetApplicationDetails(string candidateProfileId, string jobAdvId)
        {
            string query = @$"select pl.*, n.""Name"" as NationalityName, g.""Name"" as GenderName, m.""Name"" as MaritalStatusName,
                                pp.""Name"" as PassportIssueCountryName, vc.""Name"" as VisaCountryName, vt.""Name"" as VisaTypeName, ocv.""Name"" as OtherCountryVisaName,
                                ocvt.""Name"" as OtherCountryVisaTypeName, cc.""Name"" as CurrentAddressCountryName, pc.""Name"" as PermanentAddressCountryName
                                FROM rec.""Application"" as pl
                                LEFT JOIN cms.""Country"" as n ON n.""Id"" = pl.""NationalityId""
                                LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = pl.""Gender""
                                LEFT JOIN rec.""ListOfValue"" as m ON m.""Id"" = pl.""MaritalStatus""
                                LEFT JOIN cms.""Country"" as pp ON pp.""Id"" = pl.""PassportIssueCountryId""
                                LEFT JOIN cms.""Country"" as vc ON vc.""Id"" = pl.""VisaCountry""
                                LEFT JOIN rec.""ListOfValue"" as vt ON vt.""Id"" = pl.""VisaType""
                                LEFT JOIN cms.""Country"" as ocv ON ocv.""Id"" = pl.""OtherCountryVisa""
                                LEFT JOIN rec.""ListOfValue"" as ocvt ON ocvt.""Id"" = pl.""OtherCountryVisaType""
                                LEFT JOIN cms.""Country"" as cc ON cc.""Id"" = pl.""CurrentAddressCountryId""
                                LEFT JOIN cms.""Country"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                                WHERE pl.""CandidateProfileId"" = '{candidateProfileId}' and pl.""JobAdvertisementId"" = '{jobAdvId}' and pl.""IsDeleted"" = false";

            var queryData = await _queryRepo1.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task UpdateApplicationState(string users, string type)
        {
            var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == type);
            string state = state1.Id;
            var str = users.Trim(',').Replace(",", "','");
            var where = String.Concat("'", str, "'");
            //var applicationstate = type;

            string query = @$"update rec.""Application"" set ""ApplicationState""='{state}' where ""Id"" in ( {where} )";
            var result = await _appqueryRepo.ExecuteScalar<bool?>(query, null);

        }
        public async Task UpdateApplicationtStatus(string users, string type)
        {
            var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == type);
            string state = state1.Id;
            var str = users.Trim(',').Replace(",", "','");
            var where = String.Concat("'", str, "'");
            //var applicationstate = type;

            string query = @$"update rec.""Application"" set ""ApplicationStatus""='{state}' where ""Id"" in ( {where} )";
            var result = await _appqueryRepo.ExecuteScalar<bool?>(query, null);

        }
        public async Task<IdNameViewModel> GetApplicationStatusByCode(string code)
        {
            var status = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == code);
            return status;

        }
        public async Task<string> CreateApplicationStatusTrack(string applicationId, string statusCode = null, string taskReferenceId = null)
        {
            try
            {
                var application = await GetSingleById(applicationId);
                var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                ApplicationStateTrack.ApplicationId = application.Id;
                ApplicationStateTrack.ApplicationStateId = application.ApplicationState;
                if (statusCode.IsNullOrEmpty())
                {
                    var appStatus = await _repo.GetSingle<ApplicationStatus, ApplicationStatus>(x => x.Id == application.ApplicationStatus);
                    if (appStatus != null)
                    {
                        ApplicationStateTrack.ApplicationStatusCode = appStatus.Code;
                    }
                    ApplicationStateTrack.ApplicationStatusId = application.ApplicationStatus;
                }
                else
                {
                    var appStatus = await _repo.GetSingle<ApplicationStatus, ApplicationStatus>(x => x.Code == statusCode);
                    if (appStatus != null)
                    {
                        ApplicationStateTrack.ApplicationStatusId = appStatus.Id;
                    }
                    ApplicationStateTrack.ApplicationStatusCode = statusCode;
                }
                ApplicationStateTrack.TaskReferenceId = taskReferenceId;
                ApplicationStateTrack.ChangedBy = _userContext.UserId;
                ApplicationStateTrack.ChangedDate = DateTime.Now;
                var result2 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }
        public async Task<string> UpdateApplicationBatch(string applicationId, string BatchId)
        {
            var application = await GetSingleById(applicationId);
            application.WorkerBatchId = BatchId;
            var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == "AddedToBatch");
            if (status1 != null)
            {
                application.ApplicationStatus = status1.Id;
            }
            var result1 = await base.Edit<ApplicationViewModel, Application>(application);
            return result1.IsSuccess.ToString();
        }
        public async Task<string> UpdateApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state, string BatchId, string JobAddId, string JobId, string OrgId, bool autoCommit = true)
        {
            try
            {
                if (type == "JobApplication")
                {
                    var application = await GetSingleById(applicationId);
                    var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == state);
                    var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == status);
                    application.ApplicationState = state1.Id;
                    application.ApplicationStatus = status1.Id;
                    application.AppliedDate = DateTime.Now;
                    //application.OrganizationId = OrgId;
                    if (status == "SHORTLISTED")
                    {
                        application.BatchId = BatchId;
                        var batch=await _repo.GetSingleById<BatchViewModel, Batch>(BatchId);
                        application.OrganizationId = batch.OrganizationId;// OrgId;
                    }
                    // application.JobAdvertisementId = JobAddId;
                    application.JobId = JobId;                   
                    var result1 = await base.Edit<ApplicationViewModel, Application>(application);
                    if (result1.IsSuccess)
                    {
                        var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                        ApplicationStateTrack.ApplicationId = result1.Item.Id;
                        ApplicationStateTrack.ApplicationStateId = state1.Id;
                        ApplicationStateTrack.ApplicationStatusId = status1.Id;
                        ApplicationStateTrack.ChangedBy = _userContext.UserId;
                        ApplicationStateTrack.ChangedDate = DateTime.Now;
                        var result2 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                        if (status == "SHORTLISTED")
                        {
                            await CreateApplicationStatusTrack(result1.Item.Id, "SL_BATCH");
                        }
                    }

                    //string query = @$"update rec.""ApplicationStateTrack"" set ""ApplicationId""='{applicationId}', ""ApplicationStateId""='{state1.Id}', ""ApplicationStatusId""='{status1.Id}',
                    // ""ChangedBy""='{_userContext.UserId}' ";
                    //var result = await _appqueryRepo.ExecuteScalar<bool?>(query, null);
                }
                else if (type == "CandidateProfile")
                {
                    var application = await _candidateProfileBusiness.GetSingleById(CandidateProfileId);
                    var application1 = await GetSingle(x=>x.CandidateProfileId== CandidateProfileId && x.JobId== JobId);
                    if (application1==null)
                    {
                    var data = _autoMapper.Map<CandidateProfileViewModel, ApplicationViewModel>(application);
                    var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == state);
                    var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == status);
                    data.ApplicationState = state1.Id;
                    data.ApplicationStatus = status1.Id;
                    data.CandidateProfileId = application.Id;
                    data.AppliedDate = DateTime.Now;
                    data.Id = "";
                    if (status == "SHORTLISTED")
                    {
                        data.BatchId = BatchId;
                        var batch=await _repo.GetSingleById<BatchViewModel, Batch>(BatchId);
                        data.OrganizationId = batch.OrganizationId;// OrgId;
                    }
                    data.ApplicationNo = await GenerateNextApplicationNo();
                    //data.JobAdvertisementId = JobAddId;
                    data.JobId = JobId;
                   // data.OrganizationId = OrgId;
                    var result = await base.Create(data, autoCommit);
                    data.Id = result.Item.Id;

                    // create new user- update userId in candidateprofile table
                    var userViewModel = new UserViewModel();
                    if (application.Email.IsNotNull())
                    {
                        // Check email exist or not 
                        var user = await _userBusiness.GetSingle(x => x.Email == application.Email);
                        if (user != null)
                        {
                            var candmodel = new CandidateProfileViewModel()
                            {
                                Id = CandidateProfileId,
                                UserId = user.Id
                            };
                            var candResult = await _candidateProfileBusiness.UpdateCandidateProfileDetails(candmodel);
                        }
                        else
                        {
                                var random = new Random();
                                var Pass = Convert.ToString(random.Next(10000000, 99999999));
                                userViewModel.Email = application.Email;
                            userViewModel.Name = application.FirstName;
                            userViewModel.CreatedBy = _userContext.UserId;
                            userViewModel.CreatedDate = DateTime.Now;
                            userViewModel.Status = StatusEnum.Active;
                            userViewModel.Password = Pass;
                            userViewModel.ConfirmPassword = Pass;
                            userViewModel.PortalName = "CareerPortal";
                            userViewModel.UserType = UserTypeEnum.CANDIDATE;
                            userViewModel.SendWelcomeEmail = true;
                            var userResult = await _userBusiness.Create(userViewModel);

                            if (userResult.IsSuccess)
                            {
                                var candmodel = new CandidateProfileViewModel()
                                {
                                    Id = CandidateProfileId,
                                    UserId = userResult.Item.Id
                                };
                                var candResult = await _candidateProfileBusiness.UpdateCandidateProfileDetails(candmodel);
                            }
                        }
                        //var name = application.FirstName.IsNotNullAndNotEmpty() && application.LastName.IsNotNullAndNotEmpty() ?
                        //    application.FirstName + " " + application.LastName : "";

                    }


                    if (result.IsSuccess)
                    {
                        // Copy Candidate Education                   
                        var candidateEdu = await _repo.GetList<CandidateEducationalViewModel, CandidateEducational>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateEdu != null && candidateEdu.Count() > 0)
                        {
                            foreach (var education in candidateEdu)
                            {
                                var appEducation = _autoMapper.Map<CandidateEducationalViewModel, ApplicationEducationalViewModel>(education);
                                appEducation.ApplicationId = result.Item.Id;
                                appEducation.Id = "";
                                var eduresult = await base.Create<ApplicationEducationalViewModel, ApplicationEducational>(appEducation);
                            }
                        }
                        // Copy Candidate Experience
                        var candidateExp = await _repo.GetList<CandidateExperienceViewModel, CandidateExperience>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateExp != null && candidateExp.Count() > 0)
                        {
                            foreach (var exp in candidateExp)
                            {
                                var appexp = _autoMapper.Map<CandidateExperienceViewModel, ApplicationExperienceViewModel>(exp);
                                appexp.ApplicationId = result.Item.Id;
                                appexp.Id = "";
                                var expresult = await base.Create<ApplicationExperienceViewModel, ApplicationExperience>(appexp);
                            }
                        }
                        // Copy Candidate Computer Proficiency
                        var candidateCompProficiency = await _repo.GetList<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateCompProficiency != null && candidateCompProficiency.Count() > 0)
                        {
                            foreach (var Compexp in candidateCompProficiency)
                            {
                                var Compexperience = _autoMapper.Map<CandidateComputerProficiencyViewModel, ApplicationComputerProficiencyViewModel>(Compexp);
                                Compexperience.ApplicationId = result.Item.Id;
                                Compexperience.Id = "";
                                var Compexpresult = await base.Create<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>(Compexperience);
                            }
                        }
                        // Copy Candidate Language Proficiency
                        var candidateLanguageProficiency = await _repo.GetList<CandidateLanguageProficiencyViewModel, CandidateLanguageProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateLanguageProficiency != null && candidateLanguageProficiency.Count() > 0)
                        {
                            foreach (var language in candidateLanguageProficiency)
                            {
                                var lang = _autoMapper.Map<CandidateLanguageProficiencyViewModel, ApplicationLanguageProficiencyViewModel>(language);
                                lang.ApplicationId = result.Item.Id;
                                lang.Id = "";
                                var langresult = await base.Create<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>(lang);
                            }
                        }
                        // Copy Candidate Experience By Country
                        var candidateexpByCountry = await _repo.GetList<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateexpByCountry != null && candidateexpByCountry.Count() > 0)
                        {
                            foreach (var expByountry in candidateexpByCountry)
                            {
                                var countryexp = _autoMapper.Map<CandidateExperienceByCountryViewModel, ApplicationExperienceByCountryViewModel>(expByountry);
                                countryexp.ApplicationId = result.Item.Id;
                                countryexp.Id = "";
                                var countryexpresult = await base.Create<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>(countryexp);
                            }
                        }
                        // Copy Candidate Experience By Sector
                        var candidateexpBySector = await _repo.GetList<CandidateExperienceBySectorViewModel, CandidateExperienceBySector>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateexpBySector != null && candidateexpBySector.Count() > 0)
                        {
                            foreach (var expBySector in candidateexpBySector)
                            {
                                var Sectorexp = _autoMapper.Map<CandidateExperienceBySectorViewModel, ApplicationExperienceBySectorViewModel>(expBySector);
                                Sectorexp.ApplicationId = result.Item.Id;
                                Sectorexp.Id = "";
                                var Sectorexpresult = await base.Create<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>(Sectorexp);
                            }
                        }
                        // Copy Candidate Experience By Nature
                        var candidateExpByNature = await _repo.GetList<CandidateExperienceByNatureViewModel, CandidateExperienceByNature>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateExpByNature != null && candidateExpByNature.Count() > 0)
                        {
                            foreach (var expByNature in candidateExpByNature)
                            {
                                var Natureexp = _autoMapper.Map<CandidateExperienceByNatureViewModel, ApplicationeExperienceByNatureViewModel>(expByNature);
                                Natureexp.ApplicationId = result.Item.Id;
                                Natureexp.Id = "";
                                var Natureexpresult = await base.Create<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>(Natureexp);
                            }
                        }
                        // Copy Candidate Experience By Job
                        var candidateExpByJob = await _repo.GetList<CandidateExperienceByJobViewModel, CandidateExperienceByJob>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateExpByJob != null && candidateExpByJob.Count() > 0)
                        {
                            foreach (var expByJob in candidateExpByJob)
                            {
                                var Jobexp = _autoMapper.Map<CandidateExperienceByJobViewModel, ApplicationExperienceByJobViewModel>(expByJob);
                                Jobexp.ApplicationId = result.Item.Id;
                                Jobexp.Id = "";
                                var Natureexpresult = await base.Create<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>(Jobexp);
                            }
                        }
                        // Copy Candidate Driving Liciense Detail
                        var candidateDL = await _repo.GetList<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateDL != null && candidateDL.Count() > 0)
                        {
                            foreach (var dl in candidateDL)
                            {
                                var DL = _autoMapper.Map<CandidateDrivingLicenseViewModel, ApplicationDrivingLicenseViewModel>(dl);
                                DL.ApplicationId = result.Item.Id;
                                DL.Id = "";
                                var Natureexpresult = await base.Create<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>(DL);
                            }
                        }
                        // Copy Candidate Project
                        var candidateProject = await _repo.GetList<CandidateProjectViewModel, CandidateProject>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateProject != null && candidateProject.Count() > 0)
                        {
                            foreach (var project in candidateProject)
                            {
                                var candidateProj = _autoMapper.Map<CandidateProjectViewModel, ApplicationProjectViewModel>(project);
                                candidateProj.ApplicationId = result.Item.Id;
                                candidateProj.Id = "";
                                var Natureexpresult = await base.Create<ApplicationProjectViewModel, ApplicationProject>(candidateProj);
                            }
                        }
                        // Copy Candidate References
                        var candidateReferences = await _repo.GetList<CandidateReferencesViewModel, CandidateReferences>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateReferences != null && candidateReferences.Count() > 0)
                        {
                            foreach (var reference in candidateReferences)
                            {
                                var candidateref = _autoMapper.Map<CandidateReferencesViewModel, ApplicationReferencesViewModel>(reference);
                                candidateref.ApplicationId = result.Item.Id;
                                candidateref.Id = "";
                                var Natureexpresult = await base.Create<ApplicationReferencesViewModel, ApplicationReferences>(candidateref);
                            }
                        }
                        // Copy Candidate Other
                        var candidateOther = await _repo.GetList<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                        if (candidateOther != null && candidateOther.Count() > 0)
                        {
                            foreach (var reference in candidateOther)
                            {
                                var candidateref = _autoMapper.Map<CandidateExperienceByOtherViewModel, ApplicationExperienceByOtherViewModel>(reference);
                                candidateref.ApplicationId = result.Item.Id;
                                candidateref.Id = "";
                                var Natureexpresult = await base.Create<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>(candidateref);
                            }
                        }
                        var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                        ApplicationStateTrack.ApplicationId = data.Id;
                        ApplicationStateTrack.ApplicationStateId = state1.Id;
                        ApplicationStateTrack.ApplicationStatusId = status1.Id;
                        ApplicationStateTrack.ChangedBy = _userContext.UserId;
                        ApplicationStateTrack.ChangedDate = DateTime.Now;
                        var result1 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                        if (status == "SHORTLISTED")
                        {
                            await CreateApplicationStatusTrack(data.Id, "SL_BATCH");
                        }
                    }
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }

        }
        public async Task<IList<IdNameViewModel>> GetListOfValueByType(string Type)
        {
            //var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"SELECT l.'Id' ,l.'Name' 
            //                    FROM dbo.'ListOfValue' as l where l.'ListOfValueType'='"+Type+"'", null);
            var list = await _queryRepoIdName.ExecuteQueryList(@$"SELECT ""Id"",""Name"" 
                                FROM rec.""ListOfValue"" where ""ListOfValueType""='" + Type + "'", null);
            //var list = new List<IdNameViewModel>();
            //foreach (DataRow row in existColumn.Rows)
            //{
            //    var data = new IdNameViewModel();
            //    data.Name = row["Name"].ToString();
            //    data.Id = row["Id"].ToString();
            //    list.Add(data);
            //}
            return list;
        }



        public async Task<IList<IdNameViewModel>> GetListOfValueAction(string Type, string Code)
        {
            //var existColumn = await _queryRepo.ExecuteQueryDataTable(@$"SELECT l.'Id' ,l.'Name' 
            //                    FROM dbo.'ListOfValue' as l where l.'ListOfValueType'='"+Type+"'", null);
            var existColumn = await _queryRepoIdName.ExecuteQueryList(@$"SELECT ""Id"",""Name"" 
                                FROM rec.""ListOfValue"" where ""ListOfValueType""='{Type}' and ""Code"" ='{Code}' ", null);


            return existColumn.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetCountryIdNameList()
        {
            string query = @$"SELECT ""Id"", ""Name""
                                FROM cms.""Country""";
            var queryData = await _queryRepoIdName.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetNationalityIdNameList()
        {
            string query = @$"SELECT ""Id"", ""Name""
                                FROM cms.""Nationality""";
            var queryData = await _queryRepoIdName.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        //        public async Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search)
        //        {
        //            try
        //            {
        //                var query = "";
        //                //                if (search.ManpowerType != null && search.ManpowerType != "Staff")
        //                //                {
        //                //                    query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
        //                //app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
        //                //app.""LastName"" as LastName, app.""Age"" as Age,
        //                //app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
        //                //n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
        //                //gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
        //                //app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountryId,
        //                //app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
        //                //app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
        //                //app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
        //                //app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
        //                //app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
        //                //app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
        //                //app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
        //                //app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
        //                //appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
        //                //apps.""Id"" as ApplicationState,
        //                //app.""Score"" as Score,
        //                //app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
        //                //,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
        //                //appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
        //                //bs.""Code"" as BatchStatusCode,appc.""Comment"" as Comment

        //                //FROM rec.""Application"" as app
        //                //left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
        //                //left join cms.""Job"" as job on job.""Id"" = app.""JobId""
        //                //left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
        //                //left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
        //                //left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
        //                //left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
        //                //left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
        //                //left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
        //                //left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
        //                //left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
        //                //left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
        //                //left join rec.""ApplicationDrivingLicense"" as adl on adl.""ApplicationId"" = app.""Id"" 
        //                //left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
        //                //left join rec.""ApplicationEducational"" as aed on aed.""ApplicationId"" = app.""Id"" 
        //                //left join rec.""ApplicationExperience"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
        //                //left join rec.""ApplicationExperienceByCountry"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
        //                //left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
        //                //left join rec.""ApplicationExperienceByJob"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
        //                //left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
        //                //left join rec.""ApplicationExperienceBySector"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
        //                //left join (select appexpbyc.*
        //                // from rec.""ApplicationExperienceByCountry"" as appexpbyc 

        //                //           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId""  and cou.""Code"" = 'Ind'
        //                //) as appexpbyc   on appexpbyc.""ApplicationId"" = app.""Id""
        //                // left join (select appexpba.*, ct.""Code""
        //                // from rec.""ApplicationExperienceByOther"" as appexpba
        //                // join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
        //                // where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
        //                //and appexpboa.""Code"" = 'Abroad'
        //                //left join(select appexpbg.*, ctt.""Code""
        //                // from rec.""ApplicationExperienceByOther"" as appexpbg
        //                // join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
        //                // where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
        //                //and appexpbog.""Code"" = 'Gulf'
        //                // left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
        //                // from rec.""ApplicationLanguageProficiency"" as cp
        //                // join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
        //                // join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
        //                // where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
        //                //and appcpe.""Code"" = 'English'
        //                //left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
        //                // from rec.""ApplicationLanguageProficiency"" as cp
        //                // join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
        //                // join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
        //                // where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
        //                //and appcpa.""Code"" = 'Arabic'
        //                //left join rec.""ApplicationComputerProficiency"" as appcpc on appcpc.""ApplicationId""=app.""Id""
        //                //left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
        //                //left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
        //                //left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 
        //                //left join rec.""ApplicationStateComment"" as appc on appc.""ApplicationId""=app.""Id""
        //                //left join rec.""Batch"" as b on b.""Id""=app.""BatchId""
        //                //left join rec.""ListOfValue"" as bs on bs.""Id"" = b.""BatchStatus"" and bs.""ListOfValueType"" = 'BatchStatus'
        //                //where app.""JobAdvertisementId""='" + search.JobTitleForHiring + @"' #WHERE# ";

        //                //                }
        //                //                else
        //                //                {
        //                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
        //app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
        //app.""LastName"" as LastName, app.""Age"" as Age,
        //app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
        //n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
        //gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
        //app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountryId,
        //app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
        //app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
        //app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
        //app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
        //app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
        //app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
        //app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
        //app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
        //appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
        //apps.""Id"" as ApplicationState,
        //app.""Score"" as Score,
        //app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
        //,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
        //appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
        //bs.""Code"" as BatchStatusCode,appc.""Comment"" as Comment,b.""Name"" as BatchName

        //FROM rec.""Application"" as app
        //left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
        //left join cms.""Job"" as job on job.""Id"" = app.""JobId""
        //left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
        //left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
        //left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
        //left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
        //left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
        //left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
        //left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
        //left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
        //left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
        //left join rec.""ApplicationDrivingLicense"" as adl on adl.""ApplicationId"" = app.""Id"" 
        //left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
        //left join rec.""ApplicationEducational"" as aed on aed.""ApplicationId"" = app.""Id"" 
        //left join rec.""ApplicationExperience"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
        //left join rec.""ApplicationExperienceByCountry"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
        //left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
        //left join rec.""ApplicationExperienceByJob"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
        //left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
        //left join rec.""ApplicationExperienceBySector"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
        //left join (select appexpbyc.*
        // from rec.""ApplicationExperienceByCountry"" as appexpbyc 

        //           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId""  and cou.""Code"" = 'Ind'
        //) as appexpbyc   on appexpbyc.""ApplicationId"" = app.""Id""
        // left join (select appexpba.*, ct.""Code""
        // from rec.""ApplicationExperienceByOther"" as appexpba
        // join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
        // where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
        //and appexpboa.""Code"" = 'Abroad'
        //left join(select appexpbg.*, ctt.""Code""
        // from rec.""ApplicationExperienceByOther"" as appexpbg
        // join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
        // where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
        //and appexpbog.""Code"" = 'Gulf'
        // left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
        // from rec.""ApplicationLanguageProficiency"" as cp
        // join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
        // join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
        // where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
        //and appcpe.""Code"" = 'English'
        //left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
        // from rec.""ApplicationLanguageProficiency"" as cp
        // join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
        // join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
        // where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
        //and appcpa.""Code"" = 'Arabic'

        //left join rec.""ApplicationComputerProficiency"" as appcpc on appcpc.""ApplicationId""=app.""Id""
        //left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
        //left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
        //left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 
        //left join rec.""ApplicationStateComment"" as appc on appc.""ApplicationId""=app.""Id""
        //left join rec.""Batch"" as b on b.""Id""=app.""BatchId""
        //left join rec.""ListOfValue"" as bs on bs.""Id"" = b.""BatchStatus"" and bs.""ListOfValueType"" = 'BatchStatus'
        //where app.""JobId""='" + search.JobTitleForHiring + @"' #WHERE# 

        //union

        //Select distinct 'CandidateProfile' as CandidateType,'' as ApplicationId,app.""Id"" as CandidateProfileId,
        //app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName, app.""LastName"" as LastName,
        //app.""Age"" as Age, app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
        //n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
        //gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
        //app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountry,
        //app.""PassportExpiryDate"" as PassportExpiryDate,
        //app.""CurrentAddressHouse"" as CurrentAddressHouse,
        //app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
        //app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
        //app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet, app.""PermanentAddressCity"" as PermanentAddressCity,
        //app.""PermanentAddressState"" as PermanentAddressState, app.""TotalWorkExperience"" as TotalWorkExperience,
        //pac.""Name"" as PermanentAddressCountryName,
        //app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, app.""ContactPhoneLocal"" as ContactPhoneLocal,
        //'' as JobId,
        //'UnReviewed' as ApplicationStatusCode,'UnReviewed' as ApplicationStateName,  '' as ApplicationStateCode,
        //'' as ApplicationState,
        //CAST ('0.0' AS DOUBLE PRECISION) as Score,
        // app.""NetSalary"" as NetSalary,
        //app.""OtherAllowances"" as OtherAllowances
        //,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
        //appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
        //'' as BatchStatusCode,'' as Comment,'' as BatchName
        //FROM rec.""CandidateProfile"" as app
        //left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
        //left join rec.""Application"" as application on application.""CandidateProfileId""=app.""Id""  and  application.""JobId""='"+ search.JobTitleForHiring + @"'
        //left join cms.""Country"" pac on pac.""Id"" = app.""PermanentAddressCountryId""
        //left join cms.""Country"" cac on cac.""Id"" = app.""CurrentAddressCountryId""
        //left join cms.""Country"" pic on pic.""Id"" = app.""PassportIssueCountryId""
        //left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
        //left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
        //left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
        //left join rec.""CandidateDrivingLicense"" as adl on adl.""CandidateProfileId"" = app.""Id"" 
        //left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
        //left join rec.""CandidateEducational"" as aed on aed.""CandidateProfileId"" = app.""Id"" 
        //left join rec.""CandidateExperience"" as appexp on appexp.""CandidateProfileId"" = app.""Id""
        //left join rec.""CandidateExperienceByCountry"" as appexpc on appexpc.""CandidateProfileId"" = app.""Id"" 
        //left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
        //left join rec.""CandidateExperienceByJob"" as appexpj on appexpj.""CandidateProfileId"" = app.""Id"" 
        //left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
        //left join rec.""CandidateExperienceBySector"" as appexpsec on appexpsec.""CandidateProfileId"" = app.""Id"" 
        // left join (select appexpbyc.*
        // from rec.""CandidateExperienceByCountry"" as appexpbyc 

        //           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId"" and cou.""Code"" = 'Ind'
        // ) as appexpbyc  on appexpbyc.""CandidateProfileId"" = app.""Id""
        // left join (select appexpba.*, ct.""Code""
        // from rec.""CandidateExperienceByOther"" as appexpba
        // join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
        // where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""CandidateProfileId"" = app.""Id""
        //and appexpboa.""Code"" = 'Abroad'
        //left join(select appexpbg.*, ctt.""Code""
        // from rec.""CandidateExperienceByOther"" as appexpbg
        // join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
        // where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""CandidateProfileId"" = app.""Id""
        //and appexpbog.""Code"" = 'Gulf'
        // left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
        // from rec.""CandidateLanguageProficiency"" as cp
        // join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
        // join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
        // where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""CandidateProfileId""=app.""Id""
        //and appcpe.""Code"" = 'English'
        //left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
        // from rec.""CandidateLanguageProficiency"" as cp
        // join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
        // join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
        // where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""CandidateProfileId"" = app.""Id""
        //and appcpa.""Code"" = 'Arabic'

        //left join rec.""CandidateComputerProficiency"" as appcpc on appcpc.""CandidateProfileId""=app.""Id""
        //left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
        //left join rec.""CandidateReferences"" as appr on appr.""CandidateProfileId"" = app.""Id"" 
        //left join rec.""CandidateExperienceByNature"" as appebn on appebn.""CandidateProfileId"" = app.""Id"" 
        //where application.""Id"" is null and app.""SourceFrom""='Migrated' #WHERE#";
        //                // }

        //                var where = "";
        //                if (search.TotalExperience.IsNotNull())
        //                {
        //                    where += @" and app.""TotalWorkExperience"">='" + search.TotalExperience + "'";// and app.""TotalWorkExperience""='{search.TotalExperience}'";
        //                }
        //                if (search.TotalGulfExperience.IsNotNull())
        //                {
        //                    where += @" and appexpbog.""NoOfYear"">='" + search.TotalGulfExperience + "'";// and app.""TotalWorkExperience""='{search.TotalExperience}'";
        //                }
        //                if (search.JobTitle.IsNotNullAndNotEmpty())
        //                {
        //                    where += @" and ej.""Name""='" + search.JobTitle + "'";
        //                }
        //                if (search.YearOfJobExperience.IsNotNull())
        //                {
        //                    where += @" and appexpj.""NoOfYear"" >= '" + search.YearOfJobExperience + "'";
        //                }
        //                if (search.OtherExperience.IsNotNull())
        //                {
        //                    where += @" and appexpc.""CountryId"" =  '" + search.OtherExperience + "'";
        //                }
        //                if (search.YearOfOtherCountryExperience.IsNotNull())
        //                {
        //                    where += @" and appexpc.""NoOfYear"" >=  '" + search.YearOfOtherCountryExperience + "'";
        //                }
        //                if (search.Industry.IsNotNull())
        //                {
        //                    where += @" and appexpsec.""Industry""='" + search.Industry + "'";
        //                }
        //                if (search.Category.IsNotNull())
        //                {
        //                    where += @" and appexpsec.""Category"" =  '" + search.Category + "'";
        //                }
        //                if (search.YearOfIndustryExperience.IsNotNull())
        //                {
        //                    where += @" and appexpsec.""NoOfYear"" >=  '" + search.YearOfIndustryExperience + "'";
        //                }
        //                if (search.CategoryExperience.IsNotNull())
        //                {
        //                    where += @" and appexpsec.""NoOfYear"" >=  '" + search.CategoryExperience + "'";
        //                }
        //                if (search.IsEnglishProficiency == true)
        //                {
        //                    if (search.EnglishProficiency.IsNotNull())
        //                    {
        //                        where += @" and appcpe.""cpl"" =  '" + search.EnglishProficiency + "'";
        //                    }
        //                }
        //                if (search.IsArabicProficiency == true)
        //                {
        //                    if (search.ArabicProficiency.IsNotNull())
        //                    {
        //                        where += @" and appcpa.""cpl"" =  '" + search.ArabicProficiency + "'";
        //                    }
        //                }
        //                if (search.IsComputerLiteratureProficiency == true)
        //                {
        //                    if (search.ComputerLiteratureProficiency.IsNotNull())
        //                    {
        //                        where += @" and cplv.""Code"" =  '" + search.ComputerLiteratureProficiency + "'";
        //                    }
        //                }

        //                if (search.Qualification.IsNotNull())
        //                {
        //                    where += @" and aed.""QualificationId"" = '" + search.Qualification + "'";
        //                }
        //                if (search.Specialization.IsNotNull())
        //                {
        //                    where += @" and aed.""Specialization"" = '" + search.Specialization + "'";
        //                }
        //                if (search.Duration.IsNotNull())
        //                {
        //                    where += @" and aed.""Duration"" = '" + search.Duration + "'";
        //                }
        //                if (search.PassingYear.IsNotNull())
        //                {
        //                    where += @" and aed.""PassingYear"" = '" + search.PassingYear + "'";
        //                }
        //                if (search.Marks.IsNotNull())
        //                {
        //                    where += @" and aed.""Marks"" = '" + search.Marks + "'";
        //                }
        //                if (search.DL == "YES")
        //                {
        //                    if (search.Country.IsNotNullAndNotEmpty())
        //                    {
        //                        where += @" and adl.""CountryId"" = '" + search.Country + "'";
        //                    }
        //                    if (search.Type.IsNotNullAndNotEmpty())
        //                    {
        //                        where += @" and adl.""LicenseType"" ='" + search.Type + "'";
        //                    }
        //                    if (search.IssueDate.IsNotNull())
        //                    {
        //                        where += @" and adl.""IssueDate"" >= '" + search.IssueDate + "'";
        //                    }
        //                    if (search.ExpiryDate.IsNotNull())
        //                    {
        //                        where += @" and adl.""ValidUpTo"">='" + search.ExpiryDate + "'";
        //                    }
        //                }
        //                if (search.BirthDate.IsNotNull())
        //                {
        //                    where += @" and app.""BirthDate"" = '" + search.BirthDate + "'";
        //                }
        //                if (search.PassportNumber.IsNotNull())
        //                {
        //                    where += @" and app.""PassportNumber"" = '" + search.PassportNumber + "'";
        //                }
        //                if (search.NetSalary.IsNotNullAndNotEmpty())
        //                {
        //                    where += @" and app.""NetSalary"" = '" + search.NetSalary + "'";
        //                }
        //                if (search.Comment.IsNotNullAndNotEmpty())
        //                {
        //                    where += @" and appc.""Comment"" like '%" + search.NetSalary + "%'";
        //                }
        //                if (search.Nationality.IsNotNull())
        //                {
        //                    where += @" and n.""Id"" = '" + search.Nationality + "'";
        //                }
        //                if (search.Age.IsNotNull())
        //                {
        //                    where += @" and app.""Age"" = '" + search.Age + "'";
        //                }
        //                if (search.Gender.IsNotNull())
        //                {
        //                    where += @" and gen.""Id"" = '" + search.Gender + "'";
        //                }
        //                query = query.Replace("#WHERE#", where);
        //                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
        //                //if (search.ManpowerType != "Staff")
        //                //{
        //                //    allList = allList.Where(x=>x.CandidateType=="JobApplication").ToList();
        //                //}
        //                if (search.CandidateProfileSearch == true && search.JobApplicationSearch == false)
        //                {
        //                    List<ApplicationViewModel> list = new List<ApplicationViewModel>();
        //                    //if (search.AllCandidateApplication)
        //                    //{
        //                    return allList.Where(x => x.CandidateType == "CandidateProfile").ToList();
        //                    //return allList;                  
        //                    //}
        //                    //else
        //                    //{
        //                    //    return list;
        //                    //}

        //                }
        //                else if (search.CandidateProfileSearch == false && search.JobApplicationSearch == true)
        //                {
        //                    List<ApplicationViewModel> list = new List<ApplicationViewModel>();
        //                    if (!search.AllCandidateApplication && !search.ShortlistedCandidateApplication && !search.RejectedCandidateSearch && !search.WaitlistedCandidateSearch)
        //                    {
        //                        list.AddRange(allList.Where(x =>/*  x.ApplicationStateCode == "ShortListByHr" &&*/ x.CandidateType == "JobApplication").ToList());
        //                    }
        //                    if (search.AllCandidateApplication)
        //                    {
        //                        list.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" && x.CandidateType == "JobApplication").ToList());
        //                    }
        //                    //else
        //                    //{
        //                    //    list.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" || x.ApplicationStateCode == "ShortListByHr" && x.CandidateType == "JobApplication").ToList());
        //                    //}
        //                    if (search.ShortlistedCandidateApplication)
        //                    {
        //                        // return allList.Where(x => x.CandidateType == "CandidateProfile").ToList();
        //                        list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "SHORTLISTED").ToList());
        //                    }
        //                    if (search.RejectedCandidateSearch)
        //                    {
        //                        // list.AddRange(JobApplicationList.Where(x => x.ApplicationStatusCode == "Rejected").ToList());
        //                        list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "REJECTED").ToList());
        //                    }
        //                    if (search.WaitlistedCandidateSearch)
        //                    {
        //                        list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "WAITLISTED").ToList());
        //                        //list.AddRange(JobApplicationList.Where(x => x.ApplicationStatus == "Waitlisted").ToList());
        //                    }
        //                    return list;
        //                    //if (list.Count() > 0)
        //                    //{
        //                    //    return list;
        //                    //}
        //                    //return allList.Where(x => x.CandidateType == "JobApplication").ToList();
        //                }
        //                else
        //                {

        //                    List<ApplicationViewModel> Newlist = new List<ApplicationViewModel>();
        //                    Newlist.AddRange(allList.Where(x => x.CandidateType == "CandidateProfile").ToList());
        //                    if (!search.AllCandidateApplication && !search.ShortlistedCandidateApplication && !search.RejectedCandidateSearch && !search.WaitlistedCandidateSearch)
        //                    {
        //                        Newlist.AddRange(allList.Where(x => /*x.ApplicationStateCode == "UnReviewed" && x.ApplicationStateCode == "ShortListByHr" &&*/ x.CandidateType == "JobApplication").ToList());
        //                    }
        //                    if (search.AllCandidateApplication)
        //                    {
        //                        Newlist.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" && x.CandidateType == "JobApplication").ToList());
        //                    }
        //                    if (search.ShortlistedCandidateApplication)
        //                    {
        //                        Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "SHORTLISTED").ToList());
        //                    }
        //                    if (search.RejectedCandidateSearch)
        //                    {
        //                        Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "REJECTED").ToList());

        //                    }
        //                    if (search.WaitlistedCandidateSearch)
        //                    {
        //                        Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "WAITLISTED").ToList());
        //                    }

        //                    if (Newlist.Count() > 0)
        //                    {

        //                        //return Newlist;
        //                    }
        //                    else
        //                    {
        //                        Newlist = allList.ToList();
        //                        // return allList;
        //                    }
        //                    return Newlist;
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //            }
        //        }
        public async Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search)
        {
            try
            {
                var query = "";
                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Name"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
apps.""Id"" as ApplicationState,appStatus.""Id"" as ApplicationStatus,
app.""Score"" as Score, app.""QatarId"" as QatarId,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
bs.""Code"" as BatchStatusCode,#APPCOMMENT1# b.""Name"" as BatchName

FROM rec.""Application"" as app
left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""ApplicationDrivingLicense"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
left join rec.""ApplicationEducational"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperience"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperienceByCountry"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
left join rec.""ApplicationExperienceByJob"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
left join rec.""ApplicationExperienceBySector"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
left join (select appexpbyc.*
 from rec.""ApplicationExperienceByCountry"" as appexpbyc 

           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId""  and cou.""Code"" = 'Ind'
) as appexpbyc   on appexpbyc.""ApplicationId"" = app.""Id""
 left join (select appexpba.*, ct.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpba
 join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
 where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
and appexpboa.""Code"" = 'Abroad'
left join(select appexpbg.*, ctt.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpbg
 join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf'
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'

left join rec.""ApplicationComputerProficiency"" as appcpc on appcpc.""ApplicationId""=app.""Id""
left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 
#APPCOMMENT#
left join rec.""Batch"" as b on b.""Id""=app.""BatchId""
left join rec.""ListOfValue"" as bs on bs.""Id"" = b.""BatchStatus"" and bs.""ListOfValueType"" = 'BatchStatus'
where app.""JobId""='" + search.JobTitleForHiring + @"' 
--and app.""OrganizationId""='" + search.OrganizationId + @"' 
and ((apps.""Code""='ShortListByHR' and appStatus.""Code"" in ('WAITLISTED' ,'REJECTED')) or apps.""Code""='Rereviewed' or apps.""Code""='UnReviewed' )#WHERE# 

union

Select distinct 'CandidateProfile' as CandidateType,'' as ApplicationId,app.""Id"" as CandidateProfileId,
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName, app.""LastName"" as LastName,
app.""Age"" as Age, app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Name"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate,
app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet, app.""PermanentAddressCity"" as PermanentAddressCity,
app.""PermanentAddressState"" as PermanentAddressState, app.""TotalWorkExperience"" as TotalWorkExperience,
pac.""Name"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, app.""ContactPhoneLocal"" as ContactPhoneLocal,
'' as JobId,
'UnReviewed' as ApplicationStatusCode,'UnReviewed' as ApplicationStateName,  '' as ApplicationStateCode,
'' as ApplicationState,'' as ApplicationStatus,
CAST ('0.0' AS DOUBLE PRECISION) as Score,app.""QatarId"" as QatarId,
 app.""NetSalary"" as NetSalary,
app.""OtherAllowances"" as OtherAllowances
,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
'' as BatchStatusCode,'' as Comment,'' as BatchName
FROM rec.""CandidateProfile"" as app
left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join rec.""Application"" as application on application.""CandidateProfileId""=app.""Id""  
--and  application.""JobId""='" + search.JobTitleForHiring + @"' and  application.""OrganizationId""='" + search.OrganizationId + @"'
left join cms.""Country"" pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" pic on pic.""Id"" = app.""PassportIssueCountryId""
left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""CandidateDrivingLicense"" as adl on adl.""CandidateProfileId"" = app.""Id"" 
left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
left join rec.""CandidateEducational"" as aed on aed.""CandidateProfileId"" = app.""Id"" 
left join rec.""CandidateExperience"" as appexp on appexp.""CandidateProfileId"" = app.""Id""
left join rec.""CandidateExperienceByCountry"" as appexpc on appexpc.""CandidateProfileId"" = app.""Id"" 
left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
left join rec.""CandidateExperienceByJob"" as appexpj on appexpj.""CandidateProfileId"" = app.""Id"" 
left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
left join rec.""CandidateExperienceBySector"" as appexpsec on appexpsec.""CandidateProfileId"" = app.""Id"" 
 left join (select appexpbyc.*
 from rec.""CandidateExperienceByCountry"" as appexpbyc 

           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId"" and cou.""Code"" = 'Ind'
 ) as appexpbyc  on appexpbyc.""CandidateProfileId"" = app.""Id""
 left join (select appexpba.*, ct.""Code""
 from rec.""CandidateExperienceByOther"" as appexpba
 join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
 where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""CandidateProfileId"" = app.""Id""
and appexpboa.""Code"" = 'Abroad'
left join(select appexpbg.*, ctt.""Code""
 from rec.""CandidateExperienceByOther"" as appexpbg
 join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""CandidateProfileId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf'
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""CandidateLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""CandidateProfileId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""CandidateLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""CandidateProfileId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'

left join rec.""CandidateComputerProficiency"" as appcpc on appcpc.""CandidateProfileId""=app.""Id""
left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
left join rec.""CandidateReferences"" as appr on appr.""CandidateProfileId"" = app.""Id"" 
left join rec.""CandidateExperienceByNature"" as appebn on appebn.""CandidateProfileId"" = app.""Id"" 
where application.""Id"" is null 
--and application.""OrganizationId"" is null 
and app.""SourceFrom""='Migrated' #WHERE#";
                // }

                var where = "";
                var appcomment1 = @" '' as Comment, ";
                var appcomment = "";
                if (search.TotalExperience.IsNotNull())
                {
                    where += @" and app.""TotalWorkExperience"">='" + search.TotalExperience + "'";// and app.""TotalWorkExperience""='{search.TotalExperience}'";
                }
                if (search.TotalGulfExperience.IsNotNull())
                {
                    where += @" and appexpbog.""NoOfYear"">='" + search.TotalGulfExperience + "'";// and app.""TotalWorkExperience""='{search.TotalExperience}'";
                }
                if (search.JobTitle.IsNotNullAndNotEmpty())
                {
                    where += @" and ej.""Name""='" + search.JobTitle + "'";
                }
                if (search.YearOfJobExperience.IsNotNull())
                {
                    where += @" and appexpj.""NoOfYear"" >= '" + search.YearOfJobExperience + "'";
                }
                if (search.OtherExperience.IsNotNull())
                {
                    where += @" and appexpc.""CountryId"" =  '" + search.OtherExperience + "'";
                }
                if (search.YearOfOtherCountryExperience.IsNotNull())
                {
                    where += @" and appexpc.""NoOfYear"" >=  '" + search.YearOfOtherCountryExperience + "'";
                }
                if (search.Industry.IsNotNull())
                {
                    where += @" and appexpsec.""Industry""='" + search.Industry + "'";
                }
                if (search.Category.IsNotNull())
                {
                    where += @" and appexpsec.""Category"" =  '" + search.Category + "'";
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
                        where += @" and appcpe.""cpl"" =  '" + search.EnglishProficiency + "'";
                    }
                }
                if (search.IsArabicProficiency == true)
                {
                    if (search.ArabicProficiency.IsNotNull())
                    {
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

                if (search.Qualification.IsNotNull())
                {
                    where += @" and aed.""QualificationId"" = '" + search.Qualification + "'";
                }
                if (search.Specialization.IsNotNull())
                {
                    where += @" and aed.""Specialization"" = '" + search.Specialization + "'";
                }
                if (search.Duration.IsNotNull())
                {
                    where += @" and aed.""Duration"" = '" + search.Duration + "'";
                }
                if (search.PassingYear.IsNotNull())
                {
                    where += @" and aed.""PassingYear"" = '" + search.PassingYear + "'";
                }
                if (search.Marks.IsNotNull())
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
                        where += @" and adl.""IssueDate"" >= '" + search.IssueDate + "'";
                    }
                    if (search.ExpiryDate.IsNotNull())
                    {
                        where += @" and adl.""ValidUpTo"">='" + search.ExpiryDate + "'";
                    }
                }
                if (search.BirthDate.IsNotNull())
                {
                    where += @" and app.""BirthDate"" = '" + search.BirthDate + "'";
                }
                if (search.PassportNumber.IsNotNull())
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
                if (search.Nationality.IsNotNull())
                {
                    where += @" and n.""Id"" = '" + search.Nationality + "'";
                }
                if (search.Age.IsNotNull())
                {
                    where += @" and app.""Age"" = '" + search.Age + "'";
                }
                if (search.Gender.IsNotNull())
                {
                    where += @" and gen.""Id"" = '" + search.Gender + "'";
                }
                query = query.Replace("#WHERE#", where);
                if (search.Comment.IsNotNullAndNotEmpty())
                {
                    appcomment1 = @" appc.""Comment"" as Comment, ";
                    appcomment = @" left join rec.""ApplicationStateComment"" as appc on appc.""ApplicationId""=app.""Id"" ";

                }
                query = query.Replace("#APPCOMMENT1#", appcomment1);
                query = query.Replace("#APPCOMMENT#", appcomment);

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
        public async Task<IList<ApplicationViewModel>> GetWorkerPoolData(ApplicationSearchViewModel search)
        {
            try
            {
                var query = "";

                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountryId,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
apps.""Id"" as ApplicationState,app.""SalaryOnAppointment"" as SalaryOnAppointment,
app.""Score"" as Score,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
bs.""Code"" as BatchStatusCode,bs.""Id"" as BatchId,app.""WorkerBatchId"" as WorkerBatchId

FROM rec.""Application"" as app
left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""ApplicationDrivingLicense"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
left join rec.""ApplicationEducational"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperience"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperienceByCountry"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
left join rec.""ApplicationExperienceByJob"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
left join rec.""ApplicationExperienceBySector"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
left join (select appexpbyc.*
 from rec.""ApplicationExperienceByCountry"" as appexpbyc 

           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId""  and cou.""Code"" = 'Ind'
) as appexpbyc   on appexpbyc.""ApplicationId"" = app.""Id""
 left join (select appexpba.*, ct.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpba
 join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
 where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
and appexpboa.""Code"" = 'Abroad'
left join(select appexpbg.*, ctt.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpbg
 join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf'
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'

left join rec.""ApplicationComputerProficiency"" as appcpc on appcpc.""ApplicationId""=app.""Id""
left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 

left join rec.""Batch"" as b on b.""Id""=app.""BatchId""
left join rec.""ListOfValue"" as bs on bs.""Id"" = b.""BatchStatus"" and bs.""ListOfValueType"" = 'BatchStatus'
where  apps.""Code""='WorkerPool'  and app.""JobId""='" + search.JobTitleForHiring + @"' and app.""OrganizationId""='" + search.OrganizationId + @"'";// #WHERE#";
               

                //var where = "";
                //if (search.JobTitleForHiring.IsNotNullAndNotEmpty())
                //{
                //    where += @" and app.""JobId"" = '" + search.JobTitleForHiring + "'";
                //}
                //if (search.OrganizationId.IsNotNull())
                //{
                //    where += @" and bs.""OrganizationId"" = '" + search.OrganizationId + "'";
                //}
                //query = query.Replace("#WHERE#", where);

                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                return allList;
                //if (search.CandidateProfileSearch == true && search.JobApplicationSearch == false)
                //{
                //    List<ApplicationViewModel> list = new List<ApplicationViewModel>();
                //    //if (search.AllCandidateApplication)
                //    //{
                //    return allList.Where(x => x.CandidateType == "CandidateProfile").ToList();
                //    //return allList;                  
                //    //}
                //    //else
                //    //{
                //    //    return list;
                //    //}

                //}
                //else if (search.CandidateProfileSearch == false && search.JobApplicationSearch == true)
                //{
                //    List<ApplicationViewModel> list = new List<ApplicationViewModel>();
                //    if (!search.AllCandidateApplication && !search.ShortlistedCandidateApplication && !search.RejectedCandidateSearch && !search.WaitlistedCandidateSearch)
                //    {
                //        list.AddRange(allList.Where(x =>/*  x.ApplicationStateCode == "ShortListByHr" &&*/ x.CandidateType == "JobApplication").ToList());
                //    }
                //    if (search.AllCandidateApplication)
                //    {
                //        list.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" && x.CandidateType == "JobApplication").ToList());
                //    }
                //    //else
                //    //{
                //    //    list.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" || x.ApplicationStateCode == "ShortListByHr" && x.CandidateType == "JobApplication").ToList());
                //    //}
                //    if (search.ShortlistedCandidateApplication)
                //    {
                //        // return allList.Where(x => x.CandidateType == "CandidateProfile").ToList();
                //        list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "SHORTLISTED").ToList());
                //    }
                //    if (search.RejectedCandidateSearch)
                //    {
                //        // list.AddRange(JobApplicationList.Where(x => x.ApplicationStatusCode == "Rejected").ToList());
                //        list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "REJECTED").ToList());
                //    }
                //    if (search.WaitlistedCandidateSearch)
                //    {
                //        list.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "WAITLISTED").ToList());
                //        //list.AddRange(JobApplicationList.Where(x => x.ApplicationStatus == "Waitlisted").ToList());
                //    }
                //    return list;

                //}
                //else
                //{

                //    List<ApplicationViewModel> Newlist = new List<ApplicationViewModel>();
                //    Newlist.AddRange(allList.Where(x => x.CandidateType == "CandidateProfile").ToList());
                //    if (!search.AllCandidateApplication && !search.ShortlistedCandidateApplication && !search.RejectedCandidateSearch && !search.WaitlistedCandidateSearch)
                //    {
                //        Newlist.AddRange(allList.Where(x => /*x.ApplicationStateCode == "UnReviewed" && x.ApplicationStateCode == "ShortListByHr" &&*/ x.CandidateType == "JobApplication").ToList());
                //    }
                //    if (search.AllCandidateApplication)
                //    {
                //        Newlist.AddRange(allList.Where(x => x.ApplicationStateCode == "UnReviewed" && x.CandidateType == "JobApplication").ToList());
                //    }
                //    if (search.ShortlistedCandidateApplication)
                //    {
                //        Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "SHORTLISTED").ToList());
                //    }
                //    if (search.RejectedCandidateSearch)
                //    {
                //        Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "REJECTED").ToList());

                //    }
                //    if (search.WaitlistedCandidateSearch)
                //    {
                //        Newlist.AddRange(allList.Where(x => x.CandidateType == "JobApplication" && x.ApplicationStateCode == "ShortListByHr" && x.ApplicationStatusCode == "WAITLISTED").ToList());
                //    }

                //    if (Newlist.Count() > 0)
                //    {

                //        //return Newlist;
                //    }
                //    else
                //    {
                //        Newlist = allList.ToList();
                //        // return allList;
                //    }
                //    return Newlist;
                //}
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ApplicationViewModel> GetCandiadteShortListApplicationDataByApplication(string applicationId, string tempCode)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as Id,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,concat(app.""FirstName"",' ',app.""MiddleName"",' ',app.""LastName"") as FullName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
app.""Gender"" as Gender,glov.""Name"" as GenderName, app.""MaritalStatus"" as MaritalStatus,mlov.""Name"" as MaritalStatusName,
app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Id"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,app.""SalaryOnAppointment"" as SalaryOnAppointment,
appStatus.""Id"" as ApplicationStatus,appStatus.""Code"" as ApplicationStatusCode,
apps.""Id"" as ApplicationState,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
app.""Score"" as Score,app.""JobId"" as JobId,u.""Name"" as ""InterviewByUserName"",app.""AccommodationId"" as AccommodationId,
mplov.""Name"" as ManpowerTypeName,mplov.""Code"" as ManpowerTypeCode,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,
app.""JobAdvertisementId"" as JobAdvertisementId,app.""BatchId"" as BatchId,bch.""Name"" as BatchName,
task.""Id"" as TaskId,task.""TemplateCode"" as TaskTemplateCode,task.""TaskStatusCode"" as TaskStatusCode,
bchlov.""Id"" as BatchStatus, bchlov.""Name"" as BatchStatusName, bchlov.""Code"" as BatchStatusCode

FROM rec.""Application"" as app
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join rec.""Batch"" as bch on bch.""Id""=app.""BatchId""
left join rec.""ListOfValue"" as bchlov on bchlov.""Id""=bch.""BatchStatus""
left join rec.""ListOfValue"" as glov on glov.""Id""=app.""Gender""
left join rec.""ListOfValue"" as mlov on mlov.""Id""=app.""MaritalStatus""
left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""ApplicationDrivingLicense"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
left join rec.""ApplicationEducational"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperience"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperienceByCountry"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
left join rec.""ApplicationExperienceByJob"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
left join rec.""ApplicationExperienceBySector"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
left join rec.""JobAdvertisement"" as mjd on mjd.""Id"" = app.""JobAdvertisementId""
Left join rec.""ListOfValue"" as mplov ON mplov.""ListOfValueType""='LOV_MANPOWERTYPE' and mplov.""Code"" = job.""ManpowerTypeCode""
LEFT JOIN public.""User"" as u ON u.""Id""=app.""InterviewByUserId""
left join public.""RecTask"" as task on task.""ReferenceTypeId""=app.""Id"" AND task.""ReferenceTypeCode""='32' AND task.""TemplateCode""='{tempCode}'
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpc on appcpc.""ApplicationId"" = app.""Id""
and appcpc.""Code"" = 'ComputerProficiency'
left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 
WHERE app.""Id""='{applicationId}'
";
                var appdata = await _queryRepo.ExecuteQuerySingle<ApplicationViewModel>(query, null);

                return appdata;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IList<ApplicationViewModel>> GetCandiadteShortListApplicationData(ApplicationSearchViewModel search)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as Id,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
app.""Gender"" as Gender,glov.""Name"" as GenderName, app.""MaritalStatus"" as MaritalStatus,mlov.""Name"" as MaritalStatusName,
app.""PassportNumber"" as PassportNumber, pic.""Name"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Id"" as ApplicationStatus,appStatus.""Code"" as ApplicationStatusCode, appStatus.""Name"" as ApplicationStatusName,
apps.""Id"" as ApplicationState,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
app.""Score"" as Score,
mplov.""Name"" as ManpowerTypeName,mplov.""Code"" as ManpowerTypeCode,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,cu.""Name"" as NetSalaryCurrency,
app.""BatchId"" as BatchId,bch.""Name"" as BatchName,
task.""Id"" as TaskId,task.""TemplateCode"" as TaskTemplateCode,task.""TaskStatusCode"" as TaskStatusCode,
bchlov.""Id"" as BatchStatus, bchlov.""Name"" as BatchStatusName, bchlov.""Code"" as BatchStatusCode,app.""ManagerJobTitleAndNoOfSubordinate""
as ManagerJobTitleAndNoOfSubordinate, app.""TimeRequiredToJoin"" as TimeRequiredToJoin,app.""AdditionalInformation"" as AdditionalInformation,
app.""OptionForAnotherPosition"" as OptionForAnotherPosition,app.""NoOfChildren"" as NoOfChildren

FROM rec.""Application"" as app
left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""Batch"" as bch on bch.""Id""=app.""BatchId""
left join rec.""ListOfValue"" as bchlov on bchlov.""Id""=bch.""BatchStatus""
left join rec.""ListOfValue"" as glov on glov.""Id""=app.""Gender""
left join rec.""ListOfValue"" as mlov on mlov.""Id""=app.""MaritalStatus""
left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""JobAdvertisement"" as mjd on mjd.""Id"" = app.""JobAdvertisementId""

Left join rec.""ListOfValue"" as mplov ON mplov.""ListOfValueType""='LOV_MANPOWERTYPE' and mplov.""Code"" = job.""ManpowerTypeCode""
left join public.""RecTask"" as task on task.""ReferenceTypeId""=app.""Id"" AND task.""ReferenceTypeCode""='32' AND task.""TemplateCode""='{search.TemplateCode}'
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpc on appcpc.""ApplicationId"" = app.""Id""
and appcpc.""Code"" = 'ComputerProficiency'

";
                //where app.""JobAdvertisementId""='" + search.JobTitleForHiring + @"' #WHERE# 

                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);

                //var Newlist = allList.Where(x => x.ApplicationStateCode == search.ApplicationStateCode && x.BatchStatusCode=="Draft").ToList();


                List<ApplicationViewModel> Newlist = new List<ApplicationViewModel>();
                Newlist = allList;
                if (search.ApplicationStateCode.IsNotNullAndNotEmpty())
                {
                    //Newlist.AddRange(allList.Where(x => x.ApplicationStateCode == search.ApplicationStateCode).ToList());
                    Newlist = Newlist.Where(x => x.ApplicationStateCode == search.ApplicationStateCode).ToList();
                    //if (search.ApplicationStateCode == "InterviewsCompleted")
                    //{
                    //    Newlist = Newlist.Where(x => x.ApplicationStatusCode == search.ApplicationStatusCode).ToList();
                    //}
                }
                if (search.ApplicationStatusCode.IsNotNullAndNotEmpty())
                {
                    Newlist = Newlist.Where(x => x.ApplicationStatusCode == search.ApplicationStatusCode).ToList();
                }
                if (search.JobAdvertisementId.IsNotNullAndNotEmpty())
                {
                    //Newlist.AddRange(allList.Where(x => x.JobAdvertisementId == search.JobAdvertisementId).ToList());
                    Newlist = Newlist.Where(x => x.JobId == search.JobAdvertisementId).ToList();
                }
                if (search.BatchId.IsNotNullAndNotEmpty())
                {
                    //Newlist.AddRange(allList.Where(x => x.BatchId == search.BatchId).ToList());
                    Newlist = Newlist.Where(x => x.BatchId == search.BatchId).ToList();
                }
                else
                {
                    Newlist = new List<ApplicationViewModel>();
                }


                if (search.BatchStatusCode.IsNotNullAndNotEmpty())
                {
                    //Newlist.AddRange(allList.Where(x => x.BatchStatusCode == search.BatchStatusCode).ToList());
                    Newlist = Newlist.Where(x => x.BatchStatusCode == search.BatchStatusCode).ToList();
                }
                //if (search.ApplicationStatusCode.IsNotNullAndNotEmpty())
                //{
                //    Newlist = Newlist.Where(x => x.ApplicationStatusCode == search.ApplicationStatusCode).ToList();
                //}
                return Newlist;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IList<ApplicationViewModel>> GetViewApplicationDetails(string jobId, string orgId)
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
app.""ApplicationNo"" as ApplicationNo
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
join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState""
#WHERE# ";

                var where = $@" ";
                if (jobId.IsNotNullAndNotEmpty() && orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" where app.""JobId""='{jobId}' AND app.""OrganizationId""='{orgId}' ";
                }
                else if (orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" where app.""OrganizationId""='{orgId}' ";
                }
                else if (jobId.IsNotNullAndNotEmpty())
                {
                    where = @$" where app.""JobId""='{jobId}' ";
                }
                query = query.Replace("#WHERE#", where);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                return allList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        
        public async Task<IList<ApplicationViewModel>> GetApplicationPendingTask(string userId)
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
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, batch.""Name"" as BatchName,app.""OrganizationId"" as OrganizationId, org.""Name"" as OrganizationName, 
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,job.""Name"" as JobName,app.""GaecNo"" as GaecNo,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score, app.""IsLocalCandidate"" as IsLocalCandidate,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,
c.""UserId"" as ApplicationUserId,
app.""ApplicationState"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo,
task.""Id"" as TaskId,task.""TaskNo"" as TaskNo,task.""Subject"" as TaskSubject
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
left join public.""RecTask"" as service ON service.""ReferenceTypeId""=app.""Id"" AND service.""NtsType""=2
left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = service.""Id"" AND (task.""TaskStatusCode""='INPROGRESS' OR task.""TaskStatusCode""='OVERDUE')
left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
#WHERE# ";

                var where = $@" where au.""Id""='{userId}' ";
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

#WHERE# ";

                var where = $@" where apps.""Code""='WorkerPool' AND  appStatus.""Code""<>'UnderApproval' ";
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
        public async Task<IList<ApplicationViewModel>> GetTotalApplication(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode)
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
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score, app.""IsLocalCandidate"" as IsLocalCandidate,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,

task.""Id"" as TaskId,task.""TemplateCode"" as TaskTemplateCode,task.""TaskStatusCode"" as TaskStatusCode,
taskn.""Id"" as NextTaskId,taskn.""TemplateCode"" as NextTaskTemplateCode,taskn.""TaskStatusCode"" as NextTaskStatusCode,
taskv.""Id"" as VisaTaskId,taskv.""TemplateCode"" as VisaTaskTemplateCode,taskv.""TaskStatusCode"" as VisaTaskStatusCode,
c.""UserId"" as ApplicationUserId,
app.""ApplicationState"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo
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

left join public.""RecTask"" as task on task.""ReferenceTypeId""=app.""Id"" AND task.""ReferenceTypeCode""='32' AND task.""TemplateCode""='{tempcode}'
left join public.""RecTask"" as taskn on taskn.""ReferenceTypeId""=app.""Id"" AND taskn.""ReferenceTypeCode""='32' AND taskn.""TemplateCode""='{nexttempcode}'
left join public.""RecTask"" as taskv on taskv.""ReferenceTypeId""=app.""Id"" AND taskv.""ReferenceTypeCode""='32' AND taskv.""TemplateCode""='{visatempcode}'
left join rec.""CandidateProfile"" as c ON c.""Id""=app.""CandidateProfileId"" 

left join rec.""ListOfValue"" as lov on lov.""Id""=app.""Gender""
left join rec.""ListOfValue"" as vt on vt.""Id""=app.""VisaCategory""
LEFT Join rec.""Batch"" as batch on batch.""Id""=app.""BatchId""
left JOIN rec.""ListOfValue"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" 

left join cms.""Organization"" as org ON org.""Id""=batch.""OrganizationId"" 
join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState""
where app.""JobId""='{jobid}' ";
                var where = $@" where app.""JobId""='{jobid}' ";
                //if (orgId.IsNotNullAndNotEmpty())
                //{
                //    where = @$" where app.""JobId""='{jobid}' AND batch.""OrganizationId""='{orgId}' ";
                //}
                //query = query.Replace("#WHERE#", where);
                //var applicationState = "";
                //if (jobadvtstate.IsNotNullAndNotEmpty())
                //{

                //    applicationState = @"join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState"" and apps.""Code"" =  '" + jobadvtstate + "' ";

                //}
                //else
                //{
                //    applicationState = @"join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState""";

                //}
                //query = query.Replace("#APPLICATIONSTATE#", applicationState);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);

                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<IList<ApplicationViewModel>> GetJobAdvertismentState(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode, string status)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,app.""GaecNo"" as GaecNo,app.""JoiningDate"" as JoiningDate,
app.""LastName"" as LastName, app.""Age"" as Age,
app.""QatarId"" as QatarId,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
lov.""Name"" as Gender, app.""MaritalStatus"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Name"" as PassportIssueCountry,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, batch.""Name"" as BatchName, org.""Name"" as OrganizationName, 
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score, app.""IsLocalCandidate"" as IsLocalCandidate,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,

task.""Id"" as TaskId,task.""TemplateCode"" as TaskTemplateCode,task.""TaskStatusCode"" as TaskStatusCode,
taskn.""Id"" as NextTaskId,taskn.""TemplateCode"" as NextTaskTemplateCode,taskn.""TaskStatusCode"" as NextTaskStatusCode,
taskv.""Id"" as VisaTaskId,taskv.""TemplateCode"" as VisaTaskTemplateCode,taskv.""TaskStatusCode"" as VisaTaskStatusCode,
c.""UserId"" as ApplicationUserId,
app.""ApplicationState"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
app.""ApplicationNo"" as ApplicationNo,batch.""Name"" as BatchName
FROM rec.""Application"" as app
#APPLICATIONSTATE#
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join cms.""Currency"" as curr on curr.""Id"" = app.""NetSalaryCurrency""

left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""

left join public.""RecTask"" as task on task.""ReferenceTypeId""=app.""Id"" AND task.""ReferenceTypeCode""='32' AND task.""TemplateCode""='{tempcode}'
left join public.""RecTask"" as taskn on taskn.""ReferenceTypeId""=app.""Id"" AND taskn.""ReferenceTypeCode""='32' AND taskn.""TemplateCode""='{nexttempcode}'
left join public.""RecTask"" as taskv on taskv.""ReferenceTypeId""=app.""Id"" AND taskv.""ReferenceTypeCode""='32' AND taskv.""TemplateCode""='{visatempcode}'
left join rec.""CandidateProfile"" as c ON c.""Id""=app.""CandidateProfileId"" 

left join rec.""ListOfValue"" as lov on lov.""Id""=app.""Gender""
left join rec.""ListOfValue"" as vt on vt.""Id""=app.""VisaCategory""
LEFT Join rec.""Batch"" as batch on batch.""Id""=app.""BatchId""
INNER JOIN rec.""ListOfValue"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" OR lovb.""Code"" <> 'Draft'
left join cms.""Organization"" as org ON org.""Id""=batch.""OrganizationId"" 
#WHERE# ";
                //var where = $@" where batch.""JobId""='{jobid}' ";
                var where = $@" where app.""JobId""='{jobid}' ";
                if (orgId.IsNotNullAndNotEmpty())
                {
                    //where = @$" where batch.""JobId""='{jobid}' AND batch.""OrganizationId""='{orgId}' ";
                    //where = @$" where app.""JobId""='{jobid}' AND app.""OrganizationId""='{orgId}' ";
                    where = @$" where app.""JobId""='{jobid}' AND batch.""OrganizationId""='{orgId}' ";
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
                        applicationState = @"join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState"" and (apps.""Code"" = 'PostWorkerJoined' or apps.""Code"" = 'PostStaffJoined' or apps.""Code"" =  '" + jobadvtstate + "') ";
                    }
                    else
                    {
                        applicationState = @"join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState"" and apps.""Code"" =  '" + jobadvtstate + "' ";
                    }
                }
                else
                {
                    applicationState = @"join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState""";

                }
                query = query.Replace("#APPLICATIONSTATE#", applicationState);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);

                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<IList<ApplicationViewModel>> GetJobAdvertismentApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode, string templateCodeOther)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,app.""Email"" as Email,app.""ApplicationNo"" as ApplicationNo,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
lov.""Name"" as Gender, app.""MaritalStatus"" as MaritalStatus,
 app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score,
app.""ApplicationState"" as ApplicationState, appStatus.""Name"" as ApplicationStatus
FROM rec.""Application"" as app
#APPLICATIONSTATE#
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""

left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""CandidateProfile"" as c ON c.""Id""=app.""CandidateProfileId"" 
left join rec.""ListOfValue"" as lov on lov.""Id""=app.""Gender""
LEFT Join rec.""Batch"" as batch on batch.""Id""=app.""BatchId""
INNER JOIN rec.""ListOfValue"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" AND lovb.""Code"" <> 'Draft'
left join cms.""Organization"" as org ON org.""Id""=batch.""OrganizationId"" 
#WHERE# ";
                var where = $@" where batch.""JobId""='{jobadvtid}' ";
                if (orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" where batch.""JobId""='{jobadvtid}' AND batch.""OrganizationId""='{orgId}' ";
                }
                query = query.Replace("#WHERE#", where);
                var applicationState = "";
                if (jobadvtstate.IsNotNullAndNotEmpty())
                {


                    applicationState = @"join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState"" and apps.""Code"" =  '" + jobadvtstate + "' ";
                }

                query = query.Replace("#APPLICATIONSTATE#", applicationState);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                if (allList.Count() > 0)
                {

                    var allIds = string.Join(",", allList.Select(x => x.ApplicationId));
                    allIds = allIds.Replace(",", "','");

                    var taskquery = @$"SELECT task.""Id"" as Id,s.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName FROM public.""RecTask"" as s
                                   
                        left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where s.""TemplateCode"" ='{templateCode}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and s.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";



                    var otherTaskList = new List<RecTaskViewModel>();
                    if (templateCodeOther.IsNotNullAndNotEmpty())
                    {
                        var taskquery1 = @$"SELECT task.""Id"" as Id,s.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName FROM public.""RecTask"" as s
                        left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where s.""VersionNo""=cast(task.""ParentVersionNo"" as int) and s.""TemplateCode"" ='{templateCodeOther}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and s.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                        otherTaskList = await _queryTask.ExecuteQueryList<RecTaskViewModel>(taskquery1, null);
                    }

                    var tasklist = await _queryTask.ExecuteQueryList<RecTaskViewModel>(taskquery, null);


                    foreach (var item in allList)
                    {
                        var i = 1;
                        var tasks = tasklist.Where(x => x.ReferenceTypeId == item.ApplicationId);
                        foreach (var item1 in tasks)
                        {
                            var Col1 = string.Concat("Step", i);
                            ApplicationExtension.SetPropertyValue(item, Col1, item1.Id);

                            var Col2 = string.Concat("StepNo", i);
                            ApplicationExtension.SetPropertyValue(item, Col2, item1.TaskStatusName + "-" + item1.AssigneeUserName);

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



        public async Task<IList<ApplicationViewModel>> GetDirictHiringData()
        {
            try
            {
                var query = @$"Select rt.""Id"" as ServiceId,rt.""TaskNo"",'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,app.""Email"" as Email,app.""ApplicationNo"" as ApplicationNo,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,batch.""Name"" as BatchName,org.""Name"" as OrganizationName,job.""Name"" as JobName,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
lov.""Name"" as Gender, app.""MaritalStatus"" as MaritalStatus,
 app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score,
app.""ApplicationState"" as ApplicationState, appStatus.""Name"" as ApplicationStatus
from public.""RecTask"" as rt
left join rec.""Application"" as app on app.""Id""=rt.""ReferenceTypeId""
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""CandidateProfile"" as c ON c.""Id""=app.""CandidateProfileId"" 
left join rec.""ListOfValue"" as lov on lov.""Id""=app.""Gender""
LEFT Join rec.""Batch"" as batch on batch.""Id""=app.""BatchId""
left JOIN rec.""ListOfValue"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" AND lovb.""Code"" <> 'Draft'
left join cms.""Organization"" as org ON org.""Id""=batch.""OrganizationId"" 
where rt.""TemplateCode""='DIRECT_HIRING' ";
               
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                if (allList.Count() > 0)
                {

                    var allIds = string.Join(",", allList.Select(x => x.ServiceId));
                    allIds = allIds.Replace(",", "','");

                    var taskquery = @$"SELECT task.""Id"" as Id,task.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName FROM public.""RecTask"" as s
                                   
                        left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where s.""TemplateCode"" ='DIRECT_HIRING' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and task.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";



                    //var otherTaskList = new List<RecTaskViewModel>();
                    //if (templateCodeOther.IsNotNullAndNotEmpty())
                    //{
                    //    var taskquery1 = @$"SELECT task.""Id"" as Id,s.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo,  au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName FROM public.""RecTask"" as s
                    //    left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                    //    left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                    //    where s.""VersionNo""=cast(task.""ParentVersionNo"" as int) and s.""TemplateCode"" ='{templateCodeOther}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE' or s.""TaskStatusCode""='COMPLETED') and s.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                    //    otherTaskList = await _queryTask.ExecuteQueryList<RecTaskViewModel>(taskquery1, null);
                    //}

                    var tasklist = await _queryTask.ExecuteQueryList<RecTaskViewModel>(taskquery, null);


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
                        //if (templateCodeOther.IsNotNullAndNotEmpty())
                        //{
                        //    var othertasks = otherTaskList.Where(x => x.ReferenceTypeId == item.ApplicationId);
                        //    foreach (var item1 in othertasks)
                        //    {
                        //        var Col1 = string.Concat("Step", i);
                        //        ApplicationExtension.SetPropertyValue(item, Col1, item1.Id);

                        //        var Col2 = string.Concat("StepNo", i);
                        //        ApplicationExtension.SetPropertyValue(item, Col2, item1.TaskStatusName + "-" + item1.AssigneeUserName);

                        //        i++;
                        //    }
                        //}
                    }

                }
                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }

        }



        public async Task<IList<ApplicationViewModel>> GetWorkerPoolApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode)
        {
            try
            {
                var query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName,
app.""LastName"" as LastName, app.""Age"" as Age,app.""Email"" as Email,app.""ApplicationNo"" as ApplicationNo,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
lov.""Name"" as Gender, app.""MaritalStatus"" as MaritalStatus,
 app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
app.""Score"" as Score,
app.""ApplicationState"" as ApplicationState, appStatus.""Name"" as ApplicationStatus, app.""WorkerBatchId"" as WorkerBatchId,
bat.""Name"" as WorkerBatch
FROM rec.""Application"" as app
#APPLICATIONSTATE#
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join rec.""Batch"" as bat on bat.""Id"" = app.""WorkerBatchId""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""CandidateProfile"" as c ON c.""Id""=app.""CandidateProfileId"" 
left join rec.""ListOfValue"" as lov on lov.""Id""=app.""Gender""
LEFT Join rec.""Batch"" as batch on batch.""Id""=app.""BatchId""
INNER JOIN rec.""ListOfValue"" as lovb ON lovb.""Id"" = batch.""BatchStatus"" AND lovb.""Code"" <> 'Draft'
left join cms.""Organization"" as org ON org.""Id""=batch.""OrganizationId"" 
#WHERE# ";
                var where = $@" where batch.""JobId""='{jobadvtid}' ";
                if (orgId.IsNotNullAndNotEmpty())
                {
                    where = @$" where batch.""JobId""='{jobadvtid}' AND batch.""OrganizationId""='{orgId}' ";
                }
                query = query.Replace("#WHERE#", where);
                var applicationState = "";
                if (jobadvtstate.IsNotNullAndNotEmpty())
                {


                    applicationState = @"join rec.""ApplicationState"" as apps on apps.""Id""=app.""ApplicationState"" and apps.""Code"" =  '" + jobadvtstate + "' ";
                }

                query = query.Replace("#APPLICATIONSTATE#", applicationState);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                if (allList.Count() > 0)
                {

                    var allIds = string.Join(",", allList.Select(x => x.WorkerBatchId));
                    allIds = allIds.Replace(",", "','");

                    var taskquery = @$"SELECT task.""Id"" as Id,s.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo, au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName FROM public.""RecTask"" as s
                        left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where s.""TemplateCode"" ='{templateCode}' and s.""NtsType"" = 2 and (s.""TaskStatusCode""='INPROGRESS' or s.""TaskStatusCode""='OVERDUE') and s.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc";


                    var tasklist = await _queryTask.ExecuteQueryList<RecTaskViewModel>(taskquery, null);

                    var otherTaskList = new List<RecTaskViewModel>();
                    var taskquery1 = @$"SELECT task.""Id"" as Id,task.""ReferenceTypeId"" as ReferenceTypeId,task.""TaskNo"" as TaskNo, au.""Name"" as AssigneeUserName, task.""TaskStatusName"" as TaskStatusName FROM public.""RecTask"" as task
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where task.""TemplateCode"" ='WORKER_SALARY_AGENCY' and (task.""TaskStatusCode""='INPROGRESS' or task.""TaskStatusCode""='OVERDUE') and task.""ReferenceTypeId"" in ('{allIds}') order by task.""CreatedDate"" asc ";

                    otherTaskList = await _queryTask.ExecuteQueryList<RecTaskViewModel>(taskquery1, null);

                    foreach (var item in allList)
                    {
                        var i = 1;
                        var tasks = tasklist.Where(x => x.ReferenceTypeId == item.WorkerBatchId);
                        foreach (var item1 in tasks)
                        {
                            var Col1 = string.Concat("Step", i);
                            ApplicationExtension.SetPropertyValue(item, Col1, item1.Id);

                            var Col2 = string.Concat("StepNo", i);
                            ApplicationExtension.SetPropertyValue(item, Col2, item1.TaskStatusName + "-" + item1.AssigneeUserName);

                            i++;
                        }

                        foreach (var item1 in otherTaskList)
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

        public async Task<IList<ApplicationViewModel>> GetWorkPool1Data(string workerbatchid)
        {
            var query = $@"select app.""Id"",concat(app.""FirstName"",' ',app.""MiddleName"" ,' ',app.""LastName"") as FullName,app.""SalaryOnAppointment"",app.""Age"",
                jb.""Name"" as PositionName,appexp.""NoOfYear"" as AbroadExperiance,app.""HRHeadApproval"" as HRApprovl,app.""HRHeadComment"" as HRHeadComment,
                app.""HodApproval"" as HodApprovl,app.""HodComment"" as HodComment,app.""PlanningApproval"" as PlanningApprovl,app.""PlanningComment"" as PlanningComment,
                app.""EDApproval"" as EDApprovl,app.""EDComment"" as EDComment,app.""CandidateProfileId"" as CandidateProfileId,app.""PassportNumber"" as PassportNumber,
                app.""TotalWorkExperience"" as TotalWorkExperience,appcoun.""NoOfYear"" as IndiaExperiance
                from rec.""Application"" as app
                left join cms.""Job"" as jb on jb.""Id"" = app.""JobId""
               
                left join rec.""ApplicationExperienceByOther"" as appexp on appexp.""ApplicationId""=app.""Id"" 
                left join rec.""ApplicationExperienceByCountry"" as appcoun on appcoun.""ApplicationId""=app.""Id"" 
                left join cms.""Country"" as coun on coun.""Id"" = appcoun.""CountryId"" and coun.""Code""='India'
                left join rec.""ListOfValue"" as lov on lov.""Id"" = appexp.""OtherTypeId"" and  lov.""Code"" = 'Abroad'
                where app.""WorkerBatchId""='{workerbatchid}' ";
            var allList = await _appqueryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);


            return allList;
        }

        public async Task<CandidateProfileViewModel> GetDocumentsByApplication(string applicationId)
        {
            string query = @$"select cp.*, pp.""FileName"" as PassportAttachmentName, ac.""FileName"" as AcademicCertificateName, oc.""FileName"" as OtherCertificateName, 
                                cv.""FileName"" as ResumeAttachmentName, cl.""FileName"" as CoverLetterAttachmentName From rec.""Application"" as cp
                                Left Join public.""File"" as pp on pp.""Id"" = cp.""PassportAttachmentId""
                                Left Join public.""File"" as ac on ac.""Id"" = cp.""AcademicCertificateId""
                                Left Join public.""File"" as oc on oc.""Id"" = cp.""OtherCertificateId""
                                Left Join public.""File"" as cv on cv.""Id"" = cp.""ResumeId""
                                Left Join public.""File"" as cl on cl.""Id"" = cp.""CoverLetterId""
                                where cp.""Id"" = '{applicationId}'";

            var querydocData = await _queryRepo1.ExecuteQuerySingle(query, null);
            return querydocData;
        }
        public async Task<ApplicationViewModel> GetApplicationDetail(string applicationId)
        {
            //string query = @$"select cp.*, vt.""Code"" as VisaCategoryCode,mplov.""Code"" as ManpowerTypeCode
            //                    From rec.""Application"" as cp
            //                    left join rec.""ListOfValue"" as vt on vt.""Id""=cp.""VisaCategory""
            //                    left join rec.""JobAdvertisement"" as mjd on mjd.""JobId"" = cp.""JobId"" and mjd.""Status""=1
            //                    Left join rec.""ListOfValue"" as mplov ON mplov.""Id"" = mjd.""ManpowerTypeId""
            //                    where cp.""Id"" = '{applicationId}'";

            string query = @$"select cp.*, vt.""Code"" as VisaCategoryCode,mplov.""Code"" as ManpowerTypeCode,mplov.""Name"" as ManpowerTypeName
                                From rec.""Application"" as cp
                                left join rec.""ListOfValue"" as vt on vt.""Id""=cp.""VisaCategory""
                                left join rec.""JobAdvertisement"" as mjd on mjd.""JobId"" = cp.""JobId"" and mjd.""Status""=1
                                left join cms.""Job"" as job on job.""Id"" = cp.""JobId"" 
                                Left join rec.""ListOfValue"" as mplov ON mplov.""ListOfValueType""='LOV_MANPOWERTYPE' and mplov.""Code"" = job.""ManpowerTypeCode""


                                where cp.""Id"" = '{applicationId}'";


            var querydocData = await _appqueryRepo.ExecuteQuerySingle(query, null);
            return querydocData;
        }

        public async Task UpdateJobAdvtApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state, bool autoCommit = true)
        {
            if (type == "JobApplication")
            {
                var application = await GetSingleById(applicationId);
                var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == state);
                var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == status);
                application.ApplicationState = state1.Id;
                application.ApplicationStatus = status1.Id;
                var result1 = await base.Edit<ApplicationViewModel, Application>(application);
                if (result1.IsSuccess)
                {
                    var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                    ApplicationStateTrack.ApplicationId = result1.Item.Id;
                    ApplicationStateTrack.ApplicationStateId = state1.Id;
                    ApplicationStateTrack.ApplicationStatusId = status1.Id;
                    ApplicationStateTrack.ChangedBy = _userContext.UserId;
                    ApplicationStateTrack.ChangedDate = DateTime.Now;
                    var result2 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                }

                //string query = @$"update rec.""ApplicationStateTrack"" set ""ApplicationId""='{applicationId}', ""ApplicationStateId""='{state1.Id}', ""ApplicationStatusId""='{status1.Id}',
                // ""ChangedBy""='{_userContext.UserId}' ";
                //var result = await _appqueryRepo.ExecuteScalar<bool?>(query, null);
            }
            else if (type == "CandidateProfile")
            {
                var application = await _candidateProfileBusiness.GetSingleById(CandidateProfileId);
                var data = _autoMapper.Map<CandidateProfileViewModel, ApplicationViewModel>(application);
                var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == state);
                var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == status);
                data.ApplicationState = state1.Id;
                data.ApplicationStatus = status1.Id;
                data.CandidateProfileId = application.Id;
                data.Id = "";
                var result = await base.Create(data, autoCommit);

                data.Id = result.Item.Id;
                if (result.IsSuccess)
                {
                    var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                    ApplicationStateTrack.ApplicationId = data.Id;
                    ApplicationStateTrack.ApplicationStateId = state1.Id;
                    ApplicationStateTrack.ApplicationStatusId = status1.Id;
                    ApplicationStateTrack.ChangedBy = _userContext.UserId;
                    ApplicationStateTrack.ChangedDate = DateTime.Now;
                    //   string query = @$"update rec.""ApplicationStateTrack"" set ""ApplicationId""='{data.Id}', ""ApplicationStateId""='{state1.Id}', ""ApplicationStatusId""='{status1.Id}',
                    //""ChangedBy""='{_userContext.UserId}' ";
                    // var result1 = await _appqueryRepo.ExecuteScalar<bool?>(query, null);
                    var result1 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                }

            }

        }
        public async Task<CommandResult<ApplicationViewModel>> UpdateApplication(CandidateProfileViewModel model, bool autoCommit = true)
        {
            double sum = 0;
            foreach (var item in model.Criterias)
            {
                var criteria = await _repo.GetSingleGlobal<IdNameViewModel, ListOfValue>(x => x.Id == item.Value);
                if (criteria.IsNotNull() && criteria.Code.IsNotNullAndNotEmpty() && item.Weightage.HasValue && item.Weightage.IsNotNull())
                {
                    sum += (item.Weightage.Value * Convert.ToInt64(criteria.Code)) / 5;
                }
            }
            foreach (var item in model.Skills)
            {
                var skill = await _repo.GetSingleGlobal<IdNameViewModel, ListOfValue>(x => x.Id == item.Value);
                if (skill.IsNotNull() && skill.Code.IsNotNullAndNotEmpty() && item.Weightage.HasValue && item.Weightage.IsNotNull())
                {
                    sum += (item.Weightage.Value * Convert.ToInt64(skill.Code)) / 5;
                }
            }

            var application = await _candidateProfileBusiness.GetSingleById(model.Id);
            var data = _autoMapper.Map<CandidateProfileViewModel, ApplicationViewModel>(application);
            var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == "UnReviewed");//unreviewed
            //var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == "");//
            data.ApplicationState = state1.IsNotNull() ? state1.Id : "";
            //data.ApplicationStatus = status1.IsNotNull() ? status1.Id : "";
            data.CandidateProfileId = application.Id;
            data.Id = "";
            //data.BatchId = "";
            data.JobAdvertisementId = model.JobAdvertisementId;
            var jobAdv = await _jobAdvBusiness.GetSingleById(model.JobAdvertisementId);
            data.JobId = jobAdv.JobId;
            //data.OrganizationId = jobAdv.OrganizationId;
            data.Score = sum;
            data.SignatureDate = model.SignatureDate.IsNotNull() ? model.SignatureDate : System.DateTime.Now;
            data.AppliedDate = model.SignatureDate.IsNotNull() ? model.SignatureDate : System.DateTime.Now;
            data.Signature = model.Signature;
            data.ApplicationNo = await GenerateNextApplicationNo();
            var result = await base.Create(data, autoCommit);
            data.Id = result.Item.Id;
            if (result.IsSuccess)
            {
                // Copy Candidate criteria  
                if (model.Criterias.IsNotNull() && model.Criterias.Count > 0)
                {
                    foreach (var criteria in model.Criterias)
                    {
                        criteria.ApplicationId = result.Item.Id;
                        criteria.Id = "";
                        var criteriaResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(criteria);
                    }
                }
                // Copy Candidate skill
                if (model.Skills.IsNotNull() && model.Skills.Count > 0)
                {
                    foreach (var skill in model.Skills)
                    {
                        skill.ApplicationId = result.Item.Id;
                        skill.Id = "";
                        var skillResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(skill);
                    }
                }
                // Copy Candidate otherInfo
                if (model.OtherInformations.IsNotNull() && model.OtherInformations.Count > 0)
                {
                    foreach (var otherInformation in model.OtherInformations)
                    {
                        otherInformation.ApplicationId = result.Item.Id;
                        otherInformation.Id = "";
                        var otherInformationResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(otherInformation);
                    }
                }
                // Copy Candidate Education                   
                var candidateEdu = await _repo.GetList<CandidateEducationalViewModel, CandidateEducational>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateEdu != null && candidateEdu.Count() > 0)
                {
                    foreach (var education in candidateEdu)
                    {
                        var appEducation = _autoMapper.Map<CandidateEducationalViewModel, ApplicationEducationalViewModel>(education);
                        appEducation.ApplicationId = result.Item.Id;
                        appEducation.Id = "";
                        var eduresult = await base.Create<ApplicationEducationalViewModel, ApplicationEducational>(appEducation);
                    }
                }
                // Copy Candidate Experience
                var candidateExp = await _repo.GetList<CandidateExperienceViewModel, CandidateExperience>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExp != null && candidateExp.Count() > 0)
                {
                    foreach (var exp in candidateExp)
                    {
                        var appexp = _autoMapper.Map<CandidateExperienceViewModel, ApplicationExperienceViewModel>(exp);
                        appexp.ApplicationId = result.Item.Id;
                        appexp.Id = "";
                        var expresult = await base.Create<ApplicationExperienceViewModel, ApplicationExperience>(appexp);
                    }
                }
                // Copy Candidate Computer Proficiency
                var candidateCompProficiency = await _repo.GetList<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateCompProficiency != null && candidateCompProficiency.Count() > 0)
                {
                    foreach (var Compexp in candidateCompProficiency)
                    {
                        var Compexperience = _autoMapper.Map<CandidateComputerProficiencyViewModel, ApplicationComputerProficiencyViewModel>(Compexp);
                        Compexperience.ApplicationId = result.Item.Id;
                        Compexperience.Id = "";
                        var Compexpresult = await base.Create<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>(Compexperience);
                    }
                }
                // Copy Candidate Language Proficiency
                var candidateLanguageProficiency = await _repo.GetList<CandidateLanguageProficiencyViewModel, CandidateLanguageProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateLanguageProficiency != null && candidateLanguageProficiency.Count() > 0)
                {
                    foreach (var language in candidateLanguageProficiency)
                    {
                        var lang = _autoMapper.Map<CandidateLanguageProficiencyViewModel, ApplicationLanguageProficiencyViewModel>(language);
                        lang.ApplicationId = result.Item.Id;
                        lang.Id = "";
                        var langresult = await base.Create<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>(lang);
                    }
                }
                // Copy Candidate Experience By Country
                var candidateexpByCountry = await _repo.GetList<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateexpByCountry != null && candidateexpByCountry.Count() > 0)
                {
                    foreach (var expByountry in candidateexpByCountry)
                    {
                        var countryexp = _autoMapper.Map<CandidateExperienceByCountryViewModel, ApplicationExperienceByCountryViewModel>(expByountry);
                        countryexp.ApplicationId = result.Item.Id;
                        countryexp.Id = "";
                        var countryexpresult = await base.Create<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>(countryexp);
                    }
                }
                // Copy Candidate Experience By Sector
                var candidateexpBySector = await _repo.GetList<CandidateExperienceBySectorViewModel, CandidateExperienceBySector>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateexpBySector != null && candidateexpBySector.Count() > 0)
                {
                    foreach (var expBySector in candidateexpBySector)
                    {
                        var Sectorexp = _autoMapper.Map<CandidateExperienceBySectorViewModel, ApplicationExperienceBySectorViewModel>(expBySector);
                        Sectorexp.ApplicationId = result.Item.Id;
                        Sectorexp.Id = "";
                        var Sectorexpresult = await base.Create<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>(Sectorexp);
                    }
                }
                // Copy Candidate Experience By Nature
                var candidateExpByNature = await _repo.GetList<CandidateExperienceByNatureViewModel, CandidateExperienceByNature>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExpByNature != null && candidateExpByNature.Count() > 0)
                {
                    foreach (var expByNature in candidateExpByNature)
                    {
                        var Natureexp = _autoMapper.Map<CandidateExperienceByNatureViewModel, ApplicationeExperienceByNatureViewModel>(expByNature);
                        Natureexp.ApplicationId = result.Item.Id;
                        Natureexp.Id = "";
                        var Natureexpresult = await base.Create<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>(Natureexp);
                    }
                }
                // Copy Candidate Experience By Job
                var candidateExpByJob = await _repo.GetList<CandidateExperienceByJobViewModel, CandidateExperienceByJob>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExpByJob != null && candidateExpByJob.Count() > 0)
                {
                    foreach (var expByJob in candidateExpByJob)
                    {
                        var Jobexp = _autoMapper.Map<CandidateExperienceByJobViewModel, ApplicationExperienceByJobViewModel>(expByJob);
                        Jobexp.ApplicationId = result.Item.Id;
                        Jobexp.Id = "";
                        var Natureexpresult = await base.Create<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>(Jobexp);
                    }
                }
                // Copy Candidate Experience By OtherType
                var candidateExpByOther = await _repo.GetList<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExpByOther != null && candidateExpByOther.Count() > 0)
                {
                    foreach (var expByOther in candidateExpByOther)
                    {
                        var Otherexp = _autoMapper.Map<CandidateExperienceByOtherViewModel, ApplicationExperienceByOtherViewModel>(expByOther);
                        Otherexp.ApplicationId = result.Item.Id;
                        Otherexp.Id = "";
                        var Otherexpresult = await base.Create<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>(Otherexp);
                    }
                }
                // Copy Candidate Driving Liciense Detail
                var candidateDL = await _repo.GetList<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateDL != null && candidateDL.Count() > 0)
                {
                    foreach (var dl in candidateDL)
                    {
                        var DL = _autoMapper.Map<CandidateDrivingLicenseViewModel, ApplicationDrivingLicenseViewModel>(dl);
                        DL.ApplicationId = result.Item.Id;
                        DL.Id = "";
                        var Natureexpresult = await base.Create<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>(DL);
                    }
                }
                // Copy Candidate Project
                var candidateProject = await _repo.GetList<CandidateProjectViewModel, CandidateProject>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateProject != null && candidateProject.Count() > 0)
                {
                    foreach (var project in candidateProject)
                    {
                        var candidateProj = _autoMapper.Map<CandidateProjectViewModel, ApplicationProjectViewModel>(project);
                        candidateProj.ApplicationId = result.Item.Id;
                        candidateProj.Id = "";
                        var Natureexpresult = await base.Create<ApplicationProjectViewModel, ApplicationProject>(candidateProj);
                    }
                }
                // Copy Candidate References
                var candidateReferences = await _repo.GetList<CandidateReferencesViewModel, CandidateReferences>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateReferences != null && candidateReferences.Count() > 0)
                {
                    foreach (var reference in candidateReferences)
                    {
                        var candidateref = _autoMapper.Map<CandidateReferencesViewModel, ApplicationReferencesViewModel>(reference);
                        candidateref.ApplicationId = result.Item.Id;
                        candidateref.Id = "";
                        var Natureexpresult = await base.Create<ApplicationReferencesViewModel, ApplicationReferences>(candidateref);
                    }
                }
                var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                ApplicationStateTrack.ApplicationId = data.Id;
                ApplicationStateTrack.ApplicationStateId = state1.Id;
                //ApplicationStateTrack.ApplicationStatusId = status1.Id;
                ApplicationStateTrack.ChangedBy = _userContext.UserId;
                ApplicationStateTrack.ChangedDate = DateTime.Now;
                var result1 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                return CommandResult<ApplicationViewModel>.Instance(data);
            }
            return CommandResult<ApplicationViewModel>.Instance(new ApplicationViewModel(), false, "Application failed");
        }

        public async Task<CommandResult<ApplicationViewModel>> CreateApplication(CandidateProfileViewModel model,BatchViewModel batch, bool autoCommit = true)
        {
            double sum = 0;
           
            var application = await _candidateProfileBusiness.GetSingleById(model.Id);
            var data = _autoMapper.Map<CandidateProfileViewModel, ApplicationViewModel>(application);
            var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == "DirectHiring");//unreviewed
            //var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == "");//
            data.ApplicationState = state1.IsNotNull() ? state1.Id : "";
            //data.ApplicationStatus = status1.IsNotNull() ? status1.Id : "";
            data.CandidateProfileId = application.Id;
            data.Id = "";
            //data.BatchId = "";
            //data.JobAdvertisementId = model.JobAdvertisementId;
            //var jobAdv = await _jobAdvBusiness.GetSingleById(model.JobAdvertisementId);
            data.JobId = batch.JobId;
            data.OrganizationId = batch.OrganizationId;
            data.BatchId = batch.Id;
            
            data.Score = sum;
            data.SignatureDate = model.SignatureDate.IsNotNull() ? model.SignatureDate : System.DateTime.Now;
            data.AppliedDate = model.SignatureDate.IsNotNull() ? model.SignatureDate : System.DateTime.Now;
            
            data.ApplicationNo = await GenerateNextApplicationNo();
            var result = await base.Create(data, autoCommit);
            data.Id = result.Item.Id;
            if (result.IsSuccess)
            {
               
                // Copy Candidate Education                   
                var candidateEdu = await _repo.GetList<CandidateEducationalViewModel, CandidateEducational>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateEdu != null && candidateEdu.Count() > 0)
                {
                    foreach (var education in candidateEdu)
                    {
                        var appEducation = _autoMapper.Map<CandidateEducationalViewModel, ApplicationEducationalViewModel>(education);
                        appEducation.ApplicationId = result.Item.Id;
                        appEducation.Id = "";
                        var eduresult = await base.Create<ApplicationEducationalViewModel, ApplicationEducational>(appEducation);
                    }
                }
                // Copy Candidate Experience
                var candidateExp = await _repo.GetList<CandidateExperienceViewModel, CandidateExperience>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExp != null && candidateExp.Count() > 0)
                {
                    foreach (var exp in candidateExp)
                    {
                        var appexp = _autoMapper.Map<CandidateExperienceViewModel, ApplicationExperienceViewModel>(exp);
                        appexp.ApplicationId = result.Item.Id;
                        appexp.Id = "";
                        var expresult = await base.Create<ApplicationExperienceViewModel, ApplicationExperience>(appexp);
                    }
                }
                // Copy Candidate Computer Proficiency
                var candidateCompProficiency = await _repo.GetList<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateCompProficiency != null && candidateCompProficiency.Count() > 0)
                {
                    foreach (var Compexp in candidateCompProficiency)
                    {
                        var Compexperience = _autoMapper.Map<CandidateComputerProficiencyViewModel, ApplicationComputerProficiencyViewModel>(Compexp);
                        Compexperience.ApplicationId = result.Item.Id;
                        Compexperience.Id = "";
                        var Compexpresult = await base.Create<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>(Compexperience);
                    }
                }
                // Copy Candidate Language Proficiency
                var candidateLanguageProficiency = await _repo.GetList<CandidateLanguageProficiencyViewModel, CandidateLanguageProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateLanguageProficiency != null && candidateLanguageProficiency.Count() > 0)
                {
                    foreach (var language in candidateLanguageProficiency)
                    {
                        var lang = _autoMapper.Map<CandidateLanguageProficiencyViewModel, ApplicationLanguageProficiencyViewModel>(language);
                        lang.ApplicationId = result.Item.Id;
                        lang.Id = "";
                        var langresult = await base.Create<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>(lang);
                    }
                }
                // Copy Candidate Experience By Country
                var candidateexpByCountry = await _repo.GetList<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateexpByCountry != null && candidateexpByCountry.Count() > 0)
                {
                    foreach (var expByountry in candidateexpByCountry)
                    {
                        var countryexp = _autoMapper.Map<CandidateExperienceByCountryViewModel, ApplicationExperienceByCountryViewModel>(expByountry);
                        countryexp.ApplicationId = result.Item.Id;
                        countryexp.Id = "";
                        var countryexpresult = await base.Create<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>(countryexp);
                    }
                }
                // Copy Candidate Experience By Sector
                var candidateexpBySector = await _repo.GetList<CandidateExperienceBySectorViewModel, CandidateExperienceBySector>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateexpBySector != null && candidateexpBySector.Count() > 0)
                {
                    foreach (var expBySector in candidateexpBySector)
                    {
                        var Sectorexp = _autoMapper.Map<CandidateExperienceBySectorViewModel, ApplicationExperienceBySectorViewModel>(expBySector);
                        Sectorexp.ApplicationId = result.Item.Id;
                        Sectorexp.Id = "";
                        var Sectorexpresult = await base.Create<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>(Sectorexp);
                    }
                }
                // Copy Candidate Experience By Nature
                var candidateExpByNature = await _repo.GetList<CandidateExperienceByNatureViewModel, CandidateExperienceByNature>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExpByNature != null && candidateExpByNature.Count() > 0)
                {
                    foreach (var expByNature in candidateExpByNature)
                    {
                        var Natureexp = _autoMapper.Map<CandidateExperienceByNatureViewModel, ApplicationeExperienceByNatureViewModel>(expByNature);
                        Natureexp.ApplicationId = result.Item.Id;
                        Natureexp.Id = "";
                        var Natureexpresult = await base.Create<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>(Natureexp);
                    }
                }
                // Copy Candidate Experience By Job
                var candidateExpByJob = await _repo.GetList<CandidateExperienceByJobViewModel, CandidateExperienceByJob>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExpByJob != null && candidateExpByJob.Count() > 0)
                {
                    foreach (var expByJob in candidateExpByJob)
                    {
                        var Jobexp = _autoMapper.Map<CandidateExperienceByJobViewModel, ApplicationExperienceByJobViewModel>(expByJob);
                        Jobexp.ApplicationId = result.Item.Id;
                        Jobexp.Id = "";
                        var Natureexpresult = await base.Create<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>(Jobexp);
                    }
                }
                // Copy Candidate Experience By OtherType
                var candidateExpByOther = await _repo.GetList<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateExpByOther != null && candidateExpByOther.Count() > 0)
                {
                    foreach (var expByOther in candidateExpByOther)
                    {
                        var Otherexp = _autoMapper.Map<CandidateExperienceByOtherViewModel, ApplicationExperienceByOtherViewModel>(expByOther);
                        Otherexp.ApplicationId = result.Item.Id;
                        Otherexp.Id = "";
                        var Otherexpresult = await base.Create<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>(Otherexp);
                    }
                }
                // Copy Candidate Driving Liciense Detail
                var candidateDL = await _repo.GetList<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateDL != null && candidateDL.Count() > 0)
                {
                    foreach (var dl in candidateDL)
                    {
                        var DL = _autoMapper.Map<CandidateDrivingLicenseViewModel, ApplicationDrivingLicenseViewModel>(dl);
                        DL.ApplicationId = result.Item.Id;
                        DL.Id = "";
                        var Natureexpresult = await base.Create<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>(DL);
                    }
                }
                // Copy Candidate Project
                var candidateProject = await _repo.GetList<CandidateProjectViewModel, CandidateProject>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateProject != null && candidateProject.Count() > 0)
                {
                    foreach (var project in candidateProject)
                    {
                        var candidateProj = _autoMapper.Map<CandidateProjectViewModel, ApplicationProjectViewModel>(project);
                        candidateProj.ApplicationId = result.Item.Id;
                        candidateProj.Id = "";
                        var Natureexpresult = await base.Create<ApplicationProjectViewModel, ApplicationProject>(candidateProj);
                    }
                }
                // Copy Candidate References
                var candidateReferences = await _repo.GetList<CandidateReferencesViewModel, CandidateReferences>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                if (candidateReferences != null && candidateReferences.Count() > 0)
                {
                    foreach (var reference in candidateReferences)
                    {
                        var candidateref = _autoMapper.Map<CandidateReferencesViewModel, ApplicationReferencesViewModel>(reference);
                        candidateref.ApplicationId = result.Item.Id;
                        candidateref.Id = "";
                        var Natureexpresult = await base.Create<ApplicationReferencesViewModel, ApplicationReferences>(candidateref);
                    }
                }
                var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                ApplicationStateTrack.ApplicationId = data.Id;
                ApplicationStateTrack.ApplicationStateId = state1.Id;
                //ApplicationStateTrack.ApplicationStatusId = status1.Id;
                ApplicationStateTrack.ChangedBy = _userContext.UserId;
                ApplicationStateTrack.ChangedDate = DateTime.Now;
                var result1 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                return CommandResult<ApplicationViewModel>.Instance(data);
            }
            return CommandResult<ApplicationViewModel>.Instance(new ApplicationViewModel(), false, "Application failed");
        }
        public async Task<IdNameViewModel> GetApplicationStateByCode(string code)
        {
            var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == code);
            return state1;
        }
        public async Task<ApplicationViewModel> GetApplicationEvaluationDetails(string applicationId)
        {
            string query = @$"SELECT app.*,
                            CONCAT( app.""FirstName"",' ',app.""MiddleName"" ,' ',app.""LastName"") as FullName,
                                    p.""Name"" as PositionName,d.""Name"" as DivisionName,a.""Name"" as AccommodationName,
                                    st.""Name"" as SelectedThroughName,
                                    u.""Name"" as InterviewByUserName
                                    FROM rec.""Application"" as app
                                    LEFT JOIN cms.""Job"" as p ON p.""Id"" = app.""JobId""
                                    LEFT JOIN rec.""ListOfValue"" as d ON d.""Id"" = app.""DivisionId""
                                    LEFT JOIN rec.""ListOfValue"" as a ON a.""Id"" = app.""AccommodationId""
                                    LEFT JOIN rec.""ListOfValue"" as st ON st.""Id"" = app.""SelectedThroughId""
                                    LEFT JOIN public.""User"" as u ON u.""Id""=app.""InterviewByUserId""
                                    WHERE app.""Id"" = '{applicationId}'";

            var queryData = await _appqueryRepo.ExecuteQuerySingle(query, null);

            return queryData;
        }

        public async Task<ApplicationViewModel> GetApplicationDeclarationData(string applicationId)
        {
            //string query = @$"SELECT app.*,
            //                CONCAT( app.""FirstName"",' ',app.""MiddleName"" ,' ',app.""LastName"") as FullName,
            //                        d.""Name"" as DivisionName                                    
            //                        FROM rec.""Application"" as app                                    
            //                        LEFT JOIN rec.""ListOfValue"" as d ON d.""Id"" = app.""DivisionId""                                 
            //                        WHERE app.""Id"" = '{applicationId}'";

            string query = @$"SELECT app.""Id"",app.""GaecNo"",app.""WitnessName1"",app.""WitnessDesignation1"",
app.""WitnessDate1"",app.""WitnessGAEC1"",app.""WitnessName2"",app.""WitnessDesignation2"",app.""WitnessDate2"",app.""WitnessGAEC2"",
                            CONCAT( app.""FirstName"",' ',app.""MiddleName"" ,' ',app.""LastName"") as FullName,
                                    d.""Name"" as DivisionName, t.""Name"" as TitleName                                   
                                    FROM rec.""Application"" as app                                    
                                    LEFT JOIN rec.""ListOfValue"" as d ON d.""Id"" = app.""DivisionId""
                                    LEFT JOIN rec.""ListOfValue"" as t ON t.""Id"" = app.""TitleId""
                                    WHERE app.""Id"" = '{applicationId}'";



            var queryData = await _appqueryRepo.ExecuteQuerySingle(query, null);

            return queryData;
        }

        public async Task<ApplicationViewModel> GetOfferDetails(string applicationId)
        {
            string query = @$"select a.*, c.""Name"" as PermanentAddressCountryName, g.""Name"" as OfferGrade, CAST(g.""Code"" as INTEGER) as GradeCode, ce.""Value"" as BasicPay, cepa.""Value"" as ProfessionalAllowance,
                                a.""ContractStartDate"" as OfferCreatedDate, a.""JoiningNotLaterThan"" as CandJoiningDate, t.""Name"" as TitleName, od.""Name"" as OfferDesigination 
                                from rec.""Application"" as a
                                left join cms.""Job"" as od on od.""Id"" = a.""JobId""
                                left join cms.""Country"" as c on c.""Id"" = a.""PermanentAddressCountryId""
                                left join cms.""Grade"" as g on g.""Id"" = a.""OfferGrade""
                                left join rec.""ListOfValue"" as t on t.""Id"" = a.""TitleId""                                
                                left join rec.""RecruitmentCandidateElementInfo"" as ce on ce.""ApplicationId"" = a.""Id"" and ce.""ElementId"" = (select ""Id"" from rec.""RecruitmentPayElement"" where ""ElementCode""='BASIC')
                                left join rec.""RecruitmentCandidateElementInfo"" as cepa on cepa.""ApplicationId"" = a.""Id"" and cepa.""ElementId"" = (select ""Id"" from rec.""RecruitmentPayElement"" where ""ElementCode"" = 'PROFESSIONALALLOWANCE')
                                where a.""Id"" = '{applicationId}' ";

            var queryData = await _appqueryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<ApplicationViewModel> GetNameById(string applicationId)
        {
            string query = @$"select concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName, a.""GaecNo"", a.""PassportNumber"", 
            a.""ContractStartDate"" as ContractDate, n.""Name"" as NationalityName, od.""Name"" as OfferDesigination,
            rc.""Value"" as Basic, re.""Value"" as SafetyAllowance, t.""Name"" as ""TitleName""
            from rec.""Application"" as a
            left join cms.""Job"" as od on od.""Id"" = a.""JobId""
            LEFT JOIN cms.""Nationality"" as n ON n.""Id"" = a.""NationalityId""
            LEFT JOIN rec.""ListOfValue"" as t ON t.""Id"" = a.""TitleId"" 
            Left join rec.""RecruitmentCandidateElementInfo"" as rc on rc.""ApplicationId"" = a.""Id""
            and rc.""ElementId"" = (select ""Id"" from rec.""RecruitmentPayElement"" where ""ElementCode"" = 'BASIC')
            Left join rec.""RecruitmentCandidateElementInfo"" as re on re.""ApplicationId"" = a.""Id""
            and re.""ElementId"" = (select ""Id"" from rec.""RecruitmentPayElement"" where ""ElementCode"" = 'SAFETYALLOWANCE')
                               WHERE a.""Id"" = '{applicationId}' and a.""IsDeleted"" = false";

            var queryData = await _queryRepoId.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<ApplicationViewModel> GetStaffJoiningDetails(string applicationId)
        {
            string query = @$"select concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName, a.""GaecNo"", a.""JobNo"",
            a.""OfferDesigination"", a.""JoiningDate"" as CandJoiningDate, n.""Name"" as NationalityName, t.""Name"" as ""TitleName"", od.""Name"" as OfferDesigination
            from rec.""Application"" as a
            left join cms.""Job"" as od on od.""Id"" = a.""JobId""
            LEFT JOIN cms.""Nationality"" as n ON n.""Id"" = a.""NationalityId""
            LEFT JOIN rec.""ListOfValue"" as t ON t.""Id"" = a.""TitleId""             
            WHERE a.""Id"" = '{applicationId}' and a.""IsDeleted"" = false";

            var queryData = await _queryRepoId.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<ApplicationViewModel> GetIntentToOffer(string applicationId)
        {
            string query = @$"select a.*, g.""Name"" as gradename,
lovaccm.""Name"" as accom,lovvisa.""Name"" as visa
FROM rec.""Application"" as a
left join cms.""Grade"" as g on g.""Id"" = a.""OfferGrade""
left join rec.""ListOfValue"" as lovaccm on lovaccm.""Id"" = a.""AccommodationId""
left join rec.""ListOfValue"" as lovvisa on lovvisa.""Id"" = a.""VisaCategory""
            WHERE a.""Id"" = '{applicationId}' and a.""IsDeleted"" = false";

            var queryData = await _queryRepoId.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<IdNameViewModel> GetGrade(string Id)
        {
            string query = @$"select ""Name""
                                from  cms.""Grade""
                                where ""IsDeleted""=false and ""Status""=1 and ""Id"" = '{Id}'  order by ""Name""";

            var queryData = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<IdNameViewModel> GetNationality(string Id)
        {
            string query = @$"select ""Name""
                                from  cms.""Nationality""
                                 where ""IsDeleted""=false and ""Status""=1 and ""Id"" = '{Id}'  order by ""Name""";

            var queryData = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<IdNameViewModel> GetTitle(string Id)
        {
            string query = @$"select ""Name""
                                from  rec.""ListOfValue""
                                where ""ListOfValueType"" = 'PERSON_TITLE' AND ""Id"" = '{Id}' ";

            var queryData = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<IdNameViewModel> GetUserName(string Id)
        {
            string query = @$"select ""Name""
                                from  public.""User""
                                where ""Id"" = '{Id}' ";

            var queryData = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<IdNameViewModel> GetUserSign()
        {
            string query = @$"select u.""SignatureId"" as Id
        from public.""UserRoleUser"" as uru
        join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId""
        join public.""User"" as u on u.""Id""=uru.""UserId""
            where ur.""Code""='ED' ";

            var queryData = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<ApplicationViewModel> GetWSADetails(string applicationId)
        {
            string query = @$"select concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName, a.""GaecNo"", a.""PassportNumber"", 
            a.""JoiningNotLaterThan"", a.""ContractStartDate"" as ContractDate, n.""Name"" as NationalityName,
            rce.""Value"" as Accomodation, rc.""Value"" as Basic, re.""Value"" as food, t.""Name"" as TitleName, od.""Name"" as OfferDesigination
            from rec.""Application"" as a
            LEFT JOIN cms.""Nationality"" as n ON n.""Id"" = a.""NationalityId""
            left join cms.""Job"" as od on od.""Id"" = a.""JobId""
            Left join rec.""RecruitmentCandidateElementInfo"" as rce on rce.""ApplicationId"" = a.""Id""
                        and rce.""ElementId"" = (select ""Id"" from rec.""RecruitmentPayElement"" where ""ElementCode"" = 'ACCOMODATION')
            Left join rec.""RecruitmentCandidateElementInfo"" as rc on rc.""ApplicationId"" = a.""Id""
                        and rc.""ElementId"" = (select ""Id"" from rec.""RecruitmentPayElement"" where ""ElementCode"" = 'BASIC')
            Left join rec.""RecruitmentCandidateElementInfo"" as re on re.""ApplicationId"" = a.""Id""
                        and re.""ElementId"" = (select ""Id"" from rec.""RecruitmentPayElement"" where ""ElementCode"" = 'FOOD')
            Left join rec.""ListOfValue"" as t on t.""Id"" = a.""TitleId""               
            WHERE a.""Id"" = '{applicationId}' and a.""IsDeleted"" = false";

            var queryData = await _queryRepoId.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<ApplicationViewModel> GetAppDetails(string applicationId)
        {
            string query = @$"select a.*, j.""Name"" as JobName, o.""Name"" as OrganizationName
                                from rec.""Application"" as a
                                left join cms.""Job"" as j on j.""Id"" = a.""JobId""
                                left join cms.""Organization"" as o on o.""Id"" = a.""OrganizationId""
                                WHERE a.""Id"" = '{applicationId}' and a.""IsDeleted"" = false";

            var queryData = await _queryRepoId.ExecuteQuerySingle(query, null);
            return queryData;
        }


        public async Task<List<ApplicationViewModel>> GetApplicationListByCandidate(string candidateId)
        {
            string query = @$"select pl.*,apstate.""Name"" as ApplicationStateName,apstate.""Code"" as ApplicationStateCode,j.""Name"" as JobTitle
            ,cat.""Name"" as JobCategoryName,loc.""Name"" as LocationName,apst.""Name"" as ApplicationStatusName
            FROM rec.""Application"" as pl
            
            left join cms.""Job"" as j on  pl.""JobId"" =j.""Id"" 
            left join rec.""JobAdvertisement"" as jd  on  pl.""JobId"" =jd.""JobId"" and jd.""Status"" = 1
            left join rec.""ListOfValue"" as cat on jd.""JobCategoryId""=cat.""Id""
            left join cms.""Location"" as loc on jd.""LocationId""=loc.""Id"" 
            LEFT JOIN rec.""ApplicationState"" as apstate ON pl.""ApplicationState"" = apstate.""Id"" 
            LEFT JOIN rec.""ApplicationStatus"" as apst ON pl.""ApplicationStatus"" = apst.""Id""
            WHERE pl.""CandidateProfileId"" = '{candidateId}' and pl.""IsDeleted"" = false";
            var queryData = await _queryRepoId.ExecuteQueryList(query, null);
            return queryData;
        }



        public async Task<IList<ApplicationStateTrackDetailViewModel>> GetAppStateTrackDetailsByCand(string applicationId)
        {
            string query = @$"select ast.""ChangedDate"", ast.""ChangedBy"", ast.""ApplicationStateId""
            , concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName
            , a.""AppliedDate"" as AppliedDate, u.""Name"" as ChangedByName,ast.""ApplicationStatusCode"" as ApplicationStatusCode
            , apst.""Code"" as StateCode,apstatus.""Code"" as StatusCode,apstatus.""Name"" as StatusName
            ,t.""TaskNo"" as TaskNo,t.""Subject"" as TaskSubject,t.""StartDate"" as TaskStartDate
            ,t.""DueDate"" as TaskDueDate, t.""SLA"" as TaskSLA,t.""SubmittedDate"" as TaskSubmittedDate
            ,t.""CompletionDate"" as TaskCompletionDate,t.""RejectedDate"" as TaskRejectedDate	
            ,t.""AssigneeUserId"" as TaskAssignedToUserId,tu.""Email"" as TaskAssignedToEmail
            ,t.""TaskStatusCode"" as TaskStatusCode,ts.""Name"" as TaskStatusName
            ,tt.""Subject"" as TaskTemplateSubject,t.""TextValue1"",t.""TextValue2"",t.""TextValue3""
            ,t.""TextValue4"",t.""TextValue5"",t.""TextValue6"",t.""TextValue7"",t.""TextValue8""
            ,t.""TextValue9"",t.""TextValue10"",tu.""Name"" as AssigneeName
            from rec.""ApplicationStateTrack"" as ast
            join rec.""Application"" as a on a.""Id"" = ast.""ApplicationId""
            left join public.""User"" as u on u.""Id"" = ast.""ChangedBy""
            left join rec.""ApplicationState"" as apst on apst.""Id"" = ast.""ApplicationStateId""
            left join rec.""ApplicationStatus"" as apstatus on apstatus.""Id"" = ast.""ApplicationStatusId""
            left join public.""RecTask"" as t on ast.""TaskReferenceId""=t.""Id""
            left join public.""User"" as tu on t.""AssigneeUserId"" = tu.""Id""
            left join rec.""ListOfValue"" as ts on t.""TaskStatusCode""=ts.""Code""
            left join public.""RecTaskTemplate"" as tt on ast.""ApplicationStatusCode""=tt.""TemplateCode"" and t.""Id"" is not null
            where ast.""ApplicationId""='{applicationId}'";

            var states = await _apstqueryRepo.ExecuteQueryList<ApplicationStateTrackDetailViewModel>(query, null);
            var list = GetAllState();
            var applied = states.FirstOrDefault(x => x.StateCode == "UnReviewed");
            if (applied != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 1.0);
                if (item != null)
                {
                    item.ActionStatus = "Applied";
                    item.ChangedDate = applied.ChangedDate;
                }
            }
            var shortListByHr = states.FirstOrDefault(x => x.StateCode == "ShortListByHr"
            && x.ApplicationStatusCode == null);
            if (shortListByHr != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 2.0);
                if (item != null)
                {
                    item.ActionStatus = shortListByHr.StatusName.Coalesce(shortListByHr.StatusCode);
                    item.ChangedDate = shortListByHr.ChangedDate;
                }
            }
            var shortListBatch = states.FirstOrDefault(x => x.StateCode == "ShortListByHr"
          && x.ApplicationStatusCode == "SL_BATCH");
            if (shortListBatch != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 2.1);
                if (item != null)
                {
                    item.ActionStatus = shortListBatch.StatusName.Coalesce(shortListBatch.ApplicationStatusCode);
                    item.ChangedDate = shortListBatch.ChangedDate;
                }
            }
            var shortListBatchSend = states.FirstOrDefault(x => x.StateCode == "ShortListByHr"
            && x.ApplicationStatusCode == "SL_BATCH_SEND");
            if (shortListBatchSend != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 2.2);
                if (item != null)
                {
                    item.ActionStatus = shortListBatchSend.StatusName.Coalesce(shortListBatchSend.ApplicationStatusCode);
                    item.ChangedDate = shortListBatchSend.ChangedDate;
                }
            }
            //var shortListHM = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
            //&& x.ApplicationStatusCode== "SL_HM");
            //if (shortListHM != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 3.0);
            //    if (item != null)
            //    {
            //        item.ActionStatus = shortListHM.StatusName.Coalesce(shortListHM.StatusCode);
            //        item.ChangedDate = shortListHM.ChangedDate;
            //    }
            //}
            var no3_1 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
             && x.ApplicationStatusCode == "SL_HM");
            if (no3_1 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.1);
                if (item != null)
                {
                    item.ActionName = no3_1.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ChangedDate = no3_1.ChangedDate;
                }
            }
            var no3_2_1 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
             && x.ApplicationStatusCode == "SCHEDULE_INTERVIEW_RECRUITER");
            if (no3_2_1 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.21);
                if (item != null)
                {
                    item.ActionName = no3_2_1.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_2_1.ActionStatus;
                    item.ActionSLAActual = no3_2_1.ActionSLAActual;
                    item.ActionSLAPlanned = no3_2_1.ActionSLAPlanned;
                    item.DaysElapsed = no3_2_1.DaysElapsed;
                    item.ChangedDate = no3_2_1.ChangedDate;
                    item.AssigneeName = no3_2_1.AssigneeName;
                    item.TextValue1 = no3_2_1.TextValue2;
                }
            }
            var no3_2_2 = states.FirstOrDefault(x => x.StateCode == "ShortListByHm"
            && x.ApplicationStatusCode == "SCHEDULE_INTERVIEW_CANDIDATE");
            if (no3_2_2 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.22);
                if (item != null)
                {
                    item.ActionName = no3_2_2.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_2_2.ActionStatus;
                    item.ActionSLAActual = no3_2_2.ActionSLAActual;
                    item.ActionSLAPlanned = no3_2_2.ActionSLAPlanned;
                    item.DaysElapsed = no3_2_2.DaysElapsed;
                    item.ChangedDate = no3_2_2.ChangedDate;
                    item.AssigneeName = no3_2_2.AssigneeName;
                    item.TextValue1 = no3_2_2.TextValue3;
                }
            }
            var no3_3 = states.FirstOrDefault(x => x.ApplicationStatusCode == "INTERVIEW_EVALUATION_HM");
            if (no3_3 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 3.3);
                if (item != null)
                {
                    item.ActionName = no3_3.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no3_3.ActionStatus;
                    item.ActionSLAActual = no3_3.ActionSLAActual;
                    item.ActionSLAPlanned = no3_3.ActionSLAPlanned;
                    item.DaysElapsed = no3_3.DaysElapsed;
                    item.ChangedDate = no3_3.ChangedDate;
                    item.AssigneeName = no3_3.AssigneeName;
                    item.TextValue1 = no3_3.TextValue2;
                }
            }
            //var no3_3 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            // && x.ApplicationStatusCode == "INTERVIEW_EVALUATION_HM");
            //if (no3_3 != null)
            //{
            //    var item = list.FirstOrDefault(x => x.UniqueNumber == 3.3);
            //    if (item != null)
            //    {
            //        item.ActionStatus = no3_3.StatusName.Coalesce(no3_3.ApplicationStatusCode);
            //        item.ChangedDate = no3_3.ChangedDate;
            //    }
            //}

            var no4_11 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
             && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL1");
            if (no4_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.11);
                if (item != null)
                {
                    item.ActionName = no4_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_11.ActionStatus;
                    item.ActionSLAActual = no4_11.ActionSLAActual;
                    item.ActionSLAPlanned = no4_11.ActionSLAPlanned;
                    item.DaysElapsed = no4_11.DaysElapsed;
                    item.ChangedDate = no4_11.ChangedDate;
                    item.AssigneeName = no4_11.AssigneeName;
                    item.TextValue1 = no4_11.TextValue4;
                }
            }
            var no4_12 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
             && x.ApplicationStatusCode == "INTENT_TO_OFFER");
            if (no4_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.12);
                if (item != null)
                {
                    item.ActionName = no4_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_12.ActionStatus;
                    item.ActionSLAActual = no4_12.ActionSLAActual;
                    item.ActionSLAPlanned = no4_12.ActionSLAPlanned;
                    item.DaysElapsed = no4_12.DaysElapsed;
                    item.ChangedDate = no4_12.ChangedDate;
                    item.AssigneeName = no4_12.AssigneeName;
                    item.TextValue1 = no4_12.TextValue2;
                }
            }
            //New step
            
            var no4_13 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL2");
            if (no4_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.13);
                if (item != null)
                {
                    item.ActionName = no4_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_13.ActionStatus;
                    item.ActionSLAActual = no4_13.ActionSLAActual;
                    item.ActionSLAPlanned = no4_13.ActionSLAPlanned;
                    item.DaysElapsed = no4_13.DaysElapsed;
                    item.ChangedDate = no4_13.ChangedDate;
                    item.AssigneeName = no4_13.AssigneeName;
                    item.TextValue1 = no4_13.TextValue2;
                }
            }
            var no4_131 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            && x.ApplicationStatusCode == "REVIEWED_INTENT_OFFER");
            if (no4_131 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.131);
                if (item != null)
                {
                    item.ActionName = no4_131.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_131.ActionStatus;
                    item.ActionSLAActual = no4_131.ActionSLAActual;
                    item.ActionSLAPlanned = no4_131.ActionSLAPlanned;
                    item.DaysElapsed = no4_131.DaysElapsed;
                    item.ChangedDate = no4_131.ChangedDate;
                    item.AssigneeName = no4_131.AssigneeName;
                    item.TextValue1 = no4_131.TextValue3;
                }
            }
            var no4_14 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
           && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL3");
            if (no4_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.14);
                if (item != null)
                {
                    item.ActionName = no4_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_14.ActionStatus;
                    item.ActionSLAActual = no4_14.ActionSLAActual;
                    item.ActionSLAPlanned = no4_14.ActionSLAPlanned;
                    item.DaysElapsed = no4_14.DaysElapsed;
                    item.ChangedDate = no4_14.ChangedDate;
                    item.AssigneeName = no4_14.AssigneeName;
                    item.TextValue1 = no4_14.TextValue2;
                }
            }
           // var no4_15 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
           //&& x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL4");
           // if (no4_15 != null)
           // {
           //     var item = list.FirstOrDefault(x => x.UniqueNumber == 4.15);
           //     if (item != null)
           //     {
           //         item.ActionName = no4_15.TaskTemplateSubject.Coalesce(item.ActionName);
           //         item.ActionStatus = no4_15.ActionStatus;
           //         item.ActionSLAActual = no4_15.ActionSLAActual;
           //         item.ActionSLAPlanned = no4_15.ActionSLAPlanned;
           //         item.DaysElapsed = no4_15.DaysElapsed;
           //         item.ChangedDate = no4_15.ChangedDate;
           //         item.AssigneeName = no4_15.AssigneeName;
           //         item.TextValue1 = no4_15.TextValue2;
           //     }
           // }
            var no4_16 = states.FirstOrDefault(x => x.StateCode == "IntentToOffer"
            && x.ApplicationStatusCode == "EMPLOYEE_APPOINTMENT_APPROVAL5");
            if (no4_16 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 4.15);
                if (item != null)
                {
                    item.ActionName = no4_16.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no4_16.ActionStatus;
                    item.ActionSLAActual = no4_16.ActionSLAActual;
                    item.ActionSLAPlanned = no4_16.ActionSLAPlanned;
                    item.DaysElapsed = no4_16.DaysElapsed;
                    item.ChangedDate = no4_16.ChangedDate;
                    item.AssigneeName = no4_16.AssigneeName;
                    item.TextValue1 = no4_16.TextValue2;
                }
            }
            var no5_11 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_HOD");
            if (no5_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.11);
                if (item != null)
                {
                    item.ActionName = no5_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_11.ActionStatus;
                    item.ActionSLAActual = no5_11.ActionSLAActual;
                    item.ActionSLAPlanned = no5_11.ActionSLAPlanned;
                    item.DaysElapsed = no5_11.DaysElapsed;
                    item.ChangedDate = no5_11.ChangedDate;
                    item.AssigneeName = no5_11.AssigneeName;
                    item.TextValue1 = no5_11.TextValue4;
                }
            }
            
            var no5_12 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "REVIEW_WORKER_HR");
            if (no5_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.12);
                if (item != null)
                {
                    item.ActionName = no5_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_12.ActionStatus;
                    item.ActionSLAActual = no5_12.ActionSLAActual;
                    item.ActionSLAPlanned = no5_12.ActionSLAPlanned;
                    item.DaysElapsed = no5_12.DaysElapsed;
                    item.ChangedDate = no5_12.ChangedDate;
                    item.AssigneeName = no5_12.AssigneeName;
                    item.TextValue1 = no5_12.TextValue1;
                }
            }
            var no5_13 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_HR_HEAD");
            if (no5_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.13);
                if (item != null)
                {
                    item.ActionName = no5_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_13.ActionStatus;
                    item.ActionSLAActual = no5_13.ActionSLAActual;
                    item.ActionSLAPlanned = no5_13.ActionSLAPlanned;
                    item.DaysElapsed = no5_13.DaysElapsed;
                    item.ChangedDate = no5_13.ChangedDate;
                    item.AssigneeName = no5_13.AssigneeName;
                    item.TextValue1 = no5_13.TextValue4;
                }
            }
            var no5_14 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_ED");
            if (no5_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.14);
                if (item != null)
                {
                    item.ActionName = no5_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_14.ActionStatus;
                    item.ActionSLAActual = no5_14.ActionSLAActual;
                    item.ActionSLAPlanned = no5_14.ActionSLAPlanned;
                    item.DaysElapsed = no5_14.DaysElapsed;
                    item.ChangedDate = no5_14.ChangedDate;
                    item.AssigneeName = no5_14.AssigneeName;
                    item.TextValue1 = no5_14.TextValue4;
                }
            }
            var no5_15 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_POOL_HR");
            if (no5_15 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.15);
                if (item != null)
                {
                    item.ActionName = no5_15.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_15.ActionStatus;
                    item.ActionSLAActual = no5_15.ActionSLAActual;
                    item.ActionSLAPlanned = no5_15.ActionSLAPlanned;
                    item.DaysElapsed = no5_15.DaysElapsed;
                    item.ChangedDate = no5_15.ChangedDate;
                    item.AssigneeName = no5_15.AssigneeName;
                    item.TextValue1 = no5_15.TextValue4;
                }
            }
            var no5_16 = states.FirstOrDefault(x => x.StateCode == "WorkerPool"
            && x.ApplicationStatusCode == "WORKER_SALARY_AGENCY");
            if (no5_16 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 5.16);
                if (item != null)
                {
                    item.ActionName = no5_16.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no5_16.ActionStatus;
                    item.ActionSLAActual = no5_16.ActionSLAActual;
                    item.ActionSLAPlanned = no5_16.ActionSLAPlanned;
                    item.DaysElapsed = no5_16.DaysElapsed;
                    item.ChangedDate = no5_16.ChangedDate;
                    item.AssigneeName = no5_16.AssigneeName;
                    item.TextValue1 = no5_16.TextValue2;
                }
            }

            var no6_11 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
            && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_RECRUITER");
            if (no6_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.11);
                if (item != null)
                {
                    item.ActionName = no6_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_11.ActionStatus;
                    item.ActionSLAActual = no6_11.ActionSLAActual;
                    item.ActionSLAPlanned = no6_11.ActionSLAPlanned;
                    item.DaysElapsed = no6_11.DaysElapsed;
                    item.ChangedDate = no6_11.ChangedDate;
                    item.AssigneeName = no6_11.AssigneeName;
                    item.TextValue1 = no6_11.TextValue6;
                }
            }
            var no6_12 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
           && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_HR_HEAD");
            if (no6_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.12);
                if (item != null)
                {
                    item.ActionName = no6_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_12.ActionStatus;
                    item.ActionSLAActual = no6_12.ActionSLAActual;
                    item.ActionSLAPlanned = no6_12.ActionSLAPlanned;
                    item.DaysElapsed = no6_12.DaysElapsed;
                    item.ChangedDate = no6_12.ChangedDate;
                    item.AssigneeName = no6_12.AssigneeName;
                    item.TextValue1 = no6_12.TextValue2;
                }
            }
            var no6_13 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
          && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_ED");
            if (no6_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.13);
                if (item != null)
                {
                    item.ActionName = no6_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_13.ActionStatus;
                    item.ActionSLAActual = no6_13.ActionSLAActual;
                    item.ActionSLAPlanned = no6_13.ActionSLAPlanned;
                    item.DaysElapsed = no6_13.DaysElapsed;
                    item.ChangedDate = no6_13.ChangedDate;
                    item.AssigneeName = no6_13.AssigneeName;
                    item.TextValue1 = no6_13.TextValue2;
                }
            }
            var no6_14 = states.FirstOrDefault(x => x.StateCode == "FinalOffer"
            && x.ApplicationStatusCode == "FINAL_OFFER_APPROVAL_CANDIDATE");
            if (no6_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 6.14);
                if (item != null)
                {
                    item.ActionName = no6_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no6_14.ActionStatus;
                    item.ActionSLAActual = no6_14.ActionSLAActual;
                    item.ActionSLAPlanned = no6_14.ActionSLAPlanned;
                    item.DaysElapsed = no6_14.DaysElapsed;
                    item.ChangedDate = no6_14.ChangedDate;
                    item.AssigneeName = no6_14.AssigneeName;
                    item.TextValue1 = no6_14.TextValue2;
                }
            }
            var no7_11 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "INFORM_CANDIDATE_FOR_MEDICAL");
            if (no7_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.11);
                if (item != null)
                {
                    item.ActionName = no7_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_11.ActionStatus;
                    item.ActionSLAActual = no7_11.ActionSLAActual;
                    item.ActionSLAPlanned = no7_11.ActionSLAPlanned;
                    item.DaysElapsed = no7_11.DaysElapsed;
                    item.ChangedDate = no7_11.ChangedDate;
                    item.AssigneeName = no7_11.AssigneeName;
                    item.TextValue1 = no7_11.TextValue2;
                }
            }
            var no7_12 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
           && x.ApplicationStatusCode == "CHECK_MEDICAL_REPORT_INFORM_PRO");
            if (no7_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.12);
                if (item != null)
                {
                    item.ActionName = no7_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_12.ActionStatus;
                    item.ActionSLAActual = no7_12.ActionSLAActual;
                    item.ActionSLAPlanned = no7_12.ActionSLAPlanned;
                    item.DaysElapsed = no7_12.DaysElapsed;
                    item.ChangedDate = no7_12.ChangedDate;
                    item.AssigneeName = no7_12.AssigneeName;
                    item.TextValue1 = no7_12.TextValue2;
                }
            }
            var no7_13 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
           && x.ApplicationStatusCode == "OBTAIN_BUSINESS_VISA");
            if (no7_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.13);
                if (item != null)
                {
                    item.ActionName = no7_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_13.ActionStatus;
                    item.ActionSLAActual = no7_13.ActionSLAActual;
                    item.ActionSLAPlanned = no7_13.ActionSLAPlanned;
                    item.DaysElapsed = no7_13.DaysElapsed;
                    item.ChangedDate = no7_13.ChangedDate;
                    item.AssigneeName = no7_13.AssigneeName;
                    item.TextValue1 = no7_13.TextValue6;
                }
            }
            var no7_14 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE");
            if (no7_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.14);
                if (item != null)
                {
                    item.ActionName = no7_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_14.ActionStatus;
                    item.ActionSLAActual = no7_14.ActionSLAActual;
                    item.ActionSLAPlanned = no7_14.ActionSLAPlanned;
                    item.DaysElapsed = no7_14.DaysElapsed;
                    item.ChangedDate = no7_14.ChangedDate;
                    item.AssigneeName = no7_14.AssigneeName;
                    item.TextValue1 = no7_14.TextValue6;
                }
            }
            var no7_151 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "CONFIRM_TRAVELING_DATE");
            if (no7_151 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.151);
                if (item != null)
                {
                    item.ActionName = no7_151.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_151.ActionStatus;
                    item.ActionSLAActual = no7_151.ActionSLAActual;
                    item.ActionSLAPlanned = no7_151.ActionSLAPlanned;
                    item.DaysElapsed = no7_151.DaysElapsed;
                    item.ChangedDate = no7_151.ChangedDate;
                    item.AssigneeName = no7_151.AssigneeName;
                    item.TextValue1 = no7_151.TextValue4;
                }
            }
            var no7_152 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "BOOK_TICKET_ATTACH");
            if (no7_152 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.152);
                if (item != null)
                {
                    item.ActionName = no7_152.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_152.ActionStatus;
                    item.ActionSLAActual = no7_152.ActionSLAActual;
                    item.ActionSLAPlanned = no7_152.ActionSLAPlanned;
                    item.DaysElapsed = no7_152.DaysElapsed;
                    item.ChangedDate = no7_152.ChangedDate;
                    item.AssigneeName = no7_152.AssigneeName;
                    item.TextValue1 = no7_152.TextValue3;
                }
            }
            var no7_153 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "TICKET_ATTACH");
            if (no7_153 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.153);
                if (item != null)
                {
                    item.ActionName = no7_153.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_153.ActionStatus;
                    item.ActionSLAActual = no7_153.ActionSLAActual;
                    item.ActionSLAPlanned = no7_153.ActionSLAPlanned;
                    item.DaysElapsed = no7_153.DaysElapsed;
                    item.ChangedDate = no7_153.ChangedDate;
                    item.AssigneeName = no7_153.AssigneeName;
                    item.TextValue1 = no7_153.TextValue2;
                }
            }
            var no7_154 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
           && x.ApplicationStatusCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL");
            if (no7_154 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.154);
                if (item != null)
                {
                    item.ActionName = no7_154.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_154.ActionStatus;
                    item.ActionSLAActual = no7_154.ActionSLAActual;
                    item.ActionSLAPlanned = no7_154.ActionSLAPlanned;
                    item.DaysElapsed = no7_154.DaysElapsed;
                    item.ChangedDate = no7_154.ChangedDate;
                    item.AssigneeName = no7_154.AssigneeName;
                    item.TextValue1 = no7_154.TextValue2;
                }
            }
            var no7_155 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
          && x.ApplicationStatusCode == "ARRANGE_ACCOMMODATION");
            if (no7_155 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.155);
                if (item != null)
                {
                    item.ActionName = no7_155.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_155.ActionStatus;
                    item.ActionSLAActual = no7_155.ActionSLAActual;
                    item.ActionSLAPlanned = no7_155.ActionSLAPlanned;
                    item.DaysElapsed = no7_155.DaysElapsed;
                    item.ChangedDate = no7_155.ChangedDate;
                    item.AssigneeName = no7_155.AssigneeName;
                    item.TextValue1 = no7_155.TextValue2;
                }
            }
            var no7_156 = states.FirstOrDefault(x => x.StateCode == "BusinessVisa"
            && x.ApplicationStatusCode == "ARRANGE_VEHICLE_PICKUP");
            if (no7_156 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.156);
                if (item != null)
                {
                    item.ActionName = no7_156.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_156.ActionStatus;
                    item.ActionSLAActual = no7_156.ActionSLAActual;
                    item.ActionSLAPlanned = no7_156.ActionSLAPlanned;
                    item.DaysElapsed = no7_156.DaysElapsed;
                    item.ChangedDate = no7_156.ChangedDate;
                    item.AssigneeName = no7_156.AssigneeName;
                    item.TextValue1 = no7_156.TextValue2;
                }
            }
            
            var no7_21 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "APPLY_WORK_VISA_THROUGH_MOL");
            if (no7_21 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.21);
                if (item != null)
                {
                    item.ActionName = no7_21.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_21.ActionStatus;
                    item.ActionSLAActual = no7_21.ActionSLAActual;
                    item.ActionSLAPlanned = no7_21.ActionSLAPlanned;
                    item.DaysElapsed = no7_21.DaysElapsed;
                    item.ChangedDate = no7_21.ChangedDate;
                    item.AssigneeName = no7_21.AssigneeName;
                    item.TextValue1 = no7_21.TextValue4;
                }
            }
            var no7_22 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "BOOK_QVC_APPOINTMENT");
            if (no7_22 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.22);
                if (item != null)
                {
                    item.ActionName = no7_22.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_22.ActionStatus;
                    item.ActionSLAActual = no7_22.ActionSLAActual;
                    item.ActionSLAPlanned = no7_22.ActionSLAPlanned;
                    item.DaysElapsed = no7_22.DaysElapsed;
                    item.ChangedDate = no7_22.ChangedDate;
                    item.AssigneeName = no7_22.AssigneeName;
                    item.TextValue1 = no7_22.TextValue3;
                }
            }
            var no7_23 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "CONDUCT_MEDICAL_FINGER_PRINT");
            if (no7_23 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.23);
                if (item != null)
                {
                    item.ActionName = no7_23.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_23.ActionStatus;
                    item.ActionSLAActual = no7_23.ActionSLAActual;
                    item.ActionSLAPlanned = no7_23.ActionSLAPlanned;
                    item.DaysElapsed = no7_23.DaysElapsed;
                    item.ChangedDate = no7_23.ChangedDate;
                    item.AssigneeName = no7_23.AssigneeName;
                    item.TextValue1 = no7_23.TextValue3;
                }
            }

            var no7_24 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "FIT_UNFIT_ATTACH_VISA_COPY");
            if (no7_24 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.24);
                if (item != null)
                {
                    item.ActionName = no7_24.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_24.ActionStatus;
                    item.ActionSLAActual = no7_24.ActionSLAActual;
                    item.ActionSLAPlanned = no7_24.ActionSLAPlanned;
                    item.DaysElapsed = no7_24.DaysElapsed;
                    item.ChangedDate = no7_24.ChangedDate;
                    item.AssigneeName = no7_24.AssigneeName;
                    item.TextValue1 = no7_24.TextValue5;
                }
            }

            var no7_25 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE");
            if (no7_25 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.25);
                if (item != null)
                {
                    item.ActionName = no7_25.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_25.ActionStatus;
                    item.ActionSLAActual = no7_25.ActionSLAActual;
                    item.ActionSLAPlanned = no7_25.ActionSLAPlanned;
                    item.DaysElapsed = no7_25.DaysElapsed;
                    item.ChangedDate = no7_25.ChangedDate;
                    item.AssigneeName = no7_25.AssigneeName;
                    item.TextValue1 = no7_25.TextValue6;
                }
            }

            var no7_251 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "CONFIRM_TRAVELING_DATE");
            if (no7_251 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.251);
                if (item != null)
                {
                    item.ActionName = no7_251.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_251.ActionStatus;
                    item.ActionSLAActual = no7_251.ActionSLAActual;
                    item.ActionSLAPlanned = no7_251.ActionSLAPlanned;
                    item.DaysElapsed = no7_251.DaysElapsed;
                    item.ChangedDate = no7_251.ChangedDate;
                    item.AssigneeName = no7_251.AssigneeName;
                    item.TextValue1 = no7_251.TextValue4;
                }
            }

            var no7_252 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "BOOK_TICKET_ATTACH");
            if (no7_252 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.252);
                if (item != null)
                {
                    item.ActionName = no7_252.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_252.ActionStatus;
                    item.ActionSLAActual = no7_252.ActionSLAActual;
                    item.ActionSLAPlanned = no7_252.ActionSLAPlanned;
                    item.DaysElapsed = no7_252.DaysElapsed;
                    item.ChangedDate = no7_252.ChangedDate;
                    item.AssigneeName = no7_252.AssigneeName;
                    item.TextValue1 = no7_252.TextValue3;
                }
            }
            var no7_253 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "TICKET_ATTACH");
            if (no7_253 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.253);
                if (item != null)
                {
                    item.ActionName = no7_253.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_253.ActionStatus;
                    item.ActionSLAActual = no7_253.ActionSLAActual;
                    item.ActionSLAPlanned = no7_253.ActionSLAPlanned;
                    item.DaysElapsed = no7_253.DaysElapsed;
                    item.ChangedDate = no7_253.ChangedDate;
                    item.AssigneeName = no7_253.AssigneeName;
                    item.TextValue1 = no7_253.TextValue2;
                }
            }
            var no7_254 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL");
            if (no7_254 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.254);
                if (item != null)
                {
                    item.ActionName = no7_254.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_254.ActionStatus;
                    item.ActionSLAActual = no7_254.ActionSLAActual;
                    item.ActionSLAPlanned = no7_254.ActionSLAPlanned;
                    item.DaysElapsed = no7_254.DaysElapsed;
                    item.ChangedDate = no7_254.ChangedDate;
                    item.AssigneeName = no7_254.AssigneeName;
                    item.TextValue1 = no7_254.TextValue2;
                }
            }

            var no7_255 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "ARRANGE_ACCOMMODATION");
            if (no7_255 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.255);
                if (item != null)
                {
                    item.ActionName = no7_255.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_255.ActionStatus;
                    item.ActionSLAActual = no7_255.ActionSLAActual;
                    item.ActionSLAPlanned = no7_255.ActionSLAPlanned;
                    item.DaysElapsed = no7_255.DaysElapsed;
                    item.ChangedDate = no7_255.ChangedDate;
                    item.AssigneeName = no7_255.AssigneeName;
                    item.TextValue1 = no7_255.TextValue2;
                }
            }

            var no7_256 = states.FirstOrDefault(x => x.StateCode == "WorkVisa"
            && x.ApplicationStatusCode == "ARRANGE_VEHICLE_PICKUP");
            if (no7_256 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.256);
                if (item != null)
                {
                    item.ActionName = no7_256.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_256.ActionStatus;
                    item.ActionSLAActual = no7_256.ActionSLAActual;
                    item.ActionSLAPlanned = no7_256.ActionSLAPlanned;
                    item.DaysElapsed = no7_256.DaysElapsed;
                    item.ChangedDate = no7_256.ChangedDate;
                    item.AssigneeName = no7_256.AssigneeName;
                    item.TextValue1 = no7_256.TextValue2;
                }
            }

            var no7_31 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
           && x.ApplicationStatusCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS");
            if (no7_31 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.31);
                if (item != null)
                {
                    item.ActionName = no7_31.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_31.ActionStatus;
                    item.ActionSLAActual = no7_31.ActionSLAActual;
                    item.ActionSLAPlanned = no7_31.ActionSLAPlanned;
                    item.DaysElapsed = no7_31.DaysElapsed;
                    item.ChangedDate = no7_31.ChangedDate;
                    item.AssigneeName = no7_31.AssigneeName;
                    item.TextValue1 = no7_31.TextValue6;
                }
            }

            var no7_32 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
          && x.ApplicationStatusCode == "VERIFY_DOCUMENTS");
            if (no7_32 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.32);
                if (item != null)
                {
                    item.ActionName = no7_32.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_32.ActionStatus;
                    item.ActionSLAActual = no7_32.ActionSLAActual;
                    item.ActionSLAPlanned = no7_32.ActionSLAPlanned;
                    item.DaysElapsed = no7_32.DaysElapsed;
                    item.ChangedDate = no7_32.ChangedDate;
                    item.AssigneeName = no7_32.AssigneeName;
                    item.TextValue1 = no7_32.TextValue6;
                }
            }

            var no7_33 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
         && x.ApplicationStatusCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP");
            if (no7_33 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.33);
                if (item != null)
                {
                    item.ActionName = no7_33.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_33.ActionStatus;
                    item.ActionSLAActual = no7_33.ActionSLAActual;
                    item.ActionSLAPlanned = no7_33.ActionSLAPlanned;
                    item.DaysElapsed = no7_33.DaysElapsed;
                    item.ChangedDate = no7_33.ChangedDate;
                    item.AssigneeName = no7_33.AssigneeName;
                    item.TextValue1 = no7_33.TextValue6;
                }
            }

            var no7_34 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
         && x.ApplicationStatusCode == "VERIFY_VISA_TRANSFER_COMPLETED");
            if (no7_34 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.34);
                if (item != null)
                {
                    item.ActionName = no7_34.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_34.ActionStatus;
                    item.ActionSLAActual = no7_34.ActionSLAActual;
                    item.ActionSLAPlanned = no7_34.ActionSLAPlanned;
                    item.DaysElapsed = no7_34.DaysElapsed;
                    item.ChangedDate = no7_34.ChangedDate;
                    item.AssigneeName = no7_34.AssigneeName;
                    item.TextValue1 = no7_34.TextValue6;
                }
            }

            var no7_35 = states.FirstOrDefault(x => x.StateCode == "VisaTransfer"
        && x.ApplicationStatusCode == "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE");
            if (no7_35 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.35);
                if (item != null)
                {
                    item.ActionName = no7_35.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_35.ActionStatus;
                    item.ActionSLAActual = no7_35.ActionSLAActual;
                    item.ActionSLAPlanned = no7_35.ActionSLAPlanned;
                    item.DaysElapsed = no7_35.DaysElapsed;
                    item.ChangedDate = no7_35.ChangedDate;
                    item.AssigneeName = no7_35.AssigneeName;
                    item.TextValue1 = no7_35.TextValue6;
                }
            }

            var no7_41 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            && x.ApplicationStatusCode == "SUBMIT_WORK_PERMIT_DOCUMENTS");
            if (no7_41 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.41);
                if (item != null)
                {
                    item.ActionName = no7_41.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_41.ActionStatus;
                    item.ActionSLAActual = no7_41.ActionSLAActual;
                    item.ActionSLAPlanned = no7_41.ActionSLAPlanned;
                    item.DaysElapsed = no7_41.DaysElapsed;
                    item.ChangedDate = no7_41.ChangedDate;
                    item.AssigneeName = no7_41.AssigneeName;
                    item.TextValue1 = no7_41.TextValue7;
                }
            }
            var no7_42 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
        && x.ApplicationStatusCode == "VERIFY_WORK_PERMIT_DOCUMENTS");
            if (no7_42 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.42);
                if (item != null)
                {
                    item.ActionName = no7_42.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_42.ActionStatus;
                    item.ActionSLAActual = no7_42.ActionSLAActual;
                    item.ActionSLAPlanned = no7_42.ActionSLAPlanned;
                    item.DaysElapsed = no7_42.DaysElapsed;
                    item.ChangedDate = no7_42.ChangedDate;
                    item.AssigneeName = no7_42.AssigneeName;
                    item.TextValue1 = no7_42.TextValue7;
                }
            }
            var no7_43 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
        && x.ApplicationStatusCode == "OBTAIN_WORK_PERMIT_ATTACH");
            if (no7_43 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.43);
                if (item != null)
                {
                    item.ActionName = no7_43.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_43.ActionStatus;
                    item.ActionSLAActual = no7_43.ActionSLAActual;
                    item.ActionSLAPlanned = no7_43.ActionSLAPlanned;
                    item.DaysElapsed = no7_43.DaysElapsed;
                    item.ChangedDate = no7_43.ChangedDate;
                    item.AssigneeName = no7_43.AssigneeName;
                    item.TextValue1 = no7_43.TextValue8;
                }
            }
            var no7_44 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            && x.ApplicationStatusCode == "VERIFY_WORK_PERMIT_OBTAINED");
            if (no7_44 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.44);
                if (item != null)
                {
                    item.ActionName = no7_44.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_44.ActionStatus;
                    item.ActionSLAActual = no7_44.ActionSLAActual;
                    item.ActionSLAPlanned = no7_44.ActionSLAPlanned;
                    item.DaysElapsed = no7_44.DaysElapsed;
                    item.ChangedDate = no7_44.ChangedDate;
                    item.AssigneeName = no7_44.AssigneeName;
                    item.TextValue1 = no7_44.TextValue5;
                }
            }
            var no7_45 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            && x.ApplicationStatusCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE");
            if (no7_45 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.45);
                if (item != null)
                {
                    item.ActionName = no7_45.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_45.ActionStatus;
                    item.ActionSLAActual = no7_45.ActionSLAActual;
                    item.ActionSLAPlanned = no7_45.ActionSLAPlanned;
                    item.DaysElapsed = no7_45.DaysElapsed;
                    item.ChangedDate = no7_45.ChangedDate;
                    item.AssigneeName = no7_45.AssigneeName;
                    item.TextValue1 = no7_45.TextValue5;
                }
            }
            var no7_46 = states.FirstOrDefault(x => x.StateCode == "WorkPermit"
            && x.ApplicationStatusCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE");
            if (no7_45 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 7.45);
                if (item != null)
                {
                    item.ActionName = no7_45.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no7_45.ActionStatus;
                    item.ActionSLAActual = no7_45.ActionSLAActual;
                    item.ActionSLAPlanned = no7_45.ActionSLAPlanned;
                    item.DaysElapsed = no7_45.DaysElapsed;
                    item.ChangedDate = no7_45.ChangedDate;
                    item.AssigneeName = no7_45.AssigneeName;
                    item.TextValue1 = no7_45.TextValue5;
                }
            }

            var no8_11 = states.FirstOrDefault(x => x.StateCode == "StaffJoined"
            && x.ApplicationStatusCode == "FILL_STAFF_DETAILS");
            if (no8_11 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.11);
                if (item != null)
                {
                    item.ActionName = no8_11.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_11.ActionStatus;
                    item.ActionSLAActual = no8_11.ActionSLAActual;
                    item.ActionSLAPlanned = no8_11.ActionSLAPlanned;
                    item.DaysElapsed = no8_11.DaysElapsed;
                    item.ChangedDate = no8_11.ChangedDate;
                    item.AssigneeName = no8_11.AssigneeName;
                    item.TextValue1 = no8_11.TextValue9;
                }
            }

            var no8_12 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "PROVIDE_CASH_VOUCHER");
            if (no8_12 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.12);
                if (item != null)
                {
                    item.ActionName = no8_12.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_12.ActionStatus;
                    item.ActionSLAActual = no8_12.ActionSLAActual;
                    item.ActionSLAPlanned = no8_12.ActionSLAPlanned;
                    item.DaysElapsed = no8_12.DaysElapsed;
                    item.ChangedDate = no8_12.ChangedDate;
                    item.AssigneeName = no8_12.AssigneeName;
                    item.TextValue1 = no8_12.TextValue2;
                }
            }
            var no8_13 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
           && x.ApplicationStatusCode == "SEND_INTIMATION_EMAIL_ORG_UNIT_HOD_COPYING_IT");
            if (no8_13 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.13);
                if (item != null)
                {
                    item.ActionName = no8_13.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_13.ActionStatus;
                    item.ActionSLAActual = no8_13.ActionSLAActual;
                    item.ActionSLAPlanned = no8_13.ActionSLAPlanned;
                    item.DaysElapsed = no8_13.DaysElapsed;
                    item.ChangedDate = no8_13.ChangedDate;
                    item.AssigneeName = no8_13.AssigneeName;
                    item.TextValue1 = no8_13.TextValue3;
                }
            }

            var no8_14 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "SEND_FRA_HRA_REQUEST_FINANCE");
            if (no8_14 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.14);
                if (item != null)
                {
                    item.ActionName = no8_14.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_14.ActionStatus;
                    item.ActionSLAActual = no8_14.ActionSLAActual;
                    item.ActionSLAPlanned = no8_14.ActionSLAPlanned;
                    item.DaysElapsed = no8_14.DaysElapsed;
                    item.ChangedDate = no8_14.ChangedDate;
                    item.AssigneeName = no8_14.AssigneeName;
                    item.TextValue1 = no8_14.TextValue3;
                }
            }
            var no8_15 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "UPLOAD_PASSPORT_VISA_QATARID");
            if (no8_15 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.15);
                if (item != null)
                {
                    item.ActionName = no8_15.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_15.ActionStatus;
                    item.ActionSLAActual = no8_15.ActionSLAActual;
                    item.ActionSLAPlanned = no8_15.ActionSLAPlanned;
                    item.DaysElapsed = no8_15.DaysElapsed;
                    item.ChangedDate = no8_15.ChangedDate;
                    item.AssigneeName = no8_15.AssigneeName;
                    item.TextValue1 = no8_15.TextValue6;
                }
            }

            var no8_16 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "UPDATE_EMPLOYEE_FILE_IN_SAP");
            if (no8_16 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.16);
                if (item != null)
                {
                    item.ActionName = no8_16.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_16.ActionStatus;
                    item.ActionSLAActual = no8_16.ActionSLAActual;
                    item.ActionSLAPlanned = no8_16.ActionSLAPlanned;
                    item.DaysElapsed = no8_16.DaysElapsed;
                    item.ChangedDate = no8_16.ChangedDate;
                    item.AssigneeName = no8_16.AssigneeName;
                    item.TextValue1 = no8_16.TextValue1;
                }
            }


            var no8_17 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "CONFIRM_INDUCTION_DATE_TO_CANDIDATE");
            if (no8_17 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.17);
                if (item != null)
                {
                    item.ActionName = no8_17.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_17.ActionStatus;
                    item.ActionSLAActual = no8_17.ActionSLAActual;
                    item.ActionSLAPlanned = no8_17.ActionSLAPlanned;
                    item.DaysElapsed = no8_17.DaysElapsed;
                    item.ChangedDate = no8_17.ChangedDate;
                    item.AssigneeName = no8_17.AssigneeName;
                    item.TextValue1 = no8_17.TextValue2;
                }
            }

            var no8_18 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HM");
            if (no8_18 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.18);
                if (item != null)
                {
                    item.ActionName = no8_18.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_18.ActionStatus;
                    item.ActionSLAActual = no8_18.ActionSLAActual;
                    item.ActionSLAPlanned = no8_18.ActionSLAPlanned;
                    item.DaysElapsed = no8_18.DaysElapsed;
                    item.ChangedDate = no8_18.ChangedDate;
                    item.AssigneeName = no8_18.AssigneeName;
                    item.TextValue1 = no8_18.TextValue1;
                }
            }
            var no8_19 = states.FirstOrDefault(x => x.StateCode == "PostStaffJoined"
            && x.ApplicationStatusCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HR");
            if (no8_19 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.19);
                if (item != null)
                {
                    item.ActionName = no8_19.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_19.ActionStatus;
                    item.ActionSLAActual = no8_19.ActionSLAActual;
                    item.ActionSLAPlanned = no8_19.ActionSLAPlanned;
                    item.DaysElapsed = no8_19.DaysElapsed;
                    item.ChangedDate = no8_19.ChangedDate;
                    item.AssigneeName = no8_19.AssigneeName;
                    item.TextValue1 = no8_19.TextValue1;
                }
            }
            var no8_21 = states.FirstOrDefault(x => x.StateCode == "WorkerJoined"
            && x.ApplicationStatusCode == "FILL_WORKER_DETAILS");
            if (no8_21 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.21);
                if (item != null)
                {
                    item.ActionName = no8_21.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_21.ActionStatus;
                    item.ActionSLAActual = no8_21.ActionSLAActual;
                    item.ActionSLAPlanned = no8_21.ActionSLAPlanned;
                    item.DaysElapsed = no8_21.DaysElapsed;
                    item.ChangedDate = no8_21.ChangedDate;
                    item.AssigneeName = no8_21.AssigneeName;
                    item.TextValue1 = no8_21.TextValue9;
                }
            }
            var no8_22 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
           && x.ApplicationStatusCode == "PROVIDE_CASH_VOUCHER_WORKER");
            if (no8_22 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.22);
                if (item != null)
                {
                    item.ActionName = no8_22.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_22.ActionStatus;
                    item.ActionSLAActual = no8_22.ActionSLAActual;
                    item.ActionSLAPlanned = no8_22.ActionSLAPlanned;
                    item.DaysElapsed = no8_22.DaysElapsed;
                    item.ChangedDate = no8_22.ChangedDate;
                    item.AssigneeName = no8_22.AssigneeName;
                    item.TextValue1 = no8_22.TextValue2;
                }
            }
            var no8_23 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
            && x.ApplicationStatusCode == "UPLOAD_PASSPORT_VISA_QATARID_WORKER");
            if (no8_23 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.23);
                if (item != null)
                {
                    item.ActionName = no8_23.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_23.ActionStatus;
                    item.ActionSLAActual = no8_23.ActionSLAActual;
                    item.ActionSLAPlanned = no8_23.ActionSLAPlanned;
                    item.DaysElapsed = no8_23.DaysElapsed;
                    item.ChangedDate = no8_23.ChangedDate;
                    item.AssigneeName = no8_23.AssigneeName;
                    item.TextValue1 = no8_23.TextValue6;
                }
            }
            var no8_24 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
           && x.ApplicationStatusCode == "UPDATE_EMPLOYEE_FILE_IN_SAP_WORKER");
            if (no8_24 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.24);
                if (item != null)
                {
                    item.ActionName = no8_24.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_24.ActionStatus;
                    item.ActionSLAActual = no8_24.ActionSLAActual;
                    item.ActionSLAPlanned = no8_24.ActionSLAPlanned;
                    item.DaysElapsed = no8_24.DaysElapsed;
                    item.ChangedDate = no8_24.ChangedDate;
                    item.AssigneeName = no8_24.AssigneeName;
                    item.TextValue1 = no8_24.TextValue1;
                }
            }

            var no8_25 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
          && x.ApplicationStatusCode == "CONDUCT_INDUCTION");
            if (no8_25 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.25);
                if (item != null)
                {
                    item.ActionName = no8_25.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_25.ActionStatus;
                    item.ActionSLAActual = no8_25.ActionSLAActual;
                    item.ActionSLAPlanned = no8_25.ActionSLAPlanned;
                    item.DaysElapsed = no8_25.DaysElapsed;
                    item.ChangedDate = no8_25.ChangedDate;
                    item.AssigneeName = no8_25.AssigneeName;
                    item.TextValue1 = no8_25.TextValue1;
                }
            }
            var no8_26 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
          && x.ApplicationStatusCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HM");
            if (no8_26 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.26);
                if (item != null)
                {
                    item.ActionName = no8_26.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_26.ActionStatus;
                    item.ActionSLAActual = no8_26.ActionSLAActual;
                    item.ActionSLAPlanned = no8_26.ActionSLAPlanned;
                    item.DaysElapsed = no8_26.DaysElapsed;
                    item.ChangedDate = no8_26.ChangedDate;
                    item.AssigneeName = no8_26.AssigneeName;
                    item.TextValue1 = no8_26.TextValue2;
                }
            }
            var no8_27 = states.FirstOrDefault(x => x.StateCode == "PostWorkerJoined"
          && x.ApplicationStatusCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HR");
            if (no8_27 != null)
            {
                var item = list.FirstOrDefault(x => x.UniqueNumber == 8.27);
                if (item != null)
                {
                    item.ActionName = no8_27.TaskTemplateSubject.Coalesce(item.ActionName);
                    item.ActionStatus = no8_27.ActionStatus;
                    item.ActionSLAActual = no8_27.ActionSLAActual;
                    item.ActionSLAPlanned = no8_27.ActionSLAPlanned;
                    item.DaysElapsed = no8_27.DaysElapsed;
                    item.ChangedDate = no8_27.ChangedDate;
                    item.AssigneeName = no8_27.AssigneeName;
                    item.TextValue1 = no8_27.TextValue2;
                }
            }
            return list;
        }
        private List<ApplicationStateTrackDetailViewModel> GetAllState()
        {
            var list = new List<ApplicationStateTrackDetailViewModel>();
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 1.0,
                Number = "1",
                Stage = "Applied for job",
                ActionName = "Submit Application"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 2.0,
                Number = "2",
                Stage = "Shortlist by HR",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 2.1,
                Number = "",
                SubNumber = "2.1",
                Stage = "",
                ActionName = "Shortlist to Batch – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 2.2,
                Number = "",
                SubNumber = "2.2",
                Stage = "",
                ActionName = "Batch sent to Hiring Manager - Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.0,
                Number = "3",
                SubNumber = "",
                Stage = "Shortlist by HM",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.1,
                Number = "",
                SubNumber = "3.1",
                Stage = "",
                ActionName = "Shortlist for interview – Hiring Manager"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.21,
                Number = "",
                SubNumber = "3.2.1",
                Stage = "",
                ActionName = "Arrange for interview – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.22,
                Number = "",
                SubNumber = "3.2.2",
                Stage = "",
                ActionName = "Accept interview date/time – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 3.3,
                Number = "",
                SubNumber = "3.3",
                Stage = "",
                ActionName = "Evaluation – Hiring Manager"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.0,
                Number = "4",
                SubNumber = "",
                Stage = "Intent to Offer [Staff Only]",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.11,
                Number = "",
                SubNumber = "4.1.1",
                Stage = "",
                ActionName = "Prepare intent to offer – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.12,
                Number = "",
                SubNumber = "4.1.2",
                Stage = "",
                ActionName = "Accept intent to offer – Candidate"
            });
            //New step
            
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.13,
                Number = "",
                SubNumber = "4.1.3",
                Stage = "",
                ActionName = "Approval – HOD"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.131,
                Number = "",
                SubNumber = "4.1.3.1",
                Stage = "",
                ActionName = "Reviewed intent to offer – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.14,
                Number = "",
                SubNumber = "4.1.4",
                Stage = "",
                ActionName = "Approval – HR Head"
            });
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 4.15,
            //    Number = "",
            //    SubNumber = "4.1.5",
            //    Stage = "",
            //    ActionName = "Approval – Planning Head"
            //});
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 4.15,
                Number = "",
                SubNumber = "4.1.5",
                Stage = "",
                ActionName = "Approval – ED"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.0,
                Number = "5",
                SubNumber = "",
                Stage = "Worker Appointment request",
                ActionName = ""
            });                       
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.11,
                Number = "",
                SubNumber = "5.1.1",
                Stage = "",
                ActionName = "Approval – HOD"
            });
            //list.Add(new ApplicationStateTrackDetailViewModel
            //{
            //    UniqueNumber = 5.22,
            //    Number = "",
            //    SubNumber = "5.2.2",
            //    Stage = "",
            //    ActionName = "Approval – Planning Head"
            //});
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.12,
                Number = "",
                SubNumber = "5.1.2",
                Stage = "",
                ActionName = "Review Worker – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.13,
                Number = "",
                SubNumber = "5.1.3",
                Stage = "",
                ActionName = "Approval – HR Head"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.14,
                Number = "",
                SubNumber = "5.1.4",
                Stage = "",
                ActionName = "Approval – ED"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.15,
                Number = "",
                SubNumber = "5.1.5",
                Stage = "",
                ActionName = "Confirm Salary – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 5.16,
                Number = "",
                SubNumber = "5.1.6",
                Stage = "",
                ActionName = "Worker Salary Acceptance – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.0,
                Number = "6",
                SubNumber = "",
                Stage = "Final Offer",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.11,
                Number = "",
                SubNumber = "6.1.1",
                Stage = "",
                ActionName = "Prepare Final Offer - Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.12,
                Number = "",
                SubNumber = "6.1.2",
                Stage = "",
                ActionName = "Approval – HR Head"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.13,
                Number = "",
                SubNumber = "6.1.3",
                Stage = "",
                ActionName = "Approval – ED"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 6.14,
                Number = "",
                SubNumber = "6.1.4",
                Stage = "",
                ActionName = "Final Offer Acceptance - Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 70,
                Number = "7",
                SubNumber = "",
                Stage = "Visa",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.1,
                Number = "7.1",
                SubNumber = "",
                Stage = "Overseas Business Visa",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.11,
                Number = "",
                SubNumber = "7.1.1",
                Stage = "",
                ActionName = "Submit Medical Report - Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.12,
                Number = "",
                SubNumber = "7.1.2",
                Stage = "",
                ActionName = "Check Medical Report & Inform PRO – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.13,
                Number = "",
                SubNumber = "7.1.3",
                Stage = "",
                ActionName = "Obtain Business Visa & Attach – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.14,
                Number = "",
                SubNumber = "7.1.4",
                Stage = "",
                ActionName = "Receive Visa Copy & Advise traveling date – Candidate"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.151,
                Number = "",
                SubNumber = "7.1.5.1",
                Stage = "",
                ActionName = "Confirm traveling date – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.152,
                Number = "",
                SubNumber = "7.1.5.2",
                Stage = "",
                ActionName = "Send email for ticket booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.153,
                Number = "",
                SubNumber = "7.1.5.3",
                Stage = "",
                ActionName = "Attach Ticket & Hotel quarantine booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.154,
                Number = "",
                SubNumber = "7.1.5.4",
                Stage = "",
                ActionName = "Confirm receipt of ticket & date of travel – Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.155,
                Number = "",
                SubNumber = "7.1.5.5",
                Stage = "",
                ActionName = "Arrange Accommodation – Admin"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.156,
                Number = "",
                SubNumber = "7.1.5.6",
                Stage = "",
                ActionName = "Arrange Airport Pickup – Plant"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.2,
                Number = "7.2",
                SubNumber = "",
                Stage = "Overseas Worker Visa",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.21,
                Number = "",
                SubNumber = "7.2.1",
                Stage = "",
                ActionName = "Apply work visa through MOL – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.22,
                Number = "",
                SubNumber = "7.2.2",
                Stage = "",
                ActionName = "Book QVC Appointment – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.23,
                Number = "",
                SubNumber = "7.2.3",
                Stage = "",
                ActionName = "Conduct Medical/Fingerprint – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.24,
                Number = "",
                SubNumber = "7.2.4",
                Stage = "",
                ActionName = "Attach visa copy if fit – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.25,
                Number = "",
                SubNumber = "7.2.5",
                Stage = "",
                ActionName = "Receive Visa Copy & Advise traveling date – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.251,
                Number = "",
                SubNumber = "7.2.5.1",
                Stage = "",
                ActionName = "Confirm traveling date – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.252,
                Number = "",
                SubNumber = "7.2.5.2",
                Stage = "",
                ActionName = "Send email for ticket booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.253,
                Number = "",
                SubNumber = "7.2.5.3",
                Stage = "",
                ActionName = "Attach Ticket & Hotel quarantine booking – Ticketing Team"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.254,
                Number = "",
                SubNumber = "7.2.5.4",
                Stage = "",
                ActionName = "Confirm receipt of ticket & date of travel – Candidate/Agency"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.255,
                Number = "",
                SubNumber = "7.2.5.5",
                Stage = "",
                ActionName = "Arrange Accommodation – Admin"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.256,
                Number = "",
                SubNumber = "7.2.5.6",
                Stage = "",
                ActionName = "Arrange Airport Pickup – Plant"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.3,
                Number = "7.3",
                SubNumber = "",
                Stage = "Local - Visa Transfer",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.31,
                Number = "",
                SubNumber = "7.3.1",
                Stage = "",
                ActionName = "Submit Visa transfer document – candidate"
            });

            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.32,
                Number = "",
                SubNumber = "7.3.2",
                Stage = "",
                ActionName = "Verify documents – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.33,
                Number = "",
                SubNumber = "7.3.3",
                Stage = "",
                ActionName = "Submit visa transfer & confirm sponsorship change – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.34,
                Number = "",
                SubNumber = "7.3.4",
                Stage = "",
                ActionName = "Verify visa transfer completed – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.35,
                Number = "",
                SubNumber = "7.3.5",
                Stage = "",
                ActionName = "Receive visa transfer confirmation & inform joining date - Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.4,
                Number = "7.4",
                SubNumber = "",
                Stage = "Local – Work Permit",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.41,
                Number = "",
                SubNumber = "7.4.1",
                Stage = "",
                ActionName = "Submit Work Permit documents – candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.42,
                Number = "",
                SubNumber = "7.4.2",
                Stage = "",
                ActionName = "Verify documents – Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.43,
                Number = "",
                SubNumber = "7.4.3",
                Stage = "",
                ActionName = "Obtain Work Permit & Attach – PRO"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.44,
                Number = "",
                SubNumber = "7.4.4",
                Stage = "",
                ActionName = "Verify Work Permit Obtained– Recruiter"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 7.45,
                Number = "",
                SubNumber = "7.4.5",
                Stage = "",
                ActionName = "Receive Work Permit & inform joining date - Candidate"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.0,
                Number = "8",
                SubNumber = "",
                Stage = "Candidate Joining",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.1,
                Number = "8.1",
                SubNumber = "",
                Stage = "Staff Joining",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.11,
                Number = "",
                SubNumber = "8.1.1",
                Stage = "",
                ActionName = "Fill Forms, Get signature from Candidate – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.12,
                Number = "",
                SubNumber = "8.1.2",
                Stage = "",
                ActionName = "Provide Cash Voucher – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.13,
                Number = "",
                SubNumber = "8.1.3",
                Stage = "",
                ActionName = "Send intimation email to Org Unit HOD copying IT – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.14,
                Number = "",
                SubNumber = "8.1.4",
                Stage = "",
                ActionName = "Send FRA/HRA request to Finance via email – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.15,
                Number = "",
                SubNumber = "8.1.5",
                Stage = "",
                ActionName = "Upload Passport and Visa/Qatar ID in system - HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.16,
                Number = "",
                SubNumber = "8.1.6",
                Stage = "",
                ActionName = "Update employee file in SAP & Timesheet – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.17,
                Number = "",
                SubNumber = "8.1.7",
                Stage = "",
                ActionName = "Confirm Induction - HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.18,
                Number = "",
                SubNumber = "8.1.8",
                Stage = "",
                ActionName = "Confirm/Reject/Extend Probation Date - HM"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.19,
                Number = "",
                SubNumber = "8.1.9",
                Stage = "",
                ActionName = "Confirm Probation Date - HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.2,
                Number = "8.2",
                SubNumber = "",
                Stage = "Worker Joining",
                ActionName = ""
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.21,
                Number = "",
                SubNumber = "8.2.1",
                Stage = "",
                ActionName = "Fill Forms, Get signature from Candidate – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.22,
                Number = "",
                SubNumber = "8.2.2",
                Stage = "",
                ActionName = "Provide Cash Voucher – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.23,
                Number = "",
                SubNumber = "8.2.3",
                Stage = "",
                ActionName = "Upload Passport and Visa/Qatar ID in system - HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.24,
                Number = "",
                SubNumber = "8.2.4",
                Stage = "",
                ActionName = "Update employee file in SAP & Timesheet  – HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.25,
                Number = "",
                SubNumber = "8.2.5",
                Stage = "",
                ActionName = "Conduct Induction - HR"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.26,
                Number = "",
                SubNumber = "8.2.6",
                Stage = "",
                ActionName = "Confirm/Reject/Extend Probation Date - HM"
            });
            list.Add(new ApplicationStateTrackDetailViewModel
            {
                UniqueNumber = 8.27,
                Number = "",
                SubNumber = "8.2.7",
                Stage = "",
                ActionName = "Confirm Probation Date - HR"
            });
            return list;
        }
        public async Task<ApplicationViewModel> GetConfidentialAgreementDetails(string applicationId)
        {
            string query = @$"select concat(a.""FirstName"",' ',a.""MiddleName"",' ',a.""LastName"") as FullName, a.""PassportNumber"", a.""QatarId"", t.""Name"" as TitleName from rec.""Application"" as a
                           left join rec.""ListOfValue"" as t on t.""Id"" = a.""TitleId""
                           where a.""Id""='{applicationId}'";
            var list = await _queryRepoId.ExecuteQuerySingle(query, null);
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetIdNameHMOrganization(string userId)
        {
            string query = @$"Select org.""Id"" as Id, org.""Name"" as Name
                                from rec.""HiringManager"" as hm
                            Left Join rec.""HiringManagerOrganization"" as hmo ON hmo.""HiringManagerId""=hm.""Id""
                            Left Join cms.""Organization"" as org ON org.""Id""=hmo.""OrganizationId""
                            where hm.""UserId""='{userId}'";

            var list = await _queryRepoIdName.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<ApplicationViewModel> GetCompetenceMatrixDetails(string applicationId)
        {
            string query = @$"select a.*, p.""Name"" as PositionName, t.""Name"" as TitleName
                            from rec.""Application"" as a 
                            left join cms.""Job"" as p on p.""Id"" = a.""JobId""
                            left join rec.""ListOfValue"" as t on t.""Id"" = a.""TitleId""
                            where a.""Id""='{applicationId}'";
            var list = await _queryRepoId.ExecuteQuerySingle(query, null);
            return list;
        }

        public async Task<IList<ApplicationViewModel>> GetWorkerPoolBatchData(string batchid)
        {
            try
            {
                var query = "";

                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName, job.""Name"" as JobName, org.""Name"" as OrganizationName,
app.""LastName"" as LastName, app.""Age"" as Age, app.""ApplicationNo"" as ApplicationNo, bt.""Name"" as BatchName,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountryId,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
apps.""Id"" as ApplicationState,
app.""Score"" as Score,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
bs.""Code"" as BatchStatusCode,bs.""Id"" as BatchId,app.""WorkerBatchId"" as WorkerBatchId

FROM rec.""Application"" as app
left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""Job"" as job on job.""Id"" = app.""JobId""
left join cms.""Organization"" as org on org.""Id""=app.""OrganizationId""
left join rec.""Batch"" as bt on bt.""Id""= app.""WorkerBatchId""
left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""ApplicationDrivingLicense"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
left join rec.""ApplicationEducational"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperience"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperienceByCountry"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
left join rec.""ApplicationExperienceByJob"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
left join rec.""ApplicationExperienceBySector"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
left join (select appexpbyc.*
 from rec.""ApplicationExperienceByCountry"" as appexpbyc 

           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId""  and cou.""Code"" = 'Ind'
) as appexpbyc   on appexpbyc.""ApplicationId"" = app.""Id""
 left join (select appexpba.*, ct.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpba
 join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
 where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
and appexpboa.""Code"" = 'Abroad'
left join(select appexpbg.*, ctt.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpbg
 join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf'
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'

left join rec.""ApplicationComputerProficiency"" as appcpc on appcpc.""ApplicationId""=app.""Id""
left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationStateComment"" as appc on appc.""ApplicationId""=app.""Id""
left join rec.""Batch"" as b on b.""Id""=app.""BatchId""
left join rec.""ListOfValue"" as bs on bs.""Id"" = b.""BatchStatus"" and bs.""ListOfValueType"" = 'BatchStatus'
where app.""WorkerBatchId""='{batchid}' ";



                var allList = await _appqueryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IList<ApplicationViewModel>> GetBatchData(string batchid)
        {
            try
            {
                var query = "";

                query = @$"Select distinct 'JobApplication' as CandidateType,app.""Id"" as ApplicationId, app.""CandidateProfileId"" as CandidateProfileId, 
app.""FirstName"" as FirstName, app.""MiddleName"" as MiddleName, job.""Name"" as JobName, org.""Name"" as OrganizationName,
app.""LastName"" as LastName, app.""Age"" as Age, app.""ApplicationNo"" as ApplicationNo, bt.""Name"" as BatchName,
app.""BirthDate"" as BirthDate, app.""BirthPlace"" as BirthPlace,
n.""Name"" as Nationality, app.""BloodGroup"" as BloodGroup,
gen.""Name"" as Gender, maritalstatus.""Name"" as MaritalStatus,
app.""PassportNumber"" as PassportNumber, pic.""Id"" as PassportIssueCountryId,
app.""PassportExpiryDate"" as PassportExpiryDate, app.""CurrentAddressHouse"" as CurrentAddressHouse,
app.""CurrentAddressStreet"" as CurrentAddressStreet, app.""CurrentAddressCity"" as CurrentAddressCity,
app.""CurrentAddressState"" as CurrentAddressState, cac.""Name"" as CurrentAddressCountryName,
app.""PermanentAddressHouse"" as PermanentAddressHouse, app.""PermanentAddressStreet"" as PermanentAddressStreet,
app.""PermanentAddressCity"" as PermanentAddressCity, app.""PermanentAddressState"" as PermanentAddressState,
app.""TotalWorkExperience"" as TotalWorkExperience, pac.""Name"" as PermanentAddressCountryName,
app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome,
app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,
appStatus.""Code"" as ApplicationStatusCode,apps.""Name"" as ApplicationStateName, apps.""Code"" as ApplicationStateCode,
apps.""Id"" as ApplicationState,
app.""Score"" as Score,
app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances
,appexpboa.""NoOfYear"" as TotalOtherExperience,appexpbog.""NoOfYear"" as TotalGCCExperience,
appexpbyc.""NoOfYear"" as TotalIndianExperience,app.""SourceFrom"" as SourceFrom,cu.""Name"" as NetSalaryCurrency,
bs.""Code"" as BatchStatusCode,bt.""Id"" as BatchId,app.""WorkerBatchId"" as WorkerBatchId

FROM rec.""Application"" as app
left join cms.""Currency"" as cu on cu.""Id"" = app.""NetSalaryCurrency""
left join cms.""Job"" as job on job.""Id"" = app.""JobId""

left join cms.""Country"" as pac on pac.""Id"" = app.""PermanentAddressCountryId""
left join cms.""Country"" as cac on cac.""Id"" = app.""CurrentAddressCountryId""
left join cms.""Country"" as pic on pic.""Id"" = app.""PassportIssueCountryId""
left join rec.""ListOfValue"" as gen on gen.""Id""=app.""Gender"" and gen.""ListOfValueType""='LOV_GENDER'
left join rec.""ListOfValue"" as maritalstatus on maritalstatus.""Id""=app.""MaritalStatus"" and maritalstatus.""ListOfValueType""='LOV_MARITALSTATUS'
left join cms.""Nationality"" as n on n.""Id"" = app.""NationalityId""
left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId""
left join rec.""ApplicationState"" as apps on apps.""Id"" = app.""ApplicationState""
left join rec.""ApplicationStatus"" as appStatus on appStatus.""Id"" = app.""ApplicationStatus""
left join rec.""ApplicationDrivingLicense"" as adl on adl.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as dlc on dlc.""Id"" = adl.""CountryId""
left join rec.""ApplicationEducational"" as aed on aed.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperience"" as appexp on appexp.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationExperienceByCountry"" as appexpc on appexpc.""ApplicationId"" = app.""Id"" 
left join cms.""Country"" as aec on aec.""Id"" = appexpc.""CountryId""
left join rec.""ApplicationExperienceByJob"" as appexpj on appexpj.""ApplicationId"" = app.""Id"" 
left join cms.""Job"" as ej on ej.""Id"" = appexpj.""JobId""
left join rec.""ApplicationExperienceBySector"" as appexpsec on appexpsec.""ApplicationId"" = app.""Id"" 
left join (select appexpbyc.*
 from rec.""ApplicationExperienceByCountry"" as appexpbyc 

           inner join cms.""Country"" cou on cou.""Id"" = appexpbyc.""CountryId""  and cou.""Code"" = 'Ind'
) as appexpbyc   on appexpbyc.""ApplicationId"" = app.""Id""
 left join (select appexpba.*, ct.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpba
 join rec.""ListOfValue"" as ct on ct.""Id"" = appexpba.""OtherTypeId""
 where ct.""ListOfValueType"" = 'LOV_COUNTRY') as appexpboa on appexpboa.""ApplicationId"" = app.""Id""
and appexpboa.""Code"" = 'Abroad'
left join(select appexpbg.*, ctt.""Code""
 from rec.""ApplicationExperienceByOther"" as appexpbg
 join rec.""ListOfValue"" as ctt on ctt.""Id"" = appexpbg.""OtherTypeId""
 where ctt.""ListOfValueType"" = 'LOV_COUNTRY') as appexpbog on appexpbog.""ApplicationId"" = app.""Id""
and appexpbog.""Code"" = 'Gulf'
 left join (select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpe on appcpe.""ApplicationId""=app.""Id""
and appcpe.""Code"" = 'English'
left join(select cp.*, lv.""Code"",pl.""Code"" as cpl 
 from rec.""ApplicationLanguageProficiency"" as cp
 join rec.""ListOfValue"" as lv on lv.""Id"" = cp.""Language""
 join rec.""ListOfValue"" as pl on pl.""Id"" = cp.""ProficiencyLevel""
 where pl.""ListOfValueType"" = 'LOV_PROFICIENCYLEVEL' and lv.""ListOfValueType"" = 'LOV_LANGUAGE') as appcpa on appcpa.""ApplicationId"" = app.""Id""
and appcpa.""Code"" = 'Arabic'

left join rec.""ApplicationComputerProficiency"" as appcpc on appcpc.""ApplicationId""=app.""Id""
left join rec.""ListOfValue"" as cplv on cplv.""Id"" = appcpc.""ProficiencyLevel""
left join rec.""ApplicationReferences"" as appr on appr.""ApplicationId"" = app.""Id"" 
left join rec.""ApplicationeExperienceByNature"" as appebn on appebn.""ApplicationId"" = app.""Id"" 


left join rec.""Batch"" as bt on bt.""Id""= app.""BatchId""
left join cms.""Organization"" as org on org.""Id"" = bt.""OrganizationId""

left join rec.""ListOfValue"" as bs on bs.""Id"" = bt.""BatchStatus"" and bs.""ListOfValueType"" = 'BatchStatus'
where app.""BatchId""='{batchid}' ";



                var allList = await _appqueryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                return allList;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> CreateStaffCandidateAndApplication(StaffCandidateSubmitViewModel model)
        {
            if (model.Created.IsNotNull())
            {
                foreach (var can in model.Created)
                {
                    if (can.PassportNumber.IsNotNull() && can.Nationality.IsNotNull())
                    {
                        var nation = can.Nationality.Split("_")[^1];
                        var data = await _candidateProfileBusiness.GetList(x => x.PassportNumber == can.PassportNumber && x.NationalityId == nation
                        && x.FirstName == can.CandidateName && x.Email == can.EmailId);
                        if (data.Count > 0)
                        {
                            //Do nothing
                            return false;
                        }
                        else
                        {
                            var candidate = new CandidateProfileViewModel
                            {
                                FirstName = can.CandidateName,
                                PassportNumber = can.PassportNumber,
                                NationalityId = nation,
                                ContactPhoneHome = can.ContactNumber,
                                Email = can.EmailId,
                                TotalWorkExperience = can.TotalExperience.IsNotNullAndNotEmpty() ? Convert.ToDouble(can.TotalExperience) : 0,
                                NetSalary = can.PresentSalary,
                                NoticePeriod = can.NoticePeriod,
                                CurrentAddress = can.CurrentLocation,
                                SourceFrom = SourceTypeEnum.Agency.ToString(),
                                ExpectedSalary = can.ExpectedSalary,
                                AgencyId = _userContext.UserId,
                            };

                            var jobDetails = await _jobAdvBusiness.GetSingleById(can.JobAdvertisement.Split("_")[^1]);
                            //var orgId = "";
                            //if (jobDetails.IsNotNull())
                            //{
                            //    orgId = jobDetails.OrganizationId;
                            //}

                            if (can.JobAdvertisement.IsNotNull())
                            {
                                candidate.JobAdvertisement = jobDetails.JobId;

                            }
                            if (can.ResumeLink.IsNotNull() && can.ResumeLink.Contains("_"))
                            {
                                var resume = can.ResumeLink.Split("_");
                                candidate.ResumeId = resume[^1];
                            }

                            if (can.PassportLink.IsNotNull() && can.PassportLink.Contains("_"))
                            {
                                var resume = can.PassportLink.Split("_");
                                candidate.PassportAttachmentId = resume[^1];
                            }

                            if (can.DocumentLink.IsNotNull() && can.DocumentLink.Contains("_"))
                            {
                                var resume = can.DocumentLink.Split("_");
                                candidate.OtherCertificateId = resume[^1];
                            }


                            var res = await _candidateProfileBusiness.CreateCandidate(candidate);


                            if (res.IsSuccess)
                            {

                                //Create Education
                                var edu = "";
                                if (can.EducationLink.IsNotNull() && can.EducationLink.Contains("_"))
                                {
                                    edu = can.EducationLink.Split("_")[^1];
                                }

                                var qal = new CandidateEducationalViewModel
                                {
                                    CandidateProfileId = res.Item.Id,
                                    OtherQualification = can.Qualification,
                                    AttachmentId = edu
                                };
                                if (!qal.OtherQualification.IsNullOrEmpty())
                                {
                                    var canql = await _candidateEducationalBusiness.Create(qal);
                                }

                                //create experience
                                var candidateExperience = new CandidateExperienceViewModel
                                {
                                    CandidateProfileId = res.Item.Id,
                                    Employer = can.PresentEmployer,
                                    JobTitle = can.Designation,
                                    AttachmentId = can.ExperienceLetterLink.IsNotNull() ? can.ExperienceLetterLink.Split("_")[^1] : ""
                                };
                                if (candidateExperience.IsNotNull())
                                {
                                    var exp = await _candidateExperienceBusiness.Create(candidateExperience);
                                }



                                //create application
                                var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == "UnReviewed");//unreviewed
                                                                                                                                         //var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == "");//
                                var appViewModel = new ApplicationViewModel()
                                {
                                    CandidateProfileId = res.Item.Id,
                                    FirstName = can.CandidateName,
                                    PassportNumber = can.PassportNumber,
                                    NationalityId = nation,
                                    ContactPhoneHome = can.ContactNumber,
                                    Email = can.EmailId,
                                    TotalWorkExperience = can.TotalExperience.IsNotNullAndNotEmpty() ? Convert.ToDouble(can.TotalExperience) : 0,
                                    NetSalary = can.PresentSalary,
                                    NoticePeriod = can.NoticePeriod,
                                    CurrentAddress = can.CurrentLocation,
                                    JobId = jobDetails.JobId,
                                    SourceFrom = SourceTypeEnum.Agency.ToString(),
                                    ExpectedSalary = can.ExpectedSalary,
                                    AgencyId = _userContext.UserId,
                                    AppliedDate = can.CVSendDate.IsNotNull() ? DateTime.Parse(can.CVSendDate) : DateTime.Now,
                                    ApplicationState = state1.IsNotNull() ? state1.Id : "",
                                    //OrganizationId = orgId,
                                };


                                if (can.ResumeLink.IsNotNull() && can.ResumeLink.Contains("_"))
                                {
                                    var resume = can.ResumeLink.Split("_");
                                    appViewModel.ResumeId = resume[^1];
                                }

                                if (can.PassportLink.IsNotNull() && can.PassportLink.Contains("_"))
                                {
                                    var resume = can.PassportLink.Split("_");
                                    appViewModel.PassportAttachmentId = resume[^1];
                                }

                                if (can.DocumentLink.IsNotNull() && can.DocumentLink.Contains("_"))
                                {
                                    var resume = can.DocumentLink.Split("_");
                                    appViewModel.OtherCertificateId = resume[^1];
                                }


                                var app = await Create(appViewModel);
                                if (app.IsNotNull())
                                {
                                    var userViewModel = new UserViewModel();
                                    var random = new Random();
                                    var Pass = Convert.ToString(random.Next(10000000, 99999999));
                                    if (appViewModel.Email.IsNotNull())
                                    {
                                        userViewModel.Email = appViewModel.Email;
                                        userViewModel.Name = candidate.FirstName;
                                        userViewModel.CreatedBy = _userContext.UserId;
                                        userViewModel.CreatedDate = DateTime.Now;
                                        userViewModel.Status = StatusEnum.Active;
                                        userViewModel.Password = Pass;
                                        userViewModel.PortalName = "CareerPortal";
                                        userViewModel.UserType = UserTypeEnum.CANDIDATE;
                                        userViewModel.UserName = candidate.FirstName;
                                        userViewModel.ConfirmPassword = Pass;
                                        userViewModel.SendWelcomeEmail = true;
                                    }
                                    var userResult = await _userBusiness.Create(userViewModel);

                                    if (userResult.IsNotNull())
                                    {
                                        res.Item.UserId = userResult.Item.Id;
                                        var r = await _candidateProfileBusiness.EditCandidate(res.Item);
                                    }


                                    //Create Education
                                    var qalApp = new ApplicationEducationalViewModel
                                    {
                                        ApplicationId = app.Item.Id,
                                        OtherQualification = can.Qualification,
                                        AttachmentId = edu
                                    };
                                    if (!qalApp.OtherQualification.IsNullOrEmpty())
                                    {
                                        var canql = await _applicationEducationalBusiness.Create(qalApp);
                                    }

                                    //create experience
                                    var candidateExperienceApp = new ApplicationExperienceViewModel
                                    {
                                        ApplicationId = app.Item.Id,
                                        Employer = can.PresentEmployer,
                                        JobTitle = can.Designation,
                                        AttachmentId = can.ExperienceLetterLink.IsNotNull() ? can.ExperienceLetterLink.Split("_")[^1] : ""
                                    };
                                    if (candidateExperienceApp.IsNotNull())
                                    {
                                        var exp = await _applicationExperienceBusiness.Create(candidateExperienceApp);
                                    }
                                }

                            }

                        }
                    }
                }

            }
            if (model.Updated.IsNotNull())
            {
                foreach (var candidate in model.Updated)
                {
                    if (candidate.PassportNumber.IsNotNull())
                    {
                        var data = await _candidateProfileBusiness.GetSingle(x => x.PassportNumber
                        == candidate.PassportNumber);
                        if (data.IsNotNull())
                        {
                            try
                            {
                                data.Id = data.Id;
                                data.FirstName = candidate.CandidateName;
                                data.PassportNumber = candidate.PassportNumber;
                                data.ContactPhoneHome = candidate.ContactNumber;
                                data.Email = candidate.EmailId;
                                data.TotalWorkExperience = candidate.TotalExperience.IsNotNullAndNotEmpty() ? Convert.ToDouble(candidate.TotalExperience) : 0;
                                data.NetSalary = candidate.PresentSalary;
                                data.NoticePeriod = candidate.NoticePeriod;
                                data.CurrentAddress = candidate.CurrentLocation;
                                data.ResumeAttachmentName = candidate.ResumeLink;
                                data.ExpectedSalary = candidate.ExpectedSalary;
                                if (candidate.Nationality.IsNotNull())
                                {
                                    data.NationalityId = candidate.Nationality.Split("_")[^1];
                                }

                                var jobDetails = await _jobAdvBusiness.GetSingleById(candidate.JobAdvertisement.Split("_")[^1]);
                                //var orgId = "";
                                //if (jobDetails.IsNotNull())
                                //{
                                //    orgId = jobDetails.OrganizationId;
                                //}
                                if (candidate.JobAdvertisement.IsNotNull())
                                {
                                    data.JobAdvertisement = jobDetails.JobId;

                                }
                                if (candidate.ResumeLink.IsNotNull() && candidate.ResumeLink.Contains("_"))
                                {
                                    var resume = candidate.ResumeLink.Split("_");
                                    data.ResumeId = resume[^1];
                                }
                                if (candidate.PassportLink.IsNotNull() && candidate.PassportLink.Contains("_"))
                                {
                                    var resume = candidate.PassportLink.Split("_");
                                    data.PassportAttachmentId = resume[^1];
                                }

                                if (candidate.DocumentLink.IsNotNull() && candidate.DocumentLink.Contains("_"))
                                {
                                    var resume = candidate.DocumentLink.Split("_");
                                    data.OtherCertificateId = resume[^1];
                                }


                                var res = await _candidateProfileBusiness.EditCandidate(data);

                                if (res.IsNotNull())
                                {
                                    await CandidateEducation(candidate, data, res);

                                    await CandidateExperience(candidate, data, res);


                                    var appDetails = await GetSingle(x => x.CandidateProfileId == data.Id &&
                                    x.JobId == data.JobAdvertisement);

                                    if (appDetails.IsNotNull() && appDetails.ApplicationStateCode == "UnReviewed")
                                    {

                                        //updates application

                                        appDetails.CandidateProfileId = data.Id;
                                        appDetails.FirstName = candidate.CandidateName;
                                        appDetails.PassportNumber = candidate.PassportNumber;
                                        appDetails.NationalityId = data.NationalityId;
                                        appDetails.ContactPhoneHome = candidate.ContactNumber;
                                        appDetails.Email = candidate.EmailId;
                                        appDetails.TotalWorkExperience = candidate.TotalExperience.IsNotNullAndNotEmpty() ? Convert.ToDouble(candidate.TotalExperience) : 0;
                                        appDetails.NoticePeriod = candidate.NoticePeriod;
                                        appDetails.CurrentAddress = candidate.CurrentLocation;
                                        appDetails.JobId = jobDetails.JobId;
                                        //appDetails.OrganizationId = orgId;
                                        if (candidate.ResumeLink.IsNotNull() && candidate.ResumeLink.Contains("_"))
                                        {
                                            var resume = candidate.ResumeLink.Split("_");
                                            appDetails.ResumeId = resume[^1];
                                        }

                                        if (candidate.PassportLink.IsNotNull() && candidate.PassportLink.Contains("_"))
                                        {
                                            var resume = candidate.PassportLink.Split("_");
                                            appDetails.PassportAttachmentId = resume[^1];
                                        }

                                        if (candidate.DocumentLink.IsNotNull() && candidate.DocumentLink.Contains("_"))
                                        {
                                            var resume = candidate.DocumentLink.Split("_");
                                            appDetails.OtherCertificateId = resume[^1];
                                        }


                                        //if (data.JobAdvertisementId.IsNotNull())
                                        //{
                                        //    var ap = await _jobAdvertisementBusiness.GetSingle(x => x.Id == data.JobAdvertisementId);
                                        //    if (ap.IsNotNull())
                                        //    {
                                        //        appDetails.JobId = ap.JobId;
                                        //    }
                                        //}


                                        var app = await Edit(appDetails);

                                        if (app.IsNotNull())
                                        {
                                            await AppEducation(candidate, app);

                                            await AppExperience(candidate, res, app);

                                            //Update Education
                                            var qalApp = await _applicationEducationalBusiness.GetSingle(x => x.ApplicationId == appDetails.Id);

                                            if (qalApp.IsNotNull())
                                            {
                                                qalApp.ApplicationId = app.Item.Id;
                                                qalApp.OtherQualification = candidate.Qualification;
                                                var canql = await _applicationEducationalBusiness.Edit(qalApp);
                                            }

                                            //Update experience
                                            var candidateExperienceApp = await _applicationExperienceBusiness.GetSingle(x => x.ApplicationId == appDetails.Id);

                                            if (candidateExperienceApp.IsNotNull())
                                            {
                                                candidateExperienceApp.ApplicationId = app.Item.Id;
                                                candidateExperienceApp.Employer = candidate.PresentEmployer;
                                                candidateExperienceApp.JobTitle = candidate.Designation;
                                                var exp = await _applicationExperienceBusiness.Edit(candidateExperienceApp);
                                            }
                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {

                            //Do nothing
                        }
                    }
                }

            }
            return true;
        }
        public async Task<bool> CreateWorkerCandidateAndApplication(WorkerCandidateSubmitViewModel model)
        {
            if (model.Created.IsNotNull())
            {
                foreach (var can in model.Created)
                {
                    if (can.PassportNumber.IsNotNull())
                    {
                        var data = await _candidateProfileBusiness.GetList(x => x.PassportNumber == can.PassportNumber && x.FirstName == can.CandidateName);
                        if (data.Count > 0)
                        {
                            //Do nothing
                            return false;
                        }
                        else
                        {
                            var candidate = new CandidateProfileViewModel
                            {
                                FirstName = can.CandidateName,
                                ContactPhoneHome = can.Mobile,
                                PassportNumber = can.PassportNumber,
                                Remarks = can.Remarks,
                                NetSalary = can.Salary_QAR,
                                TotalWorkExperience = Convert.ToDouble(can.TotalWorkExperience),
                                SourceFrom = SourceTypeEnum.Agency.ToString(),
                                AgencyId = _userContext.UserId,

                            };
                            var jobDetails = await _jobAdvBusiness.GetSingleById(can.Position.Split("_")[^1]);
                            //var orgId = "";
                            //if (jobDetails.IsNotNull())
                            //{
                            //    orgId = jobDetails.OrganizationId;
                            //}

                            if (can.Age.IsNotNull())
                            {
                                candidate.Age = Convert.ToInt32(can.Age);
                            }

                            if (can.Position.IsNotNull())
                            {
                                candidate.JobAdvertisement = jobDetails.JobId;

                            }

                            if (can.PassportStatus.IsNotNull())
                            {
                                candidate.PassportStatusId = can.PassportStatus.Split("_")[^1];

                            }

                            if (can.DOB.IsNotNull())
                            {
                                candidate.BirthDate = Convert.ToDateTime(can.DOB);

                            }
                            if (can.PassportExpiry.IsNotNull())
                            {
                                candidate.PassportExpiryDate = Convert.ToDateTime(can.PassportExpiry);

                            }
                            if (can.PassportCountry.IsNotNull())
                            {
                                candidate.PassportIssueCountryId = can.PassportCountry.Split("_")[^1];
                            }


                            if (can.DocumentLink.IsNotNull() && can.DocumentLink.Contains("_"))
                            {
                                var resume = can.DocumentLink.Split("_");
                                candidate.OtherCertificateId = resume[^1];
                            }

                            //if (can.PassportLink.IsNotNull() && can.PassportLink.Contains("_"))
                            //{
                            //    var resume = can.PassportLink.Split("_");
                            //    candidate.PassportAttachmentId = resume[^1];
                            //}



                            var res = await _candidateProfileBusiness.CreateCandidate(candidate);


                            if (res.IsNotNull())
                            {
                                var countryIndia = string.Empty;

                                var countryListData = await _masterBusiness.GetIdNameList("Country");
                                if (countryListData.IsNotNull())
                                {
                                    var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
                                    if (selectedCountry.IsNotNull())
                                    {
                                        countryIndia = selectedCountry.Id;
                                    }

                                }
                                var candidateExpCountry = await _candidateExperienceByCountryBusiness.Create(new CandidateExperienceByCountryViewModel
                                {
                                    SequenceOrder = 1,
                                    CountryId = countryIndia,
                                    CandidateProfileId = res.Item.Id.ToString(),
                                    NoOfYear = Convert.ToDouble(can.WorkExperienceIndia)
                                });

                                var othetExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
                                if (othetExp.IsNotNull())
                                {
                                    var candidateExpOther = await _candidateExperienceByOtherBusiness.Create(new CandidateExperienceByOtherViewModel
                                    {
                                        CompanyId = othetExp.CompanyId,
                                        NoOfYear = Convert.ToDouble(can.WorkExperienceAbroad),
                                        CandidateProfileId = res.Item.Id.ToString(),
                                        SequenceOrder = 1,
                                    });
                                }

                                var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == "UnReviewed");//unreviewed


                                //create application
                                var appViewModel = new ApplicationViewModel()
                                {
                                    CandidateProfileId = res.Item.Id,
                                    FirstName = can.CandidateName,
                                    BirthDate = candidate.BirthDate,
                                    ContactPhoneHome = can.Mobile,
                                    PassportExpiryDate = candidate.PassportExpiryDate,
                                    PassportNumber = can.PassportNumber,
                                    Remarks = can.Remarks,
                                    NetSalary = can.Salary_QAR,
                                    WorkExperience = Convert.ToDouble(can.TotalWorkExperience),
                                    TotalOtherExperience = Convert.ToDouble(can.WorkExperienceAbroad),
                                    TotalWorkExperience = Convert.ToDouble(can.WorkExperienceIndia),
                                    SourceFrom = SourceTypeEnum.Agency.ToString(),
                                    JobId = jobDetails.JobId,
                                    PassportIssueCountryId = candidate.PassportIssueCountryId,
                                    AgencyId = _userContext.UserId,
                                    Age = candidate.Age,
                                    ApplicationState = state1.IsNotNull() ? state1.Id : "",
                                    //OrganizationId = orgId,
                                };

                                if (can.PassportStatus.IsNotNull())
                                {
                                    appViewModel.PassportStatusId = can.PassportStatus.Split("_")[^1];
                                }

                                if (can.DocumentLink.IsNotNull() && can.DocumentLink.Contains("_"))
                                {
                                    var resume = can.DocumentLink.Split("_");
                                    appViewModel.OtherCertificateId = resume[^1];
                                }

                                var app = await Create(appViewModel);

                                if (app.IsNotNull())
                                {
                                    if (can.CriteriaSkillsData.IsNotNullAndNotEmpty())
                                    {
                                        var skilldata = can.CriteriaSkillsData.Split("||");
                                        res.Item.JobAdvertisementId = can.Position.Split("_")[^1];
                                        res.Item.Criterias = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(skilldata[0]);
                                        res.Item.Skills = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(skilldata[1]);
                                        res.Item.OtherInformations = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(skilldata[2]);
                                        //var criteria = await UpdateApplication(res.Item);
                                        if (res.Item.Criterias.IsNotNull() && res.Item.Criterias.Count > 0)
                                        {
                                            foreach (var criteria in res.Item.Criterias)
                                            {
                                                criteria.ApplicationId = app.Item.Id;
                                                criteria.Id = "";
                                                var criteriaResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(criteria);
                                            }
                                        }
                                        // Copy Candidate skill
                                        if (res.Item.Skills.IsNotNull() && res.Item.Skills.Count > 0)
                                        {
                                            foreach (var skill in res.Item.Skills)
                                            {
                                                skill.ApplicationId = app.Item.Id;
                                                skill.Id = "";
                                                var skillResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(skill);
                                            }
                                        }
                                        // Copy Candidate otherInfo
                                        if (res.Item.OtherInformations.IsNotNull() && res.Item.OtherInformations.Count > 0)
                                        {
                                            foreach (var otherInformation in res.Item.OtherInformations)
                                            {
                                                otherInformation.ApplicationId = app.Item.Id;
                                                otherInformation.Id = "";
                                                var otherInformationResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(otherInformation);
                                            }
                                        }
                                    }

                                    var countryList = await _masterBusiness.GetIdNameList("Country");
                                    if (countryList.IsNotNull())
                                    {
                                        var selectedCountry = countryList.Where(x => x.Code == "India").FirstOrDefault();
                                        if (selectedCountry.IsNotNull())
                                        {
                                            countryIndia = selectedCountry.Id;
                                        }

                                    }
                                    try
                                    {
                                        var m = new ApplicationExperienceByCountryViewModel
                                        {
                                            CountryId = countryIndia,
                                            ApplicationId = app.Item.Id.ToString(),
                                            NoOfYear = Convert.ToDouble(can.WorkExperienceIndia)
                                        };
                                        var appExpCountry = await _applicationExperienceByCountryBusiness.Create(m);
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                    var appExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
                                    if (appExp.IsNotNull())
                                    {
                                        var candidateExpOther = await _applicationExperienceByOtherBusiness.Create(new ApplicationExperienceByOtherViewModel
                                        {
                                            CompanyId = othetExp.CompanyId,
                                            NoOfYear = Convert.ToDouble(can.WorkExperienceAbroad),
                                            ApplicationId = app.Item.Id,
                                        });
                                    }


                                }


                            }

                        }
                    }
                }

            }
            if (model.Updated.IsNotNull())
            {
                foreach (var candidate in model.Updated)
                {
                    if (candidate.PassportNumber.IsNotNull())
                    {
                        var data = await _candidateProfileBusiness.GetSingle(x => x.PassportNumber == candidate.PassportNumber);
                        if (data.IsNotNull())
                        {
                            data.Id = data.Id;
                            data.FirstName = candidate.CandidateName;
                            data.ContactPhoneHome = candidate.Mobile;
                            data.PassportNumber = candidate.PassportNumber;
                            data.Remarks = candidate.Remarks;
                            data.NetSalary = candidate.Salary_QAR;
                            data.TotalWorkExperience = Convert.ToDouble(candidate.TotalWorkExperience);
                            data.SourceFrom = SourceTypeEnum.Agency.ToString();
                            data.AgencyId = _userContext.UserId;
                            data.Age = candidate.Age.IsNotNull() ? Convert.ToInt32(candidate.Age) : 0;

                            if (candidate.PassportStatus.IsNotNull())
                            {
                                data.PassportStatusId = candidate.PassportStatus.Split("_")[^1];
                            }

                            var jobDetails = await _jobAdvBusiness.GetSingleById(candidate.Position.Split("_")[^1]);
                            //var orgId = "";
                            //if (jobDetails.IsNotNull())
                            //{
                            //    orgId = jobDetails.OrganizationId;
                            //}
                            if (candidate.Position.IsNotNull())
                            {
                                data.JobAdvertisement = jobDetails.JobId;
                            }

                            if (candidate.DOB.IsNotNull())
                            {
                                data.BirthDate = Convert.ToDateTime(candidate.DOB);

                            }
                            if (candidate.PassportExpiry.IsNotNull())
                            {
                                data.PassportExpiryDate = Convert.ToDateTime(candidate.PassportExpiry);
                            }
                            if (candidate.PassportCountry.IsNotNull())
                            {
                                data.PassportIssueCountryId = candidate.PassportCountry.Split("_")[^1];
                            }
                            if (candidate.DocumentLink.IsNotNull() && candidate.DocumentLink.Contains("_"))
                            {
                                var resume = candidate.DocumentLink.Split("_");
                                data.OtherCertificateId = resume[^1];
                            }

                            var res = await _candidateProfileBusiness.EditCandidate(data);

                            if (res.IsNotNull())
                            {

                                var countryIndia = string.Empty;

                                var countryListData = await _masterBusiness.GetIdNameList("Country");
                                if (countryListData.IsNotNull())
                                {
                                    var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
                                    if (selectedCountry.IsNotNull())
                                    {
                                        countryIndia = selectedCountry.Id;
                                    }

                                }

                                countryIndia = await CandidateExperienceByCountry(candidate, res, countryIndia, countryListData);

                                ListOfValueViewModel othetExp = await CandidateOtherExperience(candidate, res);

                                //await CandidateEducation(candidate, data, res);

                                //await CandidateExperience(candidate, data, res);

                                var appDetails = await GetSingle(x => x.CandidateProfileId == data.Id &&
                                x.JobId == data.JobAdvertisement);

                                if (appDetails.IsNotNull() && appDetails.ApplicationStateCode == "UnReviewed")
                                {

                                    appDetails.CandidateProfileId = data.Id;
                                    appDetails.Id = appDetails.Id;
                                    appDetails.FirstName = candidate.CandidateName;
                                    appDetails.BirthDate = data.BirthDate;
                                    appDetails.ContactPhoneHome = candidate.Mobile;
                                    appDetails.PassportIssueCountryId = data.PassportIssueCountryId;
                                    appDetails.PassportExpiryDate = data.PassportExpiryDate;
                                    appDetails.PassportNumber = candidate.PassportNumber;
                                    appDetails.Remarks = candidate.Remarks;
                                    appDetails.NetSalary = candidate.Salary_QAR;
                                    appDetails.TotalOtherExperience = Convert.ToDouble(candidate.WorkExperienceAbroad);
                                    appDetails.TotalWorkExperience = Convert.ToDouble(candidate.WorkExperienceIndia);
                                    appDetails.SourceFrom = SourceTypeEnum.Agency.ToString();
                                    appDetails.JobId = jobDetails.JobId;
                                    appDetails.AgencyId = _userContext.UserId;
                                    appDetails.Age = candidate.Age.IsNotNull() ? Convert.ToInt32(candidate.Age) : 0;
                                    //appDetails.OrganizationId = orgId;
                                    if (candidate.PassportStatus.IsNotNull())
                                    {
                                        appDetails.PassportStatusId = candidate.PassportStatus.Split("_")[^1];
                                    }
                                    if (candidate.DocumentLink.IsNotNull() && candidate.DocumentLink.Contains("_"))
                                    {
                                        var resume = candidate.DocumentLink.Split("_");
                                        appDetails.OtherCertificateId = resume[^1];
                                    }
                                    var app = await Edit(appDetails);

                                    if (app.IsNotNull())
                                    {

                                        if (countryListData.IsNotNull())
                                        {
                                            var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
                                            if (selectedCountry.IsNotNull())
                                            {
                                                countryIndia = selectedCountry.Id;
                                            }

                                        }

                                        await AppExperienceByCountry(candidate, countryIndia, app);

                                        await OtherAppExperience(candidate, othetExp, app);
                                    }

                                }

                            }

                        }
                        else
                        {

                            //Do nothing
                        }
                    }
                }

            }
            return true;
        }


        private async Task CandidateEducation(StaffCandidateViewModel candidate, CandidateProfileViewModel data, CommandResult<CandidateProfileViewModel> res)
        {
            var candidateEducation = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == data.Id);
            if (candidateEducation.IsNotNull())
            {
                var resume = candidate.EducationLink.IsNotNull() ? candidate.EducationLink.Split("_")[^1] : "";
                candidateEducation.AttachmentId = resume;
                candidateEducation.OtherQualification = candidate.Qualification;
                var caEd = await _candidateEducationalBusiness.Edit(candidateEducation);
            }
            else
            {
                var edu = candidate.EducationLink.IsNotNull() ? candidate.EducationLink.Split("_")[^1] : "";
                var ab = await _candidateEducationalBusiness.Create(new CandidateEducationalViewModel
                {
                    CandidateProfileId = res.Item.Id,
                    OtherQualification = candidate.Qualification,
                    AttachmentId = edu
                });
            }
        }

        private async Task CandidateExperience(StaffCandidateViewModel candidate, CandidateProfileViewModel data, CommandResult<CandidateProfileViewModel> res)
        {
            var candidateExp = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == data.Id);
            if (candidateExp.IsNotNull())
            {
                var resume = candidate.ExperienceLetterLink.IsNotNull() ? candidate.ExperienceLetterLink.Split("_")[^1] : "";
                candidateExp.AttachmentId = resume;
                candidateExp.Employer = candidate.PresentEmployer;
                candidateExp.JobTitle = candidate.Designation;
                var caEd = await _candidateExperienceBusiness.Edit(candidateExp);
            }
            else
            {
                var edu = candidate.ExperienceLetterLink.IsNotNull() ? candidate.ExperienceLetterLink.Split("_")[^1] : "";
                var ab = await _candidateExperienceBusiness.Create(new CandidateExperienceViewModel
                {
                    CandidateProfileId = res.Item.Id,
                    AttachmentId = edu,
                    Employer = candidate.PresentEmployer,
                    JobTitle = candidate.Designation,
                });
            }
        }

        private async Task AppEducation(StaffCandidateViewModel candidate, CommandResult<ApplicationViewModel> app)
        {
            var appEdu = await _applicationEducationalBusiness.GetSingle(x => x.ApplicationId == app.Item.Id);
            if (appEdu.IsNotNull())
            {
                var resume = candidate.EducationLink.IsNotNull() ? candidate.EducationLink.Split("_")[^1] : "";
                appEdu.AttachmentId = resume;
                appEdu.OtherQualification = candidate.Qualification;

                var caEd = await _applicationEducationalBusiness.Edit(appEdu);
            }
            else
            {
                if (candidate.EducationLink.IsNotNull() && candidate.EducationLink.Contains("_"))
                {
                    var edu = candidate.EducationLink.IsNotNull() ? candidate.EducationLink.Split("_")[^1] : "";
                    var ab = await _applicationEducationalBusiness.Create(new ApplicationEducationalViewModel
                    {
                        ApplicationId = app.Item.Id,
                        AttachmentId = edu,
                        OtherQualification = candidate.Qualification,
                    });
                }
            }
        }

        private async Task AppExperience(StaffCandidateViewModel candidate, CommandResult<CandidateProfileViewModel> res, CommandResult<ApplicationViewModel> app)
        {
            var appExp = await _applicationExperienceBusiness.GetSingle(x => x.ApplicationId == app.Item.Id);
            if (appExp.IsNotNull())
            {
                var resume = candidate.ExperienceLetterLink.IsNotNull() ? candidate.ExperienceLetterLink.Split("_")[^1] : "";
                appExp.AttachmentId = resume;
                appExp.Employer = candidate.PresentEmployer;
                appExp.JobTitle = candidate.Designation;
                var caEd = await _applicationExperienceBusiness.Edit(appExp);
            }
            else
            {
                var edu = candidate.ExperienceLetterLink.IsNotNull() ? candidate.ExperienceLetterLink.Split("_")[^1] : "";
                var ab = await _applicationExperienceBusiness.Create(new ApplicationExperienceViewModel
                {
                    ApplicationId = res.Item.Id,
                    AttachmentId = edu,
                    Employer = candidate.PresentEmployer,
                    JobTitle = candidate.Designation,
                });
            }
        }
        private async Task<string> CandidateExperienceByCountry(WorkerCandidateViewModel candidate, CommandResult<CandidateProfileViewModel> res, string countryIndia, IList<IdNameViewModel> countryListData)
        {
            var candidateExpCountry = await _candidateExperienceByCountryBusiness.GetSingle(x => x.CandidateProfileId == res.Item.Id);
            if (candidateExpCountry.IsNotNull())
            {
                candidateExpCountry.SequenceOrder = 1;
                candidateExpCountry.CountryId = countryIndia;
                candidateExpCountry.CandidateProfileId = res.Item.Id.ToString();
                candidateExpCountry.NoOfYear = Convert.ToDouble(candidate.WorkExperienceIndia);
                var xs = await _candidateExperienceByCountryBusiness.Edit(candidateExpCountry);
            }
            else
            {
                if (countryListData.IsNotNull())
                {
                    var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
                    if (selectedCountry.IsNotNull())
                    {
                        countryIndia = selectedCountry.Id;
                    }

                }
                var xs = await _candidateExperienceByCountryBusiness.Create(new CandidateExperienceByCountryViewModel
                {
                    SequenceOrder = 1,
                    CountryId = countryIndia,
                    CandidateProfileId = res.Item.Id.ToString(),
                    NoOfYear = Convert.ToDouble(candidate.WorkExperienceIndia)
                });
            }

            return countryIndia;
        }

        private async Task<ListOfValueViewModel> CandidateOtherExperience(WorkerCandidateViewModel candidate, CommandResult<CandidateProfileViewModel> res)
        {
            var othetExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
            if (othetExp.IsNotNull())
            {
                var candidateExpOther = await _candidateExperienceByOtherBusiness.GetSingle(x => x.CandidateProfileId == res.Item.Id);
                if (candidateExpOther.IsNotNull())
                {
                    candidateExpOther.SequenceOrder = 1;
                    candidateExpOther.CompanyId = othetExp.CompanyId;
                    candidateExpOther.NoOfYear = Convert.ToDouble(candidate.WorkExperienceAbroad);
                    candidateExpOther.CandidateProfileId = res.Item.Id.ToString();
                    var xs = await _candidateExperienceByOtherBusiness.Edit(candidateExpOther);

                }
                else
                {
                    var cxp = await _candidateExperienceByOtherBusiness.Create(new CandidateExperienceByOtherViewModel
                    {
                        CompanyId = othetExp.CompanyId,
                        SequenceOrder = 1,
                        NoOfYear = Convert.ToDouble(candidate.WorkExperienceAbroad),
                        CandidateProfileId = res.Item.Id.ToString()
                    });
                }
            }

            return othetExp;
        }

        private async Task AppExperienceByCountry(WorkerCandidateViewModel candidate, string countryIndia, CommandResult<ApplicationViewModel> app)
        {
            var appExpCountry = await _applicationExperienceByCountryBusiness.GetSingle(x => x.ApplicationId == app.Item.Id);
            if (appExpCountry.IsNotNull())
            {
                appExpCountry.CountryId = countryIndia;
                appExpCountry.ApplicationId = app.Item.Id.ToString();
                appExpCountry.NoOfYear = Convert.ToDouble(candidate.WorkExperienceIndia);
                var appExpCountryx = await _applicationExperienceByCountryBusiness.Edit(appExpCountry);
            }
            else
            {
                var appExpCountryx = await _applicationExperienceByCountryBusiness.Create(new ApplicationExperienceByCountryViewModel
                {
                    CountryId = countryIndia,
                    ApplicationId = app.Item.Id.ToString(),
                    NoOfYear = Convert.ToDouble(candidate.WorkExperienceIndia)
                });
            }
        }

        private async Task OtherAppExperience(WorkerCandidateViewModel candidate, ListOfValueViewModel othetExp, CommandResult<ApplicationViewModel> app)
        {
            var othetAppExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
            if (othetAppExp.IsNotNull())
            {
                var appExpOther = await _applicationExperienceByOtherBusiness.GetSingle(x => x.ApplicationId == app.Item.Id);
                if (appExpOther.IsNotNull())
                {
                    appExpOther.CompanyId = othetExp.CompanyId;
                    appExpOther.NoOfYear = Convert.ToDouble(candidate.WorkExperienceAbroad);
                    appExpOther.ApplicationId = app.Item.Id.ToString();
                    var candidateExpOther = await _applicationExperienceByOtherBusiness.Edit(appExpOther);
                }
                else
                {
                    var candidateExpOther = await _applicationExperienceByOtherBusiness.Create(new ApplicationExperienceByOtherViewModel
                    {
                        CompanyId = othetExp.CompanyId,
                        NoOfYear = Convert.ToDouble(candidate.WorkExperienceAbroad),
                        ApplicationId = app.Item.Id,
                    });
                }
            }
        }

        public async Task<IdNameViewModel> GetHiringManagerByApplication(string applicationId)
        {

            string query = @$"SELECT q.""HiringManager"" as Id
                            FROM rec.""Application"" as c                           
                            LEFT JOIN rec.""Batch"" as q ON q.""Id"" = c.""BatchId""
                          
                            where c.""Id""='{applicationId}'
                            ";
            var list = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return list;
        }
        public async Task<IdNameViewModel> GetHeadOfDepartmentByApplication(string applicationId)
        {

            string query = @$"SELECT q.""HeadOfDepartment"" as Id
                            FROM rec.""Application"" as c                           
                            LEFT JOIN rec.""Batch"" as q ON q.""Id"" = c.""BatchId""
                          
                            where c.""Id""='{applicationId}'
                            ";
            var list = await _queryRepoIdName.ExecuteQuerySingle(query, null);
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetJobdescription(string JobId)
        {

            string query = @$"select jd.""Id"" as Id,concat(j.""Name"",'_',o.""Name"") as Name
from rec.""JobDescription"" as jd
left join cms.""Job"" as j on j.""Id"" = jd.""JobId""
left join cms.""Organization"" as o on o.""Id"" = jd.""OrganizationId""
where jd.""JobId"" = '{JobId}'
                            ";
            var list = await _queryRepoIdName.ExecuteQueryList(query, null);
            return list;
        }

        public async Task<JobDescriptionViewModel> GetJobdescriptionById(string Id)
        {

            string query = @$"select *
from rec.""JobDescription"" as jd
where jd.""Id"" = '{Id}'
                            ";
            var list = await _jobdescripqueryRepo.ExecuteQuerySingle(query, null);
            return list;
        }

        public async Task<IList<ApplicationViewModel>> GetCandidateReportData()
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
                            app.""Email"" as Email, app.""ContactPhoneHome"" as ContactPhoneHome, batch.""Name"" as BatchName,app.""OrganizationId"" as OrganizationId, org.""Name"" as OrganizationName, 
                            app.""ContactPhoneLocal"" as ContactPhoneLocal, app.""JobId"" as JobId,job.""Name"" as JobName,app.""GaecNo"" as GaecNo,
                            appStatus.""Code"" as ApplicationStatusCode, apps.""Code"" as ApplicationStateCode,apps.""Name"" as ApplicationStateName,
                            app.""Score"" as Score, app.""IsLocalCandidate"" as IsLocalCandidate,
                            app.""NetSalary"" as NetSalary, app.""OtherAllowances"" as OtherAllowances,curr.""Name"" as SalaryCurrencyName,
                            app.""SourceFrom"" as SourceFrom, app.""AgencyId"" as AgencyId,
                            c.""UserId"" as ApplicationUserId,app.""AppliedDate"" as AppliedDate,
                            app.""ApplicationState"" as ApplicationState,vt.""Code"" as VisaCategoryCode, appStatus.""Name"" as ApplicationStatus,
                            app.""ApplicationNo"" as ApplicationNo,au.""AgencyName"" as AgencyName
                           
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
                            left join rec.""Agency"" as au on  au.""Id"" = app.""AgencyId""
                             ";

                //var where = $@" where au.""Id""='{userId}' ";
               
                //query = query.Replace("#WHERE#", where);
                var allList = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
                return allList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
