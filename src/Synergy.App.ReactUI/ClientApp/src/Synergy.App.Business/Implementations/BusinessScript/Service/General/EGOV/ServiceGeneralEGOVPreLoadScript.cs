
using Synergy.App.Business.Interface.BusinessScript.Service.General.EGOV;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.EGOV
{
    public class ServiceGeneralEGOVPreLoadScript : IServiceGeneralEGOVPreLoadScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>    
        public async Task<CommandResult<ServiceTemplateViewModel>> FillSanitationAmount(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var deberiscost = await _egovBusiness.GetEGovDebrisRateData();
            var poultrycost = await _egovBusiness.GetEGovPoultryCostData();
            var septictankcost = await _egovBusiness.GetEGovSepticTankCostData();

            //rowData["BillAmount"] = 1000;
            //var categoryId = await _lovBusiness.GetSingle(x => x.Code == "INDUSTRIAL");
            //var ConnectionCategoryId = Convert.ToString(rowData["ConnectionCategoryId"]);
            //if (ConnectionCategoryId== categoryId.Id) 
            //{
            //    var powerAvailabilityCertificate = Convert.ToString(rowData["PowerAvailabilityCertificate"]);
            //    if (powerAvailabilityCertificate.IsNullOrEmpty())
            //    {
            //        Dictionary<string, string> message = new Dictionary<string, string>();
            //        message.Add("PowerCertificate", "Power Availability Certificate is required");
            //        return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, message);
            //    }
            //}          
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

    }
}
