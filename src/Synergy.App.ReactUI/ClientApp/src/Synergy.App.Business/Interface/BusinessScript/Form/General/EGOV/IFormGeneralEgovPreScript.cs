using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;
using Synergy.App.ViewModel;

namespace Synergy.App.Business.Interface.BusinessScript.Form.General.EGOV
{
    public interface IFormGeneralEgovPreScript
    {
        Task<CommandResult<FormTemplateViewModel>> ValidateNeedsWantsTimelineFromDateandToDate(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> ValidateAssetFeeTimelineFromDateandToDate(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<FormTemplateViewModel>> ValidateEnforcementNameAndEmail(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);

    }
}
