using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ICandidateDrivingLicenseBusiness : IBusinessBase<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>
    {
        Task<IList<CandidateDrivingLicenseViewModel>> GetLicenseListByCandidate(string candidateProfileId);
        Task<IList<IdNameViewModel>> GetCountryListData();
    }
}
