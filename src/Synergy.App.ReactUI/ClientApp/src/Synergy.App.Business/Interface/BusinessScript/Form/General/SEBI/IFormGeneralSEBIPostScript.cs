using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Form.General.SEBI
{
    public interface IFormGeneralSEBIPostScript
    {
        Task<CommandResult<FormTemplateViewModel>> CreateUserAndPortalAccessForStudent(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
