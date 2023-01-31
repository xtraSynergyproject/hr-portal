using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface ICandidateComputerProficiencyBusiness : IBusinessBase<CandidateComputerProficiencyViewModel, CandidateComputerProficiency>
    {
        Task<IList<CandidateComputerProficiencyViewModel>> GetListByCandidate(string candidateProfileId);
    }
}
