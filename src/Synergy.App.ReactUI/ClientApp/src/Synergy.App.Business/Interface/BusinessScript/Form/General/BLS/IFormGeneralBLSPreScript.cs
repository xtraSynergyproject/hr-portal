using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Form.General.BLS
{
    public interface IFormGeneralBLSPreScript
    {
        Task<CommandResult<FormTemplateViewModel>> ValidateDetailsForVACExecutive(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
