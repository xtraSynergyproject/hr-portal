using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ICareerPortalBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        #region N
        Task<List<ListOfValueViewModel>> GetJobAdvertisementListWithCount(string agency);
        Task<List<LOVViewModel>> GetManpowerTypeListOfValue();
        Task<List<JobAdvertisementViewModel>> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerTypeId, string agency);
        Task<List<LOVViewModel>> GetListOfValue(string type);
        Task<JobAdvertisementViewModel> GetNameById(string jobAdvId);
        Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string JobId);
        Task<Tuple<CandidateProfileViewModel, bool>> IsCandidateProfileFilled();
        Task<ApplicationViewModel> GetApplicationData(string Id, string jobAdvId);
        Task<CandidateProfileViewModel> GetCandidateDataByUserId(string userId);
        Task<List<ApplicationViewModel>> GetApplicationListByCandidateId(string candidateId);
        Task<CandidateProfileViewModel> GetApplicationDetails(string candidateProfileId, string jobAdvId);
        Task<CandidateProfileViewModel> GetApplicationDetailsUsingAppId(string applicationId, string jobAdvId);
        Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByApplicationIdAndType(string ApplicationId, string type);
        Task<CandidateProfileViewModel> GetCandProfileDetails(string candidateProfileId);
        Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByJobAndType(string JobAdvertisementId, string type);
        Task<List<ApplicationExperienceViewModel>> GetListByApplication(string candidateProfileId);
        Task<List<ApplicationExperienceByCountryViewModel>> GetApplicationExpByCountryList(string candidateProfileId);
        Task<List<ApplicationExperienceByJobViewModel>> GetApplicationExpByJobList(string candidateProfileId);
        Task<List<ApplicationeExperienceByNatureViewModel>> GetApplicationExpByNatureList(string candidateProfileId);
        Task<List<ApplicationExperienceBySectorViewModel>> GetApplicationListBySector(string candidateProfileId);
        Task<List<ApplicationProjectViewModel>> GetApplicationProjectList(string candidateProfileId);
        Task<List<ApplicationEducationalViewModel>> GetApplicantsEducationInfoList(QualificationTypeEnum qualificationType, string candidateProfileId);
        Task<List<CandidateEducationalViewModel>> GetListByCandidate(QualificationTypeEnum qualificationType, string candidateProfileId);
        Task<List<ApplicationComputerProficiencyViewModel>> GetApplicationCompProfList(string candidateProfileId);
        Task<List<CandidateComputerProficiencyViewModel>> GetCandidateCompProfList(string candidateProfileId);
        Task<List<ApplicationLanguageProficiencyViewModel>> GetApplicationLangProfList(string candidateProfileId);
        Task<List<CandidateLanguageProficiencyViewModel>> GetCandidateLangProfList(string candidateProfileId);
        Task<List<ApplicationDrivingLicenseViewModel>> GetLicenseListByApplication(string candidateProfileId);
        Task<List<CandidateDrivingLicenseViewModel>> GetLicenseListByCandidate(string candidateProfileId);
        Task<List<ApplicationReferencesViewModel>> GetApplicationRefList(string candidateProfileId);

        Task<CandidateProfileViewModel> GetCandidateById(string id);
        Task<IdNameViewModel> GetNationalityIdByName();
        Task<CandidateProfileViewModel> GetCandidateByUser();
        Task<CandidateProfileViewModel> GetCandidateByEmail();
        Task<CandidateProfileViewModel> GetCandidateByPassportNo(string passportNo);
        Task<CandidateExperienceViewModel> GetCandidateExperienceDuration(string candidateProfileId);
        Task<ApplicationViewModel> GetApplicationById(string appId);
        Task<CandidateProfileViewModel> CheckCandExitsByIdnPassportNo(string id, string passportNo);
        Task UpdateApplicationExperienceWhenProfileUpdate(string profileId, string applicationId);
        Task UpdateApplicationEducationWhenProfileUpdate(string profileId, string applicationId);
        Task<CommandResult<ApplicationViewModel>> UpdateApplication(CandidateProfileViewModel model);
        Task<List<ApplicationViewModel>> GetApplicationListByCandidate(string candidateId);
        Task<List<ApplicationViewModel>> GetBookmarksJobList(string jobIds);
        Task<List<ApplicationViewModel>> GetJobAdvertisementByAgency();

        Task<List<ManpowerRecruitmentSummaryViewModel>> GetActiveManpowerRecruitmentSummaryData();
        Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search);
        Task<List<CandidateExperienceViewModel>> GetListByCandidate(string candidateProfileId);
        Task<CandidateProfileViewModel> GetDocumentsByCandidate(string candidateProfileId);
        Task<CandidateProfileViewModel> GetDocumentsByApplication(string applicationId);
        Task<string> UpdateApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state, string BatchId, string JobAddId, string JobId, string OrgId);
        Task<RecBatchViewModel> GetBatchDetailsById(string id);
        Task<string> GenerateNextApplicationNo();
        Task<IdNameViewModel> GetGrade(string Id);



        #endregion
        #region s
        Task<CandidateExperienceByCountryViewModel> GetCandidateExperiencebyCountryDetails(string Id);
        Task<List<CandidateExperienceByCountryViewModel>> ReadCandidateExperiencebyCountry(string candidateProfileId);
        Task<bool> DeleteCandExpByCountry(string NoteId);
        Task<CandidateExperienceByJobViewModel> GetCandidateExperiencebyJobDetails(string Id);
        Task<List<CandidateExperienceByJobViewModel>> ReadCandidateExperiencebyJob(string candidateProfileId);
        Task<bool> DeleteCandExpByJob(string NoteId);
        Task<CandidateExperienceByNatureViewModel> GetCandidateExperiencebyNatureDetails(string Id);
        Task<List<CandidateExperienceByNatureViewModel>> ReadCandidateExperiencebyNature(string candidateProfileId);

        Task<bool> DeleteCandExpByNature(string NoteId);
        Task<CandidateExperienceByOtherViewModel> GetCandidateExperiencebyOtherDetails(string Id);
        Task<List<CandidateExperienceByOtherViewModel>> ReadCandidateExperiencebyOther(string candidateProfileId);
        Task<bool> DeleteCandExpByOther(string NoteId);
        Task<CandidateExperienceBySectorViewModel> GetCandidateExperiencebySectorDetails(string Id);
        Task<List<CandidateExperienceBySectorViewModel>> ReadCandidateExperiencebySector(string candidateProfileId);
        Task<bool> DeleteCandExpBySector(string NoteId);
        Task<CandidateProjectViewModel> GetCandidateExperiencebyProjectDetails(string Id);
      
        Task<bool> DeleteCandExpByProject(string NoteId);
        Task<List<CandidateProjectViewModel>> ReadCandidateExperiencebyProject(string candidateProfileId);
        Task<CandidateReferencesViewModel> GetCandidateReferenceDetails(string Id);
        Task<List<CandidateReferencesViewModel>> ReadCandidateReference(string candidateProfileId);
        Task<bool> DeleteCandRefer(string NoteId);
        Task<CandidateEducationalViewModel> GetCandidateEducational(string Id);
        Task<bool> DeleteCandidateEducational(string NoteId);
        Task<List<CandidateEducationalViewModel>> ReadCandidateEducational(QualificationTypeEnum qualificationType, string candidateProfileId);
        Task<bool> DeleteCandidateComputerProf(string NoteId);
        Task<CandidateComputerProficiencyViewModel> GetCandidateComputerProf(string Id);
        Task<bool> DeleteCandidateLanguageProf(string NoteId);
        Task<CandidateLanguageProficiencyViewModel> GetCandidateLanguageProf(string Id);
        Task<bool> DeleteCandidateDrivingLicense(string NoteId);
        Task<CandidateDrivingLicenseViewModel> GetCandidateDrivingLicense(string Id);
        Task<List<ApplicationJobCriteriaViewModel>> GetCriteriaData(string ApplicationId, string type);
        Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaList(string ApplicationId, string type);
        Task<ApplicationViewModel> GetAppDetailsById(string appId);
        Task<CandidateEducationalViewModel> GetCandidateEducationalbyId(string Id);
        Task<CandidateExperienceViewModel> GetCandidateExperiencebyId(string Id);
        Task<RecApplicationViewModel> GetCandidateAppDetails(string canId, string jobId);
        Task<List<CandidateProfileViewModel>> GetStaffList(string id);
        Task<bool> DeleteBeneficiary(string NoteId);
        Task<ApplicationBeneficiaryViewModel> GetBeneficiaryDataByid(string Id, string referId);
        Task<List<ApplicationBeneficiaryViewModel>> ReadBeneficiaryData(string referId);

        #endregion s

        #region p
        Task<CandidateExperienceViewModel> GetCandidateExperience(string Id);
        Task<bool> DeleteCandidateExperience(string NoteId);
        Task<List<CandidateExperienceViewModel>> ReadCandidateExperience(string candidateProfileId);
        Task<List<CandidateProfileViewModel>> GetWorkerList(string id);
        Task<bool> CreateWorkerCandidateAndApplication(List<WorkerCandidateViewModel> list);
        #endregion
    }
}
