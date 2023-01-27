using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Synergy.App.Business
{
    public class CareerPortalBusiness : BusinessBase<ServiceViewModel, NtsService>, ICareerPortalBusiness
    {
        private readonly IRecQueryBusiness _recQueryBusiness;
        private readonly ILOVBusiness _LOVBusiness;
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly IRepositoryBase<NoteViewModel, NtsNote> _querynoteRepo;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserBusiness _userBusiness;
        private IUserContext _userContext;
        public CareerPortalBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IMapper autoMapper,
            IRepositoryQueryBase<ServiceViewModel> queryRepo,
            IRecQueryBusiness recQueryBusiness,
            ILOVBusiness LOVBusiness,
            ICmsBusiness cmsBusiness,
            INoteBusiness noteBusiness,
            IUserContext userContext,
            IUserBusiness userBusiness,
            IRepositoryBase<NoteViewModel, NtsNote> querynoteRepo) : base(repo, autoMapper)
        {
            _recQueryBusiness = recQueryBusiness;
            _LOVBusiness = LOVBusiness;
            _queryRepo = queryRepo;
            _querynoteRepo = querynoteRepo;
            _cmsBusiness = cmsBusiness;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _userBusiness = userBusiness;

        }

        #region N
        public async Task<List<ListOfValueViewModel>> GetJobAdvertisementListWithCount(string agency)
        {
            var queryData = await _recQueryBusiness.GetJobAdvertisementListWithCount(agency);
            return queryData;
        }

        public async Task<List<LOVViewModel>> GetManpowerTypeListOfValue()
        {

            var data = await _LOVBusiness.GetList(x => x.LOVType == "REC_MANPOWER" && x.Status != StatusEnum.Inactive);
            return data;
        }





        public async Task<List<JobAdvertisementViewModel>> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerTypeId, string agency)
        {
            var result = await _recQueryBusiness.GetJobAdvertisementList(keyWord, categoryId, locationId, manpowerTypeId, agency);
            return result;
        }


        public async Task<List<LOVViewModel>> GetListOfValue(string type)
        {
            var data = await _LOVBusiness.GetList(x => x.LOVType == type && x.Status != StatusEnum.Inactive);
            return data;
        }

        public async Task<JobAdvertisementViewModel> GetNameById(string jobAdvId)
        {
            var queryData = await _recQueryBusiness.GetNameById(jobAdvId);
            return queryData;
        }

        public async Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string JobId)
        {
            var queryData = await _recQueryBusiness.GetJobIdNameListByJobAdvertisement(JobId);
            return queryData;
        }

        public async Task<Tuple<CandidateProfileViewModel, bool>> IsCandidateProfileFilled()
        {
            var res = await _recQueryBusiness.IsCandidateProfileFilled();
            return res;
        }

        public async Task<ApplicationViewModel> GetApplicationData(string Id, string jobAdvId)
        {
            var res = await _recQueryBusiness.GetApplicationData(Id, jobAdvId);
            return res;
        }

        public async Task<CandidateProfileViewModel> GetCandidateDataByUserId(string userId)
        {
            var res = await _recQueryBusiness.GetCandidateDataByUserId(userId);
            return res;
        }

        public async Task<List<ApplicationViewModel>> GetApplicationListByCandidateId(string candidateId)
        {
            var res = await _recQueryBusiness.GetApplicationListByCandidateId(candidateId);
            return res;
        }

        public async Task<CandidateProfileViewModel> GetApplicationDetails(string candidateProfileId, string jobAdvId)
        {
            var res = await _recQueryBusiness.GetApplicationDetails(candidateProfileId, jobAdvId);
            return res;
        }
        public async Task<CandidateProfileViewModel> GetApplicationDetailsUsingAppId(string applicationId, string jobAdvId)
        {
            var res = await _recQueryBusiness.GetApplicationDetailsUsingAppId(applicationId, jobAdvId);
            return res;
        }

        public async Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByApplicationIdAndType(string ApplicationId, string type)
        {
            var res = await _recQueryBusiness.GetApplicationJobCriteriaByApplicationIdAndType(ApplicationId, type);
            return res;
        }

        public async Task<CandidateProfileViewModel> GetCandProfileDetails(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetCandProfileDetails(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByJobAndType(string JobAdvertisementId, string type)
        {
            var res = await _recQueryBusiness.GetApplicationJobCriteriaByJobAndType(JobAdvertisementId, type);
            return res;
        }

        public async Task<List<ApplicationExperienceViewModel>> GetListByApplication(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetListByApplication(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationExperienceByCountryViewModel>> GetApplicationExpByCountryList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationExpByCountryList(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationExperienceByJobViewModel>> GetApplicationExpByJobList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationExpByJobList(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationeExperienceByNatureViewModel>> GetApplicationExpByNatureList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationExpByNatureList(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationExperienceBySectorViewModel>> GetApplicationListBySector(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationListBySector(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationProjectViewModel>> GetApplicationProjectList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationProjectList(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationEducationalViewModel>> GetApplicantsEducationInfoList(QualificationTypeEnum qualificationType, string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicantsEducationInfoList(qualificationType, candidateProfileId);
            return res;
        }

        public async Task<List<CandidateEducationalViewModel>> GetListByCandidate(QualificationTypeEnum qualificationType, string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetListByCandidate(qualificationType, candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationComputerProficiencyViewModel>> GetApplicationCompProfList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationCompProfList(candidateProfileId);
            return res;
        }

        public async Task<List<CandidateComputerProficiencyViewModel>> GetCandidateCompProfList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetCandidateCompProfList(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationLanguageProficiencyViewModel>> GetApplicationLangProfList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationLangProfList(candidateProfileId);
            return res;
        }

        public async Task<List<CandidateLanguageProficiencyViewModel>> GetCandidateLangProfList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetCandidateLangProfList(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationDrivingLicenseViewModel>> GetLicenseListByApplication(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetLicenseListByApplication(candidateProfileId);
            return res;
        }

        public async Task<List<CandidateDrivingLicenseViewModel>> GetLicenseListByCandidate(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetLicenseListByCandidate(candidateProfileId);
            return res;
        }

        public async Task<List<ApplicationReferencesViewModel>> GetApplicationRefList(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetApplicationRefList(candidateProfileId);
            return res;
        }

        public async Task<IdNameViewModel> GetNationalityIdByName()
        {
            var res = await _recQueryBusiness.GetNationalityIdByName();
            return res;
        }

        public async Task<CandidateProfileViewModel> GetCandidateByUser()
        {
            var result = await _recQueryBusiness.GetCandidateByUser();
            return result;
        }

        public async Task<CandidateProfileViewModel> GetCandidateByEmail()
        {
            var result = await _recQueryBusiness.GetCandidateByEmail();
            return result;
        }

       

        public async Task<CandidateProfileViewModel> GetCandidateById(string id)
        {
            var result = await _recQueryBusiness.GetCandidateById(id);
            return result;
        }

        public async Task<CandidateProfileViewModel> GetCandidateByPassportNo(string passportNo)
        {
            var res = await _recQueryBusiness.GetCandidateByPassportNo(passportNo);
            return res;
        }

        public async Task<CandidateProfileViewModel> CheckCandExitsByIdnPassportNo(string id, string passportNo)
        {
            var res = await _recQueryBusiness.CheckCandExitsByIdnPassportNo(id, passportNo);
            return res;
        }

        public async Task<CandidateExperienceViewModel> GetCandidateExperienceDuration(string candidateProfileId)
        {
            var exp = await _recQueryBusiness.GetCandidateExperienceDuration(candidateProfileId);
            return exp;
        }

        public async Task<ApplicationViewModel> GetApplicationById(string appId)
        {
            var queryData = await _recQueryBusiness.GetApplicationById(appId);
            return queryData;
        }

        public async Task UpdateApplicationExperienceWhenProfileUpdate(string profileId, string applicationId)
        {

            // Copy Candidate Experience
            //var appExp = await _repo.GetList<ApplicationExperienceViewModel, ApplicationExperience>(x => x.ApplicationId == applicationId);
            //foreach (var item in appExp)
            //{
            //    await _applicationExperienceBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationExperience", applicationId);

            //var candidateExp = await _repo.GetList<CandidateExperienceViewModel, CandidateExperience>(x => x.CandidateProfileId == profileId);
            var candidateExp = await _recQueryBusiness.GetCandidateExpByCandidateId(profileId);
            

            if (candidateExp != null && candidateExp.Count() > 0)
            {
                foreach (var exp in candidateExp)
                {
                    var appexp = _autoMapper.Map<CandidateExperienceViewModel, ApplicationExperienceViewModel>(exp);
                    appexp.ApplicationId = applicationId;
                    appexp.Id = "";
                    //var expresult = await base.Create<ApplicationExperienceViewModel, ApplicationExperience>(appexp);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(appexp);

                    var expresult = await _noteBusiness.ManageNote(notemodel);

                }
            }

            // Copy Candidate Experience By Country
            //var appPExperienceByCountry = await _repo.GetList<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>(x => x.ApplicationId == applicationId);
            //foreach (var item in appPExperienceByCountry)
            //{
            //    await _applicationExperienceByCountryBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationExperienceByCountry", applicationId);

            //var candidateexpByCountry = await _repo.GetList<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry>(x => x.CandidateProfileId == profileId);
            var candidateexpByCountry = await _recQueryBusiness.GetCandidateExpCountryByCandidateId(profileId);

            if (candidateexpByCountry != null && candidateexpByCountry.Count() > 0)
            {
                foreach (var expByountry in candidateexpByCountry)
                {
                    var countryexp = _autoMapper.Map<CandidateExperienceByCountryViewModel, ApplicationExperienceByCountryViewModel>(expByountry);
                    countryexp.ApplicationId = applicationId;
                    countryexp.Id = "";
                    //var countryexpresult = await base.Create<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>(countryexp);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_COUNTRY";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(countryexp);

                    var countryexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
            // Copy Candidate Experience By Sector
            //var appPExperienceBySector = await _repo.GetList<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>(x => x.ApplicationId == applicationId);
            //foreach (var item in appPExperienceBySector)
            //{
            //    await _applicationExperienceBySectorBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationExperienceBySector", applicationId);

            //var candidateexpBySector = await _repo.GetList<CandidateExperienceBySectorViewModel, CandidateExperienceBySector>(x => x.CandidateProfileId == profileId);
            var candidateexpBySector = await _recQueryBusiness.GetCandidateExpSectorByCandidateId(profileId);

            if (candidateexpBySector != null && candidateexpBySector.Count() > 0)
            {
                foreach (var expBySector in candidateexpBySector)
                {
                    var Sectorexp = _autoMapper.Map<CandidateExperienceBySectorViewModel, ApplicationExperienceBySectorViewModel>(expBySector);
                    Sectorexp.ApplicationId = applicationId;
                    Sectorexp.Id = "";
                    //var Sectorexpresult = await base.Create<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>(Sectorexp);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_SECTOR";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(Sectorexp);

                    var Sectorexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
            // Copy Candidate Experience By Nature
            //var appPExperienceByNature = await _repo.GetList<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>(x => x.ApplicationId == applicationId);
            //foreach (var item in appPExperienceByNature)
            //{
            //    await _applicationExperienceByNatureBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationExperienceByNature", applicationId);

            //var candidateExpByNature = await _repo.GetList<CandidateExperienceByNatureViewModel, CandidateExperienceByNature>(x => x.CandidateProfileId == profileId);
            var candidateExpByNature = await _recQueryBusiness.GetCandidateExpNatureByCandidateId(profileId);


            if (candidateExpByNature != null && candidateExpByNature.Count() > 0)
            {
                foreach (var expByNature in candidateExpByNature)
                {
                    var Natureexp = _autoMapper.Map<CandidateExperienceByNatureViewModel, ApplicationeExperienceByNatureViewModel>(expByNature);
                    Natureexp.ApplicationId = applicationId;
                    Natureexp.Id = "";
                    //var Natureexpresult = await base.Create<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>(Natureexp);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_NATURE";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(Natureexp);

                    var Natureexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
            // Copy Candidate Experience By Job
            //var appPExperienceByJob = await _repo.GetList<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>(x => x.ApplicationId == applicationId);
            //foreach (var item in appPExperienceByJob)
            //{
            //    await _applicationExperienceByJobBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationExperienceByJob", applicationId);

            //var candidateExpByJob = await _repo.GetList<CandidateExperienceByJobViewModel, CandidateExperienceByJob>(x => x.CandidateProfileId == profileId);
            var candidateExpByJob = await _recQueryBusiness.GetCandidateExpJobByCandidateId(profileId);

            if (candidateExpByJob != null && candidateExpByJob.Count() > 0)
            {
                foreach (var expByJob in candidateExpByJob)
                {
                    var Jobexp = _autoMapper.Map<CandidateExperienceByJobViewModel, ApplicationExperienceByJobViewModel>(expByJob);
                    Jobexp.ApplicationId = applicationId;
                    Jobexp.Id = "";
                    //var Natureexpresult = await base.Create<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>(Jobexp);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_JOB";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(Jobexp);

                    var Jobexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }

            // Copy Candidate Other
            //var appExperienceByOther = await _repo.GetList<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>(x => x.ApplicationId == applicationId);
            //foreach (var item in appExperienceByOther)
            //{
            //    await _applicationExperienceByOtherBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationExperienceByOther", applicationId);

            //var candidateOther = await _repo.GetList<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>(x => x.CandidateProfileId == profileId);
            var candidateOther = await _recQueryBusiness.GetCandidateExpOtherByCandidateId(profileId);


            if (candidateOther != null && candidateOther.Count() > 0)
            {
                foreach (var reference in candidateOther)
                {
                    var candidateref = _autoMapper.Map<CandidateExperienceByOtherViewModel, ApplicationExperienceByOtherViewModel>(reference);
                    candidateref.ApplicationId = applicationId;
                    candidateref.Id = "";
                    //var Natureexpresult = await base.Create<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>(candidateref);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_OTHER";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(candidateref);

                    var Otherexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
        }


        public async Task UpdateApplicationEducationWhenProfileUpdate(string profileId, string applicationId)
        {
            //var appEdu = await _repo.GetList<ApplicationEducationalViewModel, ApplicationEducational>(x => x.ApplicationId == applicationId);
            //foreach (var item in appEdu)
            //{
            //    await _applicationEducationalBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationEducational", applicationId);

            //var candidateEdu = await _repo.GetList<CandidateEducationalViewModel, CandidateEducational>(x => x.CandidateProfileId == profileId);
            var candidateEdu = await _recQueryBusiness.GetCandidateEduByCandidateId(profileId);

            if (candidateEdu != null && candidateEdu.Count() > 0)
            {
                foreach (var education in candidateEdu)
                {
                    var appEducation = _autoMapper.Map<CandidateEducationalViewModel, ApplicationEducationalViewModel>(education);
                    appEducation.ApplicationId = applicationId;
                    appEducation.Id = "";
                    //var eduresult = await base.Create<ApplicationEducationalViewModel, ApplicationEducational>(appEducation);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_EDUCATIONAL";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(appEducation);

                    var eduresult = await _noteBusiness.ManageNote(notemodel);

                }
            }

            // Copy Candidate Computer Proficiency
            //var appProficiency = await _repo.GetList<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>(x => x.ApplicationId == applicationId);
            //foreach (var item in appProficiency)
            //{
            //    await _applicationComputerProficiencyBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationComputerProficiency", applicationId);

            //var candidateCompProficiency = await _repo.GetList<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>(x => x.CandidateProfileId == profileId);
            var candidateCompProficiency = await _recQueryBusiness.GetCandidateCompProfByCandidateId(profileId);


            if (candidateCompProficiency != null && candidateCompProficiency.Count() > 0)
            {
                foreach (var Compexp in candidateCompProficiency)
                {
                    var Compexperience = _autoMapper.Map<CandidateComputerProficiencyViewModel, ApplicationComputerProficiencyViewModel>(Compexp);
                    Compexperience.ApplicationId = applicationId;
                    Compexperience.Id = "";
                    //var Compexpresult = await base.Create<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>(Compexperience);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_COMP_PROFICIENCY";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(Compexperience);

                    var Compexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
            // Copy Candidate Language Proficiency
            //var appLanguageProficiency = await _repo.GetList<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>(x => x.ApplicationId == applicationId);
            //foreach (var item in appLanguageProficiency)
            //{
            //    await _applicatioLanguageProficiencyBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationLanguageProficiency", applicationId);

            //var candidateLanguageProficiency = await _repo.GetList<CandidateLanguageProficiencyViewModel, CandidateLanguageProficiency>(x => x.CandidateProfileId == profileId);
            var candidateLanguageProficiency = await _recQueryBusiness.GetCandidateLangProfByCandidateId(profileId);


            if (candidateLanguageProficiency != null && candidateLanguageProficiency.Count() > 0)
            {
                foreach (var language in candidateLanguageProficiency)
                {
                    var lang = _autoMapper.Map<CandidateLanguageProficiencyViewModel, ApplicationLanguageProficiencyViewModel>(language);
                    lang.ApplicationId = applicationId;
                    lang.Id = "";
                    //var langresult = await base.Create<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>(lang);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_LANG_PROFICIENCY";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(lang);

                    var langresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
            // Copy Candidate Driving Liciense Detail
            //var appDrivingLicense = await _repo.GetList<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>(x => x.ApplicationId == applicationId);
            //foreach (var item in appDrivingLicense)
            //{
            //    await _applicationDrivingLicenseBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationDrivingLicense", applicationId);

            //var candidateDL = await _repo.GetList<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>(x => x.CandidateProfileId == profileId);
            var candidateDL = await _recQueryBusiness.GetCandidateDrivingLicenceByCandidateId(profileId);


            if (candidateDL != null && candidateDL.Count() > 0)
            {
                foreach (var dl in candidateDL)
                {
                    var DL = _autoMapper.Map<CandidateDrivingLicenseViewModel, ApplicationDrivingLicenseViewModel>(dl);
                    DL.ApplicationId = applicationId;
                    DL.Id = "";
                    //var Natureexpresult = await base.Create<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>(DL);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_DRIVING_LICENSE";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(DL);

                    var Natureexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
            // Copy Candidate Project
            //var appProject = await _repo.GetList<ApplicationProjectViewModel, ApplicationProject>(x => x.ApplicationId == applicationId);
            //foreach (var item in appProject)
            //{
            //    await _applicationProjectBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationProject", applicationId);

            //var candidateProject = await _repo.GetList<CandidateProjectViewModel, CandidateProject>(x => x.CandidateProfileId == profileId);
            var candidateProject = await _recQueryBusiness.GetCandidateProjectByCandidateId(profileId);


            if (candidateProject != null && candidateProject.Count() > 0)
            {
                foreach (var project in candidateProject)
                {
                    var candidateProj = _autoMapper.Map<CandidateProjectViewModel, ApplicationProjectViewModel>(project);
                    candidateProj.ApplicationId = applicationId;
                    candidateProj.Id = "";
                    //var Natureexpresult = await base.Create<ApplicationProjectViewModel, ApplicationProject>(candidateProj);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_PROJECT";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(candidateProj);

                    var Natureexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }
            // Copy Candidate References
            //var appReferences = await _repo.GetList<ApplicationReferencesViewModel, ApplicationReferences>(x => x.ApplicationId == applicationId);
            //foreach (var item in appReferences)
            //{
            //    await _applicationReferencesBusiness.Delete(item.Id);
            //}

            await _recQueryBusiness.UpdateTable("applicationReferences", applicationId);

            //var candidateReferences = await _repo.GetList<CandidateReferencesViewModel, CandidateReferences>(x => x.CandidateProfileId == profileId);
            var candidateReferences = await _recQueryBusiness.GetCandidateReferencesByCandidateId(profileId);

            if (candidateReferences != null && candidateReferences.Count() > 0)
            {
                foreach (var reference in candidateReferences)
                {
                    var candidateref = _autoMapper.Map<CandidateReferencesViewModel, ApplicationReferencesViewModel>(reference);
                    candidateref.ApplicationId = applicationId;
                    candidateref.Id = "";
                    //var Natureexpresult = await base.Create<ApplicationReferencesViewModel, ApplicationReferences>(candidateref);

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "REC_APPLICATION_REFERENCES";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    notemodel.Json = JsonConvert.SerializeObject(candidateref);

                    var Natureexpresult = await _noteBusiness.ManageNote(notemodel);

                }
            }

        }

        public async Task<CommandResult<ApplicationViewModel>> UpdateApplication(CandidateProfileViewModel model)
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

            //1. var application = await _candidateProfileBusiness.GetSingleById(model.Id);
            var application = await GetCandidateById(model.Id);

            var data = _autoMapper.Map<CandidateProfileViewModel, ApplicationViewModel>(application);
            var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == "UnReviewed");//unreviewed
            //var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == "");//
            data.ApplicationState = state1.IsNotNull() ? state1.Id : "";
            //data.ApplicationStatus = status1.IsNotNull() ? status1.Id : "";
            data.CandidateId = application.Id;
            data.Id = "";
            //data.BatchId = "";
            data.JobAdvertisementId = model.JobAdvertisementId;
            //var jobAdv = await _jobAdvBusiness.GetSingleById(model.JobAdvertisementId);

            //2.  jobAdv query Changed
            var jobAdv = await _recQueryBusiness.GetJobAdvertisementData(model.JobAdvertisementId);

            data.JobId = jobAdv.JobId;
            //data.OrganizationId = jobAdv.OrganizationId;
            data.Score = sum;
            data.SignatureDate = model.SignatureDate.IsNotNull() ? model.SignatureDate : System.DateTime.Now;
            data.AppliedDate = model.SignatureDate.IsNotNull() ? model.SignatureDate : System.DateTime.Now;
            data.Signature = model.Signature;
            //data.ApplicationNo = await GenerateNextApplicationNo();

            //3. Application Note Create Here 
            //var result = await this.Create(data);

            // ApplicationStateId set to UnReviewed
            var lovstate = await _LOVBusiness.GetSingle(x => x.LOVType == "APPLICATION_STATE" && x.Code == "UnReviewed");
            data.ApplicationStateId = lovstate.Id; //"f9c43c63-5cb4-4d4b-9825-70f0692a9c78";
            data.ApplicationNo = await GenerateNextApplicationNo();
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Create;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "REC_APPLICATION";
            //noteTempModel.NoteId = data.ApplicationNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(data);
            var result = await _noteBusiness.ManageNote(notemodel);

            // Dbt need to change each create statement into note create part??

            data.Id = result.Item.UdfNoteTableId;

            if (result.IsSuccess)
            {

                // Copy Candidate criteria  
                if (model.Criterias.IsNotNull() && model.Criterias.Count > 0)
                {
                    foreach (var criteria in model.Criterias)
                    {
                        criteria.ApplicationId = result.Item.UdfNoteTableId;
                        criteria.Id = "";
                        //var criteriaResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(criteria);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_JOB_CRITERIA";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(criteria);
                        var criteriaResult = await _noteBusiness.ManageNote(appmodel);

                    }
                }
                // Copy Candidate skill
                if (model.Skills.IsNotNull() && model.Skills.Count > 0)
                {
                    foreach (var skill in model.Skills)
                    {
                        skill.ApplicationId = result.Item.UdfNoteTableId;
                        skill.Id = "";
                        //var skillResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(skill);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_JOB_CRITERIA";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(skill);
                        var criteriaResult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate otherInfo
                if (model.OtherInformations.IsNotNull() && model.OtherInformations.Count > 0)
                {
                    foreach (var otherInformation in model.OtherInformations)
                    {
                        otherInformation.ApplicationId = result.Item.UdfNoteTableId;
                        otherInformation.Id = "";
                        //var otherInformationResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(otherInformation);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_JOB_CRITERIA";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(otherInformation);
                        var otherInformationResult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Education
                //var candidateEdu = await _repo.GetList<CandidateEducationalViewModel, CandidateEducational>(x => x.CandidateProfileId == data.CandidateId);
                var candidateEdu = await _recQueryBusiness.GetCandidateEduByCandidateId(data.CandidateId);

                if (candidateEdu != null && candidateEdu.Count() > 0)
                {
                    foreach (var education in candidateEdu)
                    {
                        var appEducation = _autoMapper.Map<CandidateEducationalViewModel, ApplicationEducationalViewModel>(education);
                        appEducation.ApplicationId = result.Item.UdfNoteTableId;
                        appEducation.Id = "";
                        //var eduresult = await base.Create<ApplicationEducationalViewModel, ApplicationEducational>(appEducation);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_EDUCATIONAL";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(appEducation);
                        var eduresult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Experience
                //var candidateExp = await _repo.GetList<CandidateExperienceViewModel, CandidateExperience>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateExp = await _recQueryBusiness.GetCandidateExpByCandidateId(data.CandidateId);


                if (candidateExp != null && candidateExp.Count() > 0)
                {
                    foreach (var exp in candidateExp)
                    {
                        var appexp = _autoMapper.Map<CandidateExperienceViewModel, ApplicationExperienceViewModel>(exp);
                        appexp.ApplicationId = result.Item.UdfNoteTableId;
                        appexp.Id = "";
                        //var expresult = await base.Create<ApplicationExperienceViewModel, ApplicationExperience>(appexp);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(appexp);
                        var expresult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Computer Proficiency
                //var candidateCompProficiency = await _repo.GetList<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateCompProficiency = await _recQueryBusiness.GetCandidateCompProfByCandidateId(data.CandidateId);

                if (candidateCompProficiency != null && candidateCompProficiency.Count() > 0)
                {
                    foreach (var Compexp in candidateCompProficiency)
                    {
                        var Compexperience = _autoMapper.Map<CandidateComputerProficiencyViewModel, ApplicationComputerProficiencyViewModel>(Compexp);
                        Compexperience.ApplicationId = result.Item.UdfNoteTableId;
                        Compexperience.Id = "";
                        //var Compexpresult = await base.Create<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>(Compexperience);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_COMP_PROFICIENCY";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(Compexperience);
                        var Compexpresult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Language Proficiency
                //var candidateLanguageProficiency = await _repo.GetList<CandidateLanguageProficiencyViewModel, CandidateLanguageProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateLanguageProficiency = await _recQueryBusiness.GetCandidateLangProfByCandidateId(data.CandidateId);

                if (candidateLanguageProficiency != null && candidateLanguageProficiency.Count() > 0)
                {
                    foreach (var language in candidateLanguageProficiency)
                    {
                        var lang = _autoMapper.Map<CandidateLanguageProficiencyViewModel, ApplicationLanguageProficiencyViewModel>(language);
                        lang.ApplicationId = result.Item.UdfNoteTableId;
                        lang.Id = "";
                        //var langresult = await base.Create<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>(lang);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_LANG_PROFICIENCY";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(lang);
                        var langresult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Experience By Country
                //var candidateexpByCountry = await _repo.GetList<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateexpByCountry = await _recQueryBusiness.GetCandidateExpCountryByCandidateId(data.CandidateId);


                if (candidateexpByCountry != null && candidateexpByCountry.Count() > 0)
                {
                    foreach (var expByountry in candidateexpByCountry)
                    {
                        var countryexp = _autoMapper.Map<CandidateExperienceByCountryViewModel, ApplicationExperienceByCountryViewModel>(expByountry);
                        countryexp.ApplicationId = result.Item.UdfNoteTableId;
                        countryexp.Id = "";
                        //var countryexpresult = await base.Create<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>(countryexp);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_COUNTRY";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(countryexp);
                        var countryexpresult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Experience By Sector
                //var candidateexpBySector = await _repo.GetList<CandidateExperienceBySectorViewModel, CandidateExperienceBySector>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateexpBySector = await _recQueryBusiness.GetCandidateExpSectorByCandidateId(data.CandidateId);

                if (candidateexpBySector != null && candidateexpBySector.Count() > 0)
                {
                    foreach (var expBySector in candidateexpBySector)
                    {
                        var Sectorexp = _autoMapper.Map<CandidateExperienceBySectorViewModel, ApplicationExperienceBySectorViewModel>(expBySector);
                        Sectorexp.ApplicationId = result.Item.UdfNoteTableId;
                        Sectorexp.Id = "";
                        //var Sectorexpresult = await base.Create<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>(Sectorexp);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_SECTOR";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(Sectorexp);
                        var Sectorexpresult = await _noteBusiness.ManageNote(appmodel);

                    }
                }
                // Copy Candidate Experience By Nature
                //var candidateExpByNature = await _repo.GetList<CandidateExperienceByNatureViewModel, CandidateExperienceByNature>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateExpByNature = await _recQueryBusiness.GetCandidateExpNatureByCandidateId(data.CandidateId);

                if (candidateExpByNature != null && candidateExpByNature.Count() > 0)
                {
                    foreach (var expByNature in candidateExpByNature)
                    {
                        var Natureexp = _autoMapper.Map<CandidateExperienceByNatureViewModel, ApplicationeExperienceByNatureViewModel>(expByNature);
                        Natureexp.ApplicationId = result.Item.UdfNoteTableId;
                        Natureexp.Id = "";
                        //var Natureexpresult = await base.Create<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>(Natureexp);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_NATURE";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(Natureexp);
                        var Natureexpresult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Experience By Job
                //var candidateExpByJob = await _repo.GetList<CandidateExperienceByJobViewModel, CandidateExperienceByJob>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateExpByJob = await _recQueryBusiness.GetCandidateExpJobByCandidateId(data.CandidateId);

                if (candidateExpByJob != null && candidateExpByJob.Count() > 0)
                {
                    foreach (var expByJob in candidateExpByJob)
                    {
                        var Jobexp = _autoMapper.Map<CandidateExperienceByJobViewModel, ApplicationExperienceByJobViewModel>(expByJob);
                        Jobexp.ApplicationId = result.Item.UdfNoteTableId;
                        Jobexp.Id = "";
                        //var Natureexpresult = await base.Create<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>(Jobexp);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_JOB";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(Jobexp);
                        var Natureexpresult = await _noteBusiness.ManageNote(appmodel);
                    }
                }
                // Copy Candidate Experience By OtherType
                //var candidateExpByOther = await _repo.GetList<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateExpByOther = await _recQueryBusiness.GetCandidateExpOtherByCandidateId(data.CandidateId);


                if (candidateExpByOther != null && candidateExpByOther.Count() > 0)
                {
                    foreach (var expByOther in candidateExpByOther)
                    {
                        var Otherexp = _autoMapper.Map<CandidateExperienceByOtherViewModel, ApplicationExperienceByOtherViewModel>(expByOther);
                        Otherexp.ApplicationId = result.Item.UdfNoteTableId;
                        Otherexp.Id = "";
                        //var Otherexpresult = await base.Create<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>(Otherexp);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_OTHER";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(Otherexp);
                        var Otherexpresult = await _noteBusiness.ManageNote(appmodel);

                    }
                }
                // Copy Candidate Driving Liciense Detail
                //var candidateDL = await _repo.GetList<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateDL = await _recQueryBusiness.GetCandidateDrivingLicenceByCandidateId(data.CandidateId);

                if (candidateDL != null && candidateDL.Count() > 0)
                {
                    foreach (var dl in candidateDL)
                    {
                        var DL = _autoMapper.Map<CandidateDrivingLicenseViewModel, ApplicationDrivingLicenseViewModel>(dl);
                        DL.ApplicationId = result.Item.UdfNoteTableId;
                        DL.Id = "";
                        //var Natureexpresult = await base.Create<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>(DL);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_DRIVING_LICENSE";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(DL);
                        var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                    }
                }
                // Copy Candidate Project
                //var candidateProject = await _repo.GetList<CandidateProjectViewModel, CandidateProject>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateProject = await _recQueryBusiness.GetCandidateProjectByCandidateId(data.CandidateId);


                if (candidateProject != null && candidateProject.Count() > 0)
                {
                    foreach (var project in candidateProject)
                    {
                        var candidateProj = _autoMapper.Map<CandidateProjectViewModel, ApplicationProjectViewModel>(project);
                        candidateProj.ApplicationId = result.Item.UdfNoteTableId;
                        candidateProj.Id = "";
                        //var Natureexpresult = await base.Create<ApplicationProjectViewModel, ApplicationProject>(candidateProj);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_PROJECT";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(candidateProj);
                        var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                    }
                }
                // Copy Candidate References
                //var candidateReferences = await _repo.GetList<CandidateReferencesViewModel, CandidateReferences>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                var candidateReferences = await _recQueryBusiness.GetCandidateReferencesByCandidateId(data.CandidateId);


                if (candidateReferences != null && candidateReferences.Count() > 0)
                {
                    foreach (var reference in candidateReferences)
                    {
                        var candidateref = _autoMapper.Map<CandidateReferencesViewModel, ApplicationReferencesViewModel>(reference);
                        candidateref.ApplicationId = result.Item.UdfNoteTableId;
                        candidateref.Id = "";
                        //var Natureexpresult = await base.Create<ApplicationReferencesViewModel, ApplicationReferences>(candidateref);

                        var appTempModel = new NoteTemplateViewModel();
                        appTempModel.DataAction = DataActionEnum.Create;
                        appTempModel.ActiveUserId = _userContext.UserId;
                        appTempModel.TemplateCode = "REC_APPLICATION_REFERENCES";
                        var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                        appmodel.Json = JsonConvert.SerializeObject(candidateref);
                        var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                    }
                }
                var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                ApplicationStateTrack.ApplicationId = data.Id;
                ApplicationStateTrack.ApplicationStateId = state1.Id;
                //ApplicationStateTrack.ApplicationStatusId = status1.Id;
                ApplicationStateTrack.ChangedBy = _userContext.UserId;
                ApplicationStateTrack.ChangedDate = DateTime.Now;

                
                //var result1 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);

                var TempModel = new NoteTemplateViewModel();
                TempModel.DataAction = DataActionEnum.Create;
                TempModel.ActiveUserId = _userContext.UserId;
                TempModel.TemplateCode = "REC_APPLICATION_STATE_TRACK";
                var appTrackmodel = await _noteBusiness.GetNoteDetails(TempModel);

                appTrackmodel.Json = JsonConvert.SerializeObject(ApplicationStateTrack);
                var result1 = await _noteBusiness.ManageNote(appTrackmodel);

                return CommandResult<ApplicationViewModel>.Instance(data);
            }
            return CommandResult<ApplicationViewModel>.Instance(new ApplicationViewModel(), false, "Application failed");
        }

        public async Task<List<ApplicationViewModel>> GetApplicationListByCandidate(string candidateId)
        {
            var res = await _recQueryBusiness.GetApplicationListByCandidate(candidateId);
            return res;
        }

        public async Task<List<ApplicationViewModel>> GetBookmarksJobList(string jobIds)
        {
            var res = await _recQueryBusiness.GetBookmarksJobList(jobIds);
            return res;
        }

        public async Task<List<ApplicationViewModel>> GetJobAdvertisementByAgency()
        {
            var res = await _recQueryBusiness.GetJobAdvertisementByAgency();
            return res;
        }


        public async Task<List<ManpowerRecruitmentSummaryViewModel>> GetActiveManpowerRecruitmentSummaryData()
        {
            var res = await _recQueryBusiness.GetActiveManpowerRecruitmentSummaryData();
            return res;
        }

        public async Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search)
        {
            var res = await _recQueryBusiness.GetCandiadteShortListDataByHR(search);
            return res;

        }

        public async Task<List<CandidateExperienceViewModel>> GetListByCandidate(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetListByCandidate(candidateProfileId);
            return res;
        }

        public async Task<CandidateProfileViewModel> GetDocumentsByCandidate(string candidateProfileId)
        {
            var res = await _recQueryBusiness.GetDocumentsByCandidate(candidateProfileId);
            return res;
        }

        public async Task<CandidateProfileViewModel> GetDocumentsByApplication(string applicationId)
        {
            var res = await _recQueryBusiness.GetDocumentsByApplication(applicationId);
            return res;
        }

        public async Task<string> CreateApplicationStatusTrack(string applicationId, string statusCode = null, string taskReferenceId = null)
        {
            try
            {
                var application = await _recQueryBusiness.GetApplicationDetailsById(applicationId);
                var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                ApplicationStateTrack.ApplicationId = application.Id;
                ApplicationStateTrack.ApplicationStateId = application.ApplicationState;
                if (statusCode.IsNullOrEmpty())
                {
                    //var appStatus = await _repo.GetSingle<ApplicationStatus, ApplicationStatus>(x => x.Id == application.ApplicationStatus);
                    var appStatus = await _LOVBusiness.GetSingle(x => x.Id == application.ApplicationStatus);
                    if (appStatus != null)
                    {
                        ApplicationStateTrack.ApplicationStatusCode = appStatus.Code;
                    }
                    ApplicationStateTrack.ApplicationStatusId = application.ApplicationStatus;
                }
                else
                {
                    //var appStatus = await _repo.GetSingle<ApplicationStatus, ApplicationStatus>(x => x.Code == statusCode);
                    var appStatus = await _LOVBusiness.GetSingle(x => x.Code == statusCode);
                    if (appStatus != null)
                    {
                        ApplicationStateTrack.ApplicationStatusId = appStatus.Id;
                    }
                    ApplicationStateTrack.ApplicationStatusCode = statusCode;
                }
                ApplicationStateTrack.TaskReferenceId = taskReferenceId;
                ApplicationStateTrack.ChangedBy = _userContext.UserId;
                ApplicationStateTrack.ChangedDate = DateTime.Now;

                var TempModel = new NoteTemplateViewModel();
                TempModel.DataAction = DataActionEnum.Create;
                TempModel.ActiveUserId = _userContext.UserId;
                TempModel.TemplateCode = "REC_APPLICATION_STATE_TRACK";
                var appTrackmodel = await _noteBusiness.GetNoteDetails(TempModel);

                appTrackmodel.Json = JsonConvert.SerializeObject(ApplicationStateTrack);
                var result2 = await _noteBusiness.ManageNote(appTrackmodel);

                //var result2 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        public async Task<string> UpdateApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state, string BatchId, string JobAddId, string JobId, string OrgId)
        {
            try
            {
                if (type == "JobApplication")
                {
                    //var application = await _recQueryBusiness.GetApplicationDetailsById(applicationId);
                    
                    
                    var state1 = await _LOVBusiness.GetSingle(X => X.Code == state);
                    var status1 = await _LOVBusiness.GetSingle(X => X.Code == status);



                    //application.ApplicationStateId = state1.Id;
                    //application.ApplicationStatusId = status1.Id;
                    //application.AppliedDate = DateTime.Now;

                    if (status == "SHORTLISTED")
                    {
                        //application.BatchId = BatchId;

                        var batch = await _recQueryBusiness.GetBatchDetailsById(BatchId);
                        //application.OrganizationId = batch.OrganizationId;// OrgId;
                        await _recQueryBusiness.UpdateApplicationBatch(applicationId, status1.Id, state1.Id, BatchId, batch.OrganizationId, JobId);
                    }
                    else
                    {
                        await _recQueryBusiness.UpdateApplicationBatch(applicationId, status1.Id, state1.Id, null, null, JobId);
                    }
                    //application.JobId = JobId;

                    //var noteTempModel = new NoteTemplateViewModel();
                    //noteTempModel.DataAction = DataActionEnum.Edit;
                    //noteTempModel.ActiveUserId = _userContext.UserId;
                    //noteTempModel.NoteId = application.ApplicationNoteId;
                    //var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);


                    //notemodel.Json = JsonConvert.SerializeObject(application);

                    //var result1 = await _noteBusiness.ManageNote(notemodel);


                    //var result1 = await base.Edit<ApplicationViewModel, Application>(application);
                    if (true)
                    {
                        var ApplicationStateTrack = new ApplicationStateTrackViewModel();
                        ApplicationStateTrack.ApplicationId = applicationId;
                        ApplicationStateTrack.ApplicationStateId = state1.Id;
                        ApplicationStateTrack.ApplicationStatusId = status1.Id;
                        ApplicationStateTrack.ChangedBy = _userContext.UserId;
                        ApplicationStateTrack.ChangedDate = DateTime.Now;

                        var TempModel = new NoteTemplateViewModel();
                        TempModel.DataAction = DataActionEnum.Create;
                        TempModel.ActiveUserId = _userContext.UserId;
                        TempModel.TemplateCode = "REC_APPLICATION_STATE_TRACK";
                        var appTrackmodel = await _noteBusiness.GetNoteDetails(TempModel);

                        appTrackmodel.Json = JsonConvert.SerializeObject(ApplicationStateTrack);
                        var result2 = await _noteBusiness.ManageNote(appTrackmodel);


                        //var result2 = await base.Create<ApplicationStateTrackViewModel, ApplicationStateTrack>(ApplicationStateTrack);
                        if (status == "SHORTLISTED")
                        {
                            await CreateApplicationStatusTrack(applicationId, "SL_BATCH");
                        }
                    }

                    //string query = @$"update rec.""ApplicationStateTrack"" set ""ApplicationId""='{applicationId}', ""ApplicationStateId""='{state1.Id}', ""ApplicationStatusId""='{status1.Id}',
                    // ""ChangedBy""='{_userContext.UserId}' ";
                    //var result = await _appqueryRepo.ExecuteScalar<bool?>(query, null);
                }
                else if (type == "CandidateProfile")
                {
                    var application = await _recQueryBusiness.GetCandidateById(CandidateProfileId);
                    //var application1 = await GetSingle(x => x.CandidateProfileId == CandidateProfileId && x.JobId == JobId);
                    var application1 = await _recQueryBusiness.GetApplicationDataByCandidateIdandJobId(CandidateProfileId, JobId);
                    if (application1 == null)
                    {
                        var data = _autoMapper.Map<CandidateProfileViewModel, ApplicationViewModel>(application);

                        //var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == state);
                        //var status1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Code == status);

                        var state1 = await _LOVBusiness.GetSingle(X => X.Code == state);
                        var status1 = await _LOVBusiness.GetSingle(X => X.Code == status);

                        data.ApplicationState = state1.Id;
                        data.ApplicationStatus = status1.Id;
                        data.CandidateProfileId = application.Id;
                        data.AppliedDate = DateTime.Now;
                        data.Id = "";
                        if (status == "SHORTLISTED")
                        {
                            data.BatchId = BatchId;
                            var batch = await _repo.GetSingleById<BatchViewModel, Batch>(BatchId);
                            data.OrganizationId = batch.OrganizationId;// OrgId;
                        }
                        //data.ApplicationNo = await GenerateNextApplicationNo();
                        //data.JobAdvertisementId = JobAddId;
                        data.JobId = JobId;
                        // data.OrganizationId = OrgId;
                        //var result = await this.Create(data);

                        data.ApplicationStateId = "f9c43c63-5cb4-4d4b-9825-70f0692a9c78";
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "REC_APPLICATION";
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        notemodel.Json = JsonConvert.SerializeObject(data);
                        var result = await _noteBusiness.ManageNote(notemodel);

                        data.Id = result.Item.UdfNoteTableId;

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
                                var candResult = await _recQueryBusiness.UpdateCandidateProfileDetails(candmodel);
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
                                    var candResult = await _recQueryBusiness.UpdateCandidateProfileDetails(candmodel);
                                }
                            }
                            //var name = application.FirstName.IsNotNullAndNotEmpty() && application.LastName.IsNotNullAndNotEmpty() ?
                            //    application.FirstName + " " + application.LastName : "";

                        }


                        if (result.IsSuccess)
                        {
                            // Copy Candidate Education                   
                            //var candidateEdu = await _repo.GetList<CandidateEducationalViewModel, CandidateEducational>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateEdu = await _recQueryBusiness.GetCandidateEduByCandidateId(data.CandidateId);

                            if (candidateEdu != null && candidateEdu.Count() > 0)
                            {
                                foreach (var education in candidateEdu)
                                {
                                    var appEducation = _autoMapper.Map<CandidateEducationalViewModel, ApplicationEducationalViewModel>(education);
                                    appEducation.ApplicationId = result.Item.UdfNoteTableId;
                                    appEducation.Id = "";
                                    //var eduresult = await base.Create<ApplicationEducationalViewModel, ApplicationEducational>(appEducation);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_EDUCATIONAL";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(appEducation);
                                    var eduresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Experience
                            //var candidateExp = await _repo.GetList<CandidateExperienceViewModel, CandidateExperience>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateExp = await _recQueryBusiness.GetCandidateExpByCandidateId(data.CandidateId);

                            if (candidateExp != null && candidateExp.Count() > 0)
                            {
                                foreach (var exp in candidateExp)
                                {
                                    var appexp = _autoMapper.Map<CandidateExperienceViewModel, ApplicationExperienceViewModel>(exp);
                                    appexp.ApplicationId = result.Item.UdfNoteTableId;
                                    appexp.Id = "";
                                    //var expresult = await base.Create<ApplicationExperienceViewModel, ApplicationExperience>(appexp);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(appexp);
                                    var expresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Computer Proficiency
                            //var candidateCompProficiency = await _repo.GetList<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateCompProficiency = await _recQueryBusiness.GetCandidateCompProfByCandidateId(data.CandidateId);

                            if (candidateCompProficiency != null && candidateCompProficiency.Count() > 0)
                            {
                                foreach (var Compexp in candidateCompProficiency)
                                {
                                    var Compexperience = _autoMapper.Map<CandidateComputerProficiencyViewModel, ApplicationComputerProficiencyViewModel>(Compexp);
                                    Compexperience.ApplicationId = result.Item.UdfNoteTableId;
                                    Compexperience.Id = "";
                                    //var Compexpresult = await base.Create<ApplicationComputerProficiencyViewModel, ApplicationComputerProficiency>(Compexperience);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_COMP_PROFICIENCY";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(Compexperience);
                                    var Compexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Language Proficiency
                            //var candidateLanguageProficiency = await _repo.GetList<CandidateLanguageProficiencyViewModel, CandidateLanguageProficiency>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateLanguageProficiency = await _recQueryBusiness.GetCandidateLangProfByCandidateId(data.CandidateId);

                            if (candidateLanguageProficiency != null && candidateLanguageProficiency.Count() > 0)
                            {
                                foreach (var language in candidateLanguageProficiency)
                                {
                                    var lang = _autoMapper.Map<CandidateLanguageProficiencyViewModel, ApplicationLanguageProficiencyViewModel>(language);
                                    lang.ApplicationId = result.Item.UdfNoteTableId;
                                    lang.Id = "";
                                    //var langresult = await base.Create<ApplicationLanguageProficiencyViewModel, ApplicationLanguageProficiency>(lang);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_LANG_PROFICIENCY";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(lang);
                                    var langresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Experience By Country
                            //var candidateexpByCountry = await _repo.GetList<CandidateExperienceByCountryViewModel, CandidateExperienceByCountry>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateexpByCountry = await _recQueryBusiness.GetCandidateExpCountryByCandidateId(data.CandidateId);

                            if (candidateexpByCountry != null && candidateexpByCountry.Count() > 0)
                            {
                                foreach (var expByountry in candidateexpByCountry)
                                {
                                    var countryexp = _autoMapper.Map<CandidateExperienceByCountryViewModel, ApplicationExperienceByCountryViewModel>(expByountry);
                                    countryexp.ApplicationId = result.Item.UdfNoteTableId;
                                    countryexp.Id = "";
                                    //var countryexpresult = await base.Create<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>(countryexp);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_COUNTRY";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(countryexp);
                                    var countryexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Experience By Sector
                            //var candidateexpBySector = await _repo.GetList<CandidateExperienceBySectorViewModel, CandidateExperienceBySector>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateexpBySector = await _recQueryBusiness.GetCandidateExpSectorByCandidateId(data.CandidateId);

                            if (candidateexpBySector != null && candidateexpBySector.Count() > 0)
                            {
                                foreach (var expBySector in candidateexpBySector)
                                {
                                    var Sectorexp = _autoMapper.Map<CandidateExperienceBySectorViewModel, ApplicationExperienceBySectorViewModel>(expBySector);
                                    Sectorexp.ApplicationId = result.Item.UdfNoteTableId;
                                    Sectorexp.Id = "";
                                    //var Sectorexpresult = await base.Create<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>(Sectorexp);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_SECTOR";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(Sectorexp);
                                    var Sectorexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Experience By Nature
                            //var candidateExpByNature = await _repo.GetList<CandidateExperienceByNatureViewModel, CandidateExperienceByNature>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateExpByNature = await _recQueryBusiness.GetCandidateExpNatureByCandidateId(data.CandidateId);

                            if (candidateExpByNature != null && candidateExpByNature.Count() > 0)
                            {
                                foreach (var expByNature in candidateExpByNature)
                                {
                                    var Natureexp = _autoMapper.Map<CandidateExperienceByNatureViewModel, ApplicationeExperienceByNatureViewModel>(expByNature);
                                    Natureexp.ApplicationId = result.Item.UdfNoteTableId;
                                    Natureexp.Id = "";
                                    //var Natureexpresult = await base.Create<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>(Natureexp);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_NATURE";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(Natureexp);
                                    var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Experience By Job
                            // var candidateExpByJob = await _repo.GetList<CandidateExperienceByJobViewModel, CandidateExperienceByJob>(x => x.CandidateProfileId == result.Item.CandidateProfileId);
                            var candidateExpByJob = await _recQueryBusiness.GetCandidateExpJobByCandidateId(data.CandidateId);

                            if (candidateExpByJob != null && candidateExpByJob.Count() > 0)
                            {
                                foreach (var expByJob in candidateExpByJob)
                                {
                                    var Jobexp = _autoMapper.Map<CandidateExperienceByJobViewModel, ApplicationExperienceByJobViewModel>(expByJob);
                                    Jobexp.ApplicationId = result.Item.UdfNoteTableId;
                                    Jobexp.Id = "";
                                    //var Natureexpresult = await base.Create<ApplicationExperienceByJobViewModel, ApplicationExperienceByJob>(Jobexp);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_JOB";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(Jobexp);
                                    var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Driving Liciense Detail
                            //var candidateDL = await _repo.GetList<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>(x => x.CandidateProfileId == result.Item.CandidateProfileId);

                            var candidateDL = await _recQueryBusiness.GetCandidateDrivingLicenceByCandidateId(data.CandidateId);

                            if (candidateDL != null && candidateDL.Count() > 0)
                            {
                                foreach (var dl in candidateDL)
                                {
                                    var DL = _autoMapper.Map<CandidateDrivingLicenseViewModel, ApplicationDrivingLicenseViewModel>(dl);
                                    DL.ApplicationId = result.Item.UdfNoteTableId;
                                    DL.Id = "";
                                    //var Natureexpresult = await base.Create<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>(DL);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_DRIVING_LICENSE";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(DL);
                                    var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Project
                            //var candidateProject = await _repo.GetList<CandidateProjectViewModel, CandidateProject>(x => x.CandidateProfileId == result.Item.CandidateProfileId);

                            var candidateProject = await _recQueryBusiness.GetCandidateProjectByCandidateId(data.CandidateId);

                            if (candidateProject != null && candidateProject.Count() > 0)
                            {
                                foreach (var project in candidateProject)
                                {
                                    var candidateProj = _autoMapper.Map<CandidateProjectViewModel, ApplicationProjectViewModel>(project);
                                    candidateProj.ApplicationId = result.Item.UdfNoteTableId;
                                    candidateProj.Id = "";
                                    //var Natureexpresult = await base.Create<ApplicationProjectViewModel, ApplicationProject>(candidateProj);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_PROJECT";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(candidateProj);
                                    var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate References
                            //var candidateReferences = await _repo.GetList<CandidateReferencesViewModel, CandidateReferences>(x => x.CandidateProfileId == result.Item.CandidateProfileId);

                            var candidateReferences = await _recQueryBusiness.GetCandidateReferencesByCandidateId(data.CandidateId);


                            if (candidateReferences != null && candidateReferences.Count() > 0)
                            {
                                foreach (var reference in candidateReferences)
                                {
                                    var candidateref = _autoMapper.Map<CandidateReferencesViewModel, ApplicationReferencesViewModel>(reference);
                                    candidateref.ApplicationId = result.Item.UdfNoteTableId;
                                    candidateref.Id = "";
                                    //var Natureexpresult = await base.Create<ApplicationReferencesViewModel, ApplicationReferences>(candidateref);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_REFERENCES";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(candidateref);
                                    var Natureexpresult = await _noteBusiness.ManageNote(appmodel);

                                }
                            }
                            // Copy Candidate Other
                            //var candidateOther = await _repo.GetList<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>(x => x.CandidateProfileId == result.Item.CandidateProfileId);

                            var candidateOther = await _recQueryBusiness.GetCandidateExpOtherByCandidateId(data.CandidateId);

                            if (candidateOther != null && candidateOther.Count() > 0)
                            {
                                foreach (var reference in candidateOther)
                                {
                                    var candidateref = _autoMapper.Map<CandidateExperienceByOtherViewModel, ApplicationExperienceByOtherViewModel>(reference);
                                    candidateref.ApplicationId = result.Item.UdfNoteTableId;
                                    candidateref.Id = "";
                                    //var Natureexpresult = await base.Create<ApplicationExperienceByOtherViewModel, ApplicationExperienceByOther>(candidateref);

                                    var appTempModel = new NoteTemplateViewModel();
                                    appTempModel.DataAction = DataActionEnum.Create;
                                    appTempModel.ActiveUserId = _userContext.UserId;
                                    appTempModel.TemplateCode = "REC_APPLICATION_EXPERIENCE_OTHER";
                                    var appmodel = await _noteBusiness.GetNoteDetails(appTempModel);

                                    appmodel.Json = JsonConvert.SerializeObject(candidateref);
                                    var Otherexpresult = await _noteBusiness.ManageNote(appmodel);

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

        public async Task<RecBatchViewModel> GetBatchDetailsById(string id)
        {
            var result = await _recQueryBusiness.GetBatchDetailsById(id);
            return result;

        }

        public async Task<string> GenerateNextApplicationNo()
        {
            var date = DateTime.Now.Date;
            var id = await _recQueryBusiness.GenerateNextDatedApplicationId();
            return string.Concat("AP-", String.Format("{0:dd-MM-yyyy}", date), "-", id);
        }

        public async Task<IdNameViewModel> GetGrade(string Id)
        {
            var queryData = await _recQueryBusiness.GetGrade(Id);
            return queryData;
        }

        #endregion

        #region s
        public async Task<bool> DeleteCandExpByCountry(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                var res = await _recQueryBusiness.DeleteCandExpByCountry(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }
        public async Task<CandidateExperienceByCountryViewModel> GetCandidateExperiencebyCountryDetails(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperiencebyCountryDetails(Id);
            return queryData;
        }

        public async Task<List<CandidateExperienceByCountryViewModel>> ReadCandidateExperiencebyCountry(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateExperiencebyCountry(candidateProfileId);
            return queryData;
        }
        public async Task<bool> DeleteCandExpByJob(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                var res = await _recQueryBusiness.DeleteCandExpByJob(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }
        public async Task<CandidateExperienceByJobViewModel> GetCandidateExperiencebyJobDetails(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperiencebyJobDetails(Id);
            return queryData;
        }

        public async Task<List<CandidateExperienceByJobViewModel>> ReadCandidateExperiencebyJob(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateExperiencebyJob(candidateProfileId);
            return queryData;
        }
        public async Task<bool> DeleteCandExpByNature(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                var res = await _recQueryBusiness.DeleteCandExpByNature(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }
        public async Task<CandidateExperienceByNatureViewModel> GetCandidateExperiencebyNatureDetails(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperiencebyNatureDetails(Id);
            return queryData;
        }
        public async Task<List<CandidateExperienceByNatureViewModel>> ReadCandidateExperiencebyNature(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateExperiencebyNature(candidateProfileId);
            return queryData;
        }


        public async Task<bool> DeleteCandExpByOther(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {
                var res = await _recQueryBusiness.DeleteCandExpByOther(NoteId);
                return res;
                //await Delete(NoteId);
            }
            return false;
        }
        public async Task<CandidateExperienceByOtherViewModel> GetCandidateExperiencebyOtherDetails(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperiencebyOtherDetails(Id);
            return queryData;
        }

        public async Task<List<CandidateExperienceByOtherViewModel>> ReadCandidateExperiencebyOther(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateExperiencebyOther(candidateProfileId);
            return queryData;
        }
        public async Task<bool> DeleteCandExpBySector(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {
                var res = await _recQueryBusiness.DeleteCandExpBySector(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }
        public async Task<CandidateExperienceBySectorViewModel> GetCandidateExperiencebySectorDetails(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperiencebySectorDetails(Id);
            return queryData;
        }

        public async Task<List<CandidateExperienceBySectorViewModel>> ReadCandidateExperiencebySector(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateExperiencebySector(candidateProfileId);
            return queryData;
        }
        public async Task<bool> DeleteCandExpByProject(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {
                var res = await _recQueryBusiness.DeleteCandExpByProject(NoteId);
                return res;
                //await Delete(NoteId);
            }
            return false;
        }
        public async Task<CandidateProjectViewModel> GetCandidateExperiencebyProjectDetails(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperiencebyProjectDetails(Id);
            return queryData;
        }
        public async Task<List<CandidateProjectViewModel>> ReadCandidateExperiencebyProject(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateExperiencebyProject(candidateProfileId);
            return queryData;
        }
        public async Task<CandidateReferencesViewModel> GetCandidateReferenceDetails(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateReferenceDetails(Id);
            return queryData;
        }

        public async Task<List<CandidateReferencesViewModel>> ReadCandidateReference(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateReference(candidateProfileId);
            return queryData;
        }
        public async Task<bool> DeleteCandRefer(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                var res = await _recQueryBusiness.DeleteCandRefer(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }
        public async Task<bool> DeleteCandidateEducational(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                var res = await _recQueryBusiness.DeleteCandidateEducational(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }
       
        public async Task<CandidateEducationalViewModel> GetCandidateEducational(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateEducational(Id);
            return queryData;
        }
        public async Task<List<CandidateEducationalViewModel>> ReadCandidateEducational(QualificationTypeEnum qualificationType, string candidateProfileId )
        {
            var queryData = await _recQueryBusiness.ReadCandidateEducational(qualificationType, candidateProfileId);
            return queryData;
        }
        public async Task<bool> DeleteCandidateComputerProf(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                await _recQueryBusiness.DeleteCandidateComputerProf(NoteId);

                //await Delete(NoteId);

            }
            return false;
        }

        public async Task<CandidateComputerProficiencyViewModel> GetCandidateComputerProf(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateComputerProf(Id);
            return queryData;
        }
        public async Task<bool> DeleteCandidateLanguageProf(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {
                var res =  await _recQueryBusiness.DeleteCandidateLanguageProf(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }

        public async Task<CandidateDrivingLicenseViewModel> GetCandidateDrivingLicense(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateDrivingLicense(Id);
            return queryData;
        }
        public async Task<bool> DeleteCandidateDrivingLicense(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                await _recQueryBusiness.DeleteCandidateDrivingLicense(NoteId);

                //await Delete(NoteId);

            }
            return false;
        }

        public async Task<CandidateLanguageProficiencyViewModel> GetCandidateLanguageProf(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateLanguageProf(Id);
            return queryData;
        }
        public async Task<List<ApplicationJobCriteriaViewModel>> GetCriteriaData(string ApplicationId, string type)
        {
            var res = await _recQueryBusiness.GetCriteriaData(ApplicationId, type);
            return res;
        }
        public async Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaList(string ApplicationId, string type)
        {
            var res = await _recQueryBusiness.GetApplicationJobCriteriaList(ApplicationId, type);
            return res;
        }
        public async Task<RecApplicationViewModel> GetCandidateAppDetails(string canId,string jobId)
        {
            var queryData = await _recQueryBusiness.GetCandidateAppDetails(canId,jobId);
            return queryData;
        }
        public async Task<List<CandidateProfileViewModel>> GetStaffList(string id)
        {
            var queryData = await _recQueryBusiness.GetStaffList(id);
            return queryData;
        }
        public async Task<CandidateExperienceViewModel> GetCandidateExperiencebyId(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperiencebyId(Id);
            return queryData;
        }
        public async Task<CandidateEducationalViewModel> GetCandidateEducationalbyId(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateEducationalbyId(Id);
            return queryData;
        }
        public async Task<bool> DeleteBeneficiary(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                await _recQueryBusiness.DeleteBeneficiary(NoteId);

                //await Delete(NoteId);

            }
            return false;
        }
        public async Task<ApplicationBeneficiaryViewModel> GetBeneficiaryDataByid(string Id,string referId)
        {
            var queryData = await _recQueryBusiness.GetBeneficiaryDataByid(Id,referId);
            return queryData;
        }

        public async Task<List<ApplicationBeneficiaryViewModel>> ReadBeneficiaryData(string referId)
        {
            var queryData = await _recQueryBusiness.ReadBeneficiaryData(referId);
            return queryData;
        }
        #endregion s
        public async Task<ApplicationViewModel> GetAppDetailsById(string appId)
        {
            var result = await _recQueryBusiness.GetAppDetailsById(appId);
            return result;
        }


        #region p
        public async Task<bool> DeleteCandidateExperience(string NoteId)
        {
            var note = await _querynoteRepo.GetSingleById(NoteId);
            if (note != null)
            {

                var res = await _recQueryBusiness.DeleteCandidateExperience(NoteId);
                return res;
                //await Delete(NoteId);

            }
            return false;
        }
        //public async Task<CandidateExperienceByCountryViewModel> GetCandidateExperiencebyCountryDetails(string Id)
        //{
        //    var queryData = await _recQueryBusiness.GetCandidateExperiencebyCountryDetails(Id);
        //    return queryData;
        //}
        public async Task<CandidateExperienceViewModel> GetCandidateExperience(string Id)
        {
            var queryData = await _recQueryBusiness.GetCandidateExperience(Id);
            return queryData;
        }
        public async Task<List<CandidateExperienceViewModel>> ReadCandidateExperience(string candidateProfileId)
        {
            var queryData = await _recQueryBusiness.ReadCandidateExperience(candidateProfileId);
            return queryData;
        }

        public async Task<List<CandidateProfileViewModel>> GetWorkerList(string id)
        {
            var queryData = await _recQueryBusiness.GetWorkerList(id);
            return queryData;
        }

        public async Task<bool> CreateWorkerCandidateAndApplication(List<WorkerCandidateViewModel> list)
        {
            //if (model.IsNotNull())
            //{
            //    foreach (var can in model.Created)
            //    {
            //        if (can.PassportNumber.IsNotNull())
            //        {
            //            var data = await _candidateProfileBusiness.GetList(x => x.PassportNumber == can.PassportNumber && x.FirstName == can.CandidateName);
            //            if (data.Count > 0)
            //            {
            //                //Do nothing
            //                return false;
            //            }
            //            else
            //            {
            //                var candidate = new CandidateProfileViewModel
            //                {
            //                    FirstName = can.CandidateName,
            //                    ContactPhoneHome = can.Mobile,
            //                    PassportNumber = can.PassportNumber,
            //                    Remarks = can.Remarks,
            //                    NetSalary = can.Salary_QAR,
            //                    TotalWorkExperience = Convert.ToDouble(can.TotalWorkExperience),
            //                    SourceFrom = SourceTypeEnum.Agency.ToString(),
            //                    AgencyId = _userContext.UserId,

            //                };
            //                var jobDetails = await _jobAdvBusiness.GetSingleById(can.Position.Split("_")[^1]);
            //                //var orgId = "";
            //                //if (jobDetails.IsNotNull())
            //                //{
            //                //    orgId = jobDetails.OrganizationId;
            //                //}

            //                if (can.Age.IsNotNull())
            //                {
            //                    candidate.Age = Convert.ToInt32(can.Age);
            //                }

            //                if (can.Position.IsNotNull())
            //                {
            //                    candidate.JobAdvertisement = jobDetails.JobId;

            //                }

            //                if (can.PassportStatus.IsNotNull())
            //                {
            //                    candidate.PassportStatusId = can.PassportStatus.Split("_")[^1];

            //                }

            //                if (can.DOB.IsNotNull())
            //                {
            //                    candidate.BirthDate = Convert.ToDateTime(can.DOB);

            //                }
            //                if (can.PassportExpiry.IsNotNull())
            //                {
            //                    candidate.PassportExpiryDate = Convert.ToDateTime(can.PassportExpiry);

            //                }
            //                if (can.PassportCountry.IsNotNull())
            //                {
            //                    candidate.PassportIssueCountryId = can.PassportCountry.Split("_")[^1];
            //                }


            //                if (can.DocumentLink.IsNotNull() && can.DocumentLink.Contains("_"))
            //                {
            //                    var resume = can.DocumentLink.Split("_");
            //                    candidate.OtherCertificateId = resume[^1];
            //                }

            //                //if (can.PassportLink.IsNotNull() && can.PassportLink.Contains("_"))
            //                //{
            //                //    var resume = can.PassportLink.Split("_");
            //                //    candidate.PassportAttachmentId = resume[^1];
            //                //}



            //                var res = await _candidateProfileBusiness.CreateCandidate(candidate);


            //                if (res.IsNotNull())
            //                {
            //                    var countryIndia = string.Empty;

            //                    var countryListData = await _masterBusiness.GetIdNameList("Country");
            //                    if (countryListData.IsNotNull())
            //                    {
            //                        var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
            //                        if (selectedCountry.IsNotNull())
            //                        {
            //                            countryIndia = selectedCountry.Id;
            //                        }

            //                    }
            //                    var candidateExpCountry = await _candidateExperienceByCountryBusiness.Create(new CandidateExperienceByCountryViewModel
            //                    {
            //                        SequenceOrder = 1,
            //                        CountryId = countryIndia,
            //                        CandidateProfileId = res.Item.Id.ToString(),
            //                        NoOfYear = Convert.ToDouble(can.WorkExperienceIndia)
            //                    });

            //                    var othetExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
            //                    if (othetExp.IsNotNull())
            //                    {
            //                        var candidateExpOther = await _candidateExperienceByOtherBusiness.Create(new CandidateExperienceByOtherViewModel
            //                        {
            //                            CompanyId = othetExp.CompanyId,
            //                            NoOfYear = Convert.ToDouble(can.WorkExperienceAbroad),
            //                            CandidateProfileId = res.Item.Id.ToString(),
            //                            SequenceOrder = 1,
            //                        });
            //                    }

            //                    var state1 = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == "UnReviewed");//unreviewed


            //                    //create application
            //                    var appViewModel = new ApplicationViewModel()
            //                    {
            //                        CandidateProfileId = res.Item.Id,
            //                        FirstName = can.CandidateName,
            //                        BirthDate = candidate.BirthDate,
            //                        ContactPhoneHome = can.Mobile,
            //                        PassportExpiryDate = candidate.PassportExpiryDate,
            //                        PassportNumber = can.PassportNumber,
            //                        Remarks = can.Remarks,
            //                        NetSalary = can.Salary_QAR,
            //                        WorkExperience = Convert.ToDouble(can.TotalWorkExperience),
            //                        TotalOtherExperience = Convert.ToDouble(can.WorkExperienceAbroad),
            //                        TotalWorkExperience = Convert.ToDouble(can.WorkExperienceIndia),
            //                        SourceFrom = SourceTypeEnum.Agency.ToString(),
            //                        JobId = jobDetails.JobId,
            //                        PassportIssueCountryId = candidate.PassportIssueCountryId,
            //                        AgencyId = _userContext.UserId,
            //                        Age = candidate.Age,
            //                        ApplicationState = state1.IsNotNull() ? state1.Id : "",
            //                        //OrganizationId = orgId,
            //                    };

            //                    if (can.PassportStatus.IsNotNull())
            //                    {
            //                        appViewModel.PassportStatusId = can.PassportStatus.Split("_")[^1];
            //                    }

            //                    if (can.DocumentLink.IsNotNull() && can.DocumentLink.Contains("_"))
            //                    {
            //                        var resume = can.DocumentLink.Split("_");
            //                        appViewModel.OtherCertificateId = resume[^1];
            //                    }

            //                    var app = await Create(appViewModel);

            //                    if (app.IsNotNull())
            //                    {
            //                        if (can.CriteriaSkillsData.IsNotNullAndNotEmpty())
            //                        {
            //                            var skilldata = can.CriteriaSkillsData.Split("||");
            //                            res.Item.JobAdvertisementId = can.Position.Split("_")[^1];
            //                            res.Item.Criterias = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(skilldata[0]);
            //                            res.Item.Skills = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(skilldata[1]);
            //                            res.Item.OtherInformations = JsonConvert.DeserializeObject<List<ApplicationJobCriteriaViewModel>>(skilldata[2]);
            //                            //var criteria = await UpdateApplication(res.Item);
            //                            if (res.Item.Criterias.IsNotNull() && res.Item.Criterias.Count > 0)
            //                            {
            //                                foreach (var criteria in res.Item.Criterias)
            //                                {
            //                                    criteria.ApplicationId = app.Item.Id;
            //                                    criteria.Id = "";
            //                                    var criteriaResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(criteria);
            //                                }
            //                            }
            //                            // Copy Candidate skill
            //                            if (res.Item.Skills.IsNotNull() && res.Item.Skills.Count > 0)
            //                            {
            //                                foreach (var skill in res.Item.Skills)
            //                                {
            //                                    skill.ApplicationId = app.Item.Id;
            //                                    skill.Id = "";
            //                                    var skillResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(skill);
            //                                }
            //                            }
            //                            // Copy Candidate otherInfo
            //                            if (res.Item.OtherInformations.IsNotNull() && res.Item.OtherInformations.Count > 0)
            //                            {
            //                                foreach (var otherInformation in res.Item.OtherInformations)
            //                                {
            //                                    otherInformation.ApplicationId = app.Item.Id;
            //                                    otherInformation.Id = "";
            //                                    var otherInformationResult = await base.Create<ApplicationJobCriteriaViewModel, ApplicationJobCriteria>(otherInformation);
            //                                }
            //                            }
            //                        }

            //                        var countryList = await _masterBusiness.GetIdNameList("Country");
            //                        if (countryList.IsNotNull())
            //                        {
            //                            var selectedCountry = countryList.Where(x => x.Code == "India").FirstOrDefault();
            //                            if (selectedCountry.IsNotNull())
            //                            {
            //                                countryIndia = selectedCountry.Id;
            //                            }

            //                        }
            //                        try
            //                        {
            //                            var m = new ApplicationExperienceByCountryViewModel
            //                            {
            //                                CountryId = countryIndia,
            //                                ApplicationId = app.Item.Id.ToString(),
            //                                NoOfYear = Convert.ToDouble(can.WorkExperienceIndia)
            //                            };
            //                            var appExpCountry = await _applicationExperienceByCountryBusiness.Create(m);
            //                        }
            //                        catch (Exception ex)
            //                        {

            //                        }

            //                        var appExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
            //                        if (appExp.IsNotNull())
            //                        {
            //                            var candidateExpOther = await _applicationExperienceByOtherBusiness.Create(new ApplicationExperienceByOtherViewModel
            //                            {
            //                                CompanyId = othetExp.CompanyId,
            //                                NoOfYear = Convert.ToDouble(can.WorkExperienceAbroad),
            //                                ApplicationId = app.Item.Id,
            //                            });
            //                        }


            //                    }


            //                }

            //            }
            //        }
            //    }

            //}
            //if (model.Updated.IsNotNull())
            //{
            //    foreach (var candidate in model.Updated)
            //    {
            //        if (candidate.PassportNumber.IsNotNull())
            //        {
            //            var data = await _candidateProfileBusiness.GetSingle(x => x.PassportNumber == candidate.PassportNumber);
            //            if (data.IsNotNull())
            //            {
            //                data.Id = data.Id;
            //                data.FirstName = candidate.CandidateName;
            //                data.ContactPhoneHome = candidate.Mobile;
            //                data.PassportNumber = candidate.PassportNumber;
            //                data.Remarks = candidate.Remarks;
            //                data.NetSalary = candidate.Salary_QAR;
            //                data.TotalWorkExperience = Convert.ToDouble(candidate.TotalWorkExperience);
            //                data.SourceFrom = SourceTypeEnum.Agency.ToString();
            //                data.AgencyId = _userContext.UserId;
            //                data.Age = candidate.Age.IsNotNull() ? Convert.ToInt32(candidate.Age) : 0;

            //                if (candidate.PassportStatus.IsNotNull())
            //                {
            //                    data.PassportStatusId = candidate.PassportStatus.Split("_")[^1];
            //                }

            //                var jobDetails = await _jobAdvBusiness.GetSingleById(candidate.Position.Split("_")[^1]);
            //                //var orgId = "";
            //                //if (jobDetails.IsNotNull())
            //                //{
            //                //    orgId = jobDetails.OrganizationId;
            //                //}
            //                if (candidate.Position.IsNotNull())
            //                {
            //                    data.JobAdvertisement = jobDetails.JobId;
            //                }

            //                if (candidate.DOB.IsNotNull())
            //                {
            //                    data.BirthDate = Convert.ToDateTime(candidate.DOB);

            //                }
            //                if (candidate.PassportExpiry.IsNotNull())
            //                {
            //                    data.PassportExpiryDate = Convert.ToDateTime(candidate.PassportExpiry);
            //                }
            //                if (candidate.PassportCountry.IsNotNull())
            //                {
            //                    data.PassportIssueCountryId = candidate.PassportCountry.Split("_")[^1];
            //                }
            //                if (candidate.DocumentLink.IsNotNull() && candidate.DocumentLink.Contains("_"))
            //                {
            //                    var resume = candidate.DocumentLink.Split("_");
            //                    data.OtherCertificateId = resume[^1];
            //                }

            //                var res = await _candidateProfileBusiness.EditCandidate(data);

            //                if (res.IsNotNull())
            //                {

            //                    var countryIndia = string.Empty;

            //                    var countryListData = await _masterBusiness.GetIdNameList("Country");
            //                    if (countryListData.IsNotNull())
            //                    {
            //                        var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
            //                        if (selectedCountry.IsNotNull())
            //                        {
            //                            countryIndia = selectedCountry.Id;
            //                        }

            //                    }

            //                    countryIndia = await CandidateExperienceByCountry(candidate, res, countryIndia, countryListData);

            //                    ListOfValueViewModel othetExp = await CandidateOtherExperience(candidate, res);

            //                    //await CandidateEducation(candidate, data, res);

            //                    //await CandidateExperience(candidate, data, res);

            //                    var appDetails = await GetSingle(x => x.CandidateProfileId == data.Id &&
            //                    x.JobId == data.JobAdvertisement);

            //                    if (appDetails.IsNotNull() && appDetails.ApplicationStateCode == "UnReviewed")
            //                    {

            //                        appDetails.CandidateProfileId = data.Id;
            //                        appDetails.Id = appDetails.Id;
            //                        appDetails.FirstName = candidate.CandidateName;
            //                        appDetails.BirthDate = data.BirthDate;
            //                        appDetails.ContactPhoneHome = candidate.Mobile;
            //                        appDetails.PassportIssueCountryId = data.PassportIssueCountryId;
            //                        appDetails.PassportExpiryDate = data.PassportExpiryDate;
            //                        appDetails.PassportNumber = candidate.PassportNumber;
            //                        appDetails.Remarks = candidate.Remarks;
            //                        appDetails.NetSalary = candidate.Salary_QAR;
            //                        appDetails.TotalOtherExperience = Convert.ToDouble(candidate.WorkExperienceAbroad);
            //                        appDetails.TotalWorkExperience = Convert.ToDouble(candidate.WorkExperienceIndia);
            //                        appDetails.SourceFrom = SourceTypeEnum.Agency.ToString();
            //                        appDetails.JobId = jobDetails.JobId;
            //                        appDetails.AgencyId = _userContext.UserId;
            //                        appDetails.Age = candidate.Age.IsNotNull() ? Convert.ToInt32(candidate.Age) : 0;
            //                        //appDetails.OrganizationId = orgId;
            //                        if (candidate.PassportStatus.IsNotNull())
            //                        {
            //                            appDetails.PassportStatusId = candidate.PassportStatus.Split("_")[^1];
            //                        }
            //                        if (candidate.DocumentLink.IsNotNull() && candidate.DocumentLink.Contains("_"))
            //                        {
            //                            var resume = candidate.DocumentLink.Split("_");
            //                            appDetails.OtherCertificateId = resume[^1];
            //                        }
            //                        var app = await Edit(appDetails);

            //                        if (app.IsNotNull())
            //                        {

            //                            if (countryListData.IsNotNull())
            //                            {
            //                                var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
            //                                if (selectedCountry.IsNotNull())
            //                                {
            //                                    countryIndia = selectedCountry.Id;
            //                                }

            //                            }

            //                            await AppExperienceByCountry(candidate, countryIndia, app);

            //                            await OtherAppExperience(candidate, othetExp, app);
            //                        }

            //                    }

            //                }

            //            }
            //            else
            //            {

            //                //Do nothing
            //            }
            //        }
            //    }

            //}
            return true;
        }


        #endregion p

    }
}
