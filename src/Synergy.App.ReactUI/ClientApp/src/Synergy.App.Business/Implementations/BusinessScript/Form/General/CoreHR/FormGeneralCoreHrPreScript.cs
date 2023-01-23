using Synergy.App.Business.Interface.BusinessScript.Form.General.CoreHR;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business.Implementations.BusinessScript.Form.General.CoreHR
{
    class FormGeneralCoreHrPreScript : IFormGeneralCoreHrPreScript
    {
        public async Task<CommandResult<FormTemplateViewModel>> ValidateFinancialYearStartEndDate(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            var res = await _business.ValidateFinancialYearStartDateandEndDate(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Financial Year is Already Added for these Dates");
                return CommandResult<FormTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }
     
    }
}
