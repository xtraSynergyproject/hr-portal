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
    public interface IApplicationExperienceBusiness : IBusinessBase<ApplicationExperienceViewModel, ApplicationExperience>
    {
        Task<IList<ApplicationExperienceViewModel>> GetListByApplication(string candidateProfileId);
    }
}
