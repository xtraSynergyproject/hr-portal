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
    public interface IApplicationExperienceBusiness : IBusinessBase<ApplicationExperienceViewModel, ApplicationExperience>
    {
        Task<IList<ApplicationExperienceViewModel>> GetListByApplication(string candidateProfileId);
    }
}
