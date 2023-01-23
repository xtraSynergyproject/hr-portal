using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IApplicationJobCriteriaBusiness : IBusinessBase <ApplicationJobCriteriaViewModel, ApplicationJobCriteria>
    {
        Task<IList<ApplicationJobCriteriaViewModel>> GetCriteriaData(string applicationId, string type);
    }
}
