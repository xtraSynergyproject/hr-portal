using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ISalesBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        Task<CommandResult<NoteTemplateViewModel>> GenerateLicense(ServiceTemplateViewModel svm);
        Task<LicenseViewModel> GetSelfLicense(string machineName);
        Task<bool> EvaluateLicense(string licenseKey, string machineName);
        Task<LicenseViewModel> GetLicenseDetails(string licenseKey, string licensePrivateKey, string machineName);
    }
}
