using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;
using CMS.UI.ViewModel;

namespace CMS.Business.Interface.BusinessScript.Form.General.CoreHR
{
    public interface IFormGeneralCoreHrPreScript
    {
        Task<CommandResult<FormTemplateViewModel>> ValidateFinancialYearStartEndDate(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
