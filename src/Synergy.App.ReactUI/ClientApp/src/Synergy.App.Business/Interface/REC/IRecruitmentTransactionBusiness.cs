using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;
using System.Data;

namespace Synergy.App.Business
{
    public interface IRecruitmentTransactionBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        Task<IList<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userroleCode);

        #region j
        Task<IList<JobAdvertisementViewModel>> GetManpowerUniqueJobData();
        Task<IList<JobAdvertisementViewModel>> GetJobAdvertisement(string jobid, string rolid, StatusEnum status);
        Task<IList<JobDescriptionCriteriaViewModel>> GetJobDescCriteriaList(string type, string jobdescid);
        Task<JobDescriptionViewModel> GetJobDescriptionData(string jobId);
        Task<IList<JobCriteriaViewModel>> GetJobCriteriaList(string type, string jobadvtid);
        Task<JobAdvertisementViewModel> GetJobIdNameListByJobAdvertisement(string JobId);
        Task<JobAdvertisementViewModel> GetJobAdvertisementData(string id);
        Task<List<RecHeadOfDepartmentViewModel>> GetHODListByOrgId(string orgId);
        Task<List<RecHiringManagerViewModel>> GetHMListByOrgId(string orgId);
        Task<IList<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentSummaryData();
        Task UpdateApplicationtStatus(string users, string type);
        Task<RecBatchViewModel> GetBatchApplicantCount(string Id);
        Task UpdateStatus(string batchId, string code);
        Task<JobAdvertisementViewModel> GetState(string serId);
        Task<IList<ApplicationViewModel>> GetViewApplicationDetails(string jobId, string orgId);
        Task<IList<IdNameViewModel>> GetGradeIdNameList(string code=null);
        Task<IList<RecCandidateElementInfoViewModel>> GetElementData(string appid);
        Task<IList<RecCandidatePayElementViewModel>> GetElementPayData(string appid);
        Task<RecApplicationViewModel> GetApplicationDetailsById(string appId);
        Task<RecApplicationViewModel> GetAppointmentDetailsById(string appId);
        Task<long> GenerateFinalOfferSeq();
        Task<CandidateProfileViewModel> GetApplicationforOfferById(string appId);
        Task<IList<RecApplicationViewModel>> GetTotalApplication(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode);

        Task<RecDashboardViewModel> GetManpowerRequirement(string jobId, string orgId);
        Task<IList<RecApplicationViewModel>> GetJobAdvertismentState(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode, string status);
        Task<IList<RecApplicationViewModel>> GetDirictHiringData(string jobid, string orgId);
        Task<IList<RecApplicationViewModel>> GetJobAdvertismentApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode, string templateCodeOther);
        Task<IList<ApplicationStateTrackDetailViewModel>> GetAppStateTrackDetailsByCand(string applicationId);
        Task<List<HiringManagerViewModel>> GetHiringManagersList();
        Task<RecApplicationViewModel> GetApplicationEvaluationDetails(string applicationId);
        Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationDetails(string applicationId);
        Task<List<RecCandidateEvaluationViewModel>> GetCandidateEvaluationTemplateData();
        Task<IList<RecApplicationViewModel>> GetCandiadteShortListApplicationData(ApplicationSearchViewModel search);
        Task<List<RecBatchViewModel>> GetBatchHmData(string jobid, string orgId, string HmId, BatchTypeEnum type, string batchId);
        Task<string> CreateApplicationStatusTrack(string applicationId, string statusCode = null, string taskReferenceId = null);
        Task<IList<RecApplicationViewModel>> GetWorkerPoolBatchData(string batchid);
        Task<IList<RecApplicationViewModel>> GetBatchData(string batchid);

        #endregion j

        #region d
        Task<IList<RecTaskViewModel>> GetPendingTaskListByUserId(string userId);
        Task<DataTable> GetJobByOrgUnit(string userId);
        Task<IList<IdNameViewModel>> GetIdNameList(string type);
        Task<IList<IdNameViewModel>> GetCountryIdNameList();
        Task<DataTable> GetTaskByOrgUnit(string userId, string userroleId);
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
        #endregion d

        #region Batch
        Task<RecBatchViewModel> GetBatchDataById(string batchId);
        Task<List<ManpowerRecruitmentSummaryViewModel>> GetBatchDataByJobId(string jobId);
        Task<string> GenerateNextBatchNameUsingOrg(string Name);
        Task<List<ManpowerRecruitmentSummaryViewModel>> GetManpowerRecruitmentList(string jobId, string orgId);
        Task<IList<IdNameViewModel>> GetBatchIdNameList(string JobAddId, BatchTypeEnum batchType, string orgId);
        Task<List<ApplicationStateCommentViewModel>> GetApplicationStateCommentData(string appId, string appStateId);
        Task<IList<JobAdvertisementViewModel>> GetJobIdNameListForSelection();
        Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search);
        Task<List<RecBatchViewModel>> GetBatchData(string jobid, BatchTypeEnum type, string orgId);
        #endregion
        Task<IdNameViewModel> GetNationalitybyId(string id);
        Task<IdNameViewModel> GetTitlebyId(string id);
        Task<RecApplicationViewModel> GetApplicationDeclarationData(string applicationId);
        Task<RecApplicationViewModel> GetConfidentialAgreementDetails(string applicationId);
        Task<RecApplicationViewModel> GetCompetenceMatrixDetails(string applicationId);
        Task<IdNameViewModel> GetUserSign();
        Task<IdNameViewModel> GetJobManpowerType(string Id);
        Task<RecApplicationViewModel> GetStaffJoiningDetails(string applicationId);
        Task TriggerIntentToOffer(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerWorkerAppointment(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerFinalOffer(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerOverseasBusinessVisa(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerOverseasWorkVisa(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerTravelling(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerVisaTransfer(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerWorkPermit(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerStaffJoining(ServiceTemplateViewModel viewModel, dynamic udf);
        Task TriggerWorkerJoining(ServiceTemplateViewModel viewModel, dynamic udf);
        Task<IList<RecTaskViewModel>> GetRecTaskList(string search);
        Task<IdNameViewModel> GetCountrybyId(string id);
        Task<RecApplicationViewModel> GetApplicationForJobAdv(string applicationId);
        Task<List<JobAdvertisementViewModel>> GetJobAdvertisementListByJobId(string jobId);
        Task<IList<JobAdvertisementViewModel>> GetSelectedJobAdvertisement(string vacancyId);
        Task TriggerCRPFAppointment(ServiceTemplateViewModel viewModel, dynamic udf);
        Task<IList<JobAdvertisementViewModel>> GetJobAdvList();
        Task<IList<RecApplicationViewModel>> GetJobApplicationList();
        Task<IList<IdNameViewModel>> GetExamCenter();
        Task<MedFitCertificateViewModel> GetMedicalFitnessData(string appId);
        Task<IList<ApplicationViewModel>> GetResultData(string eId);
    }
}
