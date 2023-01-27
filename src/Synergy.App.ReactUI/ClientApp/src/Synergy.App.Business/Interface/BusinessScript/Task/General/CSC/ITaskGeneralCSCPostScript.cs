using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.CSC
{
    public interface ITaskGeneralCSCPostScript
    {
        Task<CommandResult<TaskTemplateViewModel>> SetProvisionCertificateTrue(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> SetCertificateTrue(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        
    }
}
