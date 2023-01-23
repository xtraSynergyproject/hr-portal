using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;
using Synergy.App.ViewModel;

namespace Synergy.App.Business.Interface.BusinessScript.Form.General.CoreHR
{
    public interface IFormGeneralCoreHrPreScript
    {
        Task<CommandResult<FormTemplateViewModel>> ValidateFinancialYearStartEndDate(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
