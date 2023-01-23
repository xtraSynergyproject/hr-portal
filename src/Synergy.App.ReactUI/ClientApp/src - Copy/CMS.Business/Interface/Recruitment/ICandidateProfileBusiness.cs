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
    public interface ICandidateProfileBusiness : IBusinessBase<CandidateProfileViewModel, CandidateProfile>
    {
        Task<CandidateProfileViewModel> GetCandProfileDetails(string candidateProfileId);
        Task<CandidateProfileViewModel> GetDocumentsByCandidate(string candidateProfileId);
        Task<CandidateProfileViewModel> GetCandidateByUser();
        Task<List<CandidateProfileViewModel>> GetStaffList(string userId);
        Task<List<CandidateProfileViewModel>> GetWorkerList(string userId);
        Task<CommandResult<CandidateProfileViewModel>> CreateCandidate(CandidateProfileViewModel model);
        Task<CommandResult<CandidateProfileViewModel>> EditCandidate(CandidateProfileViewModel model);

        Task<Tuple<CandidateProfileViewModel, bool>> IsCandidateProfileFilled();

        Task<CandidateProfileViewModel> UpdateCandidateProfileDetails(CandidateProfileViewModel model);
    }
}
