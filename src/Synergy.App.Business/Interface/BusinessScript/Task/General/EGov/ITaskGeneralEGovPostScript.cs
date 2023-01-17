using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Task.General.EGov
{
    public interface ITaskGeneralEGovPostScript
    {
        Task<CommandResult<TaskTemplateViewModel>> UpdateRentalRenewalDate(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> UpdateRentalStatus(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<TaskTemplateViewModel>> UpdateJSCPaymentDetails(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
