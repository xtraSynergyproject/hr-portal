
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
    public class ServiceGeneralEGOVPreScript : IServiceGeneralEGOVPreScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>    
        public async Task<CommandResult<ServiceTemplateViewModel>> ValidateNewElectricityConnection(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var categoryId = await _lovBusiness.GetSingle(x => x.Code == "INDUSTRIAL");
            var ConnectionCategoryId = Convert.ToString(rowData["ConnectionCategoryId"]);
            if (ConnectionCategoryId== categoryId.Id) 
            {
                var powerAvailabilityCertificate = Convert.ToString(rowData["PowerAvailabilityCertificate"]);
                if (powerAvailabilityCertificate.IsNullOrEmpty())
                {
                    Dictionary<string, string> message = new Dictionary<string, string>();
                    message.Add("PowerCertificate", "Power Availability Certificate is required");
                    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, message);
                }
            }          
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateComplaintTitleInGrievanceService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {           
            if(viewModel.ServiceStatusCode== "SERVICE_STATUS_DRAFT" || viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || viewModel.ServiceStatusCode == "SERVICE_STATUS_OVERDUE")
            {
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                var ComplaintTilte = Convert.ToString(rowData["ComplaintTitle"]);
                if (ComplaintTilte.IsNotNullAndNotEmpty())
                {
                    viewModel.ServiceSubject = ComplaintTilte;
                }
            }
            
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


    }
}
