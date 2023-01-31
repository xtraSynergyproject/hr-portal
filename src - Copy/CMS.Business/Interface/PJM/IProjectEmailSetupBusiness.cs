using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IProjectEmailSetupBusiness : IBusinessBase<ProjectEmailSetupViewModel, ProjectEmailSetup>
    {
        Task<IList<IdNameViewModel>> GetProjectsList();
        Task<IList<ProjectEmailSetupViewModel>> GetEmailSetupList();
        Task<IList<ProjectEmailSetupViewModel>> ReadEmailSetupUsers();
        Task<long> GetEmailSetupTotalCount(string id);
        Task<ProjectEmailSetupViewModel> UpdateEmailSetupCount(string id, long count);
        Task<IList<ProjectEmailSetupViewModel>> ReadEmailSetupByProjectId(string id);
        Task<IList<ProjectEmailSetupViewModel>> GetSmtpEmailId();
        Task<bool> TestEmail(string id);
    }
}
