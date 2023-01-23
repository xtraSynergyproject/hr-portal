using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ICandidateExperienceByJobBusiness : IBusinessBase<CandidateExperienceByJobViewModel, CandidateExperienceByJob>
    {
        Task<IList<CandidateExperienceByJobViewModel>> GetListByCandidate(string candidateProfileId);
    }
}
