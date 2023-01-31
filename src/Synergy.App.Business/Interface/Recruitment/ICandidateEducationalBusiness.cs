using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.Business
{
    public interface ICandidateEducationalBusiness : IBusinessBase<CandidateEducationalViewModel, CandidateEducational>
    {
        Task<IList<CandidateEducationalViewModel>> GetListByCandidate(QualificationTypeEnum qualificationType,string candidateProfileId);
    }
}
