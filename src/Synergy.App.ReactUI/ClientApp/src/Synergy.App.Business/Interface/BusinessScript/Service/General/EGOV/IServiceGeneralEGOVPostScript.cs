using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Service.General.EGOV
{
    public interface IServiceGeneralEGOVPostScript
    {        
        Task<CommandResult<ServiceTemplateViewModel>> UpdateBinBookingDetails(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> UpdateRentalPropertyStatus(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> UpdateJSCRevAssetDocToDMS(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
        Task<CommandResult<ServiceTemplateViewModel>> UpdateJSCRevAssetAllotmentDocToDMS(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
