using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Service.General.General
{
    public interface IServiceGeneralTECProcessPostScript
    {
        Task<CommandResult<ServiceTemplateViewModel>> CreateChildServices(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> TriggerNextServices(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
