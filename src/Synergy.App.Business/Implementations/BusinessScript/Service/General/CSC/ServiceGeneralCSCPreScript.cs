
using Synergy.App.Business.Interface.BusinessScript.Service.General.CSC;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.CSC
{
    public class ServiceGeneralCSCPreScript : IServiceGeneralCSCPreScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>    
        
        public async Task<CommandResult<ServiceTemplateViewModel>> ChangeOwnerofBirthCertificateSErvice(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {           
            if(viewModel.ServiceStatusCode== "SERVICE_STATUS_DRAFT" || viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" )
            {
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                var ApplicatId = Convert.ToString(rowData["ApplicantName"]);
                if (ApplicatId.IsNotNullAndNotEmpty())
                {
                    viewModel.OwnerUserId = ApplicatId;
                }
            }
            
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> ChangeOwnerofOBCCertificateService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                var _userBusiness = sp.GetService<IUserBusiness>();
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                var ApplicantId = Convert.ToString(rowData["ApplicantId"]);
                if (ApplicantId.IsNullOrEmpty())
                {
                   var ApplicantName= rowData["ApplicantName"]!=null?Convert.ToString(rowData["ApplicantName"]):"";
                    var Mobile = rowData["MobileNo"] != null ? Convert.ToString(rowData["MobileNo"]) : "";
                    var Email = rowData["Email"] != null ? Convert.ToString(rowData["Email"]) : "";
                    var userData = await _userBusiness.ValidateUser(Email);
                    if (userData != null)
                    {
                        viewModel.OwnerUserId = userData.Id;
                        rowData["ApplicantId"] = userData.Id;
                    }
                    else
                    {
                        // Create User
                        var userModel = new UserViewModel();
                        userModel.DataAction = DataActionEnum.Create;
                        userModel.Email = Email;
                        userModel.Name = ApplicantName;
                       

                        var userResult = await _userBusiness.Create(userModel);
                        viewModel.OwnerUserId = userResult.Item.Id;
                    }
                }
                else 
                {
                    viewModel.OwnerUserId = ApplicantId;
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

    }
}
