using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;

namespace CMS.Business
{
    public interface ICandidateExperienceBusiness : IBusinessBase<CandidateExperienceViewModel, CandidateExperience>
    {
        Task<IList<CandidateExperienceViewModel>> GetListByCandidate(string candidateProfileId);
        Task<CandidateExperienceViewModel> GetCandidateExperienceDuration(string candidateProfileId);
    }
}
