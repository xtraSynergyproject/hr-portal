
using Synergy.App.Business.Interface.BusinessScript.Service.General.PerformanceManagement;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.PerformanceManagement
{
    public class ServiceGeneralPerformanceManagementPreScript : IServiceGeneralPerformanceManagementPreScript
    {      
        /// <summary>
        /// Stop all action of service for Freezed Perofrmance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> FreezePerformanceDocumentService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var data = await _pmsBusiness.GetPerformanceDocumentMasterByServiceId(viewModel.ServiceId);
            if (data != null)
            {
                if (data.DocumentStatus == PerformanceDocumentStatusEnum.Freezed)
                {
                    var errorList = new Dictionary<string, string>();
                    errorList.Add("Freeze", "The Performance Document Master is Freezed.");
                    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
