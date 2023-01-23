
using CMS.Business.Interface.BusinessScript.Service.General.EGOV;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.EGOV
{
    public class ServiceGeneralEGOVPostScript : IServiceGeneralEGOVPostScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>    
       
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateBinBookingDetails(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                //var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var ComplaintTilte = Convert.ToString(rowData["ComplaintTitle"]);
                //if (ComplaintTilte.IsNotNullAndNotEmpty())
                //{
                //    viewModel.ServiceSubject = ComplaintTilte;
                //}
                
                await _egovBusiness.UpdateBinBookingDetails(udf);
            }
            
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateRentalPropertyStatus(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
                        
            var model = new TaskTemplateViewModel()
            {
               TemplateCode = viewModel.TemplateCode,
               ServiceStatusCode = viewModel.ServiceStatusCode
            };

            await _egovBusiness.UpdateRentalStatus(model, udf);                     

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


    }
}
