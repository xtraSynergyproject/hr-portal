using CMS.Business.Interface.BusinessScript.Service.General.CoreHR;
using CMS.Common;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.CoreHR
{
    public class ServiceGeneralCoreHRPreScript : IServiceGeneralCoreHRPreScript
    {
        public async Task<CommandResult<ServiceTemplateViewModel>> ValidateLeaveStartEndDate(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            var res = await _business.ValidateLeaveStartDateandEndDate(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Leave is Already Applied for these Dates");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> ValidatePersonEmailId(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _userBusiness = sp.GetService<IUserBusiness>();
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var email = Convert.ToString(rowData.GetValueOrDefault("EmailId"));

            var userData = await _userBusiness.ValidateUser(email);
            if (userData!=null)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Person already exist with given email.");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary> ValidateBusinessTrip </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> ValidateBusinessTrip(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var startDate = Convert.ToDateTime(udf.BusinessTripStartDate);
            var endDate = Convert.ToDateTime(udf.BusinessTripEndDate);
            if (!(startDate<=endDate))
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "The start date can not be greater than the end date");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateInitialAmountForCommunityHall(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var totalCharges = rowData.GetValueOrDefault("TotalCharges");
            rowData["InitialAmount"] = totalCharges;
            viewModel.Json = JsonConvert.SerializeObject(rowData);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);

        }
        /// <summary> ValidateBusinessTripClaim </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> ValidateBusinessTripClaim(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var errorList = new Dictionary<string, string>();
            
            double perDiemCost = udf.PerDiemCost!=null? Convert.ToDouble(udf.PerDiemCost) : 0.00;
            double ticketCost = udf.TicketCost != null ? Convert.ToDouble(udf.TicketCost) : 0.00;
            double hotelReservationCost = udf.HotelReservationCost != null ? Convert.ToDouble(udf.HotelReservationCost) : 0.00;
            double otherExpenses = udf.OtherExpenses != null ? Convert.ToDouble(udf.OtherExpenses) : 0.00;
            double totalClaimAmount = udf.TotalClaimAmount != null ? Convert.ToDouble(udf.TotalClaimAmount) : 0.00;

            if (totalClaimAmount != (perDiemCost + ticketCost + hotelReservationCost + otherExpenses))
            {
                errorList.Add("BusinessTripClaim", "Please check the Total Claim Amount.");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
