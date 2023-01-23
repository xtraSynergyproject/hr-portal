using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Service.General.CoreHR
{
    public interface IServiceGeneralCoreHRPreScript
    {
        Task<CommandResult<ServiceTemplateViewModel>> ValidateLeaveStartEndDate(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> ValidatePersonEmailId(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> ValidateBusinessTrip(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> ValidateBusinessTripClaim(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
