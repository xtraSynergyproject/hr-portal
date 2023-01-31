using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IRecQueryBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        #region N
        Task<List<ListOfValueViewModel>> GetJobAdvertisementListWithCount(string agency);
        Task<List<JobAdvertisementViewModel>> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerTypeId, string agency);
        Task<JobAdvertisementViewModel> GetNameById(string jobAdvId);
        Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string JobId);
        Task<Tuple<CandidateProfileViewModel, bool>> IsCandidateProfileFilled();
        Task<List<CandidateExperienceViewModel>> GetListByCandidate(string candidateProfileId);
        Task<List<CandidateEducationalViewModel>> GetListByCandidate(QualificationTypeEnum qualificationType, string candidateProfileId);
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
        Task UpdateTable(string type, string applicationId);
        Task<List<CandidateExperienceViewModel>> GetCandidateExpByCandidateId(string candidateId);
        Task<List<CandidateExperienceByCountryViewModel>> GetCandidateExpCountryByCandidateId(string candidateId);
        Task<List<CandidateExperienceBySectorViewModel>> GetCandidateExpSectorByCandidateId(string candidateId);
        Task<List<CandidateExperienceByNatureViewModel>> GetCandidateExpNatureByCandidateId(string candidateId);
        Task<List<CandidateExperienceByJobViewModel>> GetCandidateExpJobByCandidateId(string candidateId);
        Task<List<CandidateExperienceByOtherViewModel>> GetCandidateExpOtherByCandidateId(string candidateId);
        Task<List<CandidateReferencesViewModel>> GetCandidateReferencesByCandidateId(string candidateId);
        Task<List<CandidateProjectViewModel>> GetCandidateProjectByCandidateId(string candidateId);
        Task<List<CandidateDrivingLicenseViewModel>> GetCandidateDrivingLicenceByCandidateId(string candidateId);
        Task<List<CandidateLanguageProficiencyViewModel>> GetCandidateLangProfByCandidateId(string candidateId);
        Task<List<CandidateComputerProficiencyViewModel>> GetCandidateCompProfByCandidateId(string candidateId);
        Task<List<CandidateEducationalViewModel>> GetCandidateEduByCandidateId(string candidateId);
        Task<List<ApplicationViewModel>> GetApplicationListByCandidate(string candidateId);
        Task<List<ApplicationViewModel>> GetBookmarksJobList(string jobIds);
        Task<List<ApplicationViewModel>> GetJobAdvertisementByAgency();
        Task<CandidateProfileViewModel> GetDocumentsByCandidate(string candidateProfileId);
        Task<CandidateProfileViewModel> GetDocumentsByApplication(string applicationId);
        Task<long> GenerateNextDatedApplicationId();
        Task<IdNameViewModel> GetGrade(string Id);


        #endregion
        #region s
        Task<bool> DeleteCandExpByCountry(string NoteId);
        Task<CandidateExperienceByCountryViewModel> GetCandidateExperiencebyCountryDetails(string Id);
        Task<List<CandidateExperienceByCountryViewModel>> ReadCandidateExperiencebyCountry(string candidateProfileId);
        Task<bool> DeleteCandExpByJob(string NoteId);
        Task<CandidateExperienceByJobViewModel> GetCandidateExperiencebyJobDetails(string Id);
        Task<List<CandidateExperienceByJobViewModel>> ReadCandidateExperiencebyJob(string candidateProfileId);
        Task<bool> DeleteCandExpByNature(string NoteId);
        Task<CandidateExperienceByNatureViewModel> GetCandidateExperiencebyNatureDetails(string Id);
        Task<List<CandidateExperienceByNatureViewModel>> ReadCandidateExperiencebyNature(string candidateProfileId);

        Task<bool> DeleteCandExpByOther(string NoteId);
        Task<CandidateExperienceByOtherViewModel> GetCandidateExperiencebyOtherDetails(string Id);
        Task<List<CandidateExperienceByOtherViewModel>> ReadCandidateExperiencebyOther(string candidateProfileId);
        Task<bool> DeleteCandExpBySector(string NoteId);
        Task<CandidateExperienceBySectorViewModel> GetCandidateExperiencebySectorDetails(string Id);
        Task<List<CandidateExperienceBySectorViewModel>> ReadCandidateExperiencebySector(string candidateProfileId);
        Task<bool> DeleteCandExpByProject(string NoteId);
        Task<CandidateProjectViewModel> GetCandidateExperiencebyProjectDetails(string Id);
        Task<List<CandidateProjectViewModel>> ReadCandidateExperiencebyProject(string candidateProfileId);
        Task<List<CandidateReferencesViewModel>> ReadCandidateReference(string candidateProfileId);
        Task<bool> DeleteCandRefer(string NoteId);
        Task<CandidateReferencesViewModel> GetCandidateReferenceDetails(string Id);
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
        Task<List<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaList(string applicationId, string type);
        Task<ApplicationViewModel> GetAppDetailsById(string appId);
        Task<IdNameViewModel> GetNationalitybyId(string id);
        Task<IdNameViewModel> GetTitlebyId(string id);
        Task<RecApplicationViewModel> GetApplicationDeclarationData(string applicationId);
        Task<RecApplicationViewModel> GetConfidentialAgreementDetails(string applicationId);
        Task<RecApplicationViewModel> GetCompetenceMatrixDetails(string applicationId);
        Task<IdNameViewModel> GetUserSign();
        Task<IdNameViewModel> GetJobManpowerType(string Id);
        Task<RecApplicationViewModel> GetStaffJoiningDetails(string applicationId);
        Task<RecApplicationViewModel> GetCandidateAppDetails(string canId, string jobId);
        Task<List<CandidateProfileViewModel>> GetStaffList(string id);
        Task<CandidateEducationalViewModel> GetCandidateEducationalbyId(string Id);
        Task<CandidateExperienceViewModel> GetCandidateExperiencebyId(string Id);

        #endregion s


        #region p
        Task<CandidateExperienceViewModel> GetCandidateExperience(string Id);
        Task<bool> DeleteCandidateExperience(string NoteId);
        Task<List<CandidateExperienceViewModel>> ReadCandidateExperience(string candidateProfileId);
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameList();
        #endregion p

        #region j
        Task<IList<JobAdvertisementViewModel>> GetManpowerUniqueJobData();
        Task<IList<JobDescriptionCriteriaViewModel>> GetJobDescCriteriaList(string type, string jobdescid);
        Task<JobDescriptionViewModel> GetJobDescriptionData(string jobId);
        Task<IList<JobCriteriaViewModel>> GetJobCriteriaList(string type, string jobadvtid);
        Task<JobAdvertisementViewModel> GetJobAdvertisementData(string id);
        Task<IList<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentSummaryData();
        Task<JobAdvertisementViewModel> GetState(string serId);
        Task<IList<ApplicationViewModel>> GetViewApplicationDetails(string jobId, string orgId);
        Task<IList<IdNameViewModel>> GetGradeIdNameList(string code=null);
        Task<IList<RecCandidateElementInfoViewModel>> GetElementData(string appid);
        Task<IList<RecCandidatePayElementViewModel>> GetPayElementData(string appid);
        Task<RecApplicationViewModel> GetApplicationDetailsById(string appId);
        Task<RecApplicationViewModel> GetAppointmentDetailsById(string appId);
        Task<long> GenerateFinalOfferSeq();
        Task<CandidateProfileViewModel> GetApplicationforOfferById(string appId);
        Task<IList<RecApplicationViewModel>> GetTotalApplication(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode);
        Task<RecDashboardViewModel> GetManpowerRequirement(string jobId, string orgId);
        Task<IList<RecApplicationViewModel>> GetJobAdvertismentState(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode, string status);
        Task<IList<RecApplicationViewModel>> GetDirictHiringData(string jobid, string orgId);
        Task<IList<RecApplicationViewModel>> GetJobAdvertismentApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode=null, string templateCodeOther=null);
        Task<IList<ApplicationStateTrackDetailViewModel>> GetAppStateTrackDetailsByCand(string applicationId);
        Task<IList<ApplicationStateTrackDetailViewModel>> GetTaskCommentsList(string applicationId);
        Task<List<HiringManagerViewModel>> GetHiringManagersList();
        Task<RecApplicationViewModel> GetApplicationEvaluationDetails(string applicationId);
        Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationDetails(string applicationId);
        Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationTemplateData();
        Task<IList<RecApplicationViewModel>> GetCandiadteShortListApplicationData();
        Task<List<RecBatchViewModel>> GetBatchHmData(string jobid, string orgId, string HmId, BatchTypeEnum type, string batchId);
        Task<IList<RecApplicationViewModel>> GetWorkerPoolBatchData(string batchid);
        Task<IList<RecApplicationViewModel>> GetBatchData(string batchid);

        #endregion j

        #region d
        Task<IList<RecTaskViewModel>> GetPendingTaskListByUserId(string userId);
        Task<DataTable> GetJobByOrgUnit(string userId);
        Task<IList<IdNameViewModel>> GetIdNameList(string Type);
        Task<DataTable> GetTaskByOrgUnit(string userId, string userroleId);
        Task<IList<IdNameViewModel>> GetCountryIdNameList();
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameDashboardList();
        Task<List<IdNameViewModel>> GetOrgByJobAddId(string JobId);
        Task<IdNameViewModel> GetHmOrg(string userId);
        Task<IdNameViewModel> GetHODOrg(string userId);
        Task<JobAdvertisementViewModel> GetRecruitmentDashobardCount(string orgId);
        Task<IList<System.Dynamic.ExpandoObject>> GetPendingTaskDetailsForUser(string userId, string orgId, string userRoleCodes);
        Task<RecruitmentDashboardViewModel> GetManpowerRecruitmentSummaryByOrgJob(string organizationId, string jobId, string permission = "");
        Task<IList<ApplicationViewModel>> GetApplicationPendingTask(string userId);
        Task<IList<ApplicationViewModel>> GetApplicationWorkerPoolNotUnderApproval();
        Task<IdNameViewModel> GetJobNameById(string jobId);
        Task<List<IdNameViewModel>> GetJobIdNameDataList();
        Task<IdNameViewModel> GetApplicationStateByCode(string stateCode);
        Task<List<CandidateProfileViewModel>> GetWorkerList(string id);
        #endregion d
        #region NH
        Task<IList<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userroleCode);

        #endregion NH
        Task UpdateStatus(string batchId, string code);
        Task UpdateApplicationtStatus(string users, string type);
        Task UpdateApplicationState(string users, string type);
        Task<RecApplicationViewModel> GetApplicationDetail(string applicationId);
        Task<RecBatchViewModel> GetBatchApplicantCount(string Id);
        Task<List<RecBatchViewModel>> GetBatchData(string jobid, BatchTypeEnum type, string orgId);
         Task<RecBatchViewModel> GetBatchDataById(string batchId);
        Task<List<ManpowerRecruitmentSummaryViewModel>> GetBatchDataByJobId(string jobId);
        Task<List<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentList(string jobId, string orgId);
         Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search);
        Task<List<ApplicationStateCommentViewModel>> GetApplicationStateCommentData(string appId, string appStateId);
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameListForSelection();
        Task<IList<IdNameViewModel>> GetBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId);
        Task<IList<RecBatchViewModel>> GenerateNextBatchNo(string Name);
        Task<List<RecHeadOfDepartmentViewModel>> GetHODListByOrgId(string orgId);
        Task<List<RecHiringManagerViewModel>> GetHMListByOrgId(string orgId);
        Task<List<ManpowerRecruitmentSummaryViewModel>> GetActiveManpowerRecruitmentSummaryData();
        Task<IList<RecTaskViewModel>> GetRecTaskList(string search);
        Task<IdNameViewModel> GetCountrybyId(string id);
        Task<ApplicationViewModel> GetApplicationDataByCandidateIdandJobId(string Id, string jobId);
        Task<CandidateProfileViewModel> UpdateCandidateProfileDetails(CandidateProfileViewModel model);
        Task<RecBatchViewModel> GetBatchDetailsById(string id);
        Task UpdateApplicationStatusForInterview(dynamic udf, string statusId);
        Task UpdateApplicationBatch(string applicationId, string statusId, string stateId, string batchId, string orgId, string jobId);
        Task<List<ApplicationViewModel>> CandidateReportData();
        Task<List<ApplicationViewModel>> PendingforHM();
        Task<List<ApplicationViewModel>> RejectedforHM();
        Task<List<ApplicationViewModel>> ShortlistforFuture();
        Task<List<ApplicationViewModel>> PendingforUser();
        Task<List<ApplicationViewModel>> PendingforHR();
        Task<List<ApplicationViewModel>> PendingforED();
        Task<bool> DeleteBeneficiary(string NoteId);
        Task<ApplicationBeneficiaryViewModel> GetBeneficiaryDataByid(string Id, string referId);
        Task<List<ApplicationBeneficiaryViewModel>> ReadBeneficiaryData(string referId);
        Task<RecApplicationViewModel> GetApplicationForJobAdv(string applicationId);
        Task<List<JobAdvertisementViewModel>> GetJobAdvertisementListByJobId(string jobId);
        Task UpdateJobAdvertisementStatus(string serviceId);
        Task<IList<RecApplicationViewModel>> GetJobApplicationList();
        Task<IList<IdNameViewModel>> GetExamCenter();
        Task<MedFitCertificateViewModel> GetMedicalFitnessData(string appId);
        Task<IList<ApplicationViewModel>> GetResultData(string eId);

    }
}
