using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IApplicationBusiness : IBusinessBase<ApplicationViewModel, Application>
    {
        Task<IList<IdNameViewModel>> GetListOfValueByType(string Type);
        Task UpdateApplicationState(string users, string type);
        Task UpdateApplicationtStatus(string users, string type);

        Task<IList<IdNameViewModel>> GetCountryIdNameList();
        Task<IList<IdNameViewModel>> GetNationalityIdNameList();
        Task<string> UpdateApplicationBatch(string applicationId, string BatchId);
        Task<string> UpdateApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state, string BatchId, string JobAddId, string JobId,string orgId);
        Task<IList<ApplicationViewModel>> GetCandiadteShortListDataByHR(ApplicationSearchViewModel search);
        Task<CandidateProfileViewModel> GetApplicationDetails(string applicationId);
        Task<CandidateProfileViewModel> GetApplicationDetails(string applicationId, string jobAdvId);
        Task<IList<ApplicationViewModel>> GetJobAdvertismentState(string jobadvtid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode,string status);
        Task<IList<ApplicationViewModel>> GetWorkPool1Data(string workerbatchid);

        Task<IList<IdNameViewModel>> GetListOfValueAction(string Type, string Code);
        Task UpdateJobAdvtApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state);
        Task<IList<ApplicationViewModel>> GetCandiadteShortListApplicationData(ApplicationSearchViewModel search);
        Task<ApplicationViewModel> GetCandiadteShortListApplicationDataByApplication(string applicationId, string tempCode);
        Task<CandidateProfileViewModel> GetDocumentsByApplication(string applicationId);
        Task<ApplicationViewModel> GetOfferDetails(string applicationId);
        Task<CommandResult<ApplicationViewModel>> CreateApplication(CandidateProfileViewModel model, BatchViewModel batch);
        Task<ApplicationViewModel> GetNameById(string applicationId);
        Task<CommandResult<ApplicationViewModel>> UpdateApplication(CandidateProfileViewModel model);
        Task<ApplicationViewModel> GetApplicationEvaluationDetails(string applicationId);
        Task<ApplicationViewModel> GetWSADetails(string applicationId);
        Task<IdNameViewModel> GetGrade(string Id);
        Task<IdNameViewModel> GetNationality(string Id);
        Task<IdNameViewModel> GetTitle(string Id);
        Task<IdNameViewModel> GetUserName(string Id);
        Task<List<ApplicationViewModel>> GetApplicationListByCandidate(string id);
        Task<IList<ApplicationViewModel>> GetJobAdvertismentApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode, string templateCodeOther);
        Task<IList<ApplicationStateTrackDetailViewModel>> GetAppStateTrackDetailsByCand(string applicationId);
        Task<ApplicationViewModel> GetApplicationDeclarationData(string applicationId);
        Task<ApplicationViewModel> GetConfidentialAgreementDetails(string applicationId);
        Task<ApplicationViewModel> GetCompetenceMatrixDetails(string applicationId);

        Task<string> CreateApplicationStatusTrack(string applicationId, string statusCode = null, string taskReferenceId = null);

        Task<ApplicationViewModel> GetApplicationDetail(string applicationId);
        Task<IdNameViewModel> GetApplicationStateByCode(string code);
        Task<IList<ApplicationViewModel>> GetWorkerPoolData(ApplicationSearchViewModel search);
        Task<IList<ApplicationViewModel>> GetWorkerPoolApplication(string jobadvtid, string orgId, string jobadvtstate, string templateCode);
        Task<IList<ApplicationViewModel>> GetWorkerPoolBatchData(string batchid);
        Task<IList<ApplicationViewModel>> GetBatchData(string batchid);
        Task<IList<ApplicationViewModel>> GetTotalApplication(string jobid, string orgId, string jobadvtstate, string tempcode, string nexttempcode, string visatempcode);
        Task<IList<IdNameViewModel>> GetIdNameHMOrganization(string userId);
        Task<bool> CreateStaffCandidateAndApplication(StaffCandidateSubmitViewModel model);
        Task<bool> CreateWorkerCandidateAndApplication(WorkerCandidateSubmitViewModel model);
        Task<IdNameViewModel> GetHiringManagerByApplication(string applicationId);
        Task<IdNameViewModel> GetHeadOfDepartmentByApplication(string applicationId);
        Task<ApplicationViewModel> GetApplicationForJobAdv(string applicationId);
        Task<ApplicationViewModel> GetStaffJoiningDetails(string applicationId);
        Task<IList<IdNameViewModel>> GetJobdescription(string JobId);
        Task<JobDescriptionViewModel> GetJobdescriptionById(string Id);
        Task<IdNameViewModel> GetApplicationStatusByCode(string code);
        Task<IdNameViewModel> GetUserSign();

        Task<ApplicationViewModel> GetAppDetails(string applicationId);
        Task<IList<ApplicationViewModel>> GetViewApplicationDetails(string jobId, string orgId);
        Task<IList<ApplicationViewModel>> GetApplicationPendingTask(string userId);
        Task<IList<ApplicationViewModel>> GetApplicationWorkerPoolNotUnderApproval();
        Task<List<TreeViewViewModel>> GetHMTreeList(string id);

        Task<IList<ApplicationViewModel>> GetCandidateReportData();
        Task<IList<ApplicationViewModel>> GetDirictHiringData();
    }
}
