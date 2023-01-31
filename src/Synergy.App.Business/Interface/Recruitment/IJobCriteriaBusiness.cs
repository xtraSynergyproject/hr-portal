using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IJobCriteriaBusiness : IBusinessBase <JobCriteriaViewModel, JobCriteria>
    {
        Task<IList<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByJobAndType(string JobAdvertisementId, string type);
        Task<IList<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByApplicationIdAndType(string ApplicationId, string type);
    }
}
