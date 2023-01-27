using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Service.General.EGOV
{
    public interface IServiceGeneralEGOVPreLoadScript
    {
        Task<CommandResult<ServiceTemplateViewModel>> FillSanitationAmount(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);

    }
}
