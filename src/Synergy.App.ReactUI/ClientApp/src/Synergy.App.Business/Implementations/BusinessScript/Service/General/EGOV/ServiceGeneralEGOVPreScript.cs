
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
        public async Task<CommandResult<ServiceTemplateViewModel>> ValidateStartEndDate(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<ISmartCityBusiness>();
            var res = await _business.ValidateStartDateandEndDate(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Hoarding is Already Booked for these Dates");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> SetRevenueTypeValueForBinBooking(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _cmsBusiness = sp.GetService<ICmsBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var bookingfrom = Convert.ToDateTime(rowdata["BookingFromDate"]);
            var bookingto = Convert.ToDateTime(rowdata["BookingToDate"]);
            if (bookingfrom< System.DateTime.Today.Date)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Bin booking from date should be greater than or equal to todays date");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            if (bookingfrom > bookingto)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Bin Booking from date should be less than or equal to bin booking to date");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            var where = $@" and ""F_JSC_REV_REVENUE_TYPE"".""Code""='GARBAGE_COLLECTION' ";
            var revenueType = await _cmsBusiness.GetDataListByTemplate("JSC_REVENUE_TYPE", "", where);

            if (revenueType.IsNotNull())
            {                
                foreach (DataRow data in revenueType.Rows)
                {
                    rowdata["RevenueTypeId"] = data["Id"];
                }
            }
            if (bookingto.IsNotNull())
            {
                viewModel.DueDate = bookingto.AddDays(1);
            }
            viewModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowdata);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> GenerateServiceNoGrievanceService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _ntsBusiness = sp.GetService<INtsQueryBusiness>();
            var _smartBusiness = sp.GetService<ISmartCityQueryBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {                
                if (viewModel.ServiceNo.IsNullOrEmpty())
                {
                    var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                    var department = Convert.ToString(rowData["Department"]);
                    var ward = Convert.ToString(rowData["Ward"]==null?"00": rowData["Ward"]);
                    var dep = await _smartBusiness.GetDepartmentById(department);
                    var date = DateTime.Today;
                    //var FirstDay = new DateTime(date.Year, 1, 1);
                    var prefix = "JMC";
                    var seq =await _ntsBusiness.GetNextGrievanceServiceNo(date.Year, department,ward);
                    var sequence = seq.PadLeft(6,'0');
                    var serviceno =$"{prefix}{sequence}";
                    viewModel.ServiceNo = serviceno;
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateWorkflowLevel(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _smartCityBusiness = sp.GetService<ISmartCityBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);                       

            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                var ward = Convert.ToString(rowData["Ward"]);
                var dept = Convert.ToString(rowData["Department"]);

                if (ward.IsNotNull() && dept.IsNotNull())
                {
                    var data = await _smartCityBusiness.GetGrievanceWorkflow(ward, dept);

                    if (data.IsNotNull() && data.WorkflowLevelId.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingleById(data.WorkflowLevelId);

                        switch (lov.Code)
                        {
                            case "GR_WK_LEVEL1": rowData["WorkflowLevel"] = "1"; break;
                            case "GR_WK_LEVEL2": rowData["WorkflowLevel"] = "2"; break;
                            case "GR_WK_LEVEL3": rowData["WorkflowLevel"] = "3"; break;
                            case "GR_WK_LEVEL4": rowData["WorkflowLevel"] = "4"; break;
                            default: rowData["WorkflowLevel"] = "1"; break;
                        }
                        viewModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    }                    
                }
            }
                
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

    }
}
