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
    public interface ICandidateEducationalBusiness : IBusinessBase<CandidateEducationalViewModel, CandidateEducational>
    {
        Task<IList<CandidateEducationalViewModel>> GetListByCandidate(QualificationTypeEnum qualificationType,string candidateProfileId);
    }
}
