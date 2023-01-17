using CMS.Business.Interface.BusinessScript.Form.General.CoreHR;
using CMS.Common;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business.Implementations.BusinessScript.Form.General.CoreHR
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
