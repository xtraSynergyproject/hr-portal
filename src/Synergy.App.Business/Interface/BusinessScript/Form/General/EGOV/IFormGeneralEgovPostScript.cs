using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Form.General.EGOV
{
    public interface IFormGeneralEgovPostScript
    {
        Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInCollectorForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInDriverForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInComplaintOperatorForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInVehicleForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInTransferStationForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInWTPForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> CreateUserSubLogin(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
