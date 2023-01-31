using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IApplicationJobCriteriaBusiness : IBusinessBase <ApplicationJobCriteriaViewModel, ApplicationJobCriteria>
    {
        Task<IList<ApplicationJobCriteriaViewModel>> GetCriteriaData(string applicationId, string type);
    }
}
