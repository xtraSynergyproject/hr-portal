using Synergy.App.Business.Interface.BusinessScript.Form.General.EGOV;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business.Implementations.BusinessScript.Form.General.EGOV
{
    class FormGeneralEgovPreScript : IFormGeneralEgovPreScript
    {
        public async Task<CommandResult<FormTemplateViewModel>> ValidateNeedsWantsTimelineFromDateandToDate(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IEGovernanceBusiness>();
            var res = await _business.ValidateNeedsWantsTimelineFromDateandToDate(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Needs Wants Timeline is Already Added for these Dates");
                return CommandResult<FormTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<FormTemplateViewModel>> ValidateAssetFeeTimelineFromDateandToDate(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IEGovernanceBusiness>();
            var res = await _business.ValidateAssetFeeTimelineFromDateandToDate(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Asset fee timeline is already added for these dates");
                return CommandResult<FormTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<FormTemplateViewModel>> ValidateEnforcementNameAndEmail(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IEGovernanceBusiness>();
            bool res=false;
            if (viewModel.DataAction == DataActionEnum.Create)
            {
               res = await _business.GetEmailAndNameEnforcement(udf.SubEmail, udf.SubUserName,null);
            }
            else if (viewModel.DataAction == DataActionEnum.Edit)
            {
                res = await _business.GetEmailAndNameEnforcement(udf.SubEmail, udf.SubUserName,viewModel.RecordId);
            }
            if (res==false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "UserName or Email already exist");
                return CommandResult<FormTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }
    }
}
