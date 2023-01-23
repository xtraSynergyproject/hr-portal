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
    public interface ICandidateProfileBusiness : IBusinessBase<CandidateProfileViewModel, CandidateProfile>
    {
        Task<CandidateProfileViewModel> GetCandProfileDetails(string candidateProfileId);
        Task<CandidateProfileViewModel> GetDocumentsByCandidate(string candidateProfileId);
        Task<CandidateProfileViewModel> GetCandidateByUser();
        Task<List<CandidateProfileViewModel>> GetStaffList(string userId);
        Task<List<CandidateProfileViewModel>> GetWorkerList(string userId);
        Task<CommandResult<CandidateProfileViewModel>> CreateCandidate(CandidateProfileViewModel model, bool autoCommit = true);
        Task<CommandResult<CandidateProfileViewModel>> EditCandidate(CandidateProfileViewModel model, bool autoCommit = true);

        Task<Tuple<CandidateProfileViewModel, bool>> IsCandidateProfileFilled();

        Task<CandidateProfileViewModel> UpdateCandidateProfileDetails(CandidateProfileViewModel model);
        Task<CandidateProfileViewModel> GetCRPFEmployeeDetails(string employeeId);

    }
}
